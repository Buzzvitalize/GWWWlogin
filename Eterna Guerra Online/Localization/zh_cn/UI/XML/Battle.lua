local win = nil;
--初始化各个控件
function Battlefield_OnLoad()  
	win = UIAPI:GetElement("BattleScoreWin");	 
	win:UpdateRenderBuffer();
	
end
Battlefield_OnLoad();