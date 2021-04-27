<%@ Page Language="c#" CodeBehind="SituazioneAvvisi.aspx.cs" AutoEventWireup="True" Inherits="OPENgovTOCO.SituazioneContribuente.SituazioneAvvisi" EnableEventValidation="false" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<%@ Register Src="../Wuc/WucDatiContribuente.ascx" TagName="wucDatiContribuente" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>SituazioneAvvisi</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
</head>
<body style="overflow: auto" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout" class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <!-- Titolo -->
        <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: auto">
            <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" rowspan="2">
						<asp:ImageButton id="btnSalva" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneSalva" ToolTip="Salva" onclick="btnSalva_Click" />
                        <asp:imagebutton id="btnDelSgravio" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png" CssClass="Bottone BottoneCancella" ToolTip="Elimina Sgravio" onclick="btnDeleteSgravio_Click" />
						<asp:imagebutton id="btnSgravio" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneAttivaSgravi" ToolTip="Sgravio" onclick="btnSgravio_Click" />
						<asp:imagebutton id="btnStampaDocumenti" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneWord" ToolTip="Stampa Documenti" OnClientClick="document.getElementById('DivAttesa').style.display = '';document.getElementById('divStampa').style.display = '';document.getElementById('divVisual').style.display = 'none';" onclick="btnStampaDocumenti_Click" />
						<asp:ImageButton ID="btnClose" ToolTip="Torna alla ricerca" runat="server" CssClass="Bottone BottoneAnnulla" ImageUrl="../../images/Bottoni/transparent28x28.png" OnClick="btnClose_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span id="info" runat="server" class="NormalBold_title" style="width: 600px; height: 20px">TOSAP/COSAP - Situazione Contribuente - Situazione Avvisi</span>
                    </td>
                </tr>
            </table>
        </div>
        <!--blocco dati contribuente-->
        <%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
        <table width="100%">
            <tr id="TRPlainAnag">
                <td>
                    <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height: 200px; width: 100%" frameborder="0" marginheight="0"></iframe>
                    <asp:HiddenField ID="hdIdContribuente" runat="server" Value="-1" />
                </td>
            </tr>
            <tr id="TRSpecAnag">
                <td>
                    <uc1:wucDatiContribuente ID="wucDatiContribuente" runat="server"></uc1:wucDatiContribuente>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <!-- Dati riassuntivi e pagamenti -->
        <div id="divVisual">
            <table width="815">
                <tr>
                    <!-- Riassunto -->
                    <td width="50%">
                        <table width="90%" class="TableWithBorder">
                            <tr>
                                <td>
                                    <asp:Label CssClass="Input_Label_bold" runat="server" ID="Label1">TOTALE AVVISO ORIGINALE :</asp:Label></td>
                                <td align="right">
                                    <asp:Label CssClass="Input_Label_bold" ID="lblTotaleDaPagare" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="Input_Label_bold" runat="server" ID="Label2">TOTALE PAGATO :</asp:Label></td>
                                <td align="right">
                                    <asp:Label CssClass="Input_Label_bold" ID="lblTotalePagato" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="Input_Label_bold" runat="server" ID="Label3">RESIDUO :</asp:Label></td>
                                <td align="right">
                                    <asp:Label CssClass="Legend" ID="lblTotaleResiduo" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="Input_Label_bold" runat="server" ID="Label4">DATA SOLLECITO :</asp:Label></td>
                                <td>
                                    <asp:Label CssClass="Input_Label_bold" ID="lblTotaleDataSollecito" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="Input_Label_bold" runat="server" ID="Label5">DATA COATTIVO :</asp:Label></td>
                                <td>
                                    <asp:Label CssClass="Input_Label_bold" ID="lblTotaleDataCoattivo" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                    <!-- Pagamenti -->
                    <td width="50%" valign="top">
                        <Grd:RibesGridView ID="GrdDettaglioPagamenti" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField HeaderText="Rata" DataField="NumeroRataString">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Data Pagamento">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# OPENgovTOCO.SharedFunction.FormattaData (DataBinder.Eval(Container, "DataItem.DataPagamento")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Importo Pagato" DataField="ImportoPagato" DataFormatString="{0:0.00}">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Provenienza" DataField="Provenienza">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <!-- Dati avviso -->
            <table width="100%" style="margin-left: 3px">
                <tr>
                    <td>
                        <asp:Label CssClass="Input_Label_bold" runat="server" ID="Label6">AVVISO N°</asp:Label>
                        <asp:Label CssClass="Input_Label_bold" ID="lblAvvisoNumero" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="Input_Label_bold" runat="server" ID="Label7">ANNO</asp:Label>
                        <asp:Label CssClass="Input_Label_bold" ID="lblAvvisoAnno" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="Input_Label_bold" runat="server" ID="Label8">TOTALE</asp:Label>
                        <asp:Label CssClass="Input_Label_bold" ID="lblAvvisoTotale" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="Input_Label_bold" runat="server" ID="Label9">ARROTONDAMENTO</asp:Label>
                        <asp:Label CssClass="Input_Label_bold" ID="lblAvvisoArrotondamento" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="Input_Label_bold" runat="server" ID="Label10">DOVUTO</asp:Label>
                        <asp:Label CssClass="Input_Label_bold" ID="lblAvvisoDovuto" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="Legend" runat="server" ID="LblSgravioInCorso"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <Grd:RibesGridView ID="GrdDettaglioRuoli" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnRowCommand="GrdRowCommand">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Ubicazione">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.ArticoloTOCO.SVia")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipologia Occupazione">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.ArticoloTOCO.TipologiaOccupazione.Descrizione")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Categoria">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.ArticoloTOCO.Categoria.Descrizione")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Consistenza">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label11" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.ArticoloTOCO.Consistenza")) + " " + (DataBinder.Eval(Container, "DataItem.ArticoloTOCO.TipoConsistenzaTOCO.Descrizione"))%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Durata">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label28" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.ArticoloTOCO.DurataOccupazione")) + " " + (DataBinder.Eval(Container, "DataItem.ArticoloTOCO.TipoDurata.Descrizione"))%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tariffa">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label7" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.Tariffa.Valore")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Agev.">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label12" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.ArticoloTOCO.ElencoPercAgevolazioni"))%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Coeff.">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label8" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.ArticoloTOCO.TipologiaOccupazione.CoefficienteMoltiplicativo")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Importo" DataField="Importo">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
										<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# (DataBinder.Eval(Container, "DataItem.IdRuolo")) %>' alt=""></asp:ImageButton>
                                        <asp:HiddenField runat="server" ID="hfid" Value='<%# (DataBinder.Eval(Container, "DataItem.IdRuolo")) %>' />
									</itemtemplate>
								</asp:TemplateField>
                            </Columns>
                        </Grd:RibesGridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br>
                        <Grd:RibesGridView ID="GrdRate" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="50%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField HeaderText="N.Rata" DataField="NumeroRata">
                                    <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" Width="120px"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField Visible="True" HeaderText="Scadenza">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" Width="120px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label13" runat="server" Text='<%# OPENgovTOCO.SharedFunction.FormattaData (DataBinder.Eval(Container, "DataItem.DataScadenza")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Importo" DataField="ImportoRata" DataFormatString="{0:0.00}">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" Width="100px"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </td>
                </tr>
            </table>
        </div>
 		<div id="divEdit" style="OVERFLOW-Y:auto;WIDTH:100%;">
            <div class="col-md-12">
                <label class="lstTabRow">Dati Articolo</label>
            </div>
            <div class="col-md-12"> 
                <div class="col-md-8">
                    <label class="Input_Label">Via</label>&nbsp;
				    <asp:ImageButton ID="LnkOpenStradario" runat="server" ImageUrl="../../images/Bottoni/Listasel.png" ImageAlign="Bottom" CausesValidation="False" ToolTip="Ubicazione Immobile da Stradario."></asp:ImageButton>
                    <br>
                    <asp:TextBox ID="TxtVia" runat="server" CssClass="Input_Text" ReadOnly="True" Width="100%"></asp:TextBox>
                    <asp:HiddenField ID="hfCodVia" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hfViaRibaltata" runat="server"></asp:HiddenField>
                </div>
                <div class="col-md-2">
                    <label class="Input_Label">Civico</label><br />
                    <asp:TextBox ID="TxtCivico" onblur="isNumber(this,'','','',999999)" runat="server" Width="100px" CssClass="Input_Text_Right"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-12">
                <div class="col-md-5">
                    <label class="Input_Label">Tipologia Occupazione</label><br />
                    <asp:DropDownList ID="cmbTipologiaOccupazione" runat="server" CssClass="Input_Text" Width="100%"></asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label class="Input_Label">Categoria</label><br />
                    <asp:DropDownList ID="cmbCategoria" runat="server" CssClass="Input_Text" Width="100%"></asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label class="Input_Label">Consistenza</label><br />
                    <asp:TextBox ID="txtConsistenza" onblur="isNumber(this,'',2,'',999999)" runat="server" Width="80px" CssClass="Input_Text_Right"></asp:TextBox>
                    <asp:DropDownList ID="cmbTipoConsistenza" runat="server" CssClass="Input_Text" Width="80px"></asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label class="Input_Label">Durata</label><br />
                    <asp:TextBox ID="txtDurata" onblur="isNumber(this,'','','',999999)" Style="text-align: right" runat="server" Width="50px" CssClass="Input_Text" Enabled="true"></asp:TextBox>
                    <asp:DropDownList ID="cmbTipoDurata" runat="server" CssClass="Input_Text"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-12">
                <div class="col-md-1">
                    <br />
                    <asp:CheckBox runat="server" CssClass="Input_CheckBox_NoBorder" Text="Attrazione" ID="chkAttrazione"></asp:CheckBox>
                </div>
                <div class="col-md-4">
                    <label class="lstTabRow">Maggiorazione</label><br />
                    <div class="col-md-6">
                        <label class="Input_Label">Importo fisso</label>
                        <asp:TextBox ID="txtMaggiorazioni" onblur="if (isNumber(this,'',2,'',999999)) this.value = number_format(this.value, 2, ',', '');" runat="server" CssClass="Input_Text_right" MaxLength="10" Width="80px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <asp:TextBox ID="txtMaggiorazioniPerc" onblur="if (isNumber(this,'',2,'',999999)) this.value = number_format(this.value, 2, ',', '');" runat="server" CssClass="Input_Text_right" MaxLength="10" Width="80px"></asp:TextBox>
                        <label class="Input_Label">%</label>
                    </div>
                </div>
                <div class="col-md-1">
                    <label class="Input_Label">Detrazioni</label><br />
                    <asp:TextBox ID="txtDetrazioni" onblur="this.value = number_format(this.value, 2, ',', '');" runat="server" CssClass="Input_Text_right" MaxLength="10" Width="80px"></asp:TextBox>
                </div>
                <div class="col-md-5">
                    <Grd:RibesGridView ID="GrdAgevolazioni" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="descrizione" HeaderText="Agevolazione">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Sel.">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkSelezionato" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Selezionato")%>'></asp:CheckBox>
                                    <asp:HiddenField ID="hfIdAgevolazione" runat="server" Value='<%# Eval("IdAgevolazione") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </div>
            </div>
		</div>
        <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
            <div class="BottoneClessidra">&nbsp;</div>
            <div class="Legend">Attendere Prego</div>
        </div>
        <div id="divStampa" class="col-md-12 classeFiledSetRicerca" style="display:none;">
            <iframe runat="server" id="loadStampa" name="loadStampa" frameborder="0" width="100%" height="250px" src="../../aspvuotaremovecomandi.aspx"></iframe>
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
        <asp:HiddenField ID="hfIdFlusso" runat="server" />
        <asp:HiddenField ID="hfIdSgravio" runat="server" />
        <asp:HiddenField ID="hfVisualArticolo" runat="server" />
        <asp:HiddenField ID="hfSgravioInCorso" runat="server" />
        <asp:HiddenField ID="hfSgravato" runat="server" />
    </form>
</body>
</html>
