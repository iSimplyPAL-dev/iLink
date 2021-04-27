<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrameModificaDatoCatasto.aspx.vb" Inherits="OpenUtenze.FrameModificaDatoCatasto" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Modifica dato catastale</title>
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
		<script language='javascript' type='text/javascript'>
		function Aggiorna(){
		//alert("Ok, funzione di aggiornamento");
		if (confirm('Salvare le modifiche apportate?'))
		{
		    document.getElementById('btnEvento').click();
		//parent.opener.location.href
		}
		}
		
		function cancella(){
		if (confirm('Cancellare il record corrente?'))
		{
		    document.getElementById('btnElimina').click();
		}
		}
		</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0"
		marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr class="SfondoGenerale">
					<td align="left">
						<asp:label id="info" CssClass="NormalBold_title" runat="server">
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ACQUEDOTTO - Contratti - Gestione Contratti - Gestione Dati Catastali
			</asp:label>
					</td>
					<td align="right">
						<input class="Bottone BottoneSalva" title="Aggiorna i dati modificati" onclick="Aggiorna();" type="button"><input class="Bottone BottoneCancella" title="Elimina il record corrente" onclick="cancella();"><input type="button" class="Bottone BottoneAnnulla" title="Chiudi la finestra corrente" onclick="window.close();">
					</td>
				</tr>
			</table>
			<br>
			<br>
			<table cellSpacing="0" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td class="Input_Label_Bold">
						Interno
					</td>
					<td class="Input_Label_Bold">
						Piano
					</td>
					<td class="Input_Label_Bold">Sezione</td>
				</tr>
				<tr>
					<td class="Input_Label_Bold" style="HEIGHT: 19px">
						<asp:textbox id="txtInterno" runat="server" Cssclass="Input_Text" TextMode="SingleLine"></asp:textbox>
					</td>
					<td class="Input_Label_Bold" style="HEIGHT: 19px">
						<asp:textbox id="txtPiano" runat="server" Cssclass="Input_Text" TextMode="SingleLine"></asp:textbox>
					</td>
					<td class="Input_Label_Bold">
						<asp:TextBox id="txtSezione" Runat="server" Cssclass="Input_Text" TextMode="SingleLine"
							MaxLength="3"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td class="Input_Label_Bold">
						Foglio
					</td>
					<td class="Input_Label_Bold">
						Numero
					</td>
					<td class="Input_Label_Bold">
						Subalterno
					</td>
				</tr>
				<tr>
					<td class="Input_Label_Bold" style="HEIGHT: 19px">
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
					<td class="Input_Label_Bold">
						Est. Particella
					</td>
					<td class="Input_Label_Bold">
						Tipo Particella
					</td>
				</tr>
				<tr>
					<td class="Input_Label_Bold">
						<asp:TextBox id="txtEstParticella" Runat="server" Cssclass="Input_Text" MaxLength="4"></asp:TextBox>
					</td>
					<td class="Input_Label_Bold">
						<asp:DropDownList id="ddlTipoParticella" Runat="server" Cssclass="Input_Text" Width="116px"></asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td><asp:Button ID="btnEvento" Runat="server" style="DISPLAY: none"></asp:Button></td>
					<td><asp:Button ID="btnElimina" Runat="server" style="DISPLAY: none"></asp:Button></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
