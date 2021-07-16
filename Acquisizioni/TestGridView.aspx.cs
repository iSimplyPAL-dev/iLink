using System;
using OPENgovDL;
using Ribes.OPENgov.Utilities;
using Ribes.OPENgov.WebControls;

namespace OPENgov.Acquisizioni
{
    public partial class TestGridView : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }
        
        private void LoadData(int? pageIndex = null)
        {
            RibesGridView1.DataSource = new AnagrafeUiMovimenti().LoadAll();
            if (pageIndex.HasValue) RibesGridView1.PageIndex = pageIndex.Value;
            RibesGridView1.DataBind();
            
        }

        protected void RibesGridView1PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadData(e.NewPageIndex);
        }

        protected void RibesGridView1RowClicked(object sender, GridViewRowClickedEventArgs args)
        {
            Response.Redirect("~/AnagrafeResidenti.aspx");
        }
    }
}