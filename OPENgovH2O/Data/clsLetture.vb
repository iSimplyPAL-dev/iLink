Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility

Public Class clsLetture
    Private Shared Log As ILog = LogManager.GetLogger(GetType(clsLetture))
    Dim clsModDate As New ClsGenerale.Generale
    Dim iDB As New DBAccess.getDBobject
    '====================================================================
    'CONTROLLO DELLA DATA DI LETTURA
#Region "CONTROLLO DATA LETTURA"
    'Public Sub ControllaDataLettura(ByVal strCodContatore As String, ByVal strDataLettura As String, ByRef lngConsumoTeorico As Long, ByRef lngGiorniDiConsumo As Long, ByRef lngLetturaTeorica As Long, ByRef blnDataValida As Boolean, ByVal strCodUtente As String, ByVal strCodContratto As String)
    '    Try
    '        Dim sSQL, strDataLetturaPrecedente As String
    '        Dim lngRecordCount, lngLetturaPrecedente As Long
    '        Dim dblConsumoTeorico, dblResult, dblMediaConsumo, dblRapportoCGG As Double
    '        Dim blnPrimaLettura As Boolean
    '        Dim FncLet As New GestLetture

    '        'Ricavo se Esiste L'ultima Lettura Eseguita e Salvata in TP_Letture
    '        'sSQL= getTopOneLetture(strCodContatore, strCodUtente, "DESC", ";") 
    '        sSQL = FncLet.EstraiDatiLetturaPrecedente(strCodContatore, strDataLettura)

    '        sSQL += getTopFiveLetture(strCodContatore,
    '                                      strCodUtente, "DESC", ";") 

    '        sSQL += getValoreFondoScala(strCodContatore) 
    '        Log.Debug("ControllaDataLettura::prelevo valori::" & sSQL)
    '        Dim sqlAttiv As String
    '        Dim drAttiv as new dataview
    '        Dim myAttiv As String
    '        sqlAttiv = getDataAttivazioneContatore(strCodContatore)
    '        drAttiv = iDB.getdataview(sqlAttiv)
    '        If drAttiv.Read() Then
    '            myAttiv = drAttiv("DATAATTIVAZIONE")
    '        End If
    '        drAttiv.Close()

    '        Dim dvMyDati as new dataview
    '        Try
    '            dvMyDati = iDB.getdataview(sSQL)
    '        Catch EX As Exception

    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.ControllaDataLettura.errore: ", EX)
    '        End Try
    '        If dvMyDati.Read() Then
    '            Try
    '                'VERIFICA DELLA DATA
    '                If stringoperation.formatint(clsModDate.GiraData(strDataLettura)) < stringoperation.formatint(myAttiv) Then
    '                    dvmydati.dispose()
    '                    blnDataValida = True
    '                    Exit Sub
    '                End If
    '                If stringoperation.formatint(clsModDate.GiraData(strDataLettura)) <= stringoperation.formatint(myrow("DATALETTURA")) Then
    '                    dvmydati.dispose()
    '                    blnDataValida = True
    '                    Exit Sub
    '                End If

    '                'Dati da Lettura Attuale - Lettura precedente
    '                lngLetturaPrecedente = stringoperation.formatint(myrow("LETTURA"))
    '                strDataLetturaPrecedente = clsModDate.GiraDataFromDB(stringoperation.FormatString(myrow("DATALETTURA")))

    '                'Determino i Giorni di Consumo dati dalla differenza delle due date
    '                lngGiorniDiConsumo = getGiorniDiConsumo(strDataLetturaPrecedente,
    '                strDataLettura)

    '                'Verifico se la lettura fatturata e la prima lettura del contatore (Es:---Nuovo Contatore---)
    '                blnPrimaLettura = stringoperation.FormatBool(stringoperation.FormatBool(myrow("PRIMALETTURA")))
    '            Catch ex As Exception

    '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.ControllaDataLettura.errore: ", ex)
    '            End Try
    '            If blnPrimaLettura Then
    '                dvmydati.dispose()
    '                ConsumoLetturaTeorici_PrimaLettura(strCodContatore,
    '                 dblConsumoTeorico,
    '                 lngConsumoTeorico, lngLetturaTeorica,
    '                 lngLetturaPrecedente,
    '                 lngGiorniDiConsumo,
    '                 strCodContratto)
    '            Else
    '                dvMyDati.NextResult()
    '                While dvMyDati.Read
    '                    Log.Debug("ControllaDataLettura::devo calcolare giorni consumo")
    '                    dblRapportoCGG = stringoperation.formatint(myrow("CONSUMO")) / stringoperation.formatint(myrow("GIORNIDICONSUMO"))
    '                    If stringoperation.formatint(myrow("GIORNIDICONSUMO")) = 0 Then
    '                        'Giorni di Consumo =0 situazione anomala
    '                        dblRapportoCGG = 0
    '                    End If
    '                    dblResult = dblResult + dblRapportoCGG
    '                    lngRecordCount = lngRecordCount + 1
    '                End While
    '                If lngRecordCount > 0 Then
    '                    Try
    '                        dblMediaConsumo = dblResult / lngRecordCount
    '                    Catch ex As Exception When lngRecordCount = 0
    '                        Log.Debug("ControllaDataLettura::giornidiconsumo::", ex)
    '                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.ControllaDataLettura.errore: ", ex)
    '                    Finally
    '                        dblConsumoTeorico = dblMediaConsumo * lngGiorniDiConsumo     ' -->GIORNI DI CONSUMO
    '                    End Try
    '                    'Approssimo per eccesso dblConsumoTeorico
    '                    lngConsumoTeorico = ApprossimaNumero(dblConsumoTeorico)
    '                End If
    '                dvMyDati.NextResult()

    '                If dvMyDati.Read() Then
    '                    Log.Debug("ControllaDataLettura::calcolo lettura teorica")
    '                    lngLetturaTeorica = CalcolaLetturaTeorica(lngLetturaPrecedente,
    '                    lngConsumoTeorico,
    '                    stringoperation.formatint(myrow("VALOREFONDOSCALA")))
    '                End If
    '            End If
    '        End If
    '        If Not dvMyDati.IsClosed Then
    '            dvmydati.dispose()
    '        End If
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.ControllaDataLettura.errore: ", ex)
    '    End Try
    'End Sub

    Public Sub ControllaDataSostituzione(ByVal strDataLettura As String,
ByRef blnDataValida As Boolean, ByVal strDataSostituzione As String)

        'VERIFICA DELLA DATA
        Try
            If StringOperation.FormatInt(clsModDate.GiraData(strDataLettura)) > StringOperation.FormatInt(clsModDate.GiraData(strDataSostituzione)) Then
                'DATA NON VALIDA
                blnDataValida = True
                Exit Sub
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.ControllaDataSostituzione.errore: ", ex)
        End Try
    End Sub

    Public Sub ControllaDataAttivazione(ByVal strCodContatore As String,
  ByVal strDataLettura As String,
 ByVal strDataAttivazione As String,
 ByRef lngConsumoTeorico As Long,
 ByRef lngGiorniDiConsumo As Long,
 ByRef lngLetturaTeorica As Long,
 ByRef blnDataValida As Boolean,
 ByVal strCodUtente As String,
 ByVal strCodContratto As String)
        Try
            'VERIFICA DELLA DATA
            If StringOperation.FormatInt(clsModDate.GiraData(strDataLettura)) < StringOperation.FormatInt(clsModDate.GiraData(strDataAttivazione)) Then
                blnDataValida = True
                Exit Sub
            End If

            'Determino i Giorni di Consumo dati dalla differenza delle due date
            lngGiorniDiConsumo = getGiorniDiConsumo(strDataAttivazione,
            strDataLettura)

            lngConsumoTeorico = 0
            lngLetturaTeorica = 0
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.ControllaDataAttivazione.errore: ", ex)
        End Try
    End Sub


