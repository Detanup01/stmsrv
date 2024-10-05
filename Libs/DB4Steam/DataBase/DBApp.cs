using LiteDB;
using System.Linq.Expressions;

namespace DB4Steam;

public class DBApp
{
    public readonly static string DBName = $"filename={Path.Combine("Database", "AppInfos.db")};connection=Shared";
    public readonly static string Apps = "Apps";
    #region Apps
    public static void AddApp(JApp data)
    {
        using LiteDatabase db = new LiteDatabase(DBName);
        ILiteCollection<JApp> col = db.GetCollection<JApp>(Apps);
        if (!col.Exists(x => x.AppID == data.AppID))
        {
            col.Insert(data);
        }
    }

    public static bool EditApp(JApp data)
    {
        using LiteDatabase db = new LiteDatabase(DBName);
        ILiteCollection<JApp> col = db.GetCollection<JApp>(Apps);

        JApp? toReplace = col.FindOne(x => x.AppID == data.AppID);

        if (toReplace != null)
        {
            return col.Update(data);
        }
        return false;
    }

    public static JApp? GetApp(uint AppId)
    {
        using LiteDatabase db = new LiteDatabase(DBName);
        ILiteCollection<JApp> col = db.GetCollection<JApp>(Apps);

        return col.FindOne(x => x.AppID == AppId);
    }

    public static JApp? GetApp(Expression<Func<JApp, bool>> predicate)
    {
        using LiteDatabase db = new LiteDatabase(DBName);
        ILiteCollection<JApp> col = db.GetCollection<JApp>(Apps);

        return col.FindOne(predicate);
    }

    public static IEnumerable<JApp> GetApps(Expression<Func<JApp, bool>> predicate)
    {
        using LiteDatabase db = new LiteDatabase(DBName);
        ILiteCollection<JApp> col = db.GetCollection<JApp>(Apps);

        return col.Find(predicate);
    }

    public static void DeleteApp(uint AppId)
    {
        using LiteDatabase db = new LiteDatabase(DBName);
        ILiteCollection<JApp> col = db.GetCollection<JApp>(Apps);

        JApp? toDel = col.FindOne(x => x.AppID == AppId);

        if (toDel != null)
        {
            col.Delete(toDel.Id);
        }
    }
    #endregion
}
