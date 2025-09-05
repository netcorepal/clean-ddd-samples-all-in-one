﻿using RiskControl.Domain.AggregatesModel.DeliverAggregate;

namespace RiskControl.Infrastructure.Repositories;

public interface IDeliverRecordRepository : IRepository<DeliverRecord, DeliverRecordId>
{
}

public class DeliverRecordRepository(ApplicationDbContext context) : RepositoryBase<DeliverRecord, DeliverRecordId, ApplicationDbContext>(context), IDeliverRecordRepository
{
}

