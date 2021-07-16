<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgov.Master" AutoEventWireup="true" CodeBehind="CompraVenditaRicerca.aspx.cs" Inherits="OPENgov.Acquisizioni.AttiCompraVendita.CompraVenditaRicerca" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Row" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHeader" runat="server">
    <div id="ComandiRicerca">
        <asp:Button id="cmdStampa" Cssclass="Bottone BottoneExcel" ToolTip="Stampa..." onclick="CmdStampaClick" runat="server" CausesValidation="False"/>
        <asp:Button ID="cmdSearch" runat="server" Cssclass="BottoneRicerca Bottone" ToolTip="Cerca..." CausesValidation="False" onclick="CmdSearchClick" />
    </div>
    <div id="ComandiGestione" style="display:none;">
        <asp:Button ID="cmdBack" runat="server" Cssclass="BottoneAnnulla Bottone" ToolTip="Torna alla ricerca..." CausesValidation="False" onclick="CmdBackClick" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
    <div id="Ricerca" style="margin: 10px auto;">
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Parametri di ricerca</legend>
            <table>
                <tr>
                    <td>
                        <asp:RadioButton ID="rbImmobile" runat="server" GroupName="rbRicerca" CssClass="Input_Label" Text="Immobile" OnCheckedChanged="ChangeParamSearch" AutoPostBack="true" Checked="true"/>&nbsp;
                        <asp:RadioButton ID="rbSoggetto" runat="server" GroupName="rbRicerca" CssClass="Input_Label" Text="Soggetto" OnCheckedChanged="ChangeParamSearch" AutoPostBack="true"/>&nbsp;
                   </td>
                   <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="LblStato" runat="server" Text="Stato" CssClass="Input_Label"></asp:Label>&nbsp;
                        <asp:DropDownList ID="rddlStato" runat="server" CssClass="Input_Label"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="LblTipoImmobile" runat="server" Text="Tipo Immobile" CssClass="Input_Label"></asp:Label>&nbsp;
                        <Row:RibesDropDownList ID="rddlTipoImmobile" runat="server" CssClass="Input_Label"></Row:RibesDropDownList>
                    </td>
                </tr>
            </table>            
            <div id="RicercaImmobile">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblFoglio" runat="server" Text="Foglio" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <asp:TextBox ID="txtFoglio" runat="server" TextMode="SingleLine" CssClass="Input_Text" Columns="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblNumero" runat="server" Text="Numero" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <asp:TextBox ID="txtNumero" runat="server" TextMode="SingleLine" CssClass="Input_Text" Columns="4"></asp:TextBox>
                       </td>
                        <td>
                            <asp:Label ID="lblSub" runat="server" Text="Subalterno" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <asp:TextBox ID="txtSub" runat="server" TextMode="SingleLine" CssClass="Input_Text" Columns="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblUbicazione" runat="server" Text="Ubicazione" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <asp:TextBox ID="txtUbicazione" runat="server" TextMode="SingleLine" CssClass="Input_Text" Columns="80"></asp:TextBox>
                        </td>
                    </tr>
                </table>                
            </div>
            <div id="RicercaSoggetto" style="display:none;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblNominativo" runat="server" Text="Nominativo" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <asp:TextBox ID="txtNominativo" runat="server" TextMode="SingleLine" CssClass="Input_Text" Columns="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblCfPiva" runat="server" Text="Cod.Fiscale/P.IVA" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <asp:TextBox ID="txtCfPiva" runat="server" TextMode="SingleLine" CssClass="Input_Text" Columns="16"></asp:TextBox>
                        </td>
                    </tr>
                </table>                
            </div>            
        </fieldset>
        <asp:LinkButton ID="LblDownloadFile" Runat="server" CssClass="Input_Label" Font-Underline="True" onclick="LblDownloadFile_Click"></asp:LinkButton>
        <fieldset class="classeFiledSetNoBorder" id="fsRisultatiRicerca" runat="server">
            <legend class="Legend">Risultati della ricerca</legend>
            <asp:Label ID="lblResult" runat="server" Text="Non sono stati trovati risultati per la ricerca inserita" CssClass="Input_Label"></asp:Label>
            <Row:RibesGridView ID="rgvAtti" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="15" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" OnRowCommand="rgvAttiRowCommand" OnPageIndexChanging="rgvAttiPageIndexChanging">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="RifCatastali" HeaderText="Rif. Catastali (FG-NUM-SUB)">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Ubicazione" HeaderText="Ubicazione">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TipoImmobile" HeaderText="Tipo Immobile">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DataValidita" HeaderText="Data Validità">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DataPresentazione" HeaderText="Data Presentazione">
                        <headerstyle horizontalalign="Center"  Width="120px"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="NumNota" HeaderText="N. Nota">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DescrAtto" HeaderText="Descrizione Atto">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Stato" HeaderText="Stato">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgEdit" runat="server" CssClass="BottoneGrd BottoneApriGrd" CausesValidation="False" CommandName="EditAtto" CommandArgument='<%# Eval("IdImmobile") %>' alt=""></asp:ImageButton>
                                <span class="tooltiptext">Gestione</span>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IdAtto" HeaderText="IdAtto" Visible="False">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                </Columns>
            </Row:RibesGridView>
            <Row:RibesGridView ID="rgvAttiSoggetti" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="15" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" OnRowCommand="rgvAttiRowCommand" OnPageIndexChanging="rgvAttiPageIndexChanging">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="Nominativo" HeaderText="Nominativo">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CfPiva" HeaderText="Cod.Fiscale/P.IVA">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Diritto" HeaderText="Diritto">
                        <headerstyle horizontalalign="Center" Width="140px"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="RifCatastali" HeaderText="Rif. Catastali (FG-NUM-SUB)">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TipoImmobile" HeaderText="Tipo Immobile">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DataValidita" HeaderText="Data Validità">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DataPresentazione" HeaderText="Data Presentazione">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="NumNota" HeaderText="N. Nota">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DescrAtto" HeaderText="Descrizione Atto">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Stato" HeaderText="Stato">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgEdit" runat="server" CssClass="BottoneGrd BottoneApriGrd" CausesValidation="False" CommandName="EditAtto" CommandArgument='<%# Eval("IdImmobile") %>' alt=""></asp:ImageButton>
                                <span class="tooltiptext">Gestione</span>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IdAtto" HeaderText="IdAtto" Visible="False">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                </Columns>
            </Row:RibesGridView>
        </fieldset>
    </div>
    <div id="GestioneAtto" style="display:none;">
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Dati nota trascrizione</legend>
            <br />
            &nbsp;<asp:Label ID="lblNotaTrascrizione" runat="server" Text="" CssClass="Input_Label"></asp:Label>
            <br />&nbsp;
        </fieldset>
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Dati Immobile in nota</legend>
            <p>&nbsp;<asp:Label ID="lblRifNota" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
            <p>&nbsp;<asp:Label ID="lblCatNota" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
            <p>&nbsp;<asp:Label ID="lblUbicazioneNota" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
            <p>&nbsp;<asp:Label ID="lblUbicazioneCatasto" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
        </fieldset>
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Dati soggetti</legend>
            <table>
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblAcquirenti" runat="server" Text="Elenco Acquirenti" CssClass="Legend"></asp:Label>
                        <Row:RibesGridView ID="rgvAcquirenti" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="True" PageSize="5" HoverRowCssClass="riga_tabella_mouse_over"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" OnRowCommand="rgvAcquirentiRowCommand" OnPageIndexChanging="rgvAcquirentiPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="Nominativo" HeaderText="Nominativo">
                                    <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                    <itemstyle horizontalalign="Center"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CfPIva" HeaderText="Cod.Fiscale/P.Iva">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Diritto" HeaderText="Diritto">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Stato" HeaderText="Stato">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <itemtemplate>
                                        <div class="tooltip">
                                            <asp:ImageButton id="imgEdit" runat="server" CssClass="BottoneGrd BottoneApriGrd" CausesValidation="False" CommandName="EditAcquirente" CommandArgument='<%# Eval("IdSoggetto") %>' alt=""></asp:ImageButton>
                                            <span class="tooltiptext">Gestione</span>
                                        </div>
                                    </itemtemplate>
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                                </asp:TemplateField>
                                <asp:BoundField DataField="IdAcquirente" HeaderText="IdAcquirente" Visible="False">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Row:RibesGridView>
                    </td>
                    <td valign="top">
                        <asp:Label ID="lblCessionari" runat="server" Text="Elenco Cessionari" CssClass="Legend"></asp:Label>
                        <Row:RibesGridView ID="rgvCessionari" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="True" PageSize="5" HoverRowCssClass="riga_tabella_mouse_over"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" OnRowCommand="rgvCessionariRowCommand" OnPageIndexChanging="rgvCessionariPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="Nominativo" HeaderText="Nominativo">
                                    <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                    <itemstyle horizontalalign="Center"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CfPIva" HeaderText="Cod.Fiscale/P.Iva">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Diritto" HeaderText="Diritto">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Stato" HeaderText="Stato">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <itemtemplate>
                                        <div class="tooltip">
                                            <asp:ImageButton id="imgEdit" runat="server" CssClass="BottoneGrd BottoneApriGrd" CausesValidation="False" CommandName="EditCessionario" CommandArgument='<%# Eval("IdSoggetto") %>' alt=""></asp:ImageButton>
                                            <span class="tooltiptext">Gestione</span>
                                        </div>
                                    </itemtemplate>
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                                </asp:TemplateField>
                                <asp:BoundField DataField="IdCessionario" HeaderText="IdCessionario" Visible="False">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Row:RibesGridView>
                    </td>
                </tr>
            </table>
        </fieldset>
        <asp:HiddenField ID="hfidImmobile" runat="server" Value="0" />
    </div>
</asp:Content>
