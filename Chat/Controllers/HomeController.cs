using Chat.Models;
using Microsoft.AspNetCore.Mvc;
using models;
using service;
using System.Diagnostics;

namespace Chat.Controllers
{
    public class HomeController : Controller
    {
        private readonly NhanTinService tinNhanService;
        private readonly NguoiDungService nguoiDungService;
        private readonly KetBanService ketBanService;
        public HomeController(NhanTinService tinNhanServices, NguoiDungService nguoiDungServices, KetBanService ketBanServices)
        {
            tinNhanService = tinNhanServices;
            nguoiDungService = nguoiDungServices;
            ketBanService = ketBanServices;
        }
        public IActionResult dangnhap()
        {
            return View();
        }
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult DangNhapVaoHeThong(string tendangnhap, string matkhau)
        {
            NguoiDung nguoiDung = nguoiDungService.CheckThongTinDangNhap(tendangnhap, matkhau);
            if (nguoiDung != null)
            {
                HttpContext.Session.SetInt32("id", nguoiDung.id);
                HttpContext.Session.SetString("tendangnhap", nguoiDung.tendangnhap);
                HttpContext.Session.SetString("hovaten", nguoiDung.hovaten);
                HttpContext.Session.SetString("anhdaidien", nguoiDung.anhdaidien);
                return RedirectToAction("ChatBot", "Home");
            }
            else
            {
                return RedirectToAction("Privacy", "Home");

            }
        }
        public IActionResult ChatBot()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null && HttpContext.Session.GetString("anhdaidien") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string anhdaidien = HttpContext.Session.GetString("anhdaidien");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["anhdaidien"] = anhdaidien;
                List<KetBan> listketban = ketBanService.GetAllKetBanByIdNguoiDung(id);
                List<ModelData> listmoda = new List<ModelData>();
                foreach (KetBan item in listketban)
                {
                    ModelData modelDaTa = new ModelData()
                    {
                        ketBanlist = listketban,
                    };

                    listmoda.Add(modelDaTa);
                }
                return View(listmoda);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult KetBan()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null && HttpContext.Session.GetString("anhdaidien") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string anhdaidien = HttpContext.Session.GetString("anhdaidien");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["anhdaidien"] = anhdaidien;
                List<KetBan> listketban = ketBanService.GetAllKetBanByIdNguoiDungMoiNhat(id);
                List<ModelData> modeldatalist = new List<ModelData>();
                string html = "";
                foreach (var ketban in listketban)
                {
                    NguoiDung nguoidung = nguoiDungService.GetAllById(ketban.idnguoidung);
                    NhanTin tinnhan = tinNhanService.GetAllByTinNhanMoi(id, ketban.idnguoidung);
                    List<NhanTin> tinnhanlisst = tinNhanService.GetAllTinNhanByIdGuiNhan(id, nguoidung.id);
                    string messageSender = "";
                    string messageContent = tinnhan.NoiDung;
                    if (tinnhan.idnguoidunggui == id)
                    {
                        messageSender = "You:";
                    }
                    else
                    {
                        messageSender = "";
                    }
                    html += $@"
                        <li class=""nav-item unread"" onclick=""displayMessages({nguoidung.id})"">
                                                <a>
                                                    <figure>
                                                        <img src=""{nguoidung.anhdaidien}"" alt="""">
                                                        <span class=""status f-online""></span>
                                                    </figure>
                                                    <div class=""user-name"">
                                                        <h6 class="""">{nguoidung.hovaten}</h6>
                                                    <span>{messageSender} {messageContent}</span>
                                                    </div>
                                                    <div class=""more"">
                                                        <div class=""more-post-optns"">
                                                            <i class=""ti-more-alt""></i>
                                                            <ul>
                                                                <li><i class=""fa fa-bell-slash-o""></i>Mute</li>
                                                                <li><i class=""ti-trash""></i>Delete</li>
                                                                <li><i class=""fa fa-folder-open-o""></i>Archive</li>
                                                                <li><i class=""fa fa-ban""></i>Block</li>
                                                                <li><i class=""fa fa-eye-slash""></i>Ignore Message</li>
                                                                <li><i class=""fa fa-envelope""></i>Mark Unread</li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </a>
                                            </li>
";
                }
                return Content(html);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Tennguoidung(int idnguoidungnhan)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null && HttpContext.Session.GetString("anhdaidien") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string anhdaidien = HttpContext.Session.GetString("anhdaidien");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["anhdaidien"] = anhdaidien;
                string html = $@"
                                <input type=""hidden"" id=""idnguoidungnhaninput"" value=""{idnguoidungnhan}"">
                               <input type=""hidden"" id=""idnguoidungguiinput"" value=""{id}"">
                            ";
                return Content(html);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult TinNhan(int idnguoidungnhan)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null && HttpContext.Session.GetString("anhdaidien") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string anhdaidien = HttpContext.Session.GetString("anhdaidien");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["anhdaidien"] = anhdaidien;
                List<NhanTin> tinnhanlisst = tinNhanService.GetAllTinNhanByIdGuiNhan(id, idnguoidungnhan);
                NguoiDung nguoidung = nguoiDungService.GetAllById(idnguoidungnhan);
                string html = "";
                foreach (var tinNhan in tinnhanlisst)
                {
                    string liClass = (tinNhan.idnguoidungnhan == id) ? "me" : "you";
                    string avatarUrl = (liClass == "me") ? nguoidung.anhdaidien : anhdaidien;
                    string thoigiangui = tinNhan.ThoiGianGui.ToString("HH:mm");
                    html += $@"<li class=""{liClass}"">
                                                    <figure><img src=""{avatarUrl}"" alt=""""></figure>
                                                    <div class=""text-box"">
                                                        <p>
                                                            {tinNhan.NoiDung}
                                                        </p>
                                                        <span>
                                                            <i class=""ti-check""></i><i class=""ti-check""></i>
                                                            {thoigiangui}
                                                        </span>
                                                    </div>
                                                </li>";
                }
                return Content(html);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}