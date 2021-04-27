Imports System.Xml
Imports log4net

Public Class TabellaSottoAttivita

    Private Shared Log As ILog = LogManager.GetLogger(GetType(TabellaSottoAttivita))
    Public Function CreaTabella(ByRef tabella As Object, ByVal XMLSubActivity As String, ByVal AttualeSA As Integer, ByRef sErrore As String) As Boolean
        Dim oXml As New XmlDocument()
        Dim indiceXML As Integer
        Dim ID_SOTTOATTIVITA As String
        Dim ID_ELEMENTO As String
        Dim DESC_SOTTOATTIVITA As String
        Try
            Try
                oXml.Load(XMLSubActivity)
            Catch myerr As Exception

                sErrore = "<font color=blue size=2>"
                sErrore = sErrore & myerr.ToString & "<br>"
                sErrore = sErrore & "Errore caricamento XML:<br>"
                sErrore = sErrore & XMLSubActivity
                sErrore = sErrore & "</font>"
                CreaTabella = False
                Exit Function
            End Try
            'Indica il Numero delle sotto attivita
            Dim nRecord = oXml.SelectSingleNode("//NRecord").InnerText

            Dim TDwidth As Integer
            TDwidth = 100 / nRecord

            Dim tr As New TableRow()
            Dim tc As New TableCell()
            For indiceXML = 1 To nRecord
                tc = New TableCell()
                ID_SOTTOATTIVITA = oXml.SelectSingleNode("//GetSubActivity[@ID='" & indiceXML & "']/ID_SOTTOATTIVITA").InnerText
                ID_ELEMENTO = oXml.SelectSingleNode("//GetSubActivity[@ID='" & indiceXML & "']/ID_ELEMENTO").InnerText
                DESC_SOTTOATTIVITA = oXml.SelectSingleNode("//GetSubActivity[@ID='" & indiceXML & "']/DESC_SOTTOATTIVITA").InnerText
                tc.Attributes("width") = TDwidth & "%"
                tc.Attributes("align") = "center"
                If AttualeSA = ID_SOTTOATTIVITA Then
                    tc.Attributes("bgColor") = "#FFFF80"
                Else
                    tc.Attributes("style") = "cursor:hand"
                    tc.Attributes("bgColor") = "#FFFFC0"
                    tc.Attributes("title") = "Vai alla SottoAttività " & DESC_SOTTOATTIVITA
                    tc.Attributes("onclick") = "CaricaSottoAttivita('" & ID_SOTTOATTIVITA & "','" & ID_ELEMENTO & "','" & DESC_SOTTOATTIVITA & "');"

                End If
                tc.Text = DESC_SOTTOATTIVITA
                tc.Attributes("class") = "NormalBold"
                tr.Cells.Add(tc)
            Next
            tr.Attributes("heigth") = "20"

            tabella.Rows.Add(tr)

            tr = New TableRow()
            tc = New TableCell()
            tc.Attributes("colspan") = nRecord
            tc.Attributes("bgColor") = "#FFFF80"
            tr.Cells.Add(tc)
            tr.Attributes("heigth") = "10"
            tabella.Rows.Add(tr)
            tabella.Attributes("width") = "100%"
            tabella.GridLines = GridLines.Both

            CreaTabella = True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TabellaSottoAttivita.CreaTabella.errore: ", ex)
        End Try

    End Function

    Public Function CreaBottoni(ByRef btnTemp As Object, ByVal XMLSubActivity As String, ByVal AttualeSA As Integer, ByRef sErrore As String) As Boolean

        Dim oXml As New XmlDocument()
        Dim indiceXML As Integer
        Dim ID_SOTTOATTIVITA As String
        Dim ID_ELEMENTO As String
        Dim DESC_SOTTOATTIVITA As String
        Try
            Try
                oXml.Load(XMLSubActivity)
            Catch exError As Exception

                sErrore = "<font color=blue size=2>"
                sErrore = sErrore & exError.ToString & "<br>"
                sErrore = sErrore & "Errore caricamento XML:<br>"
                sErrore = sErrore & XMLSubActivity
                sErrore = sErrore & "</font>"

                CreaBottoni = False

                Exit Function

            End Try
            'Indica il Numero delle sotto attivita
            Dim nRecord = oXml.SelectSingleNode("//NRecord").InnerText

            btnTemp.Visible = True

            For indiceXML = 1 To nRecord

                ID_SOTTOATTIVITA = oXml.SelectSingleNode("//GetSubActivity[@ID='" & indiceXML & "']/ID_SOTTOATTIVITA").InnerText

                If AttualeSA = ID_SOTTOATTIVITA Then

                    If indiceXML <> nRecord Then
                        ID_SOTTOATTIVITA = oXml.SelectSingleNode("//GetSubActivity[@ID='" & indiceXML + 1 & "']/ID_SOTTOATTIVITA").InnerText
                        ID_ELEMENTO = oXml.SelectSingleNode("//GetSubActivity[@ID='" & indiceXML + 1 & "']/ID_ELEMENTO").InnerText
                        DESC_SOTTOATTIVITA = oXml.SelectSingleNode("//GetSubActivity[@ID='" & indiceXML + 1 & "']/DESC_SOTTOATTIVITA").InnerText
                        btnTemp.Attributes("title") = "Vai alla SottoAttività " & DESC_SOTTOATTIVITA
                        btnTemp.Attributes("onclick") = "return CaricaSottoAttivitaButton('" & ID_SOTTOATTIVITA & "','" & ID_ELEMENTO & "','" & DESC_SOTTOATTIVITA & "');"
                    Else
                        btnTemp.Visible = False
                    End If

                End If

            Next

            CreaBottoni = True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TabellaSottoAttivita.CreaBottoni.errore: ", ex)
        End Try
    End Function


End Class
