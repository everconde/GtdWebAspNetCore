using GtdWebAspNetCore.Interfaces;
using GtdWebAspNetCore.Models;
using GtdWebAspNetCore.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GtdWebAspNetCore.Controllers
{    
    public class FacturaController : Controller
    {
        private readonly IFactura _Repo;

        private readonly IProducto _ProductoRepo;
        
        public FacturaController(IFactura repo, IProducto productoRepo)
        {
            _Repo = repo;
            _ProductoRepo = productoRepo;
        }

        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Id");
            sortModel.AddColumn("NumeroFactura");
            sortModel.AddColumn("Fecha");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<Factura> items = _Repo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);

            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;

            TempData["CurrentPage"] = pg;
            return View(items);
        }

        public IActionResult Create()
        {
            Factura item = new Factura();
            item.Detalles.Add(new Detalle() { Id = 1 });
            ViewBag.ProductList = GetProducts();
            item.NumeroFactura = _Repo.GetNewNumeroFactura();
            return View(item);
        }

        [HttpPost]
        public IActionResult Create(Factura item)
        {
            item.Detalles.RemoveAll(a => a.Cantidad == 0);

            bool bolret = false;
            string errMessage = "";
            try
            {
                if (item.Detalles.Count > 0)
                {                    
                    bolret = _Repo.Create(item);
                }                    
                else
                {
                    TempData["ErrorMessage"] = "Debe ingresar al menos un detalle";
                    ModelState.AddModelError("", errMessage);
                    return View(item);
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }

            if (bolret == false)
            {
                errMessage = errMessage + " " + _Repo.GetErrors();

                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);
            }
            else
            {
                TempData["SuccessMessage"] = "" + item.NumeroFactura + " Creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
        }


        public IActionResult Details(int id)
        {
            Factura item = _Repo.GetItem(id);
            ViewBag.ProductList = GetProducts();            
            return View(item);
        }

        private List<SelectListItem> GetProducts()
        {
            var lstProducts = new List<SelectListItem>();

            PaginatedList<Producto> products = _ProductoRepo.GetItems("NombreProducto", SortOrder.Ascending, "", 1, 1000);

            lstProducts = products.Select(ut => new SelectListItem()
            {
                Value = ut.ID.ToString(),
                Text = ut.NombreProducto
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Product----"
            };

            lstProducts.Insert(0, defItem);

            return lstProducts;
        }

        public IActionResult Edit(int id)
        {
            Factura item = _Repo.GetItem(id);
            ViewBag.ProductList = GetProducts();            
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Factura item)
        {
            item.Detalles.RemoveAll(a => a.Cantidad == 0);

            bool bolret = false;
            string errMessage = "";
            try
            {
                bolret = _Repo.Edit(item);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }

            if (bolret == false)
            {
                errMessage = errMessage + " " + _Repo.GetErrors();

                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);
            }
            else
            {
                TempData["SuccessMessage"] = "" + item.NumeroFactura + " Modificado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Delete(int id)
        {
            Factura item = _Repo.GetItem(id);

            ViewBag.ProductList = GetProducts();            
            return View(item);
        }

        [HttpPost]
        public IActionResult Delete(Factura item)
        {
            item.Detalles.RemoveAll(a => a.Cantidad == 0);

            bool bolret = false;
            string errMessage = "";
            try
            {
                bolret = _Repo.Delete(item);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }

            if (bolret == false)
            {
                errMessage = errMessage + " " + _Repo.GetErrors();

                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);
            }
            else
            {
                TempData["SuccessMessage"] = "" + item.NumeroFactura + " Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
