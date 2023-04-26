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
    public class ChiTietPhieuMuonController : Controller
    {
        private ThuVienEntities db = new ThuVienEntities();
        private int MaPM = 0;

        // GET: ChiTietPhieuMuon
        public ActionResult Index(int id)
        {
            this.MaPM =   id;
            Session["PhieuMuon"] = id;
            if (MaPM != 0)
            {
                var chiTietPhieuMuons = db.ChiTietPhieuMuons.Where(c => c.MaPM == MaPM).Include(c => c.PhieuMuon).Include(c => c.Sach);
                return View(chiTietPhieuMuons.ToList());
            }
            // Sau nay chinh thanh view 404 not found
            return View();
        }

        // GET: ChiTietPhieuMuon/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuMuon chiTietPhieuMuon = db.ChiTietPhieuMuons.Find(id);
            if (chiTietPhieuMuon == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuMuon);
        }

        // GET: ChiTietPhieuMuon/Create
        public ActionResult Create(int id)
        {
            MaPM = id;
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach");
            Session["PhieuMuon"] = MaPM;
            return View();
        }

        // POST: ChiTietPhieuMuon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaCT,SoLuong,MaSach")] ChiTietPhieuMuon chiTietPhieuMuon)
        {
            chiTietPhieuMuon.MaPM = int.Parse(Session["PhieuMuon"].ToString());
            PhieuMuon phm = db.PhieuMuons.Where(x => x.MaPM == chiTietPhieuMuon.MaPM).FirstOrDefault();
            DocGia dg = db.DocGias.Where(x => x.MaDG == phm.MaDG).FirstOrDefault();

            // Loai doc gia 0.1.2 Normal.Pre.Pro
            int typeAcc = dg.LoaiDG;

            List<PhieuMuon> dangmuon = db.PhieuMuons.Where(x => x.TrangThai == 0 && x.MaDG == dg.MaDG).Include(x => x.ChiTietPhieuMuons).ToList();
            int solu = 0;
            foreach (PhieuMuon pm in dangmuon)
            {
                foreach (ChiTietPhieuMuon ct in pm.ChiTietPhieuMuons)
                {
                    solu += ct.SoLuong;
                }
            }


            Sach sach = db.Saches.Where(x => x.MaSach == chiTietPhieuMuon.MaSach).FirstOrDefault();
            int sl = 0;
            if (sach == null)
            {
                sl = 0;
            } else
            {
                sl = sach.SoLuongTT;
            }
            if (chiTietPhieuMuon.SoLuong > sl) {
                //So luong muon nhieu hon so luong co

                
                ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", chiTietPhieuMuon.MaSach);
                return View(chiTietPhieuMuon);
            }


            if (solu + chiTietPhieuMuon.SoLuong > (dg.LoaiDG * 2 + 2))
            {
                // So luong lon hon so luong dc muon
                ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", chiTietPhieuMuon.MaSach);
                return View(chiTietPhieuMuon);
            }
            if (ModelState.IsValid)
            {
                db.ChiTietPhieuMuons.Add(chiTietPhieuMuon);
                db.SaveChanges();

                sach.SoLuongTT = sach.SoLuongTT - chiTietPhieuMuon.SoLuong;
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();
                

                return RedirectToAction("Index","ChiTietPhieuMuon", new { id = chiTietPhieuMuon.MaPM});
            }

         
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", chiTietPhieuMuon.MaSach);
            return View(chiTietPhieuMuon);
        }

        // GET: ChiTietPhieuMuon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuMuon chiTietPhieuMuon = db.ChiTietPhieuMuons.Find(id);
            if (chiTietPhieuMuon == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPM = new SelectList(db.PhieuMuons, "MaPM", "MaNV", chiTietPhieuMuon.MaPM);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", chiTietPhieuMuon.MaSach);
            return View(chiTietPhieuMuon);
        }

        // POST: ChiTietPhieuMuon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaCT,MaPM,SoLuong,MaSach")] ChiTietPhieuMuon chiTietPhieuMuon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietPhieuMuon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaPM = new SelectList(db.PhieuMuons, "MaPM", "MaNV", chiTietPhieuMuon.MaPM);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", chiTietPhieuMuon.MaSach);
            return View(chiTietPhieuMuon);
        }

        // GET: ChiTietPhieuMuon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuMuon chiTietPhieuMuon = db.ChiTietPhieuMuons.Find(id);
            if (chiTietPhieuMuon == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuMuon);
        }

        // POST: ChiTietPhieuMuon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietPhieuMuon chiTietPhieuMuon = db.ChiTietPhieuMuons.Find(id);
            db.ChiTietPhieuMuons.Remove(chiTietPhieuMuon);
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
