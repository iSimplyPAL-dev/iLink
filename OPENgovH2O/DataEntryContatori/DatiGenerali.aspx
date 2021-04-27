<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DatiGenerali.aspx.vb" Inherits="OpenUtenze.DatiGenerali"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<HTML>
	<HEAD>
		<META content="text/html; charset=windows-1252" http-equiv="Content-Type">
		<meta name="vs_showGrid" content="False">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script type="text/javascript" src="../../_js/MyUtility.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" src="../../_js/NumbersOnly.js?newversion"></script>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript">
			function mioAlert(parametro)
			{
				alert(parametro);
			}
			
			function ApriModifica(parametro)
			{
				myleft=((screen.width)/2)-325;
				mytop=((screen.height)/2)-100;	
				window.open("FrameModificaDatoCatasto.aspx?IDCatasto="+parametro,"Catasto","width=650, height=200, toolbar=no,top="+mytop+",left="+myleft+", menubar=no");
			}
			
			function ApriNuovo(parametro)
			{
				myleft=((screen.width)/2)-325;
				mytop=((screen.height)/2)-100;	
				window.open("FrameInserisciDatoCatasto.aspx?IDContatore="+parametro+"&noinsert=1","Catasto","width=650, height=200, toolbar=no,top="+mytop+",left="+myleft+", menubar=no");
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
				GestAlert('a', 'warning', '', '', 'La ricerca non ha prodotto risultati !!!');
				return false;
			}

			function VerificaData(Data)
			{
				if (!IsBlank(Data.value ))
				{		
					if(!isDate(Data.value)) 
					{
					    alert("Inserire la Data  correttamente in formato: gg/mm/aaaa !");
					    Data.value = "";
						Setfocus(Data);
						return false;
					}
				}					
			}	
		//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
			function VerificaCampi()
			{	
				sMsg=""
				var iselGiro = document.getElementById('cboGiro').selectedIndex;
				var iselFognatura = document.getElementById('cboFognatura').selectedIndex;
				var iselDepurazione = document.getElementById('cboDepurazione').selectedIndex;
				var iselTipoContatore = document.getElementById('cboTipoContatore').selectedIndex;
				var iselTipoUtenze = document.getElementById('cboTipoUtenze').selectedIndex;
				//var iselUbicazione = document.getElementById('cboUbicazione.selectedIndex;
				
					//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
			
				//if(document.getElementById('hdCodAnagrafeIntestatario.value=='-1' || document.getElementById('hdCodAnagrafeUtente.value=='-1')
				if(document.getElementById('HDtxtCodIntestatario').value=='-1' || document.getElementById('HDTextCodUtente').value=='-1')
				{
					sMsg = sMsg + "[Anagrafica Intestatario/Utente mancante ]\n" ; 
				}
				else
				{
				document.getElementById('hdCodAnagrafeIntestatario').value=document.getElementById('HDtxtCodIntestatario').value;
				//document.getElementById('hdCodAnagrafeUtente').value=document.getElementById('HDTextCodUtente').value;
				} 
				if (IsBlank(document.getElementById('txtMatricola').value)) 
				{ 
					sMsg = sMsg + "[Matricola]\n" ; 
				}
				if (IsBlank(document.getElementById('txtNumeroUtente').value)) 
				{ 
					sMsg = sMsg + "[Numero Utente]\n" ; 
				}
				if (IsBlank(document.getElementById('txtNumeroUtenze').value) || document.getElementById('txtNumeroUtenze').value<1) 
				{ 
					sMsg = sMsg + "[Numero Utenze]\n" ; 
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
					if(document.getElementById('cboFognatura').value == -1)
					{
						sMsg = sMsg + "[Codice Fognatura]\n" ; 
					}
				}	
				if(document.getElementById('cboTipoUtenze').value==-1){
					sMsg=sMsg + "[Tipo Utenza]\n" ;
				}
				if(document.getElementById('chkEsenteDepurazione').checked==false)
				{
					if(document.getElementById('cboDepurazione').value == -1)
					{
						sMsg = sMsg + "[Codice Depurazione]\n" ; 
					}
				}
				if(document.getElementById('cboTipoContatore').value == -1)
					{
					sMsg = sMsg + "[Tipo Contatore]\n" ; 
				}
				if (IsBlank(document.getElementById('txtNumeroCifreContatore').value))	
				{
					sMsg=sMsg+"[Cifre Contatore]\n";
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
		//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

			function Salva(sProvenienza)
			{
			//alert("Attualmente disabilitato");
			//alert("INTESTATARIO: " + document.getElementById('hdCodAnagrafeIntestatario.value + "\nUTENTE: " + document.getElementById('hdCodAnagrafeUtente.value);
				if(document.getElementById('txtSostituito').value=="SOSTITUITO")
				{
					alert("Non e\' possibile modificare un contatore gia\' sostituito!");
					return;
				}
				if (sProvenienza!='AE')
				{
					if (confirm('Si vogliono salvare le modifiche apportate?'))
					{
						if(VerificaCampi())
						{
							if((document.getElementById('txtDataSostituzione').value!="") || (document.getElementById('txtDataCessazione').value!=""))
							{
								if (confirm('ATTENZIONE! E\' gia\' stata inserita la lettura finale?\nIl contatore verra\' chiuso, non sara\' quindi piu\' possibile inserire la lettura finale.\nPer una fatturazione corretta e\' obbligatorio inserire anche la lettura finale del sub-contatore.\nProseguire?'))
								{
									document.getElementById('btnEvento').click();					
								}
							}
							else
							{
								document.getElementById('btnEvento').click(); 
							}
						}
						else
						alert("Inserimento dati non corretto");
					}		
				}
				else
				{
					document.getElementById('btnEvento').click(); 
				}
			}
		
			//ALE CAO
			function Stampa()
			{
				if (confirm('Si vuole effettuare la stampa del contratto?'))
				{
					document.getElementById('btnStampa').click();
				}
			}
			
			function Stampa2()
			{
				if (confirm('Si vuole effettuare la stampa del preventivo?'))
				{
					document.getElementById('btnStampa2').click();
				}
			}
			//FINE ALE CAO
		
			function GetListaEnti(objFieldEnteAppartenenza,objFieldHidden,objForm) 
			{
				HIDDEN=objFieldHidden.name;
				FORM=objForm.name;
				ENTEAPPARTENENZA=objFieldEnteAppartenenza.name;
				/*WinPopUp=OpenPopup('OpenUtenze','/OpenUtenzeGC/Selezioni/FrameListaEnti.aspx?HIDDEN=' + HIDDEN + '&FORM=' +FORM +'&ENTEAPPARTENENZA='+ ENTEAPPARTENENZA,'Enti','770','500',0,0,'yes','no');*/
				WinPopUp = OpenPopup('OpenUtenze', '<%=Session("PATH_APPLICAZIONE")%>/Selezioni/FrameListaEnti.aspx?HIDDEN=' + HIDDEN + '&FORM=' + FORM + '&ENTEAPPARTENENZA=' + ENTEAPPARTENENZA, 'Enti', '770', '500', 0, 0, 'yes', 'no');
			}
			
			function GetStradario(objFieldHidden,objFieldUbicazione,CodComune,objForm) 
			{
				HIDDEN=objFieldHidden.name;
				FORM=objForm.name;
				UBICAZIONE=objFieldUbicazione.name;
				/*WinPopUp=OpenPopup('OpenUtenze','/OpenUtenzeGC/Selezioni/FrameListaSelezioni.aspx?SEARCHVIA=' + objFieldUbicazione.value+'&HIDDEN=' + HIDDEN + '&FORM=' +FORM +'&UBICAZIONE='+ UBICAZIONE +'&CODCOMUNE='+CodComune.value,'Stradario','770','500',0,0,'yes','no');*/
				WinPopUp=OpenPopup('OpenUtenze','<%=Session("PATH_APPLICAZIONE")%>/Selezioni/FrameListaSelezioni.aspx?SEARCHVIA=' + objFieldUbicazione.value+'&HIDDEN=' + HIDDEN + '&FORM=' +FORM +'&UBICAZIONE='+ UBICAZIONE +'&CODCOMUNE='+CodComune.value,'Stradario','770','500',0,0,'yes','no');
			}
				
			function ApriAnagrafica(codcontrib, sessionName)
		{ 
				cod_contribuente=codcontrib
				winWidth=980
				winHeight=680
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40
								
				PaginaPrec="DatiGenerali"
				PaginaComandi="../Anagrafica/ComandiInsertSaveAnagrafica.aspx"		
				WinPopUpRicercaAnagrafica=window.open("../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?COD_CONTRIBUENTE=" + cod_contribuente +"&sessionName=" + sessionName + "&paginacomandi="+PaginaComandi +"&paginaprec="+PaginaPrec+"&sOperazione=dettaglio","APRIANAGRAFE","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
			}		
		
			function ApriContratti()
			{ 
				if (! window.focus)return true;

				winWidth=980
				winHeight=300
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40

				if(document.getElementById('hdCodContratto').value=='0')
				{
					document.getElementById('hdCodContratto').value='-1';	
				}
				Parametri="?idcontratto="+document.getElementById('hdCodContratto').value +"&hdCodiceContratto="+document.getElementById('hdCodiceContratto').value+"&hdDataSottoScrizione="+document.getElementById('hdDataSottoScrizione').value+"&hdNumeroUtenzeContratto="+document.getElementById('hdNumeroUtenzeContratto').value+"&hdTipoUtenzaContratto="+document.getElementById('hdTipoUtenzaContratto').value+"&hdIdDiametroContatoreContratto="+document.getElementById('hdIdDiametroContatoreContratto').value+"&hdIdDiametroPresaContratto="+document.getElementById('hdIdDiametroPresaContratto').value	
				//WinPopUp=OpenPopup('OpenUtenze','<%=Session("PATH_APPLICAZIONE")%>/DataEntryContatori/FrameContratti.aspx'+Parametri,'FrameContratti','650','250',0,0,'no','no');
				WinPopUp=window.open("FrameContratti.aspx"+Parametri,"APRICONTRATTI","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
				return false;
			}
			
			function ApriLetture()
			{ 
				winWidth=980
				winHeight=680
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40
			
				if (! window.focus)return true;
				WinPopUp=window.open("../Letture/FrameLetture.aspx?IDCONTATORE="+ document.getElementById('hdCodContatore').value+"&PAG_PREC=2&IDLETTURA=0&VIEWLETTURE=1","APRILETTURE","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
				return false;
			}
			
			function Annulla()
			{
				if ('<%=Request.Item("sProvenienza")%>'!='AE')
				{
					parent.Comandi.location.href = 'ComandiRicercaContatori.aspx'
				    parent.Visualizza.location.href = 'RicercaContatori.aspx'
				}
				else
				{
				    parent.Visualizza.location.href = "../AgenziaEntrate/DatiMancanti/RicercaDatiMancanti.aspx";
				}
			}
			
		
			function PulisciAnagraficaIntestatario()
			{
				document.getElementById('txtCognomeIntestatario').value='';
				document.getElementById('txtCodiceFiscaleIntestatario').value='';
				document.getElementById('txtViaIntestatario').value='';	
				document.getElementById('hdCodAnagrafeIntestatario').value ='-1';
				document.getElementById('HDtxtCodIntestatario').value='-1';
			}
			
			function PulisciAnagraficaUtente()
			{
				document.getElementById('txtCognomeUtente').value='';
				document.getElementById('txtCodiceFiscaleUtente').value='';
				document.getElementById('txtViaUtente').value='';	
				//document.getElementById('hdCodAnagrafeUtente').value ='-1';
				document.getElementById('HDTextCodUtente').value ='-1';
			}
			
			function CalcolaNumeroUtente()
			{
				if ('<%=request.item("sProvenienza")%>'!='AE')
				{
					if (confirm('Si desidera calcolare un nuovo Numero Utente?'))
					{
						document.getElementById('bntCalcolaNumeroUtente').click(); 
						return true;
					}
					else
					{
						return false;
					}
				}
			}

			function ControllaFuoco()
			{
				if ('<%=request.item("sProvenienza")%>'!='AE')
				{
				    document.getElementById('txtNumeroUtente').focus();
					if(typeof(doFocus) != "undefined")
					{
							//document.getElementById('cboMinimi.focus();
					}
				}				
			}
		
			function AssegnaQuoteAgevolate()
			{
				if(document.getElementById('hdRiportaNumeroUtenze').value =='1')
				{
					document.getElementById('txtQuoteAgevolate').value=document.getElementById('txtNumeroUtenze').value
				}
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
				//alert(document.getElementById('TxtCodVia').value);
			}

			function ClearDatiVia()
			{
				document.getElementById('TxtVia').value='';
				document.getElementById('TxtCodVia').value='-1';
				//alert(document.getElementById('TxtCodVia').value);
				return false;
			}

			function ShowRicUIAnater(ViaContatore)
			{
				winWidth=800 
				winHeight=500 
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40 
				WinPopFamiglia=window.open('../RicercaAnater/FrmRicercaImmobile.aspx?Via='+document.getElementById('TxtVia').value,'','width='+winWidth+',height='+winHeight+', status=yes, toolbar=no,top='+mytop+',left='+myleft+',Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no') 
			}
		</script>
	</HEAD>
	<body class="Sfondo" onload="ControllaFuoco()" bottomMargin="0" leftMargin="0" topMargin="0">
		<form id="Form1" runat="server" method="post">
			<table border="0" cellSpacing="0" cellPadding="1" width="770" align="center">
				<tr>
					<td><asp:table id="tblSottoAttivita" runat="server" Height="28px" BorderWidth="0px" CellPadding="0"
							CellSpacing="0" CssClass="SottoAttivita_Label" Width="98%"></asp:table></td>
				</tr>
			</table>
			<asp:textbox style="DISPLAY: none" id="txtSostituito" Runat="server" name="txtSostituito"></asp:textbox><asp:label id="lblError" runat="server" Visible="False" Cssclass="NormalBold"></asp:label>
			<table width="770">
				<tr>
					<td colSpan="6">
						<div style="BORDER-BOTTOM: darkblue 1px solid; BORDER-LEFT: darkblue 1px solid; BORDER-TOP: darkblue 1px solid; BORDER-RIGHT: darkblue 1px solid">
							<table border="0" cellSpacing="0" cellPadding="1" width="100%" bgColor="white" align="center">
								<tr>
									<td class="Input_Label" height="20" colSpan="2"><strong>ANAGRAFICA INTESTATARIO</strong>&nbsp;
										<A onclick="ApriAnagrafica(document.getElementById('miointestatario);" href="javascript: void(0)">
										</A>&nbsp;
										<asp:imagebutton id="btnApriIntestatario" runat="server" ToolTip="Ricerca Anagrafica tributi" ImageUrl="../../Images/Bottoni/Listasel.png"></asp:imagebutton><asp:imagebutton id="LnkAnagAnater" runat="server" ToolTip="Ricerca Anagrafica da Anater" ImageUrl="../../Images/Bottoni/Listasel.png"
											imagealign="Bottom" CausesValidation="False"></asp:imagebutton>&nbsp; <A onclick="PulisciAnagraficaIntestatario();" href="javascript: void(0)">
											<IMG border="0" alt="Pulisci i dati Intestatario" align="absMiddle" src="../../Images/Bottoni/cancel.png">
										</A>
									</td>
								</tr>
								<tr>
									<td class="DettagliContribuente" width="160">Cognome/Rag. Sociale</td>
									<td class="DettagliContribuente"><asp:textbox id="txtCognomeIntestatario" runat="server" Width="100%" Cssclass="DettagliContribuente"></asp:textbox></td>
								</tr>
								<tr>
									<td class="DettagliContribuente" width="160">Data di nascita</td>
									<td class="DettagliContribuente"><asp:textbox id="txtCodiceFiscaleIntestatario" runat="server" Cssclass="DettagliContribuente"></asp:textbox></td>
								</tr>
								<tr>
									<td class="DettagliContribuente">Residente in</td>
									<td class="DettagliContribuente"><asp:textbox id="txtViaIntestatario" runat="server" Width="100%" Cssclass="DettagliContribuente"></asp:textbox></td>
								</tr>
							</table>
						</div>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td colSpan="6">
						<div style="BORDER-BOTTOM: darkblue 1px solid; BORDER-LEFT: darkblue 1px solid; BORDER-TOP: darkblue 1px solid; BORDER-RIGHT: darkblue 1px solid">
							<table border="0" cellSpacing="0" cellPadding="1" width="100%" bgColor="white" align="center">
								<tr>
									<td class="Input_Label" height="20" colSpan="4"><strong>ANAGRAFICA UTENTE</strong>&nbsp;
										<A onclick="ApriAnagrafica(document.getElementById('mioutente);" href="javascript: void(0)">
										</A>&nbsp;
										<asp:imagebutton id="btnApriUtente" runat="server" ToolTip="Ricerca Anagrafica da tributi" ImageUrl="../../Images/Bottoni/Listasel.png"></asp:imagebutton><asp:imagebutton id="LnkAnagrAnatUtente" runat="server" ToolTip="Ricerca Anagrafica da Anater" ImageUrl="../../Images/Bottoni/Listasel.png"
											imagealign="Bottom" CausesValidation="False"></asp:imagebutton>&nbsp; <A onclick="PulisciAnagraficaUtente();" href="javascript: void(0)">
											<IMG border="0" alt="Pulisce i dati Utente" align="absMiddle" src="../../Images/Bottoni/cancel.png">
										</A>
									</td>
								</tr>
								<tr>
									<td class="DettagliContribuente" width="160">Cognome/Rag. Sociale</td>
									<td class="DettagliContribuente" colSpan="3"><asp:textbox id="txtCognomeUtente" runat="server" Width="100%" Cssclass="DettagliContribuente"></asp:textbox></td>
								</tr>
								<tr>
									<td class="DettagliContribuente" width="160">Data di nascita</td>
									<td class="DettagliContribuente"><asp:textbox id="txtCodiceFiscaleUtente" runat="server" Cssclass="DettagliContribuente"></asp:textbox></td>
								</tr>
								<tr>
									<td class="DettagliContribuente">Residente in</td>
									<td class="DettagliContribuente" colSpan="3"><asp:textbox id="txtViaUtente" runat="server" Width="100%" Cssclass="DettagliContribuente"></asp:textbox></td>
								</tr>
							</table>
						</div>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="Input_Label">N.Utente<FONT class="NormalRed">*</FONT> &nbsp; <input id="btnCodiceFiscale" class="Bottone BottoneNUtente" title="Permette di assegnare un Numero Utente se mancante"
							onclick="CalcolaNumeroUtente()" type="button" name="btnCodiceFiscale">
					</td>
					<td style="WIDTH: 302px" class="Input_Label">Matricola <FONT class="NormalRed">*</FONT></td>
					<td class="Input_Label">Ente Appartenenza <FONT class="NormalRed">*</FONT> 
						<!--<A onclick="GetListaEnti(document.getElementById('txtEnteAppartenenza,document.getElementById('hdEnteAppartenenza,document.formRicercaAnagrafica);" href="javascript: void(0)"><IMG alt="Lista Entii" src="../images/lista.png" align="absMiddle" border="0"></A>--></td>
				</tr>
				<tr>
					<td><asp:textbox id="txtNumeroUtente" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}" MaxLength="50"></asp:textbox></td>
					<td style="WIDTH: 302px"><asp:textbox id="txtMatricola" runat="server" Width="160px" Cssclass="Input_Text" MaxLength="20"></asp:textbox></td>
					<td colSpan="3"><asp:textbox id="txtEnteAppartenenza" runat="server" Width="250px" Cssclass="Input_Text" Enabled="False"
							OnChange="Azzera(getElementById('hdEnteAppartenenza'),'');"></asp:textbox></td>
				</tr>
				<tr>
					<td style="WIDTH: 529px" class="Input_Label" width="529" colSpan="2">Ubicazione
						<asp:imagebutton id="LnkOpenStradario" runat="server" ToolTip="Ubicazione Immobile da Stradario."
							ImageUrl="../../Images/Bottoni/Listasel.png" imagealign="Bottom" CausesValidation="False"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="LnkPulisciStrada" runat="server" ToolTip="Pulisci i campi della Via" ImageUrl="../../Images/Bottoni/cancel.png"
							imagealign="Bottom" CausesValidation="False"></asp:imagebutton></td>
					<td class="Input_Label" width="25%">Civico</td>
					<td class="Input_Label" width="25%" colSpan="3">Esponente</td>
				</tr>
				<tr>
					<td style="WIDTH: 529px" colSpan="2">
						<!--<asp:dropdownlist id="cboUbicazione" runat="server" Width="300px" Cssclass="Input_Text"></asp:dropdownlist>-->
						<asp:textbox id="TxtVia" Width="400px" Runat="server" ReadOnly="True" cssclass="Input_Text"></asp:textbox>&nbsp;
						<asp:textbox style="DISPLAY: none" id="TxtCodVia" Runat="server">-1</asp:textbox></td>
					<td><asp:textbox id="txtCivico" runat="server" Width="100px" Cssclass="Input_Text" MaxLength="10"></asp:textbox></td>
					<td colSpan="3"><asp:textbox id="txtEsponente" runat="server" Width="100px" Cssclass="Input_Text" MaxLength="20"></asp:textbox></td>
				</tr>
				<!--Dati Catastali-->
				<tr>
					<td colSpan="4">
						<table>
							<tr>
								<td class="lstTabRow">Dati Catastali&nbsp; 
									<!--<input class="botNew" type="button" onclick="ApriNuovo(<%=CInt(Request.Params("IDCONTATORE"))%>);">--><asp:imagebutton id="LnkNewUIManuale" runat="server" ToolTip="Inserimento manuale Dati Catastali"
										ImageUrl="../../Images/Bottoni/Listasel.png" imagealign="Bottom" CausesValidation="False"></asp:imagebutton>&nbsp;
									<asp:imagebutton id="LnkNewUIAnater" runat="server" ToolTip="Dati Catastali da Anater" ImageUrl="../../Images/Bottoni/Listasel.png"
										imagealign="Bottom" CausesValidation="False"></asp:imagebutton>&nbsp;
								</td>
							</tr>
							<tr>
								<td>
									<iframe style="WIDTH: 770px; HEIGHT: 150px" id="loadGrid" src="../../aspVuota.aspx" frameBorder="0" scrolling="no"></iframe>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<!--Dati del contatore-->
				<tr>
					<td class="lstTabRow" colSpan="4">Dati del Contatore</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtspesaprev" onkeypress="NumbersOnlyLAST(event,true,false,2)" runat="server"
							CssClass="Input_Number_Generali" Visible="False" MaxLength="50"></asp:textbox></td>
					<td style="WIDTH: 302px"><asp:textbox id="txtdirittisegr" onkeypress="NumbersOnlyLAST(event,true,false,2)" runat="server"
							CssClass="Input_Number_Generali" Visible="False" MaxLength="50"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label" width="25%">Cod. Impianto</td>
					<td class="Input_Label" width="25%">Giro</td>
					<td class="Input_Label" width="25%">Sequenza</td>
					<td style="WIDTH: 302px" class="Input_Label" width="302">&nbsp;</td>
				</tr>
				<tr>
					<td><asp:dropdownlist id="cboImpianto" runat="server" Width="170px" Cssclass="Input_Text"></asp:dropdownlist></td>
					<td><asp:dropdownlist id="cboGiro" runat="server" Width="200px" Cssclass="Input_Text"></asp:dropdownlist></td>
					<td><asp:textbox id="txtSequenza" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}" Width="52px"></asp:textbox></td>
					<td style="WIDTH: 302px"><asp:textbox style="DISPLAY: none" id="txtpiano" runat="server" CssClass="Input_Number_Generali"
							MaxLength="50"></asp:textbox></td>
				</tr>
				<tr>
					<td style="WIDTH: 529px" class="Input_Label" width="529">Posizione</td>
					<td class="Input_Label" width="25%">Progressivo</td>
					<td class="Input_Label" width="25%" noWrap>Lato Strada</td>
				</tr>
				<tr>
					<td style="WIDTH: 529px"><asp:dropdownlist id="cboPosizione" runat="server" Width="170px" Cssclass="Input_Text"></asp:dropdownlist></td>
					<td><asp:textbox id="txtProgressivo" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}"></asp:textbox></td>
					<td><asp:textbox id="txtLatoStrada" runat="server" Width="40px" Cssclass="Input_Text" MaxLength="1"></asp:textbox></td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="Input_Label" width="25%">Tipo Contatore <FONT class="NormalRed">*</FONT></td>
					<td style="WIDTH: 302px" class="Input_Label" width="302">Cifre Contatore<FONT class="NormalRed">*</FONT></td>
					<td class="Input_Label" width="25%">Diametro Contatore</td>
					<td class="Input_Label" width="25%" colSpan="3">Diametro Presa</td>
					<!--<td class="Input_Label" width="25%">Cont. Prec</td>
					<td class="Input_Label" width="25%" colSpan="2">Cont. Succ.</td>-->
				</tr>
				<tr>
					<td><asp:dropdownlist id="cboTipoContatore" runat="server" Width="170px" Cssclass="Input_Text"></asp:dropdownlist></td>
					<td style="WIDTH: 302px"><asp:textbox id="txtNumeroCifreContatore" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}" MaxLength="1"></asp:textbox></td>
					<td><asp:dropdownlist id="cboDiametroContatore" runat="server" Width="170px" Cssclass="Input_Text"></asp:dropdownlist></td>
					<td><asp:dropdownlist id="cboDiametroPresa" runat="server" Width="170px" Cssclass="Input_Text"></asp:dropdownlist></td>
					<td><asp:dropdownlist style="DISPLAY: none" id="cboMinimi" runat="server" Width="444px" Cssclass="Input_Text"></asp:dropdownlist></td>
					<td><asp:textbox style="DISPLAY: none" id="TxtMatricolaContatorePrecedente" runat="server" Cssclass="Input_Text"
							readonly="true"></asp:textbox><asp:textbox style="DISPLAY: none" id="txtContatorePrecedente" runat="server" Cssclass="Input_Text"></asp:textbox></td>
					<td><asp:textbox style="DISPLAY: none" id="TxtMatricolaContatoreSuccessivo" runat="server" Cssclass="Input_Text"
							readonly="true"></asp:textbox><asp:textbox style="DISPLAY: none" id="txtContatoreSuccessivo" runat="server" Cssclass="Input_Text"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label" width="25%">Tipo Utenza <FONT class="NormalRed">*</FONT></td>
					<td class="Input_Label" width="25%">Numero Utenze <FONT class="NormalRed">*</FONT></td>
					<td class="Input_Label">Acqua Potabile</td>
				</tr>
				<tr>
					<td><asp:dropdownlist id="cboTipoUtenze" runat="server" Width="250px" Cssclass="Input_Text"></asp:dropdownlist></td>
					<td><asp:textbox id="txtNumeroUtenze" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}else{AssegnaQuoteAgevolate();}"></asp:textbox></td>
					<td colspan="2">
						<asp:checkbox id="chkEsenteAcqua" Runat="server" cssclass="Input_Label" Text="Esente" BorderStyle="None"></asp:checkbox>
						<asp:checkbox id="chkEsenteAcquaQF" Runat="server" cssclass="Input_Label" Text="Esente Quota Fissa"
							BorderStyle="None" width="300px"></asp:checkbox>
					</td>
				</tr>
				<tr>
					<td colspan="2" class="Input_Label">Fognatura <FONT class="NormalRed">*</FONT></td>
					<td colspan="2" class="Input_Label">Depurazione <FONT class="NormalRed">*</FONT></td>
				</tr>
				<tr>
					<td colspan="2">
						<asp:dropdownlist id="cboFognatura" runat="server" Width="170px" Cssclass="Input_Text"></asp:dropdownlist>&nbsp;
						<asp:checkbox id="chkEsenteFognatura" onclick="DisabilitaFognatura()" runat="server" Cssclass="Input_Label"
							Text="Esente" BorderStyle="None"></asp:checkbox>
						<asp:checkbox id="chkEsenteFogQF" Runat="server" cssclass="Input_Label" Text="Esente Quota Fissa"
							BorderStyle="None"></asp:checkbox>
					</td>
					<td colspan="2">
						<asp:dropdownlist id="cboDepurazione" runat="server" Width="170px" Cssclass="Input_Text"></asp:dropdownlist>&nbsp;
						<asp:checkbox id="chkEsenteDepurazione" onclick="DisabilitaDepurazione()" runat="server" Cssclass="Input_Label"
							Text="Esente" BorderStyle="None"></asp:checkbox>
						<asp:checkbox id="chkEsenteDepQF" Runat="server" cssclass="Input_Label" Text="Esente Quota Fissa"
							BorderStyle="None"></asp:checkbox>
					</td>
				</tr>
				<tr>
					<td><br>
					</td>
				</tr>
				<tr>
					<td class="Input_Label" width="25%">Stato Contatore</td>
				</tr>
				<tr>
					<td style="HEIGHT: 18px"><asp:dropdownlist id="cboStatoContatore" runat="server" Width="100px" Cssclass="Input_Text" Enabled="False">
							<asp:ListItem Value="-1">...</asp:ListItem>
							<asp:ListItem Value="ATT">Attivo</asp:ListItem>
							<asp:ListItem Value="RIM">Rimosso</asp:ListItem>
						</asp:dropdownlist></td>
				</tr>
				<tr>
					<td class="Input_Label">Quote Agevolate</td>
					<td class="Input_Label" width="25%">Codice Fabbricante</td>
					<td></td>
					<td class="Input_Label" width="25%">Data Sospensione Utenza</td>
				</tr>
				<tr>
					<td><asp:checkbox style="DISPLAY: none" id="chkIgnoraMora" runat="server" Width="20px" Cssclass="Input_Text"
							Text=" "></asp:checkbox></td>
				</tr>
				<tr>
					<td><asp:textbox id="txtQuoteAgevolate" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}" MaxLength="5"></asp:textbox></td>
					<td><asp:textbox id="txtCodiceFabbricatore" runat="server" Cssclass="Input_Text" MaxLength="3"></asp:textbox></td>
					<td><asp:checkbox id="chkUtenteSospeso" runat="server" Cssclass="Input_Label" Text="Utente Sospeso"
							BorderStyle="None"></asp:checkbox></td>
					<td><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this)" id="txtDataSospsensioneUtenza"
							onfocus="txtDateGotfocus(this)" runat="server" Width="112px" Cssclass="Input_Text" MaxLength="10"
							TextMode="SingleLine"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label" width="25%">Data Attivazione</td>
					<td style="WIDTH: 302px" class="Input_Label" width="302">Data Sostituzione</td>
					<td class="Input_Label" width="25%">Data Rim.Temp.</td>
					<td class="Input_Label" width="25%" colSpan="2">Data Cessazione</td>
				</tr>
				<tr>
					<td><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this)" id="txtDataAttivazione" onfocus="txtDateGotfocus(this)"
							runat="server" Cssclass="Input_Text" MaxLength="10" readonly="True" TextMode="SingleLine"></asp:textbox></td>
					<td style="WIDTH: 302px"><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this)" id="txtDataSostituzione" onfocus="txtDateGotfocus(this)"
							runat="server" Cssclass="Input_Text" MaxLength="10" ReadOnly="True" TextMode="SingleLine"></asp:textbox></td>
					<td><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this)" id="txtDataRimTemp" onfocus="txtDateGotfocus(this)"
							runat="server" Cssclass="Input_Text" MaxLength="10" TextMode="SingleLine"></asp:textbox></td>
					<td colSpan="2"><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this)" id="txtDataCessazione" onfocus="txtDateGotfocus(this)"
							runat="server" Cssclass="Input_Text" MaxLength="10" TextMode="SingleLine"></asp:textbox></td>
				</tr>
				<tr>
					<td class="lstTabRow" width="25%" colSpan="5">Sub-Contatori Associati</td>
				</tr>
				<tr>
					<td colSpan="5">
                        <Grd:RibesGridView ID="GrdSubContatori" runat="server" BorderStyle="None" 
							BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
							AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
							ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnRowCommand="GrdRowCommand">
							<PagerSettings Position="Bottom"></PagerSettings>
							<PagerStyle CssClass="CartListFooter" />
							<RowStyle CssClass="CartListItem"></RowStyle>
							<HeaderStyle CssClass="CartListHead"></HeaderStyle>
							<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
								<asp:BoundField DataField="sMATRICOLA" HeaderText="Matricola"></asp:BoundField>
								<asp:BoundField DataField="sCognomeIntestatario" HeaderText="Cognome"></asp:BoundField>
								<asp:BoundField DataField="sNomeIntestatario" HeaderText="Nome"></asp:BoundField>
								<asp:BoundField DataField="sPeriodo" HeaderText="Dal - Al"></asp:BoundField>
								<asp:BoundField DataField="subicazione" HeaderText="Ubicazione"></asp:BoundField>
								<asp:TemplateField HeaderText="Del">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" CommandArgument='<%# Eval("sMATRICOLA")%>'></asp:ImageButton>
									    <asp:HiddenField runat="server" ID="hfIDsubCONTATORE" Value='<%# Eval("IDsubCONTATORE") %>' />
                                    </ItemTemplate>
								</asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>
                    </td>
				</tr>
				<!--Dati Agenzia Entrate-->
				<tr>
					<td class="lstTabRow" colSpan="4">Dati Agenzia Entrate</td>
				</tr>
				<tr>
					<td class="Input_Label" width="50%" colSpan="2">Titolare Utenza</td>
					<td class="Input_Label" width="50%" colSpan="2">Tipologia Utenza</td>
				</tr>
				<tr>
					<td colSpan="2"><asp:dropdownlist id="ddlTitOccupazione" runat="server" Width="340px" Cssclass="Input_Text"></asp:dropdownlist></td>
					<td colSpan="2"><asp:dropdownlist id="ddlTipoUtenza" runat="server" Width="380px" Cssclass="Input_Text"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td class="Input_Label" width="50%" colSpan="2">Tipo Unita'</td>
					<td class="Input_Label" width="50%" colSpan="2">Assenza dati catastali</td>
				</tr>
				<tr>
					<td colSpan="2"><asp:dropdownlist id="ddlTipoUnita" runat="server" Width="245px" Cssclass="Input_Text"></asp:dropdownlist></td>
					<td colSpan="2"><asp:dropdownlist id="ddlAssenzaDatiCat" runat="server" Width="349px" Cssclass="Input_Text"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td class="lstTabRow" colSpan="5">Note Contatore</td>
				</tr>
				<tr>
					<td colSpan="5"><asp:textbox onkeydown="controlla(this,formRicercaAnagrafica,2000)" id="txtNote" onkeyup="controlla(this,formRicercaAnagrafica,2000)"
							runat="server" Height="80" Width="764px" Cssclass="Input_Text" TextMode="MultiLine"></asp:textbox></td>
				</tr>
				<asp:textbox style="DISPLAY: none" id="txtCodFisUte" Runat="server"></asp:textbox></table>
			<input id="paginacomandi" type="hidden" name="paginacomandi">
			<asp:textbox id="TxtDataAttivazioneRibaltata" style="DISPLAY: none" CssClass="Input_Text" Runat="server"></asp:textbox>
			<asp:textbox id="TxtDataSostituzioneRibaltata" style="DISPLAY: none" CssClass="Input_Text" Runat="server"></asp:textbox>
			<asp:textbox style="DISPLAY: none" id="hdEnteAppartenenza" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdCodiceVia" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdCodAnagrafeContatore" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdCodAnagrafeIntestatario" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdCodAnagrafeUtente" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdCodContratto" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdCodContatore" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdConsumoMinimo" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdDataSottoScrizione" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdTipoUtenzaContratto" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdIdDiametroContatoreContratto" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdIdDiametroPresaContratto" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdNumeroUtenzeContratto" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdCodiceContratto" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdVirtualIDContratto" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdFuocoTipoUtenza" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="hdRiportaNumeroUtenze" runat="server"></asp:textbox><asp:button style="DISPLAY: none" id="btnEvento" runat="server" Text="Button"></asp:button><asp:button style="DISPLAY: none" id="bntCalcolaNumeroUtente" runat="server" Text="Button"></asp:button><asp:button style="DISPLAY: none" id="btnStampa" runat="server" Text="QUESTO"></asp:button><asp:button style="DISPLAY: none" id="btnStampa2" runat="server" Text="QUESTO2"></asp:button><asp:button style="DISPLAY: none" id="CmdRibaltaUIAnater" Runat="server"></asp:button>
			<!-- inseriti il 07/02/07 per gestione CMCG --><asp:textbox style="DISPLAY: none" id="HDtxtCodIntestatario" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="HDTextCodUtente" runat="server"></asp:textbox><asp:button style="DISPLAY: none" id="btnRibalta" runat="server" Text="Button"></asp:button><asp:button style="DISPLAY: none" id="btnRibaltaAnagAnater" Runat="server"></asp:button><asp:textbox style="DISPLAY: none" id="txtidContatore" runat="server"></asp:textbox><asp:textbox style="DISPLAY: none" id="UpdPrimaLettura" Runat="server"></asp:textbox>
			<!-- inseriti il 07/02/07 per gestione CMCG --><asp:button style="DISPLAY: none" id="btnSalvaContatore" runat="server" Text="Button"></asp:button><asp:button style="DISPLAY: none" id="CmdRibaltaSubContatori" runat="server" Text="Button"></asp:button><asp:button style="DISPLAY: none" id="CmdDeleteSubContatori" runat="server" Text="Button"></asp:button>
			<asp:button id="CmdGIS" style="DISPLAY: none" Runat="server"></asp:button>
		    <!-- Gestione parametri di ricerca -->
			<input id="IDContatore" type="hidden" name="IDContatore" runat="server"> 
            <input id="hdCodVia" type="hidden" name="hdCodVia">
			<input id="hdIntestatario" type="hidden" name="hdIntestatario"> 
            <input id="hdUtente" type="hidden" name="hdUtente">
			<input id="hdNumeroUtente" type="hidden" name="hdNumeroUtente"> 
            <input id="hdMatricola" type="hidden" name="hdMatricola">
			<input id="hdAvviaRicerca" type="hidden" name="hdAvviaRicerca">
		</form>
	</body>
</HTML>
