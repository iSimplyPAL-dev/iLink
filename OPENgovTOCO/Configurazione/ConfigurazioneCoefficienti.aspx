<%@ Page Language="c#" CodeBehind="ConfigurazioneCoefficienti.aspx.cs" AutoEventWireup="True" Inherits="OPENGovTOCO.Configurazione.ConfigurazioneCoefficienti" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>ConfigurazioneCoefficienti</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" src="../../_js/Toco.js?newversion"></script>
    <script>
        function confermaEliminazione() {
            if (confirm('Confermi l\'eliminazione?') == true)
                /** modificata l'azione se l'azione viene confermata **/
                /*__doPostBack();*/
                document.getElementById('btDel').click();
            else
                return false;
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden">
            <table id="Table1" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td><span style="WIDTH: 400px" id="infoEnte" class="ContentHead_Title"><asp:label id="lblTitolo" runat="server"></asp:label></span></td>
                    <td align="right" valign="middle" rowspan="2">
                        <asp:Button ID="btRibalta" runat="server" Cssclass="Bottone BottoneRibalta" OnClick="btRibalta_Click"></asp:Button>
                        <asp:Button ID="btNew" runat="server" Cssclass="Bottone BottoneNewInsert" OnClick="btNew_Click"></asp:Button>
                        <asp:Button ID="btSalva" runat="server" Cssclass="Bottone BottoneSalva" OnClick="btSalva_Click"></asp:Button>
                        <asp:Button ID="btDel" runat="server" Cssclass="Bottone BottoneCancella" OnClick="btDel_Click"></asp:Button>
                        <a class="Bottone BottoneAnnulla" title="Annulla" href="ConfigurazioneCoefficienti.aspx"></a>
                    </td>
                </tr>
                <tr>
                    <td colSpan="2"><span style="WIDTH: 400px; HEIGHT: 20px" id="info" class="NormalBold_title">TOSAP/COSAP - Configurazione - Coefficienti e Agevolazioni</span> </td>
                </tr>
            </table>
        </div>
        <div class="col-md-12">
            <fieldset style="width: 57%; height: 49px; float: left;" class="FiledSetRicerca">
                <legend class="Legend">Inserimento filtri di ricerca</legend>
                <table width="100%">
                    <tr>
                        <td style="width: 432px; height: 3px">
                            <asp:RadioButton ID="RBCategoria" TabIndex="1" runat="server" CssClass="Input_Label" Text="Categoria" TextAlign="Right" Checked="True" GroupName="RB" AutoPostBack="True" OnCheckedChanged="RBCategoria_CheckedChanged"></asp:RadioButton>&nbsp;&nbsp; 
                            <asp:RadioButton ID="RBAgevolazione" TabIndex="1" runat="server" CssClass="Input_Label" Text="Agevolazione" TextAlign="Right" GroupName="RB" AutoPostBack="True" OnCheckedChanged="RBAgevolazione_CheckedChanged"></asp:RadioButton>&nbsp;&nbsp; 
                            <asp:RadioButton ID="RBTipologiaOccupazioni" TabIndex="2" runat="server" CssClass="Input_Label" Text="Tipologia Occupazione" TextAlign="Right" GroupName="RB" AutoPostBack="True" OnCheckedChanged="RBTipologiaOccupazioni_CheckedChanged"></asp:RadioButton></td>
                    </tr>                    
                </table>                                
            </fieldset>
            <fieldset style="width: 37%; height: 49px; float: right;" class="FiledSetRicerca">
                <legend class="Legend">Ribalta</legend>
                <table>
                    <tr>
                        <td style="width:150px"><asp:Label ID="Da" runat="server" CssClass="Input_Label">Da:</asp:Label>&nbsp&nbsp<asp:TextBox ID="txtRibaltaDa" runat="server" CssClass="Input_Text" Width="50px" onBlur='isNumber(this,"","","",9999)'"></asp:TextBox></td>
                        <td><asp:Label ID="A" runat="server" CssClass="Input_Label">A:</asp:Label>&nbsp&nbsp<asp:TextBox ID="txtRibaltaA" runat="server" CssClass="Input_Text" Width="50px" onBlur='isNumber(this,"","","",9999)'></asp:TextBox></td>
                    </tr>
                </table>
            </fieldset>
            <br /><br />
            <asp:Label Style="font-family: verdana; font-size: 10px" ID="lblResultList" runat="server" Visible="False"></asp:Label><asp:Panel
                ID="pnlCoefficienti" runat="server">
                <Grd:RibesGridView ID="GrdCoefficienti" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="Descrizione" HeaderText="Descrizione"></asp:BoundField>
                        <asp:BoundField DataField="Anno" HeaderText="Anno"></asp:BoundField>
                        <asp:BoundField DataField="Coefficiente" HeaderText="Coefficiente" DataFormatString="{0:#0.00}"></asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdTabella") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hfIdTabella" Value='<%# Eval("IdTabella") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
            </asp:Panel>
            <asp:Panel ID="pnlInserisci" runat="server" Visible="False">
                <table border="0" cellpadding="0" width="100%">
                    <tr>
                        <td style="height: 16px">
                            <asp:Label ID="lblDescrizioneOperazioneInserisci" runat="server" CssClass="Legend" Width="100%">Dati Codice/Descrizione - </asp:Label>&nbsp; 
                        </td>
                    </tr>
                    <tr>
                        <td class="Input_Label">Tabella selezionata: 
                            <asp:Label ID="lblTabSelIns" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblIdTabIns" runat="server" CssClass="Input_Label"></asp:Label>
                            <asp:Label Style="font-family: Verdana; color: red; font-size: 11px" ID="Label6" runat="server">*</asp:Label><br>
                            <asp:DropDownList Style="z-index: 0" ID="ddlDescrizioniIns" runat="server"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAnnoIns" runat="server" CssClass="Input_Label">Anno</asp:Label>
                            <asp:Label Style="font-family: Verdana; color: red; font-size: 11px" ID="Label5" runat="server">*</asp:Label><br>
                            <asp:TextBox ID="txbAnnoInsert" onkeypress="return NumbersOnly(event, false, false, 6);" runat="server" Width="120" CssClass="Input_Text_Right OnlyNumber" MaxLength="4"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCoeffInsert" runat="server" CssClass="Input_Label">Coefficiente</asp:Label>
                            <asp:Label Style="font-family: Verdana; color: red; font-size: 11px" ID="Label4" runat="server">*</asp:Label><br>
                            <asp:TextBox ID="txbCoefficienteInsert" onkeypress="return NumbersOnly(event, true, false, 6);" runat="server" Width="120" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel
                ID="pnlModifica" runat="server" Visible="False">
                <table border="0" cellpadding="0" width="100%">
                    <tr>
                        <td style="height: 16px">
                            <asp:Label ID="lblDescrizioneOperazioneModifica" runat="server" CssClass="Legend" Width="100%">Dati Codice/Descrizione - </asp:Label>&nbsp; 
                            <asp:TextBox ID="txbIdRecordToMod" runat="server" Visible="False"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="Input_Label">Tabella selezionata: 
                            <asp:Label ID="lblTabSelModifica" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="Input_Label">Elemento selezionato: 
                            <asp:Label ID="lblElmSelModifica" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="Input_Label">Anno selezionato: 
                            <asp:Label ID="lblAnnoSelModifica" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCoefficienteMod" runat="server" CssClass="Input_Label">Coefficiente</asp:Label>
                            <asp:Label Style="font-family: Verdana; color: red; font-size: 11px" ID="Label3" runat="server">*</asp:Label><br>
                            <asp:TextBox ID="txbCoefficiente" onkeypress="return NumbersOnly(event, true, false, 6);" runat="server" Width="120" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox></td>
                    </tr>
                </table>
            </asp:Panel>
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
