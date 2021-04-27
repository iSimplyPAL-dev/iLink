<%@ Page Language="c#" CodeBehind="SituazioneAvvisiSearch.aspx.cs" AutoEventWireup="True" Inherits="OPENgovTOCO.SituazioneContribuente.SituazioneAvvisiSearch" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Ricerca Situazione Contribuente</title>
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
    <script type="text/javascript" src="../../_js/Toco.js?newversion"></script>
</head>
<body style="overflow: auto" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout" class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: auto">
            <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td><span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                        <asp:Label ID="lblTitolo" runat="server"></asp:Label></span></td>
                    <td align="right" rowspan="2">
						<asp:imagebutton id="Stampa" runat="server" Cssclass="Bottone BottoneExcel" ToolTip="Stampa" ImageUrl="../../images/Bottoni/transparent28x28.png" onclick="Stampa_Click"></asp:imagebutton>
                        <asp:ImageButton ID="Search" runat="server" CssClass="Bottone BottoneRicerca" ToolTip="Ricerca Avvisi" ImageUrl="../../images/Bottoni/transparent28x28.png" OnClick="Search_Click"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><span id="info" runat="server" class="NormalBold_title" style="width: 400px; height: 20px">- Avvisi - Consultazione</span>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table id="TblRicerca" cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td>
                        <fieldset class="FiledSetRicerca">
                            <legend class="Legend">Inserimento filtri di ricerca</legend>
                            <table id="TblParametri" cellspacing="1" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td>
                                        <asp:Panel ID="PanelSoggetto" runat="server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label2" CssClass="Input_Label" runat="server">Cognome</asp:Label><br>
                                                        <asp:TextBox ID="txtCognome" runat="server" CssClass="Input_Text" Width="376px"></asp:TextBox></td>
                                                    <td>
                                                        <asp:Label ID="Label3" CssClass="Input_Label" runat="server">Nome</asp:Label><br>
                                                        <asp:TextBox ID="txtNome" runat="server" CssClass="Input_Text" Width="185px"></asp:TextBox></td>
                                                    <td>
                                                        <asp:Label ID="Label4" CssClass="Input_Label" runat="server">Codice Fiscale</asp:Label><br>
                                                        <asp:TextBox ID="txtCodFiscale" runat="server" CssClass="Input_Text" Width="160px" MaxLength="16"></asp:TextBox></td>
                                                    <td>
                                                        <asp:Label ID="Label5" CssClass="Input_Label" runat="server">Partita IVA</asp:Label><br>
                                                        <asp:TextBox ID="txtPIva" runat="server" CssClass="Input_Text" Width="160px" MaxLength="11"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label9" CssClass="Input_Label" runat="server">Anno</asp:Label><br>
                                                        <asp:DropDownList ID="ddlAnnoAvviso" runat="server" CssClass="Input_Text" Width="80px"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label7" CssClass="Input_Label" runat="server">Numero avviso</asp:Label><br>
                                                        <asp:TextBox ID="txtNumeroAvviso" runat="server" CssClass="Input_Text" Width="185px"></asp:TextBox>
                                                    </td>
									                <td>
										                <asp:label CssClass="Input_Label" Runat="server" id="Label8">Sgravate</asp:label><br />
										                <asp:CheckBox id="ChkSgravate" runat="server" CssClass="Input_CheckBox_NoBorder" Text=" "></asp:CheckBox>
									                </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LblResultAvvisi" CssClass="Legend" runat="server" Visible="False">Non sono presenti avvisi</asp:Label>
                        <Grd:RibesGridView ID="GrdAvvisi" runat="server" BorderStyle="None"
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
                                <asp:TemplateField HeaderText="Nominativo">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label8" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.DettaglioContribuente.Cognome")) + " " + (DataBinder.Eval(Container, "DataItem.DettaglioContribuente.Nome")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cod.Fiscale/P.IVA">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label11" runat="server" Text='<%# OPENgovTOCO.SharedFunction.FormattaCFPIVA(DataBinder.Eval(Container, "DataItem.DettaglioContribuente.CodiceFiscale"),DataBinder.Eval(Container, "DataItem.DettaglioContribuente.PartitaIva")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField Visible="True" HeaderText="Anno" DataField="Anno">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField Visible="True" HeaderText="Numero Avviso" DataField="CodiceCartella">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField Visible="True" HeaderText="Data Avviso">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="Label10" runat="server" Text='<%# OPENgovTOCO.SharedFunction.FormattaData (DataBinder.Eval(Container, "DataItem.DataEmissione")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField Visible="True" HeaderText="Totale" DataField="ImportoCarico" DataFormatString="{0:0.00}">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField Visible="True" HeaderText="Pagato" DataField="ImpPagato" DataFormatString="{0:0.00}">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
								<asp:TemplateField HeaderText="Sgravio">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:ImageButton runat="server" CssClass="BottoneGrd" alt="" ToolTip='<%# OPENgovTOCO.SharedFunction.FormattaToolTipIsSgravato(DataBinder.Eval(Container, "DataItem.IsSgravio"),DataBinder.Eval(Container, "DataItem.IMPPRESGRAVIO")) %>' ImageUrl='<%# OPENgovTOCO.SharedFunction.FormattaIsSgravato(DataBinder.Eval(Container, "DataItem.IsSgravio")) %>'>
										</asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" CssClass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDCARTELLA") %>' alt=""></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Grd:RibesGridView>
                    </td>
                </tr>
            </table>
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
    </form>
</body>
</html>
