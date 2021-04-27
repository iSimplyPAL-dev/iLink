<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaCanoni.aspx.vb" Inherits="OpenUtenze.RicercaCanoni"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>RicercaCanoni</title>
<meta content="Microsoft Visual Studio .NET 7.1" name=GENERATOR>
<meta content="Visual Basic .NET 7.1" name=CODE_LANGUAGE>
<meta content=JavaScript name=vs_defaultClientScript>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema>
<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
<script language=javascript>						
			function Search()
			{			
			    document.getElementById('loadGrid').src = 'ResultRicercaCanoni.aspx?IdCanone=' + document.getElementById('ddlTipoCanone').value + '&Anno=' + escape(document.getElementById('txtAnno').value)
				return true;
			}
			
			function NewInsert()
			{			
				parent.Comandi.location.href='./CConfiguraCanoni.aspx?Inserimento=Inserimento'
				loadInsert.src="./ConfiguraCanoni.aspx";
				return true;
			}
			
			function ControllaAnno(oggetto){
				if (!IsBlank(oggetto.value)){
					if (!isNumber(oggetto.value, 4, 0, 1950, 2090)){
						alert ("Inserire un anno di quattro cifre\ncompreso fra 1950 e 2090")
						oggetto.value=""
						oggetto.focus()
						return false
					}
				}
			}			
		</script>
</HEAD>
<body class=Sfondo leftMargin=3 rightMargin=3>
    <form id="Form1" runat="server" method="post">
        <table id=tabEsterna cellSpacing=1 cellPadding=1 width="100%" border=0>
          <tr>
            <td>
              <FIELDSET class=FiledSetRicerca style="WIDTH: 98%; HEIGHT: 79px"><LEGEND class="Legend">Inserimento filtri di ricerca</LEGEND>
              <table width="100%">
                <tr>
                  <td width="15%"><asp:label id=Label3 CssClass="Input_Label" Runat="server">Anno</asp:Label><br><asp:textbox id=txtAnno onblur="" onfocus="" runat="server" CssClass="Input_Number_Generali" MaxLength="4" Width="72px" onchange="ControllaAnno(this)"></asp:textbox></td>
                  <td width="85%"><asp:label id=Label1 runat="server" CssClass="Input_Label">Tipologia Canone</asp:label><br ><asp:dropdownlist id=ddlTipoCanone runat="server" CssClass="Input_Text" Width="280px"></asp:dropdownlist></td></tr></table></FIELDSET> 
            </td></tr>
          <tr>
            <td style="HEIGHT: 250px"><iframe id=loadGrid style="WIDTH: 100%; HEIGHT: 250px" src="../../../aspVuota.aspx"  frameBorder=0 width="100%" height=250></IFRAME></td></tr>
          <tr>
            <td><iframe id=loadInsert src="../../../aspVuota.aspx" frameBorder=0 width="100%" height=180></IFRAME></td></tr>
        </table>
        <asp:button id="btnRibalta" style="DISPLAY: none" runat="server" onclick="btnRibalta_Click"></asp:button>
    </FORM>
</body>
</HTML>
