using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListRazor7.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookListRazor7.Pages.BookList
{
    public class UpsertModel : PageModel
    {
        private ApplicationDbContext _db;

        public UpsertModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Book Book { get; set; }
        public async Task<IActionResult> OnGet(int? id)  // because theres a possibility id wont be used if we are creating a Book we use the ? to make it a nullable interger so id can be null
        {
            Book = new Book();
            if (id == null)
                {
                //to create
                    return Page();
                }

            Book = await _db.Book.FirstOrDefaultAsync(u => u.Id == id);
            if (Book == null)
            {
                // to update
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
               if(Book.Id == 0)
                {
                    _db.Book.Add(Book);
                } else
                {
                    // this code below updates everything in the book
                    _db.Book.Update(Book);

                    // this code does it individually
                    //var BookFromDb = await _db.Book.FindAsync(Book.Id);
                    //BookFromDb.Name = Book.Name;
                    //BookFromDb.ISBN = Book.ISBN;
                    //BookFromDb.Author = Book.Author;
                }

                await _db.SaveChangesAsync();

                return RedirectToPage("Index");
            }
            return RedirectToPage();
        }
    }
}
