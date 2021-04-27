function ControllaData(oggetto)
			Dim oRe, oMatches
			dim inpStr 
			Set oRe = New RegExp
			document.Form1.ctrlFocus.value = "0"
			inpStr=oggetto.value
			select case len(inpStr)
				case 0:
					exit function 
				case 8:
					inpStr=mid(inpStr,1,2) & "/" & mid(inpStr,3,2) & "/" & mid(inpStr,5)
				case 10:
				case else
					document.Form1.ctrlFocus.value = "1"
					MsgBox  "Data in formato non valido!",vbCritical,"Errore inserimento data"
					oggetto.focus()
					oggetto.value=""
					exit function
			end select
			oRE.Pattern="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
			Set oMatches = oRe.Execute(inpStr)
			
			if oMatches.count=0 then
				document.Form1.ctrlFocus.value = "1"
				MsgBox "Data non valida!",vbCritical,"Errore inserimento data" 
				oggetto.value=""
				oggetto.focus()
				exit function
			else
				oggetto.value=inpStr
			end if
			document.Form1.ctrlFocus.value = "0"
		end function 
		
		function ControllaOra(oggetto)
			Dim oRe, oMatches
			dim inpStr 
			Set oRe = New RegExp
			inpStr=oggetto.value
			select case len(inpStr)
				case 4:
					inpStr=mid(inpStr,1,2) & ":" & mid(inpStr,3,2) 
				case 5:
				case else
					MsgBox  "Ora in formato non valido!",vbCritical,"Errore inserimento ora"
					oggetto.value=""
					oggetto.focus()
					exit function
			end select
			oRE.Pattern="^([0-1]?[0-9]|[2][0-3]).([0-5][0-9])$"
			Set oMatches = oRe.Execute(inpStr)
			if oMatches.count=0 then
				MsgBox "Ora non valida!",vbCritical,"Errore inserimento ora" 
				oggetto.value=""
				oggetto.focus()
			else
				oggetto.value=inpStr
			end if
		end function 
		
		function ControllaEmail(oggetto)
			Dim oRe, oMatches
			dim inpStr 
			Set oRe = New RegExp
			inpStr=oggetto.value
			oRE.Pattern="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name|tv))$"
			Set oMatches = oRe.Execute(inpStr)
			if oMatches.count=0 then
				MsgBox "Indirizzo e-mail non valido!",vbCritical,"Errore inserimento e-mail" 
				oggetto.value=""
				oggetto.focus()
				ControllaEmail=false
			end if
			ControllaEmail=true
		end function 
		
		
		function ControllaTelefono(oggetto)
			Dim oRe, oMatches
			dim inpStr 
			Set oRe = New RegExp
			inpStr=oggetto.value
			oRE.Pattern="(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\-\s]{10}$)"
			Set oMatches = oRe.Execute(inpStr)
			if oMatches.count=0 then
				MsgBox "Numero di telefono non valido!",vbCritical,"Errore inserimento telefono" 
				oggetto.value=""
				oggetto.focus()
			end if
		end function 
				
		function ControllaNumeri(oggetto)
			Dim oRe, oMatches
			dim inpStr 
			document.Form1.ctrlFocus.value = "0"
			inpStr = oggetto.value
			Set oRe = New RegExp
			oRE.Pattern = "^[0-9]{1,}$"
			Set oMatches = oRe.Execute(Trim(inpStr))
			if oMatches.count=0 then
				MsgBox "Valore non corretto, inserire solo cifre",vbCritical,"Errore inserimento valore" 
				oggetto.value=""
				oggetto.focus()
				document.Form1.ctrlFocus.value = "1"
				exit function
			end if
		end function
		
		function ControllaNumeriLettere(oggetto)
			Dim oRe, oMatches
			dim inpStr 
			document.Form1.ctrlFocus.value = "0"
			inpStr = oggetto.value
			Set oRe = New RegExp
			oRE.Pattern = "^[a-zA-Z0-9]{1,}$"
			Set oMatches = oRe.Execute(Trim(inpStr))
			if oMatches.count=0 then
				MsgBox "Valore non corretto, inserire solo cifre o lettere",vbCritical,"Errore inserimento valore" 
				oggetto.value=""
				oggetto.focus()
				document.Form1.ctrlFocus.value = "1"
				exit function
			end if
		end function
		
		function ControllaCodiceFiscalePIva(oggetto)
			Dim oRe, oMatches
			dim inpStr 
			Set oRe = New RegExp
			inpStr=oggetto.value
			document.Form1.ctrlFocus.value = "0"
			select case len(inpStr)
				case 0:
						document.Form1.ctrlFocus.value = "0"
						exit function
				case 11:
						oRE.Pattern="(^[0-9]{1,}$)"
						Set oMatches = oRe.Execute(inpStr)
						if oMatches.count=0 then
							MsgBox "Partita Iva non corretta",vbCritical,"Errore inserimento Partita Iva" 
							oggetto.value=""
							oggetto.focus()
							document.Form1.ctrlFocus.value = "0"
						end if
						exit function
				case 16:
					    document.Form1.ctrlFocus.value = "0"
						exit function
			'			oRE.Pattern="(^[A-Z]{6}\d{2}[A-Z]\d{2}[A-Z]\d{3}[A-Z]$)"
			'			Set oMatches = oRe.Execute(inpStr)
			'			if oMatches.count=0 then
			'				MsgBox "Codice Fiscale non corretto",vbCritical,"Errore inserimento Partita IVA" 
			'				oggetto.value=""
			'				oggetto.focus()
			'			end if
				case else
						msgbox "Errore Partita Iva o Codice Fiscale",vbCritical,"Errore inserimento Partita Iva/Codice Fiscale" 
						oggetto.value=""
						oggetto.focus()
						document.Form1.ctrlFocus.value = "1"
						exit function
			end select
		end function
		
		