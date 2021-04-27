<%@ Page Language="c#" CodeBehind="GestioneDettaglio.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.GestioneDettaglio" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>GestioneDettaglio</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%> 
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%> 
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
        function gotoControlloPreAccertamentoContribuente() {
            var Parametri = null;
            Parametri = "";
            Parametri = "?IDPaginaBack=CGL";
            /*Parametri=Parametri+ "?CHIAMATADAICI=1";*/

            document.location.href = "../Provvedimenti/GestioneLiquidazioni/GestioneElaborazione.aspx" + Parametri;
        }

        function gotoCalcoloICIpuntualeContribuente() {
            COD_CONTRIB = document.getElementById('hdIdContribuente').value;
            document.location.href = "./CalcoloICI/CalcoloICIPuntuale.aspx?COD_CONTRIB=" + COD_CONTRIB;
        }

        function SelCelle(idTestata) {
            document.getElementById('txtIdTestata').value = idTestata;
            //alert(idTestata);
            document.getElementById('btnSelCella').click();
        }

        var dcTime = 250;    // doubleclick time
        var dcDelay = 100;   // no clicks after doubleclick
        var dcAt = 0;        // time of doubleclick
        var savEvent = null; // save Event for handling doClick().
        var savEvtTime = 0;  // save time of click event.
        var savTO = null;    // handle of click setTimeOut

        function showMe(form, txt) {

        }

        function hadDoubleClick() {
            var d = new Date();
            var now = d.getTime();
            //showMe(1, "Checking DC (" + now + " - " + dcAt);
            if ((now - dcAt) < dcDelay) {
                //showMe(1, "*hadDC*");
                return true;
            }
            //showMe(1, " OK ");
            return false;
        }

        function handleWisely(which, unoclick, dueclick) {
            //alert(unoclick + ' ' + dueclick)
            //showMe(1, which + " fired...");
            switch (which) {
                case "click":
                    // If we've just had a doubleclick then ignore it
                    if (hadDoubleClick()) return false;

                    // Otherwise set timer to act.  It may be preempted by a doubleclick.
                    savEvent = which;
                    d = new Date();
                    savEvtTime = d.getTime();
                    savTO = setTimeout("doClick(savEvent,'" + unoclick + "')", dcTime);
                    break;
                case "dblclick":
                    doDoubleClick(which, dueclick);
                    break;
                default:
            }
        }

        function doClick(which, unoclick) {
            // preempt if DC occurred after original click.
            if (savEvtTime - dcAt <= 0) {
                //showMe(1, "ignore Click");
                return false;
            }
            // quando si verifica il click
            //showMe(1, "Handle Click.  ");
            //alert(unoclick);
            SelCelle(unoclick)
        }

        function doDoubleClick(which, dueclick) {
            var d = new Date();
            dcAt = d.getTime();
            if (savTO != null) {
                clearTimeout(savTO);          // Clear pending Click  
                savTO = null;
            }
            // quando si verifica il doppio click
            //showMe(1, "Handle DoubleClick at " + dcAt);
            //alert(dueclick +':mouseclick','');
            __doPostBack(dueclick + ':mouseclick', '');

        }
        //*** 20131003 - gestione atti compravendita ***
        function nascondi(chiamante, oggetto, label) {
            if (document.getElementById(oggetto).style.display == "") {
                document.getElementById(oggetto).style.display = "none"
                chiamante.title = "Visualizza '" + label + "'"
                chiamante.innerText = "Visualizza '" + label + "' >>"
            } else {
                document.getElementById(oggetto).style.display = ""
                chiamante.title = "Nascondi '" + label + "'"
                chiamante.innerText = "<< Nascondi '" + label + "'"
            }
        }
         //*** 20131018 - DOCFA ***
        function ApriDocumentoPDF(percorso) {
            winWidth = 1000
            winHeight = 700
            myleft = (screen.width - winWidth) / 2
            mytop = (screen.height - winHeight) / 2 - 40
            caratteristiche = "toolbar=no,status,left=" + myleft + ",top=" + mytop + ",height =" + winHeight + ",width=" + winWidth + ",resizable"
            window.open(percorso, "", caratteristiche);
        }
    </script>
