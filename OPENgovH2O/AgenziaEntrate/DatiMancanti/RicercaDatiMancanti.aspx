<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaDatiMancanti.aspx.vb" Inherits="OpenUtenze.RicercaDatiMancanti"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaDatiMancanti</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function keyPress()
			{
				if (window.event.keyCode==13)
				{
					Search();
				}
			}	

			function Search()
			{
				<%Session.Remove("ResultRicercaDatiMancanti")%>
			    if (document.getElementById('DdlAnno').value == '')
				{
				    GestAlert('a', 'warning', '', '', 'Selezionare un\'anno di riferimento!');
				}
				else
				{
					if (document.getElementById('OptEntrambi').checked==true)
					{
						nTipoCheck=0;
					}
					if (document.getElementById('OptDatiAnagrafici').checked==true)
					{
						nTipoCheck=1;
					}
					if (document.getElementById('OptDatiImmobile').checked==true)
					{
						nTipoCheck=2;
					}
					Parametri="DdlAnno="+document.getElementById('DdlAnno').value
					Parametri += "&TxtCognome="+document.getElementById('TxtCognome').value
					Parametri += "&TxtNome="+document.getElementById('TxtNome').value
					Parametri += "&TipoRicerca="+nTipoCheck
					document.getElementById('LoadGrid').src='ResultDatiMancanti.aspx?'+Parametri
				}
					
				return true;
			}
			
			function LoadDettaglioArticolo(nIdArticolo)
			{
				sParametri ="sProvenienza=AE&IDCONTATORE="+nIdArticolo
				parent.Visualizza.location.href='../../DataEntryContatori/DatiGenerali.aspx?'+sParametri
			}
			
			function PrintXLSDatiMancanti()
			{
				if (confirm('Si vuole produrre il file di excel?'))
				{
				    document.getElementById('CmdStampa').click();
				}
			}
		</script>
	</HEAD>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<table id="tblRicerca" cellSpacing="0" cellPadding="0" width="776" border="0">
				<tr>
					<td>
						<fieldset><legend class="Legend">Inserimento Parametri di Ricerca</legend>
							<table id="TblParametri" width="100%" runat="server">
								<tr>
									<td>
										<asp:label id="Label1" runat="server" CssClass="Input_Label">Anno Ruolo</asp:label>
										<br/>
										<asp:dropdownlist id="DdlAnno" onkeydown="keyPress();" tabIndex="1" runat="server" CssClass="Input_Text"></asp:dropdownlist>
									</td>
									<td colSpan="2">
										<asp:label id="Label2" runat="server" CssClass="Input_Label">Cognome</asp:label>
										<br/>
										<asp:textbox id="TxtCognome" onkeydown="keyPress();" tabIndex="2" runat="server" CssClass="Input_Text"
											Width="300px"></asp:textbox>
									</td>
									<td>
										<asp:label id="Label3" runat="server" CssClass="Input_Label">Nome</asp:label>
										<br/>
										<asp:textbox id="TxtNome" onkeydown="keyPress();" tabIndex="3" runat="server" CssClass="Input_Text"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td colspan="5">
										<asp:label id="Label4" runat="server" CssClass="Legend">Tipo Dati Mancanti</asp:label>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:RadioButton id="OptDatiAnagrafici" GroupName="OptTipoRicerca" CssClass="Input_Label" Text="Dati Anagrafici"
											Runat="server"></asp:RadioButton>
									</td>
									<td colspan="2">
										<asp:RadioButton id="OptDatiImmobile" GroupName="OptTipoRicerca" CssClass="Input_Label" Text="Dati Immobile"
											Runat="server"></asp:RadioButton>
									</td>
									<td>
										<asp:RadioButton id="OptEntrambi" GroupName="OptTipoRicerca" CssClass="Input_Label" Text="Entrambi"
											Runat="server" Checked="True"></asp:RadioButton>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:Label Runat="server" CssClass="Input_Label" id="Label5">Controllo su Cod.Fiscale/P.Iva e Indirizzo Residenza</asp:Label>
									</td>
									<td colspan="2">
										<asp:Label Runat="server" CssClass="Input_Label" id="Label6">Controllo su Anno, Data Inizio, Tipo e Natura Occupazione, Destinazione d'Uso</asp:Label>
									</td>
								</tr>
							</table>
						</fieldset>
					<td></td>
				<tr>
					<td>
						<asp:label id="Label9" runat="server" CssClass="Legend">Risultato Ricerca</asp:label>
						<iframe id="LoadGrid" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="300"></iframe>
					</td>
				</tr>
			</table>
			<asp:button id="CmdStampa" style="DISPLAY: none" Runat="server"></asp:button>
			<input id="paginacomandi" type="hidden" name="paginacomandi"/>
		</form>
	</body>
</HTML>
