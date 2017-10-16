var tabButtons, modeButtons, regionButtons;
function myScript() {
	tabButtons = document.getElementById("tabsBarRhombus").getElementsByTagName("li");
	modeButtons = document.getElementById("modeSelectorMenu").getElementsByTagName("li");
	regionButtons = document.getElementById("regionSelectorMenu").getElementsByTagName("li");

	for (var i = 0; i < tabButtons.length; i++) {
		tabButtons[i].addEventListener("click", function(){ changeTab(event); });
	}

	for (var i = 0; i < modeButtons.length; i++) {
		modeButtons[i].addEventListener("click", function(){ changeMode(event); });
	}

	for (var i = 0; i < regionButtons.length; i++) {
		regionButtons[i].addEventListener("click", function(){ changeRegion(event); });
	}
}

function changeTab(e) {
	for (var i = 0; i < tabButtons.length; i++) { tabButtons[i].className = "";	}
	e.target.className = "selectedTab";
}

function changeMode(e) {
	for (var i = 0; i < modeButtons.length; i++) { modeButtons[i].className = ""; }
	e.target.className = "selectedDropDownItem";
	document.getElementById("modeSelectorButton").innerText = e.target.innerText;
}

function changeRegion(e) {
	for (var i = 0; i < regionButtons.length; i++) { regionButtons[i].className = ""; }
	e.target.className = "selectedDropDownItem";
	document.getElementById("regionSelectorButton").innerText = e.target.innerText;
	
}

var launchScriptOnThemeLoad = setInterval(function() {
	if (document.getElementById("battlegroundsThemer_body")) {
		clearInterval(launchScriptOnThemeLoad); // Stop this loop
		
		myScript();
	}
}, 200); // Check loading has completed every 200ms