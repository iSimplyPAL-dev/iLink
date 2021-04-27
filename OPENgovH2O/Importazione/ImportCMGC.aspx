<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ImportCMGC.aspx.vb" Inherits="OpenUtenze.ImportCMGC"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>ImportCMGC</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function VisualizzaElaborazione(){
				DivAttesa.style.display='';
				divAcquisizione.style.display='none';
				document.getElementById('fileUpload').disabled=true;
				parent.Comandi.document.getElementById("Importa").disabled=true;
				window.setTimeout('caricaPagina()', 10000);
			}
			
			function caricaPagina(){
				document.location.href='ImportCMGC.aspx';
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
						<td>
							<fieldset class="FiledSetRicerca">
								<table cellSpacing="1" cellPadding="5" width="100%" border="0">
									<tr>
										<td style="WIDTH: 77px">
											<asp:label id="LblFile" Runat="server" CssClass="Input_Label">Percorso</asp:label>
										</td>
										<td align="left">
											<input class="Input_Label" id="FileUpload" type="file" size="81" name="FileUpload" runat="server"
												style="WIDTH: 606px; HEIGHT: 19px">
										</td>
									</tr>
									<tr>
										<td>
											<asp:Label Runat="server" CssClass="Input_Label" id="Label1">ID Impianto</asp:Label>
										</td>
										<td>
											<asp:TextBox id="TxtIDImpianto" Runat="server" CssClass="Input_Text">01</asp:TextBox>
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
								<FIELDSET class="FiledSetRicerca">
									<LEGEND class="Legend">
										Dati ultima Importazione</LEGEND>
									<table cellSpacing="1" cellPadding="4" width="100%" border="0">
										<tr>
											<td style="WIDTH: 162px">
												<asp:label id="LblTipoFile" Runat="server" CssClass="Input_Label">Nome File Importato</asp:label>
											</td>
											<td>
												<asp:label id="LblNomeFile" Runat="server" CssClass="Input_Label"></asp:label>
											</td>
										</tr>
										<tr>
											<td colspan="2">&nbsp;</td>
										</tr>
										<tr>
											<td style="WIDTH: 162px">
												<asp:label id="LblTitoloEsito" Runat="server" CssClass="Input_Label">Esito Importazione </asp:label>
											</td>
											<td>
												<b>
													<asp:label id="LblEsito" Runat="server" CssClass="Input_Label"></asp:label>
												</b>
											</td>
										</tr>
										<tr>
											<td style="WIDTH: 162px">
												<asp:label id="LblScarto" Runat="server" CssClass="Input_Label">File Scarto </asp:label>
											</td>
											<td>
												<b>
													<asp:label id="LblLinkFileScarto" Runat="server" CssClass="Input_Label" Font-Underline="True"></asp:label>
												</b>
											</td>
										</tr>
										<tr>
											<td colspan="2">&nbsp;</td>
										</tr>
										<tr>
											<td style="WIDTH: 162px">
												<asp:label id="LblTRecDaImp" Runat="server" CssClass="Input_Label">Totale Record da Importare</asp:label>
											</td>
											<td>
												<asp:label id="LblTotRecDaImport" Runat="server" CssClass="Input_Label"></asp:label>
											</td>
										</tr>
										<tr>
											<td colspan="2">&nbsp;</td>
										</tr>
										<tr>
											<td style="WIDTH: 162px">
												<asp:label id="LblTRecImp" Runat="server" CssClass="Input_Label" Width="139px">Totale Record Importati</asp:label>
											</td>
											<td>
												<asp:label id="LblImportoTotF" Runat="server" CssClass="Input_Label"></asp:label>
											</td>
										</tr>
										<tr>
											<td style="WIDTH: 162px">
												<asp:label id="LblRecScart" Runat="server" CssClass="Input_Label">Totale Record Scartati</asp:label>
											</td>
											<td>
												<asp:label id="LblTotRecScartati" Runat="server" CssClass="Input_Label"></asp:label>
											</td>
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
				<asp:button id="CmdImport" style="DISPLAY: none" runat="server" Text="Importa"></asp:button>
				<asp:button id="CmdScarica" style="DISPLAY: none" runat="server"></asp:button>
				<asp:button id="CmdForzaLetture" style="DISPLAY: none" runat="server" Text="ForzaLetture"></asp:button>
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
