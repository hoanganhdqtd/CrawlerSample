using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;

namespace ConsoleApplication1
{

    class StockDetails : IComparable<StockDetails>
    {
        public string StockSymbol { get; private set; }
        public string EquityName { get; private set; }
        public decimal PriorPrice { get; private set; }
        public decimal FloorPrice { get; private set; }
        public decimal CeilingPrice { get; private set; }
        public decimal Session1Price { get; set; }
        public Int64 Session1Qtty { get; set; }
        public decimal Session2Price { get; set; }
        public Int64 Session2Qtty { get; set; }
        public decimal BidP1 { get; set; }
        public Int64 BidV1 { get; set; }
        public decimal BidP2 { get; set; }
        public Int64 BidV2 { get; set; }
        public decimal BidP3 { get; set; }
        public Int64 BidV3 { get; set; }
        public decimal MatchPrice { get; set; }
        public Int64 MatchQtty { get; set; }
        public decimal Change { get; set; }
        public decimal OfferP1 { get; set; }
        public Int64 OfferV1 { get; set; }
        public decimal OfferP2 { get; set; }
        public Int64 OfferV2 { get; set; }
        public decimal OfferP3 { get; set; }
        public Int64 OfferV3 { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        
        public decimal AvgPrice { get; set; }
        
        public Int64 TotalQtty { get; set; }
        public Int64 FBuyQtty { get; set; }
        public Int64 FCurrentRoom { get; set; }
        public Int64 FSellQtty { get; set; }







        public StockDetails(string stockSym, string equityName, decimal ceil, decimal floor, decimal prior, decimal session1Price, Int64 session1Qtty, decimal session2Price,
            Int64 session2Qtty, decimal bidP1, Int64 bidV1, decimal bidP2, Int64 bidV2, decimal bidP3, Int64 bidV3, decimal matchPrice, Int64 matchQtty, 
            decimal change, decimal offerP1, Int64 offerV1, decimal offerP2, Int64 offerV2, decimal offerP3, Int64 offerV3, decimal openPrice, 
            decimal highPrice, decimal lowPrice, decimal avgPrice, 
            Int64 totalQtty, Int64 fBuyQtty, Int64 fCurrentRoom, Int64 fSellQtty)
        {
            StockSymbol = stockSym;
            EquityName = equityName;
            CeilingPrice = ceil;
            FloorPrice = floor;
            PriorPrice = prior;

            Session1Price = session1Price;
            Session1Qtty = session1Qtty;
            Session2Price = session2Price;
            Session2Qtty = session2Qtty;

            BidP1 = bidP1;
            BidV1 = bidV1;
            BidP2 = bidP2;
            BidV2 = bidV2;
            BidP3 = bidP3;
            BidV3 = bidV3;
            MatchPrice = matchPrice;
            MatchQtty = matchQtty;
            Change = change;
            OfferP1 = offerP1;
            OfferV1 = offerV1;
            OfferP2 = offerP2;
            OfferV2 = offerV2;
            OfferP3 = offerP3;
            OfferV3 = offerV3;
            OpenPrice = openPrice;
            HighPrice = highPrice;
            LowPrice = lowPrice;

            AvgPrice = avgPrice;

            TotalQtty = totalQtty;
            FBuyQtty = fBuyQtty;
            FCurrentRoom = fCurrentRoom;
            FSellQtty = fSellQtty;
        }

        public decimal Percent() // MatchPrice - PriorPrice
        {
            if (MatchPrice == 0 || PriorPrice == 0)
                return MatchPrice;
            else return MatchPrice - PriorPrice;
        }

        public override string ToString()
        {
            return String.Format(@"sym: {0}, equity: {1}, ceil: {2}, floor: {3}, prior: {4}, session1Price: {5}, session1Qtty: {6}, session2Price: {7}, session2Qtty: {8}, 
                                    bidV3: {9}, bidP3: {10}, bidV2: {11}, bidP2: {12}, bidV1: {13}, bidP1: {14}, matchPrice: {15}, matchQtty: {16}, percent: {17}, 
                                    offerP1: {18}, offerV1: {19}, offerP2: {20}, offerV2: {21}, offerP3: {22}, offerV3: {23}, openPrice: {24}, 
                                    highPrice: {25}, lowPrice: {26}, avgPrice: {27}, totalQtty: {28}, fBuyQtty: {29}, fCurrentRoom: {30}, fSellQtty: {31}",
                                    StockSymbol, EquityName, CeilingPrice, FloorPrice, PriorPrice, Session1Price, Session1Qtty, Session2Price, Session2Qtty,
                                    BidV3, BidP3, BidV2, BidP2, BidV1, BidP3, MatchPrice, MatchQtty, Change, OfferP1, OfferV1, OfferP2, OfferV2,
                                    OfferP3, OfferV3, OpenPrice, HighPrice, LowPrice, AvgPrice, 
                                    TotalQtty, FBuyQtty, FCurrentRoom, FSellQtty);
        }

        public int CompareTo(StockDetails other)
        {
            return String.Compare(this.StockSymbol, other.StockSymbol);
        }
    }

