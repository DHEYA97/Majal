﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majal.Core.Abstractions.Const
{
    public static class DefaultRoles
    {
        public const string Admin = nameof(Admin);
        public const string AdminRoleId = "92b75286-d8f8-4061-9995-e6e23ccdee94";
        public const string AdminRoleConcurrencyStamp = "f51e5a91-bced-49c2-8b86-c2e170c0846c";


        public const string Member = nameof(Member);
        public const string MemberRoleId = "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4";
        public const string MemberRoleConcurrencyStamp = "5ee6bc12-5cb0-4304-91e7-6a00744e042a";

        public const string ContentWriter = nameof(ContentWriter);
        public const string ContentWriterRoleId = "0F99E50D-B7AC-414C-83C9-F7036CD735D0";
        public const string ContentWriterRoleConcurrencyStamp = "06419EB4-5A67-4E86-98CA-85B29EF34909";
    }
}
