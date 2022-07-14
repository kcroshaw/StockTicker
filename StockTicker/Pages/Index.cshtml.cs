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
        public void ProgressGameplay(DateTime date)
        {
            //increase the date by 1 week
            tempDate = date.AddDays(7);

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

        public DateTime NextDay(DateTime start)
        {
            DateTime Day = start.AddDays(7);
            return Day;
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


        public IActionResult OnPostAjaxBuy()
        {   //have user indicate how much stock to buy

            //maybe have a seperate div with a input box and button that appears when buy is clicked and make the other buttons disappear temporarily?
            startDate = NextDay(startDate);
            var dateTest = startDate.ToString("yyyy-MM-dd");
            //progress the game state
            apiCall = new ApiClass(test, dateTest);
            test = apiCall.stock_.Symbol.ToString();
            OpenPrice = apiCall.stock_.Open;
            OpenPrice = Math.Truncate(OpenPrice * 100) / 100;
            //ProgressGameplay(/*pass datetime from the users DB entry*/,/*pass stock symbol from users DB entry*/)
            return new JsonResult($"The price for {test} is ${OpenPrice}");//probably change this
        }

        public IActionResult OnPostAjaxSell()
        {
            //do formla with money and amount of stock sold and adjust vars accordingly

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