using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using Xamarin.Forms;
using System.Net;
using System.Linq;

namespace DemoApp.Classes
{
    class ImgURLsOnLoad
    {
        // Адрес запроса случайных изображений
        // You can enter your own Access key or use default
        // Also can be done with Oauth to get the key
        private string requestURL = @"https://api.unsplash.com/photos/?client_id=a39be0792706d2793a01066693f424df220c49f582483c9d959fb1f6dbe76f38";
        // Адреса случайных изображений
        public List<string> RndFullUrls = new List<string>();
        public List<string> RndThumbsUrls = new List<string>();
        // Адреса избранных изображений
        // Будут подгружаться из файла и доступны по сути сразу

        public void GetImgsURLs()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(requestURL);
            HttpWebResponse responce = (HttpWebResponse)request.GetResponse();
            Stream dataStream;
            StreamReader reader;


            // Проверка ответа сервера
            if (responce == null || responce.StatusCode != HttpStatusCode.OK)
            {
                // Сообщаем пользователю о отсутсвии интернета
                Classes.AlertClass alert = new Classes.AlertClass();
                alert.AlertStart();
            }
            else
            {
                // Обрабатывает ответ сервера
                dataStream = responce.GetResponseStream();
                reader = new StreamReader(dataStream);
                string responceString = reader.ReadToEnd();
                JArray arr = JArray.Parse(responceString);
                RndThumbsUrls = new List<string>(arr.Children().Select(jt => jt["urls"]["thumb"].ToObject<string>()));
                RndFullUrls = new List<string>(arr.Children().Select(jt => jt["urls"]["full"].ToObject<string>()));
            }
        }
    }
}
