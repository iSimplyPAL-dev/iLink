<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmUI.aspx.cs" Inherits="DichiarazioniICI.FrmUI" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            var regexS = "[\\?&]ParamUIBody=([^&#]*)",
            regex = new RegExp(regexS),
            results = regex.exec(window.location.search);            
            if (results != null) {
                myParam = decodeURIComponent(results[1].replace(/\+/g, " ")).split('$');
                myUrl = 'ImmobileDettaglio.aspx?';
                for (x = 0; x < myParam.length; x++) {
                    myUrl += myParam[x] + '&';
                }
                parent.Visualizza.location.href = myUrl;
            }
        })
    </script>
</head>
<frameset rows="89,*" framespacing="0" border="0" frameborder="no">
	<frame name="logo" src="../Generali/asp/aspLogo.aspx" marginwidth="0" marginheight="0" scrolling="no" noresize>
	<frameset cols="205,*" framespacing="0" border="0" frameborder="no">
		<frame name="viste" src="../Generali/asp/aspMenu.aspx" scrolling="no" marginwidth="0" marginheight="0" noresize>
		<frameset rows="45,*,0,0" framespacing="0" border="0" frameborder="no" id="frameVisualizza">
			<frame name="Comandi" src="../aspVuotaRemoveComandi.aspx" scrolling="no" noresize id="Comandi">
			<frame name="Visualizza" src="../aspVuota.aspx" noresize id="Visualizza">
			<frame name="Basso" src="../aspVuota.aspx" scrolling="no" noresize id="Basso">
			<frame name="Nascosto" src="../aspVuota.aspx" scrolling="no" noresize id="Nascosto">
		</frameset>
	</frameset>
</frameset>
</html>
