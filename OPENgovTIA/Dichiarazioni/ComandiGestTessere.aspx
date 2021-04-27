<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiGestTessere.aspx.vb" Inherits="OPENgovTIA.ComandiGestTessere" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <head>
		<title>ComandiGestTessere</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name=vs_defaultClientScript content="JavaScript">
		<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	</head>
	<BODY MS_POSITIONING="GridLayout" class="SfondoGenerale" bottomMargin="0" leftMargin="2"
		topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				<TR>
					<TD style="WIDTH: 464px; HEIGHT: 20px" align="left">
						<SPAN class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label>
						</SPAN>
					</TD>
					<TD align="right" width="800" colSpan="2" rowSpan="2">
						<INPUT class="Bottone BottoneRibalta" id="Switch" title="Cambia Tessera" onclick="parent.Visualizza.document.getElementById('CmdLegaImmobili').click()" type="button" name="Switch"> 
						<INPUT class="Bottone BottoneApri" id="Modifica" title="Modifica Tessera" onclick="parent.Visualizza.document.getElementById('CmdModTessera').click()" type="button" name="Modifica"> 
						<INPUT class="Bottone BottoneCancella" id="Delete" title="Elimina Tessera" onclick="parent.Visualizza.DeleteTessera()" type="button" name="Delete"> 
						<INPUT class="Bottone BottoneSalva" id="Salva" title="Salva Tessera" onclick="parent.Visualizza.CheckDatiTessera('<%=Request.Item("sProvenienza")%>')" type="button" name="Salva"> 
						<INPUT class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla dichiarazione." onclick="parent.Visualizza.ClearDatiTessera()" type="button" name="Cancel">
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 463px" align="left">
						<SPAN class="NormalBold_title" id="info" runat="server" style="WIDTH: 600px; HEIGHT: 20px;"
							runat="server"> Variabile - Dichiarazioni - Gestione Tessere </SPAN>
					</TD>
				</TR>
			</table>
			&nbsp;
		</form>
  </BODY>
</HTML>
