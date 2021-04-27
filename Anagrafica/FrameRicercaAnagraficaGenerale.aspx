<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD><TITLE>Anagrafica</TITLE>
		<%

	dim Source
	dim sessionName
	sessionName = Request.Item("sessionName")
	Source="RicercaAnagrafica.aspx?popup=1&sessionName=" & sessionName
%>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<frameset rows="50,*" framespacing="0" border="0" frameborder="no">
		<FRAME name="Comandi" id="Comandi" scrolling="no" noresize>
		<FRAME name="Visualizza" id="Visualizza" src="<%=Source %> " scrolling="auto" noresize>
	</frameset>
</HTML>
