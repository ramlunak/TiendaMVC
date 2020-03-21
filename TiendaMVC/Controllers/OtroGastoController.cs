using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ControlGastos.Models;

namespace ControlGastos.Controllers
{
    public class OtroGastoController : Controller
    {
        private ControlGastosContext db = new ControlGastosContext();

        // GET: OtroGasto
        public async Task<ActionResult> Index()
        {
            return View(await db.OtroGastoes.ToListAsync());
        }

        // GET: OtroGasto/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtroGasto otroGasto = await db.OtroGastoes.FindAsync(id);
            if (otroGasto == null)
            {
                return HttpNotFound();
            }
            return View(otroGasto);
        }

        // GET: OtroGasto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OtroGasto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre")] OtroGasto otroGasto)
        {
            if (ModelState.IsValid)
            {
                db.OtroGastoes.Add(otroGasto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(otroGasto);
        }

        // GET: OtroGasto/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtroGasto otroGasto = await db.OtroGastoes.FindAsync(id);
            if (otroGasto == null)
            {
                return HttpNotFound();
            }
            return View(otroGasto);
        }

        // POST: OtroGasto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre")] OtroGasto otroGasto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(otroGasto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(otroGasto);
        }

        // GET: OtroGasto/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtroGasto otroGasto = await db.OtroGastoes.FindAsync(id);
            if (otroGasto == null)
            {
                return HttpNotFound();
            }
            return View(otroGasto);
        }

        // POST: OtroGasto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OtroGasto otroGasto = await db.OtroGastoes.FindAsync(id);
            db.OtroGastoes.Remove(otroGasto);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
