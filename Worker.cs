using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;
using WorkerService.Test.Models;
using WorkerService.Test.Util;

namespace WorkerService.Test
{
    public class Worker : BackgroundService {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger) {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            try {
                string pathToFile = "./Names.json";
                List<Person> people  = await FileParse.ReturnContentsOfFileAsync<List<Person>>(pathToFile);
                Random rand = new Random();
            
                

                while(!stoppingToken.IsCancellationRequested) {
                    Person person =  people[rand.Next(0, people.Count)];
                    if(_logger.IsEnabled(LogLevel.Information)) {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                        _logger.LogWarning($"{person.id}\n  {person.name}\n\n");
                    }
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch(OperationCanceledException) {
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
            }
            catch(Exception ex) {
                _logger.LogError(ex, "{Message}", ex.Message);

                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }



        }
    }
}
