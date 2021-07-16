<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgov.Master" AutoEventWireup="true" CodeBehind="AnagrafeResidenti.aspx.cs" Inherits="OPENgov.Acquisizioni.AnagrafeResidenti" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Row" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="ContentHeader" runat="server">
    <div class="tooltip">
        <asp:Button ID="cmdResFromTributi" runat="server" Cssclass="BottoneForzaDati Bottone" OnClick="cmdResFromTributiClick"/>
        <span class="tooltiptext">Importa Residenti dalla banca dati tributaria</span>
    </div>
    <div class="tooltip">
        <asp:Button ID="cmdImports" runat="server" Cssclass="Bottone BottoneImport" OnClick="BtnUpdateClick" ValidationGroup="UploadValidation"/>
        <span class="tooltiptext">Importa flussi</span>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBody" runat="server">
    <div style="margin: 0 auto;">
        <fieldset class="classeFiledSetRicerca">
            <legend class="Legend">Importa Dati Esterni</legend>
            <asp:Label ID="lblAnagType" runat="server" Text="Tipologia" CssClass="Input_Label"></asp:Label>
            <br/>
            <Row:RibesDropDownList ID="cmbFileType" CssClass="Input_CheckBox_NoBorder" runat="server" DefaultFirstText="..." />
            <asp:RequiredFieldValidator ID="rfvFileType" runat="server" ControlToValidate="cmbFileType" InitialValue="..." ErrorMessage="Scegli la tipologia di file da acquisire"  ValidationGroup="UploadValidation"></asp:RequiredFieldValidator>
            <br/>
            <asp:Label ID="lblUploadFile" runat="server" Text="File" CssClass="Input_Label"></asp:Label>
            <br/>
            <asp:FileUpload ID="fileUpload" runat="server" CssClass="Input_Label"/>
            <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" CssClass="Input_Label" ControlToValidate="fileUpload" ErrorMessage="Seleziona il file da importare" ValidationGroup="UploadValidation"></asp:RequiredFieldValidator>
            <br/>
            <asp:Label ID="lblMessage" runat="server" Text="" Visible="false" CssClass="ERRORSTYLE"></asp:Label>
        </fieldset>
        <fieldset class="classeFiledSetNoBorder">
            <legend class="Legend">Elenco Dati Esterni</legend>
            <Row:RibesGridView ID="gvAnagFiles" runat="server" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
            AutoGenerateColumns="False" AllowPaging="True" PageSize="20"
            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" 
                onrowcommand="GvAnagFilesRowCommand" onpageindexchanging="GvAnagFilesPageIndexChanging" >
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                <Columns>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgExtension" runat="server" CssClass="BottoneGrd BottoneDownloadGrd" CausesValidation="False" CommandName="download" CommandArgument='<%# Eval("IdAnagFile") %>' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Download file</span>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgLog" runat="server" CssClass="BottoneGrd BottoneRicercaGrd" CausesValidation="False" CommandName="ViewLog" CommandArgument='<%# Eval("IdAnagFile") %>' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Log file</span>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="AnagFileType" HeaderText="Tipo">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="FileName" HeaderText="File">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="InsertDateTime" HeaderText="Data inserimento">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ReadDateTime" HeaderText="Data lettura" NullDisplayText="-">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="AnagFileLog" HeaderText="Esito" NullDisplayText="-">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:BoundField>
                </Columns>
            </Row:RibesGridView>
            <br/>
            <asp:DataList ID="dlLog" runat="server" CellPadding="4" ForeColor="#333333" 
                Width="100%" onitemdatabound="DlLogItemDataBound">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <AlternatingItemStyle BackColor="White" ForeColor="#284775" />
                <ItemStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <ItemTemplate>
                    <asp:Image ID="imgSeverity" runat="server" />
                    &nbsp;<asp:Label ID="lblRow" runat="server" Text=""></asp:Label>
                    &nbsp;<asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                </ItemTemplate>
            </asp:DataList>
        </fieldset>
    </div>
</asp:Content>
