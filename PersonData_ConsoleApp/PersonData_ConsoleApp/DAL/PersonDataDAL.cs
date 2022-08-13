using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;


namespace PersonData_ConsoleApp
{
    public class PersonDataDAL
    {
        #region Set XML file Path & File Name

        private string FolderPath = Environment.CurrentDirectory + "\\PersonData\\";
        private string PersonDataFileName = "PersonData.xml";
        private string CountryDataFileName = "CountryData.xml";

        #endregion Set XML file Path & File Name

        #region Constructor
        public PersonDataDAL()
        {
            try
            {
                DataSet dsPersonData = CommonUtility.XmlFile_Read(FolderPath, PersonDataFileName);
                DataSet dsCountryData = CommonUtility.XmlFile_Read(FolderPath, CountryDataFileName);

                if (dsPersonData != null)
                    dsPerson = dsPersonData;
                else
                    CreateData();

                if (dsCountryData != null)
                    dsCountry = dsCountryData;
                else
                    CreateCountryData();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Load Data Error : " + ex.Message.ToString() + Environment.NewLine + FolderPath);
            }
        }
        #endregion Constructor

        #region Initialize Tables

        public DataSet dsPerson { get; set; }
        public DataSet dsCountry { get; set; }
        private void CreateData()
        {
            try
            {
                dsPerson = new DataSet();
                DataTable dtPersonDetail = new DataTable();
                dtPersonDetail.TableName = "PersonDetail";
                dtPersonDetail.Columns.Add("PersonId", typeof(int));
                dtPersonDetail.Columns.Add("FirstName", typeof(string));
                dtPersonDetail.Columns.Add("LastName", typeof(string));
                dtPersonDetail.Columns.Add("DateOfBirth", typeof(DateTime));
                dtPersonDetail.Columns.Add("NickName", typeof(string));

                dsPerson.Tables.Add(dtPersonDetail);

                DataTable dtAddressDetail = new DataTable();
                dtAddressDetail.TableName = "AddressDetail";
                dtAddressDetail.Columns.Add("AddressId", typeof(int));
                dtAddressDetail.Columns.Add("Line1", typeof(string));
                dtAddressDetail.Columns.Add("Line2", typeof(string));
                dtAddressDetail.Columns.Add("Country", typeof(string));
                dtAddressDetail.Columns.Add("PostCode", typeof(string));
                dtAddressDetail.Columns.Add("PersonId", typeof(int));

                dsPerson.Tables.Add(dtAddressDetail);

                dtPersonDetail.Columns["PersonId"].AutoIncrement = true;
                dtPersonDetail.Columns["PersonId"].AutoIncrementSeed = dtPersonDetail.Rows.Count + 1;
                dtPersonDetail.Columns["PersonId"].AutoIncrementStep = 1;
                dtPersonDetail.Constraints.Add("Primary_PersonId", dtPersonDetail.Columns["PersonId"], true);

                dtAddressDetail.Columns["AddressId"].AutoIncrement = true;
                dtAddressDetail.Columns["AddressId"].AutoIncrementSeed = dtAddressDetail.Rows.Count + 1;
                dtAddressDetail.Columns["AddressId"].AutoIncrementStep = 1;
                dtAddressDetail.Constraints.Add("Primary_AddressId", dtAddressDetail.Columns["AddressId"], true);

                dsPerson.Tables["AddressDetail"].Constraints.Add("Refrence_AddressDetail", dsPerson.Tables["PersonDetail"].Columns["PersonId"], dsPerson.Tables["AddressDetail"].Columns["PersonId"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Data Error : " + ex.Message.ToString());
            }
        }
        private void CreateCountryData()
        {
            try
            {
                dsCountry = new DataSet();
                DataTable dtCountry = new DataTable();
                dtCountry.TableName = "CountryData";
                dtCountry.Columns.Add("CountryName", typeof(string));

                dtCountry.Rows.Add("Russia");
                dtCountry.Rows.Add("Germany");
                dtCountry.Rows.Add("United Kingdom");
                dtCountry.Rows.Add("France");
                dtCountry.Rows.Add("Italy");
                dtCountry.Rows.Add("Spain");
                dtCountry.Rows.Add("Ukraine");
                dtCountry.Rows.Add("Poland");
                dtCountry.Rows.Add("Romania");
                dtCountry.Rows.Add("Netherlands");
                dtCountry.Rows.Add("Belgium");
                dtCountry.Rows.Add("Czechia");
                dtCountry.Rows.Add("Greece");
                dtCountry.Rows.Add("Portugal");
                dtCountry.Rows.Add("Sweden");
                dtCountry.Rows.Add("Hungary");
                dtCountry.Rows.Add("Belarus");
                dtCountry.Rows.Add("Austria");
                dtCountry.Rows.Add("Serbia");
                dtCountry.Rows.Add("Switzerland");
                dtCountry.Rows.Add("Bulgaria");
                dtCountry.Rows.Add("Denmark");
                dtCountry.Rows.Add("Finland");
                dtCountry.Rows.Add("Slovakia");
                dtCountry.Rows.Add("Norway");
                dtCountry.Rows.Add("Ireland");
                dtCountry.Rows.Add("Croatia");
                dtCountry.Rows.Add("Moldova");
                dtCountry.Rows.Add("Bosnia and Herzegovina");
                dtCountry.Rows.Add("Albania");
                dtCountry.Rows.Add("Lithuania");
                dtCountry.Rows.Add("North Macedonia");
                dtCountry.Rows.Add("Slovenia");
                dtCountry.Rows.Add("Latvia");
                dtCountry.Rows.Add("Kosovo");
                dtCountry.Rows.Add("Estonia");
                dtCountry.Rows.Add("Montenegro");
                dtCountry.Rows.Add("Luxembourg");
                dtCountry.Rows.Add("Malta");
                dtCountry.Rows.Add("Iceland");
                dtCountry.Rows.Add("Andorra");
                dtCountry.Rows.Add("Monaco");
                dtCountry.Rows.Add("Liechtenstein");
                dtCountry.Rows.Add("San Marino");
                dtCountry.Rows.Add("Holy See");
                dsCountry.Tables.Add(dtCountry);

                CommonUtility.XmlFile_Write(dsCountry, FolderPath, CountryDataFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Data Error : " + ex.Message.ToString());
            }
        }

        #endregion Initialize Tables

        #region Insert,Update & Delete Methods

        public void AddPerson()
        {
            try
            {
                DataRow drNewPerson = dsPerson.Tables["PersonDetail"].NewRow();

                do
                {
                    GetData("Enter person first name : ", drNewPerson, "First Name", "FirstName", "Text", true);
                    GetData("Enter person last name : ", drNewPerson, "Last Name", "LastName", "Text", true);
                }
                while ((ValidatePerson(CommonUtility.CString(drNewPerson["FirstName"]), CommonUtility.CString(drNewPerson["LastName"]), 0)) == false);

                GetData("Enter person date of birth : ", drNewPerson, "Date Of Birth", "DateOfBirth", "Date", true);

                GetData("Enter person nick name : ", drNewPerson, "Nick Name", "NickName", "Text", false);

                dsPerson.Tables["PersonDetail"].Rows.Add(drNewPerson);

                DataRow[] drPerson = dsPerson.Tables["PersonDetail"].Select("FirstName='" + CommonUtility.CString(drNewPerson["FirstName"]).Replace("'", "''") + "' And LastName='" + CommonUtility.CString(drNewPerson["LastName"]).Replace("'", "''") + "'");
                if (drPerson.Length > 0)
                    AddAddress(CommonUtility.Cint(drPerson[0]["PersonId"]), false);

                CommonUtility.XmlFile_Write(dsPerson, FolderPath, PersonDataFileName);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n*********** Data save successfully  ***********");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Error : " + ex.Message.ToString());
            }
            finally
            {
                Console.ResetColor();
            }
        }
        public void AddAddress(int PersonId, bool IsForExistingPerson)
        {
            try
            {
                if (IsPersonExist(PersonId) == true)
                {
                    DataRow drNewAddress = dsPerson.Tables["AddressDetail"].NewRow();
                    Console.WriteLine("Enter Address Detail");
                    do
                    {
                        GetData("Enter address line 1 : ", drNewAddress, "Line1", "Line1", "NoValidation", true);
                        GetData("Enter address line 2 : ", drNewAddress, "Line2", "Line2", "Text", false);
                        do
                        {
                            GetData("Enter country : ", drNewAddress, "Country", "Country", "Text", false);
                        }
                        while (IsValidCountry(CommonUtility.CString(drNewAddress["Country"])) == false);

                        GetData("Enter postCode : ", drNewAddress, "PostCode", "PostCode", "AlphaNumeric", false);
                        drNewAddress["PersonId"] = CommonUtility.Cint(PersonId);
                    }
                    while (ValidateAddress(CommonUtility.CString(drNewAddress["Line1"]), CommonUtility.CString(drNewAddress["Line2"]),
                    CommonUtility.CString(drNewAddress["Country"]), CommonUtility.CString(drNewAddress["PostCode"]),
                     0, CommonUtility.Cint(drNewAddress["PersonId"])) == false);

                    dsPerson.Tables["AddressDetail"].Rows.Add(drNewAddress);

                    if (IsForExistingPerson == true)
                    {
                        CommonUtility.XmlFile_Write(dsPerson, FolderPath, PersonDataFileName);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n*********** Data save successfully  ***********");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Error : " + ex.Message.ToString());
            }
            finally
            {
                Console.ResetColor();
            }
        }
        public void UpdatePerson()
        {
            int PersonId;
            try
            {
                Console.WriteLine("Enter person id for edit data : ");
                PersonId = CommonUtility.Cint(Console.ReadLine());

                if (IsPersonExist(PersonId) == true)
                {
                    DataRow[] drUpdateData = dsPerson.Tables["PersonDetail"].Select("PersonId=" + CommonUtility.Cint(PersonId));

                    do
                    {
                        GetData("Enter person first name : ", drUpdateData[0], "First Name", "FirstName", "Text", true);
                        GetData("Enter person last name : ", drUpdateData[0], "Last Name", "LastName", "Text", true);
                    }
                    while ((ValidatePerson(CommonUtility.CString(drUpdateData[0]["FirstName"]), CommonUtility.CString(drUpdateData[0]["LastName"]), CommonUtility.Cint(drUpdateData[0]["PersonId"]))) == false);

                    GetData("Enter person date of birth : ", drUpdateData[0], "Date Of Birth", "DateOfBirth", "Date", true);

                    GetData("Enter person nick name : ", drUpdateData[0], "Nick Name", "NickName", "Text", false);

                    CommonUtility.XmlFile_Write(dsPerson, FolderPath, PersonDataFileName);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n*********** Data update successfully  ***********");
                }
                else
                {
                    Console.WriteLine("\n No Data Found For PersonId :" + CommonUtility.CString(PersonId));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update Error : " + ex.Message.ToString());
            }
            finally
            {
                Console.ResetColor();
            }
        }
        public void UpdateAddress()
        {
            int AddressId;
            try
            {
                Console.WriteLine("Enter Address ID For Edit Data : ");
                AddressId = CommonUtility.Cint(Console.ReadLine());

                if (IsAddressExist(AddressId) == true)
                {
                    DataRow[] drUpdateData = dsPerson.Tables["AddressDetail"].Select("AddressId=" + CommonUtility.Cint(AddressId));

                    do
                    {
                        GetData("Enter address line 1 : ", drUpdateData[0], "Line1", "Line1", "NoValidation", true);
                        GetData("Enter address line 2 : ", drUpdateData[0], "Line2", "Line2", "Text", false);
                        do
                        {
                            GetData("Enter country : ", drUpdateData[0], "Country", "Country", "Text", false);
                        }
                        while (IsValidCountry(CommonUtility.CString(drUpdateData[0]["Country"])) == false);
                        GetData("Enter postCode : ", drUpdateData[0], "PostCode", "PostCode", "AlphaNumeric", false);
                    }
                    while (ValidateAddress(CommonUtility.CString(drUpdateData[0]["Line1"]), CommonUtility.CString(drUpdateData[0]["Line2"]),
                    CommonUtility.CString(drUpdateData[0]["Country"]), CommonUtility.CString(drUpdateData[0]["PostCode"]),
                    CommonUtility.Cint(drUpdateData[0]["AddressId"]), CommonUtility.Cint(drUpdateData[0]["PersonId"])) == false);

                    CommonUtility.XmlFile_Write(dsPerson, FolderPath, PersonDataFileName);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n*********** Data update successfully  ***********");
                }
                else
                {
                    Console.WriteLine("\n No Data Found For PersonId :" + CommonUtility.CString(AddressId));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update Error : " + ex.Message.ToString());
            }
            finally
            {
                Console.ResetColor();
            }
        }
        public void DeletePerson()
        {
            int PersonId;
            try
            {
                Console.WriteLine("Enter person id for delete : ");
                PersonId = CommonUtility.Cint(Console.ReadLine());

                if (IsPersonExist(PersonId) == true)
                {
                    Console.WriteLine("\nData for person id " + CommonUtility.CString(PersonId) + " is");
                    SearchData((int)CommonUtility.SearchDataBy.ByPersonId, PersonId);

                    DataRow[] drDeleteData = dsPerson.Tables["PersonDetail"].Select("PersonId=" + CommonUtility.Cint(PersonId));
                    Console.WriteLine("Are you sure you want to delete? (Y/N)");

                    if (CommonUtility.CString(Console.ReadLine()).ToUpper() == "Y")
                    {
                        DataRow[] drAddress = dsPerson.Tables["AddressDetail"].Select("PersonId=" + CommonUtility.Cint(PersonId));

                        foreach (DataRow dr in drAddress)
                            dsPerson.Tables["AddressDetail"].Rows.Remove(dr);

                        dsPerson.Tables["PersonDetail"].Rows.Remove(drDeleteData[0]);

                        dsPerson.AcceptChanges();

                        CommonUtility.XmlFile_Write(dsPerson, FolderPath, PersonDataFileName);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n*********** Data delete successfully  ***********");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n No data found for PersonId :" + CommonUtility.CString(PersonId));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete Error : " + ex.Message.ToString());
            }
            finally
            {
                Console.ResetColor();
            }
        }
        public void DeleteAddress()
        {
            int AddressId;
            try
            {
                Console.WriteLine("Enter address id for delete : ");
                AddressId = CommonUtility.Cint(Console.ReadLine());

                if (IsAddressExist(AddressId) == true)
                {
                    DataRow[] drDeleteData = dsPerson.Tables["AddressDetail"].Select("AddressId=" + CommonUtility.Cint(AddressId));
                    Console.WriteLine("Are you sure you want to delete? (Y/N)");

                    if (CommonUtility.CString(Console.ReadLine()).ToUpper() == "Y")
                    {
                        dsPerson.Tables["AddressDetail"].Rows.Remove(drDeleteData[0]);

                        dsPerson.AcceptChanges();

                        CommonUtility.XmlFile_Write(dsPerson, FolderPath, PersonDataFileName);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n*********** Data delete successfully  ***********");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n No data found for PersonId :" + CommonUtility.CString(AddressId));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete Error : " + ex.Message.ToString());
            }
            finally
            {
                Console.ResetColor();
            }
        }

        #endregion Insert,Update & Delete Methods

        #region Get Data
        private void GetData(string ShowText, DataRow drNewPerson, string Field_Caption, string Field_Name, string Field_DataType, bool IsRequired)
        {
            try
            {
                Console.WriteLine("\n" + ShowText);
                {
                    string value = "";
                    string Msg = ".";

                    while (Msg != "")
                    {
                        value = CommonUtility.CString(Console.ReadLine());
                        ValidateStringData(Field_Caption, value, Field_DataType, IsRequired, ref Msg);
                    }

                    if (Field_DataType == "Date")
                        drNewPerson[Field_Name] = CommonUtility.CDateTime(value);
                    else
                        drNewPerson[Field_Name] = value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get Data Error : " + ex.Message.ToString());
            }
        }
        #endregion Get Data

        #region Validation Methods
        private bool ValidatePerson(string FirstName, string LastName, int PersonId)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                DataRow[] drValidatePerson = dsPerson.Tables["PersonDetail"].Select("FirstName='" + FirstName.Replace("'", "''") + "' And LastName='" + LastName.Replace("'", "''") + "' And PersonId<>" + CommonUtility.Cint(PersonId));
                if (drValidatePerson.Length > 0)
                {
                    Console.WriteLine("Validation : Person already exists , Please check it");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Person Validation Error : " + ex.Message.ToString());
                return false;
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private bool ValidateAddress(string Line1, string Line2, string Country, string PostCode, int AddressId, int PersonId)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                DataRow[] drValidateAddress = dsPerson.Tables["AddressDetail"].Select("Line1='" + Line1.Replace("'", "''") + "' And Line2='" + Line2.Replace("'", "''") + "' And Country='" + Country.Replace("'", "''") + "' And PostCode='" + PostCode.Replace("'", "''") + "' And AddressId<>" + CommonUtility.Cint(AddressId) + " And PersonId=" + CommonUtility.Cint(PersonId));
                if (drValidateAddress.Length > 0)
                {
                    Console.WriteLine("Validation : Address already exists for this person, Please check it");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Address Validation Error : " + ex.Message.ToString());
                return false;
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private void ValidateStringData(string Caption, string StringValue, string Field_DataType, bool IsRequired, ref string Msg)
        {
            try
            {
                DateTime DateValue;
                Console.ForegroundColor = ConsoleColor.Red;
                if (CommonUtility.CString(StringValue) == "" && IsRequired == true)
                {
                    Msg = "Validation : " + Caption + " can't be empty! Please enter " + Caption.ToLower() + "\n";
                    Console.WriteLine(Msg, ConsoleColor.Red);
                }
                else if (CommonUtility.CString(StringValue) != "")
                {
                    if (CommonUtility.CString(StringValue).Any(char.IsNumber) == true && Field_DataType == "Text")
                    {
                        Msg = "Validation : " + Caption + " Invalid input , Enter only character\n";
                        Console.WriteLine(Msg);
                    }
                    else if (CommonUtility.CString(StringValue).All(char.IsDigit) == false && Field_DataType == "Number")
                    {
                        Msg = "Validation : " + Caption + " Invalid input , Enter only number\n";
                        Console.WriteLine(Msg, ConsoleColor.Red);
                    }
                    else if (IsStringWithSpecialCharacters(CommonUtility.CString(StringValue)) == true && Field_DataType == "AlphaNumeric")
                    {
                        Msg = "Validation : " + Caption + " Invalid input , Special character not allow\n";
                        Console.WriteLine(Msg);
                    }
                    else if (DateTime.TryParse(StringValue, out DateValue) == false && Field_DataType == "Date")
                    {
                        Msg = "Validation : " + Caption + " Invalid input , Enter date in (dd/MM/yyyy) format\n";
                        Console.WriteLine(Msg);
                    }
                    else
                    {
                        Msg = "";
                    }
                }
                else
                {
                    Msg = "";
                }
            }
            catch (Exception ex)
            {
                Msg = "Validation Error : " + ex.Message.ToString();
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private bool IsStringWithSpecialCharacters(string StringValue)
        {
            try
            {
                string specialCharacters = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
                char[] specialCharactersArray = specialCharacters.ToCharArray();

                int index = StringValue.IndexOfAny(specialCharactersArray);

                if (index == -1)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Special character validation error : " + ex.Message.ToString());
                return false;
            }
        }
        private bool IsPersonExist(int PersonId)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (dsPerson == null || dsPerson.Tables["PersonDetail"].Rows.Count == 0)
                    throw new Exception("Validation : No any data found");

                DataRow[] drPersonData = dsPerson.Tables["PersonDetail"].Select("PersonId=" + CommonUtility.Cint(PersonId));

                if (drPersonData.Length > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Person validation error : " + ex.Message.ToString());
                return false;
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private bool IsAddressExist(int AddressId)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;

                if (dsPerson == null || dsPerson.Tables["AddressDetail"].Rows.Count == 0)
                    throw new Exception("Validation : No any data found");

                DataRow[] drAddressData = dsPerson.Tables["AddressDetail"].Select("AddressId=" + CommonUtility.Cint(AddressId));

                if (drAddressData.Length > 0)
                {
                    Console.WriteLine("\nInvalid address , Address already exists for this person");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Person validation error : " + ex.Message.ToString());
                return false;
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private bool IsValidCountry(string CountryName)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;

                if (dsCountry == null || dsCountry.Tables["CountryData"].Rows.Count == 0)
                    throw new Exception("Country Validation : No any data found");

                DataRow[] drCountryData = dsCountry.Tables["CountryData"].Select("CountryName='" + CommonUtility.CString(CountryName).Replace("'", "''") + "'");

                if (drCountryData.Length > 0)
                {
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid country , It's must be valid country in Europe");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Country validation error : " + ex.Message.ToString());
                return false;
            }
            finally
            {
                Console.ResetColor();
            }
        }

        #endregion Validation Methods

        #region Search Data
        public void SearchData(int SearchBy, int PersonId)
        {
            int PersonId_Search;
            string Value_Search = "";
            try
            {
                string FilterString = "1=1";

                if (PersonId > 0)
                {
                    PersonId_Search = PersonId;
                    FilterString = " PersonId=" + PersonId_Search;
                }


                if (dsPerson == null || dsPerson.Tables["PersonDetail"].Rows.Count == 0)
                    throw new Exception("No any data found for search please first add person detail");

                if (SearchBy == (int)CommonUtility.SearchDataBy.AllPerson || SearchBy == (int)CommonUtility.SearchDataBy.ByPersonId ||
                    SearchBy == (int)CommonUtility.SearchDataBy.ByFirstName || SearchBy == (int)CommonUtility.SearchDataBy.ByLastName)
                {
                    if (SearchBy == (int)CommonUtility.SearchDataBy.ByPersonId && PersonId <= 0)
                    {
                        Console.WriteLine("Enter person id for search data :");
                        PersonId_Search = CommonUtility.Cint(Console.ReadLine());
                        FilterString = " PersonId=" + PersonId_Search;
                    }
                    else if (SearchBy == (int)CommonUtility.SearchDataBy.ByFirstName || SearchBy == (int)CommonUtility.SearchDataBy.ByLastName)
                    {
                        Console.WriteLine("Enter person " + (SearchBy == (int)CommonUtility.SearchDataBy.ByFirstName ? "firstname" : "lastname") + " for search data :");
                        Value_Search = CommonUtility.CString(Console.ReadLine());
                        FilterString = (SearchBy == (int)CommonUtility.SearchDataBy.ByFirstName ? "firstname='" : "lastname='") + CommonUtility.CString(Value_Search) + "'";
                    }

                    int[] arryPersonColumnWidth = new int[dsPerson.Tables["PersonDetail"].Columns.Count];

                    int columncounter = 0;
                    foreach (DataColumn dc in dsPerson.Tables["PersonDetail"].Columns)
                    {
                        int maxColumnLength = dc.ColumnName.Length;
                        foreach (DataRow dr in dsPerson.Tables["PersonDetail"].Rows)
                        {
                            string strData = CommonUtility.CString(dr[dc.ColumnName]);
                            if (dc.DataType == typeof(DateTime))
                                strData = CommonUtility.CDateTime(dr[dc.ColumnName]).ToString("dd/MM/yyyy");

                            if (strData.Length > maxColumnLength)
                                maxColumnLength = CommonUtility.CString(dr[dc.ColumnName]).Length;
                        }

                        arryPersonColumnWidth[columncounter] = maxColumnLength;
                        columncounter++;
                    }

                    columncounter = 0;
                    string PersonCol = "";

                    foreach (DataColumn dc in dsPerson.Tables["PersonDetail"].Columns)
                    {
                        PersonCol += (PersonCol == "" ? "| " : " | ") + dc.ColumnName.PadRight(arryPersonColumnWidth[columncounter], (char)32);
                        columncounter++;
                    }
                    PersonCol += " |";

                    string AddressCol = "";
                    int[] arryAddressColumnWidth = new int[dsPerson.Tables["AddressDetail"].Columns.Count];
                    if (SearchBy == (int)CommonUtility.SearchDataBy.ByPersonId)
                    {
                        columncounter = 0;
                        foreach (DataColumn dc in dsPerson.Tables["AddressDetail"].Columns)
                        {
                            if (dc.ColumnName == "PersonId")
                                continue;

                            int maxColumnLength = dc.ColumnName.Length;
                            foreach (DataRow dr in dsPerson.Tables["AddressDetail"].Rows)
                            {
                                if (CommonUtility.CString(dr[dc.ColumnName]).Length > maxColumnLength)
                                    maxColumnLength = CommonUtility.CString(dr[dc.ColumnName]).Length;
                            }

                            arryAddressColumnWidth[columncounter] = maxColumnLength;
                            columncounter++;
                        }

                        columncounter = 0;
                        foreach (DataColumn dc in dsPerson.Tables["AddressDetail"].Columns)
                        {
                            if (dc.ColumnName == "PersonId")
                                continue;

                            AddressCol += (AddressCol == "" ? "| " : " | ") + dc.ColumnName.PadRight(arryAddressColumnWidth[columncounter], (char)32);
                            columncounter++;
                        }
                        AddressCol += " |";
                    }

                    int rowcount = 0;
                    string PersonData = "";

                    foreach (DataRow drData in dsPerson.Tables["PersonDetail"].Select(FilterString))
                    {
                        string NewPersonDataLine = "";

                        if (rowcount == 0)
                            PersonData += GetTableLineByWidth(PersonCol.Length) + Environment.NewLine + PersonCol +
                                Environment.NewLine + GetTableLineByWidth(PersonCol.Length) + Environment.NewLine;

                        columncounter = 0;
                        foreach (DataColumn dc in dsPerson.Tables["PersonDetail"].Columns)
                        {
                            NewPersonDataLine += (NewPersonDataLine == "" ? "| " : " | ") + (dc.DataType == Type.GetType("System.DateTime") ?
                                CommonUtility.CDateTime(drData[dc.ColumnName]).Date.ToString("dd/MM/yyyy").PadRight(arryPersonColumnWidth[columncounter], (char)32) :
                                CommonUtility.CString(drData[dc.ColumnName]).PadRight(arryPersonColumnWidth[columncounter], (char)32));
                            columncounter++;
                        }
                        PersonData += NewPersonDataLine + " |" + Environment.NewLine + GetTableLineByWidth(PersonCol.Length) + Environment.NewLine;

                        if (SearchBy == (int)CommonUtility.SearchDataBy.ByPersonId)
                        {
                            PersonData += GetTableLineByWidth(AddressCol.Length) + Environment.NewLine + AddressCol +
                            Environment.NewLine + GetTableLineByWidth(AddressCol.Length) + Environment.NewLine;

                            foreach (DataRow drAddress in dsPerson.Tables["AddressDetail"].Select("PersonId=" + CommonUtility.Cint(drData["PersonId"])))
                            {
                                string NewAddressDataLine = "";
                                columncounter = 0;
                                foreach (DataColumn dc in dsPerson.Tables["AddressDetail"].Columns)
                                {
                                    if (dc.ColumnName == "PersonId")
                                        continue;

                                    NewAddressDataLine += (NewAddressDataLine == "" ? "| " : " | ") +
                                        CommonUtility.CString(drAddress[dc.ColumnName]).PadRight(arryAddressColumnWidth[columncounter], (char)32);
                                    columncounter++;
                                }
                                PersonData += NewAddressDataLine + " |" + Environment.NewLine + GetTableLineByWidth(AddressCol.Length) + Environment.NewLine;
                            }
                        }
                        rowcount++;
                    }

                    Console.WriteLine(PersonData);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Search Error : " + ex.Message.ToString());
            }
        }

        private string GetTableLineByWidth(int Length)
        {
            try
            {
                return (new string('-', Length));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Table Print Error : " + ex.Message.ToString());
                return "";
            }
        }

        #endregion Search Data


    }
}
