<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="aspVuotaMenu.aspx.vb" Inherits="OPENgov.aspVuotaMenu" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="Styles.css" type="text/css" rel="stylesheet" />
	<script type="text/javascript">		
	function Chiudi() {
		parent.location.href = 'Default.aspx';
	}
	</script>
    </head>
	<body class="SfondoGenerale" style="OVERFLOW-X: hidden" leftMargin="0"	MS_POSITIONING="GridLayout">
		<div class="labellogOff"><br />
			&nbsp;Benvenuto <%if Session("usernameAnater") <> "" Then : Response.Write(Session("usernameAnater")) : Else : Response.Write(Session("username")) : End If%> <label class="barra">|</label>
			<label onclick="Chiudi()" class="logOff">Log off</label>
		</div>
        <div class="col-md-12">&nbsp;</div>
	</body>
</html>
