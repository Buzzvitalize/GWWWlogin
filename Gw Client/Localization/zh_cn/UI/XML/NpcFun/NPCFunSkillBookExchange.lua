--~ UIList_SkillBookExchange = {
--~ 	{1,101,ENDTEXT,SkillBookExchange1},
--~ 	{1,102,TEXT,SkillBookExchange2},
--~ 	{1,103,INPUT1},
--~ 	{1,104,TEXT,SkillBookExchange3},
--~ 	{1,105,INPUT1},
--~ 	{2,201,ENDTEXT,SkillBookExchange6},
--~ 	{2,202,ENDTEXT,SkillBookExchange10},
--~ 	{2,290,ENDTEXT,REVIVE296T},
--~ 	{2,291,ENDTEXT,SkillBookExchange5},
--~ 	{2,292,ENDTEXT,SkillBookExchange7},
--~ 	{2,293,ENDTEXT,SkillBookExchange4},
--~ }

--~ local win = UIAPI:GetElement("FirstWin")
--~ UIMAKER:Create(win,'SkillBookExchange')
local win = UIAPI:GetElement("FirstWin");

function NpcFunSkillBookExchange_SetUI(Type,Index)
    FirstWin_ButtonA1:Visible(true);
    FirstWin_ButtonA2:Visible(true);
    win:Visible(true);
end

function NpcFunSkillBookExchange_SetText(Type,Index,BtnID,SubID)
    if Index == 1 then
        if SubID == 101 then
            FirstWin_Text1:SetText(SkillBookExchange1);
            FirstWin_Text1:Visible(true)
            NPCFUN:EndMessage(true)
        elseif SubID == 102 then
            FirstWin_Text1:SetText(SkillBookExchange2);
            FirstWin_Text1:Visible(true)
        elseif SubID == 103 then
			FirstWin_ItemBtn1:Visible(true)
			FirstWin_ItemBtn1:SetPosition(25,190)
        elseif SubID == 104 then
            FirstWin_Text1:SetText(SkillBookExchange3)
            FirstWin_Text1:Visible(true)
		elseif SubID == 105 then
			FirstWin_ItemBtn1:Visible(true)
			FirstWin_ItemBtn1:SetPosition(25,190)
        end
    elseif Index == 2 then
        if SubID == 201 then
            FirstWin_Text1:SetText(SkillBookExchange6)
            FirstWin_Text1:Visible(true)
            NPCFUN:EndMessage(true)
        elseif SubID == 202 then
            FirstWin_Text1:SetText(SkillBookExchange10)
            FirstWin_Text1:Visible(true)
            NPCFUN:EndMessage(true)
        elseif SubID == 290 then
            FirstWin_Text1:SetText(REVIVE296T)
            FirstWin_Text1:Visible(true)
            NPCFUN:EndMessage(true)
        elseif SubID == 291 then
            FirstWin_Text1:SetText(SkillBookExchange5)
            FirstWin_Text1:Visible(true)
            NPCFUN:EndMessage(true)
        elseif SubID == 292 then
            FirstWin_Text1:SetText(SkillBookExchange7)
            FirstWin_Text1:Visible(true)
            NPCFUN:EndMessage(true)
        elseif SubID == 293 then
            FirstWin_Text1:SetText(SkillBookExchange4)
            FirstWin_Text1:Visible(true)
            NPCFUN:EndMessage(true)
        end
    end
end
