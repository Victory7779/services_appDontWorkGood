using Microsoft.EntityFrameworkCore;
using services_app.Models;

namespace services_app.Services
{
    public interface IServiceUsers
    {
        public UserContext? db { get; set; }
        public IEnumerable<User>? Read();
        public User? Create(User? user);
        public User? Update(User? user);
        public void Delete(int id);
        public User GetUserById(int id);

    }
    public class ServiceUsers : IServiceUsers
    {
        //public List<User>? Users = new List<User>
        //{
        //    new User { Id = 1, FirstName = "John", LastName = "Doe", EmailName = "john.doe@example.com" },
        //    new User { Id = 2, FirstName = "Jane", LastName = "Smith", EmailName = "jane.smith@example.com" },
        //    new User { Id = 3, FirstName = "Michael", LastName = "Johnson", EmailName = "michael.johnson@example.com" },
        //    new User { Id = 4, FirstName = "Emily", LastName = "Davis", EmailName = "emily.davis@example.com" },
        //    new User { Id = 5, FirstName = "Daniel", LastName = "Williams", EmailName = "daniel.williams@example.com" },
        //    new User { Id = 6, FirstName = "Sarah", LastName = "Brown", EmailName = "sarah.brown@example.com" },
        //    new User { Id = 7, FirstName = "David", LastName = "Jones", EmailName = "david.jones@example.com" },
        //    new User { Id = 8, FirstName = "Laura", LastName = "Miller", EmailName = "laura.miller@example.com" },
        //    new User { Id = 9, FirstName = "James", LastName = "Wilson", EmailName = "james.wilson@example.com" },
        //    new User { Id = 10, FirstName = "Olivia", LastName = "Moore", EmailName = "olivia.moore@example.com" }
        //};
        /*
         USE academy
            go
            INSERT INTO Users (FirstName, LastName, EmailName) VALUES
            ('John', 'Doe', 'john.doe@example.com'),
            ('Jane', 'Smith', 'jane.smith@example.com'),
            ('Michael', 'Johnson', 'michael.johnson@example.com'),
            ('Emily', 'Davis', 'emily.davis@example.com'),
            ('Daniel', 'Williams', 'daniel.williams@example.com'),
            ('Sarah', 'Brown', 'sarah.brown@example.com'),
            ('David', 'Jones', 'david.jones@example.com'),
            ('Laura', 'Miller', 'laura.miller@example.com'),
            ('James', 'Wilson', 'james.wilson@example.com'),
            ('Olivia', 'Moore', 'olivia.moore@example.com');
         */
        public UserContext? db { get; set; }
        public User? Create(User? user)
        {
            db?.Users.Add(user);
            return user;
        }
        public void Delete(int id)
        {
            db?.Users.Remove(GetUserById(id));
        }

        public User? GetUserById(int id)
        {
            return db?.Users.Find(id);
        }

        public IEnumerable<User>? Read()
        {
            return db?.Users.ToList();
        }

        public User? Update(User? user)
        {
            db.Entry(user).State = EntityState.Modified;
            return user;
        }
    }
}
