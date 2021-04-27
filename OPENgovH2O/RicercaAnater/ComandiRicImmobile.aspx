<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRicImmobile.aspx.vb" Inherits="OpenUtenze.ComandiRicImmobile"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>ComandiRicImmobile</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name=vs_defaultClientScript content="JavaScript">
		<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function RibaltaUIAnater(){
				parent.opener.document.getElementById('CmdRibaltaUIAnater').click();
				parent.window.close();
			}
		
			function ControlloAssocia(){
				if (parent.Visualizza.loadGrid.ResultRicImmobile==undefined){
					alert("Per Associare un Immobile, effettuare la ricerca,\nselezionare un Immobile e premere il pulsante Associa.")
				}
				else
				{
				    parent.Visualizza.loadGrid.document.getElementById('CmdRibaltaAnater').click()
				}
			}
		</script>
  </HEAD>
	<BODY leftMargin="0" topMargin="6" marginwidth="0" marginheight="0" class="SfondoGenerale">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td><asp:label id="Comune" CssClass="ContentHead_Title" Width="100%" runat="server"></asp:label></td>
				<td align="right" colSpan="2" rowSpan="2" width="40%">
					<input class="Bottone BottoneAssocia" id="Associa" title="Associa" onclick="ControlloAssocia()" type="button" name="Associa"> 
					<input class="Bottone BottoneRicerca" id="Ricerca" title="Ricerca" onclick="parent.Visualizza.SearchImmobileAnater()" type="button" name="Ricerca"> 
					<input class="Bottone BottoneAnnulla" id="Esci" title="Esci" onclick="parent.window.close()" type="button" name="Esci">
					&nbsp;
				</td>
			</tr>
			<tr>
				<td align="left"><asp:label id="info" CssClass="NormalBold_title" Width="100%" runat="server"></asp:label></td>
			</tr>
		</table>
	</BODY>
</HTML>
