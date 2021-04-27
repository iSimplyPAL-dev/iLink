<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiAggiornamentoMassivo.aspx.vb" Inherits="Provvedimenti.ComandiAggiornamentoMassivo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiEnte</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script>
		function VisualizzaLabel(){
						
			//parent.Comandi.info.innerText="<%=Session("DESC_TIPO_PROC_SERV")%>"
			parent.Comandi.infoEnte.innerText="<%=Session("DESCRIZIONE_ENTE")%>"
			parent.Comandi.info.innerText= "<%=Session("DESC_TIPO_PROC_SERV") %>" + '  -  Aggiornamento Massivo Date Atti '
			
		}
		</script>
	</HEAD>
	<body onload="VisualizzaLabel();" class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66"
		leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left"><asp:label id="infoEnte" runat="server" Width="400px" CssClass="ContentHead_Title"></asp:label></td>
				<td align="right" width="800" colSpan="2" rowSpan="2">
					<INPUT class="Bottone BottoneSalva" id="Salva" title="Aggiornamento Massivo Date" onclick="parent.Visualizza.Salva()"
						type="button" name="Search"> <INPUT class="Bottone Bottoneannulla" id="Annulla" title="Torna alla pagina di Ricerca Semplice o Avanzata Atti"
						onclick="parent.Visualizza.Back()" type="button" name="Clear">
					<!--<INPUT class="Bottone Bottoneannulla" id="Annulla" title="Pulisce i filtri utilizzati per la ricerca"
						style="BORDER-RIGHT: 1px outset; BORDER-TOP: 1px outset; BORDER-LEFT: 1px outset; WIDTH: 30px; BORDER-BOTTOM: 1px outset; HEIGHT: 30px"
						onclick="parent.Visualizza.Clear()" type="button" name="Clear">-->
				</td>
			</tr>
			<TR>
				<TD style="WIDTH: 463px" align="left">
					<asp:label id="info" CssClass="NormalBold_title" Width="524px" runat="server" Height="20px"
						></asp:label></TD>
			</TR>
		</table>
	</body>
</HTML>
