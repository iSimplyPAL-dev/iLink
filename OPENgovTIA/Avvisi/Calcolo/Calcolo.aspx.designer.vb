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


Partial Public Class Calcolo
    
    '''<summary>
    '''Controllo Form1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Form1 As Global.System.Web.UI.HtmlControls.HtmlForm
    
    '''<summary>
    '''Controllo info.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents info As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo LblTipoMQ.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblTipoMQ As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo optFlusso.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents optFlusso As Global.System.Web.UI.WebControls.RadioButton
    
    '''<summary>
    '''Controllo txtInizioConf.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtInizioConf As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo txtFineConf.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtFineConf As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo txtGGScadenza.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtGGScadenza As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo chkSimulazione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkSimulazione As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Controllo lblUploadFiles.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblUploadFiles As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo fuMyFiles.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents fuMyFiles As Global.System.Web.UI.WebControls.FileUpload
    
    '''<summary>
    '''Controllo lblNoteFlussi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblNoteFlussi As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo lblMessage.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblMessage As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblIntestTesBidoneDaElab.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblIntestTesBidoneDaElab As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo fsRuoliImportati.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents fsRuoliImportati As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo GrdRuoliImportati.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents GrdRuoliImportati As Global.Ribes.OPENgov.WebControls.RibesGridView
    
    '''<summary>
    '''Controllo LblAvanzamento.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblAvanzamento As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo LblResultRuoliVsCatPF.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblResultRuoliVsCatPF As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo GrdRuoliVsCatPF.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents GrdRuoliVsCatPF As Global.Ribes.OPENgov.WebControls.RibesGridView
    
    '''<summary>
    '''Controllo LblResultRuoliVsCatPV.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblResultRuoliVsCatPV As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo GrdRuoliVsCatPV.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents GrdRuoliVsCatPV As Global.Ribes.OPENgov.WebControls.RibesGridView
    
    '''<summary>
    '''Controllo LblResultRuoliVsCatPM.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblResultRuoliVsCatPM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo GrdRuoliVsCatPM.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents GrdRuoliVsCatPM As Global.Ribes.OPENgov.WebControls.RibesGridView
    
    '''<summary>
    '''Controllo LblResultRuoliVsCatPC.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblResultRuoliVsCatPC As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo GrdRuoliVsCatPC.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents GrdRuoliVsCatPC As Global.Ribes.OPENgov.WebControls.RibesGridView
    
    '''<summary>
    '''Controllo CmdVisualizza.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdVisualizza As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo CmdDownload.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdDownload As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo CmdUpload.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdUpload As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo hfIdRuoloMinutaAvvisi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents hfIdRuoloMinutaAvvisi As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo hdMinutaAnagAllRow.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents hdMinutaAnagAllRow As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo hdMinutaStampatoreAllowed.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents hdMinutaStampatoreAllowed As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo hdIsMinutaXStampatore.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents hdIsMinutaXStampatore As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo hdPFPVUniqueRow.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents hdPFPVUniqueRow As Global.System.Web.UI.WebControls.HiddenField
End Class
