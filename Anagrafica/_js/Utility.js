<!--
//*****************************************************************************************
//Nota: quando si aggiunge una funzione in questo file, aggiungere un commento a questa lista )
//FUNZIONI PUBBLICHE : 
// 
// CheckLenTxt		verifica che i controlli elencati non siano vuoti
// IsBlank				verifica se il campo text è VUOTO o NO
// isWindow				verifica se l'oggetto passato è un handle di finestra valido
// Reverse				capovolge una stringa 
// Right					ritorna x caratteri a destra
// Left						ritorna x caratteri a sinistra
// GetParameter   	Ritorna il VALORE di un parametro dato il suo NOME
// NewIFrame		Include una pagina in un'altra usando il tag IFRAME (per ora non compatibile con netscape)
// LoadPagePopup	Se pagina in popup, carica url nello stesso popup altrimenti (per es. nel caso di IFRAME) carica url in un nuovo popup

// Funzioni per gestione liste
// Naviga					effettua l'incremento/decremento del numero della pagina
//
// Funzioni per gestione liste con checkbox
// CheckOrd				controlla se la colonna ordinata è quella corrente
// show_props			usata per debug
//								serve per passare alla dll l'elenco degli elementi presentati a video
// 
// StrUrl               restituisce la stringa di input sostituendone i caratteri speciali
//                      (blank, ecc.)
// 
//RendiVisibile		rende visibili o nascosti uno o piu DIV (due stringhe in ingresso,
//								una con l'elenco dei nomi delle DIV e l'altra con il loro stato)
//
//CreaLayer				crea una riga che genera la DIV con il nome passato come parametro
//ChiudiLayer			chiude la DIV creata con la funzione precedente
//
//LeggiLayer			restituische l'oggetto style del Layer passato come parametro
//CreaTabPercentuale crea una tabella che visualizza graficamente
//                   la percentuale tra i valori forniti
//CToNum          ritorna il numero in base alle impostazioni del client
//FormatNum       mantiene due cifre decimali
//SelezionaID		  seleziona un elemento della combo (value)
//RiempicmbMat		riempe il combo data una matrice con value nella prima colonna e descrizione nella seconda
//Svuotacmb				svuota una combo
//OpenPopup       apre un popup
//isObject        controlla se il campo passato è effettivamente un oggetto
//RestoreDefaultSelection		ripristina una combo al valore iniziale (utile dopo un 'confirm' annullato)
//SplitValue      esegue lo split di una stringa
//*****************************************************************************************


//*************************************************************************************
//*************************************************************************************
//												DETECT DEL BROWSER
//*************************************************************************************
//*************************************************************************************
var	BROWSER_IE=1;
var	BROWSER_NN4=2;
var	BROWSER_NN6=3;

var defSel=0;			//valore di default delle select
var strSep=", ";	//separatore delle stringhe 



//*************************************************************************************
//*************************************************************************************
//												RICONOSCIMENTO DEL TIPO DI BROWSER
//*************************************************************************************
//*************************************************************************************
 
intBrowser=BROWSER_IE; // di default il browser è IE
intBrowser=getBrowserType();

//*************************************************************************************
//FUNZIONE : getBrowserType
//           Restituisce il tipo di BROWSER in uso 
//
function Controlla()
{ 	
	if (typeof(WinPopUp)=="object") 
	{ 
		if(typeof(WinPopUp.name)!="unknown") 
		{ 
			WinPopUp.close() 
		} 
	} 
} 

window.onfocus=Controlla

function getBrowserType()
{
	var ua = window.navigator.userAgent;
	var intBrow=BROWSER_IE;
									
	ua = ua.toLowerCase();
	if (ua.indexOf('msie')==-1)
	{	// NETSCAPE o ALTRO BROWSER
		if (ua.indexOf('gecko')!=-1)
			intBrow=BROWSER_NN6; // il browser è Netscape 6
		else
			intBrow=BROWSER_NN4; // il browser è Netscape 4
	}
	return intBrow;
}

function LeggiLayer(sIdLayer,blnUseParentWindow)
{
		switch(intBrowser)
		{
			case BROWSER_NN4:
				if (!blnUseParent)
					objLayer=document.layers[sId];										//caso di PAGINA CORRENTE
				else
					objLayer=top.opener.document.layers[sId];				//caso di POPUP
				if (!isUndefined(objLayer))
					return objLayer;
				return false;
				break;
				
			default:				//IE o NN >= 6
				var objLayer = getObj(sIdLayer,blnUseParentWindow);
				if (!isUndefined(objLayer))
					return objLayer.style;
				else
					return false;
				break;
		}
}

//*************************************************************************************
//FUNZIONE : getBrowserVersion
//           Restituisce la versione del BROWSER in uso 
//
function getBrowserVersion()
{
	var ua = window.navigator.userAgent;
	var nInd1,nInd2;
	var sVersion="";
									
	ua    = ua.toLowerCase();
	nInd1 = ua.indexOf('msie');
	if (nInd1>=0)	//BROWSER IE
	{	
		nInd2 = ua.indexOf(';',nInd1+4);
		sVersion = ua.substring(nInd1+5,nInd2);
		return sVersion;
	}

	nInd1 = ua.indexOf('netscape');
	if (nInd1>=0)	
	{	
		sVersion = ua.substring(nInd1+10);
		return sVersion;
	}

	return sVersion;
}


//*************************************************************************************
//*************************************************************************************
//												OGGETTI GENERICI DEL DOCUMENT o FORM
//*************************************************************************************
//*************************************************************************************
 

