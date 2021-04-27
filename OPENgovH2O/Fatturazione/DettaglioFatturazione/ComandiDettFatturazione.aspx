<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiDettFatturazione.aspx.vb" Inherits="OpenUtenze.ComandiDettFatturazione"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>ComandiDettFatturazione</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
  </HEAD>
	<BODY leftMargin="0" topMargin="6" marginwidth="0" marginheight="0" class="SfondoGenerale">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td><asp:label id="Comune" CssClass="ContentHead_Title" Width="100%" runat="server"></asp:label></td>
					<td align="right" rowSpan="2" width="40%">
						<input class="Bottone BottoneWord" id="ElaborazioneDocumenti" title="Elaborazione Documenti" onclick="parent.Visualizza.StampaDoc()" type="button" name="ElaborazioneDocumenti">
						<input class="Bottone BottoneCalcolo" id="Ricalcolo" title="Ricalcola Fattura" onclick="parent.Visualizza.RicalcoloFattura()" type="button" name="Ricalcolo">
						<input class="Bottone BottoneApri" id="Modifica" title="Variazioni" onclick="parent.Visualizza.Modifica()" type="button" name="Modifica">
						<input class="Bottone BottoneCancella" id="Delete" title="Annulla Documento" onclick="parent.Visualizza.AnnulloDettFatturazione()" type="button" name="Delete">
						<input class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla ricerca" onclick="parent.Visualizza.ClearDatiDettFatturazione()" type="button" name="Cancel" style="visibility:visible;">
					</td>
				</tr>
				<tr>
					<td align="left"><asp:label id="info" CssClass="NormalBold_title" Width="100%" runat="server"></asp:label></td>
				</tr>
			</table>
		</form>
	</BODY>
</HTML>
