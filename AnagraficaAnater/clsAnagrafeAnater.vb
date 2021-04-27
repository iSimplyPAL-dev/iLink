Imports System.Web.HttpContext
Imports OPENUtility
Imports System.String
Imports log4net

Public Class clsAnagrafeAnater
    Private Shared Log As ILog = LogManager.GetLogger(GetType(clsAnagrafeAnater))
    Public Structure DettaglioEnte
        Dim COMUNE As String
        Dim CAP As String
        Dim CODISTAT As String
        Dim CODBELFIORE As String
        Dim PROV As String
    End Structure

    Public Function GetEnteByCodComune(ByVal CodComune As String) As DettaglioEnte
        'Dim WFErrore As String
        'Dim WFSessione As CreateSessione
        dim sSQL as string
        Dim cmdMyCommand As SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim adoRecEnteUtente As New DataSet

        Try
            'WFSessione = New CreateSessione(HttpContext.Current.Session("PARAMETROENV"), HttpContext.Current.Session("username"), ConfigurationManager.AppSettings("OPENGOVG"))
            'If Not WFSessione.CreaSessione(HttpContext.Current.Session("username"), WFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If

            ' il dataset dei comuni ha i seguenti campi
            sSQL = "select * from comuni where codice_istat = '" & Right("000000" & CodComune, 6) & "'"
            'adoRecEnteUtente = WFSessione.oSession.oAppDB.GetPrivateDataSet(sSQL)

            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov) 'WFSessioneAnagrafica.oSession.oAppDB.GetConnection()
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(adoRecEnteUtente, "Create DataView")

            Dim objDettaglioEnte As DettaglioEnte

            objDettaglioEnte.CAP = ""
            objDettaglioEnte.CODBELFIORE = ""
            objDettaglioEnte.CODISTAT = ""
            objDettaglioEnte.COMUNE = ""
            objDettaglioEnte.PROV = ""

            If Not adoRecEnteUtente Is Nothing Then
                If adoRecEnteUtente.Tables(0).Rows.Count > 0 Then
                    objDettaglioEnte.CAP = adoRecEnteUtente.Tables(0).Rows(0)("CAP").ToString
                    objDettaglioEnte.CODBELFIORE = adoRecEnteUtente.Tables(0).Rows(0)("IDENTIFICATIVO").ToString
                    objDettaglioEnte.CODISTAT = adoRecEnteUtente.Tables(0).Rows(0)("CODICE_ISTAT").ToString
                    objDettaglioEnte.COMUNE = adoRecEnteUtente.Tables(0).Rows(0)("COMUNE").ToString
                    objDettaglioEnte.PROV = adoRecEnteUtente.Tables(0).Rows(0)("PV").ToString
                End If
            End If

            Return objDettaglioEnte
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.clsAnagrafeAnater.GetEnteByCodComune.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetEnteByCodComune " + ex.Message)
        Finally
            'If Not WFSessione.oSession Is Nothing Then
            '    WFSessione.Kill()
            'End If
        End Try
    End Function
End Class
