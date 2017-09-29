﻿using System.Collections.Generic;
using RepoWebShop.Models;

namespace RepoWebShop.Interfaces
{
    public interface IPieRepository
    {
        IEnumerable<Pie> Pies { get; }
        IEnumerable<PieDetail> PiesOfTheWeek { get; }
        Pie GetPieById(int pieId);
        Pie Add(Pie pie);
        void Delete(int pieId);
    }
}