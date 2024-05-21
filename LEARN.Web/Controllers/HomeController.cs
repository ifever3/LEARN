using LEARN.Web.Models;
using LEARN.Web.websocket;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.WebSockets;

namespace LEARN.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebSocket _webSocket;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Example data
            return View();
        }

        public IActionResult websocket()
        {                       
            return View();
        }

        [HttpGet("/ws")]
        public async Task WebSocketServer()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                try
                {
                    var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                    await new WebSocketHelper().WebSocketReceive(socket);
                }
                catch (Exception)
                {
                }
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
