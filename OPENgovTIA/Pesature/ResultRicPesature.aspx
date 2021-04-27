<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ResultRicPesature.aspx.vb" Inherits="OPENgovTIA.ResultRicPesature"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Ddiv HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ResultRicPesature</title>
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
			function DettaglioPesature(IdTessera)
			{
	    		//parent.parent.Comandi.location.href='ComandiGestPesature.aspx'
			    parent.parent.Visualizza.location.href = 'GestionePesature.aspx?IdTessera=' + IdTessera
			    parent.parent.Comandi.location.href = '../../aspVuotaRemoveComandi.aspx'
			    parent.parent.Basso.location.href = '../../aspVuotaRemoveComandi.aspx'
			}
		</script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="0" rightMargin="0">
		<form id="Form1" runat="server" method="post">
			<div class="col-md-12">
				<asp:label id="LblResult" Runat="server" CssClass="Legend">La ricerca non ha prodotto risultati.</asp:label>
                <Grd:RibesGridView ID="GrdTessere" runat="server" BorderStyle="None" 
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                    OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <PagerStyle CssClass="CartListFooter" />
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField DataField="sCognome" HeaderText="Cognome">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="sNome" HeaderText="Nome">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="sCFPIva" HeaderText="Cod.Fiscale/P.IVA">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Indirizzo">
                            <HeaderStyle horizontalalign="Center"></HeaderStyle>
                            <ItemStyle horizontalalign="Justify"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStradario" Text='<%# DataBinder.Eval(Container, "DataItem.sVia") +" "+  DataBinder.Eval(Container, "DataItem.sCivico") +" "+  DataBinder.Eval(Container, "DataItem.sInterno") +" "+  DataBinder.Eval(Container, "DataItem.sEsponente") +" "+  DataBinder.Eval(Container, "DataItem.sScala")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="sNumTessera" HeaderText="N.Tessera">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="nTotConferimenti" HeaderText="N.Conferimenti">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="nTotVolume" HeaderText="Litri" DataFormatString="{0:0.00}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Fatturato">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<asp:CheckBox ID="chkSel" runat="server" Checked='<%# Eval("IsFatturato") %>'/>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
                            <itemtemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("nIdTessera") %>' alt=""></asp:ImageButton>
                            </itemtemplate>
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Center"></itemstyle>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
			</div>
			<div class="col-md-12">
				<fieldset class="FiledSetRicerca col-md-12">
					<legend class="Legend">Totalizzatori</legend>
					<div class="col-md-12">
						<div class="col-md-3">
							<asp:Label ID="LblNContribuenti" Runat="server" CssClass="Input_Label col-md-12">0</asp:Label>
						</div>
						<div class="col-md-3">
							<asp:Label ID="LblNConferimenti" Runat="server" CssClass="Input_Label col-md-12">0</asp:Label>
						</div>
						<div class="hidden">
							<asp:Label ID="LblKg" Runat="server" CssClass="Input_Label col-md-12">0</asp:Label>
						</div>
						<div class="col-md-4">
							<asp:Label ID="LblVolume" Runat="server" CssClass="Input_Label col-md-12">0</asp:Label>
						</div>
					</div>
				</fieldset>
			</div>
			<asp:Button ID="CmdDelete" Runat="server" style="DISPLAY: none"></asp:Button>
		</form>
	</body>
</HTML>
