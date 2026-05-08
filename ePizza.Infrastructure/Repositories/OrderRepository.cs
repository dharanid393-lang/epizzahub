using AutoMapper;
using ePizza.Domain.Interfaces;
using ePizza.Domain.Models;
using ePizza.Infrastructure.Entities;
using System;
namespace ePizza.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<OrderDomain, Order>, IOrderRepository
    {
        public OrderRepository(ePizzaDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
