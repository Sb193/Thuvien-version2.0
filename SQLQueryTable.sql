CREATE TABLE Nguoi(
	MaNguoi				INT IDENTITY					Primary Key,
	HoTen 				nvarchar(50)				not NULL,
	Gioitinh			nvarchar(10)				not Null,
	NgaySinh			date						not NULL,
	DiaChi				nvarchar(100)				not NULL,
	Sdt					char(10)					NULL,
)



CREATE TABLE LoaiTK(
	id int primary key,
	name nvarchar(20)
)
GO
INSERT INTO LoaiTK(
	id,
	name
) VALUES (
	0,
	N'Admin'
),(
	1,
	N'Nhân viên'
)

CREATE TABLE TaiKhoan (
	TaiKhoan	varchar (50)	Primary Key,
	MatKhau		varchar(50)		not NULL,
	LoaiTK		int				Default((0)),
	MaNV		varchar(10),
	Constraint LoaiTK_FK Foreign Key(LoaiTK) References LoaiTK(id),
	Constraint NhanVien_FK Foreign Key(MaNV) References NhanVien(MaNV)
)

CREATE TABLE NhanVien (
	MaNV		varchar(10)				Primary Key,
	Chucvu		varchar(50)		,
	MaNguoi		int				,
	Constraint MaNguoi1_FK Foreign Key(MaNguoi) References Nguoi(MaNguoi)
)

CREATE TABLE LoaiDG(
	id int primary key,
	name nvarchar(20)
)
GO
INSERT INTO LoaiDG(
	id,
	name
) VALUES (
	0,
	N'Normal'
),(
	1,
	N'Premium'
),(
	2,
	N'Pro'
)

GO


CREATE TABLE DocGia (
	MaDG			 INT IDENTITY				Primary Key,
	NgheNghiep		 nvarchar(10)			not NULL,
	LoaiDG			 int			not NULL,
	MaNguoi			 int				,
	Constraint MaNguoi_FK2 Foreign Key(MaNguoi) References Nguoi(MaNguoi),
	Constraint LoaiDG_FK Foreign KEY (LoaiDG) References LoaiDG(id)
)

CREATE TABLE TheLoai(
	MaTL		INT IDENTITY			Primary Key,
	TenTL		nvarchar(50)		not NULL,
)

CREATE TABLE Sach (
	MaSach		INT IDENTITY		Primary Key,
	TenSach		nvarchar(50)	not NULL,
	TenTG		nvarchar(50)	not NULL,
	SoLuong		int				not NULL,
	SoLuongTT	int				not NULL,
	MaTL		int		not NULL,
	Constraint MaTL_FK Foreign Key(MaTL) References TheLoai(MaTL)
)


CREATE TABLE TheThuVien (
	MaDG		int			,
	ThoiHan		date				not NULL,
	Constraint MaDG_PK Primary Key (MaDG),
	Constraint MaDG_FK Foreign Key(MaDG) References DocGia(MaDG)
)

CREATE TABLE LuaChon(
	id int primary key,
	name nvarchar(20)
)

CREATE TABLE TrangThai(
	id int primary key,
	name nvarchar(20)
)

GO
INSERT INTO LuaChon(
	id ,
	name
) VALUES (
	0,
	N'Mượn đọc tại chỗ'
),(
	1,
	N'Mượn mang về'
)

INSERT INTO TrangThai(
	id ,
	name
) VALUES (
	0,
	N'Đang mượn'
),(
	1,
	N'Đã hoàn thành'
) , (
	2,
	N'Quá hạn'
)

CREATE TABLE PhieuMuon(
	MaPM		INT IDENTITY		Primary Key,
	MaDG		int	,
	MaNV		varchar(10)		,
	NgayMuon	date			not NULL,
	NgayTra		date			not NULL,
	LuaChon		int			    not NULL,
	TrangThai	int				not NULL,
	Constraint MaDG_FK1 Foreign Key(MaDG) References TheThuVien(MaDG),
	Constraint MaNV_FK Foreign Key(MaNV) References NhanVien(MaNV),
	Constraint Luachon_FK Foreign Key(LuaChon) References LuaChon(id),
	Constraint Trangthai_FK Foreign Key(TrangThai) References TrangThai(id)
)

CREATE TABLE ChiTietPhieuMuon (
	MaCT		INT IDENTITY		PRIMARY KEY,
	MaPM		int		,
	SoLuong		int				not NULL,
	MaSach		int		,
	Constraint MaPM_FK Foreign Key(MaPM) References PhieuMuon(MaPM),
	Constraint MaSach_FK Foreign Key(MaSach) References Sach(MaSach)
)

CREATE TABLE PhieuPhat(
	MaPP		INT IDENTITY				Primary Key,
	MaPM		int				,
	LyDo		nvarchar(150)			not NULL,
	Constraint MaPM_FK1 Foreign Key(MaPM) References PhieuMuon(MaPM)
)