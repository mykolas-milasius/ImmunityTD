a<?php
// This is a simple PHP script that would handle user registration.

// Database configuration
$host = "localhost";
$dbName = "immunitytd"; // Your database name
$dbUser = "root"; // Your database username
$dbPass = ""; // Your database password

// Connect to the database
$pdo = new PDO("mysql:host=$host;dbname=$dbName", $dbUser, $dbPass);

// Check if the data from the Unity form is set
if (isset($_POST['username']) && isset($_POST['password'])) {
    $username = trim($_POST['username']);
    $password = password_hash($_POST['password'], PASSWORD_DEFAULT); // Hash the password

    // Check if the username already exists in the database
    $stmt = $pdo->prepare("SELECT COUNT(*) FROM users WHERE username = ?");
    $stmt->execute([$username]);
    $count = $stmt->fetchColumn();

    if ($count > 0) {
        // Username already exists, return error response
        echo "Username already exists";
    } else {
        // Username does not exist, proceed with registration
        $stmt = $pdo->prepare("INSERT INTO users (username, password_hash) VALUES (?, ?)");
        try {
            $stmt->execute([$username, $password]);
            echo "User registered successfully";
        } catch (PDOException $e) {
            echo "Registration error: " . $e->getMessage();
        }
    }
} else {
    echo "Username and password required";
}
?>


// SQL Dump commands for table creation and constraints
$sqlDump = "
-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 03, 2024 at 08:23 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = \"NO_AUTO_VALUE_ON_ZERO\";
START TRANSACTION;
SET time_zone = \"+00:00\";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

-- Database: `immunitytd`

-- Table structure for table `top10highscores`

CREATE TABLE IF NOT EXISTS `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(255) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;


CREATE TABLE IF NOT EXISTS `top10highscores` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `score` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `top10highscores_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;


-- Indexes for table `top10highscores`


-- Indexes for table `users`

-- AUTO_INCREMENT for dumped tables

-- AUTO_INCREMENT for table `top10highscores`



-- AUTO_INCREMENT for table `users`



-- Constraints for table `top10highscores`

ALTER TABLE `top10highscores`
  ADD CONSTRAINT `top10highscores_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
";

// Execute SQL dump commands
?>
