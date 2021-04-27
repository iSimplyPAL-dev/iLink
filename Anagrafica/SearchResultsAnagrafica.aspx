<%@ Page Language="vb" CodeBehind="SearchResultsAnagrafica.aspx.vb" AutoEventWireup="false" Inherits="SearchResultsAnagrafica" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<HTML>
	<HEAD>
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		    function LoadContribuente(IdContribuente) {
		        parent.location.href='FormAnagrafica.aspx?popup=0&COD_CONTRIBUENTE='+IdContribuente+'&ID_DATA_ANAGRAFICA=-1';
		    }
		</script>
	</HEAD>
	<BODY class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginwidth="0" marginheight="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<TD><asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label></TD>
				</tr>
				<tr>
					<td>
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
					            <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
					            <asp:BoundField DataField="CODICE_CONTRIBUENTE" SortExpression="CODICE_CONTRIBUENTE" HeaderText="Codice Contribuente">
						            <ItemStyle Width="150px" HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					            </asp:BoundField>
					            <asp:TemplateField SortExpression="NOMINATIVO" HeaderText="Nominativo">
						            <ItemTemplate>
							            <asp:Label id="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NOMINATIVO") %>'></asp:Label>
						            </ItemTemplate>
					            </asp:TemplateField>
					            <asp:BoundField DataField="CFPI" SortExpression="CFPI" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
					            <asp:BoundField DataField="DN" HeaderText="Data di Nascita">
						            <ItemStyle Width="80px" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					            </asp:BoundField>
					            <asp:BoundField DataField="SESSO" HeaderText="Sesso">
						            <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					            </asp:BoundField>
					            <asp:BoundField DataField="ICI" HeaderText="IMU">
						            <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					            </asp:BoundField>
					            <asp:BoundField DataField="TARSU" HeaderText="TARI">
						            <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					            </asp:BoundField>
					            <asp:BoundField DataField="OSAP" HeaderText="OSAP">
						            <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					            </asp:BoundField>
					            <asp:BoundField DataField="SCUOLA" HeaderText="SCUOLE">
						            <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					            </asp:BoundField>
					            <asp:BoundField DataField="H2O" HeaderText="Acquedotto">
						            <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					            </asp:BoundField>
					            <asp:BoundField DataField="PROVVEDIMENTI" HeaderText="Provv.">
						            <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					            </asp:BoundField>
					            <asp:BoundField DataField="DUM" HeaderText="Data Ultima Modifica">
						            <ItemStyle Width="80px" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					            </asp:BoundField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center" Width="40px"></headerstyle>
									<itemstyle horizontalalign="Center"></itemstyle>
									<itemtemplate>
										<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' alt=""></asp:ImageButton>
					                    <asp:HiddenField runat="server" ID="hfIdDataAnagrafica" Value='<%# Eval("IDDATAANAGRAFICA") %>' />
									</itemtemplate>
								</asp:TemplateField>
				            </Columns>
			            </Grd:RibesGridView>
					</td>
				</tr>
			</table>
			<input id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza">
		</FORM>

	</BODY>
</HTML>
