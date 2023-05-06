using QLBH.Fastfood.Data;
using QLBH.Fastfood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH.Fastfood.Service
{
    public interface IPhanQuyenService
    {
        PhanQuyen GetUserTypeByID(int ID);
        IEnumerable<PhanQuyen> GetListUserType();
        PhanQuyen Add(PhanQuyen userRole);
        IEnumerable<PhanQuyen> GetUserTypeList();
        IEnumerable<PhanQuyen> GetUserTypeList(string keyWord);
        IEnumerable<PhanQuyen> GetUserTypeListName(string keyword);
        PhanQuyen GetByID(int ID);
        void Update(PhanQuyen userRole);
        void Delete(PhanQuyen userRole);
        void Save();
    }
    public class PhanQuyenService : IPhanQuyenService
    {
        private readonly UnitOfWork context;
        public PhanQuyenService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public PhanQuyen GetUserTypeByID(int ID)
        {
            return context.PhanQuyenRepository.GetDataByID(ID);
        }

        public IEnumerable<PhanQuyen> GetListUserType()
        {
            return context.PhanQuyenRepository.GetAllData();
        }

        public PhanQuyen Add(PhanQuyen userRole)
        {
            this.context.PhanQuyenRepository.Insert(userRole);
            return userRole;
        }

        public IEnumerable<PhanQuyen> GetUserTypeList()
        {
            IEnumerable<PhanQuyen> listUserType = this.context.PhanQuyenRepository.GetAllData();
            return listUserType;
        }

        public IEnumerable<PhanQuyen> GetUserTypeList(string keyWord)
        {
            IEnumerable<PhanQuyen> listUserType = this.context.PhanQuyenRepository.GetAllData(x => x.TenQuyen.Contains(keyWord));
            return listUserType;
        }

        public IEnumerable<PhanQuyen> GetUserTypeListName(string keyword)
        {
            IEnumerable<PhanQuyen> listUserTypeName = this.context.PhanQuyenRepository.GetAllData(x => x.TenQuyen.Contains(keyword));
            return listUserTypeName;
        }

        public PhanQuyen GetByID(int ID)
        {
            return this.context.PhanQuyenRepository.GetDataByID(ID);
        }

        public void Update(PhanQuyen userRole)
        {
            this.context.PhanQuyenRepository.Update(userRole);
        }

        public void Delete(PhanQuyen userRole)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }

}