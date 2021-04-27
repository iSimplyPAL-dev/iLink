<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DatiContribuente.aspx.vb" Inherits="OPENgovTIA.DatiContribuente" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>DatiContribuente</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function ApriRicercaAnagrafe(nomeSessione){ 
				winWidth=980 
				winHeight=680 
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40 
				Parametri="sessionName=" + nomeSessione 
				WinPopUpRicercaAnagrafica=window.open("../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?"+Parametri,"","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
			}

			function ClearDatiContrib()
			{
				if (confirm('Si desidera eliminare il Contribuente?'))
				{
				    document.getElementById('LblNominativo').value = '';
					document.getElementById('LblNascita').value = '';
					document.getElementById('LblResidenza').value = '';
					document.getElementById('TxtCodContribuente').value = '-1';
					document.getElementById('TxtIdDataAnagrafica').value = '-1';
				}
				return false;
			}

			function ApriRicAnater(){
				// apro il popup di ricerca in anagrafe anater
				winWidth=980 
				winHeight=500 
				myleft=(screen.availWidth-winWidth)/2 
				mytop=(screen.availheight-winHeight)/2 - 40 
				var parametri = 'popup=1';
				
				WinPopAnater=window.open('../../AnagraficaAnater/popAnagAnater.aspx?'+parametri,'','width='+winWidth+',height='+winHeight+',top='+mytop+',left='+myleft+' status=yes, toolbar=no,scrollbar=no, resizable=no') 
			}
		</script>
	</head>
	<body MS_POSITIONING="GridLayout" topmargin="0" leftmargin="0">
		<form id="Form1" runat="server" method="post">
			<table id="TblDatiContribuente" border="1" cellSpacing="0" cellPadding="0" width="765px" height="100px">
				<tr>
					<td borderColor="darkblue">
						<table id="Table1" border="0" cellSpacing="1" cellPadding="1" width="100%">
							<tr valign="top">
								<td class="Input_Label" height="20"><strong>DATI CONTRIBUENTE</strong>
									<asp:label style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" id="Label32" Runat="server">*</asp:label>
									<asp:imagebutton id="LnkAnagTributi" runat="server" style="display:none" imagealign="Bottom" CausesValidation="False" ToolTip="Ricerca Anagrafica da Tributi" ImageUrl="../../images/Bottoni/Listasel.png"></asp:imagebutton>&nbsp;
									<asp:imagebutton id="LnkAnagAnater" runat="server" style="display:none" imagealign="Bottom" CausesValidation="False" ToolTip="Ricerca Anagrafica da Anater" ImageUrl="../../images/Bottoni/Listasel.png"></asp:imagebutton>&nbsp;
									<asp:imagebutton id="LnkPulisciContr" runat="server" style="display:none" imagealign="Bottom" CausesValidation="False" ToolTip="Pulisci i campi Contribuente" ImageUrl="../../images/Bottoni/cancel.png"></asp:imagebutton>
								</td>
							</tr>
							<tr>
								<td class="DettagliContribuente"><asp:label id="LblNominativo" runat="server" CssClass="DettagliContribuente"></asp:label></td>
							</tr>
							<tr>
								<td class="DettagliContribuente"><asp:label id="LblNascita" runat="server" CssClass="DettagliContribuente"></asp:label></td>
							</tr>
							<tr>
								<td class="DettagliContribuente"><asp:label id="LblResidenza" runat="server" CssClass="DettagliContribuente"></asp:label></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<asp:textbox id="TxtCodContribuente" Runat="server" style="DISPLAY: none" Width="10px">-1</asp:textbox>
			<asp:textbox id="TxtIdDataAnagrafica" Runat="server" Visible="False" Width="10px">-1</asp:textbox>
			<asp:button id="btnRibalta" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:button id="btnRibaltaAnagAnater" style="DISPLAY: none" Runat="server"></asp:button>
		</form>
	</body>
</HTML>
