<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="aspDialogBoxe.aspx.vb" Inherits="OPENgov.aspDialogBoxe" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../../Styles.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
    <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
</head>
<body>
    <div class="modal-box">
        <div id="divAlert" class="modal-alert">
            <span class="closebtnalert" onclick="CloseAlert()">&times;</span>
            <p id="pAlert">testo di esempio</p>
            <input type="text" class="prompttxt"/>
            <button id="btnDialogBoxeOK" class="confirmbtn Bottone BottoneOK" onclick="DialogConfirmOK();"></button>&nbsp;
            <button id="btnDialogBoxeKO" class="confirmbtn Bottone BottoneKO" onclick="DialogConfirmKO();"></button>
            <button id="CmdDialogConfirmOK" class="hidden" onclick="RaiseDialogConfirmOK();"></button>
            <button id="CmdDialogConfirmKO" class="hidden" onclick="RaiseDialogConfirmKO();"></button>
            <input type="hidden" id="hfCloseAlert" />
            <input type="hidden" id="hfDialogOK" />
            <input type="hidden" id="hfDialogKO" />
        </div>
        <div id="divLoadWindow" class="hidden">
            <iframe id="ifrLoadWindow" runat="server" src="../../aspVuotaRemoveComandi.aspx" style="width:980px;height:680px;"></iframe>
        </div>
    </div>
    <input type="hidden" id="cmdHeight" value="0" />
</body>
</html>
