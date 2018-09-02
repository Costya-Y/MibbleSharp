using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MibbleBrowser
{
    internal class MibNodeFormHandler
    {
        internal string Oid = "";
        internal string OidType = "";
        internal string Description = "";
        internal string IsAccessible = "";

        internal MibNodeFormHandler(MibNode node)
        {

            if (node == null)
            {
                return;
            }
            if (node.Value != null)
            {
                Oid = node.Value.ToString();
            }

            if (node.SnmpObjType != null)
            {
                Description = Regex.Replace(node.SnmpObjType.Description, "\\s+", " ", RegexOptions.IgnoreCase);
                if (node.SnmpObjType.Syntax != null)
                {
                    OidType = Regex.Replace(node.SnmpObjType.Syntax.ToString(), "^.*universal\\s*\\d+\\S*", "", RegexOptions.IgnoreCase);
                }
                if (node.SnmpObjType.Access != null)
                {
                    IsAccessible = node.SnmpObjType.Access.ToString();
                }
            }
            else
            {
                Description = string.Join(
                "\r\n",
                node.Description
                .Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()));
            }
        }
    }
}
