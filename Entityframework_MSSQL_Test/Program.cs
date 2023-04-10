// See https://aka.ms/new-console-template for more information
using Entityframework_MSSQL_Test.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Hello, World!");

UserContext dbContext = new(new DbContextOptions(true);