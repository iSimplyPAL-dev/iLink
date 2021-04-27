<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ResultPag.aspx.vb" Inherits="OPENgovTIA.ResultPag" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ResultPag</title>
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
    		function LoadPagamento(Tributo,NonAbb,IdPagamento,Anno,CFPIVA,CodCartella,DataPagamento){
    		    //parent.parent.Comandi.location.href='ComandiGestPag.aspx?TRIBUTO='+ Tributo
    		    parent.parent.Comandi.location.href = '../../../aspVuotaRemoveComandi.aspx';
				parent.parent.Visualizza.location.href = 'GestPag.aspx?TRIBUTO='+ Tributo + '&NonAbb=' + NonAbb + '&IdListPagamento=' + IdPagamento + '&Anno=' + Anno + '&CFPIVA=' + CFPIVA + '&CodCartella=' + CodCartella + '&DataPagamento=' + DataPagamento
    		}
    		function CheckAuth() {
    		    if ($('#hfProvMenu').val() == 'CMGC|0') {
    		        console.log('Posizione in sola consultazione!');
    		        GestAlert('a', 'info', '', '', 'Posizione in sola consultazione!');
    		        return false;
    		    }
    		    else {
    		        return true;
    		    }
    		}
		</script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout" rightmargin="0" leftmargin="0">
		<form id="Form1" runat="server" method="post">
			<table width="100%" style="Z-INDEX: 101; POSITION: absolute; TOP: 0px; LEFT: 0px">
				<tr>
					<td>
						<asp:label id="LblResult" Runat="server" CssClass="Legend">Risultati della Ricerca</asp:label>
					</td>
					<td align="right">
					    &emsp;&emsp;&emsp;<asp:Label ID="LblNPag" CssClass="Legend" runat="server"></asp:Label>
					    &emsp;&emsp;
					    <asp:Label ID="LblImpPag" CssClass="Legend" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<Grd:RibesGridView ID="GrdPagamenti" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="14"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
								<asp:BoundField DataField="sAnno" HeaderText="Anno"></asp:BoundField>
								<asp:BoundField DataField="sNumeroAvviso" HeaderText="Avviso/Id.Operazione"></asp:BoundField>
								<asp:TemplateField HeaderText="Nominativo">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.sCognome")+" "+ DataBinder.Eval(Container, "DataItem.sNome") %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="sCFPIVA" HeaderText="Cod.Fiscale/P.IVA">
                                    <ItemStyle Width="120px"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Provenienza">
									<ItemStyle HorizontalAlign="Justify"></ItemStyle>
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# FncGrd.FormattaProvPagGrd(DataBinder.Eval(Container, "DataItem.sProvenienza")) %>'></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="sNumeroRata" HeaderText="N. Rata">
									<ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Data Pagamento">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="LblDataPag" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataPagamento")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Data Riversamento">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataAccredito")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="dImportoPagamento" HeaderText="Imp. Pagato" DataFormatString="{0:N}">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="">
	                                <headerstyle horizontalalign="Center"></headerstyle>
	                                <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
	                                <itemtemplate>
	                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("Id") %>' alt="" OnClientClick="return CheckAuth();"></asp:ImageButton>
	                                    <asp:HiddenField runat="server" ID="hfIdContribuente" Value='<%# Eval("IdContribuente") %>' />
                                        <asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("Id") %>' />
	                                </itemtemplate>
                                </asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>
					</td>
				</tr>
			</table>
            <asp:HiddenField ID="hfProvMenu" runat="server" />
		</form>
	</body>
</HTML>
