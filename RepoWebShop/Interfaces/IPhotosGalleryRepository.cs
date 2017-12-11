﻿using RepoWebShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepoWebShop.Interfaces
{
    public interface IPhotosGalleryRepository
    {
        void AddFlickrAlbum(string setId);
        void HideFlickrAlbum(string setId);
        void RemoveFlickrAlbum(string setId);
        IEnumerable<GalleryFlickrAlbum> GetFlickrAlbums();
        IEnumerable<PhotosetPhotos> GetGalleryPictures();
    }
}
