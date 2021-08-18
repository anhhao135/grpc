// Copyright 2015 gRPC authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Threading.Tasks;
using Grpc.Core;
using Helloworld;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace GreeterServer
{
    class GreeterImpl : Greeter.GreeterBase
    {
        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
        }

        public override Task<GoodbyeReply> SayGoodbye(GoodbyeRequest request, ServerCallContext context)
        {
            GoodbyeReply byeBye = new GoodbyeReply();
            byeBye.Message = "goodbye! " + request.Name;
            return Task.FromResult(byeBye);
        }

        public override async Task SendCatPic(CatPicRequest request, IServerStreamWriter<ChunkCatPicReply> responseStream, ServerCallContext context)
        {

            Console.WriteLine("sending cat pic... enter file to send name");



            var filePath = request.FilePath;

            var fileInfo = new FileInfo(filePath);

            var chunk = new ChunkCatPicReply
            {
                FileName = Path.GetFileName(filePath),
                FileSize = fileInfo.Length
            };

            var chunkSize = 64 * 1024;

            var fileBytes = File.ReadAllBytes(filePath);

            var fileChunk = new byte[chunkSize];

            var offset = 0;

            while (offset < fileBytes.Length)
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                  break;
                }

                var length = Math.Min(chunkSize, fileBytes.Length - offset);
                Buffer.BlockCopy(fileBytes, offset, fileChunk, 0, length);

                offset += length;

                chunk.ChunkSize = length;
                chunk.Chunk = Google.Protobuf.ByteString.CopyFrom(fileChunk);

                await responseStream.WriteAsync(chunk).ConfigureAwait(false);

            }

        }
    }

    class Program
    {
        const int Port = 30051;

        public static void Main(string[] args)
        {
            Server server = new Server
            {
                Services = { Greeter.BindService(new GreeterImpl()) },
                Ports = { new ServerPort("0.0.0.0", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
