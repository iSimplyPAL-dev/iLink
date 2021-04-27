<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AcqPagamenti.aspx.vb" Inherits="OpenUtenze.AcqPagamenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>AcqPagamenti</title>
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
		<script language="javascript" type="text/javascript">
			function VisualizzaElaborazione(){
				DivAttesa.style.display='';
				DivAcq.style.display='none';
				DivEsitoAcq.style.display='';
				document.getElementById('fileUpload').disabled=true;
				parent.frames.item("comandi").document.getElementById("Import").disabled=true;
				window.setTimeout('caricaPagina()', 10000);
			}
			
			function caricaPagina(){
				document.location.href='AcqPagamenti.aspx';
			}
			
			function VisualizzaForm(){
				DivAttesa.style.display='none';
				DivAcq.style.display='';
				document.getElementById('fileUpload').disabled=false;
				parent.frames.item("comandi").document.getElementById("Import").disabled=false;
			}
			
		</script>
	</HEAD>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="FrmAcqPagamenti" runat="server">
			<table width="100%">
				<tr>
					<td width="100%" class="Input_Label">Si ricorda che per acquisire il file dei 
						pagamenti quest'ultimo deve:<br>
						- essere in formato txt;<br>
						- il nome del file deve avere dal carattere 22 per 8 caratteri <b>[conto corrente]</b>, 
						es. xxxxxxxxxxxxxxxxxxxx<b><%=session("conto_corrente")%></b>xxxxxx.txt;<br>
						in caso contrario l'operazione non sarà portata a termine.<br>
						<br>
					</td>
				</tr>
			</table>
			<div id="DivAcq">
				<table cellSpacing="0" cellPadding="0" width="100%" border="0">
					<tr>
						<td>
							<fieldset class="FiledSetRicerca">
								<legend class="Legend">
								</legend>
								<table cellSpacing="1" cellPadding="5" width="100%" border="0">
									<tr>
										<td>
											<asp:label id="LblPercorso" CssClass="Input_Label" Runat="server">Percorso</asp:label>
										</td>
										<td align="left">
											<input class="Input_Label" id="fileUpload" type="file" size="100" name="fileUpload" runat="server">
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
							<div id="DivEsitoAcq">
								<fieldset class="FiledSetRicerca">
									<legend class="Legend">
										Dati ultima Importazione</legend>
									<table cellSpacing="1" cellPadding="4" width="100%" border="0">
										<tr>
											<td style="WIDTH: 219px">
												<asp:label id="LblTipoFile" CssClass="Input_Label" Runat="server">Nome File Importato</asp:label>
											</td>
											<td>
												<asp:label id="LblNomeFile" CssClass="Input_Label" Runat="server"></asp:label>
											</td>
										</tr>
										<tr>
											<td colSpan="2">&nbsp;</td>
										</tr>
										<tr>
											<td style="WIDTH: 219px">
												<asp:label id="LblTitoloEsito" CssClass="Input_Label" Runat="server">Esito Importazione </asp:label>
											</td>
											<td>
												<b>
													<asp:label id="LblEsito" CssClass="Input_Label" Runat="server"></asp:label></b>
											</td>
										</tr>
										<tr>
											<td style="WIDTH: 219px">
												<asp:label id="Label3" CssClass="Input_Label" Runat="server">File Scarti </asp:label>
											</td>
											<td>
												<asp:label ID="LblFileScarti" Runat="server" CssClass="Input_Label" Font-Underline="True"></asp:label>
											</td>
										</tr>
										<tr>
											<td colSpan="2">&nbsp;</td>
										</tr>
										<tr>
											<td style="WIDTH: 219px">
												<asp:label id="Label1" CssClass="Input_Label" Runat="server">Totale Pagamenti da Acquisire </asp:label>
											</td>
											<td>
												<asp:label id="LblRcDaImp" CssClass="Input_Label" Runat="server"></asp:label>
											</td>
										</tr>
										<tr>
											<td style="WIDTH: 219px">
												<asp:label id="Label4" CssClass="Input_Label" Runat="server">Totale Importi da Acquisire </asp:label>
											</td>
											<td>
												<asp:label id="LblImportiDaImp" CssClass="Input_Label" Runat="server"></asp:label>
											</td>
										</tr>
										<tr>
											<td colSpan="2">&nbsp;</td>
										</tr>
										<tr>
											<td style="WIDTH: 219px">
												<asp:label id="Label5" CssClass="Input_Label" Runat="server">Totale Pagamenti Acquisiti </asp:label>
											</td>
											<td>
												<asp:label id="LblRcAcq" style="TEXT-ALIGN: right" CssClass="Input_Label" Runat="server"></asp:label>
											</td>
										</tr>
										<tr>
											<td style="WIDTH: 219px">
												<asp:label id="Label6" CssClass="Input_Label" Runat="server">Totale Importi Acquisiti </asp:label>
											</td>
											<td>
												<asp:label id="LblImportiAcq" style="TEXT-ALIGN: right" CssClass="Input_Label" Runat="server"></asp:label>
											</td>
										</tr>
										<tr>
											<td style="WIDTH: 219px">
												<asp:label id="Label7" CssClass="Input_Label" Runat="server">Totale Pagamenti Scartati </asp:label>
											</td>
											<td>
												<asp:label id="LblRcScarti" style="TEXT-ALIGN: right" CssClass="Input_Label" Runat="server"></asp:label>
											</td>
										</tr>
										<tr>
											<td style="WIDTH: 219px">
												<asp:label id="Label2" CssClass="Input_Label" Runat="server">Totale Importi Scartati </asp:label>
											</td>
											<td>
												<asp:label id="LblImportiScarti" style="TEXT-ALIGN: right" CssClass="Input_Label" Runat="server"></asp:label>
											</td>
										</tr>
									</table>
								</fieldset>
							</div>
						</td>
					</tr>
				</table>
			</div>
			<div id="DivAttesa" style="DISPLAY: none; Z-INDEX: 101; LEFT: 232px; TOP: 320px" align="center"
				runat="server">
				<table id="attesaCarica" borderColor="#ff0000" cellSpacing="0" cellPadding="0" width="50%"
					align="center" bgColor="white" border="1">
					<tr>
						<td align="center"><br>
							<p class="Legend">Elaborazione in Corso....</p>
							<p><IMG alt="" src="../../images/Clessidra.gif"></p>
							<p class="Legend">Attendere Prego...</p>
						</td>
					</tr>
				</table>
			</div>
			<asp:button id="CmdImporta" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="CmdScarica" style="DISPLAY: none" runat="server"></asp:button>
		</form>
	</body>
</HTML>
