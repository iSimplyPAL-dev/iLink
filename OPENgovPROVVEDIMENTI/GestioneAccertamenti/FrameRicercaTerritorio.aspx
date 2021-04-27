<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrameRicercaTerritorio.aspx.vb" Inherits="Provvedimenti.FrameRicercaTerritorio" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
		<HEAD>
				<TITLE>Ricerca Immobili Territorio</TITLE>
<%
	dim Source
	dim codContribuente
	dim anno
	dim nominativo
	codContribuente = Request.item("codContribuente")
	anno = Request.item("anno")
	nominativo = Request.item("nominativo")
	Source="SearchTerritorio.aspx?codContribuente=" & codContribuente & "&anno=" & anno & "&nominativo=" & nominativo
%>
				<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
				<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
				<meta name="vs_defaultClientScript" content="JavaScript">
				<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		</HEAD>
		<frameset rows="50,*" framespacing="0" border="0" frameborder="no">
				<FRAME name="Comandi" scrolling="no" noresize>
				<FRAME name="Visualizza" src="<%=Source %> " scrolling="auto" noresize>
		</frameset>
</HTML>
