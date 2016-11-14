USE UltraSoundProtocolsDB;

CREATE TABLE
	Tbl_Doctors
    (
		dct_id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
		dct_firstname VARCHAR(255) NOT NULL,
		dct_middlename VARCHAR(255) NULL,
		dct_lastname VARCHAR(255) NOT NULL,
		dct_status BIT NOT NULL DEFAULT 1
	);

CREATE TABLE
	Tbl_Gender
	(
		gnd_id INT NOT NULL IDENTITY(0, 1) PRIMARY KEY,
		gnd_value VARCHAR(40) NOT NULL
	);

CREATE TABLE
	Tbl_Patients
	(
		pat_id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
		pat_firstname VARCHAR(255) NOT NULL,
		pat_middlename VARCHAR(255) NULL,
		pat_lastname VARCHAR(255) NOT NULL,
		pat_gender INT NOT NULL,
		pat_birthdate DATE NOT NULL,
		pat_numberambulatorycard VARCHAR(255) NULL
	);

CREATE TABLE
	Tbl_MedicalEquipments
	(
		meq_id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
		meq_name VARCHAR(255) NOT NULL
	);

CREATE TABLE
	Tbl_ExaminationTypes
	(
		ext_id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
		ext_name VARCHAR(255) NOT NULL
	);

CREATE TABLE
	Tbl_Protocols
	(
		prt_id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
		prt_datetime DATETIME NOT NULL,
		prt_doctor INT NOT NULL,
		prt_patient INT NOT NULL,
		prt_equipment INT NOT NULL,
		prt_source VARCHAR(255) NULL
	);

CREATE TABLE
	Tbl_Source
	(
		src_id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
		src_value VARCHAR(255) NOT NULL
	);
