using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WCFServiceTemplate;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web;
using System.Collections.Generic;

namespace WCFPGMSFront
{
    public class Service1 : IService1
    {

        #region Basic        
        int intCommandTimeOut = 240;

        public DateTime? ToDateTime(string strDate)
        {
            DateTime? cDate = null;
            if (strDate == null || strDate.Trim() == "" || strDate.Trim().ToLower().Contains("&nbsp;"))
            {
                cDate = null;
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                DateTime dtProjectStartDate = Convert.ToDateTime(strDate);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                cDate = Convert.ToDateTime((Convert.ToDateTime(dtProjectStartDate).ToString("MM/dd/yyyy")));
            }
            return cDate;
        }
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public bool CheckWebserver()
        {
            return true;
        }

        private static void ConvertTableToList<T>(DataRow dr, ref T objMaster)
        {
            try
            {
                foreach (PropertyInfo property in objMaster.GetType().GetProperties())
                {
                    if (dr.Table.Columns.Contains(property.Name.ToString()))
                    {

                        if (dr[property.Name] == DBNull.Value)
                        {
                            property.SetValue(objMaster, null, null);
                        }
                        else if (property.PropertyType.ToString() == "System.Nullable`1[System.Char]")
                        {
                            Type Primitive = Nullable.GetUnderlyingType(property.PropertyType);
                            object Temp = Convert.ChangeType(dr[property.Name], Type.GetType(Primitive.FullName), CultureInfo.InvariantCulture);
                            property.SetValue(objMaster, Temp, null);

                        }
                        else if (property.PropertyType.ToString() == "System.Nullable`1[System.Decimal]")
                        {
                            Type Primitive = Nullable.GetUnderlyingType(property.PropertyType);
                            object Temp = Convert.ChangeType(dr[property.Name], Type.GetType(Primitive.FullName), CultureInfo.InvariantCulture);
                            property.SetValue(objMaster, Temp, null);

                        }
                        else
                        {
                            property.SetValue(objMaster, dr[property.Name], null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private static T ConvertTableToListNew<T>(DataRow dr) where T : new()
        {
            T objMaster = new T();
            try
            {

                foreach (PropertyInfo property in objMaster.GetType().GetProperties())
                {

                    if (dr.Table.Columns.Contains(property.Name.ToString()))
                    {

                        if (dr[property.Name] == DBNull.Value)
                        {
                            property.SetValue(objMaster, null, null);
                        }
                        else if (property.PropertyType.ToString() == "System.Nullable`1[System.Decimal]")
                        {
                            Type Primitive = Nullable.GetUnderlyingType(property.PropertyType);
                            object Temp = Convert.ChangeType(dr[property.Name], Type.GetType(Primitive.FullName), CultureInfo.InvariantCulture);
                            property.SetValue(objMaster, Temp, null);

                        }
                        else
                        {
                            property.SetValue(objMaster, dr[property.Name], null);
                        }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objMaster;
        }

        private DbType ConvertNullableIntoDatatype(PropertyInfo PropInfoCol)
        {
            DbType dbt = new DbType();
            if (PropInfoCol.PropertyType.Name.Contains("Nullable"))
            {
                if (PropInfoCol.Name == "DbId" || PropInfoCol.Name == "DBId")
                {
                    dbt = DbType.Int32;
                }
                else
                {
                    if (PropInfoCol.PropertyType.ToString().Contains("DateTime"))
                    {
                        dbt = DbType.DateTime;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Int32"))
                    {
                        dbt = DbType.Int32;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Decimal"))
                    {
                        dbt = DbType.Decimal;
                    }

                    else if (PropInfoCol.PropertyType.ToString().Contains("Byte"))
                    {
                        dbt = DbType.Byte;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("bool"))
                    {
                        dbt = DbType.Boolean;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("String"))
                    {
                        dbt = DbType.String;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Char") || PropInfoCol.PropertyType.ToString().Contains("char"))
                    {
                        dbt = DbType.String;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Time"))
                    {
                        dbt = DbType.Time;
                    }
                }
            }
            else
            {
                if (PropInfoCol.Name == "DbId" || PropInfoCol.Name == "DBId")
                {
                    dbt = DbType.Int32;
                }
                else
                {
                    if (PropInfoCol.PropertyType.ToString().Contains("Byte"))
                    {
                        dbt = DbType.Byte;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("bool"))
                    {
                        dbt = DbType.Boolean;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Char") || PropInfoCol.PropertyType.ToString().Contains("char"))
                    {
                        dbt = DbType.String;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Time"))
                    {
                        dbt = DbType.Time;
                    }
                    else
                    {
                        dbt = (DbType)Enum.Parse(typeof(DbType), PropInfoCol.PropertyType.Name);
                    }
                }
            }
            return dbt;
        }

        GeneralFunction GF = new GeneralFunction();

        private DbType ConvertNullable(PropertyInfo PropInfoCol)
        {
            DbType dbt = new DbType();
            if (PropInfoCol.PropertyType.Name.Contains("Nullable"))
            {
                if (PropInfoCol.Name == "DbId" || PropInfoCol.Name == "DBId")
                {
                    dbt = DbType.Int32;
                }
                else
                {
                    if (PropInfoCol.PropertyType.ToString().Contains("DateTime"))
                    {
                        dbt = DbType.DateTime;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Int32"))
                    {
                        dbt = DbType.Int32;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Decimal"))
                    {
                        dbt = DbType.Decimal;
                    }

                    else if (PropInfoCol.PropertyType.ToString().Contains("Byte"))
                    {
                        dbt = DbType.Binary;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("bool"))
                    {
                        dbt = DbType.Boolean;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("String"))
                    {
                        dbt = DbType.String;
                    }
                }
            }
            else
            {
                if (PropInfoCol.Name == "DbId" || PropInfoCol.Name == "DBId")
                {
                    dbt = DbType.Int32;
                }
                else
                {
                    if (PropInfoCol.PropertyType.ToString().Contains("Byte"))
                    {
                        dbt = DbType.Binary;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("bool"))
                    {
                        dbt = DbType.Boolean;
                    }
                    else
                    {
                        dbt = (DbType)Enum.Parse(typeof(DbType), PropInfoCol.PropertyType.Name);
                    }
                }
            }

            return dbt;
        }

        private string GetClientIP()
        {
            string ip = "";
            try
            {
                OperationContext context = OperationContext.Current;
                MessageProperties prop = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpoint =
                   prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                ip = endpoint.Address;
            }
            catch (Exception)
            {
                ip = "Unable to get Ip Address.";
            }

            return ip;

        }

        public static string DecryptStringAES(string cipherText)
        {

            var keybytes = Encoding.UTF8.GetBytes("A51f7e2h2j58r2d5");
            var iv = Encoding.UTF8.GetBytes("A51f7e2h2j58r2d5");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        #endregion

        #region Send Mail
        public bool SendMailMessage(string strFrom, string strPSW, string strTo, string strReplyTo, string strBcc, string strCc, string strSubject, string strBody, Stream streamAtt, string strAttFileName)
        {
            bool blnMailSentStatus = false;

            try
            {
                MailMessage mMailMessage = new MailMessage();
                mMailMessage.From = new MailAddress(strFrom);
                mMailMessage.To.Add(new MailAddress(strTo));
                if (strReplyTo != "")
                {
                    mMailMessage.ReplyTo = new MailAddress(strReplyTo);
                }

                if ((strBcc != null) && (strBcc != string.Empty))
                {
                    mMailMessage.Bcc.Add(new MailAddress(strBcc));
                }

                if ((strCc != null) && (strCc != string.Empty))
                {
                    mMailMessage.CC.Add(new MailAddress(strCc));
                }
                mMailMessage.Subject = strSubject;
                mMailMessage.Body = strBody;
                mMailMessage.IsBodyHtml = true;
                mMailMessage.Priority = MailPriority.Normal;

                if (streamAtt != null && streamAtt.Length > 0)
                {
                    if (String.IsNullOrEmpty(strAttFileName))
                        strAttFileName = "Attachment1";
                    mMailMessage.Attachments.Add(new Attachment(streamAtt, strAttFileName));
                }

                SmtpClient mSmtpClient = new SmtpClient();

                // mSmtpClient.Timeout = 20000;
                mSmtpClient.EnableSsl = true;
                // mSmtpClient.UseDefaultCredentials = false;
                // mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mSmtpClient.Credentials = new NetworkCredential(strFrom, strPSW);

                mSmtpClient.Host = "smtp.gmail.com";
                mSmtpClient.Port = 587;
                mSmtpClient.Send(mMailMessage);
                blnMailSentStatus = true;

            }
            catch (Exception ex)
            {
                string strEx = ex.Message;
            }
            return blnMailSentStatus;
        }
        #endregion

        #region Send SMS
        public string SendSMS(string MobileNo, string SMSText)
        {
            string strStatus = "failed";
            try
            {
                string authKey = "86595AqYFjZW7pIN557a8b5e";      //Old Key
                //string authKey = "234977AhvY26xSfP45b9603fc";
                //Sender ID,While using route4 sender id should be 6 characters long.
                string senderId = "iBosch";
                string message = HttpUtility.UrlEncode(SMSText);

                //Prepare you post parameters
                StringBuilder sbPostData = new StringBuilder();
                sbPostData.AppendFormat("authkey={0}", authKey);
                sbPostData.AppendFormat("&mobiles={0}", MobileNo);
                sbPostData.AppendFormat("&message={0}", message);
                sbPostData.AppendFormat("&sender={0}", senderId);
                // sbPostData.AppendFormat("&route={0}", "default");
                sbPostData.AppendFormat("&route={0}", 4);

                // For Hindi Message
                //sbPostData.AppendFormat("&unicode={0}", 1);

                //Call Send SMS API
                string sendSMSUri = "http://api.msg91.com/sendhttp.php";
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                if (response.StatusDescription.ToLower() == "ok")
                {

                }
                //Close the response
                reader.Close();
                response.Close();
                strStatus = "success";
            }
            catch (SystemException ex)
            {
                strStatus = ex.Message.ToString();
            }
            return strStatus;
        }
        #endregion

        #region Transaction
        #region Login 
        public returndbmlUser UserGetByLoginId(string strLoginId, string strPassword)
        {
            returndbmlUser objreturndbmlUser = new returndbmlUser();

            Database db = new SqlDatabase(GF.StrSetConnection());
            DataSet ds = new DataSet();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("Front.UserViewFrontGetByLoginId", strLoginId);

                db.LoadDataSet(cmd, ds, new string[] { "UserView" });

                if (ds.Tables["UserView"].Rows.Count > 0)
                {
                    objreturndbmlUser.objdbmlUserView = new ObservableCollection<dbmlUserView>
                    (from dRow in ds.Tables["UserView"].AsEnumerable() select (ConvertTableToListNew<dbmlUserView>(dRow)));

                    if (objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailVerify == null || objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailVerify == false)
                    {
                        objreturndbmlUser.objdbmlStatus.StatusId = 20;//Verification Pending
                        objreturndbmlUser.objdbmlStatus.Status = "Your email verification and password creation is pending, we have sent you mail on " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailId + " for Login ID " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().LoginId + " with a verification link.Please check you mail and click verification link to create password.";
                    }
                    else if (objreturndbmlUser.objdbmlUserView.FirstOrDefault().PassWord.Trim().Length < 3)
                    {
                        objreturndbmlUser.objdbmlStatus.StatusId = 30;// Password creation Pending
                        objreturndbmlUser.objdbmlStatus.Status = "Your password creation is pending, we have sent you mail on " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailId + " for Login ID " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().LoginId + " with a verification link.Please check you mail and click verification link to create password.";
                    }
                    else if (objreturndbmlUser.objdbmlUserView.FirstOrDefault().Active == false)
                    {
                        objreturndbmlUser.objdbmlStatus.StatusId = 40; // Activation Pending
                        objreturndbmlUser.objdbmlStatus.Status = "Your registration process was started on " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().CreateDate.ToString("dd/MM/yyyy") + " after eMail verification and password creation.Login ID " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().LoginId + " activation is pending at Natrax for Company " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().ZZCompanyName + ".Please wait till further intimation by mail on " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailId + ".You may contact us on XXXXXXXXXX in case";
                    }
                    else if (Cryptographer.CompareHash("ePGMS", strPassword, objreturndbmlUser.objdbmlUserView.FirstOrDefault().PassWord))
                    {
                        objreturndbmlUser.objdbmlStatus.StatusId = 1;
                    }
                    else
                    {
                        objreturndbmlUser = new returndbmlUser();
                        objreturndbmlUser.objdbmlStatus.StatusId = 10; // Password Not Match
                        objreturndbmlUser.objdbmlStatus.Status = "Incorrect Password";
                    }
                }
                else
                {
                    objreturndbmlUser = new returndbmlUser();
                    objreturndbmlUser.objdbmlStatus.StatusId = 5; // Password Not Match
                    objreturndbmlUser.objdbmlStatus.Status = "Incorrect User Name";
                }
            }
            catch (Exception ex)
            {
                objreturndbmlUser.objdbmlStatus.StatusId = 99;
                objreturndbmlUser.objdbmlStatus.Status = ex.Message + ex.StackTrace;
            }
            return objreturndbmlUser;
        }
            
        public returndbmlDashBoardWorkFlowViewFront DashBoardWorkFlowCount(int intUserId,int intCompanyId)
        {
            returndbmlDashBoardWorkFlowViewFront objreturndbmlDashBoardWorkFlowViewFront = new returndbmlDashBoardWorkFlowViewFront();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Setting].[DashBoardWorkFlowCount]", intUserId, intCompanyId);
                db.LoadDataSet(cmdGet, ds, new string[] { "DashBoard" });
                if (ds.Tables["DashBoard"].Rows.Count > 0)
                {
                    objreturndbmlDashBoardWorkFlowViewFront.objdbmlDashBoardWorkFlowViewFront = new ObservableCollection<dbmlDashBoardWorkFlowViewFront>(from dRow in ds.Tables["DashBoard"].AsEnumerable()
                                                                                                                                                         select (ConvertTableToListNew<dbmlDashBoardWorkFlowViewFront>(dRow)));
                }

                objreturndbmlDashBoardWorkFlowViewFront.objdbmlStatus.StatusId = 1;
                objreturndbmlDashBoardWorkFlowViewFront.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlDashBoardWorkFlowViewFront.objdbmlStatus.StatusId = 99;
                objreturndbmlDashBoardWorkFlowViewFront.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlDashBoardWorkFlowViewFront;
        }
                
        public returndbmlUser UserViewFrontGetByCompanyId(int intCompanyId)
        {
             returndbmlUser objreturndbmlUser = new returndbmlUser();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("Front.UserViewFrontGetByCompanyId", intCompanyId);
                db.LoadDataSet(cmdGet, ds, new string[] { "UserView" });
                if (ds.Tables["UserView"] != null && ds.Tables["UserView"].Rows.Count > 0)
                {
                    objreturndbmlUser.objdbmlUserView = new ObservableCollection<dbmlUserView>
                                                                    (from dRow in ds.Tables["UserView"].AsEnumerable() select (ConvertTableToListNew<dbmlUserView>(dRow)));
                }

                objreturndbmlUser.objdbmlStatus.StatusId = 1;
                objreturndbmlUser.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlUser.objdbmlStatus.StatusId = 99;
                objreturndbmlUser.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlUser;
        }

        public returndbmlUser UserViewFrontGetByDepartmentId(int intDepartmentId)
        {
            returndbmlUser objreturndbmlUser = new returndbmlUser();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("Front.UserViewFrontGetByDepartmentId", intDepartmentId);
                db.LoadDataSet(cmdGet, ds, new string[] { "UserView" });
                if (ds.Tables["UserView"] != null && ds.Tables["UserView"].Rows.Count > 0)
                {
                    objreturndbmlUser.objdbmlUserView = new ObservableCollection<dbmlUserView>
                                                                    (from dRow in ds.Tables["UserView"].AsEnumerable() select (ConvertTableToListNew<dbmlUserView>(dRow)));
                }

                objreturndbmlUser.objdbmlStatus.StatusId = 1;
                objreturndbmlUser.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlUser.objdbmlStatus.StatusId = 99;
                objreturndbmlUser.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlUser;
        }

        public returndbmlUser UserViewGetByLoginIdUserId(string strLoginId, int intUserId)
        {
            returndbmlUser objreturndbmlUser = new returndbmlUser();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("Front.UserViewGetByLoginIdUserId", strLoginId, intUserId);
                db.LoadDataSet(cmdGet, ds, new string[] { "UserView" });
                if (ds.Tables["UserView"] != null && ds.Tables["UserView"].Rows.Count > 0)
                {
                    objreturndbmlUser.objdbmlUserView = new ObservableCollection<dbmlUserView>
                                                                    (from dRow in ds.Tables["UserView"].AsEnumerable() select (ConvertTableToListNew<dbmlUserView>(dRow)));
                }

                objreturndbmlUser.objdbmlStatus.StatusId = 1;
                objreturndbmlUser.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlUser.objdbmlStatus.StatusId = 99;
                objreturndbmlUser.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlUser;
        }

        public returndbmlUser UserPaswordForgot(string strLoginId, string strEMail)
        {
            returndbmlUser objreturndbmlUser = new returndbmlUser();
            objreturndbmlUser = UserViewGetByLoginIdUserId(strLoginId,0);
            if (objreturndbmlUser.objdbmlUserView == null || objreturndbmlUser.objdbmlUserView.Count == 0)
            {
                objreturndbmlUser.objdbmlStatus.StatusId = 5;
                objreturndbmlUser.objdbmlStatus.Status = "User Details Not Found for LoginId : '" + strLoginId + "' and EMail : '" + strEMail + "'";
            }
            else if (objreturndbmlUser.objdbmlUserView.Count > 0 && objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailId != null &&
                    objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailId.Trim().ToLower() != strEMail.Trim().ToLower())
            {
                objreturndbmlUser.objdbmlStatus.StatusId = 10;
                objreturndbmlUser.objdbmlStatus.Status = "User Details Not Found for LoginId : '" + strLoginId + "' and EMail : '" + strEMail + "'";
            }
            else if (objreturndbmlUser.objdbmlUserView.Count > 0 && objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailId != null &&
                    objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailId.Trim().ToLower() == strEMail.Trim().ToLower())
            {
                DbTransaction trans; DbConnection con;
                Database db = new SqlDatabase(GF.StrSetConnection());
                con = db.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                System.Data.Common.DbCommand cmd = null;
                int intUserId = objreturndbmlUser.objdbmlUserView.FirstOrDefault().UserId;
                try
                {
                    cmd = db.GetStoredProcCommand("[Front].[UserPaswordForgot]");
                    db.AddInParameter(cmd, "UserId", DbType.Int32, intUserId);
                    db.ExecuteNonQuery(cmd, trans);

                    trans.Commit();

                    string strHost = System.Configuration.ConfigurationManager.AppSettings["strHostName"]; //"https://localhost:44307/";
                    string strLink = strHost + "Home/VerifyeMail?xyz=0dfs,ktgbdas,hdffg.khdfrhdduihdgtymdmpxjidgndlxcmhdgmdpldjn,dlkchgj,d,.fddfyre,hjlhhjhjlhjljhjlhdkjdhhdk,dmdhhnd,dkmdndhnndmdkkfbhjyhnhhfssdfgngfgfghgfjfgjgffbgfhfhfhdffdsfdgfdfhfhgfhgfjfwrtwfghkyredcbnmkiufssfgyhvgdrfrthhhjhmjmd&abc="
                                        + objreturndbmlUser.objdbmlUserView.FirstOrDefault().UserId
                                        + "&lmn=0dshffn56tgrehbncv6nwyuwgkliurscvjl'ljugbmkl;lkitgn;''lkjhhhjl;llkyhfcfbmkkdfhdfgffhf561g4d5bvgdf1bbdfbdvfgbnvbncvncvbbnxcdgfcbcb";
                    string strFrom = "testdemo052020@gmail.com", strReplyTo = "",
                        strTo = objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailId,
                        strBcc = string.Empty,
                        strCc = string.Empty,
                        strSubject = string.Empty, strBody = string.Empty;


                    strSubject = "Natrax - Forgot Password";
                    strBody = "Hello, ";

                    strBody += "<br /><br /><b>" + objreturndbmlUser.objdbmlUserView.FirstOrDefault().UserName + "</b>";
                    strBody += "<br /><b>" + objreturndbmlUser.objdbmlUserView.FirstOrDefault().ZZCompanyName + "</b>";

                    strBody += "<br /><br />Forgot password reset link for Login-ID <b> '" + objreturndbmlUser.objdbmlUserView.FirstOrDefault().LoginId + "</b>'";
                    strBody += ", please click on the below link to create password";
                    strBody += "<br /><b><a href='"+strLink+"'>Click Here</a></b>";



                    strBody += "<br /><br /><br />Regards";
                    strBody += "<br /><br /><span style='font-weight:bold;font-family:Trebuchet MS;font-style:italic'>Natrax Administrator</span>";

                    bool blnSentMail = SendMailMessage(strFrom, "test@dem0", strTo, strReplyTo, strBcc, strCc, strSubject, strBody, null, "");

                    objreturndbmlUser.objdbmlStatus.StatusId = 1;
                    objreturndbmlUser.objdbmlStatus.Status = "Password reset process for " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().UserName + " shall be initiated.<br>Forgot password reset link has been sent to '" + objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailId + "'.<br>Please click on link to create password for Login ID - " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().LoginId + "."; 

                }
                catch (Exception ex)
                {
                    objreturndbmlUser.objdbmlStatus.StatusId = 99;
                    objreturndbmlUser.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                    trans.Rollback();
                }
                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                objreturndbmlUser.objdbmlStatus.StatusId = 10;
                objreturndbmlUser.objdbmlStatus.Status = "User Details Not Found for LoginId : '" + strLoginId + "' and EMail : '" + strEMail + "'";
            }
            return objreturndbmlUser;
        }

        #endregion

        #region Properties / OptionList
        public returndbmlProperty PropertiesGetAll()
        {
            returndbmlProperty objreturndbmlProperty = new returndbmlProperty();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Setting].[PropertiesGetAll]");
                db.LoadDataSet(cmdGet, ds, new string[] { "Properties" });
                if (ds.Tables["Properties"].Rows.Count > 0)
                {
                    objreturndbmlProperty.objdbmlProperty = new ObservableCollection<dbmlProperty>(from dRow in ds.Tables["Properties"].AsEnumerable()
                                                                                                   select (ConvertTableToListNew<dbmlProperty>(dRow)));
                }

                objreturndbmlProperty.objdbmlStatus.StatusId = 1;
                objreturndbmlProperty.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlProperty.objdbmlStatus.StatusId = 99;
                objreturndbmlProperty.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlProperty;
        }

        public returndbmlProperty PropertiesGetByPropertyTypeId(int intPropertyTypeId)
        {
            returndbmlProperty objreturndbmlProperty = new returndbmlProperty();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Setting].[PropertiesGetByPropertyTypeId]", intPropertyTypeId);
                db.LoadDataSet(cmdGet, ds, new string[] { "Properties" });
                if (ds.Tables["Properties"].Rows.Count > 0)
                {
                    objreturndbmlProperty.objdbmlProperty = new ObservableCollection<dbmlProperty>(from dRow in ds.Tables["Properties"].AsEnumerable()
                                                                                                   select (ConvertTableToListNew<dbmlProperty>(dRow)));
                }

                objreturndbmlProperty.objdbmlStatus.StatusId = 1;
                objreturndbmlProperty.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlProperty.objdbmlStatus.StatusId = 99;
                objreturndbmlProperty.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlProperty;
        }

        public returndbmlOptionList OptionListGetByPropertyId(int intPropertyId)
        {
            returndbmlOptionList objreturndbmlOptionList = new returndbmlOptionList();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Setting].[OptionListGetByPropertyId]", intPropertyId);
                db.LoadDataSet(cmdGet, ds, new string[] { "OptionList" });
                if (ds.Tables["OptionList"].Rows.Count > 0)
                {
                    objreturndbmlOptionList.objdbmlOptionList = new ObservableCollection<dbmlOptionList>(from dRow in ds.Tables["OptionList"].AsEnumerable()
                                                                                                         select (ConvertTableToListNew<dbmlOptionList>(dRow)));
                }

                objreturndbmlOptionList.objdbmlStatus.StatusId = 1;
                objreturndbmlOptionList.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlOptionList.objdbmlStatus.StatusId = 99;
                objreturndbmlOptionList.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlOptionList;
        }
        #endregion

        #region Company/Department      
        public returndbmlCompanyView CustomerMasterInsertFront(returndbmlCompanyView objreturndbmlCompanyView)
        {
            returndbmlCompanyView objreturndbmlCompanyViewReturn = new returndbmlCompanyView();
            returndbmlUser objreturndbmlUserTemp = UserViewGetByLoginIdUserId(objreturndbmlCompanyView.objdbmlCompanyView.FirstOrDefault().ZZLoginId,0);
            if (objreturndbmlUserTemp.objdbmlUserView != null && objreturndbmlUserTemp.objdbmlUserView.Count > 0)
            {
                objreturndbmlCompanyViewReturn.objdbmlStatus.StatusId = 10;
                objreturndbmlCompanyViewReturn.objdbmlStatus.Status = "LoginId already exist";
            }
            else
            {
                DbTransaction trans;
                DbConnection con;
                Database db = new SqlDatabase(GF.StrSetConnection());
                con = db.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                System.Data.Common.DbCommand cmd = null;
                try
                {
                    int intCompanyId = 0;
                    foreach (var Header in objreturndbmlCompanyView.objdbmlCompanyView)
                    {
                        cmd = db.GetStoredProcCommand("[Front].[CustomerMasterInsertFront]");
                        foreach (PropertyInfo PropInfoCol in Header.GetType().GetProperties())
                        {
                            string str = PropInfoCol.Name;
                            if (str.Length <= 2)
                                str = str + "modified";
                            if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                            {
                                DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                                db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(Header, null));
                            }
                            if (str.ToUpper() == "ZZLOGINID")
                            {
                                DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                                db.AddInParameter(cmd, "LoginId", dbt, PropInfoCol.GetValue(Header, null));
                            }

                        }

                        db.AddOutParameter(cmd, "IdOut", DbType.Int32, 0);
                        db.ExecuteNonQuery(cmd, trans);
                        intCompanyId = (int)db.GetParameterValue(cmd, "@IdOut");

                    }

                    trans.Commit();


                    objreturndbmlCompanyViewReturn = CompanyViewGetByCompanyId(intCompanyId);
                    returndbmlUser objreturndbmlUser = UserViewFrontGetByCompanyId(intCompanyId);
                    objreturndbmlCompanyViewReturn.objdbmlUserView = objreturndbmlUser.objdbmlUserView;

                    if (objreturndbmlUser.objdbmlUserView.Count > 0 && intCompanyId > 0)
                    {
                        string strHost = System.Configuration.ConfigurationManager.AppSettings["strHostName"]; //"https://localhost:44307/";
                        string strLink = strHost + "Home/VerifyeMail?xyz=0dfs,ktgbdas,hdffg.khdfrhdduihdgtymdmpxjidgndlxcmhdgmdpldjn,dlkchgj,d,.fddfyre,hjlhhjhjlhjljhjlhdkjdhhdk,dmdhhnd,dkmdndhnndmdkkfbhjyhnhhfssdfgngfgfghgfjfgjgffbgfhfhfhdffdsfdgfdfhfhgfhgfjfwrtwfghkyredcbnmkiufssfgyhvgdrfrthhhjhmjmd&abc="
                                            + objreturndbmlUser.objdbmlUserView.FirstOrDefault().UserId
                                            +"&lmn=0dshffn56tgrehbncv6nwyuwgkliurscvjl'ljugbmkl;lkitgn;''lkjhhhjl;llkyhfcfbmkkdfhdfgffhf561g4d5bvgdf1bbdfbdvfgbnvbncvncvbbnxcdgfcbcb";
                        string strFrom = "testdemo052020@gmail.com", strReplyTo = "",
                            strTo = objreturndbmlCompanyViewReturn.objdbmlCompanyView.FirstOrDefault().Email,
                            strBcc = string.Empty,
                            strCc = string.Empty,
                            strSubject = string.Empty, strBody = string.Empty;


                        strSubject = "Natrax - Verify email";
                        strBody = "Hello, ";

                        strBody += "<br /><br /><b>" + objreturndbmlCompanyViewReturn.objdbmlCompanyView.FirstOrDefault().ContactPerson + "</b>";
                        strBody += "<br /><b>" + objreturndbmlCompanyViewReturn.objdbmlCompanyView.FirstOrDefault().CompanyName + "</b>";

                        strBody += "<br /><br />You have successfully registered on Natrax with Login-ID <b> '" + objreturndbmlUser.objdbmlUserView.FirstOrDefault().LoginId+"</b>'";
                        strBody += ", please click on the below link to verify your email and create password";
                        strBody += "<br /><b><a href='" + strLink + "'>Click Here</a></b>";



                        strBody += "<br /><br /><br />Regards";
                        strBody += "<br /><br /><span style='font-weight:bold;font-family:Trebuchet MS;font-style:italic'>Natrax Administrator</span>";

                        bool blnSentMail = SendMailMessage(strFrom, "test@dem0", strTo, strReplyTo, strBcc, strCc, strSubject, strBody, null, "");

                    }
                }
                catch (Exception ex)
                {
                    objreturndbmlCompanyViewReturn.objdbmlStatus.StatusId = 99;
                    objreturndbmlCompanyViewReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                    trans.Rollback();
                }
                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return objreturndbmlCompanyViewReturn;
        }

        public returndbmlUser UsereMailIdVerification(int intUserId)
        {
            returndbmlUser objreturndbmlUser = new returndbmlUser();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("[Front].[UsereMailIdVerification]");
                db.AddInParameter(cmd, "UserId", DbType.Int32, intUserId);
                db.AddOutParameter(cmd, "IdOut", DbType.Int32, 0);
                db.ExecuteNonQuery(cmd, trans);
                int intIdOut = (int)db.GetParameterValue(cmd, "@IdOut");

                if (intIdOut > 0)
                {
                    trans.Commit();
                    objreturndbmlUser = UserViewGetByLoginIdUserId("",intUserId);
                    objreturndbmlUser.objdbmlStatus.StatusId = 1;
                    objreturndbmlUser.objdbmlStatus.Status = "Your emailId - " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailId + " is successfully verified, Please create Password";
                }
                else if(intIdOut==-1)
                {
                    objreturndbmlUser.objdbmlStatus.StatusId = -1;
                    objreturndbmlUser.objdbmlStatus.Status ="User Details Not Found";
                }
                else if (intIdOut == -2)
                {                   
                    objreturndbmlUser = UserViewGetByLoginIdUserId("", intUserId);
                    objreturndbmlUser.objdbmlStatus.StatusId = -2;
                    if (objreturndbmlUser.objdbmlUserView.Count > 0)
                    {
                        if (objreturndbmlUser.objdbmlUserView.FirstOrDefault().PassWord.Trim().Length <3)
                        {
                            objreturndbmlUser.objdbmlStatus.StatusId = 1;
                            objreturndbmlUser.objdbmlStatus.Status = "Your emailId - " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailId + " is already verified, Please create Password";
                        }
                        else if(objreturndbmlUser.objdbmlUserView.FirstOrDefault().Active == false)
                        {
                            objreturndbmlUser.objdbmlStatus.StatusId = -3;
                            objreturndbmlUser.objdbmlStatus.Status = "Your registration process was started on " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().CreateDate.ToString("dd/MM/yyyy") + " after eMail verification and password creation.<br>Login ID " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().LoginId + " activation is pending at Natrax for Company " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().ZZCompanyName + ".<br>Please wait till further intimation by mail on " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().EmailId + ".<br>You may contact us on XXXXXXXXXX in case";
                        }
                        else
                        {
                            objreturndbmlUser.objdbmlStatus.StatusId = -2;
                            objreturndbmlUser.objdbmlStatus.Status = "Your Login ID - " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().LoginId + " for " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().UserName + " / " + objreturndbmlUser.objdbmlUserView.FirstOrDefault().ZZCompanyName + " has been activated by Natrax Administrator.";
                        }
                    }
                    else
                    {
                        objreturndbmlUser.objdbmlStatus.StatusId = -4;
                        objreturndbmlUser.objdbmlStatus.Status = "User Details Not Found";
                    }
                   
                }

            }
            catch (Exception ex)
            {
                objreturndbmlUser.objdbmlStatus.StatusId = 99;
                objreturndbmlUser.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlUser;
        }

        public returndbmlUser UserPaswordReset(int intUserId,string strPassword)
        {
            returndbmlUser objreturndbmlUser = new returndbmlUser();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                string strUserPassword = Cryptographer.CreateHash("ePGMS", strPassword);

                cmd = db.GetStoredProcCommand("[Front].[UserPaswordReset]");
                db.AddInParameter(cmd, "UserId", DbType.Int32, intUserId);
                db.AddInParameter(cmd, "Password", DbType.String, strUserPassword);
                db.ExecuteNonQuery(cmd, trans);

                trans.Commit();
                objreturndbmlUser = UserViewGetByLoginIdUserId("", intUserId);
                objreturndbmlUser.objdbmlStatus.StatusId = 1;
                if (objreturndbmlUser.objdbmlUserView.Count > 0 && objreturndbmlUser.objdbmlUserView.FirstOrDefault().Active == true)
                {
                    objreturndbmlUser.objdbmlStatus.Status = "New Password successfully created. Go to Login Page for Login to Natrax";
                }
                else if(objreturndbmlUser.objdbmlUserView.Count > 0)
                {
                    dbmlUserView objUserView = objreturndbmlUser.objdbmlUserView.FirstOrDefault();
                    objreturndbmlUser.objdbmlStatus.Status = "Your registration process was started on "+ objUserView.CreateDate.ToString("dd/MM/yyyy")+ " after eMail verification and password creation.<br>Login ID "+ objUserView.LoginId+ " activation is pending at Natrax for Company " + objUserView.ZZCompanyName + ".<br>Please wait till further intimation by mail on " + objUserView.EmailId + ".<br>You may contact us on XXXXXXXXXX in case";
                }
            }
            catch (Exception ex)
            {
                objreturndbmlUser.objdbmlStatus.StatusId = 99;
                objreturndbmlUser.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlUser;
        }

        public returndbmlCompanyDepartment CompanyDepartmentInsert(returndbmlCompanyDepartment objreturndbmlCompanyDepartment)
        {
            returndbmlCompanyDepartment objreturndbmlCompanyDepartmentReturn = new returndbmlCompanyDepartment();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intCompanyId = 0;
                foreach (var Header in objreturndbmlCompanyDepartment.objdbmlCompanyDepartment)
                {
                    intCompanyId = (int)Header.CustomerMasterId;
                    cmd = db.GetStoredProcCommand("[Master].[CompanyDepartmentInsert]");
                    foreach (PropertyInfo PropInfoCol in Header.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(Header, null));
                        }
                    }
                   
                    db.ExecuteNonQuery(cmd, trans);                   

                }

                trans.Commit();


                objreturndbmlCompanyDepartmentReturn = CompanyDepartmentGetByCustomerMasterId(intCompanyId);
                
            }
            catch (Exception ex)
            {
                objreturndbmlCompanyDepartmentReturn.objdbmlStatus.StatusId = 99;
                objreturndbmlCompanyDepartmentReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlCompanyDepartmentReturn;
        }

        public returndbmlCompanyDepartment CompanyDepartmentUpdate(returndbmlCompanyDepartment objreturndbmlCompanyDepartment)
        {
            returndbmlCompanyDepartment objreturndbmlCompanyDepartmentReturn = new returndbmlCompanyDepartment();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intCompanyId = 0;
                foreach (var Header in objreturndbmlCompanyDepartment.objdbmlCompanyDepartment)
                {
                    intCompanyId = (int)Header.CustomerMasterId;
                    cmd = db.GetStoredProcCommand("[Master].[CompanyDepartmentUpdate]");
                    foreach (PropertyInfo PropInfoCol in Header.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(Header, null));
                        }
                    }

                    db.ExecuteNonQuery(cmd, trans);

                }

                trans.Commit();


                objreturndbmlCompanyDepartmentReturn = CompanyDepartmentGetByCustomerMasterId(intCompanyId);

            }
            catch (Exception ex)
            {
                objreturndbmlCompanyDepartmentReturn.objdbmlStatus.StatusId = 99;
                objreturndbmlCompanyDepartmentReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlCompanyDepartmentReturn;
        }

        public returndbmlUser UserInsert(returndbmlUser objreturndbmlUser)
        {
            returndbmlUser objreturndbmlUserReturn = new returndbmlUser();
            returndbmlCompanyView objreturndbmlCompanyViewReturn = new returndbmlCompanyView();
            returndbmlUser objreturndbmlUserTemp = UserViewGetByLoginIdUserId(objreturndbmlUser.objdbmlUserView.FirstOrDefault().LoginId, 0);
            if (objreturndbmlUserTemp.objdbmlUserView != null && objreturndbmlUserTemp.objdbmlUserView.Count > 0)
            {
                objreturndbmlUserReturn.objdbmlStatus.StatusId = 10;
                objreturndbmlUserReturn.objdbmlStatus.Status = "LoginId already exist";
            }
            else
            {
                DbTransaction trans; DbConnection con;
                Database db = new SqlDatabase(GF.StrSetConnection());
                con = db.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                System.Data.Common.DbCommand cmd = null;
                try
                {
                    int intCompanyId = 0;
                    foreach (var Header in objreturndbmlUser.objdbmlUserView)
                    {
                        intCompanyId = (int)Header.CustomerMasterId;
                        cmd = db.GetStoredProcCommand("[Security].[UserInsert]");
                        foreach (PropertyInfo PropInfoCol in Header.GetType().GetProperties())
                        {
                            string str = PropInfoCol.Name;
                            if (str.Length <= 2)
                                str = str + "modified";
                            if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                            {
                                DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                                if (str.ToUpper() == "PASSWORD" && Convert.ToString(PropInfoCol.GetValue(Header, null)).Length>2)
                                {
                                    string strUserPassword = Cryptographer.CreateHash("ePGMS", Convert.ToString(PropInfoCol.GetValue(Header, null)));
                                    db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, strUserPassword);
                                }
                                else
                                {                                  
                                    db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(Header, null));
                                }
                            }
                        }

                        db.AddOutParameter(cmd, "IdOut", DbType.Int32, 0);
                        db.ExecuteNonQuery(cmd, trans);
                        int UserId = (int)db.GetParameterValue(cmd, "@IdOut");

                    }

                    trans.Commit();


                    objreturndbmlUserReturn = UserViewFrontGetByCompanyId(intCompanyId);

                }
                catch (Exception ex)
                {
                    objreturndbmlUserReturn.objdbmlStatus.StatusId = 99;
                    objreturndbmlUserReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                    trans.Rollback();
                }
                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return objreturndbmlUserReturn;
        }

        public returndbmlUser UserUpdate(returndbmlUser objreturndbmlUser)
        {
            returndbmlUser objreturndbmlUserReturn = new returndbmlUser();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intCompanyId = 0;
                foreach (var Header in objreturndbmlUser.objdbmlUserView)
                {
                    intCompanyId = (int)Header.CustomerMasterId;
                    cmd = db.GetStoredProcCommand("[Security].[UserUpdate]");
                    foreach (PropertyInfo PropInfoCol in Header.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(Header, null));
                        }
                    }

                    db.ExecuteNonQuery(cmd, trans);

                }

                trans.Commit();


                objreturndbmlUserReturn = UserViewFrontGetByCompanyId(intCompanyId);

            }
            catch (Exception ex)
            {
                objreturndbmlUserReturn.objdbmlStatus.StatusId = 99;
                objreturndbmlUserReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlUserReturn;
        }

        public returndbmlCompanyView CompanyViewGetByCompanyId(int intCompanyId)
        {
            returndbmlCompanyView objreturndbmlCompanyView = new returndbmlCompanyView();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("Front.CustomerMasterViewFrontGetByCustomerMasterId", intCompanyId);
                db.LoadDataSet(cmdGet, ds, new string[] { "CompanyView" });
                if (ds.Tables["CompanyView"] != null && ds.Tables["CompanyView"].Rows.Count > 0)
                {
                    objreturndbmlCompanyView.objdbmlCompanyView = new ObservableCollection<dbmlCompanyView>
                                                                    (from dRow in ds.Tables["CompanyView"].AsEnumerable() select (ConvertTableToListNew<dbmlCompanyView>(dRow)));
                }

                objreturndbmlCompanyView.objdbmlStatus.StatusId = 1;
                objreturndbmlCompanyView.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlCompanyView.objdbmlStatus.StatusId = 99;
                objreturndbmlCompanyView.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlCompanyView;
        }

        public returndbmlCompanyDepartment CompanyDepartmentGetByCustomerMasterId(int intCustomerMasterId)
        {
            returndbmlCompanyDepartment objreturndbmlCompanyDepartment = new returndbmlCompanyDepartment();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("Front.CompanyDepartmentViewFrontGetByCompanyId", intCustomerMasterId);
                db.LoadDataSet(cmdGet, ds, new string[] { "CompanyDepartment" });
                if (ds.Tables["CompanyDepartment"].Rows.Count > 0)
                {
                    objreturndbmlCompanyDepartment.objdbmlCompanyDepartment = new ObservableCollection<dbmlCompanyDepartment>(from dRow in ds.Tables["CompanyDepartment"].AsEnumerable() select (ConvertTableToListNew<dbmlCompanyDepartment>(dRow)));
                }

                objreturndbmlCompanyDepartment.objdbmlStatus.StatusId = 1;
                objreturndbmlCompanyDepartment.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlCompanyDepartment.objdbmlStatus.StatusId = 99;
                objreturndbmlCompanyDepartment.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlCompanyDepartment;
        }

        public returndbmlState StateGetAll()
        {
            returndbmlState objreturndbmlState = new returndbmlState();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Master].[StateGetAll]");
                db.LoadDataSet(cmdGet, ds, new string[] { "State" });
                if (ds.Tables["State"].Rows.Count > 0)
                {
                    objreturndbmlState.objdbmlState = new ObservableCollection<dbmlState>(from dRow in ds.Tables["State"].AsEnumerable()
                                                                                          select (ConvertTableToListNew<dbmlState>(dRow)));
                }

                objreturndbmlState.objdbmlStatus.StatusId = 1;
                objreturndbmlState.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlState.objdbmlStatus.StatusId = 99;
                objreturndbmlState.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlState;
        }

        public returndbmlDistrict DistrictGetByStateId(int intStateId)
        {
            returndbmlDistrict objreturndbmlDistrict = new returndbmlDistrict();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Master].[DistrictGetByStateId]", intStateId);
                db.LoadDataSet(cmdGet, ds, new string[] { "District" });
                if (ds.Tables["District"].Rows.Count > 0)
                {
                    objreturndbmlDistrict.objdbmlDistrict = new ObservableCollection<dbmlDistrict>(from dRow in ds.Tables["District"].AsEnumerable()
                                                                                                   select (ConvertTableToListNew<dbmlDistrict>(dRow)));
                }

                objreturndbmlDistrict.objdbmlStatus.StatusId = 1;
                objreturndbmlDistrict.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlDistrict.objdbmlStatus.StatusId = 99;
                objreturndbmlDistrict.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlDistrict;
        }

        public returndbmlDistrict DistrictGetAll()
        {
            returndbmlDistrict objreturndbmlDistrict = new returndbmlDistrict();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Master].[DistrictGetAll]");
                db.LoadDataSet(cmdGet, ds, new string[] { "District" });
                if (ds.Tables["District"].Rows.Count > 0)
                {
                    objreturndbmlDistrict.objdbmlDistrict = new ObservableCollection<dbmlDistrict>(from dRow in ds.Tables["District"].AsEnumerable()
                                                                                                   select (ConvertTableToListNew<dbmlDistrict>(dRow)));
                }

                objreturndbmlDistrict.objdbmlStatus.StatusId = 1;
                objreturndbmlDistrict.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlDistrict.objdbmlStatus.StatusId = 99;
                objreturndbmlDistrict.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlDistrict;
        }

        public returndbmlCustomerMasterPhoto CustomerMasterPhotoGetByCustomerMasterId(int intCustomerMasterId)
        {
            returndbmlCustomerMasterPhoto objreturndbmlCustomerMasterPhoto = new returndbmlCustomerMasterPhoto();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Front].[CustomerMasterPhotoGetByCustomerMasterId]", intCustomerMasterId);
                db.LoadDataSet(cmdGet, ds, new string[] { "CustomerMasterPhoto" });
                if (ds.Tables["CustomerMasterPhoto"].Rows.Count > 0)
                {
                    objreturndbmlCustomerMasterPhoto.objdbmlCustomerMasterPhoto = new ObservableCollection<dbmlCustomerMasterPhoto>(from dRow in ds.Tables["CustomerMasterPhoto"].AsEnumerable()
                                                                                                   select (ConvertTableToListNew<dbmlCustomerMasterPhoto>(dRow)));
                }

                objreturndbmlCustomerMasterPhoto.objdbmlStatus.StatusId = 1;
                objreturndbmlCustomerMasterPhoto.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlCustomerMasterPhoto.objdbmlStatus.StatusId = 99;
                objreturndbmlCustomerMasterPhoto.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlCustomerMasterPhoto;
        }

        public returndbmlCustomerMasterPhoto CustomerMasterPhotoInsert(returndbmlCustomerMasterPhoto objreturndbmlCustomerMasterPhoto)
        {
            returndbmlCustomerMasterPhoto objreturndbmlCustomerMasterPhotoReturn = new returndbmlCustomerMasterPhoto();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intCustomerMasterId = 0;
                foreach (var Header in objreturndbmlCustomerMasterPhoto.objdbmlCustomerMasterPhoto)
                {
                    intCustomerMasterId = (int)Header.CustomerMasterId;
                    cmd = db.GetStoredProcCommand("[Front].[CustomerMasterPhotoInsert]");
                    foreach (PropertyInfo PropInfoCol in Header.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(Header, null));
                        }
                    }

                    db.ExecuteNonQuery(cmd, trans);
                }

                trans.Commit();

                objreturndbmlCustomerMasterPhotoReturn = CustomerMasterPhotoGetByCustomerMasterId(intCustomerMasterId);

            }
            catch (Exception ex)
            {
                objreturndbmlCustomerMasterPhotoReturn.objdbmlStatus.StatusId = 99;
                objreturndbmlCustomerMasterPhotoReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlCustomerMasterPhotoReturn;
        }

