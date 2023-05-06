using QLBH.Fastfood.Data;
using QLBH.Fastfood.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QLBH.Fastfood.Service
{
    public interface IDonHangService
    {
        DonHang AddOrder(DonHang order);
        IEnumerable<DonHang> GetAll();
        IEnumerable<DonHang> GetOrderNotApproval();
        IEnumerable<DonHang> GetOrderNotDelivery();
        IEnumerable<DonHang> GetOrderDeliveredAndPaid();
        IEnumerable<DonHang> ApprovedAndNotDelivery();
        IEnumerable<DonHang> GetDelivered();
        DonHang GetByID(int ID);
        IEnumerable<DonHang> GetByCustomerID(int ID);
        DonHang Approved(int ID);
        DonHang Delivered(int ID);
        DonHang Received(int ID);
        decimal GetTotalRevenue();
        int GetTotalOrder();
        void Update(DonHang order);
        void UpdateTotal(int OrderID, decimal Total);
        IEnumerable<DonHang> GetListOrderStatistic(DateTime from, DateTime to);
        bool Paid(int ID);
    }
    public class DonHangService : IDonHangService
    {
        private readonly UnitOfWork context;
        public DonHangService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public DonHang AddOrder(DonHang order)
        {
            this.context.DonHangRepository.Insert(order);
            return order;
        }

        public DonHang Approved(int ID)
        {
            DonHang order = context.DonHangRepository.GetDataByID(ID);
            order.ChapNhan = true;
            context.DonHangRepository.Update(order);
            return order;
        }

        public IEnumerable<DonHang> ApprovedAndNotDelivery()
        {
            return context.DonHangRepository.GetAllData(x => x.ChapNhan == true && x.GiaoHang == false);
        }

        public DonHang Delivered(int ID)
        {
            DonHang order = context.DonHangRepository.GetDataByID(ID);
            order.GiaoHang = true;
            context.DonHangRepository.Update(order);
            return order;
        }

        public DonHang GetByID(int ID)
        {
            return context.DonHangRepository.GetDataByID(ID);
        }

        public IEnumerable<DonHang> GetDelivered()
        {
            return context.DonHangRepository.GetAllData(x => x.GiaoHang == true && x.HuyDon == false && x.DaNhan == false);
        }

        public IEnumerable<DonHang> GetOrderDeliveredAndPaid()
        {
            return context.DonHangRepository.GetAllData(x => x.TraTien == true && x.GiaoHang == true && x.HuyDon == false);
        }

        public IEnumerable<DonHang> GetOrderNotDelivery()
        {
            return context.DonHangRepository.GetAllData(x => x.GiaoHang == false && x.HuyDon == false);
        }

        public IEnumerable<DonHang> GetOrderNotApproval()
        {
            return context.DonHangRepository.GetAllData(x => x.ChapNhan == false && x.HuyDon == false);
        }

        public void Update(DonHang order)
        {
            context.DonHangRepository.Update(order);
        }

        public IEnumerable<DonHang> GetByCustomerID(int ID)
        {
            return context.DonHangRepository.GetAllData(x => x.IDUser == ID);
        }

        public DonHang Received(int ID)
        {
            DonHang order = context.DonHangRepository.GetDataByID(ID);
            order.DaNhan = true;
            order.TraTien = true;
            order.NgayGiao = DateTime.Now;
            context.DonHangRepository.Update(order);
            //Update PurchasedCount
            IEnumerable<ChiTietDonHang> orderDetails = context.ChiTietDonHangRepository.GetAllData(x => x.ID == ID);
            foreach (var item in orderDetails)
            {
                SanPham product = context.SanPhamRepository.GetDataByID(item.MaSP);
                product.SoLanMua += item.SoLuong;
                product.SoLuong -= item.SoLuong;
                context.SanPhamRepository.Update(product);
            }
            return order;
        }

        public bool Paid(int ID)
        {
            DonHang order = context.DonHangRepository.GetDataByID(ID);
            if (order != null)
            {
                order.TraTien = true;
                context.DonHangRepository.Update(order);
            }
            return true;
        }

        public decimal GetTotalRevenue()
        {
            return context.ChiTietDonHangRepository.GetAllData(x => x.DonHang.TraTien == true).Sum(x => x.GiaTien);
        }

        public int GetTotalOrder()
        {
            return context.DonHangRepository.GetAllData().Count();
        }

        public void UpdateTotal(int OrderID, decimal Total)
        {
            DonHang order = context.DonHangRepository.GetDataByID(OrderID);
            order.TongCong = Total;
            context.DonHangRepository.Update(order);
        }

        public IEnumerable<DonHang> GetListOrderStatistic(DateTime from, DateTime to)
        {
            IEnumerable<ChiTietDonHang> orderDetails = context.ChiTietDonHangRepository.GetAllData(x => DbFunctions.TruncateTime(x.DonHang.NgayDat) >= from.Date && DbFunctions.TruncateTime(x.DonHang.NgayDat) <= to.Date);

            List<int> OrderIDs = new List<int>();
            foreach (var item in orderDetails)
            {
                OrderIDs.Add(item.MaDH);
            }
            if (OrderIDs.Count() > 0)
            {
                return context.DonHangRepository.GetAllData(x => x.DaNhan == true && OrderIDs.Contains(x.MaDH)).OrderByDescending(x => x.TongCong);
            }
            return null;
        }

        public IEnumerable<DonHang> GetAll()
        {
            return context.DonHangRepository.GetAllData();
        }
    }

}