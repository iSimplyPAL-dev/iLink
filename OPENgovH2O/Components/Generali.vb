Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Public Class Generali
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(Generali))
    Dim _Const As New Costanti

    Public Sub LoadDropDownList(ByVal cboTemp As DropDownList, ByVal dsTemp As DataSet, ByVal DataValueField As String, ByVal DataTextField As String, ByVal lngSelectedID As Long)
        Try
            Dim dt As DataTable = dsTemp.Tables(0)
            Dim rowNull As DataRow = dt.NewRow()
            rowNull(DataTextField) = "..."
            rowNull(DataValueField) = "-1"
            dsTemp.Tables(0).Rows.InsertAt(rowNull, 0)

            cboTemp.DataSource = dsTemp
            cboTemp.DataValueField = DataValueField
            cboTemp.DataTextField = DataTextField
            cboTemp.DataBind()

            If lngSelectedID <> -1 Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Generali.LoadDropDownList.errore: ", ex)
        End Try
    End Sub

    'Public Function ROT13Encode(ByVal InputText As String) As String
    '    Try
    '        Dim i As Integer
    '        Dim CurrentCharacter As Char
    '        Dim CurrentCharacterCode As Integer
    '        Dim EncodedText As String = ""

    '        'Iterate through the length of the input parameter  
    '        For i = 0 To InputText.Length - 1
    '            'Convert the current character to a char  
    '            CurrentCharacter = System.Convert.ToChar(InputText.Substring(i, 1))

    '            'Get the character code of the current character  
    '            CurrentCharacterCode = Microsoft.VisualBasic.Asc(CurrentCharacter)

    '            'Modify the character code of the character, - this  
    '            'so that "a" becomes "n", "z" becomes "m", "N" becomes "Y" and so on  
    '            If CurrentCharacterCode >= 97 And CurrentCharacterCode <= 109 Then
    '                CurrentCharacterCode = CurrentCharacterCode + 13

    '            Else
    '                If CurrentCharacterCode >= 110 And CurrentCharacterCode <= 122 Then
    '                    CurrentCharacterCode = CurrentCharacterCode - 13

    '                Else
    '                    If CurrentCharacterCode >= 65 And CurrentCharacterCode <= 77 Then
    '                        CurrentCharacterCode = CurrentCharacterCode + 13

    '                    Else
    '                        If CurrentCharacterCode >= 78 And CurrentCharacterCode <= 90 Then
    '                            CurrentCharacterCode = CurrentCharacterCode - 13
    '                        End If
    '                    End If
    '                End If    'Add the current character to the string to be returned 
    '            End If
    '            EncodedText = EncodedText + Microsoft.VisualBasic.ChrW(CurrentCharacterCode)
    '        Next i

    '        Return EncodedText
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Generali.ROT13Encode.errore: ", ex)
    '    End Try
    'End Function

    Public Sub FillDropDownSQL(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal lngSelectedID As Long)
        Try
            Dim myListItem As ListItem
            myListItem = New ListItem
            myListItem.Text = "..."
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem

                myListItem.Text = CStr(dr.Item(1)) & "--" & "[" & CStr(dr.Item(0)) & "]"
                myListItem.Value = dr.Item(0)
                cboTemp.Items.Add(myListItem)

            End While

            If lngSelectedID <> -1 Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If

        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Generali.FillDropDownSQL.errore: ", ex)
        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try
    End Sub
    Public Sub FillDropDownSQL(ByVal cboTemp As DropDownList, ByVal dvMyDati As DataView, ByVal lngSelectedID As Long)
        Try
            Dim myListItem As ListItem
            myListItem = New ListItem
            myListItem.Text = "..."
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            For Each myRow As DataRowView In dvMyDati
                myListItem = New ListItem

                myListItem.Text = CStr(myRow(1)) & "--" & "[" & CStr(myRow(0)) & "]"
                myListItem.Value = myRow(0)
                cboTemp.Items.Add(myListItem)
            Next

            If lngSelectedID <> -1 Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Generali.FillDropDownSQL.DataView.errore: ", ex)
        Finally
            If Not dvMyDati Is Nothing Then
                dvMyDati.Dispose()
            End If
        End Try
    End Sub
    Public Sub FillDropDownSQLStrade(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal lngSelectedID As Long)
        Try
            Dim myListItem As ListItem
            myListItem = New ListItem
            myListItem.Text = "..."
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem

                myListItem.Text = dr.GetString(2) & "--" & dr.GetString(1)
                myListItem.Value = dr.GetInt32(0)
                cboTemp.Items.Add(myListItem)
            End While

            If lngSelectedID <> -1 Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If

        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Generali.FillDropDownSQLStrade.errore: ", ex)
        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try
    End Sub

    Public Sub SelectIndexDropDownList(ByVal cboTemp As DropDownList, ByVal strValue As String)
        Dim blnFindElement As Boolean = False
        Dim intCount As Integer = 0
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

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Generali.SelectIndexDropDownlist.errore: ", ex)
        End Try
    End Sub
End Class
