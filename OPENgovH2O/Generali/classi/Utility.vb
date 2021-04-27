Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports log4net
Imports Utility
''' <summary>
''' Classe generale che eredita Page.
''' Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
''' </summary>
Public Class BasePage
    Inherits Page
    Private Shared Log As ILog = LogManager.GetLogger(GetType(BasePage))

    Private Sub BasePage_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_H2O
            If ConstSession.UserName = "" Then
                RegisterScript("parent.location.href = '" & Request.Url.GetLeftPart(UriPartial.Authority) & "/" & Request.ApplicationPath & "/Default.aspx';GestAlert('a', 'warning', '', '', 'Sessione scaduta rieffettuare LOGIN');", Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug("OPENgovH2O.BasePage.BasePage_Init.errore: ", ex)
        End Try
    End Sub

    Protected Sub RegisterScript(ByVal script As String, ByVal type As Type)
        Try
            ConstSession.CountScript = (ConstSession.CountScript + 1)
            Dim uniqueId As String = ("spc_" _
                        + (ConstSession.CountScript.ToString _
                        + (DateTime.Now.ToString + ("." + DateTime.Now.Millisecond.ToString))))
            Dim sScript As String = "<script language='javascript'>"
            sScript = (sScript + script)
            sScript = (sScript + "</script>")
            ClientScript.RegisterStartupScript(type, uniqueId, sScript)
        Catch ex As Exception
            Log.Debug("OPENgovH2O.BasePage.RegisterScript.errore: ", ex)
        End Try
    End Sub
End Class

Public Class MyUtility
    'Public Const TRIBUTO_H2O As String = "9000"
    'Public Const TRIBUTO_IVA As String = "9800"
    Private Shared Log As ILog = LogManager.GetLogger(GetType(MyUtility))
    Enum TypeFieldXML
        TypeInteger = 1
        TypeDate = 2
        TypeString = 3
        TypeMoney = 4
    End Enum

    'Public Shared Function Condizione(ByVal strCampoChiave As String, ByVal txtTemp As TextBox) As String
    '    'Costruisce la stringa di filtro 
    '    'ES A;C --- ricerca stringhe  Comprese tra A E C...

    '    Dim intIndex As Integer
    '    Dim strFrom As String
    '    Dim strTo As String
    '    Dim str As String
    '    Dim iLen As Integer
    '    Dim n As Integer
    '    Dim strlast As String
    '    Dim i As Integer

    '    Try
    '        intIndex = InStr(1, txtTemp.Text, ";")
    '        If intIndex > 0 Then
    '            strFrom = Mid(txtTemp.Text, 1, intIndex - 1)
    '            strTo = Mid(txtTemp.Text, intIndex + 1)
    '            str = strTo
    '            iLen = Len(str)

    '            n = Asc(Mid(str, 1, 1))
    '            If Mid(str, 1, 1) <> "Z" And strTo <> "" Then
    '                strTo = Chr(n + 1)
    '            End If
    '            Condizione = "<Cond Logical=""and"">" & strCampoChiave & "&gt;=""" & strFrom & """</Cond>"
    '            If strTo <> "" Then
    '                If iLen = 1 Then
    '                    Condizione = Condizione & "<Cond Logical=""and"">" & strCampoChiave & "&lt;=""" & strTo & """</Cond>"
    '                End If
    '                If iLen > 1 Then
    '                    strTo = ""
    '                    'last=Chr (Asc (mid(str,1,1))+1)
    '                    strlast = Chr(Asc(Mid(str, iLen, 1)) + 1)
    '                    For i = 1 To iLen - 1
    '                        strTo = strTo & Mid(str, i, 1)
    '                    Next
    '                    strTo = strTo & strlast
    '                    Condizione = Condizione & "<Cond Logical=""and"">" & strCampoChiave & "&lt;""" & strTo & """</Cond>"
    '                End If
    '            End If
    '        Else
    '            strFrom = txtTemp.Text
    '            strTo = ""
    '            iLen = Len(txtTemp.Text)
    '            n = Asc(Mid(txtTemp.Text, 1, 1))
    '            If (UCase(Mid(txtTemp.Text, 1, 1)) <> "Z") Then
    '                strTo = Chr(n + 1)
    '            Else
    '                strTo = UCase(Mid(txtTemp.Text, 1, 1))
    '            End If
    '            If strTo = "Z" Then
    '                Condizione = "<Cond Logical=""and"">" & strCampoChiave & "&gt;=""" & strFrom & """</Cond>"
    '            Else
    '                Condizione = "<Cond Logical=""and"">" & strCampoChiave & "&gt;=""" & strFrom & """</Cond>"
    '                If iLen = 1 Then
    '                    Condizione = Condizione & "<Cond Logical='and'>" & strCampoChiave & "&lt;'" & strTo & "'</Cond>"
    '                End If

    '            End If

    '            If iLen > 1 Then
    '                strTo = ""
    '                strlast = Chr(Asc(Mid(txtTemp.Text, iLen, 1)) + 1)
    '                For i = 1 To iLen - 1
    '                    strTo = strTo & Mid(txtTemp.Text, i, 1)
    '                Next
    '                strTo = strTo & strlast
    '                Condizione = Condizione & "<Cond Logical=""and"">" & strCampoChiave & "&lt;=""" & strTo & """</Cond>"
    '            End If
    '        End If
    '    Catch ex As Exception

    '        Log.Debug("OPENgovH2O.MyUtility.Condizione.errore: ", ex)

    '    End Try

    'End Function

    Public Shared Function objToStr(ByRef strInput As String, ByVal strTypeField As TypeFieldXML) As Object

        objToStr = ""
        Try
            If Len(strInput) = 0 Then
                objToStr = System.DBNull.Value
            Else
                Select Case CInt(strTypeField)
                    Case CInt(TypeFieldXML.TypeInteger)
                        objToStr = CType(strInput, Integer)
                    Case CInt(TypeFieldXML.TypeDate)
                        objToStr = CType(strInput, Date)
                    Case CInt(TypeFieldXML.TypeMoney)
                        objToStr = CType(strInput, Decimal)
                    Case Else
                        objToStr = CStr(strInput)
                End Select
            End If
        Catch ex As Exception

            Log.Debug("OPENgovH2O.MyUtility.objToStr.errore: ", ex)

        End Try
    End Function

    Public Shared Function cTolng(ByRef objInput As Object) As Long

        cTolng = 0
        Try
            If Not IsDBNull(objInput) And Not IsNothing(objInput) Then
                If IsNumeric(objInput) Then
                    cTolng = CLng(objInput)
                End If
            End If

        Catch ex As Exception

            Log.Debug("OPENgovH2O.utility.stringoperation.formatint.errore: ", ex)

        End Try
    End Function

    Public Shared Function CDateToDB(ByVal vInput As Object, Optional ByRef blnFormatoInputServer As Boolean = False) As String
        Dim strDate As String
        Dim sTesto As String

        CDateToDB = "Null"

        Try

            If Not IsDBNull(vInput) And Not IsNothing(vInput) Then
                sTesto = CStr(vInput)
                'verifica che sia una data valida (il formato non importa!!!)
                If Not IsDate(sTesto) Then
                    Exit Function
                End If
                If blnFormatoInputServer = True Then
                    'Formato in input in inglese, il DB lo vuole in italiano
                    strDate = Format(Day(CDate(sTesto)), "00") & "/" & Format(Month(CDate(sTesto)), "00") & "/" & Format(Year(CDate(sTesto)), "0000")

                Else
                    'Formato ITALIANO : applica solo una formattazione ai campi
                    Dim varAr() As String = Split(sTesto, "/")

                    sTesto = Day(CDate(sTesto)) & "/" & Month(CDate(sTesto)) & "/" & Year(CDate(sTesto))

                    '    strDate_test = CDate(str)


                    '    strDate = String.Format("{0:dd}", varAr(0))




                    '  strDate = strDate & "/" & String.Format(varAr(1), "00")
                    '  strDate = strDate & "/" & Format(Year(CDate("01/01/" & varAr(2))), "0000")

                    '  strTime = String.Format(sTesto, "HH:mm:ss")

                    '  If Left(strTime, 2) = "00" And Mid(strTime, 4, 2) = "00" And Right(strTime, 2) = "00" Then
                    '    sTesto = strDate
                    '  Else
                    '    sTesto = strDate & " " & Left(strTime, 2) & ":" & Mid(strTime, 4, 2) & ":" & Right(strTime, 2)
                    '  End If
                End If

                'Verifica , dopo aver ricostruito la data , che sia una data valida
                If Not IsDate(sTesto) Then 'NOTA: il test con IsDate() va comunque bene anche se gli vengono passati GG e MM invertiti purchè i valori siano validi
                    CDateToDB = "Null"
                Else
                    CDateToDB = "'" & sTesto & "'"
                End If
            End If
        Catch ex As Exception

            Log.Debug("OPENgovH2O.MyUtility.CDateToDB.errore: ", ex)

        End Try
    End Function
    Public Shared Function CStrToDB(ByVal vInput As Object, Optional ByRef blnClearSpace As Boolean = False) As String
        Dim sTesto As String

        CStrToDB = "''"

        Try
            If Not IsDBNull(vInput) And Not IsNothing(vInput) Then

                sTesto = CStr(vInput)
                If blnClearSpace Then
                    sTesto = Trim(sTesto)
                End If
                If Trim(sTesto) <> "" Then
                    CStrToDB = "'" & Replace(sTesto, "'", "''") & "'"
                End If
            End If
        Catch ex As Exception

            Log.Debug("OPENgovH2O.utility.stringoperation.formatstring(.errore: ", ex)

        End Try
    End Function
    Public Shared Function CIdFromDB(ByVal vInput As Object) As String

        CIdFromDB = "-1"
        Try
            If Not IsDBNull(vInput) And Not IsNothing(vInput) And Not IsNothing(vInput) Then
                If IsNumeric(vInput) Then
                    If CDbl(vInput) > 0 Then
                        CIdFromDB = CStr(CDbl(vInput))
                    End If
                End If
            End If
        Catch ex As Exception

            Log.Debug("OPENgovH2O.MyUtility.CldFromDB.errore: ", ex)

        End Try
    End Function

    Public Shared Function GetFileName(ByRef sFilePathname As String, Optional ByRef strName As String = "", Optional ByRef strExtention As String = "") As String

        On Error GoTo GetFilename_Err
        Dim sTmp As String
        Dim lPos As Integer
        GetFileName = ""

        sFilePathname = Replace(sFilePathname, "/", "\")

        sTmp = StrReverse(sFilePathname)
        lPos = InStr(1, sTmp, "\") - 1

        If lPos >= 0 Then
            GetFileName = StrReverse(Mid(sTmp, 1, lPos))
        Else
            GetFileName = sFilePathname
        End If

        GetFileName = Replace(GetFileName, vbCrLf, "")

        If strName <> "" Then
            strName = GetFileNameNoExt(GetFileName)
        End If

        If strExtention <> "" Then
            strExtention = GetFileExt(GetFileName)
        End If


GetFilename_Exit:
        Exit Function
GetFilename_Err:
        'Resume GetFilename_Exit
    End Function

    '****************************************************************************
    'Ritorna l'estensione del file
    'es. GetFileExt("file01.doc") -> ritorna "doc"
    'Nota :
    'Usare la funzione GetFilename() al posto di questa funzione
    Public Shared Function GetFileExt(ByRef strFilename As String) As String
        On Error GoTo GetFileExt_Err
        Dim sTmp As String
        Dim lngPos As Integer
        Dim lngCount As Integer
        Dim strPoint As String
        GetFileExt = ""

        For lngCount = Len(strFilename) To 2 Step -1
            strPoint = Mid(strFilename, lngCount, 1)
            If strPoint = "." Then
                lngPos = lngCount + 1
            End If
        Next
        GetFileExt = Mid(strFilename, lngPos, Len(strFilename) + 1 - lngPos)

GetFileExt_Exit:
        Exit Function
GetFileExt_Err:
        'Resume GetFileExt_Exit
    End Function

    '****************************************************************************
    'Ritorna il nome del file SENZA l'estensione
    'es. GetFileNameNoExt("file01.doc") -> ritorna "file01"
    'Nota :
    'Usare la funzione GetFilename() al posto di questa funzione
    Public Shared Function GetFileNameNoExt(ByRef strFilename As String) As String

        On Error GoTo GetFileNameNoExt_Err
        Dim lngPos As Integer
        Dim lngCount As Integer
        Dim strFName As String

        GetFileNameNoExt = ""
        lngPos = 0

        For lngCount = 1 To Len(strFilename)
            If (Mid(strFilename, lngCount, 1) = "\") Then lngPos = lngCount
        Next lngCount

        'RECUPERA IL FILE SENZA PATH
        strFName = Right(strFilename, Len(strFilename) - lngPos)

        'RECUPERA IL FILE SENMZA ESTENSIONE
        lngPos = InStr(strFName, ".")
        If lngPos <> 0 Then
            strFName = Left(strFName, lngPos - 1)
        End If
        GetFileNameNoExt = strFName

GetFileNameNoExt_Exit:
        Exit Function
GetFileNameNoExt_Err:
        'Resume GetFileNameNoExt_Exit
    End Function

    Public Shared Function GetStradario(ByVal CodiceComune As String) As Boolean
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim DBAccess As New DBAccess.getDBobject
        GetStradario = False
        Try
            sSQL = "SELECT COD_COMUNE FROM STRADARIO WHERE COD_COMUNE=" & CStrToDB(CodiceComune)
            dvMyDati = DBAccess.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    GetStradario = True
                Next
            End If
            dvMyDati.Dispose()
        Catch ex As Exception
            Log.Debug("OPENgovH2O.MyUtility.GetStradario.errore: ", ex)
        End Try
    End Function
    Public Shared Function AddBackSlashToPath(ByRef sPath As Object, Optional ByRef blnRemoveInitialSlash As Boolean = True) As String
        Try

            'converte la variabile passata in una stringa
            AddBackSlashToPath = ""
            If IsDBNull(sPath) Or IsNothing(sPath) Then

                sPath = ""
            End If

            sPath = CStr(sPath)
            AddBackSlashToPath = sPath

            If Len(sPath) = 0 Then Exit Function

            'Aggiunge la \ alla fine del path

            If Right(sPath, 1) <> "\" And Right(sPath, 1) <> "/" Then 'And Len(sPath) > 1 Then

                sPath = sPath & "\"
            End If

            'Rimuove la \ iniziale
            '(questo permette di concatenare più path relativi senza aggiungere if nel codice)
            If blnRemoveInitialSlash Then
                If Left(sPath, 1) = "\" Or Left(sPath, 1) = "/" Then

                    sPath = Mid(sPath, 2)
                End If
            End If

            AddBackSlashToPath = sPath


        Catch Err As Exception
            Log.Debug("OPENgovH2O.MyUtility.AddBackSlashToPath.errore: ", Err)
            Throw Err
        End Try

    End Function

    Public Shared Function GiraDataFromDB(ByVal data As String) As String
        'leggo la data nel formato aaaammgg  e la metto nel formato gg/mm/aaaa
        Dim Giorno As String
        Dim Mese As String
        Dim Anno As String
        Try
            If data <> "" Then
                Giorno = Mid(data, 7, 2)
                Mese = Mid(data, 5, 2)
                Anno = Mid(data, 1, 4)
                GiraDataFromDB = Giorno & "/" & Mese & "/" & Anno
            Else
                GiraDataFromDB = ""
            End If
        Catch ex As Exception
            Log.Debug("OPENgovH2O.MyUtility.GiraDataFromDB.errore: ", ex)
            GiraDataFromDB = ""
        End Try
    End Function
    Public Shared Function WriteFile(ByVal sFile As String, ByVal sDatiFile As String) As Integer
        Try
            Dim MyFileToWrite As IO.StreamWriter = IO.File.AppendText(sFile)

            MyFileToWrite.WriteLine(sDatiFile)
            MyFileToWrite.Flush()

            MyFileToWrite.Close()
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.MyUtility.WriteFile.errore: ", Err)
            Return 0
        End Try
    End Function
    'Public Function LogQuery(cmdMyCommand As SqlClient.SqlCommand) As String
    '    Try
    '        Return "OPENgovH2O.ExecQuery.query->con=" + cmdMyCommand.Connection.ConnectionString + " text=" + cmdMyCommand.CommandText + " " + Utility.Costanti.GetValParamCmd(cmdMyCommand)
    '    Catch ex As Exception
    '        Return "OPENgovH2O.LogQuery.errore:" + ex.Message
    '    End Try
    'End Function
    Public Function ImportoArrotondato(ByVal importo As Double) As String
        importo = (Int((importo * 100) + 0.5)) / 100
        ImportoArrotondato = CStr(importo)
    End Function
    Public Function ImportoArrotondatoEuroIntero(ByVal importo As String, ByRef ArrotondamentoEuro As Double) As String
        Dim nParteDecimale As Integer
        Dim nCifreDecimali As Integer
        Dim nParteIntera As Integer

        importo = importo.Replace(".", ",")
        Try
            If InStr(importo, ",") = 0 Then
                nCifreDecimali = 0
            Else
                nCifreDecimali = CInt(importo.Substring(InStr(importo, ","), importo.Length - InStr(importo, ",")).Length)
            End If

            If nCifreDecimali = 1 Then
                nParteDecimale = CInt(importo.Substring(InStr(importo, ","), 1)) * 10
                nParteIntera = CInt(importo.Substring(0, InStr(importo, ",") - 1))
            ElseIf nCifreDecimali = 0 Then
                nParteDecimale = 0
                nParteIntera = importo
            Else
                nParteDecimale = CInt(importo.Substring(InStr(importo, ","), 2))
                nParteIntera = CInt(importo.Substring(0, InStr(importo, ",") - 1))
            End If

            If nParteDecimale > 49 Then
                'ImportoArrotondatoEuroIntero = Int(CDbl(importo.Replace(",", "."))) + 1
                ImportoArrotondatoEuroIntero = nParteIntera + 1
                ArrotondamentoEuro = (100 - nParteDecimale) / 100
            ElseIf nParteDecimale = 0 Then
                'ImportoArrotondatoEuroIntero = importo.Replace(",", ".")
                ImportoArrotondatoEuroIntero = nParteIntera
                ArrotondamentoEuro = 0
            Else
                'ImportoArrotondatoEuroIntero = Int(CDbl(importo.Replace(",", ".")))
                ImportoArrotondatoEuroIntero = nParteIntera
                ArrotondamentoEuro = (-nParteDecimale) / 100
            End If
        Catch ex As Exception
            Log.Debug("OPENgovH2O.Generale.ImportoArrotondatoEuroIntero.errore: ", ex)
            Return importo
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myTable"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetMaxID(myTable As String) As Integer
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim myID As Integer = 1

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetMaxID", "MYTABLE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("MYTABLE", myTable))
                ctx.Dispose()
            End Using
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myID = Utility.StringOperation.FormatInt(myRow("maxid"))
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.MyUtility.GetMaxID.errore: ", ex)
        Finally
            dvMyDati.Dispose()
        End Try
        Return myID
    End Function
    'Public Function GetMaxID(myTable As String) As Integer
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow
    '    Dim myID As Integer = 1

    '    Try
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@MYTABLE", myTable)
    '        cmdMyCommand.CommandText = "prc_GetMaxID"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            If Not IsDBNull(dtMyRow("maxid")) Then
    '                myID = CInt(dtMyRow("maxid"))
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.MyUtility.GetMaxID.errore: ", ex)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '    Finally
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    '    Return myID
    'End Function
End Class
''' <summary>
''' Classe per la gestione delle variabili da sessione e da config
''' </summary>
Public Class ConstSession
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConstSession))
    Public Shared CountScript As Integer = 0

    Public Shared ReadOnly Property Ambiente() As String
        Get
            Try
                If (HttpContext.Current.Session("Ambiente") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("Ambiente").ToString
                End If
            Catch ex As Exception
                Log.Debug("OPENgovH2O.ConstSession.Ambiente.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property IdEnte() As String
        Get
            Try
                If (HttpContext.Current.Session("COD_ENTE") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("COD_ENTE").ToString
                End If
            Catch ex As Exception
                Log.Debug("OPENgovH2O.ConstSession.IdEnte.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property CodIstat() As String
        Get
            Try
                'If (HttpContext.Current.Session("CODICEISTAT") Is Nothing) Then
                '    Return ""
                'Else
                '    Return HttpContext.Current.Session("CODICEISTAT").ToString
                'End If
                Return IdEnte
            Catch ex As Exception
                Log.Debug("OPENgovH2O.ConstSession.CodIstat.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property DescrizioneEnte() As String
        Get
            Try
                If (HttpContext.Current.Session("DESCRIZIONE_ENTE") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("DESCRIZIONE_ENTE").ToString
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.DescrizioneEnte.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property Belfiore() As String
        Get
            Try
                If (HttpContext.Current.Session("COD_BELFIORE") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("COD_BELFIORE").ToString
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.Belfiore.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property CodTributo() As String
        Get
            'Try
            '    If (HttpContext.Current.Session("COD_TRIBUTO") Is Nothing) Then
            '        HttpContext.Current.Session("COD_TRIBUTO") = "9000"
            '        Return "9000"
            '    Else
            '        Return HttpContext.Current.Session("COD_TRIBUTO").ToString
            '    End If
            'Catch ex As Exception
            '     Log.Debug("OPENgovH2O.ConstSession.CodTributo.errore: ", ex)
            '    HttpContext.Current.Session("COD_TRIBUTO") = "9000"
            Return "9000"
            'End Try
        End Get
    End Property
    Public Shared ReadOnly Property DescrPeriodo() As String
        Get
            Try
                If (HttpContext.Current.Session("PERIODO") Is Nothing Or HttpContext.Current.Session("PERIODO") = "") Then
                    Dim FncGen As New ClsGenerale.Generale
                    FncGen.GetPeriodoAttuale
                End If
            Catch ex As Exception
                Log.Debug("OPENgovH2O.ConstSession.DescrPeriodo.errore: ", ex)
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale
            End Try
            Return HttpContext.Current.Session("PERIODO")
        End Get
    End Property
    Public Shared ReadOnly Property IdPeriodo() As Integer
        Get
            Try
                If (HttpContext.Current.Session("PERIODOID") Is Nothing) Then
                    Return 0
                Else
                    Return CInt(HttpContext.Current.Session("PERIODOID"))
                End If
            Catch ex As Exception
                Log.Debug("OPENgovH2O.ConstSession.IdPeriodo.errore: ", ex)
                Return 0
            End Try
        End Get
    End Property

    Public Shared ReadOnly Property BaseTempo() As Integer
        Get
            Try
                If (Not ConfigurationManager.AppSettings("BaseTempo") Is Nothing) Then
                    Return ConfigurationManager.AppSettings("BaseTempo")
                Else
                    Return 365
                End If
            Catch ex As Exception
                Log.Debug("OPENgov.ConstSession.BaseTempo.errore: ", ex)
                Return 365
            End Try
        End Get
    End Property
#Region "Path"
    Public Shared ReadOnly Property PathApplicazione() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_APPLICAZIONE") Is Nothing) Then
                    HttpContext.Current.Session("PATH_APPLICAZIONE") = ""
                    Return ""
                Else
                    HttpContext.Current.Session("PATH_APPLICAZIONE") = ConfigurationManager.AppSettings("PATH_APPLICAZIONE")
                    Return ConfigurationManager.AppSettings("PATH_APPLICAZIONE")
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.PathApplicazione.errore: ", ex)
                HttpContext.Current.Session("PATH_APPLICAZIONE") = ""
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property Path_H2O() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_OPENGOVH2O") Is Nothing) Then
                    Return "/OPENgovH2O"
                Else
                    Return ConfigurationManager.AppSettings("PATH_OPENGOVH2O").ToString()
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.Path_H20.errore: ", ex)
                Return "/OPENgovH2O"
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathProspetti() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_PROSPETTI_EXCEL") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_PROSPETTI_EXCEL").ToString()
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.PathProspetti.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathStampe() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("DIRTEMPLATE") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("DIRTEMPLATE").ToString()
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.PathStampe.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathVirtualStampe() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("DIRTEMPLATEVIRTUAL") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("DIRTEMPLATEVIRTUAL").ToString()
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.PathVirtualStampe.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathFileAE() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_FILE_AE") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_FILE_AE")
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovH2O.ConstSession.PathFileAE.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathRepository() As String
        Get
            Try
                If (Not ConfigurationManager.AppSettings("PathRepository") Is Nothing) Then
                    Return ConfigurationManager.AppSettings("PathRepository")
                Else
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug("OPENgov.ConstSession.PathRepository.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property

    Public Shared ReadOnly Property PathScarti() As String
        Get
            Return "Scarti\"
        End Get
    End Property
#End Region
#Region "Url Servizi"
    Public Shared ReadOnly Property UrlStradario() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("UrlPopUpStradario") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("UrlPopUpStradario").ToString()
                End If
            Catch ex As Exception
                Log.Debug("OPENgovH2O.ConstSession.UrlStradario.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlMotoreH2O() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLServizioRuoloH2O") Is Nothing) Then
                    Return "http://localhost:50011/MotoreH2O.rem"
                Else
                    Return ConfigurationManager.AppSettings("URLServizioRuoloH2O").ToString()
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.UrlMotoreH20.errore: ", ex)
                Return "thttp://localhost:50011/MotoreH2O.rem"
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlServizioStampeICI() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLElaborazioneDatiStampeICI") Is Nothing) Then
                    Return "tcp://localhost:33446/ElaborazioneStampeICI.rem"
                Else
                    Return ConfigurationManager.AppSettings("URLElaborazioneDatiStampeICI").ToString()
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.UrlServizioStampeICI.errore: ", ex)
                Return "tcp://localhost:33446/ElaborazioneStampeICI.rem"
            End Try
        End Get
    End Property
#End Region
#Region "stringhe connessione db"
    Public Shared ReadOnly Property DBType() As String
        Get
            Return "SQL"
        End Get
    End Property
    Public Shared ReadOnly Property StringConnection() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("connectionStringOPENgovH2O") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("connectionStringOPENgovH2O").ToString
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.StringConnection.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property StringConnectionOPENgov() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("connectionStringSQLOPENgov") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("connectionStringSQLOPENgov").ToString
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.StringConnectionOPENgov.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property StringConnectionAnagrafica() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("connectionStringSQLOPENAnagrafica") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("connectionStringSQLOPENAnagrafica")
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.StringConnectionAnagrafica.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
#End Region
#Region "parametri ribesframework"
    Public Shared ReadOnly Property DescrTipoProcServ() As String
        Get
            Try
                If (HttpContext.Current.Session("DESC_TIPO_PROC_SERV") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("DESC_TIPO_PROC_SERV").ToString
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.DescrTipoProcServ.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    'Public Shared ReadOnly Property ParametroEnv() As String
    '    Get
    '        Try
    '            If (HttpContext.Current.Session("PARAMETROENV") Is Nothing) Then
    '                Return ConfigurationManager.AppSettings("ParametroEnv").ToString
    '            Else
    '                Return HttpContext.Current.Session("PARAMETROENV").ToString
    '            End If
    '        Catch ex As Exception

    '            Log.Debug("OPENgovH2O.ConstSession.ParametroEnv.errore: ", ex)
    '            Return ConfigurationManager.AppSettings("ParametroEnv").ToString
    '        End Try
    '    End Get
    'End Property
    Public Shared ReadOnly Property UserName() As String
        Get
            Try
                If (HttpContext.Current.Session("username") Is Nothing) Then
                    Return ConfigurationManager.AppSettings("UserFramework").ToString
                Else
                    Return HttpContext.Current.Session("username").ToString
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.UserName.errore: ", ex)
                Return ConfigurationManager.AppSettings("UserName").ToString
            End Try
        End Get
    End Property
    'Public Shared ReadOnly Property IdentificativoApplicazione() As String
    '    Get
    '        Try
    '            If (HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") Is Nothing) Then
    '                Return ConfigurationManager.AppSettings("IDENTIFICATIVOAPPLICAZIONE").ToString
    '            Else
    '                Return HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE").ToString
    '            End If
    '        Catch ex As Exception

    '            Log.Debug("OPENgovH2O.ConstSession.IdentificativoApplicazione.errore: ", ex)
    '            Return ConfigurationManager.AppSettings("IDENTIFICATIVOAPPLICAZIONE").ToString
    '        End Try
    '    End Get
    'End Property
#End Region
#Region "GIS"
    Public Shared ReadOnly Property VisualGIS() As Boolean
        Get
            Try
                If (HttpContext.Current.Session("VisualGIS") Is Nothing) Then
                    Log.Debug("VisualGIS nothing quindi false")
                    Return False
                Else
                    Log.Debug("VisualGIS=" & CBool(HttpContext.Current.Session("VisualGIS")).ToString)
                    Return CBool(HttpContext.Current.Session("VisualGIS"))
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.VisualGIS.errore: ", ex)
                Return False
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlWSGIS() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("UrlWSGIS") Is Nothing) Then
                    Return "http://gis.oikosweb.com/CATWS/Gismappale.asmx"
                Else
                    Return ConfigurationManager.AppSettings("UrlWSGIS").ToString()
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.UrlWSGIS.errore: ", ex)
                Return "http://gis.oikosweb.com/CATWS/Gismappale.asmx"
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlWebGIS() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("UrlWebGIS") Is Nothing) Then
                    Return "http://map.portalecomuni.net/mapguide/wgis/ddd.html?&GisGuidMap="
                Else
                    If ConfigurationManager.AppSettings("UrlWebGIS").ToString().EndsWith("&GisGuidMap=") = False Then
                        Return ConfigurationManager.AppSettings("UrlWebGIS").ToString() + "&GisGuidMap="
                    Else
                        Return ConfigurationManager.AppSettings("UrlWebGIS").ToString()
                    End If
                End If
            Catch ex As Exception

                Log.Debug("OPENgovH2O.ConstSession.UrlWebGIS.errore: ", ex)
                Return "http://map.portalecomuni.net/mapguide/wgis/ddd.html?&GisGuidMap="
            End Try
        End Get
    End Property
#End Region
End Class