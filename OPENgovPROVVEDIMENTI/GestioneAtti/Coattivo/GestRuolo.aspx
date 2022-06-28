<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestRuolo.aspx.vb" Inherits="Provvedimenti.GestRuolo" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Calcolo</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
        function CheckRuolo()
        {
            var ListTrib = '';
            if (document.getElementById('chk8852').checked)
            {
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
            if (ListTrib == '')
            {
                GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Tributo!');
                return false;
            }
            if ($('#txtDalNotifica').val() == '' || $('#txtAlNotifica').val() == '') {
                GestAlert('a', 'warning', '', '', 'Inserire un periodo di notifica!');
                Setfocus(document.getElementById('TxtDalNotifica'));
                return false;
            }
            else {
                var starttime = $('#txtDalNotifica').val();
                var endtime = $('#txtAlNotifica').val();
                //Start date split to UK date format and add 31 days for maximum datediff
                starttime = starttime.split("/");
                starttime = new Date(starttime[2], starttime[1] - 1, starttime[0]);
                //End date split to UK date format 
                endtime = endtime.split("/");
                endtime = new Date(endtime[2], endtime[1] - 1, endtime[0]);
                if (endtime <= starttime) {
                    GestAlert('a', 'warning', '', '', 'La Data di Fine e\' minore/uguale alla Data di Inizio!');
                    Setfocus(document.getElementById('TxtAlNotifica'));
                    return false;
                }
            }
            DivAttesa.style.display = '';
            $('#CmdCalcola').click();//FrmGestRuolo.CmdCalcola.click()
        }
    </script>
</head>
<body class="Sfondo" leftmargin="0" topmargin="0" ms_positioning="GridLayout">
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
                        <input class="Bottone BottoneHistory" id="Precedenti" title="Ruoli Precedenti" onclick="DivAttesa.style.display = ''; document.getElementById('CmdOld').click();" type="button" name="Precedenti">
                        <input class="Bottone BottoneCreaFile" id="CreaFile" title="Crea 290" onclick="DivAttesa.style.display = ''; document.getElementById('CmdEstrai').click();" type="button" name="CreaFile">
                        <input class="Bottone BottoneFolderAccept" id="Approva" title="Approva" onclick="DivAttesa.style.display = '';document.getElementById('CmdApprova').click();" type="button" name="Approva">
                        <input class="Bottone BottoneExcel" id="Stampa" title="Stampa" onclick="DivAttesa.style.display = ''; document.getElementById('CmdStampa').click();" type="button" name="Stampa">
                        <input class="Bottone BottoneCalcolo" id="ElaboraRuolo" title="Calcola" onclick="CheckRuolo();" type="button" name="ElaboraRuolo">
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
        <div id="divDatiDelibera" class="col-md-12">
            <div class="col-md-2">
                <p>
                    <label class="Input_Label">Numero Delibera</label>
                </p>
                <asp:TextBox ID="TxtNDelibera" runat="server" CssClass="Input_Text col-md-12"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <p>
                    <label class="Input_Label">Dal-Al</label>
                </p>
                <asp:TextBox runat="server" ID="txtInizioDelibera" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtFineDelibera" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
            </div>
            <div class="col-md-12">
                <p>
                    <label class="Input_Label">Estremi della delibera con cui l'ente affida la riscossione ai sensi dell'art.2,comma2,D.Lgs n.193 del 2016. La Data Inizio deve essere maggiore del 01/10/2016</label>
                </p>
            </div>
        </div>
        &nbsp;
		<div id="TblRicerca" class="col-md-12">
            <div runat="server" id="DivRiepilogoDaElab" class="col-md-12">
                <fieldset class="classeFieldSetRicerca col-md-12">
                    <div class="col-md-12">
                        <div class="col-md-1">
                            <label class="Input_Label">Anno</label><br />
                            <asp:DropDownList ID="ddlAnno" runat="server" CssClass="Input_Text_Right TextDate"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:CheckBox ID="chk8852" runat="server" CssClass="Input_CheckBox_NoBorder" Text="IMU" />
                            &nbsp;<asp:CheckBox ID="chkTASI" runat="server" CssClass="Input_CheckBox_NoBorder" Text="TASI" />
                            <br /><asp:CheckBox ID="chk0434" runat="server" CssClass="Input_CheckBox_NoBorder" Text="TARES/TARI" />
                            &nbsp;<asp:CheckBox ID="chk0453" runat="server" CssClass="Input_CheckBox_NoBorder" Text="TOSAP/COSAP" />
                        </div>
                            <div class="col-md-3">
                            <label class="Input_Label">Notificati</label><br />
                            <label class="Input_Label">Dal</label>&nbsp;<asp:TextBox runat="server" ID="txtDalNotifica" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                            <label class="Input_Label">Al</label>&nbsp;<asp:TextBox runat="server" ID="txtAlNotifica" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="Input_Label">Provenienza</label><br />
                            <asp:DropDownList ID="ddlProvenienza" runat="server" CssClass="Input_Text"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:CheckBox ID="chkInteressi" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Applica Interessi"></asp:CheckBox><br />
                            <asp:CheckBox ID="chkSpese" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Applica Spese di Notifica"></asp:CheckBox>
                        </div>
                     </div>
                    <br /><br /><br />
                    <div class="col-md-12">
                        <asp:Label ID="lblMessage" runat="server" Text="" Visible="false" CssClass="ERRORSTYLE"></asp:Label>
                    </div>
                </fieldset>
            </div>
            <div class="col-md-12">
                <Grd:RibesGridView ID="GrdDateElaborazione" runat="server" BorderStyle="None"
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                    OnRowCommand="GrdRowCommand">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <PagerStyle CssClass="CartListFooter" />
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Data Calcolo">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataCreazione"))%>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="sTipoCalcolo" HeaderText="Anno"></asp:BoundField>
                        <asp:BoundField DataField="sNomeRuolo" HeaderText="Tributi"></asp:BoundField>
                        <asp:BoundField DataField="sTipoRuolo" HeaderText="Provenienza"></asp:BoundField>
                        <asp:TemplateField HeaderText="Notifica Dal">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataInizioConf"))%>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Notifica Al">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataFineConf"))%>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="nAvvisi" HeaderText="N.Atti">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ImpAvvisi" HeaderText="Tot. " DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" CssClass="BottoneGrd BottoneApriGrd" CommandName="RowOK" CommandArgument='<%# Eval("IdFlusso") %>' alt="" ToolTip="Visualizza posizioni"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
            </div>
            <div class="col-md-12">
                <fieldset runat="server" id="FileEstratto" class="classeFieldSetRicerca col-md-12">
                    <label runat="server" class="Legend">Cliccare sul link a fianco per scaricare il 290 estratto</label>&nbsp;
                    <asp:label ID="LblFile290" Runat="server" CssClass="Input_Label hidden" Font-Underline="True"></asp:label><br />
                </fieldset>
                <Grd:RibesGridView ID="GrdPosizioni" runat="server" BorderStyle="None"
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="15"
                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                    OnPageIndexChanging="GrdPageIndexChanging" OnRowCommand="GrdRowCommand">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <PagerStyle CssClass="CartListFooter" />
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
						<asp:TemplateField HeaderText="Nominativo">
							<ItemStyle HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Cognome") + " " + DataBinder.Eval(Container, "DataItem.Nome") %>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Cod.Fiscale/P.IVA">
							<ItemStyle Width="120px" HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# FncGrd.FormattaCFPIVA(DataBinder.Eval(Container, "DataItem.CODICE_FISCALE"), DataBinder.Eval(Container, "DataItem.PARTITA_IVA")) %>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
                        <asp:BoundField DataField="NUMERO_ATTO" HeaderText="N.Atto"></asp:BoundField>
                        <asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Descrizione"></asp:BoundField>
                        <asp:TemplateField HeaderText="Data Emissione Coattivo">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataInserimento"))%>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DATA_NOTIFICA_AVVISO" HeaderText="Data Notifica"><ItemStyle HorizontalAlign="Right"></ItemStyle></asp:BoundField>
                        <asp:BoundField DataField="ImportoCoattivo" HeaderText="Imposta" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="IMPORTO_SANZIONI" HeaderText="Sanzioni" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="InteressiCoattivo" HeaderText="Interessi" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="SpeseCoattivo" HeaderText="Spese" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="TotaleCoattivo" HeaderText="Totale" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" CssClass="BottoneGrd BottoneCancellaGrd" CommandName="RowDel" CommandArgument='<%# Eval("Id") %>' alt="" ToolTip="Elimina"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
            </div>
            <div class="col-md-12">
                <Grd:RibesGridView ID="GrdRuoliPrec" runat="server" BorderStyle="None"
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                    OnRowCommand="GrdRowCommand">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <PagerStyle CssClass="CartListFooter" />
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Data Calcolo">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataCreazione"))%>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="sTipoCalcolo" HeaderText="Anno"></asp:BoundField>
                        <asp:BoundField DataField="sNomeRuolo" HeaderText="Tributi"></asp:BoundField>
                        <asp:BoundField DataField="sTipoRuolo" HeaderText="Provenienza"></asp:BoundField>
                        <asp:TemplateField HeaderText="Notifica Dal">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataInizioConf"))%>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Notifica Al">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataFineConf"))%>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="nAvvisi" HeaderText="N.Atti">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ImpAvvisi" HeaderText="Tot. " DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Data Estrazione">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataElabDoc"))%>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" CssClass="BottoneGrd BottoneApriGrd" CommandName="RowOK" CommandArgument='<%# Eval("IdFlusso") %>' alt="" ToolTip="Visualizza posizioni"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" CssClass="BottoneGrd BottonePrintGrd" CommandName="RowPrint" CommandArgument='<%# Eval("IdFlusso") %>' alt="" ToolTip="Stampa Minuta"></asp:ImageButton>
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
        <asp:Button CssClass="hidden" ID="CmdCalcola" runat="server" />
        <asp:Button CssClass="hidden" ID="CmdApprova" runat="server" />
        <asp:Button CssClass="hidden" ID="CmdEstrai" runat="server" />
        <asp:Button CssClass="hidden" id="CmdScarica" runat="server" />
        <asp:Button CssClass="hidden" id="CmdStampa" runat="server" />
        <asp:Button CssClass="hidden" id="CmdOld" runat="server" />
        <asp:HiddenField ID="hfIdRuolo" runat="server" Value="-1" />
    </form>
</body>
</html>
