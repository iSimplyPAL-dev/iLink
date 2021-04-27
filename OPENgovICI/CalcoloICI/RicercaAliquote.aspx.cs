using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using log4net;
using DichiarazioniICI.Database ;

namespace DichiarazioniICI.CalcoloICI
{
    /// <summary>
    /// Pagina per la ricerca aliquote.
    /// Contiene i parametri di ricerca e le funzioni della comandiera.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class RicercaAliquote : BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RicercaAliquote));

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Put user code to initialize the page here
                if (!(Page.IsPostBack))
                {
                    this.LoadDDL();
                    if (Request["Anno"] != null)
                        ddlAnno.SelectedValue = Request["Anno"].ToString();
                    string strscript = "";
                    //strscript += "parent.Comandi.location.href='./CRicercaAliquote.aspx';";
                    strscript += "divCopyTo.style.display = 'none';parent.Comandi.location.href='CRicercaAliquote.aspx';";
                    if (ddlAnno.SelectedValue != "-1")
                        strscript += "Search();";
                    RegisterScript(strscript,this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaAliquote.Page_Load.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        protected void btnRibalta_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (txtAnnoTo.Text == string.Empty)
                {
                    RegisterScript("divCopyTo.style.display = '';divSearch.style.display = 'none';GestAlert('a', 'warning', '', '', 'Anno di destinazione mancante!');", this.GetType());
                    return;
                }
                if (ddlAnnoFrom.SelectedValue == "-1")
                {
                    RegisterScript("divCopyTo.style.display = '';divSearch.style.display = 'none';GestAlert('a', 'warning', '', '', ('Anno di partenza mancante!');", this.GetType());
                    return;
                }
                foreach(ListItem myList in ddlAnnoFrom.Items)
                {
                    if (myList.Value == txtAnnoTo.Text)
                    {
                        RegisterScript("divCopyTo.style.display = '';divSearch.style.display = 'none';GestAlert('a', 'warning', '', '', ('Anno di destinazione già configurato!');", this.GetType());
                        return;
                    }
                }
                if (new Aliquote().RibaltaAliquota(Business.ConstWrapper.CodiceEnte, ddlAnnoFrom.SelectedValue, txtAnnoTo.Text))
                {
                    RegisterScript("divCopyTo.style.display = 'none';divSearch.style.display = '';GestAlert('a', 'success', '', '', ('Ribaltamento effettuato con successo!');", this.GetType());
                    LoadDDL();
                }
                else
                    RegisterScript("divCopyTo.style.display = '';divSearch.style.display = 'none';GestAlert('a', 'danger', '', '', ('Errore in ribaltamento!');", this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaAliquote.btnRibalta_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        private void LoadDDL()
        {
            ListItem myListItem = new ListItem();
            DataView myDataview;

            //*** 20140509 - TASI ***
            myListItem = new ListItem();
            myListItem.Text = "...";
            myListItem.Value = "";
            ddlTributo.Items.Add(myListItem);

            myDataview = new Aliquote().ListaTributo();
            try
            {
                foreach (DataRow myRow in myDataview.Table.Rows)
                {
                    ListItem myListItem1 = new ListItem();
                    myListItem1.Text = (string)myRow["DESCR"];
                    myListItem1.Value = (string)myRow["TIPO"];
                    ddlTributo.Items.Add(myListItem1);
                }
                //*** ***

                //anno
                //*** 20150430 - TASI Inquilino ***
                //myListItem = new ListItem();
                //myListItem.Value = "-1";
                //myListItem.Text = "...";
                //ddlAnno.Items.Add(myListItem);
                //for (int i=DateTime.Today.Year + 1;i > 1989 ;i--)
                //{
                //    myListItem = new ListItem();
                //    myListItem.Value = i.ToString();
                //    myListItem.Text = i.ToString();
                //    ddlAnno.Items.Add(myListItem);
                //}
                ddlAnno.Items.Clear();
                ddlAnnoFrom.Items.Clear();
                myListItem = new ListItem();
                myListItem.Text = "...";
                myListItem.Value = "-1";
                ddlAnno.Items.Add(myListItem);
                ddlAnnoFrom.Items.Add(myListItem);

                myDataview = new Aliquote().ListaAnni(Business.ConstWrapper.CodiceEnte);
                foreach (DataRow myRow in myDataview.Table.Rows)
                {
                    ListItem myListItem1 = new ListItem();
                    myListItem1.Text = myRow["DESCR"].ToString();
                    myListItem1.Value = myRow["ID"].ToString();
                    ddlAnno.Items.Add(myListItem1);
                    ddlAnnoFrom.Items.Add(myListItem1);
                }
                //*** ***
                //tipo aliquota
                myListItem.Text = "...";
                myListItem.Value = "-1";
                ddlTipo.Items.Add(myListItem);

                //DataView myDataview = GetTipoAliquote();
                myDataview = GetTipoAliquote();
                foreach (DataRow myRow in myDataview.Table.Rows)
                {
                    ListItem myListItem1 = new ListItem();
                    myListItem1.Text = (string)myRow["DESCR"];
                    myListItem1.Value = (string)myRow["TIPO"];
                    ddlTipo.Items.Add(myListItem1);
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaAliquote.LoadDDL.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        #region METODI
        /// <summary>
        /// Ritorna una DataView valorizzata con l'elenco degli anni di riferimento.
        /// </summary>
        protected DataView GetTipoAliquote()
        {
            DataView Vista = new Aliquote().ListaTipoAliquote("-1");
            Vista.Sort = "";
            return Vista;
        }
        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

    }
}
