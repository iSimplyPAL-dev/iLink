Imports System.IO
Imports System
Imports log4net

Public Class GestioneFile
    Private Shared Log2 As ILog = LogManager.GetLogger(GetType(GestioneFile))
    'Metodo che scrive su di un file ricevendo come parametri la stringa da scrivere 
    'e il percorso col nome del file da scrivere
    'In caso di errore restituisce al metodo l'errore altrimenti restituisce la stringa
    'OK
    Friend Function ScriviFile(ByVal strToWrite As String, ByVal FileName As String) As String
        Dim strwriterobj As StreamWriter
        ' Create a text File and assign this textfile obj to streamwriter
        ' Object created above
        Try
            If File.Exists(FileName) = True Then
                strwriterobj = File.AppendText(FileName)
            Else
                strwriterobj = File.CreateText(FileName)
            End If
            strwriterobj.WriteLine(strToWrite)
            ' we are done with the file so close the stream object 
            strwriterobj.Close()
            ScriviFile = "OK"
        Catch Err As Exception
            'Se si è verificato un errore cancello il file
            Log2.Debug(ConstSession.IdEnte + " - OPENgovH2O.GestioneFile.ScriviFile.errore: ", Err)
            If File.Exists(FileName) = True Then
                File.Delete(FileName)
            End If
            Dim Log As New GestioneFile()
            Log.ScriviFileLog(Err, "Scrittura su file", "Errore.txt", "c:\", "")
            ScriviFile = "<p>Si è verificato il seguente errore:</p>" &
                         Err.Message & "<br>"
        End Try
    End Function

    Friend Sub ScriviFileLog(ByVal Errore As Exception, ByVal Operazione As String, ByVal FileName As String, ByVal Path As String, ByVal Header As String)
        Dim strwriterobj As StreamWriter
        Dim strToWrite As String
        ' Create a text File and assign this textfile obj to streamwriter
        ' Object created above
        Try
            If File.Exists(Path & FileName) = True Then
                strwriterobj = File.AppendText(Path & FileName)
            Else
                'Creo il file di log e "l'intestazione" del file
                strwriterobj = File.CreateText(Path & FileName)
                If Header = "" Then
                    strToWrite = "DATA;DESCRIZIONE ERRORE;OPERAZIONE"
                Else
                    strToWrite = Header
                End If
                strwriterobj.WriteLine(strToWrite)
            End If
            'Scrivo l'errore
            strToWrite = DateTime.Now() & ";" & CStr(Errore.Message) & ";" & Operazione
            strwriterobj.WriteLine(strToWrite)
            ' we are done with the file so close the stream object 
            strwriterobj.Close()
        Catch Err As Exception
            Log2.Debug(ConstSession.IdEnte + " - OPENgovH2O.GestioneFile.ScriviFileLog.errore: ", Err)
            Dim Log As New GestioneFile()
            'Richiamo la procedura di log e scrivo l'errore - RISCHIO DEADLOCK?
            'Log.ScriviFileLog(Err, "", "", "", "")
        End Try
    End Sub
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    Friend Sub trace(ByVal Errore As Exception, ByVal Operazione As String, ByVal FileName As String, ByVal Path As String, ByVal Header As String)

        On Error Resume Next

        Dim strwriterobj As StreamWriter
        Dim strToWrite As String
        ' Create a text File and assign this textfile obj to streamwriter
        ' Object created above

        If File.Exists(Path & FileName) = True Then
            strwriterobj = File.AppendText(Path & FileName)
        Else
            'Creo il file di log e "l'intestazione" del file
            strwriterobj = File.CreateText(Path & FileName)
            If Header = "" Then
                strToWrite = "DATA;DESCRIZIONE ERRORE;OPERAZIONE"
            Else
                strToWrite = Header
            End If
            strwriterobj.WriteLine(strToWrite)
        End If
        'Scrivo l'errore
        strToWrite = DateTime.Now() & ";" & CStr(Errore.Message) & ";" & Operazione
        strwriterobj.WriteLine(strToWrite)
        ' we are done with the file so close the stream object 
        strwriterobj.Close()

    End Sub
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    Friend Sub DeleteFiles(ByVal FileName As String)
        Try
            If File.Exists(FileName) = True Then
                File.Delete(FileName)
            End If
        Catch Err As Exception
            Log2.Debug(ConstSession.IdEnte + " - OPENgovH2O.GestioneFile.DeleteFiles.errore: ", Err)
        End Try
    End Sub
    Friend Function FileExist(ByVal FileName As String) As Boolean
        FileExist = False
        Try
            If File.Exists(FileName) Then
                FileExist = True
            End If
        Catch Err As Exception
            Log2.Debug(ConstSession.IdEnte + " - OPENgovH2O.GestioneFile.FileExist.errore: ", Err)
        End Try
    End Function
End Class
