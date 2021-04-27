Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility

Public Class GestContratti
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestContratti))
    Dim iDB As New DBAccess.getDBobject
    Dim _Const As New Costanti
    Dim RaiseError As New GestioneFile
    Dim oReplace As New ClsGenerale.Generale
    Dim FncContatori As New GestContatori
    Private NomeDBAnagrafe As String = ConfigurationManager.AppSettings("NOME_DATABASE_ANAGRAFICA") & ".dbo"

    Enum DBOperation
        DB_INSERT = 1
        DB_UPDATE = 0
    End Enum

    'Public Function GetParametriResidenzaSpedizione(ByVal codcontribuente As Int64) As new dataview
    '    Try
    '        Dim sSQL As String
    '        Dim DR As new dataview

    '        sSQL = "SELECT * FROM " & NomeDBAnagrafe & ".INDIRIZZI_SPEDIZIONE "
    '        sSQL += " WHERE COD_CONTRIBUENTE=" & codcontribuente
    '        sSQL += " AND DATA_FINE_VALIDITA IS NULL OR DATA_FINE_VALIDITA=''"

    '        DR = iDB.getdataview(sSQL)
    '        Return DR

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetParametriResidenzaSpedizione.errore: ", ex)
    '    End Try

    'End Function

    'Public Function GetEnte(ByVal sCodiceISTAT As String) As String
    '    Dim Comune As String = ""
    '    Dim sSQL As String
    '    Dim dr As new dataview
    '    Try
    '        sSQL = "SELECT COMUNE"
    '        sSQL += " FROM TP_ENTE"
    '        sSQL += " WHERE (CODENTE = " & sCodiceISTAT & ")"
    '        dr = iDB.getdataview(sSQL)
    '        While dr.Read()
    '            Comune = dr("COMUNE")
    '        End While
    '        dr.Close()
    '        Return Comune
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetEnte.errore: ", ex)
    '    End Try
    'End Function

    'Public Function getParametriResidenza(ByVal codcontribuente As Int64) As new dataview
    '    Dim sSQL As String
    '    Dim DR As new dataview
    '    Try
    '        sSQL = "SELECT * FROM " & NomeDBAnagrafe & ".ANAGRAFICA"
    '        sSQL += " WHERE COD_CONTRIBUENTE=" & codcontribuente
    '        sSQL += " AND DATA_FINE_VALIDITA IS NULL OR DATA_FINE_VALIDITA=''"

    '        DR = iDB.getdataview(sSQL)

    '        Return DR
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetParametriResidenza.errore: ", ex)
    '    End Try
    'End Function

    'Public Function getDataTableStampaRiassuntiva(ByVal strutente As String, ByVal strintestatario As String,
    'ByVal codcontratto As Int32, ByVal ubicazione As Int16, ByVal stato As Int16) As DataTable
    '    Dim sSQL As String
    '    Dim oconn As New SqlConnection

    '    oconn.ConnectionString = ConstSession.StringConnection

    '    Try
    '        sSQL = "Select distinct tp_contratti.*, stradario.tipo_strada, stradario.strada,"
    '        sSQL += " tp_contratti.codice_istat, tp_contratti.sCivico_ubicazione, tp_contratti.cod_strada, tp_contratti.sDataAttivazione, tp_contratti.sdatasospensioneutenza,"
    '        sSQL += " tp_contratti.sesponentecivico,"
    '        sSQL += " ANA_UTENTE.COGNOME_DENOMINAZIONE AS COGNOME,  ANA_UTENTE.NOME AS NOME,"
    '        sSQL += "ANA_INTESTATARIO.COGNOME_DENOMINAZIONE AS COGNOME2, ANA_INTESTATARIO.NOME AS NOME2,"
    '        sSQL += " TP_TIPIUTENZA.DESCRIZIONE"

    '        'INNER
    '        sSQL += " FROM TP_CONTRATTI LEFT JOIN TR_CONTRATTI_UTENTE ON TP_CONTRATTI.CODCONTRATTO ="
    '        sSQL += " TR_CONTRATTI_UTENTE.CODCONTRATTO"

    '        'INNER
    '        sSQL += " LEFT Join TR_CONTRATTI_INTESTATARIO ON TP_CONTRATTI.CODCONTRATTO = TR_CONTRATTI_INTESTATARIO.CODCONTRATTO"

    '        sSQL += " LEFT JOIN STRADARIO  ON STRADARIO.COD_STRADA = TP_CONTRATTI.COD_STRADA "


    '        '=====================
    '        'UTENTE
    '        '=====================
    '        'INNER
    '        sSQL += " LEFT JOIN  " & NomeDBAnagrafe & ".ANAGRAFICA AS ANA_UTENTE"
    '        sSQL += " ON TR_CONTRATTI_UTENTE.COD_CONTRIBUENTE = ANA_UTENTE.COD_CONTRIBUENTE"
    '        '=====================
    '        'FINE UTENTE
    '        '=====================
    '        'INNER
    '        sSQL += " LEFT Join  " & NomeDBAnagrafe & ".ANAGRAFICA ANA_INTESTATARIO ON "
    '        sSQL += " TR_CONTRATTI_INTESTATARIO.COD_CONTRIBUENTE = ANA_INTESTATARIO.COD_CONTRIBUENTE"

    '        sSQL += " INNER JOIN TP_TIPIUTENZA ON TP_CONTRATTI.IDTIPOUTENZA = TP_TIPIUTENZA.IDTIPOUTENZA"

    '        sSQL += "  AND (ANA_UTENTE.DATA_FINE_VALIDITA IS NULL OR ANA_UTENTE.DATA_FINE_VALIDITA='')"

    '        sSQL += "  AND (ANA_INTESTATARIO.DATA_FINE_VALIDITA IS NULL OR ANA_INTESTATARIO.DATA_FINE_VALIDITA='')"

    '        Dim miocodice As String = ConstSession.CodIstat

    '        miocodice = Right(miocodice, Len(miocodice) - 2)



    '        Dim dt As DataTable
    '        Dim ds As DataSet

    '        ds = iDB.RunSQLReturnDataSet(sSQL)
    '        dt = ds.Tables(0)

    '        Return dt
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetDataTableStampaRiassuntiva.errore: ", ex)
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="strUtente"></param>
    ''' <param name="strIntestatario"></param>
    ''' <param name="CodContratto"></param>
    ''' <param name="nIdVia"></param>
    ''' <param name="Stato"></param>
    ''' <param name="NomeIntestatario"></param>
    ''' <param name="NomeUtente"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetListaContratti(ByVal sIdEnte As String, ByVal strUtente As String, ByVal strIntestatario As String, ByVal CodContratto As String, ByVal nIdVia As Integer, ByVal Stato As Integer, ByVal NomeIntestatario As String, ByVal NomeUtente As String) As DataSet
        Dim sSQL As String = ""
        Dim dsMyDati As New DataSet
        Try
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetElencoContratti", "IDENTE", "CODICECONTRATTO", "IDVIA", "COGNOMEINT", "NOMEINT", "COGNOMEUTE", "NOMEUTE", "STATO")
                dsMyDati = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                        , ctx.GetParam("CODICECONTRATTO", CodContratto) _
                        , ctx.GetParam("IDVIA", nIdVia) _
                        , ctx.GetParam("COGNOMEINT", strIntestatario) _
                        , ctx.GetParam("NOMEINT", NomeIntestatario) _
                        , ctx.GetParam("COGNOMEUTE", strUtente) _
                        , ctx.GetParam("NOMEUTE", NomeUtente) _
                        , ctx.GetParam("STATO", Stato)
                    )
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(sIdEnte + " - OPENgovH2O.GestContratti.GetListaContratti.errore: ", ex)
        End Try
        Return dsMyDati
    End Function
    'Public Function GetListaContratti(ByVal sIdEnte As String, ByVal strUtente As String, ByVal strIntestatario As String, ByVal CodContratto As String, ByVal nIdVia As Integer, ByVal Stato As Integer, ByVal NomeIntestatario As String, ByVal NomeUtente As String) As DataSet
    '    Dim sSQL As String
    '    Dim GetLista As New objDBListSQL
    '    Dim oConn As New SqlConnection
    '    Dim oCmd As New SqlCommand

    '    GetLista.oConn = oConn
    '    GetLista.oComm = oCmd
    '    oConn.ConnectionString = ConstSession.StringConnection
    '    Try
    '        sSQL = "SELECT DISTINCT COGNOMEINT, NOMEINT, COGNOMEUT, NOMEUT"
    '        sSQL += " , VIA_UBICAZIONE, CIVICO_UBICAZIONE, ESPONENTE_CIVICO"
    '        sSQL += " , CODCONTRATTO, CODICECONTRATTO, DATASOTTOSCRIZIONE, DATACESSAZIONE"
    '        sSQL += " FROM OPENGOV_ELENCO_CONTRATTI"
    '        sSQL += " WHERE 1=1 "
    '        sSQL += " AND (CODENTE=" & sIdEnte & ")"
    '        If Len(CodContratto) > 0 Then
    '            sSQL += " AND CODICECONTRATTO='" & CodContratto & "' "
    '        End If
    '        If nIdVia <> -1 Then
    '            sSQL += " AND COD_STRADA=" & nIdVia & " "
    '        End If
    '        If Len(Trim(strIntestatario)) > 0 Then
    '            sSQL += " AND (COGNOMEINT LIKE '" & Replace(Replace(Trim(strIntestatario), "'", "''"), "*", "%") & "%')"
    '        End If
    '        If Len(Trim(NomeIntestatario)) > 0 Then
    '            sSQL += " AND (NOMEINT LIKE '" & Replace(Replace(Trim(NomeIntestatario), "'", "''"), "*", "%") & "%')"
    '        End If
    '        If Len(Trim(strUtente)) > 0 Then
    '            sSQL += " AND (COGNOMEUT LIKE  '" & Replace(Replace(Trim(strUtente), "'", "''"), "*", "%") & "%')"
    '        End If
    '        If Len(Trim(NomeUtente)) > 0 Then
    '            sSQL += " AND (NOMEUT LIKE '" & Replace(Replace(Trim(NomeUtente), "'", "''"), "*", "%") & "%')"
    '        End If
    '        If Stato >= 1 Then
    '            sSQL += " AND (STATO=" & Stato & ") "
    '        End If
    '        sSQL += " ORDER BY COGNOMEINT,NOMEINT,COGNOMEUT,NOMEUT"

    '        GetLista.Query = sSQL

    '        Dim DBACCESS As New DBAccess.getDBobject
    '        Dim DS As DataSet
    '        DS = DBACCESS.RunSQLReturnDataSet(GetLista.Query)

    '        GetLista.RecordCount = DS.Tables(0).Rows.Count
    '        Return DS
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetListaContratti.errore: ", ex)
    '    End Try

    'End Function

    'Public Overloads Function GetListaContatori(ByVal codcontratto As Int32, ByVal ubicazione As Int16, ByVal boolpendente As Boolean) As objDBListSQL

    '    Dim Query As String
    '    Dim QueryCount As String
    '    Dim QueryWhere As String
    '    Dim QueryFrom As String
    '    Dim SearchChar As String = "*"
    '    Dim intPosString As Integer


    '    Dim GetLista As New objDBListSQL
    ' Try
    '    GetLista.oConn = DBAccess.GetConnectionGrid


    '    Query = ""
    '    Query = "SELECT DISTINCT  TP_CONTATORI.*, STRADARIO.TIPO_STRADA,STRADARIO.STRADA" & vbCrLf
    '    QueryFrom = QueryFrom & "FROM TP_CONTATORI INNER JOIN STRADARIO  ON STRADARIO.COD_STRADA = TP_CONTATORI.COD_STRADA" & vbCrLf
    '    QueryWhere = "WHERE 1=1" & vbCrLf



    '    'ale cao pendente
    '    If boolpendente = True Then
    '        QueryWhere = QueryWhere & "AND TP_CONTATORI.PENDENTE=1 "
    '    End If
    '    'fine ale cao
    '    If ubicazione <> -1 Then
    '        QueryWhere = QueryWhere & "AND TP_CONTATORI.COD_STRADA=" & ubicazione & vbCrLf
    '    End If

    '    If Len(Trim(codcontratto)) > 0 Then
    '        QueryWhere = QueryWhere & "and codcontratto=" & codcontratto & vbCrLf
    '    End If



    '    GetLista.Query = Query & QueryFrom & QueryWhere
    '    GetLista.QueryCount = "Select Count(*) AS NUMERORECORD " & QueryFrom & QueryWhere

    '    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    '    'Ritorno il Record Count
    '    Dim drTemp As new dataview
    '    drTemp = DBAccess.getdataview(GetLista.QueryCount)
    '    If drTemp.Read Then
    '        GetLista.RecordCount = utility.stringoperation.formatint(drTemp.Item("NUMERORECORD"))
    '    End If
    '    drTemp.Close()
    '    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/'
    '
    '   Catch ex As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetListaContatori.errore: ", ex)
    ' End Try
    '    Return GetLista


    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idContratto"></param>
    ''' <returns>True=esiste un contatore associato attivo;False=non esiste un contatore associato attivo</returns>
    Public Function getEsistonoContatori(ByVal idContratto As Integer) As Boolean
        Dim sSQL As String
        Dim attivo As Boolean = False
        Dim dvMyDati As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetEsistonoContatori", "IDCONTRATTO")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDCONTRATTO", idContratto))
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            attivo = Utility.StringOperation.FormatBool(myRow("attivo"))
                        Next
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + " - OPENgovH2O.GestContratti.GetLastCodContratto.errore: ", ex)
                Finally
                    dvMyDati.Dispose()
                End Try
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContratti.GetEsistonoContatori.errore: ", ex)
        End Try
        Return attivo
    End Function

    'Public Function getContatoreAssociato(ByVal IDContratto As Int32) As Int32
    '    Dim IDEstratto As Int32
    '    Dim sSQL As String
    '    Try
    '        sSQL = "SELECT CODCONTATORE FROM TP_CONTATORI WHERE CODCONTRATTO=" & IDContratto
    '        Dim drID As new dataview
    '        drID = iDB.getdataview(sSQL)

    '        While drID.Read
    '            IDEstratto = drID("CODCONTATORE")
    '        End While

    '        drID.Close()
    '        Return IDEstratto
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetContatoreAssociato.errore: ", ex)
    '    End Try
    'End Function

    'Public Function GetLastCodContratto(ByVal sIdEnte As String, ByVal WFSessione As OPENUtility.CreateSessione) As String
    '    Dim sSQL, sLastContratto As String
    '    Dim DrDati As SqlClient.new dataview

    '    Try
    '        sSQL = "SELECT TOP 1 CODICECONTRATTO, DATASOTTOSCRIZIONE"
    '        sSQL += " FROM TP_CONTRATTI"
    '        sSQL += " WHERE (CODENTE='" & sIdEnte & "')"
    '        sSQL += " ORDER BY DATASOTTOSCRIZIONE DESC"
    '        'eseguo la query
    '        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While DrDati.Read
    '            sLastContratto = "N.Contratto " & CStr(DrDati("codicecontratto"))
    '            sLastContratto += " del " & oReplace.GiraDataFromDB(CStr(DrDati("datasottoscrizione")))
    '        Loop

    '        Return sLastContratto
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetLastCodContratto.errore: ", Err) 
    '        Return ""
    '    Finally
    '        DrDati.Close()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetLastCodContratto(ByVal sIdEnte As String) As String
        Dim sLastContratto As String = ""
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetLastContratto", "IDENTE")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte))
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            sLastContratto = "N.Contratto " & StringOperation.FormatString(myRow("codicecontratto"))
                            sLastContratto += " del " & StringOperation.FormatString(myRow("datasottoscrizione"))
                        Next
                    End If
                Catch ex As Exception
                    Log.Debug(sIdEnte + " - OPENgovH2O.GestContratti.GetLastCodContratto.errore: ", ex)
                Finally
                    dvMyDati.Dispose()
                End Try
                ctx.Dispose()
            End Using
        Catch Err As Exception
            Log.Debug(sIdEnte + " - OPENgovH2O.GestContratti.GetLastCodContratto.errore: ", Err)
            sLastContratto = ""
        End Try
        Return sLastContratto
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IDContratto"></param>
    ''' <param name="CodEnte"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetDetailsContratti(ByVal IDContratto As Integer, ByVal CodEnte As String) As objContratto
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim lgnTipoOperazione As Long = DBOperation.DB_UPDATE
        Dim oMyContatore As New objContatore
        Dim DetailsContratto As New objContratto

        Dim DBContatori As GestContatori = New GestContatori

        Try
            If IDContratto = 0 Then lgnTipoOperazione = DBOperation.DB_INSERT
            If lgnTipoOperazione = DBOperation.DB_UPDATE Then
                Try
                    Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                        Try
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "DetailContatoriNEW", "CODCONTRATTO")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODCONTRATTO", IDContratto))
                            If Not dvMyDati Is Nothing Then
                                For Each myRow As DataRowView In dvMyDati
                                    'prelevo i dati del contatore
                                    oMyContatore = DBContatori.GetDetailsContatori(StringOperation.FormatInt(myRow("codcontatore")), -1)
                                Next
                            End If
                        Catch ex As Exception
                            Log.Debug(CodEnte + " - OPENgovH2O.GestContratti.GetDetailsContratti.DetailContatoriNEW.errore: ", ex)
                        Finally
                            dvMyDati.Dispose()
                        End Try
                        Try
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "DetailContratti", "CODCONTRATTO")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODCONTRATTO", IDContratto))
                            If Not dvMyDati Is Nothing Then
                                For Each myRow As DataRowView In dvMyDati
                                    DetailsContratto = New objContratto
                                    DetailsContratto.nIdContratto = StringOperation.FormatInt(myRow("CODCONTRATTO"))
                                    If DetailsContratto.nIdContratto = -1 Then
                                        DetailsContratto.nIdContratto = 0
                                    End If
                                    DetailsContratto.sCodiceContratto = StringOperation.FormatString(myRow("CODICECONTRATTO"))
                                    DetailsContratto.sDataSottoscrizione = oReplace.GiraDataFromDB(Utility.StringOperation.FormatString(myRow("DATASOTTOSCRIZIONE")))
                                    DetailsContratto.bIsRichiestaSub = StringOperation.FormatString(myRow("richiestasub"))
                                    DetailsContratto.sNoteRichiestaSub = StringOperation.FormatString(myRow("NOTERICHIESTASUB"))
                                    DetailsContratto.sNote = StringOperation.FormatString(myRow("NOTE"))
                                    DetailsContratto.sDataCessazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("DATACESSAZIONE")))

                                    DetailsContratto.oContatore = oMyContatore
                                Next
                            End If
                        Catch ex As Exception
                            Log.Debug(CodEnte + " - OPENgovH2O.GestContratti.GetDetailsContratti.DetailContratti.errore: ", ex)
                        Finally
                            dvMyDati.Dispose()
                        End Try
                        Try
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "DetailAnagraficaIntestatarioCONTR", "CODCONTRATTO")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODCONTRATTO", IDContratto))
                            If Not dvMyDati Is Nothing Then
                                For Each myRow As DataRowView In dvMyDati
                                    DetailsContratto.nIdIntestatario = StringOperation.FormatString(myRow("COD_CONTRIBUENTE"))
                                Next
                            End If
                        Catch ex As Exception
                            Log.Debug(CodEnte + " - OPENgovH2O.GestContratti.GetDetailsContratti.DetailAnagraficaIntestatarioCONTR.errore: ", ex)
                        Finally
                            dvMyDati.Dispose()
                        End Try
                        Try
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "DetailAnagraficaUtenteCONTR", "CODCONTRATTO")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODCONTRATTO", IDContratto))
                            If Not dvMyDati Is Nothing Then
                                For Each myRow As DataRowView In dvMyDati
                                    DetailsContratto.nIdUtente = StringOperation.FormatString(myRow("COD_CONTRIBUENTE"))
                                Next
                            End If
                        Catch ex As Exception
                            Log.Debug(CodEnte + " - OPENgovH2O.GestContratti.GetDetailsContratti.DetailAnagraficaUtenteCONTR.errore: ", ex)
                        Finally
                            dvMyDati.Dispose()
                        End Try
                        ctx.Dispose()
                    End Using
                Catch ex As Exception
                    Log.Debug(CodEnte + " - OPENgovH2O.GestContratti.GetDetailsContratti.errore: ", ex)
                Finally
                    dvMyDati.Dispose()
                End Try
            Else
                DetailsContratto.oContatore = oMyContatore
            End If
            Return DetailsContratto
        Catch ex As Exception
            Log.Debug(CodEnte + " - OPENgovH2O.GestContratti.GetDetailsContratti.errore: ", ex)
            Return Nothing
        End Try
    End Function
    'Public Function GetDetailsContratti(ByVal IDContratto As Integer, ByVal CodEnte As String) As objContratto
    '    Dim DetailsContratto As New objContratto
    '    Dim lgnTipoOperazione As Long = DBOperation.DB_UPDATE
    '    Dim oMyContatore As New objContatore
    '    Dim DBContatori As GestContatori = New GestContatori

    '    Try
    '        If IDContratto = 0 Then lgnTipoOperazione = DBOperation.DB_INSERT
    '        If lgnTipoOperazione = DBOperation.DB_UPDATE Then
    '            Dim drDetailsContatore As new dataview
    '            drDetailsContatore = iDB.RunSPReturnRS("DetailContatoriNEW", New SqlParameter("@CodContratto", IDContratto))
    '            If drDetailsContatore.Read Then
    '                'prelevo i dati del contatore
    '                oMyContatore = DBContatori.GetDetailsContatori(CInt(drDetailsContatore("codcontatore")), -1) ', CodEnte)
    '            End If
    '            drDetailsContatore.Close()

    '            Dim drDetailsContratto As new dataview
    '            drDetailsContratto = iDB.RunSPReturnRS("DetailContratti", New SqlParameter("@CodContratto", IDContratto))
    '            If drDetailsContratto.Read Then
    '                DetailsContratto.nIdContratto = MyUtility.CIdFromDB(drDetailsContratto("CODCONTRATTO"))
    '                If DetailsContratto.nIdContratto = -1 Then
    '                    DetailsContratto.nIdContratto = 0
    '                End If
    '                DetailsContratto.sCodiceContratto = Utility.StringOperation.FormatString(drDetailsContratto("CODICECONTRATTO"))
    '                DetailsContratto.sDataSottoscrizione = oReplace.GiraDataFromDB(Utility.StringOperation.FormatString(drDetailsContratto("DATASOTTOSCRIZIONE")))
    '                DetailsContratto.bIsRichiestaSub = drDetailsContratto("richiestasub")
    '                DetailsContratto.sNoteRichiestaSub = Utility.StringOperation.FormatString(drDetailsContratto("NOTERICHIESTASUB"))
    '                DetailsContratto.sNote = Utility.StringOperation.FormatString(drDetailsContratto("NOTE"))
    '                DetailsContratto.sDataCessazione = oReplace.GiraDataFromDB(Utility.StringOperation.FormatString(drDetailsContratto("DATACESSAZIONE")))

    '                DetailsContratto.oContatore = oMyContatore
    '            End If

    '            Dim drDetailsAnagrafica As new dataview
    '            drDetailsAnagrafica = iDB.RunSPReturnRS("DetailAnagraficaIntestatarioCONTR", New SqlParameter("@CodContratto", IDContratto))
    '            If drDetailsAnagrafica.Read Then
    '                DetailsContratto.nIdIntestatario = Utility.StringOperation.FormatString(drDetailsAnagrafica("COD_CONTRIBUENTE"))
    '            End If
    '            drDetailsAnagrafica.Close()
    '            drDetailsAnagrafica = iDB.RunSPReturnRS("DetailAnagraficaUtenteCONTR", New SqlParameter("@CodContratto", IDContratto))
    '            If drDetailsAnagrafica.Read Then
    '                DetailsContratto.nIdUtente = Utility.StringOperation.FormatString(drDetailsAnagrafica("COD_CONTRIBUENTE"))
    '            End If
    '            drDetailsAnagrafica.Close()
    '        Else
    '            DetailsContratto.oContatore = oMyContatore
    '        End If
    '        Return DetailsContratto
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetDetailsContratti.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function

    '    Public Sub SetContrattoVECCHIO(ByVal CodContratto As String, ByVal CodEnte As String, ByVal CodEnteAppartenenza As String, ByVal idImpianto As Integer, _
    'ByVal idGiro As Integer, ByVal Sequenza As String, ByVal CodPosizione As Integer, ByVal PosizioneProgressiva As String, ByVal LatoStrada As String, ByVal idTipoContatore As Integer, _
    'ByVal CodFognatura As Integer, ByVal CodDepurazione As Integer, _
    'ByVal EsenteFognatura As Boolean, ByVal EsenteDepurazione As Boolean, ByVal IgnoraMora As Boolean, ByVal Note As String, ByVal DataAttivazione As String, _
    ' ByVal NumeroUtenze As String, ByVal idTipoUtenza As Integer, ByVal idDiametroContatore As Integer, ByVal idDiametroPresa As Integer, _
    '   ByVal ubicazione As String, ByVal civico As String, ByVal idVia As Integer, _
    '   ByVal hdDataSottoScrizione As String, ByVal hdConsumoMinimo As String, _
    '    ByVal hdIdDiametroPresaContratto As Integer, _
    '   ByVal UtenteSospeso As Boolean, ByVal CodiceFabbricante As String, _
    '  ByVal CifreContatore As String, ByVal CodIva As Integer, ByVal StatoContatore As String, ByVal Penalita As String, ByVal CodiceISTAT As String, ByVal Esponente As String, ByVal IDMINIMO As Integer, ByVal IDTIPOATTIVITA As Integer, ByVal CODICEIMPIANTO As Integer)

    '        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    '        dim sSQL as string
    '        Dim lngTipoOp As Long
    '        Dim IDValue As Long
    '        Dim drTemp As new dataview
    '        Dim blnPrincipale As Boolean
    '        Dim oProcess As Object
    '        Dim valore As String
    '        Dim strAppoggio As String
    '        Dim strValue As String

    '        Dim sqlTrans As SqlTransaction
    '        Dim sqlConn As New SqlConnection

    '        lngTipoOp = DBOperation.DB_INSERT

    '        If CInt(CodContratto) > 0 Then
    '            lngTipoOp = DBOperation.DB_UPDATE
    '        End If

    '        If lngTipoOp = DBOperation.DB_INSERT Then

    '            '************************************************************************************
    '            sSQL="SELECT MAX(CODCONTRATTO) FROM TP_CONTRATTI" & vbCrLf
    '            IDValue = DBAccess.RunActionQueryIdentiy(sSQL)

    '            IDValue = IDValue + 1


    '            '************************************************************************************


    '            sSQL="INSERT INTO TP_CONTATORI" & vbCrLf
    '            sSQL+="(CODCONTRATTO,CODENTE1,CODENTEAPPARTENENZA1,CODIMPIANTO,IDGIRO," & vbCrLf
    '            sSQL+="SEQUENZA,CODPOSIZIONE,POSIZIONEPROGRESSIVA,LATOSTRADA,IDTIPOCONTATORE, " & vbCrLf
    '            sSQL+="CODFOGNATURA,CODDEPURAZIONE,ESENTEFOGNATURA,ESENTEDEPURAZIONE, " & vbCrLf
    '            sSQL+="IGNORAMORA,NOTE,DATAATTIVAZIONE,NUMEROUTENZE," & vbCrLf
    '            sSQL+="IDTIPOUTENZA,CODDIAMETROCONTATORE,CODDIAMETROPRESA,COD_STRADA,CIVICO_UBICAZIONE," & vbCrLf
    '            sSQL+="UTENTESOSPESO,CODICEFABBRICANTE," & vbCrLf
    '            sSQL+="CIFRECONTATORE,CODIVA,STATOCONTATORE,PENALITA,CODICE_ISTAT,ESPONENTE_CIVICO,IDMINIMO,IDTIPOATTIVITA)" & vbCrLf
    '            sSQL+="VALUES ( " & vbCrLf
    '            sSQL+= utility.stringoperation.formatint(IDValue) & "," & vbCrLf

    '            sSQL+= utility.stringoperation.formatstring(CodEnte) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatstring(CodEnteAppartenenza) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatint(idImpianto) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatint(idGiro) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatstring(Sequenza) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatint(CodPosizione) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatstring(PosizioneProgressiva) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatstring(LatoStrada) & "," & vbCrLf

    '            sSQL+= utility.stringoperation.formatint(idTipoContatore) & "," & vbCrLf

    '            sSQL+= utility.stringoperation.formatint(CodFognatura) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatint(CodDepurazione) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatbool(EsenteFognatura) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatbool(EsenteDepurazione) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatbool(IgnoraMora) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatstring(Note) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatstring(oReplace.GiraData(DataAttivazione)) & "," & vbCrLf

    '            sSQL+= CInt(NumeroUtenze) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatint(idTipoUtenza) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatint(idDiametroContatore) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatint(idDiametroPresa) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatint(idVia) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatstring(civico) & "," & vbCrLf

    '            sSQL+= utility.stringoperation.formatbool(UtenteSospeso) & "," & vbCrLf

    '            sSQL+= utility.stringoperation.formatstring(CodiceFabbricante) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatstring(CifreContatore) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatint(CodIva) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatstring(StatoContatore) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatstring(Penalita) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatstring(CodiceISTAT) & "," & vbCrLf



    '            ' '====================================================================================
    '            ' 'PER PRODUZIONE
    '            ' '====================================================================================
    '            ' 'Prelevo gli Ultimi 3 caratteri del codice impianto
    '            strAppoggio = Right(utility.stringoperation.formatstring(CODICEIMPIANTO), 3)
    '            strAppoggio = strAppoggio.PadLeft(3, "0")

    '            sSQL+= utility.stringoperation.formatstring(UCase(strAppoggio)) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatstring(UCase(strAppoggio)) & "," & vbCrLf


    '            sSQL+= utility.stringoperation.formatstring(UCase(Esponente)) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatint(IDMINIMO) & "," & vbCrLf
    '            sSQL+= utility.stringoperation.formatint(IDTIPOATTIVITA) & vbCrLf

    '            sSQL+=" )"

    '            Try

    '                Dim sqlCmdInsert As SqlCommand

    '                sqlConn.ConnectionString = ConstSession.StringConnection
    '                sqlConn.Open()

    '                sqlTrans = sqlConn.BeginTransaction

    '                sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)

    '                sqlCmdInsert.ExecuteNonQuery()





    '                sqlTrans.Commit()

    '            Catch er As Exception
    '           Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.SetContrattoVECCHIO.errore: ", er)
    '                sqlTrans.Rollback()
    '                RaiseError.trace(er, sSql.Replace(vbCrLf, ""), ConfigurationManager.AppSettings("Filename"), ConfigurationManager.AppSettings("FilePath"), "")
    '                Throw
    '            Finally
    '                sqlConn.Close()
    '            End Try


    '        End If
    '        If lngTipoOp = DBOperation.DB_UPDATE Then

    '            sSQL="UPDATE TP_CONTRATTI SET "

    '            sSQL+=", CODENTE1 =" & utility.stringoperation.formatstring(CodEnte)
    '            sSQL+=", CODENTEAPPARTENENZA1=" & utility.stringoperation.formatstring(CodEnteAppartenenza)
    '            sSQL+=", CODIMPIANTO =" & utility.stringoperation.formatint(idImpianto)
    '            sSQL+=", IDGIRO =" & utility.stringoperation.formatint(idGiro)
    '            sSQL+=", SEQUENZA =" & utility.stringoperation.formatstring(Sequenza)
    '            sSQL+=", CODPOSIZIONE =" & utility.stringoperation.formatint(CodPosizione)
    '            sSQL+=", POSIZIONEPROGRESSIVA =" & utility.stringoperation.formatstring(PosizioneProgressiva)
    '            sSQL+=", LATOSTRADA =" & utility.stringoperation.formatstring(LatoStrada)

    '            sSQL+=", IDTIPOCONTATORE =" & utility.stringoperation.formatint(idTipoContatore)

    '            sSQL+=", CODFOGNATURA =" & utility.stringoperation.formatint(CodFognatura)
    '            sSQL+=", CODDEPURAZIONE =" & utility.stringoperation.formatint(CodDepurazione)
    '            sSQL+=", ESENTEFOGNATURA =" & utility.stringoperation.formatbool(EsenteFognatura)
    '            sSQL+=", ESENTEDEPURAZIONE =" & utility.stringoperation.formatbool(EsenteDepurazione)
    '            sSQL+=", IGNORAMORA = " & utility.stringoperation.formatbool(IgnoraMora)
    '            sSQL+=", NOTE= " & utility.stringoperation.formatstring(Note)
    '            sSQL+=", DATAATTIVAZIONE= " & utility.stringoperation.formatstring(oReplace.GiraData(DataAttivazione))

    '            sSQL+=", NUMEROUTENZE= " & CInt(NumeroUtenze)
    '            sSQL+=", IDTIPOUTENZA =" & utility.stringoperation.formatint(idTipoUtenza)
    '            sSQL+=", CODDIAMETROCONTATORE =" & utility.stringoperation.formatint(idDiametroContatore)
    '            sSQL+=", COD_STRADA =" & utility.stringoperation.formatint(idVia)
    '            sSQL+=", CIVICO_UBICAZIONE =" & utility.stringoperation.formatstring(civico)

    '            sSQL+=", UTENTESOSPESO =" & utility.stringoperation.formatbool(UtenteSospeso)

    '            sSQL+=", CODICEFABBRICANTE =" & utility.stringoperation.formatstring(CodiceFabbricante)
    '            sSQL+=", CIFRECONTATORE =" & utility.stringoperation.formatstring(CifreContatore)
    '            sSQL+=", CODIVA =" & utility.stringoperation.formatint(CodIva)
    '            sSQL+=", STATOCONTATORE =" & utility.stringoperation.formatstring(StatoContatore)
    '            sSQL+=", PENALITA =" & utility.stringoperation.formatstring(Penalita)




    '            '====================================================================================
    '            'PER PRODUZIONE
    '            '====================================================================================
    '            'Prelevo gli Ultimi 3 caratteri del codice impianto
    '            strAppoggio = Right(utility.stringoperation.formatstring(CODICEIMPIANTO), 3)
    '            strAppoggio = strAppoggio.PadLeft(3, "0")


    '            sSQL+=", ESPONENTE_CIVICO =" & utility.stringoperation.formatstring(UCase(Esponente))
    '            sSQL+=", IDMINIMO =" & utility.stringoperation.formatint(IDMINIMO)
    '            sSQL+=", IDTIPOATTIVITA =" & utility.stringoperation.formatint(IDTIPOATTIVITA)
    '            sSQL+=" WHERE CODCONTRATTO =" & utility.stringoperation.formatstring(CodContratto)

    '            Try

    '                Dim sqlCmdInsert As SqlCommand
    '                sqlConn.ConnectionString = ConstSession.StringConnection

    '                sqlConn.Open()
    '                sqlTrans = sqlConn.BeginTransaction
    '                sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)


    '                sqlCmdInsert.ExecuteNonQuery()

    '                sSQL=""
    '                sSQL="UPDATE TR_CONTRATTI_INTESTATARIO SET" & vbCrLf
    '                sSQL+="COD_CONTRIBUENTE=" & 2 & vbCrLf
    '                sSQL+="WHERE CODCONTRATTO =" & utility.stringoperation.formatstring(CodContratto)

    '                sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                sqlCmdInsert.ExecuteNonQuery()

    '                sSQL=""
    '                sSQL="UPDATE TR_CONTRATTI_UTENTE SET" & vbCrLf
    '                sSQL+="COD_CONTRIBUENTE=" & 2 & vbCrLf
    '                sSQL+="WHERE CODCONTRATTO =" & utility.stringoperation.formatstring(CodContratto)

    '                sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                sqlCmdInsert.ExecuteNonQuery()

    '                sqlTrans.Commit()

    '            Catch er As Exception
    '              Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.SetContrattoVECCHIO.errore: ", er)
    '                sqlTrans.Rollback()
    '                RaiseError.trace(er, sSql.Replace(vbCrLf, ""), ConfigurationManager.AppSettings("Filename"), ConfigurationManager.AppSettings("FilePath"), "")
    '                Throw
    '            Finally
    '                sqlConn.Close()
    '            End Try
    '        End If

    '    End Sub

    'Public Overloads Function getListaDocumenti(ByVal codcontratto As Int32) As objDBListSQL
    '    dim sSQL as string
    '    Dim queryFrom As String
    '    Dim queryWhere As String
    '    Dim queryOrder As String
    '    Dim getLista2 As New objDBListSQL
    '    Dim oConn As New SqlConnection
    '    Dim oCmd As New SqlCommand

    '    getLista2.oConn = oConn
    '    getLista2.oComm = oCmd
    '    oConn.ConnectionString = ConstSession.StringConnection

    '    Dim sqlCmdInsert As SqlCommand
    'Try

    '    sSQL=""
    '    queryFrom = ""
    '    queryWhere = ""
    '    queryOrder = ""

    '    sSQL+="SELECT LINK,"
    '    sSQL+=" DATA,"
    '    sSQL+=" DESCRIZIONE, TR_CONTRATTI_DOCUMENTI.CODCONTRATTO AS MIOCODICE"

    '    queryFrom = " FROM TP_DOCUMENTI"
    '    queryFrom = queryFrom & " INNER JOIN TR_CONTRATTI_DOCUMENTI"
    '    queryFrom = queryFrom & " ON TP_DOCUMENTI.CODDOCUMENTO = TR_CONTRATTI_DOCUMENTI.CODDOCUMENTO"

    '    queryWhere = " WHERE TR_CONTRATTI_DOCUMENTI.CODCONTRATTO = " & codcontratto

    '    queryOrder = " ORDER BY DESCRIZIONE, DATA"



    '    getLista2.Query = sSQL & queryFrom & queryWhere & queryOrder
    '    getLista2.QueryCount = "Select count(*) as NUMERORECORD"
    '    getLista2.QueryCount = getLista2.QueryCount & queryFrom
    '    getLista2.QueryCount = getLista2.QueryCount & queryWhere


    '    Dim drdocumenti As new dataview
    '    drdocumenti = iDB.getdataview(getLista2.QueryCount)
    '    If drdocumenti.Read Then
    '        getLista2.RecordCount = utility.stringoperation.formatint(drdocumenti.Item("NUMERORECORD"))
    '    End If
    '    drdocumenti.Close()

    '    oConn.Open()


    '    Return getLista2
    'Catch Err As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetListaDocumenti.errore: ",Err)
    'End Try
    'End Function
    Public Function getListaDocumenti(ByVal codcontratto As Integer) As DataView
        Dim myDv As DataView = Nothing
        Dim cmdMyCommand As New SqlCommand
        Try
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "SELECT LINK,DATA,DESCRIZIONE, TR_CONTRATTI_DOCUMENTI.CODCONTRATTO AS MIOCODICE"
            cmdMyCommand.CommandText += " FROM TP_DOCUMENTI"
            cmdMyCommand.CommandText += " INNER JOIN TR_CONTRATTI_DOCUMENTI ON TP_DOCUMENTI.CODDOCUMENTO = TR_CONTRATTI_DOCUMENTI.CODDOCUMENTO"
            cmdMyCommand.CommandText += " WHERE TR_CONTRATTI_DOCUMENTI.CODCONTRATTO = " & codcontratto
            cmdMyCommand.CommandText += " ORDER BY DESCRIZIONE, DATA"
            myDv = iDB.GetDataView(cmdMyCommand)
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContratti.GetListaDocumenti.errore: ", ex)
            myDv = Nothing
        End Try
        Return myDv
    End Function

    'Public Sub setDocumentoPreventivo(ByVal LinkDocumento As String, ByVal codcontratto As Int32)
    '    Dim sSQL As String
    '    Dim mioMassimo As Int32
    '    Dim sqlConn As New SqlConnection
    '    Dim sqlTrans As SqlTransaction

    '    Dim data As String = DateTime.Now.ToString("yyyyMMdd HH:mm:ss").Replace(".", ":")

    '    sSQL = ""
    '    sSQL = "INSERT INTO TP_DOCUMENTI"
    '    sSQL += "(LINK,"
    '    sSQL += " DATA,"
    '    sSQL += " DESCRIZIONE)"
    '    sSQL += " VALUES ("
    '    sSQL += "'" & LinkDocumento & "',"
    '    sSQL += "'" & data & "',"
    '    sSQL += "'preventivo')"
    '    sSQL += "select @@IDENTITY as MAX_CODDOCUMENTO "
    '    Try
    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()

    '        Dim drDocumenti As new dataview
    '        drDocumenti = iDB.getdataview(sSQL)

    '        If drDocumenti.Read Then
    '            mioMassimo = drDocumenti("MAX_CODDOCUMENTO")
    '        End If
    '        drDocumenti.Close()

    '        sqlConn.Close()

    '        sSQL = ""

    '        sSQL = "INSERT INTO TR_CONTRATTI_DOCUMENTI"
    '        sSQL += "(CODCONTRATTO,"
    '        sSQL += "CODDOCUMENTO)"
    '        sSQL += " VALUES ("
    '        sSQL += codcontratto & ","
    '        sSQL += mioMassimo & ")"


    '        Try

    '            Dim sqlCmdInsert2 As SqlCommand

    '            sqlConn.ConnectionString = ConstSession.StringConnection
    '            sqlConn.Open()

    '            sqlTrans = sqlConn.BeginTransaction

    '            sqlCmdInsert2 = New SqlCommand(sSQL, sqlConn, sqlTrans)

    '            sqlCmdInsert2.ExecuteNonQuery()

    '            sqlTrans.Commit()

    '        Catch er As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.setDocumentoPreventivo.errore: ", er)
    '            sqlTrans.Rollback()
    '            RaiseError.trace(er, sSQL.Replace(vbCrLf, ""), ConfigurationManager.AppSettings("Filename"), ConfigurationManager.AppSettings("FilePath"), "")
    '            Throw
    '        Finally
    '            sqlConn.Close()
    '        End Try

    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.setDocumentoPreventivo.errore: ", Err)
    '    End Try

    'End Sub


    'Public Sub SetDocumentoContratto(ByVal LinkDocumento As String, ByVal codcontratto As Int32)
    '    Dim sSQL As String
    '    Dim mioMassimo As Int32
    '    Dim sqlTrans As SqlTransaction
    '    Dim sqlConn As New SqlConnection


    '    Dim data As String = DateTime.Now.ToString("yyyyMMdd HH:mm:ss").Replace(".", ":")

    '    sSQL = ""

    '    sSQL = "INSERT INTO TP_DOCUMENTI"
    '    sSQL += "(LINK,"
    '    sSQL += " DATA,"
    '    sSQL += " DESCRIZIONE)"
    '    sSQL += " VALUES ("
    '    sSQL += "'" & LinkDocumento & "',"
    '    sSQL += "'" & data & "',"
    '    sSQL += "'contratto')"
    '    sSQL += "select @@IDENTITY as MAX_CODDOCUMENTO "

    '    Try

    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()

    '        Dim drDocumenti As new dataview
    '        drDocumenti = iDB.getdataview(sSQL)

    '        If drDocumenti.Read Then
    '            mioMassimo = drDocumenti("MAX_CODDOCUMENTO")
    '        End If

    '        drDocumenti.Close()
    '        sqlConn.Close()

    '        sSQL = ""
    '        sSQL = "INSERT INTO TR_CONTRATTI_DOCUMENTI"
    '        sSQL += "(CODCONTRATTO,"
    '        sSQL += "CODDOCUMENTO)"
    '        sSQL += " VALUES ("
    '        sSQL += codcontratto & ","
    '        sSQL += mioMassimo & ")"

    '        Try

    '            Dim sqlCmdInsert2 As SqlCommand

    '            sqlConn.ConnectionString = ConstSession.StringConnection
    '            sqlConn.Open()

    '            sqlTrans = sqlConn.BeginTransaction

    '            sqlCmdInsert2 = New SqlCommand(sSQL, sqlConn, sqlTrans)

    '            sqlCmdInsert2.ExecuteNonQuery()

    '            sqlTrans.Commit()

    '        Catch er As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.setDocumentoContratto.errore: ", er)
    '            sqlTrans.Rollback()
    '            RaiseError.trace(er, sSQL.Replace(vbCrLf, ""), ConfigurationManager.AppSettings("Filename"), ConfigurationManager.AppSettings("FilePath"), "")
    '            Throw
    '        Finally
    '            sqlConn.Close()
    '        End Try
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.setDocumentoContratto.errore: ", Err)
    '    End Try
    'End Sub

    Public Function EliminaContratto(ByVal CodContratto As Integer) As Boolean
        Dim cmdMyCommand As New SqlCommand()
        Try
            cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText = "prc_TP_CONTRATTI_D"
            cmdMyCommand.Parameters.AddWithValue("@IDCONTRATTO", CodContratto)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContratti.EliminaContratto.errore: ", ex)
            Return False
        Finally
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CodContratto"></param>
    ''' <param name="nIdPeriodo"></param>
    ''' <param name="oDatiContratto"></param>
    ''' <param name="IsVoltura"></param>
    ''' <param name="bAcqFromTXT"></param>
    ''' <param name="nMyIdContatore"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function SaveContratto(ByVal CodContratto As Integer, ByVal nIdPeriodo As Integer, ByRef oDatiContratto As objContratto, IsVoltura As Integer, ByVal bAcqFromTXT As Boolean, ByRef nMyIdContatore As Integer) As Boolean ', IdContatoreOrg As Integer
        Dim lngTipoOp As Long = DBOperation.DB_INSERT
        Dim nMyIdContratto As Long

        Try
            If CodContratto > 0 Then
                lngTipoOp = DBOperation.DB_UPDATE
            End If

            If lngTipoOp = DBOperation.DB_INSERT Then
                If ControllaCodice(oDatiContratto.sIdEnte.ToString, oDatiContratto.sCodiceContratto, IsVoltura) = "-1" Then
                    nMyIdContratto = New MyUtility().GetMaxID("TP_CONTRATTI")

                    Try
                        oDatiContratto.nIdContratto = nMyIdContratto
                        oDatiContratto.oContatore.nIdContratto = oDatiContratto.nIdContratto
                        If SetContratto(oDatiContratto) = False Then
                            Log.Debug("setcontratto::no sql insert contatori")
                            Return False
                        End If
                    Catch er As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContratti.SaveContratto.errore: ", er)
                        Throw
                    End Try

                    If bAcqFromTXT = False Then
                        If oDatiContratto.oContatore.nIdContatore > 0 Then
                            oDatiContratto.oContatore.tDataVariazione = Now
                        Else
                            oDatiContratto.oContatore.tDataInserimento = Now
                        End If
                        oDatiContratto.oContatore.nIdIntestatario = oDatiContratto.nIdIntestatario
                        oDatiContratto.oContatore.nIdUtente = oDatiContratto.nIdUtente
                        oDatiContratto.oContatore.nIdContatore = New MyUtility().GetMaxID("TP_CONTATORI")
                        If FncContatori.SetContatore(oDatiContratto.oContatore, 0, -1, -1) = False Then
                            Log.Debug("SaveContratto::no sql insert contatori")
                            Return False
                        End If
                    End If
                Else
                    Return False
                End If
            ElseIf lngTipoOp = DBOperation.DB_UPDATE Then
                oDatiContratto.nIdContratto = CodContratto
                If SetContratto(oDatiContratto) = False Then
                    Log.Debug("SaveContratto::no sql insert contatori")
                    Return False
                End If
                'storicizzo il record originale
                Dim myContatore As New objContatore
                myContatore = New GestContatori().GetDetailsContatori(oDatiContratto.oContatore.nIdContatore, CodContratto)
                myContatore.tDataVariazione = Now
                If New GestContatori().SetContatore(myContatore, 0, 0, oDatiContratto.oContatore.nIdContatore) = False Then
                    Log.Debug("SaveContratto::no sql insert contatori")
                    Return False
                End If
                'inserisco il record attuale
                oDatiContratto.oContatore.nIdContatore = New MyUtility().GetMaxID("TP_CONTATORI")
                oDatiContratto.oContatore.sMatricola = myContatore.sMatricola
                oDatiContratto.oContatore.nIdContratto = CodContratto
                oDatiContratto.oContatore.oDatiCatastali = New GestContatori().GetDetailsCatasto(-1, myContatore.nIdContatore)
                If FncContatori.SetContatore(oDatiContratto.oContatore, -1, -1, myContatore.nIdContatore) = False Then
                    Log.Debug("SaveContratto::no sql insert contatori")
                    Return False
                End If
            End If
            Try
                For Each myCatRow As objDatiCatastali In oDatiContratto.oContatore.oDatiCatastali
                    If New GestContatori().SetDatiCatastali(myCatRow.sInterno, myCatRow.sPiano, myCatRow.sFoglio, myCatRow.sNumero, myCatRow.nSubalterno, oDatiContratto.oContatore.nIdContatore, myCatRow.sSezione, myCatRow.sEstensioneParticella, myCatRow.sIdTipoParticella) <= 0 Then
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContratti.SaveContratto.errore in inserimento dati catastali")
                    End If
                Next
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContratti.SaveContratto.SetDatiCatastali.errore: ", ex)
            End Try
            '***INSERISCO LA PRIMA LETTURA***
            If IsVoltura = 1 Then
                Dim dvMyDati As New DataView
                Dim lngLastLettura As Long
                dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 1, oDatiContratto.oContatore.nIdContatorePrec, oReplace.FormattaData("A", "", DateTime.MaxValue.ToShortDateString, False), "<")
                For Each myRow As DataRowView In dvMyDati
                    lngLastLettura = StringOperation.FormatInt(myRow("lettura"))
                Next
                dvMyDati.Dispose()
                Log.Debug("SaveContratto::primalettura")
                FncContatori.SetPrimaLettura(oDatiContratto.oContatore.nIdContatore, nIdPeriodo, oDatiContratto.oContatore.sDataAttivazione, CLng(oDatiContratto.oContatore.nIdContatorePrec), oDatiContratto.nIdUtente, oDatiContratto.oContatore.sNumeroUtente, lngLastLettura)
            End If
            nMyIdContatore = oDatiContratto.oContatore.nIdContatore
            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContratti.SaveContratto.errore: ", Err)
            Return False
        End Try
    End Function
    'Public Function SetContratto(ByVal CodContratto As String, ByVal nIdPeriodo As Integer, ByRef oDatiContratto As objContratto, Optional ByVal bAcqFromTXT As Boolean = False) As Boolean
    '    Dim sSQL, sLastDataLettura As String
    '    Dim lngTipoOp As Long
    '    Dim nMyIdContratto As Long
    '    Dim drTemp As new dataview
    '    Dim sqlTrans As SqlTransaction
    '    Dim sqlConn As New SqlConnection
    '    Dim nMyIdContatore As Integer
    '    Dim sqlCmdInsert As SqlCommand

    '    Try
    '        lngTipoOp = DBOperation.DB_INSERT

    '        If CInt(CodContratto) > 0 Then
    '            lngTipoOp = DBOperation.DB_UPDATE
    '        End If

    '        If lngTipoOp = DBOperation.DB_INSERT Then
    '            Log.Debug("setcontratto::insert")
    '            If ControllaCodice(oDatiContratto.sIdEnte.ToString, oDatiContratto.sCodiceContratto) = "-1" Then
    '                'sSQL = "SELECT MAX(TP_CONTRATTI.CODCONTRATTO) AS ID_MAX"
    '                'sSQL += " FROM TP_CONTRATTI WITH (NOLOCK)"
    '                'drTemp = iDB.getdataview(sSQL)
    '                'If drTemp.Read = True Then
    '                '    If Not IsDBNull(drTemp("ID_MAX")) Then
    '                '        nMyIdContratto = drTemp("ID_MAX")
    '                '    End If
    '                'End If
    '                'drTemp.Close()
    '                'nMyIdContratto = nMyIdContratto + 1
    '                nMyIdContratto = New MyUtility().GetMaxID("TP_CONTRATTI")

    '                Try
    '                    oDatiContratto.nIdContratto = CStr(nMyIdContratto)
    '                    oDatiContratto.oContatore.nIdContratto = oDatiContratto.nIdContratto
    '                    sqlConn.ConnectionString = ConstSession.StringConnection
    '                    sqlConn.Open()
    '                    sqlTrans = sqlConn.BeginTransaction
    '                    If SetContratto(lngTipoOp, oDatiContratto, sqlConn, sqlTrans) = False Then
    '                        Log.Debug("setcontratto::no sql insert contatori")
    '                        Return False
    '                    End If
    '                    ''************************************************************************************
    '                    ''INSERIMENTO DATI GENERALI NELLA TABELLA TP_CONTRATTI
    '                    ''************************************************************************************
    '                    'sSQL = GetSQLContratti(lngTipoOp, oDatiContratto, nMyIdContratto)
    '                    'Log.Debug("setcontratto::insert tp_contratti::" & sSQL)
    '                    'If sSQL <> "" Then
    '                    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                    '    sqlCmdInsert.ExecuteNonQuery()
    '                    'Else
    '                    '    Return False
    '                    'End If

    '                    ''************************************************************************************
    '                    ''INSERIMENTO ANAGRAFICHE NELLA TABELLA TR_CONTRATTI_INTESTATARIO
    '                    ''************************************************************************************
    '                    'sSQL = GetSQLIntestatarioUtente(lngTipoOp, "TR_CONTRATTI_INTESTATARIO", "CODCONTRATTO", nMyIdContratto, oDatiContratto.nIdIntestatario)
    '                    'Log.Debug("setcontratto::insert intestatario::" & sSQL)
    '                    'If sSQL <> "" Then
    '                    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                    '    sqlCmdInsert.ExecuteNonQuery()
    '                    'Else
    '                    '    Return False
    '                    'End If

    '                    ''************************************************************************************
    '                    ''INSERIMENTO ANAGRAFICHE NELLA TABELLA TR_CONTRATTI_UTENTE
    '                    ''************************************************************************************
    '                    'Log.Debug("setcontratto::insert utente" & sSQL)
    '                    'sSQL = GetSQLIntestatarioUtente(lngTipoOp, "TR_CONTRATTI_UTENTE", "CODCONTRATTO", nMyIdContratto, oDatiContratto.nIdUtente)
    '                    'If sSQL <> "" Then
    '                    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                    '    sqlCmdInsert.ExecuteNonQuery()
    '                    'Else
    '                    '    Return False
    '                    'End If

    '                    sqlTrans.Commit()
    '                Catch er As Exception
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.SetContratto.errore: ", er)
    '                    Log.Debug("setcontratto::errore", er)
    '                    sqlTrans.Rollback()
    '                    RaiseError.trace(er, sSQL.Replace(vbCrLf, ""), ConfigurationManager.AppSettings("Filename"), ConfigurationManager.AppSettings("FilePath"), "")
    '                    Throw
    '                Finally
    '                    sqlConn.Close()
    '                End Try

    '                '************************************************************************************
    '                If bAcqFromTXT = False Then
    '                    sqlConn.ConnectionString = ConstSession.StringConnection
    '                    sqlConn.Open()
    '                    If nMyIdContatore > 0 Then
    '                        oDatiContratto.oContatore.tDataVariazione = Now
    '                    Else
    '                        oDatiContratto.oContatore.tDataInserimento = Now
    '                    End If
    '                    ''Posso inserire qua x il data entry contatori
    '                    'sSQL = "SELECT MAX(TP_CONTATORI.CODCONTATORE) AS ID_MAX"
    '                    'sSQL += " FROM TP_CONTATORI WITH (NOLOCK)"
    '                    'drTemp = iDB.getdataview(sSQL)
    '                    'If drTemp.Read = True Then
    '                    '    If Not IsDBNull(drTemp("ID_MAX")) Then
    '                    '        nMyIdContatore = drTemp("ID_MAX")
    '                    '    End If
    '                    'End If
    '                    'drTemp.Close()
    '                    'nMyIdContatore += 1
    '                    nMyIdContatore = New MyUtility().GetMaxID("TP_CONTATORI")
    '                    '************************************************************************************
    '                    '===========================================================================================
    '                    'POPOLO LA TABELLA DEI CONTATORI INSERENDO COME CHIAVE ESTERNA L'ID DEL CONTRATTO APPENA CREATO.
    '                    'IN QUESTO MODO CONTRATTO E CONTATORE SONO DIRETTAMENTE COLLEGATI
    '                    '===========================================================================================
    '                    '====================================================================================
    '                    'INSERIMENTO DATI GENERALI NELLA TABELLA TP_CONTATORI
    '                    '====================================================================================
    '                    'sqlConn.ConnectionString = ConstSession.StringConnection
    '                    'sqlConn.Open()
    '                    'sSQL = FncContatori.GetSQLContatori(lngTipoOp, oDatiContratto.oContatore, nMyIdContatore, nMyIdContratto)
    '                    'Log.Debug("GestContratti::SetContratto::query di inserimento in contatori::" & sSQL)
    '                    'If sSQL <> "" Then
    '                    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn)
    '                    '    sqlCmdInsert.ExecuteNonQuery()
    '                    'Else
    '                    '    Return False
    '                    'End If
    '                    Log.Debug("setcontratto::insert contatori")
    '                    'Dim oMyCommand As New SqlCommand
    '                    'oDatiContratto.oContatore.nIdContratto = nMyIdContratto
    '                    'oMyCommand = FncContatori.GetSQLContatori(0, oDatiContratto.oContatore, nMyIdContatore, nMyIdContratto, -1, "", sqlConn, sqlTrans)
    '                    'If Not oMyCommand Is Nothing Then
    '                    '    Log.Debug("setcontratto::" & oMyCommand.CommandText)
    '                    '    oMyCommand.ExecuteNonQuery()
    '                    'Else
    '                    '    Log.Debug("setcontratto::no sql insert contatori")
    '                    '    Return False
    '                    'End If
    '                    If FncContatori.SetContatore(oDatiContratto.oContatore, sqlConn, sqlTrans, 0, -1) = False Then
    '                        Log.Debug("setcontratto::no sql insert contatori")
    '                        Return False
    '                    End If
    '                    ''====================================================================================
    '                    ''INSERIMENTO ANAGRAFICHE NELLA TABELLA TR_CONTATORI_INTESTATARIO
    '                    ''====================================================================================
    '                    'sSQL = GetSQLIntestatarioUtente(lngTipoOp, "TR_CONTATORI_INTESTATARIO", "CODCONTATORE", nMyIdContatore, oDatiContratto.nIdIntestatario)
    '                    'Log.Debug("setcontratto::insert intestatario " & sSQL)
    '                    'If sSQL <> "" Then
    '                    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                    '    sqlCmdInsert.ExecuteNonQuery()
    '                    'Else
    '                    '    Return False
    '                    'End If
    '                    ''====================================================================================
    '                    ''INSERIMENTO ANAGRAFICHE NELLA TABELLA TR_CONTATORI_UTENTE
    '                    ''====================================================================================
    '                    'sSQL = GetSQLIntestatarioUtente(lngTipoOp, "TR_CONTATORI_UTENTE", "CODCONTATORE", nMyIdContatore, oDatiContratto.nIdUtente)
    '                    'Log.Debug("setcontratto::insert utente" & sSQL)
    '                    'If sSQL <> "" Then
    '                    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                    '    sqlCmdInsert.ExecuteNonQuery()
    '                    'Else
    '                    '    Return False
    '                    'End If
    '                    sqlConn.Close()
    '                End If
    '            Else
    '                Return False
    '            End If
    '        ElseIf lngTipoOp = DBOperation.DB_UPDATE Then
    '            Log.Debug("setcontratto::update")
    '            '******************************************************************************************
    '            'UPDATE TABELLA TP_CONTRATTI
    '            '******************************************************************************************
    '            Try
    '                sqlConn.ConnectionString = ConstSession.StringConnection
    '                sqlConn.Open()
    '                sqlTrans = sqlConn.BeginTransaction
    '                If SetContratto(DBOperation.DB_INSERT, oDatiContratto, sqlConn, sqlTrans) = False Then
    '                    Log.Debug("setcontratto::no sql insert contatori")
    '                    Return False
    '                End If
    '                'sSQL = GetSQLContratti(lngTipoOp, oDatiContratto, CodContratto)
    '                'Log.Debug("setcontratto::insert contratti" & sSQL)
    '                'If sSQL <> "" Then
    '                '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                '    sqlCmdInsert.ExecuteNonQuery()
    '                'Else
    '                '    Return False
    '                'End If
    '                ''******************************************************************************************
    '                ''UPDATE ANAGRAFICHE TABELLA TR_CONTRATTI_INTESTATARIO
    '                ''******************************************************************************************
    '                'sSQL = GetSQLIntestatarioUtente(lngTipoOp, "TR_CONTRATTI_INTESTATARIO", "CODCONTRATTO", CodContratto, oDatiContratto.nIdIntestatario)
    '                'Log.Debug("setcontratto::insert intestatario" & sSQL)
    '                'If sSQL <> "" Then
    '                '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                '    sqlCmdInsert.ExecuteNonQuery()
    '                'Else
    '                '    Return False
    '                'End If
    '                ''******************************************************************************************
    '                ''UPDATE ANAGRAFICHE TABELLA TR_CONTRATTI_UTENTE
    '                ''******************************************************************************************
    '                'sSQL = GetSQLIntestatarioUtente(lngTipoOp, "TR_CONTRATTI_UTENTE", "CODCONTRATTO", CodContratto, oDatiContratto.nIdUtente)
    '                'Log.Debug("setcontratto::insert utente" & sSQL)
    '                'If sSQL <> "" Then
    '                '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                '    sqlCmdInsert.ExecuteNonQuery()
    '                'Else
    '                '    Return False
    '                'End If

    '                'Dim CodContrattoPerVoltura As Int32 = CodContratto
    '                'Dim drContatoreAssociato As new dataview
    '                Dim myContatore As New objContatore
    '                myContatore = New GestContatori().GetDetailsContatori(-1, CodContratto)
    '                'sSQL = "SELECT TP_CONTATORI.CODCONTATORE"
    '                'sSQL += " FROM TP_CONTATORI WITH (NOLOCK)"
    '                'sSQL += " WHERE TP_CONTATORI.CODCONTRATTO= " & CodContratto
    '                'drContatoreAssociato = iDB.getdataview(sSQL)
    '                'While drContatoreAssociato.Read()
    '                '    myContatore = drContatoreAssociato("CODCONTATORE")
    '                'End While
    '                'drContatoreAssociato.Close()

    '                If New GestContatori().SetContatore(myContatore, sqlConn, sqlTrans, 0, 0) = False Then
    '                    Log.Debug("setcontratto::no sql insert contatori")
    '                    Return False
    '                End If
    '                ''******************************************************************************************
    '                ''UPDATE ANAGRAFICHE TABELLA TR_CONTATORI_UTENTE
    '                ''******************************************************************************************
    '                'sSQL = GetSQLIntestatarioUtente(lngTipoOp, "TR_CONTATORI_UTENTE", "CODCONTATORE", myContatore, oDatiContratto.nIdUtente)
    '                'Log.Debug("setcontratto::insert utente" & sSQL)
    '                'If sSQL <> "" Then
    '                '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                '    sqlCmdInsert.ExecuteNonQuery()
    '                'Else
    '                '    Return False
    '                'End If

    '                ''******************************************************************************************
    '                ''UPDATE ANAGRAFICHE TABELLA TR_CONTATORI_INTESTATARIO
    '                ''******************************************************************************************
    '                'sSQL = GetSQLIntestatarioUtente(lngTipoOp, "TR_CONTATORI_INTESTATARIO", "CODCONTATORE", myContatore, oDatiContratto.nIdIntestatario)
    '                'Log.Debug("setcontratto::insert intestatario" & sSQL)
    '                'If sSQL <> "" Then
    '                '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                '    sqlCmdInsert.ExecuteNonQuery()
    '                'Else
    '                '    Return False
    '                'End If

    '                '==================================================
    '                'UPDATE DATI RIDONDANTI
    '                '==================================================
    '                oDatiContratto.oContatore.tDataVariazione = Now
    '                'sSQL = FncContatori.GetSQLContatori(lngTipoOp, oDatiContratto.oContatore, myContatore, CodContratto)
    '                'If sSQL <> "" Then
    '                '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                '    sqlCmdInsert.ExecuteNonQuery()
    '                'Else
    '                '    Return False
    '                'End If
    '                sqlTrans.Commit()
    '                oDatiContratto.oContatore.nIdContratto = CodContratto
    '                'Dim oMyCommand As New SqlCommand
    '                'oMyCommand = FncContatori.GetSQLContatori(0, oDatiContratto.oContatore, myContatore, CodContratto, -1, "", sqlConn, sqlTrans)
    '                'Log.Debug("setcontratto::contatori")
    '                'If Not oMyCommand Is Nothing Then
    '                '    Log.Debug("setcontratto::" & oMyCommand.CommandText)
    '                '    oMyCommand.ExecuteNonQuery()
    '                'Else
    '                '    Log.Debug("setcontratto::no insert contatori")
    '                '    Return False
    '                'End If
    '                If FncContatori.SetContatore(oDatiContratto.oContatore, sqlConn, sqlTrans, -1, -1) = False Then
    '                    Log.Debug("setcontratto::no sql insert contatori")
    '                    Return False
    '                End If


    '                If oDatiContratto.sDataCessazione <> "" Then
    '                    sqlTrans = sqlConn.BeginTransaction
    '                    'se la data di cessazione è popolata, creo un nuovo contratto identico a quello modificato
    '                    'ma senza popolare le anagrafiche per l'intestatario e l'utente, il codice contratto
    '                    'ma popolando la data sottoscrizione=datacessazione+1
    '                    'sSQL = "SELECT MAX(TP_CONTRATTI.CODCONTRATTO) AS ID_MAX"
    '                    'sSQL += " FROM TP_CONTRATTI WITH (NOLOCK)"
    '                    'drTemp = iDB.getdataview(sSQL)
    '                    'If drTemp.Read = True Then
    '                    '    If Not IsDBNull(drTemp("ID_MAX")) Then
    '                    '        nMyIdContratto = drTemp("ID_MAX")
    '                    '    End If
    '                    'End If
    '                    'drTemp.Close()
    '                    'nMyIdContratto += 1
    '                    'sSQL = "SELECT MAX(TP_CONTATORI.CODCONTATORE) AS ID_MAX"
    '                    'sSQL += " FROM TP_CONTATORI WITH (NOLOCK)"
    '                    'drTemp = iDB.getdataview(sSQL)
    '                    'If drTemp.Read = True Then
    '                    '    If Not IsDBNull(drTemp("ID_MAX")) Then
    '                    '        nMyIdContatore = drTemp("ID_MAX")
    '                    '    End If
    '                    'End If
    '                    'drTemp.Close()
    '                    'nMyIdContatore += 1
    '                    nMyIdContratto = New MyUtility().GetMaxID("TP_CONTRATTI")
    '                    nMyIdContatore = New MyUtility().GetMaxID("TP_CONTATORI")

    '                    oDatiContratto.nIdIntestatario = -1
    '                    oDatiContratto.nIdUtente = -1
    '                    oDatiContratto.nIdContratto = -1
    '                    oDatiContratto.sDataSottoscrizione = DateAdd(DateInterval.Day, 1, CDate(oDatiContratto.sDataCessazione))

    '                    'Ok, qua verrà effettuata la voltura del contratto
    '                    If SetContratto(DBOperation.DB_INSERT, oDatiContratto, sqlConn, sqlTrans) = False Then
    '                        Log.Debug("setcontratto::no sql insert contatori")
    '                        Return False
    '                    End If
    '                    'sSQL = GetSQLContratti(DBOperation.DB_INSERT, oDatiContratto, nMyIdContratto)
    '                    'Log.Debug("setcontratto::contratti::" & sSQL)
    '                    'If sSQL <> "" Then
    '                    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                    '    sqlCmdInsert.ExecuteNonQuery()
    '                    'Else
    '                    '    Return False
    '                    'End If
    '                    ''==========================================
    '                    ''INSERIMENTO ANAGRAFICA INTESTATARIO
    '                    ''==========================================
    '                    'sSQL = GetSQLIntestatarioUtente(DBOperation.DB_INSERT, "TR_CONTRATTI_INTESTATARIO", "CODCONTRATTO", nMyIdContratto, -1)
    '                    'Log.Debug("setcontratto::intestatario::" & sSQL)
    '                    'If sSQL <> "" Then
    '                    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                    '    sqlCmdInsert.ExecuteNonQuery()
    '                    'Else
    '                    '    Return False
    '                    'End If

    '                    ''==========================================
    '                    ''INSERIMENTO ANAGRAFICA UTENTE
    '                    ''==========================================
    '                    'sSQL = GetSQLIntestatarioUtente(DBOperation.DB_INSERT, "TR_CONTRATTI_UTENTE", "CODCONTRATTO", nMyIdContratto, -1)
    '                    'Log.Debug("setcontratto::utente::" & sSQL)
    '                    'If sSQL <> "" Then
    '                    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                    '    sqlCmdInsert.ExecuteNonQuery()
    '                    'Else
    '                    '    Return False
    '                    'End If

    '                    oDatiContratto.nIdContratto = nMyIdContratto
    '                    'questa nuova variabile mi serve per reindirizzare la pagina nel nuovo contratto creato
    '                    'alla fine del metodo ritorna a datigeneralicontr.aspx e come parametro del contratto mette 
    '                    'nuovoIDCalcolato, che sarebbe il nuovo contratto senza data di sottoscrizione, codice
    '                    'in stringa del contratto e le 2 anagrafiche, cioè di utente e intestatario

    '                    'seleziono l'id del contatore a cui era associato il vecchio contratto
    '                    ' è uguale alla variabile ==> mioContratto

    '                    'CREO IL NUOVO CONTATORE DA ASSOCIARE AL NUOVO CONTRATTO
    '                    'sSQL = FncContatori.GetSQLContatori(DBOperation.DB_INSERT, Nothing, nMyIdContatore, nMyIdContratto, myContatore, oDatiContratto.sDataSottoscrizione)
    '                    'If sSQL <> "" Then
    '                    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                    '    sqlCmdInsert.ExecuteNonQuery()
    '                    'Else
    '                    '    Return False
    '                    'End If
    '                    oDatiContratto.oContatore.nIdContratto = nMyIdContratto
    '                    oDatiContratto.oContatore.nIdContatore = nMyIdContatore
    '                    'oMyCommand = FncContatori.GetSQLContatori(0, Nothing, nMyIdContatore, nMyIdContratto, myContatore, oDatiContratto.sDataSottoscrizione, sqlConn, sqlTrans)
    '                    'Log.Debug("setcontratto::contatori")
    '                    'If Not oMyCommand Is Nothing Then
    '                    '    Log.Debug("setcontratto::" & oMyCommand.CommandText)
    '                    '    oMyCommand.ExecuteNonQuery()
    '                    'Else
    '                    '    Log.Debug("setcontratto::not sqlcontatori")
    '                    '    Return False
    '                    'End If
    '                    If FncContatori.SetContatore(oDatiContratto.oContatore, sqlConn, sqlTrans, -1, -1) = False Then
    '                        Log.Debug("setcontratto::no sql insert contatori")
    '                        Return False
    '                    End If

    '                    '==============================
    '                    'RIBALTAMENTO DATI CATASTALI
    '                    '==============================
    '                    If FncContatori.SetDatiCatastali("", "", "", "", "", nMyIdContatore, "", "", "") <= 0 Then
    '                        Log.Debug("setcontratto::no sql insert SetDatiCatastali")
    '                    End If

    '                    '***INSERISCO LA PRIMA LETTURA***
    '                    Dim conteggioLetture As New DataTable
    '                    Dim lngLastLettura As Long
    '                    conteggioLetture = New GestLetture().getTopOneLetture(nMyIdContatore, oReplace.FormattaData("A", "", DateTime.MaxValue.ToShortDateString, False), "P")
    '                    For Each myRowLet As DataRow In conteggioLetture.Rows
    '                        If Not IsDBNull(myRowLet("lettura")) Then
    '                            lngLastLettura = myRowLet("lettura")
    '                        End If
    '                        If Not IsDBNull(myRowLet("datalettura")) Then
    '                            sLastDataLettura = oReplace.FormattaData("G", "/", CStr(myRowLet("datalettura")), False)
    '                        End If
    '                    Next
    '                    conteggioLetture.Dispose()

    '                    sqlTrans.Commit()
    '                    If Not IsDBNull(oDatiContratto.sDataSottoscrizione) Then
    '                        'se la data di attivazione non è Null
    '                        If oDatiContratto.sDataSottoscrizione <> "" Then
    '                            'inserisco la prima lettura uguale a 0 per questo contatore
    '                            If sLastDataLettura = "" Then
    '                                sLastDataLettura = oDatiContratto.sDataSottoscrizione
    '                            End If
    '                            Log.Debug("setcontratto::primalettura")
    '                            FncContatori.SetPrimaLettura(nMyIdContatore, nIdPeriodo, sLastDataLettura, CLng(oDatiContratto.oContatore.nIdContatorePrec), oDatiContratto.nIdUtente, oDatiContratto.oContatore.sNumeroUtente, lngLastLettura, sqlConn, sqlTrans)
    '                        End If
    '                    End If
    '                    '********************************
    '                End If
    '                Log.Debug("setcontratto::ok")
    '            Catch er As Exception

    '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.SetContratto.errore: ", er)
    '                sqlTrans.Rollback()
    '                RaiseError.trace(er, sSQL.Replace(vbCrLf, ""), ConfigurationManager.AppSettings("Filename"), ConfigurationManager.AppSettings("FilePath"), "")
    '                Throw
    '            Finally
    '                sqlConn.Close()
    '            End Try
    '        End If
    '        Return True
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.SetContratto.errore: ", Err)
    '        Return False
    '    End Try
    'End Function

    'Public Sub SetContratto_20080804(ByVal CodContratto As String, ByRef odaticontratto As objContratto)

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    'dim sSQL as string
    'Dim lngTipoOp As Long
    'Dim IDValue As Long
    'Dim drTemp As new dataview
    'Dim blnPrincipale As Boolean
    'Dim oProcess As Object
    'Dim valore As String
    'Dim strAppoggio As String
    'Dim strValue As String

    'Dim sqlTrans As SqlTransaction
    'Dim sqlConn As New SqlConnection
    'Dim oReplace as new ClsGenerale.Generale
    'Try
    'lngTipoOp = DBOperation.DB_INSERT

    'If CInt(CodContratto) > 0 Then
    '    lngTipoOp = DBOperation.DB_UPDATE
    'End If


    'Dim mioContratto As Int32


    'If ControllaCodice(odaticontratto.nIdContratto) <> "-1" Then
    '    Dim miastringadiprova As String = "Non si puo inserire un codice contratto uguale ad uno precedentemente inserito"
    '    'se passa di qua, non verrà inserito un nuovo contratto in quanto il codice contratto
    '    ' di tipo stringa è già esistente
    'End If

    'If lngTipoOp = DBOperation.DB_INSERT And ControllaCodice(odaticontratto.nIdContratto) = "-1" Then

    '    '************************************************************************************
    '    sSQL="SELECT MAX(CODCONTRATTO) AS ID_MAX FROM TP_CONTRATTI" & vbCrLf
    '    drTemp = DBAccess.getdataview(sSQL)
    '    If drTemp.Read = True Then
    '        If Not IsDBNull(drTemp("ID_MAX")) Then
    '            IDValue = drTemp("ID_MAX")
    '        End If
    '    End If
    '    drTemp.Close()


    '    IDValue = IDValue + 1


    '    '************************************************************************************
    '    'INSERIMENTO DATI GENERALI NELLA TABELLA TP_CONTRATTI
    '    '************************************************************************************

    '    sSQL="INSERT INTO TP_CONTRATTI"

    '    '=========================
    '    'INIZIO MODIFICHE ALE CAO
    '    '=========================

    '    sSQL+="(PIANO,"
    '    sSQL+=" CODCONTRATTO,"

    '    sSQL+=" CODICECONTRATTO,"
    '    sSQL+="DATASOTTOSCRIZIONE,"

    '    sSQL+=" FOGLIO,"
    '    sSQL+=" NUMERO,"
    '    sSQL+=" SUBALTERNO,"

    '    sSQL+=" SPESA,"
    '    sSQL+=" DIRITTI,"
    '    sSQL+=" PENDENTE,"
    '    '=========================
    '    'FINE MODIFICHE ALE CAO
    '    '=========================

    '    sSQL+=" CODENTE,"

    '    'sSQL+=" NUMEROUTENZE,"
    '    sSQL+=" CODIMPIANTO,"
    '    sSQL+=" IDGIRO,"
    '    sSQL+=" SEQUENZA,"

    '    sSQL+=" IDTIPOCONTATORE,"
    '    'sSQL+=" IDTIPOUTENZA,"
    '    sSQL+=" CODPOSIZIONE,"
    '    sSQL+=" POSIZIONEPROGRESSIVA,"
    '    sSQL+=" NOTE,"
    '    sSQL+=" DATAATTIVAZIONE,"

    '    'sSQL+=" CODFOGNATURA,"
    '    'sSQL+=" CODDEPURAZIONE,"
    '    'sSQL+=" ESENTEFOGNATURA,"
    '    'sSQL+=" ESENTEDEPURAZIONE,"
    '    sSQL+=" CODDIAMETROCONTATORE,"
    '    sSQL+=" CODDIAMETROPRESA,"

    '    sSQL+=" IDMINIMO,"
    '    sSQL+=" LATOSTRADA,"
    '    sSQL+=" IGNORAMORA,"
    '    sSQL+=" CODENTE1,"
    '    sSQL+=" CODENTEAPPARTENENZA1,"
    '    'sSQL+=" COD_STRADA,"
    '    'sSQL+=" CIVICO_UBICAZIONE,"
    '    sSQL+=" DATASOSPENSIONEUTENZA,"
    '    sSQL+=" UTENTESOSPESO,"

    '    sSQL+=" CODICEFABBRICANTE,"
    '    sSQL+=" CIFRECONTATORE,"
    '    sSQL+=" CODIVA,"
    '    sSQL+=" STATOCONTATORE,"
    '    sSQL+=" PENALITA,"
    '    sSQL+=" CODICE_ISTAT,"
    '    'sSQL+=" ESPONENTE_CIVICO,"
    '    sSQL+=" PROPRIETARIO,"
    '    sSQL+=" RICHIESTASUB,"
    '    sSQL+=" NOTERICHIESTASUB,"
    '    'sSQL+=" NUMEROUTENTE,"
    '    sSQL+=" IDTIPOATTIVITA)"

    '    sSQL+=" VALUES ( "






    '    '=========================
    '    'INIZIO MODIFICHE ALE CAO
    '    '=========================



    '    If odaticontratto.sPiano <> "" Then
    '        sSQL+="'0',"
    '    Else
    '        sSQL+="Null, "
    '    End If

    '    sSQL+= IDValue & ", "

    '    sSQL+="'" & CStr(odaticontratto.nIdContratto) & "', "

    '    sSQL+="'" & CStr(oReplace.GiraData(odaticontratto.sdatasottoscrizione)) & "', "

    '    If odaticontratto.sFoglio <> "" Then
    '        sSQL+="'" & CStr(odaticontratto.sFoglio) & "', "
    '    Else
    '        sSQL+="Null, "
    '    End If



    '    If odaticontratto.sNumero <> "" Then
    '        sSQL+="'" & CStr(odaticontratto.sNumero) & "', "
    '    Else
    '        sSQL+="Null, "
    '    End If


    '    If odaticontratto.nsubalterno <> 0 Then
    '        sSQL+= CInt(odaticontratto.nsubalterno) & ", "
    '    Else
    '        sSQL+="Null, "
    '    End If

    '    If odaticontratto.nSpesa <> 0 Then
    '        sSQL+= odaticontratto.nspesa.ToString().Replace(",", ".") & ", "
    '    Else
    '        sSQL+="Null, "
    '    End If


    '    If odaticontratto.nDiritti <> 0 Then
    '        sSQL+= odaticontratto.nDiritti.ToString().Replace(",", ".") & ", "
    '    Else
    '        sSQL+="Null, "
    '    End If

    '    If odaticontratto.bIsPendente <> 1 Then
    '        sSQL+= 0 & ", "
    '    Else
    '        sSQL+= 1 & ", "
    '    End If

    '    '=========================
    '    'FINE MODIFICHE ALE CAO
    '    '=========================


    '    sSQL+= CInt(odaticontratto.sidEnte) & ","

    '    'sSQL+="'" & CStr(odaticontratto.sNumeroUtenze) & "', "
    '    If odaticontratto.nIdImpianto <> -1 Then
    '        sSQL+= CInt(odaticontratto.nIdImpianto) & ", "
    '    Else
    '        sSQL+=" Null, "
    '    End If
    '    If odaticontratto.nGiro <> -1 Then
    '        sSQL+= CInt(odaticontratto.nGiro) & ","
    '    Else
    '        sSQL+=" Null,"
    '    End If
    '    sSQL+="'" & CStr(odaticontratto.sSequenza) & "',"

    '    If odaticontratto.nTipoContatore <> -1 Then
    '        sSQL+= CInt(odaticontratto.nTipoContatore) & ","
    '    Else
    '        sSQL+=" Null,"
    '    End If
    '    'If odaticontratto.nTipoUtenza <> -1 Then
    '    '    sSQL+= CInt(odaticontratto.nTipoUtenza) & ","
    '    'Else
    '    '    sSQL+=" Null,"
    '    'End If
    '    sSQL+= CInt(odaticontratto.nPosizione) & ","
    '    sSQL+="'" & CStr(odaticontratto.sProgressivo) & "',"
    '    sSQL+="'" & CStr(oReplace.ReplaceChar(odaticontratto.sNote)) & "',"
    '    sSQL+="'" & CStr(oReplace.GiraData(odaticontratto.sDataAttivazione)) & "',"

    '    'If odaticontratto.ncodfognatura <> -1 Then
    '    '    sSQL+= CInt(odaticontratto.ncodfognatura) & ","
    '    'Else
    '    '    sSQL+=" Null,"
    '    'End If
    '    'If odaticontratto.ncoddepurazione <> -1 Then
    '    '    sSQL+= CInt(odaticontratto.ncoddepurazione) & ","
    '    'Else
    '    '    sSQL+="Null,"
    '    'End If
    '    'sSQL+= odaticontratto.bEsenteFognatura & ","
    '    'sSQL+= odaticontratto.bEsenteDepurazione & ","
    '    If odaticontratto.nDiametroCont <> -1 Then
    '        sSQL+= CInt(odaticontratto.nDiametroCont) & ","
    '    Else
    '        sSQL+="Null,"
    '    End If

    '    If odaticontratto.nDiametroPresa <> -1 Then
    '        sSQL+= CInt(odaticontratto.nDiametroPresa) & ","
    '    Else
    '        sSQL+="Null,"
    '    End If

    '    If odaticontratto.nIdMinimo <> -1 Then
    '        sSQL+= CInt(odaticontratto.nIdMinimo) & ","
    '    Else
    '        sSQL+="Null,"
    '    End If
    '    sSQL+="'" & CStr(odaticontratto.sLatoStrada) & "',"
    '    sSQL+= odaticontratto.bignoramora & ","
    '    sSQL+="'" & CStr(odaticontratto.sidEnte) & "',"
    '    sSQL+="'" & odaticontratto.sidEnteAppartenenza & "',"
    '    'If odaticontratto.nIdVia <> -1 Then
    '    '    sSQL+= CInt(odaticontratto.nIdVia) & ","
    '    'Else
    '    '    sSQL+=" Null,"
    '    'End If
    '    'sSQL+="'" & CStr(odaticontratto.sCivico) & "',"
    '    sSQL+="'" & CStr(oReplace.GiraData(odaticontratto.sDataSospensioneUtenza)) & "',"
    '    sSQL+= odaticontratto.bUtenteSospeso & ","

    '    sSQL+="'" & CStr(odaticontratto.sCodiceFabbricante) & "',"
    '    sSQL+="'" & CStr(odaticontratto.sCifreContatore) & "',"
    '    If odaticontratto.nCodIva <> -1 Then
    '        sSQL+= CInt(odaticontratto.nCodIva) & ","
    '    Else
    '        sSQL+="Null,"
    '    End If
    '    sSQL+="'" & CStr(odaticontratto.sStatoContatore) & "',"
    '    sSQL+="'" & CStr(odaticontratto.sPenalita) & "',"
    '    sSQL+="'" & CStr(odaticontratto.sCodiceISTAT) & "',"
    '    'sSQL+="'" & CStr(odaticontratto.sEsponente) & "',"



    '    ' '====================================================================================
    '    ' 'PER PRODUZIONE
    '    ' '====================================================================================
    '    ' 'Prelevo gli Ultimi 3 caratteri del codice impianto
    '    strAppoggio = Right(utility.stringoperation.formatstring(odaticontratto.nIdImpianto), 3)
    '    strAppoggio = strAppoggio.PadLeft(3, "0")

    '    sSQL+= CInt(odaticontratto.nproprietario) & ","

    '    If odaticontratto.richiestaSub = False Then
    '        sSQL+= 0 & ","
    '    Else
    '        sSQL+= 1 & ","
    '    End If

    '    sSQL+="'" & CStr(oReplace.ReplaceChar(odaticontratto.snoteRichiestaSub)) & "',"

    '    'sSQL+= odaticontratto.INumeroUtente & ","

    '    If odaticontratto.nIdAttivita <> -1 Then
    '        sSQL+= CInt(odaticontratto.nIdAttivita)
    '    Else
    '        sSQL+="Null"
    '    End If



    '    sSQL+=" )"
    '    Dim sqlCmdInsert As SqlCommand
    '    Try



    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()

    '        sqlTrans = sqlConn.BeginTransaction

    '        sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)

    '        sqlCmdInsert.ExecuteNonQuery()

    '        odaticontratto.nidcontratto = CStr(IDValue)


    '        '************************************************************************************
    '        'INSERIMENTO ANAGRAFICHE NELLA TABELLA TR_CONTRATTI_INTESTATARIO
    '        '************************************************************************************
    '        sSQL=""
    '        sSQL="INSERT INTO TR_CONTRATTI_INTESTATARIO" & vbCrLf
    '        sSQL+="(CODCONTRATTO,COD_CONTRIBUENTE)" & vbCrLf
    '        sSQL+="VALUES ( " & vbCrLf
    '        sSQL+= CInt(IDValue) & "," & vbCrLf
    '        sSQL+= CInt(odaticontratto.nidIntestatario) & vbCrLf
    '        sSQL+=" )"

    '        sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '        sqlCmdInsert.ExecuteNonQuery()


    '        sSQL=""
    '        sSQL="INSERT INTO "

    '        '************************************************************************************
    '        'INSERIMENTO ANAGRAFICHE NELLA TABELLA TR_CONTRATTI_UTENTE
    '        '************************************************************************************
    '        sSQL=""
    '        sSQL="INSERT INTO TR_CONTRATTI_UTENTE" & vbCrLf
    '        sSQL+="(CODCONTRATTO,COD_CONTRIBUENTE)" & vbCrLf
    '        sSQL+="VALUES ( " & vbCrLf
    '        sSQL+= CInt(IDValue) & "," & vbCrLf
    '        sSQL+= CInt(odaticontratto.nidUtente) & vbCrLf
    '        sSQL+=" )"

    '        sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '        sqlCmdInsert.ExecuteNonQuery()


    '        sqlTrans.Commit()

    '    Catch er As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.SetContratto_20080804.errore: ", er)
    '        sqlTrans.Rollback()
    '        RaiseError.trace(er, sSql.Replace(vbCrLf, ""), ConfigurationManager.AppSettings("Filename"), ConfigurationManager.AppSettings("FilePath"), "")
    '        Throw
    '    Finally
    '        sqlConn.Close()
    '    End Try

    '    Dim stringa As String = "Posso inserire qua x il data entry contatori"
    '    '************************************************************************************
    '    Dim MASSIMOIDValue As Int32


    '    sSQL="SELECT MAX(CODCONTATORE) AS ID_MAX FROM TP_CONTATORI" & vbCrLf
    '    drTemp = DBAccess.getdataview(sSQL)
    '    'DBAccess.RunSQLReturnDataSet()

    '    If drTemp.Read = True Then
    '        If Not IsDBNull(drTemp("ID_MAX")) Then
    '            MASSIMOIDValue = drTemp("ID_MAX")
    '        End If
    '    End If
    '    drTemp.Close()

    '    'IDValue = DBAccess.RunActionQueryIdentiy(sSQL)
    '    MASSIMOIDValue = MASSIMOIDValue + 1
    '    '************************************************************************************



    '    '===========================================================================================
    '    'POPOLO LA TABELLA DEI CONTATORI INSERENDO COME CHIAVE ESTERNA L'ID DEL CONTRATTO APPENA CREATO.
    '    'IN QUESTO MODO CONTRATTO E CONTATORE SONO DIRETTAMENTE COLLEGATI
    '    '===========================================================================================

    '    '====================================================================================
    '    '<< CORRETTO QUI >>
    '    'SOLO SE E' STATA INSERITA LA DATA DI ATTIVAZIONE
    '    '====================================================================================
    '    'If odaticontratto.sDataAttivazione <> "" Then
    '    stringa = "Ok, la data di attivazione è inserita quindi creo il nuovo contatore"
    '    stringa.ToUpper()

    '    '====================================================================================
    '    'INSERIMENTO DATI GENERALI NELLA TABELLA TP_CONTATORI
    '    '====================================================================================

    '    sSQL=""
    '    sSQL="INSERT INTO TP_CONTATORI"
    '    sSQL+="(CODCONTATORE,"
    '    sSQL+="CODENTE,"
    '    sSQL+="NUMEROUTENZE,"
    '    sSQL+="CODIMPIANTO,"
    '    sSQL+="IDGIRO,"
    '    sSQL+="SEQUENZA,"
    '    sSQL+="IDTIPOCONTATORE,"
    '    sSQL+="IDTIPOUTENZA,"
    '    sSQL+="CODPOSIZIONE,"
    '    sSQL+="POSIZIONEPROGRESSIVA,"
    '    sSQL+="NOTE,"
    '    sSQL+="DATAATTIVAZIONE,"
    '    sSQL+="CODFOGNATURA,"
    '    sSQL+="CODDEPURAZIONE,"
    '    sSQL+="ESENTEFOGNATURA,"
    '    sSQL+="ESENTEDEPURAZIONE,"
    '    sSQL+="ESENTEACQUA,"
    '    sSQL+="CODDIAMETROCONTATORE,"
    '    sSQL+="CODDIAMETROPRESA,"
    '    sSQL+="CODCONTRATTO,"
    '    sSQL+="LATOSTRADA,"
    '    sSQL+="IGNORAMORA,"
    '    sSQL+="CODENTE1,"
    '    sSQL+="CODENTEAPPARTENENZA1,"
    '    sSQL+="COD_STRADA,"
    '    sSQL+="CIVICO_UBICAZIONE,"
    '    sSQL+="DATASOSPENSIONEUTENZA,"
    '    sSQL+="UTENTESOSPESO,"
    '    sSQL+="CODICEFABBRICANTE,"
    '    sSQL+="CIFRECONTATORE,"
    '    sSQL+="CODIVA,"
    '    sSQL+="STATOCONTATORE,"
    '    sSQL+="PENALITA,"
    '    sSQL+="CODICE_ISTAT,"
    '    sSQL+="ESPONENTE_CIVICO,"
    '    sSQL+="IDMINIMO,"
    '    sSQL+="IDTIPOATTIVITA,"
    '    sSQL+="PIANO,"
    '    sSQL+="FOGLIO,"
    '    sSQL+="NUMERO,"
    '    sSQL+="SUBALTERNO,"
    '    sSQL+="PENDENTE,"
    '    sSQL+="SPESA,"
    '    sSQL+="PROPRIETARIO,"
    '    sSQL+="NUMEROUTENTE,"
    '    sSQL+="DIRITTI,"
    '    sSQL+="VIA_UBICAZIONE)"

    '    sSQL+=" VALUES ("
    '    sSQL+= MASSIMOIDValue & ", "
    '    sSQL+= CInt(odaticontratto.sidEnte) & ", "
    '    sSQL+= CInt(odaticontratto.sNumeroUtenze) & ", "

    '    If odaticontratto.nIdImpianto <> -1 Then
    '        sSQL+= CInt(odaticontratto.nIdImpianto) & ", "
    '    Else
    '        sSQL+=" Null, "
    '    End If

    '    If odaticontratto.nGiro <> -1 Then
    '        sSQL+= CInt(odaticontratto.nGiro) & ","
    '    Else
    '        sSQL+=" Null,"
    '    End If

    '    sSQL+="'" & CStr(odaticontratto.sSequenza) & "',"

    '    If odaticontratto.nTipoContatore <> -1 Then
    '        sSQL+= CInt(odaticontratto.nTipoContatore) & ","
    '    Else
    '        sSQL+=" Null,"
    '    End If


    '    If odaticontratto.nTipoUtenza <> -1 Then
    '        sSQL+= CInt(odaticontratto.nTipoUtenza) & ","
    '    Else
    '        sSQL+=" Null,"
    '    End If

    '    sSQL+= CInt(odaticontratto.nPosizione) & ","

    '    sSQL+="'" & CStr(odaticontratto.sProgressivo) & "',"

    '    sSQL+="'" & CStr(oReplace.ReplaceChar(odaticontratto.sNote)) & "',"

    '    sSQL+="'" & CStr(oReplace.GiraData(odaticontratto.sDataAttivazione)) & "',"

    '    If odaticontratto.ncodfognatura <> -1 Then
    '        sSQL+= CInt(odaticontratto.ncodfognatura) & ","
    '    Else
    '        sSQL+=" Null,"
    '    End If

    '    If odaticontratto.ncoddepurazione <> -1 Then
    '        sSQL+= CInt(odaticontratto.ncoddepurazione) & ","
    '    Else
    '        sSQL+="Null,"
    '    End If

    '    sSQL+= odaticontratto.bEsenteFognatura & ","

    '    sSQL+= odaticontratto.bEsenteDepurazione & ","

    '    sSQL+= odaticontratto.bEsenteAcqua & ","

    '    If odaticontratto.nDiametroCont <> -1 Then
    '        sSQL+= CInt(odaticontratto.nDiametroCont) & ","
    '    Else
    '        sSQL+="Null,"
    '    End If

    '    If odaticontratto.nDiametroPresa <> -1 Then
    '        sSQL+= CInt(odaticontratto.nDiametroPresa) & ","
    '    Else
    '        sSQL+="Null,"
    '    End If

    '    sSQL+= CInt(IDValue) & ","

    '    sSQL+="'" & CStr(odaticontratto.sLatoStrada) & "',"

    '    sSQL+= odaticontratto.bignoramora & ","

    '    sSQL+="'" & CStr(odaticontratto.sidEnte) & "',"

    '    sSQL+="'" & odaticontratto.sidEnteAppartenenza & "',"

    '    If odaticontratto.nIdVia <> -1 Then
    '        sSQL+= CInt(odaticontratto.nIdVia) & ","
    '    Else
    '        sSQL+=" Null,"
    '    End If

    '    sSQL+="'" & CStr(odaticontratto.sCivico) & "',"

    '    sSQL+="'" & CStr(oReplace.GiraData(odaticontratto.sDataSospensioneUtenza)) & "',"

    '    sSQL+= odaticontratto.bUtenteSospeso & ","

    '    sSQL+="'" & CStr(odaticontratto.sCodiceFabbricante) & "',"

    '    sSQL+="'" & CStr(odaticontratto.sCifreContatore) & "',"

    '    If odaticontratto.nCodIva <> -1 Then
    '        sSQL+= CInt(odaticontratto.nCodIva) & ","
    '    Else
    '        sSQL+="Null,"
    '    End If

    '    sSQL+="'" & CStr(odaticontratto.sStatoContatore) & "',"

    '    sSQL+="'" & CStr(odaticontratto.sPenalita) & "',"

    '    sSQL+="'" & CStr(odaticontratto.sCodiceISTAT) & "',"

    '    sSQL+="'" & CStr(odaticontratto.sEsponente) & "',"

    '    If odaticontratto.nIdMinimo <> -1 Then
    '        sSQL+= CInt(odaticontratto.nIdMinimo) & ","
    '    Else
    '        sSQL+="Null,"
    '    End If

    '    If odaticontratto.nIdAttivita <> -1 Then
    '        sSQL+= CInt(odaticontratto.nIdAttivita) & ","
    '    Else
    '        sSQL+="Null,"
    '    End If

    '    If odaticontratto.sPiano <> "" Then
    '        sSQL+="'0',"
    '    Else
    '        sSQL+="Null, "
    '    End If

    '    If odaticontratto.sFoglio <> "" Then
    '        sSQL+="'" & CStr(odaticontratto.sFoglio) & "', "
    '    Else
    '        sSQL+="Null, "
    '    End If

    '    If odaticontratto.sNumero <> "" Then
    '        sSQL+="'" & CStr(odaticontratto.sNumero) & "', "
    '    Else
    '        sSQL+="Null, "
    '    End If


    '    If odaticontratto.nsubalterno <> 0 Then
    '        sSQL+= CInt(odaticontratto.nsubalterno) & ", "
    '    Else
    '        sSQL+="Null, "
    '    End If

    '    If odaticontratto.bIsPendente <> 1 Then
    '        sSQL+= 0 & ", "
    '    Else
    '        sSQL+= 1 & ", "
    '    End If

    '    If odaticontratto.nSpesa <> 0 Then
    '        sSQL+= odaticontratto.nspesa.ToString().Replace(",", ".") & ", "
    '    Else
    '        sSQL+="Null, "
    '    End If

    '    sSQL+= CInt(odaticontratto.nproprietario) & ","

    '    sSQL+= odaticontratto.INumeroUtente & ","

    '    If odaticontratto.nDiritti <> 0 Then
    '        sSQL+= odaticontratto.nDiritti.ToString().Replace(",", ".") & ","
    '    Else
    '        sSQL+="Null,"
    '    End If

    '    sSQL+="'" & CStr(oReplace.ReplaceChar(odaticontratto.sUbicazione)) & "'"

    '    sSQL+=" )"

    '    'Dim sqlCmdInsert As SqlCommand
    '    sqlConn.ConnectionString = ConstSession.StringConnection

    '    sqlConn.Open()
    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn)
    '    sqlCmdInsert.ExecuteNonQuery()
    '    '====================================================================================
    '    'INSERIMENTO ANAGRAFICHE NELLA TABELLA TR_CONTATORI_INTESTATARIO
    '    '====================================================================================

    '    sSQL=""
    '    sSQL="INSERT INTO TR_CONTATORI_INTESTATARIO" & vbCrLf
    '    sSQL+="(CODCONTATORE,COD_CONTRIBUENTE)" & vbCrLf
    '    sSQL+="VALUES ( " & vbCrLf
    '    sSQL+= CInt(MASSIMOIDValue) & "," & vbCrLf
    '    sSQL+= CInt(odaticontratto.nidIntestatario) & vbCrLf
    '    sSQL+=" )"

    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '    sqlCmdInsert.ExecuteNonQuery()

    '    '====================================================================================
    '    'INSERIMENTO ANAGRAFICHE NELLA TABELLA TR_CONTATORI_UTENTE
    '    '====================================================================================

    '    sSQL=""
    '    sSQL="INSERT INTO TR_CONTATORI_UTENTE" & vbCrLf
    '    sSQL+="(CODCONTATORE,COD_CONTRIBUENTE)" & vbCrLf
    '    sSQL+="VALUES ( " & vbCrLf
    '    sSQL+= CInt(MASSIMOIDValue) & "," & vbCrLf
    '    sSQL+= CInt(odaticontratto.nidUtente) & vbCrLf
    '    sSQL+=" )"

    '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '    sqlCmdInsert.ExecuteNonQuery()

    '    sqlConn.Close()

    'End If



    'If lngTipoOp = DBOperation.DB_UPDATE Then

    '    '******************************************************************************************
    '    'UPDATE TABELLA TP_CONTRATTI
    '    '******************************************************************************************

    '    Dim nuovoIDcalcolato As Int32

    '    sSQL="SELECT MAX(CODCONTRATTO) AS ID_MAX FROM TP_CONTRATTI" & vbCrLf
    '    drTemp = DBAccess.getdataview(sSQL)
    '    If drTemp.Read = True Then
    '        If Not IsDBNull(drTemp("ID_MAX")) Then
    '            nuovoIDcalcolato = drTemp("ID_MAX")
    '        End If
    '    End If
    '    drTemp.Close()

    '    nuovoIDcalcolato = nuovoIDcalcolato + 1


    '    '============
    '    Dim myVar As Int32
    '    sSQL="SELECT MAX(CODCONTATORE) AS ID_MAX FROM TP_CONTATORI" & vbCrLf
    '    drTemp = DBAccess.getdataview(sSQL)
    '    If drTemp.Read = True Then
    '        If Not IsDBNull(drTemp("ID_MAX")) Then
    '            myVar = drTemp("ID_MAX")
    '        End If
    '    End If
    '    drTemp.Close()

    '    sSQL="UPDATE TP_CONTRATTI SET "


    '    '=============================
    '    'INIZIO MODIFICHE ALE CAO
    '    '=============================
    '    'sSQL+=" NUMEROUTENTE=" & odaticontratto.INumeroUtente & ","
    '    sSQL+=" NOTERICHIESTASUB='" & oReplace.ReplaceChar(odaticontratto.snoteRichiestaSub) & "',"


    '    If odaticontratto.richiestaSub = False Then
    '        sSQL+=" RICHIESTASUB=0,"
    '    Else
    '        sSQL+="RICHIESTASUB=1,"
    '    End If

    '    sSQL+=" PROPRIETARIO=" & CInt(odaticontratto.nproprietario) & ","
    '    sSQL+=" PIANO ='0',"
    '    sSQL+=" FOGLIO ='" & CStr(odaticontratto.sFoglio) & "', "
    '    sSQL+=" NUMERO ='" & CStr(odaticontratto.sNumero) & "', "
    '    sSQL+=" SUBALTERNO=" & CInt(odaticontratto.nsubalterno) & ", "
    '    sSQL+=" SPESA =" & odaticontratto.nspesa.ToString().Replace(",", ".") & ", "
    '    sSQL+=" DIRITTI =" & odaticontratto.nDiritti.ToString().Replace(",", ".") & ", "
    '    sSQL+=" PENDENTE =" & CInt(odaticontratto.bIsPendente) & ", "


    '    sSQL+=" CODICECONTRATTO ='" & CStr(odaticontratto.nIdContratto) & "', "
    '    sSQL+=" DATASOTTOSCRIZIONE='" & CStr(oReplace.GiraData(odaticontratto.sdatasottoscrizione)) & "', "
    '    '=============================
    '    'FINE MODIFICHE ALE CAO
    '    '=============================


    '    sSQL+=" CODENTE = " & CInt(odaticontratto.sidEnte) & ", "

    '    'sSQL+=" NUMEROUTENZE = '" & CStr(odaticontratto.sNumeroUtenze) & "', "
    '    If odaticontratto.nIdImpianto <> -1 Then
    '        sSQL+=" CODIMPIANTO = " & CInt(odaticontratto.nIdImpianto) & ", "
    '    Else
    '        sSQL+=" CODIMPIANTO = Null, "
    '    End If
    '    If odaticontratto.nGiro <> -1 Then
    '        sSQL+=" IDGIRO = " & CInt(odaticontratto.nGiro) & ", "
    '    Else
    '        sSQL+=" IDGIRO = Null, "
    '    End If
    '    sSQL+=" SEQUENZA = '" & CStr(odaticontratto.sSequenza) & "', "

    '    If odaticontratto.nTipoContatore <> -1 Then
    '        sSQL+=" IDTIPOCONTATORE = " & CInt(odaticontratto.nTipoContatore) & ", "
    '    Else
    '        sSQL+=" IDTIPOCONTATORE = Null, "
    '    End If
    '    'If odaticontratto.nTipoUtenza <> -1 Then
    '    '    sSQL+=" IDTIPOUTENZA = " & CInt(odaticontratto.nTipoUtenza) & ", "
    '    'Else
    '    '    sSQL+=" IDTIPOUTENZA = Null, "
    '    'End If
    '    sSQL+=" CODPOSIZIONE = " & CInt(odaticontratto.nPosizione) & ", "
    '    sSQL+=" POSIZIONEPROGRESSIVA = '" & CStr(odaticontratto.sProgressivo) & "', "
    '    sSQL+=" NOTE = '" & CStr(oReplace.ReplaceChar(odaticontratto.sNote)) & "', "
    '    sSQL+=" DATAATTIVAZIONE = '" & CStr(oReplace.GiraData(odaticontratto.sDataAttivazione)) & "', "
    '    sSQL+=" DATACESSAZIONE = '" & CStr(oReplace.GiraData(odaticontratto.sDataCessazione)) & "', "
    '    'If odaticontratto.ncodfognatura <> -1 Then
    '    '    sSQL+=" CODFOGNATURA = " & CInt(odaticontratto.ncodfognatura) & ", "
    '    'Else
    '    '    sSQL+=" CODFOGNATURA = Null, "
    '    'End If
    '    'If odaticontratto.ncoddepurazione <> -1 Then
    '    '    sSQL+=" CODDEPURAZIONE = " & CInt(odaticontratto.ncoddepurazione) & ", "
    '    'Else
    '    '    sSQL+=" CODDEPURAZIONE = Null, "
    '    'End If
    '    'sSQL+=" ESENTEFOGNATURA = " & odaticontratto.bEsenteFognatura & ", "
    '    'sSQL+=" ESENTEDEPURAZIONE = " & odaticontratto.bEsenteDepurazione & ", "



    '    If odaticontratto.nDiametroCont <> -1 Then
    '        sSQL+=" CODDIAMETROCONTATORE = " & CInt(odaticontratto.nDiametroCont) & ", "
    '    Else
    '        sSQL+=" CODDIAMETROCONTATORE = Null, "
    '    End If



    '    If odaticontratto.nDiametroPresa <> -1 Then
    '        sSQL+=" CODDIAMETROPRESA = " & CInt(odaticontratto.nDiametroPresa) & ", "
    '    Else
    '        sSQL+=" CODDIAMETROPRESA = Null, "
    '    End If




    '    If odaticontratto.nIdMinimo <> -1 Then
    '        sSQL+=" IDMINIMO = " & CInt(odaticontratto.nIdMinimo) & ", "
    '    Else
    '        sSQL+=" IDMINIMO = Null, "
    '    End If
    '    sSQL+=" LATOSTRADA = '" & CStr(odaticontratto.sLatoStrada) & "', "
    '    sSQL+=" IGNORAMORA = " & odaticontratto.bignoramora & ", "
    '    sSQL+=" CODENTE1 = '" & CStr(odaticontratto.sidEnte) & "', "
    '    sSQL+=" CODENTEAPPARTENENZA1 = '" & CStr(odaticontratto.sidEnteAppartenenza) & "', "
    '    'If odaticontratto.nIdVia <> -1 Then
    '    '    sSQL+=" COD_STRADA = " & CInt(odaticontratto.nIdVia) & ", "
    '    'Else
    '    '    sSQL+=" COD_STRADA = Null, "
    '    'End If
    '    'sSQL+=" CIVICO_UBICAZIONE = '" & CStr(odaticontratto.sCivico) & "', "
    '    sSQL+=" DATASOSPENSIONEUTENZA = '" & CStr(oReplace.GiraData(odaticontratto.sDataSospensioneUtenza)) & "', "
    '    sSQL+=" UTENTESOSPESO = " & odaticontratto.bUtenteSospeso & ", "

    '    sSQL+=" CODICEFABBRICANTE = '" & CStr(odaticontratto.sCodiceFabbricante) & "', "
    '    sSQL+=" CIFRECONTATORE = '" & CStr(odaticontratto.sCifreContatore) & "', "
    '    If odaticontratto.nCodIva <> -1 Then
    '        sSQL+=" CODIVA = " & CInt(odaticontratto.nCodIva) & ", "
    '    Else
    '        sSQL+=" CODIVA = Null, "
    '    End If
    '    sSQL+=" STATOCONTATORE = '" & CStr(odaticontratto.sStatoContatore) & "', "
    '    sSQL+=" PENALITA = '" & CStr(odaticontratto.sPenalita) & "', "
    '    sSQL+=" CODICE_ISTAT = '" & CStr(odaticontratto.sCodiceISTAT) & "', "
    '    'sSQL+=" ESPONENTE_CIVICO = '" & CStr(odaticontratto.sEsponente) & "', "
    '    If odaticontratto.nIdAttivita <> -1 Then
    '        sSQL+=" IDTIPOATTIVITA = " & CInt(odaticontratto.nIdAttivita) & " "
    '    Else
    '        sSQL+=" IDTIPOATTIVITA = Null "
    '    End If

    '    '====================================================================================
    '    'PER PRODUZIONE
    '    '====================================================================================
    '    'Prelevo gli Ultimi 3 caratteri del codice impianto

    '    strAppoggio = Right(utility.stringoperation.formatstring(odaticontratto.nIdImpianto), 3)
    '    strAppoggio = strAppoggio.PadLeft(3, "0")

    '    sSQL+=" WHERE CODCONTRATTO =" & utility.stringoperation.formatstring(CodContratto)


    '    odaticontratto.nidcontratto = utility.stringoperation.formatstring(CodContratto)

    '    Try

    '        Dim sqlCmdInsert As SqlCommand
    '        sqlConn.ConnectionString = ConstSession.StringConnection

    '        sqlConn.Open()
    '        sqlTrans = sqlConn.BeginTransaction
    '        sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)


    '        sqlCmdInsert.ExecuteNonQuery()

    '        '******************************************************************************************
    '        'UPDATE ANAGRAFICHE TABELLA TR_CONTRATTI_INTESTATARIO
    '        '******************************************************************************************
    '        sSQL=""
    '        sSQL="UPDATE TR_CONTRATTI_INTESTATARIO SET" & vbCrLf
    '        sSQL+="COD_CONTRIBUENTE=" & utility.stringoperation.formatint(odaticontratto.nidIntestatario) & vbCrLf
    '        sSQL+="WHERE CODCONTRATTO =" & utility.stringoperation.formatstring(CodContratto)

    '        sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '        sqlCmdInsert.ExecuteNonQuery()


    '        '******************************************************************************************
    '        'UPDATE ANAGRAFICHE TABELLA TR_CONTRATTI_UTENTE
    '        '******************************************************************************************
    '        sSQL=""
    '        sSQL="UPDATE TR_CONTRATTI_UTENTE SET" & vbCrLf
    '        sSQL+="COD_CONTRIBUENTE=" & utility.stringoperation.formatint(odaticontratto.nidUtente) & vbCrLf
    '        sSQL+="WHERE CODCONTRATTO=" & utility.stringoperation.formatstring(CodContratto)


    '        Dim CodContrattoPerVoltura As Int32
    '        CodContrattoPerVoltura = CodContratto



    '        sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '        sqlCmdInsert.ExecuteNonQuery()



    '        Dim drContatoreAssociato As new dataview

    '        Dim ultimoContratto As Int32
    '        ultimoContratto = CInt(utility.stringoperation.formatstring(CodContratto))
    '        Dim newSql As String
    '        newSql = "SELECT CODCONTATORE FROM TP_CONTATORI WHERE CODCONTRATTO=" & utility.stringoperation.formatstring(CodContratto)

    '        drContatoreAssociato = DBAccess.getdataview(newSql)
    '        Dim myContatore As Int32
    '        While drContatoreAssociato.Read()
    '            myContatore = drContatoreAssociato("CODCONTATORE")
    '        End While
    '        drContatoreAssociato.Close()

    '        '******************************************************************************************
    '        'UPDATE ANAGRAFICHE TABELLA TR_CONTATORI_UTENTE
    '        '******************************************************************************************
    '        sSQL=""
    '        sSQL="UPDATE TR_CONTATORI_UTENTE SET " & vbCrLf
    '        sSQL+="COD_CONTRIBUENTE=" & utility.stringoperation.formatint(odaticontratto.nidUtente) & vbCrLf
    '        sSQL+="WHERE CODCONTATORE=" & utility.stringoperation.formatstring(myContatore)
    '        sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '        sqlCmdInsert.ExecuteNonQuery()

    '        '******************************************************************************************
    '        'UPDATE ANAGRAFICHE TABELLA TR_CONTATORI_INTESTATARIO
    '        '******************************************************************************************
    '        sSQL=""
    '        sSQL="UPDATE TR_CONTATORI_INTESTATARIO SET " & vbCrLf
    '        sSQL+="COD_CONTRIBUENTE=" & utility.stringoperation.formatint(odaticontratto.nidIntestatario) & vbCrLf
    '        sSQL+="WHERE CODCONTATORE=" & utility.stringoperation.formatstring(myContatore)
    '        sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '        sqlCmdInsert.ExecuteNonQuery()


    '        '==================================================
    '        'UPDATE DATI RIDONDANTI
    '        '==================================================
    '        sSQL=""
    '        sSQL+="UPDATE TP_CONTATORI SET"
    '        sSQL+=" NUMEROUTENTE=" & CInt(odaticontratto.INumeroUtente) & ","
    '        If odaticontratto.nIdVia <> -1 Then
    '            sSQL+="COD_STRADA=" & CInt(odaticontratto.nIdVia) & ","
    '        Else
    '            sSQL+=" COD_STRADA=Null,"
    '        End If
    '        sSQL+=" CIVICO_UBICAZIONE='" & odaticontratto.sCivico & "',"
    '        sSQL+=" ESPONENTE_CIVICO='" & odaticontratto.sEsponente & "',"


    '        If odaticontratto.ncodfognatura <> -1 Then
    '            sSQL+=" CODFOGNATURA = " & CInt(odaticontratto.ncodfognatura) & ", "
    '        Else
    '            sSQL+=" CODFOGNATURA = Null, "
    '        End If
    '        If odaticontratto.ncoddepurazione <> -1 Then
    '            sSQL+=" CODDEPURAZIONE = " & CInt(odaticontratto.ncoddepurazione) & ", "
    '        Else
    '            sSQL+=" CODDEPURAZIONE = Null, "
    '        End If
    '        sSQL+=" ESENTEFOGNATURA = " & odaticontratto.bEsenteFognatura & ", "
    '        sSQL+=" ESENTEDEPURAZIONE = " & odaticontratto.bEsenteDepurazione & ", "
    '        sSQL+=" ESENTEACQUA = " & odaticontratto.bEsenteAcqua & ","
    '        sSQL+=" NUMEROUTENZE = " & CInt(odaticontratto.sNumeroUtenze) & ", "

    '        If odaticontratto.nTipoUtenza <> -1 Then
    '            sSQL+=" IDTIPOUTENZA = " & CInt(odaticontratto.nTipoUtenza) & ","
    '        Else
    '            sSQL+=" IDTIPOUTENZA = Null, "
    '        End If

    '        sSQL+=" VIA_UBICAZIONE= '" & CStr(oReplace.ReplaceChar(odaticontratto.sUbicazione)) & "'"

    '        'sSQL+=" WHERE CODCONTATORE=" & utility.stringoperation.formatstring(myContatore)
    '        sSQL+=" WHERE CODCONTATORE=" & myContatore

    '        sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '        sqlCmdInsert.ExecuteNonQuery()


    '        'vado a selezionarmi l'id del contatore da aggiornare per il contratto aggiornato
    '        sSQL=""
    '        sSQL="SELECT CODCONTATORE"
    '        sSQL+=" FROM TP_CONTATORI"
    '        sSQL+=" WHERE CODCONTRATTO=" & ultimoContratto


    '        Dim drmioContratto As new dataview
    '        drmioContratto = DBAccess.getdataview(sSQL)

    '        While drmioContratto.Read()
    '            mioContratto = drmioContratto("CODCONTATORE")
    '        End While

    '        drmioContratto.Close()





    '        If odaticontratto.sDataCessazione <> "" Then
    '            'se la data di cessazione è popolata, creo un nuovo contratto identico 
    '            'a quello modificato
    '            'ma senza popolare le anagrafiche per l'intestatario e l'utente



    '            Dim passaQua As String = "Ok, qua verrà effettuata la voltura del contratto"



    '            sSQL="INSERT INTO TP_CONTRATTI"

    '            '=========================
    '            'INIZIO MODIFICHE ALE CAO
    '            '=========================

    '            sSQL+="(PIANO,"
    '            sSQL+=" PROPRIETARIO,"
    '            sSQL+=" RICHIESTASUB,"
    '            sSQL+=" NOTERICHIESTASUB,"
    '            'sSQL+=" NUMEROUTENTE,"
    '            sSQL+=" CODCONTRATTO,"


    '            sSQL+=" FOGLIO,"
    '            sSQL+=" NUMERO,"
    '            sSQL+=" SUBALTERNO,"

    '            sSQL+=" SPESA,"
    '            sSQL+=" DIRITTI,"
    '            sSQL+=" PENDENTE,"
    '            '=========================
    '            'FINE MODIFICHE ALE CAO
    '            '=========================

    '            sSQL+=" CODENTE,"

    '            'sSQL+=" NUMEROUTENZE,"
    '            sSQL+=" CODIMPIANTO,"
    '            sSQL+=" IDGIRO,"
    '            sSQL+=" SEQUENZA,"

    '            sSQL+=" IDTIPOCONTATORE,"
    '            'sSQL+=" IDTIPOUTENZA,"
    '            sSQL+=" CODPOSIZIONE,"
    '            sSQL+=" POSIZIONEPROGRESSIVA,"
    '            sSQL+=" NOTE,"
    '            sSQL+=" DATAATTIVAZIONE,"

    '            'sSQL+=" CODFOGNATURA,"
    '            'sSQL+=" CODDEPURAZIONE,"
    '            'sSQL+=" ESENTEFOGNATURA,"
    '            'sSQL+=" ESENTEDEPURAZIONE,"
    '            sSQL+=" CODDIAMETROCONTATORE,"
    '            sSQL+=" CODDIAMETROPRESA,"

    '            sSQL+=" IDMINIMO,"
    '            sSQL+=" LATOSTRADA,"
    '            sSQL+=" IGNORAMORA,"
    '            sSQL+=" CODENTE1,"
    '            sSQL+=" CODENTEAPPARTENENZA1,"
    '            'sSQL+=" COD_STRADA,"
    '            'sSQL+=" CIVICO_UBICAZIONE,"
    '            sSQL+=" DATASOSPENSIONEUTENZA,"
    '            sSQL+=" UTENTESOSPESO,"

    '            sSQL+=" CODICEFABBRICANTE,"
    '            sSQL+=" CIFRECONTATORE,"
    '            sSQL+=" CODIVA,"
    '            sSQL+=" STATOCONTATORE,"
    '            sSQL+=" PENALITA,"
    '            sSQL+=" CODICE_ISTAT,"
    '            'sSQL+=" ESPONENTE_CIVICO,"

    '            sSQL+=" IDTIPOATTIVITA)"

    '            sSQL+=" VALUES ( "



    '            '=========================
    '            'INIZIO MODIFICHE ALE CAO
    '            '=========================

    '            If odaticontratto.sPiano <> "" Then
    '                sSQL+="'0',"
    '            Else
    '                sSQL+="Null, "
    '            End If

    '            sSQL+= CInt(odaticontratto.nproprietario) & ","

    '            'sSQL+= odaticontratto.richiestaSub & ","
    '            If odaticontratto.richiestaSub = False Then
    '                sSQL+= 0 & ","
    '            Else
    '                sSQL+= 1 & ","
    '            End If



    '            sSQL+="'" & oReplace.ReplaceChar(odaticontratto.snoteRichiestaSub) & "',"

    '            'sSQL+= odaticontratto.INumeroUtente & ","

    '            sSQL+= nuovoIDcalcolato & ", "

    '            Dim codcontrattoInContatori As Int32
    '            codcontrattoInContatori = nuovoIDcalcolato

    '            If odaticontratto.sFoglio <> "" Then
    '                sSQL+="'" & CStr(odaticontratto.sFoglio) & "', "
    '            Else
    '                sSQL+="Null, "
    '            End If



    '            If odaticontratto.sNumero <> "" Then
    '                sSQL+="'" & CStr(odaticontratto.sNumero) & "', "
    '            Else
    '                sSQL+="Null, "
    '            End If


    '            If odaticontratto.nsubalterno <> 0 Then
    '                sSQL+= CInt(odaticontratto.nsubalterno) & ", "
    '            Else
    '                sSQL+="Null, "
    '            End If

    '            If odaticontratto.nSpesa <> 0 Then
    '                sSQL+= odaticontratto.nspesa.ToString().Replace(",", ".") & ", "
    '            Else
    '                sSQL+="Null, "
    '            End If


    '            If odaticontratto.nDiritti <> 0 Then
    '                sSQL+= odaticontratto.nDiritti.ToString().Replace(",", ".") & ", "
    '            Else
    '                sSQL+="Null, "
    '            End If

    '            If odaticontratto.bIsPendente <> 1 Then
    '                sSQL+= 0 & ", "
    '            Else
    '                sSQL+= 1 & ", "
    '            End If

    '            '=========================
    '            'FINE MODIFICHE ALE CAO
    '            '=========================


    '            sSQL+= CInt(odaticontratto.sidEnte) & ","

    '            'sSQL+="'" & CStr(odaticontratto.sNumeroUtenze) & "', "
    '            If odaticontratto.nIdImpianto <> -1 Then
    '                sSQL+= CInt(odaticontratto.nIdImpianto) & ", "
    '            Else
    '                sSQL+=" Null, "
    '            End If
    '            If odaticontratto.nGiro <> -1 Then
    '                sSQL+= CInt(odaticontratto.nGiro) & ","
    '            Else
    '                sSQL+=" Null,"
    '            End If
    '            sSQL+="'" & CStr(odaticontratto.sSequenza) & "',"

    '            If odaticontratto.nTipoContatore <> -1 Then
    '                sSQL+= CInt(odaticontratto.nTipoContatore) & ","
    '            Else
    '                sSQL+=" Null,"
    '            End If
    '            'If odaticontratto.nTipoUtenza <> -1 Then
    '            '    sSQL+= CInt(odaticontratto.nTipoUtenza) & ","
    '            'Else
    '            '    sSQL+=" Null,"
    '            'End If
    '            sSQL+= CInt(odaticontratto.nPosizione) & ","
    '            sSQL+="'" & CStr(odaticontratto.sProgressivo) & "',"
    '            sSQL+="'" & CStr(oReplace.ReplaceChar(odaticontratto.sNote)) & "',"
    '            sSQL+="'" & CStr(oReplace.GiraData(odaticontratto.sDataAttivazione)) & "',"


    '            If odaticontratto.nDiametroCont <> -1 Then
    '                sSQL+= CInt(odaticontratto.nDiametroCont) & ","
    '            Else
    '                sSQL+="Null,"
    '            End If

    '            If odaticontratto.nDiametroPresa <> -1 Then
    '                sSQL+= CInt(odaticontratto.nDiametroPresa) & ","
    '            Else
    '                sSQL+="Null,"
    '            End If

    '            If odaticontratto.nIdMinimo <> -1 Then
    '                sSQL+= CInt(odaticontratto.nIdMinimo) & ","
    '            Else
    '                sSQL+="Null,"
    '            End If
    '            sSQL+="'" & CStr(odaticontratto.sLatoStrada) & "',"
    '            sSQL+= odaticontratto.bignoramora & ","
    '            sSQL+="'" & CStr(odaticontratto.sidEnte) & "',"
    '            sSQL+="'" & odaticontratto.sidEnteAppartenenza & "',"

    '            sSQL+="'" & CStr(oReplace.GiraData(odaticontratto.sDataSospensioneUtenza)) & "',"
    '            sSQL+= odaticontratto.bUtenteSospeso & ","

    '            sSQL+="'" & CStr(odaticontratto.sCodiceFabbricante) & "',"
    '            sSQL+="'" & CStr(odaticontratto.sCifreContatore) & "',"
    '            If odaticontratto.nCodIva <> -1 Then
    '                sSQL+= CInt(odaticontratto.nCodIva) & ","
    '            Else
    '                sSQL+="Null,"
    '            End If
    '            sSQL+="'" & CStr(odaticontratto.sStatoContatore) & "',"
    '            sSQL+="'" & CStr(odaticontratto.sPenalita) & "',"
    '            sSQL+="'" & CStr(odaticontratto.sCodiceISTAT) & "',"




    '            ' '====================================================================================
    '            ' 'PER PRODUZIONE
    '            ' '====================================================================================
    '            ' 'Prelevo gli Ultimi 3 caratteri del codice impianto
    '            strAppoggio = Right(utility.stringoperation.formatstring(odaticontratto.nIdImpianto), 3)
    '            strAppoggio = strAppoggio.PadLeft(3, "0")


    '            If odaticontratto.nIdAttivita <> -1 Then
    '                sSQL+= CInt(odaticontratto.nIdAttivita)
    '            Else
    '                sSQL+="Null"
    '            End If
    '            sSQL+=" )"


    '            sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '            sqlCmdInsert.ExecuteNonQuery()
    '            '=========================================================
    '            'INSERIMENTO DELLE ANAGRAFICHE
    '            '=========================================================

    '            '==========================================
    '            'INSERIMENTO ANAGRAFICA INTESTATARIO
    '            '==========================================

    '            sSQL=""
    '            sSQL+="INSERT INTO TR_CONTRATTI_INTESTATARIO"
    '            sSQL+="(CODCONTRATTO,COD_CONTRIBUENTE)"
    '            sSQL+="VALUES ("
    '            sSQL+= nuovoIDcalcolato & ","
    '            sSQL+= -1 & ")"

    '            sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '            sqlCmdInsert.ExecuteNonQuery()



    '            '==========================================
    '            'INSERIMENTO ANAGRAFICA UTENTE
    '            '==========================================

    '            sSQL=""
    '            sSQL+="INSERT INTO TR_CONTRATTI_UTENTE"
    '            sSQL+="(CODCONTRATTO,COD_CONTRIBUENTE)"
    '            sSQL+="VALUES ("
    '            sSQL+= nuovoIDcalcolato & ","
    '            sSQL+= -1 & ")"

    '            sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '            sqlCmdInsert.ExecuteNonQuery()



    '            odaticontratto.nidcontratto = nuovoIDcalcolato
    '            'questa nuova variabile mi serve per reindirizzare la pagina nel nuovo contratto creato
    '            'alla fine del metodo ritorna a datigeneralicontr.aspx e come parametro del contratto mette 
    '            'nuovoIDCalcolato, che sarebbe il nuovo contratto senza data di sottoscrizione, codice
    '            'in stringa del contratto e le 2 anagrafiche, cioè di utente e intestatario


    '            'seleziono l'id del contatore a cui era associato il vecchio contratto
    '            ' è uguale alla variabile ==> mioContratto

    '            'COMMENTO QUA NON SERVE +
    '            'sSQL=""
    '            'sSQL+="UPDATE TP_CONTATORI SET"
    '            'sSQL+=" CODCONTRATTO=" & nuovoIDcalcolato
    '            'sSQL+=" WHERE CODCONTATORE=" & mioContratto

    '            'sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '            'sqlCmdInsert.ExecuteNonQuery()


    '            'CREO IL NUOVO CONTATORE DA ASSOCIARE AL NUOVO CONTRATTO
    '            Dim getNewID As Int32

    '            'drTemp.Close()
    '            'Dim dsTemp As DataSet

    '            'sSQL="SELECT MAX(CODCONTATORE) AS ID_MAX FROM TP_CONTATORI" & vbCrLf
    '            'dsTemp = DBAccess.RunSQLReturnDataSet(sSQL)
    '            ''If drTemp.Read = True Then
    '            ''    If Not IsDBNull(drTemp("ID_MAX")) Then
    '            ''        getNewID = drTemp("ID_MAX")
    '            ''    End If
    '            ''End If
    '            'Dim dtTemp As DataTable
    '            'dtTemp = dsTemp.Tables(0)
    '            'getNewID = dtTemp.Rows(0)("ID_MAX")
    '            'drTemp.Close()
    '            'ID NUOVO CONTATORE
    '            getNewID = myVar + 1

    '            sSQL=""
    '            sSQL+="INSERT INTO TP_CONTATORI "
    '            sSQL+=" (PROPRIETARIO,CODCONTATORE,CODCONTRATTO,CODENTE,CODENTEAPPARTENENZA, CODIMPIANTO, IDGIRO, IDTIPOCONTATORE, "
    '            sSQL+="IDTIPOUTENZA,NUMEROUTENZE, CODPOSIZIONE, CODFOGNATURA, CODDEPURAZIONE, "
    '            sSQL+="CODDIAMETROCONTATORE, CODDIAMETROPRESA, CODLETTURISTA, CODPDA,"
    '            sSQL+="COD_STRADA, CODIVA, IDTIPOATTIVITA,"

    '            sSQL+="CODENTE1,CODENTEAPPARTENENZA1,CIVICO_UBICAZIONE, FRAZIONE_UBICAZIONE, QUOTEAGEVOLATE,"
    '            sSQL+="CODICEFABBRICANTE, CIFRECONTATORE, STATOCONTATORE, PENALITA, CODICE_ISTAT,"
    '            sSQL+="CODICE_PUNTO_PRESA, ESPONENTE_CIVICO, DIAMETROCONTATORE,"
    '            sSQL+="ESENTEACQUA,ESENTEFOGNATURA, ESENTEDEPURAZIONE, SCARICATOSUPDA, LETTO, DARICONTROLLARE,"
    '            sSQL+="MODULOAUTOLETTURA, LASCIATOAVVISO, ANOMALIA, IGNORAMORA, UTENTESOSPESO, VIA_UBICAZIONE"
    '            sSQL+=")"
    '            sSQL+=" SELECT 0," & getNewID & "," & nuovoIDcalcolato & ","
    '            sSQL+="CODENTE,CODENTEAPPARTENENZA,CODIMPIANTO,IDGIRO,IDTIPOCONTATORE,"
    '            sSQL+="IDTIPOUTENZA,NUMEROUTENZE,CODPOSIZIONE,CODFOGNATURA,CODDEPURAZIONE,"
    '            sSQL+="CODDIAMETROCONTATORE,CODDIAMETROPRESA,CODLETTURISTA,CODPDA,"
    '            sSQL+="COD_STRADA, CODIVA, IDTIPOATTIVITA,"
    '            sSQL+="CODENTE1,CODENTEAPPARTENENZA1,CIVICO_UBICAZIONE, FRAZIONE_UBICAZIONE, QUOTEAGEVOLATE,"
    '            sSQL+="CODICEFABBRICANTE, CIFRECONTATORE, STATOCONTATORE, PENALITA, CODICE_ISTAT,"
    '            sSQL+="CODICE_PUNTO_PRESA, ESPONENTE_CIVICO, DIAMETROCONTATORE,"
    '            sSQL+="ESENTEACQUA,ESENTEFOGNATURA, ESENTEDEPURAZIONE, SCARICATOSUPDA, LETTO, DARICONTROLLARE,"
    '            sSQL+="MODULOAUTOLETTURA, LASCIATOAVVISO, ANOMALIA, IGNORAMORA, UTENTESOSPESO, VIA_UBICAZIONE"
    '            sSQL+=" FROM TP_CONTATORI WHERE CODCONTRATTO=" & CodContrattoPerVoltura
    '            '===================================
    '            'VECCHIA QUERY VOLTURA
    '            '===================================
    '            'Dim drDatiContatore As new dataview
    '            'sSQL="SELECT * FROM TP_CONTATORI WHERE CODCONTRATTO=" & CodContrattoPerVoltura

    '            'drDatiContatore = DBAccess.getdataview(sSQL)

    '            'Dim NUMEROUTENZE, CODENTE, CODENTEAPPARTENENZA, CODIMPIANTO, IDGIRO, IDTIPOCONTATORE, IDTIPOUTENZA, CODPOSIZIONE, CODFOGNATURA, CODDEPURAZIONE, _
    '            'CONSUMOSTIMATO, CODDIAMETROCONTATORE, CODDIAMETROPRESA, CODLETTURISTA, CODPDA _
    '            ', COD_STRADA, CODIVA, IDTIPOATTIVITA As Int16


    '            'Dim SEQUENZA, POSIZIONEPROGRESSIVA, NOTE, LATOSTRADA _
    '            ', CODENTE1, CODENTEAPPARTENENZA1, CIVICO_UBICAZIONE, FRAZIONE_UBICAZIONE, QUOTEAGEVOLATE _
    '            ', CODICEFABBRICANTE, CIFRECONTATORE, STATOCONTATORE, PENALITA, CODICE_ISTAT _
    '            ', CODICE_PUNTO_PRESA, ESPONENTE_CIVICO, DIAMETROCONTATORE, VIA_UBICAZIONE As String

    '            'Dim ESENTEACQUA, ESENTEFOGNATURA, ESENTEDEPURAZIONE, SCARICATOSUPDA, LETTO, DARICONTROLLARE, _
    '            'MODULOAUTOLETTURA, LASCIATOAVVISO, ANOMALIA, IGNORAMORA, UTENTESOSPESO As Boolean

    '            'Dim codcontatorePerCatastali As Int32
    '            'codcontatorePerCatastali = -1

    '            'While drDatiContatore.Read()

    '            '    If Not IsDBNull(drDatiContatore("CODCONTATORE")) Then
    '            '        codcontatorePerCatastali = drDatiContatore("CODCONTATORE")
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODENTE")) Then
    '            '        CODENTE = drDatiContatore("CODENTE")
    '            '    Else
    '            '        CODENTE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODENTEAPPARTENENZA")) Then
    '            '        CODENTEAPPARTENENZA = drDatiContatore("CODENTEAPPARTENENZA")
    '            '    Else
    '            '        CODENTEAPPARTENENZA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("NUMEROUTENZE")) Then
    '            '        NUMEROUTENZE = drDatiContatore("NUMEROUTENZE")
    '            '    Else
    '            '        NUMEROUTENZE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODIMPIANTO")) Then
    '            '        CODIMPIANTO = drDatiContatore("CODIMPIANTO")
    '            '    Else
    '            '        CODIMPIANTO = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("IDGIRO")) Then
    '            '        IDGIRO = drDatiContatore("IDGIRO")
    '            '    Else
    '            '        IDGIRO = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("SEQUENZA")) Then
    '            '        SEQUENZA = drDatiContatore("SEQUENZA")
    '            '    Else
    '            '        SEQUENZA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("IDTIPOCONTATORE")) Then
    '            '        IDTIPOCONTATORE = drDatiContatore("IDTIPOCONTATORE")
    '            '    Else
    '            '        IDTIPOCONTATORE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("IDTIPOUTENZA")) Then
    '            '        IDTIPOUTENZA = drDatiContatore("IDTIPOUTENZA")
    '            '    Else
    '            '        IDTIPOUTENZA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODPOSIZIONE")) Then
    '            '        CODPOSIZIONE = drDatiContatore("CODPOSIZIONE")
    '            '    Else
    '            '        CODPOSIZIONE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("POSIZIONEPROGRESSIVA")) Then
    '            '        POSIZIONEPROGRESSIVA = drDatiContatore("POSIZIONEPROGRESSIVA")
    '            '    Else
    '            '        POSIZIONEPROGRESSIVA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("NOTE")) Then
    '            '        NOTE = drDatiContatore("NOTE")
    '            '    Else
    '            '        NOTE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODFOGNATURA")) Then
    '            '        CODFOGNATURA = drDatiContatore("CODFOGNATURA")
    '            '    Else
    '            '        CODFOGNATURA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODDEPURAZIONE")) Then
    '            '        CODDEPURAZIONE = drDatiContatore("CODDEPURAZIONE")
    '            '    Else
    '            '        CODDEPURAZIONE = Nothing
    '            '    End If

    '            '    'Dim ESENTEFOGNATURA, ESENTEDEPURAZIONE, SCARICATOSUPDA, LETTO, DARICONTROLLARE, 
    '            '    'MODULOAUTOLETTURA, LASCIATOAVVISO, ANOMALIA, IGNORAMORA, UTENTESOSPESO As Boolean

    '            '    If Not IsDBNull(drDatiContatore("ESENTEACQUA")) Then
    '            '        ESENTEACQUA = drDatiContatore("ESENTEACQUA")
    '            '    Else
    '            '        ESENTEACQUA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("ESENTEFOGNATURA")) Then
    '            '        ESENTEFOGNATURA = drDatiContatore("ESENTEFOGNATURA")
    '            '    Else
    '            '        ESENTEFOGNATURA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("ESENTEDEPURAZIONE")) Then
    '            '        ESENTEDEPURAZIONE = drDatiContatore("ESENTEDEPURAZIONE")
    '            '    Else
    '            '        ESENTEDEPURAZIONE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CONSUMOSTIMATO")) Then
    '            '        CONSUMOSTIMATO = drDatiContatore("CONSUMOSTIMATO")
    '            '    Else
    '            '        CONSUMOSTIMATO = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODDIAMETROCONTATORE")) Then
    '            '        CODDIAMETROCONTATORE = drDatiContatore("CODDIAMETROCONTATORE")
    '            '    Else
    '            '        CODDIAMETROCONTATORE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODDIAMETROPRESA")) Then
    '            '        CODDIAMETROPRESA = drDatiContatore("CODDIAMETROPRESA")
    '            '    Else
    '            '        CODDIAMETROPRESA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("SCARICATOSUPDA")) Then
    '            '        SCARICATOSUPDA = drDatiContatore("SCARICATOSUPDA")
    '            '    Else
    '            '        SCARICATOSUPDA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("LETTO")) Then
    '            '        LETTO = drDatiContatore("LETTO")
    '            '    Else
    '            '        LETTO = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("DARICONTROLLARE")) Then
    '            '        DARICONTROLLARE = drDatiContatore("DARICONTROLLARE")
    '            '    Else
    '            '        DARICONTROLLARE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("MODULOAUTOLETTURA")) Then
    '            '        MODULOAUTOLETTURA = drDatiContatore("MODULOAUTOLETTURA")
    '            '    Else
    '            '        MODULOAUTOLETTURA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODLETTURISTA")) Then
    '            '        CODLETTURISTA = drDatiContatore("CODLETTURISTA")
    '            '    Else
    '            '        CODLETTURISTA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODPDA")) Then
    '            '        CODPDA = drDatiContatore("CODPDA")
    '            '    Else
    '            '        CODPDA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("LATOSTRADA")) Then
    '            '        LATOSTRADA = drDatiContatore("LATOSTRADA")
    '            '    Else
    '            '        LATOSTRADA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("LASCIATOAVVISO")) Then
    '            '        LASCIATOAVVISO = drDatiContatore("LASCIATOAVVISO")
    '            '    Else
    '            '        LASCIATOAVVISO = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("ANOMALIA")) Then
    '            '        ANOMALIA = drDatiContatore("ANOMALIA")
    '            '    Else
    '            '        ANOMALIA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("IGNORAMORA")) Then
    '            '        IGNORAMORA = drDatiContatore("IGNORAMORA")
    '            '    Else
    '            '        IGNORAMORA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODENTE1")) Then
    '            '        CODENTE1 = drDatiContatore("CODENTE1")
    '            '    Else
    '            '        CODENTE1 = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODENTEAPPARTENENZA1")) Then
    '            '        CODENTEAPPARTENENZA1 = drDatiContatore("CODENTEAPPARTENENZA1")
    '            '    Else
    '            '        CODENTEAPPARTENENZA1 = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("COD_STRADA")) Then
    '            '        COD_STRADA = drDatiContatore("COD_STRADA")
    '            '    Else
    '            '        COD_STRADA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CIVICO_UBICAZIONE")) Then
    '            '        CIVICO_UBICAZIONE = drDatiContatore("CIVICO_UBICAZIONE")
    '            '    Else
    '            '        CIVICO_UBICAZIONE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("FRAZIONE_UBICAZIONE")) Then
    '            '        FRAZIONE_UBICAZIONE = drDatiContatore("FRAZIONE_UBICAZIONE")
    '            '    Else
    '            '        FRAZIONE_UBICAZIONE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("UTENTESOSPESO")) Then
    '            '        UTENTESOSPESO = drDatiContatore("UTENTESOSPESO")
    '            '    Else
    '            '        UTENTESOSPESO = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("QUOTEAGEVOLATE")) Then
    '            '        QUOTEAGEVOLATE = drDatiContatore("QUOTEAGEVOLATE")
    '            '    Else
    '            '        QUOTEAGEVOLATE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODICEFABBRICANTE")) Then
    '            '        CODICEFABBRICANTE = drDatiContatore("CODICEFABBRICANTE")
    '            '    Else
    '            '        CODICEFABBRICANTE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CIFRECONTATORE")) Then
    '            '        CIFRECONTATORE = drDatiContatore("CIFRECONTATORE")
    '            '    Else
    '            '        CIFRECONTATORE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODIVA")) Then
    '            '        CODIVA = drDatiContatore("CODIVA")
    '            '    Else
    '            '        CODIVA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("STATOCONTATORE")) Then
    '            '        STATOCONTATORE = drDatiContatore("STATOCONTATORE")
    '            '    Else
    '            '        STATOCONTATORE = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("PENALITA")) Then
    '            '        PENALITA = drDatiContatore("PENALITA")
    '            '    Else
    '            '        PENALITA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODICE_ISTAT")) Then
    '            '        CODICE_ISTAT = drDatiContatore("CODICE_ISTAT")
    '            '    Else
    '            '        CODICE_ISTAT = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("CODICE_PUNTO_PRESA")) Then
    '            '        CODICE_PUNTO_PRESA = drDatiContatore("CODICE_PUNTO_PRESA")
    '            '    Else
    '            '        CODICE_PUNTO_PRESA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("ESPONENTE_CIVICO")) Then
    '            '        ESPONENTE_CIVICO = drDatiContatore("ESPONENTE_CIVICO")
    '            '    Else
    '            '        ESPONENTE_CIVICO = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("IDTIPOATTIVITA")) Then
    '            '        IDTIPOATTIVITA = drDatiContatore("IDTIPOATTIVITA")
    '            '    Else
    '            '        IDTIPOATTIVITA = Nothing
    '            '    End If

    '            '    If Not IsDBNull(drDatiContatore("DIAMETROCONTATORE")) Then
    '            '        DIAMETROCONTATORE = drDatiContatore("DIAMETROCONTATORE")
    '            '    Else
    '            '        DIAMETROCONTATORE = Nothing
    '            '    End If

    '            '    '*** Fabi
    '            '    If Not IsDBNull(drDatiContatore("VIA_UBICAZIONE")) Then
    '            '        VIA_UBICAZIONE = drDatiContatore("VIA_UBICAZIONE")
    '            '    Else
    '            '        VIA_UBICAZIONE = Nothing
    '            '    End If
    '            '    '*** /Fabi

    '            'End While

    '            'drDatiContatore.Close()


    '            ''Dim CODENTE, CODENTEAPPARTENENZA, CODIMPIANTO, IDGIRO, IDTIPOCONTATORE, IDTIPOUTENZA, CODPOSIZIONE, CODFOGNATURA, CODDEPURAZIONE, _
    '            ''CONSUMOSTIMATO, CODDIAMETROCONTATORE, CODDIAMETROPRESA, CODLETTURISTA, CODPDA _
    '            '', COD_STRADA, CODIVA, IDTIPOATTIVITA As Int16


    '            ''Dim NUMEROUTENZE, SEQUENZA, POSIZIONEPROGRESSIVA, NOTE, LATOSTRADA _
    '            '', CODENTE1, CODENTEAPPARTENENZA1, CIVICO_UBICAZIONE, FRAZIONE_UBICAZIONE, QUOTEAGEVOLATE _
    '            '', CODICEFABBRICANTE, CIFRECONTATORE, STATOCONTATORE, PENALITA, CODICE_ISTAT _
    '            '', CODICE_PUNTO_PRESA, ESPONENTE_CIVICO, DIAMETROCONTATORE As String

    '            ''Dim ESENTEFOGNATURA, ESENTEDEPURAZIONE, SCARICATOSUPDA, LETTO, DARICONTROLLARE, _
    '            ''MODULOAUTOLETTURA, LASCIATOAVVISO, ANOMALIA, IGNORAMORA, UTENTESOSPESO As Boolean

    '            'sSQL=""
    '            'sSQL+="INSERT INTO TP_CONTATORI "
    '            'sSQL+="(PROPRIETARIO,CODCONTATORE,CODCONTRATTO,CODENTE,CODENTEAPPARTENENZA, CODIMPIANTO, IDGIRO, IDTIPOCONTATORE, "
    '            'sSQL+="IDTIPOUTENZA, CODPOSIZIONE, CODFOGNATURA, CODDEPURAZIONE, "
    '            'sSQL+="CODDIAMETROCONTATORE, CODDIAMETROPRESA, CODLETTURISTA, CODPDA,"
    '            'sSQL+="COD_STRADA, CODIVA, IDTIPOATTIVITA,"

    '            'sSQL+="CODENTE1,CODENTEAPPARTENENZA1,CIVICO_UBICAZIONE, FRAZIONE_UBICAZIONE, QUOTEAGEVOLATE,"
    '            'sSQL+="CODICEFABBRICANTE, CIFRECONTATORE, STATOCONTATORE, PENALITA, CODICE_ISTAT,"
    '            'sSQL+="CODICE_PUNTO_PRESA, ESPONENTE_CIVICO, DIAMETROCONTATORE,"
    '            'sSQL+="ESENTEACQUA,ESENTEFOGNATURA, ESENTEDEPURAZIONE, SCARICATOSUPDA, LETTO, DARICONTROLLARE,"
    '            'sSQL+="MODULOAUTOLETTURA, LASCIATOAVVISO, ANOMALIA, IGNORAMORA, UTENTESOSPESO, VIA_UBICAZIONE"
    '            'sSQL+=") VALUES("

    '            'sSQL+= 0 & ","

    '            'sSQL+= getNewID & ","

    '            'sSQL+= codcontrattoInContatori & ","

    '            'If CODENTE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= CODENTE & ","
    '            'End If

    '            'If CODENTEAPPARTENENZA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= CODENTEAPPARTENENZA & ","
    '            'End If

    '            'If CODIMPIANTO = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= CODIMPIANTO & ","
    '            'End If

    '            'If IDGIRO = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= IDGIRO & ","
    '            'End If

    '            'If IDTIPOCONTATORE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= IDTIPOCONTATORE & ","
    '            'End If

    '            'If IDTIPOUTENZA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= IDTIPOUTENZA & ","
    '            'End If

    '            'If CODPOSIZIONE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= CODPOSIZIONE & ","
    '            'End If

    '            'If CODFOGNATURA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= CODFOGNATURA & ","
    '            'End If

    '            'If CODDEPURAZIONE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= CODDEPURAZIONE & ","
    '            'End If

    '            'If CODDIAMETROCONTATORE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= CODDIAMETROCONTATORE & ","
    '            'End If

    '            'If CODDIAMETROPRESA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= CODDIAMETROPRESA & ","
    '            'End If

    '            'If CODLETTURISTA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= CODLETTURISTA & ","
    '            'End If

    '            'If CODPDA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= CODPDA & ","
    '            'End If

    '            'If COD_STRADA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= COD_STRADA & ","
    '            'End If

    '            'If CODIVA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= CODIVA & ","
    '            'End If

    '            'If IDTIPOATTIVITA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+= IDTIPOATTIVITA & ","
    '            'End If

    '            'If CODENTE1 = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & CODENTE1 & "',"
    '            'End If

    '            'If CODENTEAPPARTENENZA1 = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & CODENTEAPPARTENENZA1 & "',"
    '            'End If

    '            'If CIVICO_UBICAZIONE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & CIVICO_UBICAZIONE & "',"
    '            'End If

    '            'If FRAZIONE_UBICAZIONE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & FRAZIONE_UBICAZIONE & "',"
    '            'End If

    '            'If QUOTEAGEVOLATE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & QUOTEAGEVOLATE & "',"
    '            'End If

    '            'If CODICEFABBRICANTE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & CODICEFABBRICANTE & "',"
    '            'End If

    '            'If CIFRECONTATORE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & CIFRECONTATORE & "',"
    '            'End If

    '            'If STATOCONTATORE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & STATOCONTATORE & "',"
    '            'End If

    '            'If PENALITA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & PENALITA & "',"
    '            'End If

    '            'If CODICE_ISTAT = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & CODICE_ISTAT & "',"
    '            'End If

    '            'If CODICE_PUNTO_PRESA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & CODICE_PUNTO_PRESA & "',"
    '            'End If

    '            'If ESPONENTE_CIVICO = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & ESPONENTE_CIVICO & "',"
    '            'End If

    '            'If DIAMETROCONTATORE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    sSQL+="'" & DIAMETROCONTATORE & "',"
    '            'End If

    '            ''Dim ESENTEFOGNATURA, ESENTEDEPURAZIONE, SCARICATOSUPDA, LETTO, DARICONTROLLARE, _
    '            ''MODULOAUTOLETTURA, LASCIATOAVVISO, ANOMALIA, IGNORAMORA, UTENTESOSPESO As Boolean

    '            'If ESENTEACQUA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    If ESENTEACQUA = True Then
    '            '        ESENTEACQUA = 1
    '            '    Else
    '            '        ESENTEACQUA = 0
    '            '    End If
    '            '    sSQL+= ESENTEACQUA & ","
    '            'End If

    '            'If ESENTEFOGNATURA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    If ESENTEFOGNATURA = True Then
    '            '        ESENTEFOGNATURA = 1
    '            '    Else
    '            '        ESENTEFOGNATURA = 0
    '            '    End If
    '            '    sSQL+= ESENTEFOGNATURA & ","
    '            'End If

    '            'If ESENTEDEPURAZIONE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    If ESENTEDEPURAZIONE = True Then
    '            '        ESENTEDEPURAZIONE = 1
    '            '    Else
    '            '        ESENTEDEPURAZIONE = 0
    '            '    End If
    '            '    sSQL+= ESENTEDEPURAZIONE & ","
    '            'End If

    '            'If SCARICATOSUPDA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    If SCARICATOSUPDA = True Then
    '            '        SCARICATOSUPDA = 1
    '            '    Else
    '            '        SCARICATOSUPDA = 0
    '            '    End If
    '            '    sSQL+= SCARICATOSUPDA & ","
    '            'End If

    '            'If LETTO = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    If LETTO = True Then
    '            '        LETTO = 1
    '            '    Else
    '            '        LETTO = 0
    '            '    End If
    '            '    sSQL+= LETTO & ","
    '            'End If

    '            'If DARICONTROLLARE = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    If DARICONTROLLARE = True Then
    '            '        DARICONTROLLARE = 1
    '            '    Else
    '            '        DARICONTROLLARE = 0
    '            '    End If
    '            '    sSQL+= DARICONTROLLARE & ","
    '            'End If

    '            'If MODULOAUTOLETTURA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    If MODULOAUTOLETTURA = True Then
    '            '        MODULOAUTOLETTURA = 1
    '            '    Else
    '            '        MODULOAUTOLETTURA = 0
    '            '    End If
    '            '    sSQL+= MODULOAUTOLETTURA & ","
    '            'End If

    '            'If LASCIATOAVVISO = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    If LASCIATOAVVISO = True Then
    '            '        LASCIATOAVVISO = 1
    '            '    Else
    '            '        LASCIATOAVVISO = 0
    '            '    End If
    '            '    sSQL+= LASCIATOAVVISO & ","
    '            'End If

    '            'If ANOMALIA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    If ANOMALIA = True Then
    '            '        ANOMALIA = 1
    '            '    Else
    '            '        ANOMALIA = 0
    '            '    End If
    '            '    sSQL+= ANOMALIA & ","
    '            'End If

    '            'If IGNORAMORA = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    If IGNORAMORA = True Then
    '            '        IGNORAMORA = 1
    '            '    Else
    '            '        IGNORAMORA = 0
    '            '    End If
    '            '    sSQL+= IGNORAMORA & ","
    '            'End If

    '            'If UTENTESOSPESO = Nothing Then
    '            '    sSQL+="Null,"
    '            'Else
    '            '    If UTENTESOSPESO = True Then
    '            '        UTENTESOSPESO = 1
    '            '    Else
    '            '        UTENTESOSPESO = 0
    '            '    End If
    '            '    sSQL+= UTENTESOSPESO & ","
    '            'End If

    '            ''*** Fabi
    '            'If VIA_UBICAZIONE = Nothing Then
    '            '    sSQL+="Null"
    '            'Else
    '            '    sSQL+="'" & CStr(oReplace.ReplaceChar(VIA_UBICAZIONE)) & "')"
    '            'End If
    '            ''*** /Fabi

    '            '==========================
    '            'FINE VECCHIA QUERY
    '            '==========================


    '            sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '            sqlCmdInsert.ExecuteNonQuery()



    '            '=========================================================
    '            'INSERIMENTO DELLE ANAGRAFICHE
    '            '=========================================================

    '            '==========================================
    '            'INSERIMENTO ANAGRAFICA INTESTATARIO
    '            '==========================================

    '            sSQL=""
    '            sSQL+="INSERT INTO TR_CONTATORI_INTESTATARIO"
    '            sSQL+="(CODCONTATORE,COD_CONTRIBUENTE)"
    '            sSQL+="VALUES ("
    '            sSQL+= getNewID & ","
    '            sSQL+= -1 & ")"

    '            sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '            sqlCmdInsert.ExecuteNonQuery()



    '            '==========================================
    '            'INSERIMENTO ANAGRAFICA UTENTE
    '            '==========================================

    '            sSQL=""
    '            sSQL+="INSERT INTO TR_CONTATORI_UTENTE"
    '            sSQL+="(CODCONTATORE,COD_CONTRIBUENTE)"
    '            sSQL+="VALUES ("
    '            sSQL+= getNewID & ","
    '            sSQL+= -1 & ")"

    '            sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '            sqlCmdInsert.ExecuteNonQuery()


    '            '==============================
    '            'RIBALTAMENTO DATI CATASTALI
    '            'NEW
    '            '==============================

    '            sSQL=""
    '            sSQL="INSERT INTO TR_CONTATORI_CATASTALI ("
    '            sSQL+="CODCONTATORE,INTERNO,PIANO,FOGLIO,NUMERO,SUBALTERNO)"
    '            sSQL+=" SELECT " & getNewID & ",INTERNO,PIANO,FOGLIO,NUMERO,SUBALTERNO "
    '            sSQL+=" FROM TR_CONTATORI_CATASTALI "
    '            sSQL+=" WHERE CODCONTATORE=" & myContatore

    '            sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '            sqlCmdInsert.ExecuteNonQuery()

    '            '==============================
    '            'FINE
    '            'RIBALTAMENTO DATI CATASTALI
    '            'NEW
    '            '==============================


    '            '===============================
    '            'RIBALTAMENTO DATI CATASTALI
    '            'SOSPESO
    '            '===============================
    '            'Dim drDatiCatastali As new dataview
    '            'sSQL=""
    '            'sSQL="SELECT * FROM TR_CONTATORI_CATASTALI WHERE CODCONTATORE=" & codcontatorePerCatastali
    '            'drDatiCatastali = DBAccess.getdataview(sSQL)
    '            'Dim sqlCatasto As String

    '            'While drDatiCatastali.Read()
    '            '    sqlCatasto = ""
    '            '    sqlCatasto = sqlCatasto & "INSERT INTO TR_CONTATORI_CATASTALI ("
    '            '    sqlCatasto = sqlCatasto & "CODCONTATORE,INTERNO,PIANO,FOGLIO,NUMERO,SUBALTERNO)"
    '            '    sqlCatasto = sqlCatasto & "VALUES("
    '            '    sqlCatasto = sqlCatasto & getNewID & ","
    '            '    sqlCatasto = sqlCatasto & "'" & drDatiCatastali("INTERNO") & "',"
    '            '    sqlCatasto = sqlCatasto & "'" & drDatiCatastali("PIANO") & "',"
    '            '    sqlCatasto = sqlCatasto & "'" & drDatiCatastali("FOGLIO") & "',"
    '            '    sqlCatasto = sqlCatasto & "'" & drDatiCatastali("NUMERO") & "',"
    '            '    sqlCatasto = sqlCatasto & drDatiCatastali("SUBALTERNO") & ","
    '            '    sqlCatasto = sqlCatasto & ")"
    '            '    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '            '    sqlCmdInsert.ExecuteNonQuery()
    '            'End While
    '            'drDatiCatastali.Close()

    '            '===============================
    '            'FINE RIBALTAMENTO DATI CATASTALI
    '            '===============================


    '        End If

    '        sqlTrans.Commit()

    '    Catch er As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.SetContratto_20080804.errore: ", er)
    '        sqlTrans.Rollback()
    '        RaiseError.trace(er, sSql.Replace(vbCrLf, ""), ConfigurationManager.AppSettings("Filename"), ConfigurationManager.AppSettings("FilePath"), "")
    '        Throw
    '    Finally
    '        sqlConn.Close()
    '    End Try

    'End If

    '    Catch er As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.SetContratto_20080804.errore: ", er)
    '    End Try
    ' End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="CodiceContratto"></param>
    ''' <param name="IsVoltura"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function ControllaCodice(ByVal sIdEnte As String, ByVal CodiceContratto As String, IsVoltura As Integer) As String
        Dim IDestratto As String = "-1"
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        If IsVoltura <= 0 Then
            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetElencoContratti", "IDENTE", "CODICECONTRATTO", "IDVIA", "COGNOMEINT", "NOMEINT", "COGNOMEUTE", "NOMEUTE", "STATO")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                        , ctx.GetParam("CODICECONTRATTO", CodiceContratto) _
                        , ctx.GetParam("IDVIA", -1) _
                        , ctx.GetParam("COGNOMEINT", "") _
                        , ctx.GetParam("NOMEINT", "") _
                        , ctx.GetParam("COGNOMEUTE", "") _
                        , ctx.GetParam("NOMEUTE", "") _
                        , ctx.GetParam("STATO", -1)
                    )
                    ctx.Dispose()
                End Using
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        IDestratto = Utility.StringOperation.FormatString(myRow("CODCONTRATTO"))
                    Next
                End If
            Catch ex As Exception
                Log.Debug(sIdEnte + " - OPENgovH2O.GestContratti.ControllaCodice.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try
        End If
        Return IDestratto
    End Function
    'Public Function ControllaCodice(ByVal sIdEnte As String, ByVal CodiceContratto As String) As String
    '    Dim IDestratto As String = "-1"
    '    Dim sSQL As String
    '    Dim drCodiceEstratto As new dataview
    '    Try
    '        sSQL = "SELECT TP_CONTRATTI.CODCONTRATTO"
    '        sSQL += " FROM TP_CONTRATTI WITH (NOLOCK)"
    '        sSQL += " WHERE (TP_CONTRATTI.CODENTE='" & sIdEnte & "') AND (TP_CONTRATTI.CODICECONTRATTO='" & CodiceContratto & "')"
    '        drCodiceEstratto = iDB.getdataview(sSQL)
    '        If drCodiceEstratto.Read = True Then
    '            If Not IsDBNull(drCodiceEstratto("CODCONTRATTO")) Then
    '                IDestratto = drCodiceEstratto("CODCONTRATTO")
    '            End If
    '        End If
    '        ControllaCodice = IDestratto

    '    Catch er As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.ControllaCodice.errore: ", er)
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nNumUtente"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetNumeroUtente(ByVal sIdEnte As String, ByVal nNumUtente As Integer) As Integer
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetNumeroUtente", "IDENTE")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte))
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            nNumUtente = StringOperation.FormatInt(myRow("numutente"))
                        Next
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContratti.GetNumeroUtente.errore: ", ex)
                Finally
                    dvMyDati.Dispose()
                End Try
                ctx.Dispose()
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestContratti.GetNumeroUtente.errore: ", Err)
            nNumUtente = -1
        End Try
        Return nNumUtente
    End Function
    'Public Function GetNumeroUtente(ByVal sIdEnte As String, ByVal nNumUtente As Integer) As Integer
    '    Dim sSQL As String
    '    Dim DrNUtente As new dataview
    '    Dim bExist As Boolean = False

    '    Try
    '        Do
    '            If nNumUtente = 0 Then
    '                sSQL = "SELECT MAX(CAST(TP_CONTATORI.NUMEROUTENTE AS NUMERIC)) AS NUMUTENTE"
    '                sSQL += " FROM TP_CONTATORI"
    '                sSQL += " WHERE (TP_CONTATORI.CODENTE=" & sIdEnte & ")"
    '                DrNUtente = iDB.getdataview(sSQL)
    '                Do While DrNUtente.Read
    '                    If Not IsDBNull(DrNUtente("numutente")) Then
    '                        nNumUtente = CInt(DrNUtente("numutente")) + 1
    '                    End If
    '                Loop
    '                DrNUtente.Close()
    '            Else
    '                nNumUtente += 1
    '                sSQL = "SELECT TP_CONTATORI.NUMEROUTENTE"
    '                sSQL += " FROM TP_CONTATORI"
    '                sSQL += " WHERE (TP_CONTATORI.CODENTE=" & sIdEnte & ") AND (TP_CONTATORI.NUMEROUTENTE = " & nNumUtente & ")"
    '                DrNUtente = iDB.getdataview(sSQL)
    '                If DrNUtente.HasRows Then
    '                    bExist = True
    '                Else
    '                    bExist = False
    '                End If
    '                DrNUtente.Close()
    '            End If
    '        Loop Until bExist = False

    '        Return nNumUtente
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetNumeroUtente.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function

    Public Function GetEsistente(ByVal IdEnte As String, ByVal NUtente As Integer, ByVal CodContratto As Integer) As Boolean
        Dim sSQL As String
        Dim dvMyDati As New DataView

        sSQL = "SELECT *"
        sSQL += " FROM TP_CONTRATTI"
        sSQL += " WHERE CODENTE='" & IdEnte & "' AND (NUMEROUTENTE = " & NUtente & " AND CODCONTRATTO <> " & CodContratto & ")"
        dvMyDati = iDB.GetDataView(sSQL)
        If Not dvMyDati Is Nothing Then
            For Each myRow As DataRowView In dvMyDati
                Return True
            Next
        Else
            Return False
        End If
        dvMyDati.Dispose()
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oMyContratto"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function SetContratto(ByVal oMyContratto As objContratto) As Boolean
        Dim sSQL As String = ""
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_CONTRATTI_IU", "CODCONTRATTO", "CODENTE", "DATASOTTOSCRIZIONE", "DATAINVIODOCUMENTI", "TERMINATO", "CONSUMOMINIMO", "CODDIAMETROCONTATORE", "CODDIAMETROPRESA", "IDTIPOUTENZA" _
                        , "NUMEROUTENZE", "CODICECONTRATTO", "IDCONTATORE", "LINK_DOCUMENTO", "LINK_PREVENTIVO", "CODICE_ISTAT", "CODIMPIANTO", "CODENTEAPPARTENENZA1", "PIANO", "FOGLIO", "NUMERO", "SUBALTERNO", "SPESA", "DIRITTI", "PENDENTE" _
                        , "IDGIRO", "SEQUENZA", "IDTIPOCONTATORE", "CODPOSIZIONE", "POSIZIONEPROGRESSIVA", "NOTE", "DATAATTIVAZIONE", "CODFOGNATURA", "CODDEPURAZIONE", "ESENTEFOGNATURA", "ESENTEDEPURAZIONE", "IDMINIMO", "LATOSTRADA", "IGNORAMORA" _
                        , "CODENTE1", "COD_STRADA", "CIVICO_UBICAZIONE", "DATASOSPENSIONEUTENZA", "UTENTESOSPESO", "CODICEFABBRICANTE", "CIFRECONTATORE", "CODIVA", "STATOCONTATORE", "PENALITA", "ESPONENTE_CIVICO", "IDTIPOATTIVITA", "DATACESSAZIONE", "PROPRIETARIO" _
                        , "RICHIESTASUB", "NOTERICHIESTASUB", "NUMEROUTENTE", "COLONNACOMODO", "ESENTEACQUA")
                ctx.ExecuteNonQuery(sSQL, ctx.GetParam("CODCONTRATTO", oMyContratto.nIdContratto) _
                        , ctx.GetParam("CODENTE", oMyContratto.sIdEnte) _
                        , ctx.GetParam("DATASOTTOSCRIZIONE", oReplace.GiraData(StringOperation.FormatDateTime(oMyContratto.sDataSottoscrizione))) _
                        , ctx.GetParam("DATAINVIODOCUMENTI", "") _
                        , ctx.GetParam("TERMINATO", 0) _
                        , ctx.GetParam("CONSUMOMINIMO", 0) _
                        , ctx.GetParam("CODDIAMETROCONTATORE", oMyContratto.oContatore.nDiametroContatore) _
                        , ctx.GetParam("CODDIAMETROPRESA", oMyContratto.oContatore.nDiametroPresa) _
                        , ctx.GetParam("IDTIPOUTENZA", 0) _
                        , ctx.GetParam("NUMEROUTENZE", 0) _
                        , ctx.GetParam("CODICECONTRATTO", oMyContratto.sCodiceContratto) _
                        , ctx.GetParam("IDCONTATORE", 0) _
                        , ctx.GetParam("LINK_DOCUMENTO", "") _
                        , ctx.GetParam("LINK_PREVENTIVO", "") _
                        , ctx.GetParam("CODICE_ISTAT", oMyContratto.oContatore.sCodiceISTAT) _
                        , ctx.GetParam("CODIMPIANTO", oMyContratto.oContatore.nIdImpianto) _
                        , ctx.GetParam("CODENTEAPPARTENENZA1", oMyContratto.oContatore.sIdEnteAppartenenza) _
                        , ctx.GetParam("PIANO", "") _
                        , ctx.GetParam("FOGLIO", "") _
                        , ctx.GetParam("NUMERO", "") _
                        , ctx.GetParam("SUBALTERNO", 0) _
                        , ctx.GetParam("SPESA", oMyContratto.oContatore.nSpesa) _
                        , ctx.GetParam("DIRITTI", oMyContratto.oContatore.nDiritti) _
                        , ctx.GetParam("PENDENTE", oMyContratto.oContatore.bIsPendente) _
                        , ctx.GetParam("IDGIRO", oMyContratto.oContatore.nGiro) _
                        , ctx.GetParam("SEQUENZA", oMyContratto.oContatore.sSequenza) _
                        , ctx.GetParam("IDTIPOCONTATORE", oMyContratto.oContatore.nTipoContatore) _
                        , ctx.GetParam("CODPOSIZIONE", oMyContratto.oContatore.nPosizione) _
                        , ctx.GetParam("POSIZIONEPROGRESSIVA", oMyContratto.oContatore.sProgressivo) _
                        , ctx.GetParam("NOTE", oReplace.ReplaceChar(oMyContratto.sNote)) _
                        , ctx.GetParam("DATAATTIVAZIONE", oReplace.GiraData(StringOperation.FormatDateTime(oMyContratto.oContatore.sDataAttivazione))) _
                        , ctx.GetParam("CODFOGNATURA", 0) _
                        , ctx.GetParam("CODDEPURAZIONE", 0) _
                        , ctx.GetParam("ESENTEFOGNATURA", 0) _
                        , ctx.GetParam("ESENTEDEPURAZIONE", 0) _
                        , ctx.GetParam("IDMINIMO", oMyContratto.oContatore.nIdMinimo) _
                        , ctx.GetParam("LATOSTRADA", oMyContratto.oContatore.sLatoStrada) _
                        , ctx.GetParam("IGNORAMORA", oMyContratto.oContatore.bIgnoraMora) _
                        , ctx.GetParam("CODENTE1", oMyContratto.sIdEnte) _
                        , ctx.GetParam("COD_STRADA", 0) _
                        , ctx.GetParam("CIVICO_UBICAZIONE", "") _
                        , ctx.GetParam("DATASOSPENSIONEUTENZA", oReplace.GiraData(StringOperation.FormatDateTime(oMyContratto.oContatore.sDataSospensioneUtenza))) _
                        , ctx.GetParam("UTENTESOSPESO", oMyContratto.oContatore.bUtenteSospeso) _
                        , ctx.GetParam("CODICEFABBRICANTE", oMyContratto.oContatore.sCodiceFabbricante) _
                        , ctx.GetParam("CIFRECONTATORE", oMyContratto.oContatore.sCifreContatore) _
                        , ctx.GetParam("CODIVA", oMyContratto.oContatore.nCodIva) _
                        , ctx.GetParam("STATOCONTATORE", oMyContratto.oContatore.sStatoContatore) _
                        , ctx.GetParam("PENALITA", oMyContratto.oContatore.sPenalita) _
                        , ctx.GetParam("ESPONENTE_CIVICO", "") _
                        , ctx.GetParam("IDTIPOATTIVITA", oMyContratto.oContatore.nIdAttivita) _
                        , ctx.GetParam("DATACESSAZIONE", "") _
                        , ctx.GetParam("PROPRIETARIO", oMyContratto.oContatore.nProprietario) _
                        , ctx.GetParam("RICHIESTASUB", oMyContratto.bIsRichiestaSub) _
                        , ctx.GetParam("NOTERICHIESTASUB", oReplace.ReplaceChar(oMyContratto.sNoteRichiestaSub)) _
                        , ctx.GetParam("NUMEROUTENTE", 0) _
                        , ctx.GetParam("COLONNACOMODO", "") _
                        , ctx.GetParam("ESENTEACQUA", 0)
                    )

                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TR_INTESTATARIOUTENTE_IU", "NAMETBL", "IDRIF", "IDCONTRIBUENTE")
                ctx.ExecuteNonQuery(sSQL, ctx.GetParam("NAMETBL", "TR_CONTRATTI_INTESTATARIO") _
                        , ctx.GetParam("IDRIF", oMyContratto.nIdContratto) _
                        , ctx.GetParam("IDCONTRIBUENTE", oMyContratto.nIdIntestatario)
                    )

                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TR_INTESTATARIOUTENTE_IU", "NAMETBL", "IDRIF", "IDCONTRIBUENTE")
                ctx.ExecuteNonQuery(sSQL, ctx.GetParam("NAMETBL", "TR_CONTRATTI_UTENTE") _
                        , ctx.GetParam("IDRIF", oMyContratto.nIdContratto) _
                        , ctx.GetParam("IDCONTRIBUENTE", oMyContratto.nIdUtente)
                    )

                ctx.Dispose()
            End Using
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.SetContratto.errore: ", ex)
            Return False
        End Try
    End Function
    'Public Function SetContratto(ByVal nTypeOperation As Integer, ByVal oMyContratto As objContratto, ByVal oMyConnection As SqlConnection, ByVal oMyTransaction As SqlTransaction) As Boolean
    '    Dim cmdMyCommand As New SqlCommand()
    '    Try
    '        cmdMyCommand.Connection = oMyConnection
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText = "prc_TP_CONTRATTI_IU"
    '        cmdMyCommand.Parameters.AddWithValue("@CODCONTRATTO", oMyContratto.nIdContratto)
    '        cmdMyCommand.Parameters.AddWithValue("@CODENTE", oMyContratto.sIdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@DATASOTTOSCRIZIONE", oReplace.GiraData(oMyContratto.sDataSottoscrizione))
    '        cmdMyCommand.Parameters.AddWithValue("@DATAINVIODOCUMENTI", "")
    '        cmdMyCommand.Parameters.AddWithValue("@TERMINATO", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@CONSUMOMINIMO", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@CODDIAMETROCONTATORE", oMyContratto.oContatore.nDiametroContatore)
    '        cmdMyCommand.Parameters.AddWithValue("@CODDIAMETROPRESA", oMyContratto.oContatore.nDiametroPresa)
    '        cmdMyCommand.Parameters.AddWithValue("@IDTIPOUTENZA", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@NUMEROUTENZE", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@CODICECONTRATTO", oMyContratto.sCodiceContratto)
    '        cmdMyCommand.Parameters.AddWithValue("@IDCONTATORE", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@LINK_DOCUMENTO", "")
    '        cmdMyCommand.Parameters.AddWithValue("@LINK_PREVENTIVO", "")
    '        cmdMyCommand.Parameters.AddWithValue("@CODICE_ISTAT", oMyContratto.oContatore.sCodiceISTAT)
    '        cmdMyCommand.Parameters.AddWithValue("@CODIMPIANTO", oMyContratto.oContatore.nIdImpianto)
    '        cmdMyCommand.Parameters.AddWithValue("@CODENTEAPPARTENENZA1", oMyContratto.oContatore.sIdEnteAppartenenza)
    '        cmdMyCommand.Parameters.AddWithValue("@PIANO", "")
    '        cmdMyCommand.Parameters.AddWithValue("@FOGLIO", "")
    '        cmdMyCommand.Parameters.AddWithValue("@NUMERO", "")
    '        cmdMyCommand.Parameters.AddWithValue("@SUBALTERNO", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@SPESA", oMyContratto.oContatore.nSpesa)
    '        cmdMyCommand.Parameters.AddWithValue("@DIRITTI", oMyContratto.oContatore.nDiritti)
    '        cmdMyCommand.Parameters.AddWithValue("@PENDENTE", oMyContratto.oContatore.bIsPendente)
    '        cmdMyCommand.Parameters.AddWithValue("@IDGIRO", oMyContratto.oContatore.nGiro)
    '        cmdMyCommand.Parameters.AddWithValue("@SEQUENZA", oMyContratto.oContatore.sSequenza)
    '        cmdMyCommand.Parameters.AddWithValue("@IDTIPOCONTATORE", oMyContratto.oContatore.nTipoContatore)
    '        cmdMyCommand.Parameters.AddWithValue("@CODPOSIZIONE", oMyContratto.oContatore.nPosizione)
    '        cmdMyCommand.Parameters.AddWithValue("@POSIZIONEPROGRESSIVA", oMyContratto.oContatore.sProgressivo)
    '        cmdMyCommand.Parameters.AddWithValue("@NOTE", oReplace.ReplaceChar(oMyContratto.sNote))
    '        cmdMyCommand.Parameters.AddWithValue("@DATAATTIVAZIONE", oReplace.GiraData(oMyContratto.oContatore.sDataAttivazione))
    '        cmdMyCommand.Parameters.AddWithValue("@CODFOGNATURA", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@CODDEPURAZIONE", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@ESENTEFOGNATURA", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@ESENTEDEPURAZIONE", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@IDMINIMO", oMyContratto.oContatore.nIdMinimo)
    '        cmdMyCommand.Parameters.AddWithValue("@LATOSTRADA", oMyContratto.oContatore.sLatoStrada)
    '        cmdMyCommand.Parameters.AddWithValue("@IGNORAMORA", oMyContratto.oContatore.bIgnoraMora)
    '        cmdMyCommand.Parameters.AddWithValue("@CODENTE1", oMyContratto.sIdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@COD_STRADA", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@CIVICO_UBICAZIONE", "")
    '        cmdMyCommand.Parameters.AddWithValue("@DATASOSPENSIONEUTENZA", oReplace.GiraData(oMyContratto.oContatore.sDataSospensioneUtenza))
    '        cmdMyCommand.Parameters.AddWithValue("@UTENTESOSPESO", oMyContratto.oContatore.bUtenteSospeso)
    '        cmdMyCommand.Parameters.AddWithValue("@CODICEFABBRICANTE", oMyContratto.oContatore.sCodiceFabbricante)
    '        cmdMyCommand.Parameters.AddWithValue("@CIFRECONTATORE", oMyContratto.oContatore.sCifreContatore)
    '        cmdMyCommand.Parameters.AddWithValue("@CODIVA", oMyContratto.oContatore.nCodIva)
    '        cmdMyCommand.Parameters.AddWithValue("@STATOCONTATORE", oMyContratto.oContatore.sStatoContatore)
    '        cmdMyCommand.Parameters.AddWithValue("@PENALITA", oMyContratto.oContatore.sPenalita)
    '        cmdMyCommand.Parameters.AddWithValue("@ESPONENTE_CIVICO", "")
    '        cmdMyCommand.Parameters.AddWithValue("@IDTIPOATTIVITA", oMyContratto.oContatore.nIdAttivita)
    '        cmdMyCommand.Parameters.AddWithValue("@DATACESSAZIONE", "")
    '        cmdMyCommand.Parameters.AddWithValue("@PROPRIETARIO", oMyContratto.oContatore.nProprietario)
    '        cmdMyCommand.Parameters.AddWithValue("@RICHIESTASUB", oMyContratto.bIsRichiestaSub)
    '        cmdMyCommand.Parameters.AddWithValue("@NOTERICHIESTASUB", oReplace.ReplaceChar(oMyContratto.sNoteRichiestaSub))
    '        cmdMyCommand.Parameters.AddWithValue("@NUMEROUTENTE", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@COLONNACOMODO", "")
    '        cmdMyCommand.Parameters.AddWithValue("@ESENTEACQUA", 0)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()
    '        '*** ***

    '        '************************************************************************************
    '        'INSERIMENTO ANAGRAFICHE NELLA TABELLA TR_CONTRATTI_INTESTATARIO
    '        '************************************************************************************
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText = "prc_TR_INTESTATARIOUTENTE_IU"
    '        cmdMyCommand.Parameters.AddWithValue("@NAMETBL", "TR_CONTRATTI_INTESTATARIO")
    '        cmdMyCommand.Parameters.AddWithValue("@IDRIF", oMyContratto.nIdContratto)
    '        cmdMyCommand.Parameters.AddWithValue("@IDCONTRIBUENTE", oMyContratto.nIdIntestatario)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()

    '        '************************************************************************************
    '        'INSERIMENTO ANAGRAFICHE NELLA TABELLA TR_CONTRATTI_UTENTE
    '        '************************************************************************************
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText = "prc_TR_INTESTATARIOUTENTE_IU"
    '        cmdMyCommand.Parameters.AddWithValue("@NAMETBL", "TR_CONTRATTI_UTENTE")
    '        cmdMyCommand.Parameters.AddWithValue("@IDRIF", oMyContratto.nIdContratto)
    '        cmdMyCommand.Parameters.AddWithValue("@IDCONTRIBUENTE", oMyContratto.nIdUtente)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()

    '        Return True
    '    Catch ex As Exception
    '        oMyTransaction.Rollback()
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.SetContratto.errore: ", ex)
    '        Return False
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    'Private Function GetSQLContratti(ByVal nDBOperation As DBOperation, ByVal oMyContratto As objContratto, ByVal nIdContratto As Long) As String
    '    Dim sSQL As String

    '    Try
    '        Select Case nDBOperation
    '            Case DBOperation.DB_INSERT
    '                sSQL = "INSERT INTO TP_CONTRATTI"
    '                'sSQL += "(PIANO,CODCONTRATTO,CODICECONTRATTO,DATASOTTOSCRIZIONE,FOGLIO,NUMERO,SUBALTERNO,"
    '                sSQL += "(CODCONTRATTO,CODICECONTRATTO,DATASOTTOSCRIZIONE,"
    '                sSQL += " SPESA,DIRITTI,PENDENTE,CODENTE,CODIMPIANTO,IDGIRO,SEQUENZA,IDTIPOCONTATORE,CODPOSIZIONE,"
    '                sSQL += " POSIZIONEPROGRESSIVA,NOTE,DATAATTIVAZIONE,CODDIAMETROCONTATORE,CODDIAMETROPRESA,"
    '                sSQL += " IDMINIMO,LATOSTRADA,IGNORAMORA,CODENTE1,CODENTEAPPARTENENZA1,DATASOSPENSIONEUTENZA,"
    '                sSQL += " UTENTESOSPESO,CODICEFABBRICANTE,CIFRECONTATORE,CODIVA,STATOCONTATORE,PENALITA,CODICE_ISTAT,"
    '                sSQL += " PROPRIETARIO,RICHIESTASUB,NOTERICHIESTASUB,IDTIPOATTIVITA)"
    '                sSQL += " VALUES ( "
    '                'If oMyContratto.oContatore.oDatiCatastali.sPiano <> "" Then
    '                '    sSQL += "'0',"
    '                'Else
    '                '    sSQL += "Null, "
    '                'End If
    '                sSQL += nIdContratto & ", '" & CStr(oMyContratto.sCodiceContratto) & "', '" & CStr(oReplace.GiraData(oMyContratto.sDataSottoscrizione)) & "', "
    '                'If oMyContratto.oContatore.oDatiCatastali.sFoglio <> "" Then
    '                '    sSQL += "'" & CStr(oMyContratto.oContatore.oDatiCatastali.sFoglio) & "', "
    '                'Else
    '                '    sSQL += "Null, "
    '                'End If
    '                'If oMyContratto.oContatore.oDatiCatastali.sNumero <> "" Then
    '                '    sSQL += "'" & CStr(oMyContratto.oContatore.oDatiCatastali.sNumero) & "', "
    '                'Else
    '                '    sSQL += "Null, "
    '                'End If
    '                'If oMyContratto.oContatore.oDatiCatastali.nSubalterno <> 0 Then
    '                '    sSQL += CStr(oMyContratto.oContatore.oDatiCatastali.nSubalterno) & ", "
    '                'Else
    '                '    sSQL += "Null, "
    '                'End If
    '                If oMyContratto.oContatore.nSpesa <> 0 Then
    '                    sSQL += oMyContratto.oContatore.nSpesa.ToString().Replace(",", ".") & ", "
    '                Else
    '                    sSQL += "Null, "
    '                End If
    '                If oMyContratto.oContatore.nDiritti <> 0 Then
    '                    sSQL += oMyContratto.oContatore.nDiritti.ToString().Replace(",", ".") & ", "
    '                Else
    '                    sSQL += "Null, "
    '                End If
    '                sSQL += CInt(oMyContratto.oContatore.bIsPendente) & ","
    '                sSQL += CStr(oMyContratto.sIdEnte) & ","
    '                If oMyContratto.oContatore.nIdImpianto <> -1 Then
    '                    sSQL += CStr(oMyContratto.oContatore.nIdImpianto) & ", "
    '                Else
    '                    sSQL += " Null, "
    '                End If
    '                If oMyContratto.oContatore.nGiro <> -1 Then
    '                    sSQL += CStr(oMyContratto.oContatore.nGiro) & ","
    '                Else
    '                    sSQL += " Null,"
    '                End If
    '                sSQL += "'" & CStr(oMyContratto.oContatore.sSequenza) & "',"
    '                If oMyContratto.oContatore.nTipoContatore <> -1 Then
    '                    sSQL += CStr(oMyContratto.oContatore.nTipoContatore) & ","
    '                Else
    '                    sSQL += " Null,"
    '                End If
    '                sSQL += CStr(oMyContratto.oContatore.nPosizione) & ","
    '                sSQL += "'" & CStr(oMyContratto.oContatore.sProgressivo) & "',"
    '                sSQL += "'" & CStr(oReplace.ReplaceChar(oMyContratto.sNote)) & "',"
    '                sSQL += "'" & CStr(oReplace.GiraData(oMyContratto.oContatore.sDataAttivazione)) & "',"
    '                If oMyContratto.oContatore.nDiametroContatore <> -1 Then
    '                    sSQL += CStr(oMyContratto.oContatore.nDiametroContatore) & ","
    '                Else
    '                    sSQL += "Null,"
    '                End If
    '                If oMyContratto.oContatore.nDiametroPresa <> -1 Then
    '                    sSQL += CStr(oMyContratto.oContatore.nDiametroPresa) & ","
    '                Else
    '                    sSQL += "Null,"
    '                End If
    '                If oMyContratto.oContatore.nIdMinimo <> -1 Then
    '                    sSQL += CStr(oMyContratto.oContatore.nIdMinimo) & ","
    '                Else
    '                    sSQL += "Null,"
    '                End If
    '                sSQL += "'" & CStr(oMyContratto.oContatore.sLatoStrada) & "',"
    '                sSQL += CInt(oMyContratto.oContatore.bIgnoraMora) & ","
    '                sSQL += "'" & CStr(oMyContratto.sIdEnte) & "',"
    '                sSQL += "'" & oMyContratto.oContatore.sIdEnteAppartenenza & "',"
    '                sSQL += "'" & CStr(oReplace.GiraData(oMyContratto.oContatore.sDataSospensioneUtenza)) & "',"
    '                sSQL += CInt(oMyContratto.oContatore.bUtenteSospeso) & ","
    '                sSQL += "'" & CStr(oMyContratto.oContatore.sCodiceFabbricante) & "',"
    '                sSQL += "'" & CStr(oMyContratto.oContatore.sCifreContatore) & "',"
    '                If oMyContratto.oContatore.nCodIva <> -1 Then
    '                    sSQL += CStr(oMyContratto.oContatore.nCodIva) & ","
    '                Else
    '                    sSQL += "Null,"
    '                End If
    '                sSQL += "'" & CStr(oMyContratto.oContatore.sStatoContatore) & "',"
    '                sSQL += "'" & CStr(oMyContratto.oContatore.sPenalita) & "',"
    '                sSQL += "'" & CStr(oMyContratto.oContatore.sCodiceISTAT) & "',"
    '                sSQL += CStr(oMyContratto.oContatore.nProprietario) & "," & CInt(oMyContratto.bIsRichiestaSub) & ","
    '                sSQL += "'" & CStr(oReplace.ReplaceChar(oMyContratto.sNoteRichiestaSub)) & "',"
    '                If oMyContratto.oContatore.nIdAttivita <> -1 Then
    '                    sSQL += CStr(oMyContratto.oContatore.nIdAttivita)
    '                Else
    '                    sSQL += "Null"
    '                End If
    '                sSQL += " )"

    '            Case DBOperation.DB_UPDATE
    '                sSQL = "UPDATE TP_CONTRATTI SET "
    '                sSQL += " NOTERICHIESTASUB='" & oReplace.ReplaceChar(oMyContratto.sNoteRichiestaSub) & "',"
    '                If oMyContratto.bIsRichiestaSub = False Then
    '                    sSQL += " RICHIESTASUB=0,"
    '                Else
    '                    sSQL += "RICHIESTASUB=1,"
    '                End If
    '                sSQL += " PROPRIETARIO=" & CInt(oMyContratto.oContatore.nProprietario) & ","
    '                'sSQL += " PIANO ='0',"
    '                'sSQL += " FOGLIO ='" & CStr(oMyContratto.oContatore.oDatiCatastali.sFoglio) & "', "
    '                'sSQL += " NUMERO ='" & CStr(oMyContratto.oContatore.oDatiCatastali.sNumero) & "', "
    '                'sSQL += " SUBALTERNO=" & CInt(oMyContratto.oContatore.oDatiCatastali.nSubalterno) & ", "
    '                sSQL += " SPESA =" & oMyContratto.oContatore.nSpesa.ToString().Replace(",", ".") & ", "
    '                sSQL += " DIRITTI =" & oMyContratto.oContatore.nDiritti.ToString().Replace(",", ".") & ", "
    '                sSQL += " PENDENTE =" & CInt(oMyContratto.oContatore.bIsPendente) & ", "
    '                sSQL += " CODICECONTRATTO ='" & CStr(oMyContratto.sCodiceContratto) & "', "
    '                sSQL += " DATASOTTOSCRIZIONE='" & CStr(oReplace.GiraData(oMyContratto.sDataSottoscrizione)) & "', "
    '                sSQL += " CODENTE = " & CInt(oMyContratto.sIdEnte) & ", "
    '                If oMyContratto.oContatore.nIdImpianto <> -1 Then
    '                    sSQL += " CODIMPIANTO = " & CInt(oMyContratto.oContatore.nIdImpianto) & ", "
    '                Else
    '                    sSQL += " CODIMPIANTO = Null, "
    '                End If
    '                If oMyContratto.oContatore.nGiro <> -1 Then
    '                    sSQL += " IDGIRO = " & CInt(oMyContratto.oContatore.nGiro) & ", "
    '                Else
    '                    sSQL += " IDGIRO = Null, "
    '                End If
    '                sSQL += " SEQUENZA = '" & CStr(oMyContratto.oContatore.sSequenza) & "', "

    '                If oMyContratto.oContatore.nTipoContatore <> -1 Then
    '                    sSQL += " IDTIPOCONTATORE = " & CInt(oMyContratto.oContatore.nTipoContatore) & ", "
    '                Else
    '                    sSQL += " IDTIPOCONTATORE = Null, "
    '                End If
    '                sSQL += " CODPOSIZIONE = " & CInt(oMyContratto.oContatore.nPosizione) & ", "
    '                sSQL += " POSIZIONEPROGRESSIVA = '" & CStr(oMyContratto.oContatore.sProgressivo) & "', "
    '                sSQL += " NOTE = '" & CStr(oReplace.ReplaceChar(oMyContratto.sNote)) & "', "
    '                sSQL += " DATAATTIVAZIONE = '" & CStr(oReplace.GiraData(oMyContratto.oContatore.sDataAttivazione)) & "', "
    '                sSQL += " DATACESSAZIONE = '" & CStr(oReplace.GiraData(oMyContratto.oContatore.sDataCessazione)) & "', "
    '                If oMyContratto.oContatore.nDiametroContatore <> -1 Then
    '                    sSQL += " CODDIAMETROCONTATORE = " & CInt(oMyContratto.oContatore.nDiametroContatore) & ", "
    '                Else
    '                    sSQL += " CODDIAMETROCONTATORE = Null, "
    '                End If
    '                If oMyContratto.oContatore.nDiametroPresa <> -1 Then
    '                    sSQL += " CODDIAMETROPRESA = " & CInt(oMyContratto.oContatore.nDiametroPresa) & ", "
    '                Else
    '                    sSQL += " CODDIAMETROPRESA = Null, "
    '                End If
    '                If oMyContratto.oContatore.nIdMinimo <> -1 Then
    '                    sSQL += " IDMINIMO = " & CInt(oMyContratto.oContatore.nIdMinimo) & ", "
    '                Else
    '                    sSQL += " IDMINIMO = Null, "
    '                End If
    '                sSQL += " LATOSTRADA = '" & CStr(oMyContratto.oContatore.sLatoStrada) & "', "
    '                sSQL += " IGNORAMORA = " & CInt(oMyContratto.oContatore.bIgnoraMora) & ", "
    '                sSQL += " CODENTE1 = '" & CStr(oMyContratto.sIdEnte) & "', "
    '                sSQL += " CODENTEAPPARTENENZA1 = '" & CStr(oMyContratto.oContatore.sIdEnteAppartenenza) & "', "
    '                sSQL += " DATASOSPENSIONEUTENZA = '" & CStr(oReplace.GiraData(oMyContratto.oContatore.sDataSospensioneUtenza)) & "', "
    '                sSQL += " UTENTESOSPESO = " & CInt(oMyContratto.oContatore.bUtenteSospeso) & ", "
    '                sSQL += " CODICEFABBRICANTE = '" & CStr(oMyContratto.oContatore.sCodiceFabbricante) & "', "
    '                sSQL += " CIFRECONTATORE = '" & CStr(oMyContratto.oContatore.sCifreContatore) & "', "
    '                If oMyContratto.oContatore.nCodIva <> -1 Then
    '                    sSQL += " CODIVA = " & CInt(oMyContratto.oContatore.nCodIva) & ", "
    '                Else
    '                    sSQL += " CODIVA = Null, "
    '                End If
    '                sSQL += " STATOCONTATORE = '" & CStr(oMyContratto.oContatore.sStatoContatore) & "', "
    '                sSQL += " PENALITA = '" & CStr(oMyContratto.oContatore.sPenalita) & "', "
    '                sSQL += " CODICE_ISTAT = '" & CStr(oMyContratto.oContatore.sCodiceISTAT) & "', "
    '                If oMyContratto.oContatore.nIdAttivita <> -1 Then
    '                    sSQL += " IDTIPOATTIVITA = " & CInt(oMyContratto.oContatore.nIdAttivita) & " "
    '                Else
    '                    sSQL += " IDTIPOATTIVITA = Null "
    '                End If
    '                sSQL += " WHERE CODCONTRATTO =" & nIdContratto
    '        End Select

    '        Return sSQL
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetSQLContratti.errore: ", Err)
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
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestContratti.GetSQLIntestatarioUtente.errore: ", Err)
    '        Return ""
    '    End Try
    'End Function
End Class

