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
                cmd = db.GetStoredProcCommand("Security.UserGetByLoginId", strLoginId);

                db.LoadDataSet(cmd, ds, new string[] { "UserView" });

                if (ds.Tables["UserView"].Rows.Count > 0)
                {
                    objreturndbmlUser.objdbmlUserView = new ObservableCollection<dbmlUserView>
                    (from dRow in ds.Tables["UserView"].AsEnumerable() select (ConvertTableToListNew<dbmlUserView>(dRow)));


                    if (Cryptographer.CompareHash("ePGMS", strPassword, objreturndbmlUser.objdbmlUserView.FirstOrDefault().PassWord))
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
            }
            catch (Exception ex)
            {
                objreturndbmlUser.objdbmlStatus.StatusId = 99;
                objreturndbmlUser.objdbmlStatus.Status = ex.Message + ex.StackTrace;
            }
            return objreturndbmlUser;
        }

        public returndbmlCompanyView CompanyViewGetByCompanyId(int intCompanyId)
        {
            returndbmlCompanyView objreturndbmlCompanyView = new returndbmlCompanyView();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("Front.[CompanyViewGetByCompanyId]", intCompanyId);
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

        #endregion

        #region Properties
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

        public returndbmlCompanyDepartment CompanyDepartmentGetByCustomerMasterId(int intCustomerMasterId)
        {
            returndbmlCompanyDepartment objreturndbmlCompanyDepartment = new returndbmlCompanyDepartment();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Master].[CompanyDepartmentGetByCustomerMasterId]", intCustomerMasterId);
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

                    cmd = db.GetStoredProcCommand("[Front].[UpdateTabStatusToBookingByBookingId]", intDocId,40);
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
                cmd = db.GetStoredProcCommand("[Transaction].[ListOfVehicleComponentDeleteByDocIdCompId]", intDocId, intVehCompId);
                db.ExecuteNonQuery(cmd, trans);

                trans.Commit();
                objreturndbmlListOfVehicleComponent.objdbmlStatus.StatusId = 1;
                objreturndbmlListOfVehicleComponent.objdbmlStatus.Status = "Successful";

                objreturndbmlListOfVehicleComponent = ListOfVehicleComponentGetByDocId(intDocId);
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
                int intServiceId=(int)objreturndbmlTrackBookingDetail.objdbmlTrackBookingTimeDetail.FirstOrDefault().ServiceId;
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

                cmd = db.GetStoredProcCommand("[Front].[TrackBookingDetailInsertFront]", intBookingId, intTrackGroupId, intVehicleId);
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
        public returndbmlWorkFlowView WorkFlowViewGetByBPId(int intBPId)
        {
            returndbmlWorkFlowView objreturndbmlWorkFlowView = new returndbmlWorkFlowView();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(GF.StrSetConnection());
                System.Data.Common.DbCommand cmdGet = null;

                cmdGet = db.GetStoredProcCommand("[Front].[WorkFlowViewGetByBPId]", intBPId);
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

        public returndbmlBooking WorkFlowActivityInsert(int intDocId,int intBPId,int intWorkPlowId,int intStatusId,string strRemark,int intCreateId)
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

                objreturndbmlBooking =BookingViewGetByBookingId(intDocId);
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
        #endregion

        #endregion





        #endregion

    }
}