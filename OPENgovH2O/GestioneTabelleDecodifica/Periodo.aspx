<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Periodo.aspx.vb" Inherits="OpenUtenze.Periodo" %>
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
			
					if (IsBlank(document.getElementById('txtPerido').value))
					{
						sMsg = sMsg + "[Perido]\n" ; 
					}						
						if (!IsBlank(sMsg)) 
						{ 
							strMessage ="Attenzione...\n\n I campi elencati sono obbligatori!\n\n" 
							alert(strMessage + sMsg);
							Setfocus(document.getElementById('txtPerido')) 
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
						function VerificaData(Data)
		{
				if (!IsBlank(Data.value ))
				{		
					if(!isDate(Data.value)) 
					{
					    alert("Inserire la Data  correttamente in formato: gg/mm/aaaa !");
					    Data.value = "";
						Setfocus(Data);
						return false;
					}
				}					
		}
		</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" onload="document.getElementById('txtPerido').focus();"
		marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td><asp:label id="lblOperation" runat="server" CssClass="Legend" width="100%"></asp:label></td>
				</tr>
				<tr>
					<td class="bordoIframe" width="100%">&nbsp;
					</td>
				</tr>
			</table>
			<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td><asp:label id="lblErrorMessage" runat="server" Cssclass="NormalBold"></asp:label></td>
				</tr>
				<tr>
					<td class="Input_Label" noWrap width="25%">Periodo&nbsp;<FONT class="NormalRed">*</FONT>(AAAA/MM)</td>
					<td class="Input_Label" noWrap width="25%">Da</td>
					<td class="Input_Label" noWrap width="25%">A</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtPerido" tabIndex="1" runat="server" MaxLength="7" Width="150" cssclass="Input_Text"></asp:textbox></td>
					<td><asp:textbox id="txtDaData" tabIndex="2" runat="server" MaxLength="10" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox></td>
					<td><asp:textbox id="txtDataA" tabIndex="3" runat="server" MaxLength="10" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label" noWrap width="25%"><br>Periodo Attuale</td>
					<td class="Input_Label" noWrap width="25%">Periodo Storico</td>
				</tr>
				<tr>
					<td><asp:checkbox id="chkAttuale" runat="server" Cssclass="Input_CheckBox_NoBorder" Width="20px" Text=" "></asp:checkbox></td>
					<td><asp:checkbox id="chkStorico" runat="server" Cssclass="Input_c" Width="20px" Text=" "></asp:checkbox></td>
				</tr>
				<tr>
					<td colspan="3">
						<br>
						<fieldset  class="FiledSetRicerca"><legend class="Legend">Tipo Arrotondamento su Consumo</legend>
							<table>
								<tr>
									<td>
										<asp:RadioButton Runat="server" ID="OptNoArrotond" GroupName="OptTipoArrotondamento" Text="Nessun Arrotondamento" CssClass="Input_Label"></asp:RadioButton>
									</td>
									<td>
										<asp:RadioButton Runat="server" ID="OptArrotondMatematico" GroupName="OptTipoArrotondamento" Text="Arrotondamento Matematico" CssClass="Input_Label" Checked="True"></asp:RadioButton>
									</td>
									<td>
										<asp:RadioButton Runat="server" ID="OptArrotondEccesso" GroupName="OptTipoArrotondamento" Text="Arrotondamento per Eccesso" CssClass="Input_Label"></asp:RadioButton>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td colSpan="3" class="Input_Label" noWrap width="25%">Note</td>
				</tr>
				<tr>
					<td colSpan="3">
						<asp:textbox id="txtNote" tabIndex="3" runat="server" Cssclass="Input_Text" Width="650px" Height="55" TextMode="MultiLine" onKeyDown="controlla(this,frmTabelle,200);" onKeyUp="controlla(this,frmTabelle,200);"></asp:textbox>
					</td>
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
