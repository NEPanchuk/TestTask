using AutoMapper;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using TestTask.BLL.DTO;
using TestTask.BLL.Services;
using TestTask.WEB.Models;

namespace TestTask.WEB.Controllers
{
    public class HomeController : Controller
    {
        private UserService userService { get; set; }
        public HomeController() 
        {
            userService = new UserService();
        }
        public ActionResult Index()
        {
            var userInfo = userService.ViewUserInfoDTO();

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserInfoDTO, UserInfoViewModel>()).CreateMapper();
            ViewBag.usersInfo = mapper.Map<IEnumerable<UserInfoDTO>, List<UserInfoViewModel>>(userInfo);

            return View("Index");
        }

        public ActionResult Upload(HttpPostedFileBase csvFile)
        {
            List<string[]> listUserData = userService.UploadData(csvFile);

            userService.SaveChangesUsers(listUserData);
            userService.SaveChangesRelatives(listUserData);

            return RedirectToAction("Index");
        }
    }
}