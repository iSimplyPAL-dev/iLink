<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GenMassiva.aspx.vb" Inherits="Provvedimenti.GenMassiva" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Calcolo</title>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
        function CheckDati() {
            var ListTrib = '';
            if (document.getElementById('chk8852').checked) {
                ListTrib += '-8852';
            }
            if (document.getElementById('chkTASI').checked) {
                ListTrib += '-TASI';
            }
            if (document.getElementById('chk0434').checked) {
                ListTrib += '-0434';
            }
            if (document.getElementById('chk0453').checked) {
                ListTrib += '-0453';
            }
            if (ListTrib == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Tributo!');
                return false;
            }
            if ($('#ddlAnno').val() == '-1') {
                GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Anno!');
                return false;
            }
            if ($('#txtSoglia').val() == '') {
                GestAlert('a', 'warning', '', '', 'Inserire una soglia minima!');
                return false;
            }
            if ($('#txtGGScad').val() == '') {
                GestAlert('a', 'warning', '', '', 'Inserire i giorni di scadenza!');
                return false;
            }
            return true;
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table1">
                <tr valign="top">
                    <td align="left">
                        <span id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server" class="ContentHead_Title"></asp:Label><br />
                        </span>
                    </td>
                    <td align="right" rowspan="2">
                        <asp:Button runat="server" ID="CmdSearch" Text="" CssClass="Bottone BottoneRicerca" OnClientClick="DivAttesa.style.display = '';" OnClick="CmdSearch_Click" />
                        <asp:Button runat="server" ID="CmdOKMinuta" Text="" CssClass="Bottone BottoneFolderAccept" OnClientClick="DivAttesa.style.display = '';" OnClick="CmdOKMinuta_Click" />
                        <asp:Button runat="server" ID="CmdPrintMinuta" Text="" CssClass="Bottone BottoneExcel" OnClientClick="DivAttesa.style.display = '';" OnClick="CmdPrintMinuta_Click" />
                        <asp:Button runat="server" ID="CmdCalcola" Text="" CssClass="Bottone BottoneCalcolo" OnClientClick="DivAttesa.style.display = '';return CheckDati();" OnClick="CmdCalcola_Click" />
                   </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 400px">- Coattivo</span>
                    </td>
                </tr>
            </table>
        </div>
        &nbsp;
		<div id="TblRicerca" class="col-md-12">
            <div class="col-md-12">
                <fieldset class="classeFieldSetRicerca" runat="server">
                    <legend class="Legend" runat="server">Parametri Elaborazione</legend>
                    <div class="col-md-12">
                        <div class="col-md-4">
                            <label class="Input_Label">Tributo</label>
                            &nbsp;<asp:CheckBox ID="chk8852" runat="server" CssClass="Input_Label" Text="IMU" />
                            &nbsp;<asp:CheckBox ID="chkTASI" runat="server" CssClass="Input_Label" Text="TASI" />
                            &nbsp;<asp:CheckBox ID="chk0434" runat="server" CssClass="Input_Label" Text="TARES/TARI" />
                            &nbsp;<asp:CheckBox ID="chk0453" runat="server" CssClass="Input_Label" Text="TOSAP/COSAP" />
                        </div>
                        <div class="col-md-2">
                            <label class="Input_Label">Anno</label>
                            &nbsp;<asp:DropDownList ID="ddlAnno" runat="server" CssClass="Input_Text TextDate"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <label class="Input_Label">Soglia Insoluto </label>
				            &nbsp;<asp:TextBox runat="server" ID="txtSoglia" CssClass="Input_Text_Right TextDate" onblur="if (!isNumber(this.value, 2, 0)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}">0</asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="Input_Label">Scaduto da </label>
				            &nbsp;<asp:TextBox runat="server" ID="txtGGScad" CssClass="Input_Text_Right TextDate" onblur="if (!isNumber(this.value, 0, 0)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI!')}">90</asp:TextBox>
                            &nbsp;<label class="Input_Label">GG</label>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div class="col-md-12">
                <asp:label id="LblResult" CssClass="Legend" Runat="server">Risultati della Ricerca</asp:label>
                <Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None"
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="20"
                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                    OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <PagerStyle CssClass="CartListFooter" />
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
						<asp:BoundField DataField="Cognome" SortExpression="Cognome" HeaderText="Cognome"></asp:BoundField>
						<asp:BoundField DataField="Nome" SortExpression="Nome" HeaderText="Nome"></asp:BoundField>
						<asp:BoundField DataField="CFPIVA" SortExpression="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
						<asp:BoundField DataField="DescrTributo" HeaderText="Tributo"></asp:BoundField>
                        <asp:BoundField DataField="Anno" HeaderText="Anno"></asp:BoundField>
						<asp:BoundField DataField="Motivazione" HeaderText="Motivazione"></asp:BoundField>
                        <asp:TemplateField HeaderText="Data Creazione">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataCreazione"))%>' ID="Label10">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="impDifImposta" HeaderText="Diff.Imposta" DataFormatString="{0:N}">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
						</asp:BoundField>
                        <asp:BoundField DataField="impSanzioni" HeaderText="Sanzioni" DataFormatString="{0:N}">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
						</asp:BoundField>
                        <asp:BoundField DataField="impInteressi" HeaderText="Interessi" DataFormatString="{0:N}">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
						</asp:BoundField>
                        <asp:BoundField DataField="impAltro" HeaderText="Altro" DataFormatString="{0:N}">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
						</asp:BoundField>
                        <asp:BoundField DataField="impSpese" HeaderText="Spese" DataFormatString="{0:N}">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
						</asp:BoundField>
                        <asp:BoundField DataField="impTotale" HeaderText="Totale" DataFormatString="{0:N}">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
						</asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <HeaderTemplate>
                                <asp:Label runat="server">Sel</asp:Label>
                                &nbsp;<asp:CheckBox runat="server" ID="chkSelAll" CssClass="Input_Text" Text="Tutti"></asp:CheckBox>
                                &nbsp;<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneRibaltaGrd" CommandName="RowCopy" alt="" ToolTip="Copia su tutti gli avvisi"></asp:ImageButton>
                                &nbsp;<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" alt="" ToolTip="Cancella da tutti gli avvisi"></asp:ImageButton>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								&nbsp;<asp:CheckBox runat="server" ID="chkSel" CssClass="Input_Text" Text=""></asp:CheckBox>
                                &nbsp;<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
					            <asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("id") %>' />
							</ItemTemplate>
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
    </form>
</body>
</html>
