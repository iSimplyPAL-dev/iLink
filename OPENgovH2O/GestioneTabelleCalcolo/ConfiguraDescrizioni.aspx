<%@ Page Language="vb" AutoEventWireup="false" Codebehind="getElementById('aspx.vb" Inherits="OpenUtenze.ConfiguraDescrizioni"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>ConfiguraDescrizioni</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">		
		function ControllaDati ()
		{	
		    if (document.getElementById('txtDescrizione').value=='') 
			{ 
				alert("E' necessario valorizzare il campo Descrizione!")
				return false; 
			}		

			var str =new String
			str = document.getElementById('txtDescrizione').value
			str = str.length 
 			if (str > 255) 
			{ 
				alert("Il campo Descrizione deve essere al massimo di 255 caratteri! Lunghezza attuale:" + str)
				return false; 
			}		
		
 			document.getElementById('BtnSalva').click()
		}	
		
		function ConfermaCancellazione()
		{
			if (confirm('Si vuole eliminare la configurazione selezionata?'))
			{
			    document.getElementById('BtnElimina').click()					
			}
		}		
		
		function ConfermaUscita()
			{
				
				if (confirm('Si vuole uscire dalla videata di Inserimento?'))
				{
					parent.parent.Comandi.location.href="./ComandiRicercaDescrizioni.aspx";
					inserttype=document.getElementById('txtTabella').value
					parent.parent('visualizza').location.href='./RicercaDescrizioni.aspx?EffettuaRicerca=si' + '&Tabella=' + inserttype
				}
			}
			
		function UscitaDopoOperazione()
			{
		    inserttype = document.getElementById('txtTabella').value;
				/*inserttype="TP_ADDIZIONALI";*/
				parent.parent.Comandi.location.href="./ComandiRicercaDescrizioni.aspx";
				parent.parent('visualizza').location.href='./RicercaDescrizioni.aspx?EffettuaRicerca=si' + '&Tabella=' + inserttype
			}

		function InsertCategoria()
		{	
		
		    document.getElementById('loadGrid').src = 'ResultRicercaDescrizioni.aspx?Codice=' + document.getElementById('ddlCodice').value
			return true;
		}
		</script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>		
</HEAD>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<asp:button id="BtnElimina" style="DISPLAY: none" runat="server" Width="32px" Text="Elimina"
				Height="32px"></asp:button>
			<asp:button id="BtnSalva" style="DISPLAY: none" runat="server" Text="Salva" Height="32px" Width="32px"></asp:button>
			<table style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px; HEIGHT: 137px" cellPadding="0" width="100%"
				border="0">
				<tr>
					<td style="HEIGHT: 16px" colSpan="2"><asp:label id="lblDescrizioneOperazione" CssClass="Legend" runat="server" Width="100%">Dati Addizionale - </asp:label>&nbsp;</td>
				</tr>
				<tr>
					<td>
					    <asp:label id="Label2" CssClass="Input_Label" Runat="server">Descrizione</asp:label><br />
						<asp:textbox id="txtDescrizione" runat="server" Height="55px" Width="488px" CssClass="Input_Text" MaxLength="255" TextMode="MultiLine"></asp:textbox>
						<br>
						<asp:textbox id="txtCodice" runat="server" Visible="False"></asp:textbox>
						<asp:textbox id="txtTabella" runat="server"></asp:textbox>
					</td>
					<td valign="top">
					    <div id="divApplicaA" runat="server" class="FiledSetRicerca">
					        <asp:Label runat="server" CssClass="Legend" Text="Applica a"></asp:Label><br /><br />
						    <asp:RadioButton runat="server" ID="optH2O" GroupName="ApplicaA" CssClass="Input_Label" Text="Acqua" Checked="true" />&nbsp;
						    <asp:RadioButton runat="server" ID="optDep" GroupName="ApplicaA" CssClass="Input_Label" Text="Depurazione" Checked="false" />&nbsp;
						    <asp:RadioButton runat="server" ID="optFog" GroupName="ApplicaA" CssClass="Input_Label" Text="Fognatura" Checked="false" />
					    </div>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
