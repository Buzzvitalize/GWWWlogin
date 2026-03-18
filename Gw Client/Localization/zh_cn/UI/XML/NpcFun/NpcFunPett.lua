local win = UIAPI:GetElement("FirstWin");
--活动
function NpcFunPett_SetUI(Type,Index)
	FirstWin_ButtonA1:Visible(true);
	FirstWin_ButtonA2:Visible(true);
	win:Visible(true);

end

function NpcFunPett_SetText(Type,Index,BtnID,SubID)
	if Index== 1 then    ----第几个页面------
		if SubID == 100 then
			FirstWin_Text1:SetText(NF_L0_P001);
			FirstWin_Text1:Visible(true);
			print(BtnID,SubID);
			local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetText(NF_L0_P100);
			Button:SetPosition(25,80)
			Button:Visible(true);
		elseif SubID == 101 then
			print(BtnID,SubID);
			local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetText(NG_L0_P100);
			Button:SetPosition(25,110)
			Button:Visible(true);
		end;
	elseif Index == 2 then	--服务器提示消息---------------------
		if SubID == 110 then
			local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,380);
			Button:SetText("");
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif SubID == 111 then
			FirstWin_Text1:SetText(NF_L0_P102);
			FirstWin_Text1:Visible(true);
		elseif SubID == 112 then
			FirstWin_Text1:SetText(NG_L0_P102);
			FirstWin_Text1:Visible(true);
		elseif SubID == 118 then
			FirstWin_Text1:SetText(NF_L0_P111);
			FirstWin_Text1:Visible(true);
			NPCFUN:EndMessage(true);
		elseif SubID == 119 then
			FirstWin_Text1:SetText(NF_L0_S009);
			FirstWin_Text1:Visible(true);
			NPCFUN:EndMessage(true);
		elseif math.mod(SubID ,100) == 1 then
			FirstWin_Text2:SetText(NF_L0_P103 .. "|cFFF79709" .. string.format("%.2f", (SubID - 1)/10000) .. "|cFFFFFFFF");
			FirstWin_Text2:Visible(true);
			FirstWin_Text2:SetPosition(25,155);
		elseif math.mod(SubID ,100) == 14 then
			FirstWin_Text2:SetText(NG_L0_P103 .. "|cFFF79709" .. string.format("%.2f", (SubID - 14)/10000) .. "|cFFFFFFFF");
			FirstWin_Text2:Visible(true);
			FirstWin_Text2:SetPosition(25,155);
		elseif SubID == 116 then
			print(BtnID,SubID);
			local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetText(NF_L0_P124);
			Button:SetPosition(25,180)
			Button:Visible(true);
		elseif SubID == 117 then
			print(BtnID,SubID);
			local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetText(NG_L0_P112);
			Button:SetPosition(25,180)
			Button:Visible(true);

		end;
	elseif Index >= 3 then	--服务器提示消息---------------------
		if SubID == 120 then
			FirstWin_Text1:SetText(NF_L0_P104);
			FirstWin_Text1:Visible(true);
			FirstWin_ButtonA3:Visible(true);
			FirstWin_ButtonA3:SetText(NF_L0_P124);
			FirstWin_ButtonA3:SetPosition(400,100);
		elseif SubID == 129 then
			FirstWin_Text1:SetText(NF_L0_S009);
			FirstWin_Text1:Visible(true);
			NPCFUN:EndMessage(true);
		elseif SubID == 126 then
			FirstWin_Text1:SetText(NG_L0_P112);
			FirstWin_Text1:Visible(true);
			NPCFUN:EndMessage(true);
		elseif SubID == 127 then
			FirstWin_Text1:SetText(NF_L0_P112);
			FirstWin_Text1:Visible(true);
			NPCFUN:EndMessage(true);
		elseif SubID == 128 then
			FirstWin_Text1:SetText(NF_L0_P111);
			FirstWin_Text1:Visible(true);
			NPCFUN:EndMessage(true);
		elseif math.mod(SubID ,100) == 2 then
                        local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,80);
			Button:SetText(NF_L0_P105 .. "|cFFF79709" .. string.format("%.2f", (SubID - 2)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 3 then
                        local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,100);
			Button:SetText(NF_L0_P106 .. "|cFFF79709" .. string.format("%.2f", (SubID - 3)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 4 then
                        local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,120);
			Button:SetText(NF_L0_P107 .. "|cFFF79709" .. string.format("%.2f", (SubID - 4)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 5 then
                        local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,140);
			Button:SetText(NF_L0_P108 .. "|cFFF79709" .. string.format("%.2f", (SubID - 5)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 6 then
                        local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,160);
			Button:SetText(NF_L0_P109 .. "|cFFF79709" .. string.format("%.2f", (SubID - 6)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 7 then
                        local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,180);
			Button:SetText(NF_L0_P110 .. "|cFFF79709" .. string.format("%.2f", (SubID - 7)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif SubID == 130 then
			FirstWin_Text1:SetText(NG_L0_P104);
			FirstWin_Text1:Visible(true);	
			FirstWin_ButtonA3:Visible(true);
			FirstWin_ButtonA3:SetText(NG_L0_P112);
			FirstWin_ButtonA3:SetPosition(400,100);
		elseif math.mod(SubID ,100) == 8 then
						local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,80);
			Button:SetText(NG_L0_P105 .. "|cFFF79709" .. string.format("%.2f", (SubID - 8)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 9 then
						local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,100);
			Button:SetText(NG_L0_P106 .. "|cFFF79709" .. string.format("%.2f", (SubID - 9)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 10 then
						local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,120);
			Button:SetText(NG_L0_P107 .. "|cFFF79709" .. string.format("%.2f", (SubID - 10)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 11 then
						local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,140);
			Button:SetText(NG_L0_P108 .. "|cFFF79709" .. string.format("%.2f", (SubID - 11)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 12 then
						local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,160);
			Button:SetText(NG_L0_P109 .. "|cFFF79709" .. string.format("%.2f", (SubID - 12)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 13 then
						local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,180);
			Button:SetText(NG_L0_P110 .. "|cFFF79709" .. string.format("%.2f", (SubID - 13)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif SubID == 140 then
			FirstWin_Text1:SetText(NG_L0_P204);
			FirstWin_Text1:Visible(true);	
			NPCFUN:EndMessage(true);
		elseif math.mod(SubID ,100) == 14 then
						local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,80);
			Button:SetText(NG_L0_P205 .. "|cFFF79709" .. string.format("%.2f", (SubID - 8)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 15 then
						local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,100);
			Button:SetText(NG_L0_P206 .. "|cFFF79709" .. string.format("%.2f", (SubID - 9)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 16 then
						local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,120);
			Button:SetText(NG_L0_P207 .. "|cFFF79709" .. string.format("%.2f", (SubID - 10)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 17 then
						local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,140);
			Button:SetText(NG_L0_P208 .. "|cFFF79709" .. string.format("%.2f", (SubID - 11)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 18 then
						local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,160);
			Button:SetText(NG_L0_P209 .. "|cFFF79709" .. string.format("%.2f", (SubID - 12)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		elseif math.mod(SubID ,100) == 19 then
						local Button = win:GetChild("FirstWin_Button" .. BtnID);
			Button:SetPosition(25,180);
			Button:SetText(NG_L0_P210 .. "|cFFF79709" .. string.format("%.2f", (SubID - 13)/10000 ) .. "|cFFFFFFFF");
			Button:Visible(true);
			Button:Enable(false);
			UIAPI:SetChecked(false,Button);
		end;
	end;
end
