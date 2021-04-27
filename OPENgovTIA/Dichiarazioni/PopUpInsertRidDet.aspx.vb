Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la gestione delle riduzioni/esenzioni.
''' Contiene i dati di dettaglio e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class PopUpInsertRidDet
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(PopUpInsertRidDet))

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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim FncCodDescr As New GestCodDescr
        Dim oListCodDescr() As ObjCodDescr
        Dim oMyCodDescr As New ObjCodDescr

        Try
            If Page.IsPostBack = False Then
                If ConstSession.IsFromTARES = "1" Then
                    info.InnerText = "TARI "
                Else
                    info.InnerText = "TARSU "
                End If
                If ConstSession.IsFromVariabile = "1" Then
                    info.InnerText += " Variabile "
                End If
                info.InnerText += info.InnerText
                'controllo cosa caricare
                If Request.Item("sTypeShow") = ObjCodDescr.TIPO_RIDUZIONI Then
                    LblIntestazione.Text = ObjRidEse.TIPO_RIDUZIONI
                ElseIf Request.Item("sTypeShow") = ObjCodDescr.TIPO_ESENZIONI Then
                    LblIntestazione.Text = ObjRidEse.TIPO_ESENZIONI
                End If
                'carico le riduzioni
                oMyCodDescr.IdEnte = ConstSession.IdEnte
                oListCodDescr = FncCodDescr.GetCodDescr(ConstSession.StringConnection, oMyCodDescr, Request.Item("sTypeShow"), Utility.StringOperation.FormatString(Request.Item("Anno")))
                If Not IsNothing(oListCodDescr) Then
                    GrdRisultati.DataSource = oListCodDescr
                    GrdRisultati.DataBind()
                    LblResult.Visible = False
                Else
                    LblResult.Visible = True
                End If
                GrdRisultati.Style.Add("display", "")
                Session("oListCodDescr") = oListCodDescr
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.PopUpInsertRidDet.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            'WFSessione.Kill()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalva.Click
        Dim sScript As String
        Try
            'aggiorno la pagina chiamante
            '*** 20120917 - sgravi ***
            If Request.Item("Provenienza") = "S" Then
                sScript = "opener.parent.Visualizza.document.getElementById('CmdSalvaRidEse').click();"
            ElseIf Request.Item("Provenienza") = "T" Then
                sScript = "opener.parent.Visualizza.document.getElementById('CmdSalvaRidEse').click();"
            Else
                sScript = "opener.parent.Visualizza.document.getElementById('CmdSalvaVani').click();"
            End If
            '*** ***
            sScript += "window.close()"
            RegisterScript( sScript,Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.PopUpInsertRidDet.CmdSalva_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowBind" Then
                For Each myRow As GridViewRow In GrdRisultati.Rows
                    If IDRow = myRow.Cells(0).Text Then
                        Dim oMyAgevolazioni As ObjRidEseApplicati
                        Dim oListAgevolazioni() As ObjRidEseApplicati = Nothing
                        Dim nList As Integer = 0

                        If Request.Item("sTypeShow") = ObjCodDescr.TIPO_RIDUZIONI Then
                            'carico il dataset delle riduzioni
                            If Not Session("oDatiRid") Is Nothing Then
                                'carico l'array originario
                                oListAgevolazioni = Session("oDatiRid")
                            End If
                        Else
                            'carico il dataset delle detassazioni
                            If Not Session("oDatiDet") Is Nothing Then
                                'carico l'array originario
                                oListAgevolazioni = Session("oDatiDet")
                            End If
                        End If
                        If Not IsNothing(oListAgevolazioni) Then
                            'controllo che la riduzione selezionata non sia già presente
                            For nList = 0 To oListAgevolazioni.GetUpperBound(0)
                                If oListAgevolazioni(nList).sCodice = myRow.Cells(0).Text Then
                                    'evidenzio la riga selezionata
                                    'GrdRisultati.SelectedItem.Attributes.Add("style", "CartListItemSel")
                                    Exit Sub
                                End If
                            Next
                            'aggiungo una riga
                            nList = oListAgevolazioni.GetUpperBound(0) + 1
                        End If
                        'dimensiono l'array
                        ReDim Preserve oListAgevolazioni(nList)
                        'carico i dati dal form
                        oMyAgevolazioni = New ObjRidEseApplicati
                        oMyAgevolazioni.sTipoValore = Request.Item("sTypeShow")
                        '*** 20120917 - sgravi ***
                        If Request.Item("Provenienza") = "S" Then
                            oMyAgevolazioni.Riferimento = ObjRidEseApplicati.RIF_ARTICOLO
                        ElseIf Request.Item("Provenienza") = "T" Then
                            oMyAgevolazioni.Riferimento = ObjRidEseApplicati.RIF_TESSERA
                        Else
                            oMyAgevolazioni.Riferimento = ObjRidEseApplicati.RIF_UI
                        End If
                        '*** ***
                        oMyAgevolazioni.IdRiferimento = TxtIdRiferimento.Text
                        oMyAgevolazioni.sCodice = myRow.Cells(0).Text
                        oMyAgevolazioni.sDescrizione = myRow.Cells(1).Text
                        oMyAgevolazioni.ID = CType(myRow.FindControl("hfid"), HiddenField).Value
                        'carico l'array
                        oListAgevolazioni(nList) = oMyAgevolazioni
                        'memorizzo l'oggetto nella sessione
                        If Request.Item("Provenienza") = "T" Then
                            If Request.Item("sTypeShow") = ObjCodDescr.TIPO_RIDUZIONI Then
                                Session("oDatiRidTessera") = oListAgevolazioni
                            Else
                                Session("oDatiDetTessera") = oListAgevolazioni
                            End If
                        Else
                            If Request.Item("sTypeShow") = ObjCodDescr.TIPO_RIDUZIONI Then
                                Session("oDatiRid") = oListAgevolazioni
                            Else
                                Session("oDatiDet") = oListAgevolazioni
                            End If
                        End If
                        'aggiorno la pagina chiamante
                        '*** 20120917 - sgravi ***
                        Dim sScript As String
                        If Request.Item("Provenienza") = "S" Then
                            sScript = "opener.parent.Visualizza.document.getElementById('CmdSalvaRidEse').click();"
                        ElseIf Request.Item("Provenienza") = "T" Then
                            sScript = "opener.parent.Visualizza.document.getElementById('CmdSalvaRidEse').click();"
                        Else
                            sScript = "opener.parent.Visualizza.document.getElementById('CmdSalvaVani').click();"
                        End If
                        '*** ***
                        sScript += "window.close()"
                        RegisterScript(sScript, Me.GetType)
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.PopUpInsertRidDet.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdRisultati_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrdRisultati.SelectedIndexChanged
    '    Try
    '        Dim oMyAgevolazioni As ObjRidEseApplicati
    '        Dim oListAgevolazioni() As ObjRidEseApplicati
    '        Dim nList As Integer = 0

    '        If Request.Item("sTypeShow") = ObjCodDescr.TIPO_RIDUZIONI Then
    '            'carico il dataset delle riduzioni
    '            If Not Session("oDatiRid") Is Nothing Then
    '                'carico l'array originario
    '                oListAgevolazioni = Session("oDatiRid")
    '            End If
    '        Else
    '            'carico il dataset delle detassazioni
    '            If Not Session("oDatiDet") Is Nothing Then
    '                'carico l'array originario
    '                oListAgevolazioni = Session("oDatiDet")
    '            End If
    '        End If
    '        If Not IsNothing(oListAgevolazioni) Then
    '            'controllo che la riduzione selezionata non sia già presente
    '            For nList = 0 To oListAgevolazioni.GetUpperBound(0)
    '                If oListAgevolazioni(nList).sCodice = GrdRisultati.Items(GrdRisultati.SelectedIndex).Cells(0).Text Then
    '                    'evidenzio la riga selezionata
    '                    'GrdRisultati.SelectedItem.Attributes.Add("style", "CartListItemSel")
    '                    Exit Sub
    '                End If
    '            Next
    '            'aggiungo una riga
    '            nList = oListAgevolazioni.GetUpperBound(0) + 1
    '        End If
    '        'dimensiono l'array
    '        ReDim Preserve oListAgevolazioni(nList)
    '        'carico i dati dal form
    '        oMyAgevolazioni = New ObjRidEseApplicati
    '        oMyAgevolazioni.sTipoValore = Request.Item("sTypeShow")
    '        '*** 20120917 - sgravi ***
    '        If Request.Item("Provenienza") = "S" Then
    '            oMyAgevolazioni.Riferimento = ObjRidEseApplicati.RIF_ARTICOLO
    '        ElseIf Request.Item("Provenienza") = "T" Then
    '            oMyAgevolazioni.Riferimento = ObjRidEseApplicati.RIF_TESSERA
    '        Else
    '            oMyAgevolazioni.Riferimento = ObjRidEseApplicati.RIF_UI
    '        End If
    '        '*** ***
    '        oMyAgevolazioni.IdRiferimento = TxtIdRiferimento.Text
    '        oMyAgevolazioni.sCodice = GrdRisultati.Items(GrdRisultati.SelectedIndex).Cells(0).Text
    '        oMyAgevolazioni.sDescrizione = GrdRisultati.Items(GrdRisultati.SelectedIndex).Cells(1).Text
    '        oMyAgevolazioni.ID = GrdRisultati.Items(GrdRisultati.SelectedIndex).Cells(3).Text
    '        'carico l'array
    '        oListAgevolazioni(nList) = oMyAgevolazioni
    '        'memorizzo l'oggetto nella sessione
    '        If Request.Item("Provenienza") = "T" Then
    '            If Request.Item("sTypeShow") = ObjCodDescr.TIPO_RIDUZIONI Then
    '                Session("oDatiRidTessera") = oListAgevolazioni
    '            Else
    '                Session("oDatiDetTessera") = oListAgevolazioni
    '            End If
    '        Else
    '            If Request.Item("sTypeShow") = ObjCodDescr.TIPO_RIDUZIONI Then
    '                Session("oDatiRid") = oListAgevolazioni
    '            Else
    '                Session("oDatiDet") = oListAgevolazioni
    '            End If
    '        End If

    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.PopUpInsertRidDet.GrdRisultati_SelectedIndexChanged.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
End Class

