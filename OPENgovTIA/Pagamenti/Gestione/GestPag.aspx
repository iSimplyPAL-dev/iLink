<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestPag.aspx.vb" Inherits="OPENgovTIA.GestPag" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>GestPag</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
        function DivCartellazione() {
            divCartellazione.style.display = '';
            divDataEntry.style.display = 'none';
            document.getElementById('rdbDataEntry').checked = false;
            document.getElementById('rdbDaCartellazione').checked = true;
            document.getElementById("Search").disabled = false;
            document.getElementById('btnTrovaRate').disabled = false;
            document.getElementById('txtAnnoVersamento').disabled = false;
            document.getElementById('txtNAvviso').disabled = false;
            document.getElementById('lblAnnoVersamento').disabled = false;
            document.getElementById('lblNavviso').disabled = false;
        }

        function DivDataEntry() {
            divCartellazione.style.display = 'none';
            divDataEntry.style.display = '';
            document.getElementById('rdbDataEntry').checked = true;
            document.getElementById('rdbDaCartellazione').checked = false;
            document.getElementById("Search").disabled = true;
            document.getElementById('btnTrovaRate').disabled = true;
            document.getElementById('txtAnnoVersamento').disabled = true;
            document.getElementById('txtNAvviso').disabled = true;
            document.getElementById('lblAnnoVersamento').disabled = true;
            document.getElementById('lblNavviso').disabled = true;
        }

        function ApriRicercaAnagrafe(nomeSessione) {
            winWidth = 980
            winHeight = 680
            myleft = (screen.width - winWidth) / 2
            mytop = (screen.height - winHeight) / 2 - 40
            Parametri = "sessionName=" + nomeSessione
            WinPopUpRicercaAnagrafica = window.open("../../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?" + Parametri, "", "width=" + winWidth + ",height=" + winHeight + ", status=yes, toolbar=no,top=" + mytop + ",left=" + myleft + ",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no")
        }

        function ClearDatiContrib() {
            if (confirm('Si desidera eliminare il Contribuente?')) {
                document.getElementById('TxtCognome').value = '';
                document.getElementById('TxtCodFiscale').value = '';
                document.getElementById('TxtPIva').value = '';
                document.getElementById('TxtNome').value = '';
                document.getElementById('TxtDataNascita').value = '';
                document.getElementById('TxtLuogoNascita').value = '';
                document.getElementById('TxtResVia').value = '';
                document.getElementById('TxtResCivico').value = '';
                document.getElementById('TxtResEsponente').value = '';
                document.getElementById('TxtResInterno').value = '';
                document.getElementById('TxtResScala').value = '';
                document.getElementById('TxtResCAP').value = '';
                document.getElementById('TxtResComune').value = '';
                document.getElementById('TxtResPv').value = '';
                document.getElementById('M').checked = false;
                document.getElementById('F').checked = false;
                document.getElementById('G').checked = false;
                document.getElementById('TxtCodContribuente').value = '-1';

                document.getElementById('LnkPulisciContr').click()
            }
            return false;
        }

        function CheckDatiDich() {
            document.getElementById('btnSalvaDati').click()
        }

        function keyPress() {
            if (window.event.keyCode == 13) {
                document.getElementById('btnTrovaRate').click();
            }
        }
        function keyPressAbb() {
            if (window.event.keyCode == 13) {
                document.getElementById('CmdRicercaEmesso').click();
            }
        }
    </script>
