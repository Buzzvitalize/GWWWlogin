local win = UIAPI:GetElement("FirstWin");
--圣衣铸造
function NpcFunChangeName_SetUI(Type,Index)
	FirstWin_ButtonA1:SetPosition(145,500);
	FirstWin_ButtonA2:SetPosition(220,500);
	FirstWin_ButtonA1:Visible(true);
	FirstWin_ButtonA2:Visible(true);
	win:Visible(true);
end

function NpcFunChangeName_SetText(Type,Index,BtnID,SubID)
	if Index == 1 then
	   if SubID  == 2101 then
		FirstWin_Text1:SetText(NF_L0_ZBJY2101);
		FirstWin_Text1:Visible(true);
		FirstWin_Text1:SetPosition(25,100);
	   else
		FirstWin_Text1:SetText(CN0002);
		FirstWin_Text1:Visible(true);		
	   end
	elseif Index == 2 then --服务器消息提示
	   if SubID  == 106 then
		FirstWin_Text1:SetText(CN0003);
		FirstWin_Text1:Visible(true);
		FirstWin_Text2:SetText(CN0004);
		FirstWin_Text2:Visible(true);
		FirstWin_Text2:SetPosition(25,120);
		FirstWin_Text3:SetText(CN0005);
		FirstWin_Text3:Visible(true);
		FirstWin_Text3:SetPosition(25,185);
		InputText1:Visible(true);
		InputText1:SetPosition(60,160);
		Input11:Visible(true);
		Input11:SetPosition(60,160);
		Input12:Visible(true);
		Input12:SetPosition(52,160);
		Input13:Visible(true);
		Input13:SetPosition(154,160);
	   end;
     elseif Index >= 3 then
	if math.mod(SubID, 10) == 106 then
           FirstWin_Text2:SetText(NF_L0_ZBJY405..((SubID-6)/10));
	   FirstWin_Text2:Visible(true);
	   FirstWin_Text2:SetPosition(43,225);
        end;
	if math.mod(SubID, 10) == 107 then
           FirstWin_Text3:SetText(NF_L0_ZBJY404..((SubID-7)/10));
	   FirstWin_Text3:Visible(true);
	   FirstWin_Text3:SetPosition(43,245);
        end;
      end
        if SubID  == 999 then
	   FirstWin_Text1:SetText(CN0009);
	   FirstWin_Text1:Visible(true);
	   NPCFUN:EndMessage(true);
        end;
		if SubID  == 888 then
			FirstWin_Text1:SetText(CN0008);
			FirstWin_Text1:Visible(true);
			NPCFUN:EndMessage(true);
		end;
		if SubID  == 889 then
			FirstWin_Text1:SetText(CN0007);
			FirstWin_Text1:Visible(true);
			NPCFUN:EndMessage(true);
		end;
	if math.mod(SubID ,100) == 0 then
	   if SubID / 100 == 4 then
	      FirstWin_ButtonA1:Visible(false);
	      FirstWin_ButtonA2:Visible(false);
	      FirstWin_ButtonA3:Visible(true);
              FirstWin_ButtonA3:SetText(CN0006);
	      FirstWin_ButtonA3:SetPosition(400,250);
		  FirstWin_Text1:SetText(CN0003);
		  FirstWin_Text1:Visible(true);
		  FirstWin_Text2:SetText(CN0004);
		  FirstWin_Text2:Visible(true);
		  FirstWin_Text2:SetPosition(25,120);
		  FirstWin_Text3:SetText(CN0005);
		  FirstWin_Text3:Visible(true);
		  FirstWin_Text3:SetPosition(25,185);
		  InputText1:Visible(true);
		  InputText1:SetPosition(60,160);
		  Input11:Visible(true);
		  Input11:SetPosition(60,160);
		  Input12:Visible(true);
		  Input12:SetPosition(52,160);
		  Input13:Visible(true);
		  Input13:SetPosition(154,160);
	   end;
	   FirstWin_Button1:SetTextFormat(5);
	   FirstWin_Button2:SetTextFormat(5);

	   NPCFUN:EndMessage(true);
	elseif math.mod(SubID ,100) == 1 then
	   if ( SubID - 1 ) / 100 == 1 then
	      local Button = win:GetChild("FirstWin_Button" .. BtnID);
	      Button:SetText(CN0001);
	      Button:Visible(true);
	      Button:SetPosition(25,205);
	   end;
	end;
end


