<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrameCessazione.aspx.vb" Inherits="OpenUtenze.FrameCessazione"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Cessazione e voltura del contratto</title>
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
			function Cessa()
			{  
				//la data di voltura deve essere posteriore/uguale all'ultima presente
				if (document.getElementById('TxtDataLastLettura').value!='' && document.getElementById('txtDataCess').value!='') 
				{
					var starttime = document.getElementById('TxtDataLastLettura').value
					var endtime = document.getElementById('txtDataCess').value
					//Start date split to UK date format and add 31 days for maximum datediff
					starttime = starttime.split("/"); 
					starttime = new Date(starttime[2],starttime[1]-1,starttime[0]); 
					//End date split to UK date format 
					endtime = endtime.split("/");
					endtime = new Date(endtime[2],endtime[1]-1,endtime[0]); 
					if (endtime<starttime)
					{
						alert("La Data di Voltura e\' minore dell'ultima lettura presente.\nImpossibile Proseguire!");
						return false;
					}
				}
				parent.opener.parent.Visualizza.document.getElementById("txtDataCessazione").value=document.getElementById('txtDataCess').value;
				parent.opener.parent.Visualizza.document.getElementById("TxtDataCessazioneRibaltata").value=document.getElementById('txtDataCess').value;
				if(document.getElementById('txtDataCess').value!=""){
				} 
				window.close();
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
				document.getElementById('txtDataCess').focus();
			}
		</script>
	</HEAD>
	<body onload="AssegnaFuoco();" class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0"
		rightMargin="0" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<div class="SfondoGenerale" align="right">
				<input class="Bottone BottoneSalva" title="Associa la data inserita" onclick="Cessa();" type="button">
				<input type="button" class="Bottone BottoneAnnulla" title="Chiudi la finestra corrente" onclick="window.close();">
			</div>
			<br>
			<br>
			<table cellSpacing="0" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td class="Input_Label_Bold">
						<P>Inserisci la data di cessazione del contratto, quindi effettua il salvataggio.
							<br>
							Se risulta vuota, non verra' effettuata la voltura del contratto.</P>
					</td>
				</tr>
				<tr>
					<td>
					</td>
				</tr>
				<tr>
					<td class="Input_Label_Bold">
						<asp:textbox id="txtDataCess" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"
							runat="server" Cssclass="Input_Text" MaxLength="10" TextMode="SingleLine"></asp:textbox>
					</td>
				</tr>
			</table>
			<asp:TextBox ID="TxtDataLastLettura" Runat="server" style="DISPLAY:none"></asp:TextBox>
		</form>
	</body>
</HTML>
