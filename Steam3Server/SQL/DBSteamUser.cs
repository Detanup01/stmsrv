using LiteDB;

namespace Steam3Server.SQL
{
    internal class DBSteamUser
    {
        public readonly static string DBName = "Database/SteamUser.db";
        public readonly static string RegisteredUser = "RegisteredUser";
        public readonly static string LoggedUser = "LoggedUser";

        #region Registered User
        public static void AddRegisteredUser(JRegisteredUser data)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JRegisteredUser>(RegisteredUser);
                if (!col.Exists(x => x.SteamID == data.SteamID))
                {
                    var x = col.Count();
                    col.Insert(data);
                }
            }
        }

        public static void EditRegisteredUser(JRegisteredUser data)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JRegisteredUser>(RegisteredUser);

                var toReplace = col.FindOne(x => x.SteamID == data.SteamID);

                if (toReplace != null)
                {
                    col.Update(data);
                }
            }
        }

        public static JRegisteredUser? GetRegisteredUser(ulong SteamID)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JRegisteredUser>(RegisteredUser);

                var toGet = col.FindOne(x => x.SteamID == SteamID);
                return toGet;
            }
        }

        public static void DeleteRegisteredUser(ulong SteamID)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JRegisteredUser>(RegisteredUser);

                var toDel = col.FindOne(x => x.SteamID == SteamID);

                if (toDel != null)
                {
                    col.Delete(toDel.Id);
                }
            }
        }
        #endregion
        #region Logged User
        public static void AddLoggedUser(JLoggedUser data)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JLoggedUser>(LoggedUser);
                if (!col.Exists(x => x.SteamID == data.SteamID))
                {
                    var x = col.Count();
                    col.Insert(data);
                }
            }
        }

        public static void EditLoggedUser(JLoggedUser data)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JLoggedUser>(LoggedUser);

                var toReplace = col.FindOne(x => x.SteamID == data.SteamID);

                if (toReplace != null)
                {
                    col.Update(data);
                }
            }
        }

        public static JLoggedUser? GetLoggedUser(ulong SteamID)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JLoggedUser>(LoggedUser);

                var toGet = col.FindOne(x => x.SteamID == SteamID);
                return toGet;
            }
        }

        public static void DeleteLoggedUser(ulong SteamID)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JLoggedUser>(LoggedUser);

                var toDel = col.FindOne(x => x.SteamID == SteamID);

                if (toDel != null)
                {
                    col.Delete(toDel.Id);
                }
            }
        }
        #endregion
    }
}
