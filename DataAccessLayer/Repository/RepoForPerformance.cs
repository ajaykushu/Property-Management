using DataAccessLayer.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class RepoForPerformance : IRepoForPerformance 
    {
        private readonly AppDBContext _context;

        public RepoForPerformance(AppDBContext context)
        {
            _context = context;
        }

        public void GetMenu()
        {
          
        }



    }
}
