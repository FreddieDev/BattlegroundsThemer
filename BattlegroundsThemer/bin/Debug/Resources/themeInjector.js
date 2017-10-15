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
	// Inject theme HTML
	// --------------------------------------------
	var themeHTML = document.createElement("div");
	themeHTML.style.width = "100%";
	themeHTML.style.height = "100%";
	themeHTML.style.position = "absolute";
	themeHTML.style.left = "0";
	themeHTML.style.top = "0";
	themeHTML.innerHTML = "BT_HTMLHERE";
	document.body.appendChild(themeHTML);

	
	
	// --------------------------------------------
	// Prepare theme variables
	// --------------------------------------------
	// Initialise UI element container variables
	var container_play = 		document.getElementsByClassName("bro-play")[0];
	
	// Initialise UI element variables
	var menu_playButton = 		document.getElementsByClassName("bro-condition-play")[0];
	var menu_username = 		document.getElementsByClassName("userid")[0];
	var menu_battlepoints;
	var menu_version = 			document.getElementsByClassName("bro-version")[0];
	var menu_homeTab;
	var menu_characterTab;
	var menu_rewardsTab;
	var menu_statisticsTab;
	var menu_notification;
	
	// Initialise custom UI element variables
	var customMenu_playButton = 	document.getElementById("battlegroundsThemer_play");
	var customMenu_username = 		document.getElementById("battlegroundsThemer_username");
	var customMenu_battlepoints = 	document.getElementById("battlegroundsThemer_battlepoints");
	var customMenu_version = 		document.getElementById("battlegroundsThemer_version");
	var customMenu_logo = 			document.getElementById("battlegroundsThemer_logo");
	var customMenu_homeTab = 		document.getElementById("battlegroundsThemer_home");
	var customMenu_characterTab = 	document.getElementById("battlegroundsThemer_character");
	var customMenu_rewardsTab = 	document.getElementById("battlegroundsThemer_rewards");
	var customMenu_statisticsTab =	document.getElementById("battlegroundsThemer_stats");
	var customMenu_notification =	document.getElementById("battlegroundsThemer_notification");
	
	

	// --------------------------------------------
	// Check if each custom element exists, and if so, hide the original and link them together
	// --------------------------------------------
	hideUIElement(container_play);
	customMenu_playButton.addEventListener('click', function (event) {
		menu_playButton.click();
	});
	/*
	// Click version number to reload page for debugging
	customMenu_version.addEventListener('click', function (event) {
		engine.trigger('OpenExternalBrowser', 'http://steamcommunity.com/app/578080/announcements/');
	});
	
	customMenu_logo.addEventListener('click', function (event) {
		location.reload();
	});
	*/
	
	// --------------------------------------------
	// Update custom UI element content
	// --------------------------------------------
	customMenu_username.innerText = menu_username.innerText;
	customMenu_version.innerText = menu_version.innerText;

	
	
	// --------------------------------------------
	// Keep UI in sync
	// --------------------------------------------
	var syncBattlepoints = setInterval(function() {
		// Keep battlepoints in sync
		menu_battlepoints = document.getElementsByClassName("userpoint")[0].getElementsByTagName("em")[0];
		customMenu_battlepoints.innerText = menu_battlepoints.innerText;
		
		// Keep notifications in sync
		menu_notification = document.getElementsByClassName("notice-msg")[0];
		if (menu_notification) {
			customMenu_notification.style.visibility = "visible";
			customMenu_notification.innerText = menu_notification.innerText;
		} else {
			customMenu_notification.style.visibility = "hidden";
		}
	}, 200); // Update the battlepoints counter every 200ms
}

// Hides an element while still keeping it clickable via JS click()
function hideUIElement(elementToHide) {
	elementToHide.setProperty("opacity", "0", "important");
	elementToHide.setProperty("visibility", "hidden", "important");
}

var waitForFullyLoaded = setInterval(function() {
	if (document.getElementsByClassName("userid")[0]) {
		clearInterval(waitForFullyLoaded); // Stop this loop
		initThemeInjection();
	}
}, 200); // Check loading has completed every 200ms