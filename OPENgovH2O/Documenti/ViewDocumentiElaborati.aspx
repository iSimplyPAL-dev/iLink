<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ViewDocumentiElaborati.aspx.vb" Inherits="OpenUtenze.ViewDocumentiElaborati"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ViewDocumentiElaborati</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function LoadElaborazioneDocumenti(IdRuolo, Anno, DataCartellazione, TipoRuolo, IdContribuente)
    		{    			
				parent.parent.Comandi.location.href='../Documenti/ComandiDocumenti.aspx'
				parent.parent.Visualizza.location.href='../Documenti/RicercaDocumenti.aspx?IdRuolo='+IdRuolo + '&Anno=' + Anno + '&DataCartellazione=' + DataCartellazione + '&TipoRuolo=' + TipoRuolo
    		}
			function CloseMe() {
			    parent.document.getElementById('divStampa').style.display = 'none';
			    parent.document.getElementById('divDettaglio').style.display = '';
			}
		</script>
	</HEAD>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
            <div id="divComandiera" class="col-md-12">
                <table width="100%">
				    <tr>
					    <td align="left">
						    <span id="title" class="lstTabRow" style="HEIGHT:20px">Stampa</span>
					    </td>
					    <td align="right">
						    <input class="Bottone Bottoneannulla" id="Esci" title="Esci" onclick="CloseMe();" type="button" name="Esci">
					    </td>
				    </tr>
                </table>
            </div>
            <div id="divRuolo" class="col-md-12">
                <table width="100%">
				    <tr>
					    <td>
						    <table id="TblDatiContribuente" cellSpacing="0" cellPadding="0" width="100%" class="dati_anagrafe_tarsu_blu" border="1">
							    <tr>
								    <td>
									    <table id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
									        <tr>
										        <td class="Input_Label" colSpan="6" height="20"><strong>DATI RUOLO</strong></td>
									        </tr>
									        <tr>
										        <td class="DettagliContribuente">Ruolo</td>
										        <td>&nbsp;</td>
										        <td class="DettagliContribuente">relativo all'anno</td>
										        <td><asp:label id="lblAnnoRuolo" runat="server" CssClass="DettagliContribuente">Label</asp:label></td>
										        <td class="DettagliContribuente">Elaborato in data</td>
										        <td><asp:label id="lblDataCartellazione" runat="server" CssClass="DettagliContribuente">Label</asp:label></td>
									        </tr>
									        <tr>
										        <td colspan="4">
										            <asp:label id="lblDocElaborati" CssClass="DettagliContribuente" Width="100%" Runat="server">Documenti Effettivi già elaborati:     </asp:label>
										        </td>
										        <td colspan="2">
										            <asp:label id="lblDocDaElaborare" CssClass="DettagliContribuente" Width="100%" Runat="server">Documenti da elaborare:     </asp:label>
										        </td>														
									        </tr>
								        </table>
							        </td>
						        </tr>
					        </table>
					    </td>
				    </tr>
                </table>
            </div>
            <div class="col-md-12">
                <table width="100%">
				    <tr>
					    <td><asp:label id="Label1" runat="server" CssClass="Legend">Elenco Documenti Elaborati</asp:label></td>
				    </tr>
				    <tr>
					    <td><asp:label id="lblMessage" runat="server" Cssclass="NormalBold"></asp:label></td>
				    </tr>
				    <tr>
					    <td>
                            <Grd:RibesGridView ID="GrdDocumenti" runat="server" BorderStyle="None" 
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
									    <HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
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
            </div>
		</form>
	</body>
</HTML>
