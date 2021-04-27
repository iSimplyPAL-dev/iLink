Imports log4net

Namespace generalClass
    ''' <summary>
    ''' Classe per le funzioni generali delle combo
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    Public Class loadComboBox
        Private Shared Log As ILog = LogManager.GetLogger(GetType(loadComboBox))
        Public Sub loadCombo(ByVal objCombo As DropDownList, ByVal objDW As DataView)
            Dim indexValue, indexText As Integer
            Try
                If objDW.Table.Columns.Count = 0 Then
                indexValue = 0
                indexText = 0
            End If

                If objDW.Count <> 0 Then
                    Dim iConteggio As Integer = 0
                    objCombo.Items.Clear()
                    objCombo.Items.Add("...")
                    objCombo.Items(0).Value = ""
                    Do While iConteggio <= objDW.Count - 1
                        objCombo.Items.Add(objDW.Item(iConteggio)(indexText))
                        objCombo.Items(objCombo.Items.Count - 1).Value = objDW.Item(iConteggio)(indexValue)
                        iConteggio = iConteggio + 1
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.loadComboBox.loadCombo.errore: ", ex)
            End Try
        End Sub

        Public Sub loadDropDownList(ByVal objCombo As DropDownList, ByVal objDW As DataView)
            Dim indexValue, indexText As Integer
            Try
                If objDW.Table.Columns.Count = 0 Then
                    indexValue = 0
                    indexText = 0
                Else
                    indexValue = 1
                    indexText = 0
                End If

                'objCombo.Items.Clear()
                'objCombo.Items.Add("...")
                'objCombo.Items(0).Value = ""

                If objDW.Count <> 0 Then
                    Dim iConteggio As Integer = 0
                    Do While iConteggio <= objDW.Count - 1
                        objCombo.Items.Add(objDW.Item(iConteggio)(indexText))
                        objCombo.Items(objCombo.Items.Count - 1).Value = objDW.Item(iConteggio)(indexValue)
                        iConteggio = iConteggio + 1
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.loadComboBox.loadDropDownList.errore: ", ex)
            End Try
        End Sub

        Public Sub loadDropDownListConcat(ByVal objCombo As DropDownList, ByVal objDW As DataView)
            Dim indexValue, indexText As Integer
            Try
                If objDW.Table.Columns.Count = 0 Then
                    indexValue = 0
                    indexText = 0
                Else
                    indexValue = 1
                    indexText = 0
                End If

                'objCombo.Items.Clear()
                'objCombo.Items.Add("...")
                'objCombo.Items(0).Value = ""

                If objDW.Count <> 0 Then
                    Dim iConteggio As Integer = 0
                    Do While iConteggio <= objDW.Count - 1
                        objCombo.Items.Add(objDW.Item(iConteggio)(indexValue) & " - " & objDW.Item(iConteggio)(indexText))
                        objCombo.Items(objCombo.Items.Count - 1).Value = objDW.Item(iConteggio)(indexValue)
                        iConteggio = iConteggio + 1
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.loadComboBox.loadDropDownListConcat.errore: ", ex)
            End Try
        End Sub

        Public Sub loadDropDownListConcatConTipologia(ByVal objCombo As DropDownList, ByVal objDW As DataView)
            Dim indexValue, indexText, indexField As Integer
            Try
                If objDW.Table.Columns.Count = 0 Then
                    indexValue = 0
                    indexText = 0
                    indexField = 0
                Else
                    indexField = 2
                    indexValue = 1
                    indexText = 0
                End If

                'objCombo.Items.Clear()
                'objCombo.Items.Add("...")
                'objCombo.Items(0).Value = ""

                If objDW.Count <> 0 Then
                    Dim iConteggio As Integer = 0
                    Do While iConteggio <= objDW.Count - 1
                        objCombo.Items.Add(objDW.Item(iConteggio)(indexValue) & " - " & objDW.Item(iConteggio)(indexText) & "  -  TIPOLOGIA RIDUZIONE:" & objDW.Item(iConteggio)(indexField))
                        objCombo.Items(objCombo.Items.Count - 1).Value = objDW.Item(iConteggio)(indexValue)
                        iConteggio = iConteggio + 1
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.loadComboBox.loadDropDownListConcatConTipologia.errore: ", ex)
            End Try
        End Sub

        Public Sub loadDropDownListConcatRowEmpty(ByVal objCombo As DropDownList, ByVal objDW As DataView)
            Dim indexValue, indexText As Integer
            Try
                If objDW.Table.Columns.Count = 0 Then
                    indexValue = 0
                    indexText = 0
                Else
                    indexValue = 1
                    indexText = 0
                End If

                objCombo.Items.Clear()
                objCombo.Items.Add("...")
                objCombo.Items(0).Value = ""

                If objDW.Count <> 0 Then
                    Dim iConteggio As Integer = 0
                    Do While iConteggio <= objDW.Count - 1
                        objCombo.Items.Add(objDW.Item(iConteggio)(indexValue) & " - " & objDW.Item(iConteggio)(indexText))
                        objCombo.Items(objCombo.Items.Count - 1).Value = objDW.Item(iConteggio)(indexValue)
                        iConteggio = iConteggio + 1
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.loadComboBox.loadDropDownListConcatRowEmpty.errore: ", ex)
            End Try
        End Sub

        Public Sub loadDropDownListConcatCATEGORIADOM(ByVal objCombo As DropDownList, ByVal objDW As DataView)
            Dim indexValue, indexText As Integer
            Try
                If objDW.Table.Columns.Count = 0 Then
                    indexValue = 0
                    indexText = 0
                Else
                    indexValue = 1
                    indexText = 0
                End If

                'objCombo.Items.Clear()
                objCombo.Items.Add("DOM - DOMESTICA")
                objCombo.Items(0).Value = "DOM"

                If objDW.Count <> 0 Then
                    Dim iConteggio As Integer = 0
                    Do While iConteggio <= objDW.Count - 1
                        objCombo.Items.Add(objDW.Item(iConteggio)(indexValue) & " - " & objDW.Item(iConteggio)(indexText))
                        objCombo.Items(objCombo.Items.Count - 1).Value = objDW.Item(iConteggio)(indexValue)
                        iConteggio = iConteggio + 1
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.loadComboBox.loadDropDownListConcatCATEGORIADOM.errore: ", ex)
            End Try
        End Sub

        Public Sub loadDropDownListConcatCATEGORIADOMRowEmpty(ByVal objCombo As DropDownList, ByVal objDW As DataView)
            Dim indexValue, indexText As Integer
            Try
                If objDW.Table.Columns.Count = 0 Then
                    indexValue = 0
                    indexText = 0
                Else
                    indexValue = 1
                    indexText = 0
                End If

                'objCombo.Items.Clear()
                objCombo.Items.Clear()
                objCombo.Items.Add("...")
                objCombo.Items(0).Value = ""
                objCombo.Items.Add("DOM - DOMESTICA")
                objCombo.Items(1).Value = "DOM"

                If objDW.Count <> 0 Then
                    Dim iConteggio As Integer = 0
                    Do While iConteggio <= objDW.Count - 1
                        objCombo.Items.Add(objDW.Item(iConteggio)(indexValue) & " - " & objDW.Item(iConteggio)(indexText))
                        objCombo.Items(objCombo.Items.Count - 1).Value = objDW.Item(iConteggio)(indexValue)
                        iConteggio = iConteggio + 1
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.loadComboBox.loadDropDownListConcatCATEGORIADOMRowEmpty.errore: ", ex)
            End Try
        End Sub


        Public Sub loadComboAll(ByVal objCombo As DropDownList, ByVal objDW As DataView)
            Dim indexValue, indexText As Integer
            Try
                If objDW.Table.Columns.Count = 0 Then
                    indexValue = 0
                    indexText = 0
                End If

                If objDW.Count <> 0 Then
                    Dim iConteggio As Integer = 0
                    objCombo.Items.Clear()
                    objCombo.Items.Add("...")
                    objCombo.Items(0).Value = ""
                    objCombo.Items.Add("TUTTE")
                    objCombo.Items(1).Value = "0"
                    Do While iConteggio <= objDW.Count - 1
                        objCombo.Items.Add(objDW.Item(iConteggio)(indexText))
                        objCombo.Items(objCombo.Items.Count - 1).Value = objDW.Item(iConteggio)(indexValue)
                        iConteggio = iConteggio + 1
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.loadComboBox.loadComboAll.errore: ", ex)
            End Try
        End Sub

        Public Sub loadComboAnnoTermico(ByVal objCombo As DropDownList, ByVal objDW As DataView)
            Dim indexValue, indexText As Integer
            Try
                If objDW.Table.Columns.Count = 0 Then
                    indexValue = 0
                    indexText = 0
                End If

                If objDW.Count <> 0 Then
                    Dim iConteggio As Integer = 0
                    objCombo.Items.Clear()
                    objCombo.Items.Add("...")
                    objCombo.Items(0).Value = ""
                    Do While iConteggio <= objDW.Count - 1
                        objCombo.Items.Add(Mid(objDW.Item(iConteggio)(indexText), 1, 4) & "/" & Mid(objDW.Item(iConteggio)(indexText), 5, 4))
                        objCombo.Items(objCombo.Items.Count - 1).Value = objDW.Item(iConteggio)(indexValue)
                        iConteggio = iConteggio + 1
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.loadComboBox.loadComboAnnoTermico.errore: ", ex)
            End Try
        End Sub


    End Class
End Namespace