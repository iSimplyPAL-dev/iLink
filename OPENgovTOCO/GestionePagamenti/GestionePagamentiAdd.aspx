<%@ Register Src="../Wuc/WucGestionePagamenti.ascx" TagName="wucGestPagamentiData" TagPrefix="uc1" %>
<%@ Page language="c#" Codebehind="GestionePagamentiAdd.aspx.cs" AutoEventWireup="True" Inherits="OPENGovTOCO.GestionePagamenti.GestionePagamentiAdd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>GestionePagamentiAdd</title>
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
		<script type="text/javascript" src="../_js/Toco.js?newversion"></script>
		<style type="text/css">
			.impSuperato td { background-color: red; color: white; font-size:10px;}
			.impSuperatoCellImporto { background-color: green; }
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
							<input title="Cancella Pagamento" type="button" class="Bottone BottoneCancella" id="btnCancella" onclick="unlockPagamento();" disabled="disabled">
							<input title="Modifica Pagamento" type="button" class="Bottone BottoneApri" id="btnModifica" disabled="disabled">
							<asp:button title="Salva Pagamento" id="btnSalva" Runat="server" OnClick="btnSalva_OnClick" Cssclass="Bottone BottoneSalva"></asp:button>						
							<asp:button title="Ricerca" id="btnRicerca" Runat="server" OnClick="btnRicerca_OnClick" Cssclass="Bottone Bottonericerca"></asp:button>							
							<a title="Torna alla ricerca" href="GestionePagamentiSearch.aspx?NewSearch=false" class="Bottone BottoneAnnulla" style="display:inline-block"></a>
						</td>
					</tr>
					<tr>
						<td style="WIDTH: 463px">
							<span id="info" runat="server" class="NormalBold_title" style="WIDTH: 400px; HEIGHT: 20px">Tosap/Cosap - Pagamenti - Gestione Pagamenti</span>
						</td>
					</tr>
				</table>
			</div>
			<asp:button id="btnRibalta" Text="btnRibalta" style="DISPLAY:none" Runat="server" OnClick="btnRibalta_OnClick"></asp:button>
			<div style="width:100%">
			    <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				<uc1:wucGestPagamentiData id="wucGestionePagamenti" runat="server" style="Z-INDEX: 0;width=100%"></uc1:wucGestPagamentiData>
			</div>
		</form>
	</body>
</HTML>
