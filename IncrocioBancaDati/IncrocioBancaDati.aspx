<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IncrocioBancaDati.aspx.vb" Inherits="OPENgov.IncrocioBancaDati" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>IncrocioBancaDati</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
        function Ricerca() {
            if (document.getElementById("ddlTributo").selectedIndex != 0 && (document.getElementById("radioGA").checked || document.getElementById("radioAG").checked || document.getElementById("radioSUP").checked)) {
                DivAttesa.style.display = '';
                document.getElementById("btnRicerca").click()
            } else {
                GestAlert('a', 'warning', '', '', 'Selezionare il Tributo e almeno un\'opzione di ricerca');
            }
        }

        function PulisciCampi() {
            document.getElementById("radioGA").checked = false;
            document.getElementById("radioAG").checked = false;
            document.getElementById("radioSUP").checked = false;
            document.getElementById("radioGA").disabled = false;
            document.getElementById("radioAG").disabled = false;
            document.getElementById("radioSUP").disabled = false;
            document.getElementById("ddlTributo").selectedIndex = 0;
            document.getElementById("btnPulisci").click()
            parent.Comandi.document.getElementById("Excel").style.display = "none"

        }

        function ModificaRadio() {
            parent.Comandi.document.getElementById("Excel").style.display = "none"
            document.getElementById("btnModificaRadio").click()
        }

        function Excel() {
            document.getElementById("btnExcel").click()
        }

        function Nascondi() {
            if (document.getElementById("RibesgridUI") != null) {
                document.getElementById("RibesgridUI").style.display = "none"
            }
            parent.Comandi.document.getElementById("Excel").style.display = "none"
        }

        function UpdateFigli() {
            document.getElementById("CmdUpdateFigli").click()
        }
    </script>
</head>
<body class="SfondoVisualizza" leftmargin="20" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td>
                    <fieldset class="classeFiledSetRicerca">
                        <legend class="Legend">Parametri di Ricerca</legend>
                        <br />
                        <asp:Label ID="Label1" runat="server" CssClass="Input_Label">Tributo</asp:Label>&nbsp;
				            <asp:DropDownList ID="ddlTributo" runat="server" CssClass="Input_Label" onchange="ModificaRadio()"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
				            <asp:Label ID="Label3" runat="server" CssClass="Input_Label" Visible="false">Anno</asp:Label>&nbsp;
				            <asp:DropDownList ID="ddlAnno" runat="server" CssClass="Input_Label" Visible="false"></asp:DropDownList><br />
                        <br />
                        <asp:RadioButton ID="radioGA" runat="server" CssClass="Input_Label" GroupName="radioui" Text="Tutte le UI presenti in GEC e non in ANATER" onclick="Nascondi()"></asp:RadioButton><br />
                        <asp:RadioButton ID="radioAG" runat="server" CssClass="Input_Label" GroupName="radioui" Text="Tutte le UI presenti in ANATER e non in GEC" onclick="Nascondi()"></asp:RadioButton><br />
                        <asp:RadioButton ID="radioSUP" runat="server" CssClass="Input_Label" GroupName="radioui" Text="Tutte le UI presenti in entrambe le banche dati ma con superfici incoerenti" onclick="Nascondi()"></asp:RadioButton><br />
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Label ID="lblNotFound" runat="server" CssClass="NormalRed" Visible="False">Nessuna Unità Immobiliare trovata</asp:Label>
                    <Grd:RibesGridView ID="GrdUI" runat="server" BorderStyle="None"
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
                            <asp:TemplateField SortExpression="COD_TRIBUTO" HeaderText="Tributo">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# SetTributo(DataBinder.Eval(Container, "DataItem.COD_TRIBUTO")) %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hfCOD_TRIBUTO" Value='<%# Eval("COD_TRIBUTO") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Via" SortExpression="Via" ReadOnly="True" HeaderText="Via">
                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Civico" SortExpression="Civico" ReadOnly="True" HeaderText="Civico">
                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Interno" SortExpression="Interno" ReadOnly="True" HeaderText="Interno">
                                <ItemStyle HorizontalAlign="right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Foglio" SortExpression="Foglio" ReadOnly="True" HeaderText="Foglio">
                                <ItemStyle HorizontalAlign="right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Numero" SortExpression="Numero" ReadOnly="True" HeaderText="Numero">
                                <ItemStyle HorizontalAlign="right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Subalterno" SortExpression="Subalterno" ReadOnly="True" HeaderText="Subalterno">
                                <ItemStyle HorizontalAlign="right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="StatoUtilizzoANA" SortExpression="StatoUtilizzoANA" ReadOnly="True" HeaderText="Stato Utilizzo">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="StatoUtilizzoGEC" SortExpression="StatoUtilizzoGEC" ReadOnly="True" HeaderText="St. Ut. GEC">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SupAnater" SortExpression="SupAnater" ReadOnly="True" HeaderText="Sup. ANATER">
                                <ItemStyle HorizontalAlign="right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SupGec" SortExpression="SupGec" ReadOnly="True" HeaderText="Sup. GEC">
                                <ItemStyle HorizontalAlign="right"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
        </table>
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
        <asp:Button ID="btnRicerca" Style="display: none" runat="server" Text="Ricerca"></asp:Button>
        <asp:Button ID="btnModificaRadio" Style="display: none" runat="server" Text="Button"></asp:Button>
        <asp:Button ID="btnPulisci" Style="display: none" runat="server" Text="Pulisci"></asp:Button>
        <asp:Button ID="btnExcel" Style="display: none" runat="server" Text="Excel"></asp:Button>
        <asp:Button ID="CmdUpdateFigli" runat="server" Style="display: none"></asp:Button>
    </form>
</body>
</html>
