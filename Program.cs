// using System;
// using System.Collections.Concurrent;
// using System.Diagnostics;
// using System.Globalization;
// using System.Linq;
// using System.Threading.Tasks;
// using CsvHelper;
// using CsvHelper.Configuration;
// using System.Object;
//dotnet add package <CsvReader>
using System.Text.Json;
using Newtonsoft.Json;

namespace Parallelism
{
    class Program
    {
        //private static object sync=new object();
        static async Task Main(string[] args)
        {
            //msft.us, aapl.us
            //Console.WriteLine("Tickers: ");
            //string? ticker=Console.ReadLine();
            //string[] strings = ticker.Split();
            //var tickers= strings;
           // var stockTasks=ticker.Select(t=> GetStocksAsync(t));
            //var stockData=await Task.WhenAll(stockTasks);
            //foreach(var t in tickers)
            //{
                await guardarPost();
            //}

        
        }


        static readonly HttpClient client = new HttpClient();

        static async Task guardarPost()
        {
            List<PostData> posts;
            //List<string> pp=new List<string>();
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try	
            {
                HttpResponseMessage response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                //string responseBody = await client.GetStringAsync(uri);
                //Console.WriteLine(responseBody);
                //var savejson=JsonSerializer.Serialize(responseBody);
                posts =JsonConvert.DeserializeObject<List<PostData>>(responseBody);
                Parallel.ForEach(posts, p=>
                {
                    posts.Add(p);
                    //pp.Add(p.Title);
                    Console.WriteLine("PostTitle: " + p.Title);
                    
                });
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            //Console.WriteLine(pp.Count());
        }

        public async Task createFile(String token){
            var postId=token;
            string folderName=@"C:\Users\rolan\Documents\UNIVERSIDAD\concurrencia\tarea2AP\Posts";
            System.IO.Directory.CreateDirectory(folderName);
            string fileName=postId;
            folderName=System.IO.Path.Combine(folderName,fileName);
            Console.WriteLine("Direccion de mi Archivo: {0}\n",folderName);
             if (!System.IO.File.Exists(folderName))
        {
            using (System.IO.FileStream fs = System.IO.File.Create(folderName))
            {
               
            }
        }
        else
        {
            Console.WriteLine("File \"{0}\" already exists.", fileName);
            return;
        }

        // Read and display the data from your file.
        try
        {
            byte[] readBuffer = System.IO.File.ReadAllBytes(folderName);
            foreach (byte b in readBuffer)
            {
                Console.Write(b + " ");
            }
            Console.WriteLine();
        }
        catch (System.IO.IOException e)
        {
            Console.WriteLine(e.Message);
        }

        // Keep the console window open in debug mode.
        System.Console.WriteLine("Press any key to exit.");
        System.Console.ReadKey();
    
            // await File.WriteAllTextAsync(@"C:\Users\rolan\Documents\UNIVERSIDAD\concurrencia\tarea2AP\{postId}.txt", postId);
            //Parallel.ForEach();
        }

    }

    
    public class PostData{

        public int User{get; set;}
        public int Id {get; set;}
        public string? Title {get; set;}
        public string? Body {get; set;}
    }

    public class Comments{
        public int PostId{get; set;}
        public int Id {get; set;}
        public string? Name {get; set;}
        public string? Email {get; set;}
        public string? Body {get; set;}

    }
    
}
