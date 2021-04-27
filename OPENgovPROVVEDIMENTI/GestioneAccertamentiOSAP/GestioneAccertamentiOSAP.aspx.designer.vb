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


Partial Public Class GestioneAccertamentiOSAP
    
    '''<summary>
    '''Controllo Form1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Form1 As Global.System.Web.UI.HtmlControls.HtmlForm
    
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
    '''Controllo btnFocus.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents btnFocus As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo Imagebutton.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Imagebutton As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo txtNominativo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtNominativo As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo btnRibalta.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents btnRibalta As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo txtHiddenIdDataAnagrafica.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtHiddenIdDataAnagrafica As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo lblNumeroProvvedimento.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblNumeroProvvedimento As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo ddlAnno.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ddlAnno As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo chkspese.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkspese As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Controllo Label2.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label2 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo btnSearchDichiarazioni.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents btnSearchDichiarazioni As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo btnSvuotaSessionContribuente.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents btnSvuotaSessionContribuente As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo btngotoVersContribuente.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents btngotoVersContribuente As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo txtCerca.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtCerca As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo btnSearchDichiarazioniEffettivo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents btnSearchDichiarazioniEffettivo As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo CmdRibaltaUIAnater.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdRibaltaUIAnater As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo CmdRibalta.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdRibalta As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo BtnAnnulloNoAcc.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents BtnAnnulloNoAcc As Global.System.Web.UI.WebControls.Button
End Class
