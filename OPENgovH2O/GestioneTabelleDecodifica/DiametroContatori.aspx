<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DiametroContatori.aspx.vb" Inherits="OpenUtenze.DiametroContatori" %>
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
			
				
					if(!IsBlank(document.getElementById('txtDiametroContatore').value ))
					{			
							if(document.getElementById('txtDiametroContatore').value < 0)
							{
								alert("Diametro Contatore non valido inserire un valore maggiore di zero!");
								Setfocus(document.getElementById('txtDiametroContatore'));
								return false;
							} 
							if(!isNumber(document.getElementById('txtDiametroContatore').value,5,1))
							{
								alert("Diametro Contatore non valido inserire una sola cifra decimale!");
								Setfocus(document.getElementById('txtDiametroContatore'));
								return false;
							}
						}			
						else
						{
							alert("Diametro Contatore obbligatorio!");
							Setfocus(document.getElementById('txtDiametroContatore'));
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
					max.value 
					}
            </script>
	</HEAD>
	<body class="Sfondo" onload="document.getElementById('txtDiametroContatore').focus();" bottomMargin="0" leftMargin="0" topMargin="0" marginwidth="0" marginheight="0">
		<form id="Form1" runat="server" method="post">
			<br>
			<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td colspan="2">
						<asp:Label id="lblOperation" runat="server" CssClass="Legend" width="100%"></asp:Label></td>
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
					<td class="Input_Label" noWrap width="25%">Diametro Contatore&nbsp;<FONT class="NormalRed">*</FONT></td>
					<td class="Input_Label" noWrap width="25%"><!--Diametro prevalente dei contatori 
						installati-->&nbsp;</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtDiametroContatore" tabIndex="2" runat="server" cssclass="Input_Number_Generali" MaxLength="5" onfocus="txtNumberGotfocus(this);" onblur="txtNumberLostfocus(this);"></asp:textbox></td>
					<td>&nbsp;<!--<asp:checkbox id="chkPrevalente" runat="server" Cssclass="Input_Text" Width="20px" Text=" "></asp:checkbox>--></td>
					<!--<td><asp:dropdownlist id="cboTariffeContatori" tabIndex="1" runat="server" Width="150" Cssclass="Input_Text"></asp:dropdownlist></td>-->
				</tr>
				<tr>
					<td colspan="2" class="Input_Label" noWrap width="25%">Descrizione</td>
				</tr>
				<tr>
					<td colspan="2"><asp:textbox id="txtDescrizione" tabIndex="3" runat="server" cssclass="Input_Text" Width="476px" MaxLength="200"></asp:textbox></td>
				</tr>
				<tr>
					<td colspan="2" class="Input_Label" noWrap width="25%">Note</td>
				</tr>
				<tr>
					<td colspan="2"><asp:textbox id="txtNote" tabIndex="4" runat="server" Cssclass="Input_Text" Width="764px" Height="80" TextMode="MultiLine" onKeyDown="controlla(this,frmTabelle,300);" onKeyUp="controlla(this,frmTabelle,300);"></asp:textbox></td>
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
