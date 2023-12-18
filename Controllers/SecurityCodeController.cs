using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyCaThi.Data;
using QuanLyCaThi.Models;
using QuanLyCaThi.Models.Process;

namespace QuanLyCaThi.Controllers
{
    public class SecurityCodeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SecurityCodeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SecurityCode
        public async Task<IActionResult> Index()
        {
              return _context.SecurityCode != null ? 
                          View(await _context.SecurityCode.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.SecurityCode'  is null.");
        }
        public async Task<IActionResult> InitialOrRestore()
        {
            var model = await _context.SecurityCode.ToListAsync();
            if(model.Count == 0)
            {
                var newKeyInsert = new SecurityCode();
                newKeyInsert.SecurityKey = StringProcess.CreateMD5Hash("12312300");
                _context.Add(newKeyInsert);
                var newKeyExport = new SecurityCode();
                newKeyExport.SecurityKey = StringProcess.CreateMD5Hash("123123");
                _context.Add(newKeyExport);
            }else {
                var keyInsert = await _context.SecurityCode.FindAsync(model[0].id);
                keyInsert.SecurityKey = StringProcess.CreateMD5Hash("12312300");
                var keyExport = await _context.SecurityCode.FindAsync(model[1].id);
                keyExport.SecurityKey = StringProcess.CreateMD5Hash("123123");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: SecurityCode/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SecurityCode == null)
            {
                return NotFound();
            }

            var securityCode = await _context.SecurityCode.FindAsync(id);
            if (securityCode == null)
            {
                return NotFound();
            }
            return View(securityCode);
        }

        // POST: SecurityCode/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,SecurityKey")] SecurityCode securityCode)
        {
            if (id != securityCode.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    securityCode.SecurityKey = StringProcess.CreateMD5Hash(securityCode.SecurityKey);
                    _context.Update(securityCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecurityCodeExists(securityCode.id))
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
            return View(securityCode);
        }

        private bool SecurityCodeExists(int id)
        {
          return (_context.SecurityCode?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
