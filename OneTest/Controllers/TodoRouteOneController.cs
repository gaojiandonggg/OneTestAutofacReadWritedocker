using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OneTest.Model;
using GaoJD.Club.BusinessEntity;
using GaoJD.Club.Redis;
using GaoJD.Club.Utility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

#region
namespace OneTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TodoRouteOneController : BaseApiController
    {

        private readonly TodoContext _context;
        #endregion

        public TodoRouteOneController(TodoContext context, IRedisClient redisClient) : base(redisClient)
        {
            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        #region snippet_GetAll    
        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItems.ToList();
        }
        #endregion


        #region snippet_GetByID
        [HttpGet("{id}/{id1}")]
        //[Route("{id}/{id1}")]
        [AllowAnonymous]
        public ActionResult<TodoItem> GetById(long id, long id1)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            //RedisClient.Set();
            return item;
        }
        #endregion

        #region snippet_GetByID
        [HttpGet("{id}/{id1}/{id3}")]
        //[Route("{id}/{id1}/{id3}")]
        public ActionResult<TodoItem> GetById(long id, long id1, int id3)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        #endregion



        #region snippet_GetByID
        [HttpPost("{id}/{id1}")]
        //[Route("{id}/{id1}")]
        public ActionResult<TodoItem> PostModel(long id, long id1)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        #endregion

        #region snippet_GetByID
        [HttpPost("{id}/{id1}/{id3}")]
        //[Route("{id}/{id1}/{id3}")]
        public ActionResult<TodoItem> PostModel(long id, long id1, int id3)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }


        [HttpPost]
        //[Route("{id}/{id1}")]
        public ActionResult<TodoItem> PostTodoItemModel([FromBody]TodoItem todoItem)
        {
            var item = _context.TodoItems.Find(todoItem.Id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }



        [HttpPost("{id}")]
        public ActionResult<TodoItem> PostTodoItemModelTwo(int id, TodoItem todoItem)
        {
            var item = _context.TodoItems.Find(todoItem.Id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }



        #endregion


    }
}
