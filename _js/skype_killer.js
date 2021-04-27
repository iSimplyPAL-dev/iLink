// Title: Skype Killer Code v1.1
// Author: Oliver Green - http://www.codeblog.co.uk - green2go@gmail.com
// Licesnse: Freely distributable, you MUST maintain this credits & usage line at all times please. :)
// Description: Removes the Skype toolbar inline links within IE, Firefox & Chrome. 
// (Only tested in IE8 with 4.2.0.4997 of the Skype add-on loaded, FireFox 3.6.3 with 4.2.0.5016 of the Toolbar
// & Chrome 4.1.249.1045 with 1.0.0.5048 of the Toolbar loaded)
// Usage: Call the function killSkype() on document load.

var ksEles;
var ksRuns = 0;
var ksComplete = false;
var browserName=navigator.appName; 

//Per richiamare la funzione killSkype aggiungere allo script la stringa window.onload =killSkype;

function killSkype() {
	if(ksComplete == false){
		ksEles = document.body.getElementsByTagName("*");
		var ksFound = false;
			for(i=0;i<ksEles.length;i++){
			if(ksEles[i].id=="__skype_highlight_id") ksEles[i].innerHTML = "";
			if(ksEles[i].id=="__skype_highlight_originaltext_id") ksEles[i].setAttribute("class", "");
			if(ksEles[i].className=="skype_pnh_print_container") ksEles[i].setAttribute("class", "");
			if(ksEles[i].className=="skype_pnh_container"){
				ksEles[i].innerHTML = "";
				setTimeout("killSkype()",10);
				ksFound = true;
				if(browserName=="Microsoft Internet Explorer") break;
			}	
		}
		if(ksRuns < 200 && ksFound == false)
			{ setTimeout("killSkype()",10); ksRuns++; } 
		else { if(browserName!=="Microsoft Internet Explorer") ksComplete = true; }
	} 
}













