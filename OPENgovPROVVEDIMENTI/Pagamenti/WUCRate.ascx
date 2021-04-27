<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="WUCRate.ascx.vb" Inherits="Provvedimenti.Provvedimenti.usercontrol.WUCRate" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<fieldset>
    <legend id="lbbLegend" class="Legend" runat="server">Elenco Rate</legend>
    <asp:Label ID="lblInfoRate" CssClass="Input_Label NormalRed" Visible="False" runat="server"></asp:Label>
    <Grd:RibesGridView ID="GrdRate" runat="server" BorderStyle="None"
        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="99%"
        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
        OnRowCommand="GrdRowCommand" OnRowDataBound="GrdRowDataBound">
        <PagerSettings Position="Bottom"></PagerSettings>
        <PagerStyle CssClass="CartListFooter" />
        <RowStyle CssClass="CartListItem"></RowStyle>
        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
        <Columns>
            <asp:TemplateField HeaderText="N Rata">
                <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="lblRata" runat="server" Text='<%# nRata(DataBinder.Eval(Container, "DataItem.n_rata")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Data Scadenza">
                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="Label7" runat="server" Text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_scadenza")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Importo Rata">
                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="Label6" runat="server" Text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.valore_rata"),2) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Importo Interesse">
                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="Label4" runat="server" Text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.valore_interesse"),2) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Importo Totale">
                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="lblImportoTotale" runat="server" Text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.importo_totale_rata"),2) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Importo Pagato">
                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="lblImportoPagato" CssClass="bold" runat="server" Text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.importo_pagato"),2) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Data Accredito">
                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="lblDataAcc" CssClass="bold" runat="server" Text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_accredito")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Data Pagamento">
                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:Label ID="lblDataPag" CssClass="bold" runat="server" Text='<%# annoBarra(DataBinder.Eval(Container, "DataItem.data_pagamento")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="DescProvenienza" ReadOnly="True" HeaderText="Provenienza">
                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="Rata">
                <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:ImageButton ID="imgDelete" runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" CommandArgument='<%# Eval("id_rata_acc") %>' alt="" OnClientClick="return confirm('Attenzione!\nStai per eliminare una rata.\nVuoi procedere?')"></asp:ImageButton>
                    <asp:HiddenField runat="server" ID="hfid_accorpamento" Value='<%# Eval("id_accorpamento") %>' />
                    <asp:HiddenField runat="server" ID="hfid_provvedimento" Value='<%# Eval("id_provvedimento") %>' />
                    <asp:HiddenField runat="server" ID="hfid_rata_acc" Value='<%# Eval("id_rata_acc") %>' />
                    <asp:HiddenField runat="server" ID="hfid_rata_provv" Value='<%# Eval("id_rata_provv") %>' />
                    <asp:HiddenField runat="server" ID="hfPROVENIENZA" Value='<%# Eval("PROVENIENZA") %>' />
                    <asp:HiddenField runat="server" ID="hfid_pagato" Value='<%# Eval("id_pagato") %>' />
                    <asp:HiddenField runat="server" ID="hftipo" Value='<%# Eval("tipo") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Pag.">
                <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:ImageButton ID="imgOpenRata" runat="server" Cssclass="BottoneGrd BottoneNewInsertGrd" CommandName="RowAdd" CommandArgument='<%# Eval("n_rata") %>' alt=""></asp:ImageButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </Grd:RibesGridView>
    <asp:TextBox ID="txtRatePagate" runat="server" Visible="false"></asp:TextBox>
    <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
</fieldset>
