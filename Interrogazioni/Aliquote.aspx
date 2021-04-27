<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Aliquote.aspx.vb" Inherits="OPENgov.Aliquote" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
</head>
<body leftMargin="3" topMargin="3">
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
                        <input class="Bottone BottoneExcel" runat="server" id="Excel" title="Stampa elenco Anagrafiche in formato Excel" onclick="document.getElementById('DivAttesa').style.display='';document.getElementById('btnStampaExcel').click();" type="button" name="Excel" />
                        <input class="Bottone BottoneRicerca" runat="server" id="Ricerca" title="Ricerca" onclick="document.getElementById('DivAttesa').style.display = ''; document.getElementById('btnRicerca').click();" type="button" name="Search" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 463px" align="left">
                        <span class="NormalBold_title" id="info" style="width: 400px; height: 20px">Aliquote</span>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td>
                        <fieldset class="classeFiledSetRicerca">
                            <legend class="Legend">Inserimento filtri di Ricerca</legend>
                            <table cellspacing="1" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td class="Input_Label">
                                        Tributo<br />
                                        <asp:DropDownList ID="ddlTributo" runat="server" CssClass="Input_Label" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                    <td class="Input_Label">
                                        Anno<br />
                                        <asp:TextBox ID="txtAnno" runat="server" MaxLength="4" CssClass="Input_Text_Right OnlyNumber" Width="60px"></asp:TextBox>
                                    </td>
                                    <td class="Input_Label" id="optTARSU">
                                        <asp:RadioButton ID="optTARSUTariffe" runat="server" CssClass="Input_Radio" GroupName="optTARSU" Text="Tariffe" />&nbsp;
                                        <asp:RadioButton ID="optTARSURiduzioni" runat="server" CssClass="Input_Radio" GroupName="optTARSU" Text="Riduzioni" />&nbsp;
                                        <asp:RadioButton ID="optTARSUEsenzioni" runat="server" CssClass="Input_Radio" GroupName="optTARSU" Text="Esenzioni" />&nbsp;
                                    </td>
                                    <td class="Input_Label" id="optICI">
                                        <asp:RadioButton ID="optICIIMU" runat="server" CssClass="Input_Radio" GroupName="optICI" Text="ICI/IMU" />&nbsp;
                                        <asp:RadioButton ID="optICITASI" runat="server" CssClass="Input_Radio" GroupName="optICI" Text="TASI" />&nbsp;
                                    </td>
                                    <td class="Input_Label" id="optH2O">
                                        <asp:RadioButton ID="optH2OAddizionali" runat="server" CssClass="Input_Radio" GroupName="optH2O" Text="Addizionali" />&nbsp;
                                        <asp:RadioButton ID="optH2OCanoni" runat="server" CssClass="Input_Radio" GroupName="optH2O" Text="Canoni" />&nbsp;
                                        <asp:RadioButton ID="optH2OScaglioni" runat="server" CssClass="Input_Radio" GroupName="optH2O" Text="Scaglioni" />&nbsp;
									    <asp:RadioButton ID="optH2ONolo" runat="server" CssClass="Input_Radio" GroupName="optH2O" Text="Nolo" />&nbsp;
									    <asp:RadioButton ID="optH2OQuotaFissa" runat="server" CssClass="Input_Radio" GroupName="optH2O" Text="Quota Fissa" />&nbsp;
                                    </td>
                                </tr>
                            </table>
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
                <tr>
                    <td>
 					    <asp:label id="LblResult" CssClass="Legend" Runat="server"></asp:label>
                        <Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None" 
						    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
						    AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
						    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
						    <PagerSettings Position="Bottom"></PagerSettings>
						    <PagerStyle CssClass="CartListFooter" />
						    <RowStyle CssClass="CartListItem"></RowStyle>
						    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
						    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                            </Columns>
                        </Grd:RibesGridView>
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
        <asp:Button ID="btnStampaExcel" Style="display: none" runat="server" Text="Button"></asp:Button>
        <asp:Button ID="btnRicerca" Style="display: none" runat="server" Text="Button"></asp:Button>
    </form>
</body>
</html>
