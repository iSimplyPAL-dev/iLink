<%@ Page language="c#" Codebehind="cmdElaborazioneDocumenti.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.ElaborazioneDocumenti.cmdElaborazioneDocumenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <head>
		<title>Comandi Elaborazione Documenti</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" Content="C#"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
  </head>
	<body MS_POSITIONING="GridLayout" class="SfondoGenerale" bottomMargin="0"
		bgColor="#ffcc66" leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td align="left" style="WIDTH: 641px"><span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label></span></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<input class="Bottone BottoneCancella" id="btnCancella" title="Elimina Elaborazioni Effettuate" onclick="parent.Visualizza.document.getElementById('btnElimina').click();"  type="button" name="btnElimina"/>
						<input class="Bottone BottoneWord" id="btnElabora" title="Elabora Documenti" onclick="parent.Visualizza.document.getElementById('DivAttesa').style.display = ''; parent.Visualizza.document.getElementById('divStampa').style.display = ''; parent.Visualizza.document.getElementById('divElabDoc').style.display = 'none'; parent.Visualizza.document.getElementById('btnElabora').click();"  type="button" name="btnElabora"/>
						<input class="Bottone BottoneRicerca" id="btnRicerca" title="Ricerca" onclick="parent.Visualizza.document.getElementById('DivAttesa').style.display = '';parent.Visualizza.document.getElementById('btnRicerca').click();" type="button" name="btnRicerca" />
						&nbsp;<input class="Bottone BottoneAnnulla" id="Esci" title="Esci" onclick="parent.Visualizza.document.getElementById('btnClose').click();" type="button" name="Esci"/>
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 641px" align="left">
						<span class="NormalBold_title" id="info" style="WIDTH: 580px; HEIGHT: 24px">ICI/IMU/TASI - Elaborazione Documenti relativi  all'anno <% = Request["AnnoRiferimento"].ToString()%></span>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
