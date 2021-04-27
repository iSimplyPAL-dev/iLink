<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PopUpInsertVani.aspx.vb" Inherits="OPENgovTIA.PopUpInsertVani" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>PopUpInsertVani</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
        function CheckDatiVani() {
            document.getElementById('CmdSalvaVani').click()
        }

        function DeleteVani() {
            if (confirm('Si desidera eliminare il Vano?')) {
                document.getElementById('CmdDeleteVani').click()
            }
            return false;
        }
    </script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" runat="server" method="post">
        <table id="tblModificaIndirizzo" width="100%" cellspacing="0" cellpadding="0">
            <tr>
                <td class="SfondoGenerale" style="height: 43px">
                    <asp:Label ID="lblTitolo" runat="server" CssClass="ContentHead_Title"></asp:Label><br />
                    <label id="info" runat="server" class="NormalBold_title">&nbsp;&nbsp; - Dichiarazioni - Gestione Immobili - Gestione Dati Vani</label>
                </td>
                <td class="SfondoGenerale" style="height: 43px" align="right">
                    <input class="Bottone BottoneApri" id="Modifica" title="Modifica" onclick="getElementById('CmdModVani').click()" type="button" name="Modifica" />
                    <input class="Bottone Bottonecancella" id="Delete" title="Elimina" onclick="DeleteVani()" type="button" name="Delete" />
                    <input class="Bottone BottoneSalva" id="Salva" title="Salva" onclick="CheckDatiVani()" type="button" name="Salva" />
                    <input class="Bottone BottoneAnnulla" id="Esci" title="Esci" onclick="getElementById('CmdBack').click();" type="button" name="Esci" />&nbsp;
                </td>
            </tr>
            <!--blocco dati contribuente-->
            <%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
            <tr id="TRPlainAnag">
                <td colspan="2">
                    <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height: 200px; width: 100%" frameborder="0" marginheight="0"></iframe>
                    <asp:HiddenField ID="hdIdContribuente" runat="server" Value="-1" />
                </td>
            </tr>
            <tr id="TRSpecAnag">
                <td colspan="2">
                    <table id="TblDatiContribuente" cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="1">
                        <tr>
                            <td bordercolor="darkblue">
                                <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                                    <tr>
                                        <td class="Input_Label" colspan="4" height="20"><strong>DATI CONTRIBUENTE</strong></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="lblNominativo" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="lblCFPIVA" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente" colspan="2">
                                            <asp:Label ID="lblDatiNascita" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente" colspan="2">
                                            <asp:Label ID="lblResidenza" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!--blocco dati tessera-->
            <tr>
                <td colspan="2">
                    <div id="DivTessera" runat="server">
                        <table id="TblDatiTessera" cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="1">
                            <tr>
                                <td bordercolor="darkblue">
                                    <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td class="Input_Label" colspan="4" height="20"><strong>DATI TESSERA</strong></td>
                                        </tr>
                                        <tr>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblNTessera" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblCodInterno" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblCodUtente" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblDataRilascio" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="LblDataCessazione" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <!--Blocco Dati UI-->
            <tr>
                <td colspan="2">
                    <table id="Table3" cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="1">
                        <tr valign="top">
                            <td bordercolor="darkblue">
                                <table id="Table4" cellspacing="1" cellpadding="1" width="100%" border="0">
                                    <tr>
                                        <td class="Input_Label" colspan="5"><strong>DATI IMMOBILE</strong></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente" colspan="3">
                                            <asp:Label ID="LblUbicazione" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente" colspan="2">
                                            <asp:Label ID="LblRifCat" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="LblDataInizio" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="LblDataFine" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="LblMQCatasto" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="LblMQTassabili" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="DettagliContribuente" colspan="2">
                                            <asp:Label ID="LblCat" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="LblComponentiPF" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="LblComponentiPV" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        <td class="DettagliContribuente">
                                            <asp:Label ID="LblForzaCalcoloPV" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!-- Blocco dati Vani -->
            <tr>
                <td colspan="2">
                    <br />
                    <Grd:RibesGridView ID="GrdVani" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowCommand="GrdRowCommand" OnRowDataBound="GrdRowDataBound">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Cat.TARSU">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlCatTARSU" runat="server" CssClass="Input_Text" DataSource="<%# GetCatTARSU() %>" DataTextField="Descrizione" DataValueField="IdCategoria" Width="200px"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cat.">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlCatTARES" runat="server" CssClass="Input_Text" DataSource="<%# GetCatTARES() %>" DataTextField="Descrizione" DataValueField="IdCategoria" Width="200px"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="N.Comp.">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNC" runat="server" CssClass="Input_Text_Right" Text='<%# (DataBinder.Eval(Container, "DataItem.nNC")) %>' Width="50px" onblur="if (!isNumber(this.value, 0, 0)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Componenti!')}"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="N.Comp.PV">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNCPV" runat="server" CssClass="Input_Text_Right" Text='<%# (DataBinder.Eval(Container, "DataItem.nNCPV")) %>' Width="50px" onblur="if (!isNumber(this.value, 0, 0)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Componenti Variabile!')}"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Vano">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTipoVano" runat="server" CssClass="Input_Text" DataSource="<%# GetTipoVano() %>" DataTextField="Descrizione" DataValueField="IdTipoVano" Width="200px"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="N.Vani">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNVani" runat="server" CssClass="Input_Text_Right" Text='<%# (DataBinder.Eval(Container, "DataItem.nVani")) %>' Width="50px" onblur="if (!isNumber(this.value, 0, 0)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Numero Vani!')}"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mq">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMQ" runat="server" CssClass="Input_Text_Right" Text='<%# (DataBinder.Eval(Container, "DataItem.nMQ")) %>' Width="50px" onblur="if (!isNumber(this.value)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo Mq!')}"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vani Esenti">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkEsente" Checked='<%# (DataBinder.Eval(Container, "DataItem.bIsEsente")) %>' Width="50px"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Forza PV">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkForzaCalcoloPV" Checked='<%# (DataBinder.Eval(Container, "DataItem.bForzaCalcolaPV")) %>' Width="50px"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneNewInsertGrd" CommandName="RowNew" CommandArgument='<%# Eval("IdOggetto") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfIdOggetto" Value='<%# Eval("IdOggetto") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table id="TblDati" style="display: none">
                        <tr>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label1">Cat./Destinazione d'uso</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlCategorie1" CssClass="Input_Text" runat="server" Width="370px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label2">Tipo Vano</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlTipoVano1" CssClass="Input_Text" runat="server" Width="130px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label3">N.Vani</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNVani1" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right">1</asp:TextBox>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label4">MQ</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNMQ1" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right"></asp:TextBox>&nbsp;
                            </td>
                            <!--*** 20130325 - gestione mq tassabili per TARES ***-->
                            <td valign="bottom">
                                <asp:CheckBox ID="ChkEsente1" runat="server" CssClass="Input_Checkbox" Text="Esente"></asp:CheckBox>
                            </td>
                            <!--*** ***-->
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label5">Cat./Destinazione d'uso</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlCategorie2" CssClass="Input_Text" runat="server" Width="370px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label6">Tipo Vano</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlTipoVano2" CssClass="Input_Text" runat="server" Width="130px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label7">N.Vani</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNVani2" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right">1</asp:TextBox>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label8">MQ</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNMQ2" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right"></asp:TextBox>&nbsp;
                            </td>
                            <!--*** 20130325 - gestione mq tassabili per TARES ***-->
                            <td valign="bottom">
                                <asp:CheckBox ID="ChkEsente2" runat="server" CssClass="Input_Checkbox" Text="Esente"></asp:CheckBox>
                            </td>
                            <!--*** ***-->
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label9">Cat./Destinazione d'uso</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlCategorie3" CssClass="Input_Text" runat="server" Width="370px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label10">Tipo Vano</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlTipoVano3" CssClass="Input_Text" runat="server" Width="130px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label11">N.Vani</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNVani3" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right">1</asp:TextBox>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label12">MQ</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNMQ3" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right"></asp:TextBox>&nbsp;
                            </td>
                            <!--*** 20130325 - gestione mq tassabili per TARES ***-->
                            <td valign="bottom">
                                <asp:CheckBox ID="ChkEsente3" runat="server" CssClass="Input_Checkbox" Text="Esente"></asp:CheckBox>
                            </td>
                            <!--*** ***-->
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label13">Cat./Destinazione d'uso</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlCategorie4" CssClass="Input_Text" runat="server" Width="370px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label14">Tipo Vano</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlTipoVano4" CssClass="Input_Text" runat="server" Width="130px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label15">N.Vani</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNVani4" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right">1</asp:TextBox>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label16">MQ</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNMQ4" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right"></asp:TextBox>&nbsp;
                            </td>
                            <!--*** 20130325 - gestione mq tassabili per TARES ***-->
                            <td valign="bottom">
                                <asp:CheckBox ID="ChkEsente4" runat="server" CssClass="Input_Checkbox" Text="Esente"></asp:CheckBox>
                            </td>
                            <!--*** ***-->
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label17">Cat./Destinazione d'uso</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlCategorie5" CssClass="Input_Text" runat="server" Width="370px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label18">Tipo Vano</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlTipoVano5" CssClass="Input_Text" runat="server" Width="130px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label19">N.Vani</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNVani5" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right">1</asp:TextBox>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label20">MQ</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNMQ5" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right"></asp:TextBox>&nbsp;
                            </td>
                            <!--*** 20130325 - gestione mq tassabili per TARES ***-->
                            <td valign="bottom">
                                <asp:CheckBox ID="ChkEsente5" runat="server" CssClass="Input_Checkbox" Text="Esente"></asp:CheckBox>
                            </td>
                            <!--*** ***-->
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label21">Cat./Destinazione d'uso</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlCategorie6" CssClass="Input_Text" runat="server" Width="370px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label22">Tipo Vano</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlTipoVano6" CssClass="Input_Text" runat="server" Width="130px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label23">N.Vani</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNVani6" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right">1</asp:TextBox>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label24">MQ</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNMQ6" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right"></asp:TextBox>&nbsp;
                            </td>
                            <!--*** 20130325 - gestione mq tassabili per TARES ***-->
                            <td valign="bottom">
                                <asp:CheckBox ID="ChkEsente6" runat="server" CssClass="Input_Checkbox" Text="Esente"></asp:CheckBox>
                            </td>
                            <!--*** ***-->
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label25">Cat./Destinazione d'uso</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlCategorie7" CssClass="Input_Text" runat="server" Width="370px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label26">Tipo Vano</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlTipoVano7" CssClass="Input_Text" runat="server" Width="130px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label27">N.Vani</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNVani7" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right">1</asp:TextBox>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label28">MQ</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNMQ7" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right"></asp:TextBox>&nbsp;
                            </td>
                            <!--*** 20130325 - gestione mq tassabili per TARES ***-->
                            <td valign="bottom">
                                <asp:CheckBox ID="ChkEsente7" runat="server" CssClass="Input_Checkbox" Text="Esente"></asp:CheckBox>
                            </td>
                            <!--*** ***-->
                        </tr>
                        <tr>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label29">Cat./Destinazione d'uso</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlCategorie8" CssClass="Input_Text" runat="server" Width="370px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label30">Tipo Vano</asp:Label>
                                <br />
                                <asp:DropDownList ID="DdlTipoVano8" CssClass="Input_Text" runat="server" Width="130px"></asp:DropDownList>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label31">N.Vani</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNVani8" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right">1</asp:TextBox>&nbsp;
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label32">MQ</asp:Label>
                                <br />
                                <asp:TextBox ID="TxtNMQ8" CssClass="Input_Text" runat="server" Width="70px" Style="text-align: right"></asp:TextBox>&nbsp;
                            </td>
                            <!--*** 20130325 - gestione mq tassabili per TARES ***-->
                            <td valign="bottom">
                                <asp:CheckBox ID="ChkEsente8" runat="server" CssClass="Input_Checkbox" Text="Esente"></asp:CheckBox>
                            </td>
                            <!--*** ***-->
                        </tr>
                    </table>
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
        <asp:TextBox ID="TxtIdVano1" runat="server" Style="display: none">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdVano2" runat="server" Style="display: none">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdVano3" runat="server" Style="display: none">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdVano4" runat="server" Style="display: none">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdVano5" runat="server" Style="display: none">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdVano6" runat="server" Style="display: none">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdVano7" runat="server" Style="display: none">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdVano8" runat="server" Style="display: none">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdDettaglioTestata" runat="server" Style="display: none">-1</asp:TextBox>
        <asp:Button ID="CmdDeleteVani" runat="server" Style="display: none"></asp:Button>
        <asp:Button ID="CmdModVani" runat="server" Style="display: none"></asp:Button>
        <asp:Button ID="CmdSalvaVani" runat="server" Style="display: none"></asp:Button>
        <asp:Button ID="CmdBack" runat="server" Style="display: none"></asp:Button>
    </form>
</body>
</html>
