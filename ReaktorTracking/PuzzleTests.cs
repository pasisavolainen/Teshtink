using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace ReaktorTracking
{
    public class PuzzleTests
    {
        [Fact]
        public void Unique16Chars()
        {
            var text = "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";

            for (int idx = 0; idx < text.Length - 16; idx++)
            {
                var sstr = text.Substring(idx, 16);
                if (sstr.Distinct().Count() != 16)
                    continue;
                Assert.Null(Encoding.ASCII.GetString(Convert.FromBase64String(sstr)));
            }
        }

        [Fact]
        public void DataLog()
        {
            var f = File.ReadAllText("c:/temp/ppb.bin.log");
            var bchars = f.Split(' ').Select(bitToChar).ToArray();
            var str = Encoding.ASCII.GetString(bchars);

            var readings = JsonConvert.DeserializeObject<rday[]>(str);

            var maxday = readings.OrderByDescending(r => r.sum).First();

            var maxr = maxday.readings.OrderByDescending(r => r.sum).First();
            var name = Hex2Str(maxr.id);

            Assert.Null(name);

            byte bitToChar(string s)
            {
                byte ret = 0;
                foreach (var b in s)
                {
                    ret <<= 1;
                    ret |= (b == '1' ? (byte)1 : (byte)0);
                }
                return ret;
            };
        }

        private static string Hex2Str(string s)
        {
            var bytes = Enumerable.Range(0, s.Length / 2)
                                    .Select(i => Convert.ToByte(s.Substring(i * 2, 2), 16))
                                    .ToArray();
            return ASCIIEncoding.UTF8.GetString(bytes);
        }

        public class rday
        {
            public string date;
            public rtime[] readings;

            public int sum => readings.Sum(r => r.sum);
        }
        public class rtime
        {
            public int time;
            public string id;

            public Dictionary<string, int> contaminants;
            public int sum => contaminants.Sum(kvp => kvp.Value);
        }

        [Fact]
        public void Flood()
        {
            var rawstr = File.ReadAllText("c:/temp/flood.txt");
            // useful for sheets.google.com, colorize between(-7, 7) as light gray
            // var rws = File.OpenWrite("c:/temp/ddd.csv");

            var regions = JsonConvert.DeserializeObject<regionc>(rawstr);

            string ret = "";

            foreach (var r in regions.regions)
            {
                var previous = r.readings.First();

                foreach (var curReading in r.readings.Skip(1))
                {
                    var diff = previous.reading.Zip(curReading.reading, (a, b) => a - b).ToList();

                    //var rstr = string.Join(", ", diff);
                    //rws.Write(UTF8Encoding.UTF8.GetBytes($"{r.regionID}, {curReading.readingID}, {rstr}\n"));

                    var diffsum = diff.Select(Math.Abs).Sum();

                    if (diffsum >= 1000)
                    {
                        var js = "";//JsonConvert.SerializeObject(ch1, Formatting.None);
                        ret += (r.regionID + ":" + diffsum + " " + curReading.readingID) + "=> " + js + "\n";
                    }

                    previous = curReading;
                }
            }
            //rws.Flush(true);
            //rws.Close();
            Assert.Null(ret);
        }

        public class regionc
        {
            public region[] regions;
        }
        public class region
        {
            public string regionID;
            public rreading[] readings;
        }
        public class rreading
        {
            public string readingID;
            public string date;
            public int[] reading;
        }
    }
}
