using EcoGuardian_Backend.Shared.Domain.Repositories;
using EcoGuardian_Backend.SubscriptionsAndPayment.Application.Internal.OutBoundServices;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.Aggregates;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.Commands;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Model.ValueObjects;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Repositories;
using EcoGuardian_Backend.SubscriptionsAndPayment.Domain.Services;

namespace EcoGuardian_Backend.SubscriptionsAndPayment.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    ISubscriptionStateRepository subscriptionStateRepository,
    IExternalPayerService externalPayerService,
    ISubscriptionTypeRepository subscriptionTypeRepository,
    IUnitOfWork unitOfWork
) : ISubscriptionCommandService
{
    public async Task<Subscription?> Handle(CreateSubscriptionCommand command)
    {
        // TODO: Validar si el usuario ya tiene una suscripción activa
        try
        {
            var userId = await externalPayerService.CheckExternalPayerExists(command.UserId);
            var subscription = new Subscription(
                userId,
                command.SubscriptionTypeId,
                command.Amount,
                command.Currency
            );

            await subscriptionRepository.AddAsync(subscription);
            await unitOfWork.CompleteAsync();

            return subscription;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error al crear la suscripción.", ex);
        }
    }


    public async Task<Subscription?> Handle(UpdateSubscriptionTypeCommand command)
    {
        try 
        {
            var userId = await externalPayerService.CheckExternalPayerExists(command.UserId);
            var subscription = await subscriptionRepository.GetByIdAsync(command.Id);
            if (subscription == null)
            {
                throw new BadHttpRequestException("No se encontró la suscripción con el ID proporcionado.");
            }
        
            var subscriptionType = await subscriptionTypeRepository.GetByIdAsync(command.SubscriptionTypeId);
            if (subscriptionType == null)
            {
                throw new BadHttpRequestException("No se encontró el tipo de suscripción con el ID proporcionado.");
            }

            // actualizamos los campos
            subscription.SubscriptionTypeId = command.SubscriptionTypeId;
            subscription.Amount = GetAmountForSubscriptionType(command.SubscriptionTypeId);
            subscription.SubscriptionStateId = (int)ESubscriptionStates.Inactive;
            subscription.UpdatedAt = DateTime.UtcNow;
            subscription.UserId = userId;
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync();
            return subscription;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error al actualizar la suscripción.", ex);
        }
        
    }


    // metodo privado para obtener el monto de la suscripción según el tipo (esto para la actualizacion de suscripción)
    private decimal GetAmountForSubscriptionType(int subscriptionTypeId)
    {
        switch (subscriptionTypeId)
        {
            case 1: return 50;
            case 2: return 100;
            case 3: return 500;
            default:
                throw new InvalidOperationException("01101100 01101100 01101111 01110010 01100001 00100000 01101101 01100001 01110100 01101001");
        }
    }
}