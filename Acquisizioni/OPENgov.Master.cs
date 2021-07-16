using System;
using OPENgovDL;
using Ribes.OPENgov.Utilities;
using System.Configuration;
using System.IO;
using log4net.Config;

namespace OPENgov.Acquisizioni
{
    public partial class OPENgov : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pathfileinfo = ConfigurationManager.AppSettings["pathfileconflog4net"];
            FileInfo fileconfiglog4net = new FileInfo(pathfileinfo);
            XmlConfigurator.ConfigureAndWatch(fileconfiglog4net);

                string connection;
            if (ConfigurationManager.ConnectionStrings["OPENgov"] != null)
                connection = ConfigurationManager.ConnectionStrings["OPENgov"].ToString();
            else
                connection = "";
            Enti ente = new Enti("SQL", connection) { CodEnte = ((BasePage)Page).Ente };
            if (ente.Load())
            {
                lblEnte.Text = ente.Denominazione;
                ((BasePage)Page).TypeDimensioneComune = ente.fk_IdTypeAteco;
                ((BasePage)Page).PosizioneGeograficaComune = ente.PosizioneGeografica;
            }
            lblInfo.Text = ((BasePage)Page).BreadCrumb;
        }
    }
}