<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FrameSubContatore.aspx.vb" Inherits="OpenUtenze.FrameSubContatore" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Associa Sub Contatore</title>
    <meta name="vs_showGrid" content="False">
    <link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
    <meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <%--<script type="text/javascript" src="../../_js/MyUtility.js?newversion"></script>--%>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" src="../../_js/NumbersOnly.js?newversion"></script>
    <script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
    <script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
    <script type="text/javascript">
        function Search() {
            document.getElementById('btnSearch').click();
            //Parametri="?matricola="+document.getElementById('txtMatricola.value+"&contatoreprincipale="+document.getElementById('ContNascosto.value+"&nutente="+document.getElementById('txtNumeroUtente.value+"&via="+document.getElementById('TxtVia.value+"&intestatario="+document.getElementById('txtIntestatario.value+"&utente="+document.getElementById('txtUtente.value;
            //loadGridSub.location.href="SearchResultsSubContatori.aspx"+Parametri;
            return true;
        }

        function Associa() {
            if (confirm('Associare il contatore selezionato?')) {
                document.getElementById('btnAssocia').click()
                /*window.opener.parent.frames.item("visualizza").formRicercaAnagrafica.txtSubAssociato.value=document.getElementById('Associato.value;
                window.opener.parent.frames.item("visualizza").formRicercaAnagrafica.txtMatricolaSubAssociato.value=document.getElementById('AssociatoMatricola.value;
                window.close();*/
            }
        }

        function AssegnaFuoco() {
            document.getElementById('txtIntestatario').focus();
        }
    </script>
</head>
<body class="Sfondo" onload="AssegnaFuoco();" bottommargin="0" leftmargin="0" rightmargin="0"
    topmargin="0" marginwidth="0" marginheight="0">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" align="right">
            <input class="Bottone BottoneRicerca" onclick="Search();" type="button">
            <input class="Bottone BottoneAnnulla" onclick="window.close();" type="button">
        </div>
        <table border="0" cellspacing="1" cellpadding="1" width="98%" align="center">
            <tr>
                <td>
                    <fieldset style="width: 100%" class="FiledSetRicerca">
                        <legend class="Legend">Inserimento filtri di ricerca</legend>
                        <table border="0" cellspacing="1" cellpadding="1" width="98%" align="center">
                            <tr>
                                <td class="Input_Label_Bold" colspan="2">Intestatario</td>
                                <td class="Input_Label_Bold" colspan="2">Utente</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtIntestatario" runat="server" CssClass="Input_Text" MaxLength="100" Width="250px"></asp:TextBox></td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtUtente" runat="server" CssClass="Input_Text" MaxLength="100" Width="250px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="Input_Label_Bold">Matricola</td>
                                <td class="Input_Label_Bold">N. Utente</td>
                                <td class="Input_Label_Bold" colspan="2">Ubicazione
										<asp:ImageButton ID="Imagebutton1" runat="server" Height="1" Width="1" ToolTip="ciao"></asp:ImageButton>
                                    <asp:ImageButton ID="LnkOpenStradario" runat="server" ImageUrl="../../Images/Bottoni/Listasel.png" ImageAlign="Bottom" CausesValidation="false" ToolTip="Ubicazione Immobile da Stradario."></asp:ImageButton>&nbsp;
										<asp:ImageButton ID="LnkPulisciStrada" runat="server" ImageUrl="../../Images/Bottoni/cancel.png" ImageAlign="Bottom" CausesValidation="false" ToolTip="Pulisci i campi della Via"></asp:ImageButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtMatricola" runat="server" CssClass="Input_Text" MaxLength="20" Width="130"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="txtNumeroUtente" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}" MaxLength="18" Width="171px"></asp:TextBox></td>
                                <td colspan="2">
                                    <asp:TextBox ID="TxtVia" CssClass="Input_Text" Width="248px" ReadOnly="True" runat="server"></asp:TextBox>&nbsp;
										<asp:TextBox Style="display: none" ID="TxtCodVia" runat="server">-1</asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
        <br>
        <table border="0" cellspacing="0" cellpadding="1" width="100%" align="center">
            <tr>
                <td class="NormalBold" colspan="3">Visualizzazione Contatori Estratti</td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblMessage" runat="server" CssClass="NormalBold"></asp:Label>
                    <Grd:RibesGridView ID="GrdContatori" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowCommand="GrdRowCommand">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="MATRICOLA" HeaderText="Matricola"></asp:BoundField>
                            <asp:BoundField DataField="COGNOME_INT" HeaderText="Cognome"></asp:BoundField>
                            <asp:BoundField DataField="NOME_INT" HeaderText="Nome"></asp:BoundField>
                            <asp:BoundField DataField="PERIODO" HeaderText="Periodo"></asp:BoundField>
                            <asp:TemplateField HeaderText="Ubicazione">
                                <ItemTemplate>
                                    <asp:Label ID="LblUbicazione" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.via_ubicazione") + " " + DataBinder.Eval(Container, "DataItem.civico_ubicazione") %>'>Label</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneAssociaGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODCONTATORE") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfIdContatore" Value='<%# Eval("CODCONTATORE") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
        </table>
        <asp:TextBox Style="display: none" ID="ContNascosto" runat="server">-1</asp:TextBox>
        <asp:Button Style="display: none" ID="btnSearch" runat="server" Text="Button"></asp:Button>
        <asp:Button ID="btnAssocia" Style="display: none" runat="server" Text="Button"></asp:Button>
    </form>
</body>
</html>
