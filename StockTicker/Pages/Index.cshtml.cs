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

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        
        private int weekcounter = 0;
        private DateTime tempDate;
        

        public ApiClass apiCall;

        private DateTime startDate;

        public List<string> listOfStrings = new List<string>();

        public string test;

        public double OpenPrice;

        public double Balance = 10000.00;

        public int StockOwned = 0;

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Page();
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

        }

//***********Helper functions*************************

        //this function should be used to update the data shown on the charts 
        public string ProgressGameplay(string val)
        {
            //increase the date by 1 week
            startDate = startDate.AddDays(7);

            //increase week counter variable
            weekcounter++;

            //check to see if 7 weeks have passed and quit game if yes?
            if (weekcounter > 7)
            {
                //end game
                //sell all remaining stocks
                //update bank account
                //post results
            }
            else
            {
                var dateTest = startDate.ToString("yyyy-MM-dd");
                apiCall = new ApiClass(val, dateTest);
                test = apiCall.stock_.Symbol.ToString();
                OpenPrice = apiCall.stock_.Open;
                OpenPrice = Math.Truncate(OpenPrice * 100) / 100;
            }

            return $"The price for {test} is ${OpenPrice} - ";

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
 
//************Ajax functions**********************

        public IActionResult OnPostAjaxGameStart(string val)
        {
            //Call api and assign response to Stock Object with selected ticker and date
            startDate = RandomDay();
            var dateTest = startDate.ToString("yyyy-MM-dd");
            apiCall = new ApiClass(val, dateTest);
            test = apiCall.stock_.Symbol.ToString();
            OpenPrice = apiCall.stock_.Open;
            OpenPrice = Math.Truncate(OpenPrice * 100) / 100;
            return new JsonResult($"The price for {test} is ${OpenPrice} - ");
        }

        public IActionResult OnPostAjaxBuy(string val)//pass in amount of stocks to buy
        {
            //int amount = (int)val;
            
            //do formula with money and amount of stock bought and adjust vars accordingly

            
            return new JsonResult(ProgressGameplay(val));
        }

        public IActionResult OnPostAjaxSell(string val)//pass in amount of stocks to sell
        {
            //do formla with money and amount of stock sold and adjust vars accordingly

            return new JsonResult(ProgressGameplay(val));
        }
        
        public IActionResult OnPostAjaxHold(string val)
        {
            //do nothing and progress the game state
            //ProgressGameplay(/*pass datetime from the users DB entry*/,/*pass stock symbol from users DB entry*/);

            return new JsonResult(ProgressGameplay(val));
        }

        public IActionResult OnPostAjaxQuit()
        {
            //quit game completely
            //sell all stock the user has and add the money to bank account
            //show ending results

            return RedirectToPage("./Index");//probably change this
        }

    }
}