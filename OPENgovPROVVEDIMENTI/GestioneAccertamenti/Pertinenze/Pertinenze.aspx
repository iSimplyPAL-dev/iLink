<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Pertinenze.aspx.vb" Inherits="Provvedimenti.Pertinenze" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
	<title>Pertinenze</title>
	<meta content="False" name="vs_snapToGrid">
	<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
	<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
	<meta content="JavaScript" name="vs_defaultClientScript">
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></SCRIPT>
</head>
	<body class="SfondoVisualizza" leftMargin="20" topMargin="5" onload="document.formPertinenze.focus();"
		rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<FIELDSET class="classeFiledSet" style="WIDTH: 27.29%; HEIGHT: 59px"><LEGEND class="Legend">Pertinenza
				</LEGEND>
				<asp:button id="btnRibalta" style="Z-INDEX: 101; POSITION: absolute; DISPLAY: none; TOP: 180px; LEFT: 25px"
					runat="server" Text="Ribalta Dati Griglia"></asp:button>
				<TABLE id="TableRifCatastali" cellPadding="0" width="107" align="left" border="0">
					<TR>
						<TD class="Input_Label" style="WIDTH: 92px; HEIGHT: 2px" width="92"><asp:label id="lblFoglio" runat="server">Foglio</asp:label></TD>
						<TD class="Input_Label" style="WIDTH: 58px; HEIGHT: 2px" width="58"><asp:label id="lblNumero" runat="server">Numero</asp:label></TD>
						<TD class="Input_Label" style="WIDTH: 52px; HEIGHT: 2px" width="52"><asp:label id="lblSub" runat="server">SubAlterno</asp:label></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 92px; HEIGHT: 19px">
							<asp:textbox id="txtFoglio" tabIndex="4" runat="server" Width="57px" CssClass="Input_Text_Right OnlyNumber" Height="18px" onkeypress="return NumbersOnly(event,false,false,0)"></asp:textbox>
						<TD style="WIDTH: 58px; HEIGHT: 19px"><asp:textbox id="txtNumero" tabIndex="5" runat="server" Height="18px" CssClass="Input_Text_Right OnlyNumber" Width="48px" onkeypress="return NumbersOnly(event,false,false,0)"></asp:textbox></TD>
						<TD style="WIDTH: 52px; HEIGHT: 19px"><asp:textbox id="txtSubalterno" tabIndex="6" runat="server" Height="18px" CssClass="Input_Text_Right OnlyNumber" Width="48px" onkeypress="return NumbersOnly(event,false,false,0)"></asp:textbox></TD>
					</TR>
				</TABLE>
			</FIELDSET>
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
			<asp:button id="btnFocus" style="Z-INDEX: 102; POSITION: absolute; DISPLAY: none; TOP: 157px; LEFT: 25px"
				runat="server" Height="1px" Width="1px"></asp:button>
			<asp:Button id="btnCercaImmobile" style="Z-INDEX:103; POSITION:absolute; DISPLAY:none; TOP:176px; LEFT:312px"
				runat="server" Text="btnCercaImmobile"></asp:Button></form>
	</body>
</html>
