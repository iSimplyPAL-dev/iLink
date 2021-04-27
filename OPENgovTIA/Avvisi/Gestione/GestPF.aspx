<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GestPF.aspx.vb" Inherits="OPENgovTIA.GestPF" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>GestPF</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%        End If%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" src="../../../_js/ControlloData.js?newversion"></script>
		<script type="text/javascript">
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
				var strada
				if (objStrada.TipoStrada != '&nbsp;')
				{
					strada= objStrada.TipoStrada;
				}
				if (objStrada.Strada != '&nbsp;')
				{
					strada=strada+ ' ' + objStrada.Strada;
				}
				if (objStrada.Frazione!='CAPOLUOGO')
				{
					strada= strada+ ' ' + objStrada.Frazione;
				}
				strada = strada.replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
				document.getElementById('TxtCodVia').value = objStrada.CodStrada;
				document.getElementById('TxtVia').value=strada;
				document.getElementById('TxtViaRibaltata').value = strada;
			}

			function ShowRicUIAnater()
				{
					winWidth=800 
					winHeight=600 
					myleft=(screen.width-winWidth)/2 
					mytop=(screen.height-winHeight)/2 - 40 
					WinPopFamiglia=window.open("../RicercaAnater/FrmRicercaImmobile.aspx?sProvenienza=A","","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
				}

			function ShowInsertRidDet(sTypeShow)
			{ 
				//alert(IdProvv);
				if (document.getElementById('TxtAnno').value=='')
				{
					GestAlert('a', 'warning', '', '', 'E\' necessario inserire l\'Anno!');
					Setfocus(document.getElementById('TxtAnno'));
					return false;
				}
				winWidth=690 
				winHeight=200 
				myleft=(screen.width-winWidth)/2
				mytop = (screen.height - winHeight) / 2 - 40
				var myProv="U"
				if (document.getElementById('TxtTipoPartita').value == 'PC')
				    myProv = "T"
				Parametri="Provenienza=S&sTypeShow=" + sTypeShow  +"&Anno=" + document.getElementById('TxtAnno').value 
				WinPopRidDet=window.open("../../Dichiarazioni/PopUpInsertRidDet.aspx?"+Parametri,"","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
			}

			function CheckDatiArticolo(IsRipulisci)
			{
				document.getElementById('TxtIsRipulisci').value=IsRipulisci;
				//devo avere il contribuente
				//devo avere l'anno
				if (document.getElementById('TxtAnno').value=='')
				{
					GestAlert('a', 'warning', '', '', 'E\' necessario inserire l\'Anno!');
					Setfocus(document.getElementById('TxtAnno'));
					return false;
				}
				//devo avere i dati dell'immobile
				if (document.getElementById('TxtVia').value=='')
				{
					/*GestAlert('a', 'warning', '', '', 'E\' necessario inserire la Via!');
					return false;*/
				}
				else
				{
					if(!IsValidChar(document.getElementById('TxtVia').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Via!');
						Setfocus(document.getElementById('TxtVia'));
						return false;
					}	
				}
				//devo avere una data di inizio o i bimestri
				if (document.getElementById('TxtDataInizio').value=='' && document.getElementById('TxtBimestri').value=='') 
				{ 
					GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Data di Inizio o i Bimestri!');
					Setfocus(document.getElementById('TxtDataInizio'));
					return false; 
				}
				//devo avere la tariffa
				if (document.getElementById('TxtTariffa').value=='')
				{
					GestAlert('a', 'warning', '', '', 'E\' necessario inserire la Categoria!');
					Setfocus(document.getElementById('TxtTariffa'));
					return false;
				}
				//devo avere i mq
				if (document.getElementById('TxtMq').value=='')
				{
					GestAlert('a', 'warning', '', '', 'E\' necessario inserire i Metri!');
					Setfocus(document.getElementById('TxtMq'));
					return false;
				}
				else
				{
					sGG=document.getElementById('TxtMq').value;
					if (!isNumber(sGG))
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo MQ!');
						Setfocus(document.getElementById('TxtMq'));
						return false;
					}	
				}
				//se forzo l'importo devo inserire l'importo articolo
				if (document.getElementById('ChkImpForzato').checked==true)
				{
					if (document.getElementById('TxtImpArticolo').value=='') 
					{
						GestAlert('a', 'warning', '', '', 'E\' necessario inserire l\'Importo Articolo!');
						Setfocus(document.getElementById('TxtImpArticolo'));
						return false;
					}
					else
					{
						sGG=document.getElementById('TxtImpArticolo').value;
						if (!isNumber(sGG))
						{
							GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo Importo Articolo!');
							Setfocus(document.getElementById('TxtImpArticolo'));
							return false;
						}	
					}
				}
				//se ho tarsu giornaliera devo avere i bimestri
				if (document.getElementById('ChkIsGiornaliera').checked==true){
					if (document.getElementById('TxtBimestri').value=='') 
					{
						GestAlert('a', 'warning', '', '', 'Inserire il numero di giorni per la TARSU giornaliera!');
						Setfocus(document.getElementById('TxtBimestri'));
						return false;
					}
					else
					{
						sGG=document.getElementById('TxtBimestri').value;
						if (!isNumber(sGG,0,0))
						{
							GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Giorni TARSU!');
							Setfocus(document.getElementById('TxtBimestri'));
							return false;
						}	
					}
				}
				else
				{
					//se ho i bimestri non devono essere maggiori di 6 e interi
					if (document.getElementById('TxtBimestri').value!='')
					{
						if (document.getElementById('TxtBimestri').value>6)
						{
							GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI inferiori a 6 nel campo Bimestri!');
							Setfocus(document.getElementById('TxtBimestri'));
							return false;
						}
						else
						{
							sGG=document.getElementById('TxtBimestri').value;
							if (!isNumber(sGG,0,0))
							{
								GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Bimestri!');
								Setfocus(document.getElementById('TxtBimestri'));
								return false;
							}	
						}
					}
				}
				//se ho differenza d'imposta devo avere d'accertamento
				if (document.getElementById('TxtImpDiffImposta').value!='')
				{
				    if (document.getElementById('ChkDaAccertamento').checked==false) 
					{
						GestAlert('a', 'warning', '', '', 'E\' necessario inserire il Flag Da Accertamento!');
						Setfocus(document.getElementById('ChkDaAccertamento'));
						return false;
					}
				}
				//il civico deve essere numerico
				if (document.getElementById('TxtCivico').value!='') {
					sGG=document.getElementById('TxtCivico').value;
					if (!isNumber(sGG,0,0))
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Civico!');
						Setfocus(document.getElementById('TxtCivico'));
						return false;
					}	
				}
				//il numero componenti deve essere numerico
				if (document.getElementById('TxtNComponenti').value!='') {
					sGG=document.getElementById('TxtNComponenti').value;
					if (!isNumber(sGG,0,0))
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Componenti!');
						Setfocus(document.getElementById('TxtNComponenti'));
						return false;
					}	
				}
				//controllo la presenza di stringhe valide
				if (document.getElementById('TxtEsponente').value!=''){
					if(!IsValidChar(document.getElementById('TxtEsponente').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Esponente!');
						Setfocus(document.getElementById('TxtEsponente'));
						return false;
					}	
				}
				if (document.getElementById('TxtInterno').value!=''){
					if(!IsValidChar(document.getElementById('TxtInterno').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Interno!');
						Setfocus(document.getElementById('TxtInterno'));
						return false;
					}	
				}
				if (document.getElementById('TxtScala').value!=''){
					if(!IsValidChar(document.getElementById('TxtScala').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Scala!');
						Setfocus(document.getElementById('TxtScala'));
						return false;
					}	
				}

				if (document.getElementById('TxtFoglio').value!=''){
					if(!IsValidChar(document.getElementById('TxtFoglio').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Foglio!');
						Setfocus(document.getElementById('TxtFoglio'));
						return false;
					}	
				}
				if (document.getElementById('TxtNumero').value!=''){
					if(!IsValidChar(document.getElementById('TxtNumero').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Numero!');
						Setfocus(document.getElementById('TxtNumero'));
						return false;
					}	
				}
				if (document.getElementById('TxtSubalterno').value!=''){
					if(!IsValidChar(document.getElementById('TxtSubalterno').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Subalterno!');
						Setfocus(document.getElementById('TxtSubalterno'));
						return false;
					}	
				}

				if (document.getElementById('TxtDescrDiffImposta').value!=''){
					if(!IsValidChar(document.getElementById('TxtDescrDiffImposta').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Differenza d\'imposta!');
						Setfocus(document.getElementById('TxtDescrDiffImposta'));
						return false;
					}	
				}
				if (document.getElementById('TxtDescrSanzioni').value!=''){
					if(!IsValidChar(document.getElementById('TxtDescrSanzioni').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Sanzioni!');
						Setfocus(document.getElementById('TxtDescrSanzioni'));
						return false;
					}	
				}
				if (document.getElementById('TxtDescrInteressi').value!=''){
					if(!IsValidChar(document.getElementById('TxtDescrInteressi').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Interessi!');
						Setfocus(document.getElementById('TxtDescrInteressi'));
						return false;
					}	
				}
				if (document.getElementById('TxtDescrSpeseNot').value!=''){
					if(!IsValidChar(document.getElementById('TxtDescrSpeseNot').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Spese notifica!');
						Setfocus(document.getElementById('TxtDescrSpeseNot'));
						return false;
					}	
				}
				//gli importi devono essere validi
				if (document.getElementById('TxtImpSanzioni').value!='') 
				{
					sGG=document.getElementById('TxtImpSanzioni').value;
					if (!isNumber(sGG))
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo Importo Sanzioni!');
						Setfocus(document.getElementById('TxtImpSanzioni'));
						return false;
					}	
				}
				if (document.getElementById('TxtImpInteressi').value!='') 
				{
					sGG=document.getElementById('TxtImpInteressi').value;
					if (!isNumber(sGG))
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo Importo Interessi!');
						Setfocus(document.getElementById('TxtImpInteressi'));
						return false;
					}	
				}
				if (document.getElementById('TxtImpSpeseNot').value!='') 
				{
					sGG=document.getElementById('TxtImpSpeseNot').value;
					if (!isNumber(sGG))
					{
						GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo Importo Spese Notifica!');
						Setfocus(document.getElementById('TxtImpSpeseNot'));
						return false;
					}	
				}
				//se ho la data inizio controllo che sia valida e coerente con l'anno
				if (document.getElementById('TxtDataInizio').value!='') 
				{ 
					if(!isDate(document.getElementById('TxtDataInizio').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire la Data di Inizio correttamente in formato: GG/MM/AAAA!');
						Setfocus(document.getElementById('TxtDataInizio'));
						return false;
					}	
					else
					{	
						var sAnno =new String
						sAnno = document.getElementById('TxtDataInizio').value
						sAnno = sAnno.substring(6,10)
						if (document.getElementById('TxtAnno').value < sAnno)
						{
							GestAlert('a', 'warning', '', '', 'La Data di Inizio non e\' coerente con l\'Anno!');
							Setfocus(document.getElementById('TxtDataInizio'));
							return false;
						}
					}
				}		
				//se ho la data fine controllo che sia valida e coerente con l'anno
				if (document.getElementById('TxtDataFine').value!='') 
				{ 
					if(!isDate(document.getElementById('TxtDataFine').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire la Data di Fine correttamente in formato: GG/MM/AAAA!');
						Setfocus(document.getElementById('TxtDataFine'));
						return false;
					}	
					else
					{	
						var sAnno =new String
						sAnno = document.getElementById('TxtDataFine').value
						sAnno = sAnno.substring(6,10)
						if (document.getElementById('TxtAnno').value > sAnno)
						{
							GestAlert('a', 'warning', '', '', 'La Data di Fine non e\' coerente con l\'Anno!');
							Setfocus(document.getElementById('TxtDataFine'));
							return false;
						}
					}
				}		
				//se ho la data inizio e fine controllo che siano coerenti
				if (document.getElementById('TxtDataInizio').value!='' && document.getElementById('TxtDataFine').value!='') 
				{
					var starttime = document.getElementById('TxtDataInizio').value
					var endtime = document.getElementById('TxtDataFine').value
					//Start date split to UK date format and add 31 days for maximum datediff
					starttime = starttime.split("/"); 
					starttime = new Date(starttime[2],starttime[1]-1,starttime[0]); 
					//End date split to UK date format 
					endtime = endtime.split("/");
					endtime = new Date(endtime[2],endtime[1]-1,endtime[0]); 
					if (endtime<=starttime)
					{
						GestAlert('a', 'warning', '', '', 'La Data di Fine e\' minore/uguale alla Data di Inizio!');
						Setfocus(document.getElementById('TxtDataFine'));
						return false;
					}
				}
				
				document.getElementById('CmdSalva').click()
			}
			
			function DeleteArticolo()
			{
				if (confirm('Si desidera eliminare l\'articolo?'))
				{
				    document.getElementById('CmdDelete').click()
				}
				return false;
			}

			function ClearDatiArticolo()
			{ 
				if (document.getElementById('TxtId').value=='-1' && document.getElementById('TxtCodCartella').value!='-1')
				{
					if (confirm('I dati non sono stati salvati.\nUscire senza salvare?'))
					{
					    document.getElementById('CmdClearDatiArticolo').click()
					}
				}
				else
				{
					document.getElementById('CmdClearDatiArticolo').click()
				}
			}
		</script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
            <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden">
                <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table1">
                    <tr valign="top">
                        <td align="left">
                            <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                                <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                            </span>
                        </td>
                        <td align="right" rowspan="2">
						    <INPUT class="Bottone BottoneApri" id="Modifica" title="Modifica Articolo" onclick="document.getElementById('CmdModifica').click()" type="button" name="Modifica"> 
						    <INPUT class="Bottone BottoneCancella" id="Delete" title="Elimina Articolo" onclick="DeleteArticolo()" type="button" name="Delete"> 
						    <INPUT class="Bottone BottoneCalcolo" id="sgravio" title="Calcola Sgravio" onclick="document.getElementById('CmdSgravi').click()" type="button" name="sgravio">
						    <INPUT class="Bottone BottoneSalva" style="DISPLAY:none" id="Salva" title="Salva Articolo" onclick="CheckDatiArticolo(0)" type="button" name="Salva"> 
						    <INPUT class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla ricerca" onclick="ClearDatiArticolo()" type="button" name="Cancel">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <span class="NormalBold_title" id="info" runat="server" style="width: 400px">- Avvisi - Gestione - Dettaglio</span>
                        </td>
                    </tr>
                </table>
            </div>
            &nbsp;
		    <div class="col-md-12">
			    <table width="100%">
				    <tr>
				        <td class="hidden">
				            <asp:Label ID="Label13" CssClass="lstTabRow" runat="server">Tipologia Calcolo</asp:Label><br />
				            <fieldset id="Fieldset1" class="classeFieldSetRicerca" runat="server">
				                <table>
				                    <tr>
				                        <td>
				                            <asp:Label ID="Label16" runat="server" CssClass="Input_Label">Tipo Calcolo</asp:Label>&nbsp;
				                            <asp:Label runat="server" CssClass="Input_Label" ID="LblTipoCalcolo"></asp:Label>
				                        </td>
				                        <td>
				                            <asp:CheckBox ID="ChkConferimenti" runat="server" CssClass="Input_Label" Text="presenza Conferimenti" />
				                        </td>
				                        <td>
				                            <asp:CheckBox ID="ChkMaggiorazione" runat="server" CssClass="Input_Label" text="presenza Maggiorazione"/>
				                        </td>
				                        <td>
				                            <asp:Label ID="Label18" runat="server" CssClass="Input_Label">Tipo Superfici</asp:Label>&nbsp;
				                            <asp:Label runat="server" CssClass="Input_Label" ID="LblTipoMQ"></asp:Label>
				                        </td>
				                    </tr>
				                </table>
				            </fieldset>
				        </td>
				    </tr>
				    <!--blocco dati contribuente--><%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
				    <tr id="TRPlainAnag">
				        <td colspan="2">
				            <iframe id="ifrmAnag" runat="server" src="../../../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				            <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				        </td>
				    </tr>
				    <tr id="TRSpecAnag">
					    <td colspan="2" width="100%">
						    <iframe id="LoadAnagrafica" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="100" runat="server" style="Z-INDEX: 0"></iframe>
					    </td>
				    </tr>
				    <!--Blocco Dati UI-->
				    <tr>
					    <td>
						    <table id="TblDati" width="100%">
							    <tr>
								    <td colspan="8"><asp:label id="Label10" CssClass="lstTabRow" Width="100%" Runat="server">Dati Immobile</asp:label></td>
							    </tr>
							    <tr>
								    <td colspan="8"><asp:label id="Label3" CssClass="Input_Label" Runat="server">Via</asp:label><asp:label id="Label14" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label>&nbsp;
									    <asp:imagebutton id="LnkOpenStradario" runat="server" ToolTip="Ubicazione Immobile da Stradario."
										    CausesValidation="False" imagealign="Bottom" ImageUrl="../../../images/Bottoni/Listasel.png"></asp:imagebutton><br />
									    <asp:textbox id="TxtVia" CssClass="Input_Text" Width="500px" Runat="server" ReadOnly="True"></asp:textbox><asp:textbox id="TxtCodVia" style="DISPLAY: none" CssClass="Input_Text" Runat="server"></asp:textbox>
									    <asp:textbox id="TxtViaRibaltata" style="DISPLAY: none" CssClass="Input_Text" Runat="server"></asp:textbox>
								    </td>
							    </tr>
							    <tr>
								    <td><asp:label id="Label4" CssClass="Input_Label" Runat="server">Civico</asp:label><br />
									    <asp:textbox id="TxtCivico" CssClass="Input_Text" Width="70px" Runat="server"></asp:textbox></td>
								    <td><asp:label id="Label5" CssClass="Input_Label" Runat="server">Esponente</asp:label><br />
									    <asp:textbox id="TxtEsponente" CssClass="Input_Text" Width="70px" Runat="server"></asp:textbox></td>
								    <td><asp:label id="Label6" CssClass="Input_Label" Runat="server">Interno</asp:label><br />
									    <asp:textbox id="TxtInterno" CssClass="Input_Text" Width="70px" Runat="server"></asp:textbox></td>
								    <td><asp:label id="Label8" CssClass="Input_Label" Runat="server">Scala</asp:label><br />
									    <asp:textbox id="TxtScala" CssClass="Input_Text" Width="70px" Runat="server"></asp:textbox></td>
							    </tr>
							    <tr>
								    <td><asp:label id="Label11" CssClass="Input_Label" Runat="server">Data Inizio</asp:label><asp:label id="Label17" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label><br />
									    <asp:textbox id="TxtDataInizio" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" Runat="server" maxlength="10"></asp:textbox></td>
								    <td><asp:label id="Label12" CssClass="Input_Label" Runat="server">Data Fine</asp:label><br />
									    <asp:textbox id="TxtDataFine" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" Runat="server" maxlength="10"></asp:textbox></td>
								    <td>
								        <asp:label CssClass="Input_Label" Runat="server">Tempo</asp:label><br />
									    <asp:textbox id="TxtTempo" CssClass="Input_Text_Right" Width="50px" Runat="server"></asp:textbox>
								    </td>
								    <td><asp:label id="Label9" CssClass="Input_Label" Runat="server">TARSU giornaliera</asp:label><br />
									    <asp:checkbox id="ChkIsGiornaliera" CssClass="Input_CheckBox" Runat="server" AutoPostBack="True"></asp:checkbox></td>
								    <td><asp:label id="Label15" CssClass="Input_Label" Runat="server">N.GG</asp:label><br />
									    <asp:textbox id="TxtGGTarsu" CssClass="Input_Text_Right" Width="50px" Runat="server"
										    Enabled="False"></asp:textbox></td>
								    <td><asp:label id="Label32" CssClass="Input_Label" Runat="server">MQ Tassabili</asp:label><br>
									    <asp:textbox id="TxtMQTassabili" CssClass="Input_Text_right" Width="80px" Runat="server"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="Label27" Runat="server" CssClass="Input_Label">Componenti PF</asp:label><br />
									    <asp:textbox id="TxtNComponenti" Runat="server" CssClass="Input_Text" style="TEXT-ALIGN: right"
										    MaxLength="2" Width="50px"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="Label34" Runat="server" CssClass="Input_Label">Componenti PV</asp:label><br />
									    <asp:textbox id="TxtNComponentiPV" Runat="server" CssClass="Input_Text" style="TEXT-ALIGN: right"
										    MaxLength="2" Width="50px"></asp:textbox>
								    </td>
							    </tr>
							    <tr>
								    <!--*** 20130228 - gestione categoria Ateco per TARES ***-->
								    <td colspan="7">
									    <asp:Label CssClass="Input_Label" Runat="server" id="Label35">Cat.Ateco</asp:Label><br>
									    <asp:DropDownList id="DDlCatAteco" CssClass="Input_Label" Runat="server" Width="500px"></asp:DropDownList>
								    </td>
								    <td><asp:label id="LblForzaPV" CssClass="Input_Label" Runat="server">Forza Calcolo PV</asp:label><br />
									    <asp:checkbox id="ChkForzaPV" CssClass="Input_CheckBox" Runat="server" AutoPostBack="True"></asp:checkbox></td>
								    <!--*** ***-->
							    </tr>
							    <tr>
								    <td>
								        <asp:label CssClass="Input_Label" Runat="server">Anno</asp:label><br />
									    <asp:textbox id="TxtAnno" CssClass="Input_Text_Right OnlyNumber" Runat="server" width="90px" Enabled="False"></asp:textbox>
								    </td>
								    <td><asp:label ID="Label1" CssClass="Input_Label" Runat="server">Tariffa €</asp:label><br />
									    <asp:textbox id="TxtTariffa" CssClass="Input_Text_Right" Runat="server" width="90px" Enabled="False"></asp:textbox></td>
								    <td><asp:label ID="Label2" CssClass="Input_Label" Runat="server">Imp.Articolo €</asp:label><br />
									    <asp:textbox id="TxtImpArticolo" CssClass="Input_Text_Right OnlyNumber" Runat="server" width="90px"></asp:textbox></td>
								    <td><asp:label CssClass="Input_Label" Runat="server">Imp.Netto €</asp:label><br />
									    <asp:textbox id="TxtImpNetto" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);" Runat="server" width="90px" Enabled="False"></asp:textbox></td>
								    <td><asp:label CssClass="Input_Label" Runat="server"></asp:label><br />
									    <asp:checkbox id="ChkImpForzato" CssClass="Input_Label" Runat="server" Text="Forza l'importo" TextAlign="Right"></asp:checkbox></td>
							    </tr>
						    </table>
					    </td>
				    </tr>
				    <tr>
					    <td>
						    <table id="TblUnitaImmo" cellSpacing="0" cellPadding="0" border="0" width="100%">
							    <!--Blocco Dati Catastali-->
							    <tr>
								    <td style="WIDTH: 100%">
									    <table id="TblCatastali" cellSpacing="0" cellPadding="0" width="100%" border="0">
										    <tr>
											    <td colSpan="6"><asp:label id="Label30" CssClass="lstTabRow" Width="100%" Runat="server">Dati Catastali</asp:label>
												    <!--<asp:label CssClass="lstTabRow" Width="672px" Runat="server">&nbsp;</asp:label>-->
												    <!--<a class="Link_Label" id="LnkRicDatiCatastaliAnater" href="" Runat="server">&gt;&gt;</a>-->
											    </td>
										    </tr>
										    <tr>
											    <td><asp:label id="Label38" CssClass="Input_Label" Runat="server">Foglio</asp:label><br />
												    <asp:textbox id="TxtFoglio" CssClass="Input_Text" Width="90px" Runat="server"></asp:textbox></td>
											    <td><asp:label id="Label39" CssClass="Input_Label" Runat="server">Numero</asp:label><br />
												    <asp:textbox id="TxtNumero" CssClass="Input_Text" Width="90px" Runat="server"></asp:textbox></td>
											    <td><asp:label id="Label40" CssClass="Input_Label" Runat="server">Subalterno</asp:label><br />
												    <asp:textbox id="TxtSubalterno" CssClass="Input_Text" Width="90px" Runat="server"></asp:textbox></td>
											    <td><asp:label id="Label28" runat="server" CssClass="Input_Label">Est.Particella</asp:label><br />
												    <asp:textbox id="TxtEstParticella" tabIndex="10" runat="server" CssClass="Input_Text" Width="90px">0</asp:textbox></td>
											    <td><asp:label id="Label29" runat="server" CssClass="Input_Label">Tipo Particella</asp:label><br />
												    <asp:dropdownlist id="DdlTipoParticella" CssClass="Input_Text" Width="100px" Runat="server"></asp:dropdownlist></td>
											    <td><asp:label id="Label23" runat="server" CssClass="Input_Label">Sezione</asp:label><br />
												    <asp:textbox id="TxtSezione" runat="server" CssClass="Input_Text" Width="90px"></asp:textbox></td>
										    </tr>
									    </table>
								    </td>
							    </tr>
							    <tr>
								    <td><br />
								    </td>
							    </tr>
							    <!--Blocco Dati Riduzioni/Detassazioni-->
							    <tr>
								    <td>
									    <table id="TblRidEse" cellSpacing="0" cellPadding="0" border="0" width="100%">
										    <tr>
											    <td>
												    <table>
													    <tr>
														    <td width="45%"><asp:label id="Label31" CssClass="lstTabRow" Runat="server">Dati Riduzioni</asp:label>
														    <asp:imagebutton id="LnkNewRid" CssClass="nascosto" Runat="server" ImageUrl="../../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px"></asp:imagebutton>
														    <asp:imagebutton id="LnkDelRid" CssClass="hidden" Runat="server" ImageUrl="../../../images/Bottoni/cestino.png" Height="15px" Width="15px"></asp:imagebutton>
														    <asp:label CssClass="lstTabRow" Width="210px" Runat="server">&nbsp;</asp:label>
													    </td>
													    </tr>
													    <tr>
														    <td width="45%">
														        <asp:label id="LblResultRid" CssClass="Legend" Runat="server">Non sono presenti riduzioni</asp:label>
                                                                <Grd:RibesGridView ID="GrdRiduzioni" runat="server" BorderStyle="None" 
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
																	    <asp:BoundField DataField="SCODICE" HeaderText="Codice"></asp:BoundField>
																	    <asp:BoundField DataField="SDESCRIZIONE" HeaderText="Descrizione"></asp:BoundField>
								                                        <asp:TemplateField HeaderText="">
									                                        <headerstyle horizontalalign="Center"></headerstyle>
									                                        <itemstyle horizontalalign="Center"></itemstyle>
									                                        <itemtemplate>
										                                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" CommandArgument='<%# Eval("SCODICE") %>' alt=""></asp:ImageButton>
					                                                            <asp:HiddenField runat="server" ID="hfIDRIFERIMENTO" Value='<%# Eval("IDRIFERIMENTO") %>' />
                                                                                <asp:HiddenField runat="server" ID="hfSTIPOVALORE" Value='<%# Eval("STIPOVALORE") %>' />
									                                        </itemtemplate>
								                                        </asp:TemplateField>
																    </Columns>
															    </Grd:RibesGridView>
														    </td>
													    </tr>
												    </table>
											    </td>
											    <td>
												    <table>
													    <tr>
														    <td style="HEIGHT: 30px" width="45%"><asp:label id="Label33" CssClass="lstTabRow" Runat="server">Dati Esenzioni</asp:label>
														    <asp:imagebutton id="LnkNewDet" CssClass="nascosto" Runat="server" ImageUrl="../../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px"></asp:imagebutton>
														    <asp:imagebutton id="LnkDelDet" CssClass="hidden" Runat="server" ImageUrl="../../../images/Bottoni/cestino.png" Height="15px" Width="15px"></asp:imagebutton>
														    <asp:label CssClass="lstTabRow" Width="188px" Runat="server">&nbsp;</asp:label>
													    </td>
													    </tr>
													    <tr>
														    <td width="45%">
                                                                <asp:label id="LblResultDet" CssClass="Legend" Runat="server">Non sono presenti Esenzioni</asp:label>
                                                                <Grd:RibesGridView ID="GrdDetassazioni" runat="server" BorderStyle="None" 
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
																	    <asp:BoundField DataField="SCODICE" HeaderText="Codice"></asp:BoundField>
																	    <asp:BoundField DataField="SDESCRIZIONE" HeaderText="Descrizione"></asp:BoundField>
								                                        <asp:TemplateField HeaderText="">
									                                        <headerstyle horizontalalign="Center"></headerstyle>
									                                        <itemstyle horizontalalign="Center"></itemstyle>
									                                        <itemtemplate>
										                                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" CommandArgument='<%# Eval("SCODICE") %>' alt=""></asp:ImageButton>
					                                                            <asp:HiddenField runat="server" ID="hfIDRIFERIMENTO" Value='<%# Eval("IDRIFERIMENTO") %>' />
                                                                                <asp:HiddenField runat="server" ID="hfSTIPOVALORE" Value='<%# Eval("STIPOVALORE") %>' />
									                                        </itemtemplate>
								                                        </asp:TemplateField>
																    </Columns>
															    </Grd:RibesGridView>
                                                            </td>
													    </tr>
												    </table>
											    </td>
										    </tr>
									    </table>
								    </td>
							    </tr>
							    <tr>
								    <td style="WIDTH: 100%"><br />
								    </td>
							    </tr>
							    <!--Blocco Dati Agenzia Entrate-->
							    <tr>
								    <td style="WIDTH: 100%">
									    <table id="TblAgenziaEntrate" cellSpacing="0" cellPadding="0" width="100%" border="0">
										    <tr>
											    <td colSpan="5"><asp:label id="Label19" CssClass="lstTabRow" Width="100%" Runat="server">Dati Agenzia Entrate</asp:label></td>
										    </tr>
										    <tr>
											    <td><asp:label id="Label24" CssClass="Input_Label" Runat="server">Titolo Occupazione</asp:label><br />
												    <asp:dropdownlist id="DdlTitOccupaz" CssClass="Input_Text" Runat="server" Width="350px"></asp:dropdownlist></td>
											    <td><asp:label id="Label20" CssClass="Input_Label" Runat="server">Natura Occupazione</asp:label><br />
												    <asp:dropdownlist id="DdlNatOccupaz" CssClass="Input_Text" Runat="server" Width="300px"></asp:dropdownlist></td>
										    </tr>
										    <tr>
											    <td><asp:label id="Label21" CssClass="Input_Label" Runat="server">Destinazione d'Uso</asp:label><br />
												    <asp:dropdownlist id="DdlDestUso" CssClass="Input_Text" Runat="server" Width="350px"></asp:dropdownlist></td>
											    <td><asp:label id="Label22" CssClass="Input_Label" Runat="server">Tipo Unita'</asp:label><br />
												    <asp:dropdownlist id="DdlTipoUnita" CssClass="Input_Text" Runat="server"></asp:dropdownlist></td>
										    </tr>
										    <tr>
											    <td colspan="2"><asp:label id="Label26" CssClass="Input_Label" Runat="server">Assenza Dati Catastali</asp:label><br />
												    <asp:dropdownlist id="DdlAssenzaDatiCat" CssClass="Input_Text" Runat="server" Width="450px"></asp:dropdownlist></td>
										    </tr>
									    </table>
								    </td>
							    </tr>
							    <tr>
								    <td style="WIDTH: 100%"><br />
								    </td>
							    </tr>
							    <!--Blocco Note UI-->
							    <tr>
								    <td style="WIDTH: 100%">
									    <table id="TblNoteUI" cellSpacing="0" cellPadding="0" width="100%" border="0">
										    <tr>
											    <td><asp:label id="Label7" CssClass="lstTabRow" Width="100%" Runat="server">Note</asp:label><br />
												    <asp:textbox id="TxtNote" CssClass="Input_Text" Runat="server" Height="32px" TextMode="MultiLine"
													    width="100%"></asp:textbox></td>
										    </tr>
									    </table>
								    </td>
							    </tr>
						    </table>
					    </td>
				    </tr>
			    </table>
            </div>
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
			<asp:textbox id="TxtId" style="DISPLAY: none" Runat="server">-1</asp:textbox>
			<asp:textbox id="TxtIdArticolo" style="DISPLAY: none" Runat="server">-1</asp:textbox>
			<asp:textbox id="TxtIdFlussoRuolo" style="DISPLAY: none" Runat="server">-1</asp:textbox>
			<asp:textbox id="TxtCodContribuente" style="DISPLAY: none" Runat="server">-1</asp:textbox>
			<asp:textbox id="TxtIdDettaglioTestata" style="DISPLAY: none" Runat="server"></asp:textbox>
			<asp:textbox id="TxtIdTariffa" style="DISPLAY: none" Runat="server">-1</asp:textbox>
			<asp:textbox id="TxtCodCartella" style="DISPLAY: none" Runat="server"></asp:textbox>
			<asp:TextBox ID="TxtTipoPartita" style="DISPLAY: none" Runat="server"></asp:textbox>
			<asp:button id="CmdSalvaRidEse" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="CmdSalva" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="CmdSalvaDati" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="CmdModifica" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="CmdDelete" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="CmdClearDatiArticolo" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="CmdSgravi" style="DISPLAY: none" runat="server" Width="32px" Height="32px" Text="Sgravio"></asp:button>
			<asp:button id="CmdRibaltaUIAnater" style="DISPLAY: none" Runat="server"></asp:button>
		</form>
	</body>
</HTML>
