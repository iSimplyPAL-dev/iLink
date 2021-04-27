Imports log4net
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Web.Security

Namespace Ribes.OPENgov.Utilities
    ''' <summary>
    ''' Contiene le funzioni per comunicare col DB.
    ''' </summary>
    <Serializable()>
    Public Class ClsDatabase
        Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(ClsDatabase))

#Region "Variabili e costruttore"
        <NonSerialized()>
        Private _sqlConnection As SqlConnection

        <NonSerialized()>
        Private _ownConnection As SqlConnection

        <NonSerialized()>
        Private _ownTransaction As SqlTransaction

        <NonSerialized()>
        Private _keepConnection As Boolean
#End Region
#Region "Gestione della connessione"
        Protected Overloads Sub Connect()
            Try
                If (_connectionCallback Is Nothing) Then
                    If ((_paramsCacheSpan = 0) _
                                    OrElse ((_paramsLastLoad + _paramsCacheSpan) _
                                    <= Environment.TickCount)) Then
                        _cachedConnection = PrepareConnection(ConfigurationManager.AppSettings("ConnectionTERRITORIO"))
                        _paramsLastLoad = Environment.TickCount
                    End If
                    Connect(Nothing)
                Else
                    Dim connectionName As String
                    _connectionCallback(connectionName)
                    Connect(connectionName)
                End If
            Catch ex As Exception
                Log.Debug("ClsDatabase::Connect::si è verificato il seguente errore::", ex)
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Apre la connessione al DB.
        ''' </summary>
        ''' <exception cref="Exception">Eccezione generata in caso di connessione fallita.</exception>
        Protected Overloads Sub Connect(ByVal connection As String)
            If ((_ownConnection Is Nothing) And Not _keepConnection) Then
                If (_sqlConnection Is Nothing) Then
                    _sqlConnection = New SqlConnection()

                    Try
                        If connection Is Nothing Then
                            connection = _cachedConnection
                        End If
                        _sqlConnection.ConnectionString = connection
                        _sqlConnection.Open()
                    Catch ex As Exception
                        Try
                            If (_sqlConnection.State <> ConnectionState.Closed) Then
                                _sqlConnection.Close()
                            End If
                            _sqlConnection.ConnectionString += ";Pooling=false"
                            _sqlConnection.Open()
                        Catch err As Exception
                            Log.Debug("ClsDatabase::Connect(" + connection + ")::si è verificato il seguente errore::", err)
                            Throw
                        End Try
                    End Try
                End If
            End If
        End Sub

        Protected Overloads Sub Connect(ByVal server As String, ByVal database As String, ByVal login As String, ByVal password As String)
            _cachedConnection = String.Format("User ID={0};Initial Catalog={1};Data Source={2};Password={3}", login, database, server, password)
            Connect()
        End Sub

        ''' <summary>
        ''' Prepara la stringa di connessione al DB.
        ''' </summary>
        ''' <param name="databaseConnection">Il parametro di configurazione contenente la stringa di connessione.</param>
        Private Shared Function PrepareConnection(ByVal databaseConnection As String) As String
            Return ConfigurationManager.ConnectionStrings(databaseConnection).ConnectionString
        End Function
#End Region
#Region "Gestione della disconnessione"
        Protected Overloads Sub Disconnect()
            If ((Not (_sqlConnection) Is Nothing) _
                            AndAlso Not _keepConnection) Then
                Try
                    _sqlConnection.Close()
                Catch ex As Exception
                    Log.Debug("ClsDatabase::Disconnect::si è verificato il seguente errore::", ex)
                End Try
                _sqlConnection.Dispose()
                _sqlConnection = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Chiude la connessione al DB e effettua il "Close" e il "Dispose" degli oggetti inerenti.
        ''' </summary>
        ''' <param name="disposables">La lista degli oggetti su cui effettuare il "Close" e il "Dispose".</param>
        Protected Overloads Sub Disconnect(ByVal ParamArray disposables() As IDisposable)
            Disconnect()
            For Each disposable As IDisposable In disposables
                If (Not (disposable) Is Nothing) Then
                    Try
                        Dim method As MethodInfo = disposable.GetType.GetMethod("Close", New Type(-1) {})
                        If (Not (method) Is Nothing) Then
                            method.Invoke(disposable, Nothing)
                        End If
                    Catch ex As Exception
                        Log.Debug("ClsDatabase::Disconnect::si è verificato il seguente errore::", ex)
                    End Try
                    disposable.Dispose()
                End If
            Next
        End Sub
#End Region
#Region "Proprietà pubbliche"
        Protected Property KeepConnection As Boolean
            Get
                Return _keepConnection
            End Get
            Set(value As Boolean)
                _keepConnection = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la connessione al DB da utilizzare.
        ''' </summary>
        Protected ReadOnly Property SqlConnection As SqlConnection
            Get
                If _ownConnection Is Nothing Then
                    Return _sqlConnection
                Else
                    Return _ownConnection
                End If
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta una personale connessione al DB già aperta.
        ''' </summary>
        Public Property OwnConnection As SqlConnection
            Get
                Return _ownConnection
            End Get
            Set(value As SqlConnection)
                _ownConnection = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una personale transazione relativa alla connessione aperta.
        ''' </summary>
        Public Property OwnTransaction As SqlTransaction
            Get
                Return _ownTransaction
            End Get
            Set(value As SqlTransaction)
                _ownTransaction = value
            End Set
        End Property
