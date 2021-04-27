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
using System.Text;
using System.Reflection;
using System.ComponentModel;
using log4net;

namespace OPENgovTOCO
{
    /// <summary>
    /// Classe per le funzioni generali di utilità
    /// </summary>
    public class SharedFunction
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SharedFunction));
        //Get Cell Number by Column Name
        public static System.Web.UI.WebControls.TableCell CellByName(System.Web.UI.WebControls.TableRow item, string name)
        {
            try {
                Ribes.OPENgov.WebControls.RibesGridView grid = (Ribes.OPENgov.WebControls.RibesGridView)item.Parent.Parent;
                checked
                {
                    for (int col = 0; col <= item.Cells.Count - 1; col++)
                    {
                        if (grid.Columns[col].HeaderText == name)
                        {
                            return item.Cells[col];
                        }
                    }
                }
                // not found
                return null;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.CellByName.errore: ", Err);
                throw Err;
            }
        }


        /*public static object StringToInteger(string value)
		{
			
			return null;
		}*/



        //public static string DbNullToString(object obj)
        //{
        //    try {
        //        if (obj == null)
        //        {
        //            return string.Empty;
        //        }

        //        return obj.ToString();
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.StringOperation.FormatString.errore: ", Err);
        //        throw Err;
        //    }

        //}

        public static object StringToDbNull(object obj)
        {
            try {
                if (Convert.ToString(obj).CompareTo("") == 0)
                {
                    return System.DBNull.Value;
                }

                return obj.ToString();
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.StringToDbNull.errore: ", Err);
                throw Err;
            }

        }

        public static object IntegerToDbNull(int intValue)
        {
            try {
                if ((intValue >= 0))
                {
                    return intValue;
                }
                else
                {
                    return System.DBNull.Value;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.IntegerToDbNull.errore: ", Err);
                throw Err;
            }
        }

        public static object IntegerFormatToDbNull(object obj)
        {
            try {
                if (obj == null)
                {
                    return System.DBNull.Value;
                }
                else
                {
                    if (Convert.ToInt32(obj) >= 0)
                    {
                        return Convert.ToInt32(obj);
                    }
                    else
                    {
                        return System.DBNull.Value;
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.IntegerFormatToDbNull.errore: ", Err);
                throw Err;
            }
        }

        public static object DbNullToInteger(object obj)
        {
            try {

                if (obj == null)
                {
                    return System.DBNull.Value;
                }
                else
                {
                    if (Convert.ToInt32(obj) >= 0)
                    {
                        return Convert.ToInt32(obj);
                    }
                    else
                    {
                        return System.DBNull.Value;
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.DbNullToInteger.errore: ", Err);
                throw Err;
            }
        }

        public static object DateToDbNull(System.DateTime dateValue)
        {
            try {
                if (dateValue == System.DateTime.MinValue)
                {
                    return System.DBNull.Value;
                }

                return dateValue;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.DateToDbNull.errore: ", Err);
                throw Err;
            }

        }

        public static object DateFormatToDbNull(object obj)
        {
            try {
                if (obj == null)
                {
                    return System.DBNull.Value;
                }
                else
                {
                    if (Convert.ToDateTime(obj) == System.DateTime.MinValue)
                    {
                        return System.DBNull.Value;
                    }
                    else
                    {
                        return Convert.ToDateTime(obj);
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.DateFormatToDbNull.errore: ", Err);
                throw Err;
            }
        }

        public static double FormatDoubleToDb(string obj)
        {
            try {
                if (obj == null || obj.CompareTo("") == 0)
                    return 0;
                else
                    return double.Parse(obj);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.FormatDoubeToDb.errore: ", Err);
                throw Err;
            }
        }

        public static object FormattaCFPIVA(object CodFiscale, object PartitaIva)
        {
            try {
                if (PartitaIva != null && PartitaIva.ToString() != string.Empty)
                {
                    return PartitaIva;
                }
                else
                {
                    return CodFiscale;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.FormattaCFPIVA.errore: ", Err);
                throw Err;
            }
        }

        public static object FormattaVia(object Via, object Civico, object Interno, object Esponente, object Scala)
        {
            try {
                string ret = string.Empty;

                if (Via != null)
                {
                    ret = Via.ToString();
                }
                else
                {
                    ret = "";
                }
                if (Civico != null)
                {
                    if ((int)Civico > 0)
                    {
                        ret += " " + Civico.ToString();
                    }
                    else
                    {
                        ret += "";
                    }
                }
                if (Esponente != null)
                {
                    if (Esponente.ToString() != "")
                    {
                        ret += " " + Esponente.ToString();
                    }
                    else
                    {
                        ret += "";
                    }
                }
                else
                {
                    ret += "";
                }
                if (Scala != null)
                {
                    if (Scala.ToString() != "")
                    {
                        ret += " " + Scala.ToString();
                    }
                    else
                    {
                        ret += "";
                    }
                }
                else
                {
                    ret += "";
                }
                if (Interno != null)
                {
                    if (Interno.ToString() != "")
                    {
                        ret += " " + Interno.ToString();
                    }
                    else
                    {
                        ret += "";
                    }
                }
                else
                {
                    ret += "";
                }

                return ret;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.FormattaVia.errore: ", Err);
                throw Err;
            }
        }

        public static string FormattaData(object objData)
        {
            try {
                if (objData != null && ((DateTime)objData).CompareTo(DateTime.MinValue) != 0)
                {
                    DateTime data = (DateTime)objData;
                    return data.ToString("dd/MM/yyyy");
                }
                else
                    return string.Empty;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.FormattaData.errore: ", Err);
                throw Err;
            }
        }

        public static string FormattaIsSgravato(object nSgravio) {
            string sRet = string.Empty;
            bool Sgravio = false;
            try
            {
                bool.TryParse(nSgravio.ToString(), out Sgravio);
                if (Sgravio)
                    sRet = @"..\..\images\Bottoni\Add.png";
                else
                    sRet = @"..\..\images\Bottoni\trasparente.png";
            }
            catch (Exception ex) {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunctionGrd.FormattaIsSgravato.errore: ", ex);
            }
            return sRet;
        }
        public static string FormattaToolTipIsSgravato(object nSgravio, object impOrg)
        {
            string sRet = string.Empty;
            bool Sgravio = false;
            try
            {
                bool.TryParse(nSgravio.ToString(), out Sgravio);
                if (Sgravio)
                {
                    sRet = "Presenza Sgravio";
                    if (impOrg != null)
                        sRet += " Importo Originale " + string.Format("€ {0:0.00}", impOrg.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.FormattaToolTipIsSgravato.errore: ", ex);
            }
            return sRet;
        }

        /// <summary>
        /// A function that splits a string with a fixed interval
        /// of characters.
        /// </summary>
        /// <param name="number">The number of characters to be separated.</param>
        /// <param name="contents">String contents to be formatted.</param>
        /// <returns></returns>
        public static string SplitStringAt(int number, string contents)
        {
            try {
                if ((number == 0) || (number < 0) || (number > contents.Length))
                {
                    throw new ArgumentOutOfRangeException("Number");
                }
                else
                {
                    int count = 0;

                    StringBuilder _string = new StringBuilder();

                    foreach (char c in contents.ToCharArray())
                    {
                        if (count == number)
                        {
                            count = 0;
                            _string.Append("§");
                        }
                        else
                        {
                            _string.Append(c);
                            count++;
                        }
                    }

                    return _string.ToString();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.SplitStringAt.errore: ", Err);
                throw Err;
            }

        }

        public static void ChangeControlStatus(bool status, System.Web.UI.Control ctrlRef)
        {
            try {

                foreach (System.Web.UI.Control c in ctrlRef.Controls)
                {

                    if (c is TextBox)

                        ((TextBox)c).Enabled = status;

                    else if (c is Button)

                        ((Button)c).Enabled = status;

                    else if (c is RadioButton)

                        ((RadioButton)c).Enabled = status;

                    else if (c is ImageButton)

                        ((ImageButton)c).Enabled = status;

                    else if (c is CheckBox)

                        ((CheckBox)c).Enabled = status;

                    else if (c is DropDownList)

                        ((DropDownList)c).Enabled = status;

                    else if (c is HyperLink)

                        ((HyperLink)c).Enabled = status;

                    else if (c is LinkButton)

                        ((LinkButton)c).Enabled = status;

                    foreach (System.Web.UI.Control ctrl in c.Controls)
                    {
                        if (ctrl is TextBox)

                            ((TextBox)ctrl).Enabled = status;

                        else if (ctrl is Button)

                            ((Button)ctrl).Enabled = status;

                        else if (ctrl is RadioButton)

                            ((RadioButton)ctrl).Enabled = status;

                        else if (ctrl is ImageButton)

                            ((ImageButton)ctrl).Enabled = status;

                        else if (ctrl is CheckBox)

                            ((CheckBox)ctrl).Enabled = status;

                        else if (ctrl is DropDownList)

                            ((DropDownList)ctrl).Enabled = status;

                        else if (ctrl is HyperLink)

                            ((HyperLink)ctrl).Enabled = status;

                        else if (c is LinkButton)

                            ((LinkButton)c).Enabled = status;

                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.ChangeControlStatus.errore: ", Err);

            }
        }

        public static void ClearControls(System.Web.UI.Control ctrlRef)
        {
            try {
                foreach (Control c in ctrlRef.Controls)
                {
                    foreach (Control ctrl in c.Controls)
                    {
                        if (ctrl is TextBox)
                        {
                            ((TextBox)ctrl).Text = string.Empty;
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.ClearControls.errore: ", Err);

            }
        }

        public static string EnumStringValueOf(Enum value)
        {
            try {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
                else
                {
                    return value.ToString();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.EnumStringValueOf.errore: ", Err);
                throw Err;
            }
        }

        public static object EnumValueOf(string value, Type enumType)
        {
            try {
                string[] names = Enum.GetNames(enumType);
                foreach (string name in names)
                {
                    if (EnumStringValueOf((Enum)Enum.Parse(enumType, name)).Equals(value))
                    {
                        return Enum.Parse(enumType, name);
                    }
                }

                throw new ArgumentException("The string is not a description or value of the specified enum.");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.EnumValueOf.errore: ", Err);
                throw Err;
            }

        }


        public static void EnableDisableButton(ref ImageButton btn, bool EnableState)
        {
            try {
                btn.Enabled = EnableState;

                if (EnableState)
                {
                    btn.Style.Remove("filter");
                    btn.Style.Remove("opacity");
                    btn.Style.Add("cursor", "pointer");
                }
                else
                {
                    btn.Style.Add("filter", "alpha(opacity=50)");
                    btn.Style.Add("opacity", "0.50");
                    btn.Style.Add("cursor", "default");
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.EnableDisableButton.errore: ", Err);
                throw Err;
            }
        }


        /// <summary>
        /// l'equivalente inserita nel FW 2
        /// </summary>
        /// <param name="objInput"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(object objInput)
        {
            try {
                if (objInput == null)
                    return true;

                if (objInput.ToString().Trim() == string.Empty)
                    return true;
                else
                    return false;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.IsNullOrEmpty.errore: ", Err);
                throw Err;
            }
        }

        public static int IntTryParse(string Number, int DefaultValue)
        {
            int number = 0;
            try { number = int.Parse(Number); }
            catch (Exception Err)
            {
                number = DefaultValue;
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.IntTryParse.errore: ", Err);
            }
            return number;
        }

        public static decimal DecimaltryParse(string Number, decimal DefaultValue)
        {
            decimal number = 0;
            try { number = decimal.Parse(Number); }
            catch (Exception Err) {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.DecimaltryParse.errore: ", Err);
                number = DefaultValue; }
            return number;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataStrInput"></param>
        /// <returns></returns>
        public static string ReplaceDataForDB(string dataStrInput)
        {
            try {
                if (dataStrInput != null)
                    dataStrInput.Replace(".", ":");

                return dataStrInput;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SharedFunction.ReplaceDataForDB.errore: ", Err);
                throw Err;
            }
        }
    }
}



