// Declare UI element container variables
var container_play; // The play menu/buttons
var container_userInfo; // Holds username and BP
var container_header; // Holds the tabbar

// Declare UI element variables
var menu_playButton;
var menu_username;
//var menu_battlepoints; No battlepoints var needed here, as this must be located every time battlepoints require resyncing
var menu_version;
//var menu_logo; Menu logo is a pusedo element and must therefore be removed another way than targeting a specific dom element
var hasHiddenMenuLogo = false;
var menu_homeTab;
var menu_characterTab;
var menu_rewardsTab;
var menu_statisticsTab;
var menu_leaderboardsTab;
var menu_notification;

// Declare custom UI element variables
var customMenu_playButton;
var customMenu_username;
var customMenu_battlepoints;
var customMenu_version;
var customMenu_logo;
var customMenu_homeTab;
var customMenu_characterTab;
var customMenu_rewardsTab;
var customMenu_statisticsTab;
var customMenu_leaderboardsTab;
var customMenu_notification;


function loadElementsIntoVars() {
	// Assign UI element container variables
	container_play = 		document.getElementsByTagName("select-game")[0];
	container_userInfo =	document.getElementsByClassName("bro-userinfo")[0];
	container_header = 		document.getElementsByClassName("bro-header")[0];
	
	
	// Assign UI element variables
	menu_playButton = 		document.getElementsByClassName("bro-condition-play")[0];
	menu_username = 		document.getElementsByClassName("userid")[0];
	// BP removed
	menu_version = 			document.getElementsByClassName("bro-version")[0];
	// menu_logo removed
	
	menu_homeTab =			document.querySelector("button[routerlink='game/home']");
	menu_characterTab =		document.querySelector("button[routerlink='game/character']");
	menu_rewardsTab = 		document.querySelector("button[routerlink='game/store']");
	menu_statisticsTab = 	document.querySelector("button[routerlinkactive]:not([routerlink])");
	menu_leaderboardsTab =	document.querySelector("button[routerlink='game/leaderboard']");
	
	//menu_notification =
	
	
	
	// Assign custom UI element variables
	customMenu_playButton = 		document.getElementById("battlegroundsThemer_play");
	customMenu_username = 			document.getElementById("battlegroundsThemer_username");
	customMenu_battlepoints = 		document.getElementById("battlegroundsThemer_battlepoints");
	customMenu_version = 			document.getElementById("battlegroundsThemer_version");
	customMenu_logo = 				document.getElementById("battlegroundsThemer_logo");
	customMenu_homeTab = 			document.getElementById("battlegroundsThemer_home");
	customMenu_characterTab = 		document.getElementById("battlegroundsThemer_character");
	customMenu_rewardsTab = 		document.getElementById("battlegroundsThemer_rewards");
	customMenu_statisticsTab =		document.getElementById("battlegroundsThemer_stats");
	customMenu_leaderboardsTab =	document.getElementById("battlegroundsThemer_leaderboard");
	customMenu_notification =		document.getElementById("battlegroundsThemer_notification");
}

function manageReplacedUI() {
	loadElementsIntoVars();
	
	// --------------------------------------------
	// Hide elements that have been rethemed
	// --------------------------------------------
	if (customMenu_playButton) container_play.style.display = "none";
	if (customMenu_battlepoints || customMenu_username) container_userInfo.style.opacity = "0";
	if (customMenu_version) menu_version.style.display = "none";
	if (customMenu_homeTab) container_header.style.display = "none";
	
	// Remove default menu logo:
	if (customMenu_logo && !hasHiddenMenuLogo) {
		var styleElem = document.head.appendChild(document.createElement("style"));
		styleElem.innerHTML = ".wrapper[_ngcontent-c0]:after { display: none; }";
		
		hasHiddenMenuLogo = true;
	}
}

