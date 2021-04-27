Imports log4net
Imports System.Web.Caching
''' <summary>
''' Classe per la gestione delle variabili costanti
''' </summary>
Public Class Costanti
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(Costanti))

    Public Const INIT_VALUE_NUMBER As Integer = -1
    Public Const INIT_VALUE_NUMBER_STRING As String = "-1"
    Public Const INIT_VALUE_STRING As String = ""
    Public Const INIT_VALUE_BOOL As Boolean = False
    Public Const VALUE_NUMBER_ZERO As Integer = 0
    Public Const VALUE_NUMBER_UNO As Integer = 1
    Public Const VALUE_INCREMENT As Integer = 1

    Public Const MASCHIO As String = "M"
    Public Const FEMMINA As String = "F"
    Public Const PERSONAGIURIDICA As String = "G"

    Public Enum enmContesto
        DELETTURE = 1      'DATA ENTRY LETTURE
        DECONTATORI = 2 'DATA ENTRY CONTATORI
    End Enum
End Class
''' <summary>
''' Classe per la gestione delle variabili da sessione e da config
''' </summary>
Public Class ConstSession
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(ConstSession))
    Public Shared CountScript As Integer = 0
    Shared _IdVariazioneTributaria As Integer = -1

    Public Shared ReadOnly Property IdEnte() As String
        Get
            Try
                If (HttpContext.Current.Session("COD_ENTE") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("COD_ENTE").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTERRITORIO.ConstSession.IDENTE.errore:", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property DescrizioneEnte() As String
        Get
            Try
                If (HttpContext.Current.Session("DESCRIZIONE_ENTE") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("DESCRIZIONE_ENTE").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTERRITORIO.ConstSession.DescrizioneEnte.errore:", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared Property IdVariazioneTributaria As Integer
        Get
            Return _IdVariazioneTributaria
        End Get
        Set(value As Integer)
            _IdVariazioneTributaria = value
        End Set
    End Property
#Region "stringhe connessione db"
    Public Shared ReadOnly Property DBType() As String
        Get
            Return "SQL"
        End Get
    End Property
    Public Shared ReadOnly Property StringConnection() As String
        Get
            Try
                If Not ConfigurationManager.AppSettings("connectionStringOPENgovTERRITORIO") Is Nothing Then
                    Return ConfigurationManager.AppSettings("connectionStringOPENgovTERRITORIO").ToString
                Else
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTERRITORIO.ConstSession.StringConnection.errore:", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property StringConnectionOPENgov() As String
        Get
            Try
                If Not ConfigurationManager.AppSettings("connectionStringSQLOPENgov") Is Nothing Then
                    Return ConfigurationManager.AppSettings("connectionStringSQLOPENgov")
                Else
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTERRITORIO.ConstSession.StringConnectionOPENgov.errore:", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property StringConnectionOPENgovCOMUNISTRADE() As String
        Get
            Try
                If Not ConfigurationManager.AppSettings("ConnessioneDBComuniStrade") Is Nothing Then
                    Return ConfigurationManager.AppSettings("ConnessioneDBComuniStrade")
                Else
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTERRITORIO.ConstSession.StringConnectionOPENgovCOMUNISTRADE.errore:", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property StringConnectionAnagrafica() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("connectionStringSQLOPENAnagrafica") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("connectionStringSQLOPENAnagrafica")
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTERRITORIO.ConstSession.StringConnectionAnagrafica.errore:", ex)
                Return ""
            End Try
        End Get
    End Property
#End Region
    Public Shared ReadOnly Property UrlServizioStradario() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLServizioStradario") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("URLServizioStradario").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTERRITORIO.ConstSession.UrlServizioStradario.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    '#Region "Parametri RibesFramework"
    '    Public Shared ReadOnly Property ParametroEnv() As String
    '        Get
    '            Try
    '                If Not ConfigurationManager.AppSettings("ParametroEnv") Is Nothing Then
    '                    Return ConfigurationManager.AppSettings("ParametroEnv").ToString
    '                Else
    '                    Return "OG_TERRITORIO"
    '                End If
    '            Catch ex As Exception
    '                Log.Debug(("DichiarazioneSession::ParametroEnv::si è verificato il seguente errore::" + ex.Message))
    '                Return "OG_TERRITORIO"
    '            End Try
    '        End Get
    '    End Property
    Public Shared ReadOnly Property UserName() As String
        Get
            Try
                If (HttpContext.Current.Session("username") Is Nothing) Then
                    Return ConfigurationManager.AppSettings("UserFramework").ToString
                Else
                    Return HttpContext.Current.Session("username").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTERRITORIO.ConstSession.UserName.errore:", ex)
                Return ConfigurationManager.AppSettings("UserFramework").ToString
            End Try
        End Get
    End Property
    '    Public Shared ReadOnly Property IdentificativoApplicazione() As String
    '        Get
    '            Try
    '                If Not ConfigurationManager.AppSettings("IDENTIFICATIVOAPPLICAZIONE").ToString Is Nothing Then
    '                    Return ConfigurationManager.AppSettings("IDENTIFICATIVOAPPLICAZIONE").ToString
    '                Else
    '                    Return "OPENGOVT"
    '                End If
    '            Catch ex As Exception
    '                Log.Debug(("DichiarazioneSession::IdentificativoApplicazione::si è verificato il seguente errore::" + ex.Message))
    '                Return "OPENGOVT"
    '            End Try
    '        End Get
    '    End Property
    '    Public Shared ReadOnly Property ParametroAnagrafica() As String
    '        Get
    '            Try
    '                If Not ConfigurationManager.AppSettings("ANAGRAFICA") Is Nothing Then
    '                    Return ConfigurationManager.AppSettings("ANAGRAFICA")
    '                Else
    '                    Return "OPENGOVA"
    '                End If
    '            Catch ex As Exception
    '                Log.Debug(("DichiarazioneSession::ParametroAnagrafica::si è verificato il seguente errore::" + ex.Message))
    '                Return "OPENGOVA"
    '            End Try
    '        End Get
    '    End Property
    '#End Region
#Region "Path immagini"
    Public Shared ReadOnly Property PathFabbricato() As String
        Get
            Try
                If Not ConfigurationManager.AppSettings("PATH_IMAGES") Is Nothing Then
                    Return ConfigurationManager.AppSettings("PATH_IMAGES").ToString
                Else
                    Return "" '"http://opengov.isimply.it/OPENgovTERRITORIO/archivio/Foto/"
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTERRITORIO.ConstSession.PathFabbricato.errore:", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathPlanimetrie() As String
        Get
            Try
                If Not ConfigurationManager.AppSettings("PATH_IMAGES_PLANIMETRIE") Is Nothing Then
                    Return ConfigurationManager.AppSettings("PATH_IMAGES_PLANIMETRIE").ToString
                Else
                    Return "" '"http://opengov.isimply.it/OPENgovTERRITORIO/archivio/Planimetrie/"
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTERRITORIO.ConstSession.PathPlanimetrie.errore:", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathSaveFabbricato() As String
        Get
            Try
                If Not ConfigurationManager.AppSettings("PATH_SAVEIMAGES_FABBRICATO") Is Nothing Then
                    Return ConfigurationManager.AppSettings("PATH_SAVEIMAGES_FABBRICATO").ToString
                Else
                    Return "~/archivio/Foto/"
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTERRITORIO.ConstSession.PathSaveFabbricato.errore:", ex)
                Return "~/archivio/Foto/"
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathSavePlanimetrie() As String
        Get
            Try
                If Not ConfigurationManager.AppSettings("PATH_SAVEIMAGES_PLANIMETRIE") Is Nothing Then
                    Return ConfigurationManager.AppSettings("PATH_SAVEIMAGES_PLANIMETRIE").ToString
                Else
                    Return "~/archivio/Planimetrie/"
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTERRITORIO.ConstSession.PathSavePlanimetrie.errore:", ex)
                Return "~/archivio/Planimetrie/"
            End Try
        End Get
    End Property
#End Region
    'Public Shared ReadOnly Property IsConnectionByWorkFlow() As Boolean
    '    Get
    '        Try
    '            If Not ConfigurationManager.AppSettings("ConnByWF") Is Nothing Then
    '                Return CBool(ConfigurationManager.AppSettings("ConnByWF"))
    '            Else
    '                Return False
    '            End If
    '        Catch ex As Exception
    '            Log.Debug("IsConnectionByWorkFlow::si è verificato il seguente errore::", ex)
    '            Return False
    '        End Try
    '    End Get
    'End Property
End Class