</head>
<body class="Sfondo" ms_positioning="GridLayout">
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
						<INPUT class="Bottone BottoneCancella" id="Cancella" title="Cancella Pagamento" onclick="if (confirm('Si vuole eliminare il pagamento?')) { document.getElementById('btnCancellaPag').click(); }" type="button" name="Cancella"> 
						<INPUT class="Bottone BottoneApri" id="Modifica" title="Modifica Pagamento." onclick="document.getElementById('btnModPagamenti').click();" type="button" name="Modifica"> 
						<INPUT class="Bottone BottoneSalva" id="Salva" title="Salva pagamento." onclick="CheckDatiDich();" type="button" name="Salva"> 
                        <INPUT class="Bottone Bottonericerca" id="Search" title="Ricerca" onclick="document.getElementById('CmdRicercaEmesso').click();" type="button" name="Search">
						<INPUT class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla ricerca." onclick="document.getElementById('btnBack').click();" type="button" name="Cancel">
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 400px">Variabile - Pagamenti - Gestione Pagamenti</span>
                    </td>
                </tr>
            </table>
        </div>
        &nbsp;
		<div class="col-md-12">
            <div id="DivNonAbb" runat="server">
                <fieldset class="FiledSetRicerca">
                    <asp:Label ID="LblNonAbb" runat="server" CssClass="Legend"></asp:Label>
                    <br />
                </fieldset>
                <fieldset class="FiledSetRicerca">
                    <br />
                    <legend class="Legend">Inserimento parametri di  ricerca</legend>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblCFPIva" CssClass="Input_Label" runat="server">Cod.Fiscale/P.IVA</asp:Label><br />
                                <asp:TextBox ID="txtRicCFPIva" CssClass="Input_Text" runat="server" Width="185px" MaxLength="16"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="Label7" CssClass="Input_Label" runat="server">Id.Operazione</asp:Label><br />
                                <asp:TextBox ID="txtRicIdOperazione" CssClass="Input_Text_Right" runat="server" Width="200px" MaxLength="18"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCognome" CssClass="Input_Label" runat="server">Cognome</asp:Label><br />
                                <asp:TextBox ID="txtRicCognome" CssClass="Input_Text" runat="server" Width="285px"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="lblNome" CssClass="Input_Label" runat="server">Nome</asp:Label><br />
                                <asp:TextBox ID="txtRicNome" CssClass="Input_Text" runat="server" Width="185px"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="Label23" CssClass="Input_Label" runat="server">Numero Avviso</asp:Label><br />
                                <asp:TextBox ID="txtRicCodCartella" CssClass="Input_Text" runat="server" Width="185px"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="Label25" CssClass="Input_Label" runat="server">Importo</asp:Label><br />
                                <asp:TextBox ID="txtRicImpPagato" CssClass="Input_Text_Right OnlyNumber" runat="server" Width="130px" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:TextBox></td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <asp:Label CssClass="Legend" runat="server" ID="LblResultRicNonAbb"></asp:Label>
                    <Grd:RibesGridView ID="GrdAvvisi" runat="server" BorderStyle="None"
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
                            <asp:TemplateField HeaderText="Nominativo">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label24" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.sCognome")+" "+ DataBinder.Eval(Container, "DataItem.sNome") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="sCFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
                            <asp:BoundField DataField="sNumeroAvviso" HeaderText="Avviso"></asp:BoundField>
                            <asp:BoundField DataField="sAnno" HeaderText="Anno"></asp:BoundField>
                            <asp:BoundField DataField="sNumeroRata" HeaderText="N. Rata">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sCodBollettino" HeaderText="Id.Operazione">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="dImportoRata" HeaderText="Imp. Rata" DataFormatString="{0:N}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sSegno" HeaderText="Imp. Avviso" DataFormatString="{0:N}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="dImportoPagamento" HeaderText="Pagato Avviso" DataFormatString="{0:N}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageRata" runat="server" Cssclass="BottoneGrd BottoneRataGrd" CommandName="RowAbbRata" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfIdContribuente" Value='<%# Eval("IdContribuente") %>' />
                                    <asp:HiddenField runat="server" ID="HFIDRATA" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField runat="server" ID="HFIDAVVISO" Value='<%# Eval("IDFLUSSO") %>' />
                                    <asp:HiddenField runat="server" ID="hfDataScadenza" Value='<%# Eval("tDataScadenza") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageAvviso" runat="server" Cssclass="BottoneGrd BottoneAvvisoGrd" CommandName="RowAbbAvviso" CommandArgument='<%# Eval("IDFLUSSO") %>' alt=""></asp:ImageButton>
                                 </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </fieldset>
            </div>
            <div id="DivDE" runat="server">
                <table id="TblGenPag" cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tr>
                        <td width="100%">
                            <fieldset class="FiledSetRicerca">
                                <legend class="Legend">Tipo Inserimento</legend>
                                <!--Tipo Operazione-->
                                <table id="tblTipoOperazione" width="100%">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbDaCartellazione" runat="server" CssClass="Input_Label" GroupName="Versamento" Text="Da Cartellazione" AutoPostBack="True"></asp:RadioButton>
                                        </td>
                                        <td width="10%">&nbsp;</td>
                                        <td>
                                            <asp:RadioButton ID="rdbDataEntry" runat="server" CssClass="Input_Label" GroupName="Versamento" Text="Data Entry Manuale" AutoPostBack="True"></asp:RadioButton>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 13px"></td>
                    </tr>
                    <tr>
                        <!--Anno del versamento-->
                        <td width="100%">
                            <fieldset class="FiledSetRicerca">
                                <legend class="Legend">Inserimento Parametri di Ricerca Emesso</legend>
                                <table id="TblTestata" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAnnoVersamento" Width="129px" runat="server" CssClass="Input_Label">Anno Versamento</asp:Label></td>
                                        <td style="width: 118px">
                                            <asp:TextBox ID="txtAnnoVersamento" Width="60" runat="server" CssClass="Input_Text_Right OnlyNumber" MaxLength="4"></asp:TextBox></td>
                                        <td style="width: 95px">
                                            <asp:Label ID="lblNavviso" runat="server" CssClass="Input_Label">Num. Avviso</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtNAvviso" Width="227px" runat="server" CssClass="Input_Text" MaxLength="18"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
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
                            <div id="DivContribuente" runat="server">
                                <table id="TblContribuente" cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td colspan="6" width="100%">
                                            <asp:Label ID="Label45" runat="server" CssClass="lstTabRow">Dati Contribuente</asp:Label>
                                            <asp:Label ID="Label32" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label>&nbsp;
						                        <asp:ImageButton ID="LnkAnagrafica" runat="server" ImageAlign="Bottom" CausesValidation="False" ToolTip="Ricerca Anagrafica da Tributi"
                                                    ImageUrl="../../../images/Bottoni/Listasel.png"></asp:ImageButton>&nbsp;
						                        <asp:ImageButton ID="LnkPulisciContr" runat="server" ImageAlign="Bottom" CausesValidation="False"
                                                    ToolTip="Pulisci i campi Contribuente" ImageUrl="../../../images/Bottoni/cancel.png"></asp:ImageButton>
                                            <asp:Label ID="Label26" Width="576px" runat="server" CssClass="lstTabRow">&nbsp;</asp:Label>
                                        </td>
                                    </tr>
                                    <!--prima riga-->
                                    <tr>
                                        <td width="280">
                                            <asp:Label ID="Label8" runat="server" CssClass="Input_Label">Cod.Fiscale</asp:Label><br />
                                            <asp:TextBox ID="TxtCodFiscale" Width="185px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                        <td width="275" colspan="3">
                                            <asp:Label ID="Label9" runat="server" CssClass="Input_Label">Partita Iva</asp:Label><br />
                                            <asp:TextBox ID="TxtPIva" Width="140px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="Label10" runat="server" CssClass="Input_Label">Sesso</asp:Label><br />
                                            <asp:RadioButton ID="F" runat="server" CssClass="Input_Label" GroupName="Sesso" Text="F"></asp:RadioButton><asp:RadioButton ID="M" runat="server" CssClass="Input_Label" GroupName="Sesso" Text="M"></asp:RadioButton><asp:RadioButton ID="G" runat="server" CssClass="Input_Label" GroupName="Sesso" Text="G"></asp:RadioButton></td>
                                        <td>
                                            <asp:TextBox ID="TxtIdDataAnagrafica" Width="10px" runat="server" Visible="False">-1</asp:TextBox>
                                            <asp:Button ID="btnRibalta" Style="display: none" runat="server"></asp:Button>
                                        </td>
                                    </tr>
                                    <!--seconda riga-->
                                    <tr>
                                        <td width="280">
                                            <asp:Label ID="Label11" runat="server" CssClass="Input_Label">Cognome/Rag.Soc</asp:Label><br />
                                            <asp:TextBox ID="TxtCognome" Width="265px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                        <td width="275" colspan="3">
                                            <asp:Label ID="Label12" runat="server" CssClass="Input_Label">Nome</asp:Label><br />
                                            <asp:TextBox ID="TxtNome" Width="230px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                    </tr>
                                    <!--terza riga-->
                                    <tr>
                                        <td width="280">
                                            <asp:Label ID="Label13" runat="server" CssClass="Input_Label">Data Nascita</asp:Label><br />
                                            <asp:TextBox ID="TxtDataNascita" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                        <td width="275" colspan="3">
                                            <asp:Label ID="Label14" runat="server" CssClass="Input_Label">Luogo Nascita</asp:Label><br />
                                            <asp:TextBox ID="TxtLuogoNascita" Width="250px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                    </tr>
                                    <!--quarta riga-->
                                    <tr>
                                        <td width="280">
                                            <asp:Label ID="Label15" runat="server" CssClass="Input_Label">Via</asp:Label><br />
                                            <asp:TextBox ID="TxtResVia" Width="265px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" CssClass="Input_Label">Civico</asp:Label><br />
                                            <asp:TextBox ID="TxtResCivico" Width="50px" runat="server" CssClass="Input_Text_Right"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="Label17" runat="server" CssClass="Input_Label">Esponente</asp:Label><br />
                                            <asp:TextBox ID="TxtResEsponente" Width="50px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                        <td width="70">
                                            <asp:Label ID="Label18" runat="server" CssClass="Input_Label">Interno</asp:Label><br />
                                            <asp:TextBox ID="TxtResInterno" Width="50px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="Label19" runat="server" CssClass="Input_Label">Scala</asp:Label><br />
                                            <asp:TextBox ID="TxtResScala" Width="50px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                    </tr>
                                    <!--quinta riga-->
                                    <tr>
                                        <td width="280">
                                            <asp:Label ID="Label20" runat="server" CssClass="Input_Label">CAP</asp:Label><br />
                                            <asp:TextBox ID="TxtResCAP" Width="80px" runat="server" CssClass="Input_Text_Right"></asp:TextBox></td>
                                        <td width="275" colspan="3">
                                            <asp:Label ID="Label21" runat="server" CssClass="Input_Label">Comune</asp:Label><br />
                                            <asp:TextBox ID="TxtResComune" Width="250px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="Label22" runat="server" CssClass="Input_Label">Provincia</asp:Label><br />
                                            <asp:TextBox ID="TxtResPv" Width="50px" runat="server" CssClass="Input_Text"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="100%">
                            <asp:Label ID="Label1" runat="server" CssClass="lstTabRow">Dati Versamento</asp:Label>
                            <asp:Label ID="LblProvenienza" runat="server" CssClass="Input_Label"></asp:Label>
                            <asp:Label ID="Label2" Width="542px" runat="server" CssClass="lstTabRow">&nbsp;</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divCartellazione">
                                <label id="LblInfoMagg" runat="server" class="Input_Label_title">N.B.&emsp;L'Importo pagato deve sempre essere il totale versato.<br />
                                    Il campo importo Maggiorazione non è obbligatorio, se non inserito il sistema lo ricaverà scorporandolo dal totale versato.</label><br />
                                <asp:Label ID="lblRisultato" runat="server" CssClass="Input_Label">Dati Cartellazione</asp:Label><br />
                                <Grd:RibesGridView ID="GrdPagamenti" runat="server" BorderStyle="None"
                                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                    AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                    OnRowDataBound="GrdRowDataBound">
                                    <PagerSettings Position="Bottom"></PagerSettings>
                                    <PagerStyle CssClass="CartListFooter" />
                                    <RowStyle CssClass="CartListItem"></RowStyle>
                                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sel *">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckbSelezione" runat="server"></asp:CheckBox>
                                                <asp:HiddenField runat="server" ID="hfIdContribuente" Value='<%# Eval("IdContribuente") %>' />
                                                <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                                                <asp:HiddenField runat="server" ID="hfIdFlusso" Value='<%# Eval("IDFLUSSO") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="sANNO" HeaderText="Anno"></asp:BoundField>
                                        <asp:BoundField DataField="sNumeroAvviso" HeaderText="N. Avviso"></asp:BoundField>
                                        <asp:BoundField DataField="sNumeroRata" HeaderText="N. Rata">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="dImportoRata" HeaderText="Imp. Rata" DataFormatString="{0:N}">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Scadenza">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDataScadenza" runat="server" CssClass="Input_Text_Right TextDate" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataScadenza"))%>'>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Data Pagamento *">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDataPagamento" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataPagamento")) %>'>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Data Riversamento">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDataAccredito" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataAccredito")) %>'>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Imp. Pagato*">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotalePagamento" onkeypress="return NumbersOnly(event, true, false, 2);" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" Text='<%# DataBinder.Eval(Container, "DataItem.dImportoPagamento") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Imp. Maggiorazione">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPagMagg" onkeypress="return NumbersOnly(event, true, false, 2);" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" Text='<%# DataBinder.Eval(Container, "DataItem.dImportoStat") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </Grd:RibesGridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divDataEntry" style="width: 100%; display: none">
                                <label class="Input_Label_title">N.B.&emsp;L'Importo pagato deve essere comprensivo della maggiorazione.<br />
                                    Il campo Maggiorazione sarà usato per il dettaglio.</label><br />
                                <br />
                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAnno" runat="server" CssClass="Input_Label">Anno</asp:Label><asp:Label ID="Label3" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br />
                                            <asp:TextBox ID="TxtAnno" Width="50" runat="server" CssClass="Input_Text_Right OnlyNumber" MaxLength="4"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="lblNumAvvisoDE" runat="server" CssClass="Input_Label">N. Avviso</asp:Label><asp:Label ID="Label6" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br />
                                            <asp:TextBox ID="txtNAvvisoDE" Width="150" runat="server" CssClass="Input_Text" MaxLength="18"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="lblNRata" runat="server" CssClass="Input_Label">N. Rata</asp:Label><br />
                                            <asp:TextBox ID="txtNRata" Width="50" runat="server" CssClass="Input_Text" MaxLength="2"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="lblDataPag" runat="server" CssClass="Input_Label">Data Pagamento</asp:Label><asp:Label ID="Label4" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br />
                                            <asp:TextBox ID="txtDataPag" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="lblDataAccredito" runat="server" CssClass="Input_Label">Data Riversamento</asp:Label><br />
                                            <asp:TextBox ID="txtDataAccreditoDE" onblur="txtDateLostfocus(this);VerificaData(this);"  onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="lblImportoPag" runat="server" CssClass="Input_Label">Importo Pagato</asp:Label><asp:Label ID="Label5" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br />
                                            <asp:TextBox onkeypress="return NumbersOnly(event, true, true, 2);" ID="txtImportoPag" Width="100px" runat="server" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="LblMaggiorazione" runat="server" CssClass="Input_Label">Maggiorazione</asp:Label><br />
                                            <asp:TextBox onkeypress="return NumbersOnly(event, true, false, 2);" ID="txtImportoMagg" Width="100px" runat="server" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
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
        <asp:TextBox ID="TxtIdPag" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdPagamento" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="TxtProvenienza" Style="display: none" runat="server">DEMANUALE</asp:TextBox>
        <asp:Button ID="btnSalvaDati" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdSalvaPag" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="btnClearDatiPag" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="btnModPagamenti" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="btnCancellaPag" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="btnTrovaRate" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdRicercaEmesso" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="btnBack" Style="display: none" runat="server" />
    </form>
</body>
</html>
