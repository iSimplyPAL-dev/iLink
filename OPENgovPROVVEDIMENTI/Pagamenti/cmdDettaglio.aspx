<%@ Page Language="vb" AutoEventWireup="false" Codebehind="cmdDettaglio.aspx.vb" Inherits="Provvedimenti.cmdDettaglio" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>cmdDettaglio</title>
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
	function Abilita_btnDelete(valore){
		document.getElementById ("btnDelete").style.display =valore
	}
	function Abilita_btnPagamento(valore){
		document.getElementById ("btnPagamento").style.display =valore
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
						<input class="Bottone BottoneCancella" id="btnDelete" title="Elimina Accorpamento/Provvedimento" onclick="parent.Visualizza.EliminaAccorpamento();" type="button" name="btnDelete" style="display:none">
						<input class="Bottone BottoneNewInsert" id="btnPagamento" title="Inserisci Nuovo Pagamento" onclick="parent.Visualizza.InserisciNuovoPagamento();" type="button" name="btnPagamento">
						<input class="Bottone BottoneAnnulla" id="btnAnnulla" title="Torna alla videata di ricerca" onclick="TornaRicerca();" type="button" name="btnAnnulla">
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 400px">Accertamenti - Pagamenti - Dettaglio</span>
                    </td>
                </tr>
            </table>
		</form>
	</body> 
</html>
