using IrcDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace twitch_spam
{
    class Program
    {
        static List<String> accounts = new List<String>();
        static List<String> messages = new List<String>();
        static Random rnd = new Random();
        static string victim;

        static void send_twitch_message(string account, string message)
        {
            var server = "irc.twitch.tv";

            var username = account.Split('#')[0];
            var oauth = account.Split('#')[1];


            var client = new IrcDotNet.TwitchIrcClient();
            client.Connect(server, false, new IrcUserRegistrationInfo(){ NickName = username,    Password = oauth,  UserName = username });


            client.SendRawMessage("PASS " + oauth + "\n");
            client.SendRawMessage("NICK " + username + "\n");
            client.SendRawMessage("JOIN #" + victim + "\n");


            string say = ":USER!USER@USER.tmi.twitch.tv PRIVMSG #CHANNEL :MESSAGE";
            say = say.Replace("USER", username);
            say = say.Replace("CHANNEL", victim);
            say = say.Replace("MESSAGE", message);
            client.SendRawMessage(say);

      

        }


    


        static void Main(string[] args)
        {
            Console.Title = "Twitch Spammer v0.1 by Brian";

         

            try
            {   
                using (StreamReader sr = new StreamReader("./config.txt",Encoding.Default))
                {
                    String line;


                    while(  (line = sr.ReadLine()) != null)
                    {

                        if (!line.StartsWith("#"))
                        {
                            if (line.Contains("account="))
                            {
                                string account = line.Split('=')[1];

                                string name = account.Split('#')[0];
                                string oauth = account.Split('#')[1];

                                Console.WriteLine("[Account Added]: " + name);
                                accounts.Add(account);

                            }

                            if (line.Contains("message="))
                            {
                                string message = line.Split('=')[1];
                                Console.WriteLine("[Message Added]: " + message);
                                messages.Add(message);
                            }

                        }

                       
                    }

                 

                    //Console.WriteLine(line);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("[Error]: Config file not found!");
                Console.WriteLine(e.Message);
                System.Environment.Exit(1);
            }

            Console.WriteLine("");

            Console.WriteLine("[INFO]: Found " + accounts.Count + " Accounts!");
            Console.WriteLine("[INFO]: Found " + messages.Count + " Messages!");

            Console.WriteLine("");

            Console.Write("Enter Twitch Name: ");
            victim = Console.ReadLine();
            Console.WriteLine("victim => " + victim);


            while (true)
            {

                send_twitch_message(accounts[rnd.Next(accounts.Count)], messages[rnd.Next(messages.Count)]);
                System.Threading.Thread.Sleep(80);
            }

            


            Console.Read();

        }
    }
}
