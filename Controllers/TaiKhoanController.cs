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
    public class TaiKhoanController : Controller
    {
        private ThuVienEntities db = new ThuVienEntities();

        // GET: TaiKhoan
        public ActionResult Index()
        {
            var taiKhoans = db.TaiKhoans.Include(t => t.LoaiTK1).Include(t => t.NhanVien);
            return View(taiKhoans.ToList());
        }

        // GET: TaiKhoan/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // GET: TaiKhoan/Create
        public ActionResult Create()
        {
            ViewBag.LoaiTK = new SelectList(db.LoaiTKs, "id", "name");
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Chucvu");
            return View();
        }

        // POST: TaiKhoan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaiKhoan1,MatKhau,LoaiTK,MaNV")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LoaiTK = new SelectList(db.LoaiTKs, "id", "name", taiKhoan.LoaiTK);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Chucvu", taiKhoan.MaNV);
            return View(taiKhoan);
        }

        // GET: TaiKhoan/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoaiTK = new SelectList(db.LoaiTKs, "id", "name", taiKhoan.LoaiTK);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Chucvu", taiKhoan.MaNV);
            return View(taiKhoan);
        }

        // POST: TaiKhoan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaiKhoan1,MatKhau,LoaiTK,MaNV")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoaiTK = new SelectList(db.LoaiTKs, "id", "name", taiKhoan.LoaiTK);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Chucvu", taiKhoan.MaNV);
            return View(taiKhoan);
        }

        // GET: TaiKhoan/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: TaiKhoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            db.TaiKhoans.Remove(taiKhoan);
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
