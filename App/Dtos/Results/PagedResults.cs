using App.Dtos.DisplayDtos;
using App.Entities;

namespace App.Dtos.Results
{
    public class PagedResults<T>    
    {
        public IEnumerable<T> Items { get; set; }   

        public int ResultFrom { get; set; }
        public int ResultTo { get; set; }


        public int Pages { get; set; }
        public int AllResultsCount { get; set; }    

        public PagedResults(IEnumerable<T> items, int pageSize, int pageNumber, int resultsCount)   
        {
            Items = items;
            ResultFrom = ((pageNumber - 1) * pageSize) + 1;
            ResultTo = pageSize * pageNumber;        
            Pages = (int)Math.Ceiling((double)resultsCount / pageSize);
            AllResultsCount = resultsCount;

        }



    }



    
}
