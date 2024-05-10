using System;
using System.Collections.Generic;
using System.Text;
using Elasticsearch.Net;
using Nest;

namespace CaiAdmin.Service
{
    public class TestEsService
    {
        public ElasticClient ElasticClient;

        public TestEsService()
        {
            var uris = new List<Uri> { new Uri("http://localhost:9200") };

            var pool = new SniffingConnectionPool(uris);

            //var setting = new ConnectionSettings(new Uri("http://localhost:9200"));
            var setting = new ConnectionSettings(pool);
            //setting.DefaultMappingFor(typeof(TestEsModel), c => c.IndexName("es09").IdProperty("Keyword"));
            var esClient = new ElasticClient(setting);
            this.ElasticClient = esClient;
        }

        public CreateIndexResponse CreatIndex(string indexName)
        {
            //if (this.ElasticClient.Indices.Exists(indexName).Exists)
            //{
            //    return;
            //}
            //var indexState = new IndexState()
            //{
            //    Settings = new IndexSettings
            //    {
            //        NumberOfReplicas = 1,
            //        NumberOfShards = 5
            //    }
            //};
            var res = this.ElasticClient.Indices.Create(indexName, cd => cd.Settings(s => s.NumberOfShards(5).NumberOfReplicas(0)).Map<TestEsModel2>(m => m.AutoMap()));
            return res;
        }

        public DeleteIndexResponse DeleteIndex(string indexName)
        {
            return this.ElasticClient.Indices.Delete(indexName);
        }

        public CreateResponse CreateDoc()
        {
            return this.ElasticClient.CreateDocument(new TestEsModel()
            {
                TestKeyword = "Hello",
                TestBool = true,
                TestText3 = string.Empty,
                TestText2 = "123"
            });
        }

        public CreateResponse CreateDoc2()
        {
            return this.ElasticClient.Create(new TestEsModel()
            {
                TestKeyword = Guid.NewGuid().ToString(),
                TestBool = true,
                TestText3 = string.Empty,
                TestText2 = "1234",
                Models = new List<TestEsSubModel> {
                     new TestEsSubModel{
                      Name = DateTime.Now.ToString()
                     },
                     new TestEsSubModel{
                      Name = "Caijt"
                     },
                     new TestEsSubModel{
                      Name = "Jorn"
                     },
                     new TestEsSubModel{
                      Name = "I am black"
                     }
                 }
            }, d => d.Index("es10").Id("testKeyword"));
        }
        public IndexResponse SaveDoc()
        {
            return this.ElasticClient.Index(new TestEsModel()
            {
                Keyword = "Hello1",
                TestText = "1",
                TestKeyword = "Hello",
                TestBool = true,
                TestText3 = string.Empty,
                TestText2 = "1234",
                Text02 = "444",
                Text = "555"
            }, d => d.Index("es09"));
        }

        /// <summary>
        /// 部分更新
        /// </summary>
        /// <returns></returns>
        public UpdateResponse<TestEsModel> UpdateDoc()
        {
            return this.ElasticClient.Update<TestEsModel>("hehehe", o => o.Index("es09").Doc(new TestEsModel
            {
                TestText = DateTime.Now.ToString()
            }));
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <returns></returns>
        public DeleteResponse DeleteDoc()
        {
            return this.ElasticClient.Delete<TestEsModel>("hehehe", o => o.Index("es09"));
        }

        public BulkAllObservable<TestEsModel> BulkAll()
        {
            var list = new List<TestEsModel> {
                new TestEsModel{
                     Int=1,
                     Keyword = "1",
                     TestKeyword = DateTime.Now.ToString()
                },
                new TestEsModel{
                     Int=1,
                     Keyword = "2",
                     TestKeyword = DateTime.Now.ToString()

                }

            };
            var r = new BulkAllRequest<TestEsModel>(list);
            r.Index = "es09";

            //return this.ElasticClient.BulkAll(r);
            return this.ElasticClient.BulkAll(list, d => d.Index("es09"));
        }

        public BulkResponse Bulk()
        {
            var list = new List<TestEsModel> {
                new TestEsModel{
                     Int=1,
                      Keyword = "1",
                     TestKeyword = DateTime.Now.ToString()
                },
                new TestEsModel{
                     Int=1,
                    Keyword = "2",
                     TestKeyword = DateTime.Now.ToString()
                }

            };
            var r = new BulkAllRequest<TestEsModel>(list);
            r.Index = "es09";

            //return this.ElasticClient.BulkAll(r);
            return this.ElasticClient.Bulk(d => d.Index("es09").IndexMany(list, (d, e) => d.Id(e.Keyword)));
        }

        //public BulkResponse Search()
        //{
        //    var s = new SearchRequest<TestEsModel>();
        //    s.Query = new QueryContainer();
        //    var d = new SearchDescriptor<TestEsModel>();
        //    d.MatchAll();
        //    d.Query(a =>a.Bool(b=>b.Filter) a.Term("1", 1) || a.Term("1", 2));



        //    this.ElasticClient.Search<TestEsModel>(d);
        //}

    }

    public class TestEsModel
    {
        [Keyword]
        public string Keyword { get; set; }
        [Text]
        public string TestText { get; set; }
        [Keyword]
        public string TestKeyword { get; set; }
        [PropertyName("testtext2")]
        [Text(Fielddata = true)]
        public string TestText2 { get; set; }
        [PropertyName("testText")]
        [Keyword]
        public string TestText1 { get; set; }
        [Text(Name = "testText33")]
        public string TestText3 { get; set; }
        [Boolean]
        public bool TestBool { get; set; }

        [Number(NumberType.Integer)]
        public int Int { get; set; }

        public List<string> Array { get; set; }

        public TestEsSubModel Sub { get; set; }
        [Text]
        public string Text { get; set; }
        public string Text01 { get; set; }

        public string Text02 { get; set; }

        public List<TestEsSubModel> Models { get; set; }
        public List<int> Ints { get; set; }

    }

    public class TestEsSubModel
    {
        [Keyword]
        public string Name { get; set; }

        [Number(Ignore = true)]
        public int Age { get; set; }
    }

    public class TestEsModel2
    {
        public string Name { get; set; }
        public int Age { get; set; }
        [Nested]
        public List<TestEsModel3> Phones { get; set; }
    }
    public class TestEsModel3
    {
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
