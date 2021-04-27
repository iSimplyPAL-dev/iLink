<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DichEmesso.aspx.vb" Inherits="OPENgov.DichEmesso" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../_js/Utility.js?newversion"></script>
        <script src="../_js/VerificaCampi.js?newversion" type="text/javascript"></script>
        <script type="text/javascript" type="text/javascript">
        function ApriStradario(FunzioneRitorno, CodEnte) {
            var TipoStrada = '';
            var Strada = '';
            var CodStrada = '';
            var CodTipoStrada = '';
            var Frazione = '';
            var CodFrazione = '';

            var Parametri = '';
            if (CodEnte=='-1' || CodEnte=='')
                CodEnte=$('#ddlEnti').val();
            Parametri += 'CodEnte=' + CodEnte;
            Parametri += '&TipoStrada=' + TipoStrada;
            Parametri += '&Strada=' + Strada;
            Parametri += '&CodStrada=' + CodStrada;
            Parametri += '&CodTipoStrada=' + CodTipoStrada;
            Parametri += '&Frazione=' + Frazione;
            Parametri += '&CodFrazione=' + CodFrazione;
            Parametri += '&Stile=<% = Session("StileStradario") %>';
                Parametri += '&FunzioneRitorno=' + FunzioneRitorno;

                window.open('<% response.Write(UrlStradario) %>?' + Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');
                return false;
            }

            function RibaltaStrada(objStrada) {
                if (objStrada.TipoStrada != '&nbsp;') {
                    document.getElementById('txtVia').value = (objStrada.TipoStrada + ' ' + objStrada.Strada).replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
                }
                else {//.replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); }).replace('&#232;', 'e')
                    document.getElementById('txtVia').value = objStrada.Strada.replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
                }
            }

            function ClearDatiVia() {
                document.getElementById('txtVia').value = '';
                return false;
            }

            function ControlloParametri() {
                if (document.getElementById('ddlEnti').value == '-1')
                    GestAlert('a', 'warning', '', '', 'Selezionare un ente!');
                else {
                    if (document.getElementById('txtCognome').value == '' && document.getElementById('txtNome').value == ''
                        && document.getElementById('txtCodiceFiscale').value == '' && document.getElementById('txtPartIva').value == ''
                        && document.getElementById('txtDal').value == '' && document.getElementById('txtAl').value == ''
                        && document.getElementById('txtVia').value == '' && document.getElementById('txtNumCiv').value == '' && document.getElementById('txtInterno').value == ''
                        && document.getElementById('txtFoglio').value == '' && document.getElementById('txtNumero').value == '' && document.getElementById('txtSubalterno').value == ''
                        && document.getElementById('TxtNTessera').value == '' && document.getElementById('TxtConfDal').value == '' && document.getElementById('TxtConfAl').value == ''
                    )
                        alert('Impostare almeno un parametro!');
                    else
                    { document.getElementById('DivAttesa').style.display = ''; document.getElementById('btnRicerca').click(); }
                }
            }

            function keyPress() {
                if (window.event.keyCode == 13) {
                    ControlloParametri();
                }
            }
    </script>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" align="right" style="width: 100%; height: 35px">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table2">
                <tr>
                    <td style="width: 464px; height: 20px" align="left">
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" width="800" colspan="2" rowspan="2">
                        <input class="Bottone BottoneExcel" runat="server" id="Excel" title="Stampa elenco Anagrafiche in formato Excel"
                            onclick="document.getElementById('DivAttesa').style.display = ''; document.getElementById('btnStampaExcel').click();"
                            type="button" name="Excel" />
                        <input class="Bottone BottoneRicerca" runat="server" id="Ricerca" title="Ricerca" onclick="ControlloParametri();"
                            type="button" name="Search" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 463px" align="left">
                        <span class="NormalBold_title" id="info" style="width: 400px; height: 20px">Interrogazioni generali</span>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table id="TblRicerca" cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td>
                        <fieldset class="classeFiledSetRicerca">
                            <legend class="Legend">Inserimento filtri di Ricerca</legend>
                            <table id="TblParametri" cellspacing="1" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblEnti" runat="server" CssClass="Input_Label">Ente</asp:Label><br />
                                        <asp:DropDownList ID="ddlEnti" runat="server" CssClass="Input_Text" DataTextField="string" DataValueField="string"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Input_Label">Cognome<br />
                                        <asp:TextBox ID="txtCognome" runat="server" MaxLength="100" CssClass="Input_Text" Width="376px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label" colspan="2">Nome<br />
                                        <asp:TextBox ID="txtNome" runat="server" MaxLength="50" CssClass="Input_Text" Width="185px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Codice Fiscale<br />
                                        <asp:TextBox ID="txtCodiceFiscale" runat="server" MaxLength="16" CssClass="Input_Text" Width="170px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Partita IVA<br />
                                        <asp:TextBox ID="txtPartIva" runat="server" MaxLength="16" CssClass="Input_Text" Width="160px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Dal<br />
                                        <asp:TextBox ID="txtDal" runat="server" MaxLength="16" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Al<br />
                                        <asp:TextBox ID="txtAl" runat="server" MaxLength="16" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trInterGen">
                                    <td class="Input_Label">Via
									<asp:ImageButton ID="LnkOpenStradario" runat="server" ToolTip="Ubicazione Immobile da Stradario." CausesValidation="False" ImageAlign="Bottom" ImageUrl="../images/Bottoni/Listasel.png"></asp:ImageButton>&nbsp;
									<asp:ImageButton ID="LnkPulisciStrada" runat="server" ToolTip="Pulisci i campi della Via" CausesValidation="False" ImageAlign="Bottom" ImageUrl="../images/Bottoni/cancel.png" Width="10px" Height="10px"></asp:ImageButton><br />
                                        <asp:TextBox ID="txtVia" runat="server" MaxLength="100" CssClass="Input_Text" Width="376px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label" colspan="2">Numero Civico<br />
                                        <asp:TextBox ID="txtNumCiv" runat="server" MaxLength="50" CssClass="Input_Text" Width="90px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Interno<br />
                                        <asp:TextBox ID="txtInterno" runat="server" MaxLength="16" CssClass="Input_Text" Width="150px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Foglio<br />
                                        <asp:TextBox ID="txtFoglio" runat="server" MaxLength="16" CssClass="Input_Text" Width="90px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Numero<br />
                                        <asp:TextBox ID="txtNumero" runat="server" MaxLength="16" CssClass="Input_Text" Width="90px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Subalterno<br />
                                        <asp:TextBox ID="txtSubalterno" runat="server" MaxLength="16" CssClass="Input_Text" Width="90px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trTes">
                                    <td class="Input_Label">Numero Tessera<br />
                                        <asp:TextBox ID="TxtNTessera" runat="server" CssClass="Input_Text" Width="150px"></asp:TextBox>
                                    </td>
                                    <td valign="top">
                                        <asp:RadioButton ID="optTutti" runat="server" CssClass="Input_Radio" GroupName="optConferimenti" Checked="true" Text="Tutti" /><br />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="optSi" runat="server" CssClass="Input_Radio" GroupName="optConferimenti" Text="con Conferimenti" /><br />
                                        <asp:RadioButton ID="optNo" runat="server" CssClass="Input_Radio" GroupName="optConferimenti" Text="senza Conferimenti" />
                                    </td>
                                    <td class="Input_Label">Dal<br />
                                        <asp:TextBox ID="TxtConfDal" runat="server" MaxLength="16" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Al<br />
                                        <asp:TextBox ID="TxtConfAl" runat="server" MaxLength="16" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                       <div id="DivAttesa" runat="server" style="z-index: 103; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
                    </td>
                </tr>
            </table>
            <fieldset class="classeFiledSetRicerca" style="display: none" id="fieldsetICI">
                <legend class="Legend">IMU/TASI:</legend>
                <legend class="lead_Emphasized">Dichiarazioni</legend>
                <Grd:RibesGridView ID="GrdDichICI" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                        <asp:BoundField DataField="Nominativo" HeaderText="Nominativo"></asp:BoundField>
                        <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
                        <asp:BoundField DataField="Via" HeaderText="Via"></asp:BoundField>
                        <asp:BoundField DataField="Foglio" HeaderText="Foglio"></asp:BoundField>
                        <asp:BoundField DataField="Numero" HeaderText="Numero"></asp:BoundField>
                        <asp:BoundField DataField="Subalterno" HeaderText="Sub."></asp:BoundField>
                        <asp:TemplateField HeaderText="Dal">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblDalGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.datainizio")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Al">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblAlGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.datafine")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="categoria" HeaderText="Cat."></asp:BoundField>
                        <asp:BoundField DataField="classe" HeaderText="Classe"></asp:BoundField>
                        <asp:BoundField DataField="Valoreimmobile" HeaderText="Rendita/Valore" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                        <asp:BoundField DataField="Consistenza" HeaderText="Cons." DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                        <asp:BoundField DataField="PercPossesso" HeaderText="Pos." DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                        <asp:TemplateField HeaderText="Carat">
                            <ItemTemplate>
                                <asp:Label ID="lblcaratgrd" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.carat") %>'
                                    ToolTip='<%# DataBinder.Eval(Container, "DataItem.descrcarat") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdOggetto") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hfIdOggetto" Value='<%# Eval("IdOggetto") %>' />
                                <asp:HiddenField runat="server" ID="hfIdTestata" Value='<%# Eval("IdTestata") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
                <br />
                <%--griglia emessoICI--%>
                <legend class="lead_Emphasized">Dovuto</legend>
                <Grd:RibesGridView ID="GrdEmessoICI" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                        <asp:BoundField DataField="NOMINATIVO" HeaderText="Nominativo"></asp:BoundField>
                        <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
                        <asp:BoundField DataField="ANNO" HeaderText="Anno"></asp:BoundField>
                        <asp:BoundField DataField="TRIBUTO" HeaderText="Tributo"></asp:BoundField>
                        <asp:BoundField DataField="ABIPRINC" HeaderText="Abi. Princ. €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ALTRIFAB" HeaderText="Altri Fab. €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ALTRIFABSTATO" HeaderText="Altri Fab. Stato €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="AREEFAB" HeaderText="Aree Fab. €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="AREEFABSTATO" HeaderText="Aree Fab. Stato €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="TERRENI" HeaderText="Terreni €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="TERRENISTATO" HeaderText="Terreni Stato €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="FABRUR" HeaderText="Fab.Rur. €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="FABRURSTATO" HeaderText="Fab.Rur. Stato €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="USOPRODCATD" HeaderText="Uso Prod.Cat.D €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="USOPRODCATDSTATO" HeaderText="Uso Prod.Cat.D Stato €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DETRAZ" HeaderText="Detraz. €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="TOTALE" HeaderText="Totale €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="NFABB" HeaderText="Num. Fab."></asp:BoundField>
                        <asp:BoundField DataField="PAGATO" HeaderText="Pagato €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hfCOD_CONTRIBUENTE" Value='<%# Eval("COD_CONTRIBUENTE") %>' />
                                <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
            </fieldset>
            <fieldset class="classeFiledSetRicerca" style="display: none" id="fieldsetTARSU">
                <legend class="Legend">TARI:</legend>
                <%--griglia DichTARSU--%>
                <legend class="lead_Emphasized">Dichiarazioni</legend>
                <Grd:RibesGridView ID="GrdDichTARSU" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                        <asp:BoundField DataField="NOMINATIVO" HeaderText="Nominativo"></asp:BoundField>
                        <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
                        <asp:BoundField DataField="VIA" HeaderText="Via"></asp:BoundField>
                        <asp:BoundField DataField="FOGLIO" HeaderText="Foglio"></asp:BoundField>
                        <asp:BoundField DataField="NUMERO" HeaderText="Numero"></asp:BoundField>
                        <asp:BoundField DataField="SUB" HeaderText="Sub."></asp:BoundField>
                        <asp:TemplateField HeaderText="Dal">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblDalGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DAL")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Al">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblAlGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.AL")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CAT" HeaderText="Cat."></asp:BoundField>
                        <asp:BoundField DataField="MQ" HeaderText="MQ" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="MQTASSABILI" HeaderText="MQ Tassabili" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="NVANI" HeaderText="N.Vani">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hfIDCONTRIBUENTE" Value='<%# Eval("IDCONTRIBUENTE") %>' />
                                <asp:HiddenField runat="server" ID="hfIDTESSERA" Value='<%# Eval("IDTESSERA") %>' />
                                <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
                <br />
                <%--griglia EmessoTARSU--%>
                <legend class="lead_Emphasized">Dovuto</legend>
                <Grd:RibesGridView ID="GrdEmessoTARSU" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                        <asp:BoundField DataField="NOMINATIVO" HeaderText="Nominativo"></asp:BoundField>
                        <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
                        <asp:BoundField DataField="ANNO" HeaderText="Anno"></asp:BoundField>
                        <asp:BoundField DataField="IMPFISSA" HeaderText="Fissa €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="IMPVARIABILE" HeaderText="Variabile €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="IMPCONFERIMENTI" HeaderText="Conferimenti €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="IMPMAGGIORAZIONE" HeaderText="Maggiorazione €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="NAVVISO" HeaderText="N.Avviso"></asp:BoundField>
                        <asp:BoundField DataField="CARICO" HeaderText="Carico €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PAGATO" HeaderText="Pagato €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                                <asp:HiddenField runat="server" ID="hfIDFLUSSO" Value='<%# Eval("IDFLUSSO") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
            </fieldset>
            <fieldset class="classeFiledSetRicerca" style="display: none" id="fieldsetOSAP">
                <legend class="Legend">OSAP:</legend>
                <%--griglia DichOSAP--%>
                <legend class="lead_Emphasized">Dichiarazioni</legend>
                <Grd:RibesGridView ID="GrdDichOSAP" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                        <asp:BoundField DataField="NOMINATIVO" HeaderText="Nominativo"></asp:BoundField>
                        <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
                        <asp:BoundField DataField="VIA" HeaderText="Via"></asp:BoundField>
                        <asp:TemplateField HeaderText="Dal">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblDalGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.datainizio")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Al">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblAlGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.datafine")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DURATA" HeaderText="Durata">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="OCCUPAZIONE" HeaderText="Occupazione"></asp:BoundField>
                        <asp:BoundField DataField="CONSISTENZA" HeaderText="Consistenza" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="CAT" HeaderText="Cat."></asp:BoundField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDDICHIARAZIONE") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hfIDDICHIARAZIONE" Value='<%# Eval("IDDICHIARAZIONE") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
                <br />
                <%--griglia 'EmessoOSAP--%>
                <legend class="lead_Emphasized">Dovuto</legend>
                <Grd:RibesGridView ID="GrdEmessoOSAP" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                        <asp:BoundField DataField="NOMINATIVO" HeaderText="Nominativo"></asp:BoundField>
                        <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
                        <asp:BoundField DataField="ANNO" HeaderText="Anno"></asp:BoundField>
                        <asp:BoundField DataField="NAVVISO" HeaderText="N.Avviso"></asp:BoundField>
                        <asp:BoundField DataField="CARICO" HeaderText="Carico €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PAGATO" HeaderText="Pagato €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDCARTELLA") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hfIDCARTELLA" Value='<%# Eval("IDCARTELLA") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
            </fieldset>
            <fieldset class="classeFiledSetRicerca" style="display: none" id="fieldsetSCUOLA">
                <legend class="Legend">SCUOLA:</legend>
                <%--griglia DichSCUOLA--%>
                <legend class="lead_Emphasized">Dichiarazioni</legend>
                <Grd:RibesGridView ID="GrdDichSCUOLA" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                        <asp:BoundField DataField="NOMINATIVO" HeaderText="Nominativo"></asp:BoundField>
                        <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
                        <asp:BoundField DataField="VIA" HeaderText="Figli"></asp:BoundField>
                        <asp:TemplateField HeaderText="Dal">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblDalGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.datainizio")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Al">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblAlGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.datafine")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DURATA" HeaderText="Durata">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="OCCUPAZIONE" HeaderText="Servizio"></asp:BoundField>
                        <asp:BoundField DataField="CONSISTENZA" HeaderText="Consistenza" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="CAT" HeaderText="Cat."></asp:BoundField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDDICHIARAZIONE") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hfIDDICHIARAZIONE" Value='<%# Eval("IDDICHIARAZIONE") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
                <br />
                <%--griglia 'EmessoSCUOLA--%>
                <legend class="lead_Emphasized">Dovuto</legend>
                <Grd:RibesGridView ID="GrdEmessoSCUOLA" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                        <asp:BoundField DataField="NOMINATIVO" HeaderText="Nominativo"></asp:BoundField>
                        <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
                        <asp:BoundField DataField="ANNO" HeaderText="Anno"></asp:BoundField>
                        <asp:BoundField DataField="NAVVISO" HeaderText="N.Avviso"></asp:BoundField>
                        <asp:BoundField DataField="CARICO" HeaderText="Carico €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PAGATO" HeaderText="Pagato €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDCARTELLA") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hfIDCARTELLA" Value='<%# Eval("IDCARTELLA") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
            </fieldset>
            <fieldset class="classeFiledSetRicerca" style="display: none" id="fieldsetTESSERE">
                <Grd:RibesGridView ID="GrdTessere" runat="server" BorderStyle="None"
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <PagerStyle CssClass="CartListFooter" />
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                        <asp:BoundField DataField="NOMINATIVO" HeaderText="Nominativo"></asp:BoundField>
                        <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
                        <asp:BoundField DataField="DESCRTIPOTESSERA" HeaderText="Tipo Tessera"></asp:BoundField>
                        <asp:BoundField DataField="NUMERO_TESSERA" HeaderText="N.Tessera"></asp:BoundField>
                        <asp:BoundField DataField="CODICE_INTERNO" HeaderText="Cod.Interno"></asp:BoundField>
                        <asp:BoundField DataField="CODICE_UTENTE" HeaderText="Cod.Utente"></asp:BoundField>
                        <asp:TemplateField HeaderText="Dal">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblDalGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATA_RILASCIO")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Al">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblAlGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATA_CESSAZIONE")) %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TOTVOLUME" HeaderText="Conferimenti" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrdGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hfIDCONTRIBUENTE" Value='<%# Eval("IDCONTRIBUENTE") %>' />
                                <asp:HiddenField runat="server" ID="hfIDTESTATA" Value='<%# Eval("IDTESTATA") %>' />
                                <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
            </fieldset>
            <fieldset class="classeFiledSetRicerca" style="display: none" id="fieldsetH2O">
                <legend class="Legend">ACQUEDOTTO:</legend>
                <%--griglia DichH2O--%>
                <legend class="lead_Emphasized">Contatori</legend>
                <Grd:RibesGridView ID="GrdDichH2O" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                        <asp:BoundField DataField="INTESTATARIO" HeaderText="Intestatario"></asp:BoundField>
                        <asp:BoundField DataField="UTENTE" HeaderText="Utente"></asp:BoundField>
                        <asp:BoundField DataField="CONTRATTO" HeaderText="Contratto"></asp:BoundField>
                        <asp:BoundField DataField="NUTENTE" HeaderText="N.Utente"></asp:BoundField>
                        <asp:BoundField DataField="MATRICOLA" HeaderText="Matricola"></asp:BoundField>
                        <asp:BoundField DataField="UBICAZIONE" HeaderText="Ubicazione"></asp:BoundField>
                        <asp:TemplateField HeaderText="Installazione">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.INSTALLAZIONE")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cessazione">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.CESSAZIONE")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ULTIMALETTURA" HeaderText="Ultima Lettura"></asp:BoundField>
						<asp:TemplateField HeaderText="Sub.">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<asp:ImageButton ID="imgEdit" runat="server" Height="15px" Width="15px" ImageUrl='<%# FncGrd.FormattaIsSubContatore(DataBinder.Eval(Container, "DataItem.CODCONTATORESUB")) %>' ToolTip='<%# FncGrd.FormattaToolTipSubContatore(DataBinder.Eval(Container, "DataItem.MATRICOLAPRINCIPALE")) %>'></asp:ImageButton>
							</ItemTemplate>
						</asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
                <br />
                <%--griglia EmessoH2O--%>
                <legend class="lead_Emphasized">Dovuto</legend>
                <Grd:RibesGridView ID="GrdEmessoH2O" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                        <asp:BoundField DataField="NOMINATIVO" HeaderText="Nominativo"></asp:BoundField>
                        <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
                        <asp:BoundField DataField="PERIODO" HeaderText="Periodo"></asp:BoundField>
                        <asp:BoundField DataField="MATRICOLA" HeaderText="Matricola"></asp:BoundField>
                        <asp:BoundField DataField="TIPO" HeaderText="Tipo"></asp:BoundField>
                        <asp:TemplateField HeaderText="Data Documento">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATADOCUMENTO")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NDOCUMENTO" HeaderText="N.Documento"></asp:BoundField>
                        <asp:BoundField DataField="EMESSO" HeaderText="Emesso €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PAGATO" HeaderText="Pagato €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                </Grd:RibesGridView>
            </fieldset>
            <fieldset class="classeFiledSetRicerca" style="display: none" id="fieldsetPROVV">
                <legend class="Legend">PROVVEDIMENTI:</legend>
                <%-- griglia 'EmessoPROVV--%>
                <Grd:RibesGridView ID="GrdEmessoPROVV" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                        <asp:BoundField DataField="NOMINATIVO" HeaderText="Nominativo"></asp:BoundField>
                        <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
                        <asp:BoundField DataField="ANNO" HeaderText="Anno"></asp:BoundField>
                        <asp:BoundField DataField="TIPO" HeaderText="Tipo"></asp:BoundField>
                        <asp:BoundField DataField="NATTO" HeaderText="N. Atto"></asp:BoundField>
                        <asp:TemplateField HeaderText="Data Creazione">
                            <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATACREAZIONE")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TOTALERID" HeaderText="Totale Rid €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="TOTALE" HeaderText="Totale €" DataFormatString="{0:N}">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="STATOAVVISO" HeaderText="Stato Avviso"></asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id_provvedimento") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hfid_provvedimento" Value='<%# Eval("id_provvedimento") %>' />
                                <asp:HiddenField runat="server" ID="hfcod_tipo_provvedimento" Value='<%# Eval("cod_tipo_provvedimento") %>' />
                                <asp:HiddenField runat="server" ID="hfcod_tipo_procedimento" Value='<%# Eval("cod_tipo_procedimento") %>' />
                                <asp:HiddenField runat="server" ID="hfcod_contribuente" Value='<%# Eval("cod_contribuente") %>' />
                                <asp:HiddenField runat="server" ID="hfcod_tributo" Value='<%# Eval("cod_tributo") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
                <br />
            </fieldset>
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
        <asp:Button ID="btnStampaExcel" Style="display: none" runat="server" Text="Button"></asp:Button>
        <asp:Button ID="btnRicerca" Style="display: none" runat="server" Text="Button"></asp:Button>
    </form>
</body>
</html>
