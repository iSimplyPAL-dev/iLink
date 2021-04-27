<%@ Page language="c#" Codebehind="ViewDocElaborati.aspx.cs" AutoEventWireup="True" Inherits="OPENgovTOCO.Elaborazioni.Documenti.ViewDocElaborati" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
  <head>
    <title>ViewDocElaborati</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	<script type="text/javascript">
    	function downloadAll(){
    		
    		/*var strpath=prompt ("Inserire il percorso dove salvare i file","c:\\")
    		if (strpath!=""){
    			document.getElementById ("txtPath").value=strpath*/
    			document.getElementById ("btnDownloadAll").click()
    		/*}else{
    			alert ("Non   stato inserito il percorso dove salvare i file")
    		}*/
    	}
        function CloseMe()
        {
            parent.document.getElementById('divStampa').style.display = 'none'; 
            parent.document.getElementById('divVisual').style.display = '';
        }
	</script>
</HEAD>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table style="Z-INDEX: 100; LEFT: 0px; POSITION: absolute; TOP: 0px" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td align="left">
						<span id="title" class="lstTabRow" style="HEIGHT:20px">Stampa</span>
					</td>
					<td align="right">
						<input class="Bottone Bottoneannulla" id="Esci" title="Esci" onclick="CloseMe();" type="button" name="Esci">
					</td>
				</tr>
				<TR>
					<TD><asp:label id="Label1" runat="server" CssClass="Legend">Elenco Documenti Elaborati</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label></TD>
				</TR>
				<TR>
					<TD>
						<asp:label id="lblDownloadAll" runat="server" CssClass="Link_Label" onclick="downloadAll()">Scarica tutti i file</asp:label>
						<asp:Button id="btnDownloadAll" runat="server" Text="Scarica tutti i file" style="DISPLAY:none"></asp:Button>
						<asp:TextBox id="txtPath" runat="server" style="DISPLAY:none"></asp:TextBox>								
					</TD>
				</TR>
				<TR>
					<TD>
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
                    </TD>
				</TR>
			</table>
            <div id="divDialogBox" class="col-md-12">
                <div class="modal-box">
                    <div id="divAlert" class="modal-alert">
                        <span class="closebtnalert" onclick="CloseAlert()">&times;</span>
                        <p id="pAlert">testo di esempio</p>
                        <input type="text" class="prompttxt"/>
                        <button id="btnDialogBoxeOK" class="confirmbtn Bottone BottoneOK" onclick="DialogConfirmOK();"></button>&nbsp;
                        <button id="btnDialogBoxeKO" class="confirmbtn Bottone BottoneKO" onclick="DialogConfirmKO();"></button>
                        <button id="CmdDialogConfirmOK" class="hidden" onclick="RaiseDialogConfirmOK();"></button>
                        <button id="CmdDialogConfirmKO" class="hidden" onclick="RaiseDialogConfirmKO();"></button>
                        <input type="hidden" id="hfCloseAlert" />
                        <input type="hidden" id="hfDialogOK" />
                        <input type="hidden" id="hfDialogKO" />
                    </div>
                </div>
                <input type="hidden" id="cmdHeight" value="0" />
            </div>
		</form>
	</body>
</html>
