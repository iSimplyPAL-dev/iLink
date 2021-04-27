''' <summary>
''' Classe per la gestione delle variabili costanti
''' </summary>
Public Class Costanti
    Public Const MATRICOLA_NON_RILEVATA As String = "NR"
    Public Const MASCHIO As String = "M"
    Public Const FEMMINA As String = "F"
    Public Const PERSONAGIURIDICA As String = "G"
    '*** 20141117 - voce di costo specifica per utente ***
    Public Const IDVOCEAGGIUNTIVAUTENTE As Integer = 1
    '*** ***
    Public Enum enmContesto
        DELETTURE = 1   'DATA ENTRY LETTURE
        DECONTATORI = 2 'DATA ENTRY CONTATORI
    End Enum
End Class
Public Class Enumeratore
    Public Enum UpdateRecordStatus
        Updated = 0
        Insert = 1
    End Enum
End Class
