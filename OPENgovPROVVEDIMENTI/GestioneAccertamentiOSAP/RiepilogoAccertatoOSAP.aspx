<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RiepilogoAccertatoOSAP.aspx.vb" Inherits="Provvedimenti.RiepilogoAccertatoOSAP"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RiepilogoAccertatoOSAP</title>
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
						<asp:BoundField DataField="Progressivo" ReadOnly="True" HeaderText="Prg">
							<headerstyle width="10px"></headerstyle>
							<itemstyle horizontalalign="Center"></itemstyle>
						</asp:BoundField>
						<asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
							<ItemStyle Width="300px"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label35" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"),DataBinder.Eval(Container, "DataItem.Civico"),DataBinder.Eval(Container, "DataItem.Interno"),DataBinder.Eval(Container, "DataItem.Esponente"),DataBinder.Eval(Container, "DataItem.Scala")) %>'>Label</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Data Inizio">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label7" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataInizioOccupazione"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Data Fine">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label8" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataFineOccupazione"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Durata">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle Width="120px" HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label28" runat="server" text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.DurataOccupazione"),DataBinder.Eval(Container, "DataItem.TipoDurata.Descrizione"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Tipo Occup.">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label27" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.TipologiaOccupazione.Descrizione"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Cat.">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label9" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.Categoria.Descrizione"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Cons.">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label33" runat="server" text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.Consistenza"), DataBinder.Eval(Container, "DataItem.TipoConsistenzaTOCO.Descrizione"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Tariffa">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemTemplate>
								<asp:Label id="Label10" runat="server" text='<%# FncForGrd.FormattaCalcolo("T",DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Imp.">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemTemplate>
								<asp:Label id="Label11" runat="server" text='<%# FncForGrd.FormattaCalcolo("I",DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
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
						<asp:BoundField DataField="Progressivo" ReadOnly="True" HeaderText="Prg">
							<headerstyle width="10px"></headerstyle>
							<itemstyle horizontalalign="Center"></itemstyle>
						</asp:BoundField>
						<asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
							<ItemStyle Width="300px"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label1" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"), DataBinder.Eval(Container, "DataItem.Civico"), DataBinder.Eval(Container, "DataItem.Interno"), DataBinder.Eval(Container, "DataItem.Esponente"), DataBinder.Eval(Container, "DataItem.Scala")) %>'>Label</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Data Inizio">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label2" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataInizioOccupazione"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Data Fine">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label3" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataFineOccupazione"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Durata">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle Width="120px" HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label4" runat="server" text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.DurataOccupazione"),DataBinder.Eval(Container, "DataItem.TipoDurata.Descrizione"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Tipo Occup.">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label5" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.TipologiaOccupazione.Descrizione"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Cat.">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label12" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.Categoria.Descrizione"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Cons.">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label13" runat="server" text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.Consistenza"), DataBinder.Eval(Container, "DataItem.TipoConsistenzaTOCO.Descrizione"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Tariffa">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemTemplate>
								<asp:Label id="Label14" runat="server" text='<%# FncForGrd.FormattaCalcolo("T",DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Imp.">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemTemplate>
								<asp:Label id="Label15" runat="server" text='<%# FncForGrd.FormattaCalcolo("I",DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="ImpDiffImposta" ReadOnly="True" HeaderText="Diff. Imposta" DataFormatString="{0:#,##0.00}">
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
								<asp:Label runat="server" Text='<%# FncForGrd.CalcolaTotaliGrd(DataBinder.Eval(Container, "DataItem.ImpDiffImposta"),DataBinder.Eval(Container, "DataItem.ImpInteressi"), DataBinder.Eval(Container, "DataItem.ImpSanzioni"))%>' ID="Label6">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField headertext="Sanz">
							<headerstyle width="20px"></headerstyle>
							<itemstyle horizontalalign="Center"></itemstyle>
							<itemtemplate>
								<asp:checkbox style="disabled:true;" id="chkSanzioni" runat="server"></asp:checkbox>                                
                                <asp:HiddenField runat="server" ID="hfIDSANZIONI" Value='' />
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
			<fieldset id="fldPreAcc" runat="server"><legend class="Legend">Riepilogo Dati PreAccertamento</legend><br>
				<table class="Input_Label" border="0" cellSpacing="0" cellPadding="2" width="580" align="left">
					<colgroup>
						<col width="80">
						<col width="80">
						<col width="80">
						<col width="80">
						<col width="160">
						<col width="80">
					</colgroup>
					<tr align="right">
						<td>Dichiarato</td>
						<td><asp:label id="LblImpDichPreAcc" runat="server"></asp:label></td>
						<td>Versato</td>
						<td><asp:label id="LblImpVersPreAcc" runat="server"></asp:label></td>
						<td>Differenza di imposta</td>
						<td><asp:label id="LblImpDifImpPreAcc" runat="server"></asp:label></td>
					</tr>
					<tr align="right">
						<td colSpan="4">&nbsp;</td>
						<td>Sanzioni</td>
						<td><asp:label id="LblImpSanzPreAcc" runat="server"></asp:label></td>
					</tr>
					<tr align="right">
						<td colSpan="4">&nbsp;</td>
						<td>Interessi</td>
						<td><asp:label id="LblImpIntPreAcc" runat="server"></asp:label></td>
					</tr>
					<tr align="right">
						<td colSpan="4">&nbsp;</td>
						<td>Totale</td>
						<td><asp:label id="LblImpTotPreAcc" runat="server"></asp:label></td>
					</tr>
				</table>
			</fieldset>
			<fieldset id="fldAcc" runat="server"><legend class="Legend">Riepilogo Dati Fase Accertamento</legend><br>
				<table class="Input_Label" border="0" cellSpacing="0" cellPadding="2" width="580" align="left">
					<colgroup>
						<col width="80">
						<col width="80">
						<col width="80">
						<col width="80">
						<col width="160">
						<col width="80">
					</colgroup>
					<tr align="right">
						<td>Dichiarato</td>
						<td><asp:label id="LblImpDich" runat="server"></asp:label></td>
						<td>Accertato</td>
						<td><asp:label id="LblImpAcc" runat="server"></asp:label></td>
						<td>Differenza di imposta</td>
						<td><asp:label id="LblImpDifImp" runat="server"></asp:label></td>
					</tr>
					<tr align="right">
						<td colSpan="4">&nbsp;</td>
						<td>Sanzioni</td>
						<td><asp:label id="LblImpSanz" runat="server"></asp:label></td>
					</tr>
					<tr align="right">
						<td colSpan="4">&nbsp;</td>
						<td>Sanzioni Ridotte</td>
						<td><asp:label id="LblImpSanzRid" runat="server"></asp:label></td>
					</tr>
					<tr align="right">
						<td colSpan="4">&nbsp;</td>
						<td>Interessi</td>
						<td><asp:label id="LblImpInt" runat="server"></asp:label></td>
					</tr>
					<tr align="right">
						<td colSpan="4">&nbsp;</td>
						<td>Totale</td>
						<td><asp:label id="LblImpTot" runat="server"></asp:label></td>
					</tr>
				</table>
			</fieldset>
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
