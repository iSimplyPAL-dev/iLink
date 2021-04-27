<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiOperazioniSuTabelle.aspx.vb" Inherits="OpenUtenze.ComandiOperazioniSuTabelle" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title></title>
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
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">
			function Salva()
			{
			    parent.Visualizza.iframetabella.document.getElementById('btnSalva').click();
			}
			function Cancella()
			{
				if (confirm('Si desidera eseguire la cancellazione dell\'elemento selezionato?'))
				{	
				    parent.Visualizza.iframetabella.document.getElementById('btnCancella').click();
				}		
				else
				{
					return false;
				}
			}
			function Annulla()
			{
			    parent.Visualizza.iframetabella.document.getElementById('btnAnnulla').click();
	
			}
	</script>
	</HEAD>
	<BODY class="SfondoGenerale" bottomMargin="0" rightmargin="2" leftMargin="2" topMargin="10" marginwidth="0" marginheight="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td><asp:label id="Comune" CssClass="ContentHead_Title" Width="504px" runat="server"></asp:label></td>
					<td align="right" colSpan="2" rowSpan="2" width="120">
						<input class="Bottone BottoneCancella" onclick="Cancella();" type="button" id="CANCELLA" name="CANCELLA" title="Consente di eliminare l'elemento selezionato">
						<input class="Bottone BottoneSalva" onclick="Salva();" type="button" name="SALVA" title="Salva i dati inseriti o modificati nella Tabella">
						<input class="Bottone Bottoneannulla" onclick="Annulla();" type="button" name="ANNULLA" title="Torna alla pagina di visualizzazione dati">
					</td>
				</tr>
				<tr>
					<td align="left">
						<asp:label id="info" CssClass="NormalBold_title" Width="580" runat="server"></asp:label>
					</td>
				</tr>
			</table>
		</form>
	</BODY>
</HTML>
