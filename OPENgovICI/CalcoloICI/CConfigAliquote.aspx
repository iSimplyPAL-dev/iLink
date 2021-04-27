<%@ Page language="c#" Codebehind="CConfigAliquote.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.CConfigAliquote" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CConfigAliquote</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		function GoToRicercaAliquote()
		{
			parent.Visualizza.location.href="RicercaAliquote.aspx";
		}
		</script>		
	</HEAD>
	<body class="SfondoGenerale" >
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellspacing="0" cellpadding="0" width="100%">
				<tr>
					<td>
						<span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label></span>
						<br />
						<!--*** 20120704 - IMU ***--><!--*** 20140509 - TASI ***-->
						<span id="info">ICI/IMU - Configurazione Aliquote - Gestione</span>
					</td>
					<td align="right">										  
						<input class="Bottone BottoneSalva" id="Insert" onclick="parent.Visualizza.controlliSalva()" type="button" name="Insert" title="Salva Aliquota/Detrazione"> 
						<input class="Bottone BottoneCancella" id="Delete" title="Elimina Aliquota/Detrazione" onclick="parent.Visualizza.controlliElimina()" type="button" name="Delete"> 
						<input class="Bottone BottoneAnnulla" id="Annulla" title="Torna alla pagina di Ricerca." onclick="GoToRicercaAliquote();" type="button" name="Annulla">
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
