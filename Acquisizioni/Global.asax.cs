using System;
using System.IO;
using System.Web;
using Ribes.OPENgov.Utilities;
using FutureFog.U4N.General;

namespace OPENgov.Acquisizioni
{
    public class Globalx : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //Global.IniFilePath = Server.MapPath("./Config.ini");
            Global.Log = new Log(string.Concat(MiscTools.AppendFolderSeparator(Server.MapPath("~")), "Log\\"));

            Global.Log.Start();
            Global.Log.Write(LogSeverity.Debug, "Global", "Application_Start", "Site start");

            ////Load all pages lables
            //LoadPagesLables();

            Setting set = new Setting("PageTitle");
            set.Load();
            Application["PageTitle"] = string.IsNullOrEmpty(set.Value) ? "GasProject" : set.Value;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (Global.Log == null) return;
            Global.Log.Stop();
            Global.Log.Dispose();
        }
    }
}