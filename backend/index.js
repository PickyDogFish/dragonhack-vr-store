require("dotenv").config();

const express = require("express");
const router = require("./router");
const path = require("path");
const app = express();
const PORT = process.env.PORT;

// Logging
app.use((req, res, next) => {
    console.log(req.method + " " + req.url);
    next();
});

// Routing
app.use("/api", router);
app.get("/", (req, res) => {
    res.sendFile(path.join(__dirname, 'public', 'index.html'));
});
app.use(express.static(path.join(__dirname, 'public')));

// Start the server
app.listen(PORT, () => {
    console.log("API running on port " + PORT);
});