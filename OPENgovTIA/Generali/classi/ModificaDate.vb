Imports log4net
''' <summary>
''' Classe per le funzioni generali delle date
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ModificaDate
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ModificaDate))
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
                GiraData = Anno & Mese & Giorno
            Else
                GiraData = ""
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.CConfiguraCanoni.GiraData.errore: ", ex)
            Return ""
        End Try
    End Function


    Public Function GiraDataFromDB(ByVal data As String) As String
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.CConfiguraCanoni.GiraDataFromDB.errore: ", ex)
            Return ""
        End Try
    End Function

    Public Function SottraiData(ByVal strData As String, ByVal lngNumeroDaSottrarre As Long) As String

        Dim intGiorno As Object
        Dim intMese As Object
        Dim intAnno As Object
        Dim I As Long

        Try
            'If Trim(strData) <> "" Then
            '    DayMonthYear(strData, intGiorno, intMese, intAnno)
            'End If

            intGiorno = Right(strData, 2)
            intMese = Mid(strData, 5, 2)
            intAnno = Left(strData, 4)

            If lngNumeroDaSottrarre > 0 Then
                For I = 1 To lngNumeroDaSottrarre

                    Select Case CInt(intMese)
                        Case 12, 10, 7, 5

                            If (intGiorno - 1) = 0 Then
                                intGiorno = 30
                                intMese = intMese - 1

                            Else
                                intGiorno = intGiorno - 1
                            End If
                        Case 2
                            If (intGiorno - 1) = 0 Then
                                intGiorno = 31
                                intMese = intMese - 1
                            Else
                                intGiorno = intGiorno - 1
                            End If

                        Case 3
                            If (intGiorno - 1) = 0 Then
                                If isBisestile(intAnno) = False Then
                                    intGiorno = 28
                                Else
                                    intGiorno = 29
                                End If
                                intMese = intMese - 1
                            Else
                                intGiorno = intGiorno - 1
                            End If
                        Case 4, 6, 9, 11
                            If (intGiorno - 1) = 0 Then
                                intGiorno = 31
                                intMese = intMese - 1
                            Else
                                intGiorno = intGiorno - 1
                            End If
                        Case 8, 1
                            If (intGiorno - 1) = 0 Then
                                intGiorno = 31
                                If intMese = 1 Then
                                    intMese = 12
                                    intAnno = intAnno - 1
                                Else
                                    intMese = intMese - 1
                                End If
                            Else
                                intGiorno = intGiorno - 1
                            End If

                    End Select
                Next
            End If

            'SottraiData = Format(intGiorno, "00") & "/" & Format(intMese, "00") & "/" & Format(intAnno, "0000")
            SottraiData = Format(intAnno, "0000") & Format(intMese, "00") & Format(intGiorno, "00")

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.CConfiguraCanoni.SottraiData.errore: ", ex)
            Return ""
        End Try

    End Function

    Private Function DayMonthYear(ByVal vrnData As Object,
    ByVal intGiorno As Object,
    ByVal intMese As Object,
    ByVal intAnno As Object) As Boolean

        Dim varAr As Object

        DayMonthYear = False
        Try
            'If Not vrnData Is System.DBNull.Value And Not System.IsEmpty(vrnData) And Not IsMissing(vrnData) Then
            varAr = Split(vrnData, "/")
            intGiorno = varAr(0)
            intMese = varAr(1)
            intAnno = Format(Year("01/01/" & varAr(2)), "0000")
            'End If

            DayMonthYear = True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.CConfiguraCanoni.DayMonthYear.errore: ", ex)
        End Try
    End Function

    Private Function isBisestile(ByVal Anno As Integer) As Boolean
        isBisestile = ((Anno Mod 400) = 0) Or (((Anno Mod 4) = 0) And (Anno Mod 100) <> 0)
    End Function

End Class
