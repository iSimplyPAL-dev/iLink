<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfAddizionali.aspx.vb" Inherits="OPENgovTIA.ConfAddizionali" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title></title>
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
        <script type="text/javascript">
            function CheckDati() {
                if (document.getElementById('txtAnno').value == '') {
                    GestAlert('a', 'warning', '', '', 'E\' necessario inserire un Anno!');
                    return false;
                }
            if (document.getElementById('ddlGestAddizionali').value == '')
                {
                    GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Capitolo!');
                    return false;
                }
                    if (document.getElementById('txtPercentuale').value == '')
                {
                    GestAlert('a', 'warning', '', '', 'E\' necessario inserire una Percentuale!');
                    return false;
                }
                    document.getElementById('CmdSalva').click();
            }
        </script>
    </head>
	<body class="Sfondo" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<div id="divComandi" class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: hidden">
				<div class="col-md-6">
					<span class="ContentHead_Title col-md-12" id="infoEnte">
						<asp:Label id="lblTitolo" runat="server"></asp:Label><br />
					</span>
					<span class="NormalBold_title col-md-12" id="Span1" runat="server" runat="server">
                        <asp:Label id="info" runat="server">- Configurazioni - Ricerca Addizionali</asp:Label>
                    </span>
				</div>
				<div class="col-md-5" align="right">
					<input class="Bottone BottoneCancella" id="Delete" title="Elimina Addizionale" onclick="document.getElementById('CmdElimina').click()" type="button" name="Erase"> 
					<input class="Bottone BottoneSalva" id="Insert" title="Salva Addizionale" onclick="CheckDati();" type="button" name="Insert"> 
					<input class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla Gestione" onclick="document.getElementById('CmdBack').click()" type="button" name="Delete">
					<input class="Bottone BottoneNewInsert" id="NewInsert" title="Inserisci" onclick="document.getElementById('CmdNew').click()" type="button" name="NewInsert"> 
					<input class="Bottone BottoneRicerca" id="Search" title="Ricerca" onclick="document.getElementById('CmdSearch').click()" type="button" name="Search">
				</div>
			</div>
			&nbsp;
			<div id="divRic" class="col-md-12">
				<fieldset class="FiledSetRicerca" style="WIDTH: 98%; HEIGHT: 79px">
                    <legend class="Legend">Inserimento filtri di ricerca</legend>
					<div class="col-md-6">
                        <asp:label id="Label1" runat="server" CssClass="Input_Label">Codice - Descrizione</asp:label><br />
						<asp:dropdownlist id="ddlRicAddizionali" runat="server" CssClass="Input_Text" Width="336px"></asp:dropdownlist>
					</div>
					<div class="col-md-2">
                        <asp:label id="Label5" runat="server" CssClass="Input_Label">Anno</asp:label><br />
						<asp:TextBox id="txtRicAnno" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox>
					</div>
				</fieldset>
				<div id="divResult" class="col-md-12">
                    <asp:label id="LblResult" CssClass="Legend" Runat="server">Risultati della Ricerca</asp:label>
			        <Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None" 
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
					        <asp:BoundField DataField="ANNO" HeaderText="Anno">
						        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
						        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					        </asp:BoundField>
					        <asp:BoundField DataField="DESCRIZIONE" HeaderText="Capitolo">
						        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					        </asp:BoundField>
					        <asp:BoundField DataField="VALORE" HeaderText="%" DataFormatString="{0:N}">
						        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
						        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					        </asp:BoundField>
					        <asp:TemplateField HeaderText="">
						        <headerstyle horizontalalign="Center"></headerstyle>
						        <ItemStyle horizontalalign="Center" Width="40px"></ItemStyle>
						        <itemtemplate>
							        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDADDIZIONALE") %>' alt=""></asp:ImageButton>
					                <asp:HiddenField runat="server" ID="hfIDCAPITOLO" Value='<%# Eval("CODICECAPITOLO") %>' />
                                    <asp:HiddenField runat="server" ID="hfIDADDIZIONALE" Value='<%# Eval("IDADDIZIONALE") %>' />
                                    <asp:HiddenField runat="server" ID="hfTipoCalcolo" Value='<%# Eval("tipocalcolo") %>' />
						        </itemtemplate>
					        </asp:TemplateField>
				        </Columns>
			        </Grd:RibesGridView>
				</div>
			</div>
			<div id="divGest" class="col-md-12">
			    <fieldset class="FiledSetRicerca">
				    <legend class="Legend">Dati Addizionale</legend>
					<div class="col-md-2">
						<asp:Label Runat="server" CssClass="Input_Label" id="Label3">Anno</asp:Label>
						<br />
						<asp:textbox id="txtAnno" onchange="ControllaAnno(this)" runat="server" CssClass="Input_Text_Right TextDate OnlyNumber" MaxLength="4" onblur="" onfocus=""></asp:textbox>
					</div>
					<div class="col-md-5">
						<asp:Label Runat="server" CssClass="Input_Label" id="Label2">Capitolo</asp:Label>
						<br />
						<asp:dropdownlist id="ddlGestAddizionali" runat="server" Width="250px" CssClass="Input_CheckBox"></asp:dropdownlist>
					</div>
					<div class="col-md-2">
						<asp:Label Runat="server" CssClass="Input_Label" id="Label4">Valore</asp:Label>
						<br />
						<asp:textbox id="txtPercentuale" runat="server" Width="128px" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value,-1,2,0.1,100)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo Valore!')}"></asp:textbox>
					</div>
					<div class="col-md-2">
						<asp:RadioButton Runat="server" CssClass="Input_Label" id="optPercentuale" GroupName="optTipoCalcolo" Checked="true" Text="Percentuale"></asp:RadioButton>
						<asp:RadioButton Runat="server" CssClass="Input_Label" id="optFissa" GroupName="optTipoCalcolo" Text="Importo Fisso"></asp:RadioButton>
					</div>
			    </fieldset>
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
            <asp:HiddenField ID="hfIdAddizionale" runat="server" />
            <asp:HiddenField ID="hfIdCapitolo" runat="server" />
			<asp:button id="CmdSearch" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="CmdSalva" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="CmdElimina" style="DISPLAY: none" runat="server"></asp:button>
            <asp:button id="CmdNew" style="DISPLAY: none" runat="server"></asp:button>
            <asp:button id="CmdBack" style="DISPLAY: none" runat="server"></asp:button>
		</form>
	</body>
</html>
