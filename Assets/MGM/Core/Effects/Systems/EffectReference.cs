using System;
using System.Security.Cryptography;
using System.Text;

namespace Wayn.Mgm.Effects
{
    public struct EffectReference
    {
        public ulong TypeId;
        public int VersionId;

        public static ulong GetTypeId(Type t)
        {
            return BitConverter.ToUInt64(new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(t.FullName)), 0);
        }
    }


}
