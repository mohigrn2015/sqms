using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Irony.Parsing;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SQMS.Models;
using SQMS.Utility;
using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using System.Threading.Tasks; 

namespace SQMS.SignalRHub
{

    public class notifyDisplay : Hub
    {
        private readonly IHubContext<notifyDisplay> _hubContext;
        public notifyDisplay(IHubContext<notifyDisplay> hubContext)
        {
            _hubContext = hubContext;

        }
        public override async Task OnConnectedAsync()
        {

            await base.OnConnectedAsync();
        }

        public async Task CallToken(int counter_id)
        {

            try
            {
                if (counter_id > 0)
                {
                    await _hubContext.Clients.All.SendAsync("callToken", counter_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            //finally
            //{
            //    TextLogger textLogger = new TextLogger();
            //    textLogger.LogWrite("callToken: " + _hubContext + " responseModel: " + _hubContext.Clients.All);
            //}
        }

        public async Task CounterStatusChanged(int branch_id)
        {
            try
            {
                if (branch_id > 0)
                {
                    await _hubContext.Clients.All.SendAsync("counterStatusChanged", branch_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SendNotification(string users, string message, bool hasAttachment, int notification_id)
        {
            try
            {
                if (users.Length > 0)
                {
                    await _hubContext.Clients.All.SendAsync("sendNotification", users, message, hasAttachment, notification_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SendMessages(int branch_id, string counter_no, string token_no, bool tokenAdded, bool tokenCalled, bool playListChanged, bool footerChanged)
        {
            try
            {
                if (token_no == "")
                {
                    await _hubContext.Clients.All.SendAsync("updateMessages", "", branch_id, counter_no, token_no, tokenAdded, tokenCalled, playListChanged, footerChanged);
                }
                else
                {
                    string text = string.Format(ApplicationSetting.voiceText, token_no, counter_no);
                    await _hubContext.Clients.All.SendAsync("updateMessages", text, branch_id, counter_no, token_no, tokenAdded, tokenCalled, playListChanged, footerChanged);
                }
                if (branch_id > 0)
                {
                    Loggin(branch_id, counter_no, token_no, tokenAdded, tokenCalled, playListChanged, footerChanged);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void Loggin(int branch_id, string counter_no, string token_no, bool tokenAdded, bool tokenCalled, bool playListChanged, bool footerChanged)
        {
            if (ApplicationSetting.AllowSignalRLoggin)
            {
                SignalRBroadcastLog log = new SignalRBroadcastLog()
                {
                    branch_id = branch_id,
                    counter_no = counter_no,
                    token_no = token_no,
                    is_token_added = (tokenAdded ? 1 : 0),
                    is_token_called = (tokenCalled ? 1 : 0),
                    is_playlist_changed = (playListChanged ? 1 : 0),
                    is_footer_changed = (footerChanged ? 1 : 0)
                };
                new BLL.BLLLog().SignalRBroadCastLogCreate(log);
            }
        }
    }
}
