<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VisualizzaValoriVoci.aspx.vb" Inherits="Provvedimenti.VisualizzaValoriVoci" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>VisualizzaValoriVoci</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../../_js/Custom.js?newversion"></script>
</head>
<body class="SfondoVisualizza">
    <form id="Form1" runat="server" method="post">
        <div id="voci">
            <Grd:RibesGridView ID="GrdVoci" runat="server" BorderStyle="None"
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
                    <asp:BoundField DataField="DESC_PARAMETRO" HeaderText="Parametro">
                        <HeaderStyle Width="10%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Condizione" HeaderText="Condizione">
                        <HeaderStyle Width="10%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DESC_BASE_RAFFRONTO" HeaderText="Base raffronto">
                        <HeaderStyle Width="10%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
                            <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("id") %>' />
                            <asp:HiddenField runat="server" ID="hfCALCOLATA_SU" Value='<%# Eval("CALCOLATA_SU") %>' />
                            <asp:HiddenField runat="server" ID="hfParametro" Value='<%# Eval("Parametro") %>' />
                            <asp:HiddenField runat="server" ID="hfBase_raffronto" Value='<%# Eval("Base_raffronto") %>' />
                            <asp:HiddenField runat="server" ID="hfRiducibile" Value='<%# Eval("Riducibile") %>' />
                            <asp:HiddenField runat="server" ID="hfCumulabile" Value='<%# Eval("Cumulabile") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Grd:RibesGridView>
            &nbsp;
        </div>
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
    </form>
</body>
</html>
