<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfigAnniProvvedimenti.aspx.vb" Inherits="Provvedimenti.ConfigAnniProvvedimenti" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ConfigAnniProvvedimenti</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script src="../../../_js/VerificaCampi.js?newversion" type="text/javascript"></script>
    <script type="text/javascript">
        function Salva() {
            intSelected = document.getElementById('ddlTributo').selectedIndex;
            if (document.getElementById('ddlTributo')[intSelected].value == '-1') {
                GestAlert('a', 'warning', '', '', 'Tutti i dati sono obbligatori');
                document.getElementById('ddlTributo').focus();
                return false;
            }

            intSelected = document.getElementById('ddlProvvedimenti').selectedIndex;
            if (document.getElementById('ddlProvvedimenti')[intSelected].value == '-1') {
                GestAlert('a', 'warning', '', '', 'Tutti i dati sono obbligatori');
                document.getElementById('ddlProvvedimenti').focus();
                return false;
            }

            if (document.getElementById('txtAnno').value == '') {
                GestAlert('a', 'warning', '', '', 'Tutti i dati sono obbligatori');
                document.getElementById('txtAnno').focus();
                return false;
            }

            if (document.getElementById('txtSoglia').value == '') {
                GestAlert('a', 'warning', '', '', 'Tutti i dati sono obbligatori');
                document.getElementById('txtSoglia').focus();
                return false;
            }
            //*** 20140701 - IMU/TARES ***
            if (document.getElementById('txtQuotaRiduzioneSanzioni').value == '') {
                GestAlert('a', 'warning', '', '', 'Tutti i dati sono obbligatori');
                document.getElementById('txtQuotaRiduzioneSanzioni').focus();
                return false;
            }
            //'*** ***

            if (document.getElementById("txtOperazione").value == "U") {
                if (confirm('Confermi la modifica dell\'elemento selezionato?')) {
                    document.getElementById("btnSalva").click()
                }
            } else {
                document.getElementById("btnSalva").click()
            }
        }

        function Pulisci() {
            document.getElementById("btnPulisci").click()
        }

        function Cancella() {
            if (document.getElementById("txtOperazione").value == "U") {
                if (confirm('Confermi la cancellazione dell\'elemento selezionato?')) {
                    document.getElementById("btnCancella").click()
                }
            } else {
                GestAlert('a', 'warning', '', '', 'Selezionare l\'elemento da eliminare')
            }
        }

        function ControllaAnno(oggetto) {
            if (!IsBlank(oggetto.value)) {
                if (!isNumber(oggetto.value, 4, 0, 1950, 2090)) {
                    GestAlert('a', 'warning', '', '', 'Inserire un anno di quattro cifre\ncompreso fra 1950 e 2090')
                    oggetto.value = ""
                    oggetto.focus()
                    return false
                }
            }
        }

        function ControllaSoglia(oggetto) {
            if (!IsBlank(oggetto.value)) {
                if (!isNumber(oggetto.value, 4, 2, 0, 100000)) {
                    GestAlert('a', 'warning', '', '', 'Inserire un numero positivo con i decimali separati dalla virgola')
                    oggetto.value = ""
                    oggetto.focus()
                    return false
                }
            }
        }

        function ControllaQuotaRiduzioneSanzioni(oggetto) {
            if (!IsBlank(oggetto.value)) {
                if (!isNumber(oggetto.value, 1, 0, 0, 100000)) {
                    GestAlert('a', 'warning', '', '', 'Inserire un numero intero')
                    oggetto.value = ""
                    oggetto.focus()
                    return false
                }
            }
        }
    </script>
</head>
<body class="SfondoVisualizza">
    <form id="Form1" runat="server" method="post">
        <fieldset class="classeFiledSet100">
            <legend class="Legend">Visualizzazione Anni Provvedimenti</legend>
        </fieldset>
        <asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" Visible="False">lblMessage</asp:Label>
        <Grd:RibesGridView ID="GrdAnniProvvedimenti" runat="server" BorderStyle="None"
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
                <asp:BoundField DataField="DescTributo" HeaderText="Tributo">
                    <HeaderStyle Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DescTipoProvvedimento" HeaderText="Tipo Provvedimento">
                    <HeaderStyle Width="45%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ANNO" HeaderText="Anno">
                    <HeaderStyle Width="15%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="IMPORTO_MINIMO_ANNO" HeaderText="Soglia minima €">
                    <HeaderStyle Width="15%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("PROGRESSIVO") %>' alt=""></asp:ImageButton>
                        <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("PROGRESSIVO") %>' />
                        <asp:HiddenField runat="server" ID="hfCOD_TRIBUTO" Value='<%# Eval("COD_TRIBUTO") %>' />
                        <asp:HiddenField runat="server" ID="hfCOD_TIPO_PROVVEDIMENTO" Value='<%# Eval("COD_TIPO_PROVVEDIMENTO") %>' />
                        <asp:HiddenField runat="server" ID="hfQUOTARIDUZIONESANZIONI" Value='<%# Eval("QUOTARIDUZIONESANZIONI") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Grd:RibesGridView>
        <br>
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Parametri di Configurazione</legend>
            <br>
            <table cellspacing="0" cellpadding="2" width="100%" border="0">
                <tr class="Input_Label">
                    <td width="10%" style="height: 41px">Tributo
                    </td>
                    <td width="20%" style="height: 41px">
                        <asp:DropDownList ID="ddlTributo" runat="server" CssClass="Input_Text" Width="100px" AutoPostBack="True"></asp:DropDownList></td>
                    <td width="20%" style="height: 41px">Tipo Provvedimento
                    </td>
                    <td width="50%" style="height: 41px" colspan="3">
                        <asp:DropDownList ID="ddlProvvedimenti" runat="server" CssClass="Input_Text" Width="100%"></asp:DropDownList></td>
                </tr>
                <tr class="Input_Label">
                    <td>Anno
                    </td>
                    <td style="width: 134px">
                        <asp:TextBox ID="txtAnno" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="40px" MaxLength="4" onchange="ControllaAnno(this)"></asp:TextBox></td>
                    <td>Soglia Minima €</td>
                    <td>
                        <asp:TextBox ID="txtSoglia" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="75px" MaxLength="7" onblur="ControllaSoglia(this)" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:TextBox></td>
                    <!--*** 20140701 - IMU/TARES ***-->
                    <td>Quota Riduzione Sanzioni</td>
                    <td>
                        <asp:TextBox ID="txtQuotaRiduzioneSanzioni" runat="server" CssClass="Input_Text_Right" Width="75px" MaxLength="3" onblur="ControllaQuotaRiduzioneSanzioni(this)">3</asp:TextBox></td>
                    <!--*** ***-->
                </tr>
            </table>
            <br>
        </fieldset>
        &nbsp;
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
        <asp:Button ID="btnCancella" Style="display: none" runat="server" Text="Cancella"></asp:Button>
        <asp:Button ID="btnPulisci" Style="display: none" runat="server" Text="Pulisci"></asp:Button>
        <asp:TextBox ID="txtOperazione" Style="display: none" runat="server"></asp:TextBox>
    </form>
</body>
</html>
