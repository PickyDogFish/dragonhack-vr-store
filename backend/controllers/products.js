const con = require("../dbconnector");

const listAllCategories = async (req, res) => {
    con.query("SELECT * FROM Categories;")
        .then(([rows, fields]) => res.status(200).json(rows))
        .catch((err) => {
            console.warn("Error in querying all categories:", err);
            res.status(500).json();
        });
};

const listProducts = (req, res) => {
    if (!req.query.category) {
        res.status(400).json();
        return;
    }
    con.query("SELECT * FROM Products WHERE Category=" + req.query.category)
        .then(([rows, columns]) => res.status(200).json(rows))
        .catch((err) => {
            console.warn("Error in querying products:", err);
            res.status(500).json();
        });
}

const getModelData = (req, res) => {
    if (!req.query.id) {
        res.status(400).json();
        return;
    }
    con.query("SELECT * FROM Models WHERE id=" + req.query.id)
        .then(([rows, fields]) => res.status(200).json(rows))
        .catch((err) => {
            console.warn("Error in querying model:", err);
            res.status(500).json();
        });
}

module.exports = {
    listAllCategories,
    listProducts,
    getModelData
}