using NUnit.Framework;
using OilServiceLib;
using System.Linq;

namespace Test
{
    public class UnitTests
    {
        private static OilService _oilService = new OilService();

        [Test]
        public void ArgumentTest()
        {
            var dummy = false;
            try
            {
                _oilService.GetOilPriceTrend("2020-01-25", "2020-01-23");
            }
            catch (System.ArgumentException)
            {
                dummy = true;
            }
            Assert.IsTrue(dummy);
        }

        [Test]
        public void ReadPublicDataTest()
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                var responseString = client.GetStringAsync("https://pkgstore.datahub.io/core/oil-prices/brent-daily_json/data/78b325d2b9b2be78282cfd9f62978149/brent-daily_json.json").Result;
                _oilService.SetData(responseString);
                Assert.IsTrue(_oilService.IsDataPopulated);
            }
            catch { }
        }

        [Test]
        public void ServiceFunctionalityTest()
        {
            _oilService.SetData();
            var result = _oilService.GetOilPriceTrend("1987-05-20", "1987-05-22");
            var passed = true;
            passed &= result.prices.FirstOrDefault(T => T.dateISO8601 == "1987-05-20") != null;
            passed &= result.prices.FirstOrDefault(T => T.dateISO8601 == "1987-05-21") != null;
            passed &= result.prices.FirstOrDefault(T => T.dateISO8601 == "1987-05-22") != null;
            Assert.IsTrue(passed);
        }
    }
}