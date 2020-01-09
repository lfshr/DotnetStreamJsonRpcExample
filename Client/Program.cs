using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using Shared;
using StreamJsonRpc;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Connecting to server...");

            using (var stream = new NamedPipeClientStream(
                ".",
                "RPCTestPipe",
                PipeDirection.InOut,
                PipeOptions.Asynchronous
            ))
            {
                await stream.ConnectAsync();
                Console.WriteLine("Connected!");
                var jsonRpc = JsonRpc.Attach<IServerService>(stream);

                var message = await jsonRpc.GetHelloWorld();

                Console.WriteLine($"Received message: {message}");
                Console.WriteLine("Closing connection...");
            }

            Console.ReadLine();
        }
    }
}