//*************************************************************************************
//FUNZIONE : getObj
//           Restituisce il puntatore ad un oggetto della pagina
//           in modo indipendente dal BROWSER utilizzato
//PARAMETRI :
//sId       : Id dell'oggetto Layer di cui si vuole ottenere il puntatore
//blnUseParentWindow (OPZIONALE)
//          : se =1 usa la finestra principale
//            se =0 usa la finestra corrente (DEFAULT)
//UTILIZZO  : 
//          getObj('div1',1).bgColor='ff0000';
//
function getObj(sId,blnUseParentWindow)
{
	var blnUseParent=false;
	if(!isUndefined(blnUseParentWindow) && blnUseParentWindow!=0) blnUseParent=true;
		
	switch(intBrowser)
	{
		case BROWSER_NN4:
			break;

		case BROWSER_NN6:
			if (!blnUseParent)
				obj=document.getElementById(sId);						//caso di PAGINA CORRENTE
//				obj=eval('document.' + sId);							//
			else
				if(isWindow(top.opener))
					obj=top.opener.document.getElementById(sId);				//caso di POPUP
//					obj=eval('top.opener.document.' + sId);				//
				else
					obj=parent.document.getElementById(sId);	//caso di IFRAME o FRAME
//					obj=parent.document.getElementById(sId);	//
			break;

		default:					//IE
			if (!blnUseParent)
				obj=document.all[sId];						//caso di PAGINA CORRENTE
			else
				if(isWindow(top.opener))
					obj=top.opener.document.all[sId];				//caso di POPUP
				else
					obj=parent.document.all[sId];	//caso di IFRAME o FRAME
			break;
	}
	return obj;
}

//*****************************************************************************************
//ritorna l'oggetto corrispondente al campo della form indicata
//ricercandolo o nella pagina corrente o in quella principale (in caso di popup)
//sForm	: nome della form
//sField : nome del campo da ricercare
//blnUseParentWindow (OPZIONALE)
//       : se =1 usa la finestra principale
//         se =0 usa la finestra corrente
//UTILIZZO:
//es. getFormObj('frm1','txtnome',1).value='pippo';
//
function getFormObj(sForm,
											sField,
											blnUseParentWindow)
{
	var objField;
	if(isUndefined(blnUseParentWindow))
		blnUseParentWindow = false;
		
	if(blnUseParentWindow)	//CASO IN CUI I CAMPI SONO IN UNA PAGINA DI POPUP o FRAME, IFRAME
		if(!isWindow(top.opener))		
			objField = eval('parent.document.' + sForm + '.' + sField);
		else												
			objField = eval('top.opener.document.' + sForm + '.' + sField);
	else										// CASO IN CUI I CAMPI SONO SULLA STESSA PAGINA
		objField = eval('document.' + sForm + '.' + sField);
return objField;
}


//*****************************************************
//rende visibili o nascoste le DIV passate nella stringa in input
//a seconda dello stato voluto
//strListaDiv        : (Nome1-Nome2-Nome3)
//strListaStatoDiv   : (0-1-1) dove 1-> VISIBILE  e  0->NASCONDI
//blnUseParentWindow (OPZIONALE)
//                   : se =1 usa la finestra principale
//                     se =0 usa la finestra corrente
//blnUsePosition     (OPZIONALE)
//                   : se true indica di rendere static la posizione 
//                     della div quando non è visibile 
//                     (con questa impostazione quando la div non è visibile 
//                     NON occupa spazio nella pagina)

function showObj(
											 sObjId,
											 blnVisible,
											 blnUseParentWindow,
											 blnUsePosition)
{
	var i;
	var strVisibile= 'visible';
	var strNascosto= 'hidden';
	var blnUseStatic = false;
	
	if(!isUndefined(blnUsePosition))
		blnUseStatic = blnUsePosition;

	if (intBrowser==BROWSER_NN4)
	{
		strVisibile = 'show';
		strNascosto = 'hide';
	}
	if (!blnVisible) //NASCONDE
	{
		getObj(sObjId,blnUseParentWindow).style.visibility = strNascosto;
		if(blnUseStatic)
			getObj(sObjId,blnUseParentWindow).style.position='absolute';
	}
	else						//RENDE VISIBILE
	{
		getObj(sObjId,blnUseParentWindow).style.visibility = strVisibile;
		if(blnUseStatic)
			getObj(sObjId,blnUseParentWindow).style.position='static';
	}
}


//*************************************************************************************
//*************************************************************************************
//															LAYER o DIV
//*************************************************************************************
//*************************************************************************************


//*************************************************************************************
//FUNZIONE : LeggiLayer
//           Restituisce il puntatore all'oggetto STYLE di un LAYER 
//           in modo indipendente dal BROWSER utilizzato
//PARAMETRI :
//           (Vedi i parametri della funzione getLayer())
//UTILIZZO  : 
//           LeggiLayer('div1',1).position='static';
//NOTE      :
//           Per rendere visibili o nascosti dei layer o div usare RendiVisibile()
//           e non LeggiLayer('div1',1).visibility='hidden'; in quanto non compatibile
//           con Netscape4
//
//function getLayerStyle(sIdLayer,blnUseParentWindow)
function LeggiLayer(sIdLayer,blnUseParentWindow)
{
		switch(intBrowser)
		{
			case BROWSER_NN4:
				if (!blnUseParent)
					objLayer=document.layers[sId];										//caso di PAGINA CORRENTE
				else
					objLayer=top.opener.document.layers[sId];				//caso di POPUP
				if (!isUndefined(objLayer))
					return objLayer;
				return false;
				break;
				
			default:				//IE o NN >= 6
				var objLayer = getObj(sIdLayer,blnUseParentWindow);
				if (!isUndefined(objLayer))
					return objLayer.style;
				else
					return false;
				break;
		}
}

//*****************************************************
//rende visibili o nascoste le DIV passate nella stringa in input
//a seconda dello stato voluto
//strListaDiv        : (Nome1-Nome2-Nome3)
//strListaStatoDiv   : (0-1-1) dove 1-> VISIBILE  e  0->NASCONDI
//blnUseParentWindow (OPZIONALE)
//                   : se =1 usa la finestra principale
//                     se =0 usa la finestra corrente
//blnUsePosition     (OPZIONALE)
//                   : se true indica di rendere static la posizione 
//                     della div quando non è visibile 
//                     (con questa impostazione quando la div non è visibile 
//                     NON occupa spazio nella pagina)

