<%@ Page Language="c#" CodeBehind="ElaborazioneDocumenti.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.ElaborazioneDocumenti.ElaborazioneDocumenti" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <!--*** 20120704 - IMU ***-->
    <title>Dettaglio Contribuente Calcolo ICI/IMU</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" type="text/javascript">
        function ApriTipiRendita() {
            var obj = document.getElementById('divTipiRendita');
            if (obj.style.display == '')
                obj.style.display = 'none'
            else
                obj.style.display = ''

        }
        function ControllaNumeroUtenti(oggetto) {
            if (oggetto.value != "") {
                if (parseInt(oggetto.value) > 500) {
                    GestAlert('a', 'warning', '', '', 'Il numero massimo di contribuenti per documento è 500.');
                    oggetto.value = ""
                    oggetto.focus();
                    return false;
                }
            }
        }

        function ApriPopUpStampaDocumenti() {
            //window.open('DownloadDocumenti.aspx?idFlussoRuolo=' + document.getElementById('txtIdFlussoRuolo').value, 'DownloadDoc', '');
            document.getElementById('DivAttesa').style.display = 'none';
            document.getElementById('divStampa').style.display = ''; 
            document.getElementById('divElabDoc').style.display = 'none';
            document.getElementById('loadStampa').src = 'DownloadDocumenti.aspx?Provenienza=E&idFlussoRuolo=' + document.getElementById('txtIdFlussoRuolo').value;
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <fieldset class="col-md-12 classeFiledSetRicerca">
            <legend class="Legend">Riepilogo Elaborazioni Effettuate</legend>
            <table>
                <tr>
                    <td class="Input_Label" align="left">Anno Riferimento<br />
                        <asp:TextBox ID="txtAnno" runat="server" CssClass="Input_Text_Right OnlyNumber" ReadOnly="true" Width="90px"></asp:TextBox>
                    </td>
                    <!--*** 20140509 - TASI ***-->
                    <td>
                        <asp:CheckBox ID="chkICI" runat="server" CssClass="Input_Label" Text="ICI/IMU" Checked="true" Enabled="false" oncheckedchanged="Tributo_CheckedChanged"/>
                        <br />
                        <asp:CheckBox ID="chkTASI" runat="server" CssClass="Input_Label" Text="TASI" Checked="true" Enabled="false" oncheckedchanged="Tributo_CheckedChanged"/>
                    </td>
                    <!--*** ***-->
					<!--*** 20150430 - TASI Inquilino ***-->
					<td>
					    <asp:CheckBox ID="chkProp" runat="server" CssClass="Input_Label" AutoPostBack="true" Text="Proprietario" Checked="true" oncheckedchanged="Tributo_CheckedChanged"/>
					    <br />
					    <asp:CheckBox ID="chkInquilino" runat="server" CssClass="Input_Label" AutoPostBack="true" Text="Inquilino" Checked="true" oncheckedchanged="Tributo_CheckedChanged"/>
					</td>
					<!--*** ***-->
                    <td>
                        <asp:CheckBox ID="chkVersatoNelDovuto" runat="server" CssClass="Input_Label" Text="Versato dell'Anno precedente ribaltato nel dovuto" Enabled="false"></asp:CheckBox>&nbsp;&nbsp;
                        <br />
                        <asp:CheckBox ID="chkArrotondamento" runat="server" CssClass="Input_Label" Text="Calcola Arrotondamento" Enabled="false"></asp:CheckBox>
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbStandard" runat="server" CssClass="Input_Label" Text="Calcolo standard" Checked="True" GroupName="OptTipoCalcolo" Enabled="false"></asp:RadioButton>
                        <br />
                        <asp:RadioButton ID="rdbCalcoloNetto" runat="server" CssClass="Input_Label" Text="Calcolo al netto del Versato" GroupName="OptTipoCalcolo" Enabled="false"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblElaborazioniEffettuate" runat="server" CssClass="NormalRed"></asp:Label>
                    </td>
                    <td>
                        <a id="lnkScaricaDocEffettiviElab" class="Input_Label_bold" href="javascript:ApriPopUpStampaDocumenti();" runat="server" style="display: none">Scarica Documenti Effettivi Elaborati »</a>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
            <div class="BottoneClessidra">&nbsp;</div>
            <div class="Legend">Attendere Prego</div>
        </div>
        <div id="divElabDoc" class="col-md-12">
            <fieldset class="col-md-12 classeFiledSetRicerca" style="padding-right: 0px; margin-right: 0px">
                <!--*** 20120704 - IMU ***-->
                <legend class="Legend">Inserimento filtri di ricerca</legend>
                <table cellpadding="5" width="100%">
                    <!--<tr>
                    <td><label class="Input_Label">Nominativo:</label></td>
                    <td><asp:textbox id=txtNominativo Runat="server" cssclass="Input_Text"></asp:textbox></td>
                    </tr>-->
                    <tr>
                        <td>
                            <label class="Input_Label"> Nominativo Da</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNominativoDa" runat="server" CssClass="Input_Text"></asp:TextBox>
                        </td>
                        <td>
                            <label class="Input_Label"> Nominativo A</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNominativoA" runat="server" CssClass="Input_Text"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RadioButton Style="display: none" ID="optAll" GroupName="TypeElab" runat="server" CssClass="Input_Radio" Text="Tutti" Checked="true" />&nbsp
                            <asp:RadioButton Style="display: none" ID="optToElab" GroupName="TypeElab" runat="server" CssClass="Input_Radio" Text="Da Elaborare" />&nbsp
                            <asp:RadioButton Style="display: none" ID="optAlreadyElab" GroupName="TypeElab" runat="server" CssClass="Input_Radio" Text="Elaborati" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <iframe id="iFrameDaElaborare" src="AvanzamentoElaborazione.aspx" frameborder="0" width="100%" height="400px"></iframe>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset class="col-md-12 classeFiledSetRicerca" style="padding-right: 0px; margin-right: 0px">
                <legend class="Legend">Parametri Elaborazione</legend>
                <table cellpadding="5">
                    <tr>
                        <td valign="top">
                            <label class="Input_Label">Ordinamento:</label>
                        </td>
                        <td valign="top">
                            <asp:RadioButton ID="radioIndirizzo" onclick="btnRicerca.click()" runat="server" CssClass="Input_Label" GroupName="TipoOrdinamento" Text="Indirizzo"></asp:RadioButton>
                        </td>
                        <td valign="top">
                            <asp:RadioButton ID="radioNominativo" onclick="btnRicerca.click()" runat="server" CssClass="Input_Label" GroupName="TipoOrdinamento" Text="Nominativo" Checked="true"></asp:RadioButton>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td style="width: 160px" valign="top">
                            <label class="Input_Label">Contribuenti per documento</label>
                        </td>
                        <td valign="top">
                            <a class="Input_Label" href="javascript:ApriTipiRendita();">Tipi Rendita da Escludere »</a>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <label class="Input_Label">Tipo Elaborazione:</label>
                        </td>
                        <td valign="top">
                            <asp:RadioButton ID="radioProva" runat="server" CssClass="Input_Label" GroupName="TipoElaborazione" Text="Prova" Checked="True" AutoPostBack="true" OnCheckedChanged="radioProva_CheckedChanged"></asp:RadioButton>
                        </td>
                        <td valign="top">
                            <asp:RadioButton ID="radioEffettivo" runat="server" CssClass="Input_Label" GroupName="TipoElaborazione" Text="Effettivo" AutoPostBack="true" OnCheckedChanged="radioEffettivo_CheckedChanged"></asp:RadioButton>
                        </td>
                        <td valign="top">
                            <asp:CheckBox ID="chkSelTutti" runat="server" CssClass="Input_Label" Text="Seleziona Tutti" AutoPostBack="true" Visible="false" OnCheckedChanged="chkSelTutti_CheckedChanged"></asp:CheckBox>
                        </td>
                        <td valign="top">
                            <asp:CheckBox ID="chkSendMail" runat="server" CssClass="Input_Label" Text="Invio tramite EMail" Visible="false" Checked="false"></asp:CheckBox>
                        </td>
                        <td valign="top" align="center">
                            <asp:TextBox onkeypress="return NumbersOnly(event, false, false, 0);" ID="txtNumContrib" runat="server" CssClass="Input_Text_Right OnlyNumber" value="50" onchange="ControllaNumeroUtenti(this);" MaxLength="2" Width="40"></asp:TextBox>
                        </td>
                        <td valign="top">
                            <div id="divTipiRendita" style="display: none">
                                <Grd:RibesGridView ID="GrdTipoRendita" runat="server" BorderStyle="None" 
                                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                    AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                    <PagerSettings Position="Bottom"></PagerSettings>
                                    <PagerStyle CssClass="CartListFooter" />
                                    <RowStyle CssClass="CartListItem"></RowStyle>
                                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="SIGLA" HeaderText="Tipo Rendita">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="100px" VerticalAlign="Middle"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="COD_RENDITA" HeaderText="Cod Rendita" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="100px" VerticalAlign="Middle"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIZIONE" HeaderText="Tipo Rendita">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Escludi">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" VerticalAlign="Middle"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkEsclusione" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </Grd:RibesGridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset class="col-md-12 classeFiledSetRicerca" style="padding-right: 0px;margin-right: 0px">
                <legend class="Legend">Parametri Gestione Bollettini</legend>
                <table cellpadding="5">
                    <tr>
                        <td>
                            <asp:RadioButton ID="radioTuttiBollettini" runat="server" CssClass="Input_Label"
                                Text="Standard Acconto/Saldo" AutoPostBack="false" GroupName="opzBollettini"
                                Checked="True"></asp:RadioButton>
                        </td>
                        <td>
                            <asp:RadioButton ID="radioSoloAcconto" runat="server" CssClass="Input_Label" Text="Solo Acconto"
                                AutoPostBack="false" GroupName="opzBollettini" Checked="false"></asp:RadioButton>
                        </td>
                        <td>
                            <asp:RadioButton ID="radioSoloSaldo" runat="server" CssClass="Input_Label" Text="Solo Saldo"
                                AutoPostBack="false" GroupName="opzBollettini" Checked="false"></asp:RadioButton>
                        </td>
                        <td>
                            <asp:RadioButton ID="radioBollettiniSenzaImporti" runat="server" CssClass="Input_Label"
                                Text="Bollettini Senza Importi" AutoPostBack="false" GroupName="opzBollettini">
                            </asp:RadioButton>
                        </td>
                        <td>
                            <asp:RadioButton ID="radioNoBollettini" runat="server" CssClass="Input_Label" Text="Nessun Bollettino"
                                AutoPostBack="false" GroupName="opzBollettini"></asp:RadioButton>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div id="divStampa" class="col-md-12 classeFiledSetRicerca" style="display:none;">
            <iframe runat="server" id="loadStampa" name="loadStampa" frameborder="0" width="100%" height="250px" src="../../aspvuota.aspx"></iframe>
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
        <input id="txtIdFlussoRuolo" type="hidden" name="txtIdFlussoRuolo" runat="server" value="-1" />
        <asp:Button ID="btnRicerca" Style="display: none" runat="server" Text="Ricerca" OnClick="btnRicerca_Click"> </asp:Button>
        <asp:Button ID="btnElimina" runat="server" Text="Elimina Elaborazioni" Style="display: none" OnClick="btnElimina_Click"></asp:Button>
        <asp:Button ID="btnElabora" runat="server" Text="Elabora" Style="display: none" OnClick="btnElabora_Click"> </asp:Button>
        <asp:Button ID="btnClose" runat="server" Text="chiudi" Style="display: none" OnClick="btnClose_Click"></asp:Button>
    </form>
</body>
</html>
