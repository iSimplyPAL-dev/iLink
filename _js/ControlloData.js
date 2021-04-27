function isUndefined(strField)
{
	if( typeof(strField) == "undefined")
		return true;
	else 
		return false;
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

//funzione da chiamare sull'evento onfocus di un controllo <input type=text>
//che acccetta DATE
function txtDateGotfocus(objCtrl)
{
	if(isUndefined(objCtrl)) return false;
	
	objCtrl.select();
}

function VerificaData(Data)
{
	if (!IsBlank(Data.value ))
	{		
		if(!isDate(Data.value)) 
		{
		    GestAlert('a', 'warning', '', '', 'Inserire la data correttamente in formato: gg/mm/aaaa!');
		    Data.value = "";
			Setfocus(Data);
			return false;
		}
	}					
}	

function TrackBlur(element) 
{

	if (typeof element.id != "undefined")
	gLastElement = element.id;
	document.formRicercaAnagrafica.txtNameObject.value =  gLastElement;
}

function TrackBlurGenerale(element) 
{

	if (typeof element.id != "undefined")
	gLastElement = element.id;
	document.Form1.txtNameObject.value =  gLastElement;
}

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