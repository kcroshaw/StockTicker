using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockTicker.Interfaces;
using StockTicker.Data;
using StockTicker.Models;
using System;
using ChartDirector;
using System.Net;
using Newtonsoft.Json;
using StockTicker.Services;

namespace StockTicker.Pages
{


    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ApiClass apiCall;

        public List<string> listOfStrings = new List<string>();


        //DONT USE THESE TO STORE PERSISTENT DATA - only use as temp vars if necessary
        public string tickerSymbol = "";

        public DateTime tempDate;

        public double tempOpenPrice = 0.0;
        
        public double tempBalance = 0.00;
        
        public int tempStockOwned = 0;
        //******************************************************************
        
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
  
        public Stock StockObj { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                //set temp vars equal to their respective data members in the stock model
                tickerSymbol = StockObj.Symbol;
                tempDate = StockObj.Date;
                tempOpenPrice = StockObj.Open;
                tempBalance = StockObj.Balance;
                tempStockOwned = StockObj.StockOwned;               
                
                return Page();
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

        }

//***********Helper functions*************************

        //this function should be used to update the data shown on the charts 
        public string ProgressGameplay()
        {
            tempDate = NextDay(StockObj.Date);
            //check if the day is today or in the future, if so we need to quit or it will break
            if (tempDate >= DateTime.Now)
            {
                return QuitGame();
            }
            var dateTest = tempDate.ToString("yyyy-MM-dd");
            //progress the game state
            apiCall = new ApiClass(StockObj.Symbol, dateTest);
            tickerSymbol = apiCall.stock_.Symbol.ToString();
            tempOpenPrice = apiCall.stock_.Open;
            tempOpenPrice = Math.Truncate(tempOpenPrice * 100) / 100;

            //before this method ends we need to save the variables
            StockObj.Date = tempDate;
            StockObj.Open = tempOpenPrice;

            return "The price for " + tickerSymbol + " is " + tempOpenPrice;

        }

        public DateTime RandomDay()
        {
            Random gen = new Random();
            DateTime start = DateTime.Today.AddYears(-2);
            int range = (DateTime.Today.AddMonths(-6) - start).Days;
            DateTime randDay = start.AddDays(gen.Next(range));
            if (randDay.DayOfWeek == DayOfWeek.Saturday || randDay.DayOfWeek == DayOfWeek.Sunday)
            {
                randDay = randDay.AddDays(2);
            }
                return randDay;
        }

        public DateTime NextDay(DateTime start)
        {
            DateTime Day = start.AddDays(7);
            return Day;
        }

        //quit game completely
        public string QuitGame()
        {
            //sell all stock the user has and add the money to bank account
            double amount = StockObj.StockOwned * StockObj.Open;
            StockObj.Balance += amount;

            //show ending results by returning a string to the Ajax handler
            return "Game Over! You ended with $" + StockObj.Balance + "\n\r" + "That means your overall gain/loss was $" + (StockObj.Balance-10000);
        }

        //************Ajax functions**********************

        public IActionResult OnPostAjaxGameStart(string val)
        {
            //Call api and assign response to Stock Object with selected ticker and date
            tempDate = RandomDay();
            var dateTest = tempDate.ToString("yyyy-MM-dd");
            apiCall = new ApiClass(val, dateTest);
            tickerSymbol = apiCall.stock_.Symbol.ToString();
            tempOpenPrice = apiCall.stock_.Open;
            tempOpenPrice = Math.Truncate(tempOpenPrice * 100) / 100;

            //before method ends save all important data
            StockObj.Open = tempOpenPrice;
            StockObj.Date = tempDate;
            StockObj.Balance = 10000.00;
            StockObj.Symbol = tickerSymbol;

            return new JsonResult($"The price for {tickerSymbol} is ${tempOpenPrice}: \n\r");
        }


        public IActionResult OnPostAjaxBuy(string val)
        {
            int buyAmount = Convert.ToInt32(val);

            //do stock and money algorithm
            double cost = StockObj.Open * buyAmount;
            //check to see if we have enough money
            if (cost > StockObj.Balance)
            {
                return new JsonResult("You do not have enough money to buy " + val + " shares.");
            }
            
            //save data
            StockObj.Balance -= cost;
            StockObj.StockOwned += buyAmount;
                       
            return new JsonResult("You bought " + val + " shares and spent  $" + cost + " \n\r" + ProgressGameplay());
        }

        public IActionResult OnPostAjaxSell(string val)
        {
            int sellAmount = Convert.ToInt32(val);

            //check to see if we even have enough stock to sell
            if (sellAmount > StockObj.StockOwned)
            {
                return new JsonResult("You do not have enough stock to sell " + val + " shares.");
            }
            
            //do stock and money algorithm
            double money = StockObj.Open * sellAmount;
            StockObj.Balance += money;
            StockObj.StockOwned -= sellAmount;

            return new JsonResult("You sold " + val + " shares and made $" + money + " \n\r" + ProgressGameplay());
        }

        public IActionResult OnPostAjaxHold()
        {
            //do nothing and progress the game



            return new JsonResult("You decided to hold. " + " \n\r" + ProgressGameplay());
        }

        public IActionResult OnPostAjaxQuit()
        {
            return new JsonResult(QuitGame()); 
        }

    }
}