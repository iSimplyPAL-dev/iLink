<%@ Page language="c#" Codebehind="ElaborazioniCatasto.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.ConfrontaConCatasto.ElaborazioniCatasto" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<TITLE></TITLE>
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
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<!--*** 20120704 - IMU ***-->
			<asp:Button id="btnICI" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 136px" runat="server"
				Text="Stampa Posizioni di catasto non presenti in ICI/IMU" Width="312px" onclick="btnICI_Click"></asp:Button>
			<asp:Button id="btnClasseCatastato" style="Z-INDEX: 102; LEFT: 8px; POSITION: absolute; TOP: 104px"
				runat="server" Text="Stampa Diff ICI/IMU - CATASTO" onclick="btnClasseCatastato_Click"></asp:Button>
			<asp:Button id="btnPassaggioProprieta" style="Z-INDEX: 102; LEFT: 10px; POSITION: absolute; TOP: 75px"
				runat="server" Text="Stampa Passaggio Proprietà" onclick="btnPassaggioProprieta_Click"></asp:Button>
			<asp:TextBox id="txtAnno" style="Z-INDEX: 103; LEFT: 8px; POSITION: absolute; TOP: 168px" CssClass="Input_Text_Right OnlyNumber" runat="server"></asp:TextBox>
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
