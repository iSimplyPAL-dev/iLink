<%@ Page Language="vb" AutoEventWireup="false" Codebehind="cmdRicerca.aspx.vb" Inherits="Provvedimenti.cmdRicerca" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>cmdRicerca</title>
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
	function NuovaRateizzazione(){
		try{
			parent.Visualizza.NuovaRateizzazione();		
		}catch(e){
		}
    }
    function Statistiche() {
        parent.Visualizza.location.href = "statistiche.aspx"
        parent.Comandi.location.href = "cmdStatistiche.aspx"
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
					    <input style="display:none" class="Bottone BottoneRateizzazioni" id="cmdStatistiche" title="Statistiche" onclick="Statistiche()" type="button" name="cmdStatistiche">
						<input class="Bottone BottoneRateizzazioni" id="btnRateizza" title="Rateizza Pagamenti" onclick="NuovaRateizzazione()" type="button" name="btnRateizza">
						<input class="Bottone BottoneNewInsert" id="btnPagamento" title="Inserisci Nuovo Pagamento" onclick="parent.Visualizza.InserisciNuovoPagamento();" type="button" name="btnPagamento">
						<input class="Bottone BottoneRicerca" id="btnRicerca" title="Ricerca" onclick="parent.Visualizza.Ricerca();" type="button" name="btnRicerca">
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 400px">Accertamenti - Pagamenti - Ricerca</span>
                    </td>
                </tr>
            </table>
		</form>
	</body> 

</html>
