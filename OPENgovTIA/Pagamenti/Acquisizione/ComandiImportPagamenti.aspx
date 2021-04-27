<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiImportPagamenti.aspx.vb" Inherits="OPENgovTIA.ComandiImportPagamenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ComandiImportPagamenti</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</head>
	<body class="SfondoGenerale" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				<TR>
					<TD style="WIDTH: 464px; HEIGHT: 20px" align="left">
						<SPAN class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label>
						</SPAN>
					</TD>
					<TD align="right" width="800" colSpan="2" rowSpan="2">
						<input class="Bottone BottoneHistory" id="Precedenti" title="Flussi Precedenti" onclick="parent.Visualizza.document.getElementById('CmdOld').click();" type="button" name="Precedenti">
                        <INPUT class="Bottone BottoneImport" id="Import" title="Acquisizione Pagamenti" onclick="parent.Visualizza.document.getElementById('CmdImporta').click()" type="button" name="Import">
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 463px" align="left">
						<SPAN class="NormalBold_title" id="info" runat="server" style="WIDTH: 400px; HEIGHT: 20px;">Variabile - Pagamenti - Acquisizione</SPAN>
					</TD>
				</TR>
			</table>
			&nbsp;
		</form>
	</body>
</HTML>
