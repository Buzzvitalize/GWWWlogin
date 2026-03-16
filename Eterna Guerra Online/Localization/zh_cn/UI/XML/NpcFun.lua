print("success");

--定义拓展的NPC功能类型
--NPC_FLAG_SYS
local NPC_FLAG_SYS_NONE    = 0
local NPC_FLAG_SYS_TRANMIT = 1  --传送
local NPC_FLAG_SYS_CREDIT_EXCHANGE = 2 --声望
local NPC_FLAG_SYS_GRANT_OFFICAL   = 4 --官职
local NPC_FLAG_SYS_OFFICAL_SALARY  = 8  --俸禄
local NPC_FLAG_SYS_ALTAR = 16  --祭坛
local NPC_FLAG_GUILDQUEST = 32  --公会任务

local NPC_FLAG_LivingSkill = 128
--获取所有UI资源
local win = UIAPI:GetElement("FirstWin");
local FirstWin_Text1= win:GetChild("FirstWin_Text1");
local FirstWin_Text2= win:GetChild("FirstWin_Text2");
local FirstWin_Text3= win:GetChild("FirstWin_Text3");
		
local FirstWin_Button1 = win:GetChild("FirstWin_Button1");
local FirstWin_Button2 = win:GetChild("FirstWin_Button2");
local FirstWin_Button3 = win:GetChild("FirstWin_Button3");
local FirstWin_Button4 = win:GetChild("FirstWin_Button4");
local FirstWin_Button5 = win:GetChild("FirstWin_Button5");
local FirstWin_Button6 = win:GetChild("FirstWin_Button6");
local FirstWin_Button7 = win:GetChild("FirstWin_Button7");
local FirstWin_Button8 = win:GetChild("FirstWin_Button8");
local FirstWin_Button9 = win:GetChild("FirstWin_Button9");
local FirstWin_Button10 = win:GetChild("FirstWin_Button10");
        
local FirstWin_CheckBox1 = win:GetChild("FirstWin_CheckBox1");
local FirstWin_CheckBox2 = win:GetChild("FirstWin_CheckBox2");
local FirstWin_CheckBox3 = win:GetChild("FirstWin_CheckBox3");
local FirstWin_CheckBox4 = win:GetChild("FirstWin_CheckBox4");
		
local FirstWin_ButtonA1 = win:GetChild("FirstWin_ButtonA1");
local FirstWin_ButtonA2 = win:GetChild("FirstWin_ButtonA2");

