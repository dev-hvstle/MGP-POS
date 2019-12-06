-- phpMyAdmin SQL Dump
-- version 4.9.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 06, 2019 at 09:40 AM
-- Server version: 10.4.8-MariaDB
-- PHP Version: 7.3.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `db_mgppos`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_contactnumber`
--

CREATE TABLE `tbl_contactnumber` (
  `supplierID` int(11) NOT NULL,
  `contactNumber` bigint(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_contactnumber`
--

INSERT INTO `tbl_contactnumber` (`supplierID`, `contactNumber`) VALUES
(11, 9284458725),
(12, 9284458707),
(12, 9284458725);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_products`
--

CREATE TABLE `tbl_products` (
  `PID` int(11) NOT NULL,
  `PName` varchar(100) NOT NULL,
  `PPrice` double(20,2) NOT NULL,
  `supplierID` int(11) NOT NULL,
  `modifiedBy` int(11) NOT NULL,
  `dateModified` date NOT NULL,
  `timeModified` varchar(20) DEFAULT NULL,
  `isPriority` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_products`
--

INSERT INTO `tbl_products` (`PID`, `PName`, `PPrice`, `supplierID`, `modifiedBy`, `dateModified`, `timeModified`, `isPriority`) VALUES
(7, 'Plastic', 20.00, 5, 1, '2019-11-29', '12:00 am', 1),
(8, 'Bag', 30.00, 5, 1, '2019-11-29', '3:00 am', 1),
(9, 'Chocolate', 150.00, 5, 1, '2019-12-04', '3:01 am', 1),
(10, 'Metal', 50.00, 6, 1, '2019-12-05', '3:05 am', 0),
(11, 'BOSSP', 1000.00, 7, 1, '2019-12-06', '03:34 pm', 1);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_supplier`
--

CREATE TABLE `tbl_supplier` (
  `supplierID` int(11) NOT NULL,
  `supplierName` varchar(50) NOT NULL,
  `locationCity` varchar(30) NOT NULL,
  `modifiedBy` int(11) DEFAULT NULL,
  `dateModified` date NOT NULL,
  `timeModified` varchar(20) NOT NULL,
  `isPriority` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_supplier`
--

INSERT INTO `tbl_supplier` (`supplierID`, `supplierName`, `locationCity`, `modifiedBy`, `dateModified`, `timeModified`, `isPriority`) VALUES
(5, 'Mix Plant', 'Makati City', 1, '2019-11-29', '', 0),
(6, 'King Plant', 'Valenzuela', 2, '2019-12-05', '', 0),
(7, 'Lala Move', 'Makati', 1, '2019-12-06', '01:58 am', 1),
(8, 'Grab Food', 'Mandaluyong', 1, '2019-12-06', '02:04 am', 1),
(9, 'Lazada', 'Manila', 1, '2019-12-06', '02:08 am', 1),
(10, 'Lazada', 'Makati', 1, '2019-12-06', '02:10 am', 1),
(11, 'Shopee', 'Bulacan', 1, '2019-12-06', '09:27 am', 1),
(12, 'Zaful', 'Caloocan', 1, '2019-12-06', '03:05 pm', 1);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_transactiondetails`
--

CREATE TABLE `tbl_transactiondetails` (
  `transactionID` int(11) NOT NULL,
  `PID` int(11) NOT NULL,
  `productQuantity` int(11) NOT NULL,
  `productSubtotal` double(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_transactiondetails`
--

INSERT INTO `tbl_transactiondetails` (`transactionID`, `PID`, `productQuantity`, `productSubtotal`) VALUES
(12, 7, 1, 20.00),
(13, 8, 1, 30.00),
(14, 8, 10, 300.00),
(15, 8, 5, 150.00),
(20, 8, 4, 120.00),
(25, 7, 1, 20.00),
(26, 7, 1, 20.00),
(27, 7, 1, 20.00),
(27, 8, 1, 30.00),
(31, 7, 1, 20.00),
(31, 8, 1, 30.00),
(32, 8, 10, 300.00),
(32, 7, 10, 200.00),
(33, 7, 10, 200.00),
(33, 8, 10, 300.00),
(34, 9, 2, 300.00),
(35, 11, 1, 1000.00),
(36, 7, 1, 20.00),
(36, 8, 50, 1500.00),
(37, 7, 10, 200.00),
(37, 8, 20, 600.00),
(38, 7, 10, 200.00),
(38, 9, 5, 750.00),
(38, 10, 7, 350.00),
(39, 7, 10, 200.00),
(39, 8, 30, 900.00);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_transactionmaster`
--

CREATE TABLE `tbl_transactionmaster` (
  `transactionID` int(11) NOT NULL,
  `customerName` varchar(50) DEFAULT NULL,
  `userID` int(11) NOT NULL,
  `totalItems` int(11) NOT NULL,
  `totalAmount` double(10,2) NOT NULL,
  `paymentAmount` double(10,2) NOT NULL,
  `paymentChange` double(10,2) NOT NULL,
  `transactionDate` date NOT NULL,
  `transactionTime` varchar(20) NOT NULL,
  `isPriority` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_transactionmaster`
