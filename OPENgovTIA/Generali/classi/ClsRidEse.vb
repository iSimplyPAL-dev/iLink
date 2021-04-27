Imports log4net
Imports OPENUtility
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports Utility
''' <summary>
''' Classe per la consultazione riduzioni/esenzioni
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestRidEse
    Inherits ObjRidEseApplicati
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestRidEse))
    Private cmdMyCommand As New SqlClient.SqlCommand
    Private DrReturn As SqlClient.SqlDataReader

    Public Function GetRidEse(ByVal myConnectionString As String, ByVal oMyObj As ObjRidEse, ByVal sTabella As String) As ObjRidEse() 'ByVal WFSessione As CreateSessione,
        Dim oListToReturn() As ObjRidEse = Nothing
        Dim oMyRidEse As ObjRidEse
        Dim nList As Integer = -1

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString) 'WFSessione.oSession.oAppDB.GetConnection
            cmdMyCommand.CommandText = "SELECT TIPI.IDENTE, COD.ID, TIPI.CODICE"
            cmdMyCommand.CommandText += ", TIPI.DESCRIZIONE, TIPO, VALORE, COD.TIPOAPPLICAZIONE"
            cmdMyCommand.CommandText += ", ANNO, DATA_INIZIO_VALIDITA, DATA_FINE_VALIDITA"
            cmdMyCommand.CommandText += ", DESCRTIPO= CASE WHEN TIPO= 'I' THEN 'IMPORTO' WHEN TIPO= 'F' THEN 'FORMULA' ELSE '%' END"
            cmdMyCommand.CommandText += ", APPL.DESCRIZIONE AS DESCRAPPLICAZIONE"
            cmdMyCommand.CommandText += " FROM TBL" & sTabella & " COD"
            cmdMyCommand.CommandText += " INNER JOIN TBLTIPO" & sTabella & " TIPI ON COD.IDENTE=TIPI.IDENTE AND COD.CODICE=TIPI.CODICE"
            cmdMyCommand.CommandText += " INNER JOIN TBLTIPOAPPLICAZIONE_RIDESE APPL ON COD.TIPOAPPLICAZIONE=APPL.CODICE"
            'VALORIZZO I PARAMETERS:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText += " WHERE (1=1)"
            cmdMyCommand.CommandText += " AND (TIPI.IDENTE=@IDENTE)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyObj.IdEnte
            If oMyObj.sCodice <> "" Then
                cmdMyCommand.CommandText += " AND (TIPI.CODICE=@CODICE)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE", SqlDbType.NVarChar)).Value = oMyObj.sCodice
            End If
            If oMyObj.sAnno <> "" Then
                cmdMyCommand.CommandText += " AND (ANNO=@ANNO)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyObj.sAnno
            End If
            cmdMyCommand.CommandText += " ORDER BY TIPI.DESCRIZIONE"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrReturn = cmdMyCommand.ExecuteReader 'WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
            Do While DrReturn.Read
                nList += 1
                oMyRidEse = New ObjRidEse
                oMyRidEse.ID = CInt(DrReturn("id"))
                oMyRidEse.IdEnte = CStr(DrReturn("idente"))
                oMyRidEse.sCodice = CStr(DrReturn("codice"))
                oMyRidEse.sDescrizione = CStr(DrReturn("descrizione"))
                oMyRidEse.sTipoOggetto = sTabella
                oMyRidEse.sTipoValore = CStr(DrReturn("tipo"))
                oMyRidEse.sDescrTipo = CStr(DrReturn("descrtipo"))
                oMyRidEse.sValore = CStr(DrReturn("valore"))
                oMyRidEse.nTipoApplicazione = CInt(DrReturn("tipoapplicazione"))
                oMyRidEse.sDescrApplicazione = CStr(DrReturn("descrapplicazione"))
                oMyRidEse.sAnno = CStr(DrReturn("anno"))
                oMyRidEse.tDataInizioValidita = CDate(DrReturn("data_inizio_validita"))
                If Not IsDBNull(DrReturn("data_fine_validita")) Then
                    oMyRidEse.tDataFineValidita = CDate(DrReturn("data_fine_validita"))
                End If
                'dimensiono l'array
                ReDim Preserve oListToReturn(nList)
                'memorizzo i dati nell'array
                oListToReturn(nList) = oMyRidEse
            Loop

            HttpContext.Current.Session("dsRidEseSort") = oListToReturn
            Return oListToReturn

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRidEse.GetRidEse.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            DrReturn.Close()
        End Try
    End Function
    Public Function GetRidEseApplicate(ByVal myConnectionString As String, ByVal oMyObj As ObjRidEse, ByVal sTabella As String, ByVal sRiferimento As String, ByVal nIdRiferimento As Integer, ByVal TipoPartita As String) As ObjRidEseApplicati()
        Dim oListToReturn() As ObjRidEseApplicati = Nothing
        Dim oMyRidEse As ObjRidEseApplicati
        Dim nList As Integer = -1
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "sp_GetRidEseApplicate", "MyTabella", "MyRiferimento", "IDENTE", "IDRIFERIMENTO", "CODICE", "ANNO", "TipoPartita")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("MyTabella", sTabella) _
                        , ctx.GetParam("MyRiferimento", sRiferimento) _
                        , ctx.GetParam("IDENTE", oMyObj.IdEnte) _
                        , ctx.GetParam("IdRiferimento", nIdRiferimento) _
                        , ctx.GetParam("CODICE", oMyObj.sCodice) _
                        , ctx.GetParam("ANNO", oMyObj.sAnno) _
                        , ctx.GetParam("TipoPartita", TipoPartita)
                    )
                ctx.Dispose()
            End Using
            For Each dtMyRow As DataRowView In dvMyDati
                nList += 1
                oMyRidEse = New ObjRidEseApplicati
                oMyRidEse.IdRiferimento = nIdRiferimento
                oMyRidEse.Riferimento = sRiferimento
                oMyRidEse.ID = StringOperation.FormatInt(dtMyRow("id"))
                oMyRidEse.IdEnte = StringOperation.FormatString(dtMyRow("idente"))
                oMyRidEse.sCodice = StringOperation.FormatString(dtMyRow("codice"))
                oMyRidEse.sDescrizione = StringOperation.FormatString(dtMyRow("descrizione"))
                oMyRidEse.sTipoOggetto = sTabella
                oMyRidEse.sTipoValore = StringOperation.FormatString(dtMyRow("tipo"))
                oMyRidEse.sDescrTipo = StringOperation.FormatString(dtMyRow("descrtipo"))
                oMyRidEse.sValore = StringOperation.FormatString(dtMyRow("valore"))
                oMyRidEse.nTipoApplicazione = StringOperation.FormatInt(dtMyRow("tipoapplicazione"))
                oMyRidEse.sDescrApplicazione = StringOperation.FormatString(dtMyRow("descrapplicazione"))
                oMyRidEse.sAnno = StringOperation.FormatString(dtMyRow("anno"))
                oMyRidEse.tDataInizioValidita = StringOperation.FormatDateTime(dtMyRow("data_inizio_validita"))
                oMyRidEse.tDataFineValidita = StringOperation.FormatDateTime(dtMyRow("data_fine_validita"))
                'dimensiono l'array
                ReDim Preserve oListToReturn(nList)
                'memorizzo i dati nell'array
                oListToReturn(nList) = oMyRidEse
            Next
            Return oListToReturn
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRidEse.GetRidEseApplicate.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function GetRidEseApplicate(ByVal myConnectionString As String, ByVal oMyObj As ObjRidEse, ByVal sTabella As String, ByVal sRiferimento As String, ByVal nIdRiferimento As Integer) As ObjRidEseApplicati()
    '    Dim oListToReturn() As ObjRidEseApplicati = Nothing
    '    Dim oMyRidEse As ObjRidEseApplicati
    '    Dim nList As Integer = -1

    '    Try
    '        'Valorizzo la connessione
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "sp_GetRidEseApplicate"
    '        'VALORIZZO I PARAMETERS:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MyTabella", SqlDbType.NVarChar)).Value = sTabella
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MyRiferimento", SqlDbType.NVarChar)).Value = sRiferimento
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyObj.IdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDRIFERIMENTO", SqlDbType.Int)).Value = nIdRiferimento
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE", SqlDbType.NVarChar)).Value = oMyObj.sCodice
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyObj.sAnno
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        DrReturn = cmdMyCommand.ExecuteReader
    '        Do While DrReturn.Read
    '            nList += 1
    '            oMyRidEse = New ObjRidEseApplicati
    '            oMyRidEse.IdRiferimento = nIdRiferimento
    '            oMyRidEse.Riferimento = sRiferimento
    '            oMyRidEse.ID = CInt(DrReturn("id"))
    '            oMyRidEse.IdEnte = CStr(DrReturn("idente"))
    '            oMyRidEse.sCodice = CStr(DrReturn("codice"))
    '            oMyRidEse.sDescrizione = CStr(DrReturn("descrizione"))
    '            oMyRidEse.sTipoOggetto = sTabella
    '            oMyRidEse.sTipoValore = CStr(DrReturn("tipo"))
    '            oMyRidEse.sDescrTipo = CStr(DrReturn("descrtipo"))
    '            oMyRidEse.sValore = CStr(DrReturn("valore"))
    '            oMyRidEse.nTipoApplicazione = CInt(DrReturn("tipoapplicazione"))
    '            oMyRidEse.sDescrApplicazione = CStr(DrReturn("descrapplicazione"))
    '            oMyRidEse.sAnno = CStr(DrReturn("anno"))
    '            oMyRidEse.tDataInizioValidita = CDate(DrReturn("data_inizio_validita"))
    '            If Not IsDBNull(DrReturn("data_fine_validita")) Then
    '                oMyRidEse.tDataFineValidita = CDate(DrReturn("data_fine_validita"))
    '            End If
    '            'dimensiono l'array
    '            ReDim Preserve oListToReturn(nList)
    '            'memorizzo i dati nell'array
    '            oListToReturn(nList) = oMyRidEse
    '        Loop

    '        Return oListToReturn
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestRidEse.GetRidEseApplicate.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        DrReturn.Close()
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    '    '*** 20131104 - TARES ***
    'Public Function GetRidEseApplicate(ByVal oMyObj As ObjRidEse, ByVal sTabella As String, ByVal sRiferimento As String, ByVal nIdRiferimento As Integer, ByVal TipoPartita As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjRidEseApplicati()
    '    Dim oListToReturn() As ObjRidEseApplicati = Nothing
    '    Dim oMyRidEse As ObjRidEseApplicati
    '    Dim nList As Integer = -1
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        Log.Debug("GetRidEseApplicate::devo prelevare " + sRiferimento)
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@MyTabella", sTabella)
    '        cmdMyCommand.Parameters.AddWithValue("@MyRiferimento", sRiferimento)
    '        cmdMyCommand.Parameters.AddWithValue("@IDENTE", oMyObj.IdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@IDRIFERIMENTO", nIdRiferimento)
    '        cmdMyCommand.Parameters.AddWithValue("@CODICE", oMyObj.sCodice)
    '        cmdMyCommand.Parameters.AddWithValue("@ANNO", oMyObj.sAnno)
    '        cmdMyCommand.Parameters.AddWithValue("@TipoPartita", TipoPartita)
    '        cmdMyCommand.CommandText = "sp_GetRidEseApplicate"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            nList += 1
    '            oMyRidEse = New ObjRidEseApplicati
    '            oMyRidEse.IdRiferimento = nIdRiferimento
    '            oMyRidEse.Riferimento = sRiferimento
    '            oMyRidEse.ID = CInt(dtMyRow("id"))
    '            oMyRidEse.IdEnte = CStr(dtMyRow("idente"))
    '            oMyRidEse.sCodice = CStr(dtMyRow("codice"))
    '            oMyRidEse.sDescrizione = CStr(dtMyRow("descrizione"))
    '            oMyRidEse.sTipoOggetto = sTabella
    '            oMyRidEse.sTipoValore = CStr(dtMyRow("tipo"))
    '            oMyRidEse.sDescrTipo = CStr(dtMyRow("descrtipo"))
    '            oMyRidEse.sValore = CStr(dtMyRow("valore"))
    '            oMyRidEse.nTipoApplicazione = CInt(dtMyRow("tipoapplicazione"))
    '            oMyRidEse.sDescrApplicazione = CStr(dtMyRow("descrapplicazione"))
    '            oMyRidEse.sAnno = CStr(dtMyRow("anno"))
    '            oMyRidEse.tDataInizioValidita = CDate(dtMyRow("data_inizio_validita"))
    '            If Not IsDBNull(dtMyRow("data_fine_validita")) Then
    '                oMyRidEse.tDataFineValidita = CDate(dtMyRow("data_fine_validita"))
    '            End If
    '            'dimensiono l'array
    '            ReDim Preserve oListToReturn(nList)
    '            'memorizzo i dati nell'array
    '            oListToReturn(nList) = oMyRidEse
    '        Next
    '        Log.Debug("GetRidEseApplicate::prelevato")
    '        Return oListToReturn
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRidEse.GetRidEseApplicate.errore: ", Err)
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
    '    '*** ***
