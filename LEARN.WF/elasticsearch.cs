namespace LEARN.WF;

using Kusto.Data.Data;
using MongoDB.Driver;
using Nest;
using System.Net;

public class elasticsearch
{
    public class Shop
    {
        public string UUID { set; get; }

        public string ItemType { set; get; }

        public long ItemId { set; get; }

        public string ItemName { set; get; }

        public long Gold { set; get; }

        public long Number { set; get; }

        public string Data { set; get; }

        public string Quality { set; get; }

        public string Category { set; get; }

        public string SellerIp { set; get; }
    }


    public void  ExecuteAsync()
    { 
        var connsetting = new ConnectionSettings(new Uri("http://localhost:9200"));
        var indexname = "say_index";
        var esclient = new ElasticClient(connsetting.DefaultIndex(indexname));
        var shop = new Shop()
        {
            UUID = "123456",
            ItemName="dfdf"
        };

        var res2 =  esclient.IndexAsync(shop, g => g.Index(indexname));

        var result = esclient.Search<Shop>(
                s => s
                    .Explain() //参数可以提供查询的更多详情。            

            .Source(sc => sc.Includes(ic => ic
                    .Fields(
                        fd => fd.UUID
                        ))
            ));

        Console.WriteLine(result);
    }
}
