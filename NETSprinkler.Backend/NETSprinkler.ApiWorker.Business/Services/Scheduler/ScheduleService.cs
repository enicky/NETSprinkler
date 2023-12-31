﻿using AutoMapper;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NETSprinkler.Common.Repositories;
using NETSprinkler.Contracts.Entity.Schedule;
using NETSprinkler.Models.Entity.Schedule;

namespace NETSprinkler.ApiWorker.Business.Services.Scheduler
{
    public interface IScheduleService
	{
        Task AddAsync(ScheduleDto schedule);
        Task DeleteAsync(int id);
        Task<List<ScheduleDto>> GetAll(CancellationToken cancellationToken);
        Task<ScheduleDto> GetScheduleById(int jobId);
        Task DisableSchedule(int id);
        Task EnableSchedule(int id);
        Task SetName(int scheduleId, string name);
    }


    public class ScheduleService : IScheduleService
    {
        private readonly ILogger<ScheduleService> logger;
        private readonly IRepositoryAsync<Schedule> schedulerRepository;
        private readonly IMapper mapper;

        public ScheduleService(ILogger<ScheduleService> logger, IRepositoryAsync<Schedule> schedulerRepository, IMapper mapper)
		{
            this.logger = logger;
            this.schedulerRepository = schedulerRepository;
            this.mapper = mapper;
        }

        public async Task AddAsync(ScheduleDto schedule)
        {
            await schedulerRepository.AddAsync(mapper.Map<Schedule>(schedule)).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            var schedule = await schedulerRepository.GetById(id).ConfigureAwait(false);
            if (schedule == null) return;
            await schedulerRepository.DeleteAsync(schedule);
        }

        public async Task DisableSchedule(int id)
        {
            var schedule = await schedulerRepository.GetById(id).ConfigureAwait(false);
            schedule!.Enabled = false;

        }

        public async Task EnableSchedule(int id)
        {
            var schedule = await schedulerRepository.GetById(id).ConfigureAwait(false);
            schedule!.Enabled = true;
        }

        public async Task<List<ScheduleDto>> GetAll(CancellationToken cancellationToken)
        {
            logger.LogInformation($"[ScheduleService:GetAll] retrieve all schedules from scheduler repository");
            return mapper.Map<List<ScheduleDto>>(await schedulerRepository.Entities.ToListAsync(cancellationToken));
        }

        public async Task<ScheduleDto> GetScheduleById(int jobId)
        {
            logger.LogInformation($"[ScheduleService:GetAll] retrueve ScheduleDto by id {jobId}");
            return mapper.Map<ScheduleDto>(await schedulerRepository.Entities.FirstOrDefaultAsync(q => q.Id == jobId).ConfigureAwait(false));
        }

        public async Task SetName(int scheduleId, string name)
        {
            var schedule = await schedulerRepository.GetById(scheduleId);
            if (schedule == null) return;
            schedule.Name = name;
        }
    }
}

