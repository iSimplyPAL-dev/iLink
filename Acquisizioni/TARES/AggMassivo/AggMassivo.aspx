<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgov.Master" AutoEventWireup="true" CodeBehind="AggMassivo.aspx.cs" Inherits="OPENgov.Acquisizioni.TARES.AggMassivo.AggMassivo" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Row" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHeader" runat="server">
    <div id="divCmdSearch">
        <asp:Button ID="cmdPrint" runat="server" Cssclass="BottoneExcel Bottone" ToolTip="Stampa" CausesValidation="False" onclick="CmdPrintClick"/>
        <asp:Button ID="cmdSave" runat="server" Cssclass="BottoneSalva Bottone" ToolTip="Salva" CausesValidation="False" onclick="CmdSaveClick"/>
        <asp:Button ID="cmdSearch" runat="server" Cssclass="BottoneRicerca Bottone" ToolTip="Cerca..." CausesValidation="True" onclick="CmdSearchClick" />
    </div>
    <div id="divSimula">
        <asp:Button ID="cmdPrintSimula" runat="server" Cssclass="BottoneExcel Bottone" ToolTip="Stampa" CausesValidation="False" onclick="CmdPrintClick"/>
        <asp:Button ID="cmdAggSimula" runat="server" Cssclass="BottoneSalva Bottone" ToolTip="Aggiorna" CausesValidation="False" onclick="CmdSaveClick"/>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
    <div id="SearchParam" style="margin: 0 auto;">
	    <a title="Visualizza Parametri Ricerca per Aggiornamento Massivo" onclick="nascondi(this,'ParamSearch','Parametri Ricerca per Aggiornamento Massivo')" href="#" class="Legend" id="aVisParamSearch">Visualizza Parametri Ricerca per Aggiornamento Massivo</a>
	    <br/>
        <div id="ParamSearch">
            <fieldset class="FiledSetRicerca">
                <legend class="Legend">Parametri di Ricerca</legend>
                <table>
				    <tr>
					    <td>
						    <asp:Label id="Label7" CssClass="Input_Label" Runat="server">Via</asp:Label>&nbsp;
						    <div class="tooltip">
                                <asp:imagebutton id="LnkOpenStradario" runat="server" alt="" CausesValidation="False" imagealign="Bottom" ImageUrl="../../images/Bottoni/Listasel.png"></asp:imagebutton>&nbsp;
                                <span class="tooltiptext">Ubicazione Immobile da Stradario</span>
                            </div>
						    <div class="tooltip">
                                <asp:imagebutton id="LnkPulisciStrada" runat="server" alt="" CausesValidation="False" imagealign="Bottom" ImageUrl="../../images/Bottoni/cancel.png" style="width:10px;height:10px"></asp:imagebutton>
                                <span class="tooltiptext">Pulisci i campi della Via</span>
                            </div><br />
						    <asp:textbox id="TxtVia" runat="server" CssClass="Input_Text" Width="400px" ReadOnly="True"></asp:textbox>
                            <asp:textbox id="TxtCodVia" style="DISPLAY: none" CssClass="Input_Text" Runat="server"></asp:textbox>
							<asp:textbox id="TxtViaRibaltata" style="DISPLAY: none" CssClass="Input_Text" Runat="server"></asp:textbox>
                        </td>
					    <td>
						    <asp:Label id="Label8" CssClass="Input_Label" Runat="server">Civico</asp:Label><br />
						    <asp:textbox id="TxtCivico" runat="server" CssClass="Input_Text" Width="50px"></asp:textbox>
                        </td>
					    <td>
						    <asp:Label id="Label10" runat="server" CssClass="Input_Label">Foglio</asp:Label><br />
						    <asp:TextBox id="TxtFoglio" runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox>
                        </td>
					    <td>
						    <asp:Label id="Label9" runat="server" CssClass="Input_Label">Numero</asp:Label><br />
						    <asp:TextBox id="TxtNumero" runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox>
                        </td>
					    <td>
						    <asp:Label id="Label12" runat="server" CssClass="Input_Label">Subalterno</asp:Label><br />
						    <asp:TextBox id="TxtSubalterno" runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox>
                        </td>
					    <td>
						    <asp:Label id="Label2" runat="server" CssClass="Input_Label">Cat.Catastale</asp:Label><br />
                            <Row:RibesDropDownList ID="rddlCatCatastale" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>
                        </td>
                        <td style="width:auto">
                            <asp:Label runat="server" CssClass="Input_Label">Periodo (dal/al)</asp:Label><br />
                            <asp:TextBox ID="txtDal" runat="server" CssClass="Input_Text_Right" Width="80px" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>&nbsp;
                            <asp:TextBox ID="txtAl" runat="server" CssClass="Input_Text_Right" Width="80px" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
					    <td valign="top">
						    <asp:Label runat="server" CssClass="Input_Label">Categoria</asp:Label><br />
                            <Row:RibesDropDownList ID="rddlCat" runat="server" DefaultFirstValue="" DefaultFirstText="..." Width="400px" CssClass="Input_Text"></Row:RibesDropDownList>
                        </td>
					    <td valign="top" colspan="2">
						    <asp:Label runat="server" CssClass="Input_Label">N.Componenti</asp:Label><br />
                            <asp:TextBox ID="txtNC" runat="server" CssClass="Input_Text_Numbers" Width="50px"></asp:TextBox>
                        </td>
					    <td valign="top" colspan="2">
							<asp:CheckBox ID="chkPF" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Parte Fissa" /><br />
							<asp:CheckBox ID="chkPV" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Parte Variabile" />
                        </td>
                    </tr>
                    <tr>
					    <td>
						    <asp:Label id="Label3" runat="server" CssClass="Input_Label">Riduzione</asp:Label><br />
                            <Row:RibesDropDownList ID="rddlRid" runat="server" DefaultFirstValue="" DefaultFirstText="..." Width="400px" CssClass="Input_Text"></Row:RibesDropDownList><br />
						    <asp:Label id="Label4" runat="server" CssClass="Input_Label">Detassazione</asp:Label><br />
                            <Row:RibesDropDownList ID="rddlDet" runat="server" DefaultFirstValue="" DefaultFirstText="..." Width="400px" CssClass="Input_Text"></Row:RibesDropDownList>
                        </td>
					    <td valign="top" colspan="2">
						    <asp:Label runat="server" CssClass="Input_Label">Stato Occupazione</asp:Label><br />
                            <Row:RibesDropDownList ID="rddlStatoOccupazione" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>
                        </td>
					    <td valign="top" colspan="2">
                            <asp:RadioButton ID="optRes" runat="server" CssClass="Input_Label" GroupName="rbTypeRes" Text="Residente (banca dati demografico)" Width="250px" /><br />
                            <asp:RadioButton ID="optNoRes" runat="server" CssClass="Input_Label" GroupName="rbTypeRes" Text="Non Residente"/><br />
                            <asp:RadioButton ID="optResNoRes" runat="server" CssClass="Input_Label" GroupName="rbTypeRes" Text="Tutti" Checked="true" />
                        </td>
					    <td valign="top" colspan="2">
						    <asp:CheckBox ID="chkEsente" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Vano Esente" ToolTip="Vano Esente" /><br />
                            <asp:CheckBox ID="chkMoreUI" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Più di una Unità" ToolTip="Più di una Unità per Categoria Catastale" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
    <div id="AggParam" style="margin:0 auto;">
	    <a title="Visualizza Parametri per Aggiornamento Massivo" onclick="nascondi(this,'ParamAgg','Parametri per Aggiornamento Massivo')" href="#" class="Legend">Visualizza Parametri per Aggiornamento Massivo</a>
	    <br/>
        <div id="ParamAgg">
            <fieldset class="FiledSetRicerca">
                <legend class="Legend">Dati per Aggiornamento</legend>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label11" runat="server" CssClass="Input_Label">Dal</asp:Label><br />
                            <asp:TextBox ID="txtAggDal" runat="server" CssClass="Input_Text_Right" Width="80px" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>&nbsp;
                            <br />
                            <asp:RequiredFieldValidator ID="rfvAggDal" runat="server" ErrorMessage="Inserisci la data" ControlToValidate="txtAggDal" ValidationGroup="SaveAggMassivo"></asp:RequiredFieldValidator>
                        </td>
					    <td>
						    <asp:Label ID="Label1" runat="server" CssClass="Input_Label">Categoria</asp:Label><br />
                            <Row:RibesDropDownList ID="rddlAggCat" runat="server" DefaultFirstValue="" DefaultFirstText="..." Width="400px" CssClass="Input_Text"></Row:RibesDropDownList>
                        </td>
					    <td>
						    <asp:Label ID="Label5" runat="server" CssClass="Input_Label">N.Componenti Fissa</asp:Label><br />
                            <asp:TextBox ID="txtAggNCFissa" runat="server" CssClass="Input_Text_Numbers" Width="50px"></asp:TextBox>
                        </td>
					    <td>
						    <asp:Label ID="Label6" runat="server" CssClass="Input_Label">N.Componenti Variabile</asp:Label><br />
                            <asp:TextBox ID="txtAggNCVariabile" runat="server" CssClass="Input_Text_Numbers" Width="50px"></asp:TextBox>
                        </td>
					    <td>
						    <asp:Label ID="Label13" runat="server" CssClass="Input_Label">Stato Occupazione</asp:Label><br />
                            <Row:RibesDropDownList ID="rddlAggStatoOccupazione" runat="server" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text"></Row:RibesDropDownList>
                        </td>
                    </tr>
                    <tr>
					    <td>
						    <asp:Label id="Label15" runat="server" CssClass="Input_Label">Riduzione</asp:Label><br />
                            <Row:RibesDropDownList ID="rddlAggRid" runat="server" DefaultFirstValue="" DefaultFirstText="..." Width="400px" CssClass="Input_Text"></Row:RibesDropDownList>
                        </td>
					    <td colspan="2">
						    <asp:Label id="Label16" runat="server" CssClass="Input_Label">Detassazione</asp:Label><br />
                            <Row:RibesDropDownList ID="rddlAggDet" runat="server" DefaultFirstValue="" DefaultFirstText="..." Width="400px" CssClass="Input_Text"></Row:RibesDropDownList>
                        </td>
					    <td>
						    <asp:CheckBox ID="chkAggEsente" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Vano Esente" ToolTip="Vano Esente" /><br />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
    <div id="SearchResult" style="margin: 0 auto;">
        <fieldset class="classeFiledSetNoBorder">
            <legend class="Legend">Risultati della ricerca</legend>
            <Row:RibesGridView ID="rgvUI" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="99%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" HoverRowCssClass="riga_tabella_mouse_over"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                onpageindexchanging="rgvUIPageIndexChanging" onrowcommand="rgvUIRowCommand">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                <Columns>
                    <asp:TemplateField HeaderText="Nominativo">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <ItemTemplate>
                            <asp:Label ID="lblNominativo" runat="server" Text='<%# FncGrd.FormattaNominativo(Eval("Anagrafe.Cognome"),Eval("Anagrafe.Nome")) %>'></asp:Label>
                            <asp:HiddenField ID="hfIdArticolo" runat="server" Value='<%# Eval("Id") %>' />
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cod.Fiscale/P.IVA">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <ItemTemplate>
                            <asp:Label ID="lblCFPIVA" runat="server" Width="100px" Text='<%# FncGrd.FormattaCFPIVA(Eval("Anagrafe.CodiceFiscale"),Eval("Anagrafe.PartitaIva")) %>'></asp:Label>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ubicazione (rif. cat.)">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <ItemTemplate>
                            <asp:Label ID="lblUbicaz" runat="server" Text='<%# FncGrd.FormattaVia(Eval("Partita.sVia"),Eval("Partita.sCivico"),Eval("Partita.sInterno"),Eval("Partita.sEsponente"),Eval("Partita.sScala"),Eval("Partita.sFoglio"),Eval("Partita.sNumero"),Eval("Partita.sSubalterno")) %>'></asp:Label>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dal">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtInizio" runat="server" CssClass="Input_Text_Right" Width="75px" Text='<%# FncGrd.FormattaDataGrd(Eval("Partita.tDataInizio")) %>'></asp:TextBox>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Al">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtFine" runat="server" CssClass="Input_Text_Right" Width="75px" Text='<%# FncGrd.FormattaDataGrd(Eval("Partita.tDataFine")) %>'></asp:TextBox>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cat. Catastale">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <ItemTemplate>
                            <asp:Label ID="lblCatCat" runat="server" Text='<%# FncGrd.FormattaItemGrd(Eval("Partita.sCatCatastale")) %>'></asp:Label>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stato Occupazione">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <ItemTemplate>
                            <asp:Label ID="lblStatoOccup" runat="server" Text='<%# FncGrd.FormattaItemGrd(Eval("Partita.sDescrOccupazione")) %>'></asp:Label>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Categoria">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <ItemTemplate>
                            <Row:RibesDropDownList ID="ddlCat" runat="server" Width="150px" DataSource="<%# LoadCat()%>" DataTextField="Descrizione" DataValueField="IdCategoriaAteco" DefaultFirstValue="" DefaultFirstText="..." CssClass="Input_Text" SelectedValue='<%# Eval("Partita.IdCatAteco") %>'></Row:RibesDropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NC Fissa">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtNCFissa" runat="server" Width="40px" CssClass="Input_Text_Numbers" Text='<%# Eval("Partita.nNComponenti") %>'></asp:TextBox>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NC Variabile">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtNCVariabile" runat="server" Width="40px" CssClass="Input_Text_Numbers" Text='<%# Eval("Partita.nComponentiPV") %>'></asp:TextBox>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MQ Tassabili">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <ItemTemplate>
                            <asp:TextBox ID="txtMQ" runat="server" Width="60px" CssClass="Input_Text_Numbers" Text='<%# Eval("Partita.nMQTassabili") %>'></asp:TextBox>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rid/Det">
                        <headerstyle horizontalalign="Center" Width="60px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkRidDet" runat="server" CssClass="Input_CheckBox_NoBorder" Checked='<%# FncGrd.FormattaHasRidDet(Eval("Partita")) %>' Enabled="false" />
                            <div class="tooltip">
                                <asp:ImageButton id="imgEdit" runat="server" CssClass="BottoneGrd BottoneApriGrd" CausesValidation="False" CommandName="EditRidEse" CommandArgument='<%# Eval("Id") %>' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Modifica</span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sel">
                        <headerstyle horizontalalign="Center" Width="40px"></headerstyle>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkAll" runat="server" Text="Tutte" ToolTip="Tutte" TextAlign="Left" Checked="false" AutoPostBack="true" OnCheckedChanged="SelAllRow" />
                        </HeaderTemplate>
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSel" runat="server" CssClass="Input_CheckBox_NoBorder" Checked='<%# Eval("IsSel") %>' AutoPostBack="true" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <headerstyle horizontalalign="Center" Width="40px"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                        <ItemTemplate>
                            <div class="tooltip">
                                <asp:ImageButton id="imgSave" runat="server" CssClass="BottoneGrd BottoneSalvaGrd" CausesValidation="False" CommandName="SaveUI" CommandArgument='<%# Eval("Id") %>' alt=""></asp:ImageButton> 
                                <span class="tooltiptext">Salva</span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                </Columns>
            </Row:RibesGridView>
        </fieldset>
    </div>
    <div id="divRidDet" style="margin: 0 auto;z-index:100;top:50px;left:10px;position:absolute;width:600px" class="SfondoGenerale">
        <table>
            <tr>
                <td class="SfondoGenerale">
                    <asp:Label runat="server" CssClass="NormalBold_title">Riduzioni/Detassazioni</asp:Label>
                </td>
                <td class="SfondoGenerale">
                    <asp:Button ID="CmdExitRidEse" runat="server" Cssclass="Bottone BottoneAnnulla Bottone" ToolTip="Indietro" CausesValidation="True" onclick="CmdExitRidEseClick" style="float:right;"/>
                    <asp:Button ID="CmdSaveRidDet" runat="server" Cssclass="Bottone BottoneSalva Bottone" ToolTip="Salva" CausesValidation="False" onclick="CmdSaveRidDetClick" style="float:right;"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label CssClass="Legend" Runat="server" ID="LblResultRid">Non sono presenti Riduzioni</asp:Label>
                    <Row:RibesGridView ID="rgvRid" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="10" HoverRowCssClass="riga_tabella_mouse_over"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        onpageindexchanging="rgvRidDetPageIndexChanging" onrowcommand="rgvRidDetRowCommand">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="Codice" HeaderText="Codice">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Definizione" HeaderText="Descrizione">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Sel">
                                <headerstyle horizontalalign="Center" Width="60px"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSel" runat="server" CssClass="Input_CheckBox_NoBorder" Checked="false" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Row:RibesGridView>
                </td>
                <td>
			        <asp:Label CssClass="Legend" Runat="server" ID="LblResultDet">Non sono presenti Detassazioni</asp:Label>
                    <Row:RibesGridView ID="rgvDet" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="10" HoverRowCssClass="riga_tabella_mouse_over"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        onpageindexchanging="rgvRidDetPageIndexChanging" onrowcommand="rgvRidDetRowCommand">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="Codice" HeaderText="Codice">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Definizione" HeaderText="Descrizione">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Sel">
                                <headerstyle horizontalalign="Center" Width="60px"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSel" runat="server" CssClass="Input_CheckBox_NoBorder" Checked="false" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Row:RibesGridView>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hfIdUI" runat="server"/>
    </div>
</asp:Content>
