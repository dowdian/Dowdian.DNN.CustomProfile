// <copyright file="DnnScheduleRepository.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using DotNetNuke.Framework;
using DotNetNuke.Services.Scheduling;

namespace Dowdian.Modules.CustomProfile.Repositories.Dnn
{
    /// <summary>
    /// IDnnUserRepository
    /// </summary>
    public partial interface IDnnScheduleRepository
    {
        /// <summary>
        /// Get a list of all the Scheduled Jobs within this DNN Instance.
        /// </summary>
        /// <returns>IEnumerable of ScheduleItem</returns>
        IEnumerable<ScheduleItem> GetSchedule();

        /// <summary>
        /// Get the history for a given Scheduled Job
        /// </summary>
        /// <param name="scheduleId">int scheduleId</param>
        /// <returns>IEnumerable of ScheduleHistoryItem</returns>
        IEnumerable<ScheduleHistoryItem> GetScheduleHistory(int scheduleId);
    }

    /// <summary>
    /// DnnUserRepository
    /// </summary>
    public partial class DnnScheduleRepository : ServiceLocator<IDnnScheduleRepository, DnnScheduleRepository>, IDnnScheduleRepository
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IEnumerable<ScheduleItem> GetSchedule()
        {
            var CustomProfileScheduledJobs = SchedulingProvider.Instance().GetSchedule().ToArray();
            List<ScheduleItem> CustomProfileScheduleList = new List<ScheduleItem>();
            foreach (ScheduleItem scheduleItem in CustomProfileScheduledJobs)
            {
                CustomProfileScheduleList.Add(scheduleItem);
            }

            return CustomProfileScheduleList;
        }

        public IEnumerable<ScheduleHistoryItem> GetScheduleHistory(int scheduleId)
        {
            var CustomProfileScheduleHistory = SchedulingProvider.Instance().GetScheduleHistory(scheduleId);
            List<ScheduleHistoryItem> CustomProfileScheduleHistoryList = new List<ScheduleHistoryItem>();
            foreach (ScheduleHistoryItem scheduleHistoryItem in CustomProfileScheduleHistory)
            {
                CustomProfileScheduleHistoryList.Add(scheduleHistoryItem);
            }

            return CustomProfileScheduleHistoryList;
        }

        protected override Func<IDnnScheduleRepository> GetFactory()
        {
            return () => new DnnScheduleRepository();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}