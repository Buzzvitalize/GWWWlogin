from __future__ import annotations

import os
from typing import Any

import pyodbc
from dotenv import load_dotenv
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field

load_dotenv()

DB_CONNECTION_STRING = os.getenv("DB_CONNECTION_STRING", "")

app = FastAPI(title="Eterna Guerra Login Server", version="1.0.0")


class LoginRequest(BaseModel):
    username: str = Field(min_length=3, max_length=64)
    password: str = Field(min_length=3, max_length=256)
    region: int = 11
    locale: str = "en_us"
    client_version: str = "gw_setup_2.48.001_usa"


class LoginResponse(BaseModel):
    access_token: str
    refresh_token: str
    expires_in: int
    player_id: int
    nickname: str
    faction: str


def get_conn() -> pyodbc.Connection:
    if not DB_CONNECTION_STRING:
        raise RuntimeError("DB_CONNECTION_STRING no configurado")
    return pyodbc.connect(DB_CONNECTION_STRING, timeout=5)


def map_row(cursor: pyodbc.Cursor) -> dict[str, Any]:
    row = cursor.fetchone()
    if row is None:
        raise HTTPException(status_code=500, detail="AUTH_RESULT_EMPTY")
    cols = [c[0] for c in cursor.description]
    return dict(zip(cols, row))


@app.get("/health")
def health() -> dict[str, str]:
    return {"status": "ok"}


@app.post("/auth/login", response_model=LoginResponse)
def login(payload: LoginRequest) -> LoginResponse:
    try:
        with get_conn() as conn:
            cursor = conn.cursor()
            cursor.execute(
                "EXEC dbo.usp_Auth_Login @Username=?, @Password=?, @Region=?, @Locale=?, @ClientVersion=?",
                payload.username,
                payload.password,
                payload.region,
                payload.locale,
                payload.client_version,
            )
            result = map_row(cursor)
    except pyodbc.Error as exc:
        raise HTTPException(status_code=500, detail=f"SQL_ERROR: {exc}") from exc
    except RuntimeError as exc:
        raise HTTPException(status_code=500, detail=str(exc)) from exc

    if not result.get("IsSuccess"):
        raise HTTPException(status_code=401, detail=result.get("ErrorCode", "INVALID_CREDENTIALS"))

    return LoginResponse(
        access_token=result["AccessToken"],
        refresh_token=result["RefreshToken"],
        expires_in=int(result["ExpiresIn"]),
        player_id=int(result["PlayerId"]),
        nickname=result["Nickname"],
        faction=result["Faction"],
    )
