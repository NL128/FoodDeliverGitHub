using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class RestoranDomainModel
    {
        public int RestoranID { get; set; }
        public string Ime { get; set; }
        public string Logo { get; set; }
        public string Koordinate { get; set; }
        public decimal MinimalnaNarudzbina { get; set; }
        public bool DostavaSeNaplacuje { get; set; }
        public decimal Ocena { get; set; }
        public string Kuhinja { get; set; }

    }
    public class Grad
    {
        public int GradId { get; set; }
        public string ImeGrada { get; set; }
        public int OpstinaId { get; set; }
        public string Opstina { get; set; }
        public int PostanskiBroj { get; set; }
    }
    public class RestoraniDomainModel
    {
        public int TotalCount { get; set; }
        public Grad city = new Grad();
        public List<RestoranDomainModel> restoranDomainModels = new List<RestoranDomainModel>();
        public List<KuhinjeModels> KuhinjeModels = new List<KuhinjeModels>();




    }

    public class MenuAndProducts
    {
        public int RestoranID { get; set; }
        public List<Menu> menus = new List<Menu>();
        public List<Proizvod> proizvods = new List<Proizvod>();

    }
    public class Menu
    {
        public int ID { get; set; }
        public string MenuName { get; set; }
        public int RestoranId { get; set; }
    }
    public class Proizvod
    {
        public int ID { get; set; }
        public string ProizvodIme { get; set; }
        public decimal Cena { get; set; }
        public string Opis { get; set; }
        public int MenuId { get; set; }
    }
}
