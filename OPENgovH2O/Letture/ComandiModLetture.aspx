<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiModLetture.aspx.vb" Inherits="OpenUtenze.ComandiModLetture" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Comandi ModLetture</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">
			function AnnullaSitContrib()
			{				
				parent.Visualizza.location.href='<%=Request.Item("OPENgovPATH")%>/SituazioneContribuente/SitContrib.aspx?COD_CONTRIBUENTE=<%=Request.Item("CodContribuente")%>';
			}
			</script>
	</HEAD>
	<BODY class="SfondoGenerale" bottomMargin="0" leftMargin="0" topMargin="10" marginheight="0"
		marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td>
					<asp:label id="Comune" runat="server" Width="100%" CssClass="ContentHead_Title"></asp:label>
				</td>
				<td align="right" rowSpan="2" width="35%">
					<input class="Bottone BottoneAnnulla" id="btnAnnullaSitContrib" runat="server" title="Torna alla Situazione Contribuente"
						onclick="AnnullaSitContrib();" type="button" name="AnnullaSitContrib"> &nbsp;
				</td>
			</tr>
			<tr>
				<td align="left">
					<asp:label id="info" CssClass="NormalBold_title" Width="100%" runat="server"></asp:label>
				</td>
			</tr>
		</table>
	</BODY>
</HTML>
