<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaDocumenti.aspx.vb" Inherits="OpenUtenze.RicercaDocumenti"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaDocumenti</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function Search()
			{
			    DivAttesa.style.display = '';
				document.getElementById('loadGrid').src = 'RisultatiDocumenti.aspx?NominativoDa='+document.getElementById('txtNominativoDa').value +'&NominativoA='+document.getElementById('txtNominativoA').value+'&NumeroFattura='+document.getElementById('txtNumeroFattura').value+'&DataFattura='+document.getElementById('txtDataFattura').value+'&IdRuolo='+document.getElementById('txtIdRuolo').value				
			}
			
			function ElaborazioniDocumenti()
			{
				if (confirm('Si vuole procedere con l\'elaborazione dei documenti?'))
					{
					    DivAttesa.style.display = '';
					    document.getElementById('lblElaborazioniEffettuate').value = 'Elaborazione in corso...Attendere prego!';
					    document.getElementById('CmdElaborazione').click()
					}					
			}
				
			function ApprovaDocumenti()
			{
			if (confirm('Si vuole procedere con l\'approvazione dell\'elaborazione dei documenti?'))
				document.getElementById('CmdApprovaDoc').click()
			}
			
			function EliminaElaborazione()
			{
			if (confirm('Si vuole procedere con l\'eliminazione dei documenti effettivi già elaborati?'))
				document.getElementById('CmdEliminaDoc').click()
			}
			
			function ConfermaUscita()
			{
			document.getElementById('CmdUscita').click()
			}
			
			function ApriVisualizzaDocElaborati()
			{			
				// apro il popup di visualizzazione doc elaborati
				winWidth=980 
				winHeight=500 
				myleft=(screen.availWidth-winWidth)/2 
				mytop=(screen.availheight-winHeight)/2 - 40 
				WinPopDoc=window.open('ViewDocumentiElaborati.aspx','','width='+winWidth+',height='+winHeight+',top='+mytop+',left='+myleft+' status=yes, toolbar=no,scrollbar=no, resizable=no') 
			}
			
			function Fatturazione(){
				parent.Visualizza.location.href='../Fatturazione/Fatturazione.aspx?paginacomandi=./ComandiFatturazione.aspx'
			}
			</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table style="Z-INDEX: 100; LEFT: 0px; POSITION: absolute; TOP: 0px" cellSpacing="0" cellPadding="0" width="98%" border="0">
				<tr>
					<td>					
						<table id="TblDatiContribuente" cellSpacing="0" cellPadding="0" width="100%" class="dati_anagrafe_tarsu_blu" border="1">
							<tr>
								<td>
									<table id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
									    <tr>
										    <td class="Input_Label" colSpan="6" height="20"><strong>DATI RUOLO</strong></td>
									    </tr>
									    <tr>
										    <td class="DettagliContribuente">Ruolo</td>
										    <td>&nbsp;</td>
										    <td class="DettagliContribuente">relativo all'anno</td>
										    <td><asp:label id="lblAnnoRuolo" runat="server" CssClass="DettagliContribuente">Label</asp:label></td>
										    <td class="DettagliContribuente">Elaborato in data</td>
										    <td><asp:label id="lblDataCartellazione" runat="server" CssClass="DettagliContribuente">Label</asp:label></td>
									    </tr>
									    <tr>
										    <td colspan="4">
										        <asp:label id="lblDocElaborati" CssClass="DettagliContribuente" Width="100%" Runat="server">Documenti Effettivi già elaborati:     </asp:label>
										    </td>
										    <td colspan="2">
										        <asp:label id="lblDocDaElaborare" CssClass="DettagliContribuente" Width="100%" Runat="server">Documenti da elaborare:     </asp:label>
										    </td>														
									    </tr>
								    </table>
							    </td>
						    </tr>
						    <tr>
							    <td>
								    <asp:Label id="lblElaborazioniEffettuate" runat="server" Cssclass="NormalRed"></asp:Label>
							    </td>
						    </tr>
					    </table>
                    </td>
                </tr>
                <tr>
                    <td>					
					    <table id="TblParametriRicerca" cellSpacing="1" cellPadding="1" width="100%" border="0">
						    <tr>
							    <td>
								    <fieldset class="FiledSetRicerca"><LEGEND class="Legend">Inserimento filtri di ricerca</LEGEND>
									    <table id="TblParametri" cellSpacing="1" cellPadding="1" width="100%" border="0">
										    <tr>
											    <td>
												    <asp:label id="Label7" CssClass="Input_Label" Runat="server">Nominativo Da</asp:label><BR>
												    <asp:textbox id="txtNominativoDa" runat="server" CssClass="Input_Text" Width="250px" ></asp:textbox></td>
											    <td><asp:label id="Label4" CssClass="Input_Label" Runat="server">Nominativo A</asp:label><BR>
												    <asp:textbox id="txtNominativoA" runat="server" CssClass="Input_Text" Width="250px" ></asp:textbox></td>
											    <td><asp:label id="Label1" CssClass="Input_Label" Runat="server">Numero Fattura</asp:label><BR>
												    <asp:textbox id="txtNumeroFattura" runat="server" CssClass="Input_Text" Width="100px" ></asp:textbox></td>
											    <td><asp:label id="Label2" CssClass="Input_Label" Runat="server">Data Fattura</asp:label><BR>
												    <asp:textbox id="txtDataFattura" runat="server" CssClass="Input_Text" Width="80px" ></asp:textbox></td>
										    </tr>
									    </table>
								    </fieldset>
							    </td>
						    </tr>
					    </table>
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
					<td width="100%">
					    <iframe id="loadGrid" src="../../aspVuota.aspx" frameBorder="0" width="100%" height="450"></iframe>
					</td>
				</tr>
		    </table>
		    <asp:button id="CmdElaborazione" CssClass="hidden" runat="server"></asp:button>
            <asp:button id="CmdExpFat" CssClass="hidden" runat="server"></asp:button>
            <asp:button id="CmdApprovaDoc" CssClass="hidden" runat="server"></asp:button>
            <asp:button id="CmdEliminaDoc" CssClass="hidden" runat="server"></asp:button>
            <asp:button id="CmdUscita" CssClass="hidden" runat="server"></asp:button>
            <asp:textbox id="txtIdRuolo" CssClass="hidden" runat="server" Width="136px" AutoPostBack="True"></asp:textbox>
		</form>
	</body>
</HTML>
