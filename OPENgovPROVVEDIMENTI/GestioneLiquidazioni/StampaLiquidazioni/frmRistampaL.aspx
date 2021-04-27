<%
    Dim Tributo
    Tributo = Request.Item("CODTRIBUTO")
    Dim tipodoc
    tipodoc =request.item("TIPODOC")
    'dim tipostampa
    'tipostampa=request.item("TIPOSTAMPA")
%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<TITLE>Stampa</TITLE>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<frameset rows="50,*,0">
		<frame src="comandiRistampaL.aspx" NAME="comandi" frameborder="0" noresize>
		<frame src="../../../aspvuota.aspx" NAME="corpo" frameborder="0">
		<frame src="ristampaOriginaleL.aspx?CODTRIBUTO=<%=Tributo%>&tipodoc=<%=tipodoc%>" NAME="nascosto" frameborder="0">
	</frameset>
</HTML>
