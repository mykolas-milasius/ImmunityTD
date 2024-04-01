<?php
// This is a simple PHP script that would handle user login.

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
    $password = $_POST['password']; // Get the password

    // Check if the username exists in the database
    $stmt = $pdo->prepare("SELECT password_hash FROM users WHERE username = ?");
    $stmt->execute([$username]);
    $row = $stmt->fetch(PDO::FETCH_ASSOC);

    if ($row) {
        // Username exists, check the password
        if (password_verify($password, $row['password_hash'])) {
            // Password is correct, login successful
            echo "Login successful";
        } else {
            // Password is incorrect, return error response
            echo "Invalid username or password";
        }
    } else {
        // Username does not exist, return error response
        echo "Invalid username or password";
    }
} else {
    echo "Username and password required";
}
?>