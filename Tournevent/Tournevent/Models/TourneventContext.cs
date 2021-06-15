using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class TourneventContext
    {
        private static readonly string dataSource = @"(localdb)\MSSQLLocalDB";
        public static string UserRights = "Benutzer";

        public static string Connect()
        {
            SqlConnectionStringBuilder sqlString = new SqlConnectionStringBuilder()
            {
                DataSource = dataSource,
                InitialCatalog = "RLP2021_INAI3a_Gruppe5_ID",
                UserID = "RLP2021_" + UserRights,
                Password = "RLP2021_" + UserRights,
            };
            EntityConnectionStringBuilder entityString = new EntityConnectionStringBuilder()
            {
                Provider = "System.Data.SqlClient",
                Metadata = "res://*/Models.DataModel.csdl|res://*/Models.DataModel.ssdl|res://*/Models.DataModel.msl",
                ProviderConnectionString = sqlString.ToString()
            };
            return entityString.ConnectionString;
        }
    }
}