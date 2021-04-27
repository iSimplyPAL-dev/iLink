Imports log4net
Imports Utility

#Region "Interrogazioni Generali Utenti"
''' <summary>
''' Definizione oggetto ricerca interrogazioni generali
''' </summary>
Public Class ObjInterGenSearch
    Public Const Sog_ALL As Integer = -1
    Public Const Sog_NoRes As Integer = 0
    Public Const Sog_Res As Integer = 1
    Private _IdEnte As String = ""
    Private _Dal As DateTime = Date.MaxValue
    Private _Al As DateTime = Date.MaxValue
    Private _sCognome As String = ""
    Private _sNome As String = ""
    Private _sCF As String = ""
    Private _sPIVA As String = ""
    Private _sNTessera As String = ""
    Private _sVia As String = ""
    Private _sCivico As String = ""
    Private _sInterno As String = ""
    Private _sFoglio As String = ""
    Private _sNumero As String = ""
    Private _sSubalterno As String = ""
    Private _Chiusa As Integer = 0
    Private _rbSoggetto As Boolean = False
    Private _rbImmobile As Boolean = False
    Private _provenienza As String = ""
    Private _catcatastale As String = ""
    Private _typesogres As Integer = Sog_ALL
    Private _idcatTARES As Integer = -1
    Private _nc As Integer = -1
    Private _isPF As Boolean = False
    Private _isPV As Boolean = False
    Private _isEsente As Boolean = False
    Private _idrid As String = ""
    Private _iddet As String = ""
    Private _moreUI As Boolean = False
    Private _idstatooccup As String = ""
    Private _TipoConf As Integer = -1
    Private _ConfDal As DateTime = Date.MaxValue
    Private _ConfAl As DateTime = Date.MaxValue

    Public Property IdEnte() As String
        Get
            Return _IdEnte
        End Get
        Set(ByVal Value As String)
            _IdEnte = Value
        End Set
    End Property
    Public Property Dal() As DateTime
        Get
            Return _Dal
        End Get
        Set(ByVal Value As DateTime)
            _Dal = Value
        End Set
    End Property
    Public Property Al() As DateTime
        Get
            Return _Al
        End Get
        Set(ByVal Value As DateTime)
            _Al = Value
        End Set
    End Property
    Public Property sCognome() As String
        Get
            Return _sCognome
        End Get
        Set(ByVal Value As String)
            _sCognome = Value
        End Set
    End Property
    Public Property sNome() As String
        Get
            Return _sNome
        End Get
        Set(ByVal Value As String)
            _sNome = Value
        End Set
    End Property
    Public Property sCF() As String
        Get
            Return _sCF
        End Get
        Set(ByVal Value As String)
            _sCF = Value
        End Set
    End Property
    Public Property sPIVA() As String
        Get
            Return _sPIVA
        End Get
        Set(ByVal Value As String)
            _sPIVA = Value
        End Set
    End Property
    Public Property sNTessera() As String
        Get
            Return _sNTessera
        End Get
        Set(ByVal Value As String)
            _sNTessera = Value
        End Set
    End Property
    Public Property sVia() As String
        Get
            Return _sVia
        End Get
        Set(ByVal Value As String)
            _sVia = Value
        End Set
    End Property
    Public Property sCivico() As String
        Get
            Return _sCivico
        End Get
        Set(ByVal Value As String)
            _sCivico = Value
        End Set
    End Property
    Public Property sInterno() As String
        Get
            Return _sInterno
        End Get
        Set(ByVal Value As String)
            _sInterno = Value
        End Set
    End Property
    Public Property rbSoggetto() As Boolean
        Get
            Return _rbSoggetto
        End Get
        Set(ByVal Value As Boolean)
            _rbSoggetto = Value
        End Set
    End Property
    Public Property rbImmobile() As Boolean
        Get
            Return _rbImmobile
        End Get
        Set(ByVal Value As Boolean)
            _rbImmobile = Value
        End Set
    End Property
    Public Property Chiusa() As Integer
        Get
            Return _Chiusa
        End Get
        Set(ByVal Value As Integer)
            _Chiusa = Value
        End Set
    End Property
    Public Property sFoglio() As String
        Get
            Return _sFoglio
        End Get
        Set(ByVal Value As String)
            _sFoglio = Value
        End Set
    End Property
    Public Property sNumero() As String
        Get
            Return _sNumero
        End Get
        Set(ByVal Value As String)
            _sNumero = Value
        End Set
    End Property
    Public Property sSubalterno() As String
        Get
            Return _sSubalterno
        End Get
        Set(ByVal Value As String)
            _sSubalterno = Value
        End Set
    End Property
    Public Property sProvenienza() As String
        Get
            Return _provenienza
        End Get
        Set(ByVal Value As String)
            _provenienza = Value
        End Set
    End Property
    Public Property IdCatCatastale() As String
        Get
            Return _catcatastale
        End Get
        Set(ByVal Value As String)
            _catcatastale = Value
        End Set
    End Property
    Public Property TypeSogRes() As Integer
        Get
            Return _typesogres
        End Get
        Set(ByVal Value As Integer)
            _typesogres = Value
        End Set
    End Property
    Public Property IdCatTARES() As Integer
        Get
            Return _idcatTARES
        End Get
        Set(ByVal Value As Integer)
            _idcatTARES = Value
        End Set
    End Property
    Public Property nComponenti() As Integer
        Get
            Return _nc
        End Get
        Set(ByVal Value As Integer)
            _nc = Value
        End Set
    End Property
    Public Property IsPF() As Boolean
        Get
            Return _isPF
        End Get
        Set(ByVal Value As Boolean)
            _isPF = Value
        End Set
    End Property
    Public Property IsPV() As Boolean
        Get
            Return _isPV
        End Get
        Set(ByVal Value As Boolean)
            _isPV = Value
        End Set
    End Property
    Public Property IsEsente() As Boolean
        Get
            Return _isEsente
        End Get
        Set(ByVal Value As Boolean)
            _isEsente = Value
        End Set
    End Property
    Public Property IdRiduzione() As String
        Get
            Return _idrid
        End Get
        Set(ByVal Value As String)
            _idrid = Value
        End Set
    End Property
    Public Property IdDetassazione() As String
        Get
            Return _iddet
        End Get
        Set(ByVal Value As String)
            _iddet = Value
        End Set
    End Property
    Public Property HasMoreUI() As Boolean
        Get
            Return _moreUI
        End Get
        Set(ByVal Value As Boolean)
            _moreUI = Value
        End Set
    End Property
    Public Property IdStatoOccupazione() As String
        Get
            Return _idstatooccup
        End Get
        Set(ByVal Value As String)
            _idstatooccup = Value
        End Set
    End Property
    Public Property TipoConf() As Integer
        Get
            Return _TipoConf
        End Get
        Set(ByVal Value As Integer)
            _TipoConf = Value
        End Set
    End Property
    Public Property ConfDal() As DateTime
        Get
            Return _ConfDal
        End Get
        Set(ByVal Value As DateTime)
            _ConfDal = Value
        End Set
    End Property
    Public Property ConfAl() As DateTime
        Get
            Return _ConfAl
        End Get
        Set(ByVal Value As DateTime)
            _ConfAl = Value
        End Set
    End Property
End Class
#End Region
#Region "Interrogazioni Generali Aliquote"
Public Enum TipoInterAliquote
    Tariffe
    Riduzioni
    Esenzioni
    ICI
    TASI
    Addizionali
    Canoni
    Scaglioni
    Nolo
    QuotaFissa
    TariffeOSAP
    TariffeScuola
End Enum
''' <summary>
''' Definizione oggetto ricerca interrogazioni aliquote
''' </summary>
Public Class ObjInterAliquoteSearch
    Private _IdTributo As String = ""
    Private _Ambiente As String = ""
    Private _IdEnte As String = ""
    Private _Anno As String = ""
    Private _Tipo As TipoInterAliquote

    Public Property IdTributo() As String
        Get
            Return _IdTributo
        End Get
        Set(ByVal Value As String)
            _IdTributo = Value
        End Set
    End Property
    Public Property Ambiente() As String
        Get
            Return _Ambiente
        End Get
        Set(ByVal Value As String)
            _Ambiente = Value
        End Set
    End Property
    Public Property IdEnte() As String
        Get
            Return _IdEnte
        End Get
        Set(ByVal Value As String)
            _IdEnte = Value
        End Set
    End Property
    Public Property Anno() As String
        Get
            Return _Anno
        End Get
        Set(ByVal Value As String)
            _Anno = Value
        End Set
    End Property
    Public Property Tipo() As TipoInterAliquote
        Get
            Return _Tipo
        End Get
        Set(ByVal Value As TipoInterAliquote)
            _Tipo = Value
        End Set
    End Property
