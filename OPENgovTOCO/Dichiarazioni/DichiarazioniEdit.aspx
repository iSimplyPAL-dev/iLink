<%@ Page Language="C#" AutoEventWireup="True" Codebehind="DichiarazioniEdit.aspx.cs" Inherits="OPENgovTOCO.Dichiarazioni.DichiarazioniEdit"%>
<%@ Register Src="../Wuc/WucDichiarazione.ascx" TagName="wucDichiarazioneData" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Nuova Dichiarazione</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
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
	</HEAD>
	<body MS_POSITIONING="GridLayout" style="OVERFLOW:auto" bottomMargin="0" leftMargin="0"
		topMargin="0" rightMargin="0" class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: auto">
				<table style="WIDTH:100%" border="0">
					<tr valign="top">
						<td>
							<span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
								<asp:Label id="lblTitolo" runat="server"></asp:Label>
							</span>
						</td>
						<td align="right" valign="middle" rowspan="2">
							<asp:ImageButton runat="server" Cssclass="Bottone BottoneSalva" ImageUrl="../../images/Bottoni/transparent28x28.png"
								id="Salva" AlternateText="Salva dichiarazione" onclick="Salva_Click" />
							<asp:ImageButton runat="server" Cssclass="Bottone BottoneAnnulla" id="Cancel" ImageUrl="../../images/Bottoni/transparent28x28.png"
								AlternateText="Torna alla ricerca" onclick="Cancel_Click" />
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<span class="NormalBold_title" id="info" style="WIDTH: 400px; HEIGHT: 20px">
								TOSAP/COSAP - Dichiarazioni - Gestione - Edit</span>
						</td>
					</tr>
				</table>
			</div>
			<asp:button id="btnRibalta" Text="btnRibalta" style="DISPLAY:none" Runat="server"></asp:button>
			<div style="OVERFLOW-Y:auto;WIDTH:100%">
				<uc1:wucDichiarazioneData id="wucDichiarazione" runat="server" style="Z-INDEX: 0"></uc1:wucDichiarazioneData>
			</div>
		</form>
	</body>
</HTML>
