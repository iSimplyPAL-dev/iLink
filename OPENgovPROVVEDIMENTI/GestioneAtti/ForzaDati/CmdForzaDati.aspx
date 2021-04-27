<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CmdForzaDati.aspx.vb" Inherits="Provvedimenti.CmdForzaDati" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CmdForzaDati</title>
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
	</HEAD>
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
						<INPUT class="Bottone BottoneCancella" id="Delete" title="Elimina Atto" onclick="parent.Visualizza.DeleteDati()" type="button" name="Delete"> 
						<INPUT class="Bottone BottoneSalva" id="Salva" title="Salva Dati Forzati" onclick="parent.Visualizza.SaveDati()" type="button" name="Salva"> 
						<INPUT class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla videata Consultazione Dati Attuali" onclick="parent.Visualizza.location.href='../GestioneAtti.aspx<%=Session("ParamGestioneAtti")%>'" type="button" name="Cancel">
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 463px" align="left">
						<SPAN class="NormalBold_title" id="info" style="WIDTH: 400px; HEIGHT: 20px;">Gestione Atti - Forza Dati</SPAN>
					</TD>
				</TR>
			</table>
			&nbsp;
		</form>
	</BODY>
</HTML>