#End Region
    'FINE CONTROLLO DELLA DATA DI LETTURA
    '====================================================================
    'CONTROLLO DELLA PRIMA LETTURA
#Region "CONTROLLO PRIMA LETTURA"
    'Private Function ConsumoLetturaTeorici_PrimaLettura(ByVal strCodContatore As String,
    'ByRef dblConsumoTeorico As Double,
    'ByRef lngConsumoTeorico As Long,
    'ByRef lngLetturaTeorica As Long,
    'ByRef lngLetturaPrecedente As Long,
    'ByRef lngGiorniDiConsumo As Long,
    'ByVal strCodContratto As String)
    '    Dim sSQL As String
    '    Dim dblResult As Double
    '    Dim blnConsumoMinimoContrattuale As Boolean

    '    blnConsumoMinimoContrattuale = False

    '    sSQL = "SELECT  TP_CONTRATTI.CONSUMOMINIMO  FROM TP_CONTRATTI  "
    '    sSQL += "WHERE "
    '    sSQL += "CODCONTRATTO=" & strCodContratto 
    '    sSQL += "AND "
    '    sSQL += "CONSUMOMINIMO IS NOT NULL  "
    '    Dim dvMyDati as new dataview = iDB.getdataview(sSQL)
    '    Try
    '        If dvMyDati.Read Then

    '            dblResult = (stringoperation.FormatString(myrow("CONSUMOMINIMO")) / 365) ' Consumo giornaliero
    '            dblConsumoTeorico = dblResult * lngGiorniDiConsumo  '-->GIORNI DI CONSUMO
    '            dblConsumoTeorico = FormatNumber(dblConsumoTeorico, 2)

    '            blnConsumoMinimoContrattuale = True

    '        End If

    '        If Not dvMyDati.IsClosed Then
    '            dvmydati.dispose()
    '        End If

    '        'Verifico se trovo nella tabella TIPIUTENZA il campo minimo fatturabile su base annua

    '        If Not blnConsumoMinimoContrattuale Then

    '            sSQL = "SELECT  TP_TIPIUTENZA.CONSUMOMINIMOANNUO "
    '            sSQL += "FROM TP_CONTATORI INNER JOIN "
    '            sSQL += "TP_TIPIUTENZA ON TP_CONTATORI.IDTIPOUTENZA = TP_TIPIUTENZA.IDTIPOUTENZA "
    '            sSQL += "WHERE "
    '            sSQL += "CODCONTATORE=" & strCodContatore 


    '            dvMyDati = iDB.getdataview(sSQL)

    '            If dvMyDati.Read Then
    '                dblResult = stringoperation.formatint(myrow("CONSUMOMINIMOANNUO")) / 365    ' Consumo giornaliero
    '                dblConsumoTeorico = dblResult * lngGiorniDiConsumo  ' -->GIORNI DI CONSUMO
    '            End If

    '        End If

    '        If Not dvMyDati.IsClosed Then
    '            dvmydati.dispose()
    '        End If

    '        lngConsumoTeorico = ApprossimaNumero(dblConsumoTeorico)

    '        dvMyDati = iDB.getdataview(getValoreFondoScala(strCodContatore))


    '        If dvMyDati.Read() Then
    '            lngLetturaTeorica = CalcolaLetturaTeorica(lngLetturaPrecedente,
    '           lngConsumoTeorico,
    '           stringoperation.formatint(myrow("VALOREFONDOSCALA")))
    '        End If

    '        If Not dvMyDati.IsClosed Then
    '            dvmydati.dispose()
    '        End If
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.ConsumoLetturaTeorici_PrimaLettura.errore: ", ex)
    '    End Try
    'End Function

#End Region
    'FINE CONTROLLO DELLA PRIMA LETTURA
    '====================================================================
    'CALCOLA CONSUMO EFFETTIVO
