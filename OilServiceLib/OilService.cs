using AustinHarris.JsonRpc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

namespace OilServiceLib
{
    /// <summary>
    /// class containing public functions
    /// </summary>
    public class OilService : JsonRpcService
    {
        /// <summary>
        /// date format to parse input dates
        /// </summary>
        private static string _dateFormat = "yyyy-MM-dd";

        /// <summary>
        /// internal data to fetch
        /// </summary>
        private static IEnumerable<OilPricePerDay> _oilPriceData;

        public bool IsDataPopulated
        {
            get
            {
                return _oilPriceData.Count() != 0;
            }
        }

        /// <summary>
        /// retrieves oil prices between 2 dates
        /// </summary>
        /// <param name="startDateISO8601"></param>
        /// <param name="endDateISO8601"></param>
        /// <returns></returns>
        [JsonRpcMethod]
        public Prices GetOilPriceTrend(string startDateISO8601, string endDateISO8601)
        {
            DateTime startDate = DateTime.ParseExact(startDateISO8601, _dateFormat, CultureInfo.InvariantCulture), endDate = DateTime.ParseExact(endDateISO8601, _dateFormat, CultureInfo.InvariantCulture);
            if (startDate > endDate)
                throw new ArgumentException("start date is grater than end date");
            var toReturn = _oilPriceData.Where(T => DateTime.ParseExact(T.Date, _dateFormat, CultureInfo.InvariantCulture) >= startDate && DateTime.ParseExact(T.Date, _dateFormat, CultureInfo.InvariantCulture) <= endDate);
            if (toReturn != null)
                toReturn = toReturn.OrderBy(T => T.Date);
            var dummy = new Prices();
            dummy.prices = toReturn.Select(PricesMapper.MapPrice);
            return dummy;
        }

        /// <summary>
        /// sets oil data from a json
        /// </summary>
        /// <param name="jsonData"></param>
        public void SetData(string jsonData)
        {
            _oilPriceData = JsonConvert.DeserializeObject<IEnumerable<OilPricePerDay>>(jsonData);
        }

        /// <summary>
        /// sets oil data with static json
        /// </summary>
        public void SetData()
        {
            string jsonData = "[{ 'Date': '1987-05-20', 'Price': 18.63},{ 'Date': '1987-05-21', 'Price': 18.45},{ 'Date': '1987-05-22', 'Price': 18.55},{ 'Date': '1987-05-25', 'Price': 18.6},{ 'Date': '1987-05-26', 'Price': 18.63},{ 'Date': '1987-05-27', 'Price': 18.6},{ 'Date': '1987-05-28', 'Price': 18.6},{ 'Date': '1987-05-29', 'Price': 18.58},{ 'Date': '1987-06-01', 'Price': 18.65},{ 'Date': '1987-06-02', 'Price': 18.68},{ 'Date': '1987-06-03', 'Price': 18.75},{ 'Date': '1987-06-04', 'Price': 18.78},{ 'Date': '1987-06-05', 'Price': 18.65},{ 'Date': '1987-06-08', 'Price': 18.75},{ 'Date': '1987-06-09', 'Price': 18.78},{ 'Date': '1987-06-10', 'Price': 18.78},{ 'Date': '1987-06-11', 'Price': 18.68},{ 'Date': '1987-06-12', 'Price': 18.78},{ 'Date': '1987-06-16', 'Price': 18.9},{ 'Date': '1987-06-17', 'Price': 19.03},{ 'Date': '1987-06-18', 'Price': 19.05},{ 'Date': '1987-06-19', 'Price': 19.05},{ 'Date': '1987-06-22', 'Price': 19.1},{ 'Date': '1987-06-23', 'Price': 18.9},{ 'Date': '1987-06-24', 'Price': 18.75},{ 'Date': '1987-06-25', 'Price': 18.7},{ 'Date': '1987-06-26', 'Price': 19.08},{ 'Date': '1987-06-29', 'Price': 19.15},{ 'Date': '1987-06-30', 'Price': 19.08},{ 'Date': '1987-07-01', 'Price': 18.98},{ 'Date': '1987-07-02', 'Price': 19.25},{ 'Date': '1987-07-03', 'Price': 19.33},{ 'Date': '1987-07-06', 'Price': 19.48},{ 'Date': '1987-07-07', 'Price': 19.5},{ 'Date': '1987-07-08', 'Price': 19.48},{ 'Date': '1987-07-09', 'Price': 19.68},{ 'Date': '1987-07-10', 'Price': 19.73},{ 'Date': '1987-07-13', 'Price': 19.83},{ 'Date': '1987-07-14', 'Price': 19.88},{ 'Date': '1987-07-15', 'Price': 20.28} ]";
            SetData(jsonData);
        }
    }

    /// <summary>
    /// internal use struct to parse input json
    /// </summary>
    public struct OilPricePerDay
    {
        public string Date { get; set; }
        public double Price { get; set; }
    }
}