    public static class StringExt
    {

        public static string SubstringJava(this string self, int startPos, int endPos)
        {
            return self.Substring(startPos, endPos - startPos);
        }
    }
    

    class Program
    {
        // for null value
        public static decimal getFromStringToken(String stringtok)
        {
            var result = (stringtok != null && stringtok != "") ? decimal.Parse(stringtok) : 0;
            return result;
        }


        static void Main(string[] args)
        {
            
            CrawlData();
        }

        //public static async void CrawlData()
        //{
        //    while (true)
        //    {
        //        GetData();
        //        await Task.Delay(TimeSpan.FromMinutes(1));
        //    }
        //}

        public static void CrawlData()
        {
            Thread t = new Thread(new ParameterizedThreadStart(ThreadLoop));

            //t.Start((Action)GetDataFromFpts);
            t.Start();

            //GetDataFromFpts();
        }

        public static void ThreadLoop(object callback)
        {
            while (true)
            {
                GetDataFromFpts();
                
                //((Delegate)callback).DynamicInvoke(null);
                Thread.Sleep(10000);
            }
        }

         
        public static void GetDataFromFpts()
        {
            
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create("http://priceboard.fpts.com.vn/hsx/data.ashx?s=quote&l=All");
            

            // Set the Method property of the request to POST.
            request.Method = "GET";
            

            // Set the ContentType property of the WebRequest.
            request.ContentType = "text/plain; charset=utf-8";

            
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            if ((((HttpWebResponse)response).StatusDescription).Equals("OK"))
            {
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer);
                //Console.ReadLine();
                
                // Clean up the streams.
                reader.Close();
                dataStream.Close();

                
                var linetokens = responseFromServer.Substring(1, responseFromServer.Length - 2).Split(new string[] { "},{" }, StringSplitOptions.None);
                

                for (int i = 0; i < linetokens.Length - 1; i++)
                {
                    linetokens[i] = linetokens[i] + "}";
                }
                for (int i = 1; i < linetokens.Length; i++)
                {
                    linetokens[i] = "{" + linetokens[i];
                }
                
                //Console.ReadLine();
                var StockDict = new Dictionary<String, StockDetails>();

                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = @"workstation id=StockTrainer.mssql.somee.com;
                packet size=4096;
                user id=lmtri1995_SQLLogin_1;
                pwd =cu1mfemumv;
                data source=StockTrainer.mssql.somee.com;
                persist security info=False;
                initial catalog=StockTrainer";

                Dictionary<String, String> EquityNameDict;
                try
                {
                    // get company name
                    EquityNameDict = GetCompanyNameFromVietstock();
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("Network error: " + e.ToString());
                    // get company name
                    EquityNameDict = GetCompanyNameFromBanggia2();
                }

                //Console.ReadLine();

                foreach (var linetok in linetokens)
                {
                    JObject linejson = JObject.Parse(linetok);

                    //var Ticker = linejson["Info"][0][1].ToObject<string>();
                    var StockSymbol = linejson["Info"][0][1].ToObject<string>();
                    var EquityName = EquityNameDict[StockSymbol];
                    var PrevClosePrice = linejson["Info"][1][1].ToObject<decimal>();
                    var Ceiling = linejson["Info"][2][1].ToObject<decimal>();
                    var Floor = linejson["Info"][3][1].ToObject<decimal>();
                    var BidPrice3 = linejson["Info"][5][1].ToObject<decimal>();
                    var BidVol3 = linejson["Info"][6][1].ToObject<Int64>();
                    var BidPrice2 = linejson["Info"][7][1].ToObject<decimal>();
                    var BidVol2 = linejson["Info"][8][1].ToObject<Int64>();
                    var BidPrice1 = linejson["Info"][9][1].ToObject<decimal>();
                    var BidVol1 = linejson["Info"][10][1].ToObject<Int64>();
                    var MatchPrice = linejson["Info"][11][1].ToObject<decimal>();
                    var MatchVol = linejson["Info"][12][1].ToObject<Int64>();
                    var Change = linejson["Info"][13][1].ToObject<decimal>();
                    var AskPrice1 = linejson["Info"][14][1].ToObject<decimal>();
                    var AskVol1 = linejson["Info"][15][1].ToObject<Int64>();
                    var AskPrice2 = linejson["Info"][16][1].ToObject<decimal>();
                    var AskVol2 = linejson["Info"][17][1].ToObject<Int64>();
                    var AskPrice3 = linejson["Info"][18][1].ToObject<decimal>();
                    var AskVol3 = linejson["Info"][19][1].ToObject<Int64>();
                    var TotalVolume = linejson["Info"][21][1].ToObject<Int64>();
                    var OpenPrice = linejson["Info"][22][1].ToObject<decimal>();
                    var HighestPrice = linejson["Info"][23][1].ToObject<decimal>();
                    var LowestPrice = linejson["Info"][24][1].ToObject<decimal>();

                    var FrgnBuy = linejson["Info"][26][1].ToObject<Int64>();
                    var FrgnSell = linejson["Info"][27][1].ToObject<Int64>();
                    var RoomLeft = linejson["Info"][28][1].ToObject<Int64>();
                    try
                    {
                        StockDict.Add(StockSymbol, new StockDetails(StockSymbol, EquityName, Ceiling, Floor, PrevClosePrice, 0, 0,
                            0, 0, BidPrice1, BidVol1, BidPrice2, BidVol2, BidPrice3, BidVol3, MatchPrice, MatchVol, Change, AskPrice1, AskVol1, AskPrice2, AskVol2,
                            AskPrice3, AskVol3, OpenPrice, HighestPrice, LowestPrice, 0, TotalVolume, FrgnBuy, RoomLeft, FrgnSell));
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Opps error at line: " + linetok + " " + e.ToString());
                    }

                }

                foreach (var stockdetailpair in StockDict)
                {
                    Console.WriteLine(stockdetailpair.Value.ToString());
                    Console.WriteLine();
                }
                


                //InsertStockTable(StockDict);
                UpdateStockPrice(StockDict);
                InsertPriceHistory(StockDict);
            }
            else
            {
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            }            
            response.Close();

            
            
        }
 


