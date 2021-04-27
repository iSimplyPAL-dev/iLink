Imports log4net
''' <summary>
''' Pagina per la consultazione incrociata tra banche dati.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Public Class ModuloTributi
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ModuloTributi))
    'Private GestErrore As New VisualizzaErrore
    Protected srcComandi As String
    Protected srcVisualizza As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim FncLogEnte As New SelectEnti
            Dim FncLog As New UtilityOPENgov
            Dim myUser As String = ""
            Dim sBelfiore As String = ""
            Dim sTributo As String = ""
            Dim sRifCat As String = ""
            Dim PathPage As String = ""
            Dim PageVisual As String = "ErrRichiamo.aspx"
            Dim PageCom As String = "../aspVuota.aspx"

            If Not Request.Item("LogName") Is Nothing Then
                myUser = Request.Item("LogName").ToString
            Else
                myUser = COSTANTValue.ConstSession.UserName
            End If
            If Not Request.Item("Ente") Is Nothing Then
                sBelfiore = Request.Item("Ente").ToString
            End If
            If Not Request.Item("Tributo") Is Nothing Then
                sTributo = Request.Item("Tributo").ToString
            End If
            If Not Request.Item("RifCat") Is Nothing Then
                sRifCat = Request.Item("RifCat").ToString
            End If
            If FncLog.IsValidUser(myUser, "") Then
                If sBelfiore <> "" And sRifCat.Split(CChar("-")).Length = 2 Then
                    If FncLogEnte.LoginEnte("", sBelfiore) = 1 Then
                        Select Case sTributo
                            Case Utility.Costanti.TRIBUTO_ICI
                                PathPage = Application("nome_sito").ToString & ConfigurationManager.AppSettings("PATH_OPENGOVI") & "/" 'Application("nome_sito").tostring & 
                                PageVisual = "Gestione.aspx?Org=GIS&RifCat=" & sRifCat
                                PageCom = "CGestione.aspx"
                            Case Utility.Costanti.TRIBUTO_TARSU
                                PathPage = Application("nome_sito").ToString & ConfigurationManager.AppSettings("PATH_OPENGOVTA") & "/" '
                                PageVisual = "Dichiarazioni/RicercaDichiarazione.aspx?Org=GIS&RifCat=" & sRifCat
                                PageCom = "Dichiarazioni/ComandiRicDichiarazione.aspx"
                            Case Utility.Costanti.TRIBUTO_H2O
                                PathPage = Application("nome_sito").ToString & ConfigurationManager.AppSettings("PATH_OPENGOVH2O") & "/" 'Application("nome_sito").tostring & 
                                PageVisual = "DataEntryContatori/RicercaContatori.aspx?Org=GIS&RifCat=" & sRifCat
                                PageCom = "DataEntryContatori/ComandiRicercaContatori.aspx"
                        End Select
                    End If
                End If
                Log.Debug("VISUALIZZA::" & PathPage & PageVisual)
                Log.Debug("COMANDI::" & PathPage & PageCom)
                'ifrmVisualizza.Attributes.Add("src", PathPage & PageVisual)
                'ifrmComandi.Attributes.Add("src", PathPage & PageCom)
                'sScript += "ifrmVisualizza.location.href='" & PathPage & PageVisual & "';" & vbCrLf
                'sScript += "ifrmComandi.location.href='" & PathPage & PageCom & "';"
                'ClientScript.RegisterStartupScript(Me.GetType(),"loadifrm", "<script language='javascript'>" & sScript & "</script>")
                'sScript = "frames.item('Visualizza').src='" & PathPage & PageVisual & "';" & vbCrLf
                'sScript += "frames.item('Comandi').src='" & PathPage & PageCom & "';"
                'ClientScript.RegisterStartupScript(Me.GetType(),"loadfrm", "<script language='javascript'>" & sScript & "</script>")
                srcComandi = PathPage & PageCom
                srcVisualizza = PathPage & PageVisual
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ModuloTributi.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class