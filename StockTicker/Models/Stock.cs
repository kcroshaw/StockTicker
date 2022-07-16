using System.ComponentModel.DataAnnotations;

namespace StockTicker.Models
{
    public class Stock
    {
        [Key]
        public int StockId { get; set; }

        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public double Balance { get; set; }
        public double Open { get; set; }
        public int StockOwned { get; set; }

    }
}
