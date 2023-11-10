namespace Steam3Server.SQL
{
    public class DBPrepare
    {
        public static void Prepare()
        {
            if (!Directory.Exists("Database")) { Directory.CreateDirectory("Database"); }
        }
    }
}
