<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AcqPesature.aspx.vb" Inherits="OPENgovTIA.AcqPesature"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>AcqPesature</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" type="text/javascript">
			function VisualizzaElaborazione(){
				DivAttesa.style.display='';
				DivAcq.style.display='none';
				DivEsitoAcq.style.display='';
				document.getElementById('fileUpload').disabled=true;
				parent.Comandi.document.getElementById("Import").disabled=true;
				window.setTimeout('caricaPagina()', 10000);
			}
			
			function caricaPagina(){
				document.location.href='AcqPesature.aspx';
			}
			
			function VisualizzaForm(){
				DivAttesa.style.display='none';
				DivAcq.style.display='';
				document.getElementById('fileUpload').disabled=false;
				//parent.Comandi.document.getElementById("Import").disabled=false;
			}
			
		</script>
	</head>
	<body class="SfondoVisualizza" leftMargin="1" topMargin="1" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
		    <div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: hidden">
			    <table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				    <TR>
					    <TD style="WIDTH: 464px; HEIGHT: 20px" align="left">
						    <SPAN class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							    <asp:Label id="lblTitolo" runat="server"></asp:Label>
						    </SPAN>
					    </TD>
					    <TD align="right" width="800" colSpan="2" rowSpan="2">
						    <INPUT class="Bottone BottoneImport" id="Import" title="Acquisizione Pesature" onclick="document.getElementById('CmdImporta').click()" type="button" name="Import">
					    </TD>
				    </TR>
				    <TR>
					    <TD style="WIDTH: 463px" align="left">
						    <SPAN class="NormalBold_title" id="info" runat="server" style="WIDTH: 400px; HEIGHT: 20px">
							     Variabile - Acquisizione Conferimenti</SPAN>
					    </TD>
				    </TR>
			    </table>
		    </div>
			<div id="DivAcq" class="col-md-12">
				<div class="col-md-12">
					<fieldset class="FiledSetRicerca col-md-12">
				        <p>
                            Per una corretta importazione, si ricorda che il file deve:
                            <ul type="disc">
							    <li>avere il nome che inizia con <b>[codice_ente]_[mese di riferimento]_[anno di riferimento]</b>, es.<%=Session("COD_ENTE")%>_<%=Today.Month.ToString.PadLeft(2, "0")%>_<%=Today.Year%>;</li>
							    <li>essere in formato CSV</li>
                                <li>avere dimensione massima <b>4MB <i>(4095KB)</i></b></li>
							    <li>avere come separatore il carattere <i>;</i>     <em>(punto e virgola)</em></li>
							    <li>avere come campi, nell'ordine:
                                    <ul type="disc">
                                        <li>CODICE TESSERA</li>
                                        <li>TIPO CONFERIMENTO</li>
                                        <li>DATA ORA</li>
                                        <li>LITRI</li>
                                        <li>NOTE</li>
                                    </ul>
                                </li>
                            </ul>
				        </p>
						<asp:label id="LblPercorso" CssClass="Input_Label" Runat="server">Percorso</asp:label>
						<input class="Input_Label" id="fileUpload" type="file" size="100" name="fileUpload" runat="server">
					</fieldset>
				</div>
				<div id="DivEsitoAcq">
					<fieldset class="FiledSetRicerca">
						<legend class="Legend">Dati ultima Importazione</legend>
						<div class="col-md-12">
                            <label class="Input_Label">Nome File Importato</label>&emsp;
                            <asp:label id="LblNomeFile" CssClass="Input_Label_bold" Runat="server"></asp:label>
						</div>
						<div class="col-md-12">
                            <label class="Input_Label">Esito Importazione</label>&emsp;
                            <asp:label id="LblEsito" CssClass="Input_Label_bold" Runat="server"></asp:label>
						</div>
						<div class="col-md-12">
							<label class="Input_Label">File Scarti</label>&emsp;
                            <asp:label ID="LblFileScarti" Runat="server" CssClass="Input_Label_bold" Font-Underline="True"></asp:label>
						</div>
						<div class="col-md-12">
							<label class="Input_Label">Totale Record da Importare</label>&emsp;
                            <asp:label id="LblRcDaImp" CssClass="Input_Label_bold" Runat="server"></asp:label>
						</div>
						<div class="col-md-6">
							<label class="Input_Label">Totale Tessere da Importare</label>&emsp;
                            <asp:label id="LblTessereDaImp" CssClass="Input_Label_bold" Runat="server"></asp:label>
						</div>
						<div class="col-md-6">
							<label class="Input_Label">Totale Tessere Importate</label>&emsp;
                            <asp:label id="LblTessereImport" CssClass="Input_Label_bold" Runat="server"></asp:label>
						</div>
						<div class="col-md-6">
							<label class="Input_Label">Totale Conferimenti Importati</label>&emsp;
                            <asp:label id="LblConfImport" CssClass="Input_Label_bold" Runat="server"></asp:label>
						</div>
						<div class="col-md-6">
							<label class="Input_Label">Totale Litri Importati</label>&emsp;
                            <asp:label id="LblLitriImport" CssClass="Input_Label_bold" Runat="server"></asp:label>
						</div>
						<div class="col-md-6">
							<label class="Input_Label">Totale Record Importati</label>&emsp;
                            <asp:label id="LblRcImport" CssClass="Input_Label_bold" Runat="server"></asp:label>
						</div>
						<div class="col-md-6">
							<label class="Input_Label">Totale Record Scartati</label>&emsp;
                            <asp:label id="LblRcScarti" CssClass="Input_Label_bold" Runat="server"></asp:label>
						</div>
					</fieldset>
				</div>
			</div>
            <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                <div class="BottoneClessidra">&nbsp;</div>
                <div class="Legend">Attendere Prego</div>
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
			<asp:button id="CmdImporta" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="CmdScarica" style="DISPLAY: none" runat="server"></asp:button>
		</form>
	</body>
</HTML>
