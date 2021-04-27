<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiDocumenti.aspx.vb" Inherits="OpenUtenze.ComandiDocumenti"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiCreazioneDocumenti</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">		
		function checkElaboraDoc()
		{
			//alert(parent.Visualizza.frames('loadGrid').document.getElementById('GrdFattura'));
		    //if (parent.Visualizza.frames('loadGrid').document.getElementById('GrdFattura') == null)
		    var myIFrame = parent.Visualizza.document.getElementById('loadGrid');
            var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
            if (myContent.document.getElementById('GrdFattura') == null)
            {
				alert("Per effettuare la stampa massiva dei Documenti e\' necessario effettuare la ricerca!");
				return false;
			}
			else
			{
				parent.Visualizza.ElaborazioniDocumenti();
			}
		}		
		</script>
	</HEAD>
	<body class="SfondoGenerale" bottomMargin="0" leftMargin="0" topMargin="6" rightMargin="0"
		MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td><asp:label id="Comune" CssClass="ContentHead_Title" Width="100%" runat="server"></asp:label></td>
					<td align="right" colSpan="2" rowSpan="2" width="40%">
						<input class="Bottone Bottonecancella" id="EliminaElabora" title="Eliminazione Elaborazione" onclick="parent.Visualizza.EliminaElaborazione()" type="button" name="EliminaElabora"> 
                        <input class="Bottone BottoneFolderAccept" id="ApprovaMinuta" title="Approvazione Elaborazione" onclick="parent.Visualizza.ApprovaDocumenti();" type="button" name="ApprovaMinuta"> 
                        <input class="Bottone BottoneCreaFile" id="ExpFatEle" title="Estrai Fatture Elettroniche" onclick="DivAttesa.style.display = ''; parent.Visualizza.document.getElementById('CmdExpFat').click();" type="button" name="ExpFatEle">
                        <input class="Bottone BottoneWord" id="ElaborazioneDocumenti" title="Elaborazione Documenti" onclick="checkElaboraDoc();" type="button" name="ElaborazioneDocumenti">
						<input class="Bottone Bottonericerca" id="Search" title="Ricerca" onclick="parent.Visualizza.Search()" type="button" name="Search"> 
                        <input class="Bottone BottoneAnnulla" id="Cancel" title="Uscita" onclick="parent.Visualizza.Fatturazione()" type="button" name="Cancel"> &nbsp;
					</td>
				</tr>
				<tr>
					<td align="left"><asp:label id="info" CssClass="NormalBold_title" Width="100%" runat="server"></asp:label></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
