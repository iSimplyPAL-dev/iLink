<%@ Page language="c#" Codebehind="DownloadDocumenti.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.ElaborazioneDocumenti.DownloadDocumenti" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<head>
		<title>DownloadDocumenti</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="C#"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
        <script type="text/javascript">
            function CloseMe()
            {
                parent.document.getElementById('divStampa').style.display = 'none'; 
                if('<% = Request["Provenienza"].ToString()%>'=='E'){
                    parent.document.getElementById('divElabDoc').style.display = '';
                }else{
                    parent.document.getElementById('DivCalcolo').style.display = '';
                }
            }
        </script>
	</head>
	<body class="SfondoVisualizza">
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
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
			            <Grd:RibesGridView ID="GrdDaElaborare" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
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
</HTML>
