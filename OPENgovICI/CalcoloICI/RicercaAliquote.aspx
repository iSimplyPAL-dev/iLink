<%@ Page language="c#" Codebehind="RicercaAliquote.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.RicercaAliquote" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RicercaAliquote</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
        <script type="text/javascript">
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
    		
		    function Search()
		    {    		
			    //alert(document.frmRicerca.txtPercentuale.value);   			
			    /*if(document.frmRicerca.txtValore.value=='')
			    {
				    alert("Inserire un valore!")
				    return false;
			    }*/		    						
		        strValue=document.getElementById('txtValore').value;						
			    strValue = strValue.replace(",",".");
			    //alert(strValuePerc)    			
			    if(!IsNumeric(strValue))
			    {
			        GestAlert('a', 'warning', '', '', 'Valore inserito in un formato non valido!');
				    return false;
			    }				    								
			    Anno=document.getElementById('ddlAnno').value;
			    Tipo=document.getElementById('ddlTipo').value;
			    Parametri = "?ANNO=" + Anno + "&TIPO=" + Tipo + "&VALORE=" + strValue;
			    //*** 20140509 - TASI ***
			    Parametri += "&COD_TRIBUTO=" + document.getElementById('ddlTributo').value;
			    //*** ***
			    document.getElementById('loadGrid').src='ResultRicercaAliquote.aspx' + Parametri;
    	
			    return true;
		    }	

		    function viewDSAAP(){
			    if (document.getElementById ("ddlTipo").value=="DSAAP"){
			        GestAlert('a', 'warning', '', '', 'DETRAZIONE STATALE ABITAZIONE PRINCIPALE');
			    }
		    }

		    function GoToConfigAliquote() {
		        location.href = "ConfigAliquote.aspx?COD_TRIBUTO=" + document.getElementById('ddlTributo').value;
		    }
		    function CopyTo() {
		        divCopyTo.style.display='';
		        divSearch.style.display = 'none';
		    }
		    function GoToRicercaAliquote() {
		        divCopyTo.style.display = 'none';
		        divSearch.style.display = '';
		    }
		</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0">
		<form id="Form1" runat="server" method="post">
            <div id="divSearch" class="col-md-12">
			    <table id="tabEsterna" cellspacing="1" cellpadding="1" width="100%" border="0">
				    <tr>
					    <td class="Input_Label_title" colspan="3">Parametri di Ricerca</td>
				    </tr>
				    <tr>
					    <td style="WIDTH: 150px">
						    <asp:Label id="Label3" runat="server" CssClass="Input_Label">Anno</asp:Label>
					    </td>
					    <!--*** 20140509 - TASI ***-->
					    <td>
						    <asp:Label id="Label4" runat="server" CssClass="Input_Label">Tributo</asp:Label>
					    </td>
					    <!--*** ***-->
					    <td style="WIDTH: 456px">
						    <asp:Label id="Label1" runat="server" CssClass="Input_Label">Tipo Aliquota</asp:Label>
					    </td>
					    <td>
						    <asp:Label id="Label2" runat="server" CssClass="Input_Label">Valore</asp:Label>
					    </td>
				    </tr>
				    <tr>
					    <td style="WIDTH: 150px">
						    <asp:DropDownList id="ddlAnno" runat="server" CssClass="Input_Text" Width="120px"></asp:DropDownList>
					    </td>
					    <!--*** 20140509 - TASI ***-->
					    <td>
						    <asp:DropDownList id="ddlTributo" runat="server" CssClass="Input_Text" Width="150px"></asp:DropDownList>
					    </td>
					    <!--*** ***-->
					    <td style="WIDTH: 456px">
						    <asp:DropDownList id="ddlTipo" runat="server" CssClass="Input_Text" Width="424px"></asp:DropDownList>
					    </td>
					    <td>
						    <asp:TextBox id="txtValore" runat="server" CssClass="Input_Text"></asp:TextBox>
					    </td>
				    </tr>
				    <tr>
					    <td colspan="4">
					        <iframe class="bordoIFRAME" id="loadGrid" width="100%" height="350px" frameBorder="0"></iframe>
					    </td>
				    </tr>
				    <tr>
					    <td colspan="4" class="Input_Label">
						    Le informazioni dichiarative che contribuiscono al calcolo del dovuto IMU/TASI
						    <ul>
							    <li style="float:left;">
							        <span style="font-weight:bold">Caratteristica</span>
							        <br />
								    La caratteristica identifica il tipo di immobile:
								    <ul style="font-weight:normal; list-style-type:square">
                                        <li>Terreno agricolo</li>
                                        <li>Terreno edificabile</li>
                                        <li>Fabbricato con rendita catastale</li>
                                        <li>Fabbricato a libri contabili</li>
								    </ul>
							    </li>
							    <li style="float:left; width:200px; font-weight:bold"><span>I tipi di utilizzo</span>
								    <ul style="font-weight:normal; list-style-type:square">
                                        <li>Abitazione principale</li>
                                        <li>A disposizione</li>
                                        <li>Locato</li>
                                        <li>Comodato d’uso</li>
                                        <li>…</li>
								    </ul>
							    </li>
							    <li style="float:left; width:200px; font-weight:bold"><span>Altre Informazioni</span>
								    <ul style="font-weight:normal; list-style-type:square">
                                        <li><span>Flag Abitazione principale</span></li>
                                        <li><span>Flag pertinenza</span></li>
                                        <li><span>Flag Coltivatore diretto</span></li>
                                        <li><span>Categorie catastali</span></li>
								    </ul>
							    </li>
						    </ul>
					    </td>
				    </tr>
				    <tr>
				        <td colspan="4" class="Input_Label">
						    <i>Alcuni esempi di caricamento in Dichiarazione:</i>
						    <ul class="aliquote">
                                <li><span>Abitazione principale:</span> selezionare tipo utilizzo “abitazione principale” ed attivare flag “abitazione principale”</li>
                                <li><span>Immobile locato:</span> selezionare tipo utilizzo “locato”, nessun flag attivo</li>
                                <li><span>Immobile concesso in comodato:</span> 
                                    <ul style="list-style-type:square">
                                        <li>se equiparato all’abitazione principale deve essere attivo flag abitazione principale e tipo utilizzo corrispondente</li>
                                        <li>se non equiparato, ma definita aliquota specifica, flag abitazione principale attivo e tipo di utilizzo corrispondente</li>
                                    </ul>
                                </li>
                                <li><span>Pertinenza:</span> selezionare tipo utilizzo “abitazione principale” ed attivare flag “pertinenza”</li>
                                <li><span>Fabbricati con aliquote specifiche:</span> nessun flag attivo, tipo di utilizzo corrispondente (il sistema identifica l’aliquota in base alla categoria catastale)</li>
						    </ul>
					    </td>
				    </tr>
			    </table>
            </div>
            <div id="divCopyTo" class="col-md-12">
                <div class="col-md-3">
                    <asp:Label runat="server" CssClass="Input_Label">Da Anno</asp:Label>&nbsp;
                    <asp:DropDownList ID="ddlAnnoFrom" runat="server" CssClass="Input_Text"></asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <asp:Label runat="server" CssClass="Input_Label">Ad Anno</asp:Label>&nbsp;
                    <asp:TextBox ID="txtAnnoTo" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="80px"></asp:TextBox>
                 </div>
                <div class="col-md-3">
                    <input class="Bottone BottoneSalva" id="Insert" onclick="document.getElementById('btnRibalta').click()" type="button" name="Insert" title="Salva">
                    <input class="Bottone BottoneAnnulla" id="Annulla" title="Torna alla pagina di Ricerca." onclick="GoToRicercaAliquote();" type="button" name="Annulla">
                </div>
            </div>
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
            <asp:button id="btnRibalta" style="DISPLAY: none" runat="server" onclick="btnRibalta_Click"></asp:button>
		</form>
	</body>
</HTML>
