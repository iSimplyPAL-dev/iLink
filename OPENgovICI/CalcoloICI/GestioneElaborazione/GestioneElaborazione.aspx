<%@ Page language="c#" Codebehind="GestioneElaborazione.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.GestioneElaborazione.GestioneElaborazione" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>GestioneElaborazione</title>
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
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0">
		<form id="Form1" runat="server" method="post">
			<table cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td>
						<FIELDSET class="classeFiledSetIframe"><LEGEND class="Legend">Elaborazioni 
								effettuate</LEGEND>
							<table cellPadding="0" width="100%" border="0">
								<tr>
									<td>
										<iframe style="WIDTH: 100%; HEIGHT: 300px" name="Repository" runat=server  src="../../aspVuota.aspx" frameBorder="no"
											id="Repository"></iframe>
									</td>
								</tr>
							</table>
						</FIELDSET>
					</td>
				</tr>
				<tr>
					<td>
						<FIELDSET class="classeFiledSetIframe"><LEGEND class="Legend">Stato 
								Elaborazione</LEGEND>
							<table cellPadding="0" width="100%" border="0">
								<tr>
									<td><iframe style="WIDTH: 100%; HEIGHT: 150px" name="ProgressTask" runat=server src="../../aspVuota.aspx"
											frameBorder="no" id="ProgressTask"></iframe>
									</td>
								</tr>
							</table>
						</FIELDSET>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
