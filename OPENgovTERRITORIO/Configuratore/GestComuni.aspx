<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestComuni.aspx.vb" Inherits="OpenGovTerritorio.GestComuni" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Gestione Comuni</title>
	<link href="../../Styles.css" type="text/css" rel="stylesheet" />
	<%        If Session("SOLA_LETTURA") = "1" Then%>
	<link href="../../solalettura.css" type="text/css" rel="stylesheet" />
	<%end if%>
	<script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
    <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
        function StampaElenco() {
            if (document.getElementById('GrdResult') == null) {
                GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Eseguire prima la ricerca.');
            }
            else {
                DivAttesa.style.display = '';
                document.getElementById('CmdStampa').click();
            }
            return false;
        }
        function NuovoComune() {
            document.getElementById('hdIdComune').value = '-1';
            $('#ViewAdd').show(); $('#SearchItem').hide();
        }
        function BackSearch() {
            document.getElementById('hdIdComune').value = '-1';
            $('#ViewAdd').hide(); $('#SearchItem').show();
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table1">
                <tr valign="top">
                    <td align="left">
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" rowspan="2">
                        <input class="Bottone BottoneExcel" id="Stampa" title="Stampa" onclick="StampaElenco();" type="button" name="Stampa">
                        <input class="Bottone BottoneNewInsert" id="Nuovo" title="Inserimento nuovo Comune" onclick="NuovoComune();" type="button" name="Nuovo">
                        <input class="Bottone BottoneRicerca" id="Ricerca" title="Ricerca" onclick="DivAttesa.style.display = ''; document.getElementById('CmdRicerca').click();" type="button" name="Ricerca">
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 400px">Configurazione - Comuni</span>
                    </td>
                </tr>
            </table>
        </div>
        &nbsp;
		<div id="SearchItem" class="col-md-12">
            <div class="col-md-12">
                <fieldset class="FiledSetRicerca">
                    <legend class="Legend">Inserimento filtri di ricerca</legend>
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <p>
                                <label class="Input_Label">Comune</label>
                            </p>
                            <asp:TextBox ID="TxtSearchComune" runat="server" CssClass="Input_Text col-md-11"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label class="Input_Label">PV</label>
                            </p>
                            <asp:TextBox ID="TxtSearchPV" runat="server" CssClass="Input_Text col-md-11"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label class="Input_Label">CAP</label>
                            </p>
                            <asp:TextBox ID="TxtSearchCAP" runat="server" CssClass="Input_Text col-md-11"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label class="Input_Label">Belfiore</label>
                            </p>
                            <asp:TextBox ID="TxtSearchBelfiore" runat="server" CssClass="Input_Text col-md-11"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label class="Input_Label">ISTAT</label>
                            </p>
                            <asp:TextBox runat="server" ID="TxtSearchIstat" CssClass="Input_Text col-md-11"></asp:TextBox>
                        </div>
                    </div>
                </fieldset>
            </div>
             <div class="col-md-12">
				<asp:label id="LblResult" Runat="server" CssClass="Legend">La ricerca non ha prodotto risultati.</asp:label>
                <Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None" 
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="20"
                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                    OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <PagerStyle CssClass="CartListFooter" />
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField DataField="Denominazione" HeaderText="Comune">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Provincia" HeaderText="PV">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Cap" HeaderText="CAP">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="CodBelfiore" HeaderText="Belfiore">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="CodIstat" HeaderText="ISTAT">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
            </div>
            <div id="DivAttesa" runat="server" style="z-index: 103; position: absolute;display:none;">
                <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                <div class="BottoneClessidra">&nbsp;</div>
                <div class="Legend">Attendere Prego...<asp:Label ID="LblAvanzamento" runat="server"></asp:Label></div>
            </div>
        </div>
        <div id="ViewAdd">
			<fieldset>
				<legend class="Input_Label_title">Inserimento/Modifica Comune</legend>
				<asp:HiddenField ID="hdIdComune" runat="server" Value="-1"/>
				<div class="col-md-12">
                    <div class="col-md-9"></div>
                    <div class="col-md-2">
        		        <div class="tooltip">
                            <img style="CURSOR: pointer" alt="" class="Bottone BottoneSalva" onclick="if (confirm('Si vogliono salvare le modifiche apportate?')){DivAttesa.style.display = '';document.getElementById('CmdSalva').click();}" />
                            <span class="tooltiptext">Salva</span>&nbsp; 
                        </div>
        		        <div class="tooltip">
                            <img style="CURSOR: pointer" alt="" class="Bottone BottoneAnnulla" onclick="BackSearch();" />
                            <span class="tooltiptext">Torna indietro</span>&nbsp; 
                        </div>
                    </div>
				</div>
                <div class="col-md-12">
                    <div class="col-md-6">
                        <p>
                            <label class="Input_Label">Comune</label>
                        </p>
                        <asp:TextBox ID="TxtComune" runat="server" CssClass="Input_Text col-md-11"></asp:TextBox>
                    </div>
                    <div class="col-md-1">
                        <p>
                            <label class="Input_Label">PV</label>
                        </p>
                        <asp:TextBox ID="TxtPV" runat="server" CssClass="Input_Text col-md-11"></asp:TextBox>
                    </div>
                    <div class="col-md-1">
                        <p>
                            <label class="Input_Label">CAP</label>
                        </p>
                        <asp:TextBox ID="TxtCAP" runat="server" CssClass="Input_Text col-md-11"></asp:TextBox>
                    </div>
                    <div class="col-md-1">
                        <p>
                            <label class="Input_Label">Belfiore</label>
                        </p>
                        <asp:TextBox ID="TxtBelfiore" runat="server" CssClass="Input_Text col-md-11"></asp:TextBox>
                    </div>
                    <div class="col-md-1">
                        <p>
                            <label class="Input_Label">ISTAT</label>
                        </p>
                        <asp:TextBox runat="server" ID="TxtIstat" CssClass="Input_Text col-md-11"></asp:TextBox>
                    </div>
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
        <asp:Button Style="display: none" ID="CmdRicerca" runat="server"></asp:Button>
        <asp:Button Style="display: none" ID="CmdNuovo" runat="server"></asp:Button>
        <asp:Button Style="display: none" ID="CmdStampa" runat="server"></asp:Button>
        <asp:Button Style="display: none" ID="CmdSalva" runat="server"></asp:Button>
        <asp:Button Style="display: none" ID="CmdIndietro" runat="server"></asp:Button>
    </form>
</body>
</html>
