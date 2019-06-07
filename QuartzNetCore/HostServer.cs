using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;
using System.Threading;
using System.Threading.Tasks;

namespace QuartzNetCore
{
    public class HostServer : IHostedService
    {
        private readonly ILogger _logger;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;

        public HostServer(ILogger<HostServer> logger, ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }

        public IScheduler Scheduler { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("开始Quartz调度...");

            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;
            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("停止Quartz调度...");

            await Scheduler.Shutdown(cancellationToken);
        }
    }
}