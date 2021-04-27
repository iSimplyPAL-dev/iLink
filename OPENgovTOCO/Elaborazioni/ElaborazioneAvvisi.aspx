<%@ Page language="c#" Codebehind="ElaborazioneAvvisi.aspx.cs" AutoEventWireup="True" Inherits="OPENgovTOCO.Elaborazioni.ElaborazioneAvvisi" EnableEventValidation="false" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Elaborazione avvisi</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" src="../../_js/Toco.js?newversion"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" class="Sfondo">
		<form id="Form1" runat="server" method="post" class="FormNoComandi">
			<div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: auto">
				<table style="WIDTH: 100%" cellSpacing="0" cellPadding="0" border="0">
					<tr vAlign="top">
						<td><span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px"><asp:label id="lblTitolo" runat="server"></asp:label></span></td>
						<td vAlign="middle" align="right" rowSpan="2">
							<input class="Bottone BottoneDownload" id="DownloadTemplate" title="Download Template" type="button" name="DownloadTemplate" onclick="document.getElementById('divElaborazioneInCorso').style.display = ''; document.getElementById('btnDownload').click();" />
                            <input class="Bottone BottoneImport" id="UploadTemplate" title="Upload Template" type="button" name="UploadTemplate" onclick="document.getElementById('divElaborazioneInCorso').style.display = ''; document.getElementById('btnUpload').click();"/>
						    <asp:imagebutton id="btnEliminaRuolo" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneCancella" ToolTip="Elimina Ruolo" onclick="btnEliminaRuolo_Click"></asp:imagebutton>
							<asp:imagebutton id="btnStampaDocumenti" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneWord" ToolTip="Stampa Documenti" onclick="btnStampaDocumenti_Click"></asp:imagebutton>
							<asp:imagebutton id="btnCalcolaRate" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneNumerazione" ToolTip="Calcola Rate" onclick="btnCalcolaRate_Click"></asp:imagebutton>
							<asp:imagebutton id="btnApprovaMinuta" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneFolderAccept" ToolTip="Approva Minuta" onclick="btnApprovaMinuta_Click"></asp:imagebutton>
							<asp:imagebutton id="btnStampaMinuta" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneExcel" ToolTip="Stampa Minuta" onclick="btnStampaMinuta_Click"></asp:imagebutton>
							<asp:imagebutton id="btnCalcola" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneCalcolo" ToolTip="Avvia calcolo avvisi" onclick="btnCalcola_Click"></asp:imagebutton>
                            <asp:imagebutton id="btnImport" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneImport" ToolTip="Acquisizione" onclick="btnImport_Click"></asp:imagebutton>
						</td>
					</tr>
					<tr>
						<td colSpan="2"><span id="info" runat="server" class="NormalBold_title" style="WIDTH: 400px; HEIGHT: 20px"> - Elaborazione - Avvisi</span>
						</td>
					</tr>
				</table>
			</div>
			<div style="WIDTH: 100%">
				<table id="TblRicerca" cellSpacing="1" cellPadding="1" width="100%" border="0">
					<tr>
						<td>
							<fieldset class="classeFieldSetRicerca"><legend class="Legend">Parametri di calcolo</legend>
								<table id="TblParametri" cellSpacing="1" cellPadding="1" width="100%" border="0">
									<tr>
										<td class="Input_Label"><asp:label id="lblAnno" CssClass="Input_Label" Runat="server">Anno</asp:label><br>
											<asp:dropdownlist id="ddlAnno" runat="server" CssClass="Input_Text" AutoPostBack="True" onselectedindexchanged="ddlAnno_SelectedIndexChanged"></asp:dropdownlist></td>
										<td class="Input_Label"><asp:label id="Label10" CssClass="Input_Label" Runat="server">Importo minimo per rateizzazione (euro)</asp:label><br>
											<asp:textbox id="txtSogliaMinima" runat="server" CssClass="Input_Text_right" onblur="if (isNumber(this,'',2,'',999999)) this.value = number_format(this.value, 2, ',', '');">0</asp:textbox></td>
										<td>
											<asp:RadioButton ID="OptOrdinario" Runat="server" GroupName="TipoRuolo" Text="Ordinario" CssClass="Input_Label" AutoPostBack="True" oncheckedchanged="ddlAnno_SelectedIndexChanged"></asp:RadioButton>
										</td>
										<td>
											<asp:RadioButton ID="OptSuppletivo" Runat="server" GroupName="TipoRuolo" Text="Suppletivo" CssClass="Input_Label" AutoPostBack="True"></asp:RadioButton>
										</td>
                                        <td>
                                            <!--*** 201511 - template documenti per ruolo ***-->
                                            <asp:Label ID="lblUploadFile" runat="server" Text="File" CssClass="Input_Label"></asp:Label>&nbsp;
                                            <asp:FileUpload ID="fileUploadDOT" runat="server" CssClass="Input_Text" Width="400px" />
                                            <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="fileUploadDOT" ErrorMessage="Seleziona il file da importare" ValidationGroup="UploadValidation"></asp:RequiredFieldValidator>
                                            <br />
                                            <asp:Label ID="lblMessage" runat="server" Text="" Visible="false" CssClass="ERRORSTYLE"></asp:Label>
                                        </td>
									</tr>
								</table>
							</fieldset>
						</td>
					</tr>
				</table>
				<br /><br />
				<table id="tblStatistiche" cellSpacing="1" cellPadding="1" width="100%" border="0" runat="server">
                    <tr id="trImport">
						<td>
							<fieldset class="FiledSetRicerca">
								<legend class="Legend">Flusso da Importare</legend>
								<table  cellSpacing="1" cellPadding="1" width="100%" border="0">
									<tr>
										<td>
											<asp:label id="LblPercorso" CssClass="Input_Label" Runat="server">Percorso</asp:label>
										</td>
										<td align="left">
											<input class="Input_Label" id="fileUpload" type="file" size="100" name="fileUpload" runat="server" />
										</td>
									</tr>
								</table>
							</fieldset><br /><br />
						</td>
					</tr>
                    <tr id="trOccupToElab">
                        <td>
                            <fieldset class="FiledSetRicerca">
                                <legend class="Legend">Occupazioni da Elaborare</legend>
                                <Grd:RibesGridView ID="GrdOccupaz" runat="server" BorderStyle="None" 
                                      BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                      AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                      ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                      <PagerSettings Position="Bottom"></PagerSettings>
                                      <PagerStyle CssClass="CartListFooter" />
                                      <RowStyle CssClass="CartListItem"></RowStyle>
                                      <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                      <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
									<Columns>
										<asp:BoundField DataField="" HeaderText="IdTipologiaOccupazione" Visible="False"></asp:BoundField>
										<asp:BoundField DataField="descrizione" HeaderText="Occupazione">
											<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
										</asp:BoundField>
										<asp:TemplateField HeaderText="Sel.">
											<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" runat="server" Text="Tutte" ToolTip="Tutte" TextAlign="Left" Checked="false" AutoPostBack="true" OnCheckedChanged="SelAllRow" />
                                            </HeaderTemplate>
											<ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
											<ItemTemplate>
												<asp:CheckBox id="ChkSelezionato" runat="server" AutoPostBack="true" Checked='false' OnCheckedChanged="ChkSelezionato_CheckedChanged"></asp:CheckBox>
                                                <asp:HiddenField runat="server" ID="hfIdTipologiaOccupazione" Value='<%# Eval("IdTipologiaOccupazione") %>' />
											</ItemTemplate>
										</asp:TemplateField>
									</Columns>
									</Grd:RibesGridView>                            
                            </fieldset>
                        </td>
                    </tr>
					<tr>
						<td>
							<fieldset class="classeFieldSetRicerca">
                                <legend class="Legend">Riepilogo Dati da inserire a Ruolo</legend><br />
								<table id="TblStatistichePre" cellSpacing="1" cellPadding="1" width="100%" border="0">
									<tr>
										<td class="Input_Label" width="34%">
											<asp:label id="Label1" CssClass="Input_Label" Runat="server">N. Utenti</asp:label>
											&nbsp;&nbsp; <asp:label id="lblPreNUtenti" CssClass="Input_Label_bold" Runat="server"></asp:label>
										</td>
										<td class="Input_Label" width="33%">
											<asp:label id="Label2" CssClass="Input_Label" Runat="server">N. Dichiarazioni</asp:label>
											&nbsp;&nbsp; <asp:label id="lblPreNDichiarazioni" CssClass="Input_Label_bold" Runat="server"></asp:label>
										</td>
										<td class="Input_Label" width="33%">
											<asp:label id="Label4" CssClass="Input_Label" Runat="server">N. Articoli</asp:label>
											&nbsp;&nbsp; <asp:label id="lblPreNArticoli" CssClass="Input_Label_bold" Runat="server"></asp:label>
										</td>
									</tr>
								</table>
                                <br />
							</fieldset>
						</td>
					</tr>
					<tr>
						<td><br />
							<fieldset class="classeFieldSetRicerca">
                                <legend class="Legend">Riepilogo Dati Ruolo</legend><br />
								<table id="TblStatistichePost" cellSpacing="1" cellPadding="1" width="100%" border="0">
									<tr>
										<td class="Input_Label" width="34%">
											<asp:label id="Label5" CssClass="Input_Label" Runat="server">N. Utenti</asp:label>
											&nbsp;&nbsp; <asp:label id="lblPostNUtenti" CssClass="Input_Label_bold" Runat="server"></asp:label>
										</td>
										<td class="Input_Label" width="33%">
											<asp:label id="Label7" CssClass="Input_Label" Runat="server">N. Dichiarazioni</asp:label>
											&nbsp;&nbsp; <asp:label id="lblPostNDichiarazioni" CssClass="Input_Label_bold" Runat="server"></asp:label>
										</td>
										<td class="Input_Label" width="33%">
											<asp:label id="Label9" CssClass="Input_Label" Runat="server">N. Articoli</asp:label>
											&nbsp;&nbsp; <asp:label id="lblPostNArticoli" CssClass="Input_Label_bold" Runat="server"></asp:label>
										</td>
									</tr>
									<tr>
										<td class="Input_Label">
											<asp:label id="Label6" CssClass="Input_Label" Runat="server">Importo totale</asp:label>
											&nbsp;&nbsp; <asp:label id="lblPostImportoTotale" CssClass="Input_Label_bold" Runat="server"></asp:label>
										</td>
									</tr>
									<tr>
										<td class="Input_Label" colspan="3"><asp:label id="Label8" CssClass="Input_Label" Runat="server">Note</asp:label>
											&nbsp;&nbsp; <asp:label id="lblPostNote" CssClass="Input_Label_bold" Runat="server"></asp:label>
										</td>
									</tr>
								</table>
                                <br />
							</fieldset>
						</td>
					</tr>
				</table>
                <div id="divElaborazioneInCorso" runat="server" style="z-index: 101; position: absolute;display:none;">
                    <div class="Legend" style="margin-top:40px;">Elaborazione ruoli anno <asp:Label Runat="server" ID="lblElaborazioneAnno"></asp:Label> in Corso...</div>
                    <div class="BottoneClessidra">&nbsp;</div>
                    <div class="Legend">Attendere Prego...<asp:Label ID="LblAvanzamento" runat="server"></asp:Label></div>
                </div>
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
            <asp:Button ID="btnDownload" style="display:none" runat="server" OnClick="BtnDownloadClick" />
            <asp:Button ID="btnUpload" style="display:none" runat="server" OnClick="BtnUploadClick" ValidationGroup="UploadValidation"/>
            <asp:HiddenField ID="hfOccupazioniToElab" runat="server" />
            <asp:HiddenField ID="hfIdElab" runat="server" Value="-1" />
		</form>
	</body>
</HTML>
