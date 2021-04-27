<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CmdImporatzioneLetture.aspx.vb" Inherits="OpenUtenze.CmdImporatzioneLetture"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CmdImporatzioneLetture</title>
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
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
		<script>
			function ConfermaImportazione(){
			    var periodo = '<%=Session("PERIODO").ToString().Replace("\", "\\") %>';
				if (confirm('Si vuole procedere con l\'acquisizione delle letture per il periodo '+ periodo +'?'))
				{
				    parent.Visualizza.document.getElementById('btnImporta').click();
				}
			}
			
			function ImportCMGC()
			{
				parent.Visualizza.location.href='../../Importazione/ImportCMGC.aspx';
			}
		</script>
	</HEAD>
	<BODY class="SfondoGenerale" bottomMargin="0" leftMargin="2" topMargin="6" rightMargin="2"
		marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td><asp:label id="Comune" runat="server" Width="504px" CssClass="ContentHead_Title"></asp:label></td>
					<td align="right" width="200" colSpan="2" rowSpan="2">
						<input id="ImportaCMGC" style="display:none" name="ImportaCMGC" onclick="ImportCMGC();" type="button" class="Bottone BottoneElaborazione">
						<input class="Bottone BottoneImport" id="Importa" title="Avvia Importazione" onclick="ConfermaImportazione();" type="button" name="Importa">
					</td>
				</tr>
				<tr>
					<td align="left"><asp:label id="info" runat="server" Width="504px" CssClass="NormalBold_title"></asp:label></td>
				</tr>
			</table>
		</form>
	</BODY>
</HTML>
