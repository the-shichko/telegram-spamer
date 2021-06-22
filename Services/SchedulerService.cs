using System;
using System.Collections.Generic;
using System.Threading;

namespace telegram_spamer.Services
{
    public class SchedulerService
    {
        private static SchedulerService _instance;
        private readonly List<Timer> _timers = new List<Timer>();

        public static SchedulerService Instance => _instance ??= new SchedulerService();

        public void ScheduleTask(double intervalInHour, Action task)
        {
            var timer = new Timer(x => { task.Invoke(); }, null, TimeSpan.Zero, TimeSpan.FromHours(intervalInHour));
            _timers.Add(timer);
        }

        public void StopTasks()
        {
            _timers.Clear();
        }
    }
    
    public static class Scheduler
    {
        public static void IntervalInSeconds(double interval, Action task)
        {
            interval /= 3600;
            SchedulerService.Instance.ScheduleTask(interval, task);
        }
        public static void IntervalInMinutes(double interval, Action task)
        {
            interval /= 60;
            SchedulerService.Instance.ScheduleTask(interval, task);
        }
        public static void IntervalInHours(double interval, Action task)
        {
            SchedulerService.Instance.ScheduleTask(interval, task);
        }
        public static void IntervalInDays(double interval, Action task)
        {
            interval *= 24;
            SchedulerService.Instance.ScheduleTask(interval, task);
        }

        public static void StopTasks()
        {
            SchedulerService.Instance.StopTasks();
        }
    }
}