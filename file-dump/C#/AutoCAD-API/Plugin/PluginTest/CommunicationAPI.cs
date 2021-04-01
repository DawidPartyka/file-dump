using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTest
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class Create
    {
        public string Name { get; set; }
        public List<MyLine> Lines { get; set; }
        public List<MyPolyline> Polylines { get; set; }
        public List<MyCircle> Circles { get; set; }
    }

    public class CommunicationAPI
    {
        private RestClient client;
        private string path = "Projects";
        public CommunicationAPI(string url)
        {
            client = new RestClient(url);
        }

        public Result CreateDoc(MyDoc doc)
        {
            var req = new RestRequest(path, Method.POST, DataFormat.Json).AddJsonBody(new Create() 
            { 
                Name = doc.Name,
                Lines = doc.Lines,
                Polylines = doc.Polylines,
                Circles = doc.Circles
            });

            var res = client.Post(req);

            if (res.StatusCode == System.Net.HttpStatusCode.Created)
                return new Result() { Success = true, Message = Encoding.UTF8.GetString(res.RawBytes, 0, res.RawBytes.Length) };
            else
                return new Result() { Success = false, Message = res.StatusCode.ToString() };
        }

        public Result GetDoc(int id)
        {
            var req = new RestRequest($"{path}/{id}", Method.GET, DataFormat.Json);
            var res = client.Get<MyDoc>(req);

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
                return new Result() { Success = true, Message = Encoding.UTF8.GetString(res.RawBytes, 0, res.RawBytes.Length) };
            else
                return new Result() { Success = false, Message = res.StatusCode.ToString() };
        }
    }
}
