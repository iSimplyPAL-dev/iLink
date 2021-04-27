<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaRuoliPerConsultaDocElaborati.aspx.vb" Inherits="OpenUtenze.RicercaRuoliPerConsultaDocElaborati"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaRuoliPerConsultaDocElaborati</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function Search()
			{	
				document.getElementById('loadGrid').src = 'RisultatiRuoliConsultaDocElaborati.aspx?DataFattura='+document.RicercaRuoliPerConsultaDocElaborati.cmbDataFattura.value 
			}		
		</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="3" rightMargin="3" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="TblRicerca" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<tr>
					<td>
						<fieldset class="FiledSetRicerca"><LEGEND class="Legend">Inserimento filtri di ricerca</LEGEND>
							<table id="TblParametri" cellSpacing="1" cellPadding="1" width="100%" border="0">
								<tr>
									<td width="150"><asp:label id="Label7" CssClass="Input_Label" Runat="server" Width="168px">Data Fattura/Nota di Credito</asp:label><BR>
										<asp:dropdownlist id="cmbDataFattura" runat="server" CssClass="Input_Text" AutoPostBack="True" Width="120px"></asp:dropdownlist></td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td width="100%" colSpan="5"><iframe id="loadGrid" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="450"></iframe>
					</td>
				</tr>
				<tr>
					<td colSpan="5">
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
