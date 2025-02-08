using CommunityToolkit.Mvvm.Messaging;

namespace SchoolManage.App.Services;

public interface IMessengerService
{
    IMessenger Messenger { get; }

    void Send<TMessage>(TMessage message)
        where TMessage : class;
}
