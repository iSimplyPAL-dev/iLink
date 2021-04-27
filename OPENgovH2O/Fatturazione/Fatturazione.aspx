<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Fatturazione.aspx.vb" Inherits="OpenUtenze.Fatturazione" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Fatturazione</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
        function ElaborazioneDocumenti() {
            document.getElementById('CmdElaborazioniDocumenti').click()
        }

        function NumerazioneFatturazione() {
            //devo avere il numero di partenza
            if (document.getElementById('TxtNIniziale').value == '') {
                alert("E\' necessario inserire il numero di partenza delle fatture!");
                Setfocus(document.getElementById('TxtNIniziale'));
                return false;
            }
            else {
                sGG = document.getElementById('TxtNIniziale').value;
                if (!isNumber(sGG)) {
                    alert("Inserire solo NUMERI nel campo Numero Iniziale Fattura!");
                    Setfocus(document.getElementById('TxtNIniziale'));
                    return false;
                }
            }
            //devo avere la data del documento
            if (document.getElementById('TxtDataFattura').value == '') {
                alert("E\' necessario valorizzare il campo Data di Emissione!");
                Setfocus(document.getElementById('TxtDataFattura'));
                return false;
            }
            else {
                if (!isDate(document.getElementById('TxtDataFattura').value)) {
                    alert("Inserire la Data di Emissione correttamente in formato: GG/MM/AAAA!");
                    Setfocus(document.getElementById('TxtDataFattura'));
                    return false;
                }
            }
            //devo avere la data di scadenza		
            if (document.getElementById('TxtDataScadenza').value == '') {
                alert("E\' necessario valorizzare il campo Data di Scadenza!");
                Setfocus(document.getElementById('TxtDataScadenza'));
                return false;
            }
            else {
                if (!isDate(document.getElementById('TxtDataScadenza').value)) {
                    alert("Inserire la Data di Scadenza correttamente in formato: GG/MM/AAAA!");
                    Setfocus(document.getElementById('TxtDataScadenza'));
                    return false;
                }
            }

            if (confirm('Si desidera procedere con la Numerazione dei Documenti?')) {
                DivAttesa.style.display = '';
                document.getElementById('CmdNumerazioneDocumenti').click()
            }
            return false;
        }

        function CancellaMinuta() {
            if (confirm('Si desidera eliminare l\'approvazione della Minuta?')) {
                document.getElementById('CmdCancellaMinuta').click()
            }
            return false;
        }

        function ApprovaMinuta() {
            if (confirm('Si desidera approvare la Minuta?')) {
                document.getElementById('CmdApprovaMinuta').click()
            }
            return false;
        }

        function StampaMinuta() {
            //*** 20141107 ***
            document.getElementById('hdMinutaAnagAllRow').value = "0";
            if (confirm('Si desidera, in caso di piu\' contatori, stampare l\'anagrafica su tutte le righe?'))
                document.getElementById('hdMinutaAnagAllRow').value = "1";
            DivAttesa.style.display = '';
            document.getElementById('CmdStampaMinuta').click()
        }

        function Visualizza() {
            parent.parent.Visualizza.location.href = 'DettaglioFatturazione/RicercaFatturazione.aspx?paginacomandi=ComandiRicFatturazione.aspx&paginacomandichiamante=' + document.getElementById('paginacomandi').value + '&IdPeriodo=' +<%=Session("PERIODOID")%> +'&Provenienza=E'
			}

			function ConfigRate() {
			    parent.parent.Visualizza.location.href = '../GestioneTabelleCalcolo/Rate/ConfiguraRate.aspx'
			    parent.parent.Comandi.location.href = '../GestioneTabelleCalcolo/Rate/CConfiguraRate.aspx'
			}

			function Fatturazione() {
			    if (document.getElementById('TxtNContatoriNoLettura').value != 0) {
			        if (confirm('Sono presenti ' + document.getElementById('TxtNContatoriNoLettura').value + ' contatori senza lettura.\nSi vuole procedere con il Calcolo dei Documenti?')) {
			            DivAttesa.style.display = '';
			            document.getElementById('CmdCalcola').click();
			        }
			    }
			    else {
			        if (confirm('Si desidera procedere con il Calcolo dei Documenti?')) {
			            DivAttesa.style.display = '';
			            document.getElementById('CmdCalcola').click();
			        }
			    }
			    return false;
			}

			function DeleteFatturazione() {
			    if (confirm('Si desidera eliminare la Fatturazione?')) {
			        document.getElementById('CmdDeleteFatturazione').click()
			    }
			    return false;
			}

			function Estrazione290() {
			    if (confirm('Si vuole procedere con l\'elaborazione del flusso 290?'))
			        DivAttesa.style.display = '';
			    document.getElementById('CmdEstrazione290').click()
			}
			//*** 201511 - template documenti per ruolo ***
			function UploadTemplate() {
			    DivAttesa.style.display = '';
			    document.getElementById('BtnUploadClick').click();
			}
			function DownloadTemplate() {
			    DivAttesa.style.display = '';
			    document.getElementById('BtnDownloadClick').click();
			}
			//*** ***
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <table id="TblRicerca" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td>
                    <asp:Label ID="Label27" Width="100%" runat="server" CssClass="lstTabRow">Riepilogo Dati da Fatturare per il Periodo</asp:Label></td>
            </tr>
            <tr>
                <td>
                    <br>
                    <fieldset class="classeFieldSetRicerca" id="DivRiepilogoDaFatturare" runat="server">
                        <table id="TblRiepilogoDaFatturare" cellspacing="1" cellpadding="1" width="100%" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="Label28" runat="server" CssClass="Input_Label">N.Contribuenti</asp:Label></td>
                                <td>
                                    <asp:Label ID="LblNContribDaFatt" runat="server" CssClass="Input_Label"></asp:Label></td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" CssClass="Input_Label">N.Letture</asp:Label></td>
                                <td>
                                    <asp:Label ID="LblNLettureDaFatt" runat="server" CssClass="Input_Label"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label14" runat="server" CssClass="Input_Label">Note</asp:Label></td>
                                <td colspan="3">
                                    <asp:Label ID="LblNotePrec" runat="server" CssClass="Input_Label"></asp:Label></td>
                                <asp:TextBox ID="TxtNContatoriNoLettura" runat="server" Style="display: none">0</asp:TextBox>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td>
                    <br>
                    <br>
                    <asp:Label ID="Label26" Width="100%" runat="server" CssClass="lstTabRow">Riepilogo Dati Fatturazione in Corso per il Periodo</asp:Label></td>
            </tr>
            <tr>
                <td>
                    <br>
                    <fieldset class="classeFieldSetRicerca" id="DivRiepilogoFatturazione" runat="server">
                        <table id="TblRiepilogoFatturazione" cellspacing="1" cellpadding="1" width="100%" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="Label24" runat="server" CssClass="Input_Label">N.Contribuenti</asp:Label></td>
                                <td>
                                    <asp:Label ID="LblNContrib" runat="server" CssClass="Input_Label"></asp:Label></td>
                                <td>
                                    <asp:Label ID="Label23" runat="server" CssClass="Input_Label">N.Documenti</asp:Label></td>
                                <td>
                                    <asp:Label ID="LblNDoc" runat="server" CssClass="Input_Label"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" CssClass="Input_Label">Tot.Importi Positivi</asp:Label></td>
                                <td>
                                    <asp:Label ID="LblImpPositivi" runat="server" CssClass="Input_Label"></asp:Label></td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" CssClass="Input_Label">Tot.Importi Negativi</asp:Label></td>
                                <td>
                                    <asp:Label ID="LblImpNegativi" runat="server" CssClass="Input_Label"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label25" runat="server" CssClass="Input_Label">Note</asp:Label></td>
                                <td colspan="3">
                                    <asp:Label ID="LblNote" runat="server" CssClass="Input_Label"></asp:Label></td>
                            </tr>
                        </table>
                    </fieldset>
                    <br>
                </td>
            </tr>
            <tr>
                <td>
                    <br>
                    <div class="classeFieldSetRicerca" id="DivDatiFatturazione">
                        <table id="TblDatiFatturazione" cellspacing="1" cellpadding="1" width="100%" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" CssClass="Input_Label">Prefisso Numero Fattura</asp:Label></td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" CssClass="Input_Label">Numero Iniziale Fattura</asp:Label></td>
                                <td>
                                    <asp:Label ID="Label18" runat="server" CssClass="Input_Label">Suffisso Numero Fattura</asp:Label></td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" CssClass="Input_Label">Data Emissione Fattura</asp:Label></td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" CssClass="Input_Label">Data Scadenza</asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="TxtPrefissoFattura" Width="50px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="TxtNIniziale" Style="text-align: right" Width="50px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="TxtSuffissoFattura" Width="50px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="TxtDataFattura" onblur="txtDateLostfocus(this);VerificaData(this);" Style="text-align: right"
                                        onfocus="txtDateGotfocus(this);" Width="80px" runat="server" CssClass="Input_Text" MaxLength="10"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="TxtDataScadenza" onblur="txtDateLostfocus(this);VerificaData(this);" Style="text-align: right"
                                        onfocus="txtDateGotfocus(this);" Width="80px" runat="server" CssClass="Input_Text" MaxLength="10"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:FileUpload ID="fileUpload" runat="server" CssClass="Input_Text" Width="600px" />
                                    <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="fileUpload" ErrorMessage="Seleziona il file da importare" ValidationGroup="UploadValidation"></asp:RequiredFieldValidator>
                                    <br />
                                    <asp:Label ID="lblMessage" runat="server" Text="" Visible="false" CssClass="ERRORSTYLE"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br>
                </td>
            </tr>
            <tr>
                <td>
                    <Grd:RibesGridView ID="GrdDateRuolo" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Data Calcolo">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataCalcoli"))%>' ID="Label10">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Stampa Minuta">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataStampaMinuta"))%>' ID="Label11">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Approvazione Minuta">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataOkMinuta"))%>' ID="Label8">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Numerazione">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataNumerazione"))%>' ID="Label12">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Approvazione Documenti">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataApprovazioneDOC"))%>' ID="Label13">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="nFirstNDoc" HeaderText="Primo N.Fattura">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="nLastNDoc" HeaderText="Ultimo N.Fattura">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                    <br>
                    <b>
                        <asp:Label ID="lblLinkFile290" runat="server" CssClass="Input_Label" Font-Underline="True"></asp:Label></b>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                        <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                        <div class="BottoneClessidra">&nbsp;</div>
                        <div class="Legend">Attendere Prego</div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <br>
                    <br>
                    <asp:Label ID="Label3" Width="100%" runat="server" CssClass="lstTabRow">Riepilogo Dati Fatturazioni Precedenti per il Periodo</asp:Label></td>
            </tr>
            <tr>
                <td>
                    <br>
                    <Grd:RibesGridView ID="GrdFatturazioniPrec" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowCommand="GrdRowCommand">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="nNContribuenti" HeaderText="N.Contribuenti"></asp:BoundField>
                            <asp:BoundField DataField="nNDocumenti" HeaderText="N.Documenti"></asp:BoundField>
                            <asp:BoundField DataField="impPositivi" HeaderText="Tot.Importi Pos." DataFormatString="{0:0.00}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="impNegativi" HeaderText="Tot.Importi Neg." DataFormatString="{0:0.00}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Data Emissione">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataEmissioneFattura"))%>' ID="Label9">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Approvazione Minuta">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataOkMinuta"))%>' ID="Label15">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Approvazione Documenti">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataApprovazioneDOC"))%>' ID="Label17">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="nFirstNDoc" HeaderText="Primo N.Fattura">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="nLastNDoc" HeaderText="Ultimo N.Fattura">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageStampa" CommandName="RowUpdate" CommandArgument='<%# Eval("IdFlusso") %>' runat="server" Width="16px" Height="19px" ImageUrl="..\..\images\Bottoni\printer-print.png" alt="Stampa Minuta" OnClientClick="if (confirm('Si desidera, in caso di piu\' contatori, stampare l\'anagrafica su tutte le righe?')){document.getElementById('hdMinutaAnagAllRow').value='1';}DivAttesa.style.display='';"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdFlusso") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("IdFlusso") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
        </table>
        <asp:Label ID="LblIdFlusso" Style="display: none" runat="server"></asp:Label>
        <input id="paginacomandi" type="hidden" name="paginacomandi">
        <asp:Button ID="CmdElaborazioniDocumenti" Style="display: none" runat="server" Text="Elaborazione Documenti"></asp:Button>
        <asp:Button ID="CmdNumerazioneDocumenti" Style="display: none" runat="server" Text="Numerazione Documenti"></asp:Button>
        <asp:Button ID="CmdCancellaMinuta" Style="display: none" runat="server" Text="Elimina Approvazione Minuta"></asp:Button>
        <asp:Button ID="CmdApprovaMinuta" Style="display: none" runat="server" Text="Approvazione Minuta"></asp:Button>
        <asp:Button ID="CmdStampaMinuta" Style="display: none" runat="server" Text="Stampa Minuta" Width="136px"></asp:Button>
        <asp:Button ID="CmdVisualizzaFatture" Style="display: none" runat="server" Text="Visualizza Fattura"></asp:Button>
        <asp:Button ID="CmdDeleteFatturazione" Style="display: none" runat="server" Text="Elimina Fatturazione"></asp:Button>
        <asp:Button ID="CmdCalcola" Style="display: none" runat="server" Width="136px" Text="Calcola"></asp:Button>
        <asp:Button ID="CmdEstrazione290" Style="display: none" runat="server" Width="136px" Text="Estrazione 290"></asp:Button>
        <asp:Button ID="CmdScarica" Style="display: none" runat="server"></asp:Button>
        <asp:Button Style="display: none" ID="btnDownload" runat="server" OnClick="BtnDownloadClick" />
        <asp:Button Style="display: none" ID="btnUpload" runat="server" OnClick="BtnUploadClick" ValidationGroup="UploadValidation" />
        <!--*** 20141107 ***-->
        <asp:HiddenField ID="hdMinutaAnagAllRow" runat="server" Value="0" />
    </form>
</body>
</html>