//function ShowLayer(
function RendiVisibile(
											 strListaDiv,
											 strListaStatoDiv,
											 blnUseParentWindow,
											 blnUsePosition)
{
	var arrDIV = strListaDiv.split('-');
	var sTmp1 = strListaStatoDiv.toString();
	var arrStato = sTmp1.split('-');
	var i;
	var strVisibile, strNascosto;
	var blnUseStatic = false;
	
	if(!isUndefined(blnUsePosition))
		blnUseStatic = blnUsePosition;

	strVisibile = 'visible';
	strNascosto = 'hidden';
	
	//FUNZIONA SOLO CON EXPLORER O NN6!!!!
	if (intBrowser!=BROWSER_NN4)
	{
		if (arrDIV.length != arrStato.length)
		{
			alert("Errore nei parametri passati alla funzione RendiVisibile!!");
			return;
		}
			
		for (i=0 ; i<arrDIV.length ; i++)
		{
			if (arrStato[i]==0) //nasconde
			{
				LeggiLayer(arrDIV[i],blnUseParentWindow).visibility = strNascosto;
				if(blnUseStatic)
					LeggiLayer(arrDIV[i],blnUseParentWindow).position='absolute';
			}
			else //rende visibile
			{
				LeggiLayer(arrDIV[i],blnUseParentWindow).visibility = strVisibile;
				if(blnUseStatic)
					LeggiLayer(arrDIV[i],blnUseParentWindow).position='static';
			}
		}
	}
}

//*******************************************************************************************
//	FUNCTION   : isLayerVisible
//							 ritorna true se il LAYER è visibile, false se non visibile
//	PARAMETERS : 
//objLayer : nome del LAYER
//
function isLayerVisible(objLayer)
{
	st = LeggiLayer(objLayer).visibility;
	st = st.toLowerCase();
	
	if (st == 'show' || st == 'visible')
		return true;
	else
		return false;
}

//***************************************************************************************
//FUNCTION :	setLayerContent
//            Setta il contenuto (formato html) di una div. 
//            Utilizzata per visualizzare il risultato di una ricerca 
//            o visualizzare un messaggio senza dover aprire un POPUp
//idDivDest       : id della DIV di cui si vuole modificare il contenuto (INNERHTML)
//idIFrameSource  : id della IFRAME dalla quale leggere il contenuto (INNERHTML)
//strHtmlSource   : testo formato HTML (senza body) da copiare nella DIV
//
function setLayerContent(idDivDest,idIFrameSource,strHtmlSource) 
{
	var sIFrameCont;
	
	//VERIFICA PARAMETRI
	if(idDivDest=='')
		{	alert('showError:\n' + 'parametri errati\n E\' necessario passare idDivDest');	return;	}

	var objDiv    = getObj(idDivDest);
	if(idIFrameSource!='')
	{
		var objIFrame = getIFrame(idIFrameSource);
		switch(intBrowser)
		{
			case BROWSER_NN4:
				break;
			case BROWSER_NN6:
				var sIFrameCont = objIFrame.contentDocument.body.innerHTML;
				break;
			default:	//IE
				sIFrameCont = objIFrame.document.body.innerHTML;
				break;
		}
	}
	else
		sIFrameCont = strHtmlSource;
		
	objDiv.innerHTML = sIFrameCont;
}



//**************************************************************************************
//
//function newLayerI(strNomeLayer,blnVisibility,intTop, intLeft)
function CreaLayer(strNomeLayer,blnVisibility,intTop, intLeft)
{
	var strVisibility = "hidden";
	var strOutput;
	
	//if (intBrowser!=BROWSER_NN4)
	{
		if(!isUndefined(blnVisibility) && blnVisibility) 
			strVisibility = "visible";
			
		strOutput = "<DIV ";
		strOutput += " id='" + strNomeLayer + "'";
		strOutput += " style='visibility: " + strVisibility + ";";
		if(!isUndefined(intTop) && !isUndefined(intLeft))
		{
			strOutput += " z-index:11; position: absolute;";
			strOutput += " top: " + intTop + "px;";
			strOutput += " left: " + intLeft + "px;";
		}
		strOutput += "'";
		strOutput += ">";
	//alert(strOutput);
		document.writeln(strOutput);
	}
}

//*****************************************************************************************
//
//function newLayerF()
function ChiudiLayer()
{
	//if (intBrowser!=BROWSER_NN4)
		document.writeln("</DIV>");
}




//*************************************************************************************
//*************************************************************************************
//																IFRAME
//*************************************************************************************
//*************************************************************************************


//*************************************************************************************
//FUNZIONE : getIFrame
//           Restituisce il puntatore ad un oggetto getIFrame nella pagina
//           in modo indipendente dal BROWSER utilizzato
//PARAMETRI :
//sId       : Id dell'oggetto Layer di cui si vuole ottenere il puntatore
//blnUseParentWindow (OPZIONALE)
//          : se =1 usa la finestra principale
//            se =0 usa la finestra corrente (DEFAULT)
//UTILIZZO  : 
//          getIFrame('div1',1).bgColor='ff0000';
//
function getIFrame(sId,blnUseParentWindow)
{
	var blnUseParent=false;
	var obj;
	if(!isUndefined(blnUseParentWindow) && blnUseParentWindow!=0) blnUseParent=true;
		
	switch(intBrowser)
	{
		case BROWSER_NN4:
			break;

		case BROWSER_NN6:
			if (!blnUseParent)
				obj=document.getElementById(sId);						//caso di PAGINA CORRENTE
			else
				if(isWindow(top.opener))
					obj=top.opener.document.getElementById(sId);				//caso di POPUP
				else
					obj=parent.document.getElementById(sId);	//caso di IFRAME o FRAME
			break;

		default:					//IE
			if (!blnUseParent)
				obj=eval('document.' + sId);						//caso di PAGINA CORRENTE
			else
				if(isWindow(top.opener))
					obj=eval('top.opener.document.' + sId);				//caso di POPUP
				else
					obj=eval('parent.document.' + sId);	//caso di IFRAME o FRAME
			break;
	}
	return obj;
}

