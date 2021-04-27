Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Anagrafica
Imports System.Web.HttpContext
'Imports OPENUtility
Imports log4net
'Imports RIBESFrameWork

Namespace ANAGRAFICAWEB

    ''' -----------------------------------------------------------------------------
    ''' Project	 : Anagrafica
    ''' Class	 : OrderDetails
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    ''' <history>
    ''' 	[antonello]	24/01/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class ListAnagrafica
        Public p_dsItemsANAGRAFICA As DataSet = Nothing
        Public p_daItemsANAGRAFICA As SqlDataAdapter = Nothing
    End Class
    ''' -----------------------------------------------------------------------------
    ''' Project	 : Anagrafica
    ''' Class	 : AnagraficaDB
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    ''' <history>
    ''' 	[antonello]	24/01/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class AnagraficaDB
        Private Log As ILog = LogManager.GetLogger(GetType(AnagraficaDB))
        Private m_ParametroRicerca As String = ""
        Private cmdMyCommand As New SqlCommand

#Region "RIBESFRAMEWORK"
        'Public Function GetListAnagragrafica(ByVal WFSessione As CreateSessione, ByVal strCognome As String, ByVal strNome As String, ByVal strCodiceFiscale As String, ByVal strPartitaIva As String, ByVal strCodEnte As String, ByVal strCODContribuente As String, ByVal blnDaRicontrollare As Boolean, ByVal strComuneResidenza As String, ByVal strProvinciaResidenza As String, ByVal strDataNascita As String, ByVal strDataMorte As String, ByVal strVia As String, ByVal strCodVia As String, ByVal blnNonAgganciato As Boolean, ByVal nSearchContatti As Integer) As ListAnagrafica
        '    Try
        '        Log.Debug("GetListAnagragrafica:: inizio")
        '        Dim intCount As Integer
        '        Dim intParam As Integer
        '        Dim intCODContribuente As Integer = -1
        '        Dim param As New SqlParameter
        '        Dim paramsCollection As New Collection
        '        Dim arrayParam() As SqlParameter
        '        Dim objDBAccess As New DBManager
        '        Dim objConn As New SqlConnection
        '        Dim objCommand As New SqlCommand
        '        Dim SqlDataAdapter As New SqlDataAdapter
        '        Dim dsListAnagrafica As New DataSet
        '        Dim intDaricontrollareSp As Integer
        '        Dim objOPENUtility As New OPENUtility.ModificaDate

        '        Dim intNonAgganciate As Integer

        '        Dim strWFErrore As String
        '        Log.Debug("GetListAnagragrafica::devo aprire wf")
        '        Log.Debug("GetListAnagragrafica::parametroenv::" & ConstSession.ParametroEnv)
        '        Log.Debug("GetListAnagragrafica::username::" & ConstSession.UserName)
        '        Log.Debug("GetListAnagragrafica::IDENTIFICATIVOAPPLICAZIONE::" & ConstSession.IdentificativoApplicazione)
        '        WFSessione = New CreateSessione(ConstSession.UserName, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
        '        If Not WFSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
        '        End If
        '        Log.Debug("GetListAnagragrafica::aperto")
        '        If blnDaRicontrollare Then
        '            intDaricontrollareSp = 1
        '        Else
        '            intDaricontrollareSp = 0
        '        End If
        '        '*** Fabiana 30082007
        '        If blnNonAgganciato Then
        '            intNonAgganciate = 1
        '        Else
        '            intNonAgganciate = 0
        '        End If
        '        If IsNumeric(strCODContribuente) Then
        '            intCODContribuente = CInt(strCODContribuente)
        '        End If
        '        param = New SqlParameter("@CodContribuente", SqlDbType.NVarChar, 50)
        '        param.Value = strCODContribuente
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@DaRicontrollare", SqlDbType.NVarChar, 50)
        '        param.Value = CStr(intDaricontrollareSp)
        '        paramsCollection.Add(param)


        '        param = New SqlParameter("@ParametroRicerca", SqlDbType.NVarChar, 250)
        '        param.Value = m_ParametroRicerca
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@Cognome", SqlDbType.NVarChar, 100)
        '        param.Value = strCognome
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@Nome", SqlDbType.NVarChar, 50)
        '        param.Value = strNome
        '        paramsCollection.Add(param)


        '        param = New SqlParameter("@CodiceFiscale", SqlDbType.NVarChar, 16)
        '        param.Value = strCodiceFiscale
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@PartitaIVA", SqlDbType.NVarChar, 11)
        '        param.Value = strPartitaIva
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@CodEnte", SqlDbType.NVarChar, 20)
        '        param.Value = strCodEnte
        '        paramsCollection.Add(param)
        '        param = New SqlParameter("@Da", SqlDbType.NVarChar, 50)
        '        param.Value = ""
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@A", SqlDbType.NVarChar, 50)
        '        param.Value = ""
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@Via", SqlDbType.NVarChar, 50)
        '        param.Value = strVia
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@CodVia", SqlDbType.NVarChar, 20)
        '        param.Value = strCodVia
        '        paramsCollection.Add(param)

        '        '*** Fabiana 30082007
        '        param = New SqlParameter("@NonAgganciate", SqlDbType.NVarChar, 50)
        '        param.Value = CStr(intNonAgganciate)
        '        paramsCollection.Add(param)

        '        '*** Dipe 16/01/2008
        '        param = New SqlParameter("@ComuneResidenza", SqlDbType.NVarChar, 50)
        '        param.Value = strComuneResidenza
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@ProvinciaResidenza", SqlDbType.NVarChar, 2)
        '        param.Value = strProvinciaResidenza
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@DataNascita", SqlDbType.NVarChar, 10)
        '        param.Value = objOPENUtility.GiraData(strDataNascita)
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@DataMorte", SqlDbType.NVarChar, 10)
        '        param.Value = objOPENUtility.GiraData(strDataMorte)
        '        paramsCollection.Add(param)

        '        intParam = paramsCollection.Count
        '        ReDim arrayParam(intParam - 1)
        '        For intCount = 0 To intParam - 1
        '            arrayParam(intCount) = paramsCollection(intCount + 1)
        '        Next
        '        Log.Debug("GetListAnagragrafica::cambio connessione su anagrafica::" & ConstSession.IdentificativoApplicazione)
        '        objDBAccess = WFSessione.oSession.GetPrivateDBManager(ConstSession.IdentificativoApplicazione)
        '        Log.Debug("GetListAnagragrafica::cambiato connessione")
        '        objDBAccess.GetPrivateRunSPForRibesDataGrid("sp_RicercaPersoneStradario", objConn, objCommand, arrayParam)
        '        Log.Debug("GetListAnagragrafica:: eseguito stored sp_RicercaPersoneStradario")
        '        SqlDataAdapter = objDBAccess.GetPrivateDataAdapter(objCommand)
        '        SqlDataAdapter.Fill(dsListAnagrafica, "ANAGRAFICA")

        '        Dim ListAnagrafica As New ListAnagrafica
        '        ListAnagrafica.p_daItemsANAGRAFICA = SqlDataAdapter
        '        ListAnagrafica.p_dsItemsANAGRAFICA = dsListAnagrafica
        '        Return ListAnagrafica
        '    Catch Err As Exception
        '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Anagrafica.AnagraficaDB.GetListAnagrafica.errore: ", Err)
        '        Throw New Exception
        '    End Try
        'End Function

        'Public Function GetListAnagragraficaStampaExcel(ByVal WFSessione As CreateSessione, ByVal strCognome As String, ByVal strNome As String, ByVal strCodiceFiscale As String, ByVal strPartitaIva As String, ByVal strCodEnte As String, ByVal strCODContribuente As String, ByVal blnDaRicontrollare As Boolean, ByVal strComuneResidenza As String, ByVal strProvinciaResidenza As String, ByVal strDataNascita As String, ByVal strDataMorte As String, Optional ByVal strViaResidenza As String = "", Optional ByVal strCodViaResidenza As String = "", Optional ByVal blnNonAgganciato As Boolean = False) As ListAnagrafica
        '    Try
        '        Dim intCount As Integer
        '        Dim intParam As Integer
        '        Dim intCODContribuente As Integer = -1
        '        Dim param As New SqlParameter
        '        Dim paramsCollection As New Collection
        '        Dim arrayParam() As SqlParameter
        '        Dim objDBAccess As New DBManager
        '        Dim objConn As New SqlConnection
        '        Dim objCommand As New SqlCommand
        '        Dim SqlDataAdapter As New SqlDataAdapter
        '        Dim dsListAnagrafica As New DataSet
        '        Dim intDaricontrollareSp As Integer
        '        Dim intNonAgganciate As Integer

        '        Dim strWFErrore As String
        '        WFSessione = New CreateSessione(ConstSession.UserName, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
        '        If Not WFSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
        '        End If

        '        If blnDaRicontrollare Then
        '            intDaricontrollareSp = 1
        '        Else
        '            intDaricontrollareSp = 0
        '        End If

        '        '*** Fabiana 30082007 
        '        If blnNonAgganciato Then
        '            intNonAgganciate = 1
        '        Else
        '            intNonAgganciate = 0
        '        End If

        '        If IsNumeric(strCODContribuente) Then
        '            intCODContribuente = CInt(strCODContribuente)
        '        End If
        '        param = New SqlParameter("@CodContribuente", SqlDbType.NVarChar, 50)
        '        param.Value = strCODContribuente
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@DaRicontrollare", SqlDbType.NVarChar, 50)
        '        param.Value = CStr(intDaricontrollareSp)
        '        paramsCollection.Add(param)


        '        param = New SqlParameter("@ParametroRicerca", SqlDbType.NVarChar, 250)
        '        param.Value = m_ParametroRicerca
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@Cognome", SqlDbType.NVarChar, 100)
        '        param.Value = strCognome
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@Nome", SqlDbType.NVarChar, 50)
        '        param.Value = strNome
        '        paramsCollection.Add(param)


        '        param = New SqlParameter("@CodiceFiscale", SqlDbType.NVarChar, 16)
        '        param.Value = strCodiceFiscale
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@PartitaIVA", SqlDbType.NVarChar, 11)
        '        param.Value = strPartitaIva
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@CodEnte", SqlDbType.NVarChar, 20)
        '        param.Value = strCodEnte
        '        paramsCollection.Add(param)
        '        param = New SqlParameter("@Da", SqlDbType.NVarChar, 50)
        '        param.Value = ""
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@A", SqlDbType.NVarChar, 50)
        '        param.Value = ""
        '        paramsCollection.Add(param)


        '        param = New SqlParameter("@Via", SqlDbType.NVarChar, 50)
        '        param.Value = strViaResidenza
        '        paramsCollection.Add(param)

        '        param = New SqlParameter("@CodVia", SqlDbType.NVarChar, 20)
        '        param.Value = strCodViaResidenza
        '        paramsCollection.Add(param)

        '        '***Fabiana 30082007
        '        param = New SqlParameter("@NonAgganciate", SqlDbType.NVarChar, 50)
        '        param.Value = CStr(intNonAgganciate)
        '        paramsCollection.Add(param)

        '        intParam = paramsCollection.Count
        '        ReDim arrayParam(intParam - 1)
        '        For intCount = 0 To intParam - 1
        '            arrayParam(intCount) = paramsCollection(intCount + 1)
        '        Next

        '        objDBAccess = WFSessione.oSession.GetPrivateDBManager(ConstSession.IdentificativoApplicazione)
        '        objDBAccess.GetPrivateRunSPForRibesDataGrid("sp_RicercaPersoneStampaExcel", objConn, objCommand, arrayParam)

        '        SqlDataAdapter = objDBAccess.GetPrivateDataAdapter(objCommand)
        '        SqlDataAdapter.Fill(dsListAnagrafica, "ANAGRAFICA")

        '        Dim ListAnagrafica As New ListAnagrafica
        '        ListAnagrafica.p_daItemsANAGRAFICA = SqlDataAdapter
        '        ListAnagrafica.p_dsItemsANAGRAFICA = dsListAnagrafica
        '        Return ListAnagrafica

        '    Catch Err As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Anagrafica.AnagraficaDB.GetListAnagraficaStampaExcel.errore: ", Err)
        '        Throw New Exception
        '    End Try
        'End Function
