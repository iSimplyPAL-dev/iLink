'***********************************************************************
'Class: SostiuzioneContatore
'
'Scopo: La Classe 
'ha lo scopo di salvare i dati delle variazioni contrattuali nel Database --OpenUtenze
'
'Calls : SostituioneContatore/ControlloStato/COntrolloStato1.aspx
'Author : Antonello Lo Bianco Luglio 29, 2003
'***********************************************************************
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Public Class setSostituzioneContatore
    '***********************************************************************
    'Sub: setDataSostiuzioneContatore
    '
    'Scopo: 
    'ha lo scopo di salvare i dati Della Sostituzione Contatore  nel Database --OpenUtenze
    '
    'Calls : SosstituzioneContatore/ControlloStato/ControlloStato1.aspx
    'Parameters: strIDDOC valore del Codice Contatore Da Sostituire
    '                   strXMLFileName Nome e Path del File Xml
    '                   
    'Author : Antonello Lo Bianco Luglio 29, 2003
    '***********************************************************************

    Inherits DBManager
    Private Shared Log As ILog = LogManager.GetLogger(GetType(setSostituzioneContatore))
    Public Sub setDataSostituzioneContatore(ByVal strIDDOC As String, _
                                                               ByVal strXMLFileName As String)
        Dim sqlConn As New SqlConnection()
        Dim sqlTrans As SqlTransaction
        Dim blnOkDB As Boolean
        Try
            sqlConn.ConnectionString = ConstSession.StringConnection
        sqlConn.Open()
        'Inizio la transazione
        sqlTrans = sqlConn.BeginTransaction
        'Salvataggio dei dati Per Lo storico
        blnOkDB = setTP_CONTATORI_STO(sqlConn, sqlTrans, strIDDOC, "sp_InsertTP_CONTATORI_STO")

        If blnOkDB Then blnOkDB = setTP_CONTATORISOSTITUITI(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_UpdateTP_CONTATORISOSTITUZIONE")

        If blnOkDB Then blnOkDB = setTP_PREVENTIVOSOSTITUZIONE(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_UpdateTP_PREVENTIVOSOSTITUZIONE")

        If blnOkDB Then
            sqlTrans.Commit()
        Else
            sqlTrans.Rollback()
            Throw New System.Exception("Si è verificato un errore durante il  Salvataggio dei dati nel DataBase:OpenUtenze-Verificare il file ErrorDb_logError.log !")
        End If
            sqlConn.Close()

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.setSostituzioneContatore.setDataSostituzioneContatore.errore: ", ex)
        End Try

    End Sub

End Class
