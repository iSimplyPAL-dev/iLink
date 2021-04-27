<%@ Page Language="vb" AutoEventWireup="false" Codebehind="NuovoInserimentoTipoVoci.aspx.vb" Inherits="Provvedimenti.NuovoInserimentoTipoVoci"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>NuovoInserimentoTipoVoci</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		function Back(){
			parent.Comandi.location.href ="ComandiConfigTipoVoci.aspx"
			location.href ="ConfigTipoVoci.aspx"
		}
		function abilitaConfigura(){
		    parent.Comandi.document.getElementById('Configura').disabled = false
		}
		function disabilitaConfigura(){
		    parent.Comandi.document.getElementById('Configura').disabled=true
		}
		function Clear(){
			document.getElementById ("ddlTributo").disabled=false
			document.getElementById ("ddlCapitolo").disabled=false
			document.getElementById ("ddlProvvedimenti").disabled=false
			document.getElementById ("ddlVoce").disabled=false
			document.getElementById ("ddlMisura").disabled=false
			//document.getElementById ("ddlCalcolata").disabled=false
			document.getElementById ("ddlFase").disabled=false
			
			document.getElementById ("ddlTributo").options(0).selected=true
			if (document.getElementById ("ddlCapitolo").options(0)!=null)
				document.getElementById ("ddlCapitolo").options(0).selected=true
			if (document.getElementById ("ddlProvvedimenti").options(0)!=null)
				document.getElementById ("ddlProvvedimenti").options(0).selected=true
			if (document.getElementById ("ddlVoce").options(0)!=null)
				document.getElementById ("ddlVoce").options(0).selected=true
			if (document.getElementById ("ddlMisura").options(0)!=null)
				document.getElementById ("ddlMisura").options(0).selected=true
			if (document.getElementById ("ddlFase").options(0)!=null)
				document.getElementById ("ddlFase").options(0).selected=true
			//document.getElementById ("ddlCalcolata").options(0).selected=true
			document.getElementById ("btnPulisci").click()
			//ifrmConfigura.location.href="../../../../Generali/asp/aspVuota.aspx"
		}
		function Delete(){
			if (confirm('Confermi l\'eliminazione della voce selezionata?')){
				document.getElementById ("btnCancella").click()
				Clear()
			}
		}
		function Save(){
			
			//ifrmConfigura.formVoci.btnSalva.click()
			document.getElementById ("btnSalva").click()
		}
		function Configura(){
			iTributo=document.getElementById ("ddlTributo").value
			iCapitolo=document.getElementById ("ddlCapitolo").value
			iProvvedimenti=document.getElementById ("ddlProvvedimenti").value
			iVoce=document.getElementById ("ddlVoce").value
			iMisura=document.getElementById ("ddlMisura").value
			iFase=document.getElementById ("ddlFase").value
			sVoceAttribuita=document.getElementById ("txtVoceAttribuita").value
			sIDTIPOVOCE=document.getElementById ("txtIDTIPOVOCE").value
			//iCalcolato=document.getElementById ("ddlCalcolata").options.value
			
			//alert(iCapitolo);
			if (sIDTIPOVOCE!=""){
			
				document.getElementById ("ddlTributo").disabled=true
				document.getElementById ("ddlCapitolo").disabled=true
				document.getElementById ("ddlProvvedimenti").disabled=true
				document.getElementById ("ddlVoce").disabled=true
				document.getElementById ("ddlMisura").disabled=true
				document.getElementById ("ddlFase").disabled=true
				document.getElementById ("txtVoceAttribuita").disabled=true
				//document.getElementById ("ddlCalcolata").disabled=true
				
				if (iTributo != "-1" && iCapitolo != "-1" && iProvvedimenti != "-1" && iMisura != "-1")//&& iVoce != "-1" 
				{
					//parent.Comandi.Form1.Salva.disabled=false
					//parent.Comandi.Form1.Elimina.disabled=false
					location.href="configuraVoci.aspx?CODTRIBUTO="+iTributo+"&CODCAPITOLO="+iCapitolo+"&CODTIPOPROVVEDIMENTO="+iProvvedimenti+"&CODVOCE="+iVoce+"&CODMISURA="+iMisura+"&CODFASE="+iFase+"&VoceAttribuita="+sVoceAttribuita+"&IDTIPOVOCE="+sIDTIPOVOCE
					parent.Comandi.location.href="ComandiInserimentoVoci.aspx"
					
				}else{
				    GestAlert('a', 'warning', '', '', 'Selezionare tutti i parametri che identificano la voce')
				}
			}else{
			    GestAlert('a', 'warning', '', '', 'È necessario salvare la voce prima di poter configurare i valori voce')
			}
			
			
		}
		function AbilitaCombo(){
			iCapitolo=document.getElementById ("ddlCapitolo").value
			iTributo=document.getElementById ("ddlTributo").value
			//alert(iCapitolo);
			//alert(iTributo);
			
			if (iCapitolo=='0004')
			{
				document.getElementById ("ddlTributo").disabled=false
				document.getElementById ("ddlCapitolo").disabled=false
				document.getElementById ("ddlProvvedimenti").disabled=false
				document.getElementById ("ddlVoce").disabled=false
				document.getElementById ("ddlMisura").disabled=true
				document.getElementById ("ddlFase").disabled=true
				document.getElementById ("txtVoceAttribuita").disabled=false
							
			}
			else
			{			
				//document.getElementById ("ddlTributo").disabled=true
				document.getElementById ("ddlCapitolo").disabled=true
				document.getElementById ("ddlProvvedimenti").disabled=true
				document.getElementById ("ddlVoce").disabled=true
				document.getElementById ("ddlMisura").disabled=true
				document.getElementById ("ddlFase").disabled=false
				document.getElementById ("txtVoceAttribuita").disabled=false
			
			}
			//if (iTributo!='8832' && iTributo!='-1'){
			//	document.getElementById ("ddlFase").disabled=true
			//}
			//alert (document.getElementById ("ddlFase").disabled)
		
		}
		function DisabilitaCombo(){
			document.getElementById ("ddlTributo").disabled=true
			document.getElementById ("ddlCapitolo").disabled=true
			document.getElementById ("ddlProvvedimenti").disabled=true
			document.getElementById ("ddlVoce").disabled=true
			document.getElementById ("ddlMisura").disabled=true
			//document.getElementById ("ddlCalcolata").disabled=true
			document.getElementById ("ddlFase").disabled=true
			document.getElementById ("txtVoceAttribuita").disabled=true
			//ifrmConfigura.location.href="../../../../Generali/asp/aspVuota.aspx"
			
		}
		function Abilita(valore){
			/*if (valore=="P"){
				document.getElementById ("ddlCalcolata").disabled=false 
			}else{
				document.getElementById ("ddlCalcolata").options(0).selected=true
				document.getElementById ("ddlCalcolata").disabled=true
			}*/
		
		}
		</script>
