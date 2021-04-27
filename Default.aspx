<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Default.aspx.vb" Inherits="OPENgov._Default" enableViewState="False"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title><% = System.Configuration.ConfigurationManager.AppSettings("TitoloApplicazione") %></title>
        <link href="images/logo_opengov.png" rel="shortcut icon" type="image/x-icon" />
		<LINK href="Styles.css" type="text/css" rel="stylesheet">
		<script type="text/javascript">

		function Controlla(){
			document.getElementById ("TestValidita").value ="1"
		}
		function PulisciCampi(){
			document.getElementById ("Username").value="";
			document.getElementById ("Password").value="";
		}
		</script>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css"> 
            body { overflow:hidden; }
		</style>
	</HEAD>
	<body class="SfondoGenerale margin_Zero" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<IMG height="92" src="images/testata.jpg" width="100%">
			<table class="margin_Zero" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>												
					<td>
                        <iframe class="SfondoGenerale" id="ifrmLogin" src="Login/login.aspx?NOME_OPERATORE=<%=Request.item("NOME_OPERATORE")%>&amp;PASSWORD=<%=Request.item("PASSWORD")%>&amp;PROFILO=<%=Request.item("PROFILO")%>&amp;LOGINTYPE=<%=Request.item("LOGINTYPE")%>&amp;CODICE_ENTE=<%=Request.item("CODICE_ENTE")%>" frameBorder="0" width="250" height="300"></iframe>
					</td>					
					<td class="Input_Label_login" style="WIDTH: 100%" vAlign="top" align="left"><label class="Input_Label_login_Account14"><% = System.Configuration.ConfigurationManager.AppSettings("TitoloApplicazione") %></label>
						<hr class="hrlogin">
					</td>
				</tr>
			</table>
			</form>
	</body>
</HTML>