#End Region
#Region "Metodi di supporto"
        Protected Function CreateCommand() As SqlCommand
            Try
                Dim sqlCmd As SqlCommand = SqlConnection.CreateCommand
                If (Not (_ownTransaction) Is Nothing) Then
                    sqlCmd.Transaction = _ownTransaction
                End If
                sqlCmd.CommandTimeout = 0
                Return sqlCmd
            Catch ex As Exception
                Log.Debug("ClsDatabase::CreateCommand::si è verificato il seguente errore::", ex)
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Applica OwnConnection e OwnTransaction ad un altro oggetto derivato.
        ''' </summary>
        ''' <param name="database">L'oggetto derivato a cui applicare OwnConnection e OwnTransaction.</param>
        Protected Sub ApplyDatabaseTo(ByVal database As ClsDatabase)
            If (Not (database) Is Nothing) Then
                database._ownConnection = SqlConnection
                database._ownTransaction = OwnTransaction
            End If
        End Sub
#End Region
#Region "Membri statici"
        <NonSerialized()>
        Private Shared _connectionCallback As DatabaseConnectionCallback

        <NonSerialized()>
        Private Shared _paramsCacheSpan As Integer = (60 * 1000)

        <NonSerialized()>
        Private Shared _paramsLastLoad As Integer = Integer.MinValue

        <NonSerialized()>
        Private Shared _cachedConnection As String

        ''' <summary>
        ''' Restituisce o imposta una callback che può essere utilizzata per passare i parametri di connessione.
        ''' </summary>
        Public Shared Property ConnectionCallback As DatabaseConnectionCallback
            Get
                Return _connectionCallback
            End Get
            Set(value As DatabaseConnectionCallback)
                _connectionCallback = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica per quanti millisecondi tenere in cache i parametri di connessione.
        ''' </summary>
        ''' <exception cref="ArgumentOutOfRangeException">Se il valore fornito è negativo.</exception>
        Public Shared Property ParamsCacheSpan As Integer
            Get
                Return _paramsCacheSpan
            End Get
            Set(value As Integer)
                If (value < 0) Then
                    Throw New ArgumentOutOfRangeException("Il valore non può essere negativo.", CType(Nothing, Exception))
                End If
                _paramsCacheSpan = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il valore minimo della data compatibile con SQL2005
        ''' </summary>
        Public Shared ReadOnly Property MinDate As DateTime
            Get
                Return New DateTime(1753, 1, 1)
            End Get
        End Property
