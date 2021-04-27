<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ControlloCinCFPI.aspx.vb" Inherits="ControlloCinCFPI"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>ControlloCinCFPI</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script type="text/javascript" type="text/javascript">
			// funzione che chiede la conferma di salvataggio in caso di CIN errato
				function ConfermaCINErrato(MessaggioErrore){
					//alert('ConfermaCin');
				    if (confirm(MessaggioErrore + ' Si vuole salvare ugualmente il dato?')){
						parent.document.getElementById('btnSalva').click();
					}else{
						parent.document.getElementById('txtPartitaIva').value = '';
						parent.document.getElementById('txtCodiceFiscale').value = '';
						parent.document.getElementById('btnSalva').click();
					}	
				}
				
				function SalvaAnagrafe(){
					parent.document.getElementById('btnSalva').click();
				}
		</script>
	</head>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
		</form>
	</body>
</html>
