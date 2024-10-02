using AUCDemo;
using AuctionDemo;
using AuctionDemo;
using HtmlAgilityPack;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

public class Program
{

    public static void Main(string[] args)
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load("https://ineichen.com/auctions/past/");
        AuctionData d=new AuctionData();
        d.FetchData();
    }
}
