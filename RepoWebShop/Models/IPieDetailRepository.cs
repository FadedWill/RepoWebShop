﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepoWebShop.Models
{
    public interface IPieDetailRepository
    {
        IEnumerable<PieDetail> PieDetails { get; }
        PieDetail GetPieDetailById(int pieDetailId);
    }
}