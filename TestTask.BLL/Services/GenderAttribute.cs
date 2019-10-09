using System.Text.RegularExpressions;

namespace TestTask.BLL.Services
{
    class GenderAttribute
    {
        public static bool IsRussian(string text)
        {
            return Regex.IsMatch(text, "[а-яА-ЯеЁ]+");
        }
        public static bool GenderDefinition(string dataGender)
        {
            bool genderUser = false;
            string genderSource = dataGender.ToLower();
            if (IsRussian(genderSource))
            {
                if (!genderSource.Contains("м"))
                {
                    genderUser = true;
                }
            }
            else
            {
                if (genderSource.Contains("f"))
                {
                    genderUser = true;
                }
            }
            return genderUser;
        }

    }
}

