using QLBH.Fastfood.Data;
using QLBH.Fastfood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH.Fastfood.Service
{
    public interface ITaiKhoanService
    {
        TaiKhoan Add(TaiKhoan user);
        TaiKhoan CheckLogin(string email, string password);
        IEnumerable<TaiKhoan> GetList(int ID);
        IEnumerable<TaiKhoan> GetList();
        int GetTotalUser();
        int GetTotalEmployee();
        List<string> GetUserListName(string keyword);
        TaiKhoan GetByID(int ID);
        void Update(TaiKhoan user);
        void Block(TaiKhoan user);
        void Active(TaiKhoan user);
        void ResetPassword(int EmloyeeID, string NewPassword);
        bool CheckPhoneNumber(string PhoneNumber);
        bool CheckName(string Name);
        bool CheckEmail(string Email);
        TaiKhoan GetByPhoneNumber(string PhoneNumber);
        TaiKhoan GetByName(string Name);
        TaiKhoan GetByEmail(string Email);
        string GetEmailByID(int ID);
        void UpdateAmountPurchased(int ID, decimal AmountPurchased);
        IEnumerable<TaiKhoan> GetUserListForStatistic();
        bool CheckEmailPhone(string email, string phone);
    }
    public class TaiKhoanService : ITaiKhoanService
    {
        private readonly UnitOfWork context;
        public TaiKhoanService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

         public IEnumerable<TaiKhoan> GetUserListForStatistic()
         {
            IEnumerable<TaiKhoan> listUser = this.context.TaiKhoanRepository.GetAllData(x => x.SoLanDaMua > 0 && x.XoaTK == false && x.PhanQuyen.TenQuyen == "Client").OrderByDescending(x => x.SoLanDaMua);
            return listUser;
         }

        public void UpdateAmountPurchased(int ID, decimal AmountPurchased)
        {
            TaiKhoan user = context.TaiKhoanRepository.GetDataByID(ID);
            user.SoLanDaMua += AmountPurchased;
            context.TaiKhoanRepository.Update(user);
        }

        public void Active(TaiKhoan user)
        {
            user.XoaTK = false;
            this.context.TaiKhoanRepository.Update(user);
        }

        public TaiKhoan Add(TaiKhoan user)
        {
            user.IDRole = 1;
            user.SoLanDaMua = 0;
            user.XoaTK = false;
            this.context.TaiKhoanRepository.Insert(user);
            return user;
        }

        public void Block(TaiKhoan user)
        {
            user.XoaTK = true;
            this.context.TaiKhoanRepository.Update(user);
        }

        public TaiKhoan CheckLogin(string email, string password)
        {
            TaiKhoan user = this.context.TaiKhoanRepository.GetAllData().SingleOrDefault(x => x.Email == email && x.MatKhau == password && x.XoaTK == false);
            return user;
        }

        public bool CheckEmailPhone(string email, string phone)
        {
            TaiKhoan user = this.context.TaiKhoanRepository.GetAllData().SingleOrDefault(x => x.Email == email && x.SDT == phone && x.XoaTK == false);
            if (user != null)
            {
                return false;
            }
            return true;
        }

        public TaiKhoan GetByID(int ID)
        {
            return this.context.TaiKhoanRepository.GetDataByID(ID);
        }

        public List<string> GetUserListName(string keyword)
        {
            IEnumerable<TaiKhoan> listUserName = this.context.TaiKhoanRepository.GetAllData(x => x.HoTen.Contains(keyword) && x.XoaTK == false);
            List<string> names = new List<string>();
            foreach (var item in listUserName)
            {
                names.Add(item.HoTen);
            }
            return names;
        }

        public IEnumerable<TaiKhoan> GetList(int ID)
        {
            return context.TaiKhoanRepository.GetAllData(x => x.IDUser != ID);
        }

        public int GetTotalUser()
        {
            return context.TaiKhoanRepository.GetAllData(x => x.PhanQuyen.TenQuyen == "Client").Count();
        }

        public int GetTotalEmployee()
        {
            return context.TaiKhoanRepository.GetAllData(x => x.PhanQuyen.TenQuyen != "Client").Count();
        }

        public void ResetPassword(int UserID, string NewPassword)
        {
            TaiKhoan user = GetByID(UserID);
            user.MatKhau = NewPassword;
            context.TaiKhoanRepository.Update(user);
        }

        public void Update(TaiKhoan user)
        {
            this.context.TaiKhoanRepository.Update(user);
        }
        public bool CheckPhoneNumber(string PhoneNumber)
        {
            var check = context.TaiKhoanRepository.GetAllData(x => x.SDT == PhoneNumber && x.XoaTK == false);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public bool CheckName(string Name)
        {
            var check = context.TaiKhoanRepository.GetAllData(x => x.HoTen == Name && x.XoaTK == false);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public bool CheckEmail(string Email)
        {
            var check = context.TaiKhoanRepository.GetAllData(x => x.Email == Email && x.XoaTK == false);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }
        public TaiKhoan GetByPhoneNumber(string PhoneNumber)
        {
            TaiKhoan user = context.TaiKhoanRepository.GetAllData().FirstOrDefault(x => x.SDT == PhoneNumber);
            return user;
        }

        public TaiKhoan GetByName(string Name)
        {
            TaiKhoan user = context.TaiKhoanRepository.GetAllData().FirstOrDefault(x => x.HoTen == Name);
            return user;
        }

        public TaiKhoan GetByEmail(string Email)
        {
            TaiKhoan user = context.TaiKhoanRepository.GetAllData().FirstOrDefault(x => x.Email == Email);
            return user;
        }

        public IEnumerable<TaiKhoan> GetList()
        {
            return context.TaiKhoanRepository.GetAllData();
        }

        public string GetEmailByID(int ID)
        {
            string email = context.TaiKhoanRepository.GetAllData().FirstOrDefault(x => x.IDUser == ID).Email;
            return email;
        }
    }
 
}