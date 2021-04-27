<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ModLettureHome.aspx.vb" Inherits="OpenUtenze.ModLettureHome" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>Gestione Letture</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<META content="True" name="vs_showGrid">
		<META content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<META content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<META content="JavaScript" name="vs_defaultClientScript">
		<META content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/MyUtility.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript">
		function EliminaLettura()
		{
			if (confirm('Si conferma la cancellazione della lettura?'))
			{
				document.getElementById('btnElimina').click();
			}
		}
		
		 function MessageNotFound()
		{
			GestAlert('a', 'warning', '', '', 'La ricerca non ha prodotto risultati !!!');
			return false;
		}
		
		 function GetStradario(objFieldHidden,objFieldUbicazione,CodComune,objForm) 
		{
			HIDDEN=objFieldHidden.name;
			FORM=objForm.name;
			UBICAZIONE=objFieldUbicazione.name;
			if(document.getElementById('hdSceltaViaLetture').value=='1')
			{
				WinPopUp=OpenPopup('OpenUtenze','<%=Session("PATH_APPLICAZIONE")%>/Selezioni/FrameListaSelezioni.aspx?SEARCHVIA=' + UBICAZIONE +'&HIDDEN=' + HIDDEN + '&FORM=' +FORM +'&UBICAZIONE='+ UBICAZIONE +'&CODCOMUNE='+CodComune,'Stradario','770','500',0,0,'yes','no');
			}
			else
			{
				GestAlert('a', 'warning', '', '', 'Funzione non abilitata per il DE Contatori !!');
			}
		}

		function VerificaData(Data)
		{
			if (!IsBlank(Data.value ))
			{		
				if(!isDate(Data.value)) 
				{
				    alert("Inserire la Data  correttamente in formato: gg/mm/aaaa !");
				    Data.value = "";
					Setfocus(Data);
					return false;
				}
			}					
		}	
		
		function AttivaAnomalie(oSel) 
		{
			oSel.disabled=false;

			d=new Date();

			s= d.getDate()+"/"+(d.getMonth()+1)+"/"+d.getYear();
			document.getElementById('chkDARicontrollare').checked=false;
			document.getElementById('chkLasciatoAvviso').checked=false;
			document.getElementById('txtDataPassaggio').value=s;
			document.getElementById('chkLasciatoAvviso').disabled=true;
			document.getElementById('chkDARicontrollare').disabled=true;
			document.getElementById('txtIDAnomalia').value='3';
		}

		function PulisciLista(oSel) 
		{
			var opt, i = 0;
			while (opt = oSel.options[i++]) 
			{
				opt.selected = false;
			}
			oSel.disabled=true;
			document.getElementById('txtDataPassaggio').value='';
			document.getElementById('chkLasciatoAvviso').disabled=false;
			document.getElementById('chkDARicontrollare').disabled=false;
			document.getElementById('txtIDAnomalia').value='';
		}

		function limitOptions(oSel, howmany) 
		{ 
			var opt, i = 0, msg = '',  thismany = howmany, toomany = new Array();
			while (opt = oSel.options[i++]) 
			{
				if (opt.selected) --howmany; 
				if (howmany < 0) toomany[toomany.length] = opt; 
			} 
			if(howmany < 0) 
			{ 
				msg += 'Numero massimo di Anomalie selezionabili  = ' + thismany + '.'; 
				alert(msg); 
				i = 0; 
				while (opt = toomany[i++]) 
				opt.selected = false; 
				return false; 
			} 
			else
			{
				document.getElementById('txtIDAnomalia').value=howmany
			}
		}

        function DisabilitaAbilitaCampi()
        {
        if(document.getElementById('chkLasciatoAvviso').checked == true)   
        {
			document.getElementById('chkFatturazione').disabled=true;
			document.getElementById('chkFatturazioneSospesa').disabled=true;
			document.getElementById('chkIncongruenteForzato').disabled=true;
			document.getElementById('chkGiroContatore').disabled=true;
			
			document.getElementById('chkGiroContatore').checked=false;
			document.getElementById('chkIncongruenteForzato').checked=false;
			document.getElementById('chkFatturazioneSospesa').checked=false;
			document.getElementById('chkFatturazione').checked=false;
			
			document.getElementById('cboModalitaLettura').value='-1';
			document.getElementById('cboStatoLetturaPage').value='-1';
			
			document.getElementById('txtDatadiLettura').value='';
			document.getElementById('txtLetturaAttuale').value='';
			document.getElementById('txtConsEffettivo').value='';
			document.getElementById('txtGGConsumo').value='';
			document.getElementById('txtLetturaTeorica').value='';
			document.getElementById('txtConsumoTeorico').value='';
			
			document.getElementById('txtDatadiLettura').disabled=true;
			document.getElementById('txtLetturaAttuale').disabled=true;
			document.getElementById('txtConsEffettivo').disabled=true;
			document.getElementById('cboModalitaLettura').disabled=true;
			document.getElementById('cboStatoLetturaPage').disabled=true;
			document.getElementById('lstAnomalieScelte').disabled=false;
			document.getElementById('lstAnomalie').disabled=false;
			document.getElementById('Button1').disabled=false;
			document.getElementById('Button2').disabled=false;
        }
        else
        {
			if(IsBlank(document.getElementById('txtDataPassaggio').value))   
			{
				document.getElementById('chkFatturazione').disabled=false;
				document.getElementById('chkFatturazioneSospesa').disabled=false;
				document.getElementById('chkIncongruenteForzato').disabled=false;
				document.getElementById('chkGiroContatore').disabled=false;

				document.getElementById('txtConsEffettivo').disabled=false;

				document.getElementById('txtDatadiLettura').disabled=false;
				document.getElementById('txtLetturaAttuale').disabled=false;
				document.getElementById('cboModalitaLettura').disabled=false;
				document.getElementById('cboStatoLetturaPage').disabled=false;
				document.getElementById('lstAnomalieScelte').disabled=false;
				document.getElementById('lstAnomalie').disabled=false;
				document.getElementById('Button1').disabled=false;
				document.getElementById('Button2').disabled=false;
				document.getElementById('txtDatadiLettura').focus();
			}
        }
        }
        
	function DisabilitaAbilitaCampiDataPassaggio()
	{
		if(!IsBlank(document.getElementById('txtDataPassaggio').value))   
        {
			document.getElementById('chkFatturazione').disabled=true;
			document.getElementById('chkFatturazioneSospesa').disabled=true;
			document.getElementById('chkIncongruenteForzato').disabled=true;
			document.getElementById('chkGiroContatore').disabled=true;
			
			document.getElementById('chkGiroContatore').checked=false;
			document.getElementById('chkIncongruenteForzato').checked=false;
			document.getElementById('chkFatturazioneSospesa').checked=false;
			document.getElementById('chkFatturazione').checked=false;
			
			document.getElementById('cboModalitaLettura').value='-1';
			document.getElementById('cboStatoLetturaPage').value='-1';
			
			document.getElementById('txtDatadiLettura').value='';
			document.getElementById('txtLetturaAttuale').value='';
			document.getElementById('txtConsEffettivo').value='';
			document.getElementById('txtGGConsumo').value='';
			document.getElementById('txtLetturaTeorica').value='';
			document.getElementById('txtConsumoTeorico').value='';	
			
			document.getElementById('txtDatadiLettura').disabled=true;
			document.getElementById('txtLetturaAttuale').disabled=true;
			document.getElementById('txtConsEffettivo').disabled=true;
			document.getElementById('cboModalitaLettura').disabled=true;
			document.getElementById('cboStatoLetturaPage').disabled=true;
			document.getElementById('lstAnomalieScelte').disabled=false;
			document.getElementById('lstAnomalie').disabled=false;
			document.getElementById('Button1').disabled=false;
			document.getElementById('Button2').disabled=false;
        }
        else
        {
            if(document.getElementById('chkLasciatoAvviso').checked == false)   
            {
				document.getElementById('chkFatturazione').disabled=false;
				document.getElementById('chkFatturazioneSospesa').disabled=false;
				document.getElementById('chkIncongruenteForzato').disabled=false;
				document.getElementById('chkGiroContatore').disabled=false;

				document.getElementById('txtConsEffettivo').disabled=false;

				document.getElementById('txtDatadiLettura').disabled=false;
				document.getElementById('txtLetturaAttuale').disabled=false;
				document.getElementById('cboModalitaLettura').disabled=false;
				document.getElementById('cboStatoLetturaPage').disabled=false;
				document.getElementById('lstAnomalieScelte').disabled=false;
				document.getElementById('lstAnomalie').disabled=false;
				document.getElementById('Button1').disabled=false;
				document.getElementById('Button2').disabled=false;
				document.getElementById('txtDatadiLettura').focus();
            }
        }
	}
				
	function ConfermaConsumoNegativo()
	{
		if (confirm('Attenzione:\nLa lettura inserita risulta essere incongruente\ncontinuare con la registrazione dei dati ? '))
		{
			document.getElementById('btnAppoggio').click();         
		}
		else
		{
			return false;
		}
	}	
			
	function ConfermaConsumoNegativoGriglia()
	{
		if (confirm('Attenzione:\nLa lettura inserita risulta essere incongruente\ncontinuare con la registrazione dei dati ? '))
		{
      		document.getElementById('txtConfirm').value='1'   
			document.getElementById('btnAppoggioGriglia').click();         
		}
		else
		{
			document.getElementById('txtConfirm').value=''
			return false;
		}
	}	
	
	function SetPosition()
	{
		strSetPosition=document.getElementById('txtSetPosition').value;
		
		if(strSetPosition='')
		{
			location.href='#griglia';
			Setfocus(document.getElementById('txtUbicazione'));
		}
		else
		{
			lh='#' + strSetPosition	
			location.href=lh
		}
	}

	function PulisciCampi()
	{
		
	}	
		
	function VisualizzaGiri(txtTemp,txtIDTemp,txtFocusTemp,objForm)
	{
		strtxtTemp=txtTemp.name;
		strtxtIDTemp=txtIDTemp.name;
		strtxtFocusTemp=txtFocusTemp.name;
		strFormName=objForm.name;

		WinPopUp=OpenPopup('OpenUtenze','<%=Session("PATH_APPLICAZIONE")%>/DataEntryLetture/VisualizzaGiri.aspx?FIELDMNAME='+strtxtTemp+'&IDFIELDMNAME='+strtxtIDTemp+'&FOCUSFIELDMNAME='+strtxtFocusTemp+'&FORM='+strFormName,'W3','500','500',0,0,'yes','no');
	}

	function PulisciCampo(txtTemp,txtIDTemp)
	{
		document.getElementById('txtTemp').value=''
		document.getElementById('txtIDTemp').value=''
	}

	function VisualizzaPosizione(txtTemp,txtIDTemp,txtFocusTemp,objForm)
	{
		strtxtTemp=txtTemp.name;
		strtxtIDTemp=txtIDTemp.name;
		strtxtFocusTemp=txtFocusTemp.name;
		strFormName=objForm.name;

		WinPopUp=OpenPopup('OpenUtenze','<%=Session("PATH_APPLICAZIONE")%>/DataEntryLetture/VisualizzaPosizione.aspx?FIELDMNAME='+strtxtTemp+'&IDFIELDMNAME='+strtxtIDTemp+'&FOCUSFIELDMNAME='+strtxtFocusTemp+'&FORM='+strFormName,'W3','500','500',0,0,'yes','no');
	}
		
	function VisualizzaAnomalia(txtTemp,txtIDTemp,txtFocusTemp,objForm)
	{
		strtxtTemp=txtTemp.name;
		strtxtIDTemp=txtIDTemp.name;
		strtxtFocusTemp=txtFocusTemp.name;
		strFormName=objForm.name;

		WinPopUp=OpenPopup('OpenUtenze','<%=Session("PATH_APPLICAZIONE")%>/DataEntryLetture/VisualizzaAnomalie.aspx?FIELDMNAME='+strtxtTemp+'&IDFIELDMNAME='+strtxtIDTemp+'&FOCUSFIELDMNAME='+strtxtFocusTemp+'&FORM='+strFormName,'W3','500','500',0,0,'yes','no');
	}
		
	function VerificaData(txtData)
	{
		if (!IsBlank(txtData.value ))
		{	
			if(!isDate(txtData.value)) 
			{
				alert("Inserire la Data di Lettura correttamente in formato: GG/MM/AAAA!");
				txtData.value='';
				Setfocus(txtData);
				return false;
			}
		}				
	}	
	
	function VerificaDataDiLettura(txtData)
	{
		if (!IsBlank(txtData.value ))
		{	
			if(!isDate(txtData.value)) 
			{
				alert("Inserire la Data di Lettura correttamente in formato: GG/MM/AAAA!");
				txtData.value='';
				Setfocus(txtData);
				return false;
			}		
			else
			{
			ExecDataLettura();
			}		
		}	
	}
	
	function Salva()
	{
	    console.log("letturefase2");
		document.getElementById('btnConferma').click();
	}
	
	function Annulla()
	{
		frmHidden.action = "../DataEntryLetture/RicercaLetture.aspx";
		frmHidden.submit();
	}
	
	function ExecDataLettura()
	{
		parent.frames.item('Nascosto').location.href='ExecDataLettura.aspx?IDCONTATORE=' +getElementById('IDContatore').value +'&DATALETTURA=' +getElementById('txtDatadiLettura').value+'&IDLETTURA='+getElementById('IDLettura').value;
	}

	function ExecLettura()
	{
		if(document.getElementById('txtLetturaAttuale').value!='')
		{
			parent.frames.item('Nascosto').location.href='ExecLettura.aspx?IDCONTATORE=' +getElementById('IDContatore').value +'&LETTURA=' +getElementById('txtLetturaAttuale').value+'&DATALETTURA='+getElementById('txtDatadiLettura').value+'&IDLETTURA='+getElementById('IDLettura').value;
		}
	}

	function ErrorDataLettura()
	{
		GestAlert('a', 'warning', '', '', 'Attenzione: data di lettura Inserita non valida.Verificare se esiste la data Successiva e quella Precedente. La data di lettura deve essere inoltre coerente con la data di attivazione del contatore (o sostituzione, nel caso fosse stato sostituito)');
		document.getElementById('txtDatadiLettura').focus();
		document.getElementById('txtDatadiLettura').value='';
	}
		
	 function VerificaConsumoNegativo()
	 {
		if(document.getElementById('txtConsEffettivo').value <0 && document.getElementById('txtSubConsumo').value=='')
		{
			if (confirm('Attenzione:\nLa lettura inserita risulta essere incongruente\ncontinuare con la registrazione dei dati ? '))
			{
				if(!VerificaCampi())
				{
					return false;
				}
				else
				{
					document.frmHidden.submit(); 
				}
			}
			else
			{
				return false;
			}
		}
		else
		{
			if (confirm('Si vogliono salvare le modifiche apportate?'))
			{
				return false;
			}
			else
			{
				if(!VerificaCampi())
				{
					return false;
				}
				else
				{
					document.frmHidden.submit(); 
				}
			}
		}
		return true;
	 } 
		
	function VerificaCampi()
	{	
		var i; var iLength; 
		isel=document.getElementById('cboSelezionePeriodo').selectedIndex;
		iLength=document.getElementById('lstAnomalieScelte').options.length;	
		sMsg=""

		if(document.getElementById('chkLasciatoAvviso').checked==true )
		{ 
			if (IsBlank(document.getElementById('txtDataPassaggio').value)) 
				{
					sMsg = sMsg + "[Inserire la Data di Passaggio]\n";  
				} 
		}  
    
        if(document.getElementById('chkLasciatoAvviso').checked==false && IsBlank(document.getElementById('txtDataPassaggio').value))
        {
            if (IsBlank(document.getElementById('txtDatadiLettura').value)) 
			{ 
				sMsg = sMsg + "[Data  Lettura]\n" ; 
			} 
							
			if (IsBlank(document.getElementById('txtLetturaAttuale').value ))
			{
				if(iLength ==0)
				{
					sMsg = sMsg + "[Lettura Attuale. Per non salvare la lettura inserire almeno una anomalia]\n" ; 
				}	
			}
							
			if(SelezionePeriodo.style.display=='')
			{
				if(document.getElementById('cboSelezionePeriodo[isel]').value == '-1')
				{
					sMsg = sMsg + "[Selezione Periodo]\n" ; 
				}
			} 	
		}	
				
		if(document.getElementById('chkLasciatoAvviso').checked == true  || !IsBlank(document.getElementById('txtDataPassaggio').value)) 
		{
			if(SelezionePeriodo.style.display=='')
			{
				if(document.getElementById('cboSelezionePeriodo[isel]').value == '-1')
				{
					sMsg = sMsg + "[Selezione Periodo]\n" ; 
				}
			} 	
		}
    
		if (IsBlank(sMsg)) 
		{ 
			return true; 
		} 
		else 
		{ 
			strMessage ="Attenzione...\n\n I campi elencati sono obbligatori!\n\n" 
			alert(strMessage + sMsg);
			Setfocus(document.getElementById('txtDatadiLettura')) ;
			return false; 
		} 
	  } 
