<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GestRibaltaVar.aspx.vb" Inherits="OpenUtenze.GestRibaltaVar" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>GestRibaltaVar</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name=vs_defaultClientScript content="JavaScript">
		<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
  </HEAD>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<table style="Z-INDEX: 101; POSITION: absolute; TOP: 0px; LEFT: 0px" width="100%">
				<tr>
					<td>
						<asp:label id="LblResult" CssClass="Legend" Runat="server">Risultati della Ricerca</asp:label>
					</td>
					<td align="right">
						<asp:checkbox id="chkElaboraTutti" runat="server" CssClass="Input_Label" Text="Seleziona Tutti" Checked="False" AutoPostBack="True"></asp:checkbox>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<Grd:RibesGridView ID="GrdRibaltaVar" runat="server" BorderStyle="None" 
						    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
						    AutoGenerateColumns="False" AllowPaging="False" AllowSorting="false" PageSize="10"
						    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
						    OnPageIndexChanging="GrdPageIndexChanging">
						    <PagerSettings Position="Bottom"></PagerSettings>
						    <PagerStyle CssClass="CartListFooter" />
						    <RowStyle CssClass="CartListItem"></RowStyle>
						    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
						    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
								<asp:BoundField DataField="sPeriodo" HeaderText="Periodo">
									<ItemStyle Width="80px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="sNomeIntestatario" HeaderText="Intestatario">
									<ItemStyle Width="300px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="sNomeUtente" HeaderText="Utente">
									<ItemStyle Width="300px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="sMatricola" HeaderText="Matricola">
									<ItemStyle Width="100px"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Var.su Contatore">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:ImageButton id="imgEdit" runat="server" Height="15px" Width="15px" CommandName="Edit" ImageUrl='<%# FormattaVariazione(DataBinder.Eval(Container, "DataItem.sVarContatore")) %>'>
										</asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Var.su Lettura">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:ImageButton id="Imagebutton1" runat="server" Height="15px" Width="15px" CommandName="Edit" ImageUrl='<%# FormattaVariazione(DataBinder.Eval(Container, "DataItem.sVarLettura")) %>'>
										</asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Sel.">
									<ItemStyle HorizontalAlign="Center" Width="20px"></ItemStyle>
									<ItemTemplate>
										<asp:CheckBox id="ChkSelezionato" runat="server" AutoPostBack="True" Checked='<%# DataBinder.Eval(Container, "DataItem.bIsSel")%>'></asp:CheckBox>
									<asp:HiddenField runat="server" ID="hfnIdVariazione" Value='<%# Eval("nIdVariazione") %>' />
                                    </ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox id="TextBox1" runat="server"></asp:TextBox>

									</EditItemTemplate>
								</asp:TemplateField>
								
							</Columns>
							</Grd:RibesGridView>
					</td>
				</tr>
				<tr>
					<td>
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
			</table>
			<asp:Button id="CmdRibaltaVar" Runat="server" style="DISPLAY:none"></asp:Button>
		</form>
	</body>
</HTML>
