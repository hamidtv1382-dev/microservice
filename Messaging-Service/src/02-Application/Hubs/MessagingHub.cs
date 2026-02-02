using Microsoft.AspNetCore.SignalR;

namespace Messaging_Service.src._02_Application.Hubs
{
    public class MessagingHub : Hub
    {
        // اضافه کردن کاربر به گروه هنگام اتصال
        public override async Task OnConnectedAsync()
        {
            var chatId = Context.GetHttpContext()?.Request.Query["chatId"].ToString();
            if (!string.IsNullOrEmpty(chatId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var chatId = Context.GetHttpContext()?.Request.Query["chatId"].ToString();
            if (!string.IsNullOrEmpty(chatId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
