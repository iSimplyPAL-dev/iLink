<%@ Page language="c#" Codebehind="DichiarazioniView.aspx.cs" AutoEventWireup="True" Inherits="OPENgovTOCO.Dichiarazioni.DichiarazioniView" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<%@ Register Src="../Wuc/WucDichiarazione.ascx" TagName="wucDichiarazioneData" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Visualizza Dichiarazione</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" src="../../_js/Toco.js?newversion"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" style="OVERFLOW:auto" bottomMargin="0" leftMargin="0"
		topMargin="0" rightMargin="0">
		<form id="Form1" runat="server" method="post">
			<div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: auto">
				<table style="WIDTH:100%" border="0">
					<tr valign="top">
						<td>
							<span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
								<asp:Label id="lblTitolo" runat="server"></asp:Label>
							</span>
						</td>
						<td align="right" valign="middle" rowspan="2">
                            <!--*** 201810 - Calcolo puntuale ***-->
                            <asp:ImageButton runat="server" Cssclass="Bottone BottoneElabora hidden" ImageUrl="../../images/Bottoni/transparent28x28.png" id="Calcola" AlternateText="Calcola Avviso" OnClientClick="if (confirm('Si vuole eseguire il calcolo puntuale dell\'Avviso?')){$('#DivCalcoloAvviso').show();$('#hfCalcoloAvviso').val('1');$('#Edit').hide();$('#Delete').hide();$('#Calcola').hide();}else{GestAlert('a', 'warning', '', '', 'coso');}" />
							<asp:ImageButton runat="server" Cssclass="Bottone BottoneCancella" ImageUrl="../../images/Bottoni/transparent28x28.png" id="Delete" AlternateText="Elimina dichiarazione" onclick="Delete_Click" />
							<asp:ImageButton runat="server" Cssclass="Bottone BottoneApri" ImageUrl="../../images/Bottoni/transparent28x28.png" id="Edit" AlternateText="Modifica dichiarazione" onclick="Edit_Click" />
							<asp:ImageButton runat="server" Cssclass="Bottone BottoneAnnulla" id="Cancel" ImageUrl="../../images/Bottoni/transparent28x28.png" AlternateText="Torna alla ricerca" onclick="Cancel_Click" />
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<span class="NormalBold_title" id="info" style="WIDTH: 400px; HEIGHT: 20px">TOSAP/COSAP - Dichiarazioni - Gestione</span>
						</td>
					</tr>
				</table>
			</div>
			<asp:button id="btnRibalta" Text="btnRibalta" style="DISPLAY:none" Runat="server"></asp:button>
			<div style="OVERFLOW-Y:auto;WIDTH:100%;HEIGHT:547px">
				<uc1:wucDichiarazioneData id="wucDichiarazione" runat="server" style="Z-INDEX: 0"></uc1:wucDichiarazioneData>
			</div>
            <!--*** 201810 - Calcolo puntuale ***-->
            <div id="DivCalcoloAvviso" class="Sfondo col-md-12">
			    <div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: auto">
				    <table style="WIDTH: 100%" cellSpacing="0" cellPadding="0" border="0">
					    <tr>
						    <td>
							    <span runat="server" id="Subtitle" class="NormalBold_title" style="WIDTH: 400px; HEIGHT: 20px">Calcolo Avviso</span>
						    </td>
						    <td vAlign="middle" align="right">
							    <asp:imagebutton runat="server" id="btnCalcola" ImageUrl="../../images/Bottoni/transparent28x28.png" Cssclass="Bottone BottoneCalcolo" ToolTip="Avvia calcolo avvisi" onclick="Calcola_Click" OnClientClick="$('#divElaborazioneInCorso').show();"></asp:imagebutton>
                                <asp:ImageButton runat="server" Cssclass="Bottone BottoneAnnulla" id="BackToDich" ImageUrl="../../images/Bottoni/transparent28x28.png" AlternateText="Torna alla Concessione" OnClientClick="$('#DivCalcoloAvviso').hide();$('#hfCalcoloAvviso').val('0');$('#Edit').show();$('#Delete').show();$('#Calcola').show();" />
						    </td>
					    </tr>
				    </table>
			    </div>
			    <div class="col-md-12">
                    <div class="col-md-12">
				        <div class="col-md-1">
					        <asp:label runat="server" id="lblAnno" CssClass="Input_Label">Anno</asp:label><br>
					        <asp:dropdownlist runat="server" id="ddlAnno" CssClass="Input_Text"></asp:dropdownlist>
				        </div>
				        <div class="col-md-1">
					        <asp:label runat="server" CssClass="Input_Label">Data Scadenza</asp:label><br>
					        <asp:textbox runat="server" id="TxtDataScadenza" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" maxlength="10"></asp:textbox>				
				        </div>
				        <div class="col-md-3">
					        <asp:RadioButton runat="server" ID="optEffettivo" GroupName="TipoRuolo" Text="Effettivo" CssClass="Input_Label" Checked="True"></asp:RadioButton>
					        <asp:RadioButton runat="server" ID="optSimulazione" GroupName="TipoRuolo" Text="Simulazione" CssClass="Input_Label"></asp:RadioButton>
				        </div>
				        <div class="col-md-6">
					        <!--*** 201511 - template documenti per ruolo ***-->
					        <asp:Label runat="server" ID="lblUploadFile" Text="File" CssClass="Input_Label"></asp:Label>&nbsp;
					        <asp:FileUpload ID="fileUploadDOT" runat="server" CssClass="Input_Text" Width="400px" />
					        <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="fileUploadDOT" ErrorMessage="Seleziona il file da importare" ValidationGroup="UploadValidation"></asp:RequiredFieldValidator>
					        <br />
					        <asp:Label runat="server" ID="lblMessage" Text="" Visible="false" CssClass="ERRORSTYLE"></asp:Label>
				        </div>
                    </div><br />
				    <div class="col-md-12">
					    <asp:Label runat="server" ID="lblRiepilogoAvviso" CssClass="Input_Label"></asp:Label><br>
                        <Grd:RibesGridView ID="GrdDocumenti" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical"  Width="600px"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
						    <Columns>
							    <asp:BoundField DataField="name" SortExpression="name" HeaderText="Nome Documento">
								    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
							    </asp:BoundField>
							    <asp:TemplateField>
								    <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Center"></ItemStyle>
								    <ItemTemplate>
									    <A href='<%# DataBinder.Eval(Container, "DataItem.url") %>' target="_blank">Apri</A>
								    </ItemTemplate>
								    <EditItemTemplate>
									    <A href='<%# DataBinder.Eval(Container, "DataItem.url") %>' target="_blank">Apri</A>
								    </EditItemTemplate>
							    </asp:TemplateField>
						    </Columns>
					    </Grd:RibesGridView>
				    </div>
			    </div>
                <asp:HiddenField runat="server" ID="hfCalcoloAvviso" Value="0" />
            </div>
			<div id="divElaborazioneInCorso" runat="server" style="z-index: 101; position: absolute;display:none;">
				<div class="Legend" style="margin-top:40px;">Elaborazione ruoli anno <asp:Label runat="server" ID="lblElaborazioneAnno"></asp:Label> in Corso...</div>
				<div class="BottoneClessidra">&nbsp;</div>
				<div class="Legend">Attendere Prego...<asp:Label runat="server" ID="LblAvanzamento"></asp:Label></div>
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