End Class
''' <summary>
''' Classe per la consultazione delle addizionali 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestAddizionali
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestAddizionali))
    Private cmdMyCommand As New SqlClient.SqlCommand
    Private DrReturn As SqlClient.SqlDataReader

    '    'Public Function GetAddizionaliAssociate(ByVal WFSessione As CreateSessione, ByVal oMyObj As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale) As Integer
    '    '    Dim nAssociate As Integer = 0

    '    '    Try
    '    '        cmdMyCommand.CommandText = "SELECT IDAVVISO"
    '    '        cmdMyCommand.CommandText += " FROM TBLCARTELLEDETTAGLIOVOCI"
    '    '        cmdMyCommand.CommandText += " WHERE (DATA_VARIAZIONE IS NULL)"
    '    '        cmdMyCommand.CommandText += " AND (CODICE_CAPITOLO= '" & oMyObj.CodiceCapitolo & "')"
    '    '        cmdMyCommand.CommandText += " AND (ANNO='" & oMyObj.Anno & "')"
    '    '        'eseguo la query
    '    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '    '        Do While DrReturn.Read
    '    '            nAssociate += 1
    '    '        Loop

    '    '        Return nAssociate
    '    '    Catch Err As Exception
    '    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAddizionali.GetAddizionaliAssociate.errore: ", Err)
    '    '        Return -1
    '    '    Finally
    '    '        DrReturn.Close()
    '    '    End Try
    '    'End Function

    '    'Public Function GetAddizionale(ByVal sIdEnte As String, ByVal sAnno As String, ByVal sCodice As String, ByVal WFSessione As OPENUtility.CreateSessione) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale()
    '    '    Dim oListAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
    '    '    Dim oMyAddizionale As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
    '    '    Dim drDati As SqlClient.SqlDataReader
    '    '    Dim nList As Integer = -1

    '    '    Try
    '    '        cmdMyCommand.CommandText = "SELECT C.*, DESCRIZIONE"
    '    '        cmdMyCommand.CommandText += " FROM TBLADDIZIONALIENTE C"
    '    '        cmdMyCommand.CommandText += " INNER JOIN TBLADDIZIONALI D ON C.CODICE=D.CODICE"
    '    '        cmdMyCommand.CommandText += " WHERE (1=1)"
    '    '        cmdMyCommand.CommandText += " AND (IDENTE=@IDENTE)"
    '    '        'valorizzo i parameters:
    '    '        cmdMyCommand.Parameters.Clear()
    '    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '    '        If sAnno <> "" Then
    '    '            cmdMyCommand.CommandText += " AND (ANNO=@ANNO)"
    '    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno
    '    '        End If
    '    '        If sCodice <> "" Then
    '    '            cmdMyCommand.CommandText += " AND (C.CODICE=@CODICE)"
    '    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE", SqlDbType.NVarChar)).Value = sCodice
    '    '        End If
    '    '        'eseguo la query
    '    '        drDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '    '        Do While drDati.Read
    '    '            oMyAddizionale = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
    '    '            oMyAddizionale.Anno = drDati("ANNO")
    '    '            oMyAddizionale.CodiceCapitolo = drDati("CODICE")
    '    '            oMyAddizionale.Descrizione = drDati("DESCRIZIONE")
    '    '            oMyAddizionale.idAddizionale = drDati("ID")
    '    '            oMyAddizionale.Valore = drDati("VALORE")
    '    '            nList += 1
    '    '            ReDim Preserve oListAddizionali(nList)
    '    '            oListAddizionali(nList) = oMyAddizionale
    '    '        Loop
    '    '        Return oListAddizionali
    '    '    Catch Err As Exception
    '    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAddizionali.GetAddizionale.errore: ", Err)
    '    '        Return Nothing
    '    '    Finally
    '    '        drDati.Close()
    '    '    End Try
    '    'End Function

    '    'Public Function DeleteAddizionale(ByVal WFSessione As CreateSessione, ByVal oMyObj As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale, ByRef strError As String) As Boolean
    '    '    Dim myIdentity As Integer

    '    '    Try
    '    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LA ADDIZIONALE NON SIA STATA ASSOCIATA AD UN ARTICOLO A RUOLO
    '    '        myIdentity = GetAddizionaliAssociate(WFSessione, oMyObj)
    '    '        If myIdentity > 0 Then
    '    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '    '            strError = "Impossibile ELIMINARE la Posizione selezionata! La posizione è già stata associata ad un articolo a Ruolo!"
    '    '            Return False
    '    '        End If

    '    '        cmdMyCommand.CommandText = "DELETE"
    '    '        cmdMyCommand.CommandText += " FROM TBLADDIZIONALIENTE"
    '    '        cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '    '        'VALORIZZO I PARAMETERS:
    '    '        cmdMyCommand.Parameters.Clear()
    '    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.NVarChar)).Value = oMyObj.idAddizionale
    '    '        'eseguo la query
    '    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '    '            Return False
    '    '        End If

    '    '        Return True
    '    '    Catch Err As Exception
    '    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAddizionali.DeleteAddizionale.errore: ", Err)
    '    '        Return False
    '    '    End Try
    '    'End Function

    '    'Public Function SetAddizionale(ByVal WFSessione As CreateSessione, ByVal sIdEnte As String, ByVal oMyObj As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale, ByRef sError As String) As Boolean
    '    '    Dim myIdentity As Integer
    '    '    Dim oAddizionale() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale

    '    '    Try
    '    '        oAddizionale = GetAddizionale(sIdEnte, oMyObj.Anno, oMyObj.CodiceCapitolo, WFSessione)
    '    '        If Not IsNothing(oAddizionale) Then
    '    '            sError = "E\' già presente una posizione per i dati Inseriti.\nAggiornare la posizione presente."
    '    '            Return False
    '    '        End If

    '    '        cmdMyCommand.CommandText = "INSERT INTO TBLADDIZIONALIENTE"
    '    '        cmdMyCommand.CommandText += " (IDENTE, CODICE"
    '    '        cmdMyCommand.CommandText += ", VALORE"
    '    '        cmdMyCommand.CommandText += ", ANNO, DATA_INIZIO_VALIDITA, DATA_FINE_VALIDITA)"
    '    '        cmdMyCommand.CommandText += " VALUES(@IDENTE,@CODICE"
    '    '        cmdMyCommand.CommandText += ",@VALORE"
    '    '        cmdMyCommand.CommandText += ",@ANNO,@DATA_INIZIO_VALIDITA,@DATA_FINE_VALIDITA)"
    '    '        'VALORIZZO I PARAMETERS:
    '    '        cmdMyCommand.Parameters.Clear()
    '    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE", SqlDbType.NVarChar)).Value = oMyObj.CodiceCapitolo
    '    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALORE", SqlDbType.NVarChar)).Value = oMyObj.Valore
    '    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyObj.Anno
    '    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INIZIO_VALIDITA", SqlDbType.DateTime)).Value = CDate("01/01/" & oMyObj.Anno)
    '    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE_VALIDITA", SqlDbType.DateTime)).Value = CDate("31/12/" & oMyObj.Anno)
    '    '        'eseguo la query
    '    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '    '            Return False
    '    '        End If
    '    '        Return True
    '    '    Catch Err As Exception
    '    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAddizionali.SetAddizionale.errore: ", Err)
    '    '        Return False
    '    '    End Try
    '    'End Function

    '    'Public Function UpdateAddizionale(ByVal WFSessione As CreateSessione, ByVal sIdEnte As String, ByVal oMyObj As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale, ByRef strError As String) As Boolean
    '    '    Dim myIdentity As Integer
    '    '    Dim oAddizionaleOld() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale

    '    '    Try
    '    '        'PRELEVO I DATI DELLA ADDIZIONALE DA MODIFICARE
    '    '        oAddizionaleOld = GetAddizionale(sIdEnte, oMyObj.Anno, oMyObj.CodiceCapitolo, WFSessione)
    '    '        If IsNothing(oAddizionaleOld) Then
    '    '            strError = "Non sono presenti dati per l'Addizionale Selezionata."
    '    '            Return False
    '    '        End If

    '    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LA ADDIZIONALE NON SIA STATA ASSOCIATA AD UN ARTICOLO A RUOLO
    '    '        myIdentity = GetAddizionaliAssociate(WFSessione, oMyObj)
    '    '        If myIdentity > 0 Then
    '    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '    '            strError = "Impossibile MODIFICARE la posizione selezionata!\nLa posizione è già stata associata ad un articolo a Ruolo!"
    '    '            Return False
    '    '        End If

    '    '        'SE L'IDCATEGORIA è CAMBIATO o è CAMBIATA LA DATA_INIZIO_VALIDITA
    '    '        If oAddizionaleOld(0).Anno <> oMyObj.Anno Then
    '    '            'CONTROLLO PRIMA SE PER L'ANNO E LA CATEGORIA INSERITE HO GIà UNA ADDIZIONALE VALORIZZATA
    '    '            oAddizionaleOld = GetAddizionale(sIdEnte, oMyObj.Anno, oMyObj.CodiceCapitolo, WFSessione)
    '    '            If Not IsNothing(oAddizionaleOld) Then
    '    '                strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
    '    '                Return False
    '    '            End If

    '    '            'aggiorno i dati
    '    '            cmdMyCommand.CommandText = "UPDATE TBLADDIZIONALIENTE"
    '    '            cmdMyCommand.CommandText += " SET ANNO=@ANNO"
    '    '            cmdMyCommand.CommandText += ", DATA_INIZIO_VALIDITA=@DATA_INIZIO_VALIDITA"
    '    '            cmdMyCommand.CommandText += ", DATA_FINE_VALIDITA=@DATA_FINE_VALIDITA"
    '    '            cmdMyCommand.CommandText += ", VALORE=@VALORE"
    '    '            cmdMyCommand.CommandText += ", CODICE=@CODICE"
    '    '            cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '    '            cmdMyCommand.CommandText += " AND (IDENTE=@IDENTE)"
    '    '            'VALORIZZO I PARAMETERS:
    '    '            cmdMyCommand.Parameters.Clear()
    '    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyObj.Anno
    '    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INIZIO_VALIDITA", SqlDbType.DateTime)).Value = CDate("01/01/" & oMyObj.Anno)
    '    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE_VALIDITA", SqlDbType.DateTime)).Value = CDate("31/12/" & oMyObj.Anno)
    '    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALORE", SqlDbType.NVarChar)).Value = oMyObj.Valore
    '    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE", SqlDbType.NVarChar)).Value = oMyObj.CodiceCapitolo
    '    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oAddizionaleOld(0).idAddizionale
    '    '            'eseguo la query
    '    '            If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '    '                Return False
    '    '            End If

    '    '        ElseIf oAddizionaleOld(0).Valore <> oMyObj.Valore And oAddizionaleOld(0).Anno = oMyObj.Anno Then
    '    '            'ALTRIMENTI SE CAMBIA SOLO valore e tipo AGGIORNO valore e tipo
    '    '            cmdMyCommand.CommandText = "UPDATE TBLADDIZIONALIENTE"
    '    '            cmdMyCommand.CommandText += " SET VALORE=@VALORE"
    '    '            cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '    '            cmdMyCommand.CommandText += " AND (IDENTE=@IDENTE)"
    '    '            'VALORIZZO I PARAMETERS:
    '    '            cmdMyCommand.Parameters.Clear()
    '    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALORE", SqlDbType.NVarChar)).Value = oMyObj.Valore
    '    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oAddizionaleOld(0).idAddizionale
    '    '            'eseguo la query
    '    '            If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '    '                Return False
    '    '            End If
    '    '        End If
    '    '        Return True
    '    '    Catch Err As Exception
    '    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAddizionali.UpdateAddizionale.errore: ", Err)
    '    '        Return False
    '    '    End Try
    '    'End Function

    '*** 20131104 - TARES ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sCodice"></param>
    ''' <param name="sTipoRuolo"></param>
    ''' <returns></returns>
    Public Function GetAddizionale(myConnectionString As String, ByVal sIdEnte As String, ByVal sAnno As String, ByVal sCodice As String, sTipoRuolo As String) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale()
        Dim oListAddizionali As New ArrayList
        Dim oMyAddizionale As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetAddizionali", "IDENTE", "ANNO", "CODICE", "TIPORUOLO")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("ANNO", sAnno) _
                            , ctx.GetParam("CODICE", sCodice) _
                            , ctx.GetParam("TIPORUOLO", sTipoRuolo)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestAddizionali.GetAddizionale.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    oMyAddizionale = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
                    oMyAddizionale.Anno = StringOperation.FormatString(myRow("ANNO"))
                    oMyAddizionale.CodiceCapitolo = StringOperation.FormatString(myRow("CODICE"))
                    oMyAddizionale.Descrizione = StringOperation.FormatString(myRow("DESCRIZIONE"))
                    oMyAddizionale.idAddizionale = StringOperation.FormatInt(myRow("ID"))
                    oMyAddizionale.Valore = StringOperation.FormatDouble(myRow("VALORE"))
                    oMyAddizionale.TipoCalcolo = StringOperation.FormatString(myRow("TIPOCALCOLO"))
                    oListAddizionali.Add(oMyAddizionale)
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestAddizionali.GetAddizionale.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
        Return CType(oListAddizionali.ToArray(GetType(RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale)), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale())
    End Function
    Public Function GetSpeseIngiunzione(myConnectionString As String, ByVal IdEnte As String, ByVal IdTributo As String, ByVal Anno As String, ByVal CodTipoProvvedimento As String) As Double
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim impSpese As Double = 0

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetSpese", "IDENTE", "IDTRIBUTO", "ANNO", "CODTIPOPROVVEDIMENTO")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", IdEnte) _
                            , ctx.GetParam("IDTRIBUTO", IdTributo) _
                            , ctx.GetParam("ANNO", Anno) _
                            , ctx.GetParam("CODTIPOPROVVEDIMENTO", CodTipoProvvedimento)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestAddizionali.GetSpeseIngiunzione.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    impSpese = StringOperation.FormatDouble(myRow("VALORE"))
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestAddizionali.GetSpeseIngiunzione.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
        Return impSpese
    End Function
    'Public Function GetAddizionale(myConnectionString As String, ByVal sIdEnte As String, ByVal sAnno As String, ByVal sCodice As String, sTipoRuolo As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale()
    '    Dim oListAddizionali As New ArrayList
    '    Dim oMyAddizionale As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow
    '    Dim myAdapter As New SqlClient.SqlDataAdapter

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '            cmdMyCommand.Connection.Open()
    '            cmdMyCommand.CommandTimeout = 0
    '        End If

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IdEnte", sIdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@Anno", sAnno)
    '        cmdMyCommand.Parameters.AddWithValue("@CODICE", sCodice)
    '        cmdMyCommand.Parameters.AddWithValue("@TIPORUOLO", sTipoRuolo)
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetAddizionali"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyAddizionale = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
    '            oMyAddizionale.Anno = dtMyRow("ANNO")
    '            oMyAddizionale.CodiceCapitolo = dtMyRow("CODICE")
    '            oMyAddizionale.Descrizione = dtMyRow("DESCRIZIONE")
    '            oMyAddizionale.idAddizionale = dtMyRow("ID")
    '            oMyAddizionale.Valore = dtMyRow("VALORE")
    '            oMyAddizionale.TipoCalcolo = dtMyRow("TIPOCALCOLO")
    '            oListAddizionali.Add(oMyAddizionale)
    '        Next

    '        Return CType(oListAddizionali.ToArray(GetType(RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale)), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestAddizionali.GetAddizionale.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        myAdapter.Dispose()
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    '*** ***
End Class
''' <summary>
''' Classe per la consultazione delle tariffe
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestTariffe
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestTariffe))
    Private DrReturn As SqlClient.SqlDataReader

    'Public Function GetTariffeAssociate(ByVal WFSessione As CreateSessione, ByVal oMyObj As objtariffa) As Integer
    '    Dim nAssociate As Integer = 0

    '    Try
    '        cmdMyCommand.CommandText = "SELECT IDRUOLO"
    '        cmdMyCommand.CommandText += " FROM TBLARTICOLI"
    '        cmdMyCommand.CommandText += " WHERE (DATA_VARIAZIONE IS NULL)"
    '        cmdMyCommand.CommandText += " AND (IDTariffa= " & oMyObj.ID & ")"
    '        'eseguo la query
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrReturn.Read
    '            nAssociate += 1
    '        Loop

    '        Return nAssociate
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTariffe.GetTariffeAssociate.errore: ", Err))
    '        Return -1
    '    Finally
    '        DrReturn.Close()
    '    End Try
    'End Function

    'Public Function GetTariffa(ByVal WFSessione As CreateSessione, ByVal oMyObj As ObjTariffa) As ObjTariffa()
    '    Dim oListToReturn() As ObjTariffa
    '    Dim oMyTariffa As ObjTariffa
    '    Dim nList As Integer = -1

    '    Try
    '        'Valorizzo la connessione
    '        cmdMyCommand.Connection = WFSessione.oSession.oAppDB.GetConnection
    '        cmdMyCommand.CommandText = "SELECT TIPI.IDENTE, COD.ID, TIPI.CODICE"
    '        cmdMyCommand.CommandText += ", TIPI.DESCRIZIONE, VALORE"
    '        cmdMyCommand.CommandText += ", ANNO, DATA_INIZIO_VALIDITA, DATA_FINE_VALIDITA"
    '        cmdMyCommand.CommandText += " FROM TBLTARIFFE COD"
    '        cmdMyCommand.CommandText += " INNER JOIN TBLCATEGORIE TIPI ON COD.IDENTE=TIPI.IDENTE AND COD.CODICE=TIPI.CODICE"
    '        cmdMyCommand.CommandText += " WHERE (TIPI.IDENTE=@IDENTE)"
    '        'VALORIZZO I PARAMETERS:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyObj.IdEnte
    '        If oMyObj.sCodice <> "" Then
    '            cmdMyCommand.CommandText += " AND (TIPI.CODICE=@CODICE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE", SqlDbType.NVarChar)).Value = oMyObj.sCodice
    '        End If
    '        If oMyObj.sAnno <> "" Then
    '            cmdMyCommand.CommandText += " AND (ANNO=@ANNO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyObj.sAnno
    '        End If
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrReturn.Read
    '            nList += 1
    '            oMyTariffa = New ObjTariffa
    '            oMyTariffa.ID = CInt(DrReturn("id"))
    '            oMyTariffa.IdEnte = CStr(DrReturn("idente"))
    '            oMyTariffa.sCodice = CStr(DrReturn("codice"))
    '            oMyTariffa.sDescrizione = CStr(DrReturn("descrizione"))
    '            oMyTariffa.sValore = CStr(DrReturn("valore"))
    '            oMyTariffa.sAnno = CStr(DrReturn("anno"))
    '            oMyTariffa.tDataInizioValidita = CDate(DrReturn("data_inizio_validita"))
    '            If Not IsDBNull(DrReturn("data_fine_validita")) Then
    '                oMyTariffa.tDataFineValidita = CDate(DrReturn("data_fine_validita"))
    '            End If
    '            'dimensiono l'array
    '            ReDim Preserve oListToReturn(nList)
    '            'memorizzo i dati nell'array
    '            oListToReturn(nList) = oMyTariffa
    '        Loop

    '        HttpContext.Current.Session("dsTariffaSort") = oListToReturn
    '        Return oListToReturn

    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTariffe.GetTariffa.errore: ", Err))
    '        Return Nothing
    '    Finally
    '        DrReturn.Close()
    '    End Try
    'End Function

    'Public Function DeleteTariffa(ByVal WFSessione As CreateSessione, ByVal oMyObj As ObjTariffa, ByRef strError As String) As Boolean
    '    Dim myIdentity As Integer

    '    Try
    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LA Tariffa NON SIA STATA ASSOCIATA AD UN ARTICOLO A RUOLO
    '        myIdentity = GetTariffeAssociate(WFSessione, oMyObj)
    '        If myIdentity > 0 Then
    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '            strError = "Impossibile ELIMINARE la Posizione selezionata! La posizione è già stata associata ad un articolo a Ruolo!"
    '            Return False
    '        End If

    '        cmdMyCommand.CommandText = "DELETE"
    '        cmdMyCommand.CommandText += " FROM TBLTARIFFE"
    '        cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '        'VALORIZZO I PARAMETERS:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.NVarChar)).Value = oMyObj.ID
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '            Return False
    '        End If

    '        Return True
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTariffe.DeleteTariffa.errore: ", Err))
    '        Return False
    '    End Try
    'End Function

    'Public Function SetTariffa(ByVal WFSessione As CreateSessione, ByVal oMyObj As ObjTariffa, ByRef strErrorTariffa As String) As Boolean
    '    Dim myIdentity As Integer
    '    Dim oTariffa() As ObjTariffa

    '    Try
    '        oMyObj.tDataInizioValidita = CDate("01/01/" & oMyObj.sAnno)
    '        oMyObj.tDataFineValidita = CDate("31/12/" & oMyObj.sAnno)

    '        oTariffa = GetTariffa(WFSessione, oMyObj)
    '        If Not IsNothing(oTariffa) Then
    '            strErrorTariffa = "E\' già presente una posizione per i dati Inseriti.\nAggiornare la posizione presente."
    '            Return False
    '        End If

    '        cmdMyCommand.CommandText = "INSERT INTO TBLTARIFFE"
    '        cmdMyCommand.CommandText += " (IDENTE, CODICE"
    '        cmdMyCommand.CommandText += ", VALORE"
    '        cmdMyCommand.CommandText += ", ANNO, DATA_INIZIO_VALIDITA, DATA_FINE_VALIDITA)"
    '        cmdMyCommand.CommandText += " VALUES(@IDENTE,@CODICE"
    '        cmdMyCommand.CommandText += ",@VALORE"
    '        cmdMyCommand.CommandText += ",@ANNO,@DATA_INIZIO_VALIDITA,@DATA_FINE_VALIDITA)"
    '        'VALORIZZO I PARAMETERS:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyObj.IdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE", SqlDbType.NVarChar)).Value = oMyObj.sCodice
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALORE", SqlDbType.Float)).Value = oMyObj.sValore
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyObj.sAnno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INIZIO_VALIDITA", SqlDbType.DateTime)).Value = oMyObj.tDataInizioValidita
    '        If Not IsDBNull(oMyObj.tDataFineValidita) Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE_VALIDITA", SqlDbType.DateTime)).Value = oMyObj.tDataFineValidita
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE_VALIDITA", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        End If
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '            Return False
    '        End If
    '        Return True
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTariffe.SetTariffa.errore: ", Err))
    '        Return False
    '    End Try
    'End Function

    'Public Function UpdateTariffa(ByVal WFSessione As CreateSessione, ByVal oMyObj As ObjTariffa, ByRef strError As String) As Boolean
    '    Dim myIdentity As Integer
    '    Dim oTariffaOld() As ObjTariffa

    '    Try
    '        oMyObj.tDataInizioValidita = CDate("01/01/" & oMyObj.sAnno)
    '        oMyObj.tDataFineValidita = CDate("31/12/" & oMyObj.sAnno)

    '        'PRELEVO I DATI DELLA Tariffa DA MODIFICARE
    '        oTariffaOld = GetTariffa(WFSessione, oMyObj)
    '        If IsNothing(oTariffaOld) Then
    '            strError = "Non sono presenti dati per la Tariffa Selezionata."
    '            Return False
    '        End If

    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LA Tariffa NON SIA STATA ASSOCIATA AD UN ARTICOLO A RUOLO
    '        myIdentity = GetTariffeAssociate(WFSessione, oMyObj)
    '        If myIdentity > 0 Then
    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '            strError = "Impossibile MODIFICARE la posizione selezionata!\nLa posizione è già stata associata ad un articolo a Ruolo!"
    '            Return False
    '        End If

    '        'SE L'IDCATEGORIA è CAMBIATO o è CAMBIATA LA DATA_INIZIO_VALIDITA
    '        If oTariffaOld(0).sAnno <> oMyObj.sAnno Then
    '            'CONTROLLO PRIMA SE PER L'ANNO E LA CATEGORIA INSERITE HO GIà UNA Tariffa VALORIZZATA
    '            oTariffaOld = GetTariffa(WFSessione, oMyObj)
    '            If Not IsNothing(oTariffaOld) Then
    '                strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
    '                Return False
    '            End If

    '            'aggiorno i dati
    '            cmdMyCommand.CommandText = "UPDATE TBLTARIFFE"
    '            cmdMyCommand.CommandText += " SET ANNO=@ANNO"
    '            cmdMyCommand.CommandText += ", DATA_INIZIO_VALIDITA=@DATA_INIZIO_VALIDITA"
    '            cmdMyCommand.CommandText += ", DATA_FINE_VALIDITA=@DATA_FINE_VALIDITA"
    '            cmdMyCommand.CommandText += ", VALORE=@VALORE"
    '            cmdMyCommand.CommandText += ", CODICE=@CODICE"
    '            cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '            cmdMyCommand.CommandText += " AND (IDENTE=@IDENTE)"
    '            'VALORIZZO I PARAMETERS:
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyObj.IdEnte
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyObj.sAnno
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INIZIO_VALIDITA", SqlDbType.DateTime)).Value = oMyObj.tDataInizioValidita
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE_VALIDITA", SqlDbType.DateTime)).Value = oMyObj.tDataFineValidita
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALORE", SqlDbType.Float)).Value = oMyObj.sValore
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE", SqlDbType.NVarChar)).Value = oMyObj.sCodice
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oTariffaOld(0).ID
    '            'eseguo la query
    '            If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '                Return False
    '            End If

    '        ElseIf oTariffaOld(0).sValore <> oMyObj.sValore And oTariffaOld(0).sAnno = oMyObj.sAnno Then
    '            'ALTRIMENTI SE CAMBIA SOLO valore e tipo AGGIORNO valore e tipo
    '            cmdMyCommand.CommandText = "UPDATE TBLTARIFFE"
    '            cmdMyCommand.CommandText += " SET VALORE=@VALORE"
    '            cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '            cmdMyCommand.CommandText += " AND (IDENTE=@IDENTE)"
    '            'VALORIZZO I PARAMETERS:
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyObj.IdEnte
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALORE", SqlDbType.Float)).Value = oMyObj.sValore
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oTariffaOld(0).ID
    '            'eseguo la query
    '            If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '                Return False
    '            End If
    '        End If
    '        Return True
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTariffe.UpdateTariffa.errore: ", Err))
    '        Return False
    '    End Try
    'End Function

    '*** 20131104 - TARES ***
    Public Function GetTariffa(myConnectionString As String, ByVal oMyObj As ObjTariffa, ByVal TypeTassazione As Integer) As ObjTariffa()
        Dim oListToReturn() As ObjTariffa = Nothing
        Dim oMyTariffa As ObjTariffa
        Dim nList As Integer = -1
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetTariffe", "IDENTE", "ANNO", "PercentTariffe", "TipoMQ", "CODICE", "TYPETASSAZIONE", "CATEGORIA")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", oMyObj.IdEnte) _
                        , ctx.GetParam("ANNO", oMyObj.sAnno) _
                        , ctx.GetParam("PercentTariffe", 100) _
                        , ctx.GetParam("TipoMQ", "D") _
                        , ctx.GetParam("CODICE", oMyObj.sCodice) _
                        , ctx.GetParam("TYPETASSAZIONE", TypeTassazione) _
                        , ctx.GetParam("CATEGORIA", "")
                    )
                ctx.Dispose()
            End Using
            For Each dtMyRow As DataRowView In dvMyDati
                nList += 1
                oMyTariffa = New ObjTariffa
                oMyTariffa.ID = StringOperation.FormatInt(dtMyRow("idtariffa"))
                oMyTariffa.sValore = StringOperation.FormatString(dtMyRow("importo_tariffa"))
                oMyTariffa.sDescrizione = StringOperation.FormatString(dtMyRow("descrizione"))
                'dimensiono l'array
                ReDim Preserve oListToReturn(nList)
                'memorizzo i dati nell'array
                oListToReturn(nList) = oMyTariffa
            Next
            Return oListToReturn
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTariffe.GetTariffa.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function GetTariffa(ByVal oMyObj As ObjTariffa, ByVal TypeTassazione As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjTariffa()
    '    Dim oListToReturn() As ObjTariffa = Nothing
    '    Dim oMyTariffa As ObjTariffa
    '    Dim nList As Integer = -1
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IDENTE", oMyObj.IdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@ANNO", oMyObj.sAnno)
    '        cmdMyCommand.Parameters.AddWithValue("@CODICE", oMyObj.sCodice)
    '        cmdMyCommand.Parameters.AddWithValue("@TYPETASSAZIONE", TypeTassazione)
    '        cmdMyCommand.Parameters.AddWithValue("@PercentTariffe", 100)
    '        cmdMyCommand.Parameters.AddWithValue("@TipoMQ", "D")
    '        cmdMyCommand.CommandText = "prc_GetTariffe"
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            nList += 1
    '            oMyTariffa = New ObjTariffa
    '            oMyTariffa.ID = CInt(dtMyRow("idtariffa"))
    '            oMyTariffa.sValore = CStr(dtMyRow("importo_tariffa"))
    '            oMyTariffa.sDescrizione = CStr(dtMyRow("descrizione"))
    '            'dimensiono l'array
    '            ReDim Preserve oListToReturn(nList)
    '            'memorizzo i dati nell'array
    '            oListToReturn(nList) = oMyTariffa
    '        Next

    '        Return oListToReturn
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTariffe.GetTariffa.errore: ", Err)
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
End Class
''' <summary>
''' Classe per la visualizazione delle descrizione ridduzioni
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestCodDescr
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestCodDescr))

    Public Function GetCodDescr(ByVal myConnectionString As String, ByVal oMyObj As ObjCodDescr, ByVal sTabella As String, ByVal Anno As String) As ObjCodDescr()
        Dim oListToReturn() As ObjCodDescr = Nothing
        Dim oMyCodDescr As ObjCodDescr
        Dim nList As Integer = -1
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetRidEse", "MyTabella", "IDENTE", "CODICE", "ANNO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("MyTabella", sTabella) _
                        , ctx.GetParam("IDENTE", oMyObj.IdEnte) _
                        , ctx.GetParam("CODICE", oMyObj.sCodice) _
                        , ctx.GetParam("ANNO", Anno)
                    )
                ctx.Dispose()
            End Using
            For Each dtMyRow As DataRowView In dvMyDati
                nList += 1
                oMyCodDescr = New ObjCodDescr
                oMyCodDescr.Id = StringOperation.FormatInt(dtMyRow("id"))
                oMyCodDescr.IdEnte = StringOperation.FormatString(dtMyRow("idente"))
                oMyCodDescr.sCodice = StringOperation.FormatString(dtMyRow("codice"))
                oMyCodDescr.sDescrizione = StringOperation.FormatString(dtMyRow("descrizione"))
                'dimensiono l'array
                ReDim Preserve oListToReturn(nList)
                'memorizzo i dati nell'array
                oListToReturn(nList) = oMyCodDescr
            Next
            HttpContext.Current.Session("dsCodDescrSort") = oListToReturn
            Return oListToReturn
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestCodDescr.GetCodDescr.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function GetCodDescr(ByVal myConnectionString As String, ByVal oMyObj As ObjCodDescr, ByVal sTabella As String, ByVal Anno As String) As ObjCodDescr()
    '    Dim oListToReturn() As ObjCodDescr = Nothing
    '    Dim oMyCodDescr As ObjCodDescr
    '    Dim nList As Integer = -1

    '    Try
    '        'Valorizzo la connessione
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0

    '        cmdMyCommand.CommandText = "SELECT TIPI.IDENTE"
    '        cmdMyCommand.CommandText += ", TIPI.CODICE, TIPI.DESCRIZIONE"
    '        If Anno <> "" Then
    '            cmdMyCommand.CommandText += ", COD.ID"
    '        Else
    '            cmdMyCommand.CommandText += ", -1 AS ID"
    '        End If
    '        cmdMyCommand.CommandText += " FROM TBL" & sTabella & " TIPI"
    '        If Anno <> "" Then
    '            If ConstSession.IsFromVariabile = "1" Then
    '                cmdMyCommand.CommandText += " INNER JOIN TBL" & sTabella.Replace("TIPO", "") & " COD ON COD.IDENTE=TIPI.IDENTE AND COD.CODICE=TIPI.CODICE AND COD.ANNO=" & Anno '*** x CMGC ***
    '            Else
    '                If sTabella.IndexOf("RIDUZ") > 1 Then
    '                    cmdMyCommand.CommandText += " INNER JOIN TBLRIDUZIONI COD ON COD.IDENTE=TIPI.IDENTE AND COD.IDRIDUZIONE=TIPI.CODICE AND COD.ANNO=" & Anno '*** x RIBES ***
    '                Else
    '                    cmdMyCommand.CommandText += " INNER JOIN TBLDETASSAZIONI COD ON COD.IDENTE=TIPI.IDENTE AND COD.IDDETASSAZIONE=TIPI.CODICE AND COD.ANNO=" & Anno '*** x RIBES ***
    '                End If
    '            End If
    '        End If
    '        cmdMyCommand.CommandText += " WHERE (TIPI.IDENTE=@IDENTE)"
    '        'VALORIZZO I PARAMETERS:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyObj.IdEnte
    '        If oMyObj.sCodice <> "" Then
    '            cmdMyCommand.CommandText += " AND (TIPI.CODICE=@CODICE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE", SqlDbType.NVarChar)).Value = oMyObj.sCodice
    '        End If
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        DrReturn = cmdMyCommand.ExecuteReader
    '        Do While DrReturn.Read
    '            nList += 1
    '            oMyCodDescr = New ObjCodDescr
    '            oMyCodDescr.Id = CInt(DrReturn("id"))
    '            oMyCodDescr.IdEnte = CStr(DrReturn("idente"))
    '            oMyCodDescr.sCodice = CStr(DrReturn("codice"))
    '            oMyCodDescr.sDescrizione = CStr(DrReturn("descrizione"))
    '            'dimensiono l'array
    '            ReDim Preserve oListToReturn(nList)
    '            'memorizzo i dati nell'array
    '            oListToReturn(nList) = oMyCodDescr
    '        Loop

    '        HttpContext.Current.Session("dsCodDescrSort") = oListToReturn
    '        Return oListToReturn
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestCodDescr.GetCodDescr.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        DrReturn.Close()
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
End Class
