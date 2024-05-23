using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Mosviewer.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<ElasticsearchClient>(sp =>
                    {
                        var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
                            .DefaultIndex("supportdocument-idx");
                        return new ElasticsearchClient(settings);
                    });
                    services.AddHostedService<Worker>();
                });
    }
}
