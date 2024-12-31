using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net.Data;
using asp.net.Interfaces;
using asp.net.Models;
using Microsoft.EntityFrameworkCore;

namespace asp.net.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }
    }
}