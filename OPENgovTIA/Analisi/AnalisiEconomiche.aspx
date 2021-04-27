<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AnalisiEconomiche.aspx.vb" Inherits="OPENgovTIA.AnalisiEconomiche" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title></title>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js?newversion"></script>
		<script type="text/javascript">
			//QUESTA FUNZIONE FA IN MODO CHE QUANDO VIENE PREMUTO IL TASTO INVIO IL FORM VENGA SPEDITO
			function ciao()
			{
				if (window.event.keyCode==13){
					LoadAnalisi()
				}
			}
			document.onkeydown = ciao				
			
			function LoadAnalisi()
			{
			    document.getElementById('DivAttesa').style.display = ""
			    document.getElementById('CmdAnalisi').click()
			}
			function ShowDettCategoria() {
			    if (document.getElementById('DivDettCategoria').style.display == "") {
			        document.getElementById('DivDettCategoria').style.display = "none"
			    }
			    else {
			        document.getElementById('DivDettCategoria').style.display = ""
			    }
			}			
			function LoadGrafico() {
			    document.getElementById('DivAttesa').style.display = ""
			    document.getElementById('DivAnalisi').style.display = "none"
			    document.getElementById('CmdGrafico').click()
			}
			function LoadStampa() {
			    document.getElementById('CmdStampa').click()
			}
		</script>
    </head>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
		    <div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: hidden">
			    <table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				    <tr>
					    <td style="WIDTH: 464px; HEIGHT: 20px" align="left">
						    <span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							    <asp:Label id="lblTitolo" runat="server"></asp:Label>
						    </span>
					    </td>
					    <td align="right" width="800" colSpan="2" rowSpan="2">
						    <input class="Bottone BottoneExcel" id="Stampa" title="Stampa" onclick="LoadStampa()" type="button" name="Stampa"> 
						    <input class="Bottone BottoneGrafico" id="Grafico" title="Visualizza grafico" onclick="LoadGrafico()" type="button" name="Grafico"> 
						    <input class="Bottone BottoneRicerca" id="Ricerca" title="Ricerca" onclick="LoadAnalisi()" type="button" name="Ricerca">
					    </td>
				    </tr>
				    <tr>
					    <td style="WIDTH: 463px" align="left">
						    <span class="NormalBold_title" id="info" runat="server" style="WIDTH: 400px; HEIGHT: 20px">
							     - Analisi - Economiche</span>
					    </td>
				    </tr>
			    </table>
		    </div>
		    &nbsp;
		    <div id="divSearch">
			    <table id="tabEsterna" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 50px" cellSpacing="1" cellPadding="1" width="98%" border="0">
				    <tr>
					    <td>
						    <fieldset class="FiledSetRicerca" style="WIDTH: 100%">
							    <legend class="Legend">Inserimento filtri di ricerca</legend>
							    <table width="100%">
								    <tr>
									    <td><asp:label id="Label1" Runat="server" CssClass="Input_Label">Anno</asp:label><br />
										    <asp:dropdownlist id="DdlAnno" Runat="server" CssClass="Input_Text" AutoPostBack="True"></asp:dropdownlist>
									    </td>
									    <td><asp:label id="Label2" Runat="server" CssClass="Input_Label">Tipo Ruolo</asp:label><br />
										    <asp:dropdownlist id="DdlTipoRuolo" Runat="server" CssClass="Input_Text" AutoPostBack="True"></asp:dropdownlist>
									    </td>
									    <td><asp:label id="Label4" Runat="server" CssClass="Input_Label">Data Accredito DAL</asp:label><br />
										    <asp:textbox id="TxtValutaDal" Runat="server" CssClass="Input_Text" Width="120px"></asp:textbox>
									    </td>
									    <td><asp:label id="Label5" Runat="server" CssClass="Input_Label">AL</asp:label><br />
										    <asp:textbox id="TxtValutaAl" Runat="server" CssClass="Input_Text" Width="120px"></asp:textbox>
									    </td>
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
		                    <div id="DivAnalisi" runat="server">
			                    <table cellSpacing="0" cellPadding="0" width="100%" border="0">
				                    <tr>
					                    <td>
						                    <!--Riepilogo Ruolo/Avvisi-->
						                    <fieldset class="FiledSetRicerca" style="WIDTH: 100%">
							                    <table height="50" width="100%">
								                    <!--Intestazione-->
								                    <tr>
									                    <td colSpan="5"><asp:label id="Label3" Runat="server" CssClass="Legend">Dati Ruolo/Avvisi</asp:label></td>
								                    </tr>
								                    <!--Emesso-->
								                    <tr>
									                    <td><asp:label id="Label52" Runat="server" CssClass="Input_Label">N. Articoli</asp:label></td>
									                    <td align="right"><asp:label id="LblNArticoli" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td width="20"></td>
									                    <td align="right"><asp:label id="Label55" Runat="server" CssClass="Input_Label">Totale Importo a Ruolo €</asp:label></td>
									                    <td align="right"><asp:label id="LblTotImpRuolo" Runat="server" CssClass="Input_Label"></asp:label></td>
								                    </tr>
								                    <tr>
									                    <td><asp:label id="Label6" Runat="server" CssClass="Input_Label">N. Avvisi</asp:label></td>
									                    <td align="right"><asp:label id="LblNAvvisi" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td width="20"></td>
									                    <td align="right"><asp:label id="Label7" Runat="server" CssClass="Legend">Totale Importo Avvisi €</asp:label></td>
									                    <td align="right"><asp:label id="LblTotImpAvvisi" Runat="server" CssClass="Legend"></asp:label></td>
								                    </tr>
								                    <!--Pagato-->
								                    <tr>
									                    <td><br />
									                    </td>
								                    </tr>
								                    <!--Pagati Totalmente-->
								                    <tr>
									                    <td><asp:label id="Label8" Runat="server" CssClass="Input_Label">N. Avvisi Pagati Totalmente</asp:label></td>
									                    <td align="right"><asp:label id="LblNPagTot" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td width="20"></td>
									                    <td align="right"><asp:label id="Label9" Runat="server" CssClass="Input_Label">Totale Pagato €</asp:label></td>
									                    <td align="right"><asp:label id="LblImpPagTot" Runat="server" CssClass="Input_Label"></asp:label></td>
								                    </tr>
								                    <!--Pagati Parzialmente-->
								                    <tr>
									                    <td><asp:label id="Label11" Runat="server" CssClass="Input_Label">N. Avvisi Pagati Parzialmente</asp:label></td>
									                    <td align="right"><asp:label id="LblNPagParz" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td width="20"></td>
									                    <td align="right"><asp:label id="Label13" Runat="server" CssClass="Input_Label">Totale Acconti €</asp:label></td>
									                    <td style="BORDER-BOTTOM: #00008b 1px solid" align="right"><asp:label id="LblImpPagParz" Runat="server" CssClass="Input_Label"></asp:label></td>
								                    </tr>
								                    <!--Totale Incassato-->
								                    <tr>
									                    <td align="right" colSpan="4"><asp:label id="Label15" Runat="server" CssClass="Legend">Totale Incassato €</asp:label></td>
									                    <td align="right"><asp:label id="LblTotIncassato" Runat="server" CssClass="Legend"></asp:label></td>
								                    </tr>
							                    </table>
							                    <!--Riepilogo Generale-->
							                    <table width="100%">
								                    <!--Intestazione-->
								                    <tr>
									                    <td colSpan="7"><asp:label style="TOP: 150px; LEFT: 3px" Runat="server" CssClass="Legend">Riepilogo Generale</asp:label></td>
								                    </tr>
								                    <tr>
									                    <td width="100"><asp:label id="Label10" Runat="server" CssClass="Input_Label">N.Utenti</asp:label></td>
									                    <td align="center" width="250"><asp:label id="Label12" Runat="server" CssClass="Input_Label">Emesso Avvisi €</asp:label></td>
									                    <td align="center" width="15"></td>
									                    <td align="center" width="100"><asp:label id="Label16" Runat="server" CssClass="Input_Label">Incassato €</asp:label></td>
									                    <td align="center" width="15"></td>
									                    <td align="center" width="100"><asp:label id="Label18" Runat="server" CssClass="Input_Label">Insoluto €</asp:label></td>
									                    <td align="center" width="120"><asp:label id="Label19" Runat="server" CssClass="Legend">% Insoluto</asp:label></td>
								                    </tr>
								                    <tr>
									                    <td align="center"><asp:label id="LblNUtenti" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td align="center"><asp:label id="LblEmesso" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td align="center"><asp:label id="Label17" Runat="server" CssClass="Input_Label">-</asp:label></td>
									                    <td align="center"><asp:label id="LblIncassato" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td align="center"><asp:label id="Label39" Runat="server" CssClass="Input_Label">=</asp:label></td>
									                    <td align="center"><asp:label id="LblInsoluto" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td align="center"><asp:label id="LblPercentualeInsoluto" Runat="server" CssClass="Legend"></asp:label></td>
									                    <td><asp:textbox id="TxtIncassato" Runat="server" CssClass="Legend hidden" Width="0">0</asp:textbox><asp:textbox id="TxtInsoluto" Runat="server" CssClass="Legend hidden" Width="0">0</asp:textbox></td>
								                    </tr>
								                    <tr>
									                    <td width="100"><asp:label ID="Label53" Runat="server" CssClass="Legend">Maggiorazione</asp:label></td>
									                    <td align="center" width="250"><asp:label ID="Label57" Runat="server" CssClass="Input_Label">Emesso  €</asp:label></td>
									                    <td align="center" width="15"></td>
									                    <td align="center" width="100"><asp:label ID="Label60" Runat="server" CssClass="Input_Label">Incassato €</asp:label></td>
									                    <td align="center" width="15"></td>
									                    <td align="center" width="100"><asp:label ID="Label63" Runat="server" CssClass="Input_Label">Insoluto €</asp:label></td>
									                    <td align="center" width="120"><asp:label ID="Label65" Runat="server" CssClass="Legend">% Insoluto</asp:label></td>
								                    </tr>
								                    <tr>
									                    <td align="center"></td>
									                    <td align="center"><asp:label id="LblEmessoStat" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td align="center"><asp:label id="Label66" Runat="server" CssClass="Input_Label">-</asp:label></td>
									                    <td align="center"><asp:label id="LblIncassatoStat" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td align="center"><asp:label id="Label67" Runat="server" CssClass="Input_Label">=</asp:label></td>
									                    <td align="center"><asp:label id="LblInsolutoStat" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td align="center"><asp:label id="LblPercentualeInsolutoStat" Runat="server" CssClass="Legend"></asp:label></td>
									                    <td><asp:textbox id="TxtIncassatoStat" Runat="server" CssClass="Legend hidden" Width="0">0</asp:textbox><asp:textbox id="TxtInsolutoStat" Runat="server" CssClass="Legend hidden" Width="0">0</asp:textbox></td>
								                    </tr>
							                    </table>
						                    </fieldset>
					                    </td>
				                    </tr>
				                    <tr>
					                    <td><br />
						                    <!--Dettaglio Emesso/Incassato-->
						                    <fieldset class="FiledSetRicerca" style="WIDTH: 100%">
							                    <table width="100%">
								                    <tr>
									                    <td colSpan="3">
										                    <!--Intestazione--><asp:label id="Label20" Runat="server" CssClass="Legend">Dettaglio Emesso</asp:label>
									                    </td>
									                    <td width="10"></td>
									                    <td colSpan="3">
										                    <!--Intestazione--><asp:label id="Label27" Runat="server" CssClass="Legend">Dettaglio Incassato</asp:label>
									                    </td>
								                    </tr>
								                    <tr>
									                    <td colspan="7">
										                    <!--<asp:imagebutton id="LnkOpenDettCategoria" OnClick="ShowDettCategoria()" ToolTip="Dettaglio Imposta per Categoria Tariffaria" CausesValidation="False" imagealign="Bottom" ImageUrl="../../../images/Bottoni/Listasel.gif"></asp:imagebutton>-->
										                    <A title="Dettaglio Imposta per Categoria Tariffaria" class="Input_Label" onclick="ShowDettCategoria()"
											                    href="#">Dettaglio Imposta per Categoria Tariffaria &gt;&gt;</A>
										                    <div id="DivDettCategoria" style="PADDING-BOTTOM:0px; MARGIN:0px; PADDING-LEFT:0px; PADDING-RIGHT:0px; DISPLAY:none; PADDING-TOP:0px">
											                    <table width="100%" height="100%">
												                    <tr>
													                    <td valign="top" style="width:50%">
                                                                            <Grd:RibesGridView ID="GrdEmessoDettCategoria" runat="server" BorderStyle="None" 
                                                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10">
                                                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                                                <PagerStyle CssClass="CartListFooter" />
                                                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
															                    <Columns>
																                    <asp:BoundField DataField="descrizione" HeaderText="Categoria"></asp:BoundField>
																                    <asp:BoundField DataField="importo" HeaderText="Imp. " DataFormatString="{0:0.00}">
																	                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
																                    </asp:BoundField>
																                    <asp:BoundField DataField="importopv" HeaderText="Imp.PV " DataFormatString="{0:0.00}">
																	                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
																                    </asp:BoundField>
															                    </Columns>
														                    </Grd:RibesGridView>
														                    <br />
													                    </td>
													                    <td></td>
													                    <td valign="top" style="width:50%">
                                                                            <Grd:RibesGridView ID="GrdIncassatoDettCategoria" runat="server" BorderStyle="None" 
                                                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10">
                                                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                                                <PagerStyle CssClass="CartListFooter" />
                                                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
															                    <Columns>
																                    <asp:BoundField DataField="descrizione" HeaderText="Categoria"></asp:BoundField>
																                    <asp:BoundField DataField="importo" HeaderText="Imp. " DataFormatString="{0:0.00}">
																	                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
																                    </asp:BoundField>
																                    <asp:BoundField DataField="importopv" HeaderText="Imp.PV " DataFormatString="{0:0.00}">
																	                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
																                    </asp:BoundField>
															                    </Columns>
                                                                            </Grd:RibesGridView>
														                    <br />
													                    </td>
												                    </tr>
											                    </table>
										                    </div>
									                    </td>
								                    </tr>
								                    <tr>
									                    <!--Imposta Emesso-->
									                    <td><asp:label id="Label22" Runat="server" CssClass="Input_Label">Imposta €</asp:label></td>
									                    <td align="right" width="20%"><asp:label id="LblEmessoImposta" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label14" Runat="server" CssClass="Input_Label">+</asp:label></td>
									                    <td></td>
									                    <!--Imposta Incassato-->
									                    <td><asp:label id="Label29" Runat="server" CssClass="Input_Label">Imposta €</asp:label></td>
									                    <td align="right" width="20%"><asp:label id="LblIncassatoImposta" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label44" Runat="server" CssClass="Input_Label">+</asp:label></td>
								                    </tr>
								                    <tr>
									                    <!--Conferimenti Emesso-->
									                    <td><asp:label id="Label38" Runat="server" CssClass="Input_Label">Conferimenti €</asp:label></td>
									                    <td align="right"><asp:label id="LblEmessoConferimenti" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label70" Runat="server" CssClass="Input_Label">+</asp:label></td>
									                    <td></td>
									                    <!--Conferimenti Incassato-->
									                    <td><asp:label id="Label71" Runat="server" CssClass="Input_Label">Conferimenti €</asp:label></td>
									                    <td align="right"><asp:label id="LblIncassatoConferimenti" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label73" Runat="server" CssClass="Input_Label">+</asp:label></td>
								                    </tr>
								                    <tr>
									                    <!--Sanzioni Emesso-->
									                    <td><asp:label id="Label23" Runat="server" CssClass="Input_Label">Sanzioni €</asp:label></td>
									                    <td align="right"><asp:label id="LblEmessoSanzioni" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label21" Runat="server" CssClass="Input_Label">+</asp:label></td>
									                    <td></td>
									                    <!--Sanzioni Incassato-->
									                    <td><asp:label id="Label30" Runat="server" CssClass="Input_Label">Sanzioni €</asp:label></td>
									                    <td align="right"><asp:label id="LblIncassatoSanzioni" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label45" Runat="server" CssClass="Input_Label">+</asp:label></td>
								                    </tr>
								                    <tr>
									                    <!--Interessi Emesso-->
									                    <td><asp:label id="Label24" Runat="server" CssClass="Input_Label">Interessi €</asp:label></td>
									                    <td style="BORDER-BOTTOM: #00008b 1px solid" align="right"><asp:label id="LblEmessoInteressi" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label25" Runat="server" CssClass="Input_Label">=</asp:label></td>
									                    <td></td>
									                    <!--Interessi Incassato-->
									                    <td><asp:label id="Label28" Runat="server" CssClass="Input_Label">Interessi €</asp:label></td>
									                    <td style="BORDER-BOTTOM: #00008b 1px solid" align="right"><asp:label id="LblIncassatoInteressi" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label43" Runat="server" CssClass="Input_Label">=</asp:label></td>
								                    </tr>
								                    <tr>
									                    <!--Imposta Emesso-->
									                    <td><asp:label Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td align="right" width="20%"><asp:label id="LblEmessoRuolo" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label Runat="server" CssClass="Input_Label">+</asp:label></td>
									                    <td></td>
									                    <!--Imposta Incassato-->
									                    <td><asp:label id="Label32" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td align="right" width="20%"><asp:label id="LblIncassatoRuolo" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label35" Runat="server" CssClass="Input_Label">+</asp:label></td>
								                    </tr>
								                    <tr>
									                    <!--Addizionale EX ECA 10% Emesso-->
									                    <td><asp:label Runat="server" CssClass="Input_Label">Addizionale EX ECA 10% €</asp:label></td>
									                    <td align="right"><asp:label id="LblEmessoECA" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label41" Runat="server" CssClass="Input_Label">+</asp:label></td>
									                    <td></td>
									                    <!--Addizionale EX ECA 10% Incassato-->
									                    <td><asp:label id="Label42" Runat="server" CssClass="Input_Label">Addizionale EX ECA 10% €</asp:label></td>
									                    <td align="right"><asp:label id="LblIncassatoECA" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label49" Runat="server" CssClass="Input_Label">+</asp:label></td>
								                    </tr>
								                    <tr>
									                    <!--Tributo EX ECA Emesso-->
									                    <td><asp:label id="Label51" Runat="server" CssClass="Input_Label">Tributo EX ECA €</asp:label></td>
									                    <td align="right"><asp:label id="LblEmessoMECA" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label54" Runat="server" CssClass="Input_Label">+</asp:label></td>
									                    <td></td>
									                    <!--Tributo EX ECA Incassato-->
									                    <td><asp:label id="Label56" Runat="server" CssClass="Input_Label">Tributo EX ECA €</asp:label></td>
									                    <td align="right"><asp:label id="LblIncassatoMECA" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label58" Runat="server" CssClass="Input_Label">+</asp:label></td>
								                    </tr>
								                    <tr>
									                    <!--Aggio Per Ente Emesso-->
									                    <td><asp:label id="Label50" Runat="server" CssClass="Input_Label">Aggio Per Ente €</asp:label></td>
									                    <td align="right"><asp:label id="LblEmessoAggio" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label37" Runat="server" CssClass="Input_Label">+</asp:label></td>
									                    <td></td>
									                    <!--Aggio Per Ente Incassato-->
									                    <td><asp:label id="Label31" Runat="server" CssClass="Input_Label">Aggio Per Ente €</asp:label></td>
									                    <td align="right"><asp:label id="LblIncassatoAggio" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label46" Runat="server" CssClass="Input_Label">+</asp:label></td>
								                    </tr>
								                    <tr>
									                    <!--Addizionale Provinciale Per Ente Emesso-->
									                    <td><asp:label id="Label59" Runat="server" CssClass="Input_Label">Addizionale Provinciale Per Ente €</asp:label></td>
									                    <td align="right"><asp:label id="LblEmessoProvEnte" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label61" Runat="server" CssClass="Input_Label">+</asp:label></td>
									                    <td></td>
									                    <!--Addizionale Provinciale Per Ente Incassato-->
									                    <td><asp:label id="Label62" Runat="server" CssClass="Input_Label">Addizionale Provinciale Per Ente €</asp:label></td>
									                    <td align="right"><asp:label id="LblIncassatoProvEnte" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label64" Runat="server" CssClass="Input_Label">+</asp:label></td>
								                    </tr>
								                    <tr>
									                    <!--Addizionale Provinciale Emesso-->
									                    <td><asp:label id="Label34" Runat="server" CssClass="Input_Label">Addizionale Provinciale €</asp:label></td>
									                    <td style="BORDER-BOTTOM: #00008b 1px solid" align="right"><asp:label id="LblEmessoProv" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label40" Runat="server" CssClass="Input_Label">=</asp:label></td>
									                    <td></td>
									                    <!--Addizionale Provinciale Incassato-->
									                    <td><asp:label id="Label36" Runat="server" CssClass="Input_Label">Addizionale Provinciale €</asp:label></td>
									                    <td style="BORDER-BOTTOM: #00008b 1px solid" align="right"><asp:label id="LblIncassatoProv" Runat="server" CssClass="Input_Label"></asp:label></td>
									                    <td><asp:label id="Label47" Runat="server" CssClass="Input_Label">=</asp:label></td>
								                    </tr>
								                    <tr>
									                    <!--Totale Emesso-->
									                    <td><asp:label id="Label26" Runat="server" CssClass="Legend">Importo Emesso €</asp:label></td>
									                    <td align="right"><asp:label id="LblEmessoTot" Runat="server" CssClass="Legend"></asp:label></td>
									                    <td></td>
									                    <td></td>
									                    <!--Totale Incassato-->
									                    <td><asp:label id="Label33" Runat="server" CssClass="Legend">Importo Incassato €</asp:label></td>
									                    <td align="right"><asp:label id="LblIncassatoTot" Runat="server" CssClass="Legend"></asp:label></td>
									                    <td></td>
								                    </tr>
								                    <tr>
									                    <!--Maggiorazione Emesso-->
									                    <td><asp:label ID="Label68" Runat="server" CssClass="Legend">Maggiorazione Emesso €</asp:label></td>
									                    <td align="right"><asp:label id="LblEmessoMagg" Runat="server" CssClass="Legend"></asp:label></td>
									                    <td></td>
									                    <td></td>
									                    <!--Maggiorazione Incassato-->
									                    <td><asp:label ID="Label69" Runat="server" CssClass="Legend">Maggiorazione Incassato €</asp:label></td>
									                    <td align="right"><asp:label id="LblIncassatoMagg" Runat="server" CssClass="Legend"></asp:label></td>
									                    <td></td>
								                    </tr>
							                    </table>
						                    </fieldset>
					                    </td>
				                    </tr>
			                    </table>
		                    </div>
                            <div id="chart_div" class="col-md-12" style="height:350px;"></div>
				        </td>
				    </tr>
			    </table>
		    </div>
			<asp:button id="CmdAnalisi" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="CmdStampa" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="CmdGrafico" style="DISPLAY: none" runat="server"></asp:button>
		</form>
	</body>
</html>
