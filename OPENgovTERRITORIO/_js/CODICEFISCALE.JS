<!--
function CalcolaCodice(CmpCF,txtCognome,txtNome,txtSesso,Data,CodiceLuogo) {


var Error=false;
var msg="";
var CodFisc = '';

if (!txtCognome.value){
	Error=true;
	msg=msg+"Cognome\n"
}
if (!txtNome.value){
	Error=true;
	msg=msg+"Nome\n"
}
if (!txtSesso.value){
	Error=true;
	msg=msg+"Sesso\n"
}
if (!CodiceLuogo.value){
	Error=true;
	msg=msg+"Luogo di nascita\n"
}
if (!Data.value){
	Error=true;
	msg=msg+"Data di nascita\n"
}

if (!Error){

	var MyCF=CmpCF.value

	//Elimino gli spazi dal cognome
	//var strAppCogn=txtCognome.value.toUpperCase();
	var strAppCogn=txtCognome.value
	strAppCogn=strAppCogn.toUpperCase();
	var len=strAppCogn.length
	var newStrCognome=""
	for (i=0;i<len;i++){
		if (strAppCogn.charAt(i)!=" " && strAppCogn.charAt(i)!="'"){
			newStrCognome=newStrCognome+strAppCogn.charAt(i)
		}
	}

	//Elimino gli spazi dal nome
	var strAppNome=txtNome.value
	var len=strAppNome.length
	var newStrNome=""
	for (i=0;i<len;i++){
		if (strAppNome.charAt(i)!=" " && strAppNome.charAt(i)!="'"){
			newStrNome=newStrNome+strAppNome.charAt(i)
		}
	}




	var sGiorno=Data.value.substr(0,2)
	var sMese=Data.value.substr(3,2)
	var sAnno=Data.value.substr(6)

	g = new Number();
	m = new Number();
	g = sGiorno;
	m = sMese;


	/* Costruzione stringa codice fiscale */

	var NewCF=""
	NewCF += MakeNC(newStrCognome,'C'); 
	NewCF += MakeNC(newStrNome,'N');
	NewCF += sAnno.substr(2,2);
	NewCF += MakeDtMese(g,m,txtSesso.value);
	NewCF += CodiceLuogo.value;

	CodFisc=NewCF
	NewCF += valida(CodFisc);

	if(MyCF!="")
	{
		bRet=true
		if (NewCF!=MyCF)
		{
			bRet=GestAlert('c', 'question', 'Il Codice Fiscale inserito ("+MyCF+") \nè diverso da quello calcolato ("+NewCF+").\n\nSe si vuole mantenere quello inserito premere OK.\nSe si vuole inserire quello calcolato premere Annulla")
		}
		if (bRet)
		{
			CmpCF.value=MyCF
			CmpCF.focus();
		}
		else
		{
			CmpCF.value=NewCF
			CmpCF.focus();
		}
	}
	else
	{
		CmpCF.value=NewCF
		CmpCF.focus();
	}
 } 
 else
 {
	alert ("Inserire i seguenti campi:\n"+msg)
 }
}

/*

Funzione per determinare i caratteri del Cognome e del Nome

Parametri:
strName	: stringa che contiene il cognome od il nome
strType : stringa che contiene 'C' per determinare i caratteri del cognome
          'N' per determinare i caratteri del nome

La routine non ha nessun controllo per la validazione delle stringhe passate

*/

function MakeNC(strName, strType)
{

 strCons = new Array;
 strVocl = new Array;

var intNumCons = 0;
var intNumVocl = 0;
var intPosCons = 0;
var intPosVocl = 0;
var intLenStr = strName.length;
var intLoop = 0;
    
var strIDName=''

do {
    strIDName = strName.substr(intLoop , 1);
   
    if ((strIDName == 'A') || ( strIDName == 'E') || (strIDName == 'O') || (strIDName == 'U') || (strIDName == 'I'))
      {
      strVocl[intPosVocl] = strIDName;
      ++intPosVocl;
      ++intNumVocl;
      }
  
  else {    

           strCons[intPosCons] = strIDName;
          ++intPosCons ;
          ++intNumCons ;
         }  
    
     ++intLoop;} while (intLoop < intLenStr); 
    
    strIDName = '';

    if (strType == 'C'){
        if (intNumCons >= 3)
            {strIDName += strCons[0];
             strIDName += strCons[1];
             strIDName += strCons[2];}
        
        if ((intNumCons == 2) && (intNumVocl >= 1)){
            strIDName += strCons[0];
            strIDName += strCons[1];
            strIDName += strVocl[0];}
        
        if ((intNumCons == 1) && (intNumVocl >= 2)){
            strIDName += strCons[0];
            strIDName += strVocl[0];
            strIDName += strVocl[1];}
        
        if ((intNumCons == 1) && (intNumVocl == 1)){
            strIDName += strCons[0];
            strIDName += strVocl[0];
            strIDName += 'X';}
        
        if ((intNumCons == 0) && (intNumVocl >= 2)) {
            strIDName += strVocl[0];
            strIDName += strVocl[1];
            strIDName += 'X';}
        }
        
    if (strType == 'N'){
        if (intNumCons >= 4){
            strIDName += strCons[0]; 
            strIDName += strCons[2];
            strIDName += strCons[3];}
       
        if (intNumCons == 3){
            strIDName += strCons[0];
            strIDName += strCons[1];
            strIDName += strCons[2];}
        
        if ((intNumCons == 2) && (intNumVocl >= 1)){
            strIDName += strCons[0];
            strIDName += strCons[1];
            strIDName += strVocl[0];}
        
        if ((intNumCons == 1) && (intNumVocl >= 2)){
            strIDName += strCons[0];
            strIDName += strVocl[0];
            strIDName += strVocl[1];}
        
        if ((intNumCons == 1) && (intNumVocl == 1)){
            strIDName += strCons[0];
            strIDName += strVocl[0];
            strIDName += 'X';}
        
        if ((intNumCons == 0) && (intNumVocl >= 2)){
            strIDName += strVocl[0];
            strIDName += strVocl[1];
            strIDName += "X";}
        }


return strIDName;

}

/*
 Converte il mese nella lettera corrispondente e ritorna 
 la stringa MeseGiorno
 
 Parametri:
 
 Giorno : Giorno di nascita in formato stringa d
 Mese   : Mese di nascita in formato stringa mm
 Sesso  : Stringa contenente 'M' per maschio, 'F' per femmina
  
*/

function MakeDtMese(Giorno,Mese,Sesso)
{

var strDtNasc = '';
var strMmNasc = '';
var strGgNasc = '';
var GiornoMese= '';


  switch (Mese){
  
    case "01":
        strMmNasc = 'A';
        break;
        
    case '02':
        strMmNasc = 'B';
        break;
        
    case '03':
        strMmNasc = 'C';
        break;
    case '04':
        strMmNasc = 'D';
        break;
    case '05':
        strMmNasc = 'E';
        break;
    case '06':
        strMmNasc = 'H';
        break;
    case '07':
        strMmNasc = 'L';
        break;
    case '08':
        strMmNasc = 'M';
        break;
    case '09':
        strMmNasc = 'P';
        break;
    case '10':
        strMmNasc = 'R';
        break;
    case '11':
        strMmNasc = 'S';
        break;
    case '12':
        strMmNasc = 'T';
        break;
   }

    
    if (Sesso=='F'){
    
         strGgNasc = (Giorno*40)/40+40;
    }
     else
     {
      strGgNasc = Giorno;
     } 
    
    GiornoMese += strMmNasc;
    GiornoMese += strGgNasc;

    return GiornoMese;


}

/* 
Carattere di validazione

Determina il carattere finale del codice per la validazione

Parametri:

codice : stringa del codice fiscale determinato in precedenza (15 caratteri)

*/

function valida(codice){

strPari = new Array(8);
strDispari = new Array(8);

var intLoop;
var intSommaPari = 0;
var intSommaDispari = 0;
var intResto = 0;

var lettera='';

  strDispari[0] = codice.substr(0,1);
  strDispari[1] = codice.substr(2,1);
  strDispari[2] = codice.substr(4,1);
  strDispari[3] = codice.substr(6,1);
  strDispari[4] = codice.substr(8,1);
  strDispari[5] = codice.substr(10,1);
  strDispari[6] = codice.substr(12,1);
  strDispari[7] = codice.substr(14,1);
  strPari[0] = codice.substr(1,1);
  strPari[1] = codice.substr(3,1);
  strPari[2] = codice.substr(5,1);
  strPari[3] = codice.substr(7,1);
  strPari[4] = codice.substr(9,1);
  strPari[5] = codice.substr(11,1);
  strPari[6] = codice.substr(13,1);
  strPari[7] = codice.substr(15,1);

     
    for (intLoop = 0; intLoop <= 7 ; ++intLoop){
        
        switch (strDispari[intLoop]){
        
        case "0":
            intSommaDispari = intSommaDispari + 1;
            break;    
        case "1":
            intSommaDispari = intSommaDispari + 0;
            break;
        case "2":
            intSommaDispari = intSommaDispari + 5;
            break;
        case "3":
            intSommaDispari = intSommaDispari + 7;
            break;
        case "4":
            intSommaDispari = intSommaDispari + 9;
            break;
        case "5":
            intSommaDispari = intSommaDispari + 13;
            break;
        case "6":
            intSommaDispari = intSommaDispari + 15;
            break;    
        case "7":
            intSommaDispari = intSommaDispari + 17;
            break;
        case "8":
            intSommaDispari = intSommaDispari + 19;
            break;
        case "9":
            intSommaDispari = intSommaDispari + 21;
            break;
        case "A":
            intSommaDispari = intSommaDispari + 1;
            break;
        case "B":
            intSommaDispari = intSommaDispari + 0;
            break;
        case "C":
            intSommaDispari = intSommaDispari + 5;
            break;
        case "D":
            intSommaDispari = intSommaDispari + 7;
            break;
        case "E":
            intSommaDispari = intSommaDispari + 9;
            break;
        case "F":
            intSommaDispari = intSommaDispari + 13;
            break;
        case "G":
            intSommaDispari = intSommaDispari + 15;
            break;
        case "H":
            intSommaDispari = intSommaDispari + 17;
            break;
        case "I":
            intSommaDispari = intSommaDispari + 19;
            break;
        case "J":
            intSommaDispari = intSommaDispari + 21;
            break;
        case "K":
            intSommaDispari = intSommaDispari + 2;
            break;
        case "L":
            intSommaDispari = intSommaDispari + 4;
            break;
        case "M":
            intSommaDispari = intSommaDispari + 18;
            break;
        case "N":
            intSommaDispari = intSommaDispari + 20;
            break;
        case "O":
            intSommaDispari = intSommaDispari + 11;
            break;
        case "P":
            intSommaDispari = intSommaDispari + 3;
            break;
        case "Q":
            intSommaDispari = intSommaDispari + 6;
            break;
        case "R":
            intSommaDispari = intSommaDispari + 8;
            break;
        case "S":
            intSommaDispari = intSommaDispari + 12;
            break;
        case "T":
            intSommaDispari = intSommaDispari + 14;
            break;
        case "U":
            intSommaDispari = intSommaDispari + 16;
            break;
        case "V":
            intSommaDispari = intSommaDispari + 10;
            break;
        case "W":
            intSommaDispari = intSommaDispari + 22;
            break;
        case "X":
            intSommaDispari = intSommaDispari + 25;
            break;
        case "Y":
            intSommaDispari = intSommaDispari + 24;
            break;
        case "Z":
            intSommaDispari = intSommaDispari + 23;
            break;
        } 
     }

     
      for (intLoop = 0; intLoop<= 7 ; intLoop++){
        switch (strPari[intLoop]){
        case "0":
            intSommaPari = intSommaPari + 0;
            break;
        case "1":
            intSommaPari = intSommaPari + 1;
            break;
        case "2":
            intSommaPari = intSommaPari + 2;
            break;
        case "3":
            intSommaPari = intSommaPari + 3;
            break;
        case "4":
            intSommaPari = intSommaPari + 4;
            break;
        case "5":
            intSommaPari = intSommaPari + 5;
            break;
        case "6":
            intSommaPari = intSommaPari + 6;
            break;
        case "7":
            intSommaPari = intSommaPari + 7;
            break;
        case "8":
            intSommaPari = intSommaPari + 8;
            break;
        case "9":
            intSommaPari = intSommaPari + 9;
            break;
        case "A":
            intSommaPari = intSommaPari + 0;
            break;
        case "B":
            intSommaPari = intSommaPari + 1;
            break;
        case "C":
            intSommaPari = intSommaPari + 2;
            break;
        case "D":
            intSommaPari = intSommaPari + 3;
            break;
        case "E":
            intSommaPari = intSommaPari + 4;
            break;
        case "F":
            intSommaPari = intSommaPari + 5;
            break;
        case "G":
            intSommaPari = intSommaPari + 6;
            break;
        case "H":
            intSommaPari = intSommaPari + 7;
            break;
        case "I":
            intSommaPari = intSommaPari + 8;
            break;
        case "J":
            intSommaPari = intSommaPari + 9;
            break;
        case "K":
            intSommaPari = intSommaPari + 10;
            break;
        case "L":
            intSommaPari = intSommaPari + 11;
            break;
        case "M":
            intSommaPari = intSommaPari + 12;
            break;
        case "N":
            intSommaPari = intSommaPari + 13;
            break;
        case "O":
            intSommaPari = intSommaPari + 14;
            break;
        case "P":
            intSommaPari = intSommaPari + 15;
            break;
        case "Q":
            intSommaPari = intSommaPari + 16;
            break;
        case "R":
            intSommaPari = intSommaPari + 17;
            break;
        case "S":
            intSommaPari = intSommaPari + 18;
            break;
        case "T":
            intSommaPari = intSommaPari + 19;
            break;
        case "U":
            intSommaPari = intSommaPari + 20;
            break;
        case "V":
            intSommaPari = intSommaPari + 21;
            break;
        case "W":
            intSommaPari = intSommaPari + 22;
            break;
        case "X":
            intSommaPari = intSommaPari + 23;
            break;
        case "Y":
            intSommaPari = intSommaPari + 24;
            break;
        case "Z":
            intSommaPari = intSommaPari + 25;
            break;
        }
    }
  

    intResto = 0;
    intResto = (intSommaDispari + intSommaPari) % 26;
     
    intResto = intResto+65


/***************************************************
Istruzioni successive da tenere a mente
****************************************************/
        
        lettera = intResto.toString(16);  // Converte il numero intResto in una
										  // stringa esadecimale
										  									
        lettera=unescape('%'+lettera);    // Restituisce il carattere corrispondente
                                          // al valore esadecimale 

        return lettera ;  
}

-->