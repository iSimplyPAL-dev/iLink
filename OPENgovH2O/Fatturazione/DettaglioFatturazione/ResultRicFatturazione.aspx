
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResultRicFatturazione.aspx.vb" Inherits="OpenUtenze.ResultRicFatturazione" %>


<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ResultRicFatturazione</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
        function DettaglioDocumento(IdDoc, TipoProvenienza) {
            console.log("dettaglioDocumento");
            console.log("DettaglioFatturazione.aspx?IdDocumento=" + IdDoc + "&paginacomandi=ComandiDettFatturazione.aspx&PaginaChiamante=RicercaFatturazione.aspx?paginacomandi=ComandiRicFatturazione.aspx&Provenienza=" + TipoProvenienza + "&ProvenienzaChiamante=" + document.Form1.TxtProvenienza.value);
            parent.parent.Visualizza.location.href = 'DettaglioFatturazione.aspx?IdDocumento=' + IdDoc + '&paginacomandi=ComandiDettFatturazione.aspx&PaginaChiamante=RicercaFatturazione.aspx?paginacomandi=ComandiRicFatturazione.aspx&Provenienza=' + TipoProvenienza + '&ProvenienzaChiamante=' + document.Form1.TxtProvenienza.value
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <table style="z-index: 101; left: 0px; position: absolute; top: 0px" width="100%">
            <tr>
                <td>
                    <asp:Label ID="LblResult" CssClass="Legend" runat="server">Risultati della Ricerca</asp:Label>

                    <Grd:RibesGridView ID="GrdDocumenti" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="sPeriodo" HeaderText="Periodo">
                                <ItemStyle Width="80px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sCognome" HeaderText="Cognome">
                                <ItemStyle Width="300px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sNome" HeaderText="Nome">
                                <ItemStyle Width="200px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sCodFiscalePIva" HeaderText="Cod.Fiscale/P.IVA">
                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sMatricola" HeaderText="Matricola">
                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sTipoDocumento" HeaderText="Tipo Documento">
                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="tDataDocumento" HeaderText="Data Documento" DataFormatString="{0:dd/MM/yyyy}">
                                <ItemStyle HorizontalAlign="Right" Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sNDocumento" HeaderText="N.Documento">
                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="impDocumento" HeaderText="Imp. Documento" DataFormatString="{0:0.00}">
                                <ItemStyle HorizontalAlign="Right" Width="120px"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>                                    
                                    <asp:ImageButton id="ImgDet" runat="server" CssClass="BottoneGrd bottoneApri" CommandName="RowOpen" CommandArgument='<%# Eval("nIdDocumento") %>' alt=""></asp:ImageButton>                                    
                                    <asp:HiddenField runat="server" ID="hfnIdDocumento" Value='<%# Eval("nIdDocumento") %>' />
                                    <asp:HiddenField runat="server" ID="hfsVariato" Value='<%# Eval("sVariato") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
            <tr>
                <td>
                    <fieldset class="FiledSetRicerca" style="width: 100%">
                        <legend class="Legend">Totalizzatori</legend>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label12" CssClass="Legend" runat="server">N.Contribuenti</asp:Label></td>
                                <td>
                                    <asp:Label ID="LblNContribuenti" CssClass="Input_Label" runat="server">0</asp:Label></td>
                                <td>
                                    <asp:Label ID="Label3" CssClass="Legend" runat="server">N.Fatture</asp:Label></td>
                                <td>
                                    <asp:Label ID="LblNFatture" CssClass="Input_Label" runat="server">0</asp:Label></td>
                                <td>
                                    <asp:Label ID="Label4" CssClass="Legend" runat="server">N.Note</asp:Label></td>
                                <td>
                                    <asp:Label ID="LblNNote" CssClass="Input_Label" runat="server">0</asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" CssClass="Legend" runat="server">Imp.Fatture</asp:Label></td>
                                <td>
                                    <asp:Label ID="LblImpFatture" CssClass="Input_Label" runat="server">0</asp:Label></td>
                                <td>
                                    <asp:Label ID="Label6" CssClass="Legend" runat="server">Imp.Note</asp:Label></td>
                                <td>
                                    <asp:Label ID="LblImpNote" CssClass="Input_Label" runat="server">0</asp:Label></td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
        <asp:TextBox ID="TxtProvenienza" runat="server" Style="visibility:hidden"></asp:TextBox>
    </form>
</body>
</html>