--

INSERT INTO `tbl_transactionmaster` (`transactionID`, `customerName`, `userID`, `totalItems`, `totalAmount`, `paymentAmount`, `paymentChange`, `transactionDate`, `transactionTime`, `isPriority`) VALUES
(1, '', 1, 10, 250.00, 300.00, 50.00, '2019-11-30', '', 0),
(2, '', 1, 4, 100.00, 100.00, 0.00, '2019-11-30', '', 0),
(3, '', 1, 6, 140.00, 150.00, 10.00, '2019-11-30', '', 0),
(4, '', 1, 2, 50.00, 100.00, 50.00, '2019-11-30', '', 0),
(5, '', 1, 2, 50.00, 100.00, 50.00, '2019-11-30', '', 0),
(6, '', 1, 4, 100.00, 101.00, 1.00, '2019-11-30', '', 0),
(7, '', 1, 2, 50.00, 111.00, 61.00, '2019-11-30', '', 0),
(8, '', 1, 2, 50.00, 111.00, 61.00, '2019-11-30', '', 0),
(9, '', 1, 2, 50.00, 111.00, 61.00, '2019-11-30', '', 0),
(10, '', 1, 2, 50.00, 222.00, 172.00, '2019-11-30', '', 0),
(11, '', 1, 2, 50.00, 70.00, 20.00, '2019-11-30', '', 0),
(12, '', 1, 10, 250.00, 300.00, 50.00, '2019-11-30', '', 0),
(13, '', 1, 2, 50.00, 111.00, 61.00, '2019-11-30', '', 0),
(14, '', 1, 2, 50.00, 60.00, 10.00, '2019-11-30', '', 0),
(15, '', 1, 20, 500.00, 1000.00, 500.00, '2019-11-30', '', 0),
(16, '', 1, 10, 250.00, 300.00, 50.00, '2019-11-30', '', 0),
(17, '', 1, 6, 150.00, 200.00, 50.00, '2019-11-30', '', 0),
(18, '', 1, 6, 150.00, 200.00, 50.00, '2019-11-30', '', 0),
(19, '', 1, 4, 100.00, 101.00, 1.00, '2019-11-30', '', 0),
(20, '', 1, 4, 100.00, 101.00, 1.00, '2019-11-30', '', 0),
(21, '', 1, 8, 200.00, 300.00, 100.00, '2019-11-30', '', 0),
(22, '', 1, 2, 50.00, 66.00, 16.00, '2019-11-30', '', 0),
(23, '', 1, 2, 50.00, 66.00, 16.00, '2019-11-30', '', 0),
(24, '', 1, 2, 50.00, 55.00, 5.00, '2019-11-30', '', 0),
(25, '', 1, 2, 50.00, 66.00, 16.00, '2019-11-30', '', 0),
(26, '', 1, 3, 80.00, 88.00, 8.00, '2019-11-30', '', 0),
(27, '', 1, 2, 50.00, 66.00, 16.00, '2019-11-30', '', 0),
(28, '', 1, 2, 50.00, 55.00, 5.00, '2019-11-30', '', 0),
(29, '', 1, 2, 50.00, 66.00, 16.00, '2019-11-30', '', 0),
(30, '', 1, 2, 50.00, 66.00, 16.00, '2019-11-30', '', 0),
(31, '', 1, 2, 50.00, 66.00, 16.00, '2019-11-30', '', 0),
(32, '', 1, 20, 500.00, 600.00, 100.00, '2019-12-01', '', 0),
(33, '', 1, 20, 500.00, 600.00, 100.00, '2019-12-04', '', 0),
(34, '', 1, 2, 300.00, 350.00, 50.00, '2019-12-04', '3:00 am', 0),
(35, '', 1, 1, 1000.00, 1000.00, 0.00, '2019-12-05', '12:00 am', 0),
(36, '', 6, 51, 1520.00, 1600.00, 80.00, '2019-12-05', '1:16 pm', 1),
(37, '', 1, 30, 800.00, 800.00, 0.00, '2019-12-05', '1:20 pm', 1),
(38, '', 1, 22, 1300.00, 1500.00, 200.00, '2019-12-06', '01:46 pm', 1),
(39, 'Justine', 1, 40, 1100.00, 1200.00, 100.00, '2019-12-06', '02:59 pm', 1);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_users`
--

CREATE TABLE `tbl_users` (
  `userID` int(11) NOT NULL,
  `userType` varchar(20) NOT NULL,
  `userName` varchar(20) NOT NULL,
  `userPassword` varchar(64) DEFAULT NULL,
  `userFirstName` varchar(30) NOT NULL,
  `userMiddleName` varchar(20) DEFAULT NULL,
  `userLastName` varchar(20) NOT NULL,
  `dateModified` date DEFAULT NULL,
  `timeModified` varchar(20) DEFAULT NULL,
  `isPriority` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_users`
--

