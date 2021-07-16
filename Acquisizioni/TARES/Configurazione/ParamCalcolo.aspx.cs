using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FutureFog.U4N.General;
using OPENgovDL;
using Ribes.OPENgov.Utilities;
using System.Data;
using log4net;

namespace OPENgov.Acquisizioni.TARES.Configurazione
{/// <summary>
/// Pagina per la consultazione/configurazione dei parametri generali di calcolo.
/// Le possibili opzioni sono:
/// - Inserisci nuovo
/// - Salva
/// - Torna alla videata precedente
/// </summary>
    public partial class ParamCalcolo : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ParamCalcolo));
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BreadCrumb = "TARES/TARI - Configurazione - Parametri Calcolo";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadCombo();
            LoadGridView(0);
        }
        #region "Bottoni"
        protected void CmdSaveClick(object sender, EventArgs e)
        {
            try
            {
                string typeMQ = "D";
                string typeNCNonRes = "F";
                string typeValUpdateRes = "A";
                int NCNonRes = 0;

                if (optCat.Checked == true)
                    typeMQ = "C";
                if (optNCSup.Checked == true)
                    typeNCNonRes = "S";
                if (optGiornaliero.Checked == true)
                    typeValUpdateRes = "G";
                if (optMensile.Checked == true)
                    typeValUpdateRes = "M";
                if (optBimestrale.Checked == true)
                    typeValUpdateRes = "B";

                ParametriCalcoloEnte myParamCalcolo = new ParametriCalcoloEnte();
                myParamCalcolo.FromVariabile = FromVariabile;
                myParamCalcolo.Anno = txtYear.Text;
                myParamCalcolo.Ente = Ente;
                myParamCalcolo.HasConferimenti = chkPC.Checked;
                myParamCalcolo.HasMaggiorazione = chkPM.Checked;
                myParamCalcolo.TypeCalcolo = rddlTipoCalcolo.SelectedValue;
                myParamCalcolo.TypeMQ = typeMQ;
                myParamCalcolo.TypeNCNonRes = typeNCNonRes;
                int.TryParse(txtNCForfait.Text, out NCNonRes);
                myParamCalcolo.NCNonRes = NCNonRes;
                myParamCalcolo.TypeValiditaUpdateRes = typeValUpdateRes;
                if (!myParamCalcolo.Save())
                {
                    ManageBottoniera(true);
                    string sScript;
                    System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                    sScript = "<script language='javascript'>";
                    sScript += "alert('Impossibile salvare i parametri. Controlla che non siano già presenti');";
                    sScript += "</script>";
                    sBuilder.Append(sScript);
                    ClientScript.RegisterStartupScript(this.GetType(), "saveparam", sBuilder.ToString());
                }
                else
                {
                    LoadGridView(0);
                    string sScript;
                    System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                    sScript = "<script language='javascript'>";
                    sScript += "alert('Salvataggio terminato con successo!');";
                    sScript += "</script>";
                    sBuilder.Append(sScript);
                    ClientScript.RegisterStartupScript(this.GetType(), "saveparam", sBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.ParamCalcolo.CmdSaveClick.errore::", ex);;
                throw ex;
            }
        }

        protected void CmdBackClick(object sender, EventArgs e)
        {
            HideDiv("divEdit"); HideDiv("editParamCalcolo");
            ShowDiv("divSelect");ShowDiv("searchParamCalcolo");            
        }

        protected void CmdInsertClick(object sender, EventArgs e)
        {
            ShowDiv("divEdit"); ShowDiv("editParamCalcolo");
            HideDiv("divSelect");HideDiv("searchParamCalcolo"); 
        }
        #endregion

        #region "Griglia ParamCalcolo"
        protected void RgvParamCalcoloPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadGridView(e.NewPageIndex);
        }

        protected void RgvParamCalcoloRowClicked(object sender, Ribes.OPENgov.WebControls.GridViewRowClickedEventArgs args)
        {
            //ParamCalcoloEnte tariffa = new ParamCalcoloEnte { IdTariffa = int.Parse(args.Row.Cells[0].Text), Ente = Ente };
            //if (!tariffa.Load()) return;
            //hfIdCategoriaAteco.Value = tariffa.IdCategoriaAteco.ToString(CultureInfo.InvariantCulture);
            //txtCodice.Text = tariffa.CodiceCategoria;
            //txtDescrizione.Text = tariffa.Definizione;
            //chkDomestica.Checked = tariffa.Domestica;
            //ManageBottoniera(true);
        }

        protected void RgvParamCalcoloRowCommand(object sender, GridViewCommandEventArgs e)
        {
            ParametriCalcoloEnte myParamCalcolo = new ParametriCalcoloEnte { Ente = Ente, FromVariabile = FromVariabile, Anno = e.CommandArgument.ToString() };
            if (myParamCalcolo.Load())
            {
                switch (e.CommandName)
                {
                    case "DeleteParamCalcolo":
                        if (myParamCalcolo.Delete())
                            LoadGridView(0);
                        else
                        {
                            string sScript;
                            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                            sScript = "<script language='javascript'>";
                            sScript += "alert('Impossibile eliminare i parametri');";
                            sScript += "</script>";
                            sBuilder.Append(sScript);
                            ClientScript.RegisterStartupScript(this.GetType(), "errParamCalcolo", sBuilder.ToString());
                            ManageBottoniera(false);
                        }
                        break;
                    case "EditParamCalcolo":
                        txtYear.Text = myParamCalcolo.Anno;
                        chkPC.Checked=myParamCalcolo.HasConferimenti;
                        chkPM.Checked= myParamCalcolo.HasMaggiorazione;
                        rddlTipoCalcolo.SelectedValue=myParamCalcolo.TypeCalcolo;
                        txtNCForfait.Text = myParamCalcolo.NCNonRes.ToString();
                        if (myParamCalcolo.TypeMQ == "C")
                            optCat.Checked = true;
                        if (myParamCalcolo.TypeNCNonRes == "S")
                            optNCSup.Checked = true;
                        switch (myParamCalcolo.TypeValiditaUpdateRes)
                        {
                            case "G":
                                optGiornaliero.Checked=true;
                                break;
                            case "B":
                                optBimestrale.Checked = true;
                                break;
                            case "M":
                                optMensile.Checked = true;
                                break;
                        }
                        ManageBottoniera(true);
                        break;
                    default:
                        ManageBottoniera(false);
                        break;
                }
            }

        }
        #endregion

        #region "Griglia NCSup"
        protected void RgvNCSupPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            //LoadGridView(e.NewPageIndex, false);
        }

        protected void RgvNCSupRowClicked(object sender, Ribes.OPENgov.WebControls.GridViewRowClickedEventArgs args)
        {
            //NCSupEnte tariffa = new NCSupEnte { IdTariffa = int.Parse(args.Row.Cells[0].Text), Ente = Ente, FromVariabile=FromVariabile };
            //if (!tariffa.Load()) return;
            //hfIdCategoriaAteco.Value = tariffa.IdCategoriaAteco.ToString(CultureInfo.InvariantCulture);
            //txtCodice.Text = tariffa.CodiceCategoria;
            //txtDescrizione.Text = tariffa.Definizione;
            //chkDomestica.Checked = tariffa.Domestica;
            //ManageBottoniera(true);
        }

        protected void RgvNCSupRowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int idTariffa;
            //int.TryParse(e.CommandArgument.ToString(), out idTariffa);
            //NCSupEnte tariffa = new NCSupEnte { IdTariffa = idTariffa, FromVariabile=FromVariabile };
            //if (tariffa.Load())
            //{
            //    switch (e.CommandName)
            //    {
            //        case "DeleteATECO":
            //            if (tariffa.Delete())
            //                LoadGridView(0, false);
            //            else
            //            {
            //                string sScript;
            //                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            //                sScript = "<script language='javascript'>";
            //                sScript += "alert('" + string.Format("Impossibile eliminare la tariffa: {0}",
            //                                                                           tariffa.IdTariffa) + "');";
            //                sScript += "</script>";
            //                sBuilder.Append(sScript);
            //                ClientScript.RegisterStartupScript(this.GetType(), "errIdCategoriaAteco", sBuilder.ToString());
            //                ManageBottoniera(false);
            //            }
            //            break;
            //        default:
            //            ManageBottoniera(false);
            //            break;
            //    }
            //}
        }
        #endregion

        private void LoadCombo()
        {
            try
            {
                ParametriCalcoloEnte myType = new ParametriCalcoloEnte { Ente = Ente };
                rddlTipoCalcolo.DataSource = myType.LoadTypeCalcolo();
                rddlTipoCalcolo.DataValueField = "TypeCalcolo";
                rddlTipoCalcolo.DataTextField = "TypeCalcolo";
                rddlTipoCalcolo.DataBind();
                rddlTipoCalcolo.SelectedValue = RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRuolo.TipoCalcolo.TARES;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.ParamCalcolo.LoadCombo.errore::", ex);;
                throw;
            }
        }

        private void LoadGridView(int page)
        {
            try
            {
                ParametriCalcoloEnte myParam = new ParametriCalcoloEnte { Ente = Ente, FromVariabile=FromVariabile };
                rgvParamCalcolo.DataSource = myParam.LoadAll();
                rgvParamCalcolo.PageIndex = page;
                rgvParamCalcolo.DataBind();

                ManageBottoniera(false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.ParamCalcolo.LoadGridView.errore::", ex);;
                throw;
            }
        }

        private void ManageBottoniera(bool showEdit)
        {
            if (showEdit)
            {
                ShowDiv("divEdit"); ShowDiv("editParamCalcolo");
                HideDiv("divSelect"); HideDiv("searchParamCalcolo");
            }
            else
            {
                ShowDiv("divSelect"); ShowDiv("searchParamCalcolo");
                HideDiv("divEdit"); HideDiv("editParamCalcolo");
            }
            lblEditMessage.Visible = false;
        }

        protected void ChangeTypeNCSup(object sender, System.EventArgs e)
        {
            if (optNCForfait.Checked == true)
            {
                rgvNCSup.Visible = false;
                txtNCForfait.Visible = true;
            }
            else
            {
                //rgvNCSup.Visible = true;
                txtNCForfait.Visible = false;
                //NCSup myNCSup = new NCSup { Ente = Ente, Anno = txtYear.Text };
                //rgvNCSup.DataSource = myNCSup.LoadAll();
                //rgvNCSup.PageIndex = 0;
                //rgvNCSup.DataBind();

                ManageBottoniera(true);
                string sScript;
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                sScript = "<script language='javascript'>";
                sScript += "alert('Funzionalità al momento non disponibile');";
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "ChangeTypeNCSup", sBuilder.ToString());
            }
        }
    }
}