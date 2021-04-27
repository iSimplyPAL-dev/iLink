<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrmRicercaImmobile.aspx.vb" Inherits="OpenUtenze.FrmRicercaImmobile" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>FrmRicercaImmobile</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
	<frameset rows="45,*,0,0" framespacing="0" border="1" frameborder="no" id="frameVisualizza">
		<FRAME name="Comandi" src="ComandiRicImmobile.aspx?Provenienza=<%=Request.item("Provenienza")%>" scrolling="no" noresize>
		<FRAME name="Visualizza" src="RicercaImmobile.aspx?Provenienza=<%=Request.item("Provenienza")%>&Via=<%=Request.item("Via")%>" scrolling="no" noresize>
		<FRAME name="Basso" src="../../aspVuota.aspx" scrolling="yes" noresize>
		<FRAME name="Nascosto" src="../../aspVuota.aspx" scrolling="yes" noresize>
	</frameset>
</html>
