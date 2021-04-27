<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConfRate.aspx.vb" Inherits="OPENgovTIA.ConfRate" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>ConfRate</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
	</head>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
		    <div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: hidden">
		        <table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
			        <tr>
				        <td align="left">
					        <span class="ContentHead_Title" id="infoEnte">
						        <asp:Label id="lblTitolo" runat="server"></asp:Label>
					        </span>
				        </td>
				        <td align="right" colSpan="2" rowSpan="2" width="80%">
						    <input class="Bottone BottoneSalva" id="Insert" title="Salva Rata" onclick="document.getElementById('CmdSalva').click();" type="button" name="Insert" />
						    <input class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla Gestione" onclick="location.href='../../Avvisi/Calcolo/Calcolo.aspx?IsFromVariabile=<%=Request.Item("IsFromVariabile")%>';" type="button" name="Delete" />
				        </td>
			        </tr>
			        <tr>
				        <td align="left" colspan="2">
					        <span class="NormalBold_title" id="info" runat="server"> Variabile - Configurazioni - Rate</span>
				        </td>
			        </tr>
		        </table>
		    </div>
		    &nbsp;
			<table width="100%">
				<tr>
				    <td colspan="2">
                        <fieldset class="classeFieldSetRicerca hidden" runat="server">
                            <legend class="Legend" runat="server">Tipologia Calcolo</legend>
                            <div class="col-md-12">
                                <div class="col-md-2">
                                    <label class="Input_Label">Calcolo</label>&nbsp;
				                    <asp:Label runat="server" CssClass="Input_Label" ID="LblTipoCalcolo"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <label class="Input_Label"> su Superfici</label>&nbsp;
				                    <asp:Label runat="server" CssClass="Input_Label" ID="LblTipoMQ"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:CheckBox ID="ChkConferimenti" runat="server" CssClass="Input_Label" Text="presenza Conferimenti" />
                                </div>
                                <div class="col-md-2">
                                    <asp:CheckBox ID="ChkMaggiorazione" runat="server" CssClass="Input_Label" Text="presenza Maggiorazione" />
                                </div>
                            </div>
                        </fieldset>
				    </td>
				</tr>
				<tr>
					<td colspan="2">
						<fieldset><asp:label id="LblInfo" Runat="server" CssClass="Input_Label">
								Per una corretta configurazione delle rate procedere come segue:<br /><br />
								- cliccare con il mouse sulla riga vuota per inserire una nuova rata;<br />
								- popolare i dati spostandosi, da una colonna all'altra, utilizzando la tabulazione e/o le freccie direzionali;<br />
							</asp:label><br />
						</fieldset>
					</td>
				</tr>
				<tr>
				    <td>
				        <div class="classeFieldSetRicerca" runat="server">
				            <asp:Label runat="server" CssClass="Legend" Text="Canale di Pagamento"></asp:Label>
				            <br />
				            <asp:RadioButton ID="rbF24" runat="server" GroupName="CanalePagamento" Checked="true" Text="F24" CssClass="Input_Label" ToolTip="F24"/><br />
				            <asp:RadioButton ID="rbBollettinoTD896" runat="server" GroupName="CanalePagamento" Checked="false" Text="Bollettino Postale Premarcato" CssClass="Input_Label" ToolTip="TD896"/>&nbsp;&nbsp;
				            <asp:RadioButton ID="rbBollettinoTD123" runat="server" GroupName="CanalePagamento" Checked="false" Text="Bollettino Postale Bianco Precompilato" CssClass="Input_Label" ToolTip="TD123"/>
				        </div>
				    </td>
				    <td>
			            <div class="classeFieldSetRicerca" runat="server">
				            <asp:Label ID="Label1" runat="server" CssClass="Legend" Text="Soglia minima rateizzazione"></asp:Label>
				            <br />
			                <asp:Label runat="server" CssClass="Input_Label"></asp:Label><br />
				            &nbsp;<asp:TextBox runat="server" ID="TxtSogliaMinima" CssClass="Input_Text_right OnlyNumber"></asp:TextBox>
				        </div>
				    </td>
				</tr>
				<tr>
					<td Width="100%" colspan="2">
						<Grd:RibesGridView ID="GrdRate" runat="server" BorderStyle="None" 
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
								<asp:TemplateField HeaderText="N.Rata">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtNRata" runat="server" Width="100px" CssClass="Input_Text_Right" Text='<%# DataBinder.Eval(Container, "DataItem.NumeroRata") %>'>
										</asp:TextBox>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Descrizione">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtDescrRata" runat="server" CssClass="Input_Text" Text='<%# DataBinder.Eval(Container, "DataItem.DescrizioneRata") %>'>
										</asp:TextBox>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Data Scadenza">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtDataScadenza" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataScadenza")) %>'>
										</asp:TextBox>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="%">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox runat="server" id="txtPercentuale" Width="100px" CssClass="Input_Text_Right OnlyNumber" Text='<%# (DataBinder.Eval(Container, "DataItem.Percentuale")) %>'></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imposta">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:CheckBox runat="server" ID="ChkImposta" Checked='<%# (DataBinder.Eval(Container, "DataItem.HasImposta")) %>'/>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Maggiorazione">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:CheckBox runat="server" ID="ChkMaggiorazione" Checked='<%# (DataBinder.Eval(Container, "DataItem.HasMaggiorazione")) %>'/>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="">
	                                <headerstyle horizontalalign="Center"></headerstyle>
	                                <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
	                                <itemtemplate>
                                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneNewInsertGrd" CommandName="RowNew" alt=""></asp:ImageButton>
	                                </itemtemplate>
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
			<asp:Button ID="CmdSalva" Runat="server" style="DISPLAY:none"></asp:Button>
		</form>
	</body>
</html>
