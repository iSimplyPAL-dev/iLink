Imports log4net
Imports OggettiComuniStrade
''' <summary>
''' Classe per la gestione delle stampe in formato CSV/XLS
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsStampaXLS
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsStampaXLS))

    Private Function AddRowStampa(ByRef DtAddRow As DataTable, ByVal sValueRow As String) As Integer
        Dim sTextRow() As String
        Dim DrAddRow As DataRow
        Dim x As Integer = 0

        Try
            'aggiungo una nuova riga nel datarow
            DrAddRow = DtAddRow.NewRow
            'controllo se la riga e\' scritta
            If sValueRow <> "" Then
                sTextRow = sValueRow.Split(CChar("|"))
                For x = 0 To UBound(sTextRow)
                    'popolo la riga nel datarow
                    DrAddRow(x) = sTextRow(x)
                Next
            End If
            'aggiorno la riga al datatable
            DtAddRow.Rows.Add(DrAddRow)

            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.ClsStampaXLS.AddRowStampa.errore: ", Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DvDati"></param>
    ''' <param name="sIntestazioneEnte"></param>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Public Function PrintVariazioniTributi(ByVal DvDati As DataView, ByVal sIntestazioneEnte As String, nCampi As Integer) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 0 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next

            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Elenco Variazioni Tributarie"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = ""
            sDatiStampa += "Tributo|Foglio|Numero|Subalterno|Operatore|Data|Causale"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For Each myRow As DataRowView In DvDati
                sDatiStampa = ""
                sDatiStampa += Utility.StringOperation.FormatString(myRow("TRIBUTO"))
                sDatiStampa += "|"
                sDatiStampa += Utility.StringOperation.FormatString(myRow("FOGLIO"))
                sDatiStampa += "|"
                sDatiStampa += Utility.StringOperation.FormatString(myRow("NUMERO"))
                sDatiStampa += "|"
                sDatiStampa += Utility.StringOperation.FormatString(myRow("SUBALTERNO"))
                sDatiStampa += "|"
                sDatiStampa += Utility.StringOperation.FormatString(myRow("OPERATORE"))
                sDatiStampa += "|"
                sDatiStampa += Utility.StringOperation.FormatString(myRow("DATAVARIAZIONE_V"))
                sDatiStampa += "|"
                sDatiStampa += Utility.StringOperation.FormatString(myRow("CAUSALE"))
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next

            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.ClsStampaXLS.PrintVariazioniTributi.errore: ", Err)
            Return Nothing
        End Try
    End Function
    Public Function PrintComuni(ByVal ListItems() As OggettoEnte, nCampi As Integer) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 0 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next

            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = "Elenco Comuni"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = ""
            sDatiStampa += "Denominazione|Provincia|CAP|Belfiore|Istat"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For Each myRow As OggettoEnte In ListItems
                sDatiStampa = ""
                sDatiStampa += myRow.Denominazione
                sDatiStampa += "|"
                sDatiStampa += myRow.Provincia
                sDatiStampa += "|"
                sDatiStampa += myRow.Cap
                sDatiStampa += "|"
                sDatiStampa += myRow.CodBelfiore
                sDatiStampa += "|"
                sDatiStampa += myRow.CodIstat
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next

            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.ClsStampaXLS.PrintComuni.errore: ", Err)
            Return Nothing
        End Try
    End Function
End Class

''' <summary>
''' Definizione oggetto ricerca variazioni tributarie
''' </summary>
Public Class objVariazioniTributiSearch
    Private _idente As String = ""
    Private _idvariazione As Integer = -1
    Private _tributo As String = ""
    Private _foglio As String = ""
    Private _numero As String = ""
    Private _sub As String = ""
    Private _dal As DateTime = Date.MaxValue
    Private _al As DateTime = Date.MaxValue
    Private _operatore As String = ""
    Private _idcausale As Integer = -1
    Private _istrattato As Integer = -1 'Non definito=-1,non trattato=0,trattato=1

    Public Property IdEnte() As String
        Get
            Return _idente
        End Get
        Set(ByVal value As String)
            _idente = value
        End Set
    End Property
    Public Property IdVariazione() As Integer
        Get
            Return _idvariazione
        End Get
        Set(ByVal value As Integer)
            _idvariazione = value
        End Set
    End Property
    Public Property Tributo() As String
        Get
            Return _tributo
        End Get
        Set(ByVal value As String)
            _tributo = value
        End Set
    End Property
    Public Property Foglio() As String
        Get
            Return _foglio
        End Get
        Set(ByVal value As String)
            _foglio = value
        End Set
    End Property
    Public Property Numero() As String
        Get
            Return _numero
        End Get
        Set(ByVal value As String)
            _numero = value
        End Set
    End Property
    Public Property Subalterno() As String
        Get
            Return _sub
        End Get
        Set(ByVal value As String)
            _sub = value
        End Set
    End Property
    Public Property Dal() As DateTime
        Get
            Return _dal
        End Get
        Set(ByVal value As DateTime)
            _dal = value
        End Set
    End Property
    Public Property Al() As DateTime
        Get
            Return _al
        End Get
        Set(ByVal value As DateTime)
            _al = value
        End Set
    End Property
    Public Property Operatore() As String
        Get
            Return _operatore
        End Get
        Set(ByVal value As String)
            _operatore = value
        End Set
    End Property
    Public Property IdCausale() As Integer
        Get
            Return _idcausale
        End Get
        Set(ByVal value As Integer)
            _idcausale = value
        End Set
    End Property
    Public Property IsTrattato() As Integer
        Get
            Return _istrattato
        End Get
        Set(ByVal value As Integer)
            _istrattato = value
        End Set
    End Property
