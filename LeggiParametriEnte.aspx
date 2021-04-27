<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LeggiParametriEnte.aspx.vb" Inherits="OPENgov.LeggiParametriEnte" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>LeggiParametriEnte</title>
		<meta name="vs_showGrid" content="False">
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	    <script type="text/javascript" src="_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="_js/Utility.js?newversion"></script>
		<script type="text/javascript" >
			function DoReload(){
				document.location.reload();
			}
			// faccio ricaricare la pagina ogni dieci secondi per vedere la coda dell'elaborazione delle anagrafike
			//window.setTimeout("DoReload()", 5000);
		</script>
	</HEAD>
	<body class="SfondoVisualizza">
		<form id="Form1" runat="server" method="post">
			<fieldset class="classeFiledSetRicerca" id="AttivitaPendentiAnater" runat=server style="display:none;">
				<legend class="Legend">Attività pendenti Anagrafiche</legend>
				&nbsp;<br>
				<table width="100%" cellspacing="0" id="AnagraficheNonRibaltate" runat="server">
					<tr>
						<td><asp:Label Runat="server" ID="lblAnagrafiche" CssClass="Input_Label"></asp:Label></td>
						<td width="35"><asp:Button ID="btnRibaltaAnagrafiche" Runat="server" Text="OK"></asp:Button></td>
					</tr>
				</table>
				<table width="100%" cellspacing="0" id="tblNonConfigurato" runat="server">
					<tr>
						<td>
							<asp:Label Runat="server" ID="lblNonConfigurato" CssClass="Input_Label">
								Nessuna Scadenza configurata per il ribaltamento delle anagrafiche.		
							</asp:Label>
						</td>
					</tr>
				</table>
				<table width="100%" cellspacing="0" id="tblRibaltamentoInCorso" runat="server">
					<tr>
						<td align="center"><label class="Input_Label">Ribaltamento Anagrafiche in corso</label></td>
					</tr>
					<tr>
						<td align="center"><img src="images/loading.png"></td>
					</tr>
					<tr>
						<td align="center"><label class="Input_Label">Attendere Prego</label></td>
					</tr>
				</table>
			</fieldset>
            <div id="divRelease" class="col-md-12">
                <div id="divListRelShort"></div>
                <div id="divListRelComplete"></div>
            </div>
            <div id="divDialogBox" class="col-md-12">
                <div class="modal-box">
                    <div id="divAlert" class="modal-alert">
                        <span class="closebtnalert" onclick="CloseAlert()">&times;</span>
                        <p id="pAlert">testo di esempio</p>
                        <input type="text" class="prompttxt"/>
                        <button id="btnDialogBoxeOK" class="confirmbtn Bottone BottoneOK" onclick="DialogConfirmOK();"></button>&nbsp;
                        <button id="btnDialogBoxeKO" class="confirmbtn Bottone BottoneKO" onclick="DialogConfirmKO();"></button>
                        <button id="CmdDialogConfirmOK" class="hidden" onclick="RaiseDialogConfirmOK();"></button>
                        <button id="CmdDialogConfirmKO" class="hidden" onclick="RaiseDialogConfirmKO();"></button>
                        <input type="hidden" id="hfCloseAlert" />
                        <input type="hidden" id="hfDialogOK" />
                        <input type="hidden" id="hfDialogKO" />
                    </div>
                </div>
                <input type="hidden" id="cmdHeight" value="0" />
            </div>
            <asp:Button ID="CmdLogOut" runat="server" Text="" CssClass="hidden" OnClick="CmdLogOut_Click"></asp:Button>
		</form>
	</body>
</HTML>
