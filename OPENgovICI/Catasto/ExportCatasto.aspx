<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportCatasto.aspx.cs" Inherits="DichiarazioniICI.ExportCatasto" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ExportCatasto</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js?newversion"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#optExpDich, #optExpDichWork, #optImpDich").change(function () {
                ChangeOptExpImp();
            });
        });
        function ChangeOptExpImp() {
            $('#divExpImpPeriodo').hide();
            $('#divImpDich').hide();
            $('#divResult').hide();
            $('#Upload').hide();
            $('#Print').hide();
            if ($('#optExpDich').is(':checked')) {
                $('#divExpImpPeriodo').show();
                $('#Print').show();
            }
            else if ($('#optImpDich').is(':checked')) {
                $('#divImpDich').show();
                $('#Upload').show();
                $('#Search').hide();
            }
            else {
                $('#Print').show();
            }
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: auto">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table1">
                <tr valign="top">
                    <td align="left">
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblEnte" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" rowspan="2">
                        <input class="Bottone BottoneUpload" id="Upload" title="Upload Flussi" onclick="DivAttesa.style.display = ''; document.getElementById('CmdUpload').click();" type="button" name="Upload">
                        <input class="Bottone BottoneExcel" id="Print" title="Estrai in Excel" onclick="DivAttesa.style.display = ''; document.getElementById('CmdPrint').click();" type="button" name="Print">
                        <input class="Bottone Bottonericerca" id="Search" title="Ricerca" onclick="DivAttesa.style.display = ''; document.getElementById('CmdSearch').click();" type="button" name="Search">
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 400px">Catasto - Elaborazioni</span>
                    </td>
                </tr>
            </table>
        </div>
        &nbsp;
        <div class="col-md-12 Input_Label">
            <fieldset class="classeFieldSetRicerca">
                <div id="lblBelfiore" class="col-md-10 Input_Label_Italic"></div>
            </fieldset>
            <fieldset class="classeFieldSetRicerca">
                <p>la procedura parte in automatico ogni <label class="Input_Label_bold">30minuti</label></p>
                <p>se si ha la gestione del verticale bisogna <label class="Input_Label_bold Input_Label_Italic">estrarre la banca dati</label></p>
                <p>procedere quindi nell'ordine con</p>
                <ul style="list-style-type:lower-alpha">
                    <li>
                        <span class="Input_Label_bold Input_Label_Italic">carico dei flussi</span>&nbsp;
                        <span>selezionare i flussi di catasto e di banca dati tributaria</span>
                    </li>
                    <li>
                        <span class="Input_Label_bold Input_Label_Italic">estrazione incrocio</span>
                        <span>estrarre la nuova situazione dichiarativa a seguito di incrocio con catasto</span>
                    </li>                
                    <li>
                        <span class="Input_Label_bold Input_Label_Italic">estrazione anomalie</span>&nbsp;
                        <span>estrarre i prospetti di anomalie</span>
                    </li>
                    <li>
                        <span>se si ha la gestione del verticale</span>
                        <span class="Input_Label_bold Input_Label_Italic">importazione incrocio</span>
                    </li>
			    </ul>
                <p>si può controllare lo stato di avanzamento dalla voce <label class="Input_Label_bold Input_Label_Italic">monitoraggio</label></p>
            </fieldset>
        </div>
        <div id="divExpImpBancaDati" class="col-md-12">
            <fieldset class="classeFieldSetRicerca">
                <legend class="Legend">Estrazione Anomalie</legend>
                <div class="col-md-12">
                    <asp:RadioButton ID="optExpDich" runat="server" Text="Estrazione Banca Dati" GroupName="OptExpImp" CssClass="Input_Label"></asp:RadioButton>&nbsp;
                    <asp:RadioButton ID="optExpDichWork" runat="server" Text="Estrazione incrocio" GroupName="OptExpImp" CssClass="Input_Label"></asp:RadioButton>&nbsp;
                    <asp:RadioButton ID="optImpDich" runat="server" Text="Importazione incrocio" GroupName="OptExpImp" CssClass="Input_Label"></asp:RadioButton>&nbsp;
                </div>
            </fieldset>
            <div id="divExpImpPeriodo" class="col-md-12">
                <fieldset class="classeFieldSetRicerca ">
                    <div class="col-md-12 Input_Label">Selezionare le date di estrazione:</div>
                    <div class="col-md-2">
                        <p><label class="Input_Label">Dal</label></p>
                        <asp:TextBox runat="server" ID="txtDal" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <p><label class="Input_Label">Al</label></p>
                        <asp:TextBox runat="server" ID="txtAl" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                    </div>
                </fieldset>
            </div><br />
            <div id="divImpDich" class="col-md-12">
                <fieldset class="FiledSetRicerca col-md-12">
                    <div class="col-md-12 Input_Label">
                        <span>Il flusso da caricare deve essere:</span>
					    <ul style="list-style-type:square">
                            <li>
                                <span class="Input_Label_bold Input_Label_Italic">Estrazione incrocio con catasto (.CSV)</span>&nbsp;
                                <span>In formato CSV con separatore “;” e intestazioni di colonna nella prima riga.</span>
                            </li>
					    </ul>
                    </div><br />
                    <div class="col-md-12">
                        <asp:Label ID="lblUploadRibaltaFile" runat="server" Text="Flusso" CssClass="Input_Label"></asp:Label>&nbsp;
                        <asp:FileUpload ID="fileUploadRibalta" runat="server" CssClass="Input_Text" Width="600px" />
                    </div>
                </fieldset>
            </div>
        </div>
        <div id="divUpload" class="col-md-12">
			<fieldset class="FiledSetRicerca col-md-12">
                <div class="col-md-12 Input_Label">
                    <span>È possibile caricare un solo flusso per volta.<br />I flussi da caricare sono:</span>
					<ul style="list-style-type:square">
                        <li>
                            <span class="Input_Label_bold Input_Label_Italic">Aggiornamento fabbricati (.FAB)</span>&nbsp;
                            <span>E' lo scarico degli aggiornamenti per un dato periodo di interesse.</span>
                        </li>
                        <li>
                            <span class="Input_Label_bold Input_Label_Italic">Storico fabbricati (.STO)</span>&nbsp;
                            <span>E' la fornitura completa alla data precedente il periodo degli aggiornamenti di interesse.</span>
                        </li>
                        <li><span class="Input_Label_bold Input_Label_Italic">Terreni (.TER)</span></li>
                        <li><span class="Input_Label_bold Input_Label_Italic">Soggetti (.SOG)</span></li>
                        <li><span class="Input_Label_bold Input_Label_Italic">Titoli (.TIT)</span></li>
                        <li>
                            <span class="Input_Label_bold Input_Label_Italic">Banca dati tributaria (.CSV)</span>&nbsp;
                            <span>In formato CSV con separatore “;” e intestazioni di colonna nella prima riga.</span>
                        </li>
					</ul>
                </div><br />
                <div class="col-md-12">
                    <asp:Label ID="lblUploadFile" runat="server" Text="Flusso" CssClass="Input_Label"></asp:Label>&nbsp;
                    <asp:FileUpload ID="fileUpload" runat="server" CssClass="Input_Text" Width="600px" />
                </div><br />
                <div id="divFilesUploaded" class="col-md-12 Input_CheckBox_NoBorder"></div>
			</fieldset>
        </div>
        <div id="divAnomalie" class="col-md-12">
            <fieldset class="classeFieldSetRicerca">
                <legend class="Legend">Anomalie/Estrazioni</legend>
                <div class="col-md-12">
                    <asp:RadioButton ID="optTitNoSog" runat="server" Text="Titolarità non presenti in Soggetti" Checked="True" GroupName="OptAnomalie" CssClass="Input_Label"></asp:RadioButton>&nbsp;
                    <asp:RadioButton ID="optSogNoTit" runat="server" Text="Soggetti non presenti in Titolarità" GroupName="OptAnomalie" CssClass="Input_Label"></asp:RadioButton>&nbsp;
                    <asp:RadioButton ID="optTitNoFab" runat="server" Text="Titolarità non presenti in Fabbricati" GroupName="OptAnomalie" CssClass="Input_Label"></asp:RadioButton>&nbsp;
                    <asp:RadioButton ID="optFabNoTit" runat="server" Text="Fabbricati non presenti in Titolarità" GroupName="OptAnomalie" CssClass="Input_Label"></asp:RadioButton>&nbsp;
                </div><br />
                <div class="col-md-12">
                    <asp:RadioButton ID="optNoDiritto" runat="server" Text="Diritto mancante" GroupName="OptAnomalie" CssClass="Input_Label"></asp:RadioButton>&nbsp;
                    <asp:RadioButton ID="optNoPoss" runat="server" Text="% di possesso mancante" GroupName="OptAnomalie" CssClass="Input_Label"></asp:RadioButton>&nbsp;
                    <asp:RadioButton ID="optNoSogRif" runat="server" Text="Soggetti in Comunione mancanti" GroupName="OptAnomalie" CssClass="Input_Label"></asp:RadioButton>&nbsp;
                    <asp:RadioButton ID="optRurale" runat="server" Text="Ruralità" GroupName="OptAnomalie" CssClass="Input_Label"></asp:RadioButton>&nbsp;
                    <asp:RadioButton ID="optCatasto" runat="server" Text="Catasto" GroupName="OptAnomalie" CssClass="Input_Label"></asp:RadioButton>&nbsp;
                </div>
            </fieldset>
        </div>
        <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
            <div class="BottoneClessidra">&nbsp;</div>
            <div class="Legend">Attendere Prego</div>
        </div>
        <label id="lblError" class="Legend"></label>
        <div id="divResult" class="col-md-12" style="overflow: auto">
            <Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None" 
				BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
				AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
				ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                OnPageIndexChanging="GrdPageIndexChanging">
				<PagerSettings Position="Bottom"></PagerSettings>
				<PagerStyle CssClass="CartListFooter" />
				<RowStyle CssClass="CartListItem"></RowStyle>
				<HeaderStyle CssClass="CartListHead"></HeaderStyle>
				<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                <Columns></Columns>
            </Grd:RibesGridView>
            <asp:LinkButton ID="LblDownloadFile" Runat="server" CssClass="Input_Label" Font-Underline="True" onclick="LblDownloadFile_Click"></asp:LinkButton>
        </div>
        <div id="divMonitoring" class="col-md-12" style="overflow: auto">
            <Grd:RibesGridView ID="GrdMonitorGen" runat="server" BorderStyle="None" 
				BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
				AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
				ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                OnPageIndexChanging="GrdPageIndexChanging" OnRowCommand="GrdRowCommand">
				<PagerSettings Position="Bottom"></PagerSettings>
				<PagerStyle CssClass="CartListFooter" />
				<RowStyle CssClass="CartListItem"></RowStyle>
				<HeaderStyle CssClass="CartListHead"></HeaderStyle>
				<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="N.Elaborazione"></asp:BoundField>
                    <asp:BoundField DataField="ESITOUPLOAD" HeaderText="Carico flussi"></asp:BoundField>
                    <asp:BoundField DataField="ESITOIMPORT" HeaderText="Importazione"></asp:BoundField>
                    <asp:BoundField DataField="ESITOCONVERT" HeaderText="Conv.Cat."></asp:BoundField>
                    <asp:BoundField DataField="ESITOINCROCIO" HeaderText="Incrocio Cat.-Banca dati trib."></asp:BoundField>
                    <asp:BoundField DataField="ESITOESTRAZIONEDICHWORK" HeaderText="Estraz. Dich. da Cat."></asp:BoundField>
                    <asp:BoundField DataField="ESITOESTRAZIONETITVSSOG" HeaderText="Estraz. Tit. non in Sog."></asp:BoundField>
                    <asp:BoundField DataField="ESITOESTRAZIONESOGVSTIT" HeaderText="Estraz. Sog. non in Tit."></asp:BoundField>
                    <asp:BoundField DataField="ESITOESTRAZIONETITVSFAB" HeaderText="Estraz. Tit. non in Fab."></asp:BoundField>
                    <asp:BoundField DataField="ESITOESTRAZIONEFABVSTIT" HeaderText="Estraz. Fab. non in Tit."></asp:BoundField>
                    <asp:BoundField DataField="ESITOESTRAZIONEDIRITTOMANCANTE" HeaderText="Estraz. Diritto mancante"></asp:BoundField>
                    <asp:BoundField DataField="ESITOESTRAZIONEPOSSMANCANTE" HeaderText="Estraz. % pos. mancante"></asp:BoundField>
                    <asp:BoundField DataField="ESITOESTRAZIONECOMUNIONEMANCANTE" HeaderText="Estraz. Sog. in Comunione mancanti"></asp:BoundField>
                    <asp:BoundField DataField="ESITOIMPORTVERTICALE" HeaderText="Imp. su Verticale"></asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" CssClass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Grd:RibesGridView>
            <Grd:RibesGridView ID="GrdMonitorDet" runat="server" BorderStyle="None" 
				BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
				AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
				ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
				<PagerSettings Position="Bottom"></PagerSettings>
				<PagerStyle CssClass="CartListFooter" />
				<RowStyle CssClass="CartListItem"></RowStyle>
				<HeaderStyle CssClass="CartListHead"></HeaderStyle>
				<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="N.Elaborazione"></asp:BoundField>
                    <asp:BoundField DataField="FASE" HeaderText="Fase"></asp:BoundField>
                    <asp:TemplateField HeaderText="Inizio">
                        <ItemStyle Width="120px"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaDataOraGrd(DataBinder.Eval(Container, "DataItem.INIZIO")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fine">
                        <ItemStyle Width="120px"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaDataOraGrd(DataBinder.Eval(Container, "DataItem.FINE")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ESITO" HeaderText="Esito"></asp:BoundField>
                    <asp:BoundField DataField="TEMPO" HeaderText="Durata"></asp:BoundField>
                </Columns>
            </Grd:RibesGridView>
            <div id="chart_div" class="col-md-6" style="height:400px;"></div>
        </div>
        <asp:Button Style="display: none" ID="CmdSearch" runat="server" OnClick="CmdSearch_Click" />
        <asp:Button Style="display: none" ID="CmdPrint" runat="server" OnClick="CmdPrint_Click"/>
        <asp:Button Style="display: none" ID="CmdUpload" runat="server" OnClick="CmdUpload_Click" />
        <asp:HiddenField ID="hfIdElab" runat="server" Value="-1" />
    </form>
</body>
</html>
