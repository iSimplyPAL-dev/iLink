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