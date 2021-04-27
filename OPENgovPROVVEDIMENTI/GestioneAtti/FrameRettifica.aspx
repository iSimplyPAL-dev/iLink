<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<TITLE>Rettifica</TITLE>
		<%
            Dim Source
            'M.B. il codice a seguire basato suui parametri dell URL è stato commentato dopo il passaggio al nuovo framework
            'dim nome,cognome
            'Dim Parametri
            'Parametri = ""
            'Parametri = "?COD_CONTRIBUENTE=" & Request.Item("COD_CONTRIBUENTE")
            'Parametri = Parametri + "&ID_PROVVEDIMENTO=" & Request.Item("ID_PROVVEDIMENTO")
            'Parametri = Parametri + "&ANNO=" & Request.Item("ANNO")
            'Parametri = Parametri + "&DATA_ELABORAZIONE=" & Request.Item("DATA_ELABORAZIONE")
            'Parametri = Parametri + "&COD_TRIBUTO=" & Request.Item("COD_TRIBUTO")
            'Parametri = Parametri + "&TIPO_OPERAZIONE=" & Request.Item("TIPO_OPERAZIONE")
            'Parametri = Parametri + "&TIPO_RICERCA=" & Request.Item("TIPO_RICERCA")
            'Parametri = Parametri + "&DATADAAGGIORNARE=" & Request.Item("DATADAAGGIORNARE")
            'Parametri = Parametri + "&NOMINATIVO=" & Request.Item("NOMINATIVO")

            'dim sTIPO_PROC=Request.Item("TIPO_PROCEDIMENTO")
            Dim sCOD_TRIBUTO=Request.Item("COD_TRIBUTO")

            If sCOD_TRIBUTO = "8852" Then
                'if sTIPO_PROC="A" then
                Source = "..\GestioneAccertamenti\GestioneAccertamenti.aspx" '& Parametri
                'Else
                'Source="..\GestioneLiquidazioni\MasterDetailLiquidazioni.aspx" & Parametri
                'end if
            ElseIf sCOD_TRIBUTO = "TASI" Then
                Source = "..\GestioneAccertamentiTASI\GestioneAccertamenti.aspx" '& Parametri
                '*** 20130801 - accertamento OSAP ***
            ElseIf sCOD_TRIBUTO = "0453" Then
                Source = "..\GestioneAccertamentiOSAP\GestioneAccertamentiOSAP.aspx" '& Parametri
            Else
                Source = "..\GestioneAccertamentiTARSU\GestioneAccertamentiTARSU.aspx" '& Parametri
                'Source = "..\GestioneAccertamentiTARSU\GestioneAccertamentiTARSU.aspx&" + Parametri
            End if
%>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<frameset rows="50,*,0" framespacing="0" border="0" frameborder="no">
		<FRAME name="Comandi" scrolling="no" noresize>
		<FRAME name="Visualizza" src="<%=Source %> " noresize>
		<FRAME name="nascosto" scrolling="no" noresize>
	</frameset>
</HTML>
