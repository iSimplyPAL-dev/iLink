<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaContratti.aspx.vb" Inherits="OpenUtenze.RicercaContratti"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
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
	<%--	<script type="text/javascript" src="../../_js/MyUtility.js?newversion"></script>--%>
		<script type="text/javascript"  src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript">		
		function Search(){
		    DivAttesa.style.display = '';
		    // alert("Ok, questa è la funzione di ricerca");
					//var iselUbicazione = document.getElementById('cboUbicazione.selectedIndex;
		    var iselStato=document.getElementById('cboStato').selectedIndex;
					//var valueUbicazione=document.getElementById('cboUbicazione[iselUbicazione]').value;
					var valueUbicazione = document.getElementById('TxtCodVia').value;
					var intestatario=document.getElementById('txtNominativoIntestatario').value;
					var utente=document.getElementById('txtNominativoUtente').value;
					var NomeUtente=document.getElementById('NomeUtente').value;
					var NomeIntestatario=document.getElementById('NomeIntestatario').value;
				//	Parametri="?matricola="+document.frmRicerca.txtMatricola.value+"&ubicazione="+document.frmRicerca.cboUbicazione[iselUbicazione].value+"&numeroutente="+document.frmRicerca.txtNumeroUtente.value
				
					//Parametri="?utente="+utente+"&NomeUtente="+NomeUtente+"&NomeIntestatario="+NomeIntestatario+"&intestatario="+intestatario+"&ubicazione="+valueUbicazione+"&stato="+iselStato+"&codContratto="+document.getElementById('txtCodiceContratto.value;
					Parametri="?utente="+utente+"&NomeUtente="+NomeUtente+"&NomeIntestatario="+NomeIntestatario+"&intestatario="+intestatario+"&ubicazione="+encodeURIComponent(valueUbicazione)+"&stato="+iselStato+"&codContratto="+document.getElementById('txtCodiceContratto').value;
				
				//GestAlert('a', 'warning', '', '', 'Ricerca: ' + Parametri);
					document.getElementById('loadGrid').src="../DataEntryContratti/SearchResultsContratti.aspx"+Parametri
			
				return true;
		}
		
		function Nuovo(){
		//alert("Ok, questa è la funzione per aggiungere un contratto");
		    document.getElementById('btnNuovo').click();
		}
		
		function Ex(){
		    DivAttesa.style.display = '';
		    document.getElementById('StampaExcel').click();
//		    parent.loadGrid.document.getElementById("imgStampaRiassuntiva").click();
//		parent.document.getElementById("loadGrid").document.getElementById("imgStampaRiassuntiva").click();
		}
		
		
		function PulisciCampi(){
		//alert("Questa pulisce tutti i campi di ricerca");
		document.getElementById('txtNominativoIntestatario').value="";
		document.getElementById('txtNominativoUtente').value="";
		document.getElementById('chkpendente').checked = false;
		document.getElementById('txtCodiceContratto').value="";
		//document.getElementById('cboUbicazione.selectedIndex=0;
		document.getElementById('NomeUtente').value="";
		document.getElementById('NomeIntestatario').value="";
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
				
				window.open('<% response.Write(UrlStradario) %>?'+Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');
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
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" marginheight="0" marginwidth="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td>
					    <fieldset class="FiledSetRicerca" style="WIDTH: 98%;">
					        <legend class="Legend">Inserimento filtri di ricerca</legend>
							<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
								<tr>
									<td class="Input_Label">Cognome intestatario</td>
									<td class="Input_Label">Nome Intestatario</td>
								</tr>
								<tr>
									<td>
									    <asp:textbox id="txtNominativoIntestatario" tabIndex="1" runat="server" cssclass="Input_Text" maxlength="100" Width="376px"></asp:textbox>
									</td>
									<td>
									    <asp:textbox id="NomeIntestatario" Cssclass="Input_Text" Width="185px" Runat="server" tabIndex="2"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td class="Input_Label">Cognome utente</td>
									<td class="Input_Label">Nome utente</td>
								</tr>
								<tr>
									<td>
									    <asp:textbox id="txtNominativoUtente" tabIndex="4" runat="server" cssclass="Input_Text" maxlength="100" Width="376px"></asp:textbox>
									</td>
									<td>
									    <asp:textbox id="NomeUtente" Cssclass="Input_Text" Runat="server" width="185px" tabIndex="5"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td class="Input_Label">Ubicazione
										<%--<asp:imagebutton id="imgVuota" runat="server" tooltip="ciao" width="1" Height="1"></asp:imagebutton>--%>
										<asp:imagebutton id="LnkOpenStradario" runat="server" ToolTip="Ubicazione Immobile da Stradario." CausesValidation="False" imagealign="Bottom" ImageUrl="../../Images/Bottoni/Listasel.png"></asp:imagebutton>&nbsp;
										<asp:imagebutton id="LnkPulisciStrada" runat="server" ToolTip="Pulisci i campi della Via" CausesValidation="False" imagealign="Bottom" ImageUrl="../../Images/Bottoni/cancel.png"></asp:imagebutton>
									</td>
									<td class="Input_Label">Codice contratto</td>
									<td class="Input_Label">Stato del contratto</td>
								<tr>
									<td><!--<asp:dropdownlist id="cboUbicazione" tabIndex="3" runat="server" Cssclass="Input_Text"></asp:dropdownlist>-->
										<asp:TextBox ID="TxtVia" cssclass="Input_Text" Runat="server" Width="400px" TabIndex="3" ReadOnly="True"></asp:TextBox>&nbsp;
										<asp:TextBox id="TxtCodVia" style="DISPLAY: none" Runat="server">-1</asp:TextBox>
									</td>
									<td>
									    <asp:textbox id="txtCodiceContratto" tabIndex="6" runat="server" cssclass="Input_Text" maxlength="100" Width="100"></asp:textbox><asp:button id="btnNuovo" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
									</td>
									<td class="Input_Label"><asp:dropdownlist id="cboStato" Cssclass="Input_Text" Runat="server" Width="150"></asp:dropdownlist></td>
									<!-- ALE CAO -->
									<!--<td class="Input_Label" width="25%" align="center">Pendente<asp:CheckBox ID="chkpendente" Runat="server"></asp:CheckBox>--></tr>
							</table>
						</fieldset>
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
						<iframe class="bordoIFRAME" id="loadGrid" name="loadGrid" frameBorder="0" width="100%" height="350px" src="../../aspVuota.aspx"></iframe>
					</td>
				</tr>
			</table>
			<asp:Button id="StampaExcel" runat="server" Text="Button" style="DISPLAY:none"></asp:Button>
		</form>
	</body>
</HTML>
