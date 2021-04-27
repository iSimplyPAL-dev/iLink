<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Calcolo.aspx.vb" Inherits="OPENgovTIA.Calcolo" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Calcolo</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
        function ElaboraRuolo()
        {
            if (document.getElementById('ddlAnno').value=='') 
            {
                GestAlert('a', 'warning', '', '', 'E\' necessario inserire un Anno!');
                return false;
            }
            if (document.getElementById('ddlTipoRuolo').value=='') 
            {
                GestAlert('a', 'warning', '', '', 'E\' necessario inserire una Tipologia di Ruolo!');
                return false;
            }
            DivAttesa.style.display='';
            DivDettaglio.style.display='none';
            document.getElementById('CmdCalcola').click()	
        }
        function ConfermaCartellazione()
        {
            if (confirm('Si vuole procedere con l\'elaborazione degli avvisi?'))
                document.getElementById('CmdCartella').click()
        }
        function DeleteCartellazione()
        {
            if (confirm('Si vuole procedere con l\'eliminazione degli avvisi?'))
                document.getElementById('CmdDeleteElab').click()
        }            
        function Minuta() {
            //*** 20141107 ***
            document.getElementById('hdMinutaAnagAllRow').value = "0";
            if (confirm('Si desidera stampare l\'anagrafica su tutte le righe?'))
                document.getElementById('hdMinutaAnagAllRow').value = "1";
            if (document.getElementById('hdMinutaStampatoreAllowed').value=="1")
            {
                if (confirm('Si desidera stampare la Minuta per lo Stampatore?'))
                    document.getElementById('hdIsMinutaXStampatore').value = "1";
                else
                    document.getElementById('hdIsMinutaXStampatore').value = "0";
            }
            if (document.getElementById('hdPFPVUniqueRow').value=="0")
            {
                if (confirm('Si desidera stampare la Minuta su di un\'unica riga?'))
                    document.getElementById('hdPFPVUniqueRow').value = "1";
            }
            DivAttesa.style.display = ''; 
            document.getElementById('CmdStampaMinuta').click();
        }
        //*** 201809 Bollettazione Vigliano in OPENgov ***
        function AbilitaAnno(){
            if (document.getElementById('optFlusso').checked == true){
                $('#divParamRuolo').hide();
                $('#lblUploadFiles').text('Flussi');
                $('#lblNoteFlussi').show();
                $('#divUploadFiles').show();
            }
            else{
                $('#divParamRuolo').show();
                $('#lblUploadFiles').text('Template Documento di Stampa');
                $('#lblNoteFlussi').hide();
            }
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table1">
                <tr valign="top">
                    <td align="left">
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" rowspan="2">
                        <input class="Bottone BottoneDownload" id="DownloadTemplate" title="Download Template" onclick="DivAttesa.style.display='';document.getElementById('CmdDownload').click();" type="button" name="DownloadTemplate">
                        <input class="Bottone BottoneUpload" id="UploadTemplate" title="Upload Template" onclick="DivAttesa.style.display='';document.getElementById('CmdUpload').click();" type="button" name="UploadTemplate">
                        <input class="Bottone BottoneStampaAlt" id="StampaAvvisi" title="Stampa Minuta Avvisi" onclick="DivAttesa.style.display='';document.getElementById('CmdMinutaAvvisi').click();" type="button" name="StampaAvvisi">
                        <input class="Bottone BottoneCancella" id="CancellaCartellazione" title="Elimina Ruolo" onclick="document.getElementById('CmdDeleteElab').click();" type="button" name="DeleteCartellazione">
                        <input class="Bottone BottonePreview" id="DettaglioRuolo" title="Dettaglio Ruolo" onclick="document.getElementById('CmdDettaglio').click();" type="button" name="DettaglioRuolo" style="display: none">
                        <input class="Bottone BottoneWord" title="Elabora Documenti" id="ElaborazioneDocumenti" onclick="document.getElementById('CmdDocumenti').click()" type="button" name="Modifica">
                        <input class="Bottone BottoneNumerazione" id="CalcolaRate" title="Calcolo Rate" onclick="DivAttesa.style.display='';document.getElementById('CmdCartella').click();" type="button" name="Modifica">
                        <input class="Bottone BottoneCalendario" id="ConfigRate" title="Configura Rate" onclick="document.location.href='../../Configurazione/Rate/ConfRate.aspx?IsFromVariabile=<%=Request.Item("IsFromVariabile") %>';" type="button" name="ConfigRate">
                        <input class="Bottone BottoneFolderAccept" id="ApprovaMinuta" title="Approva Minuta" onclick="document.getElementById('CmdApprovaMinuta').click();" type="button" name="ApprovaMinuta">
                        <input class="Bottone BottoneExcel" id="StampaMinuta" title="Stampa Minuta" onclick="Minuta()" type="button" name="StampaMinuta">
                        <input class="Bottone BottoneApri" id="Visualizza" title="Visualizza Calcoli" onclick="document.getElementById('CmdVisualizza').click()" type="button" name="Visualizza">
                        <input class="Bottone BottoneCalcolo" id="ElaboraRuolo" title="Calcola" onclick="DivAttesa.style.display='';if(document.getElementById('txtInizioConf').value==''){GestAlert('a', 'warning', '', '', 'Inserire la Data Inizio Conferimenti!')}else{document.getElementById('CmdCalcola').click();}" type="button" name="ElaboraRuolo">
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 400px">- Avvisi - Elaborazioni</span>
                    </td>
                </tr>
            </table>
        </div>
        &nbsp;
		<div id="TblRicerca" class="col-md-12">
            <div class="col-md-12">
                <fieldset class="classeFieldSetRicerca hidden" runat="server">
                    <legend class="Legend" runat="server">Tipologia Calcolo</legend>
                    <div class="col-md-12">
                        <div class="col-md-2">
                            <label class="Input_Label">Calcolo</label>&nbsp;
				            <asp:Label runat="server" CssClass="Input_Label" ID="LblTipoCalcolo"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <label class="Input_Label"> su Superfici</label>&nbsp;
				            <asp:Label runat="server" CssClass="Input_Label" ID="LblTipoMQ"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:CheckBox ID="ChkConferimenti" runat="server" CssClass="Input_Label" Text="presenza Conferimenti" />
                        </div>
                        <div class="col-md-2">
                            <asp:CheckBox ID="ChkMaggiorazione" runat="server" CssClass="Input_Label" Text="presenza Maggiorazione" />
                        </div>
                    </div>
                </fieldset>
                <fieldset class="classeFieldSetRicerca col-md-12">
                    <div class="col-md-12">
                        <!--*** 201809 Bollettazione Vigliano in OPENgov ***-->
                        <div class="col-md-3">
                            <asp:RadioButton ID="optAutomatica" runat="server" Text="Generazione Da Dichiarazione" Checked="True" GroupName="OptAutoManual" CssClass="Input_Label" ToolTip="Generazione Da Dichiarazione" onclick="AbilitaAnno()"></asp:RadioButton>
                            <br />
                            <asp:RadioButton ID="optManuale" runat="server" Text="Generazione Manuale" GroupName="OptAutoManual" CssClass="Input_Label hidden" ToolTip="Generazione Manuale" onclick="AbilitaAnno()"></asp:RadioButton>
                            <br />
                            <asp:RadioButton ID="optFlusso" runat="server" Text="Generazione Da Flusso" GroupName="OptAutoManual" CssClass="Input_Label" ToolTip="Generazione Da Flusso" onclick="AbilitaAnno()"></asp:RadioButton>
                        </div>
                        <div id="divParamRuolo" class="col-md-8">
                            <div class="col-md-1">
                                <p>
                                    <label class="Input_Label">Anno Ruolo</label>
                                </p>
                                <asp:DropDownList ID="ddlAnno" runat="server" CssClass="Input_Text col-md-12" AutoPostBack="True"></asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <p>
                                    <label class="Input_Label">Tipologia Calcolo</label>
                                </p>
                                <asp:DropDownList ID="ddlTipoCalcolo" runat="server" CssClass="Input_Text col-md-12" AutoPostBack="True"></asp:DropDownList>
                            </div>
                            <div id="PercentTariffe" class="col-md-1">
                                <p>
                                    <label class="Input_Label">%</label>
                                </p>
                                <asp:TextBox ID="TxtPercentTariffe" runat="server" CssClass="Input_Text col-md-12">100</asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <p>
                                    <label class="Input_Label">Soglia minima</label>
                                </p>
                                <asp:TextBox ID="txtSogliaMinima" runat="server" CssClass="Input_Text_right col-md-6">0</asp:TextBox>
                            </div>
                            <div id="DataConf" class="col-md-3">
                                <p>
                                    <label class="Input_Label">Conferimenti Dal-Al</label>
                                </p>
                                <asp:TextBox runat="server" ID="txtInizioConf" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtFineConf" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                            </div>
                            <!--**** 201809 - Cartelle Insoluti ***-->
                            <div id="DecorrenzaTermine" class="col-md-3">
                                <p>
                                    <label class="Input_Label">GG scadenza</label>
                                </p>
                                <asp:TextBox runat="server" ID="txtGGScadenza" CssClass="Input_Text_Right TextDate" onblur="if (!isNumber(this.value, 0, 0)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI!')}">60</asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:CheckBox ID="chkSimulazione" runat="server" CssClass="Input_CheckBox_NoBorder col-md-12" Text="Simulazione" />
                            </div>
                        </div>
                        <!--*** 201511 - template documenti per ruolo ***-->
                        <div id="divUploadFiles" class="col-md-8">
                            <asp:Label ID="lblUploadFiles" runat="server" Text="Template Documento di Stampa" CssClass="Input_Label"></asp:Label>&nbsp;
                            <asp:FileUpload ID="fuMyFiles" runat="server" CssClass="Input_Text col-md-12" Multiple="Multiple" />
                            <asp:Label ID="lblNoteFlussi" runat="server" Text="N.B. il nome file deve iniziare con il Codice Catastale dell'ente e deve essere in formato 290-SEAB." CssClass="Input_Emphasized col-md-12"></asp:Label>
                        </div>
                    </div>
                    <br /><br /><br />
                    <div class="col-md-12">
                        <asp:Label ID="lblMessage" runat="server" Text="" Visible="false" CssClass="ERRORSTYLE"></asp:Label>
                    </div>
                </fieldset>
            </div>
            <div class="col-md-12">
                <br />
                <fieldset id="DivRiepilogoDaElab" class="classeFieldSetRicerca" runat="server">
                    <legend class="Legend" runat="server" Width="100%">Riepilogo Dati da inserire a Ruolo</legend>
                    <div class="col-md-12" id="TblRiepilogoDaElaburare">
                        <div class="col-md-12">
                            <div class="col-md-3">
                                <asp:Label ID="LblIntestNDom" CssClass="Input_Label" runat="server">N.Utenti DOM</asp:Label>
                                <asp:Label ID="LblNDom" CssClass="Input_Label" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="LblIntestNNonDom" CssClass="Input_Label" runat="server">N.Utenti Non DOM</asp:Label>
                                <asp:Label ID="LblNNonDom" CssClass="Input_Label" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="Label28" CssClass="Input_Label" runat="server">N.Utenti</asp:Label>
                                <asp:Label ID="LblNContribDaElab" CssClass="Input_Label" runat="server"></asp:Label>
                            </div>
                        </div><br />
                        <div class="col-md-12">
                            <div class="col-md-3">
                                <asp:Label ID="LblIntestMQDom" CssClass="Input_Label" runat="server">Tot.MQ Dom</asp:Label>
                                <asp:Label ID="LblMQDom" CssClass="Input_Label" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="LblIntestMQNonDom" CssClass="Input_Label" runat="server">Tot.MQ Non Dom</asp:Label>
                                <asp:Label ID="LblMQNonDom" CssClass="Input_Label" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="Label3" CssClass="Input_Label" runat="server">Tot.MQ</asp:Label>
                                <asp:Label ID="LblMQDaElab" CssClass="Input_Label" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-12" id="RiepDaElabTessere">
                            <div class="col-md-3">
                                <asp:Label ID="LblIntestTessereDaElab" CssClass="Input_Label" runat="server">N.Tessere</asp:Label>
                                <asp:Label ID="LblNTessereDaElab" CssClass="Input_Label" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="LblIntestTesNoUI" CssClass="Input_Label" runat="server">N.Tessere senza Immobili</asp:Label><br />
                                <asp:Label ID="LblIntestTesBidoneDaElab" CssClass="Input_Label" runat="server">N.Tessere Bidone</asp:Label>
                                <asp:Label ID="LblTesNoUIDaElab" CssClass="Input_Label" runat="server"></asp:Label>
                                <asp:Label ID="LblTesBidoneDaElab" CssClass="Input_Label" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="LblIntestConf" CssClass="Input_Label" runat="server">Tot. Conferimenti</asp:Label>
                                <asp:Label ID="LblConferimentiDaElab" CssClass="Input_Label" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div class="col-md-12">
                <br />
                <fieldset id="DivRiepilogoElab" class="classeFieldSetRicerca" runat="server">
                    <legend class="Legend" runat="server" Width="100%">Riepilogo Dati Ruolo</legend>
                    <div class="col-md-12">
                            <div class="col-md-12" id="TblRiepilogoFatturazione">
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        <asp:Label ID="Label24" CssClass="Input_Label" runat="server">N.Utenti</asp:Label>
                                        <asp:Label ID="LblNContrib" CssClass="Input_Label" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="Label23" CssClass="Input_Label" runat="server">N.Avvisi</asp:Label>
                                        <asp:Label ID="LblNDoc" CssClass="Input_Label" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="Label18" CssClass="Input_Label" runat="server">N.Scarti</asp:Label>
                                        <asp:Label ID="LblNScarti" CssClass="Input_Label" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        <asp:Label ID="Label2" CssClass="Input_Label" runat="server">Tot. Parte Fissa</asp:Label>
                                        <asp:Label ID="LblImpPF" CssClass="Input_Label" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="Label7" CssClass="Input_Label" runat="server">Tot. Variabile</asp:Label>
                                        <asp:Label ID="LblImpPV" CssClass="Input_Label" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="LblIntestImpPC" CssClass="Input_Label" runat="server">Tot. Variabile Tessere</asp:Label>
                                        <asp:Label ID="LblImpPC" CssClass="Input_Label" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="col-md-12">
                                    <div class="col-md-3">
                                        <asp:Label ID="Label35" CssClass="Input_Label" runat="server">Tot.TEFA</asp:Label>
                                        <asp:Label ID="LblImpAddiz" CssClass="Input_Label" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="LblIntestImpPM" CssClass="Input_Label" runat="server">Tot. Maggiorazione</asp:Label>
                                        <asp:Label ID="LblImpPM" CssClass="Input_Label" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="Label29" CssClass="Input_Label" runat="server">Tot. Avvisi</asp:Label>
                                        <asp:Label ID="LblImpTot" CssClass="Input_Label" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <asp:Label ID="Label25" CssClass="Input_Label" runat="server">Note</asp:Label>
                                        <asp:Label ID="LblNote" CssClass="Input_Label" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                    </div>
                </fieldset>
            </div>
            <div class="col-md-12">
                <Grd:RibesGridView ID="GrdDateElaborazione" runat="server" BorderStyle="None"
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
                        <asp:TemplateField HeaderText="Data Calcolo">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataCreazione"))%>' ID="Label10">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Stampa Minuta">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataStampaMinuta"))%>' ID="Label11">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Approvazione Minuta">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataOkMinuta"))%>' ID="Label8">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Elaborazione Rate">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataCartellazione"))%>' ID="Label12">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Elaborazione Documenti">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataElabDOC"))%>' ID="Label13">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Approvazione Documenti">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataOKDOC"))%>' ID="Label14">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" CssClass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdFlusso") %>' alt=""></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
                <fieldset id="fsRuoliImportati" class="classeFieldSetRicerca" runat="server">
                    <legend class="Legend" runat="server" Width="100%">Ruoli Importati</legend>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdRuoliImportati" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnRowCommand="GrdRowCommand">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="sAnno" HeaderText="Anno"></asp:BoundField>
                                <asp:BoundField DataField="sDescrRuolo" HeaderText="Tipo"></asp:BoundField>
                                <asp:BoundField DataField="nContribuenti" HeaderText="N.Utenti">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="nAvvisi" HeaderText="N.Avvisi">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ImpAvvisi" HeaderText="Tot. " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                </fieldset>
            </div>
            <div id="DivAttesa" runat="server" style="z-index: 103; position: absolute;display:none;">
                <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                <div class="BottoneClessidra">&nbsp;</div>
                <div class="Legend">Attendere Prego...<asp:Label ID="LblAvanzamento" runat="server"></asp:Label></div>
            </div>
            <div class="col-md-12" id="DivDettaglio" runat="server" style="display: none">
                <fieldset class="classeFieldSetRicerca" runat="server">
                    <legend class="Legend" runat="server" Width="100%">Dettaglio Ruolo</legend>
                    <div class="col-md-6">
					    <asp:Label ID="LblResultRuoliVsCatPF" CssClass="Legend" runat="server">La ricerca non ha prodotto risultati.</asp:Label>
                        <Grd:RibesGridView ID="GrdRuoliVsCatPF" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="sDescrCategoria" HeaderText="Categoria"></asp:BoundField>
                                <asp:BoundField DataField="impRuolo" HeaderText="Lordo " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impRiduzione" HeaderText="Riduzione " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impDetassazione" HeaderText="Esenzione " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impNetto" HeaderText="Netto " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="LblResultRuoliVsCatPV" CssClass="Legend" runat="server">La ricerca non ha prodotto risultati.</asp:Label>
                        <Grd:RibesGridView ID="GrdRuoliVsCatPV" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="sDescrCategoria" HeaderText="Categoria"></asp:BoundField>
                                <asp:BoundField DataField="impRuolo" HeaderText="Lordo " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impRiduzione" HeaderText="Riduzione " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impDetassazione" HeaderText="Esenzione " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impNetto" HeaderText="Netto " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="LblResultRuoliVsCatPM" CssClass="Legend" runat="server">La ricerca non ha prodotto risultati.</asp:Label>
                        <Grd:RibesGridView ID="GrdRuoliVsCatPM" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="sDescrCategoria" HeaderText="Categoria"></asp:BoundField>
                                <asp:BoundField DataField="impRuolo" HeaderText="Lordo " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impRiduzione" HeaderText="Riduzione " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impDetassazione" HeaderText="Esenzione " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impNetto" HeaderText="Netto " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div class="col-md-6" id="RuoliVsCatPC">
                        <asp:Label ID="LblResultRuoliVsCatPC" CssClass="Legend" runat="server">La ricerca non ha prodotto risultati.</asp:Label>
                        <Grd:RibesGridView ID="GrdRuoliVsCatPC" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="sDescrCategoria" HeaderText="Categoria"></asp:BoundField>
                                <asp:BoundField DataField="impRuolo" HeaderText="Lordo " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impRiduzione" HeaderText="Riduzione " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impDetassazione" HeaderText="Esenzione " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impNetto" HeaderText="Netto " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                </fieldset>
            </div>
            <div class="col-md-12" id="DivRuoliPrec" runat="server" style="display: none">
                <fieldset class="classeFieldSetRicerca" runat="server">
                    <legend class="Legend" runat="server" Width="100%">Riepilogo Ruoli Precedenti per il Periodo</legend>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdRuoliPrec" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnRowCommand="GrdRowCommand">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="sDescrTipoRuolo" HeaderText="Tipo"></asp:BoundField>
                                <asp:BoundField DataField="nContribuenti" HeaderText="N.Utenti">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="nAvvisi" HeaderText="N.Avvisi">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impPF" HeaderText="Imponibile " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impPV" HeaderText="Addizionali " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="impPM" HeaderText="Maggiorazione " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ImpAvvisi" HeaderText="Tot. " DataFormatString="{0:N}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Data Approvazione Documenti">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataOKDOC"))%>' ID="Label21">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" CssClass="BottoneGrd BottonePrintGrd" CommandName="RowPrint" CommandArgument='<%# Eval("IdFlusso") %>' alt=""></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                </fieldset>
            </div>
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
        <asp:Label Style="display: none" ID="LblIdElab" runat="server">-1</asp:Label>
        <asp:Button Style="display: none" ID="CmdCalcola" runat="server" Text="Calcola"></asp:Button>
        <asp:Button Style="display: none" ID="CmdVisualizza" runat="server" Text="Visualizza"></asp:Button>
        <asp:Button Style="display: none" ID="CmdStampaMinuta" runat="server" Text="Stampa Minuta"></asp:Button>
        <asp:Button Style="display: none" ID="CmdApprovaMinuta" runat="server" Text="Approvazione Minuta"></asp:Button>
        <asp:Button Style="display: none" ID="CmdCartella" runat="server" Text="Cartellazione"></asp:Button>
        <asp:Button Style="display: none" ID="CmdDeleteElab" runat="server" Text="Elimina Ruolo"></asp:Button>
        <asp:Button Style="display: none" ID="CmdDocumenti" runat="server" Text="Elaborazione Documenti"></asp:Button>
        <asp:Button Style="display: none" ID="CmdMinutaAvvisi" runat="server" Text="Stampa Minuta Avvisi"></asp:Button>
        <asp:Button Style="display: none" ID="CmdDettaglio" runat="server" Text="Visualizza Dettaglio Ruolo"></asp:Button>
        <asp:Button Style="display: none" ID="CmdDownload" runat="server" />
        <asp:Button Style="display: none" ID="CmdUpload" runat="server" />
        <asp:HiddenField ID="hfIdRuoloMinutaAvvisi" runat="server" Value="-1" />
        <!--*** 20141107 ***-->
        <asp:HiddenField ID="hdMinutaAnagAllRow" runat="server" Value="0" />
        <!--*** 201809 Bollettazione Vigliano in OPENgov ***-->
        <asp:HiddenField ID="hdMinutaStampatoreAllowed" runat="server" Value="0" />
        <asp:HiddenField ID="hdIsMinutaXStampatore" runat="server" Value="0" />
        <asp:HiddenField ID="hdPFPVUniqueRow" runat="server" Value="0" />
    </form>
</body>
</html>
