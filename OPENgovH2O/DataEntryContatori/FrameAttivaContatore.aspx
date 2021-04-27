<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrameAttivaContatore.aspx.vb" Inherits="OpenUtenze.FrameAttivaContatore" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>Attivazione del contatore</title>
		<meta content="False" name="vs_showGrid">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script type="text/javascript" src="../../_js/MyUtility.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" src="../../_js/NumbersOnly.js?newversion"></script>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
		<script language='javascript' type='text/javascript'>
		function Sostituisci()
		{  
		    if (document.getElementById('txtDataAttiv').value == "")
			{
				alert("Popolare la data di attivazione, oppure uscire.");
				return;
			}
			//la data di attivazione deve essere coerente con la data di sottoscrizione del contratto
		if (document.getElementById('TxtDataSottoscrizione').value!='') 
			{
				var starttime = document.getElementById('TxtDataSottoscrizione').value
				var endtime = document.getElementById('txtDataAttiv').value
				//Start date split to UK date format and add 31 days for maximum datediff
				starttime = starttime.split("/"); 
				starttime = new Date(starttime[2],starttime[1]-1,starttime[0]); 
				//End date split to UK date format 
				endtime = endtime.split("/");
				endtime = new Date(endtime[2],endtime[1]-1,endtime[0]); 
				if (endtime<starttime)
				{
					if (confirm('La Data di Attivazione e\' minore alla Data di Sottoscrizione.\nProseguire ugualmente?'))
					{
					    parent.opener.parent.Visualizza.document.getElementById("txtDataAttivazione").value=document.getElementById('txtDataAttiv').value;
						parent.opener.parent.Visualizza.document.getElementById("TxtDataAttivazioneRibaltata").value=document.getElementById('txtDataAttiv').value;
						window.close();
					}
				}
				else
				{
				    parent.opener.parent.Visualizza.document.getElementById("txtDataAttivazione").value=document.getElementById('txtDataAttiv').value;
					parent.opener.parent.Visualizza.document.getElementById("TxtDataAttivazioneRibaltata").value=document.getElementById('txtDataAttiv').value;
					window.close();
				}
			}
			else
			{
			    parent.opener.parent.Visualizza.document.getElementById("txtDataAttivazione").value=document.getElementById('txtDataAttiv').value;
				parent.opener.parent.Visualizza.document.getElementById("TxtDataAttivazioneRibaltata").value=document.getElementById('txtDataAttiv').value;
				window.close();
			}
		}
  
		function VerificaData(Data)
		{
			if (!IsBlank(Data.value ))
			{		
				if(!isDate(Data.value)) 
				{
				    alert("Inserire la Data  correttamente in formato: gg/mm/aaaa !");
				    Data.value = "";
					Setfocus(Data);
					return false;
				}
			}					
		}
		
		function AssegnaFuoco()
		{
		    document.getElementById('txtDataAttiv').focus();
		}
		</script>
</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<div class="SfondoGenerale" align="right">
				<input class="Bottone BottoneSalva" id="BottoneSalva" title="Associa la data inserita" onclick="Sostituisci();" type="button">
				<input type="button" class="Bottone BottoneAnnulla" title="Chiudi la finestra corrente" onclick="window.close();">
			 </div>
			<br>
			<br>
			<table cellSpacing="0" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td class="Input_Label_Bold">
						<asp:label ID="lblTesto" Runat="server">Inserisci la data di attivazione del contatore, quindi effettua il salvataggio</asp:label>
					</td>
				</tr>
				<tr>
					<td>
					</td>
				</tr>
				<tr>
					<td class="Input_Label_Bold">
						<asp:textbox id="txtDataAttiv" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" Cssclass="Input_Text" MaxLength="10" TextMode="SingleLine"></asp:textbox>
					</td>
				</tr>
			</table>
			<asp:TextBox ID="TxtDataSottoscrizione" Runat="server" style="DISPLAY:none"></asp:TextBox>
		</form>
	</body>
</HTML>
