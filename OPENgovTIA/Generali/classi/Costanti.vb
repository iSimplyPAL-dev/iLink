Imports log4net
''' <summary>
''' Classe per la gestione delle variabili costanti
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class Costanti
    Public Const SuperUser As String = "SUPPAUSR"
    Public Class FormProvenienza
        Public Const Dichiarazione As Integer = 1
        Public Const Tessera As Integer = 2
        Public Const DatiAE As Integer = 3
        Public Const ICICheckRif As Integer = 4
        Public Const ICIDich As Integer = 5
        Public Const InterGen As Integer = 6
        Public Const Notifiche As Integer = 7
    End Class
    Public Class BloccoSgravi
        Public Const NO As Integer = -1
        Public Const Bloccato As Integer = 0
        Public Const Fine As Integer = 1
    End Class
    Public Enum enmContesto
        None = 0
        DELETTURE = 1    'DATA ENTRY LETTURE
        DECONTATORI = 2 'DATA ENTRY CONTATORI
    End Enum
    Public Enum TipoDefaultCmb
        None = 0
        STRINGA = 1
        NUMERO = 2
    End Enum
End Class

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
                Log.Debug(" - OPENgovTIA.ConstSession.Ambiente.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property IdEnte() As String
        Get
            Try
                'HttpContext.Current.Session("COD_ENTE") = "096024"
                If (HttpContext.Current.Session("COD_ENTE") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("COD_ENTE").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.IdEnte.errore: ", ex)
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
                Log.Debug(" - OPENgovTIA.ConstSession.DescrizioneEnte.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property CodTributo() As String
        Get
            Try
                If (HttpContext.Current.Session("COD_TRIBUTO") Is Nothing) Then
                    Return Utility.Costanti.TRIBUTO_TARSU
                Else
                    Return HttpContext.Current.Session("COD_TRIBUTO").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.CodTributo.errore: ", ex)
                Return Utility.Costanti.TRIBUTO_TARSU
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
                Log.Debug(" - OPENgovTIA.ConstSession.Belfiore.errore: ", ex)
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
                Log.Debug(" - OPENgovTIA.ConstSession.TributoTARESF24.errore: ", ex)
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
                Log.Debug(" - OPENgovTIA.ConstSession.IdTypeAteco.errore: ", ex)
                Return 2
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property IsFromTARES() As String
        Get
            Try
                If (HttpContext.Current.Session("IsFromTARES") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("IsFromTARES").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.IsFromTARES.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property IsFromVariabile(ByVal MenuFrom As String) As String
        Get
            Try
                'non da sessione ma da request del menù selezionato per poi memorizzarla in sessione
                If MenuFrom <> "" Then
                    HttpContext.Current.Session("IsFromVariabile") = MenuFrom
                    Return MenuFrom
                Else
                    HttpContext.Current.Session("IsFromVariabile") = ""
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.IsFromVariabile.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property IsFromVariabile() As String
        Get
            Try
                If (HttpContext.Current.Session("IsFromVariabile") Is Nothing) Then
                    HttpContext.Current.Session("IsFromVariabile") = "0"
                    Return "0"
                Else
                    If HttpContext.Current.Session("IsFromVariabile").ToString.Trim = "" Then
                        HttpContext.Current.Session("IsFromVariabile") = "0"
                    End If
                    Return HttpContext.Current.Session("IsFromVariabile").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.IsFromVariabile.errore: ", ex)
                HttpContext.Current.Session("IsFromVariabile") = "0"
                Return "0"
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
                Log.Debug(" - OPENgovTIA.ConstSession.NameSistemaTerritorio.errore: ", ex)
                Return "Territorio"
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property nMaxDocPerFile() As Integer
        Get
            Try
                If (ConfigurationManager.AppSettings("NDocPerFile") Is Nothing) Then
                    Return 50
                Else
                    Return CInt(ConfigurationManager.AppSettings("NDocPerFile"))
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.nMaxDocPerFile.errore: ", ex)
                Return 50
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property nPercentMQCat() As Double
        Get
            Try
                If (ConfigurationManager.AppSettings("PercentualeSupCatastale") Is Nothing) Then
                    Return 0.8
                Else
                    Return ConfigurationManager.AppSettings("PercentualeSupCatastale")
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.nPercentMQCat.errore: ", ex)
                Return 0.8
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property IsMinutaXStampatore() As Integer
        Get
            Try
                If (ConfigurationManager.AppSettings("MinutaXStampatore") Is Nothing) Then
                    Return 0
                Else
                    Return CInt(ConfigurationManager.AppSettings("MinutaXStampatore"))
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.IsMinutaXStampatore.errore: ", ex)
                Return False
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property bPFPVUniqueRow() As Integer
        Get
            Try
                If (ConfigurationManager.AppSettings("PFPVUniqueRow") Is Nothing) Then
                    Return 0
                Else
                    Return CInt(ConfigurationManager.AppSettings("PFPVUniqueRow"))
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.bPFPVUniqueRow.errore: ", ex)
                Return False
            End Try
        End Get
    End Property
    ''' <summary>
    ''' se sono a true vuol dire che ho un unico pannello di visualizzazione anagrafica incluso nelle varie pagine
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property HasPlainAnag() As Boolean
        Get
            Try
                If (ConfigurationManager.AppSettings("HasPlainAnag") Is Nothing) Then
                    Return False
                Else
                    Return CBool(ConfigurationManager.AppSettings("HasPlainAnag"))
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.HasPlainAnag.errore: ", ex)
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
                Log.Debug(" - OPENgovTIA.ConstSession.HasDummyDich.errore: ", ex)
                Return False
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property bTransitoPag() As Boolean
        Get
            Try
                If (ConfigurationManager.AppSettings("TransitoPag") Is Nothing) Then
                    Return False
                Else
                    Return CBool(ConfigurationManager.AppSettings("TransitoPag"))
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.bTransitoPag.errore: ", ex)
                Return False
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property LinguaDate() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("lingua_date") Is Nothing) Then
                    Return False
                Else
                    Return ConfigurationManager.AppSettings("lingua_date").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.LinguaDate.errore: ", ex)
                Return False
            End Try
        End Get
    End Property
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
    Public Shared ReadOnly Property StringConnection() As String
        Get
            Try
                '    If (HttpContext.Current.Session("StringConnection") Is Nothing) Then
                '        Return ConfigurationManager.AppSettings("connectionStringOpenGovTIA").ToString
                '    Else
                '        Return HttpContext.Current.Session("StringConnection").ToString
                '    End If
                'Catch ex As Exception
                '    Log.Debug(("DichiarazioneSession::StringConnection::si è verificato il seguente errore::" + ex.Message))
                '    Return ConfigurationManager.AppSettings("connectionStringOpenGovTIA").ToString
                'If (HttpContext.Current.Session("StringConnection") Is Nothing) Then
                If IsFromVariabile = "1" Then
                    HttpContext.Current.Session("StringConnection") = ConfigurationManager.AppSettings("connectionStringOpenGovTIA").ToString
                    Return ConfigurationManager.AppSettings("connectionStringOpenGovTIA").ToString
                Else
                    HttpContext.Current.Session("StringConnection") = ConfigurationManager.AppSettings("connectionStringOpenGovTARSU").ToString
                    Return ConfigurationManager.AppSettings("connectionStringOpenGovTARSU").ToString
                End If
                'Else
                'Return HttpContext.Current.Session("StringConnection").ToString
                'End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.StringConnection.errore: ", ex)
                If IsFromVariabile = "1" Then
                    HttpContext.Current.Session("StringConnection") = ConfigurationManager.AppSettings("connectionStringOpenGovTIA").ToString
                    Return ConfigurationManager.AppSettings("connectionStringOpenGovTIA").ToString
                Else
                    HttpContext.Current.Session("StringConnection") = ConfigurationManager.AppSettings("connectionStringOpenGovTARSU").ToString
                    Return ConfigurationManager.AppSettings("connectionStringOpenGovTARSU").ToString
                End If
            End Try
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
                Log.Debug(" - OPENgovTIA.ConstSession.StringConnectionOPENgov.errore: ", ex)
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
                Log.Debug(" - OPENgovTIA.ConstSession.StringConnectionAnagrafica.errore: ", ex)
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
                Log.Debug(" - OPENgovTIA.ConstSession.StringConnectionICI.errore: ", ex)
                Return ConfigurationManager.AppSettings("connectionStringOpenGovICI").ToString
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
                Log.Debug(" - OPENgovTIA.ConstSession.StringConnectionOSAP.errore: ", ex)
                Return ConfigurationManager.AppSettings("connectionStringOpenGovOSAP").ToString
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property StringConnectionProvv() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("connectionStringOPENgovPROVVEDIMENTI") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("connectionStringOPENgovPROVVEDIMENTI")
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.StringConnectionProvv.errore: ", ex)
                Return ConfigurationManager.AppSettings("connectionStringOPENgovPROVVEDIMENTI").ToString
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
                Log.Debug(" - OPENgovTIA.ConstSession.ParametroEnv.errore: ", ex)
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
                Log.Debug(" - OPENgovTIA.ConstSession.UserName.errore: ", ex)
                Return ConfigurationManager.AppSettings("UserFramework").ToString
            End Try
        End Get
    End Property
    'Public Shared ReadOnly Property IdentificativoApplicazione() As String
    '    Get
    '        Try
    '            If (HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") Is Nothing) Then
    '                If IsFromVariabile() = "1" Then
    '                    HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") = ConfigurationManager.AppSettings("OPENGOVTIA").ToString
    '                    Return ConfigurationManager.AppSettings("OPENGOVTIA").ToString
    '                Else
    '                    HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") = ConfigurationManager.AppSettings("OPENGOVTA").ToString
    '                    Return ConfigurationManager.AppSettings("OPENGOVTA").ToString
    '                End If
    '            Else
    '                Return HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE").ToString
    '            End If
    '        Catch ex As Exception
    '            Log.Debug(" - OPENgovTIA.ConstSession.IdentificativoApplicazione.errore: ", ex)
    '            If IsFromVariabile() = "1" Then
    '                HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") = ConfigurationManager.AppSettings("OPENGOVTIA").ToString
    '                Return ConfigurationManager.AppSettings("OPENGOVTIA").ToString
    '            Else
    '                HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") = ConfigurationManager.AppSettings("OPENGOVTA").ToString
    '                Return ConfigurationManager.AppSettings("OPENGOVTA").ToString
    '            End If
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
    '            Log.Debug(" - OPENgovTIA.ConstSession.ParametroAnagrafica.errore: ", ex)
    '            Return "OPENGOVA"
    '        End Try
    '    End Get
    'End Property
#End Region
#Region "Url Servizi/Siti"
    Public Shared ReadOnly Property UrlStradario() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("UrlPopUpStradario") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("UrlPopUpStradario").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.UrlStradario.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlTerritorio() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("UrlPopUpTerritorio") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("UrlPopUpTerritorio").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.UrlTerritorio.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlServizioRuolo() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLServizioRuoloTARSU") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("URLServizioRuoloTARSU").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.UrlServizioRuolo.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlServizioStampeICI() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLElaborazioneDatiStampeICI") Is Nothing) Then
                    Return "tcp://192.168.14.187:33446/ElaborazioneStampeICI.rem"
                Else
                    Return ConfigurationManager.AppSettings("URLElaborazioneDatiStampeICI").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.UrlServizioStampeICI.errore: ", ex)
                Return "tcp://192.168.14.187:33446/ElaborazioneStampeICI.rem"
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlServizioAnater() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLanaterTARSU") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("URLanaterTARSU").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.UrlServizioAnater.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlWSOPENae() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLWSOPENae") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("URLWSOPENae").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.UrlWSOPENae.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlServiziLiquidazione() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLServiziLiquidazioni") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("URLServiziLiquidazioni").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.UrlServiziLiquidazione.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlServiziAccertamenti() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLServiziAccertamenti") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("URLServiziAccertamenti").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.UrlServiziAccertamenti.errore: ", ex)
                Return ""
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
                Log.Debug(" - OPENgovTIA.ConstSession.Path_ICI.errore: ", ex)
                Return "/DichiarazioniICI"
            End Try
        End Get
    End Property
