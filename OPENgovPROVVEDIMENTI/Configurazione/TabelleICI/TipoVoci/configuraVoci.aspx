<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="configuraVoci.aspx.vb" Inherits="Provvedimenti.configuraVoci" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>configuraVoci</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">

        function AbilitaPulsanti() {
            parent.parent.Comandi.document.getElementById('Salva').disabled = false
            parent.parent.Comandi.document.getElementById('Elimina').disabled = false
        }
        function Save() {
            document.getElementById("btnSalva").click()
        }
        function Delete() {
            if (confirm('Confermi l\'eliminazione della voce selezionata?')) {
                document.getElementById("btnCancella").click()
            }
        }
        function Clear() {
            document.getElementById("btnClear").click()
        }
        function Back() {
            document.getElementById("btnBack").click()
        }
    </script>
</head>
<body class="SfondoVisualizza">
    <form id="Form1" runat="server" method="post">
        <fieldset class="classeFiledSetIframe">
            <legend class="Legend">Visualizzazione Voci</legend>
            <table border="0" cellspacing="0" cellpadding="2" width="100%">
                <tr>
                    <td width="40%">
                        <label class="NormalBold">
                            Voce:</label>
                        <asp:Label ID="lblVoce" runat="server" CssClass="Input_Label">Label</asp:Label></td>
                    <td width="60%">
                        <label class="NormalBold">
                            Tipo Provvedimento:</label>
                        <asp:Label ID="lblTipoProvvedimento" runat="server" CssClass="Input_Label">Label</asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <label class="NormalBold">
                            Misura:</label>
                        <asp:Label ID="lblMisura" runat="server" CssClass="Input_Label">Label</asp:Label></td>
                    <td></td>
                </tr>
            </table>
        </fieldset>
        <fieldset class="classeFiledSetIframe">
            <legend class="Legend">Visualizzazione Voci</legend>
        </fieldset>
        <!--<iframe class="SfondoVisualizza" id="ifrmVoci" name="ifrmVoci" src="../../../../Generali/asp/aspVuota.aspx"
				frameBorder="0" width="100%" height="270"></iframe>-->
        <asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" Visible="False">lblMessage</asp:Label>
        <Grd:RibesGridView ID="GrdVoci" runat="server" BorderStyle="None"
            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
            OnRowCommand="GrdRowCommand">
            <PagerSettings Position="Bottom"></PagerSettings>
            <PagerStyle CssClass="CartListFooter" />
            <RowStyle CssClass="CartListItem"></RowStyle>
            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
            <Columns>
                <asp:BoundField DataField="ANNO" HeaderText="Anno">
                    <HeaderStyle Width="10%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="VALORE" HeaderText="Valore">
                    <HeaderStyle Width="10%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="MINIMO" HeaderText="Minimo">
                    <HeaderStyle Width="10%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Riducibile">
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Width="10%" VerticalAlign="Middle"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# SetValoreSINO(DataBinder.Eval(Container, "DataItem.Riducibile"))%>'>Label</asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cumulabile">
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Width="10%" VerticalAlign="Middle"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# SetValoreSINO(DataBinder.Eval(Container, "DataItem.Cumulabile"))%>'>Label</asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DESC_BASE_CALCOLO" HeaderText="Calcolata su">
                    <HeaderStyle Width="20%"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DESC_BASE_RAFFRONTO" HeaderText="Base raffronto">
                    <HeaderStyle Width="10%"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DESC_PARAMETRO" HeaderText="Parametro">
                    <HeaderStyle Width="10%"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Condizione" HeaderText="Condizione">
                    <HeaderStyle Width="10%"></HeaderStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Intrasmissibilità se">
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Width="10%" VerticalAlign="Middle"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DESC_BASE_RAFFRONTO_INTR") & " " & DataBinder.Eval(Container, "DataItem.PARAMETRO_INTR") & " " & DataBinder.Eval(Container, "DataItem.Condizione_INTR") %>'>Label</asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID_VALORE_VOCE") %>' alt=""></asp:ImageButton>
                        <asp:HiddenField runat="server" ID="hfParametro_intr" Value='<%# Eval("Parametro_intr") %>' />
                        <asp:HiddenField runat="server" ID="hfBase_raffronto_intr" Value='<%# Eval("Base_raffronto_intr") %>' />
                        <asp:HiddenField runat="server" ID="hfCondizione_intr" Value='<%# Eval("Condizione_intr") %>' />
                        <asp:HiddenField runat="server" ID="hfCALCOLATA_SU" Value='<%# Eval("CALCOLATA_SU") %>' />
                        <asp:HiddenField runat="server" ID="hfParametro" Value='<%# Eval("Parametro") %>' />
                        <asp:HiddenField runat="server" ID="hfBase_raffronto" Value='<%# Eval("Base_raffronto") %>' />
                        <asp:HiddenField runat="server" ID="hfCOD_TIPO_INTERESSE" Value='<%# Eval("COD_TIPO_INTERESSE") %>' />
                        <asp:HiddenField runat="server" ID="hfID_VALORE_VOCE" Value='<%# Eval("ID_VALORE_VOCE") %>' />
                        <asp:HiddenField runat="server" ID="hfRiducibile" Value='<%# Eval("Riducibile") %>' />
                        <asp:HiddenField runat="server" ID="hfCumulabile" Value='<%# Eval("Cumulabile") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Grd:RibesGridView>
        <br>
        <br>
        <fieldset class="classeFiledSetIframe">
            <legend class="Legend">Configurazione Voci</legend>
        </fieldset>
        <table border="0" cellspacing="0" cellpadding="2" width="100%">
            <tr class="Input_Label">
                <td>Anno</td>
                <td>
                    <asp:TextBox ID="txtanno" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="44px" MaxLength="4"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblvalore" runat="server" CssClass="Input_Label">Valore</asp:Label>&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtvalore" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="91px" MaxLength="10"></asp:TextBox></td>
                <td>
                    <asp:Label ID="lblMinimo" runat="server" CssClass="Input_Label">Minimo</asp:Label></td>
                <td>
                    <asp:TextBox ID="txtMinimo" runat="server" CssClass="Input_Text_Right" Width="85px" MaxLength="10"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    <asp:CheckBox ID="chkRiducibile" runat="server" CssClass="Input_Label" BorderWidth="0px" Text="Riducibile"></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp; 
                    <asp:CheckBox ID="chkCumulabile" runat="server" CssClass="Input_Label" BorderWidth="0px" Text="Cumulabile"></asp:CheckBox></td>
            </tr>
            <tr class="Input_Label">
                <td>
                    <asp:Label ID="lblBaseRaffronto" runat="server" CssClass="Input_Label" Width="104px">Base Raffronto</asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlBaseRaffronto" runat="server" Width="250px" CssClass="Input_Text"></asp:DropDownList></td>
                <td>
                    <asp:Label ID="lblParametro" runat="server" CssClass="Input_Label">Parametro</asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlParametro" runat="server" Width="156px" CssClass="Input_CheckBox"></asp:DropDownList>&nbsp; 
                    <asp:Label ID="lblCondizione" runat="server" CssClass="Input_Label">Condizione</asp:Label><asp:TextBox ID="txtCondizione" runat="server" CssClass="Input_Text" Width="72px"></asp:TextBox></td>
            </tr>
            <tr class="Input_Label">
                <td>
                    <asp:Label ID="lblcalcSu" runat="server" CssClass="Input_Label" Width="80px">Calcolata su</asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlCalcolata" runat="server" Width="250px" CssClass="Input_Text"></asp:DropDownList></td>
            </tr>
            <tr class="Input_Label">
                <td>
                    <asp:Label ID="lblTipoInteresse" runat="server" CssClass="Input_Label" Width="88px">Tipo Interesse</asp:Label></td>
                <td style="width: 274px">
                    <asp:DropDownList ID="ddlTipoInteresse" runat="server" Width="211px" CssClass="Input_Text"></asp:DropDownList></td>
                <td colspan="3">
                    <asp:TextBox Style="display: none" ID="txtHiddenIDVALOREVOCE" runat="server" Width="48px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="Legend" colspan="3">Intrasmissibilità delle sanzioni</td>
            </tr>
            <tr class="Input_Label">
                <td>
                    <asp:Label ID="lblBaseRaffronto_Intr" runat="server" CssClass="Input_Label" Width="104px">Base Raffronto</asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlBaseRaffronto_Intr" runat="server" CssClass="Input_Text" Width="250px"></asp:DropDownList></td>
                <td>
                    <asp:Label ID="lblParametro_Intr" runat="server" CssClass="Input_Label">Parametro</asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlParametro_Intr" runat="server" CssClass="Input_CheckBox" Width="156px"></asp:DropDownList>&nbsp; 
                    <asp:Label ID="lblCondizione_intr" runat="server" CssClass="Input_Label">Condizione</asp:Label><asp:TextBox ID="txtCondizione_Intr" runat="server" CssClass="Input_Text" Width="72px"></asp:TextBox></td>
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
        <asp:Button Style="display: none" ID="btnSalva" runat="server" Text="Salva"></asp:Button><asp:Button Style="display: none" ID="btnCancella" runat="server" Text="Button"></asp:Button><asp:Button Style="display: none" ID="btnClear" runat="server" Text="Button"></asp:Button><asp:Button Style="display: none" ID="btnBack" runat="server" Text="Button"></asp:Button><asp:TextBox Style="display: none" ID="txtInsUp" runat="server" Width="16px"></asp:TextBox>
    </form>
</body>
</html>
