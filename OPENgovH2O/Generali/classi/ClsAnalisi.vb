Imports log4net

Public Class ClsDettaglioVoci
    Public Const CAPITOLO_CONSUMO As String = "0000"
    Public Const CAPITOLO_CANONI As String = "0001"
    Public Const CAPITOLO_ADDIZIONALI As String = "0002"
    Public Const CAPITOLO_NOLO As String = "0003"
    Public Const CAPITOLO_QUOTAFISSA As String = "0004"
    Public Const CAPITOLO_IVA As String = "9996"
    Public Const CAPITOLO_ARROTONDAMENTO As String = "9999"

    Public Const VOCE_DEPURAZIONE As String = "001"
    Public Const VOCE_FOGNATURA As String = "002"
    Public Const VOCE_DEPURAZIONEQF As String = "0005"
    Public Const VOCE_FOGNATURAQF As String = "0006"

End Class

Public Class ClsAnalisiLevelDB
    Private Shared Log As ILog = LogManager.GetLogger("ClsAnalisiLevelDB")
    Private iDB As New DBAccess.getDBobject
    Private cmdMyCommand As New SqlClient.SqlCommand
    Private DvDati As DataView

    Public Function GetRiepilogoEmesso(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sPeriodo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String) As DataView
        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'valorizzo il commandText 
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "sp_AnalisiRiepilogoEmesso"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURE", SqlDbType.NVarChar)).Value = sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERIODO", SqlDbType.NVarChar)).Value = sPeriodo
            If sAccreditoDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.NVarChar)).Value = sAccreditoDal
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.NVarChar)).Value = sAccreditoAl
            End If
            Log.Debug("ClsAnalisiLevelDB::GetRiepilogoEmesso::sql::" & cmdMyCommand.CommandText & "::ENTE::" & sMyIdEnte & "::sAnno::" & sAnno & "::sPeriodo::" & sPeriodo & "::sAccreditoDal::" & sAccreditoDal & "::sAccreditoAl::" & sAccreditoAl)
            'eseguo la query
            DvDati = iDB.GetDataView(cmdMyCommand)
            Return DvDati
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAnalisiLevelDB.GetRiepilogoEmesso.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function GetRiepilogoDaEmettere(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sPeriodo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String) As DataView
        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'valorizzo il commandText 
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "sp_AnalisiRiepilogodaEmettere"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURE", SqlDbType.NVarChar)).Value = sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERIODO", SqlDbType.NVarChar)).Value = sPeriodo
            If sAccreditoDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.NVarChar)).Value = sAccreditoDal
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.NVarChar)).Value = sAccreditoAl
            End If
            Log.Debug("ClsAnalisiLevelDB::GetRiepilogoDaEmettere::sql::" & cmdMyCommand.CommandText & "::ENTE::" & sMyIdEnte & "::sAnno::" & sAnno & "::sPeriodo::" & sPeriodo & "::sAccreditoDal::" & sAccreditoDal & "::sAccreditoAl::" & sAccreditoAl)
            'eseguo la query
            DvDati = iDB.GetDataView(cmdMyCommand)
            Return DvDati
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAnalisiLevelDB.GetRiepilogoDaEmettere.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function GetRiepilogoEmessoEvaso(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sPeriodo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String, ByVal IsEvaseTotalmente As Integer) As DataView
        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'valorizzo il commandText 
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "sp_AnalisiRiepilogoEmessoEvaso"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURE", SqlDbType.NVarChar)).Value = sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERIODO", SqlDbType.NVarChar)).Value = sPeriodo
            If sAccreditoDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.NVarChar)).Value = sAccreditoDal
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.NVarChar)).Value = sAccreditoAl
            End If
            If IsEvaseTotalmente = 1 Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@EVASETOT", SqlDbType.NVarChar)).Value = 1
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@EVASEPARZ", SqlDbType.NVarChar)).Value = 0
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@EVASETOT", SqlDbType.NVarChar)).Value = 0
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@EVASEPARZ", SqlDbType.NVarChar)).Value = 1
            End If
            Log.Debug("ClsAnalisiLevelDB::GetRiepilogoEmessoEvaso::sql::" & cmdMyCommand.CommandText & "::ENTE::" & sMyIdEnte & "::sAnno::" & sAnno & "::sPeriodo::" & sPeriodo & "::sAccreditoDal::" & sAccreditoDal & "::sAccreditoAl::" & sAccreditoAl & "::IsEvaseTotalmente::" & IsEvaseTotalmente)
            'eseguo la query
            DvDati = iDB.GetDataView(cmdMyCommand)
            Return DvDati
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAnalisiLevelDB.GetRiepilogoEmessoEvaso.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function GetDettaglioEmesso(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sPeriodo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String) As DataView
        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'valorizzo il commandText 
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "sp_AnalisiDettaglioEmesso"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURE", SqlDbType.NVarChar)).Value = sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERIODO", SqlDbType.NVarChar)).Value = sPeriodo
            If sAccreditoDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.NVarChar)).Value = sAccreditoDal
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.NVarChar)).Value = sAccreditoAl
            End If
            Log.Debug("ClsAnalisiLevelDB::GetDettaglioEmesso::sql::" & cmdMyCommand.CommandText & "::ENTE::" & sMyIdEnte & "::sAnno::" & sAnno & "::sPeriodo::" & sPeriodo & "::sAccreditoDal::" & sAccreditoDal & "::sAccreditoAl::" & sAccreditoAl)
            'eseguo la query
            DvDati = iDB.GetDataView(cmdMyCommand)
            Return DvDati
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAnalisiLevelDB.GetDettaglioEmesso.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function GetDettaglioDaEmettere(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sPeriodo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String) As DataView
        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'valorizzo il commandText 
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "sp_AnalisiDettaglioDaEmettere"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURE", SqlDbType.NVarChar)).Value = sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERIODO", SqlDbType.NVarChar)).Value = sPeriodo
            If sAccreditoDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.NVarChar)).Value = sAccreditoDal
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.NVarChar)).Value = sAccreditoAl
            End If
            Log.Debug("ClsAnalisiLevelDB::GetDettaglioDaEmettere::sql::" & cmdMyCommand.CommandText & "::ENTE::" & sMyIdEnte & "::sAnno::" & sAnno & "::sPeriodo::" & sPeriodo & "::sAccreditoDal::" & sAccreditoDal & "::sAccreditoAl::" & sAccreditoAl)
            'eseguo la query
            DvDati = iDB.GetDataView(cmdMyCommand)
            Return DvDati
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAnalisiLevelDB.GetDettaglioDaEmettere.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function GetDettaglioIncassato(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sPeriodo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String) As DataView
        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'valorizzo il commandText 
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "sp_AnalisiDettaglioIncassato"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURE", SqlDbType.NVarChar)).Value = sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERIODO", SqlDbType.NVarChar)).Value = sPeriodo
            If sAccreditoDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.NVarChar)).Value = sAccreditoDal
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.NVarChar)).Value = sAccreditoAl
            End If
            Log.Debug("ClsAnalisiLevelDB::GetDettaglioIncassato::sql::" & cmdMyCommand.CommandText & "::ENTE::" & sMyIdEnte & "::sAnno::" & sAnno & "::sPeriodo::" & sPeriodo & "::sAccreditoDal::" & sAccreditoDal & "::sAccreditoAl::" & sAccreditoAl)
            'eseguo la query
            DvDati = iDB.GetDataView(cmdMyCommand)
            Return DvDati
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAnalisiLevelDB.GetDettaglioIncassato.errore: ", Err)
            Return Nothing
        End Try
    End Function

    'Public Function GetRiepilogoEmesso(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sPeriodo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String, ByVal WFSession As OPENUtility.CreateSessione) As DataView
    '	Try
    '		'Valorizzo la connessione
    '		cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '		'valorizzo il commandText 
    '		cmdMyCommand.CommandType = CommandType.StoredProcedure
    '		cmdMyCommand.CommandText = "sp_AnalisiRiepilogoEmesso"
    '		'valorizzo i parameters
    '		cmdMyCommand.Parameters.Clear()
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURE", SqlDbType.NVarChar)).Value = sAnno
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERIODO", SqlDbType.NVarChar)).Value = sPeriodo
    '		If sAccreditoDal <> "" Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.NVarChar)).Value = sAccreditoDal
    '		End If
    '		If sAccreditoAl <> "" Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.NVarChar)).Value = sAccreditoAl
    '		End If
    '		Log.Debug("ClsAnalisiLevelDB::GetRiepilogoEmesso::sql::" & cmdMyCommand.CommandText & "::ENTE::" & sMyIdEnte & "::sAnno::" & sAnno & "::sPeriodo::" & sPeriodo & "::sAccreditoDal::" & sAccreditoDal & "::sAccreditoAl::" & sAccreditoAl)
    '		'eseguo la query
    '		DvDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '		Return DvDati
    '	Catch Err As Exception
    '		Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAnalisiLevelDB.GetRiepilogoEmesso.errore: ", Err)
    '		Return Nothing
    '	End Try
    'End Function

    'Public Function GetRiepilogoDaEmettere(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sPeriodo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String, ByVal WFSession As OPENUtility.CreateSessione) As DataView
    '	Try
    '		'Valorizzo la connessione
    '		cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '		'valorizzo il commandText 
    '		cmdMyCommand.CommandType = CommandType.StoredProcedure
    '		cmdMyCommand.CommandText = "sp_AnalisiRiepilogodaEmettere"
    '		'valorizzo i parameters
    '		cmdMyCommand.Parameters.Clear()
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURE", SqlDbType.NVarChar)).Value = sAnno
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERIODO", SqlDbType.NVarChar)).Value = sPeriodo
    '		If sAccreditoDal <> "" Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.NVarChar)).Value = sAccreditoDal
    '		End If
    '		If sAccreditoAl <> "" Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.NVarChar)).Value = sAccreditoAl
    '		End If
    '		Log.Debug("ClsAnalisiLevelDB::GetRiepilogoDaEmettere::sql::" & cmdMyCommand.CommandText & "::ENTE::" & sMyIdEnte & "::sAnno::" & sAnno & "::sPeriodo::" & sPeriodo & "::sAccreditoDal::" & sAccreditoDal & "::sAccreditoAl::" & sAccreditoAl)
    '		'eseguo la query
    '		DvDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '		Return DvDati
    '	Catch Err As Exception
    '		Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAnalisiLevelDB.GetRiepilogoDaEmettere.errore: ", Err)
    '		Return Nothing
    '	End Try
    'End Function

    'Public Function GetRiepilogoEmessoEvaso(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sPeriodo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String, ByVal IsEvaseTotalmente As Integer, ByVal WFSession As OPENUtility.CreateSessione) As DataView
    '	Try
    '		'Valorizzo la connessione
    '		cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '		'valorizzo il commandText 
    '		cmdMyCommand.CommandType = CommandType.StoredProcedure
    '		cmdMyCommand.CommandText = "sp_AnalisiRiepilogoEmessoEvaso"
    '		'valorizzo i parameters
    '		cmdMyCommand.Parameters.Clear()
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURE", SqlDbType.NVarChar)).Value = sAnno
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERIODO", SqlDbType.NVarChar)).Value = sPeriodo
    '		If sAccreditoDal <> "" Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.NVarChar)).Value = sAccreditoDal
    '		End If
    '		If sAccreditoAl <> "" Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.NVarChar)).Value = sAccreditoAl
    '		End If
    '		If IsEvaseTotalmente = 1 Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@EVASETOT", SqlDbType.NVarChar)).Value = 1
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@EVASEPARZ", SqlDbType.NVarChar)).Value = 0
    '		Else
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@EVASETOT", SqlDbType.NVarChar)).Value = 0
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@EVASEPARZ", SqlDbType.NVarChar)).Value = 1
    '		End If
    '		Log.Debug("ClsAnalisiLevelDB::GetRiepilogoEmessoEvaso::sql::" & cmdMyCommand.CommandText & "::ENTE::" & sMyIdEnte & "::sAnno::" & sAnno & "::sPeriodo::" & sPeriodo & "::sAccreditoDal::" & sAccreditoDal & "::sAccreditoAl::" & sAccreditoAl & "::IsEvaseTotalmente::" & IsEvaseTotalmente)
    '		'eseguo la query
    '		DvDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '		Return DvDati
    '	Catch Err As Exception
    '		Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAnalisiLevelDB.GetRiepilogoEmessoEvaso.errore: ", Err)
    '		Return Nothing
    '	End Try
    'End Function

    'Public Function GetDettaglioEmesso(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sPeriodo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String, ByVal WFSession As OPENUtility.CreateSessione) As DataView
    '	Try
    '		'Valorizzo la connessione
    '		cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '		'valorizzo il commandText 
    '		cmdMyCommand.CommandType = CommandType.StoredProcedure
    '		cmdMyCommand.CommandText = "sp_AnalisiDettaglioEmesso"
    '		'valorizzo i parameters
    '		cmdMyCommand.Parameters.Clear()
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURE", SqlDbType.NVarChar)).Value = sAnno
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERIODO", SqlDbType.NVarChar)).Value = sPeriodo
    '		If sAccreditoDal <> "" Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.NVarChar)).Value = sAccreditoDal
    '		End If
    '		If sAccreditoAl <> "" Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.NVarChar)).Value = sAccreditoAl
    '		End If
    '		Log.Debug("ClsAnalisiLevelDB::GetDettaglioEmesso::sql::" & cmdMyCommand.CommandText & "::ENTE::" & sMyIdEnte & "::sAnno::" & sAnno & "::sPeriodo::" & sPeriodo & "::sAccreditoDal::" & sAccreditoDal & "::sAccreditoAl::" & sAccreditoAl)
    '		'eseguo la query
    '		DvDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '		Return DvDati
    '	Catch Err As Exception
    '		Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAnalisiLevelDB.GetRiepilogoEmessoEvaso.errore: ", Err)
    '		Return Nothing
    '	End Try
    'End Function

    'Public Function GetDettaglioDaEmettere(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sPeriodo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String, ByVal WFSession As OPENUtility.CreateSessione) As DataView
    '	Try
    '		'Valorizzo la connessione
    '		cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '		'valorizzo il commandText 
    '		cmdMyCommand.CommandType = CommandType.StoredProcedure
    '		cmdMyCommand.CommandText = "sp_AnalisiDettaglioDaEmettere"
    '		'valorizzo i parameters
    '		cmdMyCommand.Parameters.Clear()
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURE", SqlDbType.NVarChar)).Value = sAnno
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERIODO", SqlDbType.NVarChar)).Value = sPeriodo
    '		If sAccreditoDal <> "" Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.NVarChar)).Value = sAccreditoDal
    '		End If
    '		If sAccreditoAl <> "" Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.NVarChar)).Value = sAccreditoAl
    '		End If
    '		Log.Debug("ClsAnalisiLevelDB::GetDettaglioDaEmettere::sql::" & cmdMyCommand.CommandText & "::ENTE::" & sMyIdEnte & "::sAnno::" & sAnno & "::sPeriodo::" & sPeriodo & "::sAccreditoDal::" & sAccreditoDal & "::sAccreditoAl::" & sAccreditoAl)
    '		'eseguo la query
    '		DvDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '		Return DvDati
    '	Catch Err As Exception
    '		Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAnalisiLevelDB.GetRiepilogoEmessoEvaso.errore: ", Err)
    '		Return Nothing
    '	End Try
    'End Function

    'Public Function GetDettaglioIncassato(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sPeriodo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String, ByVal WFSession As OPENUtility.CreateSessione) As DataView
    '	Try
    '		'Valorizzo la connessione
    '		cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '		'valorizzo il commandText 
    '		cmdMyCommand.CommandType = CommandType.StoredProcedure
    '		cmdMyCommand.CommandText = "sp_AnalisiDettaglioIncassato"
    '		'valorizzo i parameters
    '		cmdMyCommand.Parameters.Clear()
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURE", SqlDbType.NVarChar)).Value = sAnno
    '		cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERIODO", SqlDbType.NVarChar)).Value = sPeriodo
    '		If sAccreditoDal <> "" Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.NVarChar)).Value = sAccreditoDal
    '		End If
    '		If sAccreditoAl <> "" Then
    '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.NVarChar)).Value = sAccreditoAl
    '		End If
    '		Log.Debug("ClsAnalisiLevelDB::GetDettaglioIncassato::sql::" & cmdMyCommand.CommandText & "::ENTE::" & sMyIdEnte & "::sAnno::" & sAnno & "::sPeriodo::" & sPeriodo & "::sAccreditoDal::" & sAccreditoDal & "::sAccreditoAl::" & sAccreditoAl)
    '		'eseguo la query
    '		DvDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '		Return DvDati
    '	Catch Err As Exception
    '		Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAnalisiLevelDB.GetDettaglioIncassato.errore: ", Err)
    '		Return Nothing
    '	End Try
    'End Function
End Class
