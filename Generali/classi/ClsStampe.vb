Imports log4net
''' <summary>
''' Classe per la gestione delle stampe in formato CSV/XLS
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsStampaXLS
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsStampaXLS))
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DtAddRow"></param>
    ''' <param name="sValueRow"></param>
    ''' <returns></returns>
    Public Function AddRowStampa(ByRef DtAddRow As DataTable, ByVal sValueRow As String) As Integer
        Dim sTextRow() As String
        Dim DrAddRow As DataRow
        Dim x As Integer = 0

        Try
            'aggiungo una nuova riga nel datarow
            DrAddRow = DtAddRow.NewRow
            'controllo se la riga e\' scritta
            If sValueRow <> "" Then
                sTextRow = sValueRow.Split(CChar("|"))
                For x = 0 To UBound(sTextRow)
                    'popolo la riga nel datarow
                    DrAddRow(x) = sTextRow(x)
                Next
            End If
            'aggiorno la riga al datatable
            DtAddRow.Rows.Add(DrAddRow)

            Return 1
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsStampaXLS.AddRowStampa.errore: " + sValueRow, Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dsDati"></param>
    ''' <param name="sTitoloReport"></param>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Public Function PrintAliquote(ByVal dsDati As DataSet, ByVal sTitoloReport As String, ByRef nCampi As Integer) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim nTotPag As Integer = 0
        Dim impTotPagatoComune As Double = 0
        Dim impTotPagatoStat As Double = 0
        Dim impTotPagato As Double = 0
        Dim impTotEmessoComune As Double = 0
        Dim impTotEmessoStat As Double = 0
        Dim impTotEmesso As Double = 0

        Try
            nCampi = dsDati.Tables(0).Columns.Count
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 1 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, CChar("0")))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione del report
            sDatiStampa = sTitoloReport
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, CChar("|"))
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, CChar("|"))
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = ""
            For Each myCol As DataColumn In dsDati.Tables(0).Columns
                If sDatiStampa <> "" Then
                    sDatiStampa += "|"
                End If
                sDatiStampa += myCol.ColumnName
            Next
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For Each ItemRow As DataRowView In dsDati.Tables(0).DefaultView
                sDatiStampa = ""
                For x = 0 To nCampi - 1
                    If sDatiStampa <> "" Then
                        sDatiStampa += "|"
                    End If
                    If Not IsDBNull(ItemRow(x)) Then
                        sDatiStampa += CStr(ItemRow(x))
                    End If
                Next
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next

            Return DtStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsStampaXLS.PrintAliquote.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DtDatiStampa"></param>
    ''' <param name="dvDati"></param>
    ''' <returns></returns>
    Public Function PrintCruscotto(ByVal DtDatiStampa As DataTable, ByVal dvDati As DataView) As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim nCol As Integer = 6
        Dim x, i As Integer

        Try
            dr = DtDatiStampa.NewRow
            dr(0) = "Cruscotto"
            dr(2) = "Data Stampa:" & DateTime.Now.Date            'DateTime.Today.Day & "/" & DateTime.Today.Month & "/" & DateTime.Today.Year
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            x = 0
            dr(x) = "Anno"
            x += 1
            dr(x) = "Descrizione Ente"
            x += 1
            dr(x) = "Tributo"
            x += 1
            dr(x) = "N.Utenti"
            x += 1
            dr(x) = "N.Documenti"
            x += 1
            dr(x) = "Emesso"
            x += 1
            dr(x) = "Incassato"
            DtDatiStampa.Rows.Add(dr)
            For i = 0 To dvDati.Count - 1
                dr = DtDatiStampa.NewRow
                x = 0
                If Not IsDBNull(dvDati.Item(i)("Anno")) Then
                    dr(x) = CStr(dvDati.Item(i)("Anno"))
                Else
                    dr(x) = ""
                End If
                x += 1
                dr(x) = Utility.StringOperation.FormatString(dvDati.Item(i)("Descrizione_Ente"))
                x += 1
                If Not IsDBNull(dvDati.Item(i)("DescrTributo")) Then
                    dr(x) = "'" & CStr(dvDati.Item(i)("DescrTributo"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("NUtenti")) Then
                    dr(x) = CStr(dvDati.Item(i)("NUtenti"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("NDoc")) Then
                    dr(x) = CStr(dvDati.Item(i)("NDoc"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("ImpEmesso")) Then
                    dr(x) = CStr(dvDati.Item(i)("ImpEmesso"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("ImpIncassato")) Then
                    dr(x) = CStr(dvDati.Item(i)("ImpIncassato"))
                Else
                    dr(x) = ""
                End If
                DtDatiStampa.Rows.Add(dr)
            Next i
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)

            Return DtDatiStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsStampaXLS.PrintCruscotto.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dsDati"></param>
    ''' <param name="sTitoloReport"></param>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Public Function PrintFattVSIncas(ByVal dsDati As DataSet, ByVal sTitoloReport As String, ByVal nCampi As Integer) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim nTotPag As Integer = 0
        Dim impTotPagatoComune As Double = 0
        Dim impTotPagatoStat As Double = 0
        Dim impTotPagato As Double = 0
        Dim impTotEmessoComune As Double = 0
        Dim impTotEmessoStat As Double = 0
        Dim impTotEmesso As Double = 0

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 1 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, CChar("0")))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione del report
            sDatiStampa = sTitoloReport
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, CChar("|"))
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, CChar("|"))
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'azzero la variabile per valorizzarla con quelli reali
            nCampi = 0
            If Not IsNothing(dsDati.Tables("EMESSO")) Then
                nCampi = dsDati.Tables("EMESSO").Columns.Count
                'inserisco una riga vuota
                sDatiStampa = "Ruolo"
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, CChar("|"))
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco le intestazioni di colonna
                sDatiStampa = ""
                Dim ListColICI As New ArrayList
                Dim ciccio As New clsInterrogazioni.ColICI(ListColICI)

                For Each myCol As DataColumn In dsDati.Tables("EMESSO").Columns
                    If sDatiStampa <> "" Then
                        sDatiStampa += "|"
                    End If
                    Dim ColIntest As String = myCol.ColumnName
                    For Each myItem As clsInterrogazioni.ColICI In ListColICI
                        Select Case myCol.ColumnName
                            Case myItem.Name
                                ColIntest = myItem.Description
                                Exit For
                        End Select
                    Next
                    sDatiStampa += ColIntest
                Next
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'ciclo sui dati da stampare
                For Each ItemRow As DataRowView In dsDati.Tables("EMESSO").DefaultView
                    sDatiStampa = ""
                    For x = 0 To nCampi - 1
                        If sDatiStampa <> "" Then
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(ItemRow(x)) Then
                            sDatiStampa += CStr(ItemRow(x))
                        End If
                    Next
                    If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            If Not IsNothing(dsDati.Tables("NETTO")) Then
                nCampi = dsDati.Tables("NETTO").Columns.Count
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, CChar("|"))
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = "Netto Sgravi"
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, CChar("|"))
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco le intestazioni di colonna
                sDatiStampa = ""
                For Each myCol As DataColumn In dsDati.Tables("NETTO").Columns
                    If sDatiStampa <> "" Then
                        sDatiStampa += "|"
                    End If
                    sDatiStampa += myCol.ColumnName
                Next
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'ciclo sui dati da stampare
                For Each ItemRow As DataRowView In dsDati.Tables("NETTO").DefaultView
                    sDatiStampa = ""
                    For x = 0 To nCampi - 1
                        If sDatiStampa <> "" Then
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(ItemRow(x)) Then
                            sDatiStampa += CStr(ItemRow(x))
                        End If
                    Next
                    If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            If Not IsNothing(dsDati.Tables("PAGATO")) Then
                nCampi = dsDati.Tables("PAGATO").Columns.Count
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, CChar("|"))
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = "Incassato"
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, CChar("|"))
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco le intestazioni di colonna
                sDatiStampa = ""
                Dim ListColICI As New ArrayList
                Dim ciccio As New clsInterrogazioni.ColICI(ListColICI)

                For Each myCol As DataColumn In dsDati.Tables("PAGATO").Columns
                    If sDatiStampa <> "" Then
                        sDatiStampa += "|"
                    End If
                    Dim ColIntest As String = myCol.ColumnName
                    For Each myItem As clsInterrogazioni.ColICI In ListColICI
                        Select Case myCol.ColumnName
                            Case myItem.Name
                                ColIntest = myItem.Description
                                Exit For
                        End Select
                    Next
                    sDatiStampa += ColIntest
                Next
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'ciclo sui dati da stampare
                For Each ItemRow As DataRowView In dsDati.Tables("PAGATO").DefaultView
                    sDatiStampa = ""
                    For x = 0 To nCampi - 1
                        If sDatiStampa <> "" Then
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(ItemRow(x)) Then
                            sDatiStampa += CStr(ItemRow(x))
                        End If
                    Next
                    If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            If nCampi <= 0 Then
                DtStampa = Nothing
            End If
            Return DtStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsStampaXLS.PrintFattVSIncas.errore: ", Err)
            Return Nothing
        End Try
    End Function
End Class
