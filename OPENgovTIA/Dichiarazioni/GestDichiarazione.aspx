<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GestDichiarazione.aspx.vb" Inherits="OPENgovTIA.GestDichiarazione"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <head>
		<title>GestDichiarazione</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name=vs_defaultClientScript content="JavaScript">
		<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%        End If%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">
			function VisualizzaDiv(sNomeDiv){
				if (document.getElementById(sNomeDiv).style.display == '')
					{document.getElementById(sNomeDiv).style.display = 'none'} //nascosto
				else
					{document.getElementById(sNomeDiv).style.display = ''} // visibile
			}

			function ShowInsertTessera(IdContribuente,IdTestata,IdUniqueTessera,AzioneProv,Provenienza)
			{ 
				sParametri ="IdContribuente="+IdContribuente+"&IdTestata="+IdTestata+"&IdUniqueTessera="+IdUniqueTessera+"&AzioneProv="+AzioneProv
				parent.Comandi.location.href='ComandiGestTessere.aspx?AzioneProv='+AzioneProv+"&Provenienza="+ Provenienza
				parent.Visualizza.location.href='GestTessere.aspx?'+sParametri
			}

			function ShowInsertUI(IdContribuente, IdTestata, IdTessera, IdUniqueUI, AzioneProv, Provenienza, IdList, IsFromVariabile) {
			    sParametri = "IdContribuente=" + IdContribuente + "&IdTestata=" + IdTestata + "&IdTessera=" + IdTessera + "&IdUniqueUI=" + IdUniqueUI + "&AzioneProv=" + AzioneProv + "&Provenienza=" + Provenienza + "&IdList=" + IdList
			    sParametri += "&IsFromVariabile=" + IsFromVariabile
			    //parent.Comandi.location.href = 'ComandiGestImmobili.aspx?Provenienza=' + Provenienza + '&AzioneProv=' + AzioneProv
			    parent.Visualizza.location.href = 'GestImmobili.aspx?' + sParametri
			}
			function ShowRicUIAnater() {
			    winWidth = 1000
			    winHeight = 800
			    myleft = (screen.width - winWidth) / 2
			    mytop = (screen.height - winHeight) / 2 - 40
			    WinPopFamiglia = window.open("../RicercaAnater/FrmRicercaImmobile.aspx", "", "width=" + winWidth + ",height=" + winHeight + ", status=yes, toolbar=no,top=" + mytop + ",left=" + myleft + ",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no")
			}            

			function ClearDatiDich()
			{ 
			    document.getElementById('CmdClearDatiDich').click()
			}

			function CheckDatiDich()
			{
			    if (document.getElementById('hdIdContribuente').value == '-1')
				{
					GestAlert('a', 'warning', '', '', 'E\' necessario selezionare il Contribuente!');
					return false; 
				}

			    if (document.getElementById('TxtDataDichiarazione').value=='') 
				{ 
					GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Data di Dichiarazione!');
				    Setfocus(document.getElementById('TxtDataDichiarazione'));
					return false; 
				}		
				else 
				{
				    if(!isDate(document.getElementById('TxtDataDichiarazione').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire la Data di Dichiarazione correttamente in formato: GG/MM/AAAA!');
						Setfocus(document.getElementById('TxtDataDichiarazione'));
						return false;
					}	
				}		
				if (document.getElementById('TxtDataCessazione').value!='') 
				{ 
					if(!isDate(document.getElementById('TxtDataCessazione').value)) 
					{
						GestAlert('a', 'warning', '', '', 'Inserire la Data di Cessazione correttamente in formato: GG/MM/AAAA!');
						Setfocus(document.getElementById('TxtDataCessazione'));
						return false;
					}	
					else
					{
						//se ho la data cessazione controllo che sia coerente
						if (document.getElementById('TxtDataDichiarazione').value!='' && document.getElementById('TxtDataCessazione').value!='') 
						{
							var starttime = document.getElementById('TxtDataDichiarazione').value
							var endtime = document.getElementById('TxtDataCessazione').value
							//Start date split to UK date format and add 31 days for maximum datediff
							starttime = starttime.split("/"); 
							starttime = new Date(starttime[2],starttime[1]-1,starttime[0]); 
							//End date split to UK date format 
							endtime = endtime.split("/");
							endtime = new Date(endtime[2],endtime[1]-1,endtime[0]); 
							if (endtime<=starttime)
							{
								GestAlert('a', 'warning', '', '', 'La Data di Cessazione e\' minore/uguale alla Data di Dichiarazione!');
								Setfocus(document.getElementById('TxtDataCessazione'));
								return false;
							}
						}
					}
				}		
				document.getElementById('CmdSaveDatiDich').click()
			}				
			
			function DeleteDich()
			{
				if (confirm('Si desidera eliminare la Dichiarazione?'))
				{
				    if (document.getElementById('TxtIsInRuolo').value>0)
					{
						if (!confirm('Per la dichiarazione sono già stati calcolati articoli.\nSi vuole proseguire?'))
						{
							return false;
						}
					}
				document.getElementById('CmdDeleteDich').click()
				}
				return false;
			}
			
			function nascondi(chiamante, oggetto, label) {
			    if (document.getElementById(oggetto).style.display == "") {
			        document.getElementById(oggetto).style.display = "none"
			        chiamante.title = "Visualizza " + label
			        chiamante.innerText = "Visualizza " + label
			    } else {
			        document.getElementById(oggetto).style.display = ""
			        chiamante.title = "Nascondi " + label
			        chiamante.innerText = "Nascondi " + label
			    }
			}
			function ApriRicercaAnagrafe(nomeSessione){ 
				winWidth=980 
				winHeight=680 
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40 
				Parametri="sessionName=" + nomeSessione 
				WinPopUpRicercaAnagrafica=window.open("../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?"+Parametri,"","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
			}
			function ClearDatiContrib()
			{
				if (confirm('Si desidera eliminare il Contribuente?'))
				{
				    document.getElementById('TxtCognome').value = '';
					document.getElementById('TxtCodFiscale').value = '';
					document.getElementById('TxtPIva').value = '';
					document.getElementById('TxtNome').value = '';
					document.getElementById('TxtDataNascita').value = '';
					document.getElementById('TxtLuogoNascita').value = '';
					document.getElementById('TxtResVia').value = '';
					document.getElementById('TxtResCivico').value = '';
					document.getElementById('TxtResEsponente').value = '';
					document.getElementById('TxtResInterno').value = '';
					document.getElementById('TxtResScala').value = '';
					document.getElementById('TxtResCAP').value = '';
					document.getElementById('TxtResComune').value = '';
					document.getElementById('TxtResPv').value = '';
					document.getElementById('M').checked = false;
					document.getElementById('F').checked = false;
					document.getElementById('G').checked = false;
					document.getElementById('TxtCodContribuente').value = '-1';
				}
				return false;
			}
			function ApriRicAnater(){
				// apro il popup di ricerca in anagrafe anater
				winWidth=980 
				winHeight=500 
				myleft=(screen.availWidth-winWidth)/2 
				mytop=(screen.availheight-winHeight)/2 - 40 
				var parametri = 'popup=1';
				
				WinPopAnater=window.open('../../AnagraficaAnater/popAnagAnater.aspx?'+parametri,'','width='+winWidth+',height='+winHeight+',top='+mytop+',left='+myleft+' status=yes, toolbar=no,scrollbar=no, resizable=no') 
			}
			function CalcoloRuolo() {
			    parent.Comandi.location.href = '../../aspVuotaRemoveComandi.aspx';
			    divCalcolo.style.display = '';
			    divDichiarazione.style.display = 'none';
			    divCalcolato.style.display = 'none';
			}
			function CalcoloPuntuale()
			{
			    if (document.getElementById('ddlAnno').value == '')
			    {
			        GestAlert('a', 'warning', '', '', 'E\' necessario inserire un Anno!');
			        return false;
			    } else if (document.getElementById('txtScadenza').value == '') {
			        GestAlert('a', 'warning', '', '', 'E\' necessario inserire la data di Scadenza!');
			        return false;
			    } else
			    {
			        document.getElementById('hfCalcoloPuntuale').value = 1;
			        DivAttesa.style.display = '';
			        document.getElementById('CmdCalcola').click();
			    }
			}
			function Dovuto() {
			    if (document.getElementById('hdIdContribuente').value == '') {
			        GestAlert('a', 'warning', '', '', 'E\' necessario inserire un Contribuente!');
			        return false;
			    } else {
			        document.getElementById('CmdDovuto').click();
			    }
			}
		</script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
            <div id="divDichiarazione" runat="server">
			    <table id="TblGenDich" style="WIDTH: 100%" cellSpacing="1" cellPadding="1" border="0">
				    <!--blocco dati testata-->
				    <tr>
					    <td colspan="5">
						    <asp:Label CssClass="lstTabRow" Runat="server" id="Label1" Width="100%">Dati Testata</asp:Label>
					    </td>
				    </tr>
				    <tr>
					    <td>
						    <div id="DivTestata" runat="server">
							    <table id="TblTestata" cellspacing="0" cellpadding="0" border="0">
								    <tr>
									    <td Width="130">
										    <asp:Label CssClass="Input_Label" Runat="server" id="Label2">Data Dichiarazione</asp:Label>
										    <asp:Label ID="Label30" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:Label>
										    <br />
										    <asp:TextBox Runat="server" ID="TxtDataDichiarazione" CssClass="Input_Text_Right TextDate" maxlength="10" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
									    </td>
									    <td Width="170">
										    <asp:Label CssClass="Input_Label" Runat="server" id="Label3">N.Dichiarazione</asp:Label>
										    <asp:Label ID="Label31" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:Label>
										    <br />
										    <asp:TextBox ID="TxtNDichiarazione" CssClass="Input_Text" Runat="server"></asp:TextBox>
									    </td>
									    <td Width="130">
										    <asp:Label CssClass="Input_Label" Runat="server" id="Label4">Data Protocollo</asp:Label>
										    <br />
										    <asp:TextBox Runat="server" ID="TxtDataProtocollo" CssClass="Input_Text_Right TextDate" maxlength="10" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
									    </td>
									    <td Width="150">
										    <asp:Label CssClass="Input_Label" Runat="server" id="Label5">N.Protocollo</asp:Label>
										    <br />
										    <asp:TextBox ID="TxtNProtocollo" CssClass="Input_Text" Runat="server"></asp:TextBox>
									    </td>
									    <td Width="130">
										    <asp:Label CssClass="Input_Label" Runat="server" id="Label6">Data Cessazione</asp:Label>
										    <br />
										    <asp:TextBox Runat="server" ID="TxtDataCessazione" CssClass="Input_Text_Right TextDate" maxlength="10" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
									    </td>
								    </tr>
							    </table>
						    </div>
					    </td>
				    </tr>
				    <!--blocco dati contribuente--><%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
				    <tr id="TRPlainAnag">
				        <td>
				            <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				            <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				        </td>
				    </tr>
				    <tr id="TRSpecAnag">
					    <td>
						    <div id="DivContribuente" runat="server">
							    <table id="TblContribuente" cellspacing="0" cellpadding="0" border="0">
				                    <tr>
					                    <td colspan="6">
						                    <asp:Label CssClass="lstTabRow" Runat="server" id="Label45">Dati Contribuente</asp:Label>
						                    <asp:Label ID="Label32" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:Label>
						                    <asp:imagebutton id="LnkAnagTributi" runat="server" ImageUrl="../../images/Bottoni/Listasel.png" ToolTip="Ricerca Anagrafica da Tributi" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>&nbsp;
						                    <asp:imagebutton id="LnkAnagAnater" runat="server" ImageUrl="../../images/Bottoni/Listasel.png" ToolTip="Ricerca Anagrafica da Anater" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>&nbsp;
						                    <asp:imagebutton id="LnkPulisciContr" runat="server" ImageUrl="../../images/Bottoni/cancel.png" ToolTip="Pulisci i campi Contribuente" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>
						                    <asp:Label CssClass="lstTabRow" Runat="server" id="Label26" Width="555px">&nbsp;</asp:Label>
					                    </td>
				                    </tr>
								    <!--prima riga-->
								    <tr>
									    <td width="280">
										    <asp:label id="Label8" CssClass="Input_Label" Runat="server">Cod.Fiscale</asp:label>
										    <br />
										    <asp:textbox id="TxtCodFiscale" CssClass="Input_Text" Runat="server" Width="185px"></asp:textbox>
									    </td>
									    <td colspan="3" width="275">
										    <asp:label id="Label9" CssClass="Input_Label" Runat="server">Partita Iva</asp:label>
										    <br />
										    <asp:textbox id="TxtPIva" CssClass="Input_Text" Runat="server" Width="140px"></asp:textbox>
									    </td>
									    <td>
										    <asp:Label CssClass="Input_Label" Runat="server" id="Label10">Sesso</asp:Label>
										    <br />
										    <asp:RadioButton ID="F" CssClass="Input_Label" Runat="server" Text="F" GroupName="Sesso"></asp:RadioButton>
										    <asp:RadioButton ID="M" CssClass="Input_Label" Runat="server" Text="M" GroupName="Sesso"></asp:RadioButton>
										    <asp:RadioButton ID="G" CssClass="Input_Label" Runat="server" Text="G" GroupName="Sesso"></asp:RadioButton>
									    </td>
									    <td>
										    <asp:textbox id="TxtIdDataAnagrafica" Runat="server" Visible="False" Width="10px">-1</asp:textbox>
										    <asp:button id="btnRibalta" style="DISPLAY: none" Runat="server"></asp:button>
										    <asp:button id="btnRibaltaAnagAnater" style="DISPLAY: none" Runat="server"></asp:button>
									    </td>
								    </tr>
								    <!--seconda riga-->
								    <tr>
									    <td width="280">
										    <asp:label id="Label11" CssClass="Input_Label" Runat="server">Cognome/Rag.Soc</asp:label>
										    <br />
										    <asp:textbox id="TxtCognome" CssClass="Input_Text" Runat="server" Width="265px"></asp:textbox>
									    </td>
									    <td colspan="3" width="275">
										    <asp:label id="Label12" CssClass="Input_Label" Runat="server">Nome</asp:label>
										    <br />
										    <asp:textbox id="TxtNome" CssClass="Input_Text" Runat="server" Width="230px"></asp:textbox>
									    </td>
								    </tr>
								    <!--terza riga-->
								    <tr>
									    <td width="280">
										    <asp:label id="Label13" CssClass="Input_Label" Runat="server">Data Nascita</asp:label>
										    <br />
										    <asp:textbox id="TxtDataNascita" Runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox>
									    </td>
									    <td colspan="3" width="275">
										    <asp:label id="Label14" CssClass="Input_Label" Runat="server">Luogo Nascita</asp:label>
										    <br />
										    <asp:textbox id="TxtLuogoNascita" CssClass="Input_Text" Runat="server" Width="250px"></asp:textbox>
									    </td>
								    </tr>
								    <!--quarta riga-->
								    <tr>
									    <td width="280">
										    <asp:label id="Label15" CssClass="Input_Label" Runat="server">Via</asp:label>
										    <br />
										    <asp:textbox id="TxtResVia" CssClass="Input_Text" Runat="server" Width="265px"></asp:textbox>
									    </td>
									    <td>
										    <asp:label id="Label16" CssClass="Input_Label" Runat="server">Civico</asp:label>
										    <br />
										    <asp:textbox id="TxtResCivico" CssClass="Input_Text_Right" Runat="server" Width="50px"></asp:textbox>
									    </td>
									    <td>
										    <asp:label id="Label17" CssClass="Input_Label" Runat="server">Esponente</asp:label>
										    <br />
										    <asp:textbox id="TxtResEsponente" CssClass="Input_Text" Runat="server" Width="50px"></asp:textbox>
									    </td>
									    <td Width="70">
										    <asp:label id="Label18" CssClass="Input_Label" Runat="server">Interno</asp:label>
										    <br />
										    <asp:textbox id="TxtResInterno" CssClass="Input_Text" Runat="server" Width="50px"></asp:textbox>
									    </td>
									    <td>
										    <asp:label id="Label19" CssClass="Input_Label" Runat="server">Scala</asp:label>
										    <br />
										    <asp:textbox id="TxtResScala" CssClass="Input_Text" Runat="server" Width="50px"></asp:textbox>
									    </td>
								    </tr>
								    <!--quinta riga-->
								    <tr>
									    <td width="280">
										    <asp:label id="Label20" CssClass="Input_Label" Runat="server">CAP</asp:label>
										    <br />
										    <asp:textbox id="TxtResCAP" CssClass="Input_Text_Right" Runat="server" Width="80px"></asp:textbox>
									    </td>
									    <td colspan="3" width="275">
										    <asp:label id="Label21" Runat="server" CssClass="Input_Label">Comune</asp:label>
										    <br />
										    <asp:textbox id="TxtResComune" CssClass="Input_Text" Runat="server" Width="250px"></asp:textbox>
									    </td>
									    <td>
										    <asp:label id="Label22" CssClass="Input_Label" Runat="server">Provincia</asp:label>
										    <br />
										    <asp:textbox id="TxtResPv" CssClass="Input_Text" Runat="server" Width="50px"></asp:textbox>
									    </td>
								    </tr>
				                    <!--Blocco Dati Famiglia-->
				                    <tr>
					                    <td colspan="6">
					                        <br />
		                                    <a title="Visualizza Dati Famiglia/Residenti" onclick="nascondi(this,'divFamiglia','Dati Famiglia/Residenti')" href="#" class="lstTabRow"  style="display:none;width:100%">Visualizza Dati Famiglia/Residenti</a>
					                        <div id="divFamiglia" runat="server" class="col-md-12">
										        <asp:Label CssClass="Legend" Runat="server" ID="LblResultFamiglia">Non sono presenti Dati per la Famiglia</asp:Label>
                                                <Grd:RibesGridView ID="GrdFamiglia" runat="server" BorderStyle="None" 
                                                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                    AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                                                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                                    <PagerSettings Position="Bottom"></PagerSettings>
                                                    <PagerStyle CssClass="CartListFooter" />
                                                    <RowStyle CssClass="CartListItem"></RowStyle>
                                                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Nominativo">
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle horizontalalign="Justify"></ItemStyle>
													        <ItemTemplate>
														        <asp:Label id="Label40" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.COGNOME") & " " & DataBinder.Eval(Container, "DataItem.NOME") %>'>
														        </asp:Label>
													        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="cod_fiscale" HeaderText="Cod.Fiscale">
                                                            <HeaderStyle HorizontalAlign="Left"></headerstyle>
                                                            <itemstyle horizontalalign="Justify"></itemstyle>
                                                        </asp:BoundField>
						                                <asp:TemplateField HeaderText="Data Nascita">
							                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							                                <ItemTemplate>
								                                <asp:Label id="lblDataNascitaGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATA_NASCITA")) %>'></asp:Label>
							                                </ItemTemplate>
						                                </asp:TemplateField>
                                                        <asp:BoundField DataField="LUOGO_NASCITA" HeaderText="Luogo Nascita">
                                                            <HeaderStyle HorizontalAlign="Left"></headerstyle>
                                                            <itemstyle horizontalalign="Justify"></itemstyle>
                                                        </asp:BoundField>
						                                <asp:TemplateField HeaderText="Data Validità">
							                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							                                <ItemTemplate>
								                                <asp:Label id="lblDatamorteGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATA_MORTE")) %>'></asp:Label>
							                                </ItemTemplate>
						                                </asp:TemplateField>
                                                        <asp:BoundField DataField="VIA" HeaderText="Indirizzo">
                                                            <HeaderStyle HorizontalAlign="Left"></headerstyle>
                                                            <itemstyle horizontalalign="Justify"></itemstyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DESCRIZIONE_POS" HeaderText="Parentela">
                                                            <HeaderStyle HorizontalAlign="Left"></headerstyle>
                                                            <itemstyle horizontalalign="Justify"></itemstyle>
                                                        </asp:BoundField>
						                                <asp:TemplateField HeaderText="Data Morte">
							                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
							                                <ItemTemplate>
								                                <asp:Label id="LblDataMov" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.LASTMOV")) %>'></asp:Label>
							                                </ItemTemplate>
						                                </asp:TemplateField>
                                                    </Columns>
                                                </Grd:RibesGridView>
					                        </div>
					                        <br />
					                    </td>
				                    </tr>				
							    </table>
						    </div>
					    </td>
				    </tr>
                    <!--Blocco Dati Tessere-->
				    <tr>
					    <td>
						    <div id="DivTessere" runat="server">
							    <table id="TblTessere" cellspacing="0" cellpadding="0" border="0" width="100%">
				                    <tr>
					                    <td>
						                    <asp:Label CssClass="lstTabRow" Runat="server" id="Label23">Dati Tessere</asp:Label>
						                    <asp:Label ID="Label33" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:Label>
						                    <asp:imagebutton id="LnkNewTessera" runat="server" ImageUrl="../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px" ToolTip="Nuova Tessera" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>&nbsp;
						                    <asp:Label CssClass="lstTabRow" Runat="server" id="Label7" Width="605px">&nbsp;</asp:Label>
						                    <asp:textbox id="TxtTessere" Runat="server" style="DISPLAY: none" Width="10px">-1</asp:textbox>
					                    </td>
				                    </tr>
								    <tr>
									    <td>
										    <asp:Label CssClass="Legend" Runat="server" ID="LblResultUITes">Non sono presenti Tessere</asp:Label>
                                            <Grd:RibesGridView ID="GrdTessere" runat="server" BorderStyle="None" 
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="false"
                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                                OnRowCommand="GrdRowCommand">
                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                <PagerStyle CssClass="CartListFooter" />
                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
											    <Columns>
												    <asp:BoundField DataField="sNumeroTessera" HeaderText="N.Tessera">
													    <HeaderStyle HorizontalAlign="Left"></headerstyle>
                                                        <ItemStyle Width="80px"></ItemStyle>
												    </asp:BoundField>
												    <asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
							                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													    <ItemStyle Width="300px"></ItemStyle>
													    <ItemTemplate>
														    <asp:Label id="Label35" runat="server" text='<%# FncGrd.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"), DataBinder.Eval(Container, "DataItem.scivico"), DataBinder.Eval(Container, "DataItem.sInterno"), DataBinder.Eval(Container, "DataItem.sEsponente"), DataBinder.Eval(Container, "DataItem.sScala"), DataBinder.Eval(Container, "DataItem.sfoglio"), DataBinder.Eval(Container, "DataItem.snumero"), DataBinder.Eval(Container, "DataItem.ssubalterno"))  %>'>Label</asp:Label>
													    </ItemTemplate>
												    </asp:TemplateField>
												    <asp:TemplateField HeaderText="Cat. Catastale">
							                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													    <ItemTemplate>
														    <asp:Label id="Label24" runat="server" text='<%# FncGrd.FormattaCatCatastale(DataBinder.Eval(Container, "DataItem.sfoglio"), DataBinder.Eval(Container, "DataItem.snumero"), DataBinder.Eval(Container, "DataItem.ssubalterno")) %>'>Label</asp:Label>
													    </ItemTemplate>
												    </asp:TemplateField>
												    <asp:TemplateField HeaderText="Data Inizio">
							                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													    <ItemStyle HorizontalAlign="Center"></ItemStyle>
													    <ItemTemplate>
														    <asp:Label id="Label34" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataInizio")) %>'>
														    </asp:Label>
													    </ItemTemplate>
												    </asp:TemplateField>
												    <asp:TemplateField HeaderText="Data Fine">
							                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													    <ItemStyle HorizontalAlign="Center"></ItemStyle>
													    <ItemTemplate>
														    <asp:Label id="Label25" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataFine")) %>'>
														    </asp:Label>
													    </ItemTemplate>
												    </asp:TemplateField>
												    <asp:BoundField DataField="sNote" HeaderText="Note">
													    <HeaderStyle HorizontalAlign="Left"></headerstyle>
                                                        <ItemStyle Width="150px"></ItemStyle>
												    </asp:BoundField>
												    <asp:TemplateField HeaderText="">
													    <HeaderStyle HorizontalAlign="Left"></headerstyle>
													    <itemstyle horizontalalign="Center"></itemstyle>
													    <itemtemplate>
                                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneHomeGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDUI") %>' alt=""/>
                                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneTessereGrd" CommandName="RowTessera" CommandArgument='<%# Eval("ID") %>' alt=""/>
														    <asp:HiddenField ID="hfIdTestata" runat="server" Value='<%# Eval("IDTESTATA") %>' />
                                                            <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("ID") %>' />
                                                            <asp:HiddenField ID="hfIdTessera" runat="server" Value='<%# Eval("ID") %>' />
                                                            <asp:HiddenField ID="hfIdUI" runat="server" Value='<%# Eval("IDUI") %>' />
                                                            <asp:HiddenField ID="hfIdDettaglioTestata" runat="server" Value='<%# Eval("IDDETTAGLIOTESTATA") %>' />
													    </itemtemplate>
												    </asp:TemplateField>
											    </Columns>
										    </Grd:RibesGridView>
									    </td>
								    </tr>
				                    <tr>
					                    <td><br /></td>
				                    </tr>
							    </table>
						    </div>
					    </td>
				    </tr>
				    <!--Blocco Dati UI-->
				    <tr>
					    <td>
						    <div id="DivImmobili" runat="server">
							    <table id="TblImmobili" cellspacing="0" cellpadding="0" border="0" width="100%">
				                    <tr>
					                    <td>
						                    <asp:Label CssClass="lstTabRow" Runat="server" id="Label29">Dati Immobili</asp:Label>
						                    <asp:Label ID="Label36" style="FONT-SIZE: 11px; COLOR: red; FONT-FAMILY: Verdana" Runat="server">*</asp:Label>
						                    <asp:imagebutton id="LnkNewUI" runat="server" ImageUrl="../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px" ToolTip="Nuovo Immobile" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>&nbsp;
						                    <asp:imagebutton id="LnkNewUIAnater" runat="server" ImageUrl="../../images/Bottoni/Listasel.png" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>&nbsp;
						                    <asp:Label CssClass="lstTabRow" Runat="server" id="Label37" Width="605px">&nbsp;</asp:Label>
						                    <asp:textbox id="TxtImmobili" Runat="server" style="DISPLAY: none" Width="10px">-1</asp:textbox>
						                    <asp:button id="CmdRibaltaUIAnater" style="DISPLAY: none" Runat="server"></asp:button>
					                    </td>
				                    </tr>
								    <tr>
									    <td>
										    <asp:Label CssClass="Legend" Runat="server" ID="LblResultUI">Non sono presenti unità immobiliari</asp:Label>
                                            <Grd:RibesGridView ID="GrdImmobili" runat="server" BorderStyle="None" 
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="true" PageSize="70"
                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                                OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                <PagerStyle CssClass="CartListFooter" />
                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
											    <Columns>
												    <asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
							                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													    <ItemStyle Width="300px"></ItemStyle>
													    <ItemTemplate>
														    <asp:Label id="Label35" runat="server" text='<%# FncGrd.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"), DataBinder.Eval(Container, "DataItem.scivico"), DataBinder.Eval(Container, "DataItem.sInterno"), DataBinder.Eval(Container, "DataItem.sEsponente"), DataBinder.Eval(Container, "DataItem.sScala"), DataBinder.Eval(Container, "DataItem.sfoglio"), DataBinder.Eval(Container, "DataItem.snumero"), DataBinder.Eval(Container, "DataItem.ssubalterno")) %>'>Label</asp:Label>
													    </ItemTemplate>
												    </asp:TemplateField>
												    <asp:TemplateField HeaderText="Data Inizio Occupazione">
							                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													    <ItemStyle HorizontalAlign="Center"></ItemStyle>
													    <ItemTemplate>
														    <asp:Label id="Label34" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.TDATAINIZIO")) %>'>
														    </asp:Label>
													    </ItemTemplate>
												    </asp:TemplateField>
												    <asp:TemplateField HeaderText="Data Fine Occupazione">
							                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													    <ItemStyle HorizontalAlign="Center"></ItemStyle>
													    <ItemTemplate>
														    <asp:Label id="Label25" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.TDATAFINE")) %>'>
														    </asp:Label>
													    </ItemTemplate>
												    </asp:TemplateField>
												    <asp:BoundField DataField="scatcatastale" HeaderText="Cat. Catastale">
							                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>                                         
													    <ItemStyle HorizontalAlign="Right"></ItemStyle>
												    </asp:BoundField>
												    <asp:BoundField DataField="NVANI" HeaderText="N.Vani">
							                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
												    </asp:BoundField>
												    <asp:BoundField DataField="NMQ" HeaderText="Tot.MQ" DataFormatString="{0:0.00}">
													    <HeaderStyle HorizontalAlign="Left"></headerstyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
												    </asp:BoundField>
												    <asp:BoundField DataField="NMQANATER" HeaderText="Tot.MQ Territorio" DataFormatString="{0:0.00}">
													    <HeaderStyle HorizontalAlign="Left"></headerstyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
												    </asp:BoundField>
												    <asp:BoundField DataField="SNOTEUI" HeaderText="Note">
							                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
													    <ItemStyle Width="200px"></ItemStyle>
												    </asp:BoundField>
												    <asp:TemplateField HeaderText="Sel.">
												        <ItemTemplate>
												            <asp:CheckBox ID="chkSel" runat="server" Checked="true" />
												        </ItemTemplate>
												    </asp:TemplateField>
												    <asp:TemplateField HeaderText="">
													    <HeaderStyle HorizontalAlign="Left"></headerstyle>
													    <itemstyle horizontalalign="Center"></itemstyle>
													    <itemtemplate>
                                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""/>
														    <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("ID") %>' />
                                                            <asp:HiddenField ID="hfIdDettaglioTestata" runat="server" Value='<%# Eval("IDDETTAGLIOTESTATA") %>' />
                                                            <asp:HiddenField ID="hfIdTestata" runat="server" Value='<%# Eval("IDTESTATA") %>' />
                                                            <asp:HiddenField ID="hfIdPadre" runat="server" Value='<%# Eval("IDPADRE") %>' />
                                                            <asp:HiddenField ID="hfGGTarsu" runat="server" Value='<%# Eval("NGGTARSU") %>' />
                                                            <asp:HiddenField ID="hfDataInserimento" runat="server" Value='<%# Eval("TDATAINSERIMENTO") %>' />
                                                            <asp:HiddenField ID="hfDataVariazione" runat="server" Value='<%# Eval("TDATAVARIAZIONE") %>' />
                                                            <asp:HiddenField ID="hfDataCessazione" runat="server" Value='<%# Eval("TDATACESSAZIONE") %>' />
                                                            <asp:HiddenField ID="hfOperatore" runat="server" Value='<%# Eval("SOPERATORE") %>' />
                                                            <asp:HiddenField ID="hfFoglio" runat="server" Value='<%# Eval("sfoglio") %>' />
                                                            <asp:HiddenField ID="hfNumero" runat="server" Value='<%# Eval("snumero") %>' />
                                                            <asp:HiddenField ID="hfSub" runat="server" Value='<%# Eval("ssubalterno") %>' />
													    </itemtemplate>
												    </asp:TemplateField>
											    </Columns>
										    </Grd:RibesGridView>
									    </td>
								    </tr>
				                    <tr>
					                    <td><br />
					                    </td>
				                    </tr>
							    </table>
						    </div>
					    </td>
				    </tr>
				    <!--Blocco Dati Provenienza-->
				    <tr>
					    <td>
						    <div id="DivProvenienza" runat="server">
							    <table id="TblProvenienza" cellspacing="0" cellpadding="0" border="0" width="100%">
				                    <tr>
					                    <td colspan="2">
						                    <asp:Label CssClass="lstTabRow" Runat="server" id="Label41" Width="100%">Dati Provenienza</asp:Label>
					                    </td>
				                    </tr>
								    <tr>
									    <td style="width:200px">
										    <asp:Label CssClass="Input_Label" Runat="server" id="Label27">Provenienza</asp:Label>
										    <br />
										    <asp:DropDownList ID="DdlTipoDich" CssClass="Combo" Runat="server"></asp:DropDownList>
									    </td>
									    <td>
										    <asp:Label CssClass="Input_Label" Runat="server" id="Label28">Note Dichiarazione</asp:Label>
										    <br />
										    <asp:TextBox ID="TxtNoteDich" CssClass="Input_Text" Runat="server" width="500px" Height="32px"></asp:TextBox>
									    </td>
								    </tr>
							    </table>
						    </div>
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
            <div id="divCalcolo" runat="server" style="display:none;">
                <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden">
                    <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table1">
                        <tr valign="top">
                            <td align="left">
                                <span class="ContentHead_Title" id="infoEntePunt" style="width: 400px"></span>
                            </td>
                            <td align="right" rowspan="2">
                                <input class="Bottone BottoneCancella hidden" id="CancellaCartellazione" title="Elimina Ruolo" onclick="document.getElementById('CmdDeleteElab').click();" type="button" name="CancellaCartellazione">
                                <input class="Bottone BottoneWord" id="ElaborazioneDocumenti" title="Elabora Documenti" onclick="DivAttesa.style.display = ''; divAvviso.style.display = 'none'; document.getElementById('CmdDocumenti').click()" type="button" name="ElaborazioneDocumenti">
                                <input class="Bottone BottoneCalcolo" id="ElaboraRuolo" title="Calcola" onclick="CalcoloPuntuale()" type="button" name="ElaboraRuolo">
                            	<input class="Bottone BottoneAnnulla" id="Cancel" title="Chiudi" onclick="document.getElementById('hfCalcoloPuntuale').value=0;divCalcolo.style.display = 'none';divDichiarazione.style.display = '';parent.Comandi.location.href='ComandiGestDichiarazioni.aspx?sProvenienza=N';" type="button" name="Cancel"> 
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <span class="NormalBold_title" id="infoPunt" runat="server" style="width: 400px">Calcolo puntuale</span>
                            </td>
                        </tr>
                    </table>
                </div>
                &nbsp;
               <div id="divParamRuolo" class="col-md-8">
                    <div class="col-md-2">
                        <p>
                            <label class="Input_Label">Anno Ruolo</label>
                        </p>
                        <asp:DropDownList ID="ddlAnno" runat="server" CssClass="Input_Text col-md-8"></asp:DropDownList>
                    </div>
                    <div id="PercentTariffe" class="col-md-1">
                        <p>
                            <label class="Input_Label">%</label>
                        </p>
                        <asp:TextBox ID="TxtPercentTariffe" runat="server" CssClass="Input_Text col-md-12">100</asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <p>
                            <label class="Input_Label">Soglia minima</label>
                        </p>
                        <asp:TextBox ID="txtSogliaMinima" runat="server" CssClass="Input_Text_right col-md-6">0</asp:TextBox>
                    </div>
                    <div id="DataConf" class="col-md-3 hidden">
                        <p>
                            <label class="Input_Label">Conferimenti Dal-Al</label>
                        </p>
                        <asp:TextBox runat="server" ID="txtInizioConf" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                        <asp:TextBox runat="server" ID="txtFineConf" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <p>
                            <label class="Input_Label">Scadenza</label>
                        </p>
                        <asp:TextBox runat="server" ID="txtScadenza" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                    </div>
                    <div class="col-md-1 hidden">
                        <asp:CheckBox ID="chkSimulazione" runat="server" CssClass="Input_CheckBox_NoBorder col-md-12" Text="Simulazione" />
                    </div>
                </div>
				<table class="col-md-12" cellSpacing="1" cellPadding="1" border="0">
 				    <tr id="TRPlainAnagPunt">
				        <td>
				            <iframe id="ifrmAnagPunt" runat="server" src="../../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				        </td>
				    </tr>
                </table>
                <div id="divCalcolato">
                    <div id="divAvviso">
                        <div style="border:2px solid;" class="col-md-6">
						    <table class="col-md-12" cellSpacing="1" cellPadding="1" border="0">
							    <tr>
								    <td class="DettagliContribuente" colspan="2">
									    <asp:label id="LblDatiAvviso" runat="server" CssClass="DettagliContribuente">Label</asp:label>
								    </td>
							    </tr>
							    <tr>
								    <td class="DettagliContribuente col-md-2">
									    <asp:label id="Label38" runat="server" CssClass="DettagliContribuente">Emesso</asp:label>
								    </td>
								    <td class="DettagliContribuente col-md-4" align="right"">
									    <asp:label id="LblEmesso" runat="server" CssClass="DettagliContribuente">Label</asp:label>
								    </td>
							    </tr>
						    </table>
                        </div>
                        <div class="col-md-12">
                            <table class="col-md-12">
				                <!--Blocco Dati Tessere-->
				                <tr>
					                <td colspan="2">
					                    <div id="divCalcTessere" runat="server" class="hidden">
						                    <asp:Label CssClass="lstTabRow" Runat="server" id="Label39">Dati Parte Conferimenti</asp:Label>
						                    <asp:Label CssClass="lstTabRow" Runat="server" id="Label42" Width="590px">&nbsp;</asp:Label>
						                    <asp:Label CssClass="Legend" Runat="server" ID="LblResultTessere">Non sono presenti Tessere</asp:Label><br />
                                            <Grd:RibesGridView ID="grdCalcTessere" runat="server" BorderStyle="None" 
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
								                    <asp:BoundField DataField="sNumeroTessera" HeaderText="N.Tessera">
									                    <ItemStyle Width="80px"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sCodUtente" HeaderText="N.Utente">
									                    <ItemStyle Width="80px"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:TemplateField HeaderText="Data Rilascio">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label id="Label34" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataRilascio")) %>'>
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="Data Cessazione">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label id="Label25" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataCessazione")) %>'>
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="N.Conferimenti">
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label runat="server" text='<%# FncGrd.FormattaConferimenti("N", DataBinder.Eval(Container, "DataItem.oPesature")) %>' id="Label8">
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="Tot.Volume">
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label runat="server" text='<%# FncGrd.FormattaConferimenti("P", DataBinder.Eval(Container, "DataItem.oPesature")) %>' id="Label9">
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="Agev.">
									                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:ImageButton id="Imagebutton1" runat="server" Height="15px" Width="15px" CommandName="Edit" ImageUrl='<%# FncGrd.FormattaRidDet(DataBinder.Eval(Container, "DataItem.oRiduzioni")) %>'>
										                    </asp:ImageButton>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="">
									                    <headerstyle horizontalalign="Center"></headerstyle>
									                    <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									                    <itemtemplate>
										                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
					                                        <asp:HiddenField runat="server" ID="hfIDTESTATA" Value='<%# Eval("IDTESTATA") %>' />
                                                            <asp:HiddenField runat="server" ID="hfIDTESSERA" Value='<%# Eval("IDTESSERA") %>' />
									                    </itemtemplate>
								                    </asp:TemplateField>
							                    </Columns>
						                    </Grd:RibesGridView>
					                    </div>
					                </td>
				                </tr>
				                <!--Dati Articoli-->
				                <tr>
					                <td colspan="2">
						                <asp:Label CssClass="lstTabRow" Runat="server" id="Label43">Dati Parte Fissa</asp:Label>&nbsp;
						                <asp:Label CssClass="lstTabRow" Runat="server" id="Label44" Width="605px">&nbsp;</asp:Label>
					                </td>
				                </tr>
				                <tr>
					                <td colspan="2">
						                <asp:label id="LblResultArticoli" Runat="server" CssClass="Legend">Non sono presenti Articoli</asp:label>
                                        <Grd:RibesGridView ID="GrdArticoli" runat="server" BorderStyle="None" 
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
								                <asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
									                <ItemStyle Width="300px"></ItemStyle>
									                <ItemTemplate>
										                <asp:Label id="Label13" runat="server" text='<%# FncGrd.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"), DataBinder.Eval(Container, "DataItem.scivico"), DataBinder.Eval(Container, "DataItem.sInterno"), DataBinder.Eval(Container, "DataItem.sEsponente"), DataBinder.Eval(Container, "DataItem.sScala"), DataBinder.Eval(Container, "DataItem.sfoglio"), DataBinder.Eval(Container, "DataItem.snumero"), DataBinder.Eval(Container, "DataItem.ssubalterno")) %>'>Label</asp:Label>
									                </ItemTemplate>
								                </asp:TemplateField>
								                <asp:TemplateField HeaderText="Cat.">
									                <ItemStyle Width="250px"></ItemStyle>
									                <ItemTemplate>
										                <asp:Label runat="server" Text='<%# FncGrd.FormattaCategoria(DataBinder.Eval(Container, "DataItem.sCategoria"), DataBinder.Eval(Container, "DataItem.sDescrCategoria"))%>' ID="Label2">
										                </asp:Label>
									                </ItemTemplate>
								                </asp:TemplateField>
								                <asp:BoundField DataField="impTariffa" HeaderText="Tariffa " DataFormatString="{0:0.000000}">
									                <ItemStyle Width="50px" HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundField>
								                <asp:BoundField DataField="Nmq" HeaderText="MQ" DataFormatString="{0:N}">
									                <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundField>
								                <asp:BoundField DataField="Nbimestri" HeaderText="Tempo">
									                <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundField>
								                <asp:BoundField DataField="impNetto" HeaderText="Imp." DataFormatString="{0:N}">
									                <ItemStyle Width="50px" HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundField>
								                <asp:TemplateField HeaderText="Rid.">
									                <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                <ItemTemplate>
										                <asp:ImageButton id="Imagebutton2" runat="server" Height="15px" Width="15px" CommandName="Edit" ImageUrl='<%# FncGrd.FormattaRidDet(DataBinder.Eval(Container, "DataItem.impRiduzione")) %>'></asp:ImageButton>
									                </ItemTemplate>
								                </asp:TemplateField>
								                <asp:TemplateField HeaderText="Esenz.">
									                <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                <ItemTemplate>
										                <asp:ImageButton id="Imagebutton3" runat="server" Height="15px" Width="15px" CommandName="Edit" ImageUrl='<%# FncGrd.FormattaRidDet(DataBinder.Eval(Container, "DataItem.impDetassazione")) %>'></asp:ImageButton>
                                                        <asp:HiddenField runat="server" ID="hfid" Value='<%# Eval("Id") %>' />
                                                        <asp:HiddenField runat="server" ID="hfIdContribuente" Value='<%# Eval("IdContribuente") %>' />
                                                        <asp:HiddenField runat="server" ID="hfTipoPartita" Value='<%# Eval("TipoPartita") %>' />
									                </ItemTemplate>
								                </asp:TemplateField>
							                </Columns>
						                </Grd:RibesGridView>
						                <br />
					                </td>
				                </tr>
				                <!--Dati Dettaglio Avviso e Rate-->
				                <tr>
					                <td><asp:label id="Label46" Width="100%" CssClass="lstTabRow" Runat="server">Dati Dettaglio Avviso</asp:label></td>
					                <td><asp:label id="Label47" Width="100%" CssClass="lstTabRow" Runat="server">Dati Rate</asp:label></td>
				                </tr>
				                <tr>
					                <td valign="top">
						                <asp:label id="LblResultDettVoci" Runat="server" CssClass="Legend">Non è presente il Dettaglio dell'Avviso</asp:label>
                                        <Grd:RibesGridView ID="GrdDettaglioVoci" runat="server" BorderStyle="None" 
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
								                <asp:BoundField DataField="sDescrizione" HeaderText="Descrizione">
									                <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                </asp:BoundField>
								                <asp:BoundField DataField="impDettaglio" HeaderText="Importo " DataFormatString="{0:N}">
									                <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundField>
							                </Columns>
						                </Grd:RibesGridView>
					                </td>
					                <td valign="top">
						                <asp:label id="LblResultRate" Runat="server" CssClass="Legend">Non sono presenti Rate</asp:label>
                                        <Grd:RibesGridView ID="GrdRate" runat="server" BorderStyle="None" 
                                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                            <PagerSettings Position="Bottom"></PagerSettings>
                                            <PagerStyle CssClass="CartListFooter" />
                                            <RowStyle CssClass="CartListItem"></RowStyle>
                                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                <Columns>
								                <asp:BoundField DataField="sNRata" HeaderText="N.Rata">
									                <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                </asp:BoundField>
								                <asp:BoundField DataField="sDescrRata" HeaderText="Descrizione">
									                <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                </asp:BoundField>
								                <asp:TemplateField HeaderText="Data Scadenza">
									                <ItemStyle Width="50px"></ItemStyle>
									                <ItemTemplate>
										                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataScadenza"))%>' ID="Label3">
										                </asp:Label>
									                </ItemTemplate>
								                </asp:TemplateField>
								                <asp:BoundField DataField="impRata" HeaderText="Importo " DataFormatString="{0:N}">
									                <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundField>
							                </Columns>
						                </Grd:RibesGridView>
					                </td>
				                </tr>
                            </table>
                        </div>
                    </div>
                    <div id="divStampa" class="col-md-12 classeFiledSetRicerca" style="display:none;">
                        <iframe runat="server" id="loadStampa" name="loadStampa" frameborder="0" width="100%" height="250px" src="../../../aspvuota.aspx"></iframe>
                    </div>
                </div>
            </div>
            <div id="DivAttesa" runat="server" style="z-index: 103; position: absolute;display:none;">
                <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                <div class="BottoneClessidra">&nbsp;</div>
                <div class="Legend">Attendere Prego...<asp:Label ID="LblAvanzamento" runat="server"></asp:Label></div>
            </div>
			<asp:TextBox ID="TxtIdDich" Runat="server" style="DISPLAY: none">-1</asp:TextBox>
			<asp:TextBox ID="TxtIdTestata" Runat="server" style="DISPLAY: none">-1</asp:TextBox>
			<asp:TextBox ID="TxtIsInRuolo" Runat="server" style="DISPLAY: none">-1</asp:TextBox>
			<asp:Button ID="CmdPopUpUI" Runat="server" style="DISPLAY: none"></asp:Button>
			<asp:Button ID="CmdSalvaDati" Runat="server" style="DISPLAY: none"></asp:Button>
			<asp:Button ID="CmdClearDatiDich" Runat="server" style="DISPLAY: none"></asp:Button>
			<asp:Button ID="CmdModDich" Runat="server" style="DISPLAY: none"></asp:Button>
			<asp:Button ID="CmdDeleteDich" Runat="server" style="DISPLAY: none"></asp:Button>
			<asp:Button ID="CmdSaveDatiDich" Runat="server" style="DISPLAY: none"></asp:Button>
			<asp:Button ID="CmdStampa" Runat="server" style="DISPLAY: none"></asp:Button>
			<asp:Button ID="btnSalvaDichiarazione" Runat="server" style="DISPLAY: none"></asp:Button>
			<asp:button id="CmdGIS" style="DISPLAY: none" Runat="server"></asp:button>
            <asp:Button ID="CmdCalcola" runat="server" Style="display: none" Text="Calcola"></asp:Button>
            <asp:Button ID="CmdDocumenti" runat="server" Style="display: none" Text="Elaborazione Documenti"></asp:Button>
            <asp:Button ID="CmdDeleteElab" runat="server" Style="display: none" Text="Elimina Ruolo"></asp:Button>
            <asp:Button ID="CmdDownload" runat="server" Style="display: none" />
            <asp:Button ID="CmdUpload" runat="server" Style="display: none" />
            <asp:Button ID="CmdDovuto" runat="server" Style="display:none" />
            <asp:HiddenField ID="hfCalcoloPuntuale" runat="server" value="0"/>
		</form>
	</body>
</HTML>

