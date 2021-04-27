<%@ Page language="c#" Codebehind="StatoElaborazioneICImassivo.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.StatoElaborazioneICImassivo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>StatoElaborazioneICImassivo</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
  </HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0">
		<form id="Form1" runat="server" method="post">
			<div align=center id="ElaborazioneIncorso" style="DISPLAY:none; WIDTH:100%; HEIGHT:136px">
				<table cellSpacing="5" cellPadding="0" width="100%" border="0" class="dati_anagrafe_tarsu_blu">
					<tr>
						<td align="center"><p>Stato Elaborazione</p>
						</td>
					</tr>
					<tr>
						<!--*** 20120704 - IMU ***-->
						<td align="center"><p>Elaborazione&nbsp;Calcolo Massivo&nbsp;in Corso....</p>
						</td>
					</tr>
					<tr>
						<td class="riga_menu" align="center"><p><IMG alt="" src="../../images/loading.png"></p>
						</td>
					</tr>
					<tr>
						<td align="center">Attendere Prego...</td>
					</tr>
				</table>
			</div>
			<div id="ElaborazioneErrore" style="DISPLAY:none; WIDTH:100%; HEIGHT:136px">
				<table cellSpacing="5" cellPadding="0" width="100%" border="0" class="dati_anagrafe_tarsu_blu">
					<tr>
						<td align="center"><p>Stato Elaborazione</p>
						</td>
					</tr>
					<tr>
						<td align="center">&nbsp;
						</td>
					</tr>
					<tr>
						<!--*** 20120704 - IMU ***-->
						<td align="center"><p>Elaborazione&nbsp;Calcolo Massivo terminato con errori</p>
						</td>
					</tr>
				</table>
			</div>
			<div id="NessunaElaborazione" style="DISPLAY:none; WIDTH:100%; HEIGHT:136px">
				<table cellSpacing="5" cellPadding="0" width="100%" border="0" class="dati_anagrafe_tarsu_blu">
					<tr>
						<td align="center"><p>Stato Elaborazione</p>
						</td>
					</tr>
					<tr>
						<td align="center">&nbsp;
						</td>
					</tr>
					<tr>
						<!--*** 20120704 - IMU ***-->
						<td align="center"><p>Non ci sono elaborazioni di&nbsp;Calcolo Massivo in corso....</p>
						</td>
					</tr>
				</table>
			</div>
			<div id="ElaborazioneTerminata" style="DISPLAY:none; WIDTH:100%; HEIGHT:136px">
				<table cellSpacing="5" cellPadding="0" width="100%" border="0" class="dati_anagrafe_tarsu_blu">
					<tr>
						<td align="center"><p>Stato Elaborazione</p>
						</td>
					</tr>
					<tr>
						<td align="center">&nbsp;
						</td>
					</tr>
					<tr>
						<!--*** 20120704 - IMU ***-->
						<td align="center"><p>Elaborazione&nbsp;Calcolo Massivo terminata con successo!</p>
						</td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
