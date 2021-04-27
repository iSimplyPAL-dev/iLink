<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DatiAnagrafeResidenti.aspx.vb" Inherits="OPENgov.DatiAnagrafeResidenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>DatiAnagrafeResidenti</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoVisualizza" leftMargin="20" topMargin="5" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table style="Z-INDEX: 101; POSITION: absolute; WIDTH: 641px; HEIGHT: 134px; TOP: 16px; LEFT: 16px"
				cellSpacing="0" cellPadding="1" width="641" align="center" border="0">
				<tr>
					<td class="lstTabRow" colSpan="3">Dati Acquisiti     -     Azione
						<asp:label id="LblAzione" runat="server" CssClass="Legend"></asp:label>
					</td>
				</tr>
				<tr>
					<td class="Input_Label" colspan="2">Cognome
					</td>
					<td class="Input_Label">Nome</td>
				</tr>
				<tr>
					<td colspan="2"><asp:textbox id="txtCognome" tabIndex="3" runat="server" MaxLength="100" Width="400" CssClass="Input_Text"></asp:textbox></td>
					<td><asp:textbox id="txtNome" tabIndex="4" runat="server" MaxLength="50" Width="200" CssClass="Input_Text"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label">Codice Fiscale
					</td>
					<td class="Input_Label">Sesso</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtCodiceFiscale" tabIndex="1" runat="server" cssClass="Input_Text" MaxLength="16"
							ToolTip="Codice Fiscale" Width="200px"></asp:textbox>
					<td><asp:textbox id="txtSesso" tabIndex="8" runat="server" MaxLength="10" Width="50px" CssClass="Input_Text"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label">Luogo di Nascita&nbsp;
					</td>
					<td class="Input_Label">Data di Nascita&nbsp;
					</td>
					<td class="Input_Label">Data di Morte&nbsp;
					</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtLuogoNascita" tabIndex="6" runat="server" MaxLength="50" Width="300px" CssClass="Input_Text"></asp:textbox></td>
					<td><asp:textbox id="txtDataNascita" tabIndex="7" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox></td>
					<td><asp:textbox id="txtDataMorte" tabIndex="7" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);""></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label" colspan="2">Indirizzo&nbsp;
					</td>
					<td class="Input_Label">Codice Via&nbsp;
					</td>
				</tr>
				<TR>
					<td colspan="2"><asp:textbox id="txtIndirizzo" tabIndex="7" runat="server" MaxLength="2" Width="400px" CssClass="Input_Text"></asp:textbox></td>
					<td><asp:textbox id="txtcodvia" tabIndex="7" runat="server" MaxLength="2" Width="96px" CssClass="Input_Text"></asp:textbox></td>
				<tr>
					<td class="Input_Label">Numero Famiglia&nbsp;
					</td>
					<td class="Input_Label">Posizione Famiglia&nbsp;
					</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtNfamiglia" tabIndex="6" runat="server" MaxLength="50" Width="150px" CssClass="Input_Text"></asp:textbox></td>
					<td colspan="2"><asp:textbox id="txtPosizione" tabIndex="7" runat="server" MaxLength="2" Width="300px" CssClass="Input_Text"></asp:textbox></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
