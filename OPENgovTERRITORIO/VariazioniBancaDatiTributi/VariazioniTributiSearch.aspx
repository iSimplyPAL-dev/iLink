<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VariazioniTributiSearch.aspx.vb" Inherits="OpenGovTerritorio.VariazioniTributiSearch" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Variazioni tributi</title>
    <link href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css" rel="stylesheet" type="text/css" />
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
        <script type="text/javascript" type="text/javascript">
            function LoadSearch() {
                DivAttesa.style.display = '';
                document.getElementById('CmdSearch').click();
            }
		    function keyPress()
		    {
			    if(window.event.keyCode==13)
			    {
			        LoadSearch();
			    }
		    }
        </script>
</head>
<body style="OVERFLOW: hidden" bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0" MS_POSITIONING="GridLayout">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" style="OVERFLOW: hidden; WIDTH: 100%; HEIGHT: 45px">
		    <table style="WIDTH: 100%" border="0" cellpadding="0" cellspacing="0">
			    <tr valign="top">
				    <td><span style="WIDTH: 400px" id="infoEnte" class="ContentHead_Title"><asp:label id="lblTitolo" runat="server"></asp:label></span></td>
				    <td align="right" valign="middle" rowspan="2">
					    <input class="Bottone BottoneExcel" id="Print" title="Stampa" onclick="DivAttesa.style.display = ''; document.getElementById('CmdStampa').click()" type="button" name="Print">
                        <asp:imagebutton id="SetTrattato" runat="server" CssClass="Bottone BottoneOk hidden" ToolTip="Setta posizioni selezionate come trattate"></asp:imagebutton>
                        <input id="Search" runat="server" class="Bottone BottoneRicerca" ToolTip="Ricerca" onclick="LoadSearch()" />
					    <asp:Button ID="CmdSearch" runat="server" CssClass="hidden"/>
                        <asp:Button ID="CmdStampa" runat="server" CssClass="hidden"/>
				    </td>
			    </tr>
			    <tr>
				    <td colspan="2"><span style="WIDTH: 400px; HEIGHT: 20px" id="info" class="NormalBold_title">Gestione Variazioni tributarie</span></td>
			    </tr>
		    </table>
        </div>
		<div style="WIDTH: 100%; HEIGHT: 560px">
			<table id="TblRicerca" border="0" cellspacing="1" cellpadding="1" width="100%">
				<tr>
					<td>
					    <!--tipo di ricerca-->
						<fieldset class="FiledSetRicerca"><legend class="Legend">Inserimento filtri di ricerca</legend>
					        <asp:panel id="TblParametri" Runat="server" border="0" cellspacing="1" cellpadding="1" width="100%">
							    <table>
				                    <tr>
					                    <td class="Input_Label" colspan="7">
					                        <asp:radiobutton id="RbNonTrattato" runat="server" AutoPostBack="true" Checked="true" CssClass="Input_Label" Text="Non trattato" GroupName="TipoVar"></asp:radiobutton>
					                        <asp:radiobutton id="RbTrattato" runat="server" AutoPostBack="true" Checked="False" CssClass="Input_Label" Text="trattato" GroupName="TipoVar"></asp:radiobutton>
					                    </td>
				                    </tr>
				                    <tr>
					                    <td colspan="7">
										    <asp:Label id="Label6" CssClass="Input_Label" Runat="server">Tributo</asp:Label><br />
						                    <asp:dropdownlist id="DdlTributo" runat="server" CssClass="Input_Text"></asp:dropdownlist>
					                    </td>
				                    </tr>
								    <tr>
									    <td>
										    <asp:Label id="Label2" CssClass="Input_Label" Runat="server">Foglio</asp:Label><br />
										    <asp:textbox id="TxtFoglio" runat="server" CssClass="Input_Text" Width="50px"></asp:textbox>
										</td>
									    <td>
										    <asp:Label id="Label3" CssClass="Input_Label" Runat="server">Numero</asp:Label><br />
										    <asp:textbox id="TxtNumero" runat="server" CssClass="Input_Text" Width="50px"></asp:textbox>
										</td>
									    <td>
										    <asp:Label id="Label1" CssClass="Input_Label" Runat="server">Subalterno</asp:Label><br />
										    <asp:textbox id="TxtSub" runat="server" CssClass="Input_Text" Width="50px"></asp:textbox>
										</td>
									    <td>
										    <asp:Label Runat="server" CssClass="Input_Label" id="Label4">Dal</asp:Label><br />
										    <asp:TextBox id="TxtDal" Runat="server" CssClass="Input_Text" style="TEXT-ALIGN: right" Width="80px" maxlength="10"
											    onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
									    </td>
									    <td>
										    <asp:Label Runat="server" CssClass="Input_Label" id="Label5">Al</asp:Label><br />
										    <asp:TextBox id="TxtAl" Runat="server" CssClass="Input_Text" style="TEXT-ALIGN: right" Width="80px" maxlength="10"
											    onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
									    </td>
									    <td>
										    <asp:Label Runat="server" CssClass="Input_Label" id="Label7">Operatore</asp:Label><br />
										    <asp:DropDownList id="DdlOperatore" Runat="server" CssClass="Input_Label"></asp:DropDownList>
									    </td>
                                        <td>
                                            <asp:Label runat="server" CssClass="Input_Label">Causali</asp:Label><br />
                                            <asp:DropDownList ID="DdlCausali" runat="server" CssClass="Input_Label"></asp:DropDownList>
                                        </td>
								    </tr>
							    </table>
						    </asp:panel>
						</fieldset>
					</td>
				</tr>
			</table>
			<table width="100%">
				<tr>
					<td>
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
				<tr>
					<td>
						<asp:label id="LblResultdichiarazioni" CssClass="Legend" Runat="server" style="display:none">Non sono presenti dichiarazioni</asp:label>
                        <Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="15"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
								<asp:BoundField DataField="Foglio" HeaderText="Foglio"></asp:BoundField>
								<asp:BoundField DataField="Numero" HeaderText="Numero"></asp:BoundField>
								<asp:BoundField DataField="Subalterno" HeaderText="Sub"></asp:BoundField>
								<asp:BoundField DataField="Tributo" HeaderText="Tributo"></asp:BoundField>
								<asp:BoundField DataField="Causale" HeaderText="Causale"></asp:BoundField>
								<asp:BoundField DataField="DataVariazione_V" HeaderText="Data Variazione"></asp:BoundField>
								<asp:BoundField DataField="Operatore" HeaderText="Operatore"></asp:BoundField>
								<asp:TemplateField HeaderText="Sel">
									<ItemTemplate>
										<asp:CheckBox id="ckbSelezione" runat="server"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
                                        <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("ID") %>' />
                                        <asp:HiddenField ID="hfIdUI" runat="server" Value='<%# Eval("IdRifUI") %>' />
                                        <asp:HiddenField ID="hfIdClass" runat="server" Value='<%# Eval("IdRifClass") %>' />
                                        <asp:HiddenField ID="hfIdVar" runat="server" Value='<%# Eval("TipoVariazione") %>' />
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
