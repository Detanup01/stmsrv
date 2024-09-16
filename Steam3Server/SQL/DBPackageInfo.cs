using LiteDB;

namespace Steam3Server.SQL
{
    public class DBPackageInfo
    {
        public readonly static string DBName = "Database/PackageInfos.db";
        public readonly static string Packages = "Packages";
        public readonly static string PackageInfoCache = "PackageInfoCache";
        #region Apps
        public static void AddPackage(JPackage data)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPackage>(Packages);
                if (!col.Exists(x => x.SubID == data.SubID))
                {
                    var x = col.Count();
                    col.Insert(data);
                }
            }
        }

        public static void EditPackage(JPackage data)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPackage>(Packages);

                var toReplace = col.FindOne(x => x.SubID == data.SubID);

                if (toReplace != null)
                {
                    col.Update(data);
                }
            }
        }

        public static JPackage? GetPackage(uint SubId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPackage>(Packages);

                var toGet = col.FindOne(x => x.SubID == SubId);
                return toGet;
            }
        }

        public static void DeletePackage(uint SubId)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPackage>(Packages);

                var toDel = col.FindOne(x => x.SubID == SubId);

                if (toDel != null)
                {
                    col.Delete(toDel.Id);
                }
            }
        }
        #endregion
        #region PackageInfoCache
        public static void AddPackageInfoCache(JPackageInfo data)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPackageInfo>(PackageInfoCache);
                if (!col.Exists(x => x.Id == 1))
                {
                    var x = col.Count();
                    col.Insert(data);
                }
            }
        }

        public static void EditPackageInfoCache(JPackageInfo data)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPackageInfo>(PackageInfoCache);

                var toReplace = col.FindOne(x => x.Id == 1);

                if (toReplace != null)
                {
                    col.Update(data);
                }
            }
        }

        public static JPackageInfo? GetPackageInfoCache()
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPackageInfo>(PackageInfoCache);

                var toGet = col.FindOne(x => x.Id == 1);
                return toGet;
            }
        }

        public static void DeletePackageInfoCache(int Id)
        {
            using (var db = new LiteDatabase(DBName))
            {
                var col = db.GetCollection<JPackageInfo>(PackageInfoCache);
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
