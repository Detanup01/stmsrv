using System.Text;

namespace Steam3Server.Others
{
    public static class AppInfoNodeKV
    {
        public static string ParseToVDF(this AppInfoNode node, int step = 0)
        {
            string Base = string.Empty;
            if (node.Value == null)
            {
                foreach (var item in node.Items)
                {
                    Base += "\n";
                    if (step == 0)
                    {
                        Base += $"\"{item.Key}\"\n{{";
                        Base += ParseToVDF(item.Value, step + 1);
                        Base += "\n}";
                    }
                    else
                    {
                        Base += new string('\t', step);
                        if (item.Value.Value != null)
                        {
                            Base += $"\"{item.Key}\"\t\t\"{item.Value.Value}\"";
                        }
                        else
                        {
                            Base += $"\"{item.Key}\"\n";
                            Base += new string('\t', step) + "{";
                            Base += ParseToVDF(item.Value, step + 1);
                            Base += "\n" + new string('\t', step) + "}";
                        }
                            
                    }
                }
            }
            return Base;
        }

        public static byte[] ParseToBin(this AppInfoNode node)
        {
            List<byte> bytes = new();
            if (node.Value == null)
            {
                foreach (var item in node.Items)
                {
                    if (item.Value.Value != null)
                    {
                        if (item.Value.IsInt)
                        {
                            bytes.Add(0x02);
                            bytes.AddRange(WriteToNullString(item.Key));
                            bytes.AddRange(BitConverter.GetBytes(uint.Parse(item.Value.Value)));
                        }
                        else
                        {
                            bytes.Add(0x01);
                            bytes.AddRange(WriteToNullString(item.Key));
                            bytes.AddRange(WriteToNullString(item.Value.Value));
                        }
                    }
                    else
                    {
                        bytes.Add(0x00);
                        bytes.AddRange(WriteToNullString(item.Key));
                        bytes.AddRange(ParseToBin(item.Value));
                    }
                }
            }
            else
            {
                throw new Exception(" check node value");
            }
            bytes.Add(0x08);
            return bytes.ToArray();
        }

        public static byte[] WriteToNullString(string Key)
        {
            return Encoding.UTF8.GetBytes(Key).Concat( new byte[] { 0 }).ToArray();
        }
    }
}
