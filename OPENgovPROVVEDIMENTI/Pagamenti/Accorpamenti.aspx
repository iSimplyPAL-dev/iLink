<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Accorpamenti.aspx.vb" Inherits="Provvedimenti.Accorpamenti" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Accorpamenti</title>
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
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" src="../../_js/skype_killer.js?newversion"></script>
    <script type="text/javascript">
        window.onload = killSkype;
        function DeleteContrib() {
            document.getElementById('txtNominativo').value = '';
            document.getElementById('hdIdContribuente').value = '-1';
            document.getElementById('txtHiddenIdDataAnagrafica').value = '-1';
            document.getElementById("btnPulisciGriglia").click();
            AbilitafldPagamento('none');
            Abilita_fldRate('none');
            Abilita_btnRateizza('none');
            NascondiTuttiBottoni();
        }

        function ApriRicercaAnagrafe(nomeSessione) {
            winWidth = 980
            winHeight = 680
            myleft = (screen.width - winWidth) / 2
            mytop = (screen.height - winHeight) / 2 - 40
            caratteristiche = "toolbar=no,status,left=" + myleft + ",top=" + mytop + ",height =" + winHeight + ",width=" + winWidth + ",resizable"
            Parametri = "sessionName=" + nomeSessione
            WinPopUpRicercaAnagrafica = window.open("../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?" + Parametri, "", caratteristiche)
            return false;
        }

        function RateizzaSelezionati() {
            AbilitafldPagamento('none')
            Abilita_fldRate('none')
            document.getElementById("btnRateizzaSelezionati").click()
        }
        function AbilitafldPagamento(valore) {
            document.getElementById("fldPagamento").style.display = valore
        }
        function Abilita_fldRate(valore) {
            document.getElementById("fldRate").style.display = valore
        }
        function Abilita_btnCaricaRateizzazioni(valore) {
            parent.Comandi.Abilita_btnCaricaRateizzazioni(valore)
        }
        function Abilita_btnRateizza(valore) {
            parent.Comandi.Abilita_btnRateizza(valore)
        }
        function Abilita_btnCalcolaTotale(valore) {
            parent.Comandi.Abilita_btnCalcolaTotale(valore)
        }
        function Abilita_btnSalvaRate(valore) {
            parent.Comandi.Abilita_btnSalvaRate(valore)
        }
        function NascondidgdRateizzazioni() {
            document.getElementById("btnNascondidgdRateizzazioni").click()
        }

        function NascondiTuttiBottoni() {
            Abilita_btnSalvaRate('none')
            Abilita_btnCalcolaTotale('none')
            Abilita_btnCaricaRateizzazioni('none')
        }

        function CambiaRate() {
            Abilita_btnSalvaRate('none');
            Abilita_btnCalcolaTotale('none');
            Abilita_fldRate('none')
        }
        function CalcolaRateizzazioni() {
            Abilita_btnSalvaRate('none')

            if (document.getElementById("txtNumRate").value == "") {
                GestAlert('a', 'warning', '', '', 'Inserimento obbligatorio del numero di rate')
            }
            else {
                if (!isNumber(document.getElementById("txtNumRate").value, 10, 0, 1, 'undefined')) {
                    GestAlert('a', 'warning', '', '', 'Inserire un numero intero positivo')
                    document.getElementById("txtNumRate").value = ""
                    document.getElementById("txtNumRate").focus()
                }
                else {
                    if (document.getElementById("ddlInteressi").value == "-1") {
                        GestAlert('a', 'warning', '', '', 'Selezionare un Interesse')
                    }
                    else {
                        /*if (document.getElementById("ckPolizza").checked==true){
                            if (document.getElementById("txtPolizza").value==""){
                                GestAlert('a', 'warning', '', '', 'Inserire il valore della Polizza Fideiussoria')
                            }
                            else{
                                if (!isNumber(document.getElementById("txtPolizza").value, 4, 2, 1, 100000)){
                                    alert ("Inserire un numero positivo con i decimali separati dalla virgola")
                                    document.getElementById("txtPolizza").value=""
                                    document.getElementById("txtPolizza").focus()
                                }
                                else{
                                    document.getElementById ("btnCreaRate").click ();
                                    
                                }
                            }
                        }
                        else{*/
                        document.getElementById("btnCreaRate").click();
                        //}
                    }
                }
            }
        }
        function Totale() {
            document.getElementById("btnCalcolaRate").click();
        }
        function SalvaRateizzazioni() {
            document.getElementById("btnSalvaRateizzazioni").click();
        }
        function InserimentoPagamenti() {
            if (confirm('Salvataggio effettuato correttamente.\n\nSi desidera inserire il pagamento?')) {
                parent.Comandi.location.href = "cmdPagamenti.aspx?from=Dettaglio"
                parent.Visualizza.location.href = "Pagamenti.aspx?from=Dettaglio"
            } else {
                document.getElementById("btnSearchProvvedimenti").click();
            }
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <table id="tablebb" border="0" cellpadding="0" width="100%">
            <!--blocco dati contribuente-->
            <%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
            <tr id="TRPlainAnag">
                <td>
                    <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height: 200px; width: 100%" frameborder="0" marginheight="0"></iframe>
                    <asp:HiddenField ID="hdIdContribuente" runat="server" Value="-1" />
                </td>
            </tr>
            <tr id="TRSpecAnag">
                <td>
                    <table>
                        <tr>
                            <td class="Input_Label">
                                <asp:Label ID="lblNominativo" runat="server">Nominativo</asp:Label>&nbsp;
						            <asp:Button ID="btnFocus" runat="server" Width="1px" Height="1px"></asp:Button>
                                <asp:ImageButton ID="Imagebutton" runat="server" CausesValidation="False" class="BottoneSel BottoneLista"></asp:ImageButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtNominativo" TabIndex="4" runat="server" Width="492px" ToolTip="Nominativo"
                                    CssClass="Input_Text" Enabled="False"></asp:TextBox>
                                <img id="imageDelete" onmouseover="this.style.cursor='hand'" onclick="DeleteContrib();"
                                    alt="Pulisci Nominativo Selezionato" src="../images/cancel.png" width="10px" height="10px">
                                <asp:Button ID="btnRibalta" runat="server" Width="1px" Height="2px" CssClass="displaynone" Text="Ribalta"></asp:Button>
                                <asp:TextBox ID="txtHiddenIdDataAnagrafica" runat="server" Width="24px" Height="20px" CssClass="displaynone"></asp:TextBox>
                                <asp:Button ID="btnSearchProvvedimenti" runat="server" Width="1px" Height="2px" CssClass="displaynone"
                                    Text="Provvedimenti"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <p>
            <asp:Label ID="lblInfoProvv" runat="server" CssClass="Input_Label"></asp:Label></p>
        <Grd:RibesGridView ID="GrdProvvedimenti" runat="server" BorderStyle="None"
            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="99%"
            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
            OnRowDataBound="GrdRowDataBound">
            <PagerSettings Position="Bottom"></PagerSettings>
            <PagerStyle CssClass="CartListFooter" />
            <RowStyle CssClass="CartListItem"></RowStyle>
            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
            <Columns>
                <asp:BoundField DataField="anno" ReadOnly="True" HeaderText="Anno" SortExpression="Anno">
                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="numero_atto" ReadOnly="True" HeaderText="N. Atto" SortExpression="numero_atto">
                    <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Tributo">
                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# DescTributo(DataBinder.Eval(Container, "DataItem.cod_tributo")) %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Importo Ridotto &euro;" SortExpression="Importo_totale_ridotto">
                    <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblImporto_Totale_Ridotto" runat="server" Text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.Importo_totale_ridotto"),2) %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Importo &euro;" SortExpression="Importo_totale">
                    <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblImporto_Totale" runat="server" Text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.Importo_totale"),2) %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data Elaborazione" SortExpression="data_elaborazione">
                    <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDataElab" runat="server" Text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_elaborazione")) %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data Notifica" SortExpression="data_notifica_avviso">
                    <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDataNot" runat="server" Text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_notifica_avviso")) %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data Stampa" SortExpression="data_stampa">
                    <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDataStampa" runat="server" Text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_stampa")) %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data Consegna" SortExpression="data_consegna_avviso">
                    <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDataCons" runat="server" Text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_consegna_avviso")) %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data Annullamento" SortExpression="data_annullamento_avviso">
                    <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDataAnn" runat="server" Text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_annullamento_avviso")) %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ProvProc" ReadOnly="True" HeaderText="Gruppo">
                    <HeaderStyle HorizontalAlign="Center" Width="13%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Sel">
                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSeleziona" runat="server" readonly="true" Checked='<%# DataBinder.Eval(Container, "DataItem.Selezionato")%>'></asp:CheckBox>
                        <asp:HiddenField runat="server" ID="hfCOD_TRIBUTO" Value='<%# Eval("COD_TRIBUTO") %>' />
                        <asp:HiddenField runat="server" ID="hfid_provvedimento" Value='<%# Eval("id_provvedimento") %>' />
                        <asp:HiddenField runat="server" ID="hfID_ACCORPAMENTO" Value='<%# Eval("ID_ACCORPAMENTO") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Grd:RibesGridView>
        <br>
        <fieldset id="fldPagamento" style="display: none">
            <legend class="Legend">Dati Rateizzazioni</legend>
            <br>
            <table border="0" cellpadding="2" cellspacing="0">
                <tr class="Input_Label bold">
                    <td>
                        <asp:RadioButton ID="OptRidotto" GroupName="DaRateizzare" runat="server" Checked="True"></asp:RadioButton>
                        <asp:RadioButton ID="OptPieno" GroupName="DaRateizzare" runat="server"></asp:RadioButton>
                    </td>
                    <!--<td>Totale Ridotto da Rateizzare:
							<asp:label id="ImpTotRateizzare" runat="server"></asp:label>€
						</td>
						<td>Totale Pieno da Rateizzare:
							<asp:label id="ImpTotRateizzarePieno" runat="server"></asp:label>€
						</td>-->
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNRate" runat="server" CssClass="Input_Label">Numero Rate</asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtNumRate" runat="server" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 0);" onchange="CambiaRate()" Width="50px"></asp:TextBox></td>
                </tr>
                <tr class="Input_Label">
                    <td>Interessi</td>
                    <td>
                        <asp:DropDownList ID="ddlInteressi" runat="server" onchange="CambiaRate()" CssClass="Input_Label"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" CssClass="Input_Label">Data Inizio Calcolo Interessi</asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtDataInizioInteressi" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Label1" runat="server" CssClass="Input_Label bold">Gestione Calcolo Date Rate</asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" CssClass="Input_Label">Data Inizio Rate</asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtDataInizioRate" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onchange="CambiaRate()"></asp:TextBox>
                        <asp:Label ID="Label5" runat="server" CssClass="Input_Label"></asp:Label>
                    </td>
                </tr>
                <tr class="Input_Label">
                    <td>Intervallo Rate</td>
                    <td>
                        <asp:DropDownList ID="ddlIntervallo" runat="server" onchange="CambiaRate()" CssClass="Input_Label"></asp:DropDownList>
                        <asp:DropDownList ID="ddlTipoIntervallo" runat="server" onchange="CambiaRate()" CssClass="Input_Label"></asp:DropDownList>
                    </td>
                </tr>
            </table>
            <!--<asp:checkbox id="ckInteressi" cssclass="Input_Label w20" runat="server" text=" Interessi " checked="True"></asp:checkbox>-->
            <asp:CheckBox ID="ckPolizza" CssClass="Input_Label w20" runat="server" Text=" Polizza Fideiussoria pari a € "
                onclick="CambiaRate()" Visible="False"></asp:CheckBox>
            <asp:TextBox ID="txtPolizza" runat="server" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);" onchange="CambiaRate()" Visible="False"></asp:TextBox>
        </fieldset>
        <br>
        <br>
        <fieldset id="fldRate" style="display: none">
            <legend class="Legend">Elenco Rate</legend>
            <Grd:RibesGridView ID="GrdRateizzazioni" runat="server" BorderStyle="None"
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="99%"
                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                <Columns>
                    <asp:TemplateField HeaderText="N&#176; Rata">
                        <HeaderStyle Width="50px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblRate" Text='<%# DataBinder.Eval(Container, "DataItem.NUMERO_RATA") %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importo">
                        <HeaderStyle Width="150px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblImportoRate" Text='<%# EuroForGridView(DataBinder.Eval(Container, "DataItem.IMPORTO_RATA")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Scadenza">
                        <HeaderStyle Width="150px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtDataScadenza" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" Text='<%# GiraDataFromDB(DataBinder.Eval(Container, "DataItem.SCADENZA")) %>'>
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Interessi">
                        <HeaderStyle Width="150px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtInteressi" runat="server" Width="100px" CssClass="Input_Text_Right" Text='<%# EuroForGridView(DataBinder.Eval(Container, "DataItem.IMPORTO_INTERESSI")) %>'></asp:TextBox>						
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Totale">
                        <HeaderStyle Width="150px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtTotale" runat="server" Width="100px" CssClass="Input_Text_Right" Text='<%# EuroForGridView(DataBinder.Eval(Container, "DataItem.IMPORTO_TOTALE")) %>'>
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Grd:RibesGridView>
        </fieldset>
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
        <asp:TextBox ID="txtRateCalc" Style="display: none" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtSelezionato" runat="server" CssClass="displaynone"></asp:TextBox>
        <asp:TextBox ID="txtID_PROVVEDIMENTO" runat="server" CssClass="displaynone"></asp:TextBox>
        <asp:TextBox ID="txtID_ACCORPAMENTO" runat="server" CssClass="displaynone"></asp:TextBox>
        <asp:Button ID="btnSelezionaProvvedimenti" runat="server" Width="1px" Height="2px" CssClass="displaynone"
            Text="SelezionaProvvedimenti"></asp:Button>
        <asp:Button ID="btnPulisciGriglia" runat="server" Width="1px" Height="2px" CssClass="displaynone"
            Text="btnPulisciGriglia"></asp:Button>
        <asp:Button ID="btnRateizzaSelezionati" runat="server" Width="1px" Height="2px" CssClass="displaynone"
            Text="btnRateizzaSelezionati"></asp:Button>
        <asp:Button ID="btnCreaRate" runat="server" Width="1px" Height="2px" CssClass="displaynone"
            Text="btnCreaRate"></asp:Button>
        <asp:Button ID="btnCalcolaRate" runat="server" Width="1px" Height="2px" CssClass="displaynone"
            Text="btnCalcolaRate"></asp:Button>
        <asp:Button ID="btnSalvaRateizzazioni" runat="server" Width="1px" Height="2px" CssClass="displaynone"
            Text="btnSalvaRateizzazioni"></asp:Button>
        <asp:Button ID="btnNascondidgdRateizzazioni" runat="server" Width="1px" Height="2px" CssClass="displaynone"
            Text="btnNascondidgdRateizzazioni"></asp:Button>
    </form>
</body>
</html>
