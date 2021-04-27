<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Scaglioni.aspx.vb" Inherits="OpenUtenze.Scaglioni" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Scaglioni</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
        function Ricerca() {
            document.getElementById("DivAttesa").style.display = "";
            document.getElementById("btnRicerca").click()
        }
        function Excel() {
            document.getElementById("btnExcel").click()
        }
        function Pulisci() {
            document.getElementById("btnPulisci").click()
        }

        function viewhideButton(value) {

            if (value == 'P') {
                parent.Comandi.document.getElementById("Modifica").style.display = ''
                parent.Comandi.document.getElementById("Excel").style.display = ''
                parent.Comandi.document.getElementById("Ricerca").style.display = ''
                parent.Comandi.document.getElementById("Pulisci").style.display = ''

                parent.Comandi.document.getElementById("InserisciAS").style.display = 'none'
                parent.Comandi.document.getElementById("SalvaAS").style.display = 'none'
                parent.Comandi.document.getElementById("AnnullaAS").style.display = 'none'

                parent.Comandi.document.getElementById("info").innerText = "Acquedotto - Analisi Scaglioni"
            } else {
                parent.Comandi.document.getElementById("Modifica").style.display = 'none'
                parent.Comandi.document.getElementById("Excel").style.display = 'none'
                parent.Comandi.document.getElementById("Ricerca").style.display = 'none'
                parent.Comandi.document.getElementById("Pulisci").style.display = 'none'

                parent.Comandi.document.getElementById("InserisciAS").style.display = ''
                parent.Comandi.document.getElementById("SalvaAS").style.display = ''
                parent.Comandi.document.getElementById("AnnullaAS").style.display = ''

                parent.Comandi.document.getElementById("info").innerText = "Acquedotto - Modifica Scaglioni"
            }

        }

        function Modifica() {
            viewhideButton("S")
            document.getElementById("btnModificaScaglioni").click()
        }

        function Salva() {
            viewhideButton("P")
            document.getElementById("btnSalvaScaglioni").click()
        }

        function Annulla() {
            viewhideButton("P")
            document.getElementById("btnAnnullaModifica").click()
        }

        function Inserisci() {
            document.getElementById("btnInserisceScaglione").click()
        }

        function VievMod(valore) {
            document.getElementById("divModScaglioni").style.display = valore
        }

        function SalvaModS() {
            var msgErrorObb = new String()
            var msgErrorNum = new String()
            var msg = new String()
            msg = ""
            msgErrorObb = ""
            msgErrorNum = ""
            if (document.getElementById("txtDA").value == "") {
                msgErrorObb += "Campo Da\n"
            } else if (isNaN(document.getElementById("txtDA").value)) {
                msgErrorNum += "campo Da\n"
            }
            if (document.getElementById("txtA").value == "") {
                msgErrorObb += "Campo A\n"
            } else if (isNaN(document.getElementById("txtA").value)) {
                msgErrorNum += "campo A\n"
            }
            if (msgErrorObb != "") {
                msg += "I seguenti campi sono obbligatori\n" + msgErrorObb + "\n"
            }
            if (msgErrorNum != "") {
                msg += "I seguenti campi devono essere dei numeri\n" + msgErrorNum + "\n"
            }
            if (msg != "")
                alert(msg)
            else
                document.getElementById("btnSalvaModS").click()
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <p>
            <br />
            <asp:Label ID="Label27" runat="server" CssClass="lstTabRow">Consumo per Scaglioni per il Periodo</asp:Label>&nbsp; &nbsp; &nbsp; 
		    <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="Input_Text" Width="100px"></asp:DropDownList>
        </p>
        <asp:Label ID="lblError" runat="server" CssClass="ErrorText NormalBold" Visible="False"></asp:Label><br>
        <Grd:RibesGridView ID="GrdScaglioni" runat="server" BorderStyle="None"
            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
            <PagerSettings Position="Bottom"></PagerSettings>
            <PagerStyle CssClass="CartListFooter" />
            <RowStyle CssClass="CartListItem"></RowStyle>
            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
            <Columns>
                <asp:BoundField DataField="sDescrizioneTU" HeaderText="Tipo Utenza">
                    <HeaderStyle HorizontalAlign="Left" Width="200px" VerticalAlign="Middle"></HeaderStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Scaglione">
                    <HeaderStyle HorizontalAlign="Center" Width="200px" VerticalAlign="Middle"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Da") & " - " & DataBinder.Eval(Container, "DataItem.a") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Consumo">
                    <HeaderStyle HorizontalAlign="Center" Width="150px" VerticalAlign="Middle"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="tDataApprovazioneDOC" runat="server" Text='<%# FormatNumber (DataBinder.Eval(Container, "DataItem.dQuantita"),2)%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="N.Utenze">
                    <HeaderStyle HorizontalAlign="Center" Width="150px" VerticalAlign="Middle"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# FormatNumber (DataBinder.Eval(Container, "DataItem.nUtenze"),0)%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Grd:RibesGridView>
        <Grd:RibesGridView ID="GrdAddScaglioni" runat="server" BorderStyle="None"
            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
            OnRowCommand="GrdRowCommand">
            <PagerSettings Position="Bottom"></PagerSettings>
            <PagerStyle CssClass="CartListFooter" />
            <RowStyle CssClass="CartListItem"></RowStyle>
            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="Tipo Utenza">
                    <HeaderStyle Width="180px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="sDescrizioneTU" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.sDescrizioneTU")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Da">
                    <HeaderStyle Width="100px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDa" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Da")%>' Width="50px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="A">
                    <HeaderStyle Width="100px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblA" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.A")%>' Width="50px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mod">
                    <HeaderStyle Width="40px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton ID="imgModifica" runat="server" CommandName="RowEdit" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' Width="28px" Height="26px" ImageUrl="..\..\images\Bottoni\modifica.png" title="Modifica i valori degli Scaglioni"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Del">
                    <HeaderStyle Width="40px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton ID="imgDelete" runat="server" CommandName="RowDelete" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' Width="28px" Height="26px" ImageUrl="..\..\images\Bottoni\cestino.png" title="Elimina lo Scaglione"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Grd:RibesGridView>
        <div id="divModScaglioni" runat="server" style="display: none" class="col-md-12">
            <div id="ModScaglioniCmd">
                <div class="Legend">
                    <asp:Label ID="Label7" runat="server">Modifica/Aggiungi Valori</asp:Label>
                </div>
                <div class="fr">
                    <input id="iSalvaModS" class="Bottone BottoneSalva" title="Salva Scaglione" onclick="SalvaModS();" type="button" name="iSalvaModS">
                    <input id="AnnullaModS" class="Bottone BottoneChiudi" title="Annulla le modifiche" onclick="VievMod('none');" type="button" name="AnnullaModS">
                </div>
            </div>
            <div id="ModScaglioniVis" class="col-md-12">
                <asp:TextBox ID="txtRiga" runat="server" Style="display: none"></asp:TextBox>
                <asp:Label ID="Label4" runat="server" Width="70px" CssClass="Input_Label">Tipo Utenza</asp:Label>
                <asp:DropDownList ID="ddlDescrizioneTU" runat="server" CssClass="Input_Text"></asp:DropDownList><br>
                <asp:Label ID="Label5" runat="server" Width="70px" CssClass="Input_Label">Da</asp:Label>
                <asp:TextBox ID="txtDA" runat="server" Width="50px" CssClass="Input_Text_Numbers OnlyNumber"></asp:TextBox><br>
                <asp:Label ID="Label6" runat="server" Width="70px" CssClass="Input_Label">A</asp:Label>
                <asp:TextBox ID="txtA" runat="server" Width="50px" CssClass="Input_Text_Numbers OnlyNumber"></asp:TextBox><br>
            </div>
        </div>
        <div id="DivAttesa" runat="server" style="z-index: 103; position: absolute;display:none;">
            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
            <div class="BottoneClessidra">&nbsp;</div>
            <div class="Legend">Attendere Prego...<asp:Label ID="LblAvanzamento" runat="server"></asp:Label></div>
        </div>
        <asp:Button Style="display: none" ID="btnRicerca" runat="server" Text="Ricerca"></asp:Button>
        <asp:Button Style="display: none" ID="btnPulisci" runat="server" Text="Pulisci"></asp:Button>
        <asp:Button Style="display: none" ID="btnExcel" runat="server" Text="Excel"></asp:Button>
        <asp:Button Style="display: none" ID="btnModificaScaglioni" runat="server" Text="Modifica Scaglioni"></asp:Button>
        <asp:Button Style="display: none" ID="btnSalvaScaglioni" runat="server" Text="Salva Scaglioni"></asp:Button>
        <asp:Button Style="display: none" ID="btnInserisceScaglione" runat="server" Text="Inserisce Scaglione"></asp:Button>
        <asp:Button Style="display: none" ID="btnAnnullaModifica" runat="server" Text="Annulla modifica Scaglioni"></asp:Button>
        <asp:Button Style="display: none" ID="btnSalvaModS" runat="server" Text="Salva valori modificati"></asp:Button>
    </form>
</body>
</html>
