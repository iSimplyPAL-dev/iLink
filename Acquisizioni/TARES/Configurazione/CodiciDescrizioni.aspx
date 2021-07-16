<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgov.Master" AutoEventWireup="true" CodeBehind="CodiciDescrizioni.aspx.cs" Inherits="OPENgov.Acquisizioni.TARES.Configurazione.CodiciDescrizioni" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Row" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHeader" runat="server">
    <div id="divSelect">
        <asp:Button ID="cmdCopyCategories" runat="server" Cssclass="BottoneDuplica Bottone" ToolTip="Importa le Categorie TARES Ministeriali" CausesValidation="False" onclick="CmdCopyCategoriesClick"/>
        <asp:Button ID="cmdInsert" runat="server" Cssclass="BottoneNewInsert Bottone" ToolTip="Inserisci nuovo" CausesValidation="False" onclick="CmdInsertClick"/>
        <asp:Button ID="cmdSearch" runat="server" Cssclass="BottoneRicerca Bottone" ToolTip="Cerca..." CausesValidation="False" onclick="CmdSearchClick" />
    </div>
    <div id="divEdit">
        <asp:Button ID="cmdSave" runat="server" Cssclass="BottoneSalva Bottone" ToolTip="Salva modifiche" CausesValidation="True" onclick="CmdSaveClick" ValidationGroup="SaveCategory"/>
        <asp:Button ID="cmdBack" runat="server" Cssclass="BottoneAnnulla Bottone" ToolTip="Annulla" CausesValidation="False" onclick="CmdBackClick"/>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
    <div style="margin: 10px auto;">
        <fieldset class="FiledSetRicerca">
            <asp:RadioButton ID="optCategorie" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeCodDescr" OnCheckedChanged="ChangeTypeCodDescr" Text="Categorie" Checked="true" />&nbsp;
            <asp:RadioButton ID="optTipoConferimenti" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeCodDescr" OnCheckedChanged="ChangeTypeCodDescr" Text="Tipo Conferimenti"/>&nbsp;
            <asp:RadioButton ID="optCatConferimenti" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeCodDescr" OnCheckedChanged="ChangeTypeCodDescr" Text="Tariffe Conferimenti" Visible="false"/>&nbsp;
            <asp:RadioButton ID="optRiduzioni" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeCodDescr" OnCheckedChanged="ChangeTypeCodDescr" Text="Riduzioni"/>&nbsp;
            <asp:RadioButton ID="optEsenzioni" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeCodDescr" OnCheckedChanged="ChangeTypeCodDescr" Text="Esenzioni"/>&nbsp;
            <asp:RadioButton ID="optMaggiorazioni" runat="server" AutoPostBack="true" CssClass="Input_Label" GroupName="rbTypeCodDescr" OnCheckedChanged="ChangeTypeCodDescr" Text="Maggiorazioni"/>&nbsp;
            <br/>
            <asp:Label ID="lblMessage" runat="server" Text="" Visible="false" CssClass="ERRORSTYLE"></asp:Label>
        </fieldset>
    </div>
    <div id="searchCategoria" style="margin: 10px auto;">        
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Inserimento filtri di ricerca</legend>
            <asp:Label ID="lblAnagType" runat="server" Text="Categoria" CssClass="Input_Label"></asp:Label>
            <br/>
            <Row:RibesDropDownList ID="rddlCategorie" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>
        </fieldset>
        <fieldset class="classeFiledSetNoBorder">
            <legend class="Legend"><asp:Label ID="lblDatiComune" runat="server">Risultati della ricerca</asp:Label></legend>
            <Row:RibesGridView ID="rgvCategorie" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                onpageindexchanging="RgvCategoriePageIndexChanging" onrowcommand="RgvCategorieRowCommand">
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
                    <asp:BoundField DataField="Definizione" HeaderText="Descrizione">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgEdit" runat="server" CssClass="BottoneGrd BottoneApriGrd" CausesValidation="False" CommandName="EditATECO" CommandArgument='<%# Eval("IdCategoriaAteco") %>' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Modifica</span>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgDelete" runat="server" CssClass="BottoneGrd BottoneCancellaGrd" CausesValidation="False" CommandName="DeleteATECO" CommandArgument='<%# Eval("IdCategoriaAteco") %>' alt="" OnClientClick="return confirm(''Attenzione!\nStai per eliminare una categoria.\nVuoi procedere?')"></asp:ImageButton> 
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
    <div id="searchRidEse" style="margin: 10px auto;">
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
                    <asp:BoundField DataField="Codice" HeaderText="Codice">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Definizione" HeaderText="Descrizione">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <asp:ImageButton id="imgEdit" runat="server" BorderWidth="0px" BorderStyle="None" 
                                ImageUrl="~/Images/Bottoni/apri1.png" CausesValidation="False" CommandName="EditRidEse"
                                CommandArgument='<%# Eval("Codice") %>' Height="20px" ToolTip="Modifica..."></asp:ImageButton> 
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgDelete" runat="server" CssClass="BottoneGrd BottoneCancellaGrd" CausesValidation="False" CommandName="DeleteRidEse" CommandArgument='<%# Eval("Codice") %>' alt="" OnClientClick="return confirm('Attenzione!\nStai per eliminare una Configurazione.\nVuoi procedere?')"></asp:ImageButton> 
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
    <div id="editCodDescr" style="display:none;">
        <fieldset class="FiledSetRicerca">
            <legend class="Header_Label">Inserisci/Modifica</legend>
            <asp:HiddenField ID="hfIdCategoriaAteco" runat="server" Value="0" />
            <div style="float:left;margin-right:20px">
                <asp:Label ID="lblCodice" runat="server" Text="Codice" CssClass="Input_Label"></asp:Label>
                <br/>
                <asp:TextBox ID="txtCodice" runat="server" CssClass="Input_Text"></asp:TextBox>
                <br/>
                <asp:RequiredFieldValidator ID="rfvCodice" runat="server" ErrorMessage="Inserisci il codice" ControlToValidate="txtCodice" ValidationGroup="SaveCategory"></asp:RequiredFieldValidator>
            </div>
            <div style="float:left;margin-right:20px">
                <asp:Label ID="lblDescrizione" runat="server" Text="Descrizione" CssClass="Input_Label"></asp:Label>
                <br/>
                <asp:TextBox ID="txtDescrizione" runat="server" TextMode="MultiLine" CssClass="Input_Text" Columns="80"></asp:TextBox>
                <br/>
                <asp:RequiredFieldValidator ID="rfvDescrizione" runat="server" ErrorMessage="Inserisci la descrizione" ControlToValidate="txtDescrizione" ValidationGroup="SaveCategory"></asp:RequiredFieldValidator>
            </div>
            <div>
                <asp:Label ID="lblDomestica" runat="server" Text="Domestica" CssClass="Input_Label"></asp:Label>
                <br/>
                <asp:CheckBox ID="chkDomestica" runat="server" />
            </div>
            <div style="float:left;margin-right:20px">
                <asp:Label ID="lblEditMessage" runat="server" Text="" Visible="false" CssClass="ERRORSTYLE"></asp:Label>
            </div>
        </fieldset>
    </div>
</asp:Content>
