const express = require("express");
const router = express.Router();

const productsController = require("./controllers/products");

router.get("/categories", productsController.listAllCategories);
router.get("/products", productsController.listProducts);

module.exports = router;