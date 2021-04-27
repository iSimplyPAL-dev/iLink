<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ModuloTributi.aspx.vb" Inherits="OPENgov.ModuloTributi" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title><% = System.Configuration.ConfigurationManager.AppSettings("TitoloApplicazione") %></title>
	    <meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
	    <meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
	    <meta name="vs_defaultClientScript" content="JavaScript">
	    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
    </head>
    <frameset rows="89,*" framespacing="0" border="0" frameborder="no">
	    <frame name="logo" src="../Generali/asp/aspLogo.aspx" marginwidth="0" marginheight="0" scrolling="no" noresize>
	    <frameset cols="205,*" framespacing="0" border="0" frameborder="no">
		    <frame name="viste"  src="../Generali/asp/aspmenu.aspx" scrolling="no" marginwidth="0" marginheight="1" noresize>
		    <frameset rows="45,*,0,0" framespacing="0" border="1" frameborder="no" id="frameVisualizza">
			    <FRAME name="Comandi" src="<%=srcComandi%>" scrolling="no" noresize>
			    <FRAME name="visualizza" src="<%=srcVisualizza%>" noresize>
			    <FRAME name="Basso" src="../aspVuota.aspx" scrolling="no" noresize>
			    <FRAME name="Nascosto" src="../aspVuota.aspx" scrolling="yes" noresize >
		    </frameset>
	    </frameset>
    </frameset>
</html>
