<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestTessere.aspx.vb" Inherits="OPENgovTIA.GestTessere" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>GestTessere</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" src="../../_js/ControlloData.js?newversion"></script>
    <script type="text/javascript">
        function ShowInsertUI(IdContribuente, IdTestata, IdTessera, IdUniqueUI, AzioneProv, Provenienza, IdList, IsFromVariabile) {
            sParametri = "IdContribuente=" + IdContribuente + "&IdTestata=" + IdTestata + "&IdTessera=" + IdTessera + "&IdUniqueUI=" + IdUniqueUI + "&AzioneProv=" + AzioneProv + "&Provenienza=" + Provenienza + "&IdList=" + IdList
            sParametri += "&IsFromVariabile=" + IsFromVariabile
            //parent.Comandi.location.href = 'ComandiGestImmobili.aspx?Provenienza=' + Provenienza + '&AzioneProv=' + AzioneProv
            parent.Visualizza.location.href = 'GestImmobili.aspx?' + sParametri
        }

        function ShowRicUIAnater() {
            winWidth = 1000
            winHeight = 800
            myleft = (screen.width - winWidth) / 2
            mytop = (screen.height - winHeight) / 2 - 40
            WinPopFamiglia = window.open("../RicercaAnater/FrmRicercaImmobile.aspx", "", "width=" + winWidth + ",height=" + winHeight + ", status=yes, toolbar=no,top=" + mytop + ",left=" + myleft + ",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no")
        }

        function ShowInsertRidEse(sTypeShow) {
            winWidth = 690
            winHeight = 200
            myleft = (screen.width - winWidth) / 2
            mytop = (screen.height - winHeight) / 2 - 40
            Parametri = "Provenienza=T&sTypeShow=" + sTypeShow
            WinPopRidEse = window.open("./PopUpInsertRidDet.aspx?" + Parametri, "", "width=" + winWidth + ",height=" + winHeight + ", status=yes, toolbar=no,top=" + mytop + ",left=" + myleft + ",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no")
        }

        function CheckDatiTessera(sProvenienza) {
            console.log('controllo tessera');
            //controllo la presenza del numero tessera
			if (document.getElementById('TxtNumeroTessera').value == '') {
                GestAlert('a', 'warning', '', '', 'Inserire o il numero tessera!');
                Setfocus(document.getElementById('TxtNumeroTessera'));
                return false;
            }
            //controllo la validita di Cod.Utente
			if (document.getElementById('TxtCodUtente').value != '') {
                if (!IsValidChar(document.getElementById('TxtCodUtente').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Codice Utente!');
                    Setfocus(document.getElementById('TxtCodUtente'));
                    return false;
                }
            }
            //controllo la validita di codice interno
			if (document.getElementById('TxtCodInterno').value != '') {
                if (!IsValidChar(document.getElementById('TxtCodInterno').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Codice Interno!');
                    Setfocus(document.getElementById('TxtCodInterno'));
                    return false;
                }
            }
            //controllo la validita di Data Rilascio
			if (document.getElementById('TxtDataRilascio').value != '') {
                if (!isDate(document.getElementById('TxtDataRilascio').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire la Data di Rilascio correttamente in formato: GG/MM/AAAA!');
                    Setfocus(document.getElementById('TxtDataRilascio'));
                    return false;
                }
            }
            //controllo la validita di Data Cessazione
			if (document.getElementById('TxtDataCessazione').value != '') {
                if (!isDate(document.getElementById('TxtDataCessazione').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire la Data di Cessazione correttamente in formato: GG/MM/AAAA!');
                    Setfocus(document.getElementById('TxtDataCessazione'));
                    return false;
                }
                else {
                    var starttime = document.getElementById('TxtDataRilascio').value
                    var endtime = document.getElementById('TxtDataCessazione').value
                    //Start date split to UK date format and add 31 days for maximum datediff
                    starttime = starttime.split('/');
                    starttime = new Date(starttime[2], starttime[1] - 1, starttime[0]);
                    //End date split to UK date format 
                    endtime = endtime.split('/');
                    endtime = new Date(endtime[2], endtime[1] - 1, endtime[0]);
                    if (endtime <= starttime) {
                        GestAlert('a', 'warning', '', '', 'La Data di Fine e\' minore/uguale alla Data di Inizio!');
                        Setfocus(document.getElementById('TxtDataCessazione'));
                        return false;
                    }
                }
            }
            //controllo la validita di Note
			if (document.getElementById('TxtNote').value != '') {
                if (!IsValidChar(document.getElementById('TxtNote').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Note!');
                    Setfocus(document.getElementById('TxtNote'));
                    return false;
                }
            }
            //*** 201511 - gestione tipo tessera ***
			if (document.getElementById('DdlTipoTessera').value == '-1') {
                GestAlert('a', 'warning', '', '', 'Selezionare il tipo tessera!');
                Setfocus(document.getElementById('DdlTipoTessera'));
                return false;
            }
            if (document.getElementById('TxtId').value == '-1' && document.getElementById('TxtNumeroTessera').value == 'NO-TESSERA') {
                if (confirm('Gli immobili NON saranno legati a tessere!\nVuoi proseguire?')) {
                    document.getElementById('CmdSalvaDatiTessera').click();
                }
            }
            else {
                console.log('salvo');
                document.getElementById('CmdSalvaDatiTessera').click();
            }
        }

        function ClearDatiTessera() {
            if (document.getElementById('TxtIdTessera').value == '-1' && document.getElementById('TxtIdTestata').value != '-1') {
                if (confirm('I dati non sono stati salvati.\nUscire senza salvare?')) {
                    document.getElementById('CmdClearDatiTessera').click()
                }
            }
            else {
                document.getElementById('CmdClearDatiTessera').click()
            }
        }

        function DeleteTessera() {
            if (confirm('Si desidera eliminare la Tessera?')) {
                document.getElementById('CmdDeleteTessera').click()
            }
            return false;
        }
    </script>
</head>
<body class="Sfondo" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post">
        <table id="TblGenDich" style="width: 100%" cellspacing="1" cellpadding="1" border="0">
            <!--blocco dati contribuente-->
            <%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
            <tr id="TRPlainAnag">
                <td colspan="2">
                    <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height: 200px; width: 100%" frameborder="0" marginheight="0"></iframe>
                    <asp:HiddenField ID="hdIdContribuente" runat="server" Value="-1" />
                </td>
            </tr>
            <tr id="TRSpecAnag">
                <td colspan="2">
                    <table id="TblDatiContribuente" cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="1">
                        <tr>
                            <td bordercolor="darkblue">
                                <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                                    <tr>
                                        <td class="Input_Label" colspan="4" height="20"><strong>DATI CONTRIBUENTE</strong></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="lblNominativo" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="lblCFPIVA" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente" colspan="2">
                                            <asp:Label ID="lblDatiNascita" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente" colspan="2">
                                            <asp:Label ID="lblResidenza" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!--Blocco Dati Tessera-->
            <tr>
                <td>
                    <table id="TblDati" width="100%">
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="Label10" CssClass="lstTabRow" Width="100%" runat="server">Dati Tessera</asp:Label></td>
                        </tr>
                        <tr>
                            <!--*** 201511 - gestione tipo tessera ***-->
                            <td>
                                <asp:Label runat="server" CssClass="Input_Label">Tipo Tessera</asp:Label><br />
                                <asp:DropDownList ID="DdlTipoTessera" runat="server" CssClass="Input_Text"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" CssClass="Input_Label">N.Tessera</asp:Label>
                                <asp:Label ID="Label6" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNumeroTessera" runat="server" CssClass="Input_Text" MaxLength="16" Width="150" TabIndex="1"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" CssClass="Input_Label">Codice Interno</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtCodInterno" runat="server" CssClass="Input_Text" MaxLength="250" TabIndex="2"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" CssClass="Input_Label">Cod.Utente</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtCodUtente" runat="server" CssClass="Input_Text" MaxLength="250" TabIndex="3"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" CssClass="Input_Label">Data Rilascio</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtDataRilascio" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10" TabIndex="5"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label22" runat="server" CssClass="Input_Label">Data Cessazione</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtDataCessazione" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10" TabIndex="6"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!--Blocco Dati Conferimenti-->
            <tr>
                <td width="100%">
                    <asp:Label CssClass="lstTabRow" runat="server" ID="Label4">Dati Conferimenti</asp:Label>
                    <asp:ImageButton ID="LnkNewConferimento" runat="server" ToolTip="Nuovo Conferimento" CausesValidation="False" ImageAlign="Bottom" ImageUrl="../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px"></asp:ImageButton>&nbsp;
                        <asp:Label CssClass="lstTabRow" runat="server" ID="Label8" Width="605px">&nbsp;</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="DivConferimenti" runat="server">
                        <table id="TblConferimenti" cellspacing="0" cellpadding="0" border="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Label CssClass="Legend" runat="server" ID="LblResultConferimenti">Non sono presenti Conferimenti</asp:Label>
                                    <Grd:RibesGridView ID="GrdConferimenti" runat="server" BorderStyle="None"
                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                        <PagerSettings Position="Bottom"></PagerSettings>
                                        <PagerStyle CssClass="CartListFooter" />
                                        <RowStyle CssClass="CartListItem"></RowStyle>
                                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                        <Columns>
                                            <asp:BoundField DataField="sanno" HeaderText="Anno">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="tipoconferimento" HeaderText="Tipo Conferimenti">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="nconferimenti" HeaderText="N.Conferimenti" DataFormatString="{0:0}">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="nvolume" HeaderText="Tot.Volume" DataFormatString="{0:N}">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                        </Columns>
                                    </Grd:RibesGridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 100%">
                    <br />
                </td>
            </tr>
            <!--Blocco Dati UI-->
            <tr>
                <td>
                    <asp:Label CssClass="lstTabRow" runat="server" ID="Label23">Dati Immobili</asp:Label>
                    <asp:Label ID="Label16" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label>
                    <asp:ImageButton ID="LnkNewUI" runat="server" ImageUrl="../../images/Bottoni/Listasel.png" ToolTip="Nuovo Immobile"
                        CausesValidation="False" ImageAlign="Bottom"></asp:ImageButton>&nbsp;
						<asp:ImageButton ID="LnkNewUIAnater" runat="server" ImageUrl="../../images/Bottoni/Listasel.png"
                            CausesValidation="False" ImageAlign="Bottom"></asp:ImageButton>&nbsp;
						<asp:Label CssClass="lstTabRow" runat="server" ID="Label17" Width="605px">&nbsp;</asp:Label>
                    <asp:TextBox ID="TxtImmobili" runat="server" Style="display: none" Width="10px">-1</asp:TextBox>
                    <asp:Button ID="CmdRibaltaUIAnater" Style="display: none" runat="server"></asp:Button>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="DivImmobili" runat="server">
                        <table id="TblImmobili" cellspacing="0" cellpadding="0" border="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Label CssClass="Legend" runat="server" ID="LblResultImmobili">Non sono presenti unità immobiliari</asp:Label>
                                    <Grd:RibesGridView ID="GrdImmobili" runat="server" BorderStyle="None"
                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                        OnRowCommand="GrdRowCommand" OnRowDataBound="GrdRowDataBound">
                                        <PagerSettings Position="Bottom"></PagerSettings>
                                        <PagerStyle CssClass="CartListFooter" />
                                        <RowStyle CssClass="CartListItem"></RowStyle>
                                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
                                                <ItemStyle Width="300px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label35" runat="server" Text='<%# FncGrd.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"), DataBinder.Eval(Container, "DataItem.scivico"), DataBinder.Eval(Container, "DataItem.sInterno"), DataBinder.Eval(Container, "DataItem.sEsponente"), DataBinder.Eval(Container, "DataItem.sScala"), DataBinder.Eval(Container, "DataItem.sfoglio"), DataBinder.Eval(Container, "DataItem.snumero"), DataBinder.Eval(Container, "DataItem.ssubalterno")) %>'>Label</asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Inizio Occupazione">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label34" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.TDATAINIZIO")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Fine Occupazione">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label25" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.TDATAFINE")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="scatcatastale" HeaderText="Cat. Catastale">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NVANI" HeaderText="N.Vani">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NMQ" HeaderText="Tot.MQ" DataFormatString="{0:N}">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NMQANATER" HeaderText="Tot.MQ Territorio" DataFormatString="{0:N}">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SNOTEUI" HeaderText="Note">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Width="200px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkAssociata" runat="server" ToolTip="Associato/Non associato alla tessera" Checked='<%# DataBinder.Eval(Container, "DataItem.id")%>'></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
                                                    <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                                                    <asp:HiddenField runat="server" ID="hfIDDETTAGLIOTESTATA" Value='<%# Eval("IDDETTAGLIOTESTATA") %>' />
                                                    <asp:HiddenField runat="server" ID="hfIDTESSERA" Value='<%# Eval("IDTESSERA") %>' />
                                                    <asp:HiddenField runat="server" ID="hfIDPADRE" Value='<%# Eval("IDPADRE") %>' />
                                                    <asp:HiddenField runat="server" ID="hfNGGTARSU" Value='<%# Eval("NGGTARSU") %>' />
                                                    <asp:HiddenField runat="server" ID="hfTDATAINSERIMENTO" Value='<%# Eval("TDATAINSERIMENTO") %>' />
                                                    <asp:HiddenField runat="server" ID="hfTDATAVARIAZIONE" Value='<%# Eval("TDATAVARIAZIONE") %>' />
                                                    <asp:HiddenField runat="server" ID="hfTDATACESSAZIONE" Value='<%# Eval("TDATACESSAZIONE") %>' />
                                                    <asp:HiddenField runat="server" ID="hfSOPERATORE" Value='<%# Eval("SOPERATORE") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </Grd:RibesGridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 100%">
                    <br />
                </td>
            </tr>
            <!--Blocco Dati Riduzioni/Detassazioni-->
            <tr>
                <td style="width: 100%">
                    <table id="TblRidEse" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label31" CssClass="lstTabRow" runat="server">Dati Riduzioni</asp:Label><asp:ImageButton ID="LnkNewRid" CssClass="nascosto" runat="server" ImageUrl="../../images/Bottoni/nuovoinseriscigrd.png"></asp:ImageButton><asp:ImageButton ID="LnkDelRid" CssClass="nascosto" runat="server" ImageUrl="../../images/Bottoni/cestinogrd.png"></asp:ImageButton><asp:Label ID="Label1" CssClass="lstTabRow" Width="210px" runat="server">&nbsp;</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblResultRid" CssClass="Legend" runat="server">Non sono presenti riduzioni</asp:Label>
                                            <Grd:RibesGridView ID="GrdRiduzioni" runat="server" BorderStyle="None"
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
                                                    <asp:BoundField DataField="SCODICE" HeaderText="Codice"></asp:BoundField>
                                                    <asp:BoundField DataField="SDESCRIZIONE" HeaderText="Descrizione"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneCancella" CommandName="RowDelete" CommandArgument='<%# Eval("SCODICE") %>' alt=""></asp:ImageButton>
                                                            <asp:HiddenField runat="server" ID="hfIDRIFERIMENTO" Value='<%# Eval("IDRIFERIMENTO") %>' />
                                                            <asp:HiddenField runat="server" ID="hfSTIPOVALORE" Value='<%# Eval("STIPOVALORE") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Grd:RibesGridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label33" CssClass="lstTabRow" runat="server">Dati Esenzioni</asp:Label><asp:ImageButton ID="LnkNewDet" CssClass="nascosto" runat="server" ImageUrl="../../images/Bottoni/nuovoinseriscigrd.png"></asp:ImageButton><asp:ImageButton ID="LnkDelDet" CssClass="nascosto" runat="server" ImageUrl="../../images/Bottoni/cestinogrd.png"></asp:ImageButton><asp:Label ID="Label2" CssClass="lstTabRow" Width="188px" runat="server">&nbsp;</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblResultDet" CssClass="Legend" runat="server">Non sono presenti Esenzioni</asp:Label>
                                            <Grd:RibesGridView ID="GrdDetassazioni" runat="server" BorderStyle="None"
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
                                                    <asp:BoundField DataField="SCODICE" HeaderText="Codice"></asp:BoundField>
                                                    <asp:BoundField DataField="SDESCRIZIONE" HeaderText="Descrizione"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneCancella" CommandName="RowDelete" CommandArgument='<%# Eval("SCODICE") %>' alt=""></asp:ImageButton>
                                                            <asp:HiddenField runat="server" ID="hfIDRIFERIMENTO" Value='<%# Eval("IDRIFERIMENTO") %>' />
                                                            <asp:HiddenField runat="server" ID="hfSTIPOVALORE" Value='<%# Eval("STIPOVALORE") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Grd:RibesGridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100%">
                    <br />
                </td>
            </tr>
            <!--Blocco Note Tessera-->
            <tr>
                <td style="width: 100%">
                    <table id="TblNoteTessera" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td>
                                <asp:Label ID="Label7" CssClass="lstTabRow" Width="762px" runat="server">Note Tessere</asp:Label><br />
                                <asp:TextBox ID="TxtNote" CssClass="Input_Text" runat="server" Height="32px" TextMode="MultiLine"
                                    Width="700px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
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
        <asp:TextBox ID="txtDataDichiarazione" Style="display: none" runat="server"></asp:TextBox>
        <asp:TextBox ID="TxtIsRibalta" Style="display: none" runat="server">0</asp:TextBox>
        <asp:TextBox ID="TxtIdTessera" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="TxtId" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdTestata" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdPadre" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:Button ID="CmdSalvaRidEse" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdSalvaTessera" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdSalvaDatiTessera" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdModTessera" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdDeleteTessera" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdClearDatiTessera" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdLegaImmobili" Style="display: none" runat="server"></asp:Button>
    </form>
</body>
</html>
