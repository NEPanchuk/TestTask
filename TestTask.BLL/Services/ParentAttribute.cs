namespace TestTask.BLL.Services
{
    class ParentAttribute
    {
        public static int? ParentDefinition(string dataParent)
        {
            int? parentUser = null;
            if (dataParent != "")
            {
                parentUser = int.Parse(dataParent);
            }
            return parentUser;
        }
    }
}
