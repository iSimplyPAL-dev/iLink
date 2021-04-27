<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiInserimentoTipoVoci.aspx.vb" Inherits="Provvedimenti.ComandiInserimentoTipoVoci"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiInserimentoTipoVoci</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoGenerale" bottomMargin="0" leftMargin="2" topMargin="6"
		rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td style="WIDTH: 475px; HEIGHT: 18px" align="left"><span id="infoEnte" runat="server" class="ContentHead_Title" style="WIDTH:400px"></span></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
                        <INPUT class="Bottone BottonePulisci hidden" id="Pulisci" title="Pulisci Videata" onclick="parent.Visualizza.Clear()" type="button" name="Clear" style="display:none" />
						<INPUT class="Bottone BottoneApri" id="Abilita" title="Abilita modifica" onclick="parent.Visualizza.AbilitaCombo()" type="button" name="Abilita" />
                        <INPUT class="Bottone Bottonecancella" id="Elimina" title="Elimina voce" onclick="parent.Visualizza.Delete()" type="button" name="Elimina" />
                        <INPUT class="Bottone BottoneDetail" id="Configura" title="Configura Valori" onclick="parent.Visualizza.Configura()" type="button" name="Configura" />
                        <INPUT class="Bottone BottoneSalva" id="Salva" title="Salva" onclick="parent.Visualizza.Save()" type="button" name="Save" />
                        <INPUT class="Bottone Bottoneannulla" id="Torna" title="Torna indietro" onclick="parent.Visualizza.Back()" type="button" name="Torna" />
					</td>
				</tr>
				<TR>
					<TD style="WIDTH: 475px" align="left">
						<span id="info" runat="server" class="NormalBold_title" style="WIDTH:524px;HEIGHT:20px"> </span>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
