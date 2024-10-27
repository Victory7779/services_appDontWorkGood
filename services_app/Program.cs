using Azure.Core;
using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using services_app.Controllers;
using services_app.Models;
using services_app.Services;
using System.Runtime.CompilerServices;
using Dapper;

namespace services_app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton<IServiceUsers,  ServiceUsers>();
            builder.Services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                    );
            });
            builder.Services.AddControllers();
            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            //app.Run(async (context) =>
            //{
            //    var response = context.Response;
            //    var request = context.Request;
            //    var path = request.Path;
            //    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


            //    if (path == "/api/home")// read
            //    {
            //        //await GetAllUser(response, connectionString);
            //    }
            //    else
            //    if (path == "/api/home")//create
            //    {
            //        await CreateUser(response, request, connectionString);
            //    }
            //    else if (path == "/api/home")//update
            //    {
            //        await UpdateUser(response, request, connectionString);
            //    }
            //    else if (path == "/api/home/${id}")//delete
            //    {
            //        await DeleteUser(response, request, connectionString);
            //    }
            //    else//read
            //    {
            //        response.ContentType = "text/html; charset=utf-8";
            //        await response.SendFileAsync("/api/home");
            //    }

            //});

            app.Run();
        }

        private static async Task DeleteUser(HttpResponse response, HttpRequest request, string? connectionString)
        {
            try
            {
                int id = await request.ReadFromJsonAsync<int>();
                User? user = await GetUserId(id, connectionString);
                if (user!=null)
                {
                    using (var db = new SqlConnection(connectionString))
                    {
                        db.Open();
                        string sql = $"" +
                            $"DELETE FROM Users WHERE Id={id}";
                        await db.ExecuteAsync(sql);
                        db.Close();
                    }
                    await response.WriteAsJsonAsync(user);
                }
                else
                {
                    response.StatusCode = 400;
                    await response.WriteAsJsonAsync(new { message = "Корустивач НЕ знайдений!!!" });
                } 
            }
            catch(Exception)
            {
                response.StatusCode = 400;
                await response.WriteAsJsonAsync(new { message = "Некоректні дані!!!" });
            }
        }

        private static async Task UpdateUser(HttpResponse response, HttpRequest request, string? connectionString)
        {
            try
            {
                User? user = await request.ReadFromJsonAsync<User>();
                if (user != null)
                {
                    using (var db = new SqlConnection(connectionString))
                    {
                        db.Open();
                        string sql = $"" +
                            $"UPDATE Users " +
                            $"SET FirstName='{user.FirstName}', LastName='{user.LastName}', EmailName='{user.EmailName}' " +
                            $"WHERE Id={user}";
                        db.Close();
                    }
                    await response.WriteAsJsonAsync(user);
                }
                else
                {
                    response.StatusCode = 400;
                    await response.WriteAsJsonAsync(new { message = "Корустивач НЕ знайдений!!!" });
                }

            }
            catch (Exception)
            {
                response.StatusCode = 400;
                await response.WriteAsJsonAsync(new { message = "Некоректні дані!!!" });
            }
        }

        private static async Task CreateUser(HttpResponse response, HttpRequest request, string? connectionString)
        {
            try
            {
                User? user = await request.ReadFromJsonAsync<User>();
                using (var db = new SqlConnection(connectionString))
                {
                    db.Open();
                    string sql = $"" +
                    $"INSERT User(FirstName, LastName, EmailName) " +
                    $"VALUE ('{user?.FirstName}', '{user?.LastName}', '{user?.EmailName}')";
                    await db.ExecuteAsync(sql);
                    db.Close();
                }
                await response.WriteAsJsonAsync(user);


            }
            catch (Exception)
            {
                response.StatusCode = 400;
                await response.WriteAsJsonAsync(new { message = "Некоректні дані!!!" });
            }
        }

        private static async Task<User>? GetUserId(int id, string? connectionString)
        {
            try
            {
                User? user;
                using (var db = new SqlConnection(connectionString))
                {
                    db.Open();
                    string sql = $"" +
                        $"SELECT * FROM Users WHERE Id={id}";
                    user = await db.QueryFirstAsync<User>(sql);
                    db.Close();
                }
                return user;


            }
            catch (Exception)
            {
                return null;
            }
        }

        private static async Task GetAllUser(HttpResponse response, string? connectionString)
        {
            using(var db=new SqlConnection(connectionString))
            {
                db.Open();
                string sql = $"" +
                    $"SELECT * FROM Users";
                var users = await db.QueryAsync<User>(sql);
                db.Close();
                await response.WriteAsJsonAsync(users);
            }
        }
    }
}
