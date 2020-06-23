using BussinessLayer.Interfaces;
using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace BussinessLayer
{
    public class RestoranBusiness : IRestoranBusiness
    {
        
        public MenuAndProducts GetMenuAndProducts(int restoranId)
        {
            MenuAndProducts menuAndProducts = new MenuAndProducts();
            string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                using(SqlCommand com = new SqlCommand("spGetMenuDetails", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@id", restoranId);
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    if (con.State == ConnectionState.Open)
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                        {
                            DataSet ds = new DataSet();
                            adapter.Fill(ds);
                            foreach(DataRow r in ds.Tables[1].Rows)
                            {
                                Menu menu = new Menu();
                                menu.ID = Convert.ToInt32( r["ID"]);
                                menu.MenuName = Convert.ToString(r["Menu"]);
                                menu.RestoranId = Convert.ToInt32(r["RestoranId"]);
                                menuAndProducts.menus.Add(menu);
                            }
                            foreach(DataRow productRow in ds.Tables[0].Rows)
                            {
                                Proizvod proizvod = new Proizvod();
                                proizvod.ID = Convert.ToInt32(productRow["ID"]);
                                proizvod.ProizvodIme = Convert.ToString(productRow["Proizvod"]);
                                proizvod.Cena = Convert.ToDecimal(productRow["Cena"]);
                                proizvod.Opis = Convert.ToString(productRow["Opis"]);
                                proizvod.MenuId = Convert.ToInt32(productRow["MenuId"]);
                                menuAndProducts.proizvods.Add(proizvod);
                            }
                        }
                    }
                }
            }

            return menuAndProducts;
        }
        public RestoranDomainModel GetRestoranById(int restoranId)
        {
            RestoranDomainModel restoranDomainModel = new RestoranDomainModel();
            string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using(SqlConnection con = new SqlConnection(conString))
            {
                if(con.State== ConnectionState.Closed) { con.Open(); }
                if(con.State== ConnectionState.Open)
                {
                    using (SqlCommand com = new SqlCommand("SELECT RestoranID,Ime,Koordinate,Logo,MinimalnaNarudzbina,Ocena FROM RestoranTbl WHERE RestoranID=@id", con))
                    {
                        com.Parameters.AddWithValue("@id", restoranId);
                        SqlDataReader r = com.ExecuteReader();

                        if (r.HasRows)
                        {
                            restoranDomainModel.RestoranID = Convert.ToInt32(r["RestoranID"]);
                            restoranDomainModel.Ime = Convert.ToString(r["Ime"]);
                            restoranDomainModel.Koordinate = Convert.ToString(r["Koordinate"]);
                            restoranDomainModel.Logo = Convert.ToString(r["Logo"]);
                            restoranDomainModel.MinimalnaNarudzbina = Convert.ToDecimal(r["MinimalnaNarudzbina"]);
                            restoranDomainModel.Ocena = Convert.ToDecimal(r["Ocena"]);
                        }
                        
                    }
                }
                
            }
           
            return restoranDomainModel;
        }

        public RestoraniDomainModel GetRestorans(int initTake)
        {
            RestoraniDomainModel restoranDomainModels = new RestoraniDomainModel();

            
            string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlCommand com = new SqlCommand("spGetFirstElements", con))
                    {
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@count", initTake);
                        if (con.State != System.Data.ConnectionState.Open) { con.Open(); }
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter(com))
                            {
                                da.Fill(ds);
                            }

                        }
                    }
                }
                
                for(int i=0,j=ds.Tables[0].Rows.Count-1;i< ds.Tables[0].Rows.Count;i++,j--)
                {
                    if (j < i) { break; }
                    restoranDomainModels.restoranDomainModels.Add(RestoranDomainModel(ds, i));
                    restoranDomainModels.restoranDomainModels.Add(RestoranDomainModel(ds,j));
                }
                for (int i = 0, j = ds.Tables[1].Rows.Count - 1; i < ds.Tables[1].Rows.Count; i++, j--)
                {
                    if (j < i) { break; }
                    restoranDomainModels.KuhinjeModels.Add(KuhinjeModelsMethod(ds, i));
                    restoranDomainModels.KuhinjeModels.Add(KuhinjeModelsMethod(ds, j));
                }
                DataRow total = ds.Tables[2].Rows[0];
                restoranDomainModels.TotalCount = (int)total["Total"];
            }
            catch (SqlException ex)
            {
                string sqlerro = ex.Message;
            }
            catch (Exception ex)
            {
                string regularException = ex.Message;
            }

            return restoranDomainModels;
        }
        private KuhinjeModels KuhinjeModelsMethod(DataSet ds ,int i)
        {
            KuhinjeModels kuhinjeModels = new KuhinjeModels();
            DataRow r = ds.Tables[1].Rows[i];
            kuhinjeModels.KuhinjeID = (int)r["KuhinjeID"];
            kuhinjeModels.Ime = Convert.ToString(r["Ime"]);


            return kuhinjeModels;
        }
        private RestoranDomainModel RestoranDomainModel(DataSet ds,int i)
        {
            RestoranDomainModel rdm = new RestoranDomainModel();
            
            DataRow r = ds.Tables[0].Rows[i];
            rdm.RestoranID = (int)r["RestoranID"];
            rdm.Ime = Convert.ToString(r["Ime"]);
            rdm.Logo = Convert.ToString(r["Logo"]);
            if (r["Koordinate"] != null)
            {
                rdm.Koordinate = Convert.ToString(r["Koordinate"]);
            }
            rdm.MinimalnaNarudzbina = Convert.ToDecimal(r["MinimalnaNarudzbina"]);
            rdm.DostavaSeNaplacuje = Convert.ToBoolean(r["DostavaSeNaplacuje"]);
            rdm.Ocena = Convert.ToDecimal(r["Ocena"]);
            if (r["Kuhinja"] != null)
            {
                rdm.Kuhinja = Convert.ToString(r["Kuhinja"]);
            }
            return rdm;
        }
        public RestoraniDomainModel GetRestoransByOffset(int offset, int take)
        {
            RestoraniDomainModel restoranDomainModels = new RestoraniDomainModel();
            //var res = dbcontext.RestoranTbls.OrderBy(x => x.RestoranID).Skip(offset).Take(take).ToList();
            //int count = res.Count;
            //restoranDomainModels.TotalCount = dbcontext.RestoranTbls.ToList().Count;
            //for (int i = 0, j = count - 1; i < count; i++, j--)
            //{
            //    if (j < i) { break; }
            //    RestoranDomainModel restoranDomainModel = new RestoranDomainModel();
            //    restoranDomainModel.RestoranID = res[i].RestoranID;
            //    restoranDomainModel.Ime = res[i].Ime;
            //    restoranDomainModel.Koordinate = res[i].Koordinate;
            //    restoranDomainModel.Logo = res[i].Logo;
            //    restoranDomainModel.MinimalnaNarudzbina = res[i].MinimalnaNarudzbina;
            //    restoranDomainModel.Ocena = res[i].Ocena;
            //    RestoranDomainModel r = new RestoranDomainModel();
            //    r.RestoranID = res[j].RestoranID;
            //    r.Ime = res[j].Ime;
            //    r.Koordinate = res[j].Koordinate;
            //    r.Logo = res[j].Logo;
            //    r.MinimalnaNarudzbina = res[j].MinimalnaNarudzbina;
            //    r.Ocena = res[j].Ocena;
            //    restoranDomainModels.restoranDomainModels.Add(restoranDomainModel);
            //    restoranDomainModels.restoranDomainModels.Add(r);
            //}
            //restoranDomainModels.KuhinjeModels.AddRange(GetKuhinjes());
            return restoranDomainModels;
        }

        public RestoraniDomainModel GetRestoransByOffsetAndCity(int offset, int take, string city)
        {
            RestoraniDomainModel restoranDomainModels = new RestoraniDomainModel();
            return restoranDomainModels;
        }

        public RestoraniDomainModel GetRestoransByAll(int offset, int take, string opstina, int postcode, string restoran, string kuhinje)
        {
            RestoraniDomainModel restoranDomainModels = new RestoraniDomainModel();


            string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlCommand com = new SqlCommand("spSearchBasic", con))
                    {
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@opstina", opstina);
                        com.Parameters.AddWithValue("@postCode", postcode);
                        com.Parameters.AddWithValue("@restorna", restoran);
                        com.Parameters.AddWithValue("@kuhinje", kuhinje);

                        if (con.State != System.Data.ConnectionState.Open) { con.Open(); }
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter(com))
                            {
                                da.Fill(ds);
                            }

                        }
                    }
                }
                if (ds.Tables[0].Rows.Count > 1)
                {
                    for (int i = 0, j = ds.Tables[0].Rows.Count - 1; i < ds.Tables[0].Rows.Count; i++, j--)
                    {
                        if (j < i) { break; }
                        restoranDomainModels.restoranDomainModels.Add(RestoranDomainModel(ds, i));
                        restoranDomainModels.restoranDomainModels.Add(RestoranDomainModel(ds, j));
                    }
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        restoranDomainModels.restoranDomainModels.Add(RestoranDomainModel(ds, i));

                    }
                }



            }
            catch (SqlException ex)
            {
                string sqlerro = ex.Message;
            }
            catch (Exception ex)
            {
                string regularException = ex.Message;
            }

            return restoranDomainModels;
        }
       
        public RestoraniDomainModel GetRestoransPartialy(int offset, int take, string opstina, int postcode, string restoran, string kuhinje)
        {
            RestoraniDomainModel restoranDomainModels = new RestoraniDomainModel();


            string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlCommand com = new SqlCommand("spSearchBasicPartially", con))
                    {
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@opstina", opstina);
                        com.Parameters.AddWithValue("@postCode", postcode);
                        com.Parameters.AddWithValue("@restorna", restoran);
                        com.Parameters.AddWithValue("@kuhinje", kuhinje);
                        com.Parameters.AddWithValue("@skip", offset);
                        if (con.State != System.Data.ConnectionState.Open) { con.Open(); }
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter(com))
                            {
                                da.Fill(ds);
                            }

                        }
                    }
                }
                if (ds.Tables[0].Rows.Count > 1)
                {
                    for (int i = 0, j = ds.Tables[0].Rows.Count - 1; i < ds.Tables[0].Rows.Count; i++, j--)
                    {
                        if (j < i) { break; }
                        restoranDomainModels.restoranDomainModels.Add(RestoranDomainModel(ds, i));
                        restoranDomainModels.restoranDomainModels.Add(RestoranDomainModel(ds, j));
                    }
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        restoranDomainModels.restoranDomainModels.Add(RestoranDomainModel(ds, i));

                    }
                }



            }
            catch (SqlException ex)
            {
                string sqlerro = ex.Message;
            }
            catch (Exception ex)
            {
                string regularException = ex.Message;
            }

            return restoranDomainModels;
        }
    }
}