        public static Dictionary<String, String> GetCompanyNameFromVietstock()
        {
            
            //WebRequest request = WebRequest.Create("http://banggia2.ssi.com.vn/AjaxWebService.asmx/GetDataHoseStockList");
            WebRequest request = WebRequest.Create("http://banggia.vietstock.vn/StockHandler.ashx?option=init&getVersion=-1&IndexCode=VNINDEX&catid=1");
            request.Method = "POST";

            // Create POST data and convert it to a byte array.
            //string postData = "This is a test that posts this string to a Web server.";
            //byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json; charset=UTF-8";
            // Set the ContentLength property of the WebRequest.
            //request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            //dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            //StreamReader reader = new StreamReader(dataStream);
            StreamReader reader = new StreamReader(dataStream, Encoding.UTF8, true);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            //Console.ReadLine();
            // Clean up the streams.

            reader.Close();
            dataStream.Close();
            response.Close();

            
            var linetokens = responseFromServer.Substring(1, responseFromServer.Length - 2).Split(new string[] { "},{" }, StringSplitOptions.None);
            var StockName = new Dictionary<String, String>();

            for (int i = 0; i < linetokens.Length - 1; i++)
            {
                linetokens[i] = linetokens[i] + "}";
            }
            for (int i = 1; i < linetokens.Length; i++)
            {
                linetokens[i] = "{" + linetokens[i];
            }

            foreach (var linetok in linetokens)
            {
                JObject linejson = JObject.Parse(linetok);
                
                StockName[linejson["ST"].ToObject<String>()] = linejson["SN"].ToObject<String>();
            }
            
            //Console.WriteLine();
            return StockName;
        }

        // get company name from bangia2
        public static Dictionary<String, String> GetCompanyNameFromBanggia2()
        {
            WebRequest request = WebRequest.Create("http://banggia2.ssi.com.vn/AjaxWebService.asmx/GetDataHoseStockList");

            // Set the request method
            request.Method = "POST";

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json; charset=UTF-8";

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            dataStream.Close();

            // Get the response.
            WebResponse response = request.GetResponse();

            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            if (((HttpWebResponse)response).StatusDescription.Equals("OK"))
            {
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();

                var linetokens = responseFromServer.Substring(6, responseFromServer.Length - 11).Split(new String[] { "|0|#" }, StringSplitOptions.None);
                var StockName = new Dictionary<String, String>();

                foreach (var linetok in linetokens)
                {
                    StockName[linetok.Split('|')[0]] = linetok.Split('|')[1];
                }

                return StockName;
            }
            else
            {
                dataStream.Close();
                response.Close();
                throw new InvalidOperationException("Network error, not 200 OK");
            }

        }

