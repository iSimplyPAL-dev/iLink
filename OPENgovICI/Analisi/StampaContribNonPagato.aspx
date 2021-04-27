<%@ Page language="c#" Codebehind="StampaContribNonPagato.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.StampaContribNonPagato" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>StampaContribNonPagato</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" event="onkeypress" for="document">
			if(window.event.keyCode==13){
				parent.Comandi.getElementById('Insert').click();
			}
		</script>	
		<script type="text/javascript">
			function controlloanno(){
			    GestAlert('a', 'warning', '', '', 'Selezionare l\'anno di riferimento');
				document.getElementById("ddlAnnoRiferimento").focus();
			} 
			function estraiExcel() {
			    if (document.getElementById('GrdRisultati') == null) {
			        GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Eseguire la ricerca.');
			    }
			    else {
			        document.getElementById('DivAttesa').style.display = '';
			        document.getElementById('btnStampaExcel').click()
			    }
			    return false;
			}
		</script>
	</HEAD>
	<body class="Sfondo" leftMargin="3" rightMargin="3" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<asp:button id="btnStampaExcel" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
			<asp:button id="btnTrova" style="DISPLAY: none" runat="server" Text="Trova" ToolTip="Permette di eseguire una ricerca in funzione dei filtri utilizzati" onclick="btnTrova_Click"></asp:button>
			<table id="tblCorpo" cellSpacing="1" cellPadding="1" width="100%" align="center" border="0">
				<tr>
					<td>
			            <asp:Label id="lblTesto" Runat="server" CssClass="Legend">Elenco dei contribuenti che, in base all'anno selezionato, non hanno effettuato versamenti.</asp:Label>
			            <br />
					</td>
				</tr>
				<tr>
					<td>
						<fieldset class="classeFiledSetRicerca"><legend class="Legend">Inserimento parametri di ricerca</legend>
							<table id="tblFiltri" cellSpacing="1" cellPadding="5" width="100%" border="0">
								<tr>
									<td>
										<asp:Label id="lblAnnoRif" CssClass="Input_Label" Runat="server">Anno di Riferimento</asp:Label><br />
										<asp:dropdownlist id="ddlAnnoRiferimento" runat="server" Width="200px" CssClass="Input_Text" DESIGNTIMEDRAGDROP="24"></asp:dropdownlist>
									</td>
									<!--*** 20140630 - TASI ***-->
		                            <td>
		                                <asp:CheckBox ID="chkICI" runat="server" CssClass="Input_Label" Text="ICI/IMU" Checked="true"/>
		                                &nbsp;
		                                <asp:CheckBox ID="chkTASI" runat="server" CssClass="Input_Label" Text="TASI" Checked="false"/>
		                            </td>
									<!--*** ***-->
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td colspan="6">
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
				<tr>
					<td>
	                    <asp:label id="lblRisultati" runat="server" CssClass="Legend">Risultati della Ricerca</asp:label>
	                    <br />
						<Grd:RibesGridView ID="GrdRisultati" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="20"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnPageIndexChanging="GrdPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
	                            <asp:BoundField DataField="COGNOME" HeaderText="Cognome" SortExpression="Cognome">
		                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
	                            </asp:BoundField>
	                            <asp:BoundField DataField="NOME" HeaderText="Nome" SortExpression="Nome">
		                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
	                            </asp:BoundField>
	                            <asp:BoundField DataField="CFPIVA" HeaderText="Codice Fiscale/Partita Iva">
		                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
	                            </asp:BoundField>
								<asp:BoundField DataField="AnnoRiferimento" ReadOnly="True" HeaderText="Anno" SortExpression="AnnoRiferimento">
									<HeaderStyle ></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundField>
	                            <asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Tributo">
		                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
		                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
	                            </asp:BoundField>
								<asp:TemplateField HeaderText="Dovuto ">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="Label2" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoDovuto")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>
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
		</form>
	</body>
</HTML>
