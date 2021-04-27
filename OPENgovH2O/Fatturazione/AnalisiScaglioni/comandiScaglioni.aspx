<%@ Page Language="vb" AutoEventWireup="false" Codebehind="comandiScaglioni.aspx.vb" Inherits="OpenUtenze.comandiScaglioni"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
    <title>Comandi Scaglioni</title>
<meta name=GENERATOR content="Microsoft Visual Studio .NET 7.1">
<meta name=CODE_LANGUAGE content="Visual Basic .NET 7.1">
<meta name=vs_defaultClientScript content=JavaScript>
<meta name=vs_targetSchema content=http://schemas.microsoft.com/intellisense/ie5>
<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
  </HEAD>
<BODY class="SfondoGenerale" leftMargin=0 topMargin=6 marginheight="0" marginwidth="0">
	<table border=0 cellSpacing=0 cellPadding=0 width="100%" align=right>
		<tr>
			<td>
				<asp:label id=Comune runat="server" Width="100%" CssClass="ContentHead_Title"></asp:label>
			</td>
			<td rowSpan=2 width="40%" colSpan=2 align=right>
				<!--Bottoni per la modifica degli scaglioni-->
				<input id=InserisciAS class="Bottone BottoneNewInsert" title="Inserisci Nuovo Scaglione" onclick=parent.Visualizza.Inserisci(); type=button name=InserisciAS style ="display:none">
				<input id=SalvaAS class="Bottone BottoneSalva" title="Salva Scaglioni" onclick=parent.Visualizza.Salva(); type=button name=SalvaAS style ="display:none"> 
				<input id=AnnullaAS class="Bottone BottoneAnnulla" title="Annulla le modifiche" onclick=parent.Visualizza.Annulla(); type=button name=AnnullaAS style ="display:none"> 
		
				<!--Bottoni principali della videata-->
				<input id="Modifica" class="Bottone BottoneModifica hidden" title="Modifica Scaglioni" onclick="parent.Visualizza.Modifica();" type="button" name="Modifica"> 
				<input id="Excel" class="Bottone BottoneExcel" title="Esporta in Excel" onclick="parent.Visualizza.Excel();" type="button" name="Excel"> 
				<input id="Ricerca" class="Bottone BottoneRicerca" title="Ricerca" onclick="parent.Visualizza.Ricerca();" type="button" name="Ricerca"> 
				<input id="Pulisci" class="Bottone BottonePulisci hidden" title="Pulisci i campi" onclick="parent.Visualizza.Pulisci();" type="button" name="Pulisci"> 
			</td>
		</tr>
		<tr>
			<td align=left>
				<asp:label id=info runat="server" Width="100%" CssClass="NormalBold_title"></asp:label>
			</td>
		</tr>
	</table>

</body>
</html>
