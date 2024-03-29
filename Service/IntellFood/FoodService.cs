﻿using AutoMapper;
using Dto.IRepository.IntellOpinionInfo;
using Dto.IService.IntellFood;
using Dtol.dtol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModel.FoodViewModel.MiddleModel;
using ViewModel.FoodViewModel.RequestViewModel;

namespace Dto.Service.IntellFood
{
    public class FoodService : IFoodService
    {
        private readonly IFoodInfoRepository _IFoodInfoRepository;
        private readonly IRelate_Food_UserRepository  _IRelate_Food_UserRepository;
        private readonly IMapper _IMapper;

        public FoodService(IFoodInfoRepository  foodInfoRepository, 
                            IRelate_Food_UserRepository relate_Food_UserRepository,
                            IMapper mapper)
        {
            _IFoodInfoRepository = foodInfoRepository;
            _IRelate_Food_UserRepository = relate_Food_UserRepository;
            _IMapper = mapper;
        }
        /// <summary>
        /// 菜单增加
        /// </summary>
        /// <param name="foodInfoAddViewModel"></param>
        /// <returns></returns>
        public int Food_Add(FoodInfoAddViewModel foodInfoAddViewModel)
        {

            var food_Info = _IMapper.Map<FoodInfoAddViewModel, Food_Info>(foodInfoAddViewModel);
            _IFoodInfoRepository.Add(food_Info);
            return _IFoodInfoRepository.SaveChanges();
        }

      
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="foodInfoDelViewModel"></param>
        /// <returns></returns>
        public int Food_Delete(FoodInfoDelViewModel foodInfoDelViewModel)
        {

            int DeleteRowsNum = _IFoodInfoRepository
                 .DeleteByFoodInfoIdList(foodInfoDelViewModel.DeleleIdList);
            if (DeleteRowsNum == foodInfoDelViewModel.DeleleIdList.Count)
            {
                return DeleteRowsNum;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取菜单数量
        /// </summary>
        /// <param name="foodInfoSearchViewModel"></param>
        /// <returns></returns>
        public int Food_Get_ALLNum(FoodInfoSearchViewModel foodInfoSearchViewModel)
        {
            return _IFoodInfoRepository.GetFoodAll(foodInfoSearchViewModel).Count();
        }
        /// <summary>
        /// 菜单查询
        /// </summary>
        /// <param name="foodInfoSearchViewModel"></param>
        /// <returns></returns>
        public List<Food_Info> Food_Search(FoodInfoSearchViewModel foodInfoSearchViewModel)
        {

            List<Food_Info> user_Departs = _IFoodInfoRepository.SearchFoodInfoByWhere(foodInfoSearchViewModel);
            //List<string> a=_IFoodInfoRepository.SearchFoodTypeInfoByWhere(foodInfoSearchViewModel);

            return user_Departs;
        }
        /// 只查询菜品种类
        /// </summary>
        /// <param name="foodInfoSearchViewModel"></param>
        /// <returns></returns>
        public List<string> FoodType_Search(FoodInfoSearchViewModel foodInfoSearchViewModel)
        {
        
            List<string> foodType = _IFoodInfoRepository.SearchFoodTypeInfoByWhere(foodInfoSearchViewModel);
            return foodType;
        }
        /// <summary>
        /// 更新菜单信息
        /// </summary>
        /// <param name="foodInfoUpdateViewModel"></param>
        /// <returns></returns>
        public int Food_Update(FoodInfoUpdateViewModel foodInfoUpdateViewModel)
        {
            var food_Info = _IFoodInfoRepository.GetInfoByFoodId(foodInfoUpdateViewModel.Id);
            var food_Info_update = _IMapper.Map<FoodInfoUpdateViewModel, Food_Info>(foodInfoUpdateViewModel, food_Info);
            _IFoodInfoRepository.Update(food_Info_update);
            return _IFoodInfoRepository.SaveChanges();
        }

        public bool Food_Single(FoodInfoValideRepeat foodInfoValideRepeat)
        {

            IQueryable<Food_Info> Queryable_UserDepart = _IFoodInfoRepository
                                                                 .GetInfoByFoodId(foodInfoValideRepeat.Code);
            return (Queryable_UserDepart.Count() < 1) ?
                   true : false;
        }
        /// <summary>
        /// 根据用户和菜单点赞
        /// </summary>
        /// <param name="foodByUserSearchViewMode"></param>
        /// <returns></returns>
        public int Food_Relate_User(FoodByUserPraiseViewModel foodByUserSearchViewMode)
        {
            int count = _IRelate_Food_UserRepository.SearchFoodInfoByWhere(foodByUserSearchViewMode);
         
            if(count>0)
            {

                int DelNum = _IRelate_Food_UserRepository
                         .RelateFoodToUserDel(foodByUserSearchViewMode);
            
                return 0;
            }
            else
            {
                var node_Info = _IMapper.Map<FoodByUserPraiseViewModel, User_Relate_Food>(foodByUserSearchViewMode);
                _IRelate_Food_UserRepository.Add(node_Info);
                 _IRelate_Food_UserRepository.SaveChanges();
                return 1;
            }
        }
        /// <summary>
        /// 根据用户和菜单点差评
        /// </summary>
        /// <param name="foodByUserCpViewModel"></param>
        /// <returns></returns>
        public int Food_Relate_UserCp(FoodByUserAddCpViewModel foodByUserAddCpViewModel)
        {
            int count = _IRelate_Food_UserRepository.SearchFoodCpInfoByWhere(foodByUserAddCpViewModel);

            if (count > 0)
            {

                return -1;
            }
            else
            {
                var node_Info = _IMapper.Map<FoodByUserAddCpViewModel, User_Relate_Food>(foodByUserAddCpViewModel);
                _IRelate_Food_UserRepository.Add(node_Info);
                _IRelate_Food_UserRepository.SaveChanges();
                return 1;
            }
        }

        /// <summary>
        /// 根据用户和菜单查询差评信息
        /// </summary>
        /// <param name="foodByUserSearchCpViewModel"></param>
        /// <returns></returns>
        public List<FoodCpMiddlecs>  Food_Relate_User_Search_CP(FoodByUserSearchCpViewModel  foodByUserSearchCpViewModel)
        {

            List <User_Relate_Food>  user_Relate_Foods = _IRelate_Food_UserRepository.SearchFoodInfoByWhere(foodByUserSearchCpViewModel);
            var Cp_Info = _IMapper.Map<List<User_Relate_Food>, List<FoodCpMiddlecs>>(user_Relate_Foods);
            return Cp_Info;
        }
        /// <summary>
        /// 根据用户和菜单增加评价信息
        /// </summary>
        /// <param name="foodByUserSearchViewMode"></param>
        /// <returns></returns>
        public int Food_Relate_User_ADD_Pj(FoodByUserAddCpViewModel  foodByUserAddCpViewModel)
        {
            int count = _IRelate_Food_UserRepository.SearchFoodInfoByWhere(foodByUserAddCpViewModel);

            if (count > 0)
            {

                return -1;
            }
            else
            {
                var node_Info = _IMapper.Map<FoodByUserAddCpViewModel, User_Relate_Food>(foodByUserAddCpViewModel);
                _IRelate_Food_UserRepository.Add(node_Info);
                _IRelate_Food_UserRepository.SaveChanges();
                return 1;
            }
        }


        public int Food_Relate_User_Del(FoodByUserPraiseViewModel foodByUserSearchViewModelt)
        {
            int DelNum = _IRelate_Food_UserRepository
                     .RelateFoodToUserDel(foodByUserSearchViewModelt);

            return DelNum;
        }
        /// <summary>
        /// 点赞数量
        /// </summary>
        /// <param name="praiseNumSearchMiddlecs"></param>
        /// <returns></returns>
        public List<FoodPraiseNumMiddlecs> PraiseNumByFoodId(PraiseNumSearchMiddlecs praiseNumSearchMiddlecs)
        {
            List < FoodPraiseNumMiddlecs > fp= _IRelate_Food_UserRepository.RelateFoodToFoodIdSearch(praiseNumSearchMiddlecs);
            return fp;
        }

        /// <summary>
        /// 差评数量
        /// </summary>
        /// <param name="praiseNumSearchMiddlecs"></param>
        /// <returns></returns>
        public List<FoodPraiseNumMiddlecs> CpNumByFoodId(PraiseNumSearchMiddlecs praiseNumSearchMiddlecs)
        {
            List<FoodPraiseNumMiddlecs> fp = _IRelate_Food_UserRepository.RelateFoodToFoodIdCpSearch(praiseNumSearchMiddlecs);
            return fp;
        }
        /// <summary>
        ///根据菜Id删除点赞数量
        /// </summary>
        /// <param name="foodByUserPraiseDelViewModel"></param>
        /// <returns></returns>
        public int By_Food_Id_Del(FoodByUserPraiseDelViewModel foodByUserPraiseDelViewModel)
        {
            int DeleteRowsNum = _IRelate_Food_UserRepository
                  .ByFoodIdDel(foodByUserPraiseDelViewModel);
         
            return DeleteRowsNum;
         
        }
        /// <summary>
        ///根据菜Id删除差评数量
        /// </summary>
        /// <param name="foodByUserPraiseDelViewModel"></param>
        /// <returns></returns>
        public int By_Food_Id_DelCp(FoodByUserPraiseDelViewModel foodByUserPraiseDelViewModel)
        {
            int DeleteRowsNum = _IRelate_Food_UserRepository
                  .ByFoodIdDelCp(foodByUserPraiseDelViewModel);

            return DeleteRowsNum;

        }
        /// <summary>
        /// 食物差评数量
        /// </summary>
        /// <param name="foodByUserSearchCpViewModel"></param>
        /// <returns></returns>
        public int FoodCp_Get_ALLNum(FoodByUserSearchCpViewModel foodByUserSearchCpViewModel)
        {
            return _IRelate_Food_UserRepository.SearchFoodInfoByWhere(foodByUserSearchCpViewModel).Count();
        }
    }
}
