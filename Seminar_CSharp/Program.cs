using Seminar_CSharp.Class;

namespace Seminar_CSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {


                Project project = new Project { Name = "Project1" };
                User vlad = new User { Name = "Vlad", Surname = "Malashchuk", NickName = "xmalas04", Photo = "123"};
                db.Users.Add(vlad);
                db.SaveChanges();
                var users = db.Users.ToList();
                Console.WriteLine("List of Objects:");
                foreach (User u in users)
                {
                    Console.WriteLine($"{u.UserId}.{u.Name} - {u.Surname}");
                }
            }
        }
    }
}