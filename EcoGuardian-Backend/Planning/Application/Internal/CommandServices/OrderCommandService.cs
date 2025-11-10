using EcoGuardian_Backend.Planning.Application.Internal.OutboundServices;
using EcoGuardian_Backend.Planning.Domain.Model.Aggregates;
using EcoGuardian_Backend.Planning.Domain.Model.Commands;
using EcoGuardian_Backend.Planning.Domain.Model.Entities;
using EcoGuardian_Backend.Planning.Domain.Repositories;
using EcoGuardian_Backend.Planning.Domain.Services;
using EcoGuardian_Backend.Shared.Domain.Repositories;

namespace EcoGuardian_Backend.Planning.Application.Internal.CommandServices;

public class OrderCommandService(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IExternalUserService externalUserService) : IOrderCommandService
{
    public async Task<Order> Handle(CreateOrderCommand command)
    {
        var userId = await externalUserService.CheckUserExists(command.ConsumerId);
        var order = new Order(
            command.Action,
            userId,
            command.InstallationDate,
            command.Details?.Select(d => new OrderDetail
            {
                DeviceId = d.DeviceId,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                Description = d.Description
            }).ToList() ?? new List<OrderDetail>()
        );
        await orderRepository.AddAsync(order);
        await unitOfWork.CompleteAsync();
    
        return order;
    }

    public async Task Handle(UpdateOrderCommand command)
    {
        var order = await orderRepository.GetByIdAsync(command.Id);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {command.Id} not found.");
        }
        
        var consumerId = await externalUserService.CheckUserExists(command.ConsumerId);
        var specialistId = 0;
        if (command.SpecialistId != null)
        {
            specialistId = await externalUserService.CheckUserExists(command.SpecialistId);
        }

        order.Update(
            command.Action,
            command.StateId,
            consumerId,
            specialistId,
            command.InstallationDate
        );
        orderRepository.Update(order);
        await unitOfWork.CompleteAsync();
    }

    public async Task Handle(UpdateOrderStateCommand command)
    {
        var order = await orderRepository.GetByIdAsync(command.OrderId);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {command.OrderId} not found.");
        }
        order.UpdateState(command.StateId);
        orderRepository.Update(order);
        await unitOfWork.CompleteAsync();
    }
}