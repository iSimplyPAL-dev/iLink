<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaAnagraficaDoppia.aspx.vb" Inherits="RicercaAnagraficaDoppia" %>

<html>
  <head>
		<%
	Dim sessioneName
	sessionName = Request.item("sessionName")
%>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../_js/Utility.js?newversion"></script>
		<script type="text/javascript" src="../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript">								
		    function UpdateContrib() {
		        if (document.getElementById('loadGrid') == null) {
		            GestAlert('a', 'warning', '', '', 'Eseguire prima la ricerca e poi effettuare l\'aggiornamento del soggetto anagrafico nell\'intero sistema!')
		            return false;
		        }
		        else {
		            var myIFrame = document.getElementById('loadGrid');
		            var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
		            myContent.UpdateContrib();
		        }
		    }
		    function estraiExcel() {
		        if (document.getElementById('loadGrid') == null) {
		            GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Eseguire la ricerca.');
		        }
		        else {
		            DivAttesa.style.display = '';
		            var myIFrame = document.getElementById('loadGrid');
		            var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
		            myContent.estraiExcel();
		        }
		        return false;
		    }
		    function Search(popUp)
		{
		    DivAttesa.style.display = '';
		    if(document.getElementById('optNOM').checked==false && document.getElementById('optCFPI').checked==false)
			{
				alert("Selezionare una Opzione di Ricerca!")
				return false;
			}		

			if(document.getElementById('txtPercentuale').value=='')
			{
				alert("Inserire una Percentuale di Precisione!")
				return false;
			}
						
			strValuePerc=document.getElementById('txtPercentuale').value;						
			strValuePerc = strValuePerc.replace(",",".");
			//alert(strValuePerc)
			
			if(!IsNumeric(strValuePerc))
			{
				alert("Inserire una Percentuale di Precisione!")
				return false;
			}				
			
			//int TipoRicerca;
			//double dblPerc;
			if(document.getElementById('optNOM').checked==true) 
			{
				TipoRicerca=1;				
			}
        else if(document.getElementById('optCFPI').checked==true)
			{
				TipoRicerca=0;
			}

		    Parametri = "?TIPO_RICERCA=" + TipoRicerca + "&PERCENTUALE=" + strValuePerc + "&Nominativo=" + document.getElementById('TxtNominativo').value + "&HasVuoti=" + document.getElementById('ChkHasVuoti').checked;			
			loadGrid.src="SearchResultsAnagraficaDoppia.aspx"+Parametri
	
			return true;
		}
		
		function IsNumeric(sText)
		{
			var ValidChars = "0123456789.";
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
    </head>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0">
		<form id="Form1" runat="server" method="post">
			<table id="tabEsterna" cellSpacing="1" cellPadding="1" width="100%" align="center" border="0">
				<tr>
					<td colspan="2">
					    <asp:label id="lblDescr" runat="server" CssClass="Input_Label">Il sistema evidenzia tutte le posizioni anagrafiche doppie per nominativo e/o codice fiscale in base ad una percentuale di precisione</asp:label>
					</td>
				</tr>
				<tr>
				    <td colspan="2">
				        <fieldset class="classeFiledSet" runat="server">
				            <legend class="Legend">Inserimento parametri di Ricerca</legend>
				            <table width="100%">
				                <tr>
					                <td>
					                    <asp:radiobutton id="optCFPI" runat="server" Text=" Anagrafiche Doppie per Codice Fiscale / Partita Iva" CssClass="Input_radio" GroupName="ANAGDOPPIE" checked="True"></asp:radiobutton>
					                </td>
					                <td colspan="2" align="center">
					                    <asp:radiobutton id="optNOM" runat="server" Text="  Anagrafiche Doppie per Nominativo" CssClass="Input_radio" GroupName="ANAGDOPPIE"></asp:radiobutton>
					                </td>
				                </tr>
				                <tr>
				                    <td>
				                        <asp:CheckBox ID="ChkHasVuoti" runat="server" CssClass="Input_CheckBox_NoBorder" TextAlign="Left" Text="Includi Doppi per Cod.Fiscale/P.Iva o Nominativo Vuoti" />
				                    </td>
					                <td>
						                <asp:label id="Label1" runat="server" CssClass="Input_Label">% di Precisione</asp:label>&nbsp;
						                <asp:textbox id="txtPercentuale" runat="server" CssClass="Input_Text" Width="40px"></asp:textbox>
					                </td>
					                <td>
						                <asp:label id="Label2" runat="server" cssclass="Input_Label">Cod.Fiscale/P.Iva o Nominativo</asp:label>&nbsp;
						                <asp:textbox id="TxtNominativo" runat="server" cssclass="Input_Text" width="300px"></asp:textbox>
					                </td>
				                </tr>
				            </table>
				        </fieldset>
				    </td>
			    </tr>
				<tr>
					<td colspan="2">
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
				<tr>
					<td colspan="2" class="Input_Label_title" colSpan="4">Visualizzazione Anagrafiche Estratte</td>
				</tr>
				<tr>
					<td colspan="2">
					    <iframe class="bordoIframe" id="loadGrid" style="WIDTH: 100%; HEIGHT: 400px" src="../aspVuota.aspx" frameBorder="0" width="100%" height="300"></iframe>
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
			<asp:button id="btnNuovo" style="DISPLAY: none" runat="server"></asp:button>
		</form>
	</body>
</html>
