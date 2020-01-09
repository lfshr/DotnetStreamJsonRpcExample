using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using Shared;
using StreamJsonRpc;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int clientId = 0;
            while (true)
            {
                Console.WriteLine("Waiting for client to make a connection...");
                var stream = new NamedPipeServerStream(
                    "RPCTestPipe",
                    PipeDirection.InOut,
                    NamedPipeServerStream.MaxAllowedServerInstances,
                    PipeTransmissionMode.Byte,
                    PipeOptions.Asynchronous);

                await stream.WaitForConnectionAsync();
                Console.WriteLine("Client Connected!");

                var jsonRpc = JsonRpc.Attach(stream, new Server());
                await jsonRpc.Completion;

                Console.WriteLine("Client disconnected");
            }
        }
    }

    class Server : IServerService
    {
        public async Task<string> GetHelloWorld()
        {
            return "Hello World";
        }
    }
}
