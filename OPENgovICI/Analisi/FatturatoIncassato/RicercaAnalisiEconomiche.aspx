<%@ Page language="c#" Codebehind="RicercaAnalisiEconomiche.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.RicercaAnalisiEconomiche" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RicercaAnalisiEconomiche</title>
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
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript">
		    function LoadAnalisi() {
		        document.getElementById('DivAttesa').style.display = '';
		        //*** 20140630 - TASI ***
			    var Tributo = '';
			    if (document.getElementById('chkICI').checked == true && document.getElementById('chkTASI').checked == false)
			        Tributo = '8852';
			    else if (document.getElementById('chkICI').checked == false && document.getElementById('chkTASI').checked == true)
			        Tributo = 'TASI';
			    document.getElementById('LoadResult').src = 'ResultAnalisiEconomiche.aspx?DdlAnno=' + document.getElementById('DdlAnno').value + '&AccreditoDal=' + document.getElementById('TxtValutaDal').value + '&AccreditoAl=' + document.getElementById('TxtValutaAl').value + "&Tributo=" + Tributo;
			}
		</script>
	</HEAD>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="tabEsterna" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<tr>
					<td>
						<fieldset class="FiledSetRicerca" style="WIDTH: 100%">
							<legend class="Legend">Inserimento filtri di ricerca</legend>
							<table width="100%">
								<tr width="100%">
                                    <!--*** 20140509 - TASI ***-->
					                <td>
					                    <asp:CheckBox ID="chkICI" runat="server" CssClass="Input_Label" Text="ICI/IMU" Checked="true"/>
					                    <br />
					                    <asp:CheckBox ID="chkTASI" runat="server" CssClass="Input_Label" Text="TASI" Checked="false"/>
					                </td>
					                <!--*** ***-->
									<td><asp:label id="Label1" Runat="server" CssClass="Input_Label">Anno</asp:label><br />
										<asp:dropdownlist id="DdlAnno" Runat="server" CssClass="Input_Text" AutoPostBack="True"></asp:dropdownlist>
									</td>
									<td><asp:label id="Label4" Runat="server" CssClass="Input_Label">Data Accredito DAL</asp:label><br />
										<asp:textbox id="TxtValutaDal" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" Runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox>
									</td>
									<td><asp:label id="Label5" Runat="server" CssClass="Input_Label">AL</asp:label><br />
										<asp:textbox id="TxtValutaAl" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" Runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td colspan="6">
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
				<tr width="100%">
					<td width="100%" height="490">
						<iframe id="LoadResult" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="100%"></iframe>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
