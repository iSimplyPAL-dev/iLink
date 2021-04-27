<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrameLetture.aspx.vb" Inherits="OpenUtenze.FrameLetture" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<TITLE>Gestione Letture</TITLE>
		<script language="VB" runat="server">
			Dim IDLettura as integer 
			Dim IDCONTATORE as integer 
			Dim title as String 	
			Dim PAG_PREC  as String 	
			dIM ViewLetture as integer ' Gestione Bottoni pagina comandi
			
		Sub Page_Load(Src As Object, E As EventArgs)
			
			ViewLetture=Request.Params("VIEWLETTURE")
			PAG_PREC=Request.Params("PAG_PREC")
			IDLettura=Request.Params("IDLETTURA")
			IDCONTATORE=Request.Params("IDCONTATORE")
		
			if IDCONTATORE=0 then
				title="Utility - Contatori - " & Session("DESC_TIPO_PROC_SERV") & " - " & "Inserimento Letture" & "&enteperiodo=" & " Ente: " & Session("COMUNEENTE") & " - Periodo: " & Session("PERIODOID")
			end if
			if IDCONTATORE>0 then
				title="Utility - Contatori - " & Session("DESC_TIPO_PROC_SERV") & " - " & "Modifica Letture" & "&enteperiodo=" & " Ente: " & Session("COMUNEENTE") & " - Periodo: " & Session("PERIODOID")
			end if

		End Sub
		</script>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<frameset rows="45,*,0" framespacing="0" border="1" frameborder="no" id="framePopUp">
		<frame name="Comandi" src="ComandiLetture.aspx?title=<%=title%>&PAG_PREC=<%=request.item("PAG_PREC")%>&VIEWLETTURE=<%=Request.item("VIEWLETTURE")%>" scrolling="no" noresize>
		<frame name="visualizza" src="Letture.aspx?hdIDContatore=<%=Request.item("IDCONTATORE")%>&PAG_PREC=2">
		<frame name="Nascosto" src="../../aspVuota.aspx">
	</frameset>
</HTML>
