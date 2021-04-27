<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CGestioneAccertamentiOSAP.aspx.vb" Inherits="Provvedimenti.CGestioneAccertamentiOSAP"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CGestioneAccertamentiOSAP</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%End If%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script>
			function VisualizzaLabel(){
				//parent.Comandi.info.innerText="<%=Session("DESC_TIPO_PROC_SERV")%>"
				parent.Comandi.infoEnte.innerText="<%=session("DESCRIZIONE_ENTE")%>"
			    parent.Comandi.info.innerText = 'Accertamenti TOSAP/COSAP - Gestione'
			}
			
		</script>
	</HEAD>
	<body onload="VisualizzaLabel();" class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66"
		leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left"><asp:label id="infoEnte" runat="server" Width="400px" CssClass="ContentHead_Title"></asp:label></td>
				<td align="right" width="800" colSpan="2" rowSpan="2">
					<INPUT style="DISPLAY:none" class="Bottone BottoneJudgeHammer" id="CmdAnnulloNoAcc" title="Crea Autotutela di annullamento senza accertato" onclick="parent.Visualizza.document.getElementById('BtnAnnulloNoAcc').click()" type="button" name="CmdAnnullo"> 
					<INPUT style="DISPLAY:none" class="Bottone BottoneUserHome hidden" id="VersContribuente" title="Visualizza Versamenti del Contribuente" onclick="parent.Visualizza.gotoVersContribuente();" type="button" name="VersContribuente"> 
					<INPUT class="Bottone BottoneRicerca" id="Search" title="Cerca Dichiarato da accertare" style="DISPLAY:none" onclick="parent.Visualizza.checkDati();return false;" type="button" name="Search">
					<INPUT class="Bottone BottoneElabora" id="btnAccertamento" title="Accerta" onclick="parent.Visualizza.EseguiAccertamento();" type="button" name="btnAccertamento"> 
					<INPUT class="Bottone BottoneRibalta" id="btnRibaltaImmobile" title="Ribalta Immobile" onclick="parent.Visualizza.RibaltaImmobileAccertamento();" type="button" name="btnRibaltaImmobile">
				</td>
			</tr>
			<TR>
				<TD style="WIDTH: 463px" align="left">
					<asp:label id="info" CssClass="NormalBold_title" Width="524px" runat="server" Height="20px"></asp:label></TD>
			</TR>
		</table>
	</body>
</HTML>