#End Region
#Region "Path"
    Public Shared ReadOnly Property PathProspetti() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_PROSPETTI_EXCEL") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_PROSPETTI_EXCEL").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathProspetti.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathImport() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_ACQUISIZIONE") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_ACQUISIZIONE").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathImport.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathSpostaImport() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathSpostaImport.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathSpostaNoImport() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_SPOSTA_NON_ACQUISITO") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_SPOSTA_NON_ACQUISITO").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathSpostaNoImport.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathPagamentiInLavorazione() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_INACQUISIZIONE_PAG") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_INACQUISIZIONE_PAG").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathPagamentiInLavorazione.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathPagamentiOK() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_OKACQUISIZIONE_PAG") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_OKACQUISIZIONE_PAG").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathPagamentiOK.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathPagamentiScarti() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_KOACQUISIZIONE_PAG") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_KOACQUISIZIONE_PAG").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathPagamentiScarti.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathF24Acquisiti() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_F24_ACQUISITI") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_F24_ACQUISITI").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathF24Acquisti.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathF24() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_F24") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_F24").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathF24.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathStampe() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("DIRTEMPLATE") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("DIRTEMPLATE").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathStampe.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathVirtualStampe() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("DIRTEMPLATEVIRTUAL") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("DIRTEMPLATEVIRTUAL").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathVirtualStampe.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathZIP() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_ZIP") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_ZIP").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathZIP.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathFileAE() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_FILE_AE") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_FILE_AE")
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathFileAE.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathFileIsolaEcologica() As String
        Get
            Try
                If (HttpContext.Current.Session("PathFileIsolaEcologica") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("PathFileIsolaEcologica").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.PathFileIsolaEcologica.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
#End Region
#Region "GIS"
    Public Shared ReadOnly Property VisualGIS() As Boolean
        Get
            Try
                If (HttpContext.Current.Session("VisualGIS") Is Nothing) Then
                    Return False
                Else
                    Return CBool(HttpContext.Current.Session("VisualGIS"))
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.VisualGIS.errore: ", ex)
                Return False
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlWSGIS() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("UrlWSGIS") Is Nothing) Then
                    Return "http://gis.oikosweb.com/CATWS/Gismappale.asmx"
                Else
                    Return ConfigurationManager.AppSettings("UrlWSGIS").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.UrlWSGIS.errore: ", ex)
                Return "http://gis.oikosweb.com/CATWS/Gismappale.asmx"
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlWebGIS() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("UrlWebGIS") Is Nothing) Then
                    Return "http://map.portalecomuni.net/mapguide/wgis/ddd.html?&GisGuidMap="
                Else
                    If ConfigurationManager.AppSettings("UrlWebGIS").ToString().EndsWith("&GisGuidMap=") = False Then
                        Return ConfigurationManager.AppSettings("UrlWebGIS").ToString() + "&GisGuidMap="
                    Else
                        Return ConfigurationManager.AppSettings("UrlWebGIS").ToString()
                    End If
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovTIA.ConstSession.UrlWebGIS.errore: ", ex)
                Return "http://map.portalecomuni.net/mapguide/wgis/ddd.html?&GisGuidMap="
            End Try
        End Get
    End Property
#End Region
End Class