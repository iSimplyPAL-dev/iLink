<%@ Page language="c#" Codebehind="ComandiAnalisiEconomiche.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.Analisi.FatturatoIncassato.ComandiAnalisiEconomiche" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ComandiAnalisiEconomiche</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		    function LoadGrafico(){
		        var myIFrame = parent.Visualizza.document.getElementById('LoadResult');
		        var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
		        myContent.LoadGrafico();
		    }
		</script>
	</HEAD>
	<BODY  MS_POSITIONING="GridLayout" class="SfondoGenerale" bottomMargin="0" leftMargin="2"
		topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				<tr>
					<td style="WIDTH: 464px; HEIGHT: 20px" align="left">
						<span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label>
						</span>
					</td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<input class="Bottone BottoneGrafico" id="Grafico" title="Visualizza grafico." onclick="LoadGrafico()" type="button" name="Grafico"> 
                        <input class="Bottone BottoneRicerca" id="Ricerca" title="Ricerca." onclick="parent.Visualizza.LoadAnalisi()" type="button" name="Ricerca">
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 463px" align="left">
						<!--*** 20120704 - IMU ***-->
						<span class="NormalBold_title" id="info" style="WIDTH: 400px; HEIGHT: 20px">
							ICI/IMU - Analisi - Economiche</span>
					</td>
				</tr>
			</table>
			&nbsp;
		</form>
	</BODY>
</HTML>
