<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Default.aspx.vb" Inherits="OpenUtenze._Default" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Default</title>
		<link rel="stylesheet" type="text/css" href="Styles.css">
		<script type="text/javascript">

		/*function apri(){	
			myleft=((screen.width)/2)-150
			mytop=((screen.height)/2)-75
			Parametri="Path=/OpenUtenze&Applicazione=OU"
			window.open("login/Login.aspx?"+Parametri,"login","width=300, height=140, status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=yes")		
		}*/
		function apri_(){	
			myleft=((screen.width)/2)-150
			mytop=((screen.height)/2)-75
			//window.open("Ente.aspx","login","width=300, height=140, status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=yes")		
			window.open("login/login.aspx","login","width=300, height=140, status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=yes")		
		}
		</script>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout" onload="apri()" class="logOff" background="images/bgrepeat.jpg">
		<br>
		<br>
		<br>
		<br>
		<br>
		<br>
		<br>
		<br>
		<br>
		<br>
		<br>
		<br>
		<br>
		<br>
		<a href="javascript:apri()"><font color="#000000">Login</font></a>
	</body>
</HTML>
