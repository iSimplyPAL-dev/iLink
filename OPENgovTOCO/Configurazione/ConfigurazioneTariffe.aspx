<%@ Page Language="c#" CodeBehind="ConfigurazioneTariffe.aspx.cs" AutoEventWireup="True" Inherits="OPENGovTOCO.Configurazione.ConfigurazioneTariffe" EnableEventValidation="false" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>ConfigurazioneTariffe</title>
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
                //__doPostBack();
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
                    <td rowspan="2" width="800" colspan="2" align="right">
                        <asp:Button ID="btRibalta" runat="server" Cssclass="Bottone BottoneRibalta" OnClick="btRibalta_Click"></asp:Button>
                        <asp:Button ID="btNew" runat="server" Cssclass="Bottone BottoneNewInsert" OnClick="btNew_Click"></asp:Button>
                        <asp:Button ID="btSalva" runat="server" Cssclass="Bottone BottoneSalva" OnClick="btSalva_Click"></asp:Button>
                        <asp:Button ID="btDel" runat="server" Cssclass="Bottone BottoneCancella" OnClick="btDel_Click"></asp:Button>
                        <asp:ImageButton ID="btSearch" runat="server" ToolTip="Ricerca Tariffe" Cssclass="Bottone BottoneRicerca" ImageUrl="../../images/Bottoni/transparent28x28.png" Height="24px" OnClick="btSearch_Click"></asp:ImageButton>
                        <a class="Bottone BottoneAnnulla" title="Annulla" href="ConfigurazioneTariffe.aspx"></a>
                    </td>
                </tr>
                <tr>
                    <td colSpan="2"><span style="WIDTH: 400px; HEIGHT: 20px" id="info" class="NormalBold_title">TOSAP/COSAP - Configurazione - Tariffe</span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-md-12">
            <fieldset style="width:65%; float: left;" class="FiledSetRicerca">
                <legend class="Legend">Inserimento filtri di ricerca</legend>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label Style="z-index: 0" ID="lblCategorie" runat="server" CssClass="Input_Label">Seleziona Categoria</asp:Label><br>
                            <asp:DropDownList Style="z-index: 0" ID="ddlCategorie" runat="server" CssClass="Input_Text"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label Style="z-index: 0" ID="lblDurata" runat="server" CssClass="Input_Label">Seleziona Durata</asp:Label>
                            <br>
                            <asp:DropDownList Style="z-index: 0" ID="ddlDurata" runat="server" CssClass="Input_Text"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label Style="z-index: 0" ID="lblTributo" runat="server" CssClass="Input_Label">Tributo:</asp:Label>
                            <br>
                            <asp:DropDownList Style="z-index: 0" ID="ddlTributo" runat="server" CssClass="Input_Text"></asp:DropDownList>
                        </td>                        
						<td>
                            <asp:Label Style="z-index: 0" ID="lblAnno" runat="server" CssClass="Input_Label">Anno:</asp:Label>
                            <br>
                            <asp:TextBox Style="z-index: 0" ID="txbAnno" runat="server" onkeypress="return NumbersOnly(event, true, false, 6);" MaxLength="4" CssClass="Input_Text_Right OnlyNumber" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label Style="z-index: 0" ID="lblTipologieOccupazione" runat="server" CssClass="Input_Label">Seleziona Tipologia occupazione</asp:Label><br>
                            <asp:DropDownList Style="z-index: 0" ID="ddlTipologieOccupazione" runat="server" CssClass="Input_Text"></asp:DropDownList>
                        </td>                                           
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width:25%;float: right;" class="FiledSetRicerca">
                <legend class="Legend">Ribalta</legend>
                <table>
                    <tr>
                        <td style="width:150px"><asp:Label ID="Da" runat="server" CssClass="Input_Label">Da:</asp:Label>&nbsp&nbsp<asp:TextBox ID="txtRibaltaDa" runat="server" CssClass="Input_Text" Width="50px" onBlur='isNumber(this,"","","",9999)'></asp:TextBox></td>
                        <td><asp:Label ID="A" runat="server" CssClass="Input_Label">A:</asp:Label>&nbsp&nbsp<asp:TextBox ID="txtRibaltaA" runat="server" CssClass="Input_Text" Width="50px" onBlur='isNumber(this,"","","",9999)'></asp:TextBox></td>
                    </tr>
                </table>
            </fieldset>
            <br /><br />
            <asp:Label Style="font-family: verdana; font-size: 10px" ID="lblResultList" Visible="False" runat="server"></asp:Label>
            <asp:Panel ID="pnlCoefficienti" runat="server" Visible="true">
                <Grd:RibesGridView ID="GrdTariffe" runat="server" BorderStyle="None"
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
                        <asp:BoundField DataField="DescrTributo" HeaderText="Tributo"></asp:BoundField>
                        <asp:BoundField DataField="Categoria" HeaderText="Categoria"></asp:BoundField>
                        <asp:BoundField DataField="TipologiaOccupazione" HeaderText="Tipologia Occupazione"></asp:BoundField>
                        <asp:BoundField DataField="Durata" HeaderText="Durata"></asp:BoundField>
                        <asp:BoundField DataField="Anno" HeaderText="Anno"></asp:BoundField>
                        <asp:BoundField DataField="Valore" HeaderText="Valore"></asp:BoundField>
                        <asp:BoundField DataField="MinimoApplicabile" HeaderText="MinimoApplicabile"></asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdTariffa") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField runat="server" ID="hfIdTariffa" Value='<%# Eval("IdTariffa") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
            </asp:Panel>
            <asp:TextBox ID="txbIdRecordToMod" Visible="False" runat="server"></asp:TextBox>
            <asp:Panel ID="pnlInserisci" Visible="False" runat="server">
                <table border="0" cellpadding="0" width="100%" Class="Input_Label">
                    <tr>
                        <td>
                            <asp:Label ID="lblTributoIns" CssClass="Input_Label" runat="server">Tributo:</asp:Label><br>
                            <asp:DropDownList Style="z-index: 0" ID="ddlTributoIns" runat="server" CssClass="Input_Text"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCategoriaIns" CssClass="Input_Label" runat="server">Categoria:</asp:Label><br>
                            <asp:DropDownList Style="z-index: 0" ID="ddlCategoriaIns" runat="server" CssClass="Input_Text"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTipologiaOccupazioneIns" CssClass="Input_Label" runat="server">Tipologia Occupazione:</asp:Label><br>
                            <asp:DropDownList Style="z-index: 0" ID="ddlTipologiaOccupazioneIns" runat="server" CssClass="Input_Text"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDurataIns" CssClass="Input_Label" runat="server">Durata:</asp:Label><br>
                            <asp:DropDownList Style="z-index: 0" ID="ddlDurataIns" runat="server" CssClass="Input_Text"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAnnoIns" CssClass="Input_Label" runat="server">Anno:</asp:Label><br>
                            <asp:TextBox ID="txbAnnoIns" onkeypress="return NumbersOnly(event, false, false, 0);" MaxLength="4" runat="server" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblValoreIns" CssClass="Input_Label" runat="server">Valore:</asp:Label><br>
                            <asp:TextBox ID="txbValoreIns" onkeypress="return NumbersOnly(event, true, false, 6);" runat="server" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMinimoApplicabileIns" CssClass="Input_Label" runat="server">Minimo applicabile:</asp:Label><br>
                            <asp:TextBox ID="txbMinimoApplicabileIns" onkeypress="return NumbersOnly(event, true, false, 2);" runat="server" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlModifica" Visible="false" runat="server">
                <table border="0" cellpadding="0" width="100%" height="15%" Class="Input_Label">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server">Tributo:</asp:Label><br>
                            <asp:Label ID="lblTributoEdit" runat="server" CssClass="Input_Label"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" CssClass="Input_Label" runat="server">Categoria:</asp:Label><br>
                            <asp:Label ID="lblCategoriaEdit" runat="server" CssClass="Input_Label"></asp:Label></td>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" CssClass="Input_Label" runat="server">Tipologia Occupazione:</asp:Label><br>
                                <asp:Label ID="lblTipologiaOccupazioneEdit" runat="server" CssClass="Input_Label"></asp:Label></td>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" CssClass="Input_Label" runat="server">Durata:</asp:Label><br>
                                    <asp:Label ID="lblDurataEdit" runat="server" CssClass="Input_Label"></asp:Label></td>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" CssClass="Input_Label" runat="server">Anno:</asp:Label><br>
                                        <asp:Label ID="lblAnnoEdit" runat="server" CssClass="Input_Label"></asp:Label></td>
                                </tr>
                    <tr>
                        <td>
                            <asp:Label Style="z-index: 0" ID="lblValoreEdit" CssClass="Input_Label" runat="server">Valore:</asp:Label><br>
                            <asp:TextBox ID="txbValoreEdit" onkeypress="return NumbersOnly(event, true, false, 6);" runat="server" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Style="z-index: 0" ID="lblMinimoApplicabileEdit" CssClass="Input_Label" runat="server">Minimo applicabile:</asp:Label><br>
                            <asp:TextBox ID="txbMinimoApplicabileEdit" onkeypress="return NumbersOnly(event, true, false, 2);" runat="server" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox></td>
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