//***************************************************************************************
//Cambia il SOURCE di una IFRAME (in base al tipo di browser)
//
function setIFrameContent(strIFrame,url) 
{
	var objIFrame;
	switch (intBrowser) 
	{
		case BROWSER_NN4:
			var lyr = document.layers[strIFrame];
			lyr.load(url,lyr.clip.width)
			break;
		case BROWSER_NN6:
			//objIFrame=document.getElementById(strIFrame);
			objIFrame=getIFrame(strIFrame);
			objIFrame.src = url;
			break;
		default:	//IE
			//objIFrame=eval('document.' + strIFrame);
			objIFrame=getIFrame(strIFrame);
			objIFrame.location = url;
	}
}

//*****************************************************************************************
//Include una pagina in un'altra usando il tag IFRAME (per ora non compatibile con netscape)
function NewIFrame(id,width,height,border,scroll,src)
{
	var code="";
	//if(!top.opener)
	//	src=src+"&IFrame=true";
	//  IEXPLORER
	code=code + "<IFRAME";
	code=code + " ID=" + id;
	code=code + " name=" + id;
	code=code + " WIDTH=" + width; 
	code=code + " FRAMEBORDER=" + border; 
	code=code + " SCROLLING=" + scroll; 
	code=code + " SRC=" + src; 
	code=code + " HEIGHT=" + height; 
	code=code + "></IFRAME>"; 
	document.write(code);
}











//*************************************************************************************
//*************************************************************************************
//															FUNZIONI GENERICHE
//*************************************************************************************
//*************************************************************************************



//*****************************************************************************************
// crea una div in secondo piano per visualizzare 
// ad es. il riepilogo delle impostazioni filtri
function CreaBoxFiltroBegin(strTitle,strDivName,strDivNameClose,blnDivVis,intDivTop,intDivLeft,strPathImg)
// strDivNameClose nome della seconda div che fa le funzioni di "promemoria chiuso"
{
	var strOut;
	if(isUndefined(strPathImg))
		strPathImg='../';
		
	CreaLayer(strDivName,blnDivVis,intDivTop,intDivLeft);
	strOut = "<TABLE border=1 cellspacing=0 cellpadding=0 bordercolorlight=DarkBlue bordercolordark=#87cefa bgcolor=White id=tabMOVE>";
	strOut += "<tr><td>";
	strOut += "<TABLE border=0 cellspacing=0 cellpadding=0 align=center>";
	strOut += "<tr><td colspan=3>";
	strOut += "    <TABLE border=0 cellspacing=0 align=center width=100% bgcolor=#add8e6>";
	strOut += "			<tr>";
	strOut += "				<td width=9 nowrap align=center><a href=\"javascript:CambiaStato('"
	strOut += strDivName + "','" + strDivNameClose + "','" + strPathImg +"');\">";
	strOut += "						<img src=\"" + strPathImg + "_img/lstOrderByDESC.png\" border=0 name=\"imgfilter\"></a></td>";
	strOut += "				<td id=tdMOVE width=90% class=fltBoxTitle nowrap align=center ";
/*	strOut += " onmousemove=\"moveEl();\" ";
	strOut += " onselectstart=\"checkEl();\" ";
	strOut += " onmouseover=\"cursEl();\" ";
	
	strOut += " onmousedown=\"grabEl();\" ";
	strOut += " onmouseup=\"dropEl();\" ";
*/	
	strOut += ">" + strTitle + "</td>";
	strOut += "			</tr>";
	strOut += "		</table>";
	strOut += "</td></tr>";
	
	document.writeln(strOut);
}
//*****************************************************************************************
function CreaBoxFiltroEnd()
{
	var strOut;
	strOut = "</table></td></tr></table>";
	document.writeln(strOut);
	ChiudiLayer();
}


//*****************************************************************************************
//Funzione per controllo della tabella che visualizza le impostazione del filtro
//*****************************************************************************************
function CambiaStato(strDivName,strDivNameAttiva,strPathImg)
{
	if (!isUndefined(LeggiLayer('divFiltroDRAG')))
	{
		RendiVisibile(strDivName,'0');
		RendiVisibile(strDivNameAttiva,'1');
		if (document.images['imgfilter'].src.indexOf("lstOrderByDESC.png")!=-1)
			document.images['imgfilter'].src= strPathImg + "_img/lstOrderByASC.png";
		else
			document.images['imgfilter'].src= strPathImg + "_img/lstOrderByDESC.png"
		activeEl=LeggiLayer(strDivNameAttiva);
		moveNoDrag();	
	}
}
//*****************************************************************************************
//Crea e riempie una TextArea in modo compatibile con Netscape
function CreaTextArea(strText,strStyle,intRows,intCols,blnEnabled,intMaxCharXRow)
{
	var strOutput="<TEXTAREA";
	var re=/<BR>/gi;

	//Crea una textarea solo se il numero di caratteri per riga supera quello indicato
	if(!isUndefined(intMaxCharXRow) && !isUndefined(strText))
		if(strText.length <= intMaxCharXRow && strText.indexOf("<BR>")==-1)
		{
			if(strText.length==0) strText += "&nbsp;";
			document.writeln(strText);
			return;
		}
		
	if (intBrowser!=BROWSER_NN4) strOutput += "";
	
	if(!isUndefined(blnEnabled) && blnEnabled==false)	
		strOutput += " onfocus='this.blur();'";
		
	if(!isUndefined(strStyle) && strStyle!='')	strOutput += " class=" + strStyle;
	if(!isUndefined(intRows) && !isNaN(intRows))		
	{
		//if (intBrowser!=BROWSER_NN4) intRows = intRows/2;
		strOutput += " rows=" + intRows;
	}
	if(!isUndefined(intCols) && !isNaN(intCols))		
	{
		if (intBrowser==BROWSER_NN4) intCols = intCols/2;
		strOutput += " cols=" + intCols;
	}
	strOutput += ">";
	if(!isUndefined(strText)) strOutput += strText.replace(re,'\n');
	strOutput += "</TEXTAREA>";
	
	document.writeln(strOutput);
}


