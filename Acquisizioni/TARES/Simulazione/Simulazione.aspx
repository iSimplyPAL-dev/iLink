<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgov.Master" AutoEventWireup="true" CodeBehind="Simulazione.aspx.cs" Inherits="OPENgov.Acquisizioni.TARES.Simulazione.Simulazione" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Row" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHeader" runat="server">
    <div id="divSearch">
        <asp:Button ID="cmdPrint" runat="server" Cssclass="BottoneExcel Bottone" ToolTip="Stampa" CausesValidation="False" onclick="CmdPrintClick"/>
        <asp:Button ID="cmdDelete" runat="server" Cssclass="BottoneCancella Bottone" ToolTip="Elimina" CausesValidation="True" onclick="CmdDeleteClick" />
        <asp:Button ID="cmdSave" runat="server" Cssclass="BottoneSalva Bottone" ToolTip="Salva" CausesValidation="True" onclick="CmdSaveClick" />
        <asp:Button ID="cmdCalc" runat="server" Cssclass="BottoneConvert Bottone" ToolTip="Calcola" CausesValidation="False" onclick="CmdCalcoloClick"/>
        <asp:Button ID="cmdAnalyze" runat="server" Cssclass="BottoneUtilities Bottone" ToolTip="Estrai elenco immobili" CausesValidation="True" onclick="CmdAnalyzeClick" />
        <asp:Button ID="cmdSearch" runat="server" Cssclass="BottoneRicerca Bottone" ToolTip="Cerca" CausesValidation="True" onclick="CmdSearchClick" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
    <div id="ParamSearch" style="margin: 0 auto;">
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Parametri di Calcolo</legend>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblYear" runat="server" Text="Anno" CssClass="Input_Label"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTipo" runat="server" Text="Tipo Calcolo" CssClass="Input_Label hidden"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTipoMQ" runat="server" Text="Tipologia Superfici" CssClass="Input_Label"></asp:Label>
                    </td>
                    <td rowspan="2">
                        <a title="KA da usare" onclick="nascondi(this,'KA','KA')" href="#" class="Legend">Visualizza KA</a>
                        <br />
                        <a title="KB da usare" onclick="nascondi(this,'KB','KB')" href="#" class="Legend">Visualizza KB</a>
                    </td>
                    <td rowspan="2">
                        <a title="KC da usare" onclick="nascondi(this,'KC','KC')" href="#" class="Legend">Visualizza KC</a>
                        <br />
                        <a title="KD da usare" onclick="nascondi(this,'KD','KD')" href="#" class="Legend">Visualizza KD</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="Input_Text" Width="80px"></asp:TextBox>
                    </td>
                    <td>
                        <Row:RibesDropDownList ID="rddlTipoCalcolo" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text hidden"></Row:RibesDropDownList>
                    </td>
                    <td>
                        <asp:RadioButton ID="optDic" runat="server" CssClass="Input_Label" GroupName="rbTypeMQ" Text="Dichiarate" Checked="true" />&nbsp;
                        <asp:RadioButton ID="optCat" runat="server" CssClass="Input_Label" GroupName="rbTypeMQ" Text="Catastali"/>&nbsp;
                    </td>
                </tr>
            </table>
            <div id="KA" style="margin: 0 auto;">
                <table>
                    <tr>
                        <td style="width:100%">
                            <Row:RibesGridView ID="rgvKA" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" HoverRowCssClass="riga_tabella_mouse_over"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                onpageindexchanging="rgvKAPageIndexChanging">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="DescrCategoria" HeaderText="Categoria">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Justify"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NC" HeaderText="Componenti">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nord" HeaderText="Nord">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Centro" HeaderText="Centro">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Sud" HeaderText="Sud">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Valido">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtValore" runat="server" CssClass="Input_Text_Numbers" Text='<%# Eval("CoefficienteUsato") %>'></asp:TextBox>
                                            <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("IdUsato") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Row:RibesGridView>
                        </td>
                        <td valign="top" style="width:35px">
                            <asp:Button ID="CmdSaveKA" runat="server" Cssclass="Bottone BottoneSalva Bottone" ToolTip="Salva K" CausesValidation="True" onclick="CmdSaveKAClick" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="KB" style="margin: 0 auto;">
                <table>
                    <tr>
                        <td style="width:100%">
                            <Row:RibesGridView ID="rgvKB" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" HoverRowCssClass="riga_tabella_mouse_over"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                onpageindexchanging="rgvKBPageIndexChanging">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="DescrCategoria" HeaderText="Categoria">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Justify"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NC" HeaderText="Componenti">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Minimo" HeaderText="Minimo">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Medio" HeaderText="Medio">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Massimo" HeaderText="Massimo">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Valido">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtValore" runat="server" CssClass="Input_Text_Numbers" Text='<%# Eval("CoefficienteUsato") %>'></asp:TextBox>
                                            <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("IdUsato") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Row:RibesGridView>
                        </td>
                        <td valign="top" style="width:35px">
                            <asp:Button ID="CmdSaveKB" runat="server" Cssclass="Bottone BottoneSalva Bottone" ToolTip="Salva K" CausesValidation="True" onclick="CmdSaveKBClick" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="KC" style="margin: 0 auto;">
                <table>
                    <tr>
                        <td style="width:100%">
                            <Row:RibesGridView ID="rgvKC" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="True" PageSize="31" HoverRowCssClass="riga_tabella_mouse_over"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                onpageindexchanging="rgvKCPageIndexChanging">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="DescrCategoria" HeaderText="Categoria">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Justify"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NordMin" HeaderText="Nord Min">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NordMax" HeaderText="Nord Max">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CentroMin" HeaderText="Centro Min">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CentroMax" HeaderText="Centro Max">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SudMin" HeaderText="Sud Min">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SudMax" HeaderText="Sud Max">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Valido">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtValore" runat="server" CssClass="Input_Text_Numbers" Text='<%# Eval("CoefficienteUsato") %>'></asp:TextBox>
                                            <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("IdUsato") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Row:RibesGridView>
                        </td>
                        <td valign="top" style="width:35px">
                            <asp:Button ID="CmdSaveKC" runat="server" Cssclass="Bottone BottoneSalva Bottone" ToolTip="Salva K" CausesValidation="True" onclick="CmdSaveKCClick" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="KD" style="margin: 0 auto;">
                <table>
                    <tr>
                        <td style="width:100%">
                            <Row:RibesGridView ID="rgvKD" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="True" PageSize="31" HoverRowCssClass="riga_tabella_mouse_over"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                onpageindexchanging="rgvKDPageIndexChanging">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="DescrCategoria" HeaderText="Categoria">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Justify"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NordMin" HeaderText="Nord Min">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NordMax" HeaderText="Nord Max">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CentroMin" HeaderText="Centro Min">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CentroMax" HeaderText="Centro Max">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SudMin" HeaderText="Sud Min">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SudMax" HeaderText="Sud Max">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Valido">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right"></itemstyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtValore" runat="server" CssClass="Input_Text_Numbers" Text='<%# Eval("CoefficienteUsato") %>'></asp:TextBox>
                                            <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("IdUsato") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Row:RibesGridView>
                        </td>
                        <td valign="top" style="width:35px">
                            <asp:Button ID="CmdSaveKD" runat="server" Cssclass="Bottone BottoneSalva Bottone" ToolTip="Salva K" CausesValidation="True" onclick="CmdSaveKDClick" />
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
        <fieldset class="FiledSetRicerca">                
            <legend class="Legend">Dati PEF</legend>
            <table>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label runat="server" CssClass="Input_Label">N.Utenze</asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" CssClass="Input_Label">% Utenze</asp:Label>
                    </td>
                    <td></td>
                    <td>
                        <asp:Label runat="server" CssClass="Input_Label">MQ</asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" CssClass="Input_Label">% MQ</asp:Label>
                    </td>
                    <td></td>
                    <td>
                        <asp:Label runat="server" CssClass="Input_Label">Rifiuti prodotti (Kg)</asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" CssClass="Input_Label">% KG</asp:Label>
                    </td>
                    <td></td>
                    <td align="center">
                        <asp:Label runat="server" CssClass="Input_Label">Costi Parte Fissa</asp:Label>
                    </td>
                    <td></td>
                    <td align="center">
                        <asp:Label runat="server" CssClass="Input_Label">Costi Parte Variabile</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" CssClass="Input_Label">Totali</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtNUtenzeTot" CssClass="Input_Text_Right" runat="server" Width="60px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtPercentUtenzeTot" CssClass="Input_Text_Right" runat="server" Width="60px"></asp:TextBox>
                    </td>
                    <td rowspan="4" valign="middle">
                        <div class="VerticalLine" style="height:80px;"></div>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtMQTot" CssClass="Input_Text_Right" runat="server" Width="90px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtPercentMQTot" CssClass="Input_Text_Right" runat="server" Width="60px" Enabled="false"></asp:TextBox>
                    </td>
                    <td rowspan="4" valign="middle">
                        <div class="VerticalLine" style="height:80px;"></div>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtKgTot" CssClass="Input_Text_Right" runat="server" AutoPostBack="true" OnTextChanged="txtTotCalc"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtPercentKGTot" CssClass="Input_Text_Right" runat="server" Width="60px"></asp:TextBox>
                    </td>
                    <td rowspan="4" valign="middle">
                        <div class="VerticalLine" style="height:80px;"></div>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="TxtCostiPFTot" CssClass="Input_Text_Right" runat="server" AutoPostBack="true" OnTextChanged="txtTotCalc"></asp:TextBox>
                    </td>
                    <td rowspan="4" valign="middle">
                        <div class="VerticalLine" style="height:80px;"></div>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="TxtCostiPVTot" CssClass="Input_Text_Right" runat="server" AutoPostBack="true" OnTextChanged="txtTotCalc"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="14">
                        <asp:Label runat="server" CssClass="lstTabRow" Width="100%" Height="1px">&nbsp</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" CssClass="Input_Label">Domestiche</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtNUtenzeDom" CssClass="Input_Text_Right" runat="server" Width="60px" AutoPostBack="true" OnTextChanged="txtTotCalc"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtPercentUtenzeDom" CssClass="Input_Text_Right" runat="server" Width="60px" AutoPostBack="true" OnTextChanged="txtTotCalc"></asp:TextBox>
                    </td>  
                    <td>
                        <asp:TextBox ID="TxtMQDom" CssClass="Input_Text_Right" runat="server" Width="90px" AutoPostBack="true" OnTextChanged="txtTotCalc"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtPercentMQDom" CssClass="Input_Text_Right" runat="server" Width="60px" Enabled="false"></asp:TextBox>
                    </td>  
                    <td>
                        <asp:TextBox ID="TxtKgDom" CssClass="Input_Text_Right" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtPercentKGDom" CssClass="Input_Text_Right" runat="server" Width="60px" AutoPostBack="true" OnTextChanged="txtTotCalc"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtCostiPFDom" CssClass="Input_Text_Right" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtCostiPVDom" CssClass="Input_Text_Right" runat="server"></asp:TextBox>
                    </td> 
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" CssClass="Input_Label">Non Domestiche</asp:Label>
                    </td>                 
                    <td>
                        <asp:TextBox ID="TxtNUtenzeNonDom" CssClass="Input_Text_Right" runat="server" Width="60px" AutoPostBack="true" OnTextChanged="txtTotCalc"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtPercentUtenzeNonDom" CssClass="Input_Text_Right" runat="server" Width="60px" AutoPostBack="true" OnTextChanged="txtTotCalc"></asp:TextBox>
                    </td>  
                    <td>
                        <asp:TextBox ID="TxtMQNonDom" CssClass="Input_Text_Right" runat="server" Width="90px" AutoPostBack="true" OnTextChanged="txtTotCalc"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtPercentMQNonDom" CssClass="Input_Text_Right" runat="server" Width="60px" Enabled="false"></asp:TextBox>
                    </td>  
                    <td>
                        <asp:TextBox ID="TxtKgNonDom" CssClass="Input_Text_Right" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtPercentKGNonDom" CssClass="Input_Text_Right" runat="server" Width="60px" AutoPostBack="true" OnTextChanged="txtTotCalc"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtCostiPFNonDom" CssClass="Input_Text_Right" runat="server"></asp:TextBox>
                    </td> 
                    <td>
                        <asp:TextBox ID="TxtCostiPVNonDom" CssClass="Input_Text_Right" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div id="ElabProgress" style="width:350px; margin: 0 auto;">
        <div class="modalLoadingContainer">
            <div class="modalLoading">
                <asp:Label ID="lblProgressMessage" runat="server" Text="Elaborazione in corso..."></asp:Label>
                <p>
                    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/Images/loader.png" ToolTip="Caricamento in corso..." />
                </p>
                <asp:Label ID="Label1" runat="server" Text="Attendere Prego..."></asp:Label><br />
                <asp:Label ID="LblAvanzamento" runat="server"></asp:Label>
            </div>
        </div>
    </div>
	<a title="Visualizza Posizioni" onclick="nascondi(this,'AggMassivo','Posizioni')" href="#" class="Legend">Visualizza Posizioni</a>
	<br/>
    <div id="AggMassivo" style="margin: 0 auto;">
		<iframe id="frAggMassivo" name="frAggMassivo" frameborder="0" width="100%" height="600px" runat="server" src="../AggMassivo/AggMassivo.aspx"></iframe>
    </div>
	<a title="Visualizza Totalizzatori" onclick="nascondi(this,'SimulaTotali','Totalizzatori')" href="#" class="Legend">Visualizza Totalizzatori</a>
	<br/>
    <div id="SimulaTotali" style="margin: 0 auto;">
        <Row:RibesGridView ID="rgvTotali" runat="server" BorderStyle="None" 
            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
            AutoGenerateColumns="False" AllowPaging="True" PageSize="10" HoverRowCssClass="riga_tabella_mouse_over"
            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
            onpageindexchanging="rgvTotaliPageIndexChanging">
            <PagerSettings Position="Bottom"></PagerSettings>
            <PagerStyle CssClass="CartListFooter" />
            <RowStyle CssClass="CartListItem"></RowStyle>
            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
            <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
            <Columns>
                <asp:BoundField DataField="DescrCategoria" HeaderText="Categoria">
                    <headerstyle horizontalalign="Center"></headerstyle>
                    <itemstyle horizontalalign="Justify"></itemstyle>
                </asp:BoundField>
                <asp:BoundField DataField="nComponentiPF" HeaderText="Componenti PF">
                    <headerstyle horizontalalign="Center"></headerstyle>
                    <itemstyle horizontalalign="Right"></itemstyle>
                </asp:BoundField>
                <asp:BoundField DataField="nComponentiPV" HeaderText="Componenti PV">
                    <headerstyle horizontalalign="Center"></headerstyle>
                    <itemstyle horizontalalign="Right"></itemstyle>
                </asp:BoundField>
                <asp:BoundField DataField="DescrRiduzione" HeaderText="Riduzione">
                    <headerstyle horizontalalign="Center"></headerstyle>
                    <itemstyle horizontalalign="Justify"></itemstyle>
                </asp:BoundField>
                <asp:BoundField DataField="DescrDetassazione" HeaderText="Detassazione">
                    <headerstyle horizontalalign="Center"></headerstyle>
                    <itemstyle horizontalalign="Justify"></itemstyle>
                </asp:BoundField>
                <asp:BoundField DataField="nUtenze" HeaderText="N.Utenti">
                    <headerstyle horizontalalign="Center"></headerstyle>
                    <itemstyle horizontalalign="Right"></itemstyle>
                </asp:BoundField>
                <asp:BoundField DataField="nMQ" HeaderText="MQ">
                    <headerstyle horizontalalign="Center"></headerstyle>
                    <itemstyle horizontalalign="Right"></itemstyle>
                </asp:BoundField>
                <asp:BoundField DataField="UtenzeUtili" HeaderText="Utenze Nette">
                    <headerstyle horizontalalign="Center"></headerstyle>
                    <itemstyle horizontalalign="Right"></itemstyle>
                </asp:BoundField>
                <asp:BoundField DataField="MQUtiliPF" HeaderText="MQ Netti Fissa">
                    <headerstyle horizontalalign="Center"></headerstyle>
                    <itemstyle horizontalalign="Right"></itemstyle>
                </asp:BoundField>
                <asp:BoundField DataField="MQUtiliPV" HeaderText="MQ Netti Variabile">
                    <headerstyle horizontalalign="Center"></headerstyle>
                    <itemstyle horizontalalign="Right"></itemstyle>
                </asp:BoundField>
            </Columns>
        </Row:RibesGridView>
    </div>
	<a title="Visualizza Tariffe" onclick="nascondi(this,'SimulaTariffe','Tariffe')" href="#" class="Legend" id="aVisTariffe">Visualizza Tariffe</a>
	<br/>
    <div id="SimulaTariffe" style="margin: 0 auto;">
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" CssClass="Legend">Parte Fissa</asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" CssClass="Legend">Parte Variabile</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <Row:RibesGridView ID="rgvPFDOM" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="7" HoverRowCssClass="riga_tabella_mouse_over"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        onpageindexchanging="rgvTariffePageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="Descrizione" HeaderText="Categoria (NC)">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="MQ" HeaderText="MQ">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CoeffK" HeaderText="Coefficiente">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="MQNormalizzati" HeaderText="MQ Normalizzati">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Tariffa" HeaderText="Tariffa">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                         </Columns>
                    </Row:RibesGridView>
                </td>
                <td>
                    <Row:RibesGridView ID="rgvPVDOM" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="7" HoverRowCssClass="riga_tabella_mouse_over"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        onpageindexchanging="rgvTariffePageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="Descrizione" HeaderText="Categoria (NC)">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="MQ" HeaderText="Utenti">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CoeffK" HeaderText="Coefficiente">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="MQNormalizzati" HeaderText="Utenti Normalizzati">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Tariffa" HeaderText="Tariffa">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                         </Columns>
                    </Row:RibesGridView>
                </td>
            </tr>
            <tr>
                <td>
                    <Row:RibesGridView ID="rgvPFNONDOM" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="31" HoverRowCssClass="riga_tabella_mouse_over"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        onpageindexchanging="rgvTariffePageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="Descrizione" HeaderText="Categoria">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="MQ" HeaderText="MQ">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CoeffK" HeaderText="Coefficiente">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="MQNormalizzati" HeaderText="MQ Normalizzati">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Tariffa" HeaderText="Tariffa">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                         </Columns>
                    </Row:RibesGridView>
                </td>
                <td>
                    <Row:RibesGridView ID="rgvPVNONDOM" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="31" HoverRowCssClass="riga_tabella_mouse_over"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        onpageindexchanging="rgvTariffePageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="Descrizione" HeaderText="Categoria">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="MQ" HeaderText="MQ">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CoeffK" HeaderText="Coefficiente">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="MQNormalizzati" HeaderText="MQ Normalizzati">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Tariffa" HeaderText="Tariffa">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                         </Columns>
                    </Row:RibesGridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" CssClass="Legend">Tariffe Determinate</asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <Row:RibesGridView ID="rgvTariffe" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="50" HoverRowCssClass="riga_tabella_mouse_over"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        onpageindexchanging="rgvTariffePageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="Descrizione" HeaderText="Categoria (NC)">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="impPF" HeaderText="Tariffa PF">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="impPV" HeaderText="Tariffa PV">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                         </Columns>
                    </Row:RibesGridView>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hfIdSimulazione" runat="server"/>    
</asp:Content>
