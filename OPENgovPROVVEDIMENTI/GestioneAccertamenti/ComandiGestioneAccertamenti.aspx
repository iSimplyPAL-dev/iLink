<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiGestioneAccertamenti.aspx.vb" Inherits="Provvedimenti.ComandiGestioneAccertamenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiEnte</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script>
		    function RibaltaImmobileAccertamento() {
			var myIFrame = parent.Visualizza.document.getElementById('loadGridDichiarato');
			var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
			if (myContent.document.getElementById('GrdDichiarato') == null) {
			//if (parent.Visualizza.frames.item('loadGridDichiarato').document.getElementById('RibesGridAnagrafica')==null){
				GestAlert('a', 'warning', '', '', 'Per procedere con il ribaltamento dell\'immobile è necessario ricercare un contribuente e selezionare un anno!!');
			}
			else{
				/*if (parent.Visualizza.frames.item('loadGridAccertato').document.getElementById('RibesGridAnagrafica')!=null){
					GestAlert('a', 'warning', '', '', 'ATTENZIONE sono già stati ribaltati degli immobili!\nPer effettuare il ribaltamento automatico eliminare gli immobili precedentemente inseriti.');
				}
				else{*/
					myContent.RibaltaImmobileAccertamento();//parent.Visualizza.loadGridDichiarato.RibaltaImmobileAccertamento();
				//}
			}
		}
		
		function VisualizzaLabel(){						
			//parent.Comandi.info.innerText="<%=Session("DESC_TIPO_PROC_SERV")%>"
			parent.Comandi.infoEnte.innerText="<%=Session("DESCRIZIONE_ENTE")%>";
			parent.Comandi.info.innerText= 'Accertamenti ICI/IMU - Gestione';//"<%=Session("DESC_TIPO_PROC_SERV") %>" + '  -  Ricerca'			
		}		
		
		function EseguiAccertamento()
		{			
			var myIFrame = parent.Visualizza.document.getElementById('loadGridAccertato');
			var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
			if (myContent.document.getElementById('GrdAccertato') == null) {
			//if (parent.Visualizza.frames.item('loadGridAccertato').document.getElementById('RibesGridAnagrafica')==null){
			//if ( parent.Visualizza.document.getElementById('loadGridAccertato').document.getElementById('RibesGridAnagrafica')==null){
			//if ( parent.Visualizza.frames.item('loadGridAccertato').frmSearchResults.RibesGridAnagrafica==null){
					GestAlert('a', 'warning', '', '', 'Impossibile proseguire con l\'Accertamento della posizione!\n E\' necessario eseguire un inserimento manuale degli Immobili.');
				}
			else
				{
					if (confirm('Si desidera proseguire con l\'Accertamento della posizione?'))
					{
						parent.Visualizza.document.getElementById('attesaElabAccertamento').style.display='';
						//parent.Visualizza.document.getElementById('loadGridAccertato').frmSearchResults.btnAccertamento.click()
						//parent.Visualizza.frames.item('loadGridAccertato').document.getElementById('btnAccertamento').click();
                        myContent.document.getElementById('btnAccertamento').click();
					}					
				}				
			return false;											
		}
		
		function gotoVersContribuente()
		{
				if ( parent.Visualizza.document.getElementById('txtHiddenCodContribuente').value =='-1')
				
				{
					GestAlert('a', 'warning', '', '', 'Impossibile visualizzare i Versamenti del Contribuente.\n Eseguire la ricerca e selezionare un contribuente.');
				}
				/*else if ( parent.Visualizza.document.getElementById('ddlAnno').value =='-1')
				
				{
					GestAlert('a', 'warning', '', '', 'Impossibile visualizzare i Versamenti del Contribuente.\n Eseguire la ricerca e selezionare un contribuente.');
				}*/				
				else
				{				
				    parent.Visualizza.document.getElementById('btngotoVersContribuente').click()
														   								
				}
				return false;										
		}				
		</script>
	</HEAD>
	<body onload="VisualizzaLabel();" class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66"
		leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left"><asp:label id="infoEnte" runat="server" Width="400px" CssClass="ContentHead_Title"></asp:label></td>
				<td align="right" width="800" colSpan="2" rowSpan="2">
					<INPUT class="Bottone BottoneElabora" id="btnAccertamento" title="Accerta" onclick="EseguiAccertamento();" type="button" name="btnAccertamento"> 
                    <INPUT class="Bottone BottoneUserHome hidden" id="VersContribuente" title="Visualizza Versamenti del Contribuente" onclick="gotoVersContribuente();" type="button" name="VersContribuente"> 
                    <INPUT class="Bottone BottoneRicerca" id="Search" title="Cerca Dichiarato da accertare" style="BORDER-RIGHT:1px outset; BORDER-TOP:1px outset; DISPLAY:none; BORDER-LEFT:1px outset; WIDTH:30px; BORDER-BOTTOM:1px outset; HEIGHT:30px" onclick="parent.Visualizza.checkDati();return false;" type="button" name="Search">
					<INPUT class="Bottone BottoneRibalta" id="btnRibaltaImmobile" title="Ribalta Immobile" onclick="RibaltaImmobileAccertamento();" type="button" name="btnRibaltaImmobile"> 
					<!--		<INPUT class="Bottone BottonePulisci hidden" id="Clear" title="Pulisci videata per nuova Associazione"
						style="BORDER-RIGHT: 1px outset; BORDER-TOP: 1px outset; BORDER-LEFT: 1px outset; WIDTH: 30px; BORDER-BOTTOM: 1px outset; HEIGHT: 30px"
						onclick="parent.Visualizza.formRicercaAnagrafica.Clear()" type="button" name="Clear">
			-->
				</td>
			</tr>
			<TR>
				<TD style="WIDTH: 463px" align="left">
					<asp:label id="info" CssClass="NormalBold_title" Width="524px" runat="server" Height="20px"></asp:label></TD>
			</TR>
		</table>
        <div id="divDialogBox" class="col-md-12">
            <div class="modal-box">
                <div id="divAlert" class="modal-alert">
                    <span class="closebtnalert" onclick="CloseAlert()">&times;</span>
                    <p id="pAlert">testo di esempio</p>
                    <input type="text" class="prompttxt"/>
                    <button id="btnDialogBoxeOK" class="confirmbtn Bottone BottoneOK" onclick="DialogConfirmOK();"></button>&nbsp;
                    <button id="btnDialogBoxeKO" class="confirmbtn Bottone BottoneKO" onclick="DialogConfirmKO();"></button>
                    <button id="CmdDialogConfirmOK" class="hidden" onclick="RaiseDialogConfirmOK();"></button>
                    <button id="CmdDialogConfirmKO" class="hidden" onclick="RaiseDialogConfirmKO();"></button>
                    <input type="hidden" id="hfCloseAlert" />
                    <input type="hidden" id="hfDialogOK" />
                    <input type="hidden" id="hfDialogKO" />
                </div>
            </div>
            <input type="hidden" id="cmdHeight" value="0" />
        </div>
	</body>
</HTML>
