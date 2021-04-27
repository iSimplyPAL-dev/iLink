<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ResultAvvisi.aspx.vb" Inherits="OPENgovTIA.ResultAvvisi" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ResultAvvisi</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
    		function LoadAvvisi(IsFromVariabile,IdAvviso,IdRuolo,AzioneProv){
    		    parent.parent.Comandi.location.href = '../../../aspVuotaRemoveComandi.aspx';
    		    parent.parent.Basso.location.href = '../../../aspVuotaRemoveComandi.aspx';
    		    parent.parent.Nascosto.location.href = '../../../aspVuotaRemoveComandi.aspx';
    		    parent.parent.Visualizza.location.href = 'GestAvvisi.aspx?IsFromVariabile=' + IsFromVariabile + '&IdUniqueAvviso=' + IdAvviso + '&IdRuolo=' + IdRuolo + '&AzioneProv=' + AzioneProv;
    		}
		</script>
	</head>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0" style="Z-INDEX: 102; POSITION: absolute; TOP: 0px; LEFT: 0px">
				<tr>
					<td>
						<asp:label id="LblResult" CssClass="Legend" Runat="server">Risultati della Ricerca</asp:label>
					</td>
				</tr>
				<TR>
					<TD>
                        <Grd:RibesGridView ID="GrdAvvisi" runat="server" BorderStyle="None" 
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
								<asp:BoundField DataField="sCognome" SortExpression="sCognome" HeaderText="Cognome"></asp:BoundField>
								<asp:BoundField DataField="sNome" SortExpression="sNome" HeaderText="Nome"></asp:BoundField>
								<asp:BoundField DataField="sCodFiscale" SortExpression="sCfPiva" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
								<asp:BoundField DataField="sAnnoRiferimento" HeaderText="Anno"></asp:BoundField>
								<asp:BoundField DataField="impPF" HeaderText="Imp.Fissa" DataFormatString="{0:N}">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="impPV" HeaderText="Imp.Variabile" DataFormatString="{0:N}">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="impPC" HeaderText="Imp.Conferimenti" DataFormatString="{0:N}">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="impPM" HeaderText="Imp.Maggiorazione" DataFormatString="{0:N}">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="sCodiceCartella" HeaderText="N.Avviso"></asp:BoundField>
								<asp:BoundField DataField="impCarico" HeaderText="Carico" DataFormatString="{0:N}">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="impPagato" HeaderText="Pagato" DataFormatString="{0:N}">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:ImageButton id="imgEdit" runat="server" Height="15px" Width="15px" ToolTip='<%# FncGrd.FormattaToolTipIsSgravato(DataBinder.Eval(Container, "DataItem.bIsSgravio"),DataBinder.Eval(Container, "DataItem.IMPPRESGRAVIO")) %>' ImageUrl='<%# FncGrd.FormattaIsSgravato(DataBinder.Eval(Container, "DataItem.bIsSgravio")) %>'>
										</asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
										<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
					                    <asp:HiddenField runat="server" ID="hfIdContribuente" Value='<%# Eval("IdContribuente") %>' />
									</itemtemplate>
								</asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