End Class
''' <summary>
''' Definizione oggetto ricerca interrogazioni fatturato/incassato
''' </summary>
Public Class ObjInterFattVSIncasSearch
    Private _IdTributo As String = ""
    Private _Ambiente As String = ""
    Private _Ente As String = ""
    Private _Anno As String = ""
    Private _Param3 As String = ""
    Private _Dal As Date = Date.MaxValue
    Private _Al As Date = Date.MaxValue
    Private _Rata As Integer = -1

    Public Property IdTributo() As String
        Get
            Return _IdTributo
        End Get
        Set(ByVal Value As String)
            _IdTributo = Value
        End Set
    End Property
    Public Property Ambiente() As String
        Get
            Return _Ambiente
        End Get
        Set(ByVal Value As String)
            _Ambiente = Value
        End Set
    End Property
    Public Property IdEnte() As String
        Get
            Return _Ente
        End Get
        Set(ByVal Value As String)
            _Ente = Value
        End Set
    End Property
    Public Property Anno() As String
        Get
            Return _Anno
        End Get
        Set(ByVal Value As String)
            _Anno = Value
        End Set
    End Property
    Public Property Param3() As String
        Get
            Return _Param3
        End Get
        Set(ByVal Value As String)
            _Param3 = Value
        End Set
    End Property
    Public Property tDal() As Date
        Get
            Return _Dal
        End Get
        Set(ByVal Value As Date)
            _Dal = Value
        End Set
    End Property
    Public Property tAl() As Date
        Get
            Return _Al
        End Get
        Set(ByVal Value As Date)
            _Al = Value
        End Set
    End Property
    Public Property Rata() As Integer
        Get
            Return _Rata
        End Get
        Set(ByVal Value As Integer)
            _Rata = Value
        End Set
    End Property
End Class
#End Region
''' <summary>
''' Classe di gestione interrogazioni 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Public Class clsInterrogazioni
    Private Shared Log As ILog = LogManager.GetLogger(GetType(clsInterrogazioni))
    Public Const TipoInterrogazione_Dich As String = "D"
    Public Const TipoInterrogazione_Emesso As String = "E"
    Public Const TipoInterrogazione_Tessere As String = "T"
    ''' <summary>
    ''' Enumeratore per il tipo di grafico da produrre in Analisi Eventi
    ''' </summary>
    Public Enum AnalisiEventi_TipoChart
        Nullo
        Operatore
        Tributo
        Tempo
    End Enum
    Public Structure ColICI
        Dim Name As String
        Dim Description As String

        Public Sub New(ByRef MyVal As ArrayList)
            Dim myCol As New ColICI
            Try
                MyVal = New ArrayList
                myCol = New ColICI
                myCol.Name = "IMP_ABI_PRINC"
                myCol.Description = "ABI. PRINC. (3912-3958)"
                MyVal.Add(myCol)
                myCol = New ColICI
                myCol.Name = "IMP_ALTRI_FAB"
                myCol.Description = "ALTRI FAB. (3918-3961)"
                MyVal.Add(myCol)
                myCol = New ColICI
                myCol.Name = "IMP_AREE_FAB"
                myCol.Description = "AREE FAB. (3916-3960)"
                MyVal.Add(myCol)
                myCol = New ColICI
                myCol.Name = "IMP_TERRENI"
                myCol.Description = "TERRENI (3914)"
                MyVal.Add(myCol)
                myCol = New ColICI
                myCol.Name = "IMP_FABRURUSOSTRUM"
                myCol.Description = "FAB.RUR. (3913-3959)"
                MyVal.Add(myCol)
                myCol = New ColICI
                myCol.Name = "IMP_USOPRODCATD"
                myCol.Description = "USO PROD.CAT.D (3930)"
                MyVal.Add(myCol)
                myCol = New ColICI
                myCol.Name = "IMP_ALTRI_FAB_STATO"
                myCol.Description = "ALTRI FAB. STATO (3919)"
                MyVal.Add(myCol)
                myCol = New ColICI
                myCol.Name = "IMP_AREE_FAB_STATO"
                myCol.Description = "AREE FAB. STATO (3917)"
                MyVal.Add(myCol)
                myCol = New ColICI
                myCol.Name = "IMP_TERRENI_STATO"
                myCol.Description = "TERRENI STATO (3915)"
                MyVal.Add(myCol)
                myCol = New ColICI
                myCol.Name = "IMP_FABRURUSOSTRUM_STATO"
                myCol.Description = "FAB.RUR. STATO (3919)"
                MyVal.Add(myCol)
                myCol = New ColICI
                myCol.Name = "IMP_USOPRODCATD_STATO"
                myCol.Description = "USO PROD.CAT.D STATO (3925)"
                MyVal.Add(myCol)
                myCol = New ColICI
                myCol.Name = "IMP_RAVOPER"
                myCol.Description = "RAV. OPEROSO"
                MyVal.Add(myCol)
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.clsInterrogazioni.New.errore: ", ex)
                MyVal = Nothing
            End Try
        End Sub
    End Structure
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="sTipoInterr"></param>
    ''' <param name="myParam"></param>
    ''' <param name="sIdTributo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetInterrogazioneGenerale(ByVal myStringConnection As String, ByVal sTipoInterr As String, myParam As ObjInterGenSearch, sIdTributo As String) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Add("@IDENTE", SqlDbType.NVarChar).Value = myParam.IdEnte
            cmdMyCommand.Parameters.Add("@IDTRIBUTO", SqlDbType.NVarChar).Value = sIdTributo
            cmdMyCommand.Parameters.Add("@COGNOME", SqlDbType.NVarChar).Value = myParam.sCognome
            cmdMyCommand.Parameters.Add("@NOME", SqlDbType.NVarChar).Value = myParam.sNome
            cmdMyCommand.Parameters.Add("@CODFISCALE", SqlDbType.NVarChar).Value = myParam.sCF
            cmdMyCommand.Parameters.Add("@PARTITAIVA", SqlDbType.NVarChar).Value = myParam.sPIVA
            cmdMyCommand.Parameters.Add("@DAL", SqlDbType.DateTime).Value = myParam.Dal
            cmdMyCommand.Parameters.Add("@AL", SqlDbType.DateTime).Value = myParam.Al
            cmdMyCommand.Parameters.Add("@VIA", SqlDbType.NVarChar).Value = myParam.sVia
            cmdMyCommand.Parameters.Add("@CIVICO", SqlDbType.NVarChar).Value = myParam.sCivico
            cmdMyCommand.Parameters.Add("@INTERNO", SqlDbType.NVarChar).Value = myParam.sInterno
            cmdMyCommand.Parameters.Add("@FOGLIO", SqlDbType.NVarChar).Value = myParam.sFoglio
            cmdMyCommand.Parameters.Add("@NUMERO", SqlDbType.NVarChar).Value = myParam.sNumero
            cmdMyCommand.Parameters.Add("@SUB", SqlDbType.NVarChar).Value = myParam.sSubalterno
            cmdMyCommand.Parameters.Add("@NUMEROTESSERA", SqlDbType.NVarChar).Value = myParam.sNTessera
            cmdMyCommand.Parameters.Add("@TIPOCONF", SqlDbType.Int).Value = myParam.TipoConf
            cmdMyCommand.Parameters.Add("@CONFDAL", SqlDbType.DateTime).Value = myParam.ConfDal
            cmdMyCommand.Parameters.Add("@CONFAL", SqlDbType.DateTime).Value = myParam.ConfAl
            If sTipoInterr = TipoInterrogazione_Dich Then
                cmdMyCommand.CommandText = "prc_GetInterGenDich"
            ElseIf sTipoInterr = TipoInterrogazione_Tessere Then
                cmdMyCommand.CommandText = "prc_GetInterGenTessere"
            Else
                cmdMyCommand.CommandText = "prc_GetInterGenEmesso"
            End If
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.clsInterrogazioni.GetInterrogazioneGenerale.errore: ", ex)
            Return Nothing
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function
    'Public Function GetInterrogazioneGenerale(ByVal myStringConnection As String, ByVal sTipoInterr As String, myParam As ObjInterGenSearch, sIdTributo As String) As DataSet
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Add("@IDENTE", SqlDbType.NVarChar).Value = myParam.IdEnte
    '        cmdMyCommand.Parameters.Add("@IDTRIBUTO", SqlDbType.NVarChar).Value = sIdTributo
    '        cmdMyCommand.Parameters.Add("@COGNOME", SqlDbType.NVarChar).Value = myParam.sCognome
    '        cmdMyCommand.Parameters.Add("@NOME", SqlDbType.NVarChar).Value = myParam.sNome
    '        cmdMyCommand.Parameters.Add("@CODFISCALE", SqlDbType.NVarChar).Value = myParam.sCF
    '        cmdMyCommand.Parameters.Add("@PARTITAIVA", SqlDbType.NVarChar).Value = myParam.sPIVA
    '        cmdMyCommand.Parameters.Add("@DAL", SqlDbType.DateTime).Value = myParam.Dal
    '        cmdMyCommand.Parameters.Add("@AL", SqlDbType.DateTime).Value = myParam.Al
    '        cmdMyCommand.Parameters.Add("@VIA", SqlDbType.NVarChar).Value = myParam.sVia
    '        cmdMyCommand.Parameters.Add("@CIVICO", SqlDbType.NVarChar).Value = myParam.sCivico
    '        cmdMyCommand.Parameters.Add("@INTERNO", SqlDbType.NVarChar).Value = myParam.sInterno
    '        cmdMyCommand.Parameters.Add("@FOGLIO", SqlDbType.NVarChar).Value = myParam.sFoglio
    '        cmdMyCommand.Parameters.Add("@NUMERO", SqlDbType.NVarChar).Value = myParam.sNumero
    '        cmdMyCommand.Parameters.Add("@SUB", SqlDbType.NVarChar).Value = myParam.sSubalterno
    '        cmdMyCommand.Parameters.Add("@NUMEROTESSERA", SqlDbType.NVarChar).Value = myParam.sNTessera
    '        cmdMyCommand.Parameters.Add("@TIPOCONF", SqlDbType.Int).Value = myParam.TipoConf
    '        cmdMyCommand.Parameters.Add("@CONFDAL", SqlDbType.DateTime).Value = myParam.ConfDal
    '        cmdMyCommand.Parameters.Add("@CONFAL", SqlDbType.DateTime).Value = myParam.ConfAl
    '        If sTipoInterr = TipoInterrogazione_Dich Then
    '            cmdMyCommand.CommandText = "prc_GetInterGenDich"
    '        ElseIf sTipoInterr = TipoInterrogazione_Tessere Then
    '            cmdMyCommand.CommandText = "prc_GetInterGenTessere"
    '        Else
    '            cmdMyCommand.CommandText = "prc_GetInterGenEmesso"
    '        End If
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()
    '        Return myDataSet
    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.clsInterrogazioni.GetInterrogazioneGenerale.errore: ", ex)
    '        Return Nothing
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="sAmbiente"></param>
    ''' <param name="sEnte"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sIdTributo"></param>
    ''' <returns></returns>
    Public Function GetInterrogazioneCruscotto(ByVal myStringConnection As String, sAmbiente As String, ByVal sEnte As String, ByVal sAnno As String, ByVal sIdTributo As String) As DataSet
        Dim sSQL As String = ""
        Dim myDataSet As DataSet
        Try
            'Valorizzo la connessione
            Dim oDbManagerRepository As New DBModel(COSTANTValue.ConstSession.DBType, myStringConnection)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetInterCruscotto", "IDENTE", "ANNO", "IDTRIBUTO")
                myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("IDENTE", sEnte) _
                        , ctx.GetParam("ANNO", sAnno) _
                        , ctx.GetParam("IDTRIBUTO", sIdTributo)
                    )
                ctx.Dispose()
            End Using
            Return myDataSet
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.clsInterrogazioni.GetInterrogazioneCruscotto.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="oParam"></param>
    ''' <returns></returns>
    Public Function GetInterAliquote(ByVal myStringConnection As String, ByVal oParam As ObjInterAliquoteSearch) As DataSet
        Dim sSQL As String = ""
        Dim myDataSet As DataSet
        Try
            'Valorizzo la connessione
            Dim oDbManagerRepository As New DBModel(COSTANTValue.ConstSession.DBType, myStringConnection)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetInterAliquote", "IDENTE", "ANNO", "TIPO")
                myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("IDENTE", oParam.IdEnte) _
                        , ctx.GetParam("ANNO", oParam.Anno) _
                        , ctx.GetParam("TIPO", oParam.Tipo)
                    )
                ctx.Dispose()
            End Using
            Return myDataSet
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.clsInterrogazioni.GetInterAliquote.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="oParam"></param>
    ''' <returns></returns>
    Public Function GetInterFattVSIncas(ByVal myStringConnection As String, ByVal oParam As ObjInterFattVSIncasSearch) As DataSet
        Dim sSQL As String = ""
        Dim myDataSet As New DataSet
        Dim dsReturn As New DataSet
        Try
            'Valorizzo la connessione
            Dim oDbManagerRepository As New DBModel(COSTANTValue.ConstSession.DBType, myStringConnection)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetInterFattVSIncas", "IDTRIBUTO", "IDENTE", "ANNO", "PARAM3", "DAL", "AL", "RATA", "TIPO")
                myDataSet = ctx.GetDataSet(sSQL, "EMESSO", ctx.GetParam("IDTRIBUTO", oParam.IdTributo) _
                        , ctx.GetParam("IDENTE", oParam.IdEnte) _
                        , ctx.GetParam("ANNO", oParam.Anno) _
                        , ctx.GetParam("PARAM3", oParam.Param3) _
                        , ctx.GetParam("DAL", oParam.tDal) _
                        , ctx.GetParam("AL", oParam.tAl) _
                        , ctx.GetParam("RATA", oParam.Rata) _
                        , ctx.GetParam("TIPO", "E")
                    )
                If myDataSet.Tables.Count > 0 Then
                    dsReturn.Tables.Add(myDataSet.Tables(0).Copy)
                End If
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetInterFattVSIncas", "IDTRIBUTO", "IDENTE", "ANNO", "PARAM3", "DAL", "AL", "RATA", "TIPO")
                myDataSet = ctx.GetDataSet(sSQL, "NETTO", ctx.GetParam("IDTRIBUTO", oParam.IdTributo) _
                        , ctx.GetParam("IDENTE", oParam.IdEnte) _
                        , ctx.GetParam("ANNO", oParam.Anno) _
                        , ctx.GetParam("PARAM3", oParam.Param3) _
                        , ctx.GetParam("DAL", oParam.tDal) _
                        , ctx.GetParam("AL", oParam.tAl) _
                        , ctx.GetParam("RATA", oParam.Rata) _
                        , ctx.GetParam("TIPO", "N")
                    )
                If myDataSet.Tables.Count > 0 Then
                    dsReturn.Tables.Add(myDataSet.Tables(0).Copy)
                End If
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetInterFattVSIncas", "IDTRIBUTO", "IDENTE", "ANNO", "PARAM3", "DAL", "AL", "RATA", "TIPO")
                myDataSet = ctx.GetDataSet(sSQL, "PAGATO", ctx.GetParam("IDTRIBUTO", oParam.IdTributo) _
                        , ctx.GetParam("IDENTE", oParam.IdEnte) _
                        , ctx.GetParam("ANNO", oParam.Anno) _
                        , ctx.GetParam("PARAM3", oParam.Param3) _
                        , ctx.GetParam("DAL", oParam.tDal) _
                        , ctx.GetParam("AL", oParam.tAl) _
                        , ctx.GetParam("RATA", oParam.Rata) _
                        , ctx.GetParam("TIPO", "P")
                    )
                If myDataSet.Tables.Count > 0 Then
                    dsReturn.Tables.Add(myDataSet.Tables(0).Copy)
                End If
                ctx.Dispose()
            End Using

            Return dsReturn
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.clsInterrogazioni.GetInterFattVSIncas.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione per la ricerca di inserimento/modifica/cancellazione avvenuti in base ai parametri
    ''' </summary>
    ''' <param name="IdEnte">string</param>
    ''' <param name="Tributo">string</param>
    ''' <param name="IdCausale">int</param>
    ''' <param name="Operatore">string</param>
    ''' <param name="Dal">DateTime</param>
    ''' <param name="Al">DateTime</param>
    ''' <param name="myStringConnection">string stringa di connessione</param>
    ''' <returns>DataSet con i record che rispondono ai criteri</returns>
    Public Function GetAnalisiEventi(ByVal IdEnte As String, Tributo As String, IdCausale As String, Operatore As String, Dal As DateTime, Al As DateTime, ByVal myStringConnection As String) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetAnalisiEventi"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
            cmdMyCommand.Parameters.AddWithValue("@Tributo", Tributo)
            cmdMyCommand.Parameters.AddWithValue("@Dal", Dal)
            cmdMyCommand.Parameters.AddWithValue("@Al", Al)
            cmdMyCommand.Parameters.AddWithValue("@Operatore", Operatore)
            cmdMyCommand.Parameters.AddWithValue("@IdCausale", IdCausale)
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "analisieventi")
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgov.clsInterrogazioni.GetAnalisiEventi.errore: ", ex)
            Return Nothing
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
        Return myDataSet
    End Function
    ''' <summary>
    ''' Funzione per la ricerca dei valori in forma aggregata di inserimento/modifica/cancellazione avvenuti in base ai parametri
    ''' </summary>
    ''' <param name="IdEnte">string</param>
    ''' <param name="Tributo">string</param>
    ''' <param name="IdCausale">int</param>
    ''' <param name="Operatore">string</param>
    ''' <param name="Dal">DateTime</param>
    ''' <param name="Al">DateTime</param>
    ''' <param name="QtaTempo">int</param>
    ''' <param name="TipoTempo">string {G=Giorni,S=Settimane,M=Mesi}</param>
    ''' <param name="myStringConnection">string stringa di connessione</param>
    ''' <returns>DataSet con i record che rispondono ai criteri</returns>
    Public Function GetChartAnalisiEventi(ByVal IdEnte As String, Tributo As String, IdCausale As String, Operatore As String, Dal As DateTime, Al As DateTime, QtaTempo As Integer, TipoTempo As String, ByVal myStringConnection As String) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetAnalisiEventiChart"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
            cmdMyCommand.Parameters.AddWithValue("@Tributo", Tributo)
            cmdMyCommand.Parameters.AddWithValue("@Dal", Dal)
            cmdMyCommand.Parameters.AddWithValue("@Al", Al)
            cmdMyCommand.Parameters.AddWithValue("@Operatore", Operatore)
            cmdMyCommand.Parameters.AddWithValue("@IdCausale", IdCausale)
            cmdMyCommand.Parameters.AddWithValue("@Tipo", AnalisiEventi_TipoChart.Operatore)
            cmdMyCommand.Parameters.AddWithValue("@QtaTempo", QtaTempo)
            cmdMyCommand.Parameters.AddWithValue("@TipoTempo", TipoTempo)
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "operatori")

            cmdMyCommand.CommandText = "prc_GetAnalisiEventiChart"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
            cmdMyCommand.Parameters.AddWithValue("@Tributo", Tributo)
            cmdMyCommand.Parameters.AddWithValue("@Dal", Dal)
            cmdMyCommand.Parameters.AddWithValue("@Al", Al)
            cmdMyCommand.Parameters.AddWithValue("@Operatore", Operatore)
            cmdMyCommand.Parameters.AddWithValue("@IdCausale", IdCausale)
            cmdMyCommand.Parameters.AddWithValue("@Tipo", AnalisiEventi_TipoChart.Tributo)
            cmdMyCommand.Parameters.AddWithValue("@QtaTempo", QtaTempo)
            cmdMyCommand.Parameters.AddWithValue("@TipoTempo", TipoTempo)
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "tributo")

            cmdMyCommand.CommandText = "prc_GetAnalisiEventiChart"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
            cmdMyCommand.Parameters.AddWithValue("@Tributo", Tributo)
            cmdMyCommand.Parameters.AddWithValue("@Dal", Dal)
            cmdMyCommand.Parameters.AddWithValue("@Al", Al)
            cmdMyCommand.Parameters.AddWithValue("@Operatore", Operatore)
            cmdMyCommand.Parameters.AddWithValue("@IdCausale", IdCausale)
            cmdMyCommand.Parameters.AddWithValue("@Tipo", AnalisiEventi_TipoChart.Tempo)
            cmdMyCommand.Parameters.AddWithValue("@QtaTempo", QtaTempo)
            cmdMyCommand.Parameters.AddWithValue("@TipoTempo", TipoTempo)
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "tempo")
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgov.clsInterrogazioni.GetChartAnalisiEventi.errore: ", ex)
            Return Nothing
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
        Return myDataSet
    End Function
End Class
