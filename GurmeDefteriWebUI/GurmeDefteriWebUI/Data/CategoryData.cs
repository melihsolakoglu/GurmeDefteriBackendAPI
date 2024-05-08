namespace GurmeDefteriWebUI.Data
{
    public static class CategoryData
    {
        static CategoryData()
        {
            categories.Sort();
        }
        public static List<string> categories = new List<string>
        {"Ana Yemek","Çorba","İçicek","Salata","Tatlı"
        };

    }
}