</HEAD>
	<body class="SfondoVisualizza">
		<form id="Form1" runat="server" method="post">
			<fieldset class="classeFiledSet100"><legend class="Legend"></legend>
				<table cellSpacing="0" cellPadding="2" width="100%" border="0">
					<tr>
						<td colspan="2" class="Input_Label">
							<P>Capitolo Sanzioni: Misura "Percentuale" o "Fissa", Voce "Tipologia di Sanzione". 
								Da configurare per ogni tipologia di Provvedimento e, in caso di 
								Pre-Accertamento, anche per fase.</P>
							<P>Capitolo Interessi: Misura "Interessi", Voce "Interessi". Da configurare (scelta 
								della tipologia di interesse) per ogni tipologia di Provvedimento e, in caso di 
								Pre-Accertamento, anche per fase.</P>
							<P>Capitolo Spese di Notifica: Misura "Fissa", Voce "Spese di Notifica". Da 
								configurare per ogni tipologia di Provvedimento&nbsp;senza distinzione di fase.</P>
							<P>Voce attribuita è la descrizione che comparirà sul documento.</P>
							<P>&nbsp;</P>
						</td>
					</tr>
					<tr>
						<td colspan="2">
						<asp:TextBox id="txtIDTIPOVOCE" runat="server" style="DISPLAY:none"></asp:TextBox>
						</td>
					</tr>
					<tr class="Input_Label">
						<td width="15%" style="HEIGHT: 21px">Tributo</td>
						<td width="85%" style="HEIGHT: 21px"><asp:dropdownlist id="ddlTributo" runat="server" cssclass="Input_Text" AutoPostBack="True" Width="590px"></asp:dropdownlist></td>
					</tr>
					<tr class="Input_Label">
						<td>Capitolo</td>
						<td><asp:dropdownlist id="ddlCapitolo" runat="server" cssclass="Input_Text" Width="280px" AutoPostBack="True"></asp:dropdownlist></td>
					</tr>
					<tr class="Input_Label">
						<td>Provvedimenti</td>
						<td><asp:dropdownlist id="ddlProvvedimenti" runat="server" cssclass="Input_Text" Width="280px"></asp:dropdownlist></td>
					</tr>
					<tr class="Input_Label">
						<td>Misura</td>
						<td><asp:dropdownlist id="ddlMisura" runat="server" cssclass="Input_Text" Width="280px"></asp:dropdownlist></td>
					</tr>
					<tr class="Input_Label">
						<td>Fase</td>
						<td>
							<asp:dropdownlist id="ddlFase" runat="server" cssclass="Input_Text" Width="280px"></asp:dropdownlist>
							<!--<asp:dropdownlist id="ddlCalcolata" runat="server" cssclass="Input_Text" Width="180px" ></asp:dropdownlist>-->
						</td>
					</tr>
					<tr class="Input_Label">
						<td>Voce</td>
						<td><asp:dropdownlist id="ddlVoce" runat="server" cssclass="Input_Text" Width="590px"></asp:dropdownlist></td>
					</tr>
					<tr class="Input_Label">
						<td>Voce Attribuita</td>
						<td>
							<asp:TextBox id="txtVoceAttribuita" runat="server" cssclass="Input_Text" Width="592px"></asp:TextBox></td>
					</tr>
				</table>
				<asp:Button id="btnSalva" style="DISPLAY:none" runat="server" Text="Salva"></asp:Button>
				<asp:Button id="btnCancella" style="DISPLAY:none" runat="server" Text="Cancella"></asp:Button>
				<br>
				<asp:Button id="btnPulisci" style="DISPLAY:none" runat="server" Text="Pulisci"></asp:Button>
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
			<!--<iframe class="SfondoVisualizza" id="ifrmConfigura" name="ifrmConfigura" src="../../../../Generali/asp/aspVuota.aspx"
				frameBorder="0" width="100%" height="420"></iframe>-->
		</form>
	</body>
</HTML>
