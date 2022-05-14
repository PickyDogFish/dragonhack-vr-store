require("dotenv").config();

const express = require("express");
const router = require("./router");
const app = express();
const PORT = process.env.PORT;

const apiRouter = require("./router");

// Logging
app.use((req, res, next) => {
    console.log(req.method + " " + req.url);
    next();
});

// Routing
app.use("/api", router);
app.get("/", (req, res) => {
    res.send("yourmom");
});

// Start the server
app.listen(PORT, () => {
    console.log("API running on port " + PORT);
});