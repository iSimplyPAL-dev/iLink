<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrmRicercaImmobile.aspx.vb" Inherits="OPENgovTIA.FrmRicercaImmobile" %>
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
		<FRAME id="Comandi" name="Comandi" src="ComandiRicImmobile.aspx?Provenienza=<%=Request.item("sProvenienza")%>" scrolling="no" noresize>
		<FRAME id="Visualizza" name="Visualizza" src="RicercaImmobile.aspx?Provenienza=<%=Request.item("sProvenienza")%>&TxtCodVia=<%=Request.item("TxtCodVia")%>&TxtVia=<%=Request.item("TxtVia")%>&TxtCivico=<%=Request.item("TxtCivico")%>&TxtInterno=<%=Request.item("TxtInterno")%>&TxtFoglio=<%=Request.item("TxtFoglio")%>&TxtNumero=<%=Request.item("TxtNumero")%>&TxtSubalterno=<%=Request.item("TxtSubalterno")%>" scrolling="no" noresize>
		<FRAME id="Basso" name="Basso" src="../../aspVuota.aspx" scrolling="yes" noresize>
		<FRAME id="Nascosto" name="Nascosto" src="../../aspVuota.aspx" scrolling="yes" noresize>
	</frameset>
</html>
