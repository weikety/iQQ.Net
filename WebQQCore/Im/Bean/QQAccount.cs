﻿using System;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Bean
{
    [Serializable]
    public class QQAccount : QQUser
    {
        public string Password { get; set; }
        public string Username { get; set; }

        public byte[] Md5PwdArray => Password.Md5ToArray();
        public string Md5Pwd => Password.Md5ToString();
    }
}
