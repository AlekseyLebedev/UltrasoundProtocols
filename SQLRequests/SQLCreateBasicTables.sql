USE UltraSoundProtocolsDB;

CREATE TABLE
	Tbl_Doctors
    (
		dct_id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
		dct_firstname NVARCHAR(255) NOT NULL,
		dct_middlename NVARCHAR(255) NULL,
		dct_lastname NVARCHAR(255) NOT NULL,
		dct_status BIT NOT NULL DEFAULT 1
	);

CREATE TABLE
	Tbl_Patients
	(
		pat_id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
		pat_firstname NVARCHAR(255) NOT NULL,
		pat_middlename NVARCHAR(255) NULL,
		pat_lastname NVARCHAR(255) NOT NULL,
		pat_gender INT NOT NULL,
		pat_birthdate DATE NOT NULL,
		pat_numberambulatorycard NVARCHAR(255) NULL
	);

CREATE TABLE
	Tbl_MedicalEquipments
	(
		meq_id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
		meq_name NVARCHAR(255) NOT NULL
	);

CREATE TABLE
	Tbl_Templates
	(
		tem_id NVARCHAR(255) NOT NULL PRIMARY KEY,
		tem_name NVARCHAR(255) NOT NULL,
		tem_template NVARCHAR(max) NOT NULL
	);

CREATE TABLE
	Tbl_Protocols
	(
		prt_id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
		prt_datetime DATETIME NOT NULL,
		prt_doctor INT NOT NULL,
		prt_patient INT NOT NULL,
		prt_equipment INT NOT NULL,
		prt_source NVARCHAR(255) NULL
	);

CREATE TABLE
	Tbl_Source
	(
		src_id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
		src_value NVARCHAR(255) NOT NULL
	);
