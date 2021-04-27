<%@ Page Language="c#" CodeBehind="Gestione.aspx.cs" AutoEventWireup="false" Inherits="DichiarazioniICI.Gestione" EnableEventValidation="false" %>

<%@ Register TagPrefix="uc1" TagName="FiltroPersona" Src="UserControl/FiltroPersona.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FiltroImmobile" Src="UserControl/FiltroImmobile.ascx" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Gestione</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%> 
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%> 
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../_js/skype_killer.js?newversion"></script>
    <script type="text/javascript" src="../_js/ControlloData.js?newversion"></script>
    <script type="text/javascript" src="../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" event="onkeypress" for="document">
        if (window.event.keyCode == 13) {
            //parent.Comandi.document.getElementById('Delete.click();
            document.getElementById('FiltroPersona1_btnTrova').click()
            //document.getElementById('FiltroImmobile1_btnTrova.click()
        }
    </script>
    <script type="text/javascript" type="text/javascript">
        function Search() {
            if (document.getElementById('FiltroPersona1_btnTrova') != null) {
                document.getElementById('FiltroPersona1_btnTrova').click();
            }
            else {
                document.getElementById('FiltroImmobile1_btnTrova').click();
            }
        }
    </script>
</head>
<body class="Sfondo" leftmargin="3" rightmargin="3" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post">
        <table id="tblCorpo" cellspacing="1" cellpadding="1" width="100%" align="center" border="0">
            <!--*** 20131003 - gestione atti compravendita ***-->
            <tr>
                <td>
                    <div id="AttoCompraVendita" class="FiledSetRicerca" style="display: none" runat="server">
                        <fieldset>
                            <legend class="Legend">Dati nota trascrizione</legend>
                            <br />
                            &nbsp;<asp:Label ID="lblNotaTrascrizione" runat="server" Text="" CssClass="Input_Label"></asp:Label>
                            <br />
                            &nbsp;
                        </fieldset>
                        <fieldset>
                            <legend class="Legend">Dati Immobile in nota</legend>
                            <p>&nbsp;<asp:Label ID="lblRifNota" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
                            <p>&nbsp;<asp:Label ID="lblCatNota" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
                            <p>
                                &nbsp;<asp:Label ID="lblUbicazioneNota" runat="server" Text="" CssClass="Input_Label"></asp:Label>
                                &nbsp;<asp:Label ID="lblUbicazioneCatasto" runat="server" Text="" CssClass="Input_Label"></asp:Label>
                            </p>
                            &nbsp;
                        </fieldset>
                        <fieldset>
                            <legend class="Legend">Dati Soggetto in nota</legend>
                            <br />
                            &nbsp;<asp:Label ID="lblSoggettoNota" runat="server" Text="" CssClass="Input_Label"></asp:Label>
                            <br />
                            &nbsp;
                        </fieldset>
                    </div>
                </td>
            </tr>
            <!--*** ***-->
            <tr>
                <td>
                    <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                        <tr>
                            <td colspan="2">
                                <fieldset class="classeFiledSetRicerca">
                                    <legend class="Legend">Inserimento filtri di ricerca</legend>
                                    <table id="tblSoggettoImmobile" cellspacing="1" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td colspan="5">
                                                <asp:Label ID="lblEnti" runat="server" CssClass="Input_Label">Ente</asp:Label><br />
                                                <asp:DropDownList ID="ddlEnti" runat="server" CssClass="Input_Text" DataTextField="string" DataValueField="string"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Input_Label">
                                                <asp:RadioButton ID="rdbSoggetto" runat="server" Text="Per Soggetto" Checked="True" AutoPostBack="True"></asp:RadioButton><asp:RadioButton ID="rdbImmobile" runat="server" Text="Per Immobile" AutoPostBack="True"></asp:RadioButton><asp:Button ID="btnInserimentoDichiarazione" Style="display: none" runat="server" Text="Inserimento Dichiarazione"></asp:Button><asp:Button ID="btnStampaExcel" Style="display: none" runat="server" Text="Button"></asp:Button></td>
                                            <td>
                                                <asp:Label CssClass="Input_Label" runat="server" ID="LblProv">Provenienza Dichiarazione</asp:Label><br />
                                                <asp:DropDownList ID="ddlProv" CssClass="Input_Label" runat="server" DataValueField="Codice" DataTextField="Descrizione" DataSource="<%# ListProvenienze() %>"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" CssClass="Input_Label" ID="Label14">Immobili Dal</asp:Label><br />
                                                <asp:TextBox runat="server" ID="TxtDal" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" MaxLength="10"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" CssClass="Input_Label" ID="Label15">Al</asp:Label><br />
                                                <asp:TextBox runat="server" ID="TxtAl" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" MaxLength="10"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" CssClass="Input_Label" ID="Label13">Tipo Dichiarazioni</asp:Label><br />
                                                <asp:DropDownList ID="ddlBonificato" runat="server" CssClass="Input_Text">
                                                    <asp:ListItem Value="-1">[TUTTE]</asp:ListItem>
                                                    <asp:ListItem Value="0">APERTE</asp:ListItem>
                                                    <asp:ListItem Value="1">CESSATE</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <uc1:FiltroPersona ID="FiltroPersona1" runat="server" Visible="<%# rdbSoggetto.Checked %>"></uc1:FiltroPersona>
                                                <uc1:FiltroImmobile ID="FiltroImmobile1" runat="server" Visible="<%# rdbImmobile.Checked %>"></uc1:FiltroImmobile>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" height="20">
                                <br />
                                &nbsp;<asp:Label ID="lblRisultati" runat="server" CssClass="Legend">Risultati della Ricerca</asp:Label><br />
                            </td>
                            <td align="right">
                                <asp:Label ID="lblNRecord" runat="server" CssClass="Legend"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset class="classeFiledSetNoBorder">
                                    <table id="Table4" cellspacing="1" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <Grd:RibesGridView ID="GrdContribuenti" runat="server" BorderStyle="None"
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
                                                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                                                        <asp:BoundField DataField="Cognome" HeaderText="Cognome" SortExpression="Cognome"></asp:BoundField>
                                                        <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome"></asp:BoundField>
                                                        <asp:BoundField DataField="codiceFiscale" HeaderText="Codice Fiscale">
                                                            <ItemStyle Width="120px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PartitaIva" HeaderText="Partita IVA">
                                                            <ItemStyle Width="120px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Data Inizio">
                                                            <ItemStyle Width="80px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataInizio")) %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Data Fine">
                                                            <ItemStyle Width="80px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataFine")) %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sel.">
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSel" runat="server" AutoPostBack="true" Checked='<%# Business.CoreUtility.FormattaGrdCheck(DataBinder.Eval(Container, "DataItem.bSel")) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("codcontribuente") %>' alt=""></asp:ImageButton>
                                                                <asp:HiddenField runat="server" ID="hfcodcontribuente" Value='<%# Eval("codcontribuente") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </Grd:RibesGridView>
                                                <Grd:RibesGridView ID="GrdImmobili" runat="server" BorderStyle="None"
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
                                                        <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                                                        <asp:BoundField DataField="Cognome" HeaderText="Cognome" SortExpression="Cognome"></asp:BoundField>
                                                        <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Cod.Fiscale/P.IVA">
                                                            <ItemStyle Width="120px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Business.CoreUtility.FormattaCFPIVA(DataBinder.Eval(Container, "DataItem.codiceFiscale"),DataBinder.Eval(Container, "DataItem.PartitaIva")) %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Data Inizio">
                                                            <ItemStyle Width="150px"></ItemStyle>
                                                                <itemtemplate>
																	<asp:Label ID="Label2" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataInizio")) %>'>
																	</asp:Label>
																</itemtemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Data Fine">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataFine")) %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Foglio" HeaderText="Foglio" SortExpression="Foglio"></asp:BoundField>
                                                        <asp:BoundField DataField="Numero" HeaderText="Num." SortExpression="Numero"></asp:BoundField>
                                                        <asp:TemplateField SortExpression="Subalterno" HeaderText="Sub.">
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSubalterno" runat="server" Text='<%# Business.CoreUtility.FormattaGrdInt(DataBinder.Eval(Container, "DataItem.Subalterno")) %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Categoria" HeaderText="Cat."></asp:BoundField>
                                                        <asp:BoundField DataField="Classe" HeaderText="Cl."></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Sel.">
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSel" runat="server" AutoPostBack="true" Checked='<%# Business.CoreUtility.FormattaGrdCheck(DataBinder.Eval(Container, "DataItem.bSel")) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("codcontribuente") %>' alt=""></asp:ImageButton>
                                                                <asp:HiddenField runat="server" ID="hfcodcontribuente" Value='<%# Eval("codcontribuente") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </Grd:RibesGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
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
        <asp:Button ID="CmdGIS" Style="display: none" runat="server"></asp:Button>
    </form>
</body>
</html>