//*****************************************************************************************
// Retrieve the value of the cookie with the specified name.
function GetCookie(sName)
{
  // cookies are separated by semicolons
  var aCookie = document.cookie.split("; ");
  for (var i=0; i < aCookie.length; i++)
  {
    // a name/value pair (a crumb) is separated by an equal sign
    var aCrumb = aCookie[i].split("=");
    if (sName == aCrumb[0]) 
      return unescape(aCrumb[1]);
  }

  // a cookie with the requested name does not exist
  return '';
}

//*****************************************************************************************
// Delete the cookie with the specified name.
function DelCookie(sName)
{
  document.cookie = sName + "=" + escape(sValue) + "; expires=Fri, 31 Dec 1999 23:59:59 GMT;";
}

//*****************************************************************************************
// Create a cookie with the specified name and value.
// The cookie expires at the end of the 20th century.
function SetCookie(sName, sValue, sPath)
{
	today = new Date();
	year = today.getFullYear()+1;
	date = new Date(year,11,31); //31/12 dell'anno successivo
	
	strTmp = sName + "=" + escape(sValue) + "; expires=" + date.toGMTString() + ";";
	
	if (!(isUndefined(sPath)))
		strTmp = strTmp + " path=" + sPath + ";";
	
	document.cookie = strTmp;
}

//*****************************************************************************************
//Se pagina in popup, carica url nello stesso popup
//altrimenti (per es. nel caso di IFRAME) carica url in un nuovo popup
// objClose : oggetto hdnClose, utilizzato per non rinfrescare il layer /*Laura*/
function LoadPagePopup(url,bars,objClose)
{
	var param;
	var url;
	
	if (!isUndefined(objClose))
			objClose.value='0';
			
	if (isWindow(top.opener))
		window.location=url;
	else
		{
		param='height=480,width=650,top=55,left=110,status=no,toolbar=no,menubar=no,resizable=yes,location=no';
		if (bars==true)
			param=param + ',scrollbars=yes';
		window.open(url,'wp',param);
		}
}

//*****************************************************************************************
//Gestione di due listbox (tutte le opzioni, solo le opzioni selezionate): 
//sposta opzione scelta dalla lista completa alla lista delle opzioni selezionate
function ShiftToSel(objAll,objSel,strMsg)
{
	if (objAll.selectedIndex < 0)
	{
		alert('Selezionare un ' + strMsg); 
		return false;
	}
	else
	{
		var lngValue=objAll.options[objAll.selectedIndex].value;
		var strText=objAll.options[objAll.selectedIndex].text;
		var lngNumSel = objSel.length;
		if (lngNumSel>0 && objSel.options[0].value=="-1")
				objSel.options[0] = null;
		lngNumSel = objSel.length;
		objSel.options[lngNumSel] = new Option(strText,lngValue);
		objAll.options[objAll.selectedIndex] = null;
		return true
	}
}

//*****************************************************************************************
//Gestione di due listbox (tutte le opzioni, solo le opzioni selezionate): 
//sposta opzione scelta dalla lista delle opzioni selezionate alla lista di tutte le opzioni
function ShiftToAll(objAll,objSel,strMsg)
{
	if (objSel.selectedIndex < 0)
	{
		alert('Selezionare un ' + strMsg);
		return false;
	}
	else
	{
		var lngValue=objSel.options[objSel.selectedIndex].value;
		var strText=objSel.options[objSel.selectedIndex].text;
		var lngNumAll = objAll.length;
		objAll.options[lngNumAll] = new Option(strText,lngValue);
		objSel.options[objSel.selectedIndex] = null;
		return true
	}
}	

//*****************************************************************************************
//CreaTabPercentuale crea una tabella che visualizza graficamente
//la percentuale tra i valori forniti
function CreaTabPercentuale(strStyleTab,        //stile della tabella
														strTabPercProgress, //stile del numero percentuale scritto a fianco
														dblVal1,            //valore totale
														dblVal2,            //valore attuale
														strColorPieno,      //colore della percentuale piena (fino al 75%)
														strColorPieno75,    //colore della percentuale piena (dal 75% al 99%)
														strColorPieno99,    //colore della percentuale piena (oltre il 99%)
														strColorVuoto,      //colore della percentuale vuota
														intWidth            //larghezza della tabella
														)
{
	var strOutput ;
	var lngPerc ;
	var lngInd ;
	var lngDiff ;
	var strPerc ;
	
	//calcolo della percentuale arrotondato ad un intero
	lngPerc = (dblVal1 > 0) ? parseInt(((dblVal2 * 100) / dblVal1)) : -1 ;

	//se la percentuale e' > 999 viene scritto ">999";
	//se dblVal1 e' <= 0 viene scritto "nd" (non definito);
	//altrimenti la percentuale vera e propria
	lngInd = lngPerc ;
	if (lngPerc == -1)
		{
		lngInd = 0 ;
		strPerc = "nd" ;
		}
	else
		if (lngPerc > 999)
			{
			strPerc = ">999" ;
			}
	else strPerc = eval(lngInd) + "%" ;

	strColorPieno =
		((eval(lngInd) < 75) ? strColorPieno : ((eval(lngInd) < 99) ? strColorPieno75 : strColorPieno99)) ;
	strOutput =	"<table border=0 width=100% cellpadding=0 cellspacing=0>\n" ;
	strOutput += "	<tr>\n" ;
	strOutput += "		<td width=" + intWidth + ">\n" ;
	strOutput += "			<table border=1 width=100% cellpadding=0 cellspacing=0\n" ;
	strOutput += "						bordercolor=Black bordercolorlight=White>\n" ;
	strOutput += "				<tr>\n" ;
	strOutput += "					<td>\n" ;
	strOutput += "						<table border=0 height=8 width=100% cellpadding=0 cellspacing=0>\n" ;
	strOutput += "							<tr>\n" ;
	if (lngInd > 0) 
		strOutput += "									<td align=right width=" + lngInd + "% bgcolor=" + strColorPieno + " class=" + strStyleTab + "></td>\n" ;
	lngDiff = 100 - lngInd ;
	strOutput += "								<td bgcolor=" + strColorVuoto + " width=" + lngDiff + "% class=" + strStyleTab + "></td>\n" ;
	strOutput += "							</tr>\n" ;
	strOutput += "						</table>\n" ;
	strOutput += "					</td>\n" ;
	strOutput += "				</tr>\n" ;
	strOutput += "			</table>\n" ;
	strOutput += "		</td>\n" ;
	strOutput += "		<td align=right class=" + strTabPercProgress + "> " + strPerc + "</td>\n" ;
	strOutput += "	</tr>\n" ;
	strOutput += "</table>\n" ;
		
//alert(strOutput);
	document.writeln(strOutput) ;
}

