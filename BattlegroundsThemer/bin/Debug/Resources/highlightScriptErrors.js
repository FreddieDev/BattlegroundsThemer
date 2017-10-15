// This script enables script error displaying by adding a div to the page when a script error occurs.

// Wait for UI to load before watching for script errors:
var waitForUILoaded = setInterval(function() {
	if (document.getElementsByClassName("userid")[0]) {
		document.addEventListener('error', function(e) {
			var errorRevealer = document.createElement("p");
			errorRevealer.style.width = "30vw";
			errorRevealer.style.height = "8vw";
			errorRevealer.style.position = "absolute";
			errorRevealer.style.margin = "0 auto";
			errorRevealer.style.bottom = "0";
			errorRevealer.style.left = "40%";
			errorRevealer.style.zIndex = "2"; // Must be -10 or less to get below the version number and steam news
			errorRevealer.style.background = "#f99";
			errorRevealer.innerText = document.documentElement.outerHTML;
			document.body.appendChild(errorRevealer);

			errorRevealer.innerText = "Script error: " + e.message;
		}, false);
			
		// Stop the loop
		clearInterval(waitForUILoaded);
	}
}, 1000);