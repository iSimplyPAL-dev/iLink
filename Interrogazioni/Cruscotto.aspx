<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Cruscotto.aspx.vb" Inherits="OPENgov.Cruscotto" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
    <script type="text/javascript" type="text/javascript">
        function Search() {
            if (document.getElementById('GrdResult') != null)
                document.getElementById('GrdResult').style.display = '';
            document.getElementById('DivAttesa').style.display = '';
            document.getElementById('btnRicerca').click();
        }

        function keyPress() {
            if (window.event.keyCode == 13) {
                Search();
            }
        }
    </script>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" align="right" style="width: 100%; height: 35px">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table2">
                <tr>
                    <td style="width: 464px; height: 20px" align="left">
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" width="800" colspan="2" rowspan="2">
                        <input class="Bottone BottoneExcel" runat="server" id="Excel" title="Stampa in formato Excel" onclick="document.getElementById('DivAttesa').style.display = ''; document.getElementById('btnStampaExcel').click();" type="button" name="Excel" />
                        <input class="Bottone BottoneRicerca" runat="server" id="Ricerca" title="Ricerca" onclick="Search();" type="button" name="Search" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 463px" align="left">
                        <span class="NormalBold_title" id="info" style="width: 400px; height: 20px">Cruscotto</span>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table id="TblRicerca" cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td>
                        <fieldset class="classeFiledSetRicerca">
                            <legend class="Legend">Inserimento filtri di Ricerca</legend>
                            <table id="TblParametri" cellspacing="1" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEnti" runat="server" CssClass="Input_Label">Ente</asp:Label><br />
                                        <asp:DropDownList ID="ddlEnti" runat="server" CssClass="Input_Text"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAnno" runat="server" CssClass="Input_Label">Anno</asp:Label><br />
                                        <asp:TextBox ID="txtAnno" runat="server" MaxLength="4" CssClass="Input_Text_Right OnlyNumber" Width="100px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTributo" runat="server" CssClass="Input_Label">Tributo</asp:Label><br />
                                        <asp:DropDownList ID="ddlTributo" runat="server" CssClass="Input_Text"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
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
                        <Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="20"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" 
                            OnPageIndexChanging="GrdPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="Anno" HeaderText="Anno"></asp:BoundField>
                                <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                                <asp:BoundField DataField="DescrTributo" HeaderText="Tributo"></asp:BoundField>
                                <asp:BoundField DataField="NUtenti" HeaderText="N.Utenti"></asp:BoundField>
                                <asp:BoundField DataField="NDoc" HeaderText="N.Documenti"></asp:BoundField>
                                <asp:BoundField DataField="ImpEmesso" HeaderText="Emesso" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                <asp:BoundField DataField="ImpIncassato" HeaderText="Incassato" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnStampaExcel" Style="display: none" runat="server" Text="Button"></asp:Button>
        <asp:Button ID="btnRicerca" Style="display: none" runat="server" Text="Button"></asp:Button>
    </form>
</body>
</html>
