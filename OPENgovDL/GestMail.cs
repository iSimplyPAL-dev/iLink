using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;
using log4net;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione delle mail da inviare
    /// </summary>
    public class InvioMail : DbObject<InvioMail>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InvioMail));
        #region Variables and constructor
        public InvioMail()
        {
            Reset();
        }
        public InvioMail(int IDLotto)
        {
            Reset();
            IdLotto = IDLotto;
        }
        #endregion

        #region Public properties
        public int IdLotto { get; set; }
        public string Ente { get; set; }
        public string Anno { get; set; }
        public string Tributo { get; set; }
        public string DescrTributo { get; set; }
        public string EMailFrom { get; set; }
        public string EMailSubject { get; set; }
        public string EMailBody { get; set; }
        public string EMailAdministrative { get; set; }
        public int NDestinatari { get; set; }
        public string DescrStato { get; set; }
        public string Server { get; set; }
        public string ServerPort { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
        public int SSL { get; set; }
        public string SenderName { get; set; }
        public string WarningRecipient { get; set; }
        public string MailArchive { get; set; }
        public string WarningSubject { get; set; }
        public string WarningMessage { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is InvioMail) &&
                ((obj as InvioMail).IdLotto == IdLotto);
        }
        public override int GetHashCode()
        {
            return GenerateHashCode(IdLotto);
        }
        public override sealed void Reset()
        {
            IdLotto = default(int);
            Ente = string.Empty;
            Anno = string.Empty;
            Tributo= string.Empty;
            DescrTributo = string.Empty;
            EMailFrom = string.Empty;
            EMailSubject = string.Empty;
            EMailBody = string.Empty;
            EMailAdministrative = string.Empty;
            NDestinatari = default(int);
            DescrStato = string.Empty;
            Server = string.Empty;
            ServerPort = string.Empty;
            Sender = string.Empty;
            Password = string.Empty;
            SenderName = string.Empty;
            WarningRecipient = string.Empty;
            MailArchive = string.Empty;
            WarningSubject = string.Empty;
            WarningMessage = string.Empty;
            SSL = 0;
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override InvioMail[] LoadAll()
        {
            throw new NotImplementedException();
        }

        public override bool Save()
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_SetLottiDaInviare";
                sqlCmd.Parameters.AddWithValue("@Lotto", DbParam.Get(IdLotto));
                sqlCmd.Parameters.AddWithValue("@EMailFrom", DbParam.Get(EMailFrom));
                sqlCmd.Parameters.AddWithValue("@EMailSubject", DbParam.Get(EMailSubject));
                sqlCmd.Parameters.AddWithValue("@EMailBody", DbParam.Get(EMailBody));
                sqlCmd.Parameters.AddWithValue("@EMailAdministrative", DbParam.Get(EMailAdministrative));
                sqlCmd.Parameters.AddWithValue("@Server", DbParam.Get(Server));
                sqlCmd.Parameters.AddWithValue("@ServerPort", DbParam.Get(ServerPort));
                sqlCmd.Parameters.AddWithValue("@SSL", DbParam.Get(SSL));
                sqlCmd.Parameters.AddWithValue("@Sender", DbParam.Get(Sender));
                sqlCmd.Parameters.AddWithValue("@SenderName", DbParam.Get(SenderName));
                sqlCmd.Parameters.AddWithValue("@Password", DbParam.Get(Password));
                sqlCmd.Parameters.AddWithValue("@WarningRecipient", DbParam.Get(WarningRecipient));
                sqlCmd.Parameters.AddWithValue("@WarningSubject", DbParam.Get(WarningSubject));
                sqlCmd.Parameters.AddWithValue("@WarningMessage", DbParam.Get(WarningMessage));
                sqlCmd.Parameters.AddWithValue("@MailArchive", DbParam.Get(MailArchive));
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.InvioMail.Save.errore::", ex);;
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion

        public InvioMail[] LoadTributi(bool bIsInviati)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetTributiLottiDaInviare";
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Tributo", DbParam.Get(Tributo));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlCmd.Parameters.AddWithValue("@IsInviati", DbParam.Get(bIsInviati));
                sqlRead = sqlCmd.ExecuteReader();
                List<InvioMail> list = new List<InvioMail>();
                while (sqlRead.Read())
                {
                    InvioMail item = new InvioMail();
                    item.Tributo = DbValue<string>.Get(sqlRead["CodTributo"]);
                    item.DescrTributo = DbValue<string>.Get(sqlRead["DescrTributo"]);
                    list.Add(item);
                }
                return list.ToArray();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.InvioMail.LoadTributi.errore::", ex);;
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public InvioMail[] LoadAnni(bool bIsInviati)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetAnniLottiDaInviare";
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Tributo", DbParam.Get(Tributo));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlCmd.Parameters.AddWithValue("@IsInviati", DbParam.Get(bIsInviati)); 
                sqlRead = sqlCmd.ExecuteReader();
                List<InvioMail> list = new List<InvioMail>();
                while (sqlRead.Read())
                {
                    InvioMail item = new InvioMail();
                    item.Anno = DbValue<int>.Get(sqlRead["anno"]).ToString();
                    list.Add(item);
                }
                return list.ToArray();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.InvioMail.LoadAnni.errore::", ex);;
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public InvioMail[] LoadLottiDaIviare()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetLottiDaInviare";
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Tributo", DbParam.Get(Tributo));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlRead = sqlCmd.ExecuteReader();
                List<InvioMail> list = new List<InvioMail>();
                while (sqlRead.Read())
                {
                    InvioMail item = new InvioMail();
                    item.IdLotto = DbValue<int>.Get(sqlRead["Lotto"]);
                    item.Ente = DbValue<string>.Get(sqlRead["IdEnte"]);
                    item.Anno = DbValue<int>.Get(sqlRead["Anno"]).ToString();
                    item.Tributo = DbValue<string>.Get(sqlRead["CodTributo"]);
                    item.DescrTributo = DbValue<string>.Get(sqlRead["DescrTributo"]);
                    item.NDestinatari = DbValue<int>.Get(sqlRead["ndest"]);
                    item.Server =DbValue<string>.Get(sqlRead["server"]);
                    item.ServerPort = DbValue<string>.Get(sqlRead["serverport"]);
                    item.SSL = DbValue<int>.Get(sqlRead["ssl"]);
                    item.Sender = DbValue<string>.Get(sqlRead["sender"]);
                    item.Password = DbValue<string>.Get(sqlRead["password"]);
                    item.SenderName = DbValue<string>.Get(sqlRead["sendername"]);
                    item.WarningRecipient = DbValue<string>.Get(sqlRead["warningrecipient"]);
                    item.WarningSubject = DbValue<string>.Get(sqlRead["warningsubject"]);
                    item.WarningMessage = DbValue<string>.Get(sqlRead["warningmessage"]);
                    item.MailArchive = DbValue<string>.Get(sqlRead["mailarchive"]);
                    list.Add(item);
                }

                return list.ToArray();

            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.InvioMail.LoadLottiDaInviare.errore::", ex);;
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public InvioMail[] LoadEsitiInvio()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetLottiEsitoInvio";
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Tributo", DbParam.Get(Tributo));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlRead = sqlCmd.ExecuteReader();
                List<InvioMail> list = new List<InvioMail>();
                while (sqlRead.Read())
                {
                    InvioMail item = new InvioMail();
                    item.IdLotto = DbValue<int>.Get(sqlRead["Lotto"]);
                    item.Ente = DbValue<string>.Get(sqlRead["IdEnte"]);
                    item.Anno = DbValue<int>.Get(sqlRead["Anno"]).ToString();
                    item.Tributo = DbValue<string>.Get(sqlRead["CodTributo"]);
                    item.DescrTributo = DbValue<string>.Get(sqlRead["DescrTributo"]);
                    item.EMailSubject = DbValue<string>.Get(sqlRead["OGGETTO_MAIL"]);
                    item.NDestinatari = DbValue<int>.Get(sqlRead["ndest"]);
                    item.DescrStato = DbValue<string>.Get(sqlRead["DESCRSTATO"]);
                    list.Add(item);
                }

                return list.ToArray();

            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.InvioMail.LoadEsitiInvio.errore::", ex);;
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }
    }
    /// <summary>
    /// Classe per la gestione dei destinatari delle mail da inviare
    /// </summary>
    public class InvioMailDestinatari : DbObject<InvioMailDestinatari>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InvioMailDestinatari));
        #region Variables and constructor
        public InvioMailDestinatari()
        {
            Reset();
        }
        public InvioMailDestinatari(int ID)
        {
            Reset();
            Id = ID;
        }
        #endregion

        #region Public properties
        public int Id { get; set; }
        public int IdLotto { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string CFPIVA { get; set; }
        public string EMailDest { get; set; }
        public string Stato { get; set; }
        public string DescrStato { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is InvioMailDestinatari) &&
                ((obj as InvioMailDestinatari).IdLotto == IdLotto);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdLotto);
        }

        public override sealed void Reset()
        {
            IdLotto = default(int);
            Cognome = string.Empty;
            Nome = string.Empty;
            CFPIVA = string.Empty;
            EMailDest = string.Empty;
            Stato = string.Empty;
            DescrStato = string.Empty;
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override InvioMailDestinatari[] LoadAll()
        {
            throw new NotImplementedException();
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion

        public InvioMailDestinatari[] LoadEsitiInvio()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetDestEsitoInvio";
                sqlCmd.Parameters.AddWithValue("@Lotto", DbParam.Get(IdLotto));
                sqlRead = sqlCmd.ExecuteReader();
                List<InvioMailDestinatari> list = new List<InvioMailDestinatari>();
                while (sqlRead.Read())
                {
                    InvioMailDestinatari item = new InvioMailDestinatari();
                    item.Cognome = DbValue<string>.Get(sqlRead["Cognome"]);
                    item.Nome = DbValue<string>.Get(sqlRead["Nome"]).ToString();
                    item.CFPIVA = DbValue<string>.Get(sqlRead["CFPIVA"]);
                    item.EMailDest = DbValue<string>.Get(sqlRead["EMail_Dest"]);
                    item.Stato = DbValue<string>.Get(sqlRead["Stato"]);
                    item.DescrStato = DbValue<string>.Get(sqlRead["DescrStato"]);
                    list.Add(item);
                }

                return list.ToArray();

            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.InvioMailDestinatari.LoadEsitiInvio.errore::", ex);;
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }
        public bool ResendMail()
        {
            bool myRet = false;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_SetReInvio";
                sqlCmd.Parameters.AddWithValue("@Lotto", DbParam.Get(IdLotto));
                sqlRead = sqlCmd.ExecuteReader();
                while (sqlRead.Read())
                {
                    if (DbValue<int>.Get(sqlRead["id"]) == 1)
                        myRet= true;
                    else
                        myRet= false;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.InvioMailDestinatari.ResendMail.errore::", ex); ;
                myRet= false;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
            return myRet;
        }
    }
}
