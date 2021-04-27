<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfigTipologieSanzioni.aspx.vb" Inherits="Provvedimenti.ConfigTipologieSanzioni" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ConfigTipologieSanzioni</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script src="../../../_js/VerificaCampi.js?newversion" type="text/javascript"></script>
    <script type="text/javascript">
        function Abilita() {
            parent.Comandi.document.getElementById("salva").disabled = false

            document.getElementById("txtOperazione").value = "U"
            document.getElementById("btnAbilita").click()
        }

        function Nuovo() {
            parent.Comandi.document.getElementById("salva").disabled = false
            parent.Comandi.document.getElementById("Elimina").disabled = true
            parent.Comandi.document.getElementById("abilita").disabled = true
            document.getElementById("btnNuovo").click()

        }

        function Modifica() {
            document.getElementById("divcampi").style.display = ""
            document.getElementById("divgriglia").style.display = "none"
            parent.Comandi.document.getElementById("divsalva").style.display = ""
            parent.Comandi.document.getElementById("divnuovo").style.display = "none"
        }
        function ModificaIndietro() {
            document.getElementById("divcampi").style.display = "none"
            document.getElementById("divgriglia").style.display = ""
            parent.Comandi.document.getElementById("divsalva").style.display = "none"
            parent.Comandi.document.getElementById("divnuovo").style.display = ""
        }

        function Salva() {

            if (document.getElementById("txtOperazione").value == "U") {
                if (confirm('Confermi la modifica dell\'elemento selezionato?')) {
                    document.getElementById("btnSalva").click()
                }
            } else {
                document.getElementById("txtOperazione").value = "S"
                document.getElementById("btnSalva").click()
            }
        }
        function Pulisci() {
            parent.Comandi.document.getElementById("Elimina").disabled = true
            document.getElementById("btnPulisci").click()
        }
        function Elimina() {
            if (confirm('Confermi la cancellazione dell\'elemento selezionato?')) {
                document.getElementById("txtOperazione").value = "E"
                document.getElementById("btnElimina").click()
            }
        }

        function indietro() {
            document.getElementById("divcampi").style.display = "none"
            document.getElementById("divgriglia").style.display = ""
            parent.Comandi.document.getElementById("divsalva").style.display = "none"
            parent.Comandi.document.getElementById("divnuovo").style.display = ""
            document.getElementById("btnIndietro").click()
        }
        function DisabilitaPulsanti() {
            parent.Comandi.document.getElementById("salva").disabled = true
            parent.Comandi.document.getElementById("Elimina").disabled = false
            parent.Comandi.document.getElementById("abilita").disabled = false
        }

        function RicaricaPagina() {
            //alert("RicaricaPagina");
            document.location.href = 'ConfigTipologieSanzioni.aspx';
        }
    </script>
</head>
<body class="SfondoVisualizza">
    <form id="Form1" runat="server" method="post">
        <div id="divcampi" style="display: none">
            <fieldset class="classeFiledSet100">
                <legend class="Legend">Parametri di Configurazione</legend>
                <br>
                <table cellspacing="0" cellpadding="2" width="100%" border="0">
                    <tr class="Input_Label" valign="top">
                        <td class="Input_Label" width="20%">Tributo
                        </td>
                        <td class="Input_Label" width="80%">
                            <asp:DropDownList ID="ddlTributo" runat="server" CssClass="Input_Text" Width="200px" Enabled="False"
                                AutoPostBack="True">
                            </asp:DropDownList></td>
                    </tr>
                    <tr class="Input_Label" valign="top">
                        <td class="Input_Label">Codice Tipo Voce
                        </td>
                        <td class="Input_Label">
                            <asp:TextBox ID="txtCodVoce" runat="server" Width="60px" Enabled="False" CssClass="Input_Text"
                                MaxLength="4"></asp:TextBox>
                            <!--<asp:dropdownlist id="ddlTipoVoci" runat="server" cssclass="Input_Text" Width="640px" Enabled="False"></asp:dropdownlist>-->
                        </td>
                    </tr>
                    <tr class="Input_Label" valign="top">
                        <td>Descrizione Tipo Voce</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtDescSanzione" runat="server" Width="572px" Enabled="False" CssClass="Input_Text"
                                MaxLength="255" Height="56px" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                </table>
                <br>
            </fieldset>
        </div>
        <div id="divgriglia">
            <fieldset class="classeFiledSetIframe100 ">
                <legend class="Legend">Visualizzazione Tipologia Voci</legend>
                <br>
                <asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" Visible="False">lblMessage</asp:Label>
            </fieldset>
            <Grd:RibesGridView ID="GrdSanzioni" runat="server" BorderStyle="None"
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
                    <asp:BoundField DataField="descTributo" HeaderText="Tipo Tributo">
                        <HeaderStyle Width="10%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="COD_VOCE" HeaderText="Codice Voce">
                        <HeaderStyle Width="20%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DESCRIZIONE" HeaderText="Descrizione Tipo Voce">
                        <HeaderStyle Width="70%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField Visible="False" DataField="COD_TRIBUTO" HeaderText="COD_TRIBUTO"></asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("PROGRESSIVO") %>' alt=""></asp:ImageButton>
                            <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("PROGRESSIVO") %>' />
                            <asp:HiddenField runat="server" ID="hfCOD_TRIBUTO" Value='<%# Eval("COD_TRIBUTO") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Grd:RibesGridView>
            &nbsp;
				<br>
        </div>
        <asp:Label CssClass="Input_Label" runat="server" ID="Label1">Il Codice Voce 97 deve essere obbligatoriamente legato all'instrasmissibilità agli eredi</asp:Label>
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
        <asp:Button ID="btnPulisci" Style="display: none" runat="server" Text="Pulisci"></asp:Button>
        <asp:Button ID="btnNuovo" Style="display: none" runat="server" Text="Nuovo"></asp:Button>
        <asp:Button ID="btnAbilita" Style="display: none" runat="server" Text="Abilita"></asp:Button>
        <asp:Button ID="btnIndietro" Style="display: none" runat="server" Text="Indietro"></asp:Button>
        <asp:TextBox ID="txtOperazione" Style="display: none" runat="server" size="1"></asp:TextBox>
        <asp:HiddenField ID="hfIdRow" runat="server" Value="-1" />
    </form>
</body>
</html>
