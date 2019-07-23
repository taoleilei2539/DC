﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Dtol.dtol
{
    public class Bus_Payment
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 缴费状态-是否缴费
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 站点id
        /// </summary>
        public int? Bus_StationId { get; set; }
        public Bus_Station Bus_Station { get; set; }
        /// <summary>
        /// 线路id
        /// </summary>
        public int? Bus_LineId { get; set; }
        public Bus_Line Bus_Line { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int? User_InfoId { get; set; }
        public User_Info User_Info { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public int? User_DepartId { get; set; }
        public User_Depart User_Depart { get; set; }

        /// <summary>
        /// 班车Id   ---外键
        /// </summary>
        public int? Bus_InfoId { get; set; }

        public Bus_Info Bus_Info { get; set; }
        public DateTime? createDate{ get; set; }
        public DateTime? updateDate { get; set; }
    }
}
