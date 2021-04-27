<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SearchDatiAccertato.aspx.vb" Inherits="Provvedimenti.SearchDatiAccertato" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head>		
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
		    function ApriRicercaAccertato() {
		        if (checkDatiSelezionati() == true) {
		            winWidth = 980
		            winHeight = 470
		            myleft = (screen.width - winWidth) / 2
		            mytop = (screen.height - winHeight) / 2 - 40
		            caratteristiche = "toolbar=no,status,left=" + myleft + ",top=" + mytop + ",height =" + winHeight + ",width=" + winWidth + ",resizable"
		            WinPopUpRicercaTerritorio = window.open("../GestioneAccertamenti/FrameRicercaAccertato.aspx", "", caratteristiche)
		        }
		        return false;
		    }
		    function ApriInserimentoImmobile() {
		        if (checkDatiSelezionati() == true) {
		            winWidth = 1000
		            winHeight = 400
		            myleft = (screen.width - winWidth) / 2
		            mytop = (screen.height - winHeight) / 2 - 40
		            caratteristiche = "toolbar=no,status,left=" + myleft + ",top=" + mytop + ",height =" + winHeight + ",width=" + winWidth + ",resizable"
		            Parametri = "codContribuente=" + document.getElementById('hfIdContribuente').value
		            Parametri += "&annoAccertamento=" + document.getElementById('hfAnno').value
		            Parametri += "&CodTributo=TASI"
		            WinPopUpRicercaTerritorio = window.open("../GestioneAccertamenti/FrameInserisciImmobileManuale.aspx?" + Parametri, "", caratteristiche)
		        }
		        return false;
		    }
		    function FineElaborazioneAccertamento() {
				parent.parent.opener.document.getElementById('txtDataRettificaAvviso').value='<% =Now.Date.ToShortDateString() %>';
			}
			
		    function apriModificaImmobileAnater(idProgressivo, annoAccertamento, CodContribuente, CodTributo)
			{
				winWidth=1000
				winHeight=400
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40 
				caratteristiche="toolbar=no,status,left="+myleft+",top="+mytop+",height ="+winHeight+",width="+winWidth+",resizable"
				Parametri = "idProgressivo=" + idProgressivo + "&annoAccertamento=" + annoAccertamento + "&CodContribuente=" + CodContribuente + "&CodTributo=" + CodTributo
				WinPopUpSanzioni=window.open("../GestioneAccertamenti/FrameInserisciImmobileManuale.aspx?"+Parametri,"",caratteristiche)				
				return false;
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
				WinPopUpSanzioni = window.open("../GestioneAccertamenti/Sanzioni/FrameSanzioni.aspx?" + Parametri, "", caratteristiche)
				return false;
			}
			
			function visBottoneAccerta()
			{
			    parent.parent.Comandi.btnAccertamento.style.display='';
			}
			
			function RiepilogoAccertato(rettificato,id_provvedimento)
			{				
				parent.document.getElementById('attesaElabAccertamento').style.display='none';
			    //parent.location.href='../GestioneAccertamenti/RiepilogoAccertato.aspx?nominativo='+parent.document.formRicercaAnagrafica.txtNominativo.value+'&anno='+parent.document.getElementById('ddlAnno').value'&RETTIFICATO='+rettificato+'&id_provvedimento='+id_provvedimento
				parent.location.href = '../GestioneAccertamenti/RiepilogoAccertato.aspx?Tributo=TASI&IdContribuente=' + document.getElementById('hfIdContribuente').value + '&anno=' + parent.document.getElementById('ddlAnno').value + '&RETTIFICATO=' + rettificato + '&id_provvedimento=' + id_provvedimento
			}

			function ControlloLegame(){
				if (confirm('Attenzione!!\nUn immobile Dichiarato non è stato LEGATO ad alcun immobile Accertato!\nProseguendo questo immobile non verrà considerato nell\'accertamento\nContinuare? ')){
					document.getElementById('txtControlloLegame').value="1";
					document.getElementById('btnAccertamento').click();
                }else{
					parent.document.getElementById('attesaElabAccertamento').style.display='none';
                }			
			}

			function getCalcolaSpese(){
				if (parent.document.getElementById('chkspese').checked){
				    document.getElementById('hfCalcolaSpese').value = "1"
				}else{
				    document.getElementById('hfCalcolaSpese').value = "0"
				}
			}			
		</script>
