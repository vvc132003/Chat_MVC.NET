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
                                                            <i class=""fas fa-ellipsis-h""></i>
                                                            <ul>
                                                                <li><i class=""fas fa-bell-slash""></i>Mute</li>
                                                                <li><i class=""fas fa-trash-alt""></i>Delete</li>
                                                                <li><i class=""fas fa-folder-open""></i>Archive</li>
                                                                <li><i class=""fas fa-ban""></i>Block</li>
                                                                <li><i class=""fas fa-eye-slash""></i>Ignore Message</li>
                                                                <li><i class=""fas fa-envelope""></i>Mark Unread</li>
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
                if (tinnhanlisst.Count > 0)
                {
                    foreach (var tinNhan in tinnhanlisst)
                    {
                        string classs = (tinNhan.idnguoidungnhan == id) ? "me" : "you";
                        string anh = (classs == "me") ? nguoidung.anhdaidien : anhdaidien;
                        string thoigiangui = tinNhan.ThoiGianGui.ToString("HH:mm");
                        html += $@"<li class=""{classs}"">
                                                    <figure><img src=""{anh}"" alt=""""></figure>
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
                }
                else
                {
                    html += @"
                    <div class=""mesge-area conversations"">
											<div class=""empty-chat"">
												<div class=""no-messages"">
													<i class=""fas fa-comments""></i> 
													<p>Nhập tin nhắn để gửi</p>
												</div>
											</div>
										</div>
";
                }
                return Content(html);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Nguoidung(int idnguoidungnhan)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null && HttpContext.Session.GetString("anhdaidien") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string anhdaidien = HttpContext.Session.GetString("anhdaidien");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["anhdaidien"] = anhdaidien;
                NguoiDung nguoidung = nguoiDungService.GetAllById(idnguoidungnhan);
                string html = "";
                html += $@"
                                                <figure>
                                                    <img src=""{nguoidung.anhdaidien}"" alt="""">
                                                    <span class=""status f-online""></span>
                                                </figure>
                                                <div>
                                                    <h6 class=""unread"">{nguoidung.hovaten}</h6>
                                                    <span>Online</span>
                                                </div>
";

                return Content(html);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult ThongTinNguoidung(int idnguoidungnhan)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null && HttpContext.Session.GetString("anhdaidien") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string anhdaidien = HttpContext.Session.GetString("anhdaidien");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["anhdaidien"] = anhdaidien;
                NguoiDung nguoidung = nguoiDungService.GetAllById(idnguoidungnhan);
                string html = "";
                html += $@"
                                        <figure><img src=""{nguoidung.anhdaidien}"" alt=""""></figure>
                                            <h6>{nguoidung.hovaten}</h6>
                                            <span>Online</span>
                                            <div class=""userabout"">
                                                <span>About</span>
                                                <p>
                                                    I love reading, traveling and discovering new things. You need to
                                                    be happy in life.
                                                </p>
                                                <ul>
                                                    <li><span>Phone:</span> +123976980</li>
                                                    <li>
                                                        <span>Website:</span> <a href=""#"" title="""">www.abc.com</a>
                                                    </li>
                                                    <li>
                                                        <span>Email:</span> <a href=""http://wpkixx.com/cdn-cgi/l/email-protection""
                                                                               class=""__cf_email__""
                                                                               data-cfemail=""a0d3c1cdd0ccc5e0c7cdc1c9cc8ec3cfcd"">[email&#160;protected]</a>
                                                    </li>
                                                    <li><span>Phone:</span> Ontario, Canada</li>
                                                </ul>
                                            </div>
";

                return Content(html);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}