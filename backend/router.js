const express = require("express");
const router = express.Router();

const productsController = require("./controllers/products");

router.get("/categories", productsController.listAllCategories);

module.exports = router;