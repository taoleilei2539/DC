﻿
using Dtol;
using Dtol.dtol;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dto.IRepository.IntellUser;

namespace Dto.Repository.IntellUser
{
    public class UserInfoRepository : IUserInfoRepository
    {
        protected readonly DtolContext Db;
        protected readonly DbSet<User_Info> DbSet;

        public UserInfoRepository(DtolContext context)
        {
            Db = context;
            DbSet = Db.Set<User_Info>();
        }

        public virtual void Add(User_Info obj)
        {
            DbSet.Add(obj);
        }

        public virtual User_Info GetById(Guid id)
        {
            return DbSet.Find(id);
        }

        public virtual IQueryable<User_Info> GetAll()
        {
            return DbSet;
        }

        public virtual void Update(User_Info obj)
        {
            DbSet.Update(obj);
        }

        public virtual void Remove(Guid id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public int SaveChanges()
        {
            return Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
        User_Info IRepository<User_Info>.GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User_Info> GetInfoByUserid(string userid)
        {
            IQueryable<User_Info> user_Infos= DbSet.Where(uid => uid.Equals(userid));
            return user_Infos;
        }


        IQueryable<User_Info> IRepository<User_Info>.GetAll()
        {
            throw new NotImplementedException();
        }


        public void Update(User_Depart obj)
        {
            throw new NotImplementedException();
        }

        
    }
}