#End Region
    End Class

    ''' <summary>
    ''' Definizione di una callback che può essere utilizzata per passare i parametri di connessione.
    ''' </summary>
    ''' <param name="connectionName">La stringa di connessione</param>
    Public Delegate Sub DatabaseConnectionCallback(ByRef connectionName As String)
End Namespace
''' <summary>
''' Classe generale che eredita BasePage.
''' Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
''' </summary>
Public Class BaseEnte
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(BaseEnte))

    Private Sub BaseEnte_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            If ConstSession.IdEnte = "" Then
                RegisterScript("parent.location.href = '" & Request.Url.GetLeftPart(UriPartial.Authority) & "/" & Request.ApplicationPath & "/Default.aspx';GestAlert('a', 'warning', '', '', 'Sessione scaduta rieffettuare LOGIN');", Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTERRITORIO.BaseEnte.BasePage_Init.errore: ", ex)
        End Try
    End Sub
End Class
''' <summary>
''' Classe generale che eredita Page.
''' Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
''' </summary>
Public Class BasePage
    Inherits Page
    Private Shared Log As ILog = LogManager.GetLogger(GetType(BasePage))

    Private Sub BasePage_Init(sender As Object, e As EventArgs) Handles Me.Init
        Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_TARSU
        Session("IsFromTARES") = "1"
        Try
            If ConstSession.UserName = "" Then
                RegisterScript("parent.location.href = '" & Request.Url.GetLeftPart(UriPartial.Authority) & "/" & Request.ApplicationPath & "/Default.aspx';GestAlert('a', 'warning', '', '', 'Sessione scaduta rieffettuare LOGIN');", Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTERRITORIO.BasePage.BasePage_Init.errore: ", ex)
        End Try
    End Sub

    Protected Sub RegisterScript(ByVal script As String, ByVal type As Type)
        ConstSession.CountScript = (ConstSession.CountScript + 1)
        Dim uniqueId As String = ("spc_" _
                    + (ConstSession.CountScript.ToString _
                    + (DateTime.Now.ToString + ("." + DateTime.Now.Millisecond.ToString))))
        Dim sScript As String = "<script language='javascript'>"
        sScript = (sScript + script)
        sScript = (sScript + "</script>")
        ClientScript.RegisterStartupScript(type, uniqueId, sScript)
    End Sub
End Class
''' <summary>
''' Classe che incapsula tutti le utility necessarie alll'applicativo OPENgovTerritorio.
''' </summary>
Public Class ClsUtilities
    Inherits Ribes.OPENgov.Utilities.ClsDatabase
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(ClsUtilities))

    Public Function ReplaceCharsForSearch(ByVal myString As String) As String
        Dim sReturn As String

        sReturn = ReplaceChar(myString)
        Return sReturn
    End Function

    Public Function ReplaceChar(ByVal myString As String) As String
        Dim sReturn As String

        sReturn = Replace(myString, "'", "''")
        sReturn = Replace(sReturn, "*", "%")
        sReturn = Replace(sReturn, "&nbsp;", " ")
        sReturn = Trim(sReturn)
        Return sReturn
    End Function
    Public Sub PopolaComboVieFrazioni(ByVal sEnte As String, ByVal DdlToponimi As DropDownList, ByVal DdlFrazioni As DropDownList, ByVal MyStringConnection As String)
        Try
            Dim sSQL As String

            sSQL = "SELECT TOPONIMO,ID_TOPONIMO"
            sSQL += " FROM V_TIPO_VIE"
            sSQL += " WHERE (COD_ENTE ='" & sEnte & "')"
            sSQL += " ORDER BY TOPONIMO"
            LoadComboGenerale(DdlToponimi, sSQL, MyStringConnection)
            sSQL = "SELECT FRAZIONE, ID_FRAZIONE"
            sSQL += " FROM V_FRAZIONI"
            sSQL += " WHERE (COD_ENTE ='" & sEnte & "')"
            sSQL += " ORDER BY FRAZIONE"
            LoadComboGenerale(DdlFrazioni, sSQL, MyStringConnection)
        Catch ex As Exception
            Log.Error("Si è verificato un errore in InsertStrade::PopolaCombos::" & ex.Message)
            Throw
        End Try
    End Sub

    Public Sub LoadComboGenerale(ByVal ddl As DropDownList, ByVal sSQL As String, ByVal MyStringConnection As String)
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader

        Try
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandTimeout = 0
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(MyStringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandText = sSQL
            myDataReader = cmdMyCommand.ExecuteReader
            'eseguo la query
            LoadCombo(ddl, myDataReader)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTERRITORIO.ClsUtilities.LoadComboGenerale.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di LoadComboGenerale " + ex.Message + "::SQL::" + sSQL + "::MyStringConnection::" + MyStringConnection)
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    Public Sub LoadComboGenerale(ByVal ddl As DropDownList, ByVal sMyConn As String, ByVal sStoredProcedure As String, ByVal oListParam() As SearchParameter)
        Dim dbEngine_ As DAL.DBEngine
        Dim dt As DataTable = New DataTable
        dbEngine_ = DBEngineFactory.GetDBEngine(sMyConn)
        Dim myParam As SearchParameter

        Try
            dbEngine_.OpenConnection()
            Try
                If Not oListParam Is Nothing Then
                    Log.Debug("LoadComboGenerale::ho parametri")
                    For Each myParam In oListParam
                        dbEngine_.AddParameter(myParam.Name, myParam.Value, myParam.Direction)
                    Next
                End If
                dbEngine_.ExecuteQuery(dt, sStoredProcedure, CommandType.StoredProcedure)
            Catch ex As Exception
                Log.Debug(("LoadComboGenerale::si è verificato il seguente errore::" + ex.Message))
                Throw
            Finally
                dbEngine_.CloseConnection()
            End Try
            LoadCombo(ddl, dt)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTERRITORIO.ClsUtilities.LoadComboGenerale(param).errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di LoadComboGenerale " + ex.Message)
        End Try
    End Sub

    Private Sub LoadCombo(ByVal objCombo As DropDownList, ByVal objDR As SqlClient.SqlDataReader)
        objCombo.Items.Clear()
        objCombo.Items.Add("...")
        objCombo.Items(0).Value = "-1"
        If Not objDR Is Nothing Then
            Do While objDR.Read
                If Not IsDBNull(objDR(0)) Then
                    objCombo.Items.Add(objDR(0))
                    objCombo.Items(objCombo.Items.Count - 1).Value = objDR(1)
                End If
            Loop
        End If
    End Sub

    Private Sub LoadCombo(ByVal objCombo As DropDownList, ByVal objDT As DataTable)
        Dim myRow As DataRow

        objCombo.Items.Clear()
        objCombo.Items.Add("...")
        objCombo.Items(0).Value = "-1"
        If Not objDT Is Nothing Then
            For Each myRow In objDT.Rows
                If Not IsDBNull(myRow(0)) Then
                    objCombo.Items.Add(myRow(0))
                    objCombo.Items(objCombo.Items.Count - 1).Value = myRow(1)
                End If
            Next
        End If
    End Sub
End Class
''' <summary>
''' Definizione oggetto parametri ricerca
''' </summary>
Public Class SearchParameter
    Dim _Name As String = ""
    Dim _Value As String = ""
    Dim _Direction As System.Data.ParameterDirection = ParameterDirection.Input

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property
    Public Property Value() As String
        Get
            Return _Value
        End Get
        Set(ByVal value As String)
            _Value = value
        End Set
    End Property
    Public Property Direction() As System.Data.ParameterDirection
        Get
            Return _Direction
        End Get
        Set(ByVal value As System.Data.ParameterDirection)
            _Direction = value
        End Set
    End Property
End Class