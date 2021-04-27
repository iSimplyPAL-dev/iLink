<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VisualAnag.aspx.vb" Inherits="OPENgov.VisualAnag" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
	<link href="../../Styles.css" type="text/css" rel="stylesheet">
	<%        If Session("SOLA_LETTURA") = "1" Then%>
	<link href="../../solalettura.css" type="text/css" rel="stylesheet">
	<%end if%>
	<script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
    <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	<script type="text/javascript">
		function nascondi(chiamante, oggetto, label) {
		    if (document.getElementById(oggetto).style.display == "") {
		        document.getElementById(oggetto).style.display = "none"
		        chiamante.title = "Visualizza " + label
		        chiamante.innerText = "Visualizza " + label
		        parent.document.getElementById('ifrmAnag').style.height = "200px"
		    } else {
		        document.getElementById(oggetto).style.display = ""
		        chiamante.title = "Nascondi " + label
		        chiamante.innerText = "Nascondi " + label
		        parent.document.getElementById('ifrmAnag').style.height = "300px"
		    }
		}
		function ApriRicercaAnagrafe(nomeSessione) {
		    winWidth = 980
		    winHeight = 680
		    myleft = (screen.width - winWidth) / 2
		    mytop = (screen.height - winHeight) / 2 - 40
		    Parametri = "sessionName=" + nomeSessione
		    WinPopUpRicercaAnagrafica = window.open("../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?" + Parametri, "", "width=" + winWidth + ",height=" + winHeight + ", status=yes, toolbar=no,top=" + mytop + ",left=" + myleft + ",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no")
		    //LoadOpenWindow('../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?' + Parametri);
		}
		function ApriRicAnater() {
		    winWidth = 980
		    winHeight = 500
		    myleft = (screen.availWidth - winWidth) / 2
		    mytop = (screen.availheight - winHeight) / 2 - 40
		    var parametri = 'popup=1';
		    WinPopAnater = window.open('../../AnagraficaAnater/popAnagAnater.aspx?' + parametri, '', 'width=' + winWidth + ',height=' + winHeight + ',top=' + mytop + ',left=' + myleft + ' status=yes, toolbar=no,scrollbar=no, resizable=no')
		}
		function ClearDatiContrib() {
		    if (confirm('Si desidera eliminare il Contribuente?')) {
		        document.getElementById('txtCognome').value = '';
		        document.getElementById('txtNome').value = '';
		        document.getElementById('txtCodFiscale').value = '';
		        document.getElementById('txtPIVA').value = '';
		        document.getElementById('txtDataNasc').value = '';
		        document.getElementById('txtComNasc').value = '';
		        document.getElementById('txtProvNasc').value = '';
		        document.getElementById('rdbFemmina').checked = false;
		        document.getElementById('rdbMaschio').checked = false;
		        document.getElementById('rdbGiuridica').checked = false;
		        document.getElementById('txtViaRes').value = '';
		        document.getElementById('txtCapRes').value = '';
		        document.getElementById('txtComuneRes').value = '';
		        document.getElementById('txtProvRes').value = '';
		        document.getElementById('hfCodContribuente').value = '-1';
		        document.getElementById('hfIdDataAnagrafica').value = '-1';
		        parent.document.getElementById('hdIdContribuente').value = '-1';
            }
		    return false;
		}
	</script>
