<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RiepilogoAccertatoTARSU.aspx.vb" Inherits="Provvedimenti.RiepilogoAccertatoTARSU"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RiepilogoAccertatoTARSU</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
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
			WinPopUpSanzioni=window.open("../GestioneAccertamenti/Sanzioni/FrameSanzioni.aspx?"+Parametri,"",caratteristiche)
			//"width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
		}
		/*
		
		
		function ApriDettaglioSanzioni(idLegame,idCheck, idSanzioni,bloccaCheck)
		{
			if (eval("idCheck".checked) == false)
			{
				return false;
			}
			winWidth=600
			winHeight=300
			myleft=(screen.width-winWidth)/2 
			mytop=(screen.height-winHeight)/2 - 40 
			Parametri="idLegame=" + idLegame + "&strSanzioni=" + idSanzioni + "&bloccaCheck=" + bloccaCheck
			WinPopUpSanzioni=window.open("../GestioneAccertamenti/Sanzioni/FrameSanzioni.aspx?"+Parametri,"","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
		}*/
		</script>
	</HEAD>
	<BODY class="Sfondo" bottomMargin="0" topMargin="5" rightMargin="0" marginwidth="0" marginheight="0">
		<form id="Form1" runat="server" method="post">
			<FIELDSET class="classeFiledSet">
				<LEGEND class="Legend">Dati Contribuente</LEGEND>
				<table height="40" cellSpacing="1" cellPadding="1" width="728" align="left" border="0">
					<TR>
						<td class="Input_Label" width="123" height="10">Contribuente:</td>
						<td><asp:label id="LblContribuente" runat="server" CssClass="Input_Label"></asp:label></td>
					</TR>
					<TR>
						<TD class="Input_Label" width="123" height="10">Anno Accertamento:</TD>
						<td><asp:label id="LblAnnoAccertamento" runat="server" CssClass="Input_Label"></asp:label></td>
					</TR>
				</table>
			</FIELDSET>
			<br>
			<asp:label id="LblTipoAvviso" runat="server" CssClass="Input_Label" height="10" width="496px"></asp:label><br>
			<br>
			<FIELDSET class="classeFiledSet">
				<LEGEND class="Legend">Riepilogo Dati Dichiarato</LEGEND>
				<Grd:RibesGridView ID="GrdDichiarato" runat="server" BorderStyle="None" 
					BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
					AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
					ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
					OnRowDataBound="GrdRowDataBound">
					<PagerSettings Position="Bottom"></PagerSettings>
					<PagerStyle CssClass="CartListFooter" />
					<RowStyle CssClass="CartListItem"></RowStyle>
					<HeaderStyle CssClass="CartListHead"></HeaderStyle>
					<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
					<Columns>
						<asp:TemplateField HeaderText="Dal">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataInizio")) %>' ID="Label1">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Al">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataFine")) %>' ID="Label2">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Ubicazione">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.svia") & " " & FncGrd.FormattaNumeriGrd(DataBinder.Eval(Container, "DataItem.scivico")) & " " & DataBinder.Eval(Container, "DataItem.sinterno")%>' ID="Label29">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="sfoglio" ReadOnly="True" HeaderText="Foglio">
							<HeaderStyle Width="15px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="snumero" ReadOnly="True" HeaderText="Numero">
							<HeaderStyle Width="15px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="ssubalterno" ReadOnly="True" HeaderText="Sub.">
							<HeaderStyle Width="15px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="sdescrCategoria" ReadOnly="True" HeaderText="Cat.">
							<HeaderStyle Width="10px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
					    <asp:BoundField DataField="nComponenti" ReadOnly="True" HeaderText="NC">
						    <ItemStyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					    </asp:BoundField>
					    <asp:BoundField DataField="nComponentiPV" ReadOnly="True" HeaderText="NC PV">
						    <ItemStyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					    </asp:BoundField>
					    <asp:TemplateField HeaderText="Forza PV">
						    <ItemStyle horizontalalign="Center"></ItemStyle>
						    <ItemTemplate>
							    <asp:CheckBox id="chkForzaPV" runat="server" ReadOnly="True" Checked='<%# DataBinder.Eval(Container,"DataItem.bForzaPV")%>'></asp:CheckBox>
						    </ItemTemplate>
					    </asp:TemplateField>
						<asp:BoundField DataField="ImpTariffa" ReadOnly="True" HeaderText="Tariffa">
							<HeaderStyle Width="10px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="nMQ" ReadOnly="True" HeaderText="MQ">
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="nBimestri" ReadOnly="True" HeaderText="Bim">
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="ImpNetto" ReadOnly="True" HeaderText="Imp. Ruolo" DataFormatString="{0:#,##0.00}">
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="IdLegame" ReadOnly="True" HeaderText="Leg"></asp:BoundField>
					</Columns>
					</Grd:RibesGridView>
				<br>
				<asp:Label id="LblDich" runat="server" CssClass="Input_Label" Width="553px"></asp:Label>
			</FIELDSET>
			<br>
			<br>
			<FIELDSET class="classeFiledSet">
				<LEGEND class="Legend">Riepilogo Dati Accertato</LEGEND>
				<Grd:RibesGridView ID="GrdAccertato" runat="server" BorderStyle="None" 
					BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
					AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
					ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
					OnRowDataBound="GrdRowDataBound">
					<PagerSettings Position="Bottom"></PagerSettings>
					<PagerStyle CssClass="CartListFooter" />
					<RowStyle CssClass="CartListItem"></RowStyle>
					<HeaderStyle CssClass="CartListHead"></HeaderStyle>
					<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
					<Columns>
						<asp:TemplateField HeaderText="Dal">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataInizio")) %>' ID="Label4">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Al">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataFine")) %>' ID="Label5">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Ubicazione">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left" width="150px" VerticalAlign="Middle"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.svia") & " " & FncGrd.FormattaNumeriGrd(DataBinder.Eval(Container, "DataItem.scivico")) & " " & DataBinder.Eval(Container, "DataItem.sinterno")%>' ID="Label3">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="sfoglio" ReadOnly="True" HeaderText="Fg">
							<HeaderStyle Width="15px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="snumero" ReadOnly="True" HeaderText="Num">
							<HeaderStyle Width="15px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="ssubalterno" ReadOnly="True" HeaderText="Sub.">
							<HeaderStyle Width="15px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="sdescrCategoria" ReadOnly="True" HeaderText="Cat.">
							<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
					    <asp:BoundField DataField="nComponenti" ReadOnly="True" HeaderText="NC">
						    <ItemStyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					    </asp:BoundField>
					    <asp:BoundField DataField="nComponentiPV" ReadOnly="True" HeaderText="NC PV">
						    <ItemStyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					    </asp:BoundField>
					    <asp:TemplateField HeaderText="Forza PV">
						    <ItemStyle horizontalalign="Center"></ItemStyle>
						    <ItemTemplate>
							    <asp:CheckBox id="chkForzaPV" runat="server" ReadOnly="True" Checked='<%# DataBinder.Eval(Container,"DataItem.bForzaPV")%>'></asp:CheckBox>
						    </ItemTemplate>
					    </asp:TemplateField>
						<asp:BoundField DataField="ImpTariffa" ReadOnly="True" HeaderText="Tariffa">
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="nMQ" ReadOnly="True" HeaderText="MQ">
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="nBimestri" ReadOnly="True" HeaderText="Bim">
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="ImpRuolo" ReadOnly="True" HeaderText="Imp. Ruolo" DataFormatString="{0:#,##0.00}">
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="ImpNetto" ReadOnly="True" HeaderText="Diff. Imposta" DataFormatString="{0:#,##0.00}">
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="ImpSanzioni" ReadOnly="True" HeaderText="Sanzioni" DataFormatString="{0:#,##0.00}">
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:BoundField DataField="ImpInteressi" ReadOnly="True" HeaderText="Interessi" DataFormatString="{0:#,##0.00}">
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:TemplateField HeaderText="Imp. Totale">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# FncForGrd.CalcolaTotaliGrd(DataBinder.Eval(Container, "DataItem.ImpNetto"),DataBinder.Eval(Container, "DataItem.ImpInteressi"), DataBinder.Eval(Container, "DataItem.ImpSanzioni"))%>' ID="Label6">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField headertext="Sanz">
							<headerstyle width="20px"></headerstyle>
							<itemstyle horizontalalign="Center"></itemstyle>
							<itemtemplate>
								<asp:checkbox style="disabled:true;" id="chkSanzioni" runat="server"></asp:checkbox>
						        <asp:HiddenField runat="server" ID="hfSANZIONI" Value='<%# Eval("SANZIONI") %>' />
                                <asp:HiddenField runat="server" ID="hfid" Value='<%# Eval("id") %>' />
                                <asp:HiddenField runat="server" ID="hfsdescrSANZIONI" Value='<%# Eval("sdescrSANZIONI") %>' />
							</itemtemplate>
						</asp:TemplateField>
                        <asp:BoundField DataField="IdLegame" ReadOnly="True" HeaderText="Leg"></asp:BoundField>
					</Columns>
					</Grd:RibesGridView>
				<br>
				<asp:Label id="LblAcc" runat="server" CssClass="Input_Label" Width="553px"></asp:Label>
			</FIELDSET>
			<br>
			<br>
			<FIELDSET class="classeFiledSet">
				<LEGEND class="Legend">Riepilogo Totali</LEGEND>
				<table height="81" cellSpacing="1" cellPadding="1" width="350" align="left" border="0">
					<TR>
						<TD class="Input_Label" width="170" height="10">Differenza di imposta €:</TD>
						<td align="right"><asp:label id="LblDiffImpostaAvviso" runat="server" CssClass="Input_Label" Width="100px"></asp:label></td>
					</TR>
					<TR>
						<TD class="Input_Label" width="170" height="10">Sanzioni €:</TD>
						<td align="right"><asp:label id="LblSanzAvviso" runat="server" CssClass="Input_Label" Width="100px"></asp:label></td>
					</TR>
					<TR>
						<TD class="Input_Label" width="170" height="10">Sanzioni Ridotto €:</TD>
						<td align="right"><asp:label id="LblSanzRidotteAvviso" runat="server" CssClass="Input_Label" Width="100px"></asp:label></td>
					</TR>
					<TR>
						<TD class="Input_Label" width="170" height="10">Interessi €:</TD>
						<td align="right"><asp:label id="LblIntAvviso" runat="server" CssClass="Input_Label" Width="100px"></asp:label></td>
					</TR>
					<TR>
						<TD class="Input_Label" width="170" height="10">Totale €:</TD>
						<td align="right"><asp:label id="LblTotAvviso" runat="server" CssClass="Input_Label" Width="100px"></asp:label></td>
					</TR>
				</table>
			</FIELDSET>
			<div id="divMotivazione" class="divMotivazioneh">
				<div class="chiudi"><input class="Bottone Bottoneannulla" id="Annulla" title="Nascondi" onclick="document.getElementById('divMotivazione').className='divMotivazioneh'"
						type="button" name="Annulla"></div>
				<div id="motivazione" class="cmotivazione"></div>
			</div>
		</FORM>
	</BODY>
</HTML>
