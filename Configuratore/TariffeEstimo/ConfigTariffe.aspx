<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfigTariffe.aspx.vb" Inherits="OPENgov.ConfigTariffe" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ConfigTariffe</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" type="text/javascript">
        function ConfermaElimina() {
            if (document.getElementById('txtID').value != '') {
                if (confirm('Si desidera eliminare la tariffa selezionata?')) {
                    return true;
                } else {
                    return false;
                }
            } else {
                GestAlert('a', 'warning', '', '', 'Selezionare la Tariffa che si desidera eliminare!');
                return false;
            }
        }

        function Search() {
            document.getElementById('btnRicerca').click();
        }

        function Delete() {
            document.getElementById('btnElimina').click();
        }

        function Nuovo() {
            document.getElementById('btnNuovo').click();
        }
        function Salva() {
            // controllo che i campi obbligatori siano stati valorizzati
            var CampiObbligatori = '';
            if (document.getElementById('radioTabella_0').checked) {
                /*alert('0');*/
                if (document.getElementById('txtDataInizio').value == '') {
                    CampiObbligatori += '\n-Data Inizio Validità';
                }
                if (document.getElementById('txtZona').value == '') {
                    CampiObbligatori += '\n-Zona';
                }
                if (document.getElementById('txtTariffa').value == '') {
                    CampiObbligatori += '\n-Tariffa';
                }
            }

            if (document.getElementById('radioTabella_1').checked) {
                if (document.getElementById('txtZona').value == '') {
                    CampiObbligatori += '\n-Zona';
                }
                if (document.getElementById('txtTariffa').value == '') {
                    CampiObbligatori += '\n-Tariffa';
                }
                if (document.getElementById('txtDataInizio').value == '') {
                    CampiObbligatori += '\n-Data Inizio Validità';
                }
                /*alert(document.getElementById('ddlCategoria').value);*/
                if (document.getElementById('ddlCategoria').value == '...') {
                    CampiObbligatori += '\n-Categoria';
                }
                if (document.getElementById('ddlClasse').value == '...') {
                    CampiObbligatori += '\n-Classe';
                }
            }

            if (CampiObbligatori != '') {
                GestAlert('a', 'warning', '', '', 'Attenzione, i seguenti campi sono obbligatori.' + CampiObbligatori);
            } else {
                document.getElementById('btnSalva').click();
            }
        }
    </script>
