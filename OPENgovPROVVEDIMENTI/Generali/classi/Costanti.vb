Imports System.Web.Security
Imports log4net
''' <summary>
''' Classe generale che eredita BasePage.
''' Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class BaseEnte
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(BaseEnte))
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BaseEnte_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            If ConstSession.IdEnte = "" Then
                RegisterScript("GestAlert('a', 'warning', 'CmdLogOut', '', 'Sessione scaduta rieffettuare LOGIN');", Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(" - OPENgovPROVVEDIMENTI.BaseEnte.BasePage_Init.errore: ", ex)
        End Try
    End Sub
End Class
''' <summary>
''' Classe generale che eredita Page.
''' Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class BasePage
    Inherits Page
    Private Shared Log As ILog = LogManager.GetLogger(GetType(BasePage))
    Public Shared authCookie As HttpCookie
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BasePage_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            Dim isExpired As Boolean = False
            'get cookie
            authCookie = HttpContext.Current.Request.Cookies.Get("aplckute")
            If Not authCookie Is Nothing Then
                Dim authTicket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(authCookie.Value)
                If Not authTicket Is Nothing Then
                    If authTicket.Expired = False Then
                        Dim myData As String() = authTicket.UserData.Split("|")
                        If Not myData Is Nothing Then
                            If myData.GetUpperBound(0) <> 1 Then
                                Log.Debug("sessione scaduta perchè non ho userdata nel cookie")
                                isExpired = True
                            Else
                                'If myData(0) <> ConstSession.UserName Then
                                Log.Debug("utente cookie->" + myData(0) + "  utente sessione->" + ConstSession.UserName)
                                If ConstSession.UserName <> "" Then
                                    'Log.Debug("sessione scaduta perchè utente cookie diverso da utente sessione")
                                    'isExpired = True
                                Else
                                    Session("username") = myData(0)
                                    End If
                                'End If
                            End If
                        Else
                            Log.Debug("sessione scaduta perchè userdata cookie null")
                            isExpired = True
                        End If
                    Else
                        Log.Debug("sessione scaduta perchè cookie scaduto")
                        isExpired = True
                    End If
                Else
                    Log.Debug("sessione scaduta perchè non ticket cookie")
                    isExpired = True
                End If
            Else
                Log.Debug("sessione scaduta perchè non cookie")
                isExpired = True
            End If
            If ConstSession.UserName = "" Then
                Log.Debug("sessione scaduta perchè sessione operatore vuoto")
                isExpired = True
            End If
            If isExpired = True Then
                HttpContext.Current.Session("username") = ""
                RegisterScript("GestAlert('a', 'warning', 'CmdLogOut', '', 'Sessione scaduta rieffettuare LOGIN');", Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.BasePage_Init.errore: ", ex)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="script"></param>
    ''' <param name="type"></param>
    Protected Sub RegisterScript(ByVal script As String, ByVal type As Type)
        ConstSession.CountScript = (ConstSession.CountScript + 1)
        Try
            Dim uniqueId As String = ("spc_" _
                    + (ConstSession.CountScript.ToString _
                    + (DateTime.Now.ToString + ("." + DateTime.Now.Millisecond.ToString))))
            Dim sScript As String = "<script language='javascript'>"
            sScript += script
            sScript += "</script>"
            ClientScript.RegisterStartupScript(type, uniqueId, sScript)
        Catch ex As Exception
            Log.Debug(" - OPENgovPROVVEDIMENTI.BaseRegisterScript.errore: ", ex)
        End Try
    End Sub
End Class
''' <summary>
''' Classe per la gestione delle variabili costanti
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class CostantiProvv
    '    Public Const ID_PROVVEDIMENTO_LIQUIDAZIONE As Integer = 4 ' avviso di accertamento in rettifica
    '   Public Const VOCE_TARDIVO_VERSAMENTO As String = "2"
    'Public Const TRIBUTO_ICI As String = "8852"
    'Public Const TRIBUTO_TARSU As String = "0434"
    'Public Const TRIBUTO_H2O As String = "9000"
    ''*** 20130801 - accertamento OSAP ***
    'Public Const TRIBUTO_OSAP As String = "0453" '"OSAP"
    ''*** ***
    'Public Const INIT_VALUE_NUMBER As Integer = -1
    'Public Const INIT_VALUE_NUMBER_STRING As String = "-1"
    'Public Const INIT_VALUE_STRING As String = ""
    'Public Const INIT_VALUE_BOOL As Boolean = False
    'Public Const VALUE_NUMBER_ZERO As Integer = 0
    'Public Const VALUE_NUMBER_UNO As Integer = 1
    'Public Const VALUE_INCREMENT As Integer = 1

    'Public Const MASCHIO As String = "M"
    'Public Const FEMMINA As String = "F"
    'Public Const PERSONAGIURIDICA As String = "G"

    'Public Const ID_FASE1 As Integer = 1
    'Public Const ID_FASE2 As Integer = 2
    'Public Const ID_FASE3 As Integer = 3

    'Public Const PROVVEDIMENTO_NESSUN_AVVISO As Integer = 0
    'Public Const PROVVEDIMENTO_QUESTIONARIO As Integer = 1
    ''**** 201809 - Cartelle Insoluti ***
    'Public Const PROVVEDIMENTO_INGIUNZIONE As Integer = 2
    'Public Const PROVVEDIMENTO_COATTIVO As Integer = 8
    'Public Const PROVVEDIMENTO_RAVVEDIMENTOOPEROSO As Integer = 9
    ''*** ***
    'Public Const PROVVEDIMENTO_ACCERTAMENTO_UFFICIO As Integer = 3
    'Public Const PROVVEDIMENTO_ACCERTAMENTO_RETTIFICA As Integer = 4
    'Public Const PROVVEDIMENTO_RIMBORSO As Integer = 5
    'Public Const PROVVEDIMENTO_AUTOTUTELA_RETTIFICA As Integer = 6
    'Public Const PROVVEDIMENTO_AUTOTUTELA_ANNULLAMENTO As Integer = 7

    'Public Const COD_CAPITOLO_SANZIONE As String = "0002"
    'Public Const COD_CAPITOLO_INTERESSE As String = "0003"
    'Public Const COD_CAPITOLO_SPESE As String = "0004"
    Public Const ID_PROVVEDIMENTO_LIQUIDAZIONE As Integer = 2
    Public Const VOCE_TARDIVO_VERSAMENTO As String = "2"

    'DATI PER GESTIRE I DOCUMENTI INVIATI AI CONTRIBUENTI
    'Public Const TIPO_DOC_LETTERA As String = "LETTERA" 'LETTERA GENERICA DA PROVVEDIMENTI
    'Public Const TIPO_DOC_LETTERA_CONTATTO As String = "CONTATTO" ' LETTERA SPECIFICA DA PRE ACCERTAMENTO
    'Public Const TIPO_DOC_RICHIESTAINFO As String = "RICHIESTAINFO" 'RICHIESTA INFO DA PRE ACCERTAMENTO
    'Public Const TIPO_DOC_INFORMATIVA_ICI As String = "INFORMATIVAICI" 'INFORMATIVA ICI
    'Public Const TIPO_DOC_INFORMATIVA_TARSU As String = "INFORMATIVATARSU" 'INFORMATIVA TARSU

    '*** 20120704 - IMU ***
    'DATI PER GESTIRE I DOCUMENTI INVIATI AI CONTRIBUENTI
    Public Const DESCRIZIONE_TIPO_DOC_LETTERA As String = "Lettera" 'LETTERA GENERICA DA PROVVEDIMENTI
    Public Const DESCRIZIONE_TIPO_DOC_LETTERA_CONTATTO As String = "Lettera di Contatto" ' LETTERA SPECIFICA DA PRE ACCERTAMENTO
    Public Const DESCRIZIONE_TIPO_DOC_RICHIESTAINFO As String = "Richiesta Informazioni" 'RICHIESTA INFO DA PRE ACCERTAMENTO
    Public Const DESCRIZIONE_TIPO_DOC_INFORMATIVA_ICI As String = "Informativa ICI/IMU" 'INFORMATIVA ICI
    'Public Const DESCRIZIONE_TIPO_DOC_INFORMATIVA_TARSU As String = "Informativa TARSU" 'INFORMATIVA TARSU
    Public Const DESCRIZIONE_TIPO_DOC_PREACCERTAMENTO As String = "Avviso Pre Accertamento ICI/IMU"
    Public Const DESCRIZIONE_TIPO_DOC_PREACCERTAMENTO_BOZZA As String = "Bozza Avviso Pre Accertamento ICI/IMU"
    Public Const DESCRIZIONE_TIPO_DOC_QUESTIONARIO As String = "Questionario ICI/IMU"
    Public Const DESCRIZIONE_TIPO_DOC_RIMBORSO_ICI As String = "Rimborso ICI/IMU"
    Public Const DESCRIZIONE_TIPO_DOC_RIMBORSO_ICI_BOZZA As String = "Bozza Rimborso ICI/IMU"

    Public Const COD_TIPO_DOC_INFORMATIVA_ICI As String = "1"
    Public Const COD_TIPO_DOC_LETTERA As String = "2"
    Public Const COD_TIPO_DOC_LETTERA_CONTATTO As String = "3"
    Public Const COD_TIPO_DOC_RICHIESTAINFO As String = "4"
    Public Const COD_TIPO_DOC_QUESTIONARIO As String = "5"
    Public Const COD_TIPO_DOC_PREACCERTAMENTO As String = "6"
    Public Const COD_TIPO_DOC_PREACCERTAMENTO_BOZZA As String = "7"
    Public Const COD_TIPO_DOC_ANNULLAMENTO As String = "14"

    Public Const COD_TIPO_DOC_BOLL_PRE_E_ACCERTAMENTO As String = "8"
    Public Const COD_TIPO_DOC_ACCERTAMENTO As String = "9"
    Public Const COD_TIPO_DOC_ACCERTAMENTO_BOZZA As String = "10"
    Public Const COD_TIPO_DOC_RIMBORSO_ICI As String = "11"
    Public Const COD_TIPO_DOC_RIMBORSO_ICI_BOZZA As String = "12"
    Public Const COD_TIPO_DOC_BOLL_PRE_E_ACCERTAMENTO_RATEIZZATI As String = "13"

    'Public Const COD_TIPO_DOC_INFORMATIVA_TARSU As String = "4"
    'Public Const PROCEDIMENTO_QUESTIONARIO As String = "Q"
    'Public Const PROCEDIMENTO_LIQUIDAZIONE As String = "L"
    'Public Const PROCEDIMENTO_ACCERTAMENTO As String = "A"

    'var per gestire la stampa dei doc nella pagina ristampaOriginaleL.aspx
    'e nella pagina ristampaOriginaleA.aspx
    Public Const DOCUMENTO_ACCERTAMENTO As String = "DOCACC"
    Public Const DOCUMENTO_PREACCERTAMENTO As String = "DOCPREACC"
    Public Const DOCUMENTO_ACCERTAMENTO_BOZZA As String = "DOCACCBOZZA"
    Public Const DOCUMENTO_PREACCERTAMENTO_BOZZA As String = "DOCPREACCBOZZA"
    Public Const DOCUMENTO_RIMBORSO_ICI As String = "DOCRIMBORSO_ICI"
    Public Const DOCUMENTO_RIMBORSO_ICI_BOZZA As String = "DOCRIMBORSO_ICI_BOZZA"
    Public Const DOCUMENTO_ANNULLAMENTO As String = "DOCANNULLAMENTO"
    Public Const DOCUMENTO_LETTERA_RI As String = "LETTERARI"
    Public Const DOCUMENTO_RI As String = "RI"

    Public Const BOLLETTINI_ACCERTAMENTO As String = "BOLLACC"
    Public Const BOLLETTINI_ACCERTAMENTO_RATE As String = "BOLLACC_RATE"

    '**** 201809 - Cartelle Insoluti ***
    Public Enum Provenienza
        DataEntry = 1
        NotificaCartelle = 2
    End Enum
    '*** ***

    Public Enum enmContesto
        DELETTURE = 1    'DATA ENTRY LETTURE
        DECONTATORI = 2 'DATA ENTRY CONTATORI
    End Enum
End Class
''' <summary>
''' Classe per la gestione delle variabili da sessione e da config
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ConstSession
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConstSession))
    Public Shared CountScript As Integer = 0
    Public Shared nTry As Integer = 0

    Public Shared ReadOnly Property Ambiente() As String
        Get
            Try
                If (HttpContext.Current.Session("Ambiente") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("Ambiente").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.Ambiente.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property IdEnte() As String
        Get
            Try
                If (HttpContext.Current.Session("cod_ente") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("cod_ente").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.IdEnte.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property IdEnteCredBen() As String
        Get
            Try
                If (HttpContext.Current.Session("idente_credben") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("idente_credben").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.IdEnteCredBen.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property IdEnteCNC() As String
        Get
            Try
                If (HttpContext.Current.Session("idente_cnc") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("idente_cnc").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.IdEnteCNC.errore: ", ex)
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
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.DescrizioneEnte.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property CodTributo() As String
        Get
            Try
                If (HttpContext.Current.Session("COD_TRIBUTO") Is Nothing) Then
                    Return "8852"
                Else
                    Return HttpContext.Current.Session("COD_TRIBUTO").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.CodTributo.errore: ", ex)
                Return "8852"
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
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.Belfiore.errore: ", ex)
                Return ""
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
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.IdTypeAteco.errore: ", ex)
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
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.IsFromTARES.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property TributiBollettinoF24() As String
        Get
            Try
                If (HttpContext.Current.Session("TributiBollettinoF24") Is Nothing) Then
                    Return ""
                Else
                    Return HttpContext.Current.Session("TributiBollettinoF24").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.TributiBollettinoF24.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property IsFromVariabile(ByVal MenuFrom As String) As String
        Get
            Try
                'non da sessione ma da request del menù selezionato per poi memorizzarla in sessione
                If MenuFrom <> "" Then
                    HttpContext.Current.Session("IsFromVariabile") = "1"
                    Return "1"
                Else
                    HttpContext.Current.Session("IsFromVariabile") = ""
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.IsFromVariabile.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property IsFromVariabile() As String
        Get
            Try
                If (HttpContext.Current.Session("IsFromVariabile") Is Nothing) Then
                    If IdentificativoApplicazione = "OPENgovPROVVEDIMENTI" Then
                        If IdEnte = "007025" Or IdEnte = "007039" Then
                            Return ""
                        Else
                            Return "1"
                        End If
                    Else
                        Return ""
                    End If
                Else
                    If IdentificativoApplicazione = "OPENgovPROVVEDIMENTI" Then
                        If IdEnte = "007025" Or IdEnte = "007039" Then
                            Return ""
                        Else
                            Return "1"
                        End If
                    Else
                        Return ""
                    End If
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.IsFromVariabile.errore: ", ex)
                If IdentificativoApplicazione = "OPENgovPROVVEDIMENTI" Then
                    If IdEnte = "007025" Or IdEnte = "007039" Then
                        Return ""
                    Else
                        Return "1"
                    End If
                Else
                    Return ""
                End If
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
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.HasPlainAnag.errore: ", ex)
                Return False
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
                Log.Debug("OPENgovPROVVEDIMENTI.ConstSession.UrlStradario.errore: ", ex)
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
    Public Shared ReadOnly Property NameSistemaTerritorio() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("NameSistemaTerritorio") Is Nothing) Then
                    Return "Territorio"
                Else
                    Return ConfigurationManager.AppSettings("NameSistemaTerritorio")
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.NameSistemaTerritorio.errore: ", ex)
                Return "Territorio"
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property CONFIGURAZIONE_DICHIARAZIONE() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("CONFIGURAZIONE_DICHIARAZIONE") Is Nothing) Then
                    Return "False"
                Else
                    Return ConfigurationManager.AppSettings("CONFIGURAZIONE_DICHIARAZIONE").ToString()
                End If
            Catch ex As Exception

                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.CONFIGURAZIONE_DICHIARAZIONE.errore: ", ex)
                Return "False"
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
                If (ConfigurationManager.AppSettings("connectionStringOPENgovPROVVEDIMENTI") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("connectionStringOPENgovPROVVEDIMENTI").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.StringConnection.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property NameDBProvvedimenti() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("NOME_DATABASE_PROVVEDIMENTI") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("NOME_DATABASE_PROVVEDIMENTI").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.NameDBProvvedimenti.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property NameDBICI() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("NOME_DATABASE_ICI") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("NOME_DATABASE_ICI").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.NameDBICI.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property namedbopengov() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("NOME_DATABASE_OPENGOV") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("NOME_DATABASE_OPENGOV").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.namedbopengov.errore: ", ex)
                Return ""
            End Try
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
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.StringConnectionOPENgov.errore: ", ex)
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
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.StringConnectionAnagrafica.errore: ", ex)
                Return ""
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
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.StringConnectionICI.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property StringConnectionTARSU() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("connectionStringOpenGovTIA") Is Nothing) Then
                    If (ConfigurationManager.AppSettings("connectionStringOpenGovTARSU") Is Nothing) Then
                        Return ""
                    Else
                        Return ConfigurationManager.AppSettings("connectionStringOpenGovTARSU")
                    End If
                Else
                    Return ConfigurationManager.AppSettings("connectionStringOpenGovTIA")
                End If

            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.StringConnectionTARSU.errore: ", ex)
                Return ""
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
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.StringConnectionOSAP.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property StringConnectionCatasto() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("sqlAccessCatasto") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("sqlAccessCatasto")
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.StringConnectionCatasto.errore: ", ex)
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
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.ParamentroEnv.errore: ", ex)
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
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.UserName.errore: ", ex)
                Return ConfigurationManager.AppSettings("UserFramework").ToString
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property IdentificativoApplicazione() As String
        Get
            Try
                If (HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") Is Nothing) Then
                    HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") = ConfigurationManager.AppSettings("OPENGOVP").ToString
                    Return ConfigurationManager.AppSettings("OPENGOVP").ToString
                Else
                    Return HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE").ToString
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.IdentificativoApplicazione.errore: ", ex)
                HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") = ConfigurationManager.AppSettings("OPENGOVP").ToString
                Return ConfigurationManager.AppSettings("OPENGOVP").ToString
            End Try
        End Get
    End Property
#End Region
#Region "Url Servizi"
    Public Shared ReadOnly Property UrlMotoreOSAP() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("UrlMotoreTOCO") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("UrlMotoreTOCO").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.UrlMotoreOSAP.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlServizioStampe() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("ServizioStampe") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("ServizioStampe").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.UrlServizioStampe.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property UrlServizioStampeICI() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLElaborazioneDatiStampeICI") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("URLElaborazioneDatiStampeICI").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.UrlServizioStampeICI.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property URLServiziFreezer() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLServiziFreezer") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("URLServiziFreezer").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.URLServiziFreezer.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property URLServiziAccertamenti() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLServiziAccertamenti") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("URLServiziAccertamenti").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.URLServiziAccertamenti.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property URLServiziElaborazioneAtti() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLServiziElaborazioneAtti") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("URLServiziElaborazioneAtti").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.URLServiziElaborazioneAtti.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property URLServiziLiquidazioni() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLServiziLiquidazioni") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("URLServiziLiquidazioni").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.URLServiziLiquidazioni.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property URLGestioneConfigurazione() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("URLGestioneConfigurazione") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("URLGestioneConfigurazione").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.URLGestioneConfigurazione.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
#End Region
#Region "Path"
    Public Shared ReadOnly Property PathStampe() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("DIRTEMPLATE") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("DIRTEMPLATE").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.PathStampe.errore: ", ex)
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
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.PathVirtualStampe.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property PathEstrazione290() As String
        Get
            Try
                If (ConfigurationManager.AppSettings("PATH_ESTRAZIONE290") Is Nothing) Then
                    Return ""
                Else
                    Return ConfigurationManager.AppSettings("PATH_ESTRAZIONE290").ToString()
                End If
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.PathEstrazione290.errore: ", ex)
                Return ""
            End Try
        End Get
    End Property
#End Region
    Public Shared ReadOnly Property AnnoAccertamentoOSAP(ByVal Anno As String) As String
        Get
            Try
                'non da sessione ma da request del menù selezionato per poi memorizzarla in sessione
                If Anno <> "" Then
                    HttpContext.Current.Session("AnnoAccertamentoOSAP") = Anno
                End If

                If (HttpContext.Current.Session("AnnoAccertamentoOSAP") Is Nothing) Then
                    Return DateTime.Now.Year
                Else
                    Return HttpContext.Current.Session("AnnoAccertamentoOSAP").ToString
                End If

            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.ConstSession.AnnoAccertamentoOSAP.errore: ", ex)
                Return DateTime.Now.Year
            End Try
        End Get
    End Property
End Class
