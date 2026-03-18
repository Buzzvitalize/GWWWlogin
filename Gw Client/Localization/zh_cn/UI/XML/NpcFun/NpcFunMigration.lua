local win = UIAPI:GetElement("FirstWin");
--����
function NpcMigration_SetUI(Type,Index)
	FirstWin_ButtonA1:Visible(true);
	FirstWin_ButtonA2:Visible(true);
	win:Visible(true);

end

function NpcMigration_SetText(Type,Index,BtnID,SubID)
	if Index == 1 then
		if SubID == 104 then
			FirstWin_Text1:SetText(Migration4);
			FirstWin_Text1:Visible(true);
		elseif SubID == 105 then
			local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,135);
			Button:SetText(Migration5);
			Button:Visible(true);
		elseif SubID == 106 then
			local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,135);
			Button:SetText(Migration6);
			Button:Visible(true);
		
		end
	elseif Index== 2 then    ----�ڼ���ҳ��------
		if SubID == 107 then
			FirstWin_Text1:SetText(Migration1);
			FirstWin_Text1:Visible(true);
		elseif SubID == 108 then
			FirstWin_Text1:SetText(Migration2);
			FirstWin_Text1:Visible(true);
		elseif SubID == 109 then
			FirstWin_Text1:SetText(Migration3);
			FirstWin_Text1:Visible(true);
		elseif math.mod(SubID ,100) == 2 then
			FirstWin_Text1:SetText(Migration7..( ( ( ( SubID - 2 ) / 100 ) - ( ( SubID - 2 ) / 100 ) % 60 ) / 60 )..Migration8..( ( ( SubID - 2 ) / 100 ) % 60 )..Migration9);
			FirstWin_Text1:Visible(true);
			NPCFUN:EndMessage(true);
		end
	end;
end