INSERT INTO `tbl_users` (`userID`, `userType`, `userName`, `userPassword`, `userFirstName`, `userMiddleName`, `userLastName`, `dateModified`, `timeModified`, `isPriority`) VALUES
(1, 'SuperAdmin', 'adminvey', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Vey', 'Sanchez', 'Resurreccion', '2019-11-26', '6:57 pm', 1),
(2, 'SuperAdmin', 'adminjustine', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 'Justine', '', 'Asejo', '2019-11-26', '12:00 am', 1),
(3, 'Admin', 'mgp', '7160f8688035138fcbc9a6c8041949ebea0c0d21a1b1d063f839f38d5c2be8f9', 'Maria Gracia', '', 'Paguia', '2019-12-02', '3:00 am', 1),
(4, 'Parametric', 'theone', '16367aacb67a4a017c8da8ab95682ccb390863780f7114dda0a0e0c55644c7c4', 'Cha', '', 'Resurreccion', '2019-12-02', '1:00 am', 1),
(5, 'Parametric', 'ggwp', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 'Justin', '', 'Garcia', '2019-12-04', '12:00 pm', 1),
(6, 'Admin', 'adminmira', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 'Almira', '', 'Domingo', '2019-12-05', '03:11 pm', 1),
(7, 'Admin', 'adminespesor', '756bc47cb5215dc3329ca7e1f7be33a2dad68990bb94b76d90aa07f4e44a233a', 'Eliza', 'Santos', 'Espesor', '2019-12-06', '09:48 am', 1);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_usertype`
--

CREATE TABLE `tbl_usertype` (
  `typeName` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_usertype`
--

INSERT INTO `tbl_usertype` (`typeName`) VALUES
('Admin'),
('Parametric'),
('SuperAdmin');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_contactnumber`
--
ALTER TABLE `tbl_contactnumber`
  ADD KEY `supplierID` (`supplierID`);

--
-- Indexes for table `tbl_products`
--
ALTER TABLE `tbl_products`
  ADD PRIMARY KEY (`PID`),
  ADD KEY `supplierID` (`supplierID`),
  ADD KEY `modifiedBy` (`modifiedBy`);

--
-- Indexes for table `tbl_supplier`
--
ALTER TABLE `tbl_supplier`
  ADD PRIMARY KEY (`supplierID`);

--
-- Indexes for table `tbl_transactiondetails`
--
ALTER TABLE `tbl_transactiondetails`
  ADD KEY `transactionID` (`transactionID`),
  ADD KEY `PID` (`PID`);

--
-- Indexes for table `tbl_transactionmaster`
--
ALTER TABLE `tbl_transactionmaster`
  ADD PRIMARY KEY (`transactionID`);

--
-- Indexes for table `tbl_users`
--
ALTER TABLE `tbl_users`
  ADD PRIMARY KEY (`userID`),
  ADD KEY `userType` (`userType`);

--
-- Indexes for table `tbl_usertype`
--
ALTER TABLE `tbl_usertype`
  ADD PRIMARY KEY (`typeName`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_products`
--
ALTER TABLE `tbl_products`
  MODIFY `PID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `tbl_supplier`
--
ALTER TABLE `tbl_supplier`
  MODIFY `supplierID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `tbl_transactionmaster`
--
ALTER TABLE `tbl_transactionmaster`
  MODIFY `transactionID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=40;

--
-- AUTO_INCREMENT for table `tbl_users`
--
ALTER TABLE `tbl_users`
  MODIFY `userID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `tbl_contactnumber`
--
ALTER TABLE `tbl_contactnumber`
  ADD CONSTRAINT `tbl_contactnumber_ibfk_1` FOREIGN KEY (`supplierID`) REFERENCES `tbl_supplier` (`supplierID`);

--
-- Constraints for table `tbl_products`
--
ALTER TABLE `tbl_products`
  ADD CONSTRAINT `tbl_products_ibfk_1` FOREIGN KEY (`supplierID`) REFERENCES `tbl_supplier` (`supplierID`),
  ADD CONSTRAINT `tbl_products_ibfk_2` FOREIGN KEY (`supplierID`) REFERENCES `tbl_supplier` (`supplierID`),
  ADD CONSTRAINT `tbl_products_ibfk_3` FOREIGN KEY (`modifiedBy`) REFERENCES `tbl_users` (`userID`);

--
-- Constraints for table `tbl_transactiondetails`
--
ALTER TABLE `tbl_transactiondetails`
  ADD CONSTRAINT `tbl_transactiondetails_ibfk_1` FOREIGN KEY (`transactionID`) REFERENCES `tbl_transactionmaster` (`transactionID`),
  ADD CONSTRAINT `tbl_transactiondetails_ibfk_2` FOREIGN KEY (`PID`) REFERENCES `tbl_products` (`PID`);

--
-- Constraints for table `tbl_users`
--
ALTER TABLE `tbl_users`
  ADD CONSTRAINT `tbl_users_ibfk_1` FOREIGN KEY (`userType`) REFERENCES `tbl_usertype` (`typeName`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