</head>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginwidth="0"
		marginheight="0">
		<form id="Form1" runat="server" method="post">
			<table cellPadding="0" width="100%" border="0">
				<tr>
                    <td>
                        <asp:Button class="btnList_botton" ID="btnInsManuale" onmouseover="this.className='btnList_botton'" onmouseout="this.className='btnList_botton'" runat="server" CausesValidation="False" Text=" Ins. Manuale " ToolTip="Inserimento Manuale Immobile"></asp:Button>
                        <asp:Button class="btnList_botton" ID="btnAccertato" onmouseover="this.className='btnList_botton'" onmouseout="this.className='btnList_botton'" runat="server" CausesValidation="False" Text=" Accertato " ToolTip="Ricerca e Selezione Immobile da Accertato già Calcolato"></asp:Button>
                    </td>
					<td><asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label></td>
				</tr>
			</table>
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
						<HeaderStyle width="10px"></HeaderStyle>
						<ItemStyle horizontalalign="Center"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Dal">
						<HeaderStyle horizontalalign="Center" width="70px"></HeaderStyle>
						<ItemStyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:Label id="lblDal" runat="server" Text='<%# FncGrd.annoBarra(DataBinder.Eval(Container, "DataItem.DAL")) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Al">
						<HeaderStyle horizontalalign="Center" width="70px"></HeaderStyle>
						<ItemStyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:Label id="lblAl" runat="server" Text='<%# FncGrd.annoBarra(DataBinder.Eval(Container, "DataItem.AL")) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="Foglio" ReadOnly="True" HeaderText="Fg">
						<HeaderStyle width="15px"></HeaderStyle>
						<ItemStyle horizontalalign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="Numero" ReadOnly="True" HeaderText="N&#176;">
						<HeaderStyle width="15px"></HeaderStyle>
						<ItemStyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Sub">
						<itemtemplate>
							<asp:Label runat="server" Text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.Subalterno")) %>'></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField headertext="Cat">
						<HeaderStyle horizontalalign="Center" width="15px"></HeaderStyle>
						<itemtemplate>
							<asp:label runat="server" text='<%# DataBinder.Eval(Container, "DataItem.Categoria") %>'></asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField headertext="Cl">
						<HeaderStyle horizontalalign="Center" width="15px"></HeaderStyle>
						<itemtemplate>
							<asp:label runat="server" text='<%# DataBinder.Eval(Container, "DataItem.CLASSE") %>'></asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Cons">
						<itemtemplate>
							<asp:Label runat="server" Text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.Consistenza")) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField ReadOnly="True" datafield="zona" headertext="Zona"></asp:BoundField>
					<asp:BoundField DataField="TIPORENDITA" ReadOnly="True" HeaderText="TR">
						<HeaderStyle width="10px"></HeaderStyle>
						<ItemStyle horizontalalign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField headertext="Rendita">
						<HeaderStyle horizontalalign="Center" width="25px"></HeaderStyle>
						<ItemStyle horizontalalign="Right"></ItemStyle>
						<itemtemplate>
							<asp:label id="lblRendita" runat="server" text='<%# FncGrd.FormattaNumero(DataBinder.Eval(Container, "DataItem.Rendita"), 2) %>'>
							</asp:label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Valore">
						<HeaderStyle horizontalalign="Center" width="25px"></HeaderStyle>
						<ItemStyle horizontalalign="Right"></ItemStyle>
						<itemtemplate>
							<asp:Label id="lblRendita_Valore" runat="server" Text='<%# FncGrd.FormattaNumero(DataBinder.Eval(Container, "DataItem.Valore"), 2) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="percPossesso" ReadOnly="True" HeaderText="% Poss">
						<HeaderStyle width="15px"></HeaderStyle>
						<ItemStyle horizontalalign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="TITPOSSESSO" ReadOnly="True" HeaderText="Tit.Poss">
						<HeaderStyle width="15px"></HeaderStyle>
						<ItemStyle horizontalalign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="NUtilizzatori" ReadOnly="True" HeaderText="N. Utiliz">
						<HeaderStyle width="20px"></HeaderStyle>
						<ItemStyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Princ">
						<HeaderStyle width="25px"></HeaderStyle>
						<ItemStyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:CheckBox id="chkPrinc" runat="server" Checked='<%# FncGrd.checkFlag(DataBinder.Eval(Container, "DataItem.FLAGPRINCIPALE")) %>' Enabled="False">
							</asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Pert">
						<HeaderStyle width="25px"></HeaderStyle>
						<ItemStyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:CheckBox id="chkPert" runat="server" Checked='<%# FncGrd.checkPertinenza(DataBinder.Eval(Container, "DataItem.IdImmobilePertinenza"), DataBinder.Eval(Container, "DataItem.ID")) %>' Enabled="False">
							</asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Rid">
						<HeaderStyle width="25px"></HeaderStyle>
						<ItemStyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:CheckBox id="chkRidotto" runat="server" Checked='<%# FncGrd.checkMesiRiduzione(DataBinder.Eval(Container, "DataItem.mesiriduzione")) %>' Enabled="False">
							</asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="DESCRTIPOTASI" ReadOnly="True" HeaderText="Tipo">
						<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="TotDovuto" ReadOnly="True" HeaderText="Imp." DataFormatString="{0:#,##0.00}">
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Sanz">
						<HeaderStyle width="25px"></HeaderStyle>
						<ItemStyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:CheckBox id="chkSanzioni" runat="server" readonly="true" style="disabled:true;"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Int.">
						<headerstyle width="25px"></HeaderStyle>
						<itemstyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
                            <asp:ImageButton id="imgInteressi" runat="server" Height="15px" Width="15px" CommandName="RowInteressi" CommandArgument='<%# Eval("IDLEGAME") %>' ImageUrl='..\..\images\Bottoni\trasparente.png' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Leg">
						<HeaderStyle width="25px"></HeaderStyle>
						<ItemStyle horizontalalign="Right"></ItemStyle>
						<itemtemplate>
							<asp:Label id="lblLegame" runat="server" CssClass="Input_Label" Text='<%# DataBinder.Eval(Container, "DataItem.IdLegame") %>'></asp:Label>
						</ItemTemplate>
						<edititemtemplate>
							<asp:TextBox id="txtLegame" runat="server" CssClass="Input_Text_Right" Width="24px"></asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Modif">
						<ItemStyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:ImageButton id="imgEdit" runat="server" Cssclass="BottoneGrd BottoneModificaGrd" CommandName="RowEdit" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</ItemTemplate>
						<edititemtemplate>
							<asp:ImageButton id="imgUpdate" runat="server" Cssclass="BottoneGrd BottoneModificaGrd" CommandName="RowUpdate" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
							<asp:ImageButton id="ImageButton2" runat="server" Width="14px" Height="17px" ImageUrl="..\..\images\Bottoni\cancel.png" CommandName="RowCancel" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Del">
						<itemtemplate>
							<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Compl">
						<HeaderStyle horizontalalign="Center"></HeaderStyle>
						<ItemStyle horizontalalign="Center" Width="40px"></ItemStyle>
						<itemtemplate>
							<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
                            <asp:HiddenField runat="server" ID="hfIDSANZIONI" Value='<%# Eval("IDSANZIONI") %>' />
                            <asp:HiddenField runat="server" ID="hfInteressi" Value='<%# Eval("CalcolaInteressi") %>' />
						</ItemTemplate>
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
			<asp:textbox id="txtControlloLegame" runat="server" style="DISPLAY: none"></asp:textbox>
			<asp:Button id="btnAssociaAccertato" runat="server" Text="Associa Immobili Accertati" style="DISPLAY: none"></asp:Button>
            <asp:HiddenField id="hdEnteAppartenenza" runat="server" />
			<asp:HiddenField id="hfCalcolaSpese" runat="server" />
        </form>
	</body>
</html>
