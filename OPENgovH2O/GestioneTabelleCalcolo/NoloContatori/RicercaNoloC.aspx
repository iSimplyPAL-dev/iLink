<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaNoloC.aspx.vb" Inherits="OpenUtenze.RicercaNoloC"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaNoloC</title>
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
			    document.getElementById('loadGrid').src = 'ResultRicercaNoloC.aspx?IdCanone=' + document.getElementById('ddlTipoContatore').value + '&Anno=' + escape(document.getElementById('txtAnno').value)
				return true;
			}
			
			function NewInsert()
			{			
				parent.Comandi.location.href='./CConfiguraNoloC.aspx?Inserimento=Inserimento'
				loadInsert.src="./ConfiguraNoloC.aspx";
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
						<FIELDSET class="FiledSetRicerca">
							<LEGEND class="Legend">Inserimento filtri di ricerca</LEGEND>
							<table width="100%">
								<tr>
									<td style="WIDTH: 15%">
										<asp:Label Runat="server" CssClass="Input_Label" id="Label3">Anno</asp:Label><br>
										<asp:textbox id="txtAnno" onchange="ControllaAnno(this)" runat="server" Width="80px" CssClass="Input_Number_Generali"
											MaxLength="4" onblur="" onfocus=""></asp:textbox>
									</td>
									<td style="WIDTH: 85%"><asp:label id="Label1" runat="server" CssClass="Input_Label">Tipologia Contatore</asp:label><br>
										<asp:dropdownlist id="ddlTipoContatore" runat="server" Width="280px" CssClass="Input_Text"></asp:dropdownlist></td>
								</tr>
							</table>
						</FIELDSET>
					</td>
				</tr>
				<tr>
					<td style="HEIGHT: 250px">
						<iframe id="loadGrid" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="250px"></iframe>
					</td>
				</tr>
				<tr>
					<td>
						<iframe id="loadInsert" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="130px"></iframe>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
