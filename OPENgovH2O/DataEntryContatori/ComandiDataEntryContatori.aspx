<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiDataEntryContatori.aspx.vb" Inherits="OpenUtenze.ComandiDataEntryContatori" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>Comandi Ricerca Contatori</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<%if (Session("SOLA_LETTURA")="1") then%>
		<LINK rel="stylesheet" type="text/css" href="../solalettura.css">
		<%end if%>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">
			function associaSub()
			{
				myleft=((screen.width)/2)-350;
				mytop=((screen.height)/2)-250;
				
				var IDCont=parent.Visualizza.document.getElementById("hdCodContatore").value;
				window.open("FrameSubContatore.aspx?contID="+IDCont,"Sub","width=700, height=500, toolbar=no,top="+mytop+",left="+myleft+", menubar=no,status=yes");
			}
			
			function Conferma()
			{				
				parent.Visualizza.Salva('<%=Request.Item("sProvenienza")%>');				
			}
			
			function Annulla()
			{				
				parent.Visualizza.Annulla();	
			}

			function AnnullaSitContrib()
			{				
				parent.Visualizza.location.href='<%=Request.Item("OPENgovPATH")%>/SituazioneContribuente/SitContrib.aspx?COD_CONTRIBUENTE=<%=Request.Item("CodContribuente")%>';
			}
			
			function Stampa()
			{
				parent.Visualizza.Stampa();
			}
			
			function Stampa2()
			{
				parent.Visualizza.Stampa2();
			}
				
			function sostituisciContatore()
			{
				myleft=((screen.width)/2)-250;
				mytop=((screen.height)/2)-100;
				window.open("FrameSostituzione.aspx","Sostituzione","width=500, height=200, toolbar=no,top="+mytop+",left="+myleft+", menubar=no,status=yes");
			}
			
			function attivaContatore()
			{
				myleft=((screen.width)/2)-250;
				mytop=((screen.height)/2)-100;
				var IDCont=parent.Visualizza.document.getElementById("hdCodContatore").value;
				var IdContratto=parent.Visualizza.document.getElementById("hdCodContratto").value;
				window.open("FrameAttivaContatore.aspx?IDCont="+IDCont+"&IdContratto="+IdContratto,"Attivazione","width=500, height=200, toolbar=no,top="+mytop+",left="+myleft+", menubar=no,status=yes");
			}
			
			function ApriContratti()
			{
				parent.Visualizza.ApriContratti();
			}
			
			function ApriLetture()
			{
			    parent.Visualizza.ApriLetture();
			}
		</script>
</HEAD>
	<BODY class="SfondoGenerale" bottomMargin="0" leftMargin="0" topMargin="10" marginheight="0" marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td>
					<asp:label id="Comune" runat="server" Width="100%" CssClass="ContentHead_Title"></asp:label>
				</td>
				<td align="right" rowSpan="2" width="35%">
					<input class="Bottone BottoneGIS" id="GIS" title="Visualizza GIS" onclick="parent.Visualizza.document.getElementById('CmdGIS').click()" type="button" name="GIS"> 
					<input class="Bottone BottoneApriContratto" id="btnApriContratti" title="Visualizza il contratto associato" onclick="ApriContratti();" type="button" name="btnApriContratti" runat="server">
					<input class="Bottone BottoneApriLettureContatori" id="btnApriLetture" title="Visualizza le letture per questo contatore" onclick="ApriLetture();" type="button" name="btnApriLetture" runat="server">
					<input class="Bottone BottoneAssociaSubContatore" id="btnSub" title="Associa un sub contatore" onclick="associaSub();" type="button" name="btnSub" runat="server">
					<input class="Bottone BottoneSostituzione" id="btnSost" title="Effettua la sostituzione del contatore" onclick="Sostituisci();" type="button" name="Sostituisci" runat="server">
					<input class="Bottone BottoneSalva" id="button2" title="Conferma i dati" onclick="Conferma();" type="button" name="Conferma" runat="server">
					<input class="Bottone BottoneAttivaContatore" id="btnAttivaContatore" title="Attiva il contatore corrente" onclick="attivaContatore();" type="button" name="Attiva" runat="server">
					<input class="Bottone BottoneAnnulla" id="btnAnnulla" runat="server" title="Torna alla Pagina  di ricerca" onclick="Annulla();" type="button" name="Annulla">
					<input class="Bottone BottoneAnnulla" id="btnAnnullaSitContrib" runat="server" title="Torna alla Situazione Contribuente" onclick="AnnullaSitContrib();" type="button" name="AnnullaSitContrib">
					&nbsp;
				</td>
			</tr>
			<tr>
				<td align="left">
					<asp:label id="info" CssClass="NormalBold_title" Width="100%" runat="server"></asp:label>
				</td>
			</tr>
		</table>
	</BODY>
</HTML>
