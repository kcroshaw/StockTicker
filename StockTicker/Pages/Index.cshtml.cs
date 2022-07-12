﻿using Microsoft.AspNetCore.Mvc;
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

        public List<string> listOfStrings = new List<string>();

        public string test;

        public string tick = "GME";

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                //Call api and assign response to Stock Object with selected ticker and date
                apiCall = new ApiClass("tick");
                test = apiCall.stock_.Symbol.ToString();


                return Page();
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

        }

        public IActionResult OnPostGetAjax(string name)
        {
            tick = String.Format("{0}", Request.Form["pick"]);

            return Page();//new JsonResult("Hello " + name);
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


        public DateTime RandomDay()
        {
            Random gen = new Random();
            DateTime start = DateTime.Today.AddYears(-10);
            int range = (DateTime.Today.AddMonths(-6) - start).Days;
            
            return start.AddDays(gen.Next(range));
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
            return RedirectToPage("./Index");

            //call function that handles gameplay stuff
            //ProgressGameplay(/*pass datetime from the users DB entry*/,/*pass stock symbol from users DB entry*/);


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