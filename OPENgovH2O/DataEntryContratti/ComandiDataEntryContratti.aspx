<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiDataEntryContratti.aspx.vb" Inherits="OpenUtenze.ComandiDataEntryContratti"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiDataEntryContratti</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		function EliminaContratto(){
			if (confirm('Il contratto verrà eliminato definitivamente: continuare?'))
			{
				parent.Visualizza.DeleteContratto();
			}
		}
		
		function controllacontatore(){		
			//alert("Ok, sei in controllacontatore.");
		    if (parent.Visualizza.document.getElementById('txtidContatore').value == "")
			{
				//alert("Il contratto non è ancora stato creato");
				Buttonpreventivo.style.display="none";
				//alert("Bottone di stampa del preventivo disabilitato");
				button1.style.display="none";
				//alert("Bottone di stampa del contratto disabilitato");
				btnvoltura.style.display="none";
				//alert("Bottone di voltura del contratto disabilitato");
				btnElimina.style.display="none";
			}
			else
			{
				//alert("Il contratto è esistente");
				Buttonpreventivo.style.display='';
				//alert("Bottone di stampa del preventivo ora visibile");
				button1.style.dispaly='';
				//alert("Bottone di stampa del contratto ora visibile");
				btnvoltura.style.display='';
				//alert("Bottone di voltura del contratto ora visibile");
				btnElimina.style.display='';
			}
		}
		
		function Stampa()
			{
				parent.Visualizza.Stampa();
			}
		function Stampa2()
			{
			   parent.Visualizza.Stampa2();
			}
		
		function Conferma() {
            console.log("eccomi che confermo")
		    //alert("Ok, questa è la funzione di salvataggio del contratto");

            //versione originale
		    //parent.Visualizza.Salva();

            //versione nuova funzionante
            parent.Visualizza.Salva();
		}
		
		function Annulla()
			{
			    parent.Visualizza.Annulla();
			}

			function CessaContratto() {
			    //alert("Ok, passa dalla funzione per cessare il contratto");
			    myleft = ((screen.width) / 2) - 250;
			    mytop = ((screen.height) / 2) - 100;
			    window.open("FrameCessazione.aspx?data=" + parent.Visualizza.document.getElementById('txtDataCessazione').value + "&IdContatore=" + parent.Visualizza.document.getElementById('hdCodContatore').value + "&IdUtente=" + parent.Visualizza.document.getElementById('HDTextCodUtente').value, "Cessazione", "width=500, height=200, toolbar=no,top=" + mytop + ",left=" + myleft + ", menubar=no,status=yes");
			}
		</script>
	</HEAD>
	<body class="SfondoGenerale" bottomMargin="0" leftMargin="0" topMargin="10" marginwidth="0"
		marginheight="0" onload="controllacontatore();">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td>
					<asp:label id="Comune" runat="server" Width="100%" CssClass="ContentHead_Title"></asp:label>
				</td>
				<td align="right" rowSpan="2" width="30%">
					<input class="Bottone BottonePreventivo" id="Buttonpreventivo" title="Stampa il preventivo" onclick="Stampa2();" type="button" name="Buttonpreventivo" runat="server" alt="Stampa il preventivo"/>
					<input class="Bottone BottoneWord" id="button1" title="Stampa il contratto" onclick="Stampa();" type="button" name="stampa" runat="server" alt="Stampa il contratto"/>
					<input class="Bottone BottoneVoltura" id="btnvoltura" title="Voltura questo contratto" alt="Voltura questo contratto" name="BottoneVoltura" runat="server" type="button"/>
					<input class="Bottone BottoneSalva" id="button2" title="Conferma i dati" onclick="Conferma();" type="button" name="Conferma" runat="server"/>
					<input class="Bottone BottoneCancella" id="btnElimina" name="btnElimina" title="Elimina il contratto" onclick="EliminaContratto();" type="button" name="Elimina"/>
					<input class="Bottone BottoneAnnulla" title="Torna alla Pagina  di ricerca" onclick="Annulla();" type="button" name="Annulla"/>
					&nbsp;
				</td>
			</tr>
			<tr>
				<td align="left">
					<asp:label id="info" CssClass="NormalBold_title" Width="100%" runat="server"></asp:label>
				</td>
			</tr>
		</table>
	</body>
</HTML>
