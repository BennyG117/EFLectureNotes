using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EFLectureNotes.Models;
using System.ComponentModel;

//!ADDED for session check
using Microsoft.AspNetCore.Mvc.Filters;

namespace EFLectureNotes.Controllers;


//! Add SessionCheck above class controller* 
[SessionCheck]
public class PostController : Controller
{
    private readonly ILogger<PostController> _logger;
    
    // Add field - adding context into our class // "db" can eb any name
    private MyContext db;

    // update below adding context, etc
    public PostController(ILogger<PostController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }




    // Update AllPosts Route - update View* ============================================
    [HttpGet("/posts")]
    public IActionResult Index()
    {
        List<Post> allPosts = db.Posts.ToList();
        return View("AllPosts", allPosts);
    }


    // New View Route
    [HttpGet("posts/new")]
    public IActionResult NewPost()
    {
        return View("New");
    }


    // CreatePost method ============================================
    [HttpPost("posts/create")]
    public IActionResult CreatePost(Post newPost)
    {
        if(!ModelState.IsValid)
        {
            return View("New");
        }
        //! using db table name "Posts"
        db.Posts.Add(newPost);
        db.SaveChanges();
        return RedirectToAction("Index");
    }


    // view one post method ============================================
    [HttpGet("posts/{postId}")]
    public IActionResult ViewPost(int postId)
    {
        //Query below:
        Post post = db.Posts.FirstOrDefault(post => post.PostId == postId);
        if(post == null) 
        {
            return RedirectToAction("Index");
        }
        return View("ViewPost", post );
    }


    // EDIT post method ============================================
    [HttpGet("posts/{postId}/edit")]
    public IActionResult Edit(int postId)
    {
        //Query below:
        Post? post = db.Posts.FirstOrDefault(post => post.PostId == postId);
        if(post == null) 
        {
            return RedirectToAction("Index");
        }
        return View("Edit", post );
    }

    //Update Method ============================================
    [HttpPost("posts/{postId}/update")]
    // MatchCasing to the postId route
    public IActionResult Update(Post editPost, int postId)

    {
        if(!ModelState.IsValid)
        {
            return Edit(postId);
            // return View("Edit");
        }
        Post? post = db.Posts.FirstOrDefault(post => post.PostId == postId);
        if(post == null) 
        {
            return RedirectToAction("Index");
        }
        post.Topic = editPost.Topic;
        post.Body = editPost.Body;
        post.ImageUrl = editPost.ImageUrl;
        db.Posts.Update(post);
        db.SaveChanges();
        return RedirectToAction("ViewPost", new {postId = postId});

    }


    //Delete Method ============================================
    [HttpPost("posts/{postId}/delete")]
    public IActionResult Delete(int postId)
    {
        Post? post = db.Posts.FirstOrDefault(post => post.PostId == postId);
        db.Posts.Remove(post);
        db.SaveChanges();
        // ListSortDescription in the all posts for Index*
        return RedirectToAction("Index");
    }

//!SESSION CHECK ===========================================
// Name this anything you want with the word "Attribute" at the end -- adding filter for session at top*
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? userId = context.HttpContext.Session.GetInt32("UUID");
        // Check to see if we got back null
        if(userId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("Index", "User", null);
        }
    }
}





    // Privacy Route ============================================
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
