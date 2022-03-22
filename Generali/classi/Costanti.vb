Imports log4net

Namespace COSTANTValue
    ''' <summary>
    ''' Classe per la gestione delle variabili da sessione e da config
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    ''' <revisionHistory>
    ''' <revision date="10/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Lista Rilasci</em>
    ''' Aggiunto GetRepositoryRilasci
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="15/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Clausole Contrattuali</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Class ConstSession
        Private Shared Log As ILog = LogManager.GetLogger(GetType(ConstSession))
        Public CountScript As Integer = 0

        Public Structure TipoProfilo
            Shared Amministratore As String = "ADM"
            Shared Operatore As String = "URW"
            Shared SolaLettura As String = "URO"
            Shared ControlloSitoOnLine As String = "CSO"
            Shared SingolaVoceLettura As String = "SFR"
        End Structure

        Public Shared ReadOnly Property GetRepositoryMenu() As String
            Get
                Try
                    If (Not ConfigurationManager.AppSettings("PathMenu") Is Nothing) Then
                        Return ConfigurationManager.AppSettings("PathMenu")
                    Else
                        Return ""
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.GetRepositoryMenu.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property GetRepository() As String
            Get
                Try
                    If (Not ConfigurationManager.AppSettings("PathRepository") Is Nothing) Then
                        Return ConfigurationManager.AppSettings("PathRepository")
                    Else
                        Return ""
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.GetRepository.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property GetRepositoryRilasci() As String
            Get
                Try
                    If (Not ConfigurationManager.AppSettings("RepositoryRilasci") Is Nothing) Then
                        Return ConfigurationManager.AppSettings("RepositoryRilasci")
                    Else
                        Return ""
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.GetRepositoryRilasci.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property GetRepositoryContratti() As String
            Get
                Try
                    If (Not ConfigurationManager.AppSettings("RepositoryContratti") Is Nothing) Then
                        Return ConfigurationManager.AppSettings("RepositoryContratti")
                    Else
                        Return ""
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.GetRepositoryContratti.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property GetRepositoryExport() As String
            Get
                Try
                    If (Not ConfigurationManager.AppSettings("RepositoryExport") Is Nothing) Then
                        Return ConfigurationManager.AppSettings("RepositoryExport")
                    Else
                        Return ""
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.GetRepositoryExport.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property UrlStradario() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("UrlPopUpStradario") Is Nothing) Then
                        Return ""
                    Else
                        Return ConfigurationManager.AppSettings("UrlPopUpStradario").ToString()
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.UrlStradario.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property StileStradario() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("StileStradario") Is Nothing) Then
                        Return ""
                    Else
                        Return ConfigurationManager.AppSettings("StileStradario").ToString()
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.StileStradario.errore: ", ex)
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
                    Log.Debug("OPENgov.ConstSession.UrlComuni.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        'Public Shared ReadOnly Property TributiAbilitati() As String
        '    Get
        '        Try
        '            If (ConfigurationManager.AppSettings("APPLICATIONS_ENABLED") Is Nothing) Then
        '                Return "OPENGOVA,OPENGOVG,OPENGOVI,OPENGOVTA,OPENGOVP"
        '            Else
        '                Return ConfigurationManager.AppSettings("APPLICATIONS_ENABLED")
        '            End If
        '        Catch ex As Exception
        '            Log.Debug("OPENgov.ConstSession.TributiAbilitati.errore: ", ex)
        '            Return "OPENGOVA,OPENGOVG,OPENGOVI,OPENGOVTA,OPENGOVP"
        '        End Try
        '    End Get
        'End Property
        Public Shared ReadOnly Property Ambiente() As String
            Get
                Try
                    If (HttpContext.Current.Session("Ambiente") Is Nothing) Then
                        Return ConfigurationManager.AppSettings("Ambiente")
                    Else
                        Return HttpContext.Current.Session("Ambiente").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.Ambiente.errore: ", ex)
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
                    Log.Debug("OPENgov.ConstSession.IdEnte.errore: ", ex)
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
                    Log.Debug("OPENgov.ConstSession.DescrizioneEnte.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property IsFromVariabile() As String
            Get
                Try
                    If (Ambiente = "CMGC") Then
                        Return "1"
                    Else
                        Return "0"
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.IsFromVariabile.errore: ", ex)
                    Return "0"
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property CodTributo() As String
            Get
                Try
                    If (HttpContext.Current.Session("COD_TRIBUTO") Is Nothing) Then
                        Return ""
                    Else
                        Return HttpContext.Current.Session("COD_TRIBUTO").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.CodTributo.errore: ", ex)
                    Return ""
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
                    Log.Debug("OPENgov.ConstSession.BelFiore.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property TributoTARESF24() As String
            Get
                Try
                    If (HttpContext.Current.Session("TributoTARESF24") Is Nothing) Then
                        Return "3944"
                    Else
                        Return HttpContext.Current.Session("TributoTARESF24").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.TributoTARESF24.errore: ", ex)
                    Return "3944"
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property IdTypeAteco() As Integer
            Get
                Try
                    If (HttpContext.Current.Session("IdTypeAteco") Is Nothing) Then
                        Return 2
                    Else
                        Return CInt(HttpContext.Current.Session("IdTypeAteco"))
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.IdTypeAteco.errore: ", ex)
                    Return 2
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property NameSistemaTerritorio() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("NameSistemaTerritorio") Is Nothing) Then
                        Return "Territorio"
                    Else
                        Return ConfigurationManager.AppSettings("NameSistemaTerritorio")
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.NameSistemaTerritorio.errore: ", ex)
                    Return "Territorio"
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property HasANATER() As Boolean
            Get
                Try
                    If (ConfigurationManager.AppSettings("USO_ANATER") Is Nothing) Then
                        Return False
                    Else
                        Return CBool(ConfigurationManager.AppSettings("USO_ANATER"))
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.HasANATER.errore: ", ex)
                    Return False
                End Try
            End Get
        End Property
        ''' <summary>
        ''' se sono a true vuol dire che ho:
        ''' - la gestione delle dichiarazioni fittizie
        ''' - i parametri di ricerca dichiarazioni completi
        ''' - il calcolo della parte variabile solo se NCPV>0 anziché FLAG_FORZA_CALCOLO_PV
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property HasDummyDich() As Boolean
            Get
                Try
                    If (ConfigurationManager.AppSettings("HasDummyDich") Is Nothing) Then
                        Return False
                    Else
                        Return CBool(ConfigurationManager.AppSettings("HasDummyDich"))
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.HasDummyDich.errore: ", ex)
                    Return False
                End Try
            End Get
        End Property
        '*** 201805 - Expire&Strong Password ***
        Public Shared ReadOnly Property PasswordExpireDays() As Integer
            Get
                Try
                    If (ConfigurationManager.AppSettings("PasswordExpireDays") Is Nothing) Then
                        Return 90
                    Else
                        Return CInt(ConfigurationManager.AppSettings("PasswordExpireDays"))
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.PasswordExpireDays.errore: ", ex)
                    Return 90
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property urlResetPassword() As String
            Get
                Return "ResetPassword.aspx"
            End Get
        End Property
        '*** ***
        '**** 201809 - Cartelle Insoluti ***
        Public Shared ReadOnly Property HasNotifiche() As Boolean
            Get
                Try
                    If (HttpContext.Current.Session("hasruoloinsoluti") Is Nothing) Then
                        If (ConfigurationManager.AppSettings("HasNotifiche0434Ord") Is Nothing) Then
                            Return False
                        Else
                            If ConfigurationManager.AppSettings("HasNotifiche0434Ord") = "1" Then
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    Else
                        Return CBool(HttpContext.Current.Session("hasruoloinsoluti"))
                    End If
                Catch ex As Exception
                    Log.Debug(" - OPENgovTIA.ConstSession.HasNotifiche.errore: ", ex)
                    Return False
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
                    If (HttpContext.Current.Session("StringConnectionOPENgov") Is Nothing) Then
                        Return ConfigurationManager.AppSettings("connectionStringSQLOPENgov").ToString
                    Else
                        Return HttpContext.Current.Session("StringConnectionOPENgov").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.StringConnectionOPENgov.errore: ", ex)
                    Return ConfigurationManager.AppSettings("connectionStringSQLOPENgov").ToString
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
                    Log.Debug("OPENgov.ConstSession.StringConnectionAnagrafica.errore: ", ex)
                    Return ConfigurationManager.AppSettings("connectionStringSQLOPENgov").ToString
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property StringConnectionICI() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("connectionStringOpenGovICI") Is Nothing) Then
                        Return ""
                    Else
                        Return ConfigurationManager.AppSettings("connectionStringOpenGovICI")
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.StringConnectionAnagrafica.errore: ", ex)
                    Return ConfigurationManager.AppSettings("connectionStringOpenGovICI").ToString
                End Try
            End Get
        End Property

        Public Shared ReadOnly Property StringConnectionTARSU() As String
            Get
                Try
                    If Ambiente = "CMGC" Then
                        If (ConfigurationManager.AppSettings("connectionStringOpenGovTIA") Is Nothing) Then
                            Return ""
                        Else
                            Return ConfigurationManager.AppSettings("connectionStringOpenGovTIA").ToString
                        End If
                    Else
                        If (ConfigurationManager.AppSettings("connectionStringOpenGovTARSU") Is Nothing) Then
                            Return ""
                        Else
                            Return ConfigurationManager.AppSettings("connectionStringOpenGovTARSU").ToString
                        End If
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.StringConnectionTARSU.errore: ", ex)
                    Return ConfigurationManager.AppSettings("connectionStringOpenGovTARSU").ToString
                End Try
            End Get
        End Property

        Public Shared ReadOnly Property StringConnectionOSAP() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("connectionStringOpenGovOSAP") Is Nothing) Then
                        Return ""
                    Else
                        Return ConfigurationManager.AppSettings("connectionStringOpenGovOSAP")
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.StringConnectionOSAP.errore: ", ex)
                    Return ConfigurationManager.AppSettings("connectionStringOpenGovOSAP").ToString
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property StringConnectionH2O() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("connectionStringOPENgovH2O") Is Nothing) Then
                        Return ""
                    Else
                        Return ConfigurationManager.AppSettings("connectionStringOPENgovH2O")
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.StringConnectionH2O.errore: ", ex)
                    Return ConfigurationManager.AppSettings("connectionStringOPENgovH2O").ToString
                End Try
            End Get
        End Property

        Public Shared ReadOnly Property StringConnectionPROVVEDIMENTI() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("connectionStringOpenGovPROVVEDIMENTI") Is Nothing) Then
                        Return ""
                    Else
                        Return ConfigurationManager.AppSettings("connectionStringOpenGovPROVVEDIMENTI")
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.StringConnectionPROVVEDIMENTI.errore: ", ex)
                    Return ConfigurationManager.AppSettings("connectionStringOpenGovPROVVEDIMENTI").ToString
                End Try
            End Get
        End Property

