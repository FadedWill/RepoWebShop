﻿using RepoWebShop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepoWebShop.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace RepoWebShop.Repositories
{
    public class PhotosetAlbums : IPhotosetAlbums
    {
        private readonly IConfiguration _config;
        private readonly string apiUrl = "https://api.flickr.com/services/rest/";
        private readonly PhotosetList albums;

        private List<PhotosetPhotos> pictures = new List<PhotosetPhotos>();

            //{
            //    var photoSetid = photoset.Id;
            //    var queryString = $"?method=flickr.photosets.getPhotos&api_key={_config["FlickrClientId"]}&photoset_id={photoSetid}&format=json&user_id={_config["FlickrUserId"]}&nojsoncallback=?";

            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl + queryString);
            //    request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            //    request.Accept = "application/json";
            //    request.Method = "GET";

            //    HttpWebResponse apiResult = (HttpWebResponse)request.GetResponse();
            //    result.Add(new JsonSerializer().Deserialize<PhotosetPhotos>(new JsonTextReader(new StreamReader(apiResult.GetResponseStream()))));
            //}

        public PhotosetAlbums(IConfiguration config)
        {
            _config = config;
            albums = GetPhotosets();
        }

        public PhotosetList Albums
        {
            get
            {
                return albums;
            }
        }

        public PhotosetPhotos GetPhotos(long id)
        {
            var result = pictures.FirstOrDefault(x => x.Photoset.Id == id);
            if (result != null)
                return result;
            else
                GetPics(id);
            return pictures.FirstOrDefault(x => x.Photoset.Id == id);
        }

        private PhotosetList GetPhotosets()
        {
            var queryString = $"?method=flickr.photosets.getList&api_key={_config["FlickrClientId"]}&format=json&user_id={_config["FlickrUserId"]}&nojsoncallback=?";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl + queryString);
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            request.Accept = "application/json";
            request.Method = "GET";

            HttpWebResponse apiResult = (HttpWebResponse)request.GetResponse();
            return new JsonSerializer().Deserialize<PhotosetList>(new JsonTextReader(new StreamReader(apiResult.GetResponseStream())));
        }

        private void GetPics(long id)
        {
            var queryString = $"?method=flickr.photosets.getPhotos&api_key={_config["FlickrClientId"]}&photoset_id={id}&format=json&user_id={_config["FlickrUserId"]}&nojsoncallback=?";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl + queryString);
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            request.Accept = "application/json";
            request.Method = "GET";

            HttpWebResponse apiResult = (HttpWebResponse)request.GetResponse();
            var result = new JsonSerializer().Deserialize<PhotosetPhotos>(new JsonTextReader(new StreamReader(apiResult.GetResponseStream())));

            pictures.Add(result);
        }
    }
}