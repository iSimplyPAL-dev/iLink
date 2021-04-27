Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports Utility
''' <summary>
''' Classe per la gestione della tessera
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestTessera
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestTessera))
    Private oReplace As New generalClass.generalFunction
    Private cmdMyCommand As New SqlClient.SqlCommand
    Private DrReturn As SqlClient.SqlDataReader
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nIdContribuente"></param>
    ''' <param name="nIdTestata"></param>
    ''' <param name="sCodiceUtente"></param>
    ''' <param name="nIdTessera"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="nIdAvviso"></param>
    ''' <param name="bGetUI"></param>
    ''' <param name="bGetPesature"></param>
    ''' <returns></returns>
    Public Function GetTessera(ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal nIdContribuente As Integer, ByVal nIdTestata As Integer, ByVal sCodiceUtente As String, ByVal nIdTessera As Integer, ByVal sAnno As String, ByVal nIdAvviso As Integer, ByVal bGetUI As Boolean, ByVal bGetPesature As Boolean) As ObjTessera()
        Dim FncDettaglio As New GestDettaglioTestata
        Dim FncPesature As New GestPesatura
        Dim FncRidEse As New GestRidEse
        Dim oMyTessera As ObjTessera
        Dim oListTessere() As ObjTessera = Nothing
        Dim nTessere As Integer = -1
        Dim oRicRidEse As New ObjRidEse
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TBLTESSERE_S", "IDENTE", "ID", "IDCONTRIBUENTE", "IDTESTATA", "ANNO", "CODUTENTE", "IDAVVISO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                        , ctx.GetParam("ID", nIdTessera) _
                        , ctx.GetParam("IDCONTRIBUENTE", nIdContribuente) _
                        , ctx.GetParam("IDTESTATA", nIdTestata) _
                        , ctx.GetParam("ANNO", sAnno) _
                        , ctx.GetParam("CODUTENTE", sCodiceUtente) _
                        , ctx.GetParam("IDAVVISO", nIdAvviso)
                    )
                ctx.Dispose()
            End Using
            For Each dtMyRow As DataRowView In dvMyDati
                oMyTessera = New ObjTessera

                oMyTessera.Id = StringOperation.FormatInt(dtMyRow("id"))
                oMyTessera.IdTessera = StringOperation.FormatInt(dtMyRow("idtessera"))
                oMyTessera.IdTestata = StringOperation.FormatInt(dtMyRow("idtestata"))
                oMyTessera.IdEnte = StringOperation.FormatString(dtMyRow("ente"))
                oMyTessera.IdContribuente = StringOperation.FormatInt(dtMyRow("contribuente"))
                oMyTessera.sCodUtente = StringOperation.FormatString(dtMyRow("codice_utente"))
                oMyTessera.sCodInterno = StringOperation.FormatString(dtMyRow("codice_interno"))
                oMyTessera.sNumeroTessera = StringOperation.FormatString(dtMyRow("numero_tessera"))
                oMyTessera.tDataRilascio = StringOperation.FormatDateTime(dtMyRow("data_rilascio"))
                oMyTessera.sNote = StringOperation.FormatString(dtMyRow("note_tessera"))
                oMyTessera.tDataInserimento = StringOperation.FormatDateTime(dtMyRow("data_inserimento"))
                oMyTessera.tDataVariazione = StringOperation.FormatDateTime(dtMyRow("data_variazione"))
                oMyTessera.tDataCessazione = StringOperation.FormatDateTime(dtMyRow("data_cessazione"))
                oMyTessera.sOperatore = StringOperation.FormatString(dtMyRow("operatore"))
                '*** 201511 - gestione tipo tessera ***
                oMyTessera.IdTipoTessera = StringOperation.FormatInt(dtMyRow("idtipotessera"))
                '*** ***
                If bGetUI = True Then
                    oMyTessera.oImmobili = FncDettaglio.GetDettaglioTestata(myConnectionString, -1, oMyTessera.Id, -1, sIdEnte, False)
                End If
                If bGetPesature = True Then
                    oMyTessera.oPesature = FncPesature.GetPesatura(myConnectionString, -1, oMyTessera.Id, oMyTessera.sNumeroTessera)
                End If
                Try
                    oRicRidEse.IdEnte = oMyTessera.IdEnte
                    oMyTessera.oRiduzioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_TESSERA, oMyTessera.Id, "")
                    oMyTessera.oDetassazioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_TESSERA, oMyTessera.Id, "")
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.GetTessera.errore: ", ex)
                End Try
                nTessere += 1
                ReDim Preserve oListTessere(nTessere)
                oListTessere(nTessere) = oMyTessera
            Next
            Return oListTessere
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.GetTessera.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function GetTessera(ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal nIdContribuente As Integer, ByVal nIdTestata As Integer, ByVal sCodiceUtente As String, ByVal nIdTessera As Integer, ByVal sAnno As String, ByVal nIdAvviso As Integer, ByVal bGetUI As Boolean, ByVal bGetPesature As Boolean) As ObjTessera()
    '    Dim FncDettaglio As New GestDettaglioTestata
    '    Dim FncPesature As New GestPesatura
    '    Dim FncRidEse As New GestRidEse
    '    Dim oMyTessera As ObjTessera
    '    Dim oListTessere() As ObjTessera = Nothing
    '    Dim nTessere As Integer = -1
    '    Dim oRicRidEse As New ObjRidEse
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_TBLTESSERE_S"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = nIdAvviso
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = nIdTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = nIdTessera
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODUTENTE", SqlDbType.NVarChar)).Value = sCodiceUtente
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyTessera = New ObjTessera

    '            oMyTessera.Id = StringOperation.Formatint(dtMyRow("id"))
    '            oMyTessera.IdTessera = StringOperation.Formatint(dtMyRow("idtessera"))
    '            oMyTessera.IdTestata = StringOperation.Formatint(dtMyRow("idtestata"))
    '            oMyTessera.IdEnte = StringOperation.FormatString(dtMyRow("ente"))
    '            oMyTessera.IdContribuente = StringOperation.Formatint(dtMyRow("contribuente"))
    '            oMyTessera.sCodUtente = StringOperation.FormatString(dtMyRow("codice_utente"))
    '            oMyTessera.sCodInterno = StringOperation.FormatString(dtMyRow("codice_interno"))
    '            If Not IsDBNull(dtMyRow("numero_tessera")) Then
    '                oMyTessera.sNumeroTessera = StringOperation.FormatString(dtMyRow("numero_tessera"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_rilascio")) Then
    '                oMyTessera.tDataRilascio = StringOperation.Formatdatetime(dtMyRow("data_rilascio"))
    '            End If
    '            If Not IsDBNull(dtMyRow("note_tessera")) Then
    '                oMyTessera.sNote = StringOperation.FormatString(dtMyRow("note_tessera"))
    '            End If
    '            oMyTessera.tDataInserimento = StringOperation.Formatdatetime(dtMyRow("data_inserimento"))
    '            If Not IsDBNull(dtMyRow("data_variazione")) Then
    '                oMyTessera.tDataVariazione = StringOperation.Formatdatetime(dtMyRow("data_variazione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_cessazione")) Then
    '                oMyTessera.tDataCessazione = StringOperation.Formatdatetime(dtMyRow("data_cessazione"))
    '            End If
    '            oMyTessera.sOperatore = StringOperation.FormatString(dtMyRow("operatore"))
    '            '*** 201511 - gestione tipo tessera ***
    '            oMyTessera.IdTipoTessera = StringOperation.Formatint(dtMyRow("idtipotessera"))
    '            '*** ***
    '            If bGetUI = True Then
    '                oMyTessera.oImmobili = FncDettaglio.GetDettaglioTestata(myConnectionString, -1, oMyTessera.Id, -1, sIdEnte, False)
    '            End If
    '            If bGetPesature = True Then
    '                oMyTessera.oPesature = FncPesature.GetPesatura(myConnectionString, -1, oMyTessera.Id, oMyTessera.sNumeroTessera)
    '            End If
    '            Try
    '                oRicRidEse.IdEnte = oMyTessera.IdEnte
    '                oMyTessera.oRiduzioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_TESSERA, oMyTessera.Id, "")
    '                oMyTessera.oDetassazioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_TESSERA, oMyTessera.Id, "")
    '            Catch ex As Exception
    '                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.GetTessera.errore: ", ex)
    '            End Try
    '            nTessere += 1
    '            ReDim Preserve oListTessere(nTessere)
    '            oListTessere(nTessere) = oMyTessera
    '        Next

    '        Return oListTessere
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.GetTessera.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="sNTessera"></param>
    ''' <returns></returns>
    Public Function GetNTessera(ByVal myConnectionString As String, ByVal sNTessera As String) As Integer
        '***prelevo la tessera attiva per l'utente: 0= non trovato; >0= trovato/errore***
        Dim nTessera As Integer = -1
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                sSQL = "SELECT *"
                sSQL += " FROM TBLTESSERE"
                sSQL += " WHERE (YEAR(CASE WHEN ISDATE(DATA_VARIAZIONE)=0 THEN '99991231' ELSE DATA_VARIAZIONE END)=9999)"
                sSQL += " AND (YEAR(CASE WHEN ISDATE(DATA_CESSAZIONE)=0 THEN '99991231' ELSE DATA_CESSAZIONE END)=9999 IS NULL)"
                sSQL += " AND (NUMERO_TESSERA=@NTESSERA)"
                sSQL = ctx.GetSQL(DBModel.TypeQuery.View, sSQL)
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("NTESSERA", sNTessera))
                ctx.Dispose()
            End Using
            For Each myRow As DataRowView In dvMyDati
                nTessera = StringOperation.FormatInt(myRow("id"))
            Next
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.GetNTessera.errore: ", Err)
            Return -1
        Finally
            dvMyDati.Dispose()
        End Try
        Return nTessera
    End Function
    'Public Function GetNTessera(ByVal myConnectionString As String, ByVal sNTessera As String) As Integer
    '    '***prelevo la tessera attiva per l'utente: 0= non trovato; >0= trovato/errore***
    '    Dim nTessera As Integer = -1
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text

    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM TBLTESSERE"
    '        cmdMyCommand.CommandText += " WHERE (DATA_VARIAZIONE IS NULL)"
    '        cmdMyCommand.CommandText += " AND (DATA_CESSAZIONE IS NULL)"
    '        cmdMyCommand.CommandText += " AND (NUMERO_TESSERA=@NTESSERA)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NTESSERA ", SqlDbType.NVarChar)).Value = sNTessera
    '        'eseguo la query
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        DrReturn = cmdMyCommand.ExecuteReader
    '        Do While DrReturn.Read
    '            nTessera = StringOperation.FormatInt(DrReturn("id"))
    '        Loop

    '        Return nTessera
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.GetNTessera.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return 1
    '    Finally
    '        DrReturn.Close()
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="sDescrEnte"></param>
    ''' <returns></returns>
    Public Function GetCodUtenteAutomatico(ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal sDescrEnte As String) As String
        Dim sMyCodUtente As String = ""
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            sDescrEnte = sDescrEnte.Replace("Comune di ", "")
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                sSQL = "SELECT IDENTE, MAX(CASE WHEN LEN(CODICE_UTENTE)>2 THEN CASE WHEN ISNUMERIC(SUBSTRING(CODICE_UTENTE,3,LEN(CODICE_UTENTE)-2))=1 THEN CAST(SUBSTRING(CODICE_UTENTE,3,LEN(CODICE_UTENTE)-2) AS NUMERIC) ELSE '0' END ELSE '0' END) AS PROGUTENTE"
                sSQL += " FROM TBLTESSERE"
                sSQL += " WHERE (IDENTE=@IDENTE)"
                sSQL += " GROUP BY IDENTE"
                sSQL = ctx.GetSQL(DBModel.TypeQuery.View, sSQL)
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte))
                ctx.Dispose()
            End Using
            For Each myRow As DataRowView In dvMyDati
                If Not IsDBNull(myRow("progUtente")) Then
                    If sDescrEnte.IndexOf(" ") > 0 Then
                        sMyCodUtente = sDescrEnte.Substring(0, 1) + sDescrEnte.Substring(sDescrEnte.IndexOf(" "), 1)
                    Else
                        sMyCodUtente = sDescrEnte.Substring(0, 2)
                    End If
                    sMyCodUtente += (StringOperation.FormatInt(myRow("progUtente")) + 1).ToString.PadLeft(5, "0")
                End If
            Next
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.GetCodUtenteAutomatico.errore: ", Err)
            Return ""
        Finally
            dvMyDati.Dispose()
        End Try
        Return sMyCodUtente.ToUpper
    End Function
    'Public Function GetCodUtenteAutomatico(ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal sDescrEnte As String) As String
    '    Dim sMyCodUtente As String = ""
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        sDescrEnte = sDescrEnte.Replace("Comune di ", "")

    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        'prelevo i dati
    '        cmdMyCommand.CommandText = "SELECT IDENTE, MAX(CASE WHEN LEN(CODICE_UTENTE)>2 THEN CASE WHEN ISNUMERIC(SUBSTRING(CODICE_UTENTE,3,LEN(CODICE_UTENTE)-2))=1 THEN CAST(SUBSTRING(CODICE_UTENTE,3,LEN(CODICE_UTENTE)-2) AS NUMERIC) ELSE '0' END ELSE '0' END) AS PROGUTENTE"
    '        cmdMyCommand.CommandText += " FROM TBLTESSERE"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        cmdMyCommand.CommandText += " GROUP BY IDENTE"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        'eseguo la query
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        DrReturn = cmdMyCommand.ExecuteReader
    '        If DrReturn.Read Then
    '            If Not IsDBNull(DrReturn("progUtente")) Then
    '                If sDescrEnte.IndexOf(" ") > 0 Then
    '                    sMyCodUtente = sDescrEnte.Substring(0, 1) + sDescrEnte.Substring(sDescrEnte.IndexOf(" "), 1)
    '                Else
    '                    sMyCodUtente = sDescrEnte.Substring(0, 2)
    '                End If
    '                sMyCodUtente += (StringOperation.FormatInt(DrReturn("progUtente")) + 1).ToString.PadLeft(5, "0")
    '            End If
    '        End If
    '        Return sMyCodUtente.ToUpper
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.GetCodUtenteAutomatico.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return ""
    '    Finally
    '        DrReturn.Close()
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function

    Public Function GetTesVSUI(ByVal oMyTestata As ObjTestata) As ObjTesseraUI()
        Dim oListTesUI() As ObjTesseraUI = Nothing
        Dim oMyTesUi As ObjTesseraUI
        Dim nList, x, y As Integer

        Try
            nList = -1
            If Not oMyTestata.oTessere Is Nothing Then
                Log.Debug("GetTesVSUI::oTessere.len::" & oMyTestata.oTessere.GetUpperBound(0).ToString())
                For x = 0 To oMyTestata.oTessere.GetUpperBound(0)
                    If Not IsNothing(oMyTestata.oTessere(x).oImmobili) Then
                        For y = 0 To oMyTestata.oTessere(x).oImmobili.GetUpperBound(0)
                            oMyTesUi = New ObjTesseraUI
                            oMyTesUi.Id = oMyTestata.oTessere(x).Id
                            oMyTesUi.IdTessera = oMyTestata.oTessere(x).IdTessera
                            oMyTesUi.IdTestata = oMyTestata.oTessere(x).IdTestata
                            oMyTesUi.sNumeroTessera = oMyTestata.oTessere(x).sNumeroTessera
                            oMyTesUi.tDataRilascio = oMyTestata.oTessere(x).tDataRilascio
                            oMyTesUi.sNote = oMyTestata.oTessere(x).sNote
                            oMyTesUi.tDataCessazione = oMyTestata.oTessere(x).tDataCessazione
                            oMyTesUi.IdUI = oMyTestata.oTessere(x).oImmobili(y).Id
                            oMyTesUi.IdDettaglioTestata = oMyTestata.oTessere(x).oImmobili(y).IdDettaglioTestata
                            oMyTesUi.sVia = oMyTestata.oTessere(x).oImmobili(y).sVia
                            oMyTesUi.sCivico = oMyTestata.oTessere(x).oImmobili(y).sCivico
                            oMyTesUi.sEsponente = oMyTestata.oTessere(x).oImmobili(y).sEsponente
                            oMyTesUi.sInterno = oMyTestata.oTessere(x).oImmobili(y).sInterno
                            oMyTesUi.sScala = oMyTestata.oTessere(x).oImmobili(y).sScala
                            oMyTesUi.sFoglio = oMyTestata.oTessere(x).oImmobili(y).sFoglio
                            oMyTesUi.sNumero = oMyTestata.oTessere(x).oImmobili(y).sNumero
                            oMyTesUi.sSubalterno = oMyTestata.oTessere(x).oImmobili(y).sSubalterno
                            oMyTesUi.tDataInizio = oMyTestata.oTessere(x).oImmobili(y).tDataInizio
                            oMyTesUi.tDataFine = oMyTestata.oTessere(x).oImmobili(y).tDataFine
                            nList += 1
                            ReDim Preserve oListTesUI(nList)
                            oListTesUI(nList) = oMyTesUi
                        Next
                    Else
                        oMyTesUi = New ObjTesseraUI
                        oMyTesUi.Id = oMyTestata.oTessere(x).Id
                        oMyTesUi.IdTessera = oMyTestata.oTessere(x).IdTessera
                        oMyTesUi.IdTestata = oMyTestata.oTessere(x).IdTestata
                        oMyTesUi.sNumeroTessera = oMyTestata.oTessere(x).sNumeroTessera
                        oMyTesUi.tDataRilascio = oMyTestata.oTessere(x).tDataRilascio
                        oMyTesUi.sNote = oMyTestata.oTessere(x).sNote
                        oMyTesUi.tDataCessazione = oMyTestata.oTessere(x).tDataCessazione
                        nList += 1
                        ReDim Preserve oListTesUI(nList)
                        oListTesUI(nList) = oMyTesUi
                    End If
                Next
            End If
            Return oListTesUI
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.GetTesVSUI.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function GetRiepilogoConferimenti(ByVal myConnectionString As String, ByVal nIdTessera As Integer) As ObjRiepilogoPesature()
        Dim oListConferimenti() As ObjRiepilogoPesature = Nothing
        Dim oMyConferimento As ObjRiepilogoPesature
        Dim nList As Integer = -1
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetRiepilogoPesature", "IDTESSERA")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDTESSERA", nIdTessera))
                ctx.Dispose()
            End Using
            For Each dtMyRow As DataRowView In dvMyDati
                oMyConferimento = New ObjRiepilogoPesature

                oMyConferimento.IdTessera = StringOperation.FormatInt(dtMyRow("idtessera"))
                oMyConferimento.sAnno = StringOperation.FormatString(dtMyRow("anno"))
                '*** 201712 - gestione tipo conferimento ***
                oMyConferimento.TipoConferimento = dtMyRow("tipoconferimento").ToString
                '*** ***
                If Not IsDBNull(dtMyRow("npesature")) Then
                    oMyConferimento.nConferimenti = StringOperation.FormatInt(dtMyRow("npesature"))
                End If
                If Not IsDBNull(dtMyRow("totkg")) Then
                    oMyConferimento.nKG = CDbl(dtMyRow("totkg"))
                End If
                If Not IsDBNull(dtMyRow("totvolume")) Then
                    oMyConferimento.nVolume = CDbl(dtMyRow("totvolume"))
                End If
                nList += 1
                ReDim Preserve oListConferimenti(nList)
                oListConferimenti(nList) = oMyConferimento
            Next
            Return oListConferimenti
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.GetRiepilogoConferimenti.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="oMyConferimento"></param>
    ''' <returns></returns>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    ''' <revisionHistory><revision date="17/02/2021">l'abbinamento deve essere fatto solo sulle tessere con codice, non abbino i conferimenti su tessera NO-TESSERA, filtro messo in stored</revision></revisionHistory>
    Public Function AbbinaTessera(ByVal myConnectionString As String, ByVal oMyConferimento As ObjPesatura) As ObjTessera
        Dim oMyTessera As New ObjTessera
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_AbbinaTessera", "NUMERO_TESSERA", "DATACONFERIMENTO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("NUMERO_TESSERA", oMyConferimento.sNumeroTessera) _
                        , ctx.GetParam("DATACONFERIMENTO", oMyConferimento.tDataOraConferimento.ToShortDateString)
                    )
                ctx.Dispose()
            End Using
            For Each myRow As DataRowView In dvMyDati
                oMyTessera = New ObjTessera
                oMyTessera.Id = StringOperation.FormatInt(myRow("id"))
                oMyTessera.IdTessera = StringOperation.FormatInt(myRow("idtessera"))
                oMyTessera.IdTestata = StringOperation.FormatInt(myRow("idtestata"))
                oMyTessera.IdEnte = StringOperation.FormatString(myRow("idente"))
                oMyTessera.IdContribuente = StringOperation.FormatInt(myRow("idcontribuente"))
                oMyTessera.sCodUtente = StringOperation.FormatString(myRow("codice_utente"))
                oMyTessera.sCodInterno = StringOperation.FormatString(myRow("codice_interno"))
                oMyTessera.sNumeroTessera = StringOperation.FormatString(myRow("numero_tessera"))
                oMyTessera.tDataRilascio = StringOperation.FormatDateTime(myRow("data_rilascio"))
                oMyTessera.sNote = StringOperation.FormatString(myRow("note_tessera"))
                oMyTessera.tDataInserimento = StringOperation.FormatDateTime(myRow("data_inserimento"))
                oMyTessera.tDataVariazione = StringOperation.FormatDateTime(myRow("data_variazione"))
                oMyTessera.tDataCessazione = StringOperation.FormatDateTime(myRow("data_cessazione"))
                oMyTessera.sOperatore = StringOperation.FormatString(myRow("operatore"))
            Next
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.AbbinoTessera.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
        Return oMyTessera
    End Function
    'Public Function AbbinaTessera(ByVal myConnectionString As String, ByVal oMyConferimento As ObjPesatura) As ObjTessera
    '    Dim oMyTessera As New ObjTessera

    '    Try
    '        oMyTessera = Nothing
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_AbbinaTessera"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_TESSERA", SqlDbType.NVarChar)).Value = oMyConferimento.sNumeroTessera
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATACONFERIMENTO", SqlDbType.DateTime)).Value = oMyConferimento.tDataOraConferimento.ToShortDateString
    '        'eseguo la query
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        DrReturn = cmdMyCommand.ExecuteReader
    '        Do While DrReturn.Read
    '            oMyTessera = New ObjTessera
    '            oMyTessera.Id = StringOperation.FormatInt(DrReturn("id"))
    '            oMyTessera.IdTessera = StringOperation.FormatInt(DrReturn("idtessera"))
    '            oMyTessera.IdTestata = StringOperation.FormatInt(DrReturn("idtestata"))
    '            oMyTessera.IdEnte = StringOperation.FormatString(DrReturn("idente"))
    '            oMyTessera.IdContribuente = StringOperation.FormatInt(DrReturn("idcontribuente"))
    '            oMyTessera.sCodUtente = StringOperation.FormatString(DrReturn("codice_utente"))
    '            oMyTessera.sCodInterno = StringOperation.FormatString(DrReturn("codice_interno"))
    '            If Not IsDBNull(DrReturn("numero_tessera")) Then
    '                oMyTessera.sNumeroTessera = StringOperation.FormatString(DrReturn("numero_tessera"))
    '            End If
    '            If Not IsDBNull(DrReturn("data_rilascio")) Then
    '                oMyTessera.tDataRilascio = StringOperation.FormatDateTime(DrReturn("data_rilascio"))
    '            End If
    '            If Not IsDBNull(DrReturn("note_tessera")) Then
    '                oMyTessera.sNote = StringOperation.FormatString(DrReturn("note_tessera"))
    '            End If
    '            oMyTessera.tDataInserimento = StringOperation.FormatDateTime(DrReturn("data_inserimento"))
    '            If Not IsDBNull(DrReturn("data_variazione")) Then
    '                oMyTessera.tDataVariazione = StringOperation.FormatDateTime(DrReturn("data_variazione"))
    '            End If
    '            If Not IsDBNull(DrReturn("data_cessazione")) Then
    '                oMyTessera.tDataCessazione = StringOperation.FormatDateTime(DrReturn("data_cessazione"))
    '            End If
    '            oMyTessera.sOperatore = StringOperation.FormatString(DrReturn("operatore"))
    '        Loop

    '        Return oMyTessera
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.AbbinoTessera.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        DrReturn.Close()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    Public Function HasTessereValide(ByVal oListTessere() As ObjTessera) As Integer
        '0= non trovato; >0= trovato/errore
        Try
            Dim x As Integer
            Dim nTessere As Integer = 0

            If Not oListTessere Is Nothing Then
                For x = 0 To oListTessere.GetUpperBound(0)
                    If oListTessere(x).tDataCessazione = Date.MinValue And oListTessere(x).tDataVariazione = Date.MinValue Then
                        nTessere += 1
                    End If
                Next
            End If

            Return nTessere
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.HasTessereValide.errore: ", Err)
            Return 1
        End Try
    End Function

    Public Function GetTesseraBidone(DBType As String, ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal MyGrid As Ribes.OPENgov.WebControls.RibesGridView, ByVal nIdMyTessera As Integer, ByVal nIdTestata As Integer, ByVal sOperatore As String) As Integer
        Dim itemGrid As GridViewRow
        Dim nIdTesseraBidone As Integer = -1
        Dim FncTessera As New DichManagerTARSU(DBType, myConnectionString, "", "") 'As New GestTessera
        Dim bServeBidone As Boolean = False

        Try
            'faccio un primo ciclo sulla griglia per prelevarmi l'id della tessera bidone
            For Each itemGrid In MyGrid.Rows
                'è l'id tessera in griglia diverso da quello del form
                If nIdMyTessera <> CType(CType(itemGrid.FindControl("hfIDTESSERA"), HiddenField).Value, Long) Then
                    nIdTesseraBidone = CType(CType(itemGrid.FindControl("hfIDTESSERA"), HiddenField).Value, Long)
                End If
                If CType(itemGrid.FindControl("ChkAssociata"), CheckBox).Checked = False Then
                    bServeBidone = True
                End If
            Next
            'se non ho trovato la tessera bidone e serve la creo
            If nIdTesseraBidone < 1 And bServeBidone = True Then
                Dim oMyTessera As New ObjTessera
                oMyTessera.IdTestata = nIdTestata
                oMyTessera.IdEnte = sIdEnte
                oMyTessera.sCodInterno = ObjTessera.TESSERA_BIDONE
                oMyTessera.tDataRilascio = Now
                oMyTessera.tDataInserimento = Now
                oMyTessera.sOperatore = sOperatore
                'aggiungo la nuova tessera
                nIdTesseraBidone = FncTessera.SetTessera(Utility.Costanti.AZIONE_UPDATE, oMyTessera, oMyTessera.IdContribuente)
                If nIdTesseraBidone = 0 Then
                    Throw New Exception("Errore in inserimento nuova tessera bidone!")
                End If
            End If
            Return nIdTesseraBidone
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.GetTesseraBidone.errore: ", Err)
            Throw Err
        End Try
    End Function

    '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
    'Public Function SetTesseraCompleta(ByVal myConnectionString As String, ByVal oMyTessera() As ObjTessera, ByVal nIdTestata As Integer) As Integer
    '    Dim nMyReturn, x, y As Integer
    '    Dim FncRidEse As New GestRidEse

    '    Try
    '        For x = 0 To oMyTessera.GetUpperBound(0)
    '            'memorizzo l'id testata di riferimento
    '            oMyTessera(x).IdTestata = nIdTestata
    '            oMyTessera(x).Id = -1
    '            'richiamo la funzione di inserimento dettaglio testata
    '            nMyReturn = SetTessera(myConnectionString, 0, oMyTessera(x))
    '            If nMyReturn = 0 Then
    '                Return 0
    '            End If
    '            'memorizzo l'id inserito
    '            oMyTessera(x).Id = nMyReturn
    '            'inserisco l'immobile se presente
    '            If Not IsNothing(oMyTessera(x).oImmobili) Then
    '                Dim FncDettaglioTestata As New GestDettaglioTestata
    '                nMyReturn = FncDettaglioTestata.SetDettaglioTestataCompleta(myConnectionString, oMyTessera(x).oImmobili, oMyTessera(x).IdTestata, oMyTessera(x).Id)
    '                If nMyReturn <= 0 Then
    '                    Return 0
    '                End If
    '            End If
    '            If Not oMyTessera(x).oRiduzioni Is Nothing Then
    '                'inserisco i dati di riduzione
    '                If FncRidEse.SetRidEseApplicate(Costanti.AZIONE_NEW, myConnectionString, oMyTessera(x).oRiduzioni, ObjRidEse.TIPO_RIDUZIONI, oMyTessera(x).Id, -1) = 0 Then
    '                    Return 0
    '                End If
    '            End If
    '            If Not oMyTessera(x).oDetassazioni Is Nothing Then
    '                'inserisco i dati di Esenzione
    '                If FncRidEse.SetRidEseApplicate(Costanti.AZIONE_NEW, myConnectionString, oMyTessera(x).oRiduzioni, ObjRidEse.TIPO_RIDUZIONI, oMyTessera(x).Id, -1) = 0 Then
    '                    Return 0
    '                End If
    '            End If
    '        Next
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessera.SetTesseraCompleta.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetTessera(ByVal myConnectionString As String, ByVal DbOperation As Integer, ByVal oTessera As ObjTessera, Optional ByVal nIdContrib As Integer = -1) As Integer
    '    Dim myIdentity As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        Select Case DbOperation
    '            Case 0, 1
    '                'costruisco la query
    '                cmdMyCommand.CommandText = "prc_TBLTESSERE_IU"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oTessera.Id
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESSERA", SqlDbType.Int)).Value = oTessera.IdTessera
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = oTessera.IdTestata
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oTessera.IdContribuente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oTessera.IdEnte
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_UTENTE", SqlDbType.NVarChar)).Value = oTessera.sCodUtente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_TESSERA", SqlDbType.NVarChar)).Value = oTessera.sNumeroTessera
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_INTERNO", SqlDbType.NVarChar)).Value = oTessera.sCodInterno
    '                If oTessera.tDataRilascio = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_RILASCIO", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_RILASCIO", SqlDbType.DateTime)).Value = oTessera.tDataRilascio
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE_TESSERA", SqlDbType.NVarChar)).Value = oTessera.sNote
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oTessera.tDataInserimento
    '                If oTessera.tDataVariazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oTessera.tDataVariazione
    '                End If
    '                If oTessera.tDataCessazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oTessera.tDataCessazione
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oTessera.sOperatore
    '                cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
    '                'eseguo la query
    '                cmdMyCommand.ExecuteNonQuery()
    '                myIdentity = cmdMyCommand.Parameters("@ID").Value
    '            Case 2
    '                'costruisco la query
    '                cmdMyCommand.CommandText = "prc_TBLTESSERE_D"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                If nIdContrib <> -1 Then
    '                    cmdMyCommand.CommandText += " (IDCONTRIBUENTE=@IDCONTRIBUENTE)"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContrib
    '                Else
    '                    cmdMyCommand.CommandText += " (ID=@ID)"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oTessera.Id
    '                End If
    '                'eseguo la query
    '                If cmdMyCommand.ExecuteNonQuery <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oTessera.Id
    '        End Select
    '        Return myIdentity
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessera.SetTessera.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ObjTessera::SetTessera::" & Err.Message & " SQL::" & cmdMyCommand.CommandText)
    '        Return 0
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    '*** ***
    'Public Function SetTesseraAvviso(ByVal nIdAvviso As Integer, ByVal nIdTessera As Integer, ByVal nIdFlusso As Integer, ByVal DbOperation As Integer, ByVal cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim myIdentity As Integer

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '            cmdMyCommand.CommandTimeout = 0
    '        End If
    '        cmdMyCommand.CommandType = CommandType.Text
    '        Select Case DbOperation
    '            Case Utility.Costanti.AZIONE_NEW
    '                cmdMyCommand.CommandText = "INSERT INTO TBLCARTELLETESSERE (IDAVVISO,IDTESSERA)"
    '                cmdMyCommand.CommandText += " VALUES(@IDAVVISO,@IDTESSERA)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = nIdAvviso
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESSERA", SqlDbType.Int)).Value = nIdTessera
    '                'eseguo la query
    '                Log.Debug("SetTesseraAvviso::" & cmdMyCommand.CommandText & " " & Utility.Costanti.GetValParamCmd(cmdMyCommand))
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = cmdMyCommand.ExecuteReader
    '                Do While DrReturn.Read
    '                    myIdentity = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '            Case Utility.Costanti.AZIONE_DELETE
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TBLCARTELLETESSERE"
    '                cmdMyCommand.CommandText += " WHERE (1=1)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                If nIdAvviso <> -1 Then
    '                    cmdMyCommand.CommandText += " AND (IDAVVISO=@IDAVVISO)"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = nIdAvviso
    '                End If
    '                If nIdTessera <> -1 Then
    '                    cmdMyCommand.CommandText += " AND (IDTESSERA=@IDTESSERA)"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESSERA", SqlDbType.Int)).Value = nIdTessera
    '                End If
    '                If nIdFlusso <> -1 Then
    '                    cmdMyCommand.CommandText += " AND (IDAVVISO IN ("
    '                    cmdMyCommand.CommandText += "  SELECT ID "
    '                    cmdMyCommand.CommandText += "  FROM TBLCARTELLE"
    '                    cmdMyCommand.CommandText += "  WHERE IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO))"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlusso
    '                End If
    '                'eseguo la query
    '                Log.Debug("SetTesseraAvviso::" & cmdMyCommand.CommandText & " " & Utility.Costanti.GetValParamCmd(cmdMyCommand))
    '                If cmdMyCommand.ExecuteNonQuery <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = 1
    '        End Select
    '        Return myIdentity
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessera.SetTesseraAvviso.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ObjTessera::SetTesseraAvviso::" & Err.Message & " SQL::" & cmdMyCommand.CommandText)
    '        Return 0
    '    Finally
    '        If cmdMyCommandOut Is Nothing Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    '*** 20131104 - TARES ***
    'Public Function GetTessera(ByVal sIdEnte As String, ByVal nIdContribuente As Integer, ByVal nIdTestata As Integer, ByVal sCodiceUtente As String, ByVal nIdTessera As Integer, ByVal sAnno As String, ByVal nIdAvviso As Integer, ByVal bGetUI As Boolean, ByVal bGetPesature As Boolean, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjTessera()
    '    Dim oMyTessera As ObjTessera
    '    Dim oListTessere As New ArrayList
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        cmdMyCommand.CommandText = "prc_GetTessera"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.Int)).Value = sAnno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = nIdTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = nIdAvviso
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODUTENTE", SqlDbType.VarChar)).Value = sCodiceUtente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = nIdTessera
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyTessera = New ObjTessera

    '            oMyTessera.Id = StringOperation.FormatInt(dtMyRow("id"))
    '            oMyTessera.IdTessera = StringOperation.FormatInt(dtMyRow("idtessera"))
    '            oMyTessera.IdTestata = StringOperation.FormatInt(dtMyRow("idtestata"))
    '            oMyTessera.IdEnte = StringOperation.FormatString(dtMyRow("ente"))
    '            oMyTessera.IdContribuente = StringOperation.FormatInt(dtMyRow("contribuente"))
    '            oMyTessera.sCodUtente = StringOperation.FormatString(dtMyRow("codice_utente"))
    '            oMyTessera.sCodInterno = StringOperation.FormatString(dtMyRow("codice_interno"))
    '            If Not IsDBNull(dtMyRow("numero_tessera")) Then
    '                oMyTessera.sNumeroTessera = StringOperation.FormatString(dtMyRow("numero_tessera"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_rilascio")) Then
    '                oMyTessera.tDataRilascio = StringOperation.FormatDateTime(dtMyRow("data_rilascio"))
    '            End If
    '            If Not IsDBNull(dtMyRow("note_tessera")) Then
    '                oMyTessera.sNote = StringOperation.FormatString(dtMyRow("note_tessera"))
    '            End If
    '            oMyTessera.tDataInserimento = StringOperation.FormatDateTime(dtMyRow("data_inserimento"))
    '            If Not IsDBNull(dtMyRow("data_variazione")) Then
    '                oMyTessera.tDataVariazione = StringOperation.FormatDateTime(dtMyRow("data_variazione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_cessazione")) Then
    '                oMyTessera.tDataCessazione = StringOperation.FormatDateTime(dtMyRow("data_cessazione"))
    '            End If
    '            oMyTessera.sOperatore = StringOperation.FormatString(dtMyRow("operatore"))
    '            '*** 201511 - gestione tipo tessera ***
    '            oMyTessera.IdTipoTessera = StringOperation.FormatInt(dtMyRow("idtipotessera"))
    '            '*** ***
    '            oListTessere.Add(oMyTessera)
    '        Next

    '        Return CType(oListTessere.ToArray(GetType(ObjTessera)), ObjTessera())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTessera.GetTessera.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    '*** ***
    'Public Function GetTessera(ByVal sIdEnte As String, ByVal nIdContribuente As Integer, ByVal nIdTestata As Integer, ByVal sCodiceUtente As String, ByVal nIdTessera As Integer, ByVal sAnno As String, ByVal nIdAvviso As Integer, ByVal bGetUI As Boolean, ByVal bGetPesature As Boolean, ByRef DBEngineOut As DBEngine) As ObjTessera()
    '    Dim FncDettaglio As New GestDettaglioTestata
    '    Dim FncPesature As New GestPesatura
    '    Dim FncRidEse As New GestRidEse
    '    Dim oMyTessera As ObjTessera
    '    Dim oListTessere As New ArrayList
    '    Dim nTessere As Integer = -1
    '    Dim oRicRidEse As New ObjRidEse
    '    Dim MyDBEngine As DBEngine = Nothing
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (DBEngineOut) Is Nothing) Then
    '            MyDBEngine = DBEngineOut
    '        Else
    '            MyDBEngine = Utility.DBEngineFactory.GetDBEngine(ConstSession.StringConnection)
    '            MyDBEngine.OpenConnection()
    '        End If

    '        MyDBEngine.ClearParameters()
    '        MyDBEngine.AddParameter("@IdEnte", sIdEnte, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@IDCONTRIBUENTE", nIdContribuente, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@IDAVVISO", nIdAvviso, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@IDTESTATA", nIdTestata, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@CODUTENTE", sCodiceUtente, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@ID", nIdTessera, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@Anno", sAnno, ParameterDirection.Input)
    '        MyDBEngine.ExecuteQuery(dtMyDati, "prc_GetTessera", CommandType.StoredProcedure)
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyTessera = New ObjTessera

    '            oMyTessera.Id = StringOperation.Formatint(dtMyRow("id"))
    '            oMyTessera.IdTessera = StringOperation.Formatint(dtMyRow("idtessera"))
    '            oMyTessera.IdTestata = StringOperation.Formatint(dtMyRow("idtestata"))
    '            oMyTessera.IdEnte = StringOperation.FormatString(dtMyRow("ente"))
    '            oMyTessera.IdContribuente = StringOperation.Formatint(dtMyRow("contribuente"))
    '            oMyTessera.sCodUtente = StringOperation.FormatString(dtMyRow("codice_utente"))
    '            oMyTessera.sCodInterno = StringOperation.FormatString(dtMyRow("codice_interno"))
    '            If Not IsDBNull(dtMyRow("numero_tessera")) Then
    '                oMyTessera.sNumeroTessera = StringOperation.FormatString(dtMyRow("numero_tessera"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_rilascio")) Then
    '                oMyTessera.tDataRilascio = StringOperation.Formatdatetime(dtMyRow("data_rilascio"))
    '            End If
    '            If Not IsDBNull(dtMyRow("note_tessera")) Then
    '                oMyTessera.sNote = StringOperation.FormatString(dtMyRow("note_tessera"))
    '            End If
    '            oMyTessera.tDataInserimento = StringOperation.Formatdatetime(dtMyRow("data_inserimento"))
    '            If Not IsDBNull(dtMyRow("data_variazione")) Then
    '                oMyTessera.tDataVariazione = StringOperation.Formatdatetime(dtMyRow("data_variazione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_cessazione")) Then
    '                oMyTessera.tDataCessazione = StringOperation.Formatdatetime(dtMyRow("data_cessazione"))
    '            End If
    '            oMyTessera.sOperatore = StringOperation.FormatString(dtMyRow("operatore"))
    '            'If bGetUI = True Then
    '            '    oMyTessera.oImmobili = FncDettaglio.GetDettaglioTestata(oMyTessera.Id, -1, sIdEnte, False, MyDBEngine)
    '            'End If
    '            'If bGetPesature = True Then
    '            '    oMyTessera.oPesature = FncPesature.GetPesatura(Nothing, -1, oMyTessera.Id)
    '            'End If
    '            'Try
    '            '    oRicRidEse.IdEnte = oMyTessera.IdEnte
    '            '    oMyTessera.oRiduzioni = FncRidEse.GetRidEseApplicate(oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_TESSERA, oMyTessera.Id, "", MyDBEngine)
    '            '    oMyTessera.oDetassazioni = FncRidEse.GetRidEseApplicate(oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_TESSERA, oMyTessera.Id, "", MyDBEngine)
    '            'Catch ex As Exception
    '            '    Log.Debug("Si è verificato un errore in ObjTessera::GetTessera::in caricamento riduzioni")
    '            'End Try
    '            oListTessere.Add(oMyTessera)
    '        Next

    '        Return CType(oListTessere.ToArray(GetType(ObjTessera)), ObjTessera())
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessera.HasTessereValide.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ObjTessera::GetTessera::" & Err.Message & " SQL::" & cmdMyCommand.CommandText)
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (DBEngineOut Is Nothing) Then
    '            MyDBEngine.CloseConnection()
    '        End If
    '    End Try
    'End Function

    'Public Function SetTesseraAvviso(ByVal nIdAvviso As Integer, ByVal nIdTessera As Integer, ByVal nIdFlusso As Integer, ByVal DbOperation As Integer, ByRef DBEngineOut As DBEngine) As Integer
    '    Dim MyDBEngine As DBEngine = Nothing
    '    Dim MyParamCollection As IDataParameterCollection
    '    Dim MyParam As IDataParameter
    '    Dim MyProcedure As String = "prc_TBLCARTELLETESSERE_IU"

    '    Try
    '        If (Not (DBEngineOut) Is Nothing) Then
    '            MyDBEngine = DBEngineOut
    '        Else
    '            MyDBEngine = Utility.DBEngineFactory.GetDBEngine(ConstSession.StringConnection)
    '            MyDBEngine.OpenConnection()
    '            MyDBEngine.BeginTransaction()
    '        End If
    '        MyDBEngine.ClearParameters()
    '        MyDBEngine.AddParameter("@ID", -1, ParameterDirection.InputOutput)
    '        MyDBEngine.AddParameter("@IDAVVISO", nIdAvviso, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@IDTESSERA", nIdTessera, ParameterDirection.Input)
    '        Select Case DbOperation
    '            Case Costanti.AZIONE_NEW
    '                MyProcedure = "prc_TBLCARTELLETESSERE_IU"
    '            Case Costanti.AZIONE_DELETE
    '                MyDBEngine.AddParameter("@IDFLUSSO_RUOLO", nIdFlusso, ParameterDirection.Input)
    '                MyProcedure = "prc_TBLCARTELLETESSERE_D"
    '        End Select
    '        MyParamCollection = MyDBEngine.ExecuteNonQuery(MyProcedure, CommandType.StoredProcedure)
    '        MyParam = CType(MyParamCollection("@ID"), IDataParameter)
    '        If (DBEngineOut Is Nothing) Then
    '            MyDBEngine.CommitTransaction()
    '        End If
    '        Return StringOperation.Formatint(MyParam.Value)
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessera.SetTesseraAvviso.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ObjTessera::SetTesseraAvviso::" & Err.Message & " SQL::" & cmdMyCommand.CommandText)
    '        If (DBEngineOut Is Nothing) Then
    '            MyDBEngine.RollbackTransaction()
    '        End If
    '        Return 0
    '    Finally
    '        If (DBEngineOut Is Nothing) Then
    '            MyDBEngine.CloseConnection()
    '        End If
    '    End Try
    'End Function

    'Public Function GetRiepilogoConferimenti(ByVal nIdTessera As Integer, ByRef DBEngineOut As DBEngine) As ObjRiepilogoPesature()
    '    Dim oListConferimenti() As ObjRiepilogoPesature
    '    Dim oMyConferimento As ObjRiepilogoPesature
    '    Dim nList As Integer = -1
    '    Dim MyDBEngine As DBEngine = Nothing
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (DBEngineOut) Is Nothing) Then
    '            MyDBEngine = DBEngineOut
    '        Else
    '            MyDBEngine = Utility.DBEngineFactory.GetDBEngine(ConstSession.StringConnection)
    '            MyDBEngine.OpenConnection()
    '        End If

    '        MyDBEngine.ClearParameters()
    '        MyDBEngine.AddParameter("@IDTESSERA", nIdTessera, ParameterDirection.Input)
    '        MyDBEngine.ExecuteQuery(dtMyDati, "prc_GetRiepilogoConferimenti", CommandType.StoredProcedure)
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyConferimento = New ObjRiepilogoPesature

    '            oMyConferimento.IdTessera = StringOperation.Formatint(dtMyRow("idtessera"))
    '            oMyConferimento.sAnno = StringOperation.FormatString(dtMyRow("anno"))
    '            If Not IsDBNull(dtMyRow("npesature")) Then
    '                oMyConferimento.nConferimenti = StringOperation.Formatint(dtMyRow("npesature"))
    '            End If
    '            If Not IsDBNull(dtMyRow("totkg")) Then
    '                oMyConferimento.nKG = CDbl(dtMyRow("totkg"))
    '            End If
    '            If Not IsDBNull(dtMyRow("totvolume")) Then
    '                oMyConferimento.nVolume = CDbl(dtMyRow("totvolume"))
    '            End If
    '            nList += 1
    '            ReDim Preserve oListConferimenti(nList)
    '            oListConferimenti(nList) = oMyConferimento
    '        Next

    '        Return oListConferimenti
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessera.GetRiepilogoConferimenti.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (DBEngineOut Is Nothing) Then
    '            MyDBEngine.CloseConnection()
    '        End If
    '    End Try
    'End Function
End Class
