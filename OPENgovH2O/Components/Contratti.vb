Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Public Class DetailsContratti

    Public dsTipoUtenza As DataSet
    Public dsDiametroContatore As DataSet
    Public dsDiametroPresa As DataSet

    Public drDiametroContatore As SqlDataReader
    Public drDiametroPresa As SqlDataReader

    Public lngIdContratto As Long
    Public lngTipoUtenza As Long
    Public lngDiametroPresa As Long
    Public lngDiametroContatore As Long
    Public lngVirtualIDContratto As Long

    Public CodiceContratto As String
    Public NumeroUtenze As String
    Public ConsumoMinimo As String
    Public DataSottoscrizione As String

End Class

Public Class DBContratti

    Dim DBAccess As New DBAccess.getDBobject
    Dim _Const As New Costanti
    Dim ModDate As New ClsGenerale.Generale

    Private Shared Log As ILog = LogManager.GetLogger(GetType(DBContratti))

    Enum DBOperation
        DB_INSERT = 1
        DB_UPDATE = 0
    End Enum

    Public Function GetDetailsContratti(ByVal CodContratto As Integer) As DetailsContratti
        Dim DetailsContratti As New DetailsContratti
        Try
            Dim lgnTipoOperazione As Long = DBOperation.DB_UPDATE

            If CodContratto = -1 Then lgnTipoOperazione = DBOperation.DB_INSERT

            'Caricamento DropDownList 
            DetailsContratti.dsDiametroContatore = DBAccess.RunSPReturnDataSet("sp_DiametroContatore", "TP_DIAMETROCONTATORE")
            DetailsContratti.dsTipoUtenza = DBAccess.RunSPReturnDataSet("sp_TipiUtenza", "TP_TIPIUTENZA", New SqlParameter("@COD_ENTE", ConstSession.IdEnte))
            DetailsContratti.dsDiametroPresa = DBAccess.RunSPReturnDataSet("sp_DiametroPresa", "TP_DIAMETROPRESA")
            DetailsContratti.drDiametroContatore = DBAccess.GetDataReader("Select CODDIAMETROCONTATORE,DIAMETROCONTATORE,DESCRIZIONE  From TP_DIAMETROCONTATORE ")
            DetailsContratti.drDiametroPresa = DBAccess.GetDataReader("Select CODDIAMETROPRESA,DIAMETROPRESA,DESCRIZIONE  From TP_DIAMETROPRESA")

            If lgnTipoOperazione = DBOperation.DB_UPDATE Then
                Dim drDetailsContratto As SqlDataReader
                drDetailsContratto = DBAccess.RunSPReturnRS("DetailContratti", New SqlParameter("@CodContratto", CodContratto))
                If drDetailsContratto.Read Then
                    DetailsContratti.lngIdContratto = MyUtility.CIdFromDB(drDetailsContratto("CODCONTRATTO"))
                    DetailsContratti.lngTipoUtenza = MyUtility.CIdFromDB(drDetailsContratto("IDTIPOUTENZA"))
                    DetailsContratti.lngDiametroContatore = MyUtility.CIdFromDB(drDetailsContratto("CODDIAMETROCONTATORE"))
                    DetailsContratti.lngDiametroPresa = MyUtility.CIdFromDB(drDetailsContratto("CODDIAMETROPRESA"))
                    DetailsContratti.lngVirtualIDContratto = MyUtility.CIdFromDB(drDetailsContratto("CODCONTRATTO"))
                    DetailsContratti.CodiceContratto = Utility.StringOperation.FormatString(drDetailsContratto("CODICECONTRATTO"))
                    DetailsContratti.ConsumoMinimo = Utility.StringOperation.FormatString(drDetailsContratto("CONSUMOMINIMO"))
                    DetailsContratti.DataSottoscrizione = ModDate.GiraDataFromDB(Utility.StringOperation.FormatString(drDetailsContratto("DATASOTTOSCRIZIONE")))
                    DetailsContratti.NumeroUtenze = Utility.StringOperation.FormatString(drDetailsContratto("NUMEROUTENZE"))
                End If
                drDetailsContratto.Close()
            ElseIf lgnTipoOperazione = DBOperation.DB_INSERT Then
                DetailsContratti.lngIdContratto = -1
                DetailsContratti.lngTipoUtenza = -1
                DetailsContratti.lngDiametroContatore = -1
                DetailsContratti.lngDiametroPresa = -1
                DetailsContratti.lngVirtualIDContratto = -1
                DetailsContratti.CodiceContratto = ""
                DetailsContratti.ConsumoMinimo = ""
                DetailsContratti.DataSottoscrizione = ""
                DetailsContratti.NumeroUtenze = ""
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBContratti.GetDetailsContratti.errore: ", ex)
        End Try
        Return DetailsContratti
    End Function

End Class
