<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestDE.aspx.vb" Inherits="Provvedimenti.GestDE" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>GestioneDataEntry</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%        End If%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
        function CheckDati() {
            if (document.getElementById('hdIdContribuente').value == '-1') {
                GestAlert('a', 'warning', '', '', 'E\' necessario selezionare il Contribuente!');
                return false;
            }
            if (document.getElementById('DdlTributo').value == '-1') {
                GestAlert('a', 'warning', '', '', 'E\' necessario inserire il Tributo!');
                Setfocus(document.getElementById('DdlTributo'));
                return false;
            }
            if (document.getElementById('TxtAnno').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario inserire l\'Anno!');
                Setfocus(document.getElementById('TxtAnno'));
                return false;
            }
            if (document.getElementById('TxtNumAtto').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario inserire il Numero dell\'Atto!');
                Setfocus(document.getElementById('TxtNumAtto'));
                return false;
            }
            if (document.getElementById('TxtDataAtto').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Data Atto!');
                Setfocus(document.getElementById('TxtDataAtto'));
                return false;
            }
            if (document.getElementById('TxtImpDiffImposta').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Importo Differenza d\'Imposta!');
                Setfocus(document.getElementById('TxtImpDiffImposta'));
                return false;
            }
            if (document.getElementById('TxtImpSanzioni').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Importo Sanzioni!');
                Setfocus(document.getElementById('TxtImpSanzioni'));
                return false;
            }
            if (document.getElementById('TxtImpInteressi').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Importo Interessi!');
                Setfocus(document.getElementById('TxtImpInteressi'));
                return false;
            }
            if (document.getElementById('TxtImpSpeseNot').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Importo Spese Notifica!');
                Setfocus(document.getElementById('TxtImpSpeseNot'));
                return false;
            }
            if (document.getElementById('TxtImpTotale').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Importo Totale!');
                Setfocus(document.getElementById('TxtImpTotale'));
                return false;
            }
            if (document.getElementById('TxtDataNotifica').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Data Notifica!');
                Setfocus(document.getElementById('TxtDataNotifica'));
                return false;
            }
            else {
                //controllo che la Notifica sia dal 2020 in poi
                var sAnno = new String
                sAnno = document.getElementById('TxtDataNotifica').value
                sAnno = sAnno.substring(6, 10)
                if (2020 > sAnno) {
                    GestAlert('a', 'warning', '', '', 'La Data di Notifica deve essere maggiore di 31/12/2019!');
                    Setfocus(document.getElementById('TxtDataNotifica'));
                    return false;
                }
            }
            //controllo che siano coerenti Notifica e Data Atto
            if (document.getElementById('TxtDataNotifica').value != '' && document.getElementById('TxtDataAtto').value != '') {
                var starttime = document.getElementById('TxtDataAtto').value
                var endtime = document.getElementById('TxtDataNotifica').value
                //Start date split to UK date format and add 31 days for maximum datediff
                starttime = starttime.split("/");
                starttime = new Date(starttime[2], starttime[1] - 1, starttime[0]);
                //End date split to UK date format 
                endtime = endtime.split("/");
                endtime = new Date(endtime[2], endtime[1] - 1, endtime[0]);
                if (endtime <= starttime) {
                    GestAlert('a', 'warning', '', '', 'La Data di Notifica e\' minore/uguale alla Data dell\'Atto!');
                    Setfocus(document.getElementById('TxtDataFine'));
                    return false;
                }
            }
            //il totale deve essere coerente
            impTot = parseFloat(document.getElementById('TxtImpTotale').value.replace(',', '.'));
            mySum = parseFloat(document.getElementById('TxtImpDiffImposta').value.replace(',', '.')) + parseFloat(document.getElementById('TxtImpSanzioni').value.replace(',', '.')) + parseFloat(document.getElementById('TxtImpInteressi').value.replace(',', '.')) + parseFloat(document.getElementById('TxtImpSpeseNot').value.replace(',', '.'));
            if (impTot != mySum) {
                GestAlert('a', 'warning', '', '', 'Totale non coerente con somma di Importi!');
                GestAlert('a', 'warning', '', '', impTot +' '+ mySum);
                Setfocus(document.getElementById('TxtImpTotale'));
                return false;
            }
            document.getElementById('CmdSave').click()
        }
        function DeleteDE() {
            if (confirm('Si desidera eliminare la Dichiarazione?')) {
                document.getElementById('CmdDelete').click()
            }
            return false;
        }
    </script>
