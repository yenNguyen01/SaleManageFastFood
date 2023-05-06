using QLBH.Fastfood.Data;
using QLBH.Fastfood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH.Fastfood.Service
{
    public interface IGioHangService
    {
        void AddCartIntoUser(GioHang cart);
        bool CheckCartUser(int UserID);
        void UpdateQuantityCartUser(int Quantity, int ProductID, int UserID);
        void AddQuantityProductCartUser(int ProductID, int UserID);
        void RemoveCart(int ProductID, int UserID);
        GioHang AddCart(GioHang cart);
        List<GioHang> GetCart(int UserID);
        GioHang GetCartByID(int ID);
        bool CheckProductInCart(int ProductID, int UserID);
        void RemoveCartDeleteAccount(int UserID);
    }
    public class GioHangService : IGioHangService
    {
        private readonly UnitOfWork context;
        public GioHangService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public GioHang AddCart(GioHang cart)
        {
            context.GioHangRepository.Insert(cart);
            return cart;
        }

        public void AddCartIntoUser(GioHang cart)
        {
            context.GioHangRepository.Insert(cart);
        }

        public void AddQuantityProductCartUser(int ProductID, int UserID)
        {
            GioHang cartUpdate = context.GioHangRepository.GetAllData().Single(x => x.MaSP == ProductID && x.IDUser == UserID);
            cartUpdate.SoLuong += 1;
            cartUpdate.TongCong = cartUpdate.SoLuong * cartUpdate.SanPhams.GiaSP;
            context.GioHangRepository.Update(cartUpdate);
        }

        public bool CheckCartUser(int UserID)
        {
            if (context.GioHangRepository.GetAllData().Where(x => x.IDUser == UserID).Count() > 0)
                return true;
            return false;
        }

        public bool CheckProductInCart(int ProductID, int UserID)
        {
            if (context.GioHangRepository.GetAllData().Where(x => x.IDUser == UserID && x.MaSP == ProductID).Count() > 0)
                return true;
            return false;
        }

        public List<GioHang> GetCart(int UserID)
        {
            return context.GioHangRepository.GetAllData().Where(x => x.IDUser == UserID).ToList();
        }

        public GioHang GetCartByID(int ID)
        {
            return context.GioHangRepository.GetDataByID(ID);
        }

        public void RemoveCart(int ProductID, int MemberID)
        {
            GioHang cart = context.GioHangRepository.GetAllData().Single(x => x.MaSP == ProductID && x.IDUser == MemberID);
            context.GioHangRepository.Remove(cart);
        }

        public void RemoveCartDeleteAccount(int MemberID)
        {
            List<GioHang> cart = GetCart(MemberID);
            if (cart != null)
            {
                foreach (GioHang item in cart)
                {
                    context.GioHangRepository.Remove(item);
                }
            }
        }

        public void UpdateQuantityCartUser(int Quantity, int ProductID, int UserID)
        {
            GioHang cartUpdate = context.GioHangRepository.GetAllData().Single(x => x.MaSP == ProductID && x.IDUser == UserID);
            cartUpdate.SoLuong = Quantity;
            cartUpdate.TongCong = cartUpdate.SoLuong * cartUpdate.SanPhams.GiaSP;
            context.GioHangRepository.Update(cartUpdate);
        }
    }
}