using Microsoft.VisualBasic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Drawing;

namespace OPENgovTOCO
{
    /// <summary>
    /// Classe per la gestione delle variabili costanti
    /// </summary>
	public class OSAPConst
	{
		public enum OperationType
		{
			VIEW = 1,
			EDIT = 2,
			ADD = 3,
			DELETE = 4,
			NONE = 5,
			ADDFROMEDIT = 6
		}
	}
	
}
