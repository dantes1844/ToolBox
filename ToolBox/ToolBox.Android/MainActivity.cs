using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ToolBox.Core;
using System.IO;

namespace ToolBox.Droid
{
    [Activity(Label = "ToolBox", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private void InialData()
        {
            try
            {
                var sqliteFilename = SystemConst.DbName;
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                var path = Path.Combine(documentsPath, sqliteFilename);
                if (File.Exists(path))
                {
                   // return;
                }

                var s = Resources.OpenRawResource(Resource.Raw.toolbox);
                //创建写入列
                FileStream writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                ReadWriteStream(s, writeStream);
                Toast.MakeText(this, "初始化数据完了", ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "初始化数据挂了:" + ex.ToString(), ToastLength.Long).Show();
            }
        }

        void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            using (readStream)
            using (writeStream)
            {
                int Length = 256;
                byte[] buffer = new byte[Length];
                int bytesRead = readStream.Read(buffer, 0, Length);

                // 写入所需字节
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = readStream.Read(buffer, 0, Length);
                }
                readStream.Close();
                writeStream.Close();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            InialData();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private DateTime? lastClickTime = null;

        public override void OnBackPressed()
        {
            if (lastClickTime.HasValue && (DateTime.Now - lastClickTime.Value).TotalSeconds < 2)
            {
                base.OnBackPressed();
            }
            else
            {
                Toast.MakeText(Application.Context, "再点一次退出", ToastLength.Short).Show();
                lastClickTime = DateTime.Now;
            }
        }
    }
}