</script>
</HEAD>
	<BODY class="Sfondo" bottomMargin="0" leftMargin="15" topMargin="0" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<br>
			<table cellSpacing="0" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td class="lstTabRow" colSpan="4">&nbsp;</td>
				</tr>
				<tr>
				</tr>
				<tr>
					<td class="lstTabRow NormalRed" colspan="4" style="font-size: 16px;">ATTENZIONE! Per modificare una lettura già presente bisogna cancellarla e inserirla come nuova</asp:label>
					</td>
				</tr>
				<tr>
					<td class="lstTabRow" colSpan="4">&nbsp;</td>
				</tr>
				<tr>
				</tr>
				<tr>
					<td class="lstTabRow" colSpan="4">Dati di Lettura&nbsp;<asp:label id="lblContatore" runat="server" cssclass="NormalBold"></asp:label>
					</td>
				</tr>
				<tr>
					<td colSpan="4"><asp:label id="lblError" runat="server" Cssclass="NormalBold"></asp:label></td>
				</tr>
				<tr>
					<td colSpan="4">&nbsp;</td>
				</tr>
				<tr>
					<td class="Input_Label">Data Lettura Precedente</td>
					<td class="Input_Label">Lettura Precedente</td>
					<td class="Input_Label">Consumo SubContatore</td>
					<td class="Input_Label" id="SelezionePeriodo" style="DISPLAY: none">Selezione Periodo&nbsp;<FONT class="NormalRed">*</FONT></td>
				</tr>
				<tr>
					<td><asp:textbox id="txtDatadiLetturaPrec" runat="server" Width="96px" cssclass="Input_Text" MaxLength="10" ToolTip="Data Lettura Precedente" Enabled="False"></asp:textbox></td>
					<td><asp:textbox id="txtLetturaAttualePrec" runat="server" Width="120px" cssclass="Input_Number_Generali" MaxLength="10" ToolTip="Lettura Precedente" Enabled="False"></asp:textbox></td>
					<td><asp:textbox id="txtSubConsumo" runat="server" Width="120px" cssclass="Input_Number_Generali" MaxLength="10" ToolTip="Consumo SubContatore" Enabled="False"></asp:textbox></td>
					<td><asp:dropdownlist id="cboSelezionePeriodo" style="DISPLAY: none" runat="server" Width="152px" cssclass="Input_Text"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td class="Input_Label">Data Lettura&nbsp;<FONT class="NormalRed">*</FONT></td>
					<td class="Input_Label">Lettura Attuale <FONT class="NormalRed">*</FONT></td>
					<td class="Input_Label">Giorni di Consumo</td>
					<td class="Input_Label">Consumo Effettivo</td>
				</tr>
				<tr>
					<!--<td><asp:textbox id="txtDatadiLetturaOLD" onblur="txtDateLostfocus(this);VerificaDataDiLettura(this);" onfocus="txtDateGotfocus(this);" runat="server" Width="96px" cssclass="Input_Text" MaxLength="10" ToolTip="Data ultima Lettura"></asp:textbox></td>-->
					<td><asp:textbox id="txtDatadiLettura" onblur="txtDateLostfocus(this);" onfocus="txtDateGotfocus(this);" runat="server" Width="96px" cssclass="Input_Text" MaxLength="10" ToolTip="Data ultima Lettura" AutoPostBack="true"></asp:textbox></td>
					<td><asp:textbox id="txtLetturaAttuale" onkeyup="disableLetterChr(this);" runat="server" Width="120px" cssclass="Input_Number_Generali" MaxLength="10" ToolTip="Lettura Attuale" AutoPostBack="true"></asp:textbox></td>
					<td><asp:textbox id="txtGGConsumo" runat="server" Width="112px" cssclass="Input_Number_Generali" ToolTip="Giorni di Consumo" ReadOnly="True"></asp:textbox></td>
					<td><asp:textbox id="txtConsEffettivo" onkeyup="disableLetterChar(this);" runat="server" Width="120px" cssclass="Input_Number_Generali" MaxLength="10" ToolTip="Consumo Effettivo"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label">Lettura Teorica</td>
					<td class="Input_Label">Consumo Teorico</td>
					<td class="Input_Label">Modalita' Lettura</td>
					<td class="Input_Label">Stato Lettura</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtLetturaTeorica" runat="server" Width="124px" cssclass="Input_Number_Generali" ToolTip="Lettura Teorica" ReadOnly="True" TextMode="SingleLine"></asp:textbox></td>
					<td><asp:textbox id="txtConsumoTeorico" tabIndex="3" runat="server" Width="124px" cssclass="Input_Number_Generali" ToolTip="Consumo Teorico" ReadOnly="True"></asp:textbox></td>
					<td><asp:dropdownlist id="cboModalitaLettura" runat="server" Width="152px" cssclass="Input_Text"></asp:dropdownlist></td>
					<td><asp:dropdownlist id="cboStatoLetturaPage" runat="server" Width="152px" cssclass="Input_Text"></asp:dropdownlist></td>
				</tr>
				<tr id="ContatoriField">
				    <td><asp:CheckBox ID="chkPrimaLett" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Prima Lettura" Enabled="false" /></td>
					<td><asp:checkbox id="chkFatturazione" runat="server" Cssclass="Input_CheckBox_NoBorder" Text="Fatturazione" Enabled="false"></asp:checkbox></td>
					<td><asp:checkbox id="chkGiroContatore" runat="server" Cssclass="Input_CheckBox_NoBorder" Text="Giro Contatore" Enabled="false"></asp:checkbox></td>
					<td><asp:checkbox id="chkConsumoNegativoForzato" runat="server" Cssclass="Input_CheckBox_NoBorder" Text="Consumo Negativo Forzato"></asp:checkbox>
					    <asp:checkbox style="DISPLAY: none" id="chkFatturazioneSospesa" runat="server" Cssclass="Input_CheckBox_NoBorder" Text="Fatturazione Sospesa"></asp:checkbox>&nbsp;
					    <asp:checkbox style="DISPLAY: none" id="chkIncongruenteForzato" runat="server" Cssclass="Input_CheckBox_NoBorder" Text="Forza Incongruenza"></asp:checkbox></td>
				</tr>
			</table>
			<br>
			<fieldset class="classeFiledSet">
				<legend class="Legend">Gestione Anomalie</legend>
				<table cellSpacing="0" cellPadding="1" width="98%" align="center" border="0">
					<tr id="LettureLabel" style="DISPLAY: none">
						<td class="Input_Label">Data Passaggio</td>
						<td class="Input_Label" colSpan="3">Lasciato Avviso</td>
					</tr>
					<tr id="LettureField" style="DISPLAY: none">
						<td><asp:textbox id="txtDataPassaggio" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" Width="100px" cssclass="Input_Text" MaxLength="10" ToolTip="Data da Associare al Lasciato Avviso"></asp:textbox></td>
						<td colSpan="3"><asp:checkbox id="chkLasciatoAvviso"  runat="server" Cssclass="Input_CheckBox_NoBorder" Width="20px" ToolTip="Lasciato Avviso" Text=" " Height="6px"></asp:checkbox></td>
					</tr>
					<tr id="LettureFieldAnomalie" style="DISPLAY: none">
						<td noWrap colSpan="4">
							<table>
								<tr>
									<td class="Input_Label">Elenco Anomalie</td>
									<td>&nbsp;</td>
									<td class="Input_Label">Anomalie Applicate</td>
								</tr>
								<tr>
									<td><asp:listbox id="lstAnomalie" runat="server" Width="308px" cssclass="Input_Text" Height="116px" SelectionMode="Multiple" onchange="return limitOptions(this, 3);"></asp:listbox></td>
									<td>
										<table border="0">
											<tr>
												<td align="center"><asp:button id="Button1" runat="server" cssclass="Bottone Bottone_Elimina" ToolTip="Sposta le anomalie dalla Lista di Sinistra a quella di Destra" Text=">>" Font-Bold="True" ForeColor="Black"></asp:button></td>
											</tr>
											<tr>
												<td align="center"><asp:button id="Button2" runat="server" cssclass="Bottone Bottone_Elimina" ToolTip="Elimina le anomalie dalla Lista di Destra" Text="<<" Font-Bold="True" ForeColor="Black"></asp:button></td>
											</tr>
										</table>
									</td>
									<td><asp:listbox id="lstAnomalieScelte" runat="server" Width="312px" cssclass="Input_Text" Height="116px" SelectionMode="Multiple" onChange=""></asp:listbox></td>
								</tr>
								<tr>
									<td colSpan="3"><asp:label id="maxanomalie" runat="server" Cssclass="NormalBold"></asp:label></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</fieldset>
			<table cellSpacing="0" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td class="Input_Label" colSpan="4">Note Lettura</td>
				</tr>
				<tr>
					<td colSpan="4">
						<asp:textbox id="txtNoteLettura" runat="server" Width="496px" cssclass="Input_Text" MaxLength="500" ToolTip="Note di Lettura" TextMode="MultiLine" Height="61px"></asp:textbox>
					</td>
				</tr>
				<tr id="gridanomalie" style="DISPLAY: none">
					<td colSpan="4">
						<Grd:RibesGridView ID="GrdLettureHome" runat="server" BorderStyle="None" 
							BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
							AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
							ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
							<PagerSettings Position="Bottom"></PagerSettings>
							<PagerStyle CssClass="CartListFooter" />
							<RowStyle CssClass="CartListItem"></RowStyle>
							<HeaderStyle CssClass="CartListHead"></HeaderStyle>
							<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
								<asp:TemplateField HeaderText="Data Passaggio">
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# FncGrd.FormatGrdData(DataBinder.Eval(Container, "DataItem.DATADIPASSAGGIO")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Anomalia 1">
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# DescrizioneAnomalia(DataBinder.Eval(Container, "DataItem.COD_ANOMALIA1")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Anomalia 2">
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# DescrizioneAnomalia(DataBinder.Eval(Container, "DataItem.COD_ANOMALIA2")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Anomalia 3">
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# DescrizioneAnomalia(DataBinder.Eval(Container, "DataItem.COD_ANOMALIA3")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
							</Columns>
							</Grd:RibesGridView>
					</td>
				</tr>
			</table>
			<asp:textbox id="txtIDAnomalia" style="DISPLAY: none" runat="server"></asp:textbox>
			<asp:button id="btnConferma" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="btnAppoggio" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="btnElimina" style="DISPLAY: none" runat="server"></asp:button>
			<input id="IDContatore" type="hidden" name="IDContatore">
			<input id="IDLettura" type="hidden" name="IDLettura"> 
			<input id="hdDataLettura" type="hidden" name="hdDataLettura">
			<input id="hdLettura" type="hidden" name="hdLettura">
			<input id="hdIDContatore" type="hidden" name="hdIDContatore"><input id="hdCodiceVia" type="hidden" name="hdCodiceVia" value="-1">
			<input id="hdIntestatario" type="hidden" name="hdIntestatario"> <input id="hdUtente" type="hidden" name="hdUtente">
			<input id="hdUbicazioneText" type="hidden" name="hdUbicazioneText"> <input id="hdGiro" type="hidden" name="hdGiro" value="-1">
			<input id="hdNumeroUtente" type="hidden" name="hdNumeroUtente"> <input id="hdNonLetti" type="hidden" name="hdNonLetti">
			<input id="hdDaRicontrollare" type="hidden" name="hdDaRicontrollare"> <input id="hdConAnomalie" type="hidden" name="hdConAnomalie">
			<input id="hdCessati" type="hidden" name="hdCessati" value="-1"> <input id="paginacomandi" type="hidden" name="paginacomandi">
			<input id="PAG_PREC" type="hidden" name="PAG_PREC">
		</form>
	</BODY>
</HTML>
