<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResultRicImmobile.aspx.vb" Inherits="OPENgovTIA.ResultRicImmobile" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ResultRicImmobile</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post" width="100%>
        <asp:Label ID="LblResult" runat="server" CssClass="Legend" Width="100%">Non sono presenti Immobili</asp:Label>
        <Grd:RibesGridView ID="GrdRicerca" runat="server" BorderStyle="None"
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
                <asp:TemplateField SortExpression="UBICAZIONE" HeaderText="Ubicazione">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UniTipoVia") & " " & DataBinder.Eval(Container, "DataItem.unidescrizionevia") & " " & DataBinder.Eval(Container, "DataItem.uninumerociv") & " " & DataBinder.Eval(Container, "DataItem.uniespoubi") & " " & DataBinder.Eval(Container, "DataItem.uniinternoubi") & " " & DataBinder.Eval(Container, "DataItem.uniscalaubi")%>' ID="Label29">
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="UNIFOGLIO" HeaderText="Foglio">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="56px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="UNINUMMAPP" HeaderText="Numero">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="56px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="UNISUBALTERNO" HeaderText="Subalterno">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="56px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="UNIDATAINIZIO" HeaderText="Data Inizio" DataFormatString="{0:dd/MM/yyyy}">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Data Fine">
                    <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="Label25" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.UNIDATAFINE")) %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="UNIDESCRDESTUSO" HeaderText="Cat./Destinazione d'uso">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="UNIcategcatastale" HeaderText="Cat.Catastale">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Sel.">
                    <ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelezionato" runat="server" AutoPostBack="True" Checked='<%# DataBinder.Eval(Container, "DataItem.Selezionato")%>'></asp:CheckBox>
                        <asp:HiddenField runat="server" ID="hfUniCodRicerca" Value='<%# Eval("UniCodRicerca") %>' />
                        <asp:HiddenField runat="server" ID="hfUniNote" Value='<%# Eval("UniNote") %>' />
                        <asp:HiddenField runat="server" ID="hfUniCodTabDestUso" Value='<%# Eval("UniCodTabDestUso") %>' />
                        <asp:HiddenField runat="server" ID="hfUniPianoUbi" Value='<%# Eval("UniPianoUbi") %>' />
                        <asp:HiddenField runat="server" ID="hfUniCodComUbi" Value='<%# Eval("UniCodComUbi") %>' />
                        <asp:HiddenField runat="server" ID="hfUniCodVia" Value='<%# Eval("UniCodVia") %>' />
                        <asp:HiddenField runat="server" ID="hfUniNumProgrFabbricato" Value='<%# Eval("UniNumProgrFabbricato") %>' />
                        <asp:HiddenField runat="server" ID="hfIDProgressivo" Value='<%# Eval("IDProgressivo") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Grd:RibesGridView>
        <iframe id="LoadGridVani" runat="server" src="../../aspVuota.aspx" frameborder="0" width="100%" height="400px"></iframe>
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
        <asp:Button ID="CmdRibaltaAnater" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdSearchVani" Style="display: none" runat="server"></asp:Button>
    </form>
</body>
</html>
