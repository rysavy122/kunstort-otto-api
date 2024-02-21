using App.Models;
using App.Data;
using System.Collections.Generic;
using System.Linq;

namespace App.Services
{
    public class MessageService : IMessageService
    {
        private readonly OttoDbContext _context;

        public MessageService(OttoDbContext context)
        {
            _context = context;
        }

        public Message GetAdminMessage()
        {
            var message = new Message { Text = "This is an admin message." };
            return message;
        }

        public Message GetProtectedMessage()
        {
            var message = new Message { Text = "This is a protected message." };
            return message;
        }

        public Message GetPublicMessage()
        {
            var message = new Message { Text = "Hier wird die Öffentliche Forschungsfrage stehen." };
            return message;
        }

        public Message CreateMessage(Message message)
        {
            if (_context != null)
            {
                _context.Messages.Add(message);
                _context.SaveChanges();
            }
            return message;
        }

        public IEnumerable<Message> GetAllMessages()
        {
            return _context.Messages.ToList();
        }

        public Message GetMessageById(int id)
        {
            return _context?.Messages.FirstOrDefault(m => m.Id == id);
        }

        public Message UpdateMessage(int id, Message message)
        {
            var existingMessage = _context.Messages.FirstOrDefault(m => m.Id == id);
            if (existingMessage != null)
            {
                existingMessage.Text = message.Text;
                _context.SaveChanges();
            }
            return existingMessage;
        }

        public void DeleteMessage(int id)
        {
            var message = _context.Messages.FirstOrDefault(m => m.Id == id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                _context.SaveChanges();
            }
        }

        private void SaveMessage(Message message)
        {
            if (_context != null)
            {
                _context.Messages.Add(message);
                _context.SaveChanges();
            }
        }
    }
}
