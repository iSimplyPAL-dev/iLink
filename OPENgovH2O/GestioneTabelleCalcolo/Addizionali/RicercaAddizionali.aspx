<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaAddizionali.aspx.vb" Inherits="OpenUtenze.RicercaAddizionali"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaAddizionali</title>
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
			    document.getElementById('loadGrid').src='ResultRicercaAddizionali.aspx?IdAddizionale='+document.getElementById('ddlAddizionali98.40.value + '&Anno=' + escape(document.getElementById('txtAnno98.40.value)
				/*document.getElementById('loadGrid').src="./ResultRicercaCategoria.aspx"*/
				return true;
			}
			
			function NewInsert()
			{			
				/*parent.Comandi.location.href="./CConfiguraAddizionali.aspx";*/
				parent.Comandi.location.href='./CConfiguraAddizionali.aspx?Inserimento=Inserimento'
				loadInsert.src="./ConfiguraAddizionali.aspx";
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
									<td width="15%"><asp:label id="Label3" CssClass="Input_Label" Runat="server">Anno</asp:label><br>
										<asp:textbox id="txtAnno" onblur="" onfocus="" runat="server" CssClass="Input_Number_Generali"
											Width="80px" MaxLength="4" onchange="ControllaAnno(this)"></asp:textbox></td>
									<td width="85%" class="Input_Label"><asp:label id="Label1" runat="server" CssClass="Input_Label">Descrizione </asp:label><br>
										<asp:dropdownlist id="ddlAddizionali" runat="server" CssClass="Input_Text" Width="280px"></asp:dropdownlist></td>
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
