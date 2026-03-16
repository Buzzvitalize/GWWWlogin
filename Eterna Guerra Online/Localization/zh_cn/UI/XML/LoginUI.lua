--message config
local uiapi = UIAPI

local win = nil;

function Login_OnLoad()

	win = uiapi:GetElement("InterfaceWin");
	win:UpdateRenderBuffer();

end

function InterfaceBtn_OnClick()

	win:Visible(not win:IsVisible());

end

Login_OnLoad();
