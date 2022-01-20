Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
'Imports OPENUtility
Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility

'*******************************************************
'
' GetList Class
'
' Incapusula la Logica necessaria per estrarre i contatori
' che rispondono ai parametri di ricerca 
' nel DataBase OpenUtenze
'   
'*******************************************************
Public Class objDBListSQL
    Public oConn As SqlConnection
    Public oComm As SqlCommand
    Public lngRecordCount As Integer

    Public Query As String
    Public QueryCount As String
    Public RecordCount As Long
    Public TableName As String
End Class

'*******************************************************
'
' GetContatore Class
'
' Incapusula la Logica necessaria per determinare il dettaglio del Contatore
' dal DataBase OpenUtenze
'   
'*******************************************************

'Public Class DetailsContatore

'  Public dsTipoContatore As DataSet
'  Public dsCodiceImpianto As DataSet
'  Public dsPosizioneContatore As DataSet
'  Public dsCodFognatura As DataSet
'  Public dsCodDepurazione As DataSet
'  Public dsGiro As DataSet
'  Public dsTipoUtenza As DataSet
'  Public dsTipoAttivita As DataSet
'  Public dsDiametroContatore As DataSet
'  Public dsDiametroPresa As DataSet
'  Public drStrade As new dataview
'  Public drCodiceImpianto As new dataview
'  Public drGiro As new dataview
'  Public drPosizioneContatore As new dataview
'  Public drFognatura As new dataview
'  Public drDepurazione As new dataview
'  Public drDiametroContatore As new dataview
'  Public drDiametroPresa As new dataview
'  Public drIVA As new dataview
'    Public blnLasciatoAvviso As Boolean
'    Public lngTipoContatore As Long
'  Public lngCodImpianto As Long
'  Public lngPosizioneContatore As Long
'  Public .nCodfognatura As Long
'  Public lngCodDepurazione As Long
'  Public lngIdGiro As Long
'  Public lngCodAnaGrafeContatore As Long
'  Public CodContibuenteIntestatario As Long
'  Public CodContibuenteUtente As Long
'  Public lngTipoUtenza As Long
'  Public lngIdTipoAttivita As Long
'    Public lngDiametroPresa As Long
'  Public lngDiametroContatore As Long
'  Public lngCodStrada As Long
'  Public lngIDIVA As Long
'  Public lngIDMinimo As Long
'  Public StatoContatore As String
'    Public AssoggettamentoPenalita As String
'    '=======================
'    'ALE CAO
'    '=======================
'    Public piano As String
'    Public foglio As String
'    Public numero As String
'    Public subalterno As Int16
'    Public spesa As Double
'    Public diritti As Double
'    Public pendente As Int16
'    Public IDSubAssociato As Integer
'    Public sMatricolaSubAssociato As String
'    Public proprietario As Int16
'    '=======================
'    'FINE ALE CAO
'    '=======================
'    Public Matricola As String
'    Public Sequenza As String
'  Public PosizioneProgressiva As String
'  Public LatoStrada As String
'  Public NumeroUtente As String
'    Public NumeroUtenze As Long
'  Public ContatorePrecedente As String
'  Public ContatoreSuccessivo As String
'  Public Note As String
'  Public Ubicazione As String
'  Public Civico As String
'  Public ESPONENTE_CIVICO As String
'  Public CifreContatore As String
'  Public QuoteAgevolate As String
'  Public CodiceFabbricante As String
'  Public Impianto As String
'    Public EsenteFognatura As Boolean
'    Public EsenteDepurazione As Boolean
'    Public EsenteAcqua As Boolean
'  Public IgnoraMora As Boolean
'  Public UtenteSospeso As Boolean
'    Public DataAttivazione As String
'  Public DataSostituzione As String
'  Public DataRimTemp As String
'  Public DataCessazione As String
'    Public DataSospensione As String
'    ''''//Dati Contratto''''''''''''''''''''''''
'  Public lngIdContratto As Long
'  Public lngTipoUtenzaContratto As Long
'  Public lngDiametroPresaContratto As Long
'  Public lngDiametroContatoreContratto As Long
'  Public CodiceContratto As String
'    Public NumeroUtenzeContratto As Long
'  Public ConsumoMinimo As String
'    Public DataSottoscrizione As String
'    'Public nQualificaTitolareUtenza As Integer
'    ''''''''''''''''''''''''''''''''''''''''''''''
'    'Dati agenzia entrate
'    Public nIdTitoloOccupazione As Integer = -1
'    Public sTipoUnita As String = ""
'    Public nIdAssenzaDatiCatastali As Integer = -1
'    Public nIdTipoUtenza As Integer = -1
'    '/ dati agenzia entrate
'End Class

