<%@ Page language="c#" Codebehind="CVersamenti.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CVersamenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CVersamenti</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%> 
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%> 
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66"
		leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td style="WIDTH: 464px; HEIGHT: 20px" align="left"><span id="infoEnte" class="ContentHead_Title" style="WIDTH:400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label></span></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<input class="Bottone Bottonebonifica hidden" id="Bonifica" title="Bonifica la dichiarazione." style="display:none;" onclick="parent.Visualizza.document.getElementById('btnBonifica').click()" type="button" name="Bonifica">&nbsp;
						<input class="Bottone BottoneApri" id="Unlock" onclick="parent.Visualizza.document.getElementById('btnAbilita').click()" type="button" name="Unlock" title="Abilita i controlli per scrivere.">
						<input class="Bottone BottoneSalva" id="Insert" onclick="parent.Visualizza.ControllaCampi();" type="button" name="Insert" title="Salva versamento."> 
						<input class="Bottone Bottonecancella" id="Cancel" onclick="parent.Visualizza.document.getElementById('btnElimina').click()" type="button" name="Insert" title="Elimina versamento.">&nbsp;
						<input class="Bottone Bottoneannulla" id="Delete" title="Torna alla pagina di gestione." onclick="parent.Visualizza.document.getElementById('btnIndietro').click()" type="button" name="Delete">
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 463px" align="left"><span id="info" class="NormalBold_title" style="WIDTH:400px;HEIGHT:20px;">
							Versamenti - Inserimento Versamenti</span></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