</head>
<body class="Sfondo" style="width:99%;">
    <form id="Form1" runat="server" method="post">
        <div id="divAnag" runat="server">
		    <table border="0" cellpadding="0" cellspacing="0" width="100%">
			    <tr>
				    <td>
					    <asp:label id="lblDatiContribuente" runat="server" CssClass="lstTabRow" Font-Bold="True">Dati Contribuente</asp:label>&nbsp;
						<div class="tooltip">
                            <asp:imagebutton id="LnkAnagTributi" runat="server" CssClass="BottoneSel BottoneLista" alt="" ToolTip="Ricerca Anagrafica da Tributi" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>
                            <span class="tooltiptext">Ricerca Anagrafica da Tributi</span>
                        </div>&nbsp;
						<div class="tooltip">
                            <asp:imagebutton id="LnkAnagAnater" runat="server" CssClass="BottoneSel BottoneLista" alt="" ToolTip="Ricerca Anagrafica da Anater" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>
                            <span class="tooltiptext">Ricerca Anagrafica da Anater</span>
                        </div>&nbsp;
						<div class="tooltip">
                            <asp:imagebutton id="LnkPulisciContr" runat="server" class="BottoneSel BottoneClear" alt="" ToolTip="Pulisci i campi Contribuente" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>
                            <span class="tooltiptext">Pulisci i campi Contribuente</span>
                        </div>
				    </td>
				    <td align="left" colspan="5">
				        <asp:label ID="lblRiemp" CssClass="lstTabRow" Runat="server" Width="500px">&nbsp;</asp:label>
						<asp:HiddenField id="hfCodContribuente" Runat="server" Value="-1"></asp:HiddenField>
						<asp:HiddenField id="hfIdDataAnagrafica" Runat="server" Value="-1"></asp:HiddenField>
					    <asp:button id="btnRibalta" style="display: none" runat="server"></asp:button>
					    <asp:button id="btnRibaltaAnagAnater" style="display: none" Runat="server"></asp:button>
				    </td>
			    </tr>
			    <tr>
				    <td>
					    <asp:label id="lblCodiceFiscale" runat="server" CssClass="Input_Label">Codice Fiscale</asp:label><br />
					    <asp:textbox id="txtCodFiscale" runat="server" CssClass="Input_Text" Width="185px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
				    <td>
					    <asp:label id="lblPartitaIVA" runat="server" CssClass="Input_Label">Partita IVA</asp:label><br />
					    <asp:textbox id="txtPIVA" runat="server" CssClass="Input_Text" Width="140px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
				    <td>&nbsp;
					    <asp:label id="lblSesso" runat="server" CssClass="Input_Label">Sesso</asp:label><br />
					    <asp:radiobutton id="rdbMaschio" runat="server" CssClass="Input_Label" Text="M"></asp:radiobutton>&nbsp;
					    <asp:radiobutton id="rdbFemmina" runat="server" CssClass="Input_Label" Text="F"></asp:radiobutton>&nbsp;
					    <asp:radiobutton id="rdbGiuridica" runat="server" CssClass="Input_Label" Text="G"></asp:radiobutton>
				    </td>
				    <td>
					    <asp:label id="lblDataNascita" runat="server" CssClass="Input_Label">Data Nascita</asp:label><br />
					    <asp:textbox id="txtDataNasc" runat="server" CssClass="Input_Text_Right TextDate" ReadOnly="True"></asp:textbox>
				    </td>
				    <td>
					    <asp:label id="lblComuneNascita" runat="server" CssClass="Input_Label">Comune Nascita</asp:label><br />
					    <asp:textbox id="txtComNasc" runat="server" CssClass="Input_Text" Width="300px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
				    <td>
					    <asp:label id="lblProv" runat="server" CssClass="Input_Label">Prov.</asp:label><br />
					    <asp:textbox id="txtProvNasc" runat="server" CssClass="Input_Text" Width="30px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
			    </tr>
			    <tr>
				    <td colspan="3">
					    <asp:label id="lblCognome" runat="server" CssClass="Input_Label">Cognome/Rag.Soc</asp:label><br />
					    <asp:textbox id="txtCognome" runat="server" CssClass="Input_Text" Width="400px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
				    <td colspan="3">
					    <asp:label id="lblNome" runat="server" CssClass="Input_Label">Nome</asp:label><br />
					    <asp:textbox id="txtNome" runat="server" CssClass="Input_Text" Width="300px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
			    </tr>
			    <tr>
				    <td colspan="3">
					    <asp:label id="lblVia" runat="server" CssClass="Input_Label">Indirizzo Residenza</asp:label><br />
					    <asp:textbox id="txtViaRes" runat="server" CssClass="Input_Text" Width="550px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
				    <td>
					    <asp:label id="lblCAP" runat="server" CssClass="Input_Label">CAP</asp:label><br />
					    <asp:textbox id="txtCapRes" runat="server" CssClass="Input_Text" Width="65px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
				    <td>
					    <asp:label id="lblComuneResidenza" runat="server" CssClass="Input_Label">Comune Residenza</asp:label><br />
					    <asp:textbox id="txtComuneRes" runat="server" CssClass="Input_Text" Width="300px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
				    <td>
					    <asp:label id="lblProvincia" runat="server" CssClass="Input_Label">Prov.</asp:label><br />
					    <asp:textbox id="txtProvRes" runat="server" CssClass="Input_Text" Width="30px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
			    </tr>
			    <tr style="display:none">
				    <td colspan="3">
					    <asp:label id="lblViaCO" runat="server" CssClass="Input_Label">Indirizzo Spedizione</asp:label><br />
					    <asp:textbox id="txtViaCO" runat="server" CssClass="Input_Text" Width="550px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
				    <td>
					    <asp:label id="lblCAPCO" runat="server" CssClass="Input_Label">CAP</asp:label><br />
					    <asp:textbox id="txtCapCO" runat="server" CssClass="Input_Text" Width="65px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
				    <td>
					    <asp:label id="lblComuneCO" runat="server" CssClass="Input_Label">Comune Spedizione</asp:label><br />
					    <asp:textbox id="txtComuneCO" runat="server" CssClass="Input_Text" Width="300px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
				    <td>
					    <asp:label id="lblProvinciaCO" runat="server" CssClass="Input_Label">Prov.</asp:label><br />
					    <asp:textbox id="txtProvCO" runat="server" CssClass="Input_Text" Width="30px" Text="" ReadOnly="True"></asp:textbox>
				    </td>
			    </tr>
		    </table>
        </div>
		<!--Blocco Dati Famiglia-->
	    <br />
        <a title="Visualizza Dati Famiglia/Residenti" onclick="nascondi(this,'divFamiglia','Dati Famiglia/Residenti')" href="#" class="lstTabRow" style="width:100%">Visualizza Dati Famiglia/Residenti</a>
		<asp:imagebutton id="ibNewRes" Runat="server" ImageUrl="../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px" ToolTip="Inserisci in Residenti" OnClientClick="return confirm('Si vuole inserire in Residenti?\nSara\' inserito solo se residente in comune e con un indirizzo valido.')"></asp:imagebutton>
	    <div id="divFamiglia" runat="server" style="width:100%; display:none">
	        <table width="100%">
			    <tr>
				    <td>
						<asp:Label CssClass="Legend" Runat="server" ID="LblResultFamiglia">Non sono presenti Dati per la Famiglia</asp:Label>
						<Grd:RibesGridView ID="GrdFamiglia" runat="server" BorderStyle="None" 
							BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
							AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
							ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
							<PagerSettings Position="Bottom"></PagerSettings>
							<PagerStyle CssClass="CartListFooter" />
							<RowStyle CssClass="CartListItem"></RowStyle>
							<HeaderStyle CssClass="CartListHead"></HeaderStyle>
							<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
								<asp:TemplateField HeaderText="Nominativo">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.COGNOME")&" "&DataBinder.Eval(Container, "DataItem.NOME") %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="cod_fiscale" HeaderText="Cod.Fiscale"></asp:BoundField>
								<asp:TemplateField HeaderText="Data Nascita">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblDataNascitaGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATA_NASCITA")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="LUOGO_NASCITA" HeaderText="Luogo Nascita"></asp:BoundField>
								<asp:TemplateField HeaderText="Data Morte">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblDatamorteGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATA_MORTE")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="VIA" HeaderText="Indirizzo"></asp:BoundField>
								<asp:BoundField DataField="DESCRIZIONE_POS" HeaderText="Parentela"></asp:BoundField>
								<asp:TemplateField HeaderText="Data Validità">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="LblDataMov" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.LASTMOV")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>
				    </td>
			    </tr>
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
    </form>
</body>
</html>
