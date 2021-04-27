<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrameInserisciImmobileManuale.aspx.vb" Inherits="Provvedimenti.FrameInserisciImmobileManuale"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<TITLE>Inserimento Manuale Immobile</TITLE>
		<%
            dim Source
            dim codContribuente
            dim idProgressivo
            dim anno
            codContribuente = Request.Item("CodContribuente")
            anno = Request.Item("annoAccertamento")
            idProgressivo =Request.item("idProgressivo")
            '*** 20130304 - gestione dati da territorio ***
            dim Parametri="&txtIdTerUI=" & Request.item("txtIdTerUI")
            Parametri+="&txtIdTerProprieta=" & Request.item("txtIdTerProprieta")
            Parametri+="&txtIdTerProprietario=" & Request.item("txtIdTerProprietario")
            Source="InserimentoManualeImmobile.aspx?codContribuente=" & codContribuente & "&anno=" & anno & "&idProgressivo=" & idProgressivo & Parametri
            '*** ***
		%>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<frameset rows="50,*" framespacing="0" border="0" frameborder="no">
		<FRAME name="Comandi" scrolling="no" noresize>
		<FRAME name="Visualizza" src="<%=Source %> " noresize>
	</frameset>
</HTML>
