using Android.Database.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Core;
using ToolBox.Core.Domain;

namespace ToolBox.Data
{
    public class BusStationMarkRepository : IRepository<BusStationMark>
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), SystemConst.DbName);

        public Task<bool> AddItemAsync(BusStationMark item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<BusStationMark> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BusStationMark>> GetItemsAsync(bool forceRefresh = false)
        {
            List<BusStationMark> list = new List<BusStationMark>();
            try
            {
                var conn = CreateConnection();

                var cursor = conn.RawQuery("select * from BusStationMark", null);

                for (int i = 0; i < cursor.Count; i++)
                {
                    if (i == 0)             //确定游标位置
                    {
                        cursor.MoveToFirst();
                    }
                    else
                    {
                        cursor.MoveToNext();
                    }
                    var idiom = new BusStationMark();
                    idiom.Id = cursor.GetString(cursor.GetColumnIndex("Id"));
                    idiom.BusNumber = cursor.GetString(cursor.GetColumnIndex("BusNumber"));
                    idiom.MarkStationNumber = cursor.GetInt(cursor.GetColumnIndex("MarkStationNumber"));
                    idiom.CreationTime = Convert.ToDateTime(cursor.GetString(cursor.GetColumnIndex("CreationTime")));
                    idiom.IsDeleted = cursor.GetInt(cursor.GetColumnIndex("IsDeleted")) == 1;
                    list.Add(idiom);
                }
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {

            }
            return await Task.FromResult(list);
        }

        public Task<bool> UpdateItemAsync(BusStationMark item)
        {
            throw new NotImplementedException();
        }

        private SQLiteDatabase CreateConnection()
        {
            return SQLiteDatabase.OpenOrCreateDatabase(dbPath, null);
        }

    }
}
