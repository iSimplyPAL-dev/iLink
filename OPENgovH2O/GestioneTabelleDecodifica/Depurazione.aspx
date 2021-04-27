<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Depurazione.aspx.vb" Inherits="OpenUtenze.Depurazione" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Ricerca Letture</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta content="True" name="vs_showGrid">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/MyUtility.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
		<script>
		function VerificaCampi()
				{	
					sMsg=""
				//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
			
					if (IsBlank(document.getElementById('txtDescrizione').value ))
					{
						sMsg = sMsg + "[Descrizione]\n" ; 
					}						
						if (!IsBlank(sMsg)) 
						{ 
							strMessage ="Attenzione...\n\n I campi elencati sono obbligatori!\n\n" 
							alert(strMessage + sMsg);
							Setfocus(document.getElementById('txtDescrizione')) 
							return false; 
					}		
					return true; 
				}	
	function Aggiorna()
				{
					if (confirm('L\'elemento che si cerca di modificare e\' usato da altre tabelle.Modificare comunque?'))
					{
						document.getElementById('btnForzaModifica').click(); 
					}	
				}
				function controlla(max,Max,maxlettere) 
				{
					if (max.value.length >maxlettere) 
					max.value = max.value.substring(0,maxlettere);
				}
		</script>
	</HEAD>
	<body class="Sfondo" onload="document.getElementById('txtCodiceDepurazione').focus();"
		bottomMargin="0" leftMargin="0" topMargin="0" marginwidth="0" marginheight="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>
						<asp:Label id="lblOperation" runat="server" CssClass="Legend" width="100%"></asp:Label></td>
				</tr>
				<tr>
					<td class="bordoIframe" width="100%">&nbsp;
					</td>
				</tr>
			</table>
			<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td>
						<asp:Label id="lblErrorMessage" runat="server" Cssclass="NormalBold"></asp:Label></td>
				</tr>
				<tr>
					<td class="Input_Label" noWrap width="25%">Codice Depurazione</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtCodiceDepurazione" tabIndex="1" runat="server" cssclass="Input_Text"
							Width="150" MaxLength="10" ReadOnly="True"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label" noWrap width="25%">Descrizione&nbsp;<FONT class="NormalRed">*</FONT></td>
				</tr>
				<tr>
					<td><asp:textbox id="txtDescrizione" tabIndex="2" runat="server" cssclass="Input_Text" Width="476px"
							MaxLength="200" ReadOnly="True"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label" noWrap width="25%">Note</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtNote" tabIndex="3" runat="server" Cssclass="Input_Text" Width="650px"
							Height="80" TextMode="MultiLine" onKeyDown="controlla(this,frmTabelle,300);" onKeyUp="controlla(this,frmTabelle,300);"
							ReadOnly="True"></asp:textbox></td>
				</tr>
				<tr>
				</tr>
			</table>
			<input id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza"> <input id="paginacomandi" type="hidden" name="paginacomandi">
			<asp:Button id="btnSalva" runat="server" CssClass="hidden"></asp:Button>
			<asp:Button id="btnCancella" runat="server" CssClass="hidden"></asp:Button>
			<asp:Button id="btnAnnulla" runat="server" CssClass="hidden"></asp:Button>
			<asp:Button id="btnForzaModifica" runat="server" CssClass="hidden"></asp:Button>
		</form>
	</body>
</HTML>
