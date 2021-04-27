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


Partial Public Class GestioneAnagrafeResidenti
    
    '''<summary>
    '''Controllo Form1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Form1 As Global.System.Web.UI.HtmlControls.HtmlForm
    
    '''<summary>
    '''Controllo txtCognome.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtCognome As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo txtNome.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtNome As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo txtCodiceFiscale.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtCodiceFiscale As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo txtNumFamiglia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtNumFamiglia As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo optTrattato.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents optTrattato As Global.System.Web.UI.WebControls.RadioButton
    
    '''<summary>
    '''Controllo optNonTrattato.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents optNonTrattato As Global.System.Web.UI.WebControls.RadioButton
    
    '''<summary>
    '''Controllo optTuttiTrattato.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents optTuttiTrattato As Global.System.Web.UI.WebControls.RadioButton
    
    '''<summary>
    '''Controllo optSiSuTributo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents optSiSuTributo As Global.System.Web.UI.WebControls.RadioButton
    
    '''<summary>
    '''Controllo optNoSuTributo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents optNoSuTributo As Global.System.Web.UI.WebControls.RadioButton
    
    '''<summary>
    '''Controllo optTuttiSuTributo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents optTuttiSuTributo As Global.System.Web.UI.WebControls.RadioButton
    
    '''<summary>
    '''Controllo DivAttesa.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents DivAttesa As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo GrdRes.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents GrdRes As Global.Ribes.OPENgov.WebControls.RibesGridView
    
    '''<summary>
    '''Controllo btnSalva.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents btnSalva As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo btnRicerca.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents btnRicerca As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo btnView.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents btnView As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo btnAbbina.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents btnAbbina As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo MyImgButton.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents MyImgButton As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo btnRibalta.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents btnRibalta As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo btnStampaExcel.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents btnStampaExcel As Global.System.Web.UI.WebControls.Button
End Class
