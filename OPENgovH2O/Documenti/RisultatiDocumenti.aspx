<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RisultatiDocumenti.aspx.vb" Inherits="OpenUtenze.RisultatiDocumenti" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>RisultatiDocumenti</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <table style="z-index: 101; left: 0px; position: absolute; top: 0px" width="100%">
            <tr>
                <td colspan="5">
                    <asp:Label ID="Label6" CssClass="lstTabRow" runat="server" Width="100%">Documenti Del Ruolo da Elaborare</asp:Label></td>
            </tr>
            <tr>
                <td>
                    <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                        <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                        <div class="BottoneClessidra">&nbsp;</div>
                        <div class="Legend">Attendere Prego</div>
                    </div>
                    <Grd:RibesGridView ID="GrdFattura" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnPageIndexChanging="GrdPageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="sCognome" SortExpression="sCognome" HeaderText="Cognome">
                                <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                <ItemStyle Width="300px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sNome" SortExpression="sNome" HeaderText="Nome">
                                <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                <ItemStyle Width="200px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sCodFiscalePIva" HeaderText="Cod.Fiscale/P.IVA">
                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Tipo Documento">
                                <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                <ItemStyle Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# FncGrd.GetTipologiaDocumento(DataBinder.Eval(Container, "DataItem.sTipoDocumento")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Documento">
                                <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" Width="50px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataDocumento")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="sNDocumento" HeaderText="N.Documento">
                                <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Imp. Totale &amp;#8364">
                                <HeaderStyle HorizontalAlign="Center" ForeColor="White"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" Width="120px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# FncGrd.EuroForGridView(DataBinder.Eval(Container, "DataItem.impDocumento")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sel.">
                                <HeaderStyle HorizontalAlign="Center" ForeColor="White"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkSelezionato" runat="server" AutoPostBack="True" Checked='<%# DataBinder.Eval(Container, "DataItem.Selezionato")%>'></asp:CheckBox>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
            <tr>
                <td>
                    <Grd:RibesGridView ID="GrdTotaliDettaglio" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="DescrVoce" HeaderText="Descrizione">
                                <HeaderStyle ForeColor="White"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Imp. Totale ">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# FncGrd.EuroForGridView(DataBinder.Eval(Container, "DataItem.ImportoVoce")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table5" cellspacing="1" cellpadding="1" width="100%" border="0">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label5" CssClass="lstTabRow" runat="server" Width="100%">Ordinamento</asp:Label></td>
                            <td colspan="4">
                                <asp:Label ID="Label7" CssClass="lstTabRow" runat="server" Width="100%">Elaborazione</asp:Label></td>
                            <td>
                                <asp:Label CssClass="lstTabRow" runat="server" ID="lblNumDocPerFile">N. File per Gruppo</asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton ID="optIndirizzo" runat="server" CssClass="Input_Label" Text="Indirizzo" Checked="True"
                                    GroupName="OptOrdinamento" ToolTip="Ordinamento per indirizzo" AutoPostBack="True"></asp:RadioButton></td>
                            <td>
                                <asp:RadioButton ID="optNominativo" runat="server" CssClass="Input_Label" Text="Nominativo" Checked="False"
                                    GroupName="OptOrdinamento" ToolTip="Ordinamento per nominativo" AutoPostBack="True"></asp:RadioButton></td>
                            <td>
                                <asp:RadioButton ID="optProve" runat="server" CssClass="Input_Label" Text="Prove" Checked="True"
                                    GroupName="OptElaborazione" ToolTip="Elaborazione Prove" AutoPostBack="True"></asp:RadioButton></td>
                            <td>
                                <asp:RadioButton ID="optEffettivo" runat="server" CssClass="Input_Label" Text="Effettivo" Checked="False"
                                    GroupName="OptElaborazione" ToolTip="Elaborazione effettiva" AutoPostBack="True"></asp:RadioButton></td>
                            <td>
                                <asp:CheckBox ID="chkElaboraTutti" runat="server" CssClass="Input_Label" Text="Elabora Tutti" Checked="False" AutoPostBack="True" Visible="False"></asp:CheckBox>
                                <!--*** 20140509 - TASI ****-->
                                <br />
                                <asp:CheckBox ID="chkSendMail" runat="server" CssClass="Input_Label" Text="Invio tramite EMail" Visible="false" Checked="false" AutoPostBack="true"></asp:CheckBox>
                                <!--*** ***-->
                            </td>
                            <td>
                                <asp:CheckBox ID="chkElaboraBollettini" runat="server" Checked="False" CssClass="Input_Label" AutoPostBack="true" Text="Elabora Bollettini"></asp:CheckBox>
                                <br />
                                <asp:RadioButton ID="optTD896" runat="server" GroupName="TipoBollettino" Text="TD896" CssClass="Input_Label" Enabled="false" />&nbsp;
									<asp:RadioButton ID="optTD123" runat="server" GroupName="TipoBollettino" Text="TD123" CssClass="Input_Label" Enabled="false" Checked="true" />&nbsp;
									<asp:RadioButton ID="optF24" runat="server" GroupName="TipoBollettino" Text="F24" CssClass="Input_Label" Enabled="false" Style="display: none" />&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtNumDoc" runat="server" CssClass="Input_Text" Width="70" AutoPostBack="true" onkeypress="return NumbersOnly(event);">50</asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
