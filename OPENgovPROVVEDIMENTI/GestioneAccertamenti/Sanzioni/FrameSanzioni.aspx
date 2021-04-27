<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD><TITLE>Sanzioni</TITLE>
		<%
	dim Source
	dim anno
	dim idLegame
	dim strSanzioni
	dim bloccaCheck 
	dim id_provvedimento
	
	anno = Request.item("anno")
	idLegame = Request.item("idLegame")
	strSanzioni = Request.item("strSanzioni")  
	bloccaCheck = Request.item("bloccaCheck")  
	id_provvedimento = Request.item("id_provvedimento")  
	Source="Sanzioni.aspx?anno=" & anno & "&idLegame=" & idLegame & "&strSanzioni=" & strSanzioni & "&bloccaCheck=" & bloccaCheck & "&id_provvedimento=" & id_provvedimento 
%>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<frameset rows="50,*" framespacing="0" border="0" frameborder="no">
		<FRAME ID="Comandi" name="Comandi" scrolling="no" noresize>
		<FRAME ID="Visualizza" name="Visualizza" src="<%=Source %> " noresize>
	</frameset>
</HTML>
