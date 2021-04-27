<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiLetture.aspx.vb" Inherits="OpenUtenze.FrameComandiLetture" %>
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
		function ConfermaOperazione()
		{		
		    console.log("letturafase1");
			parent.Visualizza.Salva();
		}
		function Annulla()
		{
			parent.Visualizza.Annulla();	
		}
		function NuovoModifica(idPeriodo)
		{
			parent.Visualizza.NuovoModifica(idPeriodo);
		}
		function EliminaLettura()
		{
			parent.Visualizza.EliminaLettura();
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
					<%
					dim idPeriodo 
					
					    If Session("PERIODOID") <> "" Then
					        idPeriodo = 1
					    Else
					        idPeriodo = 0
					    End If
					%>
						<input class="Bottone BottoneSalva" id="Conferma" title="Salva i dati" onclick="ConfermaOperazione();" type="button" name="Conferma"> 
						<input class="Bottone BottoneNewInsert" id="Nuovo" title="" onclick="NuovoModifica(<%=idPeriodo%>);" type="button" name="Nuovo"> 
						<input class="Bottone BottoneCancella" id="Cancella" title="" onclick="EliminaLettura();" type="button" name="Cancella"> 
						<input class="Bottone BottoneAnnulla" id="Annulla" title="Chiude la Pagina" onclick="parent.window.close();" type="button" name="Annulla">
					</td>
				</tr>
				<tr>
					<td align="left"><asp:label id="info" runat="server" Width="504px" CssClass="NormalBold_title"></asp:label></td>
				</tr>
			</table>
		</form>
	</BODY>
</HTML>
