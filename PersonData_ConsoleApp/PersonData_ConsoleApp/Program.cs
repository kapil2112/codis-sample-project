using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonData_ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null)
                OperationData();
        }

        private static void CallMain()
        {
            try
            {
                Console.WriteLine("Press any key to continue....");
                Console.ReadKey();
                OperationData();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
            }
        }
        private static void OperationData()
        {
            try
            {
                int UserInput = 0;
                PersonDataDAL _PersonDAL = new PersonDataDAL();

                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Choose Your Options");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Enter 1 For Add Data");
                Console.WriteLine("Enter 2 For Edit Data");
                Console.WriteLine("Enter 3 For Delete Data");
                Console.WriteLine("Enter 4 For View Data");
                Console.WriteLine("Enter 5 For Exit");
                Console.WriteLine("--------------------------------------");
                int option = CommonUtility.Cint(Console.ReadLine());
                if (option == 1)
                {
                    Console.WriteLine("\n====================================================");
                    Console.WriteLine("Enter 1 for add new person & address detail");
                    Console.WriteLine("Enter 2 for add an address detail of existing person");
                    Console.WriteLine("====================================================");

                    UserInput = CommonUtility.Cint(Console.ReadLine());

                    if (UserInput == (int)1)
                    {
                        _PersonDAL.AddPerson();
                    }
                    else if (UserInput == (int)2)
                    {
                        Console.WriteLine("Enter person id for add an address");
                        _PersonDAL.AddAddress(CommonUtility.Cint(Console.ReadLine()), true);
                    }

                    CallMain();
                }
                else if (option == 2)
                {
                    Console.WriteLine("\n================================================");
                    Console.WriteLine("Enter 1 For update detail of existing person");
                    Console.WriteLine("Enter 2 For update an address of existing person");
                    Console.WriteLine("================================================");

                    UserInput = Convert.ToInt32(Console.ReadLine());

                    if (UserInput == (int)1)
                        _PersonDAL.UpdatePerson();
                    else if (UserInput == (int)2)
                        _PersonDAL.UpdateAddress();

                    CallMain();
                }
                else if (option == 3)
                {
                    Console.WriteLine("\n================================================");
                    Console.WriteLine("Enter 1 For delete person & their address detail");
                    Console.WriteLine("Enter 2 For delete an address of existing person");
                    Console.WriteLine("================================================");

                    UserInput = Convert.ToInt32(Console.ReadLine());

                    if (UserInput == (int)1)
                        _PersonDAL.DeletePerson();
                    else if (UserInput == (int)2)
                        _PersonDAL.DeleteAddress();

                    CallMain();
                }
                else if (option == 4)
                {
                    Console.WriteLine("\n==========================================================");
                    Console.WriteLine("Enter 1 for view all persons detail");
                    Console.WriteLine("Enter 2 for view persons and their all address Detail By Id");
                    Console.WriteLine("Enter 3 for view persons detail by first name");
                    Console.WriteLine("Enter 4 for view persons detail by last name");
                    Console.WriteLine("==========================================================");

                    _PersonDAL.SearchData(CommonUtility.Cint(Console.ReadLine()), 0);
                    CallMain();
                }
                else if (option == 5)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Main(new string[] { "" });
                }
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Operation Data Error : " + ex.Message.ToString());
            }
        }

    }
}
