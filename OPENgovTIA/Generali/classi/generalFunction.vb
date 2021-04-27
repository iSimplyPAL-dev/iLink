Imports log4net
Imports System
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.TextBox
Imports Utility

Namespace generalClass
    ''' <summary>
    ''' Classe per le funzioni generali di utilità
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    Public Class generalFunction
        Private Shared Log As ILog = LogManager.GetLogger(GetType(generalFunction))

        Public Function ReplaceCharsForSearch(ByVal objText As TextBox) As String
            Dim returnStr As String
            returnStr = ReplaceChar(objText.Text)
            Return returnStr
        End Function

        Public Function ReplaceCharsForSearch(ByVal objStr As String) As String
            Dim returnStr As String
            returnStr = ReplaceChar(objStr)
            Return returnStr
        End Function

        'Parametri passati all'url
        Public Function ReplaceCharsForParameters(ByVal objStr As String) As String
            Dim returnStr As String
            returnStr = Replace(objStr, "%", "*")
            returnStr = Replace(objStr, "''", "'")
            returnStr = Trim(returnStr)
            Return returnStr
        End Function

        Public Function ReplaceChar(ByVal str As String) As String
            Dim returnStr As String
            returnStr = Replace(str, "'", "''")
            returnStr = Replace(returnStr, "*", "%")
            returnStr = Replace(returnStr, "&nbsp;", " ")
            returnStr = Trim(returnStr)
            Return returnStr
        End Function

        Public Function ReplaceNumberForDB(ByVal sNumber As String) As String
            Try
                Dim sFormatNumber As String

                sFormatNumber = sNumber.Replace(",", ".")

                Return sFormatNumber
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.generalFunction.ReplaceNumberForDB.errore: ", ex)
                Throw ex
                Exit Function
            End Try
        End Function

        Public Sub showText(ByRef ppp As Page)
            Dim Control As Control
            Try
                For Each Control In ppp.Controls
                    If TypeOf Control Is TextBox Then
                        Control.Visible = False
                    End If
                Next
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.generalFunction.showText.errore: ", ex)
            End Try
        End Sub

        Public Function ReplaceDataForDB(ByVal myString As String) As String
            Dim sReturn As String
            Try
                sReturn = CDate(myString).ToString(ConfigurationManager.AppSettings("lingua_date")).Replace(".", ":")
                Return sReturn
            Catch ex As Exception
                Throw ex
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.generalFunction.ReplaceDataForDB.errore: ", ex)
                Exit Function
            End Try
        End Function

        Public Function FormattaData(ByVal data As String, ByVal sTypeFormat As String) As String
            'STYPEFORMAT
            'G=leggo la data nel formato aaaammgg  e la metto nel formato gg/mm/aaaa
            'A=leggo la data nel formato gg/mm/aaaa  e la metto nel formato aaaammgg
            Dim Giorno As String
            Dim Mese As String
            Dim Anno As String
            Try
                If data <> "" Then
                    Select Case sTypeFormat
                        Case "G"
                            Giorno = Mid(data, 7, 2)
                            Mese = Mid(data, 5, 2)
                            Anno = Mid(data, 1, 4)
                            Return Giorno & "/" & Mese & "/" & Anno
                        Case "A"
                            Try
                                Giorno = CStr(CDate(data).Day).PadLeft(2, "0")
                                Mese = CStr(CDate(data).Month).PadLeft(2, "0")
                                Anno = CStr(CDate(data).Year).PadLeft(4, "0")
                            Catch ex As Exception
                                Giorno = Mid(data, 1, 2)
                                Mese = Mid(data, 4, 2)
                                Anno = Mid(data, 7, 4)
                            End Try
                            Return Anno & Mese & Giorno
                        Case "E"
                            Mese = Mid(data, 1, 2)
                            Giorno = Mid(data, 4, 2)
                            Anno = Mid(data, 7, 4)
                            Return Anno & Mese & Giorno
                        Case Else
                            Return ""
                    End Select
                Else
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.generalFunction.FormattaData.errore: ", ex)
                Return ""
            End Try
        End Function

        'Public Function PopolaJSdisabilita(ByVal sArray As Array) As String
        '    Dim StrMid As String
        '    Dim StrMidTotale As String = ""
        '    Dim i As Integer

        '    Try
        '        For i = 0 To sArray.Length - 1
        '            StrMid = "document.getElementById('" & sArray(i).ToString() & "').disabled=true;"
        '            StrMidTotale += StrMid
        '        Next

        '        Return StrMidTotale
        '    Catch ex As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.generalFunction.PopolaJSdisabilita.errore: ", ex)
        '        Return ""
        '    End Try
        'End Function

        Public Function WriteFile(ByVal sFile As String, ByVal sDatiFile As String) As Integer
            Try
                Dim MyFileToWrite As IO.StreamWriter = IO.File.AppendText(sFile)

                MyFileToWrite.WriteLine(sDatiFile)
                MyFileToWrite.Flush()

                MyFileToWrite.Close()
                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.generalFunction.WriteFile.errore: ", Err)
                Return 0
            End Try
        End Function

        Public Sub LoadComboConfig(ByVal ddlMyDati As DropDownList, ByVal sMyTabella As String, ByVal sIdEnte As String, ByVal MyStringConnection As String)
            Dim SQL As String

            Try
                SQL = "SELECT CODICE + ' - ' + DESCRIZIONE AS DESCRIZIONE, CODICE"
                SQL += " FROM " & sMyTabella
                If sIdEnte <> "" Then
                    SQL += " WHERE (IDENTE='" & sIdEnte & "')"
                End If
                LoadComboGenerale(ddlMyDati, SQL, MyStringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.generalFunction.LoadComboConfig.errore: ", ex)
                Throw New Exception("Problemi nell'esecuzione di LoadComboCategorie " + ex.Message)
            End Try
        End Sub

        Public Sub LoadComboGenerale(ByVal ddl As DropDownList, ByVal sSQL As String, ByVal MyStringConnection As String, bAddDefaultValue As Boolean, TypeDefaultValue As Costanti.TipoDefaultCmb)
            Dim myDataView As New DataView

            Try
                Using ctx As New DBModel(ConstSession.DBType, MyStringConnection)
                    Try
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.View, sSQL)
                        myDataView = ctx.GetDataView(sSQL, "TBL")
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.generalFunction.LoadComboGenerale.erroreQuery: ", ex)
                    Finally
                        ctx.Dispose()
                    End Try
                    loadCombo(ddl, myDataView, bAddDefaultValue, TypeDefaultValue)
                End Using
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.generalFunction.LoadComboGenerale.errore: ", Err)
            Finally
                myDataView.Dispose()
            End Try
        End Sub
        'Public Sub LoadComboGenerale(ByVal ddl As DropDownList, ByVal sSQL As String, ByVal MyStringConnection As String, bAddDefaultValue As Boolean, TypeDefaultValue As Costanti.TipoDefaultCmb)
        '    Dim cmdMyCommand As New SqlClient.SqlCommand
        '    Dim myDataReader As SqlClient.SqlDataReader

        '    Try
        '        cmdMyCommand.CommandType = CommandType.Text
        '        cmdMyCommand.CommandTimeout = 0
        '        'Valorizzo la connessione
        '        cmdMyCommand.Connection = New SqlClient.SqlConnection(MyStringConnection)
        '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
        '            cmdMyCommand.Connection.Open()
        '        End If
        '        cmdMyCommand.CommandText = sSQL
        '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
        '        myDataReader = cmdMyCommand.ExecuteReader
        '        'eseguo la query
        '        loadCombo(ddl, myDataReader, bAddDefaultValue, TypeDefaultValue)
        '    Catch ex As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.generalFunction.LoadComboGenerale.errore: ", ex)
        '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
        '    Finally
        '        cmdMyCommand.Connection.Close()
        '        cmdMyCommand.Dispose()
        '    End Try
        'End Sub

        Public Sub loadCombo(ByVal objCombo As DropDownList, ByVal objDR As SqlClient.SqlDataReader, bAddDefaultValue As Boolean, TypeDefaultValue As Costanti.TipoDefaultCmb)
            Try
                objCombo.Items.Clear()
                If bAddDefaultValue Then
                    objCombo.Items.Add("...")
                    If TypeDefaultValue = Costanti.TipoDefaultCmb.STRINGA Then
                        objCombo.Items(0).Value = ""
                    Else
                        objCombo.Items(0).Value = "-1"
                    End If
                End If
                If Not objDR Is Nothing Then
                    Do While objDR.Read
                        If Not IsDBNull(objDR(0)) Then
                            objCombo.Items.Add(objDR(0))
                            objCombo.Items(objCombo.Items.Count - 1).Value = objDR(1)
                        End If
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.generalFunction.loadCombo.errore: ", ex)
            End Try
        End Sub

        Public Sub loadCombo(ByVal objCombo As DropDownList, ByVal objDW As DataView, bAddDefaultValue As Boolean, TypeDefaultValue As Costanti.TipoDefaultCmb)
            Try
                objCombo.Items.Clear()
                If bAddDefaultValue Then
                    objCombo.Items.Add("...")
                    If TypeDefaultValue = Costanti.TipoDefaultCmb.STRINGA Then
                        objCombo.Items(0).Value = ""
                    Else
                        objCombo.Items(0).Value = "-1"
                    End If
                End If
                If Not objDW Is Nothing Then
                    If objDW.Count <> 0 Then
                        Dim iConteggio As Integer = 0
                        Do While iConteggio <= objDW.Count - 1
                            objCombo.Items.Add(objDW.Item(iConteggio)(0))
                            objCombo.Items(objCombo.Items.Count - 1).Value = objDW.Item(iConteggio)(1)
                            iConteggio = iConteggio + 1
                        Loop
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.generalFunction.loadCombo.errore: ", ex)
            End Try
        End Sub
    End Class
End Namespace
