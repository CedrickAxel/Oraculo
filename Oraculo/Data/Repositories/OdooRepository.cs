using Oraculo.Models;
using Sap.Data.Hana;

namespace Oraculo.Data.Repositories
{
    public class OdooRepository : IOdooRepository
    {
        private readonly Func<int, HanaConnection> _connectionString;

        public OdooRepository(Func<int, HanaConnection> connectionString)
        {
            _connectionString = connectionString;
        }

        protected HanaConnection dbConnection(int environment)
        {
            return _connectionString(environment);
        }

        public async Task<List<OdooProductStockPrice>> GetProductsAsync(int environment)
        {
            var products = new List<OdooProductStockPrice>();

            using (HanaConnection conn = dbConnection(environment))
            {
                await conn.OpenAsync();

                string query = @"
                    SELECT 
                        T0.""ItemCode"",
                        T0.""ItemName"",
                        T3.""ItmsGrpCod"",
                        T3.""ItmsGrpNam"",
                        T0.""InvntryUom"",
                        T0.""SellItem"",
                        T0.""PrchseItem"",

                        -- LISTAS DE PRECIO
                        MAX(CASE WHEN T1.""PriceList"" = 1 THEN T1.""Price"" END) AS ""PLGDL"",
                        MAX(CASE WHEN T1.""PriceList"" = 2 THEN T1.""Price"" END) AS ""PLColima"",
                        MAX(CASE WHEN T1.""PriceList"" = 6 THEN T1.""Price"" END) AS ""PLGuzman"",
                        MAX(CASE WHEN T1.""PriceList"" = 7 THEN T1.""Price"" END) AS ""PLTecoman"",
                        MAX(CASE WHEN T1.""PriceList"" = 8 THEN T1.""Price"" END) AS ""PLManzanillo"",

                        -- STOCK POR SUCURSAL / ALMACÉN
                        SUM(CASE WHEN T2.""WhsCode"" = '309' THEN T2.""OnHand"" ELSE 0 END) AS ""Mandarina"",
                        SUM(CASE WHEN T2.""WhsCode"" = '311' THEN T2.""OnHand"" ELSE 0 END) AS ""Mercado"",
                        SUM(CASE WHEN T2.""WhsCode"" = '313' THEN T2.""OnHand"" ELSE 0 END) AS ""Granadilla"",
                        SUM(CASE WHEN T2.""WhsCode"" = '315' THEN T2.""OnHand"" ELSE 0 END) AS ""BaseAerea"",
                        SUM(CASE WHEN T2.""WhsCode"" = '317' THEN T2.""OnHand"" ELSE 0 END) AS ""Tlajomulco"",
                        SUM(CASE WHEN T2.""WhsCode"" = '338' THEN T2.""OnHand"" ELSE 0 END) AS ""TlajomulcoCentro"",
                        SUM(CASE WHEN T2.""WhsCode"" = '319' THEN T2.""OnHand"" ELSE 0 END) AS ""OchoDeJulio"",
                        SUM(CASE WHEN T2.""WhsCode"" = '321' THEN T2.""OnHand"" ELSE 0 END) AS ""JuanDeLaBarrera"",
                        SUM(CASE WHEN T2.""WhsCode"" = '325' THEN T2.""OnHand"" ELSE 0 END) AS ""ChavezCarrillo"",
                        SUM(CASE WHEN T2.""WhsCode"" = '327' THEN T2.""OnHand"" ELSE 0 END) AS ""NinosHeroes"",
                        SUM(CASE WHEN T2.""WhsCode"" = '329' THEN T2.""OnHand"" ELSE 0 END) AS ""Tecoman"",
                        SUM(CASE WHEN T2.""WhsCode"" = '323' THEN T2.""OnHand"" ELSE 0 END) AS ""CiudadGuzman"",
                        SUM(CASE WHEN T2.""WhsCode"" = '331' THEN T2.""OnHand"" ELSE 0 END) AS ""Manzanillo"",
                        SUM(CASE WHEN T2.""WhsCode"" = '334' THEN T2.""OnHand"" ELSE 0 END) AS ""VillaDeAlvarez""

                    FROM OITM T0

                    INNER JOIN ITM1 T1 
                        ON T0.""ItemCode"" = T1.""ItemCode""

                    LEFT JOIN OITW T2
                        ON T0.""ItemCode"" = T2.""ItemCode""

                    INNER JOIN ""OITB"" T3 
                        ON T0.""ItmsGrpCod"" = T3.""ItmsGrpCod""

                    WHERE 
                        T0.""frozenFor"" = 'N'
                        AND LEFT(T0.""ItemCode"", 2) BETWEEN '01' AND '07'

                    GROUP BY
                        T0.""ItemCode"",
                        T0.""ItemName"",
                        T3.""ItmsGrpCod"",
                        T3.""ItmsGrpNam"",
                        T0.""InvntryUom"",
                        T0.""SellItem"",
                        T0.""PrchseItem""
                    ";

                using (HanaCommand cmd = new HanaCommand(query, conn))
                {
                    using (HanaDataReader reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            products.Add(new OdooProductStockPrice
                            {
                                ItemCode = reader["ItemCode"].ToString(),
                                ItemName = reader["ItemName"].ToString(),
                                ItmsGrpCod = reader["ItmsGrpCod"].ToString(),
                                ItmsGrpNam = reader["ItmsGrpNam"].ToString(),
                                InvntryUom = reader["InvntryUom"].ToString(),
                                SellItem = reader["SellItem"].ToString(),
                                PrchseItem = reader["PrchseItem"].ToString(),

                                // PRECIOS
                                PLGDL = reader["PLGDL"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PLGDL"]),
                                PLColima = reader["PLColima"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PLColima"]),
                                PLGuzman = reader["PLGuzman"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PLGuzman"]),
                                PLTecoman = reader["PLTecoman"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PLTecoman"]),
                                PLManzanillo = reader["PLManzanillo"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PLManzanillo"]),

                                // STOCKS
                                Mandarina = reader["Mandarina"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Mandarina"]),
                                Mercado = reader["Mercado"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Mercado"]),
                                Granadilla = reader["Granadilla"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Granadilla"]),
                                BaseAerea = reader["BaseAerea"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["BaseAerea"]),
                                Tlajomulco = reader["Tlajomulco"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Tlajomulco"]),
                                TlajomulcoCentro = reader["TlajomulcoCentro"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TlajomulcoCentro"]),
                                OchoDeJulio = reader["OchoDeJulio"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["OchoDeJulio"]),
                                JuanDeLaBarrera = reader["JuanDeLaBarrera"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["JuanDeLaBarrera"]),
                                ChavezCarrillo = reader["ChavezCarrillo"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ChavezCarrillo"]),
                                NinosHeroes = reader["NinosHeroes"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["NinosHeroes"]),
                                Tecoman = reader["Tecoman"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Tecoman"]),
                                CiudadGuzman = reader["CiudadGuzman"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["CiudadGuzman"]),
                                Manzanillo = reader["Manzanillo"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Manzanillo"]),
                                VillaDeAlvarez = reader["VillaDeAlvarez"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["VillaDeAlvarez"])
                            });
                        }
                    }
                }
            }

            return products;
        }
    }
}
