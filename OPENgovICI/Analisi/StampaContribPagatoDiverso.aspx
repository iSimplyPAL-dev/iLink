<%@ Page Language="c#" CodeBehind="StampaContribPagatoDiverso.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.StampaContribPagatoDiverso" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>StampaContribPagatoDiverso</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
        function estraiExcel() {
            if (document.getElementById('GrdRisultati') == null) {
                GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Eseguire la ricerca.');
            }
            else {
                document.getElementById('DivAttesa').style.display = '';
                document.getElementById('btnStampaExcel').click()
            }
            return false;
        }
    </script>
</head>
<body class="Sfondo" leftmargin="3" rightmargin="3" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post">
        <asp:Button ID="btnStampaExcel" Style="display: none" runat="server" Text="Button"></asp:Button><asp:Button ID="btnTrova" Style="display: none" runat="server" Text="Trova" ToolTip="Permette di eseguire una ricerca in funzione dei filtri utilizzati" OnClick="btnTrova_Click"></asp:Button>
        <table id="tblCorpo" cellspacing="1" cellpadding="1" width="100%" align="center" border="0">
            <tr>
                <td>
                    <asp:Label ID="lblTesto" runat="server" CssClass="Legend">Elenco dei contribuenti che, in base all'anno selezionato, hanno pagato diverso dal dovuto calcolato.</asp:Label>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <fieldset class="classeFiledSetRicerca">
                        <legend class="Legend">Inserimento parametri di ricerca</legend>
                        <table id="tblFiltri" cellspacing="1" cellpadding="5" width="100%" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblAnnoRif" runat="server" CssClass="Input_Label">Anno di Riferimento</asp:Label><br />
                                    <asp:DropDownList ID="ddlAnnoRiferimento" runat="server" CssClass="Input_Text" Width="200px" DESIGNTIMEDRAGDROP="24"></asp:DropDownList></td>
                                <!--*** 20140630 - TASI ***-->
                                <td>
                                    <asp:CheckBox ID="chkICI" runat="server" CssClass="Input_Label" Text="ICI/IMU" Checked="true" />
                                    &nbsp;
		                                <asp:CheckBox ID="chkTASI" runat="server" CssClass="Input_Label" Text="TASI" Checked="false" />
                                </td>
                                <!--*** ***-->
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                        <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                        <div class="BottoneClessidra">&nbsp;</div>
                        <div class="Legend">Attendere Prego</div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRisultati" runat="server" CssClass="Legend">Risultati della Ricerca</asp:Label>
                    <br />
                    <Grd:RibesGridView ID="GrdRisultati" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="20"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnPageIndexChanging="GrdPageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="COGNOME" HeaderText="Cognome" SortExpression="Cognome">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NOME" HeaderText="Nome" SortExpression="Nome">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CFPIVA" HeaderText="Codice Fiscale/Partita Iva">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="AnnoRiferimento" ReadOnly="True" HeaderText="Anno" SortExpression="AnnoRiferimento">
                                <HeaderStyle></HeaderStyle>
                                <ItemStyle HorizontalAlign="right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Tributo">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Dovuto ">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoDovuto")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Versato ">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoPagato")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Differenza ">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.diff")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
