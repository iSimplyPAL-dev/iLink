<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Dettaglio.aspx.vb" Inherits="Provvedimenti.Dettaglio" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<%@ Register TagPrefix="uc1" TagName="ElencoRate" Src="WUCRate.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Dettaglio</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" src="../../_js/skype_killer.js?newversion"></script>
    <script type="text/javascript">
        window.onload = killSkype;
        function InserisciNuovoPagamento() {
            parent.Comandi.location.href = "cmdPagamenti.aspx?from=Dettaglio"
            parent.Visualizza.location.href = "Pagamenti.aspx?from=Dettaglio"
        }
        function EliminaAccorpamento() {
            if (confirm('Si desidera eliminare l\'accorpamento/rateizzazione selezionato?'))
                document.getElementById("btnEliminaAccorpamento").click()
        }
        function Abilita_btnDelete(valore) {
            parent.Comandi.Abilita_btnDelete(valore)
        }
        function Abilita_btnPagamento(valore) {
            parent.Comandi.Abilita_btnPagamento(valore)
        }

    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
		    <table width="100%">
				<!--blocco dati contribuente--><%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
				<tr id="TRPlainAnag">
				    <td>
				        <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				        <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				    </td>
				</tr>
				<tr id="TRSpecAnag">
				    <td>
			            <table id="tblGetAnagrafica" border="0" cellPadding="0" width="100%" style="DISPLAY:none">
				            <tr>
					            <td class="Input_Label">
						            <asp:label id="lblNominativo" runat="server">Nominativo</asp:label>&nbsp;
						            <asp:button id="btnFocus" runat="server" width="1px" height="1px"></asp:button><asp:imagebutton id="Imagebutton" runat="server" causesvalidation="False" class="BottoneSel BottoneLista"></asp:imagebutton>
					            </td>
				            </tr>
				            <tr>
					            <td>
						            <asp:textbox id="txtNominativo" tabIndex="4" runat="server" width="492px" tooltip="Nominativo"
							            cssclass="Input_Text" enabled="False"></asp:textbox>
						            <IMG id="imageDelete" onmouseover="this.style.cursor='hand'" onclick="DeleteContrib();"
							            alt="Pulisci Nominativo Selezionato" src="../images/cancel.png" Width="10px" Height="10px">
						            <asp:button id="btnRibalta" runat="server" width="1px" height="2px" cssclass="displaynone" text="Ribalta"></asp:button>
						            <asp:textbox id="txtHiddenIdDataAnagrafica" runat="server" width="24px" height="20px" cssclass="displaynone">-1</asp:textbox>
						            <asp:button id="btnSearchProvvedimenti" runat="server" width="1px" height="2px" cssclass="displaynone"
							            text="Provvedimenti"></asp:button>
					            </td>
				            </tr>
			            </table>
			            <table id="tblDatiAnagrafica" class="dati_anagrafe_tarsu_blu" border="0" cellspacing="0"
				            cellpadding="2" width="100%" height="102" style="DISPLAY:none">
				            <tr>
					            <td>
						            <asp:label id="lblCognomeNome" runat="server" height="12px" width="243px"></asp:label>
					            </td>
					            <td>
						            <asp:label id="Label32" runat="server" height="12px" width="32px">CF/P.IVA:</asp:label>
						            <asp:label id="lblCfPiva" runat="server"></asp:label></td>
					            <td>
						            <asp:label id="Label33" runat="server" height="12px" width="32px">SESSO:</asp:label>
						            <asp:label id="lblSesso" runat="server"></asp:label>
					            </td>
				            </tr>
				            <tr>
					            <td>
						            <asp:label id="Label34" runat="server" height="12px" width="131px">DATA DI NASCITA:</asp:label>
						            <asp:label id="lblDataNascita" runat="server"></asp:label>
					            </td>
					            <td colspan="2">
						            <asp:label id="Label35" runat="server" height="12px" width="159px">COMUNE DI NASCITA:</asp:label>
						            <asp:label id="lblComuneNascita" runat="server"></asp:label>
					            </td>
				            </tr>
				            <tr>
					            <td colspan="3">
						            <asp:label id="Label36" runat="server" height="12px" width="116px">RESIDENTE IN:</asp:label>
						            <asp:label id="lblResidenza" runat="server" width="550px"></asp:label>
					            </td>
				            </tr>
			            </table>
				    </td>
				</tr>
		    </table>
        <br>
        <fieldset>
            <legend class="Legend">Elenco provvedimenti</legend>
            <asp:Label ID="lblInfoProvv" runat="server" Visible="False" CssClass="Input_Label NormalRed"></asp:Label>
            <Grd:RibesGridView ID="GrdProvvedimenti" runat="server" BorderStyle="None"
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="99%"
                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="anno" ReadOnly="True" HeaderText="Anno">
                        <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="numero_atto" ReadOnly="True" HeaderText="Atto">
                        <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="descrtrib" ReadOnly="True" HeaderText="Tributo">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Imp.Ridotto">
                        <HeaderStyle HorizontalAlign="Center" Width="14%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="LblImpRidotto" runat="server" Text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.importo_totale_ridotto"),2) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imp.Pieno">
                        <HeaderStyle HorizontalAlign="Center" Width="14%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="LblImpPieno" runat="server" Text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.importo_totale"),2) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Data Elaborazione">
                        <HeaderStyle HorizontalAlign="Center" Width="14%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblDal" runat="server" Text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_elaborazione")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Data Notifica">
                        <HeaderStyle HorizontalAlign="Center" Width="14%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_notifica_avviso")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Data Consegna">
                        <HeaderStyle HorizontalAlign="Center" Width="14%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_consegna_avviso")) %>'>
                            </asp:Label>
                            <asp:HiddenField runat="server" ID="hfid_provvedimento" Value='<%# Eval("id_provvedimento") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Grd:RibesGridView>
        </fieldset>
        <br>
        <uc1:ElencoRate id="ElencoRate" runat="server"></uc1:ElencoRate>
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
		<asp:HiddenField runat="server" ID="hfIdProvvedimento" Value="0" />
		<asp:HiddenField runat="server" ID="hfIdAccorpamento" value="0"/>
        <asp:Button ID="btnEliminaAccorpamento" runat="server" Width="1px" Height="2px" CssClass="displaynone" Text="EliminaAccorpamento"></asp:Button>
        <fieldset id="fldPagamento" class="displaynone"></fieldset>
    </form>
</body>
</html>
