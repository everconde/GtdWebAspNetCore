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
    public class ProductoRepo : IProducto
    {
        private readonly GtdWebContext _context; // for connecting to efcore.

        public ProductoRepo(GtdWebContext context) // will be passed by dependency injection.
        {
            _context = context;
        }

        public Producto Create(Producto Product)
        {
            _context.Productos.Add(Product);
            _context.SaveChanges();
            return Product;
        }

        public Producto Delete(Producto Producto)
        {
            Producto = pGetItem(Producto.ID);
            _context.Productos.Attach(Producto);
            _context.Entry(Producto).State = EntityState.Deleted;
            _context.SaveChanges();
            return Producto;
        }

        public Producto Edit(Producto Product)
        {
            _context.Productos.Attach(Product);
            _context.Entry(Product).State = EntityState.Modified;
            _context.SaveChanges();
            return Product;
        }

        private List<Producto> DoSort(List<Producto> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "nombreproducto")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.NombreProducto).ToList();
                else
                    items = items.OrderByDescending(n => n.NombreProducto).ToList();
            }
            else if (SortProperty.ToLower() == "id")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.ID).ToList();
                else
                    items = items.OrderByDescending(n => n.ID).ToList();
            }            

            return items;
        }

        public PaginatedList<Producto> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Producto> items = new List<Producto>();

            if (SearchText != "" && SearchText != null)
            {
                items = _context.Productos.Where(n => n.NombreProducto.Contains(SearchText))                    
                    .ToList();
            }
            else
                items = _context.Productos.ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<Producto> retItems = new PaginatedList<Producto>(items, pageIndex, pageSize);

            return retItems;
        }

        public Producto GetItem(string Code)
        {
            Producto item = _context.Productos.Where(u => u.ID == Code)                
                .FirstOrDefault();            
            return item;
        }

        public Producto pGetItem(string Code)
        {
            Producto item = _context.Productos.Where(u => u.ID == Code)
                .FirstOrDefault();
            return item;
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.Productos.Where(n => n.NombreProducto.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemExists(string name, string Code)
        {
            if (Code == "")
                return IsItemExists(name);
            var strCode = _context.Productos.Where(n => n.NombreProducto == name).Max(cd => cd.ID);
            if (strCode == null || strCode == Code)
                return false;
            else
                return true;
        }
        public bool IsItemCodeExists(string itemCode)
        {
            int ct = _context.Productos.Where(n => n.ID.ToLower() == itemCode.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemCodeExists(string itemCode, string name)
        {
            if (name == "")
                return IsItemCodeExists(itemCode);
            var strName = _context.Productos.Where(n => n.ID == itemCode).Max(nm => nm.NombreProducto);
            if (strName == null || strName == name)
                return false;
            else
                return IsItemExists(name);
        }
    }
}
