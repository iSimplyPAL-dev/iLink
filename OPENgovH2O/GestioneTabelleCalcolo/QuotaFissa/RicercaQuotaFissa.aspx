<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaQuotaFissa.aspx.vb" Inherits="OpenUtenze.RicercaQuotaFissa"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaQuotaFissa</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function Search()
			{			
				//*** 20121212 riclassifica contatori ***
			    document.getElementById('loadGrid').src = 'ResultRicercaQuotaFissa.aspx?IdUtenza=' + document.getElementById('ddlTipoUtenza').value + '&Anno=' + escape(document.getElementById('txtAnno').value)
				//document.getElementById('loadGrid').src='ResultRicercaQuotaFissa.aspx?IdUtenza='+document.Form1.ddlTipoUtenza.value + '&TipoCanone='+document.Form1.ddlTipoCanone.value + '&Anno=' + escape(document.Form1.txtAnno.value)
				return true;
			}
			
			function NewInsert()
			{			
				parent.Comandi.location.href='./CConfiguraQuotaFissa.aspx?Inserimento=Inserimento'
				loadInsert.src="./ConfiguraQuotaFissa.aspx";
				return true;
			}
		
			function ControllaAnno(oggetto){
				if (!IsBlank(oggetto.value)){
					if (!isNumber(oggetto.value, 4, 0, 1950, 2090)){
						alert ("Inserire un anno di quattro cifre\ncompreso fra 1950 e 2090")
						oggetto.value=""
						oggetto.focus()
						return false
					}
				}
			}			
		</script>
	</HEAD>
	<body class="Sfondo" leftMargin="3" rightMargin="3">
		<form id="Form1" runat="server" method="post">
			<table id="tabEsterna" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<tr>
					<td>
						<fieldset class="FiledSetRicerca" style="WIDTH: 98%"><legend class="Legend">Inserimento 
								filtri di ricerca</legend>
							<table width="100%">
								<tr>
									<td style="WIDTH: 15%">
										<asp:Label Runat="server" CssClass="Input_Label" id="Label3">Anno</asp:Label><br>
										<asp:textbox id="txtAnno"  runat="server" Width="72px" CssClass="Input_Number_Generali"
											MaxLength="4" AutoPostBack="True"></asp:textbox>
									</td>
									<td><asp:label id="Label1" runat="server" CssClass="Input_Label">Tipologia Utenza</asp:label><br>
										<asp:dropdownlist id="ddlTipoUtenza" runat="server" Width="280px" CssClass="Input_Text"></asp:dropdownlist></td>
									<!--
									<td><asp:label id="Label2" runat="server" CssClass="Input_Label">Tipo Canone</asp:label><br>
										<asp:dropdownlist id="ddlTipoCanone" runat="server" Width="280px" CssClass="Input_Text"></asp:dropdownlist></td>
										-->
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td><iframe id="loadGrid" style="WIDTH: 100%; HEIGHT: 250px" src="../../../aspVuota.aspx" frameBorder="0"></iframe>
					</td>
				</tr>
				<tr>
					<td><iframe id="loadInsert" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="150px"></iframe>
					</td>
				</tr>
			</table>
            <asp:button id="btnRibalta" style="DISPLAY: none" runat="server" onclick="btnRibalta_Click"></asp:button>
		</form>
	</body>
</HTML>
