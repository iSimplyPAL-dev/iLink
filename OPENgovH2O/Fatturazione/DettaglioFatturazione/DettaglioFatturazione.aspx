 <%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DettaglioFatturazione.aspx.vb" Inherits="OpenUtenze.DettaglioFatturazione" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>DettaglioFatturazione</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
        function ClearDatiDettFatturazione() {
            parent.Visualizza.location.href = document.getElementById('TxtPaginaComandiChiamante').value
        }

        function RicalcoloFattura() {
            //devo avere il tipo utenza
            if (document.getElementById('DdlTipoUtenza').value == '') {
                alert("E\' necessario selezionare il Tipo Utenza!");
                Setfocus(document.getElementById('DdlTipoUtenza'));
                return false;
            }
            //devo avere il tipo contatore
            if (document.getElementById('DdlTipoContatore').value == '') {
                alert("E\' necessario selezionare il Tipo Contatore!");
                Setfocus(document.getElementById('DdlTipoContatore'));
                return false;
            }
            //devo avere il numero di utenze
            if (document.getElementById('TxtNUtenze').value == '') {
                alert("E\' necessario inserire il numero di utenze!");
                Setfocus(document.getElementById('TxtNUtenze'));
                return false;
            }
            else {
                sGG = document.getElementById('TxtNUtenze').value;
                if (!isNumber(sGG)) {
                    alert("Inserire solo NUMERI nel campo Numero Utenze!");
                    Setfocus(document.getElementById('TxtNUtenze'));
                    return false;
                }
                else {
                    if (document.getElementById('TxtNUtenze').value == '0') {
                        alert("Il numero di utenze deve essere maggiore di ZERO!");
                        Setfocus(document.getElementById('TxtNUtenze'));
                        return false;
                    }
                }
            }
            //devo avere la data di lettura attuale
            if (document.getElementById('TxtDataLettAtt').value == '') {
                alert("E\' necessario inserire la Data di Lettura Attuale!");
                Setfocus(document.getElementById('TxtDataLettAtt'));
                return false;
            }
            else {
                //controllo che le date siano coerenti
                if (document.getElementById('TxtDataLettAtt').value != '' && document.getElementById('TxtDataLettPrec').value != '') {
                    var starttime = document.getElementById('TxtDataLettPrec').value
                    var endtime = document.getElementById('TxtDataLettAtt').value
                    //Start date split to UK date format and add 31 days for maximum datediff
                    starttime = starttime.split("/");
                    starttime = new Date(starttime[2], starttime[1] - 1, starttime[0]);
                    //End date split to UK date format 
                    endtime = endtime.split("/");
                    endtime = new Date(endtime[2], endtime[1] - 1, endtime[0]);
                    if (endtime <= starttime) {
                        alert("La Data di Lettura Attuale e\' minore/uguale alla Data di Lettura Precedente!");
                        Setfocus(document.getElementById('TxtDataLettAtt'));
                        return false;
                    }
                }
            }
            //devo avere la lettura attuale
            if (document.getElementById('TxtLettAtt').value == '') {
                alert("E\' necessario inserire la Lettura Attuale!");
                Setfocus(document.getElementById('TxtLettAtt'));
                return false;
            }
            else {
                sGG = document.getElementById('TxtLettAtt').value;
                if (!isNumber(sGG)) {
                    alert("Inserire solo NUMERI nel campo Lettura Attuale!");
                    Setfocus(document.getElementById('TxtLettAtt'));
                    return false;
                }
                else {
                    /*if (document.getElementById('TxtLettAtt').value=='0')
                    {
                        alert("La Lettura Attuale deve essere maggiore di ZERO!");
                        Setfocus(document.getElementById('TxtLettAtt);
                        return false;
                    }
                    else
                    {*/
                    var LettPrec = document.getElementById('TxtLettPrec').value
                    var LettAtt = document.getElementById('TxtLettAtt').value
                    if (LettAtt - LettPrec < 0) {
                        alert("La Lettura Attuale deve essere maggiore della Lettura Precedente!");
                        Setfocus(document.getElementById('TxtLettAtt'));
                        return false;
                    }
                    /*}*/
                }
            }
            //il consumo deve essere numerico
            if (document.getElementById('TxtConsumo').value != '') {
                sGG = document.getElementById('TxtConsumo').value;
                if (!isNumber(sGG)) {
                    alert("Inserire solo NUMERI nel campo Consumo!");
                    Setfocus(document.getElementById('TxtConsumo'));
                    return false;
                }
            }
            //i giorni devono essere numerici
            if (document.getElementById('TxtGiorni').value != '') {
                sGG = document.getElementById('TxtGiorni').value;
                if (!isNumber(sGG)) {
                    alert("Inserire solo NUMERI nel campo Giorni!");
                    Setfocus(document.getElementById('TxtGiorni'));
                    return false;
                }
            }
            document.getElementById('CmdRicalcolo').click()
        }

        function AnnulloDettFatturazione() {
            if (confirm('Si desidera eliminare il documento?')) {
                document.getElementById('CmdAnnullo').click()
            }
            return false;
        }
        function Modifica() {
            document.getElementById('CmdModifica').click()
        }
        function ShowNotaCreditoNewFattura(IdDoc, TipoProvenienza) {
            winWidth = 800
            winHeight = 700
            myleft = (screen.width - winWidth) / 2
            mytop = (screen.height - winHeight) / 2 - 40
            Parametri = 'IdDocumento=' + IdDoc + '&paginacomandi=ComandiDettFatturazione.aspx&PaginaChiamante=DettaglioFatturazione.aspx?paginacomandi=ComandiRicFatturazione.aspx&Provenienza=' + TipoProvenienza + '&ProvenienzaChiamante=C'
            WinPop = window.open("./getElementById('aspx?" + Parametri, "", "status=1,toolbar=1,scrollbars=1, width=800,height=400")
        }
        function StampaDoc() {
            DivAttesa.style.display = '';
            document.getElementById('divStampa').style.display = '';
            document.getElementById('divDettaglio').style.display = 'none';
            document.getElementById('CmdStampaDoc').click();
        }
    </script>