#End Region
#Region "NO RIBESFRAMEWORK"
        ''' <summary>
        ''' Funzione che restituisce l'elenco delle anagrafiche rispondenti ai criteri impostati
        ''' </summary>
        ''' <param name="StringConnection"></param>
        ''' <param name="bIsStampa"></param>
        ''' <param name="sCognome"></param>
        ''' <param name="sNome"></param>
        ''' <param name="sCodiceFiscale"></param>
        ''' <param name="sPartitaIva"></param>
        ''' <param name="sAmbiente"></param>
        ''' <param name="sCodEnte"></param>
        ''' <param name="sCodContribuente"></param>
        ''' <param name="bDaRicontrollare"></param>
        ''' <param name="sComuneResidenza"></param>
        ''' <param name="sProvinciaResidenza"></param>
        ''' <param name="sDataNascita"></param>
        ''' <param name="sDataMorte"></param>
        ''' <param name="sVia"></param>
        ''' <param name="sCodVia"></param>
        ''' <param name="bNonAgganciato"></param>
        ''' <param name="nSearchContatti"></param>
        ''' <param name="nSearchInvio"></param>
        ''' <param name="nSearchModulo"></param>
        ''' <returns></returns>
        ''' <revisionHistory>
        ''' <revision date="14/05/2014">
        ''' cambiata la connessione
        ''' </revision>
        ''' </revisionHistory>
        ''' <revisionHistory>
        ''' <revision date="12/04/2019">
        ''' <strong>Qualificazione AgID-analisi_rel01</strong>
        ''' <em>Esportazione completa dati</em>
        ''' </revision>
        ''' </revisionHistory>
        Public Function GetListAnagragrafica(StringConnection As String, ByVal bIsStampa As Boolean, ByVal sCognome As String, ByVal sNome As String, ByVal sCodiceFiscale As String, ByVal sPartitaIva As String, sAmbiente As String, ByVal sCodEnte As String, ByVal sCodContribuente As String, ByVal bDaRicontrollare As Boolean, ByVal sComuneResidenza As String, ByVal sProvinciaResidenza As String, ByVal sDataNascita As String, ByVal sDataMorte As String, ByVal sVia As String, ByVal sCodVia As String, ByVal bNonAgganciato As Boolean, ByVal nSearchContatti As Integer, ByVal nSearchInvio As Integer, ByVal nSearchModulo As Integer) As ListAnagrafica
            Dim myAdapter As New SqlDataAdapter
            Dim dsListAnagrafica As New DataSet
            Dim sMyStoreProcedure As String

            Try
                If bIsStampa Then
                    sMyStoreProcedure = "sp_RicercaPersoneStampaExcel"
                Else
                    sMyStoreProcedure = "sp_RicercaPersoneStradario"
                End If
                cmdMyCommand = ConstSession.InizializzaCmd(StringConnection)
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = sMyStoreProcedure
                cmdMyCommand.Parameters.Clear()
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Ambiente", SqlDbType.NVarChar)).Value = sAmbiente
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CodEnte", SqlDbType.NVarChar)).Value = sCodEnte
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CodContribuente", SqlDbType.NVarChar)).Value = sCodContribuente
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Cognome", SqlDbType.NVarChar)).Value = sCognome
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Nome", SqlDbType.NVarChar)).Value = sNome
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CodiceFiscale", SqlDbType.NVarChar)).Value = sCodiceFiscale
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PartitaIVA", SqlDbType.NVarChar)).Value = sPartitaIva
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Via", SqlDbType.NVarChar)).Value = sVia
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CodVia", SqlDbType.NVarChar)).Value = sCodVia
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ComuneResidenza", SqlDbType.NVarChar)).Value = sComuneResidenza
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ProvinciaResidenza", SqlDbType.NVarChar)).Value = sProvinciaResidenza
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataNascita", SqlDbType.NVarChar)).Value = sDataNascita
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataMorte", SqlDbType.NVarChar)).Value = sDataMorte
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Contatti", SqlDbType.Int)).Value = nSearchContatti
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Invio", SqlDbType.Int)).Value = nSearchInvio
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Modulo", SqlDbType.Int)).Value = nSearchModulo
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Da", SqlDbType.NVarChar)).Value = ""
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@A", SqlDbType.NVarChar)).Value = ""
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ParametroRicerca", SqlDbType.NVarChar)).Value = m_ParametroRicerca
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DaRicontrollare", SqlDbType.Int)).Value = CInt(bDaRicontrollare)
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NonAgganciate", SqlDbType.Int)).Value = CInt(bNonAgganciato)
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                myAdapter.SelectCommand = cmdMyCommand
                myAdapter.Fill(dsListAnagrafica, "ANAGRAFICA")

                Dim ListAnagrafica As New ListAnagrafica
                ListAnagrafica.p_daItemsANAGRAFICA = myAdapter
                ListAnagrafica.p_dsItemsANAGRAFICA = dsListAnagrafica
                Return ListAnagrafica
            Catch Err As Exception
                Log.Debug(sCodEnte + " - Anagrafica.AnagraficaDB.GetListAnagrafica.errore: ", Err)
                Throw New Exception
            Finally
                cmdMyCommand.Connection.Close()
                cmdMyCommand.Dispose()
            End Try
        End Function
        'Public Function GetListAnagragrafica(ByVal bIsStampa As Boolean, ByVal sCognome As String, ByVal sNome As String, ByVal sCodiceFiscale As String, ByVal sPartitaIva As String, sAmbiente As String, ByVal sCodEnte As String, ByVal sCodContribuente As String, ByVal bDaRicontrollare As Boolean, ByVal sComuneResidenza As String, ByVal sProvinciaResidenza As String, ByVal sDataNascita As String, ByVal sDataMorte As String, ByVal sVia As String, ByVal sCodVia As String, ByVal bNonAgganciato As Boolean, ByVal nSearchContatti As Integer, ByVal nSearchInvio As Integer, ByVal nSearchModulo As Integer) As ListAnagrafica
        '    Dim myAdapter As New SqlDataAdapter
        '    Dim dsListAnagrafica As New DataSet
        '    Dim sMyStoreProcedure As String

        '    Try
        '        If bIsStampa Then
        '            sMyStoreProcedure = "sp_RicercaPersoneStampaExcel"
        '        Else
        '            sMyStoreProcedure = "sp_RicercaPersoneStradario"
        '        End If
        '        '*** 20140514 cambiata la connessione ***
        '        cmdMyCommand = ConstSession.InizializzaCmd(ConstSession.StringConnectionAnagrafica)
        '        'Log.Debug("Anagrafica::Components::GetListAnagragrafica::esecuzione sp_RicercaPersoneStradario")
        '        cmdMyCommand.CommandType = CommandType.StoredProcedure
        '        cmdMyCommand.CommandText = sMyStoreProcedure
        '        cmdMyCommand.Parameters.Clear()
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Ambiente", SqlDbType.NVarChar)).Value = sAmbiente
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CodEnte", SqlDbType.NVarChar)).Value = sCodEnte
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CodContribuente", SqlDbType.NVarChar)).Value = sCodContribuente
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Cognome", SqlDbType.NVarChar)).Value = sCognome
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Nome", SqlDbType.NVarChar)).Value = sNome
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CodiceFiscale", SqlDbType.NVarChar)).Value = sCodiceFiscale
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PartitaIVA", SqlDbType.NVarChar)).Value = sPartitaIva
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Via", SqlDbType.NVarChar)).Value = sVia
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CodVia", SqlDbType.NVarChar)).Value = sCodVia
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ComuneResidenza", SqlDbType.NVarChar)).Value = sComuneResidenza
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ProvinciaResidenza", SqlDbType.NVarChar)).Value = sProvinciaResidenza
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataNascita", SqlDbType.NVarChar)).Value = sDataNascita
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataMorte", SqlDbType.NVarChar)).Value = sDataMorte
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Contatti", SqlDbType.Int)).Value = nSearchContatti
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Invio", SqlDbType.Int)).Value = nSearchInvio
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Modulo", SqlDbType.Int)).Value = nSearchModulo
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Da", SqlDbType.NVarChar)).Value = ""
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@A", SqlDbType.NVarChar)).Value = ""
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ParametroRicerca", SqlDbType.NVarChar)).Value = m_ParametroRicerca
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DaRicontrollare", SqlDbType.Int)).Value = CInt(bDaRicontrollare)
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NonAgganciate", SqlDbType.Int)).Value = CInt(bNonAgganciato)
        '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
        '        myAdapter.SelectCommand = cmdMyCommand
        '        myAdapter.Fill(dsListAnagrafica, "ANAGRAFICA")

        '        Dim ListAnagrafica As New ListAnagrafica
        '        ListAnagrafica.p_daItemsANAGRAFICA = myAdapter
        '        ListAnagrafica.p_dsItemsANAGRAFICA = dsListAnagrafica
        '        Return ListAnagrafica
        '    Catch Err As Exception
        '        Log.Debug(sCodEnte + " - Anagrafica.AnagraficaDB.GetListAnagrafica.errore: ", Err)
        '        Throw New Exception
        '    Finally
        '        cmdMyCommand.Connection.Close()
        '        cmdMyCommand.Dispose()
        '    End Try
        'End Function
#End Region
        Public Sub SelectIndexDropDownList(ByVal cboTemp As DropDownList, ByVal strValue As String)

            Dim blnFindElement As Boolean = False
            Dim intCount As Integer = 1
            Dim intNumberElements As Integer = cboTemp.Items.Count
            Try
                Do While intCount < intNumberElements
                    cboTemp.SelectedIndex = intCount
                    If cboTemp.SelectedItem.Value = strValue Then
                        cboTemp.SelectedItem.Text = cboTemp.Items(intCount).Text
                        blnFindElement = True
                        Exit Do
                    End If
                    intCount = intCount + 1
                Loop
                If Not blnFindElement Then cboTemp.SelectedIndex = "-1"
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Anagrafica.SelectIndexDropDownList.errore: ", ex)
            End Try
        End Sub
        Public Function RetDataView(ByVal DataSet As DataSet) As DataView
            Dim View As New DataView
            Try
                Dim Table As DataTable = DataSet.Tables("CONTATTI")
                View = Table.DefaultView
                View.Sort = "TipoRiferimento"
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.BAsePage.DataView.errore: ", ex)
            End Try
            Return View
        End Function

        Public Sub New(ByVal strParametroRicerca As String)
            m_ParametroRicerca = strParametroRicerca
        End Sub
        Public Sub New()
        End Sub
    End Class
End Namespace
