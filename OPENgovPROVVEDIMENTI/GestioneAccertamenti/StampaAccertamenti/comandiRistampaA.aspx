<%@ Page Language="vb" AutoEventWireup="false" Codebehind="comandiRistampaA.aspx.vb" Inherits="Provvedimenti.comandiRistampaA" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>comandiRistampa</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		function PopolaLabel(){
			document.getElementById("infoEnte").innerText="<%=Session("DESCRIZIONE_ENTE")%>"
		    //document.getElementById("info").innerText="Gestione Atti - Stampa"
		    document.getElementById("title").innerText = "Stampa"
        }
        function chiudiFinestra(){			
			//parent.window.close ()
            //parent.opener.focus() 
            parent.parent.document.getElementById('divStampa').style.display = 'none';
            parent.parent.document.getElementById('divAtto').style.display = '';
        }
		</script>
	</HEAD>
	<body class="Sfondo" onload="PopolaLabel()" bottomMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td style="HEIGHT: 18px" align="left">
						<span id="infoEnte" class="ContentHead_Title hidden"></span>
					</td>
					<td align="right" colSpan="2" rowSpan="2">
						<INPUT class="Bottone BottoneAnnulla" id="Chiudi" title="Chiude la finestra" onclick="chiudiFinestra();" type="button" name="Chiudi">
					</td>
				</tr>
				<tr>
					<td align="left">
						<span id="title" class="lstTabRow" style="HEIGHT:20px"></span>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
