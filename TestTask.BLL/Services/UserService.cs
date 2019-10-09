using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestTask.BLL.DTO;
using TestTask.DAL.EF;
using TestTask.DAL.Entities;

namespace TestTask.BLL.Services
{
    public class UserService
    {
        UserContext Database { get; set; }

        /*public UserService(string connectionString)
        {
            //UserContext db = new UserContext(connectionString);
            UserContext db = new UserContext();
            Database = db;
        }*/

        public UserService()
        {
            //UserContext db = new UserContext(connectionString);
            UserContext db = new UserContext();
            Database = db;
        }
        public IEnumerable<UserInfoDTO> ViewUserInfoDTO() 
        {
            var unionRelativesUsers = from r in Database.Relatives
                                      join u in Database.Users on r.ParentId equals u.Id into ru
                                      from u in ru.DefaultIfEmpty()
                                      select new
                                      {
                                          r.Id,
                                          u.Name
                                      };

            var unionUserRU = from u in Database.Users
                              join ru in unionRelativesUsers on u.Id equals ru.Id
                              select new UserInfoDTO
                              {
                                  Name = u.Name,
                                  DateBirth = u.DateBirth,
                                  Gender = u.Gender,
                                  ParentName = ru.Name
                              };

            IEnumerable<UserInfoDTO> usersInfo = unionUserRU;

            return usersInfo;
        }
        public List<string[]> UploadData(HttpPostedFileBase csvFile)
        {
            byte[] byteData = new byte[csvFile.ContentLength];
            csvFile.InputStream.Read(byteData, 0, csvFile.ContentLength);

            string[] dataUsers = (System.Text.Encoding.Default.GetString(byteData, 0, byteData.Length)).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            List<string[]> listUserData = new List<string[]>();
            for (int i = 0; i < dataUsers.Length; i++)
            {
                listUserData.Add(dataUsers[i].Split(';'));
            }

            return listUserData;
        }

        public void SaveChangesUsers(List<string[]> listUserData)
        {
            for (int i = 1; i < listUserData.Count; i++)
            {
                bool genderUser = GenderAttribute.GenderDefinition(listUserData[i][3]);

                int? parentUser = ParentAttribute.ParentDefinition(listUserData[i][4]);
                User user= new User
                {
                    Name = listUserData[i][1].Trim(),
                    DateBirth = Convert.ToDateTime(listUserData[i][2]),
                    Gender = genderUser,
                    ParentId = parentUser
                };
                Database.Users.Add(user);
            }
            Database.SaveChanges();
        }

        public void SaveChangesRelatives(List<string[]> listUserData) 
        {
            for (int i = 1; i < listUserData.Count; i++)
            {
                int? parentUser = ParentAttribute.ParentDefinition(listUserData[i][4]);

                Relative relative = new Relative
                {
                    ParentId = parentUser
                };
                Database.Relatives.Add(relative);
            }
            Database.SaveChanges();
        }


}
}
