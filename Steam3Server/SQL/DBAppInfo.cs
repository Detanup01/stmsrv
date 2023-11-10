using LiteDB;

namespace Steam3Server.SQL
{
    public class DBAppInfo
    {
        public readonly static string DBName = "Database/AppInfos.db";
        public readonly static string Apps = "Apps";
        public readonly static string AppInfoCache = "AppInfoCache";
        #region Apps
        public static void AddApp(JApp data)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JApp>(Apps);
                if (!col.Exists(x => x.AppID == data.AppID))
                {
                    var x = col.Count();
                    col.Insert(data);
                }
            }
        }

        public static void EditApp(JApp data)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JApp>(Apps);

                var toReplace = col.FindOne(x => x.AppID == data.AppID);

                if (toReplace != null)
                {
                    col.Update(data);
                }
            }
        }

        public static JApp? GetApp(uint AppId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JApp>(Apps);

                var toGet = col.FindOne(x => x.AppID == AppId);
                return toGet;
            }
        }

        public static void DeleteApp(uint AppId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JApp>(Apps);

                var toDel = col.FindOne(x => x.AppID == AppId);

                if (toDel != null)
                {
                    col.Delete(toDel.Id);
                }
            }
        }
        #endregion
        #region AppInfoCache
        public static void AddAppInfoCache(JAppInfo data)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppInfo>(AppInfoCache);
                if (!col.Exists(x => x.Id == 1))
                {
                    var x = col.Count();
                    col.Insert(data);
                }
            }
        }

        public static void EditAppInfoCache(JAppInfo data)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppInfo>(AppInfoCache);

                var toReplace = col.FindOne(x => x.Id == 1);

                if (toReplace != null)
                {
                    col.Update(data);
                }
            }
        }

        public static JAppInfo? GetAppInfoCache()
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppInfo>(AppInfoCache);

                var toGet = col.FindOne(x => x.Id == 1);
                return toGet;
            }
        }

        public static void DeleteAppInfoCache()
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JAppInfo>(AppInfoCache);
                col.DeleteAll();
                var toDel = col.FindOne(x => x.Id == 1);

                if (toDel != null)
                {
                    col.Delete(toDel.Id);
                }
            }
        }
        #endregion
    }
}
