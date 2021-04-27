/* FUNZIONI PER LA GESTIONE DELLE STRADE E DEI COMUNI */
// funzione che permette di aprire il popup di ricerce dei comuni. 	
function ApriComuni(FunzioneJSRitorno,CodBelfiore,Cap,CodCNC,CodIstat,Denominazione,Provincia, StileStradario, UrlPopComuni){
	var Parametri = '';
	Parametri = 'FunzioneRitorno='+FunzioneJSRitorno;
    Parametri += '&CodBelfiore='+CodBelfiore;
    Parametri += '&Cap='+Cap;
    Parametri += '&CodCNC='+CodCNC;
	Parametri += '&CodIstat='+CodIstat;
    Parametri += '&Denominazione='+Denominazione;
    Parametri += '&Provincia='+Provincia;
    Parametri += '&Stile='+StileStradario;
    
	var f = window.open(UrlPopComuni + '?'+Parametri, 'fComuni', 'top ='+ (screen.height - 550) / 2 +', left='+ (screen.width - 600) / 2 +' width=600,height=550, status=yes, toolbar=no,scrollbar=no, resizable=no');
	f.focus();
}