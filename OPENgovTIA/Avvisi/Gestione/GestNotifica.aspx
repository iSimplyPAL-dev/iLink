<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestNotifica.aspx.vb" Inherits="OPENgovTIA.GestNotifica" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
    <script lang="text/javascript">
        function keyPress() {
            if (window.event.keyCode == 13) {
                DivAttesa.style.display = '';
                document.getElementById('CmdSearch').click();
            }
        }
        function LoadAvvisi(IsFromVariabile, IdAvviso, IdRuolo, AzioneProv) {
            parent.Comandi.location.href = 'ComandiGestAvvisi.aspx';
            parent.Visualizza.location.href = 'GestAvvisi.aspx?IsFromVariabile=' + IsFromVariabile + '&IdUniqueAvviso=' + IdAvviso + '&IdRuolo=' + IdRuolo + '&AzioneProv=' + AzioneProv + '&Provenienza=7';
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table1">
                <tr valign="top">
                    <td align="left">
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" rowspan="2">
                        <input class="Bottone BottoneExcel" id="Print" title="Stampa" onclick="DivAttesa.style.display = ''; document.getElementById('CmdPrint').click()" type="button" name="Print" />
                        <input class="Bottone BottoneSalva" id="Save" title="Salva" onclick="DivAttesa.style.display = ''; document.getElementById('CmdSave').click();" type="button" name="Save" />
                        <input class="Bottone BottoneRicerca" id="Search" title="Ricerca" onclick="DivAttesa.style.display = ''; document.getElementById('CmdSearch').click();" type="button" name="Search" />
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 400px">- Avvisi - Gestione Notifica</span>
                    </td>
                </tr>
            </table>
        </div>
        &nbsp;
		<div id="TblRicerca" class="col-md-12">
            <div class="col-md-12">
                <fieldset class="classeFieldSetRicerca" runat="server">
                    <legend class="Legend" runat="server">Inserimento filtri di ricerca</legend>
                    <div class="col-md-5">
						<asp:Label id="Label1" CssClass="Input_Label" Runat="server">Cognome</asp:Label><br />
						<asp:textbox id="txtCognome" runat="server" CssClass="Input_Text col-md-10"></asp:textbox>
                    </div>
                    <div class="col-md-4">
						<asp:Label id="Label4" CssClass="Input_Label" Runat="server">Nome</asp:Label><br />
						<asp:textbox id="txtNome" runat="server" CssClass="Input_Text col-md-10"></asp:textbox>
                    </div>
                    <div class="col-md-2">
 						<asp:Label id="Label5" CssClass="Input_Label" Runat="server">Cod.Fiscale/P.IVA</asp:Label><br />
						<asp:textbox id="txtCFPIVA" runat="server" CssClass="Input_Text col-md-10" MaxLength="16"></asp:textbox>
                    </div>
					<div class="col-md-1">
						<asp:label id="Label2" Runat="server" CssClass="Input_Label">Anno</asp:label><br />
						<asp:dropdownlist id="ddlAnno" Runat="server" CssClass="Input_Text"></asp:dropdownlist>
					</div>
					<div class="col-md-2">
						<asp:label id="Label6" Runat="server" CssClass="Input_Label">Tipo Ruolo</asp:label><br />
						<asp:dropdownlist id="ddlTipoRuolo" Runat="server" CssClass="Input_Text"></asp:dropdownlist>
					</div>
                    <div class="col-md-2">
                        <label class="Input_Label">Data Emissione</label><br />
				        <asp:TextBox runat="server" ID="txtDataEmissione" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
 						<asp:Label CssClass="Input_Label" Runat="server">N.Avviso</asp:Label><br />
						<asp:textbox id="txtCodCartella" runat="server" CssClass="Input_Text col-md-10" MaxLength="18"></asp:textbox>
                    </div>
                </fieldset>
            </div>
            <div class="col-md-12">
                <asp:label id="LblResult" CssClass="Legend" Runat="server">Risultati della Ricerca</asp:label>
                <Grd:RibesGridView ID="GrdAvvisi" runat="server" BorderStyle="None"
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
						<asp:BoundField DataField="sCognome" SortExpression="sCognome" HeaderText="Cognome"></asp:BoundField>
						<asp:BoundField DataField="sNome" SortExpression="sNome" HeaderText="Nome"></asp:BoundField>
						<asp:BoundField DataField="sCodFiscale" SortExpression="sCfPiva" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
						<asp:BoundField DataField="sAnnoRiferimento" HeaderText="Anno"></asp:BoundField>
                        <asp:TemplateField HeaderText="Data Emissione">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataEmissione"))%>' ID="Label14"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
						<asp:BoundField DataField="sCodiceCartella" HeaderText="N.Avviso"></asp:BoundField>
						<asp:BoundField DataField="impCarico" HeaderText="Carico" DataFormatString="{0:N}">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
						</asp:BoundField>
                        <asp:TemplateField HeaderText="Data Notifica">
                            <HeaderTemplate>
                                <asp:Label runat="server">Data Notifica</asp:Label>
                                &nbsp;<asp:TextBox runat="server" ID="txtDataNotificaAll" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                                &nbsp;<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneRibaltaGrd" CommandName="RowCopy" alt="" ToolTip="Copia su tutti gli avvisi"></asp:ImageButton>
                                &nbsp;<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" alt="" ToolTip="Cancella da tutti gli avvisi"></asp:ImageButton>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:TextBox runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataNotifica"))%>' ID="txtDataNotifica" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
						<asp:TemplateField HeaderText="">
							<headerstyle horizontalalign="Center"></headerstyle>
							<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
							<itemtemplate>
								<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
					            <asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("id") %>' />
							</itemtemplate>
						</asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
            </div>
            <div id="DivAttesa" runat="server" style="z-index: 103; position: absolute;display:none;">
                <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
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
        <asp:Button Style="display: none" ID="CmdSearch" runat="server" />
        <asp:Button Style="display: none" ID="CmdSave" runat="server" />
        <asp:Button Style="display: none" ID="CmdPrint" runat="server" />
    </form>
</body>
</html>
