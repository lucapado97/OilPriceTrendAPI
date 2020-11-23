using System;
using System.Collections.Generic;
using System.Text;

namespace OilServiceLib
{
    /// <summary>
    /// output model class
    /// </summary>
    public class Prices
    {
        public IEnumerable<Price> prices;

        public Prices()
        {
            prices = new List<Price>();
        }
    }

    /// <summary>
    /// output struct
    /// </summary>
    public class Price
    {
        public string dateISO8601 { get; set; }
        public double price { get; set; }
    }

    /// <summary>
    /// class necessary to map internal to output structure
    /// </summary>
    public static class PricesMapper
    {
        /// <summary>
        /// maps OilPricePerDay struct to Price struct
        /// </summary>
        /// <param name="toMap"></param>
        /// <returns></returns>
        public static Price MapPrice(OilPricePerDay toMap)
        {
            return new Price()
            {
                dateISO8601 = toMap.Date,
                price = toMap.Price,

            };
        }
    }
}