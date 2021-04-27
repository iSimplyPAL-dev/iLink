'------------------------------------------------------------------------------
' <generato automaticamente>
'     Codice generato da uno strumento.
'
'     Le modifiche a questo file possono causare un comportamento non corretto e verranno perse se
'     il codice viene rigenerato. 
' </generato automaticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class PopUpInsertVani
    
    '''<summary>
    '''Controllo Form1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Form1 As Global.System.Web.UI.HtmlControls.HtmlForm
    
    '''<summary>
    '''Controllo lblTitolo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblTitolo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo ifrmAnag.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ifrmAnag As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo hdIdContribuente.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents hdIdContribuente As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo lblNominativo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblNominativo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo lblCFPIVA.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblCFPIVA As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo lblDatiNascita.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblDatiNascita As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo lblResidenza.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblResidenza As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DivTessera.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DivTessera As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo LblNTessera.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblNTessera As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblCodInterno.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblCodInterno As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblCodUtente.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblCodUtente As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblDataRilascio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblDataRilascio As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblDataCessazione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblDataCessazione As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblUbicazione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblUbicazione As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblRifCat.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblRifCat As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblDataInizio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblDataInizio As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblDataFine.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblDataFine As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblMQCatasto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblMQCatasto As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblMQTassabili.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblMQTassabili As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblCat.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblCat As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblComponentiPF.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblComponentiPF As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblComponentiPV.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblComponentiPV As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblForzaCalcoloPV.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblForzaCalcoloPV As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo GrdVani.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents GrdVani As Global.Ribes.OPENgov.WebControls.RibesGridView
    
    '''<summary>
    '''Controllo Label1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label1 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlCategorie1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlCategorie1 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label2.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label2 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlTipoVano1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlTipoVano1 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label3.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label3 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNVani1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNVani1 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo Label4.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label4 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNMQ1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNMQ1 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo ChkEsente1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ChkEsente1 As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Controllo Label5.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label5 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlCategorie2.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlCategorie2 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label6.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label6 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlTipoVano2.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlTipoVano2 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label7.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label7 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNVani2.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNVani2 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo Label8.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label8 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNMQ2.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNMQ2 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo ChkEsente2.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ChkEsente2 As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Controllo Label9.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label9 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlCategorie3.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlCategorie3 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label10.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label10 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlTipoVano3.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlTipoVano3 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label11.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label11 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNVani3.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNVani3 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo Label12.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label12 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNMQ3.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNMQ3 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo ChkEsente3.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ChkEsente3 As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Controllo Label13.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label13 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlCategorie4.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlCategorie4 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label14.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label14 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlTipoVano4.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlTipoVano4 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label15.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label15 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNVani4.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNVani4 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo Label16.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label16 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNMQ4.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNMQ4 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo ChkEsente4.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ChkEsente4 As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Controllo Label17.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label17 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlCategorie5.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlCategorie5 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label18.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label18 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlTipoVano5.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlTipoVano5 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label19.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label19 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNVani5.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNVani5 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo Label20.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label20 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNMQ5.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNMQ5 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo ChkEsente5.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ChkEsente5 As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Controllo Label21.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label21 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlCategorie6.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlCategorie6 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label22.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label22 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlTipoVano6.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlTipoVano6 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label23.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label23 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNVani6.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNVani6 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo Label24.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label24 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNMQ6.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNMQ6 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo ChkEsente6.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ChkEsente6 As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Controllo Label25.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label25 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlCategorie7.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlCategorie7 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label26.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label26 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlTipoVano7.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlTipoVano7 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label27.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label27 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNVani7.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNVani7 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo Label28.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label28 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNMQ7.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNMQ7 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo ChkEsente7.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ChkEsente7 As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Controllo Label29.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label29 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlCategorie8.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlCategorie8 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label30.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label30 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo DdlTipoVano8.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DdlTipoVano8 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo Label31.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label31 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNVani8.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNVani8 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo Label32.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label32 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo TxtNMQ8.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtNMQ8 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo ChkEsente8.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ChkEsente8 As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Controllo TxtIdVano1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtIdVano1 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo TxtIdVano2.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtIdVano2 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo TxtIdVano3.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtIdVano3 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo TxtIdVano4.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtIdVano4 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo TxtIdVano5.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtIdVano5 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo TxtIdVano6.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtIdVano6 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo TxtIdVano7.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtIdVano7 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo TxtIdVano8.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtIdVano8 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo TxtIdDettaglioTestata.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtIdDettaglioTestata As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo CmdDeleteVani.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdDeleteVani As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo CmdModVani.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdModVani As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo CmdSalvaVani.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdSalvaVani As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo CmdBack.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdBack As Global.System.Web.UI.WebControls.Button
End Class
