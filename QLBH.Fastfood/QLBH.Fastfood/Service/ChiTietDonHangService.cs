using QLBH.Fastfood.Data;
using QLBH.Fastfood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH.Fastfood.Service
{
    public interface IChiTietDonHangService
    {
        ChiTietDonHang AddOrderDetail(ChiTietDonHang order);
        IEnumerable<ChiTietDonHang> GetByOrderID(int ID);
        void SetIsRating(int ID);
        void Save();
    }
    public class ChiTietDonHangService : IChiTietDonHangService
    {
        private readonly UnitOfWork context;
        public ChiTietDonHangService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public ChiTietDonHang AddOrderDetail(ChiTietDonHang orderDetail)
        {
            this.context.ChiTietDonHangRepository.Insert(orderDetail);
            return orderDetail;
        }

        public IEnumerable<ChiTietDonHang> GetByOrderID(int ID)
        {
            return this.context.ChiTietDonHangRepository.GetAllData().Where(x => x.ID == ID);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void SetIsRating(int ID)
        {
            ChiTietDonHang orderDetail = context.ChiTietDonHangRepository.GetDataByID(ID);
            orderDetail.DanhGia = true;
            context.ChiTietDonHangRepository.Update(orderDetail);
        }
    }
}