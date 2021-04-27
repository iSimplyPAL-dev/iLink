<%@ Page language="c#" Codebehind="GetVersamenti.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.GetVersamenti" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!doctype html public "-//w3c//dtd html 4.0 transitional//en" >
<html>
<head>
	<title>RicercaVersamenti</title>
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
</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="0" topMargin="0">
		<form id="Form1" runat="server" method="post">
			<table id="tblgrid" width="100%">
				<tr>
				    <td>
		                <asp:label runat="server" id="lblRisultati" CssClass="lstTabRow" Width="100%"></asp:label> <br />
				    </td>
			    </tr>
			    <tr>
			        <td>
		                <Grd:RibesGridView ID="GrdRisultati" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
			                <columns>
	                            <asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Tributo">
		                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
		                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
	                            </asp:BoundField>
				                <asp:TemplateField headertext="Data Pagam.">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="left" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label runat="server" text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataPagamento")) %>' id="lblDataPagamento">
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Data Rivers.">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="left" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label runat="server" text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataRiversamento")) %>' id="Label7">
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Acc./Saldo">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="left" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label id="lblSaldo" runat="server" text='<%# Business.CoreUtility.FormattaGrdAccontoSaldo(DataBinder.Eval(Container, "DataItem.Saldo"), DataBinder.Eval(Container, "DataItem.Acconto")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField  headertext="Violaz.">
					                <itemstyle horizontalalign="center"></itemstyle>
					                <itemtemplate>
						                <asp:label id="Label9" runat="server" text='<%# Business.CoreUtility.FormattaGrdViolazione(DataBinder.Eval(Container, "DataItem.Violazione")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Imp. Abi. Princ.">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="Right" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label id="Label4" runat="server" text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoAbitazPrincipale")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Imp. Altri Fab.">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="Right" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label id="Label3" runat="server" text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoAltriFabbric")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Imp. Altri Fab. Stato">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="Right" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label id="Label1" runat="server" text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoAltriFabbricstatale")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Imp. Aree Fab.">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="Right" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label id="Label2" runat="server" text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoAreeFabbric")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Imp. Aree Fab. Stato">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="Right" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label id="Label10" runat="server" text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoAreeFabbricstatale")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Imp. Terreni">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="Right" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label id="Label11" runat="server" text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImpoTerreni")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Imp. Terreni Stato">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="Right" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label id="Label12" runat="server" text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoTerrenistatale")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Imp. Fab.Rur.">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="Right" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label id="Label13" runat="server" text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMPORTOFABRURUSOSTRUM")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Imp. Fab.Rur. Stato">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="Right" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label id="Label14" runat="server" text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMPORTOFABRURUSOSTRUMstatale")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Detraz.">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="Right" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label id="Label5" runat="server" text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.DetrazioneAbitazPrincipale")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:TemplateField headertext="Imp. Pagato">
					                <headerstyle horizontalalign="Center"></headerstyle>
					                <itemstyle horizontalalign="Right" width="10%"></itemstyle>
					                <itemtemplate>
						                <asp:label id="Label6" runat="server" text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoPagato")) %>'>
						                </asp:label>
					                </itemtemplate>
				                </asp:TemplateField>
				                <asp:BoundField DataField="NumeroFabbricatiPosseduti" headertext="Num. Fabb."></asp:BoundField>
			                </columns>
		                </Grd:RibesGridView>
			        </td>
			    </tr>
			</table>
	    </form>
    </body>
</html>
