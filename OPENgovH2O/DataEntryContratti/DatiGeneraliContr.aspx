<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DatiGeneraliContr.aspx.vb" Inherits="OpenUtenze.DatiGeneraliContr"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<HTML>
	<HEAD>
        <title></title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta content="False" name="vs_showGrid">
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
		<%--<script type="text/javascript" src="../../_js/MyUtility.js?newversion"></script>--%>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<%--<script type="text/javascript" src="../../_js/NumbersOnly.js?newversion"></script>--%>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript">
		function ApriNuovo(parametro)
		{
			myleft=((screen.width)/2)-325;
			mytop=((screen.height)/2)-100;	
			window.open("../DataEntryContatori/FrameInserisciDatoCatasto.aspx?IDContatore="+parametro+"&noinsert=1","Catasto","width=650, height=200, toolbar=no,top="+mytop+",left="+myleft+", menubar=no");
		}		
		
		function DeleteContratto()
		{
			document.getElementById('btnEliminaContratto').click();
		}

		function ShowRicUIAnater(ViaContatore)
		{
			winWidth=800 
			winHeight=500 
			myleft=(screen.width-winWidth)/2 
			mytop=(screen.height-winHeight)/2 - 40 
			WinPopFamiglia=window.open('../RicercaAnater/FrmRicercaImmobile.aspx?Via='+document.getElementById('TxtVia').value,'','width='+winWidth+',height='+winHeight+', status=yes, toolbar=no,top='+mytop+',left='+myleft+',Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no') 
		}
		
		function ApriRicAnater()
		{
			// apro il popup di ricerca in anagrafe anater
			winWidth=980 
			winHeight=500 
			myleft=(screen.availWidth-winWidth)/2 
			mytop=(screen.availheight-winHeight)/2 - 40 
			var parametri = 'popup=1';
			WinPopAnater=window.open('../AnagrafeAnater/popAnagAnater.aspx?'+parametri,'','width='+winWidth+',height='+winHeight+',top='+mytop+',left='+myleft+' status=yes, toolbar=no,scrollbar=no, resizable=no') 
		}

		function controlla(max,Max,maxlettere) 
		{
			if (max.value.length >maxlettere) 
			max.value = max.value.substring(0,maxlettere);
		}
		
		function DisabilitaFognatura()
		{
			if(document.getElementById('chkEsenteFognatura').checked==true)
			{
				document.getElementById('cboFognatura').selectedIndex='0';
				document.getElementById('cboFognatura').disabled=true;
			}
			else
			{
				document.getElementById('cboFognatura').disabled=false;
			}	
		}
		
		function DisabilitaDepurazione()
		{
			if(document.getElementById('chkEsenteDepurazione').checked==true)
			{
				document.getElementById('cboDepurazione').selectedIndex='0';
				document.getElementById('cboDepurazione').disabled=true;
			}
			else
			{
				document.getElementById('cboDepurazione').disabled=false;
			}
		}
		
		function MessageNotFound()
		{
			GestAlert('a', 'warning', '', '', 'La ricerca non ha prodotto risultati!');
			return false;
		}

		function VerificaObbligatori()
		{	
			sMsg=""
			var iselNumeroUtenze=document.getElementById('txtNumeroUtenze').value;
			
			if(document.getElementById('txtIdentificativoContratto').value == '')
			{
				sMsg = sMsg + "[Codice Contratto]\n" ;
			}
			
			if(document.getElementById('txtDataSottoscr').value == '')
			{
				sMsg = sMsg + "[Data Sottoscrizione]\n" ;
			}
			
			if(document.getElementById('miointestatario').value == '-1')
			{
				sMsg = sMsg + "[Anagrafica Intestatario]\n" ;
			}
			
			if(document.getElementById('mioutente').value == '-1' )
			{
				sMsg = sMsg + "[Anagrafica Utente]\n" ;
			}
			
			if(iselNumeroUtenze=="" || iselNumeroUtenze<1){
			sMsg=sMsg + "[Numero Utenze]\n" ;
			}
			
			if (document.getElementById('cboTipoUtenze').value == -1) {
			sMsg=sMsg + "[Tipo Utenza]\n" ;
			}
			
			if (sMsg!="") 
			{ 
					strMessage ="Attenzione...\n\n I campi elencati sono obbligatori!\n\n" 
					alert(strMessage + sMsg);
					Setfocus(document.getElementById('txtNumeroUtente')) 
					return false; 		
			}		
			return true; 
		}

		function VerificaCampi()
		{	
			sMsg=""
			var iselGiro = document.getElementById('cboGiro').selectedIndex;
			var iselFognatura = document.getElementById('cboFognatura').selectedIndex;
			var iselDepurazione = document.getElementById('cboDepurazione').selectedIndex;
			var iselTipoContatore = document.getElementById('cboTipoContatore').selectedIndex;
			var iselUbicazione = document.getElementById('cboUbicazione').selectedIndex;
			
			if (IsBlank(document.getElementById('txtMatricola').value)) 
			{ 
				sMsg = sMsg + "[Matricola]\n" ; 
			}
			if (IsBlank(document.getElementById('txtNumeroUtente').value)) 
			{ 
				sMsg = sMsg + "[Numero Utente]\n" ; 
			}
			
			if (IsBlank(document.getElementById('txtEnteAppartenenza').value ))
			{
				sMsg = sMsg + "[Ente Appartenenza]\n" ; 
			}						
			if (!IsBlank(document.getElementById('txtEnteAppartenenza').value ) && IsBlank(document.getElementById('hdEnteAppartenenza').value ))
			{
				sMsg = sMsg + "[Ente Appartenenza non Selezionato da lista Enti]\n" ; 
			}						
			if(document.getElementById('chkEsenteFognatura').checked==false)
			{
				if(document.getElementById('cboFognatura[iselFognatura]').value == -1)
				{
					sMsg = sMsg + "[Codice Fognatura]\n" ; 
				}
			}			
			if(document.getElementById('chkEsenteDepurazione').checked==false)
			{
				if(document.getElementById('cboDepurazione[iselDepurazione]').value == -1)
				{
					sMsg = sMsg + "[Codice Depurazione]\n" ; 
				}
			}		
			if (!IsBlank(sMsg)) 
			{ 
					strMessage ="Attenzione...\n\n I campi elencati sono obbligatori!\n\n" 
					alert(strMessage + sMsg);
					Setfocus(document.getElementById('txtNumeroUtente')) 
					return false; 
			}		
			return true; 
		}	
		
		function Salva()
		{
			if (confirm('Si vogliono salvare le modifiche apportate?'))
			{
				if(VerificaObbligatori())
				{
					if(document.getElementById('txtDataCessazione').value!="")
					{
						if (confirm('Verra\' effettuata la voltura: continuare?'))
						{
						document.getElementById('btnEvento2').click(); 
						}
					}
					else
					{
					document.getElementById('btnEvento2').click(); 	
					}
				}
			}		
		}
		
		//ALE CAO
		function Stampa()
		{
			if (confirm('Si vuole creare un nuovo documento ed effettuare la stampa del contratto?'))
			{
			document.getElementById('btnStampa').click();
			}
		}
		
		function Stampa2()
		{
			if (confirm('Si vuole creare un nuovo documento ed effettuare la stampa del preventivo?'))
			{
			document.getElementById('btnStampa2').click();
			}
		}
		//FINE ALE CAO
			
		function ApriAnagrafica(codcontrib, sessionName) {

            
            // M.B. ANAGRAFICAH2O
			cod_contribuente=codcontrib
			winWidth=980
			winHeight=680
			myleft=(screen.width-winWidth)/2 
			mytop=(screen.height-winHeight)/2 - 40
							
			PaginaPrec="DatiGeneraliContr"
			PaginaComandi="../../Anagrafica/ComandiInsertSaveAnagrafica.aspx"		
			WinPopUpRicercaAnagrafica=window.open("../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?COD_CONTRIBUENTE=" + cod_contribuente +"&sessionName=" + sessionName + "&paginacomandi="+PaginaComandi +"&paginaprec="+PaginaPrec+"&sOperazione=dettaglio","APRIANAGRAFE","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
		}		
		
		function Annulla()
		{
			parent.Visualizza.location.href = "RicercaContratti.aspx";
		}
		
		function PulisciAnagraficaIntestatario()
		{
			document.getElementById('txtCognomeIntestatario').value='';
			document.getElementById('txtCodiceFiscaleIntestatario').value='';
			document.getElementById('txtViaIntestatario').value='';	
			document.getElementById('hdCodAnagrafeIntestatario').value ='-1';
			document.getElementById('HDtxtCodIntestatario').value='-1';
			document.getElementById('miointestatario').value='-1';
		}
		
		function PulisciAnagraficaUtente()
		{
			document.getElementById('txtCognomeUtente').value='';
			document.getElementById('txtCodiceFiscaleUtente').value='';
			document.getElementById('txtViaUtente').value='';	
			document.getElementById('hdCodAnagrafeUtente').value ='-1';
			document.getElementById('HDTextCodUtente').value ='-1';
			document.getElementById('mioutente').value='-1';
		}
		
		function CalcolaNumeroUtente()
		{
			if (confirm('Si desidera calcolare un nuovo Numero Utente?'))
			{
				document.getElementById('CalcolaNU').click(); 
				return true;
			}
			else
			{
			return false;
			}
		}

		function ControllaFuoco()
		{
			document.getElementById('txtNumeroUtente').focus();
		}
		
		function AssegnaQuoteAgevolate()
		{
			if(document.getElementById('hdRiportaNumeroUtenze').value =='1')
			{
				document.getElementById('txtQuoteAgevolate').value=document.getElementById('txtNumeroUtenze').value
			}
		}
		
		
		function attivaContatore()
		{
			myleft=((screen.width)/2)-350;
			mytop=((screen.height)/2)-250;
			window.open("FrameAttivazione.aspx","Attivazione","width=700, height=500, toolbar=no,top="+mytop+",left="+myleft+", menubar=no,status=yes");
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
				document.getElementById('TxtCodVia').value='-1';
				return false;
			}
		</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" onload="ControllaFuoco();">
		<form id="Form1" runat="server" method="post">
			<asp:button id="btnEliminaContratto" style="DISPLAY: none" runat="server" Text="Button"></asp:button><br>
			<!-- inseriti il 07/02/07 per gestione CMCG -->
			<asp:textbox id="HDtxtCodIntestatario" style="DISPLAY: none" runat="server"></asp:textbox><asp:textbox id="HDTextCodUtente" style="DISPLAY: none" runat="server"></asp:textbox>
			<asp:button id="btnRibalta" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
			<asp:button id="btnRibaltaAnagAnater" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:textbox id="txtidContatore" style="DISPLAY: none" runat="server"></asp:textbox>
			<!-- inseriti il 07/02/07 per gestione CMCG -->
			<asp:button id="CalcolaNU" style="DISPLAY: none" Text="CalcolaNumeroUtente" Runat="server"></asp:button>
			<table cellSpacing="0" cellPadding="1" width="98%" align="center" border="0" id="TblGen">
				<tr>
					<td colSpan="6"><asp:label id="lblError" runat="server" Cssclass="NormalBold"></asp:label></td>
				</tr>
				<tr>
					<td class="lstTabRow" colSpan="6">Dati Generali</td>
				</tr>
				<tr>
					<td class="Input_Label">Codice Contratto <FONT class="NormalRed">*</FONT></td>
					<td class="Input_Label">Data di Sottoscrizione <font class="NormalRed">*</font></td>
					<td class="Input_Label">N.Utente&nbsp; <input class="Bottone BottoneNUtente" id="btnCodiceFiscale" title="Permette di assegnare un Numero Utente se mancante"
							onclick="CalcolaNumeroUtente();" type="button" name="btnCodiceFiscale" width="30">
					</td>
					<td class="Input_Label" colspan="2">Ultimo Codice Contratto Inserito</td>
				</tr>
				<tr>
					<td>
						<asp:textbox id="txtIdentificativoContratto" runat="server" Cssclass="Input_Text"></asp:textbox>
					</td>
					<td>
						<asp:textbox id="txtDataSottoscr" runat="server" Cssclass="Input_Text" MaxLength="10" TextMode="SingleLine" AutoPostBack="True" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox>
					</td>
					<td>
						<asp:textbox id="txtNumeroUtente" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}" MaxLength="50"></asp:textbox>
					</td>
					<td colspan="2">
						<asp:textbox id="txtLastIdContratto" runat="server" Cssclass="Input_Text" ReadOnly="True" Width="100%"></asp:textbox>
						<asp:button id="btnAttivaContatore" Text="Attiva il contatore" Runat="server" Visible="False"></asp:button>
						<asp:label id="lblattcont" Runat="server" Visible="False"></asp:label>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td colspan="6">
						<div style="BORDER-BOTTOM: darkblue 1px solid; BORDER-LEFT: darkblue 1px solid; BORDER-TOP: darkblue 1px solid; BORDER-RIGHT: darkblue 1px solid">
							<table cellSpacing="0" cellPadding="1" width="100%" align="center" bgColor="white" border="0">
								<tr>
									<td class="Input_Label" colspan="2" height="20">
										<strong>ANAGRAFICA INTESTATARIO</strong>&nbsp; <A onclick="ApriAnagrafica(document.getElementById('miointestatario);" href="javascript: void(0)">
										</A>&nbsp;
										<asp:imagebutton id="btnApriIntestatario" runat="server" ImageUrl="../../Images/Bottoni/Listasel.png"
											ToolTip="Ricerca Anagrafica tributi"></asp:imagebutton>
										<asp:imagebutton id="LnkAnagAnater" runat="server" ImageUrl="../../Images/Bottoni/Listasel.png" ToolTip="Ricerca Anagrafica da Anater"
											CausesValidation="False" imagealign="Bottom"></asp:imagebutton>&nbsp; <A onclick="PulisciAnagraficaIntestatario();" href="javascript: void(0)">
											<IMG alt="Pulisci i dati Intestatario" src="../../Images/Bottoni/cancel.png" align="absMiddle"
												border="0"> </A>
									</td>
								</tr>
								<tr>
									<td class="DettagliContribuente" width="160">Cognome/Rag. Sociale</td>
									<td class="DettagliContribuente">
										<asp:textbox id="txtCognomeIntestatario" Width="100%" runat="server" Cssclass="DettagliContribuente"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td class="DettagliContribuente" width="160">Data di nascita</td>
									<td class="DettagliContribuente">
										<asp:textbox id="txtCodiceFiscaleIntestatario" runat="server" Cssclass="DettagliContribuente"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td class="DettagliContribuente">Residente in</td>
									<td class="DettagliContribuente">
										<asp:textbox id="txtViaIntestatario" runat="server" Width="100%" Cssclass="DettagliContribuente"></asp:textbox>
									</td>
								</tr>
							</table>
						</div>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td colspan="6">
						<div style="BORDER-BOTTOM: darkblue 1px solid; BORDER-LEFT: darkblue 1px solid; BORDER-TOP: darkblue 1px solid; BORDER-RIGHT: darkblue 1px solid">
							<table cellSpacing="0" cellPadding="1" width="100%" align="center" bgColor="white" border="0">
								<tr>
									<td class="Input_Label" colspan="4" height="20">
										<strong>ANAGRAFICA UTENTE</strong>&nbsp; <A onclick="ApriAnagrafica(document.getElementById('mioutente);" href="javascript: void(0)">
										</A>&nbsp;
										<asp:imagebutton id="btnApriUtente" runat="server" ImageUrl="../../Images/Bottoni/Listasel.png" ToolTip="Ricerca Anagrafica da tributi"></asp:imagebutton>
										<asp:imagebutton id="LnkAnagrAnatUtente" runat="server" ImageUrl="../../Images/Bottoni/Listasel.png"
											ToolTip="Ricerca Anagrafica da Anater" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>&nbsp;
										<A onclick="PulisciAnagraficaUtente();" href="javascript: void(0)"><IMG alt="Pulisce i dati Utente" src="../../Images/Bottoni/cancel.png" align="absMiddle"
												border="0"> </A>
									</td>
								</tr>
								<tr>
									<td class="DettagliContribuente" width="160">Cognome/Rag. Sociale</td>
									<td class="DettagliContribuente" colspan="3">
										<asp:textbox id="txtCognomeUtente" Width="100%" runat="server" Cssclass="DettagliContribuente"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td class="DettagliContribuente" width="160">Data di nascita</td>
									<td class="DettagliContribuente">
										<asp:textbox id="txtCodiceFiscaleUtente" runat="server" Cssclass="DettagliContribuente"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td class="DettagliContribuente">Residente in</td>
									<td class="DettagliContribuente" colspan="3">
										<asp:textbox id="txtViaUtente" runat="server" Cssclass="DettagliContribuente" Width="100%"></asp:textbox>
									</td>
								</tr>
							</table>
						</div>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="lstTabRow" colSpan="6">Dati del Contatore</td>
				</tr>
				<tr>
					<td class="Input_Label">Ente Appartenenza</td>
					<td class="Input_Label" colSpan="3">Ubicazione
						<asp:imagebutton id="LnkOpenStradario" runat="server" ToolTip="Ubicazione Immobile da Stradario."
							CausesValidation="False" imagealign="Bottom" ImageUrl="../../Images/Bottoni/Listasel.png"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="LnkPulisciStrada" runat="server" ToolTip="Pulisci i campi della Via" CausesValidation="False"
							imagealign="Bottom" ImageUrl="../../Images/Bottoni/cancel.png"></asp:imagebutton>
					</td>
					<td class="Input_Label">Civico</td>
					<td class="Input_Label">Esponente</td>
				</tr>
				<tr>
					<td>
						<asp:textbox id="txtEnteAppartenenza" runat="server" Cssclass="Input_Text" Width="200px" Enabled="False"
							OnChange="Azzera(getElementById('hdEnteAppartenenza,'');"></asp:textbox>
					</td>
					<td colSpan="3">
						<asp:TextBox ID="TxtVia" cssclass="Input_Text" Runat="server" Width="400px"></asp:TextBox>&nbsp;
						<asp:TextBox id="TxtCodVia" style="DISPLAY: none" Runat="server">-1</asp:TextBox>
					</td>
					<td>
						<asp:textbox id="txtCivico" runat="server" Cssclass="Input_Text" Width="100px" MaxLength="10"></asp:textbox>
					</td>
					<td>
						<asp:textbox id="txtEsponente" runat="server" Cssclass="Input_Text" Width="100px" MaxLength="20"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td class="lstTabRow" colSpan="6">Dati Catastali&nbsp;</td>
				<tr>
					<td colspan="6">
						<asp:label id="LblResRifCat" Cssclass="NormalBold" Runat="server">Non sono presenti riferimenti catastali</asp:label>
						<Grd:RibesGridView ID="GrdRifCat" runat="server" BorderStyle="None" 
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
								<asp:BoundField DataField="sInterno" HeaderText="Interno"></asp:BoundField>
								<asp:BoundField DataField="sPiano" HeaderText="Piano"></asp:BoundField>
								<asp:BoundField DataField="sSezione" HeaderText="Sezione"></asp:BoundField>
								<asp:BoundField DataField="sFoglio" HeaderText="Foglio"></asp:BoundField>
								<asp:BoundField DataField="sNumero" HeaderText="Numero"></asp:BoundField>
								<asp:TemplateField HeaderText="Subalterno">
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.nSubalterno")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="sEstensioneParticella" HeaderText="Est. Particella"></asp:BoundField>
								<asp:BoundField DataField="sIdTipoParticella" HeaderText="Tipo Particella"></asp:BoundField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
									<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.nSubalterno")) %>' alt=""></asp:ImageButton>
									</itemtemplate>
								</asp:TemplateField>
							</Columns>
							</Grd:RibesGridView>
					</td>
				</tr>
				<tr>
					<td colSpan="3">
						<asp:dropdownlist id="cboImpianto" runat="server" Cssclass="Input_Text" Visible="False"></asp:dropdownlist>
					</td>
					<td>
						<asp:dropdownlist id="cboGiro" runat="server" Cssclass="Input_Text" Visible="False"></asp:dropdownlist>
					</td>
					<td>
						<asp:textbox id="txtSequenza" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}" Visible="False"></asp:textbox>
					</td>
					<td colSpan="3">
						<asp:dropdownlist id="cboPosizione" runat="server" Cssclass="Input_Text" Visible="False"></asp:dropdownlist>
					</td>
					<td>
						<asp:textbox id="txtProgressivo" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}" Visible="False"></asp:textbox>
					</td>
					<td>
						<asp:textbox id="txtLatoStrada" runat="server" Cssclass="Input_Text" MaxLength="1" Visible="False"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:dropdownlist id="cboTipoContatore" runat="server" Cssclass="Input_Text" visible="False"></asp:dropdownlist>
					</td>
					<td>
						<asp:textbox id="txtNumeroCifreContatore" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}"
							MaxLength="1" visible="False"></asp:textbox>
					</td>
					<td></td>
					<td></td>
				</tr>
				<tr>
					<td class="Input_Label" colspan="2">Tipo Utenza <FONT class="NormalRed">*</FONT></td>
					<td class="Input_Label">Numero Utenze <FONT class="NormalRed">*</FONT></td>
					<td class="Input_Label"></td>
					<td>
						<asp:dropdownlist id="cboDiametroContatore" runat="server" Width="170px" Cssclass="Input_Text" Visible="False"></asp:dropdownlist>
					</td>
					<td colSpan="3">
						<asp:dropdownlist id="cboDiametroPresa" runat="server" Cssclass="Input_Text" Width="170px" Visible="False"></asp:dropdownlist>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<asp:dropdownlist id="cboTipoUtenze" runat="server" Cssclass="Input_Text" Width="350px"></asp:dropdownlist>
					</td>
					<td>
						<asp:textbox id="txtNumeroUtenze" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}else{AssegnaQuoteAgevolate();};"></asp:textbox>
					</td>
					<td>
						<asp:CheckBox ID="chkEsenteAcqua" Text="Esente Acqua Potabile" Runat="server" cssclass="Input_Label" BorderStyle="None"></asp:CheckBox>
					</td>
					<td colspan="2">
						<asp:checkbox id="chkEsenteAcquaQF" Runat="server" cssclass="Input_Label" Text="Esente Quota Fissa Acqua Potabile"
							BorderStyle="None" width="300px"></asp:checkbox>
					</td>
				</tr>
				<tr>
					<td class="Input_Label">Fognatura</td>
					<td class="Input_Label"></td>
					<td class="Input_Label"></td>
					<td class="Input_Label">Depurazione</td>
					<td class="Input_Label"></td>
					<td class="Input_Label"></td>
				</tr>
				<tr>
					<td>
						<asp:dropdownlist id="cboFognatura" runat="server" Cssclass="Input_Text" Width="200px"></asp:dropdownlist>
					</td>
					<td>
						<asp:checkbox id="chkEsenteFognatura" onclick="DisabilitaFognatura()" runat="server" Text="Esente Fognatura" Cssclass="Input_Label"
							BorderStyle="None"></asp:checkbox>
					</td>
					<td>
						<asp:CheckBox ID="chkEsenteDepQF" Text="Esente Quota Fissa Fognatura" Runat="server" cssclass="Input_Label" BorderStyle="None"></asp:CheckBox>
					</td>
					<td>
						<asp:dropdownlist id="cboDepurazione" runat="server" Cssclass="Input_Text" Width="170px"></asp:dropdownlist>
					</td>
					<td>
						<asp:checkbox id="chkEsenteDepurazione" onclick="DisabilitaDepurazione()" runat="server" Text="Esente Depurazione"
							Cssclass="Input_Label" BorderStyle="None"></asp:checkbox>
					</td>
					<td>
						<asp:CheckBox ID="chkEsenteFogQF" Text="Esente Quota Fissa Depurazione" Runat="server" cssclass="Input_Label" BorderStyle="None"></asp:CheckBox>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="Input_Label">Data Attivazione</td>
					<td class="Input_Label">Data Sospensione</td>
					<td class="Input_Label">Data Cessazione</td>
					<td class="Input_Label" colSpan="2"></td>
				</tr>
				<tr>
					<td>
						<asp:textbox id="txtDataAttivazione" runat="server" Cssclass="Input_Text" ReadOnly="True"></asp:textbox>
					</td>
					<td>
						<asp:textbox id="txtDataSospsensioneUtenza" runat="server" Cssclass="Input_Text" ReadOnly="True"></asp:textbox>
					</td>
					<td>
						<asp:textbox id="txtDataCessazione" runat="server" Cssclass="Input_Text" ReadOnly="True"></asp:textbox>
					</td>
					<td colSpan="2"></td>
				</tr>
				<tr>
					<td>
						<label class="Input_Label">Richiesta sub contatore</label>&nbsp;
						<asp:checkbox id="chkRichiestaSub" Text="" Runat="server" BorderStyle="None" cssclass="Input_Text"></asp:checkbox>
					</td>
					<td colSpan="4">
						<div class="Input_Label">Note sub-contatore</div>
						<asp:textbox id="txtNoteSub" onkeyup="controlla(this,formRicercaAnagrafica,500);" runat="server"
							Height="30" Cssclass="Input_Text" Width="554px" TextMode="MultiLine" onkeydowb="controlla(this,formRicercaAnagrafica,500);"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="lstTabRow" colSpan="6">Dati Agenzia Entrate</td>
				</tr>
				<tr>
					<td class="Input_Label" colspan="3">Titolare Utenza</td>
					<td class="Input_Label" colspan="3">Tipologia Utenza</td>
				</tr>
				<tr>
					<td colspan="3">
						<asp:DropDownList id="ddlTitOccupazione" Runat="server" Cssclass="Input_Text" Width="500px"></asp:DropDownList>
					</td>
					<td colspan="3">
						<asp:DropDownList id="ddlTipoUtenza" Runat="server" Cssclass="Input_Text" Width="500px"></asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td class="Input_Label" colspan="3">Tipo Unita'</td>
					<td class="Input_Label" colspan="3">Assenza Dati Catastali</td>
				</tr>
				<tr>
					<td colspan="3">
						<asp:DropDownList id="ddlTipoUnita" Runat="server" Cssclass="Input_Text" Width="500px"></asp:DropDownList>
					</td>
					<td colspan="3">
						<asp:DropDownList id="ddlAssenzaDatiCat" Runat="server" Cssclass="Input_Text" Width="500px"></asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td class="lstTabRow" colSpan="6">Dati del Preventivo</td>
				</tr>
				<tr>
					<td class="Input_Label">Spesa preventivo</td>
					<td class="Input_Label" colSpan="4">Diritti di segreteria</td>
				</tr>
				<tr>
					<td>
						<asp:textbox id="txtspesaprev" runat="server" CssClass="Input_Number_Generali" MaxLength="50"></asp:textbox>
					</td>
					<td>
						<asp:textbox id="txtdirittisegr" runat="server" CssClass="Input_Number_Generali" MaxLength="50"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td></td>
					<td colSpan="3">
						<asp:dropdownlist id="cboMinimi" runat="server" Width="444px" Cssclass="Input_Text" Visible="False"></asp:dropdownlist>
					</td>
				</tr>
				<tr>
					<td style="HEIGHT: 18px">
						<asp:dropdownlist id="cboStatoContatore" runat="server" Cssclass="Input_Text" Width="100px" Visible="False">
							<asp:ListItem Value="ATT">Attivo</asp:ListItem>
							<asp:ListItem Value="RIM">Rimosso</asp:ListItem>
						</asp:dropdownlist>
					</td>
				</tr>
				<tr>
					<td class="lstTabRow" colSpan="6">Note Contratto</td>
				</tr>
				<tr>
					<td colSpan="6">
						<asp:textbox id="txtNote" onkeydown="controlla(this,formRicercaAnagrafica,500);" onkeyup="controlla(this,formRicercaAnagrafica,500);"
							runat="server" Height="80" Cssclass="Input_Text" Width="764px" TextMode="MultiLine"></asp:textbox>
					</td>
				</tr>
			</table>
			<asp:textbox id="miointestatario" style="DISPLAY: none" Runat="server"></asp:textbox>
			<asp:textbox id="mioutente" style="DISPLAY: none" Runat="server"></asp:textbox>
			<asp:textbox id="TxtDataCessazioneRibaltata" style="DISPLAY: none" CssClass="Input_Text" Runat="server"></asp:textbox>
			<input id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza"> <input id="hdCodiceVia" type="hidden" value="-1" name="hdCodiceVia">
			<input id="hdCodAnagrafeContatore" type="hidden" name="hdCodAnagrafeContatore"> <input id="hdCodAnagrafeIntestatario" type="hidden" name="hdCodAnagrafeIntestatario">
			<input id="hdCodAnagrafeUtente" type="hidden" name="hdCodAnagrafeUtente" runat="server">
			<input id="hdCodContratto" type="hidden" name="hdCodContratto"> <input id="hdCodContatore" type="hidden" name="hdCodContatore">
			<input id="hdConsumoMinimo" type="hidden" name="hdConsumoMinimo"> <input id="hdDataSottoScrizione" type="hidden" name="hdDataSottoScrizione">
			<input id="hdTipoUtenzaContratto" type="hidden" name="hdTipoUtenzaContratto"> <input id="hdIdDiametroContatoreContratto" type="hidden" name="hdIdDiametroContatoreContratto">
			<input id="hdIdDiametroPresaContratto" type="hidden" name="hdIdDiametroPresaContratto">
			<input id="hdNumeroUtenzeContratto" type="hidden" name="hdNumeroUtenzeContratto">
			<input id="hdCodiceContratto" type="hidden" name="hdCodiceContratto"> <input id="hdVirtualIDContratto" type="hidden" name="hdVirtualIDContratto">
			<input id="hdFuocoTipoUtenza" type="hidden" value="-1" name="hdFuocoTipoUtenza">
			<input id="hdRiportaNumeroUtenze" type="hidden" name="hdRiportaNumeroUtenze"> <input id="paginacomandi" type="hidden" name="paginacomandi">
			<asp:button id="btnEvento2" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
			<asp:button id="bntCalcolaNumeroUtente" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
			<asp:button id="btnStampa" style="DISPLAY: none" runat="server" Text="QUESTO"></asp:button>
			<asp:button id="btnStampa2" style="DISPLAY: none" runat="server" Text="QUESTO2"></asp:button>
            <asp:HiddenField ID="hfIDContatore" runat="server" Value="-1" />
		    <!-- Gestione parametri di ricerca -->
			<input id="IDContratto" type="hidden" name="IDContratto" runat="server"/> <input id="hdCodVia" type="hidden" name="hdCodVia">
			<input id="hdIntestatario" type="hidden" name="hdIntestatario"> <input id="hdUtente" type="hidden" name="hdUtente">
			<input id="hdNumeroUtente" type="hidden" name="hdNumeroUtente"> 
			<input id="hdMatricola" runat="server" type="hidden" name="hdMatricola">
			<input id="hdAvviaRicerca" type="hidden" name="hdAvviaRicerca">
		</form>
	</body>
</HTML>
