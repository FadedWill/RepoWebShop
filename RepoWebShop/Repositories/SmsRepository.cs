﻿using Microsoft.Extensions.Configuration;
using RepoWebShop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace RepoWebShop.Repositories
{
    public class SmsRepository : ISmsRepository
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _sender;
        private readonly IConfiguration _config;

        public SmsRepository(IConfiguration config)
        {
            _accountSid = config.GetSection("TwilioAccoundSid").Value;
            _authToken = config.GetSection("TwilioAuthToken").Value;
            _sender = config.GetSection("TwilioSender").Value;
            _config = config;
            TwilioClient.Init(_accountSid, _authToken);
        }

        public void NotifyAdmins(string v)
        {
            var numbers = _config.GetSection("AdminMobiles").GetChildren().Select(x => x.Value);
            foreach(var number in numbers)
                SendSms(number, v);
        }

        public MessageResource SendSms(string phone, string body) =>
            MessageResource.Create(
                from: new PhoneNumber(_sender),
                to: new PhoneNumber("+" + phone),
                body: body);
    }
}
