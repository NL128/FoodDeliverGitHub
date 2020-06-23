using BussinessLayer.Interfaces;
using DomainLayer;
using FoodDelivery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Configuration;
using System.Net;

namespace FoodDelivery.Controllers
{
    public class PonudaController : Controller
    {
        IRestoranBusiness _restoranBusiness;
       public PonudaController(IRestoranBusiness restoranBusiness)
        {
            _restoranBusiness = restoranBusiness;

        }
        // GET: Ponuda
        public ActionResult Index()
        {
           
            
            RestoraniDomainModel restoran1 = _restoranBusiness.GetRestorans(10);
            
            return View(restoran1);
        }
        public ActionResult Restoran(string id)
        {
            MenuAndProducts menuAndProducts  = new MenuAndProducts();
            int _id = 0;
            if(int.TryParse(id,out _id))
            {
                menuAndProducts.RestoranID = _id;
                menuAndProducts = _restoranBusiness.GetMenuAndProducts(_id);
            }

            
            return View(menuAndProducts);
        }
        public ActionResult GetRestoranDetails(string id)
        {
            RestoranDomainModel restoranDomainModel = new RestoranDomainModel();
            int _id = 0;
            if(int.TryParse(id,out _id))
            {
               
                restoranDomainModel = _restoranBusiness.GetRestoranById(_id);
            }
           
            return PartialView("~/Views/Shared/_loadCard_Restoran_area.cshtml", restoranDomainModel);
        }
        public ActionResult Search(string opstina, int postCode, string restoran, string kuhinje,int skip)
        {
            string [] kuhinjeObj = JsonConvert.DeserializeObject<string []>(kuhinje);
            string kuhinjeNiz = "";
            if (kuhinje.Length > 0)
            {
                int currentConveted = 0;
               
                StringBuilder s = new StringBuilder();
                for(int i = 0; i < kuhinjeObj.Length; i++)
                {
                    if(kuhinjeObj[i]!= null)
                    {
                        if (int.TryParse(kuhinjeObj[i], out currentConveted)){

                            if (s.Length<=0)
                            {
                                s.Append(kuhinjeObj[i]);
                            }
                            else if (i > 0 && i < kuhinjeObj.Length - 1)
                            {
                                s.Append(',');
                                s.Append(kuhinjeObj[i]);
                            }
                            else
                            {
                                s.Append(',');
                                s.Append(kuhinjeObj[i]);
                            }

                        }
                    }
                    
                }
                kuhinjeNiz = s.ToString();
              
            }
            RestoraniDomainModel restoraniDomainModel = _restoranBusiness.GetRestoransByAll(skip, 10, opstina, postCode, restoran, kuhinjeNiz);
            if (restoraniDomainModel.restoranDomainModels.Count > 0)
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NoContent;
            }
            return PartialView("~/Views/Shared/_RestoraniList.cshtml", restoraniDomainModel);
        }
        
            public ActionResult SearchPartial(string opstina, int postCode, string restoran, string kuhinje,int skip)
            {
                string[] kuhinjeObj = JsonConvert.DeserializeObject<string[]>(kuhinje);
                string kuhinjeNiz = "";
                if (kuhinje.Length > 0)
                {
                    int currentConveted = 0;

                    StringBuilder s = new StringBuilder();
                    for (int i = 0; i < kuhinjeObj.Length; i++)
                    {
                        if (kuhinjeObj[i] != null)
                        {
                            if (int.TryParse(kuhinjeObj[i], out currentConveted))
                            {

                                if (s.Length <= 0)
                                {
                                    s.Append(kuhinjeObj[i]);
                                }
                                else if (i > 0 && i < kuhinjeObj.Length - 1)
                                {
                                    s.Append(',');
                                    s.Append(kuhinjeObj[i]);
                                }
                                else
                                {
                                    s.Append(',');
                                    s.Append(kuhinjeObj[i]);
                                }

                            }
                        }

                    }
                    kuhinjeNiz = s.ToString();

                }
                RestoraniDomainModel restoraniDomainModel = _restoranBusiness.GetRestoransPartialy(skip, 10, opstina, postCode, restoran, kuhinjeNiz);
            if (restoraniDomainModel.restoranDomainModels.Count > 0)
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NoContent;
            }  
            return PartialView("~/Views/Shared/_RestoraniList.cshtml", restoraniDomainModel);
            }
    }
}