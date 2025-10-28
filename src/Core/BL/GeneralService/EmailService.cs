using BL.Contracts.GeneralService;

namespace Bl.GeneralService
{

    public class EmailService : IEmailService
    {
        public bool SendEmail(string ToEmail, string Subject, string Body, IEnumerable<string> AttachmentFiles = null)
        {
            try
            {
                return true;
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress(Helper.sWebsiteEmail, Helper.sDisplayName);
                //    //destination adress
                //    mail.To.Add(new MailAddress(ToEmail, Helper.sDisplayName));
                //    mail.Subject = Subject;
                //    mail.Body = Body;
                //    //set to true, to specify that we are sending html text.
                //    mail.IsBodyHtml = true;
                //    // Can set to false, if you are sending pure text.
                //    mail.Priority = MailPriority.Normal;
                //    if (AttachmentFiles != null)
                //    {
                //        foreach (var item in AttachmentFiles)
                //        {
                //            mail.Attachments.Add(new Attachment(item));
                //        }
                //    }

                //    using (SmtpClient smtp = new SmtpClient(Helper.sHost, Helper.sPort))
                //    {
                //        smtp.UseDefaultCredentials = true;
                //        //passing the credentials for authentication
                //        smtp.Credentials = new NetworkCredential(Helper.sWebsiteEmail, Helper.sEmailPass);
                //        //Authentication required
                //        smtp.EnableSsl = Helper.sEnableSsl;
                //        smtp.Timeout = 32656532;
                //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //        //sending email.
                //        smtp.Send(mail);
                //        return true;
                //    }
                //}
            }
            catch (Exception d)
            {
                return false;
            }
        }

        public bool SendEmailConfirmActivationByCode(string ToEmail, string code)
        {
            try
            {
                return true;
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress(Helper.sWebsiteEmail, Helper.sDisplayName);
                //    //destination adress
                //    mail.To.Add(new MailAddress(ToEmail, Helper.sDisplayName));
                //    mail.Subject = "تفعيل الايميل على موقع ItLegend";
                //    mail.Body = "من خلال  الكود  ItLegend  يرجى الضغط على هذا الرابط" + "<div><h3>" + code + "</h3></div>";
                //    //set to true, to specify that we are sending html text.
                //    mail.IsBodyHtml = true;
                //    // Can set to false, if you are sending pure text.
                //    mail.Priority = MailPriority.Normal;
                //    using (SmtpClient smtp = new SmtpClient(Helper.sHost, Helper.sPort))//ocbookshop.com
                //    {
                //        smtp.UseDefaultCredentials = true;
                //        //passing the credentials for authentication
                //        smtp.Credentials = new NetworkCredential(Helper.sWebsiteEmail, Helper.sEmailPass);
                //        //Authentication required
                //        smtp.EnableSsl = Helper.sEnableSsl;
                //        smtp.Timeout = 32656532;
                //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //        //sending email.
                //        smtp.Send(mail);
                //        return true;
                //    }
                //}
            }
            catch (Exception d)
            {
                return false;
            }
        }
        public bool SendEmailConfirmActivation(string ToEmail, string link)
        {
            try
            {
                return true;
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress(Helper.sWebsiteEmail, Helper.sDisplayName);
                //    //destination adress
                //    mail.To.Add(new MailAddress(ToEmail, Helper.sDisplayName));
                //    mail.Subject = "تفعيل الحساب على موقع الاكاديمية";
                //    mail.Body = "لتفعيل  حسابك  على  موقع الاكاديمية  يرجى الضغط على هذا الرابط" + link;
                //    //set to true, to specify that we are sending html text.
                //    mail.IsBodyHtml = true;
                //    // Can set to false, if you are sending pure text.
                //    mail.Priority = MailPriority.Normal;
                //    using (SmtpClient smtp = new SmtpClient(Helper.sHost, Helper.sPort))//ocbookshop.com
                //    {
                //        smtp.UseDefaultCredentials = true;
                //        //passing the credentials for authentication
                //        smtp.Credentials = new NetworkCredential(Helper.sWebsiteEmail, Helper.sEmailPass);
                //        //Authentication required
                //        smtp.EnableSsl = Helper.sEnableSsl;
                //        smtp.Timeout = 32656532;
                //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //        //sending email.
                //        smtp.Send(mail);
                //        return true;
                //    }
                //}
            }
            catch (Exception d)
            {
                return false;
            }
        }
    }
}