        public returndbmlCustomerMasterPhoto CustomerMasterPhotoDelete(int intCustomerMasterId,int intIamgeSerialNo)
        {
            returndbmlCustomerMasterPhoto objreturndbmlCustomerMasterPhotoReturn = new returndbmlCustomerMasterPhoto();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("[Front].[CustomerMasterPhotoDelete]", intCustomerMasterId, intIamgeSerialNo);
                db.ExecuteNonQuery(cmd, trans);

                trans.Commit();

                objreturndbmlCustomerMasterPhotoReturn = CustomerMasterPhotoGetByCustomerMasterId(intCustomerMasterId);

            }
            catch (Exception ex)
            {
                objreturndbmlCustomerMasterPhotoReturn.objdbmlStatus.StatusId = 99;
                objreturndbmlCustomerMasterPhotoReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlCustomerMasterPhotoReturn;
        }

        #endregion

        #region Booking

        #region Basic      
        public returndbmlBooking BookingInsert(returndbmlBooking objreturndbmlBooking)
        {
            returndbmlBooking returndbmlBookingReturn = new returndbmlBooking();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intBookingId = 0;
                foreach (var Header in objreturndbmlBooking.objdbmlBookingList)
                {
                    cmd = db.GetStoredProcCommand("[Transaction].[BookingInsert]");
                    foreach (PropertyInfo PropInfoCol in Header.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(Header, null));
                        }
                    }

