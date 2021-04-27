<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrameRicercaAccertato.aspx.vb" Inherits="Provvedimenti.FrameRicercaAnater" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD><TITLE>Ricerca Immobili Accertati</TITLE>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	<%
        Dim Source
        Dim Comandi
        'Dim codContribuente
        'Dim anno
        'Dim nominativo
        'Dim tributo
        'codContribuente = Request.Item("codContribuente")
        'anno = Request.Item("anno")
        'nominativo = Request.Item("nominativo")
        'tributo = Request.Item("tributo")
        Source = "SearchAccertato.aspx" '?tributo=" & tributo & "&codContribuente=" & codContribuente & "&anno=" & anno & "&nominativo=" & nominativo & "&provenienza=" & Request.Item("provenienza")
        Comandi = "ComandiGestioneAccertato.aspx"
	%>
	</HEAD>
	<frameset rows="50,*" framespacing="0" border="0" frameborder="no">
		<frame name="Comandi" src="<%=Comandi%>" scrolling="no" noresize>
		<FRAME name="Visualizza" src="<%=Source %> " noresize>
	</frameset>
</HTML>
