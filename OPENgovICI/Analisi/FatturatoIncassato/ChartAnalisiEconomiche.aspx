<%@ Page language="c#" Codebehind="ChartAnalisiEconomiche.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.Analisi.FatturatoIncassato.ChartAnalisiEconomiche" %>
<%@ Register tagPrefix="Web" Namespace="WebChart" Assembly="WebChart" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ChartAnalisiEconomiche</title>
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
	<body class="Sfondo" MS_POSITIONING="GridLayout" leftmargin="0">
		<Web:ChartControl Width="900px" Height="450px" id="MyChartAnalisi" runat="Server">
			<Background Color="232,248,255" EndPoint="900,900" />
			<XAxisFont StringFormat="Center, Center, Character, NoFontFallback" />
		</Web:ChartControl>
	</body>
</HTML>