//*****************************************************************************************
//CreateOption crea un elemento vuoto al fondo di una Select, se è vuoto

function CreateOption(objSel		//oggetto Select
											)
{
	if (objSel.length != 0)
	{
		if (objSel.options[objSel.length - 1].value != '')
			objSel[objSel.length] = new Option();
	}
	else
		objSel[objSel.length] = new Option();
}

//*****************************************************************************************
//UpdateLista aggiorna l'ultimo elemento 'Option' di una Select con i valori passati

function UpdateLista(objLista,	//oggetto Select
										lngID,			//variabile x il 'value' dell'Option
										strTxt,			//variabile x il testo dell'Option
										blnclose,   //boolean x fare o meno la close window
										blnIncr)   //boolean: true aggiorna la lunghezza del combo
{
	objLista.options[objLista.length - 1].text = strTxt;
	objLista.options[objLista.length - 1].value = lngID;
	if (isUndefined(blnclose)) blnclose = true ;
	if (isUndefined(blnIncr))  blnIncr = false ;
	if (blnclose)  
	  window.close();
	 else 
	 if (blnIncr)  
		objLista.length = objLista.length +1;
}

//*****************************************************************************************
//RemoveItem rimuove 1 elemento di una lista di tipo Select (se selezionato)

function RemoveItem(objLista		//oggetto Lista
										)
{
	if (objLista.selectedIndex == '-1')
		alert("Seleziona un elemento dalla Lista per cancellarla");
	else
		objLista.options[objLista.selectedIndex] = null;
	if (objLista.length>0)
		objLista.selectedIndex=0;
}

//*****************************************************************************************
//GetElements ritorna in 2 campi hidden una stringa di ID e una di 'nomi'
//contenuti in una lista di tipo Select

function GetElements(objLista,	//oggetto Select
										hdID,				//oggetto hidden x gli ID
										hdNomi			//oggetto hidden x i 'nomi'
										)
{
	i = 1;
	
	for (i; i <= objLista.length; i++)
	{
		if (objLista[i - 1].value != '')
		{
			if (hdID.value == '')
				hdID.value = objLista[i - 1].value;
			else
				hdID.value = hdID.value + strSep + objLista[i - 1].value;
			
			if (hdNomi.value == '')
				hdNomi.value = objLista[i - 1].text;
			else
				hdNomi.value = hdNomi.value + strSep + objLista[i - 1].text;
		}
	}
}

//*****************************************************************************************

//ritorna il numero fornito come parametro modificato in base alle impostazioni del client
function CToNum(strNum,blnSosVirgola)
{
	var rev = /,/gi ;
//viene protetto il punto perche' viene visto probabilmente come carattere speciale
	var rep = /\./gi ;
	var len ;
	var i1 ;

	strNum = strNum.toString() ;
	len = strNum.length ;
//alert("Inizio: strNum " + strNum + " blnSos " + blnSosVirgola + " len " + len) ;
	if (isUndefined(blnSosVirgola)) blnSosVirgola = true ;
	if (len > 0)
		if (blnSosVirgola)
			{
			strNum = strNum.replace(rep,'') ;
			strNum = strNum.replace(rev,'.') ;
			}
		else
			strNum = strNum.replace(rep,',') ;
//alert("Fine: strNum " + strNum) ;
	return((len) ? strNum : "0") ;
}

//*****************************************************************************************

//mantiene "NDec" cifre decimali
//DecRequired = 1 --> inserisce degli 0 se il numero di cifre decimali è minore della lunghezza richiesta (nMaxNumDecimal)

function FormatNum(Num,NDec,DecRequired)
{	
	var i;
	var sZero = '0';
	iNum = Num;
	Num = Num.toString();
	pos = Num.indexOf(".");
	len = Num.length;
	
	if(isUndefined(NDec))
		NDec = 2;
		
	if(isUndefined(DecRequired))
		DecRequired = 0;

	if (len-pos > NDec && pos > 0)
	{
		iNum = Math.round(iNum * Math.pow(10,NDec)) / Math.pow(10,NDec);
		Num = iNum.toString();
		Num = Num.substr(0, pos + NDec + 1);
	}
	if(eval(DecRequired) == 1)		
		{
		len = Num.length;
		if(len-pos < NDec )
			{	//non ci sono abastanza cifre decimali
			if(pos<0)
				{	//manca il punto decimale --> numero intero
				len = NDec;
				Num = Num + '.';
				}
			else	//numero non intero
				len = NDec - (len-pos) + 1;
				
			for(i=0;i<len;i++)
				{
				Num = Num + sZero;
				}
			}	
		}
	return Num;
}
//*****************************************************************************************

function SelezionaID (objcmb,isel)
//objcmb : oggetto combo
//isel   : valore dell'elemento selezionato
{
	if (isel>0)
	{
		for (var i=0;i<objcmb.length;i++)
			if (objcmb.options[i].value==isel) 
			{
				objcmb.options[i].selected=true;   
				break;
			}
	}
}

