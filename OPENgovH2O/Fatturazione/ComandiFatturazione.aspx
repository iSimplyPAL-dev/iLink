<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiFatturazione.aspx.vb" Inherits="OpenUtenze.ComandiFatturazione"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiFatturazione</title>
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
	</HEAD>
	<BODY leftMargin="0" topMargin="6" marginwidth="0" marginheight="0" class="SfondoGenerale">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td><asp:label id="Comune" CssClass="ContentHead_Title" Width="100%" runat="server"></asp:label></td>
				<td align="right" colSpan="2" rowSpan="2" width="50%">
					<input style="DISPLAY:none" class="Bottone BottoneCreaFile" id="CreaFile" title="Estrazione 290" onclick="parent.Visualizza.Estrazione290()" type="button" name="CreaFile">
					<!--*** 201511 - template documenti per ruolo ***-->
                    <input class="Bottone BottoneDownload" id="DownloadTemplate" title="Download Template" onclick="parent.Visualizza.DownloadTemplate();" type="button" name="DownloadTemplate"> 
					<input class="Bottone BottoneUpload" id="UploadTempalte" title="Upload Template" onclick="parent.Visualizza.UploadTemplate();" type="button" name="UploadTemplate"> 
					<input class="Bottone BottoneWord" id="ElaborazioneDocumenti" title="Elaborazione Documenti" onclick="parent.Visualizza.ElaborazioneDocumenti();" type="button" name="ElaborazioneDocumenti">
					<input class="Bottone BottoneNumerazione" id="NumerazioneFatturazione" title="Numerazione Fatturazione" onclick="parent.Visualizza.NumerazioneFatturazione();" type="button" name="NumerazioneFatturazione"> 
					<input class="Bottone BottoneCancella" onclick="parent.Visualizza.CancellaMinuta();" type="button" id="Cancella" name="Cancella" title="Elimina l'approvazione della Minuta Fatturazione H2O">
					<input class="Bottone BottoneFolderAccept" id="ApprovaMinuta" title="Approva Minuta Fatturazione H2O" onclick="parent.Visualizza.ApprovaMinuta();" type="button" name="ApprovaMinuta"> 
					<input class="Bottone BottoneExcel" id="StampaMinuta" title="Stampa Minuta Fatturazione H2O" onclick="parent.Visualizza.StampaMinuta();" type="button" name="StampaMinuta"> 
					<input class="Bottone BottoneCancellaLista" onclick="parent.Visualizza.DeleteFatturazione();" type="button" id="DeleteFatturazione" name="DeleteFatturazione" title="Elimina la Fatturazione H2O">
					<input class="Bottone BottoneCalendario" onclick="parent.Visualizza.ConfigRate();" type="button" id="ConfigRate" name="ConfigRate" title="Configurazione Rate">
					<input class="Bottone BottoneApri" onclick="parent.Visualizza.Visualizza();" type="button" id="Visualizza" name="Cancella" title="Visualizza Fatturazione H2O">
					<input class="Bottone BottoneCalcolo" id="ElaboraFatturazione" title="Effettua Fatturazione" onclick="parent.Visualizza.Fatturazione();" type="button" name="ElaboraFatturazione">
					&nbsp;
				</td>
			</tr>
			<tr>
				<td align="left"><asp:label id="info" CssClass="NormalBold_title" Width="100%" runat="server"></asp:label></td>
			</tr>
		</table>
	</BODY>
</HTML>
