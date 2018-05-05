﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepoWebShop.Interfaces;
using RepoWebShop.Models;
using RepoWebShop.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepoWebShop.Filters;

namespace RepoWebShop.Controllers
{
    [PageVisitAsync]
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IPieDetailRepository _pieDetailRepository;
        private readonly IGalleryRepository _galleryRepository;
        private readonly IFlickrRepository _flickrRepository;
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly List<SelectListItem> _categories;

        

        public AdminController(IGalleryRepository galleryRepository, IFlickrRepository flickrRepository, IMapper mapper, AppDbContext appDbContext, IPieDetailRepository pieDetailRepository, ICategoryRepository categoryRepository, IPieRepository pieRepository)
        {
            _galleryRepository = galleryRepository;
            _flickrRepository = flickrRepository;
            _pieDetailRepository = pieDetailRepository;
            _categoryRepository = categoryRepository;
            _pieRepository = pieRepository;
            _appDbContext = appDbContext;
            _mapper = mapper;
            _categories = _appDbContext.Categories.Select(x => new SelectListItem() { Value = x.CategoryId.ToString(), Text = x.CategoryName }).ToList();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            var result = _appDbContext.Contacts.ToList();
            return View(result);
        }

        [HttpGet]
        public IActionResult NewContact()
        {
            var result = new Contact();
            return View(result);
        }

        [HttpPost]
        public IActionResult NewContact(Contact contact)
        {
            if(ModelState.IsValid)
            {
                _appDbContext.Contacts.Add(contact);
                _appDbContext.SaveChanges();
                return RedirectToAction("Contacts");
            }
            return View(contact);
        }

        [HttpGet]
        public IActionResult EditContact(int id)
        {
            var result = _appDbContext.Contacts.FirstOrDefault(x => x.ContactId == id);
            if (result == null)
                return NotFound();
            return View(result);
        }

        [HttpPost]
        public IActionResult EditContact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.Contacts.Update(contact);
                _appDbContext.SaveChanges();
                return RedirectToAction("Contacts");
            }
            return View(contact);
        }

        public IActionResult Visits()
        {
            var visits = _appDbContext.PageVisits.OrderByDescending(x => x.PageVisitId).ToList();

            var visitsByPath = visits.GroupBy(x => x.Path).Select(group => new KeyValuePair<string, int>(group.Key, group.Count())).OrderByDescending(x => x.Value).ToList();

            var visitsByIp = visits.GroupBy(x => x.Ip).Select(group => new KeyValuePair<string, int>(group.Key, group.Count())).OrderByDescending(x => x.Value).ToList();

            VisitsViewModel model = new VisitsViewModel
            {
                Visits = visits,
                ByPath = visitsByPath,
                ByIp = visitsByIp
            };

            return View(model);
        }

        public IActionResult Notifications()
        {
            var result = _appDbContext.AdminNotifications.ToList();
            return View(result);
        }

        public IActionResult Prices()
        {
            return View();
        }

        public IActionResult Errors()
        {
            var result = _appDbContext.Exceptions.OrderByDescending(x => x.SiteExceptionId).ToList();
            return View(result);
        }

        public IActionResult AllProducts()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditPieDetail(int id)
        {
            var pieDetail = _pieDetailRepository.PieDetails.First(x => x.PieDetailId == id);

            if (pieDetail == null)
                return NotFound();
            else
            {
                var pieDetailCreateViewModel = _mapper.Map<PieDetail, PieDetailCreateViewModel>(pieDetail);
                var albumes = _flickrRepository.Albums.OrderBy(x => x.Title._Content).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Title._Content });

                pieDetailCreateViewModel.Categories = _categories;
                pieDetailCreateViewModel.Albumes = albumes.ToList();
                return View(pieDetailCreateViewModel);
            }
        }

        [HttpGet]
        [Route("[Controller]/EditPie/{pieDetailId}/{pieId}")]
        public IActionResult EditPie(int pieDetailId, int pieId)
        {
            var pie = _pieRepository.AllPies.FirstOrDefault(x => x.PieId == pieId);
            if (pie == null || pie.PieDetailId != pieDetailId)
                return NotFound();

            else
                return View(pie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/EditPie/{pieDetailId}/{pieId}")]
        public async Task<IActionResult> EditPie(Pie pie)
        {

            if (ModelState.IsValid)
            {
                await _pieRepository.Update(pie);
                
                return RedirectToAction(nameof(EditPieDetail), "Admin", new { id = pie.PieDetailId });
            }
            return View(_pieRepository.AllPies.FirstOrDefault(x => x.PieId == pie.PieId));
        }

        [HttpGet]
        public IActionResult Galleries()
        {
            var result = new List<GalleryFlickrAlbumViewModel>();
            var savedAlbums = _galleryRepository.GetFlickrAlbums();

            foreach(var set in _flickrRepository.Albums)
            {
                GalleryFlickrAlbumViewModel album = new GalleryFlickrAlbumViewModel();
                album.FlickrSetId = set.Id;
                album.FlickrSetTitle = set.Title._Content;
                album.InFlickr = true;
                var savedAlbum = savedAlbums.FirstOrDefault(x => x.FlickrSetId == set.Id);
                if(savedAlbum == null)
                    album.InGallery = false;
                else
                {
                    album.InGallery = savedAlbum.InGallery;
                    album.GalleryFlickrAlbumId = savedAlbum.GalleryFlickrAlbumId;
                }
                result.Add(album);
            }

            foreach(var orphanAlbum in savedAlbums.Where(x => !result.Select(y => y.FlickrSetId).Contains(x.FlickrSetId)))
            {
                GalleryFlickrAlbumViewModel album = new GalleryFlickrAlbumViewModel();
                album.GalleryFlickrAlbumId = orphanAlbum.GalleryFlickrAlbumId;
                album.FlickrSetId = orphanAlbum.FlickrSetId;
                album.InFlickr = false;
                album.InGallery = orphanAlbum.InGallery;
                result.Add(album);
            }
            
            return View(result.OrderBy(x => x.FlickrSetTitle));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPieDetail(PieDetailCreateViewModel pieDetailCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                await _pieDetailRepository.Update(_mapper.Map<PieDetailCreateViewModel, PieDetail>(pieDetailCreateViewModel));

                //return View(pieDetailCreateViewModel);
            }

            pieDetailCreateViewModel.Categories = _categories;
            var albumes = _flickrRepository.Albums.OrderBy(x => x.Title._Content).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Title._Content });
            pieDetailCreateViewModel.Albumes = albumes.ToList();

            return View(pieDetailCreateViewModel);
        }

        [HttpGet]
        [Route("[Controller]/EditPieDetail/AddPie/{id}")]
        public IActionResult AddPieToPieDetail(int id)
        {
            var pieDetail = _pieDetailRepository.PieDetails.First(x => x.PieDetailId == id);
            Pie model = new Pie()
            {
                PieDetail = pieDetail,
                PieDetailId = pieDetail.PieDetailId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/EditPieDetail/AddPie/{id}")]
        public IActionResult AddPieToPieDetail(Pie pie)
        {
            if (ModelState.IsValid)
            {
                var pieId = _pieRepository.Add(pie);

                return RedirectToAction("EditPieDetail/" + pieId.PieDetailId);
            }

            pie.PieDetail = _pieDetailRepository.GetPieDetailById(pie.PieDetailId);
            return View(pie);
        }
    }
}
