using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static void Main()
    {
        int port = 8080;
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine("서버 시작! 포트: " + port);

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("클라이언트 연결됨!");

            // 클라이언트와 통신을 별도 스레드에서 처리
            new Thread(() =>
            {

                NetworkStream stream = client.GetStream();

                // 1. 클라이언트로부터 명령 수신
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine("수신: " + message);

                // 2. 서버에서 클라이언트로 메시지 전송 (예: 3초 후)
                Thread.Sleep(3000);
                string serverMsg = "서버에서 Unity로: Hello!";
                byte[] serverMsgBytes = Encoding.UTF8.GetBytes(serverMsg);
                stream.Write(serverMsgBytes, 0, serverMsgBytes.Length);

                client.Close();
            }).Start();
        }
    }
}