﻿using AutoMapper;
using Dtol.dtol;
using ViewModel.SuggestBoxViewModel.MiddleModel;
using ViewModel.SuggestBoxViewModel.RequestViewModel;

namespace Dto.Service.AutoMapper.SuggestBoxMapper.SuggestBoxReqMapper
{
    public class SuggestReqMapper : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public SuggestReqMapper()
        {
            CreateMap<SuggestBoxAddViewModel, Suggest_Box>();
            CreateMap<SuggestBoxUpdateViewModel, Suggest_Box>();
            CreateMap<Suggest_Box, SuggestInfoMiddlecs > ()
            .ForMember(s => s.UserName, sp => sp.MapFrom(src => src.User_Info.UserName))
            .ForMember(s => s.Name, sp => sp.MapFrom(src => src.User_Info.User_Depart.Name));
        }
    }
}
