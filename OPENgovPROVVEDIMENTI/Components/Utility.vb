Imports System
Imports System.Data.SqlClient
Imports log4net

''' <summary>
''' Classe che incapsula tutti le utility necessarie alll'applicativo OPENgovProvvedimenti.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class MyUtility
    Private Shared Log As ILog = LogManager.GetLogger(GetType(MyUtility))
    Dim Constant As CostantiProvv
    ''' <summary>
    ''' Il metodo SelectIndexDropDownList consente di selezionare l'id di un elemento di un controllo DropDownList salvato nel database
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="strValue"></param>
    Private Sub SelectIndexDropDownList(ByVal cboTemp As DropDownList, ByVal strValue As String)

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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.SelectIndexDropDownList.errore: ", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Il metodo FillDropDownSQL consente di caricre un controllo DropDownList da un data reader caricato dal database
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="dr"></param>
    ''' <param name="lngSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQL(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal lngSelectedID As Long, ByVal strTesto As String)
        Try

            Dim myListItem As ListItem
            myListItem = New ListItem


            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem

                myListItem.Text = dr.GetString(1)
                myListItem.Value = dr.GetInt32(0)

                cboTemp.Items.Add(myListItem)

            End While

            If lngSelectedID <> Utility.Costanti.INIT_VALUE_NUMBER Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.FillDropDownSQL.errore: ", Err)
            Throw New Exception("Class Utility::Sub FillDropDownSQL::" & Err.Message)
        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try

    End Sub
    ''' <summary>
    ''' Il metodo FillDropDownSQL consente di caricre un controllo DropDownList da un data reader caricato dal database
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="ds"></param>
    ''' <param name="lngSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQL(ByVal cboTemp As DropDownList, ByVal ds As DataSet, ByVal lngSelectedID As Long, ByVal strTesto As String)
        Try
            Dim myListItem As New ListItem
            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            For Each myRow As DataRow In ds.Tables(0).Rows
                myListItem = New ListItem

                myListItem.Text = Utility.StringOperation.FormatString(myRow.Item(1))
                myListItem.Value = Utility.StringOperation.FormatInt(myRow.Item(0))

                cboTemp.Items.Add(myListItem)
            Next

            If lngSelectedID <> Utility.Costanti.INIT_VALUE_NUMBER Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.FillDropDownSQL.DataSet.errore: ", Err)
            Throw New Exception("Class Utility::Sub FillDropDownSQL::" & Err.Message)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="dr"></param>
    ''' <param name="strSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLValueString(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal strSelectedID As String, ByVal strTesto As String)
        Try
            Dim myListItem As ListItem
            myListItem = New ListItem

            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem

                myListItem.Text = dr.GetString(1)
                myListItem.Value = dr.GetString(0)

                cboTemp.Items.Add(myListItem)
            End While

            If strSelectedID <> CStr(Utility.Costanti.INIT_VALUE_NUMBER) Then
                SelectIndexDropDownList(cboTemp, strSelectedID)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.FillDropDownSQLValueString.errore: ", Err)
        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="ds"></param>
    ''' <param name="strSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLValueString(ByVal cboTemp As DropDownList, ByVal ds As DataSet, ByVal strSelectedID As String, ByVal strTesto As String)
        Try
            Dim myListItem As ListItem
            myListItem = New ListItem
            Dim iCount As Integer

            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)

            For iCount = 0 To ds.Tables(0).Rows.Count - 1
                myListItem = New ListItem

                myListItem.Text = ds.Tables(0).Rows(iCount).Item(1)
                myListItem.Value = ds.Tables(0).Rows(iCount).Item(0)

                cboTemp.Items.Add(myListItem)

            Next

            If strSelectedID <> CStr(Utility.Costanti.INIT_VALUE_NUMBER) Then
                SelectIndexDropDownList(cboTemp, strSelectedID)
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.FillDropDownSQLValueString.DataSet.errore: ", Err)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="dr"></param>
    ''' <param name="strSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLSingleString(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal strSelectedID As String, ByVal strTesto As String)
        Try

            Dim myListItem As ListItem
            myListItem = New ListItem


            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem

                myListItem.Text = dr.GetString(0)
                myListItem.Value = dr.GetString(0)

                cboTemp.Items.Add(myListItem)

            End While

            If strSelectedID <> CStr(Utility.Costanti.INIT_VALUE_NUMBER) Then
                SelectIndexDropDownList(cboTemp, strSelectedID)
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.FillDropDownSQLSingleString.errore: ", Err)
        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="ds"></param>
    ''' <param name="strSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLSingleString(ByVal cboTemp As DropDownList, ByVal ds As DataSet, ByVal strSelectedID As String, ByVal strTesto As String)
        Try

            Dim myListItem As ListItem
            myListItem = New ListItem
            Dim iCount As Integer

            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)

            For iCount = 0 To ds.Tables(0).Rows.Count - 1
                myListItem = New ListItem
                myListItem.Text = ds.Tables(0).Rows(iCount).Item(0)
                myListItem.Value = ds.Tables(0).Rows(iCount).Item(0)

                cboTemp.Items.Add(myListItem)
            Next

            If strSelectedID <> CStr(Utility.Costanti.INIT_VALUE_NUMBER) Then
                SelectIndexDropDownList(cboTemp, strSelectedID)
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.FillDropDownSQLSingleString.DataSet.errore: ", Err)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="ds"></param>
    ''' <param name="lngSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLValueStringCodDesc(ByVal cboTemp As DropDownList, ByVal ds As DataSet, ByVal lngSelectedID As String, ByVal strTesto As String)
        Try

            Dim myListItem As ListItem
            myListItem = New ListItem
            Dim iCount As Integer


            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            For iCount = 0 To ds.Tables(0).Rows.Count - 1
                myListItem = New ListItem

                myListItem.Text = ds.Tables(0).Rows(iCount).Item(0) & " - " & ds.Tables(0).Rows(iCount).Item(1)
                myListItem.Value = ds.Tables(0).Rows(iCount).Item(0)
                myListItem.Attributes.Add("title", ds.Tables(0).Rows(iCount).Item(0) & " - " & ds.Tables(0).Rows(iCount).Item(1))

                cboTemp.Items.Add(myListItem)

            Next

            If lngSelectedID <> CStr(Utility.Costanti.INIT_VALUE_NUMBER) Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.FillDropDownSQLValueStringCodDesc.DataSet.errore: ", Err)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="dr"></param>
    ''' <param name="lngSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLValueStringCodDesc(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal lngSelectedID As String, ByVal strTesto As String)
        Try

            Dim myListItem As ListItem
            myListItem = New ListItem


            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem

                myListItem.Text = dr.GetString(0) & " - " & dr.GetString(1)
                myListItem.Value = dr.GetString(0)
                myListItem.Attributes.Add("title", dr.GetString(0) & " - " & dr.GetString(1))

                cboTemp.Items.Add(myListItem)

            End While

            If lngSelectedID <> CStr(Utility.Costanti.INIT_VALUE_NUMBER) Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.FillDropDownSQLValueStringCodDesc.errore: ", Err)
        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="dr"></param>
    ''' <param name="lngSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLString(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal lngSelectedID As Long, ByVal strTesto As String)
        Try

            Dim myListItem As ListItem
            myListItem = New ListItem


            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem

                myListItem.Text = dr.GetString(0)
                myListItem.Value = dr.GetString(0)
                cboTemp.Items.Add(myListItem)

            End While

            If lngSelectedID <> Utility.Costanti.INIT_VALUE_NUMBER Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.FillDropDownSQLString.errore: ", Err)
        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="vInput"></param>
    ''' <param name="blnClearSpace"></param>
    ''' <returns></returns>
    Public Function CStrToDB(ByVal vInput As Object, Optional ByRef blnClearSpace As Boolean = False) As String
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
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.CStrToDB.errore: ", Err)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="vInput"></param>
    ''' <returns></returns>
    Public Function CDoubleToDB(ByVal vInput As Object) As String

        Dim strToDbl As String = "Null"
        Try
            If Not IsDBNull(vInput) And Not IsNothing(vInput) Then

                strToDbl = CStr(vInput)
                strToDbl = Replace(strToDbl, ",", ".")
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.CDoubleToDB.errore: ", Err)
        End Try
        Return strToDbl
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="vInput"></param>
    ''' <returns></returns>
    Public Function CBoolToDB(ByVal vInput As Object) As Integer

        Dim blnToDB As Boolean = False
        Try
            If Not IsDBNull(vInput) And Not IsNothing(vInput) Then
                blnToDB = vInput
                CBoolToDB = Convert.ToInt32(blnToDB)
            Else
                CBoolToDB = 0
            End If

            Return CBoolToDB
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.CBoolToDB.errore: ", Err)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="vInput"></param>
    ''' <returns></returns>
    Public Function CStrToDBForIn(ByVal vInput As String) As String
        Dim sTesto As String
        Dim i As Integer

        CStrToDBForIn = "''"
        Dim arrayIn() As String
        arrayIn = Split(vInput, ",")
        Try
            For i = -1 To UBound(arrayIn) - 1
                sTesto = sTesto & "'" & arrayIn(i + 1) & "',"
            Next

            sTesto = Left(sTesto, Len(sTesto) - 1)

            Return sTesto
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.CStrToDBForIn.errore: ", Err)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strInput"></param>
    ''' <returns></returns>
    Public Function CToStr(ByRef strInput As Object) As String

        CToStr = ""
        Try
            If Not IsDBNull(strInput) And Not IsNothing(strInput) Then
                CToStr = CStr(strInput)
            End If

            Return CToStr
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.CToStr.errore: ", Err)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    Public Function GiraData(ByVal data As String) As String
        'leggo la data nel formato gg/mm/aaaa e la metto nel formato aaaammgg
        Dim Giorno As String
        Dim Mese As String
        Dim Anno As String
        Try
            If data <> "" Then
                Giorno = Mid(data, 1, 2)
                Mese = Mid(data, 4, 2)
                Anno = Mid(data, 7, 4)
                Return Anno & Mese & Giorno
            Else
                Return ""
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.GiraData.errore: ", Err)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    Public Function GiraDataFromDB(ByVal data As String) As String
        'leggo la data nel formato aaaammgg  e la metto nel formato gg/mm/aaaa
        Dim Giorno As String
        Dim Mese As String
        Dim Anno As String
        GiraDataFromDB = ""
        Try
            If data <> "" Then
                Giorno = Mid(data, 7, 2)
                Mese = Mid(data, 5, 2)
                Anno = Mid(data, 1, 4)
                GiraDataFromDB = Giorno & "/" & Mese & "/" & Anno
                If Utility.StringOperation.FormatDateTime(GiraDataFromDB).ToShortDateString = DateTime.MaxValue.ToShortDateString Or Utility.StringOperation.FormatDateTime(GiraDataFromDB).ToShortDateString = DateTime.MinValue.ToShortDateString Then
                    GiraDataFromDB = ""
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.GiraDataFromDB.errore: ", Err)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    Public Function GiraDataCompletaFromDB(ByVal data As String) As String
        'leggo la data nel formato aaaa/mm/gg  e la metto nel formato gg/mm/aaaa
        Dim Giorno As String
        Dim Mese As String
        Dim Anno As String
        GiraDataCompletaFromDB = ""
        Try
            If data <> "" Then
                Giorno = Mid(data, 9, 2)
                Mese = Mid(data, 6, 2)
                Anno = Mid(data, 1, 4)
                GiraDataCompletaFromDB = Giorno & "/" & Mese & "/" & Anno
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.GiraDataCompletaFromDB.errore: ", Err)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objInput"></param>
    ''' <returns></returns>
    Public Function cToDbl(ByRef objInput As Object) As Double

        cToDbl = 0
        Try
            If Not IsDBNull(objInput) And Not IsNothing(objInput) Then
                If IsNumeric(objInput) Then
                    cToDbl = CDbl(objInput)
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.cToDbl.errore: ", Err)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myString"></param>
    ''' <returns></returns>
    Public Function ReplaceDataForDB(ByVal myString As String) As String
        Dim sReturn As String
        Try
            sReturn = CDate(myString).ToString(ConfigurationManager.AppSettings("lingua_date")).Replace(".", ":")
            Return sReturn
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.MyUtility.ReplaceDataForDB.errore: ", ex)
            Throw ex
            Exit Function
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myString"></param>
    ''' <returns></returns>
    Public Function ReplaceCharForFile(ByVal myString As String) As String
        Dim sReturn As String

        sReturn = myString
        sReturn = sReturn.Replace("à", "a'")
        sReturn = sReturn.Replace("é", "e'")
        sReturn = sReturn.Replace("è", "e'")
        sReturn = sReturn.Replace("ì", "i'")
        sReturn = sReturn.Replace("ò", "o'")
        sReturn = sReturn.Replace("ù", "u'")

        sReturn = sReturn.Replace("à".ToUpper, "a'".ToUpper)
        sReturn = sReturn.Replace("é".ToUpper, "e'".ToUpper)
        sReturn = sReturn.Replace("è".ToUpper, "e'".ToUpper)
        sReturn = sReturn.Replace("ì".ToUpper, "i'".ToUpper)
        sReturn = sReturn.Replace("ò".ToUpper, "o'".ToUpper)
        sReturn = sReturn.Replace("ù".ToUpper, "u'".ToUpper)

        sReturn = sReturn.Replace("ç", "c")
        sReturn = sReturn.Replace("°", "")
        sReturn = sReturn.Replace("€", "euro")
        sReturn = sReturn.Replace("£", "lire")

        Return sReturn
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sFile"></param>
    ''' <param name="sDatiFile"></param>
    ''' <returns></returns>
    Public Function WriteFile(ByVal sFile As String, ByVal sDatiFile As String) As Integer
        Try
            Dim MyFileToWrite As IO.StreamWriter = IO.File.AppendText(sFile)

            MyFileToWrite.WriteLine(sDatiFile)
            MyFileToWrite.Flush()

            MyFileToWrite.Close()
            Return 1
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in WriteFile::" & Err.Message)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="FileName"></param>
    ''' <returns></returns>
    Public Function DeleteFile(ByVal FileName As String) As Boolean
        Try
            If IO.File.Exists(FileName) = True Then
                IO.File.Delete(FileName)
            End If
            Return True
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in DeleteFile::" & Err.Message)
            Return False
        End Try
    End Function
End Class
