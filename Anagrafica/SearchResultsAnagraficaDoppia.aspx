<%@ Page Language="vb" CodeBehind="SearchResultsAnagraficaDoppia.aspx.vb" AutoEventWireup="false" Inherits="SearchResultsAnagraficaDoppia" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<HTML>
	<HEAD>
		<title></title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%End If%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		function GoToDettaglio(intCod_Contribuente,intIDDataAnagrafica)
		{
			//alert(intCod_Contribuente + " _ " + intIDDataAnagrafica)
		
			Parametri="?popup=0&COD_CONTRIBUENTE="+intCod_Contribuente+"&ID_DATA_ANAGRAFICA="+intIDDataAnagrafica+"&PAGEFROM=DOPP";
			parent.location.href="FormAnagrafica.aspx"+Parametri
        }
		function UpdateContrib()
		{
			//alert("pippo")
			var iCount;
			var Elemento;
			aspCheckBoxID='ChkPrinc'
			iCount=0;
			re = new RegExp(':' + aspCheckBoxID + '$')  //generated control name starts with a colon			
	
			for(i = 0; i < document.forms[0].elements.length; i++) 
			{
				elm = document.forms[0].elements[i]					
				if (elm.type == 'checkbox') 
				{			
					
					a = elm.id.indexOf(aspCheckBoxID);
										
					if (a!=-1)
					{								
						if (aspCheckBoxID==elm.id) 
						{								
							Elemento=elm
						}
						if(elm.checked ==true)
						{									
							iCount=iCount+1;
						}
					}
				}
			}
						
			if (iCount==0)
			{					
				GestAlert('a', 'warning', '', '',"Selezionare una Anagrafica Principale!")
				return false;					
			}
			if (iCount>1)
			{					
				GestAlert('a', 'warning', '', '',"Selezionare solo una Anagrafica Principale!")
				return false;					
			}			
			
			var iCountzz;
			var Elementozz;
			aspCheckBoxIDzz='ChkSelect'
			iCountzz=0;		
			rezz = new RegExp(':' + aspCheckBoxIDzz + '$')  //generated control name starts with a colon			
	
			for(i = 0; i < document.forms[0].elements.length; i++) 
			{
				elmzz = document.forms[0].elements[i]					
				if (elmzz.type == 'checkbox') 
				{							
					
					a = elmzz.id.indexOf(aspCheckBoxIDzz);
										
					if (a!=-1)
					{
						if (aspCheckBoxIDzz==elmzz.id) 
						{								
							Elementozz=elmzz
						}
						if(elmzz.checked ==true)
						{									
							iCountzz=iCountzz+1;
						}
					}
				}
			}				
			
			if (iCountzz==0)
			{					
				GestAlert('a', 'warning', '', '',"Selezionare almeno una Anagrafica Secondaria!")
				return false;					
			}
			else 
			{		
				bRet=confirm('Sei sicuro effettuare l\'aggiornamento del soggetto anagrafico nell\'intero sistema?')
				if (bRet)
				{
                    parent.parent.Visualizza.DivAttesa.style.display='';
                    document.getElementById('btnSalva').click();
					return false;		
				}				
				else				
				{
					return false;	
				}												
			}					
		}
		function estraiExcel()
		{
		    document.getElementById('btnStampaExcel').click();
		}
		</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td><asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label></td>
				</tr>
				<tr>
				    <td>
                        <Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				            <Columns>
					            <asp:TemplateField HeaderText="Princ">
						            <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
						            <ItemStyle HorizontalAlign="Center"></ItemStyle>
						            <ItemTemplate>
							            <asp:CheckBox id="ChkPrinc" runat="server"></asp:CheckBox>
						            </ItemTemplate>
						            <EditItemTemplate>
							            <asp:TextBox id="TextBox1" runat="server"></asp:TextBox>
						            </EditItemTemplate>
					            </asp:TemplateField>
					            <asp:TemplateField HeaderText="Nominativo">
						            <HeaderStyle  Width="200px"></HeaderStyle>
						            <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# FncGrd.FormattaNominativo(DataBinder.Eval(Container, "DataItem.COGNOME_DENOMINAZIONE"), DataBinder.Eval(Container, "DataItem.NOME")) %>'></asp:Label>
						            </ItemTemplate>
					            </asp:TemplateField>
					            <asp:BoundField DataField="COD_FISCALE" HeaderText="Codice Fiscale">
						            <HeaderStyle Width="120px"></HeaderStyle>
						            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					            </asp:BoundField>
					            <asp:BoundField DataField="PARTITA_IVA" HeaderText="Partita Iva">
						            <HeaderStyle Width="90px"></HeaderStyle>
						            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					            </asp:BoundField>
					            <asp:BoundField HeaderText="Indirizzo Residenza" DataField="residenza"></asp:BoundField>
					            <asp:TemplateField HeaderText="Sel">
						            <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
						            <ItemStyle HorizontalAlign="Center"></ItemStyle>
						            <ItemTemplate>
							            <asp:CheckBox id="ChkSelect" runat="server"></asp:CheckBox>
						            </ItemTemplate>
						            <EditItemTemplate>
							            <asp:TextBox id="Textbox2" runat="server"></asp:TextBox>
						            </EditItemTemplate>
					            </asp:TemplateField>
					            <asp:BoundField Visible="true" DataField="COD_CONTRIBUENTE" HeaderText="Codice Contribuente"></asp:BoundField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
										<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("COD_CONTRIBUENTE") %>' alt=""></asp:ImageButton>
					                    <asp:HiddenField runat="server" ID="hfIdDataAnagrafica" Value='<%# Eval("IDDATAANAGRAFICA") %>' />
									</itemtemplate>
								</asp:TemplateField>
				            </Columns>
                        </Grd:RibesGridView>
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
			<asp:button id="btnSalva" style="display: none" runat="server"></asp:button>
			<asp:button id="btnStampaExcel" style="display: none" runat="server"></asp:button>
		</form>
	</body>
</HTML>
