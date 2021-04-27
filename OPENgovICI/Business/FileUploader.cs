using log4net;
using System;
using System.IO;
using System.Web;

namespace Business
{
    /// <summary>
    /// Classe per la gestione degli upload dei files
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class FileUploader
	{
		private string _contentType = "*/*";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
		public delegate void FileUploaded(string FileName);
        		
        /// <summary>
        /// Evento di avvenuto upload
        /// </summary>
		public event FileUploaded OnUpload = null;
        private static readonly ILog log = LogManager.GetLogger(typeof(FileUploader));
        /// <summary>
        /// 
        /// </summary>
        public FileUploader()
		{
			
		}

		/// <summary>
		/// Effettua l'upload
		/// </summary>
		/// <param name="postedFile"></param>
		/// <param name="DestinationFile"></param>
		/// <returns></returns>
		public bool Upload(HttpPostedFile postedFile, string DestinationFile)
		{
			bool retVal = true;
			// Mi prealloco il buffer in base alle dimensioni del file inviato
			int DimensioneBuffer = postedFile.ContentLength;
            try { 
			if(_contentType=="*/*" || (_contentType != "*/*" && postedFile.ContentType == _contentType))
			{
				_contentType = postedFile.ContentType;
				
				if(DimensioneBuffer > 0)
				{
					byte[] Buffer = new byte[DimensioneBuffer];
					int BytesLetti = postedFile.InputStream.Read(Buffer, 0, DimensioneBuffer);

					FileStream fs = new FileStream(DestinationFile , FileMode.Create);
					fs.Write(Buffer, 0, BytesLetti);
					fs.Close();
					if(OnUpload != null)
						OnUpload(DestinationFile);
				}
				else
					retVal = false;
				
			}
			else
			{
				// Non è soddisfatta la tipologia di file richiesta
				retVal = false;
				//throw new FileUploaderExcepion("Tipologia del file errata");
			}

			return retVal;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.FileUploader.Upload.errore: ", Err);
                throw Err;
            }
        }
	}
}
