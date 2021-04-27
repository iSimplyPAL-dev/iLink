Imports log4net

Namespace ANAGRAFICAWEB
    ''' <summary>
    ''' Classe per la gestione delle variabili da sessione e da config
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    Public Class ConstSession
        Private Shared Log As ILog = LogManager.GetLogger(GetType(ConstSession))
        Public Shared CountScript As Integer = 0

        Public Shared ReadOnly Property Ambiente() As String
            Get
                Try
                    If (HttpContext.Current.Session("Ambiente") Is Nothing) Then
                        Return ""
                    Else
                        Return HttpContext.Current.Session("Ambiente").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.Ambiente.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property IdEnte() As String
            Get
                Try
                    If (HttpContext.Current.Session("COD_ENTE") Is Nothing) Then
                        Return ""
                    Else
                        Return HttpContext.Current.Session("COD_ENTE").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.IdEnte.errore: ", ex)
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
                    Log.Debug("Anagrafica.ConstSession.DescrizioneEnte.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property CodTributo() As String
            Get
                Try
                    If (HttpContext.Current.Session("COD_TRIBUTO") Is Nothing) Then
                        Return "0434"
                    Else
                        Return HttpContext.Current.Session("COD_TRIBUTO").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.CodTributo.errore: ", ex)
                    Return "0434"
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property Belfiore() As String
            Get
                Try
                    If (HttpContext.Current.Session("COD_BELFIORE") Is Nothing) Then
                        Return ""
                    Else
                        Return HttpContext.Current.Session("COD_BELFIORE").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.Belfiore.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property Operatore() As String
            Get
                Try
                    If (HttpContext.Current.Session("Operatore") Is Nothing) Then
                        Return "ADMIN"
                    Else
                        Return HttpContext.Current.Session("Operatore").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.Operatore.errore: ", ex)
                    Return "ADMIN"
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property HasResidenti() As Boolean
            Get
                Try
                    If (ConfigurationManager.AppSettings("VisualResidenti") Is Nothing) Then
                        Return False
                    Else
                        Return ConfigurationManager.AppSettings("VisualResidenti")
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.HasResidenti.errore: ", ex)
                    Return False
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property ApplicationsEnabled() As String
            Get
                Try
                    If (HttpContext.Current.Session("APPLICATIONS_ENABLED") Is Nothing) Then
                        If (ConfigurationManager.AppSettings("APPLICATIONS_ENABLED") Is Nothing) Then
                            Return "AnagEnte"
                        Else
                            Return ConfigurationManager.AppSettings("APPLICATIONS_ENABLED").ToString
                        End If
                    Else
                        Return HttpContext.Current.Session("APPLICATIONS_ENABLED").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.ApplicationsEnabled.errore: ", ex)
                    Return "AnagEnte"
                End Try
            End Get
        End Property
#Region "stringhe connessione db"
        Public Shared ReadOnly Property DBType() As String
            Get
                Return "SQL"
            End Get
        End Property
        Public Shared ReadOnly Property StringConnectionOPENgov() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("connectionStringSQLOPENgov") Is Nothing) Then
                        Return ""
                    Else
                        Return ConfigurationManager.AppSettings("connectionStringSQLOPENgov").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.StringConncetionOPENgov.errore: ", ex)
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
                    Log.Debug("Anagrafica.ConstSession.StringConnectionAnagrafica.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
#End Region
#Region "parametri ribesframework"
        Public Shared ReadOnly Property ParametroEnv() As String
            Get
                Try
                    If (HttpContext.Current.Session("PARAMETROENV") Is Nothing) Then
                        Return ConfigurationManager.AppSettings("ParametroEnv").ToString
                    Else
                        Return HttpContext.Current.Session("PARAMETROENV").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.ParametroEnv.errore: ", ex)
                    Return ConfigurationManager.AppSettings("ParametroEnv").ToString
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property UserName() As String
            Get
                Try
                    If (HttpContext.Current.Session("username") Is Nothing) Then
                        Return ConfigurationManager.AppSettings("UserFramework").ToString
                    Else
                        Return HttpContext.Current.Session("username").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.UserName.errore: ", ex)
                    Return ConfigurationManager.AppSettings("UserFramework").ToString
                End Try
            End Get
        End Property
#End Region
        Public Shared ReadOnly Property UrlStradario() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("UrlPopUpStradario") Is Nothing) Then
                        Return ""
                    Else
                        Return ConfigurationManager.AppSettings("UrlPopUpStradario").ToString()
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.UrlStradario.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property UrlComuni() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("UrlPopUpComuni") Is Nothing) Then
                        Return ""
                    Else
                        Return ConfigurationManager.AppSettings("UrlPopUpComuni").ToString()
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.UrlComuni.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property

        Public Shared Function InizializzaCmd(ByVal StringConnection As String) As SqlClient.SqlCommand
            Log.Debug("AnagraficaWEB::InizializzaCmd::apertura della connessione al DB::" & StringConnection)
            Dim cmdMyCommand As New SqlClient.SqlCommand

            Try
                cmdMyCommand.Connection = New SqlClient.SqlConnection(StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                Return cmdMyCommand
            Catch ex As Exception
                Log.Debug("Anagrafica.ConstSession.InizializzaCmd.errore: ", ex)
                Throw New Exception("InizializzaCmd::" & ex.Message)
            End Try
        End Function
#Region "Cartelle altri moduli"
        Public Shared ReadOnly Property Path_ICI() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("PATH_OPENGOVI") Is Nothing) Then
                        Return "/DichiarazioniICI"
                    Else
                        Return ConfigurationManager.AppSettings("PATH_OPENGOVI").ToString()
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.Path_ICI.errore: ", ex)
                    Return "/DichiarazioniICI"
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property Path_TARSU() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("PATH_OPENGOVTIA") Is Nothing) Then
                        Return "/OPENgovTIA"
                    Else
                        Return ConfigurationManager.AppSettings("PATH_OPENGOVTIA").ToString()
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.Path_TARSU.errore: ", ex)
                    Return "/OPENgovTIA"
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property Path_OSAPSCUOLE() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("PATH_OPENGOVTOCO") Is Nothing) Then
                        Return "/OPENgovOSAP"
                    Else
                        Return ConfigurationManager.AppSettings("PATH_OPENGOVTOCO").ToString()
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.Path_OSAPSCUOLE.errore: ", ex)
                    Return "/OPENgovOSAP"
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property Path_H2O() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("PATH_OPENGOVH2O") Is Nothing) Then
                        Return "/OPENgov_H2O"
                    Else
                        Return ConfigurationManager.AppSettings("PATH_OPENGOVH2O").ToString()
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.Path_H2O.errore: ", ex)
                    Return "/OPENgov_H2O"
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property Path_Provvedimenti() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("PATH_OPENGOVP") Is Nothing) Then
                        Return "/OPENgovPROVVEDIMENTI"
                    Else
                        Return ConfigurationManager.AppSettings("PATH_OPENGOVP").ToString()
                    End If
                Catch ex As Exception
                    Log.Debug("Anagrafica.ConstSession.Path_Provvedimenti.errore: ", ex)
                    Return "/OPENgovPROVVEDIMENTI"
                End Try
            End Get
        End Property
