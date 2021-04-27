<!-- Begin


function Azzera(oggetto,valore){
	oggetto.value=valore
	return
}

function txtNumberLostfocus(objCtrl,blnSepMigliaia)
{
	var strOut;
	var rePunti=/\./g;
	var strValue;

	if(isUndefined(objCtrl)) return false;
	if(isUndefined(blnSepMigliaia)) blnSepMigliaia = true;

	strValue = objCtrl.value;
	if(strValue=='') return true;
	
	//se in questo evento c'è un punto lo considera come separatore dei decimali 
	//inserito da tastierino numerico
	strValue = strValue.replace(rePunti,",");	
	strOut = StrInsSeparatori(strValue, blnSepMigliaia);
	objCtrl.value = strOut 
}

//******************************************************************************************
//funzione da chiamare sull'evento onfocus di un controllo <INPUT type=text>
//che acccetta numeri o valute
function txtNumberGotfocus(objCtrl)
{
	var strValue;
	var rePunti=/\./g;

	if(isUndefined(objCtrl)) return false;

	strValue = objCtrl.value;
	strValue = strValue.replace(rePunti,"");	//elimina eventuali punti '.' separatori delle migliaia
	objCtrl.value=strValue;
	objCtrl.select();
}
function StrInsSeparatori(strIn, blnSepMigliaia)
{
	var rePunti=/\./g;
	var strInteri='';
	var strDecimali='';
	var vetNumber;
	var strInteriNew;
	var i;
	
	vetNumber = strIn.split(",");
	if(vetNumber.length == 2)
	{ 
		strInteri = vetNumber[0];
		strDecimali = vetNumber[1];
	}
	else
		strInteri = strIn;

	strInteri=removeChar(strInteri,"");
	strDecimali=removeChar(strDecimali,"");
	strInteri = strInteri.replace(rePunti,"");	//elimina dalla parte intera eventuali punti '.'
	
	strInteriNew=strInteri;
	//Inserisce i separatori delle migliaia
	if(blnSepMigliaia)
	{
		strInteriNew="";
		for(i=strInteri.length ; i>0 ; i-=3)
			strInteriNew = "." + strInteri.substring(i-3,i) + strInteriNew;
		strInteriNew=strInteriNew.substring(1,strInteriNew.length);
	}
	if(strInteriNew=="") strInteriNew = "0"; //strInteriNew = "";
	strInteri = strInteriNew;
	if(strDecimali.length != 0)		//deve esistere almeno una cifra dopo la virgola
		strInteri = strInteri + "," + strDecimali
	return(strInteri);
}
//******************************************************************************************
//funzione da chiamare sull'evento onfocus di un controllo <INPUT type=text>
//che acccetta DATE
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
		if(!isUndefined(intMaxVal) && jTrim(intMaxVal.toString())!='')
		{
			//converte intMaxVal nel formato inglese
			intMaxVal = intMaxVal.toString();
			intMaxVal = intMaxVal.replace(reVirg,'.');
//alert('intMaxVal=' + intMaxVal + '\nintNewNumber=' +intNewNumber);
			if(intNewNumber>intMaxVal) return false;
		}
		//Verifica limite inferiore
		if(!isUndefined(intMinVal) && jTrim(intMinVal.toString())!='')
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


function isUndefined(strField)
{
	if( typeof(strField) == "undefined")
		return true;
	else 
		return false;
}
function txtDateGotfocus(objCtrl)
{
	
	if(isUndefined(objCtrl)) return false;
	objCtrl.select();
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
	
	//if(isNaN(gg) || isNaN(mm) || isNaN(aaaa)) return false;
	//if(gg=="" || mm=="" ) return false;
	
	//porta il formato sempre a GG/MM/AAAA	
	if (gg.length==0) gg="00";
	if (gg.length==1) gg="0"+gg;
	if (mm.length==0) mm="00";
	if (mm.length==1) mm="0"+mm;
	if((strTmp=CompletaAnno(aaaa))!="") 
		aaaa = strTmp;
	else
		if(aaaa.length<4) aaaa = "0000";
			
	
	//alert("gg=" + gg + "\nmm=" + mm + "\naaaa=" + aaaa);
	objCtrl.value = gg + "/" + mm + "/" + aaaa
	
	return true;
}



function removeChar(strValue,strNewChar)
{
	var reTmp=/\D/g;
	//var reTmp=/^\[-+]\D/g;
	var blnNeg=false;
	
	//alert("Before strValue=" + strValue);
	if(isUndefined(strValue)) strValue = "";
	if(isUndefined(strNewChar)) strNewChar = "";
	if(strValue.charAt(0) == '-') blnNeg=true;
	
	strValue = strValue.replace(reTmp,strNewChar);
	if(blnNeg) strValue='-' + strValue;
	//alert("After strValue=" + strValue);
	return strValue;
}


//**********************************************************************
//Elimina gli spazi iniziali e finali dalla stringa passata
//NOTA : funzione copiata dal negozio : autore Pregione
function jTrim(sInput)
{
 var i;
 
 //cerca la posizione del primo spazio
 for (i = 0; i < sInput.length; i++)
  if (sInput.charAt(i) != ' ')
   break;
   
 // elimina gli spazi iniziali  
 if (i>0)
  sInput = sInput.substr(i);
 
 if (sInput.length > 0)
 {
	//cerca la posizione del primo carattere <> da spazio partendo dal fondo
  for (i = sInput.length - 1; i >= 0; i--)
   if (sInput.charAt(i) != ' ')
    break;
  
  // elimina gli spazi finali
  if (i >= 0)
   sInput = sInput.substr(0, i +1);
 }
 return sInput;
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

	//alert('data = ' + strData);	
	//separa data e ora
	ArrDate = strData.split(" ");
	strData = ArrDate[0];
	if(ArrDate.length==2)
		sOra = ArrDate[1];
	else
		sOra = "";
		
	//alert('data = ' + strData);	
	//alert('ora = ' + sOra);

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
//alert(gg+'/'+mm+'/'+aaaa+'\n'+gg+'/'+parseInt(mm*1)+'/'+aaaa+'\n'+dTestDate.getDate()+'/'+(dTestDate.getMonth()+1)+'/'+dTestDate.getFullYear());
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

//alert(dTestDate.getDate()+'/'+(dTestDate.getMonth()+1)+'/'+dTestDate.getYear()+'\n'+ dData1.getDate()+'/'+(dData1.getMonth()+1)+'/'+dData1.getYear());

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




//-->

