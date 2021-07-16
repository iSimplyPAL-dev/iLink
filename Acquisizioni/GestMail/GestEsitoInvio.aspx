<%@ Page Language="C#" MasterPageFile="~/OPENgov.Master" AutoEventWireup="true" CodeBehind="GestEsitoInvio.aspx.cs" Inherits="OPENgov.Acquisizioni.GestMail.GestEsitoInvio" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Row" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHeader" runat="server">
    <div id="ComandiRicerca">
        <asp:Button ID="cmdSearch" runat="server" Cssclass="BottoneRicerca Bottone" ToolTip="Cerca..." CausesValidation="False" onclick="CmdSearchClick" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
    <div id="RicercaLotti" style="margin:10px auto;">
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Parametri di ricerca</legend>
            <div id="Div2">
                <table>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblTributo" runat="server" Text="Tributo" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <Row:RibesDropDownList ID="rddlTributo" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblAnno" runat="server" Text="Anno" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <Row:RibesDropDownList ID="rddlAnno" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>&nbsp;
                        </td>
                    </tr>
                </table>                
            </div>
        </fieldset>
    </div>
    <div id="ResultRicerca">
        <fieldset class="classeFiledSetNoBorder">
            <legend class="Legend">Risultati della ricerca</legend>
            <asp:Label ID="lblResultLotti" runat="server" Text="Non sono stati trovati Lotti in Invio" CssClass="Input_Label"></asp:Label>
            <Row:RibesGridView ID="rgvLotti" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="15" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" OnRowCommand="rgvLottiRowCommand" OnPageIndexChanging="rgvLottiPageIndexChanging">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="DescrTributo" HeaderText="Tributo">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Anno" HeaderText="Anno">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="IdLotto" HeaderText="N.Lotto">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="EMailSubject" HeaderText="Oggetto Mail">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="NDestinatari" HeaderText="N.Destinatari">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DescrStato" HeaderText="Stato">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgView" runat="server" CssClass="BottoneGrd BottoneApriGrd" CausesValidation="False" CommandName="ViewLotto" CommandArgument='<%# Eval("IdLotto") %>' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Dettaglio</span>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton runat="server" CssClass="BottoneGrd BottoneMailGrd" CausesValidation="False" CommandName="Resend" CommandArgument='<%# Eval("IdLotto") %>' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Invio mail</span>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                </Columns>
            </Row:RibesGridView>
        </fieldset>
    </div>
   <div id="DivDettaglioLotto">
        <fieldset class="classeFiledSetNoBorder">
            <legend class="Legend">Risultati della ricerca</legend>
            <asp:Label ID="lblResultEsitiInvio" runat="server" Text="Non sono stati trovati Esiti di Invio" CssClass="Input_Label"></asp:Label>
            <Row:RibesGridView ID="rgvEsitiInvio" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="15" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" OnPageIndexChanging="rgvEsitiInvioPageIndexChanging">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="Cognome" HeaderText="cognome">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Nome" HeaderText="Nome">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="EmailDest" HeaderText="Indirizzo Mail">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DescrStato" HeaderText="Stato">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                </Columns>
            </Row:RibesGridView>
        </fieldset>
        <asp:HiddenField ID="hfIDLotto" runat="server" Value="0" />
    </div>
</asp:Content>
