<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConfiguraQuotaFissa.aspx.vb" Inherits="OpenUtenze.ConfiguraQuotaFissa"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ConfiguraQuotaFissa</title>
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
			    if (document.getElementById('ddlTipoUtenza').value=='') 
					{ 
						alert("E' necessario valorizzare il campo Capitolo!");
						return false;
					}
								
			    if (isNaN(parseFloat(document.getElementById('txtTariffaH2O').value)))
					{
						alert("Inserire solo NUMERI nel campo Tariffa H2O!");
						Setfocus(document.getElementById('txtTariffaH2O'));
						return false;
					}
										
			    if (isNaN(parseFloat(document.getElementById('txtTariffaFog').value)))
					{
						alert("Inserire solo NUMERI nel campo Tariffa Fognature!");
						Setfocus(document.getElementById('txtTariffaFog'));
						return false;
					}
									
			    if (isNaN(parseFloat(document.getElementById('txtTariffaDep').value)))
					{
						alert("Inserire solo NUMERI nel campo Tariffa Depurazione!");
						Setfocus(document.getElementById('txtTariffaDep'));
						return false;
					}
					
			    if ((parseInt(document.getElementById('txtTariffaH2O').value) <= 0) && (parseInt(document.getElementById('txtTariffaFog').value) <= 0) && (parseInt(document.getElementById('txtTariffaDep').value) <= 0))
					{
						alert("Valorizzare almeno un campo tariffa con un valore maggiore di 0");
						return false;
					}
					
			    if (document.getElementById('txtDa').value=='') 
					{ 
						alert("E' necessario valorizzare il campo Da!");
						return false; 
					}		
					else
					{
			        sValore = document.getElementById('txtDa').value;						
						if (!isNumber(sValore,0,0))
						{
							alert("Inserire solo NUMERI INTERI nel campo DA!");
							Setfocus(document.getElementById('txtDa'));
							return false;
						}
						else
						{
							var sCheck =new String
							sCheck = document.getElementById('txtDa').value
							sCheck = sCheck.length 
 							if (sCheck > 9) 
							{
								alert("Il campo DA puo\' valere al massimo 999999999!");
								Setfocus(document.getElementById('txtDa'));
								return false;
							}
						}
						/****permetto l'inserimento di 0***
						if(sValore=='0')
						{
							alert("Inserire un valore numerico maggiore di 0 nel campo DA!");
							Setfocus(document.Form1.txtDa);
							return false;
						}*/						
					}

			    if (document.getElementById('txtA').value=='') 
					{ 
						alert("E' necessario valorizzare il campo A!");
						return false; 
					}		
					else
					{
			        sValore = document.getElementById('txtA').value;
						if (!isNumber(sValore,0,0))
						{
							alert("Inserire solo NUMERI INTERI nel campo A!");
							Setfocus(document.getElementById('txtA'));
							return false;
						}
						else
						{
							var sCheck =new String
							sCheck = document.getElementById('txtA').value
							sCheck = sCheck.length 
 							if (sCheck > 9) 
							{
								alert("Il campo A puo\' valere al massimo 999999999!");
								Setfocus(document.getElementById('txtA'));
								return false;
							}
						}
						if(parseInt(sValore)<=parseInt(document.getElementById('txtDa').value))
						{
							alert("Inserire un valore numerico maggiore del Campo DA!");
							Setfocus(document.getElementById('txtDa'));
							return false;
						}												
					}
					
			    if (document.getElementById('txtIva').value=='') 
					{ 
						alert("E' necessario valorizzare il campo IVA!");
						return false; 
					}		
					else
					{
			        strValore = document.getElementById('txtIva').value;						
												if(!IsNumeric(strValore))
						{
							alert("Inserire un valore numerico nel campo IVA");
							Setfocus(document.getElementById('txtIva'));
							return false;
						}
						else
						{
												    sGG=document.getElementById('txtIva').value;
							if (!isNumber(sGG,0,3))
							{
								alert("Inserire al massimo 3 decimali dopo la virgola nel campo IVA!");
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
					parent.parent.Comandi.location.href="./CRicercaQuotaFissa.aspx";
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
					parent.parent.Comandi.location.href="./CRicercaQuotaFissa.aspx";
					parent.frames.item('loadInsert').location.href="../../../aspVuota.aspx";					
				}
				
			function ConfermaCancellazione()
			{
				if (confirm('Si vuole eliminare la Quota Fissa Selezionata?'))
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
			<table style="Z-INDEX: 101; POSITION: absolute; TOP: 8px; LEFT: 0px" cellPadding="0" width="100%"
				border="0">
				<tr>
					<td colSpan="6"><asp:label id="lblDescrizioneOperazione" runat="server" Width="100%" CssClass="Legend">Dati Quota Fissa - </asp:label>&nbsp;</td>
				</tr>
				<tr>
					<td>
						<asp:label id="Label3" CssClass="Input_Label" Runat="server">Anno</asp:label>&nbsp;&nbsp;
					</td>
					<td>
						<asp:textbox id="txtAnno" onblur="" onfocus="" runat="server" Width="80px" CssClass="Input_Number_Generali"
							MaxLength="4" AutoPostBack="True"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label1" CssClass="Input_Label" Runat="server">Tipologia Utenza</asp:label>&nbsp;&nbsp;
					</td>
					<td>
						<asp:dropdownlist id="ddlTipoUtenza" runat="server" Width="280px" Cssclass="Input_Text"></asp:dropdownlist>
					</td>
				</tr>
				<tr>
					<td>
						<asp:label id="Label5" CssClass="Input_Label" Runat="server">Da</asp:label>&nbsp;&nbsp;
					</td>
					<td>
						<asp:textbox id="txtDa" runat="server" Width="80px" CssClass="Input_Number_Generali" MaxLength="255"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label7" CssClass="Input_Label" Runat="server">A</asp:label>&nbsp;&nbsp;
					</td>
					<td>
						<asp:textbox id="txtA" runat="server" Width="80px" CssClass="Input_Number_Generali" MaxLength="255"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:label id="Label2" CssClass="Input_Label" Runat="server">Importo H2O 
						</asp:label>&nbsp;&nbsp;
					</td>
					<td>
						<asp:textbox id="txtTariffaH2O" runat="server" Width="80px" CssClass="Input_Number_Generali"
							MaxLength="255"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label4" CssClass="Input_Label" Runat="server">Importo 
						Depurazione </asp:label>&nbsp;&nbsp;
					</td>
					<td>
						<asp:textbox id="txtTariffaDep" runat="server" Width="80px" CssClass="Input_Number_Generali"
							MaxLength="255"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:label id="Label8" CssClass="Input_Label" Runat="server">Importo Fognatura 
						</asp:label>&nbsp;&nbsp;
					</td>
					<td>
						<asp:textbox id="txtTariffaFog" runat="server" Width="80px" CssClass="Input_Number_Generali"
							MaxLength="255"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label6" CssClass="Input_Label" Runat="server">% Iva</asp:label>&nbsp;&nbsp;
					</td>
					<td>
						<asp:textbox id="txtIva" runat="server" Width="80px" CssClass="Input_Number_Generali" MaxLength="255"></asp:textbox>
					</td>
				</tr>
			</table>
			<asp:button id="BtnSalva" style="DISPLAY: none" runat="server" Width="32px" Text="Salva" Height="32px"></asp:button>
			<asp:button id="BtnElimina" style="DISPLAY: none" runat="server" Width="32px" Text="Elimina"
				Height="32px"></asp:button>
		</form>
	</body>
</HTML>
