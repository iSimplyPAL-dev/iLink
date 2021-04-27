<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaAnalisiEconomiche.aspx.vb" Inherits="OpenUtenze.RicercaAnalisiEconomiche" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaAnalisiEconomiche</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">
			//QUESTA FUNZIONE FA IN MODO CHE QUANDO VIENE PREMUTO IL TASTO INVIO IL FORM VENGA SPEDITO
			function Search()
			{
				if (window.event.keyCode==13){
					LoadAnalisi()
				}
			}
			document.onkeydown = Search				
			
			//*** 20130204 - analisi economiche senza filtro per ente
			function LoadAnalisi(TypeRicerca)
			{
				//document.frames.item('LoadResult').location.href = 'ResultAnalisiEconomiche.aspx?DdlAnno='+ParametriRicerca.DdlAnno.value+'&DdlPeriodo='+ParametriRicerca.DdlPeriodo.value+'&AccreditoDal='+ParametriRicerca.TxtValutaDal.value+'&AccreditoAl='+ParametriRicerca.TxtValutaAl.value
			    document.getElementById('LoadResult').src = 'ResultAnalisiEconomiche.aspx?DdlAnno=' + ParametriRicerca.DdlAnno.value + '&DdlPeriodo=' + ParametriRicerca.DdlPeriodo.value + '&AccreditoDal=' + ParametriRicerca.TxtValutaDal.value + '&AccreditoAl=' + ParametriRicerca.TxtValutaAl.value + '&TypeRicerca=' + TypeRicerca
			}
		</script>
	</HEAD>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<table id="tabEsterna" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px" cellSpacing="1"
				cellPadding="1" width="100%" border="0">
				<tr>
					<td>
						<FIELDSET class="FiledSetRicerca" style="WIDTH: 100%">
							<LEGEND class="Legend">
								Inserimento filtri di ricerca</LEGEND>
							<table width="100%">
								<tr width="100%">
									<td><asp:label id="Label2" Runat="server" CssClass="Input_Label">Anno</asp:label><br>
										<asp:dropdownlist id="DdlAnno" Runat="server" CssClass="Input_Text" AutoPostBack="True"></asp:dropdownlist>
									</td>
									<td><asp:label id="Label1" Runat="server" CssClass="Input_Label">Periodo</asp:label><br>
										<asp:dropdownlist id="DdlPeriodo" Runat="server" CssClass="Input_Text" AutoPostBack="True"></asp:dropdownlist>
									</td>
									<td><asp:label id="Label4" Runat="server" CssClass="Input_Label">Data Accredito DAL</asp:label><br>
										<asp:textbox id="TxtValutaDal" Runat="server" CssClass="Input_Text" Width="120px" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox>
									</td>
									<td><asp:label id="Label5" Runat="server" CssClass="Input_Label">AL</asp:label><br>
										<asp:textbox id="TxtValutaAl" Runat="server" CssClass="Input_Text" Width="120px" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox>
									</td>
								</tr>
							</table>
						</FIELDSET>
					</td>
				</tr>
				<TR width="100%">
					<td width="100%" height="470">
						<IFRAME id="LoadResult" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="100%"></IFRAME>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
