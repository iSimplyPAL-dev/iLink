Imports System.Data
Imports System.Diagnostics
Imports System.Xml
Imports System.Text
Imports log4net

Public Class DataSetForGrid
    Private FncGen As New ClsGenerale.Generale

    Private Shared Log As ILog = LogManager.GetLogger(GetType(DataSetForGrid))
    Public Shared Sub ElaboraXMLFILE(ByVal strXMLFileName As String)

        Dim xmlDoc As New XmlDocument()
        Dim nodelistcharges As XmlNodeList
        Dim root As XmlNode

        xmlDoc.Load(strXMLFileName)


        Dim strPathMetaData As String = "/GetPendingDocumentsList/GetPendingDocuments"

        root = xmlDoc.DocumentElement
        root.RemoveChild(root.Item("NRecord"))

        nodelistcharges = xmlDoc.SelectNodes(strPathMetaData)
        Try
            Dim i, icount As Integer
            For i = 0 To nodelistcharges.Count - 1
                If nodelistcharges.Item(i).HasChildNodes Then
                    Dim CAMPICHIAVE As XmlNodeList = xmlDoc.SelectNodes("/GetPendingDocumentsList/GetPendingDocuments/XML_CAMPI_CHIAVE")
                    If CAMPICHIAVE.Item(i).HasChildNodes Then
                        For icount = 0 To CAMPICHIAVE.Item(i).ChildNodes.Count - 1
                            Dim elem As XmlElement = xmlDoc.CreateElement(CAMPICHIAVE.Item(i).ChildNodes(icount).Name)
                            elem.InnerText = CAMPICHIAVE.Item(i).ChildNodes(icount).InnerText
                            CAMPICHIAVE.Item(i).ParentNode.AppendChild(elem)
                        Next
                    End If
                End If
            Next

            Dim root1 As XmlElement = xmlDoc.DocumentElement
            Dim elemList As XmlNodeList = root1.SelectNodes("/GetPendingDocumentsList/GetPendingDocuments/XML_CAMPI_CHIAVE")
            For i = 0 To elemList.Count - 1
                elemList.Item(i).RemoveAll()
            Next
            'Salvo il file dopo le Modifiche
            xmlDoc.Save(strXMLFileName)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DataSetForGrid.ElaboraXMLFILE.errore: ", ex)
        End Try
    End Sub

    Public Shared Function EmptyXMLFILE(ByVal strXMLFileName As String) As Boolean
        Try
            Dim xmlDoc As New XmlDocument()
            Dim lngTotalRecord As Long
            EmptyXMLFILE = False

            xmlDoc.Load(strXMLFileName)
            lngTotalRecord = xmlDoc.SelectSingleNode("//NRecord").InnerText
            If lngTotalRecord = 0 Then
                Exit Function
            End If
            EmptyXMLFILE = True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DataSetForGrid.EmptyXMLFILE.errore: ", ex)
        End Try
    End Function

    Public Sub PrepareDataSetFromXML(ByRef _ds As DataSet, _
                                                    ByVal cboTemp As System.Web.UI.WebControls.DropDownList, _
                                                    ByVal WFSession As Object, _
                                                    ByVal strPath As String, _
                                                    ByVal strFilterDoc As String, _
                                                    ByVal strTipoServizio As String)


        Dim xmlDoc As New XmlDocument
        Dim strCboValue As String
        Dim dsTemp As New DataSet
        Dim strXMLFile As String

        Try
            If IsNothing(cboTemp.SelectedItem.Value) Then
                strCboValue = ""
            Else
                strCboValue = cboTemp.SelectedItem.Value.ToString
            End If

            strXMLFile = "\" & WFSession.oOM.GetGlobalPendingDocuments(CStr(strTipoServizio),
                                                   strCboValue, strFilterDoc).XMLFileName


            DataSetForGrid.ElaboraXMLFILE(strPath & strXMLFile)

            Dim fsWriteXml As New System.IO.FileStream _
                    (strPath & strXMLFile, System.IO.FileMode.Open, IO.FileAccess.Read)

            dsTemp.ReadXml(fsWriteXml, XmlReadMode.Auto)
            _ds = dsTemp

            fsWriteXml.Close()

            Dim clsFile As New GestioneFile
            FncGen.DeleteFile(strPath & strXMLFile)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DataSetForGrid.PrepareDataSetFromXML.errore: ", ex)
        End Try

    End Sub
    Public Function RenderDataXML(ByVal cboTemp As System.Web.UI.WebControls.DropDownList, _
                                                    ByVal WFSession As Object, _
                                                    ByVal strPath As String, _
                                                    ByVal strFilterDoc As String, _
                                                    ByVal strTipoServizio As String) As Boolean

        'Verifica se il File XML dei pendenti contiene record prima di passare alla funzione PrepareDataSetFromXML

        Dim strXMLFile As String
        Dim xmlDoc As New XmlDocument
        Dim strCboValue As String

        RenderDataXML = False
        Try
            If IsNothing(cboTemp.SelectedItem.Value) Then
                strCboValue = ""
            Else
                strCboValue = cboTemp.SelectedItem.Value.ToString
            End If

            strXMLFile = "\" & WFSession.oOM.GetGlobalPendingDocuments(CStr(strTipoServizio),
                                                 strCboValue, strFilterDoc).XMLFileName

            'Se ritorna True il file dei pendenti contiene record
            If DataSetForGrid.EmptyXMLFILE(strPath & strXMLFile) Then
                RenderDataXML = True
            End If
            'Cancello il file creato per la Verifica
            Dim clsFile As New GestioneFile
            FncGen.DeleteFile(strPath & strXMLFile)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DataSetForGrid.RenderDataXML.errore: ", ex)
        End Try
    End Function

End Class
