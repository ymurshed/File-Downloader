namespace File.Api.Helper
{
    public static class Converter
    {
        public static T? GetStringDbValue<T>(object value) where T : class?
        {
            return value == DBNull.Value ? null : (T)value;
        }

        public static T? GetDbValue<T>(object value) where T : struct
        {
            return value == DBNull.Value ? null : (T)value;
        }
    }
}
