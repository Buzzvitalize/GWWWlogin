local win = UIAPI:GetElement("FirstWin");
--����
function NpcLevelSeal_SetUI(Type,Index)
	FirstWin_ButtonA1:Visible(true);
	FirstWin_ButtonA2:Visible(true);
	win:Visible(true);

end


function NpcLevelSeal_SetText(Type,Index,BtnID,SubID)
	if Index == 1 then
		if SubID == 101 then
			FirstWin_Text1:SetText(Seal1);
			FirstWin_Text1:Visible(true);
		elseif SubID == 102 then
			local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,135);
			Button:SetText(Seal2);
			Button:Visible(true);
		elseif SubID == 103 then
			local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,160);
			Button:SetText(Seal3);
			Button:Visible(true);
		end
	elseif Index== 2 then    ----�ڼ���ҳ��------
		if SubID == 104 then
			FirstWin_Text1:SetText(Seal4);
			FirstWin_Text1:Visible(true);
		elseif SubID == 105 then
			FirstWin_Text1:SetText(Seal5);
			FirstWin_Text1:Visible(true);
		elseif SubID == 106 then
			FirstWin_Text1:SetText(Seal6);
			FirstWin_Text1:Visible(true);
		elseif SubID == 107 then
			FirstWin_Text1:SetText(Seal7);
			FirstWin_Text1:Visible(true);
		elseif SubID == 108 then
			FirstWin_Text1:SetText(Seal8);
			FirstWin_Text1:Visible(true);
		elseif SubID == 109 then
			FirstWin_Text1:SetText(Seal9);
			FirstWin_Text1:Visible(true);
		end
	end;
end