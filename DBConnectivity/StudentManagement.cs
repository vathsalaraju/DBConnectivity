using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DBConnectivity
{
    class StudentManagement
    {
        public static void Main()
        {
            UserInterface ui = new ScreenDescription();
            while (true)
            {
                ui.showFirstScreen();
            }
        }
    }
    
}
