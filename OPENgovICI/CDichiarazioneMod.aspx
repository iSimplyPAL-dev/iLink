<%@ Page language="c#" Codebehind="CDichiarazioneMod.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CDichiarazioneMod" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CDichiarazione</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%> 
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%> 
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
        <script>
            function ribalta() {
                if (confirm('Si desidera inserire la dichiarazione per l\'immobile?'))
                parent.Visualizza.document.getElementById('cmdPrecarica').click();
            }
        </script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66"
		leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				<tr>
					<td style="WIDTH: 464px; HEIGHT: 20px" align="left"><span id="infoEnte" class="ContentHead_Title" style="WIDTH:400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label></span></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<input class="Bottone Bottonebonifica" id="Bonifica" title="Bonifica la dichiarazione." style="display:none;" onclick="parent.Visualizza.document.getElementById('btnBonifica').click()" type="button" name="Bonifica">
						<input style="display:none;" class="Bottone BottoneNewInsert" id="DatiAggiuntivi" title="Visualizza i dati aggiuntivi della dichiarazione." onclick="parent.Visualizza.document.getElementById('btnDatiAggiuntivi').click()" type="button" name="DatiAggiuntivi"> 
                        <input class="Bottone BottoneRibalta hidden" id="Precarica" title="Precarica" onclick="ribalta();" type="button" name="Precarica">
						<input class="Bottone BottoneNewInsert" id="New" title="Inserisci un nuovo immobili alla dichiarazione." onclick="parent.Visualizza.document.getElementById('btnImmobili').click()" type="button" name="Insert"> 
						<input class="Bottone BottoneApri" id="Unlock" onclick="parent.Visualizza.document.getElementById('btnAbilita').click()" type="button" name="Unlock" title="Abilita i contolli per scrivere.">
						<input class="Bottone BottoneSalva" id="Insert" onclick="parent.Visualizza.Salva()" type="button" name="Insert" title="Salva dichiarazione."> 
						<input class="Bottone Bottonecancella" id="Delete" title="Elimina dichiarazione." onclick="parent.Visualizza.document.getElementById('btnElimina').click()" type="button" name="Delete">&nbsp;
                        <input class="Bottone Bottoneannulla" id="Annulla" title="Torna alla pagina di gestione." onclick="parent.Visualizza.document.getElementById('btnIndietro').click()" type="button" name="Delete">
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 463px" align="left"><span id="info" class="NormalBold_title" style="WIDTH:400px;HEIGHT:20px;">Dichiarazioni - Inserimento dichiarazione</span></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