</head>
<body class="Sfondo" leftmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post">
        <div class="col-md-12">
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="LblNUtente" runat="server" Width="100%" CssClass="lstTabRow"></asp:Label></td>
                    <td></td>
                    <td colspan="4">
                        <asp:Label ID="LblRifNotaCredito" runat="server" Width="90%" CssClass="NormalRed"></asp:Label>&nbsp;
					    <asp:ImageButton ID="LnkNotaCredito" runat="server" ToolTip="Visualizza Fattura a fronte di Nota di Credito" CausesValidation="False" ImageAlign="Bottom" ImageUrl="../../images/Bottoni/Listasel.png"></asp:ImageButton>
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-md-12">
            <table width="100%">
                <tr>
                    <td colspan="7">
                        <table id="TblDatiContribuente" cellspacing="0" cellpadding="0" width="100%" border="1">
                            <!--Dati Intestatario-->
                            <tr>
                                <td bordercolor="darkblue" bgcolor="white">
                                    <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td class="Input_Label" colspan="4" height="20"><strong>DATI INTESTATARIO</strong></td>
                                        </tr>
                                        <tr>
                                            <td class="DettagliContribuente" width="160">Cognome/Rag. Sociale</td>
                                            <td class="DettagliContribuente" width="25%">
                                                <asp:Label ID="LblCognomeIntest" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                            <td class="DettagliContribuente" width="110">Nome</td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblNomeIntest" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="DettagliContribuente">Data di Nascita</td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblDataNascitaIntest" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="TxtCodIntestatario" Style="display: none" runat="server"></asp:TextBox></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td class="DettagliContribuente">Residente in</td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblIndirizzoIntest" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                            <td class="DettagliContribuente">Comune (Prov.)</td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblComuneIntest" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <!--Dati Utente-->
                            <tr>
                                <td bordercolor="darkblue" bgcolor="white">
                                    <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td class="Input_Label" colspan="4" height="20"><strong>DATI UTENTE</strong></td>
                                        </tr>
                                        <tr>
                                            <td class="DettagliContribuente" width="160">Cognome/Rag. Sociale</td>
                                            <td class="DettagliContribuente" width="25%">
                                                <asp:Label ID="LblCognome" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                            <td class="DettagliContribuente" width="110">Nome</td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblNome" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="DettagliContribuente">Data di Nascita</td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblDataNascita" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="TxtCodUtente" runat="server" Style="display: none"></asp:TextBox></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td class="DettagliContribuente">Residente in</td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblIndirizzo" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                            <td class="DettagliContribuente">Comune (Prov.)</td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblComune" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <!--Dati Documento-->
                            <tr>
                                <td bordercolor="darkblue" bgcolor="white">
                                    <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td class="Input_Label" colspan="4" height="20"><strong>DATI FATTURAZIONE</strong></td>
                                        </tr>
                                        <tr>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblTipoDoc" runat="server" CssClass="DettagliContribuente"></asp:Label></td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblNDoc" runat="server" CssClass="DettagliContribuente"></asp:Label></td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblDataDoc" runat="server" CssClass="DettagliContribuente"></asp:Label></td>
                                            <td class="DettaglioContribuente">
                                                <asp:Label ID="LblTotFattura" CssClass="DettagliContribuente" runat="server">0</asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <!--Dati Contatore-->
                <tr>
                    <td colspan="7">
                        <asp:Label ID="Label1" Width="100%" CssClass="lstTabRow" runat="server">Dati Contatore</asp:Label></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="Label7" CssClass="Input_Label" runat="server">Via</asp:Label><br>
                        <asp:TextBox ID="TxtVia" Width="400px" CssClass="Input_Text" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label9" CssClass="Input_Label" runat="server">Civico</asp:Label><br>
                        <asp:TextBox ID="TxtCivico" Width="70px" CssClass="Input_Text" runat="server"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="Label10" CssClass="Input_Label" runat="server">Frazione</asp:Label><br>
                        <asp:TextBox ID="TxtFrazione" Width="200px" CssClass="Input_Text" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label14" CssClass="Input_Label" runat="server">Matricola</asp:Label><br>
                        <asp:TextBox ID="TxtMatricola" Width="100px" CssClass="Input_Text" runat="server"></asp:TextBox>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="Label21" CssClass="Input_Label" runat="server">Tipo Utenza</asp:Label><br>
                        <asp:DropDownList ID="DdlTipoUtenza" Width="300px" CssClass="Input_Text" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label15" CssClass="Input_Label" runat="server">N.Utenze</asp:Label><br>
                        <asp:TextBox ID="TxtNUtenze" Style="text-align: right" Width="50px" CssClass="Input_Text" runat="server"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="Label5" CssClass="Input_Label" runat="server">Tipo Contatore</asp:Label><br>
                        <asp:DropDownList ID="DdlTipoContatore" Width="250px" CssClass="Input_Text" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label25" CssClass="Input_Label" runat="server">Acqua Potabile</asp:Label><br>
                        <asp:CheckBox ID="ChkEsenAcqua" CssClass="Input_Label" runat="server" Text="Esente"></asp:CheckBox>
                        <asp:CheckBox ID="ChkEsenAcquaQF" CssClass="Input_Label" runat="server" Text="Esente Quota Fissa"></asp:CheckBox>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="Label26" CssClass="Input_Label" runat="server">Depurazione</asp:Label><br>
                        <asp:DropDownList ID="DdlDepurazione" CssClass="Input_Text" runat="server"></asp:DropDownList>&nbsp;
						    <asp:CheckBox ID="ChkEsenDepurazione" CssClass="Input_Label" runat="server" Text="Esente"></asp:CheckBox>
                        <asp:CheckBox ID="ChkEsenDepQF" CssClass="Input_Label" runat="server" Text="Esente Quota Fissa"></asp:CheckBox>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="Label24" CssClass="Input_Label" runat="server">Fognatura</asp:Label><br>
                        <asp:DropDownList ID="DdlFognatura" CssClass="Input_Text" runat="server"></asp:DropDownList>&nbsp;
						    <asp:CheckBox ID="ChkEsenFognatura" CssClass="Input_Label" runat="server" Text="Esente"></asp:CheckBox>
                        <asp:CheckBox ID="ChkEsenFogQF" CssClass="Input_Label" runat="server" Text="Esente Quota Fissa"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <!--Dati Letture-->
                <tr>
                    <td colspan="7">
                        <asp:Label ID="Label12" Width="100%" CssClass="lstTabRow" runat="server">Dati Letture</asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label23" CssClass="Input_Label" runat="server">Data Lettura Precedente</asp:Label><br>
                        <asp:TextBox ID="TxtDataLettPrec" Style="text-align: right" Width="100px" CssClass="Input_Text" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label17" CssClass="Input_Label" runat="server">Lettura Precedente</asp:Label><br>
                        <asp:TextBox ID="TxtLettPrec" Style="text-align: right" Width="90px" CssClass="Input_Text" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label19" CssClass="Input_Label" runat="server">Data Lettura Attuale</asp:Label><br>
                        <asp:TextBox ID="TxtDataLettAtt" onblur="txtDateLostfocus(this);VerificaData(this);" Style="text-align: right"
                            onfocus="txtDateGotfocus(this);" Width="100px" CssClass="Input_Text" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label22" CssClass="Input_Label" runat="server">Lettura Attuale</asp:Label><br>
                        <asp:TextBox ID="TxtLettAtt" Style="text-align: right" Width="90px" CssClass="Input_Text" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label6" CssClass="Input_Label" runat="server">Consumo</asp:Label><br>
                        <asp:TextBox ID="TxtConsumo" Style="text-align: right" Width="70px" CssClass="Input_Text" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label11" CssClass="Input_Label" runat="server">Giorni</asp:Label><br>
                        <asp:TextBox ID="TxtGiorni" Style="text-align: right" Width="50px" CssClass="Input_Text" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label27" CssClass="Input_Label" runat="server">SubConsumo</asp:Label><br>
                        <asp:TextBox ID="TxtSubConsumo" Style="text-align: right" Width="70px" CssClass="Input_Text" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divDettaglio" class="col-md-12">
            <table width="100%">
                <!--Dati Scaglioni-->
                <tr>
                    <td colspan="7">
                        <asp:Label ID="Label8" Width="100%" CssClass="lstTabRow" runat="server">Dati Scaglioni</asp:Label></td>
                </tr>
                <tr>
                    <td colspan="7">
                        <iframe id="LoadScaglioni" src="DettFatturaScaglioni.aspx" frameborder="0" width="100%" height="115" runat="server"></iframe>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <!--Dati Canoni-->
                <tr>
                    <td colspan="7">
                        <asp:Label ID="Label2" Width="100%" CssClass="lstTabRow" runat="server">Dati Canoni</asp:Label></td>
                </tr>
                <tr>
                    <td colspan="7">
                        <iframe id="LoadCanoni" src="DettFatturaCanoni.aspx" frameborder="0" width="100%" height="115" runat="server"></iframe>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <!--Dati Nolo e Quota Fissa -->
                <tr>
                    <td colspan="3">
                        <asp:Label ID="Label13" Width="100%" CssClass="lstTabRow" runat="server">Dati Nolo</asp:Label></td>
                    <td colspan="4">
                        <asp:Label ID="Label16" Width="100%" CssClass="lstTabRow" runat="server">Dati Quota Fissa</asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <iframe id="LoadNolo" src="DettFatturaNolo.aspx" frameborder="0" width="100%" height="115" runat="server"></iframe>
                        <asp:CheckBox ID="ChkNoloUnaTantum" CssClass="Input_Label" runat="server" Text="Applica Nolo UNA TANTUM"></asp:CheckBox>
                    </td>
                    <td colspan="4">
                        <iframe id="LoadQuotaFissa" src="DettFatturaQuotaFissa.aspx" frameborder="0" width="100%" height="115" runat="server"></iframe>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <!--Dati Addizionali-->
                <tr>
                    <td colspan="7">
                        <asp:Label ID="Label18" Width="100%" CssClass="lstTabRow" runat="server">Dati Addizionali</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <table cellspacing="1" cellpadding="1" width="100%" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="LblResultAddizionali" CssClass="Legend" runat="server">Non sono presenti Addizionali</asp:Label>
                                    <Grd:RibesGridView ID="GrdAddizionali" runat="server" BorderStyle="None"
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
                                            <asp:BoundField DataField="sDescrizione" HeaderText="Descrizione">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="impTariffa" HeaderText="Tariffa " DataFormatString="{0:0.00}">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="nAliquota" HeaderText="Aliquota" DataFormatString="{0:0.00}">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="impAddizionale" HeaderText="Importo " DataFormatString="{0:0.00}">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' alt=""></asp:ImageButton>
                                                    <asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("Id") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </Grd:RibesGridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divVoceAggiuntiva" runat="server">
                                        <Grd:RibesGridView ID="GrdVoceAggiuntiva" runat="server" BorderStyle="None"
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
                                                <asp:TemplateField HeaderText="Descrizione">
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtDescrVoceAgg" runat="server" CssClass="Input_Text" Width="400px" Text='<%# (DataBinder.Eval(Container, "DataItem.sDescrizione")) %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tariffa ">
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtTariffaVoceAgg" runat="server" CssClass="Input_Text_Right" Text='<%# (DataBinder.Eval(Container, "DataItem.impTariffa")) %>' onblur="if (!isNumber(this').value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo Tariffa!');}"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Aliquota">
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtAliquotaVoceAgg" runat="server" CssClass="Input_Text_Right" Text='<%# (DataBinder.Eval(Container, "DataItem.nAliquota")) %>' onblur="if (!isNumber(this').value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo Aliquota!');}"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgSave" CommandName="Update" runat="server" Width="16px" Height="19px" ImageUrl="..\..\images\Bottoni\salva.png" alt="Salva Voce Aggiuntiva"></asp:ImageButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgDel" CommandName="Delete" runat="server" Width="16px" Height="19px" ImageUrl="..\..\images\Bottoni\cestino.png" alt="Elimina Voce Aggiuntiva" OnClientClick="return confirm('Si vuole eliminare la Voce Aggiuntiva?')"></asp:ImageButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("Id") %>' alt=""></asp:ImageButton>
                                                        <asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("Id") %>' />
                                                        <asp:HiddenField runat="server" ID="hfnIdAddizionale" Value='<%# Eval("nIdAddizionale") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </Grd:RibesGridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <!--*** ***-->
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <!--Dati Dettaglio Iva e Rate-->
                <tr>
                    <td colspan="3">
                        <asp:Label ID="Label20" Width="100%" CssClass="lstTabRow" runat="server">Dati Dettaglio Iva</asp:Label></td>
                    <td colspan="4">
                        <asp:Label ID="Label4" Width="100%" CssClass="lstTabRow" runat="server">Dati Rate</asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <iframe id="LoadDettaglioIva" src="DettFatturaDettaglioIva.aspx" frameborder="0" width="100%" height="115" runat="server"></iframe>
                    </td>
                    <td colspan="4">
                        <Grd:RibesGridView ID="GrdRate" runat="server" BorderStyle="None"
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
                                <asp:BoundField DataField="sNRata" HeaderText="N.Rata">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="sDescrRata" HeaderText="Descrizione">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Data Scadenza">
                                    <ItemStyle Width="50px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataScadenza"))%>' ID="Label3">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="impRata" HeaderText="Importo " DataFormatString="{0:0.00}">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("Id") %>' alt=""></asp:ImageButton>
                                        <asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("Id") %>' />
                                        <!--CODICE_CONTRIBUENTE-->
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Grd:RibesGridView>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divStampa" class="col-md-12 classeFiledSetRicerca" style="display:none;">
            <iframe runat="server" id="loadStampa" name="loadStampa" frameborder="0" width="100%" height="250px" src="../../../aspvuota.aspx"></iframe>
        </div>
        <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
            <div class="BottoneClessidra">&nbsp;</div>
            <div class="Legend">Attendere Prego</div>
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
        <input id="paginacomandi" type="hidden" name="paginacomandi">
        <asp:TextBox ID="TxtPaginaComandiChiamante" Style="display: none" runat="server"></asp:TextBox>
        <asp:Button ID="CmdModifica" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdRicalcolo" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdAnnullo" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdStampaDoc" Style="display: none" runat="server"></asp:Button>
    </form>
</body>
</html>
