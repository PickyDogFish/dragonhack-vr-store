const mysql = require("mysql2");

const connector = mysql.createConnection({
    host: "localhost",
    user: "admin",
    password: "admintestpass",
    database: "vrstore"
});

connector.connect((err) => {
    if (err)
        throw err;
    console.log("Database connection established.");
});

module.exports = connector.promise();