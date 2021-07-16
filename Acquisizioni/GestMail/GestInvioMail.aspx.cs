using System;
using System.Web.UI.WebControls;
using System.Configuration;
using FutureFog.U4N.General;
using OPENgovDL;
using Ribes.OPENgov.Utilities;
using System.Collections.Generic;
using System.Web.Mail;
using log4net;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Data.SqlClient;
using System.Data;

namespace OPENgov.Acquisizioni.GestMail
{/// <summary>
/// Pagina per la gestione dell'invio delle mail.
/// Le possibili opzioni sono:
/// - Salva
/// - Ricerca
/// </summary>
    public partial class GestInvioMail : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GestInvioMail));
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BreadCrumb = "Invio Mail - Gestione Lotti di invio";
            if (ConfigurationManager.AppSettings["MailServer"] != null)
                txtServer.Text = ConfigurationManager.AppSettings["MailServer"].ToString();
            if (ConfigurationManager.AppSettings["MailServerPort"] != null)
                txtServerPort.Text = ConfigurationManager.AppSettings["MailServerPort"].ToString();
            if (ConfigurationManager.AppSettings["MailSSL"] != null)
                txtSSL.Text = ConfigurationManager.AppSettings["MailSSL"].ToString();
            if (ConfigurationManager.AppSettings["MailSender"] != null)
                txtSender.Text = ConfigurationManager.AppSettings["MailSender"].ToString();
            if (ConfigurationManager.AppSettings["MailSenderName"] != null)
                txtSenderName.Text = ConfigurationManager.AppSettings["MailSenderName"].ToString();
            if (ConfigurationManager.AppSettings["MailPassword"] != null)
                txtSenderPwd.Text = ConfigurationManager.AppSettings["MailPassword"].ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack) return;
                HideDiv("InvioInCorso"); ShowDiv("ResultRicerca");
                LoadCombos();
                LoadSearch();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.GestMail.Page_Load.errore::", ex);
                throw;
            }
        }
        #region "Bottoni"
        protected void CmdSearchClick(object sender, EventArgs e)
        {
            LoadSearch();
            HideDiv("InvioInCorso"); ShowDiv("ResultRicerca");
        }
        protected void CmdSaveClick(object sender, EventArgs e)
        {
            try
            {
                string sMessage = string.Empty;

                if (!ControlliPreSalvataggio(ref sMessage))
                {
                    HideDiv("InvioInCorso"); ShowDiv("ResultRicerca");
                    string sScript = string.Empty;
                    System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                    sBuilder = new System.Text.StringBuilder();
                    sScript = "<script language='javascript'>";
                    sScript += "alert('" + sMessage + "!');";
                    sScript += "</script>";
                    sBuilder.Append(sScript);
                    ClientScript.RegisterStartupScript(this.GetType(), "checkparam", sBuilder.ToString());
                    return;
                }
                if (SaveDatiMail())
                {
                    txtServer.Text = txtServerPort.Text =  txtSSL.Text =txtSender.Text =   txtSenderPwd.Text = txtSubject.Text = txtBody.Text = string.Empty;
                    LoadSearch();
                    LoadCombos();
                    HideDiv("ResultRicerca"); ShowDiv("InvioInCorso");
                    string sScript = "<script language='javascript'>";
                    sScript += "alert('Salvataggio terminato con successo!');";
                    sScript += "</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "saveparam", sScript);
                    if (ConfigurationManager.AppSettings["MailSendSameTime"] != null)
                    {
                        if (ConfigurationManager.AppSettings["MailSendSameTime"].ToString().ToLower() == "false")
                        {
                            SendMail();
                        }
                    }
                }
                else
                {
                    string sScript = "<script language='javascript'>";
                    sScript += "alert('Impossibile salvare i parametri per il lotto!');";
                    sScript += "</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "saveparam", sScript);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.GestMail.CmdSaveClick.errore::", ex);
            }
        }
        #endregion

        private void LoadCombos()
        {
            try
            {
                InvioMail myGestMail = new InvioMail { Ente = Ente };
                rddlTributo.DataSource = myGestMail.LoadTributi(false);
                rddlTributo.DataValueField = "tributo";
                rddlTributo.DataTextField = "DescrTributo";
                rddlTributo.DataBind();

                rddlAnno.DataSource = myGestMail.LoadAnni(false);
                rddlAnno.DataValueField = "Anno";
                rddlAnno.DataTextField = "Anno";
                rddlAnno.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.GestMail.LoadCombos.errore::", ex);
                throw;
            }
        }
        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearch(int? page = 0)
        {
            string tributo = string.Empty;
            string anno = string.Empty;
            try
            {
                tributo = rddlTributo.SelectedValue;
                anno = rddlAnno.SelectedValue;
                InvioMail LottiToSend = new InvioMail { Ente = Ente, Tributo = tributo, Anno = anno };
                if (LottiToSend != null)
                {
                    rgvLotti.DataSource = LottiToSend.LoadLottiDaIviare();
                    if (page.HasValue)
                        rgvLotti.PageIndex = page.Value;
                    rgvLotti.DataBind();
                    if (rgvLotti.Rows.Count > 0)
                        lblResultLotti.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.GestMail.LoadSearch.errore::", ex);
                throw;
            }
        }
        private bool ControlliPreSalvataggio(ref string sErrMessage)
        {
            bool bLottoSel = false;

            try
            {
                if (txtServer.Text.Trim() ==string.Empty)
                {
                    sErrMessage = "Inserire il server di invio!";
                    return false;
                }
                if (txtServerPort.Text.Trim() == string.Empty) 
                {
                    sErrMessage = "Inserire la porta del server di invio!";
                    return false;
                }
                if (txtSSL.Text.Trim() == string.Empty) 
                {
                    sErrMessage = "Inserire SSL!";
                    return false;
                }
                if (txtSender.Text.Trim() == string.Empty) 
                {
                    sErrMessage = "Inserire la mail del mittente!";
                    return false;
                }
                if (txtSenderPwd.Text.Trim() == string.Empty) 
                {
                    sErrMessage = "Inserire la password della mail di invio!";
                    return false;
                }
                //controllo che sia presente l'oggetto della mail
                if (txtSubject.Text.Trim() == string.Empty)
                {
                    sErrMessage = "Inserire oggetto della mail!";
                    return false;
                }
                //controllo che sia presente il corpo della mail
                if (txtBody.Text.Trim() == string.Empty)
                {
                    sErrMessage = "Inserire il testo della mail!";
                    return false;
                }
                //controllo che sia selezionato almento un lotto
                foreach (GridViewRow myRow in rgvLotti.Rows)
                {
                    if (((CheckBox)myRow.Cells[4].FindControl("chkSel")).Checked)
                    {
                        bLottoSel = true;
                    }
                }
                if (!bLottoSel)
                {
                    sErrMessage = "Selezionare almeno un lotto di invio!";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.GestMail.ControlliPreSalvataggio.errore::", ex);
                throw ex;
            }
        }
        private bool SaveDatiMail()
        {
            string sScript = string.Empty;
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();

            try
            {
                //salvo i dati inseriti per i lotti selezionati
                foreach (GridViewRow myRow in rgvLotti.Rows)
                {
                    if (((CheckBox)myRow.FindControl("chkSel")).Checked)
                    {
                        InvioMail myInvioMail = new InvioMail();
                        myInvioMail.IdLotto = int.Parse(myRow.Cells[2].Text);
                        myInvioMail.EMailFrom = txtSender.Text;
                        myInvioMail.EMailSubject = txtSubject.Text;
                        myInvioMail.EMailBody = txtBody.Text;
                        myInvioMail.Server = txtServer.Text;
                        myInvioMail.ServerPort = txtServerPort.Text;
                        myInvioMail.SSL = int.Parse(txtSSL.Text);
                        myInvioMail.Sender = txtSender.Text;
                        myInvioMail.Password = txtSenderPwd.Text;
                        myInvioMail.SenderName = txtSenderName.Text;
                        myInvioMail.WarningRecipient = ((HiddenField)myRow.FindControl("hfWarningRecipient")).Value;
                        myInvioMail.WarningSubject = ((HiddenField)myRow.FindControl("hfWarningSubject")).Value;
                        myInvioMail.WarningMessage = ((HiddenField)myRow.FindControl("hfWarningMessage")).Value;
                        myInvioMail.MailArchive = ((HiddenField)myRow.FindControl("hfMailArchive")).Value;

                        if (!myInvioMail.Save())
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.GestMail.SaveDatiMail.errore::", ex);
                throw ex;
            }
        }
        private void SendMail()
        {
            try
            {
                try
                {
                    string sErr = string.Empty;
                    MailToSend[] ListInvio = new MailToSend() { Ente = Ente }.LoadMailToSend();
                    if (ListInvio != null)
                    {
                        foreach (MailToSend myInvio in ListInvio)
                            new EmailService().StartSendMail(myInvio);
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("OPENgov.20.GestMail.SendMail.errore::", ex);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.GestMail.SendMail.errore::", ex);
            }
        }
    }
    public class EmailService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EmailService));
        private MailToSend myListMail;

        public void StartSendMail(MailToSend ListMailToSend)
        {
            try
            {
                ThreadStart threadDelegate = new ThreadStart(this.SendEmailMassive);
                Thread t = new Thread(threadDelegate);

                myListMail = ListMailToSend;

                t.Start();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.EmailService.StartSendMail.errore::", ex);
            }
        }
        public bool SendEmail(MailToSend myMail, List<string> ListTO, List<string> ListCC, List<string> ListBCC, List<MailAttachment> ListAttachment, out string sErr)
        {
            sErr = string.Empty;
            try
            {
                MailMessage mail = new MailMessage();
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", myMail.Server);
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", myMail.ServerPort);
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2"); //Send the message using the network (SMTP over the network)
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //YES
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", myMail.Sender);
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", myMail.Password);
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", myMail.SSL);

                mail.From = myMail.Sender;
                string mailTo = string.Empty;
                foreach (string myRecipient in ListTO)
                {
                    mailTo = (mailTo != string.Empty ? ";" : "") + myRecipient;
                }
                mail.To = mailTo;
                mailTo = string.Empty;
                foreach (string myRecipient in ListCC)
                {
                    mailTo = (mailTo != string.Empty ? ";" : "") + myRecipient;
                }
                mail.Cc = mailTo;
                string mailToBCC = string.Empty;
                if (myMail.SSL == 0)
                {
                    foreach (string myRecipient in ListBCC)
                    {
                        mailToBCC = (mailTo != string.Empty ? ";" : "") + myRecipient;
                    }
                }
                mail.Bcc = mailToBCC;
                mail.Subject = myMail.EMailSubject;
                mail.Body = myMail.EMailBody;
                foreach (MailAttachment myAttach in ListAttachment)
                {
                    mail.Attachments.Add(myAttach);
                }
                try
                {
                    SmtpMail.Send(mail);
                }
                catch (Exception mailEx)
                {
                    Log.Debug("OPENgov.20.EmailService.SendEmail.Send.errore::", mailEx);
                    sErr += "SendEmail.errore invio ";
                    try
                    {
                        mail = new MailMessage();
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", myMail.Server);
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", myMail.ServerPort);
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2"); //Send the message using the network (SMTP over the network)
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //YES
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", myMail.Sender);
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", myMail.Password);
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", myMail.SSL);
                        mail.From = myMail.SenderName;
                        mail.To = myMail.WarningRecipient;
                        mail.Subject = myMail.WarningSubject;
                        mail.Body = (myMail.WarningMessage + " Errore rilevato:" + mailEx.Message + "     Mail inviata a:" + mailTo + "     Mail:" + myMail.EMailSubject + "     " + myMail.EMailBody);

                        SmtpMail.Send(mail);
                    }
                    catch (Exception err)
                    {
                        Log.Debug("OPENgov.20.EmailService.SendEmailWarning.errore::", err);
                        sErr+= "SendEmailWarning.errore";
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.EmailService.SendEmail.errore::", ex);
                sErr += "SendEmail.errore ";
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", myMail.Sender);
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", myMail.ServerPort);
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2"); //Send the message using the network (SMTP over the network)
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //YES
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", myMail.SenderName);
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", myMail.Password);
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", myMail.SSL);
                    mail.From = myMail.SenderName;
                    mail.To = myMail.WarningRecipient;
                    mail.Subject = myMail.WarningSubject;
                    mail.Body = (myMail.WarningMessage +"     "+ ListTO.ToString());

                    SmtpMail.Send(mail);
                }
                catch (Exception err)
                {
                    Log.Debug("OPENgov.20.EmailService.SendEmailWarning.errore::", err);
                    sErr+= "SendEmailWarning.errore";
                }
                return false;
            }
        }

        private void SendEmailMassive()
        {
            int nSend = 0;
            string sError = string.Empty;
            try
            {
                CacheManagerMail.SetSendEmailInCorso(myListMail.IdLotto);
                foreach (string Recipient in myListMail.ListRecipient)
                {
                    CacheManagerMail.SetSendEmailAvanzamento("Posizione " + nSend.ToString() + " di " + myListMail.ListRecipient.Count.ToString());
                    if (SendEmail(myListMail, new List<string>() { Recipient }, new List<string>(), myListMail.ListRecipientBCC, myListMail.ListAttachments, out sError))
                        new MailToSend() { Ente = myListMail.Ente,IdLotto=myListMail.IdLotto }.SetResultInvio(Recipient,"Y",string.Empty);
                    else
                        new MailToSend() { Ente = myListMail.Ente, IdLotto = myListMail.IdLotto }.SetResultInvio(Recipient, "N",sError);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.EmailService.SendEmailMassive.errore::", ex);
            }
            finally
            {
                CacheManagerMail.RemoveSendEmailInCorso();
            }
        }
    }
    public class MailToSend : InvioMail
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MailToSend));
        #region Variables and constructor
        public MailToSend()
        {
            Reset(); Clear();
        }
        #endregion

        #region Public properties
        public List<string> ListRecipient { get; set; }
        public List<string> ListRecipientBCC { get; set; }
        public List<System.Web.Mail.MailAttachment> ListAttachments { get; set; }
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
        public void Clear()
        {
            ListRecipient = new List<string>();
            ListRecipientBCC = new List<string>();
            ListAttachments = new List<System.Web.Mail.MailAttachment>();
        }
        /*public override bool Load()
        {
            throw new NotImplementedException();
        }
        public override InvioMail[] LoadAll()
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
        }*/
        #endregion
        public MailToSend[] LoadMailToSend()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetMailDaInviare";
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlRead = sqlCmd.ExecuteReader();
                List<MailToSend> list = new List<MailToSend>();
                while (sqlRead.Read())
                {
                    MailToSend item = new MailToSend();
                    item.IdLotto = DbValue<int>.Get(sqlRead["Lotto"]);
                    item.Ente = DbValue<string>.Get(sqlRead["IdEnte"]);
                    item.Anno = DbValue<int>.Get(sqlRead["Anno"]).ToString();
                    item.Tributo = DbValue<string>.Get(sqlRead["CodTributo"]);
                    item.DescrTributo = DbValue<string>.Get(sqlRead["DescrTributo"]);
                    item.NDestinatari = DbValue<int>.Get(sqlRead["ndest"]);
                    list.Add(item);
                }

                return list.ToArray();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.MailToSend.LoadMailToSend.errore::", ex);
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }
        public bool SetResultInvio(string Recipient,string Result,string DescrResult)
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_SetResultInvio";
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@IdLotto", DbParam.Get(IdLotto));
                sqlCmd.Parameters.AddWithValue("@Recipient", DbParam.Get(Recipient));
                sqlCmd.Parameters.AddWithValue("@Result", DbParam.Get(Result));
                sqlCmd.Parameters.AddWithValue("@DescrResult", DbParam.Get(DescrResult));
                if (sqlCmd.ExecuteNonQuery() <= 0)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.MailToSend.SetResultInvio.errore::", ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }
    }
    /// <summary>
    /// Classe per la gestione cache dell'elaborazione asincrona
    /// </summary>
    public class CacheManagerMail
    {
        private static System.Web.Caching.Cache IISCache = HttpRuntime.Cache;
        private static readonly ILog Log = LogManager.GetLogger(typeof(CacheManager));

        private CacheManagerMail()
        {
        }

        #region SendEmail
        private static string SendEmailInCorsoKey = "SendEmailInCorso";
        private static string SendEmailAvanzamentoKey = "SendEmailAvanzamento";
        public static int GetSendEmailInCorso()
        {
            try
            {
                if (IISCache[SendEmailInCorsoKey] != null)
                    return (int)IISCache[SendEmailInCorsoKey];
                else
                    return (-1);
            }
            catch (Exception Err)
            {
                Log.Debug("OPENgov.20.CacheManager.GetSendEmailInCorso.errore: ", Err);
                throw Err;
            }
        }
        public static void SetSendEmailInCorso(int IdSend)
        {
            try
            {
                IISCache.Insert(SendEmailInCorsoKey, IdSend, null,
                    Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
                    CacheItemPriority.NotRemovable, null);
            }
            catch (Exception Err)
            {
                Log.Debug("OPENgov.20.CacheManager.SetSendEmailInCorso.errore: ", Err);

            }
        }
        public static void SetSendEmailAvanzamento(string sMyDati)
        {
            try
            {
                IISCache.Insert(SendEmailAvanzamentoKey, sMyDati, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
            catch (Exception Err)
            {
                Log.Debug("OPENgovOSAP.CacheManager.SetAvanzamentoElaborazione.errore: ", Err);

            }
        }
        public static void RemoveSendEmailInCorso()
        {
            IISCache.Remove(SendEmailInCorsoKey);
            IISCache.Remove(SendEmailAvanzamentoKey);
        }
        #endregion SendEmail
    }
}
