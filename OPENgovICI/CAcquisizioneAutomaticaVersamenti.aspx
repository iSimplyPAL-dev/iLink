<%@ Page language="c#" Codebehind="CAcquisizioneAutomaticaVersamenti.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CAcqiusizioneAutomaticaVersamenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CAcqiusizioneAutomaticaVersamenti</title>
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
	<body class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66" leftMargin="2" topMargin="6"
		rightMargin="2" MS_POSITIONING="GridLayout" marginwidth="0" marginheight="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td style="WIDTH: 464px; HEIGHT: 20px" align="left"><span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label></span></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">&nbsp; 
						<input class="Bottone BottoneExcel" id="Excel" title="Stampa controllo importazioni in formato Excel" onclick="parent.Visualizza.document.getElementById('btnStampaExcel').click();" type="button" name="Excel" />
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 463px" align="left"><span class="NormalBold_title" id="info" style="WIDTH: 400px; HEIGHT: 20px;">
							ICI/IMU - Versamenti - Acquisizione</span></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
