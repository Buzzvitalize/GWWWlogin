from __future__ import annotations

import json
import os
import socket
from pathlib import Path
from typing import Any

import pyodbc
from dotenv import load_dotenv
from fastapi import FastAPI, Header, HTTPException
from pydantic import BaseModel, Field

load_dotenv()

DB_CONNECTION_STRING = os.getenv("DB_CONNECTION_STRING", "").strip()
DB_DRIVER = os.getenv("DB_DRIVER", "ODBC Driver 18 for SQL Server")
DB_SERVER = os.getenv("DB_SERVER", r"localhost\SQLEXPRESS")
DB_NAME = os.getenv("DB_NAME", "EternaGuerraAuth")
DB_AUTH_MODE = os.getenv("DB_AUTH_MODE", "trusted").lower().strip()  # trusted | sql
DB_USER = os.getenv("DB_USER", r"ECOSEA-DO\SQLEXPRESS")
DB_PASSWORD = os.getenv("DB_PASSWORD", "")

WORLD_IP = os.getenv("WORLD_IP", "127.0.0.1")
WORLD_PORT = int(os.getenv("WORLD_PORT", "5999"))

app = FastAPI(title="Eterna Guerra Login/Game API", version="2.2.0")

BUNDLE_FILE = Path(__file__).resolve().parent / "assets" / "en_us_login_bundle.json"


def load_login_bundle() -> dict[str, Any]:
    if not BUNDLE_FILE.exists():
        return {"locale": "en_us", "login_copy": {}, "error_messages": {}}
    return json.loads(BUNDLE_FILE.read_text(encoding="utf-8"))



class RegisterRequest(BaseModel):
    username: str = Field(min_length=3, max_length=64)
    password: str = Field(min_length=6, max_length=256)
    nickname: str = Field(min_length=3, max_length=64)
    region: int = 11
    locale: str = "en_us"


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


class CharacterCreateRequest(BaseModel):
    character_name: str = Field(min_length=3, max_length=24)
    class_code: str = Field(min_length=3, max_length=16)
    faction: str = Field(default="Athens", min_length=3, max_length=16)


class CharacterResponse(BaseModel):
    character_id: int
    character_name: str
    class_code: str
    faction: str
    level: int
    map_code: str


class EnterWorldResponse(BaseModel):
    world_ip: str
    world_port: int
    character_id: int
    character_name: str
    map_code: str


def build_connection_string() -> str:
    if DB_CONNECTION_STRING:
        return DB_CONNECTION_STRING

    base = (
        f"DRIVER={{{DB_DRIVER}}};"
        f"SERVER={DB_SERVER};"
        f"DATABASE={DB_NAME};"
        "Encrypt=no;TrustServerCertificate=yes;"
    )

    if DB_AUTH_MODE == "sql":
        return f"{base}UID={DB_USER};PWD={DB_PASSWORD};"

    return f"{base}Trusted_Connection=yes;"


def get_conn() -> pyodbc.Connection:
    conn_str = build_connection_string()
    return pyodbc.connect(conn_str, timeout=5)


def fetch_one_dict(cursor: pyodbc.Cursor) -> dict[str, Any]:
    row = cursor.fetchone()
    if row is None:
        raise HTTPException(status_code=500, detail="AUTH_RESULT_EMPTY")
    cols = [c[0] for c in cursor.description]
    return dict(zip(cols, row))


def extract_token(authorization: str | None) -> str:
    if not authorization:
        raise HTTPException(status_code=401, detail="MISSING_AUTHORIZATION")
    parts = authorization.split(" ", 1)
    if len(parts) != 2 or parts[0].lower() != "bearer":
        raise HTTPException(status_code=401, detail="INVALID_AUTHORIZATION_FORMAT")
    return parts[1].strip()


def access_token_to_guid(access_token: str) -> str:
    if len(access_token) != 36:
        raise HTTPException(status_code=401, detail="INVALID_TOKEN")
    return access_token


@app.get("/health")
def health() -> dict[str, str]:
    return {"status": "ok"}


@app.get("/db/config")
def db_config() -> dict[str, str]:
    return {
        "server": DB_SERVER,
        "database": DB_NAME,
        "auth_mode": DB_AUTH_MODE,
    }


def is_world_online(host: str, port: int, timeout_s: float = 1.0) -> bool:
    try:
        with socket.create_connection((host, port), timeout=timeout_s):
            return True
    except OSError:
        return False




@app.get("/localization/en-us/login-bundle")
def localization_login_bundle() -> dict[str, Any]:
    return load_login_bundle()

@app.get("/emulator/status")
def emulator_status() -> dict[str, str | int | bool]:
    online = is_world_online(WORLD_IP, WORLD_PORT)
    return {
        "world_ip": WORLD_IP,
        "world_port": WORLD_PORT,
        "online": online,
        "message": "Sistema Online" if online else "Sistema Offline",
    }


