<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaPesature.aspx.vb" Inherits="OPENgovTIA.RicercaPesature"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>RicercaPesature</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script type="text/javascript" src="../_js/VerificaCampi.js?newversion"></script>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function Search()
			{
			    var IsFatturato=2
			    if (document.getElementById('optYes').value == 1) {
			        IsFatturato = 1;
			    }
			    else if (document.getElementById('optNo').value == 1) {
			        IsFatturato = 0;
			    }
				//se ho messo dal devo mettere anche al
				if ((document.getElementById('TxtDal').value!='' && document.getElementById('TxtAl').value=='') || (document.getElementById('TxtDal').value=='' && document.getElementById('TxtAl').value!=''))
				{
					GestAlert('a', 'warning', '', '', 'E\' necessario inserire entrambe le date del periodo!')
				}
				else
				{
				    document.getElementById('LoadResult').src = 'ResultRicPesature.aspx?TxtCognome=' + document.getElementById('TxtCognome').value + '&TxtNome=' + document.getElementById('TxtNome').value + '&TxtCodFiscale=' + document.getElementById('TxtCodFiscale').value + '&TxtPIva=' + document.getElementById('TxtPIva').value + '&TxtDal=' + document.getElementById('TxtDal').value + '&TxtAl=' + document.getElementById('TxtAl').value + '&TxtCodUtente=' + document.getElementById('TxtCodUtente').value + '&TxtNTessera=' + document.getElementById('TxtNTessera').value + '&TxtCodTessera=' + document.getElementById('TxtCodTessera').value + '&Fatturato=' + IsFatturato + '&TxtFileImport=' + document.getElementById('TxtFileImport').value
				}
			}
				
			function EstraiExcel()
			{
				//se ho messo dal devo mettere anche al
				if ((document.getElementById('TxtDal').value!='' && document.getElementById('TxtAl').value=='') || (document.getElementById('TxtDal').value=='' && document.getElementById('TxtAl').value!=''))
				{
					GestAlert('a', 'warning', '', '', 'E\' necessario inserire entrambe le date del periodo!')
				}
				else
				{
					document.getElementById('CmdStampa').click()
				}
			}

			function Delete()
			{
				if (parent.Visualizza.LoadResult.ResultRicTessere==undefined)
				{
					GestAlert('a', 'warning', '', '', 'Per Eliminare gli Conferimenti, effettuare la ricerca e premere il pulsante Cancella.')
				}
				else
				{
					if (confirm('Si vogliono eliminare gli Conferimenti ricercati?'))
					{
					    parent.Visualizza.LoadResult.document.getElementById('CmdDelete').click()
					}
				}
			}
			
			function AbilitaDelete()
			{
				/*if (document.FrmRicerca.TxtDal.value!='' && document.FrmRicerca.TxtAl.value!='')
				{
					parent.Comandi.document.getElementById('Cancella').style.display='';
				}
				else
				{
					parent.Comandi.document.getElementById('Cancella').style.display='none';
				}*/
			}

			function keyPress()
				{
					if (window.event.keyCode==13)
					{
						Search();
					}
				}	
		</script>
	</head>
	<body class="Sfondo" bottomMargin="0" leftMargin="3" rightMargin="3">
		<form id="Form1" runat="server" method="post">
            <div class="col-md-12">
				<fieldset class="FiledSetRicerca">
					<legend class="Legend">Inserimento filtri di ricerca</legend>
					<div class="col-md-12">
						<div class="col-md-4">
							<p><label id="Label4" class="Input_Label">Cognome</label></p>
							<asp:textbox id="TxtCognome" Runat="server" CssClass="Input_Text col-md-12"></asp:textbox>
                        </div>
                        <div class="col-md-3">
							<p><label id="Label5"class="Input_Label">Nome</label></p>
							<asp:textbox id="TxtNome" Runat="server" CssClass="Input_Text col-md-12"></asp:textbox>
						</div>
						<div class="col-md-2">
							<p><label id="Label6"class="Input_Label">Cod.Fiscale</label></p>
							<asp:textbox id="TxtCodFiscale" Runat="server" CssClass="Input_Text col-md-12"></asp:textbox>
						</div>
						<div class="col-md-2">
							<p><label id="Label1"class="Input_Label">Partita IVA</label></p>
							<asp:textbox id="TxtPIva" Runat="server" CssClass="Input_Text col-md-12"></asp:textbox>
						</div>
						<div class="hidden">
							<p><label id="Label9"class="Input_Label">Cod.Utente</label></p>
							<asp:textbox id="TxtCodUtente" Runat="server" CssClass="Input_Text col-md-12"></asp:textbox>
						</div>
						<div class="col-md-2">
							<p><label id="Label2"class="Input_Label">N.Tessera</label></p>
							<asp:textbox id="TxtNTessera" Runat="server" CssClass="Input_Text col-md-12"></asp:textbox>
						</div>
						<div class="hidden">
							<p><label id="Label3"class="Input_Label">Cod.Tessera</label></p>
							<asp:textbox id="TxtCodTessera" Runat="server" CssClass="Input_Text col-md-12"></asp:textbox>
						</div>
						<div class="col-md-1">
							<p><label id="Label7"class="Input_Label">Data Dal</label></p>
							<asp:textbox id="TxtDal" onblur="txtDateLostfocus(this);VerificaData(this);AbilitaDelete();" onfocus="txtDateGotfocus(this);" Runat="server" CssClass="Input_Text_Right TextDate" maxlength="10"></asp:textbox>
						</div>
						<div class="col-md-1">
							<p><label id="Label8"class="Input_Label">Al</label></p>
							<asp:textbox id="TxtAl" onblur="txtDateLostfocus(this);VerificaData(this);AbilitaDelete();" onfocus="txtDateGotfocus(this);" Runat="server" CssClass="Input_Text_Right TextDate" maxlength="10"></asp:textbox>
						</div>
						<div class="col-md-4">
							<p><label id="Label11"class="Input_Label">File importato</label></p>
							<asp:textbox id="TxtFileImport" Runat="server" CssClass="Input_Text col-md-12"></asp:textbox>
						</div>
						<div class="Input_Label col-md-3">
							<p></p>
                            <asp:radiobutton id="optYes" runat="server" Text="Fatturato" Checked="false" GroupName="optFatt"></asp:radiobutton>
							<asp:radiobutton id="optNo" runat="server" Text="da Fatturare" Checked="false" GroupName="optFatt"></asp:radiobutton>
							<asp:radiobutton id="optAll" runat="server" Text="Tutti" Checked="true" GroupName="optFatt"></asp:radiobutton>
						</div>
					</div>
				</fieldset>
            </div>
            <div class="col-md-12">
                <iframe id="LoadResult" src="../../aspVuota.aspx" frameBorder="0" width="100%" height="100%"></iframe>
            </div>
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
			<asp:button id="CmdStampa" style="DISPLAY: none" Runat="server"></asp:button>
		</form>
	</body>
</HTML>
