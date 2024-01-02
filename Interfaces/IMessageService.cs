using App.Models;
namespace App.Services;

public interface IMessageService
{
    Message GetPublicMessage();
    Message GetProtectedMessage();
    Message GetAdminMessage();

    //CRUD OPERATIONS
    IEnumerable<Message> GetAllMessages();
    Message GetMessageById(int id);
    Message CreateMessage(Message message);
    Message UpdateMessage(int id, Message message);
    void DeleteMessage(int id);

}
