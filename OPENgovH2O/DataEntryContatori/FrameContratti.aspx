<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FrameContratti.aspx.vb" Inherits="OpenUtenze.FrameContratti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<TITLE>Gestione Contratti</TITLE>
		<script language="VB" runat="server">
			Dim IDCONTRATTO as integer 
			Dim title as String 
			DIm hdCodiceContratto as string 
			Dim hdDataSottoScrizione as string 
			Dim hdNumeroUtenzeContratto as string 
			Dim hdTipoUtenzaContratto as integer
			Dim hdIdDiametroContatoreContratto as integer
			Dim hdIdDiametroPresaContratto as integer
			
				
		Sub Page_Load(Src As Object, E As EventArgs)
			
			hdCodiceContratto=Request.Params("hdCodiceContratto")
			hdDataSottoScrizione =Request.Params("hdDataSottoScrizione")
			hdNumeroUtenzeContratto= Request.Params("hdNumeroUtenzeContratto")
			hdTipoUtenzaContratto =CInt(Request.Params("hdTipoUtenzaContratto"))
			hdIdDiametroContatoreContratto= CInt(Request.Params("hdIdDiametroContatoreContratto"))
			hdIdDiametroPresaContratto =CInt(Request.Params("hdIdDiametroPresaContratto"))
			
			IDCONTRATTO=Request.Params("idcontratto")
			if IDCONTRATTO=-1 then
		            title = "Utility - Contatori - " & Session("DESC_TIPO_PROC_SERV") & " - " & "Inserimento Contratto" & "&enteperiodo=" & " Ente: " & Session("COMUNEENTE") & " - Periodo: " & Session("PERIODOID")
			end if
			if IDCONTRATTO >0  then
		            title = "Utility - Contatori - " & Session("DESC_TIPO_PROC_SERV") & " - " & "Modifica Contratto" & "&enteperiodo=" & " Ente: " & Session("COMUNEENTE") & " - Periodo: " & Session("PERIODOID")
			end if
		End Sub
		</script>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<frameset rows="45,*,0" framespacing="0" border="1" frameborder="no" id="framePopUp">
		<frame name="Comandi" src="ComandiContratti.aspx?title=<%=title%>" scrolling="no" noresize>
		<frame name="visualizza" src="Contratti.aspx?idcontratto=<%=Request.item("idcontratto")%>&hdCodiceContratto=<%=Request.item("hdCodiceContratto")%>&hdDataSottoScrizione=<%=Request.item("hdDataSottoScrizione")%>&hdNumeroUtenzeContratto=<%=hdNumeroUtenzeContratto%>&hdTipoUtenzaContratto=<%=hdTipoUtenzaContratto%>&hdIdDiametroContatoreContratto=<%=hdIdDiametroContatoreContratto%>&hdIdDiametroPresaContratto=<%=hdIdDiametroPresaContratto%>">
		<frame name="Nascosto" src="../../aspVuota.aspx">
	</frameset>
</HTML>
