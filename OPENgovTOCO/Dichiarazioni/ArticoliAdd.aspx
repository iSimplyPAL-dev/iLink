<%@ Page Language="C#" AutoEventWireup="True" Codebehind="ArticoliAdd.aspx.cs" Inherits="OPENgovTOCO.Dichiarazioni.ArticoliAdd"%>
<%@ Register Src="../Wuc/WucArticolo.ascx" TagName="wucArticoloData" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Nuovo Immobile</title>
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
		<script type="text/javascript" src="../../_js/ControlloData.js?newversion"></script>
		<script type="text/javascript" src="../../_js/Toco.js?newversion"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" style="OVERFLOW:auto" bottomMargin="0" leftMargin="0"
		topMargin="0" rightMargin="0" class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<div class="SfondoGenerale" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 45px">
				<table style="WIDTH:100%" border="0">
					<tr valign="top">
						<td>
							<span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
								<asp:Label id="lblTitolo" runat="server"></asp:Label>
							</span>
						</td>
						<td align="right" valign="middle" rowspan="2">&nbsp;
							<asp:ImageButton runat="server" Cssclass="Bottone BottoneSalva" ImageUrl="../../images/Bottoni/transparent28x28.png"
								id="SalvaFromEdit" AlternateText="Salva articolo" onclick="SalvaFromEdit_Click" />
							<asp:ImageButton runat="server" Cssclass="Bottone BottoneSalva" ImageUrl="../../images/Bottoni/transparent28x28.png"
								id="Salva" AlternateText="Salva articolo" onclick="Salva_Click" />
							<asp:ImageButton runat="server" Cssclass="Bottone BottoneAnnulla" id="Cancel" ImageUrl="../../images/Bottoni/transparent28x28.png"
								AlternateText="Torna alla ricerca" onclick="Cancel_Click" />
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<span class="NormalBold_title" id="info" style="WIDTH: 400px; HEIGHT: 20px">
								TOSAP/COSAP - Dichiarazioni - Gestione - Nuovo Articolo</span>
						</td>
					</tr>
				</table>
			</div>
			<div style="OVERFLOW-Y:auto;WIDTH:100%">
				<uc1:wucArticoloData id="wucArticolo" runat="server"></uc1:wucArticoloData>
			</div>
		</form>
	</body>
</HTML>