@app.post("/auth/register")
def register(payload: RegisterRequest) -> dict[str, str]:
    try:
        with get_conn() as conn:
            cursor = conn.cursor()
            cursor.execute(
                "EXEC dbo.usp_Auth_Register @Username=?, @Password=?, @Nickname=?, @Region=?, @Locale=?",
                payload.username,
                payload.password,
                payload.nickname,
                payload.region,
                payload.locale,
            )
            result = fetch_one_dict(cursor)
    except pyodbc.Error as exc:
        raise HTTPException(status_code=500, detail=f"SQL_ERROR: {exc}") from exc

    if not result.get("IsSuccess"):
        raise HTTPException(status_code=400, detail=result.get("ErrorCode", "REGISTER_FAILED"))

    return {"status": "created"}


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
            result = fetch_one_dict(cursor)
    except pyodbc.Error as exc:
        raise HTTPException(status_code=500, detail=f"SQL_ERROR: {exc}") from exc

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


@app.get("/characters", response_model=list[CharacterResponse])
def list_characters(authorization: str | None = Header(default=None)) -> list[CharacterResponse]:
    access_token = access_token_to_guid(extract_token(authorization))

    sql = """
    SELECT c.CharacterId, c.CharacterName, c.ClassCode, c.Faction, c.Level, c.MapCode
    FROM dbo.AuthSession s
    INNER JOIN dbo.PlayerCharacter c ON c.PlayerId = s.PlayerId AND c.IsDeleted = 0
    WHERE s.AccessToken = ? AND s.ExpiresAt > SYSUTCDATETIME() AND s.RevokedAt IS NULL
    ORDER BY c.CharacterId;
    """

    try:
        with get_conn() as conn:
            cursor = conn.cursor()
            rows = cursor.execute(sql, access_token).fetchall()
    except pyodbc.Error as exc:
        raise HTTPException(status_code=500, detail=f"SQL_ERROR: {exc}") from exc

    return [
        CharacterResponse(
            character_id=int(r[0]),
            character_name=r[1],
            class_code=r[2],
            faction=r[3],
            level=int(r[4]),
            map_code=r[5],
        )
        for r in rows
    ]


@app.post("/characters", response_model=CharacterResponse)
def create_character(payload: CharacterCreateRequest, authorization: str | None = Header(default=None)) -> CharacterResponse:
    access_token = access_token_to_guid(extract_token(authorization))

    try:
        with get_conn() as conn:
            cursor = conn.cursor()
            cursor.execute(
                "EXEC dbo.usp_Character_Create @AccessToken=?, @CharacterName=?, @ClassCode=?, @Faction=?",
                access_token,
                payload.character_name,
                payload.class_code,
                payload.faction,
            )
            result = fetch_one_dict(cursor)
    except pyodbc.Error as exc:
        raise HTTPException(status_code=500, detail=f"SQL_ERROR: {exc}") from exc

    if not result.get("IsSuccess"):
        raise HTTPException(status_code=400, detail=result.get("ErrorCode", "CHARACTER_CREATE_FAILED"))

    return CharacterResponse(
        character_id=int(result["CharacterId"]),
        character_name=result["CharacterName"],
        class_code=result["ClassCode"],
        faction=result["Faction"],
        level=int(result["Level"]),
        map_code=result["MapCode"],
    )


@app.post("/characters/{character_id}/select")
def select_character(character_id: int, authorization: str | None = Header(default=None)) -> dict[str, str]:
    access_token = access_token_to_guid(extract_token(authorization))

    try:
        with get_conn() as conn:
            cursor = conn.cursor()
            cursor.execute(
                "EXEC dbo.usp_Character_Select @AccessToken=?, @CharacterId=?",
                access_token,
                character_id,
            )
            result = fetch_one_dict(cursor)
    except pyodbc.Error as exc:
        raise HTTPException(status_code=500, detail=f"SQL_ERROR: {exc}") from exc

    if not result.get("IsSuccess"):
        raise HTTPException(status_code=400, detail=result.get("ErrorCode", "CHARACTER_SELECT_FAILED"))

    return {"status": "selected"}


@app.post("/session/enter-world", response_model=EnterWorldResponse)
def enter_world(authorization: str | None = Header(default=None)) -> EnterWorldResponse:
    access_token = access_token_to_guid(extract_token(authorization))

    sql = """
    SELECT TOP 1 c.CharacterId, c.CharacterName, c.MapCode
    FROM dbo.AuthSession s
    INNER JOIN dbo.PlayerCharacter c ON c.CharacterId = s.ActiveCharacterId
    WHERE s.AccessToken = ?
      AND s.ExpiresAt > SYSUTCDATETIME()
      AND s.RevokedAt IS NULL
      AND c.IsDeleted = 0;
    """

    try:
        with get_conn() as conn:
            cursor = conn.cursor()
            row = cursor.execute(sql, access_token).fetchone()
    except pyodbc.Error as exc:
        raise HTTPException(status_code=500, detail=f"SQL_ERROR: {exc}") from exc

    if row is None:
        raise HTTPException(status_code=400, detail="NO_ACTIVE_CHARACTER")

    if not is_world_online(WORLD_IP, WORLD_PORT):
        raise HTTPException(status_code=503, detail="WORLD_EMULATOR_OFFLINE")

    return EnterWorldResponse(
        world_ip=WORLD_IP,
        world_port=WORLD_PORT,
        character_id=int(row[0]),
        character_name=row[1],
        map_code=row[2],
    )
