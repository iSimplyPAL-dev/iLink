<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgov.Master" AutoEventWireup="true" CodeBehind="Tariffe.aspx.cs" Inherits="OPENgov.Acquisizioni.TARES.Configurazione.Tariffe" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Row" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHeader" runat="server">
    <div id="divSearch">
        <asp:Button ID="cmdPrint" runat="server" Cssclass="BottoneExcel Bottone" ToolTip="Stampa" CausesValidation="False" onclick="CmdPrintClick"/>
        <asp:Button ID="cmdCopyYear" runat="server" Cssclass="BottoneDuplica Bottone" ToolTip="Ribalta Anno" CausesValidation="False" onclick="CmdCopyYearClick"/>
        <asp:Button ID="cmdSave" runat="server" Cssclass="BottoneSalva Bottone" ToolTip="Salva modifiche" CausesValidation="False" onclick="CmdSaveClick" ValidationGroup="SaveCategory"/>
        <asp:Button ID="cmdInsert" runat="server" Cssclass="BottoneNewInsert Bottone" ToolTip="Inserisci nuovo Anno" CausesValidation="False" onclick="CmdInsertClick"/>
        <asp:Button ID="cmdSearch" runat="server" Cssclass="BottoneRicerca Bottone" ToolTip="Cerca..." CausesValidation="True" onclick="CmdSearchClick" />
    </div>
    <div id="divEdit" style="display:none;">
        <asp:Button ID="Button1" runat="server" Cssclass="BottoneSalva Bottone" ToolTip="Salva modifiche" CausesValidation="True" onclick="CmdSaveClick" ValidationGroup="SaveCategory"/>
        <asp:Button ID="cmdBack" runat="server" Cssclass="BottoneAnnulla Bottone" ToolTip="Annulla" CausesValidation="False" onclick="CmdBackClick"/>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
    <div style="margin: 10px auto;">
        <fieldset class="FiledSetRicerca">
            <asp:RadioButton ID="optCategorie" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeCodDescr" OnCheckedChanged="ChangeTypeCodDescr" Text="Tariffe Categorie" Checked="true" />&nbsp;
            <asp:RadioButton ID="optCatConferimenti" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeCodDescr" OnCheckedChanged="ChangeTypeCodDescr" Text="Tariffe Conferimenti"/>&nbsp;
            <asp:RadioButton ID="optRiduzioni" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeCodDescr" OnCheckedChanged="ChangeTypeCodDescr" Text="Riduzioni"/>&nbsp;
            <asp:RadioButton ID="optEsenzioni" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeCodDescr" OnCheckedChanged="ChangeTypeCodDescr" Text="Esenzioni"/>&nbsp;
            <asp:RadioButton ID="optMaggiorazioni" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeCodDescr" OnCheckedChanged="ChangeTypeCodDescr" Text="Maggiorazioni"/>&nbsp;
        </fieldset>
    </div>
    <div id="ParamSearch" style="margin: 0 auto;">
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Inserimento filtri di ricerca</legend>
            <asp:Label ID="lblYear" runat="server" Text="Anno" CssClass="Input_Label"></asp:Label>&nbsp;
            <Row:RibesDropDownList ID="rddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rddlYearSelectedIndexChanged" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>&nbsp;
            <asp:HiddenField ID="hfTypeCalcolo" runat="server"/>
            <asp:Label ID="lblCat" runat="server" Text="Categoria" CssClass="Input_Label"></asp:Label>&nbsp;
            <Row:RibesDropDownList ID="rddlCategorie" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text" Width="600px"></Row:RibesDropDownList>&nbsp;
            <Row:RibesDropDownList ID="rddlTipoCat" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>&nbsp;
        </fieldset>
    </div>
    <div id="ParamCopyYear" style="margin: 0 auto;" class="Input_Label" style="display:none;">
        <fieldset class="FiledSetRicerca">
            <strong>Attenzione:</strong>i dati relativi alle Tariffe verranno copiati dall'anno prescelto sull'anno indicato dall'operatore, sovrascrivendo eventuali tariffe già impostate a fronte dell'anno di destinazione.<br />
            <asp:Label ID="lblYearFrom" runat="server" Text="Da Anno" CssClass="Input_Label"></asp:Label>&nbsp;
            <Row:RibesDropDownList ID="rddlYearFrom" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>&nbsp;
            <asp:Label ID="lblYearTo" runat="server" Text="Ad Anno" CssClass="Input_Label"></asp:Label>&nbsp;
            <Row:RibesDropDownList ID="rddlYearTo" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>&emsp;&emsp;&emsp;&emsp;
            <asp:Button ID="cmdSaveCopy" runat="server" Cssclass="Bottone BottoneSalva Bottone" ToolTip="Ribalta" CausesValidation="False" onclick="CmdSaveCopyClick"/>
        </fieldset>
    </div>
    <div id="SearchTariffe" style="display:none;">
        <fieldset class="classeFiledSetNoBorder">
            <legend class="Legend">Risultati della ricerca</legend>
            <Row:RibesGridView ID="rgvTariffe" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="50" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                onpageindexchanging="RgvTariffePageIndexChanging" onrowcommand="RgvTariffeRowCommand">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="Anno" HeaderText="Anno">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Descrizione" HeaderText="Categoria (NC)">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Tariffa PF">
                        <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtPF" runat="server" CssClass="Input_Text_Numbers" Text='<%# Eval("impPF") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tariffa PV">
                        <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtPV" runat="server" CssClass="Input_Text_Numbers" Text='<%# Eval("impPV") %>'></asp:TextBox>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgDelete" runat="server" CssClass="BottoneGrd BottoneCancellaGrd" CausesValidation="False" CommandName="DeleteTariffe" CommandArgument='<%# Eval("IdTariffa") %>' alt="" OnClientClick="return confirm('Attenzione!\nStai per eliminare una tariffa:\nVuoi procedere?')"></asp:ImageButton> 
                                <span class="tooltiptext">Elimina</span>
                            </div>
                            <asp:HiddenField ID="hfNC" runat="server" Value='<%# Eval("NComponenti") %>' />
                            <asp:HiddenField ID="hfIdCategoria" runat="server" Value='<%# Eval("IdCategoria") %>' />
                            <asp:HiddenField ID="hfIdTariffa" runat="server" Value='<%# Eval("IdTariffa") %>' />
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                </Columns>
            </Row:RibesGridView>
        </fieldset>
    </div>
    <div id="SearchRidEse" style="display:none;">
        <fieldset class="classeFiledSetNoBorder">
            <legend class="Legend">Risultati della ricerca</legend>
            <Row:RibesGridView ID="rgvRidEse" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                onpageindexchanging="RgvRidEsePageIndexChanging" onrowcommand="RgvRidEseRowCommand">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="Anno" HeaderText="Anno">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Left"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="PF">
                        <headerstyle horizontalalign="Center" Width="30px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkPF" runat="server" CssClass="Input_CheckBox_NoBorder" Checked='<%# Eval("hasPF") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PV">
                        <headerstyle horizontalalign="Center" Width="30px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkPV" runat="server" CssClass="Input_CheckBox_NoBorder" Checked='<%# Eval("hasPV") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PC">
                        <headerstyle horizontalalign="Center" Width="30px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkPC" runat="server" CssClass="Input_CheckBox_NoBorder" Checked='<%# Eval("hasPC") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PM">
                        <headerstyle horizontalalign="Center" Width="30px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkPM" runat="server" CssClass="Input_CheckBox_NoBorder" Checked='<%# Eval("hasPM") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imponibile">
                        <headerstyle horizontalalign="Center" Width="60px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkImponibile" runat="server" CssClass="Input_CheckBox_NoBorder" Checked='<%# Eval("hasImponibile") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DescrTipo" HeaderText="Tipo">
                        <headerstyle horizontalalign="Center" Width="30px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Valore" HeaderText="Valore">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgEdit" runat="server" CssClass="BottoneGrd BottoneApriGrd" CausesValidation="False" CommandName="EditRidEse" CommandArgument='<%# Eval("IdTariffa") %>' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Modifica</span>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgDelete" runat="server" CssClass="BottoneGrd BottoneCancellaGrd" CausesValidation="False" CommandName="DeleteRidEse" CommandArgument='<%# Eval("IdTariffa") %>' alt="" OnClientClick="return confirm('Attenzione!\nStai per eliminare una tariffa:\nVuoi procedere?')"></asp:ImageButton> 
                                <span class="tooltiptext">Elimina</span>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                </Columns>
            </Row:RibesGridView>
        </fieldset>
    </div>
    <div id="SearchMaggiorazioni" style="display:none;">
        <fieldset class="classeFiledSetNoBorder">
            <legend class="Legend">Risultati della ricerca</legend>
            <!--*** 201712 - gestione tipo conferimento ***-->
            <Row:RibesGridView ID="rgvMaggiorazioni" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                onpageindexchanging="RgvMaggiorazioniPageIndexChanging" onrowcommand="RgvMaggiorazioniRowCommand">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="Anno" HeaderText="Anno">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TipoConferimento" HeaderText="Tipo Conferimento">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Importo">
                        <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtPM" runat="server" CssClass="Input_Text_Numbers" Text='<%# Eval("impPF") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Moltiplicatore Min.">
                        <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtMoltiplicatoreMinimo" runat="server" CssClass="Input_Text_Numbers" Text='<%# Eval("MoltiplicatoreMinimo") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imp.Minimo">
                        <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtImpMinimo" runat="server" CssClass="Input_Text_Numbers" Text='<%# Eval("impMinimo") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgDelete" runat="server" CssClass="BottoneGrd BottoneCancellaGrd" CausesValidation="False" CommandName="DeleteMaggiorazioni" CommandArgument='<%# Eval("IdTariffa") %>' alt="" OnClientClick="return confirm('Attenzione!\nStai per eliminare una tariffa:\nVuoi procedere?')"></asp:ImageButton> 
                                <span class="tooltiptext">Elimina</span>
                            </div>
                            <asp:HiddenField ID="hfIdCategoria" runat="server" Value='<%# Eval("IdCategoria") %>' />
                            <asp:HiddenField ID="hfIdTariffa" runat="server" Value='<%# Eval("IdTariffa") %>' />
                            <asp:HiddenField ID="hfIdTipoConferimento" runat="server" Value='<%# Eval("IdTipoConferimento") %>' />
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                </Columns>
            </Row:RibesGridView>
        </fieldset>
    </div>
    <div id="AddRidEse" style="margin: 10px auto;" style="display:none;">
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Anno" CssClass="Input_Label"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Riduzione" CssClass="Input_Label"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Tipologia" CssClass="Input_Label"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Valore" CssClass="Input_Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <Row:RibesDropDownList ID="rddlYearRidEse" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>
                </td>
                <td>
                    <Row:RibesDropDownList ID="rddlRidEse" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>
                </td>
                <td>
                    <Row:RibesDropDownList ID="rddlTipoRidEse" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>
                </td>
                <td>
                    <asp:TextBox ID="txtValoreRidEse" runat="server" CssClass="Input_Text_Numbers"></asp:TextBox>
                    <asp:HiddenField ID="hfIdRidEse" runat="server" Value="0" />
                    <asp:HiddenField ID="hfIsNewRid" runat="server" Value="0" />
                </td>
            </tr>
            <tr>
                <td colspan="3" class="hidden">
                    <Row:RibesGridView ID="rgvCat" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
				        AutoGenerateColumns="False" AllowPaging="True" PageSize="30" HoverRowCssClass="riga_tabella_mouse_over"
				        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        onpageindexchanging="RgvCatPageIndexChanging" onrowcommand="RgvCatRowCommand">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="CodiceCategoria" HeaderText="Codice">
                                <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Descrizione" HeaderText="Descrizione">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Sel">
                                <headerstyle horizontalalign="Center" Width="60px"></headerstyle>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAll" runat="server" Text="Tutte" ToolTip="Tutte" TextAlign="Left" Checked="true" Enabled="false" />
                                </HeaderTemplate>
                                <itemstyle horizontalalign="Center"></itemstyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSel" runat="server" CssClass="Input_CheckBox_NoBorder" Checked="true" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Row:RibesGridView>
                </td>
                <td valign="top">
                    <asp:RadioButton ID="optSingleP" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeAppl" OnCheckedChanged="ChangeTypeAppl" Text="Singole Parti" Checked="true" />&nbsp;
                    <br />
                    <asp:CheckBox ID="chkPF" runat="server" CssClass="Input_CheckBox_NoBorder" Text="PF" ToolTip="Applica a Parte Fissa" />&nbsp;<asp:CheckBox ID="chkPV" runat="server" CssClass="Input_CheckBox_NoBorder" Text="PV" ToolTip="Applica a Parte Variabile" />&nbsp;<asp:CheckBox ID="chkPC" runat="server" CssClass="Input_CheckBox_NoBorder" Text="PC" ToolTip="Applica a Conferimenti" />&nbsp;<asp:CheckBox ID="chkPM" runat="server" CssClass="Input_CheckBox_NoBorder" Text="PM" ToolTip="Applica a Maggiorazione" />&nbsp;
                    <br /><br /><br />
                    <asp:RadioButton ID="optImponibile" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeAppl" OnCheckedChanged="ChangeTypeAppl" Text="Imponibile" />&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
