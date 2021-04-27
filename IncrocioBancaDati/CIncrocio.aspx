<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CIncrocio.aspx.vb" Inherits="OPENgov.CIncrocio"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
    <title>CIncrocio</title>
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end If%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
</HEAD>
  <body onload="VisualizzaLabel();" class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66"
		leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td align="left" width=464 height=18><asp:label id="infoEnte" runat="server" Width="400px" CssClass="ContentHead_Title"></asp:label></td>
				<td align="right" width="800" colSpan="2" rowSpan="2">
						<INPUT class="Bottone BottoneBonifica" id="UpdateFigli" title="Aggiorna Figli da Residenti" onclick="parent.Visualizza.UpdateFigli()" type="button" name="UpdateFigli"> 
						<input class="Bottone BottoneExcel"	id="Excel" title="Estrae in Excel" onclick="parent.Visualizza.Excel()" type="button" name="Excel"> 
						<input class="Bottone BottoneRicerca" id="Search" title="Avvia Ricerca" onclick="parent.Visualizza.Ricerca()" type="button" name="Search"> 
				</td>
			</tr>
			<tr>
				<td align="left" width=463>
					<asp:label id="info" CssClass="NormalBold_title" Width="524px" runat="server" Height="20px"
						ForeColor="white"></asp:label>
				</td>
			</tr>
		</table>
	</body>
</HTML>
