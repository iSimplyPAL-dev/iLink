<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRicerca.aspx.vb" Inherits="OpenUtenze.ComandiRicerca" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title></title>
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
			function Ricerca()
			{
				parent.Visualizza.Search();	    
			}
			function Nuovo()
			{
			    parent.Visualizza.Nuovo();
			}
		</script>
	</HEAD>
	<BODY class="SfondoGenerale" bottomMargin="0" leftMargin="0" topMargin="10" marginwidth="0" marginheight="0">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td>
					<asp:label id="Comune" CssClass="ContentHead_Title" Width="504px" runat="server"></asp:label>
				</td>
				<td align="right" colSpan="2" rowSpan="2" width="150">
					<%--<input class="Bottone BottoneElaborazione" runat="server" id="ContrattiFromContatori" style="DISPLAY:none" title="ContrattiFromContatori" onclick="parent.Visualizza.ContrattiFromContatori();" 
					type="button" name="ContrattiFromContatori"/>--%>
					<input class="Bottone BottoneExcel" runat="server" id="Excel" title="Stampa riassuntiva" onclick="parent.Visualizza.Ex();"
					 type="button" name="Excel"/>
				   <input class="Bottone BottoneNewInsert" runat="server" onclick="Nuovo();"
				   type="button" name="MODIFICA" title="Consente di inserire un Nuovo elemento nella Tabella"/>
					<input class="Bottone BottoneRicerca" runat="server" id="Ricerca" title="Avvia la ricerca..." onclick="Ricerca();" 
					type="button" name="Ricerca"/>					
				</td>
			</tr>
			<tr>
				<td align="left"><asp:label id="info" CssClass="NormalBold_title" Width="504px" runat="server"></asp:label></td>
			</tr>
		</table>
	</BODY>
</HTML>
