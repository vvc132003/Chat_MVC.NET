using Microsoft.AspNetCore.SignalR;
using models;
using service;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly NhanTinService tinNhanService;
        private readonly NguoiDungService nguoiDungService;
        private readonly KetBanService ketBanService;
        public ChatHub(NhanTinService tinNhanServices, NguoiDungService nguoiDungServices, KetBanService ketBanServices)
        {
            tinNhanService = tinNhanServices;
            nguoiDungService = nguoiDungServices;
            ketBanService = ketBanServices;
        }
        public async Task SendMessage(int idnguoidunggui, int idnguoidungnhan, string NoiDung)
        {
            await tinNhanService.ThemTinNhan(idnguoidunggui, idnguoidungnhan, NoiDung);
            await Clients.All.SendAsync("ReceiveMessage", idnguoidunggui, idnguoidungnhan, NoiDung);
        }
    }
}