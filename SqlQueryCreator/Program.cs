using System;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using QueryBuilder.Enums;
using QueryBuilder.Models;
using QueryBuilder.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlQueryCreator.Operations;

namespace SqlQueryCreator
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }
        static void Main(string[] args)        
        {
            var option = ReadOptions();
            GenerateQuery(option);
            Console.ReadKey();
        }
        
        private static int ReadOptions()
        {
            Console.WriteLine("select Options 1.Where Clause 2.Join Clause");
            int option = Convert.ToInt32(Console.ReadLine().ToString());
            return option;
        }

        private static void GenerateQuery(int queryType)
        {            
            QueryBuilderService query = new QueryBuilderService();
            QueryGenerator generate = new QueryGenerator();
            if (queryType == 1)
            {
                generate.WhereQuery(query);                
            }
            else
            {
                generate.JoinQuery(query);
            }
        }
        
    }
}
