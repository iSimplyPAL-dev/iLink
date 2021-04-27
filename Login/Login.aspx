<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Login.aspx.vb" Inherits="OPENgov.Login" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>Login -- OPENgov -- </title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../_js/Utility.js?newversion"></SCRIPT>
		<script type="text/javascript">	
		function keyPress()
		{
		 if(window.event.keyCode==13)
		 {
		   if(!Controlla())
		   {
			return false;
		   } 
		 }
		}	
		function Controlla()
		{
			//document.getElementById('TestValidita').value ="0";
			if(IsBlank(document.getElementById('Username').value) || IsBlank(document.getElementById('Password').value))
			{
				document.getElementById('TestValidita').value ="1";
				GestAlert('a', 'warning', '', '', 'Inserire Username e Password');
				Setfocus(document.getElementById('Username'));
				return false;	
			}
			else
			{	
				document.getElementById('Accedi').click();
				return true;
			}
		}
		
		function setfocus(){
			if (document.getElementById('Form1').style.display!="none" )
			    document.getElementById('Username').focus()
		}
		
		</script>
		<script type="text/vbscript">
		
		function ChiudiFinestra()
			window.close()
			opener.focus()
		end function 
		</script>
</HEAD>
	<body class="SfondoGenerale" onload="setfocus();" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="4" width="100%" border="0">
				<tr vAlign="top">
					<td style="WIDTH: 5px">&nbsp;
					</td>
					<td id="LeftPane">
						<hr style="WIDTH: 170px" class="hrlogin" align="left">
						<span class="Input_Label_login_Account" style="HEIGHT: 20px">Account Utente</span>
						<br>
						<span class="Input_Label_login">User Name:</span>
						<br>
						<asp:textbox id="Username" onkeydown="keyPress();" runat="server" CssClass="Input_Text" Width="165"></asp:textbox><br>
						<span class="Input_Label_login">Password:</span>
						<br>
						<asp:textbox id="Password" onkeydown="keyPress();" runat="server" CssClass="Input_Text" Width="165" TextMode="Password" AutoCompleteType="Disabled" autocomplete="off"></asp:textbox><br>
						<br>
						<asp:label id="lblMessage" runat="server" CssClass="ErrorMessage"></asp:label>
						<br>
						<br>
						<input id="btnRibaltaTesto" onclick="return Controlla();" type="button" value="Accedi" name="image1">
					</td>
				</tr>
			</table>
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
			<INPUT style="WIDTH: 80px; HEIGHT: 20px" type="hidden" size="8" name="TestValidita">
			<asp:Button id="Accedi" style="DISPLAY:none" runat="server" Height="24px" Width="72px" Text="Accedi" Cssclass="Bottone Bottone"></asp:Button>
		</form>
	</body>
</HTML>
