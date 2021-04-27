Imports System.Data.SqlClient
Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility

Public Class ClsRibaltaVar
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsRibaltaVar))
    Private oReplace As New ClsGenerale.Generale
    Private iDB As New DBAccess.getDBobject
    Public Enum sTypeRicerca
        VARFATT_DAGESTIRE = 1
        VARFATT_GESTITE = 2
    End Enum

    'Public Function GetRicercaVariazioni(ByVal sIDEnte As String, ByVal sTypeRicerca As Integer, ByVal WFSessione As OPENUtility.CreateSessione, Optional ByVal nIDContatore As Integer = -1) As objRicercaVariazione()
    '    dim sSQL as string
    '    Dim dvMyDati As SqlClient.SqlDataReader
    '    Dim oVariazione As objRicercaVariazione
    '    Dim oListVariazioni() As objRicercaVariazione
    '    Dim nList As Integer = -1

    '    Try
    '        sSQL = "SELECT *"
    '        sSQL += " FROM OPENgov_ELENCO_VARIAZIONI"
    '        sSQL += " WHERE (OPENgov_ELENCO_VARIAZIONI.CODENTE='" & sIDEnte & "')"
    '        Select Case sTypeRicerca
    '            Case ClsRibaltaVar.sTypeRicerca.VARFATT_DAGESTIRE
    '                sSQL += " AND (OPENgov_ELENCO_VARIAZIONI.DATA_VARIAZIONE IS NULL)"
    '            Case Else
    '                sSQL += " AND (NOT OPENgov_ELENCO_VARIAZIONI.DATA_VARIAZIONE IS NULL)"
    '        End Select
    '        If nIDContatore <> -1 Then
    '            sSQL += " AND (OPENgov_ELENCO_VARIAZIONI.CODCONTATORE=" & nIDContatore & ")"
    '        End If
    '        sSQL += " ORDER BY OPENgov_ELENCO_VARIAZIONI.CODENTE, OPENgov_ELENCO_VARIAZIONI.PERIODO, OPENgov_ELENCO_VARIAZIONI.NOMINATIVOINT, OPENgov_ELENCO_VARIAZIONI.NOMINATIVOUT, OPENgov_ELENCO_VARIAZIONI.MATRICOLA"
    '        'eseguo la query
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While dvMyDati.Read
    '            oVariazione = New objRicercaVariazione
    '            oVariazione.nIdVariazione = StringOperation.FormatInt(myrow("idvariazione"))
    '            oVariazione.sIdEnte = StringOperation.Formatstring(myrow("codente"))
    '            oVariazione.sPeriodo = StringOperation.Formatstring(myrow("periodo"))
    '            oVariazione.sNomeIntestatario = StringOperation.Formatstring(myrow("nominativoint"))
    '            oVariazione.sNomeUtente = StringOperation.Formatstring(myrow("nominativout"))
    '            oVariazione.sMatricola = StringOperation.Formatstring(myrow("matricola"))
    '            oVariazione.sVarContatore = StringOperation.Formatstring(myrow("varcontatore"))
    '            oVariazione.sVarLettura = StringOperation.Formatstring(myrow("varlettura"))
    '            If Not IsDBNull(dvMyDati("data_inserimento")) Then
    '                oVariazione.tDataInserimento = StringOperation.Formatdatetime(myrow("data_inserimento"))
    '            End If
    '            If Not IsDBNull(dvMyDati("data_variazione")) Then
    '                oVariazione.tDataVariazione = StringOperation.Formatdatetime(myrow("data_variazione"))
    '            End If
    '            If Not IsDBNull(dvMyDati("azione")) Then
    '                oVariazione.sAzione = StringOperation.Formatstring(myrow("azione"))
    '            End If
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListVariazioni(nList)
    '            oListVariazioni(nList) = oVariazione
    '        Loop

    '        Return oListVariazioni
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.GetRicercaVariazioni.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsFatture::GetRicercaVariazioni::" & Err.Message & " SQL: " & sSQL)
    '        
    '        Return Nothing
    '    Finally
    '        dvMyDati.Close()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIDEnte"></param>
    ''' <param name="sTypeRicerca"></param>
    ''' <param name="nIDContatore"></param>
    ''' <param name="nIdPeriodo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetRicercaVariazioni(ByVal sIDEnte As String, ByVal sTypeRicerca As Integer, ByVal nIDContatore As Integer, nIdPeriodo As Integer) As objRicercaVariazione()
        Dim oVariazione As objRicercaVariazione
        Dim oListVariazioni() As objRicercaVariazione = Nothing
        Dim nList As Integer = -1
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetElencoVariazioni", "IDENTE", "TYPERICERCA", "IDCONTATORE", "IDPERIODO")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIDEnte) _
                            , ctx.GetParam("TYPERICERCA", sTypeRicerca) _
                            , ctx.GetParam("IDCONTATORE", nIDContatore) _
                            , ctx.GetParam("IDPERIODO", nIdPeriodo)
                        )
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            oVariazione = New objRicercaVariazione
                            oVariazione.nIdVariazione = StringOperation.FormatInt(myRow("idvariazione"))
                            oVariazione.sIdEnte = StringOperation.FormatString(myRow("codente"))
                            oVariazione.sPeriodo = StringOperation.FormatString(myRow("periodo"))
                            oVariazione.sNomeIntestatario = StringOperation.FormatString(myRow("nominativoint"))
                            oVariazione.sNomeUtente = StringOperation.FormatString(myRow("nominativout"))
                            oVariazione.sMatricola = StringOperation.FormatString(myRow("matricola"))
                            oVariazione.sVarContatore = StringOperation.FormatString(myRow("varcontatore"))
                            oVariazione.sVarLettura = StringOperation.FormatString(myRow("varlettura"))
                            oVariazione.tDataInserimento = StringOperation.FormatDateTime(myRow("data_inserimento"))
                            oVariazione.tDataVariazione = StringOperation.FormatDateTime(myRow("data_variazione"))
                            oVariazione.sAzione = StringOperation.FormatString(myRow("azione"))
                            'ridimensiono l'array
                            nList += 1
                            ReDim Preserve oListVariazioni(nList)
                            oListVariazioni(nList) = oVariazione
                        Next
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.GetRicercaVariazioni.errore: ", ex)
                Finally
                    dvMyDati.Dispose()
                End Try
                ctx.Dispose()
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.GetRicercaVariazioni.errore: ", Err)
            Return Nothing
        End Try
        Return oListVariazioni
    End Function
    'Public Function GetRicercaVariazioni(ByVal sIDEnte As String, ByVal sTypeRicerca As Integer, Optional ByVal nIDContatore As Integer = -1) As objRicercaVariazione()
    '    Dim sSQL As String
    '    Dim dvMyDati As SqlClient.SqlDataReader
    '    Dim oVariazione As objRicercaVariazione
    '    Dim oListVariazioni() As objRicercaVariazione
    '    Dim nList As Integer = -1

    '    Try
    '        sSQL = "SELECT *"
    '        sSQL += " FROM OPENgov_ELENCO_VARIAZIONI"
    '        sSQL += " WHERE (OPENgov_ELENCO_VARIAZIONI.CODENTE='" & sIDEnte & "')"
    '        Select Case sTypeRicerca
    '            Case ClsRibaltaVar.sTypeRicerca.VARFATT_DAGESTIRE
    '                sSQL += " AND (OPENgov_ELENCO_VARIAZIONI.DATA_VARIAZIONE IS NULL)"
    '            Case Else
    '                sSQL += " AND (NOT OPENgov_ELENCO_VARIAZIONI.DATA_VARIAZIONE IS NULL)"
    '        End Select
    '        If nIDContatore <> -1 Then
    '            sSQL += " AND (OPENgov_ELENCO_VARIAZIONI.CODCONTATORE=" & nIDContatore & ")"
    '        End If
    '        sSQL += " ORDER BY OPENgov_ELENCO_VARIAZIONI.CODENTE, OPENgov_ELENCO_VARIAZIONI.PERIODO, OPENgov_ELENCO_VARIAZIONI.NOMINATIVOINT, OPENgov_ELENCO_VARIAZIONI.NOMINATIVOUT, OPENgov_ELENCO_VARIAZIONI.MATRICOLA"
    '        'eseguo la query
    '        dvMyDati = iDB.getdataview(sSQL)
    '        Do While dvMyDati.Read
    '            oVariazione = New objRicercaVariazione
    '            oVariazione.nIdVariazione = StringOperation.FormatInt(myrow("idvariazione"))
    '            oVariazione.sIdEnte = StringOperation.Formatstring(myrow("codente"))
    '            oVariazione.sPeriodo = StringOperation.Formatstring(myrow("periodo"))
    '            oVariazione.sNomeIntestatario = StringOperation.Formatstring(myrow("nominativoint"))
    '            oVariazione.sNomeUtente = StringOperation.Formatstring(myrow("nominativout"))
    '            oVariazione.sMatricola = StringOperation.Formatstring(myrow("matricola"))
    '            oVariazione.sVarContatore = StringOperation.Formatstring(myrow("varcontatore"))
    '            oVariazione.sVarLettura = StringOperation.Formatstring(myrow("varlettura"))
    '            If Not IsDBNull(dvMyDati("data_inserimento")) Then
    '                oVariazione.tDataInserimento = StringOperation.Formatdatetime(myrow("data_inserimento"))
    '            End If
    '            If Not IsDBNull(dvMyDati("data_variazione")) Then
    '                oVariazione.tDataVariazione = StringOperation.Formatdatetime(myrow("data_variazione"))
    '            End If
    '            If Not IsDBNull(dvMyDati("azione")) Then
    '                oVariazione.sAzione = StringOperation.Formatstring(myrow("azione"))
    '            End If
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListVariazioni(nList)
    '            oListVariazioni(nList) = oVariazione
    '        Loop

    '        Return oListVariazioni
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.GetRicercaVariazioni.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsFatture::GetRicercaVariazioni::" & Err.Message & " SQL: " & sSQL)
    '        Return Nothing
    '    Finally
    '        dvMyDati.Close()
    '    End Try
    'End Function

    ' Public Function GetVariazione(ByVal nIDVariazione As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As objVariazioneFattura
    '     dim sSQL as string
    '     Dim dvMyDati As SqlClient.SqlDataReader
    '     Dim oVariazione As objVariazioneFattura
    '     Dim oMyFattura As New ObjFattura

    '     Try
    '         sSQL = "SELECT *"
    'sSQL += " FROM OPENGOV_VARIAZIONIDARIBALTARE"
    'sSQL += " WHERE (IDVARIAZIONE=" & nIDVariazione & ")"
    '         'eseguo la query
    '         dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '         Do While dvMyDati.Read
    '             oVariazione = New objVariazioneFattura
    '             oVariazione.Id = StringOperation.FormatInt(myrow("idvariazione"))
    '             oVariazione.sIdEnte = StringOperation.Formatstring(myrow("codente"))
    '             oMyFattura.Id = StringOperation.FormatInt(myrow("idfatturanota"))
    '             oVariazione.nIdContatore = StringOperation.FormatInt(myrow("codcontatore"))
    '             If Not IsDBNull(dvMyDati("codlettura")) Then
    '                 oVariazione.nIdLettura = StringOperation.FormatInt(myrow("codlettura"))
    '             End If
    '             oVariazione.nIdIntestatario = StringOperation.FormatInt(myrow("cod_intestatario"))
    '             oVariazione.nIdUtente = StringOperation.FormatInt(myrow("cod_utente"))
    '             oVariazione.nUtenze = StringOperation.FormatInt(myrow("numeroutenze"))
    '             oVariazione.nTipoUtenza = StringOperation.FormatInt(myrow("idtipoutenza"))
    '             oVariazione.nTipoContatore = StringOperation.FormatInt(myrow("idtipocontatore"))
    '             oVariazione.nCodFognatura = StringOperation.FormatInt(myrow("codfognatura"))
    '             oVariazione.nCodDepurazione = StringOperation.FormatInt(myrow("coddepurazione"))
    '             oVariazione.bEsenteFognatura = StringOperation.Formatbool(myrow("esentefognatura"))
    '             oVariazione.bEsenteDepurazione = StringOperation.Formatbool(myrow("esentefognatura"))
    '             oVariazione.bEsenteAcqua = StringOperation.Formatbool(myrow("esenteacqua"))
    '             If Not IsDBNull(dvMyDati("nolounatantum")) Then
    '                 oVariazione.bNoloUnaTantum = StringOperation.Formatbool(myrow("nolounatantum"))
    '	End If
    '	'*** 20130110 - devo prelevare anche la data lettura orginale perchè posso variarla di conseguenza non troverei più la lettura da aggiornare ***
    '	oVariazione.tDataLetturaAttOrg = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("dataletturaorg")))
    '	oVariazione.tDataLetturaAtt = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("datalettura")))
    '	oVariazione.nLetturaAtt = StringOperation.FormatInt(myrow("lettura"))
    '	oVariazione.tDataLetturaPrec = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("dataletturaprecedente")))
    '	oVariazione.nLetturaPrec = StringOperation.FormatInt(myrow("letturaprecedente"))
    '	oVariazione.nConsumo = StringOperation.FormatInt(myrow("consumo"))
    '	oVariazione.nGiorni = StringOperation.FormatInt(myrow("giornidiconsumo"))
    '	If Not IsDBNull(dvMyDati("data_inserimento")) Then
    '		oVariazione.tDataInserimento = StringOperation.Formatdatetime(myrow("data_inserimento"))
    '	End If
    '	If Not IsDBNull(dvMyDati("data_variazione")) Then
    '		oVariazione.tDataVariazione = StringOperation.Formatdatetime(myrow("data_variazione"))
    '	End If
    '	If Not IsDBNull(dvMyDati("azione")) Then
    '		oVariazione.sAzione = StringOperation.Formatstring(myrow("azione"))
    '	End If
    '	'*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '	If Not IsDBNull(dvMyDati("ESENTEACQUAQF")) Then
    '		oVariazione.bEsenteACQUAQF = StringOperation.Formatbool(myrow("ESENTEACQUAQF"))
    '	End If
    '	If Not IsDBNull(dvMyDati("ESENTEDEPURAZIONEQF")) Then
    '		oVariazione.bEsenteDepQF = StringOperation.Formatbool(myrow("ESENTEDEPURAZIONEQF"))
    '	End If
    '	If Not IsDBNull(dvMyDati("ESENTEFOGNATURAQF")) Then
    '		oVariazione.bEsenteFogQF = StringOperation.Formatbool(myrow("ESENTEFOGNATURAQF"))
    '	End If
    '	'*** ***
    '	oVariazione.oFattura = oMyFattura
    'Loop

    '         Return oVariazione
    '     Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.GetVariazione.errore: ", Err)
    '         Log.Debug("Si è verificato un errore in ClsFatture::GetVariazione::" & Err.Message & " SQL: " & sSQL)
    '         

    '         Return Nothing
    '     Finally
    '         dvMyDati.Close()
    '     End Try
    ' End Function

    Public Function GetVariazione(ByVal nIDVariazione As Integer) As objVariazioneFattura
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim oVariazione As New objVariazioneFattura
        Dim oMyFattura As New ObjFattura

        Try
            sSQL = "SELECT *"
            sSQL += " FROM OPENGOV_VARIAZIONIDARIBALTARE"
            sSQL += " WHERE (IDVARIAZIONE=" & nIDVariazione & ")"
            'eseguo la query
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    oVariazione = New objVariazioneFattura
                    oVariazione.Id = StringOperation.FormatInt(myrow("idvariazione"))
                    oVariazione.sIdEnte = StringOperation.FormatString(myrow("codente"))
                    oMyFattura.Id = StringOperation.FormatInt(myrow("idfatturanota"))
                    oVariazione.nIdContatore = StringOperation.FormatInt(myrow("codcontatore"))
                    oVariazione.nIdLettura = StringOperation.FormatInt(myrow("codlettura"))
                    oVariazione.nIdIntestatario = StringOperation.FormatInt(myrow("cod_intestatario"))
                    oVariazione.nIdUtente = StringOperation.FormatInt(myrow("cod_utente"))
                    oVariazione.nUtenze = StringOperation.FormatInt(myrow("numeroutenze"))
                    oVariazione.nTipoUtenza = StringOperation.FormatInt(myrow("idtipoutenza"))
                    oVariazione.nTipoContatore = StringOperation.FormatInt(myrow("idtipocontatore"))
                    oVariazione.nCodFognatura = StringOperation.FormatInt(myrow("codfognatura"))
                    oVariazione.nCodDepurazione = StringOperation.FormatInt(myrow("coddepurazione"))
                    oVariazione.bEsenteFognatura = StringOperation.FormatBool(myrow("esentefognatura"))
                    oVariazione.bEsenteDepurazione = StringOperation.FormatBool(myrow("esentefognatura"))
                    oVariazione.bEsenteAcqua = StringOperation.FormatBool(myrow("esenteacqua"))
                    oVariazione.bNoloUnaTantum = StringOperation.FormatBool(myrow("nolounatantum"))
                    '*** 20130110 - devo prelevare anche la data lettura orginale perchè posso variarla di conseguenza non troverei più la lettura da aggiornare ***
                    oVariazione.tDataLetturaAttOrg = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("dataletturaorg")))
                    oVariazione.tDataLetturaAtt = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("datalettura")))
                    oVariazione.nLetturaAtt = StringOperation.FormatInt(myrow("lettura"))
                    oVariazione.tDataLetturaPrec = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("dataletturaprecedente")))
                    oVariazione.nLetturaPrec = StringOperation.FormatInt(myrow("letturaprecedente"))
                    oVariazione.nConsumo = StringOperation.FormatInt(myrow("consumo"))
                    oVariazione.nGiorni = StringOperation.FormatInt(myrow("giornidiconsumo"))
                    oVariazione.tDataInserimento = StringOperation.FormatDateTime(myrow("data_inserimento"))
                    oVariazione.tDataVariazione = StringOperation.FormatDateTime(myrow("data_variazione"))
                    oVariazione.sAzione = StringOperation.FormatString(myrow("azione"))
                    '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
                    oVariazione.bEsenteAcquaQF = StringOperation.FormatBool(myrow("ESENTEACQUAQF"))
                    oVariazione.bEsenteDepQF = StringOperation.FormatBool(myrow("ESENTEDEPURAZIONEQF"))
                    oVariazione.bEsenteFogQF = StringOperation.FormatBool(myrow("ESENTEFOGNATURAQF"))
                    '*** ***
                    oVariazione.oFattura = oMyFattura
                Next
            End If

            Return oVariazione
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.GetVariazione.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function

    'Public Function SetVariazione(ByVal oMyVariazione As objVariazioneFattura, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    dim sSQL as string

    '    Try
    '        sSQL = "UPDATE TP_VARIAZIONIFATTURE SET DATA_VARIAZIONE='" & oReplace.ReplaceDataForDB(oMyVariazione.tDataVariazione) & "'"
    '        sSQL += " WHERE (TP_VARIAZIONIFATTURE.IDVARIAZIONE=" & oMyVariazione.Id & ")"
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '            Return 0
    '        End If

    '        Return 1
    '    Catch Err As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.SetVariazione.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsFatture::SetVariazione::" & Err.Message & " SQL: " & sSQL)
    '        
    '        Return -1
    '    End Try
    'End Function

    Public Function SetVariazione(ByVal oMyVariazione As objVariazioneFattura) As Integer
        Dim sSQL As String = ""

        Try
            sSQL = "UPDATE TP_VARIAZIONIFATTURE SET DATA_VARIAZIONE='" & oReplace.ReplaceDataForDB(oMyVariazione.tDataVariazione) & "'"
            sSQL += " WHERE (TP_VARIAZIONIFATTURE.IDVARIAZIONE=" & oMyVariazione.Id & ")"
            'eseguo la query
            If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                Return 0
            End If

            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.SetVariazione.errore: ", Err)
            Log.Debug("Si è verificato un errore in ClsFatture::SetVariazione::" & Err.Message & " SQL: " & sSQL)

            Return -1
        End Try
    End Function

    ' Public Function SetVariazioni(ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '     dim sSQL as string

    '     Try
    '         sSQL = "INSERT INTO TP_VARIAZIONIFATTURE(CODENTE,IDFATTURANOTA,CODCONTATORE,CODLETTURA,COD_INTESTATARIO,COD_UTENTE,"
    'sSQL += "NUMEROUTENZE,IDTIPOUTENZA,IDTIPOCONTATORE,CODFOGNATURA,CODDEPURAZIONE,ESENTEFOGNATURA,ESENTEDEPURAZIONE,ESENTEACQUA"
    ''*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    'sSQL += ",ESENTEACQUAQF,ESENTEFOGNATURAQF,ESENTEDEPURAZIONEQF,"
    '         sSQL += "DATALETTURA,LETTURA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,CONSUMO,GIORNIDICONSUMO,	DATA_INSERIMENTO)"
    '         sSQL += " SELECT TP_FATTURE_NOTE.IDENTE, DOCORG.IDFATTURANOTA, TP_LETTURE.CODCONTATORE, TP_FATTURE_NOTE.IDLETTURA, TP_FATTURE_NOTE.COD_INTESTATARIO, TP_FATTURE_NOTE.COD_UTENTE,"
    'sSQL += " TP_FATTURE_NOTE.NUTENZE, TP_FATTURE_NOTE.ID_TIPOLOGIA_UTENZA, TP_FATTURE_NOTE.ID_TIPO_CONTATORE,TP_FATTURE_NOTE.CODFOGNATURA,TP_FATTURE_NOTE.CODDEPURAZIONE,TP_FATTURE_NOTE.ESENTEFOGNATURA,TP_FATTURE_NOTE.ESENTEDEPURAZIONE,TP_FATTURE_NOTE.ESENTEACQUA"
    ''*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    'sSQL += ",TP_FATTURE_NOTE.ESENTEACQUAQF,TP_FATTURE_NOTE.ESENTEFOGNATURAQF,TP_FATTURE_NOTE.ESENTEDEPURAZIONEQF,"
    '         sSQL += " TP_FATTURE_NOTE.DATA_LETTURA_ATT,TP_FATTURE_NOTE.LETTURA_ATT,TP_FATTURE_NOTE.DATA_LETTURA_PREC,TP_FATTURE_NOTE.LETTURA_PREC,TP_FATTURE_NOTE.CONSUMO, TP_FATTURE_NOTE.GIORNI, GETDATE() - 1"
    '         sSQL += " FROM TP_FATTURE_NOTE"
    '         sSQL += " INNER JOIN TR_LETTURE_FATTURE ON TP_FATTURE_NOTE.IDFATTURANOTA=TR_LETTURE_FATTURE.IDFATTURA"
    '         sSQL += " INNER JOIN TP_LETTURE ON TR_LETTURE_FATTURE.IDLETTURA=TP_LETTURE.CODLETTURA "
    '         sSQL += " INNER JOIN ("
    '         sSQL += " 	SELECT IDFATTURANOTA, DATA_FATTURA+NUMERO_FATTURA AS RIF"
    '         sSQL += " 	FROM TP_FATTURE_NOTE"
    '         sSQL += " 	WHERE DATA_FATTURA+NUMERO_FATTURA IN ("
    '         sSQL += " 		SELECT DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO"
    '         sSQL += " 		FROM TP_FATTURE_NOTE"
    '         sSQL += " 		WHERE NOT DATA_FATTURA_RIFERIMENTO IS NULL AND DATA_FATTURA_RIFERIMENTO<>''"
    '         sSQL += " 	)"
    '         sSQL += " )DOCORG ON TP_FATTURE_NOTE.DATA_FATTURA_RIFERIMENTO+TP_FATTURE_NOTE.NUMERO_FATTURA_RIFERIMENTO=DOCORG.RIF"
    '         sSQL += " WHERE NOT DATA_FATTURA_RIFERIMENTO IS NULL AND TIPO_DOCUMENTO='F'"
    '         sSQL += " AND DOCORG.IDFATTURANOTA NOT IN ("
    '         sSQL += " 	SELECT IDFATTURANOTA"
    '         sSQL += " 	FROM TP_VARIAZIONIFATTURE"
    '         sSQL += " )"
    '         'eseguo la query
    '         WFSessione.oSession.oAppDB.Execute(sSQL)
    '         Return 1
    '     Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.SetVariazioni.errore: ", ex)
    '         Log.Debug("Si è verificato un errore in ClsFatture::SetVariazioni::" & Err.Message & " SQL: " & sSQL)
    '         Return -1
    '     End Try
    ' End Function
    Public Function SetVariazioni() As Integer
        Dim sSQL As String

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_SetVariazioni")
                ctx.ExecuteNonQuery(sSQL)
                ctx.Dispose()
            End Using
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.SetVariazioni.errore: ", Err)
            Return -1
        End Try
    End Function
    'Public Function SetVariazioni() As Integer
    '    Dim sSQL As String

    '    Try
    '        sSQL = "INSERT INTO TP_VARIAZIONIFATTURE(CODENTE,IDFATTURANOTA,CODCONTATORE,CODLETTURA,COD_INTESTATARIO,COD_UTENTE,"
    '        sSQL += "NUMEROUTENZE,IDTIPOUTENZA,IDTIPOCONTATORE,CODFOGNATURA,CODDEPURAZIONE,ESENTEFOGNATURA,ESENTEDEPURAZIONE,ESENTEACQUA"
    '        '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '        sSQL += ",ESENTEACQUAQF,ESENTEFOGNATURAQF,ESENTEDEPURAZIONEQF,"
    '        sSQL += "DATALETTURA,LETTURA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,CONSUMO,GIORNIDICONSUMO,	DATA_INSERIMENTO)"
    '        sSQL += " SELECT TP_FATTURE_NOTE.IDENTE, DOCORG.IDFATTURANOTA, TP_LETTURE.CODCONTATORE, TP_FATTURE_NOTE.IDLETTURA, TP_FATTURE_NOTE.COD_INTESTATARIO, TP_FATTURE_NOTE.COD_UTENTE,"
    '        sSQL += " TP_FATTURE_NOTE.NUTENZE, TP_FATTURE_NOTE.ID_TIPOLOGIA_UTENZA, TP_FATTURE_NOTE.ID_TIPO_CONTATORE,TP_FATTURE_NOTE.CODFOGNATURA,TP_FATTURE_NOTE.CODDEPURAZIONE,TP_FATTURE_NOTE.ESENTEFOGNATURA,TP_FATTURE_NOTE.ESENTEDEPURAZIONE,TP_FATTURE_NOTE.ESENTEACQUA"
    '        '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '        sSQL += ",TP_FATTURE_NOTE.ESENTEACQUAQF,TP_FATTURE_NOTE.ESENTEFOGNATURAQF,TP_FATTURE_NOTE.ESENTEDEPURAZIONEQF,"
    '        sSQL += " TP_FATTURE_NOTE.DATA_LETTURA_ATT,TP_FATTURE_NOTE.LETTURA_ATT,TP_FATTURE_NOTE.DATA_LETTURA_PREC,TP_FATTURE_NOTE.LETTURA_PREC,TP_FATTURE_NOTE.CONSUMO, TP_FATTURE_NOTE.GIORNI, GETDATE() - 1"
    '        sSQL += " FROM TP_FATTURE_NOTE"
    '        sSQL += " INNER JOIN TR_LETTURE_FATTURE ON TP_FATTURE_NOTE.IDFATTURANOTA=TR_LETTURE_FATTURE.IDFATTURA"
    '        sSQL += " INNER JOIN TP_LETTURE ON TR_LETTURE_FATTURE.IDLETTURA=TP_LETTURE.CODLETTURA "
    '        sSQL += " INNER JOIN ("
    '        sSQL += " 	SELECT IDFATTURANOTA, DATA_FATTURA+NUMERO_FATTURA AS RIF"
    '        sSQL += " 	FROM TP_FATTURE_NOTE"
    '        sSQL += " 	WHERE IDENTE+'|'+DATA_FATTURA+'|'+NUMERO_FATTURA IN ("
    '        sSQL += " 		SELECT IDENTE+'|'+DATA_FATTURA_RIFERIMENTO+'|'+NUMERO_FATTURA_RIFERIMENTO"
    '        sSQL += " 		FROM TP_FATTURE_NOTE"
    '        sSQL += " 		WHERE NOT DATA_FATTURA_RIFERIMENTO IS NULL AND DATA_FATTURA_RIFERIMENTO<>''"
    '        sSQL += " 	)"
    '        sSQL += " )DOCORG ON TP_FATTURE_NOTE.DATA_FATTURA_RIFERIMENTO+TP_FATTURE_NOTE.NUMERO_FATTURA_RIFERIMENTO=DOCORG.RIF"
    '        sSQL += " WHERE NOT DATA_FATTURA_RIFERIMENTO IS NULL AND TIPO_DOCUMENTO='F'"
    '        sSQL += " AND DOCORG.IDFATTURANOTA NOT IN ("
    '        sSQL += " 	SELECT IDFATTURANOTA"
    '        sSQL += " 	FROM TP_VARIAZIONIFATTURE"
    '        sSQL += " )"
    '        'eseguo la query
    '        iDB.ExecuteNonQuery(sSQL)
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.SetVariazioni.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsFatture::SetVariazioni::" & Err.Message & " SQL: " & sSQL)
    '        Return -1
    '    End Try
    'End Function

    'Public Function RibaltaVariazioni(ByVal oListVariazioni() As objRicercaVariazione, ByVal WFSessione As OPENUtility.CreateSessione, ByRef sNonAgg As String) As objRicercaVariazione()
    '    Dim oVariazione As objRicercaVariazione
    '    Dim x As Integer
    '    Dim FncContatori As New GestContatori
    '    Dim oMyContatore As New objContatore
    '    Dim oMyVariazione As New objVariazioneFattura
    '    Dim FncLettura As New GestLetture
    '    Dim oMyLettura As New ObjLettura
    '    Dim sMatricola As String

    '    Try
    '        For x = 0 To oListVariazioni.GetUpperBound(0)
    '            'controllo se devo aggiornare
    '            If oListVariazioni(x).bIsSel = True Then
    '                'prelevo i dati della lettura variati e li aggiorno
    '                oMyVariazione = GetVariazione(oListVariazioni(x).nIdVariazione, WFSessione)
    '                'se la lettura successiva è già fatturata non posso aggiornare
    '                If FncLettura.GetLetSucIsFatturata(oMyVariazione.nIdContatore, oMyVariazione.tDataLetturaAtt, sMatricola) = True Then
    '                    If oListVariazioni(x).sVarContatore.ToUpper = "X" Or oListVariazioni(x).sVarLettura.ToUpper = "X" Then
    '                        sNonAgg += "\nMatricola:" & sMatricola
    '                    End If
    '                Else
    '                    'controllo cosa devo aggiornare
    '                    If oListVariazioni(x).sVarContatore.ToUpper = "X" Then
    '			''prelevo i dati del contatore variati e li aggiorno
    '			'oMyVariazione = GetVariazione(oListVariazioni(x).nIdVariazione, WFSessione)
    '                        'prelevo i dati del contatore originale
    '                        oMyContatore = FncContatori.GetDetailsContatori(oMyVariazione.nIdContatore, WFSessione)
    '                        If Not oMyContatore Is Nothing Then
    '                            oMyContatore.tDataVariazione = Now
    '                            oMyContatore.nNumeroUtenze = oMyVariazione.nUtenze
    '                            oMyContatore.nTipoUtenza = oMyVariazione.nTipoUtenza
    '                            oMyContatore.nTipoContatore = oMyVariazione.nTipoContatore
    '                            oMyContatore.nCodFognatura = oMyVariazione.nCodFognatura
    '                            oMyContatore.bEsenteFognatura = oMyVariazione.bEsenteFognatura
    '                            oMyContatore.nCodDepurazione = oMyVariazione.nCodDepurazione
    '                            oMyContatore.bEsenteDepurazione = oMyVariazione.bEsenteDepurazione
    '                            oMyContatore.bEsenteAcqua = oMyVariazione.bEsenteAcqua
    '				'*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '				oMyContatore.bEsenteACQUAQF = oMyVariazione.bEsenteACQUAQF
    '				oMyContatore.bEsenteDepQF = oMyVariazione.bEsenteDepQF
    '				oMyContatore.bEsenteFogQF = oMyVariazione.bEsenteFogQF
    '				'*** ***

    '				If FncContatori.SetDatiContatore(oMyContatore.nIdContatore, oMyContatore) = False Then
    '					Return Nothing
    '				End If
    '			Else
    '                            Return Nothing
    '                        End If
    '                        'setto la variazione come trattata
    '                        oMyVariazione.tDataVariazione = Now
    '                        If SetVariazione(oMyVariazione, WFSessione) < 0 Then
    '                            Return Nothing
    '                        End If
    '                    End If
    '                    'controllo cosa devo aggiornare
    '                    If oListVariazioni(x).sVarLettura.ToUpper = "X" Then
    '                        'prelevo i dati della lettura variati
    '			Log.Debug("ClsFatture::RibaltaVariazioni::oMyVariazione.nIdContatore::" & oMyVariazione.nIdContatore)
    '			'*** 20130110 - devo prelevare anche la data lettura orginale perchè posso variarla di conseguenza non troverei più la lettura da aggiornare ***
    '                        oMyLettura = FncLettura.GetDettaglioLetture(oMyVariazione.nIdLettura, oMyVariazione.nIdContatore, oMyVariazione.nIdPeriodo, oMyVariazione.tDataLetturaAttOrg, -1, False)
    '                        If Not oMyLettura Is Nothing Then
    '                            oMyLettura.tDataVariazione = Now
    '                            oMyVariazione.sAzione = "VA"
    '                            oMyLettura.tDataLetturaAtt = oMyVariazione.tDataLetturaAtt
    '                            oMyLettura.nLetturaAtt = oMyVariazione.nLetturaAtt
    '                            oMyLettura.tDataLetturaPrec = oMyVariazione.tDataLetturaPrec
    '                            oMyLettura.nLetturaPrec = oMyVariazione.nLetturaPrec
    '                            oMyLettura.nConsumo = oMyVariazione.nConsumo
    '                            oMyLettura.nGiorni = oMyVariazione.nGiorni
    '                            oMyLettura.sAzione = "VA-RIBALTA"
    '                            oMyLettura.tDataInserimento = Now

    '				Log.Debug("ClsFatture::RibaltaVariazioni::oMyLettura.nIdContatore::" & oMyLettura.nIdContatore)
    '				If FncLettura.SetLetture(oMyLettura.IdLettura, oMyLettura) < 1 Then
    '					Return Nothing
    '				End If
    '			Else
    '                            Return Nothing
    '                        End If
    '                        'setto la variazione come trattata
    '                        oMyVariazione.tDataVariazione = Now
    '                        If SetVariazione(oMyVariazione, WFSessione) < 0 Then
    '                            Return Nothing
    '                        End If
    '                    End If
    '                End If
    '                'se non dove aggiornare nulla setto comunque la variazione come trattata
    '                If oListVariazioni(x).sVarContatore.ToUpper = "" And oListVariazioni(x).sVarLettura.ToUpper = "" Then
    '                    oMyVariazione.tDataVariazione = Now
    '                    If SetVariazione(oMyVariazione, WFSessione) < 0 Then
    '                        Return Nothing
    '                    End If
    '                End If
    '            End If
    '        Next

    '        Return oListVariazioni
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.RibaltaVariazioni.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    Public Function RibaltaVariazioni(ByVal oListVariazioni() As objRicercaVariazione, ByRef sNonAgg As String) As objRicercaVariazione()
        Dim x As Integer
        Dim FncContatori As New GestContatori
        Dim oMyContatore As New objContatore
        Dim oMyVariazione As New objVariazioneFattura
        Dim FncLettura As New GestLetture
        Dim oMyLettura As New ObjLettura
        Dim sMatricola As String = ""

        Try
            For x = 0 To oListVariazioni.GetUpperBound(0)
                'controllo se devo aggiornare
                If oListVariazioni(x).bIsSel = True Then
                    'prelevo i dati della lettura variati e li aggiorno
                    oMyVariazione = GetVariazione(oListVariazioni(x).nIdVariazione)
                    'se la lettura successiva è già fatturata non posso aggiornare
                    If FncLettura.GetLetSucIsFatturata(oMyVariazione.nIdContatore, oMyVariazione.tDataLetturaAtt, sMatricola) = True Then
                        If oListVariazioni(x).sVarContatore.ToUpper = "X" Or oListVariazioni(x).sVarLettura.ToUpper = "X" Then
                            sNonAgg += "\nMatricola:" & sMatricola
                        End If
                    Else
                        'controllo cosa devo aggiornare
                        If oListVariazioni(x).sVarContatore.ToUpper = "X" Then
                            ''prelevo i dati del contatore variati e li aggiorno
                            'oMyVariazione = GetVariazione(oListVariazioni(x).nIdVariazione, WFSessione)
                            'prelevo i dati del contatore originale
                            oMyContatore = FncContatori.GetDetailsContatori(oMyVariazione.nIdContatore, -1)
                            If Not oMyContatore Is Nothing Then
                                oMyContatore.tDataVariazione = Now
                                oMyContatore.nNumeroUtenze = oMyVariazione.nUtenze
                                oMyContatore.nTipoUtenza = oMyVariazione.nTipoUtenza
                                oMyContatore.nTipoContatore = oMyVariazione.nTipoContatore
                                oMyContatore.nCodFognatura = oMyVariazione.nCodFognatura
                                oMyContatore.bEsenteFognatura = oMyVariazione.bEsenteFognatura
                                oMyContatore.nCodDepurazione = oMyVariazione.nCodDepurazione
                                oMyContatore.bEsenteDepurazione = oMyVariazione.bEsenteDepurazione
                                oMyContatore.bEsenteAcqua = oMyVariazione.bEsenteAcqua
                                '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
                                oMyContatore.bEsenteAcquaQF = oMyVariazione.bEsenteAcquaQF
                                oMyContatore.bEsenteDepQF = oMyVariazione.bEsenteDepQF
                                oMyContatore.bEsenteFogQF = oMyVariazione.bEsenteFogQF
                                '*** ***

                                If FncContatori.SetDatiContatore(oMyContatore.nIdContatore, oMyContatore, False) = False Then
                                    Return Nothing
                                End If
                            Else
                                Return Nothing
                            End If
                            'setto la variazione come trattata
                            oMyVariazione.tDataVariazione = Now
                            If SetVariazione(oMyVariazione) < 0 Then
                                Return Nothing
                            End If
                        End If
                        'controllo cosa devo aggiornare
                        If oListVariazioni(x).sVarLettura.ToUpper = "X" Then
                            'prelevo i dati della lettura variati
                            Log.Debug("ClsFatture::RibaltaVariazioni::oMyVariazione.nIdContatore::" & oMyVariazione.nIdContatore)
                            '*** 20130110 - devo prelevare anche la data lettura orginale perchè posso variarla di conseguenza non troverei più la lettura da aggiornare ***
                            oMyLettura = FncLettura.GetDettaglioLetture(oMyVariazione.nIdLettura, oMyVariazione.nIdContatore, oMyVariazione.nIdPeriodo, oMyVariazione.tDataLetturaAttOrg, -1, False)
                            If Not oMyLettura Is Nothing Then
                                oMyLettura.tDataVariazione = Now
                                oMyVariazione.sAzione = "VA"
                                oMyLettura.tDataLetturaAtt = oMyVariazione.tDataLetturaAtt
                                oMyLettura.nLetturaAtt = oMyVariazione.nLetturaAtt
                                oMyLettura.tDataLetturaPrec = oMyVariazione.tDataLetturaPrec
                                oMyLettura.nLetturaPrec = oMyVariazione.nLetturaPrec
                                oMyLettura.nConsumo = oMyVariazione.nConsumo
                                oMyLettura.nGiorni = oMyVariazione.nGiorni
                                oMyLettura.sAzione = "VA-RIBALTA"
                                oMyLettura.tDataInserimento = Now

                                Log.Debug("ClsFatture::RibaltaVariazioni::oMyLettura.nIdContatore::" & oMyLettura.nIdContatore)
                                If FncLettura.SetLetture(oMyLettura.IdLettura, oMyLettura) < 1 Then
                                    Return Nothing
                                End If
                            Else
                                Return Nothing
                            End If
                            'setto la variazione come trattata
                            oMyVariazione.tDataVariazione = Now
                            If SetVariazione(oMyVariazione) < 0 Then
                                Return Nothing
                            End If
                        End If
                    End If
                    'se non dove aggiornare nulla setto comunque la variazione come trattata
                    'If oListVariazioni(x).sVarContatore.ToUpper = "" And oListVariazioni(x).sVarLettura.ToUpper = "" Then
                    If oListVariazioni(x).sVarContatore.ToUpper = "" Then
                        oMyVariazione.tDataVariazione = Now
                        If SetVariazione(oMyVariazione) < 0 Then
                            Return Nothing
                        End If
                    End If
                End If
            Next

            Return oListVariazioni
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRibaltaVar.RibaltaVariazioni.errore: ", Err)
            Return Nothing
        End Try
    End Function
End Class
