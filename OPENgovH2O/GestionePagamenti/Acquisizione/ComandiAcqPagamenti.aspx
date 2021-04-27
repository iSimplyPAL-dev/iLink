<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiAcqPagamenti.aspx.vb" Inherits="OpenUtenze.ComandiAcqPagamenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiAcqPagamenti</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoGenerale" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="2"
		topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				<TR>
					<td><asp:label id="Comune" CssClass="ContentHead_Title" Width="504px" runat="server"></asp:label></td>
					<TD align="right" width="800" colSpan="2" rowSpan="2">
						<INPUT class="BottoneImport" id="Import" title="Acquisizione Pagamenti" onclick="parent.frames.item('visualizza').FrmAcqPagamenti.CmdImporta.click()"
							type="button" name="Import">
					</TD>
				</TR>
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
