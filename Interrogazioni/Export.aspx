<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Export.aspx.vb" Inherits="OPENgov.Export" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<link href="../Styles.css" type="text/css" rel="stylesheet">
	<%if Session("SOLA_LETTURA")="1" then%>
	<link href="../solalettura.css" type="text/css" rel="stylesheet">
	<%end If%>
	<script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
    <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
    <script language="javascript" type="text/javascript">
        function ExtractFiles() {
            GestAlert('c', 'question', 'btnExtract', '', 'L\'estrazione completa della banca dati potrebbe richiedere qualche minuto.\r\nSi vuole proseguire?');
        }
		function keyPress()
		{
			if(window.event.keyCode==13)
			{
			    ExtractFiles();
			}
		}
    </script>
</head>
<body>
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" align="right" style="width: 100%; height: 45px">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table2">
                <tr>
                    <td style="width: 464px; height: 20px" align="left">
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" width="800" colspan="2" rowspan="2">
                        <input class="Bottone BottonePopolaAppoggio" runat="server" id="Extract" title="Estrazione completa banca dati" onclick="ExtractFiles();" type="button" />
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 463px" align="left">
                        <span class="NormalBold_title" id="info" style="width: 400px; height: 20px">Estrazione completa banca dati</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="ExpAllDB">
            <hr />
            <div style="margin:10px;">
                <p>La copia completa dei dati include:</p>
                <ul>
                    <li><i class="fa fa-check-square-o"></i>  Anagrafe dei tributi</li>
                    <li><i class="fa fa-check-square-o"></i>  Dichiarazioni IMU/TARI/OSAP</li>
                    <li><i class="fa fa-check-square-o"></i>  Avvisi di pagamento TARI/OSAP</li>
                    <li><i class="fa fa-check-square-o"></i>  Versamenti IMU/TARI/OSAP</li>
                    <li><i class="fa fa-check-square-o"></i>  Provvedimenti IMU/TARI/OSAP</li>
                    <li><i class="fa fa-check-square-o"></i>  Pagamenti Provvedimenti</li>
                </ul>
                <br />
                <p>La copia non include le basi dati acquisite da fonti esterne, quali:</p>
                <ul>
                    <li><i class="fa fa-minus-square-o"></i>  Catasto</li>
                    <li><i class="fa fa-minus-square-o"></i>  Docfa</li>
                    <li><i class="fa fa-minus-square-o"></i>  Atti di compravendita</li>
                </ul>
                <br />
                <p>La copia dei dati sarà in formato Excel</p>
            </div>
            <hr />
            <div id="myExpProgress"></div>
            <div id="myRecap"></div>
        </div>
       <asp:Button ID="btnExtract" runat="server" CssClass="hidden" OnClick="CmdExtract_Click"></asp:Button>
    </form>
</body>
</html>
