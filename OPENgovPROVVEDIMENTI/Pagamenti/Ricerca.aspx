<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Ricerca.aspx.vb" Inherits="Provvedimenti.Ricerca" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>Ricerca</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" src="../../_js/skype_killer.js?newversion"></script>
	<script type="text/javascript" >
		window.onload =killSkype;
		
		function Ricerca(){
			if (!document.getElementById ("optPagamenti").checked && !document.getElementById ("optAccorpamenti").checked ){
			    GestAlert('a', 'warning', '', '', 'Selezionare la tipologia di ricerca\n-Pagamenti effettuati\n-Accorpamenti/Rateizzazioni')
			}else{
				document.getElementById ("btnRicerca").click ()
			}
		}
		
		function InserisciNuovoPagamento(){
			parent.Comandi.location.href="cmdPagamenti.aspx?from=Ricerca"
			parent.Visualizza.location.href="Pagamenti.aspx?from=Ricerca"
		}
		
		function NuovaRateizzazione(){
				parent.Visualizza.location.href="Accorpamenti.aspx"
				parent.Comandi.location.href="cmdAccorpamenti.aspx"
		}
		
		function dataaccredito(valore){
			if (valore){
				document.getElementById ("tdNumAtto").className ="Input_Label_disabled"
				document.getElementById ("tdData").className ="Input_Label_disabled centrato"
				document.getElementById ("tdDataDal").className ="Input_Label_disabled AllineaDestra"
				document.getElementById ("tdDataAl").className ="Input_Label_disabled"
				document.getElementById ("txtDataDal").disabled=true
				document.getElementById ("txtDataAl").disabled=true
				document.getElementById ("txtNumAtto").disabled=true			
			}else{
				document.getElementById ("tdNumAtto").className ="Input_Label"
				document.getElementById ("tdData").className ="Input_Label centrato"
				document.getElementById ("tdDataDal").className ="Input_Label AllineaDestra"
				document.getElementById ("tdDataAl").className ="Input_Label"
				document.getElementById ("txtDataDal").disabled=false
				document.getElementById ("txtDataAl").disabled=false
				document.getElementById ("txtNumAtto").disabled=false
			}
		}
		
		function ApriDettaglio(IdContribuente,IdAccorpamento){
			parent.Visualizza.location.href="Dettaglio.aspx?IdAccorpamento=" + IdAccorpamento + "&IdContribuente=" + IdContribuente;
			parent.Comandi.location.href = "cmdDettaglio.aspx";
		}
		
		function keyPress()
		{
			if (window.event.keyCode==13)
			{
				Ricerca();
			}
		}	
	</script>
