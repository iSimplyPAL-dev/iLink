<%@ Page Language="vb" AutoEventWireup="false" Codebehind="StampaInCorso.aspx.vb" Inherits="OpenUtenze.StampaInCorso"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Elaborazione in Corso</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		function Nascondi()
		{		
			parent.Comandi.document.getElementById("EliminaElabora").style.display ="none";
			parent.Comandi.document.getElementById("ApprovaMinuta").style.display = "none";
			parent.Comandi.document.getElementById("ElaborazioneDocumenti").style.display = "none";
			parent.Comandi.document.getElementById("Search").style.display = "none";
			parent.Comandi.document.getElementById("Cancel").style.display = "none";				 
		}				
		</script>
	</HEAD>
	<body class="Sfondo" MS_POSITIONING="GridLayout" onload="Nascondi();">
		<form id="Form1" runat="server" method="post">
            <div id="attesaCarica" runat="server" style="z-index: 101; position: absolute;display:none;">
                <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                <div class="BottoneClessidra">&nbsp;</div>
                <div class="Legend">Attendere Prego</div>
            </div>
		</form>
	</body>
</HTML>
