using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AureportLogScanner.Models;

namespace AureportLogScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfAttack = 0;
            AuthenticationLogsModels auth = new AuthenticationLogsModels();

            Console.WriteLine("Welcome to AureportLogScanner ! We will begin by reading all the files logs... \n\nBeging scanning...\n");

            List<AuthenticationLogsModels> ipList = new List<AuthenticationLogsModels>();

            var filesList = Directory.GetFiles($@"D:\Utilisateurs\Bureau\Logs machine");
            int index = 1;

            foreach (var file in filesList)
            {
                Console.WriteLine($"Scanning file {index}/{filesList.Length}...");
                index++;
                var fileContent = File.ReadAllLines(file);

                foreach (var line in fileContent)
                {
                    var arrayOfWord = line.Split(' ');

                    if (arrayOfWord[0] == "type=USER_AUTH")
                    {
                        // "acct=\"root\""
                        var ip = arrayOfWord[10].Split('=')[1].Split('"')[0];
                        var user = arrayOfWord[7].Split('=')[1].Split('"')[1];

                        if (ipList.Any(t => t.Hostname == ip))
                        {
                            var list = ipList.Where(t => t.Hostname == ip).ToList();

                            var existantIpIndex = ipList.IndexOf(list[0]);

                            ipList.Insert(existantIpIndex, new AuthenticationLogsModels{ Hostname = ip, User = user, Entries = ++ipList[existantIpIndex].Entries });
                            ipList.RemoveAt(existantIpIndex);
                        }
                        else
                        {
                            ipList.Add(new AuthenticationLogsModels
                            {
                                Hostname = ip,
                                Timestamp = "",
                                User = user
                            });
                        }
                        numberOfAttack++;
                    }
                }
            }

            ipList.Sort((a, b) => b.Entries.CompareTo(a.Entries));

            Console.WriteLine("Results : \n\n----------------------------------------------------------------------\n");

            Console.WriteLine($"Total attack : {numberOfAttack}\n");

            for (int i = 0; i < 10; i++)
            {
                // Top 10 attaque SSH
                Console.WriteLine($"{i + 1}. IP : {ipList[i].Hostname} User : {ipList[i].User} Entries : {ipList[i].Entries}");
            }

            Console.WriteLine("\n----------------------------------------------------------------------\n");
        }
    }
}
