<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaImmobile.aspx.vb" Inherits="OPENgovTIA.RicercaImmobile" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>RicercaImmobile</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function SearchImmobileAnater(NewVani)
			{	var sProv
				sProv='<%=Request.item("Provenienza")%>'
				if (sProv=='N')
				{
					NewVani=1;
				}
				loadGrid.src = 'ResultRicImmobile.aspx?Provenienza=<%=Request.item("Provenienza")%>&TxtCodVia=' + document.getElementById('txtCodVia').value + '&TxtVia=' + document.getElementById('TxtVia').value + '&TxtCivico=' + document.getElementById('TxtCivico').value + '&TxtInterno=' + document.getElementById('TxtInterno').value + '&TxtFoglio=' + document.getElementById('TxtFoglio').value + '&TxtNumero=' + document.getElementById('TxtNumero').value + '&TxtSubalterno=' + document.getElementById('TxtSubalterno').value + '&NewVani=' + NewVani
			}
			function SearchVani()
			{	
				var myIFrame = document.getElementById('loadGrid');
			    var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
				myContent.document.getElementById('CmdSearchVani').click();
			}
			function ControlloAssocia()
			{
				var myIFrame = document.getElementById('loadGrid');
			    var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
				if (myContent.document.getElementById('GrdRicerca') == null)
				{
					alert("Per Associare un Soggetto Anagrafico, effettuare la ricerca,\nselezionare un Soggetto e premere il pulsante Associa.");
				}
				else
				{
					myContent.document.getElementById('CmdRibaltaAnater').click();
				}
			}
			
			function ApriStradario(){
				var CodEnte = '<% = Session("COD_ENTE") %>';
				var TipoStrada = '';
				var Strada = '';
				var CodStrada = document.getElementById('txtCodVia').value;
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
                Parametri += '&FunzioneRitorno=RibaltaStrada'

                window.open('<% = UrlStradario %>?' + Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');
				
				// l'istruzione seguente ritorna false per non fare il postback della pagina.
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
				document.getElementById('txtCodVia').value = objStrada.CodStrada;
				document.getElementById('TxtVia').value=strada;
			}
			
			function ClearDatiVia()
			{
				document.getElementById('TxtVia').value='';
				document.getElementById('txtCodVia').value='';
				return false;
			}
		</script>
	</head>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" border="0" width="100%">
				<tr>
					<td>
						<table id="TblRicerca" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<tr>
								<td>
									<fieldset class="FiledSetRicerca"><LEGEND class="Legend">Inserimento filtri di ricerca</LEGEND>
										<table id="TblParametri" cellSpacing="1" cellPadding="1" width="100%" border="0">
											<tr>
												<td colSpan="3">
													<asp:label id="Label7" Runat="server" CssClass="Input_Label">Via</asp:label>&nbsp;
													<asp:imagebutton id="LnkApriStradario" runat="server" ImageUrl="../../images/Bottoni/Listasel.png"
														ToolTip="Apri Ricerca Stradario" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>&nbsp;
													<asp:imagebutton id="LnkPulisciStrada" runat="server" ToolTip="Pulisci i campi della Via" CausesValidation="False"
														imagealign="Bottom" ImageUrl="../../images/Bottoni/cancel.png"></asp:imagebutton>
													<asp:TextBox ID="txtCodVia" Runat="server" style="DISPLAY:none"></asp:TextBox>
													<BR>
													<asp:textbox id="TxtVia" runat="server" Width="376px" CssClass="Input_Text" ReadOnly="True"></asp:textbox>
												</td>
												<td>
													<asp:label id="Label8" Runat="server" CssClass="Input_Label">Civico</asp:label><BR>
													<asp:textbox id="TxtCivico" runat="server" Width="50px" CssClass="Input_Text"></asp:textbox>
												</td>
												<td>
													<asp:label id="Label11" Runat="server" CssClass="Input_Label">Interno</asp:label><BR>
													<asp:textbox id="TxtInterno" runat="server" Width="50px" CssClass="Input_Text"></asp:textbox>
												</td>
											</tr>
											<tr>
												<td width="150">
													<asp:label id="Label1" Runat="server" CssClass="Input_Label">Foglio</asp:label><BR>
													<asp:textbox id="TxtFoglio" runat="server" Width="120px" CssClass="Input_Text"></asp:textbox>
												</td>
												<td width="150">
													<asp:label id="Label2" Runat="server" CssClass="Input_Label">Numero</asp:label><BR>
													<asp:textbox id="TxtNumero" runat="server" Width="120px" CssClass="Input_Text"></asp:textbox>
												</td>
												<td width="150">
													<asp:label id="Label3" Runat="server" CssClass="Input_Label">Subalterno</asp:label><BR>
													<asp:textbox id="TxtSubalterno" runat="server" Width="120px" CssClass="Input_Text"></asp:textbox>
												</td>
												<td></td>
												<td></td>
											</tr>
										</table>
									</fieldset>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<iframe id="loadGrid" src="../../aspVuota.aspx" frameBorder="0" width="100%" height="600px"></iframe>
		</form>
	</body>
</HTML>
