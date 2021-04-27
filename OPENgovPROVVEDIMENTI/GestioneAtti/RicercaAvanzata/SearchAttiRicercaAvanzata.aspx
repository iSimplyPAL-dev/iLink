<%@ Page Language="vb" enableViewStateMac="false" AutoEventWireup="false" Codebehind="SearchAttiRicercaAvanzata.aspx.vb" Inherits="Provvedimenti.SearchAttiRicercaAvanzata" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<HTML>
	<HEAD>
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<script type="text/javascript">
	    function StampaDocumenti()
		{
			//GestAlert('a', 'warning', '', '', 'StampaDocumenti");
			if (parent.document.getElementById('ddlAnno').value =='-1')
			{
				GestAlert('a', 'warning', '', '', 'Per effettuare la stampa massiva dei Provvedimenti è necessario selezionare un anno ed effettuare la ricerca!');
				return false;
			}
			if (parent.document.getElementById('ddlTributo').value =='-1')
			{
				GestAlert('a', 'warning', '', '', 'Per effettuare la stampa massiva dei Provvedimenti è necessario selezionare un Tributo ed effettuare la ricerca!');
				return false;
			}			
			
			if (confirm('Si desidera continuare con la stampa massiva dei Provvedimenti?'))
			{
				myLeft = (screen.availWidth / 2) - 250;
				myTop = (screen.availHeight / 2) - 150;
				
				var TRIBUTO=parent.document.getElementById('ddlTributo').innerText;
				var CODTRIBUTO=parent.document.getElementById('ddlTributo').value;
				var ANNO=parent.document.getElementById('ddlAnno').value;
			
				window.open('StampaMassivaProvedimenti.aspx?CODTRIBUTO='+CODTRIBUTO+"&ANNO="+ANNO+"&TRIBUTO="+TRIBUTO, 'PopUpStampaMassivaPROVV', 'width=550, height=300, left='+myLeft+', top='+myTop);
			
			}
			else
			{
				return false;
			}			
	}
	</script>
	<BODY class="Sfondo" style="width:99%;margin:0;">
		<form id="Form1" runat="server" method="post">
			<asp:Button id="btnElaboraProvvedimenti" style="DISPLAY: none" runat="server"></asp:Button>
			<asp:Button id="btnElaboraBollettini" style="DISPLAY: none" runat="server"></asp:Button>

			<asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label>
			
			<INPUT id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza">
			<Grd:RibesGridView ID="GrdAtti" runat="server" BorderStyle="None" 
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
					<asp:BoundField DataField="DESCRIZIONE_ENTE" HeaderText="Ente"></asp:BoundField>
					<asp:BoundField DataField="ANNO" SortExpression="ANNO" HeaderText="Anno">
						<HeaderStyle HorizontalAlign="Right" Width="3%" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="NOMINATIVO" SortExpression="NOMINATIVO" HeaderText="Intestatario">
						<HeaderStyle HorizontalAlign="Left" Width="20%" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField headertext="Cod.Fiscale/P.IVA" sortexpression="CFPIVA">
						<HeaderStyle HorizontalAlign="Left" width="15%" verticalalign="Middle"></headerstyle>
						<itemstyle horizontalalign="Left" verticalalign="Middle"></itemstyle>
						<itemtemplate>
							<asp:label runat="server" text='<%# CFPIVA(DataBinder.Eval(Container, "DataItem.CODICE_FISCALE"),DataBinder.Eval(Container, "DataItem.PARTITA_IVA")) %>' id="Label5" name="Label3">
							</asp:label>
						</itemtemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="TRIBUTO" HeaderText="Provvedimento">
						<HeaderStyle HorizontalAlign="Left" Width="22%" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="NUMERO_ATTO" HeaderText="Numero Atto">
						<HeaderStyle HorizontalAlign="center" Width="10%" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="center" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Data Creazione">
						<HeaderStyle Wrap="False" HorizontalAlign="Center" Width="10%" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
						<ItemTemplate>
							<asp:Label id=Label1 runat="server" text='<%# GiraData(DataBinder.Eval(Container, "DataItem.DATA_ELABORAZIONE"))%>'>Label</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField datafield="IMP_TOT_RIDOTTO" headertext="Totale Rid. €" DataFormatString="{0:N}">
						<headerstyle horizontalalign="left" width="10%" verticalalign="Middle"></headerstyle>
						<itemstyle horizontalalign="Right" verticalalign="Middle"></itemstyle>
					</asp:BoundField>
					<asp:BoundField DataField="IMPORTO_TOTALE" HeaderText="Totale €" DataFormatString="{0:N}">
						<HeaderStyle HorizontalAlign="left" Width="10%" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
                    <asp:BoundField DataField="STATO" HeaderText="Stato Avviso"></asp:BoundField>
					<asp:TemplateField HeaderText="">
						<headerstyle horizontalalign="Center"></headerstyle>
						<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
						<itemtemplate>
						<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID_PROVVEDIMENTO") %>' alt=""></asp:ImageButton>
						    <asp:HiddenField runat="server" ID="hfID_PROVVEDIMENTO" Value='<%# Eval("ID_PROVVEDIMENTO") %>' />
                            <asp:HiddenField runat="server" ID="hfCOD_TIPO_PROVVEDIMENTO" Value='<%# Eval("COD_TIPO_PROVVEDIMENTO") %>' />
                            <asp:HiddenField runat="server" ID="hfCOD_TIPO_PROCEDIMENTO" Value='<%# Eval("COD_TIPO_PROCEDIMENTO") %>' />
                            <asp:HiddenField runat="server" ID="hfCOD_TRIBUTO" Value='<%# Eval("COD_TRIBUTO") %>' />
						</itemtemplate>
					</asp:TemplateField>
				</Columns>
				</Grd:RibesGridView>
			<br>
			<div id="Riepilogativi" style="DISPLAY: none">
				<table class="SFONDO_TABELLA_TOTALI" cellSpacing="0" cellPadding="5" width="100%" border="0">
					<tr>
						<td class="riga_menu" align="left"><asp:label id="lblContribuenti" runat="server" Width="248px">NUMERO TOTALE CONTRIBUENTI:</asp:label></td>
						<td class="riga_menu" align="right"><asp:label id="lblTotContribuenti" CssClass="riga_menu" Runat="server" Width="150px"></asp:label></td>
						<td class="riga_menu" align="right" width="30"></td>
					</tr>
					<tr>
						<TD class="riga_menu" align="left"><asp:label id="Label37" runat="server" Width="195px" Height="12px">NUMERO TOTALE AVVISI:</asp:label></TD>
						<TD class="riga_menu" align="right" height="10"><asp:label id="lblNumeroTotaleAvvisi" runat="server" CssClass="riga_menu" Height="12px" Width="150px"></asp:label></TD>
						<td class="riga_menu" align="right" width="30"></td>
					</tr>
					<TR>
						<TD class="riga_menu"><asp:label id="Label38" runat="server" Height="12px" width="240px">IMPORTO TOTALE AVVISI:</asp:label></TD>
						<TD class="riga_menu" align="right"><asp:label id="lblImportoTotaleAvvisi" runat="server" Height="12px" Width="150px"></asp:label></TD>
						<td class="riga_menu" align="right" width="30"></td>
					</TR>
					<TR>
						<TD class="riga_menu"><asp:label id="Label2" runat="server" Height="12px" width="360px">IMPORTO TOTALE AVVISI RIDOTTO:</asp:label></TD>
						<TD class="riga_menu" align="right"><asp:label id="lblImportoTotaleAvvisiRidotto" runat="server" Height="12px" Width="150px"></asp:label></TD>
						<td class="riga_menu" align="right" width="30"></td>
					</TR>
					<TR>
						<TD class="riga_menu" noWrap align="left"><asp:label id="Label39" runat="server" Height="12px"> IMPORTO TOTALE AVVISI AL NETTO DELLE RETTIFICHE E DEGLI ANNULLAMENTI:</asp:label></TD>
						<TD class="riga_menu" align="right"><asp:label id="lblRettifiche" runat="server" Width="150px" Height="12px"></asp:label></TD>
						<td class="riga_menu" align="right" width="30"></td>
					</TR>
					<TR>
						<TD class="riga_menu" noWrap align="left"><asp:label id="Label4" runat="server" Height="12px"> IMPORTO TOTALE AVVISI RIDOTTO AL NETTO DELLE RETTIFICHE E DEGLI ANNULLAMENTI:</asp:label></TD>
						<TD class="riga_menu" align="right"><asp:label id="lblRettificheRidotto" runat="server" Width="150px" Height="12px"></asp:label></TD>
						<td class="riga_menu" align="right" width="30"></td>
					</TR>
				</table>						
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
		</FORM>
	</BODY>
</HTML>
