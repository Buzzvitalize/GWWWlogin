local win = UIAPI:GetElement("FirstWin");
--�������
function NpcFunPetMaster_SetUI(Type,Index)
	FirstWin_ButtonA1:SetPosition(145,500);
	FirstWin_ButtonA2:SetPosition(220,500);
	FirstWin_ButtonA1:Visible(true);
	FirstWin_ButtonA2:Visible(true);
	win:Visible(true);

end

function NpcFunPetMaster_SetText(Type,Index,BtnID,SubID)
	if Index == 1 then
	   if SubID == 101 then
	      FirstWin_Text1:SetText(PL0004);
	      FirstWin_Text1:Visible(true);
	      NPCFUN:NeedMessage(False);
	      local Button = win:GetChild("FirstWin_Button" .. BtnID);
	      Button:SetText(PL0001);
		  Button:SetPosition(25,100);
	      Button:Visible(true);
	   elseif SubID == 102 then
	      local Button = win:GetChild("FirstWin_Button" .. BtnID);
	      Button:SetText(PL0002);
		  Button:SetPosition(25,120);
	      Button:Visible(true);
		elseif SubID == 103 then
	      local Button = win:GetChild("FirstWin_Button" .. BtnID);
	      Button:SetText(PL0003);
		  Button:SetPosition(25,140);
	      Button:Visible(true);
	   end;
elseif Index == 2 then
   		if SubID == 201 then
   		    FirstWin_Text1:SetText(PL0005);
   		    FirstWin_Text1:Visible(true);
   		    InputText1:Visible(true);
   		    InputText1:SetPosition(60,60);
   		    Input11:Visible(true);
   		    Input11:SetPosition(60,60);
   		    Input12:Visible(true);
   		    Input12:SetPosition(52,60);
   		    Input13:Visible(true);
   		    Input13:SetPosition(154,60);
   		elseif SubID == 202 then
   		    FirstWin_Text1:SetText(PL0006);
   		    FirstWin_Text1:Visible(true);
   		    InputText1:Visible(true);
   		    InputText1:SetPosition(60,60);
   		    Input11:Visible(true);
   		    Input11:SetPosition(60,60);
   		    Input12:Visible(true);
   		    Input12:SetPosition(52,60);
   		    Input13:Visible(true);
   		    Input13:SetPosition(154,60);
   		elseif SubID == 203 then
   		    FirstWin_Text1:SetText(PL0013);
   		    FirstWin_Text1:Visible(true);
   		    InputText1:Visible(true);
   		    InputText1:SetPosition(60,60);
   		    Input11:Visible(true);
   		    Input11:SetPosition(60,60);
   		    Input12:Visible(true);
   		    Input12:SetPosition(52,60);
   		    Input13:Visible(true);
   		    Input13:SetPosition(154,60);
   		end;
	elseif Index >= 3 then
	   if SubID == 310 then
	      FirstWin_Text1:SetText(PL0007);
	      FirstWin_Text1:Visible(true);
	      NPCFUN:EndMessage(true);
	      NPCFUN:NeedMessage(False);
	   elseif SubID == 320 then
	      FirstWin_Text1:SetText(PL0008);
	      FirstWin_Text1:Visible(true);
	      NPCFUN:EndMessage(true);
	      NPCFUN:NeedMessage(False);
	   elseif SubID == 330 then
	      FirstWin_Text1:SetText(PL0009);
	      FirstWin_Text1:Visible(true);
	      NPCFUN:EndMessage(true);
	      NPCFUN:NeedMessage(False);
		elseif SubID == 340 then
	      FirstWin_Text1:SetText(PL0010);
	      FirstWin_Text1:Visible(true);
	      NPCFUN:EndMessage(true);
	      NPCFUN:NeedMessage(False);
		elseif SubID == 350 then
	      FirstWin_Text1:SetText(PL0011);
	      FirstWin_Text1:Visible(true);
	      NPCFUN:EndMessage(true);
	      NPCFUN:NeedMessage(False);
		elseif SubID == 360 then
	      FirstWin_Text1:SetText(PL0012);
	      FirstWin_Text1:Visible(true);
	      NPCFUN:EndMessage(true);
	      NPCFUN:NeedMessage(False);
		elseif SubID == 370 then
	      FirstWin_Text1:SetText(PL0014);
	      FirstWin_Text1:Visible(true);
	      NPCFUN:EndMessage(true);
	      NPCFUN:NeedMessage(False);
		elseif SubID == 400 then
   		  FirstWin_Text1:SetText(PL0005);
   		  FirstWin_Text1:Visible(true);
   		  InputText1:Visible(true);
   		  InputText1:SetPosition(60,60);
   		  Input11:Visible(true);
   		  Input11:SetPosition(60,60);
   		  Input12:Visible(true);
   		  Input12:SetPosition(52,60);
   		  Input13:Visible(true);
   		  Input13:SetPosition(154,60);
           end;
	end;
end
