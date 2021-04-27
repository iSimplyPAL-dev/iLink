<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiGenerale.aspx.vb" Inherits="Provvedimenti.ComandiGenerale"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiGenerale</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		//*** 20120704 - IMU ***
		function PopolaLabel(){
			document.getElementById("infoEnte").innerText="<%=Session("DESCRIZIONE_ENTE")%>"
			document.getElementById("info").innerText="Configurazione - Tabelle - ICI/IMU - <%=Session("DESC_TIPO_PROC_SERV")%>"
        }
		</script>
	</HEAD>
	<body class="SfondoGenerale" onload="PopolaLabel()" bottomMargin="0" leftMargin="2" topMargin="6"
		rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td style="WIDTH: 464px; HEIGHT: 18px" align="left"><span id="infoEnte" class="ContentHead_Title" style="WIDTH:400px"></span></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<INPUT class="Bottone BottoneSalva" id="Salva" title="Salva" onclick="parent.Visualizza.Salva()"
							type="button" name="Inserisci"> <INPUT class="Bottone BottonePulisci hidden hidden" id="Annulla" title="Pulisci i campi" onclick="parent.Visualizza.Clear()"
							type="button" name="Annulla">
					</td>
				</tr>
				<TR>
					<TD style="WIDTH: 463px" align="left">
						<span id="info" class="NormalBold_title" style="WIDTH:524px;HEIGHT:20px">
						</span>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
