<%@ Page language="c#" Codebehind="Versamenti.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.Versamenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Versamenti</title>
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
		<script type="text/javascript" src="../_js/ControlloData.js?newversion"></script>
		<script type="text/javascript" src="../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">
		    function PulisciContr() {
		        if (confirm('Vuoi eliminare i dati del Contribuente?')) {
		            document.getElementById('hdIdContribuente').value = '';
		            document.getElementById('txtCodFiscale').value = '';
		            document.getElementById('txtPIVA').value = '';
		            document.getElementById('txtDataNasc').value = '';
		            document.getElementById('txtComNasc').value = '';
		            document.getElementById('txtProvNasc').value = '';
		            document.getElementById('txtCognome').value = '';
		            document.getElementById('txtNome').value = '';
		            document.getElementById('txtViaRes').value = '';
		            document.getElementById('txtNumCivicoRes').value = '';
		            document.getElementById('txtEsponenteCivico').value = '';
		            document.getElementById('txtScalaRes').value = '';
		            document.getElementById('txtIntRes').value = '';
		            document.getElementById('txtComuneRes').value = '';
		            document.getElementById('txtCapRes').value = '';
		            document.getElementById('txtProvRes').value = '';
		        }
		        return false;
		    }

		    function ControllaCampi() {
		        var idContribuente = document.getElementById('hdIdContribuente').value;
		        var Annoriferimento = document.getElementById('txtAnnoRiferimento').value;
		        var NumFabbr = document.getElementById('txtNumFabbricatiPosseduti').value;
		        var DataPagamento = document.getElementById('txtDataPagamento').value;
		        var Acconto = document.getElementById('chkAcconto').checked;
		        var Saldo = document.getElementById('chkSaldo').checked;
		        var Violazione = document.getElementById('chkViolazione').checked;
		        var CampiObbligatori = '';

		        msg = 'NumFabbr = ' + NumFabbr;
		        msg += '\nAnnoRiferimento = ' + Annoriferimento;
		        msg += '\nDataPagamento = ' + DataPagamento;
		        msg += '\nAcconto = ' + Acconto;
		        msg += '\nSaldo = ' + Saldo;

		        if (idContribuente == '') {
		            CampiObbligatori += '\n- Dati Contribuente';
		        }
		        if (Annoriferimento == '') {
		            CampiObbligatori += '\n- Anno Riferimento';
		        }
		        if (DataPagamento == '') {
		            CampiObbligatori += '\n- Data Pagamento';
		        }
		        if (Violazione == false) {
		            if (NumFabbr == '') {
		                CampiObbligatori += '\n- Numero Fabbricati';
		            }
		            if ((Acconto == false) && (Saldo == false)) {
		                CampiObbligatori += '\n- Modo di Pagamento';
		            }
		        }
		        //*** 20140630 - TASI ***
		        if (document.getElementById('txtImportoPagamento').value == '' || document.getElementById('txtImportoPagamento').value == '0')
		            CampiObbligatori += '\n- Importo';
		        if (document.getElementById('optICI').Checked == false && document.getElementById('optTASI').Checked == false)
		            CampiObbligatori += '\n- Tributo';
		        //*** ***
		        if (CampiObbligatori != '') {
		            GestAlert('a', 'warning', '', '', 'Attenzione, i seguenti campi sono obbligatori :' + CampiObbligatori);
		            return false;
		        }
		        document.getElementById('btnSalva').click();
		    }
		</script>
	</HEAD>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="tblCorpo" cellSpacing="1" cellPadding="1" border="0" width="90%">
				<tr>
					<td height="23">&nbsp;
						<asp:label id="lblBonificato" runat="server" ForeColor="Red" Visible="False" CssClass="hidden">Il versamento non è bonificato.</asp:label></td>
					<td height="23"></td>
				</tr>
				<!--blocco dati contribuente--><%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
				<tr>
				    <td colSpan="6">
				        <iframe id="ifrmAnag" runat="server" src="../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				        <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				    </td>
				</tr>
				<tr id="TRSpecAnag">
				    <td colSpan="6">
				        <table width="100%">
					        <tr>
						        <td>
						            <asp:label id="lblDatiContribuente" runat="server" CssClass="lstTabRow" Font-Bold="True">Dati Contribuente</asp:label>&nbsp;
						            <asp:button id="Button1" style="DISPLAY: none" runat="server" CausesValidation="False" Text="Button" onclick="btnRibalta_Click"></asp:button>
						            <asp:imagebutton id="lnkVerificaContribuente" runat="server" CausesValidation="False" ImageUrl="../images/Bottoni/Listasel.png" ToolTip="Verifica in Anagrafica" onclick="lnkVerificaContribuente_Click"></asp:imagebutton>
						            <asp:imagebutton id="lnkPulisciContr" runat="server" ImageUrl="images\cancel.png" ToolTip="Pulisci i campi Contribuente" CausesValidation="False" imagealign="Bottom" />
						            <input id="txtIdDataAnagrafica" type="hidden" name="txtIdDataAnagrafica" runat="server" />
						        </td>
					            <td align="left" colspan="5">
					                <asp:label ID="lblRiemp" CssClass="lstTabRow" Runat="server" Width="100%">&nbsp;</asp:label>
					            </td>
					        </tr>
				            <tr>
					            <td>
						            <asp:label id="lblCodiceFiscale" runat="server" CssClass="Input_Label">Codice Fiscale</asp:label><br />
						            <asp:textbox id="txtCodFiscale" runat="server" CssClass="Input_Text" Width="185px" Text="" MaxLength="16"></asp:textbox>
					            </td>
					            <td>
						            <asp:label id="lblPartitaIVA" runat="server" CssClass="Input_Label">Partita IVA</asp:label><br />
						            <asp:textbox id="txtPIVA" runat="server" CssClass="Input_Text" Width="140px" Text="" MaxLength="11"></asp:textbox>
					            </td>
					            <td>&nbsp;
						            <asp:label id="lblSesso" runat="server" CssClass="Input_Label">Sesso</asp:label><br />
						            <asp:radiobutton id="rdbMaschio" runat="server" CssClass="Input_Label" Text="M"></asp:radiobutton>&nbsp;
						            <asp:radiobutton id="rdbFemmina" runat="server" CssClass="Input_Label" Text="F"></asp:radiobutton>&nbsp;
						            <asp:radiobutton id="rdbGiuridica" runat="server" CssClass="Input_Label" Text="G"></asp:radiobutton>
					            </td>
					            <td>
						            <asp:label id="lblDataNascita" runat="server" CssClass="Input_Label">Data Nascita</asp:label><br />
						            <asp:textbox id="txtDataNasc" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10"></asp:textbox>
					            </td>
					            <td>
						            <asp:label id="lblComuneNascita" runat="server" CssClass="Input_Label">Comune Nascita</asp:label><br />
						            <asp:textbox id="txtComNasc" runat="server" CssClass="Input_Text" Width="230px" Text="" MaxLength="30"></asp:textbox>
					            </td>
					            <td>
						            <asp:label id="lblProv" runat="server" CssClass="Input_Label">Prov.</asp:label><br />
						            <asp:textbox id="txtProvNasc" runat="server" CssClass="Input_Text" Width="30px" Text="" MaxLength="2"></asp:textbox>
					            </td>
				            </tr>
				            <tr>
					            <td colspan="3">
						            <asp:label id="lblCognome" runat="server" CssClass="Input_Label">Cognome/Rag.Soc</asp:label><br />
						            <asp:textbox id="txtCognome" runat="server" CssClass="Input_Text" Width="400px" Text="" MaxLength="70"></asp:textbox>
					            </td>
					            <td colspan="3">
						            <asp:label id="lblNome" runat="server" CssClass="Input_Label">Nome</asp:label><br />
						            <asp:textbox id="txtNome" runat="server" CssClass="Input_Text" Width="230px" Text="" MaxLength="30"></asp:textbox>
					            </td>
				            </tr>
				            <tr>
					            <td colspan="2">
						            <asp:label id="lblVia" runat="server" CssClass="Input_Label">Via</asp:label><br />
						            <asp:textbox id="txtViaRes" runat="server" CssClass="Input_Text" Width="400px" Text="" MaxLength="60"></asp:textbox>
					            </td>
					            <td>
						            <asp:label id="lblNumeroCiv" runat="server" CssClass="Input_Label">Num. Civico</asp:label><br />
						            <asp:textbox id="txtNumCivRes" runat="server" CssClass="Input_Text" Width="65px"></asp:textbox>
					            </td>
					            <td>
						            <asp:label id="lblEsponenteCivico" runat="server" CssClass="Input_Label">Esp. Civico</asp:label><br />
						            <asp:textbox id="txtEsponenteCivico" runat="server" CssClass="Input_Text" Width="65px" Text="" MaxLength="5"></asp:textbox>
					            </td>
					            <td>
						            <asp:label id="lblScala" runat="server" CssClass="Input_Label">Scala</asp:label><br />
						            <asp:textbox id="txtScalaRes" runat="server" CssClass="Input_Text" Width="65px" Text="" MaxLength="3"></asp:textbox>
					            </td>
					            <td>
						            <asp:label id="lblInt" runat="server" CssClass="Input_Label">Interno</asp:label><br />
						            <asp:textbox id="txtIntRes" runat="server" CssClass="Input_Text" Width="65px" Text="" MaxLength="5"></asp:textbox>
					            </td>
				            </tr>
				            <tr>
					            <td colspan="2">
						            <asp:label id="lblComuneResidenza" runat="server" CssClass="Input_Label">Comune Residenza</asp:label><br />
						            <asp:textbox id="txtComuneRes" runat="server" CssClass="Input_Text" Width="400px" Text="" MaxLength="30"></asp:textbox>
					            </td>
					            <td>
						            <asp:label id="lblCAP" runat="server" CssClass="Input_Label">CAP</asp:label><br />
						            <asp:textbox id="txtCapRes" runat="server" CssClass="Input_Text" Width="65px" Text="" MaxLength="5"></asp:textbox>
					            </td>
					            <td>
						            <asp:label id="lblProvincia" runat="server" CssClass="Input_Label">Prov.</asp:label><br />
						            <asp:textbox id="txtProvRes" runat="server" CssClass="Input_Text" Width="30px" Text="" MaxLength="2"></asp:textbox>
					            </td>
				            </tr>
				        </table>
				    </td>
				</tr>
				<tr>
					<td colSpan="6"><asp:label id="lblDatiVers" Runat="server" CssClass="lstTabRow" Width="100%">Dati Versamento</asp:label>
						<!--<strong class="lstTabRow">Dati Versamento</strong>--></td>
				</tr>
				<tr>
					<td>
						<asp:label id="lblAnnoRiferimento" runat="server" CssClass="Input_Label" Width="113px">Anno di Riferimento</asp:label>
						<asp:label id="lblContrAnno" style="FONT-FAMILY: Tahoma, Arial, Verdana; COLOR: red; FONT-SIZE: 12px"
							Runat="server">*</asp:label>
						<br />
						<asp:textbox id="txtAnnoRiferimento" tabIndex="1" runat="server" CssClass="Input_Text_Right OnlyNumber" Enabled="False" Text="" MaxLength="4" Width="90px"></asp:textbox>
					</td>
                    <!--*** 20140630 - TASI ***-->
					<td>
					    <asp:RadioButton ID="optICI" runat="server" CssClass="Input_Label" Text="ICI/IMU" Checked="true" GroupName="Tributo" TabIndex="2"/>
					    <br />
					    <asp:RadioButton ID="optTASI" runat="server" CssClass="Input_Label" Text="TASI" Checked="false" GroupName="Tributo" TabIndex="3"/>
					</td>
					<!--*** ***-->
					<td>
						<asp:label id="lblImportoPagamento" runat="server" CssClass="Input_Label">Importo Pagamento</asp:label>
						<asp:label id="Label2" style="FONT-FAMILY: Tahoma, Arial, Verdana; COLOR: red; FONT-SIZE: 12px" Runat="server">*</asp:label>
						<br />
						<asp:textbox id="txtImportoPagamento" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="4" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
					</td>
					<td>
						<asp:label id="lblDataPagamento" runat="server" CssClass="Input_Label">Data Pagamento</asp:label>
						<asp:label id="lblContrDataPag" style="FONT-FAMILY: Tahoma, Arial, Verdana; COLOR: red; FONT-SIZE: 12px" Runat="server">*</asp:label>
						<br />
						<asp:textbox id="txtDataPagamento" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlur(this);" tabIndex="5" runat="server" CssClass="Input_Text_Right TextDate" Text="" MaxLength="10"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label1" runat="server" CssClass="Input_Label">Data Riversamento</asp:label>
						<br />
						<asp:textbox id="TxtDataRiversamento" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlur(this);" tabIndex="6" runat="server" CssClass="Input_Text_Right TextDate" Text="" MaxLength="10"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td>
					    <asp:linkbutton id="lnkContoCorrente" runat="server" CausesValidation="False" cssclass="Input_Label" onclick="lnkContoCorrente_Click">Conto Corrente</asp:linkbutton><br />
						<asp:textbox id="txtContoCorrente" tabIndex="7" runat="server" CssClass="Input_Text_Right" Text="<%# Business.ApplicationHelper.ContoCorrente %>" MaxLength="25" Width="160px"></asp:textbox>
					</td>
					<td><asp:label id="lblNumBollettino" runat="server" cssclass="Input_Label">Num. Bollettino</asp:label><br />
						<asp:textbox id="txtNumBolletino" tabIndex="8" runat="server" CssClass="Input_Text_Right" Enabled="False" Text="" MaxLength="25" Width="160px"></asp:textbox></td>
					<td><asp:linkbutton id="lnkUbicazione" runat="server" CausesValidation="False" cssclass="Input_Label" onclick="lnkUbicazione_Click">Comune Ubicazione Immobile</asp:linkbutton><br />
						<asp:textbox id="txtComuneUbicazImmob" tabIndex="9" runat="server" CssClass="Input_Text" Text="<%# Business.ApplicationHelper.ComuneUbicazioneImmobile %>" MaxLength="50" Width="200px"></asp:textbox>
					</td>
					<td colspan="2"><asp:linkbutton id="lnkIntestatoA" runat="server" CausesValidation="False" cssclass="Input_Label" onclick="lnkIntestatoA_Click">Bollettino Intestato A</asp:linkbutton><br />
						<asp:textbox id="txtComuneIntestatrio" tabIndex="10" runat="server" CssClass="Input_Text" Text="<%# Business.ApplicationHelper.IntestatarioBollettino %>" MaxLength="60" Width="250px"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td class="Input_Label">
						<asp:label id="lblNumeroFabbricati" runat="server" Width="105px" cssclass="Input_Label" Height="2px">Numero Fabbricati </asp:label>
						<asp:label id="lblContrNFab" style="FONT-FAMILY: Tahoma, Arial, Verdana; COLOR: red; FONT-SIZE: 12px" Runat="server">*</asp:label>
						<br />
						<asp:textbox id="txtNumFabbricatiPosseduti" onkeypress="return NumbersOnly(event);" tabIndex="11" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" MaxLength="4" Width="90px"></asp:textbox>
					</td>
					<td colspan="5">
						<asp:label id="lblModoPagamento" runat="server" CssClass="Input_Label" Width="117px">Modo di Pagamento:</asp:label>
						<asp:label id="lblContrModoPag" style="FONT-FAMILY: Tahoma, Arial, Verdana; COLOR: red; FONT-SIZE: 12px" Runat="server">*</asp:label>&nbsp;&nbsp;
						<asp:label id="lblAvviso" runat="server" CssClass="Input_Label">Se il pagamento è in unica soluzione selezionare tutti e due i flag.</asp:label><br />
						<asp:checkbox id="chkAcconto" tabIndex="12" runat="server" CssClass="Input_Label" Text="Acconto"></asp:checkbox>&nbsp;
						<asp:checkbox id="chkSaldo" tabIndex="13" runat="server" CssClass="Input_Label" Text="Saldo"></asp:checkbox>
					</td>
				</tr>
				<tr>
					<td><asp:label id="lblImportoAbitazPrincipale" runat="server" cssclass="Input_Label">Importo Abitaz. Principale</asp:label><br />
						<asp:textbox id="txtImportoAbitazPrincipale" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="14" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
					</td>
					<td><asp:label id="lblDetrazioniAbitazPrincipale" runat="server" cssclass="Input_Label">Detrazione Abitaz. Principale</asp:label><br />
						<asp:textbox id="txtDetrazioniAbitazPrincipale" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="15" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
					</td>
				</tr>
				<tr>
				    <td>
					    <asp:label Width="200px" id="lblImportoAltriFabbricati" runat="server" cssclass="Input_Label">Importo Altri Fabbricati Comune</asp:label><br />
					    <asp:textbox id="txtImportoAltriFabbricati" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="16" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
				    </td>
				    <td>
					    <asp:label Width="200px" id="lblImportoAltriFabbricatiStatale" runat="server" cssclass="Input_Label">Importo Altri Fabbricati Stato</asp:label><br />
					    <asp:textbox id="txtImportoAltriFabbricatiStato" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="17" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
				    </td>
				    <td>
					    <asp:label Width="200px" id="lblImportoAreeFabbricabili" runat="server" cssclass="Input_Label">Importo Aree Fabbricabili Comune</asp:label><br />
					    <asp:textbox id="txtImportoAreeFabbricabili" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="18" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
				    </td>
				    <td>
					    <asp:label Width="200px" id="lblImportoAreeFabbricabiliStatale" runat="server" cssclass="Input_Label">Importo Aree Fabbricabili Stato</asp:label><br />
					    <asp:textbox id="txtImportoAreeFabbricabiliStato" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="19" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
				    </td>
				</tr>
				<tr>
				    <td>
					    <asp:label Width="200px" id="lblImportoTerreniAgricoli" runat="server" cssclass="Input_Label">Importo Terreni Agricoli Comune</asp:label><br />
					    <asp:textbox id="txtImportoTerreniAgricoli" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="20" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
				    </td>
				    <td>
					    <asp:label Width="200px" id="lblImportoTerreniAgricoliStatale" runat="server" cssclass="Input_Label">Importo Terreni Agricoli Stato</asp:label><br />
					    <asp:textbox id="txtImportoTerreniAgricoliStato" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="21" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
				    </td>
					<td>
						<asp:label id="lblImpFabRurUsoStrum" runat="server" cssclass="Input_Label">Importo Fabbricati Rurali</asp:label><br />
						<asp:textbox id="txtImpFabRurUsoStrum" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="22" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
					</td>
					<td>
						<asp:label id="lblImpFabRurUsoStrumStatale" runat="server" cssclass="Input_Label">Importo Fabbricati Rurali Stato</asp:label><br />
						<asp:textbox id="txtImpFabRurUsoStrumStato" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="23" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:label id="lblImpUsoProdCatD" runat="server" cssclass="Input_Label">Importo Uso Prod.Cat.D</asp:label><br />
						<asp:textbox id="txtImpUsoProdCatD" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="24" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
					</td>
					<td>
						<asp:label id="lblImpUsoProdCatDStatale" runat="server" cssclass="Input_Label">Importo Uso Prod.Cat.D Stato</asp:label><br />
						<asp:textbox id="txtImpUsoProdCatDStato" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="25" runat="server" CssClass="Input_Text_Right OnlyNumber" Text="" Width="120px"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:checkbox id="chkRavvedimentoOperoso" tabIndex="26" runat="server" CssClass="Input_Label" Text="Ravvedimento Operoso"></asp:checkbox>
					    <br />
					    <asp:checkbox id="chkViolazione" tabIndex="27" runat="server" CssClass="Input_Label" AutoPostBack="True" oncheckedchanged="chkViolazione_CheckedChanged" Text="Violazione"></asp:checkbox>
				    </td>
				    <td style="WIDTH: 126px">
				        <asp:label id="lblImportoSoprattassa" runat="server" CssClass="Input_Label">Importo Soprattassa</asp:label><br />
					    <asp:textbox id="txtImportoSoprattassa" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="28" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="120px"></asp:textbox>
					</td>
				    <td style="WIDTH:125px">
				        <asp:label id="lblPenaPecuniaria" runat="server" CssClass="Input_Label" Width="126px">Imp. Pena Pecuniaria</asp:label><br />
					    <asp:textbox id="txtPenaPecuniaria" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="29" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="120px"></asp:textbox>
					</td>
				    <td style="WIDTH: 123px">
				        <asp:label id="lblInteressi" runat="server" CssClass="Input_Label">Interessi</asp:label><br />
					    <asp:textbox id="txtInteressi" onkeypress="return NumbersOnly(event, true, true, 2);" tabIndex="30" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="90px"></asp:textbox></td>
				    <td style="WIDTH: 120px">
					    <asp:label id="lblNAtto" runat="server" CssClass="Input_Label">Numero Atto</asp:label><br />
					    <asp:TextBox Runat="server" ID="txtNAtto" CssClass="Input_Text" Width="123px" TabIndex="31"></asp:TextBox>
				    </td>
				    <td style="WIDTH:120px">
					    <asp:label id="lblDataAtto" runat="server" CssClass="Input_Label">Data Atto</asp:label><br />
					    <asp:TextBox Runat="server" ID="txtDataAtto" TabIndex="32" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);"></asp:TextBox>
				    </td>
				</tr>
				<tr>
					<td colSpan="6"><asp:label id="lblDatiProv" Runat="server" CssClass="lstTabRow" Width="100%">Dati Provenienza</asp:label>
						<!--<strong class="lstTabRow">Dati Provenienza</strong>--></td>
				</tr>
				<tr>
					<td colspan="6">
                        <asp:label id="lblProvenienza" runat="server" CssClass="Input_Label">Provenienza</asp:label><br />
                        <asp:dropdownlist id="ddlProvenienze" tabIndex="33" runat="server" CssClass="Input_Text" DataValueField="Codice" DataTextField="Descrizione" DataSource="<%# ListProvenienze() %>"> </asp:dropdownlist>
                        <input id="txtNameObject" type="hidden" name="txtNameObject" runat="server">
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
			<asp:textbox id="txtTypeOperation" tabIndex="28" runat="server" Text="" Visible="False"></asp:textbox>
	        <asp:button id="btnBonifica" style="DISPLAY: none" runat="server" Text="Bonifica"></asp:button>
	        <asp:linkbutton id="lbtnUpdate" style="DISPLAY: none" runat="server"></asp:linkbutton>
	        <asp:button id="btnSalva" style="DISPLAY: none" runat="server" Enabled="" ToolTip="Permette di salvare i dati della pagina per la modifica o l'inserimento dell'entità gestita" Text="Salva" onclick="btnSalva_Click"></asp:button>&nbsp;
		    <asp:button id="btnElimina" style="DISPLAY: none" runat="server" CausesValidation="False" Text="Elimina" onclick="btnElimina_Click"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;
		    <asp:button id="btnAbilita" style="DISPLAY: none" runat="server" CausesValidation="False" Text="Abilita" onclick="btnAbilita_Click"></asp:button>
		    <asp:button id="btnRibalta" style="DISPLAY: none" runat="server" CausesValidation="False" Text="Ribalta" onclick="btnRibalta_Click"></asp:button>
		    <asp:button id="btnIndietro" style="DISPLAY: none" runat="server" CausesValidation="False" Text="Indietro" onclick="btnIndietro_Click"></asp:button>
		</form>
	</body>
</HTML>
