<%@ Page language="c#" Codebehind="Elaborazioneincorso.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.ElaborazioneDocumenti.Elaborazioneincorso" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>Elaborazione in Corso</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
		function Nascondi(){
			parent.frames.item ("Comandi").document.getElementById("btnElabora").style.display ="none";
			parent.frames.item ("Comandi").document.getElementById("btnRicerca").style.display ="none";
			
			 
		}
    </script>  
  </head>
  <body class="Sfondo" MS_POSITIONING="GridLayout" onload="Nascondi()">
	
    <form id="Form1" runat="server" method="post">
        <div id="attesaCarica" runat="server" style="z-index: 101; position: absolute;display:none;">
            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
            <div class="BottoneClessidra">&nbsp;</div>
            <div class="Legend">Attendere Prego</div>
        </div>
     </form>
	
  </body>
</html>
