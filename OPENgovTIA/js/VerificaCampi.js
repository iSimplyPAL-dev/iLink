//controlla se il campo di testo è vuoto
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

function isUndefined(strField)
{
	if( typeof(strField) == "undefined")
		return true;
	else 
		return false;
}

function removeChar(strValue,strNewChar)
{
	var reTmp=/\D/g;
	var blnNeg=false;
	
	if(isUndefined(strValue)) strValue = "";
	if(isUndefined(strNewChar)) strNewChar = "";
	if(strValue.charAt(0) == '-') blnNeg=true;
	
	strValue = strValue.replace(reTmp,strNewChar);
	if(blnNeg) strValue='-' + strValue;

	return strValue;
}

function CompletaAnno(sAnno)
{
	if(isNaN(sAnno)) return "";
	if (sAnno.length>4 || sAnno.length<2) return "";
			
	if (sAnno.length==2)
	  if (parseInt(sAnno)<=99 && parseInt(sAnno)>=30)
			sAnno="19"+sAnno ;
		else
			sAnno="20"+sAnno ;

	if(sAnno>2100 || sAnno<1900) return "";
	
	return sAnno;
}

function Setfocus(objField)
{
	//vengono intercettati gli errori in modo che se il controllo non è visibile
	//non sia visualizzato nessun errore
	try{ 
	objField.focus();
	objField.select();
	}
	catch(objErr)
	{}
}

//******************************************************************************************
//Verifica che il valore passato sia una data valida
//strData : Data da Verificare , data in formato "GG/MM/AAAA hh:mm"
//sDataDA : Limite inferiore   , data in formato "GG/MM/AAAA hh:mm"
//sDataA  : Limite superiore   , data in formato "GG/MM/AAAA hh:mm"
function isDate(strData, sDataDA, sDataA)
{
	var gg,mm,aaaa;
	var hh,mi;
	var dTestDate;
	var ArrDate;
	var dData1;
	var ArrHour;
	var sOra, sOraDa, sOraA;

	//separa data e ora
	ArrDate = strData.split(" ");
	strData = ArrDate[0];
	if(ArrDate.length==2)
		sOra = ArrDate[1];
	else
		sOra = "";

	ArrDate = strData.split("/");
	if(ArrDate.length!=3) return false;
	gg   = ArrDate [0];
	mm   = ArrDate [1];
	 aaaa = CompletaAnno(ArrDate [2]);
  
	if(gg=='') return false;
	if(mm=='') return false;
	if(aaaa=='') return false;

	if(sOra!="")
		{
		ArrHour = sOra.split(":");
		if(ArrHour.length!=2) return false;
			hh = ArrHour[0];
			mi = ArrHour[1];
		}
	else
		{
		hh = '00';
		mi = '00';
		}

	if(hh=='') return false;
	if(mi=='') return false;

	//verifica che il campo strData contenga una DATA valida
	dTestDate = new Date(aaaa,mm-1,gg,hh,mi);
	if(dTestDate.getDate()!= parseInt(gg*1))			return false;
	if(dTestDate.getMonth()!= parseInt(mm*1)-1)		return false;
	if(dTestDate.getFullYear()!= parseInt(aaaa*1))return false;
	if(dTestDate.getHours()!= parseInt(hh*1))		return false;
	if(dTestDate.getMinutes()!= parseInt(mi*1))		return false;

	//verifica range dei valori ammessi
	if(!isUndefined(sDataDA) || !isUndefined(sDataA))
	{
		//Verifica limite inferiore
		if(!isUndefined(sDataA) && jTrim(sDataA.toString())!='')
		{
			//converte sDataA in un oggetto DATA
			//separa data e ora
			ArrDate = sDataA.split(" ");
			sDataA = ArrDate[0];
			if(ArrDate.length==2)
				sOraA = ArrDate[1];
			else
				sOraA = "";
			
			ArrDate = sDataA.split("/");
			if(sOraA!="")
				{
				ArrHour = sOraA.split(":");
				hh = ArrHour[0];
				mi = ArrHour[1];
				}
			else
				{
				hh = '00';
				mi = '00';
				}
			dData1 = new Date(ArrDate[2],ArrDate[1]-1,ArrDate[0],hh,mi);

			if(dTestDate>dData1) return false;
		}
		
		//Verifica limite superiore
		if(!isUndefined(sDataDA) && jTrim(sDataDA.toString())!='')
		{
			//converte sDataDA in un oggetto DATA
			//separa data e ora
			ArrDate = sDataDA.split(" ");
			sDataDA = ArrDate[0];
			if(ArrDate.length==2)
				sOraDa = ArrDate[1];
			else
				sOraDa = "";
			
			ArrDate = sDataDA.split("/");
			if(sOraDa!="")
				{
				ArrHour = sOraDa.split(":");
				hh = ArrHour[0];
				mi = ArrHour[1];
				}
			else
				{
				hh = '00';
				mi = '00';
				}
			dData1 = new Date(ArrDate[2],ArrDate[1]-1,ArrDate[0],hh,mi);

			if(dTestDate<dData1) return false;
		}
	}
 return true;
}

