<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfigScadenzaInteressi.aspx.vb" Inherits="Provvedimenti.ConfigScadenzaInteressi" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ConfigScadenzaInteressi</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script src="../../../_js/VerificaCampi.js?newversion" type="text/javascript"></script>
    <script type="text/javascript">
        function Salva() {

            if (document.getElementById("ddlAnno").value == "" || document.getElementById("txtData").value == "") {
                GestAlert('a', 'warning', '', '', 'Valorizzare i campi\nAnno\nData Scadenza')
            }
            else {
                document.getElementById("btnSalva").click()
            }

        }
        function Elimina() {
            if (document.getElementById("txtOperazione").value == "M") {
                if (confirm('Confermi l\'eliminazione del Tasso di Interesse?')) {
                    document.getElementById("btnElimina").click()
                }
            } else {
                GestAlert('a', 'warning', '', '', 'Selezionare l\'elemento da eliminare')
            }
        }
        function Pulisci() {
            document.getElementById("btnPulisci").click()
        }

        function RicaricaPagina() {
            //alert("RicaricaPagina");
            document.location.href = 'ConfigScadenzaInteressi.aspx';
        }

    </script>
</head>
<body class="SfondoVisualizza">
    <form id="Form1" runat="server" method="post">
        <fieldset class="classeFiledSetIframe100 ">
            <legend class="Legend">Visualizzazione Scadenze Interessi</legend>
        </fieldset>
        <asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" Visible="False">lblMessage</asp:Label>
        <Grd:RibesGridView ID="GrdInteressi" runat="server" BorderStyle="None"
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
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Data Scadenza">
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" Width="20%" VerticalAlign="Middle"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATA_SCADENZA")) %>'>Label</asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DESCRIZIONE" HeaderText="Tributo">
                    <HeaderStyle Width="15%"></HeaderStyle>
                    <ItemStyle VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NOTE" HeaderText="Note">
                    <HeaderStyle Width="55%"></HeaderStyle>
                    <ItemStyle VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField Visible="False" DataField="COD_TRIBUTO">
                    <HeaderStyle Width="15%"></HeaderStyle>
                    <ItemStyle VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField Visible="False" DataField="DATA_SCADENZA">
                    <HeaderStyle Width="15%"></HeaderStyle>
                    <ItemStyle VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("PROGRESSIVO") %>' alt=""></asp:ImageButton>
                        <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("PROGRESSIVO") %>' />
                        <asp:HiddenField runat="server" ID="hfDATA_SCADENZA" Value='<%# Eval("DATA_SCADENZA") %>' />
                        <asp:HiddenField runat="server" ID="hfCOD_TRIBUTO" Value='<%# Eval("COD_TRIBUTO") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Grd:RibesGridView>
        &nbsp;
			<br>
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Parametri di Configurazione</legend>
            <br>
            <table cellspacing="0" cellpadding="2" width="100%" border="0">
                <tr>
                    <td class="Input_Label" style="width: 46px">Anno</td>
                    <td class="Input_Label">
                        <asp:DropDownList ID="ddlAnno" runat="server" AutoPostBack="True" CssClass="Input_Text"></asp:DropDownList></td>
                    <td class="Input_Label" style="width: 71px">Data Scadenza
                    </td>
                    <td class="Input_Label" style="width: 114px">
                        <asp:TextBox ID="txtData" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10"></asp:TextBox></td>
                    <td class="Input_Label" style="width: 46px">Note
                    </td>
                    <td class="Input_Label">
                        <asp:TextBox ID="txtNote" runat="server" Width="558px" CssClass="Input_Text" MaxLength="50"></asp:TextBox></td>
                </tr>
            </table>
            <br>
        </fieldset>
        <br>
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
        <asp:Button ID="btnSalva" Style="display: none" runat="server" Text="Salva"></asp:Button><asp:Button ID="btnElimina" Style="display: none" runat="server" Text="Elimina"></asp:Button><asp:TextBox ID="txtOperazione" Style="display: none" runat="server"></asp:TextBox><asp:Button ID="btnPulisci" Style="display: none" runat="server" Text="Pulisci"></asp:Button>
    </form>
</body>
</html>
