<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchDatiAccertatoOSAP.aspx.vb" Inherits="Provvedimenti.SearchDatiAccertatoOSAP"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>SearchDatiAccertatoOSAP</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
	<script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
    <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		    function checkDatiSelezionati() {
		        if (document.getElementById('hfIdContribuente').value == "") {
		            GestAlert('a', 'warning', '', '', 'Selezionare un contribuente');
		            return false;
		        }
		        if (document.getElementById('hfAnno').value == "-1") {
		            GestAlert('a', 'warning', '', '', 'Selezionare l\'anno d\'accertamento!');
		            return false;
		        }
		        return true;
		    }
		    function ApriInserimentoImmobile() {
		        if (checkDatiSelezionati() == true) {
		            winWidth = 850
		            winHeight = 600
		            myleft = (screen.width - winWidth) / 2
		            mytop = (screen.height - winHeight) / 2 - 40
		            caratteristiche = "toolbar=no,status,left=" + myleft + ",top=" + mytop + ",height =" + winHeight + ",width=" + winWidth + ",resizable"
		            Parametri = "codContribuente=" + document.getElementById('hfIdContribuente').value
		            Parametri += "&anno=" + document.getElementById('hfAnno').value
		            Parametri += "&provenienza=Pulsante&IdArticolo=-1"
		            WinPopUpRicercaTerritorio = window.open("InserimentoManualeImmobileOSAP.aspx?" + Parametri, "", caratteristiche)
		            return false;
		        }
		    }
		    function ApriRicercaAccertato() {
		        if (checkDatiSelezionati() == true) {
		            winWidth = 980
		            winHeight = 470
		            myleft = (screen.width - winWidth) / 2
		            mytop = (screen.height - winHeight) / 2 - 40
		            caratteristiche = "toolbar=no,status,left=" + myleft + ",top=" + mytop + ",height =" + winHeight + ",width=" + winWidth + ",resizable"
		            Parametri = "tributo=OSAP"
		            Parametri = "&codContribuente=" + document.getElementById('hfIdContribuente').value
		            Parametri += "&anno=" + document.getElementById('hfAnno').value
		            WinPopUpRicercaTerritorio = window.open("../GestioneAccertamenti/FrameRicercaAccertato.aspx?" + Parametri, "", caratteristiche)
		        }
		        return false;
		    }
		    function ApriRicercaTerritorio() {
		        if (checkDatiSelezionati() == true) {
		            winWidth = 980
		            winHeight = 680
		            myleft = (screen.width - winWidth) / 2
		            mytop = (screen.height - winHeight) / 2 - 40
		            caratteristiche = "toolbar=no,status,left=" + myleft + ",top=" + mytop + ",height =" + winHeight + ",width=" + winWidth + ",resizable"
		            Parametri = "codContribuente=" + document.getElementById('hdIdContribuente').value + "&anno=" + document.getElementById('ddlAnno').value
		            Parametri = Parametri + "&nominativo=" + escape(document.getElementById('txtNominativo').value)
		            WinPopUpRicercaTerritorio = window.open("FrameRicercaTerritorio.aspx?" + Parametri, "", caratteristiche)
		        }
		        return false;
		    }
		    function ApriRicercaAnater() {
		        if (checkDatiSelezionati() == true) {
		            winWidth = 980
		            winHeight = 470
		            myleft = (screen.width - winWidth) / 2
		            mytop = (screen.height - winHeight) / 2 - 40
		            caratteristiche = "toolbar=no,status,left=" + myleft + ",top=" + mytop + ",height =" + winHeight + ",width=" + winWidth + ",resizable"
		            WinPopFamiglia = window.open("../../OPENgovOSAP/RicercaAnater/FrmRicercaImmobile.aspx?sProvenienza=A", "", caratteristiche)
		        }
		        return false;
		    }
		    function FineElaborazioneAccertamento() {
				parent.parent.opener.document.getElementById('txtDataRettificaAvviso').value='<% = now.Date.ToShortDateString() %>';
			}
			
			function ApriModificaImmobileAnater(idProgressivo,IdArticolo) {
			    winWidth=850
				winHeight=600
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40 
				caratteristiche="toolbar=no,status,left="+myleft+",top="+mytop+",height ="+winHeight+",width="+winWidth+",resizable"
				Parametri = "provenienza=Griglia" + "&codContribuente=" + parent.document.getElementById('hdIdContribuente').value + "&anno=" + parent.document.getElementById('ddlAnno').value + "&idProgressivo=" + idProgressivo + "&IdArticolo=" + IdArticolo
				WinPopUpSanzioni=window.open("./InserimentoManualeImmobileOSAP.aspx?"+Parametri,"",caratteristiche) 
			}
			
			function ApriDettaglioSanzioni(idLegame,idCheck, idSanzioni)
			{
				if (eval("idCheck".checked) == false)
				{
					return false;
				}
				winWidth = 700
				winHeight = 600
				myleft = (screen.width - winWidth) / 2
				mytop=(screen.height-winHeight)/2 - 40 
				caratteristiche="toolbar=no,status,left="+myleft+",top="+mytop+",height ="+winHeight+",width="+winWidth+",resizable"
				Parametri="idLegame=" + idLegame + "&strSanzioni=" + idSanzioni
				WinPopUpSanzioni=window.open("./../GestioneAccertamenti/Sanzioni/FrameSanzioni.aspx?"+Parametri,"",caratteristiche) 
				return false;
			}
			
			function visBottoneAccerta()
			{
				parent.parent.Comandi.btnAccertamento.style.display='';
			}
			
			function RiepilogoAccertato(rettificato,id_provvedimento)
			{
				parent.location.href='RiepilogoAccertatoOSAP.aspx?nominativo='+parent.document.getElementById('txtNominativo').value+'&anno='+parent.document.getElementById('ddlAnno').value+'&RETTIFICATO='+rettificato+'&id_provvedimento='+id_provvedimento
			}
		</script>
	</HEAD>
	<BODY class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellPadding="0" width="100%" border="0">
				<TR>
                    <td>
				        <asp:button class="btnList_botton" id="btnInsManuale" onmouseover="this.className='btnList_botton'" onmouseout="this.className='btnList_botton'" runat="server" CausesValidation="False" Text=" Ins. Manuale " tooltip="Inserimento Manuale Immobile"></asp:button>
				        <asp:button class="btnList_botton" id="btnAccertato" onmouseover="this.className='btnList_botton'" onmouseout="this.className='btnList_botton'" runat="server" CausesValidation="False" Text=" Accertato " tooltip="Ricerca e Selezione Immobile da Accertato già Calcolato"></asp:button>
                    </td>
					<TD><asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label></TD>
				</TR>
				<TR>
					<TD><INPUT id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza"></TD>
				</TR>
			</table>
            <Grd:RibesGridView ID="GrdDatiAcc" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                OnRowDataBound="GrdRowDataBound" OnRowCommand="GrdRowCommand">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				<Columns>
					<asp:BoundField DataField="Progressivo" ReadOnly="True" HeaderText="Prg">
						<headerstyle width="10px"></headerstyle>
						<itemstyle horizontalalign="Center"></itemstyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
						<ItemStyle Width="300px"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label35" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"),DataBinder.Eval(Container, "DataItem.Civico"),DataBinder.Eval(Container, "DataItem.Interno"),DataBinder.Eval(Container, "DataItem.Esponente"),DataBinder.Eval(Container, "DataItem.Scala")) %>'>Label</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Data Inizio">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label29" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataInizioOccupazione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Data Fine">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label1" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataFineOccupazione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Durata">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="120px" HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label28" runat="server" text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.DurataOccupazione"),DataBinder.Eval(Container, "DataItem.TipoDurata.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Tipo Occup.">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label27" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.TipologiaOccupazione.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Cat.">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label2" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.Categoria.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Cons.">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label33" runat="server" text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.Consistenza"), DataBinder.Eval(Container, "DataItem.TipoConsistenzaTOCO.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Tariffa">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemTemplate>
							<asp:Label id="Label4" runat="server" text='<%# FncForGrd.FormattaCalcolo("T",DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Imp.">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemTemplate>
							<asp:Label id="Label5" runat="server" text='<%# FncForGrd.FormattaCalcolo("I",DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Sanz.">
						<headerstyle width="25px"></headerstyle>
						<itemstyle horizontalalign="Center"></itemstyle>
						<itemtemplate>
							<asp:CheckBox id="chkSanzioni" readonly="true" style="disabled:true;" runat="server"></asp:CheckBox>
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Int.">
						<headerstyle width="25px"></headerstyle>
						<itemstyle horizontalalign="Center"></itemstyle>
						<itemtemplate>
							<asp:CheckBox id="ChkInteressi" runat="server" oncheckedchanged="ChkInteressi_CheckedChanged" autopostback ="True" Checked='<%# FncForGrd.checkFlag(DataBinder.Eval(Container, "DataItem.Calcola_Interessi")) %>' >
							</asp:CheckBox>
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Leg">
						<headerstyle width="25px"></headerstyle>
						<itemstyle horizontalalign="Right"></itemstyle>
						<itemtemplate>
							<asp:Label id="lblLegame" runat="server" CssClass="Input_Label" Text='<%# DataBinder.Eval(Container, "DataItem.IdLegame") %>'></asp:Label>
                            <asp:HiddenField ID="hfIdLegame" runat="server" Value='<%# Eval("IDLEGAME") %>' />
                            <asp:HiddenField ID="hfIdProv" runat="server" Value='<%# Eval("Progressivo") %>' />
						</itemtemplate>
						<edititemtemplate>
							<asp:TextBox id="txtLegame" runat="server" CssClass="Input_Text_Right" Width="24px"></asp:TextBox>
						</edititemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Modif">
						<itemstyle horizontalalign="Center"></itemstyle>
						<itemtemplate>
							<asp:ImageButton id="imgEdit" runat="server" Width="16px" CommandName="Edit" ImageUrl="..\..\images\Bottoni\modifica.png" Height="19px" CommandArgument='<%# Eval("Progressivo") %>'></asp:ImageButton>
						</itemtemplate>
						<edititemtemplate>
							<asp:ImageButton id="imgUpdate" runat="server" Width="16px" CommandName="Update" ImageUrl="..\..\images\Bottoni\modifica.png" Height="19px" CommandArgument='<%# Eval("Progressivo") %>'></asp:ImageButton>
							<asp:ImageButton id="ImageButton2" runat="server" Width="14px" CommandName="Cancel" ImageUrl="..\..\images\Bottoni\cancel.png" Height="17px" CommandArgument='<%# Eval("Progressivo") %>'></asp:ImageButton>
						</edititemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Del" Visible="false">
						<headerstyle horizontalalign="Center"></headerstyle>
						<itemstyle horizontalalign="Center"></itemstyle>
						<itemtemplate>
							<asp:ImageButton id="imgDelete" runat="server" Width="16px" CommandName="Delete" ImageUrl="..\..\images\Bottoni\cestino.png" Height="19px" CommandArgument='<%# Eval("Progressivo") %>'></asp:ImageButton>
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Compl">
						<headerstyle horizontalalign="Center"></headerstyle>
						<itemstyle horizontalalign="Center"></itemstyle>
						<itemtemplate>
							<asp:ImageButton id="ImageCompleta" runat="server" Width="16px" Height="19px" ImageUrl="..\..\images\Bottoni\apri1.png" CommandArgument='<%# Eval("Progressivo") %>'></asp:ImageButton>
						</itemtemplate>
					</asp:TemplateField>
				</Columns>
			</Grd:RibesGridView>
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
			<asp:button id="btnAccertamento" style="DISPLAY: none" runat="server" Text="Avvia Accertamento"></asp:button>
			<asp:HiddenField id="hfAnno" runat="server"></asp:HiddenField>
			<asp:HiddenField id="hfIdContribuente" runat="server"></asp:HiddenField>
			<asp:textbox id="txtRiaccerta" runat="server" visible="False"></asp:textbox>
			<asp:button id="btnRiaccerta" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
			<asp:button id="btnRicarica" style="DISPLAY: none" runat="server" Text=""></asp:button>
    </form>
  </body>
</html>
