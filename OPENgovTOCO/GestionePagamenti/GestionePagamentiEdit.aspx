<%@ Page language="c#" Codebehind="GestionePagamentiEdit.aspx.cs" AutoEventWireup="True" Inherits="OPENGovTOCO.GestionePagamenti.GestionePagamentiEdit" %>
<%@ Register Src="../Wuc/WucGestionePagamenti.ascx" TagName="wucGestPagamentiData" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>GestionePagamentiEdit</title>
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
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" src="../../_js/Toco.js?newversion"></script>
		<style type="text/css">
			.impSuperato TD { BACKGROUND-COLOR: red; COLOR: white; FONT-SIZE: 10px }
			.impSuperatoCellImporto { BACKGROUND-COLOR: green }
		</style>
	</HEAD>
	<body class="SfondoGenerale" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="2"
		topMargin="6" rightMargin="2" marginheight="0" marginwidth="0" style="HEIGHT:100%">
		<form id="Form1" runat="server" method="post">
			<div>
				<table cellspacing="0" cellpadding="0" width="100%" border="0" id="Table1">
					<tr>
						<td style="WIDTH: 464px; HEIGHT: 20px" align="left">
							<span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
								<asp:Label id="lblTitolo" runat="server"></asp:Label>
							</span>
						</td>
						<td align="right" width="800" colspan="2" rowspan="2">
							<asp:button id="btnCancella" Runat="server" OnClick="btnCancella_OnClick" Cssclass="Bottone BottoneCancella"></asp:button>
							<input type="button" class="Bottone BottoneApri" id="btnModifica" onclick="unlockPagamento();">
							<asp:button id="btnSalva" Runat="server" OnClick="btnSalva_OnClick" Cssclass="Bottone BottoneSalva" Enabled="False"></asp:button>
							<asp:button id="btnRicerca" Runat="server" Cssclass="Bottone Bottonericerca" Enabled="False"></asp:button>
							<a href="GestionePagamentiSearch.aspx?NewSearch=false" class="Bottone BottoneAnnulla" style="display:inline-block"></a>
						</td>
					</tr>
					<tr>
						<td style="WIDTH: 463px">
							<span id="info" runat="server" class="NormalBold_title" style="WIDTH: 400px; HEIGHT: 20px">Tosap/Cosap - Pagamenti - Gestione Pagamenti</span>
						</td>
					</tr>
				</table>
			</div>
			<div style="width:100%">
			    <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				<uc1:wucGestPagamentiData id="wucGestionePagamenti" runat="server" style="Z-INDEX: 0;width=100%"></uc1:wucGestPagamentiData>
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
		</form>
		
		<script type="text/javascript">
				
			function unlockPagamentoBtn()
			{
				document.getElementById('<%= btnSalva.ClientID %>').removeAttribute("disabled");
			}		
					
		</script>
	</body>
</HTML>
