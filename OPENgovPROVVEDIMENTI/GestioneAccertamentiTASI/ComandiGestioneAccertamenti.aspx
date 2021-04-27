<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ComandiGestioneAccertamenti.aspx.vb" Inherits="Provvedimenti.ComandiGestioneAccertamenti1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
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
		    parent.Comandi.info.innerText = 'Accertamenti TASI - Gestione'
		}
		</script>
	</head>
	<body onload="VisualizzaLabel();" class="SfondoGenerale" bottomMargin="0" leftMargin="2" topMargin="6" rightMargin="2">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left"><asp:label id="infoEnte" runat="server" Width="400px" CssClass="ContentHead_Title"></asp:label></td>
				<td align="right" width="800" colSpan="2" rowSpan="2">
					<input class="Bottone BottoneElabora" id="btnAccertamento" title="Accerta" onclick="parent.Visualizza.EseguiAccertamento();" type="button" name="btnAccertamento" />
					<input class="Bottone BottoneRibalta" id="btnRibaltaImmobile" title="Ribalta Immobile" onclick="parent.Visualizza.RibaltaImmobileAccertamento();" type="button" name="btnRibaltaImmobile" />
				</td>
			</tr>
			<tr>
				<td style="WIDTH: 463px" align="left">
					<asp:label id="info" CssClass="NormalBold_title" Width="524px" runat="server" Height="20px"></asp:label>
				</td>
			</tr>
		</table>
	</body>
</html>
