using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft_Graph_SDK_ASPNET_Connect.TokenStorage
{
    public class UserTokenCacheEntry
    {
        [Key]
        public int UserTokenCacheId { get; set; }
        public string UserUniqueId { get; set; }
        public byte[] CacheBits { get; set; }
        public DateTime LastWrite { get; set; }
    }
}