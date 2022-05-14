const express = require("express");
const router = express.Router();

const productsController = require("./controllers/products");

router.get("/categories", productsController.listAllCategories);
router.get("/products", productsController.listProducts);
router.get("/model", productsController.getModelData);

module.exports = router;