</head>
<body class="Sfondo" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post">
        <table id="tblCorpo" cellspacing="1" cellpadding="1" width="100%" align="center" border="0">
            <!--blocco dati contribuente-->
            <%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
            <tr id="TRPlainAnag">
                <td>
                    <iframe id="ifrmAnag" runat="server" src="../aspVuota.aspx" style="height: 200px; width: 100%" frameborder="0" marginheight="0"></iframe>
                    <asp:HiddenField ID="hdIdContribuente" runat="server" Value="-1" />
                </td>
            </tr>
            <tr id="TRSpecAnag">
                <td>
                    <table id="tblDatiContribuente" cellspacing="0" cellpadding="0" width="100%" border="1">
                        <tr>
                            <td bordercolor="darkblue">
                                <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                                    <tr>
                                        <td class="Input_Label" colspan="4" height="20"><strong>DATI CONTRIBUENTE</strong></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente" width="160">Cognome/Rag. Sociale</td>
                                        <td class="DettagliContribuente" width="25%">
                                            <asp:Label ID="lblCognome" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente" width="110">Nome</td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="lblNome" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente">Data di Nascita</td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="lblDataNascita" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente">RESIDENTE IN</td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="lblIndirizzo" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente">Comune (Prov.)</td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="lblComune" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="20">
                    <asp:Button ID="btnIndietro" Style="display: none" runat="server" Text="Indietro" OnClick="btnIndietro_Click"></asp:Button></td>
            </tr>
            <!--*** 20131003 - gestione atti compravendita ***-->
            <tr>
                <td>
                    <div class="divricerca">
                        <a id="VisCompraVendita" runat="server" title="Visualizza Atti di Compra Vendita" onclick="nascondi(this,'CompraVendite','Atti di Compra Vendita')" href="#">&lt;&lt; Visualizza Atti di Compra Vendita</a>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="CompraVendite" runat="server" style="display: none">
                        <Grd:RibesGridView ID="GrdCompraVendite" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="RIFCATASTALI" HeaderText="Fg – Num - Sub"></asp:BoundField>
                                <asp:BoundField DataField="UBICAZIONE" HeaderText="Ubicazione"></asp:BoundField>
                                <asp:BoundField DataField="DATAVALIDITA" HeaderText="Data Validità">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DATAPRESENTAZIONE" HeaderText="Data Presentazione">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="NUMNOTA" HeaderText="N. Nota">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRATTO" HeaderText="Descrizione Atto"></asp:BoundField>
                                <asp:BoundField DataField="DIRITTO" HeaderText="Diritto"></asp:BoundField>
                                <asp:BoundField DataField="TITOLARITA" HeaderText="Titolarità"></asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="AttoCompraVendita" style="display: none" runat="server">
                        <div class="lstTab">
                            <fieldset class="bordoIFRAME">
                                <legend class="Legend">Compravendita - nota trascrizione</legend>
                                &nbsp;<asp:Label ID="lblNotaTrascrizione" runat="server" Text="" CssClass="Input_Label"></asp:Label>
                            </fieldset>
                            <fieldset class="bordoIFRAME">
                                <legend class="Legend">Immobile in nota</legend>
                                <p>&nbsp;<asp:Label ID="lblRifNota" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
                                <p>&nbsp;<asp:Label ID="lblCatNota" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
                                <p>
                                    &nbsp;<asp:Label ID="lblUbicazioneNota" runat="server" Text="" CssClass="Input_Label"></asp:Label>
                                    &nbsp;<asp:Label ID="lblUbicazioneCatasto" runat="server" Text="" CssClass="Input_Label"></asp:Label>
                                </p>
                            </fieldset>
                            <fieldset class="bordoIFRAME">
                                <legend class="Legend">Soggetto in nota</legend>
                                &nbsp;<asp:Label ID="lblSoggettoNota" runat="server" Text="" CssClass="Input_Label"></asp:Label>
                            </fieldset>
                        </div>
                    </div>
                </td>
            </tr>
            <!--*** ***-->
            <tr>
                <td>
                    <fieldset class="classeFiledSet">
                        <legend class="Legend">Dichiarazioni</legend>
                        <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtIdTestata" Style="display: none" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnSelCella" Style="display: none" runat="server" OnClick="btnSelCella_Click"></asp:Button>
                                    <!--*** 20141110 - passaggio di proprietà ***-->
                                    <Grd:RibesGridView ID="GrdDichiarazioni" runat="server" BorderStyle="None"
                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="700px"
                                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                        OnRowCommand="GrdRowCommand">
                                        <PagerSettings Position="Bottom"></PagerSettings>
                                        <PagerStyle CssClass="CartListFooter" />
                                        <RowStyle CssClass="CartListItem"></RowStyle>
                                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                        <Columns>
                                            <asp:TemplateField HeaderText="N. Dichiarazione">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Business.CoreUtility.FormattaGrdInt(DataBinder.Eval(Container, "DataItem.NumeroDichiarazione")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AnnoDichiarazione" SortExpression="AnnoDichiarazione" HeaderText="Anno">
                                                <HeaderStyle Width="60px" />
                                                <ItemStyle HorizontalAlign="Right" Width="60px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NumeroProtocollo" HeaderText="N. Protocollo">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Bonificato" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBonificato" runat="server" Text='<%# Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.Bonificato")) == true ? "Sì" : "No" %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Provenienza">
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label8" runat="server" Text='<%# GetProvenienza(DataBinder.Eval(Container, "DataItem.IDProvenienza")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contribuente">
                                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label9" runat="server" Text='<%# GetContribuente(DataBinder.Eval(Container, "DataItem.contitolare")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgSel" runat="server" Cssclass="BottoneGrd BottoneHandUpGrd" CommandName="RowSelect" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
                                                    <asp:HiddenField runat="server" ID="hfidtestata" Value='<%# Eval("id") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgDet" runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgPasProp" runat="server" Cssclass="BottoneGrd BottoneKeyGoGrd" CommandName="RowCopy" CommandArgument='<%# Eval("id") %>' alt="" OnClientClick="return confirm('Si vuole eseguire il passaggio di proprieta\' degli immobili aperti in dichiarazione?')"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </Grd:RibesGridView>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td valign="top" height="20"><strong class="Input_Label"></strong></td>
            </tr>
            <tr>
                <td>
                    <fieldset class="classeFiledSet">
                        <legend class="Legend">Immobili</legend>
                        <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                            <tr>
                                <td>
                                    <Grd:RibesGridView ID="GrdImmobili" runat="server" BorderStyle="None"
                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="150"
                                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                        OnRowCommand="GrdRowCommand" OnRowDataBound="GrdRowDataBound" OnPageIndexChanging="GrdPageIndexChanging">
                                        <PagerSettings Position="Bottom"></PagerSettings>
                                        <PagerStyle CssClass="CartListFooter" />
                                        <RowStyle CssClass="CartListItem"></RowStyle>
                                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Data Inizio">
                                                <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label6" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataInizio")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Fine">
                                                <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label7" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataFine")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Foglio" SortExpression="Foglio" HeaderText="Fg.">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Numero" SortExpression="Numero" HeaderText="Num.">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField SortExpression="Subalterno" HeaderText="Sub.">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Business.CoreUtility.FormattaGrdInt(DataBinder.Eval(Container, "DataItem.Subalterno")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CategoriaCatastale" HeaderText="Cat.">
                                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Classe" HeaderText="Cl.">
                                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Rendita">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label11" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Rendita")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valore">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ValoreImmobile")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="% Poss.">
                                                <HeaderStyle Width="60px" />
                                                <ItemStyle HorizontalAlign="center" Width="60px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# Business.CoreUtility.FormattaGrdInt(DataBinder.Eval(Container, "DataItem.PercPossesso")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Abi.Prin.">
                                                <HeaderStyle Width="60px" />
                                                <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaGrdAbiPrinc(DataBinder.Eval(Container, "DataItem.AbitazionePrincipaleAttuale")) %>' ID="Label10" NAME="Label10">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="RIDESE" HeaderText="Rid/Ese">
                                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Indirizzo">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Via") + " " + Business.CoreUtility.FormattaGrdInt(DataBinder.Eval(Container, "DataItem.NumeroCivico")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sel.">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSel" runat="server" AutoPostBack="true" Checked='<%# Business.CoreUtility.FormattaGrdCheck(DataBinder.Eval(Container, "DataItem.bSel")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PDF">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDOCFAPDF" runat="server" Cssclass="BottoneGrd BottonePDFGrd" Visible="True" CommandName="RowEdit" alt=""></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Plan.">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDOCFAPlan" runat="server" Cssclass="BottoneGrd BottonePlanimetriaGrd" Visible="True" CommandName="RowView" alt=""></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgDet" runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDIMMOBILE") %>' alt=""></asp:ImageButton>
                                                    <asp:HiddenField runat="server" ID="hfidoggetto" Value='<%# Eval("IDIMMOBILE") %>' />
                                                    <asp:HiddenField runat="server" ID="hfIdTestata" Value='<%# Eval("idtestata") %>' />
                                                    <asp:HiddenField runat="server" ID="hfDOCFAPDF" Value='<%# Eval("DOCFAPDF") %>' />
                                                    <asp:HiddenField runat="server" ID="hfDOCFAPlanimetria" Value='<%# Eval("DOCFAPlanimetria") %>' />
                                                    <asp:HiddenField runat="server" ID="hfIdDOCFA" Value='<%# Eval("IdDOCFA") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </Grd:RibesGridView>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
			<!-- Blocco Dati Catasto -->
			<tr>
				<td width="100%" colspan="7">
					<div id="DivDatiCatasto">
						<br />
					    <a title="Visualizza Dati Catasto" onclick="nascondi(this,'divCatasto','Dati Catasto')" href="#" class="lstTabRow" style="width:100%">Visualizza Dati Catasto</a>
					    <div id="divCatasto" runat="server" style="width:100%; display:none">
							<table width="100%">
								<tr>
									<td>
					                    <Grd:RibesGridView ID="GrdCatasto" runat="server" BorderStyle="None" 
                                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                            <PagerSettings Position="Bottom"></PagerSettings>
                                            <PagerStyle CssClass="CartListFooter" />
                                            <RowStyle CssClass="CartListItem"></RowStyle>
                                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                <Columns></Columns>
						                </Grd:RibesGridView>
									</td>
								</tr>
							</table>
						</div>
					</div>
				</td>
			</tr>
            <tr>
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
        <asp:Button ID="cmdNewImmobile" Style="display: none" runat="server" Text="" Enabled="true"></asp:Button>
        <asp:Button ID="CmdGIS" Style="display: none" runat="server"></asp:Button>
    </form>
</body>
</html>
