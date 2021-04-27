<%@ Page Language="vb" AutoEventWireup="true" Codebehind="VecchioContatore.aspx.vb" Inherits="OpenUtenze.VecchioContatore" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>Letture del Vecchio Contatore</title>
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta content="True" name="vs_showGrid">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/MyUtility.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
		<script>
			function VisualizzaGiri(txtTemp,txtIDTemp,txtFocusTemp,objForm)
			{
				strtxtTemp=txtTemp.name;
				strtxtIDTemp=txtIDTemp.name;
				strtxtFocusTemp=txtFocusTemp.name;
				strFormName=objForm.name;
		
				/*WinPopUp=OpenPopup('OpenUtenze','/OpenUtenze/DataEntryLetture/VisualizzaGiri.aspx?FIELDMNAME='+strtxtTemp+'&IDFIELDMNAME='+strtxtIDTemp+'&FOCUSFIELDMNAME='+strtxtFocusTemp+'&FORM='+strFormName,'W3','500','500',0,0,'yes','no');*/
				WinPopUp=OpenPopup('OpenUtenze','<%=Session("PATH_APPLICAZIONE")%>/DataEntryLetture/VisualizzaGiri.aspx?FIELDMNAME='+strtxtTemp+'&IDFIELDMNAME='+strtxtIDTemp+'&FOCUSFIELDMNAME='+strtxtFocusTemp+'&FORM='+strFormName,'W3','500','500',0,0,'yes','no');
			}

		function VerificaData(txtData)
		{
			if (!IsBlank(txtData.value ))
			{	
				if(!isDate(txtData.value)) 
				{
					alert("Inserire la Data di Lettura correttamente in formato: GG/MM/AAAA!");
					txtData.value='';
					Setfocus(txtData);
					return false;
				}
			}				
		}	
		
		function VerificaDataDiLettura(txtData)
		{
			if (!IsBlank(txtData.value ))
			{	
				if(!isDate(txtData.value)) 
				{
					alert("Inserire la Data di Lettura correttamente in formato: GG/MM/AAAA!");
					txtData.value='';
					Setfocus(txtData);
					return false;
				}
				else
				{
					str=txtData.value
					alert(str)
					document.getElementById('txtDataLetturaTemp').value=	str
					document.getElementById('btnServ').click();
				}
			}				
		}	
		
		function VerificaCampi()
		{	
			sMsg=""
			if (IsBlank(document.getElementById('txtDatadiLettura').value)) 
			{ 
				sMsg = sMsg + "[Data  Lettura]\n" ; 
			} 
			if (IsBlank(document.getElementById('txtLetturaAttuale').value ))
			{
				sMsg = sMsg + "[Lettura Attuale]\n" ; 
			}
			if (IsBlank(document.getElementById('txtConsEffettivo').value ))
			{
				sMsg = sMsg + "[Consumo Effettivo]\n" ; 
			}	
			
			if (IsBlank(sMsg)) 
			{ 
					document.frmSottoAttivita.submit(); 
			} 
			else 
			{ 
				strMessage ="Attenzione...\n\n I campi elencati sono obbligatori!\n\n" 
				alert(strMessage + sMsg);
				Setfocus(document.getElementById('txtDatadiLettura'));
				return false; 
			} 
			return true; 
	  } 
		</script>
