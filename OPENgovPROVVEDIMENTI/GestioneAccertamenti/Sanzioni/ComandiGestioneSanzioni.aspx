<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiGestioneSanzioni.aspx.vb" Inherits="Provvedimenti.ComandiGestioneSanzioni"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiEnte</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66" leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left">
                    <span class="ContentHead_Title" id="infoEnte" style="WIDTH: 100%">
						<asp:Label id="lblTitolo" runat="server"></asp:Label>
					</span>
				</td>
				<td align="center" width="800" colSpan="2" rowSpan="2">
					<INPUT class="Bottone BottoneSalva hidden" id="btnSalvaMotivazioni" title="Salva Motivazioni" onclick="parent.Visualizza.document.getElementById('btnSalvaMotivazioni').click()" type="button" name="btnSalvaMotivazioni">
					<INPUT class="Bottone BottoneAssocia" id="btnAssocia" title="Associa le sanzioni all'immobile" onclick="parent.Visualizza.document.getElementById('btnAggiornaGriglia').click()" type="button" name="btnAssocia"> 
					<INPUT class="Bottone Bottoneannulla" id="Cancel" title="Esci" onclick="parent.window.close()" tabIndex="6" type="button" name="Cancel">
				</td>
			</tr>
			<TR>
				<TD style="WIDTH: 463px" align="left">
                    <span class="NormalBold_title" id="info" runat="server" style="WIDTH: 100%; HEIGHT: 20px"></span>
				</TD>
			</TR>
		</table>
	</body>
</HTML>