        // insert new stock
        private static void InsertStockTable (Dictionary<String, StockDetails> StockDict)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"workstation id=StockTrainer.mssql.somee.com;
                packet size=4096;
                user id=lmtri1995_SQLLogin_1;
                pwd =cu1mfemumv;
                data source=StockTrainer.mssql.somee.com;
                persist security info=False;
                initial catalog=StockTrainer";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;                

                // insert stock
                foreach (var stock in StockDict)
                {
                    // check if stock exists in the database
                    // if no, insert new stock
                    cmd.CommandText = String.Format(@"SELECT COUNT(*) FROM dbo.Stock WHERE Ticker LIKE '{0}'", stock.Value.StockSymbol);
                    int tickerCount = (int)cmd.ExecuteScalar();

                    if (tickerCount < 1)
                    {
                        cmd.CommandText = String.Format(@"
                        INSERT INTO dbo.Stock
                        (Ticker,
                         EquityName, 
                         Price,
                         PrevClosePrice,
                         HighPrice,
                         LowPrice,
                         OpenPrice,   
                         Volume,
                         Change,
                         MarketCap,
                         [52-week_High],
                         [52-week_Low],   
                         AskPrice,
                         BidPrice,
                         AskSize,
                         BidSize                         
                        )
                        VALUES ('{0}', N'{1}', {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15})",
                        stock.Value.StockSymbol, stock.Value.EquityName, stock.Value.MatchPrice, stock.Value.PriorPrice, stock.Value.HighPrice,
                        stock.Value.LowPrice, stock.Value.OpenPrice, 0, stock.Value.MatchPrice - stock.Value.PriorPrice, 0, 0, 0, stock.Value.OfferP1, stock.Value.BidP1,
                        stock.Value.OfferV1, stock.Value.BidV1);


                        Console.WriteLine("Inserting: " + stock.Value.StockSymbol);
                        Console.WriteLine("SQL: " + cmd.CommandText);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("OK: " + stock.Value.StockSymbol);

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                conn.Close();
            }
        } 

