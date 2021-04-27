<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrameInserimentoModifica.aspx.vb" Inherits="OpenUtenze.FrameInserimentoModifica" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<TITLE>Gestione Letture</TITLE>
		<script language="VB" runat="server">
			Dim IDLettura as integer 
			Dim IDContatore as integer 
			Dim title as String 	
			Dim PAG_PREC  as String 
			dim IsFatturata as integer
			
		Sub Page_Load(Src As Object, E As EventArgs)
			
			PAG_PREC=Request.Params("PAG_PREC")
			
			IDLettura=Request.Params("IDLETTURA")
			IDContatore=Request.Params("IDCONTATORE")
			IsFatturata=Request.Params("IsFatturata")
			if IDLettura=0 then
				if PAG_PREC=1 then
		                title = "Acquedotto - " & Session("DESC_TIPO_PROC_SERV") & " - " & "Inserimento Letture" & "&enteperiodo=" & " Ente: " & Session("COMUNEENTE") & " - Periodo: " & Session("PERIODOID")
				else
		                title = "Utility - Contatori - " & Session("DESC_TIPO_PROC_SERV") & " - " & "Inserimento Letture" & "&enteperiodo=" & " Ente: " & Session("COMUNEENTE") & " - Periodo: " & Session("PERIODOID")
				end if
			end if
			
			if IDLettura>0 then
				if PAG_PREC=1 then
		                title = "Acquedotto - " & Session("DESC_TIPO_PROC_SERV") & " - " & "Modifica Letture" & "&enteperiodo=" & " Ente: " & Session("COMUNEENTE") & " - Periodo: " & Session("PERIODOID")
				else
		                title = "Utility - Contatori - " & Session("DESC_TIPO_PROC_SERV") & " - " & "Modifica Letture" & "&enteperiodo=" & " Ente: " & Session("COMUNEENTE") & " - Periodo: " & Session("PERIODOID")
				end if
			end if
		End Sub
		</script>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<frameset rows="45,*,0" framespacing="0" border="1" frameborder="no" id="framePopUp">
		<frame name="Comandi" src="ComandiLetture.aspx?title=<%=title%>&PAG_PREC=<%=request.item("PAG_PREC")%>&IDLETTURA=<%=Request.item("IDLETTURA")%>&IsFatturata=<%=Request.item("IsFatturata")%>" scrolling="no" noresize>
		<frame name="Visualizza" src="ModLettureHome.aspx?PAG_PREC=<%=request.item("PAG_PREC")%>&IDCONTATORE=<%=Request.item("IDCONTATORE")%>&IDLETTURA=<%=Request.item("IDLETTURA")%>&IsFatturata=<%=Request.item("IsFatturata")%>">
		<frame name="Nascosto" src="../../aspVuota.aspx">
	</frameset>
</HTML>
