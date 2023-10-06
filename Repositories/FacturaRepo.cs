using GtdWebAspNetCore.Data;
using GtdWebAspNetCore.Interfaces;
using GtdWebAspNetCore.Models;
using GtdWebAspNetCore.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GtdWebAspNetCore.Repositories
{
    public class FacturaRepo : IFactura
    {
        private string _errors = "";

        public string GetErrors()
        {
            return _errors;
        }

        private readonly GtdWebContext _context; // for connecting to efcore.
        public FacturaRepo(GtdWebContext context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public bool Create(Factura factura)
        {
            bool retVal = false;
            decimal subTotalItem = 0;
            decimal subTotal = 0;            

            _errors = "";

            for(int i = 0; i < factura.Detalles.Count; i++)
            {
                subTotalItem = factura.Detalles[i].Cantidad * factura.Detalles[i].PrecioUnitario;
                subTotal = subTotalItem + subTotal;                
            }
            factura.Subtotal = subTotal;
            if (factura.Descuento > 0)            
                factura.TotalDescuento = subTotal * (factura.Descuento / 100); //resta
            if (factura.Iva > 0)
                factura.TotalImpuesto = (factura.Subtotal - factura.TotalDescuento) * (factura.Iva / 100); //suma
            factura.Total = factura.Subtotal - factura.TotalDescuento + factura.TotalImpuesto;

            try
            {
                _context.Facturas.Add(factura);
                _context.SaveChanges();
                retVal = true;
            }
            catch (Exception ex)
            {
                _errors = "Create Failed - Sql Exception Occured , Error Info : " + ex.Message;
            }
            return retVal;
        }

        public bool Delete(Factura factura)
        {
            bool retVal = false;
            _errors = "";

            try
            {
                _context.Attach(factura);
                _context.Entry(factura).State = EntityState.Deleted;
                _context.SaveChanges();
                retVal = true;
            }
            catch (Exception ex)
            {
                _errors = "Delete Failed - Sql Exception Occured , Error Info : " + ex.Message;
            }
            return retVal;
        }

        public bool Edit(Factura factura)
        {
            bool retVal = false;

            decimal subTotalItem = 0;
            decimal subTotal = 0;

            _errors = "";

            try
            {
                List<Detalle> detalles = _context.Detalles.Where(d => d.FacturaId == factura.Id).ToList();
                _context.Detalles.RemoveRange(detalles);
                _context.SaveChanges();

                for (int i = 0; i < factura.Detalles.Count; i++)
                {
                    subTotalItem = factura.Detalles[i].Cantidad * factura.Detalles[i].PrecioUnitario;
                    subTotal = subTotalItem + subTotal;
                }
                factura.Subtotal = subTotal;
                if (factura.Descuento > 0)
                    factura.TotalDescuento = subTotal * (factura.Descuento / 100); //resta
                if (factura.Iva > 0)
                    factura.TotalImpuesto = (factura.Subtotal - factura.TotalDescuento) * (factura.Iva / 100); //suma
                factura.Total = factura.Subtotal - factura.TotalDescuento + factura.TotalImpuesto;

                _context.Attach(factura);
                _context.Entry(factura).State = EntityState.Modified;
                _context.Detalles.AddRange(factura.Detalles);
                _context.SaveChanges();

                retVal = true;
            }
            catch (Exception ex)
            {
                _errors = "Update Failed - Sql Exception Occured , Error Info : " + ex.Message;
            }
            return retVal;
        }

        private List<Factura> DoSort(List<Factura> items, string SortProperty, SortOrder sortOrder)
        {
            if (SortProperty.ToLower() == "numerofactura")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.NumeroFactura).ToList();
                else
                    items = items.OrderByDescending(n => n.NumeroFactura).ToList();
            }            
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.Id).ToList();
                else
                    items = items.OrderBy(d => d.Id).ToList();
            }

            return items;
        }

        public PaginatedList<Factura> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Factura> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.Facturas.Where(n => n.NumeroFactura.Contains(SearchText))
                    .Include(s => s.Fecha)
                    .Include(c => c.NombreCliente)
                    .Include(f => f.Total)
                    .ToList();
            }
            else
                items = _context.Facturas
                   //.Include(s => ((Factura)s).Fecha)
                   //.Include(c => c.NombreCliente)
                   //.Include(f => f.Total)
                   .ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<Factura> retItems = new PaginatedList<Factura>(items, pageIndex, pageSize);

            return retItems;
        }

        public Factura GetItem(int Id)
        {            
            Factura item = _context.Facturas.Where(i => i.Id == Id)
                .Include(d => d.Detalles)
                .ThenInclude(i => i.Producto)
                .FirstOrDefault();
            
            item.Detalles.ForEach(p => p.Producto.NombreProducto = p.Producto.NombreProducto);
            //item.Detalles.ForEach(p => p.Total = p.Cantidad * p.PrecioUnitario);

            return item;
        }

        public bool IsNumeroFacturaExists(string numerofactura)
        {
            int ct = _context.Facturas.Where(n => n.NumeroFactura.ToLower() == numerofactura.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsNumeroFacturaExists(string numerofactura, int Id = 0)
        {
            if (Id == 0)
                return IsNumeroFacturaExists(numerofactura);

            var facturas = _context.Facturas.Where(n => n.NumeroFactura == numerofactura).Max(cd => cd.Id);
            if (facturas == 0 || facturas == Id)
                return false;
            else
                return true;
        }

        public string GetNewNumeroFactura()
        {
            string ponumber = "";
            var LastPoNumber = _context.Facturas.Max(cd => cd.NumeroFactura);

            if (LastPoNumber == null)
                ponumber = "FAC00001";
            else
            {
                int lastdigit = 1;
                int.TryParse(LastPoNumber.Substring(2, 5).ToString(), out lastdigit);

                ponumber = "FAC" + (lastdigit + 1).ToString().PadLeft(5, '0');
            }

            return ponumber;

        }

    }
}
