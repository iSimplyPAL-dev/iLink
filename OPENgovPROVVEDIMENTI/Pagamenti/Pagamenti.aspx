<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Pagamenti.aspx.vb" Inherits="Provvedimenti.Pagamenti" %>
<%@ Register TagPrefix="uc1" TagName="ElencoRate" Src="WUCRate.ascx" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Pagamenti</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" src="../../_js/skype_killer.js?newversion"></script>
		<script type="text/javascript">
	//window.onload =killSkype;
	function DeleteContrib()
	{
		document.getElementById('txtNominativo').value='';
		document.getElementById('txtHiddenCodContribuente').value='-1';
		document.getElementById('txtHiddenIdDataAnagrafica').value='-1';
		document.getElementById("btnPulisciGriglia").click()
	}
	function ApriRicercaAnagrafe(nomeSessione)
	{ 
		winWidth=980 
		winHeight=680 
		myleft=(screen.width-winWidth)/2 
		mytop=(screen.height-winHeight)/2 - 40 
		caratteristiche="toolbar=no,status,left="+myleft+",top="+mytop+",height ="+winHeight+",width="+winWidth+",resizable"
		Parametri="sessionName=" + nomeSessione 
		WinPopUpRicercaAnagrafica=window.open("../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?"+Parametri,"",caratteristiche) 
		return false;
	}
	function viewAnagrafica(valGetAnagrafica,valDatiAnagrafica){
		document.getElementById("tblGetAnagrafica").style.display =valGetAnagrafica
		document.getElementById("tblDatiAnagrafica").style.display =valDatiAnagrafica
	}
	function AbilitafldPagamento(valore){
		document.getElementById ("fldPagamento").className =valore
	}
	function Abilita_btnSalvaPagamento(valore) {
		parent.Comandi.Abilita_btnSalvaPagamento(valore)
	}
		</script>
	</HEAD>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
		    <table width="100%">
				<!--blocco dati contribuente--><%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
				<tr id="TRPlainAnag">
				    <td>
				        <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				        <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				    </td>
				</tr>
				<tr id="TRSpecAnag">
				    <td>
			            <table id="tblGetAnagrafica" border="0" cellPadding="0" width="100%" style="DISPLAY:none">
				            <tr>
					            <td class="Input_Label">
						            <asp:label id="lblNominativo" runat="server">Nominativo</asp:label>&nbsp;
						            <asp:button id="btnFocus" runat="server" width="1px" height="1px"></asp:button><asp:imagebutton id="Imagebutton" runat="server" causesvalidation="False" class="BottoneSel BottoneLista"></asp:imagebutton>
					            </td>
				            </tr>
				            <tr>
					            <td>
						            <asp:textbox id="txtNominativo" tabIndex="4" runat="server" width="492px" tooltip="Nominativo"
							            cssclass="Input_Text" enabled="False"></asp:textbox>
						            <IMG id="imageDelete" onmouseover="this.style.cursor='hand'" onclick="DeleteContrib();"
							            alt="Pulisci Nominativo Selezionato" src="../images/cancel.png" Width="10px" Height="10px">
						            <asp:button id="btnRibalta" runat="server" width="1px" height="2px" cssclass="displaynone" text="Ribalta"></asp:button>
						            <asp:textbox id="txtHiddenIdDataAnagrafica" runat="server" width="24px" height="20px" cssclass="displaynone">-1</asp:textbox>
						            <asp:button id="btnSearchProvvedimenti" runat="server" width="1px" height="2px" cssclass="displaynone"
							            text="Provvedimenti"></asp:button>
					            </td>
				            </tr>
			            </table>
			            <table id="tblDatiAnagrafica" class="dati_anagrafe_tarsu_blu" border="0" cellspacing="0"
				            cellpadding="2" width="100%" height="102" style="DISPLAY:none">
				            <tr>
					            <td>
						            <asp:label id="lblCognomeNome" runat="server" height="12px" width="243px"></asp:label>
					            </td>
					            <td>
						            <asp:label id="Label32" runat="server" height="12px" width="32px">CF/P.IVA:</asp:label>
						            <asp:label id="lblCfPiva" runat="server"></asp:label></td>
					            <td>
						            <asp:label id="Label33" runat="server" height="12px" width="32px">SESSO:</asp:label>
						            <asp:label id="lblSesso" runat="server"></asp:label>
					            </td>
				            </tr>
				            <tr>
					            <td>
						            <asp:label id="Label34" runat="server" height="12px" width="131px">DATA DI NASCITA:</asp:label>
						            <asp:label id="lblDataNascita" runat="server"></asp:label>
					            </td>
					            <td colspan="2">
						            <asp:label id="Label35" runat="server" height="12px" width="159px">COMUNE DI NASCITA:</asp:label>
						            <asp:label id="lblComuneNascita" runat="server"></asp:label>
					            </td>
				            </tr>
				            <tr>
					            <td colspan="3">
						            <asp:label id="Label36" runat="server" height="12px" width="116px">RESIDENTE IN:</asp:label>
						            <asp:label id="lblResidenza" runat="server" width="550px"></asp:label>
					            </td>
				            </tr>
			            </table>
				    </td>
				</tr>
		    </table>
			<br><asp:label id="lblInfoProvv" runat="server" cssclass="Input_Label"></asp:label><br>
			<Grd:RibesGridView ID="GrdProvvedimenti" runat="server" BorderStyle="None" 
				BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
				AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
				ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
				OnRowDataBound="GrdRowDataBound" OnRowCommand="GrdRowCommand">
				<PagerSettings Position="Bottom"></PagerSettings>
				<PagerStyle CssClass="CartListFooter" />
				<RowStyle CssClass="CartListItem"></RowStyle>
				<HeaderStyle CssClass="CartListHead"></HeaderStyle>
				<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				<Columns>
					<asp:BoundField DataField="anno" ReadOnly="True" HeaderText="Anno">
						<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="numero_atto" ReadOnly="True" HeaderText="Numero Atto">
						<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Importo Ridotto">
						<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:label id="lblImporto_Totale_Ridotto" runat="server" text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.Importo_totale_ridotto"),2) %>'>
							</asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Importo">
						<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:label id="lblImporto_Totale" runat="server" text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.Importo_totale"),2) %>'>
							</asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Data Elab.">
						<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:label id="Label2" runat="server" text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_elaborazione")) %>'>
							</asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Data Not.">
						<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:label id="Label3" runat="server" text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_notifica_avviso")) %>'>
							</asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="ProvProc" ReadOnly="True" HeaderText="Provenienza">
						<HeaderStyle HorizontalAlign="Center" Width="35%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="pagato" ReadOnly="True" HeaderText="Pagato" DataFormatString="{0:N}">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Sel">
						<HeaderStyle Width="5%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:checkbox id="chkSeleziona" runat="server" readonly="true" Checked='<%# DataBinder.Eval(Container, "DataItem.Selezionato")%>' AutoPostBack="true" OnCheckedChanged="chkSeleziona_CheckedChanged"></asp:checkbox>
						    <asp:HiddenField runat="server" ID="hfid_provvedimento" Value='<%# Eval("id_provvedimento") %>' />
                            <asp:HiddenField runat="server" ID="hfID_ACCORPAMENTO" Value='<%# Eval("ID_ACCORPAMENTO") %>' />
                            <asp:HiddenField runat="server" ID="hfTIPO" Value='<%# Eval("TIPO") %>' />
						</ItemTemplate>
					</asp:TemplateField>
                <asp:TemplateField HeaderText="Del">
                    <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton ID="imgDelete" runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" CommandArgument='<%# Eval("ID_ACCORPAMENTO") %>' alt="" OnClientClick="return confirm('Attenzione!\nStai per eliminare un\'accorpamento.\nVuoi procedere?')"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
				</Columns>
				</Grd:RibesGridView>
			<br>
			<uc1:elencorate id="ElencoRate" runat="server" visible="false"></uc1:elencorate>
			<br>
			<fieldset id="fldPagamento" class="displaynone">
				<legend class="Legend">
					Dati Pagamento</legend>
				<table border="0" cellPadding="0" width="60%">
					<colgroup>
						<col class="w20">
						<col class="w20">
						<col class="w20">
						<col class="w20">
						<col class="w20">
					</colgroup>
					<tr class="Input_Label">
						<td>Importo €</td>
						<td>Data Accredito</td>
						<td>Data Pagamento</td>
						<td>Numero Rata</td>
						<td>Provenienza</td>
					</tr>
					<tr>
						<td>
                            <asp:textbox id="txtImporto" runat="server" cssclass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);" size="12"></asp:textbox></td>
						<td>
                            <asp:textbox id="txtDataAcc" runat="server" cssclass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this)" size="12"></asp:textbox></td>
						<td>
                            <asp:textbox id="txtDataPag" runat="server" cssclass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this)" size="12"></asp:textbox></td>
						<td>
							<asp:textbox id="txtRata" runat="server" cssclass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 0);" size="5"></asp:textbox>
							<asp:dropdownlist id="ddlRata" runat="server" cssclass="Input_Text" visible="False" autopostback="True"></asp:dropdownlist>
						</td>
						<td><asp:dropdownlist id="ddlProvenienza" runat="server" cssclass="Input_Text"></asp:dropdownlist></td>
					</tr>
				</table>
			</fieldset>
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
            <asp:button runat="server" ID="btnSalvaPagamento" CssClass="hidden"></asp:button>
            <asp:Button runat="server" ID="CmdDeletePagamento" CssClass="hidden" />
			<asp:textbox id="txtSelezionato" runat="server" cssclass="displaynone"></asp:textbox>
			<asp:HiddenField runat="server" ID="hfIdProvvedimento" Value="0" />
			<asp:HiddenField runat="server" ID="hfIdAccorpamento" value="0"/>
			<asp:HiddenField runat="server" ID="hfTipo" />
			<asp:button id="btnSelezionaProvvedimenti" runat="server" width="1px" height="2px" cssclass="displaynone" text="SelezionaProvvedimenti"></asp:button>
			<asp:button id="btnPulisciGriglia" runat="server" width="1px" height="2px" cssclass="displaynone" text="btnPulisciGriglia"></asp:button>
            <asp:HiddenField runat="server" ID="hfIdPagamento" Value="-1" />
		</form>
	</body>
</HTML>
