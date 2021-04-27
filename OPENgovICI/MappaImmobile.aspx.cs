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
using System.Text;

namespace DichiarazioniICI
{
	/// <summary>
	/// Summary description for MappaImmobile.
	/// </summary>
	public partial class MappaImmobile :BasePage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//Type cstype = this.GetType();

			string address=Request["addr"];
			StringBuilder strBuild = new StringBuilder();
			strBuild.Append("showAddress('" + address + "');");
			RegisterScript(strBuild.ToString(),this.GetType());
		}

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
