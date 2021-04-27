<%@ Page language="c#" Codebehind="RicercaImmobile.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.Anater.RicercaImmobile" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RicercaImmobile</title>
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
		<script type="text/javascript">
			function RibaltaUIAnater()
			{
				opener.document.getElementById('CmdRibaltaUIAnater').click();
				window.close();
			}
		
			function ApriStradario(){
				var CodEnte = '<% = Session["COD_ENTE"] %>';
				var TipoStrada = '';
				var Strada = '';
				var CodStrada = document.getElementById('txtCodVia').value;
                var CodTipoStrada = '';
                var Frazione = '';
				var CodFrazione = '';
                
                var Parametri = '';
                
                Parametri += 'CodEnte='+CodEnte;
                Parametri += '&TipoStrada='+TipoStrada;
                Parametri += '&Strada='+Strada;
                Parametri += '&CodStrada='+CodStrada;
                Parametri += '&CodTipoStrada='+CodTipoStrada;
                Parametri += '&Frazione='+Frazione;
                Parametri += '&CodFrazione='+CodFrazione;
                Parametri += '&Stile=<% = Session["StileStradario"] %>';
                Parametri += '&FunzioneRitorno=RibaltaStrada'
                
				window.open('<% = this.UrlStradario %>?'+Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');
				
				// l'istruzione seguente ritorna false per non fare il postback della pagina.
				return false;
			}
			
			function RibaltaStrada(objStrada)
			{
				// popolo il campo descrizione della via di residenza
				var strada
				if (objStrada.TipoStrada != '&nbsp;')
				{
					strada= objStrada.TipoStrada;
				}
				if (objStrada.Strada != '&nbsp;')
				{
					strada=strada+ ' ' + objStrada.Strada;
				}
				if (objStrada.Frazione!='CAPOLUOGO')
				{
					strada= strada+ ' ' + objStrada.Frazione;
				}
				strada = strada.replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
				document.getElementById('txtCodVia').value = objStrada.CodStrada;
				document.getElementById('TxtVia').value=strada;
			}
			
			function ClearDatiVia()
			{
				document.getElementById('TxtVia').value='';
				document.getElementById('txtCodVia').value='';
				return false;
			}
		</script>
	</HEAD>
	<body class="Sfondo" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="3" rightMargin="3">
		<form id="Form1" runat="server" method="post">
			<table width="100%">
				<tr class="SfondoGenerale">
					<td>
						<table class="SfondoGenerale" cellSpacing="0" cellPadding="0" width="100%" align="right"
							border="0" id="Table1">
							<tr>
								<td style="WIDTH: 464px; HEIGHT: 20px" align="left">
									<span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
										<asp:Label id="lblTitolo" runat="server"></asp:Label>
									</span>
								</td>
								<td align="right" width="800" colSpan="2" rowSpan="2">
									<input class="Bottone BottoneAssocia" id="Associa" title="Associa" onclick="document.RicercaUIAnater.CmdRibaltaAnater.click();"
										type="button" name="Associa"> <input class="Bottone BottoneRicerca" id="Ricerca" title="Ricerca" onclick="document.RicercaUIAnater.CmdRicerca.click();"
										type="button" name="Ricerca"> <input class="Bottone BottoneAnnulla" id="Esci" title="Esci" onclick="window.close()" type="button"
										name="Esci">&nbsp;
								</td>
							</tr>
							<tr>
								<td style="WIDTH: 463px" align="left">
									<span class="NormalBold_title" id="info" style="WIDTH: 400px; HEIGHT: 20px;">
										ICI/IMU - Dichiarazioni - Ricerca Immobile Territorio</span>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<tr>
								<td borderColor="darkblue" colSpan="4">
									<table id="tblDatiContribuente" cellSpacing="0" cellPadding="0" width="100%" border="1">
										<tr>
											<td borderColor="darkblue">
												<table cellSpacing="1" cellPadding="1">
													<tr>
														<td class="DettagliContribuente" width="160">Cognome/Rag. Sociale</td>
														<td class="DettagliContribuente" align="left">
															<asp:label id="lblCognomeContr" runat="server"></asp:label>
														</td>
														<td class="DettagliContribuente" width="110">&nbsp;&nbsp;Nome</td>
														<td class="DettagliContribuente">
															<asp:label id="lblNomeContr" runat="server"></asp:label>
														</td>
													</tr>
													<tr>
														<td class="DettagliContribuente">Data di Nascita</td>
														<td class="DettagliContribuente" align="left">
															<asp:label id="lblDataNascContr" runat="server"></asp:label>
														</td>
														<td>&nbsp;</td>
														<td>&nbsp;</td>
													</tr>
													<tr>
														<td class="DettagliContribuente">RESIDENTE IN</td>
														<td class="DettagliContribuente" align="left">
															<asp:label id="lblResidContr" runat="server"></asp:label>
														</td>
														<td class="DettagliContribuente">&nbsp; Comune (Prov.)</td>
														<td class="DettagliContribuente" align="left">
															<asp:label id="lblComuneContr" runat="server"></asp:label>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<fieldset class="FiledSetRicerca"><LEGEND class="Legend">Inserimento filtri di ricerca</LEGEND>
										<table id="TblParametri" cellSpacing="1" cellPadding="1" width="100%" border="0">
											<tr>
												<td colSpan="3">
													<asp:label id="Label7" Runat="server" CssClass="Input_Label">Via</asp:label>&nbsp;
													<asp:imagebutton id="LnkApriStradario" runat="server" ImageUrl="../../images/Bottoni/Listasel.png"
														ToolTip="Apri Ricerca Stradario" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>&nbsp;
													<asp:imagebutton id="LnkPulisciStrada" runat="server" ToolTip="Pulisci i campi della Via" CausesValidation="False"
														imagealign="Bottom" ImageUrl="../../images/Bottoni/cancel.png"></asp:imagebutton>
													<asp:TextBox ID="txtCodVia" Runat="server" style="DISPLAY:none">-1</asp:TextBox>
													<BR>
													<asp:textbox id="TxtVia" runat="server" Width="376px" CssClass="Input_Text" ReadOnly="True"></asp:textbox>
												</td>
												<td>
													<asp:label id="Label1" Runat="server" CssClass="Input_Label">Civico</asp:label><BR>
													<asp:textbox id="TxtCivico" runat="server" Width="50px" CssClass="Input_Text"></asp:textbox>
												</td>
												<td>
													<asp:label id="Label11" Runat="server" CssClass="Input_Label">Interno</asp:label><BR>
													<asp:textbox id="TxtInterno" runat="server" Width="50px" CssClass="Input_Text"></asp:textbox>
												</td>
											</tr>
											<tr>
												<td width="150">
													<asp:label id="Label2" Runat="server" CssClass="Input_Label">Foglio</asp:label><BR>
													<asp:textbox id="TxtFoglio" runat="server" Width="120px" CssClass="Input_Text"></asp:textbox>
												</td>
												<td width="150">
													<asp:label id="Label3" Runat="server" CssClass="Input_Label">Numero</asp:label><BR>
													<asp:textbox id="TxtNumero" runat="server" Width="120px" CssClass="Input_Text"></asp:textbox>
												</td>
												<td width="150">
													<asp:label id="Label4" Runat="server" CssClass="Input_Label">Subalterno</asp:label><BR>
													<asp:textbox id="TxtSubalterno" runat="server" Width="120px" CssClass="Input_Text"></asp:textbox>
												</td>
												<td></td>
												<td></td>
											</tr>
										</table>
									</fieldset>
								</td>
							</tr>
							<tr>
								<td>
									<asp:label id="LblResult" CssClass="Legend" Runat="server">Risultati della Ricerca</asp:label>
								</td>
							</tr>
							<tr>
								<td>
									<Grd:RibesGridView ID="GrdRicerca" runat="server" BorderStyle="None" 
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
											<asp:TemplateField HeaderText="Ubicazione">
												<ItemStyle Width="300px"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label35" runat="server" text='<%# Business.CoreUtility.FormattaVia(DataBinder.Eval(Container, "DataItem.Indirizzo"),DataBinder.Eval(Container, "DataItem.civico"),DataBinder.Eval(Container, "DataItem.Interno"),DataBinder.Eval(Container, "DataItem.Esponente"),DataBinder.Eval(Container, "DataItem.scala"),DataBinder.Eval(Container, "DataItem.piano")) %>'>Label</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="FOGLIO" HeaderText="Foglio">
												<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center" Width="56px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="NUMERO" HeaderText="Numero">
												<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center" Width="56px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="SUBALTERNO" HeaderText="Subalterno">
												<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center" Width="56px"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="Data Inizio">
												<ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label5" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATA_INIZIO")) %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Data Fine">
												<ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label8" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATA_FINE")) %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="catcatastale" HeaderText="Cat.Catastale">
												<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="CLASSECATASTALE" HeaderText="Classe">
												<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="VALORECATASTALE" HeaderText="Valore" DataFormatString="{0:0.00}">
												<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="PERCENTUALE_PROPRIETA" HeaderText="%Proprietà">
												<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="Sel.">
												<ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
												<ItemTemplate>
													<asp:CheckBox id="ChkSelezionato" runat="server" AutoPostBack="True">
													</asp:CheckBox>
												</ItemTemplate>
												<EditItemTemplate>
													<asp:TextBox id="TextBox1" runat="server"></asp:TextBox>
												</EditItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="">
									            <headerstyle horizontalalign="Center"></headerstyle>
									            <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									            <itemtemplate>
										            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' alt=""></asp:ImageButton>
					                                <asp:HiddenField runat="server" ID="hfIDUI" Value='<%# Eval("IDUI") %>' />
                                                    <asp:HiddenField runat="server" ID="hfCOD_PROPRIETA" Value='<%# Eval("COD_PROPRIETA") %>' />
                                                    <asp:HiddenField runat="server" ID="hfCOD_PROPRIETARIO" Value='<%# Eval("COD_PROPRIETARIO") %>' />
                                                    <asp:HiddenField runat="server" ID="hfCOD_TIPO_RENDITA" Value='<%# Eval("COD_TIPO_RENDITA") %>' />
                                                    <asp:HiddenField runat="server" ID="hfID_VIA" Value='<%# Eval("ID_VIA") %>' />
                                                    <asp:HiddenField runat="server" ID="hfCOD_TIPO_PROPRIETA" Value='<%# Eval("COD_TIPO_PROPRIETA") %>' />
                                                    <asp:HiddenField runat="server" ID="hfMESIPOSSESSO" Value='<%# Eval("MESIPOSSESSO") %>' />
									            </itemtemplate>
								            </asp:TemplateField>
										</Columns>
									</Grd:RibesGridView>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<input type="hidden" id="txtIdContribuente" runat="server">
			<asp:Button ID="CmdRicerca" style="DISPLAY: none" Runat="server" onclick="CmdRicerca_Click"></asp:Button>
			<asp:button id="CmdRibaltaAnater" style="DISPLAY: none" Runat="server" onclick="CmdRibaltaAnater_Click"></asp:button>
		</form>
	</body>
</HTML>
