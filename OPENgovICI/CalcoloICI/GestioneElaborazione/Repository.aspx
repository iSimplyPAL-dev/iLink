<%@ Page language="c#" Codebehind="Repository.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.GestioneElaborazione.Repository" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Repository</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		
		function GoToRiepilogoICI(idProgElab)
		{
						
	        winWidth=990;
			winHeight=660;
			myleft=(screen.width-winWidth)/2
			mytop=(screen.height-winHeight)/2 -40;
						
//			CalWin = window.open("GetRiepilogoICIframe.aspx?ID_PROG_ELAB="+idProgElab+"&blnCalcoloMassivo=true","ShowCalcoloICI", "width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+", scrollbars=no,toolbar=no")
			CalWin = window.open("../GetRiepilogoICIframe.aspx?CODCONTRIB=&ANNO=-1&blnCalcoloMassivo=true&ID_PROG_ELAB="+idProgElab,"ShowCalcoloICI", "width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+", scrollbars=no,toolbar=no")
			
			//parent.Visualizza.location.href='../ConfigAliquote.aspx?ID_PROG_ELAB='+idProgElab;
		}
		</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td colSpan="4"><asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label></td>
				</tr>
				<tr>
					<td colSpan="4"><input id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza">
						<Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None" 
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
								<asp:BoundField DataField="DESCRIZIONE" HeaderText="Tipo Elaborazione">
									<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="50%" VerticalAlign="Middle"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="OPERATORE" HeaderText="Operatore">
									<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="25%" VerticalAlign="Middle"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Data Elaborazione">
									<HeaderStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Middle"></ItemStyle>
									<ItemTemplate>
										<asp:Label id=Label1 runat="server" text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DATA_ELABORAZIONE"))%>'>Label</asp:Label>
					                    <asp:HiddenField runat="server" ID="hfPROGRESSIVO" Value='<%# Eval("PROGRESSIVO") %>' />
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="NUMERO_AGGIORNATI" HeaderText="N&#176; Elaborati">
									<HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Width="15%" VerticalAlign="Middle"></ItemStyle>
								</asp:BoundField>
							</Columns>
						</Grd:RibesGridView>
					</td>
				</tr>
			</table>
		</FORM>
	</body>
</HTML>