function IsValidChar(sText)
{
    var ValidChars = "0123456789QWERTYUIOPASDFGHJKLZXCVBNMqwertyuioplkjhgfdsazxcvbnm'&$@[\/]^_{}~+-.:,;() àèéìòùÀÈÉÌÒÙ";
	var IsNumber=true;
	var Char;

	for (i = 0; i < sText.length && IsNumber == true; i++) 
		{ 
		Char = sText.charAt(i); 
		if (ValidChars.indexOf(Char) == -1) 
			{
				if (sText.charCodeAt(i) != 224 && sText.charCodeAt(i) != 233 && sText.charCodeAt(i) != 232 && sText.charCodeAt(i) != 236 && sText.charCodeAt(i) != 242 && sText.charCodeAt(i) != 249 && sText.charCodeAt(i) != 231 && sText.charCodeAt(i) != 176 && sText.charCodeAt(i) != 8364 && sText.charCodeAt(i) != 163 && sText.charCodeAt(i) != 244 && sText.charCodeAt(i) != 226 && sText.charCodeAt(i) != 63) //àéèìòùç°€£
				{
					//alert(sText.charCodeAt(i));
					IsNumber = false;
				}
			}
		}
	return IsNumber;
}		

function txtDateGotfocus(objCtrl)
{
	if(isUndefined(objCtrl)) return false;
	
	objCtrl.select();
}

