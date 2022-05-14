const con = require("../dbconnector");

const listAllCategories = async (req, res) => {
    console.log("listing all categories");
    con.query("SELECT * FROM Categories;")
        .then(([rows, fields]) => res.status(200).json(rows))
        .catch((err) => res.status(500).json());
};

const listProducts = (req, res) => {
    if (!req.query.category){
        res.status(400).json();
        return;
    }
}

module.exports = {
    listAllCategories
}