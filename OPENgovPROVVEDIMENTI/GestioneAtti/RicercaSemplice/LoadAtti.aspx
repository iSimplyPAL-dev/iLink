<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LoadAtti.aspx.vb" Inherits="Provvedimenti.LoadAtti" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<HTML>
  <HEAD>
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
  </HEAD>
	<BODY class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginheight="0"
		marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD colSpan="4"><asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label></TD>
				</TR>
				<TR>
					<TD colSpan="4"><INPUT id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza">
						<Grd:RibesGridView ID="GrdAtti" runat="server" BorderStyle="None" 
						  BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
						  AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="3"
						  ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
						  OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
						  <PagerSettings Position="Bottom"></PagerSettings>
						  <PagerStyle CssClass="CartListFooter" />
						  <RowStyle CssClass="CartListItem"></RowStyle>
						  <HeaderStyle CssClass="CartListHead"></HeaderStyle>
						  <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
								<asp:BoundField DataField="ANNO" SortExpression="ANNO" HeaderText="Anno">
									<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Width="3%" VerticalAlign="Middle"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="TRIBUTO" HeaderText="Descrizione Provvedimento">
									<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="35%" VerticalAlign="Middle"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="NUMERO_ATTO" HeaderText="Numero Atto">
									<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Width="10%" VerticalAlign="Middle"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Data Creazione">
									<HeaderStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Middle"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="Label1" runat="server" text='<%# GiraData(DataBinder.Eval(Container, "DataItem.DATA_ELABORAZIONE"))%>'>Label</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="IMPORTO_TOTALE_RIDOTTO" HeaderText="Importo Totale Rid €" DataFormatString="{0:#,##0.00}">
									<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Width="13%" VerticalAlign="Middle"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField datafield="IMPORTO_TOTALE" headertext="Importo Totale €" dataformatstring="{0:#,##0.00}">
									<HeaderStyle HorizontalAlign="Center" verticalalign="Middle"></headerstyle>
									<itemstyle horizontalalign="Right" width="13%" verticalalign="Middle"></itemstyle>
								</asp:BoundField>
								<asp:BoundField datafield="stato" headertext="Stato Avviso">
									<HeaderStyle HorizontalAlign="Center" verticalalign="Middle"></headerstyle>
									<itemstyle horizontalalign="Center" width="13%" verticalalign="Middle"></itemstyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="">
						            <headerstyle horizontalalign="Center"></headerstyle>
						            <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
						            <itemtemplate>
						            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID_PROVVEDIMENTO") %>' alt=""></asp:ImageButton>
						                <asp:HiddenField runat="server" ID="hfID_PROVVEDIMENTO" Value='<%# Eval("ID_PROVVEDIMENTO") %>' />
                                        <asp:HiddenField runat="server" ID="hfCOD_TIPO_PROVVEDIMENTO" Value='<%# Eval("COD_TIPO_PROVVEDIMENTO") %>' />
                                        <asp:HiddenField runat="server" ID="hfCOD_TIPO_PROCEDIMENTO" Value='<%# Eval("COD_TIPO_PROCEDIMENTO") %>' />
                                        <asp:HiddenField runat="server" ID="hfCOD_TRIBUTO" Value='<%# Eval("COD_TRIBUTO") %>' />
						            </itemtemplate>
					            </asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>
					</TD>
				</TR>
			</table>
		</FORM>
	</BODY>
</HTML>
