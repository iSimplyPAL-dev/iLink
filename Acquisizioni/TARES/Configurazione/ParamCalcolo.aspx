<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgov.Master" AutoEventWireup="true" CodeBehind="ParamCalcolo.aspx.cs" Inherits="OPENgov.Acquisizioni.TARES.Configurazione.ParamCalcolo" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Row" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHeader" runat="server">
    <div id="divSelect">
        <asp:Button ID="cmdInsert" runat="server" Cssclass="Bottone BottoneNewInsert submitBtn" ToolTip="Inserisci nuovo" CausesValidation="False" onclick="CmdInsertClick"/>
    </div>
    <div id="divEdit">
        <asp:Button ID="cmdSave" runat="server" Cssclass="submitBtn BottoneSalva Bottone" ToolTip="Salva modifiche" CausesValidation="True" onclick="CmdSaveClick" ValidationGroup="SaveParamCalcolo"/>
        <asp:Button ID="cmdBack" runat="server" Cssclass="submitBtn BottoneAnnulla Bottone" ToolTip="Annulla" CausesValidation="False" onclick="CmdBackClick"/>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
    <div id="searchParamCalcolo" style="margin: 10px auto;">
        <fieldset class="classeFiledSetNoBorder">
            <legend class="Legend">Risultati della ricerca</legend>
            <Row:RibesGridView ID="rgvParamCalcolo" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                onpageindexchanging="RgvParamCalcoloPageIndexChanging" onrowcommand="RgvParamCalcoloRowCommand">
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
                    <asp:BoundField DataField="TypeCalcolo" HeaderText="Tipo Calcolo">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DescrTypeMQ" HeaderText="Tipo MQ">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Conferimenti">
                        <headerstyle horizontalalign="Center" Width="30px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkHasConferimenti" runat="server" CssClass="Input_CheckBox_NoBorder" Checked='<%# Eval("hasConferimenti") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Maggiorazione">
                        <headerstyle horizontalalign="Center" Width="30px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkHasMaggiorazione" runat="server" CssClass="Input_CheckBox_NoBorder" Checked='<%# Eval("hasMaggiorazione") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DescrTypeNCNonRes" HeaderText="NC Non Res.">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DescrTypeValiditaUpdateRes" HeaderText="Agg. Res.">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgEdit" runat="server" CssClass="BottoneGrd BottoneApriGrd" CausesValidation="False" CommandName="EditParamCalcolo" CommandArgument='<%# Eval("Anno") %>' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Modifica</span>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <itemtemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgDelete" runat="server" CssClass="BottoneGrd BottoneCancellaGrd" CausesValidation="False" CommandName="DeleteParamCalcolo" CommandArgument='<%# Eval("Anno") %>' alt="" OnClientClick="return confirm('Attenzione!\nStai per eliminare un anno:\nVuoi procedere?')"></asp:ImageButton> 
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
    <div id="editParamCalcolo" style="display:none;">
        <fieldset class="FiledSetRicerca">
            <legend class="Header_Label">Inserisci/Modifica</legend>
            <fieldset class="classeFiledSetNoBorder">
                <div style="float:left;margin-right:20px">
                    <asp:Label ID="lblYear" runat="server" Text="Anno" CssClass="Input_Label"></asp:Label>
                    <br/>
                    <asp:TextBox ID="txtYear" runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox>
                    <br/>
                    <asp:RequiredFieldValidator ID="rfvYear" runat="server" ErrorMessage="Inserisci l'anno" ControlToValidate="txtYear" ValidationGroup="SaveParamCalcolo"></asp:RequiredFieldValidator>
                </div>
                <div style="float:left;margin-right:20px">
                    <asp:Label ID="lblTipo" runat="server" Text="Tipo Calcolo" CssClass="Input_Label"></asp:Label>
                    <br/>
                    <Row:RibesDropDownList ID="rddlTipoCalcolo" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text hidden"></Row:RibesDropDownList>&nbsp;
                    <br/>
                    <asp:RequiredFieldValidator ID="rfvTipoCalcolo" runat="server" ErrorMessage="Seleziona una tipologia di Calcolo..." ControlToValidate="rddlTipoCalcolo" InitialValue="" ValidationGroup="SaveParamCalcolo"></asp:RequiredFieldValidator>
                </div>
                <div style="float:left;margin-right:20px">
                    <asp:CheckBox ID="chkPC" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Presenza Conferimenti" ToolTip="Conferimenti" />&nbsp;
                    <br />
                    <asp:CheckBox ID="chkPM" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Presenza Maggiorazioni" ToolTip="Maggiorazione" />&nbsp;
                    <br />
                </div>
                <div style="float:left;margin-right:20px">
                    <asp:Label ID="lblTipoMQ" runat="server" Text="Tipologia Superfici" CssClass="Input_Label"></asp:Label>
                    <br/>
                    <asp:RadioButton ID="optDic" runat="server" CssClass="Input_Label" GroupName="rbTypeMQ" Text="Dichiarate" Checked="true" />&nbsp;
                    <asp:RadioButton ID="optCat" runat="server" CssClass="Input_Label" GroupName="rbTypeMQ" Text="Catastali"/>&nbsp;
                    <br />
                </div>
            </fieldset>
            <fieldset class="classeFiledSetNoBorder">
                <div style="float:left;margin-right:20px">
                    <asp:Label ID="Label1" runat="server" Text="Periodicità acquisizione dati anagrafici per aggiornamento Numero Componenti" CssClass="Input_Label"></asp:Label>
                    <br/>
                    <asp:RadioButton ID="optGiornaliero" runat="server" CssClass="Input_Label" GroupName="rbTypeAggRes" Text="Giornaliero" Checked="true"/>&nbsp;
                    <asp:RadioButton ID="optMensile" runat="server" CssClass="Input_Label" GroupName="rbTypeAggRes" Text="Mensile"/>&nbsp;
                    <asp:RadioButton ID="optBimestrale" runat="server" CssClass="Input_Label" GroupName="rbTypeAggRes" Text="Bimestrale"/>&nbsp;
                    <asp:RadioButton ID="optAnnuale" runat="server" CssClass="Input_Label" GroupName="rbTypeAggRes" Text="Annuale"/>&nbsp;
                    <br />
                </div>
                <div style="float:left;margin-right:20px">
                    <asp:Label runat="server" Text="N. Componenti per non Residenti" CssClass="Input_Label"></asp:Label>
                    <br />
                    <asp:RadioButton ID="optNCForfait" runat="server" CssClass="Input_Label" AutoPostBack="true" GroupName="rbTypeNC" OnCheckedChanged="ChangeTypeNCSup" Text="NC a forfait" Checked="true"/>&nbsp;
                    <asp:RadioButton ID="optNCSup" runat="server" CssClass="Input_Label" AutoPostBack="true" GroupName="rbTypeNC" OnCheckedChanged="ChangeTypeNCSup" Text="NC in base a Superficie"/>&nbsp;
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtNCForfait" runat="server" CssClass="Input_Text_Numbers" Text="2" Width="50px" Columns="2"></asp:TextBox>&nbsp;
                    <Row:RibesGridView ID="rgvNCSup" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="10" HoverRowCssClass="riga_tabella_mouse_over"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        onpageindexchanging="RgvNCSupPageIndexChanging" onrowcommand="RgvNCSupRowCommand">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Sup. DA">
                                <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDa" runat="server" CssClass="Input_Text_Numbers" Text='<%# Eval("DA") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sup. A">
                                <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtA" runat="server" CssClass="Input_Text_Numbers" Text='<%# Eval("A") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NC">
                                <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNC" runat="server" CssClass="Input_Text_Numbers" Text='<%# Eval("NC") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <itemtemplate>
                                    <div class="tooltip">
                                        <asp:ImageButton id="imgDelete" runat="server" CssClass="BottoneGrd BottoneCestinoGrd" CausesValidation="False" CommandName="DeleteNCSup" CommandArgument='<%# Eval("Da") %>' alt="" OnClientClick="return confirm('Attenzione!\nStai per eliminare un range di superfici:\nVuoi procedere?')"></asp:ImageButton> 
                                        <span class="tooltiptext">Elimina</span>
                                    </div>
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center" Width="20px"></itemstyle>
                            </asp:TemplateField>
                        </Columns>
                    </Row:RibesGridView>
                </div>
            </fieldset>
            <br />
            <div>
                <asp:Label ID="lblEditMessage" runat="server" Text="" Visible="false" CssClass="ERRORSTYLE"></asp:Label>
            </div>
        </fieldset>
    </div>
</asp:Content>
