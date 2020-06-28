using BussinessLayer.Interfaces;

using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    //public class KuhinjeBusiness : IKuhinjeBusiness
    //{
    //    skomi69_dostavaEntities1 dbcontex = new skomi69_dostavaEntities1();
    //    public List<KuhinjeModels> GetKuhinjes()
    //    {
    //        List<KuhinjeModels> kuhinjeModels = new List<KuhinjeModels>(dbcontex.KuhinjeTbls.Count());
    //        var list = dbcontex.KuhinjeTbls.ToList();
    //        for (int i = 0,j=list.Count-1; i < list.Count; i++,j--) {
    //            if (j < i) { break; }
    //            kuhinjeModels.Add(new KuhinjeModels() { Ime=list[i].Ime,KuhinjeID=list[i].KuhinjeID});
    //            kuhinjeModels.Add(new KuhinjeModels() { Ime = list[j].Ime, KuhinjeID = list[j].KuhinjeID });
    //        }



    //        return kuhinjeModels;
    //    }
    //}
}
