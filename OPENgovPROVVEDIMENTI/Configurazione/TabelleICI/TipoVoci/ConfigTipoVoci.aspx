<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConfigTipoVoci.aspx.vb" Inherits="Provvedimenti.ConfigTipoVoci" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ConfigTipoVoci</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function Search(){
				//document.getElementById("btnRicerca").click ()
				COD_TRIBUTO=document.getElementById("ddlTributo").value
				COD_CAPITOLO=document.getElementById("ddlCapitolo").value
				COD_PROVVEDIMENTI=document.getElementById("ddlProvvedimenti").value
				COD_VOCE=document.getElementById("ddlVoce").value
				COD_MISURA=document.getElementById("ddlMisura").value
				COD_FASE=document.getElementById("ddlFase").value
				//COD_CALCOLATA=document.getElementById("ddlCalcolata").value
				Parametri="COD_TRIBUTO="+COD_TRIBUTO+"&COD_CAPITOLO="+COD_CAPITOLO
				Parametri=Parametri + "&COD_PROVVEDIMENTI="+COD_PROVVEDIMENTI+"&COD_VOCE="+COD_VOCE
				Parametri=Parametri + "&COD_MISURA="+COD_MISURA+"&COD_FASE="+COD_FASE
				//Parametri=Parametri + "&COD_MISURA="+COD_MISURA+"&COD_CALCOLATA="+COD_CALCOLATA
				document.getElementById("ifrmRicerca").src="RicercaConfigTipoVoci.aspx?"+Parametri
				
			}
			function Inserisci(){
				document.location.href ="NuovoInserimentoTipoVoci.aspx?CODTRIBUTO=-1&CODCAPITOLO=-1&CODTIPOPROVVEDIMENTO=-1&CODMISURA=-1&CODCALCOLATO=-1&CODVOCE=-1&COD_FASE=-1&Nuovo=I"
			}
			function Clear(){
				document.getElementById ("btnPulisci").click()
				
			}
			function Abilita(valore){
				if (valore=="P"){
					document.getElementById ("ddlCalcolata").disabled=false 
				}else{
					document.getElementById ("ddlCalcolata").options(0).selected=true
					document.getElementById ("ddlCalcolata").disabled=true
				}
			
			}
		</script>
	</HEAD>
	<body class="SfondoVisualizza">
		<form id="Form1" runat="server" method="post">
			<fieldset class="classeFiledSet100"><legend class="Legend">Parametri di Ricerca</legend>
				<table cellSpacing="0" cellPadding="2" width="100%" border="0">
					<tr class="Input_Label">
						<td width="6%" style="HEIGHT: 50px">Tributo</td>
						<td width="27%" style="HEIGHT: 50px"><asp:dropdownlist id="ddlTributo" runat="server" cssclass="Input_Text" Width="400px" AutoPostBack="True"></asp:dropdownlist></td>
						<td width="7%" style="HEIGHT: 50px">Capitolo</td>
						<td width="27%" style="HEIGHT: 51px"><asp:dropdownlist id="ddlCapitolo" runat="server" cssclass="Input_Text" Width="180px"></asp:dropdownlist></td>
						<td width="12%" style="HEIGHT: 51px">Provvedimenti</td>
						<td width="22%" style="HEIGHT: 51px"><asp:dropdownlist id="ddlProvvedimenti" runat="server" cssclass="Input_Text" Width="300px"></asp:dropdownlist></td>
					</tr>
					<tr class="Input_Label">
						<td>Voce</td>
						<td><asp:dropdownlist id="ddlVoce" runat="server" cssclass="Input_Text" Width="300px"></asp:dropdownlist></td>
						<td>Misura</td>
						<td><asp:dropdownlist id="ddlMisura" runat="server" cssclass="Input_Text" Width="180px"></asp:dropdownlist></td>
						<td>Fase</td>
						<td>
							<asp:dropdownlist id="ddlFase" runat="server" cssclass="Input_Text" Width="180px"></asp:dropdownlist>
							<!--<asp:dropdownlist id="ddlCalcolata" runat="server" cssclass="Input_Text" Width="180px"></asp:dropdownlist>-->
						</td>
					</tr>
				</table>
				<br>
			</fieldset>
			
			<asp:button id="btnRicerca" style="DISPLAY: none" runat="server" Width="4" Height="12" Text="Cerca"></asp:button>
			<asp:button id="btnNuovo" style="DISPLAY: none" runat="server" Width="4" Height="12" Text="Nuovo"></asp:button>
			<asp:button id="btnPulisci" style="DISPLAY: none" runat="server" Width="1" Height="12" Text="Pulisci"></asp:button>
			
			<iframe class="SfondoVisualizza" id="ifrmRicerca" name="ifrmRicerca" src="../../../../aspVuota.aspx"frameBorder="0" width="100%" height="450"></iframe>
		</form>
	</body>
</HTML>
