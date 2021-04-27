<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InsertStrade.aspx.vb" Inherits="OpenGovTerritorio.InsertStrade" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
		<title>Ricerca Strade</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../../funzioni JS/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">
			function VisualizzaBottoni(){
			
				parent.Comandi.Insert.style.display=''
				parent.Comandi.Insert.title='Inserimento Nuova Strada'
				parent.Comandi.Clear.style.display=''
				parent.Comandi.Clear.title='Pulisci Videata'
				parent.Comandi.Cancel.style.display=''
				parent.Comandi.Cancel.title='Torna Indietro'
				parent.Comandi.Search.style.display='none'
				parent.Comandi.NewInsert.style.display='none'
				parent.Comandi.Delete.style.display='none'
				parent.Comandi.Modify.style.display='none'
				parent.Comandi.Unlock.style.display='none'
				
										
				//parent.Comandi.info.innerText="<%=Session("DESC_TIPO_PROC_SERV")%>"
				document.getElementById('infoEnte').innerText="<%=session("DESCRIZIONE_ENTE")%>"
				parent.Comandi.info.innerText="Configurazione  -  " + "<%=Session("DESC_TIPO_PROC_SERV") %>" + '  -  Inserimento' //+ "<%=session("DESCRIZIONE_ENTE")%>"

			}
			
			function apriCappario(){	
			
				//NomeStrada=document.Form1.ddlToponimo(document.Form1.ddlToponimo.selectedIndex).innerText + ' ' + document.Form1.TxtNomeStrada.value
			    NomeStrada=document.getElementById('TxtNomeStrada').value
			    CodiceStrada=document.getElementById('TxtCodStrada').value
				winWidth=560
				winHeight=360
				myleft=(screen.width-winWidth)/2
				mytop=(screen.height-winHeight)/2
				WinPopUp=window.open("Cappario.aspx?Via="+NomeStrada+"&CodVia="+CodiceStrada,"CAPPARIO","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=3, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no")		
			}
			
			function apriAreeStrade(){	
				//NomeStrada=document.Form1.ddlToponimo(document.Form1.ddlToponimo.selectedIndex).innerText  + ' ' + document.Form1.TxtNomeStrada.value
			    NomeStrada=document.getElementById('TxtNomeStrada').value
			    CodiceStrada=document.getElementById('TxtCodStrada').value
				winWidth=560
				winHeight=360
				myleft=(screen.width-winWidth)/2
				mytop=(screen.height-winHeight)/2
				WinPopUp=window.open("AreeStrade.aspx?Via="+NomeStrada+"&CodVia="+CodiceStrada,"AREESTRADE","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=3, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no")		
			}
			
			function Controlla(){
				if (typeof(WinPopUp)=="object") { 
					if (typeof(WinPopUp.name)!="unknown") {
						WinPopUp.close()
					}
				} 
			}

			window.onfocus =Controlla 
				
			function ValidateControlsInsert() 
			{
			    if (document.getElementById('ddlToponimo').value=='-1')
				{
					alert('Inserire il Toponimo della strada!');  
					return false;
				}
			    if (document.getElementById('ddlFrazione').value=='-1')
				{
					alert('Inserire la Frazione della strada!');  
					return false;
				}
			    if (document.getElementById('txtDenominazione').value=='')
				{
					alert('Inserire la Denominazione della strada!');  
					return false;
				}									
			    if (!isNumber(document.getElementById('txtLunghezza').value) && document.getElementById('txtLunghezza').value!='')
				{
					alert('Il campo Lunghezza è stato inserito in un formato non corretto!');  
					return false;
				}	
			    if (!isNumber(document.getElementById('txtLarghezza').value) && document.getElementById('txtLarghezza').value!='')
				{
					alert('Il campo Larghezza è stato inserito in un formato non corretto!');  
					return false;
				}
			    document.getElementById('Insert').click();
			}
		</script>
	</head>
	<body class="SfondoVisualizza" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<tr>
					<td colspan="5">
						<!--<asp:label id="Label9" runat="server" CssClass="Legend">Inserimento Nuova Via </asp:label><br />-->
						<asp:label id="Label7" runat="server" CssClass="lstTabRow" width="100%">Dati Generali</asp:label>
 					</td>
				</tr>
				<tr>
					<td colspan="2">
						<asp:label id="Label1" runat="server" CssClass="Input_Label">Toponimo</asp:label><br>
						<asp:dropdownlist id="ddlToponimo" tabIndex="1" runat="server" Width="180px" CssClass="Input_Text"></asp:dropdownlist>
					</td>
					<td colspan="2">
						<asp:label id="Label3" runat="server" CssClass="Input_Label">Frazione</asp:label><br>
						<asp:dropdownlist id="ddlFrazione" tabIndex="2" runat="server" Width="180px" CssClass="Input_Text"></asp:dropdownlist>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<asp:label id="Label2" runat="server" CssClass="Input_Label">Denominazione</asp:label><br>
						<asp:textbox id="txtDenominazione" tabIndex="3" runat="server" Width="350px" CssClass="Input_Text"></asp:textbox>
					</td>
					<td colspan="2">
						<asp:label id="Label10" runat="server" CssClass="Input_Label">CAP</asp:label><br>
						<asp:textbox id="txtCap" tabIndex="4" runat="server" Width="87px" CssClass="Input_Text" MaxLength="10"></asp:textbox>
					</td>
					<td>
						<asp:button id="btnCappario" tabIndex="5" runat="server" Width="72px" Height="22px" Text="Cappario" ToolTip="Apri Gestione Cappario" CssClass="BottoniLink" style="DISPLAY: none"></asp:button>
					</td>
				</tr>
				<tr>						
					<td>
						<asp:label id="Label4" runat="server" CssClass="Input_Label">Lunghezza (mt)</asp:label><br>
						<asp:textbox id="txtLunghezza" tabIndex="6" runat="server" Width="110px" CssClass="Input_Text"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label5" runat="server" CssClass="Input_Label">Larghezza (mt)</asp:label><br>
						<asp:textbox id="txtLarghezza" tabIndex="7" runat="server" Width="110px" CssClass="Input_Text"></asp:textbox>
					</td>
					<td colspan="2">
						<asp:label id="Label8" runat="server" CssClass="Input_Label">Ex-denominazione</asp:label><br>
						<asp:textbox id="txtExDenominazione" tabIndex="8" runat="server" Width="250px" CssClass="Input_Text"></asp:textbox>
					</td>
					<td>
						<asp:button id="btnAreeStrade" tabIndex="9" runat="server" Width="72px" Height="22px" Text="Aree" ToolTip="Apri Gestione Aree Strade" CssClass="BottoniLink" style="DISPLAY: none"></asp:button>
					</td>
				<tr>
					<td colspan="4">
						<br />
						<asp:label id="Label6" runat="server" CssClass="lstTabRow" width="100%">Note</asp:label><br />
						<asp:textbox id="txtNote" tabIndex="10" runat="server" Width="692px" Height="60px" CssClass="Input_Text" TextMode="MultiLine"></asp:textbox>
					</td>
				</tr>
			</table>
            <div id="divDialogBox" class="col-md-12">
                <div class="modal-box">
                    <div id="divAlert" class="modal-alert">
                        <span class="closebtnalert" onclick="CloseAlert()">&times;</span>
                        <p id="pAlert">testo di esempio</p>
                        <input type="text" class="prompttxt"/>
                        <button id="btnDialogBoxeOK" class="confirmbtn Bottone BottoneOK" onclick="DialogConfirmOK();"></button>&nbsp;
                        <button id="btnDialogBoxeKO" class="confirmbtn Bottone BottoneKO" onclick="DialogConfirmKO();"></button>
                        <button id="CmdDialogConfirmOK" class="hidden" onclick="RaiseDialogConfirmOK();"></button>
                        <button id="CmdDialogConfirmKO" class="hidden" onclick="RaiseDialogConfirmKO();"></button>
                        <input type="hidden" id="hfCloseAlert" />
                        <input type="hidden" id="hfDialogOK" />
                        <input type="hidden" id="hfDialogKO" />
                    </div>
                </div>
                <input type="hidden" id="cmdHeight" value="0" />
            </div>
			<asp:textbox id="TxtCodStrada" style="DISPLAY: none" runat="server">-1</asp:textbox>
			<asp:button id="Clear" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="Cancel" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="Insert" style="DISPLAY: none" runat="server"></asp:button>
		</form>
	</body>
</html>
