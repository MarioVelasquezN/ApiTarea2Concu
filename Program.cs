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
        static async Task Main(string[] args)
        {
             string path =@"data";

            try{
                if(Directory.Exists(path)){
                    Console.WriteLine("El directorio ya esta creado");
                } 
                else{
                    DirectoryInfo inf =Directory.CreateDirectory(path);
                }
            }catch(Exception e){
                Console.WriteLine("El directorio Fallo: {0}",e.ToString());
            }

            List<PostData> postlist = new List<PostData>();
            List<Comments> comments = new List<Comments>();
            Task cargar=Task.Run(async() =>
            {
                postlist=await guardarPost(postlist);
            });
            cargar.Wait();
            
            Parallel.For(0,postlist.Count,index=>
            {
                Task algo=Task.Run(async() =>
                {
                    comments=await guardarComments(comments,index);
                    string path=$@"data/{postlist[index]}.txt";
                    //using (StreamReader sr=File.Exists(path)?File.CreateText(path):File.AppendText(path)); //
                    await File.WriteAllTextAsync(path,$"{comments[index].Name}");
                    await File.AppendAllTextAsync(path,$"{comments[index].Body}");
                });
                algo.Wait();
            });


            
        }


        static readonly HttpClient client = new HttpClient();

        static async Task<List<PostData>>guardarPost(List<PostData> posts)
        {
            try	
            {
                HttpResponseMessage response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
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
            return posts;
            
        }

        static async Task<List<Comments>>guardarComments(List<Comments> com,int poss)
        {
            try	
            {
                HttpResponseMessage response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{poss}/comments");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                com =JsonConvert.DeserializeObject<List<Comments>>(responseBody);
                Parallel.ForEach(com, p=>
                {
                    com.Add(p);
                    Console.WriteLine("Comments: " + p.Name);
                });
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
            return com;
            
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
