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
    public class DocGiaController : Controller
    {
        private ThuVienEntities db = new ThuVienEntities();

        // GET: DocGia
        public ActionResult Index()
        {
            var docGias = db.DocGias.Include(d => d.LoaiDG1).Include(d => d.TheThuVien).Include(d => d.Nguoi);
            return View(docGias.ToList());
        }

        // GET: DocGia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocGia docGia = db.DocGias.Find(id);
            if (docGia == null)
            {
                return HttpNotFound();
            }
            return View(docGia);
        }

        // GET: DocGia/Create
        public ActionResult Create()
        {
            ViewBag.LoaiDG = new SelectList(db.LoaiDGs, "id", "name");
            return View();
        }

        // POST: DocGia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NgheNghiep,LoaiDG,Nguoi")] DocGia docGia)
        {
            if (ModelState.IsValid && docGia.Nguoi != null)
            {
                db.Nguois.Add(docGia.Nguoi);
                db.SaveChanges();
                Nguoi nguoi = db.Nguois.ToList().Last();
                docGia.MaNguoi = nguoi.MaNguoi;

                TheThuVien theThuVien = new TheThuVien();
                
                

                db.DocGias.Add(docGia);
                db.SaveChanges();
                DocGia newdocGia = db.DocGias.ToList().Last();
                
                DateTime dateTime = DateTime.Now.Date;
                if (newdocGia.LoaiDG == 0)
                {
                    dateTime = dateTime.AddDays(60);
                } else if (newdocGia.LoaiDG == 1) 
                {
                    dateTime = dateTime.AddDays(180);
                } else if (newdocGia.LoaiDG == 2)
                {
                    dateTime = dateTime.AddDays(365);
                }

                theThuVien.MaDG = newdocGia.MaDG;
                theThuVien.ThoiHan = dateTime;

                db.TheThuViens.Add(theThuVien);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.LoaiDG = new SelectList(db.LoaiDGs, "id", "name", docGia.LoaiDG);
            ViewBag.MaDG = new SelectList(db.TheThuViens, "MaDG", "MaDG", docGia.MaDG);
            ViewBag.MaNguoi = new SelectList(db.Nguois, "MaNguoi", "HoTen", docGia.MaNguoi);
            return View(docGia);
        }

        // GET: DocGia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocGia docGia = db.DocGias.Find(id);
            if (docGia == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoaiDG = new SelectList(db.LoaiDGs, "id", "name", docGia.LoaiDG);
            ViewBag.MaDG = new SelectList(db.TheThuViens, "MaDG", "MaDG", docGia.MaDG);
            ViewBag.MaNguoi = new SelectList(db.Nguois, "MaNguoi", "HoTen", docGia.MaNguoi);
            return View(docGia);
        }

        // POST: DocGia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDG,NgheNghiep,LoaiDG,MaNguoi")] DocGia docGia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(docGia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoaiDG = new SelectList(db.LoaiDGs, "id", "name", docGia.LoaiDG);
            ViewBag.MaDG = new SelectList(db.TheThuViens, "MaDG", "MaDG", docGia.MaDG);
            ViewBag.MaNguoi = new SelectList(db.Nguois, "MaNguoi", "HoTen", docGia.MaNguoi);
            return View(docGia);
        }

        // GET: DocGia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocGia docGia = db.DocGias.Find(id);
            if (docGia == null)
            {
                return HttpNotFound();
            }
            return View(docGia);
        }

        // POST: DocGia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocGia docGia = db.DocGias.Find(id);
            db.DocGias.Remove(docGia);
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
