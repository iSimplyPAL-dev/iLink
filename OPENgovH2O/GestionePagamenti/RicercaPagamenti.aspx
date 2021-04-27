<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaPagamenti.aspx.vb" Inherits="OpenUtenze.RicercaPagamenti"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaPagamenti</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function Search()
			{			
			    if (document.getElementById('txtDataAccreditoDal').value!='' && document.getElementById('txtDataAccreditoAl').value=='')
				{
					GestAlert('a', 'warning', '', '', 'Valorizzare entrambe le date!');
					return false;
				}
			    if (document.getElementById('txtDataAccreditoDal').value == '' && document.getElementById('txtDataAccreditoAl').value != '')
				{
					GestAlert('a', 'warning', '', '', 'Valorizzare entrambe le date!');
					return false;
				}
				
								
				document.getElementById('loadGrid').src='ResultRicercaPagamenti.aspx?Cognome='+document.getElementById('txtCognome').value+'&Nome='+document.getElementById('txtNome').value+'&CodFiscale='+document.getElementById('txtCF').value+'&PIva='+document.getElementById('txtPIVA').value+'&DdlPeriodo='+document.getElementById('DdlPeriodo').value+'&NFattura='+document.getElementById('txtNFattura').value+'&DataFattura='+document.getElementById('txtDataFattura').value+'&Anno='+document.getElementById('txtAnno').value+'&Provenienza='+document.getElementById('txtProvenienza').value+'&DataDal='+document.getElementById('txtDataAccreditoDal').value+'&DataAl='+document.getElementById('txtDataAccreditoAl').value
				return true;
			}
			
			function NewInsert()
			{			
				
				parent.Comandi.location.href='./Inserimento/CInserimentoPagamenti.aspx?Inserimento=Inserimento'
				parent.Visualizza.location.href='./Inserimento/RicercaFatture.aspx?Operazione=Inserimento'
				return true;
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
						<FIELDSET class="FiledSetRicerca" style="WIDTH: 98%; HEIGHT: 79px">
							<LEGEND class="Legend">
								Inserimento filtri di ricerca</LEGEND>
							<table width="100%">
								<tr>
									<td><asp:label id="Label7" runat="server" CssClass="Input_Label">Cognome</asp:label><br>
										<asp:textbox id="txtCognome" runat="server" CssClass="Input_Text" Width="250px"></asp:textbox></td>
									<td class="Input_Label"><asp:label id="Label8" runat="server" CssClass="Input_Label">Nome</asp:label><br>
										<asp:textbox id="txtNome" runat="server" CssClass="Input_Text" Width="184px"></asp:textbox></td>
									<td class="Input_Label"><asp:label id="Label3" runat="server" CssClass="Input_Label">Codice Fiscale</asp:label><br>
										<asp:textbox id="txtCF" runat="server" CssClass="Input_Text" Width="170px"></asp:textbox></td>
									<td class="Input_Label"><asp:label id="Label4" runat="server" CssClass="Input_Label">Partita IVA</asp:label><br>
										<asp:textbox id="txtPIVA" runat="server" CssClass="Input_Text" Width="160px"></asp:textbox></td>
								</tr>
								<tr>
									<td><asp:label id="Label2" Runat="server" CssClass="Input_Label">Periodo Fatturato</asp:label><br>
										<asp:dropdownlist id="DdlPeriodo" Runat="server" CssClass="Input_Text" AutoPostBack="True"></asp:dropdownlist>
									</td>
									<td class="Input_Label"><asp:label id="Label1" runat="server" CssClass="Input_Label">Anno Emissione</asp:label><br>
										<asp:textbox id="txtAnno" runat="server" CssClass="Input_Number_Generali" Width="104px" onchange="ControllaAnno(this)"></asp:textbox></td>
									<td class="Input_Label"><asp:label id="Label5" runat="server" CssClass="Input_Label">Numero Fattura</asp:label><br>
										<asp:textbox id="txtNFattura" runat="server" CssClass="Input_Text" Width="104px"></asp:textbox></td>
									<td class="Input_Label"><asp:label id="Label6" runat="server" CssClass="Input_Label">Data Fattura</asp:label><br>
										<asp:textbox id="txtDataFattura" onblur="txtDateLostfocus(this);VerificaData(this);" runat="server"
											CssClass="Input_Text" Width="104px"></asp:textbox></td>
								</tr>
							</table>
							<asp:textbox id="txtProvenienza" runat="server" CssClass="Input_Number_Generali" style="DISPLAY: none"></asp:textbox>
						</FIELDSET>
					</td>
				</tr>
				<tr>
					<td style="HEIGHT: 70px">
						<FIELDSET class="FiledSetRicerca" style="WIDTH: 98%; HEIGHT: 51px">
							<LEGEND class="Legend">
								Parametri di ricerca avanzata
							</LEGEND>
							<table>
								<tr>
									<td>
										<asp:Label CssClass="Input_Label" Runat="server" id="lblTesto">Data di Riversamento</asp:Label><br>
										<asp:label id="lblDataAccrDal" CssClass="Input_Label" Runat="server">Dal</asp:label>&nbsp;<asp:textbox id="txtDataAccreditoDal" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"
											CssClass="Input_Text" Runat="server" Width="100px"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="lblDataAccreditoAl" CssClass="Input_Label" Runat="server">Al</asp:label>&nbsp;<asp:textbox id="txtDataAccreditoAl" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"
											CssClass="Input_Text" Runat="server" Width="100px"></asp:textbox>
									</td>
								</tr>
							</table>
						</FIELDSET>
					</td>
				</tr>
				<tr>
					<td style="HEIGHT: 250px"><iframe id="loadGrid" style="WIDTH: 100%; HEIGHT: 350px" src="../../aspVuota.aspx" frameBorder="0" width="100%" height="350"></iframe>
					</td>
				</tr>
				<asp:button id="CmdStampaInsoluto" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
				<asp:button id="CmdStampaPagamenti" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
				<asp:button id="CmdStampaRiversamento" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
				<asp:button id="CmdStampaExcel" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
			</table>
		</form>
	</body>
</HTML>
