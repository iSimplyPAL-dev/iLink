<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ViewAccertamentiStampati.aspx.vb" Inherits="Provvedimenti.ViewAccertamentiStampati" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Elenco Documenti Elaborati</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
</head>
<body class="Sfondo" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post">
        <table cellpadding="0" border="0" style="left: 10px; width: 90%; position: absolute; top: 10px; height: 100px">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" CssClass="Input_Label">Elenco Documenti Elaborati</asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" CssClass="NormalRed"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <Grd:RibesGridView ID="GrdAnagrafica" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="name" SortExpression="name" HeaderText="Nome Documento">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <a href='<%# DataBinder.Eval(Container, "DataItem.url") %>' target="_blank">Apri</a>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <a href='<%# DataBinder.Eval(Container, "DataItem.url") %>' target="_blank">Apri</a>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
