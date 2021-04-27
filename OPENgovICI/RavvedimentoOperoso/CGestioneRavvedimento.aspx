<%@ Page language="c#" Codebehind="CGestioneRavvedimento.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.RavvedimentoOperoso.CGestioneRavvedimento" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CGestioneRavvedimento</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66"
		leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td align="left" style="WIDTH: 614px"><span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label></span></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<input class="Bottone BottoneClear" id="Clear" title="Pulisci i campi" onclick="parent.Visualizza.Clear();" type="button" name="Clear"> 
						<input class="Bottone BottoneF24" id="Excel" title="Stampa Importo dovuto" onclick="parent.Visualizza.StampaExcel();" type="button" name="Excel"> 
						<input class="Bottone BottoneSalva hidden" id="SalvaRO" title="Salva" onclick="parent.Visualizza.SalvaRavvOper();" type="button" name="SalvaRO">
						<input class="Bottone BottoneCalcolo" id="calcolaTotale" title="Calcola Importo Ravvedimento Operoso"	onclick="parent.Visualizza.CalcolaRO();" type="button" name="calcolaTotale">
						<input class="Bottone BottoneRicerca" id="Search" title="Configura ravvedimento Operoso" onclick="parent.Visualizza.ConfiguraRavvOper();" type="button" name="Search"> 
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 614px" align="left"><span class="NormalBold_title" id="info" style="WIDTH: 520px; HEIGHT: 24px">ICI/IMU - Ravvedimento Operoso</span></td>
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
		</form>
	</body>
</HTML>
