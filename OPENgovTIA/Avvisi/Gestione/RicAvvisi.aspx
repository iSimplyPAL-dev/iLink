<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicAvvisi.aspx.vb" Inherits="OPENgovTIA.RicAvvisi" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head>
		<title>RicAvvisi</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function Search() {
			    DivAttesa.style.display = ''; 
				//loadGrid.location.href = 'ResultAvvisi.aspx?IdRuolo='+document.FrmRicerca.TxtIdElab.value+'&DdlAnno='+document.FrmRicerca.DdlAnno.value+'&DdlTipoRuolo='+document.FrmRicerca.DdlTipoRuolo.value+'&DdlNProgRuolo='+document.FrmRicerca.DdlNProgRuolo.value+'&TxtCognome='+document.FrmRicerca.TxtCognome.value+'&TxtNome='+document.FrmRicerca.TxtNome.value+'&TxtCFPIVA='+document.FrmRicerca.TxtCFPIVA.value+'&TxtCodCartella='+document.FrmRicerca.TxtCodCartella.value+'&ChkSgravate='+document.FrmRicerca.ChkSgravate.checked
			    loadGrid.src = 'ResultAvvisi.aspx?IsFromVariabile=' + document.getElementById('hfIsFromVariabile').value + '+&IdRuolo=' + document.getElementById('TxtIdElab').value + '&DdlAnno=' + document.getElementById('DdlAnno').value + '&DdlTipoRuolo=' + document.getElementById('DdlTipoRuolo').value + '&DdlNProgRuolo=' + document.getElementById('DdlNProgRuolo').value + '&TxtCognome=' + document.getElementById('TxtCognome').value + '&TxtNome=' + document.getElementById('TxtNome').value + '&TxtCFPIVA=' + document.getElementById('TxtCFPIVA').value + '&TxtCodCartella=' + document.getElementById('TxtCodCartella').value + '&ChkSgravate=' + document.getElementById('ChkSgravate').checked;
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
			<table id="TblRicerca" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<tr>
					<td>
						<fieldset class="FiledSetricerca">
							<legend class="Legend">Inserimento filtri di ricerca</legend>
							<table id="TblParametri" cellSpacing="1" cellPadding="1" width="100%" border="0">
								<tr width="100%">
									<td>
										<asp:label id="Label1" Runat="server" CssClass="Input_Label">Anno</asp:label><br />
										<asp:dropdownlist id="DdlAnno" Runat="server" CssClass="Input_Text" AutoPostBack="true"></asp:dropdownlist>
									</td>
									<td>
										<asp:label id="Label6" Runat="server" CssClass="Input_Label">Tipo Ruolo</asp:label><br />
										<asp:dropdownlist id="DdlTipoRuolo" Runat="server" CssClass="Input_Text" AutoPostBack="true"></asp:dropdownlist></td>
									<td>
										<asp:label id="Label7" Runat="server" CssClass="Input_Label" style="display:none">N.Ruolo</asp:label><br />
										<asp:dropdownlist id="DdlNProgRuolo" Runat="server" CssClass="Input_Text" style="display:none"></asp:dropdownlist>
									</td>
									<td>
										<asp:label CssClass="Input_Label" Runat="server" id="Label8">Sgravate</asp:label><br />
										<asp:CheckBox id="ChkSgravate" runat="server" CssClass="Input_CheckBox_NoBorder" Text=" "></asp:CheckBox>
									</td>
								</tr>
								<tr>
									<td>
										<asp:Label id="Label2" CssClass="Input_Label" Runat="server">Cognome</asp:Label><br />
										<asp:textbox id="TxtCognome" runat="server" CssClass="Input_Text" Width="300px"></asp:textbox>
									</td>
									<td>
										<asp:Label id="Label3" CssClass="Input_Label" Runat="server">Nome</asp:Label><br />
										<asp:textbox id="TxtNome" runat="server" CssClass="Input_Text" Width="200px"></asp:textbox>
									</td>
									<td>
										<asp:Label id="Label4" CssClass="Input_Label" Runat="server">Cod.Fiscale/P.IVA</asp:Label><br />
										<asp:textbox id="TxtCFPIVA" runat="server" CssClass="Input_Text" Width="170px" MaxLength="16"></asp:textbox>
									</td>
									<td>
										<asp:Label id="Label5" CssClass="Input_Label" Runat="server">N.Avviso</asp:Label><br />
										<asp:textbox id="TxtCodCartella" runat="server" CssClass="Input_Text" Width="170px"></asp:textbox>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td colspan="2">
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
				<tr>
					<td>
						<iframe id="loadGrid" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="650"></iframe>
					</td>
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
			<asp:TextBox style="DISPLAY: none" id="TxtIdElab" Runat="server">-1</asp:TextBox>
            <asp:Button ID="CmdStampa" Runat="server" style="DISPLAY: none"></asp:Button>
            <asp:HiddenField ID="hfIsFromVariabile" runat="server" />
		</form>
	</body>
</HTML>