End Class
''' <summary>
''' Classe gestione variazione tributi
''' </summary>
Public Class MetodiVariazioniTributi
    Inherits Ribes.OPENgov.Utilities.ClsDatabase
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(MetodiVariazioniTributi))
    Public Const VariazioneSuClass As String = "CLASS"
    Public Const VariazioneSuUI As String = "UI"

    Public Shared Function SearchVariazioniTributi(ByVal SearchParams As objVariazioniTributiSearch, ByVal sMyConn As String) As DataView
        Dim objDAO As New ClsDB
        Try
            Return objDAO.SearchVariazioniTributi(SearchParams, sMyConn)
        Catch ex As Exception
            Log.Debug(("MetodiVariazioniTributi::SearchVariazioniTributi::si è verificato il seguente errore::" + ex.Message))
            Throw ex
        End Try
    End Function

#Region "Automatismo inserimento in Territorio - Select"
    Public Function GetFabToTerr(IdVariazione As Integer, sMyConn As String) As ObjFabbricato
        Dim FncDB As New ClsDB
        Dim oMyFab As New ObjFabbricato
        Dim MyRow As DataRow

        Try
            Dim MyDataTable As DataTable = FncDB.GetFabToInsert(IdVariazione, sMyConn, "prc_GetVariazioniTributiFabToTer")
            If Not MyDataTable Is Nothing Then
                For Each MyRow In MyDataTable.Rows
                    If Not IsDBNull(MyRow.Item("idente")) Then
                        oMyFab.sIdEnte = MyRow.Item("idente")
                    End If
                    If Not IsDBNull(MyRow.Item("civico")) Then
                        oMyFab.nNumeroCivico = MyRow.Item("civico")
                    End If
                    If Not IsDBNull(MyRow.Item("codvia")) Then
                        oMyFab.nCodVia = MyRow.Item("codvia")
                    End If
                    If Not IsDBNull(MyRow.Item("nomefabbricato")) Then
                        oMyFab.sNomeFabbricato = MyRow.Item("nomefabbricato")
                    End If
                    If Not IsDBNull(MyRow.Item("codcategoriacatastale")) Then
                        oMyFab.sCatCatastale = MyRow.Item("codcategoriacatastale")
                    End If
                    oMyFab.oListUnitaImmobiliari = GetUIToTerr(IdVariazione, sMyConn)
                Next
            End If
            Return oMyFab
        Catch ex As Exception
            Log.Debug(("MetodiVariazioniTributi::GetFabToTerr::si è verificato il seguente errore::" + ex.Message))
            Return Nothing
        End Try
    End Function
    Public Shared Function GetUIToTerr(IdVariazione As Integer, sMyConn As String) As ObjUnitaImmobiliare()
        Dim FncDB As New ClsDB
        Dim oMyUI As New ObjUnitaImmobiliare
        Dim myArray As New ArrayList
        Dim MyRow As DataRow

        Try
            Dim MyDataTable As DataTable = FncDB.GetFabToInsert(IdVariazione, sMyConn, "prc_GetVariazioniTributiUIToTer")
            If Not MyDataTable Is Nothing Then
                For Each MyRow In MyDataTable.Rows
                    If Not IsDBNull(MyRow.Item("idente")) Then
                        oMyUI.sIdEnte = MyRow.Item("idente")
                    End If
                    If Not IsDBNull(MyRow.Item("foglio")) Then
                        oMyUI.nFoglio = MyRow.Item("foglio")
                    End If
                    If Not IsDBNull(MyRow.Item("numero")) Then
                        oMyUI.sNumero = MyRow.Item("numero")
                    End If
                    If Not IsDBNull(MyRow.Item("sub")) Then
                        oMyUI.nSubalterno = MyRow.Item("sub")
                    End If
                    If Not IsDBNull(MyRow.Item("interno")) Then
                        oMyUI.sInterno = MyRow.Item("interno")
                    End If
                    If Not IsDBNull(MyRow.Item("datainizio")) Then
                        oMyUI.dDal = MyRow.Item("datainizio")
                    End If
                    If Not IsDBNull(MyRow.Item("datafine")) Then
                        oMyUI.dAl = MyRow.Item("datafine")
                    End If
                    oMyUI.oListClassificazioni = GetClasToTerr(IdVariazione, sMyConn)
                    myArray.Add(oMyUI)
                Next
            End If
            Return CType(myArray.ToArray(GetType(ObjUnitaImmobiliare)), ObjUnitaImmobiliare())
        Catch ex As Exception
            Log.Debug(("MetodiVariazioniTributi::GetUIToTerr::si è verificato il seguente errore::" + ex.Message))
            Return Nothing
        End Try
    End Function
    Public Shared Function GetClasToTerr(IdVariazione As Integer, sMyConn As String) As ObjClassificazione()
        Dim FncDB As New ClsDB
        Dim oMyClas As New ObjClassificazione
        Dim myArray As New ArrayList
        Dim MyRow As DataRow

        Try
            Dim MyDataTable As DataTable = FncDB.GetFabToInsert(IdVariazione, sMyConn, "prc_GetVariazioniTributiClasToTer")
            If Not MyDataTable Is Nothing Then
                For Each MyRow In MyDataTable.Rows
                    If Not IsDBNull(MyRow.Item("codcategoriacatastale")) Then
                        oMyClas.sCodCategoriaCatastale = MyRow.Item("codcategoriacatastale")
                    End If
                    If Not IsDBNull(MyRow.Item("codclasse")) Then
                        oMyClas.sCodClasse = MyRow.Item("codclasse")
                    End If
                    If Not IsDBNull(MyRow.Item("codrendita")) Then
                        oMyClas.nCodTipoRendita = MyRow.Item("codrendita")
                    End If
                    If Not IsDBNull(MyRow.Item("consistenza")) Then
                        oMyClas.nConsistenza = MyRow.Item("consistenza")
                    End If
                    If Not IsDBNull(MyRow.Item("valoreimmobile")) Then
                        oMyClas.sValoreRendita = MyRow.Item("valoreimmobile")
                    End If
                    If Not IsDBNull(MyRow.Item("datainizio")) Then
                        oMyClas.dDal = MyRow.Item("datainizio")
                    End If
                    If Not IsDBNull(MyRow.Item("datafine")) Then
                        oMyClas.dAl = MyRow.Item("datafine")
                    End If
                    If Not IsDBNull(MyRow.Item("mqcatasto")) Then
                        oMyClas.nSuperficieCatastale = MyRow.Item("mqcatasto")
                    End If
                    If Not IsDBNull(MyRow.Item("mqtassabili")) Then
                        oMyClas.nSuperficieNetta = MyRow.Item("mqtassabili")
                    End If
                    If Not IsDBNull(MyRow.Item("mqlordo")) Then
                        oMyClas.nSuperficieLorda = MyRow.Item("mqlordo")
                    End If
                    oMyClas.oListVani = GetVaniToTerr(IdVariazione, sMyConn)
                    oMyClas.oListProprieta = GetPropToTerr(IdVariazione, sMyConn)
                    oMyClas.oListConduzioni = GetCondToTerr(IdVariazione, sMyConn)
                    myArray.Add(oMyClas)
                Next
            End If
            Return CType(myArray.ToArray(GetType(ObjClassificazione)), ObjClassificazione())
        Catch ex As Exception
            Log.Debug(("MetodiVariazioniTributi::GetClasToTerr::si è verificato il seguente errore::" + ex.Message))
            Return Nothing
        End Try
    End Function
    Public Shared Function GetVaniToTerr(IdVariazione As Integer, sMyConn As String) As ObjVano()
        Dim FncDB As New ClsDB
        Dim oMyVano As New ObjVano
        Dim myArray As New ArrayList
        Dim MyRow As DataRow

        Try
            Dim MyDataTable As DataTable = FncDB.GetFabToInsert(IdVariazione, sMyConn, "prc_GetVariazioniTributiVaniToTer")
            If Not MyDataTable Is Nothing Then
                For Each MyRow In MyDataTable.Rows
                    If Not IsDBNull(MyRow.Item("mq")) Then
                        oMyVano.nMQ = MyRow.Item("mq")
                    End If
                    If Not IsDBNull(MyRow.Item("idtipovano")) Then
                        oMyVano.nTipoVano = MyRow.Item("idtipovano")
                    End If
                    If Not IsDBNull(MyRow.Item("peso_catastale")) Then
                        oMyVano.nPesoCat = MyRow.Item("peso_catastale")
                    End If
                    myArray.Add(oMyVano)
                Next
            End If
            Return CType(myArray.ToArray(GetType(ObjVano)), ObjVano())
        Catch ex As Exception
            Log.Debug(("MetodiVariazioniTributi::GetVaniToTerr::si è verificato il seguente errore::" + ex.Message))
            Return Nothing
        End Try
    End Function
    Public Shared Function GetPropToTerr(IdVariazione As Integer, sMyConn As String) As ObjProprieta()
        Dim FncDB As New ClsDB
        Dim oMyProprieta As New ObjProprieta
        Dim myArray As New ArrayList
        Dim MyRow As DataRow

        Try
            Dim MyDataTable As DataTable = FncDB.GetFabToInsert(IdVariazione, sMyConn, "prc_GetVariazioniTributiPropToTer")
            If Not MyDataTable Is Nothing Then
                For Each MyRow In MyDataTable.Rows
                    If Not IsDBNull(MyRow.Item("cod_contribuente")) Then
                        oMyProprieta.nIdAnagrafica = MyRow.Item("cod_contribuente")
                    End If
                    If Not IsDBNull(MyRow.Item("datainizio")) Then
                        oMyProprieta.dDataInizio = MyRow.Item("datainizio")
                    End If
                    If Not IsDBNull(MyRow.Item("datafine")) Then
                        oMyProprieta.dDataFine = MyRow.Item("datafine")
                    End If
                    If Not IsDBNull(MyRow.Item("percpossesso")) Then
                        oMyProprieta.nPercentProprieta = MyRow.Item("percpossesso")
                    End If
                    myArray.Add(oMyProprieta)
                Next
            End If
            Return CType(myArray.ToArray(GetType(ObjProprieta)), ObjProprieta())
        Catch ex As Exception
            Log.Debug(("MetodiVariazioniTributi::GetPropToTerr::si è verificato il seguente errore::" + ex.Message))
            Return Nothing
        End Try
    End Function
    Public Shared Function GetCondToTerr(IdVariazione As Integer, sMyConn As String) As ObjConduzione()
        Dim FncDB As New ClsDB
        Dim oMyConduzione As New ObjConduzione
        Dim myArray As New ArrayList
        Dim MyRow As DataRow

        Try
            Dim MyDataTable As DataTable = FncDB.GetFabToInsert(IdVariazione, sMyConn, "prc_GetVariazioniTributiCondToTer")
            If Not MyDataTable Is Nothing Then
                For Each MyRow In MyDataTable.Rows
                    If Not IsDBNull(MyRow.Item("cod_contribuente")) Then
                        oMyConduzione.nIdAnagrafica = MyRow.Item("cod_contribuente")
                    End If
                    If Not IsDBNull(MyRow.Item("datainizio")) Then
                        oMyConduzione.dDataInizio = MyRow.Item("datainizio")
                    End If
                    If Not IsDBNull(MyRow.Item("datafine")) Then
                        oMyConduzione.dDataFine = MyRow.Item("datafine")
                    End If
                    If Not IsDBNull(MyRow.Item("ncomponenti")) Then
                        oMyConduzione.nOccupanti = MyRow.Item("ncomponenti")
                    End If
                    myArray.Add(oMyConduzione)
                Next
            End If
            Return CType(myArray.ToArray(GetType(ObjConduzione)), ObjConduzione())
        Catch ex As Exception
            Log.Debug(("MetodiVariazioniTributi::GetCondToTerr::si è verificato il seguente errore::" + ex.Message))
            Return Nothing
        End Try
    End Function
#End Region
#Region "Automatismo inserimento in Territorio - Insert"
    Public Function SetFabToTerr(sMyConn As String, oMyFab As ObjFabbricato) As Boolean
        Dim FncDB As New ClsDB
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myTransact As SqlClient.SqlTransaction
        Dim bCommit As Boolean = True

        Try
            Connect(sMyConn)
            cmdMyCommand = CreateCommand()
            myTransact = SqlConnection.BeginTransaction("SampleTransaction")
            cmdMyCommand.Transaction = myTransact

            oMyFab.nIdFabbricato = FncDB.SetFabbricato(sMyConn, oMyFab, cmdMyCommand)
            If oMyFab.nIdFabbricato > 0 Then
                For Each oMyUI As ObjUnitaImmobiliare In oMyFab.oListUnitaImmobiliari
                    oMyUI.nIdFabbricato = oMyFab.nIdFabbricato
                    oMyUI.nIdUnitaImmobiliare = FncDB.SetUI(sMyConn, oMyUI, cmdMyCommand)
                    If oMyUI.nIdUnitaImmobiliare > 0 Then
                        For Each oMyClas As ObjClassificazione In oMyUI.oListClassificazioni
                            oMyClas.nIdUI = oMyUI.nIdUnitaImmobiliare
                            oMyClas.nIdClassificazione = FncDB.SetClassificazione(sMyConn, oMyClas, cmdMyCommand)
                            If oMyClas.nIdClassificazione > 0 Then
                                For Each oMyVano As ObjVano In oMyClas.oListVani
                                    oMyVano.nIdClassificazione = oMyClas.nIdClassificazione
                                    oMyVano.nIdVano = FncDB.SetVano(sMyConn, oMyVano, cmdMyCommand)
                                    If oMyVano.nIdVano <= 0 Then
                                        bCommit = False
                                    End If
                                Next
                                For Each oMyProprieta As ObjProprieta In oMyClas.oListProprieta
                                    oMyProprieta.nIdClassificazione = oMyClas.nIdClassificazione
                                    oMyProprieta.nIdProprieta = FncDB.SetProprieta(sMyConn, oMyProprieta, cmdMyCommand, New TextBox)
                                    If oMyProprieta.nIdProprieta <= 0 Then
                                        bCommit = False
                                    End If
                                Next
                                For Each oMyConduzione As ObjConduzione In oMyClas.oListConduzioni
                                    oMyConduzione.nIdClassificazione = oMyClas.nIdClassificazione
                                    oMyConduzione.nIdConduzione = FncDB.SetConduzione(sMyConn, oMyConduzione, cmdMyCommand, New TextBox)
                                    If oMyConduzione.nIdConduzione <= 0 Then
                                        bCommit = False
                                    End If
                                Next
                            Else
                                bCommit = False
                            End If
                        Next
                    Else
                        bCommit = False
                    End If
                Next
            Else
                bCommit = False
            End If
            If bCommit Then
                myTransact.Commit()
            Else
                myTransact.Rollback()
            End If
            Return True
        Catch ex As Exception
            Log.Debug(("MetodiVariazioniTributi::SetFabToTerr::si è verificato il seguente errore::" + ex.Message))
            myTransact.Rollback()
            Return False
        Finally
            Disconnect(cmdMyCommand)
        End Try
    End Function
#End Region
End Class
