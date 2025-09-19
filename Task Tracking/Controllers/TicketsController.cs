using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task_Tracking.Data;
using Task_Tracking.Models;


namespace Task_Tracking.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets/Kanban
        public async Task<IActionResult> Index()
        {
            var allTickets = await _context.Ticket
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatorUser)
                .ToListAsync();

            var viewModel = new KanbanViewModel
            {
                OpenTickets = allTickets.Where(t => t.Status == TicketStatus.Open).ToList(),
                InProgressTickets = allTickets.Where(t => t.Status == TicketStatus.InProgress).ToList(),
                ResolvedTickets = allTickets.Where(t => t.Status == TicketStatus.Resolved).ToList(),
                BlockerTickets = allTickets.Where(t => t.Status == TicketStatus.Blocker).ToList(),
                ClosedTickets = allTickets.Where(t => t.Status == TicketStatus.Closed).ToList()
            };

            return View(viewModel);
        }

        // GET: Tickets
        // Inside TicketsController
        public async Task<IActionResult> Kanban()
        {
            var allTickets = await _context.Ticket.Include(t => t.AssignedToUser).Include(t => t.CreatorUser).ToListAsync();

            var viewModel = new KanbanViewModel
            {
                OpenTickets = allTickets.Where(t => t.Status == TicketStatus.Open).ToList(),
                InProgressTickets = allTickets.Where(t => t.Status == TicketStatus.InProgress).ToList(),
                ResolvedTickets = allTickets.Where(t => t.Status == TicketStatus.Resolved).ToList(),
                BlockerTickets = allTickets.Where(t => t.Status == TicketStatus.Blocker).ToList(),
                ClosedTickets = allTickets.Where(t => t.Status == TicketStatus.Closed).ToList()
            };

            return View(viewModel);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatorUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["AssignedToUserId"] = new SelectList(_context.Set<User>(), "Id", "Id");
            ViewData["CreatorUserId"] = new SelectList(_context.Set<User>(), "Id", "Id");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Status,Priority,CreationDate,DueDate,AssignedToUserId,CreatorUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedToUserId"] = new SelectList(_context.Set<User>(), "Id", "Id", ticket.AssignedToUserId);
            ViewData["CreatorUserId"] = new SelectList(_context.Set<User>(), "Id", "Id", ticket.CreatorUserId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["AssignedToUserId"] = new SelectList(_context.Set<User>(), "Id", "Id", ticket.AssignedToUserId);
            ViewData["CreatorUserId"] = new SelectList(_context.Set<User>(), "Id", "Id", ticket.CreatorUserId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Status,Priority,CreationDate,DueDate,AssignedToUserId,CreatorUserId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedToUserId"] = new SelectList(_context.Set<User>(), "Id", "Id", ticket.AssignedToUserId);
            ViewData["CreatorUserId"] = new SelectList(_context.Set<User>(), "Id", "Id", ticket.CreatorUserId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatorUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
