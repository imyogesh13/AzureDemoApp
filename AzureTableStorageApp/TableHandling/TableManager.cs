using AzureTableStorageApp.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AzureTableStorageApp.TableHandling
{
    public class TableManager
    {
        private CloudTable cloudTable;
        public TableManager(string tableName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CloudStorageConnection"].ConnectionString;

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            cloudTable = cloudTableClient.GetTableReference(tableName);
            cloudTable.CreateIfNotExists();
        }

        public void InsertEntity<T>(T entity) where T : TableEntity, new()
        {
            try
            {
                var insertOperation = TableOperation.Insert(entity);
                cloudTable.Execute(insertOperation);
            }
            catch (Exception obj)
            {
                throw obj;
            }
        }

        public void UpdateEntity<T>(T entity) where T : TableEntity, new()
        {
            try
            {
                var updateOperation = TableOperation.Replace(entity);
                cloudTable.Execute(updateOperation);
            }
            catch (Exception obj)
            {
                throw obj;
            }
        }

        public List<T> RetriveEntity<T>(string query = null) where T : TableEntity, new()
        {
            try
            {
                TableQuery<T> dataTableQuery = new TableQuery<T>();
                if (!string.IsNullOrEmpty(query))
                {
                    dataTableQuery = new TableQuery<T>().Where(query);
                }
                IEnumerable<T> iDataList = cloudTable.ExecuteQuery(dataTableQuery);
                List<T> dataList = new List<T>();
                foreach (var item in iDataList)
                {
                    dataList.Add(item);
                }
                return dataList;
            }
            catch (Exception obj)
            {
                throw obj;
            }
        }

        public bool DeleteEntity<T>(T entity) where T: TableEntity, new()
        {
            try
            {
                var deleteOperation = TableOperation.Delete(entity);
                cloudTable.Execute(deleteOperation);
                return true;
            }
            catch (Exception obj)
            {
                throw obj;
            }
        }
    }
}