--显示不同类型的窗体界面
function Set_NpcFun_UI(Type,Index)
        print("Set_NpcFun_UI()");
        print(Type);
	SetOriginalPosition();

	FirstWin_Text1:Visible(false);
	FirstWin_Text2:Visible(false);
	FirstWin_Text3:Visible(false);

	FirstWin_Button1:Visible(false);
	FirstWin_Button2:Visible(false);
	FirstWin_Button3:Visible(false);
	FirstWin_Button4:Visible(false);
	FirstWin_Button5:Visible(false);
	FirstWin_Button6:Visible(false);
	FirstWin_Button7:Visible(false);
	FirstWin_Button8:Visible(false);
	FirstWin_Button9:Visible(false);
	FirstWin_Button10:Visible(false);

	FirstWin_CheckBox1:Visible(false);
	FirstWin_CheckBox2:Visible(false);
	FirstWin_CheckBox3:Visible(false);
	FirstWin_CheckBox4:Visible(false);

	FirstWin_ButtonA1:Visible(false);
	FirstWin_ButtonA2:Visible(false);

    	if Type == NPC_FLAG_SYS_TRANMIT  then
		FirstWin_ButtonA1:Visible(true);
		FirstWin_ButtonA2:Visible(true);
		win:Visible(true);
        elseif Type == NPC_FLAG_SYS_CREDIT_EXCHANGE  then
		FirstWin_ButtonA1:Visible(true);
		FirstWin_ButtonA2:Visible(true);
		win:Visible(true);
    	elseif Type == NPC_FLAG_SYS_GRANT_OFFICAL  then
		FirstWin_ButtonA1:Visible(true);
		FirstWin_ButtonA2:Visible(true);
		win:Visible(true);
    	elseif Type == NPC_FLAG_SYS_OFFICAL_SALARY then
		FirstWin_ButtonA1:Visible(true);
		FirstWin_ButtonA2:Visible(true);
		win:Visible(true);
    	elseif Type == NPC_FLAG_SYS_ALTAR then
                if Index == 1 then
	           FirstWin_Text1:SetPosition(45,100);
	           FirstWin_Text2:SetPosition(45,120);
	           FirstWin_Text3:SetPosition(45,140);

        	   FirstWin_Button1:SetPosition(45,160);
               	   FirstWin_Button2:SetPosition(45,185);
         	   FirstWin_Button3:SetPosition(45,210);
	           FirstWin_Button4:SetPosition(45,235);
	           FirstWin_Button5:SetPosition(45,260);
	           FirstWin_Button6:SetPosition(45,285);
	           FirstWin_Button7:SetPosition(45,310);
	           FirstWin_Button8:SetPosition(45,335);
	           FirstWin_Button9:SetPosition(45,360);
	           FirstWin_Button10:SetPosition(45,385);

	           FirstWin_ButtonA1:SetPosition(145,500);
	           FirstWin_ButtonA2:SetPosition(220,500);
		   FirstWin_ButtonA1:Visible(true);
		   FirstWin_ButtonA2:Visible(true);
		   win:Visible(true);
                end;
                if Index == 2 then
	           FirstWin_Text1:SetPosition(45,100);
	           FirstWin_Text2:SetPosition(45,120);
	           FirstWin_Text3:SetPosition(45,140);

        	   FirstWin_Button1:SetPosition(40,160);
               	   FirstWin_Button2:SetPosition(40,185);
         	   FirstWin_Button3:SetPosition(40,210);
	           FirstWin_Button4:SetPosition(40,235);
	           FirstWin_Button5:SetPosition(40,260);
	           FirstWin_Button6:SetPosition(40,285);
	           FirstWin_Button7:SetPosition(40,310);
	           FirstWin_Button8:SetPosition(40,335);
	           FirstWin_Button9:SetPosition(40,360);
	           FirstWin_Button10:SetPosition(40,385);

	           FirstWin_ButtonA1:SetPosition(220,500);
	           FirstWin_ButtonA2:SetPosition(220,500);
		   FirstWin_ButtonA1:Visible(true);
		   FirstWin_ButtonA2:Visible(false);
		   win:Visible(true);
                end;
                if Index == 3 then
	           FirstWin_Text1:SetPosition(45,100);
	           FirstWin_Text2:SetPosition(45,120);
	           FirstWin_Text3:SetPosition(45,140);

        	   FirstWin_Button1:SetPosition(25,160);
               	   FirstWin_Button2:SetPosition(25,185);
         	   FirstWin_Button3:SetPosition(25,210);
	           FirstWin_Button4:SetPosition(25,235);
	           FirstWin_Button5:SetPosition(25,260);
	           FirstWin_Button6:SetPosition(25,285);
	           FirstWin_Button7:SetPosition(25,310);
	           FirstWin_Button8:SetPosition(25,335);
	           FirstWin_Button9:SetPosition(25,360);
	           FirstWin_Button10:SetPosition(25,385);

	           FirstWin_ButtonA1:SetPosition(220,500);
	           FirstWin_ButtonA2:SetPosition(220,500);
		   FirstWin_ButtonA1:Visible(true);
		   FirstWin_ButtonA2:Visible(false);
		   win:Visible(true);
                end;
    	elseif Type == NPC_FLAG_LivingSkill then
		FirstWin_ButtonA1:Visible(true);
		FirstWin_ButtonA2:Visible(true);
		win:Visible(true);
    	elseif Type == NPC_FLAG_GUILDQUEST then
		FirstWin_ButtonA1:Visible(true);
		FirstWin_ButtonA2:Visible(true);
		win:Visible(true);
    	end;
end

