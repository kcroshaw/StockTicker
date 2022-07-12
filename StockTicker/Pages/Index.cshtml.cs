using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockTicker.Interfaces;
using StockTicker.Data;
using StockTicker.Models;
using System;

using ChartDirector;
using System.Net;
using Newtonsoft.Json;

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

        public List<string> listOfStrings = new List<string>();

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                //Call api and assign response to Stock Object with selected ticker and date
                var ticker = "AAPL";
                var dateTest = RandomDay();
                
                var date = dateTest.ToString("yyyy-MM-dd");
                var apiRequest = $"https://api.polygon.io/v1/open-close/{ticker}/{date}?adjusted=true&apiKey=6TH_lUVoIIueeLAJwbCSPncDIEsGQG0d";
                WebRequest request = WebRequest.Create(apiRequest);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Stock DeserializedObject = JsonConvert.DeserializeObject<Stock>(responseFromServer);

                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();
                return Page();
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

        }

        public IActionResult OnPostGetAjax(string name)
        {
            return new JsonResult("Hello " + name);
        }
        //this function should be used to update the data shown on the charts 
        public void ProgressGameplay(DateTime date, string stockSymbol)
        {
            //increase the date by 1 week
            tempDate = date.AddDays(7);
            
            //save new date to user database entry 


            //call api again using new date?
            //or at least somehow get new data for the next date            

            //update chart data

            //increase week counter variable
            weekcounter++;

            //check to see if 10 weeks have passed and quit game if yes?
            if(weekcounter > 10)
            {
                //end game
                    //sell all remaining stocks
                    //update bank account
                    //post results
            }

        }


        public DateTime RandomDay() //still grabbing weekends FIXME
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
        
        public async void InitGame()
        {
            //put $10,000 dollars into users DB entry

            //save stock symbol to users DB entry


            var stockSymb = Request.Form["stockSymbol"];

            //find random a day to start on between 6 months and 10 years ago
            RandomDay();//save this to users datbase entry

        }

        public async Task<IActionResult> OnPostGameStart()
        {
            //startArea.style.display = 'none'; //dont know how to directly access html elements from here, everything i saw online said to do this but it doesnt work..... -AG
            //gameArea.style.display = 'block';

            await Task.Run(() => InitGame());
            

            //call function that handles gameplay stuff
            //ProgressGameplay(/*pass datetime from the users DB entry*/,/*pass stock symbol from users DB entry*/);
            return RedirectToPage("./Index");

        }

        //public async Task<IActionResult> OnPostBuy()
        //{
        //    //have user indicate how much stock to buy
        //    //maybe have a seperate div with a input box and button that appears when buy is clicked and make the other buttons disappear temporarily?

        //    //progress the game state
        //    //ProgressGameplay(/*pass datetime from the users DB entry*/,/*pass stock symbol from users DB entry*/);

        //}

        //public async Task<IActionResult> OnPostSell()
        //{
        //    // have user select how much stock to sell
        //    //maybe have a seperate div with a input box and button that appears when sell is clicked and make the other buttons disappear temporarily?

        //    //add dollar amount to users account

        //    //progress the game state
        //    //ProgressGameplay(/*pass datetime from the users DB entry*/,/*pass stock symbol from users DB entry*/);

        //}

        //public async Task<IActionResult> OnPostHold()
        //{
        //    //do nothing and progress the game state
        //    //ProgressGameplay(/*pass datetime from the users DB entry*/,/*pass stock symbol from users DB entry*/);
        //}

        //public async Task<IActionResult> OnPostQuit()
        //{
        //    //quit game completely
        //        //sell all stock the user has and add the money to bank account
        //        //show ending results
        //}

    }
}