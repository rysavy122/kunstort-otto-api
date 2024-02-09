using App.Models;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[ApiController]
[Route("api/messages")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }


    [HttpGet("public")]
    public ActionResult<Message> GetPublicMessage()
    {
        return _messageService.GetPublicMessage();
    }
    [HttpGet]
    public ActionResult<IEnumerable<Message>> GetAllMessages() => Ok(_messageService.GetAllMessages());

    [HttpGet("{id}")]
    public ActionResult<Message> GetMessageById(int id)
    {
        var message = _messageService.GetMessageById(id);
        if (message == null) return NotFound();
        return Ok(message);
    }

    [HttpPost]
    public ActionResult<Message> CreateMessage([FromBody] Message message) => Ok(_messageService.CreateMessage(message));

    [HttpPut("{id}")]
    public ActionResult<Message> UpdateMessage(int id, [FromBody] Message message) => Ok(_messageService.UpdateMessage(id, message));

    [HttpDelete("{id}")]
    public IActionResult DeleteMessage(int id)
    {
        _messageService.DeleteMessage(id);
        return NoContent();
    }

    [HttpGet("protected")]
    public ActionResult<Message> GetProtectedMessage()
    {
        return _messageService.GetProtectedMessage();
    }

    [HttpGet("admin")]
    [Authorize]
    public ActionResult<Message> GetAdminMessage()
    {
        return _messageService.GetAdminMessage();
    }
}
