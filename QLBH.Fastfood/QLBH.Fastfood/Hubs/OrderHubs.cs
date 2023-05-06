using Microsoft.AspNet.SignalR;
using QLBH.Fastfood.ListenerModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace QLBH.Fastfood.Hubs
{
    public class OrderHubs : Hub
    {
        public OrderHubs()
        {
            var tableDependency = new SqlTableDependency<ListenerOrder>(ConfigurationManager.ConnectionStrings["QLBHffconnectionString"].ConnectionString, tableName: "DonHang", schemaName: "dbo", executeUserPermissionCheck: false, includeOldValues: true);
            tableDependency.OnChanged += TableDependency_Changed;
            tableDependency.OnError += TableDependency_OnError;

            tableDependency.Start();
        }

        private void TableDependency_Changed(object sender, RecordChangedEventArgs<ListenerOrder> e)
        {
            Show();
        }

        private void TableDependency_OnError(object sender, ErrorEventArgs e)
        {

        }

        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<OrderHubs>();
            context.Clients.All.displayOrderStatus();
        }
    }
}