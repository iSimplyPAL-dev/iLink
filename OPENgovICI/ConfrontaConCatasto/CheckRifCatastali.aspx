<%@ Page language="c#" Codebehind="CheckRifCatastali.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CheckRifCatastali" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CheckRifCatastali</title> 
		<!--*** 20130213 - controllo con catasto ***-->
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		function MyCall(Tipo)
		{
			document.getElementById('DivAttesa').style.display='';
			document.getElementById('LblDownloadFile').style.display='none';
			if(document.getElementById('GrdDichRifErrataCopertura')!=null)
			{
				document.getElementById('GrdDichRifErrataCopertura').style.display='none';
			}
			if(document.getElementById('GrdDichCatRifCatastali')!=null)
			{
				document.getElementById('GrdDichCatRifCatastali').style.display='none';
			}
			if(document.getElementById('GrdDichRifCatastali')!=null)
			{
				document.getElementById('GrdDichRifCatastali').style.display='none';
			}
			if (document.getElementById('GrdDichTARSUnoICI') != null) {
			    document.getElementById('GrdDichTARSUnoICI').style.display = 'none';
			}
			if (Tipo == "S")
			    document.getElementById('CmdRicerca').click();
			if (Tipo=="P")
			    document.getElementById('CmdStampa').click();
		}
		</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="3" rightMargin="3" MS_POSITIONING="GridLayout">
		<form style="Z-INDEX: 101; POSITION: absolute; TOP: 0px; LEFT: 0px" id="FrmEstrazioni"
			method="post" runat="server">
			<table width="100%">
				<tr class="SfondoGenerale">
					<td>
						<table id="Table1" class="SfondoGenerale" border="0" cellSpacing="0" cellPadding="0" width="100%"
							align="right">
							<tr>
								<td style="WIDTH: 464px; HEIGHT: 20px" align="left"><span style="WIDTH: 400px" id="infoEnte" class="ContentHead_Title"><asp:label id="lblTitolo" runat="server"></asp:label></span></td>
								<td rowspan="2" width="800" colSpan="2" align="right">
									<input id="Stampa" class="Bottone BottoneExcel" title="Stampa" onclick="MyCall('P')" type="button" name="Stampa" />
									<input id="Ricerca" class="Bottone BottoneRicerca" title="Ricerca" onclick="MyCall('S')" type="button" name="Ricerca" />
								</td>
							</tr>
							<tr>
								<td style="WIDTH: 463px" align="left"><span style="WIDTH: 400px; HEIGHT: 20px;" id="info" class="NormalBold_title">ICI/IMU - Analisi - Controlli Riferimenti Catastali</span>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table id="Table4" border="0" cellspacing="1" cellpadding="1" width="100%">
							<tr>
								<td>
									<fieldset style="WIDTH: 100%" class="FiledSetRicerca"><legend class="Legend">Inserimento filtri di ricerca</legend>
										<table width="100%">
											<tr width="100%">
												<td><asp:radiobutton id="OptRifMancanti" Text="Riferimenti mancanti" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" Runat="server" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton></td>
												<td><asp:radiobutton id="OptCatNoDic" Text="Rif.Catastali non in Dichiarazioni" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" Runat="server" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton></td>
												<td><asp:radiobutton id="OptCatEqualDic" runat="server" Text="Posizione Dichiarata uguale a Catastale" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton></td>
												<td><asp:label id="Label6" CssClass="Input_Label" Runat="server">Anno</asp:label><br />
													<asp:dropdownlist id="DdlAnno" CssClass="Input_Label" Runat="server"></asp:dropdownlist></td>
											</tr>
											<tr>
												<td><asp:radiobutton id="OptRifAccertati" Text="Riferimenti accertati" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" Runat="server" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton></td>
												<td><asp:radiobutton id="OptDicNoCat" Text="Rif.Dichiarati non a catasto" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" Runat="server" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton></td>
												<td><asp:radiobutton id="OptCatDifferentDic" runat="server" Text="Cat. e/o Classe Dichiarata diversa da Catastale" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton></td>
											</tr>
											<tr>
												<td><asp:radiobutton id="OptRifErrataCopertura" Text="Riferimenti con errata copertura di possesso" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" Runat="server" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton></td>
												<td><asp:radiobutton id="OptICInoTARSU" Text="Rif.Dichiarati in ICI/IMU e non in TARI" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" Runat="server" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton></td>
												<td><asp:radiobutton id="OptRenCatDifferentDic" runat="server" Text="Rendita e/o Consistenza Catastale diversa da Dichiarata" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton></td>
											</tr>
											<tr>
												<td><asp:radiobutton style="DISPLAY: none" id="OptRifChiusi" Text="Riferimenti chiusi e non riaperti" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" Runat="server" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton>
													<asp:radiobutton style="DISPLAY: none" id="OptRifDoppi" Text="Riferimenti doppi per lo stesso periodo" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" Runat="server" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton></td>
												<td><asp:radiobutton id="OptTARSUnoICI" Text="Rif.Dichiarati in TARI e non in ICI/IMU" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" Runat="server" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton></td>
												<td><asp:radiobutton id="OptPropCatDifferentDic" runat="server" Text="Proprietario e/o Copertura Catastale diverso da Dichiarato" GroupName="OptCheckRifCat" Checked="False" CssClass="Input_Label" OnCheckedChanged="ClearTypeCheck" AutoPostBack="true"></asp:radiobutton></td>
											</tr>
										</table>
									</fieldset>
								</td>
							</tr>
							<tr>
								<td><asp:label id="LblResult" CssClass="Legend" Runat="server">Risultati della Ricerca</asp:label></td>
							</tr>
							<tr>
								<td>
									<asp:LinkButton ID="LblDownloadFile" Runat="server" CssClass="Input_Label" Font-Underline="True" onclick="LblDownloadFile_Click"></asp:LinkButton>
								</td>
							</tr>
							<tr>
								<td>
								    <Grd:RibesGridView ID="GrdDichRifCatastali" runat="server" BorderStyle="None" 
                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="8"
                                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                        OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                                        <PagerSettings Position="Bottom"></PagerSettings>
                                        <PagerStyle CssClass="CartListFooter" />
                                        <RowStyle CssClass="CartListItem"></RowStyle>
                                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
										<Columns>
											<asp:TemplateField HeaderText="Nominativo">
												<ItemStyle Width="240px"></ItemStyle>
												<ItemTemplate>
													<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Cognome") +" "+  DataBinder.Eval(Container, "DataItem.Nome")%>' ID="Label1">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="CFPIva" HeaderText="Cod.Fiscale/P.IVA">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="Ubicazione">
												<ItemStyle Width="300px"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label35" runat="server" text='<%# Business.CoreUtility.FormattaVia(DataBinder.Eval(Container, "DataItem.Via"),DataBinder.Eval(Container, "DataItem.civico"),DataBinder.Eval(Container, "DataItem.Interno"),DataBinder.Eval(Container, "DataItem.Esponente"),DataBinder.Eval(Container, "DataItem.scala"),DataBinder.Eval(Container, "DataItem.piano")) %>'>Label</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="Foglio" HeaderText="Foglio">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="Numero" HeaderText="Numero">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="Sub" HeaderText="Subalterno">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="Data Inizio">
												<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label34" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATAINIZIO")) %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Data Fine">
												<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label25" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATAFINE")) %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="codcategoriacatastale" HeaderText="Cat.">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="codclasse" HeaderText="Classe">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="consistenza" HeaderText="Cons.">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="rendita" HeaderText="Rendita" DataFormatString="{0:0.00}">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="">
									            <headerstyle horizontalalign="Center"></headerstyle>
									            <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									            <itemtemplate>
										            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("idoggetto") %>' alt=""></asp:ImageButton>
					                                <asp:HiddenField runat="server" ID="hfidcontribuente" Value='<%# Eval("idcontribuente") %>' />
                                                    <asp:HiddenField runat="server" ID="hfidtestata" Value='<%# Eval("idtestata") %>' />
                                                    <asp:HiddenField runat="server" ID="hfidoggetto" Value='<%# Eval("idoggetto") %>' />
									            </itemtemplate>
								            </asp:TemplateField>
										</Columns>
									</Grd:RibesGridView>
									<Grd:RibesGridView ID="GrdDichCatRifCatastali" runat="server" BorderStyle="None" 
                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="8"
                                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                        OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                                        <PagerSettings Position="Bottom"></PagerSettings>
                                        <PagerStyle CssClass="CartListFooter" />
                                        <RowStyle CssClass="CartListItem"></RowStyle>
                                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
										<Columns>
											<asp:TemplateField HeaderText="Nominativo">
												<ItemTemplate>
													<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Cognome") +" "+  DataBinder.Eval(Container, "DataItem.Nome")%>' ID="Label2">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="CFPIva" HeaderText="Cod.Fiscale/P.IVA">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="Ubicazione">
												<ItemTemplate>
													<asp:Label id="Label3" runat="server" text='<%# Business.CoreUtility.FormattaVia(DataBinder.Eval(Container, "DataItem.Via"),DataBinder.Eval(Container, "DataItem.civico"),DataBinder.Eval(Container, "DataItem.Interno"),DataBinder.Eval(Container, "DataItem.Esponente"),DataBinder.Eval(Container, "DataItem.scala"),DataBinder.Eval(Container, "DataItem.piano")) %>'>Label</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="Foglio" HeaderText="Fg">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="Numero" HeaderText="Num">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="Sub" HeaderText="Sub">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="Data Inizio">
												<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label4" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATAINIZIO")) %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Data Fine">
												<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label5" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATAFINE")) %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="codcategoriacatastale" HeaderText="Cat.">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="codclasse" HeaderText="Classe">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="consistenza" HeaderText="Cons.">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="rendita" HeaderText="Rendita" DataFormatString="{0:0.00}">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="percpossesso" HeaderText="Poss." DataFormatString="{0:0.00}">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="ui_cat" HeaderText="Categ. Cat.">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="ui_classe" HeaderText="Classe Cat.">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="ui_cons" HeaderText="Cons. Cat.">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="ui_rend" HeaderText="Rendita Cat." DataFormatString="{0:0.00}">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="ui_percpossesso" HeaderText="Poss. Cat." DataFormatString="{0:0.00}">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="Prop. Cat. Diverso">
												<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:ImageButton id="Imagebutton1" runat="server" Height="15px" Width="15px" CommandName="Edit" ImageUrl='<%# Business.CoreUtility.FormattaGrdCheck(DataBinder.Eval(Container, "DataItem.ui_proprietariodiverso"),"../../images/Bottoni/") %>'>
													</asp:ImageButton>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="">
									            <headerstyle horizontalalign="Center"></headerstyle>
									            <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									            <itemtemplate>
										            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("idoggetto") %>' alt=""></asp:ImageButton>
					                                <asp:HiddenField runat="server" ID="hfidcontribuente" Value='<%# Eval("idcontribuente") %>' />
                                                    <asp:HiddenField runat="server" ID="hfidtestata" Value='<%# Eval("idtestata") %>' />
                                                    <asp:HiddenField runat="server" ID="hfidoggetto" Value='<%# Eval("idoggetto") %>' />
									            </itemtemplate>
								            </asp:TemplateField>
										</Columns>
									</Grd:RibesGridView>
									<Grd:RibesGridView ID="GrdDichRifErrataCopertura" runat="server" BorderStyle="None" 
                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="8"
                                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                        OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                                        <PagerSettings Position="Bottom"></PagerSettings>
                                        <PagerStyle CssClass="CartListFooter" />
                                        <RowStyle CssClass="CartListItem"></RowStyle>
                                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
										<Columns>
											<asp:BoundField DataField="Foglio" HeaderText="Foglio">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="Numero" HeaderText="Numero">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="Sub" HeaderText="Subalterno">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="Inizio Validita Estrazione">
												<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label9" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATAINIZIO")) %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Fine Validita Estrazione">
												<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label10" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATAFINE")) %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="copertura" HeaderText="Perc.Possesso" DataFormatString="{0:0.00}">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="">
									            <headerstyle horizontalalign="Center"></headerstyle>
									            <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									            <itemtemplate>
										            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
					                                <asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("ID") %>' />
									            </itemtemplate>
								            </asp:TemplateField>
										</Columns>
									</Grd:RibesGridView>
								    <Grd:RibesGridView ID="GrdDichTARSUnoICI" runat="server" BorderStyle="None" 
                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="8"
                                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                        OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                                        <PagerSettings Position="Bottom"></PagerSettings>
                                        <PagerStyle CssClass="CartListFooter" />
                                        <RowStyle CssClass="CartListItem"></RowStyle>
                                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
										<Columns>
											<asp:TemplateField HeaderText="Nominativo">
												<ItemStyle Width="240px"></ItemStyle>
												<ItemTemplate>
													<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Cognome") +" "+  DataBinder.Eval(Container, "DataItem.Nome")%>' ID="Label1">
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="CFPIva" HeaderText="Cod.Fiscale/P.IVA">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="Ubicazione">
												<ItemStyle Width="300px"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label35" runat="server" text='<%# Business.CoreUtility.FormattaVia(DataBinder.Eval(Container, "DataItem.Via"),DataBinder.Eval(Container, "DataItem.civico"),DataBinder.Eval(Container, "DataItem.Interno"),DataBinder.Eval(Container, "DataItem.Esponente"),DataBinder.Eval(Container, "DataItem.scala"),DataBinder.Eval(Container, "DataItem.piano")) %>'>Label</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="Foglio" HeaderText="Foglio">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="Numero" HeaderText="Numero">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="Sub" HeaderText="Subalterno">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="Data Inizio">
												<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label34" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATAINIZIO")) %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Data Fine">
												<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="Label25" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATAFINE")) %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="CATTARES" HeaderText="Cat.">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="mq" HeaderText="Mq" DataFormatString="{0:0.00}">
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="mqtassabili" HeaderText="Mq Tassabili" DataFormatString="{0:0.00}">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="nvani" HeaderText="N.Vani" DataFormatString="{0:0.00}">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundField>
											<asp:TemplateField HeaderText="">
									            <headerstyle horizontalalign="Center"></headerstyle>
									            <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									            <itemtemplate>
										            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("iddettagliotestata") %>' alt=""></asp:ImageButton>
					                                <asp:HiddenField runat="server" ID="hfidcontribuente" Value='<%# Eval("idcontribuente") %>' />
                                                    <asp:HiddenField runat="server" ID="hfIDTESSERA" Value='<%# Eval("IDTESSERA") %>' />
                                                    <asp:HiddenField runat="server" ID="hfiddettagliotestata" Value='<%# Eval("iddettagliotestata") %>' />
									            </itemtemplate>
								            </asp:TemplateField>
										</Columns>
									</Grd:RibesGridView>
                                   <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                                        <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                                        <div class="BottoneClessidra">&nbsp;</div>
                                        <div class="Legend">Attendere Prego</div>
                                    </div>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<asp:Button ID="CmdRicerca" Runat="server" style="DISPLAY: none" onclick="CmdRicerca_Click"></asp:Button>
			<asp:Button ID="CmdStampa" Runat="server" style="DISPLAY: none" onclick="CmdStampa_Click"></asp:Button></form>
	</body>
</HTML>
