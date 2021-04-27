<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchAccertatiTARSU.aspx.vb" Inherits="Provvedimenti.SearchAccertatiTARSU"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>SearchAccertatiTARSU</title>
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
		            winWidth = 940
		            winHeight = 500
		            myleft = (screen.width - winWidth) / 2
		            mytop = (screen.height - winHeight) / 2 - 40
		            caratteristiche = "toolbar=no,status,left=" + myleft + ",top=" + mytop + ",height =" + winHeight + ",width=" + winWidth + ",resizable"
		            Parametri = "codContribuente=" + document.getElementById('hfIdContribuente').value
		            Parametri += "&anno=" + document.getElementById('hfAnno').value
		            Parametri += "&provenienza=Pulsante"
		            WinPopUpRicercaTerritorio = window.open("FrameInserisciImmobileManualeTARSU.aspx?" + Parametri, "", caratteristiche)
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
		            Parametri = "tributo=0434"
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
		            WinPopFamiglia = window.open("../../OPENgovTARSU/RicercaAnater/FrmRicercaImmobile.aspx?sProvenienza=A", "", caratteristiche)
		        }
		        return false;
		    }
		    function FineElaborazioneAccertamento() {
				parent.parent.opener.document.getElementById('txtDataRettificaAvviso').value='<% =Now.Date.ToShortDateString() %>';
			}
			
			function ApriModificaImmobileAnater(idProgressivo)
			{
				winWidth=940
				winHeight=600
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40 
				caratteristiche="toolbar=no,status,left="+myleft+",top="+mytop+",height ="+winHeight+",width="+winWidth+",resizable"
				Parametri = "provenienza=Griglia" + "&codContribuente=" + parent.document.getElementById('hdIdContribuente').value + "&anno=" + parent.document.getElementById('ddlAnno').value + "&idProgressivo=" + idProgressivo 
				WinPopUpSanzioni=window.open("./FrameInserisciImmobileManualeTARSU.aspx?"+Parametri,"",caratteristiche) 
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
				parent.location.href='RiepilogoAccertatoTARSU.aspx?nominativo='+parent.document.getElementById('txtNominativo').value+'&anno='+parent.document.getElementById('ddlAnno').value+'&RETTIFICATO='+rettificato+'&id_provvedimento='+id_provvedimento
			}
		</script>
	</HEAD>
	<BODY class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginheight="0"
		marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellPadding="0" width="100%" border="0">
				<TR>
                    <td>
				        <asp:button class="btnList_botton" id="btnInsManuale" onmouseover="this.className='btnList_botton'" onmouseout="this.className='btnList_botton'" runat="server" CausesValidation="False" Text=" Ins. Manuale " tooltip="Inserimento Manuale Immobile"></asp:button>
				        <asp:button class="btnList_botton Hidden" id="btnAnater" onmouseover="this.className='btnList_botton'" onmouseout="this.className='btnList_botton'" runat="server" CausesValidation="False" Text=" Territorio " tooltip="Ricerca e Selezione Immobile da Territorio"></asp:button>
				        <asp:button class="btnList_botton" id="btnAccertato" onmouseover="this.className='btnList_botton'" onmouseout="this.className='btnList_botton'" runat="server" CausesValidation="False" Text=" Accertato " tooltip="Ricerca e Selezione Immobile da Accertato già Calcolato"></asp:button>
				        <asp:button class="BottoniAccertamenti" id="btnTerritorio" onmouseover="this.className='BottoniAccertamentiOver'" style="DISPLAY: none" onmouseout="this.className='BottoniAccertamenti'" runat="server" CausesValidation="False" Text="Territorio " Visible="False"></asp:button>
				        <asp:button class="BottoniAccertamenti" id="btnCatasto" onmouseover="this.className='BottoniAccertamentiOver'" style="DISPLAY: none" onmouseout="this.className='BottoniAccertamenti'" runat="server" CausesValidation="False" Text="Catasto " Visible="False"></asp:button>
                    </td>
					<TD><asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label></TD>
				</TR>
				<TR>
					<TD><INPUT id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza"></TD>
				</TR>
			</table>
			<!--*** 20140701 - IMU/TARES ***-->
			<Grd:RibesGridView ID="GrdAccertato" runat="server" BorderStyle="None" 
				BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
				AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
				ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
				OnRowCommand="GrdRowCommand" OnRowDataBound="GrdRowDataBound">
				<PagerSettings Position="Bottom"></PagerSettings>
				<PagerStyle CssClass="CartListFooter" />
				<RowStyle CssClass="CartListItem"></RowStyle>
				<HeaderStyle CssClass="CartListHead"></HeaderStyle>
				<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				<columns>
					<asp:BoundField DataField="Progressivo" ReadOnly="True" HeaderText="Prg">
						<headerstyle width="10px"></headerstyle>
						<itemstyle horizontalalign="Center"></itemstyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Dal">
						<headerstyle horizontalalign="Center" width="70px"></HeaderStyle>
						<itemstyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:Label runat="server" Text='<%# FncGrd.annoBarra(DataBinder.Eval(Container, "DataItem.tDataInizio")) %>' ID="lblDal">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Al">
						<headerstyle horizontalalign="Center" width="70px"></HeaderStyle>
						<itemstyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:Label runat="server" Text='<%# FncGrd.annoBarra(DataBinder.Eval(Container, "DataItem.tDataFine")) %>' ID="lblAl">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="sVia" ReadOnly="True" HeaderText="Via">
						<HeaderStyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Civico">
						<headerstyle horizontalalign="Left" width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						<itemtemplate>
							<asp:Label runat="server" Text='<%# FncGrd.FormattaNumeriGrd(DataBinder.Eval(Container, "DataItem.scivico"))%>' ID="Label29">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="sInterno" ReadOnly="True" HeaderText="Interno">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="sfoglio" ReadOnly="True" HeaderText="Foglio">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="snumero" ReadOnly="True" HeaderText="Numero">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="ssubalterno" ReadOnly="True" HeaderText="Sub.">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="sdescrCategoria" ReadOnly="True" HeaderText="Cat.">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Left" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="nComponenti" ReadOnly="True" HeaderText="NC">
						<ItemStyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="nComponentiPV" ReadOnly="True" HeaderText="NC PV">
						<ItemStyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Forza PV">
						<ItemStyle horizontalalign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="chkForzaPV" runat="server" ReadOnly="True" Checked='<%# DataBinder.Eval(Container,"DataItem.bForzaPV")%>'></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="ImpTariffa" ReadOnly="True" HeaderText="Tariffa">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="nMq" ReadOnly="True" HeaderText="Mq">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Sanz.">
						<headerstyle width="25px"></headerstyle>
						<itemstyle horizontalalign="Center"></itemstyle>
						<itemtemplate>
							<asp:CheckBox id="chkSanzioni" readonly="true" runat="server" Checked='<%# FncGrd.checkFlag(DataBinder.Eval(Container, "DataItem.Sanzioni")) %>'></asp:CheckBox>
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Int.">
						<headerstyle width="25px"></headerstyle>
						<itemstyle horizontalalign="Center"></itemstyle>
						<itemtemplate>
							<asp:CheckBox id="ChkInteressi" runat="server" oncheckedchanged="ChkInteressi_CheckedChanged" autopostback ="True" Checked='<%# FncGrd.checkFlag(DataBinder.Eval(Container, "DataItem.Calcola_Interessi")) %>' >
							</asp:CheckBox>
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Leg">
						<headerstyle width="25px"></headerstyle>
						<itemstyle horizontalalign="Right"></itemstyle>
						<itemtemplate>
							<asp:Label id="lblLegame" runat="server" CssClass="Input_Label" Text='<%# DataBinder.Eval(Container, "DataItem.IdLegame") %>'>
							</asp:Label>
						</itemtemplate>
						<edititemtemplate>
							<asp:TextBox id="txtLegame" runat="server" CssClass="Input_Text_Right" Width="24px"></asp:TextBox>
						</edititemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Modif">
						<itemstyle horizontalalign="Center"></itemstyle>
						<itemtemplate>
							<asp:ImageButton id="imgEdit" runat="server" ImageUrl="..\..\images\Bottoni\modifica.png" Height="19px" Width="16px" CommandName="RowEdit" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</itemtemplate>
						<edititemtemplate>
							<asp:ImageButton id="imgUpdate" runat="server" ImageUrl="..\..\images\Bottoni\modifica.png" Height="19px" Width="16px" CommandName="RowUpdate" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
							<asp:ImageButton id="ImageButton2" runat="server" ImageUrl="..\..\images\Bottoni\cancel.png" Height="17px" Width="14px" CommandName="RowCancel" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</edititemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Del">
						<headerstyle horizontalalign="Center"></headerstyle>
						<itemstyle horizontalalign="Center"></itemstyle>
						<itemtemplate>
							<asp:ImageButton id="imgDelete" runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Compl">
						<headerstyle horizontalalign="Center"></headerstyle>
						<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
						<itemtemplate>
							<asp:ImageButton id="ImageCompleta" runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						    <asp:HiddenField runat="server" ID="hfIdLegame" Value='<%# Eval("IdLegame") %>' />
                            <asp:HiddenField runat="server" ID="hfImpNetto" Value='<%# Eval("ImpNetto") %>' />
                            <asp:HiddenField runat="server" ID="hfImpRuolo" Value='<%# Eval("ImpRuolo") %>' />
                            <asp:HiddenField runat="server" ID="hfImpInteressi" Value='<%# Eval("ImpInteressi") %>' />
                            <asp:HiddenField runat="server" ID="hfImpSanzioni" Value='<%# Eval("ImpSanzioni") %>' />
                            <asp:HiddenField runat="server" ID="hfImpRiduzione" Value='<%# Eval("ImpRiduzione") %>' />
                            <asp:HiddenField runat="server" ID="hfImpDetassazione" Value='<%# Eval("ImpDetassazione") %>' />
                            <asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("Id") %>' />
                            <asp:HiddenField runat="server" ID="hfSanzioni" Value='<%# Eval("Sanzioni") %>' />
                            <asp:HiddenField runat="server" ID="hfInteressi" Value='<%# Eval("Interessi") %>' />
                            <asp:HiddenField runat="server" ID="hfsDescrSanzioni" Value='<%# Eval("sDescrSanzioni") %>' />
                            <asp:HiddenField runat="server" ID="hfIdDettaglioTestata" Value='<%# Eval("IdDettaglioTestata") %>' />
                            <asp:HiddenField runat="server" ID="hfTipoPartita" Value='<%# Eval("TipoPartita") %>' />
						</itemtemplate>
					</asp:TemplateField>
				</columns>
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
		</FORM>
	</BODY>
</HTML>
