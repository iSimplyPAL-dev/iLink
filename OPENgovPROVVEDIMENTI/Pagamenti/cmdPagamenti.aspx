<%@ Page Language="vb" AutoEventWireup="false" Codebehind="cmdPagamenti.aspx.vb" Inherits="Provvedimenti.cmdPagamenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>cmdPagamenti</title>
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
	/*function Abilita_btnSalvaPagamento(valore){
	    document.getElementById("btnSalvaPagamento").style.display = valore;
	    document.getElementById("CmdDeletePagamento").style.display = valore;
	}*/
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
 						<input class="Bottone BottoneCancella" id="CmdDeletePagamento" title="Elimina Pagamento" onclick="parent.Visualizza.document.getElementById('CmdDeletePagamento').click();" type="button" name="CmdDeletePagamento">
						<input class="Bottone BottoneSalva" id="btnSalvaPagamento" title="Salva Pagamento" onclick="parent.Visualizza.document.getElementById('btnSalvaPagamento').click();" type="button" name="btnSalvaPagamento">
						<input class="Bottone BottoneRicerca" id="btnRicerca" title="Ricerca" onclick="parent.Visualizza.document.getElementById('btnSearchProvvedimenti').click();" type="button" name="btnRicerca">
						<input class="Bottone BottoneAnnulla" id="btnAnnulla" title="Torna alla videata di ricerca" onclick="TornaRicerca();" type="button" name="btnAnnulla">
                   </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 400px">Accertamenti - Pagamenti - Inserimento</span>
                    </td>
                </tr>
            </table>
		</form>
	</body> 
</html>