--配置UI窗体信息生成自己想要的界面
function Set_NpcFun_Text(Type,Index,BtnID,SubID)
	if Type == NPC_FLAG_SYS_TRANMIT then    --------传送----------------------------------------------------------------------------------
		if Index == 1 then
			if SubID== 1 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_1);
				Button:Visible(true);
			elseif SubID == 2 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_2);
				Button:Visible(true);
			elseif SubID == 3 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_3);
				Button:Visible(true);
			elseif SubID == 4 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_4);
				Button:Visible(true);
                	elseif SubID == 5 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_5);
				Button:Visible(true);
                	elseif SubID == 6 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_6);
				Button:Visible(true);
                	elseif SubID == 100 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_168);
				Button:Visible(true);
                	elseif SubID == 101 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_169);
				Button:Visible(true);
                	elseif SubID == 102 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_170);
				Button:Visible(true);
                	elseif SubID == 103 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_171);
				Button:Visible(true);
                	elseif SubID == 104 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_172);
				Button:Visible(true);
                	elseif SubID == 105 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_173);
				Button:Visible(true);
                	elseif SubID == 106 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_174);
				Button:Visible(true);
                	elseif SubID == 107 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_175);
				Button:Visible(true);
                	elseif SubID == 108 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_176);
				Button:Visible(true);
                	elseif SubID == 109 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_177);
				Button:Visible(true);
                	elseif SubID == 110 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_178);
				Button:Visible(true);
                	elseif SubID == 111 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_179);
				Button:Visible(true);
                	elseif SubID == 10 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_180);
				Button:Visible(true);
                	elseif SubID == 11 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_181);
				Button:Visible(true);
                	elseif SubID == 12 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_182);
				Button:Visible(true);
                	elseif SubID == 13 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_183);
				Button:Visible(true);
                	elseif SubID == 14 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_184);
				Button:Visible(true);
                	elseif SubID == 15 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_185);
				Button:Visible(true);
                	elseif SubID == 16 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_186);
				Button:Visible(true);
                	elseif SubID == 17 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_187);
				Button:Visible(true);
                	elseif SubID == 18 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_188);
				Button:Visible(true);
                	elseif SubID == 19 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_189);
				Button:Visible(true);
                	elseif SubID == 20 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_190);
				Button:Visible(true);
                	elseif SubID == 21 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_191);
				Button:Visible(true);
                	elseif SubID == 50 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_192);
				Button:Visible(true);
                	elseif SubID == 51 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_193);
				Button:Visible(true);
                	elseif SubID == 52 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_194);
				Button:Visible(true);
                	elseif SubID == 53 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_195);
				Button:Visible(true);
                	elseif SubID == 54 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_196);
				Button:Visible(true);
                	elseif SubID == 55 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_197);
				Button:Visible(true);
                	elseif SubID == 56 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_198);
				Button:Visible(true);
                	elseif SubID == 57 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_199);
				Button:Visible(true);
                	elseif SubID == 58 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_200);
				Button:Visible(true);
                	elseif SubID == 59 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_201);
				Button:Visible(true);
                	elseif SubID == 60 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_202);
				Button:Visible(true);
                	elseif SubID == 61 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_203);
				Button:Visible(true);
                	elseif SubID == 200 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_204);
				Button:Visible(true);
                	elseif SubID == 201 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_205);
				Button:Visible(true);
                	elseif SubID == 202 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_29);
				Button:Visible(true);
                	elseif SubID == 203 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_206);
				Button:Visible(true);
                	elseif SubID == 300 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_30);
				Button:Visible(true);
                	elseif SubID == 301 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_30);
				Button:Visible(true);
			end;
		elseif Index == 2 then
			if SubID == 1000 then
				FirstWin_Text1:SetText(NF_L0_32);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
                                NPCFUN:NeedMessage(False);
			elseif SubID == 1001 then
				FirstWin_Text1:SetText(NF_L0_33);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
                                NPCFUN:NeedMessage(False);
			elseif SubID == 2001 then
				FirstWin_Text1:SetText(NF_L0_34);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
                                NPCFUN:NeedMessage(False);
			elseif SubID == 1009 then
				FirstWin_Text1:SetText(NF_L0_35);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
                                NPCFUN:NeedMessage(False);
			elseif SubID == 1010 then
				FirstWin_Text1:SetText(NF_L0_36);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
                                NPCFUN:NeedMessage(False);
			elseif SubID == 1200 then
				FirstWin_Text1:SetText(NF_L0_37);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
                                NPCFUN:NeedMessage(False);
			elseif SubID == 1111 then
				FirstWin_Text1:SetText(NF_L0_38);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
                                NPCFUN:NeedMessage(False);
			elseif SubID == 1112 then
				FirstWin_Text1:SetText(NF_L0_31);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
                                NPCFUN:NeedMessage(False);



			end;
        	end;
	elseif Type == NPC_FLAG_SYS_CREDIT_EXCHANGE then    --------兑换-----------------------------------------------------------------------
		if Index == 1 then
			if SubID== 1 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_39);
				Button:Visible(true);
			elseif SubID == 2 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_40);
				Button:Visible(true);
			elseif SubID == 3 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_41);
				Button:Visible(true);
			elseif SubID == 4 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_42);
				Button:Visible(true);
			elseif SubID == 5 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_43);
				Button:Visible(true);
			end;
		elseif Index == 2 then --服务器消息提示
			if SubID == 1000 then
				FirstWin_Text1:SetText(NF_L0_44);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
			elseif SubID == 1001 then
				FirstWin_Text1:SetText(NF_L0_45);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
			elseif SubID == 1002 then
				FirstWin_Text1:SetText(NF_L0_46);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
			elseif SubID == 1003 then
				FirstWin_Text1:SetText(NF_L0_47);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
			end;
        	end;
	elseif Type == NPC_FLAG_SYS_GRANT_OFFICAL then    --------国家官职----------------------------------------------------------------------
		if Index == 1 then
			if SubID== 1 then
				print(BtnID,SubID);
				FirstWin_Text1:SetText(NF_L0_48);
				FirstWin_Text1:Visible(true);
			elseif SubID== 2 then
				print(BtnID,SubID);
				FirstWin_Text1:SetText(NF_L0_49);
				FirstWin_Text1:Visible(true);
			elseif SubID== 3 then
				print(BtnID,SubID);
				FirstWin_Text1:SetText(NF_L0_50);
				FirstWin_Text1:Visible(true);
			elseif SubID== 4 then
				print(BtnID,SubID);
				FirstWin_Text1:SetText(NF_L0_51);
				FirstWin_Text1:Visible(true);
			elseif SubID== 5 then
				print(BtnID,SubID);
				FirstWin_Text1:SetText(NF_L0_52);
				FirstWin_Text1:Visible(true);
			elseif SubID== 255 then
				print(BtnID,SubID);
				FirstWin_Text1:SetText(NF_L0_53);
				FirstWin_Text1:Visible(true);
			end;
		elseif Index == 2 then --服务器消息提示
			if SubID == 1000 then
				FirstWin_Text1:SetText(NF_L0_54);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
			elseif SubID == 1001 then
				FirstWin_Text1:SetText(NF_L0_54);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
			end;
        	end;
	elseif Type == NPC_FLAG_SYS_OFFICAL_SALARY then  -------------------领取俸禄--------------------------------------------------------------
		if Index == 1 then
			if SubID== 1 then
				print(BtnID,SubID);
				FirstWin_Text1:SetText(NF_L0_55);
				FirstWin_Text1:Visible(true);
			
				FirstWin_CheckBox1:GetChild(0):SetTexturePos(36,0);
				FirstWin_CheckBox1:Visible(true);

				NPCFUN:SetItemData({Base=4000,BaseLv=1,AppendLv=1,Overlap=1,Binding=0,Append1=-1,Append2=-1,Append3=-1,Append4=-1,Append5=-1});
			elseif SubID== 2 then
				print(BtnID,SubID);
				FirstWin_Text1:SetText(NF_L0_56);
				FirstWin_Text1:Visible(true);
			
				FirstWin_CheckBox1:GetChild(0):SetTexturePos(36,0);
				FirstWin_CheckBox1:Visible(true);

				NPCFUN:SetItemData({Base=4000,BaseLv=1,AppendLv=1,Overlap=1,Binding=0,Append1=-1,Append2=-1,Append3=-1,Append4=-1,Append5=-1});
			elseif SubID== 3 then
				print(BtnID,SubID);
				FirstWin_Text1:SetText(NF_L0_57);
				FirstWin_Text1:Visible(true);
			
				FirstWin_CheckBox1:GetChild(0):SetTexturePos(36,0);
				FirstWin_CheckBox1:Visible(true);

				FirstWin_CheckBox2:GetChild(0):SetTexturePos(72,0);
				FirstWin_CheckBox2:Visible(true);

				NPCFUN:SetItemData({Base=4000,BaseLv=1,AppendLv=1,Overlap=1,Binding=0,Append1=-1,Append2=-1,Append3=-1,Append4=-1,Append5=-1});
				NPCFUN:SetItemData({Base=4000,BaseLv=1,AppendLv=1,Overlap=1,Binding=0,Append1=-1,Append2=-1,Append3=-1,Append4=-1,Append5=-1});
			elseif SubID== 4 then
				print(BtnID,SubID);
				FirstWin_Text1:SetText(NF_L0_58);
				FirstWin_Text1:Visible(true);
			
				FirstWin_CheckBox1:GetChild(0):SetTexturePos(36,0);
				FirstWin_CheckBox1:Visible(true);

				FirstWin_CheckBox2:GetChild(0):SetTexturePos(72,0);
				FirstWin_CheckBox2:Visible(true);

				FirstWin_CheckBox3:GetChild(0):SetTexturePos(108,0);
				FirstWin_CheckBox3:Visible(true);

				NPCFUN:SetItemData({Base=4000,BaseLv=1,AppendLv=1,Overlap=1,Binding=0,Append1=-1,Append2=-1,Append3=-1,Append4=-1,Append5=-1});
				NPCFUN:SetItemData({Base=4000,BaseLv=1,AppendLv=1,Overlap=1,Binding=0,Append1=-1,Append2=-1,Append3=-1,Append4=-1,Append5=-1});
				NPCFUN:SetItemData({Base=4000,BaseLv=1,AppendLv=1,Overlap=1,Binding=0,Append1=-1,Append2=-1,Append3=-1,Append4=-1,Append5=-1});
			elseif SubID== 5 then
				print(BtnID,SubID);
				FirstWin_Text1:SetText(NF_L0_59);
				FirstWin_Text1:Visible(true);
			
				FirstWin_CheckBox1:GetChild(0):SetTexturePos(36,0);
				FirstWin_CheckBox1:Visible(true);

				FirstWin_CheckBox2:GetChild(0):SetTexturePos(72,0);
				FirstWin_CheckBox2:Visible(true);

				FirstWin_CheckBox3:GetChild(0):SetTexturePos(108,0);
				FirstWin_CheckBox3:Visible(true);

				FirstWin_CheckBox4:GetChild(0):SetTexturePos(144,0);
				FirstWin_CheckBox4:Visible(true);

				NPCFUN:SetItemData({Base=4000,BaseLv=1,AppendLv=1,Overlap=1,Binding=0,Append1=-1,Append2=-1,Append3=-1,Append4=-1,Append5=-1});
				NPCFUN:SetItemData({Base=4000,BaseLv=1,AppendLv=1,Overlap=1,Binding=0,Append1=-1,Append2=-1,Append3=-1,Append4=-1,Append5=-1});
				NPCFUN:SetItemData({Base=4000,BaseLv=1,AppendLv=1,Overlap=1,Binding=0,Append1=-1,Append2=-1,Append3=-1,Append4=-1,Append5=-1});
				NPCFUN:SetItemData({Base=4000,BaseLv=1,AppendLv=1,Overlap=1,Binding=0,Append1=-1,Append2=-1,Append3=-1,Append4=-1,Append5=-1});
			end;
		elseif Index == 2 then --服务器消息提示
			if SubID == 1000 then
				FirstWin_Text1:SetText(NF_L0_60);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
			elseif SubID == 1001 then
				FirstWin_Text1:SetText(NF_L0_60);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
			end;
        	end;
	elseif Type == NPC_FLAG_SYS_ALTAR then    --------------祭坛-----------------------------------------------------------------------------
		if Index== 1 then    ----第几个页面------
		        FirstWin_Text1:SetText(NF_L0_61);
			FirstWin_Text1:Visible(true);
			if SubID== 1 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_62);
				Button:Visible(true);
			elseif SubID== 2 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_63);
				Button:Visible(true);
			elseif SubID==  3 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_64);
				Button:Visible(true);
                        elseif SubID==  4 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_65);
				Button:Visible(true);
                        elseif SubID==  5 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_66);
				Button:Visible(true);
                        elseif SubID==  6 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_67);
				Button:Visible(true);
                        elseif SubID== 1000 then
                                FirstWin_Text1:SetText(NF_L0_68);
                                FirstWin_Text1:Visible(true);
                                NPCFUN:EndMessage(true);
			end;

		elseif Index== 2 then
		        FirstWin_Text1:SetText(NF_L0_69);
			FirstWin_Text1:Visible(true);
                        if SubID== 1001 then
                                FirstWin_Text1:SetText(NF_L0_70);
                                FirstWin_Text1:Visible(true);
                                NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID== 1002 then
                                FirstWin_Text1:SetText(NF_L0_71);
                                FirstWin_Text1:Visible(true);
                                NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID== 1003 then
                                FirstWin_Text1:SetText(NF_L0_72);
                                FirstWin_Text1:Visible(true);
                                NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID== 1004 then
                                FirstWin_Text1:SetText(NF_L0_73);
                                FirstWin_Text1:Visible(true);
                                NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
			end;
			if SubID== 1 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_74);
				Button:Visible(true);
			elseif SubID== 2 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_75);
				Button:Visible(true);
			elseif SubID==  3 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_76);
				Button:Visible(true);
			elseif SubID== 4 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_77);
				Button:Visible(true);
			elseif SubID== 5 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_78);
				Button:Visible(true);
			elseif SubID== 6 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_79);
				Button:Visible(true);
			elseif SubID== 7 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_80);
				Button:Visible(true);
			elseif SubID== 8 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_81);
				Button:Visible(true);
			elseif SubID== 9 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_82);
				Button:Visible(true);
			elseif SubID== 10 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
				Button:SetText(NF_L0_83);
				Button:Visible(true);
			end;		
		elseif Index == 3 then		--服务器提示消息
			if SubID == 1005 then
				FirstWin_Text1:SetText(NF_L0_84);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
			elseif SubID == 1006 then
				FirstWin_Text1:SetText(NF_L0_85);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID == 1007 then
				FirstWin_Text1:SetText(NF_L0_86);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID == 1008 then
				FirstWin_Text1:SetText(NF_L0_87);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID == 1009 then
				FirstWin_Text1:SetText(NF_L0_88);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID == 1010 then
				FirstWin_Text1:SetText(NF_L0_89);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID == 1011 then
				FirstWin_Text1:SetText(NF_L0_90);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID == 1012 then
				FirstWin_Text1:SetText(NF_L0_91);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID == 1013 then
				FirstWin_Text1:SetText(NF_L0_92);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID == 1014 then
				FirstWin_Text1:SetText(NF_L0_93);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID == 1015 then
				FirstWin_Text1:SetText(NF_L0_94);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID == 1016 then
				FirstWin_Text1:SetText(NF_L0_95);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
                        elseif SubID == 1017 then
				FirstWin_Text1:SetText(NF_L0_96);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
				NPCFUN:NeedMessage(false);
			end;
		end;		
	elseif Type == NPC_FLAG_GUILDQUEST then	--------公会任务-----------------------------------------------------------------------

		if Index== 1 then    ----第几个页面------
			FirstWin_Text1:SetText(NF_L0_97);
			FirstWin_Text1:Visible(true);
			if SubID== 1 or SubID == 3 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
                        	Button:SetPosition(25,150);
				Button:SetText(NF_L0_98);
				Button:Visible(true);
			elseif SubID == 2 or SubID == 4 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
                        	Button:SetPosition(25,180);
				Button:SetText(NF_L0_99);
				Button:Visible(true);
			end;
		elseif Index == 2 then	--服务器提示消息---------------------
			if SubID == 1000 then
				FirstWin_Text1:SetText(NF_L0_100);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
			elseif SubID == 1001 then
				FirstWin_Text1:SetText(NF_L0_101);
				FirstWin_Text1:Visible(true);
				NPCFUN:EndMessage(true);
			end;
		end;	
	elseif Type == NPC_FLAG_LivingSkill then	--------公会任务-----------------------------------------------------------------------

		if Index== 1 then    ----第几个页面------
			FirstWin_Text1:SetText("请选择种类：");
			FirstWin_Text1:Visible(true);
			if SubID== 1 or SubID == 3 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
                        	Button:SetPosition(25,150);
				Button:SetText("GatheringSkill");
				Button:Visible(true);
			elseif SubID == 2 or SubID == 4 then
				print(BtnID,SubID);
				local Button = win:GetChild("FirstWin_Button" .. BtnID);
                        	Button:SetPosition(25,180);
				Button:SetText("ManufactureSkill");
				Button:Visible(true);
			end;
		elseif Index == 2 then	--服务器提示消息---------------------
			if SubID == 1000 then
				FirstWin_Text1:SetText("succed");
				FirstWin_Text1:Visible(false);
				NPCFUN:EndMessage(true);
			elseif SubID == 1001 then
				FirstWin_Text1:SetText("Cannt learn more");
				FirstWin_Text1:Visible(false);
				NPCFUN:EndMessage(true);
			end;
		end;	
				
	end;
