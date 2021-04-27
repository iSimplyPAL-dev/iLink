<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiInserimentoImmobili.aspx.vb" Inherits="Provvedimenti.ComandiInserimentoImmobili"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiEnte</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script>
		function VisualizzaLabel(){
						
			//parent.Comandi.info.innerText="<%=Session("DESC_TIPO_PROC_SERV")%>"
			parent.Comandi.infoEnte.innerText="<%=Session("DESCRIZIONE_ENTE")%>"
			parent.Comandi.info.innerText= "<%=Session("DESC_TIPO_PROC_SERV") %>" + 'Accertamenti ICI/IMU - Inserimento Manuale Immobile'
			
		}
		</script>
	</HEAD>
	<body onload="VisualizzaLabel();" class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66"
		leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left"><asp:label id="infoEnte" runat="server" Width="400px" CssClass="ContentHead_Title"></asp:label></td>
				<td align="right" width="800" colSpan="2" rowSpan="2">
					<INPUT class="Bottone BottoneAssocia" id="btnAssocia" title="Associa gli immobili al soggetto" onclick="parent.Visualizza.document.getElementById('btnRibalta').click()" type="button" name="btnAssocia"> 
                    <INPUT class="Bottone BottonePulisci hidden" id="Clear" title="Pulisci videata per nuova Associazione" onclick="parent.Visualizza.PulisciCampi()" type="button" name="Clear">
					<INPUT class="Bottone Bottoneannulla" id="Cancel" title="Esci" onclick="parent.window.close()" tabIndex="6" type="button" name="Cancel">
				</td>
			</tr>
			<TR>
				<TD style="WIDTH: 463px" align="left">
					<asp:label id="info" CssClass="NormalBold_title" Width="524px" runat="server" Height="20px"></asp:label></TD>
			</TR>
		</table>
	</body>
</HTML>