#End Region
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
                    Log.Debug("OPENgov.ConstSession.Path_ICI.errore: ", ex)
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
                    Log.Debug("OPENgov.ConstSession.Path_TARSU.errore: ", ex)
                    Return "/OPENgovTIA"
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property Path_OSAP() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("PATH_OPENGOVTOCO") Is Nothing) Then
                        Return "/OPENgovTOCO"
                    Else
                        Return ConfigurationManager.AppSettings("PATH_OPENGOVTOCO").ToString()
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.Path_OSAP.errore: ", ex)
                    Return "/OPENgovTOCO"
                End Try
            End Get
        End Property
        Public Shared ReadOnly Property Path_PROVVEDIMENTI() As String
            Get
                Try
                    If (ConfigurationManager.AppSettings("PATH_OPENGOVP") Is Nothing) Then
                        Return "/OPENgovPROVVEDIMENTI"
                    Else
                        Return ConfigurationManager.AppSettings("PATH_OPENGOVP").ToString()
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.Path_PROVVEDIMENTI.errore: ", ex)
                    Return "/OPENgovPROVVEDIMENTI"
                End Try
            End Get
        End Property
#End Region
#Region "parametri ribesframework"
        Public Shared ReadOnly Property UserName() As String
            Get
                Try
                    If (HttpContext.Current.Session("username") Is Nothing) Then
                        Return "" 'ConfigurationManager.AppSettings("UserFramework").ToString
                    Else
                        Return HttpContext.Current.Session("username").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.UserName.errore: ", ex)
                    Return "" 'ConfigurationManager.AppSettings("UserFramework").ToString
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
                    Log.Debug("OPENgov.ConstSession.ApplicationsEnabled.errore: ", ex)
                    Return "AnagEnte"
                End Try
            End Get
        End Property
        'Public Shared ReadOnly Property ParametroEnv() As String
        '    Get
        '        Try
        '            If (HttpContext.Current.Session("PARAMETROENV") Is Nothing) Then
        '                Return ConfigurationManager.AppSettings("ParametroEnv").ToString
        '            Else
        '                Return HttpContext.Current.Session("PARAMETROENV").ToString
        '            End If
        '        Catch ex As Exception
        '            Log.Debug("OPENgov.ConstSession.ParametroEnv.errore: ", ex)
        '            Return ConfigurationManager.AppSettings("ParametroEnv").ToString
        '        End Try
        '    End Get
        'End Property
        'Public Shared ReadOnly Property IdentificativoApplicazione() As String
        '    Get
        '        Try
        '            If (HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") Is Nothing) Then
        '                HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") = ConfigurationManager.AppSettings("OPENGOVG").ToString
        '                Return ConfigurationManager.AppSettings("OPENGOVG").ToString
        '            Else
        '                Return HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE").ToString
        '            End If
        '        Catch ex As Exception
        '            Log.Debug("OPENgov.ConstSession.IdentificativoApplicazione.errore: ", ex)
        '            HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") = ConfigurationManager.AppSettings("OPENGOVG").ToString
        '            Return ConfigurationManager.AppSettings("OPENGOVG").ToString
        '        End Try
        '    End Get
        'End Property
        'Public Shared ReadOnly Property ParametroOPENgov() As String
        '    Get
        '        Try
        '            If (ConfigurationManager.AppSettings("OPENGOVG") Is Nothing) Then
        '                Return "OPENGOVG"
        '            Else
        '                Return ConfigurationManager.AppSettings("OPENGOVG").ToString
        '            End If
        '        Catch ex As Exception
        '            Log.Debug("OPENgov.ConstSession.ParametroOPENgov.errore: ", ex)
        '            Return "OPENGOVG"
        '        End Try
        '    End Get
        'End Property
        'Public Shared ReadOnly Property ParametroAnagrafica() As String
        '    Get
        '        Try
        '            If (ConfigurationManager.AppSettings("ParametroAnagrafica") Is Nothing) Then
        '                Return "OPENGOVA"
        '            Else
        '                Return ConfigurationManager.AppSettings("ParametroAnagrafica")
        '            End If
        '        Catch ex As Exception
        '            Log.Debug("OPENgov.ConstSession.ParametroAnagrafica.errore: ", ex)
        '            Return "OPENGOVA"
        '        End Try
        '    End Get
        'End Property
        'Public Shared ReadOnly Property ParametroUTENZE() As String
        '    Get
        '        Try
        '            If (ConfigurationManager.AppSettings("OPENGOVUTENZE") Is Nothing) Then
        '                Return "OPENGOVUTENZE"
        '            Else
        '                Return ConfigurationManager.AppSettings("OPENGOVUTENZE").ToString
        '            End If
        '        Catch ex As Exception
        '            Log.Debug("OPENgov.ConstSession.ParametroUTENZE.errore: ", ex)
        '            Return "OPENGOVUTENZE"
        '        End Try
        '    End Get
        'End Property
        Public Shared ReadOnly Property Profilo() As String
            Get
                Try
                    If (HttpContext.Current.Session("Profilo") Is Nothing) Then
                        Return ""
                    Else
                        Return HttpContext.Current.Session("Profilo").ToString
                    End If
                Catch ex As Exception
                    Log.Debug("OPENgov.ConstSession.Profilo.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
#End Region
    End Class
End Namespace
