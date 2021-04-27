Imports System
Imports System.Data.SqlClient
Imports log4net

Namespace DLL
    ''' <summary>
    ''' Classe per le funzioni generali di utilità
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    Public Class ProvvUtility
        Private Shared Log As ILog = LogManager.GetLogger(GetType(ProvvUtility))
        'Dim _Const As New Costanti

        Public Shared Function GetParametro(ByRef strInput As Object) As String
            GetParametro = ""
            Try
                If Not IsDBNull(strInput) And Not IsNothing(strInput) Then
                    GetParametro = CStr(strInput)
                End If
                Return GetParametro
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.GetParametro.errore: ", ex)
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
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.cToIng.errore: ", ex)
            End Try
        End Function

        Public Shared Function cToDbl(ByRef objInput As Object) As Double
            cToDbl = 0
            Try
                If Not IsDBNull(objInput) And Not IsNothing(objInput) Then
                    If IsNumeric(objInput) Then
                        cToDbl = CDbl(objInput)
                        cToDbl = "" & Replace(cToDbl, ",", ".") & ""
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.cToDbl.errore: ", ex)
            End Try
        End Function

        Public Shared Function cToBool(ByRef objInput As Object) As Boolean

            cToBool = False
            Try
                If Not IsDBNull(objInput) And Not IsNothing(objInput) Then
                    cToBool = CBool(objInput)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.cToBool.errore: ", ex)
            End Try
        End Function


        Public Shared Function cToDate(ByRef objInput As Object) As Date

            cToDate = System.DateTime.FromOADate(0)
            Try
                If Not IsDBNull(objInput) And Not IsNothing(objInput) Then
                    If IsDate(objInput) Then
                        cToDate = CDate(objInput)
                    End If

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.cToDate.errore: ", ex)
            End Try
        End Function
        Public Function cDateToClient(ByRef vrnDateTime As Object) As String
            Dim strData As String

            cDateToClient = ""
            Try

                If Not IsDBNull(vrnDateTime) And Not IsNothing(vrnDateTime) Then
                    strData = CStr(vrnDateTime)
                    If IsDate(strData) Then
                        cDateToClient = Format(Day(CDate(strData)), "00") & "/" & Format(Month(CDate(strData)), "00") & "/" & Format(Year(CDate(strData)), "0000")
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.cDateToClient.errore: ", ex)
            End Try
        End Function

        Public Function CToString(ByVal strInput As Object) As String

            CToString = ""
            Try
                If Not IsDBNull(strInput) And Not IsNothing(strInput) Then
                    CToString = CStr(strInput)
                End If

                Return CToString
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.CToString.errore: ", ex)
            End Try
        End Function

        Public Function CToStr(ByRef strInput As String) As Object

            CToStr = System.DBNull.Value
            Try
                If Len(strInput) > 0 Then
                    CToStr = strInput
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.CToStr.errore: ", ex)
            End Try
        End Function


        Public Function CDateToDB(ByVal vInput As Object, Optional ByRef blnFormatoInputServer As Boolean = False) As String
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
                        sTesto = Format(Day(CDate(sTesto)), "00") & "/" & Format(Month(CDate(sTesto)), "00") & "/" & Format(Year(CDate(sTesto)), "0000")

                    Else
                        'Formato ITALIANO : applica solo una formattazione ai campi
                        sTesto = Day(CDate(sTesto)) & "/" & Month(CDate(sTesto)) & "/" & Year(CDate(sTesto))
                    End If

                    'Verifica , dopo aver ricostruito la data , che sia una data valida
                    If Not IsDate(sTesto) Then    'NOTA: il test con IsDate() va comunque bene anche se gli vengono passati GG e MM invertiti purchè i valori siano validi
                        CDateToDB = "Null"
                    Else
                        CDateToDB = "'" & sTesto & "'"
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.CDateToDB.errore: ", ex)
            End Try
        End Function
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
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.CStrToDB.errore: ", ex)
            End Try
        End Function

        Public Function CIdToDB(ByVal vInput As Object) As String

            CIdToDB = "Null"
            Try
                If Not IsDBNull(vInput) And Not IsNothing(vInput) Then
                    If IsNumeric(vInput) Then
                        If CDbl(vInput) > 0 Then
                            CIdToDB = CStr(CDbl(vInput))
                        End If
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.CldToDB.errore: ", ex)
            End Try
        End Function
        Public Function CToBit(ByRef vInput As Object) As Short

            CToBit = 0
            Try
                If Not IsDBNull(vInput) And Not IsNothing(vInput) Then
                    If CBool(vInput) Then
                        CToBit = 1
                    Else
                        CToBit = 0
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.CToBit.errore: ", ex)
            End Try
        End Function
        Public Function CIdFromDB(ByVal vInput As Object) As String

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
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.CldFromDB.errore: ", ex)
            End Try
        End Function
        Public Shared Function AddBackSlashToPath(ByRef sPath As Object, Optional ByRef blnRemoveInitialSlash As Boolean = True) As String
            'converte la variabile passata in una stringa
            AddBackSlashToPath = ""
            Try
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
                If blnRemoveInitialSlash Then

                    If Left(sPath, 1) = "\" Or Left(sPath, 1) = "/" Then

                        sPath = Mid(sPath, 2)
                    End If
                End If
                AddBackSlashToPath = sPath
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.AddBackSlashToPath.errore: ", ex)
            End Try

        End Function

        '======================================================================================
        'FUNZIONE CHE RESTITUISCE UN ID NUMERICO DA UNA TABELLA CONTATORI
        '======================================================================================
        'Public Function GetNewId(ByRef strNomeTabella As String, ByVal objDBAccess As RIBESFrameWork.DBManager) As Long
        'Public Function GetNewId(ByRef strNomeTabella As String, ByVal oSession As RIBESFrameWork.Session, ByVal Id_SottoAttivita As String) As Long

        '    dim sSQL as string
        '    Dim lngMaxId As Long


        '    Dim objDBAccess As New RIBESFrameWork.DBManager
        '    objDBAccess = oSession.GetPrivateDBManager(Id_SottoAttivita)

        '    objDBAccess.BeginTransIsolationLevel()
        '    Try
        '        sSQL="SELECT MAXID FROM CONTATORI  WHERE NOME_TABELLA ='" & strNomeTabella & "'"
        '        Dim dr As SqlDataReader = objDBAccess.CmdCreateWithTransaction(sSQL).ExecuteReader
        '        If dr.Read Then
        '            lngMaxId = dr.Item("MAXID")
        '            lngMaxId = lngMaxId + Utility.Costanti.VALUE_INCREMENT
        '        End If
        '        dr.Close()
        '        sSQL="UPDATE CONTATORI SET MAXID=" & lngMaxId & " WHERE NOME_TABELLA ='" & strNomeTabella & "'"
        '        objDBAccess.CmdCreateWithTransaction(sSQL)
        '        objDBAccess.CmdExec()
        '        objDBAccess.CommitTrans()
        '    Catch ex As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvUtility.GetNewId.errore: ", ex)
        '        objDBAccess.RollbackTrans()
        '        Throw New Exception("Errore di accesso tabella CONTATORI")
        '    End Try

        '    GetNewId = lngMaxId

        'End Function
        '======================================================================================
        'FINE FUNZIONE CHE RESTITUISCE UN ID NUMERICO DA UNA TABELLA CONTATORI
        '======================================================================================
    End Class

End Namespace
