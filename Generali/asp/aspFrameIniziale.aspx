<html>
	<head>
        <TITLE> <% = System.Configuration.ConfigurationManager.AppSettings("TitoloApplicazione") %> </TITLE>
        <link href="../../images/logo_opengov.png" rel="shortcut icon" type="image/x-icon" />
    	<%	
            Dim Logo = "aspLogo.aspx"
            Dim Viste = "aspMenu.aspx"
            Dim Nascosto = "../../aspSvuota.aspx"
            Dim Basso = "../../aspVuota.aspx"
            Dim Comandi = "aspComandiVuota.aspx"
            Dim visualizza = "../../LeggiParametriEnte.aspx?CODENTE=" + Request.Item("CODENTE")
            If Request.Item("utente") <> Nothing Then
                visualizza = "../../Login/ResetPassword.aspx?utente=" + Request.Item("utente")
                Viste = "../../aspVuotaMenu.aspx"
                Basso = "../../aspVuotaRemoveComandi.aspx"
            End If
        %>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
        <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		    window.onload = window.focus;

			$(window).on('beforeunload', function () {
			    $('#CmdLogOut', parent.parent.frames["viste"].document).click();
			});
		</script>
	</head>
	<frameset rows="89,*" framespacing="0" border="0" frameborder="no">
		<frame name="logo" src="<%=Logo%>" marginwidth="0" marginheight="0" scrolling="no" noresize>
		<frameset cols="220,*" framespacing="0" border="0" frameborder="no">
            <frame name="viste" src="<%=Viste%>" scrolling="no" marginwidth="0" marginheight="1" noresize>
			<frameset rows="45,*,0,0,0" framespacing="0" border="0" frameborder="no" id="frameVisualizza" name="frameVisualizza">
				<frame name="Comandi" src="<%=Comandi%>" scrolling="no" noresize id="Comandi">
				<frame name="Visualizza" src="<%=Visualizza%>" noresize id="Visualizza">
                <frame name="DialogBoxe" src="aspDialogBoxe.aspx" noresize id="DialogBoxe">
				<frame name="Basso" src="<%=Basso%>" scrolling="no" noresize id="Basso">
				<frame name="Nascosto" src="<%=Nascosto%>" scrolling="yes" noresize id="Nascosto">
			</frameset>
		</frameset>
	</frameset>
</html>
