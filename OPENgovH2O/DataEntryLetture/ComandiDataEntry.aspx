<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiDataEntry.aspx.vb" Inherits="OpenUtenze.ComandiDataEntry" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Comandi Ricerca Contatori</title>
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
			function Conferma()
			{				
					parent.Visualizza.SalvaDatiContatore();				
			}
			function Annulla()
			{				
					parent.Visualizza.Annulla();				
			}

			function NuovoModifica(idPeriodo)
			{						
				parent.Visualizza.NuovoModifica(idPeriodo);				
			}
		</script>
	</HEAD>
	<BODY leftMargin="0" topMargin="6" marginwidth="0" marginheight="0" class="SfondoGenerale">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td><asp:label id="Comune" CssClass="ContentHead_Title" Width="504px" runat="server"></asp:label></td>
				<td align="right" colSpan="2" rowSpan="2" width="150">
					<%
					dim idPeriodo 
					
					    If Session("PERIODOID") <> "" Then
					        idPeriodo = 1
					    Else
					        idPeriodo = 0
					    End If
					
					%>
					<input class="Bottone BottoneSalva" title="Conferma i Dati" onclick="Conferma();" type="button" name="Salva" style="display:none">
				<input class="Bottone BottoneNewInsert" title="Nuova Lettura" onclick="NuovoModifica(<%=idPeriodo%>);" type="button" name="Conferma">
					<input class="Bottone BottoneAnnulla" title="Torna alla Pagina  di ricerca" onclick="Annulla();" type="button" name="Annulla"></td>
				</td>
			</tr>
			<tr>
				<td align="left"><asp:label id="info" CssClass="NormalBold_title" Width="504px" runat="server"></asp:label></td>
			</tr>
		</table>
	</BODY>
</HTML>
