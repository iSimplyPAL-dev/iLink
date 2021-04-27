<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiDettaglio.aspx.vb" Inherits="ComandiDettaglio" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
		<HEAD>
				<title>ComandiGestioneRuolo</title>
				<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
				<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
				<meta content="JavaScript" name="vs_defaultClientScript">
				<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end If%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		    function VisualizzaLabel(){
			    parent.Comandi.infoEnte.innerText="<%=Session("DESCRIZIONE_ENTE")%>"
			    parent.Comandi.info.innerText="Anagrafica  -  Consultazione"
		    }
		</script>
		</HEAD>
		<body onload="VisualizzaLabel();" class="SfondoGenerale" bottomMargin="0"  leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
			<form id="Form1" runat="server" method="post">
				<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
					<tr>
						<td style="WIDTH: 501px; HEIGHT: 18px" align="left"><asp:label id="infoEnte" runat="server" Width="496px" CssClass="ContentHead_Title"></asp:label></td>
						<td align="right" width="800" colSpan="2" rowSpan="2">&nbsp; 
							<INPUT class="Bottone Bottoneannulla" id="Cancel" title="Torna Indietro" onclick="parent.Visualizza.document.getElementById('btnAnnulla').click()" type="button" name="Cancel">
						</td>
					</tr>
					<tr>
						<td style="WIDTH: 501px" align="left"><asp:label id="info" runat="server" Width="496px" CssClass="NormalBold_title" Height="20px"></asp:label></td>
					</tr>
				</table>
			</form>
		</body>
</HTML>
