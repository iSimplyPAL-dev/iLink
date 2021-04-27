<%@ Page language="c#" Codebehind="CGestioneElaborazione.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.GestioneElaborazione.CGestioneElaborazione" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CGestioneElaborazione</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body onload="VisualizzaLabel();" class="SfondoGenerale" bottomMargin="0" leftMargin="2" topMargin="6" rightMargin="2">
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td align="left" style="WIDTH: 641px"><span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label></span></td>
				</tr>
				<tr>
					<td style="WIDTH: 641px" align="left">
						<!--*** 20120704 - IMU ***-->
						<span class="NormalBold_title" id="info" style="WIDTH: 580px; HEIGHT: 24px">ICI/IMU - Calcolo Massivo - Stato Elaborazione</span>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
