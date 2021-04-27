<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="aspmenu.aspx.vb" Inherits="OPENgov.aspmenu" %>
<HTML>
	<HEAD>
		<title>aspmenu</title>
		<LINK href="../../Styles.css" type="text/css" rel="stylesheet">
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Utility.js?newversion"></script>
        <script src="../script/menu.js?newversion" type="text/Javascript"></script>
		<script language="jscript">		
		function Chiudi() {
		    GestAlert('c', 'question', 'CmdLogOut', '', 'Uscire dall\'applicativo?')
		    //top.window.location.href = '../../Default.aspx';
		}
		</script>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style>HTML { SCROLLBAR-FACE-COLOR: #0079ff; SCROLLBAR-HIGHLIGHT-COLOR: #e8f8ff; SCROLLBAR-SHADOW-COLOR: #0079ff; SCROLLBAR-3DLIGHT-COLOR: #0079ff; SCROLLBAR-ARROW-COLOR: #ffffff; SCROLLBAR-TRACK-COLOR: #0079ff; SCROLLBAR-BASE-COLOR: #e8f8ff }
	BODY { SCROLLBAR-FACE-COLOR: #0079ff; SCROLLBAR-HIGHLIGHT-COLOR: #e8f8ff; SCROLLBAR-SHADOW-COLOR: #0079ff; SCROLLBAR-3DLIGHT-COLOR: #0079ff; SCROLLBAR-ARROW-COLOR: #ffffff; SCROLLBAR-TRACK-COLOR: #0079ff; SCROLLBAR-BASE-COLOR: #e8f8ff }
		</style>
	</HEAD>
	<body class="SfondoGenerale" style="OVERFLOW-X: hidden" bgColor="papayawhip" leftMargin="0"	MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<div id="LoginInfo" class="labellogOff" runat="server"></div><br />
            <div id="menu_wrapper"></div>
            <asp:HiddenField ID="hfIdEnte" runat="server" />
            <asp:HiddenField ID="hfDescrizioneEnte" runat="server" />
            <asp:HiddenField ID="hfIdTributo" runat="server" />
            <asp:Button ID="CmdLoadMenuEnte" runat="server" Text="" CssClass="hidden" OnClick="LoadMenuEnte"></asp:Button>            
            <asp:Button ID="CmdLogOut" runat="server" Text="" CssClass="hidden" OnClick="LogOut"></asp:Button>            
			<DIV id="lblRelease" style="DISPLAY: none; FONT-WEIGHT: bold; FONT-SIZE: 8pt; Z-INDEX: 101; LEFT: 48px; WIDTH: 77px; CURSOR: hand; POSITION: absolute; TOP: 560px; HEIGHT: 15px"
				onclick="OpenRelease()" ms_positioning="FlowLayout">Release 1.0</DIV>
			<div class="div_scroll_menu" style="Z-INDEX: 104; LEFT: 4px; OVERFLOW-X: hidden; WIDTH: 198px; POSITION: absolute; TOP: 8px; HEIGHT: 552px;display:none" id="Navigator">
				<asp:label id="lblMenuWF" style="Z-INDEX: 102; LEFT: 4px; POSITION: absolute; TOP: 4px" runat="server" Width="188px"></asp:label>
			</div>
			<asp:textbox id="txtHiddenIDMenu" style="DISPLAY: none; Z-INDEX: 102; LEFT: 8px; POSITION: absolute; TOP: 492px" runat="server" Width="164px" Height="20px"></asp:textbox>
            <asp:textbox id="txtHiddenSingoliIDMenu" style="DISPLAY: none; Z-INDEX: 103; LEFT: 8px; POSITION: absolute; TOP: 516px" runat="server" Width="164px" Height="24px"></asp:textbox>
		</form>
	</body>
</HTML>