<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GestionePesature.aspx.vb" Inherits="OPENgovTIA.GestionePesature"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>GestionePesature</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%        End If%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" src="../../_js/ControlloData.js?newversion"></script>
		<script type="text/javascript">
			function ClearDatiPesature()
			{ 
				/*parent.parent.Comandi.location.href='ComandiRicPesature.aspx'; 
				parent.parent.Visualizza.location.href='RicercaPesature.aspx';				
				return true;*/
			    document.getElementById('CmdClearDati').click()
			}
			function CheckNewConf() {
			    if (document.getElementById('txtKG').value == '' || document.getElementById('txtKG').value == '0') {
			        GestAlert('a', 'warning', '', '', 'Inserire N.Conferimenti!');
			        return false;
			    }
			    if (document.getElementById('txtVolume').value == ''||document.getElementById('txtVolume').value == '0') {
			        GestAlert('a', 'warning', '', '', 'Inserire Volume/KG!');
			        return false;
			    }
			    if (document.getElementById('ddlTipoConferimento').value == '-1') {
			        GestAlert('a', 'warning', '', '', 'Selezionare il Tipo di Conferimento!');
			        return false;
			    }
			    document.getElementById('CmdSaveConferimento').click();
			}
		</script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table1">
                <tr valign="top">
                    <td align="left">
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" rowspan="2">
                        <input class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla ricerca." onclick="ClearDatiPesature()" type="button" name="Cancel">
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 400px">- Conferimenti - Gestione</span>
                    </td>
                </tr>
            </table>
        </div>
        &nbsp;
        <div class="col-md-12">
			<table width="100%">
				<!--blocco dati contribuente--><%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
				<tr id="TRPlainAnag">
				    <td colspan="5">
				        <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				        <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				    </td>
				</tr>
				<tr id="TRSpecAnag">
					<td width="100%" colspan="5">
						<table id="TblContribuente" cellspacing="0" cellpadding="0" border="0" width="100%">
				            <tr>
					            <td width="100%">
						            <asp:Label CssClass="lstTabRow" Runat="server" id="Label45" width="100%">Dati Contribuente</asp:Label>
					            </td>
				            </tr>
							<!--prima riga-->
							<tr>
								<td width="280">
									<asp:label id="Label23" CssClass="Input_Label" Runat="server">Cod.Fiscale</asp:label>
									<br />
									<asp:textbox id="TxtCodFiscale" CssClass="Input_Text" Runat="server" Width="185px" ReadOnly="True"></asp:textbox>
								</td>
								<td colspan="3" width="275">
									<asp:label id="Label24" CssClass="Input_Label" Runat="server">Partita Iva</asp:label>
									<br />
									<asp:textbox id="TxtPIva" CssClass="Input_Text" Runat="server" Width="140px" ReadOnly="True"></asp:textbox>
								</td>
								<td>
									<asp:Label CssClass="Input_Label" Runat="server" id="Label40">Sesso</asp:Label>
									<br />
									<asp:RadioButton ID="F" CssClass="Input_Label" Runat="server" Text="F" GroupName="Sesso" Enabled="False"></asp:RadioButton>
									<asp:RadioButton ID="M" CssClass="Input_Label" Runat="server" Text="M" GroupName="Sesso" Enabled="False"></asp:RadioButton>
									<asp:RadioButton ID="G" CssClass="Input_Label" Runat="server" Text="G" GroupName="Sesso" Enabled="False"></asp:RadioButton>
								</td>
								<td>
									<asp:textbox id="TxtIdDataAnagrafica" Runat="server" Visible="False" Width="10px">-1</asp:textbox>
									<asp:button id="btnRibalta" style="DISPLAY: none" Runat="server"></asp:button>
									<asp:button id="btnRibaltaAnagAnater" style="DISPLAY: none" Runat="server"></asp:button>
								</td>
							</tr>
							<!--seconda riga-->
							<tr>
								<td width="280">
									<asp:label id="Label41" CssClass="Input_Label" Runat="server">Cognome/Rag.Soc</asp:label>
									<br />
									<asp:textbox id="TxtCognome" CssClass="Input_Text" Runat="server" Width="265px" ReadOnly="True"></asp:textbox>
								</td>
								<td colspan="3" width="275">
									<asp:label id="Label42" CssClass="Input_Label" Runat="server">Nome</asp:label>
									<br />
									<asp:textbox id="TxtNome" CssClass="Input_Text" Runat="server" Width="230px" ReadOnly="True"></asp:textbox>
								</td>
							</tr>
							<!--terza riga-->
							<tr>
								<td width="280">
									<asp:label id="Label43" CssClass="Input_Label" Runat="server">Data Nascita</asp:label>
									<br />
									<asp:textbox id="TxtDataNascita" Runat="server" CssClass="Input_Text_Right TextDate" ReadOnly="True"></asp:textbox>
								</td>
								<td colspan="3" width="275">
									<asp:label id="Label44" CssClass="Input_Label" Runat="server">Luogo Nascita</asp:label>
									<br />
									<asp:textbox id="TxtLuogoNascita" CssClass="Input_Text" Runat="server" Width="250px" ReadOnly="True"></asp:textbox>
								</td>
							</tr>
							<!--quarta riga-->
							<tr>
								<td width="280">
									<asp:label id="Label46" CssClass="Input_Label" Runat="server">Via</asp:label>
									<br />
									<asp:textbox id="TxtResVia" CssClass="Input_Text" Runat="server" Width="265px" ReadOnly="True"></asp:textbox>
								</td>
								<td>
									<asp:label id="Label47" CssClass="Input_Label" Runat="server">Civico</asp:label>
									<br />
									<asp:textbox id="TxtResCivico" CssClass="Input_Text_Right" Runat="server" Width="50px" ReadOnly="True"></asp:textbox>
								</td>
								<td>
									<asp:label id="Label48" CssClass="Input_Label" Runat="server">Esponente</asp:label>
									<br />
									<asp:textbox id="TxtResEsponente" CssClass="Input_Text" Runat="server" Width="50px" ReadOnly="True"></asp:textbox>
								</td>
								<td Width="70">
									<asp:label id="Label49" CssClass="Input_Label" Runat="server">Interno</asp:label>
									<br />
									<asp:textbox id="TxtResInterno" CssClass="Input_Text" Runat="server" Width="50px" ReadOnly="True"></asp:textbox>
								</td>
								<td>
									<asp:label id="Label50" CssClass="Input_Label" Runat="server">Scala</asp:label>
									<br />
									<asp:textbox id="TxtResScala" CssClass="Input_Text" Runat="server" Width="50px" ReadOnly="True"></asp:textbox>
								</td>
							</tr>
							<!--quinta riga-->
							<tr>
								<td width="280">
									<asp:label id="Label51" CssClass="Input_Label" Runat="server">CAP</asp:label>
									<br />
									<asp:textbox id="TxtResCAP" CssClass="Input_Text_Right" Runat="server" Width="80px" ReadOnly="True"></asp:textbox>
								</td>
								<td colspan="3" width="275">
									<asp:label id="Label52" Runat="server" CssClass="Input_Label">Comune</asp:label>
									<br />
									<asp:textbox id="TxtResComune" CssClass="Input_Text" Runat="server" Width="250px" ReadOnly="True"></asp:textbox>
								</td>
								<td>
									<asp:label id="Label53" CssClass="Input_Label" Runat="server">Provincia</asp:label>
									<br />
									<asp:textbox id="TxtResPv" CssClass="Input_Text" Runat="server" Width="50px" ReadOnly="True"></asp:textbox>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<!--Blocco Dati Tessera-->
				<tr>
					<td colspan="5">
						<asp:Label CssClass="lstTabRow" Runat="server" id="Label1" width="100%">Dati Tessera</asp:Label>
					</td>
				</tr>
				<tr>
					<td colspan="5">
						<table cellspacing="0" cellpadding="0" border="0" width="100%">
							<tr>
								<!--*** 201511 - gestione tipo tessera ***-->
                                <td>
                                    <asp:Label runat="server" CssClass="Input_Label">Tipo Tessera</asp:Label><br />
                                    <asp:DropDownList ID="DdlTipoTessera" runat="server" CssClass="Input_Text"></asp:DropDownList>
                                </td>
								<td>
									<asp:label id="Label2" Runat="server" CssClass="Input_Label">Cod.Utente</asp:label>
									<br />
									<asp:textbox id="TxtCodUtente" Runat="server" CssClass="Input_Text" ReadOnly="True" Width="100px"></asp:textbox>
								</td>
								<td>
									<asp:label id="Label4" Runat="server" CssClass="Input_Label">N.Tessera</asp:label>
									<br />
									<asp:textbox id="TxtNTessera" Runat="server" CssClass="Input_Text" ReadOnly="True" Width="100px"></asp:textbox>
								</td>
								<td>
									<asp:label id="Label5" Runat="server" CssClass="Input_Label">Cod.Interno</asp:label>
									<br />
									<asp:textbox id="TxtCodice" Runat="server" CssClass="Input_Text" style="TEXT-ALIGN: right" Width="100" ReadOnly="True"></asp:textbox>
								</td>
								<td>
									<asp:label id="Label8" Runat="server" CssClass="Input_Label">Data Rilascio</asp:label>
									<br />
									<asp:textbox id="TxtDataRilascio" Runat="server" CssClass="Input_Text_Right TextDate" ReadOnly="True"></asp:textbox>
								</td>
								<td>
									<asp:label id="Label22" Runat="server" CssClass="Input_Label">Data Cessazione</asp:label>
									<br />
									<asp:textbox id="TxtDataCessazione" Runat="server" CssClass="Input_Text_Right TextDate" ReadOnly="True"></asp:textbox>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colSpan="5">
						<asp:label id="Label12" Runat="server" CssClass="Input_Label">Note</asp:label>
						<br />
						<asp:textbox id="TxtNote" Runat="server" CssClass="Input_Text" TextMode="MultiLine" width="700px"
							Height="32px" ReadOnly="True"></asp:textbox>
					</td>
				</tr>
				<!--Blocco Dati Pesature-->
				<tr>
					<td>
						<asp:Label CssClass="lstTabRow" Runat="server">Dati Conferimenti</asp:Label>
						<asp:imagebutton id="LnkNewConferimento" runat="server" ToolTip="Nuovo Conferimento" CausesValidation="False" imagealign="Bottom" ImageUrl="../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px"></asp:imagebutton>&nbsp;
                        <asp:Label CssClass="lstTabRow" Runat="server" id="Label17" Width="605px">&nbsp;</asp:Label>
					</td>
				</tr>
				<tr>
					<td width="100%">
						<div id="DivPesature" runat="server">
							<table id="TblPesature" cellspacing="0" cellpadding="0" border="0" width="100%">
								<tr>
									<td colspan="4">
										<asp:Label CssClass="Legend" Runat="server" ID="LblResult">Non sono presenti Conferimenti</asp:Label>
										<Grd:RibesGridView ID="GrdPesature" runat="server" BorderStyle="None" 
                                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                            OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                                            <PagerSettings Position="Bottom"></PagerSettings>
                                            <PagerStyle CssClass="CartListFooter" />
                                            <RowStyle CssClass="CartListItem"></RowStyle>
                                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
											<Columns>
												<asp:BoundField DataField="tDataOraConferimento" HeaderText="Data">
													<ItemStyle HorizontalAlign="Left"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="sTipoConferimento" HeaderText="Tipo Conferimento">
													<ItemStyle HorizontalAlign="Left"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="nLitri" HeaderText="N.Conferimenti" DataFormatString="{0:0.00}">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="nVolume" HeaderText="Volume/KG" DataFormatString="{0:0.00}">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundField>
												<asp:TemplateField HeaderText="">
									                <headerstyle horizontalalign="Center"></headerstyle>
									                <itemstyle horizontalalign="Center"></itemstyle>
									                <itemtemplate>
										                <asp:ImageButton id="imgDel" runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" CommandArgument='<%# Eval("ID") %>' alt="" OnClientClick="return confirm('Si vuole eliminare il conferimento?')"></asp:ImageButton>
									                </itemtemplate>
								                </asp:TemplateField>
												<asp:TemplateField HeaderText="">
	                                                <headerstyle horizontalalign="Center"></headerstyle>
	                                                <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
	                                                <itemtemplate>
	                                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
	                                                    <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                                                        <asp:HiddenField runat="server" ID="hfIDPESATURA" Value='<%# Eval("IDPESATURA") %>' />
                                                        <asp:HiddenField runat="server" ID="hfIDFLUSSO" Value='<%# Eval("IDFLUSSO") %>' />
	                                                </itemtemplate>
                                                </asp:TemplateField>
											</Columns>
											</Grd:RibesGridView>
										<br />
									</td>
								</tr>
								<tr>
									<td>
										<asp:Label ID="LblTotConferimenti" Runat="server" CssClass="Legend"></asp:Label>
									</td>
									<td style="display:none">
										<asp:Label ID="LblTotKG" Runat="server" CssClass="Legend"></asp:Label>
									</td>
									<td>
										<asp:Label ID="LblTotVolume" Runat="server" CssClass="Legend"></asp:Label>
									</td>
									<td>
										<asp:Label ID="LblTotMq" Runat="server" CssClass="Legend">Non sono presenti MQ attivi in Dichiarazioni TARSU</asp:Label>
									</td>
								</tr>
							</table>
						</div>
					</td>
				</tr>
                <tr>
                    <td style="width:100%">
                        <div id="GestConferimento">
                            <table width="100%" class="FiledSetRicerca">
                                <tr>
                                    <td><asp:Label runat="server" CssClass="Input_Label">Data</asp:Label></td>
                                    <td><asp:Label runat="server" CssClass="Input_Label">Ora</asp:Label></td>
                                    <td><asp:Label runat="server" CssClass="Input_Label">Tipo Conferimento</asp:Label></td>
                                    <td><asp:Label runat="server" CssClass="Input_Label">N.Conferimenti</asp:Label></td>
                                    <td><asp:Label runat="server" CssClass="Input_Label">Volume/KG</asp:Label></td>
                                    <td rowspan="2">
                                        <div class="tooltip">
                                            <img style="CURSOR: pointer" onclick="CheckNewConf();" alt="" align="bottom" class="Bottone BottoneSalva" />
                                            <span class="tooltiptext">Salva</span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtData" runat="server" CssClass="Input_Text_Right TextDate" maxlength="10" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOra" runat="server" CssClass="Input_Text_Right" Width="100px" maxlength="8"></asp:TextBox>
                                    </td>
                                    <!--*** 201712 - gestione tipo conferimento ***-->
                                    <td>
                                        <asp:DropDownList ID="ddlTipoConferimento" runat="server" CssClass="Input_Text"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtKG" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value, 0, 0)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo!')}"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVolume" runat="server" CssClass="Input_Text_Right" onblur="if (!isNumber(this.value, 0, 2)){GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI!')}"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
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
            <asp:button id="CmdSaveConferimento" style="DISPLAY: none" runat="server"></asp:button>
            <asp:button id="CmdClearDati" style="DISPLAY: none" Runat="server"></asp:button>
			<asp:HiddenField id="hdIdConferimento" runat="server" Value="-1" />
            <asp:HiddenField id="hdIdPesatura" runat="server" Value="-1" />
            <asp:HiddenField id="hdIdFlusso" runat="server" Value="-1" />
            <asp:HiddenField id="hdIdTessera" runat="server" Value="-1" />
		</form>
	</body>
</HTML>
