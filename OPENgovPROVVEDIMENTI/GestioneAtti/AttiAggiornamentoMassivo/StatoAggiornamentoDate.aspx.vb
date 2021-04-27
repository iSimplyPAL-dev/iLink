Imports System.Configuration
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports System.IO
Imports System.Xml
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters
Imports System.Collections
Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la visualizzazione stato gestione massiva delle date.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class StatoAggiornamentoDate
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(StatoAggiornamentoDate))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim lngResult As Long
        Dim objHashTable As Hashtable = New Hashtable
        'Dim objSessione As CreateSessione
        Dim objUtility As New MyUtility

        Dim blnDATACONSEGNA As Boolean
        Dim blnDATANOTIFICA As Boolean
        Dim blnDATAPERVENUTO As Boolean
        Dim blnAGGIORNAA As Boolean
        Dim blnELIMINA As Boolean
        Dim strDATA_AGGIORNAMENTO As String
        Try
            objUtility = New MyUtility
            objHashTable.Add("CodENTE", ConstSession.IdEnte)
            objHashTable.Add("COD_TRIBUTO", Session("COD_TRIBUTO"))

            Dim objdsSELEZIONE_PROVVEDIMENTI As DataSet = CType(Session("SELEZIONE_PROVVEDIMENTI_MASSIVA"), DataSet)
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            Select Case objUtility.CToStr(Request("Data"))
                Case "1"
                    blnDATACONSEGNA = True
                Case "2"
                    blnDATANOTIFICA = True
                Case "3"
                    blnDATAPERVENUTO = True
            End Select
            Select Case objUtility.CToStr(Request("Aggiorna"))
                Case "1"
                    blnAGGIORNAA = True
                    strDATA_AGGIORNAMENTO = objUtility.CToStr(Request("txtAggiornaIN"))
                Case "2"
                    blnELIMINA = True
            End Select

            objHashTable.Add("AGGIORNA_A", blnAGGIORNAA)
            objHashTable.Add("ELIMINA", blnELIMINA)
            objHashTable.Add("VALORE_DATA_AGGIORNAMENTO", strDATA_AGGIORNAMENTO)

            objHashTable.Add("DATACONSEGNA", blnDATACONSEGNA)
            objHashTable.Add("DATANOTIFICA", blnDATANOTIFICA)
            objHashTable.Add("DATAPERVENUTO", blnDATAPERVENUTO)


            Dim objCOMAggiornamentoMassivo As IElaborazioneAtti =
            Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)

            lngResult = objCOMAggiornamentoMassivo.setDATE_PROVVEDIMENTI_MASSIVA(ConstSession.StringConnection, objHashTable, objdsSELEZIONE_PROVVEDIMENTI)

            lblImpTotAvvisiAggironati.Text = FormatNumber(lngResult, 0)

            Dim sScript As String = ""
            sScript += "parent.attesaGestioneAtti_popup.style.display='none';"
            sScript += "parent.document.getElementById('txtSALVATAGGIOINCORSO').value='-1';"
            sScript += "ElaborazioneIncorso_1.style.display='';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.StatoAggiornamentoDate.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim lngResult As Long
    '    Dim objHashTable As Hashtable = New Hashtable
    '    'Dim objSessione As CreateSessione
    '    Dim objUtility As New MyUtility

    '    Dim blnDATACONSEGNA As Boolean
    '    Dim blnDATANOTIFICA As Boolean
    '    Dim blnDATAPERVENUTO As Boolean
    '    Dim blnAGGIORNAA As Boolean
    '    Dim blnELIMINA As Boolean
    '    Dim strDATA_AGGIORNAMENTO As String
    '    Try
    '        objUtility = New MyUtility

    '        objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '        objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '        objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '        objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '        objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '        objHashTable.Add("COD_TRIBUTO", Session("COD_TRIBUTO"))

    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

    '        Dim objdsSELEZIONE_PROVVEDIMENTI As DataSet = CType(Session("SELEZIONE_PROVVEDIMENTI_MASSIVA"), DataSet)


    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        '*******************************************************************
    '        '*******************************************************************
    '        Select Case objUtility.CToStr(Request("Data"))
    '            Case "1"
    '                blnDATACONSEGNA = True
    '            Case "2"
    '                blnDATANOTIFICA = True
    '            Case "3"
    '                blnDATAPERVENUTO = True
    '        End Select
    '        '********************************************************************
    '        '********************************************************************
    '        Select Case objUtility.CToStr(Request("Aggiorna"))
    '            Case "1"
    '                blnAGGIORNAA = True
    '                strDATA_AGGIORNAMENTO = objUtility.CToStr(Request("txtAggiornaIN"))
    '            Case "2"
    '                blnELIMINA = True
    '        End Select

    '        objHashTable.Add("AGGIORNA_A", blnAGGIORNAA)
    '        objHashTable.Add("ELIMINA", blnELIMINA)
    '        objHashTable.Add("VALORE_DATA_AGGIORNAMENTO", strDATA_AGGIORNAMENTO)

    '        objHashTable.Add("DATACONSEGNA", blnDATACONSEGNA)
    '        objHashTable.Add("DATANOTIFICA", blnDATANOTIFICA)
    '        objHashTable.Add("DATAPERVENUTO", blnDATAPERVENUTO)


    '        Dim objCOMAggiornamentoMassivo As IElaborazioneAtti =
    '        Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)

    '        lngResult = objCOMAggiornamentoMassivo.setDATE_PROVVEDIMENTI_MASSIVA(objHashTable, objdsSELEZIONE_PROVVEDIMENTI)

    '        lblImpTotAvvisiAggironati.Text = FormatNumber(lngResult, 0)

    '        dim sScript as string=""
    '        sScript += "parent.attesaGestioneAtti_popup.style.display='none';"
    '        sScript += "parent.formRicercaAnagrafica.txtSALVATAGGIOINCORSO.value='-1';"
    '        sScript += "ElaborazioneIncorso_1.style.display='';"
    '        RegisterScript(sScript, Me.GetType())
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.StatoAggiornamentoDate.Page_Load.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="vInput"></param>
    ''' <returns></returns>
    Public Function CToBool(ByRef vInput As Object) As Boolean

        CToBool = False
        Try
            If Not IsDBNull(vInput) And Not IsNothing(vInput) Then
                CToBool = Convert.ToBoolean(vInput)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.StatoAggiornamentoDate.CToBool.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Function
End Class
