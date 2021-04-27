<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConfigGenerale.aspx.vb" Inherits="Provvedimenti.ConfigGenerale"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ConfigGenerale</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function Salva(){
					
				
				intSelected=document.getElementById('ddlAnno').selectedIndex;
				//alert(intSelected);
				//alert(document.getElementById('ddlAnno')[intSelected].value);
				if(document.getElementById('ddlAnno')[intSelected].value=='-1')
				{
				    GestAlert('a', 'warning', '', '', 'Selezionare un Anno!');
				return false;
				}
				
				document.getElementById("btnSalva").click()
			}
			function Clear(){
				document.getElementById("btnPulisci").click() 
			}
    
		</script>
	</HEAD>
	<body class="SfondoVisualizza">
		<form id="Form1" runat="server" method="post">
			<label class="Input_Label">Anno (da Anni Provvedimenti Configurati): </label>
			<asp:dropdownlist id="ddlAnno" runat="server" AutoPostBack="True" CssClass="Input_Text"></asp:dropdownlist><br>
			<br>
			<fieldset class="fieldset"><legend class="Legend"></legend>
				<table cellSpacing="0" cellPadding="2" width="100%" border="0">
					<tr>
						<td style="WIDTH: 296px; HEIGHT: 35px"><asp:radiobutton id="radioAtto0" runat="server" CssClass="Input_Radio" Text="Rientro dati PreAccertamento ad atto definitivo"
								GroupName="radioAtto"></asp:radiobutton></td>
						<td class="Input_Label" rowSpan="2">Configurazione relativa agli atti di 
							Pre-Accertamento, per definire quando la situazione degli immobili 
							pre-accertati diventa la situazione effettiva del contribuente (ad atto 
							definitivo 60 giorni dalla data di notifica - a conferma atto quando viene 
							inserita la data di conferma). Tutti gli immobili con Foglio, Numero e 
							Subalterno variati (da incrocio con catasto), verranno chiusi e riaperti.
						</td>
					</tr>
					<tr>
						<td style="WIDTH: 296px"><asp:radiobutton id="radioAtto1" runat="server" CssClass="Input_Radio" Text="Rientro dati PreAccertamento a conferma Atto"
								GroupName="radioAtto"></asp:radiobutton></td>
					</tr>
				</table>
			</fieldset>
			<br>
			<fieldset class="fieldset"><legend class="Legend"></legend>
				<table cellSpacing="0" cellPadding="2" width="100%" border="0">
					<tr>
						<td style="WIDTH: 296px"><asp:radiobutton id="RadioSaldo0" runat="server" CssClass="Input_Radio" Text="Acconto e Saldo" GroupName="RadioSaldo"></asp:radiobutton></td>
						<!--*** 20120704 - IMU ***-->
						<td class="Input_Label" rowSpan="2">Configurazione relativa al calcolo degli 
							interessi dei Provvedimenti ICI/IMU. In base alla scelta, sarà possibile calcolare 
							gli interessi in Acconto e in Saldo, oppure solo in Saldo.
						</td>
					</tr>
					<tr>
						<td style="WIDTH: 296px"><asp:radiobutton id="RadioSaldo1" runat="server" CssClass="Input_Radio" Text="Saldo" GroupName="RadioSaldo"></asp:radiobutton></td>
					</tr>
				</table>
			</fieldset>
			<table cellSpacing="0" cellPadding="2" width="90%" border="0">
				<tr>
					<td><asp:label id="lblMessage" runat="server" CssClass="NormalRed" Visible="False">lblMessage</asp:label></td>
				</tr>
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
			<asp:button id="btnSalva" style="DISPLAY: none" runat="server" Text="Salva"></asp:button><asp:button id="btnPulisci" style="DISPLAY: none" runat="server" Text="Pulisci"></asp:button></form>
	</body>
</HTML>
