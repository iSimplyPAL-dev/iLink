<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AgenziaEntrateTracciati.aspx.vb" Inherits="OPENgovTIA.AgenziaEntrateTracciati" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title></title>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">
			function PopolaAppoggio()
			{
			    if (document.getElementById('DdlAnno').value=='')
				{
					GestAlert('a', 'warning', '', '', 'Selezionare un\'anno!');
				Setfocus(document.getElementById('DdlAnno'));
					return false
				}
			if (document.getElementById('TxtDal').value=='')
				{
					GestAlert('a', 'warning', '', '', 'Inserire una data di partenza!');
				Setfocus(document.getElementById('TxtDal'));
					return false
				}
				
				if (confirm('Si vuole procedere al popolamento della tabella d\'appoggio per l\'anno e la data selezionati?'))
				{
					DivAttesa.style.display='';
					document.getElementById('CmdPopolaTabAppoggio').click();
				}
			}		
			function EstraiTracciato()
			{
				if (confirm('Si vuole procedere all\'estrazione del tracciato per l\'anno selezionato?'))
				{
					DivAttesa.style.display='';
					document.getElementById('CmdEstraiTracciatoAE').click();
				}
			}
			function StampaDatiMancanti() {
			    if (document.getElementById('DdlAnno').value == '') {
			        GestAlert('a', 'warning', '', '', 'Selezionare un\'anno!');
			    Setfocus(document.getElementById('DdlAnno'));
			        return false
			    }
			if (document.getElementById('TxtDal').value == '') {
			        GestAlert('a', 'warning', '', '', 'Inserire una data di partenza!');
			    Setfocus(document.getElementById('TxtDal'));
			        return false
			    }
			    if (confirm('Si vuole procedere alla stampa dei dati mancanti per l\'anno e la data selezionati?'))
			    {
			        DivAttesa.style.display = '';
			        document.getElementById('CmdStampa').click();
			    }
			}
		</script>
    </head>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
		    <div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: hidden">
			    <table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				    <tr>
					    <td style="WIDTH: 464px; HEIGHT: 20px" align="left">
						    <span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							    <asp:Label id="lblTitolo" runat="server"></asp:Label>
						    </span>
					    </td>
					    <td align="right" width="800" colSpan="2" rowSpan="2">
						    <input class="Bottone BottonePopolaAppoggio" id="btnEstraiTracciatoAE" title="Estrai Tracciato" onclick="EstraiTracciato()" type="button" name="btnEstraiTracciatoAE">
						    <input class="Bottone BottoneCreaFile" id="btnPopolaTabAppoggio" title="Popola Appoggio" onclick="PopolaAppoggio()" type="button" name="btnPopolaTabAppoggio"> 
						    <input class="Bottone BottoneExcel" id="Stampa" title="Stampa Dati Mancanti" onclick="StampaDatiMancanti()" type="button" name="Stampa">
					    </td>
				    </tr>
				    <tr>
					    <td style="WIDTH: 463px" align="left">
						    <span class="NormalBold_title" id="info" runat="server" style="WIDTH: 400px; HEIGHT: 20px">
							     - Agenzia Entrate - Estrazione Tracciato</span>
					    </td>
				    </tr>
			    </table>
		    </div>
		    &nbsp;
		    <div id="divSearch">
			<table id="tblTracciatiAE" cellSpacing="0" cellPadding="0" width="748" border="0">
				<tr>
					<td>
						<fieldset><asp:label id="LblInfo" Runat="server" CssClass="Input_Label">
								Per una corretta estrazione del tracciato procedere come segue:<br /><br />
								- effettuare il controllo sui dati mancanti (pulsante <img height="16px" src="../../images/Bottoni/excel_grd.png">);<br />
								- popolare la tabella d'appoggio per l'estrazione del tracciato (pulsante <img height="16px" src="../../images/Bottoni/nuovoinserisci2.png">);<br />
								- selezionare l'anno nella tabella e generare il file (pulsante <img height="16px" src="../../images/Bottoni/icon_export.png">).<br />
							</asp:label></fieldset>
					</td>
				</tr>
				<tr>
					<td><br />
						<fieldset><legend class="Legend">Inserimento Parametri di Estrazione</legend>
							<table id="TblParametri" width="100%" runat="server">
								<tr>
									<td>
										<asp:label id="Label1" runat="server" CssClass="Input_Label">Anno Ruolo</asp:label><br />
										<asp:dropdownlist id="DdlAnno" onkeydown="keyPress();" tabIndex="1" runat="server" CssClass="Input_Text"></asp:dropdownlist>
									</td>
									<td>
										<asp:Label Runat="server" CssClass="Input_Label" id="Label3">Dal</asp:Label><br />
										<asp:TextBox ID="TxtDal" Runat="server" TabIndex="2" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" maxlength="10"></asp:TextBox>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td><asp:label id="LblMessage" runat="server" CssClass="Legend"></asp:label></td>
				</tr>
				<tr>
					<td><br />
                        <Grd:RibesGridView ID="GrdTracciatiAE" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
								<asp:BoundField DataField="Anno" HeaderText="Anno">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="NomeFile" HeaderText="Nome File"></asp:BoundField>
								<asp:TemplateField HeaderText="N.Utenti">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="Label2" Runat="server" Text='<%# FncGrd.FormatNumberInGrd(DataBinder.Eval(Container, "DataItem.NumeroUtenti")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="N.Records">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="nrc" Runat="server" Text='<%# FncGrd.FormatNumberInGrd(DataBinder.Eval(Container, "DataItem.NumeroRecords")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="N.Articoli">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="Label4" Runat="server" Text='<%# FncGrd.FormatNumberInGrd(DataBinder.Eval(Container, "DataItem.NumeroArticoli")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Data Estrazione">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="Label5" Runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataEstrazione")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
										<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneDownloadGrd" CommandName="RowDownload" CommandArgument='<%# Eval("idflusso") %>' alt=""></asp:ImageButton>
					                    <asp:HiddenField runat="server" ID="hfCodiceISTAT" Value='<%# Eval("CodiceISTAT") %>' />
                                        <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("idflusso") %>' />
									</itemtemplate>
								</asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>
						<br />
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
			</table>
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
			<asp:button id="CmdStampa" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="CmdPopolaTabAppoggio" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="CmdEstraiTracciatoAE" style="DISPLAY: none" Runat="server"></asp:button>
		</form>
	</body>
</html>
