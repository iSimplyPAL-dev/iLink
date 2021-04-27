<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Contratti.aspx.vb" Inherits="OpenUtenze.Contratti"%>
<HTML>
	<HEAD>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script type="text/javascript" src="../../_js/MyUtility.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript">		
		function VerificaData(Data)
		{
				if (!IsBlank(Data.value ))
				{		
					if(!isDate(Data.value)) 
					{
					    alert("Inserire la Data di Nascita correttamente in formato: gg/mm/aaaa !");
					    Data.value = "";
						Setfocus(Data);
						return false;
					}
				}					
		}	
		//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
		function VerificaCampi()
		{	
			sMsg=""
			
			
				//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
			if (IsBlank(document.getElementById('txtCodiceContratto').value)) 
			{ 
				sMsg = sMsg + "[Codice Contratto]\n" ; 
			}
			if (IsBlank(document.getElementById('txtDataSottoscrizione').value ))
			{
				sMsg = sMsg + "[Data Sottoscrizione]\n" ; 
			}						
		
			
			
			if (!IsBlank(sMsg)) 
			{ 
					strMessage ="Attenzione...\n\n I campi elencati sono obbligatori!\n\n" 
					alert(strMessage + sMsg);
					Setfocus(document.getElementById('txtCodiceContratto')) 
					return false; 
			}		
			return true; 
		}	
				//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

		function Salva()
		{
			if(VerificaCampi())
			{
			    document.getElementById('btnEvento').click();
			}	
		}
		</script>
	</HEAD>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="1" width="95%" align="center" border="0">
				<tr>
					<td colSpan="5">&nbsp;</td>
				</tr>
				<tr>
					<td class="Input_Label_Bold" width="25%">Codice Contratto <FONT class="NormalRed">*</FONT></td>
					<td class="Input_Label_Bold" width="25%" colspan="2">
						Data Sottoscrizione<FONT class="NormalRed">*</FONT></td>
				<tr>
					<td><asp:textbox id="txtCodiceContratto" runat="server" Cssclass="Input_Text" Enabled="false"></asp:textbox></td>
					<td><asp:textbox id="txtDataSottoscrizione" enabled="False" runat="server" Cssclass="Input_Text" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox></td>
				</tr>
				<tr>
					<td colspan="2">
						<asp:Label id="lblError" runat="server" Cssclass="NormalBold"></asp:Label></td>
				</tr>
			</table>
			<input type="hidden" id="hdVirtualIDContratto" name="hdVirtualIDContratto">
			<asp:Button id="btnEvento" style="DISPLAY: none" runat="server" Text="Button"></asp:Button>
		</form>
	</body>
</HTML>
