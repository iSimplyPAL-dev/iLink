<%@ Page Language="c#" CodeBehind="ConfigurazioneRate.aspx.cs" AutoEventWireup="True" Inherits="OPENgovTOCO.ConfigurazioneRate" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Configurazione Rate</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" src="../../_js/Toco.js?newversion"></script>
</head>
<body  ms_positioning="GridLayout" class="Sfondo">
    <form id="Form1" runat="server" method="post" class="FormNoComandi">
        <div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: auto">
            <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
                <tr valign="top">
                    <td><span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                        <asp:Label ID="lblTitolo" runat="server"></asp:Label></span></td>
                    <td valign="middle" align="right" rowspan="2">
                        <asp:ImageButton ID="btnAggiungiRata" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png"
                            Cssclass="Bottone BottoneMoreMoney" ToolTip="Aggiungi Rata" OnClick="btnAggiungiRata_Click"></asp:ImageButton>
                        <asp:ImageButton ID="btnSalvaRate" runat="server" ImageUrl="../../images/Bottoni/transparent28x28.png"
                            Cssclass="Bottone BottoneSalva" ToolTip="Salva Configurazione Rate" OnClick="btnSalvaRate_Click"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><span id="info" runat="server" class="NormalBold_title" style="width: 400px; height: 20px">- Elaborazione - Configurazione Rate</span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-md-12">
            <table id="TblRicerca" cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td>
                        <div id="divRate" runat="server">
                            <fieldset class="classeFieldSetRicerca">
                                <legend class="Legend">Parametri di configurazione</legend>
                                <table id="TblParametri" cellspacing="1" cellpadding="1" width="100%" border="0">
                                    <tr>
                                        <td class="Input_Label">
                                            <asp:Label ID="lblAnno" CssClass="Input_Label" runat="server">Anno</asp:Label><br>
                                            <asp:DropDownList ID="ddlAnno" runat="server" CssClass="Input_Text" AutoPostBack="true" OnSelectedIndexChanged="ddlAnno_SelectedIndexChanged"></asp:DropDownList></td>
                                        <td>
                                            <asp:RadioButton ID="OptOrdinario" runat="server" GroupName="TipoRuolo" Text="Ordinario" CssClass="Input_Label"
                                                AutoPostBack="True" OnCheckedChanged="ddlAnno_SelectedIndexChanged"></asp:RadioButton>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="OptSuppletivo" runat="server" GroupName="TipoRuolo" Text="Suppletivo" CssClass="Input_Label"
                                                AutoPostBack="True"></asp:RadioButton>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </td>
                </tr>
            </table>
            <br>
            <br>
            <div id="divRateConfig" runat="server">
                <fieldset class="classeFieldSetRicerca">
                    <legend class="Legend">Rate</legend>
                    <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                        <tr>
                            <td>
                                <Grd:RibesGridView ID="GrdRate" runat="server" BorderStyle="None"
                                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                    AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                    OnRowCommand="GrdRowCommand" OnRowDataBound="GrdRowDataBound">
                                    <PagerSettings Position="Bottom"></PagerSettings>
                                    <PagerStyle CssClass="CartListFooter" />
                                    <RowStyle CssClass="CartListItem"></RowStyle>
                                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Numero Rata">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNRata" runat="server" Width="100px" ReadOnly="True" AutoPostBack="False" CssClass="Input_Text_Right" Text='<%# DataBinder.Eval(Container, "DataItem.NRata") %>'>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Descrizione Rata">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDescRata" runat="server" Width="100px" AutoPostBack="False" CssClass="Input_Text" Text='<%# DataBinder.Eval(Container, "DataItem.Descrizione") %>'>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Data Scadenza">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtDataScadenza" AutoPostBack="False" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" Text='<%# OPENgovTOCO.SharedFunction.FormattaData (DataBinder.Eval(Container, "DataItem.DataScadenza")) %>'>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" ">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <div class="tooltip">
                                                    <asp:ImageButton ID="imgDelete" Width="19px" runat="server" ImageUrl="..\..\images\Bottoni\document_delete_24.png" Visible="True" Height="19px" CommandName="Delete" alt="" CommandArgument='<%# Eval("NRata") %>'></asp:ImageButton>
                                                    <span class="tooltiptext">Elimina Rata</span>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </Grd:RibesGridView>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
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