</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
            <div id="divRicerca" runat="server">
				<div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden">
					<table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table1">
						<tr valign="top">
							<td align="left">
								<span runat="server" id="infoEnte" class="ContentHead_Title" style="width: 400px"></span>
							</td>
							<td align="right" rowspan="2">
								<input class="Bottone BottoneExcel" id="Stampa" title="Stampa" onclick="DivAttesa.style.display = ''; document.getElementById('CmdStampa').click();" type="button" name="Stampa">
								<input class="Bottone BottoneNewInsert" id="NewInsert" title="Inserimento nuova posizione" onclick="document.getElementById('CmdNewInsert').click();" type="button" name="NewInsert">
								<input class="Bottone BottoneRicerca" id="Search" title="Ricerca" onclick="DivAttesa.style.display = '';document.getElementById('CmdSearch').click();" type="button" name="Search">
							</td>
						</tr>
						<tr>
							<td align="left" colspan="2">
								<span runat="server" id="info" class="NormalBold_title" style="width: 400px">- Data Entry Coattivo</span>
							</td>
						</tr>
					</table>
				</div>
			    <div id="Ricerca" style="margin:10px auto;">
                    <fieldset class="FiledSetRicerca">
                        <legend class="Legend">Inserimento filtri di ricerca</legend>
                        <table id="TblParametri" cellspacing="1" cellpadding="1" width="100%" border="0">
                            <tr>
                                <td class="Input_Label">
                                    <asp:Label Runat="server" CssClass="Input_Label">Cod.Tributo</asp:Label><br />
                                    <asp:DropDownList Runat="server" ID="DdlRicTributo" CssClass="Input_Text" Width="100px"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label CssClass="Input_Label" runat="server" ID="LblProv">Anno</asp:Label><br />
                                    <asp:TextBox runat="server" ID="TxtRicAnno" CssClass="Input_Text_Right" Width="100px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" CssClass="Input_Label" ID="Label17">Data Atto</asp:Label><br />
                                    <asp:TextBox runat="server" ID="TxtRicDataAtto" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" MaxLength="10"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label CssClass="Input_Label" runat="server" ID="Label16">Numero Atto</asp:Label><br />
                                    <asp:TextBox runat="server" ID="TxtRicNumAtto" CssClass="Input_Text_Right" Width="200px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" CssClass="Input_Label" ID="Label18">Data Notifica</asp:Label><br />
                                    <asp:TextBox runat="server" ID="TxtRicDataNotifica" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="Label2" CssClass="Input_Label" runat="server">Cognome</asp:Label><br />
                                    <asp:TextBox ID="TxtRicCognome" runat="server" CssClass="Input_Text" Width="376px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" CssClass="Input_Label" runat="server">Nome</asp:Label><br />
                                    <asp:TextBox ID="TxtRicNome" runat="server" CssClass="Input_Text" Width="185px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" CssClass="Input_Label" runat="server">Cod.Fiscale/P.IVA</asp:Label><br />
                                    <asp:TextBox ID="TxtRicCFPIVA" runat="server" CssClass="Input_Text" Width="170px" MaxLength="16"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
			    </div>
			    <div id="Result" class="col-md-12">
				    <asp:label id="LblResult" Runat="server" CssClass="Legend">La ricerca non ha prodotto risultati.</asp:label>
                    <Grd:RibesGridView ID="GrdSearch" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="20"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
						    <asp:BoundField DataField="ANNO" HeaderText="Anno"></asp:BoundField>
                            <asp:TemplateField HeaderText="Nominativo">
							    <ItemStyle HorizontalAlign="Left"></ItemStyle>
							    <ItemTemplate>
								    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Cognome") + " " + DataBinder.Eval(Container, "DataItem.Nome") %>'>
								    </asp:Label>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:TemplateField HeaderText="Cod.Fiscale/P.IVA">
							    <ItemStyle Width="120px" HorizontalAlign="Left"></ItemStyle>
							    <ItemTemplate>
								    <asp:Label runat="server" Text='<%# FncGrd.FormattaCFPIVA(DataBinder.Eval(Container, "DataItem.CODICE_FISCALE"), DataBinder.Eval(Container, "DataItem.PARTITA_IVA")) %>'>
								    </asp:Label>
							    </ItemTemplate>
						    </asp:TemplateField>
                            <asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Descrizione"></asp:BoundField>
                            <asp:BoundField DataField="NUMERO_ATTO" HeaderText="N.Atto"></asp:BoundField>
                            <asp:BoundField DataField="DATA_ELABORAZIONE" HeaderText="Data Atto"><ItemStyle HorizontalAlign="Right"></ItemStyle></asp:BoundField>
                            <asp:BoundField DataField="DATA_NOTIFICA_AVVISO" HeaderText="Data Notifica"><ItemStyle HorizontalAlign="Right"></ItemStyle></asp:BoundField>
                            <asp:BoundField DataField="ImportoCoattivo" HeaderText="Imposta" DataFormatString="{0:N}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="IMPORTO_SANZIONI" HeaderText="Sanzioni" DataFormatString="{0:N}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="InteressiCoattivo" HeaderText="Interessi" DataFormatString="{0:N}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SpeseCoattivo" HeaderText="Spese" DataFormatString="{0:N}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="TotaleCoattivo" HeaderText="Totale" DataFormatString="{0:N}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
						    <asp:TemplateField HeaderText="Sel.">
							    <HeaderStyle horizontalalign="Center"></HeaderStyle>
							    <ItemStyle HorizontalAlign="Center"></ItemStyle>
							    <ItemTemplate>
                                    <asp:ImageButton runat="server" CssClass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("Id") %>' alt="" ToolTip="Elimina"></asp:ImageButton>
                                    <asp:HiddenField ID="hfIdUI" runat="server" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
			    </div>
            </div>
            <div id="divDE" runat="server" style="display:none;">
                <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden">
                    <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table2">
                        <tr valign="top">
                            <td align="left">
                                <span runat="server" id="infoEntePunt" class="ContentHead_Title" style="width: 400px"></span>
                            </td>
                            <td align="right" rowspan="2">
                                <input class="Bottone BottoneCancella" id="Delete" title="Elimina" onclick="DeleteDE();" type="button" name="Delete">
                                <input class="Bottone BottoneSalva" id="Salva" title="Salva" onclick="CheckDati();" type="button" name="Salva">
                            	<input class="Bottone BottoneAnnulla" id="Cancel" title="Chiudi" onclick="document.getElementById('hfIdCoattivo').value=0;document.getElementById('hdIdContribuente').value=0;divDE.style.display = 'none';divRicerca.style.display = '';" type="button" name="Cancel"> 
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <span runat="server" id="infoPunt" class="NormalBold_title" style="width: 400px">Gestione</span>
                            </td>
                        </tr>
                    </table>
                </div>
                &nbsp;
				<table class="col-md-12" cellSpacing="1" cellPadding="1" border="0">
 				    <tr id="TRPlainAnagPunt">
				        <td>
				            <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				            <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				        </td>
				    </tr>
                </table>
                <div id="divDati">
                    <table id="TblDati" cellspacing="1" cellpadding="1" width="100%" border="0">
                        <tr>
                            <td>
                                <asp:Label Runat="server" CssClass="Input_Label">Cod.Tributo</asp:Label><br />
                                <asp:DropDownList Runat="server" ID="DdlTributo" CssClass="Input_Text" Width="100px"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label1">Anno</asp:Label><br />
                                <asp:TextBox runat="server" ID="TxtAnno" CssClass="Input_Text_Right" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label runat="server" CssClass="Input_Label" ID="Label5">Data Atto</asp:Label><br />
                                <asp:TextBox runat="server" ID="TxtDataAtto" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" MaxLength="10"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label6">Numero Atto</asp:Label><br />
                                <asp:TextBox runat="server" ID="TxtNumAtto" CssClass="Input_Text_Right" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td CssClass="Input_Label">
                                <asp:RadioButton runat="server" ID="RbUfficio" CssClass="Input_Radio" Text="Avviso di Accertamento d'Ufficio" Checked="true" GroupName="TipoAccertamento"></asp:RadioButton>
                                <asp:RadioButton runat="server" ID="RbRettifica" CssClass="Input_Radio" Text="Avviso di Accertamento in Rettifica" Checked="False" GroupName="TipoAccertamento"></asp:RadioButton>
                            </td>
                            <td>
                                <asp:Label runat="server" CssClass="Input_Label" ID="Label7">Data Notifica</asp:Label><br />
                                <asp:TextBox runat="server" ID="TxtDataNotifica" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
							<td>
								<asp:label ID="Label8" CssClass="Input_Label" Runat="server">Differenza d'Imposta €</asp:label><br />
								<asp:textbox Runat="server" id="TxtImpDiffImposta" CssClass="Input_Text_Right OnlyNumber" width="90px"></asp:textbox>
							</td>
							<td>
								<asp:label ID="Label9" CssClass="Input_Label" Runat="server">Sanzioni €</asp:label><br />
								<asp:textbox Runat="server" id="TxtImpSanzioni" CssClass="Input_Text_Right OnlyNumber" width="90px"></asp:textbox>
							</td>
							<td>
								<asp:label ID="Label10" CssClass="Input_Label" Runat="server">Interessi €</asp:label><br />
								<asp:textbox Runat="server" id="TxtImpInteressi" CssClass="Input_Text_Right OnlyNumber" width="90px"></asp:textbox>
							</td>
							<td>
								<asp:label ID="Label11" CssClass="Input_Label" Runat="server">Spese di Notitifica €</asp:label><br />
								<asp:textbox Runat="server" id="TxtImpSpeseNot" CssClass="Input_Text_Right OnlyNumber" width="90px"></asp:textbox>
							</td>
							<td>
								<asp:label ID="Label12" CssClass="Input_Label" Runat="server">Totale €</asp:label><br />
								<asp:textbox Runat="server" id="TxtImpTotale" CssClass="Input_Text_Right OnlyNumber" width="90px"></asp:textbox>
							</td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="DivAttesa" runat="server" style="z-index: 103; position: absolute;display:none;">
                <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                <div class="BottoneClessidra">&nbsp;</div>
                <div class="Legend">Attendere Prego...<asp:Label ID="LblAvanzamento" runat="server"></asp:Label></div>
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
			<asp:Button ID="CmdInsert" runat="server" Style="display: none" />
            <asp:Button ID="CmdDelete" runat="server" Style="display: none" />
            <asp:Button ID="CmdSearch" runat="server" Style="display:none" />
            <asp:Button ID="CmdStampa" Runat="server" style="DISPLAY: none" />
			<asp:Button ID="CmdNewInsert" runat="server" Style="display:none" />
            <asp:Button ID="CmdSave" Runat="server" style="DISPLAY: none" />
			<asp:HiddenField ID="hfIdCoattivo" runat="server" value="0"/>
		</form>
	</body>
</html>