</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" marginwidth="0"
		marginheight="0">
		<form id="Form1" runat="server" method="post">
			<asp:textbox id="txtIDGIRO" style="DISPLAY: none" runat="server"></asp:textbox><asp:textbox id="txtDataLetturaTemp" style="DISPLAY: none" runat="server"></asp:textbox>
			<table cellSpacing="0" cellPadding="1" width="75%" align="center" border="0">
				<tr>
					<td class="lstTabRow" colSpan="5">Dati Contatore&nbsp;<asp:label id="lblContatore" runat="server" cssclass="NormalBold"></asp:label></td>
				</tr>
				<tr>
					<td class="Input_Label_Bold" width="25%">Ente Di Appartenenza</td>
					<td class="Input_Label_Bold" width="25%">Impianto</td>
					<td class="Input_Label_Bold" width="25%">Ubicazione <a onclick="if (document.getElementById('txtUbicazione').value == '') {GestAlert('a', 'warning', '', '', 'Inserire il Nome della Via');} else {GetStradario(document.getElementById('hdCodiceViaLetture,document.getElementById('txtUbicazione,document.getElementById('hdEnteAppartenenzaLetture,document.frmModifica);}"
							href="javascript: void(0)"><img alt="Scelta Persona" src="../images/Lista.png" align="absMiddle" border="0"></a></td>
					<td class="Input_Label_Bold" width="25%">N° Civico</td>
				</tr>
				<tr>
					<td style="HEIGHT: 17px"><asp:textbox id="txtEnteAppartenenza" runat="server" cssclass="InpEnabled_Contatori" Enabled="False"
							ToolTip="Ente di Appartenenza" Width="180px"></asp:textbox></td>
					<td style="HEIGHT: 17px"><asp:textbox id="txtImpianto" runat="server" cssclass="InpEnabled_Contatori" Enabled="False"
							ToolTip="Codice Impianto" Width="80px"></asp:textbox></td>
					<td style="HEIGHT: 17px"><asp:textbox id="txtUbicazione" runat="server" cssclass="Input_Text" Enabled="False"
							ToolTip="Ubicazione Del Contatore" Width="188px" MaxLength="50"></asp:textbox></td>
					<td style="HEIGHT: 17px"><asp:textbox id="txtNCivico" runat="server" cssclass="Input_Number_Generali" Enabled="False"
							ToolTip="Numero Civico del Contatore" Width="49px" MaxLength="10"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label_Bold" width="25%">Giro</td>
					<td class="Input_Label_Bold" width="25%">Sequenza</td>
					<td class="Input_Label_Bold" width="25%" colSpan="2">Lato Strada</td>
				</tr>
				<tr>
					<td><asp:TextBox ReadOnly="True" cssclass="Input_Text" ID="txtGiroContatore" Runat="server"></asp:TextBox><asp:dropdownlist id="cboGiro" style="DISPLAY:none" runat="server" Enabled="False" Width="184px" Cssclass="Input_Text"></asp:dropdownlist></td>
					<td><asp:textbox id="txtSequenza" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}" ToolTip="Sequenza" Width="80px" MaxLength="5" ReadOnly="True"></asp:textbox></td>
					<td colSpan="2"><asp:textbox id="txtLatoStrada" runat="server" cssclass="Input_Text" ToolTip="Lato Strada"
							Width="56px" MaxLength="1" ReadOnly="True"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label_Bold" width="25%">Posizione Contatore</td>
					<td class="Input_Label_Bold" width="25%">Progressivo</td>
					<td class="Input_Label_Bold" width="25%">Matricola</td>
					<td class="Input_Label_Bold" width="25%">Tipo Contatore</td>
				</tr>
				<tr>
					<td><asp:TextBox ReadOnly="True" cssclass="Input_Text" ID="txtPosizioneContatore" Runat="server"></asp:TextBox><asp:dropdownlist id="cboPosizione" style="DISPLAY:none" runat="server" Enabled="False" Width="184px"
							Cssclass="Input_Text"></asp:dropdownlist></td>
					<td><asp:textbox id="txtProgressivo" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}" ToolTip="Posizione Progressiva del contatore" Width="80px" MaxLength="5" ReadOnly="True"></asp:textbox></td>
					<td><asp:textbox id="txtMatricola" runat="server" cssclass="Input_Text" Enabled="False"
							ToolTip="Matricola" Width="112px"></asp:textbox></td>
					<td><asp:TextBox ReadOnly="True" cssclass="Input_Text" ID="txtTipoContatore" Runat="server"></asp:TextBox><asp:dropdownlist id="cboTipoContatore" style="DISPLAY:none" runat="server" Enabled="False" Width="156px"
							Cssclass="Input_Text"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td class="Input_Label_Bold" width="25%" colSpan="4">Note Contatore</td>
				</tr>
				<tr>
					<td colSpan="5"><asp:textbox id="txtNoteContatore" runat="server" cssclass="Input_Text" ToolTip="Note Inerenti al Contatore"
							Width="716px" MaxLength="500" ReadOnly="True" TextMode="MultiLine" Height="54px"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label_Bold" width="25%">Data Attivazione</td>
					<td class="Input_Label_Bold" width="25%">Data Sostituzione</td>
					<td class="Input_Label_Bold" width="25%">Data Rim. Temporanea</td>
					<td class="Input_Label_Bold" width="25%">Data Cessazione</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtDataAttivazione" runat="server" cssclass="Input_Text" Enabled="False"
							ToolTip="Data di Attivazione Contatore" Width="113"></asp:textbox></td>
					<td><asp:textbox id="txtDataSostituzione" runat="server" cssclass="Input_Text" Enabled="False"
							ToolTip="Data Sostituzione" Width="113"></asp:textbox></td>
					<td><asp:textbox id="txtDataRimTemp" runat="server" cssclass="Input_Text" Enabled="False"
							ToolTip="Data Rimozione Temporanea" Width="128px"></asp:textbox></td>
					<td><asp:textbox id="txtDataCessazione" runat="server" cssclass="Input_Text" Enabled="False"
							ToolTip="Data Cessazione" Width="113px"></asp:textbox></td>
				</tr>
				<tr>
					<td class="lstTabRow" colSpan="5">&nbsp;</td>
				</tr>
				<tr>
					<td class="lstTabRow" colSpan="5">Dati Utenza</td>
				</tr>
				<tr>
					<td class="Input_Label_Bold" >N° Utente</td>
					<td class="Input_Label_Bold" >Tipo Utenza</td>
					<td class="Input_Label_Bold" >N° Utenze</td>
					<td class="Input_Label_Bold" >Minimo Fatturabile</td>
					<td class="Input_Label_Bold" >Minimo Fatturabile Rimz. Temp</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtNUtente" tabIndex="3" runat="server" cssclass="Input_Text" ToolTip="Numero Utente"
							Width="100px" ReadOnly="True"></asp:textbox></td>
					<td><asp:textbox id="txtTipoUtenza" tabIndex="3" runat="server" cssclass="Input_Text" ToolTip="Tipo di Utenza"
							Width="192px" ReadOnly="True"></asp:textbox></td>
					<td><asp:textbox id="txtNumeroUtenze" tabIndex="3" runat="server" cssclass="Input_Text"
							ToolTip="Numero Utenze" Width="100px" ReadOnly="True"></asp:textbox></td>
					<td><asp:textbox id="txtMinFatt" tabIndex="3" runat="server" cssclass="Input_Text" Enabled="False"
							ToolTip="Minimo Fatturabile" Width="112px"></asp:textbox></td>
					<td><asp:textbox id="txtMinFattRim" tabIndex="3" runat="server" cssclass="Input_Text" Enabled="False"
							ToolTip="Minimo Fatturabile rimozione temporanea" Width="112px"></asp:textbox></td>
				</tr>
				<tr>
					<td class="lstTabRow" colSpan="5">Letture Precedenti (Ultime 5 Disponibli)</td>
				</tr>
				<tr>
					<td colSpan="5"><asp:label id="info" runat="server" Cssclass="NormalBold"></asp:label></td>
				</tr>
				<tr>
					<td colSpan="5">
						<Grd:RibesGridView ID="GrdLetture" runat="server" BorderStyle="None" 
									  BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
									  AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
									  ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
									  OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
									  <PagerSettings Position="Bottom"></PagerSettings>
									  <PagerStyle CssClass="CartListFooter" />
									  <RowStyle CssClass="CartListItem"></RowStyle>
									  <HeaderStyle CssClass="CartListHead"></HeaderStyle>
									  <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<columns>
								<asp:TemplateField Visible="False">
									<itemtemplate>
										<asp:Label id="CODLETTURA" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CODLETTURA") %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Per.">
									<headerstyle wrap="False" width="5%"></headerstyle>
									<itemtemplate>
										<asp:Label id=Label9 runat="server" Text='<%# FncGrd.FormattaPeriodo(DataBinder.Eval(Container, "DataItem.PERIODO")) %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="D. Lett.">
									<headerstyle wrap="False" width="5%"></headerstyle>
									<itemstyle horizontalalign="Center" verticalalign="Middle"></itemstyle>
									<itemtemplate>
										<asp:Label id=Label1 runat="server" text='<%# FncGrd.FormatGrdData(DataBinder.Eval(Container, "DataItem.DATALETTURA"))%>'>Label</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Lett.">
									<headerstyle wrap="False" width="5%"></headerstyle>
									<itemstyle horizontalalign="Right" verticalalign="Middle"></itemstyle>
									<itemtemplate>
										<asp:Label id=Label2 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LETTURA") %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="C.E.">
									<headerstyle wrap="False" width="5%"></headerstyle>
									<itemstyle horizontalalign="Right" verticalalign="Middle"></itemstyle>
									<itemtemplate>
										<asp:Label id=Label3 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CONSUMO") %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="G.C.">
									<headerstyle wrap="False" width="5%"></headerstyle>
									<itemstyle horizontalalign="Right" verticalalign="Middle"></itemstyle>
									<itemtemplate>
										<asp:Label id=Label11 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GIORNIDICONSUMO") %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="C.T.">
									<headerstyle wrap="False" width="5%"></headerstyle>
									<itemstyle horizontalalign="Right" verticalalign="Middle"></itemstyle>
									<itemtemplate>
										<asp:Label id=Label8 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CONSUMOTEORICO") %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="T.L.">
									<headerstyle wrap="False" width="5%"></headerstyle>
									<itemtemplate>
										<asp:Label id=Label6 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MODALITA") %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="F.S.">
									<headerstyle wrap="False" horizontalalign="Center" width="5%" verticalalign="Middle"></headerstyle>
									<itemstyle wrap="False" horizontalalign="Center" verticalalign="Middle"></itemstyle>
									<itemtemplate>
										<asp:Label id=label5 runat="server" Text='<%#CheckStatus(DataBinder.Eval(Container, "DataItem.FATT")) %>'>
										</asp:Label>
									</itemtemplate>
									<footerstyle wrap="False"></footerstyle>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Fatt.">
									<headerstyle wrap="False" horizontalalign="Center" width="5%" verticalalign="Middle"></headerstyle>
									<itemstyle wrap="False" horizontalalign="Center" verticalalign="Middle"></itemstyle>
									<itemtemplate>
										<asp:Label id="FATTURATA" runat="server" Text='<%#CheckStatus(DataBinder.Eval(Container, "DataItem.FATTURATA")) %>'>
										</asp:Label>
									</itemtemplate>
									<footerstyle wrap="False"></footerstyle>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="D.Pas.">
									<headerstyle wrap="False" width="5%"></headerstyle>
									<itemtemplate>
										<asp:Label id="Label10" runat="server" Text='<%# FncGrd.FormatGrdData(DataBinder.Eval(Container, "DataItem.DATADIPASSAGGIO")) %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="An.1">
									<headerstyle wrap="False" width="15%"></headerstyle>
									<itemtemplate>
										<asp:Label id="Label14" runat="server" Text='<%# DescriAnomalia(DataBinder.Eval(Container, "DataItem.COD_ANOMALIA1")) %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="An.2">
									<headerstyle wrap="False" width="15%"></headerstyle>
									<itemtemplate>
										<asp:Label id="Label15" runat="server" Text='<%# DescriAnomalia(DataBinder.Eval(Container, "DataItem.COD_ANOMALIA2")) %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="An.3">
									<headerstyle wrap="False" width="20%"></headerstyle>
									<itemtemplate>
										<asp:Label id="Label16" runat="server" Text='<%# DescriAnomalia(DataBinder.Eval(Container, "DataItem.COD_ANOMALIA3")) %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
                                <asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
									<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' alt=""></asp:ImageButton>
									</itemtemplate>
								</asp:TemplateField>
							</columns>
							</Grd:RibesGridView>
					</td>
				</tr>
				<tr>
					<td colspan="5"><asp:label id="lblGiorniDiConsumo" runat="server" Visible="False" CssClass="Input_Label_Red">Giorni di Consumo:</asp:label><asp:textbox id="txtGiornidiConsumoGrid" tabIndex="3" runat="server" Width="76px" cssclass="InpEnabled_number"
							Enabled="False" Visible="False" ToolTip="Giorni Di Consumo"></asp:textbox><asp:label id="lblConsumoTeorico" runat="server" Visible="False" CssClass="Input_Label_Red">Consumo Teorico:</asp:label><asp:textbox id="txtConsumoTeoricoGrid" tabIndex="3" runat="server" Width="76px" cssclass="InpEnabled_number"
							Enabled="False" Visible="False" ToolTip="Consumo Teorico"></asp:textbox></td>
				</tr>
				<tr>
					<td colspan="5"></td>
				</tr>
				<tr>
					<td align="center" colspan="5"><input type="button" value="Chiudi" class="Bottone Bottone_New" onclick="window.close();" title="Chiude la Finestra"></td>
				</tr>
			</table></td></tr></TBODY></table>
		</form>
	</body>
</HTML>
