﻿using AutoMapper;
using Dtol.dtol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModel.BusViewModel.MiddleModel;
using ViewModel.BusViewModel.RequestViewModel.BusUserViewModel;

namespace Dto.Service.AutoMapper.BusMapper.BusRequestMapper
{
    public class BusUserReqMapper : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public BusUserReqMapper()
        {
            CreateMap<BusUserAddViewModel, Bus_Payment>();
            CreateMap < BusUserUpdateViewModel, Bus_Payment>();

            CreateMap<Bus_Payment, BusUserSearchMiddlecs>();
            CreateMap<BusPaymentUpdateViewModel, Bus_Payment>();
            CreateMap<NowDateUpdateViewModel, Bus_Payment>();
            CreateMap<NowDateUpdateViewModel, BusUserAddViewModel>();
            CreateMap<Bus_Payment, BusUserAddViewModel>();
        }
    }
}
