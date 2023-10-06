using GtdWebAspNetCore.Models;
using GtdWebAspNetCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GtdWebAspNetCore.Interfaces
{
    public interface IProducto
    {        
        PaginatedList<Producto> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        Producto GetItem(string Code); // read particular item

        Producto Create(Producto producto);

        Producto Edit(Producto producto);

        Producto Delete(Producto producto);

        public bool IsItemExists(string name);
        public bool IsItemExists(string name, string Code);

        public bool IsItemCodeExists(string itemCode);
        public bool IsItemCodeExists(string itemCode, string name);

    }
}