function isNumber(strField, intInteri, intDecimali, intMinVal, intMaxVal)
{	
	var strInteri='';
	var strDecimali='';
	var vetNumber;
	var rePunti=/\./gi;
	var reVirg = /,/gi ;
	
//alert("strField=" + strField + "\nintInteri=" + intInteri + "\nintDecimali=" + intDecimali + "\nintMinVal=" + intMinVal + "\nintMaxVal=" + intMaxVal);

	// *** Verifica parametri ***
	if(isUndefined(strField))					return false;
	if(IsBlank(strField))							return false;
	if(isUndefined(intInteri))				intInteri = -1;
	if(intInteri.toString()=='')			intInteri = -1;
	if(isUndefined(intDecimali))			intDecimali = -1;
	if(intDecimali.toString()=='')		intDecimali = -1;
	
	//strField = strField.replace(reVirg,'.');
    //vetNumber = strField.split(".");
	vetNumber = strField.replace(".", ",");
	vetNumber = strField.split(",");
	if(vetNumber.length>2) return false;	//verifica che non ci siano 2 o più virgole
	
	if(vetNumber.length == 2)
	{ 
		if(intDecimali == 0)	return false;	//se non sono richiesti decimali non deve essere presente la virgola
		
		strInteri = vetNumber[0];
		strDecimali = vetNumber[1];
		if(strDecimali.length == 0) return false;		//deve esistere almeno una cifra dopo la virgola
	}
	else
		strInteri = strField;
		
	strInteri = strInteri.replace(rePunti,"");	//elimina dalla parte intera eventuali punti '.'
	
	if(strInteri == '')			return false;
	if(isNaN(strInteri))		return false;
	if(isNaN(strDecimali))	return false;

	//Verifica numero di cifre intere e decimali
	if( intInteri > 0)
		if(strInteri.length > intInteri) return false;
	
	if( intDecimali > 0)
		if(strDecimali.length > intDecimali) return false;

	//Verifica il range dei valori ammessi
	if(!isUndefined(intMaxVal) || !isUndefined(intMinVal))
	{
		var intNewNumber;
		var strNum;
		strNum = strInteri;
		if(strDecimali.length >= 0) strNum += '.' + strDecimali;
		intNewNumber = new Number(strNum);

		//Verifica limite superiore
		if(!isUndefined(intMaxVal) && (intMaxVal.toString())!='')
		{
			//converte intMaxVal nel formato inglese
			intMaxVal = intMaxVal.toString();
			intMaxVal = intMaxVal.replace(reVirg,'.');
//alert('intMaxVal=' + intMaxVal + '\nintNewNumber=' +intNewNumber);
			if(intNewNumber>intMaxVal) return false;
		}
		//Verifica limite inferiore
		if(!isUndefined(intMinVal) && (intMinVal.toString())!='')
		{
			//converte intMinVal nel formato inglese
			intMinVal = intMinVal.toString();
			intMinVal = intMinVal.replace(reVirg,'.');
//alert('intMinVal=\'' + intMinVal + '\'\nintNewNumber=' +intNewNumber);
			if(intNewNumber<intMinVal) return false;
		}
	}
	
	return true;  
}

function txtDateLostfocus(objCtrl)
{
	var gg="";
	var mm="";
	var aaaa="";
	var strData;
	var strTmp;
	var len;
	
	if(isUndefined(objCtrl)) return false;
	
	strData = objCtrl.value;
	if(strData == "") return false;
	
	arrayOfStrings = strData.split(".");
	if(arrayOfStrings.length<3)
		{
			arrayOfStrings = strData.split("/");
			if(arrayOfStrings.length<3)
			{
				arrayOfStrings = strData.split("-");
				if(arrayOfStrings.length<3)
				{
					arrayOfStrings = strData.split(",");
				}
			}
		}
	
	if((arrayOfStrings.length==1) &&
		 ((arrayOfStrings[0].length==6) || (arrayOfStrings[0].length==8)))
	{
		len = arrayOfStrings[0].length;
		strTmp = arrayOfStrings[0];
		arrayOfStrings[0] = strTmp.substr(0,2);
		arrayOfStrings[1] = strTmp.substr(2,2);
		arrayOfStrings[2] = strTmp.substr(4,(len==8) ? 4 : 2);
	}
	
	if(arrayOfStrings.length>0)
		gg= removeChar(arrayOfStrings[0],"");
	if(arrayOfStrings.length>1)
		mm= removeChar(arrayOfStrings[1],"");
	if(arrayOfStrings.length>2)
	  aaaa= removeChar(arrayOfStrings[2],"");       
	
	//porta il formato sempre a GG/MM/AAAA	
	if (gg.length==0) gg="00";
	if (gg.length==1) gg="0"+gg;
	if (mm.length==0) mm="00";
	if (mm.length==1) mm="0"+mm;
	if((strTmp=CompletaAnno(aaaa))!="") 
		aaaa = strTmp;
	else
		if(aaaa.length<4) aaaa = "0000";
			
	objCtrl.value = gg + "/" + mm + "/" + aaaa
	
	return true;
}

function VerificaData(Data)
{
	if (!IsBlank(Data.value ))
	{		
		if(!isDate(Data.value)) 
		{
		    alert("Data non valida !");
		    Data.value = "";
			Setfocus(Data);
			return false;
		}
	}					
}