function initThemeInjection() {
	// --------------------------------------------
	// Inject theme CSS
	// --------------------------------------------
	var link = window.document.createElement('link');
	link.rel = "stylesheet";
	link.type = "text/css";
	link.href = "data:text/css," + "BT_CSSHERE";
	document.getElementsByTagName("HEAD")[0].appendChild(link);
	
	// --------------------------------------------
	// Inject theme HTML into top of body
	// --------------------------------------------
	var themeHTML = document.createElement("div");
	themeHTML.style.width = "100%";
	themeHTML.style.height = "100%";
	themeHTML.style.position = "absolute";
	themeHTML.style.left = "0";
	themeHTML.style.top = "0";
	themeHTML.innerHTML = "BT_HTMLHERE";
	// (U*HASD)IUY*HASD)(*&YUHASD)(&*HAWD)S*UIYHAOIWU*YDHA)*S&HD)*(A&YSHGD)*&ASHGD*&)g
	// (U*HASD)IUY*HASD)(*&YUHASD)(&*HAWD)S*UIYHAOIWU*YDHA)*S&HD)*(A&YSHGD)*&ASHGD*&)g
	// (U*HASD)IUY*HASD)(*&YUHASD)(&*HAWD)S*UIYHAOIWU*YDHA)*S&HD)*(A&YSHGD)*&ASHGD*&)g
	// (U*HASD)IUY*HASD)(*&YUHASD)(&*HAWD)S*UIYHAOIWU*YDHA)*S&HD)*(A&YSHGD)*&ASHGD*&)g
	// CONTINUE WORK FROM HERE:
	// Note: If the code is inserted using body.appendChild the menu can't be clicked
	document.body.appendChild(themeHTML);
	//var documentHeader = document.body.getElementsByTagName("header")[0];
	//document.body.insertBefore(themeHTML, documentHeader); // document.body.firstChild

	
	// --------------------------------------------
	// Hide default page elements
	// --------------------------------------------
	document.addEventListener('click', manageReplacedUI);
	manageReplacedUI();

	
	// --------------------------------------------
	// Attach event handlers to rethemed elements
	// --------------------------------------------
	customMenu_playButton.onclick = function(event) {
		if (event.target == customMenu_playButton) {
			menu_playButton.click();
		}
	}
	
	// Click version number to reload page for debugging
	customMenu_version.addEventListener('click', function (event) {
		engine.trigger('OpenExternalBrowser', 'http://steamcommunity.com/app/578080/announcements/');
	});
	
	customMenu_logo.addEventListener('click', function (event) {
		location.reload();
	});
	
	customMenu_homeTab.onclick = function(event) { menu_homeTab.click(); }
	customMenu_characterTab.onclick = function(event) { menu_characterTab.click(); }
	customMenu_rewardsTab.onclick = function(event) { menu_rewardsTab.click(); }
	customMenu_statisticsTab.onclick = function(event) { menu_statisticsTab.click(); }
	customMenu_leaderboardsTab.onclick = function(event) { menu_leaderboardsTab.click(); }
	
	// --------------------------------------------
	// Update custom UI element static content
	// --------------------------------------------
	var spoofedName = "BT_USERNAMEHERE";
	if (spoofedName != "BT_" + "USERNAMEHERE") {
		customMenu_username.innerText = spoofedName;
	} else {
		customMenu_username.innerText = menu_username.innerText;
	}
	customMenu_version.innerText = menu_version.innerText;


	
	// --------------------------------------------
	// Keep UI in sync
	// --------------------------------------------
	var richMemeCounter = 100000;
	var syncBattlepoints = setInterval(function() {
		if ("BT_IAMSORICH" != "BT_" + "IAMSORICH") {
			if (richMemeCounter == 100000) {
				setInterval(function() {
					customMenu_battlepoints.innerText = richMemeCounter;
					richMemeCounter += 2.23;
				}, 10);
			}
		} else {
			// Keep battlepoints in sync
			var menu_battlepoints = document.getElementsByClassName("userpoint")[0].getElementsByTagName("em")[0];
			customMenu_battlepoints.innerText = menu_battlepoints.innerText;
		}
		
		// Keep notifications in sync
		menu_notification = document.getElementsByClassName("notice-msg")[0];
		if (menu_notification) {
			customMenu_notification.style.visibility = "visible";
			customMenu_notification.innerText = menu_notification.innerText;
		} else {
			customMenu_notification.style.visibility = "hidden";
		}
		
		
	}, 200);
}


var waitForFullyLoaded = setInterval(function() {
	if (document.getElementsByClassName("userid")[0]) {
		clearInterval(waitForFullyLoaded); // Stop this loop
		initThemeInjection();
	}
}, 200); // Check loading has completed every 200ms