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

namespace ConsoleApplication1
{

    class StockDetails : IComparable<StockDetails>
    {
        public string StockSymbol { get; private set; }
        public double PriorPrice { get; private set; }
        public double FloorPrice { get; private set; }
        public double CeilingPrice { get; private set; }
        public double Session1Price { get; set; }
        public double Session1Qtty { get; set; }
        public double Session2Price { get; set; }
        public double Session2Qtty { get; set; }
        public double BidP1 { get; set; }
        public double BidV1 { get; set; }
        public double BidP2 { get; set; }
        public double BidV2 { get; set; }
        public double BidP3 { get; set; }
        public double BidV3 { get; set; }
        public double MatchPrice { get; set; }
        public double MatchQtty { get; set; }
        public double Change { get; set; }
        public double OfferP1 { get; set; }
        public double OfferV1 { get; set; }
        public double OfferP2 { get; set; }
        public double OfferV2 { get; set; }
        public double OfferP3 { get; set; }
        public double OfferV3 { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double AvrPrice { get; set; }
        public double TotalQtty { get; set; }
        public double FBuyQtty { get; set; }
        public double FCurrentRoom { get; set; }
        public double FSellQtty { get; set; }







        public StockDetails(string stockSym, double ceil, double floor, double prior, double session1Price, double session1Qtty, double session2Price,
            double session2Qtty, double bidP1, double bidV1, double bidP2, double bidV2, double bidP3, double bidV3, double matchPrice, double matchQtty, 
            double change, double offerP1, double offerV1, double offerP2, double offerV2, double offerP3, double offerV3, double highPrice, 
            double lowPrice, double avrPrice, double totalQtty, double fBuyQtty, double fCurrentRoom, double fSellQtty)
        {
            StockSymbol = stockSym;
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
            HighPrice = highPrice;
            LowPrice = lowPrice;
            AvrPrice = avrPrice;
            TotalQtty = totalQtty;
            FBuyQtty = fBuyQtty;
            FCurrentRoom = fCurrentRoom;
            FSellQtty = fSellQtty;
        }

        public double Percent() // MatchPrice - PriorPrice
        {
            if (MatchPrice == 0 || PriorPrice == 0)
                return MatchPrice;
            else return MatchPrice - PriorPrice;
        }

        public override string ToString()
        {
            return String.Format(@"sym: {0}, ceil: {1}, floor: {2}, prior: {3}, session1Price: {4}, session1Qtty: {5}, session2Price: {6}, session2Qtty: {7}, 
                                    bidV3: {8}, bidP3: {9}, bidV2: {10}, bidP2: {11}, bidV1: {12}, bidP1: {13}, matchPrice: {14}, matchQtty: {15}, percent: {16}, 
                                    offerP1: {17}, offerV1: {18}, offerP2: {19}, offerV2: {20}, offerP3: {21}, offerV3: {22}, highPrice: {23}, lowPrice: {24}, 
                                    avrPrice: {25}, totalQtty: {26}, fBuyQtty: {27}, fCurrentRoom: {28}, fSellQtty: {29}",
                                    StockSymbol, CeilingPrice, FloorPrice, PriorPrice, Session1Price, Session1Qtty, Session2Price, Session2Qtty,
                                    BidV3, BidP3, BidV2, BidP2, BidV1, BidP3, MatchPrice, MatchQtty, Change, OfferP1, OfferV1, OfferP2, OfferV2,
                                    OfferP3, OfferV3, HighPrice, LowPrice, AvrPrice, TotalQtty, FBuyQtty, FCurrentRoom, FSellQtty);
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

        public static double getFromStringToken(String stringtok)
        {
            var result = (stringtok != null && stringtok != "") ? Double.Parse(stringtok) : 0;
            return result;
        }


        static void Main(string[] args)
        {
            //getdata();

            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create("http://priceboard.fpts.com.vn/hsx/data.ashx?s=quote&l=All");
            // WebRequest request = WebRequest.Create("http://stockboard.sbsc.com.vn/HO.ashx?FileName=ref");
            // WebRequest request = WebRequest.Create("http://prs.tvsi.com.vn/DataForLoad.ashx?FloorCode=00");

            // Set the Method property of the request to POST.
            request.Method = "GET";

            // Create POST data and convert it to a byte array.
            //string postData = "This is a test that posts this string to a Web server.";
            //byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "text/plain; charset=utf-8";

            // Set the ContentLength property of the WebRequest.
            //request.ContentLength = byteArray.Length;

            // Get the request stream.
            //Stream dataStream = request.GetRequestStream();

            // Write the data to the request stream.
            //dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            //dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
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

            // Display the content.
            //Console.WriteLine(responseFromServer);

            //StockDetails stock = 

            //var linetokens = responseFromServer.Substring(1, responseFromServer.Length - 3).Split(new string[] { "},{" }, StringSplitOptions.None);

            //Regex r = new Regex("\"RowID\":\"d{0,4}\"[,.]\"Info\":");
            //string s = "\"RowID\":\"21\",\"Info\":";
            //Console.WriteLine(s);
            //Console.WriteLine(r.IsMatch(s));
            //Console.WriteLine(r.IsMatch("\"RowID\": + \"21\"," + "\"Info\"" +":"));



            //var linetokens = responseFromServer.Split('|'); // SBSC
            //Console.WriteLine(linetokens[0]);

            //foreach (var linetok in linetokens)
            //{
            //    Console.WriteLine(linetok);
            //}

            string path = @"C:\Users\Anh Phan\Downloads\Documents\CS487-488\Sample codes\Sample data\text.txt";
            var linetokens = responseFromServer.Substring(1, responseFromServer.Length-2).Split(new string[] { "},{" }, StringSplitOptions.None);
            //Console.WriteLine(linetokens);            
            //foreach (var linetok in linetokens)
            //{
            //    //Console.WriteLine(linetok);
            //    File.AppendAllText(path, linetok + '\n');
            //}

            for (int i = 0; i < linetokens.Length-1; i++)
            {
                linetokens[i] = linetokens[i] + "}";
            }
            for (int i = 1; i < linetokens.Length; i++)
            {
                linetokens[i] = "{" + linetokens[i];
            }
            //foreach (var linetok in linetokens)
            //{
            //    File.AppendAllText(path, linetok + '\n');
            //}

            Console.ReadLine();
            var StockDict = new Dictionary<String, StockDetails>();
            foreach (var linetok in linetokens)
            {
                JObject linejson = JObject.Parse(linetok);
                //Console.WriteLine(linejson);
                //Console.ReadLine();

                var StockName = linejson["Info"][0][1].ToObject<string>();
                var PrevClosePrice = linejson["Info"][1][1].ToObject<double>();
                var Ceiling = linejson["Info"][2][1].ToObject<double>();
                var Floor = linejson["Info"][3][1].ToObject<double>();
                var BidPrice3 = linejson["Info"][5][1].ToObject<double>();
                var BidVol3 = linejson["Info"][6][1].ToObject<double>();
                var BidPrice2 = linejson["Info"][7][1].ToObject<double>();
                var BidVol2 = linejson["Info"][8][1].ToObject<double>();
                var BidPrice1 = linejson["Info"][9][1].ToObject<double>();
                var BidVol1 = linejson["Info"][10][1].ToObject<double>();
                var MatchPrice = linejson["Info"][11][1].ToObject<double>();
                var MatchVol = linejson["Info"][12][1].ToObject<double>();
                var Change = linejson["Info"][13][1].ToObject<double>();
                var AskPrice1 = linejson["Info"][14][1].ToObject<double>();
                var AskVol1 = linejson["Info"][15][1].ToObject<double>();
                var AskPrice2 = linejson["Info"][16][1].ToObject<double>();
                var AskVol2 = linejson["Info"][17][1].ToObject<double>();
                var AskPrice3 = linejson["Info"][18][1].ToObject<double>();
                var AskVol3 = linejson["Info"][19][1].ToObject<double>();
                var TotalVolume = linejson["Info"][21][1].ToObject<double>();
                var OpenPrice = linejson["Info"][22][1].ToObject<double>();
                var HighestPrice = linejson["Info"][23][1].ToObject<double>();
                var LowestPrice = linejson["Info"][24][1].ToObject<double>();
                var Avg = linejson["Info"][25][1].ToObject<double>();
                var FrgnBuy = linejson["Info"][26][1].ToObject<double>();
                var FrgnSell = linejson["Info"][27][1].ToObject<double>();
                var RoomLeft = linejson["Info"][28][1].ToObject<double>();
                try
                {
                    StockDict.Add(StockName, new StockDetails(StockName, Ceiling, Floor, PrevClosePrice, 0, 0,
                        0, 0, BidPrice1, BidVol1, BidPrice2, BidVol2, BidPrice3, BidVol3, MatchPrice, MatchVol, Change, AskPrice1, AskVol1, AskPrice2, AskVol2,
                        AskPrice3, AskVol3, HighestPrice, LowestPrice, Avg, TotalVolume, FrgnBuy, FrgnSell, RoomLeft));
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
            //Console.WriteLine(StockDict["AAA"].CeilingPrice);
            Console.ReadLine();

        }

        private static IEnumerable<StockDetails> getdata()
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create("http://priceboard.fpts.com.vn/hsx/data.ashx?s=quote&l=All");
        
            // Set the Method property of the request to POST.
            request.Method = "GET";
            // Create POST data and convert it to a byte array.
            //string postData = "This is a test that posts this string to a Web server.";
            //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "text/plain; charset=utf-8";
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
            //var linetokens = responseFromServer.SubstringJava(15, responseFromServer.Length - 2).Split('#');
            //var linetokens = responseFromServer.Split('#');
            //var linetokens = responseFromServer.Split('#');
            //foreach (var linetok in linetokens)
            //{
            //    Console.WriteLine(linetok);
            //}

            var StockDict = new Dictionary<String, StockDetails>();

            foreach (var linetok in linetokens)
            {
                Console.WriteLine(linetok);
                var stocktoks = linetok.Split('|');
                var stocksym = stocktoks[0];

                try
                {
                    var stockceil = getFromStringToken(stocktoks[1]);
                    var stockfloor = getFromStringToken(stocktoks[2]);
                    var stockprior = getFromStringToken(stocktoks[3]);

                    var stocksession1price = getFromStringToken(stocktoks[4]);
                    var stocksession1qtty = getFromStringToken(stocktoks[5]);

                    var stocksession2price = getFromStringToken(stocktoks[6]);
                    var stocksession2qtty = getFromStringToken(stocktoks[7]);

                    var bidP1 = getFromStringToken(stocktoks[8]);
                    var bidV1 = getFromStringToken(stocktoks[9]);
                    var bidP2 = getFromStringToken(stocktoks[10]);
                    var bidV2 = getFromStringToken(stocktoks[11]);
                    var bidP3 = getFromStringToken(stocktoks[12]);
                    var bidV3 = getFromStringToken(stocktoks[13]);

                    var matchPrice = getFromStringToken(stocktoks[14]);
                    var matchQtty = getFromStringToken(stocktoks[15]);
                    // var percent = (matchPrice - stockprior);

                    var offerP1 = getFromStringToken(stocktoks[16]);
                    var offerV1 = getFromStringToken(stocktoks[17]);
                    var offerP2 = getFromStringToken(stocktoks[18]);
                    var offerV2 = getFromStringToken(stocktoks[19]);
                    var offerP3 = getFromStringToken(stocktoks[20]);
                    var offerV3 = getFromStringToken(stocktoks[21]);

                    var highPrice = getFromStringToken(stocktoks[22]);
                    var lowPrice = getFromStringToken(stocktoks[23]);
                    var avrPrice = getFromStringToken(stocktoks[24]);
                    var totalQtty = getFromStringToken(stocktoks[25]);

                    var fBuyQtty = getFromStringToken(stocktoks[26]);
                    var fCurrentRoom = getFromStringToken(stocktoks[27]);
                    var fSellQtty = getFromStringToken(stocktoks[28]);

                    StockDict.Add(stocksym, new StockDetails(stocksym, stockceil, stockfloor, stockprior, stocksession1price, stocksession1qtty,
                        stocksession2price, stocksession2qtty, bidP1, bidV1, bidP2, bidV2, bidP3, bidV3, matchPrice, matchQtty, matchPrice - stockprior,
                        offerP1, offerV1, offerV2, offerV2, offerV3, offerV3, highPrice, lowPrice, avrPrice, totalQtty, fBuyQtty, fCurrentRoom, fSellQtty));
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
            //Console.WriteLine(StockDict["AAA"].CeilingPrice);
            Console.ReadLine();

            // a['code'] => (floor, ceil, prior)
            // a['symbol'].floor => FloorPrice
            // no symbol
            // list all stocks by hashmap
            return StockDict.Values;
        }
    }
}
