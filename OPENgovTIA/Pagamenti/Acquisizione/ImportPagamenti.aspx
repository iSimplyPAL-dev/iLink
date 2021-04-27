<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ImportPagamenti.aspx.vb" Inherits="OPENgovTIA.ImportPagamenti" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ImportPagamenti</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%        End If%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" type="text/javascript">
			function VisualizzaElaborazione(){
				DivAttesa.style.display='';
				DivAcq.style.display='none';
				DivEsitoAcq.style.display='';
				document.getElementById('fileUpload').disabled=true;
				parent.Comandi.document.getElementById("Import").disabled=true;
				window.setTimeout('caricaPagina()', 10000);
			}
			
			function caricaPagina(){
				document.location.href='ImportPagamenti.aspx';
			}
			
			function VisualizzaForm(){
				DivAttesa.style.display='none';
				DivAcq.style.display='';
				document.getElementById('fileUpload').disabled=false;
				parent.Comandi.document.getElementById("Import").disabled=false;
			}
		</script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table width="100%">
				<tr id="DescrF24">
					<td width="100%" class="Input_Label">Si ricorda che per acquisire il file dei pagamenti in formato F24 quest'ultimo deve:<br />
						- avere il nome del file che inizia con la dicitura <b>F24</b><br />
						in caso contrario l'operazione non sarà portata a termine.<br />
						<br />
					</td>
				</tr>
				<tr id="DescrPOSTECC">
					<td width="100%" class="Input_Label">Si ricorda che per acquisire il file dei pagamenti scaricato da POSTE quest'ultimo deve:<br />
						- essere in formato txt;<br />
						- il nome del file deve avere dal carattere 22 per 15 caratteri <b>[conto corrente]</b>, 
						es. xxxxxxxxxxxxxxxxxxxxx<b>CCCCCCCCCCCCCCC</b>xxxxxxxxxxxxxxxxxxxxxxxxxxxxx.txt;<br />
						in caso contrario l'operazione non sarà portata a termine.<br />
						<br />
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
										<tr style="display:none">
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
										<tr style="display:none">
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
										<tr style="display:none">
											<td style="WIDTH: 219px">
												<asp:label id="Label7" CssClass="Input_Label" Runat="server">Totale Pagamenti Non Abbinati </asp:label>
											</td>
											<td>
												<asp:label id="LblRcScarti" style="TEXT-ALIGN: right" CssClass="Input_Label" Runat="server"></asp:label>
											</td>
										</tr>
										<tr>
											<td style="WIDTH: 219px">
												<asp:label id="Label2" CssClass="Input_Label" Runat="server">Totale Importi Non Abbinati </asp:label>
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
         <div id="divHistory" class="col-md-10" style="overflow: auto">
            <Grd:RibesGridView ID="GrdHistory" runat="server" BorderStyle="None" 
				BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
				AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
				ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
				<PagerSettings Position="Bottom"></PagerSettings>
				<PagerStyle CssClass="CartListFooter" />
				<RowStyle CssClass="CartListItem"></RowStyle>
				<HeaderStyle CssClass="CartListHead"></HeaderStyle>
				<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                <Columns>
                   <asp:BoundField DataField="sFileAcq" HeaderText="Flusso"></asp:BoundField>
                    <asp:TemplateField HeaderText="Data Importazione">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataAcq"))%>' ID="Label11"></asp:Label>
                            <asp:HiddenField runat="server" ID="hfIdFlusso" Value='<%# Eval("ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="sEsito" HeaderText="Esito"></asp:BoundField>
                    <asp:BoundField DataField="nRcDaAcquisire" HeaderText="Rc. Da Acq." DataFormatString="{0:N}"></asp:BoundField>
                    <asp:BoundField DataField="nRcAcquisiti" HeaderText="Rc. Acq." DataFormatString="{0:N}"></asp:BoundField>
                    <asp:BoundField DataField="nRcScarti" HeaderText="Rc. Scart." DataFormatString="{0:N}"></asp:BoundField>
                    <asp:BoundField DataField="impDaAcquisire" HeaderText="Imp. Da Acq." DataFormatString="{0:N}"></asp:BoundField>
                    <asp:BoundField DataField="impAcquisiti" HeaderText="Imp. Acq." DataFormatString="{0:N}"></asp:BoundField>
                    <asp:BoundField DataField="impScarti" HeaderText="Imp. Scart." DataFormatString="{0:N}"></asp:BoundField>
                </Columns>
            </Grd:RibesGridView>
            <div id="chart_div" class="col-md-6" style="height:400px;"></div>
        </div>
           <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                <div class="BottoneClessidra">&nbsp;</div>
                <div class="Legend">Attendere Prego...<asp:Label ID="LblAvanzamento" runat="server"></asp:Label></div>
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
			<asp:button id="CmdImporta" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="CmdScarica" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="CmdOld" style="DISPLAY: none" runat="server"></asp:button>
		</form>
	</body>
</HTML>
