<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiConfigTipologieSanzioni.aspx.vb" Inherits="Provvedimenti.ComandiConfigTipologieSanzioni"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
    <title>ComandiConfigTipologieSanzioni</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	<script type ="text/javascript">
	function PopolaLabel(){
		document.getElementById("infoEnte").innerText="<%=Session("DESCRIZIONE_ENTE")%>"
		document.getElementById("info").innerText="Configurazione - Tabelle - Voci - <%=Session("DESC_TIPO_PROC_SERV")%>"
	}

	function indietro()
	{
	//alert("pippo");
	//parent.Visualizza.location.href='ConfigTipologieSanzioni.aspx';
	//parent.Visualizza.indietro()
	}

	</script>
</HEAD>
	<body class="SfondoGenerale" onload="PopolaLabel()" bottomMargin="0" leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0" 0?>
		<form id="Form1" runat="server" method="post">
			
				<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
					<tr>
						<td style="WIDTH: 464px; HEIGHT: 18px" align="left">
						<span id="infoEnte" class="ContentHead_Title" style="WIDTH:400px"></span></td>
						<td align="right" width="800" colSpan="2" rowSpan="2">
						<div id="divnuovo">
							<INPUT class="Bottone BottoneNewInsert" id="nuovo" title="Nuovo Inserimento" onclick="parent.Visualizza.Nuovo()" type="button" name="Search">
						</div>
						<div id="divsalva" style="DISPLAY: none">
							<INPUT class="Bottone BottoneApri" id="abilita" title="Abilita" onclick="parent.Visualizza.Abilita()" type="button" name="Search">
							<INPUT class="Bottone Bottonesalva" id="salva" title="Salva" onclick="parent.Visualizza.Salva()" type="button" name="Search">
							<INPUT class="Bottone Bottonecancella" id="Elimina" title="Elimina l'elemento selezionato" onclick="parent.Visualizza.Elimina()" type="button" name="Search">
							<INPUT class="Bottone BottonePulisci hidden" id="pulisci" title="Pulisce la videata" onclick="parent.Visualizza.Pulisci()" type="button" name="Search">
							<INPUT class="Bottone Bottoneannulla" id="indietro" title="Torna alla videata precedente" onclick="parent.Visualizza.location.href='ConfigTipologieSanzioni.aspx';" type="button" name="Search">
						</div>
						</td>
					</tr>
					<tr>
						<td style="WIDTH: 463px" align="left">
							<span id="info" class="NormalBold_title" style="WIDTH:524px;HEIGHT:20px">
							</span>
						</td>
					</tr>
				</table>

		</form>

  </body>
</HTML>
