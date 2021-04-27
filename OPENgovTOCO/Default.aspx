<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OPENGovTOCO.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
	<HEAD>
		<title>Start</title>
		<%	
		    //parametri per aprire di default la pagina di ricerca pratiche 
            string Comandi = "aspVuota.aspx";//"CAcquisizioneAutomaticaVersamenti.aspx";//
            string Visualizza = "Elaborazioni/ElaborazioneAvvisi.aspx?CodTributo=0453"; //"Dichiarazioni/DichiarazioniSearch.aspx";//"Elaborazioni/AcqFlussi.aspx?CodTributo=9253"; //"AcquisizioneAutomaticaVersamenti.aspx";//"GestionePagamenti/GestionePagamentiSearch.aspx?CodTributo=9253";//"SituazioneContribuente/SituazioneAvvisiSearch.aspx"; //"Elaborazioni/ConfigurazioneRate.aspx?CodTributo=9253"; //"Dichiarazioni/DichiarazioniView.aspx?IdDichiarazione=112&Provenienza=INTERGEN";//"Configurazione/ConfigurazioneTariffe.aspx"; //"Configurazione/ConfigurazioneCoefficienti.aspx"; //"Configurazione/ConfigurazioneTabelle.aspx"; //
		    string Basso = "aspVuota.aspx";
		    string Nascosto = "aspVuota.aspx";
        %>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<frameset rows="89,*" framespacing="0" border="0" frameborder="no">
		<frame name="logo" src="../Generali/asp/aspLogo.aspx" marginwidth="0" marginheight="0" scrolling="no" noresize>
		<frameset cols="205,*" framespacing="0" border="0" frameborder="no">
			<frame name="viste" src="aspVuota.aspx" scrolling="no" marginwidth="0" marginheight="1" noresize>
			<frameset rows="45,*,0,0" framespacing="0" border="1" frameborder="no" id="frameVisualizza">
				<frame name="Comandi" src="<%=Comandi%>" scrolling="no" noresize id="Comandi"></frame>
				<frame name="Visualizza" src="<%=Visualizza%>" noresize id="Visualizza"></frame>
				<frame name="Basso" src="<%=Basso%>" scrolling="no" noresize id="Basso"></frame>
				<frame name="Nascosto" src="<%=Nascosto%>" scrolling="yes" noresize id="Nascosto"></frame>
			</frameset>
		</frameset>
	</frameset>
</html>
