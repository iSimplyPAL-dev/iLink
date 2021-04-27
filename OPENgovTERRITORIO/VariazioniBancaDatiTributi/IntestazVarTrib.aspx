<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IntestazVarTrib.aspx.vb" Inherits="OpenGovTerritorio.IntestazVarTrib" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" type="text/javascript">
        function InsertVar() {
            document.getElementById('DivAttesa').style.display = '';
            document.getElementById('CmdInsertVarTrib').click();
        }
    </script>
</head>
<body>
    <form id="Form1" runat="server" method="post">
        <div id="VariazioneTributaria" runat="server">
	        <table id="TblRicerca" border="0" cellspacing="1" cellpadding="1" width="100%">
		        <tr>
			        <td>
				        <!--Variazione Tributaria-->
				        <fieldset class="FiledSetRicerca"><legend class="Legend">Variazione Tributaria</legend>
					        <asp:panel id="TblParametri" Runat="server" border="0" cellspacing="1" cellpadding="1" width="100%">
						        <table width="100%">
							        <tr>
								        <td colspan="3">
									        <asp:Label id="Label40" CssClass="Input_Label" Runat="server">Causale:</asp:Label>&nbsp;
									        <asp:label id="LblCausale" runat="server" CssClass="Input_Label" Width="300px"></asp:label>&nbsp;
					                        <asp:Label ID="LblTrattato" runat="server" class="Input_Label"></asp:Label>
								        </td>
                                        <td align="right">
                                            <input id="InsertVarTrib" runat="server" class="BottoneRibalta" ToolTip="Inserisci Variazione tributaria" onclick="InsertVar();"/>
                                            <asp:Button ID="CmdInsertVarTrib" runat="server" style="display:none" />
                                        </td>
							        </tr>
							        <tr>
								        <td>
									        <asp:Label Runat="server" CssClass="Input_Label" id="Label41">Data Variazione:</asp:Label>&nbsp;
									        <asp:label id="LblDataVariazione" Runat="server" CssClass="Input_Label" Width="80px"></asp:label>
								        </td>
								        <td colspan="3">
									        <asp:Label Runat="server" CssClass="Input_Label" id="Label42">Operatore:</asp:Label>&nbsp;
									        <asp:Label id="LblOperatore" Runat="server" CssClass="Input_Label"></asp:Label>
								        </td>
							        </tr>
				                    <tr>
					                    <td>
									        <asp:Label id="Label36" CssClass="Input_Label" Runat="server">Tributo:</asp:Label>&nbsp;
						                    <asp:Label id="LblTributo" runat="server" CssClass="Input_Label"></asp:Label>
					                    </td>
								        <td>
									        <asp:Label id="Label37" CssClass="Input_Label" Runat="server">Foglio:</asp:Label>&nbsp;
									        <asp:label id="LblFg" runat="server" CssClass="Input_Label" Width="100px"></asp:label>
								        </td>
								        <td>
									        <asp:Label id="Label38" CssClass="Input_Label" Runat="server">Numero:</asp:Label>&nbsp;
									        <asp:label id="LblNum" runat="server" CssClass="Input_Label" Width="100px"></asp:label>
								        </td>
								        <td>
									        <asp:Label id="Label39" CssClass="Input_Label" Runat="server">Subalterno:</asp:Label>&nbsp;
									        <asp:label id="LblSub" runat="server" CssClass="Input_Label" Width="100px"></asp:label>
								        </td>
							        </tr>
				                    <tr>
					                    <td colspan="4">
									        <asp:Label Runat="server" CssClass="Input_Label" id="Label35">Dichiarazione:</asp:Label>&nbsp;
									        <asp:Label id="LblInfoDich" Runat="server" CssClass="Input_Label"></asp:Label>
					                    </td>
				                    </tr>
						        </table>
					        </asp:panel>
				        </fieldset>
			        </td>
		        </tr>
		        <tr>
			        <td>
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
			        </td>
		        </tr>
	        </table>
        </div>
        <div id="divDialogBox" class="col-md-12">
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
            </div>
            <input type="hidden" id="cmdHeight" value="0" />
        </div>
    </form>
</body>
</html>
