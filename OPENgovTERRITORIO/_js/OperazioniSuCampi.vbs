function formattaNumero(oggetto)

	str=oggetto.value
	if str<>"" then
		str=Replace (str,",",".")
		Numero=FormatNumber (str,2)
	
		'oggetto.value=Replace (Numero,".",",")
		oggetto.value=Numero
	end if
end function

function formattaNumeroEuro(oggetto,decimali)
	str=oggetto.value
	if str<>"" then
			
		trovato=InStr (1,str,",")
		if trovato=0 then
			str=str & ","
			for index=1 to decimali
				str=str & "0"
			next
			oggetto.value=str
		else
			if Len (str)=1 then
				str="0,"
				for index=1 to decimali
					str=str & "0"
				next
				oggetto.value=str
			else
				if trovato=Len (str) then
					for index=1 to decimali
						str=str & "0"
					next
					oggetto.value=str
				else
					if len(Mid (str,trovato+1))>decimali then
						mystr=Round( "0." & Mid (str,trovato+1),decimali)
						mystr=mid (mystr,3)
						oggetto.value=Mid (str,1,trovato) & mystr
					else
						oggetto.value=str & "0"
					end if
				end if
			end if
		end if	
	end if
end function

function disableSpecialCharForEuro(oggetto)
	'recupero il valore del campo
	valore=oggetto.value
	strlen=len(valore)
		
	Carattere=Right(valore,1)
	if Carattere<>"" then
		if Carattere="," then
			vir1=InStr (1,valore,",")
			vir2=InStrRev (valore,",")
			if vir1<>vir2 then
				oggetto.value= Mid (valore,1,vir1) & Replace(Mid (valore,vir1+1,Len(valore)-1),",","")
			end if
		end if

		asciiValue= Asc (Carattere)
		if 	(asciiValue<>44 and (asciiValue<48 or asciiValue>57)) then
			
			//elimino il carattere inserito
			valore=Replace(valore,Carattere,"")
			oggetto.value= valore
		end if
	end if
end function



function controllaData(oggetto)
	data=oggetto.value
	if data<>"" then
		lunghezza=len (data)
		newDate=""
		select case lunghezza
			case 10 newDate=data
			case 8 newDate=Mid (data,1,2) & "/" & Mid (data,3,2) & "/" & Mid (data,5,4)
			case else newDate=""
		end select
		
		if newDate<>"" then
			if not isDate(newDate) then
				MsgBox newDate & " non è un data valida",vbCritical ,"Errore inserimento data"
				oggetto.value=""
				oggetto.focus()
			else
				oggetto.value=newDate
			end if
		else 
			MsgBox "Data non valida",vbCritical ,"Errore inserimento data"
			oggetto.value=""
			oggetto.focus()
		end if
	end if
end function

function disableSpecialChar(oggetto)
	'recupero il valore del campo
	valore=oggetto.value
	strlen=len(valore)
		
	Carattere=Right(valore,1)
	if Carattere<>"" then
		asciiValue= Asc (Carattere)
		
		'if	(asciiValue<32 or (asciiValue>32 and asciiValue<48) or (asciiValue>57 and asciiValue<65) or (asciiValue>90 and asciiValue<97) or asciiValue>122) then
		
		if 	(asciiValue<32 or (asciiValue>32 and asciiValue<39) or (asciiValue>39 and asciiValue<46) or (asciiValue>46 and asciiValue<48) or (asciiValue>57 and asciiValue<65) or (asciiValue>90 and asciiValue<97) or asciiValue>122) then
			
			//elimino il carattere inserito
			valore=Replace(valore,Carattere,"")
			oggetto.value= valore
		end if
	end if
end function

function disableLetterChar(oggetto)
	'recupero il valore del campo
	valore=oggetto.value
	strlen=len(valore)
		
	Carattere=Right(valore,1)
	if Carattere<>"" then
		asciiValue= Asc (Carattere)
		

		if 	(asciiValue<>47 and asciiValue<>43 and (asciiValue<48 or asciiValue>57)) then
			
			//elimino il carattere inserito
			valore=Replace(valore,Carattere,"")
			oggetto.value= valore
		end if
	end if
end function


function Controllo_PartitaIVA(oggetto)

	PartitaIVA=oggetto.value
	Ritorno=true
	if Len(PartitaIVA)<>0 then
		if Len(PartitaIVA)<>16 then
			Ritorno=false
			if Len(PartitaIVA)=11 then

				For i = 1 To 10 Step 2 
					t = t + CLng(Mid(PartitaIVA, i, 1)) 'cifre dispari 
					l = 2 * CLng(Mid(PartitaIVA, i + 1, 1)) 'cifre pari 
					't = t + IIf(l < 10, l, l - 9) '(somma delle cifre) 
					if l<10 then
						t=t+l
					else
						t=t+(l-9)
					end if
				Next 
				t = t + CLng(Mid(PartitaIVA, 11, 1)) 

				Ritorno = (t Mod 10 = Empty) 
			end if	
	
			if not Ritorno then
				MsgBox "Partita IVA inserita non corretta.",vbCritical
				oggetto.focus()
			else
				MsgBox "Partita IVA inserita non corretta.",vbInformation
			end if
		end if
	end if
end function

function ControllaNumero(oggetto)
	str=oggetto.value
	if str<>"" then
		Lunghezza=Len(str)
		
		for indice=1 to Lunghezza
			if asc(mid(str,indice,1))<48 or asc(mid(str,indice,1))>57 then
				oggetto.focus()
				oggetto.value=""
				msgBox "Inserire un numero.",vbCritical,"Errore"
				exit for
			end if
		next
	end if
end function