//-->

/*Solo numeri da richiamare sull'onkeypress */
// NumbersOnly(event)  ti permette di inserire solo numeri e i caratteri  definiti nella var AllowedKeys
// NumbersOnly(event,true,false,2) permette di inserire numeri con 2 decimali non negativi
// NumbersOnly(event,true,true,2) permette di inserire numeri con 2 decimali anche negativi
// NumbersOnly(event,false,false,0) vedi NumbersOnly(event)

function NumbersOnly(_event, AllowDecimals, AllowNegatives, DecimalDigits)	{
	var _source			=	(_event.srcElement)?_event.srcElement:_event.target;
	var	_sourceText	=	_source.value;
	var keyPressed	= _event.keyCode ? _event.keyCode : _event.which ? _event.which : _event.charCode;
	var isDigit			=	(keyPressed > 47 && keyPressed < 58);
	if (isDigit) {
		if (AllowDecimals)	{
			// separatore decimali col punto
			// var dotPos		=	_sourceText.indexOf(".");
			// separatore decimali con la virgola
			var dotPos		=	_sourceText.indexOf(",");
			
			if (dotPos > -1) {
				//var caretPosition   = _sourceText.length +1;
				var caretPosition	=	(document.selection)?(document.selection.createRange().getBookmark().charCodeAt(2) - 2):_source.selectionStart;
				var _DecimalDigits	=	(DecimalDigits == null)?2:((isNaN(DecimalDigits))?2:DecimalDigits)
			
				var n = navigator;
				var na = n.appVersion;
				if (na.indexOf('MSIE 7')!=-1){
					var IE7 = true;
				}
				//alert(IE7);
				//alert(navigator.appVersion);
				//alert(navigator.userAgent);
				//alert('DotPos = ' + dotPos);
				//alert('CaretPosition = ' + caretPosition);
				//alert('_DecimalDigits = ' + _DecimalDigits);
				
			
				if (caretPosition > dotPos)
					// controllo la versione del browser
					if(IE7){	
						return ((caretPosition - dotPos - 1) > _DecimalDigits)?false:true;
					}else{
						return ((caretPosition - dotPos) > _DecimalDigits)?false:true;	
					}
					
				else
					return true;	
			}
			else
				return true;	
		}	
		else
			return true;	
	}
	else	{
		//separatore decimali con la virgola
		//if (String.fromCharCode(keyPressed) == ",")
		// separatore decimali con la virgola
		if (String.fromCharCode(keyPressed) == ",")
			if (AllowDecimals)
				return (_sourceText.indexOf(",") == -1);
			else
				return false;

		if (String.fromCharCode(keyPressed) == "-") {
			if (_sourceText.charAt(0) != "-"  &&  AllowNegatives)
				_source.value	=	"-" + _sourceText;
				return false;
		}
				
		//var AllowedKeys	=	new Array(0, 8, 9, 13, 27, 116, 37, 38, 39, 40, 46)
		var AllowedKeys	=	new Array(0, 8, 9, 13, 27, 45, 44)
				
		for (var i=0; i<AllowedKeys.length; i++)
			if (keyPressed == AllowedKeys[i])
				return true;
		
		if (isNaN(_sourceText)){
			_source.value= "";
			//_source.value= _sourceText.substring(0, _sourceText.length - 1);
			return true;
		}
	}
	
	return false;

}

function IsValidCharForCode(sText)
{
	var ValidChars = "0123456789QWERTYUIOPASDFGHJKLZXCVBNMqwertyuioplkjhgfdsazxcvbnm";
	var IsNumber=true;
	var Char;

		
	for (i = 0; i < sText.length && IsNumber == true; i++) 
		{ 
		Char = sText.charAt(i); 
		if (ValidChars.indexOf(Char) == -1) 
			{
			IsNumber = false;
			}
		}
	return IsNumber;
}		

//-->