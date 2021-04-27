<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaStrade.aspx.vb" Inherits="OpenGovTerritorio.RicercaStrade" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Configurazione Stradario</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">			
			if (document.layers)
				document.captureEvents(Event.KEYDOWN);
				document.onkeydown =
					function (evt)
					{
						var keyCode = evt ? (evt.which ? evt.which : evt.keyCode) : event.keyCode;
						if (keyCode == 13) // for enter
						{ 
						    document.getElementById('Search').click();
						}
						else
							return true;
					};
			function LoadStrade(CodStrada)//grid)
			{
				//var item = grid.getSelectedItem();
				//var CodStrada= item.getCell(0).getValue();
				parent.parent.Visualizza.location.href='InsertStrade.aspx?CODSTRADA='+CodStrada
				parent.parent.Comandi.location.href='ComandiInserimento.aspx';
			}
			function VisualizzaBottoni(){
			
				/*parent.Comandi.Search.style.display=''
				parent.Comandi.Search.title='Ricerca Strade'
				parent.Comandi.NewInsert.style.display=''
				parent.Comandi.NewInsert.title='Nuovo Inserimento'
				parent.Comandi.Insert.style.display='none'
				parent.Comandi.Delete.style.display='none'
				parent.Comandi.Modify.style.display='none'
				parent.Comandi.Cancel.style.display='none'
				parent.Comandi.Unlock.style.display='none'
				parent.Comandi.Clear.style.display='none'*/
										
				//parent.Comandi.info.innerText="<%=Session("DESC_TIPO_PROC_SERV")%>"
				document.getElementById('infoEnte').innerText="<%=session("DESCRIZIONE_ENTE")%>"
				parent.Comandi.info.innerText="Configurazione  -  " + "<%=Session("DESC_TIPO_PROC_SERV") %>" + '  -  Ricerca' // + "<%=session("DESCRIZIONE_ENTE")%>"
			}
			function keyPress()
			{
				if (window.event.keyCode==13)
				{
					Search();
				}
			}	
			function Search()
			{						
			    document.getElementById('Search').click()
			}			
		</script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="3" rightMargin="3">
		<form id="Form1" runat="server" method="post">
            <div class="col-md-12">
			    <table id="Table1" cellSpacing="0" cellPadding="0" border="0" width="95%">
				    <tr>
					    <td>
						    <fieldset class="FiledSetRicerca" style="WIDTH: 100%">
							    <legend class="Legend">Inserimento filtri di ricerca</legend>
							    <table width="100%">
								    <tr>
									    <td>
										    <asp:label id="Label7" runat="server" CssClass="Input_Label">Toponimo</asp:label><br>
										    <asp:dropdownlist id="ddlToponimo" tabIndex="1" runat="server" Width="180px" CssClass="Input_Text"></asp:dropdownlist>
									    </td>
									    <td>
										    <asp:label id="Label9" runat="server" CssClass="Input_Label">Frazione</asp:label><br>
										    <asp:dropdownlist id="ddlFrazione" tabIndex="2" runat="server" Width="180px" CssClass="Input_Text"></asp:dropdownlist>
									    </td>
								    </tr>
								    <tr>
									    <td>
										    <asp:label id="Label11" runat="server" CssClass="Input_Label">Denominazione</asp:label><br>
										    <asp:textbox id="TxtDenominazione" tabIndex="3" runat="server" Width="350px" CssClass="Input_Text"></asp:textbox>
									    </td>
									    <td>
										    <asp:label id="Label12" runat="server" CssClass="Input_Label">Cap</asp:label><br>
										    <asp:textbox id="TxtCAP" tabIndex="4" runat="server" Width="87px" CssClass="Input_Text" MaxLength="10"></asp:textbox>
									    </td>
								    </tr>
							    </table>
						    </fieldset>
					    </td>
				    </tr>
				    <tr>
					    <td>
	                        <Grd:RibesGridView id="GrdStrade" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="15"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                <Columns>
								    <asp:BoundField HeaderText="Toponimo" DataField="TOPONIMO"></asp:BoundField>
								    <asp:BoundField HeaderText="Descrizione" DataField="DESCRIZIONE_VIA"></asp:BoundField>
								    <asp:BoundField HeaderText="Frazione" DataField="FRAZIONE"></asp:BoundField>
								    <asp:BoundField HeaderText="Cap" DataField="CAP"></asp:BoundField>
								    <asp:BoundField HeaderText="Lunghezza" DataField="LUNGHEZZA"></asp:BoundField>
								    <asp:BoundField HeaderText="Larghezza" DataField="LARGHEZZA"></asp:BoundField>
								    <asp:BoundField HeaderText="Note" DataField="NOTE"></asp:BoundField>
                                    <asp:TemplateField HeaderText="">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID_VIA") %>' alt=""></asp:ImageButton>
                                            <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("ID_VIA") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
	                        </Grd:RibesGridView>
					    </td>
				    </tr>
			    </table>
            </div>
			<asp:button id="Search" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="NewInsert" style="DISPLAY: none" runat="server"></asp:button>
			<INPUT id="TestValidita" type="hidden" size="1" value="0" name="TestValidita">&nbsp;&nbsp;&nbsp;
			<INPUT id="hiddenSearch" type="hidden" size="1" name="hiddenSearch">
		</form>
	</body>
</html>
