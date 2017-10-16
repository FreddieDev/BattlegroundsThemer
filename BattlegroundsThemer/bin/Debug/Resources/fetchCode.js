// This script allows the game UI to load and then copies the HTML to the clipboard for studying.
// Alternatively you can just press J to copy it all.

// If you don't want it to simply fetch the HTML when the menu has fully loaded, set waitForUIToLoad to false and tweak the attemptCopyInterval interval to set when the code will be copied
// This can be useful because the HTML changes dynamically as the menu goes through different phases (loading, splash screens, connecting and completion)

// Enable onlyWaitForHotkey to avoid automatic copying of HTML, and instead just copy when the J key is pressed.
var onlyWaitForHotkey = true;
var waitForUIToLoad = true;
var attemptCopyInterval = 1000; // Default 1000 (1 second)

function copyPageHTML() {
	// Create a fullscreen div containing all the page's code as plain text
	var codeRevealer = document.createElement("textarea");
	codeRevealer.style.width = "100%";
	codeRevealer.style.height = "100%";
	codeRevealer.style.position = "absolute";
	codeRevealer.style.left = "0";
	codeRevealer.style.top = "0";
	codeRevealer.style.zIndex = "-10"; // Must be -10 or less to get below the version number and steam news
	codeRevealer.style.background = "#f55";
	codeRevealer.innerText = document.documentElement.outerHTML;
	document.body.appendChild(codeRevealer);
	
	// Copy all of the code from the div to the clipboard
	codeRevealer.select();
	document.execCommand("copy");
	
	// Clean up the text
	codeRevealer.innerText = "Menu code copied to your clipboard!\nMenu code copied to your clipboard!\nMenu code copied to your clipboard!\nMenu code copied to your clipboard!";
}

if (!onlyWaitForHotkey) {
	var waitForUILoaded = setInterval(function() {
		if (document.getElementsByClassName("userid")[0] && waitForUIToLoad) {
			// Stop the loop
			clearInterval(waitForUILoaded);
			
			copyPageHTML();
		}
	}, attemptCopyInterval);
}

// Debugging J hotkey to reveal page code
document.addEventListener('keydown', function(e) {
	// J key
	if (e.keyCode == 74) {
		copyPageHTML();
	}
}, false);