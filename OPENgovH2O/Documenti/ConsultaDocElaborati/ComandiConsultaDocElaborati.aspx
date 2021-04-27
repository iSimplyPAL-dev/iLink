<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiConsultaDocElaborati.aspx.vb" Inherits="OpenUtenze.ComandiConsultaDocElaborati"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiConsultaDocElaborati</title>
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
	<body class="SfondoGenerale" bottomMargin="0" leftMargin="0" topMargin="6" rightMargin="0"
		MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td><asp:label id="Comune" CssClass="ContentHead_Title" Width="100%" runat="server"></asp:label></td>
					<td align="right" colSpan="2" rowSpan="2" width="40%">
						<input class="Bottone Bottonericerca" id="Search" title="Ricerca" onclick="parent.Visualizza.Search()"
							type="button" name="Search">&nbsp;
					</td>
				</tr>
				<tr>
					<td align="left"><asp:label id="info" CssClass="NormalBold_title" Width="100%" runat="server"></asp:label></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
