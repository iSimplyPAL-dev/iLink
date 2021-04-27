<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfigTassiInteressi.aspx.vb" Inherits="Provvedimenti.ConfigTassiInteressi" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ConfigTassiInteressi</title>
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

            if (document.getElementById("txtTasso").value == "" || document.getElementById("txtDal").value == "") {
                GestAlert('a', 'warning', '', '', 'Valorizzare i campi\nTipo Interesse\nData Dal\nTasso Percentuale')
            } else {
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
        function ControllaTasso(oggetto) {
            if (!IsBlank(oggetto.value)) {
                if (!isNumber(oggetto.value, 3, 3, 0, 100)) {
                    GestAlert('a', 'warning', '', '', 'Inserire un tasso percentuale compreso fra 0 e 100\ncon al massimo tre decimali separati dalla virgola.')
                    oggetto.value = ""
                    oggetto.focus()
                }
            }
        }
        function RicaricaPagina() {
            document.location.href = 'ConfigTassiInteressi.aspx';
        }
    </script>
</head>
<body class="SfondoVisualizza">
    <form id="Form1" runat="server" method="post">
        <fieldset class="classeFiledSetIframe100 ">
            <legend class="Legend">Visualizzazione Tassi di Interesse</legend>
            <asp:Label ID="lblMessage" runat="server" Visible="False" CssClass="NormalRed">lblMessage</asp:Label>
            <Grd:RibesGridView ID="GrdInteressi" runat="server" BorderStyle="None"
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="98%"
                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="Descrizione" HeaderText="Tipo Interesse">
                        <HeaderStyle Width="30%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Dal">
                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Width="20%" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# GiraData(DataBinder.Eval(Container, "DataItem.DAL"))%>'>Label</asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Al">
                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Width="20%" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# GiraData(DataBinder.Eval(Container, "DataItem.AL"))%>'>Label</asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TASSO_ANNUALE" HeaderText="Tasso %">
                        <HeaderStyle Width="15%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Tributo">
                        <HeaderStyle Width="15%"></HeaderStyle>
                        <ItemStyle VerticalAlign="Middle"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("COD_TIPO_INTERESSE") & Eval("COD_TRIBUTO") & Eval("DAL") & Eval("AL") & Eval("COD_TIPO_INTERESSE")%>' alt=""></asp:ImageButton>
                            <asp:HiddenField runat="server" ID="hfCOD_TRIBUTO" Value='<%# Eval("COD_TRIBUTO") %>' />
                            <asp:HiddenField runat="server" ID="hfDAL" Value='<%# Eval("DAL") %>' />
                            <asp:HiddenField runat="server" ID="hfAL" Value='<%# Eval("AL") %>' />
                            <asp:HiddenField runat="server" ID="hfCOD_TIPO_INTERESSE" Value='<%# Eval("COD_TIPO_INTERESSE") %>' />
                            <asp:HiddenField runat="server" ID="hfUNICO" Value='<%# Eval("COD_TIPO_INTERESSE") & Eval("COD_TRIBUTO") & Eval("DAL") & Eval("AL") & Eval("COD_TIPO_INTERESSE")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Grd:RibesGridView>
        </fieldset>
        <br>
        <fieldset class="classeFiledSet100">
            <legend class="Legend">Parametri di Configurazione</legend>
            <br>
            <table cellspacing="0" cellpadding="2" width="100%" border="0">
                <tr>
                    <td class="Input_Label" width="10%">Tributo</td>
                    <td class="Input_Label">
                        <asp:DropDownList ID="ddlTributo" runat="server" CssClass="Input_Text" AutoPostBack="True"></asp:DropDownList>
                    </td>
                    <td class="Input_Label">Tipo Interesse</td>
                    <td class="Input_Label">
                        <asp:DropDownList ID="ddlTipoInteresse" runat="server" CssClass="Input_Text" Width="200px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="Input_Label">Dal </td>
                    <td class="Input_Label">
                        <asp:TextBox ID="txtDal" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10"></asp:TextBox>
                    </td>
                    <td class="Input_Label">Al </td>
                    <td class="Input_Label">
                        <asp:TextBox ID="txtAl" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10"></asp:TextBox>
                    </td>
                    <td class="Input_Label">% Tasso </td>
                    <td class="Input_Label">
                        <asp:TextBox ID="txtTasso" onblur="ControllaTasso(this)" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="50px" MaxLength="6" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br>
        </fieldset>
        &nbsp;
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
        <asp:Button ID="btnSalva" Style="display: none" runat="server" Text="Salva"></asp:Button>
        <asp:Button ID="btnElimina" Style="display: none" runat="server" Text="Elimina"></asp:Button>
        <asp:TextBox ID="txtOperazione" Style="display: none" runat="server"></asp:TextBox>
        <asp:Button ID="btnPulisci" Style="display: none" runat="server" Text="Pulisci"></asp:Button>        
    </form>
</body>
</html>
