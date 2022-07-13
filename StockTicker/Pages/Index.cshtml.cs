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

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                //Call api and assign response to Stock Object with selected ticker and date
                var ticker = "AAPL";
                startDate = RandomDay();
                var dateTest = startDate.ToString("yyyy-MM-dd");
                apiCall = new ApiClass(ticker,dateTest);
                test = apiCall.stock_.Symbol.ToString();
                return Page();
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

        }

//***********Helper functions*************************

        //this function should be used to update the data shown on the charts 
        public void ProgressGameplay(DateTime date, string stockSymbol)
        {
            //increase the date by 1 week
            tempDate = date.AddDays(7);
            
            //save new date to user database entry 


            //call api again using new date?
                      

            //update chart with new data

            //increase week counter variable
            weekcounter++;

            //check to see if 7 weeks have passed and quit game if yes?
            if(weekcounter > 7)
            {
                //end game
                    //sell all remaining stocks
                    //update bank account
                    //post results
            }

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
        
        public void InitGame()
        {
            //put $10,000 dollars into users DB entry

            //save stock symbol to users DB entry
            var stockSymb = Request.Form["stockSymbol"];

            //find random a day to start on between 6 months and 10 years ago
            RandomDay();//save this to users datbase entry

        }

//************Ajax functions**********************

        public IActionResult OnPostAjaxGameStart(string val)
        {
            //initialize the game
            //InitGame();

            //call function that handles gameplay stuff
            //ProgressGameplay(/*pass datetime from the users DB entry*/,/*pass stock symbol from users DB entry*/);

            return new JsonResult(val);//probably change this
        }

        public IActionResult OnPostAjaxBuy()
        {
            //have user indicate how much stock to buy
            //maybe have a seperate div with a input box and button that appears when buy is clicked and make the other buttons disappear temporarily?

            //progress the game state
            //ProgressGameplay(/*pass datetime from the users DB entry*/,/*pass stock symbol from users DB entry*/);
            
            return RedirectToPage("./Index");//probably change this
        }

        public IActionResult OnPostAjaxSell()
        {
            // have user select how much stock to sell
            //maybe have a seperate div with a input box and button that appears when sell is clicked and make the other buttons disappear temporarily?

            //add dollar amount to users account

            //progress the game state
            //ProgressGameplay(/*pass datetime from the users DB entry*/,/*pass stock symbol from users DB entry*/);

            return RedirectToPage("./Index");//probably change this
        }
        
        public IActionResult OnPostAjaxHold()
        {
            //do nothing and progress the game state
            //ProgressGameplay(/*pass datetime from the users DB entry*/,/*pass stock symbol from users DB entry*/);

            return RedirectToPage("./Index");//probably change this
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