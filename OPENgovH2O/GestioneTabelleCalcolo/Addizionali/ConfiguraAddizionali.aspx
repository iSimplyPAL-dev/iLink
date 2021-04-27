<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConfiguraAddizionali.aspx.vb" Inherits="OpenUtenze.ConfiguraAddizionali"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>ConfiguraAddizionali</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">
			function ControllaDati ()
				{	
			    if (document.getElementById('ddlAddizionali').value == '')
					{ 
						alert("E' necessario valorizzare il campo Descrizione!");
						return false;
					}

					
					if (document.getElementById('txtTariffa').value=='') 
					{ 
						alert("E' necessario valorizzare il campo Tariffa!");
						return false; 
					}		
					else
					{
						strValore = document.getElementById('txtTariffa').value;						
						if(!IsNumeric(strValore))
						{
							alert("Inserire un valore numerico nel campo Tariffa");
						Setfocus(document.getElementById('txtTariffa'));
							return false;
						}
					}
					
							if (document.getElementById('txtIva').value=='') 
					{ 
						alert("E' necessario valorizzare il campo Aliquota!");
						return false; 
					}		
					else
					{
					    strValore = document.getElementById('txtIva').value;						
						if(!IsNumeric(strValore))
						{
							alert("Inserire un valore numerico nel campo Aliquota");
						Setfocus(document.getElementById('txtIva'));
							return false;
						}
						else
						{
							sGG=document.getElementById('txtIva').value;
							if (!isNumber(sGG,0,3))
							{
								alert("Inserire al massimo 2 decimali dopo la virgola nel campo Aliquota!");
							Setfocus(document.getElementById('txtIva'));
								return false;
							}
							if (sGG>100)
							{
								alert("Inserire un valore minore o uguale a 100!");
								Setfocus(document.getElementById('txtIva'));
								return false;							
							}
						}
					}
	
							if (document.getElementById('txtAnno').value=='') 
					{ 
						alert("E' necessario valorizzare il campo Anno!");
					Setfocus(document.getElementById('txtAnno'));
						return false; 
					}		
					else 
					{
					    var txtanno = document.getElementById('txtAnno').value;
						if (txtanno.length!=4) 
						{ 
							alert("E' necessario inserire un Anno di 4 caratteri!");
							Setfocus(document.getElementById('txtAnno'));
							return false; 
						}								
					}		
					document.getElementById('BtnSalva').click()
				}	

			function ConfermaUscita()
			{
				if (confirm('Si vuole uscire dalla videata di Inserimento?'))
				{
					parent.parent.Comandi.location.href="./CRicercaAddizionali.aspx";					
					parent.frames.item('loadInsert').location.href="../../../aspVuota.aspx";					
				}
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
			
			function UscitaDopoOperazione()
				{
					parent.parent.Comandi.location.href="./CRicercaAddizionali.aspx";
					parent.frames.item('loadInsert').location.href="../../../aspVuota.aspx";					
				}
				
			function ConfermaCancellazione()
			{
				if (confirm('Si vuole eliminare l\'Addizionale Selezionato?'))
				{
				    document.getElementById('BtnElimina').click()					
				}
			}									
			
			function IsNumeric(sText)
			{
				var ValidChars = "0123456789,";
				var IsNumber=true;
				var Char;

				for (i = 0; i < sText.length && IsNumber == true; i++) 
					{ 
					Char = sText.charAt(i); 
					if (ValidChars.indexOf(Char) == -1) 
						{
						IsNumber = false;
						}
					}
				return IsNumber;
				
			}		
		</script>
</HEAD>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<table style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 8px; HEIGHT: 137px" cellPadding="0" width="100%"
				border="0">
				<tr>
					<td style="HEIGHT: 16px" colSpan="2"><asp:label id="lblDescrizioneOperazione" CssClass="Legend" runat="server" Width="100%">Dati Addizionale - </asp:label>&nbsp;</td>
				</tr>
				<tr>
					<td style="WIDTH: 15%">
						<asp:Label Runat="server" CssClass="Input_Label" id="Label3">Anno</asp:Label>
						<br>
						<asp:textbox id="txtAnno" onchange="ControllaAnno(this)" runat="server" Width="80px" CssClass="Input_Number_Generali"
							MaxLength="4" onblur="" onfocus=""></asp:textbox>
					</td>
					<td style="WIDTH: 85%; HEIGHT: 30px">
						<asp:Label Runat="server" CssClass="Input_Label" id="Label1">Descrizione</asp:Label>
						<br>
						<asp:dropdownlist id="ddlAddizionali" runat="server" Width="280px" CssClass="Input_Text"></asp:dropdownlist>
					</td>
				</tr>
				<tr>
					<td style="HEIGHT: 30px" colspan="1">
						<asp:Label Runat="server" CssClass="Input_Label" id="Label2">Importo </asp:Label>
						<br>
						<asp:textbox id="txtTariffa" runat="server" Width="80px" CssClass="Input_Number_Generali" MaxLength="255"></asp:textbox>
					</td>
					<td style="HEIGHT: 30px" colspan="1">
						<asp:Label Runat="server" CssClass="Input_Label" id="Label4">% IVA</asp:Label>
						<br>
						<asp:textbox id="txtIva" runat="server" Width="80px" CssClass="Input_Number_Generali" MaxLength="255"></asp:textbox>
					</td>
				</tr>
			</table>
			<asp:button id="BtnSalva" style="DISPLAY: none" runat="server" Width="32px" Height="32px" Text="Salva"></asp:button>
			<asp:button id="BtnElimina" style="DISPLAY: none" runat="server" Width="32px" Text="Elimina"
				Height="32px"></asp:button>
		</form>
	</body>
</HTML>
