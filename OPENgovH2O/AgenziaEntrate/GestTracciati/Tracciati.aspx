<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Tracciati.aspx.vb" Inherits="OpenUtenze.Tracciati" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Tracciati</title>
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
		<script type="text/javascript" src="../../../_js/ControlloData.js?newversion"></script>
		<script type="text/javascript">
			function PopolaAppoggio()
			{
				if (document.getElementById('DdlAnno').value=='')
				{
					GestAlert('a', 'warning', '', '', 'Selezionare un\'anno!');
					Setfocus(document.getElementById('DdlAnno'));
					return false
				}
				if (document.getElementById('TxtDal').value=='')
				{
					GestAlert('a', 'warning', '', '', 'Inserire una data di partenza!');
					Setfocus(document.getElementById('TxtDal'));
					return false
				}
				else 
				{
					if(!isDate(document.getElementById('TxtDal').value)) 
					{
						alert("Inserire la Data di Emissione correttamente in formato: GG/MM/AAAA!");
						Setfocus(document.getElementById('TxtDal'));
						return false;
					}	
				}
				
				if (confirm('Si vuole procedere al popolamento della tabella d\'appoggio per l\'anno e la data selezionati?'))
				{
					DivAttesa.style.display='';
					document.getElementById('btnPopolaTabAppoggio').click();
				}
			}
		
			function EstraiTracciato()
			{
				if (confirm('Si vuole procedere all\'estrazione del tracciato per l\'anno selezionato?'))
				{
					DivAttesa.style.display='';
					document.getElementById('btnEstraiTracciatoAE').click();
				}
			}
			function EstraiXML() {
			    if (confirm('Si vuole procedere all\'estrazione del tracciato XML per l\'anno selezionato?')) {
			        DivAttesa.style.display = '';
			        document.getElementById('btnEstraiXMLAE').click();
			    }
			}
		</script>
	</HEAD>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<table id="tblTracciatiAE" cellSpacing="0" cellPadding="0" width="748" border="0">
				<tr>
					<td>
						<fieldset><asp:label id="LblInfo" Runat="server" CssClass="Input_Label">
								Per una corretta estrazione del tracciato procedere come segue:<br><br>
								- effettuare il controllo sui dati mancanti (voce a menù Agenzia Entrate >> Dati Mancanti);<br>
								- popolare la tabella d'appoggio per l'estrazione del tracciato (pulsante <img height="16px" src="../../Images/Bottoni/icon_export.png">);<br>
								- selezionare l'anno nella tabella e generare il file (pulsante <img height="16px" src="../../Images/Bottoni/imgAvaBadge.png">).<br>
							</asp:label></fieldset>
					</td>
				</tr>
				<tr>
					<td><br>
						<fieldset><legend class="Legend">Inserimento Parametri di Estrazione</legend>
							<table id="TblParametri" width="100%" runat="server">
								<tr>
									<td>
										<asp:label id="Label1" runat="server" CssClass="Input_Label">Anno Ruolo</asp:label><br>
										<asp:dropdownlist id="DdlAnno" onkeydown="keyPress();" tabIndex="1" runat="server" CssClass="Input_Text"></asp:dropdownlist>
									</td>
									<td>
										<asp:Label Runat="server" CssClass="Input_Label" id="Label5">Dal</asp:Label><br>
										<asp:TextBox ID="TxtDal" Runat="server" TabIndex="2" CssClass="Input_Text" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" style="TEXT-ALIGN: right" maxlength="10"></asp:TextBox>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td><asp:label id="LblMessage" runat="server" CssClass="Legend"></asp:label></td>
				</tr>
				<tr>
					<td><br>
						<Grd:RibesGridView ID="GrdTracciatiAE" runat="server" BorderStyle="None" 
							BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
							AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
							ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
							OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
							<PagerSettings Position="Bottom"></PagerSettings>
							<PagerStyle CssClass="CartListFooter" />
							<RowStyle CssClass="CartListItem"></RowStyle>
							<HeaderStyle CssClass="CartListHead"></HeaderStyle>
							<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
								<asp:BoundField DataField="Anno" HeaderText="Anno">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="NomeFile" HeaderText="Nome File"></asp:BoundField>
								<asp:TemplateField HeaderText="N.Utenti">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="Label2" Runat="server" Text='<%# FncGrd.FormatNumberInGrd(DataBinder.Eval(Container, "DataItem.NumeroUtenti")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="N.Records">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="nrc" Runat="server" Text='<%# FncGrd.FormatNumberInGrd(DataBinder.Eval(Container, "DataItem.NumeroRecords")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="N.Articoli">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="Label4" Runat="server" Text='<%# FncGrd.FormatNumberInGrd(DataBinder.Eval(Container, "DataItem.NumeroArticoli")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Data Estrazione">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="Label5" Runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataEstrazione")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
										<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneDownloadGrd" CommandName="RowDownload" CommandArgument='<%# Eval("idflusso") %>' alt=""></asp:ImageButton>
					                    <asp:HiddenField runat="server" ID="hfCodiceISTAT" Value='<%# Eval("CodiceISTAT") %>' />
                                        <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("idflusso") %>' />
									</itemtemplate>
								</asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>
                        <br>
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
			</table>
			<asp:button id="btnPopolaTabAppoggio" style="DISPLAY: none" Runat="server"></asp:button>
            <asp:button id="btnEstraiTracciatoAE" style="DISPLAY: none" Runat="server"></asp:button>
            <asp:button id="btnEstraiXMLAE" style="DISPLAY: none" Runat="server"></asp:button>
			<input id="paginacomandi" type="hidden" name="paginacomandi">
		</form>
	</body>
</HTML>
