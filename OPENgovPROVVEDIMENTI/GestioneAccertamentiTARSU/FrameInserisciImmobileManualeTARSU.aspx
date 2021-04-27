<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrameInserisciImmobileManualeTARSU.aspx.vb" Inherits="Provvedimenti.FrameInserisciImmobileManualeTARSU"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<TITLE>Inserimento Manuale Immobile TARSU</TITLE>
		<%
	dim Source
	dim codContribuente
	dim idProgressivo
	dim anno
	dim provenienza
	codContribuente = Request.item("codContribuente")
	anno = Request.item("anno")
	idProgressivo=Request.item("idProgressivo")
	provenienza=Request.item("provenienza")
	Source="InserimentoManualeImmobileTARSU.aspx?codContribuente=" & codContribuente & "&anno=" & anno & "&idProgressivo=" & idProgressivo & "&provenienza=" & provenienza
%>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<frameset rows="0,*" framespacing="0" border="0" frameborder="no">
		<FRAME id="Comandi" name="Comandi" scrolling="no" noresize>
		<FRAME id="Visualizza" name="Visualizza" src="<%=Source %> " scrolling="auto" noresize>
	</frameset>
</HTML>
