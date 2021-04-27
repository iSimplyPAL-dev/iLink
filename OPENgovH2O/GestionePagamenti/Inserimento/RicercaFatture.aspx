<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaFatture.aspx.vb" Inherits="OpenUtenze.RicercaFatture"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaFatture</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">						
			function Search()
			{			
			    document.getElementById('loadGrid').src = 'ResultRicercaFatture.aspx?Cognome=' + document.getElementById('txtCognome').value + '&Nome=' + document.getElementById('txtNome').value + '&Importo=' + document.getElementById('txtImporto').value + '&NFattura=' + document.getElementById('txtNFattura').value + '&DataFattura=' + document.getElementById('txtDataFattura').value + '&Anno=' + document.getElementById('txtAnno').value + '&Operazione=' + document.getElementById('txtOperazione').value;
				return true;
			}
			function SearchModifica()
			{			
			    document.getElementById('loadGrid').src='ResultRicercaFatture.aspx?Cognome='+document.getElementById('txtCognome').value+'&Nome='+document.getElementById('txtNome').value+'&Importo='+document.getElementById('txtImporto').value+'&NFattura='+document.getElementById('txtNFattura').value+'&DataFattura='+document.getElementById('txtDataFattura').value+'&Anno='+document.getElementById('txtAnno').value+'&Operazione=modifica'
				return true;
			}	
			function NewInsert()
			{					
				parent.Comandi.location.href='./Inserimento/CInserimentoPagamenti.aspx?Inserimento=Inserimento'
				loadInsert.src="./Inserimento/InserimentoPagamenti.aspx";
				return true;
			}			
			function ConfermaUscita()
			{
				if (confirm('Si vuole uscire dalla videata di Inserimento?'))
				{
					parent.parent.Comandi.location.href="./../CRicercaPagamenti.aspx";
					parent.parent.Visualizza.location.href='./../RicercaPagamenti.aspx?EffettuaRicerca=si'
				}
			}
			function ControllaAnno(oggetto){
				if (!IsBlank(oggetto.value)){
					if (!isNumber(oggetto.value, 4, 0, 1950, 2090)){
						alert ("Inserire un anno di quattro cifre\ncompreso fra 1950 e 2090")
						oggetto.value=""
						oggetto.focus()
						return false
					}
				}
			}				
		</script>
	</HEAD>
	<body class="Sfondo" leftMargin="3" rightMargin="3">
		<form id="Form1" runat="server" method="post">
			<table id="tabEsterna" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<tr>
					<td>
						<FIELDSET class="FiledSetRicerca" style="WIDTH: 98%;"><LEGEND class="Legend">Inserimento filtri di ricerca</LEGEND>
							<table width="100%">
								<tr>
									<td class="Input_Label" style="WIDTH: 6.17%"><asp:label id="Label1" runat="server" CssClass="Input_Label">Anno Emissione</asp:label><br>
										<asp:textbox id="txtAnno" runat="server" CssClass="Input_Number_Generali" Width="104px" onchange="ControllaAnno(this)"></asp:textbox></td>
								<tr>
								<tr>
									<td class="Input_Label" style="WIDTH: 6.17%"><asp:label id="Label7" runat="server" CssClass="Input_Label">Cognome Utente</asp:label><br>
										<asp:textbox id="txtCognome" runat="server" CssClass="Input_Text" Width="184px"></asp:textbox></td>
									<td class="Input_Label" style="WIDTH: 20%" colspan="2"><asp:label id="Label8" runat="server" CssClass="Input_Label">Nome Utente</asp:label><br>
										<asp:textbox id="txtNome" runat="server" CssClass="Input_Text" Width="184px"></asp:textbox></td>
								</tr>
								<tr>
									<td class="Input_Label" style="WIDTH: 6.17%"><asp:label id="Label5" runat="server" CssClass="Input_Label">Numero Fattura</asp:label><br>
										<asp:textbox id="txtNFattura" runat="server" CssClass="Input_Text" Width="104px"></asp:textbox></td>
									<td class="Input_Label" style="WIDTH: 6.57%"><asp:label id="Label6" runat="server" CssClass="Input_Label">Data Fattura</asp:label><br>
										<asp:textbox id="txtDataFattura" onblur="txtDateLostfocus(this);VerificaData(this);" runat="server"
											CssClass="Input_Text" Width="104px"></asp:textbox></td>
									<td class="Input_Label" style="WIDTH: 10%"><asp:label id="Label2" runat="server" CssClass="Input_Label">Importo Fattura</asp:label><br>
										<asp:textbox id="txtImporto" runat="server" CssClass="Input_Number_Generali" Width="104px"></asp:textbox>
										<asp:textbox id="txtOperazione" runat="server" CssClass="Input_Number_Generali" style="DISPLAY: none"></asp:textbox>
									</td>
								</tr>
							</table>
						</FIELDSET>
					</td>
				</tr>
				<tr>
					<td>
                        <iframe id="loadGrid" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="300px"></iframe>
					</td>
				</tr>
				<tr>
					<td>
                        <iframe id="loadInsert" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="250px"> </iframe>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
