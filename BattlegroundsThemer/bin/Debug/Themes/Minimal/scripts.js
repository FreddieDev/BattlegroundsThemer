
// This script enables the UI to change selected buttons and effects onclick

var tabButtons;
window.onload = function() {
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
	for (var i = 0; i < tabButtons.length; i++) {
		tabButtons[i].className = "";
	}
	e.target.className = "selectedTab";
}

function changeMode(e) {
	for (var i = 0; i < modeButtons.length; i++) {
		modeButtons[i].className = "";
	}
	e.target.className = "selectedDropDownItem";
}

function changeRegion(e) {
	for (var i = 0; i < regionButtons.length; i++) {
		regionButtons[i].className = "";
	}
	e.target.className = "selectedDropDownItem";
}