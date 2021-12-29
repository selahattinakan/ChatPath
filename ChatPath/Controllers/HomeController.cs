using ChatPath.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ChatPath.Helper;
using ChatPath.Entity;
using System.Threading.Tasks;

namespace ChatPath.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /*Selahattin Akan*/
        public IActionResult Index()
        {
            //InsertTestData();
            ChatModel model = new();
            ViewBag.Channels=model.GetChannels();
            if (Request.Cookies["nickName"] != null)
            {
                ViewBag.Nickname = Request.Cookies["nickName"].ToString();
            }
            return View();
        }

        public IActionResult GetMessages(int chn)
        {
            ChatModel model = new ChatModel();
            List<Message> messages = model.GetMessagesByChannel(chn);
            Encryption enc = new();
            for (int i = 0; i < messages.Count; i++)
            {
                //mesajlar db'den şifrelenmiş olarak geldi, şifreler çözüldükten sonra partialview'a render etmesi için gönderildi
                messages[i].MessageText = enc.DecryptText(messages[i].MessageText);
            }
            return PartialView("~/Views/Shared/Partial/_PartialMessage.cshtml", messages);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Test mesaj verilerini veritabanına kaydeder.
        /// </summary>
        private void InsertTestData()
        {
            ChatModel model = new();
            List<Message> messages = new();
            List<Channel> channels = model.GetChannels();

            string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"wwwroot\file\", "test.txt");
            List<string> lines = System.IO.File.ReadAllLines(path).ToList();

            List<string> nicks = new() { "Selahattin", "Ahmet", "Serkan", "Kaan", "Birhan" };
            Random rnd = new();

            Encryption enc = new();

            foreach (var channel in channels)
            {
                foreach (var line in lines)
                {
                    int r = rnd.Next(nicks.Count);
                    Message tmp = new()
                    {
                        ChannelID = channel.ChannelID,
                        Date = DateTime.Now,
                        IsDeleted = false,
                        MessageText = enc.EncryptText(line),
                        NickName = nicks[r]
                    };
                    messages.Add(tmp);
                    System.Threading.Thread.Sleep(1000);
                    if (model.InsertMessage(tmp) == 1)
                    {
                        Console.WriteLine($"{tmp.NickName} : {tmp.MessageText}");
                    }
                    else
                    {
                        Console.WriteLine("Veritabanı insert işlemi başarısız!");
                    }
                }
            }
        }
    }
}
