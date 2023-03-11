using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Seminar_CSharp.Class;

namespace Seminar_CSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {



            /*       Test Connecting to Bd  START       */

            var builder = new ConfigurationBuilder();
            // setting the path to the current directory
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // get configuration from appsettings.json file
            builder.AddJsonFile("appsettings.json");
            // create a configuration
            var config = builder.Build();
            // get connection string
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;


            /*       Test Connecting to Bd END         */


            //Tests for Db. I will make UnitTest in 2nd faze. Now we have only object tests.
            using (ApplicationContext db = new ApplicationContext(options))
            {

                Activity faze1 = new Activity {Start = DateTime.Now, Description = "Project delame1" };
                Activity faze2 = new Activity { Start = DateTime.Now, Description = "Project delame2" };
                Activity faze3 = new Activity { Start = DateTime.Now, Description = "Project delame3" };
                db.Activities.AddRange(faze1,faze2,faze3);

                User Vlad = new User { Name = "Dima", Surname = "Malashchuk", NickName = "xmalas04", Photo = "111", ShowNickName=true  };
                User Kate = new User { Name = "Kate", Surname = "AvadaKedabra", NickName = "xsasu01", Photo = "222", ShowNickName = false };
                User Steve = new User { Name = "Steve", Surname = "Potter", NickName = "xxyilo28", Photo = "333", ShowNickName = true };
                db.Users.AddRange(Vlad, Kate, Steve);  
                
                Project IDM = new Project { Name = "IDM", Description="Project of IDM" };
                Project ILG = new Project { Name = "ILG", Description = "Project of ILG" };
                Project IMA1 = new Project { Name = "IMA1", Description = "Project of IMA1" };

                db.Projects.AddRange(IDM,ILG, IMA1);
             
                faze1.Project = IDM; 
                faze2.Project = ILG;
                faze3.Project = IMA1;
                faze1.User = Vlad; 
                faze2.User = Kate;
                faze3.User = Steve;
                Vlad.Projects.Add(IDM);
                Vlad.Projects.Add(ILG);
                Vlad.Projects.Add(IMA1);
                Kate.Projects.Add(IDM);
                Kate.Projects.Add(ILG);
                Kate.Projects.Add(IMA1);
                Steve.Projects.Add(IDM);
                Steve.Projects.Add(ILG);
                Steve.Projects.Add(IMA1);

                db.SaveChanges();
               
            }
        }
    }
}