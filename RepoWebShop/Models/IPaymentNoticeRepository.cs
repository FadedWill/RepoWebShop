﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepoWebShop.Models
{
    public interface IPaymentNoticeRepository
    {
        void CreatePayment(PaymentNotice paymentWebhook);
    }
}