#End Region
    End Class
    ''' <summary>
    ''' Classe per la gestione delle funzioni di formattazione per le griglie
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    Public Class FunctionGrd
        Private Shared Log As ILog = LogManager.GetLogger(GetType(FunctionGrd))
        ''' <summary>
        ''' Funzione che concatena cognome e nome
        ''' </summary>
        ''' <param name="Cognome"></param>
        ''' <param name="Nome"></param>
        ''' <returns>string risultante dalla concatenzazione</returns>
        Public Function FormattaNominativo(ByVal Cognome As Object, ByVal Nome As Object) As String
            Dim ret As String = ""
            Try
                If Not IsDBNull(Cognome) Then
                    ret += Cognome.ToString
                End If
                If Not IsDBNull(Nome) Then
                    ret += " " + Nome.ToString
                End If
            Catch ex As Exception
                Log.Debug("Anagrafica.FunctionGrd.FormattaNominativo.errore: ", ex)
                ret = ""
            End Try
            Return ret.Trim
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Via"></param>
        ''' <param name="Civico"></param>
        ''' <param name="Posizione"></param>
        ''' <param name="Esponente"></param>
        ''' <param name="Scala"></param>
        ''' <param name="Interno"></param>
        ''' <param name="Frazione"></param>
        ''' <returns></returns>
        Public Function FormattaVia(ByVal Via As Object, ByVal Civico As Object, ByVal Posizione As Object, ByVal Esponente As Object, ByVal Scala As Object, ByVal Interno As Object, ByVal Frazione As Object) As String
            Dim ret As String = String.Empty

            Try
                If Not IsDBNull(Via) Then
                    ret += Via.ToString
                End If
                If Not IsDBNull(Civico) Then
                    If CStr(Civico) <> "0" And CStr(Civico) <> "-1" Then
                        ret += " " + CStr(Civico)
                    End If
                End If
                If Not IsDBNull(Posizione) Then
                    If CStr(Posizione) <> "" Then
                        ret += " " + CStr(Posizione)
                    End If
                End If
                If Not IsDBNull(Esponente) Then
                    If CStr(Esponente) <> "" Then
                        ret += " " + CStr(Esponente)
                    End If
                End If
                If Not IsDBNull(Scala) Then
                    If CStr(Scala) <> "" Then
                        ret += " Sc." + CStr(Scala)
                    End If
                End If
                If Not IsDBNull(Interno) Then
                    If CStr(Interno) <> "" Then
                        ret += " Int." + CStr(Interno)
                    End If
                End If
                If Not IsDBNull(Frazione) Then
                    If CStr(Frazione) <> "" Then
                        ret += " Fraz." + Frazione.ToString
                    End If
                End If
            Catch ex As Exception
                Log.Debug("Anagrafica.FunctionGrd.FormattaVia.errore: ", ex)
                ret = ""
            End Try
            Return ret.Trim
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="CAP"></param>
        ''' <param name="Comune"></param>
        ''' <param name="Prov"></param>
        ''' <returns></returns>
        Public Function FormattaComune(ByVal CAP As Object, ByVal Comune As Object, ByVal Prov As Object) As String
            Dim ret As String = ""
            Try
                If Not IsDBNull(CAP) Then
                    If CStr(CAP) <> "" Then
                        ret += CAP.ToString
                    End If
                End If
                If Not IsDBNull(Comune) Then
                    If CStr(Comune) <> "" Then
                        ret += " " + Comune.ToString
                    End If
                End If
                If Not IsDBNull(Prov) Then
                    If CStr(Prov) <> "" Then
                        ret += " (" + Prov.ToString + ")"
                    End If
                End If
            Catch ex As Exception
                Log.Debug("Anagrafica.FunctionGrd.FormattaComune.errore: ", ex)
                ret = ""
            End Try
            Return ret.Trim
        End Function
    End Class
End Namespace
