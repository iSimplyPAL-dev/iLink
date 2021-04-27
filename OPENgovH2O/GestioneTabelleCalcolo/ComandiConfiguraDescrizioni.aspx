<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiConfiguraDescrizioni.aspx.vb" Inherits="OpenUtenze.ComandiConfiguraDescrizioni"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>ComandiConfiguraDescrizioni</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%        End If%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function VisualizzaLabel(){
				parent.Comandi.lblTitolo.Text="<%=Session("DESCRIZIONE_ENTE")%>"
				//parent.Comandi.infoC.innerText="Gestione Categorie"
			}
			
			function Conferma()
			{
				if (confirm('Si vogliono salvare i dati?'))
				{
					parent.Visualizza.ControllaDati();
				}
			}
			function ControllaDati() {
			    var myIFrame = parent.Visualizza.document.getElementById('loadInsert');
			    var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
			    myContent.ControllaDati();
			}
			function ConfermaCancellazione() {
			    var myIFrame = parent.Visualizza.document.getElementById('loadInsert');
			    var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
			    myContent.ConfermaCancellazione();
			}
			function ConfermaUscita() {
			    var myIFrame = parent.Visualizza.document.getElementById('loadInsert');
			    var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
			    myContent.ConfermaUscita();
			}
		</script>
	</HEAD>
	<body class="SfondoGenerale" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="2"
		topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				<tr>
					<td><asp:label id="Comune" CssClass="ContentHead_Title" Width="504px" runat="server"></asp:label></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<input class="Bottone Bottonecancella" id="Delete" title="Elimina Descrizione" onclick="ConfermaCancellazione()"
							type="button" name="Erase"> <input class="Bottone BottoneSalva" id="Insert" title="Salva Descrizione" onclick="ControllaDati();"
							type="button" name="Insert"><input class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla Gestione" onclick="ConfermaUscita()"
							type="button" name="Delete">
					</td>
				</tr>
				<tr>
					<td align="left">
						<asp:label id="info" ForeColor="White" CssClass="NormalBold_title" Width="504px" runat="server"></asp:label>
					</td>
				</tr>
			</table>
			&nbsp;
		</form>
	</body>
</HTML>
