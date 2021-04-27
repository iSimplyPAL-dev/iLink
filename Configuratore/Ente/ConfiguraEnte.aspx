<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfiguraEnte.aspx.vb" Inherits="OPENgov.ConfiguraEnte" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ConfiguraEnte</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
        function SaveCC(Tipo) {
            if (confirm('Si vogliono salvare le modifiche apportate al conto?'))
                document.getElementById('CmdSaveCC').click();
        }
        function UnloadCC() {
            document.getElementById('hdIdCC').value = "-1";
            document.getElementById('txtConto').value = "";
            document.getElementById('txtIBAN').value = "";
            document.getElementById('txtRiga1').value = "";
            document.getElementById('txtRiga2').value = "";
            document.getElementById('txtAutorizzazione').value = "";
            document.getElementById('chkInStampa').checked = false;
            document.getElementById('txtDataFineValidita').value = "";
            DivCC.Style.Add("display", "none")
        }
    </script>
</head>
<body>
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
                        <input class="Bottone BottoneSalva" runat="server" id="Salva" title="Salva" onclick="document.getElementById('btnSalva').click();" type="button" name="Search" />
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 463px" align="left">
                        <span class="NormalBold_title" id="info" style="width: 400px; height: 20px">Configura Ente</span>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td class="Input_Label">Codice Ente<br />
                        <asp:TextBox ID="txtIdEnte" runat="server" CssClass="Input_Text" Width="95px" MaxLength="6" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="Input_Label" colspan="2">Descrizione<br />
                        <asp:TextBox ID="txtDescrEnte" runat="server" CssClass="Input_Text" Width="280px" MaxLength="100" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td colspan="5">
                        <div id="divDownloadXLS" class="col-md-12">
                            <button id="btnDownload"><i class="fa fa-download"></i> Scarica le clausole contrattuali</button>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="Input_Label" colspan="2">Denominazione<br />
                        <asp:TextBox ID="txtDenominazione" runat="server" CssClass="Input_Text" Width="280px" MaxLength="100"></asp:TextBox>
                    </td>
                    <td class="Input_Label" colspan="2">Codice Belfiore<br />
                        <asp:TextBox ID="txtBelfiore" runat="server" CssClass="Input_Text" Width="95px" MaxLength="4"></asp:TextBox>
                    </td>
                    <td class="Input_Label" style="display: none">Cod.Ente CNC<br />
                        <asp:TextBox ID="txtCodEnteCNC" runat="server" CssClass="Input_Text" Width="95px" MaxLength="4"></asp:TextBox>
                    </td>
                    <td class="Input_Label" style="display: none">Cod.Ente Cred./Ben.<br />
                        <asp:TextBox ID="txtIdEnteCredBen" runat="server" CssClass="Input_Text" Width="95px" MaxLength="4"></asp:TextBox>
                    </td>
                    <td class="Input_Label">Posizione Geografica<br />
                        <asp:TextBox ID="txtPosGeo" runat="server" CssClass="Input_Text" Width="95px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td class="Input_Label" colspan="3">Tipo Categorie TARES<br />
                        <asp:RadioButton runat="server" ID="opt1" GroupName="optTypeCat" CssClass="Input_Radio" Text="sopra i 5000 abitanti" />
                        <asp:RadioButton runat="server" ID="opt2" GroupName="optTypeCat" CssClass="Input_Radio" Text="sotto i 5000 abitanti" />
                    </td>
                </tr>
                <tr>
                    <td class="Input_Label" colspan="3">Indirizzo<br />
                        <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="Input_Text" Width="280px" MaxLength="100"></asp:TextBox>
                    </td>
                    <td class="Input_Label" colspan="2">Località<br />
                        <asp:TextBox ID="txtComune" runat="server" CssClass="Input_Text" Width="223px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td class="Input_Label">C.A.P.<br />
                        <asp:TextBox ID="txtCap" runat="server" CssClass="Input_Text" Width="57px" MaxLength="5"></asp:TextBox>
                    </td>
                    <td class="Input_Label">Provincia<br />
                        <asp:TextBox ID="txtPv" runat="server" CssClass="Input_Text" Width="48px" MaxLength="2"></asp:TextBox>
                    </td>
                    <td class="Input_Label" colspan="3">Provincia Estesa<br />
                        <asp:TextBox ID="txtPvEstesa" runat="server" CssClass="Input_Text" Width="178px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Input_Label" colspan="3">Indirizzo E-mail<br />
                        <asp:TextBox ID="txtEMail" runat="server" CssClass="Input_Text" Width="368px" MaxLength="100"></asp:TextBox>
                    </td>
                    <td class="Input_Label" colspan="2">Telefono<br />
                        <asp:TextBox ID="txtTel" runat="server" CssClass="Input_Text" Width="140px" MaxLength="20"></asp:TextBox>
                    </td>
                    <td class="Input_Label" colspan="5">Fax<br />
                        <asp:TextBox ID="txtFax" runat="server" CssClass="Input_Text" Width="140px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Input_Label">Numero Abitanti<br />
                        <asp:TextBox ID="txtNumAbitanti" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="95px"></asp:TextBox>
                    </td>
                    <td class="Input_Label">Nuclei Famigliari<br />
                        <asp:TextBox ID="txtNucleiFam" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="95px"></asp:TextBox>
                    </td>
                    <td class="Input_Label">
                        <br />
                        <asp:CheckBox runat="server" ID="chkGis" CssClass="Input_CheckBox_NoBorder" Text="GIS" />
                    </td>
                    <td class="Input_Label">
                        <br />
                        <asp:CheckBox runat="server" ID="chkRuoloInsoluti" CssClass="Input_CheckBox_NoBorder" Text="Ruolo Insoluti" />
                    </td>
                    <td class="Input_Label hidden">% Contributo ANCI/CNC<br />
                        <asp:TextBox ID="txtPercAnciCnc" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="95px" MaxLength="10"></asp:TextBox>
                    </td>
                    <td class="Input_Label">Tributi con F24 in Accertamento<br />
                        <asp:TextBox ID="txtTributiBollettinoF24" runat="server" CssClass="Input_Text" Width="120px"></asp:TextBox>
                    </td>
                    <td class="Input_Label">Ambiente<br />
                        <asp:TextBox ID="txtAmbiente" runat="server" CssClass="Input_Text" Width="95px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <fieldset class="FiledSetRicerca">
                            <legend class="Legend">Dati per flusso Agenzia Entrate</legend>
                            <table>
                                <tr>
                                    <td class="Input_Label">Cod.Fiscale/P.IVA<br />
                                        <asp:TextBox ID="txtAECFPIVA" runat="server" CssClass="Input_Text" Width="160px" MaxLength="16"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">P.IVA<label class="Input_Emphasized">(da indicare solo se il comune ha sia partita iva che codice fiscale diversi fra loro)</label><br />
                                        <asp:TextBox ID="txtAEPIVA" runat="server" CssClass="Input_Text OnlyNumber" Width="160px" MaxLength="11"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Cognome<br />
                                        <asp:TextBox ID="txtAECognome" runat="server" CssClass="Input_Text" Width="250px" MaxLength="60"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Nome<br />
                                        <asp:TextBox ID="txtAENome" runat="server" CssClass="Input_Text" Width="150px" MaxLength="25"></asp:TextBox>
                                    </td>
								</tr>
								<tr>
                                    <td class="Input_Label">Sesso<br />
                                        <asp:DropDownList runat="server" ID="ddlAESex" CssClass="Input_Text" onfocus="TrackBlur(this);" Width="150px" AutoPostBack="false">
                                            <asp:ListItem Value="">...</asp:ListItem>
                                            <asp:ListItem Value="M">MASCHIO</asp:ListItem>
                                            <asp:ListItem Value="F">FEMMINA</asp:ListItem>
                                            <asp:ListItem Value="G">PERSONA GIURIDICA</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="Input_Label">Data Nascita/Sede<br />
                                        <asp:TextBox ID="txtAEData" runat="server" CssClass="Input_Text_Right TextDate" Width="110px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Comune Nascita/Sede<br />
                                        <asp:TextBox ID="txtAEComune" runat="server" CssClass="Input_Text" Width="250px" MaxLength="40"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">Prov. Nascita/Sede<br />
                                        <asp:TextBox ID="txtAEPV" runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <asp:Label runat="server" CssClass="legend">Conti correnti</asp:Label><br />
                        <Grd:RibesGridView ID="GrdCC" runat="server" BorderStyle="None"
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
                                <asp:BoundField HeaderText="Tributo" DataField="descrtributo"></asp:BoundField>
                                <asp:BoundField HeaderText="Conto" DataField="ContoCorrente"></asp:BoundField>
                                <asp:TemplateField HeaderText="Descrizione">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Intestazione_1") & " " & DataBinder.Eval(Container, "DataItem.Intestazione_2") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fine Validità">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataFineValidita")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="In Stampa">
                                    <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ContoInStampa") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdCC") %>' alt="" ImageUrl='<%# FncGrd.FormattaCC(DataBinder.Eval(Container, "DataItem.IdCC")) %>'></asp:ImageButton>
                                        <asp:HiddenField runat="server" ID="hfIdCC" Value='<%# Eval("IdCC") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Grd:RibesGridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <div id="DivCC" runat="server" style="display: none">
                            <fieldset class="FiledSetRicerca">
                                <legend class="Legend">Conto corrente</legend>
                                <table width="100%">
                                    <tr>
                                        <td colspan="4"></td>
                                        <td align="right">
                                            <asp:HiddenField ID="hdIdCC" runat="server" Value="-1" Visible="false" />
                                            <img style="cursor: pointer" onclick="SaveCC('S');" alt="Salva Conto corrente" align="bottom" border="0" src="../../images/Bottoni/salva.png" />&nbsp; 
        										<img style="cursor: pointer" onclick="document.getElementById('CmdUnloadCC').click();" alt="Torna indietro" align="bottom" border="0" src="../../images/Bottoni/annulla.png" />&nbsp; 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Input_Label">Tributo<font class="NormalRed">*</font></td>
                                        <td class="Input_Label">Numero conto<font class="NormalRed">*</font></td>
                                        <td class="Input_Label">IBAN</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlTributo" runat="server" CssClass="Input_Text"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtConto" runat="server" CssClass="Input_Text" Width="120px" MaxLength="12"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIBAN" runat="server" CssClass="Input_Text" Width="250px" MaxLength="27"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Input_Label">Descrizione prima riga</td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtRiga1" runat="server" CssClass="Input_Text" Width="100%" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Input_Label">Descrizione seconda riga</td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtRiga2" runat="server" CssClass="Input_Text" Width="100%" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Input_Label">Tipo Conto</td>
                                        <td class="Input_Label">Autorizzazione</td>
                                        <td></td>
                                        <td class="Input_Label">Fine Validità</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="Input_Text">
                                                <asp:ListItem Value="O" Selected="True">Ordinario</asp:ListItem>
                                                <asp:ListItem Value="V">Violazioni</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAutorizzazione" runat="server" CssClass="Input_Text" Width="350px" MaxLength="250"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkInStampa" runat="server" CssClass="Input_CheckBox_NoBorder" Text="In Stampa" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDataFineValidita" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlur(this);"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="Input_Label" colspan="8">Note<br />
                        <asp:TextBox ID="txtNote" runat="server" CssClass="Input_Text" Width="100%" MaxLength="300" Height="90px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
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
        <asp:Button ID="btnSalva" Style="display: none" runat="server" Text="Button"></asp:Button>
        <asp:Button ID="CmdSaveCC" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdUnloadCC" Style="display: none" runat="server"></asp:Button>
        <asp:HiddenField ID="hfClausoleContrattuali" runat="server" value="0" />
    </form>
</body>
</html>
