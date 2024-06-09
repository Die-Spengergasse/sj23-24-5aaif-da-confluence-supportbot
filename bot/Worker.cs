using Bogus;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Supportbot.Bot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Mosviewer.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ElasticsearchClient _client;

        private readonly List<string> _processedFiles = new List<string>();

        private readonly string _directoryPath = @"Supportdokumente";
        public Worker(ILogger<Worker> logger, ElasticsearchClient client)
        {
            _logger = logger;
            _client = client;
        }


        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            TimeSpan delay = TimeSpan.FromSeconds(10);
            // DateTime nextRun = DateTime.MinValue;
            // DateTime lastFileDate = DateTime.MinValue;
            // Randomizer.Seed = new Random(1811);
            // var supportDocumentFaker = new Faker<SupportDocument>().CustomInstantiator(f => new SupportDocument(
            //     f.Lorem.Sentence(),
            //     DateTime.UtcNow,
            //     f.Lorem.Paragraphs(3)));

            await _client.Indices.DeleteAsync(new DeleteIndexRequest(Indices.Index("supportdocument-idx")));

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {

                    // if (nextRun < DateTime.UtcNow)
                    // {
                    _logger.LogInformation("Checking for new files at {time}", DateTimeOffset.Now);

                    //     var document = supportDocumentFaker.Generate();
                    //     var response = await _client.IndexAsync(document);
                    //     if (!response.IsValidResponse)
                    //     {
                    //         _logger.LogError("Error indexing document: {error}", response.ElasticsearchServerError);
                    //     }

                    //     nextRun = new DateTime((lastFileDate.Ticks / TimeSpan.TicksPerHour + 1) * TimeSpan.TicksPerHour, DateTimeKind.Utc);
                    // }

                    if (!Directory.Exists(_directoryPath))
                    {
                        _logger.LogWarning("Directory does not exist : {directoryPath}", _directoryPath);
                        await Task.Delay(delay, cancellationToken);
                        continue;
                    }

                    var newFiles = Directory.GetFiles(_directoryPath, "*.txt")
                                                .Where(file => !_processedFiles.Contains(file))
                                                .ToList();

                    foreach (var file in newFiles)
                    {
                        try
                        {

                            var content = await File.ReadAllTextAsync(file, cancellationToken);
                            _logger.LogInformation("Read content from file {file}", file);

                            var document = new SupportDocument(
                                Title: Path.GetFileName(file), 
                                Created: DateTime.UtcNow, Content: content);

                            var response = await _client.IndexAsync(document);

                            if (response.IsValidResponse)
                            {
                                _processedFiles.Add(file);
                                _logger.LogInformation("Indexed file {file}", file);
                            }
                            else
                            {
                                _logger.LogError("Error indexing document: {error}", response.ElasticsearchServerError);
                            }

                        }

                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing file {file}", file);
                        }

                    }

                }

                catch (TaskCanceledException)
                {
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
                await Task.Delay(delay, cancellationToken);
            }
        }
    }
}