</head>
<body class="Sfondo" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post">
        <table id="Table1" style="z-index: 101; left: 8px; position: absolute; top: 8px" cellpadding="0"
            width="100%" border="0">
            <!--<tr>
					<td style="HEIGHT: 16px" colSpan="2">
						<asp:label id="lblDescrizioneOperazione" runat="server" CssClass="lstTabRow" Width="100%">Configurazione Tariffe Estimo </asp:label>&nbsp;
					</td>
				</tr>-->
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="radioTabella" runat="server" CssClass="Input_Label" AutoPostBack="true">
                                    <asp:ListItem Selected="true" Value="TabEstimoFab">Valori Aree Edificabili</asp:ListItem>
                                    <asp:ListItem Value="TabEstimo">Estimo Catastale</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 130px">
                                <asp:Label ID="lblDataInizio" CssClass="Input_Label" runat="server">Data Inizio Validità:</asp:Label><br>
                                <asp:TextBox ID="txtID" Style="display: none;" runat="server" CssClass="Input_Text" Width="100px" MaxLength="10"></asp:TextBox>
                                <asp:TextBox ID="txtDataInizio" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" MaxLength="10"></asp:TextBox></td>
                            <td style="width: 120px">
                                <asp:Label ID="lblDataFine" CssClass="Input_Label" runat="server">Data Fine Validità:</asp:Label><br>
                                <asp:TextBox ID="txtDataFine" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 130px">
                                <asp:Label ID="lblZona" CssClass="Input_Label" runat="server">Zona:</asp:Label><br>
                                <asp:TextBox ID="txtZona" runat="server" CssClass="Input_Text" Width="100px" MaxLength="10"></asp:TextBox></td>
                            <td style="width: 130px">
                                <asp:Label ID="lblCategoria" CssClass="Input_Label" runat="server" Enabled="False">Categoria Catastale:</asp:Label><br>
                                <asp:DropDownList ID="ddlCategoria" CssClass="Input_Text" runat="server" Width="120px" Enabled="False"></asp:DropDownList></td>
                            <td style="width: 130px">
                                <asp:Label ID="lblClasse" CssClass="Input_Label" runat="server" Enabled="False">Classe:</asp:Label><br>
                                <asp:DropDownList ID="ddlClasse" CssClass="Input_Text" runat="server" Width="120px" Enabled="False"></asp:DropDownList></td>
                            <td style="width: 130px">
                                <asp:Label ID="lblTariffa" CssClass="Input_Label" runat="server">Tariffa €:</asp:Label><br>
                                <asp:TextBox onkeypress="return NumbersOnly(event, true, false, 2);" ID="txtTariffa" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="100px" MaxLength="10"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" CssClass="Input_Label" runat="server">Note:</asp:Label><br>
                                <asp:TextBox ID="txtNote" CssClass="Input_Text" runat="server" Width="512px" TextMode="MultiLine" Height="64px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Label ID="lblRisultati" CssClass="Input_Label" runat="server" Visible="False">La ricerca effettuata non ha prodotto risultati.</asp:Label>
                    <Grd:RibesGridView ID="GrdTariffeFab" runat="server" BorderStyle="None"
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
                            <asp:TemplateField HeaderText="TARIFFA">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblTariffaGrd" runat="server" Text='<%# FormattaDecimale(DataBinder.Eval(Container, "DataItem.Tariffa_EURO")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ZONA" HeaderText="ZONA"></asp:BoundField>
                            <asp:TemplateField HeaderText="DATA INIZIO VALIDITA">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblDataInizioGrid" runat="server" Text='<%# FormattaData(DataBinder.Eval(Container, "DataItem.DataDal")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DATA FINE VALIDITA">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblDataFineGrid" runat="server" Text='<%# FormattaData(DataBinder.Eval(Container, "DataItem.DataAl")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField runat="server" ID="hfNOTEFab" Value='<%# Eval("NOTE") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                    <Grd:RibesGridView ID="GrdTariffeUrbane" runat="server" BorderStyle="None"
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
                            <asp:TemplateField HeaderText="TARIFFA">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblTariffaGrdUrb" runat="server" Text='<%# FormattaDecimale(DataBinder.Eval(Container, "DataItem.Tariffa_EURO")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ZONA" HeaderText="ZONA"></asp:BoundField>
                            <asp:TemplateField HeaderText="DATA INIZIO VALIDITA">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblDataInizioGridUrb" runat="server" Text='<%# FormattaData(DataBinder.Eval(Container, "DataItem.DataDal")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DATA FINE VALIDITA">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblDataFineGridUrb" runat="server" Text='<%# FormattaData(DataBinder.Eval(Container, "DataItem.DataAl")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="categoria" HeaderText="CATEGORIA"></asp:BoundField>
                            <asp:BoundField DataField="CLASSE" HeaderText="CLASSE"></asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField runat="server" ID="hfNOTEUrb" Value='<%# Eval("NOTE") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSalva" Style="display: none;" runat="server" Text="salva"></asp:Button>
                    <asp:Button ID="btnRicerca" runat="server" Style="display: none;" Text="cerca"></asp:Button>
                    <asp:Button ID="btnNuovo" runat="server" Style="display: none;" Text="Nuovo"></asp:Button>
                    <asp:Button ID="btnElimina" Style="display: none;" runat="server" Text="elimina"></asp:Button>
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
    </form>
</body>
</html>
