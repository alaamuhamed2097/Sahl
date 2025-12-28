#region usings
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
#endregion

namespace Bl
{
    public static class Helper
    {
        #region Methods
        //public static string SetPassword(string userPassword)
        //{
        //    string pwdToHash = userPassword + "bQ96r.s-W";
        //    string hashToStoreInDatabase = BCryptHelper.HashPassword(pwdToHash, BCryptHelper.GenerateSalt());
        //    return hashToStoreInDatabase;
        //}
        //public static bool DoesPasswordMatch(string hashedPwdFromDatabase, string userEnteredPassword)
        //{
        //    return BCryptHelper.CheckPassword(userEnteredPassword + "bQ96r.s-W", hashedPwdFromDatabase);
        //}

        public static string GetRandomKey(int len)
        {
            int maxSize = len;
            char[] chars = new char[30];
            string a;
            a = "1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data) { result.Append(chars[b % (chars.Length)]); }
            return result.ToString();
        }

        public static bool IsValidPhone(string phone)
        {
            var regex = @"^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$";
            return Regex.IsMatch(phone, regex);
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
        {
            private readonly TKey _key;
            private readonly IEnumerable<TElement> _elements;

            public Grouping(TKey key, IEnumerable<TElement> elements)
            {
                _key = key;
                _elements = elements;
            }

            public TKey Key => _key;

            public IEnumerator<TElement> GetEnumerator()
            {
                return _elements.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        public static double GetSecondsBetweenDates(DateTime date1, DateTime date2)
        {
            TimeSpan timeSpan = date2 - date1;
            return timeSpan.TotalSeconds;
        }

        public static string GetModelStateErrors(ModelStateDictionary modelState)
        {

            string description = string.Join("; ", modelState.Values
                                                   .SelectMany(x => x.Errors)
                                                   .Select(x => x.ErrorMessage));

            return description;

        }

        public static async Task<CountryResponse> GetCountryCode(int accountId, string licenseKey, int time, string ipAddress)
        {
            try
            {
                var client = new WebServiceClient(accountId, licenseKey, timeout: time);

                var countryData = await client.CountryAsync(ipAddress);
                //  var data = _maxMindClient.Country("102.45.19.43" );

                return countryData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GenerateRandomPassword()
        {
            return new Random().Next(10, 500) + "@Pass" + new Random().Next(1000, 50000);
        }

        public static async Task SendEmail(string fromEmail, string toEmail, string subject, string msgBody)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(toEmail));  // replace with valid value 
            message.From = new MailAddress(fromEmail);  // replace with valid value
            message.Subject = subject;
            message.Body = msgBody;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.UseDefaultCredentials = false;

                //gmail settings
                //smtp.Credentials = new System.Net.NetworkCredential(sWebsiteEmail, sEmailPass);
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.EnableSsl = true;

                //real email settings
                smtp.Credentials = new System.Net.NetworkCredential(Helper.sWebsiteEmail, Helper.sEmailPass);
                smtp.Host = "relay-hosting.secureserver.net";
                smtp.Port = 25;
                smtp.EnableSsl = false;

                await smtp.SendMailAsync(message);

            }

        }

        public static string getVideoId(string videoUrl)
        {
            if (videoUrl.Contains("embed")) return videoUrl;

            var uri = new Uri(videoUrl);


            var query = HttpUtility.ParseQueryString(uri.Query);

            string videoId = string.Empty;

            if (query.AllKeys.Contains("v"))
            {
                videoId = query["v"];
            }
            else
            {
                videoId = uri.Segments.Last();
            }
            return "https://www.youtube.com/embed/" + videoId;
        }
        public static string StripHTML(string HTMLText, bool decode = true)
        {
            try
            {
                Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
                var stripped = reg.Replace(HTMLText, "");
                return decode ? HttpUtility.HtmlDecode(stripped) : stripped;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }
        #endregion

        #region declarations

        //Email settings
        public static string SahlKey = "";
        public static string SahlDomain = "";
        public const string sWebsiteEmailSempil = "";
        public const string passwordEmailSempil = "";

        public const string URL = "";
        public const string sWebsiteEmail = "";
        public const string sEmailPass = "";
        //public const string sWebsiteEmail = ""; test 
        //public const string sEmailPass = ""; test 
        public const bool sEnableSsl = true;
        public const int sPort = 0;
        public const string sHost = "";
        public const string sDisplayName = "";
        public static string Msg;

        //public static string VDOCIPHERENDPOINT = "";
        //public static string VDOCIPHERENDPOINT = "";


        #region Emails
        //public static string forgotPasswordEmail = "wwwroot/EmailTemplates/ForgotPassword.html";
        //public static string mobileForgotPasswordEmail = "wwwroot/EmailTemplates/MobileForgotPassword.html";
        //public static string paymentWithVisaMasterEmail = "wwwroot/EmailTemplates/PaymentWithVisaMaster.html";
        //public static string paymentWithFawryEmail = "wwwroot/EmailTemplates/PaymentWithFawery.html";
        //public static string paymentWithMobileWallet = "wwwroot/EmailTemplates/PaymentWithMobileWallet.html";
        //public static string addingCourseToMemberEmail = "wwwroot/EmailTemplates/AddingCourseToMember.html";
        //public static string removingCourseToMemberEmail = "wwwroot/EmailTemplates/RemoveCourseToMember.html";
        //public static string confirmEmail = "wwwroot/EmailTemplates/ConfirmEmail.html";
        //public static string resetPasswordEmail = "wwwroot/EmailTemplates/ResetPassword.html";
        //public static string afterRegisterationEmail = "wwwroot/EmailTemplates/AfterRegisteration.html";
        //public static string leadRegisterEmail = "wwwroot/EmailTemplates/LeadRegister.html";
        //public static string successExamEmail = "wwwroot/EmailTemplates/SuccessExam.html";
        //public static string failExamEmail = "wwwroot/EmailTemplates/FailExam.html";
        //public static string messageToUsersEmail = "wwwroot/EmailTemplates/MessageToUsers.html";
        //public static string billing = "wwwroot/EmailTemplates/billing.html";
        #endregion


        #endregion
    }
}
