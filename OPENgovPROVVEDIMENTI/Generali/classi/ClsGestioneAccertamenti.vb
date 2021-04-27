Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
'Imports RemotingInterfaceMotoreTarsu
Imports ComPlusInterface
'Imports ComPlusInterface.ProvvedimentiTarsu.Oggetti
'Imports RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu
Imports AnagInterface
Imports System.Web.Caching
Imports Utility
''' <summary>
''' Classe per la gestione e generazione dei un atto di accertamento
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsGestioneAccertamenti
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsGestioneAccertamenti))

#Region "CalcoloAccertamento ICI/TASI"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Anagrafica"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="27/10/2014">
    ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
    ''' Nuova gestione indirizzi spedizione
    ''' </revision>
    ''' </revisionHistory>
    Public Function addRowsObjAnagrafica(ByVal Anagrafica As DettaglioAnagrafica) As DataSet
        Dim row As DataRow
        Dim newTable As DataTable
        Dim objAnagrafica As DataSet = Nothing
        Try
            objAnagrafica = CreateDsPerAnagrafica()
            newTable = objAnagrafica.Tables(0).Copy

            row = newTable.NewRow()

            row.Item("COD_CONTRIBUENTE") = Anagrafica.COD_CONTRIBUENTE
            row.Item("ID_DATA_ANAGRAFICA") = Anagrafica.ID_DATA_ANAGRAFICA
            row.Item("Cognome") = Anagrafica.Cognome
            row.Item("RappresentanteLegale") = Anagrafica.RappresentanteLegale
            row.Item("Nome") = Anagrafica.Nome
            row.Item("CodiceFiscale") = Anagrafica.CodiceFiscale
            row.Item("PartitaIva") = Anagrafica.PartitaIva
            row.Item("CodiceComuneNascita") = Anagrafica.CodiceComuneNascita
            row.Item("ComuneNascita") = Anagrafica.ComuneNascita
            row.Item("ProvinciaNascita") = Anagrafica.ProvinciaNascita
            row.Item("DataNascita") = Anagrafica.DataNascita
            row.Item("NazionalitaNascita") = Anagrafica.NazionalitaNascita
            row.Item("Sesso") = Anagrafica.Sesso
            row.Item("CodiceComuneResidenza") = Anagrafica.CodiceComuneResidenza
            row.Item("ComuneResidenza") = Anagrafica.ComuneResidenza
            row.Item("ProvinciaResidenza") = Anagrafica.ProvinciaResidenza
            row.Item("CapResidenza") = Anagrafica.CapResidenza
            row.Item("CodViaResidenza") = Anagrafica.CodViaResidenza
            row.Item("ViaResidenza") = Anagrafica.ViaResidenza
            row.Item("PosizioneCivicoResidenza") = Anagrafica.PosizioneCivicoResidenza
            row.Item("CivicoResidenza") = Anagrafica.CivicoResidenza
            row.Item("EsponenteCivicoResidenza") = Anagrafica.EsponenteCivicoResidenza
            row.Item("ScalaCivicoResidenza") = Anagrafica.ScalaCivicoResidenza
            row.Item("InternoCivicoResidenza") = Anagrafica.InternoCivicoResidenza
            row.Item("FrazioneResidenza") = Anagrafica.FrazioneResidenza
            row.Item("NazionalitaResidenza") = Anagrafica.NazionalitaResidenza
            row.Item("NucleoFamiliare") = Anagrafica.NucleoFamiliare
            row.Item("DATA_MORTE") = Anagrafica.DataMorte
            row.Item("Professione") = Anagrafica.Professione
            row.Item("Note") = Anagrafica.Note
            row.Item("DaRicontrollare") = Anagrafica.DaRicontrollare
            row.Item("DataInizioValidita") = Anagrafica.DataInizioValidita
            row.Item("DataFineValidita") = Anagrafica.DataFineValidita
            row.Item("DataUltimaModifica") = Anagrafica.DataUltimaModifica
            row.Item("Operatore") = Anagrafica.Operatore
            row.Item("CodContribuenteRappLegale") = Anagrafica.CodContribuenteRappLegale
            row.Item("CodEnte") = Anagrafica.CodEnte
            row.Item("CodIndividuale") = Anagrafica.CodIndividuale
            row.Item("CodFamiglia") = Anagrafica.CodFamiglia
            row.Item("NCTributari") = Anagrafica.NCTributari
            row.Item("DataUltimoAggTributi") = Anagrafica.DataUltimoAggTributi
            row.Item("NCAnagraficaRes") = Anagrafica.NCAnagraficaRes
            row.Item("DataUltimoAggAnagrafe") = Anagrafica.DataUltimoAggAnagrafe
            row.Item("TipoRiferimento") = Anagrafica.TipoRiferimento
            row.Item("DatiRiferimento") = Anagrafica.DatiRiferimento
            '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
            For Each MySped As ObjIndirizziSpedizione In Anagrafica.ListSpedizioni
                If MySped.CodTributo = Utility.Costanti.TRIBUTO_ICI Then
                    row.Item("ID_DATA_SPEDIZIONE") = MySped.ID_DATA_SPEDIZIONE
                    row.Item("CodTributo") = MySped.CodTributo
                    row.Item("CognomeInvio") = MySped.CognomeInvio
                    row.Item("NomeInvio") = MySped.NomeInvio
                    row.Item("CodComuneRCP") = MySped.CodComuneRCP
                    row.Item("ComuneRCP") = MySped.ComuneRCP
                    row.Item("LocRCP") = MySped.LocRCP
                    row.Item("ProvinciaRCP") = MySped.ProvinciaRCP
                    row.Item("CapRCP") = MySped.CapRCP
                    row.Item("CodViaRCP") = MySped.CodViaRCP
                    row.Item("ViaRCP") = MySped.ViaRCP
                    row.Item("PosizioneCivicoRCP") = MySped.PosizioneCivicoRCP
                    row.Item("CivicoRCP") = MySped.CivicoRCP
                    row.Item("EsponenteCivicoRCP") = MySped.EsponenteCivicoRCP
                    row.Item("ScalaCivicoRCP") = MySped.ScalaCivicoRCP
                    row.Item("InternoCivicoRCP") = MySped.InternoCivicoRCP
                    row.Item("FrazioneRCP") = MySped.FrazioneRCP
                    row.Item("DataInizioValiditaSpedizione") = MySped.DataInizioValiditaSpedizione
                    row.Item("DataFineValiditaSpedizione") = MySped.DataFineValiditaSpedizione
                    row.Item("DataUltimaModificaSpedizione") = MySped.DataUltimaModificaSpedizione
                    row.Item("OperatoreSpedizione") = MySped.OperatoreSpedizione
                End If
            Next
            '*** ***
            newTable.Rows.Add(row)
            newTable.AcceptChanges()

            objAnagrafica.Tables(0).ImportRow(row)
            objAnagrafica.Tables(0).AcceptChanges()

            Return objAnagrafica
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.addRowsObjAnagrafica.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Private Function CreateDsPerAnagrafica() As DataSet
        Dim objDS As New DataSet

        Dim newTable As DataTable
        Try
            newTable = New DataTable("ANAGRAFICA")

            Dim NewColumn As New DataColumn
            NewColumn.ColumnName = "COD_CONTRIBUENTE"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ID_DATA_SPEDIZIONE"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ID_DATA_ANAGRAFICA"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "Cognome"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "RappresentanteLegale"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "Nome"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodiceFiscale"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "INDIRIZZO"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "PartitaIva"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodiceComuneNascita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ComuneNascita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ProvinciaNascita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataNascita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "NazionalitaNascita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "Sesso"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodiceComuneResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ComuneResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ProvinciaResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CapResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodViaResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ViaResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "PosizioneCivicoResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CivicoResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "EsponenteCivicoResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ScalaCivicoResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "InternoCivicoResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "FrazioneResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "NazionalitaResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "NucleoFamiliare"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DATA_MORTE"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "Professione"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "Note"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DaRicontrollare"
            NewColumn.DataType = System.Type.GetType("System.Boolean")
            NewColumn.DefaultValue = False
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataInizioValidita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataFineValidita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataUltimaModifica"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "Operatore"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodContribuenteRappLegale"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodEnte"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodIndividuale"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodFamiglia"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "NCTributari"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataUltimoAggTributi"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "NCAnagraficaRes"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataUltimoAggAnagrafe"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodTributo"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CognomeInvio"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "NomeInvio"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodComuneRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ComuneRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "LocRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ProvinciaRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CapRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodViaRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ViaRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "PosizioneCivicoRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CivicoRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "EsponenteCivicoRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ScalaCivicoRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "InternoCivicoRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "FrazioneRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataInizioValiditaSpedizione"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataFineValiditaSpedizione"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataUltimaModificaSpedizione"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "OperatoreSpedizione"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "TipoRiferimento"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DatiRiferimento"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)


            objDS.Tables.Add(newTable)

            Return objDS
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.CreateDsPerAnagrafica.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Function CheckProgLegame(ListUI() As objUIICIAccert) As Boolean
        Dim nProg As Integer

        Try
            If Not ListUI Is Nothing Then
                nProg = 0
                Array.Sort(ListUI, New Utility.Comparatore(New String() {"IdLegame"}, New Boolean() {Utility.TipoOrdinamento.Crescente}))
                For Each myUI As objUIICIAccert In ListUI
                    nProg += 1
                    If myUI.IdLegame <> nProg Then
                        Return False
                    End If
                Next
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.CheckProgLegame.errore: ", ex)
            Return False
        End Try
    End Function
    Public Function ControlloLEGAME(ListDic() As objUIICIAccert, ListAcc() As objUIICIAccert) As Boolean
        Dim Trovato As Boolean = True
        Dim iTrovato As Integer = 0

        Try
            Dim IDLegameDich, IDLegameDich_old As Integer
            IDLegameDich_old = -1
            If Not ListDic Is Nothing Then
                For Each myDic As objUIICIAccert In ListDic
                    Dim i As Integer = 0
                    IDLegameDich = myDic.IdLegame
                    If IDLegameDich <> IDLegameDich_old Then
                        For Each myAcc As objUIICIAccert In ListAcc
                            If myAcc.IdLegame = IDLegameDich Then
                                myAcc.IdImmobileDichiarato = myDic.IdImmobile
                                myAcc.DiffImposta = myAcc.TotDovuto - myDic.TotDovuto
                                iTrovato += 1
                                Exit For
                            End If
                        Next
                    End If
                    IDLegameDich_old = IDLegameDich
                Next

                If iTrovato < ListDic.GetLength(0) Then
                    Trovato = False
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.ControlloLEGAME.errore: ", ex)
        End Try
        Return Trovato
    End Function
    Public Function ControlloLEGAMEdoppioAccertato(ListUI() As objUIICIAccert) As Boolean
        Dim Trovato As Boolean = False
        Try
            'Cerco l'immobile dichiarato in tutti gli immobili in accertato
            'Se lo trovo esco dal ciclo e proseguo a cercare con immobili
            'successivo di accertamento
            If Not ListUI Is Nothing Then
                For Each myUI As objUIICIAccert In ListUI
                    For Each myCheck As objUIICIAccert In ListUI
                        If myCheck.IdLegame = myUI.IdLegame And myCheck.Id <> myUI.Id Then
                            Trovato = True
                            Exit For
                        End If
                    Next
                Next
            End If
            'Se trovato = TRUE vuol dire che ho trovato l'immobile doppio, quindi blocco l'accertamento
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.ControlloLEGAMEdoppioAccertato.errore: ", ex)
        End Try
        Return Trovato
    End Function
    '*** 201810 - Generazione Massiva Atti ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdEnte"></param>
    ''' <param name="Tributo"></param>
    ''' <param name="TributoCalcolo"></param>
    ''' <param name="Anno"></param>
    ''' <param name="IdContribuente"></param>
    ''' <param name="IdProvRettifica"></param>
    ''' <param name="DataElabRettifica"></param>
    ''' <param name="dsDettaglioAnagrafica"></param>
    ''' <param name="ImpDichAcconto"></param>
    ''' <param name="ImpDichSaldo"></param>
    ''' <param name="ImpDichTotale"></param>
    ''' <param name="dsRiepilogoFase2"></param>
    ''' <param name="TotVersamenti"></param>
    ''' <param name="dsSanzioniFase2"></param>
    ''' <param name="ListInteressi"></param>
    ''' <param name="dsVersamentiF2"></param>
    ''' <returns></returns>
    Public Function CalcoloPreAccertamento(IdEnte As String, Tributo As String, TributoCalcolo As String, Anno As String, IdContribuente As Integer, IdProvRettifica As Integer, DataElabRettifica As String, dsDettaglioAnagrafica As DataSet, ImpDichAcconto As Double, ImpDichSaldo As Double, ImpDichTotale As Double, ByRef dsRiepilogoFase2 As ObjBaseIntSanz, ByRef TotVersamenti As Double, ByRef dsSanzioniFase2 As DataSet, ByRef ListInteressi() As ObjInteressiSanzioni, ByRef dsVersamentiF2 As DataSet) As Boolean
        Dim TipoCalcolo As Integer = DichiarazioniICI.CalcoloICI.CalcoloICI.TIPOCalcolo_STANDARD
        Dim myHashTable As New Hashtable
        Dim ListBaseCalcoli() As ObjBaseIntSanz = Nothing
        TotVersamenti = 0

        Try
            Log.Debug(IdEnte & " - calcolo fase preaccertamento")

            myHashTable.Add("ANNODA", Anno)
            myHashTable.Add("ANNOA", Anno)
            myHashTable.Add("CodContribuente", IdContribuente)
            myHashTable.Add("IdProvRett", IdProvRettifica)
            myHashTable.Add("CodENTE", IdEnte)
            myHashTable.Add("COD_TRIBUTO", Tributo)
            myHashTable.Add("TRIBUTOCALCOLO", TributoCalcolo)

            myHashTable.Add("CONNECTIONSTRINGOPENGOV", ConstSession.StringConnectionOPENgov)
            myHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)
            myHashTable.Add("CONNECTIONSTRINGANAGRAFICA", ConstSession.StringConnectionAnagrafica)
            myHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

            myHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
            myHashTable.Add("DA", "")
            myHashTable.Add("A", "")

            myHashTable.Add("DATA_ELABORAZIONE_PER_RETTIFICA", DataElabRettifica)

            Log.Debug(IdEnte & " - richiamo servizio ProcessFase2PerAccertamento")
            Dim blnRetVal As Boolean
            Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConstSession.URLServiziLiquidazioni)
            blnRetVal = objCOM.ProcessFase2(ConstSession.StringConnection, ConstSession.StringConnectionICI, IdEnte, IdContribuente, myHashTable, dsDettaglioAnagrafica, ImpDichAcconto, ImpDichSaldo, ImpDichTotale, "", ListBaseCalcoli, dsSanzioniFase2, ListInteressi, dsRiepilogoFase2, dsVersamentiF2)
            If Not dsVersamentiF2 Is Nothing Then
                If dsVersamentiF2.Tables.Count > 0 Then
                    Log.Debug(IdEnte & " - objDSVersamentiF2 count:" & dsVersamentiF2.Tables(0).Rows.Count)
                    For Each myRow As DataRow In dsVersamentiF2.Tables(0).Rows
                        TotVersamenti += myRow("ImportoPagato")
                    Next
                Else
                    Log.Debug(IdEnte & " - objDSVersamentiF2 è nothing")
                End If
            Else
                Log.Debug(IdEnte & " - objDSVersamentiF2 è nothing")
            End If
            Log.Debug(IdEnte & " - Totale Versamenti:" & TotVersamenti)
            Log.Debug(IdEnte & " - Terminata ProcessFase2PerAccertamento")
            Return True
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.CalcoloPreAccertamento.errore: ", ex)
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objHashTable"></param>
    ''' <param name="IdContribuente"></param>
    ''' <param name="Anno"></param>
    ''' <param name="dsMySanzioni"></param>
    ''' <param name="ListDichiarazioni"></param>
    ''' <param name="TipoAvviso"></param>
    ''' <param name="Fase"></param>
    ''' <param name="sDataMorte"></param>
    ''' <param name="oCalcoloSanzioniInteressi"></param>
    ''' <param name="objDSSanzioni"></param>
    ''' <param name="ListInteressi"></param>
    ''' <param name="objDTAccertato"></param>
    ''' <param name="TotDiffImpostaACCERTAMENTO"></param>
    ''' <param name="TotImpICIDichiarato"></param>
    ''' <param name="TotDiffImpostaDICHIARATO"></param>
    ''' <param name="TotImpICIACCERTAMENTO"></param>
    ''' <param name="TotInteressiACCERTAMENTO"></param>
    ''' <param name="TotSanzioniACCERTAMENTO"></param>
    ''' <param name="TotImportoSanzioniRidottoACCERTAMENTO"></param>
    ''' <param name="TotVersamenti"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory><revision date="12/11/2019">il calcolo interessi 8852/TASI deve essere fatto in acconto/saldo o in unica soluzione in base alla configurazione di TP_GENERALE_ICI</revision></revisionHistory>
    Public Function CalcoloImpSanzIntSingleUI(objHashTable As Hashtable, IdContribuente As Integer, Anno As String, dsMySanzioni As DataSet, ListDichiarazioni() As objUIICIAccert, TipoAvviso As Integer, Fase As Integer, sDataMorte As String, ByRef oCalcoloSanzioniInteressi As ObjBaseIntSanz, ByRef objDSSanzioni As DataSet, ByRef ListInteressi() As ObjInteressiSanzioni, ByRef objDTAccertato() As objUIICIAccert, ByRef TotDiffImpostaACCERTAMENTO As Double, ByRef TotImpICIDichiarato As Double, ByRef TotDiffImpostaDICHIARATO As Double, ByRef TotImpICIACCERTAMENTO As Double, ByRef TotInteressiACCERTAMENTO As Double, ByRef TotSanzioniACCERTAMENTO As Double, ByRef TotImportoSanzioniRidottoACCERTAMENTO As Double, ByRef TotVersamenti As Double) As Boolean
        Dim blnResult As Boolean
        Dim blnCalcolaInteressi As Boolean
        Dim objDSCalcoloSanzioniInteressiAppoggio As DataSet
        Dim TotDiffImposta As Double
        Dim TotDiffImpostaACCONTO As Double
        Dim TotDiffImpostaSALDO As Double
        Dim TotDiffImpostaAccontoDICHIARATO As Double 'Importo Totale Differenza di imposta ACCONTO immobili dichiarati
        Dim TotDiffImpostaSaldoDICHIARATO As Double 'Importo Totale Differenza di imposta SALDO immobili dichiarati
        Dim TotDiffImpostaAccontoACCERTAMENTO As Double 'Importo Totale Differenza di imposta atto ACCONTO di accertamento
        Dim TotDiffImpostaSaldoACCERTAMENTO As Double 'Importo Totale Differenza di imposta SALDO atto di accertamento
        Dim TotImpostaICIDichiaratoIMMOBILE As Double = 0
        Dim TotImpostaICIDichiaratoACCONTOIMMOBILE As Double = 0
        Dim TotImpostaICIDichiaratoSALDOIMMOBILE As Double = 0
        Dim TotImpostaICIAccertato As Double = 0
        Dim TotImpostaICIAccertatoACCONTO As Double = 0
        Dim TotImpostaICIAccertatoSALDO As Double = 0
        Dim ImportoSanzioneImmobile As Double = 0 'sanzioni singolo Immobile
        Dim ImportoSanzioneImmobileRidotto As Double = 0 'sanzioni singolo Immobile
        Dim ImportoInteresseImmobile As Double = 0 'interessi singolo Immobile
        Dim ssanz As String
        Dim intCount As Integer
        Dim copyRow As DataRow
        Dim objDSMotivazioniSanzioni As DataSet
        Dim dtSanzioni As New DataTable
        Dim TipoAvvisoRimborso As Integer
        Dim Trovato As Boolean = False
        Dim TipoProvvedimento As String = TipoAvviso
        Dim ListBaseCalc As New ArrayList

        Try
            '*******************************************************************
            'Calcolo ICI Totale per accertato per ogni legame
            '*******************************************************************
            For Each rowsArray As objUIICIAccert In objDTAccertato
                ListBaseCalc = New ArrayList
                Trovato = False
                If Not ListDichiarazioni Is Nothing Then
                    For Each myUI As objUIICIAccert In ListDichiarazioni
                        If rowsArray.IdLegame = myUI.IdLegame Then
                            AzzeraImporti(TotDiffImposta, TotImpostaICIAccertato, TotImpostaICIAccertatoACCONTO, TotImpostaICIAccertatoSALDO, TotImpostaICIDichiaratoIMMOBILE, TotImpostaICIDichiaratoACCONTOIMMOBILE, TotImpostaICIDichiaratoSALDOIMMOBILE, ImportoSanzioneImmobile, ImportoSanzioneImmobileRidotto, ImportoInteresseImmobile = 0)

                            'Calcolo ICI Totale per dichiarato per ogni gruppo di legame Se non ho dichiarato niente posso andare comunque in accertamento
                            TotImpostaICIDichiaratoIMMOBILE = myUI.TotDovuto + TotImpostaICIDichiaratoIMMOBILE
                            TotImpostaICIDichiaratoACCONTOIMMOBILE = myUI.AccDovuto + TotImpostaICIDichiaratoACCONTOIMMOBILE
                            TotImpostaICIDichiaratoSALDOIMMOBILE = myUI.SalDovuto + TotImpostaICIDichiaratoSALDOIMMOBILE
                            Trovato = True
                        End If
                    Next
                End If
                If Trovato = False Then
                    AzzeraImporti(TotDiffImposta, TotImpostaICIAccertato, TotImpostaICIAccertatoACCONTO, TotImpostaICIAccertatoSALDO, TotImpostaICIDichiaratoIMMOBILE, TotImpostaICIDichiaratoACCONTOIMMOBILE, TotImpostaICIDichiaratoSALDOIMMOBILE, ImportoSanzioneImmobile, ImportoSanzioneImmobileRidotto, ImportoInteresseImmobile = 0)
                End If

                TotImpICIDichiarato += TotImpostaICIDichiaratoIMMOBILE
                TotDiffImpostaDICHIARATO += TotImpostaICIDichiaratoIMMOBILE
                TotDiffImpostaAccontoDICHIARATO = TotDiffImpostaAccontoDICHIARATO + TotImpostaICIDichiaratoACCONTOIMMOBILE
                TotDiffImpostaSaldoDICHIARATO = TotDiffImpostaSaldoDICHIARATO + TotImpostaICIDichiaratoSALDOIMMOBILE

                'prelevo dall'array di immobili accertati l'informazione sul fatto di calcolare o no gli INTERESSI Calcolo Interessi su singolo Immobile
                blnCalcolaInteressi = rowsArray.CalcolaInteressi
                TotImpostaICIAccertato = rowsArray.TotDovuto + TotImpostaICIAccertato
                TotImpostaICIAccertatoACCONTO = rowsArray.AccDovuto + TotImpostaICIAccertatoACCONTO
                TotImpostaICIAccertatoSALDO = rowsArray.SalDovuto + TotImpostaICIAccertatoSALDO
                'Totale imposta accertato per riepilogo
                TotImpICIACCERTAMENTO += TotImpostaICIAccertato
                'Calcolo il Totale della differenza di imposta
                TotDiffImposta = TotImpostaICIAccertato - TotImpostaICIDichiaratoIMMOBILE
                TotDiffImpostaACCONTO = TotImpostaICIAccertatoACCONTO - TotImpostaICIDichiaratoACCONTOIMMOBILE
                TotDiffImpostaSALDO = TotImpostaICIAccertatoSALDO - TotImpostaICIDichiaratoSALDOIMMOBILE

                TotDiffImpostaACCERTAMENTO += TotDiffImposta
                TotDiffImpostaAccontoACCERTAMENTO = TotDiffImpostaAccontoACCERTAMENTO + TotDiffImpostaACCONTO
                TotDiffImpostaSaldoACCERTAMENTO = TotDiffImpostaSaldoACCERTAMENTO + TotDiffImpostaSALDO
                'Aggiorno il objDSDichiarazioni con l'importo delle sanzioni calcolate-------------
                rowsArray.DiffImposta = TotDiffImposta
                TipoAvvisoRimborso = -1
                'Rimborso. Calcolo gli interessi Attivi sul singolo immobile. Al giro dopo la var viene azzerrata a -1.
                If TotDiffImposta < 0 Then
                    TipoAvvisoRimborso = OggettoAtto.Provvedimento.Rimborso '"5"
                End If
                'HashTable per calcolo Sanzioni e interessi
                objHashTable("CONNECTIONSTRINGANAGRAFICA") = ConstSession.StringConnectionAnagrafica 'ConfigurationManager.AppSettings("connectionStringSQLOPENAnagrafica")
                If objHashTable.ContainsKey("TIPOPROVVEDIMENTO") = True Then
                    objHashTable.Remove("TIPOPROVVEDIMENTO")
                End If
                'TipoAvvisoRimborso = -1 == False ----> Non è un rimborso Devo effettuare il calcolo guardando se è un rimborso sull'immobile oppure no Se è rimborso ho solo interessi attivi
                If TipoAvvisoRimborso = -1 Then
                    objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
                    TipoProvvedimento = TipoAvviso
                Else
                    objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvvisoRimborso)
                    TipoProvvedimento = TipoAvvisoRimborso
                End If
                If objHashTable.ContainsKey("COD_TIPO_PROCEDIMENTO") = True Then
                    objHashTable("COD_TIPO_PROCEDIMENTO") = OggettoAtto.Procedimento.Accertamento
                Else
                    objHashTable.Add("COD_TIPO_PROCEDIMENTO", OggettoAtto.Procedimento.Accertamento)
                End If
                If objHashTable.Contains("ANNOACCERTAMENTO") = False Then
                    objHashTable.Add("ANNOACCERTAMENTO", Anno)
                End If

                'Calcolo Sanzioni e Interessi su singolo Immobile (la procedura di calcolo delle sanzioni le calcola solo	se l'importo è positivo)
                If TotDiffImposta <> 0 Or TotDiffImposta = 0 Then
                    blnResult = False
                    Dim objHashTableDati As New Hashtable
                    objHashTableDati.Add("IDSANZIONI", rowsArray.IdSanzioni)
                    'L'Id Immobile è il Progressivo
                    objHashTableDati.Add("IDIMMOBILE", rowsArray.Progressivo)
                    objHashTableDati.Add("IDLEGAME", rowsArray.IdLegame)
                    'Calcolo le sanzioni per i singoli Immobili
                    oCalcoloSanzioniInteressi = CreateDatasetPerSanzInt(Anno, TotDiffImposta, TotDiffImpostaACCONTO, TotDiffImpostaSALDO, 0)
                    objDSCalcoloSanzioniInteressiAppoggio = SetObjDSAppoggioSanzioni(Anno, TotDiffImposta, TotVersamenti)

                    Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
                    objDSSanzioni = objCOMDichiarazioniAccertamenti.getSanzioni(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable, objHashTableDati, oCalcoloSanzioniInteressi, objDSCalcoloSanzioniInteressiAppoggio, False, sDataMorte)

                    'Creo una copia della struttura
                    If dtSanzioni.Rows.Count = 0 And objDSSanzioni.Tables.Count > 0 Then
                        dtSanzioni = objDSSanzioni.Tables("SANZIONI").Copy
                        dtSanzioni.Clear()
                    End If
                    For intCount = 0 To objDSSanzioni.Tables("SANZIONI").Rows.Count - 1
                        objDSSanzioni.Tables("SANZIONI").Rows(intCount)("IMPORTO_GIORNI") = 0
                        copyRow = objDSSanzioni.Tables("SANZIONI").Rows(intCount)
                        copyRow("motivazioni") = rowsArray.DescrSanzioni
                        dtSanzioni.ImportRow(copyRow)
                    Next
                    objDSMotivazioniSanzioni = dsMySanzioni

                    If Not objDSMotivazioniSanzioni Is Nothing Then
                        For intCount = 0 To objDSMotivazioniSanzioni.Tables(0).Rows.Count - 1
                            Dim rows() As DataRow
                            rows = dtSanzioni.Select("ID_LEGAME='" & rowsArray.IdLegame & "'")
                            ssanz = rowsArray.DescrSanzioni
                            For y As Integer = 0 To UBound(rows)
                                rows(y).Item("Motivazioni") = ssanz
                            Next
                            dtSanzioni.AcceptChanges()
                        Next
                    End If
                    'Aggiorno il DS con l'importo delle sanzioni calcolate
                    rowsArray.ImpSanzioni = oCalcoloSanzioniInteressi.Sanzioni
                    rowsArray.ImpSanzioniRidotto = oCalcoloSanzioniInteressi.SanzioniRidotto

                    ImportoSanzioneImmobile = ImportoSanzioneImmobile + oCalcoloSanzioniInteressi.Sanzioni
                    ImportoSanzioneImmobileRidotto = ImportoSanzioneImmobileRidotto + oCalcoloSanzioniInteressi.SanzioniRidotto
                    'totale sanzione dell'atto di accertamento
                    TotSanzioniACCERTAMENTO += ImportoSanzioneImmobile
                    TotImportoSanzioniRidottoACCERTAMENTO += ImportoSanzioneImmobileRidotto

                    'CALCOLO INTERESSI
                    ImportoInteresseImmobile = 0
                    If blnCalcolaInteressi = True Then
                        Dim myItem As New ObjBaseIntSanz
                        myItem.Anno = Anno
                        myItem.COD_TIPO_PROVVEDIMENTO = TipoProvvedimento
                        myItem.DifferenzaImposta = FormatNumber(oCalcoloSanzioniInteressi.DifferenzaImposta, 2)
                        myItem.DifferenzaImpostaAcconto = FormatNumber(oCalcoloSanzioniInteressi.DifferenzaImpostaAcconto, 2)
                        myItem.DifferenzaImpostaSaldo = FormatNumber(oCalcoloSanzioniInteressi.DifferenzaImpostaSaldo, 2)
                        myItem.IdContribuente = IdContribuente
                        myItem.IdEnte = ConstSession.IdEnte
                        ListBaseCalc.Add(myItem)

                        objCOMDichiarazioniAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
                        ListInteressi = objCOMDichiarazioniAccertamenti.getInteressi(ConstSession.IdEnte, Utility.Costanti.TRIBUTO_ICI, OggettoAtto.Capitolo.Interessi, TipoProvvedimento, OggettoAtto.Procedimento.Accertamento, Fase, Now, "", "", rowsArray.IdLegame, CType(ListBaseCalc.ToArray(GetType(ObjBaseIntSanz)), ObjBaseIntSanz()), ConstSession.StringConnection)
                        If Not ListInteressi Is Nothing Then
                            'Aggiorno il DS con l'importo delle interessi calcolati
                            For Each myInt As ObjInteressiSanzioni In ListInteressi
                                rowsArray.ImpInteressi = myInt.IMPORTO_GIORNI
                                ImportoInteresseImmobile += myInt.IMPORTO_GIORNI
                            Next
                        End If

                    Else
                        rowsArray.ImpInteressi = 0
                    End If
                    'totale interessi dell'atto di accertamento
                    TotInteressiACCERTAMENTO += ImportoInteresseImmobile
                Else
                    '*******************************************************************
                    'Se sono qui l'ici dichiarato è uguale a quello accertato Sanzioni e Interessi a 0
                    '*******************************************************************
                    rowsArray.ImpSanzioni = 0
                    rowsArray.ImpInteressi = 0
                End If
                'valorizzo totale imposta per il singolo immobile
                rowsArray.Totale = rowsArray.DiffImposta + rowsArray.ImpSanzioni + rowsArray.ImpInteressi
                'l'avviso è dato dalla somma di tutte le voci di ogni singolo immobile accertato differenza di imposta, sanzioni, interessi
            Next

            If Not objDSSanzioni Is Nothing Then
                objDSSanzioni.Dispose()
            End If

            objDSSanzioni = New DataSet
            objDSSanzioni.Tables.Add(dtSanzioni.Copy)
            Log.Debug("TotSanzioni=" + TotSanzioniACCERTAMENTO.ToString() + ", TotInteressi=" + TotInteressiACCERTAMENTO.ToString())
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.CalcoloImpSanzIntSingleUI.errore: ", ex)
            Return False
        End Try
    End Function
    Private Sub AzzeraImporti(ByRef TotDiffImposta As Double, ByRef TotImpostaICIAccertato As Double, ByRef TotImpostaICIAccertatoACCONTO As Double, ByRef TotImpostaICIAccertatoSALDO As Double, ByRef TotImpostaICIDichiaratoIMMOBILE As Double, ByRef TotImpostaICIDichiaratoACCONTOIMMOBILE As Double, ByRef TotImpostaICIDichiaratoSALDOIMMOBILE As Double, ByRef ImportoSanzioneImmobile As Double, ByRef ImportoSanzioneImmobileRidotto As Double, ByRef ImportoInteresseImmobile As Double)
        Try
            TotDiffImposta = 0
            TotImpostaICIAccertato = 0
            TotImpostaICIAccertatoACCONTO = 0
            TotImpostaICIAccertatoSALDO = 0
            TotImpostaICIDichiaratoIMMOBILE = 0
            TotImpostaICIDichiaratoACCONTOIMMOBILE = 0
            TotImpostaICIDichiaratoSALDOIMMOBILE = 0

            ImportoSanzioneImmobile = 0 'sanzioni singolo Immobile
            ImportoSanzioneImmobileRidotto = 0 'sanzioni ridotte singolo Immobile
            ImportoInteresseImmobile = 0 'interessi singolo Immobile
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.AzzeraImporti.errore: ", ex)
            Throw ex
        End Try
    End Sub
    Public Function GetTipoAvviso(blnTIPO_OPERAZIONE_RETTIFICA As Boolean, IsPrimoGiro As Boolean, ListAccertato() As objUIICIAccert, ListDichiarazioni() As objUIICIAccert, ImportoTotaleAvviso As Double, dblImportoTotaleF2 As Double, ByRef iTIPOPROVV_PREACC As Integer, ByRef TipoAvviso As Integer, ByRef DescrTipoAvviso As String) As Boolean
        Dim Trovato As Boolean = False

        Try
            If IsPrimoGiro Then
                TipoAvviso = OggettoAtto.Provvedimento.AccertamentoRettifica   '4
                'Prendo immobili di accertato
                For Each myAcc As objUIICIAccert In ListAccertato
                    Trovato = False
                    'Cerco l'immobili in tutti gli immobili di dichiarato'
                    'Se lo trovo esco dal ciclo e proseguo a cercare con immobili
                    'successivo di accertamento
                    If Not ListDichiarazioni Is Nothing Then
                        For Each myDic As objUIICIAccert In ListDichiarazioni
                            If myAcc.IdLegame = myDic.IdLegame Then
                                Trovato = True
                                Exit For
                            End If
                        Next
                    End If
                    'Se trovato = False vuol dire che non ho trovato l'immobile
                    If Trovato = False Then
                        'Avviso D'ufficio
                        TipoAvviso = OggettoAtto.Provvedimento.AccertamentoUfficio    '3
                        Exit For
                    End If
                Next
            Else
                If ImportoTotaleAvviso > 0 Or dblImportoTotaleF2 > 0 Then
                    If blnTIPO_OPERAZIONE_RETTIFICA Then
                        If ((ImportoTotaleAvviso + dblImportoTotaleF2) >= -2 And (ImportoTotaleAvviso + dblImportoTotaleF2) <= 2) Then
                            TipoAvviso = OggettoAtto.Provvedimento.AutotutelaAnnullamento
                        Else
                            TipoAvviso = OggettoAtto.Provvedimento.AutotutelaRettifica
                        End If
                    Else
                        'AVVISO
                        'per determinare il tipo di avviso si prende "lo stato" più grave tra lo stato del pre accertamento e lo stato dell'accertamento
                        If (iTIPOPROVV_PREACC = OggettoAtto.Provvedimento.AccertamentoUfficio) Or (TipoAvviso = OggettoAtto.Provvedimento.AccertamentoUfficio) Then
                            'se PREACCERTAMENTO o ACCERTAMENTO hanno scaturito un avviso di accertamento d'ufficio
                            TipoAvviso = OggettoAtto.Provvedimento.AccertamentoUfficio 'AVVISO DI ACCERTAMENTO D'UFFICIO                                    
                        ElseIf (iTIPOPROVV_PREACC = OggettoAtto.Provvedimento.AccertamentoRettifica) Or (TipoAvviso = OggettoAtto.Provvedimento.AccertamentoRettifica) Then
                            'se PREACCERTAMENTO o ACCERTAMENTO hanno scaturito un avviso di accertamento in rettifica
                            TipoAvviso = OggettoAtto.Provvedimento.AccertamentoRettifica  'AVVISO DI ACCERTAMENTO IN RETTIFICA                                    
                        Else
                            TipoAvviso = OggettoAtto.Provvedimento.AccertamentoRettifica 'AVVISO DI ACCERTAMENTO IN RETTIFICA
                        End If
                    End If
                ElseIf dblImportoTotaleF2 < 0 Then
                    TipoAvviso = OggettoAtto.Provvedimento.Rimborso  'AVVISO DI ACCERTAMENTO IN RETTIFICA                                
                ElseIf dblImportoTotaleF2 = 0 Then
                    If blnTIPO_OPERAZIONE_RETTIFICA Then
                        TipoAvviso = OggettoAtto.Provvedimento.AutotutelaAnnullamento
                    Else
                        'PREACCERTAMENTO E ACCERTAMENTO HANNO DATO ESITO POSITIVO
                        'NO AVVISO - NON CREO IL PROVVEDIMENTO
                        TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
                    End If
                End If
            End If
            Select Case (TipoAvviso)
                Case OggettoAtto.Provvedimento.Rimborso
                    DescrTipoAvviso = "Avviso di rimborso"
                Case OggettoAtto.Provvedimento.AccertamentoRettifica
                    DescrTipoAvviso = "Avviso di accertamento in rettifica"
                Case OggettoAtto.Provvedimento.AccertamentoUfficio
                    DescrTipoAvviso = "Avviso di accertamento d'ufficio"
                Case OggettoAtto.Provvedimento.AutotutelaRettifica
                    DescrTipoAvviso = "Avviso di Autotutela in rettifica"
                Case OggettoAtto.Provvedimento.AutotutelaAnnullamento
                    DescrTipoAvviso = "Avviso di Autotutela di annullamento"
                Case OggettoAtto.Provvedimento.NoAvviso
                    DescrTipoAvviso = "Nessun avviso emesso"
                Case Else
                    DescrTipoAvviso = "Avviso non determinato"
            End Select
            'Rimborso
            If ImportoTotaleAvviso < 0 Then
                'If DiffTotaleSanzInt < 0 Then 'GIULIA 09082005
                TipoAvviso = OggettoAtto.Provvedimento.Rimborso '5
                DescrTipoAvviso = "Avviso di rimborso"
            End If

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - ImportoTotaleAvviso:" & ImportoTotaleAvviso & " €")
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - TipoAvviso:" & TipoAvviso & " - " & DescrTipoAvviso)
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.GetTipoAvviso.errore: ", ex)
            Return False
        End Try
    End Function
    Public Function LoadRiepilogo(TotImpICIACCERTAMENTO As Double, TotDiffImpostaACCERTAMENTO As Double, TotImportoSanzioniACCERTAMENTO As Double, TotImportoInteressiACCERTAMENTO As Double, ImportoTotaleAvviso As Double, TotImportoSanzioniRidottoACCERTAMENTO As Double, TotVersamenti As Double, TotImpICIDichiarato As Double, ByRef objhashtableRIEPILOGO As Hashtable) As Boolean
        Try
            objhashtableRIEPILOGO.Add("TotImpICIACCERTAMENTO", TotImpICIACCERTAMENTO)
            objhashtableRIEPILOGO.Add("TotDiffImpostaACCERTAMENTO", TotDiffImpostaACCERTAMENTO)
            objhashtableRIEPILOGO.Add("TotImportoSanzioniACCERTAMENTO", TotImportoSanzioniACCERTAMENTO)
            objhashtableRIEPILOGO.Add("TotImportoInteressiACCERTAMENTO", TotImportoInteressiACCERTAMENTO)
            objhashtableRIEPILOGO.Add("ImportoTotaleAvviso", ImportoTotaleAvviso)
            objhashtableRIEPILOGO.Add("TotImportoSanzioniRidottoACCERTAMENTO", TotImportoSanzioniRidottoACCERTAMENTO)
            'inserisco in riepilogo i dati relativi al totale dei versamenti 
            objhashtableRIEPILOGO.Add("TotVersamenti", TotVersamenti)
            objhashtableRIEPILOGO.Add("TotImpICIDichiarato", TotImpICIDichiarato)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - TotImpICIACCERTAMENTO=" & TotImpICIACCERTAMENTO)
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - TotDiffImpostaACCERTAMENTO=" & TotDiffImpostaACCERTAMENTO)
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - TotImportoSanzioniACCERTAMENTO=" & TotImportoSanzioniACCERTAMENTO)
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - TotImportoInteressiACCERTAMENTO=" & TotImportoInteressiACCERTAMENTO)
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - ImportoTotaleAvviso=" & ImportoTotaleAvviso)
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - TotImportoSanzioniRidottoACCERTAMENTO=" & TotImportoSanzioniRidottoACCERTAMENTO)
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - TotVersamenti=" & TotVersamenti)
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - TotImpICIDichiarato=" & TotImpICIDichiarato)
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.LoadRiepilogo.errore: ", ex)
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Anno"></param>
    ''' <param name="IdContribuente"></param>
    ''' <param name="dsRiepilogoFase2"></param>
    ''' <param name="ImportoTotaleAvviso"></param>
    ''' <param name="TotDiffImpostaACCERTAMENTO"></param>
    ''' <param name="TotImportoSanzioniACCERTAMENTO"></param>
    ''' <param name="TotImportoSanzioniRidottoACCERTAMENTO"></param>
    ''' <param name="TotImportoInteressiACCERTAMENTO"></param>
    ''' <param name="TotDiffImpostaDICHIARATO"></param>
    ''' <param name="oCalcoloSanzioniInteressi"></param>
    ''' <param name="objhashtableRIEPILOGO"></param>
    ''' <param name="iTIPOPROVV_PREACC"></param>
    ''' <param name="dblImportoTotaleF2"></param>
    ''' <returns></returns>
    Public Function GetImpFinali(Anno As String, IdContribuente As String, dsRiepilogoFase2 As ObjBaseIntSanz, ByRef ImportoTotaleAvviso As Double, TotDiffImpostaACCERTAMENTO As Double, TotImportoSanzioniACCERTAMENTO As Double, TotImportoSanzioniRidottoACCERTAMENTO As Double, TotImportoInteressiACCERTAMENTO As Double, TotDiffImpostaDICHIARATO As Double, oCalcoloSanzioniInteressi As ObjBaseIntSanz, ByRef objhashtableRIEPILOGO As Hashtable, ByRef iTIPOPROVV_PREACC As Integer, ByRef dblImportoTotaleF2 As Double) As Boolean
        Dim dblImportoDiffImpTotaleF2 As Double
        Dim dblImportoSanzRidottoTotaleF2 As Double
        Dim ImpSanzioniFase2 As Double
        Dim ImpInteressiFase2 As Double
        Dim ImpSanzioniNoFase As Double
        Dim ImpInteressiNoFase As Double
        Dim strTIPOPROVV_PREACC As String

        Try
            dblImportoDiffImpTotaleF2 = 0
            ImpSanzioniFase2 = 0
            dblImportoSanzRidottoTotaleF2 = 0
            ImpInteressiFase2 = 0
            dblImportoTotaleF2 = 0
            iTIPOPROVV_PREACC = OggettoAtto.Provvedimento.AccertamentoUfficio

            If Not dsRiepilogoFase2 Is Nothing Then
                strTIPOPROVV_PREACC = dsRiepilogoFase2.COD_TIPO_PROVVEDIMENTO
                iTIPOPROVV_PREACC = dsRiepilogoFase2.COD_TIPO_PROVVEDIMENTO

                Log.Debug("difimpf2:: " & dsRiepilogoFase2.DifferenzaImposta.ToString)
                Log.Debug("sanzf2::" & dsRiepilogoFase2.Sanzioni.ToString)
                Log.Debug("sanzridf2::" & dsRiepilogoFase2.SanzioniRidotto.ToString)
                Log.Debug("intf2::" & dsRiepilogoFase2.Interessi.ToString)

                dblImportoDiffImpTotaleF2 = dsRiepilogoFase2.DifferenzaImposta
                ImpSanzioniFase2 = dsRiepilogoFase2.Sanzioni
                dblImportoSanzRidottoTotaleF2 = dsRiepilogoFase2.SanzioniRidotto
                ImpInteressiFase2 = dsRiepilogoFase2.Interessi
                objhashtableRIEPILOGO("DIFASE2") = dblImportoDiffImpTotaleF2
                objhashtableRIEPILOGO("SANZFASE2") = ImpSanzioniFase2
                objhashtableRIEPILOGO("SANZRIDOTTOFASE2") = dblImportoSanzRidottoTotaleF2
                objhashtableRIEPILOGO("INTFASE2") = ImpInteressiFase2

                dblImportoTotaleF2 = dblImportoDiffImpTotaleF2 + ImpSanzioniFase2 + ImpInteressiFase2
                objhashtableRIEPILOGO("TOTFASE2") = dblImportoTotaleF2
            Else
                iTIPOPROVV_PREACC = OggettoAtto.Provvedimento.NoAvviso
            End If
            Log.Debug("dblImportoDiffImpTotaleF2::" & dblImportoDiffImpTotaleF2.ToString)
            Log.Debug("dblImportoSanzTotaleF2::" & ImpSanzioniFase2.ToString)
            Log.Debug("dblImportoSanzRidottoTotaleF2::" & dblImportoSanzRidottoTotaleF2.ToString)
            Log.Debug("dblImportoIntTotaleF2::" & ImpInteressiFase2.ToString)
            Log.Debug("dblImportoTotaleF2::" & dblImportoTotaleF2.ToString)

            ImpSanzioniNoFase = TotImportoSanzioniACCERTAMENTO
            ImpInteressiNoFase = TotImportoInteressiACCERTAMENTO
            '************************************************************************************************************************************
            'somma algebrica tra i totali dell'accertamento e quelli della fase2 del preaccertamento
            ImportoTotaleAvviso += dblImportoTotaleF2
            TotDiffImpostaACCERTAMENTO += dblImportoDiffImpTotaleF2
            TotImportoSanzioniRidottoACCERTAMENTO += dblImportoSanzRidottoTotaleF2
            TotImportoSanzioniACCERTAMENTO += ImpSanzioniFase2
            TotImportoInteressiACCERTAMENTO += ImpInteressiFase2
            '************************************************************************************************************************************

            objhashtableRIEPILOGO.Add("TOTAVVISO", ImportoTotaleAvviso)
            objhashtableRIEPILOGO.Add("DIAVVISO", TotDiffImpostaACCERTAMENTO)
            objhashtableRIEPILOGO.Add("SANZAVVISO", TotImportoSanzioniACCERTAMENTO)
            objhashtableRIEPILOGO.Add("SANZRIDOTTOAVVISO", TotImportoSanzioniRidottoACCERTAMENTO)
            objhashtableRIEPILOGO.Add("INTAVVISO", TotImportoInteressiACCERTAMENTO)

            If Not oCalcoloSanzioniInteressi Is Nothing Then
                oCalcoloSanzioniInteressi.Dichiarato = objhashtableRIEPILOGO("TotImpICIDichiarato")
                oCalcoloSanzioniInteressi.Accertato = objhashtableRIEPILOGO("TotImpICIACCERTAMENTO")
                oCalcoloSanzioniInteressi.Sanzioni = TotImportoSanzioniACCERTAMENTO
                oCalcoloSanzioniInteressi.SanzioniRidotto = TotImportoSanzioniRidottoACCERTAMENTO
                oCalcoloSanzioniInteressi.Interessi = TotImportoInteressiACCERTAMENTO
                oCalcoloSanzioniInteressi.DifferenzaImposta = TotDiffImpostaACCERTAMENTO
                oCalcoloSanzioniInteressi.SanzioniF2 = ImpSanzioniFase2
                oCalcoloSanzioniInteressi.InteressiF2 = ImpInteressiFase2
                oCalcoloSanzioniInteressi.SanzioniAcc = ImpSanzioniNoFase
                oCalcoloSanzioniInteressi.InteressiAcc = ImpInteressiNoFase
            ElseIf Not dsRiepilogoFase2 Is Nothing Then
                oCalcoloSanzioniInteressi = CreateDatasetPerSanzInt(Anno, 0, 0, 0, 0)
                oCalcoloSanzioniInteressi.Sanzioni = TotImportoSanzioniACCERTAMENTO
                oCalcoloSanzioniInteressi.SanzioniRidotto = TotImportoSanzioniRidottoACCERTAMENTO
                oCalcoloSanzioniInteressi.Interessi = TotImportoInteressiACCERTAMENTO
                oCalcoloSanzioniInteressi.DifferenzaImposta = TotDiffImpostaACCERTAMENTO
                oCalcoloSanzioniInteressi.SanzioniF2 = ImpSanzioniFase2
                oCalcoloSanzioniInteressi.InteressiF2 = ImpInteressiFase2
                oCalcoloSanzioniInteressi.SanzioniAcc = ImpSanzioniNoFase
                oCalcoloSanzioniInteressi.InteressiAcc = ImpInteressiNoFase
            End If

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.GetImpFinali.errore: ", ex)
            Return False
        End Try
    End Function
    'Public Function GetImpFinali(Anno As String, IdContribuente As String, dsRiepilogoFase2 As DataSet, ByRef ImportoTotaleAvviso As Double, TotDiffImpostaACCERTAMENTO As Double, TotImportoSanzioniACCERTAMENTO As Double, TotImportoSanzioniRidottoACCERTAMENTO As Double, TotImportoInteressiACCERTAMENTO As Double, TotDiffImpostaDICHIARATO As Double, objDSCalcoloSanzioniInteressi As DataSet, ByRef objhashtableRIEPILOGO As Hashtable, ByRef iTIPOPROVV_PREACC As Integer, ByRef dblImportoTotaleF2 As Double) As Boolean
    '    Dim dblImportoDiffImpTotaleF2 As Double
    '    Dim dblImportoSanzRidottoTotaleF2 As Double
    '    Dim ImpSanzioniFase2 As Double
    '    Dim ImpInteressiFase2 As Double
    '    Dim ImpSanzioniNoFase As Double
    '    Dim ImpInteressiNoFase As Double
    '    Dim strTIPOPROVV_PREACC As String

    '    Try
    '        dblImportoDiffImpTotaleF2 = 0
    '        ImpSanzioniFase2 = 0
    '        dblImportoSanzRidottoTotaleF2 = 0
    '        ImpInteressiFase2 = 0
    '        dblImportoTotaleF2 = 0
    '        iTIPOPROVV_PREACC = OggettoAtto.Provvedimento.AccertamentoUfficio

    '        If Not dsRiepilogoFase2 Is Nothing Then
    '            If dsRiepilogoFase2.Tables.Count > 0 Then
    '                If dsRiepilogoFase2.Tables(0).Rows.Count > 0 Then
    '                    strTIPOPROVV_PREACC = CType(dsRiepilogoFase2.Tables(0).Rows(0)("TIPO_PROVVEDIMENTO"), String)
    '                    iTIPOPROVV_PREACC = dsRiepilogoFase2.Tables(0).Rows(0)("TIPO_PROVVEDIMENTO")

    '                    Log.Debug("difimpf2:: " & dsRiepilogoFase2.Tables(0).Rows(0)("DIFFERENZA_IMPOSTA_TOTALE").ToString)
    '                    Log.Debug("sanzf2::" & dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_SANZIONI").ToString)
    '                    Log.Debug("sanzridf2::" & dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_SANZIONI_RIDOTTO").ToString)
    '                    Log.Debug("intf2::" & dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_INTERESSI").ToString)

    '                    dblImportoDiffImpTotaleF2 = dsRiepilogoFase2.Tables(0).Rows(0)("DIFFERENZA_IMPOSTA_TOTALE").ToString.Replace(".", ",")
    '                    ImpSanzioniFase2 = dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_SANZIONI").ToString.Replace(".", ",")
    '                    dblImportoSanzRidottoTotaleF2 = dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_SANZIONI_RIDOTTO").ToString.Replace(".", ",")
    '                    ImpInteressiFase2 = dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_INTERESSI").ToString.Replace(".", ",")
    '                    objhashtableRIEPILOGO("DIFASE2") = dblImportoDiffImpTotaleF2
    '                    objhashtableRIEPILOGO("SANZFASE2") = ImpSanzioniFase2
    '                    objhashtableRIEPILOGO("SANZRIDOTTOFASE2") = dblImportoSanzRidottoTotaleF2
    '                    objhashtableRIEPILOGO("INTFASE2") = ImpInteressiFase2

    '                    dblImportoTotaleF2 = dblImportoDiffImpTotaleF2 + ImpSanzioniFase2 + ImpInteressiFase2
    '                    objhashtableRIEPILOGO("TOTFASE2") = dblImportoTotaleF2

    '                End If
    '            Else
    '                iTIPOPROVV_PREACC = OggettoAtto.Provvedimento.NoAvviso
    '            End If
    '        Else
    '            iTIPOPROVV_PREACC = OggettoAtto.Provvedimento.NoAvviso
    '        End If
    '        Log.Debug("dblImportoDiffImpTotaleF2::" & dblImportoDiffImpTotaleF2.ToString)
    '        Log.Debug("dblImportoSanzTotaleF2::" & ImpSanzioniFase2.ToString)
    '        Log.Debug("dblImportoSanzRidottoTotaleF2::" & dblImportoSanzRidottoTotaleF2.ToString)
    '        Log.Debug("dblImportoIntTotaleF2::" & ImpInteressiFase2.ToString)
    '        Log.Debug("dblImportoTotaleF2::" & dblImportoTotaleF2.ToString)

    '        ImpSanzioniNoFase = TotImportoSanzioniACCERTAMENTO
    '        ImpInteressiNoFase = TotImportoInteressiACCERTAMENTO
    '        '************************************************************************************************************************************
    '        'somma algebrica tra i totali dell'accertamento e quelli della fase2 del preaccertamento
    '        ImportoTotaleAvviso += dblImportoTotaleF2
    '        TotDiffImpostaACCERTAMENTO += dblImportoDiffImpTotaleF2
    '        TotImportoSanzioniRidottoACCERTAMENTO += dblImportoSanzRidottoTotaleF2
    '        TotImportoSanzioniACCERTAMENTO += ImpSanzioniFase2
    '        TotImportoInteressiACCERTAMENTO += ImpInteressiFase2
    '        '************************************************************************************************************************************

    '        objhashtableRIEPILOGO.Add("TOTAVVISO", ImportoTotaleAvviso)
    '        objhashtableRIEPILOGO.Add("DIAVVISO", TotDiffImpostaACCERTAMENTO)
    '        objhashtableRIEPILOGO.Add("SANZAVVISO", TotImportoSanzioniACCERTAMENTO)
    '        objhashtableRIEPILOGO.Add("SANZRIDOTTOAVVISO", TotImportoSanzioniRidottoACCERTAMENTO)
    '        objhashtableRIEPILOGO.Add("INTAVVISO", TotImportoInteressiACCERTAMENTO)

    '        If Not objDSCalcoloSanzioniInteressi Is Nothing Then
    '            If objDSCalcoloSanzioniInteressi.Tables.Count > 0 Then
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_DICHIARATO") = objhashtableRIEPILOGO("TotImpICIDichiarato")
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_ACCERTATO") = objhashtableRIEPILOGO("TotImpICIACCERTAMENTO")
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI") = TotImportoSanzioniACCERTAMENTO
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO") = TotImportoSanzioniRidottoACCERTAMENTO
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI") = TotImportoInteressiACCERTAMENTO
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("DIFFERENZA_IMPOSTA_TOTALE") = TotDiffImpostaACCERTAMENTO
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_TOTALE_DICHIARATO") = TotDiffImpostaDICHIARATO
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_TOTALE_ACCERTATO") = ImportoTotaleAvviso
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_F2") = ImpSanzioniFase2
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI_F2") = ImpInteressiFase2
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_ACC") = ImpSanzioniNoFase
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI_ACC") = ImpInteressiNoFase
    '            End If
    '        ElseIf Not dsRiepilogoFase2 Is Nothing Then
    '            objDSCalcoloSanzioniInteressi = CreateDatasetPerSanzInt(Anno, IdContribuente, 0, 0, 0)
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI") = TotImportoSanzioniACCERTAMENTO
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO") = TotImportoSanzioniRidottoACCERTAMENTO
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI") = TotImportoInteressiACCERTAMENTO
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("DIFFERENZA_IMPOSTA_TOTALE") = TotDiffImpostaACCERTAMENTO
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_TOTALE_DICHIARATO") = TotDiffImpostaDICHIARATO
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_TOTALE_ACCERTATO") = ImportoTotaleAvviso
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_F2") = ImpSanzioniFase2
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI_F2") = ImpInteressiFase2
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_ACC") = ImpSanzioniNoFase
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI_ACC") = ImpInteressiNoFase
    '        End If

    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.GetImpFinali.errore: ", ex)
    '        Return False
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="anno"></param>
    ''' <param name="DiffImposta"></param>
    ''' <param name="DiffImpostaACCONTO"></param>
    ''' <param name="DiffImpostaSALDO"></param>
    ''' <param name="ImpPagato"></param>
    ''' <returns></returns>
    Private Function CreateDatasetPerSanzInt(ByVal anno As Integer, ByVal DiffImposta As Double, ByVal DiffImpostaACCONTO As Double, ByVal DiffImpostaSALDO As Double, ImpPagato As Double) As ObjBaseIntSanz
        Dim myItem As New ObjBaseIntSanz
        Try
            myItem.Anno = anno
            myItem.Sanzioni = 0
            myItem.SanzioniRidotto = 0
            '*****
            myItem.Interessi = 0
            myItem.DifferenzaImposta = DiffImposta
            myItem.DifferenzaImpostaAcconto = DiffImpostaACCONTO
            myItem.DifferenzaImpostaSaldo = DiffImpostaSALDO

            myItem.Dichiarato = 0
            myItem.Pagato = ImpPagato
            myItem.Accertato = 0

            myItem.ModalitaUnicaSoluzione = False
            '*** 20140701 - IMU/TARES ***
            If anno >= 2012 Then
                myItem.QuotaRiduzione = 3
            Else
                myItem.QuotaRiduzione = 4
            End If
            '*** ***
            myItem.SanzioniF2 = 0
            myItem.InteressiF2 = 0
            myItem.SanzioniAcc = 0
            myItem.InteressiAcc = 0
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.CreateDatasetPerSanzInt.errore: ", ex)
        End Try
        Return myItem
    End Function
    'Private Function CreateDatasetPerSanzInt(ByVal anno As Integer, ByVal codContribuente As String, ByVal DiffImposta As Double, ByVal DiffImpostaACCONTO As Double, ByVal DiffImpostaSALDO As Double) As DataSet
    '    Dim objDS As New DataSet
    '    Try
    '        Dim newTable As DataTable
    '        newTable = New DataTable("TABLE")
    '        Dim NewColumn As New DataColumn

    '        NewColumn = New DataColumn

    '        NewColumn.ColumnName = "COD_CONTRIBUENTE"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ANNO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)


    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DIFFERENZA_IMPOSTA_ACCONTO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DIFFERENZA_IMPOSTA_SALDO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DIFFERENZA_IMPOSTA_TOTALE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_SANZIONI"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_SANZIONI_RIDOTTO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)
    '        '************
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_INTERESSI"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FLAG_MODALITA_UNICA_SOLUZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.Boolean")
    '        NewColumn.DefaultValue = False
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_TOTALE_DICHIARATO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_TOTALE_VERSATO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_TOTALE_ACCERTATO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)
    '        '*** 20140701 - IMU/TARES ***
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "QUOTARIDUZIONESANZIONI"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = 1
    '        newTable.Columns.Add(NewColumn)
    '        '*** ***

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_SANZIONI_F2"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_INTERESSI_F2"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_SANZIONI_ACC"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_INTERESSI_ACC"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_DICHIARATO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_ACCERTATO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        Dim row1 As DataRow

    '        row1 = newTable.NewRow()
    '        row1.Item("COD_CONTRIBUENTE") = codContribuente
    '        row1.Item("ANNO") = anno
    '        row1.Item("IMPORTO_SANZIONI") = 0
    '        row1.Item("IMPORTO_SANZIONI_RIDOTTO") = 0
    '        '*****
    '        row1.Item("IMPORTO_INTERESSI") = 0
    '        row1.Item("DIFFERENZA_IMPOSTA_TOTALE") = DiffImposta
    '        row1.Item("DIFFERENZA_IMPOSTA_ACCONTO") = DiffImpostaACCONTO
    '        row1.Item("DIFFERENZA_IMPOSTA_SALDO") = DiffImpostaSALDO

    '        row1.Item("IMPORTO_TOTALE_DICHIARATO") = 0
    '        row1.Item("IMPORTO_TOTALE_VERSATO") = 0
    '        row1.Item("IMPORTO_TOTALE_ACCERTATO") = 0

    '        row1.Item("FLAG_MODALITA_UNICA_SOLUZIONE") = False
    '        '*** 20140701 - IMU/TARES ***
    '        If anno >= 2012 Then
    '            row1.Item("QUOTARIDUZIONESANZIONI") = 3
    '        Else
    '            row1.Item("QUOTARIDUZIONESANZIONI") = 4
    '        End If
    '        '*** ***
    '        row1.Item("IMPORTO_SANZIONI_F2") = 0
    '        row1.Item("IMPORTO_INTERESSI_F2") = 0
    '        row1.Item("IMPORTO_SANZIONI_ACC") = 0
    '        row1.Item("IMPORTO_INTERESSI_ACC") = 0

    '        newTable.Rows.Add(row1)

    '        objDS.Tables.Add(newTable)
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.CreateDatasetPerSanzInt.errore: ", ex)
    '    End Try
    '    Return objDS
    'End Function
    Public Function SetObjDSAppoggioSanzioni(ByVal anno As Integer, ByVal DiffImposta As Double, ByVal ImportoVersato As Double) As DataSet
        Dim objDS As New DataSet

        Try
            Dim newTableAppoggio As DataTable
            newTableAppoggio = New DataTable("TP_APPOGGIO_CALCOLO_SANZIONI")

            Dim NewColumn As New DataColumn
            NewColumn.ColumnName = "ANNO"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTableAppoggio.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "IVA"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = System.DBNull.Value
            newTableAppoggio.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "IVS"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = System.DBNull.Value
            newTableAppoggio.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "IVUS"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = System.DBNull.Value
            newTableAppoggio.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "IV"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = System.DBNull.Value
            newTableAppoggio.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DI"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = System.DBNull.Value
            newTableAppoggio.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "GG"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = System.DBNull.Value
            newTableAppoggio.Columns.Add(NewColumn)

            Dim row1 As DataRow

            row1 = newTableAppoggio.NewRow()

            row1.Item("ANNO") = anno
            row1.Item("DI") = DiffImposta
            row1.Item("IV") = ImportoVersato

            newTableAppoggio.Rows.Add(row1)

            objDS.Tables.Add(newTableAppoggio)

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.SetObjDSAppoggioSanzioni.errore: ", ex)
        End Try
        Return objDS
    End Function
#End Region

    Public Function getATTIRicercaSemplice(ByVal objHashTable As Hashtable) As DataSet
        Log.Debug("getATTIRicercaSemplice::inizio")

        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim objDSATTIRicercaSemplice As New DataSet
        Dim sSQL As String
        Dim objUtility As New MyUtility
        Dim sNumeroAvviso As String = ""
        Dim sNumeroAtto As String = ""
        Dim sCognome As String
        Dim sNome As String
        Dim sCodiceFiscale As String
        Dim sPartitaIVA As String
        Dim sCOD_ENTE As String = ""
        Dim sCOD_TRIBUTO As String = ""

        Try
            'strNumeroAvviso = CType(objHashTable("NUMEROPROVVEDIMENTO"), String)
            sNumeroAtto = CType(objHashTable("NUMEROPROVVEDIMENTO"), String)
            sCOD_ENTE = CType(objHashTable("CODENTE"), String)
            sCOD_TRIBUTO = CType(objHashTable("CODTRIBUTO"), String)

            sCognome = objUtility.CToStr(objHashTable("COGNOME"))
            sNome = objUtility.CToStr(objHashTable("NOME"))
            sCodiceFiscale = objUtility.CToStr(objHashTable("CODICEFISCALE"))
            sPartitaIVA = objUtility.CToStr(objHashTable("PARTITAIVA"))

            sSQL = "SELECT DISTINCT COD_CONTRIBUENTE, NOMINATIVO, CODICE_FISCALE, PARTITA_IVA"
            sSQL += " FROM V_GETATTIRICERCASEMPLICE"
            sSQL += " WHERE (COD_ENTE='" & sCOD_ENTE & "')"
            If Trim(sCognome) <> "" Then
                sSQL += " AND (COGNOME LIKE '" & Replace(Replace(Trim(sCognome), "'", "''"), "*", "%") & "%')"
            End If
            If Trim(sNome) <> "" Then
                sSQL += " AND (NOME LIKE '" & Replace(Replace(Trim(sNome), "'", "''"), "*", "%") & "%')"
            End If
            If Trim(sCodiceFiscale) <> "" Then
                sSQL += " AND (CODICE_FISCALE LIKE '" & Replace(Trim(sCodiceFiscale), "*", "%") & "%')"
            End If
            If Trim(sPartitaIVA) <> "" Then
                sSQL += " AND (PARTITA_IVA LIKE '" & Replace(Trim(sPartitaIVA), "*", "%") & "%')"
            End If
            If Len(sNumeroAtto) > 0 Then
                sSQL += "AND (NUMERO_ATTO LIKE '" & Replace(Trim(sNumeroAtto), "*", "%") & "%')"
            End If
            If sCOD_TRIBUTO <> "-1" And sCOD_TRIBUTO <> "" Then
                sSQL += " AND (COD_TRIBUTO='" & sCOD_TRIBUTO & "')"
            End If
            sSQL += " ORDER BY NOMINATIVO"
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(objDSATTIRicercaSemplice, "TP_ATTI_RICERCA_SEMPLICE")
            myAdapter.Dispose()

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.getATTIRicercaSemplice.errore: ", ex)
        End Try
        Return objDSATTIRicercaSemplice
    End Function

    Sub GetTariffaTarsu(ByRef myoAccertamentoSingolo As OggettoArticoloRuolo) ', ByVal WFSessione As OPENUtility.CreateSessione
        Dim sSQL As String
        Dim NOME_DATABASE_TARSU As String
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim drDati As SqlClient.SqlDataReader
        Try
            NOME_DATABASE_TARSU = ConfigurationManager.AppSettings("NOME_DATABASE_TARSU")

            sSQL = "select * from " & NOME_DATABASE_TARSU & ".dbo.TBLTARIFFE"
            sSQL += " where 1=1"
            sSQL += " and anno=" & myoAccertamentoSingolo.Anno
            sSQL += " and IDCategoria='" & myoAccertamentoSingolo.Categoria & "'"
            sSQL += " and idente='" & ConstSession.IdEnte & "'"

            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            drDati = cmdMyCommand.ExecuteReader
            'drDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
            Do While drDati.Read
                If Not IsDBNull(drDati("IDTariffa")) Then
                    myoAccertamentoSingolo.IDTariffa = drDati("IDTariffa")
                End If
                If Not IsDBNull(drDati("Importo")) Then
                    myoAccertamentoSingolo.ImpTariffa = drDati("Importo")
                End If

            Loop
            drDati.Close()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.GetTariffaTarsu.errore: ", ex)
        End Try
    End Sub

    '*** 20140701 - IMU/TARES ***
    'Public Function RicercaAtti(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sApplicazione As String, ByVal sIdEnte As String, ByVal codContribuente As Integer, ByVal anno As Integer, ByVal sTributo As String, ByVal ID_PROVVEDIMENTO_RETTIFICA As String, ByRef objDS As DataSet, ByRef objHashTable As Hashtable) As DataTable
    '    'Dim objSessione As CreateSessione
    '    Dim sSQL, strWFErrore As String
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    Dim NOME_DATABASE_ICI As String
    '    Dim objDBManager As DBManager
    '    Dim ds As DataSet
    '    Dim dt As DataTable
    '    Dim i, annotemp As Integer
    '    Dim objUtility As New MyUtility
    '    Dim strConnectionStringAnagrafica As String

    '    Try
    '        objHashTable.Add("COD_CONTRIBUENTE", objUtility.CToStr(codContribuente))
    '        objHashTable.Add("CODTRIBUTO", sTributo)
    '        objHashTable.Add("ANNO", anno)
    '        objHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
    '        objHashTable.Add("CODENTE", sIdEnte)
    '        objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '        'objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '        'objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '        'objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '        objSessione = New CreateSessione(sParametroENV, sUserName, sApplicazione)
    '        If Not objSessione.CreaSessione(sUserName, strWFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '        strConnectionStringAnagrafica = ConstSession.StringConnectionAnagrafica 'objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEANAGRAFICA")).GetConnection.ConnectionString
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", strConnectionStringAnagrafica)

    '        objHashTable.Add("ID_PROVVEDIMENTO_RETTIFICA", ID_PROVVEDIMENTO_RETTIFICA)


    '        Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
    '        objDS = objCOMRicerca.getProvvedimentiContribuente(objHashTable)

    '        If Not objDS Is Nothing Then
    '            If objDS.Tables(0).Rows.Count > 0 Then
    '                'trovato un provvedimento dello stesso anno
    '                dt = objDS.Tables(0)
    '            Else
    '                'Non ho trovato un provvedimento dello stesso anno
    '                'provo a cercare un provvedimento dell'anno precedente
    '                annotemp = anno - 1
    '                objHashTable("ANNO") = annotemp

    '                objDS = objCOMRicerca.getProvvedimentiContribuente(objHashTable)
    '                If Not objDS Is Nothing Then
    '                    'trovato un provvedimento dell'anno precedente
    '                    dt = objDS.Tables(0)
    '                End If
    '            End If
    '        End If
    '    Catch Err As Exception
    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.RicercaAtti.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        RicercaAtti = dt
    '        dt.Dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="codContribuente"></param>
    ''' <param name="anno"></param>
    ''' <param name="sTributo"></param>
    ''' <param name="ID_PROVVEDIMENTO_RETTIFICA"></param>
    ''' <param name="objDS"></param>
    ''' <param name="objHashTable"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function RicercaAtti(ByVal sIdEnte As String, ByVal codContribuente As Integer, ByVal anno As Integer, ByVal sTributo As String, ByVal ID_PROVVEDIMENTO_RETTIFICA As String, ByRef objDS As DataSet, ByRef objHashTable As Hashtable) As DataTable
        'Dim NOME_DATABASE_ICI As String
        'Dim objDBManager As DBManager
        Dim dt As DataTable
        Dim annotemp As Integer
        Dim objUtility As New MyUtility
        Dim strConnectionStringAnagrafica As String

        Try
            objHashTable.Add("COD_CONTRIBUENTE", codContribuente)
            objHashTable.Add("CODTRIBUTO", sTributo)
            objHashTable.Add("ANNO", anno)
            objHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
            objHashTable.Add("CODENTE", sIdEnte)

            strConnectionStringAnagrafica = ConstSession.StringConnectionAnagrafica 'objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEANAGRAFICA")).GetConnection.ConnectionString
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", strConnectionStringAnagrafica)

            objHashTable.Add("ID_PROVVEDIMENTO_RETTIFICA", ID_PROVVEDIMENTO_RETTIFICA)

            Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
            objDS = objCOMRicerca.getProvvedimentiContribuente(ConstSession.StringConnection, sIdEnte, objHashTable)
            If Not objDS Is Nothing Then
                If objDS.Tables(0).Rows.Count > 0 Then
                    'trovato un provvedimento dello stesso anno
                    dt = objDS.Tables(0)
                Else
                    'Non ho trovato un provvedimento dello stesso anno
                    'provo a cercare un provvedimento dell'anno precedente
                    annotemp = anno - 1
                    objHashTable("ANNO") = annotemp

                    objDS = objCOMRicerca.getProvvedimentiContribuente(ConstSession.StringConnection, sIdEnte, objHashTable)
                    If Not objDS Is Nothing Then
                        'trovato un provvedimento dell'anno precedente
                        dt = objDS.Tables(0)
                    End If
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.RicercaAtti.errore: ", Err)
            Return Nothing
        Finally
            RicercaAtti = dt
            dt.Dispose()
        End Try
    End Function
    'Public Function RicercaAtti(ByVal sIdEnte As String, ByVal codContribuente As Integer, ByVal anno As Integer, ByVal sTributo As String, ByVal ID_PROVVEDIMENTO_RETTIFICA As String, ByRef objDS As DataSet, ByRef objHashTable As Hashtable) As DataTable
    '    'Dim NOME_DATABASE_ICI As String
    '    'Dim objDBManager As DBManager
    '    Dim dt As DataTable
    '    Dim annotemp As Integer
    '    Dim objUtility As New MyUtility
    '    Dim strConnectionStringAnagrafica As String

    '    Try
    '        objHashTable.Add("COD_CONTRIBUENTE", objUtility.CToStr(codContribuente))
    '        objHashTable.Add("CODTRIBUTO", sTributo)
    '        objHashTable.Add("ANNO", anno)
    '        objHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
    '        objHashTable.Add("CODENTE", sIdEnte)
    '        objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '        'objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '        'objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '        'objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '        strConnectionStringAnagrafica = ConstSession.StringConnectionAnagrafica 'objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEANAGRAFICA")).GetConnection.ConnectionString
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", strConnectionStringAnagrafica)

    '        objHashTable.Add("ID_PROVVEDIMENTO_RETTIFICA", ID_PROVVEDIMENTO_RETTIFICA)

    '        Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
    '        objDS = objCOMRicerca.getProvvedimentiContribuente(objHashTable)
    '        If Not objDS Is Nothing Then
    '            If objDS.Tables(0).Rows.Count > 0 Then
    '                'trovato un provvedimento dello stesso anno
    '                dt = objDS.Tables(0)
    '            Else
    '                'Non ho trovato un provvedimento dello stesso anno
    '                'provo a cercare un provvedimento dell'anno precedente
    '                annotemp = anno - 1
    '                objHashTable("ANNO") = annotemp

    '                objDS = objCOMRicerca.getProvvedimentiContribuente(objHashTable)
    '                If Not objDS Is Nothing Then
    '                    'trovato un provvedimento dell'anno precedente
    '                    dt = objDS.Tables(0)
    '                End If
    '            End If
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.RicercaAtti.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        RicercaAtti = dt
    '        dt.Dispose()
    '    End Try
    'End Function

    'Public Function RicercaAccertatoICI(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sApplicazione As String, ByVal sIdEnte As String, ByVal codContribuente As Integer, ByVal annoSel As Integer, ByVal ID_Provvedimento As Integer, ByVal annoAccer As Integer, Optional ByVal provenienza As String = "acc") As DataTable
    '    'Dim objSessione As CreateSessione
    '    Dim sSQL, strWFErrore As String
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    Dim NOME_DATABASE_ICI As String
    '    Dim objDBManager As DBManager
    '    Dim ds As DataSet
    '    'Dim dt As DataTable
    '    Dim i As Integer
    '    Dim dt As New DataTable("IMMOBILI")
    '    Dim miodatarow As DataRow()
    '    Try
    '        NOME_DATABASE_ICI = ConfigurationManager.AppSettings("NOME_DATABASE_ICI")
    '        objSessione = New CreateSessione(sParametroENV, sUserName, sApplicazione)
    '        If Not objSessione.CreaSessione(sUserName, strWFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

    '        sSQL = "SELECT DATAINIZIO AS DAL, CAST(DATAFINE AS VARCHAR) AS AL,"
    '        sSQL += " FOGLIO, NUMERO, SUBALTERNO, "
    '        sSQL += " CODCATEGORIACATASTALE AS CATEGORIA, "
    '        sSQL += " CODCLASSE AS CLASSE, "
    '        sSQL += " CAST (CONSISTENZA AS NVARCHAR) AS CONSISTENZA, "
    '        sSQL += " TR=CASE "
    '        sSQL += "  WHEN COD_RENDITA IS NULL THEN '0' "
    '        sSQL += "  ELSE " & NOME_DATABASE_ICI & ".DBO.TIPO_RENDITA.SIGLA "
    '        sSQL += " END, "
    '        sSQL += " RENDITA, "
    '        sSQL += " CAST (VALOREIMMOBILE AS VARCHAR) AS RENDITA_VALORE,"
    '        sSQL += " ZONA, "
    '        'SSQL += " '-1' AS IDSANZIONI,"
    '        sSQL += " IDSANZIONI=CASE WHEN COD_VOCE IS NULL THEN '-1' ELSE COD_VOCE END, "
    '        sSQL += " TP_IMMOBILI_ACCERTATI_ACCERTAMENTI.ID_LEGAME AS IDLEGAME,"
    '        If provenienza = "ACC" Then
    '            sSQL += " -1.0 AS ICICALCOLATO,"
    '        Else
    '            sSQL += " IMPORTO_TOTALE_ICI_DOVUTO AS ICICALCOLATO,"
    '        End If
    '        sSQL += "0 AS PROGRESSIVO, "
    '        'SSQL += "  '' AS SANZIONI, "
    '        'IMPORTO SANZIONI
    '        sSQL += " SANZIONI," '=CASE WHEN MOTIVAZIONE IS NULL THEN '' ELSE MOTIVAZIONE END, "
    '        If provenienza = "ACC" Then
    '            sSQL += " '0' AS INTERESSI,"
    '        Else
    '            sSQL += " INTERESSI,"
    '        End If
    '        sSQL += " PERCPOSSESSO,"
    '        sSQL += " " & NOME_DATABASE_ICI & ".DBO.TBLPOSSESSO.DESCRIZIONE AS TITPOSSESSO,"
    '        sSQL += " CAST(ABITAZIONEPRINCIPALEATTUALE AS VARCHAR) AS FLAG_PRINCIPALE,"
    '        sSQL += " '0' AS FLAG_PERTINENZA,"
    '        sSQL += " CAST(RIDUZIONE  AS VARCHAR) AS FLAG_RIDOTTO,"
    '        sSQL += " IDIMMOBILEPERTINENTE AS IDIMMOBILEPERTINENZA ,"
    '        sSQL += " SEZIONE, "
    '        sSQL += " VIA AS INDIRIZZO,"
    '        sSQL += " " & NOME_DATABASE_ICI & ".DBO.TBLPOSSESSO.TIPOPOSSESSO AS CODTITPOSSESSO, "
    '        sSQL += "CODTIPORENDITA=CASE "
    '        sSQL += "  WHEN COD_RENDITA IS NULL THEN 'ND' "
    '        sSQL += "  ELSE CAST (COD_RENDITA AS VARCHAR) "
    '        sSQL += " END, "
    '        sSQL += "  -1.0 AS ICICALCOLATOACCONTO, -1.0 AS ICICALCOLATOSALDO, "
    '        sSQL += " NUMEROUTILIZZATORI=CASE"
    '        sSQL += "  WHEN NUMEROUTILIZZATORI IS NULL THEN '0'"
    '        sSQL += "  ELSE CAST(NUMEROUTILIZZATORI AS VARCHAR) "
    '        sSQL += " END, "
    '        sSQL += " TP_LEGAME_ACCERTAMENTO.ID_IMMOBILE_DICHIARATO AS ID_IMMOBILE_ORIGINALE_DICHIARATO,"
    '        sSQL += " CODVIA AS CODICE_VIA,"
    '        'SSQL += " 0 AS CALCOLA_INTERESSI,"
    '        sSQL += " (SELECT COUNT(*) FROM DETTAGLIO_VOCI_ACCERTAMENTI WHERE ID_PROVVEDIMENTO=TAB_PROCEDIMENTI.ID_PROVVEDIMENTO AND COD_VOCE=99 AND ID_LEGAME=TP_IMMOBILI_ACCERTATI_ACCERTAMENTI.ID_LEGAME) AS CALCOLA_INTERESSI,"
    '        sSQL += " DESCTIPORENDITA=CASE  "
    '        sSQL += " WHEN " & NOME_DATABASE_ICI & ".DBO.TIPO_RENDITA.SIGLA IS NULL THEN '' "
    '        sSQL += " ELSE " & NOME_DATABASE_ICI & ".DBO.TIPO_RENDITA.SIGLA  "
    '        sSQL += "END, "
    '        sSQL += " MOTIVAZIONE AS DESC_SANZIONE, "
    '        sSQL += " TP_IMMOBILI_ACCERTATI_ACCERTAMENTI.ID_IMMOBILE_ACCERTATO"
    '        'DIPE 14/06/2010
    '        sSQL += " ,CODCOMUNE, COMUNE, ESPCIVICO, INTERNO, NUMEROCIVICO, PIANO, SCALA,INTERNO, BARRATO, MESIESCLUSIONEESENZIONE"
    '        sSQL += " ,FLAG_ESENTE,MESIRIDUZIONE"
    '        sSQL += " ,DIFFIMPOSTA,TOTALE"
    '        sSQL += " ,DETTAGLIO_VOCI_ACCERTAMENTI.COD_TIPO_PROVVEDIMENTO"
    '        ' imu
    '        sSQL += " ,ICI_VALORE_ALIQUOTA"
    '        '*** 20120530 - IMU ***
    '        sSQL += ",COLTIVATOREDIRETTO,NUMEROFIGLI,100 AS PERCENTCARICOFIGLI"
    '        '*** ***
    '        sSQL += " FROM TAB_PROCEDIMENTI "
    '        sSQL += " INNER JOIN TP_IMMOBILI_ACCERTATI_ACCERTAMENTI"
    '        sSQL += " ON TAB_PROCEDIMENTI.ID_PROCEDIMENTO = TP_IMMOBILI_ACCERTATI_ACCERTAMENTI.ID_PROCEDIMENTO"
    '        'DIPE 09/06/2010
    '        sSQL += " INNER JOIN TP_LEGAME_ACCERTAMENTO ON TP_IMMOBILI_ACCERTATI_ACCERTAMENTI.ID_IMMOBILE_ACCERTATO = TP_LEGAME_ACCERTAMENTO.ID_IMMOBILE_ACCERTATO "
    '        sSQL += " AND TP_IMMOBILI_ACCERTATI_ACCERTAMENTI.ID_PROCEDIMENTO = TP_LEGAME_ACCERTAMENTO.ID_PROCEDIMENTO "
    '        sSQL += " AND TP_IMMOBILI_ACCERTATI_ACCERTAMENTI.ID_LEGAME = TP_LEGAME_ACCERTAMENTO.ID_LEGAME"

    '        sSQL += " LEFT OUTER JOIN " & NOME_DATABASE_ICI & ".DBO.TIPO_RENDITA"
    '        sSQL += " ON TP_IMMOBILI_ACCERTATI_ACCERTAMENTI.CODRENDITA = " & NOME_DATABASE_ICI & ".DBO.TIPO_RENDITA.COD_RENDITA"
    '        sSQL += " LEFT OUTER JOIN " & NOME_DATABASE_ICI & ".DBO.TBLPOSSESSO"
    '        sSQL += " ON TP_IMMOBILI_ACCERTATI_ACCERTAMENTI.TIPOPOSSESSO = " & NOME_DATABASE_ICI & ".DBO.TBLPOSSESSO.TIPOPOSSESSO"

    '        sSQL += " LEFT OUTER JOIN DETTAGLIO_VOCI_ACCERTAMENTI"
    '        sSQL += " ON DETTAGLIO_VOCI_ACCERTAMENTI.ID_PROVVEDIMENTO = TAB_PROCEDIMENTI.ID_PROVVEDIMENTO "
    '        sSQL += " AND DETTAGLIO_VOCI_ACCERTAMENTI.COD_VOCE<>99"
    '        sSQL += " AND DETTAGLIO_VOCI_ACCERTAMENTI.ID_LEGAME = TP_IMMOBILI_ACCERTATI_ACCERTAMENTI.ID_LEGAME"

    '        sSQL += " WHERE(COD_TRIBUTO = 8852)"
    '        sSQL += " AND COD_CONTRIBUENTE=" & codContribuente
    '        sSQL += " AND ANNO=" & annoSel
    '        sSQL += " AND TAB_PROCEDIMENTI.ID_PROVVEDIMENTO=" & ID_Provvedimento
    '        sSQL += " AND (YEAR(DATAFINE) IS NULL OR YEAR(DATAFINE)>=" & annoAccer & ")"
    '        sSQL += " ORDER BY TP_IMMOBILI_ACCERTATI_ACCERTAMENTI.ID_LEGAME, FOGLIO, NUMERO, SUBALTERNO"
    '        objDBManager = New DBManager
    '        If objDBManager.Initialize( ConstSession.StringConnection)) Then
    '            ds = objDBManager.GetPrivateDataSet(sSQL)
    '            If ds.Tables(0).Rows.Count > 0 Then
    '                For i = 0 To ds.Tables(0).Rows.Count - 1
    '                    ds.Tables(0).Rows(i).Item("progressivo") = (i + 1)

    '                    If Not ds.Tables(0).Rows(i).Item("AL") Is DBNull.Value Then

    '                        If CDate(ds.Tables(0).Rows(i).Item("AL")).Date = DateTime.MinValue.Date Or CDate(ds.Tables(0).Rows(i).Item("AL")).Date = DateTime.MaxValue.Date Then
    '                            ds.Tables(0).Rows(i).Item("AL") = ""
    '                        End If
    '                    Else
    '                        ds.Tables(0).Rows(i).Item("AL") = ""
    '                    End If
    '                    'If Not ds.Tables(0).Rows(i).Item("Rendita_Valore") Is DBNull.Value Then
    '                    '    ds.Tables(0).Rows(i).Item("Rendita_Valore") = Replace(ds.Tables(0).Rows(i).Item("Rendita_Valore"), ".", ",")
    '                    'End If
    '                    If Not ds.Tables(0).Rows(i).Item("Rendita") Is DBNull.Value Then
    '                        ds.Tables(0).Rows(i).Item("Rendita") = Replace(ds.Tables(0).Rows(i).Item("Rendita"), ".", ",")
    '                    Else
    '                        ds.Tables(0).Rows(i).Item("Rendita") = 0
    '                    End If
    '                    If ds.Tables(0).Rows(i).Item("zona") Is DBNull.Value Then
    '                        ds.Tables(0).Rows(i).Item("zona") = ""
    '                    End If
    '                    If Not ds.Tables(0).Rows(i).Item("consistenza") Is DBNull.Value Then
    '                        ds.Tables(0).Rows(i).Item("consistenza") = Replace(ds.Tables(0).Rows(i).Item("consistenza"), ".", ",")
    '                    End If
    '                    If Not ds.Tables(0).Rows(i).Item("IdImmobilePertinenza") Is DBNull.Value Then
    '                        If ds.Tables(0).Rows(i).Item("IdImmobilePertinenza") > 0 And CStr(ds.Tables(0).Rows(i).Item("categoria")).StartsWith("C") Then
    '                            'ds.Tables(0).Select("id_immobile_accertato='23221'")(0)("IDLEGAME")
    '                            'miodatarow = ds.Tables(0).Select("id_immobile_accertato='" & ds.Tables(0).Rows(i).Item("IdImmobilePertinenza") & "'")
    '                            miodatarow = ds.Tables(0).Select("IDLEGAME='" & ds.Tables(0).Rows(i).Item("IdImmobilePertinenza") & "'")
    '                            If miodatarow.Length > 0 Then
    '                                If miodatarow(0).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO") > 0 Then
    '                                    ds.Tables(0).Rows(i).Item("IdImmobilePertinenza") = miodatarow(0).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO")
    '                                    ds.Tables(0).Rows(i).Item("FLAG_PERTINENZA") = "1"
    '                                Else
    '                                    ds.Tables(0).Rows(i).Item("IdImmobilePertinenza") = ds.Tables(0).Rows(i).Item("IdImmobilePertinenza") 'miodatarow(0).Item("IDLEGAME")
    '                                    ds.Tables(0).Rows(i).Item("FLAG_PERTINENZA") = "1"
    '                                End If
    '                            Else
    '                                ds.Tables(0).Rows(i).Item("IdImmobilePertinenza") = ds.Tables(0).Rows(i).Item("IdImmobilePertinenza") 'miodatarow(0).Item("IDLEGAME")
    '                                ds.Tables(0).Rows(i).Item("FLAG_PERTINENZA") = "1"
    '                            End If
    '                            'ds.Tables(0).Rows(i).Item("IdImmobilePertinenza") = ds.Tables(0).Rows(i).Item("idlegame") 'Session("IdImmobileDiPertinenza")
    '                        Else
    '                            'Session("IdImmobileDiPertinenza") = -1
    '                        End If
    '                    Else
    '                        ds.Tables(0).Rows(i).Item("FLAG_PERTINENZA") = "0"
    '                        ds.Tables(0).Rows(i).Item("IdImmobilePertinenza") = 0
    '                    End If
    '                    If ds.Tables(0).Rows(i).Item("FLAG_PRINCIPALE") <> 0 Then
    '                        HttpContext.Current.Session("IdImmobileDiPertinenza") = ds.Tables(0).Rows(i).Item("FLAG_PRINCIPALE")
    '                    End If

    '                    'vale sempre la regola valori invertiti perchè in dichiarato dati possesso
    '                    '0=SI
    '                    '1=NO
    '                    '2=non compilato
    '                    'per cui si deve invertire il valore che arriva perchè altrimenti il checkbox non ha il valore corretto
    '                    If ds.Tables(0).Rows(i).Item("FLAG_RIDOTTO") = 1 Then
    '                        ds.Tables(0).Rows(i).Item("FLAG_RIDOTTO") = 0
    '                    Else
    '                        ds.Tables(0).Rows(i).Item("FLAG_RIDOTTO") = 1
    '                    End If

    '                    'in calcola_interessi viene inserito il numero di interessi se >0 allora 
    '                    'setto a true il calcolo interessi nella griglia immobili accertati
    '                    If ds.Tables(0).Rows(i).Item("CALCOLA_INTERESSI") > 0 Then
    '                        ds.Tables(0).Rows(i).Item("CALCOLA_INTERESSI") = 1
    '                    Else
    '                        ds.Tables(0).Rows(i).Item("CALCOLA_INTERESSI") = 0
    '                    End If

    '                    'gestione sanzioni
    '                    If ds.Tables(0).Rows(i).Item("IDSANZIONI") <> "-1" Then
    '                        ds.Tables(0).Rows(i).Item("IDSANZIONI") = ds.Tables(0).Rows(i).Item("IDSANZIONI") & "#" & ds.Tables(0).Rows(i).Item("cod_tipo_provvedimento")
    '                    End If
    '                    If Not ds.Tables(0).Rows(i).Item("DESC_SANZIONE") Is DBNull.Value Then
    '                        ds.Tables(0).Rows(i).Item("DESC_SANZIONE") = ds.Tables(0).Rows(i).Item("DESC_SANZIONE")
    '                    End If

    '                    '*** 20120709 - IMU per AF e LC devo usare il campo valore ***
    '                    Dim ValoreDich As Double = 0
    '                    If Not ds.Tables(0).Rows(i).Item("Rendita_Valore") Is DBNull.Value Then
    '                        ValoreDich = Replace(ds.Tables(0).Rows(i).Item("Rendita_Valore"), ".", ",")
    '                    End If
    '                    If ds.Tables(0).Rows(i).Item("COLTIVATOREDIRETTO") Is DBNull.Value Then
    '                        ds.Tables(0).Rows(i).Item("COLTIVATOREDIRETTO") = False
    '                    End If

    '                    Dim valore As String
    '                    '*** 20120530 - IMU ***
    '                    Dim FncValore As New ComPlusInterface.FncICI
    '                    valore = FncValore.CalcoloValore(ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, sIdEnte, annoAccer, ds.Tables(0).Rows(i).Item("TR"), ds.Tables(0).Rows(i).Item("categoria"), ds.Tables(0).Rows(i).Item("classe"), ds.Tables(0).Rows(i).Item("zona"), ds.Tables(0).Rows(i).Item("Rendita"), ValoreDich, ds.Tables(0).Rows(i).Item("consistenza"), CDate("01/01/" & annoAccer), ds.Tables(0).Rows(i).Item("COLTIVATOREDIRETTO"))
    '                    If valore <= 0 Then
    '                        If Not ds.Tables(0).Rows(i).Item("Rendita_Valore") Is DBNull.Value Then
    '                            valore = Replace(ds.Tables(0).Rows(i).Item("Rendita_Valore"), ".", ",")
    '                        End If
    '                    End If
    '                    '*** ***
    '                    ds.Tables(0).Rows(i).Item("Rendita_Valore") = valore

    '                    ''ho ordinato per legame quindi se l'idlegame è divers dal progressivo lo forzo uguale al progressivo
    '                    ''in teoria è diverso solo se ho tolto immobili per l'anno
    '                    'If ds.Tables(0).Rows(i).Item("idlegame") <> ds.Tables(0).Rows(i).Item("progressivo") Then
    '                    '    ds.Tables(0).Rows(i).Item("idlegame") = ds.Tables(0).Rows(i).Item("progressivo")
    '                    'End If
    '                Next
    '            End If
    '            dt = ds.Tables(0)
    '        Else
    '            Throw New Exception("Errore inizializzazione workflow")
    '        End If
    '    Catch ex As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.RicercaAccertatoICI.errore: ", ex)
    '    Finally
    '        ds.Dispose()
    '        dt.Dispose()
    '        RicercaAccertatoICI = dt
    '        objDBManager.Kill()
    '    End Try
    'End Function
    Public Function RicercaDicAccICI(Tipo As String, ByVal sIdEnte As String, ByVal codContribuente As Integer, ByVal annoSel As Integer, ByVal ID_Provvedimento As Integer, ByVal annoAccer As Integer, ByVal myStringConnection As String, Optional ByVal provenienza As String = "acc") As objUIICIAccert()
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myArray As New ArrayList()
        Dim ListUIAcc() As objUIICIAccert
        Dim oReplace As New OPENgovTIA.generalClass.generalFunction
        Try
            'NOME_DATABASE_ICI = ConfigurationManager.AppSettings("NOME_DATABASE_ICI")
            'objSessione = New CreateSessione(sParametroENV, sUserName, sApplicazione)
            'If Not objSessione.CreaSessione(sUserName, strWFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If
            ''strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = codContribuente
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOSEL", SqlDbType.NVarChar)).Value = annoSel
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOACCER", SqlDbType.NVarChar)).Value = annoAccer
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROVVEDIMENTO", SqlDbType.Int)).Value = ID_Provvedimento
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZA", SqlDbType.NVarChar)).Value = provenienza
            If Tipo = "D" Then
                cmdMyCommand.CommandText = "prc_GetRicercaDichiaratoICI"
            Else
                cmdMyCommand.CommandText = "prc_GetRicercaAccertatoICI"
            End If
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "IMMOBILI")
            myAdapter.Dispose()
            If myDataSet.Tables(0).DefaultView.Count <= 0 Then
                Return Nothing
            Else
                For Each myRow As DataRow In myDataSet.Tables(0).Rows
                    Dim myUI As New objUIICIAccert
                    If Not IsDBNull(myRow("id_situazione_finale")) Then
                        myUI.Id = myRow("id_situazione_finale")
                    End If
                    If Not IsDBNull(myRow("idlegame")) Then
                        myUI.IdLegame = myRow("idlegame")
                    End If
                    If Not IsDBNull(myRow("progressivo")) Then
                        myUI.Progressivo = myRow("progressivo")
                    End If
                    If Not IsDBNull(myRow("COD_CONTRIBUENTE")) Then
                        myUI.IdContribuente = myRow("COD_CONTRIBUENTE")
                    End If
                    If Not IsDBNull(myRow("ANNO")) Then
                        myUI.Anno = myRow("ANNO")
                    End If
                    If Not IsDBNull(myRow("CODTRIBUTO")) Then
                        myUI.Tributo = myRow("CODTRIBUTO")
                    End If
                    If Not IsDBNull(myRow("MESI_possesso")) Then
                        myUI.MesiPossesso = myRow("MESI_possesso")
                    End If
                    If Not IsDBNull(myRow("NUMERO_MESI_ACCONTO")) Then
                        myUI.AccMesi = myRow("NUMERO_MESI_ACCONTO")
                    End If
                    If Not IsDBNull(myRow("NUMERO_MESI_TOTALI")) Then
                        myUI.Mesi = myRow("NUMERO_MESI_TOTALI")
                    End If
                    If Not IsDBNull(myRow("NUMERO_UTILIZZATORI")) Then
                        myUI.NUtilizzatori = myRow("NUMERO_UTILIZZATORI")
                    End If
                    myUI.FlagPrincipale = 0
                    If Not IsDBNull(myRow("FLAG_PRINCIPALE")) Then
                        If myRow("FLAG_PRINCIPALE").ToString <> "0" Then
                            myUI.FlagPrincipale = 1
                        Else
                            If Not IsDBNull(myRow("COD_IMMOBILE_PERTINENZA")) Then
                                If myRow("COD_IMMOBILE_PERTINENZA").ToString.Length > 0 Then
                                    If StringOperation.FormatInt(myRow("COD_IMMOBILE_PERTINENZA")) > 0 Then
                                        myUI.FlagPrincipale = 2
                                    End If
                                End If
                            End If
                        End If
                    End If
                    If Not IsDBNull(myRow("PERC_POSSESSO")) Then
                        myUI.PercPossesso = myRow("PERC_POSSESSO")
                    End If
                    If Not IsDBNull(myRow("COD_ENTE")) Then
                        myUI.IdEnte = myRow("COD_ENTE")
                    End If
                    If Not IsDBNull(myRow("CARATTERISTICA")) Then
                        myUI.Caratteristica = myRow("CARATTERISTICA")
                    End If
                    If Not IsDBNull(myRow("VIA")) Then
                        myUI.Via = myRow("VIA")
                    End If
                    If Not IsDBNull(myRow("NUMEROCIVICO")) Then
                        myUI.NCivico += " " & myRow("NUMEROCIVICO")
                    End If
                    If Not IsDBNull(myRow("SEZIONE")) Then
                        myUI.Sezione = myRow("SEZIONE")
                    End If
                    If Not IsDBNull(myRow("FOGLIO")) Then
                        myUI.Foglio = myRow("FOGLIO")
                    End If
                    If Not IsDBNull(myRow("NUMERO")) Then
                        myUI.Numero = myRow("NUMERO")
                    End If
                    If Not IsDBNull(myRow("SUBALTERNO")) Then
                        myUI.Subalterno = myRow("SUBALTERNO")
                    End If
                    If Not IsDBNull(myRow("CATEGORIA")) Then
                        myUI.Categoria = myRow("CATEGORIA")
                    End If
                    If Not IsDBNull(myRow("CLASSE")) Then
                        myUI.Classe = myRow("CLASSE")
                    End If
                    If Not IsDBNull(myRow("FLAG_STORICO")) Then
                        myUI.FlagStorico = myRow("FLAG_STORICO")
                    End If
                    If Not IsDBNull(myRow("FLAG_PROVVISORIO")) Then
                        myUI.FlagProvvisorio = myRow("FLAG_PROVVISORIO")
                    End If
                    If Not IsDBNull(myRow("MESI_POSSESSO")) Then
                        myUI.MesiPossesso = myRow("MESI_POSSESSO")
                    End If
                    If Not IsDBNull(myRow("MESI_ESCL_ESENZIONE")) Then
                        myUI.MesiEsenzione = myRow("MESI_ESCL_ESENZIONE")
                    End If
                    If Not IsDBNull(myRow("MESI_RIDUZIONE")) Then
                        myUI.MesiRiduzione = myRow("MESI_RIDUZIONE")
                    End If
                    If Not IsDBNull(myRow("IMPORTO_DETRAZIONE")) Then
                        myUI.ImpDetrazione = myRow("IMPORTO_DETRAZIONE")
                    End If
                    If Not IsDBNull(myRow("FLAG_POSSEDUTO")) Then
                        myUI.FlagPosseduto = myRow("FLAG_POSSEDUTO")
                    End If
                    If Not IsDBNull(myRow("FLAG_ESENTE")) Then
                        myUI.FlagEsente = myRow("FLAG_ESENTE")
                    End If
                    If Not IsDBNull(myRow("FLAG_RIDUZIONE")) Then
                        myUI.FlagRiduzione = myRow("FLAG_RIDUZIONE")
                    End If
                    If Not IsDBNull(myRow("ID")) Then
                        myUI.IdImmobile = myRow("ID")
                    End If
                    If Not IsDBNull(myRow("COD_IMMOBILE_PERTINENZA")) Then
                        myUI.IdImmobilePertinenza = myRow("COD_IMMOBILE_PERTINENZA")
                    End If
                    If Not IsDBNull(myRow("DAL")) Then
                        myUI.Dal = oReplace.FormattaData(myRow("DAL"), "G")
                        myUI.DataInizio = oReplace.FormattaData(myRow("DAL"), "G")
                    End If
                    If Not IsDBNull(myRow("AL")) Then
                        myUI.Al = oReplace.FormattaData(myRow("AL"), "G")
                    End If
                    If Not IsDBNull(myRow("TIPO_RENDITA")) Then
                        myUI.TipoRendita = myRow("TIPO_RENDITA")
                    End If
                    If Not IsDBNull(myRow("IDTIPOUTILIZZO")) Then
                        myUI.IdTipoUtilizzo = myRow("IDTIPOUTILIZZO")
                    End If
                    If Not IsDBNull(myRow("IDTIPOPOSSESSO")) Then
                        myUI.IdTipoPossesso = myRow("IDTIPOPOSSESSO")
                    End If
                    If Not IsDBNull(myRow("TITPOSSESSO")) Then
                        myUI.TitPossesso = myRow("TITPOSSESSO")
                    End If
                    '*** ***
                    If Not IsDBNull(myRow("ZONA")) Then
                        myUI.Zona = myRow("ZONA")
                    End If
                    If Not IsDBNull(myRow("consistenza")) Then
                        myUI.Consistenza = myRow("consistenza")
                    End If
                    If Not IsDBNull(myRow("ABITAZIONEPRINCIPALEATTUALE")) Then
                        myUI.AbitazionePrincipaleAttuale = myRow("ABITAZIONEPRINCIPALEATTUALE")
                    End If
                    If Not IsDBNull(myRow("RENDITA")) Then
                        myUI.Rendita = myRow("RENDITA")
                    End If
                    Dim FncValore As New ComPlusInterface.FncICI
                    If Not IsDBNull(myRow("COLTIVATOREDIRETTO")) Then
                        myUI.IsColtivatoreDiretto = myRow("COLTIVATOREDIRETTO")
                    Else
                        myUI.IsColtivatoreDiretto = False
                    End If
                    If Not IsDBNull(myRow("NUMEROFIGLI")) Then
                        myUI.NumeroFigli = myRow("NUMEROFIGLI")
                    End If
                    If Not IsDBNull(myRow("PERCENTCARICOFIGLI")) Then
                        myUI.PercentCaricoFigli = myRow("PERCENTCARICOFIGLI")
                    End If
                    Dim nValoreDich As Double = 0
                    If Not IsDBNull(myRow("valore")) Then
                        nValoreDich = myRow("valore")
                    End If
                    myUI.Valore = nValoreDich 'FncValore.CalcoloValore(ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, myUI.IdEnte, myUI.Anno, myUI.TipoRendita, myUI.Categoria, myUI.Classe, myUI.Zona, myUI.Rendita, nValoreDich, myUI.Consistenza, myUI.Dal, myUI.IsColtivatoreDiretto)
                    myUI.ValoreReale = myUI.Valore
                    If Not IsDBNull(myRow("TIPOTASI")) Then
                        myUI.TipoTasi = myRow("TIPOTASI")
                    End If
                    If Not IsDBNull(myRow("DESCRTIPOTASI")) Then
                        myUI.DescrTipoTasi = myRow("DESCRTIPOTASI")
                    End If
                    If Not IsDBNull(myRow("IDCONTRIBUENTECALCOLO")) Then
                        myUI.IdContribuenteCalcolo = myRow("IDCONTRIBUENTECALCOLO")
                    End If
                    If Not IsDBNull(myRow("ICI_ACCONTO_SENZA_DETRAZIONE")) Then
                        myUI.AccSenzaDetrazione = myRow("ICI_ACCONTO_SENZA_DETRAZIONE")
                    End If
                    If Not IsDBNull(myRow("ICI_ACCONTO_DETRAZIONE_APPLICATA")) Then
                        myUI.AccDetrazioneApplicata = myRow("ICI_ACCONTO_DETRAZIONE_APPLICATA")
                    End If
                    If Not IsDBNull(myRow("ICICALCOLATOACCONTO")) Then
                        myUI.AccDovuto = myRow("ICICALCOLATOACCONTO")
                    End If
                    If Not IsDBNull(myRow("ICI_ACCONTO_DETRAZIONE_RESIDUA")) Then
                        myUI.AccDetrazioneResidua = myRow("ICI_ACCONTO_DETRAZIONE_RESIDUA")
                    End If
                    If Not IsDBNull(myRow("ICI_TOTALE_SENZA_DETRAZIONE")) Then
                        myUI.SalSenzaDetrazione = myRow("ICI_TOTALE_SENZA_DETRAZIONE")
                    End If
                    If Not IsDBNull(myRow("ICI_TOTALE_DETRAZIONE_APPLICATA")) Then
                        myUI.SalDetrazioneApplicata = myRow("ICI_TOTALE_DETRAZIONE_APPLICATA")
                    End If
                    If Not IsDBNull(myRow("ICICALCOLATOSALDO")) Then
                        myUI.SalDovuto = myRow("ICICALCOLATOSALDO")
                    End If
                    If Not IsDBNull(myRow("ICI_TOTALE_DETRAZIONE_RESIDUA")) Then
                        myUI.SalDetrazioneResidua = myRow("ICI_TOTALE_DETRAZIONE_RESIDUA")
                    End If
                    If Not IsDBNull(myRow("ICI_DOVUTA_SENZA_DETRAZIONE")) Then
                        myUI.TotSenzaDetrazione = myRow("ICI_DOVUTA_SENZA_DETRAZIONE")
                    End If
                    If Not IsDBNull(myRow("ICI_DOVUTA_DETRAZIONE_SALDO")) Then
                        myUI.TotDetrazioneApplicata = myRow("ICI_DOVUTA_DETRAZIONE_SALDO")
                    End If
                    If Not IsDBNull(myRow("ICICALCOLATO")) Then
                        myUI.TotDovuto = myRow("ICICALCOLATO")
                    End If
                    If Not IsDBNull(myRow("ICI_DOVUTA_DETRAZIONE_RESIDUA")) Then
                        myUI.TotDetrazioneResidua = myRow("ICI_DOVUTA_DETRAZIONE_RESIDUA")
                    End If
                    If Not IsDBNull(myRow("ID_ALIQUOTA")) Then
                        myUI.IdAliquota = myRow("ID_ALIQUOTA")
                    End If
                    If Not IsDBNull(myRow("ICI_VALORE_ALIQUOTA")) Then
                        myUI.Aliquota = myRow("ICI_VALORE_ALIQUOTA")
                    End If
                    If Not IsDBNull(myRow("ICI_VALORE_ALIQUOTA_STATALE")) Then
                        myUI.AliquotaStatale = myRow("ICI_VALORE_ALIQUOTA_STATALE")
                    End If
                    If Not IsDBNull(myRow("ICI_DOVUTA_ACCONTO_STATALE")) Then
                        myUI.AccDovutoStatale = myRow("ICI_DOVUTA_ACCONTO_STATALE")
                    End If
                    If Not IsDBNull(myRow("ICI_ACCONTO_DETRAZIONE_APPLICATA_STATALE")) Then
                        myUI.AccDetrazioneApplicataStatale = myRow("ICI_ACCONTO_DETRAZIONE_APPLICATA_STATALE")
                    End If
                    If Not IsDBNull(myRow("ICI_ACCONTO_DETRAZIONE_RESIDUA_STATALE")) Then
                        myUI.AccDetrazioneResiduaStatale = myRow("ICI_ACCONTO_DETRAZIONE_RESIDUA_STATALE")
                    End If
                    If Not IsDBNull(myRow("ICI_TOTALE_DOVUTA_STATALE")) Then
                        myUI.SalDovutoStatale = myRow("ICI_TOTALE_DOVUTA_STATALE")
                    End If
                    If Not IsDBNull(myRow("ICI_TOTALE_DETRAZIONE_APPLICATA_STATALE")) Then
                        myUI.SalDetrazioneApplicataStatale = myRow("ICI_TOTALE_DETRAZIONE_APPLICATA_STATALE")
                    End If
                    If Not IsDBNull(myRow("ICI_TOTALE_DETRAZIONE_RESIDUA_STATALE")) Then
                        myUI.SalDetrazioneResiduaStatale = myRow("ICI_TOTALE_DETRAZIONE_RESIDUA_STATALE")
                    End If
                    If Not IsDBNull(myRow("ICI_DOVUTA_SALDO_STATALE")) Then
                        myUI.TotDovutoStatale = myRow("ICI_DOVUTA_SALDO_STATALE")
                    End If
                    If Not IsDBNull(myRow("ICI_DOVUTA_DETRAZIONE_SALDO_STATALE")) Then
                        myUI.TotDetrazioneApplicataStatale = myRow("ICI_DOVUTA_DETRAZIONE_SALDO_STATALE")
                    End If
                    If Not IsDBNull(myRow("ICI_DOVUTA_DETRAZIONE_RESIDUA_STATALE")) Then
                        myUI.TotDetrazioneResiduaStatale = myRow("ICI_DOVUTA_DETRAZIONE_RESIDUA_STATALE")
                    End If
                    If Not IsDBNull(myRow("DIFFIMPOSTA")) Then
                        myUI.DiffImposta = myRow("DIFFIMPOSTA")
                    End If
                    If Not IsDBNull(myRow("TOTALE")) Then
                        myUI.Totale = myRow("TOTALE")
                    End If
                    If Not IsDBNull(myRow("idprocedimento")) Then
                        myUI.IdProcedimento = myRow("idprocedimento")
                    End If
                    If Not IsDBNull(myRow("calcolainteressi")) Then
                        myUI.CalcolaInteressi = myRow("calcolainteressi")
                    End If
                    If Not IsDBNull(myRow("IdSanzioni")) Then
                        myUI.IdSanzioni = myRow("IdSanzioni")
                    End If
                    If Not IsDBNull(myRow("DescrSanzioni")) Then
                        myUI.DescrSanzioni = myRow("DescrSanzioni")
                    End If
                    If Not IsDBNull(myRow("IMPSANZIONI")) Then
                        myUI.ImpSanzioni = myRow("IMPSANZIONI")
                    End If
                    If Not IsDBNull(myRow("IMPINTERESSI")) Then
                        myUI.ImpInteressi = myRow("IMPINTERESSI")
                    End If
                    myUI.IdRiferimento = 0
                    myUI.Provenienza = ""
                    myUI.Protocollo = "0"
                    myUI.MeseInizio = 0
                    myUI.DataScadenza = ""
                    myArray.Add(myUI)
                Next
                ListUIAcc = CType(myArray.ToArray(GetType(objUIICIAccert)), objUIICIAccert())
            End If
            Return ListUIAcc
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.RicercaDicAccICI.errore: ", ex)
            Return Nothing
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function

    'Public Function RicercaDichiaratoICI(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sApplicazione As String, ByVal sIdEnte As String, ByVal ID_Provvedimento As Integer) As DataTable
    '    'Dim objSessione As CreateSessione
    '    Dim sSQL, strWFErrore As String
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    Dim NOME_DATABASE_ICI As String
    '    Dim objDBManager As DBManager
    '    Dim ds As DataSet
    '    'Dim dt As DataTable
    '    Dim i As Integer
    '    Dim dt As New DataTable("IMMOBILI")
    '    Dim miodatarow As DataRow()
    '    Try
    '        NOME_DATABASE_ICI = ConfigurationManager.AppSettings("NOME_DATABASE_ICI")
    '        objSessione = New CreateSessione(sParametroENV, sUserName, sApplicazione)
    '        If Not objSessione.CreaSessione(sUserName, strWFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

    '        sSQL = "SELECT"
    '        sSQL += " progressivo,"
    '        sSQL += " datainizio,"
    '        sSQL += " datafine,"
    '        sSQL += " foglio,"
    '        sSQL += " numero,"
    '        sSQL += " subalterno,"
    '        sSQL += " codcategoriacatastale,"
    '        sSQL += " codclasse,"
    '        sSQL += " consistenza,"
    '        sSQL += " " & NOME_DATABASE_ICI & ".dbo.TIPO_RENDITA.sigla as tr,"
    '        sSQL += " valoreimmobile,"
    '        sSQL += " ICICalcolato,"
    '        sSQL += " id_legame,"
    '        sSQL += " percpossesso,"
    '        sSQL += " coltivatorediretto,"
    '        sSQL += " numerofigli"
    '        sSQL += " from tp_immobili_accertamenti"
    '        sSQL += " inner join TAB_PROCEDIMENTI"
    '        sSQL += " on TAB_PROCEDIMENTI.ID_PROCEDIMENTO = tp_immobili_accertamenti.ID_PROCEDIMENTO"
    '        sSQL += " inner join " & NOME_DATABASE_ICI & ".dbo.TIPO_RENDITA"
    '        sSQL += " on " & NOME_DATABASE_ICI & ".dbo.TIPO_RENDITA.cod_rendita=codrendita"
    '        sSQL += " where ID_Provvedimento = " & ID_Provvedimento
    '        sSQL += " and ente='" & sIdEnte & "'"
    '        'Se progressivo=0 gli immobili provengono da un accertamente per il quale non si è calcolato
    '        'il preaccertamento 
    '        sSQL += " and (progressivo<>0 or progressivo is null)"
    '        sSQL += " order by foglio,numero,subalterno"

    '        objDBManager = New DBManager
    '        If objDBManager.Initialize( ConstSession.StringConnection)) Then
    '            ds = objDBManager.GetPrivateDataSet(sSQL)
    '            dt = ds.Tables(0)
    '        Else
    '            Throw New Exception("Errore inizializzazione workflow")
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.RicercaDichiaratoICI.errore: ", ex)
    '    Finally
    '        ds.Dispose()
    '        dt.Dispose()
    '        RicercaDichiaratoICI = dt
    '        objDBManager.Kill()
    '    End Try
    'End Function
    Public Function PrepareArticoliForCalcolo(ByVal oAccertatoInserito() As ObjArticoloAccertamento, ByVal myArticolo As ObjArticolo, ByVal sTipoTassazione As String, ByRef bTrovato As Boolean) As ObjArticolo()
        Dim oListArticoli() As ObjArticolo
        Dim myArtAcc As New ObjArticoloAccertamento
        Dim myArray As New ArrayList
        Dim x As Integer

        Try
            bTrovato = False
            'converto in oggetto per calcolo
            For Each myArtAcc In oAccertatoInserito
                myArray.Add(ArticoloAccertamentoTOArticolo(myArtAcc, sTipoTassazione))
            Next
            oListArticoli = CType(myArray.ToArray(GetType(ObjArticolo)), ObjArticolo())
            'se sono un una nuova posizione aggiorno l'id con un numero fittizio perché non serve + identificarlo come nuovo
            If myArticolo.Id = -1 Then
                'ORDINO PER partita+id
                Array.Sort(oListArticoli, New Utility.Comparatore(New String() {"Id"}, New Boolean() {Utility.TipoOrdinamento.Decrescente}))
                For x = 0 To oListArticoli.GetUpperBound(0)
                    myArticolo.Id = oListArticoli(x).Id + 1
                    myArticolo.IdArticolo = myArticolo.Id
                    Exit For
                Next
            End If
            'se sono sulla posizione di videata la aggiorno con i dati nuovi altrimenti la aggiungo
            For x = 0 To oListArticoli.GetUpperBound(0)
                If oListArticoli(x).Id = myArticolo.Id Then
                    oListArticoli(x) = myArticolo
                    bTrovato = True
                Else
                    'forzo le date di inizio e fine se mancano
                    If oListArticoli(x).tDataInizio = DateTime.MinValue Then
                        oListArticoli(x).tDataInizio = "01/01" & myArticolo.sAnno
                    End If
                    If oListArticoli(x).tDataFine = DateTime.MinValue Then
                        oListArticoli(x).tDataFine = "31/12/" & myArticolo.sAnno
                    End If
                    If Not oListArticoli(x).oRiduzioni Is Nothing Then
                        'mi appoggio ad operatore per utilizzarlo come ordinamento
                        Dim n As Integer
                        For n = 0 To oListArticoli(x).oRiduzioni.GetUpperBound(0)
                            oListArticoli(x).sOperatore += oListArticoli(x).oRiduzioni(n).sCodice + "|"
                        Next
                    End If
                    If Not oListArticoli(x).oDetassazioni Is Nothing Then
                        'mi appoggio ad operatore per utilizzarlo come ordinamento
                        Dim n As Integer
                        For n = 0 To oListArticoli(x).oDetassazioni.GetUpperBound(0)
                            oListArticoli(x).sOperatore += oListArticoli(x).oDetassazioni(n).sCodice + "|"
                        Next
                    End If
                End If
            Next
            Return oListArticoli
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.PprepareArticoliForCalcolo.errore: ", ex)
            Return Nothing
        End Try
    End Function

    'Public Function RicercaAccertatoTARSU(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sApplicazione As String, ByVal nIdProvvedimento As Integer, ByRef bAnnoMod As Boolean, Optional ByVal annoAccSelezionato As Integer = -1, Optional ByVal bVisualizzazione As Boolean = False) As OggettoArticoloRuolo()
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim sSQL, WFErrore As String
    '    Dim oListAccertamento() As OggettoArticoloRuolo
    '    Dim oAccertamentoSingolo As OggettoArticoloRuolo
    '    Dim oListOggettoRiduzione() As OggettoRiduzione
    '    Dim oOggettoRiduzione As OggettoRiduzione
    '    Dim x As Integer = -1
    '    Dim i As Integer = -1
    '    Dim drDati As SqlClient.SqlDataReader
    '    Dim drDatiRid As SqlClient.SqlDataReader
    '    Dim NOME_DATABASE_TARSU As String

    '    Dim RemoRuoloTARSU As IRuoloTARSU
    '    Dim TypeOfRI As Type = GetType(IRuoloTARSU)
    '    Dim nBimestri As Integer

    '    Try
    '        RemoRuoloTARSU = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLServizioRuoloTARSU"))
    '        NOME_DATABASE_TARSU = ConfigurationManager.AppSettings("NOME_DATABASE_TARSU")
    '        WFSessione = New OPENUtility.CreateSessione(sParametroENV, sUserName, sApplicazione)
    '        If Not WFSessione.CreaSessione(sUserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'sSQL = "SELECT * FROM TBLRUOLOACCERTATO WHERE ID_PROVVEDIMENTO=" & nIdProvvedimento

    '        'recupero anche motivazione sanzione e calcolo interessi
    '        'sSQL = "SELECT TBLRUOLOACCERTATO.*, cod_tipo_provvedimento, cod_voce, motivazione,"
    '        'sSQL += " (SELECT count(*) FROM DETTAGLIO_VOCI_ACCERTAMENTI where id_provvedimento = TBLRUOLOACCERTATO.id_provvedimento and cod_voce=12  and id_legame=TBLRUOLOACCERTATO.ID_LEGAME) as CALCOLA_INTERESSI"
    '        'sSQL += " FROM TBLRUOLOACCERTATO"
    '        'sSQL += " left outer join DETTAGLIO_VOCI_ACCERTAMENTI"
    '        'sSQL += " on DETTAGLIO_VOCI_ACCERTAMENTI.ID_PROVVEDIMENTO = TBLRUOLOACCERTATO.ID_PROVVEDIMENTO"
    '        'sSQL += " and DETTAGLIO_VOCI_ACCERTAMENTI.cod_voce<>12"
    '        'sSQL += " and DETTAGLIO_VOCI_ACCERTAMENTI.id_legame = TBLRUOLOACCERTATO.id_legame"
    '        'sSQL += " WHERE TBLRUOLOACCERTATO.ID_PROVVEDIMENTO = " & nIdProvvedimento
    '        'sSQL += " order by cast(TBLRUOLOACCERTATO.id_legame as int)"
    '        sSQL = "SELECT *"
    '        sSQL += " FROM V_GetTARSUArticoliAcc"
    '        sSQL += " WHERE (ID_PROVVEDIMENTO = " & nIdProvvedimento & ")"
    '        drDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While drDati.Read
    '            x += 1
    '            ReDim Preserve oListAccertamento(x)

    '            oAccertamentoSingolo = New OggettoArticoloRuolo

    '            If Not IsDBNull(drDati("Anno")) Then
    '                oAccertamentoSingolo.Anno = annoAccSelezionato 'drDati("Anno")
    '            End If
    '            If Not IsDBNull(drDati("idCategoria")) Then
    '                oAccertamentoSingolo.Categoria = drDati("idCategoria")
    '            End If
    '            If Not IsDBNull(drDati("Civico")) Then
    '                oAccertamentoSingolo.Civico = drDati("Civico")
    '            End If
    '            If Not IsDBNull(drDati("CodVia")) Then
    '                oAccertamentoSingolo.CodVia = drDati("CodVia")
    '            End If
    '            If Not IsDBNull(drDati("Data_Fine")) Then
    '                If annoAccSelezionato <> -1 And Not bVisualizzazione Then
    '                    'modifico della data UI in base all'anno di accertamento
    '                    If Year(drDati("Data_Fine")) = annoAccSelezionato Then
    '                        oAccertamentoSingolo.DataFine = drDati("Data_Fine")
    '                    Else
    '                        oAccertamentoSingolo.DataFine = CDate(annoAccSelezionato & "-" & Month(drDati("Data_Fine")) & "-" & Day(drDati("Data_Fine")))
    '                        bAnnoMod = True
    '                    End If
    '                Else
    '                    oAccertamentoSingolo.DataFine = drDati("Data_Fine")
    '                End If
    '            End If
    '            If Not IsDBNull(drDati("Data_Inizio")) Then
    '                If annoAccSelezionato <> -1 And Not bVisualizzazione Then
    '                    'modifico della data UI in base all'anno di accertamento
    '                    If Year(drDati("Data_Inizio")) = annoAccSelezionato Then
    '                        oAccertamentoSingolo.DataInizio = drDati("Data_Inizio")
    '                    Else
    '                        oAccertamentoSingolo.DataInizio = CDate(annoAccSelezionato & "-" & Month(drDati("Data_Inizio")) & "-" & Day(drDati("Data_Inizio")))
    '                        bAnnoMod = True
    '                    End If
    '                Else
    '                    oAccertamentoSingolo.DataInizio = drDati("Data_Inizio")
    '                End If
    '            End If
    '            If Not IsDBNull(drDati("idEnte")) Then
    '                oAccertamentoSingolo.Ente = drDati("idEnte")
    '            End If
    '            If Not IsDBNull(drDati("Esponente")) Then
    '                oAccertamentoSingolo.Esponente = drDati("Esponente")
    '            End If
    '            If Not IsDBNull(drDati("Foglio")) Then
    '                oAccertamentoSingolo.Foglio = drDati("Foglio")
    '            End If
    '            If Not IsDBNull(drDati("Id")) Then
    '                oAccertamentoSingolo.Id = drDati("Id")
    '            End If
    '            If Not IsDBNull(drDati("IdContribuente")) Then
    '                oAccertamentoSingolo.IdContribuente = drDati("IdContribuente")
    '            End If

    '            If Not IsDBNull(drDati("Importo_Detassazioni")) Then
    '                oAccertamentoSingolo.ImportoDetassazione = drDati("Importo_Detassazioni")
    '            End If
    '            If Not IsDBNull(drDati("Importo_Forzato")) Then
    '                oAccertamentoSingolo.ImportoForzato = drDati("Importo_Forzato")
    '            End If
    '            If Not IsDBNull(drDati("Importo_Riduzioni")) Then
    '                oAccertamentoSingolo.ImportoRiduzione = drDati("Importo_Riduzioni")
    '            End If
    '            If Not IsDBNull(drDati("Importo")) Then
    '                oAccertamentoSingolo.ImportoRuolo = drDati("Importo")
    '            End If
    '            'recupero tariffa per l'anno di accertamento

    '            If Not bVisualizzazione Then
    '                GetTariffaTarsu(oAccertamentoSingolo, WFSessione)
    '            Else
    '                If Not IsDBNull(drDati("IDTariffa")) Then
    '                    oAccertamentoSingolo.IDTariffa = drDati("IDTariffa")
    '                End If
    '                If Not IsDBNull(drDati("Importo_Tariffa")) Then
    '                    oAccertamentoSingolo.ImpTariffa = drDati("Importo_Tariffa")
    '                End If
    '            End If



    '            If Not IsDBNull(drDati("Interno")) Then
    '                oAccertamentoSingolo.Interno = drDati("Interno")
    '            End If
    '            If Not IsDBNull(drDati("IsTarsuGiornaliera")) Then
    '                oAccertamentoSingolo.IsTarsuGiornaliera = drDati("IsTarsuGiornaliera")
    '            End If
    '            If Not IsDBNull(drDati("MQ")) Then
    '                oAccertamentoSingolo.MQ = drDati("MQ")
    '            End If
    '            If Not IsDBNull(drDati("nComponenti")) Then
    '                oAccertamentoSingolo.nComponenti = drDati("nComponenti")
    '            End If
    '            If Not IsDBNull(drDati("Numero")) Then
    '                oAccertamentoSingolo.Numero = drDati("Numero")
    '            End If

    '            'Calcolo dei bimestri
    '            'If Not IsDBNull(drDati("Bimestri")) Then
    '            '    oAccertamentoSingolo.NumeroBimestri = drDati("Bimestri")
    '            'End If
    '            If Not bVisualizzazione Then
    '                If oAccertamentoSingolo.DataInizio <> Date.MinValue Then
    '                    Dim tDataInizioPerCalcolo As DateTime
    '                    Dim tDataFinePerCalcolo As DateTime
    '                    If oAccertamentoSingolo.DataInizio <= "01/01/" + oAccertamentoSingolo.Anno Then
    '                        tDataInizioPerCalcolo = "31/12/" + (CInt(oAccertamentoSingolo.Anno) - 1).ToString
    '                    Else
    '                        tDataInizioPerCalcolo = oAccertamentoSingolo.DataInizio
    '                    End If
    '                    If oAccertamentoSingolo.DataFine > "31/12/" + oAccertamentoSingolo.Anno Then
    '                        tDataFinePerCalcolo = "31/12/" + oAccertamentoSingolo.Anno
    '                    Else
    '                        tDataFinePerCalcolo = oAccertamentoSingolo.DataFine
    '                    End If
    '                    nBimestri = RemoRuoloTARSU.GetBimestri(tDataInizioPerCalcolo, tDataFinePerCalcolo, oAccertamentoSingolo.Anno)
    '                    If nBimestri = -1 Then
    '                    Else
    '                        oAccertamentoSingolo.NumeroBimestri = nBimestri
    '                    End If
    '                End If
    '            Else
    '                'Sono in visualizzazione
    '                oAccertamentoSingolo.NumeroBimestri = drDati("Bimestri")
    '            End If
    '            If Not IsDBNull(drDati("Scala")) Then
    '                oAccertamentoSingolo.Scala = drDati("Scala")
    '            End If
    '            If Not IsDBNull(drDati("Subalterno")) Then
    '                oAccertamentoSingolo.Subalterno = drDati("Subalterno")
    '            End If
    '            If Not IsDBNull(drDati("Via")) Then
    '                oAccertamentoSingolo.Via = drDati("Via")
    '            End If
    '            If Not IsDBNull(drDati("importo_sanzioni")) Then
    '                oAccertamentoSingolo.ImpSanzioni = drDati("importo_sanzioni")
    '            End If
    '            If Not IsDBNull(drDati("importo_interessi")) Then
    '                oAccertamentoSingolo.ImpInteressi = drDati("importo_interessi")
    '            End If

    '            If Not IsDBNull(drDati("motivazione")) Then
    '                oAccertamentoSingolo.DescrSanzioni = drDati("motivazione")
    '            End If
    '            If Not IsDBNull(drDati("cod_voce")) Then
    '                If Not IsDBNull(drDati("cod_tipo_provvedimento")) Then
    '                    oAccertamentoSingolo.Sanzioni = drDati("cod_voce") & "#" & drDati("cod_tipo_provvedimento")
    '                Else
    '                    oAccertamentoSingolo.Sanzioni = drDati("cod_voce") & "#"
    '                End If
    '            End If
    '            If Not IsDBNull(drDati("calcola_interessi")) Then
    '                If drDati("calcola_interessi") = 0 Then
    '                    oAccertamentoSingolo.Calcola_Interessi = False
    '                Else
    '                    oAccertamentoSingolo.Calcola_Interessi = True
    '                End If
    '            End If
    '            If Not IsNothing(HttpContext.Current.Session("idtestata")) Then
    '                oAccertamentoSingolo.IdTestata = HttpContext.Current.Session("idtestata")
    '            ElseIf Not IsDBNull(drDati("idtestata")) Then
    '                oAccertamentoSingolo.IdTestata = drDati("idtestata")
    '            Else
    '                oAccertamentoSingolo.IdTestata = -1
    '            End If

    '            If Not IsDBNull(drDati("iddettagliotestata")) Then
    '                oAccertamentoSingolo.IdDettaglioTestata = drDati("iddettagliotestata")
    '            Else
    '                oAccertamentoSingolo.IdDettaglioTestata = -1
    '            End If

    '            'carico riduzioni
    '            If oAccertamentoSingolo.ImportoRiduzione > 0 Then
    '                sSQL = "select distinct TBLACCERTATORIDUZIONE.idente, TBLACCERTATORIDUZIONE.idriduzione, valore, codice, descrizione, "
    '                sSQL += " CASE WHEN TIPO_RIDUZIONE = 'I' THEN 'IMPORTO' WHEN TIPO_RIDUZIONE = 'F' THEN 'FORMULA' ELSE '%' END AS TIPO, "
    '                sSQL += " TIPO_RIDUZIONE as sTipoValoreRid"
    '                sSQL += " from TBLACCERTATORIDUZIONE"
    '                sSQL += " inner join " & NOME_DATABASE_TARSU & ".dbo.TBLRIDUZIONI"
    '                sSQL += " on " & NOME_DATABASE_TARSU & ".dbo.TBLRIDUZIONI.id = TBLACCERTATORIDUZIONE.idRiduzione"
    '                sSQL += " inner join " & NOME_DATABASE_TARSU & ".dbo.TBLTIPORIDUZIONI "
    '                sSQL += " on " & NOME_DATABASE_TARSU & ".dbo.TBLTIPORIDUZIONI.codice = " & NOME_DATABASE_TARSU & ".dbo.TBLRIDUZIONI.idriduzione"
    '                sSQL += " where ID_PROVVEDIMENTO = " & nIdProvvedimento
    '                If Not IsDBNull(drDati("id_legame")) Then
    '                    sSQL += " and id_legame=" & drDati("id_legame")
    '                End If

    '                drDatiRid = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '                If Not IsNothing(drDatiRid) Then
    '                    i = -1
    '                    Do While drDatiRid.Read
    '                        i += 1
    '                        ReDim Preserve oListOggettoRiduzione(i)
    '                        oOggettoRiduzione = New OggettoRiduzione
    '                        oOggettoRiduzione.Descrizione = drDatiRid("descrizione")
    '                        oOggettoRiduzione.IdDettaglioTestata = -1
    '                        oOggettoRiduzione.IdRiduzione = drDatiRid("idriduzione")
    '                        oOggettoRiduzione.sIdEnte = drDatiRid("idente")
    '                        oOggettoRiduzione.sTipo = drDatiRid("TIPO")
    '                        oOggettoRiduzione.sTipoValoreRid = drDatiRid("sTipoValoreRid")
    '                        oOggettoRiduzione.sValore = drDatiRid("valore")

    '                        oListOggettoRiduzione(i) = oOggettoRiduzione
    '                    Loop
    '                    oAccertamentoSingolo.oRiduzioni = oListOggettoRiduzione
    '                End If
    '            End If


    '            If Not bVisualizzazione Then
    '                If RemoRuoloTARSU.CalcolaImportiRuolo(oAccertamentoSingolo, oAccertamentoSingolo.oRiduzioni, oAccertamentoSingolo.oDetassazioni, 0) = False Then
    '                    'Errore
    '                End If
    '            Else
    '                oAccertamentoSingolo.ImportoNetto = drDati("Importo_Netto")
    '            End If


    '            'If Not IsDBNull(drDati("Importo_Netto")) Then
    '            '    oAccertamentoSingolo.ImportoNetto = drDati("Importo_Netto")
    '            'End If



    '            oAccertamentoSingolo.Progressivo = x + 1
    '            If Not IsDBNull(drDati("Id_Legame")) Then
    '                oAccertamentoSingolo.IdLegame = drDati("Id_Legame")
    '            Else
    '                oAccertamentoSingolo.IdLegame = oAccertamentoSingolo.Progressivo
    '            End If

    '            oListAccertamento(x) = oAccertamentoSingolo
    '        Loop
    '        Return oListAccertamento
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.RicercaAccertatoTARSU.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        drDati.Close()
    '        WFSessione.Kill()
    '    End Try
    'End Function
    Public Function TARSU_RicercaAccertato(ByVal myConnectionString As String, ByVal nIdProvvedimento As Integer, ByRef bAnnoMod As Boolean, ByVal annoAccSelezionato As Integer, ByVal bIsLettura As Boolean, ByVal sTipoCalcolo As String, ByVal bHasMaggiorazione As Boolean, ByVal bHasConferimenti As Boolean) As ObjArticoloAccertamento()
        Dim oListAccertamento() As ObjArticoloAccertamento
        Dim myArtAcc As New ObjArticoloAccertamento
        Dim oListArticoli() As ObjArticolo
        Dim myArticolo As New ObjArticolo
        Dim myDataReader As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myArray As New ArrayList
        Dim FncRicalcolo As New OPENgovTIA.ClsElabRuolo
        Dim sMyTipoCalcolo As String = ""

        Try
            Log.Debug("entro")
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure

            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROVVEDIMENTO", SqlDbType.Int)).Value = nIdProvvedimento
            cmdMyCommand.CommandText = "prc_GetTARSUArticoliAcc"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Do While myDataReader.Read
                myArtAcc = New ObjArticoloAccertamento

                myArtAcc.Id = CInt(myDataReader("id"))
                myArtAcc.IdArticolo = myArtAcc.Id 'CInt(myDataReader("idruolo"))
                myArtAcc.IdContribuente = CInt(myDataReader("idcontribuente"))
                If Not IsDBNull(myDataReader("iddettagliotestata")) Then
                    myArtAcc.IdDettaglioTestata = CInt(myDataReader("iddettagliotestata"))
                End If
                myArtAcc.TipoPartita = CStr(myDataReader("tipopartita"))
                myArtAcc.IdEnte = CStr(myDataReader("idente"))
                If Not bIsLettura Then
                    myArtAcc.sAnno = annoAccSelezionato
                Else
                    myArtAcc.sAnno = CStr(myDataReader("anno"))
                End If
                myArtAcc.sVia = CStr(myDataReader("via"))
                If Not IsDBNull(myDataReader("civico")) Then
                    myArtAcc.sCivico = CStr(myDataReader("civico"))
                End If
                If Not IsDBNull(myDataReader("esponente")) Then
                    myArtAcc.sEsponente = CStr(myDataReader("esponente"))
                End If
                If Not IsDBNull(myDataReader("interno")) Then
                    myArtAcc.sInterno = CStr(myDataReader("interno"))
                End If
                If Not IsDBNull(myDataReader("scala")) Then
                    myArtAcc.sScala = CStr(myDataReader("scala"))
                End If
                If Not IsDBNull(myDataReader("foglio")) Then
                    myArtAcc.sFoglio = CStr(myDataReader("foglio"))
                End If
                If Not IsDBNull(myDataReader("numero")) Then
                    myArtAcc.sNumero = CStr(myDataReader("numero"))
                End If
                If Not IsDBNull(myDataReader("subalterno")) Then
                    myArtAcc.sSubalterno = CStr(myDataReader("subalterno"))
                End If
                If Not IsDBNull(myDataReader("idcategoria")) Then
                    myArtAcc.sCategoria = CStr(myDataReader("idcategoria"))
                End If
                If Not IsDBNull(myDataReader("descrizione")) Then
                    myArtAcc.sDescrCategoria = CStr(myDataReader("descrizione"))
                End If
                If Not IsDBNull(myDataReader("idtariffa")) Then
                    myArtAcc.nIdTariffa = CInt(myDataReader("idtariffa"))
                End If
                If Not IsDBNull(myDataReader("importo_tariffa")) Then
                    myArtAcc.impTariffa = CDbl(myDataReader("importo_tariffa"))
                End If
                If Not IsDBNull(myDataReader("ncomponenti")) Then
                    myArtAcc.nComponenti = CInt(myDataReader("ncomponenti"))
                End If
                If Not IsDBNull(myDataReader("ncomponenti_pv")) Then
                    myArtAcc.nComponentiPV = CInt(myDataReader("ncomponenti_pv"))
                End If
                myArtAcc.nMQ = CDbl(myDataReader("mq"))
                myArtAcc.nBimestri = CInt(myDataReader("bimestri"))
                If Not IsDBNull(myDataReader("istarsugiornaliera")) Then
                    myArtAcc.bIsTarsuGiornaliera = CBool(myDataReader("istarsugiornaliera"))
                End If
                If Not IsDBNull(myDataReader("forza_calcolapv")) Then
                    myArtAcc.bForzaPV = CBool(myDataReader("forza_calcolapv"))
                End If
                If Not IsDBNull(myDataReader("importo")) Then
                    myArtAcc.impRuolo = CDbl(myDataReader("importo"))
                End If
                If Not IsDBNull(myDataReader("importo_netto")) Then
                    myArtAcc.impNetto = CStr(myDataReader("importo_netto"))
                End If
                If Not IsDBNull(myDataReader("importo_riduzioni")) Then
                    myArtAcc.impRiduzione = CDbl(myDataReader("importo_riduzioni"))
                End If
                If Not IsDBNull(myDataReader("importo_detassazioni")) Then
                    myArtAcc.impDetassazione = CDbl(myDataReader("importo_detassazioni"))
                End If
                If Not IsDBNull(myDataReader("importo_forzato")) Then
                    myArtAcc.bIsImportoForzato = CBool(myDataReader("importo_forzato"))
                End If
                'prelevo le riduzioni
                Log.Debug("carico ridese")
                myArtAcc.oRiduzioni = TARSUAcc_GetRidEseApplicate(ObjRidEse.TIPO_RIDUZIONI, myArtAcc.Id, myConnectionString)
                'prelevo le detassazioni
                myArtAcc.oDetassazioni = TARSUAcc_GetRidEseApplicate(ObjRidEse.TIPO_ESENZIONI, myArtAcc.Id, myConnectionString)

                If Not IsDBNull(myDataReader("Data_Inizio")) Then
                    If annoAccSelezionato <> -1 And Not bIsLettura Then
                        'modifico della data UI in base all'anno di accertamento
                        If Year(myDataReader("Data_Inizio")) = annoAccSelezionato Then
                            myArtAcc.tDataInizio = myDataReader("Data_Inizio")
                        Else
                            myArtAcc.tDataInizio = CDate(annoAccSelezionato & "-01-01") 'CDate(annoAccSelezionato & "-" & Month(myDataReader("Data_Inizio")) & "-" & Day(myDataReader("Data_Inizio")))
                            bAnnoMod = True
                        End If
                    Else
                        myArtAcc.tDataInizio = myDataReader("Data_Inizio")
                    End If
                End If
                If Not IsDBNull(myDataReader("Data_Fine")) Then
                    If annoAccSelezionato <> -1 And Not bIsLettura Then
                        'modifico della data UI in base all'anno di accertamento
                        If Year(myDataReader("Data_Fine")) = annoAccSelezionato Then
                            myArtAcc.tDataFine = myDataReader("Data_Fine")
                        Else
                            myArtAcc.tDataFine = CDate(annoAccSelezionato & "-12-31") 'CDate(annoAccSelezionato & "-" & Month(myDataReader("Data_Fine")) & "-" & Day(myDataReader("Data_Fine")))
                            bAnnoMod = True
                        End If
                    Else
                        myArtAcc.tDataFine = myDataReader("Data_Fine")
                    End If
                End If
                If Not IsDBNull(myDataReader("importo_sanzioni")) Then
                    myArtAcc.ImpSanzioni = myDataReader("importo_sanzioni")
                End If
                If Not IsDBNull(myDataReader("importo_interessi")) Then
                    myArtAcc.ImpInteressi = myDataReader("importo_interessi")
                End If
                If Not IsDBNull(myDataReader("motivazione")) Then
                    myArtAcc.sDescrSanzioni = myDataReader("motivazione")
                End If
                If Not IsDBNull(myDataReader("cod_voce")) Then
                    If Not IsDBNull(myDataReader("cod_tipo_provvedimento")) Then
                        myArtAcc.Sanzioni = myDataReader("cod_voce") & "#" & myDataReader("cod_tipo_provvedimento")
                    Else
                        myArtAcc.Sanzioni = myDataReader("cod_voce") & "#"
                    End If
                End If
                If Not IsDBNull(myDataReader("calcola_interessi")) Then
                    If myDataReader("calcola_interessi") = 0 Then
                        myArtAcc.Calcola_Interessi = False
                    Else
                        myArtAcc.Calcola_Interessi = True
                    End If
                End If
                myArtAcc.Progressivo = myDataReader("progressivo")
                If Not IsDBNull(myDataReader("Id_Legame")) Then
                    myArtAcc.IdLegame = myDataReader("Id_Legame")
                Else
                    myArtAcc.IdLegame = myArtAcc.Progressivo
                End If
                'le note non sono utilizzate quindi le uso come appoggio per il tipo calcolo
                myArtAcc.sNote = myDataReader("tipocalcolo")
                sMyTipoCalcolo = myDataReader("tipocalcolo")
                'se devo ricalcolare non carico gli articoli negativi
                If myArtAcc.sVia.Contains("DA DICHIARAZIONE") = False Or bIsLettura Then
                    myArray.Add(myArtAcc)
                End If
            Loop
            oListAccertamento = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())

            If Not bIsLettura Then
                Log.Debug("ricalcolo avviso")
                oListArticoli = PrepareArticoliForCalcolo(oListAccertamento, myArticolo, sTipoCalcolo, False)
                If FncRicalcolo.RicalcoloAvviso(myConnectionString, oListArticoli, "", sTipoCalcolo, bHasMaggiorazione, bHasConferimenti, "P") = False Then
                    Return Nothing
                End If
                Dim CurrentItem As New ObjArticoloAccertamento
                myArray.Clear()
                'prelevo gli articoli determinati dal ricalcolo
                oListArticoli = HttpContext.Current.Session("oListArticoli")
                For Each myArticolo In oListArticoli
                    CurrentItem = ArticoloTOArticoloAccertamento(myArticolo, True)
                    CurrentItem.IdEnte = ConstSession.IdEnte
                    If Not CurrentItem Is Nothing Then
                        For Each myArtAcc In oListAccertamento
                            If CurrentItem.Id = myArtAcc.Progressivo Then
                                CurrentItem.Sanzioni = myArtAcc.Sanzioni
                                CurrentItem.sDescrSanzioni = myArtAcc.sDescrSanzioni
                                CurrentItem.Calcola_Interessi = myArtAcc.Calcola_Interessi
                            End If
                        Next
                    Else
                        Throw New Exception("errore in caricamento articolo")
                    End If
                    myArray.Add(CurrentItem)
                Next
                oListAccertamento = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())
                'If sMyTipoCalcolo = ObjRuolo.TipoCalcolo.TARSU Then
                '    Array.Sort(oListAccertamento, New Utility.Comparatore(New String() {"Progressivo"}, New Boolean() {Utility.TipoOrdinamento.Crescente}))
                'End If
            End If
            Log.Debug("esco")
            Return oListAccertamento
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.TARSU_RicercaAccertato.errore: ", Err)
            Return Nothing
        Finally
            myDataReader.Close()
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function

    Public Function TARSUAcc_GetRidEseApplicate(ByVal sTabella As String, ByVal nIdRiferimento As Integer, ByVal myConnectionString As String) As ObjRidEseApplicati()
        Dim oListToReturn() As ObjRidEseApplicati
        Dim oMyRidEse As ObjRidEseApplicati
        Dim nList As Integer = -1
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetTARSURidEseApplicate"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MyTabella", SqlDbType.NVarChar)).Value = sTabella
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDRIFERIMENTO", SqlDbType.Int)).Value = nIdRiferimento
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            dtMyDati.Load(myDataReader)
            For Each dtMyRow In dtMyDati.Rows
                nList += 1
                oMyRidEse = New ObjRidEseApplicati
                oMyRidEse.IdRiferimento = nIdRiferimento
                oMyRidEse.Riferimento = ObjRidEseApplicati.RIF_ARTICOLO
                oMyRidEse.ID = CInt(dtMyRow("id"))
                oMyRidEse.IdEnte = CStr(dtMyRow("idente"))
                oMyRidEse.sCodice = CStr(dtMyRow("codice"))
                oMyRidEse.sDescrizione = CStr(dtMyRow("descrizione"))
                oMyRidEse.sTipoOggetto = sTabella
                oMyRidEse.sTipoValore = CStr(dtMyRow("tipo"))
                oMyRidEse.sDescrTipo = CStr(dtMyRow("descrtipo"))
                oMyRidEse.sValore = CStr(dtMyRow("valore"))
                oMyRidEse.nTipoApplicazione = CInt(dtMyRow("tipoapplicazione"))
                oMyRidEse.sDescrApplicazione = CStr(dtMyRow("descrapplicazione"))
                oMyRidEse.sAnno = CStr(dtMyRow("anno"))
                oMyRidEse.tDataInizioValidita = CDate(dtMyRow("data_inizio_validita"))
                If Not IsDBNull(dtMyRow("data_fine_validita")) Then
                    oMyRidEse.tDataFineValidita = CDate(dtMyRow("data_fine_validita"))
                End If
                'dimensiono l'array
                ReDim Preserve oListToReturn(nList)
                'memorizzo i dati nell'array
                oListToReturn(nList) = oMyRidEse
            Next
            Return oListToReturn
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.TARSUAcc_GetRidEseApplicate.errore: ", Err)
            Return Nothing
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function

    'Public Function RicercaDichiaratoTARSU(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sApplicazione As String, ByVal nIdProvvedimento As Integer) As OggettoArticoloRuolo()
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim sSQL, WFErrore As String
    '    Dim oListAccertamento() As OggettoArticoloRuolo
    '    Dim oAccertamentoSingolo As OggettoArticoloRuolo
    '    Dim x As Integer = -1
    '    Dim drDati As SqlClient.SqlDataReader

    '    Dim oListOggettoRiduzione() As OggettoRiduzione
    '    Dim oOggettoRiduzione As OggettoRiduzione
    '    Dim i As Integer = -1
    '    Dim drDatiRid As SqlClient.SqlDataReader

    '    Dim NomeDbTarsu As String = ConfigurationManager.AppSettings("NOME_DATABASE_TARSU").ToString()

    '    Try
    '        WFSessione = New OPENUtility.CreateSessione(sParametroENV, sUserName, sApplicazione)
    '        If Not WFSessione.CreaSessione(sUserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        sSQL = "SELECT *"
    '        sSQL += " FROM TBLRUOLODICHIARATO"
    '        sSQL += " WHERE (ID_PROVVEDIMENTO=" & nIdProvvedimento & ")"
    '        drDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While drDati.Read
    '            x += 1
    '            ReDim Preserve oListAccertamento(x)

    '            oAccertamentoSingolo = New OggettoArticoloRuolo
    '            If Not IsDBNull(drDati("Anno")) Then
    '                oAccertamentoSingolo.Anno = drDati("Anno")
    '            End If
    '            If Not IsDBNull(drDati("idCategoria")) Then
    '                oAccertamentoSingolo.Categoria = drDati("idCategoria")
    '            End If
    '            If Not IsDBNull(drDati("Civico")) Then
    '                oAccertamentoSingolo.Civico = drDati("Civico")
    '            End If
    '            If Not IsDBNull(drDati("CodVia")) Then
    '                oAccertamentoSingolo.CodVia = drDati("CodVia")
    '            End If
    '            If Not IsDBNull(drDati("Data_Fine")) Then
    '                oAccertamentoSingolo.DataFine = drDati("Data_Fine")
    '            End If
    '            If Not IsDBNull(drDati("Data_Inizio")) Then
    '                oAccertamentoSingolo.DataInizio = drDati("Data_Inizio")
    '            End If
    '            If Not IsDBNull(drDati("idEnte")) Then
    '                oAccertamentoSingolo.Ente = drDati("idEnte")
    '            End If
    '            If Not IsDBNull(drDati("Esponente")) Then
    '                oAccertamentoSingolo.Esponente = drDati("Esponente")
    '            End If
    '            If Not IsDBNull(drDati("Foglio")) Then
    '                oAccertamentoSingolo.Foglio = drDati("Foglio")
    '            End If
    '            If Not IsDBNull(drDati("Id")) Then
    '                oAccertamentoSingolo.Id = drDati("Id")
    '            End If
    '            If Not IsDBNull(drDati("IdContribuente")) Then
    '                oAccertamentoSingolo.IdContribuente = drDati("IdContribuente")
    '            End If
    '            If Not IsDBNull(drDati("IDTariffa")) Then
    '                oAccertamentoSingolo.IDTariffa = drDati("IDTariffa")
    '            End If
    '            If Not IsDBNull(drDati("Importo_Detassazioni")) Then
    '                oAccertamentoSingolo.ImportoDetassazione = drDati("Importo_Detassazioni")
    '            End If
    '            If Not IsDBNull(drDati("Importo_Forzato")) Then
    '                oAccertamentoSingolo.ImportoForzato = drDati("Importo_Forzato")
    '            End If
    '            If Not IsDBNull(drDati("Importo_Netto")) Then
    '                oAccertamentoSingolo.ImportoNetto = drDati("Importo_Netto")
    '            End If
    '            If Not IsDBNull(drDati("Importo_Riduzioni")) Then
    '                oAccertamentoSingolo.ImportoRiduzione = drDati("Importo_Riduzioni")
    '            End If
    '            If Not IsDBNull(drDati("Importo")) Then
    '                oAccertamentoSingolo.ImportoRuolo = drDati("Importo")
    '            End If
    '            If Not IsDBNull(drDati("Importo_Tariffa")) Then
    '                oAccertamentoSingolo.ImpTariffa = drDati("Importo_Tariffa")
    '            End If
    '            If Not IsDBNull(drDati("Interno")) Then
    '                oAccertamentoSingolo.Interno = drDati("Interno")
    '            End If
    '            If Not IsDBNull(drDati("IsTarsuGiornaliera")) Then
    '                oAccertamentoSingolo.IsTarsuGiornaliera = drDati("IsTarsuGiornaliera")
    '            End If
    '            If Not IsDBNull(drDati("MQ")) Then
    '                oAccertamentoSingolo.MQ = drDati("MQ")
    '            End If
    '            If Not IsDBNull(drDati("nComponenti")) Then
    '                oAccertamentoSingolo.nComponenti = drDati("nComponenti")
    '            End If
    '            If Not IsDBNull(drDati("Numero")) Then
    '                oAccertamentoSingolo.Numero = drDati("Numero")
    '            End If
    '            If Not IsDBNull(drDati("Bimestri")) Then
    '                oAccertamentoSingolo.NumeroBimestri = drDati("Bimestri")
    '            End If
    '            If Not IsDBNull(drDati("Scala")) Then
    '                oAccertamentoSingolo.Scala = drDati("Scala")
    '            End If
    '            If Not IsDBNull(drDati("Subalterno")) Then
    '                oAccertamentoSingolo.Subalterno = drDati("Subalterno")
    '            End If
    '            If Not IsDBNull(drDati("Via")) Then
    '                oAccertamentoSingolo.Via = drDati("Via")
    '            End If


    '            'riduzioni
    '            sSQL = "SELECT " & NomeDbTarsu & ".dbo.TBLTIPORIDUZIONI.idente,"
    '            sSQL += " " & NomeDbTarsu & ".dbo.TBLRIDUZIONI.id as idriduzione, valore, codice, descrizione,"
    '            sSQL += " CASE WHEN TIPO_RIDUZIONE = 'I' THEN 'IMPORTO' WHEN TIPO_RIDUZIONE = 'F' THEN 'FORMULA' ELSE '%' END AS TIPO, "
    '            sSQL += " TIPO_RIDUZIONE as sTipoValoreRid"
    '            sSQL += " FROM " & NomeDbTarsu & ".dbo.TBLTIPORIDUZIONI"
    '            sSQL += " INNER JOIN " & NomeDbTarsu & ".dbo.TBLDETTAGLIOTESTATARIDUZIONI "
    '            sSQL += " ON " & NomeDbTarsu & ".dbo.TBLTIPORIDUZIONI.CODICE = " & NomeDbTarsu & ".dbo.TBLDETTAGLIOTESTATARIDUZIONI.IDRIDUZIONE"
    '            sSQL += " inner join " & NomeDbTarsu & ".dbo.TBLRIDUZIONI"
    '            sSQL += " on " & NomeDbTarsu & ".dbo.TBLRIDUZIONI.idriduzione = " & NomeDbTarsu & ".dbo.TBLTIPORIDUZIONI.codice"
    '            sSQL += " WHERE IDDETTAGLIOTESTATA = " & drDati("IDDETTAGLIOTESTATA")
    '            sSQL += " AND TBLTIPORIDUZIONI.IDENTE='" & drDati("idEnte") & "'"
    '            sSQL += " and anno=" & drDati("Anno")
    '            sSQL += " ORDER BY DESCRIZIONE"

    '            drDatiRid = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '            If Not IsNothing(drDatiRid) Then
    '                i = -1
    '                Do While drDatiRid.Read
    '                    i += 1
    '                    ReDim Preserve oListOggettoRiduzione(i)
    '                    oOggettoRiduzione = New OggettoRiduzione
    '                    oOggettoRiduzione.Descrizione = drDatiRid("descrizione")
    '                    oOggettoRiduzione.IdDettaglioTestata = drDati("IDDETTAGLIOTESTATA")
    '                    oOggettoRiduzione.IdRiduzione = drDatiRid("idriduzione")
    '                    oOggettoRiduzione.sIdEnte = drDatiRid("idente")
    '                    oOggettoRiduzione.sTipo = drDatiRid("TIPO")
    '                    oOggettoRiduzione.sTipoValoreRid = drDatiRid("sTipoValoreRid")
    '                    oOggettoRiduzione.sValore = drDatiRid("valore")

    '                    oListOggettoRiduzione(i) = oOggettoRiduzione
    '                Loop
    '                oAccertamentoSingolo.oRiduzioni = oListOggettoRiduzione
    '            End If


    '            oAccertamentoSingolo.Progressivo = x + 1
    '            oAccertamentoSingolo.IdLegame = oAccertamentoSingolo.Progressivo
    '            oListAccertamento(x) = oAccertamentoSingolo
    '        Loop
    '        Return oListAccertamento
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.RicercaDichiaratoTARSU.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        drDati.Close()
    '        WFSessione.Kill()
    '    End Try
    'End Function
    Public Function TARSU_RicercaDichiarato(ByVal myConnectionString As String, ByVal nIdProvvedimento As Integer) As ObjArticoloAccertamento()
        Dim oListAccertamento() As ObjArticoloAccertamento
        Dim myArtAcc As New ObjArticoloAccertamento
        Dim myDataReader As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myArray As New ArrayList

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure

            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROVVEDIMENTO", SqlDbType.Int)).Value = nIdProvvedimento
            cmdMyCommand.CommandText = "prc_GetTARSUArticoliDich"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Do While myDataReader.Read
                myArtAcc = New ObjArticoloAccertamento

                myArtAcc.Id = CInt(myDataReader("id"))
                myArtAcc.IdArticolo = myArtAcc.Id 'CInt(myDataReader("idruolo"))
                myArtAcc.IdContribuente = CInt(myDataReader("idcontribuente"))
                If Not IsDBNull(myDataReader("iddettagliotestata")) Then
                    myArtAcc.IdDettaglioTestata = CInt(myDataReader("iddettagliotestata"))
                End If
                myArtAcc.TipoPartita = CStr(myDataReader("tipopartita"))
                myArtAcc.IdEnte = CStr(myDataReader("idente"))
                myArtAcc.sAnno = CStr(myDataReader("anno"))
                myArtAcc.sVia = CStr(myDataReader("via"))
                If Not IsDBNull(myDataReader("civico")) Then
                    myArtAcc.sCivico = CStr(myDataReader("civico"))
                End If
                If Not IsDBNull(myDataReader("esponente")) Then
                    myArtAcc.sEsponente = CStr(myDataReader("esponente"))
                End If
                If Not IsDBNull(myDataReader("interno")) Then
                    myArtAcc.sInterno = CStr(myDataReader("interno"))
                End If
                If Not IsDBNull(myDataReader("scala")) Then
                    myArtAcc.sScala = CStr(myDataReader("scala"))
                End If
                If Not IsDBNull(myDataReader("foglio")) Then
                    myArtAcc.sFoglio = CStr(myDataReader("foglio"))
                End If
                If Not IsDBNull(myDataReader("numero")) Then
                    myArtAcc.sNumero = CStr(myDataReader("numero"))
                End If
                If Not IsDBNull(myDataReader("subalterno")) Then
                    myArtAcc.sSubalterno = CStr(myDataReader("subalterno"))
                End If
                If Not IsDBNull(myDataReader("idcategoria")) Then
                    myArtAcc.sCategoria = CStr(myDataReader("idcategoria"))
                End If
                If Not IsDBNull(myDataReader("descrizione")) Then
                    myArtAcc.sDescrCategoria = CStr(myDataReader("descrizione"))
                End If
                If Not IsDBNull(myDataReader("idtariffa")) Then
                    myArtAcc.nIdTariffa = CInt(myDataReader("idtariffa"))
                End If
                If Not IsDBNull(myDataReader("importo_tariffa")) Then
                    myArtAcc.impTariffa = CDbl(myDataReader("importo_tariffa"))
                End If
                If Not IsDBNull(myDataReader("ncomponenti")) Then
                    myArtAcc.nComponenti = CInt(myDataReader("ncomponenti"))
                End If
                If Not IsDBNull(myDataReader("ncomponenti_pv")) Then
                    myArtAcc.nComponentiPV = CInt(myDataReader("ncomponenti_pv"))
                End If
                myArtAcc.nMQ = CDbl(myDataReader("mq"))
                myArtAcc.nBimestri = CInt(myDataReader("bimestri"))
                If Not IsDBNull(myDataReader("istarsugiornaliera")) Then
                    myArtAcc.bIsTarsuGiornaliera = CBool(myDataReader("istarsugiornaliera"))
                End If
                If Not IsDBNull(myDataReader("forza_calcolapv")) Then
                    myArtAcc.bForzaPV = CBool(myDataReader("forza_calcolapv"))
                End If
                If Not IsDBNull(myDataReader("importo")) Then
                    myArtAcc.impRuolo = CDbl(myDataReader("importo"))
                End If
                If Not IsDBNull(myDataReader("importo_netto")) Then
                    myArtAcc.impNetto = CStr(myDataReader("importo_netto"))
                End If
                If Not IsDBNull(myDataReader("importo_riduzioni")) Then
                    myArtAcc.impRiduzione = CDbl(myDataReader("importo_riduzioni"))
                End If
                If Not IsDBNull(myDataReader("importo_detassazioni")) Then
                    myArtAcc.impDetassazione = CDbl(myDataReader("importo_detassazioni"))
                End If
                If Not IsDBNull(myDataReader("importo_forzato")) Then
                    myArtAcc.bIsImportoForzato = CBool(myDataReader("importo_forzato"))
                End If
                'prelevo le riduzioni
                myArtAcc.oRiduzioni = TARSUAcc_GetRidEseApplicate(ObjRidEse.TIPO_RIDUZIONI, myArtAcc.Id, myConnectionString)
                'prelevo le detassazioni
                myArtAcc.oDetassazioni = TARSUAcc_GetRidEseApplicate(ObjRidEse.TIPO_ESENZIONI, myArtAcc.Id, myConnectionString)

                If Not IsDBNull(myDataReader("Data_Inizio")) Then
                    myArtAcc.tDataInizio = myDataReader("Data_Inizio")
                End If
                If Not IsDBNull(myDataReader("Data_Fine")) Then
                    myArtAcc.tDataFine = myDataReader("Data_Fine")
                End If
                myArtAcc.Progressivo = myDataReader("progressivo")
                If Not IsDBNull(myDataReader("Id_Legame")) Then
                    myArtAcc.IdLegame = myDataReader("Id_Legame")
                Else
                    myArtAcc.IdLegame = myArtAcc.Progressivo
                End If
                'le note non sono utilizzate quindi le uso come appoggio per il tipo calcolo
                myArtAcc.sNote = myDataReader("tipocalcolo")
                myArray.Add(myArtAcc)
            Loop
            oListAccertamento = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())
            Return oListAccertamento
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.TARSU_RicercaDichiarato.errore: ", Err)
            Return Nothing
        Finally
            myDataReader.Close()
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function

    ''recupero riduzioni per dichiarato
    'Public Function GetRiduzioni(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sApplicazione As String, ByVal COD_ENTE As String, ByVal IDDETTAGLIOTESTATA As Integer, ByVal ANNO As Integer) As SqlClient.SqlDataReader
    '    Dim sSQL, WFErrore As String
    '    Dim NomeDbTarsu As String = ConfigurationManager.AppSettings("NOME_DATABASE_TARSU").ToString()
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim drDatiRid As SqlClient.SqlDataReader
    '    Try

    '        WFSessione = New OPENUtility.CreateSessione(sParametroENV, sUserName, sApplicazione)
    '        If Not WFSessione.CreaSessione(sUserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'riduzioni dichiarato

    '        sSQL = "SELECT " & NomeDbTarsu & ".dbo.TBLTIPORIDUZIONI.idente,"
    '        sSQL += " " & NomeDbTarsu & ".dbo.TBLRIDUZIONI.id as idriduzione, valore, codice, descrizione,"
    '        sSQL += " CASE WHEN TIPO_RIDUZIONE = 'I' THEN 'IMPORTO' WHEN TIPO_RIDUZIONE = 'F' THEN 'FORMULA' ELSE '%' END AS TIPO, "
    '        sSQL += " TIPO_RIDUZIONE as sTipoValoreRid"
    '        sSQL += " FROM " & NomeDbTarsu & ".dbo.TBLTIPORIDUZIONI"
    '        sSQL += " INNER JOIN " & NomeDbTarsu & ".dbo.TBLDETTAGLIOTESTATARIDUZIONI "
    '        sSQL += " ON " & NomeDbTarsu & ".dbo.TBLTIPORIDUZIONI.CODICE = " & NomeDbTarsu & ".dbo.TBLDETTAGLIOTESTATARIDUZIONI.IDRIDUZIONE"
    '        sSQL += " inner join " & NomeDbTarsu & ".dbo.TBLRIDUZIONI"
    '        sSQL += " on " & NomeDbTarsu & ".dbo.TBLRIDUZIONI.idriduzione = " & NomeDbTarsu & ".dbo.TBLTIPORIDUZIONI.codice"
    '        sSQL += " WHERE IDDETTAGLIOTESTATA = " & IDDETTAGLIOTESTATA
    '        sSQL += " AND TBLTIPORIDUZIONI.IDENTE='" & COD_ENTE & "'"
    '        sSQL += " and anno=" & ANNO
    '        sSQL += " ORDER BY DESCRIZIONE"

    '        drDatiRid = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Return drDatiRid
    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.GetRiduzioni.errore: ", ex)
    '        Return Nothing
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Function
    Public Function GetRiduzioni(ByVal myConnectionString As String, ByVal COD_ENTE As String, ByVal IDDETTAGLIOTESTATA As Integer, ByVal ANNO As Integer) As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure

            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.VarChar)).Value = COD_ENTE
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDUI", SqlDbType.Int)).Value = IDDETTAGLIOTESTATA
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.Int)).Value = ANNO
            cmdMyCommand.CommandText = "prc_GetStampaAccertamentiRidTARSU"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Return myDataReader
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.GetRiduzioni.errore: ", ex)
            Return Nothing
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function
    '*** ***

    ''recupero riduzioni per accertato
    'Public Function GetRiduzioniAccertato(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sApplicazione As String, ByVal ID_PROVVEDIMENTO As Integer, ByVal idaccertamento As Integer) As SqlClient.SqlDataReader
    '    Dim sSQL, WFErrore As String
    '    Dim NomeDbTarsu As String = ConfigurationManager.AppSettings("NOME_DATABASE_TARSU").ToString()
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim drDatiRid As SqlClient.SqlDataReader
    '    Try

    '        WFSessione = New OPENUtility.CreateSessione(sParametroENV, sUserName, sApplicazione)
    '        If Not WFSessione.CreaSessione(sUserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'riduzioni accertato
    '        sSQL = "select codice,descrizione,tipo_riduzione,valore from TBLACCERTATORIDUZIONE"
    '        sSQL += " inner join OPENgovTARSU_TRIBUTI.dbo.TBLRIDUZIONI on "
    '        sSQL += " OPENgovTARSU_TRIBUTI.dbo.TBLRIDUZIONI.id = TBLACCERTATORIDUZIONE.idriduzione"
    '        sSQL += " inner join OPENgovTARSU_TRIBUTI.dbo.TBLTIPORIDUZIONI on "
    '        sSQL += " OPENgovTARSU_TRIBUTI.dbo.TBLTIPORIDUZIONI.codice = TBLRIDUZIONI.idriduzione"
    '        sSQL += " WHERE ID_PROVVEDIMENTO =" & ID_PROVVEDIMENTO
    '        sSQL += " and idaccertamento=" & idaccertamento

    '        drDatiRid = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Return drDatiRid
    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.GetRiduzioniAccertato.errore: ", ex)
    '        Return Nothing
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Function

    Public Function DeleteAttoICI(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sApplicazione As String, ByVal sIdEnte As String, ByVal nIdProvvedimento As Integer, ByVal nIdProcedimento As Integer, ByVal sAnno As String, ByVal nIdContribuente As Integer) As Boolean
        'Dim objSessione As CreateSessione
        Dim sSQL As String
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim iRet As Integer

        Try
            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString


            'If objDBManager.Initialize(ConstSession.StringConnection) Then
            sSQL = "delete from DICHIARATO_ICI_LIQUIDAZIONI "
            sSQL += " where id_procedimento= " & nIdProcedimento
            sSQL += " and idcontribuente=" & nIdContribuente
            sSQL += " and ente=" & sIdEnte
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            iRet = cmdMyCommand.ExecuteNonQuery
            Log.Debug("Cancellato " & iRet & " DICHIARATO_ICI_LIQUIDAZIONI per contribuente " & nIdContribuente & " Anno " & sAnno)

            sSQL = "delete from TP_IMMOBILI_ACCERTATI_ACCERTAMENTI "
            sSQL += " where id_procedimento = " & nIdProcedimento
            sSQL += " and ente=" & sIdEnte
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            iRet = cmdMyCommand.ExecuteNonQuery
            Log.Debug("Cancellato " & iRet & " Immobile accertato per contribuente " & nIdContribuente & " Anno " & sAnno)

            If DeleteAtto(nIdProvvedimento) = False Then
                Return False
            End If
            Log.Debug("Cancellato Provvedimento e Procedimento per contribuente " & nIdContribuente & " Anno " & sAnno & "::IDPROVVEDIMENTO::" & nIdProvvedimento)
            'Else
            '    Log.Debug("Si è verificato un errore in ClsGestioneAccertamenti::DeleteAttoICI::Errore inizializzazione workflow")
            '    Return False
            'End If
            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.DeleteAttoICI.errore: ", Err)
            Return False
        Finally
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdProvvedimento"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="nIdContribuente"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function DeleteAttoTARSU(ByVal nIdProvvedimento As Integer, ByVal sAnno As String, ByVal nIdContribuente As Integer) As Boolean
        Try
            If DeleteAtto(nIdProvvedimento) = False Then
                Return False
            End If
            Log.Debug("Cancellato Provvedimento e Procedimento per contribuente " & nIdContribuente & " Anno " & sAnno & "::IDPROVVEDIMENTO::" & nIdProvvedimento)
            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.DeleteAttoTARSU.errore: ", Err)
            Return False
        End Try
    End Function
    'Public Function DeleteAttoTARSU(ByVal nIdProvvedimento As Integer, ByVal sAnno As String, ByVal nIdContribuente As Integer) As Boolean
    '    'Dim objSessione As CreateSessione
    '    Dim sSQL As String
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim iRet As Integer

    '    Try
    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

    '        'If objDBManager.Initialize(ConstSession.StringConnection) Then
    '        sSQL = "DELETE"
    '        sSQL += " FROM TBLDETTAGLIOTESTATADETASSAZIONI"
    '        sSQL += " WHERE (ID_PROVVEDIMENTO= " & nIdProvvedimento & ")"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        iRet = cmdMyCommand.ExecuteNonQuery
    '        Log.Debug("Cancellato " & iRet & " TBLDETTAGLIOTESTATADETASSAZIONI per contribuente " & nIdContribuente & " Anno " & sAnno & "::IDPROVVEDIMENTO::" & nIdProvvedimento)

    '        sSQL = "DELETE"
    '        sSQL += " FROM TBLDETTAGLIOTESTATARIDUZIONI"
    '        sSQL += " WHERE (ID_PROVVEDIMENTO= " & nIdProvvedimento & ")"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        iRet = cmdMyCommand.ExecuteNonQuery
    '        Log.Debug("Cancellato " & iRet & " TBLDETTAGLIOTESTATARIDUZIONI per contribuente " & nIdContribuente & " Anno " & sAnno & "::IDPROVVEDIMENTO::" & nIdProvvedimento)

    '        sSQL = "DELETE"
    '        sSQL += " FROM TBLOGGETTITARSU"
    '        sSQL += " WHERE (ID_PROVVEDIMENTO= " & nIdProvvedimento & ")"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        iRet = cmdMyCommand.ExecuteNonQuery
    '        Log.Debug("Cancellato " & iRet & " TBLOGGETTITARSU per contribuente " & nIdContribuente & " Anno " & sAnno & "::IDPROVVEDIMENTO::" & nIdProvvedimento)

    '        sSQL = "DELETE"
    '        sSQL += " FROM TBLDETTAGLIOTESTATATARSU"
    '        sSQL += " WHERE (ID_PROVVEDIMENTO= " & nIdProvvedimento & ")"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        iRet = cmdMyCommand.ExecuteNonQuery
    '        Log.Debug("Cancellato " & iRet & " TBLDETTAGLIOTESTATATARSU per contribuente " & nIdContribuente & " Anno " & sAnno & "::IDPROVVEDIMENTO::" & nIdProvvedimento)

    '        sSQL = "DELETE"
    '        sSQL += " FROM TBLTESTATATARSU"
    '        sSQL += " WHERE (ID_PROVVEDIMENTO= " & nIdProvvedimento & ")"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        iRet = cmdMyCommand.ExecuteNonQuery
    '        Log.Debug("Cancellato " & iRet & " TBLTESTATATARSU per contribuente " & nIdContribuente & " Anno " & sAnno & "::IDPROVVEDIMENTO::" & nIdProvvedimento)

    '        sSQL = "DELETE"
    '        sSQL += " FROM TBLRIEPILOGOACCERTATOTARSU"
    '        sSQL += " WHERE (ID_PROVVEDIMENTO= " & nIdProvvedimento & ")"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        iRet = cmdMyCommand.ExecuteNonQuery
    '        Log.Debug("Cancellato " & iRet & " TBLRIEPILOGOACCERTATOTARSU per contribuente " & nIdContribuente & " Anno " & sAnno & "::IDPROVVEDIMENTO::" & nIdProvvedimento)

    '        sSQL = "DELETE"
    '        sSQL += " FROM TBLRUOLOACCERTATO"
    '        sSQL += " WHERE (ID_PROVVEDIMENTO= " & nIdProvvedimento & ")"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        iRet = cmdMyCommand.ExecuteNonQuery
    '        Log.Debug("Cancellato " & iRet & " TBLRUOLOACCERTATO per contribuente " & nIdContribuente & " Anno " & sAnno & "::IDPROVVEDIMENTO::" & nIdProvvedimento)

    '        sSQL = "DELETE"
    '        sSQL += " FROM TBLRUOLODICHIARATO"
    '        sSQL += " WHERE (ID_PROVVEDIMENTO= " & nIdProvvedimento & ")"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        iRet = cmdMyCommand.ExecuteNonQuery
    '        Log.Debug("Cancellato " & iRet & " TBLRUOLODICHIARATO per contribuente " & nIdContribuente & " Anno " & sAnno & "::IDPROVVEDIMENTO::" & nIdProvvedimento)

    '        If DeleteAtto(nIdProvvedimento) = False Then
    '            Return False
    '        End If
    '        Log.Debug("Cancellato Provvedimento e Procedimento per contribuente " & nIdContribuente & " Anno " & sAnno & "::IDPROVVEDIMENTO::" & nIdProvvedimento)
    '        'Else
    '        '    Log.Debug("Si è verificato un errore in ClsGestioneAccertamenti::DeleteAttoTARSU::Errore inizializzazione workflow")
    '        '    Return False
    '        'End If
    '        Return True
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.DeleteAttoTARSU.errore: ", Err)
    '        Return False
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdProvvedimento"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function DeleteAtto(ByVal nIdProvvedimento As Integer) As Boolean
        Dim sSQL As String

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_DeleteProvvedimento", "IDPROVVEDIMENTO", "OPERATORE")
                ctx.ExecuteNonQuery(sSQL, ctx.GetParam("IDPROVVEDIMENTO", nIdProvvedimento), ctx.GetParam("OPERATORE", ConstSession.UserName))
                ctx.Dispose()
            End Using
            Log.Debug("Cancellato IdProvvedimento:" & nIdProvvedimento)
            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.DeleteAtto.errore: ", Err)
            Return False
        End Try
    End Function
    'Public Function DeleteAtto(ByVal nIdProvvedimento As Integer) As Boolean ', ByVal objDBManager As DBManager
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim sSQL As String
    '    Dim iRet As Integer
    '    Try
    '        sSQL = "DELETE FROM PROVVEDIMENTI"
    '        sSQL += " WHERE ID_PROVVEDIMENTO=" & nIdProvvedimento
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        iRet = cmdMyCommand.ExecuteNonQuery()
    '        Log.Debug("Cancellato " & iRet & " PROVVEDIMENTI. IdProvvedimento:" & nIdProvvedimento)

    '        sSQL = "DELETE FROM TAB_PROCEDIMENTI"
    '        sSQL += " WHERE ID_PROVVEDIMENTO=" & nIdProvvedimento
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        iRet = cmdMyCommand.ExecuteNonQuery
    '        Log.Debug("Cancellato " & iRet & " PROCEDIMENTI. IdProvvedimento:" & nIdProvvedimento)

    '        Return True
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.DeleteAtto.errore: ", Err)
    '        Return False
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    'Private Function CalcolaValore(ByVal sUserName As String, ByVal sIdEnte As String, ByVal tipoImm As String, ByVal rendita As Decimal, ByVal AnnoAccertamento As Integer, ByVal Categoria As String, ByVal zona As String, ByVal datadal As String, ByVal Consistenza As Decimal) As String
    '    Dim ValoreImmobile As Decimal = 0
    '    Dim Rivalutazione As Decimal
    '    Dim TabTariffa As DataTable
    '    Dim strBuild As String
    '    Dim GruppoCategoria As String

    '    Try
    '        If (tipoImm.CompareTo("AF") = 0 Or tipoImm.CompareTo("LC") = 0) Then
    '            'devo vedere se c'è qualche tariffa configurata per la zona selezionata
    '            TabTariffa = New DichiarazioniICI.DatabaseOpengov.TariffeEstimoAFTable(sUserName).SelectTariffa(sIdEnte, zona, DateTime.Parse(datadal))
    '            Dim Tariffa As Decimal
    '            Dim valore As Decimal = 0
    '            If TabTariffa.Rows.Count > 0 Then
    '                Tariffa = Decimal.Parse(TabTariffa.Rows(0)("TARIFFA_EURO").ToString())
    '                'calcolo la rendita con i dati che possiedo
    '                valore = Consistenza * Tariffa
    '            Else
    '                valore = 0
    '            End If
    '            CalcolaValore = valore.ToString("N")
    '            Exit Function
    '        End If

    '        ' verifico se mi trovo a calcolare TA 
    '        If (tipoImm = "TA") Then
    '            ValoreImmobile = Decimal.Parse(rendita) * 75
    '            If (DateTime.Parse(datadal) >= DateTime.Parse("01/01/1997")) Then

    '                Rivalutazione = ValoreImmobile * 25 / 100
    '                ValoreImmobile = ValoreImmobile + Rivalutazione
    '            End If
    '            CalcolaValore = ValoreImmobile.ToString("N")
    '            Exit Function
    '        End If

    '        'se sono in presenza di categoria a/10 o c/1
    '        If (Categoria.CompareTo("A/10") = 0) Then

    '            ValoreImmobile = rendita * 50

    '            'calcolo la rivalutazione se immobile > 1997
    '            If (DateTime.Parse(datadal) >= DateTime.Parse("01/01/1997")) Then
    '                Rivalutazione = ValoreImmobile * 5 / 100
    '                ValoreImmobile = ValoreImmobile + Rivalutazione
    '            End If

    '            CalcolaValore = ValoreImmobile.ToString("N")
    '            Exit Function

    '        ElseIf (Categoria.CompareTo("C/1") = 0) Then

    '            ValoreImmobile = rendita * 34
    '            If (DateTime.Parse(datadal) >= DateTime.Parse("01/01/1997")) Then

    '                Rivalutazione = ValoreImmobile * 5 / 100
    '                ValoreImmobile = ValoreImmobile + Rivalutazione
    '            End If
    '            CalcolaValore = ValoreImmobile.ToString("N")
    '            Exit Function
    '        End If

    '        If Categoria <> "" Then

    '            GruppoCategoria = Categoria.Substring(0, 1)

    '            If (GruppoCategoria.CompareTo("A") = 0 Or GruppoCategoria.CompareTo("C") = 0) Then
    '                ValoreImmobile = rendita * 100
    '            End If

    '            If (GruppoCategoria.CompareTo("D") = 0) Then

    '                ValoreImmobile = rendita * 50
    '            ElseIf (GruppoCategoria.CompareTo("B") = 0) Then

    '                If (CInt(AnnoAccertamento) = 2006) Then
    '                    Dim RenditaMensile, valorePreOtt, valorePostOtt As Double

    '                    RenditaMensile = rendita / 12
    '                    valorePreOtt = RenditaMensile * 9 * 100
    '                    valorePostOtt = RenditaMensile * 3 * 140
    '                    ValoreImmobile = valorePreOtt + valorePostOtt

    '                ElseIf (CInt(AnnoAccertamento) < 2006) Then
    '                    ValoreImmobile = rendita * 100
    '                Else
    '                    ValoreImmobile = rendita * 140
    '                End If
    '            End If
    '            If (DateTime.Parse(datadal) >= DateTime.Parse("01/01/1997")) Then

    '                Rivalutazione = ValoreImmobile * 5 / 100
    '                ValoreImmobile = ValoreImmobile + Rivalutazione
    '            End If
    '        End If
    '        CalcolaValore = ValoreImmobile.ToString("N")
    '    Catch ex As Exception
    '        CalcolaValore = 0
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.CalcolaValore.errore: ", ex)
    '    End Try
    'End Function

    'Public Function UpdateRiepilogo(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sApplicazione As String, ByVal COD_ENTE As String, ByVal cod_tributo As String, ByVal ID_Provvedimento As Integer, ByVal objhashtableRIEPILOGO As Hashtable) As Boolean
    '    'Dim objSessione As CreateSessione
    '    Dim sSQL, strWFErrore As String
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    Dim objDBManager As DBManager
    '    Dim intRetVal As Integer
    '    Dim IMPORTO_DICHIARATO_F2, IMPORTO_VERSATO_F2, IMPORTO_DIFFERENZA_IMPOSTA_F2, IMPORTO_SANZIONI_F2, IMPORTO_INTERESSI_F2, IMPORTO_TOTALE_F2, IMPORTO_ACCERTATO_ACC, IMPORTO_DIFFERENZA_IMPOSTA_ACC, IMPORTO_SANZIONI_ACC, IMPORTO_SANZIONI_RIDOTTE_ACC, IMPORTO_INTERESSI_ACC, IMPORTO_TOTALE_ACC As String
    '    Try

    '        objSessione = New CreateSessione(sParametroENV, sUserName, sApplicazione)
    '        If Not objSessione.CreaSessione(sUserName, strWFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

    '        IMPORTO_DICHIARATO_F2 = Convert.ToString(objhashtableRIEPILOGO("TotImpICIDichiarato")).Replace(",", ".")
    '        IMPORTO_VERSATO_F2 = Convert.ToString(objhashtableRIEPILOGO("TotVersamenti")).Replace(",", ".")
    '        IMPORTO_DIFFERENZA_IMPOSTA_F2 = Convert.ToString(objhashtableRIEPILOGO("DIFASE2")).Replace(",", ".")
    '        IMPORTO_SANZIONI_F2 = Convert.ToString(objhashtableRIEPILOGO("SANZFASE2")).Replace(",", ".")
    '        IMPORTO_INTERESSI_F2 = Convert.ToString(objhashtableRIEPILOGO("INTFASE2")).Replace(",", ".")
    '        IMPORTO_TOTALE_F2 = Convert.ToString(objhashtableRIEPILOGO("TOTFASE2")).Replace(",", ".")
    '        IMPORTO_ACCERTATO_ACC = Convert.ToString(objhashtableRIEPILOGO("TotImpICIACCERTAMENTO")).Replace(",", ".")
    '        IMPORTO_DIFFERENZA_IMPOSTA_ACC = Convert.ToString(objhashtableRIEPILOGO("TotDiffImpostaACCERTAMENTO")).Replace(",", ".")
    '        IMPORTO_SANZIONI_ACC = Convert.ToString(objhashtableRIEPILOGO("TotImportoSanzioniACCERTAMENTO")).Replace(",", ".")
    '        IMPORTO_SANZIONI_RIDOTTE_ACC = Convert.ToString(objhashtableRIEPILOGO("TotImportoSanzioniRidottoACCERTAMENTO")).Replace(",", ".")
    '        IMPORTO_INTERESSI_ACC = Convert.ToString(objhashtableRIEPILOGO("TotImportoInteressiACCERTAMENTO")).Replace(",", ".")
    '        IMPORTO_TOTALE_ACC = Convert.ToString(objhashtableRIEPILOGO("ImportoTotaleAvviso")).Replace(",", ".")

    '        sSQL = "update provvedimenti set "
    '        sSQL += "IMPORTO_DICHIARATO_F2 =" & IMPORTO_DICHIARATO_F2 & ", "
    '        sSQL += "IMPORTO_VERSATO_F2 =" & IMPORTO_VERSATO_F2 & ", "
    '        sSQL += "IMPORTO_DIFFERENZA_IMPOSTA_F2 =" & IMPORTO_DIFFERENZA_IMPOSTA_F2 & ", "
    '        sSQL += "IMPORTO_SANZIONI_F2 =" & IMPORTO_SANZIONI_F2 & ", "
    '        sSQL += "IMPORTO_INTERESSI_F2 =" & IMPORTO_INTERESSI_F2 & ", "
    '        sSQL += "IMPORTO_TOTALE_F2  =" & IMPORTO_TOTALE_F2 & ", "
    '        sSQL += "IMPORTO_ACCERTATO_ACC =" & IMPORTO_ACCERTATO_ACC & ", "
    '        sSQL += "IMPORTO_DIFFERENZA_IMPOSTA_ACC =" & IMPORTO_DIFFERENZA_IMPOSTA_ACC & ", "
    '        sSQL += "IMPORTO_SANZIONI_ACC =" & IMPORTO_SANZIONI_ACC & ", "
    '        sSQL += "IMPORTO_SANZIONI_RIDOTTE_ACC =" & IMPORTO_SANZIONI_RIDOTTE_ACC & ", "
    '        sSQL += "IMPORTO_INTERESSI_ACC =" & IMPORTO_INTERESSI_ACC & ", "
    '        sSQL += "IMPORTO_TOTALE_ACC =" & IMPORTO_TOTALE_ACC

    '        sSQL += " where id_provvedimento=" & ID_Provvedimento
    '        sSQL += " and cod_ente='" & COD_ENTE & "'"
    '        sSQL += " and cod_tributo='" & cod_tributo & "'"


    '        objDBManager = New DBManager
    '        If objDBManager.Initialize( ConstSession.StringConnection)) Then
    '            intRetVal = objDBManager.Execute(sSQL)

    '            If intRetVal = -1 Then
    '                Log.Error("Application::ClsGestioneAccertamenti::Function::UpdateRiepilogo::Update Fallito")
    '                Throw New Exception("Application::ClsGestioneAccertamenti::Function::UpdateRiepilogo::Update Fallito")
    '            End If

    '        Else
    '            Throw New Exception("Errore inizializzazione workflow")
    '        End If
    '    Catch ex As Exception
    '        UpdateRiepilogo = False
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.UpdateRiepilogo.errore: ", Err)
    '    Finally
    '        UpdateRiepilogo = True
    '        objDBManager.Kill()
    '    End Try
    'End Function
    Public Function UpdateRiepilogo(ByVal COD_ENTE As String, ByVal cod_tributo As String, ByVal ID_Provvedimento As Integer, ByVal objhashtableRIEPILOGO As Hashtable) As Boolean
        Dim sSQL As String
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim intRetVal As Integer
        Dim IMPORTO_DICHIARATO_F2, IMPORTO_VERSATO_F2, IMPORTO_DIFFERENZA_IMPOSTA_F2, IMPORTO_SANZIONI_F2, IMPORTO_INTERESSI_F2, IMPORTO_TOTALE_F2, IMPORTO_ACCERTATO_ACC, IMPORTO_DIFFERENZA_IMPOSTA_ACC, IMPORTO_SANZIONI_ACC, IMPORTO_SANZIONI_RIDOTTE_ACC, IMPORTO_INTERESSI_ACC, IMPORTO_TOTALE_ACC As String

        Try
            IMPORTO_DICHIARATO_F2 = Convert.ToString(objhashtableRIEPILOGO("TotImpICIDichiarato")).Replace(",", ".")
            IMPORTO_VERSATO_F2 = Convert.ToString(objhashtableRIEPILOGO("TotVersamenti")).Replace(",", ".")
            IMPORTO_DIFFERENZA_IMPOSTA_F2 = Convert.ToString(objhashtableRIEPILOGO("DIFASE2")).Replace(",", ".")
            IMPORTO_SANZIONI_F2 = Convert.ToString(objhashtableRIEPILOGO("SANZFASE2")).Replace(",", ".")
            IMPORTO_INTERESSI_F2 = Convert.ToString(objhashtableRIEPILOGO("INTFASE2")).Replace(",", ".")
            IMPORTO_TOTALE_F2 = Convert.ToString(objhashtableRIEPILOGO("TOTFASE2")).Replace(",", ".")
            IMPORTO_ACCERTATO_ACC = Convert.ToString(objhashtableRIEPILOGO("TotImpICIACCERTAMENTO")).Replace(",", ".")
            IMPORTO_DIFFERENZA_IMPOSTA_ACC = Convert.ToString(objhashtableRIEPILOGO("TotDiffImpostaACCERTAMENTO")).Replace(",", ".")
            IMPORTO_SANZIONI_ACC = Convert.ToString(objhashtableRIEPILOGO("TotImportoSanzioniACCERTAMENTO")).Replace(",", ".")
            IMPORTO_SANZIONI_RIDOTTE_ACC = Convert.ToString(objhashtableRIEPILOGO("TotImportoSanzioniRidottoACCERTAMENTO")).Replace(",", ".")
            IMPORTO_INTERESSI_ACC = Convert.ToString(objhashtableRIEPILOGO("TotImportoInteressiACCERTAMENTO")).Replace(",", ".")
            IMPORTO_TOTALE_ACC = Convert.ToString(objhashtableRIEPILOGO("ImportoTotaleAvviso")).Replace(",", ".")

            If IMPORTO_DICHIARATO_F2 = "" Then
                IMPORTO_DICHIARATO_F2 = "0"
            End If
            If IMPORTO_VERSATO_F2 = "" Then
                IMPORTO_VERSATO_F2 = "0"
            End If
            If IMPORTO_DIFFERENZA_IMPOSTA_F2 = "" Then
                IMPORTO_DIFFERENZA_IMPOSTA_F2 = "0"
            End If
            If IMPORTO_SANZIONI_F2 = "" Then
                IMPORTO_SANZIONI_F2 = "0"
            End If
            If IMPORTO_INTERESSI_F2 = "" Then
                IMPORTO_INTERESSI_F2 = "0"
            End If
            If IMPORTO_TOTALE_F2 = "" Then
                IMPORTO_TOTALE_F2 = "0"
            End If
            If IMPORTO_ACCERTATO_ACC = "" Then
                IMPORTO_ACCERTATO_ACC = "0"
            End If
            If IMPORTO_DIFFERENZA_IMPOSTA_ACC = "" Then
                IMPORTO_DIFFERENZA_IMPOSTA_ACC = "0"
            End If
            If IMPORTO_SANZIONI_ACC = "" Then
                IMPORTO_SANZIONI_ACC = "0"
            End If
            If IMPORTO_SANZIONI_RIDOTTE_ACC = "" Then
                IMPORTO_SANZIONI_RIDOTTE_ACC = "0"
            End If
            If IMPORTO_INTERESSI_ACC = "" Then
                IMPORTO_INTERESSI_ACC = "0"
            End If
            If IMPORTO_TOTALE_ACC = "" Then
                IMPORTO_TOTALE_ACC = "0"
            End If
            sSQL = "UPDATE PROVVEDIMENTI SET "
            sSQL += "IMPORTO_DICHIARATO_F2 =" & IMPORTO_DICHIARATO_F2 & ", "
            sSQL += "IMPORTO_VERSATO_F2 =" & IMPORTO_VERSATO_F2 & ", "
            sSQL += "IMPORTO_DIFFERENZA_IMPOSTA_F2 =" & IMPORTO_DIFFERENZA_IMPOSTA_F2 & ", "
            sSQL += "IMPORTO_SANZIONI_F2 =" & IMPORTO_SANZIONI_F2 & ", "
            sSQL += "IMPORTO_INTERESSI_F2 =" & IMPORTO_INTERESSI_F2 & ", "
            sSQL += "IMPORTO_TOTALE_F2  =" & IMPORTO_TOTALE_F2 & ", "
            sSQL += "IMPORTO_ACCERTATO_ACC =" & IMPORTO_ACCERTATO_ACC & ", "
            sSQL += "IMPORTO_DIFFERENZA_IMPOSTA_ACC =" & IMPORTO_DIFFERENZA_IMPOSTA_ACC & ", "
            sSQL += "IMPORTO_SANZIONI_ACC =" & IMPORTO_SANZIONI_ACC & ", "
            sSQL += "IMPORTO_SANZIONI_RIDOTTE_ACC =" & IMPORTO_SANZIONI_RIDOTTE_ACC & ", "
            sSQL += "IMPORTO_INTERESSI_ACC =" & IMPORTO_INTERESSI_ACC & ", "
            sSQL += "IMPORTO_TOTALE_ACC =" & IMPORTO_TOTALE_ACC
            sSQL += " WHERE ID_PROVVEDIMENTO=" & ID_Provvedimento
            sSQL += " AND COD_ENTE='" & COD_ENTE & "'"
            sSQL += " AND COD_TRIBUTO='" & cod_tributo & "'"

            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            intRetVal = cmdMyCommand.ExecuteNonQuery
            If intRetVal = -1 Then
                Log.Error("Application::ClsGestioneAccertamenti::Function::UpdateRiepilogo::Update Fallito")
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.UpdateRiepilogo.query->" + sSQL)
                Throw New Exception("Application::ClsGestioneAccertamenti::Function::UpdateRiepilogo::Update Fallito")
            Else
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.UpdateRiepilogo.query->" + sSQL)
                Throw New Exception("Errore inizializzazione workflow")
            End If
        Catch ex As Exception
            UpdateRiepilogo = False
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.UpdateRiepilogo.errore: ", ex)
        Finally
            UpdateRiepilogo = True
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Function DeterminaTipoAvviso(ByVal ImpTotAvviso As Double, ByVal ImpTotPreAccertamento As Double, ByVal ImpSoglia As Double, ByVal bIsRettifica As Boolean, ByVal nTipoAvvisoPreAcc As Integer, ByRef nTipoAvviso As Integer, ByRef sTipoAvviso As String) As Boolean
        'Public Function DeterminaTipoAvviso(ByVal ImpTotAvviso As Double, ByVal ImpTotPreAccertamento As Double, ByVal ImpSoglia As Double, ByVal bIsRettifica As Boolean, ByVal nTipoAvvisoPreAcc As Integer, ByRef nTipoAvviso As Integer, ByRef sTipoAvviso As String, ByRef oMyAttoTARSU As OggettoAttoTARSU, ByRef oMyAttoICI As DataSet) As Boolean
        Try
            If ImpTotAvviso > 0 Or ImpTotPreAccertamento > 0 Then
                If bIsRettifica Then
                    If ((ImpTotAvviso + ImpTotPreAccertamento) >= ImpSoglia * -1 And (ImpTotAvviso + ImpTotPreAccertamento) <= ImpSoglia) Then
                        nTipoAvviso = OggettoAtto.Provvedimento.AutotutelaAnnullamento
                    Else
                        nTipoAvviso = OggettoAtto.Provvedimento.AutotutelaRettifica
                    End If
                Else
                    'AVVISO
                    'per determinare il tipo di avviso si prende "lo stato" più grave tra 
                    'lo stato del pre accertamento e lo stato dell'accertamento
                    If (nTipoAvvisoPreAcc = OggettoAtto.Provvedimento.AccertamentoUfficio) Or (nTipoAvviso = OggettoAtto.Provvedimento.AccertamentoUfficio) Then
                        'se PREACCERTAMENTO o ACCERTAMENTO hanno scaturito un avviso di accertamento d'ufficio
                        nTipoAvviso = OggettoAtto.Provvedimento.AccertamentoUfficio 'AVVISO DI ACCERTAMENTO D'UFFICIO                                    
                    ElseIf (nTipoAvvisoPreAcc = OggettoAtto.Provvedimento.AccertamentoRettifica) Or (nTipoAvviso = OggettoAtto.Provvedimento.AccertamentoRettifica) Then
                        'se PREACCERTAMENTO o ACCERTAMENTO hanno scaturito un avviso di accertamento in rettifica
                        nTipoAvviso = OggettoAtto.Provvedimento.AccertamentoRettifica  'AVVISO DI ACCERTAMENTO IN RETTIFICA                                    
                    Else
                        nTipoAvviso = OggettoAtto.Provvedimento.AccertamentoRettifica 'AVVISO DI ACCERTAMENTO IN RETTIFICA
                    End If
                End If
            ElseIf ImpTotPreAccertamento < 0 Then
                nTipoAvviso = OggettoAtto.Provvedimento.Rimborso  'AVVISO DI ACCERTAMENTO IN RETTIFICA                                
            ElseIf ImpTotPreAccertamento = 0 Then
                If bIsRettifica Then
                    nTipoAvviso = OggettoAtto.Provvedimento.AutotutelaAnnullamento
                Else
                    'PREACCERTAMENTO E ACCERTAMENTOHANNO DATO ESITO POSITIVO
                    'NO AVVISO - NON CREO IL PROVVEDIMENTO
                    nTipoAvviso = OggettoAtto.Provvedimento.NoAvviso
                End If
            End If
            'Rimborso 
            If ImpTotAvviso < 0 Then
                'If DiffTotaleSanzInt < 0 Then 'GIULIA 09082005
                If bIsRettifica Then 'se sono in rettifica e l'importo è minore della ImpSoglia diventa annullamento
                    If ImpTotAvviso <= ImpSoglia And ImpTotAvviso >= ImpSoglia * -1 Then
                        nTipoAvviso = OggettoAtto.Provvedimento.AutotutelaAnnullamento
                    Else
                        nTipoAvviso = OggettoAtto.Provvedimento.Rimborso '5
                    End If
                Else
                    nTipoAvviso = OggettoAtto.Provvedimento.Rimborso '5
                End If
            End If

            Select Case (nTipoAvviso)
                Case OggettoAtto.Provvedimento.Rimborso
                    sTipoAvviso = "Avviso di rimborso"
                Case OggettoAtto.Provvedimento.AccertamentoRettifica
                    sTipoAvviso = "Avviso di accertamento in rettifica"
                Case OggettoAtto.Provvedimento.AccertamentoUfficio
                    sTipoAvviso = "Avviso di accertamento d'ufficio"
                Case OggettoAtto.Provvedimento.AutotutelaRettifica
                    sTipoAvviso = "Avviso di Autotutela in rettifica"
                Case OggettoAtto.Provvedimento.AutotutelaAnnullamento
                    sTipoAvviso = "Avviso di Autotutela di annullamento"
                Case OggettoAtto.Provvedimento.NoAvviso
                    sTipoAvviso = "Nessun avviso emesso"
                Case Else
                    sTipoAvviso = "Avviso non determinato"
            End Select

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.DeterminaTipoAvviso.errore: ", ex)
            Return False
        End Try
    End Function

    '*** 20140701 - IMU/TARES ***
    Public Sub LoadTipoCalcolo(ByVal IdEnte As String, ByVal Anno As String, ByRef LblTipoCalcolo As Label, ByRef LblTipoMQ As Label, ByRef ChkConferimenti As CheckBox, ByRef ChkMaggiorazione As CheckBox, ByRef txtInizioConf As TextBox)
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow

        Try
            LblTipoCalcolo.Text = " -- "
            LblTipoMQ.Text = " -- "
            ChkConferimenti.Checked = False
            ChkMaggiorazione.Checked = False

            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnectionTARSU)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetTipoCalcolo"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = ConstSession.IdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.NVarChar)).Value = Anno
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            dtMyDati.Load(myDataReader)
            For Each dtMyRow In dtMyDati.Rows
                LblTipoCalcolo.Text = dtMyRow("tipocalcolo")
                Select Case dtMyRow("tipomq").ToString()
                    Case "D"
                        LblTipoMQ.Text = "Dichiarate"
                    Case "C"
                        LblTipoMQ.Text = "Catastali"
                End Select
                ChkConferimenti.Checked = dtMyRow("hasconferimenti")
                ChkMaggiorazione.Checked = dtMyRow("hasmaggiorazione")
                txtInizioConf.Text = New OPENgovTIA.Formatta.FunctionGrd().FormattaDataGrd(dtMyRow("data_inizio_conferimenti").ToString())
            Next
            ChkConferimenti.Enabled = False : ChkMaggiorazione.Enabled = False
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.LoadTipoCalcolo.errore: ", ex)
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub

    Public Function ArticoloTOArticoloAccertamento(ByVal myArticolo As ObjArticolo, ByVal bHasLegamiInID As Boolean) As ObjArticoloAccertamento
        Dim myItem As New ObjArticoloAccertamento

        Try
            If bHasLegamiInID Then
                myItem.Progressivo = myArticolo.Id
                myItem.IdLegame = myArticolo.Id
            End If
            myItem.Id = myArticolo.Id
            myItem.IdArticolo = myArticolo.IdArticolo
            myItem.TipoPartita = myArticolo.TipoPartita
            myItem.nComponentiPV = myArticolo.nComponentiPV
            myItem.bForzaPV = myArticolo.bForzaPV
            myItem.IdContribuente = myArticolo.IdContribuente
            myItem.IdDettaglioTestata = myArticolo.IdDettaglioTestata
            myItem.IdAvviso = myArticolo.IdAvviso
            myItem.IdEnte = myArticolo.IdEnte
            myItem.sAnno = myArticolo.sAnno
            myItem.sVia = myArticolo.sVia
            myItem.sCivico = myArticolo.sCivico
            myItem.sEsponente = myArticolo.sEsponente
            myItem.sInterno = myArticolo.sInterno
            myItem.sScala = myArticolo.sScala
            myItem.sFoglio = myArticolo.sFoglio
            myItem.sNumero = myArticolo.sNumero
            myItem.sSubalterno = myArticolo.sSubalterno
            myItem.nIdTitoloOccupaz = myArticolo.nIdTitoloOccupaz
            myItem.nIdNaturaOccupaz = myArticolo.nIdNaturaOccupaz
            myItem.nIdDestUso = myArticolo.nIdDestUso
            myItem.sIdTipoUnita = myArticolo.sIdTipoUnita
            myItem.sIdTipoParticella = myArticolo.sIdTipoParticella
            myItem.nIdAssenzaDatiCatastali = myArticolo.nIdAssenzaDatiCatastali
            myItem.sSezione = myArticolo.sSezione
            myItem.sEstensioneParticella = myArticolo.sEstensioneParticella
            myItem.sCategoria = myArticolo.sCategoria
            myItem.sDescrCategoria = myArticolo.sDescrCategoria
            myItem.nComponenti = myArticolo.nComponenti
            myItem.nIdTariffa = myArticolo.nIdTariffa
            myItem.impTariffa = myArticolo.impTariffa
            myItem.nMQ = myArticolo.nMQ
            myItem.nBimestri = myArticolo.nBimestri
            myItem.impRuolo = myArticolo.impRuolo
            myItem.impRiduzione = myArticolo.impRiduzione
            myItem.impDetassazione = myArticolo.impDetassazione
            myItem.impNetto = myArticolo.impNetto
            myItem.nIdFlussoRuolo = myArticolo.nIdFlussoRuolo
            myItem.bIsImportoForzato = myArticolo.bIsImportoForzato
            myItem.bIsTarsuGiornaliera = myArticolo.bIsTarsuGiornaliera
            myItem.tDataInizio = myArticolo.tDataInizio
            myItem.tDataFine = myArticolo.tDataFine
            myItem.sNote = myArticolo.sNote
            myItem.sTipoRuolo = myArticolo.sTipoRuolo
            myItem.nCodVia = myArticolo.nCodVia
            myItem.oRiduzioni = myArticolo.oRiduzioni
            myItem.oDetassazioni = myArticolo.oDetassazioni
            myItem.tDataInserimento = myArticolo.tDataInserimento
            myItem.tDataVariazione = myArticolo.tDataVariazione
            myItem.tDataCessazione = myArticolo.tDataCessazione
            myItem.sOperatore = myArticolo.sOperatore
            Return myItem
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.ArticoloTOArticoloAccertamento.errore: ", ex)
            Return Nothing
        End Try
    End Function

    Public Function ArticoloAccertamentoTOArticolo(ByVal myArticoloAcc As ObjArticoloAccertamento, ByVal sTipoTassazione As String) As ObjArticolo
        Dim myItem As New ObjArticolo

        Try
            If sTipoTassazione = ObjRuolo.TipoCalcolo.TARSU Then
                myItem.Id = myArticoloAcc.IdLegame
            Else
                myItem.Id = myArticoloAcc.Progressivo
            End If
            myItem.IdArticolo = myArticoloAcc.IdLegame
            myItem.TipoPartita = myArticoloAcc.TipoPartita
            myItem.nComponentiPV = myArticoloAcc.nComponentiPV
            myItem.bForzaPV = myArticoloAcc.bForzaPV
            myItem.IdContribuente = myArticoloAcc.IdContribuente
            myItem.IdDettaglioTestata = myArticoloAcc.IdDettaglioTestata
            myItem.IdAvviso = myArticoloAcc.IdAvviso
            myItem.IdEnte = myArticoloAcc.IdEnte
            myItem.sAnno = myArticoloAcc.sAnno
            myItem.sVia = myArticoloAcc.sVia
            myItem.sCivico = myArticoloAcc.sCivico
            myItem.sEsponente = myArticoloAcc.sEsponente
            myItem.sInterno = myArticoloAcc.sInterno
            myItem.sScala = myArticoloAcc.sScala
            myItem.sFoglio = myArticoloAcc.sFoglio
            myItem.sNumero = myArticoloAcc.sNumero
            myItem.sSubalterno = myArticoloAcc.sSubalterno
            myItem.nIdTitoloOccupaz = myArticoloAcc.nIdTitoloOccupaz
            myItem.nIdNaturaOccupaz = myArticoloAcc.nIdNaturaOccupaz
            myItem.nIdDestUso = myArticoloAcc.nIdDestUso
            myItem.sIdTipoUnita = myArticoloAcc.sIdTipoUnita
            myItem.sIdTipoParticella = myArticoloAcc.sIdTipoParticella
            myItem.nIdAssenzaDatiCatastali = myArticoloAcc.nIdAssenzaDatiCatastali
            myItem.sSezione = myArticoloAcc.sSezione
            myItem.sEstensioneParticella = myArticoloAcc.sEstensioneParticella
            myItem.sCategoria = myArticoloAcc.sCategoria
            myItem.sDescrCategoria = myArticoloAcc.sDescrCategoria
            myItem.nComponenti = myArticoloAcc.nComponenti
            myItem.nIdTariffa = myArticoloAcc.nIdTariffa
            myItem.impTariffa = myArticoloAcc.impTariffa
            myItem.nMQ = myArticoloAcc.nMQ
            myItem.nBimestri = myArticoloAcc.nBimestri
            myItem.impRuolo = myArticoloAcc.impRuolo
            myItem.impRiduzione = myArticoloAcc.impRiduzione
            myItem.impDetassazione = myArticoloAcc.impDetassazione
            myItem.impNetto = myArticoloAcc.impNetto
            myItem.nIdFlussoRuolo = myArticoloAcc.nIdFlussoRuolo
            myItem.bIsImportoForzato = myArticoloAcc.bIsImportoForzato
            myItem.bIsTarsuGiornaliera = myArticoloAcc.bIsTarsuGiornaliera
            myItem.tDataInizio = myArticoloAcc.tDataInizio
            myItem.tDataFine = myArticoloAcc.tDataFine
            myItem.sNote = myArticoloAcc.sNote
            myItem.sTipoRuolo = myArticoloAcc.sTipoRuolo
            myItem.nCodVia = myArticoloAcc.nCodVia
            myItem.oRiduzioni = myArticoloAcc.oRiduzioni
            myItem.oDetassazioni = myArticoloAcc.oDetassazioni
            myItem.tDataInserimento = myArticoloAcc.tDataInserimento
            myItem.tDataVariazione = myArticoloAcc.tDataVariazione
            myItem.tDataCessazione = myArticoloAcc.tDataCessazione
            myItem.sOperatore = myArticoloAcc.sOperatore
            Return myItem
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.ArticoloAccertamentoTOArticolo.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che confronta dichiarato ed accertato e ne calcola il provvedimento.
    ''' Determino il Tipo di Avviso (Accertamento o Ufficio); se trovo 1 immobile di accertato che non è in dichiarato ho avviso di Tipo = 4 altrimenti avviso di tipo 3; Cerco gli immobili accertati in tutti gli immobili di dichiarato; se non trovo tipo avviso = 3.
    ''' se il calcolo è di tipo TARES è come se non ci fosse mai il dichiarato. ciclo sull'oggetto totale per verificare il tipo di atto; accertato maggiore dichiarato popolo atto; CONFRONTO ACCERTATO E PAGATO popolo differenza d'imposta.
    ''' Calcolo gli interessi Attivi sul singolo immobile. Devo effettuare il calcolo guardando se è un rimborso sull'immobile oppure no; se è rimborso ho solo interessi attivi.
    ''' Calcolo Sanzioni e Interessi su singolo Immobile (la procedura di calcolo delle sanzioni le calcola solo se l'importo è positivo).
    ''' Se sono l'ici dichiarato è uguale a quello accertato Sanzioni e Interessi a 0.
    ''' estraggo le addizionali che devono essere applicate se il check è selezionato; per TARSU uso l'imposta dell'atto per TARES uso tutto tranne maggiorazione.
    ''' Reperisco soglia; calcolo spese; aggiorno l'importo totale comprensivo di arrotondamento; DETERMINO TIPO DI AVVISO.
    ''' Se accertato minore dichiarato Non emetto Avviso Non esiste rimborso.
    ''' Richiamo il srevizio per il salvataggio sul database.
    ''' </summary>
    ''' <param name="sTipoCalcolo"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="sAnnoRiferimento"></param>
    ''' <param name="oDichiarato"></param>
    ''' <param name="oAccertato"></param>
    ''' <param name="blnTIPO_OPERAZIONE_RETTIFICA"></param>
    ''' <param name="objHashTable"></param>
    ''' <param name="bCalcolaAddizionali"></param>
    ''' <param name="objDSMotivazioniSanzioni"></param>
    ''' <param name="sValRitornoAccertamento"></param>
    ''' <param name="sDescTipoAvviso"></param>
    ''' <param name="sScript"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="05/12/2011">
    ''' <strong>le spese devono essere messe dopo l'arrotondamento</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="01/07/2014">
    ''' <strong>IMU/TARES</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="10/2018">
    ''' <strong>Generazione Massiva Atti</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory><revision date="12/11/2019">il calcolo interessi 8852/TASI deve essere fatto in acconto/saldo o in unica soluzione in base alla configurazione di TP_GENERALE_ICI</revision></revisionHistory>
    Public Function TARSUConfrontoAccertatoDichiarato(ByVal sTipoCalcolo As String, ByVal sIdEnte As String, ByVal sAnnoRiferimento As String, ByVal oDichiarato() As ObjArticoloAccertamento, ByVal oAccertato() As ObjArticoloAccertamento, ByVal blnTIPO_OPERAZIONE_RETTIFICA As Boolean, ByVal objHashTable As Hashtable, ByVal bCalcolaAddizionali As Boolean, ByVal objDSMotivazioniSanzioni As DataSet, ByVal sValRitornoAccertamento As Integer, sDataMorte As String, ByRef sDescTipoAvviso As String, ByRef sScript As String) As OggettoAttoTARSU
        Dim FncGestAcc As New ClsGestioneAccertamenti
        Dim x, y, nList, i As Integer
        Dim nLegamePrec As Integer = -1
        Dim oDettaglioAtto() As OggettoDettaglioAtto
        Dim oListDettaglioAtto As OggettoDettaglioAtto

        Dim ImpTotAccert As Double = 0
        Dim ImpTotDich As Double = 0
        Dim oAtto As New OggettoAttoTARSU
        Dim oPagatoTARSU() As OggettoPagamenti
        Dim ClsPagato As New OPENgovTIA.ClsGestPag

        Dim DiffTotaleSanzioni As Double     'Somma algebrica fra sanzioni e interessi x det. tipo avviso
        Dim DiffTotaleInteressi As Double     'Somma algebrica fra sanzioni e interessi x det. tipo avviso
        Dim DiffTotaleSanzInt As Double     'Somma algebrica fra sanzioni e interessi x det. tipo avviso

        Dim OggettoRiepilogoAccertamento As ObjArticoloAccertamento()
        Dim blnCalcolaInteressi As Boolean

        Dim lngNewID_PROVVEDIMENTO As Long
        Dim DATA_RETTIFICA_ANNULLAMENTO As String

        Dim ObjAddizionaleServizio() As OggettoAddizionaleAccertamento = Nothing

        Dim blnResult As Boolean = False
        Dim strConnectionStringAnagrafica As String
        Dim strConnectionStringTARSU As String
        Dim objCOMUpdateDBAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
        Dim bConsentiSanzNeg As Boolean = True
        Dim impMinimoSanzione As Double = 0
        Dim nQuotaRiduzioneSanzioni As Integer = 4
        Dim TipoProvvedimento As String
        Dim ListBaseCalc As New ArrayList

        Try
            If Not oAccertato Is Nothing Then
                'Determino il Tipo di Avviso (Accertamento o Ufficio); se trovo 1 immobile di accertato che non è in dichiarato ho avviso di Tipo = 4 altrimenti avviso di tipo 3
                Dim DichiaratoNothing As New ObjArticoloAccertamento
                If oDichiarato Is Nothing Then
                    ReDim Preserve oDichiarato(0)
                    oDichiarato(0) = DichiaratoNothing
                End If
                Dim TipoAvviso As Integer

                TipoAvviso = OggettoAtto.Provvedimento.AccertamentoRettifica   '4

                'Prendo immobili di accertato
                For i = 0 To oAccertato.Length - 1
                    Dim Trovato As Boolean = False
                    'Cerco l'immobili in tutti gli immobili di dichiarato Se lo trovo esco dal ciclo e proseguo a cercare con immobili successivo di accertamento
                    For y = 0 To oDichiarato.Length - 1
                        If oAccertato(i).IdLegame = oDichiarato(y).IdLegame Then
                            Trovato = True
                            Exit For
                        End If
                    Next

                    'Se trovato = False vuol dire che non ho trovato l'immobile
                    If Trovato = False Then
                        'Avviso D'ufficio
                        TipoAvviso = OggettoAtto.Provvedimento.AccertamentoUfficio    '3
                        Exit For
                    End If
                Next

                nList = -1
                'CONFRONTO ACCERTATO E DICHIARATO
                'ciclo su tutti i record accertati
                For x = 0 To oAccertato.GetUpperBound(0)
                    'sommo gli importi a parità di livello di legame
                    oListDettaglioAtto = New OggettoDettaglioAtto
                    If oAccertato(x).IdLegame <> nLegamePrec Then
                        'aggiungo un record
                        nList += 1
                        oListDettaglioAtto = New OggettoDettaglioAtto
                        oListDettaglioAtto.IdLegame = oAccertato(x).IdLegame
                        oListDettaglioAtto.Progressivo = oAccertato(x).Progressivo
                        oListDettaglioAtto.Sanzioni = oAccertato(x).Sanzioni
                        oListDettaglioAtto.Interessi = oAccertato(x).Interessi
                        oListDettaglioAtto.Calcola_Interessi = oAccertato(x).Calcola_Interessi
                    End If
                    'sommo l'importo
                    oListDettaglioAtto.ImpAccertato += oAccertato(x).impNetto
                    If nList < 0 Then nList = 0
                    ReDim Preserve oDettaglioAtto(nList)
                    oDettaglioAtto(nList) = oListDettaglioAtto
                Next

                'se il calcolo è di tipo TARES è come se non ci fosse mai il dichiarato
                If sTipoCalcolo <> ObjRuolo.TipoCalcolo.TARES Then
                    'ciclo su tutti i record dichiarati
                    For x = 0 To oDichiarato.GetUpperBound(0)
                        'cerco il corrispettivo legame nell'oggetto di confronto
                        For y = 0 To oDettaglioAtto.GetUpperBound(0)
                            If oDettaglioAtto(y).IdLegame = oDichiarato(x).IdLegame Then
                                oDettaglioAtto(y).ImpDichiarato += oDichiarato(x).impNetto
                                Exit For
                            End If
                        Next
                    Next
                End If
                'ciclo sull'oggetto totale per verificare il tipo di atto
                nList = -1
                For x = 0 To oDettaglioAtto.GetUpperBound(0)
                    ImpTotAccert += FormatNumber(oDettaglioAtto(x).ImpAccertato, 2)
                    ImpTotDich += FormatNumber(oDettaglioAtto(x).ImpDichiarato, 2)
                Next
                nList += 1
                If ImpTotAccert >= ImpTotDich Then
                    'accertato maggiore dichiarato
                    oAtto = TARSU_PopolaAtto(sIdEnte, oDettaglioAtto)
                    oAtto.ANNO = oAccertato(0).sAnno
                    If Not oAtto Is Nothing Then
                        'SE HO UN ATTO CONFRONTO ACCERTATO E PAGATO popolo differenza d'imposta
                        Dim SearchParamPag As New OPENgovTIA.ObjSearchPagamenti
                        SearchParamPag.IdContribuente = oAtto.COD_CONTRIBUENTE
                        SearchParamPag.sAnnoRif = oAtto.ANNO
                        SearchParamPag.bRicPag = False
                        oPagatoTARSU = ClsPagato.GetListPagamenti(SearchParamPag, ConstSession.StringConnectionTARSU)
                        If Not oPagatoTARSU Is Nothing Then
                            Dim IPagato As Integer
                            For IPagato = 0 To oPagatoTARSU.Length - 1
                                oAtto.IMPORTO_PAGATO += oPagatoTARSU(IPagato).dImportoPagamento
                            Next
                        Else
                            oAtto.IMPORTO_PAGATO = 0
                        End If

                        Dim TipoAvvisoRimborso As Integer = -1
                        'Rimborso. Calcolo gli interessi Attivi sul singolo immobile.
                        'Al giro dopo la var viene azzerrata a -1.
                        If oAtto.IMPORTO_DIFFERENZA_IMPOSTA < 0 Then
                            TipoAvvisoRimborso = OggettoAtto.Provvedimento.Rimborso '"5"
                        End If

                        Dim oCalcoloSanzioniInteressi As New ObjBaseIntSanz
                        Dim dtInteressi As New DataTable
                        Dim dtSanzioni As New DataTable
                        Dim objDSCalcoloSanzioniInteressiAppoggio As DataSet
                        Dim TotImportoSanzioniACCERTAMENTO As Double = 0        'Totale Sanzioni atto di accertamento
                        Dim TotImportoSanzioniRidottoACCERTAMENTO As Double = 0 'Totale Sanzioni atto di accertamento
                        Dim TotImportoInteressiACCERTAMENTO As Double = 0       'Totale Interessi atto di accertamento
                        Dim TotDiffImpostaACCERTAMENTO As Double = 0            'Importo Totale Differenza di imposta atto di accertamento
                        Dim TotDiffImpostaAccontoACCERTAMENTO As Double = 0     'Importo Totale Differenza di imposta atto ACCONTO di accertamento
                        Dim TotDiffImpostaSaldoACCERTAMENTO As Double = 0       'Importo Totale Differenza di imposta SALDO atto di accertamento

                        Dim xDettaglio As Integer
                        Dim objDSSanzioni As New DataSet
                        Dim ListInteressi() As ObjInteressiSanzioni

                        If objHashTable.ContainsKey("CODCONTRIBUENTE") = False Then
                            objHashTable.Add("CODCONTRIBUENTE", oAtto.COD_CONTRIBUENTE)
                        End If
                        If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = True Then
                            objHashTable.Remove("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI")
                        End If
                        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

                        strConnectionStringAnagrafica = ConstSession.StringConnectionAnagrafica 'objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEANAGRAFICA")).GetConnection.ConnectionString
                        objHashTable("CONNECTIONSTRINGANAGRAFICA") = strConnectionStringAnagrafica
                        objHashTable("CODTRIBUTO") = Utility.Costanti.TRIBUTO_TARSU
                        objHashTable("CODENTE") = sIdEnte
                        If objHashTable.ContainsKey("TIPOPROVVEDIMENTO") = True Then
                            objHashTable.Remove("TIPOPROVVEDIMENTO")
                        End If
                        'TipoAvvisoRimborso = -1 == False ----> Non è un rimborso
                        'Devo effettuare il calcolo guardando se è un rimborso sull'immobile oppure no; se è rimborso ho solo interessi attivi.
                        If TipoAvvisoRimborso = -1 Then
                            objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
                            TipoProvvedimento = TipoAvviso
                        Else
                            objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvvisoRimborso)
                            TipoProvvedimento = TipoAvvisoRimborso
                        End If
                        If objHashTable.ContainsKey("COD_TIPO_PROCEDIMENTO") = True Then
                            objHashTable("COD_TIPO_PROCEDIMENTO") = OggettoAtto.Procedimento.Accertamento
                        Else
                            objHashTable.Add("COD_TIPO_PROCEDIMENTO", OggettoAtto.Procedimento.Accertamento)
                        End If
                        If objHashTable.Contains("ANNOACCERTAMENTO") = False Then
                            objHashTable.Add("ANNOACCERTAMENTO", sAnnoRiferimento)
                        End If
                        'PER CONNESSIONE TARSU
                        objHashTable("IDSOTTOAPPLICAZIONETARSU") = ConfigurationManager.AppSettings("OPENGOVTA")
                        strConnectionStringTARSU = ConstSession.StringConnectionTARSU
                        objHashTable("CONNECTIONSTRINGTARSU") = strConnectionStringTARSU
                        If sTipoCalcolo <> ObjRuolo.TipoCalcolo.TARES Then
                            bConsentiSanzNeg = False
                            'l'importo netto è impaccertato-impdichiarato
                            For i = 0 To oAccertato.Length - 1
                                For y = 0 To oDichiarato.Length - 1
                                    If oAccertato(i).IdLegame = oDichiarato(y).IdLegame Then
                                        oAccertato(i).impNetto -= oDichiarato(y).impNetto
                                        Exit For
                                    End If
                                Next
                            Next
                        End If
                        If oAtto.IMPORTO_DIFFERENZA_IMPOSTA <> 0 Then
                            'Calcolo Sanzioni e Interessi su singolo Immobile (la procedura di calcolo delle sanzioni le calcola solo	se l'importo è positivo)
                            For xDettaglio = 0 To oDettaglioAtto.Length - 1
                                Dim objHashTableDati As New Hashtable
                                If InStr(oDettaglioAtto(xDettaglio).Sanzioni, "#") Then
                                    objHashTableDati.Add("IDSANZIONI", oDettaglioAtto(xDettaglio).Sanzioni)
                                Else
                                    objHashTableDati.Add("IDSANZIONI", oDettaglioAtto(xDettaglio).Sanzioni & "#" & objHashTable("TIPOPROVVEDIMENTO"))
                                End If

                                'L'Id Immobile è il Progressivo
                                objHashTableDati.Add("IDIMMOBILE", oDettaglioAtto(xDettaglio).Progressivo)
                                objHashTableDati.Add("IDLEGAME", oDettaglioAtto(xDettaglio).IdLegame)
                                'Calcolo le sanzioni per i singoli Immobili
                                oCalcoloSanzioniInteressi = CreateDatasetPerSanzInt(objHashTable("ANNOACCERTAMENTO"), oDettaglioAtto(xDettaglio).ImpAccertato - oDettaglioAtto(xDettaglio).ImpDichiarato, 0, 0, oAtto.IMPORTO_PAGATO)
                                objDSCalcoloSanzioniInteressiAppoggio = SetObjDSAppoggioSanzioni(objHashTable("ANNOACCERTAMENTO"), oDettaglioAtto(xDettaglio).ImpAccertato - oDettaglioAtto(xDettaglio).ImpDichiarato, oAtto.IMPORTO_PAGATO)
                                Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
                                objDSSanzioni = objCOMDichiarazioniAccertamenti.getSanzioni(ConstSession.StringConnection, sIdEnte, objHashTable, objHashTableDati, oCalcoloSanzioniInteressi, objDSCalcoloSanzioniInteressiAppoggio, bConsentiSanzNeg, sDataMorte)
                                'Creo una copia della struttura
                                If dtSanzioni.Rows.Count = 0 And objDSSanzioni.Tables.Count > 0 Then
                                    dtSanzioni = objDSSanzioni.Tables("SANZIONI").Copy
                                    dtSanzioni.Clear()
                                End If

                                Dim intCount As Integer
                                Dim copyRow As DataRow

                                For intCount = 0 To objDSSanzioni.Tables("SANZIONI").Rows.Count - 1
                                    'leggo importo minimo sanzione applicata
                                    impMinimoSanzione = objDSSanzioni.Tables("SANZIONI").Rows(intCount)("IMPORTO_GIORNI")
                                    nQuotaRiduzioneSanzioni = objDSSanzioni.Tables("SANZIONI").Rows(0)("QUOTARIDUZIONESANZIONI")
                                    objDSSanzioni.Tables("SANZIONI").Rows(intCount)("IMPORTO_GIORNI") = 0
                                    copyRow = objDSSanzioni.Tables("SANZIONI").Rows(intCount)
                                    dtSanzioni.ImportRow(copyRow)
                                Next

                                If Not dtSanzioni Is Nothing Then
                                    For intCount = 0 To dtSanzioni.Rows.Count - 1
                                        Dim rows() As DataRow
                                        rows = dtSanzioni.Select("ID_LEGAME='" & oDettaglioAtto(xDettaglio).IdLegame & "'")
                                        For y = 0 To UBound(rows)
                                            rows(y).Item("Motivazioni") = oAccertato(xDettaglio).sDescrSanzioni
                                        Next
                                        dtSanzioni.AcceptChanges()
                                    Next
                                End If

                                'Aggiorno il DataSet con le sanzioni
                                'Dim dt As DataTable
                                'dt = objDSSanzioni.Tables(1)
                                'oCalcoloSanzioniInteressi.Dispose()
                                'oCalcoloSanzioniInteressi = New DataSet
                                'oCalcoloSanzioniInteressi.Tables.Add(dt.Copy)

                                'Aggiorno il DS con l'importo delle sanzioni calcolate
                                oDettaglioAtto(xDettaglio).ImpSanzioni = oCalcoloSanzioniInteressi.Sanzioni
                                oDettaglioAtto(xDettaglio).ImpSanzioniRidotto = oCalcoloSanzioniInteressi.SanzioniRidotto

                                'totale sanzione dell'atto di accertamento
                                TotImportoSanzioniACCERTAMENTO = TotImportoSanzioniACCERTAMENTO + oDettaglioAtto(xDettaglio).ImpSanzioni
                                TotImportoSanzioniRidottoACCERTAMENTO = TotImportoSanzioniRidottoACCERTAMENTO + oDettaglioAtto(xDettaglio).ImpSanzioniRidotto

                                'Somma algebrica per determinare Tipo Avviso
                                DiffTotaleSanzioni = oDettaglioAtto(xDettaglio).ImpSanzioni
                                DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleSanzioni
                                'CALCOLO INTERESSI
                                blnCalcolaInteressi = oDettaglioAtto(xDettaglio).Calcola_Interessi
                                If blnCalcolaInteressi = True Then
                                    Dim myItem As New ObjBaseIntSanz
                                    myItem.Anno = sAnnoRiferimento
                                    myItem.COD_TIPO_PROVVEDIMENTO = TipoProvvedimento
                                    myItem.DifferenzaImposta = oDettaglioAtto(xDettaglio).ImpAccertato - oDettaglioAtto(xDettaglio).ImpDichiarato
                                    myItem.IdContribuente = oDettaglioAtto(xDettaglio).IdContribuente
                                    myItem.IdEnte = sIdEnte
                                    ListBaseCalc.Add(myItem)

                                    objCOMDichiarazioniAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
                                    ListInteressi = objCOMDichiarazioniAccertamenti.getInteressi(sIdEnte, Utility.Costanti.TRIBUTO_TARSU, OggettoAtto.Capitolo.Interessi, TipoProvvedimento, OggettoAtto.Procedimento.Accertamento, OggettoAtto.Fase.DichiaratoAccertato, Now, "", "", oDettaglioAtto(xDettaglio).IdLegame, CType(ListBaseCalc.ToArray(GetType(ObjBaseIntSanz)), ObjBaseIntSanz()), ConstSession.StringConnection)
                                    'Log.Debug("prelevato interessi")
                                    If Not IsNothing(ListInteressi) Then
                                        'Aggiorno il DS con l'importo delle interessi calcolati
                                        For Each myInt As ObjInteressiSanzioni In ListInteressi
                                            oDettaglioAtto(xDettaglio).ImpInteressi = myInt.IMPORTO_GIORNI
                                            DiffTotaleInteressi = myInt.IMPORTO_GIORNI
                                        Next
                                        'totale interessi dell'atto di accertamento
                                        TotImportoInteressiACCERTAMENTO += DiffTotaleInteressi
                                        'Somma algebrica per determinare se Rimborso o Avviso di accertamento
                                        DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleInteressi
                                    End If
                                Else
                                    'Aggiorno il DataSet con gli interessi
                                    'oCalcoloSanzioniInteressi = New DataSet
                                    'oCalcoloSanzioniInteressi.Tables.Add(dt.Copy)
                                    oDettaglioAtto(xDettaglio).ImpInteressi = 0
                                    'totale interessi dell'atto di accertamento
                                    TotImportoInteressiACCERTAMENTO = TotImportoInteressiACCERTAMENTO + oDettaglioAtto(xDettaglio).ImpInteressi

                                    'Somma algebrica per determinare se Rimborso o Avviso di accertamento
                                    DiffTotaleInteressi = 0
                                    DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleInteressi
                                End If

                                Log.Debug("xDettaglio=" & xDettaglio & vbTab & " Legame:" & oDettaglioAtto(xDettaglio).IdLegame & vbTab & " - Prog:" & oDettaglioAtto(xDettaglio).Progressivo & vbTab & " - SANZIONI. Piene=" & oDettaglioAtto(xDettaglio).ImpSanzioni & " - Ridotte=" & oDettaglioAtto(xDettaglio).ImpSanzioniRidotto & vbTab & vbTab & vbTab & " - INTERESSI = " & oDettaglioAtto(xDettaglio).ImpInteressi)
                            Next

                            For i = 0 To oAccertato.Length - 1
                                For x = 0 To oDettaglioAtto.Length - 1
                                    If oAccertato(i).Progressivo = oDettaglioAtto(x).Progressivo Then
                                        If oAccertato(i).IdLegame = oDettaglioAtto(x).IdLegame Then
                                            oAccertato(i).ImpSanzioni = oDettaglioAtto(x).ImpSanzioni
                                            oAccertato(i).ImpSanzioniRid = oDettaglioAtto(x).ImpSanzioniRidotto
                                            oAccertato(i).ImpInteressi = oDettaglioAtto(x).ImpInteressi
                                        End If
                                    End If
                                Next
                            Next
                            'controllo che il totale sanzioni applicate sia superiore al minimo
                            If TotImportoSanzioniACCERTAMENTO < impMinimoSanzione And impMinimoSanzione > 0 Then
                                Dim impDifSanzXMinimo As Double = 0
                                impDifSanzXMinimo = (impMinimoSanzione - TotImportoSanzioniACCERTAMENTO)
                                TotImportoSanzioniACCERTAMENTO = 0 : TotImportoSanzioniRidottoACCERTAMENTO = 0
                                'la prima UI avrà anche la differenza per arrivare al minimo sanzione
                                For Each UIAcc As ObjArticoloAccertamento In oAccertato
                                    If UIAcc.IdLegame = "1" Then
                                        UIAcc.ImpSanzioni += impDifSanzXMinimo
                                        UIAcc.ImpSanzioniRid = FormatNumber(UIAcc.ImpSanzioni / nQuotaRiduzioneSanzioni, 2)
                                    End If
                                    'ricalcolo il totale sanzioni
                                    TotImportoSanzioniACCERTAMENTO += UIAcc.ImpSanzioni
                                    TotImportoSanzioniRidottoACCERTAMENTO += UIAcc.ImpSanzioniRid
                                Next
                            End If
                            oAtto.IMPORTO_SANZIONI = TotImportoSanzioniACCERTAMENTO
                            oAtto.IMPORTO_SANZIONI_RIDOTTO = TotImportoSanzioniRidottoACCERTAMENTO
                            oAtto.IMPORTO_INTERESSI = TotImportoInteressiACCERTAMENTO
                            Log.Debug(sIdEnte & " - TotImportoSanzioniACCERTAMENTO:" & TotImportoSanzioniACCERTAMENTO)
                            Log.Debug(sIdEnte & " - TotImportoSanzioniRidottoACCERTAMENTO:" & TotImportoSanzioniRidottoACCERTAMENTO)
                            Log.Debug(sIdEnte & " - TotImportoInteressiACCERTAMENTO:" & TotImportoInteressiACCERTAMENTO)
                        Else
                            'Se sono qui l'ici dichiarato è uguale a quello accertato Sanzioni e Interessi a 0
                            strConnectionStringAnagrafica = ConstSession.StringConnectionAnagrafica
                            If objHashTable.ContainsKey("CONNECTIONSTRINGANAGRAFICA") = True Then
                                objHashTable("CONNECTIONSTRINGANAGRAFICA") = strConnectionStringAnagrafica
                            Else
                                objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", strConnectionStringAnagrafica)
                            End If
                            If objHashTable.ContainsKey("CODENTE") = True Then
                                objHashTable("CODENTE") = sIdEnte
                            Else
                                objHashTable.Add("CODENTE", sIdEnte)
                            End If

                            If objHashTable.ContainsKey("CODCONTRIBUENTE") = False Then
                                objHashTable.Add("CODCONTRIBUENTE", oAccertato(0).IdContribuente)
                            End If
                            oAtto.IMPORTO_INTERESSI = 0
                            oAtto.IMPORTO_SANZIONI = 0
                            oAtto.IMPORTO_SANZIONI_RIDOTTO = 0
                        End If
                        'estraggo le addizionali che devono essere applicate se il check è selezionato
                        If bCalcolaAddizionali = True Then
                            Dim impImponibileAddiz As Double = 0
                            'per TARSU uso l'imposta dell'atto per TARES uso tutto tranne maggiorazione
                            If sTipoCalcolo = ObjRuolo.TipoCalcolo.TARSU Then
                                impImponibileAddiz = oAtto.IMPORTO_DIFFERENZA_IMPOSTA
                            Else
                                For Each myArt As ObjArticoloAccertamento In oAccertato
                                    If myArt.TipoPartita <> ObjArticolo.PARTEMAGGIORAZIONE Then
                                        impImponibileAddiz += myArt.impNetto
                                    End If
                                Next
                            End If
                            ObjAddizionaleServizio = GetAddizionaliAtto(sIdEnte, impImponibileAddiz, oAtto, sScript)
                        End If
                        Dim ImportoTotaleAvviso As Double
                        ImportoTotaleAvviso = oAtto.IMPORTO_DIFFERENZA_IMPOSTA + TotImportoSanzioniACCERTAMENTO + TotImportoInteressiACCERTAMENTO
                        Log.Debug(sIdEnte & " - ImportoTotaleAvviso:" & ImportoTotaleAvviso)
                        If ImportoTotaleAvviso < 0 Then
                            'sarebbe un rimborso Non emetto Avviso
                            Log.Debug(sIdEnte & " - Importo Avviso inferiore a zero. L'avviso non verrà emesso")
                            TipoAvviso = OggettoAtto.Provvedimento.Rimborso '5
                            sScript += "GestAlert('a', 'warning', '', '', 'Importo Avviso inferiore a zero. L\'avviso non verrà emesso');"
                            sScript += "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
                        Else
                            'reperisco soglia
                            Dim soglia As Double
                            Dim objProvvedimentiDB As New DBPROVVEDIMENTI.ProvvedimentiDB
                            soglia = 0
                            soglia = objProvvedimentiDB.GetSogliaMinima(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_TARSU, objHashTable("CODENTE"), TipoAvviso)
                            Log.Debug(sIdEnte & " - Soglia:" & soglia & " €")
                            'Calcolo le spese
                            Dim spese As Double
                            spese = objProvvedimentiDB.GetSpese(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_TARSU, objHashTable("CODENTE"), TipoAvviso)
                            Log.Debug(sIdEnte & " - Spese:" & spese & " €")
                            If Not oCalcoloSanzioniInteressi Is Nothing Then
                                oCalcoloSanzioniInteressi.Sanzioni = TotImportoSanzioniACCERTAMENTO
                                oCalcoloSanzioniInteressi.SanzioniRidotto = TotImportoSanzioniRidottoACCERTAMENTO

                                oCalcoloSanzioniInteressi.Interessi = TotImportoInteressiACCERTAMENTO
                                oCalcoloSanzioniInteressi.DifferenzaImposta = oAtto.IMPORTO_DIFFERENZA_IMPOSTA
                            End If

                            ' Aggiorno il DB dopo procedura di accertamento
                            If Not objDSSanzioni Is Nothing Then
                                objDSSanzioni.Dispose()
                            End If
                            objDSSanzioni = New DataSet
                            objDSSanzioni.Tables.Add(dtSanzioni.Copy)

                            oAtto.CAP_CO = ""
                            oAtto.CAP_RES = ""
                            oAtto.CITTA_CO = ""
                            oAtto.CITTA_RES = ""
                            oAtto.CIVICO_CO = ""
                            oAtto.CIVICO_RES = ""
                            oAtto.CO = ""
                            oAtto.CODICE_FISCALE = ""
                            oAtto.COGNOME = ""
                            oAtto.ESPONENTE_CIVICO_CO = ""
                            oAtto.ESPONENTE_CIVICO_RES = ""
                            oAtto.FRAZIONE_CO = ""
                            oAtto.FRAZIONE_RES = ""
                            oAtto.NOME = ""
                            oAtto.PARTITA_IVA = ""
                            oAtto.POSIZIONE_CIVICO_CO = ""
                            oAtto.POSIZIONE_CIVICO_RES = ""
                            oAtto.PROVINCIA_CO = ""
                            oAtto.PROVINCIA_RES = ""
                            oAtto.VIA_CO = ""
                            oAtto.VIA_RES = ""
                            oAtto.COD_CONTRIBUENTE = CInt(objHashTable("CODCONTRIBUENTE"))
                            oAtto.IMPORTO_SPESE = spese
                            oAtto.DATA_ELABORAZIONE = DateTime.Now.ToString("yyyyMMdd")
                            'aggiorno l'importo totale comprensivo di arrotondamento
                            oAtto.IMPORTO_TOTALE = (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO)
                            oAtto.IMPORTO_ARROTONDAMENTO = CDbl(ImportoArrotondato(oAtto.IMPORTO_TOTALE)) - oAtto.IMPORTO_TOTALE
                            oAtto.IMPORTO_TOTALE = oAtto.IMPORTO_TOTALE + oAtto.IMPORTO_ARROTONDAMENTO + oAtto.IMPORTO_SPESE
                            oAtto.IMPORTO_TOTALE_RIDOTTO = oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI_RIDOTTO + oAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO
                            oAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO = CDbl(ImportoArrotondato(oAtto.IMPORTO_TOTALE_RIDOTTO)) - oAtto.IMPORTO_TOTALE_RIDOTTO
                            oAtto.IMPORTO_TOTALE_RIDOTTO = oAtto.IMPORTO_TOTALE_RIDOTTO + oAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO + oAtto.IMPORTO_SPESE
                            For i = 0 To oAccertato.Length - 1
                                If Year(oAccertato(i).tDataInizio) < oAtto.ANNO Then
                                    oAccertato(i).tDataInizio = "01/01/" & oAtto.ANNO
                                End If
                                oAtto.IMPORTO_ACCERTATO_ACC += oAccertato(i).impNetto
                            Next
                            For i = 0 To oDichiarato.Length - 1
                                If Year(oDichiarato(i).tDataInizio) < oAtto.ANNO Then
                                    oDichiarato(i).tDataInizio = "01/01/" & oAtto.ANNO
                                End If
                                oAtto.IMPORTO_DICHIARATO_F2 += oDichiarato(i).impNetto
                            Next
                            If objHashTable.ContainsKey("VALORE_RITORNO_ACCERTAMENTO") = True Then
                                objHashTable.Remove("VALORE_RITORNO_ACCERTAMENTO")
                            End If
                            If objHashTable.Contains("TipoTassazione") = False Then
                                objHashTable.Add("TipoTassazione", sTipoCalcolo)
                            End If

                            'aggiungo anche VALORE_RITORNO_ACCERTAMENTO=0 per creare un accertamento per un anno in cui è presente un acc definitivo.
                            objHashTable.Add("VALORE_RITORNO_ACCERTAMENTO", sValRitornoAccertamento)

                            If objHashTable.ContainsKey("DATA_ANNULLAMENTO") Then objHashTable.Remove("DATA_ANNULLAMENTO")
                            If objHashTable.ContainsKey("DATA_RETTIFICA") Then objHashTable.Remove("DATA_RETTIFICA")

                            If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                                DATA_RETTIFICA_ANNULLAMENTO = DateTime.Now.ToString("yyyyMMdd")
                                If ImportoTotaleAvviso < soglia Then
                                    objHashTable.Add("DATA_ANNULLAMENTO", DATA_RETTIFICA_ANNULLAMENTO)
                                    objHashTable.Add("DATA_RETTIFICA", "")
                                Else
                                    objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA_ANNULLAMENTO)
                                    objHashTable.Add("DATA_ANNULLAMENTO", "")
                                End If
                            End If

                            'DETERMINO TIPO DI AVVISO 
                            Log.Debug(sIdEnte & " - Richiamo la funzione per determinare il tipo di avviso")
                            If FncGestAcc.DeterminaTipoAvviso(ImportoTotaleAvviso, 0, soglia, blnTIPO_OPERAZIONE_RETTIFICA, 0, TipoAvviso, sDescTipoAvviso) = False Then
                                Throw New Exception
                            End If
                            If TipoAvviso = OggettoAtto.Provvedimento.AutotutelaAnnullamento Then
                                If Not IsNothing(oAtto) Then
                                    'devo azzerare tutti gli importi
                                    oAtto.IMPORTO_ALTRO = 0
                                    oAtto.IMPORTO_ARROTONDAMENTO = 0
                                    oAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO = 0
                                    oAtto.IMPORTO_INTERESSI = 0
                                    oAtto.IMPORTO_SANZIONI = 0
                                    oAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI = 0
                                    oAtto.IMPORTO_SANZIONI_RIDOTTO = 0
                                    oAtto.IMPORTO_SPESE = 0
                                    oAtto.IMPORTO_TOTALE = 0
                                    oAtto.IMPORTO_TOTALE_RIDOTTO = 0
                                End If
                            End If
                            oAtto.TipoProvvedimento = TipoAvviso
                            If objHashTable.Contains("TIPOPROVVEDIMENTO") = False Then
                                objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
                            Else
                                objHashTable("TIPOPROVVEDIMENTO") = TipoAvviso
                            End If
                            Log.Debug(sIdEnte & " - ImportoTotaleAvviso:" & ImportoTotaleAvviso & " €")
                            Log.Debug(sIdEnte & " - TipoAvviso:" & TipoAvviso & " - " & sDescTipoAvviso)
                            Log.Debug("RICHIAMO objCOMUpdateDBAccertamenti.updateDBAccertamentiTARSU(objHashTable, objDSCalcoloSanzioniInteressi, objDSSanzioni, objDSInteressi, oAtto, oDettaglioAtto, oDichiarato, oAccertato, spese, ObjAddizionaleServizio) passando")
                            OggettoRiepilogoAccertamento = objCOMUpdateDBAccertamenti.updateDBAccertamentiTARSU(ConstSession.DBType, sIdEnte, oAtto.COD_CONTRIBUENTE, objHashTable, oCalcoloSanzioniInteressi, objDSSanzioni, ListInteressi, oAtto, oDettaglioAtto, oDichiarato, oAccertato, spese, ObjAddizionaleServizio, ConstSession.UserName)

                            'recupero id Provvedimento
                            If Not OggettoRiepilogoAccertamento Is Nothing Then
                                lngNewID_PROVVEDIMENTO = OggettoRiepilogoAccertamento(0).Id
                                'e lo assegno anche ai singoli immobili
                                For i = 0 To oAccertato.Length - 1
                                    oAccertato(i).Id = lngNewID_PROVVEDIMENTO
                                Next
                                objDSSanzioni.Dispose()
                                objDSSanzioni.Dispose()

                                If ImportoTotaleAvviso = 0 Then
                                    'Non emetto Avviso
                                    sScript += "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
                                    sScript += "parent.document.getElementById('attesaCarica').style.display='none';"
                                    If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                                        'Atto di autotutela di annullamento
                                        sScript += "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
                                        sScript += "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
                                        Log.Debug(sIdEnte & " - La posizione è corretta. Ho elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO")
                                    Else
                                        'Nessun avviso emesso
                                        'Effettuo il rientro dell'accertato
                                        '*** RIENTRO DA SISTEMARE ***
                                        'Dim Rientro As New BE_RientroDaAccertamento.ICI
                                        'Rientro.pathApplication = ConfigurationManager.AppSettings("PATH_LOG_IMMOBILI")
                                        'Rientro.PARAMETROENV = ConfigurationManager.AppSettings("PARAMETROENV")
                                        'Rientro.username = ConstSession.UserName
                                        'Rientro.IdentificativoApplicazione = ConfigurationManager.AppSettings("OPENGOVP")
                                        'Rientro.CODICE_ENTE = sIdEnte
                                        'If Rientro.Elabora(lngNewID_PROVVEDIMENTO) Then
                                        '    sscript+= "alert('La posizione è corretta.\nL\'accertato è stato inserito come nuova dichiarazione');"
                                        '    Log.Debug(sIdEnte & " - Effettuato il rientro dell'accertato")
                                        'Else
                                        '    sscript+= "alert('La posizione è corretta.\nSi è verificato un errore nel rientro dell'accertato');"
                                        '    Log.Debug(sIdEnte & " - Errore nel rientro dell'accertato")
                                        'End If
                                        sScript += "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
                                    End If
                                ElseIf ImportoTotaleAvviso < soglia Then
                                    sScript += "parent.document.getElementById('attesaCarica').style.display='none';"
                                    If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                                        'Atto di autotutela di annullamento. Importo inferiore alla soglia
                                        sScript += "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
                                        sScript += "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
                                        Log.Debug(sIdEnte & " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
                                    Else
                                        'Effettuo il rientro dell'accertato
                                        'Nessun avviso emesso. Importo inferiore alla soglia
                                        '*** RIENTRO DA SISTEMARE ***
                                        'Dim Rientro As New BE_RientroDaAccertamento.ICI
                                        'Rientro.pathApplication = ConfigurationManager.AppSettings("PATH_LOG_IMMOBILI")
                                        'Rientro.PARAMETROENV = ConfigurationManager.AppSettings("PARAMETROENV")
                                        'Rientro.username = ConstSession.UserName
                                        'Rientro.IdentificativoApplicazione = ConfigurationManager.AppSettings("OPENGOVP")
                                        'Rientro.CODICE_ENTE = sIdEnte
                                        'If Rientro.Elabora(lngNewID_PROVVEDIMENTO) Then
                                        '    sscript+= "alert('Importo Avviso inferiore alla soglia.\nL\'accertato è stato inserito come nuova dichiarazione');"
                                        '    Log.Debug(sIdEnte & " - Effettuato il rientro dell'accertato")
                                        'Else
                                        '    sscript+= "alert('Importo Avviso inferiore alla soglia.\nSi è verificato un errore nel rientro dell'accertato');"
                                        '    Log.Debug(sIdEnte & " - Errore nel rientro dell'accertato")
                                        'End If
                                        sScript += "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
                                    End If
                                    Return oAtto
                                End If

                                HttpContext.Current.Session.Add("oSituazioneAtto", oAtto)
                                HttpContext.Current.Session.Add("oSituazioneDichiarato", oDichiarato)
                                HttpContext.Current.Session.Add("oSituazioneAccertato", oAccertato)
                                '******
                                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                                    sScript += "FineElaborazioneAccertamento();" & vbCrLf
                                    sScript += "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
                                Else
                                    sScript += "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
                                End If
                                Log.Debug(sIdEnte & " - registro script per visualizzare il riepilogo accertamento")
                            Else
                                sScript += "GestAlert('a', 'danger', '', '', 'Impossibile proseguire! Si è verificato un\'errore in salvataggio!');"
                                sScript += "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
                                oAtto = Nothing
                            End If
                        End If
                    Else
                        oAtto = Nothing
                    End If
                ElseIf ImpTotAccert < ImpTotDich Then
                    'accertato minore dichiarato Non emetto Avviso Non esiste rimborso
                    sScript += "GestAlert('a', 'warning', '', '', 'Importo Accertato inferiore all\' Importo Dichiarato. L\'avviso non verrà emesso');"
                    sScript += "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
                End If
            Else
                'manca accertato
                oAtto = Nothing
            End If
            Log.Debug(sIdEnte & " - Fine")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.TARSUConfrontoAccertatoDichiarato.errore: ", ex)
            oAtto = Nothing
        End Try
        Return oAtto
    End Function
    'Public Function TARSUConfrontoAccertatoDichiarato(ByVal sTipoCalcolo As String, ByVal sIdEnte As String, ByVal sAnnoRiferimento As String, ByVal oDichiarato() As ObjArticoloAccertamento, ByVal oAccertato() As ObjArticoloAccertamento, ByVal blnTIPO_OPERAZIONE_RETTIFICA As Boolean, ByVal objHashTable As Hashtable, ByVal bCalcolaAddizionali As Boolean, ByVal objDSMotivazioniSanzioni As DataSet, ByVal sValRitornoAccertamento As Integer, sDataMorte As String, ByRef sDescTipoAvviso As String, ByRef sScript As String) As OggettoAttoTARSU
    '    Dim FncGestAcc As New ClsGestioneAccertamenti
    '    Dim x, y, nList, i As Integer
    '    Dim nLegamePrec As Integer = -1
    '    Dim oDettaglioAtto() As OggettoDettaglioAtto
    '    Dim oListDettaglioAtto As OggettoDettaglioAtto

    '    Dim ImpTotAccert As Double = 0
    '    Dim ImpTotDich As Double = 0
    '    Dim oAtto As New OggettoAttoTARSU
    '    Dim oPagatoTARSU() As OggettoPagamenti
    '    Dim ClsPagato As New OPENgovTIA.ClsGestPag

    '    Dim DiffTotaleSanzioni As Double     'Somma algebrica fra sanzioni e interessi x det. tipo avviso
    '    Dim DiffTotaleInteressi As Double     'Somma algebrica fra sanzioni e interessi x det. tipo avviso
    '    Dim DiffTotaleSanzInt As Double     'Somma algebrica fra sanzioni e interessi x det. tipo avviso

    '    Dim OggettoRiepilogoAccertamento As ObjArticoloAccertamento()
    '    Dim blnCalcolaInteressi As Boolean

    '    Dim lngNewID_PROVVEDIMENTO As Long
    '    Dim DATA_RETTIFICA_ANNULLAMENTO As String

    '    Dim ObjAddizionaleServizio() As OggettoAddizionaleAccertamento = Nothing

    '    Dim blnResult As Boolean = False
    '    Dim strConnectionStringAnagrafica As String
    '    Dim strConnectionStringTARSU As String
    '    Dim objCOMUpdateDBAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '    Dim bConsentiSanzNeg As Boolean = True
    '    Dim impMinimoSanzione As Double = 0
    '    Dim nQuotaRiduzioneSanzioni As Integer = 4
    '    Dim TipoProvvedimento As String
    '    Dim ListBaseCalc As New ArrayList

    '    Try
    '        If Not oAccertato Is Nothing Then
    '            'Determino il Tipo di Avviso (Accertamento o Ufficio); se trovo 1 immobile di accertato che non è in dichiarato ho avviso di Tipo = 4 altrimenti avviso di tipo 3
    '            Dim DichiaratoNothing As New ObjArticoloAccertamento
    '            If oDichiarato Is Nothing Then
    '                ReDim Preserve oDichiarato(0)
    '                oDichiarato(0) = DichiaratoNothing
    '            End If
    '            Dim TipoAvviso As Integer

    '            TipoAvviso = OggettoAtto.Provvedimento.AccertamentoRettifica   '4

    '            'Prendo immobili di accertato
    '            For i = 0 To oAccertato.Length - 1
    '                Dim Trovato As Boolean = False
    '                'Cerco l'immobili in tutti gli immobili di dichiarato Se lo trovo esco dal ciclo e proseguo a cercare con immobili successivo di accertamento
    '                For y = 0 To oDichiarato.Length - 1
    '                    If oAccertato(i).IdLegame = oDichiarato(y).IdLegame Then
    '                        Trovato = True
    '                        Exit For
    '                    End If
    '                Next

    '                'Se trovato = False vuol dire che non ho trovato l'immobile
    '                If Trovato = False Then
    '                    'Avviso D'ufficio
    '                    TipoAvviso = OggettoAtto.Provvedimento.AccertamentoUfficio    '3
    '                    Exit For
    '                End If
    '            Next

    '            nList = -1
    '            'CONFRONTO ACCERTATO E DICHIARATO
    '            'ciclo su tutti i record accertati
    '            For x = 0 To oAccertato.GetUpperBound(0)
    '                'sommo gli importi a parità di livello di legame
    '                oListDettaglioAtto = New OggettoDettaglioAtto
    '                If oAccertato(x).IdLegame <> nLegamePrec Then
    '                    'aggiungo un record
    '                    nList += 1
    '                    oListDettaglioAtto = New OggettoDettaglioAtto
    '                    oListDettaglioAtto.IdLegame = oAccertato(x).IdLegame
    '                    oListDettaglioAtto.Progressivo = oAccertato(x).Progressivo
    '                    oListDettaglioAtto.Sanzioni = oAccertato(x).Sanzioni
    '                    oListDettaglioAtto.Interessi = oAccertato(x).Interessi
    '                    oListDettaglioAtto.Calcola_Interessi = oAccertato(x).Calcola_Interessi
    '                End If
    '                'sommo l'importo
    '                oListDettaglioAtto.ImpAccertato += oAccertato(x).impNetto
    '                If nList < 0 Then nList = 0
    '                ReDim Preserve oDettaglioAtto(nList)
    '                oDettaglioAtto(nList) = oListDettaglioAtto
    '            Next

    '            'se il calcolo è di tipo TARES è come se non ci fosse mai il dichiarato
    '            If sTipoCalcolo <> ObjRuolo.TipoCalcolo.TARES Then
    '                'ciclo su tutti i record dichiarati
    '                For x = 0 To oDichiarato.GetUpperBound(0)
    '                    'cerco il corrispettivo legame nell'oggetto di confronto
    '                    For y = 0 To oDettaglioAtto.GetUpperBound(0)
    '                        If oDettaglioAtto(y).IdLegame = oDichiarato(x).IdLegame Then
    '                            oDettaglioAtto(y).ImpDichiarato += oDichiarato(x).impNetto
    '                            Exit For
    '                        End If
    '                    Next
    '                Next
    '            End If
    '            'ciclo sull'oggetto totale per verificare il tipo di atto
    '            nList = -1
    '            For x = 0 To oDettaglioAtto.GetUpperBound(0)
    '                ImpTotAccert += FormatNumber(oDettaglioAtto(x).ImpAccertato, 2)
    '                ImpTotDich += FormatNumber(oDettaglioAtto(x).ImpDichiarato, 2)
    '            Next
    '            nList += 1
    '            If ImpTotAccert >= ImpTotDich Then
    '                'accertato maggiore dichiarato
    '                oAtto = TARSU_PopolaAtto(sIdEnte, oDettaglioAtto)
    '                oAtto.ANNO = oAccertato(0).sAnno
    '                If Not oAtto Is Nothing Then
    '                    'SE HO UN ATTO CONFRONTO ACCERTATO E PAGATO popolo differenza d'imposta
    '                    Dim SearchParamPag As New OPENgovTIA.ObjSearchPagamenti
    '                    SearchParamPag.IdContribuente = oAtto.COD_CONTRIBUENTE
    '                    SearchParamPag.sAnnoRif = oAtto.ANNO
    '                    SearchParamPag.bRicPag = False
    '                    oPagatoTARSU = ClsPagato.GetListPagamenti(SearchParamPag, ConstSession.StringConnectionTARSU)
    '                    If Not oPagatoTARSU Is Nothing Then
    '                        Dim IPagato As Integer
    '                        For IPagato = 0 To oPagatoTARSU.Length - 1
    '                            oAtto.IMPORTO_PAGATO += oPagatoTARSU(IPagato).dImportoPagamento
    '                        Next
    '                    Else
    '                        oAtto.IMPORTO_PAGATO = 0
    '                    End If

    '                    Dim TipoAvvisoRimborso As Integer = -1
    '                    'Rimborso. Calcolo gli interessi Attivi sul singolo immobile.
    '                    'Al giro dopo la var viene azzerrata a -1.
    '                    If oAtto.IMPORTO_DIFFERENZA_IMPOSTA < 0 Then
    '                        TipoAvvisoRimborso = OggettoAtto.Provvedimento.Rimborso '"5"
    '                    End If

    '                    Dim objDSCalcoloSanzioniInteressi As New DataSet
    '                    Dim dtInteressi As New DataTable
    '                    Dim dtSanzioni As New DataTable
    '                    Dim objDSCalcoloSanzioniInteressiAppoggio As DataSet
    '                    Dim TotImportoSanzioniACCERTAMENTO As Double = 0        'Totale Sanzioni atto di accertamento
    '                    Dim TotImportoSanzioniRidottoACCERTAMENTO As Double = 0 'Totale Sanzioni atto di accertamento
    '                    Dim TotImportoInteressiACCERTAMENTO As Double = 0       'Totale Interessi atto di accertamento
    '                    Dim TotDiffImpostaACCERTAMENTO As Double = 0            'Importo Totale Differenza di imposta atto di accertamento
    '                    Dim TotDiffImpostaAccontoACCERTAMENTO As Double = 0     'Importo Totale Differenza di imposta atto ACCONTO di accertamento
    '                    Dim TotDiffImpostaSaldoACCERTAMENTO As Double = 0       'Importo Totale Differenza di imposta SALDO atto di accertamento

    '                    Dim xDettaglio As Integer
    '                    Dim objDSSanzioni As New DataSet
    '                    Dim ListInteressi() As ObjInteressiSanzioni

    '                    If objHashTable.ContainsKey("CODCONTRIBUENTE") = False Then
    '                        objHashTable.Add("CODCONTRIBUENTE", oAtto.COD_CONTRIBUENTE)
    '                    End If
    '                    If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = True Then
    '                        objHashTable.Remove("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI")
    '                    End If
    '                    objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

    '                    strConnectionStringAnagrafica = ConstSession.StringConnectionAnagrafica 'objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEANAGRAFICA")).GetConnection.ConnectionString
    '                    objHashTable("CONNECTIONSTRINGANAGRAFICA") = strConnectionStringAnagrafica
    '                    objHashTable("CODTRIBUTO") = Utility.Costanti.TRIBUTO_TARSU
    '                    objHashTable("CODENTE") = sIdEnte
    '                    If objHashTable.ContainsKey("TIPOPROVVEDIMENTO") = True Then
    '                        objHashTable.Remove("TIPOPROVVEDIMENTO")
    '                    End If
    '                    'TipoAvvisoRimborso = -1 == False ----> Non è un rimborso
    '                    'Devo effettuare il calcolo guardando se è un rimborso sull'immobile oppure no; se è rimborso ho solo interessi attivi.
    '                    If TipoAvvisoRimborso = -1 Then
    '                        objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
    '                        TipoProvvedimento = TipoAvviso
    '                    Else
    '                        objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvvisoRimborso)
    '                        TipoProvvedimento = TipoAvvisoRimborso
    '                    End If
    '                    If objHashTable.ContainsKey("COD_TIPO_PROCEDIMENTO") = True Then
    '                        objHashTable("COD_TIPO_PROCEDIMENTO") = OggettoAtto.Procedimento.Accertamento
    '                    Else
    '                        objHashTable.Add("COD_TIPO_PROCEDIMENTO", OggettoAtto.Procedimento.Accertamento)
    '                    End If
    '                    If objHashTable.Contains("ANNOACCERTAMENTO") = False Then
    '                        objHashTable.Add("ANNOACCERTAMENTO", sAnnoRiferimento)
    '                    End If
    '                    'PER CONNESSIONE TARSU
    '                    objHashTable("IDSOTTOAPPLICAZIONETARSU") = ConfigurationManager.AppSettings("OPENGOVTA")
    '                    strConnectionStringTARSU = ConstSession.StringConnectionTARSU
    '                    objHashTable("CONNECTIONSTRINGTARSU") = strConnectionStringTARSU
    '                    If sTipoCalcolo <> ObjRuolo.TipoCalcolo.TARES Then
    '                        bConsentiSanzNeg = False
    '                        'l'importo netto è impaccertato-impdichiarato
    '                        For i = 0 To oAccertato.Length - 1
    '                            For y = 0 To oDichiarato.Length - 1
    '                                If oAccertato(i).IdLegame = oDichiarato(y).IdLegame Then
    '                                    oAccertato(i).impNetto -= oDichiarato(y).impNetto
    '                                    Exit For
    '                                End If
    '                            Next
    '                        Next
    '                    End If
    '                    If oAtto.IMPORTO_DIFFERENZA_IMPOSTA <> 0 Then
    '                        'Calcolo Sanzioni e Interessi su singolo Immobile (la procedura di calcolo delle sanzioni le calcola solo	se l'importo è positivo)
    '                        For xDettaglio = 0 To oDettaglioAtto.Length - 1
    '                            Dim objHashTableDati As New Hashtable
    '                            If InStr(oDettaglioAtto(xDettaglio).Sanzioni, "#") Then
    '                                objHashTableDati.Add("IDSANZIONI", oDettaglioAtto(xDettaglio).Sanzioni)
    '                            Else
    '                                objHashTableDati.Add("IDSANZIONI", oDettaglioAtto(xDettaglio).Sanzioni & "#" & objHashTable("TIPOPROVVEDIMENTO"))
    '                            End If

    '                            'L'Id Immobile è il Progressivo
    '                            objHashTableDati.Add("IDIMMOBILE", oDettaglioAtto(xDettaglio).Progressivo)
    '                            objHashTableDati.Add("IDLEGAME", oDettaglioAtto(xDettaglio).IdLegame)
    '                            'Calcolo le sanzioni per i singoli Immobili
    '                            objDSCalcoloSanzioniInteressi = CreateDatasetPerSanzInt(objHashTable("ANNOACCERTAMENTO"), objHashTable("CODCONTRIBUENTE"), oDettaglioAtto(xDettaglio).ImpAccertato - oDettaglioAtto(xDettaglio).ImpDichiarato, 0, 0, oAtto.IMPORTO_PAGATO)
    '                            objDSCalcoloSanzioniInteressiAppoggio = SetObjDSAppoggioSanzioni(objHashTable("ANNOACCERTAMENTO"), oDettaglioAtto(xDettaglio).ImpAccertato - oDettaglioAtto(xDettaglio).ImpDichiarato, oAtto.IMPORTO_PAGATO)
    '                            Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '                            objDSSanzioni = objCOMDichiarazioniAccertamenti.getSanzioni(objHashTable, objHashTableDati, objDSCalcoloSanzioniInteressi, objDSCalcoloSanzioniInteressiAppoggio, bConsentiSanzNeg, sDataMorte)
    '                            'Creo una copia della struttura
    '                            If dtSanzioni.Rows.Count = 0 And objDSSanzioni.Tables.Count > 0 Then
    '                                dtSanzioni = objDSSanzioni.Tables("SANZIONI").Copy
    '                                dtSanzioni.Clear()
    '                            End If

    '                            Dim intCount As Integer
    '                            Dim copyRow As DataRow

    '                            For intCount = 0 To objDSSanzioni.Tables("SANZIONI").Rows.Count - 1
    '                                'leggo importo minimo sanzione applicata
    '                                impMinimoSanzione = objDSSanzioni.Tables("SANZIONI").Rows(intCount)("IMPORTO_GIORNI")
    '                                nQuotaRiduzioneSanzioni = objDSSanzioni.Tables("SANZIONI").Rows(0)("QUOTARIDUZIONESANZIONI")
    '                                objDSSanzioni.Tables("SANZIONI").Rows(intCount)("IMPORTO_GIORNI") = 0
    '                                copyRow = objDSSanzioni.Tables("SANZIONI").Rows(intCount)
    '                                dtSanzioni.ImportRow(copyRow)
    '                            Next

    '                            If Not dtSanzioni Is Nothing Then
    '                                For intCount = 0 To dtSanzioni.Rows.Count - 1
    '                                    Dim rows() As DataRow
    '                                    rows = dtSanzioni.Select("ID_LEGAME='" & oDettaglioAtto(xDettaglio).IdLegame & "'")
    '                                    For y = 0 To UBound(rows)
    '                                        rows(y).Item("Motivazioni") = oAccertato(xDettaglio).sDescrSanzioni
    '                                    Next
    '                                    dtSanzioni.AcceptChanges()
    '                                Next
    '                            End If

    '                            'Aggiorno il DataSet con le sanzioni
    '                            Dim dt As DataTable
    '                            dt = objDSSanzioni.Tables(1)
    '                            objDSCalcoloSanzioniInteressi.Dispose()
    '                            objDSCalcoloSanzioniInteressi = New DataSet
    '                            objDSCalcoloSanzioniInteressi.Tables.Add(dt.Copy)

    '                            'Aggiorno il DS con l'importo delle sanzioni calcolate
    '                            oDettaglioAtto(xDettaglio).ImpSanzioni = objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI")
    '                            oDettaglioAtto(xDettaglio).ImpSanzioniRidotto = objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO")

    '                            'totale sanzione dell'atto di accertamento
    '                            TotImportoSanzioniACCERTAMENTO = TotImportoSanzioniACCERTAMENTO + oDettaglioAtto(xDettaglio).ImpSanzioni
    '                            TotImportoSanzioniRidottoACCERTAMENTO = TotImportoSanzioniRidottoACCERTAMENTO + oDettaglioAtto(xDettaglio).ImpSanzioniRidotto

    '                            'Somma algebrica per determinare Tipo Avviso
    '                            DiffTotaleSanzioni = oDettaglioAtto(xDettaglio).ImpSanzioni
    '                            DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleSanzioni
    '                            'CALCOLO INTERESSI
    '                            blnCalcolaInteressi = oDettaglioAtto(xDettaglio).Calcola_Interessi
    '                            If blnCalcolaInteressi = True Then
    '                                Dim myItem As New ObjBaseIntSanz
    '                                myItem.Anno = sAnnoRiferimento
    '                                myItem.COD_TIPO_PROVVEDIMENTO = TipoProvvedimento
    '                                myItem.DifferenzaImposta = oDettaglioAtto(xDettaglio).ImpAccertato - oDettaglioAtto(xDettaglio).ImpDichiarato
    '                                myItem.IdContribuente = oDettaglioAtto(xDettaglio).IdContribuente
    '                                myItem.IdEnte = sIdEnte
    '                                ListBaseCalc.Add(myItem)

    '                                objCOMDichiarazioniAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '                                ListInteressi = objCOMDichiarazioniAccertamenti.getInteressi(sIdEnte, Utility.Costanti.TRIBUTO_TARSU, OggettoAtto.Capitolo.Interessi, TipoProvvedimento, OggettoAtto.Procedimento.Accertamento, OggettoAtto.Fase.DichiaratoAccertato, Now, "", "", oDettaglioAtto(xDettaglio).IdLegame, CType(ListBaseCalc.ToArray(GetType(ObjBaseIntSanz)), ObjBaseIntSanz()), ConstSession.StringConnection)
    '                                'Log.Debug("prelevato interessi")
    '                                If Not IsNothing(ListInteressi) Then
    '                                    'Aggiorno il DS con l'importo delle interessi calcolati
    '                                    For Each myInt As ObjInteressiSanzioni In ListInteressi
    '                                        oDettaglioAtto(xDettaglio).ImpInteressi = myInt.IMPORTO_GIORNI
    '                                        DiffTotaleInteressi = myInt.IMPORTO_GIORNI
    '                                    Next
    '                                    'totale interessi dell'atto di accertamento
    '                                    TotImportoInteressiACCERTAMENTO += DiffTotaleInteressi
    '                                    'Somma algebrica per determinare se Rimborso o Avviso di accertamento
    '                                    DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleInteressi
    '                                End If
    '                            Else
    '                                'Aggiorno il DataSet con gli interessi
    '                                objDSCalcoloSanzioniInteressi = New DataSet
    '                                objDSCalcoloSanzioniInteressi.Tables.Add(dt.Copy)
    '                                oDettaglioAtto(xDettaglio).ImpInteressi = 0
    '                                'totale interessi dell'atto di accertamento
    '                                TotImportoInteressiACCERTAMENTO = TotImportoInteressiACCERTAMENTO + oDettaglioAtto(xDettaglio).ImpInteressi

    '                                'Somma algebrica per determinare se Rimborso o Avviso di accertamento
    '                                DiffTotaleInteressi = 0
    '                                DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleInteressi
    '                            End If

    '                            Log.Debug("xDettaglio=" & xDettaglio & vbTab & " Legame:" & oDettaglioAtto(xDettaglio).IdLegame & vbTab & " - Prog:" & oDettaglioAtto(xDettaglio).Progressivo & vbTab & " - SANZIONI. Piene=" & oDettaglioAtto(xDettaglio).ImpSanzioni & " - Ridotte=" & oDettaglioAtto(xDettaglio).ImpSanzioniRidotto & vbTab & vbTab & vbTab & " - INTERESSI = " & oDettaglioAtto(xDettaglio).ImpInteressi)
    '                        Next

    '                        For i = 0 To oAccertato.Length - 1
    '                            For x = 0 To oDettaglioAtto.Length - 1
    '                                If oAccertato(i).Progressivo = oDettaglioAtto(x).Progressivo Then
    '                                    If oAccertato(i).IdLegame = oDettaglioAtto(x).IdLegame Then
    '                                        oAccertato(i).ImpSanzioni = oDettaglioAtto(x).ImpSanzioni
    '                                        oAccertato(i).ImpSanzioniRid = oDettaglioAtto(x).ImpSanzioniRidotto
    '                                        oAccertato(i).ImpInteressi = oDettaglioAtto(x).ImpInteressi
    '                                    End If
    '                                End If
    '                            Next
    '                        Next
    '                        'controllo che il totale sanzioni applicate sia superiore al minimo
    '                        If TotImportoSanzioniACCERTAMENTO < impMinimoSanzione And impMinimoSanzione > 0 Then
    '                            Dim impDifSanzXMinimo As Double = 0
    '                            impDifSanzXMinimo = (impMinimoSanzione - TotImportoSanzioniACCERTAMENTO)
    '                            TotImportoSanzioniACCERTAMENTO = 0 : TotImportoSanzioniRidottoACCERTAMENTO = 0
    '                            'la prima UI avrà anche la differenza per arrivare al minimo sanzione
    '                            For Each UIAcc As ObjArticoloAccertamento In oAccertato
    '                                If UIAcc.IdLegame = "1" Then
    '                                    UIAcc.ImpSanzioni += impDifSanzXMinimo
    '                                    UIAcc.ImpSanzioniRid = FormatNumber(UIAcc.ImpSanzioni / nQuotaRiduzioneSanzioni, 2)
    '                                End If
    '                                'ricalcolo il totale sanzioni
    '                                TotImportoSanzioniACCERTAMENTO += UIAcc.ImpSanzioni
    '                                TotImportoSanzioniRidottoACCERTAMENTO += UIAcc.ImpSanzioniRid
    '                            Next
    '                        End If
    '                        oAtto.IMPORTO_SANZIONI = TotImportoSanzioniACCERTAMENTO
    '                        oAtto.IMPORTO_SANZIONI_RIDOTTO = TotImportoSanzioniRidottoACCERTAMENTO
    '                        oAtto.IMPORTO_INTERESSI = TotImportoInteressiACCERTAMENTO
    '                        Log.Debug(sIdEnte & " - TotImportoSanzioniACCERTAMENTO:" & TotImportoSanzioniACCERTAMENTO)
    '                        Log.Debug(sIdEnte & " - TotImportoSanzioniRidottoACCERTAMENTO:" & TotImportoSanzioniRidottoACCERTAMENTO)
    '                        Log.Debug(sIdEnte & " - TotImportoInteressiACCERTAMENTO:" & TotImportoInteressiACCERTAMENTO)
    '                    Else
    '                        'Se sono qui l'ici dichiarato è uguale a quello accertato Sanzioni e Interessi a 0
    '                        strConnectionStringAnagrafica = ConstSession.StringConnectionAnagrafica
    '                        If objHashTable.ContainsKey("CONNECTIONSTRINGANAGRAFICA") = True Then
    '                            objHashTable("CONNECTIONSTRINGANAGRAFICA") = strConnectionStringAnagrafica
    '                        Else
    '                            objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", strConnectionStringAnagrafica)
    '                        End If
    '                        If objHashTable.ContainsKey("CODENTE") = True Then
    '                            objHashTable("CODENTE") = sIdEnte
    '                        Else
    '                            objHashTable.Add("CODENTE", sIdEnte)
    '                        End If

    '                        If objHashTable.ContainsKey("CODCONTRIBUENTE") = False Then
    '                            objHashTable.Add("CODCONTRIBUENTE", oAccertato(0).IdContribuente)
    '                        End If
    '                        oAtto.IMPORTO_INTERESSI = 0
    '                        oAtto.IMPORTO_SANZIONI = 0
    '                        oAtto.IMPORTO_SANZIONI_RIDOTTO = 0
    '                    End If
    '                    'estraggo le addizionali che devono essere applicate se il check è selezionato
    '                    If bCalcolaAddizionali = True Then
    '                        Dim impImponibileAddiz As Double = 0
    '                        'per TARSU uso l'imposta dell'atto per TARES uso tutto tranne maggiorazione
    '                        If sTipoCalcolo = ObjRuolo.TipoCalcolo.TARSU Then
    '                            impImponibileAddiz = oAtto.IMPORTO_DIFFERENZA_IMPOSTA
    '                        Else
    '                            For Each myArt As ObjArticoloAccertamento In oAccertato
    '                                If myArt.TipoPartita <> ObjArticolo.PARTEMAGGIORAZIONE Then
    '                                    impImponibileAddiz += myArt.impNetto
    '                                End If
    '                            Next
    '                        End If
    '                        ObjAddizionaleServizio = GetAddizionaliAtto(sIdEnte, impImponibileAddiz, oAtto, sScript)
    '                    End If
    '                    Dim ImportoTotaleAvviso As Double
    '                    ImportoTotaleAvviso = oAtto.IMPORTO_DIFFERENZA_IMPOSTA + TotImportoSanzioniACCERTAMENTO + TotImportoInteressiACCERTAMENTO
    '                    Log.Debug(sIdEnte & " - ImportoTotaleAvviso:" & ImportoTotaleAvviso)
    '                    If ImportoTotaleAvviso < 0 Then
    '                        'sarebbe un rimborso Non emetto Avviso
    '                        Log.Debug(sIdEnte & " - Importo Avviso inferiore a zero. L'avviso non verrà emesso")
    '                        TipoAvviso = OggettoAtto.Provvedimento.Rimborso '5
    '                        sScript += "GestAlert('a', 'warning', '', '', 'Importo Avviso inferiore a zero. L\'avviso non verrà emesso');"
    '                        sScript += "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
    '                    Else
    '                        'reperisco soglia
    '                        Dim soglia As Double
    '                        Dim objProvvedimentiDB As New DBPROVVEDIMENTI.ProvvedimentiDB
    '                        soglia = 0
    '                        soglia = objProvvedimentiDB.GetSogliaMinima(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_TARSU, objHashTable("CODENTE"), TipoAvviso)
    '                        Log.Debug(sIdEnte & " - Soglia:" & soglia & " €")
    '                        'Calcolo le spese
    '                        Dim spese As Double
    '                        spese = objProvvedimentiDB.GetSpese(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_TARSU, objHashTable("CODENTE"), TipoAvviso)
    '                        Log.Debug(sIdEnte & " - Spese:" & spese & " €")
    '                        If Not objDSCalcoloSanzioniInteressi Is Nothing Then
    '                            If objDSCalcoloSanzioniInteressi.Tables.Count > 0 Then
    '                                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI") = TotImportoSanzioniACCERTAMENTO
    '                                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO") = TotImportoSanzioniRidottoACCERTAMENTO

    '                                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI") = TotImportoInteressiACCERTAMENTO
    '                                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("DIFFERENZA_IMPOSTA_TOTALE") = oAtto.IMPORTO_DIFFERENZA_IMPOSTA
    '                            End If
    '                        End If

    '                        ' Aggiorno il DB dopo procedura di accertamento
    '                        If Not objDSSanzioni Is Nothing Then
    '                            objDSSanzioni.Dispose()
    '                        End If
    '                        objDSSanzioni = New DataSet
    '                        objDSSanzioni.Tables.Add(dtSanzioni.Copy)

    '                        oAtto.CAP_CO = ""
    '                        oAtto.CAP_RES = ""
    '                        oAtto.CITTA_CO = ""
    '                        oAtto.CITTA_RES = ""
    '                        oAtto.CIVICO_CO = ""
    '                        oAtto.CIVICO_RES = ""
    '                        oAtto.CO = ""
    '                        oAtto.CODICE_FISCALE = ""
    '                        oAtto.COGNOME = ""
    '                        oAtto.ESPONENTE_CIVICO_CO = ""
    '                        oAtto.ESPONENTE_CIVICO_RES = ""
    '                        oAtto.FRAZIONE_CO = ""
    '                        oAtto.FRAZIONE_RES = ""
    '                        oAtto.NOME = ""
    '                        oAtto.PARTITA_IVA = ""
    '                        oAtto.POSIZIONE_CIVICO_CO = ""
    '                        oAtto.POSIZIONE_CIVICO_RES = ""
    '                        oAtto.PROVINCIA_CO = ""
    '                        oAtto.PROVINCIA_RES = ""
    '                        oAtto.VIA_CO = ""
    '                        oAtto.VIA_RES = ""
    '                        oAtto.COD_CONTRIBUENTE = CInt(objHashTable("CODCONTRIBUENTE"))
    '                        oAtto.IMPORTO_SPESE = spese
    '                        oAtto.DATA_ELABORAZIONE = DateTime.Now.ToString("yyyyMMdd")
    '                        'aggiorno l'importo totale comprensivo di arrotondamento
    '                        oAtto.IMPORTO_TOTALE = (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO)
    '                        oAtto.IMPORTO_ARROTONDAMENTO = CDbl(ImportoArrotondato(oAtto.IMPORTO_TOTALE)) - oAtto.IMPORTO_TOTALE
    '                        oAtto.IMPORTO_TOTALE = oAtto.IMPORTO_TOTALE + oAtto.IMPORTO_ARROTONDAMENTO + oAtto.IMPORTO_SPESE
    '                        oAtto.IMPORTO_TOTALE_RIDOTTO = oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI_RIDOTTO + oAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO
    '                        oAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO = CDbl(ImportoArrotondato(oAtto.IMPORTO_TOTALE_RIDOTTO)) - oAtto.IMPORTO_TOTALE_RIDOTTO
    '                        oAtto.IMPORTO_TOTALE_RIDOTTO = oAtto.IMPORTO_TOTALE_RIDOTTO + oAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO + oAtto.IMPORTO_SPESE
    '                        For i = 0 To oAccertato.Length - 1
    '                            If Year(oAccertato(i).tDataInizio) < oAtto.ANNO Then
    '                                oAccertato(i).tDataInizio = "01/01/" & oAtto.ANNO
    '                            End If
    '                            oAtto.IMPORTO_ACCERTATO_ACC += oAccertato(i).impNetto
    '                        Next
    '                        For i = 0 To oDichiarato.Length - 1
    '                            If Year(oDichiarato(i).tDataInizio) < oAtto.ANNO Then
    '                                oDichiarato(i).tDataInizio = "01/01/" & oAtto.ANNO
    '                            End If
    '                            oAtto.IMPORTO_DICHIARATO_F2 += oDichiarato(i).impNetto
    '                        Next
    '                        If objHashTable.ContainsKey("VALORE_RITORNO_ACCERTAMENTO") = True Then
    '                            objHashTable.Remove("VALORE_RITORNO_ACCERTAMENTO")
    '                        End If
    '                        If objHashTable.Contains("TipoTassazione") = False Then
    '                            objHashTable.Add("TipoTassazione", sTipoCalcolo)
    '                        End If

    '                        'aggiungo anche VALORE_RITORNO_ACCERTAMENTO=0 per creare un accertamento per un anno in cui è presente un acc definitivo.
    '                        objHashTable.Add("VALORE_RITORNO_ACCERTAMENTO", sValRitornoAccertamento)

    '                        If objHashTable.ContainsKey("DATA_ANNULLAMENTO") Then objHashTable.Remove("DATA_ANNULLAMENTO")
    '                        If objHashTable.ContainsKey("DATA_RETTIFICA") Then objHashTable.Remove("DATA_RETTIFICA")

    '                        If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                            DATA_RETTIFICA_ANNULLAMENTO = DateTime.Now.ToString("yyyyMMdd")
    '                            If ImportoTotaleAvviso < soglia Then
    '                                objHashTable.Add("DATA_ANNULLAMENTO", DATA_RETTIFICA_ANNULLAMENTO)
    '                                objHashTable.Add("DATA_RETTIFICA", "")
    '                            Else
    '                                objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA_ANNULLAMENTO)
    '                                objHashTable.Add("DATA_ANNULLAMENTO", "")
    '                            End If
    '                        End If

    '                        'DETERMINO TIPO DI AVVISO 
    '                        Log.Debug(sIdEnte & " - Richiamo la funzione per determinare il tipo di avviso")
    '                        If FncGestAcc.DeterminaTipoAvviso(ImportoTotaleAvviso, 0, soglia, blnTIPO_OPERAZIONE_RETTIFICA, 0, TipoAvviso, sDescTipoAvviso) = False Then
    '                            Throw New Exception
    '                        End If
    '                        If TipoAvviso = OggettoAtto.Provvedimento.AutotutelaAnnullamento Then
    '                            If Not IsNothing(oAtto) Then
    '                                'devo azzerare tutti gli importi
    '                                oAtto.IMPORTO_ALTRO = 0
    '                                oAtto.IMPORTO_ARROTONDAMENTO = 0
    '                                oAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO = 0
    '                                oAtto.IMPORTO_INTERESSI = 0
    '                                oAtto.IMPORTO_SANZIONI = 0
    '                                oAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI = 0
    '                                oAtto.IMPORTO_SANZIONI_RIDOTTO = 0
    '                                oAtto.IMPORTO_SPESE = 0
    '                                oAtto.IMPORTO_TOTALE = 0
    '                                oAtto.IMPORTO_TOTALE_RIDOTTO = 0
    '                            End If
    '                        End If

    '                        If objHashTable.Contains("TIPOPROVVEDIMENTO") = False Then
    '                            objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
    '                        Else
    '                            objHashTable("TIPOPROVVEDIMENTO") = TipoAvviso
    '                        End If
    '                        Log.Debug(sIdEnte & " - ImportoTotaleAvviso:" & ImportoTotaleAvviso & " €")
    '                        Log.Debug(sIdEnte & " - TipoAvviso:" & TipoAvviso & " - " & sDescTipoAvviso)
    '                        Log.Debug("RICHIAMO objCOMUpdateDBAccertamenti.updateDBAccertamentiTARSU(objHashTable, objDSCalcoloSanzioniInteressi, objDSSanzioni, objDSInteressi, oAtto, oDettaglioAtto, oDichiarato, oAccertato, spese, ObjAddizionaleServizio) passando")
    '                        OggettoRiepilogoAccertamento = objCOMUpdateDBAccertamenti.updateDBAccertamentiTARSU(ConstSession.DBType, objHashTable, objDSCalcoloSanzioniInteressi, objDSSanzioni, ListInteressi, oAtto, oDettaglioAtto, oDichiarato, oAccertato, spese, ObjAddizionaleServizio, ConstSession.UserName)

    '                        'recupero id Provvedimento
    '                        If Not OggettoRiepilogoAccertamento Is Nothing Then
    '                            lngNewID_PROVVEDIMENTO = OggettoRiepilogoAccertamento(0).Id
    '                            'e lo assegno anche ai singoli immobili
    '                            For i = 0 To oAccertato.Length - 1
    '                                oAccertato(i).Id = lngNewID_PROVVEDIMENTO
    '                            Next
    '                            objDSSanzioni.Dispose()
    '                            objDSSanzioni.Dispose()

    '                            If ImportoTotaleAvviso = 0 Then
    '                                'Non emetto Avviso
    '                                sScript += "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
    '                                sScript += "parent.document.getElementById('attesaCarica').style.display='none';"
    '                                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                                    'Atto di autotutela di annullamento
    '                                    sScript += "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
    '                                    sScript += "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                                    Log.Debug(sIdEnte & " - La posizione è corretta. Ho elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO")
    '                                Else
    '                                    'Nessun avviso emesso
    '                                    'Effettuo il rientro dell'accertato
    '                                    '*** RIENTRO DA SISTEMARE ***
    '                                    'Dim Rientro As New BE_RientroDaAccertamento.ICI
    '                                    'Rientro.pathApplication = ConfigurationManager.AppSettings("PATH_LOG_IMMOBILI")
    '                                    'Rientro.PARAMETROENV = ConfigurationManager.AppSettings("PARAMETROENV")
    '                                    'Rientro.username = ConstSession.UserName
    '                                    'Rientro.IdentificativoApplicazione = ConfigurationManager.AppSettings("OPENGOVP")
    '                                    'Rientro.CODICE_ENTE = sIdEnte
    '                                    'If Rientro.Elabora(lngNewID_PROVVEDIMENTO) Then
    '                                    '    sscript+= "alert('La posizione è corretta.\nL\'accertato è stato inserito come nuova dichiarazione');"
    '                                    '    Log.Debug(sIdEnte & " - Effettuato il rientro dell'accertato")
    '                                    'Else
    '                                    '    sscript+= "alert('La posizione è corretta.\nSi è verificato un errore nel rientro dell'accertato');"
    '                                    '    Log.Debug(sIdEnte & " - Errore nel rientro dell'accertato")
    '                                    'End If
    '                                    sScript += "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                                End If
    '                            ElseIf ImportoTotaleAvviso < soglia Then
    '                                sScript += "parent.document.getElementById('attesaCarica').style.display='none';"
    '                                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                                    'Atto di autotutela di annullamento. Importo inferiore alla soglia
    '                                    sScript += "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
    '                                    sScript += "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                                    Log.Debug(sIdEnte & " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
    '                                Else
    '                                    'Effettuo il rientro dell'accertato
    '                                    'Nessun avviso emesso. Importo inferiore alla soglia
    '                                    '*** RIENTRO DA SISTEMARE ***
    '                                    'Dim Rientro As New BE_RientroDaAccertamento.ICI
    '                                    'Rientro.pathApplication = ConfigurationManager.AppSettings("PATH_LOG_IMMOBILI")
    '                                    'Rientro.PARAMETROENV = ConfigurationManager.AppSettings("PARAMETROENV")
    '                                    'Rientro.username = ConstSession.UserName
    '                                    'Rientro.IdentificativoApplicazione = ConfigurationManager.AppSettings("OPENGOVP")
    '                                    'Rientro.CODICE_ENTE = sIdEnte
    '                                    'If Rientro.Elabora(lngNewID_PROVVEDIMENTO) Then
    '                                    '    sscript+= "alert('Importo Avviso inferiore alla soglia.\nL\'accertato è stato inserito come nuova dichiarazione');"
    '                                    '    Log.Debug(sIdEnte & " - Effettuato il rientro dell'accertato")
    '                                    'Else
    '                                    '    sscript+= "alert('Importo Avviso inferiore alla soglia.\nSi è verificato un errore nel rientro dell'accertato');"
    '                                    '    Log.Debug(sIdEnte & " - Errore nel rientro dell'accertato")
    '                                    'End If
    '                                    sScript += "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                                End If
    '                                Return oAtto
    '                            End If

    '                            HttpContext.Current.Session.Add("oSituazioneAtto", oAtto)
    '                            HttpContext.Current.Session.Add("oSituazioneDichiarato", oDichiarato)
    '                            HttpContext.Current.Session.Add("oSituazioneAccertato", oAccertato)
    '                            '******
    '                            If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                                sScript += "FineElaborazioneAccertamento();" & vbCrLf
    '                                sScript += "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                            Else
    '                                sScript += "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                            End If
    '                            Log.Debug(sIdEnte & " - registro script per visualizzare il riepilogo accertamento")
    '                        Else
    '                            sScript += "GestAlert('a', 'danger', '', '', 'Impossibile proseguire! Si è verificato un\'errore in salvataggio!');"
    '                            sScript += "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
    '                            oAtto = Nothing
    '                        End If
    '                    End If
    '                Else
    '                    oAtto = Nothing
    '                End If
    '            ElseIf ImpTotAccert < ImpTotDich Then
    '                'accertato minore dichiarato Non emetto Avviso Non esiste rimborso
    '                sScript += "GestAlert('a', 'warning', '', '', 'Importo Accertato inferiore all\' Importo Dichiarato. L\'avviso non verrà emesso');"
    '                sScript += "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
    '            End If
    '        Else
    '            'manca accertato
    '            oAtto = Nothing
    '        End If
    '        Log.Debug(sIdEnte & " - Fine")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.TARSUConfrontoAccertatoDichiarato.errore: ", ex)
    '        oAtto = Nothing
    '    End Try
    '    Return oAtto
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myAtto"></param>
    ''' <returns></returns>
    Private Function AzzeraTotaliAtto(myAtto As OggettoAttoTARSU) As OggettoAttoTARSU
        Try
            'devo azzerare tutti gli importi
            myAtto.IMPORTO_ALTRO = 0
            myAtto.IMPORTO_ARROTONDAMENTO = 0
            myAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO = 0
            myAtto.IMPORTO_INTERESSI = 0
            myAtto.IMPORTO_SANZIONI = 0
            myAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI = 0
            myAtto.IMPORTO_SANZIONI_RIDOTTO = 0
            myAtto.IMPORTO_SPESE = 0
            myAtto.IMPORTO_TOTALE = 0
            myAtto.IMPORTO_TOTALE_RIDOTTO = 0
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.AzzeraTotaliAtto.errore: ", ex)
            Return Nothing
        End Try
        Return myAtto
    End Function

    Private Function GetAddizionaliAtto(IdEnte As String, ByVal impImponibile As Double, ByRef myAtto As OggettoAttoTARSU, ByRef sScript As String) As OggettoAddizionaleAccertamento()
        Dim ListAddizionali() As OggettoAddizionaleAccertamento = Nothing
        Dim FncAddiz As New OPENgovTIA.GestAddizionali

        Try
            Dim objSearch As New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
            Dim DsAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
            Dim myItem As OggettoAddizionaleAccertamento

            objSearch.Anno = myAtto.ANNO
            'Log.Debug("devo prendere addiz")
            DsAddizionali = FncAddiz.GetAddizionale(ConstSession.StringConnectionTARSU, IdEnte, myAtto.ANNO, "", "")
            If DsAddizionali Is Nothing Then
                sScript = "GestAlert('a', 'warning', '', '', 'Non sono state configurate le addizionali!Non si può procedere con la procedura di Accertamento!');"
            Else
                Dim IntAddizionale As Integer
                Dim ImpAddizionale As Double
                myAtto.IMPORTO_ALTRO = 0

                For Each myAddiz As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale In DsAddizionali
                    ImpAddizionale = 0
                    ImpAddizionale = CDbl(impImponibile * myAddiz.Valore / 100)
                    myAtto.IMPORTO_ALTRO += ImpAddizionale
                    myItem = New OggettoAddizionaleAccertamento
                    myItem.Anno = myAddiz.Anno
                    myItem.CodiceCapitolo = myAddiz.CodiceCapitolo
                    myItem.idAddizionale = myAddiz.idAddizionale
                    myItem.Valore = myAddiz.Valore
                    myItem.ImportoCalcolato = ImpAddizionale
                    myItem.Descrizione = myAddiz.Descrizione
                    ReDim Preserve ListAddizionali(IntAddizionale)
                    ListAddizionali(IntAddizionale) = myItem
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.GetAddizionaliAtto.errore: ", ex)
            ListAddizionali = Nothing
        End Try
        Return ListAddizionali
    End Function
    '*** ***

    Public Function TARSU_PopolaAtto(ByVal sIdEnte As String, ByVal oDettaglioAtto() As OggettoDettaglioAtto) As OggettoAttoTARSU
        Dim myAtto As New OggettoAttoTARSU

        Try
            For Each myDet As OggettoDettaglioAtto In oDettaglioAtto
                myAtto.IMPORTO_DIFFERENZA_IMPOSTA += FormatNumber(myDet.ImpAccertato - myDet.ImpDichiarato, 2)
            Next

            myAtto.COD_CONTRIBUENTE = oDettaglioAtto(0).IdContribuente
            myAtto.COD_ENTE = sIdEnte
            myAtto.COD_TRIBUTO = Utility.Costanti.TRIBUTO_TARSU
            myAtto.ANNO = oDettaglioAtto(0).Anno
            myAtto.CAP_CO = ""
            myAtto.CAP_RES = ""
            myAtto.CITTA_CO = ""
            myAtto.CITTA_RES = ""
            myAtto.CIVICO_CO = ""
            myAtto.CIVICO_RES = ""
            myAtto.CO = ""
            myAtto.CODICE_FISCALE = ""
            myAtto.COGNOME = ""
            myAtto.ESPONENTE_CIVICO_CO = ""
            myAtto.ESPONENTE_CIVICO_RES = ""
            myAtto.FRAZIONE_CO = ""
            myAtto.FRAZIONE_RES = ""
            myAtto.NOME = ""
            myAtto.PARTITA_IVA = ""
            myAtto.POSIZIONE_CIVICO_CO = ""
            myAtto.POSIZIONE_CIVICO_RES = ""
            myAtto.PROVINCIA_CO = ""
            myAtto.PROVINCIA_RES = ""
            myAtto.VIA_CO = ""
            myAtto.VIA_RES = ""
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.TARSU_PopolaAtto.errore: ", ex)
            myAtto = Nothing
        End Try
        Return myAtto
    End Function


    'Private Function CreateDatasetPerSanzInt(ByVal anno As Integer, ByVal codContribuente As String, ByVal DiffImposta As Double, ByVal DiffImpostaACCONTO As Double, ByVal DiffImpostaSALDO As Double, ImpPagato As Double) As DataSet
    '    Dim objDS As New DataSet

    '    Dim newTable As DataTable
    '    newTable = New DataTable("TABLE")
    '    Dim NewColumn As New DataColumn
    '    Try
    '        NewColumn = New DataColumn

    '        NewColumn.ColumnName = "COD_CONTRIBUENTE"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ANNO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)


    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DIFFERENZA_IMPOSTA_ACCONTO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DIFFERENZA_IMPOSTA_SALDO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DIFFERENZA_IMPOSTA_TOTALE"
    '        'NewColumn.DataType = System.Type.GetType("System.String")
    '        'NewColumn.DefaultValue = "0"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_SANZIONI"
    '        'NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_SANZIONI_RIDOTTO"
    '        'NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_INTERESSI"
    '        'NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FLAG_MODALITA_UNICA_SOLUZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.Boolean")
    '        NewColumn.DefaultValue = False
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_TOTALE_DICHIARATO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_TOTALE_VERSATO"
    '        'NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_TOTALE_ACCERTATO"
    '        'NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)
    '        '*** 20140701 - IMU/TARES ***
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "QUOTARIDUZIONESANZIONI"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = 1
    '        newTable.Columns.Add(NewColumn)
    '        '*** ***

    '        Dim row1 As DataRow

    '        row1 = newTable.NewRow()

    '        row1.Item("COD_CONTRIBUENTE") = codContribuente
    '        row1.Item("ANNO") = anno
    '        row1.Item("IMPORTO_SANZIONI") = 0
    '        row1.Item("IMPORTO_SANZIONI_RIDOTTO") = 0
    '        row1.Item("IMPORTO_INTERESSI") = 0
    '        row1.Item("DIFFERENZA_IMPOSTA_TOTALE") = DiffImposta
    '        row1.Item("DIFFERENZA_IMPOSTA_ACCONTO") = DiffImpostaACCONTO
    '        row1.Item("DIFFERENZA_IMPOSTA_SALDO") = DiffImpostaSALDO

    '        row1.Item("IMPORTO_TOTALE_DICHIARATO") = 0
    '        row1.Item("IMPORTO_TOTALE_VERSATO") = ImpPagato
    '        row1.Item("IMPORTO_TOTALE_ACCERTATO") = 0

    '        row1.Item("FLAG_MODALITA_UNICA_SOLUZIONE") = False
    '        '*** 20140701 - IMU/TARES ***
    '        row1.Item("QUOTARIDUZIONESANZIONI") = 3
    '        '*** ***

    '        newTable.Rows.Add(row1)

    '        objDS.Tables.Add(newTable)

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.CreateDatasetPerSanzInt.errore: ", ex)
    '    End Try
    '    Return objDS

    'End Function

    Public Function ImportoArrotondato(ByVal importo As Double) As String
        'Funzione che in base alla nuova finanziaria prevede
        'gli importi arrotondati
        'x= importo da arrotondare + 0.5
        'importo arrotondato = parte intera di x

        Dim X As Double
        Dim ImportoOut As Long
        Try
            If importo > 0 Then

                X = importo + 0.5
                If InStr(X, ",") > 0 Then
                    ImportoOut = Left(X, InStr(X, ",") - 1)
                Else
                    ImportoOut = X
                End If

            ElseIf importo < 0 Then

                X = importo - 0.5
                If InStr(X, ",") > 0 Then
                    ImportoOut = Left(X, InStr(X, ",") - 1)
                Else
                    ImportoOut = X
                End If

            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.ImportArrotondato.errore: ", ex)
        End Try
        Return ImportoOut
    End Function

    'Public Function CreateDSperCalcoloICI() As DataSet

    '    Dim objDS As DataSet = New DataSet
    '    Dim newTable As DataTable

    '    newTable = New DataTable("TP_SITUAZIONE_FINALE_ICI")
    '    Dim NewColumn As DataColumn = New DataColumn
    '    Try
    '        NewColumn.ColumnName = "ID_SITUAZIONE_FINALE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)
    '        NewColumn = New DataColumn

    '        NewColumn.ColumnName = "ANNO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)
    '        NewColumn = New DataColumn

    '        NewColumn.ColumnName = "COD_ENTE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ID_PROCEDIMENTO"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ID_RIFERIMENTO"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "PROVENIENZA"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CARATTERISTICA"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "INDIRIZZO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "SEZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FOGLIO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NUMERO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "SUBALTERNO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CATEGORIA"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CLASSE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "PROTOCOLLO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FLAG_STORICO"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "VALORE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)
    '        '*** 20140509 - TASI ***
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "VALORE_REALE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)
    '        '*** ***

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FLAG_PROVVISORIO"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "PERC_POSSESSO"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "MESI_POSSESSO"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "MESI_ESCL_ESENZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "MESI_RIDUZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_DETRAZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FLAG_POSSEDUTO"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FLAG_ESENTE"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FLAG_RIDUZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FLAG_PRINCIPALE"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "COD_CONTRIBUENTE"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "COD_IMMOBILE_PERTINENZA"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "COD_IMMOBILE"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DAL"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "AL"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NUMERO_MESI_ACCONTO"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NUMERO_MESI_TOTALI"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NUMERO_UTILIZZATORI"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "TIPO_RENDITA"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "RIDUZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "MESE_INIZIO"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DATA_SCADENZA"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)
    '        '*** 20140509 - TASI ***
    '        'NewColumn = New DataColumn
    '        'NewColumn.ColumnName = "TIPO_POSSESSO"
    '        'NewColumn.DataType = System.Type.GetType("System.String")
    '        'NewColumn.DefaultValue = ""
    '        'newTable.Columns.Add(NewColumn)
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CODTRIBUTO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IDTIPOUTILIZZO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IDTIPOPOSSESSO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ZONA"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataInizio"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)
    '        '*** ***
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "TIPO_OPERAZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)
    '        'DIPE 11/02/2009

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CONSISTENZA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ABITAZIONE_PRINCIPALE_ATTUALE"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "RENDITA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        'DIPE 02/03/2011
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_VALORE_ALIQUOTA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        '--------------------------------------------------------------------------------------
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_ACCONTO_SENZA_DETRAZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_APPLICATA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_DOVUTA_ACCONTO"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_RESIDUA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_TOTALE_SENZA_DETRAZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_APPLICATA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_TOTALE_DOVUTA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_RESIDUA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_DOVUTA_SALDO"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_DOVUTA_DETRAZIONE_SALDO"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_DOVUTA_SENZA_DETRAZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_DOVUTA_DETRAZIONE_RESIDUA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DIFFIMPOSTA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "TOTALE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)
    '        '*** Campi Detrazione Statale usati per non so cosa ***
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_STATALE_CALCOLATA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_STATALE_APPLICATA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_STATALE_RESIDUA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_SALDO_DETRAZIONE_STATALE_CALCOLATA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_SALDO_DETRAZIONE_STATALE_APPLICATA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_SALDO_DETRAZIONE_STATALE_RESIDUA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_STATALE_CALCOLATA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_STATALE_APPLICATA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_STATALE_RESIDUA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)
    '        '*** ***
    '        '*** 20120530 - IMU ***
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "COLTIVATOREDIRETTO"
    '        NewColumn.DataType = System.Type.GetType("System.Boolean")
    '        NewColumn.DefaultValue = False
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NUMEROFIGLI"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "PERCENTCARICOFIGLI"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_VALORE_ALIQUOTA_STATALE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_DOVUTA_ACCONTO_STATALE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_APPLICATA_STATALE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_RESIDUA_STATALE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_TOTALE_DOVUTA_STATALE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_APPLICATA_STATALE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_RESIDUA_STATALE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_DOVUTA_SALDO_STATALE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_DOVUTA_DETRAZIONE_SALDO_STATALE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ICI_DOVUTA_DETRAZIONE_RESIDUA_STATALE"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)
    '        '*** ***
    '        '*** 20130422 - aggiornamento IMU ***
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ID_ALIQUOTA"
    '        NewColumn.DataType = System.Type.GetType("System.Double")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)
    '        '*** ***
    '        objDS.Tables.Add(newTable)

    '        Return objDS
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.CreateDSperCalcoloICI.errore: ", ex)
    '    End Try
    'End Function

    '*** 20130801 - accertamento OSAP ***
    ''' <summary>
    ''' Determino il Tipo di Avviso (Accertamento o Ufficio) Se trovo 1 immobile di accertato che non è in dichiarato ho avviso di Tipo = 4 altrimenti avviso di tipo 3
    ''' Cerco gli mmobili accertati in tutti gli immobili di dichiarato Se lo trovo esco dal ciclo e proseguo a cercare con immobili successivo di accertamento
    ''' CONFRONTO ACCERTATO E DICHIARATO ciclo su tutti i record accertati e sommo l'importo
    ''' se accertato maggiore dichiarato CONFRONTO ACCERTATO E PAGATO; la differenza di imposta dell'atto è data da differenza imposta accertamento+differenza imposta preaccertamento
    ''' per queste fasi mi serve la percentuale di scorporo del pagato vs il dovuto;
    ''' se hanno selezionato sanzione 26 - omesso/parziale versamento calcolo sanzioni e/o interessi per confronto importo dichiarato vs importo pagato 
    ''' se hanno selezionato sanzione 25 - tardivo versamento calcolo sanzioni e/o interessi confronto data scadenza vs data pagamento 
    ''' reperisco soglia; Calcolo le spese; Aggiorno il DB 
    ''' aggiorno l'importo totale comprensivo di arrotondamento
    ''' DETERMINO TIPO DI AVVISO 
    ''' recupero id Provvedimento e lo assegno anche ai singoli immobili
    ''' se accertato minore dichiarato Non emetto Avviso Non esiste rimborso
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="IdContribuente"></param>
    ''' <param name="sAnnoRiferimento"></param>
    ''' <param name="oDichiarato"></param>
    ''' <param name="oAccertato"></param>
    ''' <param name="blnTIPO_OPERAZIONE_RETTIFICA"></param>
    ''' <param name="objHashTable"></param>
    ''' <param name="objDSMotivazioniSanzioni"></param>
    ''' <param name="sDescTipoAvviso"></param>
    ''' <param name="sScript"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="05/11/2011">
    ''' le spese devono essere messe dopo l'arrotondamento
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="30/03/2017">
    ''' tolto pagato per gestire correttamente la sazione su TARDIVO VERSAMENTO
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="05/10/2017">
    ''' per la NOFASE la differenza di imposta dell'atto è data da accertato-dichiarato; devo registrare l'importo Accertamento
    ''' per la FASE2 la differenza di imposta dell'atto è data da dichiarato-versato
    ''' devo sommare l'importo FASE1+FASE2 come PREAccertamento
    ''' per la FASE1 la differenza di imposta dell'atto è data dal pagato 
    ''' la differenza di imposta dell'atto è data da differenza imposta accertamento+differenza imposta preaccertamento
    ''' devo passare TotAcc e TotPREAcc
    ''' </revision>
    ''' </revisionHistory>
    Public Function OSAPConfrontoAccertatoDichiarato(ByVal sIdEnte As String, ByVal IdContribuente As Integer, ByVal sAnnoRiferimento As String, ByVal oDichiarato() As OSAPAccertamentoArticolo, ByVal oAccertato() As OSAPAccertamentoArticolo, ByVal blnTIPO_OPERAZIONE_RETTIFICA As Boolean, ByVal objHashTable As Hashtable, ByVal objDSMotivazioniSanzioni As DataSet, sDataMorte As String, ByRef sDescTipoAvviso As String, ByRef sScript As String) As OggettoAttoOSAP
        Dim x, y, nList, i As Integer
        Dim nLegamePrec As Integer = -1
        Dim ListDettaglioAtto() As OggettoDettaglioAtto
        Dim myDettaglioAtto As OggettoDettaglioAtto

        Dim ImpTotAccert As Double = 0
        Dim ImpTotDich As Double = 0
        Dim oAtto As New OggettoAttoOSAP
        Dim oPagatoOSAP() As OggettoPagamenti
        Dim FncPagato As New OPENgovTIA.ClsGestPag

        Dim OggettoRiepilogoAccertamento() As OSAPAccertamentoArticolo

        Dim lngNewID_PROVVEDIMENTO As Long
        Dim DATA_RETTIFICA_ANNULLAMENTO As String

        Dim blnResult As Boolean = False
        Dim objCOMUpdateDBAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
        Dim dsSanzioni, dsSanzioniImpDicVSImpPag, dsSanzioniScadDicVSDataPag, dsInteressi, dsInteressiImpDicVSImpPag, dsInteressiScadDicVSDataPag, dsInteressiTMP, dsSanzioniTMP As DataSet
        Dim oCalcoloSanzioniInteressi As New ObjBaseIntSanz
        Dim ImportoTotaleAvviso, ImportoTotAcc, ImportoTotPREAcc As Double

        Try
            ImportoTotaleAvviso = 0 : ImportoTotAcc = 0 : ImportoTotPREAcc = 0
            dsSanzioni = New DataSet : dsSanzioniImpDicVSImpPag = New DataSet : dsSanzioniScadDicVSDataPag = New DataSet : dsInteressi = New DataSet : dsInteressiImpDicVSImpPag = New DataSet : dsInteressiScadDicVSDataPag = New DataSet : dsInteressiTMP = New DataSet : dsSanzioniTMP = New DataSet

            If Not oAccertato Is Nothing Then
                'Determino il Tipo di Avviso (Accertamento o Ufficio) Se trovo 1 immobile di accertato che non è in dichiarato ho avviso di Tipo = 4 altrimenti avviso di tipo 3
                Dim DichiaratoNothing As New OSAPAccertamentoArticolo
                If oDichiarato Is Nothing Then
                    ReDim Preserve oDichiarato(0)
                    oDichiarato(0) = DichiaratoNothing
                End If
                Dim TipoAvviso As Integer

                TipoAvviso = OggettoAtto.Provvedimento.AccertamentoRettifica                '4

                'Prendo immobili di accertato
                For i = 0 To oAccertato.Length - 1
                    Dim Trovato As Boolean = False
                    'Cerco l'immobili in tutti gli immobili di dichiarato Se lo trovo esco dal ciclo e proseguo a cercare con immobili successivo di accertamento
                    For y = 0 To oDichiarato.Length - 1
                        If oAccertato(i).IdLegame = oDichiarato(y).IdLegame Then
                            Trovato = True
                            Exit For
                        End If
                    Next

                    'Se trovato = False vuol dire che nn ho trovato l'immobile
                    If Trovato = False Then
                        'Avviso D'ufficio
                        TipoAvviso = OggettoAtto.Provvedimento.AccertamentoUfficio                         '3
                        Exit For
                    End If
                Next
                nList = -1
                'CONFRONTO ACCERTATO E DICHIARATO ciclo su tutti i record accertati
                For x = 0 To oAccertato.GetUpperBound(0)
                    'sommo gli importi a parità di livello di legame
                    If oAccertato(x).IdLegame <> nLegamePrec Then
                        'aggiungo un record
                        nList += 1
                        myDettaglioAtto = New OggettoDettaglioAtto
                        myDettaglioAtto.IdLegame = oAccertato(x).IdLegame
                        myDettaglioAtto.Progressivo = oAccertato(x).Progressivo
                        myDettaglioAtto.Sanzioni = oAccertato(x).Sanzioni
                        myDettaglioAtto.Interessi = oAccertato(x).Interessi
                        myDettaglioAtto.Calcola_Interessi = oAccertato(x).Calcola_Interessi
                    End If
                    'sommo l'importo
                    myDettaglioAtto.ImpAccertato += oAccertato(x).Calcolo.ImportoCalcolato

                    ReDim Preserve ListDettaglioAtto(nList)
                    ListDettaglioAtto(nList) = myDettaglioAtto
                Next

                'ciclo su tutti i record dichiarati
                For x = 0 To oDichiarato.GetUpperBound(0)
                    'cerco il corrispettivo legame nell'oggetto di confronto
                    For y = 0 To ListDettaglioAtto.GetUpperBound(0)
                        If ListDettaglioAtto(y).IdLegame = oDichiarato(x).IdLegame Then
                            ListDettaglioAtto(y).ImpDichiarato += oDichiarato(x).Calcolo.ImportoCalcolato
                            Exit For
                        End If
                    Next
                Next

                'ciclo sull'oggetto totale per verificare il tipo di atto
                nList = -1
                For x = 0 To ListDettaglioAtto.GetUpperBound(0)
                    ImpTotAccert += ListDettaglioAtto(x).ImpAccertato
                    ImpTotDich += ListDettaglioAtto(x).ImpDichiarato
                Next
                nList += 1
                Log.Debug("totale dichiarato=" & ImpTotDich.ToString)
                Log.Debug("totale acccertato=" & ImpTotAccert.ToString)
                If ImpTotAccert >= ImpTotDich Then
                    'accertato maggiore dichiarato
                    oAtto = OSAPPopolaAtto(sIdEnte, ListDettaglioAtto, ImpTotDich, ImpTotAccert)
                    oAtto.ANNO = oAccertato(0).Anno
                    If Not oAtto Is Nothing Then
                        'SE HO UN ATTO CONFRONTO ACCERTATO E PAGATO
                        Dim SearchParamsPag As New OPENgovTIA.ObjSearchPagamenti
                        SearchParamsPag.sEnte = sIdEnte
                        SearchParamsPag.sAnnoRif = oAtto.ANNO
                        SearchParamsPag.IdContribuente = IdContribuente
                        SearchParamsPag.IdTributo = Utility.Costanti.TRIBUTO_OSAP
                        oPagatoOSAP = FncPagato.GetListPagamenti(SearchParamsPag, ConstSession.StringConnectionOSAP)
                        If Not oPagatoOSAP Is Nothing Then
                            Dim IPagato As Integer
                            For IPagato = 0 To oPagatoOSAP.Length - 1
                                oAtto.IMPORTO_PAGATO += oPagatoOSAP(IPagato).dImportoPagamento
                            Next
                        Else
                            oAtto.IMPORTO_PAGATO = 0
                        End If
                        oAtto.IMPORTO_VERSATO_F2 = oAtto.IMPORTO_PAGATO
                        'la differenza di imposta dell'atto è data da differenza imposta accertamento+differenza imposta preaccertamento
                        '*** 20170330 - tolto pagato per gestire correttamente la sazione su TARDIVO VERSAMENTO ***
                        Dim TipoAvvisoRimborso As Integer = -1

                        'Rimborso. Calcolo gli interessi Attivi sul singolo immobile. Al giro dopo la var viene azzerrata a -1.
                        If oAtto.IMPORTO_DIFFERENZA_IMPOSTA < 0 Then
                            TipoAvvisoRimborso = OggettoAtto.Provvedimento.Rimborso                            '"5"
                        End If

                        If objHashTable.ContainsKey("CODCONTRIBUENTE") = False Then
                            objHashTable.Add("CODCONTRIBUENTE", oAtto.COD_CONTRIBUENTE)
                        End If
                        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
                        If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = True Then
                            objHashTable.Remove("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI")
                        End If
                        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

                        objHashTable("IDSOTTOAPPLICAZIONEANAGRAFICA") = ConfigurationManager.AppSettings("OPENGOVTOCO")
                        objHashTable("CONNECTIONSTRINGANAGRAFICA") = ConstSession.StringConnectionAnagrafica
                        objHashTable("CODTRIBUTO") = Utility.Costanti.TRIBUTO_OSAP
                        objHashTable("CODENTE") = sIdEnte
                        If objHashTable.ContainsKey("TIPOPROVVEDIMENTO") = True Then
                            objHashTable.Remove("TIPOPROVVEDIMENTO")
                        End If
                        'TipoAvvisoRimborso = -1 == False ----> Non è un rimborso
                        'Devo effettuare il calcolo guardando se è un rimborso sull'immobile oppure no
                        'Se è rimborso ho solo interssi attivi
                        If TipoAvvisoRimborso = -1 Then
                            objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
                        Else
                            objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvvisoRimborso)
                        End If
                        If objHashTable.ContainsKey("COD_TIPO_PROCEDIMENTO") = True Then
                            objHashTable("COD_TIPO_PROCEDIMENTO") = OggettoAtto.Procedimento.Accertamento
                        Else
                            objHashTable.Add("COD_TIPO_PROCEDIMENTO", OggettoAtto.Procedimento.Accertamento)
                        End If
                        If objHashTable.Contains("ANNOACCERTAMENTO") = False Then
                            objHashTable.Add("ANNOACCERTAMENTO", sAnnoRiferimento)
                        End If
                        'PER CONNESSIONE OSAP
                        objHashTable("IDSOTTOAPPLICAZIONEOSAP") = ConfigurationManager.AppSettings("OPENGOVTA")
                        objHashTable("CONNECTIONSTRINGOSAP") = ConstSession.StringConnectionOSAP

                        '*** 20171005 - per la NOFASE la differenza di imposta dell'atto è data da accertato-dichiarato ***
                        If OSAPCalcoloSanzioniInteressi(-1, oAccertato, ListDettaglioAtto, objHashTable, -1, sDataMorte, oCalcoloSanzioniInteressi, dsInteressi, dsSanzioni, oAtto, ImportoTotaleAvviso, -1, "") = False Then
                            Throw New Exception("Errore in calcolo Sanzioni e/o Interessi")
                        End If
                        '*** 20171005 - devo registrare l'importo Accertamento ***
                        ImportoTotAcc = ImportoTotaleAvviso
                        '*** ***
                        'per queste fasi mi serve la percentuale di scorporo del pagato vs il dovuto
                        Dim PercentualeScorporo As Double = oAtto.IMPORTO_PAGATO / oAtto.IMPORTO_DICHIARATO_F2
                        If PercentualeScorporo >= 0 Then
                            PercentualeScorporo = PercentualeScorporo
                        Else
                            PercentualeScorporo = 0
                        End If
                        '*** calcolo sanzioni e/o interessi confronto importo dichiarato vs importo pagato ***
                        'se hanno selezionato sanzione 26 - omesso/parziale versamento
                        Dim bDicVSPag As Boolean = False
                        For Each myDetAtto As OggettoDettaglioAtto In ListDettaglioAtto
                            Dim listSanz() As String
                            Dim myString As String
                            For Each mySanz As String In myDetAtto.Sanzioni.Split(",")
                                listSanz = mySanz.Split("#")
                                For Each myString In listSanz
                                    If myString = "26" Then ' codice voce FISSO per omesso/parziale versamento
                                        bDicVSPag = True
                                        Exit For
                                    End If
                                Next
                            Next
                        Next
                        Dim myRow As DataRow
                        If bDicVSPag = True Then
                            oAtto.IMPORTO_DIFFERENZA_IMPOSTA = (ImpTotDich - oAtto.IMPORTO_PAGATO)
                            '*** ***
                            If OSAPCalcoloSanzioniInteressi(-1, oAccertato, ListDettaglioAtto, objHashTable, PercentualeScorporo, sDataMorte, oCalcoloSanzioniInteressi, dsInteressiTMP, dsSanzioniTMP, oAtto, ImportoTotaleAvviso, OggettoAtto.Fase.VersatoDichiarato, "26") = False Then
                                Throw New Exception("Errore in calcolo Sanzioni e/o Interessi")
                            End If
                            ImportoTotPREAcc += ImportoTotaleAvviso
                            If dsInteressiImpDicVSImpPag.Tables.Count = 0 Then
                                dsInteressiImpDicVSImpPag.Tables.Add("A")
                                dsInteressiImpDicVSImpPag.Tables(0).Merge(dsInteressiTMP.Tables(0))
                            Else
                                For Each myRow In dsInteressiTMP.Tables(0).Rows
                                    dsInteressiImpDicVSImpPag.Tables(0).ImportRow(myRow)
                                Next
                            End If
                            If dsSanzioniImpDicVSImpPag.Tables.Count = 0 Then
                                dsSanzioniImpDicVSImpPag.Tables.Add("A")
                                dsSanzioniImpDicVSImpPag.Tables(0).Merge(dsSanzioniTMP.Tables(0))
                            Else
                                For Each myRow In dsSanzioniTMP.Tables(0).Rows
                                    dsSanzioniImpDicVSImpPag.Tables(0).ImportRow(myRow)
                                Next
                            End If
                        End If
                        '*** ***
                        '*** 20171005 - per la FASE2 la differenza di imposta dell'atto è data da dichiarato-versato ***
                        oAtto.IMPORTO_DIFFERENZA_IMPOSTA = (ImpTotDich - oAtto.IMPORTO_PAGATO)
                        '*** ***
                        If OSAPCalcoloSanzioniInteressi(OggettoAtto.Fase.VersatoDichiarato, oAccertato, ListDettaglioAtto, objHashTable, PercentualeScorporo, sDataMorte, oCalcoloSanzioniInteressi, dsInteressiTMP, dsSanzioniTMP, oAtto, ImportoTotaleAvviso, OggettoAtto.Fase.VersatoDichiarato, "") = False Then
                            Throw New Exception("Errore in calcolo Sanzioni e/o Interessi")
                        End If
                        '*** 20171005 - devo sommare l'importo FASE1+FASE2 come PREAccertamento ***
                        ImportoTotPREAcc += ImportoTotaleAvviso
                        If dsInteressiImpDicVSImpPag.Tables.Count = 0 Then
                            dsInteressiImpDicVSImpPag.Tables.Add("A")
                            dsInteressiImpDicVSImpPag.Tables(0).Merge(dsInteressiTMP.Tables(0))
                        Else
                            For Each myRow In dsInteressiTMP.Tables(0).Rows
                                dsInteressiImpDicVSImpPag.Tables(0).ImportRow(myRow)
                            Next
                        End If
                        If dsSanzioniImpDicVSImpPag.Tables.Count = 0 Then
                            dsSanzioniImpDicVSImpPag.Tables.Add("A")
                            dsSanzioniImpDicVSImpPag.Tables(0).Merge(dsSanzioniTMP.Tables(0))
                        Else
                            For Each myRow In dsSanzioniTMP.Tables(0).Rows
                                dsSanzioniImpDicVSImpPag.Tables(0).ImportRow(myRow)
                            Next
                        End If
                        '*** ***
                        '*** calcolo sanzioni e/o interessi confronto data scadenza vs data pagamento ***
                        'se hanno selezionato sanzione 25 - tardivo versamento
                        bDicVSPag = False
                        For Each myDetAtto As OggettoDettaglioAtto In ListDettaglioAtto
                            Dim listSanz() As String
                            Dim myString As String
                            For Each mySanz As String In myDetAtto.Sanzioni.Split(",")
                                listSanz = mySanz.Split("#")
                                For Each myString In listSanz
                                    If myString = "25" Then ' codice voce FISSO per tardivo versamento
                                        bDicVSPag = True
                                        Exit For
                                    End If
                                Next
                            Next
                        Next
                        If bDicVSPag = True Then
                            oAtto.IMPORTO_DIFFERENZA_IMPOSTA = oAtto.IMPORTO_PAGATO
                            '*** ***
                            If OSAPCalcoloSanzioniInteressi(-1, oAccertato, ListDettaglioAtto, objHashTable, PercentualeScorporo, sDataMorte, oCalcoloSanzioniInteressi, dsInteressiTMP, dsSanzioniTMP, oAtto, ImportoTotaleAvviso, OggettoAtto.Fase.VersamentiTardivi, "25") = False Then
                                Throw New Exception("Errore in calcolo Sanzioni e/o Interessi")
                            End If
                            ImportoTotPREAcc += ImportoTotaleAvviso
                            If dsInteressiScadDicVSDataPag.Tables.Count = 0 Then
                                dsInteressiScadDicVSDataPag.Tables.Add("A")
                                dsInteressiScadDicVSDataPag.Tables(0).Merge(dsInteressiTMP.Tables(0))
                            Else
                                For Each myRow In dsInteressiTMP.Tables(0).Rows
                                    dsInteressiScadDicVSDataPag.Tables(0).ImportRow(myRow)
                                Next
                            End If
                            If dsSanzioniScadDicVSDataPag.Tables.Count = 0 Then
                                dsSanzioniScadDicVSDataPag.Tables.Add("A")
                                dsSanzioniScadDicVSDataPag.Tables(0).Merge(dsSanzioniTMP.Tables(0))
                            Else
                                For Each myRow In dsSanzioniTMP.Tables(0).Rows
                                    dsSanzioniScadDicVSDataPag.Tables(0).ImportRow(myRow)
                                Next
                            End If
                        End If
                        '*** 20171005 - per la FASE1 la differenza di imposta dell'atto è data dal pagato ***
                        oAtto.IMPORTO_DIFFERENZA_IMPOSTA = oAtto.IMPORTO_PAGATO
                        '*** ***
                        If OSAPCalcoloSanzioniInteressi(OggettoAtto.Fase.VersamentiTardivi, oAccertato, ListDettaglioAtto, objHashTable, PercentualeScorporo, sDataMorte, oCalcoloSanzioniInteressi, dsInteressiTMP, dsSanzioniTMP, oAtto, ImportoTotaleAvviso, OggettoAtto.Fase.VersamentiTardivi, "") = False Then
                            Throw New Exception("Errore in calcolo Sanzioni e/o Interessi")
                        End If
                        '*** 20171005 - devo sommare l'importo FASE1+FASE2 come PREAccertamento ***
                        ImportoTotPREAcc += ImportoTotaleAvviso
                        If dsInteressiScadDicVSDataPag.Tables.Count = 0 Then
                            dsInteressiScadDicVSDataPag.Tables.Add("A")
                            dsInteressiScadDicVSDataPag.Tables(0).Merge(dsInteressiTMP.Tables(0))
                        Else
                            For Each myRow In dsInteressiTMP.Tables(0).Rows
                                dsInteressiScadDicVSDataPag.Tables(0).ImportRow(myRow)
                            Next
                        End If
                        If dsSanzioniScadDicVSDataPag.Tables.Count = 0 Then
                            dsSanzioniScadDicVSDataPag.Tables.Add("A")
                            dsSanzioniScadDicVSDataPag.Tables(0).Merge(dsSanzioniTMP.Tables(0))
                        Else
                            For Each myRow In dsSanzioniTMP.Tables(0).Rows
                                dsSanzioniScadDicVSDataPag.Tables(0).ImportRow(myRow)
                            Next
                        End If
                        '*** ***
                        '*** 20171005 - la differenza di imposta dell'atto è data da differenza imposta accertamento+differenza imposta preaccertamento ***
                        oAtto.IMPORTO_DIFFERENZA_IMPOSTA = (ImpTotAccert - ImpTotDich) + (ImpTotDich - oAtto.IMPORTO_PAGATO)
                        '*** ***
                        Log.Debug(sIdEnte & " - ImportoTotaleAvviso:" & ImportoTotaleAvviso)
                        If (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO) < 0 Then 'If ImportoTotaleAvviso < 0 Then
                            'sarebbe un rimborso Non emetto Avviso
                            Log.Debug(sIdEnte & " - Importo Avviso inferiore a zero. L'avviso non verrà emesso")
                            TipoAvviso = OggettoAtto.Provvedimento.Rimborso
                            sScript += "GestAlert('a', 'warning', '', '', 'Importo Avviso inferiore a zero. L\'avviso non verrà emesso');"
                        Else
                            'reperisco soglia
                            Dim soglia As Double
                            Dim objProvvedimentiDB As New DBPROVVEDIMENTI.ProvvedimentiDB
                            soglia = 0
                            soglia = objProvvedimentiDB.GetSogliaMinima(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_OSAP, objHashTable("CODENTE"), TipoAvviso)
                            Log.Debug(sIdEnte & " - Soglia:" & soglia & " €")
                            'Calcolo le spese
                            Dim spese As Double
                            spese = objProvvedimentiDB.GetSpese(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_OSAP, objHashTable("CODENTE"), TipoAvviso)
                            Log.Debug(sIdEnte & " - Spese:" & spese & " €")
                            ' Aggiorno il DB dopo procedura di accertamento
                            oAtto.CAP_CO = ""
                            oAtto.CAP_RES = ""
                            oAtto.CITTA_CO = ""
                            oAtto.CITTA_RES = ""
                            oAtto.CIVICO_CO = ""
                            oAtto.CIVICO_RES = ""
                            oAtto.CO = ""
                            oAtto.CODICE_FISCALE = ""
                            oAtto.COGNOME = ""
                            oAtto.ESPONENTE_CIVICO_CO = ""
                            oAtto.ESPONENTE_CIVICO_RES = ""
                            oAtto.FRAZIONE_CO = ""
                            oAtto.FRAZIONE_RES = ""
                            oAtto.NOME = ""
                            oAtto.PARTITA_IVA = ""
                            oAtto.POSIZIONE_CIVICO_CO = ""
                            oAtto.POSIZIONE_CIVICO_RES = ""
                            oAtto.PROVINCIA_CO = ""
                            oAtto.PROVINCIA_RES = ""
                            oAtto.VIA_CO = ""
                            oAtto.VIA_RES = ""
                            oAtto.COD_CONTRIBUENTE = CInt(objHashTable("CODCONTRIBUENTE"))
                            oAtto.IMPORTO_SPESE = spese
                            oAtto.DATA_ELABORAZIONE = DateTime.Now.ToString("yyyyMMdd")
                            'aggiorno l'importo totale comprensivo di arrotondamento
                            '*** 20111205 - le spese devono essere messe dopo l'arrotondamento ***
                            oAtto.IMPORTO_TOTALE = (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO)
                            oAtto.IMPORTO_ARROTONDAMENTO = CDbl(ImportoArrotondato(oAtto.IMPORTO_TOTALE)) - oAtto.IMPORTO_TOTALE
                            oAtto.IMPORTO_TOTALE = oAtto.IMPORTO_TOTALE + oAtto.IMPORTO_ARROTONDAMENTO + oAtto.IMPORTO_SPESE
                            oAtto.IMPORTO_TOTALE_RIDOTTO = oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI_RIDOTTO + oAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO
                            oAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO = CDbl(ImportoArrotondato(oAtto.IMPORTO_TOTALE_RIDOTTO)) - oAtto.IMPORTO_TOTALE_RIDOTTO
                            oAtto.IMPORTO_TOTALE_RIDOTTO = oAtto.IMPORTO_TOTALE_RIDOTTO + oAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO + oAtto.IMPORTO_SPESE
                            For i = 0 To oAccertato.Length - 1
                                If Year(oAccertato(i).DataInizioOccupazione) < oAtto.ANNO Then
                                    oAccertato(i).DataInizioOccupazione = "01/01/" & oAtto.ANNO
                                End If
                            Next
                            For i = 0 To oDichiarato.Length - 1
                                If Year(oDichiarato(i).DataInizioOccupazione) < oAtto.ANNO Then
                                    oDichiarato(i).DataInizioOccupazione = "01/01/" & oAtto.ANNO
                                End If
                            Next

                            If objHashTable.ContainsKey("DATA_ANNULLAMENTO") Then objHashTable.Remove("DATA_ANNULLAMENTO")
                            If objHashTable.ContainsKey("DATA_RETTIFICA") Then objHashTable.Remove("DATA_RETTIFICA")

                            If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                                DATA_RETTIFICA_ANNULLAMENTO = DateTime.Now.ToString("yyyyMMdd")
                                If (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO) < soglia Then 'If ImportoTotaleAvviso < soglia Then
                                    objHashTable.Add("DATA_ANNULLAMENTO", DATA_RETTIFICA_ANNULLAMENTO)
                                    objHashTable.Add("DATA_RETTIFICA", "")
                                Else
                                    objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA_ANNULLAMENTO)
                                    objHashTable.Add("DATA_ANNULLAMENTO", "")
                                End If
                            End If

                            '****** DETERMINO TIPO DI AVVISO ******
                            Log.Debug(sIdEnte & " - Richiamo la funzione per determinare il tipo di avviso")

                            '*** 20171005 - devo passare TotAcc e TotPREAcc ***
                            If DeterminaTipoAvviso(ImportoTotAcc, ImportoTotPREAcc, soglia, blnTIPO_OPERAZIONE_RETTIFICA, 0, TipoAvviso, sDescTipoAvviso) = False Then
                                Throw New Exception
                            End If
                            If TipoAvviso = OggettoAtto.Provvedimento.AutotutelaAnnullamento Then
                                If Not IsNothing(oAtto) Then
                                    'devo azzerare tutti gli importi
                                    oAtto.IMPORTO_ALTRO = 0
                                    oAtto.IMPORTO_ARROTONDAMENTO = 0
                                    oAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO = 0
                                    oAtto.IMPORTO_INTERESSI = 0
                                    oAtto.IMPORTO_SANZIONI = 0
                                    oAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI = 0
                                    oAtto.IMPORTO_SANZIONI_RIDOTTO = 0
                                    oAtto.IMPORTO_SPESE = 0
                                    oAtto.IMPORTO_TOTALE = 0
                                    oAtto.IMPORTO_TOTALE_RIDOTTO = 0
                                End If
                            End If
                            '*** ***
                            '****** DETERMINO TIPO DI AVVISO ******

                            If objHashTable.Contains("TIPOPROVVEDIMENTO") = False Then
                                objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
                            Else
                                objHashTable("TIPOPROVVEDIMENTO") = TipoAvviso
                            End If
                            Log.Debug(sIdEnte & " - ImportoTotaleAvviso:" & (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO).ToString & " €")
                            Log.Debug(sIdEnte & " - TipoAvviso:" & TipoAvviso & " - " & sDescTipoAvviso)
                            If TipoAvviso <> OggettoAtto.Provvedimento.NoAvviso Then
                                OggettoRiepilogoAccertamento = objCOMUpdateDBAccertamenti.updateDBAccertamentiOSAP(ConstSession.DBType, sIdEnte, oAtto.COD_CONTRIBUENTE, objHashTable, oCalcoloSanzioniInteressi, dsSanzioni, dsSanzioniImpDicVSImpPag, dsSanzioniScadDicVSDataPag, dsInteressi, dsInteressiImpDicVSImpPag, dsInteressiScadDicVSDataPag, oAtto, ListDettaglioAtto, oDichiarato, oAccertato, spese, ConstSession.UserName)

                                'recupero id Provvedimento
                                lngNewID_PROVVEDIMENTO = OggettoRiepilogoAccertamento(0).IdProvvedimento
                                'e lo assegno anche ai singoli immobili
                                For i = 0 To oAccertato.Length - 1
                                    oAccertato(i).IdProvvedimento = lngNewID_PROVVEDIMENTO
                                Next
                            End If
                            dsSanzioni.Dispose() : dsSanzioniImpDicVSImpPag.Dispose() : dsSanzioniScadDicVSDataPag.Dispose()
                            dsInteressi.Dispose() : dsInteressiImpDicVSImpPag.Dispose() : dsInteressiScadDicVSDataPag.Dispose()

                            HttpContext.Current.Session.Add("oSituazioneAtto", oAtto)
                            HttpContext.Current.Session.Add("oSituazioneDichiarato", oDichiarato)
                            HttpContext.Current.Session.Add("oSituazioneAccertato", oAccertato)
                            If (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO) = 0 Then
                                'Non emetto Avviso
                                sScript += "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
                                sScript += "parent.document.getElementById('attesaCarica').style.display='none';"

                                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                                    'Atto di autotutela di annullamento
                                    sScript += "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
                                    sScript += "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
                                    Log.Debug(sIdEnte & " - La posizione è corretta. Ho elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO")
                                Else
                                    'Nessun avviso emesso
                                    'Effettuo il rientro dell'accertato
                                    sScript += "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
                                End If
                            ElseIf (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO) < soglia Then
                                sScript += "parent.document.getElementById('attesaCarica').style.display='none';"
                                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                                    'Atto di autotutela di annullamento. Importo inferiore alla soglia
                                    sScript += "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
                                    sScript += "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
                                    Log.Debug(sIdEnte & " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
                                Else
                                    'Effettuo il rientro dell'accertato
                                    'Nessun avviso emesso. Importo inferiore alla soglia
                                    sScript += "GestAlert('a', 'warning', '', '', 'Importo Avviso inferiore alla soglia.');"
                                    Log.Debug(sIdEnte & " - Nessun avviso emesso. Importo inferiore alla soglia")
                                    sScript += "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
                                End If
                            Else
                                '******
                                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                                    sScript += "FineElaborazioneAccertamento();" & vbCrLf
                                    sScript += "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
                                Else
                                    sScript += "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
                                End If
                                Log.Debug(sIdEnte & " - registro script per visualizzare il riepilogo accertamento")
                            End If
                        End If

                    Else
                        Return Nothing
                    End If
                ElseIf ImpTotAccert < ImpTotDich Then
                    'accertato minore dichiarato Non emetto Avviso Non esiste rimborso
                    sScript += "GestAlert('a', 'warning', '', '', 'Importo Accertato inferiore all\' Importo Dichiarato. L\'avviso non verrà emesso');"
                End If
            Else
                'manca accertato
                Return Nothing
            End If
            Log.Debug(sIdEnte & " - Fine")
            Return oAtto
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.OSAPConfrontoAccertatoDichiarato.errore: ", ex)
            Return Nothing
        End Try
    End Function
    'Public Function OSAPConfrontoAccertatoDichiarato(ByVal sIdEnte As String, ByVal IdContribuente As Integer, ByVal sAnnoRiferimento As String, ByVal oDichiarato() As OSAPAccertamentoArticolo, ByVal oAccertato() As OSAPAccertamentoArticolo, ByVal blnTIPO_OPERAZIONE_RETTIFICA As Boolean, ByVal objHashTable As Hashtable, ByVal objDSMotivazioniSanzioni As DataSet, sDataMorte As String, ByRef sDescTipoAvviso As String, ByRef sScript As String) As OggettoAttoOSAP
    '    Dim x, y, nList, i As Integer
    '    Dim nLegamePrec As Integer = -1
    '    Dim ListDettaglioAtto() As OggettoDettaglioAtto
    '    Dim myDettaglioAtto As OggettoDettaglioAtto

    '    Dim ImpTotAccert As Double = 0
    '    Dim ImpTotDich As Double = 0
    '    Dim oAtto As New OggettoAttoOSAP
    '    Dim oPagatoOSAP() As OggettoPagamenti
    '    Dim FncPagato As New OPENgovTIA.ClsGestPag

    '    Dim OggettoRiepilogoAccertamento() As OSAPAccertamentoArticolo

    '    Dim lngNewID_PROVVEDIMENTO As Long
    '    Dim DATA_RETTIFICA_ANNULLAMENTO As String

    '    Dim blnResult As Boolean = False
    '    Dim objCOMUpdateDBAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '    Dim dsCalcoloSanzioniInteressi, dsSanzioni, dsSanzioniImpDicVSImpPag, dsSanzioniScadDicVSDataPag, dsInteressi, dsInteressiImpDicVSImpPag, dsInteressiScadDicVSDataPag, dsInteressiTMP, dsSanzioniTMP As DataSet
    '    Dim ImportoTotaleAvviso, ImportoTotAcc, ImportoTotPREAcc As Double

    '    Try
    '        ImportoTotaleAvviso = 0 : ImportoTotAcc = 0 : ImportoTotPREAcc = 0
    '        dsCalcoloSanzioniInteressi = New DataSet : dsSanzioni = New DataSet : dsSanzioniImpDicVSImpPag = New DataSet : dsSanzioniScadDicVSDataPag = New DataSet : dsInteressi = New DataSet : dsInteressiImpDicVSImpPag = New DataSet : dsInteressiScadDicVSDataPag = New DataSet : dsInteressiTMP = New DataSet : dsSanzioniTMP = New DataSet

    '        If Not oAccertato Is Nothing Then
    '            'Determino il Tipo di Avviso (Accertamento o Ufficio) Se trovo 1 immobile di accertato che non è in dichiarato ho avviso di Tipo = 4 altrimenti avviso di tipo 3
    '            Dim DichiaratoNothing As New OSAPAccertamentoArticolo
    '            If oDichiarato Is Nothing Then
    '                ReDim Preserve oDichiarato(0)
    '                oDichiarato(0) = DichiaratoNothing
    '            End If
    '            Dim TipoAvviso As Integer

    '            TipoAvviso = OggettoAtto.Provvedimento.AccertamentoRettifica                '4

    '            'Prendo immobili di accertato
    '            For i = 0 To oAccertato.Length - 1
    '                Dim Trovato As Boolean = False
    '                'Cerco l'immobili in tutti gli immobili di dichiarato Se lo trovo esco dal ciclo e proseguo a cercare con immobili successivo di accertamento
    '                For y = 0 To oDichiarato.Length - 1
    '                    If oAccertato(i).IdLegame = oDichiarato(y).IdLegame Then
    '                        Trovato = True
    '                        Exit For
    '                    End If
    '                Next

    '                'Se trovato = False vuol dire che nn ho trovato l'immobile
    '                If Trovato = False Then
    '                    'Avviso D'ufficio
    '                    TipoAvviso = OggettoAtto.Provvedimento.AccertamentoUfficio                         '3
    '                    Exit For
    '                End If
    '            Next
    '            nList = -1
    '            'CONFRONTO ACCERTATO E DICHIARATO ciclo su tutti i record accertati
    '            For x = 0 To oAccertato.GetUpperBound(0)
    '                'sommo gli importi a parità di livello di legame
    '                If oAccertato(x).IdLegame <> nLegamePrec Then
    '                    'aggiungo un record
    '                    nList += 1
    '                    myDettaglioAtto = New OggettoDettaglioAtto
    '                    myDettaglioAtto.IdLegame = oAccertato(x).IdLegame
    '                    myDettaglioAtto.Progressivo = oAccertato(x).Progressivo
    '                    myDettaglioAtto.Sanzioni = oAccertato(x).Sanzioni
    '                    myDettaglioAtto.Interessi = oAccertato(x).Interessi
    '                    myDettaglioAtto.Calcola_Interessi = oAccertato(x).Calcola_Interessi
    '                End If
    '                'sommo l'importo
    '                myDettaglioAtto.ImpAccertato += oAccertato(x).Calcolo.ImportoCalcolato

    '                ReDim Preserve ListDettaglioAtto(nList)
    '                ListDettaglioAtto(nList) = myDettaglioAtto
    '            Next

    '            'ciclo su tutti i record dichiarati
    '            For x = 0 To oDichiarato.GetUpperBound(0)
    '                'cerco il corrispettivo legame nell'oggetto di confronto
    '                For y = 0 To ListDettaglioAtto.GetUpperBound(0)
    '                    If ListDettaglioAtto(y).IdLegame = oDichiarato(x).IdLegame Then
    '                        ListDettaglioAtto(y).ImpDichiarato += oDichiarato(x).Calcolo.ImportoCalcolato
    '                        Exit For
    '                    End If
    '                Next
    '            Next

    '            'ciclo sull'oggetto totale per verificare il tipo di atto
    '            nList = -1
    '            For x = 0 To ListDettaglioAtto.GetUpperBound(0)
    '                ImpTotAccert += ListDettaglioAtto(x).ImpAccertato
    '                ImpTotDich += ListDettaglioAtto(x).ImpDichiarato
    '            Next
    '            nList += 1
    '            Log.Debug("totale dichiarato=" & ImpTotDich.ToString)
    '            Log.Debug("totale acccertato=" & ImpTotAccert.ToString)
    '            If ImpTotAccert >= ImpTotDich Then
    '                'accertato maggiore dichiarato
    '                oAtto = OSAPPopolaAtto(sIdEnte, ListDettaglioAtto, ImpTotDich, ImpTotAccert)
    '                oAtto.ANNO = oAccertato(0).Anno
    '                If Not oAtto Is Nothing Then
    '                    'SE HO UN ATTO CONFRONTO ACCERTATO E PAGATO
    '                    Dim SearchParamsPag As New OPENgovTIA.ObjSearchPagamenti
    '                    SearchParamsPag.sEnte = sIdEnte
    '                    SearchParamsPag.sAnnoRif = oAtto.ANNO
    '                    SearchParamsPag.IdContribuente = IdContribuente
    '                    SearchParamsPag.IdTributo = Utility.Costanti.TRIBUTO_OSAP
    '                    oPagatoOSAP = FncPagato.GetListPagamenti(SearchParamsPag, ConstSession.StringConnectionOSAP)
    '                    If Not oPagatoOSAP Is Nothing Then
    '                        Dim IPagato As Integer
    '                        For IPagato = 0 To oPagatoOSAP.Length - 1
    '                            oAtto.IMPORTO_PAGATO += oPagatoOSAP(IPagato).dImportoPagamento
    '                        Next
    '                    Else
    '                        oAtto.IMPORTO_PAGATO = 0
    '                    End If
    '                    oAtto.IMPORTO_VERSATO_F2 = oAtto.IMPORTO_PAGATO
    '                    'la differenza di imposta dell'atto è data da differenza imposta accertamento+differenza imposta preaccertamento
    '                    '*** 20170330 - tolto pagato per gestire correttamente la sazione su TARDIVO VERSAMENTO ***
    '                    Dim TipoAvvisoRimborso As Integer = -1

    '                    'Rimborso. Calcolo gli interessi Attivi sul singolo immobile. Al giro dopo la var viene azzerrata a -1.
    '                    If oAtto.IMPORTO_DIFFERENZA_IMPOSTA < 0 Then
    '                        TipoAvvisoRimborso = OggettoAtto.Provvedimento.Rimborso                            '"5"
    '                    End If

    '                    If objHashTable.ContainsKey("CODCONTRIBUENTE") = False Then
    '                        objHashTable.Add("CODCONTRIBUENTE", oAtto.COD_CONTRIBUENTE)
    '                    End If
    '                    'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '                    If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = True Then
    '                        objHashTable.Remove("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI")
    '                    End If
    '                    objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

    '                    objHashTable("IDSOTTOAPPLICAZIONEANAGRAFICA") = ConfigurationManager.AppSettings("OPENGOVTOCO")
    '                    objHashTable("CONNECTIONSTRINGANAGRAFICA") = ConstSession.StringConnectionAnagrafica
    '                    objHashTable("CODTRIBUTO") = Utility.Costanti.TRIBUTO_OSAP
    '                    objHashTable("CODENTE") = sIdEnte
    '                    If objHashTable.ContainsKey("TIPOPROVVEDIMENTO") = True Then
    '                        objHashTable.Remove("TIPOPROVVEDIMENTO")
    '                    End If
    '                    'TipoAvvisoRimborso = -1 == False ----> Non è un rimborso
    '                    'Devo effettuare il calcolo guardando se è un rimborso sull'immobile oppure no
    '                    'Se è rimborso ho solo interssi attivi
    '                    If TipoAvvisoRimborso = -1 Then
    '                        objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
    '                    Else
    '                        objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvvisoRimborso)
    '                    End If
    '                    If objHashTable.ContainsKey("COD_TIPO_PROCEDIMENTO") = True Then
    '                        objHashTable("COD_TIPO_PROCEDIMENTO") = OggettoAtto.Procedimento.Accertamento
    '                    Else
    '                        objHashTable.Add("COD_TIPO_PROCEDIMENTO", OggettoAtto.Procedimento.Accertamento)
    '                    End If
    '                    If objHashTable.Contains("ANNOACCERTAMENTO") = False Then
    '                        objHashTable.Add("ANNOACCERTAMENTO", sAnnoRiferimento)
    '                    End If
    '                    'PER CONNESSIONE OSAP
    '                    objHashTable("IDSOTTOAPPLICAZIONEOSAP") = ConfigurationManager.AppSettings("OPENGOVTA")
    '                    objHashTable("CONNECTIONSTRINGOSAP") = ConstSession.StringConnectionOSAP

    '                    '*** 20171005 - per la NOFASE la differenza di imposta dell'atto è data da accertato-dichiarato ***
    '                    If OSAPCalcoloSanzioniInteressi(-1, oAccertato, ListDettaglioAtto, objHashTable, -1, sDataMorte, dsCalcoloSanzioniInteressi, dsInteressi, dsSanzioni, oAtto, ImportoTotaleAvviso, -1, "") = False Then
    '                        Throw New Exception("Errore in calcolo Sanzioni e/o Interessi")
    '                    End If
    '                    '*** 20171005 - devo registrare l'importo Accertamento ***
    '                    ImportoTotAcc = ImportoTotaleAvviso
    '                    '*** ***
    '                    'per queste fasi mi serve la percentuale di scorporo del pagato vs il dovuto
    '                    Dim PercentualeScorporo As Double = oAtto.IMPORTO_PAGATO / oAtto.IMPORTO_DICHIARATO_F2
    '                    If PercentualeScorporo >= 0 Then
    '                        PercentualeScorporo = PercentualeScorporo
    '                    Else
    '                        PercentualeScorporo = 0
    '                    End If
    '                    '*** calcolo sanzioni e/o interessi confronto importo dichiarato vs importo pagato ***
    '                    'se hanno selezionato sanzione 26 - omesso/parziale versamento
    '                    Dim bDicVSPag As Boolean = False
    '                    For Each myDetAtto As OggettoDettaglioAtto In ListDettaglioAtto
    '                        Dim listSanz() As String
    '                        Dim myString As String
    '                        For Each mySanz As String In myDetAtto.Sanzioni.Split(",")
    '                            listSanz = mySanz.Split("#")
    '                            For Each myString In listSanz
    '                                If myString = "26" Then ' codice voce FISSO per omesso/parziale versamento
    '                                    bDicVSPag = True
    '                                    Exit For
    '                                End If
    '                            Next
    '                        Next
    '                    Next
    '                    Dim myRow As DataRow
    '                    If bDicVSPag = True Then
    '                        oAtto.IMPORTO_DIFFERENZA_IMPOSTA = (ImpTotDich - oAtto.IMPORTO_PAGATO)
    '                        '*** ***
    '                        If OSAPCalcoloSanzioniInteressi(-1, oAccertato, ListDettaglioAtto, objHashTable, PercentualeScorporo, sDataMorte, dsCalcoloSanzioniInteressi, dsInteressiTMP, dsSanzioniTMP, oAtto, ImportoTotaleAvviso, OggettoAtto.Fase.VersatoDichiarato, "26") = False Then
    '                            Throw New Exception("Errore in calcolo Sanzioni e/o Interessi")
    '                        End If
    '                        ImportoTotPREAcc += ImportoTotaleAvviso
    '                        If dsInteressiImpDicVSImpPag.Tables.Count = 0 Then
    '                            dsInteressiImpDicVSImpPag.Tables.Add("A")
    '                            dsInteressiImpDicVSImpPag.Tables(0).Merge(dsInteressiTMP.Tables(0))
    '                        Else
    '                            For Each myRow In dsInteressiTMP.Tables(0).Rows
    '                                dsInteressiImpDicVSImpPag.Tables(0).ImportRow(myRow)
    '                            Next
    '                        End If
    '                        If dsSanzioniImpDicVSImpPag.Tables.Count = 0 Then
    '                            dsSanzioniImpDicVSImpPag.Tables.Add("A")
    '                            dsSanzioniImpDicVSImpPag.Tables(0).Merge(dsSanzioniTMP.Tables(0))
    '                        Else
    '                            For Each myRow In dsSanzioniTMP.Tables(0).Rows
    '                                dsSanzioniImpDicVSImpPag.Tables(0).ImportRow(myRow)
    '                            Next
    '                        End If
    '                    End If
    '                    '*** ***
    '                    '*** 20171005 - per la FASE2 la differenza di imposta dell'atto è data da dichiarato-versato ***
    '                    oAtto.IMPORTO_DIFFERENZA_IMPOSTA = (ImpTotDich - oAtto.IMPORTO_PAGATO)
    '                    '*** ***
    '                    If OSAPCalcoloSanzioniInteressi(OggettoAtto.Fase.VersatoDichiarato, oAccertato, ListDettaglioAtto, objHashTable, PercentualeScorporo, sDataMorte, dsCalcoloSanzioniInteressi, dsInteressiTMP, dsSanzioniTMP, oAtto, ImportoTotaleAvviso, OggettoAtto.Fase.VersatoDichiarato, "") = False Then
    '                        Throw New Exception("Errore in calcolo Sanzioni e/o Interessi")
    '                    End If
    '                    '*** 20171005 - devo sommare l'importo FASE1+FASE2 come PREAccertamento ***
    '                    ImportoTotPREAcc += ImportoTotaleAvviso
    '                    If dsInteressiImpDicVSImpPag.Tables.Count = 0 Then
    '                        dsInteressiImpDicVSImpPag.Tables.Add("A")
    '                        dsInteressiImpDicVSImpPag.Tables(0).Merge(dsInteressiTMP.Tables(0))
    '                    Else
    '                        For Each myRow In dsInteressiTMP.Tables(0).Rows
    '                            dsInteressiImpDicVSImpPag.Tables(0).ImportRow(myRow)
    '                        Next
    '                    End If
    '                    If dsSanzioniImpDicVSImpPag.Tables.Count = 0 Then
    '                        dsSanzioniImpDicVSImpPag.Tables.Add("A")
    '                        dsSanzioniImpDicVSImpPag.Tables(0).Merge(dsSanzioniTMP.Tables(0))
    '                    Else
    '                        For Each myRow In dsSanzioniTMP.Tables(0).Rows
    '                            dsSanzioniImpDicVSImpPag.Tables(0).ImportRow(myRow)
    '                        Next
    '                    End If
    '                    '*** ***
    '                    '*** calcolo sanzioni e/o interessi confronto data scadenza vs data pagamento ***
    '                    'se hanno selezionato sanzione 25 - tardivo versamento
    '                    bDicVSPag = False
    '                    For Each myDetAtto As OggettoDettaglioAtto In ListDettaglioAtto
    '                        Dim listSanz() As String
    '                        Dim myString As String
    '                        For Each mySanz As String In myDetAtto.Sanzioni.Split(",")
    '                            listSanz = mySanz.Split("#")
    '                            For Each myString In listSanz
    '                                If myString = "25" Then ' codice voce FISSO per tardivo versamento
    '                                    bDicVSPag = True
    '                                    Exit For
    '                                End If
    '                            Next
    '                        Next
    '                    Next
    '                    If bDicVSPag = True Then
    '                        oAtto.IMPORTO_DIFFERENZA_IMPOSTA = oAtto.IMPORTO_PAGATO
    '                        '*** ***
    '                        If OSAPCalcoloSanzioniInteressi(-1, oAccertato, ListDettaglioAtto, objHashTable, PercentualeScorporo, sDataMorte, dsCalcoloSanzioniInteressi, dsInteressiTMP, dsSanzioniTMP, oAtto, ImportoTotaleAvviso, OggettoAtto.Fase.VersamentiTardivi, "25") = False Then
    '                            Throw New Exception("Errore in calcolo Sanzioni e/o Interessi")
    '                        End If
    '                        ImportoTotPREAcc += ImportoTotaleAvviso
    '                        If dsInteressiScadDicVSDataPag.Tables.Count = 0 Then
    '                            dsInteressiScadDicVSDataPag.Tables.Add("A")
    '                            dsInteressiScadDicVSDataPag.Tables(0).Merge(dsInteressiTMP.Tables(0))
    '                        Else
    '                            For Each myRow In dsInteressiTMP.Tables(0).Rows
    '                                dsInteressiScadDicVSDataPag.Tables(0).ImportRow(myRow)
    '                            Next
    '                        End If
    '                        If dsSanzioniScadDicVSDataPag.Tables.Count = 0 Then
    '                            dsSanzioniScadDicVSDataPag.Tables.Add("A")
    '                            dsSanzioniScadDicVSDataPag.Tables(0).Merge(dsSanzioniTMP.Tables(0))
    '                        Else
    '                            For Each myRow In dsSanzioniTMP.Tables(0).Rows
    '                                dsSanzioniScadDicVSDataPag.Tables(0).ImportRow(myRow)
    '                            Next
    '                        End If
    '                    End If
    '                    '*** 20171005 - per la FASE1 la differenza di imposta dell'atto è data dal pagato ***
    '                    oAtto.IMPORTO_DIFFERENZA_IMPOSTA = oAtto.IMPORTO_PAGATO
    '                    '*** ***
    '                    If OSAPCalcoloSanzioniInteressi(OggettoAtto.Fase.VersamentiTardivi, oAccertato, ListDettaglioAtto, objHashTable, PercentualeScorporo, sDataMorte, dsCalcoloSanzioniInteressi, dsInteressiTMP, dsSanzioniTMP, oAtto, ImportoTotaleAvviso, OggettoAtto.Fase.VersamentiTardivi, "") = False Then
    '                        Throw New Exception("Errore in calcolo Sanzioni e/o Interessi")
    '                    End If
    '                    '*** 20171005 - devo sommare l'importo FASE1+FASE2 come PREAccertamento ***
    '                    ImportoTotPREAcc += ImportoTotaleAvviso
    '                    If dsInteressiScadDicVSDataPag.Tables.Count = 0 Then
    '                        dsInteressiScadDicVSDataPag.Tables.Add("A")
    '                        dsInteressiScadDicVSDataPag.Tables(0).Merge(dsInteressiTMP.Tables(0))
    '                    Else
    '                        For Each myRow In dsInteressiTMP.Tables(0).Rows
    '                            dsInteressiScadDicVSDataPag.Tables(0).ImportRow(myRow)
    '                        Next
    '                    End If
    '                    If dsSanzioniScadDicVSDataPag.Tables.Count = 0 Then
    '                        dsSanzioniScadDicVSDataPag.Tables.Add("A")
    '                        dsSanzioniScadDicVSDataPag.Tables(0).Merge(dsSanzioniTMP.Tables(0))
    '                    Else
    '                        For Each myRow In dsSanzioniTMP.Tables(0).Rows
    '                            dsSanzioniScadDicVSDataPag.Tables(0).ImportRow(myRow)
    '                        Next
    '                    End If
    '                    '*** ***
    '                    '*** 20171005 - la differenza di imposta dell'atto è data da differenza imposta accertamento+differenza imposta preaccertamento ***
    '                    oAtto.IMPORTO_DIFFERENZA_IMPOSTA = (ImpTotAccert - ImpTotDich) + (ImpTotDich - oAtto.IMPORTO_PAGATO)
    '                    '*** ***
    '                    Log.Debug(sIdEnte & " - ImportoTotaleAvviso:" & ImportoTotaleAvviso)
    '                    If (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO) < 0 Then 'If ImportoTotaleAvviso < 0 Then
    '                        'sarebbe un rimborso Non emetto Avviso
    '                        Log.Debug(sIdEnte & " - Importo Avviso inferiore a zero. L'avviso non verrà emesso")
    '                        TipoAvviso = OggettoAtto.Provvedimento.Rimborso
    '                        sScript += "GestAlert('a', 'warning', '', '', 'Importo Avviso inferiore a zero. L\'avviso non verrà emesso');"
    '                    Else
    '                        'reperisco soglia
    '                        Dim soglia As Double
    '                        Dim objProvvedimentiDB As New DBPROVVEDIMENTI.ProvvedimentiDB
    '                        soglia = 0
    '                        soglia = objProvvedimentiDB.GetSogliaMinima(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_OSAP, objHashTable("CODENTE"), TipoAvviso)
    '                        Log.Debug(sIdEnte & " - Soglia:" & soglia & " €")
    '                        'Calcolo le spese
    '                        Dim spese As Double
    '                        spese = objProvvedimentiDB.GetSpese(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_OSAP, objHashTable("CODENTE"), TipoAvviso)
    '                        Log.Debug(sIdEnte & " - Spese:" & spese & " €")
    '                        ' Aggiorno il DB dopo procedura di accertamento
    '                        oAtto.CAP_CO = ""
    '                        oAtto.CAP_RES = ""
    '                        oAtto.CITTA_CO = ""
    '                        oAtto.CITTA_RES = ""
    '                        oAtto.CIVICO_CO = ""
    '                        oAtto.CIVICO_RES = ""
    '                        oAtto.CO = ""
    '                        oAtto.CODICE_FISCALE = ""
    '                        oAtto.COGNOME = ""
    '                        oAtto.ESPONENTE_CIVICO_CO = ""
    '                        oAtto.ESPONENTE_CIVICO_RES = ""
    '                        oAtto.FRAZIONE_CO = ""
    '                        oAtto.FRAZIONE_RES = ""
    '                        oAtto.NOME = ""
    '                        oAtto.PARTITA_IVA = ""
    '                        oAtto.POSIZIONE_CIVICO_CO = ""
    '                        oAtto.POSIZIONE_CIVICO_RES = ""
    '                        oAtto.PROVINCIA_CO = ""
    '                        oAtto.PROVINCIA_RES = ""
    '                        oAtto.VIA_CO = ""
    '                        oAtto.VIA_RES = ""
    '                        oAtto.COD_CONTRIBUENTE = CInt(objHashTable("CODCONTRIBUENTE"))
    '                        oAtto.IMPORTO_SPESE = spese
    '                        oAtto.DATA_ELABORAZIONE = DateTime.Now.ToString("yyyyMMdd")
    '                        'aggiorno l'importo totale comprensivo di arrotondamento
    '                        '*** 20111205 - le spese devono essere messe dopo l'arrotondamento ***
    '                        oAtto.IMPORTO_TOTALE = (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO)
    '                        oAtto.IMPORTO_ARROTONDAMENTO = CDbl(ImportoArrotondato(oAtto.IMPORTO_TOTALE)) - oAtto.IMPORTO_TOTALE
    '                        oAtto.IMPORTO_TOTALE = oAtto.IMPORTO_TOTALE + oAtto.IMPORTO_ARROTONDAMENTO + oAtto.IMPORTO_SPESE
    '                        oAtto.IMPORTO_TOTALE_RIDOTTO = oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI_RIDOTTO + oAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO
    '                        oAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO = CDbl(ImportoArrotondato(oAtto.IMPORTO_TOTALE_RIDOTTO)) - oAtto.IMPORTO_TOTALE_RIDOTTO
    '                        oAtto.IMPORTO_TOTALE_RIDOTTO = oAtto.IMPORTO_TOTALE_RIDOTTO + oAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO + oAtto.IMPORTO_SPESE
    '                        For i = 0 To oAccertato.Length - 1
    '                            If Year(oAccertato(i).DataInizioOccupazione) < oAtto.ANNO Then
    '                                oAccertato(i).DataInizioOccupazione = "01/01/" & oAtto.ANNO
    '                            End If
    '                        Next
    '                        For i = 0 To oDichiarato.Length - 1
    '                            If Year(oDichiarato(i).DataInizioOccupazione) < oAtto.ANNO Then
    '                                oDichiarato(i).DataInizioOccupazione = "01/01/" & oAtto.ANNO
    '                            End If
    '                        Next

    '                        If objHashTable.ContainsKey("DATA_ANNULLAMENTO") Then objHashTable.Remove("DATA_ANNULLAMENTO")
    '                        If objHashTable.ContainsKey("DATA_RETTIFICA") Then objHashTable.Remove("DATA_RETTIFICA")

    '                        If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                            DATA_RETTIFICA_ANNULLAMENTO = DateTime.Now.ToString("yyyyMMdd")
    '                            If (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO) < soglia Then 'If ImportoTotaleAvviso < soglia Then
    '                                objHashTable.Add("DATA_ANNULLAMENTO", DATA_RETTIFICA_ANNULLAMENTO)
    '                                objHashTable.Add("DATA_RETTIFICA", "")
    '                            Else
    '                                objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA_ANNULLAMENTO)
    '                                objHashTable.Add("DATA_ANNULLAMENTO", "")
    '                            End If
    '                        End If

    '                        '****** DETERMINO TIPO DI AVVISO ******
    '                        Log.Debug(sIdEnte & " - Richiamo la funzione per determinare il tipo di avviso")

    '                        '*** 20171005 - devo passare TotAcc e TotPREAcc ***
    '                        If DeterminaTipoAvviso(ImportoTotAcc, ImportoTotPREAcc, soglia, blnTIPO_OPERAZIONE_RETTIFICA, 0, TipoAvviso, sDescTipoAvviso) = False Then
    '                            Throw New Exception
    '                        End If
    '                        If TipoAvviso = OggettoAtto.Provvedimento.AutotutelaAnnullamento Then
    '                            If Not IsNothing(oAtto) Then
    '                                'devo azzerare tutti gli importi
    '                                oAtto.IMPORTO_ALTRO = 0
    '                                oAtto.IMPORTO_ARROTONDAMENTO = 0
    '                                oAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO = 0
    '                                oAtto.IMPORTO_INTERESSI = 0
    '                                oAtto.IMPORTO_SANZIONI = 0
    '                                oAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI = 0
    '                                oAtto.IMPORTO_SANZIONI_RIDOTTO = 0
    '                                oAtto.IMPORTO_SPESE = 0
    '                                oAtto.IMPORTO_TOTALE = 0
    '                                oAtto.IMPORTO_TOTALE_RIDOTTO = 0
    '                            End If
    '                        End If
    '                        '*** ***
    '                        '****** DETERMINO TIPO DI AVVISO ******

    '                        If objHashTable.Contains("TIPOPROVVEDIMENTO") = False Then
    '                            objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
    '                        Else
    '                            objHashTable("TIPOPROVVEDIMENTO") = TipoAvviso
    '                        End If
    '                        Log.Debug(sIdEnte & " - ImportoTotaleAvviso:" & (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO).ToString & " €")
    '                        Log.Debug(sIdEnte & " - TipoAvviso:" & TipoAvviso & " - " & sDescTipoAvviso)
    '                        If TipoAvviso <> OggettoAtto.Provvedimento.NoAvviso Then
    '                            OggettoRiepilogoAccertamento = objCOMUpdateDBAccertamenti.updateDBAccertamentiOSAP(ConstSession.DBType, objHashTable, dsCalcoloSanzioniInteressi, dsSanzioni, dsSanzioniImpDicVSImpPag, dsSanzioniScadDicVSDataPag, dsInteressi, dsInteressiImpDicVSImpPag, dsInteressiScadDicVSDataPag, oAtto, ListDettaglioAtto, oDichiarato, oAccertato, spese, ConstSession.UserName)

    '                            'recupero id Provvedimento
    '                            lngNewID_PROVVEDIMENTO = OggettoRiepilogoAccertamento(0).IdProvvedimento
    '                            'e lo assegno anche ai singoli immobili
    '                            For i = 0 To oAccertato.Length - 1
    '                                oAccertato(i).IdProvvedimento = lngNewID_PROVVEDIMENTO
    '                            Next
    '                        End If
    '                        dsSanzioni.Dispose() : dsSanzioniImpDicVSImpPag.Dispose() : dsSanzioniScadDicVSDataPag.Dispose()
    '                        dsInteressi.Dispose() : dsInteressiImpDicVSImpPag.Dispose() : dsInteressiScadDicVSDataPag.Dispose()

    '                        HttpContext.Current.Session.Add("oSituazioneAtto", oAtto)
    '                        HttpContext.Current.Session.Add("oSituazioneDichiarato", oDichiarato)
    '                        HttpContext.Current.Session.Add("oSituazioneAccertato", oAccertato)
    '                        If (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO) = 0 Then
    '                            'Non emetto Avviso
    '                            sScript += "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
    '                            sScript += "parent.document.getElementById('attesaCarica').style.display='none';"

    '                            If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                                'Atto di autotutela di annullamento
    '                                sScript += "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
    '                                sScript += "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                                Log.Debug(sIdEnte & " - La posizione è corretta. Ho elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO")
    '                            Else
    '                                'Nessun avviso emesso
    '                                'Effettuo il rientro dell'accertato
    '                                sScript += "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                            End If
    '                        ElseIf (oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI + oAtto.IMPORTO_ALTRO) < soglia Then
    '                            sScript += "parent.document.getElementById('attesaCarica').style.display='none';"
    '                            If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                                'Atto di autotutela di annullamento. Importo inferiore alla soglia
    '                                sScript += "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
    '                                sScript += "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                                Log.Debug(sIdEnte & " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
    '                            Else
    '                                'Effettuo il rientro dell'accertato
    '                                'Nessun avviso emesso. Importo inferiore alla soglia
    '                                sScript += "GestAlert('a', 'warning', '', '', 'Importo Avviso inferiore alla soglia.');"
    '                                Log.Debug(sIdEnte & " - Nessun avviso emesso. Importo inferiore alla soglia")
    '                                sScript += "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                            End If
    '                        Else
    '                            '******
    '                            If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                                sScript += "FineElaborazioneAccertamento();" & vbCrLf
    '                                sScript += "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                            Else
    '                                sScript += "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                            End If
    '                            Log.Debug(sIdEnte & " - registro script per visualizzare il riepilogo accertamento")
    '                        End If
    '                    End If

    '                Else
    '                    Return Nothing
    '                End If
    '            ElseIf ImpTotAccert < ImpTotDich Then
    '                'accertato minore dichiarato Non emetto Avviso Non esiste rimborso
    '                sScript += "GestAlert('a', 'warning', '', '', 'Importo Accertato inferiore all\' Importo Dichiarato. L\'avviso non verrà emesso');"
    '            End If
    '        Else
    '            'manca accertato
    '            Return Nothing
    '        End If
    '        Log.Debug(sIdEnte & " - Fine")
    '        Return oAtto
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.OSAPConfrontoAccertatoDichiarato.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="oDettaglioAtto"></param>
    ''' <param name="ImpTotDichiarato"></param>
    ''' <param name="ImpTotAccertato"></param>
    ''' <returns></returns>
    Public Function OSAPPopolaAtto(ByVal sIdEnte As String, ByVal oDettaglioAtto() As OggettoDettaglioAtto, ByVal ImpTotDichiarato As Double, ByVal ImpTotAccertato As Double) As OggettoAttoOSAP
        'Dim oAttoOSAP As New MotoreOSAP.Oggetti.OggettoAttoOSAP
        Dim oAttoOSAP As New OggettoAttoOSAP
        Dim x As Integer
        Try
            For x = 0 To oDettaglioAtto.Length - 1
                oAttoOSAP.IMPORTO_DIFFERENZA_IMPOSTA += oDettaglioAtto(x).ImpAccertato - oDettaglioAtto(x).ImpDichiarato
            Next

            oAttoOSAP.COD_CONTRIBUENTE = oDettaglioAtto(0).IdContribuente
            oAttoOSAP.COD_ENTE = sIdEnte
            oAttoOSAP.COD_TRIBUTO = Utility.Costanti.TRIBUTO_OSAP
            oAttoOSAP.ANNO = oDettaglioAtto(0).Anno
            oAttoOSAP.IMPORTO_DICHIARATO_F2 = ImpTotDichiarato
            oAttoOSAP.IMPORTO_ACCERTATO_ACC = ImpTotAccertato
            oAttoOSAP.IMPORTO_DIFFERENZA_IMPOSTA_ACC = ImpTotAccertato - ImpTotDichiarato

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.OSAPPopolaAtto.errore: ", ex)
        End Try
        Return oAttoOSAP
    End Function

    Public Sub OSAPCastArtIntoProvArt(ByVal OSAPArt As IRemInterfaceOSAP.Articolo, ByRef PROVArt As OSAPAccertamentoArticolo, ByVal IdContribuente As Integer)
        Try
            PROVArt.Attrazione = OSAPArt.Attrazione
            PROVArt.Categoria = OSAPArt.Categoria
            PROVArt.Civico = OSAPArt.Civico
            PROVArt.CodVia = OSAPArt.CodVia
            PROVArt.Consistenza = OSAPArt.Consistenza
            PROVArt.DataCessazione = OSAPArt.DataCessazione
            PROVArt.DataFineOccupazione = OSAPArt.DataFineOccupazione
            PROVArt.DataInizioOccupazione = OSAPArt.DataInizioOccupazione
            PROVArt.DataInserimento = OSAPArt.DataInserimento
            PROVArt.DataVariazione = OSAPArt.DataVariazione
            PROVArt.DetrazioneImporto = OSAPArt.DetrazioneImporto
            PROVArt.Dichiarazione = OSAPArt.Dichiarazione
            PROVArt.DurataOccupazione = OSAPArt.DurataOccupazione
            PROVArt.ElencoPercAgevolazioni = OSAPArt.ElencoPercAgevolazioni
            PROVArt.Esponente = OSAPArt.Esponente
            PROVArt.IdArticolo = OSAPArt.IdArticolo
            PROVArt.IdArticoloPadre = OSAPArt.IdArticoloPadre
            PROVArt.IdDichiarazione = OSAPArt.IdDichiarazione
            PROVArt.IdTributo = OSAPArt.IdTributo
            PROVArt.Interno = OSAPArt.Interno
            PROVArt.ListAgevolazioni = OSAPArt.ListAgevolazioni
            PROVArt.MaggiorazioneImporto = OSAPArt.MaggiorazioneImporto
            PROVArt.MaggiorazionePerc = OSAPArt.MaggiorazionePerc
            PROVArt.Note = OSAPArt.Note
            PROVArt.Operatore = OSAPArt.Operatore
            PROVArt.Scala = OSAPArt.Scala
            PROVArt.SVia = OSAPArt.SVia
            PROVArt.TipoConsistenzaTOCO = OSAPArt.TipoConsistenzaTOCO
            PROVArt.TipoDurata = OSAPArt.TipoDurata
            PROVArt.TipologiaOccupazione = OSAPArt.TipologiaOccupazione
            PROVArt.IdLegame = 1

            Dim anagDAO As DAO.AnagraficheDAO = New DAO.AnagraficheDAO
            Dim MyAnag As DettaglioAnagrafica = anagDAO.GetAnagraficaContribuente(IdContribuente)

            Dim MyDic As New IRemInterfaceOSAP.DichiarazioneTosapCosap
            ReDim Preserve MyDic.ArticoliDichiarazione(0)
            MyDic.ArticoliDichiarazione(0) = OSAPArt

            MyDic.AnagraficaContribuente = MyAnag
            HttpContext.Current.Session("objDichiarazioneTosapCosap") = MyDic
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.OSAPCastArtIntoProvArt.errore: ", ex)
        End Try
    End Sub

    Public Sub OSAPCastProvArtIntoArt(ByVal PROVArt As OSAPAccertamentoArticolo, ByRef OSAPArt As IRemInterfaceOSAP.Articolo)
        Try
            OSAPArt.Attrazione = PROVArt.Attrazione
            OSAPArt.Categoria = PROVArt.Categoria
            OSAPArt.Civico = PROVArt.Civico
            OSAPArt.CodVia = PROVArt.CodVia
            OSAPArt.Consistenza = PROVArt.Consistenza
            OSAPArt.DataCessazione = PROVArt.DataCessazione
            OSAPArt.DataFineOccupazione = PROVArt.DataFineOccupazione
            OSAPArt.DataInizioOccupazione = PROVArt.DataInizioOccupazione
            OSAPArt.DataInserimento = PROVArt.DataInserimento
            OSAPArt.DataVariazione = PROVArt.DataVariazione
            OSAPArt.DetrazioneImporto = PROVArt.DetrazioneImporto
            OSAPArt.Dichiarazione = PROVArt.Dichiarazione
            OSAPArt.DurataOccupazione = PROVArt.DurataOccupazione
            OSAPArt.ElencoPercAgevolazioni = PROVArt.ElencoPercAgevolazioni
            OSAPArt.Esponente = PROVArt.Esponente
            OSAPArt.IdArticolo = PROVArt.IdArticolo
            OSAPArt.IdArticoloPadre = PROVArt.IdArticoloPadre
            OSAPArt.IdDichiarazione = PROVArt.IdDichiarazione
            OSAPArt.IdTributo = PROVArt.IdTributo
            OSAPArt.Interno = PROVArt.Interno
            OSAPArt.ListAgevolazioni = PROVArt.ListAgevolazioni
            OSAPArt.MaggiorazioneImporto = PROVArt.MaggiorazioneImporto
            OSAPArt.MaggiorazionePerc = PROVArt.MaggiorazionePerc
            OSAPArt.Note = PROVArt.Note
            OSAPArt.Operatore = PROVArt.Operatore
            OSAPArt.Scala = PROVArt.Scala
            OSAPArt.SVia = PROVArt.SVia
            OSAPArt.TipoConsistenzaTOCO = PROVArt.TipoConsistenzaTOCO
            OSAPArt.TipoDurata = PROVArt.TipoDurata
            OSAPArt.TipologiaOccupazione = PROVArt.TipologiaOccupazione
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.OSAPCastProvArtIntoArt.errore: ", ex)
        End Try
    End Sub

    Public Sub OSAPLoadParamCalcolo(ByVal Anno As Integer, ByRef ListCateg() As IRemInterfaceOSAP.Categorie, ByRef ListTipiOcc() As IRemInterfaceOSAP.TipologieOccupazioni, ByRef ListAgevolazioni() As IRemInterfaceOSAP.Agevolazione, ByRef ListTariffe() As IRemInterfaceOSAP.Tariffe, ByVal idEnte As String)
        Try
            HttpContext.Current.Session("StringConnection") = ConstSession.StringConnectionOSAP
            ListCateg = DTO.MetodiCategorie.GetCategorieForMotore(ConstSession.StringConnectionOSAP, Anno, idEnte, Utility.Costanti.TRIBUTO_OSAP)
            ListTipiOcc = DTO.MetodiTipologieOccupazioni.GetTipologieOccupazioniForMotore(ConstSession.StringConnectionOSAP, Anno, idEnte, Utility.Costanti.TRIBUTO_OSAP)
            ListAgevolazioni = DTO.MetodiAgevolazione.GetAgevolazioniForMotore(ConstSession.StringConnectionOSAP, Anno.ToString(), idEnte, Utility.Costanti.TRIBUTO_OSAP)
            ListTariffe = DTO.MetodiTariffe.GetTariffeForMotore(Anno, Utility.Costanti.TRIBUTO_OSAP)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.OSAPLoadParamCalcolo.errore: ", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che calcola sanzioni ed interessi
    ''' Calcolo Sanzioni e Interessi su singolo Immobile (la procedura di calcolo delle sanzioni le calcola solo	se l'importo è positivo)
    ''' </summary>
    ''' <param name="IdFase"></param>
    ''' <param name="oAccertato"></param>
    ''' <param name="oListDettaglioAtto"></param>
    ''' <param name="MyHashTable"></param>
    ''' <param name="PercentualeScorporo"></param>
    ''' <param name="oCalcoloSanzioniInteressi"></param>
    ''' <param name="objDSInteressi"></param>
    ''' <param name="objDSSanzioni"></param>
    ''' <param name="oAtto"></param>
    ''' <param name="ImportoTotaleAvviso"></param>
    ''' <param name="ForzaFase"></param>
    ''' <param name="SanzForced"></param>
    ''' <returns></returns>
    ''' <revisionHistory><revision date="10/10/2015">se sono in fase 1 non devo ciclare su ogni articolo ma calcolare una volta soltanto</revision></revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="04/09/2019">
    ''' gli interessi devono essere calcolati anche se non vengono calcolate sanzioni
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory><revision date="12/11/2019">il calcolo interessi 8852/TASI deve essere fatto in acconto/saldo o in unica soluzione in base alla configurazione di TP_GENERALE_ICI</revision></revisionHistory>
    Private Function OSAPCalcoloSanzioniInteressi(ByVal IdFase As Integer, ByRef oAccertato() As OSAPAccertamentoArticolo, ByVal oListDettaglioAtto() As OggettoDettaglioAtto, ByVal MyHashTable As Hashtable, ByVal PercentualeScorporo As Double, sDataMorte As String, ByRef oCalcoloSanzioniInteressi As ObjBaseIntSanz, ByRef objDSInteressi As DataSet, ByRef objDSSanzioni As DataSet, ByRef oAtto As OggettoAttoOSAP, ByRef ImportoTotaleAvviso As Double, ForzaFase As Integer, SanzForced As String) As Boolean
        Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
        Dim intCount As Integer
        Dim copyRow As DataRow
        'Dim dt As New DataTable
        Dim nListDett, x, y, i As Integer
        Dim objDSCalcoloSanzioniInteressiAppoggio As DataSet
        Dim DiffTotaleSanzioni As Double           'Somma algebrica fra sanzioni e interessi x det. tipo avviso
        Dim DiffTotaleInteressi As Double          'Somma algebrica fra sanzioni e interessi x det. tipo avviso
        Dim DiffTotaleSanzInt As Double        'Somma algebrica fra sanzioni e interessi x det. tipo avviso
        Dim dtInteressi As New DataTable
        Dim dtSanzioni As New DataTable
        Dim TotImportoSanzioniACCERTAMENTO As Double = 0           'Totale Sanzioni atto di accertamento
        Dim TotImportoSanzioniRidottoACCERTAMENTO As Double = 0       'Totale Sanzioni atto di accertamento
        Dim TotImportoInteressiACCERTAMENTO As Double = 0         'Totale Interessi atto di accertamento
        Dim TotDiffImpostaACCERTAMENTO As Double = 0         'Importo Totale Differenza di imposta atto di accertamento
        Dim ImpDiffImposta As Double
        Dim CalcOnAll As String = "0"
        Dim HasSanz As Boolean = True
        Dim ListBaseCalc As New ArrayList
        Dim ListInteressi() As ObjInteressiSanzioni

        Try
            If oAtto.IMPORTO_DIFFERENZA_IMPOSTA <> 0 Then
                '*******************************************************************
                'Calcolo Sanzioni e Interessi su singolo Immobile (la procedura di calcolo delle sanzioni le calcola solo	se l'importo è positivo)
                '*******************************************************************
                For nListDett = 0 To oListDettaglioAtto.Length - 1
                    ImpDiffImposta = 0
                    If SanzForced <> "" Then
                        HasSanz = False
                    End If
                    Dim MyHashTableDati As New Hashtable
                    Try
                        For Each mySanz As String In oListDettaglioAtto(nListDett).Sanzioni.Split(",")
                            Dim DefSanz() As String
                            DefSanz = mySanz.Split("#")
                            CalcOnAll = DefSanz(2)
                            If SanzForced <> "" Then
                                If DefSanz(0) = SanzForced Then
                                    HasSanz = True
                                End If
                            End If
                        Next
                    Catch ex As Exception

                    End Try
                    If (IdFase = OggettoAtto.Fase.VersamentiTardivi Or ForzaFase = OggettoAtto.Fase.VersamentiTardivi) And CalcOnAll = "1" Then
                        nListDett = oListDettaglioAtto.Length - 1
                    End If
                    If SanzForced <> "" Then
                        MyHashTableDati.Add("IDSANZIONI", SanzForced + "#" + MyHashTable("TIPOPROVVEDIMENTO").ToString + "#" + CalcOnAll)
                    Else
                        If InStr(oListDettaglioAtto(nListDett).Sanzioni, "#") Then
                            MyHashTableDati.Add("IDSANZIONI", oListDettaglioAtto(nListDett).Sanzioni)
                        Else
                            MyHashTableDati.Add("IDSANZIONI", oListDettaglioAtto(nListDett).Sanzioni & "#" & MyHashTable("TIPOPROVVEDIMENTO"))
                        End If
                    End If
                    If IdFase = OggettoAtto.Fase.VersamentiTardivi Or ForzaFase = OggettoAtto.Fase.VersamentiTardivi Or IdFase = OggettoAtto.Fase.VersatoDichiarato Or ForzaFase = OggettoAtto.Fase.VersatoDichiarato Then
                        If IdFase = OggettoAtto.Fase.VersamentiTardivi Or IdFase = OggettoAtto.Fase.VersatoDichiarato Then
                            MyHashTable("COD_TIPO_PROCEDIMENTO") = OggettoAtto.Procedimento.Liquidazione
                        Else
                            MyHashTable("COD_TIPO_PROCEDIMENTO") = OggettoAtto.Procedimento.Accertamento
                        End If
                        If CalcOnAll = "1" Then
                            'differenza d'imposta = dovuto singolo immobile-scorporo di pagato
                            ImpDiffImposta = oListDettaglioAtto(nListDett).ImpDichiarato - (oListDettaglioAtto(nListDett).ImpDichiarato * PercentualeScorporo)
                        Else
                            ImpDiffImposta = oListDettaglioAtto(nListDett).ImpDichiarato
                        End If
                    Else
                        'differenza d'imposta = accertato-dichiarato
                        ImpDiffImposta = oListDettaglioAtto(nListDett).ImpAccertato - oListDettaglioAtto(nListDett).ImpDichiarato
                        MyHashTable("COD_TIPO_PROCEDIMENTO") = OggettoAtto.Procedimento.Accertamento
                    End If
                    If MyHashTable.ContainsKey("ID_FASE") Then
                        MyHashTable("ID_FASE") = IdFase
                    Else
                        MyHashTable.Add("ID_FASE", IdFase)
                    End If

                    'L'Id Immobile è il Progressivo
                    MyHashTableDati.Add("IDIMMOBILE", oListDettaglioAtto(nListDett).Progressivo)
                    MyHashTableDati.Add("IDLEGAME", oListDettaglioAtto(nListDett).IdLegame)
                    '******************************************************************
                    'Calcolo le sanzioni per i singoli Immobili
                    '******************************************************************
                    If HasSanz Then
                        oCalcoloSanzioniInteressi = CreateDatasetPerSanzInt(MyHashTable("ANNOACCERTAMENTO"), ImpDiffImposta, 0, 0, oAtto.IMPORTO_PAGATO)
                        objDSCalcoloSanzioniInteressiAppoggio = SetObjDSAppoggioSanzioni(MyHashTable("ANNOACCERTAMENTO"), ImpDiffImposta, oAtto.IMPORTO_PAGATO)

                        objDSSanzioni = objCOMDichiarazioniAccertamenti.getSanzioni(ConstSession.StringConnection, ConstSession.IdEnte, MyHashTable, MyHashTableDati, oCalcoloSanzioniInteressi, objDSCalcoloSanzioniInteressiAppoggio, False, sDataMorte)

                        'Creo una copia della struttura
                        If dtSanzioni.Rows.Count = 0 And objDSSanzioni.Tables.Count > 0 Then
                            dtSanzioni = objDSSanzioni.Tables("SANZIONI").Copy
                            dtSanzioni.Clear()
                        End If

                        For intCount = 0 To objDSSanzioni.Tables("SANZIONI").Rows.Count - 1
                            objDSSanzioni.Tables("SANZIONI").Rows(intCount)("IMPORTO_GIORNI") = 0
                            copyRow = objDSSanzioni.Tables("SANZIONI").Rows(intCount)
                            dtSanzioni.ImportRow(copyRow)
                        Next

                        If Not dtSanzioni Is Nothing Then
                            For intCount = 0 To dtSanzioni.Rows.Count - 1
                                Dim rows() As DataRow
                                rows = dtSanzioni.Select("ID_LEGAME='" & oListDettaglioAtto(nListDett).IdLegame & "'")
                                For y = 0 To UBound(rows)
                                    rows(y).Item("Motivazioni") = oAccertato(nListDett).DescrSanzioni
                                Next
                                dtSanzioni.AcceptChanges()
                            Next
                        End If

                        'Aggiorno il DataSet con le sanzioni
                        'dt = objDSSanzioni.Tables(1)
                        'oCalcoloSanzioniInteressi.Dispose()
                        'oCalcoloSanzioniInteressi = New DataSet
                        'oCalcoloSanzioniInteressi.Tables.Add(dt.Copy)

                        'Aggiorno il DS con l'importo delle sanzioni calcolate
                        oListDettaglioAtto(nListDett).ImpSanzioni = oCalcoloSanzioniInteressi.Sanzioni
                        oListDettaglioAtto(nListDett).ImpSanzioniRidotto = oCalcoloSanzioniInteressi.SanzioniRidotto

                        'totale sanzione dell'atto di accertamento
                        TotImportoSanzioniACCERTAMENTO = TotImportoSanzioniACCERTAMENTO + oListDettaglioAtto(nListDett).ImpSanzioni
                        TotImportoSanzioniRidottoACCERTAMENTO = TotImportoSanzioniRidottoACCERTAMENTO + oListDettaglioAtto(nListDett).ImpSanzioniRidotto

                        'Somma algebrica per determinare Tipo Avviso
                        DiffTotaleSanzioni = oListDettaglioAtto(nListDett).ImpSanzioni
                        DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleSanzioni
                    End If
                    'CALCOLO INTERESSI
                    If oListDettaglioAtto(nListDett).Calcola_Interessi = True Then
                        Dim myItem As New ObjBaseIntSanz
                        myItem.Anno = MyHashTable("ANNOACCERTAMENTO")
                        myItem.COD_TIPO_PROVVEDIMENTO = MyHashTable("TIPOPROVVEDIMENTO")
                        myItem.DifferenzaImposta = ImpDiffImposta
                        myItem.IdContribuente = MyHashTable("CODCONTRIBUENTE")
                        myItem.IdEnte = ConstSession.IdEnte
                        ListBaseCalc.Add(myItem)

                        objCOMDichiarazioniAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
                        ListInteressi = objCOMDichiarazioniAccertamenti.getInteressi(ConstSession.IdEnte, Utility.Costanti.TRIBUTO_OSAP, OggettoAtto.Capitolo.Interessi, MyHashTable("TIPOPROVVEDIMENTO"), MyHashTable("COD_TIPO_PROCEDIMENTO"), IdFase, Now, "", "", oListDettaglioAtto(nListDett).IdLegame, CType(ListBaseCalc.ToArray(GetType(ObjBaseIntSanz)), ObjBaseIntSanz()), ConstSession.StringConnection)
                        'Log.Debug("prelevato interessi")
                        If Not IsNothing(ListInteressi) Then
                            'Aggiorno il DS con l'importo delle interessi calcolati
                            For Each myInt As ObjInteressiSanzioni In ListInteressi
                                oListDettaglioAtto(nListDett).ImpInteressi = myInt.IMPORTO_GIORNI
                                DiffTotaleInteressi = myInt.IMPORTO_GIORNI
                            Next
                            'totale interessi dell'atto di accertamento
                            TotImportoInteressiACCERTAMENTO += DiffTotaleInteressi
                            'Somma algebrica per determinare se Rimborso o Avviso di accertamento
                            DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleInteressi
                        End If
                    Else
                        oListDettaglioAtto(nListDett).ImpInteressi = 0
                        'totale interessi dell'atto di accertamento
                        TotImportoInteressiACCERTAMENTO = TotImportoInteressiACCERTAMENTO + oListDettaglioAtto(nListDett).ImpInteressi

                        'Somma algebrica per determinare se Rimborso o Avviso di accertamento
                        DiffTotaleInteressi = 0
                        DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleInteressi
                    End If
                    Log.Debug("nListDett=" & nListDett & vbTab & " Legame:" & oListDettaglioAtto(nListDett).IdLegame & vbTab & " - Prog:" & oListDettaglioAtto(nListDett).Progressivo & vbTab & " - SANZIONI. Piene=" & oListDettaglioAtto(nListDett).ImpSanzioni & " - Ridotte=" & oListDettaglioAtto(nListDett).ImpSanzioniRidotto & vbTab & vbTab & vbTab & " - INTERESSI = " & oListDettaglioAtto(nListDett).ImpInteressi)
                Next

                For i = 0 To oAccertato.Length - 1
                    For x = 0 To oListDettaglioAtto.Length - 1
                        If oAccertato(i).Progressivo = oListDettaglioAtto(x).Progressivo Then
                            If oAccertato(i).IdLegame = oListDettaglioAtto(x).IdLegame Then
                                oAccertato(i).ImpDiffImposta = oListDettaglioAtto(x).ImpAccertato - oListDettaglioAtto(x).ImpDichiarato
                                oAccertato(i).ImpSanzioni += oListDettaglioAtto(x).ImpSanzioni
                                oAccertato(i).ImpSanzioniRidotto += oListDettaglioAtto(x).ImpSanzioniRidotto
                                oAccertato(i).ImpInteressi += oListDettaglioAtto(x).ImpInteressi
                            End If
                        End If
                    Next
                Next

                oAtto.IMPORTO_SANZIONI += TotImportoSanzioniACCERTAMENTO
                oAtto.IMPORTO_SANZIONI_RIDOTTO += TotImportoSanzioniRidottoACCERTAMENTO
                oAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI = (oAtto.IMPORTO_SANZIONI - (oAtto.IMPORTO_SANZIONI_RIDOTTO * 3)) '* 4'*** 20171103 - chiedere a Liana se è *4 o *3 ***
                oAtto.IMPORTO_INTERESSI += TotImportoInteressiACCERTAMENTO
                If IdFase = OggettoAtto.Fase.VersamentiTardivi Or IdFase = OggettoAtto.Fase.VersatoDichiarato Then
                    oAtto.IMPORTO_DIFFERENZA_IMPOSTA_F2 = oAtto.IMPORTO_DICHIARATO_F2 - oAtto.IMPORTO_PAGATO
                    oAtto.IMPORTO_SANZIONI_F2 += TotImportoSanzioniACCERTAMENTO
                    oAtto.IMPORTO_INTERESSI_F2 += TotImportoInteressiACCERTAMENTO
                    oAtto.IMPORTO_TOTALE_F2 = oAtto.IMPORTO_DIFFERENZA_IMPOSTA_F2 + oAtto.IMPORTO_SANZIONI_F2 + oAtto.IMPORTO_INTERESSI_F2
                Else
                    oAtto.IMPORTO_SANZIONI_ACC += TotImportoSanzioniACCERTAMENTO
                    oAtto.IMPORTO_SANZIONI_RIDOTTE_ACC += TotImportoSanzioniRidottoACCERTAMENTO
                    oAtto.IMPORTO_INTERESSI_ACC += TotImportoInteressiACCERTAMENTO
                    oAtto.IMPORTO_TOTALE_ACC = oAtto.IMPORTO_DIFFERENZA_IMPOSTA_ACC + oAtto.IMPORTO_SANZIONI_ACC + oAtto.IMPORTO_INTERESSI_ACC
                End If
                Log.Debug(oAtto.COD_ENTE & " - TotImportoSanzioniACCERTAMENTO:" & TotImportoSanzioniACCERTAMENTO)
                Log.Debug(oAtto.COD_ENTE & " - TotImportoSanzioniRidottoACCERTAMENTO:" & TotImportoSanzioniRidottoACCERTAMENTO)
                Log.Debug(oAtto.COD_ENTE & " - TotImportoInteressiACCERTAMENTO:" & TotImportoInteressiACCERTAMENTO)
            Else
                oAtto.IMPORTO_INTERESSI += 0
                oAtto.IMPORTO_SANZIONI += 0
                oAtto.IMPORTO_SANZIONI_RIDOTTO += 0
            End If

            If Not oCalcoloSanzioniInteressi Is Nothing Then
                oCalcoloSanzioniInteressi.Sanzioni = oAtto.IMPORTO_SANZIONI
                oCalcoloSanzioniInteressi.SanzioniRidotto = oAtto.IMPORTO_SANZIONI_RIDOTTO

                oCalcoloSanzioniInteressi.Interessi = oAtto.IMPORTO_INTERESSI
                oCalcoloSanzioniInteressi.DifferenzaImposta = oAtto.IMPORTO_DIFFERENZA_IMPOSTA
            End If
            If Not objDSSanzioni Is Nothing Then
                objDSSanzioni.Dispose()
            End If
            If Not objDSInteressi Is Nothing Then
                objDSInteressi.Dispose()
            End If

            objDSSanzioni = New DataSet
            objDSInteressi = New DataSet

            objDSSanzioni.Tables.Add(dtSanzioni.Copy)
            objDSInteressi.Tables.Add(dtInteressi.Copy)

            ImportoTotaleAvviso = oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.OSAPCalcoloSanzioniInteressi.errore: ", ex)
            Return False
        End Try
    End Function
    'Private Function OSAPCalcoloSanzioniInteressi(ByVal IdFase As Integer, ByRef oAccertato() As OSAPAccertamentoArticolo, ByVal oListDettaglioAtto() As OggettoDettaglioAtto, ByVal MyHashTable As Hashtable, ByVal PercentualeScorporo As Double, sDataMorte As String, ByRef objDSCalcoloSanzioniInteressi As DataSet, ByRef objDSInteressi As DataSet, ByRef objDSSanzioni As DataSet, ByRef oAtto As OggettoAttoOSAP, ByRef ImportoTotaleAvviso As Double, ForzaFase As Integer, SanzForced As String) As Boolean
    '    Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '    Dim intCount As Integer
    '    Dim copyRow As DataRow
    '    Dim dt As New DataTable
    '    Dim nListDett, x, y, i As Integer
    '    Dim objDSCalcoloSanzioniInteressiAppoggio As DataSet
    '    Dim DiffTotaleSanzioni As Double           'Somma algebrica fra sanzioni e interessi x det. tipo avviso
    '    Dim DiffTotaleInteressi As Double          'Somma algebrica fra sanzioni e interessi x det. tipo avviso
    '    Dim DiffTotaleSanzInt As Double        'Somma algebrica fra sanzioni e interessi x det. tipo avviso
    '    Dim dtInteressi As New DataTable
    '    Dim dtSanzioni As New DataTable
    '    Dim TotImportoSanzioniACCERTAMENTO As Double = 0           'Totale Sanzioni atto di accertamento
    '    Dim TotImportoSanzioniRidottoACCERTAMENTO As Double = 0       'Totale Sanzioni atto di accertamento
    '    Dim TotImportoInteressiACCERTAMENTO As Double = 0         'Totale Interessi atto di accertamento
    '    Dim TotDiffImpostaACCERTAMENTO As Double = 0         'Importo Totale Differenza di imposta atto di accertamento
    '    Dim ImpDiffImposta As Double
    '    Dim CalcOnAll As String = "0"
    '    Dim HasSanz As Boolean = True
    '    Dim ListBaseCalc As New ArrayList
    '    Dim ListInteressi() As ObjInteressiSanzioni

    '    Try
    '        If oAtto.IMPORTO_DIFFERENZA_IMPOSTA <> 0 Then
    '            '*******************************************************************
    '            'Calcolo Sanzioni e Interessi su singolo Immobile (la procedura di calcolo delle sanzioni le calcola solo	se l'importo è positivo)
    '            '*******************************************************************
    '            For nListDett = 0 To oListDettaglioAtto.Length - 1
    '                ImpDiffImposta = 0
    '                If SanzForced <> "" Then
    '                    HasSanz = False
    '                End If
    '                Dim MyHashTableDati As New Hashtable
    '                Try
    '                    For Each mySanz As String In oListDettaglioAtto(nListDett).Sanzioni.Split(",")
    '                        Dim DefSanz() As String
    '                        DefSanz = mySanz.Split("#")
    '                        CalcOnAll = DefSanz(2)
    '                        If SanzForced <> "" Then
    '                            If DefSanz(0) = SanzForced Then
    '                                HasSanz = True
    '                            End If
    '                        End If
    '                    Next
    '                Catch ex As Exception

    '                End Try
    '                If (IdFase = OggettoAtto.Fase.VersamentiTardivi Or ForzaFase = OggettoAtto.Fase.VersamentiTardivi) And CalcOnAll = "1" Then
    '                    nListDett = oListDettaglioAtto.Length - 1
    '                End If
    '                If SanzForced <> "" Then
    '                    MyHashTableDati.Add("IDSANZIONI", SanzForced + "#" + MyHashTable("TIPOPROVVEDIMENTO").ToString + "#" + CalcOnAll)
    '                Else
    '                    If InStr(oListDettaglioAtto(nListDett).Sanzioni, "#") Then
    '                        MyHashTableDati.Add("IDSANZIONI", oListDettaglioAtto(nListDett).Sanzioni)
    '                    Else
    '                        MyHashTableDati.Add("IDSANZIONI", oListDettaglioAtto(nListDett).Sanzioni & "#" & MyHashTable("TIPOPROVVEDIMENTO"))
    '                    End If
    '                End If
    '                If IdFase = OggettoAtto.Fase.VersamentiTardivi Or ForzaFase = OggettoAtto.Fase.VersamentiTardivi Or IdFase = OggettoAtto.Fase.VersatoDichiarato Or ForzaFase = OggettoAtto.Fase.VersatoDichiarato Then
    '                    If IdFase = OggettoAtto.Fase.VersamentiTardivi Or IdFase = OggettoAtto.Fase.VersatoDichiarato Then
    '                        MyHashTable("COD_TIPO_PROCEDIMENTO") = OggettoAtto.Procedimento.Liquidazione
    '                    Else
    '                        MyHashTable("COD_TIPO_PROCEDIMENTO") = OggettoAtto.Procedimento.Accertamento
    '                    End If
    '                    If CalcOnAll = "1" Then
    '                        'differenza d'imposta = dovuto singolo immobile-scorporo di pagato
    '                        ImpDiffImposta = oListDettaglioAtto(nListDett).ImpDichiarato - (oListDettaglioAtto(nListDett).ImpDichiarato * PercentualeScorporo)
    '                    Else
    '                        ImpDiffImposta = oListDettaglioAtto(nListDett).ImpDichiarato
    '                    End If
    '                Else
    '                    'differenza d'imposta = accertato-dichiarato
    '                    ImpDiffImposta = oListDettaglioAtto(nListDett).ImpAccertato - oListDettaglioAtto(nListDett).ImpDichiarato
    '                    MyHashTable("COD_TIPO_PROCEDIMENTO") = OggettoAtto.Procedimento.Accertamento
    '                End If
    '                If MyHashTable.ContainsKey("ID_FASE") Then
    '                    MyHashTable("ID_FASE") = IdFase
    '                Else
    '                    MyHashTable.Add("ID_FASE", IdFase)
    '                End If

    '                'L'Id Immobile è il Progressivo
    '                MyHashTableDati.Add("IDIMMOBILE", oListDettaglioAtto(nListDett).Progressivo)
    '                MyHashTableDati.Add("IDLEGAME", oListDettaglioAtto(nListDett).IdLegame)
    '                '******************************************************************
    '                'Calcolo le sanzioni per i singoli Immobili
    '                '******************************************************************
    '                If HasSanz Then
    '                    objDSCalcoloSanzioniInteressi = CreateDatasetPerSanzInt(MyHashTable("ANNOACCERTAMENTO"), MyHashTable("CODCONTRIBUENTE"), ImpDiffImposta, 0, 0, oAtto.IMPORTO_PAGATO)
    '                    objDSCalcoloSanzioniInteressiAppoggio = SetObjDSAppoggioSanzioni(MyHashTable("ANNOACCERTAMENTO"), ImpDiffImposta, oAtto.IMPORTO_PAGATO)

    '                    objDSSanzioni = objCOMDichiarazioniAccertamenti.getSanzioni(MyHashTable, MyHashTableDati, objDSCalcoloSanzioniInteressi, objDSCalcoloSanzioniInteressiAppoggio, False, sDataMorte)

    '                    'Creo una copia della struttura
    '                    If dtSanzioni.Rows.Count = 0 And objDSSanzioni.Tables.Count > 0 Then
    '                        dtSanzioni = objDSSanzioni.Tables("SANZIONI").Copy
    '                        dtSanzioni.Clear()
    '                    End If

    '                    For intCount = 0 To objDSSanzioni.Tables("SANZIONI").Rows.Count - 1
    '                        objDSSanzioni.Tables("SANZIONI").Rows(intCount)("IMPORTO_GIORNI") = 0
    '                        copyRow = objDSSanzioni.Tables("SANZIONI").Rows(intCount)
    '                        dtSanzioni.ImportRow(copyRow)
    '                    Next

    '                    If Not dtSanzioni Is Nothing Then
    '                        For intCount = 0 To dtSanzioni.Rows.Count - 1
    '                            Dim rows() As DataRow
    '                            rows = dtSanzioni.Select("ID_LEGAME='" & oListDettaglioAtto(nListDett).IdLegame & "'")
    '                            For y = 0 To UBound(rows)
    '                                rows(y).Item("Motivazioni") = oAccertato(nListDett).DescrSanzioni
    '                            Next
    '                            dtSanzioni.AcceptChanges()
    '                        Next
    '                    End If

    '                    'Aggiorno il DataSet con le sanzioni
    '                    dt = objDSSanzioni.Tables(1)
    '                    objDSCalcoloSanzioniInteressi.Dispose()
    '                    objDSCalcoloSanzioniInteressi = New DataSet
    '                    objDSCalcoloSanzioniInteressi.Tables.Add(dt.Copy)

    '                    'Aggiorno il DS con l'importo delle sanzioni calcolate
    '                    oListDettaglioAtto(nListDett).ImpSanzioni = objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI")
    '                    oListDettaglioAtto(nListDett).ImpSanzioniRidotto = objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO")

    '                    'totale sanzione dell'atto di accertamento
    '                    TotImportoSanzioniACCERTAMENTO = TotImportoSanzioniACCERTAMENTO + oListDettaglioAtto(nListDett).ImpSanzioni
    '                    TotImportoSanzioniRidottoACCERTAMENTO = TotImportoSanzioniRidottoACCERTAMENTO + oListDettaglioAtto(nListDett).ImpSanzioniRidotto

    '                    'Somma algebrica per determinare Tipo Avviso
    '                    DiffTotaleSanzioni = oListDettaglioAtto(nListDett).ImpSanzioni
    '                    DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleSanzioni
    '                End If
    '                'CALCOLO INTERESSI
    '                If oListDettaglioAtto(nListDett).Calcola_Interessi = True Then
    '                    Dim myItem As New ObjBaseIntSanz
    '                    myItem.Anno = MyHashTable("ANNOACCERTAMENTO")
    '                    myItem.COD_TIPO_PROVVEDIMENTO = MyHashTable("TIPOPROVVEDIMENTO")
    '                    myItem.DifferenzaImposta = ImpDiffImposta
    '                    myItem.IdContribuente = MyHashTable("CODCONTRIBUENTE")
    '                    myItem.IdEnte = ConstSession.IdEnte
    '                    ListBaseCalc.Add(myItem)

    '                    objCOMDichiarazioniAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '                    ListInteressi = objCOMDichiarazioniAccertamenti.getInteressi(ConstSession.IdEnte, Utility.Costanti.TRIBUTO_OSAP, OggettoAtto.Capitolo.Interessi, MyHashTable("TIPOPROVVEDIMENTO"), MyHashTable("COD_TIPO_PROCEDIMENTO"), IdFase, Now, "", "", oListDettaglioAtto(nListDett).IdLegame, CType(ListBaseCalc.ToArray(GetType(ObjBaseIntSanz)), ObjBaseIntSanz()), ConstSession.StringConnection)
    '                    'Log.Debug("prelevato interessi")
    '                    If Not IsNothing(ListInteressi) Then
    '                        'Aggiorno il DS con l'importo delle interessi calcolati
    '                        For Each myInt As ObjInteressiSanzioni In ListInteressi
    '                            oListDettaglioAtto(nListDett).ImpInteressi = myInt.IMPORTO_GIORNI
    '                            DiffTotaleInteressi = myInt.IMPORTO_GIORNI
    '                        Next
    '                        'totale interessi dell'atto di accertamento
    '                        TotImportoInteressiACCERTAMENTO += DiffTotaleInteressi
    '                        'Somma algebrica per determinare se Rimborso o Avviso di accertamento
    '                        DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleInteressi
    '                    End If
    '                Else
    '                    'Aggiorno il DataSet con gli interessi
    '                    objDSCalcoloSanzioniInteressi = New DataSet
    '                    objDSCalcoloSanzioniInteressi.Tables.Add(dt.Copy)
    '                    oListDettaglioAtto(nListDett).ImpInteressi = 0
    '                    'totale interessi dell'atto di accertamento
    '                    TotImportoInteressiACCERTAMENTO = TotImportoInteressiACCERTAMENTO + oListDettaglioAtto(nListDett).ImpInteressi

    '                    'Somma algebrica per determinare se Rimborso o Avviso di accertamento
    '                    DiffTotaleInteressi = 0
    '                    DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleInteressi
    '                End If
    '                Log.Debug("nListDett=" & nListDett & vbTab & " Legame:" & oListDettaglioAtto(nListDett).IdLegame & vbTab & " - Prog:" & oListDettaglioAtto(nListDett).Progressivo & vbTab & " - SANZIONI. Piene=" & oListDettaglioAtto(nListDett).ImpSanzioni & " - Ridotte=" & oListDettaglioAtto(nListDett).ImpSanzioniRidotto & vbTab & vbTab & vbTab & " - INTERESSI = " & oListDettaglioAtto(nListDett).ImpInteressi)
    '            Next

    '            For i = 0 To oAccertato.Length - 1
    '                For x = 0 To oListDettaglioAtto.Length - 1
    '                    If oAccertato(i).Progressivo = oListDettaglioAtto(x).Progressivo Then
    '                        If oAccertato(i).IdLegame = oListDettaglioAtto(x).IdLegame Then
    '                            oAccertato(i).ImpDiffImposta = oListDettaglioAtto(x).ImpAccertato - oListDettaglioAtto(x).ImpDichiarato
    '                            oAccertato(i).ImpSanzioni += oListDettaglioAtto(x).ImpSanzioni
    '                            oAccertato(i).ImpSanzioniRidotto += oListDettaglioAtto(x).ImpSanzioniRidotto
    '                            oAccertato(i).ImpInteressi += oListDettaglioAtto(x).ImpInteressi
    '                        End If
    '                    End If
    '                Next
    '            Next

    '            oAtto.IMPORTO_SANZIONI += TotImportoSanzioniACCERTAMENTO
    '            oAtto.IMPORTO_SANZIONI_RIDOTTO += TotImportoSanzioniRidottoACCERTAMENTO
    '            oAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI = (oAtto.IMPORTO_SANZIONI - (oAtto.IMPORTO_SANZIONI_RIDOTTO * 3)) '* 4'*** 20171103 - chiedere a Liana se è *4 o *3 ***
    '            oAtto.IMPORTO_INTERESSI += TotImportoInteressiACCERTAMENTO
    '            If IdFase = OggettoAtto.Fase.VersamentiTardivi Or IdFase = OggettoAtto.Fase.VersatoDichiarato Then
    '                oAtto.IMPORTO_DIFFERENZA_IMPOSTA_F2 = oAtto.IMPORTO_DICHIARATO_F2 - oAtto.IMPORTO_PAGATO
    '                oAtto.IMPORTO_SANZIONI_F2 += TotImportoSanzioniACCERTAMENTO
    '                oAtto.IMPORTO_INTERESSI_F2 += TotImportoInteressiACCERTAMENTO
    '                oAtto.IMPORTO_TOTALE_F2 = oAtto.IMPORTO_DIFFERENZA_IMPOSTA_F2 + oAtto.IMPORTO_SANZIONI_F2 + oAtto.IMPORTO_INTERESSI_F2
    '            Else
    '                oAtto.IMPORTO_SANZIONI_ACC += TotImportoSanzioniACCERTAMENTO
    '                oAtto.IMPORTO_SANZIONI_RIDOTTE_ACC += TotImportoSanzioniRidottoACCERTAMENTO
    '                oAtto.IMPORTO_INTERESSI_ACC += TotImportoInteressiACCERTAMENTO
    '                oAtto.IMPORTO_TOTALE_ACC = oAtto.IMPORTO_DIFFERENZA_IMPOSTA_ACC + oAtto.IMPORTO_SANZIONI_ACC + oAtto.IMPORTO_INTERESSI_ACC
    '            End If
    '            Log.Debug(oAtto.COD_ENTE & " - TotImportoSanzioniACCERTAMENTO:" & TotImportoSanzioniACCERTAMENTO)
    '            Log.Debug(oAtto.COD_ENTE & " - TotImportoSanzioniRidottoACCERTAMENTO:" & TotImportoSanzioniRidottoACCERTAMENTO)
    '            Log.Debug(oAtto.COD_ENTE & " - TotImportoInteressiACCERTAMENTO:" & TotImportoInteressiACCERTAMENTO)
    '        Else
    '            oAtto.IMPORTO_INTERESSI += 0
    '            oAtto.IMPORTO_SANZIONI += 0
    '            oAtto.IMPORTO_SANZIONI_RIDOTTO += 0
    '        End If

    '        If Not objDSCalcoloSanzioniInteressi Is Nothing Then
    '            If objDSCalcoloSanzioniInteressi.Tables.Count > 0 Then
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI") = oAtto.IMPORTO_SANZIONI
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO") = oAtto.IMPORTO_SANZIONI_RIDOTTO

    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI") = oAtto.IMPORTO_INTERESSI
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("DIFFERENZA_IMPOSTA_TOTALE") = oAtto.IMPORTO_DIFFERENZA_IMPOSTA
    '            End If
    '        End If
    '        If Not objDSSanzioni Is Nothing Then
    '            objDSSanzioni.Dispose()
    '        End If
    '        If Not objDSInteressi Is Nothing Then
    '            objDSInteressi.Dispose()
    '        End If

    '        objDSSanzioni = New DataSet
    '        objDSInteressi = New DataSet

    '        objDSSanzioni.Tables.Add(dtSanzioni.Copy)
    '        objDSInteressi.Tables.Add(dtInteressi.Copy)

    '        ImportoTotaleAvviso = oAtto.IMPORTO_DIFFERENZA_IMPOSTA + oAtto.IMPORTO_SANZIONI + oAtto.IMPORTO_INTERESSI
    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.OSAPCalcoloSanzioniInteressi.errore: ", ex)
    '        Return False
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="TipoRicerca"></param>
    ''' <param name="nIdProvvedimento"></param>
    ''' <returns></returns>
    Public Function OSAPRicercaArticoliDichAcc(ByVal TipoRicerca As String, ByVal nIdProvvedimento As Integer) As OSAPAccertamentoArticolo()
        Dim sProcedureName As String
        Dim MyDBEngine As DAL.DBEngine

        Try
            MyDBEngine = DAO.DBEngineFactory.GetDBEngine(ConfigurationManager.AppSettings("connectionStringOPENgovPROVVEDIMENTI"))
            MyDBEngine.OpenConnection()

            If TipoRicerca = "D" Then
                sProcedureName = "prc_getOSAPArticoliDic"
            Else
                sProcedureName = "prc_getOSAPArticoliAcc"
            End If
            Dim dtArticoli As New DataTable
            MyDBEngine.ClearParameters()
            MyDBEngine.AddParameter("@IdProvvedimento", nIdProvvedimento, ParameterDirection.Input)
            MyDBEngine.ExecuteQuery(dtArticoli, sProcedureName, CommandType.StoredProcedure)

            Return OSAPFillArticoliDichAcc(dtArticoli)

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.OSAPRicercaArticoliDichAcc.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Private Function OSAPFillArticoliDichAcc(ByVal dt As DataTable) As OSAPAccertamentoArticolo()
        Dim MyArray As ArrayList = New ArrayList
        HttpContext.Current.Session("StringConnection") = ConfigurationManager.AppSettings("connectionStringOPENgovOSAP")
        Try
            Dim CurrentItem As OSAPAccertamentoArticolo = Nothing
            Dim i As Integer = 0
            Do While (i < dt.Rows.Count)
                CurrentItem = New OSAPAccertamentoArticolo
                '*** campi propri dell'articolo ***
                CurrentItem.IdArticolo = CType(dt.Rows(i)("IDARTICOLO"), Integer)
                CurrentItem.IdDichiarazione = CType(dt.Rows(i)("IDDICHIARAZIONE"), Integer)
                CurrentItem.IdTributo = CType(dt.Rows(i)("IDTRIBUTO"), String)
                CurrentItem.DataInserimento = CType(dt.Rows(i)("DATA_INSERIMENTO"), DateTime)
                CurrentItem.CodVia = CType(dt.Rows(i)("CODVIA"), Integer)
                CurrentItem.SVia = DTO.MetodiArticolo.GetDescrizioneVia(CurrentItem.CodVia, dt.Rows(i)("IDENTE"))
                If Not IsDBNull(dt.Rows(i)("CIVICO")) Then
                    CurrentItem.Civico = CType(dt.Rows(i)("CIVICO"), Integer)
                Else
                    CurrentItem.Civico = -1
                End If
                CurrentItem.Esponente = StringOperation.FormatString(dt.Rows(i)("ESPONENTE"))
                CurrentItem.Interno = StringOperation.FormatString(dt.Rows(i)("INTERNO"))
                CurrentItem.Scala = StringOperation.FormatString(dt.Rows(i)("SCALA"))
                CurrentItem.Categoria = DTO.MetodiCategorie.GetCategoria(ConstSession.StringConnectionOSAP, CType(dt.Rows(i)("IDCATEGORIA"), Integer), "", dt.Rows(i)("IDENTE"), Utility.Costanti.TRIBUTO_OSAP)
                CurrentItem.TipologiaOccupazione = DTO.MetodiTipologieOccupazioni.GetTipologiaOccupazione(ConstSession.StringConnectionOSAP, CType(dt.Rows(i)("IDTIPOLOGIAOCCUPAZIONE"), Integer), "", dt.Rows(i)("IDENTE"), Utility.Costanti.TRIBUTO_OSAP)
                CurrentItem.Consistenza = Double.Parse(dt.Rows(i)("CONSISTENZA").ToString)
                CurrentItem.TipoConsistenzaTOCO = DTO.MetodiTipoConsistenza.GetTipoConsistenza(ConstSession.StringConnectionOSAP, CType(dt.Rows(i)("IDTIPOCONSISTENZA"), Integer))
                CurrentItem.DataInizioOccupazione = CType(dt.Rows(i)("DATAINIZIOOCCUPAZIONE"), DateTime)
                If Not IsDBNull(dt.Rows(i)("DATAFINEOCCUPAZIONE")) Then
                    CurrentItem.DataFineOccupazione = CType(dt.Rows(i)("DATAFINEOCCUPAZIONE"), DateTime)
                Else
                    CurrentItem.DataFineOccupazione = DateTime.MaxValue
                End If
                CurrentItem.TipoDurata = DTO.MetodiDurata.GetDurata(ConstSession.StringConnectionOSAP, CType(dt.Rows(i)("IDDURATA"), Integer))
                CurrentItem.DurataOccupazione = CType(dt.Rows(i)("DURATAOCCUPAZIONE"), Integer)
                CurrentItem.MaggiorazionePerc = Double.Parse(dt.Rows(i)("MAGGIORAZIONE_PERC").ToString)
                CurrentItem.MaggiorazioneImporto = Double.Parse(dt.Rows(i)("MAGGIORAZIONE_IMPORTO").ToString)
                CurrentItem.Note = StringOperation.FormatString(dt.Rows(i)("NOTE"))
                CurrentItem.Operatore = StringOperation.FormatString(dt.Rows(i)("operatore"))
                If Not IsDBNull(dt.Rows(i)("DATA_INSERIMENTO")) Then
                    CurrentItem.DataInserimento = CType(dt.Rows(i)("DATA_INSERIMENTO"), DateTime)
                Else
                    CurrentItem.DataInserimento = DateTime.Now
                End If
                CurrentItem.ListAgevolazioni = DTO.MetodiAgevolazione.GetAgevolazioniVSArticolo(ConstSession.StringConnectionOSAP, CType(dt.Rows(i)("IDARTICOLO"), Integer), -1, dt.Rows(i)("IDENTE"))
                CurrentItem.DetrazioneImporto = Double.Parse(dt.Rows(i)("DETRAZIONE_IMPORTO").ToString)
                CurrentItem.Attrazione = CType(dt.Rows(i)("ATTRAZIONE"), Boolean)
                CurrentItem.ElencoPercAgevolazioni = DTO.MetodiAgevolazione.GetElencoPercAgevolazioni(CurrentItem.ListAgevolazioni)
                CurrentItem.IdArticoloPadre = CType(dt.Rows(i)("IDARTICOLOPADRE"), Integer)
                '*** ***
                '*** campi propri del provvedimento ***
                CurrentItem.Anno = StringOperation.FormatString(dt.Rows(i)("anno"))
                Dim CurrentCalcolo As New IRemInterfaceOSAP.CalcoloResult
                CurrentCalcolo.ImportoCalcolato = dt.Rows(i)("IMPORTO")
                CurrentCalcolo.ImportoLordo = dt.Rows(i)("IMPORTO_LORDO")
                CurrentCalcolo.TariffaApplicata = dt.Rows(i)("TARIFFA_APPLICATA")
                CurrentItem.Calcolo = CurrentCalcolo
                CurrentItem.IdLegame = dt.Rows(i)("ID_LEGAME")
                CurrentItem.IdProvvedimento = dt.Rows(i)("ID_PROVVEDIMENTO")
                CurrentItem.ImpInteressi = dt.Rows(i)("IMPORTO_INTERESSI")
                CurrentItem.ImpDiffImposta = dt.Rows(i)("IMPORTO_DIFFIMPOSTA")
                CurrentItem.ImpSanzioni = dt.Rows(i)("IMPORTO_SANZIONI")
                CurrentItem.ImpSanzioniRidotto = dt.Rows(i)("IMPORTO_SANZIONI_RIDOTTO")
                If Not IsDBNull(dt.Rows(i)("motivazione")) Then
                    CurrentItem.DescrSanzioni = dt.Rows(i)("motivazione")
                End If
                If Not IsDBNull(dt.Rows(i)("cod_voce")) Then
                    If Not IsDBNull(dt.Rows(i)("cod_tipo_provvedimento")) Then
                        CurrentItem.Sanzioni = dt.Rows(i)("cod_voce") & "#" & dt.Rows(i)("cod_tipo_provvedimento")
                    Else
                        CurrentItem.Sanzioni = dt.Rows(i)("cod_voce") & "#"
                    End If
                End If
                If Not IsDBNull(dt.Rows(i)("calcola_interessi")) Then
                    If dt.Rows(i)("calcola_interessi") = 0 Then
                        CurrentItem.Calcola_Interessi = False
                    Else
                        CurrentItem.Calcola_Interessi = True
                    End If
                End If
                CurrentItem.Progressivo = i + 1
                MyArray.Add(CurrentItem)
                i = (i + 1)
            Loop
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.OSAPFillArticoliDichAcc.errore: ", ex)
        End Try
        Return CType(MyArray.ToArray(GetType(OSAPAccertamentoArticolo)), OSAPAccertamentoArticolo())
    End Function

    'Public Function DeleteAttoOSAP(ByVal sMyConnectionString As String, ByVal IDProvvedimento As Long) As Boolean
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Try

    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        'CANCELLO PROVVEDIMENTI TRAMITE ID_PROVVEDIMENTO
    '        cmdMyCommand.CommandText = "prc_DeleteProvvedimento"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IdProvvedimento", IDProvvedimento)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()

    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.DeleteAttoOSAP.errore: ", ex)
    '        Throw New Exception("DeleteProvvedimentiAccertamentoOSAP::" & ex.Message)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    Public Function OSAPGetAgevolazioniPerStampa(ByVal TipoRicerca As String, ByVal nIdProvvedimento As Integer) As DataTable
        Dim sProcedureName As String
        Dim MyDBEngine As DAL.DBEngine

        Try
            MyDBEngine = DAO.DBEngineFactory.GetDBEngine(ConfigurationManager.AppSettings("connectionStringOPENgovPROVVEDIMENTI"))
            MyDBEngine.OpenConnection()

            If TipoRicerca = "D" Then
                sProcedureName = "prc_GetDichiaratoVSAgevolazione"
            Else
                sProcedureName = "prc_GetAccertatoVSAgevolazione"
            End If
            Dim dtArticoli As New DataTable
            MyDBEngine.ClearParameters()
            MyDBEngine.AddParameter("@IdProvvedimento", nIdProvvedimento, ParameterDirection.Input)
            MyDBEngine.ExecuteQuery(dtArticoli, sProcedureName, CommandType.StoredProcedure)
            Return dtArticoli
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.OSAPGetAgevolazioniPerStampa.errore: ", Err)
            Return Nothing
        End Try
    End Function
    '*** ***
    Public Function AggMassivoDate(ByVal myStringConnection As String, ParamSearch As ObjSearchAtti, CampoAgg As Integer, DataAgg As String) As Boolean
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_SetMassivoDate"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.VarChar)).Value = ParamSearch.IdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FILTERPARAM", SqlDbType.VarChar)).Value = New ClsRicercaAtti().generaSQLRicercaAvanzata(ParamSearch)
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CAMPO", SqlDbType.Int)).Value = CampoAgg
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA", SqlDbType.VarChar)).Value = DataAgg
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsGestioneAccertamenti.AggMassivoDate.errore: ", ex)
            Return False
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function
End Class
''' <summary>
''' Classe per la ricerca avanzata degli atti
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsRicercaAtti
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsRicercaAtti))
    Public Function generaSQLRicercaAvanzata(ByVal mySearch As ComPlusInterface.ObjSearchAtti) As String
        Dim sSQL As String
        Try
            sSQL = getStringSQLRicercaAvanzata(mySearch.Generazione.TipoRic, mySearch.Generazione.Dal, mySearch.Generazione.Al, "DATA_ELABORAZIONE")
            sSQL += getStringSQLRicercaAvanzata(mySearch.ConfermaAvviso.TipoRic, mySearch.ConfermaAvviso.Dal, mySearch.ConfermaAvviso.Al, "DATA_CONFERMA")
            sSQL += getStringSQLRicercaAvanzata(mySearch.StampaAvviso.TipoRic, mySearch.StampaAvviso.Dal, mySearch.StampaAvviso.Al, "DATA_STAMPA")
            sSQL += getStringSQLRicercaAvanzata(mySearch.ConsegnaAvviso.TipoRic, mySearch.ConsegnaAvviso.Dal, mySearch.ConsegnaAvviso.Al, "DATA_CONSEGNA_AVVISO")
            sSQL += getStringSQLRicercaAvanzata(mySearch.NotificaAvviso.TipoRic, mySearch.NotificaAvviso.Dal, mySearch.NotificaAvviso.Al, "DATA_NOTIFICA_AVVISO")
            sSQL += getStringSQLRicercaAvanzata(mySearch.RettificaAvviso.TipoRic, mySearch.RettificaAvviso.Dal, mySearch.RettificaAvviso.Al, "DATA_RETTIFICA_AVVISO")
            sSQL += getStringSQLRicercaAvanzata(mySearch.AnnullamentoAvviso.TipoRic, mySearch.AnnullamentoAvviso.Dal, mySearch.AnnullamentoAvviso.Al, "DATA_ANNULLAMENTO_AVVISO")
            sSQL += getStringSQLRicercaAvanzata(mySearch.SospensioneAutotutela.TipoRic, mySearch.SospensioneAutotutela.Dal, mySearch.SospensioneAutotutela.Al, "DATA_SOSPENSIONE_AVVISO_AUTOTUTELA")
            sSQL += getStringSQLRicercaAvanzata(mySearch.AttoDefinitivo.TipoRic, mySearch.AttoDefinitivo.Dal, mySearch.AttoDefinitivo.Al, "DATA_ATTO_DEFINITIVO")
            sSQL += getStringSQLRicercaAvanzata(mySearch.Pagamento.TipoRic, mySearch.Pagamento.Dal, mySearch.Pagamento.Al, "DATA_PAGAMENTO")
            sSQL += getStringSQLRicercaAvanzata(mySearch.SollecitoBonario.TipoRic, mySearch.SollecitoBonario.Dal, mySearch.SollecitoBonario.Al, "DATA_SOLLECITO_BONARIO")
            sSQL += getStringSQLRicercaAvanzata(mySearch.Coattivo.TipoRic, mySearch.Coattivo.Dal, mySearch.Coattivo.Al, "DATA_COATTIVO")
        Catch ex As Exception
            Log.Debug("ClsRicercaAtti.generaSQLRicercaAvanzata.errore::", ex)
            sSQL = ""
        End Try
        Return sSQL
    End Function
    Private Function getStringSQLRicercaAvanzata(TipoRicerca As Integer, ByVal Dal As Date, ByVal Al As Date, ByVal FieldName As String) As String
        Dim SQL As String = ""
        Dim oReplace As New OPENgovTIA.generalClass.generalFunction

        Try
            If TipoRicerca = ComPlusInterface.ObjSearchAttiAvanzataDate.DateNessuna Then
                '*****************************NESSUNA DATA*************************************
                SQL += " AND (" & FieldName & " IS NULL OR " & FieldName & " = ''" & ")"
            Else
                If Dal = Date.MaxValue And Al <> Date.MaxValue Then
                    SQL += " AND (" & FieldName & "<='" & oReplace.FormattaData(Al, "A") & "')"
                End If
                If Dal <> Date.MaxValue And Al <> Date.MaxValue Then
                    SQL += " AND (" & FieldName & ">='" & oReplace.FormattaData(Dal, "A") & "' AND " & FieldName & "<='" & oReplace.FormattaData(Al, "A") & "')"
                End If
                If Dal <> Date.MaxValue And Al = Date.MaxValue Then
                    SQL += " AND (" & FieldName & ">='" & oReplace.FormattaData(Dal, "A") & "')"
                End If
            End If
        Catch ex As Exception
            Log.Debug("ClsRicercaAtti.getStringSQLRicercaAvanzata.errore::", ex)
            SQL = ""
        End Try
        Return SQL
    End Function
    Public Function GetExportAtti(myStringConnection As String, IdEnte As String) As DataView
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetExportAtti"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IDENTE", IdEnte)
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            Return dtMyDati.DefaultView
        Catch Err As Exception
            Log.Debug(IdEnte + " - OPENgovPROVVEDIMENTI.ClsRicercaAtti.GetExportAtti.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
End Class
'*** 201810 - Generazione Massiva Atti ***
''' <summary>
''' Classe per la generazione massiva degli atti di accertamento di confronto fra dichiarato e versato
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsMassiva
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsMassiva))
    Private _IdEnte As String
    Private _DBType As String
    Private _StringConnection As String
    Private _StringConnectionOPENgov As String
    Private _StringConnectionAnagrafe As String
    Private _StringConnectionICI As String
    Private _StringConnectionOSAP As String
    Private _StringConnectionTARSU As String
    Private _Operatore As String
    Private _Anno As Integer
    Private _ListTributi As String
    Private _Soglia As Double
    Private _GGScadenza As Integer
    Private _cmdMyCommand As SqlClient.SqlCommand
    Private _cmdMyCommandNoTrans As SqlClient.SqlCommand
    Private _myTrans As SqlClient.SqlTransaction

    Public Function GetAtti(ByVal myStringConnection As String, ByVal IdEnte As String) As OggettoAtto()
        Dim myAtto As OggettoAtto
        Dim listAtti As New ArrayList
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetAttiMassiviDaConfermare"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = IdEnte
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            For Each dtMyRow In dtMyDati.Rows
                myAtto = New OggettoAtto
                myAtto.ID_PROVVEDIMENTO = CInt(dtMyRow("ID_PROVVEDIMENTO"))
                myAtto.COD_ENTE = CStr(dtMyRow("COD_ENTE"))
                myAtto.COD_CONTRIBUENTE = CInt(dtMyRow("COD_CONTRIBUENTE"))
                myAtto.CODICE_FISCALE = CStr(dtMyRow("CODICE_FISCALE"))
                myAtto.PARTITA_IVA = CStr(dtMyRow("PARTITA_IVA"))
                myAtto.DescrTributo = CStr(dtMyRow("descrtributo"))
                myAtto.ANNO = CStr(dtMyRow("ANNO"))
                myAtto.DATA_ELABORAZIONE = CStr(dtMyRow("DATA_ELABORAZIONE"))
                myAtto.NOTE_ACCERTAMENTO = CStr(dtMyRow("NOTE_ACCERTAMENTO"))
                myAtto.IMPORTO_DIFFERENZA_IMPOSTA = CDbl(dtMyRow("IMPORTO_DIFFERENZA_IMPOSTA"))
                myAtto.IMPORTO_SANZIONI = CDbl(dtMyRow("IMPORTO_SANZIONI"))
                myAtto.IMPORTO_INTERESSI = CDbl(dtMyRow("IMPORTO_INTERESSI"))
                myAtto.IMPORTO_ALTRO = CDbl(dtMyRow("IMPORTO_ALTRO"))
                myAtto.IMPORTO_SPESE = CDbl(dtMyRow("IMPORTO_SPESE"))
                myAtto.IMPORTO_TOTALE = CDbl(dtMyRow("IMPORTO_TOTALE"))
                listAtti.Add(myAtto)
            Next

            Return CType(listAtti.ToArray(GetType(OggettoAtto)), OggettoAtto())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ClsMassiva.GetAtti.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
#Region "Elaborazione"
    Public Sub StartElaborazione(TypeDB As String, ByVal myStringConnection As String, ByVal myStringConnectionOPENgov As String, ByVal myStringConnectionAnagrafe As String, ByVal myStringConnectionICI As String, ByVal myStringConnectionTARSU As String, ByVal myStringConnectionOSAP As String, ByVal IdEnte As String, Operatore As String, Anno As String, Tributi As String, Soglia As Double, GGScadenza As Integer)
        Dim threadDelegate As Threading.ThreadStart = New Threading.ThreadStart(AddressOf StartMassiva)
        Dim t As Threading.Thread = New Threading.Thread(threadDelegate)
        _DBType = TypeDB
        _StringConnection = myStringConnection
        _StringConnectionOPENgov = myStringConnectionOPENgov
        _StringConnectionAnagrafe = myStringConnectionAnagrafe
        _StringConnectionICI = myStringConnectionICI
        _StringConnectionTARSU = myStringConnectionTARSU
        _StringConnectionOSAP = myStringConnectionOSAP
        _IdEnte = IdEnte
        _Operatore = Operatore
        _Anno = Anno
        _ListTributi = Tributi
        _Soglia = Soglia
        _GGScadenza = GGScadenza

        'Inizio la transazione e recupero l'istanza della connessione al db
        'Devo farlo qui visto che sto eseguendo codice ancora nel thread della PostBack da FE, per cui ho il HttpContext disponibile con tutti i suoi oggetti (DichiarazioneSession, HttpSession, ecc.), mentre nel thread non ho queste informazioni
        _cmdMyCommand = StartCalcoloMassivoTransaction()
        _cmdMyCommandNoTrans = OpenCalcoloMassivoConnection()
        _myTrans = _cmdMyCommand.Transaction
        t.Start()
    End Sub
    Private Sub StartMassiva()
        CacheManager.SetElaborazioneMassivaInCorso(_Anno)

        Try
            If CalcoloAtti() = True Then
                RollbackCalcoloMassivoTransaction(_myTrans, _cmdMyCommand)
                CacheManager.RemoveRiepilogoElaborazioneMassiva()
            Else
                CommitCalcoloMassivoTransaction(_myTrans, _cmdMyCommand)
            End If
        Catch Err As Exception
            Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovPROVVEDIMENTI.ClsMassiva.StartMassiva.errore: ", Err)
            RollbackCalcoloMassivoTransaction(_myTrans, _cmdMyCommand)
            CacheManager.RemoveRiepilogoElaborazioneMassiva()
        Finally
            CacheManager.RemoveElaborazioneMassivaInCorso()
            CacheManager.RemoveAvanzamentoElaborazioneMassiva()
        End Try
    End Sub
    Private Function CalcoloAtti() As Boolean
        Dim HasError As Boolean = False
        Dim listToCalc As ArrayList
        Dim dtMyDati As New DataTable()
        Dim sAvanzamento As String
        Dim nAvanzamento As Integer = 0

        Try
            listToCalc = GetContribToCalc(_StringConnection, _IdEnte, _Anno, _ListTributi, _Soglia, _GGScadenza)
            If Not listToCalc Is Nothing Then
                For Each myItem As OggettoAtto In CType(listToCalc.ToArray(GetType(OggettoAtto)), OggettoAtto())
                    Select Case myItem.COD_TRIBUTO
                        Case Utility.Costanti.TRIBUTO_ICI, Utility.Costanti.TRIBUTO_TASI
                            If GenAtto8852TASI(myItem.COD_CONTRIBUENTE, myItem.COD_TRIBUTO) = False Then
                                Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovPROVVEDIMENTI.ClsMassiva.CalcoloAtti.errore in calcolo " + myItem.COD_TRIBUTO + " contribuente->" + myItem.COD_CONTRIBUENTE.ToString)
                            End If
                        Case Utility.Costanti.TRIBUTO_TARSU
                            If GenAtto0434(myItem.COD_CONTRIBUENTE) = False Then
                                Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovPROVVEDIMENTI.ClsMassiva.CalcoloAtti.errore in calcolo " + myItem.COD_TRIBUTO + " contribuente->" + myItem.COD_CONTRIBUENTE.ToString)
                            End If
                        Case Utility.Costanti.TRIBUTO_OSAP
                            If GenAtto0453(myItem.COD_CONTRIBUENTE) = False Then
                                Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovPROVVEDIMENTI.ClsMassiva.CalcoloAtti.errore in calcolo " + myItem.COD_TRIBUTO + " contribuente->" + myItem.COD_CONTRIBUENTE.ToString)
                            End If
                        Case Else
                    End Select
                    nAvanzamento += 1
                    sAvanzamento = "Elaborazione posizione " & nAvanzamento & " su " & listToCalc.Count
                    CacheManager.SetAvanzamentoElaborazioneMassiva(sAvanzamento)
                Next
            Else
                HasError = True
            End If
        Catch ex As Exception
            Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovPROVVEDIMENTI.ClsMassiva.CalcoloAtti.errore: ", ex)
            HasError = True
        End Try
        Return HasError
    End Function
    Private Function GetContribToCalc(ByVal myStringConnection As String, IdEnte As String, Anno As String, ListTributi As String, Soglia As Double, GGScadenza As Integer) As ArrayList
        Dim listToCalc As New ArrayList
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetAttiMassiviDaCalcolare"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = IdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.VarChar)).Value = Anno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ListTributi", SqlDbType.VarChar)).Value = ListTributi
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@GGScadenza", SqlDbType.Int)).Value = GGScadenza
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Soglia", SqlDbType.Float)).Value = Soglia
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            For Each dtMyRow In dtMyDati.Rows
                Dim myAtto As New OggettoAtto
                myAtto.COD_CONTRIBUENTE = CInt(dtMyRow("COD_CONTRIBUENTE"))
                myAtto.COD_TRIBUTO = CStr(dtMyRow("COD_TRIBUTO"))
                listToCalc.Add(myAtto)
            Next
        Catch Err As Exception
            Log.Debug(IdEnte + " - OPENgovPROVVEDIMENTI.ClsMassiva.GetContribToCalc.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            listToCalc = New ArrayList
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
        Return listToCalc
    End Function
    ''' <summary>
    ''' Funzione per la generazione automatica di un accertamento IMU/TASI
    ''' </summary>
    ''' <param name="IdContribuente"></param>
    ''' <param name="Tributo"></param>
    ''' <returns></returns>
    Private Function GenAtto8852TASI(IdContribuente As Integer, Tributo As String) As Boolean
        Dim myRet As Boolean = False
        Dim ListDichiarato() As objUIICIAccert
        Dim ListAccertato() As objUIICIAccert
        Dim ListSituazioneFinale() As objSituazioneFinale = Nothing
        Dim objHashTable As New Hashtable
        Dim myArray As New ArrayList
        Dim dsDettaglioAnagrafica As DataSet = Nothing
        Dim oDettaglioAnagrafica As New DettaglioAnagrafica
        Dim dsRiepilogoFase2 As New ObjBaseIntSanz
        Dim dsSanzioniFase2 As New DataSet
        Dim ListInteressiFase2() As ObjInteressiSanzioni
        Dim dsVersamentiF2 As New DataSet
        Dim TotVersamenti As Double
        Dim TipoAvviso As String = OggettoAtto.Provvedimento.NoAvviso

        Try
            If objHashTable.Contains("TIPO_OPERAZIONE") = False Then
                objHashTable.Add("TIPO_OPERAZIONE", OggettoAtto.Procedimento.Accertamento)
            Else
                objHashTable("TIPO_OPERAZIONE") = OggettoAtto.Procedimento.Accertamento
            End If

            'Recupero la hash table
            objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

            'Aggiungo gli altri campi nella hash table
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", _StringConnection)
            objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", _StringConnectionICI)
            objHashTable.Add("USER", _Operatore)
            objHashTable.Add("CODENTE", _IdEnte)
            objHashTable.Add("CodENTE", _IdEnte)
            objHashTable.Add("CODTIPOPROCEDIMENTO", "L")
            objHashTable.Add("TASIAPROPRIETARIO", 0)
            '*** 20140509 - TASI ***
            If objHashTable.Contains("URLServiziFreezer") = False Then
                objHashTable.Add("URLServiziFreezer", ConstSession.URLServiziFreezer)
            Else
                objHashTable("URLServiziFreezer") = ConstSession.URLServiziFreezer
            End If
            If objHashTable.Contains("CONFIGURAZIONE_DICHIARAZIONE") = False Then
                objHashTable.Add("CONFIGURAZIONE_DICHIARAZIONE", ConstSession.CONFIGURAZIONE_DICHIARAZIONE)
            Else
                objHashTable("CONFIGURAZIONE_DICHIARAZIONE") = ConstSession.CONFIGURAZIONE_DICHIARAZIONE
            End If
            If objHashTable.Contains("COD_TRIBUTO") = False Then
                objHashTable.Add("COD_TRIBUTO", Tributo)
                objHashTable.Add("CODTRIBUTO", Tributo)
            Else
                objHashTable("COD_TRIBUTO") = Tributo
                objHashTable("CODTRIBUTO") = Tributo
            End If
            If objHashTable.Contains("TRIBUTOCALCOLO") = False Then
                objHashTable.Add("TRIBUTOCALCOLO", Tributo)
            Else
                objHashTable("TRIBUTOCALCOLO") = Tributo
            End If
            If objHashTable.Contains("CODCONTRIBUENTE") = False Then
                objHashTable.Add("CODCONTRIBUENTE", IdContribuente)
            Else
                objHashTable("CODCONTRIBUENTE") = IdContribuente
            End If
            If objHashTable.Contains("ANNOACCERTAMENTO") = False Then
                objHashTable.Add("ANNOACCERTAMENTO", _Anno)
            Else
                objHashTable("ANNOACCERTAMENTO") = _Anno
            End If
            If objHashTable.Contains("ANNODA") = False Then
                objHashTable.Add("ANNODA", _Anno)
            Else
                objHashTable("ANNODA") = _Anno
            End If
            If objHashTable.Contains("ANNOA") = False Then
                objHashTable.Add("ANNOA", -1)
            Else
                objHashTable("ANNOA") = -1
            End If
            If objHashTable.Contains("CONNECTIONSTRINGOPENGOV") = False Then
                objHashTable.Add("CONNECTIONSTRINGOPENGOV", _StringConnectionOPENgov)
            Else
                objHashTable("CONNECTIONSTRINGOPENGOV") = _StringConnectionOPENgov
            End If
            If objHashTable.Contains("DBType") = False Then
                objHashTable.Add("DBType", _DBType)
            Else
                objHashTable("DBType") = _DBType
            End If
            If objHashTable.Contains("CONNECTIONSTRINGANAGRAFICA") = False Then
                objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", _StringConnectionAnagrafe)
            Else
                objHashTable("CONNECTIONSTRINGANAGRAFICA") = _StringConnectionAnagrafe
            End If
            Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
            ListDichiarato = objCOMDichiarazioniAccertamenti.GetDatiAccertamenti(_StringConnectionICI, _StringConnectionOPENgov, _IdEnte, IdContribuente, objHashTable, ListSituazioneFinale)
            For Each CurrentItem As objUIICIAccert In ListDichiarato
                myArray.Add(CurrentItem)
            Next
            ListAccertato = CType(myArray.ToArray(GetType(objUIICIAccert)), objUIICIAccert())

            oDettaglioAnagrafica = New Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(IdContribuente, -1, "", _DBType, _StringConnectionAnagrafe, False)
            If Not IsNothing(oDettaglioAnagrafica) Then
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - CodContribuente:" & oDettaglioAnagrafica.COD_CONTRIBUENTE & " - Cognome: " & oDettaglioAnagrafica.Cognome & " - Nome:" & oDettaglioAnagrafica.Nome)
                dsDettaglioAnagrafica = New ClsGestioneAccertamenti().addRowsObjAnagrafica(oDettaglioAnagrafica)
                Dim ImpDichAcconto, ImpDichSaldo, ImpDichTotale As Double
                ImpDichAcconto = 0 : ImpDichSaldo = 0 : ImpDichTotale = 0
                For Each myUI As objUIICIAccert In ListDichiarato
                    If myUI.Anno = _Anno Then
                        ImpDichAcconto += myUI.AccDovuto
                        ImpDichSaldo += myUI.SalDovuto
                        ImpDichTotale += myUI.TotDovuto
                    End If
                Next
                If New ClsGestioneAccertamenti().CalcoloPreAccertamento(_IdEnte, Utility.Costanti.TRIBUTO_ICI, Tributo, _Anno, IdContribuente, -1, DateTime.Now.ToShortDateString, dsDettaglioAnagrafica, ImpDichAcconto, ImpDichSaldo, ImpDichTotale, dsRiepilogoFase2, TotVersamenti, dsSanzioniFase2, ListInteressiFase2, dsVersamentiF2) = False Then
                    Throw New Exception("errore in calcolopreaccertamento")
                End If

                If Not dsRiepilogoFase2 Is Nothing Then
                    TipoAvviso = dsRiepilogoFase2.COD_TIPO_PROVVEDIMENTO
                Else
                    TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
                End If
                'Calcolo le spese
                Dim impSpese As Double = New DBPROVVEDIMENTI.ProvvedimentiDB().GetSpese(_Anno, Utility.Costanti.TRIBUTO_ICI, _IdEnte, TipoAvviso)
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Spese:" & impSpese & " €")
                'Aggiorno il DB dopo procedura di accertamento
                If objHashTable.Contains("TIPOPROVVEDIMENTO") = False Then
                    objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
                Else
                    objHashTable("TIPOPROVVEDIMENTO") = TipoAvviso
                End If
                If TipoAvviso <> OggettoAtto.Provvedimento.NoAvviso Then
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Chiamo updateDBAccertamenti")
                    'Inserisco i dati dell'accertamento nel database
                    Dim objCOMUpdateDBAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
                    If objCOMUpdateDBAccertamenti.updateDBAccertamenti(ConstSession.DBType, ConstSession.StringConnection, ConstSession.IdEnte, IdContribuente, objHashTable, dsRiepilogoFase2, Nothing, Nothing, impSpese, ListSituazioneFinale, ListDichiarato, ListAccertato, dsSanzioniFase2, ListInteressiFase2, dsVersamentiF2, ConstSession.UserName) < 1 Then
                        Throw New Exception("Errore in inserimento avviso")
                    End If
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Terminata updateDBAccertamenti")
                Else
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - NON chiamo updateDBAccertamenti perchè NESSUN_AVVISO")
                End If
                myRet = True
            Else
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - CodContribuente: " + IdContribuente.ToString + " Non disponibile")
                myRet = False
            End If
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovPROVVEDIMENTI.ClsMassiva.GenAtto8852TASI.errore: ", ex)
            myRet = False
        End Try
        Return myRet
    End Function
    'Private Function GenAtto8852TASI(IdContribuente As Integer, Tributo As String) As Boolean
    '    Dim myRet As Boolean = False
    '    Dim ListDichiarato() As objUIICIAccert
    '    Dim ListAccertato() As objUIICIAccert
    '    Dim ListSituazioneFinale() As objSituazioneFinale = Nothing
    '    Dim objHashTable As New Hashtable
    '    Dim myArray As New ArrayList
    '    Dim dsDettaglioAnagrafica As DataSet = Nothing
    '    Dim oDettaglioAnagrafica As New DettaglioAnagrafica
    '    Dim dsRiepilogoFase2 As New DataSet
    '    Dim dsSanzioniFase2 As New DataSet
    '    Dim ListInteressiFase2() As ObjInteressiSanzioni
    '    Dim dsVersamentiF2 As New DataSet
    '    Dim TotVersamenti As Double
    '    Dim TipoAvviso As String = OggettoAtto.Provvedimento.NoAvviso

    '    Try
    '        If objHashTable.Contains("TIPO_OPERAZIONE") = False Then
    '            objHashTable.Add("TIPO_OPERAZIONE", OggettoAtto.Procedimento.Accertamento)
    '        Else
    '            objHashTable("TIPO_OPERAZIONE") = OggettoAtto.Procedimento.Accertamento
    '        End If

    '        'Recupero la hash table
    '        objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '        'Aggiungo gli altri campi nella hash table
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", _StringConnection)
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", _StringConnectionICI)
    '        objHashTable.Add("USER", _Operatore)
    '        objHashTable.Add("CODENTE", _IdEnte)
    '        objHashTable.Add("CodENTE", _IdEnte)
    '        objHashTable.Add("CODTIPOPROCEDIMENTO", "L")
    '        objHashTable.Add("TASIAPROPRIETARIO", 0)
    '        '*** 20140509 - TASI ***
    '        If objHashTable.Contains("URLServiziFreezer") = False Then
    '            objHashTable.Add("URLServiziFreezer", ConstSession.URLServiziFreezer)
    '        Else
    '            objHashTable("URLServiziFreezer") = ConstSession.URLServiziFreezer
    '        End If
    '        If objHashTable.Contains("CONFIGURAZIONE_DICHIARAZIONE") = False Then
    '            objHashTable.Add("CONFIGURAZIONE_DICHIARAZIONE", ConstSession.CONFIGURAZIONE_DICHIARAZIONE)
    '        Else
    '            objHashTable("CONFIGURAZIONE_DICHIARAZIONE") = ConstSession.CONFIGURAZIONE_DICHIARAZIONE
    '        End If
    '        If objHashTable.Contains("COD_TRIBUTO") = False Then
    '            objHashTable.Add("COD_TRIBUTO", Tributo)
    '            objHashTable.Add("CODTRIBUTO", Tributo)
    '        Else
    '            objHashTable("COD_TRIBUTO") = Tributo
    '            objHashTable("CODTRIBUTO") = Tributo
    '        End If
    '        If objHashTable.Contains("TRIBUTOCALCOLO") = False Then
    '            objHashTable.Add("TRIBUTOCALCOLO", Tributo)
    '        Else
    '            objHashTable("TRIBUTOCALCOLO") = Tributo
    '        End If
    '        If objHashTable.Contains("CODCONTRIBUENTE") = False Then
    '            objHashTable.Add("CODCONTRIBUENTE", IdContribuente)
    '        Else
    '            objHashTable("CODCONTRIBUENTE") = IdContribuente
    '        End If
    '        If objHashTable.Contains("ANNOACCERTAMENTO") = False Then
    '            objHashTable.Add("ANNOACCERTAMENTO", _Anno)
    '        Else
    '            objHashTable("ANNOACCERTAMENTO") = _Anno
    '        End If
    '        If objHashTable.Contains("ANNODA") = False Then
    '            objHashTable.Add("ANNODA", _Anno)
    '        Else
    '            objHashTable("ANNODA") = _Anno
    '        End If
    '        If objHashTable.Contains("ANNOA") = False Then
    '            objHashTable.Add("ANNOA", -1)
    '        Else
    '            objHashTable("ANNOA") = -1
    '        End If
    '        If objHashTable.Contains("CONNECTIONSTRINGOPENGOV") = False Then
    '            objHashTable.Add("CONNECTIONSTRINGOPENGOV", _StringConnectionOPENgov)
    '        Else
    '            objHashTable("CONNECTIONSTRINGOPENGOV") = _StringConnectionOPENgov
    '        End If
    '        If objHashTable.Contains("DBType") = False Then
    '            objHashTable.Add("DBType", _DBType)
    '        Else
    '            objHashTable("DBType") = _DBType
    '        End If
    '        If objHashTable.Contains("CONNECTIONSTRINGANAGRAFICA") = False Then
    '            objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", _StringConnectionAnagrafe)
    '        Else
    '            objHashTable("CONNECTIONSTRINGANAGRAFICA") = _StringConnectionAnagrafe
    '        End If
    '        Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '        ListDichiarato = objCOMDichiarazioniAccertamenti.GetDatiAccertamenti(objHashTable, ListSituazioneFinale)
    '        For Each CurrentItem As objUIICIAccert In ListDichiarato
    '            myArray.Add(CurrentItem)
    '        Next
    '        ListAccertato = CType(myArray.ToArray(GetType(objUIICIAccert)), objUIICIAccert())

    '        oDettaglioAnagrafica = New Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(IdContribuente, -1, "", _DBType, _StringConnectionAnagrafe, False)
    '        If Not IsNothing(oDettaglioAnagrafica) Then
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - CodContribuente:" & oDettaglioAnagrafica.COD_CONTRIBUENTE & " - Cognome: " & oDettaglioAnagrafica.Cognome & " - Nome:" & oDettaglioAnagrafica.Nome)
    '            dsDettaglioAnagrafica = New ClsGestioneAccertamenti().addRowsObjAnagrafica(oDettaglioAnagrafica)
    '            Dim ImpDichAcconto, ImpDichSaldo, ImpDichTotale As Double
    '            ImpDichAcconto = 0 : ImpDichSaldo = 0 : ImpDichTotale = 0
    '            For Each myUI As objUIICIAccert In ListDichiarato
    '                If myUI.Anno = _Anno Then
    '                    ImpDichAcconto += myUI.AccDovuto
    '                    ImpDichSaldo += myUI.SalDovuto
    '                    ImpDichTotale += myUI.TotDovuto
    '                End If
    '            Next
    '            If New ClsGestioneAccertamenti().CalcoloPreAccertamento(_IdEnte, Utility.Costanti.TRIBUTO_ICI, Tributo, _Anno, IdContribuente, -1, DateTime.Now.ToShortDateString, dsDettaglioAnagrafica, ImpDichAcconto, ImpDichSaldo, ImpDichTotale, dsRiepilogoFase2, TotVersamenti, dsSanzioniFase2, ListInteressiFase2, dsVersamentiF2) = False Then
    '                Throw New Exception("errore in calcolopreaccertamento")
    '            End If

    '            If Not dsRiepilogoFase2 Is Nothing Then
    '                If dsRiepilogoFase2.Tables.Count > 0 Then
    '                    If dsRiepilogoFase2.Tables(0).Rows.Count > 0 Then
    '                        TipoAvviso = dsRiepilogoFase2.Tables(0).Rows(0)("TIPO_PROVVEDIMENTO").ToString
    '                    End If
    '                Else
    '                    TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
    '                End If
    '            Else
    '                TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
    '            End If
    '            'Calcolo le spese
    '            Dim impSpese As Double = New DBPROVVEDIMENTI.ProvvedimentiDB().GetSpese(_Anno, Utility.Costanti.TRIBUTO_ICI, _IdEnte, TipoAvviso)
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Spese:" & impSpese & " €")
    '            'Aggiorno il DB dopo procedura di accertamento
    '            If objHashTable.Contains("TIPOPROVVEDIMENTO") = False Then
    '                objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
    '            Else
    '                objHashTable("TIPOPROVVEDIMENTO") = TipoAvviso
    '            End If
    '            If TipoAvviso <> OggettoAtto.Provvedimento.NoAvviso Then
    '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Chiamo updateDBAccertamenti")
    '                'Inserisco i dati dell'accertamento nel database
    '                Dim objCOMUpdateDBAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '                If objCOMUpdateDBAccertamenti.updateDBAccertamenti(ConstSession.DBType, objHashTable, dsRiepilogoFase2, Nothing, Nothing, impSpese, ListSituazioneFinale, ListDichiarato, ListAccertato, dsSanzioniFase2, ListInteressiFase2, dsVersamentiF2, ConstSession.UserName) < 1 Then
    '                    Throw New Exception("Errore in inserimento avviso")
    '                End If
    '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Terminata updateDBAccertamenti")
    '            Else
    '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - NON chiamo updateDBAccertamenti perchè NESSUN_AVVISO")
    '            End If
    '            myRet = True
    '        Else
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - CodContribuente: " + IdContribuente.ToString + " Non disponibile")
    '            myRet = False
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovPROVVEDIMENTI.ClsMassiva.GenAtto8852TASI.errore: ", ex)
    '        myRet = False
    '    End Try
    '    Return myRet
    'End Function
    ''' <summary>
    ''' Funzione per la generazione automatica di un accertamento TARSU
    ''' </summary>
    ''' <param name="IdContribuente"></param>
    ''' <returns></returns>
    Private Function GenAtto0434(IdContribuente As Integer) As Boolean
        Dim myRet As Boolean = False
        Dim intProgressivo As Integer = 0
        Dim myArray As New ArrayList
        Dim ListAvvisi() As ObjAvviso
        Dim ListDichiarato() As ObjArticoloAccertamento
        Dim ListAccertato() As ObjArticoloAccertamento
        Dim sDescTipoAvviso, sScript As String
        Dim myAtto As New OggettoAttoTARSU
        Dim TipoTassazione As String
        Dim HasMaggiorazione As Boolean

        Try
            If _Anno > 2012 Then
                TipoTassazione = ObjRuolo.TipoCalcolo.TARES
            Else
                TipoTassazione = ObjRuolo.TipoCalcolo.TARSU
            End If
            If _Anno = 2013 Then
                HasMaggiorazione = True
            Else
                HasMaggiorazione = False
            End If

            Dim myHashTable As New Hashtable
            myHashTable.Add("CODCONTRIBUENTE", IdContribuente)
            myHashTable.Add("ANNOACCERTAMENTO", _Anno)
            '*** 20140701 - IMU/TARES ***
            myHashTable.Add("TipoTassazione", TipoTassazione)
            myHashTable.Add("TipoCalcolo", ObjRuolo.Ruolo.APercentuale)
            myHashTable.Add("DescrTipoCalcolo", "")
            myHashTable.Add("PercentTariffe", 100)
            myHashTable.Add("HasMaggiorazione", HasMaggiorazione)
            myHashTable.Add("HasConferimenti", False)
            myHashTable.Add("TipoMQ", "D")
            myHashTable.Add("impSogliaAvvisi", 0)
            myHashTable.Add("IdTestata", -1)
            '*** 20181011 Dal/Al Conferimenti ***
            myHashTable.Add("tDataInizioConf", New OPENgovTIA.generalClass.generalFunction().FormattaData(_Anno.ToString + "0101", "G"))
            myHashTable.Add("tDataFineConf", New OPENgovTIA.generalClass.generalFunction().FormattaData(_Anno.ToString + "1231", "G"))

            ListAvvisi = New OPENgovTIA.GestAvviso().GetAvviso(_StringConnectionTARSU, -1, _IdEnte, -1, _Anno, "", "", "", "", "", "", "", "", True, False, False, False, "", IdContribuente, Nothing)
            If Not ListAvvisi Is Nothing Then
                For Each myAvviso As ObjAvviso In ListAvvisi
                    If Not myAvviso.oArticoli Is Nothing Then
                        For Each myArticolo As ObjArticolo In myAvviso.oArticoli
                            Dim FncAcc As New ClsGestioneAccertamenti
                            Dim CurrentItem As ObjArticoloAccertamento = FncAcc.ArticoloTOArticoloAccertamento(myArticolo, False)
                            If Not CurrentItem Is Nothing Then
                                CurrentItem.IdEnte = ConstSession.IdEnte
                                CurrentItem.sNote = TipoTassazione
                                CurrentItem.Progressivo = intProgressivo + 1
                                intProgressivo += 1
                                CurrentItem.IdLegame = CurrentItem.Progressivo
                                myArray.Add(CurrentItem)
                            Else
                                Throw New Exception("errore in caricamento articolo")
                            End If
                        Next
                    End If
                    ListDichiarato = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())

                    For Each CurrentItem As ObjArticoloAccertamento In ListDichiarato
                        CurrentItem.Calcola_Interessi = True
                    Next
                    ListAccertato = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())

                    myAtto = New ClsGestioneAccertamenti().TARSUConfrontoAccertatoDichiarato(TipoTassazione, _IdEnte, _Anno, ListDichiarato, ListAccertato, False, myHashTable, False, New DataSet, True, "", sDescTipoAvviso, sScript)
                    If IsNothing(myAtto) Then
                        Throw New Exception("Errore in calcolo accertamento")
                    End If
                Next
            End If
            myRet = True
        Catch ex As Exception
            Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovPROVVEDIMENTI.ClsMassiva.GenAtto0434.errore: ", ex)
            myRet = False
        End Try
        Return myRet
    End Function
    Private Function GenAtto0453(IdContribuente As Integer) As Boolean
        Dim myRet As Boolean = False
        Dim myArray As New ArrayList
        Dim ListDichiarato() As OSAPAccertamentoArticolo
        Dim ListAccertato() As OSAPAccertamentoArticolo
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim FncAcc As New ClsGestioneAccertamenti
        Dim ListCateg() As IRemInterfaceOSAP.Categorie
        Dim ListTipiOcc() As IRemInterfaceOSAP.TipologieOccupazioni
        Dim ListAgevolazioni() As IRemInterfaceOSAP.Agevolazione
        Dim ListTariffe() As IRemInterfaceOSAP.Tariffe
        Dim myHashTable As Hashtable
        Dim MyMotore As IRemInterfaceOSAP.IRemotingInterfaceOSAP
        Dim ListIdDichToExam() As Integer
        Dim Dichiarazione As IRemInterfaceOSAP.DichiarazioneTosapCosap
        Dim nList As Integer = 0
        Dim myAtto As New OggettoAttoOSAP
        Dim sDescTipoAvviso, sScript As String

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnectionOSAP)
            cmdMyCommand.Connection.Open()

            'carico i parametri per il calcolo
            FncAcc.OSAPLoadParamCalcolo(_Anno, ListCateg, ListTipiOcc, ListAgevolazioni, ListTariffe, ConstSession.IdEnte)

            If myHashTable.ContainsKey("IDSOTTOAPPLICAZIONEICI") = False Then
                myHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVTA"))
            Else
                myHashTable("IDSOTTOAPPLICAZIONEICI") = ConfigurationManager.AppSettings("OPENGOVTA")
            End If
            'Aggiungo gli altri campi nella hash table
            If myHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = False Then
                myHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", _StringConnection)
            Else
                myHashTable("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = _StringConnection
            End If
            If myHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVOSAP") = False Then
                myHashTable.Add("CONNECTIONSTRINGOPENGOVOSAP", _StringConnectionOSAP)
            Else
                myHashTable("CONNECTIONSTRINGOPENGOVOSAP") = _StringConnectionOSAP
            End If
            If myHashTable.ContainsKey("USER") = False Then
                myHashTable.Add("USER", _Operatore)
            Else
                myHashTable("USER") = _Operatore
            End If
            If myHashTable.ContainsKey("CODENTE") = False Then
                myHashTable.Add("CODENTE", _IdEnte)
            Else
                myHashTable("CODENTE") = _IdEnte
            End If
            If myHashTable.ContainsKey("CodENTE") = False Then
                myHashTable.Add("CodENTE", _IdEnte)
            Else
                myHashTable("CodENTE") = _IdEnte
            End If
            If myHashTable.ContainsKey("COD_TRIBUTO") = False Then
                myHashTable.Add("COD_TRIBUTO", Utility.Costanti.TRIBUTO_OSAP)
            Else
                myHashTable("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_OSAP
            End If
            If myHashTable.ContainsKey("CODTIPOPROCEDIMENTO") = False Then
                myHashTable.Add("CODTIPOPROCEDIMENTO", "L")
            Else
                myHashTable("CODTIPOPROCEDIMENTO") = "L"
            End If
            If myHashTable.ContainsKey("ListCateg") = False Then
                myHashTable.Add("ListCateg", ListCateg)
            Else
                myHashTable("ListCateg") = ListCateg
            End If
            If myHashTable.ContainsKey("ListTipiOcc") = False Then
                myHashTable.Add("ListTipiOcc", ListTipiOcc)
            Else
                myHashTable("ListTipiOcc") = ListTipiOcc
            End If
            If myHashTable.ContainsKey("ListAgevolazioni") = False Then
                myHashTable.Add("ListAgevolazioni", ListAgevolazioni)
            Else
                myHashTable("ListAgevolazioni") = ListAgevolazioni
            End If
            If myHashTable.ContainsKey("ListTariffe") = False Then
                myHashTable.Add("ListTariffe", ListTariffe)
            Else
                myHashTable("ListTariffe") = ListTariffe
            End If

            myHashTable.Add("CODCONTRIBUENTE", IdContribuente)
            myHashTable.Add("ANNOACCERTAMENTO", _Anno)

            'attivo il servizio
            MyMotore = CType(Activator.GetObject(GetType(IRemInterfaceOSAP.IRemotingInterfaceOSAP), ConstSession.UrlMotoreOSAP), IRemInterfaceOSAP.IRemotingInterfaceOSAP)
            ListIdDichToExam = DTO.MetodiDichiarazioneTosapCosap.GetIdDichiarazioniAnno(_Anno, _IdEnte, Utility.Costanti.TRIBUTO_OSAP, IdContribuente, -1, "", cmdMyCommand)
            If ListIdDichToExam.Length > 0 Then
                For x As Integer = 0 To ListIdDichToExam.GetUpperBound(0)
                    ' Ottengo la dichiarazione con tutti gli articoli
                    Dichiarazione = DTO.MetodiDichiarazioneTosapCosap.GetDichiarazioneForMotore(ListIdDichToExam(x), _IdEnte, Utility.Costanti.TRIBUTO_OSAP, _Anno, "", cmdMyCommand)
                    ' Scorro tutti gli articoli della dichiarazione e memorizzo i ruoli
                    For Each MyUI As IRemInterfaceOSAP.Articolo In Dichiarazione.ArticoliDichiarazione
                        Dim MyDichiarato As New OSAPAccertamentoArticolo
                        Dim ResultCalcolato As IRemInterfaceOSAP.CalcoloResult = MyMotore.CalcolaOSAP(IRemInterfaceOSAP.Ruolo.E_TIPO.ORDINARIO, MyUI, ListCateg, ListTipiOcc, ListAgevolazioni, ListTariffe, Nothing)
                        If (ResultCalcolato.Result <> IRemInterfaceOSAP.E_CALCOLORESULT.OK) Then
                            Throw New Exception("errore in calcolo")
                        Else
                            FncAcc.OSAPCastArtIntoProvArt(MyUI, MyDichiarato, IdContribuente)
                            MyDichiarato.Anno = _Anno
                            MyDichiarato.Calcolo = ResultCalcolato
                            'carico il progressivo della griglia
                            nList += 1
                            MyDichiarato.Progressivo = nList
                            MyDichiarato.IdLegame = MyDichiarato.Progressivo
                        End If
                        myArray.Add(MyDichiarato)
                    Next
                Next
                ListDichiarato = CType(myArray.ToArray(GetType(OSAPAccertamentoArticolo)), OSAPAccertamentoArticolo())

                For Each CurrentItem As OSAPAccertamentoArticolo In ListDichiarato
                    CurrentItem.Calcola_Interessi = True
                    myArray.Add(CurrentItem)
                Next
                ListAccertato = CType(myArray.ToArray(GetType(OSAPAccertamentoArticolo)), OSAPAccertamentoArticolo())

                myAtto = New ClsGestioneAccertamenti().OSAPConfrontoAccertatoDichiarato(_IdEnte, IdContribuente, _Anno, ListDichiarato, ListAccertato, -1, myHashTable, New DataSet, "", sDescTipoAvviso, sScript)
                If Not IsNothing(myAtto) Then
                    Throw New Exception("Errore in calcolo accertamento")
                End If
            End If

            myRet = True
        Catch ex As Exception
            Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovPROVVEDIMENTI.ClsMassiva.GenAtto0453.errore: ", ex)
            myRet = False
        End Try
        Return myRet
    End Function
#End Region
#Region "Transaction"
    Public Function StartCalcoloMassivoTransaction() As SqlClient.SqlCommand
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myTrans As SqlClient.SqlTransaction
        Dim myConnection As New SqlClient.SqlConnection(ConstSession.StringConnection)
        myConnection.Open()
        myTrans = myConnection.BeginTransaction
        cmdMyCommand.Connection = myConnection
        cmdMyCommand.CommandTimeout = 0
        cmdMyCommand.Transaction = myTrans
        Return cmdMyCommand
    End Function
    Public Function OpenCalcoloMassivoConnection() As SqlClient.SqlCommand
        Dim cmdMyCommand As New SqlClient.SqlCommand
        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
        cmdMyCommand.Connection.Open()
        cmdMyCommand.CommandTimeout = 0
        Return cmdMyCommand
    End Function
    Public Sub CommitCalcoloMassivoTransaction(ByRef myTrans As SqlClient.SqlTransaction, ByRef cmdMyCommand As SqlClient.SqlCommand)
        myTrans.Commit()
        cmdMyCommand.Dispose()
        cmdMyCommand.Connection.Close()
    End Sub
    Public Sub RollbackCalcoloMassivoTransaction(ByRef myTrans As SqlClient.SqlTransaction, ByVal cmdMyCommand As SqlClient.SqlCommand)
        myTrans.Rollback()
        cmdMyCommand.Dispose()
        cmdMyCommand.Connection.Close()
    End Sub
#End Region
End Class
''' <summary>
''' Classe per la gestione cache dell'elaborazione asincrona
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class CacheManager
    Private Shared IISCache As System.Web.Caching.Cache = HttpRuntime.Cache
    Private Shared Log As ILog = LogManager.GetLogger(GetType(CacheManager))

    Private Sub New()
        MyBase.New()
    End Sub
#Region "Massiva"
    Private Shared ElaborazioneMassivaInCorsoKey As String = "AttiMassiviInCorso"
    Private Shared AvanzamentoElaborazioneMassivaKey As String = "AttiMassiviAvanzamentoElaborazione"
    Private Shared RiepilogoElaborazioneMassivaKey As String = "RiepilogoAttiMassivi"

#Region "Elaborazione massiva in corso"
    Public Shared Function GetElaborazioneMassivaInCorso() As Integer
        Try
            If (Not (IISCache(ElaborazioneMassivaInCorsoKey)) Is Nothing) Then
                Return CType(IISCache(ElaborazioneMassivaInCorsoKey), Integer)
            Else
                Return -1
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.CacheManger.GetElaborazioneMassivaInCorso.errore: ", ex)
        End Try
    End Function
    Public Shared Sub SetElaborazioneMassivaInCorso(ByVal Anno As Integer)
        IISCache.Insert(ElaborazioneMassivaInCorsoKey, Anno, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveElaborazioneMassivaInCorso()
        IISCache.Remove(ElaborazioneMassivaInCorsoKey)
    End Sub
#End Region
#Region "Riepilogo Elaborazione Massiva"
    Public Shared Function GetRiepilogoElaborazioneMassiva() As ObjRuolo()
        If (Not (IISCache(RiepilogoElaborazioneMassivaKey)) Is Nothing) Then
            Return CType(IISCache(RiepilogoElaborazioneMassivaKey), ObjRuolo())
        Else
            Return Nothing
        End If
    End Function
    Public Shared Sub SetRiepilogoElaborazioneMassiva(ByVal listRuoli() As ObjRuolo)
        IISCache.Insert(RiepilogoElaborazioneMassivaKey, listRuoli, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveRiepilogoElaborazioneMassiva()
        IISCache.Remove(RiepilogoElaborazioneMassivaKey)
    End Sub
#End Region
#Region "Avanzamento Elaborazione Massiva"
    Public Shared Function GetAvanzamentoElaborazioneMassiva() As String
        Try
            If (Not (IISCache(AvanzamentoElaborazioneMassivaKey)) Is Nothing) Then
                Return CType(IISCache(AvanzamentoElaborazioneMassivaKey), String)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.CacheManger.GetAvanzamentoElaborazioneMassiva.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Shared Sub SetAvanzamentoElaborazioneMassiva(ByVal sMyDati As String)
        IISCache.Insert(AvanzamentoElaborazioneMassivaKey, sMyDati, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveAvanzamentoElaborazioneMassiva()
        IISCache.Remove(AvanzamentoElaborazioneMassivaKey)
    End Sub
#End Region
#End Region
#Region "Coattivo"
    Private Shared CalcoloCoattivoInCorsoKey As String = "CoattivoCalcoloInCorso"
    Private Shared AvanzamentoCalcoloCoattivoKey As String = "CoattivoAvanzamentoCalcolo"
    Private Shared RiepilogoCalcoloCoattivoKey As String = "CoattivoRiepilogoCalcolo"

#Region "Calcolo massivo in corso"
    Public Shared Function GetCalcoloCoattivoInCorso() As String
        Try
            If (Not (IISCache(CalcoloCoattivoInCorsoKey)) Is Nothing) Then
                Return CType(IISCache(CalcoloCoattivoInCorsoKey), Integer)
            Else
                Return ""
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.CacheManger.GetCalcoloCoattivoInCorso.errore: ", ex)
            Return ""
        End Try
    End Function
    Public Shared Sub SetCalcoloCoattivoInCorso(ByVal Anno As String)
        IISCache.Insert(CalcoloCoattivoInCorsoKey, Anno, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveCalcoloCoattivoInCorso()
        IISCache.Remove(CalcoloCoattivoInCorsoKey)
    End Sub
#End Region
#Region "Riepilogo calcolo Massivo"
    Public Shared Function GetRiepilogoCalcoloCoattivo() As ObjRuolo()
        If (Not (IISCache(RiepilogoCalcoloCoattivoKey)) Is Nothing) Then
            Return CType(IISCache(RiepilogoCalcoloCoattivoKey), ObjRuolo())
        Else
            Return Nothing
        End If
    End Function
    Public Shared Sub SetRiepilogoCalcoloCoattivo(ByVal listRuoli() As ObjRuolo)
        IISCache.Insert(RiepilogoCalcoloCoattivoKey, listRuoli, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveRiepilogoCalcoloCoattivo()
        IISCache.Remove(RiepilogoCalcoloCoattivoKey)
    End Sub
#End Region
#Region "Avanzamento Elaborazione"
    Public Shared Function GetAvanzamentoCalcoloCoattivo() As String
        Try
            If (Not (IISCache(AvanzamentoCalcoloCoattivoKey)) Is Nothing) Then
                Return CType(IISCache(AvanzamentoCalcoloCoattivoKey), String)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.CacheManger.GetAvanzamentoCalcoloCoattivo.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Shared Sub SetAvanzamentoCalcoloCoattivo(ByVal sMyDati As String)
        IISCache.Insert(AvanzamentoCalcoloCoattivoKey, sMyDati, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveAvanzamentoCalcoloCoattivo()
        IISCache.Remove(AvanzamentoCalcoloCoattivoKey)
    End Sub
#End Region
#End Region
End Class
'*** ***