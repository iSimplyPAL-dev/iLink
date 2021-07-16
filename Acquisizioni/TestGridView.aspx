<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgov.Master" AutoEventWireup="true" CodeBehind="TestGridView.aspx.cs" Inherits="OPENgov.Acquisizioni.TestGridView" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Row" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="ContentHeader" runat="server">
    
</asp:Content>
<asp:Content ID="body" ContentPlaceHolderID="ContentBody" runat="server">
    <fieldset class="classeFiledSetIframe">
        <legend class="Legend">Modello nuova griglia...</legend>
        <Row:RibesGridView ID="RibesGridView1" runat="server" PageSize="10" Width="100%" AllowSorting="false"
            AllowPaging="True" onpageindexchanging="RibesGridView1PageIndexChanging" 
            EnableRowClick="true" onrowclicked="RibesGridView1RowClicked" HoverRowCssClass="riga_tabella_mouse_over" AutoGenerateColumns="False" >
            <RowStyle CssClass="CartListItem"></RowStyle>
            <AlternatingRowStyle CssClass="CartListItemAlt"></AlternatingRowStyle>
            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
            <PagerStyle CssClass="CartListFooter"></PagerStyle>
            <Columns>
                <asp:BoundField DataField="IdUIMovimenti" HeaderText="Id" Visible="False"/>
                <asp:BoundField DataField="CodAmministrativo" HeaderText="Codice"/>
                <asp:BoundField DataField="IdentificativoImmobile" HeaderText="Identificativo Immobile"/>
                <asp:BoundField DataField="Categoria" HeaderText="Categoria"/>
                <asp:BoundField DataField="Classe" HeaderText="Classe"/>
                <asp:BoundField DataField="Consistenza" HeaderText="Consistenza"/>
                <asp:BoundField DataField="Superficie" HeaderText="Superficie"/>
                <asp:BoundField DataField="RenditaEuro" HeaderText="Rendita"/>
            </Columns>
        </Row:RibesGridView>
        <Row:RibesDateBox ID="RibesDateBox1" runat="server"></Row:RibesDateBox>
    </fieldset>
</asp:Content>
