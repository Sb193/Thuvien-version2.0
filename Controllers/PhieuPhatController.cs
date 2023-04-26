using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class PhieuPhatController : Controller
    {
        private ThuVienEntities db = new ThuVienEntities();

        // GET: PhieuPhat
        public ActionResult Index()
        {
            var phieuPhats = db.PhieuPhats.Include(p => p.PhieuMuon);
            return View(phieuPhats.ToList());
        }

        // GET: PhieuPhat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuPhat phieuPhat = db.PhieuPhats.Find(id);
            if (phieuPhat == null)
            {
                return HttpNotFound();
            }
            return View(phieuPhat);
        }

        // GET: PhieuPhat/Create
        public ActionResult Create()
        {
            ViewBag.MaPM = new SelectList(db.PhieuMuons, "MaPM", "MaNV");
            return View();
        }

        // POST: PhieuPhat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPP,MaPM,LyDo")] PhieuPhat phieuPhat)
        {
            if (ModelState.IsValid)
            {
                db.PhieuPhats.Add(phieuPhat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaPM = new SelectList(db.PhieuMuons, "MaPM", "MaNV", phieuPhat.MaPM);
            return View(phieuPhat);
        }

        // GET: PhieuPhat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuPhat phieuPhat = db.PhieuPhats.Find(id);
            if (phieuPhat == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPM = new SelectList(db.PhieuMuons, "MaPM", "MaNV", phieuPhat.MaPM);
            return View(phieuPhat);
        }

        // POST: PhieuPhat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPP,MaPM,LyDo")] PhieuPhat phieuPhat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuPhat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaPM = new SelectList(db.PhieuMuons, "MaPM", "MaNV", phieuPhat.MaPM);
            return View(phieuPhat);
        }

        // GET: PhieuPhat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuPhat phieuPhat = db.PhieuPhats.Find(id);
            if (phieuPhat == null)
            {
                return HttpNotFound();
            }
            return View(phieuPhat);
        }

        // POST: PhieuPhat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuPhat phieuPhat = db.PhieuPhats.Find(id);
            db.PhieuPhats.Remove(phieuPhat);
            db.SaveChanges();
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
