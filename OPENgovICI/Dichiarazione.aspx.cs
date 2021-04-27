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
using System.Data.SqlTypes;
using DichiarazioniICI.Database;
using Business;
using AnagInterface;
using Ribes;
using log4net;
using log4net.Config;
using System.IO;

using RemotingInterfaceAnater;
using Anater.Oggetti;

namespace DichiarazioniICI
{
    /// <summary>
    /// Classe di gestione per la pagina Dichiarazione.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class DichiarazionePage : BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DichiarazionePage));
        //protected System.Web.UI.WebControls.RequiredFieldValidator rvalDatProt;
        //protected System.Web.UI.WebControls.RequiredFieldValidator rvalNumDich;
        //protected System.Web.UI.WebControls.CompareValidator cvalNumDichiaraz;
        //protected System.Web.UI.WebControls.RequiredFieldValidator rvalDatDich;
        //protected System.Web.UI.WebControls.Label lblRiemp2;

        #region Property
        /// <summary>
        /// Ritorna o assegna l'id della testata/dichiarazione.
        /// </summary>
        protected int IDTestata
        {
            get { return ViewState["IDTestata"] != null ? (int)ViewState["IDTestata"] : 0; }
            set { ViewState["IDTestata"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected int IDContribuente
        {
            get { return ViewState["IDContribuente"] != null ? (int)ViewState["IDContribuente"] : 0; }
            set { ViewState["IDContribuente"] = value; }
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        //*** 20131003 - gestione atti compravendita ***
        protected int CompraVenditaId
        {
            get { return ViewState["CompraVenditaId"] != null ? int.Parse(ViewState["CompraVenditaId"].ToString()) : 0; }
            set { ViewState["CompraVenditaId"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected int CompraVenditaIdSoggetto
        {
            get { return ViewState["CompraVenditaIdSoggetto"] != null ? int.Parse(ViewState["CompraVenditaIdSoggetto"].ToString()) : 0; }
            set { ViewState["CompraVenditaIdSoggetto"] = value; }
        }
        //*** ***
        #endregion

        #region Web Form Designer generated code
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
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

        #region Metodi
        /// <summary>
        /// Torna una Dataview con l'elenco delle provenienze.
        /// </summary>
        /// <returns></returns>
        protected DataView ListProvenienze()
        {
            DataView Vista = new ProvenienzeTable(ConstWrapper.sUsername).List(TipologieProvenienza.dichiarazioni, ConstWrapper.CodiceTributo, 0);//, ApplicationHelper.Tributo);
            return Vista;
        }

        /// <summary>
        /// Prende la quantità degli immobili quali appartengono alla testata selezionata.
        /// </summary>
        /// <param name="idTestata"></param>
        /// <returns></returns>
        private int CountImmobili(int idTestata)
        {
            int Immobili = (int)new OggettiTable(ConstWrapper.sUsername).CountImmobiliByIDTestata(idTestata);
            return Immobili;
        }

        /// <summary>
        /// Prende la quantità degli immobili già bonificati che appartengono alla testata selezionata.
        /// </summary>
        /// <param name="idTestata"></param>
        /// <returns></returns>
        private int CountImmobiliBonificati(int idTestata)
        {
            int ImmobiliBonificati = (int)new OggettiTable(ConstWrapper.sUsername).CountImmobiliBonificatiByIDTestata(idTestata);
            return ImmobiliBonificati;
        }

        /// <summary>
        /// Prende la quantità dei contitolari quali appartengono alla testata selezionata.
        /// </summary>
        /// <param name="idTestata"></param>
        /// <returns></returns>
        private int CountContitolari(int idTestata)
        {
            int Contitolari = (int)new DettaglioTestataTable(ConstWrapper.sUsername).CountContitolariByIDTestata(idTestata);
            return Contitolari;
        }

        /// <summary>
        /// Prende la quantità dei contitolari già bonificati che appartengono alla testata selezionata.
        /// </summary>
        /// <param name="idTestata"></param>
        /// <returns></returns>
        private int CountContitoariBonificati(int idTestata)
        {
            int ContitolariBonificati = (int)new DettaglioTestataTable(ConstWrapper.sUsername).CountContitolariBonificatiByIDTestata(idTestata);
            return ContitolariBonificati;
        }
        #endregion

        #region Bind Controlli
        /// <summary>
        /// Esegue il bind dei controlli che appartengono al contribuente.
        /// </summary>
        private void ControlliContribuenteBind()
        {
            txtCodFiscaleContr.DataBind();
            txtPIVAContr.DataBind();
            txtCognomeContr.DataBind();
            txtNomeContr.DataBind();
            rdbMaschioContr.DataBind();
            rdbFemminaContr.DataBind();
            txtDataNascContr.DataBind();
            txtComNascContr.DataBind();
            txtProvNascContr.DataBind();
            txtViaResContr.DataBind();
            txtNumCivResContr.DataBind();
            txtIntResContr.DataBind();
            txtScalaResContr.DataBind();
            txtComuneResContr.DataBind();
            txtProvResContr.DataBind();
            txtCapResContr.DataBind();
        }

        /// <summary>
        /// Esegue il bind dei controlli che appartengono al denunciante.
        /// </summary>
        private void ControlliDenuncianteBind()
        {
            txtCodFiscaleDen.DataBind();
            txtPIVADen.DataBind();
            txtCognomeDen.DataBind();
            txtNomeDen.DataBind();
            rdbMaschioDen.DataBind();
            rdbFemminaDen.DataBind();
            txtDataNascDen.DataBind();
            txtComNascDen.DataBind();
            txtProvNascDen.DataBind();
            txtViaResDen.DataBind();
            txtNumCivResDen.DataBind();
            txtIntResDen.DataBind();
            txtScalaResDen.DataBind();
            txtComuneResDen.DataBind();
            txtProvResDen.DataBind();
            txtCapResDen.DataBind();
        }
        #endregion

       /// <summary>
       /// 
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try { 
            if (!IsPostBack)
            {
                //*** 20131003 - gestione atti compravendita ***
                if (Request.QueryString["IdAttoCompraVendita"] != null)
                {
                    this.CompraVenditaId = int.Parse(Request.QueryString["IdAttoCompraVendita"]);
                }
                if (Request.QueryString["IdAttoSoggetto"] != null)
                {
                    this.CompraVenditaIdSoggetto = int.Parse(Request.QueryString["IdAttoSoggetto"]);
                }
                //*** *** 

                ddlProvenienze.DataBind();
                AbilitaLabelDenunciante(false);

                //salvo il typeoperation, che mi gestisce il pulsante di Back
                txtTypeOperation.Text = Request["TYPEOPERATION"].ToString();

                lblMesImmobile.Visible = false;

                if (Request.QueryString["IDTestata"] != null)
                {
                    string sScript = "parent.Comandi.location.href='CDichiarazioneMod.aspx'";
                        sScript += "parent.Basso.location.href='../aspVuota.aspx';";
                        sScript += "parent.Nascosto.location.href='../aspVuota.aspx';";
                        RegisterScript(sScript,this.GetType());

                    this.IDTestata = int.Parse(Request.QueryString["IDTestata"]);
                    // aggancio lo script del popup della bonifica
                    //btnBonifica.Attributes.Add("onclick", "javascript:window.showModalDialog('PopUpErroriDichiarazione.aspx?IDTestata=" + this.IDTestata + "', window, 'dialogHeight: 400px; dialogWidth: 400px; status: no');return false;");
                    btnElimina.Attributes.Add("onclick", "javascript:return confirm('Confermi la cancellazione?');");
                    //btnSalva.Attributes.Add("onclick", "return VerificaDataNumero();");
                    int PassProp = 0;
                    int.TryParse(Request.QueryString["PassProp"], out PassProp);
                    LoadDichiarazione(PassProp);
                }
                else
                {
                    Abilita(true);
                    AbilitaLabelDenunciante(chkEsisteDeninciante.Checked);
                    //*** 201504 - Nuova Gestione anagrafica con form unico ***
                    if (ConstWrapper.HasPlainAnag)
                        ifrmAnag.Attributes.Add("src", "../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString());

                    //string stringa ="";
                    string stringa;
                    stringa = "parent.Comandi.document.getElementById('New').style.display = 'none'";
                    //stringa = stringa + "";
                    //RegisterScript(sScript,this.GetType());,"mes",stringa);
                    RegisterScript(stringa,this.GetType());

                    string sScript = "parent.Comandi.location.href='CDichiarazione.aspx';";
                        sScript += "parent.Basso.location.href='../aspVuota.aspx';";
                        sScript += "parent.Nascosto.location.href='../aspVuota.aspx';";
                        RegisterScript(sScript,this.GetType());
                }

                btnSalva.Attributes.Add("onclick", "return VerificaDataNumero(" + hdIsPassProg.Value.ToString() + ");");
                // aggiungo gli eventi per l'eliminazione dei dati del contribuente e del denunciante
                lnkPulisciContr.Attributes.Add("onclick", "return PulContribuente();");
                lnkPulisciDen.Attributes.Add("onclick", "return PulDenunciante();");

                btnImmobili.DataBind();
                btnElimina.DataBind();

                //string str;
                //str = "Search(" + this.IDTestata + ");";
                //RegisterScript(sScript,this.GetType());, "msg", str);
                HtmlControl myFrame = (HtmlControl)(this.FindControl("loadImmobile"));
                myFrame.Attributes.Add("src", "Immobile.aspx?IdTestata=" + this.IDTestata);

                //*** 20131003 - gestione atti compravendita ***
                if (this.CompraVenditaId > 0)
                {
                    DataTable myData = new DichiarazioniView().GetCompraVendita(int.Parse(Request.QueryString["IdAttoCompraVendita"]));
                        foreach(DataRow myRow in myData.Rows)
                        {
                            lblNotaTrascrizione.Text = myRow["NotaTrascrizione"].ToString();
                            lblRifNota.Text = myRow["RifNota"].ToString();
                            lblCatNota.Text = myRow["CatNota"].ToString();
                            lblUbicazioneNota.Text = myRow["UbicazioneNota"].ToString();
                            lblUbicazioneCatasto.Text = myRow["UbicazioneCatasto"].ToString();
                        }
                                            myData = new DichiarazioniView().GetCompraVenditaSoggetto(int.Parse(Request.QueryString["IdAttoCompraVendita"]), this.IDContribuente, this.CompraVenditaIdSoggetto);
                        foreach (DataRow myRow in myData.Rows)
                        {
                            lblSoggettoNota.Text =myRow["NotaTitolarita"].ToString();
                    }
                    AttoCompraVendita.Style.Add("display", "");

                    string strscript = "parent.Comandi.getElementById('Precarica').Style.Add('display')";
                    RegisterScript(strscript, this.GetType());
                }
                else
                {
                    AttoCompraVendita.Style.Add("display", "none");
                    string sScript = "parent.Comandi.location.href='CDichiarazioneMod.aspx';";
                        sScript += "parent.Basso.location.href='../aspVuota.aspx';";
                        sScript += "parent.Nascosto.location.href='../aspVuota.aspx';";
                        sScript += "parent.Comandi.getElementById('Precarica').Style.Add('display');";
                    RegisterScript(sScript, this.GetType());
                }
                //*** ***
            }
            else
            {
                //*** 201504 - Nuova Gestione anagrafica con form unico ***
               /* if (ConstWrapper.HasPlainAnag)
                {
                    if (hdIdContribuente.Value != "" && int.Parse(hdIdContribuente.Value) > 0) {
                        ifrmAnag.Attributes.Add("src", "../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_LETTURA.ToString());
                        string stringaImmob = "parent.Comandi.document.getElementById('New').style.display = ''";
                        RegisterScript(sScript,this.GetType());, "mesImm", stringaImmob);
                    }
                    else
                        ifrmAnag.Attributes.Add("src", "../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString());
                }
                string str;
                str = "Search(" + this.IDTestata + ");";
                RegisterScript(sScript,this.GetType());, "msg", str);*/
            }
            //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            string Str = "";
            if (ConstWrapper.HasPlainAnag)
                Str += "document.getElementById('TRSpecAnag').style.display='none';";
            else
                Str += "document.getElementById('TRPlainAnag').style.display='none';";
            Str += "";
            RegisterScript(Str,this.GetType());
                //*** ***
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.Page_Load.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }
        }

        /// <summary>
        /// Verifico nell'Anagrafica se il contribuente e/o il denunciante già esiste.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkVerificaAnagrafica_Click(object sender, System.EventArgs e)
        {
            DataTable RisultatoRicerca = HelperAnagrafica.RicercaPersona(txtCognomeDen.Text, txtNomeDen.Text, txtCodFiscaleDen.Text, txtPIVADen.Text);
        }

        //        protected void btnSalva_Click(object sender, System.EventArgs e)
        //        {
        //            bool retval = true;

        //            int CodiceContribuenteContr;
        //            CodiceContribuenteContr = txtCodContribuenteCon.Value == String.Empty ? 0 : int.Parse(txtCodContribuenteCon.Value);

        //            int CodiceContribuenteDen;
        //            CodiceContribuenteDen = txtCodContribuenteDen.Value == String.Empty ? 0 : int.Parse(txtCodContribuenteDen.Value);

        //            //TestataTable Testata = new TestataTable(ConstWrapper.sUsername);
        //            TestataTable Testata = new TestataTable(ConstWrapper.sUsername);
        //            TestataRow RigaTestata = new TestataRow();

        //            RigaTestata.ID=this.IDTestata;
        //            RigaTestata.Ente = ConstWrapper.CodiceEnte;
        //         try{
        //            if (txtNumDich.Text == "")
        //                RigaTestata.NumeroDichiarazione = -1;
        //            else
        //                RigaTestata.NumeroDichiarazione = int.Parse(txtNumDich.Text);

        //            RigaTestata.AnnoDichiarazione = txtAnnoDich.Text;
        //            RigaTestata.NumeroProtocollo = txtNumProt.Text;
        //            RigaTestata.DataProtocollo = txtDataProt.Text == String.Empty ? DateTime.MinValue : Convert.ToDateTime(txtDataProt.Text);
        //            RigaTestata.TotaleModelli = txtTotModelli.Text;
        //            RigaTestata.DataInizio = txtDataDich.Text == String.Empty ? DateTime.MinValue : Convert.ToDateTime(txtDataDich.Text);
        //            RigaTestata.DataFine = DateTime.MinValue;
        //            RigaTestata.Bonificato = false;
        //            RigaTestata.Annullato = false;
        //            RigaTestata.DataInizioValidità = DateTime.Now;
        //            RigaTestata.DataFineValidità = DateTime.MinValue;
        //            //RigaTestata.Operatore = User.Identity.Name;
        //            RigaTestata.Operatore = ConstWrapper.sUsername;
        //            RigaTestata.IDContribuente = CodiceContribuenteContr;
        //            RigaTestata.IDDenunciante = CodiceContribuenteDen;
        //            RigaTestata.IDProvenienza = int.Parse(ddlProvenienze.SelectedValue);

        //            // effettuo o il salvataggio o la modifica (attraverso
        //            // storicizzazione) dei dati
        //            if(this.IDTestata == 0)
        //            {
        //                // inserisce una nuova testata
        //                int idTestata;
        //                retval = Testata.Insert(RigaTestata, out idTestata);
        //                this.IDTestata = idTestata;
        //            }
        //            else
        //            {
        //                //int idTestata;
        //                // salvo i dati nuovi
        //                //retval = Testata.Insert(RigaTestata, out idTestata);
        //                retval = Testata.Modify(RigaTestata);
        ////         STORICIZZA  :  COMMENTATO IL 25/01/2007 DA ALESSIO

        ////				if(retval)
        ////				{
        //                    // modifico i dati vecchi
        ////					RigaTestata.ID = this.IDTestata;
        ////					retval = Testata.Storicizzazione(RigaTestata.ID);
        ////					if(retval)
        ////					{
        //                        // modifico l'idTestata nel dettaglio che appartiene alla testata
        ////						//DataTable tblDettagli = new DettaglioTestataTable(User.Identity.Name).ListDettagli(this.IDTestata);
        ////						DataTable tblDettagli = new DettaglioTestataTable(ConstWrapper.sUsername).ListDettagli(this.IDTestata);
        ////						if(tblDettagli.Rows.Count > 0)
        ////						{
        //                            //new DettaglioTestataTable(User.Identity.Name).Modify(idTestata, this.IDTestata);
        ////							new DettaglioTestataTable(ConstWrapper.sUsername).Modify(RigaTestata.ID, this.IDTestata);
        ////						}
        ////
        //                        // modifico l'idTestata nella tabella Oggetti
        ////						DataTable tblImmobili = new OggettiTable(ConstWrapper.sUsername).GetImmobileByIDTestata(this.IDTestata);
        ////						if(tblImmobili.Rows.Count > 0)
        ////						{
        ////							new OggettiTable(ConstWrapper.sUsername).Modify(RigaTestata.ID, this.IDTestata);
        ////						}
        ////
        //                        // sostituisco l'id della testata vecchia con quella nuova
        ////						this.IDTestata = RigaTestata.ID;	
        ////					}
        ////				}
        ////				FINE COMMENTO				
        //            }

        //            strscript = "alert('');";RegisterScript(sScript,this.GetType());, "err", strscript);this, "Salvataggio" + (retval == true ? "" : " non") + " effettuato.");

        //            string stringaImmob ="parent.Comandi.document.getElementById('New').style.display = ''";
        //            RegisterScript(sScript,this.GetType());,"mesImm",stringaImmob);

        //            if(retval)
        //            {
        //                btnImmobili.DataBind();
        //                btnElimina.DataBind();	

        //                // Faccio una bonifica di quanto inserito per marcare gli errori
        //                //new ModuloIci(HttpContext.Current.User.Identity.Name).BonificaDichiarazione(IDTestata);
        //                // eseguo la bonifica
        //                bool RetVal;
        //                RetVal = new ModuloIci(ConstWrapper.sUsername).BonificaDichiarazione(IDTestata, ConstWrapper.StringConnection);
        //                //RetVal = new ModuloIci(String.Empty).BonificaDichiarazione(IDTestata);

        //                // se la dichiarazione non è stata bonificata apro il PopUp degli errori.
        //                if (!RetVal) 
        //                {
        //                    string strScript= "javascript:window.showModalDialog('PopUpErroriDichiarazione.aspx?IDTestata=" + IDTestata + "', window, 'dialogHeight: 400px; dialogWidth: 400px; status: no');";
        //                    RegisterScript(sScript,this.GetType());,"", "" + strScript + "");
        //                }
        //                else{
        //                    lblBonificata.Visible=false;
        //                }
        //                lblMesImmobile.Visible = true;

        //                //	altrimenti Ribalto in Anater
        //                // se la bonifica va a buon fine ribalta in anater

        //            if(RetVal)
        //            {	

        //                if (ConstWrapper.UsoAnater == "true") 
        //                {

        //                    Type typeofRI = typeof(IRemotingInterfaceICI);
        //                    IRemotingInterfaceICI remObject = (IRemotingInterfaceICI)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLanaterICI"].ToString());

        //                    int iRetValControlloAnagraficaAnater;
        //                    iRetValControlloAnagraficaAnater= remObject.ControlloAnagraficaAnater ((DettaglioAnagrafica)Session["contribuenteANATER"],(DettaglioAnagrafica)Session["denuncianteANATER"],ConstWrapper.CodiceEnte);
        //                    //valori ritornati:
        //                    //0- contribuente trovato -->residente
        //                    //1- contribuente trovato --> non  residente --> dati non variati
        //                    //2- contribuente trovato -->non  residente --> dati variati
        //                    //3- contribuente non trovato -->nuovo inserimento


        //                    string strscript = "RibaltaInAnater(" + iRetValControlloAnagraficaAnater + ");";
        //                    RegisterScript(sScript,this.GetType());,"RibaltaInAnater", "" + strscript + "");


        //                    // ribalto in anater

        //                    // preparo l'oggetto trasformatore
        ////					TrasformatoreAnater TrasformaOggettiAnater = new TrasformatoreAnater();
        ////
        ////					RigaTestata.ID=IDTestata;
        ////
        //                    //Type typeofRI = typeof(IRemotingInterfaceICI);
        //                    //IRemotingInterfaceICI remObject = (IRemotingInterfaceICI)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLanaterICI"].ToString());
        ////				
        ////					TestataRowICI oTestataANATER=new Anater.Oggetti.TestataRowICI();
        ////					DettaglioTestataRowICI oDettaglioTestataANATER=new Anater.Oggetti.DettaglioTestataRowICI();
        ////					OggettiRowICI oImmobileANATER=new Anater.Oggetti.OggettiRowICI();
        ////					DettaglioAnagrafica oAnagraficaContitolare=new DettaglioAnagrafica();		
        ////
        //                    //oTestataANATER=typeof(TestataRow);
        //                    //oTestataANATER=(TestataRowICI)RigaTestata;
        ////					TrasformatoreAnater TrasformaOggetti = new TrasformatoreAnater();
        ////				
        ////					oTestataANATER=TrasformaOggetti.TrasformaRigaTestata(RigaTestata);				
        ////
        //                    //remObject.RibaltaInAnater((DettaglioAnagrafica)Session["contribuenteANATER"],(DettaglioAnagrafica)Session["denuncianteANATER"], oTestataANATER, oDettaglioTestataANATER,oImmobileANATER ,oAnagraficaContitolare,"7026");
        ////
        ////
        //                        // SE LA DICHIARAZIONE RISULTA BONIFICATA RIBALTO IN ANATER
        //                        // preparo l'oggetto testata
        //                    // TestataRow RigaTestataAnater = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata);
        ////
        ////					DettaglioAnagrafica DatiContribuenteAnater;
        ////					DettaglioAnagrafica DatiDenuncianteAnater;
        ////						
        //                    // preparo l'oggetto DettaglioAnagrafica col contribuente
        ////					if (RigaTestata.IDContribuente != 0)
        ////						DatiContribuenteAnater = HelperAnagrafica.GetDatiPersona(RigaTestata.IDContribuente);
        ////					else
        ////						DatiContribuenteAnater = new DettaglioAnagrafica();	
        ////
        //                    // preparo l'oggetto denunciante
        ////					if (RigaTestata.IDDenunciante != 0)
        ////						DatiDenuncianteAnater = HelperAnagrafica.GetDatiPersona(RigaTestata.IDDenunciante);
        ////					else
        ////						DatiDenuncianteAnater = new DettaglioAnagrafica();
        ////
        ////
        ////						
        ////					ArrayList arrayListImmobileCompleto = new ArrayList();
        ////						
        //                    // partendo dall'id testata prendo l'elenco di tutti gli immobili presenti nella dichiarazione
        ////					DataTable ImmobiliDichiarazione = new OggettiTable(ConstWrapper.sUsername).GetImmobileByIDTestata(this.IDTestata);
        ////
        ////					foreach(DataRow RigaImmobile in ImmobiliDichiarazione.Rows){
        ////						
        ////						int IdImmobileAnater = (int)RigaImmobile["ID"];
        //                        // prendo la riga dell'immobile
        ////						OggettiRow ImmobileAnater = new OggettiTable(ConstWrapper.sUsername).GetRow(IdImmobileAnater);
        ////
        ////
        ////						DettaglioTestataRow DettaglioAnater = new DettaglioTestataTable(ConstWrapper.sUsername).GetRow(IdImmobileAnater, RigaTestata.ID, false);
        ////							
        ////						OggettoImmobileCompleto OggettoImmobileAnater = new OggettoImmobileCompleto();
        ////
        ////						OggettoImmobileAnater.oImmobile = TrasformaOggettiAnater.TrasformaRigaOggetto(ImmobileAnater);
        ////							
        ////						OggettoImmobileAnater.oDettaglioTestata = TrasformaOggettiAnater.TrasformaRigaDettaglio(DettaglioAnater);
        ////
        //                        // preparo l'oggetto contitolari
        ////
        ////						DataTable DettaglioContitolari = new DettaglioTestataTable(ConstWrapper.sUsername).List(RigaTestata.ID, IdImmobileAnater);
        ////						
        ////						int idDettaglioContitolare;
        ////						int idSoggettoContitolare;
        ////
        ////						ArrayList arrayListContitolari = new ArrayList();
        ////
        ////						foreach (DataRow RigaContitolare in DettaglioContitolari.Rows)
        //                        //for (int i=0; i < DettaglioContitolari.Rows.Count; i++)
        ////						{
        //                            //idDettaglioContitolare = (int)DettaglioContitolari.Rows[i]["ID"];
        //                            //idSoggettoContitolare = (int)DettaglioContitolari.Rows[i]["idSoggetto"];
        ////							idDettaglioContitolare = (int)RigaContitolare["ID"];
        ////							idSoggettoContitolare = (int)RigaContitolare["idSoggetto"];
        ////
        //                            // prendo il record dettagliorow
        ////							DettaglioTestataRow oTestataContitolare = new DettaglioTestataTable(ConstWrapper.sUsername).GetRow(idDettaglioContitolare);
        ////							DettaglioAnagrafica oAnagrafeContitolare = HelperAnagrafica.GetDatiPersona(idSoggettoContitolare);
        ////							
        ////							OggettoContitolare oContitolare = new OggettoContitolare();
        ////
        ////							oContitolare.objDettaglioTestataContitolare =  TrasformaOggettiAnater.TrasformaRigaDettaglio(oTestataContitolare);
        ////							oContitolare.objDettaglioAnagraficaContitolare = oAnagrafeContitolare;
        ////
        ////							
        ////							arrayListContitolari.Add(oContitolare);
        ////						}
        ////
        ////						OggettoContitolare[] oContitolareAnater = (OggettoContitolare[])arrayListContitolari.ToArray(typeof(OggettoContitolare));
        ////
        ////						OggettoImmobileAnater.ArrayObjContitolare = oContitolareAnater;
        ////					}
        ////						
        ////					OggettoImmobileCompleto[] ArrayImmobiliCompleto = (OggettoImmobileCompleto[])arrayListImmobileCompleto.ToArray(typeof(OggettoImmobileCompleto));					
        ////
        ////					Type typeofRI = typeof(IRemotingInterfaceICI);
        ////					IRemotingInterfaceICI remObject = (IRemotingInterfaceICI)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLanaterICI"].ToString());
        ////					remObject.RibaltaInAnater(DatiContribuenteAnater, DatiDenuncianteAnater, TrasformaOggettiAnater.TrasformaRigaTestata(RigaTestata), ArrayImmobiliCompleto, ConstWrapper.CodiceEnte);// TrasformaRigaDettaglio(DettaglioAnater), TrasformaRigaOggetto(ImmobileAnater), oContitolareAnater , ConstWrapper.CodiceEnte);	
        //                }

        //            }

        //            }
        //            Testata.kill ();
       //   catch (Exception Err)
         //   {
          //      log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.lnkVerificaAnagrafica_Click.errore: ", Err);
            //    Response.Redirect("../PaginaErrore.aspx");
           //     throw Err;
           // }
    //        }

    /// <summary>
    /// Prende i dati del contribuente dai TextBox per salvare nell'Anagrafica.
    /// </summary>
    /// <param name="DatiContribuente"></param>
    private void DatiContribuentePerSalvare(ref DettaglioAnagrafica DatiContribuente)
        {
            DatiContribuente = new DettaglioAnagrafica();
            DatiContribuente.CodiceFiscale = txtCodFiscaleContr.Text;
            DatiContribuente.PartitaIva = txtPIVAContr.Text;
            DatiContribuente.Cognome = txtCognomeContr.Text;
            DatiContribuente.Nome = txtNomeContr.Text;
            //DatiContribuente.Sesso = rdbFemminaContr.Checked ? "F" : "M";
            try
            {
                if (DatiContribuente.Sesso == "")
                {
                    rdbMaschioContr.Checked = false;
                    rdbFemminaContr.Checked = false;
                    rdbGiuridicaContr.Checked = false;
                }
                if (DatiContribuente.Sesso == "M")
                    rdbMaschioContr.Checked = true;
                if (DatiContribuente.Sesso == "F")
                    rdbFemminaContr.Checked = true;
                if (DatiContribuente.Sesso == "G")
                    rdbGiuridicaContr.Checked = true;

                DatiContribuente.DataNascita = txtDataNascContr.Text;
                DatiContribuente.ComuneNascita = txtComNascContr.Text;
                DatiContribuente.ProvinciaNascita = txtProvNascContr.Text;
                DatiContribuente.ViaResidenza = txtViaResContr.Text;
                DatiContribuente.CivicoResidenza = txtNumCivResContr.Text;
                DatiContribuente.InternoCivicoResidenza = txtIntResContr.Text;
                DatiContribuente.ScalaCivicoResidenza = txtScalaResContr.Text;
                DatiContribuente.ComuneResidenza = txtComuneResContr.Text;
                DatiContribuente.ProvinciaResidenza = txtProvResContr.Text;
                DatiContribuente.CapResidenza = txtCapResContr.Text;
                DatiContribuente.EsponenteCivicoResidenza = txtEsponenteCivico.Text;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.DatiContribuentePerSalvare.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }
        }

        /// <summary>
        /// Prende i dati del denunciante dai TextBox per salvare nell'Anagrafica.
        /// </summary>
        /// <param name="DatiDenunciante"></param>
        private void DatiDenunciantePerSalvare(ref DettaglioAnagrafica DatiDenunciante)
        {
            DatiDenunciante = new DettaglioAnagrafica();
            DatiDenunciante.CodiceFiscale = txtCodFiscaleDen.Text;
            DatiDenunciante.PartitaIva = txtPIVADen.Text;
            DatiDenunciante.Cognome = txtCognomeDen.Text;
            DatiDenunciante.Nome = txtNomeDen.Text;
            //DatiDenunciante.Sesso = rdbFemminaDen.Checked ? "F" : "M";
            try { 
            if (DatiDenunciante.Sesso == "")
            {
                rdbMaschioDen.Checked = false;
                rdbFemminaDen.Checked = false;
                rdbGiuridicaDen.Checked = false;
            }
            if (DatiDenunciante.Sesso == "M")
                rdbMaschioDen.Checked = true;
            if (DatiDenunciante.Sesso == "F")
                rdbFemminaDen.Checked = true;
            if (DatiDenunciante.Sesso == "G")
                rdbGiuridicaDen.Checked = true;

            DatiDenunciante.DataNascita = txtDataNascDen.Text;
            DatiDenunciante.ComuneNascita = txtComNascDen.Text;
            DatiDenunciante.ProvinciaNascita = txtProvNascDen.Text;
            DatiDenunciante.ViaResidenza = txtViaResDen.Text;
            DatiDenunciante.CivicoResidenza = txtNumCivResDen.Text;
            DatiDenunciante.InternoCivicoResidenza = txtIntResDen.Text;
            DatiDenunciante.ScalaCivicoResidenza = txtScalaResDen.Text;
            DatiDenunciante.ComuneResidenza = txtComuneResDen.Text;
            DatiDenunciante.ProvinciaResidenza = txtProvResDen.Text;
            DatiDenunciante.CapResidenza = txtCapResDen.Text;
            DatiDenunciante.EsponenteCivicoResidenza = txtEsponenteCivicoDen.Text;
            DatiDenunciante.Professione = txtProfessione.Text;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.DatiDenunciantePerSalvare.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }
        }

        /*private void lnkVerificaContribuente_Click(object sender, System.EventArgs e)
		{
			//AbilitaContribuente(true);
		}*/

        /// <summary>
        /// Metodo invocato dal popup anagrafico che valorizza i campi a video
        /// con i dati della persona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRibalta_Click(object sender, System.EventArgs e)
        {
            DettaglioAnagrafica oDettaglioAnagrafica;
            try { 
            if (Session["contribuente"] != null)
            {
                Session["contribuenteANATER"] = Session["contribuente"];

                oDettaglioAnagrafica = (DettaglioAnagrafica)(Session["contribuente"]);
                txtCodFiscaleContr.Text = oDettaglioAnagrafica.CodiceFiscale;
                txtPIVAContr.Text = oDettaglioAnagrafica.PartitaIva;
                txtCognomeContr.Text = oDettaglioAnagrafica.Cognome;
                txtNomeContr.Text = oDettaglioAnagrafica.Nome;
                hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE.ToString();
                txtCodContribuenteCon1.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE.ToString();
                this.IDContribuente = oDettaglioAnagrafica.COD_CONTRIBUENTE;
                txtIdDataAnagcon.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA.ToString();
                /*if(oDettaglioAnagrafica.Sesso == "F")
					rdbFemminaContr.Checked = true;
				else
					rdbMaschioContr.Checked = true;*/
                if (oDettaglioAnagrafica.Sesso == "")
                {
                    rdbMaschioContr.Checked = false;
                    rdbFemminaContr.Checked = false;
                    rdbGiuridicaContr.Checked = false;
                }
                if (oDettaglioAnagrafica.Sesso == "M")
                    rdbMaschioContr.Checked = true;
                if (oDettaglioAnagrafica.Sesso == "F")
                    rdbFemminaContr.Checked = true;
                if (oDettaglioAnagrafica.Sesso == "G")
                    rdbGiuridicaContr.Checked = true;

                if (oDettaglioAnagrafica.DataNascita != "00/00/1900")
                {
                    txtDataNascContr.Text = oDettaglioAnagrafica.DataNascita;
                }
                else {
                    txtDataNascContr.Text = string.Empty;
                }
                txtComNascContr.Text = oDettaglioAnagrafica.ComuneNascita;
                txtProvNascContr.Text = oDettaglioAnagrafica.ProvinciaNascita;
                txtViaResContr.Text = oDettaglioAnagrafica.ViaResidenza;
                txtNumCivResContr.Text = oDettaglioAnagrafica.CivicoResidenza;
                txtIntResContr.Text = oDettaglioAnagrafica.InternoCivicoResidenza;
                txtScalaResContr.Text = oDettaglioAnagrafica.ScalaCivicoResidenza;
                txtComuneResContr.Text = oDettaglioAnagrafica.ComuneResidenza;
                txtProvResContr.Text = oDettaglioAnagrafica.ProvinciaResidenza;
                txtCapResContr.Text = oDettaglioAnagrafica.CapResidenza;
                txtEsponenteCivico.Text = oDettaglioAnagrafica.EsponenteCivicoResidenza;
            }
            else if (Session["denunciante"] != null)
            {
                Session["denuncianteANATER"] = Session["denunciante"];

                oDettaglioAnagrafica = (DettaglioAnagrafica)(Session["denunciante"]);
                txtCodFiscaleDen.Text = oDettaglioAnagrafica.CodiceFiscale;
                txtPIVADen.Text = oDettaglioAnagrafica.PartitaIva;
                txtCognomeDen.Text = oDettaglioAnagrafica.Cognome;
                txtNomeDen.Text = oDettaglioAnagrafica.Nome;
                txtCodContribuenteDen.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE.ToString();
                /*if(oDettaglioAnagrafica.Sesso == "F")
					rdbFemminaDen.Checked = true;
				else
					rdbMaschioDen.Checked = true;*/
                if (oDettaglioAnagrafica.Sesso == "")
                {
                    rdbMaschioDen.Checked = false;
                    rdbFemminaDen.Checked = false;
                    rdbGiuridicaDen.Checked = false;
                }
                if (oDettaglioAnagrafica.Sesso == "M")
                    rdbMaschioDen.Checked = true;
                if (oDettaglioAnagrafica.Sesso == "F")
                    rdbFemminaDen.Checked = true;
                if (oDettaglioAnagrafica.Sesso == "G")
                    rdbGiuridicaDen.Checked = true;

                if (oDettaglioAnagrafica.DataNascita != "00/00/1900")
                {
                    txtDataNascDen.Text = oDettaglioAnagrafica.DataNascita;
                }
                else {
                    txtDataNascDen.Text = string.Empty;
                }
                txtComNascDen.Text = oDettaglioAnagrafica.ComuneNascita;
                txtProvNascDen.Text = oDettaglioAnagrafica.ProvinciaNascita;
                txtViaResDen.Text = oDettaglioAnagrafica.ViaResidenza;
                txtNumCivResDen.Text = oDettaglioAnagrafica.CivicoResidenza;
                txtIntResDen.Text = oDettaglioAnagrafica.InternoCivicoResidenza;
                txtScalaResDen.Text = oDettaglioAnagrafica.ScalaCivicoResidenza;
                txtComuneResDen.Text = oDettaglioAnagrafica.ComuneResidenza;
                txtProvResDen.Text = oDettaglioAnagrafica.ProvinciaResidenza;
                txtCapResDen.Text = oDettaglioAnagrafica.CapResidenza;
                txtEsponenteCivicoDen.Text = oDettaglioAnagrafica.EsponenteCivicoResidenza;
                txtProfessione.Text = oDettaglioAnagrafica.Professione;
            }

            Session["contribuente"] = null;
            Session["denunciante"] = null;

            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.btnRibalta_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkEsisteDeninciante_CheckedChanged(object sender, System.EventArgs e)
        {
            AbilitaLabelDenunciante(chkEsisteDeninciante.Checked);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerificaDenunciante_Click(object sender, System.EventArgs e)
        {
            AbilitaControlliDenunciante(true);
        }

        /// <summary>
        /// Esegue l'eliminazione della dichiarazione.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
            /// <revisionHistory>
    /// <revision date="04/07/2012">
    /// <strong>IMU</strong>
    /// passaggio tributo da ICI a IMU
    /// </revision>
    /// </revisionHistory>
protected void btnElimina_Click(object sender, System.EventArgs e)
        {
            //new TestataTable(User.Identity.Name).CancellazioneLogica(this.IDTestata);
            try { 
            if ((txtIDquestionario.Text == "0") || (txtIDquestionario.Text.CompareTo("") == 0))
            {
                //non è possibile eliminare una dichiarazione per cui è stato emesso un questionario...
                if (new TestataTable(ConstWrapper.sUsername).CancellazioneDichiarazione(this.IDTestata))
                {

                    // in presenza di anater, effettuo la segnalazione sulla tabella messaggi.
                    if (bool.Parse(Business.ConstWrapper.UsoAnater))
                    {

                        Type typeofRI = typeof(IRemotingInterfaceICI);
                        IRemotingInterfaceICI remObject = (IRemotingInterfaceICI)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLanaterICI"].ToString());


                        // devo segnare gli immobili che sono stati eliminati nella tabella messaggi di anater.
                        // prelevo l'elenco degli immobili collegati alla testata

                        DataTable oImmobiliTable = new OggettiTable(ConstWrapper.sUsername).GetImmobileByIDTestata(this.IDTestata);

                        if (oImmobiliTable.Rows.Count > 0)
                        {
                                foreach (DataRow myRow in oImmobiliTable.Rows)
                                {
                                    OggettoMessaggio ObjMessage = new OggettoMessaggio();
                                ObjMessage.CodComune = int.Parse(ConstWrapper.CodiceEnte.ToString());
                                //*** 20120704 - IMU ***
                                ObjMessage.TestoMessaggio = "GESTIONE ANATER/VERTICALE ICI/IMU -- ";
                                ObjMessage.TestoMessaggio += "L\' immobile identificato con - ";
                                ObjMessage.TestoMessaggio += "Foglio : " + myRow["Foglio"].ToString() + "; ";
                                ObjMessage.TestoMessaggio += "Numero : " + myRow["Numero"].ToString() + "; ";
                                ObjMessage.TestoMessaggio += "Subalterno : " + myRow["Subalterno"].ToString() + ";";
                                ObjMessage.TestoMessaggio += " E' stato eliminato dalla base dati del verticale.";
                                ObjMessage.UtenteDestinatario = int.Parse("999");
                                ObjMessage.UtenteScrivente = int.Parse("111");

                                log.Debug("chiamo il metodo RibaltaInAnater");
                                log.Error("chiamo il metodo RibaltaInAnater");

                                bool bMessaggio = remObject.InsertMessaggi(ObjMessage);

                            }
                        }
                    }

                    if (new OggettiTable(ConstWrapper.sUsername).DeleteItemByIDTestata(this.IDTestata))
                    {
                        if (new DettaglioTestataTable(ConstWrapper.sUsername).DeleteItemByTestata(this.IDTestata))
                        {
                            //strscript = "alert('');";RegisterScript(sScript,this.GetType());, "err", strscript);this, "La dichiarazione è stato cancellata.");
                            btnIndietro_Click(sender, e);
                        }
                        else
                        {
                            btnIndietro_Click(sender, e); 
                        }
                    }
                    else
                    {
                        btnIndietro_Click(sender, e); 
                    }
                }
                else
                {
                    btnIndietro_Click(sender, e); 
                }

            }
            else
            {
                string strscript = "GestAlert('a', 'danger', '', '', 'Non è possibile eliminare una dichiarazione per cui è stato emesso un questionario.');";
                    RegisterScript(strscript,this.GetType());
            }


                //			txtCodContribuenteCon.Value = "";
                //			txtCodContribuenteDen.Value = "";
                //
                //			txtNumProt.Text = "";
                //			txtDataProt.Text = "";
                //			txtTotModelli.Text = "";
                //			txtAnnoDich.Text = "";
                //			txtDataDich.Text = "";
                //			txtNumDich.Text = "";
                //			ddlProvenienze.SelectedValue = ddlProvenienze.Items[0].Value;
                //
                //			txtCodFiscaleContr.Text = "";
                //			txtPIVAContr.Text = "";
                //			txtCognomeContr.Text = "";
                //			txtNomeContr.Text = "";
                //			rdbMaschioContr.Checked = false;
                //			rdbFemminaContr.Checked = false;
                //			txtDataNascContr.Text = "";
                //			txtComNascContr.Text = "";
                //			txtProvNascContr.Text = "";
                //			txtViaResContr.Text = "";
                //			txtNumCivResContr.Text = "";
                //			txtIntResContr.Text = "";
                //			txtScalaResContr.Text = "";
                //			txtComuneResContr.Text = "";
                //			txtProvResContr.Text = "";
                //			txtCapResContr.Text = "";
                //
                //			txtCodFiscaleDen.Text = "";
                //			txtPIVADen.Text = "";
                //			txtCognomeDen.Text = "";
                //			txtNomeDen.Text = "";
                //			rdbMaschioDen.Checked = false;
                //			rdbFemminaDen.Checked = false;
                //			txtDataNascDen.Text = "";
                //			txtComNascDen.Text = "";
                //			txtProvNascDen.Text = "";
                //			txtViaResDen.Text = "";
                //			txtNumCivResDen.Text = "";
                //			txtIntResDen.Text = "";
                //			txtScalaResDen.Text = "";
                //			txtComuneResDen.Text = "";
                //			txtProvResDen.Text = "";
                //			txtCapResDen.Text = "";
                //
                //			this.IDTestata = 0;
                //			btnImmobili.DataBind();
                //			btnElimina.DataBind();
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.btnElimina_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }
        }

        /// <summary>
        /// Dopo la bonifica automatica se non ci sono più errori metto invisibile la label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnUpdate_Click(object sender, System.EventArgs e)
        {
            Utility.DichManagerICI.TestataRow RigaTestata = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata, -1, ConstWrapper.StringConnection);

            int ImmobiliNonBonificati = new OggettiTable(ConstWrapper.sUsername).CountImmobiliNonBonificatiByIDTestata(this.IDTestata);
            int DettagliNonBonificati = new DettaglioTestataTable(ConstWrapper.sUsername).CountDettaglioNonBonificatiByIDTestata(this.IDTestata);
            try { 
            //if (RigaTestata.Bonificato == false || ImmobiliNonBonificati > 0 || DettagliNonBonificati > 0)
            //    lblBonificata.Visible = true;
            //else
                //lblBonificata.Visible = false;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.IbtnUpdate_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }
        }

        /// <summary>
        /// Va alla pagina degli immobili.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImmobili_Click(object sender, System.EventArgs e)
        {
            // Per visualizzare l'elenco degli immobili della dichiarazione
            //ApplicationHelper.LoadFrameworkPage("SR_IMMOBILI", "?IDTestata=" + this.IDTestata.ToString() + "&TYPEOPERATION=GESTIONE" );		
            // Inserimento di un nuovo immobile nella dichiarazione 
            //*** 20131003 - gestione atti compravendita ***
            //			ApplicationHelper.LoadFrameworkPage("SR_IMMOBILE_DETTAGLIO", "?IDTestata=" + this.IDTestata.ToString() + "&TYPEOPERATION=GESTIONE");
            RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_IMMOBILE_DETTAGLIO", "?IDTestata=" + this.IDTestata.ToString() + "&IdAttoCompraVendita=" + this.CompraVenditaId.ToString() + "&TYPEOPERATION=GESTIONE"), this.GetType());
            //*** ***
        }

        #region Abilita
        /// <summary>
        /// Abilita o disabilita i controlli della pagina.
        /// </summary>
        /// <param name="abilita"></param>
        private void Abilita(bool abilita)
        {
            lblCodiceFiscaleContr.Enabled = abilita;
            lblPartitaIVAContr.Enabled = abilita;
            lblCognomeContr.Enabled = abilita;
            lblNomeContr.Enabled = abilita;
            lblSessoContr.Enabled = abilita;
            lblDataNascitaContr.Enabled = abilita;
            lblComuneNascitaContr.Enabled = abilita;
            lblProvContr.Enabled = abilita;
            lblViaContr.Enabled = abilita;
            lblNumeroCivContr.Enabled = abilita;
            lblIntContr.Enabled = abilita;
            lblScalaContr.Enabled = abilita;
            lblComuneResidenzaContr.Enabled = abilita;
            lblProvinciaContr.Enabled = abilita;
            lblCAPContr.Enabled = abilita;
            lblEsponenteCivico.Enabled = abilita;
            txtEsponenteCivico.Enabled = abilita;

            chkEsisteDeninciante.Enabled = abilita;

            lblAnnoDichiarazione.Enabled = abilita;
            txtAnnoDich.Enabled = abilita;
            lblNumProtocollo.Enabled = abilita;
            txtNumProt.Enabled = abilita;
            lblNumDichiaraz.Enabled = abilita;
            txtNumDich.Enabled = abilita;
            lblDataProtocollo.Enabled = abilita;
            txtDataProt.Enabled = abilita;
            lblTotModelli.Enabled = abilita;
            txtTotModelli.Enabled = abilita;
            lblDataDichiaraz.Enabled = abilita;
            txtDataDich.Enabled = abilita;
            lblProvenienza.Enabled = abilita;
            ddlProvenienze.Enabled = abilita;

            lnkPulisciContr.Enabled = abilita;
            lblContrAnno.Enabled = abilita;
            lblContrContrib.Enabled = abilita;
        }

        /// <summary>
        /// Abilita le label della parte denunciante
        /// </summary>
        /// <param name="abilita"></param>
        private void AbilitaLabelDenunciante(bool abilita)
        {
            lnkVerificaDenunciante.Enabled = abilita;
            lblCodiceFiscaleDen.Enabled = abilita;
            lblPartitaIVADen.Enabled = abilita;
            lblCognomeDen.Enabled = abilita;
            lblNomeDen.Enabled = abilita;
            lblSessoDen.Enabled = abilita;
            lblDataNascitaDen.Enabled = abilita;
            lblComuneNascitaDen.Enabled = abilita;
            lblProvDen.Enabled = abilita;
            lblViaDen.Enabled = abilita;
            lblNumeroCivDen.Enabled = abilita;
            lblIntDen.Enabled = abilita;
            lblScalaDen.Enabled = abilita;
            lblComuneResidenzaDen.Enabled = abilita;
            lblProvinciaDen.Enabled = abilita;
            lblCAPDen.Enabled = abilita;
            lblEsponenteCivicoDen.Enabled = abilita;
            lblProfessione.Enabled = abilita;
            lnkPulisciDen.Enabled = abilita;
        }

        /// <summary>
        /// Abilita i controlli del denunciante.
        /// </summary>
        /// <param name="abilita"></param>
        private void AbilitaControlliDenunciante(bool abilita)
        {
            txtCodFiscaleDen.Enabled = abilita;
            txtPIVADen.Enabled = abilita;
            txtCognomeDen.Enabled = abilita;
            txtNomeDen.Enabled = abilita;
            rdbFemminaDen.Enabled = abilita;
            rdbMaschioDen.Enabled = abilita;
            txtDataNascDen.Enabled = abilita;
            txtComNascDen.Enabled = abilita;
            txtProvNascDen.Enabled = abilita;
            txtViaResDen.Enabled = abilita;
            txtNumCivResDen.Enabled = abilita;
            txtIntResDen.Enabled = abilita;
            txtScalaResDen.Enabled = abilita;
            txtComuneResDen.Enabled = abilita;
            txtProvResDen.Enabled = abilita;
            txtCapResDen.Enabled = abilita;
        }
        #endregion

        /// <summary>
        /// Abilita o disabilita i controlli.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAbilita_Click(object sender, System.EventArgs e)
        {
            try { 
            if (!lblCodiceFiscaleContr.Enabled)
            {
                Abilita(true);
                AbilitaLabelDenunciante(chkEsisteDeninciante.Checked);
            }
            else
            {
                Abilita(false);
                AbilitaLabelDenunciante(false);
            }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.btnAbilita_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerificaDenunciante_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            HelperAnagrafica.PopUpAnagrafica(Page, "denunciante", txtCodContribuenteDen.Value, txtIdDataAnagDen.Text);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIndietro_Click(object sender, System.EventArgs e)
        {
            try { 
            if (txtTypeOperation.Text.ToUpper() == "GESTIONE")
            {
                // ritona alla pagina Gestione.aspx
                //ApplicationHelper.LoadFrameworkPage("SR_GESTIONE", String.Empty);

                // 23022007 Fabi - Ritorna alla pagina GestioneDettaglio.aspx
                Utility.DichManagerICI.TestataRow RigaTestata = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata, -1, ConstWrapper.StringConnection);
                Business.CoreUtility objUtility = new Business.CoreUtility();

                if ((RigaTestata.IDContribuente.ToString() != "0") && Session["BLN_BONIFICATOVALUE"] != null)
                        // se c'è il contribuente ritorna a GestioneDettaglio.aspx
                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE_DETTAGLIO", "?IDContribuente=" + (int)RigaTestata.IDContribuente + "&Bonificato=" + Session["BLN_BONIFICATOVALUE"].ToString()), this.GetType());
                else
                        // se è un dichiarazione vuota ritorna a Gestione.aspx
                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE", String.Empty), this.GetType());
            }

            else if (txtTypeOperation.Text.ToUpper() == "BONIFICA")
            {
                    RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_BONIFICA_DICH", String.Empty), this.GetType());
            }
            else if (txtTypeOperation.Text.ToUpper() == "DETTAGLIO")
            {

                DataView ListaTestate = new TestataTable(ConstWrapper.sUsername).List(this.IDContribuente, Bonificato.Tutte);
                //TestataRow RigaTestata = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata);
                Business.CoreUtility objUtility = new Business.CoreUtility();

                if (ListaTestate.Count > 0)
                {
                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE_DETTAGLIO", "?IDContribuente=" + this.IDContribuente + "&Bonificato=" + Session["BLN_BONIFICATOVALUE"].ToString()), this.GetType());
                }
                else
                {
                        // se è un dichiarazione vuota ritorna a Gestione.aspx
                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE", String.Empty), this.GetType());
                }
                //ApplicationHelper.LoadFrameworkPage("SR_GESTIONE_DETTAGLIO", "?IDContribuente=" + (int)RigaTestata.IDContribuente  + "&Bonificato=" + objUtility.BoolToDb(RigaTestata.Bonificato));

            }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.btnIndietro_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }
        }

        /// <summary>
        /// Ribalta i dati della dichiarazione nel database di Anater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRibaltaInAnater_Click(object sender, System.EventArgs e)
        {

            try
            {

                string str;
                bool UpdateAnagAnater = false;
                bool UpdateAnagVerticale = false;
                str = txtUpdateAnagraficaValue.Text;

                if (str.ToUpper() == "FALSE")
                {
                    UpdateAnagAnater = false;
                }
                else if (str.ToUpper() == "TRUE")
                {
                    UpdateAnagAnater = true;
                }
                else if (str.ToUpper() == "TRUE VERTICALE")
                {
                    UpdateAnagVerticale = true;
                }

                TrasformatoreAnater TrasformaOggettiAnater = new TrasformatoreAnater();

                log.Debug("Carico i dati della testata con id= " + this.IDTestata);
                log.Error("Carico i dati della testata con id= " + this.IDTestata);
                Utility.DichManagerICI.TestataRow RigaTestata = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata, -1, ConstWrapper.StringConnection);

                //Type typeofRI = typeof(IRemotingInterfaceICI);
                //IRemotingInterfaceICI remObject = (IRemotingInterfaceICI)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLanaterICI"].ToString());

                TestataRowICI oTestataANATER = new TestataRowICI();
                //DettaglioTestataRowICI oDettaglioTestataANATER=new DettaglioTestataRowICI();
                //OggettiRowICI oImmobileANATER=new OggettiRowICI();
                DettaglioAnagrafica oAnagraficaContitolare = new DettaglioAnagrafica();

                //oTestataANATER=typeof(TestataRow);
                //oTestataANATER=(TestataRowICI)RigaTestata;
                TrasformatoreAnater TrasformaOggetti = new TrasformatoreAnater();

                oTestataANATER = TrasformaOggetti.TrasformaRigaTestata(RigaTestata);

                //remObject.RibaltaInAnater((DettaglioAnagrafica)Session["contribuenteANATER"],(DettaglioAnagrafica)Session["denuncianteANATER"], oTestataANATER, oDettaglioTestataANATER,oImmobileANATER ,oAnagraficaContitolare,"7026");


                // SE LA DICHIARAZIONE RISULTA BONIFICATA RIBALTO IN ANATER
                // preparo l'oggetto testata
                // TestataRow RigaTestataAnater = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata);

                DettaglioAnagrafica DatiContribuenteAnater;
                DettaglioAnagrafica DatiDenuncianteAnater;

                log.Debug("preparo l'oggetto DettaglioAnagrafica col contribuente");
                log.Error("preparo l'oggetto DettaglioAnagrafica col contribuente");

                // preparo l'oggetto DettaglioAnagrafica col contribuente
                if (RigaTestata.IDContribuente != 0)
                    DatiContribuenteAnater = HelperAnagrafica.GetDatiPersona(long.Parse(RigaTestata.IDContribuente.ToString()));
                else
                    DatiContribuenteAnater = new DettaglioAnagrafica();

                // preparo l'oggetto denunciante
                if (RigaTestata.IDDenunciante != 0)
                    DatiDenuncianteAnater = HelperAnagrafica.GetDatiPersona(long.Parse(RigaTestata.IDDenunciante.ToString()));
                else
                    DatiDenuncianteAnater = new DettaglioAnagrafica();



                ArrayList arrayListImmobileCompleto = new ArrayList();

                log.Debug("partendo dall'id testata prendo l'elenco di tutti gli immobili presenti nella dichiarazione");
                log.Error("partendo dall'id testata prendo l'elenco di tutti gli immobili presenti nella dichiarazione");

                // partendo dall'id testata prendo l'elenco di tutti gli immobili presenti nella dichiarazione
                DataTable ImmobiliDichiarazione = new OggettiTable(ConstWrapper.sUsername).GetImmobileByIDTestata(this.IDTestata);

                //08/02/2008 - COMMENTATO
                //				foreach(DataRow RigaImmobile in ImmobiliDichiarazione.Rows)
                //				{
                //						
                //					int IdImmobileAnater = (int)RigaImmobile["ID"];
                // prendo la riga dell'immobile
                //					OggettiRow ImmobileAnater = new OggettiTable(ConstWrapper.sUsername).GetRow(IdImmobileAnater);
                //
                //
                //					DettaglioTestataRow DettaglioAnater = new DettaglioTestataTable(ConstWrapper.sUsername).GetRow(IdImmobileAnater, RigaTestata.ID, false);
                //								
                //					OggettoImmobileCompleto OggettoImmobileAnater = new OggettoImmobileCompleto();
                //				
                //					OggettoImmobileAnater.oImmobile = TrasformaOggettiAnater.TrasformaRigaOggetto(ImmobileAnater);
                //								
                //					OggettoImmobileAnater.oDettaglioTestata = TrasformaOggettiAnater.TrasformaRigaDettaglio(DettaglioAnater);
                //
                // preparo l'oggetto contitolari
                //
                //					log.Debug ("preparo l'oggetto contitolari" );
                //					log.Error ("preparo l'oggetto contitolari" );
                //
                //					DataTable DettaglioContitolari = new DettaglioTestataTable(ConstWrapper.sUsername).List(RigaTestata.ID, IdImmobileAnater);
                //							
                //					int idDettaglioContitolare;
                //					int idSoggettoContitolare;
                //
                //					ArrayList arrayListContitolari = new ArrayList();
                //
                //					foreach (DataRow RigaContitolare in DettaglioContitolari.Rows)
                //for (int i=0; i < DettaglioContitolari.Rows.Count; i++)
                //					{
                //idDettaglioContitolare = (int)DettaglioContitolari.Rows[i]["ID"];
                //idSoggettoContitolare = (int)DettaglioContitolari.Rows[i]["idSoggetto"];
                //						idDettaglioContitolare = (int)RigaContitolare["ID"];
                //						idSoggettoContitolare = (int)RigaContitolare["idSoggetto"];
                //
                // prendo il record dettagliorow
                //						DettaglioTestataRow oTestataContitolare = new DettaglioTestataTable(ConstWrapper.sUsername).GetRow(idDettaglioContitolare);
                //						DettaglioAnagrafica oAnagrafeContitolare = HelperAnagrafica.GetDatiPersona(long.Parse(idSoggettoContitolare.ToString()));
                //									
                //						OggettoContitolare oContitolare = new OggettoContitolare();
                //
                //						oContitolare.objDettaglioTestataContitolare =  TrasformaOggettiAnater.TrasformaRigaDettaglio(oTestataContitolare);
                //						oContitolare.objDettaglioAnagraficaContitolare = oAnagrafeContitolare;
                //
                //									
                //						arrayListContitolari.Add(oContitolare);
                //					}
                //
                //					OggettoContitolare[] oContitolareAnater = (OggettoContitolare[])arrayListContitolari.ToArray(typeof(OggettoContitolare));
                //
                //					OggettoImmobileAnater.ArrayObjContitolare = oContitolareAnater;
                //
                //					arrayListImmobileCompleto.Add(OggettoImmobileAnater);
                //				}

                log.Debug("carico l'oggetto OggettoImmobileCompleto");
                log.Error("carico l'oggetto OggettoImmobileCompleto");

                OggettoImmobileCompleto[] ArrayImmobiliCompleto = (OggettoImmobileCompleto[])arrayListImmobileCompleto.ToArray(typeof(OggettoImmobileCompleto));



                try
                {
                    bool iRetValRibaltaAnater;
                    Type typeofRI = typeof(IRemotingInterfaceICI);
                    IRemotingInterfaceICI remObject = (IRemotingInterfaceICI)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLanaterICI"].ToString());

                    log.Debug("chiamo il metodo RibaltaInAnater");
                    log.Error("chiamo il metodo RibaltaInAnater");

                    iRetValRibaltaAnater = remObject.RibaltaInAnater(DatiContribuenteAnater, DatiDenuncianteAnater, TrasformaOggettiAnater.TrasformaRigaTestata(RigaTestata), ArrayImmobiliCompleto, ConstWrapper.CodiceEnte, UpdateAnagAnater, UpdateAnagVerticale, int.Parse(Session["COD_OPERATORE"].ToString()));// TrasformaRigaDettaglio(DettaglioAnater), TrasformaRigaOggetto(ImmobileAnater), oContitolareAnater , ConstWrapper.CodiceEnte);	

                    string strscript = "GestAlert('a', 'warning', '', '', 'Ribaltamento della testata in Anater" + (iRetValRibaltaAnater == true ? "" : " non") + " effettuato.');";
                    RegisterScript(strscript,this.GetType());
                }
                catch (Exception ex)
                {
                    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.btnRibaltaInAnater_Click.errore: ", ex);
                    Response.Redirect("../PaginaErrore.aspx");
                }
            }
            catch (Exception exception)
            {
                //string pippo=exception.Message;
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.btnRibaltaInAnater_Click.errore: ", exception);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }

        private void txtNumProt_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void ddlProvenienze_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDatiAggiuntivi_Click(object sender, System.EventArgs e)
        {
            string strScript = "";
            strScript = "";
            strScript += "window.open('DatiAggiuntivi/DatiAgg.aspx?IDTestata=" + this.IDTestata + "')";
            strScript += "";
            RegisterStartupScript("", strScript);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerificaContribuente_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            HelperAnagrafica.PopUpAnagrafica(Page, "contribuente", txtCodContribuenteCon1.Text, txtIdDataAnagcon.Text);
        }

        //*** 20141110 - passaggio di proprietà ***
        private void LoadDichiarazione(int IsPassProp)
        {
            try
            {
                Utility.DichManagerICI.TestataRow RigaTestata = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata, -1, ConstWrapper.StringConnection);
                hdIdContribuente.Value = RigaTestata.IDContribuente.ToString();
                this.IDContribuente = RigaTestata.IDContribuente;
                txtCodContribuenteCon1.Text = RigaTestata.IDContribuente.ToString();
                txtCodContribuenteDen.Value = RigaTestata.IDDenunciante.ToString();

                txtNumProt.Text = RigaTestata.NumeroProtocollo;
                txtDataProt.Text = RigaTestata.DataProtocollo == DateTime.MinValue ? String.Empty : RigaTestata.DataProtocollo.ToShortDateString();
                txtTotModelli.Text = RigaTestata.TotaleModelli;
                txtAnnoDich.Text = RigaTestata.AnnoDichiarazione;
                if (RigaTestata.DataInizio == DateTime.MinValue)
                    txtDataDich.Text = string.Empty;
                else
                    txtDataDich.Text = RigaTestata.DataInizio.ToShortDateString();
                if (RigaTestata.DataFine == DateTime.MinValue)
                    TxtDataPassProp.Text = string.Empty;
                else
                    TxtDataPassProp.Text = RigaTestata.DataFine.ToShortDateString();

                if (RigaTestata.NumeroDichiarazione == -1)
                    txtNumDich.Text = string.Empty;
                else
                    txtNumDich.Text = RigaTestata.NumeroDichiarazione.ToString();

                txtIDquestionario.Text = RigaTestata.IDQuestionario.ToString();
                ddlProvenienze.SelectedValue = RigaTestata.IDProvenienza.ToString();

                //int ImmobiliNonBonificati = new OggettiTable(ConstWrapper.sUsername).CountImmobiliNonBonificatiByIDTestata(this.IDTestata);
                //int DettagliNonBonificati = new DettaglioTestataTable(ConstWrapper.sUsername).CountDettaglioNonBonificatiByIDTestata(this.IDTestata);
                //if (RigaTestata.Bonificato == false || ImmobiliNonBonificati > 0 || DettagliNonBonificati > 0)
                //    lblBonificata.Visible = true;

                hdIsPassProg.Value = IsPassProp.ToString();
                if (IsPassProp == 1)
                {
                    hdIdContribuente.Value = "-1";
                    this.IDContribuente = 0;
                    txtCodContribuenteCon1.Text = "";
                    txtCodContribuenteDen.Value = "";
                    lnkPulisciContr.Enabled = true; TxtDataPassProp.Enabled = true;
                    LoadContribuente();
                }
                else
                {
                    if (RigaTestata.IDContribuente != 0)
                        LoadContribuente();

                    if (RigaTestata.IDDenunciante != 0)
                        LoadDenunciante();
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.LoadDichiarazione.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }
        private void LoadContribuente()
        {
            try
            {
                DettaglioAnagrafica DatiContribuente = HelperAnagrafica.GetDatiPersona(long.Parse(hdIdContribuente.Value));
                //*** 201504 - Nuova Gestione anagrafica con form unico ***
                if (ConstWrapper.HasPlainAnag)
                {
                    if (hdIdContribuente.Value != "" && int.Parse(hdIdContribuente.Value) > 0)
                        ifrmAnag.Attributes.Add("src", "../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_LETTURA.ToString());
                    else
                        ifrmAnag.Attributes.Add("src", "../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString());
                }
                else
                {
                    txtCodFiscaleContr.Text = DatiContribuente.CodiceFiscale;
                    txtPIVAContr.Text = DatiContribuente.PartitaIva;
                    txtCognomeContr.Text = DatiContribuente.Cognome;
                    txtNomeContr.Text = DatiContribuente.Nome;
                    txtIdDataAnagcon.Text = DatiContribuente.ID_DATA_ANAGRAFICA.ToString();
                    if (DatiContribuente.Sesso == "")
                    {
                        rdbMaschioContr.Checked = false;
                        rdbFemminaContr.Checked = false;
                        rdbGiuridicaContr.Checked = false;
                    }
                    if (DatiContribuente.Sesso == "M")
                        rdbMaschioContr.Checked = true;
                    if (DatiContribuente.Sesso == "F")
                        rdbFemminaContr.Checked = true;
                    if (DatiContribuente.Sesso == "G")
                        rdbGiuridicaContr.Checked = true;

                    if (DatiContribuente.DataNascita != "00/00/1900")
                    {
                        txtDataNascContr.Text = DatiContribuente.DataNascita;
                    }
                    else
                    {
                        txtDataNascContr.Text = string.Empty;
                    }
                    txtComNascContr.Text = DatiContribuente.ComuneNascita;
                    txtProvNascContr.Text = DatiContribuente.ProvinciaNascita;
                    txtViaResContr.Text = DatiContribuente.ViaResidenza;
                    txtNumCivResContr.Text = DatiContribuente.CivicoResidenza;
                    txtIntResContr.Text = DatiContribuente.InternoCivicoResidenza;
                    txtScalaResContr.Text = DatiContribuente.ScalaCivicoResidenza;
                    txtComuneResContr.Text = DatiContribuente.ComuneResidenza;
                    txtProvResContr.Text = DatiContribuente.ProvinciaResidenza;
                    txtCapResContr.Text = DatiContribuente.CapResidenza;
                    txtEsponenteCivico.Text = DatiContribuente.EsponenteCivicoResidenza;
                    ControlliContribuenteBind();
                }
                Session.Add("contribuenteANATER", DatiContribuente);
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.LoadContribuente.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }
        private void LoadDenunciante()
        {
            try
            {
                chkEsisteDeninciante.Checked = true;
                DettaglioAnagrafica DatiDenunciante = HelperAnagrafica.GetDatiPersona(long.Parse(txtCodContribuenteDen.Value));
                txtCodFiscaleDen.Text = DatiDenunciante.CodiceFiscale;
                txtIdDataAnagDen.Text = DatiDenunciante.ID_DATA_ANAGRAFICA.ToString();
                txtPIVADen.Text = DatiDenunciante.PartitaIva;
                txtCognomeDen.Text = DatiDenunciante.Cognome;
                txtNomeDen.Text = DatiDenunciante.Nome;
                if (DatiDenunciante.Sesso == "")
                {
                    rdbMaschioDen.Checked = false;
                    rdbFemminaDen.Checked = false;
                    rdbGiuridicaDen.Checked = false;
                }
                if (DatiDenunciante.Sesso == "M")
                    rdbMaschioDen.Checked = true;
                if (DatiDenunciante.Sesso == "F")
                    rdbFemminaDen.Checked = true;
                if (DatiDenunciante.Sesso == "G")
                    rdbGiuridicaDen.Checked = true;

                if (DatiDenunciante.DataNascita != "00/00/1900")
                    txtDataNascDen.Text = DatiDenunciante.DataNascita;
                else
                    txtDataNascDen.Text = string.Empty;

                txtComNascDen.Text = DatiDenunciante.ComuneNascita;
                txtProvNascDen.Text = DatiDenunciante.ProvinciaNascita;
                txtViaResDen.Text = DatiDenunciante.ViaResidenza;
                txtNumCivResDen.Text = DatiDenunciante.CivicoResidenza;
                txtIntResDen.Text = DatiDenunciante.InternoCivicoResidenza;
                txtScalaResDen.Text = DatiDenunciante.ScalaCivicoResidenza;
                txtComuneResDen.Text = DatiDenunciante.ComuneResidenza;
                txtProvResDen.Text = DatiDenunciante.ProvinciaResidenza;
                txtCapResDen.Text = DatiDenunciante.CapResidenza;
                txtEsponenteCivicoDen.Text = DatiDenunciante.EsponenteCivicoResidenza;
                txtProfessione.Text = DatiDenunciante.Professione;

                Session.Add("denuncianteANATER", DatiDenunciante);
                ControlliDenuncianteBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.LoadDenunciante.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalva_Click(object sender, System.EventArgs e)
        {
            try
            {
                bool retval = true;
                Utility.DichManagerICI.TestataRow RigaTestata = new Utility.DichManagerICI.TestataRow();
                RigaTestata.ID = this.IDTestata;
                RigaTestata.Ente = ConstWrapper.CodiceEnte;
                if (txtNumDich.Text == "")
                    RigaTestata.NumeroDichiarazione = -1;
                else
                    RigaTestata.NumeroDichiarazione = int.Parse(txtNumDich.Text);
                RigaTestata.AnnoDichiarazione = txtAnnoDich.Text;
                RigaTestata.NumeroProtocollo = txtNumProt.Text;
                RigaTestata.DataProtocollo = txtDataProt.Text == String.Empty ? DateTime.MinValue : Convert.ToDateTime(txtDataProt.Text);
                RigaTestata.TotaleModelli = txtTotModelli.Text;
                RigaTestata.DataInizio = txtDataDich.Text == String.Empty ? DateTime.MinValue : Convert.ToDateTime(txtDataDich.Text);
                RigaTestata.DataFine = TxtDataPassProp.Text == String.Empty ? DateTime.MaxValue : Convert.ToDateTime(TxtDataPassProp.Text);
                RigaTestata.Bonificato = false;
                RigaTestata.Annullato = false;
                RigaTestata.DataInizioValidità = DateTime.Now;
                RigaTestata.DataFineValidità = DateTime.MinValue;
                RigaTestata.Operatore = ConstWrapper.sUsername;
                RigaTestata.IDContribuente = hdIdContribuente.Value == String.Empty ? 0 : int.Parse(hdIdContribuente.Value);
                RigaTestata.IDDenunciante = txtCodContribuenteDen.Value == String.Empty ? 0 : int.Parse(txtCodContribuenteDen.Value);
                RigaTestata.IDProvenienza = int.Parse(ddlProvenienze.SelectedValue);

                if (hdIsPassProg.Value == "1")
                    retval = SetPassaggioProp(RigaTestata);
                else
                    retval = SetTestata(RigaTestata);

                btnImmobili.DataBind();
                btnElimina.DataBind();
                string stringaImmob = "GestAlert('a', '" + (retval == true ? "success" : "warning") + "', '', '', 'Salvataggio" + (retval == true ? "" : " non") + " effettuato.');parent.Comandi.document.getElementById('New').style.display = ''";
                RegisterScript(stringaImmob,this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.btnSalva_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }

        private bool SetTestata(Utility.DichManagerICI.TestataRow myItem)
        {
            TestataTable Testata = new TestataTable(ConstWrapper.sUsername);
            int idTestata;
            try
            {
                bool retval = true;
                //effettuo il salvataggio dei dati
                if (this.IDTestata == 0)
                {
                    //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                    //retval = Testata.Insert(myItem, out idTestata);
                    retval = new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetTestata(Utility.Costanti.AZIONE_NEW, myItem, out idTestata);
                    //*** ***
                    this.IDTestata = idTestata;
                }
                else
                {
                    //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                    //retval = Testata.Modify(myItem);
                    retval = new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetTestata(Utility.Costanti.AZIONE_UPDATE, myItem, out idTestata);
                    //*** ***
                }

                if (retval)
                {
                    // Faccio una bonifica di quanto inserito per marcare gli errori
                    /*bool RetVal;
                    RetVal = new ModuloIci(ConstWrapper.sUsername).BonificaDichiarazione(IDTestata, ConstWrapper.StringConnection);
                    // se la dichiarazione non è stata bonificata apro il PopUp degli errori.
                    if (!RetVal)
                    {
                        string strScript = "javascript:window.showModalDialog('PopUpErroriDichiarazione.aspx?IDTestata=" + IDTestata + "', window, 'dialogHeight: 400px; dialogWidth: 400px; status: no');";
                        RegisterScript(sScript,this.GetType());,"", "" + strScript + "");
                    }
                    else
                    {
                        lblBonificata.Visible = false;
                    }
                    lblMesImmobile.Visible = true;

                    // se la bonifica va a buon fine ribalta in anater
                    if (RetVal)
                    {*/
                    if (ConstWrapper.UsoAnater == "true")
                    {
                        Type typeofRI = typeof(IRemotingInterfaceICI);
                        IRemotingInterfaceICI remObject = (IRemotingInterfaceICI)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLanaterICI"].ToString());

                        int iRetValControlloAnagraficaAnater;
                        iRetValControlloAnagraficaAnater = remObject.ControlloAnagraficaAnater((DettaglioAnagrafica)Session["contribuenteANATER"], (DettaglioAnagrafica)Session["denuncianteANATER"], ConstWrapper.CodiceEnte);
                        //valori ritornati:
                        //0- contribuente trovato -->residente
                        //1- contribuente trovato --> non  residente --> dati non variati
                        //2- contribuente trovato -->non  residente --> dati variati
                        //3- contribuente non trovato -->nuovo inserimento
                        string strscript = "RibaltaInAnater(" + iRetValControlloAnagraficaAnater + ");";
                        RegisterScript(strscript,this.GetType());
                    }
                    /*}*/
                }
                return retval;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.SetTestata.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                return false;
            }
            finally
            {
                Testata.kill();
            }
        }
        private bool SetPassaggioProp(Utility.DichManagerICI.TestataRow myItem)
        {
            TestataTable Testata = new TestataTable(ConstWrapper.sUsername);
            try
            {
                bool retval = true;
                Session["ListUIPassProp"] = null;

                DataView dvListUI = new DataView();
                retval = Testata.PassaggioProp(myItem, out dvListUI);

                if (retval)
                {
                    if (dvListUI.Count > 0)
                    {
                        //ricarico la pagina
                        Session["ListUIPassProp"] = dvListUI;
                        this.IDTestata = int.Parse(dvListUI[0]["IDTESTATA"].ToString());
                        LoadDichiarazione(0);
                        Abilita(true);
                        AbilitaLabelDenunciante(chkEsisteDeninciante.Checked);
                        string strscript;
                        strscript = "parent.Comandi.document.getElementById('New').style.display = 'none'";
                        strscript += "Search(" + this.IDTestata + ");";
                        RegisterScript(strscript,this.GetType());
                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE", String.Empty), this.GetType());
                    }
                    else
                        retval = false;
                }
                return retval;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.SetPassaggioProp.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                return false;
            }
            finally
            {
                Testata.kill();
            }
        }
        //*** ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdPrecarica_Click(object sender, System.EventArgs e)
        {
            int nret;
            /* INIZIO CARICAMENTO OGEETTI */
            DettaglioAnagrafica dettAnagrafica = new DettaglioAnagrafica();
            dettAnagrafica = new DichiarazioniView().GetCompraVenditaAnagrafica(int.Parse(Request.QueryString["IdAttoCompravendita"]), int.Parse(Request.QueryString["IdAttoSoggetto"]));
            Anagrafica.DLL.GestioneAnagrafica gestAnagrafica = new Anagrafica.DLL.GestioneAnagrafica();
            DettaglioAnagraficaReturn dettAnagraficaReturn = new DettaglioAnagraficaReturn();
            dettAnagraficaReturn = gestAnagrafica.GestisciAnagrafica(dettAnagrafica, ConstWrapper.DBType, ConstWrapper.StringConnectionAnagrafica, false, false);
            hdIdContribuente.Value = 8929.ToString();/*dettAnagraficaReturn.COD_CONTRIBUENTE;*/
            Utility.DichManagerICI.TestataRow MyTestata = new Utility.DichManagerICI.TestataRow();
            DataTable myData = new DichiarazioniView().GetCompraVendita(int.Parse(Request.QueryString["IdAttoCompraVendita"]));
            try {
                foreach (DataRow myRow in myData.Rows)
                {
                    MyTestata.Ente = Business.ConstWrapper.CodiceEnte;
                MyTestata.Operatore = Business.ConstWrapper.sUsername;
                MyTestata.AnnoDichiarazione = DateTime.Parse(myRow["DataPresentazione"].ToString()).Year.ToString();
                MyTestata.NumeroDichiarazione = int.Parse(myRow["NumNota"].ToString());
                MyTestata.AnnoDichiarazione = DateTime.Parse(myRow["DataPresentazione"].ToString()).Year.ToString();
                MyTestata.DataInizioValidità = DateTime.Now;
                MyTestata.IDProvenienza = 41;
                MyTestata.IDContribuente = int.Parse(hdIdContribuente.Value);
            }
            Utility.DichManagerICI.OggettiRow MyOggettiRow = new Utility.DichManagerICI.OggettiRow();
            Utility.DichManagerICI.DettaglioTestataRow MyDettaglioTestataRow = new Utility.DichManagerICI.DettaglioTestataRow();
            MyOggettiRow = ImmobileBind(int.Parse(Request.QueryString["IdAttoCompraVendita"]), int.Parse(hdIdContribuente.Value));
            MyDettaglioTestataRow = DettaglioBind(int.Parse(Request.QueryString["IdAttoCompraVendita"]));
            /* FINE CARICAMENTO OGGETTI */

            /* INIZIO SALVATAGGIO OGGETTI SU DB */
            SetTestata(MyTestata);
            if (this.IDTestata > 0)
            {
                new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetOggetti(Utility.Costanti.AZIONE_UPDATE, MyOggettiRow, this.IDTestata, out nret);
                if (nret > 0)
                {
                    MyDettaglioTestataRow.IdOggetto = nret;
                    MyDettaglioTestataRow.IdTestata = this.IDTestata;
                    new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetDettaglioTestataCompleta(MyDettaglioTestataRow, out nret);
                    if (nret > 0)
                    {
                        /* FINE SALVATAGGIO OGGETTI SU DB */

                        /* INIZIO CARICAMENTO OGGETTI A VIDEO */
                        LoadDichiarazione(0);
                        HtmlControl myFrame = (HtmlControl)(this.FindControl("loadImmobile"));
                        myFrame.Attributes.Add("src", "Immobile.aspx?IdTestata=" + this.IDTestata);
                        /* FINE CARICAMENTO OGGETTI A VIDEO */

                    }
                    else
                    {
                        string strscript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento Dettaglio');";
                        RegisterScript(strscript, this.GetType());
                    }
                }
                else
                {
                    string strscript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento Oggetti');";
                    RegisterScript(strscript, this.GetType());
                }
            }
            else
            {
                string strscript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento');";
                RegisterScript(strscript, this.GetType());
            }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.cmdPrecari_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }

        private Utility.DichManagerICI.OggettiRow ImmobileBind(int idAttoCompraVendita, int idContribuente)
        {
            try
            {
                //log.Debug ("INIZIO ImmobileBind");
                Utility.DichManagerICI.OggettiRow Riga = new Utility.DichManagerICI.OggettiRow();
                //*** 20131003 - gestione atti compravendita ***

                log.Debug("ImmobileBind::carico da oggetti compravendita::idAttoCompraVendita::" + idAttoCompraVendita.ToString() + "::idContribuente::" + idContribuente.ToString());
                Riga = new DichiarazioniView().GetCompraVenditaOggetti(idAttoCompraVendita, -1, int.Parse(hdIdContribuente.Value));
                //*** ***
                //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***               
                //*** ***
                return Riga;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.ImmobileBind.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                return new Utility.DichManagerICI.OggettiRow();
            }


        }

        private Utility.DichManagerICI.DettaglioTestataRow DettaglioBind(int idAttoCompraVendita)
        {
            try
            {
                //log.Debug ("INIZIO DettaglioBind");
                Utility.DichManagerICI.DettaglioTestataRow Riga = new Utility.DichManagerICI.DettaglioTestataRow();
                //*** 20131003 - gestione atti compravendita ***                               
                Riga = new DichiarazioniView().GetCompraVenditaDettaglioTestata(idAttoCompraVendita, -1, int.Parse(hdIdContribuente.Value));
                return Riga;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazionePage.DettaglioBind.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                return new Utility.DichManagerICI.DettaglioTestataRow();
            }
        }            
    }	
}