//*****************************************************************************************
function RiempicmbMat(ObjCmb,sVett) 
//ObjCmb : oggetto combo
//sVett	 : nome della matrice da utilizzare per riempire il combo
	{   
		var ObjVett= eval(sVett);
		if (ObjCmb.length >1)
		  Svuotacmb(ObjCmb);
		len=ObjVett.length-1;
		for (i=1; i <= len; i++) 
			ObjCmb.options[i]=new Option(ObjVett[i-1][1],ObjVett[i-1][0]);
	}

//*****************************************************************************************	
function Svuotacmb(objcmb,numElminin)
//svuota una combo
//numElminin : numero di Elementi che devono essere presenti nella combo dopo la cancellazione
{
	if (isUndefined(numElminin))
		numElminin=1;
	if (objcmb.length>numElminin)
	{
		for (i=numElminin;i<objcmb.length;i++)
			objcmb[i]=null;
		objcmb.length=numElminin;
		objcmb.selectedIndex=numElminin-1;
	}
}

//*****************************************************************************************
function isObject(w)
{
	var bIsObj;		
  if( w == null ) return false;       
  switch(intBrowser)
  {
		case	BROWSER_NN4:
			bIsObj=((''+eval(w)).substring(0,7)=='[object'); 
			break;
		case	BROWSER_NN6:
			bIsObj=((''+document.getElementById(w)).substring(0,7)=='[object'); 
			break;
		default:
				bIsObj=((''+document.all[w]).substring(0,7)=='[object'); 
			
	}
  if (!bIsObj)  return false;
	return true;
}

//*****************************************************************************************

function OpenPopup(strApp,strUrl,strName,strWidth,strHeight,offX,offY,
					strRes,strScroll,strToolb,strStat,strMenuBar,strfullscreen)
// strApp nome applicazione
// offX offset orizzontale rispetto al centro
// offY offset verticale rispetto al centro
// strRes resizable?
	{
		var rep = /-/g ;
		var hdlWindow=null;	
	
		if (strApp.length==0)
		{
			alert('errore nel passaggio dei parametri (Nome Applicazione)');
			return;
		}	
			
		if (strUrl.length==0)
		{
			alert('errore nel passaggio dei parametri (Url)');
			return;
		}	
		if (strName.length==0)
		{
			alert('errore nel passaggio dei parametri (Name)');
			return;
		}	
		if (isNaN(strWidth) || strWidth<=0)
		{
			alert('errore nel passaggio dei parametri (larghezza)');
			return;
		}
		if (isNaN(strHeight) || strHeight<=0)
		{
			alert('errore nel passaggio dei parametri (lunghezza)');
			return;
		}
		strName=strApp+strName;
		if (isUndefined(offX))
			offX=0;
		if (isUndefined(offY))
			offY=0;	
		if (isUndefined(strRes))
			strRes="yes";
		if (isUndefined(strScroll))
			strScroll="yes";	
		if (isUndefined(strToolb))
			strToolb="no";	
		if (isUndefined(strStat))
			strStat="no";	
		if (isUndefined(strMenuBar))
			strMenuBar="no";
		if (isUndefined(strfullscreen))
			strfullscreen="no";
		var posx=(screen.availWidth - strWidth-10)/2+ eval(offX);
		var posy=(screen.availHeight  - strHeight-30)/2+ eval(offY);
		var strFeatures="width=" + strWidth + ",";
		strFeatures+="height=" + strHeight + ",";
		strFeatures+="left=" + posx + ",";
		strFeatures+="top=" + posy + ",";
		strFeatures+="resizable="+ strRes + ",";
		strFeatures+="scrollbars="+ strScroll + ",";
		strFeatures+="toolbar="+ strToolb + ",";
		strFeatures+="status="+ strStat+ ",";
		strFeatures+="menubar="+ strMenuBar+",";
		strFeatures+="fullscreen=" + strfullscreen;	
		strName = strName.replace(rep,'_') ;
		hdlWindow=window.open(strUrl,strName,strFeatures);
		hdlWindow.focus();
		return hdlWindow;
	}

//********************************************************************

function RestoreDefaultSelection(objSel)
{
	for (i = 0; i < objSel.length; i++)
		if (objSel.options[i].defaultSelected == true)
			objSel.options[i].selected = true
}

// Analoga di quella presente in vb e vbscript 
// pone in un vettore gli elementi della stringa separati da strSeparator
// return di un vettore quindi chiamarla come es. objVet=SplitValue(...
function SplitValue(vrnInputParam, strSeparator) 
{
  var lngPos1, lngPos2, lngPosBegin, lngParamCount, vetParamOut, intLenSeparator, strInput;
  
  strInput = vrnInputParam;
  vetParamOut=new Array();
  intLenSeparator = strSeparator.length;
  lngPos1 = 1;
  lngParamCount = 0;
  lngPosBegin = 0;
  do 
  {
    lngPos2 = strInput.indexOf(strSeparator,lngPos1);
    if (lngPos2 <= 0)
    {
      vetParamOut[lngParamCount] = strInput.substr(lngPosBegin);
      break;
    }
    /*Verifica che il carattere successivo non sia lo stesso carattere
    del separatore, in questo caso non sarebbe un separatore valido ma
    un carattere raddoppiato (vedi byte stuffing)*/
    if (strInput.substr(lngPos2 + 1, intLenSeparator) == strSeparator)
    {
      //sposta il puntatore dopo il carattere duplicato
      lngPosBegin = lngPos1;
      lngPos1 = lngPos2 + intLenSeparator + 1;
    }
    else
    {
			strProv=strInput.substr(lngPosBegin, lngPos2 - lngPosBegin);
			vetParamOut[lngParamCount] = strProv.replace(RegExp(strSeparator + strSeparator,"gi"), strSeparator)
			lngParamCount = lngParamCount + 1;
			lngPos1 = lngPos2 + intLenSeparator;
			lngPosBegin = lngPos1;
		}
	}
	while (true)
	return vetParamOut;
}