'*******************************************************
'
' GestContatori Class
'
' Incapusula la Logica necessaria a gestire i Contatori
' nel DataBase OpenUtenze
'   
'*******************************************************
Public Class GestContatori
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestContatori))
    Private cmdMyCommand As New SqlClient.SqlCommand
    Private FncGen As New ClsGenerale.Generale
    Private iDB As New DBAccess.getDBobject
    Dim _Const As New Costanti
    Dim oReplace As New ClsGenerale.Generale
    Dim RaiseError As New GestioneFile
    Private NomeDBAnagrafe As String = ConfigurationManager.AppSettings("NOME_DATABASE_ANAGRAFICA") & ".dbo"
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//
    Enum DBOperation
        DB_INSERT = 1
        DB_UPDATE = 0
        DB_DELETE = 2
    End Enum
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//

    Public Function getListTipoContatore() As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetTipoContatore")
                dvMyDati = ctx.GetDataView(sSQL, "TBL")
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.getListTipoContatore.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Function getListCodiceImpianto() As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetImpianto")
                dvMyDati = ctx.GetDataView(sSQL, "TBL")
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.getListCodiceImpianto.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Function getListPosizioneContatore() As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetPosizioneContatore")
                dvMyDati = ctx.GetDataView(sSQL, "TBL")
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.getListPosizioneContatore.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Function getListCodFognatura() As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetFognatura")
                dvMyDati = ctx.GetDataView(sSQL, "TBL")
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.getLisCodFognatura.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Function getListCodDepurazione() As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetDepurazione")
                dvMyDati = ctx.GetDataView(sSQL, "TBL")
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.getLisCodDepurazione.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Function getListGiro(ByVal sIdEnte As String) As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetGiri", "IDENTE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte))
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.getListGiro.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Function getListTipoUtenza(ByVal sIdEnte As String, ByVal data As String, ByVal IdUtenza As Integer) As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetTipoUtenza", "IDENTE", "IDUTENZA", "ANNO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte), ctx.GetParam("IDUTENZA", IdUtenza), ctx.GetParam("ANNO", StringOperation.FormatDateTime(data).Year))
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.getListTipoUtenza.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Function getListDiametroContatore(ByVal sIdIstat As String) As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetDiametroContatore", "IDENTE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdIstat))
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.getListDiametroContatore.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Function getListDiametroPresa() As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetDiametroPresa")
                dvMyDati = ctx.GetDataView(sSQL, "TBL")
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.getListDiametroPresa.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Function GetListaSubContatori(ByVal sIdEnte As String, ByVal IdContatorePrinc As Integer, ByVal sMatricola As String, ByVal sNumUtente As String, ByVal sUbicazione As String, ByVal sNominativoIntestatario As String, ByVal sNominativoUtente As String) As DataView
        Dim sSQL As String
        Dim dvMyDati As DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetSubContatori", "IDENTE", "IDCONTATORE", "MATRICOLA", "NUMEROUTENTE", "UBICAZIONE", "INTESTATARIO", "UTENTE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                        , ctx.GetParam("IDCONTATORE", IdContatorePrinc) _
                        , ctx.GetParam("MATRICOLA", sMatricola) _
                        , ctx.GetParam("NUMEROUTENTE", sNumUtente) _
                        , ctx.GetParam("UBICAZIONE", sUbicazione) _
                        , ctx.GetParam("INTESTATARIO", sNominativoIntestatario) _
                        , ctx.GetParam("UTENTE", sNominativoUtente)
                    )
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.getListSubContatori.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function getListTipoContatore() As new dataview
    '    Try
    '        Return iDB.getdataview("SELECT  * FROM TP_TIPOCONTATORE ORDER BY IDTIPOCONTATORE")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getListTipoContatore.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function getListCodiceImpianto() As new dataview
    '    Try
    '        Return iDB.getdataview("SELECT IDIMPIANTO, DESCRIZIONE FROM TP_IMPIANTO ORDER BY IDIMPIANTO")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getListCodiceImpianto.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function getListPosizioneContatore() As new dataview
    '    Try
    '        Return iDB.getdataview("SELECT  CODPOSIZIONE, DESCRIZIONE FROM TP_POSIZIONECONTATORE ORDER BY CODPOSIZIONE")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getListPosizioneContatore.errore: ", ex)

    '        Return Nothing
    '    End Try
    'End Function
    'Public Function getListCodFognatura() As new dataview
    '    Try
    '        Return iDB.getdataview("SELECT  * FROM TP_FOGNATURA ORDER BY CODFOGNATURA")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getLisCodFognatura.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function getListCodDepurazione() As new dataview
    '    Try
    '        Return iDB.getdataview("SELECT  * FROM TP_DEPURAZIONE ORDER BY CODDEPURAZIONE")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getLisCodDepurazione.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function getListGiro(ByVal sIdEnte As String) As new dataview
    '    Try
    '        Return iDB.getdataview("SELECT * FROM TP_GIRI WHERE CODENTE='" & sIdEnte & "'")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getListGiro.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function getListTipoUtenza(ByVal sIdEnte As String, ByVal data As String, ByVal IdUtenza As Integer) As new dataview
    '    Dim sSQL As String

    '    Try
    '        If IdUtenza > 0 Then
    '            sSQL = "SELECT * "
    '            sSQL += " FROM TP_TIPIUTENZA "
    '            sSQL += " WHERE COD_ENTE='" & sIdEnte & "'"
    '            sSQL += " AND ((CASE WHEN YEAR(AL) IS NULL THEN '9999' ELSE YEAR(AL) END)>="
    '            sSQL += "       (SELECT CASE WHEN YEAR(AL) IS NULL THEN '9999' ELSE YEAR(AL) END FROM TP_TIPIUTENZA WHERE IDTIPOUTENZA=" & IdUtenza & "))"
    '            sSQL += " AND (YEAR(DAL)<="
    '            sSQL += "     (SELECT YEAR(DAL) FROM TP_TIPIUTENZA WHERE IDTIPOUTENZA=" & IdUtenza & "))"
    '        Else
    '            If (data = "") Then
    '                data = Now.ToString
    '            End If
    '            sSQL = "SELECT * "
    '            sSQL += " FROM TP_TIPIUTENZA "
    '            sSQL += " WHERE COD_ENTE='" & sIdEnte & "'"
    '            sSQL += " AND ((CASE WHEN YEAR(AL) IS NULL THEN '9999' ELSE YEAR(AL) END)>=" & Year(data) & ") AND (YEAR(DAL)<=" & Year(data) & ")"
    '        End If
    '        Return iDB.getdataview(sSQL)
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getListTipoUtenza.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function getListTipoAttivita() As new dataview
    '    Try
    '        Return iDB.getdataview("SELECT  * FROM TP_TIPOATTIVITA ORDER BY IDTIPOATTIVITA")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getListTipoAttivita.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function getListDiametroContatore(ByVal sIdIstat As String) As new dataview
    '    Try
    '        Return iDB.getdataview("SELECT CODDIAMETROCONTATORE, DESCRIZIONE FROM TP_DIAMETROCONTATORE WHERE CODICE_ISTAT='" & sIdIstat & "' ORDER BY CODDIAMETROCONTATORE")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getListDiametroContatore.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function getListDiametroPresa() As new dataview
    '    Try
    '        Return iDB.getdataview("SELECT CODDIAMETROPRESA, DESCRIZIONE FROM TP_DIAMETROPRESA ORDER BY CODDIAMETROPRESA")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getListDiametroPresa.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function getListStrade(ByVal sIdEnte As String) As new dataview
    '    Try
    '        Return iDB.getdataview("SELECT COD_STRADA,TIPO_STRADA,STRADA  FROM STRADARIO WHERE CODICE_ISTAT ='" & sIdEnte & "'")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getListStrade.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function getListIVA() As new dataview
    '    Try
    '        Return iDB.getdataview("SELECT CODIVA,DESCRIZIONE  FROM TP_IVA")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getListIVA.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function GetListaSubContatori(ByVal sIdEnte As String, ByVal IdContatorePrinc As Integer, ByVal sMatricola As String, ByVal sNumUtente As String, ByVal sUbicazione As String, ByVal sNominativoIntestatario As String, ByVal sNominativoUtente As String) As DataView
    '    Dim sSQL As String
    '    Dim dvMyDati As DataView

    '    Try
    '        sSQL = "SELECT DISTINCT CODCONTATORE, COGNOME_INT,  NOME_INT, MATRICOLA, VIA_UBICAZIONE, CIVICO_UBICAZIONE"
    '        sSQL += " FROM OPENgov_ELENCO_CONTATORI"
    '        sSQL += " WHERE (1 = 1)"
    '        sSQL += " AND (DATAATTIVAZIONE<>'')"
    '        sSQL += " AND (DATACESSAZIONE IS NULL OR DATACESSAZIONE='')"
    '        sSQL += " AND (CODCONTATORESUB IS NULL)"
    '        sSQL += " AND (CODENTE='" & sIdEnte & "')"
    '        sSQL += " AND (CODCONTATORE<>" & IdContatorePrinc & ")"
    '        If sMatricola <> "" Then
    '            sSQL += " AND (MATRICOLA ='" & sMatricola & "')"
    '        End If
    '        If sNumUtente <> "" Then
    '            sSQL += " AND (NUMEROUTENTE='" & sNumUtente & "')"
    '        End If
    '        If sUbicazione <> "" Then
    '            sSQL += " AND (VIA_UBICAZIONE LIKE'" & sUbicazione & "%')"
    '        End If
    '        If sNominativoIntestatario <> "" Then
    '            sSQL += " AND (COGNOME_INT+' '+NOME_INT LIKE'" & sNominativoIntestatario & "%')"
    '        End If
    '        If sNominativoUtente <> "" Then
    '            sSQL += " AND (COGNOME_UT+' '+NOME_UT LIKE'" & sNominativoUtente & "%')"
    '        End If
    '        sSQL += " ORDER BY COGNOME_INT,  NOME_INT, MATRICOLA, VIA_UBICAZIONE, CIVICO_UBICAZIONE"
    '        dvMyDati = iDB.GetDataView(sSQL)

    '        Return dvMyDati
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.getListSubContatori.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    Public Function GetListaCatastaliMatrice(ByVal matrice As ArrayList) As ArrayList
        Dim getList As New objDBListSQL
        getList.RecordCount = matrice.Count

        Return matrice
    End Function

    Public Function GetIdTipoParticella(ByVal descTipoParticella As String) As String
        Dim idTipoParticella As String = ""
        Try

            If descTipoParticella.IndexOf("-") > 0 Then
                Dim arrayTipoParticella() As String
                arrayTipoParticella = descTipoParticella.Split("-")

                idTipoParticella = arrayTipoParticella(0).Trim()

            Else
                idTipoParticella = descTipoParticella.Trim()
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetIdTipoParticella.errore: ", ex)
        End Try
        Return idTipoParticella
    End Function

    Public Sub GetTipoParticella(ByVal dsCatasto As DataTable)
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            If dsCatasto.Rows.Count > 0 Then
                If Not IsDBNull(dsCatasto.Rows(0)("ID_TIPO_PARTICELLA")) Then
                    Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetAETipoParticella")
                        dvMyDati = ctx.GetDataView(sSQL, "TBL")
                        ctx.Dispose()
                    End Using
                    For Each myRowCat As DataRow In dsCatasto.Rows
                        For Each myRow As DataRowView In dvMyDati
                            myRowCat("ID_TIPO_PARTICELLA") = Utility.StringOperation.FormatString(myRow("descrizione"))
                        Next
                    Next
                End If
            End If

            HttpContext.Current.Session("datacatasto") = dsCatasto
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetTipoParticella.errore: ", ex)
        End Try

    End Sub

    Public Function GetTableContatoriCessati(ByVal sIdEnte As String, ByVal nIdPeriodo As Integer, ByVal sIntestatario As String, ByVal sUtente As String, ByVal sVia As String, ByVal sNumeroUtente As String, ByVal sMatricola As String, ByVal nIdGiro As Integer, ByVal bSub As Boolean) As DataView
        Dim dvMyDati As DataView
        Dim clsPeriodo As New TabelleDiDecodifica.DBPeriodo
        Dim oPeriodo As TabelleDiDecodifica.DetailPeriodo

        Try
            'determino i dati del periodo
            oPeriodo = clsPeriodo.GetPeriodo(nIdPeriodo)

            cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Parameters.Clear()
            'valorizzo il CommandText;
            cmdMyCommand.CommandText = " SELECT * "
            cmdMyCommand.CommandText += " FROM OPENGOV_STAMPACONTATORILETTURE"
            cmdMyCommand.CommandText += " WHERE ((DATACESSAZIONE IS NOT NULL"
            cmdMyCommand.CommandText += " AND DATACESSAZIONE <> ''"
            cmdMyCommand.CommandText += " AND DATACESSAZIONE >=@DATADA)"
            cmdMyCommand.CommandText += " OR (DATASOSPENSIONEUTENZA IS NOT NULL "
            cmdMyCommand.CommandText += " AND DATASOSPENSIONEUTENZA <> '' "
            cmdMyCommand.CommandText += " AND DATACESSAZIONE >=@DATADA) "
            cmdMyCommand.CommandText += " OR (DATASOSTITUZIONE IS NOT NULL "
            cmdMyCommand.CommandText += " AND DATASOSTITUZIONE <> '' "
            cmdMyCommand.CommandText += " AND DATACESSAZIONE >=@DATADA))"
            cmdMyCommand.CommandText += " AND (CODENTE=@IDENTE )"
            cmdMyCommand.CommandText += " AND (CODPERIODO=@IDPERIODO)"
            If sIntestatario <> "" Then
                cmdMyCommand.CommandText += " AND (COGNOME_INT+' '+NOME_INT LIKE @INTESTATARIO)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTESTATARIO", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sIntestatario) & "%"
            End If
            If sUtente <> "" Then
                cmdMyCommand.CommandText += " AND (COGNOME_UT+' '+NOME_UT LIKE @UTENTE)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@UTENTE", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sUtente) & "%"
            End If
            If sVia <> "" Then
                cmdMyCommand.CommandText += " AND (VIA_UBICAZIONE=@VIA)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.NVarChar)).Value = sVia
            End If
            If sNumeroUtente <> "" Then
                cmdMyCommand.CommandText += " AND (NUMEROUTENTE=@NUTENTE)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUTENTE", SqlDbType.NVarChar)).Value = sNumeroUtente
            End If
            If sMatricola <> "" Then
                cmdMyCommand.CommandText += " AND (MATRICOLA=@MATRICOLA)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MATRICOLA", SqlDbType.NVarChar)).Value = sMatricola
            End If
            If nIdGiro > 0 Then
                cmdMyCommand.CommandText += " AND (CODGIRO=@IDGIRO)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDGIRO", SqlDbType.Int)).Value = nIdGiro
            End If
            If bSub = True Then
                cmdMyCommand.CommandText += " AND (NOT CODCONTATORESUB IS NULL)"
            End If
            cmdMyCommand.CommandText += " ORDER BY COGNOME_INT,NOME_INT"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPERIODO", SqlDbType.Int)).Value = nIdPeriodo
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADA", SqlDbType.NVarChar)).Value = oReplace.GiraData(oPeriodo.DaData)
            dvMyDati = iDB.GetDataView(cmdMyCommand)
            Return dvMyDati
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetTableContatoriCessati.errore: ", Err)
            Log.Debug("Si è verificato un errore in GestContatori::GetTableContatoriCessati::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetTableContatoriAttivi(ByVal mioGiro As Integer, ByVal miaUbicazione As Integer, ByVal sCodEnte As String, ByVal sPeriodo As String) As DataView
        Dim dvMyDati As DataView

        Try
            cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Parameters.Clear()
            'valorizzo il CommandText;
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM OPENGOV_STAMPACONTATORILETTURE"
            cmdMyCommand.CommandText += " WHERE (CODENTE=@IDENTE)"
            cmdMyCommand.CommandText += " AND YEAR(CASE WHEN ISDATE(DATAATTIVAZIONE)=0 THEN '99991231' ELSE DATAATTIVAZIONE END)<>9999"
            cmdMyCommand.CommandText += " AND YEAR(CASE WHEN ISDATE(DATACESSAZIONE)=0 THEN '99991231' ELSE DATACESSAZIONE END)=9999"
            If sPeriodo <> "" Then
                cmdMyCommand.CommandText += " AND (CODPERIODO=@CODPERIODO)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODPERIODO", SqlDbType.Int)).Value = sPeriodo
            End If
            If mioGiro <> -1 Then
                cmdMyCommand.CommandText += " AND (IDGIRO=@IDGIRO)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDGIRO", SqlDbType.Int)).Value = mioGiro
            End If
            If miaUbicazione <> -1 Then
                cmdMyCommand.CommandText += " AND (COD_STRADA=@IDVIA)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVIA", SqlDbType.Int)).Value = miaUbicazione
            End If
            cmdMyCommand.CommandText += " ORDER BY COGNOME_INT,NOME_INT"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sCodEnte
            dvMyDati = iDB.GetDataView(cmdMyCommand)
            Return dvMyDati
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetTabelContatoriAttivi.errore: ", Err)
            Log.Debug("Si è verificato un errore in GestContatori::GetTableContatoriAttivi::" & Err.Message)
            Return Nothing
        End Try
    End Function
    'Public Function GetTableContatoriAttivi(ByVal mioGiro As Integer, ByVal miaUbicazione As Integer, ByVal sCodEnte As String, ByVal sPeriodo As String) As DataView
    '    Dim dvMyDati As DataView

    '    Try
    '        cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Parameters.Clear()
    '        'valorizzo il CommandText;
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM OPENGOV_STAMPACONTATORILETTURE"
    '        cmdMyCommand.CommandText += " WHERE (CODENTE=@IDENTE)"
    '        cmdMyCommand.CommandText += " AND (DATAATTIVAZIONE IS NOT NULL AND DATAATTIVAZIONE<>'')"
    '        cmdMyCommand.CommandText += " AND (DATACESSAZIONE IS NULL OR DATACESSAZIONE='')"
    '        If sPeriodo <> "" Then
    '            cmdMyCommand.CommandText += " AND (CODPERIODO=@CODPERIODO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODPERIODO", SqlDbType.Int)).Value = sPeriodo
    '        End If
    '        If mioGiro <> -1 Then
    '            cmdMyCommand.CommandText += " AND (IDGIRO=@IDGIRO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDGIRO", SqlDbType.Int)).Value = mioGiro
    '        End If
    '        If miaUbicazione <> -1 Then
    '            cmdMyCommand.CommandText += " AND (COD_STRADA=@IDVIA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVIA", SqlDbType.Int)).Value = miaUbicazione
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY COGNOME_INT,NOME_INT"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sCodEnte
    '        dvMyDati = iDB.GetDataView(cmdMyCommand)
    '        Return dvMyDati
    '    Catch Err As Exception
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.GetTabelContatoriAttivi.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in GestContatori::GetTableContatoriAttivi::" & Err.Message)
    '        Return Nothing
    '    End Try
    'End Function

    Public Function GetDataTableContatoreAnater(ByVal CodContatore As Integer) As DataSet
        Dim sSQL As String
        Dim oConn As New SqlConnection

        oConn.ConnectionString = ConstSession.StringConnection

        sSQL = "SELECT * FROM TP_CONTATORI WHERE CODCONTATORE=" & CodContatore

        Dim ds As DataSet

        ds = iDB.RunSQLReturnDataSet(sSQL)
        'Dim dt As New DataTable

        GetDataTableContatoreAnater = ds    '.Tables(0)
        Return GetDataTableContatoreAnater

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="sMatricola"></param>
    ''' <param name="sNumeroUtente"></param>
    ''' <param name="sUbicazione"></param>
    ''' <param name="sCognomeInt"></param>
    ''' <param name="sNomeInt"></param>
    ''' <param name="sCognomeUt"></param>
    ''' <param name="sNomeUt"></param>
    ''' <param name="nStatoContratto"></param>
    ''' <param name="nStatoContatore"></param>
    ''' <param name="bSubContatore"></param>
    ''' <param name="IdContatore"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetElencoContrattiContatori(myStringConnection As String, ByVal sIdEnte As String, ByVal sMatricola As String, ByVal sNumeroUtente As String, ByVal sUbicazione As String, ByVal sCognomeInt As String, ByVal sNomeInt As String, ByVal sCognomeUt As String, ByVal sNomeUt As String, ByVal nStatoContratto As Integer, ByVal nStatoContatore As Integer, ByVal bSubContatore As Boolean, IdContatore As Integer) As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetContatori", "AMBIENTE", "IDENTE", "MATRICOLA", "NUMEROUTENTE", "UBICAZIONE", "COGNOMEINT", "NOMEINT", "COGNOMEUT", "NOMEUT", "STATOCONTRATTO", "SUBCONTATORE", "IDCONTATORE", "STATOCONTATORE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("AMBIENTE", ConstSession.Ambiente) _
                    , ctx.GetParam("IDENTE", sIdEnte) _
                    , ctx.GetParam("MATRICOLA", oReplace.ReplaceCharsForSearch(sMatricola) & "%") _
                    , ctx.GetParam("NUMEROUTENTE", sNumeroUtente) _
                    , ctx.GetParam("UBICAZIONE", oReplace.ReplaceCharsForSearch(sUbicazione) & "%") _
                    , ctx.GetParam("COGNOMEINT", oReplace.ReplaceCharsForSearch(sCognomeInt) & "%") _
                    , ctx.GetParam("NOMEINT", oReplace.ReplaceCharsForSearch(sNomeInt) & "%") _
                    , ctx.GetParam("COGNOMEUT", oReplace.ReplaceCharsForSearch(sCognomeUt) & "%") _
                    , ctx.GetParam("NOMEUT", oReplace.ReplaceCharsForSearch(sNomeUt) & "%") _
                    , ctx.GetParam("STATOCONTRATTO", nStatoContratto) _
                    , ctx.GetParam("SUBCONTATORE", If(bSubContatore = True, 1, 0)) _
                    , ctx.GetParam("IDCONTATORE", IdContatore) _
                    , ctx.GetParam("STATOCONTATORE", nStatoContatore)
                )
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch Err As Exception
            Log.Debug(sIdEnte + " - OPENgovH2O.GestContatori.GetElencoContrattiContatori.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Ritorna i dati necessari per poplare il DataGrid
    ''' </summary>
    ''' <param name="strMatricola"></param>
    ''' <param name="boolSub"></param>
    ''' <param name="strNumeroUtente"></param>
    ''' <param name="IDVia"></param>
    ''' <param name="strNominativoIntestatario"></param>
    ''' <param name="strNominativoUtente"></param>
    ''' <param name="sAmbiente"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nomeUtente"></param>
    ''' <param name="nomeIntestatario"></param>
    ''' <param name="sFoglio"></param>
    ''' <param name="sNumero"></param>
    ''' <param name="stato"></param>
    ''' <param name="nIDContatore"></param>
    ''' <param name="Contratto"></param>
    ''' <returns></returns>
    ''' <remarks>Pagina Chiamante:DataEntryContatori/SearchResultsContatori.aspx/SearchResultsContatori.aspx.vb</remarks>
    Public Function GetListaContatori(ByVal strMatricola As String, ByVal boolSub As Boolean, ByVal strNumeroUtente As String, ByVal IDVia As Integer, ByVal strNominativoIntestatario As String, ByVal strNominativoUtente As String, sAmbiente As String, ByVal sIdEnte As String, ByVal nomeUtente As String, ByVal nomeIntestatario As String, ByVal sFoglio As String, ByVal sNumero As String, ByVal stato As Integer, ByVal nIDContatore As Integer, Contratto As String) As DataSet
        Dim sSQL As String = ""
        Dim dsMyDati As New DataSet

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "sp_RicercaContatori", "AMBIENTE", "IDENTE", "IDCONTATORE", "NUMEROUTENTE", "ISSUB", "IDVIA", "NOMINATIVOINTESTATARIO", "NOMEINTESTATARIO", "NOMINATIVOUTENTE", "NOMEUTENTE", "MATRICOLA", "FOGLIO", "NUMERO", "STATO", "CONTRATTO")
                dsMyDati = ctx.GetDataSet(sSQL, "RicercaContatori", ctx.GetParam("AMBIENTE", sAmbiente) _
                                    , ctx.GetParam("IDENTE", sIdEnte) _
                                    , ctx.GetParam("IDCONTATORE", nIDContatore) _
                                    , ctx.GetParam("NUMEROUTENTE", strNumeroUtente) _
                                    , ctx.GetParam("ISSUB", CInt(boolSub)) _
                                    , ctx.GetParam("IDVIA", IDVia) _
                                    , ctx.GetParam("NOMINATIVOINTESTATARIO", strNominativoIntestatario) _
                                    , ctx.GetParam("NOMEINTESTATARIO", nomeIntestatario) _
                                    , ctx.GetParam("NOMINATIVOUTENTE", strNominativoUtente) _
                                    , ctx.GetParam("NOMEUTENTE", nomeUtente) _
                                    , ctx.GetParam("MATRICOLA", strMatricola) _
                                    , ctx.GetParam("FOGLIO", sFoglio) _
                                    , ctx.GetParam("NUMERO", sNumero) _
                                    , ctx.GetParam("STATO", stato) _
                                    , ctx.GetParam("CONTRATTO", Contratto)
                                )
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetListaContatori.errore: ", ex)
        End Try
        Return dsMyDati
    End Function

    Public Function getListaCatastali(ByVal IdCatasto As Integer, ByVal IdContatore As Integer) As DataSet
        Dim ds As DataSet
        'eseguo la query
        ds = iDB.RunSPReturnDataSet("prc_GetDatiCatastali", "GetDatiCatastali", New SqlClient.SqlParameter("@Id", IdCatasto), New SqlClient.SqlParameter("@IdContatore", IdContatore))

        Return ds
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IDCatasto"></param>
    ''' <param name="IDContatore"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetDetailsCatasto(ByVal IDCatasto As Integer, ByVal IDContatore As Integer) As objDatiCatastali()
        Dim oMyList() As objDatiCatastali = Nothing
        Dim oMyRifCat As objDatiCatastali
        Dim nList As Integer = -1
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetDatiCatastali", "ID", "IDCONTATORE")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", IDCatasto), ctx.GetParam("IDCONTATORE", IDContatore))
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            oMyRifCat = New objDatiCatastali
                            oMyRifCat.sFoglio = StringOperation.FormatString(myRow("FOGLIO"))
                            oMyRifCat.sNumero = StringOperation.FormatString(myRow("NUMERO"))
                            oMyRifCat.nSubalterno = StringOperation.FormatInt(myRow("SUBALTERNO"))
                            oMyRifCat.sInterno = StringOperation.FormatString(myRow("INTERNO"))
                            oMyRifCat.sPiano = StringOperation.FormatString(myRow("PIANO"))
                            oMyRifCat.sSezione = StringOperation.FormatString(myRow("SEZIONE"))
                            oMyRifCat.sEstensioneParticella = StringOperation.FormatString(myRow("ESTENSIONE_PARTICELLA"))
                            oMyRifCat.sIdTipoParticella = StringOperation.FormatString(myRow("ID_TIPO_PARTICELLA"))
                            oMyRifCat.nIdContatore = StringOperation.FormatInt(myRow("codcontatore"))
                            oMyRifCat.nIdCatastale = StringOperation.FormatInt(myRow("idcont_catas"))
                            nList += 1
                            ReDim Preserve oMyList(nList)
                            oMyList(nList) = oMyRifCat
                        Next
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetDetailsCatasto.errore: ", ex)
                Finally
                    dvMyDati.Dispose()
                End Try
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetDetailsCatasto.errore: ", ex)
        End Try
        Return oMyList
    End Function
    'Public Function GetDetailsCatasto(ByVal IDCatasto As Integer, ByVal IDContatore As Integer) As objDatiCatastali()
    '    Dim oMyList() As objDatiCatastali
    '    Dim oMyRifCat As objDatiCatastali
    '    Dim nList As Integer = -1
    '    Dim x As Integer
    '    Dim dsDati As New DataSet

    '    Try
    '        dsDati = iDB.RunSPReturnDataSet("prc_GetDatiCatastali", "GetDatiCatastali", New SqlClient.SqlParameter("@Id", IDCatasto), New SqlClient.SqlParameter("@IdContatore", IDContatore))
    '        For x = 0 To dsDati.Tables(0).Rows.Count - 1
    '            oMyRifCat = New objDatiCatastali
    '            If Not IsDBNull(dsDati.Tables(0).Rows(x)("FOGLIO")) Then
    '                oMyRifCat.sFoglio = dsDati.Tables(0).Rows(x)("FOGLIO")
    '            End If
    '            If Not IsDBNull(dsDati.Tables(0).Rows(x)("NUMERO")) Then
    '                oMyRifCat.sNumero = dsDati.Tables(0).Rows(x)("NUMERO")
    '            End If
    '            If Not IsDBNull(dsDati.Tables(0).Rows(x)("SUBALTERNO")) Then
    '                oMyRifCat.nSubalterno = dsDati.Tables(0).Rows(x)("SUBALTERNO")
    '            End If
    '            If Not IsDBNull(dsDati.Tables(0).Rows(x)("INTERNO")) Then
    '                oMyRifCat.sInterno = dsDati.Tables(0).Rows(x)("INTERNO")
    '            End If
    '            If Not IsDBNull(dsDati.Tables(0).Rows(x)("PIANO")) Then
    '                oMyRifCat.sPiano = dsDati.Tables(0).Rows(x)("PIANO")
    '            End If
    '            If Not IsDBNull(dsDati.Tables(0).Rows(x)("SEZIONE")) Then
    '                oMyRifCat.sSezione = dsDati.Tables(0).Rows(x)("SEZIONE")
    '            End If
    '            If Not IsDBNull(dsDati.Tables(0).Rows(x)("ESTENSIONE_PARTICELLA")) Then
    '                oMyRifCat.sEstensioneParticella = dsDati.Tables(0).Rows(x)("ESTENSIONE_PARTICELLA")
    '            End If
    '            If Not IsDBNull(dsDati.Tables(0).Rows(x)("ID_TIPO_PARTICELLA")) Then
    '                oMyRifCat.sIdTipoParticella = dsDati.Tables(0).Rows(x)("ID_TIPO_PARTICELLA")
    '            End If
    '            If Not IsDBNull(dsDati.Tables(0).Rows(x)("codcontatore")) Then
    '                oMyRifCat.nIdContatore = dsDati.Tables(0).Rows(x)("codcontatore")
    '            End If
    '            If Not IsDBNull(dsDati.Tables(0).Rows(x)("idcont_catas")) Then
    '                oMyRifCat.nIdCatastale = dsDati.Tables(0).Rows(x)("idcont_catas")
    '            End If
    '            nList += 1
    '            ReDim Preserve oMyList(nList)
    '            oMyList(nList) = oMyRifCat
    '        Next
    '        Return oMyList

    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.GetDetailsCatasto.errore: ", ex)
    '        Return Nothing
    '    Finally
    '        dsDati.Dispose()
    '    End Try
    'End Function
    Public Function GetListaCatasto(ByVal sIdEnte As String, ByVal sListContrib As String) As objDatiCatastali()
        Dim oMyList As New Generic.List(Of objDatiCatastali)
        Dim oMyRifCat As New objDatiCatastali
        Dim dsDati As New DataSet

        Try
            dsDati = iDB.RunSPReturnDataSet("prc_GetDatiCatastali", "GetDatiCatastali", New SqlClient.SqlParameter("@IdEnte", sIdEnte), New SqlClient.SqlParameter("@ListContrib", sListContrib))
            For Each myRow As DataRow In dsDati.Tables(0).Rows
                oMyRifCat = New objDatiCatastali
                If Not IsDBNull(myRow("FOGLIO")) Then
                    oMyRifCat.sFoglio = myRow("FOGLIO")
                End If
                If Not IsDBNull(myRow("NUMERO")) Then
                    oMyRifCat.sNumero = myRow("NUMERO")
                End If
                If Not IsDBNull(myRow("SUBALTERNO")) Then
                    oMyRifCat.nSubalterno = myRow("SUBALTERNO")
                End If
                If Not IsDBNull(myRow("INTERNO")) Then
                    oMyRifCat.sInterno = myRow("INTERNO")
                End If
                If Not IsDBNull(myRow("PIANO")) Then
                    oMyRifCat.sPiano = myRow("PIANO")
                End If
                If Not IsDBNull(myRow("SEZIONE")) Then
                    oMyRifCat.sSezione = myRow("SEZIONE")
                End If
                If Not IsDBNull(myRow("ESTENSIONE_PARTICELLA")) Then
                    oMyRifCat.sEstensioneParticella = myRow("ESTENSIONE_PARTICELLA")
                End If
                If Not IsDBNull(myRow("ID_TIPO_PARTICELLA")) Then
                    oMyRifCat.sIdTipoParticella = myRow("ID_TIPO_PARTICELLA")
                End If
                If Not IsDBNull(myRow("codcontatore")) Then
                    oMyRifCat.nIdContatore = myRow("codcontatore")
                End If
                If Not IsDBNull(myRow("idcont_catas")) Then
                    oMyRifCat.nIdCatastale = myRow("idcont_catas")
                End If
                oMyList.Add(oMyRifCat)
            Next
            Return oMyList.ToArray

        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetListaCatasto.errore: ", ex)
            Return Nothing
        Finally
            dsDati.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' Ritorna i dati del Contatore se esiste per la MODIFICA o prepara il form per l'INSERIMENTO
    ''' </summary>
    ''' <param name="IDContatore"></param>
    ''' <param name="IDContratto"></param>
    ''' <returns></returns>
    ''' <remarks>Pagina Chiamante:DataEntryContatori/DatiGenerali.aspx/DatiGenerali.aspx.vb</remarks>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetDetailsContatori(ByVal IDContatore As Integer, IDContratto As Integer) As objContatore
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim DetailsContatore As New objContatore
        Dim lgnTipoOperazione As Long = DBOperation.DB_UPDATE

        Try
            If IDContatore = 0 Then lgnTipoOperazione = DBOperation.DB_INSERT

            If lgnTipoOperazione = DBOperation.DB_UPDATE Then
                Try
                    Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                        Try
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "DetailContatori", "CODCONTATORE", "CODCONTRATTO")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODCONTATORE", IDContatore) _
                                    , ctx.GetParam("CODCONTRATTO", IDContratto)
                                )
                            If Not dvMyDati Is Nothing Then
                                For Each myRow As DataRowView In dvMyDati
                                    DetailsContatore.nIdContatore = StringOperation.FormatInt(myRow("CODCONTATORE"))
                                    DetailsContatore.nIdContatorePrec = StringOperation.FormatInt(myRow("CODCONTATOREPRECEDENTE"))
                                    DetailsContatore.nIdContatoreSucc = StringOperation.FormatInt(myRow("CODCONTATORESUCCESSIVO"))
                                    DetailsContatore.sIdEnte = StringOperation.FormatString(myRow("codente"))
                                    DetailsContatore.sCodiceISTAT = StringOperation.FormatString(myRow("codice_istat"))
                                    DetailsContatore.nTipoContatore = StringOperation.FormatInt(myRow("IDTIPOCONTATORE"))
                                    DetailsContatore.nIdImpianto = StringOperation.FormatInt(myRow("CODIMPIANTO"))
                                    DetailsContatore.nPosizione = StringOperation.FormatInt(myRow("CODPOSIZIONE"))
                                    DetailsContatore.nCodFognatura = StringOperation.FormatInt(myRow("CODFOGNATURA"))
                                    DetailsContatore.nCodDepurazione = StringOperation.FormatInt(myRow("CODDEPURAZIONE"))
                                    DetailsContatore.nGiro = StringOperation.FormatInt(myRow("IDGIRO"))
                                    DetailsContatore.nIdContratto = StringOperation.FormatInt(myRow("CODCONTRATTO"))
                                    If DetailsContatore.nIdContratto = -1 Then
                                        DetailsContatore.nIdContratto = 0
                                    End If
                                    DetailsContatore.nTipoUtenza = StringOperation.FormatInt(myRow("IDTIPOUTENZA"))
                                    DetailsContatore.nIdAttivita = StringOperation.FormatInt(myRow("IDTIPOATTIVITA"))
                                    DetailsContatore.nDiametroContatore = StringOperation.FormatInt(myRow("CODDIAMETROCONTATORE"))
                                    DetailsContatore.nDiametroPresa = StringOperation.FormatInt(myRow("CODDIAMETROPRESA"))
                                    DetailsContatore.nIdVia = StringOperation.FormatInt(myRow("COD_STRADA"))
                                    DetailsContatore.nCodIva = StringOperation.FormatInt(myRow("CODIVA"))
                                    DetailsContatore.nConsumoMinimo = StringOperation.FormatInt(myRow("IDMINIMO"))

                                    DetailsContatore.oDatiCatastali = GetDetailsCatasto(-1, DetailsContatore.nIdContatore)
                                    DetailsContatore.oListSubContatori = GetSubContatori(DetailsContatore.nIdContatore)

                                    DetailsContatore.nSpesa = StringOperation.FormatDouble(myRow("SPESA"))
                                    DetailsContatore.nDiritti = StringOperation.FormatDouble(myRow("DIRITTI"))

                                    DetailsContatore.nProprietario = StringOperation.FormatInt(myRow("PROPRIETARIO"))

                                    DetailsContatore.sPenalita = StringOperation.FormatString(myRow("PENALITA"))
                                    DetailsContatore.sStatoContatore = StringOperation.FormatString(myRow("STATOCONTATORE"))

                                    DetailsContatore.sMatricola = StringOperation.FormatString(myRow("MATRICOLA"))
                                    DetailsContatore.sSequenza = StringOperation.FormatString(myRow("SEQUENZA"))
                                    DetailsContatore.sProgressivo = StringOperation.FormatString(myRow("POSIZIONEPROGRESSIVA"))
                                    DetailsContatore.sLatoStrada = StringOperation.FormatString(myRow("LATOSTRADA"))
                                    DetailsContatore.sNumeroUtente = StringOperation.FormatString(myRow("NUMEROUTENTE"))
                                    DetailsContatore.nNumeroUtenze = StringOperation.FormatInt(myRow("NUMEROUTENZE"))
                                    DetailsContatore.nIdContatorePrec = StringOperation.FormatInt(myRow("PRECEDENTE"))
                                    DetailsContatore.sMatricolaContatorePrec = StringOperation.FormatString(myRow("MATRICOLAPRECEDENTE"))
                                    DetailsContatore.nIdContatoreSucc = StringOperation.FormatInt(myRow("SUCCESSIVO"))
                                    DetailsContatore.sMatricolaContatoreSucc = StringOperation.FormatString(myRow("MATRICOLASUCCESSIVO"))
                                    DetailsContatore.sNote = StringOperation.FormatString(myRow("NOTE"))
                                    DetailsContatore.sCivico = StringOperation.FormatString(myRow("CIVICO_UBICAZIONE"))
                                    DetailsContatore.sEsponenteCivico = StringOperation.FormatString(myRow("ESPONENTE_CIVICO"))
                                    DetailsContatore.sUbicazione = StringOperation.FormatString(myRow("VIA_UBICAZIONE"))

                                    DetailsContatore.bEsenteFognatura = StringOperation.FormatBool(myRow("ESENTEFOGNATURA"))
                                    DetailsContatore.bEsenteDepurazione = StringOperation.FormatBool(myRow("ESENTEDEPURAZIONE"))
                                    DetailsContatore.bEsenteAcqua = StringOperation.FormatBool(myRow("ESENTEACQUA"))
                                    DetailsContatore.bIgnoraMora = StringOperation.FormatBool(myRow("IGNORAMORA"))
                                    DetailsContatore.bUtenteSospeso = StringOperation.FormatBool(myRow("UTENTESOSPESO"))

                                    DetailsContatore.sDataAttivazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("DATAATTIVAZIONE")))
                                    DetailsContatore.sDataSostituzione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("DATASOSTITUZIONE")))
                                    DetailsContatore.sDataRimTemp = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("DATARIMOZIONETEMPORANEA")))
                                    DetailsContatore.sDataCessazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("DATACESSAZIONE")))
                                    DetailsContatore.sDataSospensioneUtenza = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("DATASOSPENSIONEUTENZA")))

                                    DetailsContatore.sQuoteAgevolate = StringOperation.FormatString(myRow("QUOTEAGEVOLATE"))
                                    DetailsContatore.sCifreContatore = StringOperation.FormatString(myRow("CIFRECONTATORE"))
                                    DetailsContatore.sCodiceFabbricante = StringOperation.FormatString(myRow("CODICEFABBRICANTE"))

                                    '*** agenzia entrate
                                    DetailsContatore.nIdTitoloOccupazione = StringOperation.FormatString(myRow("ID_TITOLO_OCCUPAZIONE"))
                                    DetailsContatore.sTipoUnita = StringOperation.FormatString(myRow("ID_TIPO_UNITA"))
                                    DetailsContatore.nIdAssenzaDatiCatastali = StringOperation.FormatString(myRow("ID_ASSENZA_DATI_CATASTALI"))
                                    DetailsContatore.nIdTipoUtenza = StringOperation.FormatString(myRow("ID_TIPO_UTENZA"))
                                    '*** /agenzia entrate
                                    '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
                                    DetailsContatore.bEsenteAcquaQF = StringOperation.FormatBool(myRow("ESENTEACQUAQF"))
                                    DetailsContatore.bEsenteDepQF = StringOperation.FormatBool(myRow("ESENTEDEPURAZIONEQF"))
                                    DetailsContatore.bEsenteFogQF = StringOperation.FormatBool(myRow("ESENTEFOGNATURAQF"))
                                    '*** ***
                                    DetailsContatore.nFondoScala = New GestLetture().GetFondoScala(-1, DetailsContatore.nIdContatore)
                                Next
                            End If
                        Catch ex As Exception
                            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetDetailsContatori.DetailContatoriNEW.errore: ", ex)
                        Finally
                            dvMyDati.Dispose()
                        End Try
                        Try
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "DetailAnagraficaIntestatario", "CodContatore")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CodContatore", DetailsContatore.nIdContatore))
                            If Not dvMyDati Is Nothing Then
                                For Each myRow As DataRowView In dvMyDati
                                    DetailsContatore.nIdIntestatario = StringOperation.FormatString(myRow("COD_CONTRIBUENTE"))
                                Next
                            End If
                        Catch ex As Exception
                            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetDetailsContatori.DetailAnagraficaIntestatario.errore: ", ex)
                        Finally
                            dvMyDati.Dispose()
                        End Try
                        Try
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "DetailAnagraficaUtente", "CodContatore")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CodContatore", DetailsContatore.nIdContatore))
                            If Not dvMyDati Is Nothing Then
                                For Each myRow As DataRowView In dvMyDati
                                    DetailsContatore.nIdUtente = StringOperation.FormatString(myRow("COD_CONTRIBUENTE"))
                                Next
                            End If
                        Catch ex As Exception
                            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetDetailsContatori.DetailAnagraficaUtente.errore: ", ex)
                        Finally
                            dvMyDati.Dispose()
                        End Try
                        ctx.Dispose()
                        ctx.Dispose()
                    End Using
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetDetailsContatori.errore: ", ex)
                Finally
                    dvMyDati.Dispose()
                End Try
            End If

            Return DetailsContatore
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetDetailsContatori.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function GetDetailsContatori(ByVal IDContatore As Integer, IDContratto As Integer) As objContatore
    '    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '    'Public Function GetDetailsContatori(ByVal IDContatore As Integer, Optional ByVal CodEnte As Integer = -1, Optional ByVal codiceIstat As String = "") As objContatore
    '    Dim DetailsContatore As New objContatore
    '    Dim lgnTipoOperazione As Long = DBOperation.DB_UPDATE

    '    Try
    '        If IDContatore = 0 Then lgnTipoOperazione = DBOperation.DB_INSERT

    '        If lgnTipoOperazione = DBOperation.DB_UPDATE Then
    '            Dim drDetailsContatore As new dataview
    '            Log.Debug("GetDetailsContatori::prelevo i dati da DetailContatori per idcontatore::" & IDContatore)
    '            drDetailsContatore = iDB.RunSPReturnRS("DetailContatori", New SqlParameter("@CodContatore", IDContatore), New SqlParameter("@CodContratto", IDContratto))
    '            If drDetailsContatore.Read Then
    '                DetailsContatore.nIdContatore = MyUtility.CIdFromDB(drDetailsContatore("CODCONTATORE"))
    '                DetailsContatore.sIdEnte = utility.stringoperation.formatstring(drDetailsContatore("codente"))
    '                DetailsContatore.sCodiceISTAT = utility.stringoperation.formatstring(drDetailsContatore("codice_istat"))
    '                DetailsContatore.nTipoContatore = MyUtility.CIdFromDB(drDetailsContatore("IDTIPOCONTATORE"))
    '                DetailsContatore.nIdImpianto = MyUtility.CIdFromDB(drDetailsContatore("CODIMPIANTO"))
    '                DetailsContatore.nPosizione = MyUtility.CIdFromDB(drDetailsContatore("CODPOSIZIONE"))
    '                DetailsContatore.nCodFognatura = MyUtility.CIdFromDB(drDetailsContatore("CODFOGNATURA"))
    '                DetailsContatore.nCodDepurazione = MyUtility.CIdFromDB(drDetailsContatore("CODDEPURAZIONE"))
    '                DetailsContatore.nGiro = MyUtility.CIdFromDB(drDetailsContatore("IDGIRO"))
    '                DetailsContatore.nIdContratto = MyUtility.CIdFromDB(drDetailsContatore("CODCONTRATTO"))
    '                If DetailsContatore.nIdContratto = -1 Then
    '                    DetailsContatore.nIdContratto = 0
    '                End If
    '                DetailsContatore.nTipoUtenza = MyUtility.CIdFromDB(drDetailsContatore("IDTIPOUTENZA"))
    '                DetailsContatore.nIdAttivita = MyUtility.CIdFromDB(drDetailsContatore("IDTIPOATTIVITA"))
    '                DetailsContatore.nDiametroContatore = MyUtility.CIdFromDB(drDetailsContatore("CODDIAMETROCONTATORE"))
    '                DetailsContatore.nDiametroPresa = MyUtility.CIdFromDB(drDetailsContatore("CODDIAMETROPRESA"))
    '                DetailsContatore.nIdVia = MyUtility.CIdFromDB(drDetailsContatore("COD_STRADA"))
    '                DetailsContatore.nCodIva = MyUtility.CIdFromDB(drDetailsContatore("CODIVA"))
    '                DetailsContatore.nConsumoMinimo = MyUtility.CIdFromDB(drDetailsContatore("IDMINIMO"))
    '                '===========================
    '                'INIZIO ALE CAO
    '                '===========================

    '                DetailsContatore.oDatiCatastali = GetDetailsCatasto(-1, DetailsContatore.nIdContatore)
    '                DetailsContatore.oListSubContatori = GetSubContatori(DetailsContatore.nIdContatore)

    '                If Not IsDBNull(drDetailsContatore("SPESA")) Then
    '                    DetailsContatore.nSpesa = drDetailsContatore("SPESA")
    '                Else
    '                    DetailsContatore.nSpesa = 0
    '                End If
    '                If Not IsDBNull(drDetailsContatore("DIRITTI")) Then
    '                    DetailsContatore.nDiritti = drDetailsContatore("DIRITTI")
    '                Else
    '                    DetailsContatore.nDiritti = 0
    '                End If

    '                DetailsContatore.nProprietario = drDetailsContatore("PROPRIETARIO")
    '                '===========================
    '                'FINE ALE CAO
    '                '===========================

    '                DetailsContatore.sPenalita = utility.stringoperation.formatstring(drDetailsContatore("PENALITA"))
    '                DetailsContatore.sStatoContatore = utility.stringoperation.formatstring(drDetailsContatore("STATOCONTATORE"))

    '                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    '                DetailsContatore.sMatricola = utility.stringoperation.formatstring(drDetailsContatore("MATRICOLA"))
    '                DetailsContatore.sSequenza = utility.stringoperation.formatstring(drDetailsContatore("SEQUENZA"))
    '                DetailsContatore.sProgressivo = utility.stringoperation.formatstring(drDetailsContatore("POSIZIONEPROGRESSIVA"))
    '                DetailsContatore.sLatoStrada = utility.stringoperation.formatstring(drDetailsContatore("LATOSTRADA"))
    '                DetailsContatore.sNumeroUtente = utility.stringoperation.formatstring(drDetailsContatore("NUMEROUTENTE"))
    '                DetailsContatore.nNumeroUtenze = CInt(drDetailsContatore("NUMEROUTENZE"))
    '                DetailsContatore.nIdContatorePrec = utility.stringoperation.formatint(drDetailsContatore("PRECEDENTE"))
    '                DetailsContatore.sMatricolaContatorePrec = utility.stringoperation.formatstring(drDetailsContatore("MATRICOLAPRECEDENTE"))
    '                DetailsContatore.nIdContatoreSucc = utility.stringoperation.formatint(drDetailsContatore("SUCCESSIVO"))
    '                DetailsContatore.sMatricolaContatoreSucc = utility.stringoperation.formatstring(drDetailsContatore("MATRICOLASUCCESSIVO"))
    '                DetailsContatore.sNote = utility.stringoperation.formatstring(drDetailsContatore("NOTE"))
    '                DetailsContatore.sCivico = utility.stringoperation.formatstring(drDetailsContatore("CIVICO_UBICAZIONE"))
    '                DetailsContatore.sEsponenteCivico = utility.stringoperation.formatstring(drDetailsContatore("ESPONENTE_CIVICO"))
    '                DetailsContatore.sUbicazione = utility.stringoperation.formatstring(drDetailsContatore("VIA_UBICAZIONE"))

    '                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '                DetailsContatore.bEsenteFognatura = utility.stringoperation.formatbool(drDetailsContatore("ESENTEFOGNATURA"))
    '                DetailsContatore.bEsenteDepurazione = utility.stringoperation.formatbool(drDetailsContatore("ESENTEDEPURAZIONE"))
    '                DetailsContatore.bEsenteAcqua = utility.stringoperation.formatbool(drDetailsContatore("ESENTEACQUA"))
    '                DetailsContatore.bIgnoraMora = utility.stringoperation.formatbool(drDetailsContatore("IGNORAMORA"))
    '                DetailsContatore.bUtenteSospeso = utility.stringoperation.formatbool(drDetailsContatore("UTENTESOSPESO"))

    '                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '                DetailsContatore.sDataAttivazione = oReplace.GiraDataFromDB(utility.stringoperation.formatstring(drDetailsContatore("DATAATTIVAZIONE")))
    '                DetailsContatore.sDataSostituzione = oReplace.GiraDataFromDB(utility.stringoperation.formatstring(drDetailsContatore("DATASOSTITUZIONE")))
    '                DetailsContatore.sDataRimTemp = oReplace.GiraDataFromDB(utility.stringoperation.formatstring(drDetailsContatore("DATARIMOZIONETEMPORANEA")))
    '                DetailsContatore.sDataCessazione = oReplace.GiraDataFromDB(utility.stringoperation.formatstring(drDetailsContatore("DATACESSAZIONE")))
    '                DetailsContatore.sDataSospensioneUtenza = oReplace.GiraDataFromDB(utility.stringoperation.formatstring(drDetailsContatore("DATASOSPENSIONEUTENZA")))

    '                DetailsContatore.sQuoteAgevolate = utility.stringoperation.formatstring(drDetailsContatore("QUOTEAGEVOLATE"))
    '                DetailsContatore.sCifreContatore = utility.stringoperation.formatstring(drDetailsContatore("CIFRECONTATORE"))
    '                DetailsContatore.sCodiceFabbricante = utility.stringoperation.formatstring(drDetailsContatore("CODICEFABBRICANTE"))

    '                '*** agenzia entrate
    '                If Not IsDBNull(drDetailsContatore("ID_TITOLO_OCCUPAZIONE")) Then
    '                    DetailsContatore.nIdTitoloOccupazione = utility.stringoperation.formatstring(drDetailsContatore("ID_TITOLO_OCCUPAZIONE"))
    '                End If

    '                If Not IsDBNull(drDetailsContatore("ID_TIPO_UNITA")) Then
    '                    DetailsContatore.sTipoUnita = utility.stringoperation.formatstring(drDetailsContatore("ID_TIPO_UNITA"))
    '                End If

    '                If Not IsDBNull(drDetailsContatore("ID_ASSENZA_DATI_CATASTALI")) Then
    '                    DetailsContatore.nIdAssenzaDatiCatastali = utility.stringoperation.formatstring(drDetailsContatore("ID_ASSENZA_DATI_CATASTALI"))
    '                End If

    '                If Not IsDBNull(drDetailsContatore("ID_TIPO_UTENZA")) Then
    '                    DetailsContatore.nIdTipoUtenza = utility.stringoperation.formatstring(drDetailsContatore("ID_TIPO_UTENZA"))
    '                End If
    '                '*** /agenzia entrate
    '                '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '                If Not IsDBNull(drDetailsContatore("ESENTEACQUAQF")) Then
    '                    DetailsContatore.bEsenteAcquaQF = utility.stringoperation.formatbool(drDetailsContatore("ESENTEACQUAQF"))
    '                End If
    '                If Not IsDBNull(drDetailsContatore("ESENTEDEPURAZIONEQF")) Then
    '                    DetailsContatore.bEsenteDepQF = CBool(drDetailsContatore("ESENTEDEPURAZIONEQF"))
    '                End If
    '                If Not IsDBNull(drDetailsContatore("ESENTEFOGNATURAQF")) Then
    '                    DetailsContatore.bEsenteFogQF = CBool(drDetailsContatore("ESENTEFOGNATURAQF"))
    '                End If
    '                '*** ***
    '            End If

    '            Dim drDetailsAnagrafica As new dataview
    '            Log.Debug("GetDetailsContatori::prelevo i dati da DetailAnagraficaIntestatario per idcontatore::" & IDContatore)
    '            drDetailsAnagrafica = iDB.RunSPReturnRS("DetailAnagraficaIntestatario", New SqlParameter("@CodContatore", IDContatore))
    '            If drDetailsAnagrafica.Read Then
    '                DetailsContatore.nIdIntestatario = utility.stringoperation.formatstring(drDetailsAnagrafica("COD_CONTRIBUENTE"))
    '            End If
    '            drDetailsAnagrafica.Close()
    '            Log.Debug("GetDetailsContatori::prelevo i dati da DetailAnagraficaIntestatario per idcontatore::" & IDContatore)
    '            drDetailsAnagrafica = iDB.RunSPReturnRS("DetailAnagraficaUtente", New SqlParameter("@CodContatore", IDContatore))
    '            If drDetailsAnagrafica.Read Then
    '                DetailsContatore.nIdUtente = utility.stringoperation.formatstring(drDetailsAnagrafica("COD_CONTRIBUENTE"))
    '            End If
    '            drDetailsAnagrafica.Close()
    '        End If

    '        Return DetailsContatore
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.GetDetailsContatori.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IDContatore"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetSubContatori(ByVal IDContatore As Integer) As ObjSubContatore()
        Dim oSubContatore As ObjSubContatore
        Dim nList As Integer = -1
        Dim oListSub() As ObjSubContatore = Nothing
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "DetailSubContatori", "CodContatore")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CodContatore", IDContatore))
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            oSubContatore = New ObjSubContatore
                            oSubContatore.IdContatorePrincipale = StringOperation.FormatInt(myRow("coDCONTATOREprincipale"))
                            oSubContatore.IdSubContatore = StringOperation.FormatInt(myRow("coDCONTATOREsub"))
                            oSubContatore.sCognomeIntestatario = StringOperation.FormatString(myRow("cognome"))
                            oSubContatore.sNomeIntestatario = StringOperation.FormatString(myRow("nome"))
                            oSubContatore.sMatricola = StringOperation.FormatString(myRow("matricola"))
                            oSubContatore.sUbicazione = StringOperation.FormatString(myRow("ubicazione"))
                            oSubContatore.sPeriodo = StringOperation.FormatString(myRow("periodo"))
                            nList += 1
                            ReDim Preserve oListSub(nList)
                            oListSub(nList) = oSubContatore
                        Next
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetSubContatori.errore: ", ex)
                Finally
                    dvMyDati.Dispose()
                End Try
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetSubContatori.errore: ", ex)
        Finally
            dvMyDati.Dispose()
        End Try
        Return oListSub
    End Function
    'Public Function GetSubContatori(ByVal IDContatore As Integer) As ObjSubContatore()
    '    Dim oSubContatore As ObjSubContatore
    '    Dim nList As Integer = -1
    '    Dim oListSub() As ObjSubContatore
    '    Dim droSubContatore As new dataview

    '    Try
    '        droSubContatore = iDB.RunSPReturnRS("DetailSubContatori", New SqlParameter("@CodContatore", IDContatore))
    '        Do While droSubContatore.Read
    '            oSubContatore = New ObjSubContatore
    '            oSubContatore.IdContatorePrincipale = CInt(droSubContatore("coDCONTATOREprincipale"))
    '            oSubContatore.IdSubContatore = CInt(droSubContatore("coDCONTATOREsub"))
    '            oSubContatore.sCognomeIntestatario = CStr(droSubContatore("cognome"))
    '            oSubContatore.sNomeIntestatario = CStr(droSubContatore("nome"))
    '            oSubContatore.sMatricola = CStr(droSubContatore("matricola"))
    '            oSubContatore.sUbicazione = CStr(droSubContatore("ubicazione"))
    '            nList += 1
    '            ReDim Preserve oListSub(nList)
    '            oListSub(nList) = oSubContatore
    '        Loop

    '        Return oListSub
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.GetSubContatori.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    Public Function VerificaMatricola(ByVal matricola As String, ByVal CodContatore As String, ByVal codiceIstat As String) As Boolean
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim lngTipoOp As Long
        Try
            lngTipoOp = DBOperation.DB_UPDATE

            If CodContatore = 0 Then lngTipoOp = DBOperation.DB_INSERT

            VerificaMatricola = False
            sSQL = "SELECT *"
            sSQL += " FROM TP_CONTATORI"
            sSQL += " WHERE MATRICOLA='" & matricola & "'"
            sSQL += " AND TP_CONTATORI.CODICE_ISTAT='" & codiceIstat & "'"
            Select Case lngTipoOp
                Case DBOperation.DB_INSERT

                Case DBOperation.DB_UPDATE
                    sSQL += " AND CODCONTATORE <> " & CodContatore
            End Select
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    VerificaMatricola = True
                Next
            End If
            dvMyDati.Dispose()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.VerificaMatricola.errore: ", Err)
        End Try
    End Function

    'Private Function ReturnCodiciEsterni(ByVal NomeTabella As String, ByVal NomeCampo As String, ByVal NomeCampoChiave As String, ByVal ValoreCampoChiave As String) As String
    '    Dim sSQL As String
    '    Dim dr As new dataview
    '    Try
    '        ReturnCodiciEsterni = ""
    '        sSQL = "SELECT " & NomeCampo & " FROM " & NomeTabella & " WHERE " & NomeCampoChiave & " = " & Utility.StringOperation.FormatString(ValoreCampoChiave)
    '        dr = iDB.getdataview(sSQL)

    '        If dr.Read Then
    '            Select Case NomeTabella
    '                Case "STRADARIO"
    '                    ReturnCodiciEsterni = Utility.StringOperation.FormatString(dr.Item("VIA"))
    '                Case Else
    '                    ReturnCodiciEsterni = Utility.StringOperation.FormatString(dr.Item(NomeCampo))
    '            End Select

    '        End If

    '        dr.Close()
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.ReturnCodiciEsterni.errore: ", Err)

    '    End Try
    'End Function

    Public Function VerificaNumeroUtente(ByVal numeroutente As String, ByVal CodContatore As String, ByVal codiceIstat As String) As Boolean
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim lngTipoOp As Long
        Try
            If Len(numeroutente) = 0 Then Exit Function
            lngTipoOp = DBOperation.DB_UPDATE

            If CodContatore = 0 Then lngTipoOp = DBOperation.DB_INSERT

            VerificaNumeroUtente = False

            sSQL = "SELECT * FROM TP_CONTATORI WHERE NUMEROUTENTE=" & numeroutente & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "TP_CONTATORI.CODICE_ISTAT=" & codiceIstat & vbCrLf
            Select Case lngTipoOp
                Case DBOperation.DB_INSERT

                Case DBOperation.DB_UPDATE
                    sSQL += "AND CODCONTATORE <> " & CodContatore
            End Select
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    VerificaNumeroUtente = True
                Next
            End If
            dvMyDati.Dispose()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.VerificaNumeroUtente.errore: ", Err)
        End Try
    End Function

    'Public Function ContatoreDaNonConsiderare(ByVal numeroutente As String, ByVal CodContatore As String, ByVal codiceIstat As String) As Boolean
    '    Dim sSQL As String
    '    Dim dvMyDati As new dataview
    '    Dim lngTipoOp As Long

    '    Try
    '        If Len(numeroutente) = 0 Then Exit Function
    '        lngTipoOp = DBOperation.DB_UPDATE

    '        If CodContatore = 0 Then lngTipoOp = DBOperation.DB_INSERT

    '        ContatoreDaNonConsiderare = False

    '        sSQL = "SELECT *"
    '        sSQL += " FROM TP_CONTATORI"
    '        sSQL += " WHERE (NUMEROUTENTE='" & numeroutente & "')"
    '        sSQL += " AND (CODICE_ISTAT=" & codiceIstat & ")"
    '        If lngTipoOp = DBOperation.DB_UPDATE Then
    '            sSQL += " AND (CODCONTATORE <> " & CodContatore & ")"
    '        End If
    '        dvMyDati = iDB.getdataview(sSQL)
    '        If dvMyDati.Read Then
    '            If Utility.StringOperation.FormatBool(dvMyDati.Item("DANONCONSIDERARE")) = True Then
    '                ContatoreDaNonConsiderare = True
    '            End If
    '        End If
    '        dvmydati.dispose()
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.ContatoreDaNonConsiderare.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in GestContatori::ContatoreDaNonConsiderare::" & Err.Message & "::SQL::" & sSQL)
    '        Return True
    '    End Try
    'End Function

    Public Function VerificaSequenza(ByVal IdGiro As String, ByVal Sequenza As String, ByVal CodContatore As String, ByVal codiceIstat As String) As Boolean
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim lngTipoOp As Long
        Try
            If Len(Sequenza) = 0 Then Exit Function
            lngTipoOp = DBOperation.DB_UPDATE

            If CodContatore = 0 Then lngTipoOp = DBOperation.DB_INSERT

            VerificaSequenza = False

            sSQL = "SELECT * FROM TP_CONTATORI WHERE IDGIRO=" & IdGiro
            sSQL += "AND SEQUENZA=" & Sequenza & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "TP_CONTATORI.CODICE_ISTAT=" & codiceIstat & vbCrLf
            Select Case lngTipoOp
                Case DBOperation.DB_INSERT

                Case DBOperation.DB_UPDATE
                    sSQL += "AND CODCONTATORE <> " & CodContatore
            End Select
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    VerificaSequenza = True
                Next
            End If
            dvMyDati.Dispose()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.VerificaSequenza.errore: ", Err)
        End Try

    End Function

    'Public Function AggiornaDatiCatastali(ByVal interno As String, ByVal piano As String, ByVal foglio As String, ByVal numero As String, ByVal subalterno As String, ByVal IDCatasto As Int32, ByVal sezione As String, ByVal estensioneParticella As String, ByVal tipoParticella As String) As Integer
    '    Dim sSQL As String
    '    Dim sqlConn As New SqlConnection

    '    Try
    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()

    '        sSQL = "UPDATE TR_CONTATORI_CATASTALI SET "
    '        sSQL += "INTERNO='" & interno & "',"
    '        sSQL += "PIANO='" & piano & "',"
    '        sSQL += "FOGLIO='" & foglio & "',"
    '        sSQL += "NUMERO='" & numero & "',"
    '        If subalterno <> "" Then
    '            sSQL += "SUBALTERNO=" & subalterno & ","
    '        Else
    '            sSQL += "SUBALTERNO=-1,"
    '        End If

    '        sSQL += "SEZIONE='" & sezione & "',"
    '        sSQL += "ESTENSIONE_PARTICELLA='" & estensioneParticella & "',"
    '        sSQL += "ID_TIPO_PARTICELLA='" & tipoParticella & "'"

    '        sSQL += " WHERE IDCONT_CATAS=" & IDCatasto
    '        Dim sqlCmdModifica As SqlCommand

    '        sqlCmdModifica = New SqlCommand(sSQL, sqlConn)

    '        sqlCmdModifica.ExecuteNonQuery()

    '        sqlConn.Close()

    '        Return 1
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.AggiornaDatiCatastali.errore: ", ex)
    '        Return -1
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="codContatore"></param>
    ''' <param name="dataLettura"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Sub UpdatePrimaLettura(ByVal codContatore As Int32, ByVal dataLettura As String)
        Dim myLettura As New ObjLettura
        Try
            myLettura.IdLettura = -1
            myLettura.nIdContatore = codContatore
            myLettura.tDataLetturaAtt = oReplace.GiraData(dataLettura)
            myLettura.sAzione = "PRIMA"
            If New GestLetture().SetLettura(myLettura) <= 0 Then
                Throw New Exception("errore in aggiornamento prima lettura")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.UpdatePrimaLettura.errore: ", ex)
            Throw
        Finally
            cmdMyCommand.Connection.Close()
        End Try
    End Sub
    'Public Sub UpdatePrimaLettura(ByVal codContatore As Int32, ByVal dataLettura As String)
    '    Try
    '        cmdMyCommand = New SqlCommand()
    '        cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText = "prc_TP_LETTURE_IU"
    '        cmdMyCommand.Parameters.AddWithValue("@CODLETTURA", -1)
    '        cmdMyCommand.Parameters.AddWithValue("@CODCONTATORE", codContatore)
    '        cmdMyCommand.Parameters.AddWithValue("@CODCONTATOREPRECEDENTE", -1)
    '        cmdMyCommand.Parameters.AddWithValue("@CODPERIODO", -1)
    '        cmdMyCommand.Parameters.AddWithValue("@DATALETTURA", oReplace.GiraData(dataLettura))
    '        cmdMyCommand.Parameters.AddWithValue("@LETTURA", -1)
    '        cmdMyCommand.Parameters.AddWithValue("@CODMODALITALETTURA", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@CONSUMO", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@NOTE", "")
    '        cmdMyCommand.Parameters.AddWithValue("@GIORNIDICONSUMO", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@CONSUMOTEORICO", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@CODUTENTE", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@IDSTATOLETTURA", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@COD_ANOMALIA1", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@COD_ANOMALIA2", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@COD_ANOMALIA3", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@INCONGRUENTE", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@DATADIPASSAGGIO", "")
    '        cmdMyCommand.Parameters.AddWithValue("@LETTURATEORICA", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@DATALETTURAPRECEDENTE", "")
    '        cmdMyCommand.Parameters.AddWithValue("@LETTURAPRECEDENTE", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@PRIMALETTURA", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@DATA_INSERIMENTO", DateTime.MaxValue)
    '        cmdMyCommand.Parameters.AddWithValue("@DATA_VARIAZIONE", DateTime.MaxValue)
    '        cmdMyCommand.Parameters.AddWithValue("@AZIONE", "PRIMA")
    '        cmdMyCommand.Parameters.AddWithValue("@GIROCONTATORE", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@FATTURAZIONESOSPESA", 0)
    '        cmdMyCommand.Parameters("@CODLETTURA").Direction = ParameterDirection.InputOutput
    '        cmdMyCommand.ExecuteNonQuery()
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.UpdatePrimaLettura.errore: ", ex)
    '        Throw
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="interno"></param>
    ''' <param name="piano"></param>
    ''' <param name="foglio"></param>
    ''' <param name="numero"></param>
    ''' <param name="subalterno"></param>
    ''' <param name="IDContatore"></param>
    ''' <param name="sezione"></param>
    ''' <param name="estensioneParticella"></param>
    ''' <param name="idTipoParticella"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function SetDatiCatastali(ByVal interno As String, ByVal piano As String, ByVal foglio As String, ByVal numero As String, ByVal subalterno As String, ByVal IDContatore As Integer, ByVal sezione As String, ByVal estensioneParticella As String, ByVal idTipoParticella As String) As Integer
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""
        Dim myID As Integer = -1
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TR_CONTATORI_CATASTALI_IU", "ID", "CODCONTATORE", "INTERNO", "PIANO", "FOGLIO", "NUMERO", "SUBALTERNO", "SEZIONE", "ESTENSIONE_PARTICELLA", "ID_TIPO_PARTICELLA")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", -1) _
                        , ctx.GetParam("CODCONTATORE", IDContatore) _
                        , ctx.GetParam("INTERNO", interno) _
                        , ctx.GetParam("PIANO", piano) _
                        , ctx.GetParam("FOGLIO", foglio) _
                        , ctx.GetParam("NUMERO", numero) _
                        , ctx.GetParam("SUBALTERNO", subalterno) _
                        , ctx.GetParam("SEZIONE", sezione) _
                        , ctx.GetParam("ESTENSIONE_PARTICELLA", estensioneParticella) _
                        , ctx.GetParam("ID_TIPO_PARTICELLA", idTipoParticella)
                    )
                ctx.Dispose()
            End Using
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myID = Utility.StringOperation.FormatInt(myRow("id"))
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.SetDatiCatastali.errore: ", ex)
            myID = -1
        Finally
            dvMyDati.Dispose()
        End Try
        Return myID
    End Function
    'Public Function SetDatiCatastali(ByVal interno As String, ByVal piano As String, ByVal foglio As String, ByVal numero As String, ByVal subalterno As String, ByVal IDContatore As Integer, ByVal sezione As String, ByVal estensioneParticella As String, ByVal idTipoParticella As String) As Integer
    '    Try
    '        cmdMyCommand = New SqlCommand()
    '        cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText = "prc_TR_CONTATORI_CATASTALI_IU"
    '        cmdMyCommand.Parameters.AddWithValue("@ID", -1)
    '        cmdMyCommand.Parameters.AddWithValue("@CODCONTATORE", IDContatore)
    '        cmdMyCommand.Parameters.AddWithValue("@INTERNO", interno)
    '        cmdMyCommand.Parameters.AddWithValue("@PIANO", piano)
    '        cmdMyCommand.Parameters.AddWithValue("@FOGLIO", foglio)
    '        cmdMyCommand.Parameters.AddWithValue("@NUMERO", numero)
    '        cmdMyCommand.Parameters.AddWithValue("@SUBALTERNO", subalterno)
    '        cmdMyCommand.Parameters.AddWithValue("@SEZIONE", sezione)
    '        cmdMyCommand.Parameters.AddWithValue("@ESTENSIONE_PARTICELLA", estensioneParticella)
    '        cmdMyCommand.Parameters.AddWithValue("@ID_TIPO_PARTICELLA", idTipoParticella)
    '        cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
    '        cmdMyCommand.ExecuteNonQuery()
    '        Return cmdMyCommand.Parameters("@ID").Value
    '    Catch ex As Exception
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.SetDatiCatastali.errore: ", ex)
    '        Return -1
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    Public Function EliminaDatiCatastali(ByVal IDCatasto As Integer) As Integer
        Try
            cmdMyCommand = New SqlCommand()
            cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText = "prc_TR_CONTATORI_CATASTALI_D"
            cmdMyCommand.Parameters.AddWithValue("@ID", IDCatasto)
            cmdMyCommand.ExecuteNonQuery()
            Return 1
        Catch ex As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.EliminaDatiCatastali.errore: ", ex)
            Return -1
        Finally
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="codContatore"></param>
    ''' <param name="codPeriodo"></param>
    ''' <param name="dataLettura"></param>
    ''' <param name="contatorePrecedente"></param>
    ''' <param name="codUtente"></param>
    ''' <param name="nUtente"></param>
    ''' <param name="lnglettura"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Sub SetPrimaLettura(ByVal codContatore As Long, ByVal codPeriodo As Long, ByVal dataLettura As String, ByVal contatorePrecedente As Long, ByVal codUtente As Long, ByVal nUtente As String, ByVal lnglettura As Long)
        Dim oMyLettura As New ObjLettura
        Try
            oMyLettura.nIdContatore = codContatore
            If contatorePrecedente <> -1 Then
                oMyLettura.nIdContatorePrec = contatorePrecedente
            End If
            oMyLettura.nIdPeriodo = codPeriodo
            oMyLettura.nIdUtente = nUtente
            oMyLettura.tDataLetturaAtt = dataLettura
            oMyLettura.nLetturaAtt = lnglettura
            oMyLettura.bIsPrimaLettura = True
            oMyLettura.tDataInserimento = Now
            oMyLettura.sAzione = "DE-INS"
            If New GestLetture().SetLetture(oMyLettura.IdLettura, oMyLettura) < 1 Then
                Throw New Exception("Errore in salvataggio prima lettura")
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.SetPrimaLettura.errore: ", Err)
        End Try
    End Sub
    'Public Sub SetPrimaLettura(ByVal codContatore As Long, ByVal codPeriodo As Long, ByVal dataLettura As String, ByVal contatorePrecedente As Long, ByVal codUtente As Long, ByVal nUtente As String, ByVal lnglettura As Long, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
    '    Dim oMyLettura As New ObjLettura
    '    Dim GestLetture As New GestLetture
    '    Try
    '        oMyLettura.nIdContatore = codContatore
    '        If contatorePrecedente <> -1 Then
    '            oMyLettura.nIdContatorePrec = contatorePrecedente
    '        End If
    '        oMyLettura.nIdPeriodo = codPeriodo
    '        oMyLettura.nIdUtente = nUtente
    '        oMyLettura.tDataLetturaAtt = dataLettura
    '        oMyLettura.nLetturaAtt = lnglettura
    '        oMyLettura.bIsPrimaLettura = True
    '        oMyLettura.tDataInserimento = Now
    '        oMyLettura.sAzione = "DE-INS"
    '        '=====================================================================================================================
    '        'SALVATAGGIO DELLE LETTURE NUOVE
    '        '=====================================================================================================================
    '        If GestLetture.SetLetture(oMyLettura.IdLettura, oMyLettura) < 1 Then

    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.SetPrimaLettura.errore: ", Err)
    '    End Try
    'End Sub

    'Public Sub SetCatastaliDatatable(ByVal mioData As DataTable, ByVal codContatore As String)
    '    Dim sSQL As String
    '    Dim sqlConn As New SqlConnection
    '    sqlConn.ConnectionString = ConstSession.StringConnection
    '    sqlConn.Open()
    '    Dim i As Int32
    '    Dim sqlCmdInsert As SqlCommand

    '    Dim DBContatori As New GestContatori

    '    Dim IDTipoParticella As String = String.Empty
    '    Try
    '        'elimino tutti i dati catastali precedentemente inseriti per il codcontatore
    '        sSQL = "DELETE"
    '        sSQL += " FROM TR_CONTATORI_CATASTALI"
    '        sSQL += " WHERE (TR_CONTATORI_CATASTALI.CODCONTATORE=" & codContatore & ")"
    '        sqlCmdInsert = New SqlCommand(sSQL, sqlConn)
    '        sqlCmdInsert.ExecuteNonQuery()
    '        'inserisco tutti i dati catastali attuali
    '        For i = 0 To mioData.Rows.Count - 1
    '            If IsDBNull(mioData.Rows(i)("ID_TIPO_PARTICELLA")) Then
    '                IDTipoParticella = ""
    '            Else
    '                IDTipoParticella = DBContatori.GetIdTipoParticella(mioData.Rows(i)("ID_TIPO_PARTICELLA"))
    '            End If
    '            sSQL = "INSERT INTO TR_CONTATORI_CATASTALI"
    '            sSQL += " (CODCONTATORE,"
    '            sSQL += " INTERNO,"
    '            sSQL += " PIANO,"
    '            sSQL += " FOGLIO,"
    '            sSQL += " NUMERO,"
    '            sSQL += " SUBALTERNO,"
    '            sSQL += " SEZIONE,"
    '            sSQL += " ESTENSIONE_PARTICELLA,"
    '            sSQL += " ID_TIPO_PARTICELLA"
    '            sSQL += ")"
    '            sSQL += " VALUES("
    '            sSQL += CInt(codContatore) & ","
    '            sSQL += "'" & CStr(mioData.Rows(i)("INTERNO")).Replace("'", "") & "',"
    '            sSQL += "'" & CStr(mioData.Rows(i)("PIANO")).Replace("'", "") & "',"
    '            sSQL += "'" & CStr(mioData.Rows(i)("FOGLIO")).Replace("'", "") & "',"
    '            sSQL += "'" & CStr(mioData.Rows(i)("NUMERO")).Replace("'", "") & "',"
    '            If mioData.Rows(i)("SUBALTERNO").ToString() <> "" Then
    '                sSQL += CInt(mioData.Rows(i)("SUBALTERNO")) & ","
    '            Else
    '                sSQL += "Null,"
    '            End If

    '            If IsDBNull(mioData.Rows(i)("SEZIONE")) Then
    '                sSQL += "Null,"
    '            Else
    '                sSQL += "'" & CStr(mioData.Rows(i)("SEZIONE")).Replace("'", "") & "',"
    '            End If

    '            If IsDBNull(mioData.Rows(i)("ESTENSIONE_PARTICELLA")) Then
    '                sSQL += "Null,"
    '            Else
    '                sSQL += "'" & CStr(mioData.Rows(i)("ESTENSIONE_PARTICELLA")).Replace("'", "") & "',"
    '            End If

    '            If IDTipoParticella = "" Then
    '                sSQL += "Null"
    '            Else
    '                sSQL += "'" & IDTipoParticella.Replace("'", "''") & "'"
    '            End If
    '            sSQL += ")"
    '            sqlCmdInsert = New SqlCommand(sSQL, sqlConn)
    '            sqlCmdInsert.ExecuteNonQuery()
    '        Next
    '        sqlConn.Close()
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.SetCatastaliDataTable.errore: ", Err)
    '    End Try
    'End Sub

    'Public Function nextID() As Int32
    '    Dim sSQL As String
    '    Dim sqlConn As New SqlConnection
    '    Dim dvMyDati As DataSet
    '    Dim perupdate As Int32
    '    Try
    '        sSQL = "SELECT MAX(CODCONTATORE) AS ID_MAX FROM TP_CONTATORI" & vbCrLf
    '        dvMyDati = iDB.RunSQLReturnDataSet(sSQL)
    '        'If dvMyDati.Read = True Then
    '        If Not IsDBNull(dvMyDati.Tables(0).Rows(0)("ID_MAX")) Then
    '            perupdate = dvMyDati.Tables(0).Rows(0)("ID_MAX")
    '        End If
    '        'End If
    '        'IDValue = DBAccess.RunActionQueryIdentiy(sSQL)
    '        perupdate = perupdate + 1
    '        Return perupdate
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.nextID.errore: ", Err)
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CodContatore"></param>
    ''' <param name="oMyContatore"></param>
    ''' <param name="bAcqFromTxt"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function SetDatiContatore(ByVal CodContatore As Integer, ByRef oMyContatore As objContatore, ByVal bAcqFromTxt As Boolean) As Boolean
        Dim lngTipoOp As Integer = DBOperation.DB_INSERT
        Dim nMyIdContatore As Integer
        Dim dvDati As DataView
        Dim x As Integer

        Try
            'determino il tipo di operazione
            If CodContatore > 0 Then
                lngTipoOp = DBOperation.DB_UPDATE
                oMyContatore.nIdContatore = CodContatore
            End If


            Try
                If lngTipoOp = DBOperation.DB_INSERT Then
                    '***prelevo l'ultimo contatore presente***
                    nMyIdContatore = New MyUtility().GetMaxID("TP_CONTATORI")
                    If nMyIdContatore < 1 Then
                        Return False
                    End If
                    '***********************************
                    oMyContatore.nIdContatore = nMyIdContatore
                    'prelevo le quote agevolate
                    dvDati = New TabelleDiDecodifica.DBTipiUtenza().GetListaTipiUtenza(ConstSession.IdEnte, oMyContatore.nTipoUtenza, -1)
                    If Not dvDati Is Nothing Then
                        For Each myViewRow As DataRowView In dvDati
                            If Utility.StringOperation.FormatBool(myViewRow("PRINCIPALE")) Then
                                oMyContatore.sQuoteAgevolate = oMyContatore.nNumeroUtenze
                            Else
                                oMyContatore.sQuoteAgevolate = "0"
                            End If
                        Next
                    End If
                    dvDati.Dispose()
                ElseIf lngTipoOp = DBOperation.DB_UPDATE Then
                    Dim myLettura As New ObjLettura
                    myLettura.nIdContatore = oMyContatore.nIdContatore
                    myLettura.nIdUtente = oMyContatore.nIdUtente
                    myLettura.sAzione = "SOLOUTENTE"
                    myLettura.IdLettura = New GestLetture().SetLettura(myLettura)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.SetDatiContatore.ListaUtenza.errore: ", ex)
                Return False
            End Try

            'inserisco il contatore
            If SetContatore(oMyContatore, 1, -1, -1) = False Then
                Return False
            End If
            'elimino tutti i sub-contatori già associati
            If SetSubContatore(2, Nothing, oMyContatore.nIdContatore) = False Then
                Return False
            End If
            'se è stato assegnato un sub-contatore, il contatore avendo ID di quest'ultimo viene segnalato come sub-contatore
            If Not IsNothing(oMyContatore.oListSubContatori) Then
                For x = 0 To oMyContatore.oListSubContatori.GetUpperBound(0)
                    If SetSubContatore(1, oMyContatore.oListSubContatori(x), oMyContatore.nIdContatore) = False Then
                        Return False
                    End If
                Next
            End If

            'a questo punto, controllo se è stata inserita la data di sostituzione:
            If StringOperation.FormatDateTime(oMyContatore.sDataSostituzione).ToShortDateString <> Date.MaxValue.ToShortDateString Then
                'inserisco un nuovo contatore ribaltando tutti i dati di quello precedente, ma:
                ' - la data di attivazione è uguale alla data di sostituzione inserita + 1
                ' - contatoreprecedente è uguale all'id del vecchio contatore
                ' - tipo contatore, cifre contatore, matricola, diametro presa, diametro contatore e codice fabbricante sono da reinserire perchè variano
                oMyContatore.nTipoContatore = -1
                oMyContatore.sCifreContatore = ""
                oMyContatore.sMatricola = ""
                oMyContatore.nDiametroPresa = -1
                oMyContatore.nDiametroContatore = -1
                oMyContatore.sCodiceFabbricante = ""
                oMyContatore.sDataAttivazione = DateAdd(DateInterval.Day, 1, StringOperation.FormatDateTime(oMyContatore.sDataSostituzione)).ToString
                oMyContatore.sDataSostituzione = ""
                oMyContatore.sDataCessazione = ""
                oMyContatore.nIdContatorePrec = oMyContatore.nIdContatore
                oMyContatore.tDataInserimento = Now
                '*** 20130328 - non ribalto tipo utenza perchè potrebbe essere variata la codifica ***
                oMyContatore.nTipoUtenza = -1
                '*** ***
                '***prelevo l'ultimo contatore presente***
                nMyIdContatore = New MyUtility().GetMaxID("TP_CONTATORI")
                oMyContatore.nIdContatore = nMyIdContatore
                '***********************************

                If SetContatore(oMyContatore, 1, -1, -1) = False Then
                    Return False
                End If

                'elimino tutti i sub-contatori già associati
                If SetSubContatore(2, Nothing, oMyContatore.nIdContatore) = False Then
                    Return False
                End If
                'se è stato assegnato un sub-contatore, il contatore avendo ID di quest'ultimo viene segnalato come sub-contatore
                If Not IsNothing(oMyContatore.oListSubContatori) Then
                    For x = 0 To oMyContatore.oListSubContatori.GetUpperBound(0)
                        oMyContatore.oListSubContatori(x).IdContatorePrincipale = oMyContatore.nIdContatore
                        If SetSubContatore(1, oMyContatore.oListSubContatori(x), oMyContatore.nIdContatore) = False Then
                            Return False
                        End If
                    Next
                End If
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.SetDatiContatore.errore: ", ex)
            Return False
        End Try
    End Function
    'Public Function SetDatiContatore(ByVal CodContatore As Integer, ByRef oMyContatore As objContatore, Optional ByVal bAcqFromTxt As Boolean = False) As Boolean
    '    Dim lngTipoOp As Long = DBOperation.DB_INSERT
    '    Dim nMyIdContatore As Long
    '    Dim oReplace As New ClsGenerale.Generale
    '    Dim sqlTrans As SqlTransaction
    '    Dim sqlConn As New SqlConnection
    '    Dim dvDati As DataView
    '    Dim x As Integer

    '    Try
    '        'determino il tipo di operazione
    '        If CInt(CodContatore) > 0 Then
    '            lngTipoOp = DBOperation.DB_UPDATE
    '        End If

    '        'apro la connessione
    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()
    '        'apro la transazione
    '        sqlTrans = sqlConn.BeginTransaction

    '        '***prelevo l'ultimo contatore presente***
    '        nMyIdContatore = New MyUtility().GetMaxID("TP_CONTATORI")
    '        If nMyIdContatore < 1 Then
    '            Return False
    '        End If
    '        '***********************************

    '        Try
    '            cmdMyCommand = New SqlCommand()
    '            cmdMyCommand.Connection = sqlConn
    '            cmdMyCommand.Connection.Open()
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.CommandType = CommandType.StoredProcedure
    '            cmdMyCommand.Parameters.Clear()
    '            If lngTipoOp = DBOperation.DB_INSERT Then
    '                oMyContatore.nIdContatore = nMyIdContatore
    '                'prelevo le quote agevolate
    '                dvDati = New TabelleDiDecodifica.DBTipiUtenza().GetListaTipiUtenza(ConstSession.IdEnte, oMyContatore.nTipoUtenza)
    '                If Not dvDati Is Nothing Then
    '                    For Each myViewRow As DataRowView In dvDati
    '                        If Utility.StringOperation.FormatBool(dvDati("PRINCIPALE")) Then
    '                            oMyContatore.sQuoteAgevolate = oMyContatore.nNumeroUtenze
    '                        Else
    '                            oMyContatore.sQuoteAgevolate = "0"
    '                        End If
    '                    Next
    '                End If
    '                dvDati.Dispose()
    '            ElseIf lngTipoOp = DBOperation.DB_UPDATE Then
    '                oMyContatore.nIdContatore = CodContatore
    '                cmdMyCommand.CommandText = "prc_TP_LETTURE_IU"
    '                cmdMyCommand.Parameters.AddWithValue("@CODLETTURA", -1)
    '                cmdMyCommand.Parameters.AddWithValue("@CODCONTATORE", oMyContatore.nIdContatore)
    '                cmdMyCommand.Parameters.AddWithValue("@CODCONTATOREPRECEDENTE", -1)
    '                cmdMyCommand.Parameters.AddWithValue("@CODPERIODO", -1)
    '                cmdMyCommand.Parameters.AddWithValue("@DATALETTURA", "")
    '                cmdMyCommand.Parameters.AddWithValue("@LETTURA", -1)
    '                cmdMyCommand.Parameters.AddWithValue("@CODMODALITALETTURA", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@CONSUMO", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@NOTE", "")
    '                cmdMyCommand.Parameters.AddWithValue("@GIORNIDICONSUMO", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@CONSUMOTEORICO", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@CODUTENTE", oMyContatore.nIdUtente)
    '                cmdMyCommand.Parameters.AddWithValue("@IDSTATOLETTURA", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@COD_ANOMALIA1", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@COD_ANOMALIA2", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@COD_ANOMALIA3", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@INCONGRUENTE", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@DATADIPASSAGGIO", "")
    '                cmdMyCommand.Parameters.AddWithValue("@LETTURATEORICA", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@DATALETTURAPRECEDENTE", "")
    '                cmdMyCommand.Parameters.AddWithValue("@LETTURAPRECEDENTE", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@PRIMALETTURA", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_INSERIMENTO", DateTime.MaxValue)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_VARIAZIONE", DateTime.MaxValue)
    '                cmdMyCommand.Parameters.AddWithValue("@AZIONE", "SOLOUTENTE")
    '                cmdMyCommand.Parameters.AddWithValue("@GIROCONTATORE", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@FATTURAZIONESOSPESA", 0)
    '                cmdMyCommand.Parameters("@CODLETTURA").Direction = ParameterDirection.InputOutput
    '                cmdMyCommand.ExecuteNonQuery()
    '            End If
    '        Catch ex As Exception
    '            sqlTrans.Rollback()
    '            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.SetDatiContatore.errore: ", ex)
    '            Return False
    '        Finally
    '            cmdMyCommand.Connection.Close()
    '        End Try

    '        'inserisco il contatore
    '        If SetContatore(oMyContatore, sqlConn, sqlTrans, 1, -1) = False Then
    '            Return False
    '        End If
    '        'elimino tutti i sub-contatori già associati
    '        If SetSubContatore(2, Nothing, oMyContatore.nIdContatore, sqlConn, sqlTrans) = False Then
    '            Return False
    '        End If
    '        'se è stato assegnato un sub-contatore, il contatore avendo ID di quest'ultimo viene segnalato come sub-contatore
    '        If Not IsNothing(oMyContatore.oListSubContatori) Then
    '            For x = 0 To oMyContatore.oListSubContatori.GetUpperBound(0)
    '                If SetSubContatore(1, oMyContatore.oListSubContatori(x), oMyContatore.nIdContatore, sqlConn, sqlTrans) = False Then
    '                    Return False
    '                End If
    '            Next
    '        End If

    '        'a questo punto, controllo se è stata inserita la data di sostituzione:
    '        If oMyContatore.sDataSostituzione <> "" Then
    '            'inserisco un nuovo contatore ribaltando tutti i dati di quello precedente, ma:
    '            ' - la data di attivazione è uguale alla data di sostituzione inserita + 1
    '            ' - contatoreprecedente è uguale all'id del vecchio contatore
    '            ' - tipo contatore, cifre contatore, matricola, diametro presa, diametro contatore e codice fabbricante sono da reinserire perchè variano
    '            oMyContatore.nTipoContatore = -1
    '            oMyContatore.sCifreContatore = ""
    '            oMyContatore.sMatricola = ""
    '            oMyContatore.nDiametroPresa = -1
    '            oMyContatore.nDiametroContatore = -1
    '            oMyContatore.sCodiceFabbricante = ""
    '            oMyContatore.sDataAttivazione = DateAdd(DateInterval.Day, 1, CDate(oMyContatore.sDataSostituzione)).ToString
    '            oMyContatore.sDataSostituzione = ""
    '            oMyContatore.sDataCessazione = ""
    '            oMyContatore.nIdContatorePrec = oMyContatore.nIdContatore
    '            oMyContatore.tDataInserimento = Now
    '            '*** 20130328 - non ribalto tipo utenza perchè potrebbe essere variata la codifica ***
    '            oMyContatore.nTipoUtenza = -1
    '            '*** ***
    '            '***prelevo l'ultimo contatore presente***
    '            nMyIdContatore = New MyUtility().GetMaxID("TP_CONTATORI")
    '            oMyContatore.nIdContatore = nMyIdContatore
    '            '***********************************

    '            If SetContatore(oMyContatore, sqlConn, sqlTrans, 1, -1) = False Then
    '                Return False
    '            End If

    '            'elimino tutti i sub-contatori già associati
    '            If SetSubContatore(2, Nothing, oMyContatore.nIdContatore, sqlConn, sqlTrans) = False Then
    '                Return False
    '            End If
    '            'se è stato assegnato un sub-contatore, il contatore avendo ID di quest'ultimo viene segnalato come sub-contatore
    '            If Not IsNothing(oMyContatore.oListSubContatori) Then
    '                For x = 0 To oMyContatore.oListSubContatori.GetUpperBound(0)
    '                    oMyContatore.oListSubContatori(x).IdContatorePrincipale = oMyContatore.nIdContatore
    '                    If SetSubContatore(1, oMyContatore.oListSubContatori(x), oMyContatore.nIdContatore, sqlConn, sqlTrans) = False Then
    '                        Return False
    '                    End If
    '                Next
    '            End If
    '        End If
    '        'confermo la transazione
    '        sqlTrans.Commit()
    '        Return True
    '    Catch ex As Exception
    '        sqlTrans.Rollback()
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.SetDatiContatore.errore: ", ex)
    '        Return False
    '    Finally
    '        sqlConn.Close()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oMyContatore"></param>
    ''' <param name="nIsFromContatore"></param>
    ''' <param name="nContatoreVoltura"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function SetContatore(ByRef oMyContatore As objContatore, nIsFromContatore As Integer, nContatoreVoltura As Integer, IdContatoreOrg As Integer) As Boolean
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            'eseguo l'insert in TP_CONTATORI
            '*** 20150212 - se chiudo contatore chiudo anche contratto ***
            'se il contatore è stato chiuso devo ribaltare questa data anche sul contratto, se è aperto anche il suo contratto deve essere aperto
            'faccio un update in tp_contatori
            ' - contatoresuccessivo è uguale all'id del nuovo contatore creato
            ' - where: l'id del contatore è uguale al contatoreprecedente
            '========================================================
            'QUA DEVO COPIARE TUTTI I DATI CATASTALI deL VECCHIO CONTATORE NEL NUOVO CONTATORE
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_CONTATORI_IU", "ORIGINE", "CODCONTATORE", "CODCONTATOREVOLTURA", "CODCONTATOREPRECEDENTE", "CODCONTATORESUCCESSIVO" _
                            , "CODENTE", "CODENTEAPPARTENENZA", "NUMEROUTENTE", "NUMEROUTENZE", "CODIMPIANTO" _
                            , "IDGIRO", "SEQUENZA", "MATRICOLA", "IDTIPOCONTATORE", "IDTIPOUTENZA", "CODPOSIZIONE", "POSIZIONEPROGRESSIVA", "NOTE" _
                            , "DATAATTIVAZIONE", "DATACESSAZIONE", "DATASOSTITUZIONE", "CODFOGNATURA", "CODDEPURAZIONE", "ESENTEFOGNATURA", "ESENTEDEPURAZIONE" _
                            , "CONSUMOSTIMATO", "MINIMOFATTURABILE", "CODDIAMETROCONTATORE", "CODDIAMETROPRESA", "SCARICATOSUPDA", "LETTO", "DARICONTROLLARE", "MODULOAUTOLETTURA", "CODLETTURISTA", "CODPDA", "CODCONTRATTO" _
                            , "DATARIMOZIONETEMPORANEA", "CONSUMOMINIMOFATTURABILE", "CONSUMOMINIMOFATTURABILERIMTEMP", "LATOSTRADA", "LASCIATOAVVISO", "DATADIPASSAGGIO", "ANOMALIA", "DATA_SCARICO_PDA", "IGNORAMORA", "CODENTE1", "CODENTEAPPARTENENZA1" _
                            , "COD_STRADA", "CIVICO_UBICAZIONE", "FRAZIONE_UBICAZIONE" _
                            , "COGNOMEPROPRIETARIOFABBRICATO", "NOMEPROPRIETARIOFABBRICATO" _
                            , "DATASOSPENSIONEUTENZA", "UTENTESOSPESO", "QUOTEAGEVOLATE", "CODICEFABBRICANTE" _
                            , "CIFRECONTATORE", "CODIVA", "STATOCONTATORE", "PENALITA", "CODICE_ISTAT" _
                            , "PROGRESSIVO_ESTRAZIONE", "ESTRATTO", "CODICE_PUNTO_PRESA", "CODICE_UTENTE_ESTERNO" _
                            , "ESPONENTE_CIVICO", "IDMINIMO", "IDTIPOATTIVITA", "NOTELETTURISTA", "DIAMETROCONTATORE", "DATACONTROLLO", "SMAT", "DANONCONSIDERARE", "MATRICOLANUMERICA", "ACQUISITO", "PIANO" _
                            , "FOGLIO", "NUMERO", "SUBALTERNO", "PENDENTE", "SPESA", "DIRITTI", "PROPRIETARIO", "SUBCONTATORE", "SUBASSOCIATO", "PROVENIENZA", "VIA_UBICAZIONE", "ESENTEACQUA" _
                            , "ID_TITOLO_OCCUPAZIONE", "ID_TIPO_UTENZA", "ID_TIPO_UNITA", "ID_ASSENZA_DATI_CATASTALI" _
                            , "DATA_INSERIMENTO", "DATA_VARIAZIONE", "AZIONE" _
                            , "ESENTEFOGNATURAQF", "ESENTEDEPURAZIONEQF", "ESENTEACQUAQF", "CODORG")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ORIGINE", nIsFromContatore) _
                            , ctx.GetParam("CODCONTATORE", oMyContatore.nIdContatore) _
                            , ctx.GetParam("CODCONTATOREVOLTURA", nContatoreVoltura) _
                            , ctx.GetParam("CODCONTATOREPRECEDENTE", oMyContatore.nIdContatorePrec) _
                            , ctx.GetParam("CODCONTATORESUCCESSIVO", oMyContatore.nIdContatoreSucc) _
                            , ctx.GetParam("CODENTE", oMyContatore.sIdEnte) _
                            , ctx.GetParam("CODENTEAPPARTENENZA", StringOperation.FormatInt(oMyContatore.sIdEnteAppartenenza)) _
                            , ctx.GetParam("NUMEROUTENTE", oMyContatore.sNumeroUtente) _
                            , ctx.GetParam("NUMEROUTENZE", oMyContatore.nNumeroUtenze) _
                            , ctx.GetParam("CODIMPIANTO", oMyContatore.nIdImpianto) _
                            , ctx.GetParam("IDGIRO", oMyContatore.nGiro) _
                            , ctx.GetParam("SEQUENZA", oMyContatore.sSequenza) _
                            , ctx.GetParam("MATRICOLA", oMyContatore.sMatricola.ToUpper()) _
                            , ctx.GetParam("IDTIPOCONTATORE", oMyContatore.nTipoContatore) _
                            , ctx.GetParam("IDTIPOUTENZA", oMyContatore.nTipoUtenza) _
                            , ctx.GetParam("CODPOSIZIONE", oMyContatore.nPosizione) _
                            , ctx.GetParam("POSIZIONEPROGRESSIVA", oMyContatore.sProgressivo) _
                            , ctx.GetParam("NOTE", oMyContatore.sNote) _
                            , ctx.GetParam("DATAATTIVAZIONE", oReplace.GiraData(StringOperation.FormatDateTime(oMyContatore.sDataAttivazione))) _
                            , ctx.GetParam("DATACESSAZIONE", oReplace.GiraData(StringOperation.FormatDateTime(oMyContatore.sDataCessazione))) _
                            , ctx.GetParam("DATASOSTITUZIONE", oReplace.GiraData(StringOperation.FormatDateTime(oMyContatore.sDataSostituzione))) _
                            , ctx.GetParam("CODFOGNATURA", oMyContatore.nCodFognatura) _
                            , ctx.GetParam("CODDEPURAZIONE", oMyContatore.nCodDepurazione) _
                            , ctx.GetParam("ESENTEFOGNATURA", oMyContatore.bEsenteFognatura) _
                            , ctx.GetParam("ESENTEDEPURAZIONE", oMyContatore.bEsenteDepurazione) _
                            , ctx.GetParam("CONSUMOSTIMATO", 0) _
                            , ctx.GetParam("MINIMOFATTURABILE", 0) _
                            , ctx.GetParam("CODDIAMETROCONTATORE", oMyContatore.nDiametroContatore) _
                            , ctx.GetParam("CODDIAMETROPRESA", oMyContatore.nDiametroPresa) _
                            , ctx.GetParam("SCARICATOSUPDA", 0) _
                            , ctx.GetParam("LETTO", 0) _
                            , ctx.GetParam("DARICONTROLLARE", 0) _
                            , ctx.GetParam("MODULOAUTOLETTURA", 0) _
                            , ctx.GetParam("CODLETTURISTA", 0) _
                            , ctx.GetParam("CODPDA", 0) _
                            , ctx.GetParam("CODCONTRATTO", oMyContatore.nIdContratto) _
                            , ctx.GetParam("DATARIMOZIONETEMPORANEA", oReplace.GiraData(StringOperation.FormatDateTime(oMyContatore.sDataRimTemp))) _
                            , ctx.GetParam("CONSUMOMINIMOFATTURABILE", 0) _
                            , ctx.GetParam("CONSUMOMINIMOFATTURABILERIMTEMP", 0) _
                            , ctx.GetParam("LATOSTRADA", oMyContatore.sLatoStrada) _
                            , ctx.GetParam("LASCIATOAVVISO", 0) _
                            , ctx.GetParam("DATADIPASSAGGIO", "") _
                            , ctx.GetParam("ANOMALIA", 0) _
                            , ctx.GetParam("DATA_SCARICO_PDA", "") _
                            , ctx.GetParam("IGNORAMORA", oMyContatore.bIgnoraMora) _
                            , ctx.GetParam("CODENTE1", oMyContatore.sIdEnte) _
                            , ctx.GetParam("CODENTEAPPARTENENZA1", oMyContatore.sIdEnteAppartenenza) _
                            , ctx.GetParam("COD_STRADA", oMyContatore.nIdVia) _
                            , ctx.GetParam("CIVICO_UBICAZIONE", oMyContatore.sCivico) _
                            , ctx.GetParam("FRAZIONE_UBICAZIONE", "") _
                            , ctx.GetParam("COGNOMEPROPRIETARIOFABBRICATO", "") _
                            , ctx.GetParam("NOMEPROPRIETARIOFABBRICATO", "") _
                            , ctx.GetParam("DATASOSPENSIONEUTENZA", oReplace.GiraData(StringOperation.FormatDateTime(oMyContatore.sDataSospensioneUtenza))) _
                            , ctx.GetParam("UTENTESOSPESO", oMyContatore.bUtenteSospeso) _
                            , ctx.GetParam("QUOTEAGEVOLATE", oMyContatore.sQuoteAgevolate) _
                            , ctx.GetParam("CODICEFABBRICANTE", oMyContatore.sCodiceFabbricante) _
                            , ctx.GetParam("CIFRECONTATORE", oMyContatore.sCifreContatore) _
                            , ctx.GetParam("CODIVA", oMyContatore.nCodIva) _
                            , ctx.GetParam("STATOCONTATORE", oMyContatore.sStatoContatore) _
                            , ctx.GetParam("PENALITA", oMyContatore.sPenalita) _
                            , ctx.GetParam("CODICE_ISTAT", oMyContatore.sCodiceISTAT) _
                            , ctx.GetParam("PROGRESSIVO_ESTRAZIONE", "") _
                            , ctx.GetParam("ESTRATTO", 0) _
                            , ctx.GetParam("CODICE_PUNTO_PRESA", Right(Utility.StringOperation.FormatString(oMyContatore.nIdImpianto).Replace("-1", "0"), 3).PadLeft(3, "0") & Utility.StringOperation.FormatString(oMyContatore.sNumeroUtente).PadLeft(10 - 3, "0")) _
                            , ctx.GetParam("CODICE_UTENTE_ESTERNO", Right(Utility.StringOperation.FormatString(oMyContatore.nIdImpianto).Replace("-1", "0"), 3).PadLeft(3, "0") & Utility.StringOperation.FormatString(oMyContatore.sNumeroUtente).PadLeft(10 - 3, "0")) _
                            , ctx.GetParam("ESPONENTE_CIVICO", oMyContatore.sEsponenteCivico) _
                            , ctx.GetParam("IDMINIMO", oMyContatore.nIdMinimo) _
                            , ctx.GetParam("IDTIPOATTIVITA", oMyContatore.nIdAttivita) _
                            , ctx.GetParam("NOTELETTURISTA", "") _
                            , ctx.GetParam("DIAMETROCONTATORE", "") _
                            , ctx.GetParam("DATACONTROLLO", "") _
                            , ctx.GetParam("SMAT", 0) _
                            , ctx.GetParam("DANONCONSIDERARE", 0) _
                            , ctx.GetParam("MATRICOLANUMERICA", "") _
                            , ctx.GetParam("ACQUISITO", 0) _
                            , ctx.GetParam("PIANO", "") _
                            , ctx.GetParam("FOGLIO", "") _
                            , ctx.GetParam("NUMERO", "") _
                            , ctx.GetParam("SUBALTERNO", 0) _
                            , ctx.GetParam("PENDENTE", oMyContatore.bIsPendente) _
                            , ctx.GetParam("SPESA", oMyContatore.nSpesa) _
                            , ctx.GetParam("DIRITTI", oMyContatore.nDiritti) _
                            , ctx.GetParam("PROPRIETARIO", oMyContatore.nProprietario) _
                            , ctx.GetParam("SUBCONTATORE", 0) _
                            , ctx.GetParam("SUBASSOCIATO", 0) _
                            , ctx.GetParam("PROVENIENZA", "") _
                            , ctx.GetParam("VIA_UBICAZIONE", oMyContatore.sUbicazione) _
                            , ctx.GetParam("ESENTEACQUA", oMyContatore.bEsenteAcqua) _
                            , ctx.GetParam("ID_TITOLO_OCCUPAZIONE", oMyContatore.nIdTitoloOccupazione) _
                            , ctx.GetParam("ID_TIPO_UTENZA", oMyContatore.nIdTipoUtenza) _
                            , ctx.GetParam("ID_TIPO_UNITA", oMyContatore.sTipoUnita) _
                            , ctx.GetParam("ID_ASSENZA_DATI_CATASTALI", oMyContatore.nIdAssenzaDatiCatastali) _
                            , ctx.GetParam("DATA_INSERIMENTO", StringOperation.FormatDateTime(Now)) _
                            , ctx.GetParam("DATA_VARIAZIONE", StringOperation.FormatDateTime(oMyContatore.tDataVariazione)) _
                            , ctx.GetParam("AZIONE", "") _
                            , ctx.GetParam("ESENTEFOGNATURAQF", oMyContatore.bEsenteFogQF) _
                            , ctx.GetParam("ESENTEDEPURAZIONEQF", oMyContatore.bEsenteDepQF) _
                            , ctx.GetParam("ESENTEACQUAQF", oMyContatore.bEsenteAcquaQF) _
                            , ctx.GetParam("CODORG", IdContatoreOrg)
                        )
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            oMyContatore.nIdContatore = Utility.StringOperation.FormatInt(myRow("ID"))
                        Next
                    End If

                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TR_INTESTATARIOUTENTE_IU", "NAMETBL", "IDRIF", "IDCONTRIBUENTE")
                    ctx.ExecuteNonQuery(sSQL, ctx.GetParam("NAMETBL", "TR_CONTATORI_INTESTATARIO") _
                            , ctx.GetParam("IDRIF", oMyContatore.nIdContatore) _
                            , ctx.GetParam("IDCONTRIBUENTE", oMyContatore.nIdIntestatario)
                        )

                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TR_INTESTATARIOUTENTE_IU", "NAMETBL", "IDRIF", "IDCONTRIBUENTE")
                    ctx.ExecuteNonQuery(sSQL, ctx.GetParam("NAMETBL", "TR_CONTATORI_UTENTE") _
                            , ctx.GetParam("IDRIF", oMyContatore.nIdContatore) _
                            , ctx.GetParam("IDCONTRIBUENTE", oMyContatore.nIdUtente)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.SetContatore.errore: ", ex)
                    Return False
                Finally
                    dvMyDati.Dispose()
                End Try
                ctx.Dispose()
            End Using
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.SetContatore.errore: ", ex)
            Return False
        End Try
    End Function
    'Public Function SetContatore(ByVal oMyContatore As objContatore, ByVal oMyConnection As SqlConnection, ByVal oMyTransaction As SqlTransaction, nIsFromContatore As Integer, nContatoreVoltura As Integer) As Boolean
    '    Try
    '        cmdMyCommand = New SqlCommand()
    '        cmdMyCommand.Connection = oMyConnection
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        'eseguo l'insert in TP_CONTATORI
    '        '*** 20150212 - se chiudo contatore chiudo anche contratto ***
    '        'se il contatore è stato chiuso devo ribaltare questa data anche sul contratto, se è aperto anche il suo contratto deve essere aperto
    '        'faccio un update in tp_contatori
    '        ' - contatoresuccessivo è uguale all'id del nuovo contatore creato
    '        ' - where: l'id del contatore è uguale al contatoreprecedente
    '        '========================================================
    '        'QUA DEVO COPIARE TUTTI I DATI CATASTALI deL VECCHIO CONTATORE NEL NUOVO CONTATORE
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText = "prc_TP_CONTATORI_IU"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ORIGINE", SqlDbType.Int)).Value = nIsFromContatore
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCONTATORE", SqlDbType.Int)).Value = oMyContatore.nIdContatore
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CONTATOREVOLTURA", SqlDbType.Int)).Value = nContatoreVoltura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCONTRATTO", SqlDbType.Int)).Value = oMyContatore.nIdContratto
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAATTIVAZIONE", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyContatore.sDataAttivazione)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCONTATOREPRECEDENTE", SqlDbType.Int)).Value = oMyContatore.nIdContatorePrec
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCONTATORESUCCESSIVO", SqlDbType.Int)).Value = oMyContatore.nIdContatoreSucc
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODENTE", SqlDbType.Int)).Value = oMyContatore.sIdEnte
    '        If IsNumeric(oMyContatore.sIdEnteAppartenenza) Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODENTEAPPARTENENZA", SqlDbType.Int)).Value = CInt(oMyContatore.sIdEnteAppartenenza)
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODENTEAPPARTENENZA", SqlDbType.Int)).Value = DBNull.Value
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMEROUTENTE", SqlDbType.NVarChar)).Value = oMyContatore.sNumeroUtente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMEROUTENZE", SqlDbType.Int)).Value = oMyContatore.nNumeroUtenze
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODIMPIANTO", SqlDbType.Int)).Value = oMyContatore.nIdImpianto
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDGIRO", SqlDbType.Int)).Value = oMyContatore.nGiro
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SEQUENZA", SqlDbType.NVarChar)).Value = oMyContatore.sSequenza
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MATRICOLA", SqlDbType.NVarChar)).Value = UCase(oMyContatore.sMatricola)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTIPOCONTATORE", SqlDbType.Int)).Value = oMyContatore.nTipoContatore
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTIPOUTENZA", SqlDbType.Int)).Value = oMyContatore.nTipoUtenza
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODPOSIZIONE", SqlDbType.Int)).Value = oMyContatore.nPosizione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@POSIZIONEPROGRESSIVA", SqlDbType.NVarChar)).Value = oMyContatore.sProgressivo
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyContatore.sNote
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATACESSAZIONE", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyContatore.sDataCessazione)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATASOSTITUZIONE", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyContatore.sDataSostituzione)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODFOGNATURA", SqlDbType.Int)).Value = oMyContatore.nCodFognatura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODDEPURAZIONE", SqlDbType.Int)).Value = oMyContatore.nCodDepurazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTEFOGNATURA", SqlDbType.Bit)).Value = oMyContatore.bEsenteFognatura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTEDEPURAZIONE", SqlDbType.Bit)).Value = oMyContatore.bEsenteDepurazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CONSUMOSTIMATO", SqlDbType.Float)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MINIMOFATTURABILE", SqlDbType.Money)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODDIAMETROCONTATORE", SqlDbType.Int)).Value = oMyContatore.nDiametroContatore
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODDIAMETROPRESA", SqlDbType.Int)).Value = oMyContatore.nDiametroPresa
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SCARICATOSUPDA", SqlDbType.Bit)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LETTO", SqlDbType.Bit)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DARICONTROLLARE", SqlDbType.Bit)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MODULOAUTOLETTURA", SqlDbType.Bit)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODLETTURISTA", SqlDbType.Int)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODPDA", SqlDbType.Int)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATARIMOZIONETEMPORANEA", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyContatore.sDataRimTemp)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CONSUMOMINIMOFATTURABILE", SqlDbType.Float)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CONSUMOMINIMOFATTURABILERIMTEMP", SqlDbType.Float)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LATOSTRADA", SqlDbType.NVarChar)).Value = oMyContatore.sLatoStrada
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LASCIATOAVVISO", SqlDbType.Bit)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADIPASSAGGIO", SqlDbType.NVarChar)).Value = DBNull.Value
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANOMALIA", SqlDbType.Bit)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_SCARICO_PDA", SqlDbType.NVarChar)).Value = DBNull.Value
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IGNORAMORA", SqlDbType.Bit)).Value = oMyContatore.bIgnoraMora
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODENTE1", SqlDbType.NVarChar)).Value = oMyContatore.sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODENTEAPPARTENENZA1", SqlDbType.NVarChar)).Value = oMyContatore.sIdEnteAppartenenza
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_STRADA", SqlDbType.Int)).Value = oMyContatore.nIdVia
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO_UBICAZIONE", SqlDbType.NVarChar)).Value = oMyContatore.sCivico
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FRAZIONE_UBICAZIONE", SqlDbType.NVarChar)).Value = ""
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOMEPROPRIETARIOFABBRICATO", SqlDbType.NVarChar)).Value = ""
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEPROPRIETARIOFABBRICATO", SqlDbType.NVarChar)).Value = ""
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATASOSPENSIONEUTENZA", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyContatore.sDataSospensioneUtenza)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@UTENTESOSPESO", SqlDbType.Bit)).Value = oMyContatore.bUtenteSospeso
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@QUOTEAGEVOLATE", SqlDbType.NVarChar)).Value = oMyContatore.sQuoteAgevolate
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICEFABBRICANTE", SqlDbType.NVarChar)).Value = oMyContatore.sCodiceFabbricante
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIFRECONTATORE", SqlDbType.NVarChar)).Value = oMyContatore.sCifreContatore
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODIVA", SqlDbType.Int)).Value = oMyContatore.nCodIva
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATOCONTATORE", SqlDbType.NVarChar)).Value = oMyContatore.sStatoContatore
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PENALITA", SqlDbType.NVarChar)).Value = oMyContatore.sPenalita
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_ISTAT", SqlDbType.NVarChar)).Value = oMyContatore.sCodiceISTAT
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROGRESSIVO_ESTRAZIONE", SqlDbType.NVarChar)).Value = ""
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESTRATTO", SqlDbType.Bit)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_PUNTO_PRESA", SqlDbType.NVarChar)).Value = Right(Utility.StringOperation.FormatString(oMyContatore.nIdImpianto), 3).PadLeft(3, "0") & Utility.StringOperation.FormatString(oMyContatore.sNumeroUtente).PadLeft(10 - 3, "0")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_UTENTE_ESTERNO", SqlDbType.NVarChar)).Value = Right(Utility.StringOperation.FormatString(oMyContatore.nIdImpianto), 3).PadLeft(3, "0") & Utility.StringOperation.FormatString(oMyContatore.sNumeroUtente).PadLeft(10 - 3, "0")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESPONENTE_CIVICO", SqlDbType.NVarChar)).Value = oMyContatore.sEsponenteCivico
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDMINIMO", SqlDbType.Int)).Value = oMyContatore.nIdMinimo
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTIPOATTIVITA", SqlDbType.Int)).Value = oMyContatore.nIdAttivita
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTELETTURISTA", SqlDbType.NVarChar)).Value = ""
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DIAMETROCONTATORE", SqlDbType.NVarChar)).Value = ""
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATACONTROLLO", SqlDbType.NVarChar)).Value = DBNull.Value
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SMAT", SqlDbType.Bit)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DANONCONSIDERARE", SqlDbType.Bit)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MATRICOLANUMERICA", SqlDbType.NVarChar)).Value = ""
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ACQUISITO", SqlDbType.Bit)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PIANO", SqlDbType.NVarChar)).Value = ""
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.NVarChar)).Value = ""
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.NVarChar)).Value = ""
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBALTERNO", SqlDbType.Int)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PENDENTE", SqlDbType.Bit)).Value = oMyContatore.bIsPendente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SPESA", SqlDbType.Float)).Value = oMyContatore.nSpesa
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DIRITTI", SqlDbType.Float)).Value = oMyContatore.nDiritti
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROPRIETARIO", SqlDbType.Int)).Value = oMyContatore.nProprietario
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBCONTATORE", SqlDbType.Bit)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBASSOCIATO", SqlDbType.Int)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZA", SqlDbType.NVarChar)).Value = ""
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA_UBICAZIONE", SqlDbType.NVarChar)).Value = oMyContatore.sUbicazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTEACQUA", SqlDbType.Bit)).Value = oMyContatore.bEsenteAcqua
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TITOLO_OCCUPAZIONE", SqlDbType.Int)).Value = oMyContatore.nIdTitoloOccupazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_UTENZA", SqlDbType.Int)).Value = oMyContatore.nIdTipoUtenza
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_UNITA", SqlDbType.NVarChar)).Value = oMyContatore.sTipoUnita
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_ASSENZA_DATI_CATASTALI", SqlDbType.Int)).Value = oMyContatore.nIdAssenzaDatiCatastali
    '        If oMyContatore.tDataInserimento = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = Now
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyContatore.tDataInserimento
    '        End If
    '        If oMyContatore.tDataVariazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyContatore.tDataVariazione
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@AZIONE", SqlDbType.NVarChar)).Value = ""
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTEFOGNATURAQF", SqlDbType.Bit)).Value = oMyContatore.bEsenteFogQF
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTEDEPURAZIONEQF", SqlDbType.Bit)).Value = oMyContatore.bEsenteDepQF
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTEACQUAQF", SqlDbType.Bit)).Value = oMyContatore.bEsenteAcquaQF
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        cmdMyCommand.Parameters("@CODCONTATORE").Direction = ParameterDirection.InputOutput
    '        cmdMyCommand.ExecuteNonQuery()
    '        oMyContatore.nIdContatore = cmdMyCommand.Parameters("@CODCONTATORE").Value
    '        '*** ***

    '        '************************************************************************************
    '        'INSERIMENTO ANAGRAFICHE NELLA TABELLA TR_CONTATORI_INTESTATARIO
    '        '************************************************************************************
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText = "prc_TR_INTESTATARIOUTENTE_IU"
    '        cmdMyCommand.Parameters.AddWithValue("@NAMETBL", "TR_CONTATORI_INTESTATARIO")
    '        cmdMyCommand.Parameters.AddWithValue("@IDRIF", oMyContatore.nIdContatore)
    '        cmdMyCommand.Parameters.AddWithValue("@IDCONTRIBUENTE", oMyContatore.nIdIntestatario)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()

    '        '************************************************************************************
    '        'INSERIMENTO ANAGRAFICHE NELLA TABELLA TR_CONTRATTI_UTENTE
    '        '************************************************************************************
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText = "prc_TR_INTESTATARIOUTENTE_IU"
    '        cmdMyCommand.Parameters.AddWithValue("@NAMETBL", "TR_CONTATORI_UTENTE")
    '        cmdMyCommand.Parameters.AddWithValue("@IDRIF", oMyContatore.nIdContatore)
    '        cmdMyCommand.Parameters.AddWithValue("@IDCONTRIBUENTE", oMyContatore.nIdUtente)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()

    '        Return True
    '    Catch ex As Exception
    '        oMyTransaction.Rollback()
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.SetContatore.errore: ", ex)
    '        Return False
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nTypeOperation"></param>
    ''' <param name="oMySubContatore"></param>
    ''' <param name="nIDContatorePrinc"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Function SetSubContatore(ByVal nTypeOperation As Integer, ByVal oMySubContatore As ObjSubContatore, ByVal nIDContatorePrinc As Integer) As Boolean
        Dim sSQL As String = ""
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                If nTypeOperation = DBOperation.DB_DELETE Then
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_SUBCONTATORI_D", "IDCONTATOREPRINCIPALE")
                    ctx.ExecuteNonQuery(sSQL, ctx.GetParam("IDCONTATOREPRINCIPALE", nIDContatorePrinc))
                Else
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_SUBCONTATORI_IU", "IDCONTATOREPRINCIPALE", "IDSUBCONTATORE")
                    ctx.ExecuteNonQuery(sSQL, ctx.GetParam("IDCONTATOREPRINCIPALE", nIDContatorePrinc) _
                            , ctx.GetParam("IDSUBCONTATORE", oMySubContatore.IdSubContatore)
                        )
                End If
                ctx.Dispose()
            End Using
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.SetSubContatore.errore: ", ex)
            Return False
        End Try
    End Function

    'Public Function GetSQLContatori(ByVal nDBOperation As DBOperation, ByVal oMyContatore As objContatore, ByVal nMyIdContatore As Long, ByVal nMyIdContratto As Integer, Optional ByVal nContatoreVoltura As Integer = -1, Optional ByVal sDataAttivazioneVoltura As String = "") As String
    '    Dim sSQL, strAppoggio As String

    '    Try
    '        Select Case nDBOperation
    '            Case DBOperation.DB_INSERT
    '                If nContatoreVoltura <> -1 Then
    '                    '*** 20130328 - non ribalto tipo utenza perchè potrebbe essere variata la codifica ***
    '                    sSQL += "INSERT INTO TP_CONTATORI "
    '                    sSQL += " (PROPRIETARIO,CODCONTATORE,CODCONTRATTO, DATAATTIVAZIONE, CODENTE,CODENTEAPPARTENENZA, CODIMPIANTO, IDGIRO, IDTIPOCONTATORE, "
    '                    'sSQL += "IDTIPOUTENZA,NUMEROUTENZE, CODPOSIZIONE, CODFOGNATURA, CODDEPURAZIONE, "
    '                    sSQL += "NUMEROUTENZE, CODPOSIZIONE, CODFOGNATURA, CODDEPURAZIONE, "
    '                    sSQL += "CODDIAMETROCONTATORE, CODDIAMETROPRESA, CODLETTURISTA, CODPDA,"
    '                    sSQL += "COD_STRADA, CODIVA, IDTIPOATTIVITA,"
    '                    sSQL += "CODENTE1,CODENTEAPPARTENENZA1,CIVICO_UBICAZIONE, FRAZIONE_UBICAZIONE, QUOTEAGEVOLATE,"
    '                    sSQL += "CODICEFABBRICANTE, CIFRECONTATORE, STATOCONTATORE, PENALITA, CODICE_ISTAT,"
    '                    sSQL += "CODICE_PUNTO_PRESA, ESPONENTE_CIVICO, DIAMETROCONTATORE,"
    '                    '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '                    sSQL += "ESENTEACQUA, ESENTEACQUAQF, ESENTEDEPURAZIONEQF, ESENTEFOGNATURAQF, ESENTEFOGNATURA, ESENTEDEPURAZIONE, SCARICATOSUPDA, LETTO, DARICONTROLLARE,"
    '                    sSQL += "MODULOAUTOLETTURA, LASCIATOAVVISO, ANOMALIA, IGNORAMORA, UTENTESOSPESO, VIA_UBICAZIONE,"
    '                    sSQL += "MATRICOLA,"
    '                    '*** FABI 20022009
    '                    sSQL += "ID_TITOLO_OCCUPAZIONE,ID_TIPO_UTENZA,ID_TIPO_UNITA,ID_ASSENZA_DATI_CATASTALI,"
    '                    sSQL += "DATA_INSERIMENTO)"
    '                    '*** /FABI
    '                    sSQL += " SELECT 0," & nMyIdContatore & "," & nMyIdContratto & ",'" & CStr(oReplace.GiraData(sDataAttivazioneVoltura)) & "',"
    '                    sSQL += "CODENTE,CODENTEAPPARTENENZA,CODIMPIANTO,IDGIRO,IDTIPOCONTATORE,"
    '                    'sSQL += "IDTIPOUTENZA,NUMEROUTENZE,CODPOSIZIONE,CODFOGNATURA,CODDEPURAZIONE,"
    '                    sSQL += "NUMEROUTENZE,CODPOSIZIONE,CODFOGNATURA,CODDEPURAZIONE,"
    '                    sSQL += "CODDIAMETROCONTATORE,CODDIAMETROPRESA,CODLETTURISTA,CODPDA,"
    '                    sSQL += "COD_STRADA, CODIVA, IDTIPOATTIVITA,"
    '                    sSQL += "CODENTE1,CODENTEAPPARTENENZA1,CIVICO_UBICAZIONE, FRAZIONE_UBICAZIONE, QUOTEAGEVOLATE,"
    '                    sSQL += "CODICEFABBRICANTE, CIFRECONTATORE, STATOCONTATORE, PENALITA, CODICE_ISTAT,"
    '                    sSQL += "CODICE_PUNTO_PRESA, ESPONENTE_CIVICO, DIAMETROCONTATORE,"
    '                    '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '                    sSQL += "ESENTEACQUA, ESENTEACQUAQF, ESENTEDEPURAZIONEQF, ESENTEFOGNATURAQF, ESENTEFOGNATURA, ESENTEDEPURAZIONE, SCARICATOSUPDA, LETTO, DARICONTROLLARE,"
    '                    sSQL += "MODULOAUTOLETTURA, LASCIATOAVVISO, ANOMALIA, IGNORAMORA, UTENTESOSPESO, VIA_UBICAZIONE,"
    '                    sSQL += "MATRICOLA,"
    '                    '*** Fabi 20022009
    '                    sSQL += "ID_TITOLO_OCCUPAZIONE,ID_TIPO_UTENZA,ID_TIPO_UNITA,ID_ASSENZA_DATI_CATASTALI,"
    '                    sSQL += "GETDATE()"
    '                    '*** /Fabi
    '                    sSQL += " FROM TP_CONTATORI"
    '                    sSQL += " WHERE (CODCONTATORE = " & nContatoreVoltura & ")"
    '                Else
    '                    sSQL = "INSERT INTO TP_CONTATORI"
    '                    sSQL += "(CODCONTATORE, CODCONTATOREPRECEDENTE, PROPRIETARIO, SPESA, DIRITTI, PENDENTE,"
    '                    sSQL += " CODCONTATORESUCCESSIVO, CODENTE, NUMEROUTENTE, NUMEROUTENZE, CODIMPIANTO,"
    '                    sSQL += " IDGIRO, SEQUENZA, MATRICOLA, IDTIPOCONTATORE, IDTIPOUTENZA,"
    '                    sSQL += " CODPOSIZIONE, POSIZIONEPROGRESSIVA, NOTE, DATAATTIVAZIONE, DATACESSAZIONE, DATASOSTITUZIONE,"
    '                    sSQL += " CODFOGNATURA, CODDEPURAZIONE"
    '                    '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '                    sSQL += ", ESENTEACQUA, ESENTEACQUAQF, ESENTEDEPURAZIONEQF, ESENTEFOGNATURAQF, ESENTEFOGNATURA, ESENTEDEPURAZIONE,"
    '                    sSQL += " CODDIAMETROCONTATORE, CODDIAMETROPRESA, DATARIMOZIONETEMPORANEA, IDMINIMO, LATOSTRADA,"
    '                    sSQL += " IGNORAMORA, CODENTE1, CODENTEAPPARTENENZA1, COD_STRADA,"
    '                    sSQL += " CIVICO_UBICAZIONE, DATASOSPENSIONEUTENZA, UTENTESOSPESO, QUOTEAGEVOLATE, CODICEFABBRICANTE,"
    '                    sSQL += " CIFRECONTATORE, CODIVA, STATOCONTATORE, PENALITA, CODICE_ISTAT, ESPONENTE_CIVICO,"
    '                    sSQL += " CODICE_PUNTO_PRESA, CODICE_UTENTE_ESTERNO, CODCONTRATTO,"
    '                    sSQL += " IDTIPOATTIVITA, VIA_UBICAZIONE,"
    '                    '** Fabi modifica 18112008 campi per gestione agenzia entrate
    '                    sSQL += "ID_TITOLO_OCCUPAZIONE,ID_TIPO_UTENZA,ID_TIPO_UNITA,ID_ASSENZA_DATI_CATASTALI,"
    '                    '** /Fabi modifica campi agenzia entrate
    '                    sSQL += "DATA_INSERIMENTO)"
    '                    sSQL += " VALUES ( " & nMyIdContatore & ", "
    '                    If oMyContatore.nIdContatorePrec > 0 Then
    '                        sSQL += CInt(oMyContatore.nIdContatorePrec) & ", "
    '                    Else
    '                        sSQL += "Null, "
    '                    End If
    '                    sSQL += CStr(oMyContatore.nProprietario) & ", "
    '                    If oMyContatore.nSpesa <> 0 Then
    '                        sSQL += oMyContatore.nSpesa.ToString().Replace(",", ".") & ", "
    '                    Else
    '                        sSQL += "Null, "
    '                    End If
    '                    If oMyContatore.nDiritti <> 0 Then
    '                        sSQL += oMyContatore.nDiritti.ToString().Replace(",", ".") & ", "
    '                    Else
    '                        sSQL += "Null, "
    '                    End If
    '                    sSQL += CInt(oMyContatore.bIsPendente) & ", "
    '                    If oMyContatore.nIdContatoreSucc > 0 Then
    '                        sSQL += CInt(oMyContatore.nIdContatoreSucc) & ", "
    '                    Else
    '                        sSQL += "Null, "
    '                    End If
    '                    sSQL += CStr(oMyContatore.sIdEnte) & ","
    '                    sSQL += "'" & CStr(oMyContatore.sNumeroUtente) & "', "
    '                    sSQL += CStr(oMyContatore.nNumeroUtenze) & ", "
    '                    If oMyContatore.nIdImpianto <> "-1" Then
    '                        sSQL += oMyContatore.nIdImpianto.ToString & ", "
    '                    Else
    '                        sSQL += " Null, "
    '                    End If
    '                    If oMyContatore.nGiro <> -1 Then
    '                        sSQL += CStr(oMyContatore.nGiro) & ","
    '                    Else
    '                        sSQL += " Null,"
    '                    End If
    '                    sSQL += "'" & CStr(oMyContatore.sSequenza) & "',"
    '                    sSQL += "'" & CStr(UCase(oMyContatore.sMatricola)) & "',"
    '                    If oMyContatore.nTipoContatore <> -1 Then
    '                        sSQL += CStr(oMyContatore.nTipoContatore) & ","
    '                    Else
    '                        sSQL += " Null,"
    '                    End If
    '                    If oMyContatore.nTipoUtenza <> -1 Then
    '                        sSQL += CStr(oMyContatore.nTipoUtenza) & ","
    '                    Else
    '                        sSQL += " Null,"
    '                    End If
    '                    sSQL += CStr(oMyContatore.nPosizione) & ","
    '                    sSQL += "'" & CStr(oMyContatore.sProgressivo) & "',"
    '                    sSQL += "'" & CStr(oReplace.ReplaceChar(oMyContatore.sNote)) & "',"
    '                    sSQL += "'" & CStr(oReplace.GiraData(oMyContatore.sDataAttivazione)) & "',"
    '                    sSQL += "'" & CStr(oReplace.GiraData(oMyContatore.sDataCessazione)) & "',"
    '                    sSQL += "'" & CStr(oReplace.GiraData(oMyContatore.sDataSostituzione)) & "',"
    '                    If oMyContatore.nCodFognatura <> -1 Then
    '                        sSQL += CStr(oMyContatore.nCodFognatura) & ","
    '                    Else
    '                        sSQL += " Null,"
    '                    End If
    '                    If oMyContatore.nCodDepurazione <> -1 Then
    '                        sSQL += CStr(oMyContatore.nCodDepurazione) & ","
    '                    Else
    '                        sSQL += "Null,"
    '                    End If
    '                    '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '                    sSQL += CInt(oMyContatore.bEsenteAcqua) & "," & CInt(oMyContatore.bEsenteAcquaQF) & "," & CInt(oMyContatore.bEsenteDepQF) & "," & CInt(oMyContatore.bEsenteFogQF) & "," & CInt(oMyContatore.bEsenteFognatura) & "," & CInt(oMyContatore.bEsenteDepurazione) & ","
    '                    If oMyContatore.nDiametroContatore <> -1 Then
    '                        sSQL += CStr(oMyContatore.nDiametroContatore) & ","
    '                    Else
    '                        sSQL += "Null,"
    '                    End If
    '                    If oMyContatore.nDiametroPresa <> -1 Then
    '                        sSQL += CStr(oMyContatore.nDiametroPresa) & ","
    '                    Else
    '                        sSQL += "Null,"
    '                    End If
    '                    sSQL += "'" & CStr(oReplace.GiraData(oMyContatore.sDataRimTemp)) & "',"
    '                    If oMyContatore.nIdMinimo <> -1 Then
    '                        sSQL += CStr(oMyContatore.nIdMinimo) & ","
    '                    Else
    '                        sSQL += "Null,"
    '                    End If
    '                    sSQL += "'" & CStr(oMyContatore.sLatoStrada) & "',"
    '                    sSQL += CInt(oMyContatore.bIgnoraMora) & ","
    '                    sSQL += "'" & CStr(oMyContatore.sIdEnte) & "',"
    '                    sSQL += "'" & CStr(oMyContatore.sIdEnteAppartenenza) & "',"
    '                    If oMyContatore.nIdVia <> -1 Then
    '                        sSQL += CStr(oMyContatore.nIdVia) & ","
    '                    Else
    '                        sSQL += " Null,"
    '                    End If
    '                    sSQL += "'" & CStr(oMyContatore.sCivico) & "',"
    '                    sSQL += "'" & CStr(oReplace.GiraData(oMyContatore.sDataSospensioneUtenza)) & "',"
    '                    sSQL += CInt(oMyContatore.bUtenteSospeso) & ","
    '                    sSQL += "'" & CStr(oMyContatore.sQuoteAgevolate) & "',"
    '                    sSQL += "'" & CStr(oMyContatore.sCodiceFabbricante) & "',"
    '                    sSQL += "'" & CStr(oMyContatore.sCifreContatore) & "',"
    '                    If oMyContatore.nCodIva <> -1 Then
    '                        sSQL += CStr(oMyContatore.nCodIva) & ","
    '                    Else
    '                        sSQL += "Null,"
    '                    End If
    '                    sSQL += "'" & CStr(oMyContatore.sStatoContatore) & "',"
    '                    sSQL += "'" & CStr(oMyContatore.sPenalita) & "',"
    '                    sSQL += "'" & CStr(oMyContatore.sCodiceISTAT) & "',"
    '                    sSQL += "'" & CStr(oMyContatore.sEsponenteCivico) & "',"
    '                    'Prelevo gli Ultimi 3 caratteri del codice impianto
    '                    strAppoggio = Right(utility.stringoperation.formatstring(oMyContatore.nIdImpianto), 3)
    '                    strAppoggio = strAppoggio.PadLeft(3, "0")
    '                    strAppoggio = strAppoggio & utility.stringoperation.formatstring(oMyContatore.sNumeroUtente).PadLeft(10 - Len(strAppoggio), "0")
    '                    sSQL += "'" & CStr(UCase(strAppoggio)) & "',"
    '                    sSQL += "'" & CStr(UCase(strAppoggio)) & "',"
    '                    'qui vado ad inserire l'id del contratto a cui è associato
    '                    If oMyContatore.nIdContratto <> -1 Then
    '                        sSQL += oMyContatore.nIdContratto.ToString & ","
    '                    Else
    '                        sSQL += "NULL,"
    '                    End If
    '                    If oMyContatore.nIdAttivita > 0 Then
    '                        sSQL += CStr(oMyContatore.nIdAttivita) & ","
    '                    Else
    '                        sSQL += "Null,"
    '                    End If
    '                    sSQL += "'" & CStr(oReplace.ReplaceChar(oMyContatore.sUbicazione)) & "',"
    '                    '** Fabi modifica 18112008 campi per gestione agenzia entrate
    '                    If oMyContatore.nIdTitoloOccupazione <> -1 Then
    '                        sSQL+=oMyContatore.nIdTitoloOccupazione & ","
    '                    Else
    '                        sSQL+="Null,"
    '                    End If
    '                    If oMyContatore.nIdTipoUtenza <> -1 Then
    '                        sSQL+=oMyContatore.nIdTipoUtenza & ","
    '                    Else
    '                        sSQL+="Null,"
    '                    End If
    '                    If oMyContatore.sTipoUnita <> "" Then
    '                        sSQL+="'" & oMyContatore.sTipoUnita & "',"
    '                    Else
    '                        sSQL+="Null,"
    '                    End If
    '                    If oMyContatore.nIdAssenzaDatiCatastali <> -1 Then
    '                        sSQL+=oMyContatore.nIdAssenzaDatiCatastali & ","
    '                    Else
    '                        sSQL+="Null,"
    '                    End If
    '                    '*** /Fabi
    '                    sSQL += "'" & oReplace.ReplaceDataForDB(oMyContatore.tDataInserimento) & "')"
    '                End If

    '            Case DBOperation.DB_UPDATE
    '                sSQL = "UPDATE TP_CONTATORI SET "
    '                sSQL += " PROPRIETARIO=" & CInt(oMyContatore.nProprietario) & ", "
    '                sSQL += " SPESA =" & oMyContatore.nSpesa.ToString().Replace(",", ".") & ", "
    '                sSQL += " DIRITTI =" & oMyContatore.nDiritti.ToString().Replace(",", ".") & ", "
    '                sSQL += " PENDENTE =" & CInt(oMyContatore.bIsPendente) & ", "
    '                sSQL += " CODENTE = " & CStr(oMyContatore.sIdEnte) & ", "
    '                sSQL += " NUMEROUTENTE = '" & CStr(oMyContatore.sNumeroUtente) & "', "
    '                sSQL += " NUMEROUTENZE = " & CInt(oMyContatore.nNumeroUtenze) & ", "
    '                If oMyContatore.nIdImpianto <> "-1" Then
    '                    sSQL += " CODIMPIANTO = " & CInt(oMyContatore.nIdImpianto) & ", "
    '                Else
    '                    sSQL += " CODIMPIANTO = Null, "
    '                End If
    '                If oMyContatore.nGiro <> -1 Then
    '                    sSQL += " IDGIRO = " & CInt(oMyContatore.nGiro) & ", "
    '                Else
    '                    sSQL += " IDGIRO = Null, "
    '                End If
    '                sSQL += " SEQUENZA = '" & CStr(oMyContatore.sSequenza) & "', "
    '                If oMyContatore.sMatricola <> "" Then
    '                    sSQL += " MATRICOLA = '" & CStr(UCase(oMyContatore.sMatricola)) & "', "
    '                End If
    '                If oMyContatore.nTipoContatore <> -1 Then
    '                    sSQL += " IDTIPOCONTATORE = " & CInt(oMyContatore.nTipoContatore) & ", "
    '                Else
    '                    sSQL += " IDTIPOCONTATORE = Null, "
    '                End If
    '                If oMyContatore.nTipoUtenza <> -1 Then
    '                    sSQL += " IDTIPOUTENZA = " & CInt(oMyContatore.nTipoUtenza) & ", "
    '                Else
    '                    sSQL += " IDTIPOUTENZA = Null, "
    '                End If
    '                sSQL += " CODPOSIZIONE = " & CInt(oMyContatore.nPosizione) & ", "
    '                sSQL += " POSIZIONEPROGRESSIVA = '" & CStr(oMyContatore.sProgressivo) & "', "
    '                sSQL += " NOTE = '" & CStr(oReplace.ReplaceChar(oMyContatore.sNote)) & "', "
    '                sSQL += " DATAATTIVAZIONE = '" & CStr(oReplace.GiraData(oMyContatore.sDataAttivazione)) & "', "
    '                If CStr(oMyContatore.sDataCessazione) <> "" Then
    '                    sSQL += " DATACESSAZIONE = '" & CStr(oReplace.GiraData(oMyContatore.sDataCessazione)) & "', "
    '                Else
    '                    sSQL += " DATACESSAZIONE = '',"
    '                End If
    '                If CStr(oMyContatore.sDataSostituzione) <> "" Then
    '                    sSQL += " DATASOSTITUZIONE = '" & CStr(oReplace.GiraData(oMyContatore.sDataSostituzione)) & "', "
    '                End If
    '                If oMyContatore.nCodFognatura > 0 Then
    '                    sSQL += " CODFOGNATURA = " & CInt(oMyContatore.nCodFognatura) & ", "
    '                Else
    '                    sSQL += " CODFOGNATURA = Null, "
    '                End If
    '                If oMyContatore.nCodDepurazione > 0 Then
    '                    sSQL += " CODDEPURAZIONE = " & CInt(oMyContatore.nCodDepurazione) & ", "
    '                Else
    '                    sSQL += " CODDEPURAZIONE = Null, "
    '                End If
    '                sSQL += " ESENTEACQUA = " & CInt(oMyContatore.bEsenteAcqua) & ", "
    '                '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '                sSQL += " ESENTEACQUAQF = " & CInt(oMyContatore.bEsenteAcquaQF) & ", "
    '                sSQL += " ESENTEFOGNATURAQF = " & CInt(oMyContatore.bEsenteFogQF) & ", "
    '                sSQL += " ESENTEDEPURAZIONEQF = " & CInt(oMyContatore.bEsenteDepQF) & ", "
    '                sSQL += " ESENTEFOGNATURA = " & CInt(oMyContatore.bEsenteFognatura) & ", "
    '                sSQL += " ESENTEDEPURAZIONE = " & CInt(oMyContatore.bEsenteDepurazione) & ", "
    '                sSQL += " CODDIAMETROCONTATORE = " & CInt(oMyContatore.nDiametroContatore) & ", "
    '                sSQL += " CODDIAMETROPRESA = " & CInt(oMyContatore.nDiametroPresa) & ", "
    '                sSQL += " DATARIMOZIONETEMPORANEA = '" & CStr(oReplace.GiraData(oMyContatore.sDataRimTemp)) & "', "
    '                If oMyContatore.nIdMinimo <> -1 Then
    '                    sSQL += " IDMINIMO = " & CInt(oMyContatore.nIdMinimo) & ", "
    '                Else
    '                    sSQL += " IDMINIMO = Null, "
    '                End If
    '                sSQL += " LATOSTRADA = '" & CStr(oMyContatore.sLatoStrada) & "', "
    '                sSQL += " IGNORAMORA = " & CInt(oMyContatore.bIgnoraMora) & ", "
    '                sSQL += " CODENTE1 = '" & CStr(oMyContatore.sIdEnte) & "', "
    '                sSQL += " CODENTEAPPARTENENZA1 = '" & CStr(oMyContatore.sIdEnteAppartenenza) & "', "
    '                If oMyContatore.nIdVia <> -1 Then
    '                    sSQL += " COD_STRADA = " & CInt(oMyContatore.nIdVia) & ", "
    '                Else
    '                    sSQL += " COD_STRADA = Null, "
    '                End If
    '                sSQL += " CIVICO_UBICAZIONE = '" & CStr(oMyContatore.sCivico) & "', "
    '                sSQL += " DATASOSPENSIONEUTENZA = '" & CStr(oReplace.GiraData(oMyContatore.sDataSospensioneUtenza)) & "', "
    '                sSQL += " UTENTESOSPESO = " & CInt(oMyContatore.bUtenteSospeso) & ", "
    '                sSQL += " QUOTEAGEVOLATE = '" & CStr(oMyContatore.sQuoteAgevolate) & "', "
    '                sSQL += " CODICEFABBRICANTE = '" & CStr(oMyContatore.sCodiceFabbricante) & "', "
    '                sSQL += " CIFRECONTATORE = '" & CStr(oMyContatore.sCifreContatore) & "', "
    '                If oMyContatore.nCodIva <> -1 Then
    '                    sSQL += " CODIVA = " & CInt(oMyContatore.nCodIva) & ", "
    '                Else
    '                    sSQL += " CODIVA = Null, "
    '                End If
    '                sSQL += " STATOCONTATORE = '" & CStr(oMyContatore.sStatoContatore) & "', "
    '                sSQL += " PENALITA = '" & CStr(oMyContatore.sPenalita) & "', "
    '                sSQL += " CODICE_ISTAT = '" & CStr(oMyContatore.sCodiceISTAT) & "', "
    '                sSQL += " ESPONENTE_CIVICO = '" & CStr(oMyContatore.sEsponenteCivico) & "', "
    '                If oMyContatore.nIdAttivita > 0 Then
    '                    sSQL += " IDTIPOATTIVITA = " & CInt(oMyContatore.nIdAttivita) & ", "
    '                Else
    '                    sSQL += " IDTIPOATTIVITA = Null, "
    '                End If
    '                strAppoggio = Right(utility.stringoperation.formatstring(oMyContatore.nIdImpianto), 3)
    '                strAppoggio = strAppoggio.PadLeft(3, "0")
    '                strAppoggio = strAppoggio & utility.stringoperation.formatstring(oMyContatore.sNumeroUtente).PadLeft(10 - Len(strAppoggio), "0")
    '                sSQL += " CODICE_PUNTO_PRESA = '" & CStr(UCase(strAppoggio)) & "', "
    '                sSQL += " CODICE_UTENTE_ESTERNO = '" & CStr(UCase(strAppoggio)) & "', "
    '                sSQL += " VIA_UBICAZIONE = '" & CStr(oReplace.ReplaceChar(oMyContatore.sUbicazione)) & "',"
    '                '*** Fabi 27112008 Agenzia entrate
    '                sSQL += " ID_TITOLO_OCCUPAZIONE = "
    '                If oMyContatore.nIdTitoloOccupazione <> -1 Then
    '                    sSQL+=oMyContatore.nIdTitoloOccupazione & ","
    '                Else
    '                    sSQL+="Null,"
    '                End If
    '                sSQL += " ID_TIPO_UNITA = "
    '                If oMyContatore.sTipoUnita <> "" Then
    '                    sSQL+="'" & oMyContatore.sTipoUnita & "',"
    '                Else
    '                    sSQL+="Null,"
    '                End If
    '                sSQL += " ID_ASSENZA_DATI_CATASTALI = "
    '                If oMyContatore.nIdAssenzaDatiCatastali <> -1 Then
    '                    sSQL+=oMyContatore.nIdAssenzaDatiCatastali & ","
    '                Else
    '                    sSQL+="Null,"
    '                End If
    '                sSQL += " ID_TIPO_UTENZA ="
    '                If oMyContatore.nIdTipoUtenza <> -1 Then
    '                    sSQL+=oMyContatore.nIdTipoUtenza & ","
    '                Else
    '                    sSQL+="Null,"
    '                End If
    '                sSQL += " DATA_VARIAZIONE='" & oReplace.ReplaceDataForDB(oMyContatore.tDataVariazione) & "'"
    '                '***/ Fabi
    '                sSQL += " WHERE (CODCONTATORE =" & nMyIdContatore & ")"
    '        End Select

    '        Return sSQL
    '    Catch Err As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.GetSQLContatori.errore: ", ex)
    '        Return ""
    '    End Try
    'End Function

    'Public Function GetSQLContatori(ByVal nIsFromContatore As Integer, ByVal oMyContatore As objContatore, ByVal nMyIdContatore As Long, ByVal nMyIdContratto As Integer, ByVal nContatoreVoltura As Integer, ByVal sDataAttivazioneVoltura As String, ByVal myConnection As SqlConnection, ByVal myTrans As SqlTransaction) As SqlCommand
    '    Try
    '        If Not myConnection Is Nothing Then
    '            cmdMyCommand.Connection = myConnection
    '            cmdMyCommand.Transaction = myTrans
    '        Else
    '            cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.CommandTimeout = 0
    '            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '                cmdMyCommand.Connection.Open()
    '            End If
    '        End If
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_TP_CONTATORI_IU"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ORIGINE", SqlDbType.Int)).Value = nIsFromContatore
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCONTATORE", SqlDbType.Int)).Value = nMyIdContatore
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CONTATOREVOLTURA", SqlDbType.Int)).Value = nContatoreVoltura
    '        If oMyContatore Is Nothing Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCONTRATTO", SqlDbType.Int)).Value = nMyIdContratto
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAATTIVAZIONE", SqlDbType.NVarChar)).Value = oReplace.GiraData(sDataAttivazioneVoltura)
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCONTRATTO", SqlDbType.Int)).Value = oMyContatore.nIdContratto
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAATTIVAZIONE", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyContatore.sDataAttivazione)
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCONTATOREPRECEDENTE", SqlDbType.Int)).Value = oMyContatore.nIdContatorePrec
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCONTATORESUCCESSIVO", SqlDbType.Int)).Value = oMyContatore.nIdContatoreSucc
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODENTE", SqlDbType.Int)).Value = oMyContatore.sIdEnte
    '            If IsNumeric(oMyContatore.sIdEnteAppartenenza) Then
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODENTEAPPARTENENZA", SqlDbType.Int)).Value = CInt(oMyContatore.sIdEnteAppartenenza)
    '            Else
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODENTEAPPARTENENZA", SqlDbType.Int)).Value = DBNull.Value
    '            End If
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMEROUTENTE", SqlDbType.NVarChar)).Value = oMyContatore.sNumeroUtente
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMEROUTENZE", SqlDbType.Int)).Value = oMyContatore.nNumeroUtenze
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODIMPIANTO", SqlDbType.Int)).Value = oMyContatore.nIdImpianto
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDGIRO", SqlDbType.Int)).Value = oMyContatore.nGiro
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SEQUENZA", SqlDbType.NVarChar)).Value = oMyContatore.sSequenza
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MATRICOLA", SqlDbType.NVarChar)).Value = UCase(oMyContatore.sMatricola)
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTIPOCONTATORE", SqlDbType.Int)).Value = oMyContatore.nTipoContatore
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTIPOUTENZA", SqlDbType.Int)).Value = oMyContatore.nTipoUtenza
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODPOSIZIONE", SqlDbType.Int)).Value = oMyContatore.nPosizione
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@POSIZIONEPROGRESSIVA", SqlDbType.NVarChar)).Value = oMyContatore.sProgressivo
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyContatore.sNote
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATACESSAZIONE", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyContatore.sDataCessazione)
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATASOSTITUZIONE", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyContatore.sDataSostituzione)
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODFOGNATURA", SqlDbType.Int)).Value = oMyContatore.nCodFognatura
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODDEPURAZIONE", SqlDbType.Int)).Value = oMyContatore.nCodDepurazione
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTEFOGNATURA", SqlDbType.Bit)).Value = oMyContatore.bEsenteFognatura
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTEDEPURAZIONE", SqlDbType.Bit)).Value = oMyContatore.bEsenteDepurazione
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CONSUMOSTIMATO", SqlDbType.Float)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MINIMOFATTURABILE", SqlDbType.Money)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODDIAMETROCONTATORE", SqlDbType.Int)).Value = oMyContatore.nDiametroContatore
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODDIAMETROPRESA", SqlDbType.Int)).Value = oMyContatore.nDiametroPresa
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SCARICATOSUPDA", SqlDbType.Bit)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LETTO", SqlDbType.Bit)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DARICONTROLLARE", SqlDbType.Bit)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MODULOAUTOLETTURA", SqlDbType.Bit)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODLETTURISTA", SqlDbType.Int)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODPDA", SqlDbType.Int)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATARIMOZIONETEMPORANEA", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyContatore.sDataRimTemp)
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CONSUMOMINIMOFATTURABILE", SqlDbType.Float)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CONSUMOMINIMOFATTURABILERIMTEMP", SqlDbType.Float)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LATOSTRADA", SqlDbType.NVarChar)).Value = oMyContatore.sLatoStrada
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LASCIATOAVVISO", SqlDbType.Bit)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADIPASSAGGIO", SqlDbType.NVarChar)).Value = DBNull.Value
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANOMALIA", SqlDbType.Bit)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_SCARICO_PDA", SqlDbType.NVarChar)).Value = DBNull.Value
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IGNORAMORA", SqlDbType.Bit)).Value = oMyContatore.bIgnoraMora
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODENTE1", SqlDbType.NVarChar)).Value = oMyContatore.sIdEnte
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODENTEAPPARTENENZA1", SqlDbType.NVarChar)).Value = oMyContatore.sIdEnteAppartenenza
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_STRADA", SqlDbType.Int)).Value = oMyContatore.nIdVia
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO_UBICAZIONE", SqlDbType.NVarChar)).Value = oMyContatore.sCivico
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FRAZIONE_UBICAZIONE", SqlDbType.NVarChar)).Value = ""
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOMEPROPRIETARIOFABBRICATO", SqlDbType.NVarChar)).Value = ""
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEPROPRIETARIOFABBRICATO", SqlDbType.NVarChar)).Value = ""
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATASOSPENSIONEUTENZA", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyContatore.sDataSospensioneUtenza)
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@UTENTESOSPESO", SqlDbType.Bit)).Value = oMyContatore.bUtenteSospeso
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@QUOTEAGEVOLATE", SqlDbType.NVarChar)).Value = oMyContatore.sQuoteAgevolate
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICEFABBRICANTE", SqlDbType.NVarChar)).Value = oMyContatore.sCodiceFabbricante
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIFRECONTATORE", SqlDbType.NVarChar)).Value = oMyContatore.sCifreContatore
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODIVA", SqlDbType.Int)).Value = oMyContatore.nCodIva
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATOCONTATORE", SqlDbType.NVarChar)).Value = oMyContatore.sStatoContatore
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PENALITA", SqlDbType.NVarChar)).Value = oMyContatore.sPenalita
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_ISTAT", SqlDbType.NVarChar)).Value = oMyContatore.sCodiceISTAT
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROGRESSIVO_ESTRAZIONE", SqlDbType.NVarChar)).Value = ""
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESTRATTO", SqlDbType.Bit)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_PUNTO_PRESA", SqlDbType.NVarChar)).Value = Right(utility.stringoperation.formatstring(oMyContatore.nIdImpianto), 3).PadLeft(3, "0") & utility.stringoperation.formatstring(oMyContatore.sNumeroUtente).PadLeft(10 - 3, "0")
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_UTENTE_ESTERNO", SqlDbType.NVarChar)).Value = Right(utility.stringoperation.formatstring(oMyContatore.nIdImpianto), 3).PadLeft(3, "0") & utility.stringoperation.formatstring(oMyContatore.sNumeroUtente).PadLeft(10 - 3, "0")
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESPONENTE_CIVICO", SqlDbType.NVarChar)).Value = oMyContatore.sEsponenteCivico
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDMINIMO", SqlDbType.Int)).Value = oMyContatore.nIdMinimo
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTIPOATTIVITA", SqlDbType.Int)).Value = oMyContatore.nIdAttivita
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTELETTURISTA", SqlDbType.NVarChar)).Value = ""
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DIAMETROCONTATORE", SqlDbType.NVarChar)).Value = ""
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATACONTROLLO", SqlDbType.NVarChar)).Value = DBNull.Value
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SMAT", SqlDbType.Bit)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DANONCONSIDERARE", SqlDbType.Bit)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MATRICOLANUMERICA", SqlDbType.NVarChar)).Value = ""
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ACQUISITO", SqlDbType.Bit)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PIANO", SqlDbType.NVarChar)).Value = ""
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.NVarChar)).Value = ""
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.NVarChar)).Value = ""
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBALTERNO", SqlDbType.Int)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PENDENTE", SqlDbType.Bit)).Value = oMyContatore.bIsPendente
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SPESA", SqlDbType.Float)).Value = oMyContatore.nSpesa
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DIRITTI", SqlDbType.Float)).Value = oMyContatore.nDiritti
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROPRIETARIO", SqlDbType.Int)).Value = oMyContatore.nProprietario
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBCONTATORE", SqlDbType.Bit)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBASSOCIATO", SqlDbType.Int)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZA", SqlDbType.NVarChar)).Value = ""
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA_UBICAZIONE", SqlDbType.NVarChar)).Value = oMyContatore.sUbicazione
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTEACQUA", SqlDbType.Bit)).Value = oMyContatore.bEsenteAcqua
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TITOLO_OCCUPAZIONE", SqlDbType.Int)).Value = oMyContatore.nIdTitoloOccupazione
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_UTENZA", SqlDbType.Int)).Value = oMyContatore.nIdTipoUtenza
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_UNITA", SqlDbType.NVarChar)).Value = oMyContatore.sTipoUnita
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_ASSENZA_DATI_CATASTALI", SqlDbType.Int)).Value = oMyContatore.nIdAssenzaDatiCatastali
    '            If oMyContatore.tDataInserimento = Date.MinValue Then
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = Now
    '            Else
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyContatore.tDataInserimento
    '            End If
    '            If oMyContatore.tDataVariazione = Date.MinValue Then
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '            Else
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyContatore.tDataVariazione
    '            End If
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@AZIONE", SqlDbType.NVarChar)).Value = ""
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTEFOGNATURAQF", SqlDbType.Bit)).Value = oMyContatore.bEsenteFogQF
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTEDEPURAZIONEQF", SqlDbType.Bit)).Value = oMyContatore.bEsenteDepQF
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTEACQUAQF", SqlDbType.Bit)).Value = oMyContatore.bEsenteAcquaQF
    '        End If
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '        Log.Debug("GetSQLContatori::" & cmdMyCommand.CommandText & "::" & sValParametri)
    '        Return cmdMyCommand
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.GetSQLContatori.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function GetSQLSubContatori(ByVal nDBOperation As DBOperation, ByVal oMySubContatore As ObjSubContatore, ByVal nIDContatorePrinc As Integer) As String
    '    Dim sSQL As String

    '    Try
    '        Select Case nDBOperation
    '            Case DBOperation.DB_INSERT
    '                sSQL += "INSERT INTO TP_SUBCONTATORI "
    '                sSQL += " (CODCONTATOREPRINCIPALE,CODCONTATORESUB,"
    '                sSQL += "DATA_INSERIMENTO)"
    '                sSQL += " VALUES ( " & oMySubContatore.IdContatorePrincipale & ", "
    '                sSQL += CInt(oMySubContatore.IdSubContatore) & ", "
    '                sSQL += "GETDATE())"

    '            Case DBOperation.DB_DELETE
    '                sSQL = "DELETE"
    '                sSQL += " FROM TP_SUBCONTATORI"
    '                sSQL += " WHERE (CODCONTATOREPRINCIPALE =" & nIDContatorePrinc & ")"
    '        End Select

    '        Return sSQL
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.GetSQLSubContatori.errore: ", Err)
    '        Return ""
    '    End Try
    'End Function

    'Private Function GetSQLIntestatarioUtente(ByVal nDBOperation As DBOperation, ByVal sNameTable As String, ByVal sNameCol As String, ByVal nIdCampo As Long, ByVal nIdContribuente As Integer) As String
    '    Dim sSQL As String
    '    Try
    '        Select Case nDBOperation
    '            Case DBOperation.DB_INSERT
    '                sSQL = "INSERT INTO " + sNameTable
    '                sSQL += "(" + sNameCol + ",COD_CONTRIBUENTE)"
    '                sSQL += " VALUES ( " & nIdCampo & "," & nIdContribuente & ")"

    '            Case DBOperation.DB_UPDATE
    '                sSQL = "UPDATE " + sNameTable + " SET"
    '                sSQL += " COD_CONTRIBUENTE=" & nIdContribuente
    '                sSQL += " WHERE " + sNameCol + "=" & nIdCampo
    '        End Select

    '        Return sSQL
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.GetSQLIntestarioUtente.errore: ", Err)

    '        Return ""
    'End Try
    'End Function

    'Public Function CreaContrattiFromContatori(ByVal sIdPeriodo As String, ByVal sIDEnte As String, ByVal nIDContatore As Integer) As Boolean
    '    Dim oListContatore As New objContatore
    '    Dim nContratto As Integer
    '    Dim oMyContratto As New objContratto
    '    Dim FncContratti As New GestContratti
    '    Dim oReplace As New ClsGenerale.Generale
    '    Dim sSQL As String
    '    Dim sqlConn As New SqlConnection

    '    Try
    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()
    '        'prelevo i dati da contatori
    '        nContratto = 0
    '        oListContatore = GetDetailsContatori(nIDContatore) ', sIDEnte)
    '        If Not oListContatore Is Nothing Then
    '            'valorizzo i dati del contratto partendo da contatori
    '            oMyContratto = New objContratto
    '            oMyContratto.nIdIntestatario = oListContatore.nIdIntestatario
    '            oMyContratto.nIdUtente = oListContatore.nIdUtente
    '            nContratto += 1
    '            oMyContratto.nIdContratto = "CMGC" & nContratto.ToString.PadLeft(5, "0")
    '            oMyContratto.sDataSottoscrizione = oReplace.FormattaData("G", "/", oListContatore.sDataAttivazione, False)

    '            'oMyContratto.oContatore.oDatiCatastali.sPiano = oListContatore.oDatiCatastali.sPiano
    '            'oMyContratto.oContatore.oDatiCatastali.sFoglio = oListContatore.oDatiCatastali.sFoglio
    '            'oMyContratto.oContatore.oDatiCatastali.sNumero = oListContatore.oDatiCatastali.sNumero
    '            'oMyContratto.oContatore.oDatiCatastali.nSubalterno = oListContatore.oDatiCatastali.nSubalterno
    '            oMyContratto.oContatore.oDatiCatastali = oListContatore.oDatiCatastali
    '            oMyContratto.oContatore.nCodIva = oListContatore.nCodIva
    '            oMyContratto.sIdEnte = oListContatore.sIdEnte
    '            oMyContratto.oContatore.nSpesa = oListContatore.nSpesa
    '            oMyContratto.oContatore.nDiametroContatore = oListContatore.nDiametroContatore
    '            oMyContratto.oContatore.nDiametroPresa = oListContatore.nDiametroPresa
    '            oMyContratto.oContatore.nDiritti = oListContatore.nDiritti
    '            oMyContratto.oContatore.nGiro = oListContatore.nGiro
    '            oMyContratto.oContatore.bIsPendente = oListContatore.bIsPendente
    '            oMyContratto.oContatore.nIdImpianto = oListContatore.nIdImpianto
    '            oMyContratto.oContatore.sSequenza = oListContatore.sSequenza
    '            oMyContratto.oContatore.nTipoContatore = oListContatore.nTipoContatore
    '            oMyContratto.oContatore.nPosizione = oListContatore.nPosizione
    '            oMyContratto.oContatore.sProgressivo = oListContatore.sProgressivo
    '            oMyContratto.oContatore.nProprietario = oListContatore.nProprietario
    '            oMyContratto.sNote = "CONTRATTO GENERATO DAL SISTEMA PARTENDO DAL CONTATORE"
    '            oMyContratto.oContatore.sDataAttivazione = oListContatore.sDataAttivazione
    '            oMyContratto.oContatore.nIdMinimo = oListContatore.nIdMinimo
    '            oMyContratto.oContatore.sLatoStrada = oListContatore.sLatoStrada
    '            oMyContratto.oContatore.bIgnoraMora = oListContatore.bIgnoraMora
    '            oMyContratto.oContatore.sIdEnteAppartenenza = oListContatore.sIdEnteAppartenenza
    '            oMyContratto.oContatore.sDataSospensioneUtenza = oListContatore.sDataSospensioneUtenza
    '            oMyContratto.oContatore.bUtenteSospeso = oListContatore.bUtenteSospeso
    '            oMyContratto.oContatore.sCodiceFabbricante = oListContatore.sCodiceFabbricante
    '            oMyContratto.oContatore.sCifreContatore = oListContatore.sCifreContatore
    '            oMyContratto.oContatore.sStatoContatore = oListContatore.sStatoContatore
    '            oMyContratto.oContatore.sPenalita = oListContatore.sPenalita
    '            oMyContratto.oContatore.sCodiceISTAT = oListContatore.sCodiceISTAT

    '            'inserisco il contratto
    '            If FncContratti.SetContratto(0, sIdPeriodo, oMyContratto, True) = False Then
    '                Return False
    '            End If
    '            'aggiorno il codice contratto sul contatore
    '            Dim oMyCmd As SqlCommand
    '            sSQL = "UPDATE TP_CONTATORI SET"
    '            sSQL += " CODCONTRATTO=" & oMyContratto.nIdContratto
    '            sSQL += " WHERE (TP_CONTATORI.CODCONTATORE=" & oListContatore.nIdContatore & ")"
    '            oMyCmd = New SqlCommand(sSQL, sqlConn)
    '            oMyCmd.ExecuteNonQuery()
    '        Else
    '            Return False
    '        End If

    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContatori.CreaContrattiFromContatori.errore: ", ex)
    '        Return False
    '    End Try
    'End Function

    Public Function GetInfoContatoreContribuente(ByVal sIdEnte As String, ByVal sCodContribuente As String) As DataView
        Dim sSQL As String
        Dim dvMyDati As DataView
        Dim NOME_DATABASE_H2O As String

        Try
            NOME_DATABASE_H2O = ConfigurationManager.AppSettings("NOME_DATABASE_H20")

            sSQL = "SELECT * FROM " & NOME_DATABASE_H2O & ".dbo.OPENgov_ELENCO_CONTATORI"
            sSQL += " WHERE (CODENTE = '" & sIdEnte & "')"
            sSQL += " AND (CODCONTRIBUENTE_UT = '" & sCodContribuente & "')"
            dvMyDati = iDB.GetDataView(sSQL)

            Return dvMyDati
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContatori.GetInfoContatoreContribuente.errore: ", Err)
            Return Nothing
        End Try
    End Function
End Class
