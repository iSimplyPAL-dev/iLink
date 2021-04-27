<%@ Page Language="vb" CodeBehind="SearchResultsAnagraficaGenerale.aspx.vb" AutoEventWireup="false" Inherits="SearchResultsAnagraficaGenerale" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<HTML>
	<HEAD>
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
	</HEAD>
	<BODY class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginwidth="0"
		marginheight="0">
		<form id="Form1" runat="server" method="post">
			<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD>&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label></TD>
				</TR>
				<TR>
					<TD>
                    <Grd:RibesGridView ID="GrdAnagrafiche" runat="server" BorderStyle="None" 
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
							<asp:BoundField DataField="CODICE_CONTRIBUENTE" SortExpression="CODICE_CONTRIBUENTE" HeaderText="Codice Contribuente">
								<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
							</asp:BoundField>
                            <asp:BoundField DataField="DESCRIZIONE_ENTE" HeaderText="Ente"></asp:BoundField>
							<asp:TemplateField SortExpression="NOMINATIVO" HeaderText="Nominativo">
								<ItemTemplate>
									<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NOMINATIVO") %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField DataField="CFPI" SortExpression="CFPI" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
							<asp:BoundField DataField="DN" HeaderText="Data di Nascita">
								<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="SESSO" HeaderText="Sesso">
								<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="DUM" HeaderText="Data Ultima Modifica">
								<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
							</asp:BoundField>
						    <asp:TemplateField>
						        <HeaderStyle horizontalalign="Center"></HeaderStyle>
						        <ItemStyle horizontalalign="Center" Width="40px"></ItemStyle>
                                <itemtemplate>
								    <asp:ImageButton runat="server" CommandName="RowBind" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' Cssclass="BottoneGrd BottoneAssociaGrd" alt=""></asp:ImageButton>
                                </itemtemplate>
                            </asp:TemplateField>
						    <asp:TemplateField>
						        <HeaderStyle horizontalalign="Center"></HeaderStyle>
						        <ItemStyle horizontalalign="Center" Width="40px"></ItemStyle>
                                <itemtemplate>
								    <asp:ImageButton runat="server" CommandName="RowOpen" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' Cssclass="BottoneGrd BottoneApriGrd" alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfCODICE_CONTRIBUENTE" Value='<%# Eval("CODICE_CONTRIBUENTE") %>' />
                                    <asp:HiddenField runat="server" ID="hfIDDATAANAGRAFICA" Value='<%# Eval("IDDATAANAGRAFICA") %>' />
                                    <asp:HiddenField runat="server" ID="hfCOGNOME_DENOMINAZIONE" Value='<%# Eval("COGNOME_DENOMINAZIONE") %>' />
                                    <asp:HiddenField runat="server" ID="hfNOME" Value='<%# Eval("NOME") %>' />
                                    <asp:HiddenField runat="server" ID="hfCOD_FISCALE" Value='<%# Eval("COD_FISCALE") %>' />
                                    <asp:HiddenField runat="server" ID="hfPARTITA_IVA" Value='<%# Eval("PARTITA_IVA") %>' />
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </asp:TemplateField>
						</Columns>
                    </Grd:RibesGridView>
					</TD>
				</TR>
			</TABLE>
			<INPUT id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza"> 
            <INPUT id="paginacomandi" type="hidden" name="paginacomandi">
            <INPUT id="Hidden1" type="hidden" name="paginacomandi">
			<asp:textbox id="txtCodContrib" runat="server" CssClass="hidden"></asp:textbox>
            <asp:textbox id="txtIdDataAnag" runat="server" CssClass="hidden"></asp:textbox>
            <asp:button id="btnAssocia" runat="server" CssClass="hidden"></asp:button>
			<asp:Button id="btnModifica" runat="server" CssClass="hidden"></asp:Button>
		</FORM>
	</BODY>
</HTML>
