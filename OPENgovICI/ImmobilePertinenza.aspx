<%@ Page language="c#" Codebehind="ImmobilePertinenza.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.ImmobilePertinenza" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>ImmobilePertinenza</title>
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
		<script type="text/javascript" type="text/javascript">
			function SelectDeselect(objCheckBox, check, aspCheckBoxID){
			//function CheckAllDataGridCheckBoxes(aspCheckBoxID, checkVal) {

				var newCheck = false;
				
				//alert(objCheckBox.checked);
				
				if (objCheckBox.checked == true){
					newCheck = true;
				}else{
					newCheck = false;
				}
				

				re = new RegExp(':' + aspCheckBoxID + '$')  //generated control name starts with a colon

				for(i = 0; i < document.forms[0].elements.length; i++) {
					elm = document.forms[0].elements[i]
					if (elm.type == 'checkbox') {
						if (re.test(elm.name)) {
							elm.checked = check;
						}
					}
				}
				
				objCheckBox.checked = newCheck;
				
				return false
			}
			
			function PopolaPertinenza(idPertinenza){
				if (idPertinenza == -1){
					idPertinenza = '-1';
					chkOk = false;
				}else{
					chkOk = true;
				}
				//alert(idPertinenza);
				parent.opener.document.getElementById('txtCodPertinenza').value = idPertinenza;
				parent.opener.document.getElementById('chkPertinenzaDummy').checked = chkOk;
				parent.opener.document.getElementById('chkPertinenzaNoDummy').checked = chkOk;
				window.close();
			}
		</script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout" leftmargin="0" bottommargin="0" rightmargin="0"
		topmargin="0">
		<form id="Form1" runat="server" method="post">
			<table width="100%" border="0" cellpadding="3" cellspacing="0">
				<!-- Comandiera -->
				<tr class="SfondoGenerale">
					<td><asp:label id="lblTitolo" runat="server"></asp:label><br />
						<span id="info">ICI/IMU - Dichiarazioni - Dettaglio immobile - Pertinenza</span></td>
					<td align="right">
						<asp:button Cssclass="Bottone BottoneSalva" id="btnAssocia" runat="server" title="Associa l'immobile alla pertinenza" onclick="btnAssocia_Click"></asp:button>
						<input type="button" class="Bottone Bottoneannulla" title="Chiudi la finestra" onclick="window.close();">
					</td>
				</tr>
				<tr>
					<td colspan="2">&nbsp;</td>
				</tr>
				<tr>
					<td colSpan="2" align="center">
                         <Grd:RibesGridView ID="GrdImmobili" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnRowDataBound="GrdRowDataBound">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<columns>
								<asp:TemplateField HeaderText="Data Inizio">
									<itemstyle horizontalalign="Center"></itemstyle>
									<itemtemplate>
										<asp:label id="Label1" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataInizio")) %>'>
										</asp:label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Data Fine">
									<itemstyle horizontalalign="Center"></itemstyle>
									<itemtemplate>
										<asp:label id="Label3" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataFine")) %>'>
										</asp:label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="Foglio" SortExpression="Foglio" HeaderText="Foglio">
									<itemstyle horizontalalign="Right"></itemstyle>
								</asp:BoundField>
								<asp:BoundField DataField="Numero" SortExpression="Numero" HeaderText="Num.">
									<itemstyle horizontalalign="Right"></itemstyle>
								</asp:BoundField>
								<asp:TemplateField SortExpression="Subalterno" HeaderText="Sub.">
									<itemstyle horizontalalign="Right"></itemstyle>
									<itemtemplate>
										<asp:label id="Label2" runat="server" Text='<%# Business.CoreUtility.FormattaGrdInt(DataBinder.Eval(Container, "DataItem.Subalterno")) %>'>
										</asp:label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="CodCategoriaCatastale" HeaderText="Cat.">
									<itemstyle horizontalalign="Left"></itemstyle>
								</asp:BoundField>
								<asp:BoundField DataField="CodClasse" HeaderText="Cl.">
									<itemstyle horizontalalign="Left"></itemstyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Valore Immobile">
									<itemstyle horizontalalign="Right"></itemstyle>
									<itemtemplate>
										<asp:label id=Label4 runat="server" Text='<%# Business.CoreUtility.FormattaGrdInt(DataBinder.Eval(Container, "DataItem.ValoreImmobile")) %>'>
										</asp:label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="% Poss.">
									<itemstyle horizontalalign="Right"></itemstyle>
									<itemtemplate>
										<asp:label id="Label5" runat="server" Text='<%# Business.CoreUtility.FormattaGrdInt(DataBinder.Eval(Container, "DataItem.PercPossesso")) %>'>
										</asp:label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Indirizzo">
									<itemstyle horizontalalign="Left"></itemstyle>
									<itemtemplate>
										<asp:Label id=lblIndirizzo runat="server" Text='<%# GetIndirizzo(Convert.ToInt32(DataBinder.Eval(Container, "DataItem.IDOggetto"))) %>'>
										</asp:label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Pertinenza">
									<itemstyle horizontalalign="Center"></itemstyle>
									<itemtemplate>
										<asp:checkbox id="chkPertinenza" onclick="SelectDeselect(this, this.check, 'chkPertinenza')" runat="server"></asp:checkbox>
                                        <asp:HiddenField runat="server" ID="hfIDOggetto" Value='<%# Eval("IDOggetto") %>' />
									</itemtemplate>
								</asp:TemplateField>
							</columns>
						</Grd:RibesGridView>
                    </td>
				</tr>
			</table>
		</form>
	</body>
</html>
