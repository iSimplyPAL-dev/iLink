<%@ Page Language="vb" CodeBehind="SearchResultsAnagrafica.aspx.vb" AutoEventWireup="false" Inherits="OPENgov.SearchResultsAnagrafica" EnableEventValidation="false" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<html>
<head>
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" marginwidth="0"
    marginheight="0">
    <form id="Form1" runat="server" method="post">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" CssClass="Input_Label_title"></asp:Label></td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
        <input id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza">
        <Grd:RibesGridView ID="GrdAnagrafica" runat="server" BorderStyle="None"
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
                <asp:TemplateField SortExpression="NOMINATIVO" HeaderText="Nominativo">
                    <HeaderStyle ForeColor="White"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AnagCognome") &amp; " " &amp; DataBinder.Eval(Container, "DataItem.AnagNome")%>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="AnagCodFisc" HeaderText="CodiceFiscale/P.I.">
                    <HeaderStyle ForeColor="White"></HeaderStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Data di nascita">
                    <HeaderStyle ForeColor="White"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDataNascita" runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.AnagDataNascita"))%>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="AnagSesso" HeaderText="Sesso">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDriga") %>' alt=""></asp:ImageButton>
                        <asp:HiddenField runat="server" ID="hfIDriga" Value='<%# Eval("IDriga") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
    </form>
</body>
</html>
