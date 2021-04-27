using System;
using Business;
using ImexInterface;
using System.Web;
using System.IO;

namespace Business
{
    /// <summary>
    /// Classe per le importazioni imex
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ImportHelper
	{
		private static IImporter GetObject()
		{
			return  (IImporter)Activator.GetObject(typeof(IImporter), ConstWrapper.ImexServiceAddress);		
		}

		/// <summary>
		/// Metodo per avviare l'importazione
		/// </summary>
		/// <param name="tipo"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static bool Import(string tipo, string fileName)
		{
			return GetObject().Import (tipo, fileName,"V",ConstWrapper.CodiceEnte, ConstWrapper.CodiceTributoAnag.ToString(),
					ConstWrapper.ComuneUbicazioneImmobile, ConstWrapper.ContoCorrente, ConstWrapper.IntestatarioBollettino,
					ConstWrapper.ParametroAnagrafica, ConstWrapper.ParametroAPPLICATION, ConstWrapper.ParametroENV, ConstWrapper.ParametroUTILITA, ConstWrapper.UserNameFramework, HttpContext.Current.Session["COD_ENTE"].ToString ());
		}


		/*public static bool Import(string tipo, string[] fileSplittato)
		{
			return GetObject().Import (tipo, fileSplittato,"V",ConstWrapper.CodiceEnte, ConstWrapper.CodiceTributo.ToString(),
				ApplicationHelper.ComuneUbicazioneImmobile, ApplicationHelper.ContoCorrente, ApplicationHelper.IntestatarioBollettino,
				ConstWrapper.ParametroAnagrafica, ConstWrapper.ParametroAPPLICATION, ConstWrapper.ParametroENV, ConstWrapper.ParametroUTILITA, ConstWrapper.UserNameFramework, HttpContext.Current.Session["COD_ENTE"].ToString ());
		}*/

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tipo"></param>
		/// <param name="fileSplittato"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static bool Import(string tipo, string[] fileSplittato, string fileName)
		{
			return GetObject().Import (tipo, fileSplittato, fileName ,"V",ConstWrapper.CodiceEnte, ConstWrapper.CodiceTributoAnag.ToString(),
				ConstWrapper.ComuneUbicazioneImmobile, ConstWrapper.ContoCorrente, ConstWrapper.IntestatarioBollettino,
				ConstWrapper.ParametroAnagrafica, ConstWrapper.ParametroAPPLICATION, ConstWrapper.ParametroENV, ConstWrapper.ParametroUTILITA, ConstWrapper.UserNameFramework, HttpContext.Current.Session["COD_ENTE"].ToString ());
		}

		/// <summary>
		/// Torna l'esito dell'ultima importazione con la data dell'esecuzione.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="dataUltimoImport"></param>
		/// <returns></returns>
		public static bool ResultLastImport(string fileName, out DateTime dataUltimoImport)
		{
			return GetObject().ResultLastImport(fileName ,ConstWrapper.ParametroAPPLICATION, ConstWrapper.ParametroENV, ConstWrapper.ParametroUTILITA, ConstWrapper.UserNameFramework, out dataUltimoImport);
		}


		
	}
}
