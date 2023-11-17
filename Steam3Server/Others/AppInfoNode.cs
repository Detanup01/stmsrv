using System.Globalization;
using System.Text;

namespace Steam3Server.Others
{
    /*
     code from: https://github.com/Depressurizer/Depressurizer/blob/master/src/Depressurizer.Core/Models/AppInfoNode.cs
     */



    /// <summary>
    ///     AppInfo Node
    /// </summary>
    public sealed class AppInfoNode
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Creates an empty AppInfo node.
        /// </summary>
        public AppInfoNode() { }

        /// <summary>
        ///     Creates an AppInfo node with the specified (node) value.
        /// </summary>
        /// <param name="value">
        ///     Data of the AppInfo node.
        /// </param>
        public AppInfoNode(string value)
        {
            Value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Dictionary containing the sub-nodes.
        /// </summary>
        public Dictionary<string, AppInfoNode> Items { get; set; } = new Dictionary<string, AppInfoNode>();

        /// <summary>
        ///     Data of the AppInfo node.
        /// </summary>
        public string Value { get; set; }

        #endregion

        #region Public Indexers

        /// <summary>
        ///     Gets or sets the sub-node with the given key.
        /// </summary>
        /// <param name="key">
        ///     Key of the sub-node to get or set.
        /// </param>
        /// <returns>
        ///     Returns the sub-node at the specified key.
        /// </returns>
        public AppInfoNode this[string key]
        {
            get => Items[key];
            set => Items[key] = value;
        }

        #endregion
    }

    public static class AppInfoNodeExt
    {
        public static AppInfoNode ReadEntries(this BinaryReader _binaryReader)
        {
            AppInfoNode result = new AppInfoNode();

            while (true)
            {
                byte type = _binaryReader.ReadByte();
                if (type == 0x08)
                {
                    break;
                }

                string key = _binaryReader.InfoReadString();

                switch (type)
                {
                    case 0x00:
                        result[key] = _binaryReader.ReadEntries();

                        break;
                    case 0x01:
                        result[key] = new AppInfoNode(_binaryReader.InfoReadString());

                        break;
                    case 0x02:
                        result[key] = new AppInfoNode(_binaryReader.ReadUInt32().ToString(CultureInfo.InvariantCulture));

                        break;
                    default:

                        throw new ArgumentOutOfRangeException(string.Format(CultureInfo.InvariantCulture, "Unknown entry type '{0}'", type));
                }
            }

            return result;
        }
        private static string InfoReadString(this BinaryReader _binaryReader)
        {
            List<byte> bytes = new List<byte>();

            try
            {
                bool stringDone = false;
                do
                {
                    byte b = _binaryReader.ReadByte();
                    if (b == 0)
                    {
                        stringDone = true;
                    }
                    else
                    {
                        bytes.Add(b);
                    }
                } while (!stringDone);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }

            return Encoding.UTF8.GetString(bytes.ToArray());
        }
    }
}