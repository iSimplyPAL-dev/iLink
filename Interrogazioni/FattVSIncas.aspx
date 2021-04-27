<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FattVSIncas.aspx.vb" Inherits="OPENgov.FattVSIncas" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<%@ Register TagPrefix="Web" Namespace="WebChart" Assembly="WebChart" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
    <script type="text/javascript" language="Javascript" src="../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js?newversion"></script>
    <script type="text/javascript">
        function nascondi(chiamante, oggetto) {
            document.getElementById(oggetto).style.display = "none"
        }
    </script>
    <title></title>
</head>
<body leftmargin="3" topmargin="3">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" align="right" style="width: 100%; height: 45px">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table2">
                <tr>
                    <td style="width: 464px; height: 20px" align="left">
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" width="800" colspan="2" rowspan="2">
                        <input class="Bottone BottoneExcel" runat="server" id="Excel" title="Stampa in formato Excel" onclick="document.getElementById('DivAttesa').style.display = ''; document.getElementById('btnStampaExcel').click();" type="button" name="Excel" />
                        <input class="Bottone BottoneGrafico" runat="server" id="Grafico" title="Grafico insoluti" onclick="document.getElementById('DivAttesa').style.display = ''; document.getElementById('btnGrafico').click();" type="button" name="Grafico" />
                        <input class="Bottone BottoneRicerca" runat="server" id="Ricerca" title="Ricerca" onclick="document.getElementById('DivAttesa').style.display = ''; document.getElementById('btnRicerca').click();" type="button" name="Search" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 463px" align="left">
                        <span id="info" runat="server" class="NormalBold_title" style="width: 400px; height: 20px">Analisi</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="mytestchart"></div>
        <div>
            <table cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td>
                        <fieldset class="classeFiledSetRicerca">
                            <legend class="Legend">Inserimento filtri di Ricerca</legend>
                            <table cellspacing="1" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td class="Input_Label">
                                        <asp:Label ID="lblEnti" runat="server" CssClass="Input_Label">Ente</asp:Label><br />
                                        <asp:DropDownList ID="ddlEnti" runat="server" CssClass="Input_Label" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                    <td class="Input_Label">
                                        <asp:Label ID="lblAnno" runat="server" CssClass="Input_Label">Anno</asp:Label><br />
                                        <asp:TextBox ID="txtAnno" runat="server" MaxLength="4" CssClass="Input_Text_Right OnlyNumber" Width="60px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">
                                        <asp:Label ID="lblParam3" runat="server" CssClass="Input_Label">Param3</asp:Label><br />
                                        <asp:DropDownList ID="ddlParam3" runat="server" CssClass="Input_Label"></asp:DropDownList>
                                        <asp:TextBox ID="txtParam3" runat="server" CssClass="Input_Text_Right" Width="80px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">
                                        <asp:Label ID="lblDal" runat="server" CssClass="Input_Label">Pagamenti Dal</asp:Label><br />
                                        <asp:TextBox ID="txtDal" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label">
                                        <asp:Label ID="lblAl" runat="server" CssClass="Input_Label">Al</asp:Label><br />
                                        <asp:TextBox ID="txtAl" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label" id="optICI">
                                        <asp:RadioButton ID="optICIIMU" runat="server" CssClass="Input_Radio" GroupName="optICI" Text="ICI/IMU" Checked="true" />&nbsp;
                                        <asp:RadioButton ID="optICITASI" runat="server" CssClass="Input_Radio" GroupName="optICI" Text="TASI" />&nbsp;
                                    </td>
                                    <td class="Input_Label" id="optICIRata">
                                        <asp:RadioButton ID="optAcconto" runat="server" CssClass="Input_Radio" GroupName="ICIRata" Text="Acconto" />&nbsp;
                                        <asp:RadioButton ID="optSaldo" runat="server" CssClass="Input_Radio" GroupName="ICIRata" Text="Saldo" />&nbsp;
                                        <asp:RadioButton ID="optAll" runat="server" CssClass="Input_Radio" GroupName="ICIRata" Text="Tutto" Checked="true" />&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
                        <div id="DivGrafico" runat="server" style="display: none">
                            <a title="Chiudi Grafico" onclick="nascondi(this,'DivGrafico')" href="#" class="lstTabRow" style="width: 100%">Chiudi Grafico</a><br />
                            <div id="chart_div"></div>
                            <!--<Web:ChartControl id="ColChartAnalisi" runat="Server" ChartPadding="30" BottomChartPadding="20"
                                TopPadding="20" Padding="20" Width="600" Height="450" BorderStyle="None" GridLines="Both" 
                                Legend-Position="Bottom" Legend-Width="30">
                                <Background Type="Solid" Color="Transparent" EndPoint="900,900"></Background>
                                <ChartTitle StringFormat="Center,Near,Character,LineLimit" ForeColor="Black"></ChartTitle>
                                <YTitle StringFormat="Center,Near,Character,LineLimit"></YTitle>
                                <YAxisFont StringFormat="Far,Near,Character,LineLimit"></YAxisFont>
                                <XTitle StringFormat="Center,Near,Character,LineLimit"></XTitle>
                                <XAxisFont StringFormat="Center,Near,Character,LineLimit"></XAxisFont>
                            </Web:ChartControl>-->
                        </div>
                    </td>
                </tr>
				<tr>
					<td>
						<asp:label id="LblResult" CssClass="Legend" Runat="server"></asp:label>
					</td>
				</tr>
                <tr>
                    <td>
                        <div id="divTARSU">
                            <asp:Label ID="LblTARSUEmesso" CssClass="Legend" runat="server">Dati Ruolo</asp:Label><br />
                            <Grd:RibesGridView ID="GrdTARSUEmesso" runat="server" BorderStyle="None"
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdRowCommand">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="Ente" HeaderText="Ente"></asp:BoundField>
                                    <asp:BoundField DataField="ANNO" HeaderText="Anno"></asp:BoundField>
                                    <asp:BoundField DataField="articoli" HeaderText="Articoli" DataFormatString="{0:0}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="avvisi" HeaderText="Avvisi" DataFormatString="{0:0}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="contribuenti" HeaderText="Contribuenti" DataFormatString="{0:0}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fissa" HeaderText="Imp. Fissa" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="variabile" HeaderText="Imp. Variabile" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="conferimenti" HeaderText="Imp. Conferimenti" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="addizionali" HeaderText="Imp. Addiz." DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="maggiorazione" HeaderText="Imp. Magg." DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="totale" HeaderText="Imp. Totale" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneGrafico" CommandName="RowOpen" CommandArgument='<%# Eval("ente") %>' alt=""></asp:ImageButton>
                                            <asp:HiddenField runat="server" ID="hfsgravi" Value='<%# Eval("sgravi") %>' />
                                            <asp:HiddenField runat="server" ID="hfincassato" Value='<%# Eval("incassato") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Grd:RibesGridView>
                            <asp:Label ID="LblTARSUNetto" CssClass="Legend" runat="server">Dati Sgravi</asp:Label><br />
                            <Grd:RibesGridView ID="GrdTARSUNetto" runat="server" BorderStyle="None"
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="Ente" HeaderText="Ente"></asp:BoundField>
                                    <asp:BoundField DataField="ANNO" HeaderText="Anno"></asp:BoundField>
                                    <asp:BoundField DataField="articoli" HeaderText="Articoli" DataFormatString="{0:0}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="avvisi" HeaderText="Avvisi" DataFormatString="{0:0}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="contribuenti" HeaderText="Contribuenti" DataFormatString="{0:0}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fissa" HeaderText="Imp. Fissa" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="variabile" HeaderText="Imp. Variabile" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="conferimenti" HeaderText="Imp. Conferimenti" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="addizionali" HeaderText="Imp. Addiz." DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="maggiorazione" HeaderText="Imp. Magg." DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="totale" HeaderText="Imp. Totale" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                            </Grd:RibesGridView>
                            <asp:Label ID="LblTARSUIncassato" CssClass="Legend" runat="server">Dati Incassato</asp:Label><br />
                            <Grd:RibesGridView ID="GrdTARSUIncassato" runat="server" BorderStyle="None"
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdRowCommand">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="Ente" HeaderText="Ente"></asp:BoundField>
                                    <asp:BoundField DataField="ANNO" HeaderText="Anno"></asp:BoundField>
                                    <asp:BoundField DataField="npagamenti" HeaderText="Pagamenti" DataFormatString="{0:0}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="imponibile" HeaderText="Imp. Ruolo" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="addizionali" HeaderText="Imp. Addiz." DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="maggiorazione" HeaderText="Imp. Magg." DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="pagato" HeaderText="Incassato" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="insoluto" HeaderText="Insoluto" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="percinsoluto" HeaderText="% Insoluto" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                            </Grd:RibesGridView>
                        </div>
                        <div id="divICI">
                            <asp:Label ID="LblICIIncassato" CssClass="Legend" runat="server">Dati Incassato</asp:Label><br />
                            <Grd:RibesGridView ID="GrdICIIncassato" runat="server" BorderStyle="None"
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdRowCommand">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="Ente" HeaderText="Ente"></asp:BoundField>
                                    <asp:BoundField DataField="Anno" HeaderText="Anno">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Abi_Princ" HeaderText="Abi. Princ. (3912-3958)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Altri_Fab" HeaderText="Altri Fab. (3918-3961)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Altri_Fab_Stato" HeaderText="Altri Fab. Stato (3919)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Aree_Fab" HeaderText="Aree Fab. (3916-3960)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Aree_Fab_Stato" HeaderText="Aree Fab. Stato (3917)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Terreni" HeaderText="Terreni (3914)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Terreni_Stato" HeaderText="Terreni Stato (3915)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_fabrurusostrum" HeaderText="Fab.Rur. (3913-3959)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_fabrurusostrum_stato" HeaderText="Fab.Rur. Stato (3919)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_UsoProdCatD" HeaderText="Uso Prod.Cat.D (3930)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_UsoProdCatD_stato" HeaderText="Uso Prod.Cat.D Stato (3925)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Detrazione" HeaderText="Detraz." DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_RavOper" HeaderText="Rav. Operoso" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Totale" HeaderText="Incassato" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Dovuto" HeaderText="Calcolato" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneGrafico" CommandName="RowOpen" CommandArgument='<%# Eval("Ente") %>' alt=""></asp:ImageButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Grd:RibesGridView>
                            <asp:Label ID="LblICIEmesso" CssClass="Legend" runat="server">Dati Dovuto</asp:Label><br />
                            <Grd:RibesGridView ID="GrdICIEmesso" runat="server" BorderStyle="None"
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdRowCommand">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="Ente" HeaderText="Ente"></asp:BoundField>
                                    <asp:BoundField DataField="Anno" HeaderText="Anno">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CAT" HeaderText="Cat."></asp:BoundField>
                                    <asp:BoundField DataField="num_fabbricati" HeaderText="N. Immobili"></asp:BoundField>
                                    <asp:BoundField DataField="Imp_Abi_Princ" HeaderText="Abi. Princ. (3912-3958)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Altri_Fab" HeaderText="Altri Fab. (3918-3961)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Altri_Fab_Stato" HeaderText="Altri Fab. Stato (3919)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Aree_Fab" HeaderText="Aree Fab. (3916-3960)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Aree_Fab_Stato" HeaderText="Aree Fab. Stato (3917)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Terreni" HeaderText="Terreni (3914)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_Terreni_Stato" HeaderText="Terreni Stato (3915)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_fabrurusostrum" HeaderText="Fab.Rur. (3913-3959)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_fabrurusostrum_stato" HeaderText="Fab.Rur. Stato (3919)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_UsoProdCatD" HeaderText="Uso Prod.Cat.D (3930)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Imp_UsoProdCatD_stato" HeaderText="Uso Prod.Cat.D Stato (3925)" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Detrazione" HeaderText="Detraz." DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Totale" HeaderText="Dovuto" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                            </Grd:RibesGridView>
                        </div>
                        <div id="divH2O">
                            <asp:Label ID="LblH2OEmesso" CssClass="Legend" runat="server">Dati Ruolo</asp:Label><br />
                            <Grd:RibesGridView ID="GrdH2OEmesso" runat="server" BorderStyle="None"
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdRowCommand">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="Ente" HeaderText="Ente"></asp:BoundField>
                                    <asp:BoundField DataField="ANNO" HeaderText="Anno"></asp:BoundField>
                                    <asp:BoundField DataField="ncontatori" HeaderText="Contatori" DataFormatString="{0:0}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="nfatture" HeaderText="Fatture" DataFormatString="{0:0}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="nnote" HeaderText="note" DataFormatString="{0:0}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="impfatture" HeaderText="Imp. Fatture" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="impnote" HeaderText="Imp. Note" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="totale" HeaderText="Imp. Totale" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneGrafico" CommandName="RowOpen" CommandArgument='<%# Eval("Ente") %>' alt=""></asp:ImageButton>
                                            <asp:HiddenField runat="server" ID="hfincassato" Value='<%# Eval("incassato") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Grd:RibesGridView>
                            <asp:Label ID="LblH2OIncassato" CssClass="Legend" runat="server">Dati Incassato</asp:Label><br />
                            <Grd:RibesGridView ID="GrdH2OIncassato" runat="server" BorderStyle="None"
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdRowCommand">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="Ente" HeaderText="Ente"></asp:BoundField>
                                    <asp:BoundField DataField="ANNO" HeaderText="Anno"></asp:BoundField>
                                    <asp:BoundField DataField="npagamenti" HeaderText="Pagamenti" DataFormatString="{0:0}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="incassato" HeaderText="Incassato" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="insoluto" HeaderText="Insoluto" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="percinsoluto" HeaderText="% Insoluto" DataFormatString="{0:N}">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                            </Grd:RibesGridView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
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
        <asp:Button ID="btnStampaExcel" Style="display: none" runat="server" Text="Button"></asp:Button>
        <asp:Button ID="btnGrafico" Style="display: none" runat="server" Text="Button"></asp:Button>
        <asp:Button ID="btnGraficoEnte" Style="display: none" runat="server" Text="Button"></asp:Button>
        <asp:Button ID="btnRicerca" Style="display: none" runat="server" Text="Button"></asp:Button>
    </form>
</body>
</html>
