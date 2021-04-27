<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrameInserisciDatoCatasto.aspx.vb" Inherits="OpenUtenze.FrameInserisciDatoCatasto" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Inserisci dato catastale</title>
		<meta content="False" name="vs_showGrid">
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
		<script type="text/javascript" src="../../_js/NumbersOnly.js?newversion"></script>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript">
			function Aggiorna()
			{
				//alert(parent.opener.loadGrid.document.getElementById('nullo'));
			    document.getElementById('btnEvento').click();
				//parent.opener.loadGrid.document.getElementById('nullo').click();
				//alert(parent.opener.loadGrid);
				//parent.opener.loadGrid.reload();
				//window.close();
			}
			
			function visualizza()
			{
				//parent.opener.loadGrid.document.getElementById('nullo').click();
				//parent.opener.loadGrid.reload();
				window.close();
			}
		</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0"
		marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr class="SfondoGenerale">
					<td align="left">
						<asp:label id="info" CssClass="NormalBold_title" runat="server"></asp:label>
					</td>
					<td align="right">
						<input class="Bottone BottoneSalva" title="Aggiorna i dati modificati" onclick="Aggiorna();" type="button">
						<input type="button" class="Bottone BottoneAnnulla" title="Chiudi la finestra corrente" onclick="visualizza();">
					</td>
				</tr>
			</table>
			<br>
			<table cellSpacing="0" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td class="Input_Label_Bold">Interno</td>
					<td class="Input_Label_Bold">Piano</td>
					<td class="Input_Label_Bold">Sezione</td>
				</tr>
				<tr>
					<td class="Input_Label_Bold">
						<asp:textbox id="txtInterno" runat="server" Cssclass="Input_Text" TextMode="SingleLine"></asp:textbox>
					</td>
					<td>
						<asp:textbox id="txtPiano" runat="server" Cssclass="Input_Text" TextMode="SingleLine"></asp:textbox>
					</td>
					<td class="Input_Label_Bold">
						<asp:textbox id="txtSezione" runat="server" Cssclass="Input_Text" TextMode="SingleLine"
							MaxLength="3"></asp:textbox>
					</td>
				<tr>
					<td class="Input_Label_Bold">Foglio</td>
					<td class="Input_Label_Bold">Numero</td>
					<td class="Input_Label_Bold">Subalterno</td>
				</tr>
				<tr>
					<td class="Input_Label_Bold">
						<asp:textbox id="txtFoglio" runat="server" Cssclass="Input_Text" TextMode="SingleLine"></asp:textbox>
					</td>
					<td class="Input_Label_Bold">
						<asp:textbox id="txtNumero" runat="server" Cssclass="Input_Text" TextMode="SingleLine"></asp:textbox>
					</td>
					<td class="Input_Label_Bold">
						<asp:textbox id="txtSubalterno" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td class="Input_Label_Bold">Est. Particella</td>
					<td class="Input_Label_Bold">Tipo particella</td>
				</tr>
				<tr>
					<td>
						<asp:TextBox id="txtEstParticella" Runat="server" Cssclass="Input_Text" MaxLength="4">0</asp:TextBox>
					</td>
					<td>
						<asp:DropDownList id="ddlTipoParticella" runat="server" Cssclass="Input_Text" Width="113px"></asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Button ID="btnEvento" Runat="server" style="DISPLAY: none"></asp:Button>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
