Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Classe per la gestione delle funzioni di formattazione
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class Formatta
    Private strDum As String


    Private Shared Log As ILog = LogManager.GetLogger(GetType(Formatta))
    'Property Accento(ByVal strString As String)
    '    Get
    '        strDum = Replace(strString, "'", Chr(180))
    '        Return strDum
    '    End Get
    '    Set(ByVal Value)
    '        Value = strDum
    '    End Set
    'End Property

    'Property Apice(ByVal strString As String)
    '    Get
    '        strDum = Replace(strString, Chr(180), "'")
    '        Return strDum
    '    End Get
    '    Set(ByVal Value)
    '        Value = strDum
    '    End Set
    'End Property

    Public Function turnData(ByVal data As String) As String
        Dim giorno, mese, anno As String
        giorno = Mid(data, 7, 2)
        mese = Mid(data, 5, 2)
        anno = Mid(data, 1, 4)
        data = giorno & "/" & mese & "/" & anno
        Return data
    End Function

    Public Function annoBarra(ByVal annoTermico As String) As String
        Dim anno, anno2 As String
        anno = Mid(annoTermico, 1, 4)
        anno2 = Mid(annoTermico, 5, 4)
        annoBarra = anno & "/" & anno2
        Return annoBarra
    End Function

    Public Function dataBarra(ByVal data As String) As String
        Dim AAAA, MM, GG As String
        AAAA = Mid(data, 1, 4)
        MM = Mid(data, 5, 2)
        GG = Mid(data, 7, 2)
        dataBarra = GG & "/" & MM & "/" & AAAA
        Return dataBarra
    End Function

    ''' <summary>
    ''' Classe per la gestione delle funzioni di formattazione per le griglie
    ''' </summary>
    Public Class FunctionGrd
        Public Const TIPOOCCUPAZIONE_OCCUPATORESIDENTI As String = "OC"
        Public Const TIPOOCCUPAZIONE_DISPOSIZIONERESIDENTI As String = "DR"
        Private Shared Log As ILog = LogManager.GetLogger(GetType(FunctionGrd))

        Public Function FormatNumberInGrd(ByVal nMyNumber As Integer) As String
            Try
                If nMyNumber > 0 Then
                    Return nMyNumber.ToString
                Else
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormatNumbrInGrd.errore: ", ex)
                Return ""
            End Try
        End Function
        Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
            Try
                If tDataGrd = Date.MinValue Or tDataGrd = Date.MaxValue Or tDataGrd = DateTime.MaxValue.ToShortDateString Or tDataGrd.ToShortDateString = DateTime.MaxValue.ToShortDateString Then
                    Return ""
                Else
                    Return tDataGrd.ToShortDateString
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormattaDataGrd.errore: ", ex)
                Return ""
            End Try
        End Function
        Public Function FormattaProvPagGrd(ByVal oMyObj As Object) As String
            Dim RetVal As String
            Try
                If Not oMyObj Is Nothing Then
                    RetVal = oMyObj.ToString().Replace("DE", "DATA ENTRY ").ToUpper()
                Else
                    RetVal = ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormattaProvPagGrd.errore: ", ex)
                RetVal = ""
            End Try
            Return RetVal
        End Function
        Public Function FormattaVia(ByVal Via As Object, ByVal Civico As Object, ByVal Interno As Object, ByVal Esponente As Object, ByVal Scala As Object, ByVal Foglio As Object, ByVal Numero As Object, ByVal Subalterno As Object) As String
            Dim ret As String = String.Empty
            Try
                If Not IsDBNull(Via) Then
                    ret = Via
                Else
                    ret = ""
                End If
                If Not IsDBNull(Civico) Then
                    If CStr(Civico) <> "0" Then
                        ret += " " + CStr(Civico)
                    Else
                        ret += ""
                    End If
                End If
                If Not IsDBNull(Esponente) Then
                    If CStr(Esponente) <> "" Then
                        ret += " " + CStr(Esponente)
                    Else
                        ret += ""
                    End If
                Else
                    ret += ""
                End If
                If Not IsDBNull(Scala) Then
                    If CStr(Scala) <> "" Then
                        ret += " " + CStr(Scala)
                    Else
                        ret += ""
                    End If
                Else
                    ret += ""
                End If
                If Not IsDBNull(Interno) Then
                    If CStr(Interno) <> "" Then
                        ret += " " + CStr(Interno)
                    Else
                        ret += ""
                    End If
                Else
                    ret += ""
                End If
                If Not IsDBNull(Foglio) Then
                    If CStr(Foglio) <> "" Then
                        ret += " (" + Foglio
                    Else
                        ret += ""
                    End If
                Else
                    ret += ""
                End If
                If Not IsDBNull(Numero) Then
                    If CStr(Numero) <> "" Then
                        ret += "/" + Numero
                    Else
                        ret += ""
                    End If
                Else
                    ret += ""
                End If
                If Not IsDBNull(Subalterno) Then
                    If CStr(Subalterno) <> "" Then
                        ret += "/" + Subalterno
                    Else
                        ret += ""
                    End If
                Else
                    ret += ""
                End If
                If Not IsDBNull(Foglio) Then
                    If CStr(Foglio) <> "" Then
                        ret += ")"
                    Else
                        ret += ""
                    End If
                End If

                Return ret
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormattaVia.errore: ", ex)
                Return ""
            End Try
        End Function

        Public Function FormattaUbicazione(ByVal oMyObj() As Object) As String
            Try
                Dim oMyImmobili() As RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata

                oMyImmobili = oMyObj
                If oMyImmobili.GetUpperBound(0) = 0 Then
                    Return FormattaVia(oMyImmobili(0).sVia, oMyImmobili(0).sCivico, oMyImmobili(0).sInterno, oMyImmobili(0).sEsponente, oMyImmobili(0).sScala, oMyImmobili(0).sFoglio, oMyImmobili(0).sNumero, oMyImmobili(0).sSubalterno)
                Else
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormattaUbicazione.errore: ", ex)
                Return ""
            End Try
        End Function

        Public Function FormattaCatCatastale(ByVal Foglio As Object, ByVal Numero As Object, ByVal Subalterno As Object) As String
            Dim FncImmo As New GestDettaglioTestata

            Try
                Return FncImmo.GetCatCatastale(ConstSession.StringConnection, ConstSession.IdEnte, Foglio, Numero, Subalterno)
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormattaCatCatastale.errore: ", Err)
                Return ""
            End Try
        End Function

        Public Function FormattaCategoria(ByVal Codice As Object, ByVal Descrizione As Object) As String
            Dim ret As String = String.Empty
            Try
                If Not IsDBNull(Descrizione) Then
                    ret = CStr(Descrizione)
                Else
                    If Not IsDBNull(Codice) Then
                        ret = Codice
                    Else
                        ret = ""
                    End If
                End If

                Return ret
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormattaCategoria.errore: ", ex)
                Return ""
            End Try
        End Function

        Public Function FormattaRidDet(ByVal impRidDet As Double) As String
            Try
                If impRidDet <> 0 Then
                    Return "..\..\..\images\Bottoni\visto.png"
                Else
                    Return "..\..\..\images\Bottoni\trasparente.png"
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormattaRidDet.errore: ", ex)
                Return ""
            End Try
        End Function
        Public Function FormattaToolTipRidDet(ByVal myList() As ObjRidEseApplicati) As String
            Dim sToolTip As String = ""
            Try
                If Not IsNothing(myList) Then
                    For Each myRD As ObjRidEseApplicati In myList
                        If sToolTip <> "" Then
                            sToolTip += vbCrLf
                        End If
                        sToolTip += myRD.sDescrizione
                    Next
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormattaToolTipRidDet.errore: ", ex)
            End Try
            Return sToolTip
        End Function
        Public Function FormattaInRuolo(ByVal nId As Integer) As String
            Try
                If nId > 0 Then
                    Return "..\..\..\images\Bottoni\Transparent 2.png"
                Else
                    Return "..\..\..\images\Bottoni\trasparente.png"
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormattaInRuolo.errore: ", ex)
                Return ""
            End Try
        End Function

        Public Function FormattaConferimenti(ByVal sTipo As String, ByVal oMyList() As RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjPesatura) As String
            Dim x As Integer
            Dim sMyRet As String = ""

            Try
                Select Case sTipo
                    Case "N"
                        If Not IsNothing(oMyList) Then
                            sMyRet = oMyList.GetUpperBound(0) + 1
                        End If
                    Case "P"
                        If Not IsNothing(oMyList) Then
                            sMyRet = "0"
                            For x = 0 To oMyList.GetUpperBound(0)
                                sMyRet += oMyList(x).nLitri
                            Next
                        End If
                End Select
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormattaConferimenti.errore: ", Err)
                sMyRet = ""
            End Try
            Return sMyRet
        End Function

        Public Function FormattaIsSgravato(ByVal nSgravio As Integer) As String
            Try
                If nSgravio < 0 Then
                    Return "..\..\..\images\Bottoni\Add.png"
                Else
                    Return "..\..\..\images\Bottoni\trasparente.png"
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormattaIsSgravato.errore: ", ex)
                Return ""
            End Try
        End Function

        Public Function FormattaToolTipIsSgravato(ByVal nSgravio As Integer, ByVal impOrg As Object) As String
            Dim sRet As String
            Try
                If nSgravio < 0 Then
                    sRet = "Presenza Sgravio"
                    If Not IsDBNull(impOrg) Then
                        sRet += " Importo Originale " & FormatNumber(impOrg, 2)
                    End If
                    Return sRet
                Else
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.FunctionGrd.FormattaToolTipIsSgravato.errore: ", ex)
                Return ""
            End Try
        End Function
    End Class
End Class
