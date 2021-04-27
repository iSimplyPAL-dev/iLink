<%@ Page language="c#" Codebehind="CVersamentiMod.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CVersamentiMod" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CVersamenti</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	</HEAD>
	<body MS_POSITIONING="GridLayout" class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66"
		leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0"> 
				<tr>
					<td style="WIDTH: 464px; HEIGHT: 20px" align="left"><span id="infoEnte" class="ContentHead_Title" style="WIDTH:400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label></span></td>
					<td align="right" width="800" colSpan="2" rowSpan="2"><input class="Bottone Bottonebonifica" id="Bonifica" title="Bonifica la dichiarazione." onclick="parent.Visualizza.formRicercaAnagrafica.btnBonifica.click()" type="button" name="Bonifica" style="display:none;">&nbsp;<input class="Bottone BottoneUnlock" id="Unlock" onclick="parent.Visualizza.formRicercaAnagrafica.btnAbilita.click()" type="button" name="Unlock" title="Abilita i controlli per scrivere.">
						<input class="Bottone Bottoneinserimento" id="Insert" onclick="parent.Visualizza.formRicercaAnagrafica.btnSalva.click()" type="button" name="Insert" title="Salva versamento.">
						 <input class="Bottone Bottonecancella" id="Cancel" onclick="parent.Visualizza.formRicercaAnagrafica.btnElimina.click()" type="button" name="Cancel" title="Elimina versamento.">&nbsp;
						 <input class="Bottone Bottoneannulla" id="Delete" title="Torna alla pagina di gestione." onclick="parent.Visualizza.formRicercaAnagrafica.btnIndietro.click()" type="button" name="Delete">
						 <input class="Bottone Bottoneannulla" id="backAttuali" title="Torna alla videata Consultazione Dati Attuali." onclick="parent.Visualizza.location.href='<%=ConfigurationManager.AppSettings["PATH_APPLICAZIONE"].ToString()%>/provvedimenti/consultazionedatiattuali/datisintetici.aspx'" type="button" name="backAttuali">
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 463px" align="left"><span id="info" class="NormalBold_title" style="WIDTH:400px;HEIGHT:20px;">
							ICI/IMU - Versamenti - Dettaglio</span></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
