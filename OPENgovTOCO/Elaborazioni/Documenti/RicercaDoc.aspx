<%@ Page Language="c#" CodeBehind="RicercaDoc.aspx.cs" AutoEventWireup="True" Inherits="OPENgovTOCO.Elaborazioni.Documenti.RicercaDoc" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>RicercaDoc</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
        var _changed = false
        function Search() {
            loadGrid.location.href = 'ResultDoc.aspx?NominativoDa=' + document.RicercaDocumenti.txtNominativoDa.value + '&NominativoA=' + document.RicercaDocumenti.txtNominativoA.value + '&CodiceCartella=' + document.RicercaDocumenti.txtCodiceCartella.value + '&IdRuolo=' + document.RicercaDocumenti.txtIdRuolo.value
        }
        function ElaborazioniDocumenti() {
            if (confirm('Si vuole procedere con l\'elaborazione dei documenti?')) {
                DivAttesa.style.display = '';
                __doPostBack();
            }
            else
                return false;
        }
        function ApprovaDocumenti() {
            if (confirm('Si vuole procedere con l\'approvazione dell\'elaborazione dei documenti?'))
                document.getElementById('CmdApprovaDoc').click()
        }
        function EliminaElaborazione() {
            if (confirm('Si vuole procedere con l\'eliminazione dei documenti effettivi già elaborati?'))
                document.getElementById('CmdEliminaDoc').click()
        }
        function ConfermaUscita() {
            document.getElementById('CmdUscita').click()
        }
        function ApriVisualizzaDocElaborati() {
            // apro il popup di visualizzazione doc elaborati
            winWidth = 980
            winHeight = 500
            myleft = (screen.availWidth - winWidth) / 2
            mytop = (screen.availheight - winHeight) / 2 - 40
            WinPopDoc = window.open('ViewDocumentiElaborati.aspx', '', 'width=' + winWidth + ',height=' + winHeight + ',top=' + mytop + ',left=' + myleft + ' status=yes, toolbar=no,scrollbar=no, resizable=no')
        }
        function BackToCalcolo() {
            parent.Comandi.location.href = '../Calcolo/ComandiCalcolo.aspx'
            parent.Visualizza.location.href = '../Calcolo/Calcolo.aspx'
        }
        function downloadAll() {
            document.getElementById("btnDownloadAll").click();
        }
        function ViewList() {
            dvDocumenti.style.display = 'none';
            dvElenco.style.display = '';
            _changed = true;
        }
        function ViewSearch() {
            if (_changed)
                _changed = false;
            else {
                dvDocumenti.style.display = '';
                dvElenco.style.display = 'none';
            }
        }
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post" class="FormNoComandi">
        <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: auto">
            <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                <tr valign="top">
                    <td><span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                        <asp:Label ID="lblTitolo" runat="server"></asp:Label></span></td>
                    <td valign="middle" align="right" rowspan="2">
                        <asp:ImageButton ID="btnEliminaElabora" runat="server" ImageUrl="../../../images/Bottoni/transparent28x28.png" Cssclass="Bottone Bottonecancella" ToolTip="Eliminazione Elaborazione"></asp:ImageButton>
                        <asp:ImageButton ID="btnApprovaDocumenti" runat="server" ImageUrl="../../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneFolderAccept" ToolTip="Approvazione Elaborazione" OnClick="btnApprovaDocumenti_Click"></asp:ImageButton>
                        <asp:ImageButton ID="btnStampaRate" runat="server" ImageUrl="../../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneExcel" ToolTip="Stampa Rate" OnClick="btnStampaRate_Click"></asp:ImageButton>
                        <asp:ImageButton ID="btnElaborazioneDocumenti" runat="server" ImageUrl="../../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneWord" ToolTip="Elaborazione Documenti" OnClientClick="document.getElementById('DivAttesa').style.display = '';document.getElementById('divStampa').style.display = '';document.getElementById('divVisual').style.display = 'none';" OnClick="btnElaborazioneDocumenti_Click"></asp:ImageButton>
                        <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="../../../images/Bottoni/transparent28x28.png" Cssclass="Bottone Bottonericerca" ToolTip="Ricerca" OnClick="btnSearch_Click"></asp:ImageButton>
                        <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="../../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneAnnulla" ToolTip="Uscita" OnClick="btnCancel_Click"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><span id="info" runat="server" class="NormalBold_title" style="width: 400px; height: 20px">TOSAP/COSAP - Elaborazione - Documenti</span></td>
                </tr>
            </table>
        </div>
        <table id="TblRicerca" cellspacing="1" cellpadding="1" width="100%" border="0">
            <tr>
                <td style="height: 70px">
                    <table id="TblDatiContribuente" cellspacing="0" cellpadding="0" width="100%" bgcolor="white"
                        border="1">
                        <tr>
                            <td>
                                <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                                    <tr>
                                        <td class="Input_Label" colspan="6" height="20"><strong>DATI RUOLO</strong></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente" style="width: 61px" width="61">Ruolo
                                        </td>
                                        <td class="DettagliContribuente" style="width: 157px" width="157">
                                            <asp:Label ID="lblTipoRuolo" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente" style="width: 152px" width="152">relativo all'anno
                                        </td>
                                        <td class="DettagliContribuente" style="width: 78px">
                                            <asp:Label ID="lblAnnoRuolo" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente" style="width: 152px">Cartellato in data
                                        </td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="lblDataCartellazione" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente" colspan="2">
                                            <asp:Label ID="lblDocElaborati" CssClass="DettagliContribuente" runat="server" Width="100%">Documenti Effettivi già elaborati:     </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNumeroDocElaborati" CssClass="DettagliContribuente" runat="server" Width="100%"></asp:Label>
                                        </td>
                                        <td class="DettagliContribuente" colspan="2">
                                            <asp:Label ID="lblDocDaElaborare" CssClass="DettagliContribuente" runat="server" Width="100%">Documenti da elaborare:     </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNumeroDocDaElaborare" CssClass="DettagliContribuente" runat="server" Width="100%"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:Label ID="lblElaborazioniEffettuate" runat="server" CssClass="NormalRed"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div id="divVisual" runat="server" class="col-md-12">
            <table id="TblParametriRicerca" cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td>
                        <fieldset class="FiledSetRicerca">
                            <legend class="Legend">Inserimento filtri di ricerca</legend>
                            <table id="TblParametri" cellspacing="1" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" CssClass="Input_Label" runat="server">Nominativo Da</asp:Label><br>
                                        <asp:TextBox ID="txtNominativoDa" runat="server" CssClass="Input_Text" Width="250px"></asp:TextBox></td>
                                    <td></td>
                                    <td>
                                        <asp:Label ID="Label4" CssClass="Input_Label" runat="server">Nominativo A</asp:Label><br>
                                        <asp:TextBox ID="txtNominativoA" runat="server" CssClass="Input_Text" Width="250px"></asp:TextBox></td>
                                    <td>
                                        <asp:Label ID="Label1" CssClass="Input_Label" runat="server">Codice Cartella</asp:Label><br>
                                        <asp:TextBox ID="txtCodiceCartella" runat="server" CssClass="Input_Text" Width="250px"></asp:TextBox></td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="Label6" Width="100%" runat="server" CssClass="lstTabRow">Cartelle Del Ruolo da Elaborare</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
                        <Grd:RibesGridView ID="GrdDocumenti" runat="server" BorderStyle="None"
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
                                <asp:TemplateField HeaderText="Cognome">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# GetContribuente(DataBinder.Eval(Container, "DataItem.DettaglioContribuente"), "Cognome") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nome">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label8" runat="server" Text='<%# GetContribuente(DataBinder.Eval(Container, "DataItem.DettaglioContribuente"), "Nome") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="N° Avviso" DataField="CodiceCartella">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Imp. Totale €" DataField="ImportoTotale" DataFormatString="{0:0.00}">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Imp. Arrotondamento €" DataField="ImportoArrotondamento" DataFormatString="{0:0.00}">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Imp. Carico €" DataField="ImportoCarico" DataFormatString="{0:0.00}">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Sel.">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkSelezionato" runat="server" AutoPostBack="True" Checked='<%# DataBinder.Eval(Container, "DataItem.Selezionato")%>' OnCheckedChanged="ChkSelezionato_CheckedChanged"></asp:CheckBox>
                                        <asp:HiddenField runat="server" ID="hfCodContribuente" Value='<%# Eval("CodContribuente") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Grd:RibesGridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="Table5" height="10" cellspacing="1" cellpadding="1" width="100%" border="0">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Label5" Width="100%" runat="server" CssClass="lstTabRow">Ordinamento</asp:Label>
                                </td>
                                <td colspan="6">
                                    <asp:Label ID="Label2" Width="100%" runat="server" CssClass="lstTabRow">Elaborazione</asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="lstTabRow" runat="server" ID="lblNumDocPerFile">N. File per Gruppo</asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="optIndirizzo" runat="server" CssClass="Input_Label" AutoPostBack="true" ToolTip="Ordinamento per indirizzo"
                                        GroupName="OptOrdinamento" Checked="true" Text="Indirizzo"></asp:RadioButton>
                                </td>
                                <td>
                                    <asp:RadioButton ID="optNominativo" runat="server" CssClass="Input_Label" AutoPostBack="true" ToolTip="Ordinamento per nominativo"
                                        GroupName="OptOrdinamento" Checked="False" Text="Nominativo"></asp:RadioButton>
                                </td>
                                <td>
                                    <asp:RadioButton ID="optProve" runat="server" CssClass="Input_Label" AutoPostBack="true" ToolTip="Elaborazione Prove"
                                        GroupName="OptElaborazione" Checked="true" Text="Prove"></asp:RadioButton>
                                </td>
                                <td>
                                    <asp:RadioButton ID="optEffettivo" runat="server" CssClass="Input_Label" AutoPostBack="true" ToolTip="Elaborazione effettiva"
                                        GroupName="OptElaborazione" Checked="False" Text="Effettivo"></asp:RadioButton>
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" Checked="False" CssClass="Input_Label" AutoPostBack="true" Text="Elabora Tutti"
                                        ID="chkElaboraTutti" OnCheckedChanged="chkElaboraTutti_CheckedChanged"></asp:CheckBox>
                                    <br />
                                    <asp:CheckBox ID="chkSendMail" runat="server" CssClass="Input_Label" Text="Invio tramite EMail" Visible="false" Checked="false"></asp:CheckBox>
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" Checked="False" CssClass="Input_Label" AutoPostBack="true" Text="Elabora Bollettini" ID="chkElaboraBollettini"></asp:CheckBox><br />
                                    <asp:CheckBox runat="server" Checked="False" CssClass="Input_Label" AutoPostBack="true" Text="Elabora Solo Bollettini" ID="chkSoloBollettino"></asp:CheckBox>
                                </td>
                                <td>
                                    <asp:RadioButton ID="optTipoBollettino451" runat="server" CssClass="Input_Label" AutoPostBack="True"
                                        GroupName="OptBollettino" Checked="True" Text="TD451"></asp:RadioButton>
                                </td>
                                <td>
                                    <asp:RadioButton ID="optTipoBollettino896" runat="server" CssClass="Input_Label" AutoPostBack="True"
                                        GroupName="OptBollettino" Checked="False" Text="TD896"></asp:RadioButton>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" CssClass="Input_Text_Right OnlyNumber" Width="70" AutoPostBack="true" onkeypress="return NumbersOnly(event);" ID="txtNumDoc">50</asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:ImageButton ID="CmdElaborazione" Style="display: none" runat="server"></asp:ImageButton>
            <asp:Button ID="CmdApprovaDoc" Style="display: none" runat="server"></asp:Button>
            <asp:Button ID="CmdEliminaDoc" Style="display: none" runat="server"></asp:Button>
            <asp:Button ID="CmdUscita" Style="display: none" runat="server"></asp:Button>
            <asp:TextBox ID="txtIdRuolo" Style="display: none" runat="server" AutoPostBack="True"></asp:TextBox>
        </div>
        <div id="dvElenco" runat="server" class="col-md-12 hidden">
            <table id="TblRisultati" cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td>
                        <asp:Label ID="Label9" runat="server" CssClass="Legend">Elenco Documenti Elaborati</asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" CssClass="NormalRed"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDownloadAll" runat="server" CssClass="Link_Label" onclick="downloadAll()">Scarica tutti i file</asp:Label>
                        <asp:Button ID="btnDownloadAll" runat="server" Text="Scarica tutti i file" Style="display: none" OnClick="btnDownloadAll_Click"></asp:Button>
                        <asp:TextBox ID="txtPath" runat="server" Style="display: none"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <Grd:RibesGridView ID="GrdElenco" runat="server" BorderStyle="None"
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
                                <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Nome Documento">
                                    <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <a href='<%# DataBinder.Eval(Container, "DataItem.Url") %>' target="_blank">Apri</a>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <a href='<%# DataBinder.Eval(Container, "DataItem.Url") %>' target="_blank">Apri</a>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Grd:RibesGridView>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divStampa" class="col-md-12 classeFiledSetRicerca" style="display:none;">
            <iframe runat="server" id="loadStampa" name="loadStampa" frameborder="0" width="100%" height="250px" src="../../../aspvuotaremovecomandi.aspx"></iframe>
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
        <asp:HiddenField ID="hfIdFlusso" runat="server" Value="0" />
        <asp:HiddenField ID="hfAnno" runat="server" Value="0" />
        <asp:HiddenField ID="hfTipoRuolo" runat="server" Value="0" />
        <asp:HiddenField ID="hfDocTot" runat="server" Value="0" />
        <asp:HiddenField ID="hfDocElab" runat="server" Value="0" />
    </form>
</body>
</html>
