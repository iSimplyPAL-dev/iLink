Imports System.IO
Imports System
Imports System.Data.SqlClient
Imports System.Data
Imports System.Data.OleDb
Imports log4net

Public Class CreaSilk

    Private Shared Log As ILog = LogManager.GetLogger(GetType(CreaSilk))

    Sub Crea_Slk(ByVal path_name As String)

        'Creo un file .slk
        Dim strwriterobj As StreamWriter
        Try
            If File.Exists(path_name) Then
                File.Delete(path_name)
            End If

            strwriterobj = File.AppendText(path_name)
            strwriterobj.Close()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.CreaSilk.Crea_Slk.errore: ", ex)
        End Try
    End Sub

    Sub Crea_intestazione(ByVal path_name As String, ByVal strConn As String, ByVal sql As String, ByVal intestazione As Integer, ByVal testo As String)
        Try
            Dim objConn As New OleDbConnection(strConn)
            Dim objCommand As New OleDbCommand(sql, objConn)
            Dim strwriterobj As StreamWriter
            objConn.Open()
            'Parte Fissa
            strwriterobj = File.AppendText(path_name)
            strwriterobj.WriteLine("ID;PWXL;N;E")
            strwriterobj.WriteLine("P;PGeneral")
            strwriterobj.WriteLine("P;P0")  'general
            strwriterobj.WriteLine("P;P0.00")   'per le cifre decimali
            strwriterobj.WriteLine("F;P0;DG0G8;M255")

            Dim objDataReader As OleDbDataReader = objCommand.ExecuteReader()
            Dim numero_campi, campo, i, count, numero_car As Integer
            'conto il numero di campi e il numero di record e cerco se c'è un campo money da formattare

            numero_campi = objDataReader.FieldCount

            Do While objDataReader.Read
                count = count + 1
            Loop
            'dimensiono il file slk con il numero di record e il numero di campi + lo spazio per l'intestazione
            strwriterobj.WriteLine("B;Y" & count + intestazione & ";X" & numero_campi & ";D0 0 1 3")
            'formatto dimensionando il campo a seconda della lunghezza del nome 
            For i = 0 To numero_campi - 1
                campo = Replace(objDataReader.GetName(i), "_", " ")
                numero_car = Len(campo) + 5
                strwriterobj.WriteLine("F;W" & i + 1 & " " & i + 1 & " " & numero_car & "")
            Next
            'F;M405;R1
            strwriterobj.WriteLine("C;Y1;X1;K" & """" & testo & """")   'creo intestazione
            For i = 0 To numero_campi - 1   'creo il campo
                campo = Replace(objDataReader.GetName(i), "_", " ")
                If InStr(campo, "Euro") > 0 Then
                    '€ == (0
                    campo = Replace(campo, "Euro", "(0")
                End If
                'deve scrivere la dim di ogno campo
                strwriterobj.WriteLine("C;Y" & 1 + intestazione & ";X" & i + 1 & ";K" & """" & campo & """")
            Next
            objDataReader.Close()
            objConn.Close()
            strwriterobj.Close()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.CreaSilk.Crea_Intestazione.errore: ", ex)
        End Try
    End Sub



    Sub Formattazzione(ByVal path_name As String, ByVal strwriterobj As StreamWriter, ByVal indice As String)
        'E'un campo money. Formatto la cella prendendo le coordinate 
        strwriterobj.WriteLine("F;P2;FF2R;X" & indice & "")
    End Sub


    Function FomatData(ByVal valore As String) As String

        Dim giorno, mese, anno As String
        anno = Mid(valore, 1, 4)
        mese = Mid(valore, 5, 2)
        giorno = Mid(valore, 7, 2)
        FomatData = giorno & "/" & mese & "/" & anno

    End Function


    Private Sub Scrivi_Cella(ByVal path_name As String, ByVal strConn As String, ByVal sql As String, ByVal intestazione As Integer)
        Dim strwriterobj As New StreamWriter(path_name)
        Try
            Dim numero_campi, campo, count_record, valore, indice, dataType As Integer
            'stringa di connessione provvisoria OLEDB
            Dim objConn As New OleDbConnection(strConn)
            objConn.Open()
            Dim objCommand As New OleDbCommand(sql, objConn)
            Dim objDataReader As OleDbDataReader = objCommand.ExecuteReader()
            strwriterobj = File.AppendText(path_name)
            count_record = 2
            numero_campi = objDataReader.FieldCount

            Do While objDataReader.Read

                For indice = 0 To numero_campi - 1
                    'controllo formattazzione'
                    Dim typeDT As DataTable = objDataReader.GetSchemaTable()
                    'Prelevo il tipo del campo
                    dataType = CInt(IIf(IsDBNull(typeDT.Rows(indice).Item(6)), "Null", typeDT.Rows(indice).Item(6)))
                    If dataType = 6 Then          'significa che è un campo money'
                        Call Formattazzione(path_name, strwriterobj, indice + 1)
                    End If
                    If dataType = 131 Then        'significa che è un campo numeric'
                        Call Formattazzione(path_name, strwriterobj, indice + 1)
                    End If

                    valore = objDataReader.Item(indice)
                    campo = Replace(objDataReader.GetName(indice), "_", " ")
                    If InStr(campo, "Data") > 0 Then
                        valore = FomatData(valore)
                        strwriterobj.WriteLine("C;Y" & count_record + intestazione & ";X" & indice + 1 & ";K" & """" & "" & valore & "" & """")
                    Else
                        If dataType = 6 Then
                            If Len(Trim(utility.stringoperation.formatstring(valore))) > 0 Then
                                valore = Format(valore, "#,##0.00")
                            End If
                        End If
                        strwriterobj.WriteLine("C;Y" & count_record + intestazione & ";X" & indice + 1 & ";K" & """" & "" & valore & "" & """")
                    End If
                Next
                count_record = count_record + 1
            Loop
            strwriterobj.WriteLine("E")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CreaSilk.Scrivi_Cella.errore: ", ex)
            Throw ex
        Finally
            strwriterobj.Close()
        End Try
    End Sub
End Class
