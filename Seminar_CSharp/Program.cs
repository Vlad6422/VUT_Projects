using Seminar_CSharp.Class;

namespace Seminar_CSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Tests for Db
            using (ApplicationContext db = new ApplicationContext())
            {

                Activity faze1 = new Activity {Start = DateTime.UtcNow, Description = "Project delame1" };
                Activity faze2 = new Activity { Start = DateTime.Now, Description = "Project delame2" };
                Activity faze3 = new Activity { Start = DateTime.Now, Description = "Project delame3" };
                db.Activities.AddRange(faze1,faze2,faze3);

                User Vlad = new User { Name = "Vlad", Surname = "Malashchuk", NickName = "xmalas04", Photo = "111", ShowNickName=true  };
                User Kate = new User { Name = "Kate", Surname = "AvadaKedabra", NickName = "xsasu01", Photo = "222", ShowNickName = false };
                User Steve = new User { Name = "Steve", Surname = "Potter", NickName = "xxyilo28", Photo = "333", ShowNickName = true };
                db.Users.AddRange(Vlad, Kate, Steve);  
                
                Project IDM = new Project { Name = "IDM", Description="Project of IDM" };
                Project ILG = new Project { Name = "ILG", Description = "Project of ILG" };
                Project IMA1 = new Project { Name = "IMA1", Description = "Project of IMA1" };
                db.Projects.AddRange(IDM,ILG, IMA1);
             //   project.Activities.Add(activity);
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
                var users = db.Users.ToList();
                Console.WriteLine("List of Objects:");
                foreach (User u in users)
                {
                    Console.WriteLine($"{u.UserId}.{u.Name} - {u.Projects.First().Name}");
                }
            }
        }
    }
}