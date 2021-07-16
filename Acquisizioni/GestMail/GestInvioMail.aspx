<%@ Page Language="C#" MasterPageFile="~/OPENgov.Master" AutoEventWireup="true" CodeBehind="GestInvioMail.aspx.cs" Inherits="OPENgov.Acquisizioni.GestMail.GestInvioMail" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Row" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHeader" runat="server">
    <div id="ComandiRicerca">
        <asp:Button ID="cmdSave" runat="server" Cssclass="BottoneSalva Bottone" ToolTip="Salva modifiche" CausesValidation="True" onclick="CmdSaveClick" ValidationGroup="SaveParamCalcolo"/>
        <asp:Button ID="cmdSearch" runat="server" Cssclass="BottoneRicerca Bottone" ToolTip="Cerca..." CausesValidation="False" onclick="CmdSearchClick" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
    <div class=""></div>
    <div id="DettaglioMail" style="margin: 10px auto;">
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Dati E-Mail</legend>
            <div id="Div1" style="margin: 10px auto;">
                <div class="col-md-11">
                    <div class="col-md-5">
                        <asp:Label ID="lblServer" runat="server" CssClass="Input_Label">Server Mail</asp:Label><br />
                        <asp:TextBox ID="txtServer" runat="server" CssClass="Input_Text col-md-11"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblServerPort" runat="server" CssClass="Input_Label">Porta</asp:Label><br />
                        <asp:TextBox ID="txtServerPort" runat="server" CssClass="Input_Text"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblSSL" runat="server" CssClass="Input_Label">SSL</asp:Label><br />
                        <asp:TextBox ID="txtSSL" runat="server" CssClass="Input_Text"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-11">
                    <div class="col-md-5">
                        <asp:Label ID="lblSender" runat="server" CssClass="Input_Label">Mittente</asp:Label><br />
                        <asp:TextBox ID="txtSender" runat="server" CssClass="Input_Text col-md-11" MaxLength="255"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblSenderName" runat="server" CssClass="Input_Label">Nome mittente</asp:Label><br />
                        <asp:TextBox ID="txtSenderName" runat="server" CssClass="Input_Text col-md-11" MaxLength="255"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblSenderPwd" runat="server" CssClass="Input_Label">Password</asp:Label><br />
                        <asp:TextBox ID="txtSenderPwd" runat="server" CssClass="Input_Text col-md-11"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-11">
                    <asp:Label ID="lblSubject" runat="server" CssClass="Input_Label">Oggetto</asp:Label><br />
                    <asp:TextBox ID="txtSubject" runat="server" CssClass="Input_Text col-md-11"></asp:TextBox>
                </div>
                <div class="col-md-11">
                    <asp:Label ID="lblBody" runat="server" CssClass="Input_Label">Testo</asp:Label><br />
                    <asp:TextBox ID="txtBody" runat="server" CssClass="Input_Text col-md-11"></asp:TextBox>
                </div>  
            </div>
        </fieldset>
    </div>
    <div id="InvioInCorso" style="margin: 5px auto;">
        <div class="modalLoadingNoContainer">
            <div class="modalLoading">
                <asp:Label ID="lblProgressMessage" runat="server" Text="Invio Mail in corso..."></asp:Label>
                <p>
                    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/Images/loader.png" ToolTip="Invio Mail in corso..." />
                </p>
                <asp:Label ID="Label1" runat="server" Text="Controllarne l'avanzamento da apposita voce a menù..."></asp:Label>
            </div>
        </div>
    </div>
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
            <asp:Label ID="lblResultLotti" runat="server" Text="Non sono stati trovati risultati per la ricerca inserita" CssClass="Input_Label"></asp:Label>
            <Row:RibesGridView ID="rgvLotti" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="False" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
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
                    <asp:BoundField DataField="NDestinatari" HeaderText="N.Destinatari">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <headerstyle horizontalalign="Center" Width="30px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSel" runat="server" CssClass="Input_CheckBox_NoBorder" />
                            <asp:HiddenField ID="hfWarningRecipient" runat="server" Value='<%# Eval("WarningRecipient") %>' />
                            <asp:HiddenField ID="hfWarningSubject" runat="server" Value='<%# Eval("WarningSubject") %>' />
                            <asp:HiddenField ID="hfWarningMessage" runat="server" Value='<%# Eval("WarningMessage") %>' />
                            <asp:HiddenField ID="hfMailArchive" runat="server" Value='<%# Eval("MailArchive") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Row:RibesGridView>
        </fieldset>
    </div>
</asp:Content>
