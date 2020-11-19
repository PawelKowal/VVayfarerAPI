using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Entities;
using VVayfarerApi.Repositories;

namespace VVayfarerApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IUserRepository _userRepo;
        private IPostRepository _postRepo;
        private ICommentRepository _commentRepo;
        private IReactionRepository _reactionRepo;
        private VVayfarerDbContext _context;
        private UserManager<User> _userManager;
        private IConfiguration _configuration;

        public UnitOfWork(VVayfarerDbContext context, UserManager<User> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepo == null)
                    _userRepo = new UserRepository(_context, _userManager, _configuration);
                return _userRepo;
            }
        }

        public IPostRepository PostRepository
        {
            get
            {
                if (_postRepo == null)
                    _postRepo = new PostRepository(_context);
                return _postRepo;
            }
        }

        public ICommentRepository CommentRepository
        {
            get
            {
                if (_commentRepo == null)
                    _commentRepo = new CommentRepository(_context);
                return _commentRepo;
            }
        }

        public IReactionRepository ReactionRepository
        {
            get
            {
                if (_reactionRepo == null)
                    _reactionRepo = new ReactionRepository(_context);
                return _reactionRepo;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
