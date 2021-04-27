<%@ Page language="C#" Codebehind="ConfigAliquote.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.ConfigAliquote" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN" >
<html>
	<head>
		<title>ConfigAliquote</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" type="text/javascript">		
		function IsNumeric(sText)
		{
			var ValidChars = "0123456789.";
			var IsNumber=true;
			var Char;

				
			for (i = 0; i < sText.length && IsNumber == true; i++) 
				{ 
				Char = sText.charAt(i); 
				if (ValidChars.indexOf(Char) == -1) 
					{
					IsNumber = false;
					}
				}
			return IsNumber;
			
		}	
		
		function controlliSalva()
		{
		    if (document.getElementById('txtAnno').value=='')
		    {
		        GestAlert('a', 'warning', '', '', 'Inserire l\'anno');
		        return false;
		    }
		    //*** 20140509 - TASI ***
		    if (document.getElementById('ddlTributo').value=='-1')
		    {
		        GestAlert('a', 'warning', '', '', 'Inserire il tributo');
		        return false;
		    }
		if(document.getElementById('txtSogliaRendita').value=='')
			{
			    GestAlert('a', 'warning', '', '', 'Inserire un Soglia Rendita!');
				return false;
			}
								
				strValue=document.getElementById('txtSogliaRendita').value;						
			strValue = strValue.replace(",",".");
			
			if(!IsNumeric(strValue))
			{
			    GestAlert('a', 'warning', '', '', 'Soglia Rendita inserita in un formato non valido!');
				return false;
			}
			//*** ***				
			if(document.getElementById('txtValore').value=='' && document.getElementById('txtMagDetraz').value=='')
			{
			    GestAlert('a', 'warning', '', '', 'Inserire un valore!');
				return false;
			}
								
			strValue=document.getElementById('txtValore').value;						
			strValue = strValue.replace(",",".");
			//alert(strValuePerc)
			
			if(!IsNumeric(strValue))
			{
			    GestAlert('a', 'warning', '', '', 'Valore inserito in un formato non valido!');
				return false;
			}				
			
			if (!confirm('Si vuole proseguire con il salvataggio?'))
			{
				return false;
			}
			else
			{								
			    document.getElementById('btnSalva').click()
			}
		}	
		
		function controlliElimina()
		{
			if(<%=ViewState["ID_ALIQUOTA"]%> ==-1)
			{
			    GestAlert('a', 'warning', '', '', 'Impossibile eseguire l\'eliminazione dell\'aliquota/detrazione!');
				return false;
			}
			else
			{	
			
				if (!confirm('Si vuole proseguire con l\'eliminazione?'))
				{
					return false;
				}
				else
				{								
				    document.getElementById('btnDelete').click()
				}			
			}
		}
		/*
		function NascondiVisualizzaDSAAP(valore){
			if (valore=="none"){
				//document.getElementById ("txtValore").value=document.getElementById ("txtMagDetraz").value;
				document.getElementById ("DSAAP").style.display="none";
				document.getElementById ("txtValore").style.display="";
				document.getElementById ("lblValore").style.display="";
				
			}else{
				//document.getElementById ("txtMagDetraz").value=document.getElementById ("txtValore").value;
				document.getElementById ("DSAAP").style.display="";
				document.getElementById ("txtValore").style.display="none";
				document.getElementById ("lblValore").style.display="none";
				
			}
		
		}*/
		
		function NascondiVisualizzaAAP(valore){
			if (valore=="none"){
				document.getElementById ("DSAAP").style.display="none";
				document.getElementById ("txtValore").style.display="";
				document.getElementById ("lblValore").style.display="";
				
			}else{
				document.getElementById ("DSAAP").style.display="";
				document.getElementById ("txtValore").style.display="";
				document.getElementById ("lblValore").style.display="";
				document.getElementById("lblMagDetraz").style.display="none";
				document.getElementById("txtMagDetraz").style.display="none";
				document.getElementById("lblTettoMassimo").style.display="none";
				document.getElementById("txtTettoMassimo").style.display="none";
			}
		}
		function NascondiVisualizzaAUG(valore){
			if (valore=="none"){
				document.getElementById ("DSAAP").style.display="none";
				document.getElementById ("txtValore").style.display="";
				document.getElementById ("lblValore").style.display="";
			}else{
				document.getElementById ("DSAAP").style.display="";
				document.getElementById ("txtValore").style.display="";
				document.getElementById ("lblValore").style.display="";
				document.getElementById("lblMagDetraz").style.display="none";
				document.getElementById("txtMagDetraz").style.display="none";
				document.getElementById("lblTettoMassimo").style.display="none";
				document.getElementById("txtTettoMassimo").style.display="none";
			}
		}
		function NascondiVisualizzaAPEX(valore){
			if (valore=="none"){
				document.getElementById ("DSAAP").style.display="none";
				document.getElementById ("txtValore").style.display="";
				document.getElementById ("lblValore").style.display="";
			}else{
				document.getElementById ("DSAAP").style.display="";
				document.getElementById ("txtValore").style.display="";
				document.getElementById ("lblValore").style.display="";
				document.getElementById("lblMagDetraz").style.display="none";
				document.getElementById("txtMagDetraz").style.display="none";
				document.getElementById("lblTettoMassimo").style.display="none";
				document.getElementById("txtTettoMassimo").style.display="none";
			}
		}
		
		function ControllaValore(){
			var valoreddltipo=document.getElementById ("ddlTipo").value
			
			switch (valoreddltipo){
				case "AAP":
					NascondiVisualizzaAAP('');
					break;
				case "AUG1":
				case "AUG2":
				case "AUG3":
					NascondiVisualizzaAUG('');
					break;
				case "APEX":
					NascondiVisualizzaAPEX('');
				default:
					NascondiVisualizzaAUG('none');
					NascondiVisualizzaAAP('none');
					NascondiVisualizzaAPEX('none');
			}
			
			/*if (valoreddltipo=="AAP"){
				NascondiVisualizzaAAP('');
			}else{
				if (valoreddltipo=="AUG1" || valoreddltipo=="AUG2" || valoreddltipo=="AUG3"){
					NascondiVisualizzaAUG('');
				}else{
					NascondiVisualizzaAUG('none');
					NascondiVisualizzaAAP('none');
				}
			}*/
		}
		
		</script>
	</head>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0">
		<form id="Form1" runat="server" method="post">
			<br />
			<table id="tblRicerca" cellspacing="1" cellpadding="1" width="100%" align="center" border="0">
				<tr>
					<td>
						<asp:label id="Label3" runat="server" CssClass="Input_Label">Anno</asp:label>
					</td>
					<!--*** 20140509 - TASI ***-->
					<td>
						<asp:Label id="Label5" runat="server" CssClass="Input_Label">Tributo</asp:Label>
					</td>
					<!--*** ***-->
					<td>
						<asp:label id="Label1" runat="server" CssClass="Input_Label">Tipo Aliquota</asp:label>
					</td>
					<td>
						<asp:label id="lblValore" runat="server" CssClass="Input_Label">Valore</asp:label>
					</td>
					<td>
						<asp:label id="Label2" runat="server" CssClass="Input_Label">Statale</asp:label>
					</td>
					<!--*** 20150430 - TASI Inquilino ***-->
					<td>
					    <asp:Label runat="server" CssClass="Input_Label">% Inquilino/Comodatario</asp:Label>
					</td>
					<!--*** ***-->
					<!--*** 20140509 - TASI ***-->
					<td colspan="2">
						<asp:Label id="Label6" runat="server" CssClass="Input_Label">Soglia Rendita</asp:Label>
					</td>
					<!--*** ***-->
				</tr>
				<tr>
					<td>
						<asp:TextBox id="txtAnno" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="90px"></asp:TextBox>
					</td>
					<!--*** 20140509 - TASI ***-->
					<td>
						<asp:DropDownList id="ddlTributo" runat="server" CssClass="Input_Text" Width="130px"></asp:DropDownList>
					</td>
					<!--*** ***-->
					<td>
						<asp:dropdownlist id="ddlTipo" runat="server" CssClass="Input_Text" Width="410px" onchange="ControllaValore()" DataValueField="TIPO" DataTextField="DESCR" DataSource="<%# GetTipoAliquote() %>"></asp:dropdownlist>
					</td>
					<td>
						<asp:textbox id="txtValore" runat="server" CssClass="Input_Text_Right" Width="100px">0</asp:textbox>
					</td>
					<td>
						<asp:textbox id="TxtAliquotaStatale" runat="server" CssClass="Input_Text_Right" Width="100px">0</asp:textbox>
					</td>
					<!--*** 20150430 - TASI Inquilino ***-->
					<td>
					    <asp:TextBox ID="TxtPercInquilino" runat="server" CssClass="Input_Text_Right" Width="60px">0</asp:TextBox>
					</td>
					<!--*** ***-->
					<!--*** 20140509 - TASI ***-->
					<td>
					    <asp:RadioButton ID="optSogliaRenditaFinoA" GroupName="TipoSogliaRendita" runat="server" CssClass="Input_Radio" Text="Fino A" Visible="false" />
					    <br />
					    <asp:RadioButton ID="optSogliaRenditaPartireDa" GroupName="TipoSogliaRendita" runat="server" CssClass="Input_Radio" Text="A partire da" Visible="false" />
					</td>
					<td>
						<asp:TextBox ID="txtSogliaRendita" runat="server" CssClass="Input_Text_Right" Width="100px">0</asp:TextBox>
					</td>
					<!--*** ***-->
				</tr>
				<tr>
					<td colspan="8">&nbsp;</td>
				</tr>
				<tr id="DSAAP">
					<td colspan="8">
						<!--<asp:label id="Label4" runat="server" CssClass="Input_Label_title">Configurazione per Abitazione Principale</asp:label>-->
						<table id="tblDSAAP" cellspacing="1" cellpadding="1" width="100%" align="center" border="0">
							<tr>
								<td>
									<asp:label id="lblMagDetraz" runat="server" CssClass="Input_Label">Maggior Detrazione ‰</asp:label>
								</td>
								<td>
									<asp:label id="lblTettoMassimo" runat="server" CssClass="Input_Label">Tetto Massimo €</asp:label>
								</td>
							</tr>
							<tr>
								<td>
									<asp:textbox id="txtMagDetraz" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="60px" onkeypress="return NumbersOnly(event,true,false,2);"></asp:textbox>
								</td>
								<td>
									<asp:textbox id="txtTettoMassimo" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="60px" onkeypress="return NumbersOnly(event,true,false,2);"></asp:textbox>
								</td>
							</tr>
							<tr>
								<td>
									<asp:label id="Label7" runat="server" CssClass="Input_Label_bold">Elenco Categorie da escludere</asp:label>
								</td>
								<td><!--<asp:label id=Label8 runat="server" CssClass="Input_Label">Elenco Titoli di possesso da escludere</asp:Label>--></td>
							</tr>
							<tr>
								<td>
									<div class="divOver">
										<Grd:RibesGridView ID="GrdCategorie" runat="server" BorderStyle="None" 
                                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                            <PagerSettings Position="Bottom"></PagerSettings>
                                            <PagerStyle CssClass="CartListFooter" />
                                            <RowStyle CssClass="CartListItem"></RowStyle>
                                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
											<Columns>
												<asp:BoundField DataField="ID" HeaderText="ID" visible="false">
													<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="CategoriaCatastale" HeaderText="Categoria Catastale">
													<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="DESCRIZIONE" HeaderText="Descrizione">
													<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
												</asp:BoundField>
												<asp:TemplateField HeaderText="Escludi">
													<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
													<ItemTemplate>
														<asp:CheckBox id="chkEsclusione" runat="server"></asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateField>
											</Columns>
										</Grd:RibesGridView>
									</div>
								</td>
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
			<asp:button id="btnDelete" style="DISPLAY: none" runat="server" onclick="btnDelete_Click"></asp:button>
			<asp:button id="btnSalva" style="DISPLAY: none" runat="server" onclick="btnSalva_Click"></asp:button>
		</form>
	</body>
</html>
