<%@ Control Language="c#" AutoEventWireup="True" Codebehind="WucDatiContribuente.ascx.cs" Inherits="OPENgovTOCO.Wuc.WucDatiContribuente" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table id="TblDatiContribuente" border="1" cellSpacing="0" cellPadding="0" width="100%"
	bgColor="white">
	<tr>
		<td borderColor="darkblue">
			<table border="0" cellSpacing="1" cellPadding="1" width="100%">
				<tr>
					<td class="Input_Label" height="20" colSpan="4"><strong>DATI CONTRIBUENTE</strong></td>
				</tr>
				<tr>
					<td class="DettagliContribuente" width="160">Cognome/Rag. Sociale</td>
					<td class="DettagliContribuente" width="25%"><asp:label id="lblCognome" runat="server" CssClass="DettagliContribuente"></asp:label></td>
					<td class="DettagliContribuente" width="110">Nome</td>
					<td class="DettagliContribuente"><asp:label id="lblNome" runat="server" CssClass="DettagliContribuente"></asp:label></td>
				</tr>
				<tr>
					<td class="DettagliContribuente">Data di Nascita</td>
					<td class="DettagliContribuente"><asp:label id="lblDataNascita" runat="server" CssClass="DettagliContribuente"></asp:label></td>
					<td></td>
					<td></td>
				</tr>
				<tr>
					<td class="DettagliContribuente">RESIDENTE IN</td>
					<td class="DettagliContribuente"><asp:label id="lblIndirizzo" runat="server" CssClass="DettagliContribuente"></asp:label></td>
					<td class="DettagliContribuente">Comune (Prov.)</td>
					<td class="DettagliContribuente"><asp:label id="lblComune" runat="server" CssClass="DettagliContribuente"></asp:label></td>
				</tr>
			</table>
		</td>
	</tr>
</table>
