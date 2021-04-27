<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrameAttivazione.aspx.vb" Inherits="OpenUtenze.FrameAttivazione"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Attiva il contatore per questo contratto</title>
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
			function  Attiva(){
			//alert("Ok, passa dalla funzione");
			    parent.opener.document.getElementById("txtDataAttivazione").value=document.getElementById('txtDataAtt').value;
			    if (document.getElementById('txtDataAtt').value != "")
					{
					parent.opener.document.getElementById("btnAttivaContatore").style.display='none';
					//alert("DISABILITATO IL Bottone PER L'ATTIVAZIONE NELLA SCHERMATA PRECEDENTE");
					parent.opener.document.getElementById("lblattcont").innerText="Al salvataggio, verra\' attivato il \n contatore per questo contratto";
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
			    document.getElementById('txtDataAtt').focus();
			}
		</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0"
		marginheight="0" marginwidth="0" onload="AssegnaFuoco()">
		<form id="Form1" runat="server" method="post">
			<div class="SfondoGenerale" align="right"><input class="Bottone BottoneSalva" title="Associa la data inserita" onclick="Attiva();" type="button"></div>
			<br>
			<br>
			<table cellSpacing="0" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td class="Input_Label_Bold">
						Inserisci la data di attivazione del contratto, quindi effettua il salvataggio
					</td>
				</tr>
				<tr>
				<td>
				
				</td>
				</tr>
				<tr>
					<td class="Input_Label_Bold">
						<asp:textbox id="txtDataAtt" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"
							runat="server" Cssclass="Input_Text" MaxLength="10" TextMode="SingleLine"></asp:textbox>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
