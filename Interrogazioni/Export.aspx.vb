Imports log4net
''' <summary>
''' 
''' </summary>
Public Class Export
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Export))

    Private Sub Export_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            lblTitolo.Text = COSTANTValue.ConstSession.DescrizioneEnte
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Interrogazioni.Export.Export_Init.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim EnteExportInCorso As String = clsExport.CacheManager.GetExportInCorso
            If EnteExportInCorso <> "" And EnteExportInCorso <> "-1" Then
                    ShowExportInCorso()
                Else
                Dim listFiles As ArrayList = New clsExport().GetRiepilogoExport(COSTANTValue.ConstSession.IdEnte, COSTANTValue.ConstSession.GetRepository + COSTANTValue.ConstSession.GetRepositoryExport + "\" + COSTANTValue.ConstSession.IdEnte + "\")
                For Each myFile As String In listFiles

                Next
                VisualizzaRiepilogo()
            End If
            'End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Interrogazioni.Export.Page_Load.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    Protected Sub CmdExtract_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim fncExp As New clsExport
        Dim myDBConn As New Utility.DBUtility.objDBConnection

        Try
            clsExport.CacheManager.RemoveAvanzamentoExport()
            myDBConn.TypeDB = COSTANTValue.ConstSession.DBType
            myDBConn.StringConnection = COSTANTValue.ConstSession.StringConnectionOPENgov
            myDBConn.StringConnectionAnag = COSTANTValue.ConstSession.StringConnectionAnagrafica
            myDBConn.StringConnectionICI = COSTANTValue.ConstSession.StringConnectionICI
            myDBConn.StringConnectionTARSU = COSTANTValue.ConstSession.StringConnectionTARSU
            myDBConn.StringConnectionOSAP = COSTANTValue.ConstSession.StringConnectionOSAP
            myDBConn.StringConnectionH2O = COSTANTValue.ConstSession.StringConnectionH2O
            myDBConn.StringConnectionProvv = COSTANTValue.ConstSession.StringConnectionPROVVEDIMENTI
            ShowExportInCorso()
            fncExp.StartExport(myDBConn, COSTANTValue.ConstSession.ApplicationsEnabled, COSTANTValue.ConstSession.Ambiente, COSTANTValue.ConstSession.IdEnte, COSTANTValue.ConstSession.IsFromVariabile, COSTANTValue.ConstSession.HasNotifiche, COSTANTValue.ConstSession.UserName, COSTANTValue.ConstSession.GetRepository + COSTANTValue.ConstSession.GetRepositoryExport + "\" + COSTANTValue.ConstSession.IdEnte + "\")
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Interrogazioni.Export.CmdExtract_Click.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub ShowExportInCorso()
        Dim sScript As String = ""
        Dim RandomGenerator As New System.Random
        Try
            If Not clsExport.CacheManager.GetAvanzamentoExport Is Nothing Then
                Dim myRandomVal As String = RandomGenerator.Next(clsExport.CacheManager.GetAvanzamentoExport(1), 101).ToString
                sScript = "$('#myExpProgress').html('"
                sScript += "<p>Estrazione Banca Dati in corso...</p>"
                sScript += "<div id=\'myProgress\'>"
                sScript += "<div id=\'myBarTot\' style=\'width:" + clsExport.CacheManager.GetAvanzamentoExport(2) + "%;\'>" + clsExport.CacheManager.GetAvanzamentoExport(2) + "%</div>"
                sScript += "</div><br/>"
                'sScript += "<div id=\'myBarSingleFile\' style=\'width:" + clsExport.CacheManager.GetAvanzamentoExport(1) + "%;\'>" + clsExport.CacheManager.GetAvanzamentoExport(1) + "%</div>"
                sScript += "<p>Estrazione " + clsExport.CacheManager.GetAvanzamentoExport(0) + " in corso...</p>"
                sScript += "<div id=\'myProgress\'>"
                sScript += "<div id=\'myBarSingleFile\' style=\'width:" + myRandomVal + "%;\'>" + myRandomVal + "%</div>"
                sScript += "</div>"
                sScript += "');"
                RegisterScript(sScript, Me.GetType())
            Else
                sScript = "$('#myExpProgress').html('"
                sScript += "<p>Estrazione Banca Dati in corso...</p>"
                sScript += "<div id=\'myProgress\'>"
                sScript += "<div id=\'myBarTot\' style=\'width:1%;\'>1%</div>"
                sScript += "</div><br/>"
                sScript += "<p>Estrazione in corso...</p>"
                sScript += "<div id=\'myProgress\'>"
                sScript += "<div id=\'myBarSingleFile\' style=\'width:0%;\'>0%</div>"
                sScript += "</div>"
                sScript += "');"
                RegisterScript(sScript, Me.GetType())
            End If
            Response.AppendHeader("refresh", "2")
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Interrogazioni.Export.ShowExportInCorso.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub VisualizzaRiepilogo()
        Try
            'carico lista completa dai files
            Dim myDir As New IO.DirectoryInfo(COSTANTValue.ConstSession.GetRepository + COSTANTValue.ConstSession.GetRepositoryExport + "\" + COSTANTValue.ConstSession.IdEnte + "\")
            Dim ListFiles As IO.FileInfo() = myDir.GetFiles()
            Dim ListExtract As New ArrayList
            Dim sScript As String = ""
            'ordino per nome
            Array.Sort(ListFiles, New Utility.Comparatore(New String() {"FullName"}, New Boolean() {Utility.TipoOrdinamento.Crescente}))
            For Each myFile As IO.FileInfo In ListFiles
                If myFile.Extension.ToLower = ".zip" Then
                    ListExtract.Add(myFile.Name)
                End If
            Next

            Try
                If ListExtract.Count > 0 Then
                    sScript += "$('#myRecap').append('<h2 class=\'Legend\'>Banche dati da scaricare</h2><ul>"
                    For Each myFile As String In ListExtract
                        sScript += "<li>"
                        sScript += "<i class=\'fa fa-download\'></i>"
                        sScript += "<a href=\'../" + COSTANTValue.ConstSession.GetRepositoryExport + "/" + COSTANTValue.ConstSession.IdEnte + "/" + myFile + "\' download=\'" + myFile + "\'>" + myFile + "</a>"
                        sScript += "<button id=\'btnDownload" + myFile.Replace(".zip", "") + "\'><i class=\'fa fa-file-archive-o\'></i> Scarica il file</button>"
                        sScript += "</li>"
                    Next
                    sScript += "</ul>');"
                    For Each myFile As String In ListExtract
                        sScript += "$('#btnDownload" + myFile.Replace(".zip", "") + "').click(function(){window.open('../" + COSTANTValue.ConstSession.GetRepositoryExport + "/" + COSTANTValue.ConstSession.IdEnte + "/" + myFile + "');});"
                    Next
                End If
                RegisterScript(sScript, Me.GetType)
            Catch ex As Exception
                Log.Debug("OPENgov.LeggiParametriEnte.LoadForm.errore: ", ex)
            End Try
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Interrogazioni.Export.VisualizzaRiepilogo.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
End Class