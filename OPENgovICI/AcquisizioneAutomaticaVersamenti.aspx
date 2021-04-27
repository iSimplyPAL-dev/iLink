<%@ Page language="c#" Codebehind="AcquisizioneAutomaticaVersamenti.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.AcquisizioneAutomaticaVersamenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AcquisizioneAutomaticaVersamenti</title>
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
	</HEAD>
	<body class="Sfondo" leftMargin="3" rightMargin="3" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="tblCorpo" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<tr>
					<td><asp:panel id="pnlFiltriRicerca" runat="server">
							<FIELDSET class="classeFiledSetRicerca"><LEGEND class="Legend">Scegli il 
									tracciato di versamenti da importare</LEGEND>
								<table id="tblImportazione" cellSpacing="1" cellPadding="5" width="100%" border="0">
									<tr>
										<td align="left"><input id="fileUpload" type="file" size="50" name="fileUpload" runat="server">&nbsp;
											<asp:dropdownlist id="ddlFormatoImport" runat="server" CssClass="Input_Text">
												<asp:ListItem Value="f24">F24</asp:ListItem>
											</asp:dropdownlist>&nbsp; 
                                            <!-- Valori di ddlFormatoImport che non servono per Grand Combin -->  
                                            <!--<asp:ListItem Value="postel">POSTEL</asp:ListItem>
										        <asp:ListItem Value="unirisc">UNIRISCOSSIONE</asp:ListItem>
												<asp:ListItem Value="ancicnc">ANCI/CNC</asp:ListItem>
										        <asp:ListItem Value="risconet">RISCONET</asp:ListItem>
										        <asp:ListItem Value="uvi">UVI</asp:ListItem>
										        <asp:ListItem Value="versamentixml">Versamenti XML</asp:ListItem>
                                            -->
											<asp:linkbutton id="lbtnImporta" runat="server" onclick="lbtnImporta_Click">Importa</asp:linkbutton>
										</td>
									</tr>
                                    <tr id="TRImportFolder" style="display:none;">
                                        <td>
                                            <asp:TextBox ID="txtPathImport" runat="server" CssClass="Input_Text" Width="400px"></asp:TextBox>&nbsp;
                                            <asp:linkbutton id="lbtImportFolder" runat="server" onclick="lbtImportFolder_Click">Importa da cartella</asp:linkbutton>
                                        </td>
                                    </tr>
									<tr>
										<td class="Input_Label" align="left"></td>
									</tr>
									<tr>
										<td class="Input_Label" align="left">
											<asp:Label id="lblUltimaImportazione" runat="server" Font-Bold="True">Ultima importazione:</asp:Label></td>
									</tr>
									<tr>
										<td class="Input_Label" align="left">
											<asp:Label id="lblOperatore" runat="server">Operatore:</asp:Label></td>
									</tr>
									<tr>
										<td class="Input_Label" align="left">
											<asp:Label id="lblFileName" runat="server">File importato:</asp:Label></td>
									</tr>
									<tr>
										<td class="Input_Label" align="left">
											<asp:Label id="lblEsito" runat="server">Esito:</asp:Label></td>
									</tr>
									<tr>
										<td>
											<table id="tbldatiImport" cellSpacing="1" cellPadding="1" width="100%" border="0">
												<tr>
													<td class="Input_Label" style="WIDTH: 15%" align="left">
														<asp:Label id="lblNrecFile" runat="server">Totale record importati:</asp:Label></td>
													<td class="Input_Label" align="left">
														<asp:Label id="lblRecImport" runat="server"></asp:Label></td>
												</tr>
												<tr>
													<td class="Input_Label" style="WIDTH: 15%" align="left">
														<asp:Label id="lblImportoTot" runat="server">Importo totale importato:</asp:Label></td>
													<td class="Input_Label" align="left">
														<asp:Label id="lblImpoTotImport" runat="server"></asp:Label></td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</FIELDSET>
						</asp:panel>
						<P><asp:label id="lblImportazioneInCorso" runat="server" Font-Bold="True" Visible="False" Font-Size="Medium">Un'importazione è già attiva, ricollegarsi quando conclusa</asp:label></P>
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
            <asp:Button ID="btnStampaExcel" Style="display: none" runat="server" Text="Button"></asp:Button>
		</form>
	</body>
</HTML>
