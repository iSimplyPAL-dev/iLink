'***********************************************************************
'Class: VariazioniContratti
'
'Scopo: La Classe 
'ha lo scopo di salvare i dati delle variazioni contrattuali nel Database --OpenUtenze
'
'Calls : VariazioniContrattuali/COntrolloStato1.aspx
'Author : Antonello Lo Bianco Luglio 29, 2003
'***********************************************************************
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Public Class VariazioniContratti
    '***********************************************************************
    'Sub: SetVariazioniContrattuali
    '
    'Scopo: 
    'ha lo scopo di salvare i dati delle variazioni contrattuali nel Database --OpenUtenze
    '
    'Calls : VariazioniContrattuali/COntrolloStato1.aspx
    'Parameters: strIDDOC valore del Codice Contatore Nuovo
    '                   strIDDOCOLD valore del Codice Contatore Vecchio
    '                   strXMLFileName Nome e Path del File Xml
    'Author : Antonello Lo Bianco Luglio 29, 2003
    '***********************************************************************
    Inherits DBManager  'Eridita dalla classe DBMANGER

    Private Shared Log As ILog = LogManager.GetLogger(GetType(VariazioniContratti))

    '  Public Sub setDataVariazioniContrattuali(ByVal strIDDOC As String, _
    '                                                             ByVal strIDDOCOLD As String, _
    '                                                             ByVal strXMLFileName As String)
    '    Dim sqlConn As New SqlConnection()
    '    Dim sqlParm As New SqlParameter()
    '    Dim sqlTrans As SqlTransaction
    '    Dim blnOkDB As Boolean

    '    sqlConn.ConnectionString = ConstSession.StringConnection
    '    sqlConn.Open()
    '    'Inizio la transazione
    '    sqlTrans = sqlConn.BeginTransaction

    '    blnOkDB = setTP_CONTATORI(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertTP_CONTATORI")
    '    'Salvataggio dei dati Per Lo storico
    'Try
    '    If blnOkDB Then blnOkDB = setTP_CONTRATTI_STO(sqlConn, sqlTrans, strIDDOCOLD, "sp_InsertTP_CONTRATTI_STO")
    '    blnUPDATE = True
    '    If blnOkDB Then blnOkDB = setTP_CONTRATTI(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_UpdateTP_CONTRATTI", True)
    '    If blnOkDB Then blnOkDB = setANAGRAFE_INTESTATARIO(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_UpdateANAGRAFE_INTESTATARIO")
    '    blnUPDATE = False
    '    If blnOkDB Then blnOkDB = setTR_CONTATORI_INTESTATARIO(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_TRInsertCommand")
    '    If blnOkDB Then blnOkDB = setANAGRAFE_CONTATORE(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertANAGRAFE_CONTATORE")
    '    If blnOkDB Then blnOkDB = setTP_LETTURE(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertTP_LETTURE")
    '    If blnOkDB Then blnOkDB = setTP_PREVENTIVO(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertTP_PREVENTIVO")

    '    If blnOkDB Then
    '      sqlTrans.Commit()
    '    Else
    '      sqlTrans.Rollback()
    '      Throw New System.Exception("Si è verificato un errore durante il  Salvataggio dei dati nel DataBase:OpenUtenze-Verificare il file ErrorDb_logError.log !")
    '    End If
    '    sqlConn.Close()

    '  Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VariazioniContratti.setDataVariazioniContrattuali.errore: ", ex)
    'End Try
    'End Sub
End Class
