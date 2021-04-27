<%@ Page language="c#" Codebehind="Dichiarazione.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.DichiarazionePage" enableViewStateMac="False" EnableEventValidation="false"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>DichiarazionePage</title>
		<meta content="True" name="vs_showGrid">
		<meta content="False" name="vs_snapToGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">
			.linkDenunciante { BORDER-BOTTOM: darkblue 1px solid; FONT-VARIANT: small-caps; FONT-FAMILY: Tahoma, Arial, Verdana; FONT-SIZE: x-small; CURSOR: hand; FONT-WEIGHT: bold; TEXT-DECORATION: none }
		</style>
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%> 
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%> 
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../_js/ControlloData.js?newversion"></script>
		<script type="text/javascript">		
			function Search(IdTestata)
			{
			    loadImmobile.location.href = 'Immobile.aspx?IdTestata=' + IdTestata
				/*loadGrid.location.href="./ResultRicercaCategoria.aspx"*/
				return true;
			}
			
			function RibaltaInAnater(iRetVal)
			{
				//alert(iRetVal)
				if (iRetVal==2)
				{
					//*** 20120704 - IMU ***
					if (!confirm('Si vuole aggiornare la posizione anagrafica di Anater rispetto a quella del verticale ICI/IMU?'))
						{					
							document.getElementById('txtUpdateAnagraficaValue').value="false";
						}
						else
						{	
							document.getElementById('txtUpdateAnagraficaValue').value="true";
						}
						//alert(document.getElementById('btnRibaltaInAnater);
						document.getElementById('btnRibaltaInAnater').click(); 		
					}
				else if (iRetVal == 0){ // Anagrafe residente
					//*** 20120704 - IMU ***
					if (!confirm('Si vuole aggiornare la posizione anagrafica del verticale ICI/IMU rispetto a quella di Anater?'))
					{
						document.getElementById('txtUpdateAnagraficaValue').value="false";
					}else{
						document.getElementById('txtUpdateAnagraficaValue').value="true verticale";
					}
					document.getElementById('btnRibaltaInAnater').click();
				}else{
					document.getElementById('txtUpdateAnagraficaValue').value="false";
					
					document.getElementById('btnRibaltaInAnater').click(); 				
				}
			}
			
			function VerificaDataNumero(IsPassProg){
				var messaggio = '';
				var endmessaggio = '';
				var campimancanti = '';
				var messaggioObbli = '';
				var endmessaggioObbli = '';
				var campimancantiObbli = '';
				
				messaggio = 'Attenzione, i seguenti campi non sono stati compilati : ';
				endmessaggio = 'Si desidera procedere ugualmente col salvataggio della dichiarazione ?';
				messaggioObbli = 'Attenzione i campi';
				endmessaggioObbli = 'sono obbligatori.';
				
				if (document.getElementById('txtAnnoDich').value == ''){
					campimancantiObbli += '\n- Anno Dichiarazione';
				}
				if (document.getElementById('hdIdContribuente').value == '' || document.getElementById('hdIdContribuente').value == '-1') {
					campimancantiObbli += '\n- Dati Contribuente';
				}
				if (IsPassProg == '1' && document.getElementById('TxtDataPassProp').value == '') {
				    if (!confirm('Si vuole eseguire la duplicazione della dichiarazione?'))
				        return false;
				}
			
				if (document.getElementById('txtDataProt').value == ''){
					campimancanti += '\n- Data Protocollo.';
				}
				
				if (document.getElementById('txtNumDich').value == ''){
					campimancanti += '\n- Numero Dichiarazione.';
				}
				
				if (campimancantiObbli != ''){
				    GestAlert('a', 'warning', '', '', messaggioObbli + campimancantiObbli + '\n' + endmessaggioObbli);
					return false;
				}
				else{
					return true;
				}
				
				if (campimancanti != ''){
				    if (confirm(messaggio + campimancanti + '\n' +endmessaggio)) {
						return true;
					}else
					{
						return false;
					}
				}else{
					return true;
				}
			}
			
			function PulContribuente(){
				if (confirm('Si desidera eliminare il Contribuente?')){
				    document.getElementById('hdIdContribuente').value = '';
					document.getElementById('txtCodContribuenteCon1').value = '';
					document.getElementById('txtCodFiscaleContr').value = '';
					document.getElementById('txtPIVAContr').value = '';
					document.getElementById('txtCognomeContr').value = '';
					document.getElementById('txtNomeContr').value = '';
					document.getElementById('txtDataNascContr').value = '';
					document.getElementById('txtComNascContr').value = '';
					document.getElementById('txtProvNascContr').value = '';
					document.getElementById('txtViaResContr').value = '';
					document.getElementById('txtNumCivResContr').value = '';
					document.getElementById('txtEsponenteCivico').value = '';
					document.getElementById('txtIntResContr').value = '';
					document.getElementById('txtScalaResContr').value = '';
					document.getElementById('txtCapResContr').value = '';
					document.getElementById('txtComuneResContr').value = '';
					document.getElementById('txtProvResContr').value = '';
					document.getElementById('rdbMaschioContr').checked = false;
					document.getElementById('rdbFemminaContr').checked = false;
					document.getElementById('rdbGiuridicaContr').checked = false;
				}
				return false;
			}
			
			function PulDenunciante(){
				if (confirm('Si desidera eliminare il Denunciante?')){
					// elimino la spunta dal check box se esiste
					document.getElementById('chkEsisteDeninciante').checked = false;
					document.getElementById('txtCodContribuenteDen').value='';
					document.getElementById('txtCodFiscaleDen').value='';
					document.getElementById('txtPIVADen').value='';
					document.getElementById('txtCognomeDen').value='';
					document.getElementById('txtNomeDen').value='';
					document.getElementById('txtProfessione').value='';
					document.getElementById('txtDataNascDen').value='';
					document.getElementById('txtComNascDen').value='';
					document.getElementById('txtProvNascDen').value='';
					document.getElementById('txtViaResDen').value='';
					document.getElementById('txtNumCivResDen').value='';
					document.getElementById('txtEsponenteCivicoDen').value='';
					document.getElementById('txtIntResDen').value='';
					document.getElementById('txtScalaResDen').value='';
					document.getElementById('txtCapResDen').value='';
					document.getElementById('txtComuneResDen').value='';
					document.getElementById('txtProvResDen').value='';
					document.getElementById('rdbMaschioDen').checked = false;
					document.getElementById('rdbFemminaDen').checked = false;
					document.getElementById('rdbGiuridicaDen').checked = false;
				}
				return false;
			}

			function Salva() {
			    if (document.getElementById('hdIsPassProg').value == '1' && document.getElementById('TxtDataPassProp').value == '') {
			       GestAlert('a', 'warning', '', '', 'Inserire la data di passaggio di proprieta\'!');
			    }
			    else {
			        document.getElementById('btnSalva').click();
			    }
			}
			function VisNascDenunciante(){
				if ((document.getElementById('divDenunciante').style.display)==''){
					document.getElementById('divDenunciante').style.display='none';
					document.getElementById('linkDenunciante').innerText='Visualizza Dati Denunciante >>';
				}else{
					document.getElementById('divDenunciante').style.display='';
					document.getElementById('linkDenunciante').innerText='<< Nascondi Dati Denunciante';
				}
			}
		</script>
	</HEAD>
	<body class="Sfondo" scroll="yes" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="tblCorpo" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<!--*** 20131003 - gestione atti compravendita ***-->
				<tr>
					<td>
						<div id="AttoCompraVendita" class="FiledSetRicerca" style="DISPLAY:none" runat="server">
							<fieldset>
								<legend class="Legend">
									Dati nota trascrizione</legend><br />
								&nbsp;<asp:Label ID="lblNotaTrascrizione" runat="server" Text="" CssClass="Input_Label"></asp:Label>
								<br />
								&nbsp;
							</fieldset>
							<fieldset>
								<legend class="Legend">
									Dati Immobile in nota</legend>
								<p>&nbsp;<asp:Label ID="lblRifNota" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
								<p>&nbsp;<asp:Label ID="lblCatNota" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
								<p>&nbsp;<asp:Label ID="lblUbicazioneNota" runat="server" Text="" CssClass="Input_Label"></asp:Label>
								&nbsp;<asp:Label ID="lblUbicazioneCatasto" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
								&nbsp;
							</fieldset>
							<fieldset>
								<legend class="Legend">
									Dati Soggetto in nota</legend><br />
								&nbsp;<asp:Label ID="lblSoggettoNota" runat="server" Text="" CssClass="Input_Label"></asp:Label>
								<br />
								&nbsp;
							</fieldset>
						</div>
					</td>
				</tr>
				<!--*** ***-->
				<tr>
					<td>&nbsp;<asp:label id="lblMesImmobile" runat="server" ForeColor="Red" Visible="False">Ora è possibile inserire i dati dell'immobile.</asp:label></td>
				</tr>
				<tr>
					<td>&nbsp;
						<asp:label id="lblBonificata" runat="server" ForeColor="Red" Visible="False" CssClass="hidden">La dichiarazione non è bonificata.</asp:label></td>
					<!--<td><a href="javascript:hidda();">&raquo;</a></td>--></tr>
				<tr>
					<td vAlign="top" height="400">
						<table id="Table3" cellSpacing="5" cellPadding="1" width="100%" border="0">
							<tbody>
								<tr>
									<td colSpan="4"><asp:label id="lblDatiTestata" runat="server" CssClass="lstTabRow" Width="100%">Dati Testata</asp:label></td>
								</tr>
								<tr>
									<td colSpan="4">
										<table width="90%">
											<tr>
												<td class="Input_Label">
												    <asp:label id="lblAnnoDichiarazione" runat="server" CssClass="Input_Label" Enabled="False">Anno Dichiaraz. </asp:label><asp:label id="lblContrAnno" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" runat="server" Enabled="False">*</asp:label><br />
													<asp:textbox id="txtAnnoDich" tabIndex="1" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="90px" Enabled="False" MaxLength="4" Text=""></asp:textbox>
													<asp:requiredfieldvalidator id="rvalAnnoDich" Runat="server" Display="None" ControlToValidate="txtAnnoDich" ErrorMessage="Campo Anno Dichiarazione obbligatorio.">***</asp:requiredfieldvalidator>
												</td>
												<td class="Input_Label">
												    <asp:label id="lblNumProtocollo" runat="server" CssClass="Input_Label" Enabled="False">Num. Protocollo</asp:label><br />
													<asp:textbox id="txtNumProt" style="TEXT-ALIGN: right" tabIndex="2" runat="server" CssClass="Input_Text" Width="90px" Enabled="False" MaxLength="9" Text=""></asp:textbox>
												</td>
												<td class="Input_Label">
												    <asp:label id="lblDataProtocollo" runat="server" CssClass="Input_Label" Enabled="False">Data Protocollo</asp:label><br />
													<asp:textbox id="txtDataProt" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlur(this);" tabIndex="3" runat="server" CssClass="Input_Text_Right TextDate" Enabled="False" MaxLength="8" Text=""></asp:textbox>
												</td>
												<td class="Input_Label">
												    <asp:label id="lblNumDichiaraz" runat="server" CssClass="Input_Label" Enabled="False">Num. Dichiaraz.</asp:label><br />
													<asp:textbox id="txtNumDich" style="TEXT-ALIGN: right" tabIndex="4" runat="server" CssClass="Input_Text" Width="90px" Enabled="False" MaxLength="4"></asp:textbox>
												</td>
												<td class="Input_Label">
												    <asp:label id="lblDataDichiaraz" runat="server" CssClass="Input_Label" Enabled="False">Data Dichiaraz.</asp:label><br />
													<asp:textbox id="txtDataDich" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlur(this);" tabIndex="5" runat="server" CssClass="Input_Text_Right TextDate" Enabled="False" MaxLength="8" Text=""></asp:textbox>
													<asp:comparevalidator id="CompareValidator1" runat="server" Display="Dynamic" ControlToValidate="txtNumDich" ErrorMessage="Numero Dichiarazione deve essere numero." Operator="DataTypeCheck" Type="Integer">***</asp:comparevalidator>
												</td>
												<td class="Input_Label">
												    <asp:label id="lblTotModelli" runat="server" CssClass="Input_Label" Enabled="False">Tot. Modelli</asp:label><br />
													<asp:textbox id="txtTotModelli" style="TEXT-ALIGN: right" tabIndex="6" runat="server" CssClass="Input_Text" Width="90px" Enabled="False" MaxLength="5" Text=""></asp:textbox>
												</td>
												<!--*** 20141110 - passaggio di proprietà ***-->
												<td>
												    <asp:Label runat="server" CssClass="Input_Label">Data Passaggio Proprietà</asp:Label><br />
												    <asp:TextBox ID="TxtDataPassProp" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlur(this);" Enabled="False"></asp:TextBox>
												</td>
											</tr>
										</table>
									</td>
								</tr>
				                <!--blocco dati contribuente--><%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
				                <tr id="TRPlainAnag">
				                    <td colspan="4">
				                        <iframe id="ifrmAnag" runat="server" src="../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				                        <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				                    </td>
				                </tr>
				                <tr id="TRSpecAnag">
				                    <td colspan="4">
										<table cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<td class="Input_Label" width="170">
												    <asp:label id="lblDatiContribuente" runat="server" CssClass="lstTabRow" Font-Bold="True">Dati Contribuente</asp:label>
												    <asp:label id="lblContrContrib" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 12px" runat="server" Enabled="False">*</asp:label>&nbsp;
													<asp:imagebutton id="lnkVerificaContribuente" tabIndex="7" runat="server" ImageUrl="images/Bottoni/Listasel.png" ToolTip="Verifica in Anagrafica" CausesValidation="False" imagealign="Bottom" onclick="lnkVerificaContribuente_Click"></asp:imagebutton>&nbsp;
													<asp:imagebutton id="lnkPulisciContr" tabIndex="7" runat="server" Enabled="false" ImageUrl="images\cancel.png" Width="10px" Height="10px" ToolTip="Pulisci i campi Contribuente" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>
													<asp:requiredfieldvalidator id="rvalCodContrib" runat="server" Display="None" ControlToValidate="txtCodContribuenteCon1" ErrorMessage="Dati Contribuente obbligatori.">***</asp:requiredfieldvalidator>
													<asp:textbox id="txtIdDataAnagcon" style="DISPLAY: none" runat="server"></asp:textbox>
													<asp:textbox id="txtTypeOperation" tabIndex="28" runat="server" Visible="False" Text="" ReadOnly="True"></asp:textbox>
													<asp:textbox id="txtUpdateAnagraficaValue" style="DISPLAY: none" tabIndex="28" runat="server" Width="45px" Text="" ReadOnly="True"></asp:textbox>
													<asp:textbox id="txtCodContribuenteCon1" style="DISPLAY: none" tabIndex="28" runat="server" Width="45px" Text=""></asp:textbox>
												</td>
												<td><asp:label id="lblRiemp" CssClass="lstTabRow" Width="100%" Runat="server">&nbsp;</asp:label></td>
											</tr>
											<tr>
												<td class="Input_Label" style="WIDTH: 210px">
												    <asp:label id="lblCodiceFiscaleContr" runat="server" CssClass="Input_Label" Enabled="False">Codice Fiscale</asp:label><br />
													<asp:textbox id="txtCodFiscaleContr" style="TEXT-ALIGN: left" runat="server" CssClass="Input_Text" Width="185px" MaxLength="16" Text="" ReadOnly="True"></asp:textbox>
												</td>
												<td class="Input_Label" style="WIDTH: 160px">
												    <asp:label id="lblPartitaIVAContr" runat="server" CssClass="Input_Label" Enabled="False">Partita IVA</asp:label><br />
													<asp:textbox id="txtPIVAContr" runat="server" CssClass="Input_Text" Width="140px" MaxLength="11" Text="" ReadOnly="True"></asp:textbox>
												</td>
												<td>&nbsp;
													<asp:label id="lblSessoContr" runat="server" CssClass="Input_Label" Enabled="False">Sesso</asp:label><br />
													<asp:radiobutton id="rdbMaschioContr" runat="server" CssClass="Input_Label" Enabled="False" Text="M"></asp:radiobutton>
													<asp:radiobutton id="rdbFemminaContr" runat="server" CssClass="Input_Label" Enabled="False" Text="F"></asp:radiobutton>
													<asp:radiobutton id="rdbGiuridicaContr" runat="server" CssClass="Input_Label" Enabled="False" Text="G"></asp:radiobutton>
												</td>
											</tr>
											<tr>
												<td class="Input_Label" style="WIDTH: 250px">
												    <asp:label id="lblCognomeContr" runat="server" CssClass="Input_Label" Enabled="False">Cognome/Rag.Soc</asp:label><br />
													<asp:textbox id="txtCognomeContr" style="TEXT-ALIGN: left" runat="server" CssClass="Input_Text" Width="230px" MaxLength="70" Text="" ReadOnly="True"></asp:textbox>
												</td>
												<td class="Input_Label" style="WIDTH: 250px" colSpan="2">
												    <asp:label id="lblNomeContr" runat="server" CssClass="Input_Label" Enabled="False">Nome</asp:label><br />
													<asp:textbox id="txtNomeContr" runat="server" CssClass="Input_Text" Width="230px" MaxLength="30" Text="" ReadOnly="True"></asp:textbox>
												</td>
											</tr>
											<tr>
												<td class="Input_Label" style="WIDTH: 100px">
												    <asp:label id="lblDataNascitaContr" runat="server" CssClass="Input_Label" Enabled="False">Data Nascita</asp:label><br />
													<asp:textbox id="txtDataNascContr" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10" ReadOnly="True"></asp:textbox>
												</td>
												<td style="WIDTH: 250px">
												    <asp:label id="lblComuneNascitaContr" runat="server" CssClass="Input_Label" Enabled="False">Comune Nascita</asp:label><br />
													<asp:textbox id="txtComNascContr" runat="server" CssClass="Input_Text" Width="230px" MaxLength="30" Text="" ReadOnly="True"></asp:textbox>
												</td>
												<td class="Input_Label" colSpan="2">
												    <asp:label id="lblProvContr" runat="server" CssClass="Input_Label" Enabled="False">Prov.</asp:label><br />
													<asp:textbox id="txtProvNascContr" runat="server" CssClass="Input_Text" Width="30px" MaxLength="2" Text="" ReadOnly="True"></asp:textbox>
												</td>
											</tr>
											<tr>
												<td style="WIDTH: 250px"><asp:label id="lblViaContr" runat="server" CssClass="Input_Label" Enabled="False">Via Residenza</asp:label><br />
													<asp:textbox id="txtViaResContr" runat="server" CssClass="Input_Text" Width="230px" MaxLength="60"
														Text="" ReadOnly="True"></asp:textbox></td>
												<td style="WIDTH: 85px">&nbsp;<asp:label id="lblNumeroCivContr" runat="server" CssClass="Input_Label" Enabled="False">Num. Civico</asp:label><br />
													&nbsp;<asp:textbox id="txtNumCivResContr" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text"
														Width="65px" ReadOnly="True"></asp:textbox>
												</td>
												<td style="WIDTH: 85px">&nbsp;<asp:label id="lblEsponenteCivico" runat="server" CssClass="Input_Label" Enabled="False">Esp. Civico</asp:label><br />
													&nbsp;<asp:textbox id="txtEsponenteCivico" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text"
														Width="65px" MaxLength="5" Text="" ReadOnly="True"></asp:textbox>
												</td>
												<td style="WIDTH: 85px">&nbsp;<asp:label id="lblIntContr" runat="server" CssClass="Input_Label" Enabled="False">Interno</asp:label><br />
													&nbsp;<asp:textbox id="txtIntResContr" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text"
														Width="65px" MaxLength="5" Text="" ReadOnly="True"></asp:textbox>
												</td>
												<td style="WIDTH: 85px">&nbsp;<asp:label id="lblScalaContr" runat="server" CssClass="Input_Label" Enabled="False">Scala</asp:label><br />
													&nbsp;<asp:textbox id="txtScalaResContr" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text"
														Width="65px" MaxLength="3" Text="" ReadOnly="True"></asp:textbox>
												</td>
											</tr>
											<tr>
												<td style="WIDTH: 253px" width="253"><asp:label id="lblComuneResidenzaContr" runat="server" CssClass="Input_Label" Enabled="False">Comune Residenza</asp:label><br />
													<asp:textbox id="txtComuneResContr" runat="server" CssClass="Input_Text" Width="230px" MaxLength="30"
														Text="" ReadOnly="True"></asp:textbox><br />
												</td>
												<td class="Input_Label" style="WIDTH: 85px"><asp:label id="lblCAPContr" runat="server" CssClass="Input_Label" Enabled="False">CAP</asp:label><br />
													<asp:textbox id="txtCapResContr" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text"
														Width="65px" MaxLength="5" Text="" ReadOnly="True"></asp:textbox></td>
												<td class="Input_Label"><asp:label id="lblProvinciaContr" runat="server" CssClass="Input_Label" Enabled="False">Prov.</asp:label><br />
													<asp:textbox id="txtProvResContr" runat="server" CssClass="Input_Text" Width="30px" MaxLength="2"
														Text="" ReadOnly="True"></asp:textbox></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td colSpan="4">
										<p id="pRicerca" style="MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px">
											<A class="linkDenunciante" id="linkDenunciante" style="FONT-SIZE: 11px" onclick="VisNascDenunciante();"
												href="#"><b>Visualizza Dati Denunciante &gt;&gt;</b></A>
										</p>
										<br />
										<div id="divDenunciante" style="DISPLAY:none">
											<table cellSpacing="0" cellPadding="0" width="100%" border="0">
												<tr>
													<td>
														<table cellSpacing="0" cellPadding="0" width="100%" border="0">
															<tr>
																<td width="300"><asp:label id="lblDenunciante" runat="server" CssClass="lstTabRow" Font-Bold="True">Dati Denunciante</asp:label>&nbsp;
																	<asp:imagebutton id="lnkVerificaDenunciante" tabIndex="9" runat="server" ImageUrl="images/Bottoni/Listasel.png"
																		ToolTip="Verifica in Anagrafica" CausesValidation="False" onclick="lnkVerificaDenunciante_Click"></asp:imagebutton>&nbsp;
																	<asp:imagebutton id="lnkPulisciDen" tabIndex="10" runat="server" Enabled="false" ImageUrl="images/cancel.png" Width="10px" Height="10px"
																		ToolTip="Verifica in Anagrafica" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>
																	<asp:checkbox id="chkEsisteDeninciante" tabIndex="8" runat="server" Enabled="False" CssClass="Input_Label"
																		Text="Denunciante presente" AutoPostBack="True" oncheckedchanged="chkEsisteDeninciante_CheckedChanged"></asp:checkbox><input id="txtCodContribuenteDen" type="hidden" name="txtCodContribuenteDen" runat="server">
																	<asp:textbox id="txtIdDataAnagDen" style="DISPLAY: none" runat="server"></asp:textbox></td>
																<td><asp:label id="Label2" CssClass="lstTabRow" Width="100%" Runat="server">&nbsp;</asp:label></td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td colSpan="4">
														<table cellSpacing="0" cellPadding="0" border="0">
															<tr>
																<td class="Input_Label" style="WIDTH: 210px"><asp:label id="lblCodiceFiscaleDen" runat="server" CssClass="Input_Label">Codice Fiscale</asp:label><br />
																	<asp:textbox id="txtCodFiscaleDen" runat="server" CssClass="Input_Text" Width="185px" MaxLength="16"
																		Text="" ReadOnly="True"></asp:textbox></td>
																<td style="WIDTH: 160px"><asp:label id="lblPartitaIVADen" runat="server" CssClass="Input_Label">Partita IVA</asp:label><br />
																	<asp:textbox id="txtPIVADen" runat="server" CssClass="Input_Text" Width="140px" MaxLength="11"
																		Text="" ReadOnly="True"></asp:textbox></td>
																<td class="Input_Label"><asp:label id="lblSessoDen" runat="server" CssClass="Input_Label">Sesso</asp:label><br />
																	<asp:radiobutton id="rdbMaschioDen" runat="server" CssClass="Input_Label" Enabled="False" Text="M"></asp:radiobutton><asp:radiobutton id="rdbFemminaDen" runat="server" CssClass="Input_Label" Enabled="False" Text="F"></asp:radiobutton><asp:radiobutton id="rdbGiuridicaDen" runat="server" CssClass="Input_Label" Enabled="False" Text="G"></asp:radiobutton></td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td colSpan="4">
														<table cellSpacing="0" cellPadding="0" border="0">
															<tr>
																<td class="Input_Label" style="WIDTH: 250px"><asp:label id="lblCognomeDen" runat="server" CssClass="Input_Label">Cognome/Rag.Soc</asp:label><br />
																	<asp:textbox id="txtCognomeDen" runat="server" CssClass="Input_Text" Width="230px" MaxLength="70"
																		Text="" ReadOnly="True"></asp:textbox></td>
																<td style="WIDTH: 250px"><asp:label id="lblNomeDen" runat="server" CssClass="Input_Label">Nome</asp:label><br />
																	<asp:textbox id="txtNomeDen" runat="server" CssClass="Input_Text" Width="230px" MaxLength="30"
																		Text="" ReadOnly="True"></asp:textbox></td>
																<td class="Input_Label"><asp:label id="lblProfessione" runat="server" CssClass="Input_Label">Professione</asp:label><br />
																	<asp:textbox id="txtProfessione" runat="server" CssClass="Input_Text" Width="230px" MaxLength="10"
																		Text="" ReadOnly="True"></asp:textbox></td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td colSpan="4">
														<table cellSpacing="0" cellPadding="0">
															<tr>
																<td class="Input_Label" style="WIDTH: 100px"><asp:label id="lblDataNascitaDen" runat="server" CssClass="Input_Label">Data Nascita</asp:label><br />
																	<asp:textbox id="txtDataNascDen" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10" Text="" ReadOnly="True"></asp:textbox></td>
																<td class="Input_Label" style="WIDTH: 250px"><asp:label id="lblComuneNascitaDen" runat="server" CssClass="Input_Label">Comune Nascita</asp:label><br />
																	<asp:textbox id="txtComNascDen" runat="server" CssClass="Input_Text" Width="230px" MaxLength="30"
																		Text="" ReadOnly="True"></asp:textbox></td>
																<td><asp:label id="lblProvDen" runat="server" CssClass="Input_Label">Prov.</asp:label><br />
																	<asp:textbox id="txtProvNascDen" runat="server" CssClass="Input_Text" Width="30px" MaxLength="2"
																		Text="" ReadOnly="True"></asp:textbox></td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td colSpan="4">
														<table cellSpacing="0" cellPadding="0">
															<tr>
																<td class="Input_Label" style="WIDTH: 250px"><asp:label id="lblViaDen" runat="server" CssClass="Input_Label">Via Residenza</asp:label><br />
																	<asp:textbox id="txtViaResDen" runat="server" CssClass="Input_Text" Width="230px" MaxLength="60"
																		Text="" ReadOnly="True"></asp:textbox></td>
																<td style="WIDTH: 85px"><asp:label id="lblNumeroCivDen" runat="server" CssClass="Input_Label">Num. Civico</asp:label><br />
																	<asp:textbox id="txtNumCivResDen" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text"
																		Width="65px" Text="" ReadOnly="True"></asp:textbox></td>
																<td class="Input_Label" style="WIDTH: 85px"><asp:label id="lblEsponenteCivicoDen" runat="server" CssClass="Input_Label">Esp. Civico</asp:label><br />
																	<asp:textbox id="txtEsponenteCivicoDen" runat="server" CssClass="Input_Text" Width="65px" Text=""
																		ReadOnly="True"></asp:textbox><br />
																</td>
																<td style="WIDTH: 85px"><asp:label id="lblIntDen" runat="server" CssClass="Input_Label">Interno</asp:label><br />
																	<asp:textbox id="txtIntResDen" runat="server" CssClass="Input_Text" Width="65px" MaxLength="5"
																		Text="" ReadOnly="True"></asp:textbox></td>
																<td style="WIDTH: 85px"><asp:label id="lblScalaDen" runat="server" CssClass="Input_Label">Scala</asp:label><br />
																	<asp:textbox id="txtScalaResDen" runat="server" CssClass="Input_Text" Width="65px" MaxLength="3"
																		Text="" ReadOnly="True"></asp:textbox></td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td colSpan="4">
														<table cellSpacing="0" cellPadding="0" border="0">
															<tr>
																<td class="Input_Label" style="WIDTH: 250px"><asp:label id="lblComuneResidenzaDen" runat="server" CssClass="Input_Label">Comune Residenza</asp:label><br />
																	<asp:textbox id="txtComuneResDen" runat="server" CssClass="Input_Text" Width="230px" MaxLength="30"
																		Text="" ReadOnly="True"></asp:textbox></td>
																<td style="WIDTH: 85px"><asp:label id="lblCAPDen" runat="server" CssClass="Input_Label">CAP</asp:label><br />
																	<asp:textbox id="txtCapResDen" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text"
																		Width="65px" MaxLength="5" Text="" ReadOnly="True"></asp:textbox></td>
																<td class="Input_Label"><asp:label id="lblProvinciaDen" runat="server" CssClass="Input_Label">Prov.</asp:label><br />
																	<asp:textbox id="txtProvResDen" runat="server" CssClass="Input_Text" Width="30px" MaxLength="2"
																		Text="" ReadOnly="True"></asp:textbox></td>
																<td>&nbsp;&nbsp;</td>
															</tr>
														</table>
													</td>
												</tr>
											</table>
										</div>
									</td>
								</tr>
								<tr>
									<td colSpan="4"><asp:label id="Label1" runat="server" CssClass="lstTabRow" Width="100%" Font-Bold="True">Dati Immobile</asp:label></td>
								</tr>
								<tr>
									<td colSpan="4">
                                        <iframe id="loadImmobile" name="loadImmobile" frameborder="0" width="100%" height="200px" runat="server" src="../aspvuota.aspx"></iframe>
									</td>
								</tr>
								<tr>
									<td colSpan="4"><asp:label id="lblDatiProvenienza" runat="server" CssClass="lstTabRow" Width="100%" Font-Bold="True">Dati Provenienza</asp:label></td>
								</tr>
								<tr>
									<td class="Input_Label"><asp:label id="lblProvenienza" runat="server" Enabled="False">Provenienza</asp:label><br />
										<asp:dropdownlist id=ddlProvenienze tabIndex=11 runat="server" CssClass="Input_Text" Enabled="False" DataValueField="Codice" DataTextField="Descrizione" DataSource="<%# ListProvenienze() %>">
										</asp:dropdownlist></td>
									<td style="WIDTH: 245px"></td>
									<td class="Input_Label" style="WIDTH: 128px"></td>
									<td><input id="txtNameObject" type="hidden" name="txtNameObject" runat="server"></td>
								</tr>
								<tr>
									<td colSpan="4"><asp:button id="btnImmobili" style="DISPLAY: none" runat="server" Enabled="<%# IDTestata != 0 ? true : false %>" Text="Button" CausesValidation="False" onclick="btnImmobili_Click"></asp:button></td>
								</tr>
							</tbody>
						</table>
					</td>
					<td vAlign="top" height="400"></td>
				</tr>
				<tr>
					<td height="25">
					    <asp:button id="btnElimina" style="DISPLAY: none" runat="server" Enabled="<%# IDTestata != 0 ? true : false %>" Text="Elimina" CausesValidation="False" onclick="btnElimina_Click"></asp:button>
					    <asp:button id="btnRibaltaInAnater" style="DISPLAY: none" runat="server" Text="RibaltaInAnater" onclick="btnRibaltaInAnater_Click"></asp:button>
					    <asp:button id="btnDatiAggiuntivi" style="DISPLAY: none" runat="server" Text="DatiAggiuntivi" onclick="btnDatiAggiuntivi_Click"></asp:button>
					    <asp:button id="btnSalva" style="DISPLAY: none" runat="server" Enabled="" Text="Salva" ToolTip="Permette di salvare i dati della pagina per la modifica o l'inserimento dell'entità gestita" onclick="btnSalva_Click"></asp:button>
					    <asp:button id="btnAbilita" style="DISPLAY: none" runat="server" Text="Abilita" CausesValidation="False" onclick="btnAbilita_Click"></asp:button>
					    <asp:button id="btnRibalta" style="DISPLAY: none" runat="server" Text="Button" CausesValidation="False" onclick="btnRibalta_Click"></asp:button>
					    <asp:button id="btnBonifica" style="DISPLAY: none" tabIndex="10" runat="server" Text="Bonifica" CausesValidation="False"></asp:button>
					    <asp:button id="btnIndietro" style="DISPLAY: none" runat="server" Text="Indietro" CausesValidation="False" onclick="btnIndietro_Click"></asp:button>
                        <asp:button id="cmdPrecarica" style="DISPLAY:none" runat="server" Text="Precarica Immobile" Enabled="true" onclick="cmdPrecarica_Click"></asp:button>
					    <asp:linkbutton id="lbtnUpdate" runat="server" Visible="False" onclick="lbtnUpdate_Click"></asp:linkbutton>
					    <asp:textbox id="txtIDquestionario" style="DISPLAY: none" runat="server" CssClass="Input_Text" Width="230px" MaxLength="30" Text=""></asp:textbox>
					    <asp:validationsummary id="valErrorSummary" runat="server" ShowSummary="False" ShowMessageBox="True"></asp:validationsummary>
					</td>
					<td height="25"></td>
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
			<!--*** 20141110 - passaggio di proprietà ***-->
			<asp:HiddenField id="hdIsPassProg" runat="server" value="0" />
		</form>
	</body>
</HTML>
