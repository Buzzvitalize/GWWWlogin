-- 2008.09.10 tavia
--帮组系统的配置------

local HelpSystem_Cofig = {};
--新手帮助
HelpSystem_Cofig[0] = {
 static_text=[[
|cffFFBBFFMap|cffffffff：Gods have endowed everybody in Gods War Online with a strong prescient ability. Hit |cffD5912BM|cffffffff to open the world map, and adjust the settings at the top of the window and to get corresponding information from the world map and minimap. It can be invaluable when travelling!

|cffFFBBFFQuest|cffffffff：Do you see the NPCs with a |cffD5912B'?'|cffffffff above their heads? Accept quests from them. When you complete a certain quest, the corresponding NPC will be displayed with a|cffD5912B'!'|cffffffff. Press the |cffD5912B'Q'|cffffffff key to check your quests and search those you are eligible for but haven't accepted.

|cffFFBBFFCharacter|cffffffff：Hit |cffD5912BC|cffffffff to look into your character information.

|cffFFBBFFParty|cffffffff：Click on a target and then click the |cffD5912B'operation'|cffffffff" button under his name in the upper center of the screen to invite him to team up with you.

|cffFFBBFFBackpack|cffffffff：Press the |cffD5912BB|cffffffff key to open your backpack.

|cffFFBBFFSkill|cffffffff：You can learn a new skill every time you reach a certain level. Purchase a book from the |cffD5912B'Skill Merchant'|cffffffff and right click it to learn the skill. Whereas, only |cffD5912B'Junior Books'|cffffffff are available at the Skill Merchant's, more adanced books can be obtained by killing monsters or from other players. Hit |cffD5912BS|cffffffff to display all the skills that you have learnt and drag the skill icon to the |cffD5912B'Action Bar'|cffffffff at the bottom of screen. After that you can cast skills just by pressing the relevant keys.

|cffFFBBFFSocial|cffffffff：Hit |cffD5912BF|cffffffff to bring up your social panel. You can add, remove, party and whisper to players. Even ignore them if you'd like.

|cffFFBBFFCombat|cffffffff：Equip your weapon and left click on a target to attack.

|cffFFBBFFChat|cffffffff：Chat with players through different channels. Click players' names, add them to friends, send team invites or guild invites.
]]
}
--人物属性
HelpSystem_Cofig[1] = {
 static_text=[[
Open Char Info Panel Shortcut  |cffD5912B C |cffffffff
Oh, my god, I don't understand all these attributes.
"Let me explain them to you! It's really not so hard." (God's voice)
  |cffFFBBFFFaith|cffffffff：The god who your faction worships.
  |cffFFBBFFClass|cffffffff：Chosen when creating your character.
  |cffFFBBFFReputation|cffffffff：Your reputation in your faction.
  |cffFFBBFFGuild|cffffffff：You can only communicate with Gods through a guild.
  |cffFFBBFFPosition|cffffffff：You status in a guild.
  |cffFFBBFFContribution|cffffffff：The more contribution you have, the more favor from the Gods which means more benefits
  |cffFFBBFFHP|cffffffff：The character's max health points.
  |cffFFBBFFMP|cffffffff：The character's max mana points.
  |cffFFBBFFATK|cffffffff：a basic attribute of Warriors and Champions.
  |cffFFBBFFDEF|cffffffff：Avoid damage from attacks.
  |cffFFBBFFHealing|cffffffff：Bonuses from healing other players. A strong attribute of priests.
  |cffFFBBFFHit|cffffffff：Hit rate for attack.
  |cffFFBBFFDodge|cffffffff：Dodge attacks.
  |cffFFBBFFCrit|cffffffff：Causes deadly damage.
  |cffFFBBFFResilience|cffffffff：Defends against deadly damage.
  |cffFFBBFFDmg|cffffffff：The higher the level of attack, the more damage will be caused.
  |cffFFBBFFMD|cffffffff：The higher the level of magic attack, the more magic damage will be caused.
  |cffFFBBFFDmg Absorption|cffffffff：Absorbs both melee damage and magic damage.

 God: I told you so much, do you understand? If not, take another look.
]]
}
--任务
HelpSystem_Cofig[2] = {
 static_text=[[
   |cffFFBBFFMain Quest|cffffffff：Each person has a primary story. Come and get into the Gods War storyline, it can lead you to the most mysterious places in Ancient Greece. Furthermore, big rewards will be there waiting too.

   |cffFFBBFFRepeated Quest|cffffffff：This kind of quest can be done repeatedly so it can help players level up quickly.

   |cffFFBBFFFaction Quest|cffffffff：A faction is a big family, your people, even strangers from the same faction can do faction quests together. However, these kind of quests are usually diffcult, but the rewards are comparable to the difficulty.

   |cffFFBBFFDaily Quest|cffffffff：These quests deliver better rewards, but they are time limited. Also, the difficulty will increase as your level goes up.

   |cffFFBBFFGuild Quest|cffffffff：More and more resources need to be collected when Guild Quests are finished. It's crucial for the Guild's prosperity. To be a powerful Guild, all members should contribute to it.

   |cffFFBBFFScroll Quest|cffffffff：Get these quests by killing monsters, and earn big rewards by doing them.


]]
}
--装备
HelpSystem_Cofig[3] = {
 static_text=[[Turn out the pockets… hm… Wow, there is a broken sheepskin, let me see…
|cffFFBBFFWeapon|cffffffff：One-Handed Sword (Warrior), Two-Handed Spear (Champion), One-Handed Wand (Priest), Two-Handed Wand (Mage)
|cffFFBBFFHead|cffffffff：Helm (Melee), Crown (Melee and Mana), Cap (Mana)
|cffFFBBFFBody|cffffffff：Armor (Melee Defense), Clothes (Mana Defense)
|cffFFBBFFGloves|cffffffff：Increases shot rate.
|cffFFBBFFSleeves, Leggings|cffffffff：Increases both melee and mana defense.
|cffFFBBFFBoots|cffffffff：Increases dodge rate.  ^o^
|cffFFBBFFNecklace|cffffffff：Increases damage absorption. ～
|cffFFBBFFRing|cffffffff：Different attributes will be given to different levels. Equip as you need.
|cffFFBBFFShield|cffffffff：Exclusive for warriors and priests. It is useful for training, as it can increase defense.
Ah? The gear has different qualities? Let me search the pocket again……
|cffD5912BQuality|cffffffff and |cffD5912BGrade|cffffffff measure how good the gear is. The Gear is better with higher |cffFFBBFFQuality|cffffffff and |cffFFBBFFGrade|cffffffff, possibly even giving off a magic light, as well.
|cffFFBBFFGear Grade|cffffffff: Common, |cff00CCFFEnhanced|cffffffff, |cff2BD591Delicate|cffffffff, |cff08D631Good|cffffffff, |cffFF9900Superior|cffffffff, |cfff0c800Classic|cffffffff, |cffFFFF00Eternal|cffffffff, |cffFF88E7Epic|cffffffff, |cffff4500Legendary|cffffffff and |cffDD00B1Mystic|cffffffff.
|cffFFBBFFStar|cffffffff：Grade 1, Grade 2,Grade 3……|cffDD00B1'Surprise'|cffffffffgrades（The gear has been upgraded to such a high level, even the Gods are surprised. So it is called the |cffDD00B1"Surprise"|cffffffff level）

]]
}
--交际
HelpSystem_Cofig[4] = {
 static_text=[[
    Communication is significant. To make friends, defeat |cffD5912B'BOSS'|cffffffffes, fight with  players from other factions or go adventuring in other factions territories requires lots of communication. Also players can whisper to each other or team up to do a quest.

   What's the |cffD5912B'Blacklist'|cffffffff? People who are placed into your Blacklist will never disturb you again, all their words will be blocked. WOW, the world is at peace.

]]
}
--公会
HelpSystem_Cofig[5] = {
 static_text=[[
The guilds in GodsWar Online offer a place where players can communicate with each other and also worship Gods to improve their abilities.

|cffFFBBFFGuild Building|cffffffff: Guild officials can build various guild buildings to improve the level of the guilds, increase the max number of guild members and provide altars. Guild buildings are the basis of a guild.

|cffFFBBFFMake offerings to Gods:|cffffffff: Players can use contribution points to make offerings to the Gods. The more offerings are made, the more bonuses players will gain, also, more offerings will be consumed per hour. The God Altar will only endow bonuses to players who have made offering to Gods.

|cffFFBBFFGuild Quest|cffffffff: Guild officials can issue guild quests every day. Guild members may gain Guild Gold, Guild Silver and Guild Contribution points by completing these quests or by donating Silver and Gold.

|cffFFBBFFGuild Welfare|cffffffff: Find the “Devoted Acolyte” in the main city and claim offerings 10 times a week. The record will be reverted back to 0 on every Saturday.

]]
}
HelpSystem_Cofig[6] = {
 static_text=[[
1. Click |cffD5912B'Set'|cffffffff to open the |cffD5912B'GodsWar Guardian Elf 1.0'|cffffffff interface and then choose some options.

2.Drag general skills to the Shortcut below, and remember to tick the |cffD5912B'Auto-cast'|cffffffffsetting. The character will then cast skills in sequence. If a skill is cooling, the next skill will be cast.

3. Tick |cffD5912B'Auto-attack'|cffffffff to make the character attack automatically.

4.Tick auto-use Healing Potion or Mana Potion in |cffD5912B'Healing settings'|cffffffff. Input number 1-9 to allow the character to use potions when HP or MP fall below |cffD5912B10%-90%|cffffffff (can be adjusted).

5. Tick |cffD5912B'Healing Potion used up'|cffffffff or |cffD5912B'Mana Potion used up'|cffffffff under |cffD5912B'Auto-back to city settings'|cffffffff and drag a portal skill or portal scroll into the frame below. so when the character runs out of potions, it will return to the main city automatically.

6. Click |cffD5912B'Save'|cffffffff to save your settings and then click |cffD5912B'Start'|cffffffff, after that the character will enter |cffD5912B'AFK'|cffffffff mode. You can also press |cffD5912B'F12'|cffffffff or click |cffD5912B'AFK'|cffffffff on the interface. If you want to change anything, click |cffD5912B'Set'|cffffffff again.

]]
}
--推荐人
HelpSystem_Cofig[7] = {
 static_text=[[
The Recommendation System aims to encourage players to invite more friends into GodsWar Online and gives players rewards for doing so.

|cffFFBBFFObtain Recommender ID|cffffffff: click the "Buzz" button on the Stats panel to obtain a Recommender ID.

|cffFFBBFFUse of Recommender ID|cffffffff: Input your recommender ID when your friend creates their character, you and your friend must be on the same server.

|cffFFBBFFRewards|cffffffff: When your friend spends money in game, you will receive 5% of the total amount spent, but this will not be deductued from your friend's Gold

]]
}
--星座系统
HelpSystem_Cofig[8] = {
 static_text=[[
Everyone has a zodiac to watch over them, heroes included. What can your zodiac do for you?

|cffFFBBFFAccumulated EXP, Talent EXP|cffffffff: A good amount of EXP is often gathered by killing monsters, from quests and from EXP stones. The accumulated EXP serves as a bonus that increases as the Zodiac advances, resulting in a greater effect.

|cffFFBBFFSkill Training|cffffffff: Zodiac can train heroes and improve their skills.
Attack skill training and improvement are ways to increase attack power. Some skills will do a fixed amount of damage while some can elevate the amount of damage by a good percentage. As attack power increases, the amount of MP consumed in the process will increase as well.
Defensive skill training and improvement are ways to decrease the enemies' attack power, and that results in less damage. Some skills will allow a fixed amount of damage done while some can cut the damage done by a good percentage.

|cffFFBBFFActivating Skill Slots|cffffffff: Skill slots can be activated using gold. The more you activate, the more skills you can learn.

|cffFFBBFFUpgrading Skill Slots|cffffffff: Skill slots can be upgraded using zodiac energy and talent points.

]]
}

--圣衣系统
HelpSystem_Cofig[9] = {
 static_text=[[
When you come to Greece, make a Holy Suit!
1.|cffFFBBFF Purchase Holy Boxes and Holy Wares |cffffffff from sources like the [Props] Merchant, the Item Mall, and the B-GOLD Shop.
2.|cffFFBBFF Storing Exp in a Holy Box|cffffffff: Bring your Holy Box to the|cffD5912B Master Vestment Forger|cffffffff and then choose |cffD5912B Holy Stone Storing Exp|cffffffff. Note: The higher your level, the more Exp you can store.
3.|cffFFBBFF Transfering EXP onto equipment|cffffffff: Bring your Holy Box containing Exp to the |cffD5912B Master Vestment Forger|cffffffff, and choose |cffD5912BTransfering EXP onto the equipment|cffffffff”.
4.|cffFFBBFF Holy Suit Design|cffffffff: Bring your refined equipment and Holy Ware to the |cffD5912BMaster Vestment Forger|cffffffff and wait patiently for your Holy Suit to be created!
]]
}

--圣石系统
HelpSystem_Cofig[10] = {
 static_text=[[
How do players obtain Holy Stones?
1.|cffFFBBFF To mount a Holy Stone on your equipment,|cffffffff you will need me to drill an opening for you. The first opening will cost you 230 gold, and a second one will cost 2300 gold.
2.|cffFFBBFF Holy Stone Choices|cffffffff: Heated Holy Stone and Cooled Holy Stone.
Heated Holy Stones can be mounted on weapons, gloves, helmets and rings.
Cooled Holy Stones can be mounted on armor, sheilds, sleeves, leggings, shoes, belts, and amulets.
3.|cffFFBBFF Holy Stone Upgrade|cffffffff: The Item Mall is a source for obtaining the Eclipse Stone, which is needed to upgrade Holy Stones. Bring your Holy Stone and Eclipse Stone to the|cffD5912B Holy Stone Artisan|cffffffff to be upgraded. Upgrade a Holy Stone from Lv.1 through Lv.4, using a Level 1 Eclipse Stone, Lv.5 through Lv.7 with a Level 2 Eclipse Stone, and Lv.8 through Lv.10 with a Level 3 Eclipse Stone.
Holy Stone upgrades are not guaranteed due to certain tech difficulties. When a Holy Stone is upgraded unsuccessfully, its level will be reduced by 1. However, if you use an Evasion Signet when upgrading a Holy Stone, the chance of success will be increased by 10% and there will be no risk of degradation from a failed upgrade.
A Copper Evasion Signet improves the success rate of a Holy Stone upgrade by 10%, and guarantees there will be no risk of degradation when upgrading to a Lv. 4 or Lv. 5 Holy Stone.
A Silver Evasion Signet improves the success rate of a Holy Stone upgrade by 10%, and guarantees there will be no risk of degradation when upgrading a Lv. 5 or Lv. 6 Holy Stone.
A Gold Evasion Signet improves the success rate of a Holy Stone upgrade by 10%, and guarantees there will be no risk of degradation when upgrading a Lv. 6 or Lv. 7 Holy Stone.
4.|cffFFBBFF Choice of Holy Spirit|cffffffff: Dramatically improves the effect of a Holy Stone. Available in the |cffD5912B Item Mall |cffffffff.
Heated Holy Stones can be implemented on the following Holy Spirits:
Fire Spirit of Destruction: Opponent's physical attacks do not affect you.
Fire Spirit of Penetration: Opponent's magic defense does not affect you.
Fire Spirit of Fist: Increases physical damage.
Fire Spirit of Fiery: Increases magic damage.
Fire Spirit of Blood：Increases bonus critical hitting.
Fire Spirit of Pressure: Increases the damage caused by critical hitting.
Fire Spirit of Assail: Increases physical damage by a certain percentage.
Fire Spirit of Lightning: Increases magic damage by a certain percentage.
Cooled Holy Stone can be implemented onto the following Holy Spirits:
Water Spirit of Darkness: Reduces physical damage by a certain percentage.
Water Spirit of Mist: Reduces magic damage by a certain percentage.
Water Spirit of Silence: Reduces physical damage taken.
Water Spirit of Chillness: Reduces magic damage taken.
Water Spirit of Ice: Reflects a certain amount of damage taken back to your opponent. Skill does not work when you are confronting a monster.
Water Spirit of Frost:  Reflects a certain amount of damage taken back to your opponent. But the skill does not work when you are confronting a monster.
Water Spirit of Intent: Reduces critical-hitting damage to you by a certain percentage.
Water Spirit of Resilience: Reduces critical-hitting damage to you.
5.|cffFFBBFF Implementing a Holy Spirit|cffffffff: Bring the Holy Spirit of your choice and the upgraded Holy Stone to the |cffD5912BHoly Stone Master|cffffffff , who will imbue the Holy Stone with the power of the Holy Spirit. The resulting attributes will vary within a certain range. If you're using a Goddess' Stone, the results will usually be optimized.
6.|cffFFBBFF Mounting a Holy Stone|cffffffff: Bring the perforated equipment and refined Holy Spirit to the |cffD5912BHoly Stone Artisan|cffffffff , who will mount the Holy Stone on the equipment. This will improve the equipments attributes significantly.

 ]]
}

--坐骑系统
HelpSystem_Cofig[11] = {
 static_text=[[
Mounts are loyal pals to the ancient Greeks. Now, veteran Greek heroes can saddle up on their favorite mount and take a ride to adventure...and beyond!
The across-the-board update of the Mount System is up now. Mounts will not only increase your movement speed, but also can enhance your fighting ability!

|cff08D631Mount Gear|cffffffff
The new mount system will also be getting a whole new look with 5 catogories of Mount Gear now available: |cfff0c800Coronet, Armor, Soul, Saddle, Tassels |cffffffff. Mount Gear (with non-speed bonus attributes): Once equipped it will increase a player's attributes;  Mount Gear (with speed bonus attributes): Once equipped it will increase a player's attributes, including movement speed.
Note: |cfff0c800Blacksmiths|cffffffff are capable of creating mount gear for all 5 categories.

|cff08D631Mount Gear Restrictions|cffffffff
Yes, there are protocols for outfitting your mount, such as |cfff0c800the level of the Mount Gear to be equipped cannot exceed the level of the mount.|cffffffff.
Note: Equipment Order
|cfff0c800Player's Level ≥Level of Mount ≥Level of Mount Gear|cffffffff

|cff08D631Mount Upgrade|cffffffff
If you have the required amount of Soul Stones, go find the |cfff0c800Mount Feeder|cffffffff(120,-170) in the main city and start to upgrade your mount! The required amount of Soul Stones is equal to the level attempted. Upgrades happen on a scale of 10 levels. For example, it takes 50 |cfff0c800 Soul Stone (Lvl 50)|cffffffff to upgrade a mount from Lvl 40 to Lvl 50. \n  It takes 120|cfff0c800 Soul Stones (Lvl 120)|cffffffff to upgrade a mount from Lvl 110 to Lvl 120.
Note:|cfff0c800Blacksmithing|cffffffff can be used to produce Soul Stones for mount upgrades.


  ]]
}

HelpSystem_Cofig[12] = {
  static_text=[[
|cffCD7F32Bronze Suit: |cffffffffThe Bronze Suit grants a maximum bonus of 10% to gear stats, with each level providing an additional 1% bonus, culminating in a total of 5% at level 5.

	           |cffCD7F32Bronze Suit         
|cffffffff-------------------------------------------------
Level 0-1   |  9,688 exp       
Level 1-2   |  58,127 exp      
Level 2-3   |  174,380 exp     
Level 3-4   |  348,759 exp     
Level 4-5   |  581,265 exp     
Level 5-6   |  4,198,026 exp   
Level 6-7   |  4,843,876 exp   
Level 7-8   |  5,489,727 exp   
Level 8-9   |  6,458,502 exp   
Level 9-10  |  7,427,277 exp  
Level 10-1  |  3,875,101 exp
    ]]
}
HelpSystem_Cofig[13] = {
  static_text=[[
|cffffffffSilver Suit: |cffffffffThe Silver Suit grants a maximum bonus of 20% to gear stats, with each level providing an additional 1% bonus, culminating in a total of 15% at level 5.

            |cffffffff Silver Suit
|cffffffff-------------------------------------------------
Level 1-2   |  58,127 exp          
Level 2-3   |  9,416,496 exp     
Level 3-4   |  14,647,883 exp    
Level 4-5   |  18,832,991 exp    
Level 5-6   |  23,018,100 exp    
Level 6-7   |  27,278,774 exp    
Level 7-8   |  31,475,509 exp    
Level 8-9   |  35,672,243 exp    
Level 9-10  |  41,967,345 exp    
Level 10-1  |  57,661,505 exp  
    ]]
}
HelpSystem_Cofig[14] = {
  static_text=[[
|cffFFD700Gold Suit: |cffffffffThe Gold Suit grants a maximum bonus of 30% to gear stats, with each level providing an additional 1% bonus, culminating in a total of 25% at level 5.

	           |cffFFD700 Gold Suit          
|cffffffff-------------------------------------------------
Level 1-2   |  61,505,605 exp   
Level 2-3   |  63,497,705 exp   
Level 3-4   |  69,193,805 exp   
Level 4-5   |  73,037,906 exp   
Level 5-6   |  76,882,006 exp   
Level 6-7   |  80,726,106 exp   
Level 7-8   |  88,414,306 exp   
Level 8-9   |  96,102,508 exp   
Level 9-10  |  100,000,000 exp  
Level 10-1  |  133,833,876 exp 
    ]]
}
HelpSystem_Cofig[15] = {
  static_text=[[
|cff00FFFFPlatinum Suit: |cffffffffThe Platinum Suit grants a maximum bonus of 40% to gear stats, with each level providing an additional 1% bonus, culminating in a total of 35% at level 5.

	           |cff00FFFF Platinum Suit
|cffffffff-------------------------------------------------
Level 1-2   |  155,886,267 exp       
Level 2-3   |  184,272,577 exp       
Level 3-4   |  234,454,153 exp       
Level 4-5   |  295,435,479 exp       
Level 5-6   |  373,355,467 exp       
Level 6-7   |  485,358,297 exp       
Level 7-8   |  616,532,565 exp       
Level 8-9   |  735,697,878 exp       
Level 9-10  |  866,697,995 exp       
Level 10-1  |  999,999,999 exp 
    ]]
}
HelpSystem_Cofig[16] = {
  static_text=[[
|cFF74DBABMithril Suit: |cffffffffThe Mithril Suit grants a maximum bonus of 50% to gear stats, with each level providing an additional 1% bonus, culminating in a total of 45% at level 5.

	           |cFF74DBAB Mithril Suit        
|cffffffff-------------------------------------------------
Level 1-2   |  12 Experience Prism  
Level 2-3   |  15 Experience Prism  
Level 3-4   |  18 Experience Prism  
Level 4-5   |  21 Experience Prism  
Level 5-6   |  24 Experience Prism  
Level 6-7   |  27 Experience Prism  
Level 7-8   |  30 Experience Prism  
Level 8-9   |  33 Experience Prism  
Level 9-10  |  36 Experience Prism  
Level 10-1  |  39 Experience Prism  
    ]]
}
HelpSystem_Cofig[17] = {
  static_text=[[
|cFFBE5BFCOrichalcum Suit: |cffffffffThe Orichalcum Suit grants a maximum bonus of 60% to gear stats, with each level providing an additional 1% bonus, culminating in a total of 55% at level 5.

	           |cFFBE5BFC Orichalcum Suit
|cffffffff-------------------------------------------------
Level 1-2   |  42 Experience Prism      
Level 2-3   |  45 Experience Prism      
Level 3-4   |  48 Experience Prism      
Level 4-5   |  51 Experience Prism      
Level 5-6   |  54 Experience Prism      
Level 6-7   |  57 Experience Prism      
Level 7-8   |  60 Experience Prism      
Level 8-9   |  63 Experience Prism      
Level 9-10  |  66 Experience Prism      
Level 10-1  |  69 Experience Prism  
    ]]
}
HelpSystem_Cofig[18] = {	
  static_text=[[
|cFFE26EA1Adamantium Suit: |cffffffffThe Adamantium Suit grants a maximum bonus of 70% to gear stats, with each level providing an additional 1% bonus, culminating in a total of 65% at level 5.

	           |cFFE26EA1 Adamantium Suit
|cffffffff-------------------------------------------------
Level 1-2   |  72 Experience Prism  
Level 2-3   |  75 Experience Prism  
Level 3-4   |  78 Experience Prism  
Level 4-5   |  81 Experience Prism  
Level 5-6   |  84 Experience Prism  
Level 6-7   |  87 Experience Prism  
Level 7-8   |  90 Experience Prism  
Level 8-9   |  93 Experience Prism  
Level 9-10  |  96 Experience Prism  
    ]]
}

HelpSystem_Cofig[19] = {	
	static_text=[[
|cFFC040FFHeated Holy Stones |cffffffffcan be implemented on the following Holy Spirits:
|cFFC040FFFire Spirit |cffffffffof Destruction: Opponent's physical attacks do not affect you.
|cFFC040FFFire Spirit |cffffffffof Penetration: Opponent's magic defense does not affect you.
|cFFC040FFFire Spirit |cffffffffof Fist: Increases physical damage.
|cFFC040FFFire Spirit |cffffffffof Fiery: Increases magic damage.
|cFFC040FFFire Spirit |cffffffffof Blood：Increases bonus critical hitting.
|cFFC040FFFire Spirit |cffffffffof Pressure: Increases the damage caused by critical hitting.
|cFFC040FFFire Spirit |cffffffffof Assail: Increases physical damage by a certain percentage.
|cFFC040FFFire Spirit |cffffffffof Lightning: Increases magic damage by a certain percentage.
|cffFFBBFF Implementing a Holy Spirit|cffffffff: Bring the Holy Spirit of your choice and the upgraded Holy Stone to the |cffD5912BHoly Stone Master|cffffffff , who will imbue the Holy Stone with the power of the Holy Spirit. The resulting attributes will vary within a certain range.

|cffFFBBFF|      Spirit      |    Min Val    |    Max Val    |
|cffffffff|---------------------------------------------|
| of Destruction       3.20%              8%           
| of Penetration       3.20%              8%           
| of Fist               160               400          
| of Fiery              120               300          
| of Blood              2.40%             6%           
| of Pressure           400               1000         
| of Assail             2.0%                5%           
| of Lightning          2.40%             6%    
|---------------------------------------------|       

	  ]]
  }

HelpSystem_Cofig[20] = {	
	static_text=[[
|cFFA080FFCooled Holy Stone |cffffffffcan be implemented onto the following Holy Spirits:
|cFFA080FFWater Spirit |cffffffffof Darkness: Reduces physical damage by a certain percentage.
|cFFA080FFWater Spirit |cffffffffof Mist: Reduces magic damage by a certain percentage.
|cFFA080FFWater Spirit |cffffffffof Silence: Reduces physical damage taken.
|cFFA080FFWater Spirit |cffffffffof Chillness: Reduces magic damage taken.
|cFFA080FFWater Spirit |cffffffffof Ice: Reflects a certain amount of damage taken back to your opponent. Skill does not work when you are confronting a monster.
|cFFA080FFWater Spirit |cffffffffof Frost:  Reflects a certain amount of damage taken back to your opponent. But the skill does not work when you are confronting a monster.
|cFFA080FFWater Spirit |cffffffffof Intent: Reduces critical-hitting damage to you by a certain percentage.
|cFFA080FFWater Spirit |cffffffffof Resilience: Reduces critical-hitting damage to you.
|cffFFBBFF Implementing a Holy Spirit|cffffffff: Bring the Holy Spirit of your choice and the upgraded Holy Stone to the |cffD5912BHoly Stone Master|cffffffff , who will imbue the Holy Stone with the power of the Holy Spirit. The resulting attributes will vary within a certain range.
|cffFFBBFF|      Spirit      |    Min Val    |    Max Val    |
|cffffffff|---------------------------------------------|
| of Darkness           2.20%            5.50%   
| of Mist               2.20%            5.50%  
| of Silence            160              400   
| of Chillness          140              350     
| of Ice                1.60%            4.0%          
| of Frost              160              400       
| of Intent             2.80%            7.0%  
| of Resilience         400              1000    
|---------------------------------------------|       

	  ]]
  }
function Get_HelpSystem_Cofig(level)
	return HelpSystem_Cofig[level];
end




