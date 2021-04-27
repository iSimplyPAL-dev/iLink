<%@ Page language="c#" Codebehind="MappaImmobile.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.MappaImmobile" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>MappaImmobile</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%> 
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%> 
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
    <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;key=ABQIAAAA7S9sISmX0w9JTtfmGFDh1xSYuxf31PbwpCnxRkS_c66mehDpfRQd3GQ0h1uxn0fkHx7daD7ifyc5lQ" type="text/javascript"></script>
	<script type="text/javascript" src="_js/geocoding.js?newversion"></script>
  </head>
  <body MS_POSITIONING="GridLayout" class="Sfondo" >
	
    <form id="Form1" runat="server" method="post">
		<div id="map" style="width: 500px; height: 300px"></div>
     </form>
	
  </body>
</html>
