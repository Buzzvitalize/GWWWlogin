-----------------------------------------
-- �ļ����ƣ�
-- ��ǰ�汾��1.1
-- ��    �ߣ�ghr
-- ������ڣ�2011.3.17
-- ��    ���������ۼƴﵽ4Сʱ��ȡ����
-----------------------------------------
--[=====[

UIList_XXXX Ϊһ����������Ϊ�Ի����ڵ�������Ŀ
	UIList_XXXX = {
		{index,SubID,TYPE,string,x,y},
		{index,SubID,TYPE,string,x,y},
		{index,SubID,TYPE,string,x,y},
		...
	}

OKBtn Ϊȷ��ȡ����ť����,Ϊ�ջ�nil��ʾĬ��(��ʾ) ,false ��ʾ����ʾ;���Ժ���һ����x y ��ʾ����ı�
	OKBtn = false
	OKBtn = {} OKBtn.x = 100 OKBtn.y = 90
	OKBtn = nil

fun Ϊ������չ���� ����flag��־�ж���������λ�� fun �� flag ��Ϊ��ѡ����

]=====]--

--TYPE DEFINE
TEXT	= 1
BUTTON	= 2
INPUT1	= 3
INPUT2	= 4
INPUT3	= 5
VALUE	= 6
ENDTEXT = 7



UIMAKER = {}
function AddWindow(WindowConf,win,BtnID,SubID)
	local x = WindowConf[5]
	local y = WindowConf[6]

	if WindowConf[3] == TEXT then
		FirstWin_Text1:SetText(WindowConf[4])
		FirstWin_Text1:Visible(true)
		if type(x) == 'number' and type(y) == 'number' then
			FirstWin_Text1:SetPosition(x,y)
		end
	elseif WindowConf[3] == ENDTEXT then
		FirstWin_Text1:SetText(WindowConf[4])
		FirstWin_Text1:Visible(true)
		if type(x) == 'number' and type(y) == 'number' then
			FirstWin_Text1:SetPosition(x,y)
		end
		NPCFUN:EndMessage(true)
	elseif WindowConf[3] == BUTTON then
		local Button = win:GetChild("FirstWin_Button" .. BtnID);
		Button:SetText(WindowConf[4])
		if type(x) == 'number' and type(y) == 'number' then
			Button:SetPosition(x,y)
		else
			Button:SetPosition(25,135)
		end
		Button:Visible(true)

	elseif WindowConf[3] == INPUT1 then
		FirstWin_ItemBtn1:Visible(true)
		if type(x) == 'number' and type(y) == 'number' then
			FirstWin_ItemBtn1:SetPosition(x,y)
		else
			FirstWin_ItemBtn1:SetPosition(25,190)
		end
	elseif WindowConf[3] == INPUT2 then
		FirstWin_ItemBtn2:Visible(true)
		if type(x) == 'number' and type(y) == 'number' then
			FirstWin_ItemBtn2:SetPosition(x,y)
		else
			FirstWin_ItemBtn2:SetPosition(45,190)
		end
	elseif WindowConf[3] == INPUT3 then
		FirstWin_ItemBtn3:Visible(true)
		if type(x) == 'number' and type(y) == 'number' then
			FirstWin_ItemBtn3:SetPosition(x,y)
		else
			FirstWin_ItemBtn3:SetPosition(65,190)
		end

	elseif WindowConf[3] == VALUE then
		InputText1:Visible(true);
		Input11:Visible(true)
		Input12:Visible(true)
		Input13:Visible(true)
		if type(x) == 'number' and type(y) == 'number' then
			Input11:SetPosition(x+8,y)
			Input12:SetPosition(x,y)
			Input13:SetPosition(x+102,y)
			InputText1:SetPosition(x+4,y+1)
		else
			Input11:SetPosition(98,200)
			Input12:SetPosition(90,200)
			Input13:SetPosition(192,200)
			InputText1:SetPosition(94,201)
		end
	end
end

function UIMAKER:Create(win,name,OKBtn)
	_G['NpcFun'..name..'_SetUI'] = function(Type,Index)
		if OKBtn ~= false then
			if type(OKBtn) == 'table' and type(OKBtn.x) == 'number' and type(OKBtn.y) == 'number' then
				FirstWin_ButtonA1:SetPosition(OKBtn.x,OKBtn.y)
				FirstWin_ButtonA2:SetPosition(OKBtn.x,OKBtn.y)
			else
				FirstWin_ButtonA1:SetPosition(145,500)
				FirstWin_ButtonA2:SetPosition(220,500)
			end

			FirstWin_ButtonA1:Visible(true)
			FirstWin_ButtonA2:Visible(true)
			win:Visible(true)
		end
	end
	_G['NpcFun'..name..'_SetText'] = function(Type,Index,BtnID,SubID)
		local _UIList = _G['UIList_'..name]
		for i,v in pairs(_UIList) do
			if v[1] == Index and v[2] == SubID then
				AddWindow(v,win,BtnID,SubID)
			elseif v[1] == Index and type(v[2]) =='table' then
				if v[2].Callback(SubID,i) then
					AddWindow(v,win,BtnID,SubID)
				end
			end
		end
	end
end

