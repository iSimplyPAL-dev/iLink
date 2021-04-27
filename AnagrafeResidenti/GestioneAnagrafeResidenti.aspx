<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GestioneAnagrafeResidenti.aspx.vb" Inherits="OPENgov.GestioneAnagrafeResidenti"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<html>
	<head>
		<title>GestioneAnagrafeResidenti</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../_js/Utility.js?newversion"></script>
        <script type="text/javascript" type="text/javascript">
			function ApriRicercaAnagrafe(nomeSessione)
			{ 
				winWidth=1200
				winHeight=880 
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40 

				caratteristiche="toolbar=no,status,left="+myleft+",top="+mytop+",height="+winHeight+",width="+winWidth+",resizable"
				Parametri="sessionName=" + nomeSessione 

				WinPopUpRicercaAnagrafica=window.open("../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?"+Parametri,"",caratteristiche) 
				return false;
			}			
			function ApriDatiAnagrafeResidenti(codfiscale, nome, cognome, indirizzo, sesso, datanascita, luogonascita, datamorte, nfamiglia, posizione, azione,codvia)
			{ 
				winWidth=700 
				winHeight=400 
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40 

				caratteristiche="toolbar=no,status,left="+myleft+",top="+mytop+",height="+winHeight+",width="+winWidth+",resizable"
				Parametri="codfiscale=" + codfiscale + "&nome=" + nome + "&cognome=" + cognome + "&indirizzo=" + indirizzo + "&sesso=" + sesso + "&datanascita=" + datanascita + "&luogonascita=" + luogonascita + "&datamorte=" + datamorte + "&nfamiglia=" + nfamiglia + "&posizione=" + posizione + "&azione=" + azione + "&codvia=" + codvia

				WinPopUpRicercaAnagrafica=window.open("DatiAnagrafeResidenti.aspx?"+Parametri,"",caratteristiche) 
				return false;
			}
        </script>
	</HEAD>
	<body class="SfondoVisualizza" leftMargin="20" topMargin="5" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="TblRicerca" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<tr>
					<td>
						<fieldset class="classeFiledSetRicerca">
							<legend class="Legend">Inserimento parametri di Ricerca</legend>
							<table id="TblParametri" cellSpacing="1" cellPadding="1" width="100%" border="0">
								<tr>
									<td class="Input_Label">Cognome<br />
										<asp:textbox id="txtCognome" runat="server" maxLength="100" cssclass="Input_Text" Width="400px"></asp:textbox>
									</td>
									<td class="Input_Label">Nome<br />
										<asp:textbox id="txtNome" runat="server" maxLength="50" cssclass="Input_Text" Width="200px"></asp:textbox>
									</td>
									<td class="Input_Label">Codice Fiscale<br />
										<asp:textbox id="txtCodiceFiscale" runat="server" maxLength="16" cssclass="Input_Text" Width="150px"></asp:textbox>
									</td>
									<td class="Input_Label">N&#176; Famiglia<br />
										<asp:textbox id="txtNumFamiglia" runat="server" cssclass="Input_Text" Width="120px"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td class="Input_Label" colspan="4">
                                        <div class="col-md-2">
										    <asp:RadioButton id="optTrattato" GroupName="Trattato" runat="server" CssClass="Input_Label" Text="Verificati" /><br />
										    <asp:RadioButton id="optNonTrattato" GroupName="Trattato" runat="server" CssClass="Input_Label" Text="Da Verificare" /><br />
										    <asp:RadioButton id="optTuttiTrattato" GroupName="Trattato" runat="server" CssClass="Input_Label" Text="Tutti" Checked="true"/>
									    </div>
									    <div class="col-md-2">
										    <asp:RadioButton id="optSiSuTributo" GroupName="VSTributo" runat="server" CssClass="Input_Label" Text="Abbinati a Tributo" /><br />
										    <asp:RadioButton id="optNoSuTributo" GroupName="VSTributo" runat="server" CssClass="Input_Label" Text="Non Abbinati a Tributo" /><br />
										    <asp:RadioButton id="optTuttiSuTributo" GroupName="VSTributo" runat="server" CssClass="Input_Label" Text="Tutti" Checked="true"/>
									    </div>
									    <div class="col-md-6">
                                            <div class="col-md-12 Input_Emphasized">Per eseguire la ricerca inserire almeno uno tra Cognome, Nome, Codice Fiscale e Numero Famiglia.<br />Per ricercare tutte le posizioni inserire <font class="Input_Label_bold">*</font> in Cognome.</div>
                                        </div>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td colspan="2">
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
				<tr>
					<td valign="top" height="350px">
			            <Grd:RibesGridView ID="GrdRes" runat="server" BorderStyle="None" 
						  BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
						  AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="15"
						  ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
						  OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
						  <PagerSettings Position="Bottom"></PagerSettings>
						  <PagerStyle CssClass="CartListFooter" />
						  <RowStyle CssClass="CartListItem"></RowStyle>
						  <HeaderStyle CssClass="CartListHead"></HeaderStyle>
						  <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				            <Columns>
								<asp:BoundField  HeaderText="Cognome" DataField="COGNOME" SortExpression="COGNOME"></asp:BoundField>
								<asp:BoundField  HeaderText="Nome" DataField="NOME"></asp:BoundField>
								<asp:BoundField HeaderText="Cod. Fiscale" DataField="COD_FISCALE"></asp:BoundField>
								<asp:TemplateField HeaderText="IMU" SortExpression="ICI"><HeaderStyle Width="40"></HeaderStyle><itemtemplate>
									<asp:CheckBox id="ChkICI"  runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.ICI") %>'></asp:CheckBox>
						            </itemtemplate>
					            </asp:TemplateField>
								<asp:TemplateField HeaderText="TARI" SortExpression="TARSU"><HeaderStyle Width="40"></HeaderStyle><itemtemplate>
									<asp:CheckBox id="ChkTARSU"  runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.TARSU") %>'></asp:CheckBox>
						            </itemtemplate>
					            </asp:TemplateField>
								<asp:TemplateField HeaderText="OSAP" SortExpression="OSAP"><HeaderStyle Width="40"></HeaderStyle><itemtemplate>
									<asp:CheckBox id="ChkOSAP"  runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.OSAP") %>'></asp:CheckBox>
						            </itemtemplate>
					            </asp:TemplateField>
								<asp:TemplateField HeaderText="H2O" SortExpression="H2O"><HeaderStyle Width="40"></HeaderStyle><itemtemplate>
									<asp:CheckBox id="ChkH2O"  runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.H2O") %>'></asp:CheckBox>
						            </itemtemplate>
					            </asp:TemplateField>
								<asp:TemplateField HeaderText="PROV" SortExpression="PROVVEDIMENTI"><HeaderStyle Width="40"></HeaderStyle><itemtemplate>
									<asp:CheckBox id="ChkPROV"  runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.PROVVEDIMENTI") %>'></asp:CheckBox>
						            </itemtemplate>
					            </asp:TemplateField>
								<asp:BoundField  HeaderText="N&#176; Famiglia" DataField="NUMERO_FAMIGLIA" SortExpression="NUMERO_FAMIGLIA"></asp:BoundField>
								<asp:BoundField  HeaderText="Posizione" DataField="DESCRIZIONE_POS" SortExpression="DESCRIZIONE_POS"></asp:BoundField>
								<asp:BoundField  HeaderText="Azione" DataField="DESCRIZIONE" SortExpression="DESCRIZIONE"></asp:BoundField>
								<asp:BoundField  HeaderText="Data Azione" DataField="DATA_MOVIMENTO" SortExpression="DATA_MOVIMENTO">
                                    <itemstyle horizontalalign="Right"></itemstyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Verificato">
									<itemstyle horizontalalign="Center"></itemstyle>
								    <itemtemplate>
									    <asp:CheckBox id="ChkVerificato" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.trattato") %>' AutoPostBack="true" OnCheckedChanged="GrdRes_CheckedChanged"></asp:CheckBox>
						                <asp:HiddenField runat="server" ID="hfcod_contribuente" Value='<%# Eval("cod_contribuente") %>' />
                                        <asp:HiddenField runat="server" ID="hfMOVIMENTOID" Value='<%# Eval("MOVIMENTOID") %>' />
                                        <asp:HiddenField runat="server" ID="hfIDDATAANAGRAFICA" Value='<%# Eval("IDDATAANAGRAFICA") %>' />
                                        <asp:HiddenField runat="server" ID="hfCOD_INDIVIDUALE" Value='<%# Eval("COD_INDIVIDUALE") %>' />
                                        <asp:HiddenField runat="server" ID="hfSESSO" Value='<%# Eval("SESSO") %>' />
                                        <asp:HiddenField runat="server" ID="hfDATA_NASCITA" Value='<%# Eval("DATA_NASCITA") %>' />
                                        <asp:HiddenField runat="server" ID="hfDATA_MORTE" Value='<%# Eval("DATA_MORTE") %>' />
                                        <asp:HiddenField runat="server" ID="hfLUOGO_NASCITA" Value='<%# Eval("LUOGO_NASCITA") %>' />
                                        <asp:HiddenField runat="server" ID="hfCOD_VIA" Value='<%# Eval("COD_VIA") %>' />
                                        <asp:HiddenField runat="server" ID="hfVIA" Value='<%# Eval("VIA") %>' />
                                        <asp:HiddenField runat="server" ID="hfNUMERO" Value='<%# Eval("NUMERO") %>' />
                                        <asp:HiddenField runat="server" ID="hfLETTERA" Value='<%# Eval("LETTERA") %>' />
                                        <asp:HiddenField runat="server" ID="hfINTERNO" Value='<%# Eval("INTERNO") %>' />
                                        <asp:HiddenField runat="server" ID="hfCODICE_AZIONE" Value='<%# Eval("CODICE_AZIONE") %>' />
                                        <asp:HiddenField runat="server" ID="hfCODICE_POSIZIONE" Value='<%# Eval("CODICE_POSIZIONE_FAMIGLIA") %>' />
						            </itemtemplate>
					            </asp:TemplateField>
					            <asp:TemplateField HeaderText="">
						            <headerstyle horizontalalign="Center"></headerstyle>
						            <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
						            <itemtemplate>
                                        <div class="tooltip">
 						                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("COD_INDIVIDUALE") %>' alt=""></asp:ImageButton>
                                           <span class="tooltiptext">Visualizza Dettaglio Residente</span>
                                        </div>
						            </itemtemplate>
					            </asp:TemplateField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center"></itemstyle>
									<itemtemplate>
                                        <div class="tooltip">
 										    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneModificaGrd" CommandName="RowEdit" CommandArgument='<%# Eval("cod_contribuente") %>' alt=""></asp:ImageButton>
                                           <span class="tooltiptext">Modifica utente tributario</span>
                                        </div>
									</itemtemplate>
								</asp:TemplateField>
				            </Columns>
				        </Grd:RibesGridView>
					</td>
				</tr>
			</table>
            <div id="divDatiRes" class="modal-box"></div>
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
			<asp:button id="btnSalva" runat="server" CssClass="hidden" Text="Salva"></asp:button>
			<asp:button id="btnRicerca" runat="server" CssClass="hidden" Text="trova"></asp:button>
			<asp:button id="btnView" runat="server" CssClass="hidden" Text="Visualizza"></asp:button>
			<asp:button id="btnAbbina" runat="server" CssClass="hidden" Text="Abbina"></asp:button>
			<asp:imagebutton id="MyImgButton" runat="server" style="DISPLAY: none" CausesValidation="False"></asp:imagebutton>
			<asp:button id="btnRibalta" runat="server" CssClass="hidden" Text="Ribalta" Width="1px" Height="2px"></asp:button>
			<asp:button id="btnStampaExcel" runat="server" CssClass="hidden" Text="Button"></asp:button>
		</form>
	</body>
</html>
