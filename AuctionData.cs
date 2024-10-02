using AuctionDemo;
using HtmlAgilityPack;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace AUCDemo
{
    public class AuctionData
    {
        static string connectionString = "Data Source=DESKTOP-5JGD5JQ;Initial Catalog=Ineichen;Integrated Security=true;Encrypt=True;TrustServerCertificate=True;";

        public void FetchData()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://ineichen.com/auctions/past/");
            List<string> idss = new List<string>();
            var ids = doc.DocumentNode.SelectNodes("//div[@class=\"auction-item__btns\"]/a");
            string pattern1 = @"auctions\/([^\/]*)\/";
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var i = id.GetAttributeValue("href", "");
                    Regex regex = new Regex(pattern1);
                    Match match = regex.Match(i);
                    if (match.Success)
                    {
                        var result = match.Groups[1].Value;
                        if (result != null) idss.Add(result);
                    }
                }
            }

            var titleNodes = doc.DocumentNode.SelectNodes("//h2/a");
            List<string> DES = new List<string>();
            var details = doc.DocumentNode.SelectNodes("//div[@class=\"auction-date-location\"]");

            if (details != null)
            {
                foreach (var detail in details)
                {
                    var linkText = detail.InnerText.Trim();
                    var cleanedText = Regex.Replace(linkText, @"\s+", " ");
                    DES.Add(cleanedText);
                }
            }

            var imgUrls = doc.DocumentNode.SelectNodes("//a[@class=\"auction-item__image\"]/img");


        List<string> lots = new List<string>();
            var lotsizes = doc.DocumentNode.SelectNodes("//div[@class=\"auction-item__btns\"]/a");

            string pattern2 = @"\d{1,}";
            if (lotsizes != null)
            {
                foreach (var lotsize in lotsizes)
                {
                    var lot = lotsize.InnerHtml.Trim();
                    Regex regex = new Regex(pattern2);
                    Match match = regex.Match(lot);
                    if (match.Success)
                    {
                        var result = match.Value;
                        lots.Add(result);
                    }
                }
            }

            var links = doc.DocumentNode.SelectNodes("//div[@class=\"auction-item__btns\"]/a");
            if (links != null)
            {
                foreach (var link in links)
                {

                    var l = link.GetAttributeValue("href", "");


                    if (!string.IsNullOrEmpty(l))
                    {


                    }

                }
            }
  
            List<string> s = new List<string>();
            var dateNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'auction-date-location__item') and not(.//b)]/text()[normalize-space() and not(preceding-sibling::br)] | //div[contains(@class, 'auction-date-location__item')]//b");
            if (dateNodes != null)
            {
                string pattern = @"^\d{1,2}";

                foreach (var linkNode in dateNodes)
                {
                    var linkText = linkNode.InnerText.Trim();
                    Match match = Regex.Match(linkText, pattern);
                    string dateRange = null;

                    if (match.Success)
                    {
                        dateRange = match.Value;
                        s.Add(dateRange);
                    }
                    else
                    {
                        s.Add(null);
                    }
                }
            };
            List<string> sM = new List<string>();
            var startMonths = doc.DocumentNode.SelectNodes("//div[contains(@class, 'auction-date-location__item') and not(.//b)]/text()[normalize-space() and not(preceding-sibling::br)] | //div[contains(@class, 'auction-date-location__item')]//b");
            if (startMonths != null)
            {
                string pattern = @"^(\s+)?\d+\s([A-z]+)";

                foreach (var linkNode in startMonths)
                {
                    var linkText = linkNode.InnerText.Trim();


                    Match match = Regex.Match(linkText, pattern);
                    string dateRange = null;

                    if (match.Success)
                    {
                        dateRange = match.Groups[2].Value;
                        sM.Add(dateRange);
                    }
                    else
                    {
                        sM.Add(null);
                    }
                }
            }
            List<string> sY = new List<string>();
            var startYears = doc.DocumentNode.SelectNodes("//div[contains(@class, 'auction-date-location__item') and not(.//b)]/text()[normalize-space() and not(preceding-sibling::br)] | //div[contains(@class, 'auction-date-location__item')]//b");

            if (startYears != null)
            {
                string pattern = @"(\d{1,2}\s*-\s*[A-Z]+\s*(\d{4}))|\b(\d{4})\b(?=\s*-)";

                foreach (var linkNode in startYears)
                {
                    var linkText = linkNode.InnerText.Trim();


                    Match match = Regex.Match(linkText, pattern);
                    string dateRange = null; // Default to null

                    if (match.Success)
                    {
                        dateRange = match.Value;
                        sY.Add(dateRange);
                    }
                    else
                    {
                        sY.Add(null);
                    }
                }
            }
            List<string> sT = new List<string>();

            var startTimes = doc.DocumentNode.SelectNodes("//div[@class=\"auction-date-location\"]");

            if (startTimes != null)
            {
                string pattern = @"(\d{1,2}:\d{2} (?:CET|\(CET\)))";

                foreach (var linkNode in startTimes)
                {
                    var linkText = linkNode.InnerText.Trim();
                    var matches = Regex.Matches(linkText, pattern);
                    if (matches.Count > 0)
                    {
                        string smallestMatch = matches[0].Value; 
                        foreach (Match match in matches)
                        {
                            if (match.Value.Length < smallestMatch.Length)
                            {
                                smallestMatch = match.Value;
                            }
                        }
                        sT.Add(smallestMatch);
                    }
                    else
                    {
                        sT.Add(null);
                    }
                }
            }
            List<string> e = new List<string>();
            var enddates = doc.DocumentNode.SelectNodes("//div[contains(@class, 'auction-date-location__item')]/b/text() | //div[contains(@class, 'auction-date-location__item')]/br/following-sibling::text()[normalize-space()] | //div[contains(@class, 'auction-date-location__item')][not(b) and not(br)]/text()[normalize-space()]\r\n");

            if (enddates != null)
            {
                string pattern = @"-\s*(\d+)";
                foreach (var linkNode in enddates)
                {
                    var linkText = linkNode.InnerText.Trim();


                    Match match = Regex.Match(linkText, pattern);
                    string dateRange = null; 

                    if (match.Success)
                    {
                        dateRange = match.Groups[1].Value;
                        e.Add(dateRange);
                    }
                    else
                    {
                        e.Add(null);
                    }
                }
            }

            List<string> eM = new List<string>();
            var endMonths = doc.DocumentNode.SelectNodes("//div[contains(@class, 'auction-date-location__item')]/b/text() | //div[contains(@class, 'auction-date-location__item')]/br/following-sibling::text()[normalize-space()] | //div[contains(@class, 'auction-date-location__item')][not(b) and not(br)]/text()[normalize-space()]\r\n");

            if (endMonths != null)
            {
                string pattern = @"-\s*\d+\s([A-Z]+)";
                foreach (var linkNode in endMonths)
                {
                    var linkText = linkNode.InnerText.Trim();
                    Match match = Regex.Match(linkText, pattern);
                    string dateRange = null; 

                    if (match.Success)
                    {
                        dateRange = match.Groups[1].Value;
                        eM.Add(dateRange);
                    }
                    else
                    {
                        eM.Add(null);
                    }
                }
            }
            List<string> eY = new List<string>();
            var endYears = doc.DocumentNode.SelectNodes("//div[contains(@class, 'auction-date-location__item')]/b/text() | //div[contains(@class, 'auction-date-location__item')]/br/following-sibling::text()[normalize-space()] | //div[contains(@class, 'auction-date-location__item')][not(b) and not(br)]/text()[normalize-space()]\r\n");

            if (endYears != null)
            {
                string pattern = @"-\s*\d+\s[A-Z]+\s(\d+)$";
                foreach (var linkNode in endYears)
                {
                    var linkText = linkNode.InnerText.Trim();


                    Match match = Regex.Match(linkText, pattern);
                    string dateRange = null;

                    if (match.Success)
                    {
                        dateRange = match.Groups[1].Value;
                        eY.Add(dateRange);
                    }
                    else
                    {
                        eY.Add(null);
                    }
                }
            }
            List<string> eT = new List<string>();
            var endTimes = doc.DocumentNode.SelectNodes("//div[@class=\"auction-date-location\"]");

            if (endTimes != null)
            {
                string pattern = @"(\d{1,2}:\d{2}\s*CET)(?=\s+[A-Za-z])";
                foreach (var linkNode in endTimes)
                {
                    var linkText = linkNode.InnerText.Trim();


                    Match match = Regex.Match(linkText, pattern);
                    string dateRange = null;

                    if (match.Success)
                    {
                        dateRange = match.Value;
                        eT.Add(dateRange);
                    }
                    else
                    {
                        eT.Add(null);
                    }
                }
            }
            List<string> loc = new List<string>();
            var Locations = doc.DocumentNode.SelectNodes("//div[@class=\"auction-date-location__item\"][2]");

            if (Locations != null)
            {
                foreach (var linkNode in Locations)
                {
                    var linkText = linkNode.InnerText.Trim();
                    loc.Add(linkText);
                }
            }

            List<AuctionModel> auctions = new List<AuctionModel>();
            for (int index = 0; index < idss.Count; index++)
            {
                var a = new AuctionModel
                {
                    Id = CleanString(idss[index]),
                    Title = CleanString(titleNodes[index].InnerText.Trim()),
                    ImageUrl = CleanString(imgUrls[index]?.GetAttributeValue("src", "") ?? string.Empty),
                    Link = CleanString(links[index]?.GetAttributeValue("href", "") ?? string.Empty),
                    Description = CleanString(DES[index]),
                    LotCount = CleanString(lots[index]),          
                    StartDate = CleanString(s[index]),
                    StartMonth = CleanString(sM[index]),
                    StartYear = CleanString(sY[index]),
                    StartTime = CleanString(sT[index]),
                    EndDate = CleanString(e[index]),
                    EndMonth = CleanString(eM[index]),
                    EndYear = CleanString(eY[index]),
                    EndTime = CleanString(eT[index]),
                    Location = CleanString(loc[index])
                };

                if (AuctionExists(a.Id))
                {
                    UpdateAuctionInDatabase(a);
                }
                else
                {
                    SaveAuctionToDatabase(a);
                }
                auctions.Add(a);
            }
        }

        public string CleanString(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            return Regex.Replace(input.Trim(), @"\s+", " ");
        }

        public void SaveAuctionToDatabase(AuctionModel auction)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "PR_Insert_Auction";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", auction.Id);
                    Console.WriteLine($"Extracted ID: '{auction.Id}'");
                    command.Parameters.AddWithValue("@Title", auction.Title);
                    Console.WriteLine($"Extracted Title: '{auction.Title}'");
                    command.Parameters.AddWithValue("@Description", auction.Description);
                    Console.WriteLine($"Extracted Description: '{auction.Description}'");
                    command.Parameters.AddWithValue("@ImageUrl", auction.ImageUrl);
                    Console.WriteLine($"Extracted ImageUrl: '{auction.ImageUrl}'");
                    command.Parameters.AddWithValue("@Link", auction.Link);
                    Console.WriteLine($"Extracted Link: '{auction.Link}'");
                    command.Parameters.AddWithValue("@LotCount", auction.LotCount);
                    Console.WriteLine($"Extracted LotCount: '{auction.LotCount}'");
                    command.Parameters.AddWithValue("@StartDate", auction.StartDate);
                    Console.WriteLine($"Extracted StartDate: '{auction.StartDate}'");
                    command.Parameters.AddWithValue("@StartMonth", auction.StartMonth);
                    Console.WriteLine($"Extracted StartMonth: '{auction.StartMonth}'");
                    command.Parameters.AddWithValue("@StartYear", auction.StartYear);
                    Console.WriteLine($"Extracted StartYear: '{auction.StartYear}'");
                    command.Parameters.AddWithValue("@StartTime", auction.StartTime);
                    Console.WriteLine($"Extracted StartTime: '{auction.StartTime}'");
                    command.Parameters.AddWithValue("@EndDate", auction.EndDate);
                    Console.WriteLine($"Extracted EndDate: '{auction.EndDate}'");
                    command.Parameters.AddWithValue("@EndMonth", auction.EndMonth);
                    Console.WriteLine($"Extracted EndMonth: '{auction.EndMonth}'");
                    command.Parameters.AddWithValue("@EndYear", auction.EndYear);
                    Console.WriteLine($"Extracted EndYear: '{auction.EndYear}'");
                    command.Parameters.AddWithValue("@EndTime", auction.EndTime);
                    Console.WriteLine($"Extracted EndTime: '{auction.EndTime}'");
                    command.Parameters.AddWithValue("@Location", auction.Location);
                    Console.WriteLine($"Extracted Location: '{auction.Location}'");

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateAuctionInDatabase(AuctionModel auction)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "PR_Update_Auction";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    Console.WriteLine("updated data");
                    command.Parameters.AddWithValue("@Id", auction.Id);
                    Console.WriteLine($"Extracted ID: '{auction.Id}'");
                    command.Parameters.AddWithValue("@Title", auction.Title);
                    Console.WriteLine($"Extracted Title: '{auction.Title}'");
                    command.Parameters.AddWithValue("@Description", auction.Description);
                    Console.WriteLine($"Extracted Description: '{auction.Description}'");
                    command.Parameters.AddWithValue("@ImageUrl", auction.ImageUrl);
                    Console.WriteLine($"Extracted ImageUrl: '{auction.ImageUrl}'");
                    command.Parameters.AddWithValue("@Link", auction.Link);
                    Console.WriteLine($"Extracted Link: '{auction.Link}'");
                    command.Parameters.AddWithValue("@LotCount", auction.LotCount);
                    Console.WriteLine($"Extracted LotCount: '{auction.LotCount}'");
                    command.Parameters.AddWithValue("@StartDate", auction.StartDate);
                    Console.WriteLine($"Extracted StartDate: '{auction.StartDate}'");
                    command.Parameters.AddWithValue("@StartMonth", auction.StartMonth);
                    Console.WriteLine($"Extracted StartMonth: '{auction.StartMonth}'");
                    command.Parameters.AddWithValue("@StartYear", auction.StartYear);
                    Console.WriteLine($"Extracted StartYear: '{auction.StartYear}'");
                    command.Parameters.AddWithValue("@StartTime", auction.StartTime);
                    Console.WriteLine($"Extracted StartTime: '{auction.StartTime}'");
                    command.Parameters.AddWithValue("@EndDate", auction.EndDate);
                    Console.WriteLine($"Extracted EndDate: '{auction.EndDate}'");
                    command.Parameters.AddWithValue("@EndMonth", auction.EndMonth);
                    Console.WriteLine($"Extracted EndMonth: '{auction.EndMonth}'");
                    command.Parameters.AddWithValue("@EndYear", auction.EndYear);
                    Console.WriteLine($"Extracted EndYear: '{auction.EndYear}'");
                    command.Parameters.AddWithValue("@EndTime", auction.EndTime);
                    Console.WriteLine($"Extracted EndTime: '{auction.EndTime}'");
                    command.Parameters.AddWithValue("@Location", auction.Location);
                    Console.WriteLine($"Extracted Location: '{auction.Location}'");

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public bool AuctionExists(string auctionId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM Auctions WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", auctionId);
                    connection.Open();
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
    }
}
