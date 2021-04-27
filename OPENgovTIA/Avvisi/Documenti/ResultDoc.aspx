<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ResultDoc.aspx.vb" Inherits="OPENgovTIA.ResultDoc" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <head>
		<title>ResultDoc</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name=vs_defaultClientScript content="JavaScript">
		<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout" topmargin="0" leftmargin="0">
		<form id="Form1" runat="server" method="post">
			<table width="100%">
				<tr>
					<td>
						<asp:label id="Label6" Width="100%" Runat="server" CssClass="lstTabRow">Cartelle Del Ruolo da Elaborare</asp:label>
					</td>
				</tr>
				<tr>
					<td>
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
                        <Grd:RibesGridView ID="GrdAvvisi" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnPageIndexChanging="GrdPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
								<asp:BoundField DataField="Cognome" SortExpression="Cognome" HeaderText="Cognome">
									<ItemStyle HorizontalAlign="Left" Width="130px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="Nome" SortExpression="Nome" HeaderText="Nome">
									<ItemStyle HorizontalAlign="Left" Width="70px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="CodiceCartella" HeaderText="N&#176; Avviso">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="ImportoTotale" HeaderText="Imp. Totale " DataFormatString="{0:N}">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="ImportoArrotondamento" HeaderText="Imp. Arrotondamento " DataFormatString="{0:N}">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="ImportoCarico" HeaderText="Imp. Carico " DataFormatString="{0:N}">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Sel.">
									<ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
									<ItemTemplate>
										<asp:CheckBox id="ChkSelezionato" runat="server" AutoPostBack="true" Checked='<%# DataBinder.Eval(Container, "DataItem.Selezionato")%>'></asp:CheckBox>
                                        <asp:HiddenField runat="server" ID="hfIdContribuente" Value='<%# Eval("IdContribuente") %>' />
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
						<table id="Table5" height="10" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<tr>
								<td colspan="2">
									<asp:label id="Label5" Width="100%" Runat="server" CssClass="lstTabRow">Ordinamento</asp:label>
								</td>
								<td colspan="4">
									<asp:label id="Label7" Width="100%" Runat="server" CssClass="lstTabRow">Elaborazione</asp:label>
								</td>
								<td><asp:Label CssClass="lstTabRow" Runat="server" ID="lblNumDocPerFile">N. Contribuenti per Documento</asp:Label></td>
							</tr>
							<tr>
								<td>
									<asp:radiobutton id="optIndirizzo" runat="server" CssClass="Input_Label" AutoPostBack="true" ToolTip="Ordinamento per indirizzo" GroupName="OptOrdinamento" Checked="true" Text="Indirizzo"></asp:radiobutton>
								</td>
								<td>
									<asp:radiobutton id="optNominativo" runat="server" CssClass="Input_Label" AutoPostBack="true" ToolTip="Ordinamento per nominativo" GroupName="OptOrdinamento" Checked="False" Text="Nominativo"></asp:radiobutton>
								</td>
								<td>
									<asp:radiobutton id="optProve" runat="server" CssClass="Input_Label" AutoPostBack="true" ToolTip="Elaborazione Prove" GroupName="OptElaborazione" Checked="true" Text="Prove"></asp:radiobutton>
								</td>
								<td>
									<asp:radiobutton id="optEffettivo" runat="server" CssClass="Input_Label" AutoPostBack="true" ToolTip="Elaborazione effettiva" GroupName="OptElaborazione" Checked="False" Text="Effettivo"></asp:radiobutton>
								</td>
								<td>
									<asp:CheckBox ID="chkElaboraTutti" runat="server" Checked="False" CssClass="Input_Label" AutoPostBack="true" Text="Elabora Tutti" Visible="False"></asp:CheckBox>
									<!--*** 20140509 - TASI ****-->
					                <br />
					                <asp:checkbox id="chkSendMail" Runat="server" CssClass="Input_Label" Text="Invio tramite EMail" Visible="false" Checked="false" AutoPostBack="true"></asp:checkbox>
					                <!--*** ***-->
								</td>
								<td>
									<asp:CheckBox ID="chkElaboraBollettini" runat="server" Checked="False" CssClass="Input_Label" AutoPostBack="true" Text="Elabora Bollettini" ></asp:CheckBox>
									<br />
									<asp:RadioButton ID="optTD896" runat="server" GroupName="TipoBollettino" Text="TD896" CssClass="Input_Label" Enabled="false" />&nbsp;
									<asp:RadioButton ID="optTD123" runat="server" GroupName="TipoBollettino" Text="TD123" CssClass="Input_Label" Enabled="false" />&nbsp;
									<asp:RadioButton ID="optF24" runat="server" GroupName="TipoBollettino" Text="F24" CssClass="Input_Label" Enabled="false" />&nbsp;
								</td>
								<td>
									<asp:TextBox ID="txtNumDoc" Runat="server" CssClass="Input_Text_Right OnlyNumber" Width="70" AutoPostBack="true" onkeypress="return NumbersOnly(event);">50</asp:TextBox>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
