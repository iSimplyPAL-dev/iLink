<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AcquisizioneLetture.aspx.vb" Inherits="OpenUtenze.AcquisizioneLetture"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>AcquisizioneLetture</title>
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
		<script type="text/javascript" src="../../../_js/MyUtility.js?newversion"></script>
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript">
			function VisualizzaElaborazione(){
				DivAttesa.style.display='';
				divAcquisizione.style.display='none';
				document.getElementById('fileUpload').disabled=true;
				parent.Comandi.document.getElementById("Importa").disabled=true;
				window.setTimeout('caricaPagina()', 10000);
			}
			
			function caricaPagina(){
				document.location.href='AcquisizioneLetture.aspx';
			}
			
			function VisualizzaForm(){
				DivAttesa.style.display='none';
				divAcquisizione.style.display='';
				document.getElementById('fileUpload').disabled=false;
				parent.Comandi.document.getElementById("Importa").disabled=false;
			}
			</script>
	</HEAD>
	<BODY class="Sfondo" bottomMargin="0" leftMargin="5" topMargin="0" marginheight="0"
		marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<DIV>
				<table cellSpacing="0" cellPadding="0" width="100%" border="0">
					<tr>
						<td width="100%">&nbsp;</td>
					</tr>
					<tr>
						<td width="100%" class="Input_Label">
							Per una corretta importazione, si ricorda che il file deve:
                            <ul type="disc">
							    <li>avere il nome che inizia con il codice dell'ente in elaborazione</li>
							    <li>essere in formato CSV</li>
							    <li>avere come separatore il carattere <i>;</i>     <em>(punto e virgola)</em></li>
							    <li>avere come campi, nell'ordine:
                                    <ul type="disc">
                                        <li>CODICE CONTATORE</li>
                                        <li>DATA LETTURA</li>
                                        <li>LETTURA</li>
                                        <li>NUMERO UTENTE</li>
                                    </ul>
                                </li>
                            </ul>
						</td>
					</tr>
					<tr>
						<td width="100%">&nbsp;</td>
					</tr>
					<tr>
						<td>
							<fieldset class="FiledSetRicerca"><legend class="Legend">Scegli la tipologia di 
									Acquisizione</legend>
								<table cellSpacing="1" cellPadding="5" width="100%" border="0">
									<tr>
										<td style="WIDTH: 77px"><asp:label id="lblPercorso" Runat="server" CssClass="Input_Label">Percorso</asp:label></td>
										<td align="left"><input class="Input_Label" id="fileUpload" type="file" size="81" name="fileUpload" runat="server"
												style="WIDTH: 606px; HEIGHT: 19px">
										</td>
									</tr>
								</table>
							</fieldset>
						</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>
							<div id="divAcquisizione">
								<FIELDSET class="FiledSetRicerca"><LEGEND class="Legend">Dati ultima Importazione</LEGEND>
									<table cellSpacing="1" cellPadding="4" width="100%" border="0">
										<tr>
											<td style="WIDTH: 162px"><asp:label id="lblTipoFile" Runat="server" CssClass="Input_Label">Nome File Importato</asp:label></td>
											<td><asp:label id="lblNomeFile" Runat="server" CssClass="Input_Label"></asp:label></td>
										</tr>
										<tr>
											<td colspan="2">&nbsp;</td>
										</tr>
										<tr>
											<td style="WIDTH: 162px"><asp:label id="lblTitoloEsito" Runat="server" CssClass="Input_Label">Esito Importazione </asp:label></td>
											<td><b><asp:label id="lblEsito" Runat="server" CssClass="Input_Label"></asp:label></b></td>
										</tr>
										<tr>
											<td style="WIDTH: 162px"><asp:label id="lblScarto" Runat="server" CssClass="Input_Label">File Scarto </asp:label></td>
											<td><b><asp:label id="lblLinkFileScarto" Runat="server" CssClass="Input_Label" Font-Underline="True"></asp:label></b></td>
										</tr>
										<tr>
											<td colspan="2">&nbsp;</td>
										</tr>
										<tr>
											<td style="WIDTH: 162px"><asp:label id="lblTRecDaImp" Runat="server" CssClass="Input_Label">Totale Record da Importare</asp:label></td>
											<td><asp:label id="lblTotRecDaImport" Runat="server" CssClass="Input_Label"></asp:label></td>
										</tr>
										<tr>
											<td colspan="2">&nbsp;</td>
										</tr>
										<tr>
											<td style="WIDTH: 162px"><asp:label id="lblTRecImp" Runat="server" CssClass="Input_Label" Width="139px">Totale Record Importati</asp:label></td>
											<td><asp:label id="lblImportoTotF" Runat="server" CssClass="Input_Label"></asp:label></td>
										</tr>
										<tr>
											<td style="WIDTH: 162px"><asp:label id="lblRecScart" Runat="server" CssClass="Input_Label">Totale Record Scartati</asp:label></td>
											<td><asp:label id="lblTotRecScartati" Runat="server" CssClass="Input_Label"></asp:label></td>
										</tr>
										<tr>
											<td colspan="2">&nbsp;</td>
										</tr>
									</table>
								</FIELDSET>
							</div>
						</td>
					</tr>
				</table>
			</DIV>
			<DIV>
				<asp:button id="btnImporta" style="DISPLAY: none" runat="server" Text="Importa"></asp:button>
				<asp:button id="CmdScarica" style="DISPLAY: none" runat="server"></asp:button>
				<input id="paginacomandi" type="hidden" name="paginacomandi">
			</DIV>
            <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                <div class="BottoneClessidra">&nbsp;</div>
                <div class="Legend">Attendere Prego</div>
            </div>
		</form>
	</BODY>
</HTML>
