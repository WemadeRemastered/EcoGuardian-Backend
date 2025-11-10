using EcoGuardian_Backend.Planning.Domain.Model.Commands;
using EcoGuardian_Backend.Planning.Domain.Model.Entities;
using EcoGuardian_Backend.Shared.Interfaces.Helpers;

namespace EcoGuardian_Backend.Planning.Domain.Model.Aggregates;

public class Order
{
    public int Id { get; }
    public string Action { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public int StateId { get; private set; }
    public int ConsumerId { get; private set; }
    public int? SpecialistId { get; private set; }
    public DateTime? InstallationDate { get; private set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public Order()
    {
        Action = string.Empty;
        CreatedAt = DateTimeConverterHelper.ToNormalizeFormat(DateTime.UtcNow);
        CompletedAt = null;
        StateId = 1;
        ConsumerId = 0;
        SpecialistId = null;
        InstallationDate = null;
    }

    public Order(
        string action,
        int consumerId,
        DateTime? installationDate,
        List<OrderDetail>? orderDetails)
    {
        Action = action;
        CreatedAt = DateTimeConverterHelper.ToNormalizeFormat(DateTime.UtcNow);
        CompletedAt = null;
        StateId = 1;
        ConsumerId = consumerId;
        SpecialistId = null;
        InstallationDate = installationDate;
        OrderDetails = orderDetails ?? [];
    }

    public void Update(
        string action,
        int stateId,
        int consumerId,
        int? specialistId,
        DateTime? installationDate)
    {
        Action = action;
        CompletedAt = DateTimeConverterHelper.ToNormalizeFormat(DateTime.UtcNow);
        StateId = stateId;
        ConsumerId = consumerId;
        SpecialistId = specialistId;
        InstallationDate = installationDate;
    }

    public void UpdateState(int newStateId)
    {
        StateId = newStateId;
    }
}
