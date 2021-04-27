<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RiepilogoAccertato.aspx.vb" Inherits="Provvedimenti.RiepilogoAccertato"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		function VisualizzaMotivazione(DescSanzione,Motivazione){

			document.getElementById("divMotivazione").className="divMotivazionev"
			
			testo="<p>&Egrave; stata applicata la sanzione<br><b>"+DescSanzione+"</b></p>"
			if (Motivazione!=""){
			testo+="con la seguente motivazione<p class='descMotivazione'>"+Motivazione+"</p>"
			}else{
			testo+="<p class='descMotivazione'>Nessuna motivazione Presente</p>"
			}
			
			document.getElementById("motivazione").innerHTML=testo
			return false
		}
		
		function ApriDettaglioSanzioni(idLegame,idCheck, idSanzioni,bloccaCheck,id_provvedimento)
		{
			if (eval("idCheck".checked) == false)
			{
				return false;
			}
			winWidth = 700
			winHeight = 600
			myleft = (screen.width - winWidth) / 2
			mytop=(screen.height-winHeight)/2 - 40 
			caratteristiche="toolbar=no,status,left="+myleft+",top="+mytop+",height ="+winHeight+",width="+winWidth+",resizable"
			Parametri="idLegame=" + idLegame + "&strSanzioni=" + idSanzioni + "&bloccaCheck=" + bloccaCheck +"&id_provvedimento=" + id_provvedimento
			WinPopUpSanzioni=window.open("./Sanzioni/FrameSanzioni.aspx?"+Parametri,"",caratteristiche)
		
			//"width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
		}
				
		</script>
	</head>
	<body class="Sfondo" bottomMargin="0" topMargin="5" rightMargin="0" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<fieldset class="classeFiledSet">
				<legend class="Legend">Riepilogo <asp:label id="lblTipoAvviso" runat="server" CssClass="Input_Label classeLegendPrincipale"></asp:label>&nbsp Anno <asp:label id="lblAnnoAccertamento" runat="server" CssClass="Input_Label classeLegendPrincipale"></asp:label></legend>			
                <iframe id="ifrmAnagRiepilogo" runat="server" src="../../aspVuota.aspx" style="height: 200px; width: 100%" frameborder="0" marginheight="0" ></iframe><br />                                	
			</fieldset>	
            				
			<fieldset class="classeFiledSet">
				<legend class="Legend">Riepilogo Dati Dichiarato</legend>
				<br />
				<!--*** 20120704 - IMU ***-->
				<Grd:RibesGridView ID="GrdDICH" runat="server" BorderStyle="None" 
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
						<asp:BoundField DataField="IDLegame" ReadOnly="True" HeaderText="Leg">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:TemplateField Visible="true" HeaderText="Dal">
							<headerstyle horizontalalign="Center" width="70px"></HeaderStyle>
							<itemstyle horizontalalign="Center"></ItemStyle>
							<itemtemplate>
								<asp:Label runat="server" Text='<%# ParseDate(DataBinder.Eval(Container, "DataItem.DAL")) %>' ID="Label1"></asp:Label>
                                <asp:HiddenField runat="server" ID="hfProgressivo" Value='<%# Eval("progressivo") %>' />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField Visible="true" HeaderText="Al">
							<headerstyle horizontalalign="Center" width="70px"></HeaderStyle>
							<itemstyle horizontalalign="Center"></ItemStyle>
							<itemtemplate>
								<asp:Label runat="server" Text='<%# ParseDate(DataBinder.Eval(Container, "DataItem.AL")) %>' ID="Label2">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="Foglio" ReadOnly="True" HeaderText="Fg">
							<HeaderStyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="Numero" ReadOnly="True" HeaderText="N&#176;">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:TemplateField headertext="Sub">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
							<itemtemplate>
								<asp:label runat="server" text='<%# IntForGridView(DataBinder.Eval(Container, "DataItem.Subalterno")) %>'>
								</asp:label>
							</itemtemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="Categoria" ReadOnly="True" HeaderText="Cat">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="CLASSE" ReadOnly="True" HeaderText="Cl">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right"></ItemStyle>
						</asp:BoundField>
						<asp:TemplateField Visible="true"  headertext="Cons">
							<headerstyle width="10px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
							<itemtemplate>
								<asp:label runat="server" text='<%# IntForGridView(DataBinder.Eval(Container, "DataItem.Consistenza")) %>' id="Label5" name="Label5">
								</asp:label>
							</itemtemplate>
						</asp:TemplateField>
						<asp:BoundField Visible="true" DataField="tiporendita" ReadOnly="True" HeaderText="TR">
							<headerstyle width="10px"></HeaderStyle>
							<itemstyle horizontalalign="Left"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="Valore" ReadOnly="True" HeaderText="Rend/Val" DataFormatString="{0:#,##0.00}">
							<headerstyle width="25px"></HeaderStyle>
							<itemstyle horizontalalign="Right"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField datafield="percPossesso" readonly="True" headertext="% Poss">
							<headerstyle width="15px"></headerstyle>
							<itemstyle horizontalalign="Right"></itemstyle>
						</asp:BoundField>
						<asp:BoundField Visible="False" DataField="percPossesso" ReadOnly="True" HeaderText="% Poss">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="TITPOSSESSO" ReadOnly="True" HeaderText="Tit.Poss">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Left"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField Visible="False" DataField="NumeroUtilizzatori" ReadOnly="True" HeaderText="N. Utiliz">
							<headerstyle width="20px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						</asp:BoundField>
					    <asp:TemplateField HeaderText="Princ">
						    <HeaderStyle Width="25px"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
						    <ItemTemplate>
							    <asp:CheckBox Enabled="False" id="CheckBox1" runat="server" Checked='<%# FncGrd.checkFlag(DataBinder.Eval(Container, "DataItem.flagprincipale")) %>' >
							    </asp:CheckBox>
						    </ItemTemplate>
					    </asp:TemplateField>
					    <asp:TemplateField HeaderText="Pert">
						    <HeaderStyle Width="25px"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
						    <ItemTemplate>
							    <asp:CheckBox Enabled="False" id="CheckBox2" runat="server" Checked='<%# FncGrd.checkPertinenza(DataBinder.Eval(Container, "DataItem.IdImmobilePertinenza"), DataBinder.Eval(Container, "DataItem.ID")) %>'>
							    </asp:CheckBox>
						    </ItemTemplate>
					    </asp:TemplateField>
					    <asp:TemplateField HeaderText="Rid">
						    <HeaderStyle Width="25px"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
						    <ItemTemplate>
							    <asp:CheckBox Enabled="False" id="CheckBox3" runat="server" Checked='<%# FncGrd.checkMesiRiduzione(DataBinder.Eval(Container, "DataItem.mesiriduzione")) %>'>
							    </asp:CheckBox>
						    </ItemTemplate>
					    </asp:TemplateField>
					    <asp:BoundField DataField="DESCRTIPOTASI" ReadOnly="True" HeaderText="Tipo">
						    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
					    </asp:BoundField>
						<asp:TemplateField Visible="False" HeaderText="Sanz">
							<headerstyle width="25px"></HeaderStyle>
							<itemstyle horizontalalign="Center"></ItemStyle>
							<itemtemplate>
								<asp:CheckBox Enabled="False" id="Checkbox4" runat="server"></asp:CheckBox>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField Visible="False" HeaderText="Modif">
							<itemstyle horizontalalign="Center"></ItemStyle>
							<itemtemplate>
								<asp:ImageButton id="Imagebutton1" runat="server" Width="16px" CommandName="Edit" ImageUrl="..\images\Bottoni\modifica.png" Height="19px"></asp:ImageButton>
							</ItemTemplate>
							<edititemtemplate>
								<asp:ImageButton id="Imagebutton3" runat="server" Width="16px" CommandName="Update" ImageUrl="..\images\Bottoni\modifica.png" Height="19px"></asp:ImageButton>
								<asp:ImageButton id="Imagebutton4" runat="server" Width="14px" CommandName="Cancel" ImageUrl="..\images\Bottoni\cancel.png" Height="17px"></asp:ImageButton>
							</EditItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField Visible="False" HeaderText="Del">
							<headerstyle horizontalalign="Center"></HeaderStyle>
							<itemstyle horizontalalign="Center"></ItemStyle>
							<itemtemplate>
								<asp:ImageButton id="imgDelete" runat="server" Width="16px" CommandName="Delete" ImageUrl="..\images\Bottoni\modifica.png" Height="19px"></asp:ImageButton>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="TOTDOVUTO" ReadOnly="True" HeaderText="Importo" DataFormatString="{0:#,##0.00}">
						<itemstyle horizontalalign="Right"></ItemStyle>
						</asp:BoundField>
					</Columns>
					</Grd:RibesGridView>
				<br />
				<asp:Label id="lblDich" cssclass ="Input_Label_14 bold" runat="server" Width="553px"></asp:Label>
			</fieldset>
			<br />
			<fieldset class="classeFiledSet">
				<legend class="Legend">Riepilogo Dati Accertato</legend>
				<br />
				<Grd:RibesGridView ID="GrdACC" runat="server" BorderStyle="None" 
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
						<asp:BoundField DataField="IDLegame" ReadOnly="True" HeaderText="Leg">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:TemplateField Visible="true" HeaderText="Dal">
							<headerstyle horizontalalign="Center" width="70px"></HeaderStyle>
							<itemstyle horizontalalign="Center"></ItemStyle>
							<itemtemplate>
								<asp:Label runat="server" Text='<%# ParseDate(DataBinder.Eval(Container, "DataItem.DAL")) %>' ID="lblDal"></asp:Label>
                                <asp:HiddenField runat="server" ID="hfProgressivo" Value='<%# Eval("progressivo") %>' />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField Visible="true" HeaderText="Al">
							<headerstyle horizontalalign="Center" width="70px"></HeaderStyle>
							<itemstyle horizontalalign="Center"></ItemStyle>
							<itemtemplate>
								<asp:Label runat="server" Text='<%# ParseDate(DataBinder.Eval(Container, "DataItem.AL")) %>' ID="lblAl">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="Foglio" ReadOnly="True" HeaderText="Fg">
							<HeaderStyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="Numero" ReadOnly="True" HeaderText="N&#176;">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:TemplateField headertext="Sub">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
							<itemtemplate>
								<asp:label runat="server" text='<%# IntForGridView(DataBinder.Eval(Container, "DataItem.Subalterno")) %>' id="Label3" name="Label3">
								</asp:label>
							</itemtemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="Categoria" ReadOnly="True" HeaderText="Cat">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="CLASSE" ReadOnly="True" HeaderText="Cl">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right"></ItemStyle>
						</asp:BoundField>
						<asp:TemplateField  headertext="Cons">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
							<itemtemplate>
								<asp:label runat="server" text='<%# IntForGridView(DataBinder.Eval(Container, "DataItem.Consistenza")) %>' id="Label4" name="Label3">
								</asp:label>
							</itemtemplate>
						</asp:TemplateField>
						<asp:BoundField Visible="true" DataField="tiporendita" ReadOnly="True" HeaderText="TR">
							<headerstyle width="10px"></HeaderStyle>
							<itemstyle horizontalalign="Left"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="Valore" ReadOnly="True" HeaderText="Rend/Val">
							<headerstyle width="25px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField datafield="percPossesso" readonly="True" headertext="% Poss">
							<headerstyle width="15px"></headerstyle>
							<itemstyle horizontalalign="Right"></itemstyle>
						</asp:BoundField>
						<asp:BoundField DataField="TITPOSSESSO" ReadOnly="True" HeaderText="Tit.Poss">
							<headerstyle width="15px"></HeaderStyle>
							<itemstyle horizontalalign="Left"></ItemStyle>
						</asp:BoundField>
					    <asp:TemplateField HeaderText="Princ">
						    <HeaderStyle Width="25px"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
						    <ItemTemplate>
							    <asp:CheckBox Enabled="False" id="chkPrinc" runat="server" Checked='<%# FncGrd.checkFlag(DataBinder.Eval(Container, "DataItem.flagprincipale")) %>' >
							    </asp:CheckBox>
						    </ItemTemplate>
					    </asp:TemplateField>
					    <asp:TemplateField HeaderText="Pert">
						    <HeaderStyle Width="25px"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
						    <ItemTemplate>
							    <asp:CheckBox Enabled="False" id="chkPert" runat="server" Checked='<%# FncGrd.checkPertinenza(DataBinder.Eval(Container, "DataItem.IdImmobilePertinenza"), DataBinder.Eval(Container, "DataItem.ID")) %>'>
							    </asp:CheckBox>
						    </ItemTemplate>
					    </asp:TemplateField>
					    <asp:TemplateField HeaderText="Rid">
						    <HeaderStyle Width="25px"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
						    <ItemTemplate>
							    <asp:CheckBox Enabled="False" id="chkRidotto" runat="server" Checked='<%# FncGrd.checkMesiRiduzione(DataBinder.Eval(Container, "DataItem.mesiriduzione")) %>'>
							    </asp:CheckBox>
						    </ItemTemplate>
					    </asp:TemplateField>
					    <asp:BoundField DataField="DESCRTIPOTASI" ReadOnly="True" HeaderText="Tipo">
						    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
					    </asp:BoundField>
						<asp:BoundField DataField="TOTDOVUTO" ReadOnly="True" HeaderText="Importo">
							<headerstyle width="25px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						</asp:BoundField>		
						<asp:BoundField DataField="DIFFIMPOSTA" ReadOnly="True" HeaderText="Diff. Imposta">
							<headerstyle width="25px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="IMPSanzioni" ReadOnly="True" HeaderText="Imp. Sanzioni">
							<headerstyle width="25px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="IMPInteressi" ReadOnly="True" HeaderText="Imp. Interessi">
							<headerstyle width="25px"></HeaderStyle>
							<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:TemplateField HeaderText="Sanz">
							<headerstyle width="20px"></HeaderStyle>
							<itemstyle horizontalalign="Center"></ItemStyle>
							<itemtemplate>
								<asp:CheckBox style="disabled:true;" id="chkSanzioni" runat="server"></asp:CheckBox>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField Visible="false" HeaderText="Modif">
							<itemstyle horizontalalign="Center"></ItemStyle>
							<itemtemplate>
								<asp:ImageButton id="imgEdit" runat="server" Width="16px" CommandName="Edit" ImageUrl="..\images\Bottoni\modifica.png" Height="19px"></asp:ImageButton>
							</ItemTemplate>
							<edititemtemplate>
								<asp:ImageButton id="imgUpdate" runat="server" Width="16px" CommandName="Update" ImageUrl="..\images\Bottoni\modifica.png" Height="19px"></asp:ImageButton>
								<asp:ImageButton id="ImageButton2" runat="server" Width="14px" CommandName="Cancel" ImageUrl="..\images\Bottoni\cancel.png" Height="17px"></asp:ImageButton>
							</EditItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField Visible="true" HeaderText="Totale">
							<headerstyle horizontalalign="Center" width="25px"></HeaderStyle>
							<itemstyle horizontalalign="Right"></ItemStyle>
							<itemtemplate>
								<asp:Label runat="server" Text='<%# FormattaNumero(DataBinder.Eval(Container, "DataItem.TOTALE"), 2) %>' ID="Label8"></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
					</Grd:RibesGridView>
				<br />
			</fieldset>
			<fieldset class="classeFiledSet">
				<legend class="Legend">Riepilogo Dati Fase 2 PreAccertamento</legend>
				<br />
				<table cellSpacing="0" cellPadding="2" align="left" width ="580px" border="0" class="Input_Label">
				<colgroup>
					<col width="80px"/>
					<col width="80px"/>
					<col width="80px"/>
					<col width="80px"/>
					<col width="160px"/>
					<col width="80px"/>
				</colgroup>
				<tr align="right">
					<td>Dichiarato</td>
					<td><asp:label id="lblTotDich1" runat="server"></asp:label>&nbsp&euro;</td>
					<td>Versato</td>
					<td><asp:label id="lblTotVers" runat="server"></asp:label>&nbsp&euro;</td>
					<td>Differenza di imposta</td>
					<td><asp:label id="lblDIF2" runat="server"></asp:label>&nbsp&euro;</td>
				</tr>
				<tr align="right">
					<td colspan="4">&nbsp;</td>
					<td>Sanzioni</td>
					<td><asp:label id="lblSANZF2" runat="server"></asp:label>&nbsp&euro;</td>
				</tr>
