using MediatR;

namespace dotnet_ultimate.Features.Products.Notifications;

public record ProductCreatedNotification(Guid Id) : INotification;
