//using Fleck;
//using System.Collections.Concurrent;
//using System.Net.Sockets;

//namespace LEARN.Web.websocket
//{
//    public class webSocket
//    {
//        public webSocket()
//        {
//            var connectSocketPool = new List<IWebSocketConnection>();
//            //创建WebSocket服务端实例并监听本机的9999端口
//            var server = new WebSocketServer("ws://127.0.0.1:9999");
//            //开启监听
//            server.Start(socket =>
//            {
//                //注册客户端连接建立事件
//                socket.OnOpen = () =>
//                {
//                    Console.WriteLine("Open");
//                    //将当前客户端连接对象放入连接池中
//                    connectSocketPool.Add(socket);
//                };
//                //注册客户端连接关闭事件
//                socket.OnClose = () =>
//                {
//                    Console.WriteLine("Close");
//                    //将当前客户端连接对象从连接池中移除
//                    connectSocketPool.Remove(socket);
//                };
//                //注册客户端发送信息事件
//                socket.OnMessage = message =>
//                {
//                    Console.WriteLine(message);
//                    //向客户端发送消息
//                    socket.Send($"服务端接收到信息：{message}");
//                };
//            });
//        }
//    }
//}
