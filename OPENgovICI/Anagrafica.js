function ApriRicercaAnagrafe(nomeSessione)
{ 
	var winWidth=980;
	var winHeight=680;
	var myleft=(screen.width-winWidth)/2;
	var mytop=(screen.height-winHeight)/2 - 40;
						
	Parametri="sessionName=" + nomeSessione;
	WinPopUpRicercaAnagrafica=window.open('../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?'+Parametri,'','width='+winWidth+',height='+winHeight+', status=yes, toolbar=no,top='+mytop+',left='+myleft+',Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no');
}		

function ApriRicercaAnagrafeCalcoloIci(nomeSessione)
{ 
	var winWidth=980;
	var winHeight=680;
	var myleft=(screen.width-winWidth)/2;
	var mytop=(screen.height-winHeight)/2 - 40;
						
	Parametri="sessionName=" + nomeSessione;
	WinPopUpRicercaAnagrafica=window.open('../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?'+Parametri,'','width='+winWidth+',height='+winHeight+', status=yes, toolbar=no,top='+mytop+',left='+myleft+',Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no');
}	


function ApriRicercaAnagrafeCompensazioni(nomeSessione)
{ 
	var winWidth=980;
	var winHeight=680;
	var myleft=(screen.width-winWidth)/2;
	var mytop=(screen.height-winHeight)/2 - 40;
						
	Parametri="sessionName=" + nomeSessione;
	WinPopUpRicercaAnagrafica = window.open('../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?' + Parametri, '', 'width=' + winWidth + ',height=' + winHeight + ', status=yes, toolbar=no,top=' + mytop + ',left=' + myleft + ',Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no');
}
