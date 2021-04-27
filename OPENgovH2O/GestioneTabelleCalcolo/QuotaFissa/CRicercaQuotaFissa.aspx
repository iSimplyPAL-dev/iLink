<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CRicercaQuotaFissa.aspx.vb" Inherits="OpenUtenze.CRicercaQuotaFissa"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>CRicercaQuotaFissa</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script>

		function VisualizzaLabel(){
			parent.Comandi.infoEnteT.innerText="<%=session("DESCRIZIONE_ENTE")%>"
			parent.Comandi.infoT.innerText="Gestione Quota Fissa - Ricerca"
		}
					
		function Cancel()
		{			
			document.getElementById('loadGrid').src="../../../aspVuota.aspx";
			return true;
		}
		</script>
	</HEAD>
	<body class="SfondoGenerale" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="2"
		topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				<tr>
					<td><asp:label id="Comune" CssClass="ContentHead_Title" Width="504px" runat="server"></asp:label></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
					    <input class="Bottone BottoneRibalta" id="CopyTo" title="Ribalta Aliquote" onclick="parent.Visualizza.document.getElementById('btnRibalta').click();" type="button" name="CopyTo">&nbsp;
						<input class="Bottone BottoneNewInsert" id="NewInsert" title="Inserisci" onclick="parent.Visualizza.NewInsert()"
							type="button" name="NewInsert"> <input class="Bottone BottoneRicerca" id="Search" title="Ricerca" onclick="parent.Visualizza.Search()"
							type="button" name="Search">
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
