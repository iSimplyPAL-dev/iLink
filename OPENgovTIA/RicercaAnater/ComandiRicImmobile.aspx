<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRicImmobile.aspx.vb" Inherits="OPENgovTIA.ComandiRicImmobile" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ComandiRicImmobile</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function RibaltaUIAnater(){
				parent.opener.document.getElementById('CmdRibaltaUIAnater').click();
				parent.window.close();
			}		
		</script>
	</head>
	<BODY MS_POSITIONING="GridLayout" class="SfondoGenerale" bottomMargin="0" leftMargin="2"
		topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				<TR>
					<TD align="left">
					</TD>
					<TD align="right" colSpan="2" rowSpan="2">
						<input class="Bottone BottoneAssocia" id="Associa" title="Associa" onclick="parent.Visualizza.ControlloAssocia()" type="button" name="Associa"> 
						<input class="Bottone BottonePreview" id="RicVani" title="Ricerca Vani" onclick="parent.Visualizza.SearchVani()" type="button" name="RicVani"> 
						<input class="Bottone BottoneRicerca" id="Ricerca" title="Ricerca" onclick="parent.Visualizza.SearchImmobileAnater(0)" type="button" name="Ricerca"> 
						<input class="Bottone BottoneAnnulla" id="Esci" title="Esci" onclick="parent.window.close()" type="button" name="Esci">&nbsp;
					</TD>
				</TR>
				<TR>
					<TD align="left">
						<SPAN class="NormalBold_title" id="info" runat="server"> Variabile - Dichiarazioni - Ricerca Immobile Territorio</SPAN>
					</TD>
				</TR>
			</table>
			&nbsp;
		</form>
	</BODY>
</HTML>
