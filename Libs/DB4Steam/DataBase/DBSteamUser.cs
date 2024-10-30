using LiteDB;
using System.Linq.Expressions;

namespace DB4Steam;

public class DBSteamUser
{
    public readonly static string DBName = $"filename={Path.Combine("Database", "SteamUser.db")};connection=Shared";
    public const string RegisteredUser = "RegisteredUser";
    public const string LoggedUser = "LoggedUser";
    public const string SteamProfile = "SteamProfile";

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

    public static IEnumerable<JRegisteredUser> GetRegisteredUsers()
    {
        using LiteDatabase db = new LiteDatabase(DBName);
        ILiteCollection<JRegisteredUser> col = db.GetCollection<JRegisteredUser>(RegisteredUser);

        return col.FindAll();
    }

    public static IEnumerable<JRegisteredUser> GetRegisteredUsers(Expression<Func<JRegisteredUser, bool>> predicate)
    {
        using LiteDatabase db = new LiteDatabase(DBName);
        ILiteCollection<JRegisteredUser> col = db.GetCollection<JRegisteredUser>(RegisteredUser);

        return col.Find(predicate);
    }

    public static void DeleteRegisteredUser(ulong SteamID)
    {
        using (var db = new LiteDatabase(DBName))
        {
            var col = db.GetCollection<JRegisteredUser>(RegisteredUser);

            var toDel = col.FindOne(x => x.SteamID == SteamID);

            if (toDel != null)
            {
                col.Delete(toDel.SteamID);
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
                col.Delete(toDel.SteamID);
            }
        }
    }
    #endregion
    #region Steam Profile
    public static void AddSteamProfile(JSteamProfile data)
    {
        using (var db = new LiteDatabase(DBName))
        {
            var col = db.GetCollection<JSteamProfile>(SteamProfile);
            if (!col.Exists(x => x.SteamID == data.SteamID))
            {
                var x = col.Count();
                col.Insert(data);
            }
        }
    }

    public static void EditSteamProfile(JSteamProfile data)
    {
        using (var db = new LiteDatabase(DBName))
        {
            var col = db.GetCollection<JSteamProfile>(SteamProfile);

            var toReplace = col.FindOne(x => x.SteamID == data.SteamID);

            if (toReplace != null)
            {
                col.Update(data);
            }
        }
    }

    public static JSteamProfile? GetSteamProfile(ulong SteamID)
    {
        using (var db = new LiteDatabase(DBName))
        {
            var col = db.GetCollection<JSteamProfile>(SteamProfile);

            var toGet = col.FindOne(x => x.SteamID == SteamID);
            return toGet;
        }
    }

    public static void DeleteSteamProfile(ulong SteamID)
    {
        using (var db = new LiteDatabase(DBName))
        {
            var col = db.GetCollection<JSteamProfile>(SteamProfile);

            var toDel = col.FindOne(x => x.SteamID == SteamID);

            if (toDel != null)
            {
                col.Delete(toDel.SteamID);
            }
        }
    }
    #endregion
}
