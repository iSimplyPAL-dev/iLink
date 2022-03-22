Imports log4net
''' <summary>
'''  Classe per la gestione delle funzioni di formattazione
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class Formatta
    Private strDum As String

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


    ''' <summary>
    ''' Classe per la gestione delle funzioni di formattazione per le griglie
    ''' </summary>
    Public Class FunctionGrd
        Private ModDate As New ModificaDate
        Private Shared Log As ILog = LogManager.GetLogger(GetType(FunctionGrd))
        Public Function annoBarra(ByVal objtemp As Object) As String
            Dim clsGeneralFunction As New MyUtility
            Dim strTemp As String = ""
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.annoBarra.IN")
            Try
                If Not IsDBNull(objtemp) Then
                    If IsDate(objtemp) Then
                        If CDate(objtemp).Date = Date.MinValue.Date Or CDate(objtemp).Date = Date.MaxValue.Date Then
                            strTemp = ""
                        Else
                            Dim MiaData As String = CType(objtemp, DateTime).ToString("yyyy/MM/dd")
                            strTemp = clsGeneralFunction.GiraDataCompletaFromDB(MiaData)
                        End If
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.annoBarra.errore: ", ex)
                strTemp = ""
            End Try
            Return strTemp
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.annoBarra.OUT")
        End Function

        Public Function FormattaDataGrd(ByVal tDataGrd As Object) As String
            Try
                If tDataGrd Is Nothing Then
                    Return ""
                Else
                    If CDate(tDataGrd) = Date.MinValue Then
                        Return ""
                    Else
                        Return tDataGrd.ToShortDateString
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.FormattaDataGrd.errore: ", ex)
                Return ""
            End Try
        End Function

        Public Function GiraData(ByVal prdStatus As Object) As String
            Dim objUtility As New MyUtility

            GiraData = ModDate.GiraDataFromDB(objUtility.CToStr(prdStatus))
            Return GiraData
        End Function

        Public Function ParseDate(ByVal objtemp As Object) As String
            Dim strTemp As String = ""
            Dim objdate As Date
            Try
                If Not IsDBNull(objtemp) Then
                    If objtemp.ToString() <> "" Then
                        objdate = CType(objtemp, Date)
                        If objdate < Date.MaxValue.ToString("dd/MM/yyyy") Then
                            strTemp = objdate.Parse(objtemp).ToString("dd/MM/yyyy")
                        End If
                        'strTemp = objtemp
                    End If
                End If
                Return strTemp
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.ParseDate.errore: ", ex)
                Return ""
            End Try
        End Function

        Public Function FormattaNumeriGrd(ByVal objInput As Object) As String
            Try
                If objInput Is Nothing Then
                    Return ""
                Else
                    If IsNumeric(objInput) Then
                        If objInput <= 0 Then
                            Return ""
                        Else
                            Return objInput.ToString
                        End If
                    Else
                        Return ""
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.FormattaNumeriGrd.errore: ", ex)
                Return ""
            End Try
        End Function

        Public Function FormatStringToEmpty(ByVal objInput As Object) As String
            Dim strOutput As String
            Try
                If (objInput Is Nothing) Then
                    strOutput = ""
                ElseIf IsDBNull(objInput) Then
                    strOutput = ""
                Else
                    If CStr(objInput) = "" Or CStr(objInput) = "0" Or CStr(objInput) = "-1" Then
                        strOutput = ""
                    Else
                        strOutput = objInput.ToString()
                    End If
                End If
                Return strOutput
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.FormatStringToEmpty.errore: ", ex)
                Return ""
            End Try
        End Function

        Public Function FormatStringToZero(ByVal objInput As Object) As String
            Dim strOutput As String
            Try
                If (objInput Is Nothing) Then
                    strOutput = ""
                ElseIf IsDBNull(objInput) Then
                    strOutput = ""
                Else
                    If CStr(objInput) = "" Or CStr(objInput) = "0" Or CStr(objInput) = "-1" Then
                        strOutput = ""
                    Else
                        strOutput = objInput.ToString()
                    End If
                End If
                Return strOutput
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.FormatStringToZero.errore: ", ex)
                Return ""
            End Try
        End Function

        Public Function checkFlag(ByVal objtemp As Object) As String
            Dim myRet As String = "False"
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.checkFlag.IN")
            Try
                If Not IsDBNull(objtemp) Then
                    objtemp = objtemp.ToString.Trim
                    If objtemp = "1" Or UCase(objtemp) = "TRUE" Or (UCase(objtemp) <> "FALSE" And objtemp <> "0" And objtemp <> "") Then
                        myRet = "True"
                    Else
                        myRet = "False"
                    End If
                Else
                    myRet = "False"
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.checkFlag.errore: ", ex)
                myRet = "False"
            End Try
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.checkFlag.OUT")
            Return myRet
        End Function

        Public Function IntForGridView(ByVal iInput As Object) As String
            Dim ret As String = String.Empty
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.IntForGridView.IN")
            Try
                If iInput.ToString() = "-1" Or iInput.ToString() = "-1,00" Then
                    ret = String.Empty
                Else
                    ret = Convert.ToString(iInput)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.IntForGridView.errore: ", ex)
                ret = ""
            End Try
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.IntForGridView.OUT")
            Return ret
        End Function

        Public Function FormattaNumero(ByVal NumeroDaFormattareParam As String, ByVal numDec As Integer) As String
            Try
                If IsDBNull(NumeroDaFormattareParam) Or NumeroDaFormattareParam = "" Or NumeroDaFormattareParam = "-1" Or NumeroDaFormattareParam = "-1,00" Then
                    NumeroDaFormattareParam = ""
                Else
                    NumeroDaFormattareParam = FormatNumber(NumeroDaFormattareParam, numDec)
                End If
                Return NumeroDaFormattareParam
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.FormattaNumero.errore: ", ex)
                Return ""
            End Try
        End Function

        'Public Function Stato(ByVal prdIDPROVVEDIMENTO As Object, ByVal prdTIPO_PROVVEDIMENTO As Object) As String
        '    Try
        '        Dim objUtility As New MyUtility
        '        Dim strIDProvvedimento As String
        '        Dim strTIPO_PROVVEDIMENTO As String
        '        Dim objProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
        '        Dim objDS As New DataSet

        '        If Not HttpContext.Current.Session("PROVVEDIMENTI_CONTRIBUENTE") Is Nothing Then
        '            objDS = HttpContext.Current.Session("PROVVEDIMENTI_CONTRIBUENTE")
        '            strIDProvvedimento = objUtility.CToStr(prdIDPROVVEDIMENTO)
        '            strTIPO_PROVVEDIMENTO = objUtility.CToStr(prdTIPO_PROVVEDIMENTO)

        '            Return objProvvedimenti.getStato(strIDProvvedimento, strTIPO_PROVVEDIMENTO, objDS)
        '        Else
        '            Return ""
        '        End If
        '    Catch ex As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.Stato.errore: ", ex)
        '        Return ""
        '    End Try
        'End Function

        Public Function checkMesiRiduzione(ByVal objtemp As Object) As String
            Try
                If Not IsDBNull(objtemp) Then
                    If Int(objtemp) > 0 Then
                        Return "True"
                    Else
                        Return "False"
                    End If
                Else
                    Return "False"
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.checkMesiRiduzione.errore: ", ex)
                Return "False"
            End Try
        End Function

        Public Function ConvertABS(ByVal prdStatus As Object) As String
            Dim objUtility As New MyUtility
            Dim dblImpTotAvviso As Double

            dblImpTotAvviso = objUtility.cToDbl(prdStatus)
            Try
                'If dblImpTotAvviso > 0 Then
                ConvertABS = objUtility.CToStr(FormatNumber(dblImpTotAvviso, 2))
                ' Else
                'ConvertABS = objUtility.CToStr(FormatNumber(Math.Abs(dblImpTotAvviso), 2))
                '  ConvertABS = objUtility.CToStr(FormatNumber(dblImpTotAvviso, 2))
                ' End If

                Return ConvertABS
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.ConvertABS.errore: ", ex)
                Return ""
            End Try
        End Function
        Public Function giorni_mese(ByVal mese As Integer) As Integer
            Try
                Select Case mese
                    Case 1, 3, 5, 7, 8, 10, 12
                        Return 31
                    Case 2
                        Return 29
                    Case 4, 6, 9, 11
                        Return 30
                End Select
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.giorni_mese.errore: ", ex)
            End Try
        End Function
        Public Function checkPertinenza(ByVal objtempPERT As Object, ByVal objtempIMM As Object) As String
            Dim myRet As String = "False"
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.checkPertinenza.IN")
            Try
                If Not IsDBNull(objtempPERT) Or Not IsDBNull(objtempIMM) Then
                    If objtempPERT <> "-1" And objtempPERT <> "0" And objtempPERT <> objtempIMM Then
                        myret = "True"
                    Else
                        myret = "False"
                    End If
                Else
                    myret = "False"
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.checkPertinenza.errore: ", ex)
                myret = "False"
            End Try
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.FunctionGrd.checkPertinenza.OUT")
            Return myRet
        End Function

        Public Function FormattaCFPIVA(CodFiscale As Object, PartitaIva As Object) As String
            Dim myRet As String = String.Empty

            If Not IsDBNull(PartitaIva) Then
                If PartitaIva.ToString() <> "" Then
                    myRet = PartitaIva.ToString()
                Else
                    If Not IsDBNull(CodFiscale) Then
                        myRet = CodFiscale.ToString()
                    Else
                        myRet = ""
                    End If
                End If
            ElseIf Not IsDBNull(CodFiscale) Then
                myRet = CodFiscale.ToString()
            End If
            Return myRet
        End Function
    End Class
    '*** 20130801 - accertamento OSAP ***
    ''' <summary>
    '''  Classe per la gestione delle funzioni di formattazione per le griglie
    ''' </summary>
    Public Class SharedGrd
        Private Shared Log As ILog = LogManager.GetLogger(GetType(SharedGrd))
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="MyVal"></param>
        ''' <param name="MyDescr"></param>
        ''' <returns></returns>
        Public Function FormattaDurCons(ByVal MyVal As Object, ByVal MyDescr As Object) As String
            Dim ret As String = String.Empty
            Try
                If Not IsDBNull(MyVal) Then
                    ret += CStr(MyVal)
                End If
                If Not IsDBNull(MyDescr) Then
                    ret += " " + MyDescr
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SharedGrd.FormattaDurCons.errore: ", ex)
            End Try
            Return ret
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="TypeFormat"></param>
        ''' <param name="MyObj"></param>
        ''' <returns></returns>
        Public Function FormattaCalcolo(ByVal TypeFormat As String, ByVal MyObj As Object) As String
            Dim ret As String = String.Empty
            Dim MyCalcolo As IRemInterfaceOSAP.CalcoloResult
            Try
                If Not IsDBNull(MyObj) Then
                    MyCalcolo = MyObj
                    If TypeFormat = "T" Then
                        ret = CStr(MyCalcolo.TariffaApplicata)
                    Else
                        ret = CStr(MyCalcolo.ImportoCalcolato)
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SharedGrd.FormattaCalcolo.errore: ", ex)
            End Try
            Return ret
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objtemp"></param>
        ''' <returns></returns>
        Public Function checkFlag(ByVal objtemp As Object) As String
            Try
                If Not IsDBNull(objtemp) Then
                    If objtemp = "1" Or UCase(objtemp) = "TRUE" Then
                        Return "True"
                    Else
                        Return "False"
                    End If
                Else
                    Return "False"
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SharedGrd.checkFlag.errore: ", ex)
                Return "False"
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dDifferenzaImposta"></param>
        ''' <param name="dInteressi"></param>
        ''' <param name="dSanzioni"></param>
        ''' <returns></returns>
        Public Function CalcolaTotaliGrd(ByVal dDifferenzaImposta As Double, ByVal dInteressi As Double, ByVal dSanzioni As Double) As String
            Dim dTotale As Double

            Try
                dTotale = dDifferenzaImposta + dInteressi + dSanzioni
                Return FormatNumber(dTotale, 2)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SharedGrd.CalcolaTotaliGrd.errore: ", ex)
                Return ""
            End Try
        End Function
    End Class
    '*** ***
End Class

