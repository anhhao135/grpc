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
using Grpc.Core;
using Helloworld;
using System.IO;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace GreeterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Channel channel = new Channel("192.0.5.251:30051", ChannelCredentials.Insecure);
            await GetCatPic(channel);

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();


      /*




        var client = new Greeter.GreeterClient(channel);
            String user = "you";

            var reply = client.SayHello(new HelloRequest { Name = user });
            Console.WriteLine("Greeting: " + reply.Message);

            var goodbyeReply = client.SayGoodbye(new GoodbyeRequest { Name = "john" });
            Console.WriteLine(goodbyeReply.Message);

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();


      */
    }

        public static async Task GetCatPic(Channel channel)
    {
      var catClient = new Greeter.GreeterClient(channel);

      var catRequest = new CatPicRequest { FilePath = "nebula.tif" };

      var tempFile = $"temp.tmp";

      var finalFile = tempFile;

      using (var call = catClient.SendCatPic(catRequest))
      {
        await using (var fs = File.OpenWrite(tempFile))
        {
          await foreach (var chunk in call.ResponseStream.ReadAllAsync().ConfigureAwait(false))
          {
            var totalSize = chunk.FileSize;

            finalFile = chunk.FileName;

            if (chunk.Chunk.Length == chunk.ChunkSize)
            {
              fs.Write(chunk.Chunk.ToByteArray());
            }

            else
            {
              fs.Write(chunk.Chunk.ToByteArray(), 0, chunk.ChunkSize);
              Console.WriteLine("final chunk size: " + chunk.ChunkSize);
            }
          }
        }

        Console.WriteLine(tempFile);

        if (finalFile != tempFile)
        {
          File.Move(tempFile, finalFile);
        }






      }
    }

    public static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
    {
      DirectoryInfo info = new DirectoryInfo(filePath);
      if (!info.Exists)
      {
        info.Create();
      }

      string path = Path.Combine(filePath, fileName);
      using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
      {
        inputStream.CopyTo(outputFileStream);
      }
    }


  }


}
