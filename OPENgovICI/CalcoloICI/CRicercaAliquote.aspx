<%@ Page language="c#" Codebehind="CRicercaAliquote.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.CRicercaAliquote" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CRicercaAliquote</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script>
		function GoToConfigAliquote()
		{
			parent.Visualizza.location.href="ConfigAliquote.aspx";
		}		
		</script>				
	</HEAD>
	<body class="SfondoGenerale" bottomMargin="0" leftMargin="2" topMargin="6" rightMargin="2">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left">
					<span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label></span>
				</td>
				<td align="right" colSpan="2" rowSpan="2">
					<input class="Bottone BottoneRibalta" id="CopyTo" title="Ribalta Aliquote" onclick="parent.Visualizza.CopyTo();" type="button" name="CopyTo">&nbsp; 
					<input class="Bottone BottoneNewInsert" id="New" title="Inserisci una nuova Aliquota." onclick="parent.Visualizza.GoToConfigAliquote();" type="button" name="Insert">&nbsp; 
					<input class="Bottone BottoneRicerca" id="Search" title="Ricerca" onclick="parent.Visualizza.Search()" type="button" name="Search">
				</td>
			</tr>
			<tr>
				<td style="WIDTH: 463px" align="left">
					<span class="NormalBold_title" id="info" style="WIDTH: 400px; HEIGHT: 20px">ICI/IMU - Configurazione Aliquote - Ricerca</span>
				</td>
			</tr>
		</table>
	</body>
</HTML>