#Region "CALCOLA CONSUMO EFFETTIVO"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strLettura"></param>
    ''' <param name="lngConsumoEffettivo"></param>
    ''' <param name="sDataLetPrec"></param>
    ''' <param name="strCodContatore"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Sub CalcolaConsumoEffettivo(ByVal strLettura As String, ByRef lngConsumoEffettivo As Long, ByVal sDataLetPrec As String, ByVal strCodContatore As String)
        Dim dvMyDati As New DataView
        Dim lngLetturaPrecedente, nConsumoSub As Integer
        Dim FncLetture As New GestLetture

        'Ricavo se Esiste L'ultima Lettura Eseguita e fatturata
        dvMyDati = GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 1, strCodContatore, sDataLetPrec, "<")
        Try
            Dim myContatore As New objContatore
            myContatore = New GestContatori().GetDetailsContatori(strCodContatore, -1)
            For Each myRow As DataRowView In dvMyDati
                lngLetturaPrecedente = StringOperation.FormatInt(myRow("lettura"))
                lngConsumoEffettivo = StringOperation.FormatInt(strLettura) - lngLetturaPrecedente
            Next

            ' Si verifica quando la Lettura Attuale é minore di quella precedente
            If lngConsumoEffettivo < 0 Then
                lngConsumoEffettivo = myContatore.nFondoScala - lngLetturaPrecedente
                lngConsumoEffettivo = (StringOperation.FormatInt(strLettura) - 0) + lngConsumoEffettivo
            End If
            '***tolgo l'eventuale consumo del subcontatore associato***
            nConsumoSub = FncLetture.GetConsumoSubContatore(CInt(strCodContatore))
            If nConsumoSub < 0 Then
                Exit Sub
            End If
            lngConsumoEffettivo -= nConsumoSub
            '**********************************************************
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.CalcolaConsumoEffettivo.errore: ", ex)
        End Try
    End Sub
    'Public Sub CalcolaConsumoEffettivo(ByVal strLettura As String, ByRef lngConsumoEffettivo As Long, ByVal sDataLetPrec As String, ByVal strCodContatore As String)
    '    Dim dvMyDati as new dataview
    '    Dim lngValoreFondoScala, lngLetturaPrecedente, nConsumoSub As Long
    '    Dim FncLetture As New GestLetture

    '    'Ricavo se Esiste L'ultima Lettura Eseguita e fatturata
    '    dvMyDati = iDB.getdataview(FncLetture.EstraiDatiLetturaPrecedente(strCodContatore, sDataLetPrec))
    '    Try
    '        If dvMyDati.Read() Then
    '            lngLetturaPrecedente = stringoperation.formatint(myrow("LETTURA"))
    '            lngConsumoEffettivo = stringoperation.formatint(strLettura) - stringoperation.formatint(myrow("LETTURA"))

    '            If Not dvMyDati.IsClosed Then
    '                dvmydati.dispose()
    '            End If

    '            ' Si verifica quando la Lettura Attuale é minore di quella precedente
    '            If lngConsumoEffettivo < 0 Then
    '                'Verifico e considero  il Giro Contatore
    '                dvMyDati = iDB.getdataview(getValoreFondoScala(strCodContatore))
    '                dvMyDati.Read()

    '                lngValoreFondoScala = stringoperation.formatint(myrow("VALOREFONDOSCALA"))
    '                lngConsumoEffettivo = lngValoreFondoScala - lngLetturaPrecedente
    '                lngConsumoEffettivo = (stringoperation.formatint(strLettura) - 0) + lngConsumoEffettivo

    '                dvmydati.dispose()
    '            End If
    '        End If
    '        '***tolgo l'eventuale consumo del subcontatore associato***
    '        nConsumoSub = FncLetture.GetConsumoSubContatore(CInt(strCodContatore))
    '        If nConsumoSub < 0 Then
    '            Exit Sub
    '        End If
    '        lngConsumoEffettivo -= nConsumoSub
    '        '**********************************************************
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.CalcolaConsumoEffettivo.errore: ", ex)
    '    End Try
    'End Sub
#End Region
    'FINE CALCOLA CONSUMO EFFETTIVO
    '====================================================================
    'VERIFICA TOLLERANZA CONSUMO

#Region "VERIFICA TOLLERANZA CONSUMO"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="lngConsumoEffettivo"></param>
    ''' <param name="lngConsumoTeorico"></param>
    ''' <param name="strCodContatore"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function VerificaTolleranzaConsumo(ByRef lngConsumoEffettivo As Integer, ByRef lngConsumoTeorico As Integer, ByVal strCodContatore As String) As Boolean
        Dim nSogliaTolleranzaPositiva, nSogliaTolleranzaNegativa As Integer
        Dim dblConsumoTolleratoPositivo, dblConsumoTolleratoNegativo As Double
        Dim myRet As Boolean = False
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetSogliaUtenza", "CODCONTATORE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODCONTATORE", strCodContatore))
                ctx.Dispose()
            End Using
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    nSogliaTolleranzaPositiva = StringOperation.FormatInt(myRow("SOGLIAPOSITIVA"))
                    nSogliaTolleranzaNegativa = StringOperation.FormatInt(myRow("SOGLIANEGATIVA"))
                Next
            End If

            dblConsumoTolleratoPositivo = lngConsumoTeorico + ((lngConsumoTeorico * nSogliaTolleranzaPositiva) / 100)
            dblConsumoTolleratoNegativo = lngConsumoTeorico - ((lngConsumoTeorico * nSogliaTolleranzaNegativa) / 100)

            If lngConsumoEffettivo >= dblConsumoTolleratoNegativo And lngConsumoEffettivo <= dblConsumoTolleratoPositivo Then
                myRet = True
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.VerificaTolleranzaConsumo.errore: ", ex)
            myRet = False
        Finally
            dvMyDati.Dispose()
        End Try
        Return myRet
    End Function
    'Public Function VerificaTolleranzaConsumo(ByRef lngConsumoEffettivo As Long,
    ' ByRef lngConsumoTeorico As Long, ByVal strCodContatore As String) As Boolean

    '    Dim sSQL As String
    '    Dim dvMyDati as new dataview
    '    Dim lngSogliaTolleranzaPositiva, lngSogliaTolleranzaNegativa As Long
    '    Dim dblConsumoTolleratoPositivo,
    '       dblConsumoTolleratoNegativo As Double

    '    VerificaTolleranzaConsumo = False

    '    sSQL = "SELECT  TP_TIPIUTENZA.SOGLIAPOSITIVA,TP_TIPIUTENZA.SOGLIANEGATIVA "
    '    sSQL += "FROM TP_CONTATORI INNER JOIN "
    '    sSQL += "TP_TIPIUTENZA ON TP_CONTATORI.IDTIPOUTENZA = TP_TIPIUTENZA.IDTIPOUTENZA "
    '    sSQL += "WHERE "
    '    sSQL += "CODCONTATORE=" & strCodContatore

    '    dvMyDati = iDB.getdataview(sSQL)
    '    Try
    '        If dvMyDati.Read() Then
    '            lngSogliaTolleranzaPositiva = stringoperation.formatint(myrow("SOGLIAPOSITIVA"))
    '            lngSogliaTolleranzaNegativa = stringoperation.formatint(myrow("SOGLIANEGATIVA"))
    '        End If
    '        dvmydati.dispose()

    '        dblConsumoTolleratoPositivo = lngConsumoTeorico + ((lngConsumoTeorico * lngSogliaTolleranzaPositiva) / 100)
    '        dblConsumoTolleratoNegativo = lngConsumoTeorico - ((lngConsumoTeorico * lngSogliaTolleranzaNegativa) / 100)

    '        If lngConsumoEffettivo >= dblConsumoTolleratoNegativo And lngConsumoEffettivo <= dblConsumoTolleratoPositivo Then
    '            VerificaTolleranzaConsumo = True
    '        End If
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.VerificaTolleranzaConsumo.errore: ", ex)
    '    End Try

    'End Function
    'FINE TOLLERANZA CONSUMO

#End Region
    'FINE VERIFICA TOLLERANZA CONSUMO
    '====================================================================
    'VERIFICA DATA GRIGLIA
#Region "Verifica Data Griglia"
    Public Function VerificaDataGriglia(ByVal strDataLettura As String, ByVal m_lngDataGriglia As Long,
   ByVal strCodContatore As String,
   ByVal strCodUtente As String,
   ByRef lngDataLetturaSuccessiva As Long,
   ByRef lngDataLetturaPrecedente As Long) As Boolean

        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim lngDataLettura As Long

        Try
            lngDataLettura = StringOperation.FormatInt(clsModDate.GiraData(strDataLettura))

            lngDataLetturaSuccessiva = 0
            lngDataLetturaPrecedente = 0
            VerificaDataGriglia = False

            'Verifico se esiste una data successiva
            sSQL = ""
            sSQL = "SELECT TOP 1 DATALETTURA "
            sSQL += "FROM TP_LETTURE "
            sSQL += "WHERE CODCONTATORE=" & strCodContatore
            'sSQL+="AND CODUTENTE=" & strCodUtente 
            sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL) "
            sSQL += "AND DATALETTURA > " & StringOperation.FormatString(m_lngDataGriglia)
            sSQL += " ORDER BY DATALETTURA"
            'Log.Debug("VerificaDataGriglia::query1::" & sSQL)
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    lngDataLetturaSuccessiva = StringOperation.FormatInt(myRow("DATALETTURA"))
                Next
            End If
            dvMyDati.Dispose()

            sSQL = "SELECT TOP 1 DATALETTURA "
            sSQL += "FROM TP_LETTURE "
            sSQL += "WHERE CODCONTATORE=" & strCodContatore
            'sSQL+="AND CODUTENTE=" & strCodUtente 
            sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL) "
            sSQL += "AND DATALETTURA < " & StringOperation.FormatString(m_lngDataGriglia)
            sSQL += " ORDER BY DATALETTURA DESC"
            'Log.Debug("VerificaDataGriglia::query2::" & sSQL)
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    lngDataLetturaPrecedente = StringOperation.FormatInt(myRow("DATALETTURA"))
                Next
            End If
            dvMyDati.Dispose()
            Select Case lngDataLetturaSuccessiva
                Case Is > 0 '  E' PRESENTE UNA LETTURA SUCCESSIVA
                    If lngDataLetturaPrecedente > 0 Then
                        If lngDataLettura > lngDataLetturaPrecedente And lngDataLettura < lngDataLetturaSuccessiva Then
                            VerificaDataGriglia = True
                        End If
                    Else
                        If lngDataLettura < lngDataLetturaSuccessiva Then
                            VerificaDataGriglia = True
                        End If
                    End If
                Case Is = 0 ' NON E' PRESENTE UNA LETTURA SUCCESSIVA
                    If lngDataLetturaPrecedente > 0 Then
                        If lngDataLettura > lngDataLetturaPrecedente Then
                            VerificaDataGriglia = True
                        End If
                    Else
                        VerificaDataGriglia = True
                    End If
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.VerificaDataGriglia.errore: ", ex)
        End Try
    End Function
#End Region
    'FINE VERIFICA DATA GRIGLIA
    '====================================================================
    'VERIFICA DELLA LETTURA 
#Region "VerificaLetturaGriglia"
    Public Function VerificaLetturaGriglia(ByVal m_lngDataGriglia As Long,
   ByVal m_lngLettura As Long,
   ByVal strLettura As String,
   ByVal strCodContatore As String,
   ByVal strCodUtente As String,
   ByRef blnConfermaConsumoNegativo As Boolean,
   ByRef blnLetturaErrata As Boolean) As Boolean

        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim lngLettura As Long
        Dim lngLetturaSuccessiva,
           lngLetturaPrecedente As Long
        Dim blnDEContatoriLetture As Boolean = False
        lngLettura = StringOperation.FormatInt(strLettura)

        lngLetturaSuccessiva = 0
        lngLetturaPrecedente = 0

        VerificaLetturaGriglia = False

        'Verifico se esiste una data successiva

        sSQL = ""
        sSQL = "SELECT TOP 1 LETTURA "
        sSQL += "FROM TP_LETTURE "
        sSQL += "WHERE CODCONTATORE=" & strCodContatore
        'sSQL+="AND CODUTENTE=" & strCodUtente 
        sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL) "
        sSQL += "AND DATALETTURA > " & StringOperation.FormatString(m_lngDataGriglia)
        sSQL += " ORDER BY DATALETTURA"


        dvMyDati = iDB.GetDataView(sSQL)
        Try
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    lngLetturaSuccessiva = StringOperation.FormatInt(myRow("LETTURA"))
                Next
            End If
            dvMyDati.Dispose()

            sSQL = ""
            sSQL = "SELECT TOP 1 LETTURA "
            sSQL += "FROM TP_LETTURE "
            sSQL += "WHERE CODCONTATORE=" & strCodContatore
            'sSQL+="AND CODUTENTE=" & strCodUtente 
            sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL) "
            sSQL += "AND DATALETTURA < " & StringOperation.FormatString(m_lngDataGriglia)
            sSQL += " ORDER BY DATALETTURA DESC"
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    lngLetturaPrecedente = StringOperation.FormatInt(myRow("LETTURA"))
                Next
            End If
            dvMyDati.Dispose()

            If lngLetturaPrecedente = 0 Then
                blnDEContatoriLetture = True
                Select Case lngLetturaSuccessiva
                    Case Is > 0
                        Select Case lngLettura
                            Case Is >= lngLetturaSuccessiva
                                blnLetturaErrata = True
                        End Select
                End Select
            End If

            If blnDEContatoriLetture = False Then
                Select Case lngLetturaSuccessiva

                    Case Is > 0    '  E' PRESENTE UNA LETTURA SUCCESSIVA
                        Select Case lngLettura
                            Case Is >= lngLetturaSuccessiva
                                blnLetturaErrata = True
                            Case Is = lngLetturaPrecedente
                                blnLetturaErrata = True
                            Case Is < lngLetturaPrecedente
                                'Verifica Giro Contatore
                                blnConfermaConsumoNegativo = True
                            Case Else
                                VerificaLetturaGriglia = True    ' Se è compresa
                        End Select

                    Case Is = 0    ' NON E' PRESENTE UNA LETTURA SUCCESSIVA

                        Select Case lngLettura

                            Case Is = lngLetturaPrecedente
                                blnLetturaErrata = True
                            Case Is < lngLetturaPrecedente
                                'Verifica Giro Contatore
                                blnConfermaConsumoNegativo = True
                            Case Is > lngLetturaPrecedente
                                VerificaLetturaGriglia = True
                        End Select
                End Select
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.VerificaLetturaGriglia.errore: ", ex)
        End Try
    End Function
#End Region
    'FINE VERIFICA LETTURA GRIGLIA
    '====================================================================
    '====================================================================
    'VERIFICA DELLA LETTURA 
#Region "VerificaLettura"

    Public Function VerificaLettura(ByVal strDataLettura As String,
    ByVal strLettura As String,
   ByVal strCodContatore As String,
   ByVal strCodUtente As String,
   ByRef blnLetturaErrata As Boolean, ByRef blnConsumoNegativo As Boolean) As Boolean

        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim lngLettura As Long
        Dim lngLetturaPrecedente As Long

        lngLettura = StringOperation.FormatInt(strLettura)

        lngLetturaPrecedente = 0
        VerificaLettura = False

        sSQL = ""
        sSQL = "SELECT TOP 1 LETTURA "
        sSQL += "FROM TP_LETTURE "
        sSQL += "WHERE "
        sSQL += "CODCONTATORE=" & strCodContatore 
        'sSQL+="AND CODUTENTE=" & strCodUtente 
        sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL) "
        sSQL += "AND DATALETTURA < " & StringOperation.FormatString(strDataLettura)
        sSQL += "ORDER BY DATALETTURA DESC"
        dvMyDati = iDB.GetDataView(sSQL)
        Try
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    lngLetturaPrecedente = StringOperation.FormatInt(myRow("LETTURA"))
                Next
            End If
            dvMyDati.Dispose()
            Select Case lngLettura
                Case Is < lngLetturaPrecedente
                    blnConsumoNegativo = True
                Case Is >= lngLetturaPrecedente
                    VerificaLettura = True
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.VerificaLettura.errore: ", ex)
        End Try
    End Function
#End Region
    'FINE VERIFICA LETTURA GRIGLIA
    '====================================================================

    'AGGIRONA I GIORNI DI CONSUMO
#Region "AggiornaGiorniDiConsumo"
    'Aggiorna i giorni di Consumo
    'Public Sub AggiornaGiorniDiConsumo(ByVal strDataGriglia As String,
    ' ByVal m_lngDataGriglia As Long,
    ' ByVal strCodContatore As String,
    ' ByVal strCodUtente As String,
    ' ByVal strCodLettura As String)

    '    Dim sSQL,
    '    strCondition,
    '    strTemp As String

    '    Dim dvMyDati as new dataview
    '    Dim lngDataLetturaSuccessiva,
    '    lngDataLetturaPrecedente,
    '    lngCodLetturaSuccessiva,

    '    lngGiorniDiConsumoSuccessivi,
    '    lngGiorniDiConsumoPrecedenti As Long
    '    lngDataLetturaSuccessiva = 0
    '    lngDataLetturaPrecedente = 0


    '    'Verifico se esiste una data successiva o piu di una data successiva

    '    sSQL = ""
    '    sSQL = "SELECT TOP 1 CODLETTURA,DATALETTURA "
    '    sSQL += "FROM TP_LETTURE "
    '    sSQL += "WHERE "
    '    sSQL += "CODCONTATORE=" & strCodContatore 
    '    'sSQL+="AND CODUTENTE=" & strCodUtente 
    '    sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL) "

    '    sSQL += "AND "

    '    strCondition = "DATALETTURA > " & stringoperation.FormatString(m_lngDataGriglia) 
    '    strCondition = strCondition & "ORDER BY DATALETTURA"

    '    strTemp = sSQL & strCondition
    '    dvMyDati = iDB.getdataview(strTemp)
    '    Try
    '        If dvMyDati.Read Then
    '            lngDataLetturaSuccessiva = stringoperation.FormatInt(myrow("DATALETTURA"))
    '            lngCodLetturaSuccessiva = stringoperation.FormatInt(myrow("CODLETTURA"))
    '            lngGiorniDiConsumoSuccessivi = DateDiff(DateInterval.Day, CDate(strDataGriglia), CDate(clsModDate.GiraDataFromDB(lngDataLetturaSuccessiva)))
    '        End If

    '        If Not dvMyDati.IsClosed Then
    '            dvmydati.dispose()
    '        End If

    '        strTemp = ""
    '        strCondition = ""
    '        strCondition = "DATALETTURA < " & stringoperation.FormatString(m_lngDataGriglia) 
    '        strCondition = strCondition & "ORDER BY DATALETTURA DESC"

    '        strTemp = sSQL & strCondition

    '        dvMyDati = iDB.getdataview(strTemp)

    '        If dvMyDati.Read Then
    '            lngDataLetturaPrecedente = stringoperation.FormatInt(myrow("DATALETTURA"))
    '            lngGiorniDiConsumoPrecedenti = DateDiff(DateInterval.Day, CDate(clsModDate.GiraDataFromDB(lngDataLetturaPrecedente)), CDate(strDataGriglia))
    '        End If

    '        If Not dvMyDati.IsClosed Then
    '            dvmydati.dispose()
    '        End If


    '        Select Case lngDataLetturaSuccessiva

    '            Case Is > 0 '  E' PRESENTE UNA LETTURA SUCCESSIVA
    '                'Due Update una precedente una successiva

    '                UpdateGiornidiConsumo(strCodContatore, strCodUtente, CStr(lngCodLetturaSuccessiva),
    '                   CStr(lngGiorniDiConsumoSuccessivi))


    '        End Select
    '        Select Case lngDataLetturaPrecedente

    '            Case Is > 0
    '                UpdateGiornidiConsumo(strCodContatore, strCodUtente, strCodLettura,
    '                CStr(lngGiorniDiConsumoPrecedenti))

    '        End Select
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.AggiornaGiorniDiConsumo.errore: ", ex)
    '    End Try
    'End Sub
#End Region
    'FINE AGGIORNA GIORNI DI CONSUMO
    '====================================================================
    'AGGIORNA GIORNI DI CONSUMO DOPP CANCELLAZIONE
    '#Region "AggiornaGiorniDiConsumoDelete"
    '    Public Sub AggiornaGiorniDiConsumoDelete(ByVal strDataGriglia As String, ByVal m_lngDataGriglia As Long, ByVal strCodContatore As String, ByVal strCodUtente As String, ByVal strCodLettura As String)
    '        Dim sSQL, strData, strCondition, strTemp As String
    '        Dim dvMyDati as new dataview
    '        Dim lngDataLetturaSuccessiva, lngDataLetturaPrecedente, lngGiorniDiConsumo, lngCodLetturaSuccessiva As Long

    '        strData = strDataGriglia
    '        strDataGriglia = clsModDate.GiraData(strDataGriglia)

    '        lngDataLetturaSuccessiva = 0
    '        lngDataLetturaPrecedente = 0

    '        'Verifico se esiste una data successiva o piu di una data successiva

    '        sSQL = ""
    '        sSQL = "SELECT TOP 1 CODLETTURA,DATALETTURA "
    '        sSQL += "FROM TP_LETTURE "
    '        sSQL += "WHERE "
    '        sSQL += "CODCONTATORE=" & strCodContatore 
    '        'sSQL+="AND CODUTENTE=" & strCodUtente 
    '        sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL) "

    '        sSQL += "AND "

    '        strCondition = "DATALETTURA > " & stringoperation.FormatString(strDataGriglia) 
    '        strCondition = strCondition & "ORDER BY DATALETTURA"

    '        strTemp = sSQL & strCondition
    '        dvMyDati = iDB.getdataview(strTemp)
    '        Try
    '            If dvMyDati.Read Then
    '                lngDataLetturaSuccessiva = stringoperation.formatint(myrow("DATALETTURA"))
    '                lngCodLetturaSuccessiva = stringoperation.formatint(myrow("CODLETTURA"))
    '            End If

    '            If Not dvMyDati.IsClosed Then
    '                dvmydati.dispose()
    '            End If


    '            strCondition = "DATALETTURA < " & stringoperation.FormatString(strDataGriglia) 
    '            strCondition = strCondition & "ORDER BY DATALETTURA DESC"
    '            strTemp = sSQL & strCondition

    '            dvMyDati = iDB.getdataview(strTemp)

    '            If dvMyDati.Read Then
    '                lngDataLetturaPrecedente = stringoperation.formatint(myrow("DATALETTURA"))
    '            End If

    '            If Not dvMyDati.IsClosed Then
    '                dvmydati.dispose()
    '            End If
    '            '
    '            Select Case lngDataLetturaSuccessiva
    '                Case Is > 0 '  E' PRESENTE UNA LETTURA SUCCESSIVA
    '                    If lngDataLetturaPrecedente = 0 Then    'Si tratta della prima lettura inserita
    '                        lngGiorniDiConsumo = 0
    '                    Else
    '                        'Due Update una precedente una successiva
    '                        lngGiorniDiConsumo = DateDiff(DateInterval.Day, CDate(clsModDate.GiraDataFromDB(lngDataLetturaPrecedente)),
    '                        CDate(clsModDate.GiraDataFromDB(lngDataLetturaSuccessiva)))
    '                    End If
    '                    UpdateGiornidiConsumo(strCodContatore, strCodUtente, CStr(lngCodLetturaSuccessiva), lngGiorniDiConsumo)
    '            End Select
    '        Catch ex As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.AggiornaGiorniDiConsumoDelete.errore: ", ex)
    '        End Try
    '    End Sub
    '#End Region
    'FINE AGGIORNA GIORNI DI CONSUMO DOPO CANCELLAZIONE
    '=========================================================================
    'AGGIORNA CONSUMO
    '=========================================================================
#Region "AggiornaConsumo"
    ' Public Sub AggiornaConsumo(ByVal strLetturaGriglia As String,
    'ByVal m_lngDataGriglia As Long,
    'ByVal strCodContatore As String,
    'ByVal strCodUtente As String,
    'ByVal strCodLettura As String)

    '     Dim sSQL, strCondition,
    '     strTemp As String
    '     Dim dvMyDati as new dataview

    '     Dim lngLetturaSuccessiva,
    '     lngLetturaPrecedente,
    '     lngCodLetturaSuccessiva,
    '             lngConsumoEffettivoSuccessivo,
    '     lngConsumoEffettivoPrecedente As Long


    '     'Verifico se esiste una data successiva

    '     sSQL = ""
    '     sSQL = "SELECT TOP 1 CODLETTURA,LETTURA,GIORNIDICONSUMO,DATALETTURA "
    '     sSQL += "FROM TP_LETTURE "
    '     sSQL += "WHERE "
    '     sSQL += "CODCONTATORE=" & strCodContatore 
    '     'sSQL+="AND CODUTENTE=" & strCodUtente 
    '     sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL) "

    '     sSQL += "AND "


    '     strCondition = "DATALETTURA > " & stringoperation.FormatString(m_lngDataGriglia) 
    '     strCondition = strCondition & "ORDER BY DATALETTURA "

    '     strTemp = sSQL & strCondition
    '     dvMyDati = iDB.getdataview(strTemp)
    '     Try
    '         If dvMyDati.Read Then

    '             lngLetturaSuccessiva = stringoperation.FormatInt(myrow("LETTURA"))
    '             lngCodLetturaSuccessiva = stringoperation.FormatInt(myrow("CODLETTURA"))
    '             lngConsumoEffettivoSuccessivo = stringoperation.FormatInt(myrow("LETTURA")) - stringoperation.FormatInt(strLetturaGriglia)

    '         End If

    '         dvmydati.dispose()


    '         strCondition = "DATALETTURA < " & stringoperation.FormatString(m_lngDataGriglia) 
    '         strCondition = strCondition & "ORDER BY DATALETTURA DESC"

    '         strTemp = sSQL & strCondition
    '         dvMyDati = iDB.getdataview(strTemp)

    '         If dvMyDati.Read Then
    '             lngLetturaPrecedente = stringoperation.FormatInt(myrow("LETTURA"))
    '             lngConsumoEffettivoPrecedente = stringoperation.FormatInt(strLetturaGriglia) - stringoperation.FormatInt(myrow("LETTURA"))
    '         End If


    '         dvmydati.dispose()


    '         Select Case lngCodLetturaSuccessiva

    '             Case Is > 0 '  E' PRESENTE UNA LETTURA SUCCESSIVA
    '                 'Due Update una precedente una successiva
    '                 UpdateConsumo(strCodContatore,
    '                   strCodUtente, CStr(lngCodLetturaSuccessiva), CStr(lngConsumoEffettivoSuccessivo))

    '         End Select

    '         UpdateConsumo(strCodContatore,
    '           strCodUtente, strCodLettura, CStr(lngConsumoEffettivoPrecedente))
    '     Catch ex As Exception
    '         Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.AggiornaConsumo.errore: ", ex)
    '     End Try
    ' End Sub
#End Region
    'FINE AGGIORNA CONSUMO
    '=========================================================================
    'AGGIORNA IL CONSUMO DOPO LA CANCELLAZIONE DI UN RECORD
    '#Region "AggiornaConsumoDelete"
    '    Public Sub AggiornaConsumoDelete(ByVal strLetturaGriglia As String, ByVal m_lngDataGriglia As String, ByVal strCodContatore As String, ByVal strCodUtente As String, ByVal strCodLettura As String)
    '        Dim sSQL, strCondition, strTemp As String
    '        Dim dvMyDati as new dataview
    '        Dim lngLetturaSuccessiva, lngLetturaPrecedente, lngCodLetturaSuccessiva, lngCodLetturaPrecedente, lngConsumo As Long
    '        Dim DataLetturaPrecedente As String
    '        lngLetturaSuccessiva = 0
    '        lngLetturaPrecedente = 0
    '        m_lngDataGriglia = clsModDate.GiraData(m_lngDataGriglia)
    '        'Verifico se esiste una data successiva

    '        sSQL = ""
    '        sSQL = "SELECT TOP 1 CODLETTURA,LETTURA,DATALETTURA "
    '        sSQL += "FROM TP_LETTURE "
    '        sSQL += "WHERE "
    '        sSQL += "CODCONTATORE=" & strCodContatore 
    '        'sSQL+="AND CODUTENTE=" & strCodUtente 
    '        sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL) "

    '        sSQL += "AND "

    '        strCondition = "DATALETTURA > " & stringoperation.FormatString(m_lngDataGriglia) 
    '        strCondition = strCondition & "ORDER BY DATALETTURA "

    '        strTemp = sSQL & strCondition
    '        dvMyDati = iDB.getdataview(strTemp)
    '        Try

    '            If dvMyDati.Read Then
    '                lngLetturaSuccessiva = stringoperation.formatint(myrow("LETTURA"))
    '                lngCodLetturaSuccessiva = stringoperation.formatint(myrow("CODLETTURA"))
    '            End If

    '            dvmydati.dispose()

    '            strCondition = "DATALETTURA < " & stringoperation.FormatString(m_lngDataGriglia)
    '            strCondition = strCondition & "ORDER BY DATALETTURA DESC"

    '            strTemp = sSQL & strCondition
    '            dvMyDati = iDB.getdataview(strTemp)

    '            If dvMyDati.Read Then
    '                lngLetturaPrecedente = stringoperation.formatint(myrow("LETTURA"))
    '                DataLetturaPrecedente = stringoperation.FormatString(myrow("DATALETTURA"))
    '                lngCodLetturaPrecedente = stringoperation.formatint(myrow("CODLETTURA"))
    '            End If

    '            If Not dvMyDati.IsClosed Then
    '                dvmydati.dispose()
    '            End If


    '            Select Case lngCodLetturaSuccessiva

    '                Case Is > 0 '  E' PRESENTE UNA LETTURA SUCCESSIVA

    '                    'Due Update una precedente una successiva
    '                    'Se non ci sono letture precedenti si tratta di prima lettura quindi il consumo e eguale a 0 
    '                    'Non viene inserita nessuna lettura di default ne dal DE dei contatatori ne da Iter Contratto

    '                    If lngLetturaPrecedente = 0 Then

    '                        lngConsumo = 0
    '                        lngLetturaPrecedente = 0
    '                        DataLetturaPrecedente = ""



    '                    Else
    '                        lngConsumo = lngLetturaSuccessiva - lngLetturaPrecedente



    '                    End If

    '                    UpdateConsumo(strCodContatore, strCodUtente, stringoperation.FormatString(lngCodLetturaSuccessiva),
    '                     stringoperation.FormatString(lngConsumo))

    '                    'Aggiornamento Data Lettura Precedente aggiornamento lettura precedente
    '                    sSQL = ""
    '                    sSQL = "UPDATE TP_LETTURE SET  "
    '                    sSQL += "DATALETTURAPRECEDENTE = " & stringoperation.FormatString(DataLetturaPrecedente) & ", "
    '                    sSQL += "LETTURAPRECEDENTE = " & lngLetturaPrecedente 
    '                    sSQL += "WHERE "
    '                    sSQL += "TP_LETTURE.CODLETTURA=" & lngCodLetturaSuccessiva 
    '                    sSQL += "AND TP_LETTURE.CODCONTATORE=" & strCodContatore
    '                    'sSQL+="AND TP_LETTURE.CODUTENTE=" & strCodUtente
    '                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
    '                        Throw New Exception("errore in::" & sSQL)
    '                    End If
    '            End Select
    '        Catch ex As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.AggiornaConsumoDelete.errore: ", ex)
    '        End Try
    '    End Sub
    '#End Region
    'FINE AGGIORNA IL CONSUMO DOPO LA CANCELLAZIONE DI UN RECORD
    '========================================================================
    'AGGIORNA CONSUMO TEORICO
#Region "AggiornaConsumoTeorico"
    'Public Sub AggiornaConsumoTeorico(ByVal strCodContatore As String,
    'ByVal strCodUtente As String)

    '    Dim sSQL As String
    '    Dim dvMyDati as new dataview
    '    Dim lngLetturaSuccessiva,
    '    lngLetturaPrecedente,
    '    lngCodLetturaSuccessiva As Long
    '    Dim dblRapportoCGG, dblMediaConsumo As Double


    '    'Verifico se esiste una data successiva

    '    Dim dblConsumoTeorico,
    '       dblResult As Double
    '    Dim lngRecordCount,
    '       lngConsumoTeorico As Long

    '    sSQL = "SELECT TOP 5 TP_LETTURE.*  FROM TP_LETTURE  "
    '    sSQL += "WHERE "
    '    sSQL += "CODCONTATORE=" & strCodContatore 
    '    'sSQL+="AND CODUTENTE=" & strCodUtente 
    '    sSQL += "AND (INCONGRUENTEFORZATO IS NULL  OR INCONGRUENTEFORZATO =0) "
    '    sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL) "

    '    sSQL += "AND PRIMALETTURA IS NULL "
    '    sSQL += "AND (TP_LETTURE.DATADIPASSAGGIO IS NULL OR TP_LETTURE.DATADIPASSAGGIO='') "
    '    sSQL += "ORDER BY DATALETTURA DESC"
    '    dvMyDati = iDB.getdataview(sSQL)
    '    Try
    '        While dvMyDati.Read
    '            dblRapportoCGG = stringoperation.FormatInt(myrow("CONSUMO")) / stringoperation.FormatInt(myrow("GIORNIDICONSUMO"))

    '            If stringoperation.FormatInt(myrow("GIORNIDICONSUMO")) = 0 Then
    '                'Giorni di Consumo =0 situazione anomala
    '                dblRapportoCGG = 0
    '            End If

    '            dblResult = dblResult + dblRapportoCGG
    '            lngRecordCount = lngRecordCount + 1
    '        End While

    '        dblMediaConsumo = dblResult / lngRecordCount

    '        dvmydati.dispose()
    '        dvMyDati = iDB.getdataview(sSQL)

    '        While dvMyDati.Read

    '            dblConsumoTeorico = dblMediaConsumo * stringoperation.FormatString(myrow("GIORNIDICONSUMO"))
    '            lngConsumoTeorico = ApprossimaNumero(dblConsumoTeorico)
    '            lngCodLetturaSuccessiva = stringoperation.FormatInt(myrow("CODLETTURA"))

    '            sSQL = ""
    '            sSQL = "UPDATE TP_LETTURE SET  "
    '            sSQL += "CONSUMOTEORICO = " & stringoperation.FormatString(lngConsumoTeorico) 
    '            sSQL += "WHERE "
    '            sSQL += "TP_LETTURE.CODLETTURA=" & stringoperation.FormatString(lngCodLetturaSuccessiva) 

    '            If iDB.ExecuteNonQuery(sSQL) <> 1 Then
    '                Throw New Exception("errore in::" & sSQL)
    '            End If
    '        End While

    '        dvmydati.dispose()
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.AggiornaConsumoTeorico.errore: ", ex)
    '    End Try

    'End Sub
#End Region
    'FINE AGGIORNA CONSUMO TEORICO
    '========================================================================
    'VERIFICA GIRO CONTATORE
#Region "VerificaGiroContatore"
    Public Function VerificaGiroContatore(ByVal lngLetturaPrec As Long, ByVal lngLettura As Long, ByVal strCodContatore As String, ByVal strCodUtente As String) As Long
        Dim lngLetturaPrecedente, lngTemp, lngConsumoEffettivo As Long

        Try
            lngConsumoEffettivo = lngLettura - lngLetturaPrec
            If lngConsumoEffettivo < 0 Then
                Dim myContatore As New objContatore
                myContatore = New GestContatori().GetDetailsContatori(strCodContatore, -1)
                lngConsumoEffettivo = myContatore.nFondoScala - lngLetturaPrecedente
                lngConsumoEffettivo = (lngLettura - 0) + lngConsumoEffettivo
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.VerificaGiroContatore.errore: ", ex)
            lngTemp = -1
        End Try
        Return lngTemp
    End Function
    'Public Function VerificaGiroContatore(ByVal lngLetturaPrec As Long, ByVal lngLettura As Long,
    'ByVal strCodContatore As String,
    'ByVal strCodUtente As String) As Long

    '    Dim dvMyDati as new dataview
    '    Dim lngValoreFondoScala,
    '     lngLetturaPrecedente,
    '     lngTemp,
    '     lngConsumoEffettivo As Long


    '    lngConsumoEffettivo = lngLettura - lngLetturaPrec
    '    Try
    '        If lngConsumoEffettivo < 0 Then

    '            'Verifico e considero  il Giro Contatore
    '            dvMyDati = iDB.getdataview(getValoreFondoScala(strCodContatore))

    '            If dvMyDati.Read() Then
    '                lngValoreFondoScala = stringoperation.formatint(myrow("VALOREFONDOSCALA"))
    '                lngConsumoEffettivo = lngValoreFondoScala - lngLetturaPrecedente
    '                lngConsumoEffettivo = (lngLettura - 0) + lngConsumoEffettivo
    '            End If

    '        End If

    '        dvmydati.dispose()

    '        VerificaGiroContatore = lngTemp

    '        Return VerificaGiroContatore
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.VerificaGiroContatore.errore: ", ex)
    '    End Try
    'End Function
#End Region
    'FINE VERIFICA GIRO CONTATORE
    '========================================================================

    '**********************************************************
    '* Funzione che ritorna i giorni che intercorrono tra due date passate è
    '* parametri
    '*le date sono passate come stringhe 
    '*la funzione effettuata una cionversione delle stringhe passate in date
    '*07/10/2003 Antonello Lo Bianco
    '**********************************************************

    Protected Function getGiorniDiConsumo(ByVal strDataPrecedente As String, ByVal strDataAttuale As String) As Long

        Dim lngGiorniDiConsumo As Long

        lngGiorniDiConsumo = 0
        lngGiorniDiConsumo = DateDiff(DateInterval.Day, CDate(strDataPrecedente), CDate(strDataAttuale))
        Return lngGiorniDiConsumo

    End Function
    'Public Function getValoreFondoScala(ByVal strCodContatore As String) As String
    '    Dim sSQL As String

    '    'sSQL = "SELECT TP_TIPOCONTATORE.VALOREFONDOSCALA "
    '    'sSQL += " FROM  TP_CONTATORI INNER JOIN "
    '    'sSQL += " TP_TIPOCONTATORE ON TP_CONTATORI.IDTIPOCONTATORE = TP_TIPOCONTATORE.IDTIPOCONTATORE "
    '    'sSQL += " WHERE TP_CONTATORI.CODCONTATORE = " & strCodContatore
    '    sSQL = "SELECT REPLICATE('9',TP_CONTATORI.CIFRECONTATORE) AS VALOREFONDOSCALA"
    '    sSQL += " FROM  TP_CONTATORI"
    '    sSQL += " WHERE (TP_CONTATORI.CODCONTATORE = " & strCodContatore & ")"

    '    Return sSQL
    'End Function

    'Protected Function getDataAttivazioneContatore(ByVal strCodContatore As String) As String

    '    Dim sSQL As String
    '    sSQL = "SELECT DATAATTIVAZIONE "
    '    sSQL += " FROM TP_CONTATORI "
    '    sSQL += " WHERE TP_CONTATORI.CODCONTATORE = " & strCodContatore
    '    Return sSQL

    'End Function

    Public Function GetTopLetture(DBType As String, StringConnection As String, myTop As Integer, IdContatore As Integer, DataLettura As String, SignData As String) As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            Using ctx As New DBModel(DBType, StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetTopLetture", "TOP", "IDCONTATORE", "DATALETTURA", "SIGNDATA")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("TOP", myTop) _
                        , ctx.GetParam("IDCONTATORE", IdContatore) _
                        , ctx.GetParam("DATALETTURA", DataLettura) _
                        , ctx.GetParam("SIGNDATA", SignData)
                    )
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.getTopLetture.errore: ", ex)
            dvMyDati = New DataView
        End Try
        Return dvMyDati
    End Function
    'Protected Function getTopFiveLetture(ByVal strCodContatore As String, ByVal strCodUtente As String, Optional ByVal strTypeOrder As String = "", Optional ByVal strQueryDelimiter As String = "") As String
    '    Dim sSQL As String

    '    sSQL = "SELECT TOP 5 TP_LETTURE.*  FROM TP_LETTURE  "
    '    sSQL += " WHERE "
    '    sSQL += " CODCONTATORE=" & strCodContatore 
    '    'sSQL += " AND CODUTENTE=" & strCodUtente 
    '    sSQL += " AND PRIMALETTURA IS NULL "
    '    sSQL += " AND (INCONGRUENTEFORZATO IS NULL  OR INCONGRUENTEFORZATO =0) "
    '    sSQL += " AND (STORICIZZATA=0 OR STORICIZZATA IS NULL) "

    '    sSQL += " AND (DATADIPASSAGGIO IS NULL OR DATADIPASSAGGIO='') "
    '    sSQL += " ORDER BY DATALETTURA "
    '    Try
    '        If Not strTypeOrder = String.Empty Then
    '            sSQL += strTypeOrder
    '        End If

    '        If Not strQueryDelimiter = String.Empty Then
    '            sSQL += strQueryDelimiter
    '        End If

    '        Return sSQL
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.getTopFiveLetture.errore: ", ex)
    '    End Try
    'End Function

    'Protected Function getTopOneLetture(ByVal IdLettura As Integer, ByVal strCodContatore As String, ByVal strCodUtente As String, Optional ByVal strTypeOrder As String = "", Optional ByVal strQueryDelimiter As String = "") As String
    '    dim sSQL as string

    '    sSQL = "SELECT TOP 1 TP_LETTURE.*"
    '    sSQL += " FROM TP_LETTURE "
    '    sSQL += " WHERE"
    '    'sSQL += " CODUTENTE=" & strCodUtente & " AND"
    '    sSQL += " (STORICIZZATA=0 OR STORICIZZATA IS NULL)"
    ' Try
    '    If IdLettura > -1 Then
    '        sSQL += " AND (CODLETTURA<>" & IdLettura & ")"
    '    End If
    '    sSQL += " AND (CODCONTATORE=" & strCodContatore & ")"
    '    sSQL += " ORDER BY DATALETTURA "

    '    If Not strTypeOrder = String.Empty Then
    '        sSQL+=strTypeOrder
    '    End If

    '    If Not strQueryDelimiter = String.Empty Then
    '        sSQL+=strQueryDelimiter
    '    End If

    '    Return sSQL
    'Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.getTopOneLetture.errore: ", ex)
    'End Try
    'End Function

    Protected Function ApprossimaNumero(ByVal dblNumber As Double) As Long
        ApprossimaNumero = System.Math.Ceiling(dblNumber)
    End Function

    'Protected Function CalcolaLetturaTeorica(ByVal lngLetturaPrecedente As Long, ByVal lngConsumoTeorico As Long, ByVal lngValoreFondoScala As Long) As Long

    '    CalcolaLetturaTeorica = lngLetturaPrecedente + lngConsumoTeorico

    '    If CalcolaLetturaTeorica > lngValoreFondoScala Then

    '        CalcolaLetturaTeorica = CalcolaLetturaTeorica - lngValoreFondoScala

    '    End If

    'End Function
    'UPDATE GIORNI DI CONSUMO
    '=========================================================================
#Region "UpdateGiornidiConsumo"
    Protected Sub UpdateGiornidiConsumo(ByVal strCodContatore As String,
   ByVal strCodUtente As String, ByVal strCodLettura As String,
   ByVal strGiorniDiConsumo As String)
        Dim sSQL As String
        Try
            sSQL = ""
            sSQL = "UPDATE TP_LETTURE SET  "
            sSQL += "GIORNIDICONSUMO = " & strGiorniDiConsumo 
            sSQL += "WHERE "
            sSQL += "TP_LETTURE.CODLETTURA=" & strCodLettura 
            sSQL += "AND TP_LETTURE.CODCONTATORE=" & strCodContatore
            'sSQL+="AND TP_LETTURE.CODUTENTE=" & strCodUtente
            If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                Throw New Exception("errore in::" & sSQL)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.UpdateGiornidiConsumo.errore: ", ex)
        End Try
    End Sub
#End Region
    'FINE UPDATE GIORNI DI CONSUMO
    '=========================================================================
    'UPDATE  CONSUMO
    '=========================================================================
#Region "UpdateConsumo"
    Protected Sub UpdateConsumo(ByVal strCodContatore As String,
   ByVal strCodUtente As String, ByVal strCodLettura As String,
   ByVal strGiorniDiConsumo As String)
        Dim sSQL As String
        Try
            sSQL = ""
            sSQL = "UPDATE TP_LETTURE SET  "
            sSQL += "CONSUMO = " & strGiorniDiConsumo 
            sSQL += "WHERE "
            sSQL += "TP_LETTURE.CODLETTURA=" & strCodLettura 
            sSQL += "AND TP_LETTURE.CODCONTATORE=" & strCodContatore
            'sSQL+="AND TP_LETTURE.CODUTENTE=" & strCodUtente
            If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                Throw New Exception("errore in::" & sSQL)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.clsLetture.UpdateConsumo.errore: ", ex)
        End Try
    End Sub
#End Region
    'FINE UPDATE CONSUMO
    '=========================================================================

    'Public Function GetFondoScala(ByVal sIDContatore As String) As Integer
    '    Dim drMyDati as new dataview
    '    Dim nMyFondoScala As Integer = 0

    '    Try
    '        drMyDati = iDB.getdataview(getValoreFondoScala(sIDContatore))
    '        If drMyDati.Read Then
    '            If Not IsDBNull(drMyDati.Item("valorefondoscala")) Then
    '                nMyFondoScala = CInt(drMyDati.Item("valorefondoscala"))
    '            End If
    '        End If
    '        Return nMyFondoScala
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.clsLetture.GetFondoScala.errore: ", Err)
    '        Return 0
    '    Finally
    '        drMyDati.Close()
    '    End Try
    'End Function
End Class
