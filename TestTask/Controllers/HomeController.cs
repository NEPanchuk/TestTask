using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TestTask.Models;

namespace TestTask.Controllers
{
    public class HomeController : Controller
    {
        private bool IsRussian(string text)
        {
            return Regex.IsMatch(text, "[а-яА-ЯеЁ]+");
        }

        UserContext db = new UserContext();

        public ActionResult Index()
        {
            var union_ru = from p in db.Relatives
                           join c in db.Users on p.ParentId equals c.Id into ps
                           from c in ps.DefaultIfEmpty()
                           select new { Id = p.Id, Name = c.Name };

            var union_user_parentName = db.Users.Join(union_ru, p => p.Id,
        c => c.Id,
        (p, c) => new User_parent
        {
            Name = p.Name,
            DateBirth = p.DateBirth,
            Gender = p.Gender,
            NameParent = c.Name
        });
            IEnumerable<User_parent> users = union_user_parentName;
            ViewBag.data = users;

            return View();
        }

        public ActionResult Upload(HttpPostedFileBase file_csv)
        {

            if (file_csv != null)
            {

                string fileName = System.IO.Path.GetFileName(file_csv.FileName);
                //file_csv.SaveAs(Server.MapPath("~/App_Data/" + fileName));
                byte[] byteData = new byte[file_csv.ContentLength];
                file_csv.InputStream.Read(byteData, 0, file_csv.ContentLength);
                string[] dataUser = (System.Text.Encoding.Default.GetString(
                 byteData,
                 0,
                 byteData.Length)).Split(new string[] { "\r\n" },
                                             StringSplitOptions.RemoveEmptyEntries);
                List<string[]> lUsers = new List<string[]>();
                for (int i = 0; i < dataUser.Length; i++)
                {
                    lUsers.Add(dataUser[i].Split(';'));
                }

                for (int i = 1; i < lUsers.Count; i++)
                {
                    bool GenderUser = false;
                    string gender = lUsers[i][3].ToLower();
                    if (IsRussian(gender))
                    {
                        if (!gender.Contains("м"))
                        {
                            GenderUser = true;
                        }
                    }
                    else
                    {
                        if (gender.Contains("f"))
                        {
                            GenderUser = true;
                        }
                    }
                    int? Parent = null;
                    if (lUsers[i][4] != "")
                    {
                        Parent = int.Parse(lUsers[i][4]);
                    }

                    User user = new User
                    {
                        Name = lUsers[i][1].Trim(),
                        DateBirth = Convert.ToDateTime(lUsers[i][2]),
                        Gender = GenderUser,
                        ParentId = Parent
                    };

                    db.Users.Add(user);

                }

                db.SaveChanges();
                for (int i = 1; i < lUsers.Count; i++)
                {
                    int? Parent = null;
                    if (lUsers[i][4] != "")
                    {
                        Parent = int.Parse(lUsers[i][4]);
                    }
                    Relative relative = new Relative
                    {
                        ParentId = Parent
                    };
                    db.Relatives.Add(relative);

                }
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

    }
}