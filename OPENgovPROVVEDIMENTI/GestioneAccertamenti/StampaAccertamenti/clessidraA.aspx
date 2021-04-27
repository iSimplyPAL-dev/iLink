<%@ Page Language="vb" AutoEventWireup="false" Codebehind="clessidraA.aspx.vb" Inherits="Provvedimenti.clessidraA" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ristampa</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<BODY class="SfondoVisualizza" style="height:80px;">
		<div style="POSITION:absolute;LEFT:150px;" id="div_attesa">
			<table width="400" height="80" cellpadding="0" cellspacing="0"  >
				<tr class="wait">
					<td>
						<img src=../../../images/Clessidra.gif WIDTH="32" HEIGHT="32">
					</td>
					<td>
						Attendere, elaborazione dati in corso...
					</td>
				</tr>
			</table>
		</div>
		<div style="LEFT:150px;POSITION:absolute;display:none" id="div_annullo">
			<table width="400" height="80" cellpadding="0" cellspacing="0"  >
				<tr class="wait">
					<td align="center">
						Elaborazione Annullata.
					</td>
				</tr>
			</table>
		</div>
		<div style="LEFT:150px;POSITION:absolute;display:none" id="div_errore">
			<table width="400" height="80" cellpadding="0" cellspacing="0"  >
				<tr class="wait">
					<td align="center">
						Si è verificato un errore.
					</td>
				</tr>
			</table>
		</div>
	</BODY>
</HTML>
