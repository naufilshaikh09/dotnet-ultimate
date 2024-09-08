using MediatR;

namespace dotnet_ultimate.Features.Products.Notifications;

public record ProductCreatedNotification(int Id) : INotification;
