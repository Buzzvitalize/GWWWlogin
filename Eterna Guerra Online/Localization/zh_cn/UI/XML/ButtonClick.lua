print("ButtonClick Init Success!!!");

local uiapi=UIAPI
local r,g,b,a=nil;

function UpListBoxBtn()

	local scroll = this:GetParent():GetChild("ListBox"):GetChild(0);
	local pos = uiapi:GetScrollPos(scroll);
	uiapi:SetScrollPos(pos-0.1,scroll);
end

function DownListBoxBtn()

	local scroll = this:GetParent():GetChild("ListBox"):GetChild(0);
	local pos = uiapi:GetScrollPos(scroll);
	uiapi:SetScrollPos(pos+0.1,scroll);
end


function UpContainerBtn()

	local scroll = this:GetParent():GetChild("Container"):GetChild(0);
	local pos = uiapi:GetScrollPos(scroll);
	uiapi:SetScrollPos(pos-0.1,scroll);
end

function DownContainerBtn()

	local scroll = this:GetParent():GetChild("Container"):GetChild(0);
	local pos = uiapi:GetScrollPos(scroll);
	uiapi:SetScrollPos(pos+0.1,scroll);
end


function UpContainer1Btn()

	local scroll = this:GetParent():GetChild("Container1"):GetChild(0);
	local pos = uiapi:GetScrollPos(scroll);
	uiapi:SetScrollPos(pos-0.1,scroll);
end

function DownContainer1Btn()

	local scroll = this:GetParent():GetChild("Container1"):GetChild(0);
	local pos = uiapi:GetScrollPos(scroll);
	uiapi:SetScrollPos(pos+0.1,scroll);
end

function EnterWebsiteBtn()
	uiapi:HyperLink("www.176.com");
end

function SetButtonAlpha()
	--this:SetFontAlpha(200);
	a,r,g,b=this:GetFontColor();
	this:SetFontColor(255 , 0 , 0 , 255);			
end

function ReButtonAlpha()
	--this:SetFontAlpha(255);	
	this:SetFontColor(r , g , b , a);		
end

function AddYPosition()
	x , y = this:GetPosition();	
	this:SetPosition(x , y-10);		
end

function ReYPosition()
	x , y = this:GetPosition();	
	this:SetPosition(x , y+10);		
end


function AutoFightHelperFunc()

	str=SB_X1_15;
	uiapi:Helper(false,str,this:Instance());	
	
end

function AutoConfigHelperFunc()

	str=SB_X1_14;
	uiapi:Helper(false,str,this:Instance());	
	
end

function HelperLeftFunc()

	uiapi:Helper(false,'',this:Instance());	
	
end