end



--客户端消息提示框--------------------------------------------------------------------------------------------
function Set_MessageBox(Type,Index,PreSubID,SubID)
	Set_NpcFun_UI(Type,Index);
	if Type == NPC_FLAG_SYS_TRANMIT2 then    --------传送提示框----------------------------------------------------------------------------------
		if SubID== 0 then
		elseif SubID== 10 then
			FirstWin_Text1:SetText(NF_L0_102);
			FirstWin_Text1:Visible(true);
			NPCFUN:HaveMessageBox(true);
		elseif SubID == 200 then
			FirstWin_Text1:SetText(NF_L0_103);
			FirstWin_Text1:Visible(true);
			NPCFUN:HaveMessageBox(true);
		elseif SubID == 30 then
			FirstWin_Text1:SetText(NF_L0_104);
			FirstWin_Text1:Visible(true);
			NPCFUN:HaveMessageBox(true);
		elseif SubID == 40 then
			FirstWin_Text1:SetText(NF_L0_105);
			FirstWin_Text1:Visible(true);
			NPCFUN:HaveMessageBox(true);
                elseif SubID == 500 then
			FirstWin_Text1:SetText(NF_L0_106);
			FirstWin_Text1:Visible(true);
			NPCFUN:HaveMessageBox(true);
                elseif SubID == 60 then
			FirstWin_Text1:SetText(NF_L0_107);
			FirstWin_Text1:Visible(true);
			NPCFUN:HaveMessageBox(true);
		end;
	elseif Type == NPC_FLAG_SYS_CREDIT_EXCHANGE then    --------兑换提示框-----------------------------------------------------------------------
		if SubID== 0 then
		elseif SubID== 1 then
		elseif SubID == 2 then
		elseif SubID == 3 then
		elseif SubID == 4 then
		elseif SubID == 5 then
		end;
	elseif Type == NPC_FLAG_SYS_GRANT_OFFICAL then    --------国家官职提示框----------------------------------------------------------------------
		if SubID== 0 then
		elseif SubID== 1 then
		elseif SubID== 2 then
		elseif SubID== 3 then
		elseif SubID== 4 then
		elseif SubID== 5 then
		elseif SubID== 255 then
		end;
	elseif Type == NPC_FLAG_SYS_OFFICAL_SALARY then  -------------------领取俸禄提示框--------------------------------------------------------------
		if SubID== 1 then
		elseif SubID== 2 then
		elseif SubID== 3 then
		elseif SubID== 4 then
		elseif SubID== 5 then
		end;
	elseif Type == NPC_FLAG_SYS_ALTAR then    --------------祭坛-----------------------------------------------------------------------------
		if Index== 2 then
                    FirstWin_ButtonA1:SetPosition(145,500);
	            FirstWin_ButtonA2:SetPosition(220,500);
		    FirstWin_ButtonA1:Visible(true);
		    FirstWin_ButtonA2:Visible(true); 	  	
			if PreSubID == 1 then  --创建祭坛
                            if SubID== 1 then
				FirstWin_Text1:SetText(NF_L0_108);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 2 then
				FirstWin_Text1:SetText(NF_L0_109);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 3 then
				FirstWin_Text1:SetText(NF_L0_110);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 4 then
				FirstWin_Text1:SetText(NF_L0_111);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 5 then
				FirstWin_Text1:SetText(NF_L0_112);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 6 then
				FirstWin_Text1:SetText(NF_L0_113);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 7 then
				FirstWin_Text1:SetText(NF_L0_114);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 8 then
				FirstWin_Text1:SetText(NF_L0_115);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 9 then
				FirstWin_Text1:SetText(NF_L0_116);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 10 then
				FirstWin_Text1:SetText(NF_L0_117);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 0 then
			        NPCFUN:EndMessage(true);
                            end;
			elseif PreSubID == 2 then  --删除祭坛
                            if SubID== 1 then
				FirstWin_Text1:SetText(NF_L0_118);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 2 then
				FirstWin_Text1:SetText(NF_L0_119);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 3 then
				FirstWin_Text1:SetText(NF_L0_120);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 4 then
				FirstWin_Text1:SetText(NF_L0_121);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 5 then
				FirstWin_Text1:SetText(NF_L0_122);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 6 then
				FirstWin_Text1:SetText(NF_L0_123);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 7 then
				FirstWin_Text1:SetText(NF_L0_124);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 8 then
				FirstWin_Text1:SetText(NF_L0_125);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 9 then
				FirstWin_Text1:SetText(NF_L0_126);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 10 then
				FirstWin_Text1:SetText(NF_L0_127);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 0 then
			        NPCFUN:EndMessage(true);
                            end;
			elseif PreSubID == 3 then  --升级祭坛
                            if SubID== 1 then
				FirstWin_Text1:SetText(NF_L0_128);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 2 then
				FirstWin_Text1:SetText(NF_L0_129);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 3 then
				FirstWin_Text1:SetText(NF_L0_130);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 4 then
				FirstWin_Text1:SetText(NF_L0_131);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 5 then
				FirstWin_Text1:SetText(NF_L0_132);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 6 then
				FirstWin_Text1:SetText(NF_L0_133);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 7 then
				FirstWin_Text1:SetText(NF_L0_134);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 8 then
				FirstWin_Text1:SetText(NF_L0_135);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 9 then
				FirstWin_Text1:SetText(NF_L0_136);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 10 then
				FirstWin_Text1:SetText(NF_L0_137);
			        FirstWin_Text1:Visible(true);
			        NPCFUN:HaveMessageBox(true);
                            elseif SubID== 0 then
			        NPCFUN:EndMessage(true);
                            end;
			elseif PreSubID == 4 then  --添加供奉
                                if SubID== 1 then	
				    FirstWin_Text1:SetText(NF_L0_138);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 2 then
				    FirstWin_Text1:SetText(NF_L0_139);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 3 then
				    FirstWin_Text1:SetText(NF_L0_140);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 4 then
				    FirstWin_Text1:SetText(NF_L0_141);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 5 then
				    FirstWin_Text1:SetText(NF_L0_142);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 6 then
				    FirstWin_Text1:SetText(NF_L0_143);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 7 then
				    FirstWin_Text1:SetText(NF_L0_144);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 8 then
				    FirstWin_Text1:SetText(NF_L0_145);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 9 then
				    FirstWin_Text1:SetText(NF_L0_146);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 10 then
				    FirstWin_Text1:SetText(NF_L0_147);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 0 then
			            NPCFUN:EndMessage(true);
                                end;
			elseif PreSubID == 5 then  --删除供奉
                                if SubID== 1 then	
				    FirstWin_Text1:SetText(NF_L0_148);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 2 then
				    FirstWin_Text1:SetText(NF_L0_149);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 3 then
				    FirstWin_Text1:SetText(NF_L0_150);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 4 then
				    FirstWin_Text1:SetText(NF_L0_151);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 5 then
				    FirstWin_Text1:SetText(NF_L0_152);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 6 then
				    FirstWin_Text1:SetText(NF_L0_153);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 7 then
				    FirstWin_Text1:SetText(NF_L0_154);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 8 then
				    FirstWin_Text1:SetText(NF_L0_155);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 9 then
				    FirstWin_Text1:SetText(NF_L0_156);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 10 then
				    FirstWin_Text1:SetText(NF_L0_157);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 0 then
			            NPCFUN:EndMessage(true);
                                end;
			elseif PreSubID == 6 then  --升级供奉
                                if SubID== 1 then	
				    FirstWin_Text1:SetText(NF_L0_158);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 2 then
				    FirstWin_Text1:SetText(NF_L0_159);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 3 then
				    FirstWin_Text1:SetText(NF_L0_160);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 4 then
				    FirstWin_Text1:SetText(NF_L0_161);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 5 then
				    FirstWin_Text1:SetText(NF_L0_162);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 6 then
				    FirstWin_Text1:SetText(NF_L0_163);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 7 then
				    FirstWin_Text1:SetText(NF_L0_164);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 8 then
				    FirstWin_Text1:SetText(NF_L0_165);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 9 then
				    FirstWin_Text1:SetText(NF_L0_166);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 10 then
				    FirstWin_Text1:SetText(NF_L0_167);
			            FirstWin_Text1:Visible(true);
			            NPCFUN:HaveMessageBox(true);
                                elseif SubID== 0 then
			            NPCFUN:EndMessage(true);
                                end;
			end;	
		end;
		
	elseif Type == NPC_FLAG_GUILDQUEST then  -------------------公会任务提示框--------------------------------------------------------------
		if SubID== 1 then
		elseif SubID== 2 then

		end;
	end;
end


--初始化UI的原始位置------------------------------------------------------------------------------------
function SetOriginalPosition()
	FirstWin_Text1:SetPosition(45,100);
	FirstWin_Text2:SetPosition(15,100);
	FirstWin_Text3:SetPosition(15,120);

	FirstWin_Button1:SetPosition(25,100);
	FirstWin_Button2:SetPosition(25,120);
	FirstWin_Button3:SetPosition(25,140);
	FirstWin_Button4:SetPosition(25,160);
	FirstWin_Button5:SetPosition(25,180);
	FirstWin_Button6:SetPosition(25,200);
	FirstWin_Button7:SetPosition(25,220);
	FirstWin_Button8:SetPosition(25,240);
	FirstWin_Button9:SetPosition(25,260);
	FirstWin_Button10:SetPosition(25,280);

	FirstWin_CheckBox1:SetPosition(45,220);
	FirstWin_CheckBox2:SetPosition(90,220);
	FirstWin_CheckBox3:SetPosition(135,220);
	FirstWin_CheckBox4:SetPosition(180,220);

	FirstWin_ButtonA1:SetPosition(145,500);
	FirstWin_ButtonA2:SetPosition(220,500);
end