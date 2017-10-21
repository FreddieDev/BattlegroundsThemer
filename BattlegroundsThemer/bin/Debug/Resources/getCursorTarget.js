// This script enables you to click an element to see its tag name, ID and class for debugging

var targetElementRevealer;
document.addEventListener('click', function(e) {
	e = e || window.event;
	var target = e.target || e.srcElement,
		text = target.textContent || text.innerText;
	
	if (targetElementRevealer) targetElementRevealer.remove();
	targetElementRevealer = document.createElement("textarea");
	targetElementRevealer.style.width = "30vw";
	targetElementRevealer.style.height = "8vw";
	targetElementRevealer.style.position = "absolute";
	targetElementRevealer.style.margin = "0 auto";
	targetElementRevealer.style.bottom = "0";
	targetElementRevealer.style.left = "40%";
	targetElementRevealer.style.zIndex = "2"; // Must be -10 or less to get below the version number and steam news
	targetElementRevealer.style.background = "#000";
	targetElementRevealer.style.color = "#fff";
	targetElementRevealer.style.padding = ".4vw";
	targetElementRevealer.style.fontFamily = "Arial";
	targetElementRevealer.style.opacity = .8;
	
	var elementString = "Target element: " + target.tagName;
	if (target.id) elementString += " ID: " + target.id;
	if (target.className) elementString += " class: " + target.className;
	targetElementRevealer.innerText = elementString;
	
	document.body.appendChild(targetElementRevealer);
	
	
	var autoHideError = setInterval(function() {
		clearInterval(autoHideError); // Stops the loop
		targetElementRevealer.remove();
	}, 5000);
}, false);