local win = UIAPI:GetElement("FirstWin");
--����
function NpcFunMoon2_SetUI(Type,Index)
	FirstWin_ButtonA1:Visible(true);
	FirstWin_ButtonA2:Visible(true);
	win:Visible(true);

end
function NpcFunSummoner_SetText(Type,Index,BtnID,SubID)
	if Index == 1 then---��һ��ҳ��
		if SubID == 100 then
			FirstWin_Text1:SetText(SU001);
			FirstWin_Text1:Visible(true);
			local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetText(SU002);
			Button:SetPosition(25,135)
			Button:Visible(true);
		elseif SubID == 101 then
			local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetText(SU001);
			Button:SetPosition(25,155)
			Button:Visible(true);
		end;
	elseif Index == 2 then    ----�ڶ���ҳ��
		if SubID == 201 then
			FirstWin_Text1:SetText(SU003);
			FirstWin_Text1:Visible(true);
			NPCFUN:EndMessage(true);
		elseif SubID == 202 then
			FirstWin_Text1:SetText(SU004);
			FirstWin_Text1:Visible(true);
			NPCFUN:EndMessage(true);
		elseif SubID == 203 then
			FirstWin_Text1:SetText(SU005);
			FirstWin_Text1:Visible(true);
		end;
	end;
end