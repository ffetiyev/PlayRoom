﻿namespace Service.Helpers.Responses
{
    public class PaginateResponse<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public List<T> Datas { get; set; }
        public PaginateResponse(List<T> datas, int currentPage, int totalPage)
        {
            Datas = datas;
            TotalPage = totalPage;
            CurrentPage = currentPage;
            
        }
    }
    
}
