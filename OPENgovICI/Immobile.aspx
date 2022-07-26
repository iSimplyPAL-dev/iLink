<%@ Page language="c#" Codebehind="Immobile.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.Immobile" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Immobile</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%> 
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%> 
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="1" cellPadding="1" width="100%" align="center" border="0">
				<tr>
					<td style="MARGIN-TOP:0px">
						<asp:label id="lblBonificata" runat="server" ForeColor="Red" Visible="False" style="DISPLAY: none">La dichiarazione non è bonificata.</asp:label>
						<asp:label id="lblMessage" runat="server" ForeColor="Red" Visible="False">Non sono presenti immobili per la dichiarazione.</asp:label>
						<asp:button id="btnTornaTestata" runat="server" style="DISPLAY: none" Text="Torna alla Testata" onclick="lnbTornaTestata_Click"></asp:button>
						<asp:button id="btnAddImmobile" runat="server" style="DISPLAY: none" Text="Aggiungi Immobile" onclick="lnbAddImmobile_Click"></asp:button>
					</td>
				</tr>
				<tr>
					<td>
						<Grd:RibesGridView ID="GrdImmobili" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="False" AllowSorting="false" PageSize="4"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
								<asp:TemplateField HeaderText="Data Inizio">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:label id="Label1" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataInizio")) %>'>
										</asp:label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Data Fine">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:label id="Label3" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataFine")) %>'>
										</asp:label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="Foglio" SortExpression="Foglio" HeaderText="Fg.">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="Numero" SortExpression="Numero" HeaderText="Num.">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField SortExpression="Subalterno" HeaderText="Sub.">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:label id="Label2" runat="server" Text='<%# Business.CoreUtility.FormattaGrdInt(DataBinder.Eval(Container, "DataItem.Subalterno")) %>'>
										</asp:label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="CodCategoriaCatastale" HeaderText="Cat.">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="CodClasse" HeaderText="Cl.">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Valore">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:label id=Label4 runat="server" Text='<%# Business.CoreUtility.FormattaGrdInt(DataBinder.Eval(Container, "DataItem.ValoreImmobile")) %>'>
										</asp:label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="% Poss.">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:label id="Label5" runat="server" Text='<%# Business.CoreUtility.FormattaGrdInt(DataBinder.Eval(Container, "DataItem.PercPossesso")) %>'>
										</asp:label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Indirizzo">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label id=lblIndirizzo runat="server" Text='<%# GetIndirizzo(Convert.ToInt32(DataBinder.Eval(Container, "DataItem.IDOggetto"))) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Bonificato" Visible="false">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label id=lblBonificato runat="server" Text='<%# Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.Bonificato")) == true ? "Sì" : "No" %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
                                <asp:TemplateField HeaderText="Sel.">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSel" runat="server" AutoPostBack="true" Checked='<%# Business.CoreUtility.FormattaGrdCheck(DataBinder.Eval(Container, "DataItem.bSel")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
										<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDOGGETTO") %>' alt=""></asp:ImageButton>					                    
                                        <asp:HiddenField runat="server" ID="hfidoggetto" Value='<%# Eval("IDOGGETTO") %>' />
									</itemtemplate>
								</asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>  
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
