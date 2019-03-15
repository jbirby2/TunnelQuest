using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Core.Migrations
{
    public partial class InsertChatLogData : Migration
    {
        public void STUB()
        {
            this.Up(null);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            try
            {
                Console.WriteLine("Inserting data from in-game chat logs.  This will take a long time...  be patient and don't exit early!");
            }
            catch { }

            using (var context = new TunnelQuestContext())
            {
                var authToken = context.AuthTokens.FirstOrDefault(token => token.Name == "default").Value;
                var chatLogic = new ChatLogic(context);

                var assembly = typeof(InsertChatLogData).GetTypeInfo().Assembly;

                int i = 1;
                bool isDone = false;
                do
                {
                    using (var stream = assembly.GetManifestResourceStream("TunnelQuest.Core.Migrations.Data.EcTunnelChatLogs.eqlog_project1999_" + i.ToString() + ".txt"))
                    {
                        try
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                // STUB
                                int stubCount = 0;
                                DateTime lastLoggedTime = DateTime.Now;

                                while (reader.EndOfStream == false && isDone == false)
                                {
                                    string logLineText = reader.ReadLine();

                                    string chatLineText = logLineText.Substring(27);
                                    string[] chatLineWords = chatLineText.Split(' ');

                                    if (chatLineWords.Length > 2 && chatLineWords[1] == "auctions,")
                                    {
                                        string timeStampString = logLineText.Substring(1, 24);
                                        string[] timeStampParts = timeStampString.Split(' ');

                                        int month;
                                        switch (timeStampParts[1])
                                        {
                                            case "Jan":
                                                month = 1;
                                                break;
                                            case "Feb":
                                                month = 2;
                                                break;
                                            case "Mar":
                                                month = 3;
                                                break;
                                            case "Apr":
                                                month = 4;
                                                break;
                                            case "May":
                                                month = 5;
                                                break;
                                            case "Jun":
                                                month = 6;
                                                break;
                                            case "Jul":
                                                month = 7;
                                                break;
                                            case "Aug":
                                                month = 8;
                                                break;
                                            case "Sep":
                                                month = 9;
                                                break;
                                            case "Oct":
                                                month = 10;
                                                break;
                                            case "Nov":
                                                month = 11;
                                                break;
                                            case "Dec":
                                                month = 12;
                                                break;
                                            default:
                                                throw new Exception("Unknown month '" + timeStampParts[1] + "'");
                                        }

                                        int day = Int32.Parse(timeStampParts[2]);
                                        int year = Int32.Parse(timeStampParts[4]);

                                        string[] timeParts = timeStampParts[3].Split(':');
                                        int hour = Int32.Parse(timeParts[0]);
                                        int minute = Int32.Parse(timeParts[1]);
                                        int second = Int32.Parse(timeParts[2]);

                                        DateTime chatLineTimestamp = new DateTime(year, month, day, hour, minute, second).AddHours(6); // +6 hours to convert the log file times from Central Standard Time to UTC

                                        var result = chatLogic.ProcessLogLine(authToken, ServerCodes.Blue, chatLineText, chatLineTimestamp);

                                        // STUB
                                        if (stubCount > 5000)
                                        {
                                            Console.WriteLine("STUB ended chat log import early for debugging purposes STUB");
                                            isDone = true;
                                            break;
                                        }
                                        else
                                        {
                                            stubCount++;
                                            if (stubCount % 100 == 0)
                                                Console.WriteLine("[" + DateTime.Now.ToString() + ", took " + (DateTime.Now - lastLoggedTime).TotalSeconds.ToString() + " seconds] " + stubCount.ToString());
                                        }
                                        // END STUB

                                    }
                                } // end while (!endOfStream)
                            }// end using (reader)

                            i++;
                        }
                        catch (ArgumentNullException)
                        {
                            isDone = true;
                        }
                    }// end using(stream)
                }
                while (!isDone);
            } // end using (context)

            try
            {
                Console.WriteLine("Finished inserting data from in-game chat logs.");
            }
            catch { }

        } // end function Up()

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // STUB TODO
        }
    }
}
