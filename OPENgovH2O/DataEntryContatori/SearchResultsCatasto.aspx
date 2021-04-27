<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SearchResultsCatasto.aspx.vb" Inherits="OpenUtenze.SearchResultsCatasto" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>SearchResultsCatasto</title>
    <link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <script type="text/javascript">
        function ApriModifica(parametro, parametro2) {
            myleft = ((screen.width) / 2) - 325;
            mytop = ((screen.height) / 2) - 100;
            window.open("FrameModificaDatoCatasto.aspx?IDCatasto=" + parametro + "&IDContatore=" + parametro2, "Catasto", "width=650, height=200, toolbar=no,top=" + mytop + ",left=" + myleft + ", menubar=no");
        }
    </script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0"
    marginwidth="0" marginheight="0">
    <form id="Form1" runat="server" method="post">
        <div align="left">
            <asp:Button ID="nullo" Style="display: none" runat="server" Text="Button"></asp:Button>
        </div>
        <br>
        <Grd:RibesGridView ID="GrdCatasto" runat="server" BorderStyle="None"
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
                <asp:BoundField DataField="INTERNO" HeaderText="Interno">
                    <HeaderStyle VerticalAlign="Middle"></HeaderStyle>
                    <ItemStyle VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="PIANO" HeaderText="Piano">
                    <HeaderStyle VerticalAlign="Middle"></HeaderStyle>
                    <ItemStyle VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="SEZIONE" HeaderText="Sezione"></asp:BoundField>
                <asp:BoundField DataField="FOGLIO" HeaderText="Foglio">
                    <HeaderStyle VerticalAlign="Middle"></HeaderStyle>
                    <ItemStyle VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NUMERO" HeaderText="Numero">
                    <HeaderStyle VerticalAlign="Middle"></HeaderStyle>
                    <ItemStyle VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Subalterno">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.Subalterno")) %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ESTENSIONE_PARTICELLA" HeaderText="Est. Particella"></asp:BoundField>
                <asp:BoundField DataField="ID_TIPO_PARTICELLA" HeaderText="Tipo Particella"></asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDCont_Catas") %>' alt=""></asp:ImageButton>
                        <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("IDCont_Catas") %>' />
                        <asp:HiddenField runat="server" ID="hfCODCONTATORE" Value='<%# Eval("CODCONTATORE") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Grd:RibesGridView>
        <div align="left">
            <asp:Label ID="lblMessage" CssClass="NormalBold" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
