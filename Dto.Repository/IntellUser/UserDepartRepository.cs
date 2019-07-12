﻿
using Dtol;
using Dtol.dtol;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dto.IRepository.IntellUser;
using ViewModel.UserViewModel.RequsetModel;
using System.Linq.Expressions;
using Dtol.EfCoreExtion;

namespace Dto.Repository.IntellUser
{
    public class UserDepartRepository : IUserDepartRepository
    {
        protected readonly DtolContext Db;
        protected readonly DbSet<User_Depart> DbSet;

        public UserDepartRepository(DtolContext context)
        {
            Db = context;
            DbSet = Db.Set<User_Depart>();

        }

        public virtual void Add(User_Depart obj)
        {
            DbSet.Add(obj);
        }

        public int DeleteByDepartidList(List<int> IdList)
        {
            int DeleteRowNum = 1;
            for (int i = 0; i < IdList.Count; i++)
            {
                var model = DbSet.Single(w => w.Id == IdList[i]);

                DbSet.Remove(model);
                SaveChanges();
                DeleteRowNum = i + 1;
            }
            return DeleteRowNum;
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }

        public IQueryable<User_Depart> GetAll()
        {
            return DbSet;
        }
        public int SaveChanges()
        {
            return Db.SaveChanges();
        }
        public User_Depart GetById(Guid id)
        {
            return DbSet.Find(id);
        }

        public IQueryable<User_Depart> GetDepartByCode(string code)
        {
            IQueryable<User_Depart> user_Departs = DbSet.Where(uid => uid.Code.Equals(code));
            return user_Departs;
        }

        public void Remove(Guid id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public List<User_Depart> SearchDepartByWhere(DepartSearchViewModel departSearchViewModel)
        {
            //查询条件
            var predicate = SearchDepartWhere(departSearchViewModel);
            return DbSet.Where(predicate).OrderBy(o => o.Code).ToList();
        }

        public void Update(User_Depart obj)
        {
            DbSet.Update(obj);
        }
        #region 查询条件
        //根据调价查询用户
        private Expression<Func<User_Depart, bool>> SearchDepartWhere(DepartSearchViewModel departSearchViewModel)
        {
            var predicate = WhereExtension.True<User_Depart>();//初始化where表达式
            predicate = predicate.And(p => p.Code.Contains(departSearchViewModel.Code));
            predicate = predicate.And(p => p.Name.Contains(departSearchViewModel.Name));
            predicate = predicate.And(p => p.ParentId.Contains(departSearchViewModel.ParentId));
            return predicate;
        }


        #endregion
    }
}
