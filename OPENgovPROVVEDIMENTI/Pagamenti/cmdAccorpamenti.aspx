<%@ Page Language="vb" AutoEventWireup="false" Codebehind="cmdAccorpamenti.aspx.vb" Inherits="Provvedimenti.cmdAccorpamenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>cmdAccorpamenti</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	<script type="text/javascript" >
	function TornaRicerca(){
		parent.Visualizza.location.href="Ricerca.aspx"
		parent.Comandi.location.href="cmdRicerca.aspx"
	}
	function Abilita_btnRateizza(valore){
		document.getElementById ("btnRateizza").style.display =valore
	}
	function Abilita_btnCaricaRateizzazioni(valore){
		document.getElementById ("btnCaricaRateizzazioni").style.display =valore
	}
	function Abilita_btnCalcolaTotale(valore){
		document.getElementById ("btnCalcolaTotale").style.display =valore
	}
	function Abilita_btnSalvaRate(valore){
		document.getElementById ("btnSalvaRate").style.display =valore
	}
	</script> 
  </head>
  <body class="SfondoGenerale" >
		<form id="Form1" runat="server" method="post">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table1">
                <tr valign="top">
                    <td align="left">
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" rowspan="2">
						<input class="Bottone BottoneSalva" id="btnSalvaRate" title="Salva le Rateizzazioni" onclick="parent.Visualizza.SalvaRateizzazioni();" type="button" name="btnSalvaRate">
						<input class="Bottone BottoneCalcolo" id="btnCalcolaTotale" title="Calcola i Totali Rate" onclick="parent.Visualizza.Totale();" type="button" name="btnCalcolaTotale"> 
						<input class="Bottone BottoneRateizzazioni" id="btnCaricaRateizzazioni" title="Carica Rateizzazioni" onclick="parent.Visualizza.CalcolaRateizzazioni()" type="button" name="btnCaricaRateizzazioni"> 
						<input class="Bottone BottoneBollettino" id="btnRateizza" title="Rateizza Provvedimenti selezionati" onclick="parent.Visualizza.RateizzaSelezionati();" type="button" name="btnRateizza">
						<input class="Bottone BottoneRicerca" id="btnRicerca" title="Ricerca" onclick="parent.Visualizza.document.getElementById('btnSearchProvvedimenti').click();" type="button" name="btnRicerca">
						<input class="Bottone BottoneAnnulla" id="btnAnnulla" title="Torna alla videata di ricerca" onclick="TornaRicerca();" type="button" name="btnAnnulla">
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 400px">Accertamenti - Rateizzazioni - Inserimento</span>
                    </td>
                </tr>
            </table>
		</form>
  </body>
</html>
