using LiteDB;
using System.Linq.Expressions;

namespace DB4Steam;

public class DBPackages
{
    public readonly static string DBName = $"filename={Path.Combine("Database", "PackageInfos.db")};connection=Shared";
    public readonly static string Packages = "Packages";
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

    public static JPackage? GetPackage(Expression<Func<JPackage, bool>> predicate)
    {
        using LiteDatabase db = new LiteDatabase(DBName);
        ILiteCollection<JPackage> col = db.GetCollection<JPackage>(Packages);

        return col.FindOne(predicate);
    }

    public static IEnumerable<JPackage> GetPackages(Expression<Func<JPackage, bool>> predicate)
    {
        using LiteDatabase db = new LiteDatabase(DBName);
        ILiteCollection<JPackage> col = db.GetCollection<JPackage>(Packages);

        return col.Find(predicate);
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
}
