using AutoMapper;
using ePizza.Domain.Models;
using ePizza.Domain.Interfaces;
using ePizza.Infrastructure.Entities;

namespace ePizza.Infrastructure.Repositories; //this semicolon added will used to remove the curly braces
public class PaymentRepository : GenericRepository<PaymentDomain, PaymentDetail>, IPaymentRepository
{
    public PaymentRepository(ePizzaDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}
