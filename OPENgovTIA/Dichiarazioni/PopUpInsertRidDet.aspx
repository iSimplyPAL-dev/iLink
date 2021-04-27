<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PopUpInsertRidDet.aspx.vb" Inherits="OPENgovTIA.PopUpInsertRidDet" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>PopUpInsertRidDet</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
			function CheckDati()
			{	
			    document.getElementById('CmdSalva').click()
			}	
    </script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" runat="server" method="post">
        <table id="tblModificaIndirizzo" width="100%" cellspacing="0" cellpadding="0">
            <tr>
                <td class="SfondoGenerale" style="height: 43px">&nbsp;
                    <label id="info" runat="server" class="NormalBold_title">
                        &nbsp;&nbsp; - Dichiarazioni - Gestione Immobili - Gestione 
							Riduzioni/Esenzioni</label>
                </td>
                <td class="SfondoGenerale" style="height: 43px" align="right">
                    <input class="Bottone BottoneAssocia hidden" id="Salva" title="Salva" onclick="CheckDati()" type="button" name="Salva">
                    <input class="Bottone BottoneAnnulla" id="Esci" title="Esci" onclick="window.close();" type="button"
                        name="Esci">&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table id="TblDati" width="100%">
                        <tr>
                            <td width="100%">
                                <asp:Label ID="LblIntestazione" runat="server" CssClass="Input_Label"></asp:Label>
                                <br />
                                <asp:Label CssClass="Legend" runat="server" ID="LblResult">Non sono presenti</asp:Label>
                                <Grd:RibesGridView ID="GrdRisultati" runat="server" BorderStyle="None"
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
                                        <asp:BoundField DataField="sCODICE" HeaderText="CODICE"></asp:BoundField>
                                        <asp:BoundField DataField="sDESCRIZIONE" HeaderText="DESCRIZIONE"></asp:BoundField>
                                        <asp:TemplateField HeaderText="">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneAssociaGrd" CommandName="RowBind" CommandArgument='<%# Eval("sCODICE") %>' alt=""></asp:ImageButton>
                                                <asp:HiddenField runat="server" ID="hfIDENTE" Value='<%# Eval("IDENTE") %>' />
                                                <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </Grd:RibesGridView>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:TextBox ID="TxtIdRiferimento" CssClass="Input_Text" runat="server" Style="display: none">-1</asp:TextBox>
        <asp:Button ID="CmdSalva" runat="server" Style="display: none"></asp:Button>
    </form>
</body>
</html>
