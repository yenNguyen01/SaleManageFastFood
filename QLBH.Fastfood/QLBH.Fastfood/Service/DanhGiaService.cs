using QLBH.Fastfood.Data;
using QLBH.Fastfood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH.Fastfood.Service
{
    public interface IDanhGiaService
    {
        void AddRating(DanhGia rating);
        int GetRating(int ProductID);
        IEnumerable<DanhGia> GetListRating(int ProductID);
        IEnumerable<DanhGia> GetListAllRating();
    }
    public class DanhGiaService : IDanhGiaService
    {
        private readonly UnitOfWork context;
        public DanhGiaService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public void AddRating(DanhGia rating)
        {
            context.DanhGiaRepository.Insert(rating);
        }

        public IEnumerable<DanhGia> GetListAllRating()
        {
            return context.DanhGiaRepository.GetAllData();
        }

        public IEnumerable<DanhGia> GetListRating(int ProductID)
        {
            return context.DanhGiaRepository.GetAllData(x => x.MaSP == ProductID);
        }

        public int GetRating(int ProductID)
        {
            IEnumerable<DanhGia> ratings = context.DanhGiaRepository.GetAllData(x => x.MaSP == ProductID);
            List<int> list = ratings.Select(x => x.Sao).ToList();
            int sum = 0;
            foreach (int item in list)
            {
                sum += item;
            }
            if (sum > 0)
                return sum / list.Count;
            else
                return 0;
        }
    }
 
}