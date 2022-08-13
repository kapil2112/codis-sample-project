using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonData_ConsoleApp
{
    public static class CommonUtility
    {
        public enum SearchDataBy : int
        {
            AllPerson = 1,
            ByPersonId = 2,
            ByFirstName = 3,
            ByLastName = 4
        };

        #region " Conversion/Casting "
        public static int Cint(object obj)
        {
            if (obj == null) return 0;
            int a;
            int.TryParse(Convert.ToString(obj), out a);
            return a;
        }
        public static String CString(object obj)
        {
            return Convert.ToString(obj);
        }
        public static DateTime CDateTime(object obj)
        {
            if (obj == null) return DateTime.Today;
            DateTime a = DateTime.Today;
            DateTime.TryParse(Convert.ToString(obj), out a);
            return a;
        }

        #endregion " Conversion/Casting "

        #region " Xml File Read/Write "

        #region " Write Xml File "

        public static void XmlFile_Write(DataSet ds, string FolderPath,string FileName)
        {
            try
            {
                int i = 0;
                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.DataType == Type.GetType("System.DateTime"))
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr[dc] != DBNull.Value)
                                {
                                    dr[dc] = CommonUtility.CDateTime(dr[dc]).ToLocalTime();
                                }
                            }
                        }
                    }
                    i++;
                }
                ds.AcceptChanges();

                DirectoryInfo dir = new DirectoryInfo(FolderPath);
                if (!dir.Exists)
                    dir.Create();

                ds.WriteXml(FolderPath + @"\" + FileName, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                Console.WriteLine("XML Write Error : " + ex.Message.ToString());
            }
        }

        #endregion " Write Xml File "

        #region " Load Xml File "

        public static DataSet XmlFile_Read(string FolderName,string FileName)
        {
            try
            {
                //FileName = f_FileName.Replace(".xml", "");

                if (File.Exists(FolderName + @"\" + FileName))
                {
                    DataSet dsPersonData = new DataSet();
                    dsPersonData.ReadXml(FolderName + @"\" + FileName);

                    foreach (DataTable dt in dsPersonData.Tables)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dc.DataType == Type.GetType("System.DateTime"))
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr[dc] != DBNull.Value)
                                    {
                                        dr[dc] = CommonUtility.CDateTime(dr[dc]).ToUniversalTime();
                                    }
                                }
                            }
                        }
                    }
                    dsPersonData.AcceptChanges();
                    return dsPersonData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("XML Read Error : " + ex.Message.ToString());
                return null;
            }
        }
        
        #endregion " Load Xml File "

        #endregion " Xml File Read/Write "
    }
}
