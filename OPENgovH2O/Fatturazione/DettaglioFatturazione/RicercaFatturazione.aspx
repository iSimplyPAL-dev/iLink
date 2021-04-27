<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaFatturazione.aspx.vb" Inherits="OpenUtenze.RicercaFatturazione"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaFatturazione</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">
			function Search()
			{
			    
				var Parametri = '';
	        
				Parametri += 'Provenienza='+document.getElementById('TxtProvenienza').value;
				Parametri += '&DdlPeriodo='+document.getElementById('DdlPeriodo').value;
				Parametri += '&DdlTipoDoc='+document.getElementById('DdlTipoDoc').value;
				Parametri += '&TxtCognome='+document.getElementById('TxtCognome').value;
				Parametri += '&TxtNome='+document.getElementById('TxtNome').value;
				Parametri += '&TxtCfPIva='+document.getElementById('TxtCfPIva').value;
				Parametri += '&TxtDataDoc='+document.getElementById('TxtDataDoc').value;
				Parametri += '&TxtNDoc='+document.getElementById('TxtNDoc').value;
				Parametri += '&TxtMatricola=' + document.getElementById('TxtMatricola').value;
				console.log('Prima href search()');			    

				LoadResult.src = 'ResultRicFatturazione.aspx?' + Parametri;
				console.log('Dopo href search()');
				return true;
			}

			function ClearDatiRicFatturazione()
			{ 
			    document.getElementById('CmdClearDati').click()
			}

			function keyPress()
			{
				if (window.event.keyCode==13)
				{
					Search();
				}
			}	
		</script>
	</HEAD>
	<body class="Sfondo" topmargin="0" bottomMargin="0" leftMargin="0" rightMargin="0">
		<form id="Form1" runat="server" method="post">
			<table id="tabEsterna" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px" cellSpacing="1"
				cellPadding="1" width="99%" border="0">
				<tr>
					<td>
						<FIELDSET class="FiledSetRicerca" style="WIDTH: 100%"><LEGEND class="Legend">Inserimento 
								filtri di ricerca</LEGEND>
							<table width="100%">
								<tr width="100%">
									<td><asp:label id="Label1" Runat="server" CssClass="Input_Label">Periodo</asp:label><br>
										<asp:dropdownlist id="DdlPeriodo" Runat="server" CssClass="Input_Text" AutoPostBack="True"></asp:dropdownlist></td>
									<td><asp:label id="Label2" Runat="server" CssClass="Input_Label">Tipo Documento</asp:label><br>
										<asp:dropdownlist id="DdlTipoDoc" Runat="server" CssClass="Input_Text" AutoPostBack="True"></asp:dropdownlist></td>
									<td><br>
									</td>
								</tr>
								<tr width="100%">
									<td><asp:label id="Label4" Runat="server" CssClass="Input_Label">Cognome</asp:label><br>
										<asp:textbox id="TxtCognome" Runat="server" CssClass="Input_Text" Width="265px"></asp:textbox></td>
									<td><asp:label id="Label5" Runat="server" CssClass="Input_Label">Nome</asp:label><br>
										<asp:textbox id="TxtNome" Runat="server" CssClass="Input_Text" Width="180px"></asp:textbox></td>
									<td><asp:label id="Label6" Runat="server" CssClass="Input_Label">Cod.Fiscale/P.IVA</asp:label><br>
										<asp:textbox id="TxtCfPIva" Runat="server" CssClass="Input_Text" Width="150px"></asp:textbox></td>
								</tr>
								<tr>
									<td><asp:label id="Label3" Runat="server" CssClass="Input_Label">Data Documento</asp:label><br>
										<asp:textbox id="TxtDataDoc" Runat="server" CssClass="Input_Text" Width="150px" MaxLength="10"
											onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox></td>
									<td><asp:label id="Label7" Runat="server" CssClass="Input_Label">N.Documento</asp:label><br>
										<asp:textbox id="TxtNDoc" Runat="server" CssClass="Input_Text" Width="180px"></asp:textbox></td>
									<td><asp:label id="Label8" Runat="server" CssClass="Input_Label">Matricola</asp:label><br>
										<asp:textbox id="TxtMatricola" Runat="server" CssClass="Input_Text" Width="150px"></asp:textbox></td>
								</tr>
							</table>
						</FIELDSET>
					</td>
				</tr>
				<TR width="100%">
					<td width="100%" height="350px">
						<IFRAME id="LoadResult" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="100%"></IFRAME>
					</td>
				</tr>
			</table>
			<input id="paginacomandi" type="hidden" name="paginacomandi">
			<asp:TextBox ID="TxtProvenienza" Runat="server" style="DISPLAY:none"></asp:TextBox>
			<asp:Button id="CmdClearDati" Runat="server" style="DISPLAY:none"></asp:Button>
		</form>
	</body>
</HTML>
