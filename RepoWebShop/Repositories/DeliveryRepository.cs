﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RepoWebShop.Extensions;
using RepoWebShop.Interfaces;
using RepoWebShop.Models;
using RepoWebShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RepoWebShop.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _config;
        private readonly int _minimumCharge;
        private readonly int _costByBlock;
        public DeliveryRepository(AppDbContext appDbContext, IConfiguration config)
        {
            _appDbContext = appDbContext;
            _config = config;
            _minimumCharge = _config.GetValue<int>("LowestDeliveryCost");
            _costByBlock = _config.GetValue<int>("DeliveryCostByBlock");
        }

        public void AddOrUpdateDelivery(DeliveryAddress deliveryAddress)
        {
            var existingAddress = _appDbContext.DeliveryAddresses.OrderByDescending(x => x.DeliveryAddressId).FirstOrDefault(x => x.ShoppingCartId == deliveryAddress.ShoppingCartId);

            if (existingAddress != null)
            {
                existingAddress.AddressLine1 = deliveryAddress.AddressLine1;
                existingAddress.Country = deliveryAddress.Country;
                existingAddress.DeliveryCost = GetDeliveryEstimate(deliveryAddress.Distance);
                existingAddress.DeliveryInstructions = deliveryAddress.DeliveryInstructions;
                existingAddress.Distance = deliveryAddress.Distance;
                existingAddress.State = deliveryAddress.State;
                existingAddress.StreetName = deliveryAddress.StreetName;
                existingAddress.StreetNumber = deliveryAddress.StreetNumber;
                existingAddress.ZipCode = deliveryAddress.ZipCode;
                existingAddress.User = deliveryAddress.User;

                _appDbContext.DeliveryAddresses.Update(existingAddress);
            }
            else
            {
                deliveryAddress.Created = DateTime.Now.Zoned(_config.GetSection("LocalZone").Value);
                _appDbContext.DeliveryAddresses.Add(deliveryAddress);
            }
            _appDbContext.SaveChanges();
        }

        public async Task<int> GetDistanceAsync(string addressLine1)
        {
            var destination = await GetGeoLocationAsync(addressLine1);
            var res1 = distance(destination.Key, destination.Value, 'K');

            return res1;
        }

        public async Task<int> GetDrivingDistanceAsync(string addressLine1)
        {
            var httpClient = new HttpClient();
            var distanceApi = "maps.googleapis.com/maps/api/distancematrix";
            var metrics = "units=metric";
            var origins = "origins=place_id:EjVBdi4gSm9zw6kgTWFyw61hIE1vcmVubyA1NjEsIEMxNDI0QUFGIENBQkEsIEFyZ2VudGluYQ"; //Store location
            var apiKey = "key=AIzaSyBxKMKlaxoM9x9Jv-r4KXxhvgHBsKbAmEk";
            var result = await httpClient.GetAsync($"https://{distanceApi}/json?{metrics}&{origins}&destinations={addressLine1}&{apiKey}");
            var body = await result.Content.ReadAsStringAsync();

            var Distance = new JsonSerializer().Deserialize<DistanceMatrix>(new JsonTextReader(new StringReader(body)));
            var distanceMts = Distance.Rows.FirstOrDefault()?.Elements?.FirstOrDefault()?.Distance?.Value ?? 0;
            return distanceMts;
        }

        private async Task<KeyValuePair<string, string>> GetGeoLocationAsync(string addressLine1)
        {
            var requestUri = $"https://maps.googleapis.com/maps/api/geocode/xml?address={(Uri.EscapeDataString(addressLine1))}&key=AIzaSyBxKMKlaxoM9x9Jv-r4KXxhvgHBsKbAmEk&sensor=false";
            KeyValuePair<string, string> result;;
            using (var client = new HttpClient())
            {
                var request = await client.GetAsync(requestUri);
                var content = await request.Content.ReadAsStringAsync();
                var xmlDocument = XDocument.Parse(content);
                var location = xmlDocument.Element("GeocodeResponse").Element("result").Element("geometry").Element("location");
                result = new KeyValuePair<string, string>(location.Element("lat").Value, location.Element("lng").Value);
            }
            return result;
        }

        private int distance(string lat, string lon, char unit)
        {
            var lat1 = Double.Parse(lat, CultureInfo.InvariantCulture);
            var lon1 = Double.Parse(lon, CultureInfo.InvariantCulture);


            var lat2 = -34.625265;
            var lon2 = -58.434483;
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (int)(dist * 1000);
        }
        
        private double deg2rad(double deg) => (deg * Math.PI / 180.0);

        private double rad2deg(double rad) => (rad / Math.PI * 180.0);

        public decimal GetDeliveryEstimate(decimal distance)
        {
            var result = ((int)(distance / 100)) * _costByBlock;
            if (result < _minimumCharge)
                result = _minimumCharge;
            return result;
        }

        public async Task<string> GuessPlaceIdAsync(string address)
        {
            var httpClient = new HttpClient();
            var autocompleteApi = "maps.googleapis.com/maps/api/place/autocomplete";
            var apiKey = "key=AIzaSyBxKMKlaxoM9x9Jv-r4KXxhvgHBsKbAmEk";
            var result = await httpClient.GetAsync($"https://{autocompleteApi}/xml?input={address}&types=address&{apiKey}");
            var body = await result.Content.ReadAsStringAsync();

            var xmlDocument = XDocument.Parse(body);
            var place_id = xmlDocument.Element("AutocompletionResponse").Element("prediction").Element("place_id").Value;


            var test = await GetPlaceAsync(place_id);

            return place_id;
        }

        public async Task<AddressViewModel> GetPlaceAsync(string placeId)
        {
            var httpClient = new HttpClient();
            var placeApi = "maps.googleapis.com/maps/api/place/details";
            var apiKey = "key=AIzaSyBxKMKlaxoM9x9Jv-r4KXxhvgHBsKbAmEk";
            var response = await httpClient.GetAsync($"https://{placeApi}/xml?placeid={placeId}&{apiKey}");
            var body = await response.Content.ReadAsStringAsync();

            var xmlDocument = XDocument.Parse(body);
            //var place = xmlDocument.Element("AutocompletionResponse").Element("prediction").Element("place_id").Value;

            var result = xmlDocument.Element("PlaceDetailsResponse").Element("result");

            var addressLine1 = result.Element("formatted_address").Value;
            string postalCode = string.Empty, streetNumber = string.Empty, streetName = string.Empty;

            foreach(var addressComponent in result.Elements("address_component"))
            {
                if (addressComponent.Element("type").Value == "route")
                    streetName = addressComponent.Element("long_name").Value;

                if (addressComponent.Element("type").Value == "street_number")
                    streetNumber = addressComponent.Element("long_name").Value;

                if (addressComponent.Element("type").Value == "postal_code")
                    postalCode = addressComponent.Element("long_name").Value;
            }

            var address = new AddressViewModel()
            {
                AddressLine1 = addressLine1,
                PostalCode = postalCode,
                StreetName = streetName,
                StreetNumber = streetNumber
            };

            return address;
        }


    }
}
