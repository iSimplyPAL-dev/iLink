<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InserimentoManualeImmobileOSAP.aspx.vb" Inherits="Provvedimenti.InserimentoManualeImmobileOSAP"%>
<%@ Register Src="../Wuc/WucArticolo.ascx" TagName="wucArticoloData" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>InserimentoManualeImmobileOSAP</title>
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
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" src="../../_js/ControlloData.js?newversion"></script>
		<script type="text/javascript" src="../../_js/Toco.js?newversion"></script>
		<script type="text/javascript">
			function Test()
			{
				parent.parent.opener.parent.document.getElementById('loadGridAccertato').src='SearchDatiAccertatoOSAP.aspx';
			}
		</script>
</HEAD>
	<body class="SfondoVisualizza" MS_POSITIONING="GridLayout" topmargin="0" leftmargin="0">
		<form id="Form1" runat="server" method="post">
			<div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: hidden">
				<table style="WIDTH:100%" border="0">
					<tr valign="top">
						<td>
							<span class="ContentHead_Title" id="infoEnte">
								<asp:Label id="lblTitolo" runat="server"></asp:Label>
							</span>
						</td>
						<td align="right" valign="middle" rowspan="2">&nbsp;
							<asp:ImageButton runat="server" Cssclass="Bottone BottoneAssocia" ImageUrl="../../images/Bottoni/transparent28x28.png"
								id="btnRibalta" AlternateText="Associa gli immobili al soggetto" />
							<asp:ImageButton runat="server" Cssclass="Bottone BottoneAnnulla" ImageUrl="../../images/Bottoni/transparent28x28.png"
								id="Cancel" AlternateText="Torna alla ricerca" />
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<span class="NormalBold_title" id="info" style="HEIGHT: 20px">TOSAP/COSAP - 
								Accertamento - Inserimento Manuale Immobile</span>
						</td>
					</tr>
				</table>
			</div>
			<div style="OVERFLOW-Y:auto;WIDTH:815px;HEIGHT:547px">
				<uc1:wucArticoloData id="wucArticolo" runat="server"></uc1:wucArticoloData>
			</div>
			<asp:textbox id="txtAnnoAccertamento" runat="server" Visible="False"></asp:textbox>
			<asp:textbox id="txtCodContribuente" runat="server" Visible="False"></asp:textbox>
			<asp:textbox id="txtRiaccerta" runat="server" visible="False"></asp:textbox>
			<asp:button id="btnRiaccerta" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
			<asp:textbox id="TxtProgGriglia" style="DISPLAY: none" Runat="server">-1</asp:textbox>
			<asp:textbox id="TxtIdLegame" style="DISPLAY: none" Runat="server">-1</asp:textbox>
		</form>
	</body>
</HTML>
