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
        private readonly TinNhan_IconService tinNhan_IconService;
        public ChatHub(NhanTinService tinNhanServices, NguoiDungService nguoiDungServices, KetBanService ketBanServices, TinNhan_IconService tinNhan_IconServices)
        {
            tinNhanService = tinNhanServices;
            nguoiDungService = nguoiDungServices;
            ketBanService = ketBanServices;
            tinNhan_IconService = tinNhan_IconServices;
        }
        public async Task SendMessage(int idnguoidunggui, int idnguoidungnhan, string NoiDung)
        {
            await tinNhanService.ThemTinNhan(idnguoidunggui, idnguoidungnhan, NoiDung);
            await Clients.All.SendAsync("ReceiveMessage", idnguoidunggui, idnguoidungnhan, NoiDung);
        }
        public async Task SendTimTinNhan(int idicon, int idntinnhan, int idnguoidungnhan)
        {
            await tinNhan_IconService.ThemTinNhanIcon(idntinnhan, idicon);
            await Clients.All.SendAsync("ReceiveMessage", idicon, idntinnhan, idnguoidungnhan);
        }
    }
}