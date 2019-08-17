﻿using System;
using System.Collections.Generic;
using System.Text;
using ViewModel.PublicViewModel;

namespace ViewModel.RepairsViewModel.RequestViewModel
{
    /// <summary>
    /// 根据节点查角色
    /// </summary>
    public class RoleByNodeSearchViewModel
    {
        /// <summary>
        /// 节点id
        /// </summary>
        public int Flow_NextNodeDefineId { get; set; }
        /// <summary>
        /// 分页
        /// </summary>
        public PageViewModel pageViewModel { get; set; }
        /// <summary>
        /// 构造方法
        /// </summary>
        public RoleByNodeSearchViewModel()
        {
            pageViewModel = new PageViewModel();
        }
    }
}