                    db.AddOutParameter(cmd, "IdOut", DbType.Int32, 0);
                    db.ExecuteNonQuery(cmd, trans);
                    intBookingId = (int)db.GetParameterValue(cmd, "@IdOut");

                }

                trans.Commit();


                returndbmlBookingReturn = BookingViewGetByBookingId(intBookingId);
            }
            catch (Exception ex)
            {
                returndbmlBookingReturn.objdbmlStatus.StatusId = 99;
                returndbmlBookingReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return returndbmlBookingReturn;
        }

        public returndbmlBooking BookingUpdate(returndbmlBooking objreturndbmlBooking)
        {
            returndbmlBooking returndbmlBookingReturn = new returndbmlBooking();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intBookingId = objreturndbmlBooking.objdbmlBookingList.FirstOrDefault().BookingId;
                foreach (var Header in objreturndbmlBooking.objdbmlBookingList)
                {
                    cmd = db.GetStoredProcCommand("[Transaction].[BookingUpdate]");
                    foreach (PropertyInfo PropInfoCol in Header.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(Header, null));
                        }
                    }
                    db.ExecuteNonQuery(cmd, trans);

                }

                trans.Commit();


                returndbmlBookingReturn = BookingViewGetByBookingId(intBookingId);
            }
            catch (Exception ex)
            {
                returndbmlBookingReturn.objdbmlStatus.StatusId = 99;
                returndbmlBookingReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return returndbmlBookingReturn;
        }

        public returndbmlStatus BookingDeleteAllByBookingId(int intBookingId)
        {
            returndbmlStatus objreturndbmlStatus = new returndbmlStatus();
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("[Transaction].[BookingDeleteAllByBookingId]");
                db.ExecuteNonQuery(cmd, trans);

                trans.Commit();
                objreturndbmlStatus.objdbmlStatus.StatusId = 1;
                objreturndbmlStatus.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlStatus.objdbmlStatus.StatusId = 99;
                objreturndbmlStatus.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlStatus;
        }

        public returndbmlBooking BookingViewGetByBookingId(int intBookingId)
        {
            returndbmlBooking objreturndbmlBooking = new returndbmlBooking();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Transaction].[BookingViewGetByBookingId]", intBookingId);
                db.LoadDataSet(cmdGet, ds, new string[] { "Booking" });
                if (ds.Tables["Booking"].Rows.Count > 0)
                {
                    objreturndbmlBooking.objdbmlBookingList = new ObservableCollection<dbmlBookingView>(from dRow in ds.Tables["Booking"].AsEnumerable() select (ConvertTableToListNew<dbmlBookingView>(dRow)));
                }

                objreturndbmlBooking.objdbmlStatus.StatusId = 1;
                objreturndbmlBooking.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlBooking.objdbmlStatus.StatusId = 99;
                objreturndbmlBooking.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlBooking;
        }

        public returndbmlBooking BookingViewGetByCompanyIdStatusPropId(int intCompanyId, int intStatusPropId)
        {
            returndbmlBooking objreturndbmlBooking = new returndbmlBooking();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Transaction].[BookingViewGetByCompanyIdStatusPropId]", intCompanyId, intStatusPropId);
                db.LoadDataSet(cmdGet, ds, new string[] { "Booking" });
                if (ds.Tables["Booking"].Rows.Count > 0)
                {
                    objreturndbmlBooking.objdbmlBookingList = new ObservableCollection<dbmlBookingView>(from dRow in ds.Tables["Booking"].AsEnumerable() select (ConvertTableToListNew<dbmlBookingView>(dRow)));
                }

                objreturndbmlBooking.objdbmlStatus.StatusId = 1;
                objreturndbmlBooking.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlBooking.objdbmlStatus.StatusId = 99;
                objreturndbmlBooking.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlBooking;
        }              

        public returndbmlBookingSearchView BookingSearchViewGetByCompanyIdFromDateToDateFront(int intCompanyId, DateTime dtFromDate, DateTime dtToDate, int intBPId, int intStatusPropId)
        {
            returndbmlBookingSearchView objreturndbmlBookingSearchView = new returndbmlBookingSearchView();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Front].[BookingSearchViewFrontGetByCompanyIdFromDateToDate]", intCompanyId, dtFromDate, dtToDate, intBPId, intStatusPropId);
                db.LoadDataSet(cmdGet, ds, new string[] { "Booking" });
                if (ds.Tables["Booking"] != null && ds.Tables["Booking"].Rows.Count > 0)
                {
                    objreturndbmlBookingSearchView.objdbmlBookingSearchView = new ObservableCollection<dbmlBookingSearchView>(from dRow in ds.Tables["Booking"].AsEnumerable()
                                                                                                                              select (ConvertTableToListNew<dbmlBookingSearchView>(dRow)));
                }

                objreturndbmlBookingSearchView.objdbmlStatus.StatusId = 1;
                objreturndbmlBookingSearchView.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlBookingSearchView.objdbmlStatus.StatusId = 99;
                objreturndbmlBookingSearchView.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlBookingSearchView;
        }

        public returndbmlStatus BookingQuotationPIDetailInsertByBookingId(int intDocId)
        {
            returndbmlStatus objreturndbmlStatus = new returndbmlStatus();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("[Transaction].[BookingQuotationPIDetailInsertByBookingId]", intDocId);
                db.ExecuteNonQuery(cmd, trans);
                trans.Commit();
                objreturndbmlStatus.objdbmlStatus.StatusId = 1;
                objreturndbmlStatus.objdbmlStatus.Status = "Success";

            }
            catch (Exception ex)
            {
                objreturndbmlStatus.objdbmlStatus.StatusId = 99;
                objreturndbmlStatus.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlStatus;
        }

        public returndbmlBookingSearchView RFQBookingSearchViewFrontGetByCompanyIdFromDateToDate(int intCompanyId, DateTime dtFromDate, DateTime dtToDate, int intBPId, int intStatusPropId)
        {
            returndbmlBookingSearchView objreturndbmlBookingSearchView = new returndbmlBookingSearchView();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Front].[RFQBookingSearchViewFrontGetByCompanyIdFromDateToDate]", intCompanyId, dtFromDate, dtToDate, intBPId, intStatusPropId);
                db.LoadDataSet(cmdGet, ds, new string[] { "Booking" });
                if (ds.Tables["Booking"] != null && ds.Tables["Booking"].Rows.Count > 0)
                {
                    objreturndbmlBookingSearchView.objdbmlBookingSearchView = new ObservableCollection<dbmlBookingSearchView>(from dRow in ds.Tables["Booking"].AsEnumerable()
                                                                                                                              select (ConvertTableToListNew<dbmlBookingSearchView>(dRow)));
                }

                objreturndbmlBookingSearchView.objdbmlStatus.StatusId = 1;
                objreturndbmlBookingSearchView.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlBookingSearchView.objdbmlStatus.StatusId = 99;
                objreturndbmlBookingSearchView.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlBookingSearchView;
        }

        public returndbmlRFQBookingDetail RFQBookingDetailGetByBookingIdBPId(int intBookingId, int intBPId)
        {
            returndbmlRFQBookingDetail objreturndbmlRFQBookingDetail = new returndbmlRFQBookingDetail();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Transaction].[BookingViewGetByBookingId]", intBookingId);
                db.LoadDataSet(cmdGet, ds, new string[] { "Booking" });
                if (ds.Tables["Booking"].Rows.Count > 0)
                {
                    objreturndbmlRFQBookingDetail.objdbmlBookingView = new ObservableCollection<dbmlBookingView>(from dRow in ds.Tables["Booking"].AsEnumerable() select (ConvertTableToListNew<dbmlBookingView>(dRow)));
                }

                cmdGet = db.GetStoredProcCommand("[Transaction].[ListOfVehicleComponentGetByDocId]", intBookingId);
                db.LoadDataSet(cmdGet, ds, new string[] { "Vehicle" });
                if (ds.Tables["Vehicle"].Rows.Count > 0)
                {
                    objreturndbmlRFQBookingDetail.objdbmlListOfVehicleComponent = new ObservableCollection<dbmlListOfVehicleComponent>(from dRow in ds.Tables["Vehicle"].AsEnumerable() select (ConvertTableToListNew<dbmlListOfVehicleComponent>(dRow)));
                }

                cmdGet = db.GetStoredProcCommand("[Front].[RFQTrackBookingDetailViewFrontGetByBookingId]", intBookingId);
                db.LoadDataSet(cmdGet, ds, new string[] { "TrackBookingDetail" });
                if (ds.Tables["TrackBookingDetail"] != null && ds.Tables["TrackBookingDetail"].Rows.Count > 0)
                {
                    objreturndbmlRFQBookingDetail.objdbmlTrackBookingDetail = new ObservableCollection<dbmlTrackBookingDetail>
                                                                    (from dRow in ds.Tables["TrackBookingDetail"].AsEnumerable() select (ConvertTableToListNew<dbmlTrackBookingDetail>(dRow)));
                }

                cmdGet = db.GetStoredProcCommand("[Front].[QFQTrackBookingTimeDetailIdViewFrontGetByBookingId]", intBookingId);
                db.LoadDataSet(cmdGet, ds, new string[] { "TrackBookingTimeDetail" });
                if (ds.Tables["TrackBookingTimeDetail"] != null && ds.Tables["TrackBookingTimeDetail"].Rows.Count > 0)
                {
                    objreturndbmlRFQBookingDetail.objdbmlTrackBookingTimeDetail = new ObservableCollection<dbmlTrackBookingTimeDetail>
                                                                    (from dRow in ds.Tables["TrackBookingTimeDetail"].AsEnumerable() select (ConvertTableToListNew<dbmlTrackBookingTimeDetail>(dRow)));
                }

                cmdGet = db.GetStoredProcCommand("[Front].[LabBookingDetailViewFrontGetByBookingId]", intBookingId);
                db.LoadDataSet(cmdGet, ds, new string[] { "LabBookingDetail" });
                if (ds.Tables["LabBookingDetail"].Rows.Count > 0)
                {
                    objreturndbmlRFQBookingDetail.objdbmlLabBookingDetailViewFront = new ObservableCollection<dbmlLabBookingDetailViewFront>(from dRow in ds.Tables["LabBookingDetail"].AsEnumerable()
                                                                                                                                             select (ConvertTableToListNew<dbmlLabBookingDetailViewFront>(dRow)));
                }

                cmdGet = db.GetStoredProcCommand("[Front].[WorkshopBookingDetailViewFrontGetByBookingId]", intBookingId);
                db.LoadDataSet(cmdGet, ds, new string[] { "WorkshopBooking" });
                if (ds.Tables["WorkshopBooking"].Rows.Count > 0)
                {
                    objreturndbmlRFQBookingDetail.objdbmlWorkshopBookingDetailViewFront = new ObservableCollection<dbmlWorkshopBookingDetailViewFront>(from dRow in ds.Tables["WorkshopBooking"].AsEnumerable()
                                                                                                                                                       select (ConvertTableToListNew<dbmlWorkshopBookingDetailViewFront>(dRow)));
                }

                cmdGet = db.GetStoredProcCommand("[Front].[BookingDetailAddOnServicesViewFrontGetByBookingId]", intBookingId);
                db.LoadDataSet(cmdGet, ds, new string[] { "BookingDetailAddOnServices" });
                if (ds.Tables["BookingDetailAddOnServices"].Rows.Count > 0)
                {
                    objreturndbmlRFQBookingDetail.objdbmlBookingDetailAddOnServicesViewFront = new ObservableCollection<dbmlBookingDetailAddOnServicesViewFront>(from dRow in ds.Tables["BookingDetailAddOnServices"].AsEnumerable()
                                                                                                                                                                 select (ConvertTableToListNew<dbmlBookingDetailAddOnServicesViewFront>(dRow)));
                }

                objreturndbmlRFQBookingDetail.objdbmlStatus.StatusId = 1;
                objreturndbmlRFQBookingDetail.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlRFQBookingDetail.objdbmlStatus.StatusId = 99;
                objreturndbmlRFQBookingDetail.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlRFQBookingDetail;
        }

        public returndbmlBooking RFQBookingDetailInsertByBookingIdBPId(int intRFQBookingId, int intRFQBPId, int intBPId,int intUserId,int intCompanyId)
        {
            returndbmlBooking objreturndbmlBooking = new returndbmlBooking();
            returndbmlRFQBookingDetail objreturndbmlRFQBookingDetail = RFQBookingDetailGetByBookingIdBPId(intRFQBookingId, intRFQBPId);
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intBookingId = 0;
                ///////////////////////////// BookingInsert ///////////////////////////////////////
                foreach (var Header in objreturndbmlRFQBookingDetail.objdbmlBookingView)
                {
                    Header.BookingId = -1;
                    Header.BookingNo = "Temp";
                    Header.BookingDate = DateTime.Now;
                    Header.RFQId = intRFQBookingId;
                    Header.BPId = intBPId;
                    Header.CompanyId = intCompanyId;
                    Header.StatusPropId = 40;
                    Header.CreateId = intUserId;
                    Header.CreateDate = DateTime.Now;
                    Header.UpdateId = intUserId;
                    Header.UpdateDate = DateTime.Now;

                    cmd = db.GetStoredProcCommand("[Transaction].[BookingInsert]");
                    foreach (PropertyInfo PropInfoCol in Header.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(Header, null));
                        }
                    }

                    db.AddOutParameter(cmd, "IdOut", DbType.Int32, 0);
                    db.ExecuteNonQuery(cmd, trans);
                    intBookingId = (int)db.GetParameterValue(cmd, "@IdOut");
                }
                if (intBookingId > 0)
                {
                    ///////////////////////////// ListOfVehicleComponentInsert ///////////////////////////////////////
                    foreach (var itm in objreturndbmlRFQBookingDetail.objdbmlListOfVehicleComponent)
                    {
                        int intVehCompId = itm.VehCompId;
                        itm.VehCompId = -1;
                        itm.DocId = intBookingId;
                        itm.BPId = intBPId;
                        itm.CreateId = intUserId;
                        itm.CreateDate = DateTime.Now;
                        itm.UpdateId = intUserId;
                        itm.UpdateDate = DateTime.Now;
                        cmd = db.GetStoredProcCommand("[Transaction].[ListOfVehicleComponentInsert]");
                        foreach (PropertyInfo PropInfoCol in itm.GetType().GetProperties())
                        {
                            string str = PropInfoCol.Name;
                            if (str.Length <= 2)
                                str = str + "modified";
                            if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                            {
                                DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                                db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(itm, null));
                            }
                        }

                        db.AddOutParameter(cmd, "IdOut", DbType.Int32, 0);
                        db.ExecuteNonQuery(cmd, trans);
                        int intIdOut = (int)db.GetParameterValue(cmd, "@IdOut");

                        if(objreturndbmlRFQBookingDetail.objdbmlTrackBookingDetail!=null && 
                            objreturndbmlRFQBookingDetail.objdbmlTrackBookingDetail.Where(x => x.VehicleId == intVehCompId)!=null &&
                            objreturndbmlRFQBookingDetail.objdbmlTrackBookingDetail.Where(x => x.VehicleId == intVehCompId).Count()>0)
                        {
                            objreturndbmlRFQBookingDetail.objdbmlTrackBookingDetail.Where(x => x.VehicleId == intVehCompId).ToList().ForEach(s => s.VehicleId = intIdOut);
                        }
                        if (objreturndbmlRFQBookingDetail.objdbmlTrackBookingTimeDetail != null &&
                           objreturndbmlRFQBookingDetail.objdbmlTrackBookingTimeDetail.Where(x => x.VehicleId == intVehCompId) != null &&
                           objreturndbmlRFQBookingDetail.objdbmlTrackBookingTimeDetail.Where(x => x.VehicleId == intVehCompId).Count() > 0)
                        {
                            objreturndbmlRFQBookingDetail.objdbmlTrackBookingTimeDetail.Where(x => x.VehicleId == intVehCompId).ToList().ForEach(s => s.VehicleId = intIdOut);
                        }
                        if (objreturndbmlRFQBookingDetail.objdbmlLabBookingDetailViewFront != null &&
                           objreturndbmlRFQBookingDetail.objdbmlLabBookingDetailViewFront.Where(x => x.VehCompId == intVehCompId) != null &&
                           objreturndbmlRFQBookingDetail.objdbmlLabBookingDetailViewFront.Where(x => x.VehCompId == intVehCompId).Count() > 0)
                        {
                            objreturndbmlRFQBookingDetail.objdbmlLabBookingDetailViewFront.Where(x => x.VehCompId == intVehCompId).ToList().ForEach(s => s.VehCompId = intIdOut);
                        }

                      
                        

                    }

                    ///////////////////////////// TrackBookingTimeDetailInsertFront ///////////////////////////////////////

                    foreach (var itmBD in objreturndbmlRFQBookingDetail.objdbmlTrackBookingDetail)
                    {
                        ObservableCollection<dbmlTrackBookingTimeDetail> TrackDetailTimeList = new ObservableCollection<dbmlTrackBookingTimeDetail>(objreturndbmlRFQBookingDetail.objdbmlTrackBookingTimeDetail.Where(x => x.BookingDetailId == itmBD.TrackBookingDetailId));

                        int intTrackGroupId = (int)itmBD.TrackGroupId;
                        int intVehicleId = (int)itmBD.VehicleId;
                        int intCategoryId = (int)itmBD.CategoryPropId;
                        decimal decRate = Math.Round(Convert.ToDecimal(Convert.ToDecimal(itmBD.QuotAmount) / Convert.ToDecimal(itmBD.BillingHrs)), 2);                      

                        foreach (var itm in TrackDetailTimeList)
                        {
                            itm.TrackBookingTimeDetailId = -1;
                            itm.BookingId = intBookingId;
                            itm.BookingDetailId = 0;
                            decimal decAmount = Math.Round(Convert.ToDecimal(decRate * itm.BillingHrs), 2);
                            itm.Rate = decRate;
                            itm.Amount = decAmount;
                            itm.CreateId = intUserId;
                            itm.CreateDate = DateTime.Now;
                            itm.UpdateId = intUserId;
                            itm.UpdateDate = DateTime.Now;

                            cmd = db.GetStoredProcCommand("[Front].[TrackBookingTimeDetailInsertFront]");
                            foreach (PropertyInfo PropInfoCol in itm.GetType().GetProperties())
                            {
                                string str = PropInfoCol.Name;
                                if (str.Length <= 2)
                                    str = str + "modified";
                                if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                                {
                                    DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                                    db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(itm, null));
                                }
                            }

                            db.ExecuteNonQuery(cmd, trans);
                        }

                        cmd = db.GetStoredProcCommand("[Front].[TrackBookingDetailInsertFront]", intBookingId, intTrackGroupId, intVehicleId, intCategoryId);
                        db.ExecuteNonQuery(cmd, trans);
                    }

                    ///////////////////////////// LabBookingDetailInsertFront ///////////////////////////////////////
                    foreach (var itm in objreturndbmlRFQBookingDetail.objdbmlLabBookingDetailViewFront)
                    {
                        itm.LabBookingDetailId = -1;
                        itm.BookingId = intBookingId;
                        decimal decRate = Math.Round(Convert.ToDecimal(itm.QuotAmount / itm.UsageHours), 2);
                        decimal decAmount = Math.Round(Convert.ToDecimal(decRate * itm.UsageHours), 2);
                        itm.Rate = decRate;
                        itm.Amount = decAmount;
                        itm.CreateId = intUserId;
                        itm.CreateDate = DateTime.Now;
                        itm.UpdateId = intUserId;
                        itm.UpdateDate = DateTime.Now;

                        cmd = db.GetStoredProcCommand("[Front].[LabBookingDetailInsertFront]");
                        foreach (PropertyInfo PropInfoCol in itm.GetType().GetProperties())
                        {
                            string str = PropInfoCol.Name;
                            if (str.Length <= 2)
                                str = str + "modified";
                            if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                            {
                                DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                                db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(itm, null));
                            }
                        }

                        db.ExecuteNonQuery(cmd, trans);
                    }

                    ///////////////////////////// WorkshopBookingDetailInsertFront ///////////////////////////////////////
                    foreach (var itm in objreturndbmlRFQBookingDetail.objdbmlWorkshopBookingDetailViewFront)
                    {
                        itm.WorkshopBookingDetailId = -1;
                        itm.BookingId = intBookingId;
                        decimal decRate = Math.Round(Convert.ToDecimal(itm.QuotAmount / itm.UsageHours), 2);
                        decimal decAmount = Math.Round(Convert.ToDecimal(decRate * itm.UsageHours), 2);
                        itm.Rate = decRate;
                        itm.Amount = decAmount;
                        itm.CreateId = intUserId;
                        itm.CreateDate = DateTime.Now;
                        itm.UpdateId = intUserId;
                        itm.UpdateDate = DateTime.Now;

                        cmd = db.GetStoredProcCommand("[Front].[WorkshopBookingDetailInsertFront]");
                        foreach (PropertyInfo PropInfoCol in itm.GetType().GetProperties())
                        {
                            string str = PropInfoCol.Name;
                            if (str.Length <= 2)
                                str = str + "modified";
                            if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                            {
                                DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                                db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(itm, null));
                            }
                        }

                        db.ExecuteNonQuery(cmd, trans);
                    }

                    ///////////////////////////// BookingDetailAddOnServicesInsertFront ///////////////////////////////////////
                    foreach (var itm in objreturndbmlRFQBookingDetail.objdbmlBookingDetailAddOnServicesViewFront)
                    {
                        itm.BookingAddOnServiceId = -1;
                        itm.BookingId = intBookingId;
                        decimal decRate = Math.Round(Convert.ToDecimal(itm.QuotAmount / itm.Consumption), 2);
                        decimal decAmount = Math.Round(Convert.ToDecimal(decRate * itm.Consumption), 2);
                        itm.Rate = decRate;
                        itm.Amount = decAmount;
                        itm.CreateId = intUserId;
                        itm.CreateDate = DateTime.Now;
                        itm.UpdateId = intUserId;
                        itm.UpdateDate = DateTime.Now;

                        cmd = db.GetStoredProcCommand("[Front].[BookingDetailAddOnServicesInsertFront]");
                        foreach (PropertyInfo PropInfoCol in itm.GetType().GetProperties())
                        {
                            string str = PropInfoCol.Name;
                            if (str.Length <= 2)
                                str = str + "modified";
                            if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                            {
                                DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                                db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(itm, null));
                            }
                        }

                        db.ExecuteNonQuery(cmd, trans);

                        cmd = db.GetStoredProcCommand("[Front].[UpdateTabStatusToBookingByBookingId]", intBookingId, 70);
                        db.ExecuteNonQuery(cmd, trans);
                    }

                    cmd = db.GetStoredProcCommand("[Front].UpdateRFQBookingByBookingId", intRFQBookingId, intBookingId);
                    db.ExecuteNonQuery(cmd, trans);

                    trans.Commit();

                    objreturndbmlBooking = BookingViewGetByBookingId(intBookingId);
                }
                else
                {
                    objreturndbmlBooking.objdbmlStatus.StatusId = 99;
                    objreturndbmlBooking.objdbmlStatus.Status = "Booking not Created";
                    trans.Rollback();
                }
            }
            catch (Exception ex)
            {
                objreturndbmlBooking.objdbmlStatus.StatusId = 99;
                objreturndbmlBooking.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlBooking;
        }

        public returndbmlServiceDateViewFront ServiceDateViewFrontGetByBookingId(int intBookingId)
        {
            returndbmlServiceDateViewFront objreturndbmlServiceDateViewFront = new returndbmlServiceDateViewFront();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Front].ServiceDateViewFrontGetByBookingId", intBookingId);
                db.LoadDataSet(cmdGet, ds, new string[] { "ServiceDateView" });
                if (ds.Tables["ServiceDateView"].Rows.Count > 0)
                {
                    objreturndbmlServiceDateViewFront.objdbmlServiceDateViewFront = new ObservableCollection<dbmlServiceDateViewFront>(from dRow in ds.Tables["ServiceDateView"].AsEnumerable()
                                                                                                                                       select (ConvertTableToListNew<dbmlServiceDateViewFront>(dRow)));
                }

                objreturndbmlServiceDateViewFront.objdbmlStatus.StatusId = 1;
                objreturndbmlServiceDateViewFront.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlServiceDateViewFront.objdbmlStatus.StatusId = 99;
                objreturndbmlServiceDateViewFront.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlServiceDateViewFront;
        }

        public returndbmlBooking UpdateServiceDateFrontByBookingIdDayDates(returndbmlServiceDateViewFront objreturndbmlServiceDateViewFront)
        {
            returndbmlBooking returndbmlBookingReturn = new returndbmlBooking();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intBookingId = (int) objreturndbmlServiceDateViewFront.objdbmlServiceDateViewFront.FirstOrDefault().BookingId;
                foreach (var Header in objreturndbmlServiceDateViewFront.objdbmlServiceDateViewFront)
                {
                    cmd = db.GetStoredProcCommand("[Front].UpdateServiceDateFrontByBookingIdDayDates");
                    foreach (PropertyInfo PropInfoCol in Header.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(Header, null));
                        }
                    }
                    db.ExecuteNonQuery(cmd, trans);

                }

                cmd = db.GetStoredProcCommand("[Front].[GenerateTrackBookingDetailByBookingIdforRFQFront]", intBookingId);
                db.ExecuteNonQuery(cmd, trans);

                trans.Commit();


                returndbmlBookingReturn = BookingViewGetByBookingId(intBookingId);
            }
            catch (Exception ex)
            {
                returndbmlBookingReturn.objdbmlStatus.StatusId = 99;
                returndbmlBookingReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return returndbmlBookingReturn;
        }

        public returndbmlBookingSearchView ToDoBookingSearchViewGetByCompanyIdFromDateToDateFront(int intCompanyId, int intUserId, DateTime dtFromDate, DateTime dtToDate, int intBPId, int intStatusPropId)
        {
            returndbmlBookingSearchView objreturndbmlBookingSearchView = new returndbmlBookingSearchView();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Front].[ToDoBookingSearchViewFrontGetByCompanyIdFromDateToDate]", intCompanyId, intUserId, dtFromDate, dtToDate, intBPId, intStatusPropId);
                db.LoadDataSet(cmdGet, ds, new string[] { "Booking" });
                if (ds.Tables["Booking"] != null && ds.Tables["Booking"].Rows.Count > 0)
                {
                    objreturndbmlBookingSearchView.objdbmlBookingSearchView = new ObservableCollection<dbmlBookingSearchView>(from dRow in ds.Tables["Booking"].AsEnumerable()
                                                                                                                              select (ConvertTableToListNew<dbmlBookingSearchView>(dRow)));
                }

                objreturndbmlBookingSearchView.objdbmlStatus.StatusId = 1;
                objreturndbmlBookingSearchView.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlBookingSearchView.objdbmlStatus.StatusId = 99;
                objreturndbmlBookingSearchView.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlBookingSearchView;
        }

        public returndbmlDashBoardDocumentViewFront DashBoardDocumentGetByBPIdWorkFlowIdStatusPropertyId(int intBPId, string strWorkflowId, string strStatusPropId,int intUserId)
        {
            returndbmlDashBoardDocumentViewFront objreturndbmlDashBoardDocumentViewFront = new returndbmlDashBoardDocumentViewFront();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Transaction].[DashBoardDocumentGetByBPIdWorkFlowIdStatusPropertyId]", intBPId, strWorkflowId, strStatusPropId, intUserId);
                db.LoadDataSet(cmdGet, ds, new string[] { "DashBoardDocument" });
                if (ds.Tables["DashBoardDocument"] != null && ds.Tables["DashBoardDocument"].Rows.Count > 0)
                {
                    objreturndbmlDashBoardDocumentViewFront.objdbmlDashBoardDocumentViewFront = new ObservableCollection<dbmlDashBoardDocumentViewFront>(from dRow in ds.Tables["DashBoardDocument"].AsEnumerable()
                                                                                                                              select (ConvertTableToListNew<dbmlDashBoardDocumentViewFront>(dRow)));
                }

                objreturndbmlDashBoardDocumentViewFront.objdbmlStatus.StatusId = 1;
                objreturndbmlDashBoardDocumentViewFront.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlDashBoardDocumentViewFront.objdbmlStatus.StatusId = 99;
                objreturndbmlDashBoardDocumentViewFront.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlDashBoardDocumentViewFront;
        }
        #endregion

        #region Vehicle Componants
        public returndbmlListOfVehicleComponent ListOfVehicleComponentInsert(returndbmlListOfVehicleComponent objreturndbmlListOfVehicleComponent)
        {
            returndbmlListOfVehicleComponent objreturndbmlListOfVehicleComponentReturn = new returndbmlListOfVehicleComponent();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intDocId = 0;
                foreach (var itm in objreturndbmlListOfVehicleComponent.objdbmlListOfVehicleComponent)
                {
                    intDocId = (int)itm.DocId;
                    cmd = db.GetStoredProcCommand("[Transaction].[ListOfVehicleComponentInsert]");
                    foreach (PropertyInfo PropInfoCol in itm.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(itm, null));
                        }
                    }

                    db.AddOutParameter(cmd, "IdOut", DbType.Int32, 0);
                    db.ExecuteNonQuery(cmd, trans);
                    int intIdOut = (int)db.GetParameterValue(cmd, "@IdOut");

                    cmd = db.GetStoredProcCommand("[Front].[UpdateTabStatusToBookingByBookingId]", intDocId, 40);
                    db.ExecuteNonQuery(cmd, trans);
                }

                trans.Commit();


                objreturndbmlListOfVehicleComponentReturn = ListOfVehicleComponentGetByDocId(intDocId);
            }
            catch (Exception ex)
            {
                objreturndbmlListOfVehicleComponentReturn.objdbmlStatus.StatusId = 99;
                objreturndbmlListOfVehicleComponentReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlListOfVehicleComponentReturn;
        }

        public returndbmlListOfVehicleComponent ListOfVehicleComponentUpdate(returndbmlListOfVehicleComponent objreturndbmlListOfVehicleComponent)
        {
            returndbmlListOfVehicleComponent objreturndbmlListOfVehicleComponentReturn = new returndbmlListOfVehicleComponent();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intDocId = 0;
                foreach (var itm in objreturndbmlListOfVehicleComponent.objdbmlListOfVehicleComponent)
                {
                    intDocId = (int)itm.DocId;
                    cmd = db.GetStoredProcCommand("[Transaction].[ListOfVehicleComponentUpdate]");
                    foreach (PropertyInfo PropInfoCol in itm.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(itm, null));
                        }
                    }

                    db.ExecuteNonQuery(cmd, trans);

                }

                trans.Commit();


                objreturndbmlListOfVehicleComponentReturn = ListOfVehicleComponentGetByDocId(intDocId);
            }
            catch (Exception ex)
            {
                objreturndbmlListOfVehicleComponentReturn.objdbmlStatus.StatusId = 99;
                objreturndbmlListOfVehicleComponentReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlListOfVehicleComponentReturn;
        }

        public returndbmlListOfVehicleComponent ListOfVehicleComponentDeleteByDocIdCompId(int intDocId, int intVehCompId)
        {
            returndbmlListOfVehicleComponent objreturndbmlListOfVehicleComponent = new returndbmlListOfVehicleComponent();
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("[Transaction].[ListOfVehicleComponentDeleteByDocIdCompId]");
                db.AddInParameter(cmd, "@DocId", DbType.Int32, intDocId);
                db.AddInParameter(cmd, "@VehCompId", DbType.Int32, intVehCompId);
                db.AddOutParameter(cmd, "@IDOut", DbType.Int32, 0);

                db.ExecuteNonQuery(cmd, trans);
                int intIdOut = (int)db.GetParameterValue(cmd, "@IDOut");             
                if(intIdOut>=0)
                {
                    trans.Commit();
                    objreturndbmlListOfVehicleComponent.objdbmlStatus.StatusId = 1;
                    objreturndbmlListOfVehicleComponent.objdbmlStatus.Status = "Successful";

                    objreturndbmlListOfVehicleComponent = ListOfVehicleComponentGetByDocId(intDocId);
                }
                else
                {
                    trans.Rollback();
                    objreturndbmlListOfVehicleComponent.objdbmlStatus.StatusId = 10;
                    if(intIdOut==-1)
                    {
                        objreturndbmlListOfVehicleComponent.objdbmlStatus.Status = "Vehicle details can not be deleted, because it is used by 'Track Booking Details'";
                    }
                    else if (intIdOut == -2)
                    {
                        objreturndbmlListOfVehicleComponent.objdbmlStatus.Status = "Vehicle/Component details can not be deleted, because it is used by 'Lab Booking Details'";
                    }

                }                 
            }
            catch (Exception ex)
            {
                objreturndbmlListOfVehicleComponent.objdbmlStatus.StatusId = 99;
                objreturndbmlListOfVehicleComponent.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlListOfVehicleComponent;
        }

        public returndbmlListOfVehicleComponent ListOfVehicleComponentGetByDocId(int intDocId)
        {
            returndbmlListOfVehicleComponent objreturndbmlListOfVehicleComponent = new returndbmlListOfVehicleComponent();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Transaction].[ListOfVehicleComponentGetByDocId]", intDocId);
                db.LoadDataSet(cmdGet, ds, new string[] { "Vehicle" });
                if (ds.Tables["Vehicle"].Rows.Count > 0)
                {
                    objreturndbmlListOfVehicleComponent.objdbmlListOfVehicleComponent = new ObservableCollection<dbmlListOfVehicleComponent>(from dRow in ds.Tables["Vehicle"].AsEnumerable() select (ConvertTableToListNew<dbmlListOfVehicleComponent>(dRow)));
                }

                objreturndbmlListOfVehicleComponent.objdbmlStatus.StatusId = 1;
                objreturndbmlListOfVehicleComponent.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlListOfVehicleComponent.objdbmlStatus.StatusId = 99;
                objreturndbmlListOfVehicleComponent.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlListOfVehicleComponent;
        }
        #endregion

        #region Driver

        #endregion

        #region Attendee

        #endregion

        #region Tracks/Services
        public returndbmlServicesView ServicesGetByBPId(int intBPId)
        {
            returndbmlServicesView objreturndbmlServicesView = new returndbmlServicesView();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Master].[ServicesGetByBPId]", intBPId);
                db.LoadDataSet(cmdGet, ds, new string[] { "ServicesView" });
                if (ds.Tables["ServicesView"] != null && ds.Tables["ServicesView"].Rows.Count > 0)
                {
                    objreturndbmlServicesView.objdbmlServicesView = new ObservableCollection<dbmlServicesView>
                                                                    (from dRow in ds.Tables["ServicesView"].AsEnumerable() select (ConvertTableToListNew<dbmlServicesView>(dRow)));
                }

                objreturndbmlServicesView.objdbmlStatus.StatusId = 1;
                objreturndbmlServicesView.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlServicesView.objdbmlStatus.StatusId = 99;
                objreturndbmlServicesView.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlServicesView;
        }

        public returndbmlTrackBookingDetail TrackBookingDetailGetByBookingIdTrackGroupId(int intBookingId, int intTrackGroupId)
        {
            returndbmlTrackBookingDetail objreturndbmlTrackBookingDetail = new returndbmlTrackBookingDetail();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Front].[TrackBookingDetailViewFrontGetByBookingIdTrackGroupId]", intBookingId, intTrackGroupId);
                db.LoadDataSet(cmdGet, ds, new string[] { "TrackBookingDetail" });
                if (ds.Tables["TrackBookingDetail"] != null && ds.Tables["TrackBookingDetail"].Rows.Count > 0)
                {
                    objreturndbmlTrackBookingDetail.objdbmlTrackBookingDetail = new ObservableCollection<dbmlTrackBookingDetail>
                                                                    (from dRow in ds.Tables["TrackBookingDetail"].AsEnumerable() select (ConvertTableToListNew<dbmlTrackBookingDetail>(dRow)));
                }

                cmdGet = db.GetStoredProcCommand("[Front].[TrackBookingTimeDetailIdViewFrontGetByBookingIdTrackGroupId]", intBookingId, intTrackGroupId);
                db.LoadDataSet(cmdGet, ds, new string[] { "TrackBookingTimeDetail" });
                if (ds.Tables["TrackBookingTimeDetail"] != null && ds.Tables["TrackBookingTimeDetail"].Rows.Count > 0)
                {
                    objreturndbmlTrackBookingDetail.objdbmlTrackBookingTimeDetail = new ObservableCollection<dbmlTrackBookingTimeDetail>
                                                                    (from dRow in ds.Tables["TrackBookingTimeDetail"].AsEnumerable() select (ConvertTableToListNew<dbmlTrackBookingTimeDetail>(dRow)));
                }

                cmdGet = db.GetStoredProcCommand("[Front].[TrackBookingTimeSummaryViewFrontGetByBookingId]", intBookingId);
                db.LoadDataSet(cmdGet, ds, new string[] { "TrackBookingTimeSummary" });
                if (ds.Tables["TrackBookingTimeSummary"] != null && ds.Tables["TrackBookingTimeSummary"].Rows.Count > 0)
                {
                    objreturndbmlTrackBookingDetail.objdbmlTrackBookingTimeSummary = new ObservableCollection<dbmlTrackBookingTimeSummary>
                                                                    (from dRow in ds.Tables["TrackBookingTimeSummary"].AsEnumerable() select (ConvertTableToListNew<dbmlTrackBookingTimeSummary>(dRow)));
                }

                objreturndbmlTrackBookingDetail.objdbmlStatus.StatusId = 1;
                objreturndbmlTrackBookingDetail.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlTrackBookingDetail.objdbmlStatus.StatusId = 99;
                objreturndbmlTrackBookingDetail.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlTrackBookingDetail;
        }

        public returndbmlBookingStatusTimeSlotView BookingStatusGetByServiceIdTimeSlotPropIdWEFDate(ObservableCollection<int> intlstServiceId, int intTimeSlotId, DateTime dtWED)
        {
            returndbmlBookingStatusTimeSlotView objreturndbmlBookingStatusTimeSlotView = new returndbmlBookingStatusTimeSlotView();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                foreach (var intServiceId in intlstServiceId)
                {
                    cmdGet = db.GetStoredProcCommand("[Front].[BookingStatusFrontGetByServiceIdTimeSlotPropIdWEFDate]", intServiceId, intTimeSlotId, dtWED);
                    db.LoadDataSet(cmdGet, ds, new string[] { "BookingStatus" });
                }

                if (ds.Tables["BookingStatus"] != null && ds.Tables["BookingStatus"].Rows.Count > 0)
                {
                    objreturndbmlBookingStatusTimeSlotView.objdbmlBookingStatusTimeSlotView = new ObservableCollection<dbmlBookingStatusTimeSlotView>
                                                                    (from dRow in ds.Tables["BookingStatus"].AsEnumerable() select (ConvertTableToListNew<dbmlBookingStatusTimeSlotView>(dRow)));
                }

                objreturndbmlBookingStatusTimeSlotView.objdbmlStatus.StatusId = 1;
                objreturndbmlBookingStatusTimeSlotView.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlBookingStatusTimeSlotView.objdbmlStatus.StatusId = 99;
                objreturndbmlBookingStatusTimeSlotView.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlBookingStatusTimeSlotView;
        }

        public returndbmlTrackBookingDetail TrackBookingDetailInsertFront(returndbmlTrackBookingDetail objreturndbmlTrackBookingDetail)
        {
            returndbmlTrackBookingDetail objreturndbmlTrackBookingDetailReturn = new returndbmlTrackBookingDetail();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intBookingId = (int)objreturndbmlTrackBookingDetail.objdbmlTrackBookingTimeDetail.FirstOrDefault().BookingId;
                int intTrackGroupId = (int)objreturndbmlTrackBookingDetail.objdbmlTrackBookingTimeDetail.FirstOrDefault().ZZTrackGroupId;
                int intVehicleId = (int)objreturndbmlTrackBookingDetail.objdbmlTrackBookingTimeDetail.FirstOrDefault().VehicleId;
                DateTime dtDate = Convert.ToDateTime(objreturndbmlTrackBookingDetail.objdbmlTrackBookingTimeDetail.FirstOrDefault().Date);
                int intServiceId = (int)objreturndbmlTrackBookingDetail.objdbmlTrackBookingTimeDetail.FirstOrDefault().ServiceId;
                int intTimeSlotId = (int)objreturndbmlTrackBookingDetail.objdbmlTrackBookingTimeDetail.FirstOrDefault().SlotPropId;
                int intCategoryId = (int)objreturndbmlTrackBookingDetail.objdbmlTrackBookingTimeDetail.FirstOrDefault().ZZCategoryId;

                cmd = db.GetStoredProcCommand("[Front].[TrackBookingTimeDetailDeleteFront]", intBookingId, intTrackGroupId, intVehicleId, dtDate, intServiceId, intTimeSlotId);
                db.ExecuteNonQuery(cmd, trans);

                foreach (var itm in objreturndbmlTrackBookingDetail.objdbmlTrackBookingTimeDetail)
                {

                    cmd = db.GetStoredProcCommand("[Front].[TrackBookingTimeDetailInsertFront]");
                    foreach (PropertyInfo PropInfoCol in itm.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(itm, null));
                        }
                    }

                    // db.AddOutParameter(cmd, "IdOut", DbType.Int32, 0);
                    db.ExecuteNonQuery(cmd, trans);
                    // int intIdOut = (int)db.GetParameterValue(cmd, "@IdOut");

                }

                cmd = db.GetStoredProcCommand("[Front].[TrackBookingDetailInsertFront]", intBookingId, intTrackGroupId, intVehicleId, intCategoryId);
                db.ExecuteNonQuery(cmd, trans);

                cmd = db.GetStoredProcCommand("[Front].[UpdateTabStatusToBookingByBookingId]", intBookingId, 40);
                db.ExecuteNonQuery(cmd, trans);

                trans.Commit();


                objreturndbmlTrackBookingDetailReturn = TrackBookingDetailGetByBookingIdTrackGroupId(intBookingId, intTrackGroupId);
            }
            catch (Exception ex)
            {
                objreturndbmlTrackBookingDetailReturn.objdbmlStatus.StatusId = 99;
                objreturndbmlTrackBookingDetailReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlTrackBookingDetailReturn;
        }

        public returndbmlTrackBookingDetail TrackBookingTimeDetailDeleteFrontByServiceId(int intBookingId, int intTrackGroupId, int intVehicleId, DateTime dtDate, int intServiceId, int intTimeSlotId)
        {
            returndbmlTrackBookingDetail objreturndbmlTrackBookingDetailReturn = new returndbmlTrackBookingDetail();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("[Front].[TrackBookingTimeDetailDeleteFrontByServiceId]", intBookingId, intTrackGroupId, intVehicleId, dtDate, intServiceId, intTimeSlotId);
                db.ExecuteNonQuery(cmd, trans);

                trans.Commit();


                objreturndbmlTrackBookingDetailReturn = TrackBookingDetailGetByBookingIdTrackGroupId(intBookingId, intTrackGroupId);
            }
            catch (Exception ex)
            {
                objreturndbmlTrackBookingDetailReturn.objdbmlStatus.StatusId = 99;
                objreturndbmlTrackBookingDetailReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlTrackBookingDetailReturn;
        }


        #endregion

        #region WorkFlow Activity
        public returndbmlWorkFlowView WorkFlowViewGetByBPId(int intBPId, int intDocId)
        {
            returndbmlWorkFlowView objreturndbmlWorkFlowView = new returndbmlWorkFlowView();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Security].[WorkFlowViewGetByBPIdDocId]", intBPId, intDocId);
                db.LoadDataSet(cmdGet, ds, new string[] { "WorkFlowView" });
                if (ds.Tables["WorkFlowView"] != null && ds.Tables["WorkFlowView"].Rows.Count > 0)
                {
                    objreturndbmlWorkFlowView.objdbmlWorkFlowView = new ObservableCollection<dbmlWorkFlowView>
                                                                    (from dRow in ds.Tables["WorkFlowView"].AsEnumerable() select (ConvertTableToListNew<dbmlWorkFlowView>(dRow)));
                }

                objreturndbmlWorkFlowView.objdbmlStatus.StatusId = 1;
                objreturndbmlWorkFlowView.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlWorkFlowView.objdbmlStatus.StatusId = 99;
                objreturndbmlWorkFlowView.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlWorkFlowView;
        }

        public returndbmlBooking WorkFlowActivityInsert(int intDocId, int intBPId, int intWorkPlowId, int intStatusId, string strRemark, int intCreateId)
        {
            returndbmlBooking objreturndbmlBooking = new returndbmlBooking();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("[Front].WorkFlowActivityInsert", intDocId, intBPId, intWorkPlowId, intStatusId, strRemark, intCreateId);
                db.ExecuteNonQuery(cmd, trans);

                trans.Commit();

                objreturndbmlBooking = BookingViewGetByBookingId(intDocId);
            }
            catch (Exception ex)
            {
                objreturndbmlBooking.objdbmlStatus.StatusId = 99;
                objreturndbmlBooking.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlBooking;
        }

        public returndbmlWorkFlowActivityTrackView WorkFlowActivityTrackGetByBPIdDocId(int intBPId, int intDocId)
        {
            returndbmlWorkFlowActivityTrackView objreturndbmlWorkFlowActivityTrackView = new returndbmlWorkFlowActivityTrackView();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Front].[WorkFlowActivityTrackGetByBPIdDocId]", intBPId, intDocId);
                db.LoadDataSet(cmdGet, ds, new string[] { "WorkFlowView" });
                if (ds.Tables["WorkFlowView"] != null && ds.Tables["WorkFlowView"].Rows.Count > 0)
                {
                    objreturndbmlWorkFlowActivityTrackView.objdbmlWorkFlowActivityTrackView = new ObservableCollection<dbmlWorkFlowActivityTrackView>
                                                                    (from dRow in ds.Tables["WorkFlowView"].AsEnumerable() select (ConvertTableToListNew<dbmlWorkFlowActivityTrackView>(dRow)));
                }

                objreturndbmlWorkFlowActivityTrackView.objdbmlStatus.StatusId = 1;
                objreturndbmlWorkFlowActivityTrackView.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlWorkFlowActivityTrackView.objdbmlStatus.StatusId = 99;
                objreturndbmlWorkFlowActivityTrackView.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlWorkFlowActivityTrackView;
        }
        #endregion

        #region Workshop Booking Detail
        public returndbmlWorkshopBookingDetailViewFront WorkshopBookingDetailInsertFront(returndbmlWorkshopBookingDetailViewFront objreturndbmlWorkshopBookingDetailViewFront)
        {
            returndbmlWorkshopBookingDetailViewFront objreturndbmlWorkshopBookingDetailViewFrontReturn = new returndbmlWorkshopBookingDetailViewFront();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intDocId = 0;
                foreach (var itm in objreturndbmlWorkshopBookingDetailViewFront.objdbmlWorkshopBookingDetailViewFront)
                {
                    intDocId = (int)itm.BookingId;
                    cmd = db.GetStoredProcCommand("[Front].[WorkshopBookingDetailInsertFront]");
                    foreach (PropertyInfo PropInfoCol in itm.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(itm, null));
                        }
                    }

                    db.ExecuteNonQuery(cmd, trans);

                    cmd = db.GetStoredProcCommand("[Front].[UpdateTabStatusToBookingByBookingId]", intDocId, 50);
                    db.ExecuteNonQuery(cmd, trans);
                }

                trans.Commit();


                objreturndbmlWorkshopBookingDetailViewFrontReturn = WorkshopBookingDetailViewFrontGetByBookingId(intDocId);
            }
            catch (Exception ex)
            {
                objreturndbmlWorkshopBookingDetailViewFrontReturn.objdbmlStatus.StatusId = 99;
                objreturndbmlWorkshopBookingDetailViewFrontReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlWorkshopBookingDetailViewFrontReturn;
        }

        public returndbmlWorkshopBookingDetailViewFront WorkshopBookingDetailDelete(int intDocId, int intWorkshopBookingDetailId)
        {
            returndbmlWorkshopBookingDetailViewFront objreturndbmlWorkshopBookingDetailViewFront = new returndbmlWorkshopBookingDetailViewFront();
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("[Transaction].[WorkshopBookingDetailDelete]", intWorkshopBookingDetailId);
                db.ExecuteNonQuery(cmd, trans);

                trans.Commit();
                objreturndbmlWorkshopBookingDetailViewFront.objdbmlStatus.StatusId = 1;
                objreturndbmlWorkshopBookingDetailViewFront.objdbmlStatus.Status = "Successful";

                objreturndbmlWorkshopBookingDetailViewFront = WorkshopBookingDetailViewFrontGetByBookingId(intDocId);
            }
            catch (Exception ex)
            {
                objreturndbmlWorkshopBookingDetailViewFront.objdbmlStatus.StatusId = 99;
                objreturndbmlWorkshopBookingDetailViewFront.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlWorkshopBookingDetailViewFront;
        }

        public returndbmlWorkshopBookingDetailViewFront WorkshopBookingDetailViewFrontGetByBookingId(int intDocId)
        {
            returndbmlWorkshopBookingDetailViewFront objreturndbmlWorkshopBookingDetailViewFront = new returndbmlWorkshopBookingDetailViewFront();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Front].[WorkshopBookingDetailViewFrontGetByBookingId]", intDocId);
                db.LoadDataSet(cmdGet, ds, new string[] { "WorkshopBooking" });
                if (ds.Tables["WorkshopBooking"].Rows.Count > 0)
                {
                    objreturndbmlWorkshopBookingDetailViewFront.objdbmlWorkshopBookingDetailViewFront = new ObservableCollection<dbmlWorkshopBookingDetailViewFront>(from dRow in ds.Tables["WorkshopBooking"].AsEnumerable()
                                                                                                                                                                     select (ConvertTableToListNew<dbmlWorkshopBookingDetailViewFront>(dRow)));
                }

                objreturndbmlWorkshopBookingDetailViewFront.objdbmlStatus.StatusId = 1;
                objreturndbmlWorkshopBookingDetailViewFront.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlWorkshopBookingDetailViewFront.objdbmlStatus.StatusId = 99;
                objreturndbmlWorkshopBookingDetailViewFront.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlWorkshopBookingDetailViewFront;
        }
        #endregion

        #region Booking Detail AddOnServices
        public returndbmlBookingDetailAddOnServicesViewFront BookingDetailAddOnServicesInsertFront(returndbmlBookingDetailAddOnServicesViewFront objreturndbmlBookingDetailAddOnServicesViewFront)
        {
            returndbmlBookingDetailAddOnServicesViewFront objreturndbmlBookingDetailAddOnServicesViewFrontReturn = new returndbmlBookingDetailAddOnServicesViewFront();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intDocId = 0;
                foreach (var itm in objreturndbmlBookingDetailAddOnServicesViewFront.objdbmlBookingDetailAddOnServicesViewFront)
                {
                    intDocId = (int)itm.BookingId;
                    cmd = db.GetStoredProcCommand("[Front].[BookingDetailAddOnServicesInsertFront]");
                    foreach (PropertyInfo PropInfoCol in itm.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(itm, null));
                        }
                    }

                    db.ExecuteNonQuery(cmd, trans);

                    cmd = db.GetStoredProcCommand("[Front].[UpdateTabStatusToBookingByBookingId]", intDocId, 60);
                    db.ExecuteNonQuery(cmd, trans);
                }

                trans.Commit();


                objreturndbmlBookingDetailAddOnServicesViewFrontReturn = BookingDetailAddOnServicesViewFrontGetByBookingId(intDocId);
            }
            catch (Exception ex)
            {
                objreturndbmlBookingDetailAddOnServicesViewFrontReturn.objdbmlStatus.StatusId = 99;
                objreturndbmlBookingDetailAddOnServicesViewFrontReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlBookingDetailAddOnServicesViewFrontReturn;
        }

        public returndbmlBookingDetailAddOnServicesViewFront BookingDetailAddOnServicesDelete(int intDocId, int intBookingDetailAddOnServicesId)
        {
            returndbmlBookingDetailAddOnServicesViewFront objreturndbmlBookingDetailAddOnServicesViewFront = new returndbmlBookingDetailAddOnServicesViewFront();
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("[Transaction].[BookingDetailAddOnServicesDelete]", intBookingDetailAddOnServicesId);
                db.ExecuteNonQuery(cmd, trans);

                trans.Commit();
                objreturndbmlBookingDetailAddOnServicesViewFront.objdbmlStatus.StatusId = 1;
                objreturndbmlBookingDetailAddOnServicesViewFront.objdbmlStatus.Status = "Successful";

                objreturndbmlBookingDetailAddOnServicesViewFront = BookingDetailAddOnServicesViewFrontGetByBookingId(intDocId);
            }
            catch (Exception ex)
            {
                objreturndbmlBookingDetailAddOnServicesViewFront.objdbmlStatus.StatusId = 99;
                objreturndbmlBookingDetailAddOnServicesViewFront.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlBookingDetailAddOnServicesViewFront;
        }

        public returndbmlBookingDetailAddOnServicesViewFront BookingDetailAddOnServicesViewFrontGetByBookingId(int intDocId)
        {
            returndbmlBookingDetailAddOnServicesViewFront objreturndbmlBookingDetailAddOnServicesViewFront = new returndbmlBookingDetailAddOnServicesViewFront();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Front].[BookingDetailAddOnServicesViewFrontGetByBookingId]", intDocId);
                db.LoadDataSet(cmdGet, ds, new string[] { "BookingDetailAddOnServices" });
                if (ds.Tables["BookingDetailAddOnServices"].Rows.Count > 0)
                {
                    objreturndbmlBookingDetailAddOnServicesViewFront.objdbmlBookingDetailAddOnServicesViewFront = new ObservableCollection<dbmlBookingDetailAddOnServicesViewFront>(from dRow in ds.Tables["BookingDetailAddOnServices"].AsEnumerable()
                                                                                                                                                                                    select (ConvertTableToListNew<dbmlBookingDetailAddOnServicesViewFront>(dRow)));
                }

                objreturndbmlBookingDetailAddOnServicesViewFront.objdbmlStatus.StatusId = 1;
                objreturndbmlBookingDetailAddOnServicesViewFront.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlBookingDetailAddOnServicesViewFront.objdbmlStatus.StatusId = 99;
                objreturndbmlBookingDetailAddOnServicesViewFront.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlBookingDetailAddOnServicesViewFront;
        }
        #endregion

        #region Lab Booking Detail
        public returndbmlLabBookingDetailViewFront LabBookingDetailInsertFront(returndbmlLabBookingDetailViewFront objreturndbmlLabBookingDetailViewFront)
        {
            returndbmlLabBookingDetailViewFront objreturndbmlLabBookingDetailViewFrontReturn = new returndbmlLabBookingDetailViewFront();
            DbTransaction trans; DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                int intDocId = 0;
                foreach (var itm in objreturndbmlLabBookingDetailViewFront.objdbmlLabBookingDetailViewFront)
                {
                    intDocId = (int)itm.BookingId;
                    cmd = db.GetStoredProcCommand("[Front].[LabBookingDetailInsertFront]");
                    foreach (PropertyInfo PropInfoCol in itm.GetType().GetProperties())
                    {
                        string str = PropInfoCol.Name;
                        if (str.Length <= 2)
                            str = str + "modified";
                        if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                        {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(cmd, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(itm, null));
                        }
                    }

                    db.ExecuteNonQuery(cmd, trans);

                    //cmd = db.GetStoredProcCommand("[Front].[UpdateTabStatusToBookingByBookingId]", intDocId, 60);
                    //db.ExecuteNonQuery(cmd, trans);
                }

                trans.Commit();


                objreturndbmlLabBookingDetailViewFrontReturn = LabBookingDetailViewFrontGetByBookingId(intDocId);
            }
            catch (Exception ex)
            {
                objreturndbmlLabBookingDetailViewFrontReturn.objdbmlStatus.StatusId = 99;
                objreturndbmlLabBookingDetailViewFrontReturn.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlLabBookingDetailViewFrontReturn;
        }

        public returndbmlLabBookingDetailViewFront LabBookingDetailDelete(int intDocId, int intLabBookingDetailId)
        {
            returndbmlLabBookingDetailViewFront objreturndbmlLabBookingDetailViewFront = new returndbmlLabBookingDetailViewFront();
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(GF.StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            System.Data.Common.DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("[Transaction].[LabBookingDetailDelete]", intLabBookingDetailId);
                db.ExecuteNonQuery(cmd, trans);

                trans.Commit();
                objreturndbmlLabBookingDetailViewFront.objdbmlStatus.StatusId = 1;
                objreturndbmlLabBookingDetailViewFront.objdbmlStatus.Status = "Successful";

                objreturndbmlLabBookingDetailViewFront = LabBookingDetailViewFrontGetByBookingId(intDocId);
            }
            catch (Exception ex)
            {
                objreturndbmlLabBookingDetailViewFront.objdbmlStatus.StatusId = 99;
                objreturndbmlLabBookingDetailViewFront.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
                trans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return objreturndbmlLabBookingDetailViewFront;
        }

        public returndbmlLabBookingDetailViewFront LabBookingDetailViewFrontGetByBookingId(int intDocId)
        {
            returndbmlLabBookingDetailViewFront objreturndbmlLabBookingDetailViewFront = new returndbmlLabBookingDetailViewFront();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Front].[LabBookingDetailViewFrontGetByBookingId]", intDocId);
                db.LoadDataSet(cmdGet, ds, new string[] { "LabBookingDetail" });
                if (ds.Tables["LabBookingDetail"].Rows.Count > 0)
                {
                    objreturndbmlLabBookingDetailViewFront.objdbmlLabBookingDetailViewFront = new ObservableCollection<dbmlLabBookingDetailViewFront>(from dRow in ds.Tables["LabBookingDetail"].AsEnumerable()
                                                                                                                                                      select (ConvertTableToListNew<dbmlLabBookingDetailViewFront>(dRow)));
                }

                objreturndbmlLabBookingDetailViewFront.objdbmlStatus.StatusId = 1;
                objreturndbmlLabBookingDetailViewFront.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlLabBookingDetailViewFront.objdbmlStatus.StatusId = 99;
                objreturndbmlLabBookingDetailViewFront.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlLabBookingDetailViewFront;
        }

        public returndbmlLablinkVorC LablinkVorCGetAll()
        {
            returndbmlLablinkVorC objreturndbmlLablinkVorC = new returndbmlLablinkVorC();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Master].[LablinkVorCGetAll]");
                db.LoadDataSet(cmdGet, ds, new string[] { "LablinkVorC" });
                if (ds.Tables["LablinkVorC"].Rows.Count > 0)
                {
                    objreturndbmlLablinkVorC.objdbmlLablinkVorC = new ObservableCollection<dbmlLablinkVorC>(from dRow in ds.Tables["LablinkVorC"].AsEnumerable()
                                                                                                            select (ConvertTableToListNew<dbmlLablinkVorC>(dRow)));
                }

                objreturndbmlLablinkVorC.objdbmlStatus.StatusId = 1;
                objreturndbmlLablinkVorC.objdbmlStatus.Status = "Successful";
            }
            catch (Exception ex)
            {
                objreturndbmlLablinkVorC.objdbmlStatus.StatusId = 99;
                objreturndbmlLablinkVorC.objdbmlStatus.Status = ex.Message.ToString() + ex.StackTrace.ToString();
            }
            return objreturndbmlLablinkVorC;
        }
        #endregion

        #endregion


        #endregion

    }
}