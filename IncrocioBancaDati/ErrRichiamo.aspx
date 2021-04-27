<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ErrRichiamo.aspx.vb" Inherits="OPENgov.ErrRichiamo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title><% = System.Configuration.ConfigurationManager.AppSettings("TitoloApplicazione") %></title>
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
    </head>
    <body class="SfondoVisualizza">
        <form id="Form1" runat="server" method="post">
            <asp:Label runat="server" CssClass="Legend" Text="Si è verificato un errore. Utente/Ente non abilitato o parametri insufficienti."></asp:Label>
        </form>
    </body>
</html>
