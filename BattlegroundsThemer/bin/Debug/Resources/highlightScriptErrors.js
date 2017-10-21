// This script enables script error displaying by adding a div to the page when a script error occurs.

var errorRevealer;
window.onerror = function(message) {
	errorRevealer = document.createElement("textarea");
	errorRevealer.style.width = "30vw";
	errorRevealer.style.height = "8vw";
	errorRevealer.style.position = "absolute";
	errorRevealer.style.margin = "0 auto";
	errorRevealer.style.bottom = "0";
	errorRevealer.style.left = "40%";
	errorRevealer.style.zIndex = "2"; // Must be -10 or less to get below the version number and steam news
	errorRevealer.style.background = "#000";
	errorRevealer.style.color = "#fff";
	errorRevealer.style.padding = ".4vw";
	errorRevealer.style.fontFamily = "Arial";
	errorRevealer.style.opacity = .8;
	errorRevealer.innerText = "Javascript error: " + message;
	document.body.appendChild(errorRevealer);
	
	var autoHideError = setInterval(function() {
		clearInterval(autoHideError); // Stops the loop
		errorRevealer.remove();
	}, 3000);
	
    return true;
}