function getFilename(strPathName)
{
	var nome;
	var Len;
	var i = 0;
	nome = "";
	
	if(isUndefined(strPathName))
		return "";
		
	Len = strPathName.length;
	for(i=Len; i >= 0; i--)
	{
		if(strPathName.charAt(i) == '\\' || strPathName.charAt(i) == '/')
			break;
	}
	i++;
	nome = nome + strPathName.substr(i);
	return nome;
}

//*****************************************************************************************
//controlla se la colonna ordinata è quella corrente
//
function CheckOrd(col, tipoOrd, bOrd )
//col: nome colonna, tipoOrd: ordinamento attuale, bOrd: ordine asc o desc 
{
	if (col==tipoOrd)
		return !bOrd;
	else
		return true;
}

//*****************************************************************************************
//controlla se il campo di testo è vuoto
//
function IsBlank(sField)
{
	var bChar=0;
	if (sField) 
		for (var i=0; i<sField.length; i++) 
		{
			//altro 160 codice per lo spazio??? 
			if (sField.charAt(i) != " " && sField.charCodeAt(i)!=160) {
				bChar = 1;
				break;
			}
		}
	if (bChar==0)
		return true;
	else
		return false;
}

//*****************************************************************************************
function show_props(obj, objName)
{   
	var result = "" ;
	var i=0;
	for (var el in obj)
	{
		result += objName + "." + el + " = " + obj[el] + "<br>";
//		if((i%2)==0) result +="\n";
		i++;
		
	}
	
	return result;
}
//*****************************************************************************************

// incrementa o decrementa il numero di pagina
	function Naviga(obj_form)
	{
		with(obj_form)
		{
			//uso pulsanti < oppure >
			if (hdIncr.value=='1')
			{
				NumPag.value=parseInt(NumPag.value) + 1;
				hdIncr.value=="";
				return true;
			}
			else	
				if (hdDecr.value=='1')
				{
					NumPag.value=parseInt(NumPag.value) - 1;
					hdDecr.value=="";
					return true;
				}
			
			if (Number(txtNumPag.value) != Number(hdCurPage.value))
			{
				//uso la textbox
				if (isNaN(txtNumPag.value) || Number(txtNumPag.value) <= 0 || txtNumPag.value.indexOf(".") >= 0 ||
							txtNumPag.value.indexOf(",") >= 0 || txtNumPag.value == '' ||
					(txtNumPag.value.length > 1 && txtNumPag.value.charAt(0) == '0')
					
					)
				{
					alert('Inserire un numero valido!');
					txtNumPag.value = hdCurPage.value;
					return false;
				}
				else
				{
					if (Number(txtNumPag.value) > Number(hdNTotPage.value))
					{
						alert('Pagina non esistente!');
						txtNumPag.value = hdCurPage.value;
						return false;
					}
					else
						NumPag.value = txtNumPag.value;
				}
			}
			
			//uso pulsanti << oppure >>
			//....
			
			return true;
		}
	}
	
//*****************************************************************************************

//capovolge stringa
function Reverse(strToReverse)
{
	var strRev = new String;
	var i = strToReverse.length;
	while (i--)
		strRev += strToReverse.charAt(i);
	
	return strRev;
}

//*****************************************************************************************

//restituisce x caratteri a destra
function Right(str, num)
{
 var strRight = new String(str);
 strRight = Reverse(strRight);
 return Reverse(strRight.substr(0, num));
}

//*****************************************************************************************

//restituisce x caratteri a sinistra
function Left(str, num)
{
 var strLeft = new String(str);
 return strLeft.substr(0, num);
}

//*****************************************************************************************

function isWindow(w)
{
	if( w == null ) return false;
	var bIsObj=((""+eval(w)).substring(0,7)=="[object");
	if (!bIsObj)	return false;
	if (w.closed)	return false;
	return true;
}

//*****************************************************************************************

function StrURL(strIn)
{
	return(escape(strIn));
}


function StrURLToJs(strIn)
{
	return(escape(strIn));
}

//******************************************************************************************
//questa funzione serve per verificare se una variabile è stata inizializzata o no
//in particolare serve per capire se un parametro di una funzione è stato passato o no
//per default tutti i parametri in Javaascript sono opzionali e per defult contengono undefined
function isUndefined(strField)
{
	if( typeof(strField) == "undefined")
		return true;
	else 
		return false;
}

//*****************************************************************************************
//Legge un parametro da  riga di comando (URL)
function GetParameter(strParamName)
{
	var msgErr;
	var strQueryString;
	var lngBeginPos;
	var lngEndPos;
	var rePlus=/\+/g;
	
	strParamName = strParamName + "=";
	
	//cambio i '%n' con i caratteri corrispettivi
	strQueryString = unescape(location.search);
	
	//replace dei caratteri + con uno SPAZIO, 
	//la Server.URLEncode() codifica uno spazio in un +
	strQueryString = strQueryString.replace(rePlus," ");
	
	//cerco il valore corrispondente alla variabile URL
	lngBeginPos = strQueryString.indexOf(strParamName);
	if (lngBeginPos == -1) 
	{	//cerco la stessa variabile senza il case sensitive del JS
		strParamName = strParamName.toLowerCase();
		lngBeginPos = strQueryString.indexOf(strParamName);
		if (lngBeginPos == -1) return "";	//stringa non trovata
	}
	
	//aggiungo la lunghezza della stringa cercata sopra
	lngBeginPos = lngBeginPos + strParamName.length;
	
	//cerco il carattere & successivo
	lngEndPos = strQueryString.indexOf("&", lngBeginPos);
								
	// -1 sgnifica che l'elemento della queryString è alla fine dell'URL..
	if (lngEndPos == -1)
		lngEndPos = strQueryString.length;
								
	//estraggo e visualizzo il messaggio
	msgErr = strQueryString.substr(lngBeginPos, (lngEndPos - lngBeginPos));
	
	return msgErr;
}





//-->