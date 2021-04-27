<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiLetture.aspx.vb" Inherits="OpenUtenze.ComandiLEtture" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Comandi Ricerca Contatori</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script src="../../_js/VerificaCampi.js?newversion" language="Javascript"></script>
		<script type="text/javascript">
			function Ricerca()
			{
			    parent.Visualizza.document.getElementById('txtRicerca').value = '1';
				parent.Visualizza.Search();
			}
		</script>
	</HEAD>
	<BODY leftMargin="0" topMargin="6" marginwidth="0" marginheight="0" class="SfondoGenerale">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td>
					<asp:label id="Comune" CssClass="ContentHead_Title" Width="100%" runat="server"></asp:label>
				</td>
				<td align="right" rowspan="2" width="30%">
					<input class="Bottone BottoneStampaContatori" id="ExcelCessati" title="Stampa contatori cessati, sostituiti, sospesi" onclick="parent.Visualizza.Ex2();" type="button" name="Excel2">
					<input class="Bottone BottoneStampaLettureNonPresenti" id="ExcelNonPresenti" title="Stampa letture non presenti" onclick="parent.Visualizza.Ex4();" type="button" name="Excel4">
					<input class="Bottone BottoneExcel" id="ExcelLetture" title="Stampa letture" onclick="parent.Visualizza.Ex3();" type="button" name="Excel3"> 
					<input class="Bottone BottoneStampaLetturista" id="Excel" title="Stampa letturista" onclick="parent.Visualizza.Ex();" type="button" name="Excel"> 
					<input class="Bottone BottoneRicerca" title="Avvia la ricerca..." onclick="Ricerca();" type="button" name="Ricerca"> 
					<!--<input class="Bottone BottoneAnnulla" title="Pulisce i Campi di Ricerca ed elimina filtro" onclick="parent.Visualizza.PulisciCampi();" type="button" name="Annulla">-->
					&nbsp;
				</td>
			</tr>
			<tr>
				<td align="left">
					<asp:label id="info" CssClass="NormalBold_title" Width="100%" runat="server"></asp:label>
				</td>
			</tr>
		</table>
	</BODY>
</HTML>
