<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaLetture.aspx.vb" Inherits="OpenUtenze.RicercaLetture" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Ricerca Letture</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta content="True" name="vs_showGrid">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript">
			function Ex()
			{
				//alert("STAMPA LETTURISTA");
			    DivAttesa.style.display = '';
			    document.getElementById('StampaExcel').click();
			}
			
			function Ex2(){
				//alert("STAMPA CONTATORI CESSATI, SOSTITUITI, SOSPESI");
			    DivAttesa.style.display = '';
			    document.getElementById('StampaExcel2').click();
			}
			
			function Ex3(){
				//alert("STAMPA LETTURE");
			    DivAttesa.style.display = '';
			    document.getElementById('StampaExcel3').click();
			}
			
			function Ex4(){
			    DivAttesa.style.display = '';
			    document.getElementById('StampaExcel4').click();
			}
			
			function MessageNotFound()
			{
				GestAlert('a', 'warning', '', '', 'La ricerca non ha prodotto risultati!');
				return false;
			}

			function Search() {
			    DivAttesa.style.display = '';
			    var iselGiro = document.getElementById('cboGiro').selectedIndex;
			    var iselCessati = document.getElementById('cboContatoriCessati').selectedIndex;
			    var valueGiro=document.getElementById('cboGiro').value
			    var valueCessati=document.getElementById('cboContatoriCessati').value
			    var valueUbicazione=document.getElementById('TxtCodVia').value
							
			    Parametri = "?intestatario=" + document.getElementById('txtIntestatario').value + "&utente=" + document.getElementById('txtUtente').value + "&ubicazione=" + valueUbicazione + "&giro=" + valueGiro + "&numeroutente=" + document.getElementById('txtNumeroUtente').value + "&cessati=" + valueCessati + "&paginacomandi=" + document.getElementById('paginacomandi').value + "&matricola=" + document.getElementById('txtMatricola').value + "&sub=" + document.getElementById('chksub').checked + "&LetturaPresente=" + document.getElementById('chkLetturaPresente').checked + "&LetturaMancante=" + document.getElementById('chkLetturaMancante').checked;
				document.getElementById('loadGrid').src="../DataEntryLetture/SearchResultsContatoriLetture.aspx"+Parametri
				return true;
			 }
	
			function PulisciCampi()
			{
			    document.getElementById('txtIntestatario').value='';
			    document.getElementById('txtUtente').value='';
			    document.getElementById('txtMatricola').value='';
			    document.getElementById('chksub').checked=false;
			    document.getElementById('hdCodiceVia').value='-1';
			    document.getElementById('txtNumeroUtente').value='';
			    document.getElementById('cboContatoriCessati').value='-1';
			    document.getElementById('cboGiro').value='-1';
			    document.getElementById('txtPulisciCampi').value='1';
				//Ripulire il filtro
				document.getElementById('loadGrid').src="../../aspVuota.aspx"
				
				Setfocus(document.getElementById('txtIntestatario'));
		      }	

			function keyPress()
			{
				if (window.event.keyCode==13)
				{
					Search();
				}
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
		        
					Parametri += 'CodEnte='+CodEnte;
					Parametri += '&TipoStrada='+TipoStrada;
					Parametri += '&Strada='+Strada;
					Parametri += '&CodStrada='+CodStrada;
					Parametri += '&CodTipoStrada='+CodTipoStrada;
					Parametri += '&Frazione='+Frazione;
					Parametri += '&CodFrazione='+CodFrazione;
				    Parametri += '&Stile=<% = Session("StileStradario") %>';
					Parametri += '&FunzioneRitorno='+FunzioneRitorno;
					
					window.open('<% response.write(UrlStradario) %>?'+Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');
					return false;
				}

				function RibaltaStrada(objStrada)
				{
					// popolo il campo descrizione della via di residenza
				    document.getElementById('TxtVia').value = (objStrada.TipoStrada + ' ' + objStrada.Strada).replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
					// popolo il campo codvia residenza
					document.getElementById('TxtCodVia').value = objStrada.CodStrada;					
				}

				function ClearDatiVia()
				{
					document.getElementById('TxtVia').value='';
					document.getElementById('TxtCodVia').value ='-1';
					return false;
				}
		</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" onload="document.getElementById('txtIntestatario').focus();"
		marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td>
					    <fieldset class="FiledSetRicerca" style="WIDTH: 98%;">
					        <legend class="Legend">Inserimento filtri di ricerca</legend>
							<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
								<tr>
									<td class="Input_Label">Intestatario</td>
									<td class="Input_Label" colSpan="2">Utente</td>
									<td class="Input_Label" colSpan="2">Ubicazione
										<asp:imagebutton runat="server" tooltip="ciao" width="1" Height="1" ID="Imagebutton1"></asp:imagebutton>
										<asp:imagebutton id="LnkOpenStradario" runat="server" ToolTip="Ubicazione Immobile da Stradario."
											CausesValidation="False" imagealign="Bottom" ImageUrl="../../Images/Bottoni/Listasel.png"></asp:imagebutton>&nbsp;
										<asp:imagebutton id="LnkPulisciStrada" runat="server" ToolTip="Pulisci i campi della Via" CausesValidation="False"
											imagealign="Bottom" ImageUrl="../../Images/Bottoni/cancel.png"></asp:imagebutton></td>
								</tr>
								<tr>
									<td>
										<asp:textbox id="txtIntestatario" tabIndex="1" runat="server" Width="250px" maxlength="100" cssclass="Input_Text"></asp:textbox>
									</td>
									<td colSpan="2"><asp:textbox id="txtUtente" tabIndex="2" runat="server" Width="250px" maxlength="100" cssclass="Input_Text"></asp:textbox></td>
									<td colSpan="2"><!--<asp:dropdownlist id="cboUbicazione" tabIndex="3" runat="server" Width="248px" Cssclass="Input_Text"
								onkeydown="keyPress();"></asp:dropdownlist>--><asp:textbox readonly="True" id="TxtVia" tabIndex="3" Width="250px" cssclass="Input_Text"
											Runat="server"></asp:textbox>&nbsp;
										<asp:textbox id="TxtCodVia" style="DISPLAY: none" Runat="server">-1</asp:textbox></td>
								</tr>
								<tr>
									<td class="Input_Label">Giro</td>
									<td class="Input_Label">Numero Utente</td>
									<td class="Input_Label">Matricola</td>
									<td></td>
									<td></td>
								</tr>
								<tr>
									<td><asp:dropdownlist id="cboGiro" tabIndex="4" runat="server" Cssclass="Input_Text" Width="228px"></asp:dropdownlist></td>
									<td><asp:textbox id="txtNumeroUtente" tabIndex="5" runat="server" Width="100px" maxlength="18" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}"></asp:textbox></td>
									<td><asp:textbox id="txtMatricola" tabIndex="6" runat="server" Width="100px" maxlength="20" cssclass="Input_Text"></asp:textbox></td>
									<td class="Input_Label"><asp:checkbox id="chksub" Text="Sub-Contatore" Runat="server"></asp:checkbox></td>
									<td class="Input_Label"><asp:checkbox id="chkLetturaPresente" Text="Lettura Presente" Runat="server"></asp:checkbox></td>
									<td class="Input_Label"><asp:checkbox id="chkLetturaMancante" Text="Lettura Mancante" Runat="server"></asp:checkbox></td>
								</tr>
								<tr>
									<asp:dropdownlist id="cboContatoriCessati" style="DISPLAY: none" tabIndex="7" runat="server" CssClass="Input_Text"
										Width="248px">
										<asp:ListItem Value="-1">...</asp:ListItem>
										<asp:ListItem Value="1">Cessati Anno Corrente</asp:ListItem>
										<asp:ListItem Value="2">Cessati Anno Precedente</asp:ListItem>
									</asp:dropdownlist>
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
					<td><iframe class="bordoIFRAME" id="loadGrid" src="../../aspVuota.aspx" frameBorder="0" width="100%" height="370"></iframe></td>
				</tr>
			</table>
			<input id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza"> 
			<input id="hdCodiceVia" type="hidden" value="-1" name="hdCodiceVia">
			<input id="paginacomandi" type="hidden" name="paginacomandi">
			<asp:button id="StampaExcel" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
			<asp:button id="StampaExcel2" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
			<asp:button id="StampaExcel3" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
			<asp:button id="StampaExcel4" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
			<asp:textbox id="txtIDGIRO" style="DISPLAY: none" runat="server"></asp:textbox>
			<asp:textbox id="txtCODINTESTATARIO" style="DISPLAY: none" runat="server"></asp:textbox>
			<asp:textbox id="txtCODUTENTE" style="DISPLAY: none" runat="server"></asp:textbox>
			<asp:textbox id="txtPulisciCampi" style="DISPLAY: none" runat="server"></asp:textbox>
			<asp:textbox id="txtRicerca" style="DISPLAY: none" runat="server"></asp:textbox>
		</form>
	</body>
</HTML>
