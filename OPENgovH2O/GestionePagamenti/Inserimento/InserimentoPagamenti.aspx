<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InserimentoPagamenti.aspx.vb" Inherits="OpenUtenze.InserimentoPagamenti" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>InserimentoPagamenti</title>
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
    <script type="text/javascript" language="Javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
        function ControllaDati() {
            if (document.getElementById('txtImportoPagamento').value == '') {
                alert("E' necessario valorizzare il campo Importo!");
                return false;
            }

            if (document.getElementById('txtDataPagamento').value == '') {
                alert("E' necessario valorizzare il campo Data Pagamento!");
                return false;
            }
            else {
                if (!isDate(document.getElementById('txtDataPagamento').value)) {
                    alert("Inserire la Data di Pagamento correttamente in formato: GG/MM/AAAA!");
                    Setfocus(document.getElementById('txtDataScadenza'));
                    return false;
                }
                else {
                    //la data di pagamento deve essere posteriore alla data di fattura
                    var starttime = document.getElementById('txtDataFattura').value;
                    var endtime = document.getElementById('txtDataPagamento').value;
                    //Start date split to UK date format and add 31 days for maximum datediff
                    starttime = starttime.split("/");
                    starttime = new Date(starttime[2], starttime[1] - 1, starttime[0]);
                    //End date split to UK date format 
                    endtime = endtime.split("/");
                    endtime = new Date(endtime[2], endtime[1] - 1, endtime[0]);
                    if (endtime <= starttime) {
                        alert("La Data di Pagamento e\' minore/uguale alla Data di Fattura!");
                        Setfocus(document.getElementById('txtDataPagamento'));
                        return false;
                    }
                }
            }

            if (document.getElementById('txtDataAccredito').value == '') {
                document.getElementById('txtDataAccredito').value = document.getElementById('txtDataPagamento').value;
            }

            if (!isDate(document.getElementById('txtDataAccredito').value)) {
                alert("Inserire la Data di Riversamento correttamente in formato: GG/MM/AAAA!");
                Setfocus(document.getElementById('txtDataAccredito'));
                return false;
            }

            document.getElementById('BtnSalva').click();
            //document.getElementById('getElementById('BtnSalva').click();
        }
        function ConfermaUscita() {
            if (confirm('Si vuole uscire dalla videata di Inserimento?')) {
                parent.parent.Comandi.location.href = "./../CRicercaPagamenti.aspx";
                parent.parent.Visualizza.location.href = './../RicercaPagamenti.aspx?EffettuaRicerca=si'
            }
        }

        function ControllaAnno(oggetto) {
            if (!IsBlank(oggetto.value)) {
                if (!isNumber(oggetto.value, 4, 0, 1950, 2090)) {
                    alert("Inserire un anno di quattro cifre\ncompreso fra 1950 e 2090")
                    oggetto.value = ""
                    oggetto.focus()
                    return false
                }
            }
        }

        function UscitaDopoOperazione() {
            parent.frames.item('loadInsert').location.href = "../../../aspVuota.aspx";
        }

        function ConfermaCancellazione() {
            if (confirm('Si vuole eliminare il Pagamento Selezionato?')) {
                document.getElementById('BtnElimina').click()
            }
        }

        function IsNumeric(sText) {
            var ValidChars = "0123456789,";
            var IsNumber = true;
            var Char;


            for (i = 0; i < sText.length && IsNumber == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    IsNumber = false;
                }
            }
            return IsNumber;

        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <table style="z-index: 101; left: 0px; position: absolute; top: 8px; height: 137px" cellpadding="0"
            width="100%" border="0">
            <tr>
                <td style="width: 511px; height: 16px" colspan="3">
                    <asp:Label ID="lblDescrizioneOperazione" CssClass="Legend" runat="server" Width="100%">Dati Entry</asp:Label>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 127px">
                    <asp:Label runat="server" CssClass="Input_Label" ID="Label3">Numero Fattura</asp:Label>
                    <br>
                    <asp:TextBox ID="txtNfattura" runat="server" Width="104px"  CssClass="Input_Text" MaxLength="20"></asp:TextBox>
                </td>
                <td style="width: 174px">
                    <asp:Label runat="server" CssClass="Input_Label" ID="Label1">Data Fattura</asp:Label>
                    <br>
                    <asp:TextBox ID="txtDataFattura" runat="server" Width="104px" CssClass="Input_Text" MaxLength="10" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 127px; height: 30px" colspan="1">
                    <asp:Label runat="server" CssClass="Input_Label" ID="Label2">Importo Pagato </asp:Label>
                    <br>
                    <asp:TextBox onkeypress="return NumbersOnly(event, true, true, 2);" ID="txtImportoPagamento" Width="100px" runat="server" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox>
                </td>
                <td style="width: 174px; height: 30px" colspan="1">
                    <asp:Label runat="server" CssClass="Input_Label" ID="Label4">Data Pagamento</asp:Label>
                    <br>
                    <asp:TextBox ID="txtDataPagamento" runat="server" Width="104px" MaxLength="10" CssClass="Input_Text" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                </td>
                <td style="height: 30px" colspan="1">
                    <asp:Label runat="server" CssClass="Input_Label" ID="lblDataAccredito">Data Riversamento</asp:Label>
                    <br>
                    <asp:TextBox ID="txtDataAccredito" runat="server" Width="104px" MaxLength="10" CssClass="Input_Text" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:Button ID="BtnSalva" Style="display: none" runat="server" Width="32px" Height="32px" Text="Salva"></asp:Button>
        <asp:Button ID="BtnElimina" Style="display: none" runat="server" Width="32px" Text="Elimina" Height="32px"></asp:Button>
    </form>
</body>
    
</html>