</head>
  <body class="Sfondo">
    <form id="Form1" runat="server" method="post">
		<table width="100%">
			<tr class="Input_Label">
				<td>Cognome</td>
				<td>Nome</td>
				<td>Codice Fiscale</td>
				<td>Partita IVA</td>
			</tr>
			<tr>
				<td><asp:TextBox id="txtCognome" runat="server" CssClass="Input_Text" Width="376px"></asp:TextBox></td>
				<td><asp:textbox id="txtNome" runat="server" CssClass="Input_Text" Width="185px"></asp:textbox></td>
				<td><asp:textbox id="txtCodFisc" runat="server" CssClass="Input_Text" Width="160px"></asp:textbox></td>
				<td><asp:textbox id="txtPiva" runat="server" CssClass="Input_Text" Width="160px"></asp:textbox></td>
			</tr>
			<tr class="Input_Label">
				<td id="tdNumAtto">Num. Atto</td>
				<td colspan="2" class="centrato" id="tdData">Data Accredito</td>
				<td><asp:radiobutton id="optPagamenti" groupname="TipoRicerca" runat="server" text="Pagamenti effettuati" cssclass="Input_Label" Checked="True"></asp:radiobutton></td>
			</tr>
			<tr class="Input_Label">
				<td><asp:textbox id="txtNumAtto" runat="server" CssClass="Input_Text"></asp:textbox></td>
				<td id="tdDataDal" class="AllineaDestra"><span>Dal</span> 
                    <asp:textbox id="txtDataDal" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this)" size="12"></asp:textbox></td>
				<td id="tdDataAl"><span>Al</span> 
                    <asp:textbox id="txtDataAl" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this)" size="12"></asp:textbox></td>
				<td><asp:radiobutton id="optAccorpamenti" groupname="TipoRicerca" runat="server" text ="Accorpamenti/Rateizzazioni" CssClass="Input_Label"></asp:radiobutton></td>
			</tr>
		</table>
		<br>
		<fieldset >
			<legend class="Legend">Risultati della Ricerca</legend>
			<br>
			<asp:label id="lblInfo" runat ="server" visible="False" class="Input_Label_bold text-danger" style="float: right;margin-right: 10px;"></asp:label>
			<Grd:RibesGridView ID="GrdPagamenti" runat="server" BorderStyle="None" 
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
					<asp:BoundField datafield="cognome" readonly="True" headertext="Cognome">
						<headerstyle horizontalalign="Center" verticalalign="top"></HeaderStyle>
						<itemstyle horizontalalign="left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField datafield="nome" readonly="True" headertext="Nome">
						<headerstyle horizontalalign="Center" verticalalign="top"></HeaderStyle>
						<itemstyle horizontalalign="left"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField headertext="Cod.Fiscale/P.IVA">
						<headerstyle horizontalalign="Center" verticalalign="top" Width="120px"></HeaderStyle>
						<itemstyle horizontalalign="Center" Width="120px"></ItemStyle>
						<itemtemplate>
							<asp:label id="Label1" runat="server" text='<%# CF_PIVA(DataBinder.Eval(Container, "DataItem.Cod_fiscale"),DataBinder.Eval(Container, "DataItem.partita_iva")) %>'>
							</asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField headertext="Data Pagamento">
						<headerstyle horizontalalign="Center" verticalalign="top" Width="120px"></HeaderStyle>
						<itemstyle horizontalalign="Center" Width="120px"></ItemStyle>
						<itemtemplate>
							<asp:label id="lblDal" runat="server" text='<%# GiraDataFromDB(DataBinder.Eval(Container, "DataItem.data_Pagamento")) %>'>
							</asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField headertext="Imp. Pagato">
						<headerstyle horizontalalign="Center" verticalalign="top" Width="100px"></HeaderStyle>
						<itemstyle horizontalalign="Right" Width="100px"></ItemStyle>
						<itemtemplate>
							<asp:label id="lblRendita_Valore" runat="server" text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.Importo_Pagato"),2) %>'>
							</asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField datafield="descrizioneProvenienza" readonly="True" headertext="Provenienza">
						<headerstyle horizontalalign="Center" verticalalign="top" Width="180px"></HeaderStyle>
						<itemstyle horizontalalign="left" Width="180px"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField datafield="doc" readonly="True" headertext="Doc.">
						<headerstyle horizontalalign="Center" verticalalign="top"></HeaderStyle>
						<itemstyle horizontalalign="left"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="">
						<headerstyle horizontalalign="Center"></headerstyle>
						<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
						<itemtemplate>
						<asp:ImageButton runat="server" Cssclass="BottoneGrd bottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id_accorpamento") %>' alt=""></asp:ImageButton>
						    <asp:HiddenField runat="server" ID="hfid_provvedimento" Value='<%# Eval("id_provvedimento") %>' />
                            <asp:HiddenField runat="server" ID="hfid_accorpamento" Value='<%# Eval("id_accorpamento") %>' />
                            <asp:HiddenField runat="server" ID="hfCOD_CONTRIBUENTE" Value='<%# Eval("COD_CONTRIBUENTE") %>' />
                            <asp:HiddenField runat="server" ID="hfTIPO" Value='<%# Eval("TIPO") %>' />
                            <asp:HiddenField runat="server" ID="hfid_pagato" Value='<%# Eval("id_pagato") %>' />
                            <asp:HiddenField runat="server" ID="hfnrata" Value='<%# Eval("n_rata") %>' />
						</itemtemplate>
					</asp:TemplateField>
				</Columns>
			</Grd:RibesGridView>
			<Grd:RibesGridView ID="GrdAccorpamenti" runat="server" BorderStyle="None" 
				BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="99%"
				AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
				ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
				OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging" OnRowDataBound="GrdRowDataBound">
				<PagerSettings Position="Bottom"></PagerSettings>
				<PagerStyle CssClass="CartListFooter" />
				<RowStyle CssClass="CartListItem"></RowStyle>
				<HeaderStyle CssClass="CartListHead"></HeaderStyle>
				<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				<columns>
					<asp:BoundField datafield="cognome" readonly="True" headertext="Cognome">
						<headerstyle horizontalalign="Center" verticalalign="top" width="25%"></HeaderStyle>
						<itemstyle horizontalalign="left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField datafield="nome" readonly="True" headertext="Nome">
						<headerstyle horizontalalign="Center" verticalalign="top" width="25%"></HeaderStyle>
						<itemstyle horizontalalign="left"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField headertext="Codice Fiscale<br>Partita IVA">
						<headerstyle horizontalalign="Center" verticalalign="top" width="15%"></HeaderStyle>
						<itemstyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:label id="Label2" runat="server" text='<%# CF_PIVA(DataBinder.Eval(Container, "DataItem.cod_fiscale"),DataBinder.Eval(Container, "DataItem.partita_iva")) %>'>
							</asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField headertext="Importo<br>Totale &euro;">
						<headerstyle horizontalalign="Center" verticalalign="top" width="10%"></HeaderStyle>
						<itemstyle horizontalalign="Right"></ItemStyle>
						<itemtemplate>
							<asp:label id="Label4" runat="server" text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.sum_valore_rata"),2) %>'>
							</asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField headertext="Interessi &euro;">
						<headerstyle horizontalalign="Center" verticalalign="top" width="10%"></HeaderStyle>
						<itemstyle horizontalalign="Right"></ItemStyle>
						<itemtemplate>
							<asp:label id="Label3" runat="server" text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.sum_valore_interesse"),2) %>'>
							</asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField datafield="nRate" readonly="True" headertext="Rate">
						<headerstyle horizontalalign="Center" verticalalign="top" width="5%"></HeaderStyle>
						<itemstyle horizontalalign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField headertext="Importo<br>Pagato &euro;">
						<headerstyle horizontalalign="Center" verticalalign="top" width="10%"></HeaderStyle>
						<itemstyle horizontalalign="Right"></ItemStyle>
						<itemtemplate>
							<asp:label id="Label5" runat="server" text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.sum_importo_pagato"),2) %>'>
							</asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField datafield="doc" readonly="True" headertext="Doc.">
						<headerstyle horizontalalign="Center" verticalalign="top"></HeaderStyle>
						<itemstyle horizontalalign="left"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="">
						<headerstyle horizontalalign="Center"></headerstyle>
						<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
						<itemtemplate>
						<asp:ImageButton runat="server" Cssclass="BottoneGrd bottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID_ACCORPAMENTO") %>' alt=""></asp:ImageButton>
						    <asp:HiddenField runat="server" ID="hfID_ACCORPAMENTO" Value='<%# Eval("ID_ACCORPAMENTO") %>' />
                            <asp:HiddenField runat="server" ID="hfCOD_CONTRIBUENTE" Value='<%# Eval("COD_CONTRIBUENTE") %>' />
                            <asp:HiddenField runat="server" ID="hfTIPO" Value='<%# Eval("TIPO") %>' />
						</itemtemplate>
					</asp:TemplateField>
				</Columns>
			</Grd:RibesGridView>
		</fieldset>
        <div id="DivAttesa" runat="server" style="z-index: 103; position: absolute;display:none;">
            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
            <div class="BottoneClessidra">&nbsp;</div>
            <div class="Legend">Attendere Prego...<asp:Label ID="LblAvanzamento" runat="server"></asp:Label></div>
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
		<asp:Button id="btnRicerca" runat="server" Text="Ricerca" cssclass="displaynone"></asp:Button>
		<asp:Button ID="CmdStampa" Runat="server" style="DISPLAY: none"></asp:Button>
    </form>
  </body>
</html>
