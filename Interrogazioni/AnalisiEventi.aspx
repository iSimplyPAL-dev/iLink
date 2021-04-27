<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AnalisiEventi.aspx.vb" Inherits="OPENgov.AnalisiEventi" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<link href="../Styles.css" type="text/css" rel="stylesheet">
	<%if Session("SOLA_LETTURA")="1" then%>
	<link href="../solalettura.css" type="text/css" rel="stylesheet">
	<%end If%>
	<script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
    <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
    <script language="javascript" type="text/javascript">
        function LoadSearch() {
            DivAttesa.style.display = '';
            $('#btnSearch').click();
        }
		function keyPress()
		{
			if(window.event.keyCode==13)
			{
			    LoadSearch();
			}
		}
    </script>
</head>
<body>
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
                        <input class="Bottone BottoneGrafico" runat="server" id="Chart" title="Visualizza dati aggregati" onclick="$('#DivAttesa').show(); $('#btnChart').click();" type="button" name="Chart" />
                        <input class="Bottone BottoneRicerca" runat="server" id="Search" title="Ricerca" onclick="LoadSearch();" type="button" />
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 463px" align="left">
                        <span class="NormalBold_title" id="info" style="width: 400px; height: 20px">Analisi Eventi</span>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <fieldset class="classeFiledSetRicerca">
                <legend class="Legend">Inserimento filtri di Ricerca</legend>
                <div class="col-md-12">
				    <div class="col-md-3">
					    <asp:Label id="Label6" CssClass="Input_Label" Runat="server">Tributo</asp:Label><br />
					    <asp:dropdownlist id="DdlTributo" runat="server" CssClass="Input_Text"></asp:dropdownlist>
				    </div>
				    <div class="col-md-4">
					    <asp:Label Runat="server" CssClass="Input_Label" id="Label7">Operatore</asp:Label><br />
					    <asp:DropDownList id="DdlOperatore" Runat="server" CssClass="Input_Label col-md-11"></asp:DropDownList>
				    </div>
                    <div class="col-md-4">
                        <asp:Label runat="server" CssClass="Input_Label">Causali</asp:Label><br />
                        <asp:DropDownList ID="DdlCausali" runat="server" CssClass="Input_Label col-md-11"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-12">
				    <div class="col-md-3">
					    <asp:Label Runat="server" CssClass="Input_Label" id="Label4">Dal</asp:Label><br />
					    <asp:TextBox id="TxtDal" Runat="server" CssClass="Input_Text_Right TextDate" maxlength="10" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
				    </div>
				    <div class="col-md-3">
					    <asp:Label Runat="server" CssClass="Input_Label" id="Label5">Al</asp:Label><br />
					    <asp:TextBox id="TxtAl" Runat="server" CssClass="Input_Text_Right TextDate" maxlength="10" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
				    </div>
                    <div class="col-md-4">
                        <fieldset class="classeFiledSetRicerca">
                            <legend class="Input_Label_title">Raggruppato per</legend>
                            <div class="col-md-11">
                                <asp:Label runat="server" CssClass="Input_Label col-md-1">Q.tà</asp:Label>&nbsp;
                                <asp:TextBox runat="server" ID="txtQta" CssClass="Input_Text_Right OnlyNumber col-md-1" onkeypress="return NumbersOnly(event,true,false,0)">1</asp:TextBox>&nbsp;
                                <asp:Label runat="server" CssClass="Input_Label col-md-1">Tipo</asp:Label>&nbsp;
                                <asp:DropDownList runat="server" id="ddlTipoQta" CssClass="Input_Text col-md-5"></asp:DropDownList>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </fieldset>
            <div id="divResult">
                <asp:label id="LblResult" CssClass="Legend" Runat="server"></asp:label>
                <Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None"
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <PagerStyle CssClass="CartListFooter" />
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
						<asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Tributo"></asp:BoundField>
						<asp:BoundField DataField="CAUSALE" HeaderText="Causale"></asp:BoundField>
						<asp:BoundField DataField="DATA_EVENTO" HeaderText="Data"></asp:BoundField>
						<asp:BoundField DataField="USER_EVENTO" HeaderText="Operatore"></asp:BoundField>
                    </Columns>
                </Grd:RibesGridView>
            </div>
            <div id="divChart"></div>
        </div>
        <div id="DivAttesa" runat="server" style="z-index: 103; position: absolute;display:none;">
            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
            <div class="BottoneClessidra">&nbsp;</div>
            <div class="Legend">Attendere Prego</div>
        </div>
        <asp:Button ID="btnPrint" runat="server" CssClass="hidden"></asp:Button>
        <asp:Button ID="btnChart" runat="server" CssClass="hidden"></asp:Button>
        <asp:Button ID="btnSearch" runat="server" CssClass="hidden"></asp:Button>
    </form>
</body>
</html>
