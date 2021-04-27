<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="WucArticolo.ascx.cs" Inherits="OPENgovTOCO.Wuc.WucArticolo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Src="./WucDatiContribuente.ascx" TagName="wucDatiContribuente" TagPrefix="uc1" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<table id="TblGenDich" style="width: 100%" cellspacing="1" cellpadding="1" border="0">
    <!--blocco dati contribuente-->
    <%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
    <tr id="TRPlainAnag">
        <td>
            <iframe id="ifrmAnag" runat="server" src="../../aspVuotaRemoveComandi.aspx" style="height: 200px; width: 100%" frameborder="0" marginheight="0"></iframe>
            <asp:HiddenField ID="hdIdContribuente" runat="server" Value="-1" />
        </td>
    </tr>
    <tr id="TRSpecAnag">
        <td>
            <uc1:wucDatiContribuente ID="wucContribuente" runat="server"></uc1:wucDatiContribuente>
        </td>
    </tr>
    <!--Blocco Dati UI-->
    <tr>
        <td>
            <table id="TblDati" width="100%">
                <tr>
                    <td colspan="9">
                        <asp:Label ID="Label10" runat="server" Width="100%" CssClass="lstTabRow">Dati Articolo</asp:Label></td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:Label ID="Label3" runat="server" CssClass="Input_Label">Via</asp:Label><asp:Label ID="Label14" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label>&nbsp;
						<asp:ImageButton ID="LnkOpenStradario" runat="server" ImageUrl="../../images/Bottoni/Listasel.png"
                            ImageAlign="Bottom" CausesValidation="False" ToolTip="Ubicazione Immobile da Stradario."></asp:ImageButton>
                        <br>
                        <asp:TextBox ID="TxtVia" runat="server" Width="720px" CssClass="Input_Text" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="TxtCodVia" Style="display: none" runat="server"></asp:TextBox>
                        <asp:TextBox ID="TxtViaRibaltata" Style="display: none" runat="server"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <asp:Label runat="server" CssClass="Input_Label" ID="Label27">Tributo</asp:Label><br>
                        <asp:TextBox ID="TxtTributo" runat="server" ReadOnly="True" Width="150px" CssClass="Input_Text"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" CssClass="Input_Label">Civico</asp:Label><br>
                        <asp:TextBox ID="TxtCivico" runat="server" Width="70px" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" CssClass="Input_Label">Esponente</asp:Label><br>
                        <asp:TextBox ID="TxtEsponente" runat="server" Width="70px" CssClass="Input_Text_Right"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" CssClass="Input_Label">Interno</asp:Label><br>
                        <asp:TextBox ID="TxtInterno" runat="server" Width="70px" CssClass="Input_Text_Right"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label8" runat="server" CssClass="Input_Label">Scala</asp:Label><br>
                        <asp:TextBox ID="TxtScala" runat="server" Width="70px" CssClass="Input_Text_Right"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label11" runat="server" CssClass="Input_Label">Data Inizio</asp:Label>
                        <asp:Label ID="Label17" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br>
                        <asp:TextBox runat="server" ID="TxtDataInizio" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" MaxLength="10"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label12" runat="server" CssClass="Input_Label">Data fine</asp:Label>
                        <asp:Label ID="Label24" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br>
                        <asp:TextBox runat="server" ID="txtDataFine" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" MaxLength="10"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label19" runat="server" CssClass="Input_Label">Tipo Durata</asp:Label>
                        <asp:Label ID="Label21" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br>
                        <asp:DropDownList ID="cmbTipoDurata" runat="server" CssClass="Input_Text" AutoPostBack="True" OnSelectedIndexChanged="cmbTipoDurata_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label15" runat="server" CssClass="Input_Label">Durata</asp:Label>
                        <asp:Label ID="Label16" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br>
                        <asp:TextBox ID="txtDurata" runat="server" Width="50px" CssClass="Input_Text_Right OnlyNumber" Enabled="true"></asp:TextBox>
                    </td>
                    <td>
                        <br>
                        <asp:CheckBox runat="server" CssClass="Input_CheckBox_NoBorder" Text="Attrazione" ID="chkAttrazione"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="Label13" runat="server" CssClass="Input_Label">Tipologia Occupazione</asp:Label>
                        <asp:Label ID="Label23" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br>
                        <asp:DropDownList ID="cmbTipologiaOccupazione" runat="server" Width="460px" CssClass="Input_Text"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label18" runat="server" CssClass="Input_Label"> Consistenza</asp:Label>
                        <asp:Label ID="Label22" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br>
                        <asp:TextBox ID="txtConsistenza" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox>
                    </td>
                    <td>
                        <br>
                        <asp:DropDownList ID="cmbTipoConsistenza" runat="server" Width="70px" CssClass="Input_Text"></asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="Label20" runat="server" CssClass="Input_Label">Categoria</asp:Label>
                        <asp:Label ID="Label9" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br>
                        <asp:DropDownList ID="cmbCategoria" runat="server" Width="128px" CssClass="Input_Text"></asp:DropDownList>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table id="TblUnitaImmo" cellspacing="0" cellpadding="0" border="0">
                <!--Blocco Dati Catastali-->
                <tr>
                    <td>
                        <table id="TblCatastali" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td colspan="6"></td>
                            </tr>
                            <tr>
                                <td>
                                    <br>
                                </td>
                                <td>
                                    <br>
                                </td>
                                <td>
                                    <br>
                                </td>
                                <td>
                                    <br>
                                </td>
                                <td>
                                    <br>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <!--Blocco Dati Riduzioni/Detassazioni-->
                <tr>
                    <td>
                        <table id="TblRidEse" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td valign="top">
                                    <table style="width: 382px; height: 28px">
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="Label33" runat="server" Width="250px" CssClass="lstTabRow">Maggiorazione</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Width="100px" CssClass="lstTabRow">Detrazioni</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Width="200px" CssClass="lstTabRow">Agevolazioni</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:TextBox ID="txtMaggiorazioni" onblur="if (isNumber(this,'',2,'',999999)) this.value = number_format(this.value, 2, ',', '');" runat="server" Width="80px" CssClass="Input_Text_Right OnlyNumber" MaxLength="10"></asp:TextBox>
                                            </td>
                                            <td valign="top">
                                                <asp:Label ID="Label25" runat="server" CssClass="Input_Label" Width="80px">Importo fisso</asp:Label>
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtMaggiorazioniPerc" onblur="if (isNumber(this,'',2,'',999999)) this.value = number_format(this.value, 2, ',', '');" runat="server" Width="50px" CssClass="Input_Text_Right OnlyNumber" MaxLength="10"></asp:TextBox>
                                            </td>
                                            <td valign="top">
                                                <asp:Label ID="Label26" runat="server" CssClass="Input_Label">%</asp:Label>
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtDetrazioni" onblur="this.value = number_format(this.value, 2, ',', '');" runat="server" Width="60px" CssClass="Input_Text_Right OnlyNumber" MaxLength="10"></asp:TextBox>
                                            </td>
                                            <td>
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
                                                                <asp:CheckBox ID="ChkSelezionato" runat="server" AutoPostBack="false" Checked='<%# DataBinder.Eval(Container, "DataItem.Selezionato")%>'></asp:CheckBox>
                                                                <asp:HiddenField ID="hfIdAgevolazione" runat="server" Value='<%# Eval("IdAgevolazione") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </Grd:RibesGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <!--Blocco Note UI-->
                <tr>
                    <td>
                        <table id="TblNoteUI" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Width="762px" CssClass="lstTabRow">Note</asp:Label><br>
                                    <asp:TextBox ID="TxtNoteUI" runat="server" CssClass="Input_Text" MaxLength="4000" Width="700px" TextMode="MultiLine" Height="32px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
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
<asp:Button ID="updateGrdRiduzioni" Style="display: none" runat="server"></asp:Button><asp:Button ID="btnUpdate" Style="display: none" runat="server"></asp:Button>
<asp:TextBox ID="TxtIdArticoloPadre" Style="display: none" runat="server" CssClass="Input_Text">0</asp:TextBox>
