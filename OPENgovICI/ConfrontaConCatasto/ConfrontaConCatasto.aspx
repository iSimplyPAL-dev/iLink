<%@ Page language="c#" Codebehind="ConfrontaConCatasto.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.ConfrontaConCatasto.ConfrontaConCatasto" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="tblCorpo" cellSpacing="1" cellPadding="1" width="100%" align="center" border="0">
				<tr>
					<td class="Input_Label">Selezionare un Anno di Riferimento per il quale effettuare 
						le stampa.
						<br />
						<br />
						Sarà possibile effettuare tre tipologie di stampe:
						<br />
						<br />
						<!--*** 20120704 - IMU ***-->
						* Confronto ICI/IMU - Catasto: è la stampa riepilogativa delle posizioni presenti 
						sia in catasto che nel verticale ICI/IMU (per foglio numero e subalterno) che 
						differiscono per categoria, classe e valore (escludendo gli immobili di catasto 
						con mappale alfanumerico e con categoria E, F/1 o F/5).
						<br />
						<br />
						* Elenco delle posizioni di Catasto non presenti in ICI/IMU: è la stampa 
						riepilogativa delle posizioni di Catasto non presenti nel verticale ICI/IMU 
						(escludendo gli immobili di catasto con mappale alfanumerico, con categoria E, 
						F/1 o F/5 e con rendita uguale a zero).
						<br />
						<br />
						* Confronto ICI/IMU - Catasto: è la stampa riepilogativa delle posizioni presenti 
						sia in catasto che nel verticale ICI/IMU (per foglio, numero e subalterno) che 
						differiscono per proprietari (escludendo gli immobili di catasto con categoria 
						E, F/1 o F/5).
						<br />
						<br />
					</td>
				</tr>
				<tr>
					<td>
						<fieldset class="classeFiledSetRicerca"><legend class="Legend">Inserimento 
								parametri di ricerca</legend>
							<table id="tblFiltri" cellSpacing="1" cellPadding="5" width="100%" border="0">
								<tr>
									<td class="Input_Label" style="HEIGHT: 10px" align="left" width="100">Anno 
										riferimento<br />
										<asp:dropdownlist id="ddlAnnoRiferimento" runat="server" Width="200px" CssClass="Input_Text">
										</asp:dropdownlist>
										<asp:Label Runat="server" id="Label1" Width="399px"><br />Per filtrare gli immobili attivi (dal <= [anno] al >= [anno])</asp:Label>
										<asp:button id="btnTrova" style="DISPLAY: none" runat="server" Text="Trova" ToolTip="Permette di eseguire una ricerca in funzione dei filtri utilizzati"></asp:button></td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td>
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
			</table>
			<!--*** 20120704 - IMU ***-->
			<asp:Button id="btnICI" style="DISPLAY: none; Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 136px" runat="server"
				Text="Stampa Posizioni di catasto non presenti in ICI/IMU" Width="312px" onclick="btnICI_Click"></asp:Button>
			<asp:Button id="btnClasseCatastato" style="DISPLAY: none; Z-INDEX: 102; LEFT: 8px; POSITION: absolute; TOP: 104px"
				runat="server" Text="Stampa Diff ICI/IMU - CATASTO" onclick="btnClasseCatastato_Click"></asp:Button>
			<asp:Button id="btnPassaggioProprieta" style="DISPLAY: none; Z-INDEX: 102; LEFT: 10px; POSITION: absolute; TOP: 75px"
				runat="server" Text="Stampa Passaggio Proprietà" onclick="btnPassaggioProprieta_Click"></asp:Button>
            <div id="attesaCarica" runat="server" style="z-index: 101; position: absolute;display:none;">
                <div class="Legend" style="margin-top:40px;">Estrazione File Catasto in Corso...</div>
                <div class="BottoneClessidra">&nbsp;</div>
                <div class="Legend">Attendere Prego</div>
            </div>
			<div id="" style="DISPLAY: none; Z-INDEX: 101; LEFT: 232px; TOP: 140px"><br />
				<p> in Corso....</p>
				<p><IMG alt="" src="../../images/Clessidra.gif"></p>
				<p>Attendere Prego...</p>
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
		</form>
	</body>
</HTML>
