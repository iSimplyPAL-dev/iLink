<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Minimi.aspx.vb" Inherits="OpenUtenze.Minimi" %>
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
					var isel = document.getElementById('cboTariffaUtilizzo').selectedIndex;
				//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
				if(document.getElementById('cboTariffaUtilizzo[isel]').value == '-1')
				{
						sMsg = sMsg + "[Tariffa Utilizzo]\n" ; 
				}
					if (IsBlank(document.getElementById('txtDescrizione').value ))
					{
						sMsg = sMsg + "[Descrizione]\n" ; 
					}					
					if (IsBlank(document.getElementById('txtMinimo').value ))
					{
						sMsg = sMsg + "[Minimo]\n" ; 
					}						
	
						if (!IsBlank(sMsg)) 
						{ 
							strMessage ="Attenzione...\n\n I campi elencati sono obbligatori!\n\n" 
							alert(strMessage + sMsg);
							Setfocus(document.getElementById('cboTariffaUtilizzo')) 
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
				
				
	function VerificaNumero(Numero)
		{
			if(!IsBlank(Numero.value ))
			{			
					if(Numero.value < 0)
					{
							alert("Minimo non valido !");
							Setfocus(Numero);
							return false;
					} 
					if(!isNumber(Numero.value,0,0))
					{
							alert("Minimo non valido.Deve essere un numero !");
							Setfocus(Numero);
							return false;
					}			
			}
				return true; 
		
		}	
				function controlla(max,Max,maxlettere) 
				{
					if (max.value.length >maxlettere) 
					max.value = max.value.substring(0,maxlettere);
				}
		</script>
	</HEAD>
	<body class="Sfondo" onload="document.getElementById('cboTariffaUtilizzo').focus();" bottomMargin="0" leftMargin="0" topMargin="0" marginwidth="0" marginheight="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>
						<asp:Label id="lblOperation" runat="server" CssClass="NormalBold"></asp:Label></td>
				</tr>
				<tr>
					<td class="bordoIframe" width="100%">&nbsp;
					</td>
				</tr>
			</table>
			<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td colspan="2">
						<asp:Label id="lblErrorMessage" runat="server" Cssclass="NormalBold"></asp:Label></td>
				</tr>
				<tr>
					<td class="Input_Label_Bold" noWrap width="25%">Tariffa Utilizzo&nbsp;<FONT class="NormalRed">*</FONT></td>
					<td class="Input_Label_Bold" noWrap width="25%">Descrizione&nbsp;<FONT class="NormalRed">*</FONT></td>
				</tr>
				<tr>
					<td><asp:dropdownlist id="cboTariffaUtilizzo" tabIndex="1" runat="server" Width="212px" Cssclass="Input_Text"></asp:dropdownlist></td>
					<td><asp:textbox id="txtDescrizione" tabIndex="2" runat="server" cssclass="Input_Text" maxlength="50" Width="340px"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label_Bold" noWrap width="25%" colspan="2">Minimo(MC) &nbsp;<FONT class="NormalRed">*</FONT></td>
				</tr>
				<tr>
					<td colspan="2"><asp:textbox id="txtMinimo" tabIndex="3" runat="server" cssclass="Input_Number_Generali" maxlength="50" Width="204px" onfocus="txtNumberGotfocus(this);" onblur="txtNumberLostfocus(this);VerificaNumero(this);"></asp:textbox></td>
				</tr>
				<tr>
					<td colspan="2" class="Input_Label_Bold" noWrap width="25%">Note</td>
				</tr>
				<tr>
					<td colspan="2"><asp:textbox id="txtNote" tabIndex="4" runat="server" Cssclass="Input_Text" maxlength="500" Width="650px" Height="80" TextMode="MultiLine" onkeydown="controlla(this,frmTabelle,200);" onkeyup="controlla(this,frmTabelle,200);"></asp:textbox></td>
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
