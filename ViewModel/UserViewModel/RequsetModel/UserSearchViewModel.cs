﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.UserViewModel.RequsetModel
{
    public partial class UserSearchViewModel
    {
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户登录账号
        /// </summary>
        public string UserId { get; set; }
        //public string PhoneCall { get; set; }
       //public string Email { get; set; }
       /// <summary>
       /// 账号状态，0启用1停用
       /// </summary>
        public string status { get; set; }
       /// <summary>
       /// 账号身份0普通身份1临时身份
       /// </summary>
        public string Levels { get; set; }
    
        //public DateTime? AddDate { get; set; }
    }
}
