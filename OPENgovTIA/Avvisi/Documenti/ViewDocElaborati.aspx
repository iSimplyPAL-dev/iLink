<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ViewDocElaborati.aspx.vb" Inherits="OPENgovTIA.ViewDocElaborati" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ViewDocElaborati</title>
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
			function LoadElaborazioneDocumenti()
    		{
			    if (document.getElementById('TxtProvenienzaForm').value == "DOC") {
    			    parent.parent.Comandi.location.href='ComandiDoc.aspx'
					parent.parent.Visualizza.location.href='RicercaDoc.aspx'
				}
				else {
				    parent.parent.Comandi.location.href = '../Gestione/ComandiRicAvvisi.aspx?IsFromVariabile=<%=Session("IsFromVariabile")%>'
				    parent.parent.Visualizza.location.href = '../Gestione/RicAvvisi.aspx?IsFromVariabile=<%=Session("IsFromVariabile")%>'
				}
    		}
    		function downloadAll(){
    			document.getElementById("btnDownloadAll").click()
    		}
            function CloseMe()
            {
                parent.document.getElementById('divStampa').style.display = 'none'; 
                parent.document.getElementById('divAvviso').style.display = '';
            }
		</script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table style="Z-INDEX: 100; POSITION: absolute; TOP: 0px; LEFT: 0px" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td align="left">
						<span id="title" class="lstTabRow" style="HEIGHT:20px">Stampa</span>
					</td>
					<td align="right">
						<input class="Bottone Bottoneannulla" id="Esci" title="Esci" onclick="CloseMe();" type="button" name="Esci">
					</td>
				</tr>
				<tr>
				    <td>
					    <asp:label id="Label1" runat="server" CssClass="Legend">Elenco Documenti Elaborati</asp:label>
				    </td>
				    <td>
					    <asp:label id="lblDownloadAll" runat="server" CssClass="Link_Label" onclick="downloadAll()">Scarica tutti i file</asp:label>
					    <asp:Button id="btnDownloadAll" runat="server" Text="Scarica tutti i file" style="DISPLAY:none"></asp:Button>
					    <asp:TextBox id="txtPath" runat="server" style="DISPLAY:none"></asp:TextBox>
				    </td>
			    </tr>
			    <tr>
				    <td colspan="2"><asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label></td>
			    </tr>
			    <tr>
				    <td colspan="2">
                    <Grd:RibesGridView ID="GrdDocumenti" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical"  Width="600px"
                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
						    <Columns>
							    <asp:BoundField DataField="name" SortExpression="name" HeaderText="Nome Documento">
								    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
							    </asp:BoundField>
							    <asp:TemplateField>
								    <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Center"></ItemStyle>
								    <ItemTemplate>
									    <A href='<%# DataBinder.Eval(Container, "DataItem.url") %>' target="_blank">Apri</A>
								    </ItemTemplate>
								    <EditItemTemplate>
									    <A href='<%# DataBinder.Eval(Container, "DataItem.url") %>' target="_blank">Apri</A>
								    </EditItemTemplate>
							    </asp:TemplateField>
						    </Columns>
					    </Grd:RibesGridView>
				    </td>
			    </tr>
			</table>
			<asp:TextBox ID="TxtProvenienzaForm" style="DISPLAY:none" Runat="server"></asp:TextBox>
		</form>
	</body>
</HTML>
