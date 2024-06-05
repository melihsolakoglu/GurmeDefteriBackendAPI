namespace GurmeDefteriWebUI.Utils
{
    public class StringPropertyTrim
    {
        public void TrimAllStringProperties(object obj)
        {
            if (obj == null)
                return;

            var stringProperties = obj.GetType().GetProperties()
                                      .Where(p => p.PropertyType == typeof(string));

            foreach (var prop in stringProperties)
            {
                var value = (string)prop.GetValue(obj);
                if (value != null)
                {
                    prop.SetValue(obj, value.Trim());
                }
            }
        }
    }
}
