
function switchSceltaInserimento(tipo)
{
	if (tipo == "cartellazione")
	{
		document.getElementById("divCartellazione").style.display = '';
		document.getElementById("divDataEntry").style.display = 'none';
	}
	if (tipo == "dataEntry")
	{
		document.getElementById("divCartellazione").style.display = 'none';
		document.getElementById("divDataEntry").style.display = '';	
	}
}

/******************* Apre Pop Up Stradario ***************/

function ApriStradario(Url,FunzioneRitorno, CodEnte)
{					
	var TipoStrada = '';
	var Strada = '';
	var CodStrada = '';
	var CodTipoStrada = '';
	var Frazione = '';
	var CodFrazione = '';

	var Parametri = '';

	Parametri += 'CodEnte='+CodEnte;
	Parametri += '&TipoStrada='+TipoStrada;
	Parametri += '&Strada='+Strada;
	Parametri += '&CodStrada='+CodStrada;
	Parametri += '&CodTipoStrada='+CodTipoStrada;
	Parametri += '&Frazione='+Frazione;
	Parametri += '&CodFrazione='+CodFrazione;
	Parametri += '&Stile=<% = HttpContext.Current.Session("StileStradario") %>';
	Parametri += '&FunzioneRitorno='+FunzioneRitorno;
	
	window.open('' + Url +'?'+Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');
	return false;
}

/********** Funzione richiamata da Stradario **************/

function RibaltaStrada(objStrada)
{
	// popolo il campo descrizione della via di residenza
	var strada
	if (objStrada.TipoStrada != '&nbsp;')
	{
		strada= objStrada.TipoStrada;
	}
	if (objStrada.Strada != '&nbsp;')
	{
		strada=strada+ ' ' + objStrada.Strada;
	}
	if (objStrada.Frazione!='CAPOLUOGO')
	{
		strada= strada+ ' ' + objStrada.Frazione;
	}
    /*strada = strada.replace('&#192;', 'A\'');*/
	strada = strada.replace('&#192;', 'À').replace('&#193;', 'Á').replace('&#194;', 'Â').replace('&#195;', 'Ã').replace('&#196;', 'Ä').replace('&#197;', 'Å').replace('&#224;', 'à').replace('&#225;', 'á').replace('&#226;', 'â').replace('&#227;', 'ã').replace('&#228;', 'ä').replace('&#229;', 'å').replace('&#200;', 'È').replace('&#201;', 'É').replace('&#202;', 'Ê').replace('&#203;', 'Ë').replace('&#232;', 'è').replace('&#233;', 'é').replace('&#234;', 'ê').replace('&#235;', 'ë').replace('&#204;', 'Ì').replace('&#205;', 'Í').replace('&#206;', 'Î').replace('&#207;', 'Ï').replace('&#236;', 'ì').replace('&#237;', 'í').replace('&#238;', 'î').replace('&#239;', 'ï').replace('&#210;', 'Ò').replace('&#211;', 'Ó').replace('&#212;', 'Ô').replace('&#213;', 'Õ').replace('&#214;', 'Ö').replace('&#216;', 'Ø').replace('&#240;', 'ð').replace('&#242;', 'ò').replace('&#243;', 'ó').replace('&#244;', 'ô').replace('&#245;', 'õ').replace('&#246;', 'ö').replace('&#217;', 'Ù').replace('&#218;', 'Ú').replace('&#219;', 'Û').replace('&#220;', 'Ü').replace('&#249;', 'ù').replace('&#250;', 'ú').replace('&#251;', 'û').replace('&#252;', 'ü');
	document.getElementById('wucArticolo_TxtCodVia').value = objStrada.CodStrada;
	document.getElementById('wucArticolo_TxtVia').value = strada;
	document.getElementById('wucArticolo_TxtViaRibaltata').value = strada;
}




/******************* Apre Pop Up Ricerca Anagrafica ***************/

function ApriRicercaAnagrafe(nomeSessione)
{ 
	winWidth=980 
	winHeight=680 
	myleft=(screen.width-winWidth)/2 
	mytop=(screen.height-winHeight)/2 - 40 
	Parametri="sessionName=" + nomeSessione
	WinPopUpRicercaAnagrafica=window.open("../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?"+Parametri,"","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no")
}



function ShowInsertRidEse()
{ 
	winWidth=690 
	winHeight=200 
	myleft=(screen.width-winWidth)/2 
	mytop=(screen.height-winHeight)/2 - 40 
	WinPopRidEse=window.open("./PopUpRiduzioni.aspx","","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
}

function ValidatePagamentiForm(controlStringArray, controlStringMessageArray)
{
	if(document.getElementById(rdbDataEntry).checked) {
		return ValidatePagamenti(controlStringArray, controlStringMessageArray);		
	} else {
		//alert('E\' necessario selezionare una rata.');
		//return false;
	}
}

function ValidatePagamenti(controlStringArray, controlStringMessageArray)
{	
	try
	{	
		var formControlsArray=controlStringArray.split(";");
		var messaggeWarningArray = controlStringMessageArray.split(";")
		
		var msg = "";
		var obj;

		for (i = 0; i < formControlsArray.length; i++) {
		    if (formControlsArray[i] != '') {
		        obj = document.getElementById(formControlsArray[i]);

		        if (obj.type == "text") {
		            if (obj.value == '' || obj.value == '-1') {
		                msg += messaggeWarningArray[i] + '\n';
		                obj.style.backgroundColor = "#fffacd";
		            }
		            else obj.style.backgroundColor = "white";
		        }
		        else if (obj.type.indexOf("select") != -1) {
		            var objValue = obj.options[obj.selectedIndex].value;
		            if (objValue == '' || objValue == '-1') {
		                msg += messaggeWarningArray[i] + '\n';
		                obj.style.backgroundColor = "#fffacd";
		            }
		            else obj.style.backgroundColor = "white";
		        }
		    }
		}
		if (msg != '') 
		{
			msg = 'Attenzione i seguenti dati sono obbligatori:' + '\n\n' + msg;
			alert (msg);
			return false;
		}
		else		
			return true;
	}
	catch(e)
	{
		alert('Javascript error:' + e.message);
		return false;
	}
}

//Controlla i dati obbligatori ricevendo due stringe con controllo e messaggio;
//ogni valore è separato da ;
function ValidateForm(controlStringArray, controlStringMessageArray)
{		
	try
	{	
		var formControlsArray=controlStringArray.split(";");
		var messaggeWarningArray = controlStringMessageArray.split(";")
		
		var msg = "";
		var obj;
		

		for (i=0;i<formControlsArray.length;i++)
		{
			obj = document.getElementById(formControlsArray[i]);
			
			//alert(obj.id + ":" + obj.value);
					
			if (obj.type=="text")
			{
				if (obj.value == '' || obj.value == '-1')
				{
					msg += messaggeWarningArray[i] + '\n';
					obj.style.backgroundColor="#fffacd";
				}
				else obj.style.backgroundColor="white";
			}
			else if (obj.type.indexOf("select") != -1)
			{
				var objValue = obj.options[obj.selectedIndex].value;
				if (objValue == '' || objValue == '-1')
				{
					msg += messaggeWarningArray[i] + '\n';
					obj.style.backgroundColor="#fffacd";
				}
				else obj.style.backgroundColor="white";
			}
			
			
		}
			
		if (msg != '') 
		{
			msg = 'Attenzione i seguenti dati sono obbligatori:' + '\n\n' + msg;
			alert (msg);
			return false;
		}
	}
	catch(e)
	{
		alert('Javascript error:' + e.message);
		return false;
	}

}


function number_format (number, decimals, dec_point, thousands_sep) {
	// Formats a number with grouped thousands
	//
	// version: 906.1806
	// discuss at: http://phpjs.org/functions/number_format
	// +   original by: Jonas Raoni Soares Silva (http://www.jsfromhell.com)
	// +   improved by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
	// +     bugfix by: Michael White (http://getsprink.com)
	// +     bugfix by: Benjamin Lupton
	// +     bugfix by: Allan Jensen (http://www.winternet.no)
	// +    revised by: Jonas Raoni Soares Silva (http://www.jsfromhell.com)
	// +     bugfix by: Howard Yeend
	// +    revised by: Luke Smith (http://lucassmith.name)
	// +     bugfix by: Diogo Resende
	// +     bugfix by: Rival
	// +     input by: Kheang Hok Chin (http://www.distantia.ca/)
	// +     improved by: davook
	// +     improved by: Brett Zamir (http://brett-zamir.me)
	// +     input by: Jay Klehr
	// +     improved by: Brett Zamir (http://brett-zamir.me)
	// +     input by: Amir Habibi (http://www.residence-mixte.com/)
	// +     bugfix by: Brett Zamir (http://brett-zamir.me)
	// *     example 1: number_format(1234.56);
	// *     returns 1: '1,235'
	// *     example 2: number_format(1234.56, 2, ',', ' ');
	// *     returns 2: '1 234,56'
	// *     example 3: number_format(1234.5678, 2, '.', '');
	// *     returns 3: '1234.57'
	// *     example 4: number_format(67, 2, ',', '.');
	// *     returns 4: '67,00'
	// *     example 5: number_format(1000);
	// *     returns 5: '1,000'
	// *     example 6: number_format(67.311, 2);
	// *     returns 6: '67.31'
	// *     example 7: number_format(1000.55, 1);
	// *     returns 7: '1,000.6'
	// *     example 8: number_format(67000, 5, ',', '.');
	// *     returns 8: '67.000,00000'
	// *     example 9: number_format(0.9, 0);
	// *     returns 9: '1'
	// *     example 10: number_format('1.20', 2);
	// *     returns 10: '1.20'
	// *     example 11: number_format('1.20', 4);
	// *     returns 11: '1.2000'
	// *     example 12: number_format('1.2000', 3);
	// *     returns 12: '1.200'
	
	if (number!=null && number != '')
	{
	
	
	
	
		number = number.replace(",",".")
		
		var n = number, prec = decimals;
	
		var toFixedFix = function (n,prec) {
		    var k = Math.pow(10,prec);
		    return (Math.round(n*k)/k).toString();
		};
	
		n = !isFinite(+n) ? 0 : +n;
		prec = !isFinite(+prec) ? 0 : Math.abs(prec);
		var sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep;
		var dec = (typeof dec_point === 'undefined') ? '.' : dec_point;
	
		var s = (prec > 0) ? toFixedFix(n, prec) : toFixedFix(Math.round(n), prec); //fix for IE parseFloat(0.55).toFixed(0) = 0;
	
		var abs = toFixedFix(Math.abs(n), prec);
		var _, i;
	
		if (abs >= 1000) {
		    _ = abs.split(/\D/);
		    i = _[0].length % 3 || 3;
	
		    _[0] = s.slice(0,i + (n < 0)) +
		          _[0].slice(i).replace(/(\d{3})/g, sep+'$1');
		    s = _.join(dec);
		} else {
		    s = s.replace('.', dec);
		}
	
		var decPos = s.indexOf(dec);
		if (prec >= 1 && decPos !== -1 && (s.length-decPos-1) < prec) {
		    s += new Array(prec-(s.length-decPos-1)).join(0)+'0';
		}
		else if (prec >= 1 && decPos === -1) {
		    s += dec+new Array(prec).join(0)+'0';
		}
		
		return s; 
		}
		else
			return '';

}

function numonly(root){
	root.value = root.value.replace(" ","");
	root.value = root.value.replace(".","");
    var reet = root.value;    
    var arr1=reet.length;      
    var ruut = reet.charAt(arr1-1);   
        if (reet.length > 0){   
        var regex = /[0-9]|\,/;   
            if (!ruut.match(regex)){   
            var reet = reet.slice(0, -1);   
            $(root).val(reet);   
            }   
        }  
 }
 
function ConfermaElimina()
{
	if (confirm('Continuare con l\'eliminazione?'))
	{
		return true;
	}
	return false;
}
	