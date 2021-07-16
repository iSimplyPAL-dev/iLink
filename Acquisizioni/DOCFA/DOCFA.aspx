<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgov.Master" AutoEventWireup="true" CodeBehind="DOCFA.aspx.cs" Inherits="OPENgov.Acquisizioni.DOCFAGestione.DOCFAGest" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Row" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHeader" runat="server">
    <div id="ComandiRicerca">
        <asp:Button ID="cmdSearch" runat="server" Cssclass="BottoneRicerca Bottone" ToolTip="Cerca..." CausesValidation="False" onclick="CmdSearchClick" />
        <asp:Button style="display:none" ID="cmdBack" runat="server" Cssclass="BottoneAnnulla Bottone" ToolTip="Torna alla ricerca..." CausesValidation="False" onclick="CmdBackClick" />
    </div>
    <div id="ComandiDettaglio" style="display:none;">
        <asp:Button ID="cmdBackDet" runat="server" Cssclass="BottoneAnnulla Bottone" ToolTip="Torna alla ricerca..." CausesValidation="False" onclick="CmdBackClick" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
    <div id="RicercaDOCFA" style="margin:10px auto;">
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Parametri di ricerca</legend>
            <div id="Div1">
                <table>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblProtocollo" runat="server" Text="Protocollo" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <asp:TextBox ID="txtProtocollo" runat="server" TextMode="SingleLine" CssClass="Input_Text" Columns="9"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblDataRegistrazione" runat="server" Text="Data Registrazione" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <asp:TextBox ID="txtDataRegistrazione" runat="server" TextMode="SingleLine" CssClass="Input_Text" Columns="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Foglio" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <asp:TextBox ID="txtFoglioDOCFA" runat="server" TextMode="SingleLine" CssClass="Input_Text" Columns="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Numero" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <asp:TextBox ID="txtNumeroDOCFA" runat="server" TextMode="SingleLine" CssClass="Input_Text" Columns="4"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Subalterno" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <asp:TextBox ID="txtSubDOCFA" runat="server" TextMode="SingleLine" CssClass="Input_Text" Columns="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Ubicazione" CssClass="Input_Label"></asp:Label>
                            <br/>
                            <asp:TextBox ID="txtUbicazioneDOCFA" runat="server" TextMode="SingleLine" CssClass="Input_Text" Columns="80"></asp:TextBox>
                        </td>
                    </tr>
                </table>                
            </div>
        </fieldset>
        <fieldset class="classeFiledSetNoBorder">
            <legend class="Legend">Risultati della ricerca</legend>
            <asp:Label ID="lblResultDOCFA" runat="server" Text="Non sono stati trovati risultati per la ricerca inserita" CssClass="Input_Label"></asp:Label>
            <Row:RibesGridView ID="rgvDOCFA" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="15" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" OnRowCommand="rgvDOCFARowCommand" OnPageIndexChanging="rgvDOCFAPageIndexChanging">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="Protocollo" HeaderText="Protocollo">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DataRegistrazione" HeaderText="Data Registrazione">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Foglio" HeaderText="Foglio">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Numero" HeaderText="Numero">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Subalterno" HeaderText="Subalterno">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Indirizzo" HeaderText="Ubicazione">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgView" runat="server" CssClass="BottoneGrd BottoneApriGrd" CausesValidation="False" CommandName="ViewDOCFA" CommandArgument='<%# Eval("IdDOCFA") %>' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Dettaglio</span>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgEdit" runat="server" CssClass="BottoneGrd BottoneSalvaGrd" CausesValidation="False" CommandName="EditDOCFA" CommandArgument='<%# Eval("IdDOCFA") %>' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Gestione</span>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                </Columns>
            </Row:RibesGridView>
        </fieldset>
    </div>
    <div id="DettaglioDOCFA" class="hidden">
        <div class="modal-content">
            <div class="modal-header">
                <span class="closebtnalert" onclick="GestDOCFA('DettaglioDOCFA')">&times;</span>
                <h2>Dati del documento</h2>
            </div>
            <p>&nbsp;<asp:Label ID="lblDocumento" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
            <p>&nbsp;<asp:Label ID="lblRifCat" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
            <p>&nbsp;<asp:Label ID="lblIndirizzo" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
            <p>&nbsp;<asp:Label ID="lblClassamento" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
        </div>
        <asp:datalist id="dlFiles" runat="server" RepeatColumns="3">
			<ItemTemplate>
				<table>
					<tr>
						<td valign="middle" align="right" width="100px">                            
							&nbsp;<img alt="" height="100px" width="100px" src='<%# Path_IMAGE(DataBinder.Eval(Container.DataItem, "nomefile")) %>'>
						</td>
						<td valign="bottom" width="200px">
						    <a target="_blank" href="<%# Path_DOC(DataBinder.Eval(Container.DataItem, "nomefile")) %>"><%# DataBinder.Eval(Container.DataItem, "nomefile")%></a>
                        </td>
					</tr>
				</table>
			</ItemTemplate>
		</asp:datalist>
    </div>
    <div id="RicercaDich" style="margin:10px auto;display:none;">
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Parametri di ricerca</legend>
            <div id="RicercaParam">
                <table>
                    <tr>
                        <td colspan="4">
                            <asp:RadioButton ID="rbICI" runat="server" GroupName="rbTributo" CssClass="Input_Label" Text="ICI/IMU" Checked="true"/>&nbsp;
                            <asp:RadioButton ID="rbTARSU" runat="server" GroupName="rbTributo" CssClass="Input_Label" Text="TARSU/TARES"/>&nbsp;
                       </td>
                    </tr>
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
        </fieldset>
        <fieldset class="classeFiledSetNoBorder">
            <legend class="Legend">Risultati della ricerca</legend>
            <asp:Label ID="lblResultDich" runat="server" Text="Non sono stati trovati risultati per la ricerca inserita" CssClass="Input_Label"></asp:Label>
            <Row:RibesGridView ID="rgvDich" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="15" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" OnRowCommand="rgvDichRowCommand" OnPageIndexChanging="rgvDichPageIndexChanging">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="Foglio" HeaderText="Foglio">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Numero" HeaderText="Numero">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Subalterno" HeaderText="Subalterno">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Ubicazione" HeaderText="Ubicazione">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DescrTributo" HeaderText="Tributo Dich.">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Nominativo" HeaderText="Nominativo Dich.">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA Dich.">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DataInizio" HeaderText="Data Inizio Dich.">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DataFine" HeaderText="Data Fine Dich.">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Classamento" HeaderText="Classamento Dich.">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgEditAll" runat="server" CssClass="BottoneGrd BottoneAssociaGrd" CausesValidation="False" CommandName="EditDichAll" CommandArgument='-1' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Abbina a tutti gli elementi della griglia</span>
                            </div>
                        </HeaderTemplate>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgEdit" runat="server" CssClass="BottoneGrd BottoneAssociaGrd" CausesValidation="False" CommandName="EditDich" CommandArgument='<%# Eval("FKIdDich") %>' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Abbina</span>
                            </div>
                            <asp:HiddenField ID="hfFKIdDich" runat="server" Value='<%# Eval("FKIdDich") %>' />
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodTributo" HeaderText="CodTributo" Visible="False">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                </Columns>
            </Row:RibesGridView>
        </fieldset>
        <asp:HiddenField ID="hfOrigineChiamata" runat="server" Value="" />
        <asp:HiddenField ID="hfIdDOCFA" runat="server" Value="0" />
    </div>
</asp:Content>