<!--				<tr align="right">
					<td colspan="4">&nbsp;</td>
					<td>Sanzioni Ridotto &euro;:</td>
					<td><asp:label id="lblSANZRIDOTTOF2" runat="server" ></asp:label></td>
				</tr>-->
				<tr align="right">
					<td colspan="4">&nbsp;</td>
					<td>Interessi</td>
					<td><asp:label id="lblINTF2" runat="server" ></asp:label>&nbsp&euro;</td>
				</tr>
				<tr align="right">
					<td colspan="4">&nbsp;</td>
					<td>Totale</td>
					<td><asp:label id="lblTOTF2" runat="server"></asp:label>&nbsp&euro;</td>
				</tr>
				</table>
				<br /><br />
			</fieldset>
			<br /><br />
			<fieldset class="classeFiledSet">
				<legend class="Legend">Riepilogo Dati Fase Accertamento </legend>
				<br />
				<table cellspacing="0" cellpadding="2" align="left" width ="580px" border="0" class="Input_Label">
				<colgroup>
					<col width="80px"/>
					<col width="80px"/>
					<col width="80px"/>
					<col width="80px"/>
					<col width="160px"/>
					<col width="80px"/>
				</colgroup>
				<tr align="right">
					<td>Accertato</td>
					<td><asp:label id="lbltotImpICI" runat="server"></asp:label>&nbsp&euro;</td>
					<td>Dichiarato</td>
					<td><asp:label id="lblTotDich2" runat="server"></asp:label>&nbsp&euro;</td>
					<td>Differenza di imposta</td>
					<td><asp:label id="lbltotDiffImp" runat="server"></asp:label>&nbsp&euro;</td>
				</tr>
				<tr align="right">
					<td colspan="4">&nbsp;</td>
					<td>Sanzioni</td>
					<td><asp:label id="lbltotImpSanz" runat="server"></asp:label>&nbsp&euro;</td>
				</tr>
				<tr align="right">
					<td colspan="4">&nbsp;</td>
					<td>Sanzioni Ridotte</td>
					<td><asp:label id="lbltotImpSanzRid" runat="server" ></asp:label>&nbsp&euro;</td>
				</tr>
				<tr align="right">
					<td colspan="4">&nbsp;</td>
					<td>Interessi</td>
					<td><asp:label id="lbltotInteressi" runat="server" ></asp:label>&nbsp&euro;</td>
				</tr>
				<tr align="right">
					<td colspan="4">&nbsp;</td>
					<td>Totale</td>
					<td><asp:label id="lbltotTotale" runat="server"></asp:label>&nbsp&euro;</td>
				</tr>
				</table>
				<br /><br />
			</fieldset>
			<br /><br />
			<fieldset class="classeFiledSet">
				<legend class="Legend">Riepilogo Totali</legend>
				<br />
				<table cellspacing="0" cellpadding="2" align="left" width ="580px" border="0" class="Input_Label">
				<colgroup>
					<col width="80px"/>
					<col width="80px"/>
					<col width="80px"/>
					<col width="80px"/>
					<col width="160px"/>
					<col width="80px"/>
				</colgroup>
				<tr align="right">
					<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
					<td>Differenza di imposta</td>
					<td><asp:label id="lblDIAVVISO" runat="server"  ></asp:label>&nbsp&euro;</td>
				</tr>
				<tr align="right">
					<td colspan="4">&nbsp;</td>
					<td>Sanzioni</td>
					<td><asp:label id="lblSANZAVVISO" runat="server" ></asp:label>&nbsp&euro;</td>
				</tr>
				<tr align="right">
					<td colspan="4">&nbsp;</td>
					<td>Sanzioni Ridotto</td>
					<td><asp:label id="lblSANZRIDOTTOAVVISO" runat="server"></asp:label>&nbsp&euro;</td>
				</tr>
				<tr align="right">
					<td colspan="4">&nbsp;</td>
					<td>Interessi</td>
					<td><asp:label id="lblINTAVVISO" runat="server"></asp:label>&nbsp&euro;</td>
				</tr>
				<tr align="right">
					<td colspan="4">&nbsp;</td>
					<td>Totale</td>
					<td><asp:label id="lblTOTALEAVVISO" runat="server"></asp:label>&nbsp&euro;</td>
				</tr>
				</table>
			</fieldset>
			<div id="divMotivazione" class="divMotivazioneh">
				<div class="chiudi"><input class="Bottone Bottoneannulla" id="Annulla" title="Nascondi" onclick="document.getElementById('divMotivazione').className='divMotivazioneh'" type="button" name="Annulla"></div>
				<div id="motivazione" class="cmotivazione"></div>
			</div>
            <asp:Button ID="CmdBack" runat="server" CssClass="hidden" />
		</form>
	</body>
</html>
