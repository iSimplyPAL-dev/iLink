<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResultRicVani.aspx.vb" Inherits="OPENgovTIA.ResultRicVani" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ResultRicVani</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <script type="text/javascript" src="../_js/VerificaCampi.js?newversion"></script>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post" width="100%">
        <asp:Label ID="LblResult" CssClass="Legend" runat="server">Non sono presenti Vani</asp:Label>
        <asp:CheckBox ID="chkSelezionaTutti" runat="server" Style="margin-left: 700px" CssClass="Input_Label" Text="Seleziona Tutti" AutoPostBack="true"></asp:CheckBox><br />
        <Grd:RibesGridView ID="GrdVani" runat="server" BorderStyle="None"
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
                <asp:BoundField DataField="CUMFOGLIO" HeaderText="Foglio">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CUMMAPPALE" HeaderText="Numero">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CUMSUBALTERNO" HeaderText="Subalterno">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DescrizioneDestinazione" HeaderText="Cat./Destinazione d'uso">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CUMSuperficie" HeaderText="MQ">
                    <HeaderStyle Width="90px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Sel.">
                    <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelezionato" runat="server" AutoPostBack="True" Checked='<%# DataBinder.Eval(Container, "DataItem.selezionato")%>'></asp:CheckBox>
                        <asp:HiddenField runat="server" ID="hfCUMTipoLocale" Value='<%# Eval("CUMTipoLocale") %>' />
                        <asp:HiddenField runat="server" ID="hfIDProgressivo" Value='<%# Eval("IDProgressivo") %>' />
                        <asp:HiddenField runat="server" ID="hfIDProgressivoUI" Value='<%# Eval("IDProgressivoUI") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="Textbox3" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Grd:RibesGridView>
        <br />
        <fieldset class="FiledSetRicerca">
            <table id="TblParametri" cellspacing="1" cellpadding="1" width="700" border="0">
                <tr>
                    <td>
                        <asp:Label ID="Label7" CssClass="Input_Label" runat="server">Categoria TARSU</asp:Label><br />
                        <asp:DropDownList ID="DdlCategoria" CssClass="Input_Text" runat="server" Width="450px" AutoPostBack="True"></asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="Label1" CssClass="Input_Label" runat="server">Data Inizio</asp:Label><br />
                        <asp:TextBox runat="server" ID="TxtDataInizio" CssClass="Input_Text_Right TextDate" AutoPostBack="True" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                    </td>
                </tr>
                <!--*** 20140805 - Gestione Categorie Vani ***-->
                <tr>
                    <td>
                        <asp:Label CssClass="Input_Label" runat="server" ID="Label35">Categoria</asp:Label><br>
                        <asp:DropDownList ID="DDlCatTARES" CssClass="Input_Label" runat="server" Width="450px" AutoPostBack="True"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label27" runat="server" CssClass="Input_Label">Componenti PF</asp:Label><br />
                        <asp:TextBox ID="TxtNComponenti" runat="server" CssClass="Input_Text" Style="text-align: right" MaxLength="2" Width="50px" AutoPostBack="True"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label34" runat="server" CssClass="Input_Label">Componenti PV</asp:Label><br />
                        <asp:TextBox ID="TxtNComponentiPV" runat="server" CssClass="Input_Text" Style="text-align: right" MaxLength="2" Width="50px" AutoPostBack="True"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </form>
</body>
</html>
