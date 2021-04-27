<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaDescrizioni.aspx.vb" Inherits="OpenUtenze.RicercaDescrizioni"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaDescrizioni</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">						
			function Search(Tabella)
			{			
				/*inserttype="TP_ADDIZIONALI";					*/
				/*alert(document.Form1.txtTabella.value)*/
			    document.getElementById('loadGrid').src = 'ResultRicercaDescrizioni.aspx?Codice=' + document.getElementById('ddlCodice').value + '&Tabella=' + document.getElementById('txtTabella').value
				
				/*document.getElementById('loadGrid').src="./ResultRicercaCategoria.aspx"*/
				return true;
			}
			
			function NewInsert()
			{			
				
				parent.Comandi.location.href='./ComandiConfiguraDescrizioni.aspx?Inserimento=Inserimento'
				loadInsert.src='./ConfiguraDescrizioni.aspx?Tabella=' + document.getElementById('txtTabella').value
				return true;
			}
		</script>
	</HEAD>
	<body class="Sfondo" leftMargin="3" rightMargin="3">
		<form id="Form1" runat="server" method="post">
			<table id="tabEsterna" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<tr>
					<td style="HEIGHT: 75px">
						<FIELDSET class="FiledSetRicerca" style="WIDTH: 98%; HEIGHT: 79px"><LEGEND class="Legend">Inserimento 
								filtri di ricerca</LEGEND>
							<table width="100%">
								<tr>
									<td style="WIDTH: 100%; HEIGHT: 3px"></td>
								</tr>
								<tr>
									<td class="Input_Label"><asp:label id="Label1" runat="server" CssClass="Input_Label">Descrizione </asp:label><br>
										<asp:dropdownlist id="ddlCodice" runat="server" CssClass="Input_Text" Width="336px"></asp:dropdownlist><asp:textbox id="txtTabella" runat="server"></asp:textbox></td>
								</tr>
							</table>
						</FIELDSET>
					</td>
				</tr>
				<tr>
					<td style="HEIGHT: 250px"><iframe id="loadGrid" style="WIDTH: 100%; HEIGHT: 250px" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="250"></iframe>
					</td>
				</tr>
				<tr>
					<td><iframe id="loadInsert" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="180"></iframe>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
