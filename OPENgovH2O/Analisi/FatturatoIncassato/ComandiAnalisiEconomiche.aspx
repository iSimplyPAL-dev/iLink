<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiAnalisiEconomiche.aspx.vb" Inherits="OpenUtenze.ComandiAnalisiEconomiche" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiAnalisiEconomiche</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoGenerale" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="2"
		topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				<tr>
					<td><asp:label id="Comune" CssClass="ContentHead_Title" Width="504px" runat="server"></asp:label></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<input class="Bottone BottoneExcel" id="Stampa" title="Stampa" onclick="parent.Visualizza.document.frames.item('LoadResult').LoadStampa()" type="button" name="Stampa"> 
						<input class="Bottone BottoneGrafico" id="Grafico" title="Visualizza grafico" onclick="parent.Visualizza.document.frames.item('LoadResult').LoadGrafico()" type="button" name="Grafico"> 
						<!--*** 20130204 - analisi economiche senza filtro per ente ***-->
						<input class="Bottone BottoneRateizzazioni" id="RicercaNoEnte" title="Ricerca senza filtro per Ente" onclick="parent.Visualizza.LoadAnalisi('NOENTE')" type="button" name="RicercaNoEnte"> 
						<!---*** ***-->
						<input class="Bottone BottoneRicerca" id="Ricerca" title="Ricerca" onclick="parent.Visualizza.LoadAnalisi('ENTE')" type="button" name="Ricerca">
					</td>
				</tr>
				<tr>
					<td align="left">
						<asp:label id="info" ForeColor="White" CssClass="NormalBold_title" Width="504px" runat="server"></asp:label>
					</td>
				</tr>
			</table>
			&nbsp;
		</form>
	</body>
</HTML>
