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
                User vlad = new User { FirstName = "Vlad", SecondName = "Malashchuk", NickName = "xmalas04", Foto = "123"};
                db.Users.Add(vlad);
                db.SaveChanges();
                var users = db.Users.ToList();
                Console.WriteLine("List of Objects:");
                foreach (User u in users)
                {
                    Console.WriteLine($"{u.Id}.{u.FirstName} - {u.SecondName}");
                }
            }
        }
    }
}