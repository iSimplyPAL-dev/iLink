<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GestioneAccertamentiOSAP.aspx.vb" Inherits="Provvedimenti.GestioneAccertamentiOSAP"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>GestioneAccertamentiOSAP</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Utility.js?newversion"></SCRIPT>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></SCRIPT>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></SCRIPT>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></SCRIPT>
		<script type="text/javascript">
		    function EseguiAccertamento() {
		        var myIFrame = document.getElementById('loadGridAccertato');
		        var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
		        if (myContent.document.getElementById('GrdDatiAcc') == null) {
		            GestAlert('a', 'warning', '', '', 'Impossibile proseguire con l\'Accertamento della posizione!\n E\' necessario eseguire un inserimento manuale degli Immobili.');
		        }
		        else {
		            if (confirm('Si desidera proseguire con l\'Accertamento della posizione?')) {
		                document.getElementById('attesaElabAccertamento').style.display = '';
		                myContent.document.getElementById('btnAccertamento').click();
		            }
		        }
		        return false;
		    }

		    function gotoVersContribuente() {
		        if (document.getElementById('txtHiddenCodContribuente').value == '-1') {
		            GestAlert('a', 'warning', '', '', 'Impossibile visualizzare i Versamenti del Contribuente.\n Eseguire la ricerca e selezionare un contribuente.');
		        }
		        else {
		            document.getElementById('btngotoVersContribuente').click()
		        }
		        return false;
		    }

		    function RibaltaImmobileAccertamento() {
		        var myIFrame = document.getElementById('loadGridDichiarato');
		        var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
		        if (myContent.document.getElementById('GrdDichiarato') == null) {
		            GestAlert('a', 'warning', '', '', 'Per procedere con il ribaltamento dell\'immobile è necessario ricercare un contribuente e selezionare un anno!!');
		        }
		        else {
		            myContent.RibaltaImmobileAccertamento();
		        }
		    }
		    function FoundAccNONDefinitivo()
			{
				if (confirm('Per contribuente e anno selezionati, è già stato elaborato un Provvedimento di Accertamento NON definitivo.\nProseguendo con la procedura di accertamento, il sistema provvederà a ricalcolarlo.\nContinuare?'))
				{		
				    document.getElementById('btnSearchDichiarazioniEffettivo').click(); 
				}		
				return false;
			}	
			
			function FoundAccDefinitivo()
			{
				//alert("Per contribuente e anno selezionati, è già stato elaborato un Provvedimento di Accertamento definitivo. Impossibile proseguire!");
				if (confirm('Per contribuente e anno selezionati, è già stato elaborato un Provvedimento di ACCERTAMENTO DEFINITIVO.\nProseguendo con la procedura di accertamento, il sistema provvederà a calcolare un nuovo avviso\nmantenendo comunque l\'avviso definitivo.\nContinuare?'))
				{
				    document.getElementById('btnSearchDichiarazioniEffettivo').click(); 
				}
				return false;
			}
				
			function DeleteContrib()
			{
				document.getElementById('txtNominativo').value='';
				document.getElementById('txtHiddenCodContribuente').value='-1';
				document.getElementById('txtHiddenIdDataAnagrafica').value='-1';
				document.getElementById('btnSvuotaSessionContribuente').click();
			}
			
			function ApriRicercaAnagrafe(nomeSessione)
			{ 
				winWidth=980 
				winHeight=680 
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40 
				caratteristiche="toolbar=no,status,left="+myleft+",top="+mytop+",height ="+winHeight+",width="+winWidth+",resizable"
				Parametri="sessionName=" + nomeSessione 
				WinPopUpRicercaAnagrafica=window.open("../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?"+Parametri,"",caratteristiche) 
				return false;
			}

			function checkDatiSelezionati()
			{
			    if (document.getElementById('hdIdContribuente').value == "")
				{
					GestAlert('a', 'warning', '', '', 'Selezionare un contribuente');
					return false;
				}
			    if (document.getElementById('ddlAnno').value == "-1")
				{
					GestAlert('a', 'warning', '', '', 'Selezionare l\'anno d\'accertamento!');
					return false;
				}
				return true;
			}
			

			function msg()
			{
				GestAlert('a', 'warning', '', '', 'Non implementato')
				return false;
			}
			
			function cerca()
			{
			    parent.Visualizza.document.getElementById('btnSearchDichiarazioni').click();
					return false;
			}
			
			function checkDati()
			{
				if (checkDatiSelezionati()==true)
				{
					parent.Comandi.btnAccertamento.style.display = 'none'
					parent.Visualizza.document.getElementById('btnSearchDichiarazioni').click();
					return false;
				}
			}
			
			function ApriVisualizzaPagOSAP(CodContribuente, AnnoRif)
			{
				winWidth=700
				winHeight=400
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40 
				caratteristiche="toolbar=no,status,left="+myleft+",top="+mytop+",height ="+winHeight+",width="+winWidth+",resizable"
				Parametri="CodContribuente=" + CodContribuente + "&AnnoRif=" + AnnoRif
				WinPopUpSanzioni=window.open("PopUpVisualizzaPagamentiOSAP.aspx?"+Parametri,"",caratteristiche) 
			}

			function FineElaborazioneAccertamento(){
				parent.opener.document.getElementById('txtDataRettificaAvviso').value='<% = now.Date.ToShortDateString() %>';
			}
			
			function RiepilogoAccertato(rettificato,id_provvedimento)
			{
			    location.href = 'RiepilogoAccertatoOSAP.aspx?nominativo=' + document.getElementById('txtNominativo').value + '&anno=' + document.getElementById('ddlAnno').value + '&RETTIFICATO=' + rettificato + '&id_provvedimento=' + id_provvedimento
			}
		</script>
	</HEAD>
	<body class="SfondoVisualizza" topMargin="5" onload="document.formRicercaAnagrafica.focus();"
		rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<fieldset class="classeFiledSetIframe100">
				<legend class="Legend">Modalità di Gestione Accertamenti</legend>
				<table id="tablebb" cellPadding="0" width="100%" border="0">
				    <!--blocco dati contribuente--><%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
				    <tr id="TRPlainAnag">
				        <td colspan="2">
				            <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				            <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				        </td>
				    </tr>
				    <tr id="TRSpecAnag">
						<td colspan="2">
						    <table width="100%">
						        <tr>
						            <td>
							            <asp:label id="lblNominativo" runat="server" class="Input_Label">Nominativo</asp:label>&nbsp;
							            <asp:button id="btnFocus" runat="server" Height="1px" Width="1px"></asp:button>
							            <asp:imagebutton id="Imagebutton" runat="server" class="BottoneSel BottoneLista" CausesValidation="False"></asp:imagebutton>
						            </td>
						        </tr>
					            <tr>
						            <td>
							            <asp:textbox id="txtNominativo" tabIndex="4" runat="server" Width="492px" Enabled="False" CssClass="Input_Text" ToolTip="Nominativo"></asp:textbox>
							            <img id="imageDelete" onmouseover="this.style.cursor='hand'" onclick="DeleteContrib();" alt="Pulisci Nominativo Selezionato" src="../../images/cancel.png" Width="10px" Height="10px">
							            <asp:button id="btnRibalta" style="DISPLAY: none" runat="server" Height="2px" Width="1px" Text="Ribalta"></asp:button>
							            <asp:textbox id="txtHiddenIdDataAnagrafica" style="DISPLAY: none" runat="server" Height="20px" Width="24px"></asp:textbox>
						            </td>
					            </tr>
						    </table>
						</td>
					</tr>
					<tr>
						<td width="25%"><asp:label cssclass="Input_Label" id="lblNumeroProvvedimento" runat="server">Anno Accertamento</asp:label></td>
						<td width="75%"></td>
					</tr>
					<tr>
						<td><asp:dropdownlist id="ddlAnno" runat="server" Width="112px" AutoPostBack="True" cssclass="Input_Text"></asp:dropdownlist></td>
						<td width="75%"><asp:checkbox id="chkspese" runat="server" AutoPostBack="False"></asp:checkbox><asp:label cssclass="Input_Label" id="Label2" runat="server">Non calcolare spese di spedizione</asp:label></td>
					</tr>
				</table>
			</fieldset>
			<fieldset class="classeFiledSetIframe100Iframe h230">
				<legend class="Legend">Dichiarato</legend>
				<iframe id="loadGridDichiarato" style="DISPLAY: none" src="../../aspVuota.aspx" frameBorder="0"
					width="100%" height="230"></iframe>
			</fieldset>
			<fieldset class="classeFiledSetIframe100Iframe h250">
				<legend class="Legend">Accertato</legend>
				<iframe id="loadGridAccertato" style="DISPLAY: none" src="../../aspVuota.aspx" frameBorder="0" width="100%" height="230"></iframe>
			</fieldset>
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
			<asp:button id="btnSearchDichiarazioni" style="DISPLAY: none" runat="server" CausesValidation="False"></asp:button>
			<asp:button id="btnSvuotaSessionContribuente" style="DISPLAY: none" runat="server" CausesValidation="False"></asp:button>
			<asp:button id="btngotoVersContribuente" style="DISPLAY: none" runat="server"></asp:button>
			<asp:textbox id="txtCerca" style="Z-INDEX: 101; POSITION: absolute; DISPLAY: none; TOP: 48px; LEFT: 760px"
				runat="server" Width="31px"></asp:textbox><asp:button id="btnSearchDichiarazioniEffettivo" style="DISPLAY: none" runat="server" CausesValidation="False"></asp:button>
			<div id="attesaCarica" style="Z-INDEX: 101; position: absolute; DISPLAY: none;">
                <div class="Legend" style="margin-top:40px;">Ricerca Dichiarato in Corso...</div>
                <div class="BottoneClessidra">&nbsp;</div>
                <div class="Legend">Attendere Prego</div>
			</div>
			<div id="attesaElabAccertamento" style="Z-INDEX: 103; POSITION: absolute; DISPLAY: none;">
                <div class="Legend" style="margin-top:40px;">Elaborazione Accertamento in Corso...</div>
                <div class="BottoneClessidra">&nbsp;</div>
                <div class="Legend">Attendere Prego</div>
			</div>
			<asp:button id="CmdRibaltaUIAnater" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="CmdRibalta" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="BtnAnnulloNoAcc" style="DISPLAY: none" runat="server"></asp:button>
		</form>
	</body>
</HTML>
