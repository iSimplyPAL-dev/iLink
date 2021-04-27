<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConfiguraNoloC.aspx.vb" Inherits="OpenUtenze.ConfiguraNoloC"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>ConfiguraNoloC</title>
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
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">
			function ControllaDati ()
				{	
			    if (document.getElementById('ddlTipoContatore').value=='') 
					{ 
						alert("E' necessario valorizzare il campo Capitolo!");
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
						/*strValore = strValore.replace(",",".");*/
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
						/*strValore = strValore.replace(",",".");*/
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
								alert("Inserire al massimo 3 decimali dopo la virgola nel campo Aliquota!");
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
					/*parent.Visualizza.Search();*/
				}	

			function ConfermaUscita()
			{
				if (confirm('Si vuole uscire dalla videata di Inserimento?'))
				{
					parent.parent.Comandi.location.href="./CRicercaNoloC.aspx";
					/*location.href='./RicercaTariffe.aspx?EffettuaRicerca=si'*/
					parent.frames.item('loadInsert').location.href="../../../aspVuota.aspx";
					/*parent.parent('visualizza').location.href='./RicercaTariffe.aspx?EffettuaRicerca=si'*/
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
				/*parent.Comandi.location.href="./CRicercaTariffe.aspx";
				location.href='./RicercaTariffe.aspx?EffettuaRicerca=si'*/
				parent.parent.Comandi.location.href="./CRicercaNoloC.aspx";
				parent.frames.item('loadInsert').location.href="../../../aspVuota.aspx";
			}
				
			function ConfermaCancellazione()
			{
				if (confirm('Si vuole eliminare il Nolo Contatore Selezionato?'))
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
	<body class="Sfondo" MS_POSITIONING="GridLayout" topmargin="0" leftmargin="0">
		<form id="Form1" runat="server" method="post">
			<table style="Z-INDEX: 101; POSITION: absolute; TOP: 8px; LEFT: 0px" cellPadding="0" width="100%" border="0">
				<tr>
					<td colSpan="6">	
						<asp:label id="lblDescrizioneOperazione" runat="server" Width="100%" CssClass="Legend">Dati Nolo Contatore - </asp:label>&nbsp;
					</td>
				</tr>
				<tr>
					<td>
						<asp:label id="Label3" CssClass="Input_Label" Runat="server">Anno</asp:label>
					</td>
					<td>
						<asp:textbox id="txtAnno" onblur="" onfocus="" runat="server" Width="80px" CssClass="Input_Number_Generali" MaxLength="4" onchange="ControllaAnno(this)"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label1" CssClass="Input_Label" Runat="server">Tipologia Contatore</asp:label>
					</td>
					<td colspan="3">
						<asp:dropdownlist id="ddlTipoContatore" runat="server" Width="280px" CssClass="Input_Text"></asp:dropdownlist>
					</td>
				</tr>
				<tr>
					<td>
						<asp:label id="Label2" CssClass="Input_Label" Runat="server">Importo </asp:label>
					</td>
					<td>
						<asp:textbox id="txtTariffa" runat="server" Width="80px" CssClass="Input_Number_Generali" MaxLength="255"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label4" CssClass="Input_Label" Runat="server">% IVA</asp:label>
					</td>
					<td>
						<asp:textbox id="txtIva" runat="server" Width="80px" CssClass="Input_Number_Generali" MaxLength="255"></asp:textbox>
					</td>
					<td>
						<asp:Label CssClass="Input_Label" Runat="server" id="Label5">Applica UNA TANTUM</asp:Label>&nbsp;&nbsp;
					</td>
					<td>
						<asp:CheckBox ID="ChkIsUnaTantum" CssClass="Input_Label" Runat="server"></asp:CheckBox>
					</td>
				</tr>
			</table>
			<asp:button id="BtnSalva" style="DISPLAY: none" runat="server" Width="32px" Text="Salva" Height="32px"></asp:button>
			<asp:button id="BtnElimina" style="DISPLAY: none" runat="server" Width="32px" Text="Elimina" Height="32px"></asp:button>
		</form>
	</body>
</HTML>
