namespace StockTicker.Models
{
    public class Stock
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public double Balance { get; set; }
        public double Open { get; set; }
        public int StockOwned { get; set; }

    }
}
