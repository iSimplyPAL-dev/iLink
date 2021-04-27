<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CheckRifCatastali.aspx.vb" Inherits="OPENgovTIA.CheckRifCatastali" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title></title>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	    <script type="text/javascript" src="../../_js/ControlloData.js?newversion"></script>
		<script type="text/javascript">
			//QUESTA FUNZIONE FA IN MODO CHE QUANDO VIENE PREMUTO IL TASTO INVIO IL FORM VENGA SPEDITO
			function ciao()
			{
				if (window.event.keyCode==13){
					MyCall('S')
				}
			}
			document.onkeydown = ciao				

			function MyCall(Tipo)
			{
				if(document.getElementById('DivAttesa')!=null)
				{
					document.getElementById('DivAttesa').style.display='';
				}
				if(document.getElementById('LblDownloadFile')!=null)
				{
					document.getElementById('LblDownloadFile').style.display='none';
				}
				if(document.getElementById('GrdCheckRifCatastali')!=null)
				{
					document.getElementById('GrdCheckRifCatastali').style.display='none';
				}
				if(document.getElementById('GrdCheckRifCatastaliAcc')!=null)
				{
					document.getElementById('GrdCheckRifCatastaliAcc').style.display='none';
				}
				if(document.getElementById('GrdCheckMQCatNoDic')!=null)
				{
					document.getElementById('GrdCheckMQCatNoDic').style.display='none';
				}
				if(document.getElementById('GrdCheckMQDicVsCat')!=null)
				{
					document.getElementById('GrdCheckMQDicVsCat').style.display='none';
				}
				if(document.getElementById('GrdDichMod')!=null)
				{
					document.getElementById('GrdDichMod').style.display='none';
				}
				if (Tipo == "S")
				    document.getElementById('CmdRicerca').click();
				if (Tipo=="P")
				    document.getElementById('CmdStampa').click()
			}
		</script>
    </head>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
		    <div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: hidden">
		        <table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
			        <tr vAlign="top">
				        <td align="left">
					        <span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
						        <asp:Label id="lblTitolo" runat="server"></asp:Label>
					        </span>
				        </td>
				        <td align="right" rowSpan="2">
						<input class="Bottone BottoneExcel" id="Stampa" title="Stampa" onclick="MyCall('P')" type="button" name="Stampa" />
						<input class="Bottone BottoneRicerca" id="Ricerca" title="Ricerca" onclick="MyCall('S')" type="button" name="Ricerca" />
				        </td>
			        </tr>
			        <tr>
				        <td align="left" colspan="2">
					        <span class="NormalBold_title" id="info" runat="server" style="WIDTH: 400px">- Analisi - Controlli Riferimenti Catastali</span>
				        </td>
			        </tr>
		        </table>
		    </div>
		    &nbsp;
		    <div id="divSearch">
			    <table id="tabEsterna" style="TOP: 0px; LEFT: 0px" cellSpacing="1" cellPadding="1" width="100%" border="0">
				    <tr>
					    <td>
						    <fieldset class="FiledSetRicerca">
							    <legend class="Legend">Inserimento filtri di ricerca</legend>
							    <table width="100%">
								    <!--*** 20130201 - gestione mq da catasto per TARES *** *** 20130619 - estrazione posizioni modificate ***-->
								    <tr width="100%">
									    <td>
										    <asp:RadioButton id="OptRifChiusi" Runat="server" CssClass="Input_Label" Checked="False" GroupName="OptCheckRifCat" Text="Riferimenti chiusi e non riaperti" AutoPostBack="True"></asp:RadioButton>
									    </td>
									    <td>
										    <asp:RadioButton id="OptCatNoDic" Runat="server" CssClass="Input_Label" Checked="False" GroupName="OptCheckRifCat" Text="Rif.Catastali non in Dichiarazioni" AutoPostBack="True"></asp:RadioButton>
									    </td>
									    <td>
										    <asp:RadioButton id="OptCatEqualDic" runat="server" CssClass="Input_Label" Checked="False" GroupName="OptCheckRifCat" Text="Sup.Dichiarata uguale a Catastale" AutoPostBack="True"></asp:RadioButton>
									    </td>
									    <td>
										    <asp:Label Runat="server" CssClass="Input_Label" id="Label1">Anno</asp:Label><br />
										    <asp:DropDownList id="DdlAnno" Runat="server" CssClass="Input_Label"></asp:DropDownList>
									    </td>
								    </tr>
								    <tr>
									    <td>
										    <asp:RadioButton id="OptRifMancanti" Runat="server" CssClass="Input_Label" Checked="False" GroupName="OptCheckRifCat" Text="Riferimenti mancanti" AutoPostBack="True"></asp:RadioButton>
									    </td>
									    <td>
										    <asp:RadioButton id="OptDicNoCat" Runat="server" CssClass="Input_Label" Checked="False" GroupName="OptCheckRifCat" Text="Rif.Dichiarati non a castato" AutoPostBack="True"></asp:RadioButton>
									    </td>
									    <td>
										    <asp:RadioButton id="OptCatDifferentDic" runat="server" CssClass="Input_Label" Checked="False" GroupName="OptCheckRifCat" Text="Sup.Dichiarata diversa da Catastale" AutoPostBack="True"></asp:RadioButton>
									    </td>
									    <td>
										    <asp:Label Runat="server" CssClass="Input_Label" id="Label4">Dal</asp:Label><br />
										    <asp:TextBox id="TxtDal" Runat="server" CssClass="Input_Text_Right TextDate" maxlength="10" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
									    </td>
									    <td>
										    <asp:Label Runat="server" CssClass="Input_Label" id="Label5">Al</asp:Label><br />
										    <asp:TextBox id="TxtAl" Runat="server" CssClass="Input_Text_Right TextDate" maxlength="10" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
									    </td>
								    </tr>
								    <tr>
									    <td>
										    <asp:RadioButton id="OptRifDoppi" Runat="server" CssClass="Input_Label" Checked="False" GroupName="OptCheckRifCat" Text="Riferimenti doppi per lo stesso periodo" AutoPostBack="True"></asp:RadioButton>
									    </td>
									    <td>
										    <asp:RadioButton id="OptDichMod" Runat="server" CssClass="Input_Label" Checked="False" GroupName="OptCheckRifCat" Text="Dichiarazioni modificate" AutoPostBack="True"></asp:RadioButton>
									    </td>
									    <td>
										    <asp:RadioButton id="OptCatGreaterDic" runat="server" CssClass="Input_Label" Checked="False" GroupName="OptCheckRifCat" Text="Sup.Catastale maggiore di Dichiarata" Tooltip="Sup.Catastale maggiore di dichiarata o non presente in catasto" AutoPostBack="True"></asp:RadioButton>
									    </td>
									    <td colspan="2">
										    <asp:Label Runat="server" CssClass="Input_Label" id="Label3">Operatore</asp:Label><br />
										    <asp:DropDownList id="DdlOperatore" Runat="server" CssClass="Input_Label"></asp:DropDownList>
									    </td>
								    </tr>
								    <tr>
									    <td>
										    <asp:RadioButton id="OptRifAccertati" Runat="server" CssClass="Input_Label" Checked="False" GroupName="OptCheckRifCat" Text="Riferimenti accertati"></asp:RadioButton>
									    </td>
									    <td>
										    <asp:RadioButton id="OptDichIns" Runat="server" CssClass="Input_Label" Checked="False" GroupName="OptCheckRifCat" AutoPostBack="true" Text="Dichiarazioni inserite"></asp:RadioButton>
									    </td>
									    <td>
										    <asp:RadioButton id="OptCatLowerDic" runat="server" CssClass="Input_Label" Checked="False" GroupName="OptCheckRifCat" Text="Sup.Catastale minore di Dichiarata" Tooltip="Sup.Catastale minore di dichiarata o non presente in dichiarazioni"></asp:RadioButton>
									    </td>
									    <td>
										    <asp:Label runat="server" CssClass="Input_Label" id="Label2">% tolleranza</asp:Label><br />
										    <asp:TextBox id="TxtTolleranza" Runat="server" CssClass="Input_Text" style="TEXT-ALIGN: right" Width="70px">0</asp:TextBox>
									    </td>
								    </tr>
								    <!--*** ***-->
							    </table>
						    </fieldset>
					    </td>
				    </tr>
				    <tr width="100%">
					    <td width="100%">
		                    <div id="divResult">
			                    <table cellSpacing="0" cellPadding="0" width="100%" border="0">
				                    <tr>
					                    <td>
                    						<asp:label id="LblResult" CssClass="Legend" Runat="server"></asp:label><br />
						                    <asp:LinkButton ID="LblDownloadFile" Runat="server" CssClass="Input_Label" Font-Underline="True"></asp:LinkButton>
					                    </td>
				                    </tr>
				                    <tr>
					                    <td>
                                            <Grd:RibesGridView ID="GrdCheckRifCatastali" runat="server" BorderStyle="None" 
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="20"
                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                                OnPageIndexChanging="GrdPageIndexChanging">
                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                <PagerStyle CssClass="CartListFooter" />
                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                    <Columns>
								                    <asp:TemplateField HeaderText="Nominativo">
									                    <ItemStyle Width="240px"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.sCognome") +" "+  DataBinder.Eval(Container, "DataItem.sNome")%>' ID="Label1">
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:BoundField DataField="sCodFiscalePIva" HeaderText="Cod.Fiscale/P.IVA">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
									                    <ItemStyle Width="300px"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label id="Label35" runat="server" text='<%# FncGrd.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"),DataBinder.Eval(Container, "DataItem.scivico"),DataBinder.Eval(Container, "DataItem.sInterno"),DataBinder.Eval(Container, "DataItem.sEsponente"),DataBinder.Eval(Container, "DataItem.sscala"),DataBinder.Eval(Container, "DataItem.sfoglio"),DataBinder.Eval(Container, "DataItem.snumero"),DataBinder.Eval(Container, "DataItem.ssubalterno")) %>'>Label</asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="Data Inizio Occupazione">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label id="Label34" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDATAINIZIO")) %>'>
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="Data Fine Occupazione">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label id="Label25" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDATAFINE")) %>'>
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="Categoria">
									                    <ItemTemplate>
										                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.sIdCategoria")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.sDescrCategoria")%>' ID="Label2">
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:BoundField DataField="nMQ" HeaderText="Tot.MQ" DataFormatString="{0:0.00}">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
							                    </Columns>
						                    </Grd:RibesGridView>
                                            <Grd:RibesGridView ID="GrdCheckRifCatastaliAcc" runat="server" BorderStyle="None" 
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="20"
                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                                OnPageIndexChanging="GrdPageIndexChanging">
                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                <PagerStyle CssClass="CartListFooter" />
                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                    <Columns>
								                    <asp:TemplateField HeaderText="Nominativo">
									                    <ItemStyle Width="240px"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.sCognome") +" "+  DataBinder.Eval(Container, "DataItem.sNome")%>' ID="Label3">
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:BoundField DataField="sCodFiscalePIva" HeaderText="Cod.Fiscale/P.IVA">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sFoglio" HeaderText="Foglio">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sNumero" HeaderText="Numero">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sSubalterno" HeaderText="Subalterno">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
							                    </Columns>
						                    </Grd:RibesGridView>
						                    <!--*** 20130201 - gestione mq da catasto per TARES ***-->
                                            <Grd:RibesGridView ID="GrdCheckMQDicVsCat" runat="server" BorderStyle="None" 
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="20"
                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                                OnPageIndexChanging="GrdPageIndexChanging">
                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                <PagerStyle CssClass="CartListFooter" />
                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                    <Columns>
								                    <asp:TemplateField HeaderText="Nominativo">
									                    <ItemStyle Width="240px"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.sCognome") +" "+  DataBinder.Eval(Container, "DataItem.sNome")%>' ID="Label4">
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:BoundField DataField="sCodFiscalePIva" HeaderText="Cod.Fiscale/P.IVA">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
									                    <ItemStyle Width="300px"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label id="Label5" runat="server" text='<%# FncGrd.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"),DataBinder.Eval(Container, "DataItem.scivico"),DataBinder.Eval(Container, "DataItem.sInterno"),DataBinder.Eval(Container, "DataItem.sEsponente"),DataBinder.Eval(Container, "DataItem.sscala"),DataBinder.Eval(Container, "DataItem.sfoglio"),DataBinder.Eval(Container, "DataItem.snumero"),DataBinder.Eval(Container, "DataItem.ssubalterno")) %>'>Label</asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="Data Inizio Occupazione">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label id="Label6" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDATAINIZIO")) %>'>
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="Data Fine Occupazione">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label id="Label7" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDATAFINE")) %>'>
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:BoundField DataField="nMQ" HeaderText="MQ Dich." DataFormatString="{0:0.00}">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="nMQCat" HeaderText="MQ Cat." DataFormatString="{0:0.00}">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="nMQDif" HeaderText="MQ Dif." DataFormatString="{0:0.00}">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
							                    </Columns>
						                    </Grd:RibesGridView>
                                            <Grd:RibesGridView ID="GrdCheckMQCatNoDic" runat="server" BorderStyle="None" 
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="20"
                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                                OnPageIndexChanging="GrdPageIndexChanging">
                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                <PagerStyle CssClass="CartListFooter" />
                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                    <Columns>
								                    <asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
									                    <ItemStyle Width="300px"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label id="Label9" runat="server" text='<%# FncGrd.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"),DataBinder.Eval(Container, "DataItem.scivico"),DataBinder.Eval(Container, "DataItem.sInterno"),DataBinder.Eval(Container, "DataItem.sEsponente"),DataBinder.Eval(Container, "DataItem.sscala"),"","","") %>'>Label</asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:BoundField DataField="sFoglio" HeaderText="Foglio">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sNumero" HeaderText="Numero">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sSubalterno" HeaderText="Subalterno">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="nMQ" HeaderText="Tot.MQ" DataFormatString="{0:0.00}">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
							                    </Columns>
						                    </Grd:RibesGridView>
						                    <!--*** ***-->
						                    <!--*** 20130619 - estrazione posizioni modificate ***-->
                                            <Grd:RibesGridView ID="GrdDichMod" runat="server" BorderStyle="None" 
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="20"
                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                                OnPageIndexChanging="GrdPageIndexChanging">
                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                <PagerStyle CssClass="CartListFooter" />
                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                    <Columns>
								                    <asp:TemplateField HeaderText="Nominativo">
									                    <ItemStyle Width="240px"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.sCognome") +" "+  DataBinder.Eval(Container, "DataItem.sNome")%>' ID="Label8">
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:BoundField DataField="sCodFiscalePIva" HeaderText="Cod.Fiscale/P.IVA">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sDescrCategoria" HeaderText="N.Dichiarazione">
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sAnno" HeaderText="Data Azione Testata">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sVia" HeaderText="Operatore Azione Testata">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sCivico" HeaderText="Data Azione Immobile">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sEsponente" HeaderText="Operatore Azione Immobile">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sScala" HeaderText="Data Azione Vano">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sInterno" HeaderText="Operatore Azione Vano">
									                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                    </asp:BoundField>
							                    </Columns>
						                    </Grd:RibesGridView>
						                    <!--*** ***-->
					                    </td>
				                    </tr>
				                    <tr>
					                    <td colSpan="3">
                                           <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                                                <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                                                <div class="BottoneClessidra">&nbsp;</div>
                                                <div class="Legend">Attendere Prego</div>
                                            </div>
					                    </td>
				                    </tr>
			                    </table>
		                    </div>
					    </td>
				    </tr>
			    </table>
		    </div>
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
			<asp:button style="DISPLAY: none" id="CmdRicerca" runat="server"></asp:button>
			<asp:button style="DISPLAY: none" id="CmdStampa" runat="server"></asp:button>
		</form>
	</body>
</html>
