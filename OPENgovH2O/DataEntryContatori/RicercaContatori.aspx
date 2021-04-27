<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaContatori.aspx.vb" Inherits="OpenUtenze.SearchContatori"%>
<HTML>
	<HEAD>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript">
			function MessageNotFound()
			{
				GestAlert('a', 'warning', '', '', 'La ricerca non ha prodotto risultati !!!');
				return false;
			}
			function Search(IsFromGIS)
			{
			    DivAttesa.style.display = '';
			    var valueUbicazione = document.getElementById('TxtCodVia').value;
			    var statoContatore = document.getElementById('cboStato').value;
			    var varSub = 0;
			    if (document.getElementById('chksub').checked)
			        varSub = 1;
			    Parametri = "?intestatario=" + document.getElementById('txtNominativoIntestatario').value + "&utente=" + document.getElementById('txtNominativoUtente').value + "&ubicazione=" + valueUbicazione + "&numeroutente=" + document.getElementById('txtNumeroUtente').value + "&matricola=" + document.getElementById('txtMatricola').value;
			    Parametri += "&sub=" + varSub;
			    Parametri += "&nomeUtente=" + document.getElementById('txtNomeUtente').value + "&nomeIntestatario=" + document.getElementById('txtNomeIntestatario').value;
			    if (IsFromGIS == '1')
			        Parametri += "&Foglio=" + document.getElementById('txtFoglio').value + "&Numero=" + document.getElementById('txtNumero').value;
			    Parametri += "&statoContatore=" + statoContatore;
			    //*** 201511 - Funzioni Sovracomunali ***
			    Parametri += "&IdEnte=" + document.getElementById('ddlEnti').value;
			    Parametri += "&Contratto=" + document.getElementById('txtContratto').value;
			    document.getElementById('loadGrid').src = "../DataEntryContatori/SearchResultsContatori.aspx" + Parametri;
			    return true;
			}
			
			function PulisciCampi()
			{
			    document.getElementById('txtMatricola').value = '';
			    document.getElementById('txtContratto').value = '';
			    document.getElementById('txtNumeroUtente').value='';
			    document.getElementById('txtNominativoIntestatario').value='';
			    document.getElementById('txtNominativoUtente').value='';
				
				document.getElementById('loadGrid').src="../../aspVuota.aspx"
				document.getElementById('txtMatricola').focus();
			}
			function keyPress()
			{
				if (window.event.keyCode==13)
				{
					Search();
				}
			}	
			function GetStradario(objFieldHidden,objFieldUbicazione,CodComune,objForm) 
			{
				HIDDEN=objFieldHidden.name;
				FORM=objForm.name;
				UBICAZIONE=objFieldUbicazione.name;
			
				WinPopUp=OpenPopup('OpenUtenze','<%=Session("PATH_APPLICAZIONE")%>/Selezioni/FrameListaSelezioni.aspx?SEARCHVIA=' + objFieldUbicazione.value+'&HIDDEN=' + HIDDEN + '&FORM=' +FORM +'&UBICAZIONE='+ UBICAZIONE +'&CODCOMUNE='+CodComune.value,'Stradario','770','500',0,0,'yes','no');
			}
			function Nuovo()
			{
				document.getElementById('btnNuovo').click();
			}
			
			function Ex()
			{
			    DivAttesa.style.display = '';
			    document.getElementById('StampaExcel').click();
			}
			
			function ApriStradario(FunzioneRitorno, CodEnte)
			{					
				var TipoStrada = '';
				var Strada = '';
				var CodStrada = '';
				var CodTipoStrada = '';
				var Frazione = '';
				var CodFrazione = '';
	        
				var Parametri = '';
	        
				if (CodEnte == '-1' || CodEnte == '')
				    CodEnte = $('#ddlEnti').val();
				Parametri += 'CodEnte=' + CodEnte;
				Parametri += '&TipoStrada='+TipoStrada;
				Parametri += '&Strada='+Strada;
				Parametri += '&CodStrada='+CodStrada;
				Parametri += '&CodTipoStrada='+CodTipoStrada;
				Parametri += '&Frazione='+Frazione;
				Parametri += '&CodFrazione='+CodFrazione;
				Parametri += '&Stile=StylesGrandCombin.css';
				Parametri += '&Stile=<% = Session("StileStradario") %>';
				Parametri += '&FunzioneRitorno='+FunzioneRitorno;
				
				window.open('<% response.write(UrlStradario) %>?'+Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');
				return false;
			}

			function RibaltaStrada(objStrada)
			{
				// popolo il campo descrizione della via di residenza
			    document.getElementById('TxtVia').value = (objStrada.TipoStrada + ' ' + objStrada.Strada).replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
			    document.getElementById('TxtViaRibaltata').value = (objStrada.TipoStrada + ' ' + objStrada.Strada).replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
				// popolo il campo codvia residenza
			    document.getElementById('TxtCodVia').value = objStrada.CodStrada;
			}

			function ClearDatiVia()
			{
			    document.getElementById('TxtVia').value = '';
			    document.getElementById('TxtViaRibaltata').value = ' ';
				document.getElementById('TxtCodVia').value ='-1';
				return false;
			}
			
			function ContrattiFromContatori()
			{
			    document.getElementById('CmdContrattiFromContatori').click();
			}
		</script>
	</HEAD>
	<body onload="document.getElementById('txtMatricola').focus();" class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<table border="0">
				<tr>
					<td>
					    <fieldset class="FiledSetRicerca">
					        <legend class="Legend">Inserimento filtri di ricerca</legend>
							<table border="0">
								<tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblEnti" runat="server" CssClass="Input_Label">Ente</asp:Label><br />
                                        <asp:DropDownList ID="ddlEnti" runat="server" CssClass="Input_Text" DataTextField="string" DataValueField="string"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
									<td class="Input_Label">Matricola</td>
                                    <td class="Input_Label">Cod.Contratto</td>
									<td class="Input_Label">N. Utente</td>
									<td class="Input_Label" colspan="2">Ubicazione
										<asp:imagebutton id="LnkOpenStradario" runat="server" ToolTip="Ubicazione Immobile da Stradario."
											CausesValidation="false" imagealign="Bottom" ImageUrl="../../Images/Bottoni/Listasel.png"></asp:imagebutton>&nbsp;
										<asp:imagebutton id="LnkPulisciStrada" runat="server" ToolTip="Pulisci i campi della Via" CausesValidation="false"
											imagealign="Bottom" ImageUrl="../../Images/Bottoni/cancel.png"></asp:imagebutton>
									</td>
								</tr>
								<tr>
									<td><asp:textbox id="txtMatricola" runat="server" Width="130px" maxlength="20" cssclass="Input_Text"></asp:textbox></td>
                                    <td><asp:textbox id="txtContratto" runat="server" Width="130px" maxlength="20" cssclass="Input_Text"></asp:textbox></td>
									<td><asp:textbox id="txtNumeroUtente" runat="server" Width="100px" maxlength="18" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}"></asp:textbox></td>
									<td colspan="2">
										<asp:TextBox ID="TxtVia" cssclass="Input_Text" Runat="server" Width="400px" ReadOnly="True"></asp:TextBox>&nbsp;
										<asp:TextBox id="TxtCodVia" style="DISPLAY: none" Runat="server">-1</asp:TextBox>
                                        <asp:TextBox ID="TxtViaRibaltata" Style="display: none" CssClass="Input_Text" runat="server"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td class="Input_Label" colspan="2">Cognome Intestatario</td>
									<td class="Input_Label" colspan="2">Nome Intestatario</td>
									<td class="Input_Label">Stato del contatore</td>
								</tr>
								<tr>
									<td colspan="2">
									    <asp:textbox id="txtNominativoIntestatario" runat="server" Width="376px" maxlength="100" cssclass="Input_Text"></asp:textbox>
									</td>
									<td colspan="2">
									    <asp:TextBox ID="txtNomeIntestatario" Runat="server" Width="185px" Cssclass="Input_Text"></asp:TextBox>
									</td>
									<!-- ALE CAO -->
									<td class="Input_Label">
										<asp:DropDownList ID="cboStato" Runat="server" Cssclass="Input_Text" Width="150"></asp:DropDownList>
									</td>
									<!-- FINE ALE CAO -->
								</tr>
								<tr>
									<td class="Input_Label" colspan="2">Cognome Utente</td>
									<td class="Input_Label" colspan="2">Nome Utente</td>
								</tr>
								<tr>
									<td colspan="2"><asp:textbox id="txtNominativoUtente" runat="server" Width="376px" maxlength="100" cssclass="Input_Text"></asp:textbox></td>
									<td colspan="2"><asp:TextBox ID="txtNomeUtente" Runat="server" Width="185px" Cssclass="Input_Text"></asp:TextBox></td>
									<td class="Input_Label"><asp:CheckBox ID="chksub" Text="Sub-Contatore" Runat="server"></asp:CheckBox></td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td>
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
				<tr>
					<td>
						<iframe class="bordoIFRAME" id="loadGrid" frameBorder="0" width="100%" height="360" src="../../aspVuota.aspx"></iframe>
					</td>
				</tr>
			</table>
			<input id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza"> 
			<input id="hdCodiceVia" type="hidden" value="-1" name="hdCodiceVia">
			<input id="paginacomandi" type="hidden" name="paginacomandi">
			<asp:button id="btnNuovo" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="CmdContrattiFromContatori" style="DISPLAY: none" runat="server"></asp:button>
			<asp:Button id="StampaExcel" runat="server" Text="Button" style="DISPLAY:none" />
			<asp:button id="CmdGIS" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:TextBox ID="txtFoglio" runat="server" style="display:none"></asp:TextBox>
			<asp:TextBox ID="txtNumero" runat="server" style="display:none"></asp:TextBox>
		</form>
	</body>
</HTML>
