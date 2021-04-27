<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicPag.aspx.vb" Inherits="OPENgovTIA.RicPag" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>RicPag</title>
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
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" type="text/javascript">
			function Search()
			{
				var Parametri = '';
				
				if (document.getElementById('txtDataPagamentoDal').value != '' && document.getElementById('txtDataPagamentoAl').value == '')
				{
				    GestAlert('a', 'warning', '', '', 'Valorizzare entrambe le date di Pagamento!');
					return false;
				}if (document.getElementById('txtDataAccreditoDal').value == '' && document.getElementById('txtDataAccreditoAl').value != '') {
				    GestAlert('a', 'warning', '', '', 'Valorizzare entrambe le date di Accredito!');
				    return false;
				}
				Parametri = 'IsFromVariabile=' + document.getElementById('hfIsFromVariabile').value
				Parametri += '&Tributo=' + document.getElementById('hdTributo').value;		
				Parametri += '&Cognome=' + document.getElementById('txtCognome').value;
				Parametri += '&Nome='+document.getElementById('txtNome').value;
				Parametri += '&CFPIVA='+document.getElementById('txtCFPIva').value;
				Parametri += '&AnnoRif='+document.getElementById('txtAnnoRif').value;
				Parametri += '&NAvviso=' + document.getElementById('txtNAvviso').value;
				Parametri += '&DataPagDal=' + document.getElementById('txtDataPagamentoDal').value;
				Parametri += '&DataPagAl=' + document.getElementById('txtDataPagamentoAl').value;
				Parametri += '&DataDal='+document.getElementById('txtDataAccreditoDal').value;
				Parametri += '&DataAl='+document.getElementById('txtDataAccreditoAl').value;
				Parametri += '&NonAbb=' + document.getElementById('chkNonAbb').checked;
				Parametri += '&Flusso=' + document.getElementById('txtFlusso').value;
				Parametri += '&Provenienza=' + document.getElementById('ddlProvenienza').value;
				document.getElementById('LoadResult').src = 'ResultPag.aspx?' + Parametri;
				return true;
			}
			
			function NewPagamento(){
			    //parent.Comandi.location.href = 'ComandiGestPag.aspx?sProvenienza=N&Tributo=' + document.getElementById('hdTributo').value;
			    parent.Comandi.location.href = '../../../aspVuotaRemoveComandi.aspx';
			    parent.Visualizza.location.href = 'GestPag.aspx?IdListPagamento=-1&Tributo=' + document.getElementById('hdTributo').value;
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
	<body class="Sfondo" bottomMargin="0" leftMargin="3" rightMargin="0" topmargin="0">
		<form id="Form1" runat="server" method="post">
			<table id="TblRicerca" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<tr>
					<td>
						<fieldset class="FiledSetRicerca">
							<LEGEND class="Legend">Inserimento parametri di  ricerca</LEGEND>
							<table>
								<tr>
									<td style="WIDTH: 210px"><asp:label id="lblCognome" CssClass="Input_Label" Runat="server">Cognome</asp:label><br />
										<asp:textbox id="txtCognome" CssClass="Input_Text" Runat="server" Width="285px"></asp:textbox></td>
									<td><asp:label id="lblNome" CssClass="Input_Label" Runat="server">Nome</asp:label><br />
										<asp:textbox id="txtNome" CssClass="Input_Text" Runat="server" Width="185px"></asp:textbox></td>
									<td style="WIDTH: 210px"><asp:label id="lblCFPIva" CssClass="Input_Label" Runat="server">Cod.Fiscale/P.IVA</asp:label><br />
										<asp:textbox id="txtCFPIva" CssClass="Input_Text" Runat="server" Width="185px" MaxLength="16"></asp:textbox></td>
									<td style="width:120px"><asp:label id="lblAnno" CssClass="Input_Label" Runat="server">Anno riferimento</asp:label><br />
										<asp:textbox id="txtAnnoRif" CssClass="Input_Text_Right OnlyNumber" Runat="server" Width="80px" MaxLength="4"></asp:textbox></td>
									<td valign="bottom">
									    <asp:CheckBox ID="chkNonAbb" runat="server" CssClass="Input_Label" Text="Non Abbinati" AutoPostBack="true" />
									</td>		
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td>
						<fieldset class="FiledSetRicerca"><legend class="Legend">Inserimento parametri di ricerca avanzata</legend>
							<table>
								<tr>
									<td style="WIDTH: 200px">
                                        <asp:label id="lblNAvviso" CssClass="Input_Label" Runat="server">Numero Avviso</asp:label><br />
										<asp:textbox id="txtNAvviso" CssClass="Input_Text" Runat="server" Width="165px"></asp:textbox>
									</td>
									<td>
										<asp:Label CssClass="Input_Label" Runat="server" id="lblTesto">Data di Pagamento</asp:Label><br />
										<asp:label id="lblDataPagamentoDal" CssClass="Input_Label" Runat="server">Dal</asp:label>&nbsp;
                                        <asp:textbox Runat="server" id="txtDataPagamentoDal" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="lblDataPagamentoAl" CssClass="Input_Label" Runat="server">Al</asp:label>&nbsp;
                                        <asp:textbox Runat="server" id="txtDataPagamentoAl" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate"></asp:textbox>
									</td>
									<td>
										<asp:Label CssClass="Input_Label" Runat="server" id="Label1">Data di Accredito</asp:Label><br />
										<asp:label id="lblDataAccreditoDal" CssClass="Input_Label" Runat="server">Dal</asp:label>&nbsp;
                                        <asp:textbox Runat="server" id="txtDataAccreditoDal" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="lblDataAccreditoAl" CssClass="Input_Label" Runat="server">Al</asp:label>&nbsp;
                                        <asp:textbox Runat="server" id="txtDataAccreditoAl" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate"></asp:textbox>
									</td>
									<td>
                                        <asp:label id="Label2" CssClass="Input_Label" Runat="server">Provenienza</asp:label><br />
										<asp:DropDownList id="ddlProvenienza" CssClass="Input_Text" Runat="server"></asp:DropDownList>
									</td>
                                </tr>
                                <tr>
									<td colspan="3">
                                        <asp:label id="lblFlusso" CssClass="Input_Label" Runat="server">Flusso</asp:label><br />
										<asp:textbox id="txtFlusso" CssClass="Input_Text" Runat="server" Width="700px"></asp:textbox>
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
					<td width="100%">
                        <iframe id="LoadResult" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="450px"></iframe>
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
            <asp:HiddenField id="hdTributo" runat="server" Value="0434" />
            <asp:HiddenField ID="hfIsFromVariabile" runat="server" />
			<asp:button id="btnStampaPag" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="btnStampaNonPag" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="btnStampaPMag" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="btnStampaPMin" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="btnStampaQuadratura" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="btnStampaRiversamento" style="DISPLAY: none" Runat="server"></asp:button>
		</form>
	</body>
</HTML>