        public static void InsertPriceHistory(Dictionary<String, StockDetails> StockDict)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"workstation id=StockTrainer.mssql.somee.com;
                packet size=4096;
                user id=lmtri1995_SQLLogin_1;
                pwd =cu1mfemumv;
                data source=StockTrainer.mssql.somee.com;
                persist security info=False;
                initial catalog=StockTrainer";
            try
            {
                conn.Open();
                //SqlCommand cmd = new SqlCommand();
                SqlCommand cmd;
                //cmd.Connection = conn;
               
                // insert stock
                foreach (var stock in StockDict)
                {
                    cmd = new SqlCommand("INSERT INTO dbo.History(Ticker, Time, HistoryPrice) VALUES (@parameter1, @parameter2, @parameter3)", conn);
                    cmd.Parameters.AddWithValue("@parameter1", stock.Value.StockSymbol);
                    cmd.Parameters.AddWithValue("@parameter2", DateTime.Now);
                    cmd.Parameters.AddWithValue("@parameter3", stock.Value.MatchPrice);


                    Console.WriteLine("Inserting price: " + stock.Value.StockSymbol);
                    Console.WriteLine("SQL: " + cmd.CommandText);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("OK: " + stock.Value.StockSymbol);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        
        public static void UpdateStockPrice(Dictionary<String, StockDetails> StockDict)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"workstation id=StockTrainer.mssql.somee.com;
                packet size=4096;
                user id=lmtri1995_SQLLogin_1;
                pwd =cu1mfemumv;
                data source=StockTrainer.mssql.somee.com;
                persist security info=False;
                initial catalog=StockTrainer";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                // insert stock
                foreach (var stock in StockDict)
                {
                    cmd.CommandText = String.Format(@"UPDATE dbo.Stock SET Price = {0},
                                                                           PrevClosePrice = {1},
                                                                           HighPrice = {2},
                                                                           LowPrice = {3},
                                                                           OpenPrice = {4},
                                                                           Volume = {5},
                                                                           Change = {6},
                                                                           MarketCap = {7},
                                                                           [52-week_High] = {8},
                                                                           [52-week_Low] = {9},
                                                                           AskPrice = {10},
                                                                           BidPrice = {11},
                                                                           AskSize = {12},
                                                                           BidSize = {13}                                                                         
                                                        WHERE Ticker = '{14}'", stock.Value.MatchPrice,
                                                                           stock.Value.PriorPrice,
                                                                           stock.Value.HighPrice,
                                                                           stock.Value.LowPrice,
                                                                           stock.Value.OpenPrice,
                                                                           0,
                                                                           stock.Value.MatchPrice - stock.Value.PriorPrice,
                                                                           0,
                                                                           0,
                                                                           0,
                                                                           stock.Value.OfferP1,
                                                                           stock.Value.BidP1,
                                                                           stock.Value.OfferV1,
                                                                           stock.Value.BidV1,
                                                                           stock.Value.StockSymbol);


                    Console.WriteLine("Updating: " + stock.Value.StockSymbol);
                    Console.WriteLine("SQL: " + cmd.CommandText);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("OK: " + stock.Value.StockSymbol);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private static IEnumerable<StockDetails> GetDataFromBanggia2()
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create("http://banggia2.ssi.com.vn/AjaxWebService.asmx/GetHoseStockQuoteInit");

            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            //string postData = "This is a test that posts this string to a Web server.";
            //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json; charset=UTF-8";
            // Set the ContentLength property of the WebRequest.
            //request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            //dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();

            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            Console.ReadLine();
            // Clean up the streams.

            reader.Close();
            dataStream.Close();
            response.Close();

            
            var linetokens = responseFromServer.Substring(15, responseFromServer.Length - 17).Split('#');
            
            var StockDict = new Dictionary<String, StockDetails>();
            var EquityNames = GetCompanyNameFromVietstock();

            foreach (var linetok in linetokens)
            {
                Console.WriteLine(linetok);
                var stocktoks = linetok.Split('|');
                var stocksym = stocktoks[0];

                try
                {
                    var ceil = getFromStringToken(stocktoks[1]);
                    var floor = getFromStringToken(stocktoks[2]);
                    var prior = getFromStringToken(stocktoks[3]);

                    var stocksession1price = getFromStringToken(stocktoks[4]);
                    var stocksession1qtty = (Int64)getFromStringToken(stocktoks[5]);

                    var stocksession2price = getFromStringToken(stocktoks[6]);
                    var stocksession2qtty = (Int64)getFromStringToken(stocktoks[7]);

                    var bidP1 = getFromStringToken(stocktoks[8]);
                    var bidV1 = (Int64)getFromStringToken(stocktoks[9]);
                    var bidP2 = getFromStringToken(stocktoks[10]);
                    var bidV2 = (Int64)getFromStringToken(stocktoks[11]);
                    var bidP3 = getFromStringToken(stocktoks[12]);
                    var bidV3 = (Int64)getFromStringToken(stocktoks[13]);

                    var matchPrice = getFromStringToken(stocktoks[14]);
                    var matchQtty = (Int64)getFromStringToken(stocktoks[15]);
                    // var percent = (matchPrice - stockprior);

                    var offerP1 = getFromStringToken(stocktoks[16]);
                    var offerV1 = (Int64)getFromStringToken(stocktoks[17]);
                    var offerP2 = getFromStringToken(stocktoks[18]);
                    var offerV2 = (Int64)getFromStringToken(stocktoks[19]);
                    var offerP3 = getFromStringToken(stocktoks[20]);
                    var offerV3 = (Int64)getFromStringToken(stocktoks[21]);

                    var openPrice = 0;
                    var highPrice = getFromStringToken(stocktoks[22]);
                    var lowPrice = getFromStringToken(stocktoks[23]);
                    var avgPrice = getFromStringToken(stocktoks[24]);
                    var totalQtty = (Int64)getFromStringToken(stocktoks[25]);

                    var fBuyQtty = (Int64)getFromStringToken(stocktoks[26]);
                    var fCurrentRoom = (Int64)getFromStringToken(stocktoks[27]);
                    var fSellQtty = (Int64)getFromStringToken(stocktoks[28]);

                    StockDict.Add(stocksym, new StockDetails(stocksym, EquityNames[stocksym], ceil, floor, prior, stocksession1price, stocksession1qtty,
                        stocksession2price, stocksession2qtty, bidP1, bidV1, bidP2, bidV2, bidP3, bidV3, matchPrice, matchQtty, matchPrice - prior,
                        offerP1, offerV1, offerV2, offerV2, offerV3, offerV3, openPrice, highPrice, lowPrice, avgPrice, totalQtty, fBuyQtty, fCurrentRoom, 
                        fSellQtty));
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Opps error at line: " + linetok + " " + e.ToString());
                }
            }
            Console.ReadLine();
            foreach (var stockdetailpair in StockDict)
            {
                Console.WriteLine(stockdetailpair.Value.ToString());
                Console.WriteLine();
            }
            
            Console.ReadLine();

            return StockDict.Values;
        }
    }
}
