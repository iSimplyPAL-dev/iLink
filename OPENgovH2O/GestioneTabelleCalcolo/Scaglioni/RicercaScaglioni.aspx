<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaScaglioni.aspx.vb" Inherits="OpenUtenze.RicercaScaglioni"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaScaglioni</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
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
			    document.getElementById('loadGrid').src = 'ResultRicercaScaglioni.aspx?IdUtenza=' + document.getElementById('ddlTipoUtenza').value + '&Anno=' + escape(document.getElementById('txtAnno').value)
				return true;
			}
			
			function NewInsert()
			{			
				parent.Comandi.location.href='./CConfiguraScaglioni.aspx?Inserimento=Inserimento'
				loadInsert.src="./ConfiguraScaglioni.aspx";
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
						<FIELDSET class="FiledSetRicerca" style="WIDTH: 98%; HEIGHT: 79px"><LEGEND class="Legend">Inserimento 
								filtri di ricerca</LEGEND>
							<table width="100%">
								<tr>
									<td style="WIDTH: 15%">
										<asp:Label Runat="server" CssClass="Input_Label" id="Label3">Anno</asp:Label><br>
										<asp:textbox id="txtAnno" runat="server" Width="80px" CssClass="Input_Number_Generali"
											MaxLength="4" onblur="" onfocus="" AutoPostBack=True></asp:textbox>
									</td>
									<td style="WIDTH: 85%"><asp:label id="Label1" runat="server" CssClass="Input_Label">Tipologia Utenza</asp:label><br>
										<asp:dropdownlist id="ddlTipoUtenza" runat="server" Width="280px" CssClass="Input_Text"></asp:dropdownlist></td>
								</tr>
							</table>
						</FIELDSET>
					</td>
				</tr>
				<tr>
					<td style="HEIGHT: 250px"><iframe id="loadGrid" style="WIDTH: 100%; HEIGHT: 250px" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="250"></iframe></td>
				</tr>
				<tr>
					<td><iframe id="loadInsert" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="180"></iframe></td>
				</tr>
			</table>
            <asp:button id="btnRibalta" style="DISPLAY: none" runat="server" onclick="btnRibalta_Click"></asp:button>
		</form>
	</body>
</HTML>
