using GtdWebAspNetCore.Models;
using GtdWebAspNetCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GtdWebAspNetCore.Interfaces
{
    public interface IFactura
    {
        public string GetErrors();

        PaginatedList<Factura> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
        Factura GetItem(int id); // read particular item

        bool Create(Factura factura);
        bool Edit(Factura factura);
        bool Delete(Factura factura);

        public bool IsNumeroFacturaExists(string numeroFactura);
        public bool IsNumeroFacturaExists(string numeroFactura, int Id);

        public string GetNewNumeroFactura();

    }
}
