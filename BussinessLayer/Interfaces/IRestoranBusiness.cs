
using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interfaces
{
  public  interface IRestoranBusiness   
    {
        MenuAndProducts GetMenuAndProducts(int restoranId);
        RestoranDomainModel GetRestoranById(int restoranId);
        RestoraniDomainModel GetRestorans(int take);
        RestoraniDomainModel GetRestoransByOffset(int offset,int take);

        RestoraniDomainModel GetRestoransByOffsetAndCity(int offset, int take, string city);
        RestoraniDomainModel GetRestoransByAll(int offset, int take, string opstina, int postcode, string restoran, string kuhinje);
        
        RestoraniDomainModel GetRestoransPartialy(int offset, int take, string opstina, int postcode, string restoran, string kuhinje);
    }
}
