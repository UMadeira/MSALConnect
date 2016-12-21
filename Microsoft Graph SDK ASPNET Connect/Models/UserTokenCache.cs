using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Models
{
    public class UserTokenCache
    {
        [Key]
        public int UserTokenCacheId { get; set; }
        public string UserUniqueId { get; set; }
        public byte[] CacheBits { get; set; }
        public DateTime LastWrite { get; set; }
    }
}